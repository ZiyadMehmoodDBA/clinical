var PatientRaces = [];
Patient_Demographic = {
    params: [],
    bIsFirstLoad: true,
    isChange: false,
    multipleEthnicityIds: '',
    ScanPrivilige: false,
    OCRPrivilige: false,
    isFinanicialAlert: true,
    imagedata: '',
    filetype: '',
    filename: '',
    foldername: '',
    PatDocId: null,
    CommunicateWithGurrantor: '',
    ProviderFName: '',
    ProviderLName: '',
    FacilityAddress: '',
    FacilityCity: '',
    FacilityState: '',
    FacilityZip: '',
    FacilityZipExt: '',
    FacilityPhone: '',
    FacilityLabel: '',
    ProviderLabel: '',
    PatientSSN: '',
    checkedEthnicityNodes: [],
    IsImageUpdated: '',
    isDocExpiryAlert: true,
    careGiverIds: '',
    ticker: {},
    EnrollmentInfoId: 0,
    Load: function (parameters) {

        Patient_Demographic.params = parameters;
        $("#pnlDemographic #divDemographicPicture #imgPatient").attr("src", "Content/images/default_male_profile.gif");
        $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_male_profile.gif");

        if (Patient_Demographic.bIsFirstLoad) {
            Patient_Demographic.bIsFirstLoad = false;
            //Patient_Demographic.isFinanicialAlert = true;


            var self = $('#pnlDemographic #frmDemographic');
            $.when(Patient_Demographic.LoadingDropDowns('pnlDemographic'), Patient_Demographic.LoadCareGiverDropDowns('pnlDemographic'), self.loadDropDowns(true)).then(function () {
                Patient_Demographic.ValidateDemographic();

                //------------------------------------
                if (!$('#' + Patient_Demographic.params["PanelID"] + ' #ddlPatientRace').data("kendoMultiSelect"))
                    utility.InitKendoRaceAutoComplete(Patient_Demographic.params["PanelID"] + ' #ddlPatientRace', Patient_Demographic.params["PanelID"] + ' #hfRaceIds');
                Patient_Demographic.IntializeMultiSelectDropDownEthnicity();
                Patient_Demographic.multipleEthnicityIds = '';
                Patient_Demographic.checkedEthnicityNodes = [];
                //var value = "- Select -";
                //$('.multiselect-container').find("li").eq(2).remove();
                //------------------------------------
                Patient_Demographic.IntializeMultiSelectDropDownCareGiver();
                $.when(Patient_Demographic.LoadPatientDemogrphic()).then(function () {
                    Patient_Demographic.LoadAllAutocomplete();
                });

            });

        } else {
            if (Patient_Demographic.params.SelectedPatientFormName == 'DayCalendar') {
                //Patient_Demographic.isFinanicialAlert = false;
                //  Patient_Demographic.GetAccountManagerPriviliges();
                delete Patient_Demographic.params.SelectedPatientFormName;
            }
            else {
                Patient_Demographic.LoadPatientDemogrphic();
            }
        }

        utility.CreateDatePicker('pnlDemographic #frmDemographic #dtpDischargeDate', function () {

        });
        if (params.PreviousTab != null && params.PreviousTab.TabID == "patTabInsurance") {
            Patient_Insurance.isChangeInInsurance(Patient_Insurance.params.mode);
        }
        $("#divDateOfDeath").addClass('hidden');
        if (globalAppdata["isDemographics"] && globalAppdata["isDemographics"].toLowerCase() == "false") {
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divSexualOrientationDdl").addClass("hidden");
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divGenderIdentityDdl").addClass("hidden");
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divBirthSexDdl").addClass("hidden");
        }
        else {
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divSexualOrientationDdl").removeClass("hidden");
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divGenderIdentityDdl").removeClass("hidden");
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divBirthSexDdl").removeClass("hidden");
        }
        if (globalAppdata["isTransPubHealthAgHealthCareSurveys"] && globalAppdata["isTransPubHealthAgHealthCareSurveys"].toLowerCase() == "false") {
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divPreferredPhone").addClass("hidden");
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divCountry").addClass("hidden");
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divPreferredAddress").addClass("hidden");
        }
        else {
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divPreferredPhone").removeClass("hidden");
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divCountry").removeClass("hidden");
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #divPreferredAddress").removeClass("hidden");
        }
    },


    LoadingDropDowns: function (PanelId) {
        var methodName = ['GetEthnicityForDemographics'];
        var dfd = new $.Deferred();
        BackgroundLoaderShow(true);
        MDVisionService.lookups(methodName, true, "").done(function (results) {
            var htmlEthnicity = '';

            var ethnicities = JSON.parse(results['GetEthnicityForDemographics']);
            var ParentEthnicityArray = [];
            ParentEthnicityArray = jQuery.grep(ethnicities, function (value) {
                return value.ParentId == null || value.ParentId == 0 || value.ParentId == "0";
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
            $('#' + PanelId + ' #divPatientEthnicity #ddlEthnicity').html(htmlEthnicity);
            BackgroundLoaderShow(false);
            dfd.resolve();
        });

        return dfd;
    },

    LoadCareGiverDropDowns: function (PanelId) {
        var methodName = ['GetPatientCareGiver'];
        var dfd = new $.Deferred();
        BackgroundLoaderShow(true);
        var data = "IsActive=1&ID=" + Patient_Demographic.params.patientID;
        MDVisionService.lookups(methodName, true, data).done(function (results) {

            var htmlCareGiver = '';
            var ParentCareGiverArray = JSON.parse(results['GetPatientCareGiver']);
            $.each(ParentCareGiverArray, function (j, result) {
                htmlCareGiver += '<option value="' + result.Id + '">' + result.Name + '</option>';
            });
            $('#' + PanelId + ' #divPatientCareGiver #ddlCareGiver').html(htmlCareGiver);

            BackgroundLoaderShow(false);
            dfd.resolve();
        });

        return dfd;
    },
    removeElementFromArray: function (array, element) {
        var index = array.indexOf(element);
        if (index !== -1)
            array.splice(index, 1);
    },
    RemovePractice: function (ctrl) {
        try {
            var obj = $("#pnlDemographic #txtFacility");
            if (obj.val() != "" && $("#pnlDemographic #txtPractice").val() == "") {
                var sourceArr = $(obj).autocomplete("option", "source");
                var haveObject = sourceArr.filter(function (itm) {
                    return itm.value.toLowerCase() == $(obj).val().toLowerCase();
                });
                if (haveObject.length > 0) {
                    $("#pnlDemographic #txtPractice").val(haveObject[0].Practice);
                    $("#pnlDemographic #hfPractice").val(haveObject[0].PracticeId);
                }
                else {
                    $("#pnlDemographic #txtPractice").val("");
                    $("#pnlDemographic #hfPractice").val("");
                }
            }
            if ($(ctrl).val() == "") {
                $("#pnlDemographic #txtPractice").val("");
                $("#pnlDemographic #hfPractice").val("");
                ScanPrivilige = false;
                OCRPrivilige = false;
            }
        } catch (e) {

        }
    },
    IntializeMultiSelectDropDownEthnicity: function () {
        try {
            $('#' + Patient_Demographic.params.PanelID + ' #ddlEthnicity').multiselect('destroy');
            $('#' + Patient_Demographic.params.PanelID + ' #ddlEthnicity').multiselect({
                //includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: 247,
                onChange: function (element, checked) {
                    var self = $('#pnlDemographic');
                    var options = $(element).parent().find('option');
                    var Selectedoptions = $(element).parent().find('option:selected');
                    if (element.length > 0) {
                        var optionText = $(element)[0].outerText;
                        var optionVal = $($(element)[0]).val();
                        if (checked) {
                            if (optionText.trim().toLowerCase() == "declined to specify" || optionText.trim().toLowerCase() == "refused to report") {
                                Selectedoptions.each(function (i, itm) {
                                    if ($(itm).val() != optionVal)
                                        $(this).prop('selected', false);
                                });
                                $('#' + Patient_Demographic.params.PanelID + " #ddlEthnicity").multiselect('refresh');
                                options.each(function (e, item) {
                                    if ($(item).val() != optionVal) {
                                        var input = $('#pnlDemographic #divPatientEthnicity input[type=checkbox][value="' + $(this).val() + '"]');
                                        input.prop('disabled', true);
                                        input.parent('li').addClass('disabled');
                                    }
                                });
                            }
                        }
                        else {
                            if (optionText.trim().toLowerCase() == "declined to specify" || optionText.trim().toLowerCase() == "refused to report") {
                                options.each(function (e, item) {
                                    if ($(item).val() != optionVal) {
                                        var input = $('#pnlDemographic #divPatientEthnicity input[type=checkbox][value="' + $(this).val() + '"]');
                                        input.prop('disabled', false);
                                        input.parent('li').removeClass('disabled');
                                    }
                                });
                            }
                            else {
                                options.each(function () {
                                    var input = $('#pnlDemographic #divPatientEthnicity input[type=checkbox][value="' + $(this).val() + '"]');
                                    input.prop('disabled', false);
                                    input.parent('li').addClass('disabled');
                                });
                            }
                        }
                    }
                    $('#' + Patient_Demographic.params.PanelID + " #ddlEthnicity").multiselect('updateButtonText');
                    var EthnicityIds = self.find('#divPatientEthnicity ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                        return this.value;
                    }).get().join(',');
                    Patient_Demographic.multipleEthnicityIds = EthnicityIds;
                    if (Patient_Demographic.multipleEthnicityIds != '')
                        Patient_Demographic.validateEthnicity(2);
                    else
                        Patient_Demographic.validateEthnicity(1);
                }
            });
            $('#' + Patient_Demographic.params.PanelID + " #ddlEthnicity").val("");

            $('#pnlDemographic').find('#divPatientEthnicity ul.multiselect-container li input[type=checkbox]').each(function () {
                if ($(this).attr('refval') == "hidden") {
                    $(this).parent().addClass('text-bold');
                }
            });
        }
        catch (ex) {
            console.log(ex);
        }
    },

    LoadAllAutocomplete: function () {
        utility.CreateDatePicker('pnlDemographic #frmDemographic #dtpDOB',
        function (ev) {
            Patient_Demographic.CalculateAge($('#pnlDemographic #frmDemographic #dtpDOB'));
            if ($('#pnlDemographic #frmDemographic').data("bootstrapValidator") != null) {
                $('#pnlDemographic #frmDemographic').bootstrapValidator('revalidateField', 'DOB');
            }
            //on-change callback method
        }, false);

        $('#pnlDemographic #frmDemographic #dtpDOB').datepicker('setEndDate', new Date());



        utility.CreateDatePicker("pnlDemographic #frmDemographic #dtpDateOfDeath", function () {
        }, true);
        $('#pnlDemographic #frmDemographic #dtpDateOfDeath').datepicker('setEndDate', new Date());

        Patient_Demographic.BindProvider();
        Patient_Demographic.BindFacility();
        Patient_Demographic.BindRefProvider();
        Patient_Demographic.BindPCPProvider();
        Patient_Demographic.BindGuarantor();
        Patient_Demographic.BindRefProviderReferral();
        Patient_Demographic.BindRefProviderTo();
        Patient_Demographic.BindLanguages();
        Patient_Demographic.BindCountries();
        Patient_Demographic.OpenAssignee();
        Patient_Demographic.BindCity("#" + Patient_Demographic.params.PanelID + " #frmDemographic");
    },

    BindLanguages: function () {
        var Ctrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #ddlPrefLanguage");
        var func = function () { return utility.GetPreferedLanguagesArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfLanguages");
        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", null, func, hfCtrl);
    },
    BindCountries: function () {
        var Ctrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #ddlCountry");
        var func = function () { return utility.GetCountriesArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfCountry");
        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", null, func, hfCtrl);
    },

    BindFacility: function () {
        var Ctrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtFacility");
        var func = function () { return utility.GetFacilityDescriptionArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfFacility");
        var onSelect = function (e) {
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtPractice").val(e.Practice);
            $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfPractice").val(e.PracticeId);
        }
        var onChange = function () { Patient_Demographic.RemovePractice(Ctrl); };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },


    BindProvider: function (isFullName, shortName) {
        var Ctrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfProvider");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindCity: function (PanelId) {
        var valid = false;
        var Ctrl = $(PanelId + " #txtCity");
        var hfCtrl = $(PanelId + " #hfCity");
        var onChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var data = this.dataSource.data();
            var haveObject = data.filter(function (obj) {
                if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                    id_ = obj.id;
                    var cityName = obj.value.split('-');
                    value_ = cityName[0];
                    return true;
                }
                else { return false; }
            });
            if (haveObject.length > 0) {
                if (hfCtrl)
                    $(hfCtrl).val(id_);
                this.value(value_);
                $(link).show();
                $(link).prev().hide();
            }

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
            }
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            var cityName = dataItem.value.split('-');
            Ctrl.val(cityName[0]);
            $(hfCtrl).val(dataItem.id);
            utility.FillDemographicCityState(PanelId, '#txtZip', '#txtCity', '#hfCity', '#txtState', '#ddlCountry', '#hfCountry', 'city', dataItem.value);
        }
        if (Ctrl.data("kendoAutoComplete"))
            Ctrl.data("kendoAutoComplete").destroy();
        $(Ctrl).kendoAutoComplete({
            dataTextField: 'value',
            filter: 'contains',
            minLength: 1,
            select: onSelect,
            change: onChange,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        utility.GetCitiesArray(Ctrl.val()).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });
    },
    populateDODischarge: function (obj) {
        if ($(obj).prop("checked") == true) {
            $('#pnlDemographic #frmDemographic #dtpDischargeDate').datepicker('setDate', new Date());
        } else {
            $('#pnlDemographic #frmDemographic #dtpDischargeDate').datepicker('setDate', null);
        }
    },
    LoadPatientDemogrphic: function () {
        //utility.ClearFormValidation('#pnlDemographic #frmDemographic', true);
        //AppPrivileges.GetFormPrivileges("Demographic", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {

        Patient_Demographic.getPrivileges().done(function (response) {
            //fixed EMR-5393
            if ($("#ddlMaritalStatus option").length <= 0) {
                var self = $('#pnlDemographic #frmDemographic');
                self.loadDropDowns(true);
            }

            if (response.status != false) {
                var jobj = JSON.parse(response);
                var obj = JSON.parse(jobj.responsePrivilages_JSON);
                $.each(obj, function (index, Item) {
                    if (Item.FormName == "ClinicalDecisionSupport_Clinical Decision Support") {
                        if (Item.privilegasMessage == "") {
                            $(" #mainForm  li#CDSAlert").show();
                            //$(" #mainForm  li#ImmunizationAlert").show();
                            $(" #mainForm #hfTriggerLocation").val('FaceSheet');
                        }
                    }
                    else if (Item.FormName == "Patient Portal Account") {
                        if (Item.privilegasMessage == "") {
                            $('#pnlDemographic #btnPatientPortal').removeAttr("style");
                        }
                        else {
                            $('#pnlDemographic #btnPatientPortal').css("display", "none");
                        }

                    }
                    else if (Item.FormName == "Demographic") {
                        if (Item.privilegasMessage != "") {
                            utility.DisplayMessages(Item.privilegasMessage, 2);
                        }
                        else {

                            $('#pnlDemographic  #frmDemographic').resetAllControls();
                            $("#pnlDemographic #hfReferralID").val(-1);
                            var referralId = $("#pnlDemographic #hfAssignee").val(-1);

                            if (Patient_Demographic.params.mode == "Add") {
                                Patient_Demographic.FacilityLabel = "Facility";
                                $("#" + Patient_Demographic.params["PanelID"] + " #lblFacility").text(Patient_Demographic.FacilityLabel);
                                Patient_Demographic.ProviderLabel = "Provider";
                                $("#" + Patient_Demographic.params["PanelID"] + " #lblProvider").text(Patient_Demographic.ProviderLabel);
                                if (globalAppdata['DefaultProviderName'] != "")
                                    $('#pnlDemographic #txtProvider').val(globalAppdata['DefaultProviderName']);
                                if (globalAppdata['DefaultProviderId'] != "")
                                    $('#pnlDemographic #hfProvider').val(globalAppdata['DefaultProviderId']);
                                if (globalAppdata['DefaultProviderId'] != "") {
                                    $('#pnlDemographic #lnkProviderEdit').css("display", "inline");
                                    $('#pnlDemographic #lblProvider').css("display", "none");
                                }

                                if (globalAppdata['DefaultFacilityName'] != "")
                                    $('#pnlDemographic #txtFacility').val(globalAppdata['DefaultFacilityName']);
                                if (globalAppdata['DefaultFacilityId'] != "")
                                    $('#pnlDemographic #hfFacility').val(globalAppdata['DefaultFacilityId']);
                                if (globalAppdata['DefaultFacilityId'] != "") {
                                    $('#pnlDemographic #lnkFacilityEdit').css("display", "inline");
                                    $('#pnlDemographic #lblFacility').css("display", "none");
                                }

                                if (globalAppdata['DefaultPracticeName'] != "")
                                    $('#pnlDemographic #txtPractice').val(globalAppdata['DefaultPracticeName']);

                                if (globalAppdata['DefaultPracticeId'] != "") {
                                    $('#pnlDemographic #hfPractice').val(globalAppdata['DefaultPracticeId']);
                                    demographicDetail.ScanOCRPriviliges(globalAppdata['DefaultPracticeId']);
                                }
                                $('#pnlDemographic #hfReferralID').val("-1");

                                $Ctrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtFacility");
                                $hfCtrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfFacility");
                                //Facility
                                utility.SetKendoAutoCompleteSourceforValidate($Ctrl, $Ctrl.val(), $hfCtrl, $hfCtrl.val());

                                $Ctrl_p = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtProvider");
                                $hfCtrl_p = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfProvider");
                                //Provider
                                utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, $Ctrl_p.val(), $hfCtrl_p, $hfCtrl_p.val());

                                //serialize data
                                $('#frmDemographic').data('serialize', $('#frmDemographic').serialize());
                            }
                            else if (Patient_Demographic.params.mode == "Edit") {
                                Patient_Demographic.FacilityLabel = "Last Seen Facility";
                                $("#" + Patient_Demographic.params["PanelID"] + " #lblFacility").text(Patient_Demographic.FacilityLabel);
                                $("#" + Patient_Demographic.params["PanelID"] + " #lnkFacilityEdit").text(Patient_Demographic.FacilityLabel);
                                Patient_Demographic.ProviderLabel = "Last Seen Provider";
                                $("#" + Patient_Demographic.params["PanelID"] + " #lblProvider").text(Patient_Demographic.ProviderLabel);
                                $("#" + Patient_Demographic.params["PanelID"] + " #lnkProviderEdit").text(Patient_Demographic.ProviderLabel);
                                //utility.ClearFormValidation('#' + Patient_Demographic.params.PanelID + " #frmDemographic");
                                Patient_Demographic.FillPatientInfo(Patient_Demographic.params).done(function () {
                                    // Set Patient Account Number into local storage from schedule flow.
                                    if (localStorage.SelectedAccountNumber == "null") {
                                        localStorage.setItem("SelectedAccountNumber", Patient_Demographic.params.PatientAccountNo);
                                    }
                                    if ($('#pnlDemographic #lnkProviderEdit').css("display") == "none") {
                                        $('#pnlDemographic #lnkProviderEdit').css("display", "inline");
                                        $('#pnlDemographic #lblProvider').css("display", "none");
                                    }
                                    if ($('#pnlDemographic #lnkFacilityEdit').css("display") == "none") {
                                        $('#pnlDemographic #lnkFacilityEdit').css("display", "inline");
                                        $('#pnlDemographic #lblFacility').css("display", "none");
                                    }
                                    if ($('#pnlDemographic #hfRefProvider').val() != "") {
                                        $('#pnlDemographic #lnkRefProviderEdit').css("display", "inline");
                                        $('#pnlDemographic #lblRefProvider').css("display", "none");
                                    }
                                    else {
                                        $('#pnlDemographic #lblRefProvider').css("display", "inline");
                                        $('#pnlDemographic #lnkRefProviderEdit').css("display", "none");
                                    }

                                    if ($('#pnlDemographic #hfPCP').val() != "") {
                                        $('#pnlDemographic #lnkPCPEdit').css("display", "inline");
                                        $('#pnlDemographic #lblPCP').css("display", "none");
                                    }
                                    else {
                                        $('#pnlDemographic #lblPCP').css("display", "inline");
                                        $('#pnlDemographic #lnkPCPEdit').css("display", "none");
                                    }

                                    if ($('#pnlDemographic #hfGuarantor').val() != "") {
                                        $('#pnlDemographic #lnkGuarantorEdit').css("display", "inline");
                                        $('#pnlDemographic #lblGuarantor').css("display", "none");
                                    }
                                    else {
                                        $('#pnlDemographic #lblGuarantor').css("display", "inline");
                                        $('#pnlDemographic #lnkGuarantorEdit').css("display", "none");
                                    }


                                    //serialize data
                                    //$('#frmDemographic').data('serialize', $('#frmDemographic').serialize());

                                    var imageSrc = $('#frmDemographic  #imgPatient').attr('src');
                                    if (imageSrc == 'Content/images/default_male_profile.gif' || imageSrc == 'Content/images/default_female_profile.gif') {
                                        $('#' + Patient_Demographic.params.PanelID + " #btnRemoveUploadedImage").hide();
                                        $('#' + Patient_Demographic.params.PanelID + " #frmDemographic #imgPatient").css('cursor', 'default');
                                    }
                                    else {
                                        $('#' + Patient_Demographic.params.PanelID + " #btnRemoveUploadedImage").show();
                                    }

                                    $('#frmDemographic').data('serialize', $('#frmDemographic').serialize());

                                    //Patient_Demographic.FillPatientInfo(Patient_Demographic.params.patientID);
                                    //Patient_Demographic.params["DemographicId"] = Patient_Demographic.params.patientID;

                                });
                            }
                            //    }
                            //    else {
                            //        utility.DisplayMessages(strMessage, 2);
                            //    }
                            //});

                        }
                    }
                    else if (Item.FormName == "Restrict User") {
                        if (Item.privilegasMessage == "") {
                            $("#pnlDemographic #btnRestictUser").removeClass('hidden');
                        }
                        else {
                            if (!$("#pnlDemographic #btnRestictUser").hasClass('hidden'))
                                $("#pnlDemographic #btnRestictUser").addClass('hidden');
                        }
                    }
                });
            }
        });

    },

    FillPatientBar: function (PatientDemographicResponse, patientID, isOpenNotes, CCmterminationReason) {
        var dfd = new $.Deferred();
        var PatientBalance = "";
        var InsuranceBalance = "";
        var PrimaryInsuranceName = "";
        var AdvanceBalance = "";
        var PrimaryInsuranceName = "";
        var PrimaryInsuranceId = "";
        var MissingDemoInfoLabelHTML = "";
        var PatientProfileInfo = JSON.parse(PatientDemographicResponse.DemographicFill_JSON != null ? PatientDemographicResponse.DemographicFill_JSON : PatientDemographicResponse);//store.fetch('selectPatientData', patientID);
        //appointmentDetail.FillPatient(patientID).done(function (response) {
        //var patient_balance = JSON.parse(response.PatientBalance_JSON);
        PatientBalance = PatientProfileInfo.PatientBalance;
        InsuranceBalance = PatientProfileInfo.InsuranceBalance;
        AdvanceBalance = PatientProfileInfo.AdvanceBalance;
        collBalance = PatientProfileInfo.CollectionBalance;
        PrimaryInsuranceName = PatientProfileInfo.InsuranceName;
        PrimaryInsuranceId = PatientProfileInfo.PatientInsuranceId;
        PrimaryInsurancePlanId = PatientProfileInfo.InsurancePlanId;

        var patientName = "<a href='#' onclick=Patient_Demographic.OpenPatientDemographic('" + patientID + "');> <strong ><span id='banner_PatientName' >" + PatientProfileInfo.FullName + "</span> (" + PatientProfileInfo.AccountNo + ") </strong> </a><button class='btn btn-success btn-xs mr-xs' id='btnDemographicLabel' hidden='' type='button' onclick='OpenDemographicLabel()'>Demographic Label</button>";

        if (PatientProfileInfo.Sex != "" && PatientProfileInfo.MaritalStatus != "" && PatientProfileInfo.Ethnicity != "" && PatientProfileInfo.PatientRaces.length > 0 && PatientProfileInfo.Address1 != "" && PatientProfileInfo.City != "" && PatientProfileInfo.State != "" && PatientProfileInfo.Zip != "" && PatientProfileInfo.HomeTel != "") {
            MissingDemoInfoLabelHTML = "";
        }
        else {
            MissingDemoInfoLabelHTML = '<span id="MissingDemoInfoLabel" class="animated infinite flash btn-flash btn-danger btn-xs tab_space d-inline-block pt-tiny pb-tiny ml-xxs">Incomplete Demographics</span>';
        }

        //TCM Section Start
        var TCMButton = "";
        if (PatientProfileInfo.ClaimFlagDescription == "Medicare Part B" && PatientProfileInfo.TransitionalCareManagement == "True") {
            TCMButton = "<a  class='btn btn-default btn-xs mr-xs' onclick=Patient_Demographic.LoadPhoneEncounter();> TCM </a>";
            $("#PatientProfile #hfDischargeDate").val(PatientProfileInfo.DischargeDate);

            //var count = parseInt($('#spnTCM').text()) + 1;
            //$('#pnlDashboard #TCM .badge').text(count);
            //$('#spnTCM').text(count);
        }

        //TCM Section End

        //CCM SECTION start
        var CCMButton = "";
        Patient_Demographic.EnrollmentInfoId = PatientProfileInfo.EnrollmentInfoId;
        if (PatientProfileInfo.CCMStatus != null && PatientProfileInfo.CCMStatus != "") {

            switch (PatientProfileInfo.CCMStatus) {

                case "Pending":
                    CCMButton = "<a  class='btn btn-success btn-xs mr-xs' onclick=Patient_Demographic.EnrollForCCM('" + patientID + "');> Enroll For CCM </a>";

                    break;
                case "Accepted":
                    CCMButton = "<a id='lnkccmHub' class='btn btn-warning btn-xs mr-xs'  onclick=Patient_Demographic.OpenCCMHub('" + PatientProfileInfo.EnrollmentInfoId + "','" + patientID + "');> CCM Hub </a>";
                    break;
                case "Declined":
                    break;
                case "Terminated":
                    CCMButton = "<a data-toggle='tooltip' data-original-title='" + CCmterminationReason + "' class='btn btn-danger btn-xs mr-xs'> CCM Terminated </a>";
                    //CCMButton = "<a data-toggle='tooltip' data-original-title='" + CCmterminationReason + "' class='btn btn-danger btn-xs mr-xs'  onclick=Patient_Demographic.OpenCCMHub('" + PatientProfileInfo.EnrollmentInfoId + "','" + patientID + "');> CCM Terminated </a>";
                    break;

            }

        }

        //CCM SECTION end
        //  var OutstandingBalance = "<strong>Pat. Bal:</strong> " + PatientOutstandingBalance + " <strong>Ins. Bal:</strong> " + InsuranceOutstandingBalance + " <strong>Adv. Bal:</strong> " + PatientAdvanceBalance;
        //patientName += OutstandingBalance;
        var patientAge = "";
        if (PatientProfileInfo.Age) {
            patientAge = PatientProfileInfo.Age.split(',');

            /////
            if (parseInt((PatientProfileInfo.Age.split(',')[0]).split(' ')[1]) > 0) {

                patientAge = PatientProfileInfo.Age.split(',')[0]; //age in years
            } else if (parseInt((PatientProfileInfo.Age.split(',')[1]).split(' ')[1]) > 0) {
                patientAge = PatientProfileInfo.Age.split(',')[1]; //age in months
            } else {
                patientAge = PatientProfileInfo.Age.split(',')[2]; //age in days

            }
        }
        var PHQScoreButton = "";
        if (PatientProfileInfo.PatientPHQScore != null && PatientProfileInfo.PatientPHQScore != "") {
            var PHQFlagColor = "";
            var intScore = parseInt(PatientProfileInfo.PatientPHQScore.substring(PatientProfileInfo.PatientPHQScore.indexOf(" = ") + 3));
            if (intScore > 0) {
                if (PatientProfileInfo.PatientPHQScore.toLowerCase().indexOf("phq-9") > -1) {
                    if (intScore >= 10 && intScore < 15) {
                        PHQFlagColor = "yellow";
                    }
                    else if (intScore >= 15) {
                        PHQFlagColor = "red";
                    }
                    else {
                        PHQFlagColor = "blue";
                    }
                }
                else if (PatientProfileInfo.PatientPHQScore.toLowerCase().indexOf("phq-2") > -1) {
                    if (intScore >= 3 && intScore < 5) {
                        PHQFlagColor = "yellow";
                    }
                    else if (intScore >= 5) {
                        PHQFlagColor = "red";
                    }
                    else {
                        PHQFlagColor = "blue";
                    }
                }
                PHQScoreButton = '<span title="' + PatientProfileInfo.PatientPHQScore + '"><i class="fa fa-flag ' + PHQFlagColor + ' fa-lg" runat="server" id="PHQScoreIcon"></i></span>';
            }
        }

        var DOB = 'DOB: <span id="banner_PatientDOB" >' + PatientProfileInfo.DOB + "</span>";
        var MaritalStatus = (PatientProfileInfo.MaritalStatus != "" ? PatientProfileInfo.MaritalStatus : "");
        var PatientSexBanner = (PatientProfileInfo.Sex != "" ? ", <span id=" + 'banner_PatientSex' + " >" + PatientProfileInfo.Sex + "</span>" : "");
        var patientInfo = DOB + ", " + patientAge + PatientSexBanner + (PatientProfileInfo.MaritalStatus != "" ? ", " + PatientProfileInfo.MaritalStatus : "");// + " &nbsp;&nbsp;&nbsp;&nbsp;" + OutstandingBalance;
        var title_patientInfo = "DOB: " + PatientProfileInfo.DOB + ", " + patientAge + (PatientProfileInfo.Sex != "" ? ", " + PatientProfileInfo.Sex : "") + (PatientProfileInfo.MaritalStatus != "" ? ", " + PatientProfileInfo.MaritalStatus : "");
        var patientAddress = ((PatientProfileInfo.Address1 != null && PatientProfileInfo.Address1 != "") ? ", " + PatientProfileInfo.Address1 + ', ' : "")
                            + ((PatientProfileInfo.City != null && PatientProfileInfo.City != "") ? PatientProfileInfo.City + ', ' : "")
                            + ((PatientProfileInfo.State != null && PatientProfileInfo.State != "") ? PatientProfileInfo.State + ' ' : "")
                            + ((PatientProfileInfo.Zip != null && PatientProfileInfo.Zip != "") ? PatientProfileInfo.Zip : "");
        var patientCityStateZip = "<span id='banner_PatientCityStateZip' >" + ((PatientProfileInfo.City != null && PatientProfileInfo.City != "") ? ", " + PatientProfileInfo.City : "")
                                   + ((PatientProfileInfo.State != null && PatientProfileInfo.State != "") ? ', ' + PatientProfileInfo.State + ' ' : " ")
                                   + ((PatientProfileInfo.Zip != null && PatientProfileInfo.Zip != "") ? PatientProfileInfo.Zip : "") + "</span>";
        var patientInfoOverflowText = "<br/> <span class='addressRow'>" + patientInfo + " <span id='banner_PatientAddress' >" + ((PatientProfileInfo.Address1 != null && PatientProfileInfo.Address1 != "") ? ", " + PatientProfileInfo.Address1 : "") + "</span></span>";
        var titleStart = "<span id='banner_PatientInfoAndAddress' data-toggle='tooltip' data-placement='bottom' MaritalStatus='" + MaritalStatus + "' title='" + title_patientInfo + patientAddress + "'>";
        var titleEnd = "</span>";
        //$("#PatientProfile #lblPatientData").html(patientName + patientInfo + patientAddress);
        $("#PatientProfile #lblPatientData").html(patientName + MissingDemoInfoLabelHTML + TCMButton + CCMButton + PHQScoreButton + titleStart + patientInfoOverflowText + patientCityStateZip + titleEnd);

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        Patient_Demographic.UpdateBalancesInBanner(InsuranceBalance, PatientBalance, AdvanceBalance, collBalance, PrimaryInsuranceName, PrimaryInsuranceId);

        $('#PatientProfile #hfPatientId').val(patientID);
        $('#PatientProfile #hfAccountNo').val(PatientProfileInfo.AccountNo);
        $('#PatientProfile #hfPatientFullNameOnly').val(PatientProfileInfo.FullName);
        $('#PatientProfile #hfPatientFullName').val(PatientProfileInfo.FullName + " " + PatientProfileInfo.AccountNo);

        $('#PatientProfile #hfPatientPracticeId').val(PatientProfileInfo.PracticeID);
        $('#PatientProfile #hfPatientFacilityId').val(PatientProfileInfo.FacilityID);

        /***setting patient's ref provdier**/
        $('#PatientProfile #hfPatientRefProviderId').val(PatientProfileInfo.RefProviderID);
        $('#PatientProfile #hfPatientRefProviderName').val(PatientProfileInfo.RefProvider);

        /* Start 09/12/2015 Muhammad Irfan to set the patient Gender globally in hidden field, for use in SocialHx */
        $('#PatientProfile #hfPatientSex').val(PatientProfileInfo.Sex);
        /* End 09/12/2015 Muhammad Irfan to set the patient Gender globally in hidden field, for use in SocialHx */

        /* Start 19/01/2016 Muhammad Arshad set the patient DOB globally in hidden field, for use in SurgicalHx */
        $('#PatientProfile #hfPatientDOB').val(PatientProfileInfo.DOB);
        /* End 19/01/2016 Muhammad Arshad set the patient DOB globally in hidden field, for use in SurgicalHx */

        $('#PatientProfile #hfPatientFacilityName').val(PatientProfileInfo.Facility);
        $('#PatientProfile #hfPatientProviderName').val(PatientProfileInfo.Provider);
        $('#PatientProfile #hfPatientProviderId').val(PatientProfileInfo.ProviderID);

        $('#PatientProfile #hfPatientMaritalStatus').val(PatientProfileInfo.MaritalStatus);
        $('#PatientProfile #hfPatientEthnicityIds').val(PatientProfileInfo.Ethnicity);
        var RaceIds = PatientProfileInfo.PatientRaces.map(function (e) {
            return e.Id;
        }).join(',');
        $('#PatientProfile #hfPatientRaceIds').val(RaceIds);
        $('#PatientProfile #hfPatientAddress1').val(PatientProfileInfo.Address1);
        $('#PatientProfile #hfPatientCity').val(PatientProfileInfo.City);
        $('#PatientProfile #hfPatientState').val(PatientProfileInfo.State);
        $('#PatientProfile #hfPatientZip').val(PatientProfileInfo.Zip);
        $('#PatientProfile #hfPatientHomeTel').val(PatientProfileInfo.HomeTel);


        //if (globalAppdata["PBPhone1"] == "True") {
        //    if (PatientProfileInfo.Cell != "") {
        //        $("#PatientProfile #Phone1").html("<strong>Phone 1:</strong> " + PatientProfileInfo.Cell);
        //    }
        //} else {
        //    $("#PatientProfile #Phone1").html("");
        //}
        //if (globalAppdata["PBPhone2"] == "True") {
        //    if (PatientProfileInfo.HomeTel != "") {
        //        $("#PatientProfile #Phone2").html("<strong>Phone 2:</strong> " + PatientProfileInfo.HomeTel);
        //    }
        //} else {
        //    $("#PatientProfile #Phone2").html("");
        //}
        if (globalAppdata["PBPreferredPhone"] == "1") {
            if (PatientProfileInfo.HomeTel != "") {
                $("#PatientProfile #PreferredPhone").html("<strong>Home Tel:</strong> " + PatientProfileInfo.HomeTel);
            } else {
                $("#PatientProfile #PreferredPhone").html("");
            }
        } else if (globalAppdata["PBPreferredPhone"] == "2") {
            if (PatientProfileInfo.Cell != "") {
                $("#PatientProfile #PreferredPhone").html("<strong>Cell:</strong> " + PatientProfileInfo.Cell);
            } else {
                $("#PatientProfile #PreferredPhone").html("");
            }
        } else if (globalAppdata["PBPreferredPhone"] == "3") {
            if (PatientProfileInfo.WorkTel != "") {
                $("#PatientProfile #PreferredPhone").html("<strong>Work Tel:</strong> " + PatientProfileInfo.WorkTel);
            } else {
                $("#PatientProfile #PreferredPhone").html("");
            }
        }
        else {
            $("#PatientProfile #PreferredPhone").html("");
        }


        if (globalAppdata["PBPCP"] == "True") {
            if (PatientProfileInfo.PCP != "") {
                $("#PatientProfile #PCP").html("<strong>PCP:</strong> " + PatientProfileInfo.PCP);
            }
            else {
                $("#PatientProfile #PCP").html("");
            }
        } else {
            $("#PatientProfile #PCP").html("");
        }
        if (globalAppdata["PBRefProvider"] == "True") {
            if (PatientProfileInfo.RefProvider != "") {
                $("#PatientProfile #RefProvider").html("<strong>Ref Provider:</strong> " + PatientProfileInfo.RefProvider);
            }
            else
                $("#PatientProfile #RefProvider").html("");
        } else {
            $("#PatientProfile #RefProvider").html("");
        }

        //set Params for Patient Eligibility
        Patient_Demographic.params.PatientAccountNo = PatientProfileInfo.AccountNo;
        Patient_Demographic.params.PatientFirstName = PatientProfileInfo.FirstName;
        Patient_Demographic.params.PatientLastName = PatientProfileInfo.LastName;
        Patient_Demographic.params.PatientProvider = PatientProfileInfo.Provider;
        Patient_Demographic.params.PatientProviderId = PatientProfileInfo.ProviderID;
        Patient_Demographic.params.PatientFacility = PatientProfileInfo.Facility;
        Patient_Demographic.params.PatientFacilityId = PatientProfileInfo.FacilityID;
        Patient_Demographic.params.PatientEmail = PatientProfileInfo.Email;

        //set params for Patient Ledger
        Patient_Demographic.params.patientAge = PatientProfileInfo.Age;
        Patient_Demographic.params.PatientSex = PatientProfileInfo.Sex;
        Patient_Demographic.params.PatientDOB = PatientProfileInfo.DOB;
        Patient_Demographic.params.PatientHomeTel = PatientProfileInfo.HomeTel;
        Patient_Demographic.params.PatientPortalStatus = PatientProfileInfo.PatientPortalStatus;
        Patient_Demographic.params.GuarantorID = PatientProfileInfo.GuarantorID;
        Patient_Demographic.params.PatWorkTel = PatientProfileInfo.WorkTel;
        Patient_Demographic.params.PatCell = PatientProfileInfo.Cell;
        Patient_Demographic.params.PatHomeTel = PatientProfileInfo.HomeTel;
        Patient_Demographic.params.SecondPrefCom = PatientProfileInfo.ScndPrefCommunicationId;
        Patient_Demographic.params.FirstPrefCom = PatientProfileInfo.PrefCommunicationId;
        Patient_Demographic.params.PracticeID = PatientProfileInfo.PracticeID;


        if (PatientProfileInfo.HomeTel != "" && PatientProfileInfo.Cell != "" && PatientProfileInfo.WorkTel != "") {
            Patient_Demographic.params.IsPhoneNoExists = true;
        } else {
            Patient_Demographic.params.IsPhoneNoExists = false;
        }

        if (PatientProfileInfo.ScndPrefCommunicationId == "" && PatientProfileInfo.PrefCommunicationId == "") {
            Patient_Demographic.params.IsPreferencesExists = false;
        } else if ((PatientProfileInfo.ScndPrefCommunicationId != undefined && PatientProfileInfo.PrefCommunicationId != undefined) && (PatientProfileInfo.ScndPrefCommunicationId != "" || PatientProfileInfo.PrefCommunicationId != "")) {
            Patient_Demographic.params.IsPreferencesExists = true;
        }

        //set Params for CharegeCapture
        Patient_Demographic.params.patFullName = PatientProfileInfo.AccountNo + ' - ' + PatientProfileInfo.FullName;
        Patient_Demographic.params.RefProviderId = PatientProfileInfo.RefProviderID;
        Patient_Demographic.params.RefProviderName = PatientProfileInfo.RefProvider;
        Patient_Demographic.params.SelfPay = PatientProfileInfo.SelfPay;

        params["PatientFirstName"] = PatientProfileInfo.FirstName;
        params["PatientLastName"] = PatientProfileInfo.LastName;
        params["PatientDOB"] = PatientProfileInfo.DOB;
        params["PatientEmail"] = PatientProfileInfo.Email;
        if (isOpenNotes == '1') {
            utility.callbackAfterAllDOMLoaded(function () {
                $.when(updateNotificationsCounts()).then(function () {
                    dfd.resolve();
                });
            });
        } else {
            Patient_Demographic.isFinanicialAlert = true;
            utility.callbackAfterAllDOMLoaded(function () {
                $.when(Patient_Demographic.FillPatientAlertsCount(isOpenNotes)).then(function () {
                    dfd.resolve();
                });
            });
        }
        //Patient_Demographic.FillPatientMessagesCount();
        //});

        if (Patient_Demographic.params.PatientId == undefined && Patient_Demographic.params.patientID == undefined) {
            if (params['patientID'] != null && params['patientID'] > 0) {
                Patient_Demographic.params.PatientId = params['patientID'];
            }
        }

        var PatientId = Patient_Demographic.params.PatientId ? Patient_Demographic.params.PatientId : Patient_Demographic.params.patientID;
        utility.ValidateMU3Demographics("#" + Patient_Demographic.params.PanelID + " #frmDemographic");

        // Commented as requirement may be need in future, so do not remove code. PRD-795
        if (parseInt(PatientProfileInfo.MissingDataAlertsCount) > 0)
            utility.toggelMU3Alerts(true, parseInt(PatientProfileInfo.MissingDataAlertsCount));
        else
            utility.toggelMU3Alerts(false, 0);

        return dfd.promise();
    },

    LoadPhoneEncounter: function () {

        $.when(ClinicalMenuSettings.ClinicalMenuSettingsSearch(null)).then(function () {
            $('#mstrTabClinical').siblings().removeClass('active');
            $('#mstrTabClinical').addClass('active');
            $('#ClinicalUL li').removeClass('nav-expanded nav-active');
            $('#ClinicalUL li#clinicalMenuNotes').addClass('nav-expanded nav-active');
            $('#ctrlPanDashBoard').hide();

            EMRUtility.unSelectOtherTabs('mstrTabClinical', 'false');

            javascript: ClinicalMenuClick(window.event, function () {
                $.when(ClinicalMenuSettings.TopButtons('clinicalMenuNotes')).then(function () {
                    ClinicalMenuSettings.selectClinicalMenu('clinicalMenuNotes');
                    SelectTab("clinicalTabPhoneEncounter", "false");

                    setTimeout(function () {
                        //Patient_Demographic.FillPatientAlertsCount();
                        $('#pnlClinicalPhoneEncounter #txtVisitReason').val('Transitional Care Management');
                        $("#pnlClinicalPhoneEncounter #ddlEncounterType").val(4);
                        $("#pnlClinicalPhoneEncounter #NoteTemplate option").each(function () {
                            if ($(this).text().toLowerCase() == "phone encounter tcm") {
                                $(this).attr('selected', 'selected');
                            }
                        });
                        $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter').bootstrapValidator('revalidateField', 'VisitReason');
                    }, 2000);
                });
            }, 0, this, 'clinicalMenuNotes', 'li');
        });
    },
    OpenScannedImage: function () {
        var strMessage = "";
        //if (event != null) {
        //    event.stopPropagation();
        //}
        //utility.SelectGridRow($("dgvPatientDocument_row" + PatDocID));
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ((['Content/images/default_male_profile.gif', 'Content/images/default_female_profile.gif'].indexOf($("#pnlDemographic #divDemographicPicture #imgPatient").attr("src")) == -1)) {

                    if (Patient_Demographic.PatDocId && (['Content/images/default_male_profile.gif', 'Content/images/default_female_profile.gif'].indexOf($("#pnlDemographic #divDemographicPicture #imgPatient").attr("src")) == -1)) {
                        var params = [];
                        params["PatientID"] = Patient_Demographic.params.patientID;
                        params["PatDocID"] = Patient_Demographic.PatDocId;
                        params["mode"] = "Edit";
                        params["RefDemographic"] = "View_Only";
                        params["FromAdmin"] = Patient_Demographic.params["FromAdmin"];
                        if (Patient_Demographic.params["FromAdmin"] == "0") {
                            params["ParentCtrl"] = Patient_Demographic.params["TabID"];
                        }
                        LoadActionPan('Document_Viewer', params);
                    } else {

                        return false;
                    }
                }
                else {

                    utility.DisplayMessages("No document is saved.", 2);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    FillPatientInfo: function (params) {
        //Patient_Demographic.params = params;
        var patient_Id = "";
        if (params.PanelID)
            Patient_Demographic.params.PanelID = params.PanelID;
        else
            Patient_Demographic.params.PanelID = "pnlDemographic";

        if (!Patient_Demographic.params.patientID || params.GrandParent == "DashboardAppointment" || params.ComeFrom == "Patient_Search") {
            patient_Id = params.patientID;
        }
        else
            patient_Id = Patient_Demographic.params.patientID;
        Patient_Demographic.params.patientID = patient_Id;
        //  Patient_Demographic.params.patientID = params.patientID;//kr
        var dfd = new $.Deferred();
        //ClosePatient();
        var patientJSON = ""; //store.fetch('selectPatientData', patientID);
        if (patientJSON != "") {
            var demographic_detail = JSON.parse(patientJSON);
            Patient_Demographic.BindJSON(demographic_detail);
            Patient_Demographic.FillPatientBar(patientJSON, Patient_Demographic.params.patientID);
            //if (response.Image_url != "") {
            //    $("#pnlDemographic #divDemographicPicture #imgPatient").attr("src", response.Image_url);
            //    $("#PatientProfile #imgPatientProfile").attr("src", response.Image_url);
            //}
            //else
            //{
            if (demographic_detail.Sex == "Female") {
                $("#pnlDemographic #divDemographicPicture #imgPatient").attr("src", "Content/images/default_female_profile.gif");
                $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_female_profile.gif");
                $("#pnlDemographic #divDemographicPicture #imgPatient").css({ "cursor": "default" });
            }
            else if (demographic_detail.Sex == "Male" || demographic_detail.Sex == "Other") {
                $("#pnlDemographic #divDemographicPicture #imgPatient").attr("src", "Content/images/default_male_profile.gif");
                $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_male_profile.gif");
                $("#pnlDemographic #divDemographicPicture #imgPatient").css({ "cursor": "default" });
            }

            //}
        }
        else {

            //var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Demographic", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {
            Patient_Demographic.FillDemographic(Patient_Demographic.params.patientID).done(function (response) {
                if (response.status != false) {
                    var demographic_detail = JSON.parse(response.DemographicFill_JSON);
                    Patient_Demographic.BindJSON(demographic_detail);
                    $('#pnlDemographic #frmDemographic #dtpDOB').datepicker('setDate', demographic_detail.DOB);
                    Patient_Demographic.CalculateAge($('#' + Patient_Demographic.params.PanelID + ' #dtpDOB'));


                    if ($("#pnlDemographic #frmDemographic #ddlPatientStatus option:selected").text().toLowerCase().trim() == "deceased") {
                        $("#pnlDemographic #frmDemographic #divDateOfDeath").removeClass('hidden');
                    }
                    else {
                        $("#pnlDemographic #frmDemographic #divDateOfDeath").addClass('hidden');
                    }
                    if (demographic_detail.PracticeID) {
                        demographicDetail.ScanOCRPriviliges(demographic_detail.PracticeID);
                    }
                    if (demographic_detail.SSN) {
                        Patient_Demographic.PatientSSN = demographic_detail.SSN;
                        $('#pnlDemographic #frmDemographic #hfPatientSSN').val(demographic_detail.SSN);
                        if (globalAppdata.IsFullSSN.toLowerCase() === 'true') {
                            $('#pnlDemographic #frmDemographic #txtSSN').attr("placeholder", "999-99-9999");
                            $('#pnlDemographic #frmDemographic #txtSSN').attr("data-mask", "999-99-9999");
                            $('#pnlDemographic #frmDemographic #txtSSN').val(demographic_detail.SSN);
                            $('#pnlDemographic #frmDemographic #txtSSN').attr("disabled", false);
                        }
                        else {
                            $('#pnlDemographic #frmDemographic #txtSSN').attr("placeholder", "XXX-XX-9999");
                            $('#pnlDemographic #frmDemographic #txtSSN').attr("data-mask", "XXX-XX-9999");
                            var lastFourDigit = demographic_detail.SSN.slice(-4);
                            var formatSSN = "XXX-XX-" + lastFourDigit;
                            $('#pnlDemographic #frmDemographic #txtSSN').val(formatSSN);
                            $('#pnlDemographic #frmDemographic #txtSSN').attr("disabled", true);
                        }
                    }
                    else {
                        Patient_Demographic.PatientSSN = demographic_detail.SSN;
                        $('#pnlDemographic #frmDemographic #txtSSN').attr("placeholder", "999-99-9999");
                        $('#pnlDemographic #frmDemographic #txtSSN').attr("data-mask", "999-99-9999");
                        $('#pnlDemographic #frmDemographic #txtSSN').val(demographic_detail.SSN);
                        $('#pnlDemographic #frmDemographic #txtSSN').attr("disabled", false);
                        $('#pnlDemographic #frmDemographic #hfPatientSSN').val(demographic_detail.SSN);
                    }
                    Patient_Demographic.CommunicateWithGurrantor = demographic_detail.CommunicateWithGurantor;


                    Patient_Demographic.ProviderFName = demographic_detail.ProviderFName;
                    Patient_Demographic.ProviderLName = demographic_detail.ProviderLName;
                    Patient_Demographic.FacilityAddress = demographic_detail.FacilityAddress;
                    Patient_Demographic.FacilityCity = demographic_detail.FacilityCity;
                    Patient_Demographic.FacilityState = demographic_detail.FacilityState;
                    Patient_Demographic.FacilityZip = demographic_detail.FacilityZip;
                    Patient_Demographic.FacilityZipExt = demographic_detail.FacilityZipExt;
                    Patient_Demographic.FacilityPhone = demographic_detail.FacilityPhone;


                    //if (response.Image_url != "") {
                    Patient_Demographic.PatDocId = demographic_detail.PatDocId;
                    if (demographic_detail.imgPatient != undefined && demographic_detail.imgPatient != "") {
                        $("#pnlDemographic #divDemographicPicture #imgPatient").attr("src", demographic_detail.imgPatient);
                        $("#PatientProfile #imgPatientProfile").attr("src", demographic_detail.imgPatient);
                        $("#pnlDemographic #divDemographicPicture #imgPatient").css({ "cursor": "pointer" });
                    }
                    else {
                        if (demographic_detail.Sex == "Female") {
                            $("#pnlDemographic #divDemographicPicture #imgPatient").attr("src", "Content/images/default_female_profile.gif");
                            $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_female_profile.gif");
                        }
                        else if (demographic_detail.Sex == "Male" || demographic_detail.Sex == "Other") {
                            $("#pnlDemographic #divDemographicPicture #imgPatient").attr("src", "Content/images/default_male_profile.gif");
                            $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_male_profile.gif");
                        }
                        $("#pnlDemographic #divDemographicPicture #imgPatient").css({ "cursor": "default" });
                    }
                    //store.clear('selectPatientData', Patient_Demographic.params.patientID);
                    //store.save('selectPatientData', response.DemographicFill_JSON, Patient_Demographic.params.patientID);
                    Patient_Demographic.FillPatientBar(response, Patient_Demographic.params.patientID);
                    dfd.resolve('ok');
                    //Start by KR to fill multi-select on 5 May 2016.

                    //fill Race Values
                    var values_ = new Array();
                    var multiselect = $('#' + Patient_Demographic.params.PanelID + ' #ddlPatientRace').data("kendoMultiSelect");



                    var multiselect_data = multiselect.dataSource.data();
                    multiselect.value([]);
                    multiselect.refresh();
                    for (var i = 0; i < demographic_detail.PatientRaces.length; i++) {
                        values_[i] = demographic_detail.PatientRaces[i].Id;
                        var filter_data = $.grep(multiselect_data, function (item, j) {
                            return item.Name == demographic_detail.PatientRaces[i].Name
                            && item.Value == demographic_detail.PatientRaces[i].Id
                        });
                        if (filter_data.length == 0)
                            multiselect_data.push({ Name: demographic_detail.PatientRaces[i].Name, Value: demographic_detail.PatientRaces[i].Id });
                    }
                    multiselect.dataSource.data(multiselect_data);
                    multiselect.value(values_);
                    multiselect.refresh();
                    $('#' + Patient_Demographic.params["PanelID"] + ' #hfRaceIds').val(values_.join(','));

                    utility.callbackAfterAllDOMLoaded(function () {
                        try {
                            if (Patient_Demographic.params["PanelID"] && Patient_Demographic.params["PanelID"].indexOf("pnlDemographic") > -1 && $("#" + Patient_Demographic.params["PanelID"]).is(":visible") == true) {

                                // serialize all data in global variable when form is  loaded
                                params.defaultDemographicSerailizeForm = $('#frmDemographic').serialize();
                                params.IsDemographicInfoGlobalyUpdated = false;
                                params.DemographicAutoUpdateActiveTab = "Demographic";
                                $("#" + Patient_Demographic.params["PanelID"] + " #frmDemographic").data('bootstrapValidator').resetForm();

                            }
                            else if ($("#pnlDemographic #frmDemographic").length > 0 && $("#pnlDemographic #frmDemographic").is(":visible") == true) {

                                // serialize all data in global variable when form is  loaded
                                params.defaultDemographicSerailizeForm = $('#frmDemographic').serialize();
                                params.IsDemographicInfoGlobalyUpdated = false;
                                params.DemographicAutoUpdateActiveTab = "Demographic";
                                $("#pnlDemographic #frmDemographic").data('bootstrapValidator').resetForm();

                            }
                            //// serialize all data in global variable when form is  loaded
                            //params.defaultDemographicSerailizeForm = $('#frmDemographic').serialize();
                            //params.IsDemographicInfoGlobalyUpdated = false;
                        } catch (e) {

                        }
                    });
                    if (demographic_detail.strEthnicityIds) {
                        var arrEthnicityIds = demographic_detail.strEthnicityIds.split(',');
                        utility.callbackAfterAllDOMLoaded(function () {
                            $.each(arrEthnicityIds, function (i, itm) {
                                var OptionText = $('#' + Patient_Demographic.params.PanelID + " #ddlEthnicity option[value='" + itm + "']").text();
                                if (OptionText.toLowerCase() == "declined to specify" || OptionText.toLowerCase() == "refused to report") {
                                    $('#' + Patient_Demographic.params.PanelID + " #ddlEthnicity option").each(function (e, item) {
                                        if ($(item).val() != itm) {
                                            var input = $('#pnlDemographic #divPatientEthnicity input[type=checkbox][value="' + $(item).val() + '"]');
                                            input.prop('disabled', true);
                                            input.parent('li').addClass('disabled');
                                        }
                                    });
                                }
                            });
                        });
                        $('#' + Patient_Demographic.params.PanelID + " #ddlEthnicity").val(arrEthnicityIds);
                        $('#' + Patient_Demographic.params.PanelID + " #ddlEthnicity").multiselect("refresh");
                        Patient_Demographic.multipleEthnicityIds = demographic_detail.strEthnicityIds;
                        if (Patient_Demographic.multipleEthnicityIds != '') {
                            Patient_Demographic.validateEthnicity(2);
                        }
                    } else {
                        $('#' + Patient_Demographic.params.PanelID + " #ddlEthnicity").find("option:selected").removeAttr("selected");
                    }
                    //End by KR to fill multi-select on 5 May 2016.

                    $("#pnlDemographic #ddlHearFrom").val(demographic_detail.HearFromId);
                    if (demographic_detail.HearFromId == '1') {
                        Patient_Demographic.getPatientReferral();
                        $("#pnlDemographic #incomingReferral").removeClass('hidden');
                    }
                    else {
                        $("#pnlDemographic #incomingReferral").addClass('hidden');
                    }

                    if (demographic_detail.HearFromId == '10') {
                        $("#pnlDemographic #txtOtherText").val(demographic_detail.HearFromOther);
                        $("#pnlDemographic #divOtherText").removeClass('hidden');
                    }
                    else {
                        $("#pnlDemographic #txtOtherText").val('');
                        $("#pnlDemographic #divOtherText").addClass('hidden');
                    }

                    if (demographic_detail.TransitionalCareManagement == 'True')
                        $("#pnlDemographic #chkTransitionalCareManagement").prop("checked", true);
                    else
                        $("#pnlDemographic #chkTransitionalCareManagement").prop("checked", false);

                    if (demographic_detail.DischargeDate != null && demographic_detail.DischargeDate != "")
                        $("#" + demographicDetail.params.PanelID + " #dtpDischargeDate").val(demographic_detail.DischargeDate);
                    else
                        $("#" + demographicDetail.params.PanelID + " #dtpDischargeDate").val('');

                    if (demographic_detail.CareGiverIds) {
                        var arrCareGiveIds = demographic_detail.CareGiverIds.split(',');
                        $('#' + Patient_Demographic.params.PanelID + " #ddlCareGiver").val(arrCareGiveIds);
                        $('#' + Patient_Demographic.params.PanelID + " #ddlCareGiver").multiselect("refresh");
                        Patient_Demographic.careGiverIds = demographic_detail.CareGiverIds;
                    } else {
                        $('#' + Patient_Demographic.params.PanelID + " #ddlCareGiver").find("option:selected").removeAttr("selected");
                        $('#' + Patient_Demographic.params.PanelID + " #ddlCareGiver").multiselect("refresh");
                    }

                    $('#frmDemographic').data('serialize', $('#frmDemographic').serialize());
                    //var self = $("#frmDemographic");

                    //var referralJSON = JSON.parse(response.ReferralJSON);
                    //utility.bindMyJSONByName(true, referralJSON, false, self);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    return dfd.promise();
                }
            });
            //    }
            //    else {
            //        utility.DisplayMessages(strMessage, 2);
            //        return dfd.promise();
            //    }
            //});
        }
        return dfd.promise();
    },

    OpenOutstandingVisit: function (patientID, PatientOutstanding, isFromCollection) {
        var params = [];


        params["PatientOutstanding"] = PatientOutstanding;

        params["patientID"] = patientID;
        params["PatientAccountNo"] = Patient_Demographic.params.PatientAccountNo;
        params["PatientFirstName"] = Patient_Demographic.params.PatientFirstName;
        params["PatientLastName"] = Patient_Demographic.params.PatientLastName;
        params["patientAge"] = Patient_Demographic.params.patientAge;
        params["PatientSex"] = Patient_Demographic.params.PatientSex;
        params["PatientDOB"] = Patient_Demographic.params.PatientDOB;
        params["PatientHomeTel"] = Patient_Demographic.params.PatientHomeTel;
        params["IsFromCollection"] = isFromCollection;
        params["PracticeId"] = Patient_Demographic.params.PracticeID;

        params["PatBanner"] = true;

        //Begin Edited by Azeem Raza Tayyab on 26-May-2016 to fix Bug#:PMS-4388
        var tab = GetCurrentSelectedTab();
        if (tab.TabID && tab.TabID.indexOf("mstr") > -1 && tab.TabID.toLowerCase().indexOf("billing") == -1) {
            SelectTab('mstrTabPatient', 'false');
            params["ParentCtrl"] = "patTabDemographic";
        }
        else {
            params["ParentCtrl"] = tab.TabID;
        }

        //End Edited by Azeem Raza Tayyab on 26-May-2016 to fix Bug#:PMS-4388
        setTimeout(function () {
            LoadActionPan('Patient_Ledger', params);
        }, 600);



    },
    loadDemographicTab: function () {
        var tab = GetCurrentSelectedTab();
        if (tab.TabID.indexOf("mstr") > -1) {
            SelectTab('mstrTabPatient', 'false');
            params["ParentCtrl"] = "patTabDemographic";
        }
        else {
            params["ParentCtrl"] = tab.TabID;
        }
    },
    OpenEmergencyContact: function () {
        var params = [];
        params["EmergencyContactId"] = "-1";
        params["FromAdmin"] = "0";
        params["patientID"] = Patient_Demographic.params.patientID;
        params["ParentCtrl"] = "patTabDemographic";
        LoadActionPan('Patient_EmergencyContact', params);
    },

    OpenDemographicQuick: function () {
        //CurrentTab = GetCurrentSelectedTab();
        //if (CurrentTab.MasterTabID == "mstrTabPatient") {
        //    ActionPanContainer = "actionPanPatient";
        //}

        var params = [];
        //params = Patient_Demographic.params

        params["FromAdmin"] = "0";

        params["ParentCtrl"] = "patTabDemographic";
        LoadActionPan('Patient_DemographicQuick', null)
    },

    FillPatientAlertsCount: function (isOpenNotes, PatientId, parentCtrl) {
        var dfd = new $.Deferred();
        $("#pnlDemographic #ddlType").val(2);
        $("#pnlDemographic #ddlStatus").val(2);
        var self = $("#pnlDemographic");
        var myJSON = self.getMyJSON();
        if (PatientId == undefined) {
            PatientId = $("#PatientProfile #hfPatientId").val();
        }

        if (PatientId && PatientId != "") {
            Patient_MessageAlert.SearchMessageAlert(myJSON, PatientId, 0, 2, 2).done(function (response) {

                if (response.status != false) {
                    if (response.MessageCount > 0) {
                        $("#spnAlertCount").text(response.MessageCount);
                        $("#btnPatientAlerts").css("display", "inline-block");

                        if (Patient_Demographic.isFinanicialAlert || isOpenNotes == '1') {
                            var selectedTab = GetCurrentSelectedTab();
                            //if (selectedTab.TabID != "mstrTabClinical" && selectedTab.TabID != "clinicalTabFaceSheet") {
                            //if (isOpenNotes != '1') {
                            Patient_Demographic.OpenPatientAlerts(PatientId, parentCtrl);
                            Patient_Demographic.isFinanicialAlert = false;
                            //}

                            //}
                        }
                        else if (Patient_Demographic.isDocExpiryAlert) {
                            Patient_Demographic.FillPatientDocumentExpiryAlert(PatientId);
                        }
                    }
                    else {
                        $("#btnPatientAlerts").css("display", "none");
                        Patient_Demographic.isFinanicialAlert = true;
                        if (Patient_Demographic.isDocExpiryAlert)
                            Patient_Demographic.FillPatientDocumentExpiryAlert(PatientId);
                    }
                    //Patient_MessageAlert.MessageAlertGridLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                dfd.resolve();
                try {

                    if (Patient_Demographic.params["PanelID"] && Patient_Demographic.params["PanelID"].indexOf("pnlDemographic") > -1 && $("#" + Patient_Demographic.params["PanelID"]).is(":visible") == true) {

                        // serialize all data in global variable when form is completly loaded
                        params.defaultDemographicSerailizeForm = $('#frmDemographic').serialize();
                        params.IsDemographicInfoGlobalyUpdated = false;
                        params.DemographicAutoUpdateActiveTab = "Demographic";
                        $("#" + Patient_Demographic.params["PanelID"] + " #frmDemographic").data('bootstrapValidator').resetForm();

                    }
                    else if ($("#pnlDemographic #frmDemographic").length > 0 && $("#pnlDemographic #frmDemographic").is(":visible") == true) {

                        // serialize all data in global variable when form is completly loaded
                        params.defaultDemographicSerailizeForm = $('#frmDemographic').serialize();
                        params.IsDemographicInfoGlobalyUpdated = false;
                        params.DemographicAutoUpdateActiveTab = "Demographic";
                        $("#pnlDemographic #frmDemographic").data('bootstrapValidator').resetForm();

                    }

                } catch (e) {

                }

            });
        }
        else {
            console.warn(" patientid is coming null in FillPatientAlertsCount. PatientId:" + PatientId)
        }
        return dfd.promise();
    },

    FillPatientMessagesCount: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Tasks", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Patient_Message.MessageSearch("", Patient_Demographic.params.patientID, 0).done(function (response) {

                    if (response.status != false) {
                        if (response.MessageCount > 0) {
                            $("#spnMessageCount").text(response.MessageCount);
                            $("#btnPatientMessages").css("display", "inline-block");
                        }
                        //else
                        //    $("#btnPatientMessages").css("display", "none");
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OpenPatientAlerts: function (PatientId, parentCtrl) {
        var params = [];
        if (PatientId == undefined) {
            if (Patient_Demographic.params.patientID && Patient_Demographic.params.patientID != "") {
                PatientId = Patient_Demographic.params.patientID;
            } else {
                PatientId = $("#PatientProfile #hfPatientId").val();
            }
        }
        if (PatientId && PatientId != "") {
            params["patientID"] = PatientId;
            if (parentCtrl) {
                params["ParentCtrl"] = parentCtrl;
            }

            LoadActionPan('Patient_MessageAlert', params)
        }
        else {
            console.warn(" patientid is coming null in OpenPatientAlerts. PatientId:" + PatientId)
        }
    },

    FillPatientDocumentExpiryAlert: function (PatientId) {
        var selectedTab = GetCurrentSelectedTab();
        if (!Patient_Demographic.isDocExpiryAlert || selectedTab.TabID != "patTabDemographic" && selectedTab.TabID != "clinicalTabFaceSheet" && selectedTab.TabID != "clinicalTabProgressNote" && selectedTab.TabID != "billTabOutOfOfficeVisits" && selectedTab.TabID != "schTabCalendar") {
            return;
        }
        var dfd = new $.Deferred();
        if (PatientId == undefined) {
            PatientId = $("#PatientProfile #hfPatientId").val();
        }

        if (PatientId && PatientId != "") {
            Patient_Document.FillPatientDocumentExpiryAlert_DbCall(PatientId).done(function (response) {

                if (response.status) {
                    if (response.DocumentCount > 0) {
                        var DocumentExpiryJSONData = JSON.parse(response.PatientDocumentExpiry_JSON);
                        var groups = {};
                        var DocumentExpiryData = DocumentExpiryJSONData;
                        for (var i = 0; i < DocumentExpiryData.length; i++) {
                            var groupName = DocumentExpiryData[i].ExpiryDate;
                            if (!groups[groupName]) {
                                groups[groupName] = [];
                            }
                            groups[groupName].push(DocumentExpiryData[i].FilePath);
                        }
                        DocumentExpiryData = [];
                        for (var groupName in groups) {
                            DocumentExpiryData.push({ ExpiryDate: groupName, FilePath: groups[groupName] });
                        }
                        var PatDocIds = "", message = "";
                        $.each(DocumentExpiryJSONData, function (i, item) {
                            PatDocIds += item.PatDocId + ',';
                        });
                        $.each(DocumentExpiryData, function (i, item) {
                            if (item.FilePath.length > 1) {
                                message += item.FilePath.join(', ') + ' in Documents is expiring on ' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '.<br />';
                            }
                            else {
                                message += item.FilePath + ' in Documents is expiring on ' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '.<br />';
                            }
                        });
                        utility.kendoAlert("Document Expiry Alert", message, "Go to Documents", function () {
                            var selectDocTab = function () {
                                utility.callbackAfterAllDOMLoaded(function () {
                                    SelectTab("patTabDocuments", "false");
                                })
                            };
                            if (selectedTab.TabID == "schTabCalendar") {
                                appointmentDetail.UnLoad(true);
                                if (PatientId != params.patientID) {
                                    $.when(setPatientBanner(PatientId, "1")).then(function () {
                                        Patient_Demographic.isDocExpiryAlert = false;
                                        utility.callbackAfterAllDOMLoaded(function () {
                                            if (!$("#mstrTabPatient").hasClass("active")) {
                                                SelectTab('mstrTabPatient', 'false');
                                            }
                                            selectDocTab();
                                        })
                                    });
                                }
                                else {
                                    Patient_Demographic.isDocExpiryAlert = false;
                                    utility.callbackAfterAllDOMLoaded(function () {
                                        if (!$("#mstrTabPatient").hasClass("active")) {
                                            SelectTab('mstrTabPatient', 'false');
                                        }
                                        selectDocTab();
                                    })
                                }
                            }
                            else {
                                Patient_Demographic.isDocExpiryAlert = false;
                                utility.callbackAfterAllDOMLoaded(function () {
                                    if (!$("#mstrTabPatient").hasClass("active")) {
                                        SelectTab('mstrTabPatient', 'false');
                                    }
                                    selectDocTab();
                                })
                            }
                        }, "Do nothing", function () {
                            Patient_Document.InsertPatientDocumentExpiryAlert(PatDocIds);
                        });
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                dfd.resolve();
            });
        }
        else {
            console.warn(" patientid is coming null in FillPatientAlertsCount. PatientId:" + PatientId)
        }
        return dfd.promise();
    },

    OpenPatientSearch: function () {
        Patient_Demographic.AutoSaveDemographic();

    },
    PatientSearchOpen: function () {
        if (typeof DefaultMenuSelected != "undefined" && DefaultMenuSelected != null && DefaultMenuSelected == "MDVisionBilling") {
            LoadActionPan('Patient_Search', null)
        }
            //Check if patient is selected in Banner
        else if ($('#PatientProfile #hfPatientId').val()) {
            if (!$("#mstrTabPatient").hasClass("active") && !$("#mstrTabClinical").hasClass("active")) {
                SelectTab('mstrTabPatient', 'false');
            }
            LoadActionPan('Patient_Search', null);
        }
        else {
            if (typeof DefaultMenuSelected != "undefined" && DefaultMenuSelected != null && DefaultMenuSelected == "MDVisionDefault") {
                if (!$("#mstrTabPatient").hasClass("active")) {
                    SelectTab('mstrTabPatient', 'false');
                }
            } else if (typeof DefaultMenuSelected != "undefined" && DefaultMenuSelected != null && DefaultMenuSelected == "MDVisioniTrack") {
                LoadActionPan('Patient_Search', null);

            }
        }
    },

    OpenUploadImage: function () {
        var pid = $("#frmDemographic #hfPractice").val();
        practiceDetail.DemographicPractice(pid).done(function (response) {
            if (response.status != false) {
                var medication_detail = JSON.parse(response.PracticeFill_JSON);
                ScanPrivilige = medication_detail.chkScan;
                OCRPrivilige = medication_detail.chkOCR;
                if (Patient_Demographic.params.PanelID == "pnlDemographic"); {
                    Patient_Demographic.ScanPrivilige = medication_detail.chkScan;
                    Patient_Demographic.OCRPrivilige = medication_detail.chkOCR;
                }
            }
            else {
                ScanPrivilige = false;
                OCRPrivilige = false;
                if (Patient_Demographic.params.PanelID == "pnlDemographic"); {
                    Patient_Demographic.ScanPrivilige = false;
                    Patient_Demographic.OCRPrivilige = false;
                }
            }
            if (ScanPrivilige == "True") {
                var params = [];
                params = Patient_Demographic.params
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "patTabDemographic";
                setDefaultValuesForScanCanvas(500, 360);
                params["PracticeId"] = pid;
                LoadActionPan('uploadImage', params);
            }
            else {
                utility.DisplayMessages("Either practice is not selected or practice does not have privileges to Scan. Please contact your administrator.", 2);
            }
        });
    },

    OpenFamily: function () {
        var params = [];
        params["FamilyId"] = "-1";
        params["FromAdmin"] = "0";
        params["patientID"] = Patient_Demographic.params.patientID;
        params["ParentCtrl"] = "patTabDemographic";

        LoadActionPan('Patient_Family', params);
    },

    OpenPreferences: function () {
        AppPrivileges.GetFormPrivileges("Preferences", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientPreferencesId"] = "-1";
                params["FromAdmin"] = "0";
                params["patientID"] = Patient_Demographic.params.patientID;
                params["GuarantorID"] = Patient_Demographic.params.GuarantorID;
                params["PatWorkTel"] = Patient_Demographic.params.PatWorkTel;
                params["PatCell"] = Patient_Demographic.params.PatCell;
                params["PatHomeTel"] = Patient_Demographic.params.PatHomeTel;
                params["PatEmail"] = Patient_Demographic.params.PatientEmail;
                params["ParentCtrl"] = "patTabDemographic";
                LoadActionPan('Patient_Preferences', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    OpenAdvancePayment: function (isIndependent) {
        var params = [];
        params["PatientAdvancePaymentSearchId"] = "-1";
        params["FromAdmin"] = "0";
        if (Patient_Demographic && Patient_Demographic.params && Patient_Demographic.params.patientID != null && Patient_Demographic.params.patientID != "" && Patient_Demographic.params.patientID != "-1") {
            params["patientID"] = Patient_Demographic.params.patientID;
        } else {
            params["patientID"] = localStorage.getItem("SelectedPatientId");
        }
        //if (isIndependent)
        //    params["ParentCtrl"] = selectedtab.TabID;
        //else
        //    params["ParentCtrl"] = "patTabDemographic";
        var tab = GetCurrentSelectedTab();
        if (tab.TabID.indexOf("mstr") > -1) {
            SelectTab('mstrTabPatient', 'false');
            params["ParentCtrl"] = "patTabDemographic";
        }
        else {
            params["ParentCtrl"] = tab.TabID;
        }
        params["IsPatientDetail"] = "0";
        LoadActionPan('Patient_AdvancePayment', params);
    },
    //Start || 16 April, 2016 || ZeeshanAK || Changes for DOC 34 - Break The Glass
    OpenRestrictUser: function (isIndependent) {
        var params = [];
        //params["PatientAdvancePaymentSearchId"] = "-1";
        params["FromAdmin"] = "0";
        params["patientID"] = Patient_Demographic.params.patientID;
        if (isIndependent)
            params["ParentCtrl"] = selectedtab.TabID;
        else
            params["ParentCtrl"] = "patTabDemographic";
        LoadActionPan('Restrict_User', params);
    },
    //End   || 16 April, 2016 || ZeeshanAK || Changes for DOC 34 - Break The Glass

    BindJSON: function (demographic_detail) {
        //var demographic_detail = JSON.parse(JSONData);

        var self = $('#pnlDemographic #frmDemographic');
        utility.bindMyJSONByName(true, demographic_detail, false, self).done(function () {


            $Ctrl_gr = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtGuarantor");
            $hfCtrl_gr = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfGuarantor");
            //Guarantor
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_gr, $Ctrl_gr.val(), $hfCtrl_gr, $hfCtrl_gr.val());

            $Ctrl_reft = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtProviderReferral");
            $hfCtrl_reft = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfProviderReferral");
            //RefProvider To
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_reft, $Ctrl_reft.val(), $hfCtrl_reft, $hfCtrl_reft.val());


            $Ctrl_ref = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtRefProviderReferral");
            $hfCtrl_ref = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfRefProviderReferral");
            //RefProvider From
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_ref, $Ctrl_ref.val(), $hfCtrl_ref, $hfCtrl_ref.val());

            $Ctrl_ref = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtRefProvider");
            $hfCtrl_ref = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfRefProvider");
            //RefProvider
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_ref, $Ctrl_ref.val(), $hfCtrl_ref, $hfCtrl_ref.val());

            $Ctrl_pcp = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtPCP");
            $hfCtrl_pcp = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfPCP");
            //PCP
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_pcp, $Ctrl_pcp.val(), $hfCtrl_pcp, $hfCtrl_pcp.val());

            $Ctrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtFacility");
            $hfCtrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfFacility");
            //Facility
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl, $Ctrl.val(), $hfCtrl, $hfCtrl.val());

            $Ctrl_p = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtProvider");
            $hfCtrl_p = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfProvider");
            //Provider
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, $Ctrl_p.val(), $hfCtrl_p, $hfCtrl_p.val());

            $Ctrl_PreferedLanguage = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #ddlPrefLanguage");
            $hfCtrl_PreferedLanguage = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfLanguages");
            //Prefered Languages
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_PreferedLanguage, $Ctrl_PreferedLanguage.val(), $hfCtrl_PreferedLanguage, $hfCtrl_PreferedLanguage.val());

            utility.ChangeNameCase($("#" + Patient_Demographic.params.PanelID + " #frmDemographic #ddlCountry"));   //change country text from UpperCase to Title Case
            $Ctrl_Country = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #ddlCountry");
            $hfCtrl_Country = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfCountry");
            //Countries
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_Country, $Ctrl_Country.val(), $hfCtrl_Country, $hfCtrl_Country.val());

            $Ctrl_Cities = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtCity");
            $hfCtrl_Cities = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfCity");
            //Cities
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_Cities, $Ctrl_Cities.val(), $hfCtrl_Cities, $hfCtrl_Cities.val());


            if ($('#pnlDemographic #frmDemographic #ddlHearFrom').val() == '10') {
                $('#pnlDemographic #frmDemographic #divOtherText').removeClass('hidden');
            }
        });

        $('#imgPatientProfile').attr('src', demographic_detail.imgPatient);

        //if (demographic_detail.Active == 'True')
        //    self.find('#chkActive').attr("checked", true);
        //else {
        //    self.find('#chkActive').attr("checked", false);
        //    self.find('#dvresion').show();
        //}
        if (demographic_detail.Active == 0)
            self.find('#dvresion').show();
        else {
            self.find('#chkActive').attr("checked", false);
            self.find('#dvresion').hide();
        }
        if (demographic_detail.BadAddress == 'True')
            self.find('#chkBadAddress').attr("checked", true);
        else
            self.find('#chkBadAddress').attr("checked", false);



        $('#pnlDemographic #frmDemographic').data('serialize', $('#pnlDemographic #frmDemographic').serialize());

    },

    ValidateDemographic: function () {
        $('#pnlDemographic #frmDemographic')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  LastName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  FirstName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Sex: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DOB: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                          date: {
                              format: date_format.toUpperCase(),
                              message: ' '
                          }
                      }
                  },
                  MaritalStatus: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Ethnicity: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  PatientRaceIds: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  PrefLanguage: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Address1: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  City: {
                      group: '.col-xs-8',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  State: {
                      group: '.col-xs-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Zip: {
                      group: '.col-xs-8',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Provider: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Facility: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Practice: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Email: {
                      group: '.col-sm-3',
                      enabled: false,
                      validators: {
                          //emailAddress: {
                          //    message: 'Email not Valid'
                          //},
                          regexp: {
                              regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                              message: 'Email not Valid'
                          }
                      }
                  },
                  RefProviderReferral: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  Status: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  HomeTel: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  //RefProvider: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  //PCP: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  //Guarantor: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},


              }
          })
            .on('keyup', 'input#txtEmail', function (e) {
                var formValidation = $("#pnlDemographic #frmDemographic").data("bootstrapValidator");
                switch ($(this).attr("name")) {
                    case 'Email':
                        var OccurenceCod1Val = $("input#txtEmail").val();
                        if (OccurenceCod1Val != "") {
                            //formValidation.enableFieldValidators('OccurrenceDate1', true);//.revalidateField('OccurrenceDate1');
                            formValidation.enableFieldValidators('Email', true);
                        }
                        else
                            formValidation.enableFieldValidators('Email', false);
                        break;
                    default:
                        break;
                }
            }).on('error.form.bv', function (e, data) {
                // if user update any changes form auto save flow then on error set global variable to its state at time of form loaded
                if (params.IsDemographicInfoGlobalyUpdated) {
                    params.IsDemographicInfoGlobalyUpdated = false;
                    params.DemographicAutoUpdateActiveTab = "Demographic";
                    if (drfdAutoSave)
                        drfdAutoSave.reject();
                }
            })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           // confirm races and ethenticity are not empty
           if (params.IsDemographicInfoGlobalyUpdated == true) {
               if ($('#pnlDemographic #hfRaceIds').val() == '') {
                   Patient_Demographic.validateRace(1);
                   params.IsDemographicInfoGlobalyUpdated = false;
                   params.DemographicAutoUpdateActiveTab = "Demographic";
                   if (drfdAutoSave)
                       drfdAutoSave.reject();
               }
               else if (Patient_Demographic.multipleEthnicityIds == '') {
                   Patient_Demographic.validateEthnicity(1);

                   params.IsDemographicInfoGlobalyUpdated = false;
                   params.DemographicAutoUpdateActiveTab = "Demographic";
                   if (drfdAutoSave) drfdAutoSave.reject();
               }
               else {
                   Patient_Demographic.DemographicSave();
               }
           }
           else {
               Patient_Demographic.DemographicSave();
           }
       });
    },

    VarifyMUAlert: function (DemographicResponse) {
        var PatientId = Patient_Demographic.params.PatientId ? Patient_Demographic.params.PatientId : Patient_Demographic.params.patientID;
        //var obj_ = utility.MU3Demographics("#" + Patient_Demographic.params.PanelID + " #frmDemographic", PatientId, DemographicResponse != null ? DemographicResponse : "");
        //Load MU3 Alerts
        utility.LoadMUAlerts(PatientId, true);
        return;

        // Commented as requirement may be need in future, so do not remove code. PRD-795
        //if (obj_ != null) {
        //    if (Patient_Demographic.params.mode == "Add") {
        //        Patient_Demographic.SaveMUAlert(obj_).done(function (result) {
        //            if (result.status != false) {
        //                utility.toggelMU3Alerts(true);
        //            }
        //            else {
        //                console.log(result.Message);
        //            }
        //        });
        //    }
        //    else {
        //        Patient_Demographic.UpdateMUAlert(obj_).done(function (result) {
        //            if (result.status != false) {
        //                var data = JSON.parse(result.MUAlerts_JSON);
        //                var IsAnyOtherAlert = data.filter(item=>item.PatientId == PatientId && item.IsShowAlert == true);
        //                if (IsAnyOtherAlert.length > 0) {
        //                    utility.toggelMU3Alerts(true, result.MissingDataAlertCount);
        //                }
        //                else {
        //                    utility.toggelMU3Alerts(false, result.MissingDataAlertCount);
        //                }
        //            }
        //            else {
        //                console.log(result.Message);
        //            }
        //        });
        //    }
        //}
    },

    ValidateValues: function () {

        var objDef = $.Deferred();
        //utility.toggelMU3Alerts(true);
        //if (!utility.ValidateAutoComplete($("#pnlDemographic #ddlPrefLanguage"), 'pnlDemographic #hfLanguages', false)) {
        //    return false;
        //}

        //if ($('#pnlDemographic #hfProvider').val() == "-1") {
        //    utility.DisplayMessages("Provider not Valid", 2);
        //    return false;
        //}

        //else if ($('#pnlDemographic #hfFacility').val() == "-1") {
        //    utility.DisplayMessages("Facility not Valid", 2);
        //    return false;
        //}

        if ($('#pnlDemographic #txtRefProvider').val() == "-1") {
            utility.DisplayMessages("Practice not Valid", 2);
            objDef.resolve(false);
        }
        else
            objDef.resolve(true);

        //else if ($('#pnlDemographic #hfRefProvider').val() == "-1" && $('#pnlDemographic #txtRefProvider').val() != "") {
        //    utility.DisplayMessages("Ref. Provider not Valid", 2);
        //    return false;
        //}


        // if ($('#pnlDemographic #hfPCP').val() == "-1" && $('#pnlDemographic #txtPCP').val() != "") {
        //    utility.DisplayMessages("PCP not Valid", 2);
        //    return false;
        //}
        //Edit By Mohsin Nasir Bug # 2901

        //if ($("#pnlDemographic #txtGuarantor").val() != "") {
        //    if (!utility.ValidateAutoComplete($("#pnlDemographic #txtGuarantor"), 'pnlDemographic #hfGuarantor', true)) {
        //        return false;
        //    }
        //}
        //else if ($("#pnlDemographic #txtGuarantor").val() == "") {
        //    utility.ValidateAutoComplete($("#pnlDemographic #txtGuarantor"), 'pnlDemographic #hfGuarantor', true);
        //}


        //    // if ($('#pnlDemographic #hfGuarantor').val() == "-1" && $('#pnlDemographic #txtGuarantor').val() != "") {
        //    //    utility.DisplayMessages("Guarantor not Valid", 2);
        //    //    return false;
        //    //}
        //else
        //    return true;

        //END Edit By Mohsin Nasir Bug # 2901

        return objDef.then(function (res) {
            return res;
        });

    },
    LoadDemographicStateInfo: function()
    {
        utility.FillDemographicCityState('#pnlDemographic #frmDemographic', '#txtZip', '#txtCity', '#hfCity', '#txtState', '#ddlCountry', '#hfCountry', 'zip');
        utility.callbackAfterAllDOMLoaded(function () {
            $('#pnlDemographic #frmDemographic').bootstrapValidator('revalidateField', 'City');
        });
       
    },
    DemographicSave: function () {
        
        var strMessage = "";
        var self = $('#pnlDemographic');
        if (Patient_Demographic.PatientSSN) {

            var txtValue = self.find("#txtSSN").val();   // get ssn value
            if (txtValue) {
                if (globalAppdata.IsFullSSN.toLowerCase() === 'false') {
                    self.find("#txtSSN").val(Patient_Demographic.PatientSSN);
                }
                self.find("#hfPatientSSN").val(Patient_Demographic.PatientSSN);
            }
            else {
                self.find("#txtSSN").val("");
                self.find("#hfPatientSSN").val("");
            }
        }
        var myJSON = self.getMyJSONByName();
        if (!txtValue) {
            txtValue = self.find("#txtSSN").val();
        }
        self.find("#txtSSN").val(txtValue); //hide ssn no.
        //var objMyJSON = JSON.parse(myJSON);
        //objMyJSON.imgBase64 = $('#pnlDemographic #imgPatient').attr('src');
        //myJSON = JSON.stringify(objMyJSON);
        //----------------------------
        var EthnicityIds = self.find('#divPatientEthnicity ul.multiselect-container li input[type=checkbox]:checked').map(function () {
            return this.value;
        }).get().join(',');
        Patient_Demographic.multipleEthnicityIds = EthnicityIds;
        if ($('#pnlDemographic #hfRaceIds').val() != '' && Patient_Demographic.multipleEthnicityIds != '') {
            Patient_Demographic.validateRace(2);
            Patient_Demographic.validateEthnicity(2);
            //----------------------------
            //$('#pnlDemographic').bootstrapValidator('revalidateField', 'DOB');
            if (Patient_Demographic.params.mode == "Add") {
                AppPrivileges.GetFormPrivileges("Demographic", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Patient_Demographic.ValidateValues().done(function (result) {
                            if (result != false) {
                                var objData = JSON.parse(myJSON);
                                objData["strEthnicityIds"] = EthnicityIds;
                                myJSON = JSON.stringify(objData);
                                Patient_Demographic.SaveDemographic(myJSON).done(function (response) {
                                    if (response.status != false) {
                                        Patient_Demographic.params.PatientId = response.PatientId;
                                        Patient_Demographic.VarifyMUAlert();
                                        // $("#txtAccountNo").val(response.AccountNumber);
                                        utility.DisplayMessages(response.message, 1);
                                        if (Patient_Demographic.params.ParentCtrl == "Patient_Search") {
                                            Patient_Search.PatientSearch(response.PatientId);
                                        }
                                        else
                                            SelectPatient(response.PatientId, "");
                                        Patient_Demographic.UnLoad();
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
            else if (Patient_Demographic.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Patient_Demographic.ValidateValues().done(function (result) {
                            if (result != false) {
                                var objData = JSON.parse(myJSON);
                                objData["strEthnicityIds"] = EthnicityIds;
                                myJSON = JSON.stringify(objData);
                                Patient_Demographic.UpdateDemographic(myJSON, Patient_Demographic.params.patientID).done(function (response) {
                                    if (response.status != false) {
                                        Patient_Demographic.IsImageUpdated = '';
                                        Patient_Demographic.params["isFromPictureMode"] = '';
                                        if (Patient_Demographic.params.ParentCtrl == "Patient_Search") {
                                            Patient_Search.PatientSearch(Patient_Demographic.params.patientID);
                                            Patient_Demographic.UnLoad();
                                        }

                                        Patient_Demographic.VarifyMUAlert();

                                        //Patient_Demographic.FillDemographic(Patient_Demographic.params.patientID)
                                        //.done(function (response) {
                                        // store.clear('selectPatientData', Patient_Demographic.params.patientID);
                                        // store.save('selectPatientData', response.DemographicFill_JSON, Patient_Demographic.params.patientID);
                                        //    Patient_Demographic.FillPatientBar(Patient_Demographic.params.patientID);
                                        //});

                                        // when user update record directly From Save button  
                                        if (!params.IsDemographicInfoGlobalyUpdated) {
                                            Patient_Demographic.LoadPatientDemogrphic();
                                            //to update count and grid on dashboardpatientchanges
                                            if ($('#pnlDashboard #frmDashboard #PatientChanges').hasClass('active')) {
                                                DashBoard.DashBoardPatientChangesSearch(null, null, null);
                                            }

                                            //load selected insurance
                                            $("#pnlPatientInsurance #frmPatientInsurance #lstInsurances li.active-plan a").click();
                                            utility.DisplayMessages(response.message, 1);
                                        }
                                        else {      // when data is submitted from auto save flow
                                            if (Patient_Demographic.params["PanelID"] && Patient_Demographic.params["PanelID"].indexOf("pnlDemographic") > -1 && $("#" + Patient_Demographic.params["PanelID"]).is(":visible") == true) {

                                                params.defaultDemographicSerailizeForm = $('#frmDemographic').serialize();
                                                if (drfdAutoSave) drfdAutoSave.resolve();

                                            }
                                        }
                                        if (Patient_Demographic.params.GrandParent == "DashboardAppointment") {
                                            Patient_Demographic.FillPatientInfo(Patient_Demographic.params).done(function () {
                                                DashBoard.NoteCreation(Patient_Demographic.params.patientID, Patient_Demographic.params.AppointmentId, Patient_Demographic.params.ProviderId, Patient_Demographic.params.ProviderName, Patient_Demographic.params.AppointmentTime, Patient_Demographic.params.VisitId, Patient_Demographic.params.VisitDate, Patient_Demographic.params.Reason, Patient_Demographic.params.FacilityName, Patient_Demographic.params.FacilityId, Patient_Demographic.params.Room, Patient_Demographic.params.NotesId);
                                            });
                                        }


                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }//edit end
        } else {
            if ($('#pnlDemographic #hfRaceIds').val() == '')
                Patient_Demographic.validateRace(1);
            if (Patient_Demographic.multipleEthnicityIds == '')
                Patient_Demographic.validateEthnicity(1);
        }
    },

    validateRace: function (operationid) {
        $("#pnlDemographic #frmDemographic #divPatientRace label").find("i").remove();
        if (operationid == 1) {
            $("#pnlDemographic #frmDemographic #divPatientRace .multiselect").css("border-color", "#cc2724");
            $("#pnlDemographic #frmDemographic #divPatientRace").find(".control-label").css("color", "#cc2724");
            $("#pnlDemographic #frmDemographic #divPatientRace").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnlDemographic #frmDemographic #divPatientRace .multiselect").css("border-color", "#3c763d");
            $("#pnlDemographic #frmDemographic #divPatientRace").find(".control-label").css("color", "#000000");
            //$("#pnlDemographic #frmDemographic #divPatientRace").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#pnlDemographic #frmDemographic #divPatientRace .multiselect").css("border-color", "#ccc");
            $("#pnlDemographic #frmDemographic #divPatientRace").find(".control-label").css("color", "#000000");
        }
    },
    validateEthnicity: function (operationid) {
        $("#pnlDemographic #frmDemographic #divPatientEthnicity label").find("i").remove();
        if (operationid == 1) {
            $("#pnlDemographic #frmDemographic #divPatientEthnicity .multiselect").css("border-color", "#cc2724");
            $("#pnlDemographic #frmDemographic #divPatientEthnicity").find(".control-label").css("color", "#cc2724");
            $("#pnlDemographic #frmDemographic #divPatientEthnicity").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Ethnicity" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnlDemographic #frmDemographic #divPatientEthnicity .multiselect").css("border-color", "#ccc");
            $("#pnlDemographic #frmDemographic #divPatientEthnicity").find(".control-label").css("color", "#000000");
            //$("#pnlDemographic #frmDemographic #divPatientEthnicity .multiselect").css("border-color", "#DCDCDO");
            //$("#pnlDemographic #frmDemographic #divPatientEthnicity").find(".control-label").css("color", "#3c763d");
            //$("#pnlDemographic #frmDemographic #divPatientEthnicity").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#pnlDemographic #frmDemographic #divPatientEthnicity .multiselect").css("border-color", "#ccc");
            $("#pnlDemographic #frmDemographic #divPatientEthnicity").find(".control-label").css("color", "#000000");
        }
    },
    CheckEthnicityValidation: function () {
        var self = $('#pnlDemographic');
        var EthnicityIds = self.find('#ddlEthnicity option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strEthnicityIds = EthnicityIds;
        Patient_Demographic.multipleEthnicityIds = strEthnicityIds;
        if (Patient_Demographic.multipleEthnicityIds != '') {
            Patient_Demographic.validateEthnicity(2);
        } else {
            Patient_Demographic.validateEthnicity(1);
        }
    },

    DemographicSavePatientPic: function () {
        var strMessage = "";
        var self = $('#pnlDemographic');
        var myJSON = self.getMyJSONByName();

        //----------------------------
        //$('#pnlDemographic').bootstrapValidator('revalidateField', 'DOB');
        if (Patient_Demographic.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var result;
                    result = true;//Patient_Demographic.ValidateValues();
                    if (result != false) {
                        var patientDocumentImageSrc = $('#frmDemographic #myCanvasUploadImg').attr('src');
                        var patientDocumentImage = "";
                        var objDef = $.Deferred();
                        if (isFileCompressed) {
                            var zip = new JSZip();
                            zip.file("Patient ID Card.jpeg", patientDocumentImageSrc.split(',')[1], { base64: true });
                            zip.generateAsync({ type: "blob", compression: "DEFLATE", compressionOptions: { level: 9 } }).then(function (blob) {
                                var fileReader = new FileReader();
                                fileReader.addEventListener("load", function () {
                                    patientDocumentImage = fileReader.result;
                                    objDef.resolve("ok")
                                }, false);
                                if (blob) {
                                    fileReader.readAsDataURL(blob);
                                }
                            });
                        } else {
                            patientDocumentImage = $('#frmDemographic #myCanvasUploadImg').attr('src');
                            objDef.resolve("ok")
                        }
                        objDef.then(function () {
                            Patient_Demographic.UpdateDemographicPatientPic(myJSON, Patient_Demographic.params.patientID, patientDocumentImage).done(function (response) {
                                if (response.status != false) {
                                    if (Patient_Demographic.params.ParentCtrl == "Patient_Search") {
                                        Patient_Search.PatientSearch(Patient_Demographic.params.patientID);
                                        Patient_Demographic.UnLoad();
                                    }
                                    Patient_Demographic.LoadPatientDemogrphic();
                                    utility.DisplayMessages(response.message, 1);
                                    Patient_Demographic.IsImageUpdated = '';
                                    Patient_Demographic.params["isFromPictureMode"] = '';
                                    $('#frmDemographic #myCanvasUploadImg').attr('src', '');
                                    //to update count and grid on dashboardpatientchanges
                                    //DashBoard.DashBoardPatientChangesSearch(null, null, null);
                                    //load selected insurance
                                    //$("#pnlPatientInsurance #frmPatientInsurance #lstInsurances li.active-plan a").click();
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        });
                    }

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    ViewResion: function (element, panelId) {

        if ($(element).is(':checked')) {
            $("" + panelId + " #dvresion").fadeOut();
        }
        else
            $("" + panelId + " #dvresion").fadeIn();

    },
    // -------------- Provider ---------------------

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmDemographic";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "patTabDemographic";
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#pnlDemographic #hfProvider').val(),'patTabDemographic');
        var params = [];
        params["ProviderId"] = $('#pnlDemographic #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'patTabDemographic';
        LoadActionPan('providerDetail', params);
    },

    HideProviderLink: function () {
        $('#pnlDemographic #txtProvider').attr("ProviderId", "-1");
        $('#pnlDemographic #hfProvider').val("-1");
        $("#pnlDemographic #lnkProviderEdit").css("display", "none");
        $("#pnlDemographic #lblProvider").css("display", "inline");
    },
    // -------------- End Provider -----------------

    // -------------- Facility ---------------------

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmDemographic";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "patTabDemographic";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#pnlDemographic #hfFacility').val(), 'patTabDemographic');
        var params = [];
        params["FacilityId"] = $('#pnlDemographic #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'patTabDemographic';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },

    HideFacilityLink: function () {
        $('#pnlDemographic #txtFacility').attr("FacilityId", "-1");
        $('#pnlDemographic #hfFacility').val("-1");
        $('#pnlDemographic #lnkFacilityEdit').css("display", "none");
        $('#pnlDemographic #lblFacility').css("display", "inline");
    },
    // -------------- End Facility -----------------

    // -------------- Ref Provider -----------------

    FillRefProviderName: function (RefProviderId, RefProviderName) {
        $('#pnlDemographic #txtRefProvider').val(RefProviderName);
        $('#pnlDemographic #hfRefProvider').val(RefProviderId);
        UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"]);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";

        params["ParentCtrl"] = "patTabDemographic";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenRefProviderDetail: function () {
        //Admin_ReferringProvider.ReferringProviderEdit($('#pnlDemographic #hfRefProvider').val(), "patTabDemographic", "txtRefProvider");
        var params = [];
        params["ReferringProviderId"] = $('#pnlDemographic #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = "patTabDemographic";

        LoadActionPan('referringproviderDetail', params);
    },

    HideRefProviderLink: function () {
        $('#pnlDemographic #hfRefProvider').val("-1");
        $('#pnlDemographic #lnkRefProviderEdit').css("display", "none");
        $('#pnlDemographic #lblRefProvider').css("display", "inline");
    },
    // -------------- End Ref Provider -------------

    // -------------- PCP --------------------------
    FillPCPName: function (PCPId, PCPName) {
        $('#pnlDemographic #txtPCP').val(PCPName);
        $('#pnlDemographic #hfPCP').val(PCPId);
        UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"]);
    },

    OpenPCP: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPCP";
        params["ParentCtrl"] = "patTabDemographic";
        params["Title"] = "Search PCP Provider";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenPCPDetail: function () {
        //Admin_ReferringProvider.ReferringProviderEdit($('#pnlDemographic #hfPCP').val(), 'patTabDemographic', 'txtPCP');
        var params = [];
        params["ReferringProviderId"] = $('#pnlDemographic #hfPCP').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPCP";
        params["mode"] = "Edit";
        params["Title"] = "PCP";
        params["ParentCtrl"] = "patTabDemographic";

        LoadActionPan('referringproviderDetail', params);
    },

    HidePCPLink: function () {
        $('#pnlDemographic #hfPCP').val("-1");
        $('#pnlDemographic #lnkPCPEdit').css("display", "none");
        $('#pnlDemographic #lblPCP').css("display", "inline");
    },
    // -------------- End PCP ----------------------

    //// -------------- Guarantor ------------

    OpenGuarantor: function () {
        var params = [];
        params["GuarantorId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "patTabDemographic";
        params["RefCtrl"] = "txtGuarantor";
        params["Address1"] = $('#' + Patient_Demographic.params.PanelID + ' #txtAddress1').val();
        params["Zip"] = $('#' + Patient_Demographic.params.PanelID + ' #txtZip').val();
        params["City"] = $('#' + Patient_Demographic.params.PanelID + ' #txtCity').val();
        params["State"] = $('#' + Patient_Demographic.params.PanelID + ' #txtState').val();
        LoadActionPan('Patient_Guarantor', params);
    },

    OpenGuarantorDetail: function () {
        //Patient_Guarantor.GuarantorEdit($('#pnlDemographic #hfGuarantor').val(), 'patTabDemographic');
        var params = [];
        params["GuarantorId"] = $('#pnlDemographic #hfGuarantor').val();
        params["mode"] = "Edit";
        params["RefCtrl"] = "txtGuarantor";
        params["ParentCtrl"] = 'patTabDemographic';
        params["Address1"] = $('#' + Patient_Demographic.params.PanelID + ' #txtAddress1').val();
        params["Zip"] = $('#' + Patient_Demographic.params.PanelID + ' #txtZip').val();
        params["City"] = $('#' + Patient_Demographic.params.PanelID + ' #txtCity').val();
        params["State"] = $('#' + Patient_Demographic.params.PanelID + ' #txtState').val();
        params["PatientId"] = Patient_Demographic.params.patientID;
        LoadActionPan('guarantorDetail', params);
    },

    HideGuarantorLink: function () {
        $('#pnlDemographic #hfGuarantor').val("-1");
        $('#pnlDemographic #lnkGuarantorEdit').css("display", "none");
        $('#pnlDemographic #lblGuarantor').css("display", "inline");
    },
    //// -------------- End Guarantor ------------

    CalculateAge: function (ev) {
        try {
            if ($(ev).val().length == 10 && utility.isValidDate($(ev).val()) && Date.parse($(ev).val())) {
                if (new Date($(ev).val()) > new Date()) {
                    $(ev).val('');
                }
                else {
                    Patient_Demographic.FillAge($(ev).val()).done(function (response) {
                        if (response.status != false) {
                            $('#pnlDemographic #txtAge').val(response.ActualAge);
                        } else {
                            $('#pnlDemographic #txtAge').val('');
                        }
                    });
                }
            } else {
                $('#pnlDemographic #txtAge').val('');

            }
        }
        catch (ex) {
            console.log(ex);
        }

    },
    RemoveUploadImage: function () {
        utility.myConfirm('Are you sure to delete the picture ?', function () {
            $('#' + Patient_Demographic.params.PanelID + " #frmDemographic #imgPatient").attr("src", "Content/images/default_male_profile.gif");
            var self = $('#' + Patient_Demographic.params.PanelID);
            var myJSON = self.getMyJSONByName();

            // AST 504
            Patient_Demographic.IsImageUpdated = true;
            Patient_Demographic.UpdateDemographic(myJSON, Patient_Demographic.params.patientID, null, null, null, true).done(function (response) {
                Patient_Demographic.IsImageUpdated = '';
                Patient_Demographic.params["isFromPictureMode"] = '';
                Patient_Demographic.LoadPatientDemogrphic();

                $('#' + Patient_Demographic.params.PanelID + " #frmDemographic #imgPatient").css('cursor', 'default');
                utility.DisplayMessages("Successfully Deleted", 1);
            });
        }
          , function () {
              //NO CALLBACK
          },
   'Confirm Delete'

   //
);


    },

    SaveMUAlert: function (AlertData) {

        return { status: false };
        // commented as per requirement, may need in future so do not remove code. PRD-795
        //var objData = {};
        //objData["MUAlerts"] = AlertData;
        //objData["CommandType"] = "save_mu_alert";
        //var data = JSON.stringify(objData);
        //return MDVisionService.PMSAPIService(data, "Patient", "MUAlert");
    },

    UpdateMUAlert: function (AlertData, IsFromNote) {


        var objData = {};
        objData["IsFromNote"] = IsFromNote == true ? true : false;
        objData["MUAlerts"] = AlertData;
        objData["CommandType"] = "update_mu_alert";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "MUAlert");
    },

    SaveDemographic: function (DemographicData, isDemographicQuick, strRaceIds, Imagedata, Filetype, Filename) {

        if (isDemographicQuick == "" || isDemographicQuick == null || isDemographicQuick == undefined)
            isDemographicQuick = "false";

        var objData = JSON.parse(DemographicData);
        objData["IsDemographicQuick"] = isDemographicQuick;
        objData["CommandType"] = "SAVE_DEMOGRAPHIC";
        objData["strRaceIds"] = strRaceIds ? strRaceIds : $('#pnlDemographic #hfRaceIds').val();
        objData["imagedata"] = Imagedata;
        objData["filetype"] = Filetype;
        objData["filename"] = Filename;
        objData["foldername"] = "130";
        // objData["foldername"] = scannerjson.ddlFolder;
        //objData["ScannerDOS"] = scannerjson.dtpDOS;
        //objData["AssignUserto"] = scannerjson.ddlAssignUserto;
        //objData["ScannerComments"] = scannerjson.txtComments;
        //objData["AssignedToName"] = scannerjson.ddlAssignUserto_text;
        //objData["imgPatient"] = $('#SearchPatient #pnlDemographic #imgPatient').attr('src');
        objData["imgPatient"] = objData["imgBase64"];
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },

    UpdateDemographicPatientPic: function (DemographicData, PatientID, patientDocumentImage) {

        var objData = JSON.parse(DemographicData);
        objData["PatientID"] = PatientID;
        objData["PatientDocumentImage"] = patientDocumentImage;
        objData["CommandType"] = "update_patient_demographic_patpic";
        if ($('#pnlDemographic #imgPatient').attr('src')) {
            objData["imgPatient"] = $('#pnlDemographic #imgPatient').attr('src');;
        }
        else {
            objData["imgPatient"] = patientDocumentImage;
        }
        objData["strRaceIds"] = $('#pnlDemographic #hfRaceIds').val();

        objData["IsImageUpdated"] = Patient_Demographic.IsImageUpdated;
        if ($('#pnlDemographic #hfReferralID').val() == "") {
            //if (Patient_Demographic.params.mode == "Edit" && $('#pnlDemographic #hfReferralID').val() == "") {
            objData["ReferralId"] = -1;
        }
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },

    UpdateDemographic: function (DemographicData, PatientID, strRaceIds, FromDetail, strEthnicityIDs, IsFromDelete) {

        var objData = JSON.parse(DemographicData);
        objData["PatientID"] = PatientID;
        objData["CommandType"] = "update_patient_demographic";
        if (!IsFromDelete)
            objData["imagedata"] = $('#frmDemographic #myCanvasUploadImg').attr('src');
        if (FromDetail == "1") {
            objData["imgPatient"] = $('#pnldemographicDetail #imgPatient').attr('src');
        } else {
            objData["imgPatient"] = $('#pnlDemographic #imgPatient').attr('src');
            if (Patient_Demographic.params["isFromPictureMode"]) {
                objData["imagedata"] = $('#pnlDemographic #imgPatient').attr('src');
            }
        }



        objData["IsImageUpdated"] = Patient_Demographic.IsImageUpdated;
        if (!strEthnicityIDs) {
            var self = $('#pnlDemographic #frmDemographic');
            strEthnicityIDs = self.find('#divPatientEthnicity ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                return this.value;
            }).get().join(',');
        }
        Patient_Demographic.multipleEthnicityIds = strEthnicityIDs;
        objData["strEthnicityIds"] = strEthnicityIDs;
        objData["strRaceIds"] = strRaceIds ? strRaceIds : $('#pnlDemographic #hfRaceIds').val();

        if ($('#pnlDemographic #hfReferralID').val() == "") {
            //if (Patient_Demographic.params.mode == "Edit" && $('#pnlDemographic #hfReferralID').val() == "") {
            objData["ReferralId"] = -1;
        }
        if (Patient_Demographic.careGiverIds == "") {
            objData["CareGiverIds"] = -1;
        }
        else {
            objData["CareGiverIds"] = Patient_Demographic.careGiverIds;
        }
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },

    FillDemographic: function (PatientID) {

        var objData = new Object();
        objData["PatientID"] = PatientID;
        objData["CommandType"] = "fill_patient_demographic";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },

    FillAge: function (BirthDate) {

        var objData = new Object();
        objData["BirthDate"] = BirthDate;
        objData["CommandType"] = "CALCULATE_AGE";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },

    UnLoad: function () {
        // when Patient Demographic is open in modal.
        if (!params.IsDemographicInfoGlobalyUpdated && params.DemographicAutoUpdateActiveTab) {
            delete params.DemographicAutoUpdateActiveTab;
        }
        if (Patient_Demographic.params != null && Patient_Demographic.params.ParentCtrl != "") {
            UnloadActionPan(Patient_Demographic.params.ParentCtrl);
        }
        else
            UnloadActionPan();
    },

    changeGender: function (ev) {
        var imageSrc = $('#frmDemographic  #imgPatient').attr('src');
        if (imageSrc == 'Content/images/default_male_profile.gif' || imageSrc == 'Content/images/default_female_profile.gif') {
            switch ($(ev.selectedOptions).text().toLowerCase()) {
                case "male":
                    $('#frmDemographic  #imgPatient').attr('src', 'Content/images/default_male_profile.gif');
                    $('#imgPatientProfile').attr('src', 'Content/images/default_male_profile.gif');
                    $('#' + Patient_Demographic.params.PanelID + " #frmDemographic #imgPatient").css('cursor', 'default');
                    break;
                case "female":
                    $('#frmDemographic  #imgPatient').attr('src', 'Content/images/default_female_profile.gif');
                    $('#imgPatientProfile').attr('src', 'Content/images/default_female_profile.gif');
                    $('#' + Patient_Demographic.params.PanelID + " #frmDemographic #imgPatient").css('cursor', 'default');
                    break
            }
        }
    },
    setImageSource: function (sourceString) {
        $('#frmDemographic  #imgPatient').attr('src', sourceString);
        $('#' + Patient_Demographic.params.PanelID + " #frmDemographic #imgPatient").css('cursor', 'pointer');
        $('#imgPatientProfile').attr('src', sourceString);
    },
    isChangeInDemographic: function (mode) {
        if ($('#frmDemographic').serialize() != $('#frmDemographic').data('serialize')) {
            Patient_Demographic.isChange = true;
            utility.myConfirm('12', function () {
                if (mode != undefined)
                    Patient_Demographic.params.mode = mode;

                Patient_Demographic.DemographicSave();
            }, function () {

            });

        }

    },

    // ----------- Open demographic from Patient Banner (IrFan) --------------

    OpenPatientDemographic: function (PatientID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                //SelectTab('mstrTabPatient', 'false');

                var params = [];
                params["mode"] = 'Edit';
                params["PatBanner"] = true;
                //params["ParentCtrl"] = Patient_Demographic.params.TabID;
                params["patientID"] = PatientID;
                params["FromAdmin"] = "0";
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    UpdateBalancesInBanner: function (InsBalance, PatBalance, AdvanceBalance, collBalance, PrimaryInsuranceName, PrimaryInsuranceId) {

        var selectedPatientId = Patient_Demographic.params.patientID == null ? params.patientID : Patient_Demographic.params.patientID;
        var InsBalance;
        var PatBalance;
        var AdvanceBalance;


        if (selectedPatientId && selectedPatientId != null) {

            //if balances are being sent in arguments then set the sent values in markup
            if (InsBalance != null && PatBalance != null && AdvanceBalance != null) {

                InsBalance = utility.convertToFigure(InsBalance, true);

                PatBalance = utility.convertToFigure(PatBalance, true);

                AdvanceBalance = utility.convertToFigure(AdvanceBalance, true);

                collBalance = utility.convertToFigure(collBalance, true);

                var InsuranceBalanceMarkup = "<a href='#' onclick=Patient_Demographic.OpenOutstandingVisit('" + selectedPatientId + "','0',false);>" + InsBalance + "</a>"
                var PatientBalanceMarkup = "<a href='#' onclick=Patient_Demographic.OpenOutstandingVisit('" + selectedPatientId + "','1',false);>" + PatBalance + "</a>"
                var PatientAdvanceBalanceMarkup = "<a href='#' onclick=Patient_Demographic.OpenAdvancePayment(true);>" + AdvanceBalance + "</a>"
                var PatientCollBalanceMarkup = "<a href='#' onclick=Patient_Demographic.OpenOutstandingVisit('" + selectedPatientId + "','0',true);>" + collBalance + "</a>"
                if (globalAppdata["PBPrimaryInsurance"] == "True") {
                    $("#PatientProfile #PriInsurance").html("<strong>Pri. Ins:</strong> " + PrimaryInsuranceName);
                } else {
                    $("#PatientProfile #PriInsurance").html("");
                }
                $("#PatientProfile #hfPatientInsuranceId").val(PrimaryInsuranceId);
                if (globalAppdata["PBPatientBalance"] == "True") {
                    $("#PatientProfile #PatBalance").html("<strong>Pat. Bal:</strong> " + PatientBalanceMarkup);
                    var patientBalance = parseFloat(PatBalance.substr(1));
                    $('#hfPatientBalance').val(patientBalance);
                    if (patientBalance > 0 && globalAppdata.sendBillingInquiry == "True") {
                        $('#sendBillingInquiry').removeClass('hidden');
                    }
                    else {
                        $('#sendBillingInquiry').addClass('hidden');
                    }
                } else {
                    $("#PatientProfile #PatBalance").html("");
                    $('#sendBillingInquiry').addClass('hidden');
                }
                if (globalAppdata["PBPlanBalance"] == "True") {
                    $("#PatientProfile #InsBalance").html("<strong>Ins. Bal:</strong> " + InsuranceBalanceMarkup);
                } else {
                    $("#PatientProfile #InsBalance").html("");
                }
                if (globalAppdata["PBPatientAdvanceBalance"] == "True") {
                    $("#PatientProfile #PatAdvanceBalance").html("<strong>Adv. Bal:</strong> " + PatientAdvanceBalanceMarkup);
                } else {
                    $("#PatientProfile #PatAdvanceBalance").html("");
                }
                if (globalAppdata["IsCollection"].toLowerCase() == "true" && globalAppdata["IsPBCollection"].toLowerCase() == "true") {
                    $("#PatientProfile #CollBalance").html("<strong>Coll. Bal:</strong> " + PatientCollBalanceMarkup);
                } else {
                    $("#PatientProfile #CollBalance").html("");
                }
                if (Patient_Demographic.EnrollmentInfoId > 0 && globalAppdata["PBPatientCCMTimer"] == "True") {
                    var ccmHubBtn = $("#PatientProfile #lblPatientData #lnkccmHub");
                    var timer = $("#PatientProfile #lblPatientData #ccmTimer");
                    if (ccmHubBtn.length > 0 && timer.length == 0) {
                        var html = Patient_Demographic.GetTimerHtml(Patient_Demographic.EnrollmentInfoId);
                        $(html).insertAfter($(ccmHubBtn));
                    }
                }
                else {
                    $("#PatientProfile #lblPatientData #ccmTimer").remove();
                }
            }


                // call the DB and get the latest balances
            else {

                //var strMessage = "";
                //AppPrivileges.GetFormPrivileges("Demographic", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                //    if (strMessage == "") {
                Patient_Demographic.FillDemographic(selectedPatientId).done(function (response) {
                    if (response.status != false) {

                        var PatientProfileInfo = JSON.parse(response.DemographicFill_JSON);

                        InsBalance = PatientProfileInfo.InsuranceBalance;
                        PatBalance = PatientProfileInfo.PatientBalance;
                        AdvanceBalance = PatientProfileInfo.AdvanceBalance;
                        PrimaryInsuranceId = PatientProfileInfo.PatientInsuranceId
                        //setting the value

                        InsBalance = utility.convertToFigure(InsBalance, true);

                        PatBalance = utility.convertToFigure(PatBalance, true);

                        AdvanceBalance = utility.convertToFigure(AdvanceBalance, true);

                        var InsuranceBalanceMarkup = "<a href='#' onclick=Patient_Demographic.OpenOutstandingVisit('" + selectedPatientId + "','0');>" + InsBalance + "</a>"
                        var PatientBalanceMarkup = "<a href='#' onclick=Patient_Demographic.OpenOutstandingVisit('" + selectedPatientId + "','1');>" + PatBalance + "</a>"
                        var PatientAdvanceBalanceMarkup = "<a href='#' onclick=Patient_Demographic.OpenAdvancePayment(true);>" + AdvanceBalance + "</a>"

                        if (globalAppdata["PBPlanBalance"] == "True") {
                            $("#PatientProfile #InsBalance").html("<strong>Ins. Bal:</strong> " + InsuranceBalanceMarkup);
                        } else {
                            $("#PatientProfile #InsBalance").html("");
                        }
                        if (globalAppdata["PBPatientBalance"] == "True") {
                            $("#PatientProfile #PatBalance").html("<strong>Pat. Bal:</strong> " + PatientBalanceMarkup);
                            var patientBalance = parseFloat(PatBalance.substr(1));
                            $('#hfPatientBalance').val(patientBalance);
                            if (patientBalance > 0 && globalAppdata.sendBillingInquiry == "True") {
                                $('#sendBillingInquiry').removeClass('hidden');
                            }
                            else {
                                $('#sendBillingInquiry').addClass('hidden');
                            }
                        } else {
                            $("#PatientProfile #PatBalance").html("");
                        }

                        $("#PatientProfile #hfPatientInsuranceId").val(PrimaryInsuranceId);
                        if (globalAppdata["PBPatientAdvanceBalance"] == "True") {
                            $("#PatientProfile #PatAdvanceBalance").html("<strong>Adv. Bal:</strong> " + PatientAdvanceBalanceMarkup);
                        } else {
                            $("#PatientProfile #PatAdvanceBalance").html("");
                        }

                        //if (globalAppdata["PBPhone1"] == "True") {
                        //    if (PatientProfileInfo.Cell != "") {
                        //        $("#PatientProfile #Phone1").html("<strong>Phone 1:</strong> " + PatientProfileInfo.Cell);
                        //    }
                        //} else {
                        //    $("#PatientProfile #Phone1").html("");
                        //}
                        //if (globalAppdata["PBPhone2"] == "True") {
                        //    if (PatientProfileInfo.HomeTel != "") {
                        //        $("#PatientProfile #Phone2").html("<strong>Phone 2:</strong> " + PatientProfileInfo.HomeTel);
                        //    }
                        //} else {
                        //    $("#PatientProfile #Phone2").html("");
                        //}
                        if (globalAppdata["PBPCP"] == "True") {
                            if (PatientProfileInfo.PCP != "") {
                                $("#PatientProfile #PCP").html("<strong>PCP:</strong> " + PatientProfileInfo.PCP);
                            }
                        } else {
                            $("#PatientProfile #PCP").html("");
                        }
                        if (globalAppdata["PBPreferredPhone"] == "1") {
                            if (PatientProfileInfo.HomeTel != "") {
                                $("#PatientProfile #PreferredPhone").html("<strong>Home Tel:</strong> " + PatientProfileInfo.HomeTel);
                            } else {
                                $("#PatientProfile #PreferredPhone").html("");
                            }
                        } else if (globalAppdata["PBPreferredPhone"] == "2") {
                            if (PatientProfileInfo.Cell != "") {
                                $("#PatientProfile #PreferredPhone").html("<strong>Cell:</strong> " + PatientProfileInfo.Cell);
                            } else {
                                $("#PatientProfile #PreferredPhone").html("");
                            }
                        } else if (globalAppdata["PBPreferredPhone"] == "3") {
                            if (PatientProfileInfo.WorkTel != "") {
                                $("#PatientProfile #PreferredPhone").html("<strong>Work Tel:</strong> " + PatientProfileInfo.WorkTel);
                            } else {
                                $("#PatientProfile #PreferredPhone").html("");
                            }
                        }
                        else {
                            $("#PatientProfile #PreferredPhone").html("");
                        }
                        if (globalAppdata["PBRefProvider"] == "True") {
                            if (PatientProfileInfo.RefProvider != "") {
                                $("#PatientProfile #RefProvider").html("<strong>Ref Provider:</strong> " + PatientProfileInfo.RefProvider);
                            }
                        } else {
                            $("#PatientProfile #RefProvider").html("");
                        }
                        if (globalAppdata["PBPatientAdvanceBalance"] == "True") {
                            $("#PatientProfile #PatAdvanceBalance").html("<strong>Adv. Bal:</strong> " + PatientAdvanceBalanceMarkup);
                        } else {
                            $("#PatientProfile #PatAdvanceBalance").html("");
                        }
                        if (globalAppdata["PBPrimaryInsurance"] == "True") {
                            $("#PatientProfile #PriInsurance").html("<strong>Pri. Ins:</strong> " + PatientProfileInfo.InsuranceName);
                        } else {
                            $("#PatientProfile #PriInsurance").html("");
                        }
                        if (Patient_Demographic.EnrollmentInfoId > 0 && globalAppdata["PBPatientCCMTimer"] == "True") {
                            var ccmHubBtn = $("#PatientProfile #lblPatientData #lnkccmHub");
                            var timer = $("#PatientProfile #lblPatientData #ccmTimer");
                            if (ccmHubBtn.length > 0 && timer.length == 0) {
                                var html = Patient_Demographic.GetTimerHtml(Patient_Demographic.EnrollmentInfoId);
                                $(html).insertAfter($(ccmHubBtn));
                            }
                        }
                        else {
                            $("#PatientProfile #lblPatientData #ccmTimer").remove();
                        }
                    }

                });
                //    }
                //    else {
                //        utility.DisplayMessages(strMessage, 2);
                //    }
                //});


            }
        }



    },

    BindGuarantor: function () {
        var Ctrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtGuarantor");
        var func = function () { return utility.GetGuarontorArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfGuarantor");
        var onChange = function () { Patient_Demographic.CommunicationValidation(Ctrl); };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, null, onChange);
    },

    PatientPortal: function () {

        AppPrivileges.GetFormPrivileges("Patient Portal Account", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientId"] = Patient_Demographic.params.patientID;
                params["ParentCtrl"] = "patTabDemographic";
                params["PatientFirstName"] = Patient_Demographic.params.PatientFirstName;
                params["PatientLastName"] = Patient_Demographic.params.PatientLastName;
                params["PatientDOB"] = Patient_Demographic.params.PatientDOB;
                params["PatientEmail"] = Patient_Demographic.params.PatientEmail;
                params["PatientPortalStatus"] = Patient_Demographic.params.PatientPortalStatus;
                //-------------------

                params["ProviderFName"] = Patient_Demographic.ProviderFName;
                params["ProviderLName"] = Patient_Demographic.ProviderLName;
                params["FacilityAddress"] = Patient_Demographic.FacilityAddress;
                params["FacilityCity"] = Patient_Demographic.FacilityCity;
                params["FacilityState"] = Patient_Demographic.FacilityState;
                params["FacilityZip"] = Patient_Demographic.FacilityZip;
                params["FacilityZipExt"] = Patient_Demographic.FacilityZipExt;
                params["FacilityPhone"] = Patient_Demographic.FacilityPhone;
                params["ZipCode"] = $("#pnlDemographic #txtZip").val();

                //-------------------
                if (Patient_Demographic.params.PatientPortalStatus == "0")
                    params["mode"] = "Add";
                else if (Patient_Demographic.params.PatientPortalStatus == "1")
                    params["mode"] = "Edit";

                LoadActionPan('Patient_AccountManager', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    /*************
     * //CacheManager.BindCodes('GetGuarantor', false).done(function (result) {
        //    $("input#txtGuarantor").autocomplete({
        //        autoFocus: true,
        //        source: Guarantors, // pass an array
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                $("#pnlDemographic #hfGuarantor").val(ui.item.id); // add the selected id
        //                if ($("#pnlDemographic #lnkGuarantorEdit").css("display") == "none") {
        //                    $("#pnlDemographic #lnkGuarantorEdit").css("display", "inline");
        //                    $("#pnlDemographic #lblGuarantor").css("display", "none");
        //                }
        //            }, 100);

        //        }
        //    });
        //});
     */

    GetAccountManagerPriviliges: function () {
        //AppPrivileges.GetFormPrivileges("Patient Portal", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {

        //        $('#pnlDemographic #btnPatientPortal').removeAttr("style");

        //    } else {
        //        $('#pnlDemographic #btnPatientPortal').css("display", "none");
        //    }
        //});

    },
    getPrivileges: function () {
        var DetailInfoObj = [];
        var objDetail = {};
        var FormName = [];
        var Permission = [];
        FormName.push("ClinicalDecisionSupport_Clinical Decision Support");
        FormName.push("Demographic");
        FormName.push("Patient Portal Account");
        FormName.push("Restrict User");
        Permission.push("VIEW");
        Permission.push("VIEW");
        Permission.push("VIEW");
        Permission.push("VIEW");
        objDetail["FormName"] = FormName;
        objDetail["Permission"] = Permission;
        // DetailInfoObj.push(objDetail);
        var data = JSON.stringify(objDetail);
        return MDVisionService.APIService(data, "FormPrivilege", "AppPrivileges");
    },

    //-------------------
    BindRefProvider: function () {
        var Ctrl = $('#pnlDemographic #txtRefProvider');
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnlDemographic #hfRefProvider");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    //-------------------
    BindPCPProvider: function () {
        var Ctrl = $('#pnlDemographic #txtPCP');
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnlDemographic #hfPCP");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    CheckCallTextPreferences: function () {
        var homeTel = $('#pnlDemographic #txtHomeTel').val();
        homeTel = homeTel.replace(/[_\W]+/g, "");
        var workTel = $('#pnlDemographic #txtWorkTel').val();
        workTel = workTel.replace(/[_\W]+/g, "");
        var cellTel = $('#pnlDemographic #txtCell').val();
        cellTel = cellTel.replace(/[_\W]+/g, "");
        if (Patient_Demographic.params.IsPreferencesExists == true) {
            if (homeTel == "" && workTel == "" && cellTel == "") {
                utility.DisplayMessages('You will not be able to receive appointment reminders if you remove this number.', 3);
            }
        }

    },
    hideShowTextbox: function () {
        if ($("#pnlDemographic #ddlHearFrom").val() == 1) {
            var bootstrapValidator = $('#frmDemographic').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('RefProviderReferral', true);

            //var bootstrapValidator = $('#frmDemographic').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('Status', true);

            Patient_Demographic.getPatientReferral();
            $("#pnlDemographic #incomingReferral").removeClass('hidden');
        }
        else {
            $("#pnlDemographic #incomingReferral").addClass('hidden');

            var bootstrapValidator = $('#frmDemographic').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('RefProviderReferral', false);

            //var bootstrapValidator = $('#frmDemographic').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('Status', false);

        }

        if ($("#pnlDemographic #ddlHearFrom").val() == 10) {
            $("#pnlDemographic #divOtherText").removeClass('hidden');
        }
        else {
            $("#pnlDemographic #txtOtherText").val('');
            $("#pnlDemographic #divOtherText").addClass('hidden');
        }
        if ($("#pnlDemographic #hfReferralID").val())
            $("#pnlDemographic #divIncomingReferralInfo #btnScanResult,#btnViewAttachment").removeClass("disableAll");
        else
            $("#pnlDemographic #divIncomingReferralInfo #btnScanResult,#btnViewAttachment").addClass("disableAll");

    },

    CommunicationValidation: function (obj) {
        if (Patient_Demographic.CommunicateWithGurrantor.toLowerCase() == "true" && $(obj).val() == "") {
            utility.DisplayMessages("Communication with Guarantor active", 3);
        }
    },

    ///Referrals -- Added by Humaira Yousaf ///

    OpenRefProviderReferral: function () {
        var params = [];
        params["ReferringProviderIdReferral"] = "-1";
        params["FromAdmin"] = "0";
        params["RefForm"] = "frmDemographic";
        params["IsOptional"] = false;
        params["RefCtrl"] = "txtRefProviderReferral";
        params["ParentCtrl"] = "patTabDemographic";
        params["RefCtrlHidden"] = "hfRefProviderReferral";
        LoadActionPan('Admin_ReferringProvider', params);
    },
    OpenProviderReferral: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmDemographic";
        params["ProviderIdReferral"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "patTabDemographic";
        params["RefCtrl"] = "txtProviderReferral";
        params["RefCtrlHidden"] = "hfProviderReferral";
        LoadActionPan('Admin_Provider', params);
    },

    BindRefProviderReferral: function () {
        var Ctrl = $('#pnlDemographic #txtRefProviderReferral');
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnlDemographic #hfRefProviderReferral");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindRefProviderTo: function (isFullName, shortName) {
        var Ctrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtProviderReferral");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfProviderReferral");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    OpenAssignee: function () {
        CacheManager.BindCodes('GetUsersFullName', true).done(function (result) {
            var Ctrl = $("#pnlDemographic #frmDemographic #txtAssignee");
            var hfCtrl = $("#pnlDemographic #frmDemographic  #hfAssignee");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", GetUsersFullName, null, hfCtrl);
        });
    },

    getPatientReferral: function () {
        utility.CreateDatePicker("pnlDemographic #dtDate", function () {
        }, true);

        $('#pnlDemographic #tmTime').timepicker({
            defaultTime: new Date()
        });

        CacheManager.BindDropDownsByID("#pnlDemographic #ddlInsuranceRef", 'GetPatientInsurance', true, Patient_Demographic.params.patientID).done(function () {
            if ($("#pnlDemographic").find("#ddlInsuranceRef option").length > 1) {
                $($("#pnlDemographic").find("#ddlInsuranceRef option")[1]).prop('selected', true);
            }
        });

        $("#pnlDemographic #divIncomingReferralInfo #attachDiv").append('<ul id="menuAttach" class="dropdown-menu" aria-labelledby="btnScanResult">' +
               '<li><a href="#" onclick="Patient_Demographic.documentScan()">Scan</a></li><li><a href="#" onclick="Patient_Demographic.documentImport()">Upload</a></li></ul>');

        $("#pnlDemographic #divIncomingReferralInfo #attachmentDiv").html('<a id="btnViewAttachment" href="#" class="dropdown-toggle btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="Patient_Demographic.loadAttachments()">View Attachment</a><ul id="menuViewAttachment" class="dropdown-menu height-max250 Of-a" aria-labelledby="btnViewAttachment"></ul>');

        Patient_Demographic.fillPatientReferralInfo();
    },
    fillPatientReferralInfo: function (params) {

        var patientId = Patient_Demographic.params.patientID;
        var referralId = $("#pnlDemographic #hfReferralID").val();

        var dfd = new $.Deferred();

        Patient_Demographic.fillPatientReferralInfo_DbCall(patientId, referralId).done(function (response) {
            if (response.status != false) {

                var self = $("#frmDemographic");
                var referralJSON = JSON.parse(response.ReferralListLoad_JSON);
                utility.bindMyJSONByName(true, referralJSON, false, self).done(function () {
                    if ($("#pnlDemographic #hfReferralID").val())
                        $("#pnlDemographic #divIncomingReferralInfo #btnScanResult,#btnViewAttachment").removeClass("disableAll");
                    else
                        $("#pnlDemographic #divIncomingReferralInfo #btnScanResult,#btnViewAttachment").addClass("disableAll");
                });

                $Ctrl_reft = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtProviderReferral");
                $hfCtrl_reft = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfProviderReferral");
                utility.SetKendoAutoCompleteSourceforValidate($Ctrl_reft, $Ctrl_reft.val(), $hfCtrl_reft, $hfCtrl_reft.val());

                $Ctrl_ref = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtRefProviderReferral");
                $hfCtrl_ref = $("#" + Patient_Demographic.params.PanelID + " #frmDemographic #hfRefProviderReferral");
                utility.SetKendoAutoCompleteSourceforValidate($Ctrl_ref, $Ctrl_ref.val(), $hfCtrl_ref, $hfCtrl_ref.val());

                $Ctrl = $("#pnlDemographic #frmDemographic #txtAssignee");
                $hfCtrl = $("#pnlDemographic #frmDemographic  #hfAssignee");
                utility.SetKendoAutoCompleteSourceforValidate($Ctrl, $Ctrl.val(), $hfCtrl, $hfCtrl.val());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                return dfd.promise();
            }
        });

        return dfd.promise();
    },
    fillPatientReferralInfo_DbCall: function (patientId, referralId) {

        var objData = new Object();
        objData["PatientID"] = patientId;
        objData["ReferralId"] = referralId;
        objData["CommandType"] = "fill_patient_referral";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },

    documentScan: function () {
        AppPrivileges.GetFormPrivileges("Documents", "SCAN", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var param = [];
                param["mode"] = "Scan";
                param["RefCtrl"] = "IncomingReferral";
                param['ReferralId'] = $("#pnlDemographic #hfReferralID").val();
                param['RefModuleName'] = "Incoming Referral";
                param['TransitionId'] = $("#pnlDemographic #hfReferralID").val();
                param['patientID'] = $('#PatientProfile #hfPatientId').val();
                param["ParentCtrl"] = Patient_Demographic.params["TabID"];
                LoadActionPan('Document_Scan', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    documentImport: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];

                var AccountNo = $("#PatientProfile #hfAccountNo").val();
                var PatientFullName = $("#PatientProfile #hfPatientFullName").val();
                var PatientId = $("#PatientProfile #hfPatientId").val();
                var PatientName = "";
                if (PatientFullName.length > 0) {
                    var Firstname = PatientFullName.split(" ")[1];
                    var Lastname = PatientFullName.split(" ")[0];
                    var MiddleInitial = PatientFullName.split(" ")[2];
                    PatientName = Lastname + " " + Firstname + " " + MiddleInitial;
                }
                params["patientId"] = $('#PatientProfile #hfPatientId').val();
                params["RefCtrl"] = "IncomingReferral";
                params['ReferralId'] = $("#pnlDemographic #hfReferralID").val();
                params['RefModuleName'] = "Incoming Referral";
                params['TransitionId'] = $("#pnlDemographic #hfReferralID").val();
                params["FromAdmin"] = "0";
                params["mode"] = "Add";
                params["ParentCtrl"] = Patient_Demographic.params["TabID"];
                params["PatientName"] = PatientName;
                LoadActionPan('Document_Import', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    loadAttachments: function (controlName, radiologyOrderId, resultId, tableId) {

        Patient_Demographic.loadAttachments_DbCall().done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var ulAttachment = null;

                if (controlName == null)
                    ulAttachment = $("#pnlDemographic #menuViewAttachment");
                else {
                    var control = eval(controlName.trim());
                    if (tableId != null) {

                        ulAttachment = $('#' + control.params.PanelID + " #" + tableId.trim() + " tr td").find('div.dropdown').find("#menuViewAttachment" + resultId);
                        if ($('#' + control.params.PanelID + " #" + tableId.trim()).parent() != null) {
                            $('#' + control.params.PanelID + " #" + tableId.trim()).parent().removeClass('Of-a');
                        }
                    }
                }
                $(ulAttachment).empty();
                if (response.AttachmentCount > 0) {
                    var attachments = JSON.parse(response.AttachmentLoad_JSON);

                    $(attachments).each(function (index, item) {
                        if (controlName == null)
                            $(ulAttachment).append('<li><a href="#" id="' + item.PatDocId + '" onclick="Patient_Demographic.showAttachment(\'' + item.PatDocId + '\',event)">' + item.ModifiedOn + '</a></li>');
                        else {
                            var onClick = controlName.trim() + ".showAttachment";
                            $(ulAttachment).append('<li><a href="#" id="' + item.PatDocId + '" onclick="' + onClick + '(\'' + item.PatDocId + '\',event,this);">' + item.ModifiedOn + '</a></li>');
                        }
                    });

                }
                else {
                    $(ulAttachment).append('<li><a href="#">No Attachment Found</a></li>');
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    loadAttachments_DbCall: function () {

        var objData = {};
        objData["TransitionId"] = $("#pnlDemographic #hfReferralID").val();
        objData["RefModuleName"] = "Incoming Referral";
        objData["PatientId"] = $("div#PatientProfile #hfPatientId").val();

        objData["commandType"] = "load_attachments";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },

    showAttachment: function (PatDocID, event) {
        $('#divIncomingReferralInfo #attachmentDiv').removeClass('open');
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientID"] = $('#PatientProfile #hfPatientId').val();
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = Patient_Demographic.params["TabID"];

                LoadActionPan('Document_Viewer', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    //end referrals ///


    //CCM Functions (START)

    EnrollForCCM: function (PatientId) {

        var params = [];

        params["PatientId"] = PatientId;
        params["FromAdmin"] = "0";
        params["mode"] = "add";
        var tab = GetCurrentSelectedTab();
        if (tab.TabID.indexOf("mstr") > -1) {
            SelectTab('mstrTabPatient', 'false');
            params["ParentCtrl"] = "patTabDemographic";
        }
        else {
            params["ParentCtrl"] = tab.TabID;
        }

        LoadActionPan('CCMEnrollmentInfo', params);
    },

    OpenCCMHub: function (EnrollmentInfoId, PatientId, IsFromNote) {
        var params = [];
        params["EnrollmentInfoId"] = EnrollmentInfoId;
        params["PatientId"] = PatientId;
        params["IsFromNote"] = IsFromNote;
        var tab = GetCurrentSelectedTab();
        if (tab.TabID.indexOf("mstr") > -1) {
            SelectTab('mstrTabPatient', 'false');
            params["ParentCtrl"] = "patTabDemographic";
        }
        else {
            params["ParentCtrl"] = tab.TabID;
        }

        LoadActionPan('CCM_Patient_Hub', params);
    },
    //CCM Functions (END)
    IsDeathSelected: function (ddlStatus) {
        if ($(ddlStatus).val() == 2) {
            if ($("#dtpDateOfDeath").val() == "" || $("#dtpDateOfDeath").val() == null) {
                $("#divDateOfDeath").removeClass('hidden');
            }
        }
        else {
            $("#divDateOfDeath").addClass('hidden');
            $("#dtpDateOfDeath").val("");
            $("#txtCauseOfDeath").val("");

        }
        if ($(ddlStatus).val() == 0) {
            $("#dvresion").show();
        }
        else {
            $("#dvresion").hide();
        }
    },
    DeathDateChange: function () {
        var bootstrapValidator = $('#frmDemographic').data('bootstrapValidator');
        // bootstrapValidator.revalidateField("DateOfDeath");
    },
    // check either user made any changes on demographic/insuranc screen
    AutoSaveDemographic: function () {
        var formId = "#pnlDemographic #frmDemographic";
        if (params.DemographicAutoUpdateActiveTab == "Insurance")
            formId = "#pnlPatientInsurance #frmPatientInsurance";
        var formCtr = $(formId);
        if (params.DemographicAutoUpdateActiveTab) {  // check active tab is   Demographic/Insurance 

            if (formCtr.serialize() != params.defaultDemographicSerailizeForm && params.IsDemographicInfoGlobalyUpdated == false) {

                var ScreenName = params.DemographicAutoUpdateActiveTab;
                if (params.DemographicAutoUpdateActiveTab == "Insurance")
                    ScreenName = "Patient Insurances";
                utility.myConfirm('Are you sure you want to save ' + ScreenName + ' changes?', function () {
                    params.IsDemographicInfoGlobalyUpdated = true;  //confirm active tab is updated
                    delete params.DemographicAutoUpdateActiveTab;
                    if (formId == "#pnlPatientInsurance #frmPatientInsurance")
                        $("#pnlPatientInsurance #frmPatientInsurance").trigger("submit");
                    else
                        $("#pnlDemographic #frmDemographic").trigger("submit");
                    if (params.IsDemographicInfoGlobalyUpdated) {
                        params.IsDemographicInfoGlobalyUpdated = false;
                        Patient_Demographic.PatientSearchOpen();
                    }
                }, function () {
                    params.IsDemographicInfoGlobalyUpdated = true;  //confirm active tab is updated
                    delete params.DemographicAutoUpdateActiveTab;
                    //  set active tab is inactive and its data is  updated without any changes and open other click tab 
                    if (params.DemographicAutoUpdateActiveTab == "Demographic")
                        Patient_Demographic.LoadPatientDemogrphic();
                    else Patient_Insurance.LoadInsuranceList();
                    Patient_Demographic.PatientSearchOpen();
                },
                                   'Confirm Change'
                               );


            }
            else {
                Patient_Demographic.PatientSearchOpen();
            }
        }
        else {
            Patient_Demographic.PatientSearchOpen();
        }
    },
    sendBillingInquiryEmail: function () {
        Patient_Demographic.BillingInquiryEmailSend().done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
            } else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
    },

    BillingInquiryEmailSend: function () {
        var objData = {};
        objData["hfPatientBalance"] = $("#PatientProfile #PatBalance").find("a").text();
        objData["hfPatientFullNameOnly"] = $("#PatientProfile #banner_PatientName").text();
        objData["hfAccountNo"] = $("#PatientProfile #hfAccountNo").val();
        objData["hfPatientProviderId"] = $("#PatientProfile #hfPatientProviderId").val();
        objData["hfPatientFacilityId"] = $("#PatientProfile #hfPatientFacilityId").val();
        objData["hfPatientId"] = $("#PatientProfile #hfPatientId").val();
        objData["hfPatientProviderName"] = $("#PatientProfile #hfPatientProviderName").val();
        objData["CommandType"] = "SEND_BILLING_INQUIRY_EMAIL";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "SendBillingInquiryEmail");
    },

    IntializeMultiSelectDropDownCareGiver: function () {
        try {
            $('#' + Patient_Demographic.params.PanelID + ' #ddlCareGiver').multiselect('destroy');
            $('#' + Patient_Demographic.params.PanelID + " #ddlCareGiver").removeClass('disabled');
            $('#' + Patient_Demographic.params.PanelID + ' #ddlCareGiver').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: 247,
                onChange: function (element, checked) {
                    var self = $('#pnlDemographic');
                    var options = $(element).parent().find('option');
                    var Selectedoptions = $(element).parent().find('option:selected');
                    if (Selectedoptions.length > 3) {
                        utility.DisplayMessages("Maximum three Care Givers can be selected.", 1);
                        var id = $(element).val();
                        var chk = self.find('#divPatientCareGiver ul.multiselect-container li input[type=checkbox][value="' + id + '"]');
                        $(chk).prop('checked', false);
                        $(chk).parents('li').removeClass('active');
                        $(element).removeAttr('selected');
                        $('#' + Patient_Demographic.params.PanelID + " #ddlCareGiver").multiselect('updateButtonText');
                        return;
                    }
                    $('#' + Patient_Demographic.params.PanelID + " #ddlCareGiver").multiselect('updateButtonText');
                    var CareGiverIds = self.find('#divPatientCareGiver ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                        return this.value;
                    }).get().join(',');
                    Patient_Demographic.careGiverIds = CareGiverIds;
                }
            });
            $('#' + Patient_Demographic.params.PanelID + " #ddlCareGiver").val("");

            $('#pnlDemographic').find('#divPatientCareGiver ul.multiselect-container li input[type=checkbox]').each(function () {
                if ($(this).attr('refval') == "hidden") {
                    $(this).parent().addClass('text-bold');
                }
            });
        }
        catch (ex) {
            console.log(ex);
        }
    },

    // Start CCM Timer
    GetTimerHtml: function (EnrollmentInfoId) {

        var timer = '<div id="ccmTimer" class="timer-container col-sm-4 p-none">' +
                     '<div class="recording-time small col-xs-5 p-none">' +
                         '<div id="divTaskHours" class="col-xs-4"><input type="number" value="0" id="txtTaskHours" name="TaskHours" min="0" max="23" placeholder="H"></div>' +
                         '<div id="divTaskMinutes" class="col-xs-4 dot"><input type="number" value="0" id="txtTaskMinutes" name="TaskMinutes" min="0" max="59" placeholder="M"></div>' +
                         '<div id="divTaskSeconds" class="col-xs-4"><input type="number" value="0" id="txtTaskSeconds" name="TaskSeconds" min="10" max="59" placeholder="S"></div>' +
                     '</div>' +
                     '<div class="text-center col-xs-3">' +
                         '<a data-toggle="tooltip" data-original-title="Start" id="btnRecord" onclick="Patient_Demographic.RecordTaskTime();" class="btn btn-xs btn-circle border pl-xs pr-xs font-xxs black"><i class="fa fa-play"></i></a>' +
                         '<a id="btnStop" data-toggle="tooltip" data-original-title="Pause" onclick="Patient_Demographic.StopTaskTime();" class="btn btn-xs btn-circle border pl-xs pr-xs font-xxs black"><i class="fa fa-pause"></i></a>' +
                         '<a id="btnReset" data-toggle="tooltip" data-original-title="Reset" onclick="Patient_Demographic.ResetTaskTime();" class="btn btn-xs btn-circle border pl-xs pr-xs font-xxs black bold">R</a>' +
                     '</div>' +
                     '<div class="text-center col-xs-4 p-none">' +
                         "<button type='button' class='btn btn-default btn-xs tab_space' onclick=Patient_Demographic.SaveTaskTime('" + EnrollmentInfoId + "');>Save</button>" +
                         "<button type='button' onclick=Patient_Demographic.OpenCallHistory('" + EnrollmentInfoId + "'); class='btn btn-success btn-xs'>Show Log</button>" +
                     '</div>' +
                   '</div>';

        return timer;
    },

    RecordTaskTime: function () {
        var self = $("#PatientProfile #lblPatientData #ccmTimer");
        self.find("#btnRecord").addClass("disableAll");
        self.find("#btnReset").addClass("disableAll");

        var hoursControl = self.find("#txtTaskHours");
        var minutesControl = self.find("#txtTaskMinutes");
        var secondsControl = self.find("#txtTaskSeconds");
        var miliSecondsControl = self.find("#txtTaskMiliSeconds");

        hoursControl.addClass("disableAll");
        minutesControl.addClass("disableAll");
        secondsControl.addClass("disableAll");

        var hours = hoursControl.val();
        var minutes = minutesControl.val();
        var seconds = secondsControl.val();

        Patient_Demographic.ticker = setInterval(function () {

            if (seconds == 60) {
                seconds = 0;
                minutes++;
            }
            if (minutes == 60) {
                minutes = 0;
                hours++;
            }
            seconds++;

            hoursControl.val(hours);
            minutesControl.val(minutes);
            secondsControl.val(seconds);

        }, 1000);
    },

    StopTaskTime: function () {
        var self = $("#PatientProfile #lblPatientData #ccmTimer");
        clearInterval(Patient_Demographic.ticker);
        self.find("#btnRecord").removeClass("disableAll");
        self.find("#btnReset").removeClass("disableAll");
        self.find("#txtTaskHours").removeClass("disableAll");
        self.find("#txtTaskMinutes").removeClass("disableAll");
        self.find("#txtTaskSeconds").removeClass("disableAll");
    },

    ResetTaskTime: function () {
        var self = $("#PatientProfile #lblPatientData #ccmTimer");
        self.find("#txtTaskHours").val(0);
        self.find("#txtTaskMinutes").val(0);
        self.find("#txtTaskSeconds").val(0);
    },

    SaveTaskTime: function (EnrollmentInfoId) {
        var self = $("#PatientProfile #lblPatientData #ccmTimer");

        var myJSON = self.getMyJSONByName();
        var objData = JSON.parse(myJSON);

        Patient_Demographic.TaskTimeSave(myJSON, EnrollmentInfoId).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                Patient_Demographic.ResetTaskTime();
            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },

    TaskTimeSave: function (TaskTimeData, EnrollmentInfoId) {
        var objData = JSON.parse(TaskTimeData);
        objData["EnrollmentInfoId"] = EnrollmentInfoId;
        objData["PatientId"] = $("#PatientProfile #hfPatientId").val();
        objData["TaskTime"] = new Date().toLocaleTimeString();
        objData["TaskDate"] = new Date();
        objData["CallerType"] = "Provider";
        objData["Caller"] = $("#PatientProfile #hfPatientProviderId").val();
        objData["ReceiverName"] = $("#PatientProfile #hfPatientFullNameOnly").val();
        objData["AddedBy"] = globalAppdata.AppUserNameFullName;
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMProgramUpdate", "SaveCCMTaskTime");
    },

    OpenCallHistory: function (EnrollmentInfoId) {
        var params = [];
        //params["ParentCtrl"] = 'Patient_Demographic';
        params["EnrollmentInfoId"] = EnrollmentInfoId;
        params["FromAdmin"] = "0";
        params["PatientId"] = $("#PatientProfile #hfPatientId").val();

        var tab = GetCurrentSelectedTab();
        if (tab.TabID.indexOf("mstr") > -1) {
            SelectTab('mstrTabPatient', 'false');
            params["ParentCtrl"] = "patTabDemographic";
        }
        else {
            params["ParentCtrl"] = tab.TabID;
        }
        setTimeout(function () {
            LoadActionPan('CCMCallDetailsHistory', params);
        }, 600);
    },
    //End CCM Timer
    PrintDemographic: function (CallerFormId) {
        var params = [];
        var self = '';
        var ParentCtrlPanelId = null;
        if (CallerFormId == 'PatDemographic') {
            params["PatientId"] = $('#PatientProfile #hfPatientId').val();
            params["DemographicsJSON"] = $('#pnlDemographic').getMyJSONByName();
            params["LabelsJSON"] = $('#pnlDemographic').find('label').map(function () {
                return this.textContent.split('*')[0];
            }).get().join(',');
        }
        else {
            params["PatientId"] = demographicDetail.params.patientID;
            params["ParentCtrl"] = 'demographicDetail';
            params["DemographicsJSON"] = $('#pnldemographicDetail').getMyJSONByName();
            ParentCtrlPanelId = demographicDetail.params.PanelID;
        }
        params["PreviewPrintFor"] = "Demographics";
        LoadActionPan('Patient_Demographic_PrintView', params, ParentCtrlPanelId);
    },
}