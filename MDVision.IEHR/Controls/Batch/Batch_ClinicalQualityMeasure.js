Batch_ClinicalQualityMeasure = {
    bIsFirstLoad: true,
    params: [],
    LastSctBaseSearch: '',
    Load: function(params) {
        Batch_ClinicalQualityMeasure.params = params;

        if (Batch_ClinicalQualityMeasure.bIsFirstLoad) {
            Batch_ClinicalQualityMeasure.bIsFirstLoad = false;
            $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #lblBasicSearch").hide();
            
            var self;
            if (Batch_ClinicalQualityMeasure.params["PanelID"] !== "pnlBatchClinicalQualityMeasure")
                self = $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure");
            else
                self = $("#pnlBatchClinicalQualityMeasure");

            self.loadDropDowns(true).done(function () {
                $($('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #ddlAgeCondition option")[6]).remove();
                utility.CreateDatePicker(Batch_ClinicalQualityMeasureDetail.params.PanelID + " #dtpDateFrom, #dtpDateTo, #dtpDateFrom", function () {}, false);
                utility.ValidateFromToDate('frmBatchClinicalQualityMeasure', 'dtpDateFrom', 'dtpDateTo', true);
                $('#dtpDateFrom').datepicker("setDate", '01/01/2016');
                $('#dtpDateTo').datepicker("setDate", '12/31/2016');
                $.when(Batch_ClinicalQualityMeasure.LoadingDropDowns('pnlBatchClinicalQualityMeasure')).then(function () {
                    $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #ddlEthnicity").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        maxHeight: 230,
                    });
                    $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #ddlRace").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        maxHeight: 230,
                    });

                });
            });
            
        }
        Batch_ClinicalQualityMeasure.BindProvider();
        Batch_ClinicalQualityMeasure.BindPatientName();
        Batch_ClinicalQualityMeasure.LoadInsurancePlan();
        Batch_ClinicalQualityMeasure.initializeKeypad();
    },

    initializeKeypad: function () {
        var self = "";
        self = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #frmBatchClinicalQualityMeasure');
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
            $.each(ParentRacesArray, function (j, result) {
                if (!result.ParentId) {
                    htmlRace += '<option refval="hidden" value="' + result.Id + '">' + result.Name + (result.Code ? (" - " + result.Code) : '') + '</option>';
                    var childRaces = $.grep(races, function (a) {
                        return a.ParentId == result.Id;
                    });
                    $.each(childRaces, function (j, itm) {
                        htmlRace += '<option value="' + itm.Id + '">' + itm.Name + (itm.Code ? (" - " + itm.Code) : '') + '</option>';
                    });
                }
                else
                    htmlRace += '<option value="' + result.Id + '">' + result.Name + (result.Code ? (" - " + result.Code) : '') + '</option>';
            });
            $.each(ParentEthnicityArray, function (j, result) {
                if (!result.ParentId) {
                    htmlEthnicity += '<option refval="hidden" value="' + result.Id + '">' + result.Name + (result.Code ? (" - " + result.Code) : '') + '</option>';
                    var childEthnicity = $.grep(ethnicities, function (a) {
                        return a.ParentId == result.Id;
                    });
                    $.each(childEthnicity, function (j, itm) {
                        htmlEthnicity += '<option value="' + itm.Id + '">' + itm.Name + (itm.Code ? (" - " + itm.Code) : '') + '</option>';
                    });
                }
                else
                    htmlEthnicity += '<option value="' + result.Id + '">' + result.Name + (result.Code ? (" - " + result.Code) : '') + '</option>';
            });
            $('#' + PanelId + ' #ddlRace').html(htmlRace);
            $('#' + PanelId + ' #ddlEthnicity').html(htmlEthnicity);
            BackgroundLoaderShow(false);
            dfd.resolve();
        });
        return dfd;
    },
    
    ResetNPI: function () {
        $("#pnlBatchClinicalQualityMeasure #hfProviderId").val("");
        $("#pnlBatchClinicalQualityMeasure #NPItxt").val("");
    },

    ResetProvider: function () {
        $("#" + Batch_ClinicalQualityMeasure.params.PanelID + " #txtProvider").val('');
        $("#" + Batch_ClinicalQualityMeasure.params.PanelID + " #hfProviderId").val('');
        if ($("#pnlBatchClinicalQualityMeasure #frmBatchClinicalQualityMeasure #lnkProviderEdit").css("display") == "inline") {
            $("#pnlBatchClinicalQualityMeasure #frmBatchClinicalQualityMeasure #lnkProviderEdit").css("display", "none");
            $("#pnlBatchClinicalQualityMeasure #frmBatchClinicalQualityMeasure #lblProvider").css("display", "inline");
        }
    },

    /******** Patient auto-complete start *******/
    BindPatientName: function () {
        var Ctrl = $("#pnlBatchClinicalQualityMeasure #frmBatchClinicalQualityMeasure #txtFullName");
        var func = function () { return utility.GetPatientArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#pnlBatchClinicalQualityMeasure #hfPatientId");
        var onSelect = function (e) {
            $("#pnlBatchClinicalQualityMeasure #hfPatientAccntNum").val(e.AccountNumber);
            utility.InsertRecentPatient(e.id);
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "FullName", "contains", null, func, hfCtrl, onSelect);
    },

    OpenPatient: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Batch_ClinicalQualityMeasure';
        LoadActionPan('Patient_Search', params);
    },
    
    FillPatientInfoFromSearch: function (PatientId, AccountNumber, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var self = null;
        self = $("#pnlBatchClinicalQualityMeasure");
        self.find("#hfPatientId").val(PatientId);
        self.find("#txtFullName").val(patFullName);
        self.find("#hfPatientAccntNum").val(AccountNumber);

        $Ctrl = $("#" + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #txtFullName");
        $hfCtrl = $("#" + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #hfPatientId");
        //Patient
        if ($Ctrl.data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl, patFullName, $hfCtrl, PatientId, "FullName");

        UnloadActionPan("Batch_ClinicalQualityMeasure");
    },
    /******** Patient auto-complete end *******/

    /******** Insurance Plan auto-complete start *******/
    OpenInsurancePlanDetail: function () {
        var params = [];
        var PanelID = Batch_ClinicalQualityMeasure.params["PanelID"];
        params["ParentCtrl"] = 'Batch_ClinicalQualityMeasure';
        params["InsurancePlanId"] = $("#pnlBatchClinicalQualityMeasure #hfInsurancePlan").val();
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        LoadActionPan('insurancePlanDetail', params, PanelID);
    },

    OpenInsurancePlan: function () {
        var params = [];
        var PanelID = Batch_ClinicalQualityMeasure.params["PanelID"];
        params["ParentCtrl"] = 'Batch_ClinicalQualityMeasure';
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        LoadActionPan('Admin_InsurancePlan', params, PanelID);
    },

    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
        utility.SetKendoAutoCompleteSourceforValidate($("#pnlBatchClinicalQualityMeasure #txtInsurancePlan"), InsurancePlanName, $("#pnlBatchClinicalQualityMeasure #hfInsurancePlan"), InsurancePlanId);
        $("#pnlBatchClinicalQualityMeasure #lnkInsurancePlanDetail").css("display", "inline");
        $("#pnlBatchClinicalQualityMeasure #lblInsurancePlan").css("display", "none");
        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);
    },

    LoadInsurancePlan: function () {
        CacheManager.BindCodes('GetInsurancePlan', false).done(function (response) {
            var Ctrl = $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " input#txtInsurancePlan");
            var hfCtrl = $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #hfInsurancePlan");
           utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", InsurancePlans, null, hfCtrl);
        });
    },
    /******** Insurance Plan auto-complete end *******/

    /******** Provider auto-complete start *******/
    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["RefCtrlHidden"] = "hfProviderId";
        params["ParentCtrl"] = "Batch_ClinicalQualityMeasure";
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #hfProviderId').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'Batch_ClinicalQualityMeasure';
        LoadActionPan('providerDetail', params);
    },

    BindProvider: function () {
        var Ctrl = $("#pnlBatchClinicalQualityMeasure #frmBatchClinicalQualityMeasure #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnlBatchClinicalQualityMeasure #hfProviderId");
        var onChange = function () { utility.FillProviderNPI('#pnlBatchClinicalQualityMeasure #frmBatchClinicalQualityMeasure', '#hfProviderId', '#NPItxt'); };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, null, onChange);
    },
    /******** Provider auto-complete end *******/

    ShowSearchFields: function (obj) {
        obj = $(obj);
        if (obj.text() == "Advance Search") {
            obj.text('Advance Search').hide();
            $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #lblBasicSearch").show();
            $('#' + Batch_ClinicalQualityMeasure.params["PanelID"] + ' div[divtype="AdvancedSearch"]').each(function () {
                $(this).css("display", "inline");
            });

            if ($('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #ddlAgeCondition").val() == "") {
                $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #ageConditionValueMonth").addClass("hidden");
                $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #ageConditionValueYear").addClass("hidden");
            }

            if ($("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #lnkInsurancePlanDetail").css("display") != "none") {
                $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #lnkInsurancePlanDetail").css("display", "none");
                $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #lblInsurancePlan").css("display", "inline");
            }
        }
        else if (obj.text() == "Basic Search") {
            obj.text('Basic Search').hide();
            $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #lblAdvanceSearch").show();
            $('#' + Batch_ClinicalQualityMeasure.params["PanelID"] + ' div[divtype="AdvancedSearch"]').each(function () {
                $(this).find('[type=hidden],[type=text], textarea, ul').each(function () {
                    $(this).val('');
                });

                $(this).find('[type=checkbox], [type=radio]').each(function () {
                    this.checked = false;
                });

                $(this).find('select').each(function () {
                    $(this).find('option:selected').removeAttr('selected');
                    $(this).find('option[value=""]').attr('selected', 'selected');
                });

                $(this).css("display", "none");
            });
            $('#' + Batch_ClinicalQualityMeasure.params["PanelID"] + ' #ddlEthnicity').multiselect('clearSelection');
            $('#' + Batch_ClinicalQualityMeasure.params["PanelID"] + ' #ddlRace').multiselect('clearSelection');
            $('#' + Batch_ClinicalQualityMeasure.params["PanelID"] + ' #ulCQMProblemList').empty();
        }
    },

    /******** Problem auto-complete start *******/
    BindICD9AutoComplete: function (element) {
        if ($(element).val().length > 3) {

            if ($(element).val().substring(0, 3).toLowerCase() == "sct") {
                Batch_ClinicalQualityMeasure.LastSctBaseSearch = $(element).val().substring(3, $(element).val().length);
            }
            else {
                Batch_ClinicalQualityMeasure.LastSctBaseSearch = "";
            }
        }
        else {
            Batch_ClinicalQualityMeasure.LastSctBaseSearch = "";
        }

        $('#pnlBatchClinicalQualityMeasure #txtProblems').attr("data-popupunload", "false");

        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Batch_ClinicalQualityMeasure", null, false);
    },

    OpenProblemSearch: function () {
        var controlToLoad = "";
        controlToLoad = "Admin_IMOICD";
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Batch_ClinicalQualityMeasure";
        params["RefCtrl"] = "txtProblems";

        $('#pnlBatchClinicalQualityMeasure #txtProblems').attr('data-popupunload', 'true');

        params["Parent"] = 'pnlAdminIMOICD';
        HiddenCtrl = 'hfICD1-1,hfICDDescription1-1,hfICD101-1,hfICD10Description1-1,hfSNOMED1-1,hfSNOMEDDescription1-1';
        params["RefHiddenCtrl"] = HiddenCtrl;
        LoadActionPan(controlToLoad, params);
    },

    deleteProblem: function (obj, ev) {
        ev.stopPropagation();
        $(obj).parent().remove();
        if ($('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #ulCQMProblemList li").length > 0) {
            $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #ulCQMProblemList li:first select").remove();
        }
    },
    /******** Problem auto-complete start *******/

    addAgeConditionValues: function () {
        var ageCondition = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #ddlAgeCondition").val();
        if (ageCondition == "") {
            $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #ageConditionValueMonth").addClass("hidden");
            $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #ageConditionValueYear").addClass("hidden");
            var ageToValue = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #frmBatchClinicalQualityMeasure input[id*=txtAgeValueMonth]').val('');
            var ageToValue = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #frmBatchClinicalQualityMeasure input[id*=txtAgeValueYear]').val('');
        }
        else {
            $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #ageConditionValueMonth").removeClass("hidden");
            $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #frmBatchClinicalQualityMeasure #ageConditionValueYear").removeClass("hidden");
        }
    },

    BatchClinicalQualityMeasureSearch: function(cqmid, pageNo, rpp) {

        if ($("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result").css("display") === "none")
            $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result").show();

        var self = $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Search");
        var myJsonByName = self.getMyJSONByName();
        var objData = JSON.parse(myJsonByName);

        var AgeInYears = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #txtAgeValueYear').val() * 12;
        var AgeInMonths = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #txtAgeValueMonth').val();
        var calAge = "";
        if (AgeInYears !="" && AgeInMonths != "") {
             calAge =parseInt(AgeInMonths, 10) + parseInt(AgeInYears, 10) ;
        } else if (AgeInYears != "" && AgeInMonths == "") {
            calAge = parseInt(AgeInYears, 10);
        } else if (AgeInYears == "" && AgeInMonths != "") {
            calAge = parseInt(AgeInMonths, 10);
        }
        objData["Age"] = calAge;

        objData["ProviderId"] = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #hfProviderId').val();
        objData["PatientId"] = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #hfPatientId').val();
        //objData["InsurancePlanId"] = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #hfInsurancePlan').val();
        var problemData = [];
        $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #ulCQMProblemList li").each(function (index, item) {
            var problem = null;
                problem = {
                    ICD10: $(this).attr('icd10code'),
                    SnomedCode: $(this).attr('snomedcode'),
                };
            problemData.push(problem);
        });
        objData["Problems"] = problemData;
        var myJson = JSON.stringify(objData);

        if ($("#ddlprovider :selected").val() == "" && $("#dtpDateFrom").val() == "" && $("#dtpDateTo").val() == "") {
            utility.DisplayMessages("Specify a Criteria to Filter", 3);
        } else {
            Batch_ClinicalQualityMeasure.SearchBatchClinicalQualityMeasure(myJson, cqmid, pageNo, rpp).done(function (response) {

            if (response.status != false) {
                Batch_ClinicalQualityMeasure.BatchClinicalQualityMeasureGridLoad(response);

                    var tableControl = Batch_ClinicalQualityMeasure.params["PanelID"] + " #dgvBatchClinicalQualityMeasure";
                    var pagingPanelControlId = Batch_ClinicalQualityMeasure.params["PanelID"] + " #divBatchClinicalQualityMeasurePaging";
                    var classControlName = "Batch_ClinicalQualityMeasure";
                    var pagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout
                    (
                        CreatePagination(response.BatchClinicalQualityMeasureCount, pageNo, 30, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords,
                            function (primaryId, pageNumber, resultPerPage) {
                                Batch_ClinicalQualityMeasure.BatchClinicalQualityMeasureSearch(primaryId, pageNumber, 30);
                    }
                        ), 10);
            } else {
                    $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #dgvBatchClinicalQualityMeasure").dataTable().fnDestroy();
                    $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result #dgvBatchClinicalQualityMeasure tbody").find("tr").remove();
                    $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #divBatchClinicalQualityMeasurePaging").remove();

                    utility.DisplayMessages(response.Message, 3);
            }
            });
        }
    },

    BatchClinicalQualityMeasureExport: function () {

        if ($("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result").css("display") === "none")
            $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result").show();

        var self = $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Search");
        var myJson = self.getMyJSON();

        if ($("#dgvBatchClinicalQualityMeasure tr").length > 1) {
            Batch_ClinicalQualityMeasure.BatchClinicalQualityMeasureExport_(myJson).done(function(response) {
                if (response.status != false) {
                    download("data:application/octet-stream;base64," + response.DownloadFile, response.FileName + ".xml", "application/octet-stream");

                } else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        } else {
            utility.DisplayMessages("No Record Found to Export.", 3);
        }
    },

    ClinicalQualityMeasureDetail: function(mode, cqmid, providerId, measure, iP, event) {
        if (event != null)
            event.stopPropagation();

        var self = $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Search");
        var myJsonByName = self.getMyJSONByName();
        var objData = JSON.parse(myJsonByName);

        var AgeInYears = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #txtAgeValueYear').val() * 12;
        var AgeInMonths = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #txtAgeValueMonth').val();
        var calAge = "";
        if (AgeInYears != "" && AgeInMonths != "") {
            calAge = parseInt(AgeInMonths, 10) + parseInt(AgeInYears, 10);
        } else if (AgeInYears != "" && AgeInMonths == "") {
            calAge = parseInt(AgeInYears, 10);
        } else if (AgeInYears == "" && AgeInMonths != "") {
            calAge = parseInt(AgeInMonths, 10);
        }

        objData["Age"] = calAge;
        objData["CQMId"] = cqmid;
        objData["ProviderId"] = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #hfProviderId').val();
        objData["PatientId"] = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #hfPatientId').val();
        objData["PatientAccountNumber"] = $("#pnlBatchClinicalQualityMeasure #hfPatientAccntNum").val();
        var problemData = [];
        $('#' + Batch_ClinicalQualityMeasure.params.PanelID + " #ulCQMProblemList li").each(function (index, item) {
            var problem = null;
            problem = {
                ICD10: $(this).attr('icd10code'),
                SnomedCode: $(this).attr('snomedcode'),
            };
            problemData.push(problem);
        });
        objData["Problems"] = problemData;
        var myJson = JSON.stringify(objData);

        var temp = JSON.parse(myJson);
        temp["Sex_text"] = temp["Sex_text"] == "- Select -" ? "" : temp["Sex_text"];
        temp["AgeCondition_text"] = temp["AgeCondition_text"] == "- Select -" ? "" : temp["AgeCondition_text"];
        temp["ProviderTypeId_text"] = temp["ProviderTypeId_text"] == "- Select -" ? "" : temp["ProviderTypeId_text"];
        myJson = JSON.stringify(temp);

        var dateFrom = $("#dtpDateFrom").val();
        var dateTo = $("#dtpDateTo").val();
        var params = [];
        params["mode"] = mode;
        params["CQMID"] = cqmid;
        params["providerId"] = providerId;
        params["Measure"] = measure;
        params["ParentCtrl"] = "batchTabClinicalQualityMeasure";
        params["dateFrom"] = dateFrom;
        params["dateTo"] = dateTo;
        params["iP"] = iP;
        params["CQMSearchData"] = myJson;
        if (iP == "0") {
            utility.DisplayMessages("There is nothing to Show.", 3);
        } else {
            LoadActionPan("Batch_ClinicalQualityMeasureDetail", params);
        }
    },

    BatchClinicalQualityMeasureGridLoad: function (response)
    {
        $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #dgvBatchClinicalQualityMeasure").dataTable().fnDestroy();
        $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result #dgvBatchClinicalQualityMeasure tbody").find("tr").remove();

        if (response.BatchClinicalQualityMeasureCount > 0)
        {
            var batchClinicalQualityMeasureLoadJson = JSON.parse(response.BatchClinicalQualityMeasureLoad_JSON);
            $.each(batchClinicalQualityMeasureLoadJson, function (i, item) {
                if (item.CQMID != "") {

                    var $row = $("<tr/>");

                    $row.attr("id", "gvBatchClinicalQualityMeasure_row" + item.CQMID);
                    $row.attr("onclick", "Batch_ClinicalQualityMeasure.ClinicalQualityMeasureDetail('Edit','" + item.CQMID + "','" + response.ProviderId + "','" + item.Measure + "','" + item.InitialPopulation + "',event);utility.SelectGridRow($(this));");

                    $row.attr("CQMID", item.CQMID);

                    $row.append("<td style=\"display:none;\">" + item.CQMID + "</td><td>" + item.MeasureNumber + "</td><td>" + item.Measure + "</td><td>" + item.InitialPopulation + "</td><td>" + item.Denominator + "</td><td>" + item.Numerator + "</td><td>" + item.DenominatorExclusion + "</td><td>" + item.DenominatorException + "</td><td>" + item.PerfromanceRate1 + "</td>");
                    $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result #dgvBatchClinicalQualityMeasure tbody").last().append($row);
                }
            });
            if ($("#dtpDateFrom").val() == "") {
                $("#dtpDateFrom").datepicker("setDate", response.DateFrom);
            }
            if ($("#dtpDateTo").val() == "") {
                $("#dtpDateTo").datepicker("setDate", response.DateTo);
            }
        }
        else
        {
            $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #dgvBatchClinicalQualityMeasure").DataTable({
                "language": {
                    "emptyTable": "No Measure Found"
                },
                "autoWidth": false,
                "bLengthChange": false,
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #dgvBatchClinicalQualityMeasure"));
        else
            $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result #dgvBatchClinicalQualityMeasure").DataTable({
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });

        var table = $("#" + Batch_ClinicalQualityMeasure.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result #dgvBatchClinicalQualityMeasure").DataTable();

        // Sort by column 1 and then re-draw
        table
        .order([1, 'asc'])
        .draw();
    },

    SearchBatchClinicalQualityMeasure: function (batchClinicalQualityMeasureData, cqmid, pageNumber, rowsPerPage) {
        if (pageNumber == null)
            pageNumber = 1;
        if (rowsPerPage == null)
            rowsPerPage = 30;
        if (cqmid == undefined) {
            cqmid = "";
        }
        var a = JSON.parse(batchClinicalQualityMeasureData);
        if (a.AgeCondition_text == "- Select -") {
            a.AgeCondition_text = "";
        }
        if (a.Sex_text == "- Select -") {
            a.Sex_text = "";
        }
        batchClinicalQualityMeasureData = JSON.stringify(a);
       
        var data = "BatchClinicalQualityMeasureData=" + batchClinicalQualityMeasureData + "&CQMID=" + cqmid + "&PageNumber=" + pageNumber + "&RowsPerPage=" + rowsPerPage;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "SEARCH_CQM_MEASURES");
    },

    BatchClinicalQualityMeasureExport_: function (batchClinicalQualityMeasureData, cqmid) {
        var providerId = $('#' + Batch_ClinicalQualityMeasure.params.PanelID + ' #hfProviderId').val();
        var appointmentDateFrom = $("#dtpDateFrom").val();
        var appointmentDateTo = $("#dtpDateTo").val();

        var data = "BatchClinicalQualityMeasureData=" + batchClinicalQualityMeasureData + "&CQMID=" + cqmid + "&providerId=" + providerId + "&AppointmentDateFrom=" + appointmentDateFrom + "&AppointmentDateTo=" + appointmentDateTo;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "EXPORT_CQM_MEASURES");
    },

    UnLoadTab: function() {
            RemoveAdminTab();
    }
}
