iTrack_eCQMs = {
    bIsFirstLoad: true,
    params: [],
    LastSctBaseSearch: '',
    Load: function (params) {
        iTrack_eCQMs.params = params;

        if (iTrack_eCQMs.bIsFirstLoad) {
            iTrack_eCQMs.bIsFirstLoad = false;
            $("#pnliTrackeCQMs #lblBasicSearch").hide();
            var self;
            if ($("#pnliTrackeCQMs") !== "pnliTrackeCQMs")
                self = $("#pnliTrackeCQMs");
            else
                self = $("#pnliTrackeCQMs");

            self.loadDropDowns(true).done(function () {
              
                utility.CreateDatePicker("pnliTrackeCQMs #dtpDateFrom, #dtpDateTo, #dtpDateFrom", function () { }, false);
                utility.ValidateFromToDate('frmiTrack_eCQMs', 'dtpDateFrom', 'dtpDateTo', true);
                $('#dtpDateFrom').datepicker("setDate", '01/01/2018');
                $('#dtpDateTo').datepicker("setDate", '12/31/2018');
                $.when(iTrack_eCQMs.LoadingDropDowns('pnliTrackeCQMs')).then(function () {
                    $("#pnliTrackeCQMs #frmiTrack_eCQMs #ddlEthnicity").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        maxHeight: 230,
                    });
                    $("#pnliTrackeCQMs #frmiTrack_eCQMs #ddlRace").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        maxHeight: 230,
                    });

                });
               
            });

        }
        iTrack_eCQMs.BindProvider();
        iTrack_eCQMs.BindPatientName();
        iTrack_eCQMs.LoadInsurancePlan();
        iTrack_eCQMs.initializeKeypad();
    },

    initializeKeypad: function () {
        var self = "";
        self = $('#pnliTrackeCQMs #frmiTrack_eCQMs');
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
        $("#pnliTrackeCQMs #hfProviderId").val("");
        $("#pnliTrackeCQMs #NPItxt").val("");
    },

    ResetProvider: function () {
        $("#pnliTrackeCQMs #txtProvider").val('');
        $("#pnliTrackeCQMs #hfProviderId").val('');
        if ($("#pnliTrackeCQMs #frmiTrack_eCQMs #lnkProviderEdit").css("display") == "inline") {
            $("#pnliTrackeCQMs #frmiTrack_eCQMs #lnkProviderEdit").css("display", "none");
            $("#pnliTrackeCQMs #frmiTrack_eCQMs #lblProvider").css("display", "inline");
        }
    },

    /******** Patient auto-complete start *******/
    BindPatientName: function () {
        var Ctrl = $("#pnliTrackeCQMs #frmiTrack_eCQMs #txtFullName");
        var func = function () { return utility.GetPatientArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#pnliTrackeCQMs #hfPatientId");
        var onSelect = function (e) {
            $("#pnliTrackeCQMs #hfPatientAccntNum").val(e.AccountNumber);
            utility.InsertRecentPatient(e.id);
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "FullName", "contains", null, func, hfCtrl, onSelect);
    },

    OpenPatient: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'iTrack_eCQMs';
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNumber, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var self = null;
        self = $("#pnliTrackeCQMs");
        self.find("#hfPatientId").val(PatientId);
        self.find("#txtFullName").val(patFullName);
        self.find("#hfPatientAccntNum").val(AccountNumber);

        $Ctrl = $("#pnliTrackeCQMs #frmiTrack_eCQMs #txtFullName");
        $hfCtrl = $("#pnliTrackeCQMs #frmiTrack_eCQMs #hfPatientId");
        //Patient
        if ($Ctrl.data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl, patFullName, $hfCtrl, PatientId, "FullName");

        UnloadActionPan("iTrackeCQMs");
    },
    /******** Patient auto-complete end *******/

    /******** Insurance Plan auto-complete start *******/
    OpenInsurancePlanDetail: function () {
        var params = [];
        var PanelID = "pnliTrackeCQMs";
        params["ParentCtrl"] = 'iTrack_eCQMs';
        params["InsurancePlanId"] = $("#pnliTrackeCQMs #hfInsurancePlan").val();
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        LoadActionPan('insurancePlanDetail', params, PanelID);
    },

    OpenInsurancePlan: function () {
        var params = [];
        var PanelID = "pnliTrackeCQMs";
        params["ParentCtrl"] = 'iTrack_eCQMs';
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        LoadActionPan('Admin_InsurancePlan', params, PanelID);
    },

    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
        utility.SetKendoAutoCompleteSourceforValidate($("#pnliTrackeCQMs #txtInsurancePlan"), InsurancePlanName, $("#pnliTrackeCQMs #hfInsurancePlan"), InsurancePlanId);
        $("#pnliTrackeCQMs #lnkInsurancePlanDetail").css("display", "inline");
        $("#pnliTrackeCQMs #lblInsurancePlan").css("display", "none");
        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);
    },

    LoadInsurancePlan: function () {
        CacheManager.BindCodes('GetInsurancePlan', false).done(function (response) {
            var Ctrl = $("#pnliTrackeCQMs input#txtInsurancePlan");
            var hfCtrl = $("#pnliTrackeCQMs #hfInsurancePlan");
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
        params["ParentCtrl"] = "iTrack_eCQMs";
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#pnliTrackeCQMs #hfProviderId').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'iTrack_eCQMs';
        LoadActionPan('providerDetail', params);
    },

    BindProvider: function () {
        var Ctrl = $("#pnliTrackeCQMs #frmiTrack_eCQMs #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnliTrackeCQMs #hfProviderId");
        var onChange = function () { utility.FillProviderNPI('#pnliTrackeCQMs #frmiTrack_eCQMs', '#hfProviderId', '#NPItxt'); };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, null, onChange);
    },
    /******** Provider auto-complete end *******/

    ShowSearchFields: function (obj) {
        obj = $(obj);
        if (obj.text() == "Advance Search") {
            obj.text('Advance Search').hide();
            $("#pnliTrackeCQMs #lblBasicSearch").show();
            $('#pnliTrackeCQMs div[divtype="AdvancedSearch"]').each(function () {
                $(this).css("display", "inline");
            });
           
        }
        else if (obj.text() == "Basic Search") {
            obj.text('Basic Search').hide();
            $("#pnliTrackeCQMs #lblAdvanceSearch").show();
            $('#pnliTrackeCQMs div[divtype="AdvancedSearch"]').each(function () {
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
            $('#pnliTrackeCQMs #ddlEthnicity').multiselect('clearSelection');
            $('#pnliTrackeCQMs #ddlRace').multiselect('clearSelection');
            $('#pnliTrackeCQMs #ulCQMProblemList').empty();
        }
    },

    /******** Problem auto-complete start *******/
    BindICD9AutoComplete: function (element) {
        if ($(element).val().length > 3) {

            if ($(element).val().substring(0, 3).toLowerCase() == "sct") {
                iTrack_eCQMs.LastSctBaseSearch = $(element).val().substring(3, $(element).val().length);
            }
            else {
                iTrack_eCQMs.LastSctBaseSearch = "";
            }
        }
        else {
            iTrack_eCQMs.LastSctBaseSearch = "";
        }

        $('#pnliTrackeCQMs #txtProblems').attr("data-popupunload", "false");

        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "iTrackeCQMs", null, false);
    },

    OpenProblemSearch: function () {
        var controlToLoad = "";
        controlToLoad = "Admin_IMOICD";
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "iTrack_eCQMs";
        params["RefCtrl"] = "txtProblems";

        $('#pnliTrackeCQMs #txtProblems').attr('data-popupunload', 'true');

        params["Parent"] = 'pnlAdminIMOICD';
        HiddenCtrl = 'hfICD1-1,hfICDDescription1-1,hfICD101-1,hfICD10Description1-1,hfSNOMED1-1,hfSNOMEDDescription1-1';
        params["RefHiddenCtrl"] = HiddenCtrl;
        LoadActionPan(controlToLoad, params);
    },

    deleteProblem: function (obj, ev) {
        ev.stopPropagation();
        $(obj).parent().remove();
        if ($("#pnliTrackeCQMs #ulCQMProblemList li").length > 0) {
            $("#pnliTrackeCQMs #ulCQMProblemList li:first select").remove();
        }
    },
    addAgeConditionValues: function () {
        var ageCondition = $("#pnliTrackeCQMs #frmiTrack_eCQMs #ddlAgeCondition").val();
        if (ageCondition == "") {
            $("#pnliTrackeCQMs #frmiTrack_eCQMs #ageConditionValueMonth").addClass("hidden");
            $("#pnliTrackeCQMs #frmiTrack_eCQMs #ageConditionValueYear").addClass("hidden");
            var ageToValue = $('#pnliTrackeCQMs #frmiTrack_eCQMs input[id*=txtAgeValueMonth]').val('');
            var ageToValue = $('#pnliTrackeCQMs #frmiTrack_eCQMs input[id*=txtAgeValueYear]').val('');
        }
        else {
            $("#pnliTrackeCQMs #frmiTrack_eCQMs #ageConditionValueMonth").removeClass("hidden");
            $("#pnliTrackeCQMs #frmiTrack_eCQMs #ageConditionValueYear").removeClass("hidden");
        }
    },

    BatchClinicalQualityMeasureSearch: function (cqmid, pageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("eCQMs", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnliTrackeCQMs #pnliTrack_eCQMs_Result").css("display") === "none")
                    $("#pnliTrackeCQMs #pnliTrack_eCQMs_Result").show();

                var self = $("#pnliTrackeCQMs #pnliTrack_eCQMs_Result");
                var myJsonByName = self.getMyJSONByName();
                var objData = JSON.parse(myJsonByName);

                var AgeInYears = $('#pnliTrackeCQMs #txtAgeValueYear').val() * 12;
                var AgeInMonths = $('#pnliTrackeCQMs #txtAgeValueMonth').val();
                var calAge = "";
                if (AgeInYears != "" && AgeInMonths != "") {
                    calAge = parseInt(AgeInMonths, 10) + parseInt(AgeInYears, 10);
                } else if (AgeInYears != "" && AgeInMonths == "") {
                    calAge = parseInt(AgeInYears, 10);
                } else if (AgeInYears == "" && AgeInMonths != "") {
                    calAge = parseInt(AgeInMonths, 10);
                }
                objData["Age"] = calAge;

                objData["ProviderId"] = $('#pnliTrackeCQMs #hfProviderId').val();
                objData["PatientId"] = $('#pnliTrackeCQMs #hfPatientId').val();
                //objData["InsurancePlanId"] = $('#pnliTrackeCQMs #hfInsurancePlan').val();
                var problemData = [];
                $("#pnliTrackeCQMs #ulCQMProblemList li").each(function (index, item) {
                    var problem = null;
                    problem = {
                        ICD10: $(this).attr('icd10code'),
                        SnomedCode: $(this).attr('snomedcode'),
                    };
                    problemData.push(problem);
                });
                objData["Problems"] = problemData;
                objData["DateFrom"] = $("#dtpDateFrom").val();
                objData["DateTo"] = $("#dtpDateTo").val();
                var myJson = JSON.stringify(objData);
               
                if ($("#ddlprovider :selected").val() == "" && $("#dtpDateFrom").val() == "" && $("#dtpDateTo").val() == "") {
                    utility.DisplayMessages("Specify a Criteria to Filter", 3);
                } else {
                    iTrack_eCQMs.SearchBatchClinicalQualityMeasure(myJson, cqmid, pageNo, rpp).done(function (response) {

                        if (response.status != false) {
                            iTrack_eCQMs.BatchClinicalQualityMeasureGridLoad(response);

                            var tableControl = "pnliTrackeCQMs #dgvBatchClinicalQualityMeasure";
                            var pagingPanelControlId = "pnliTrackeCQMs #divBatchClinicalQualityMeasurePaging";
                            var classControlName = "iTrack_eCQMs";
                            var pagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout
                            (
                                CreatePagination(response.BatchClinicalQualityMeasureCount, pageNo, 30, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords,
                                    function (primaryId, pageNumber, resultPerPage) {
                                        iTrack_eCQMs.BatchClinicalQualityMeasureSearch(primaryId, pageNumber, 30);
                                    }
                                ), 10);
                        } else {
                            $("#pnliTrackeCQMs #dgvBatchClinicalQualityMeasure").dataTable().fnDestroy();
                            $("#pnliTrackeCQMs #pnliTrack_eCQMs_Result #dgvBatchClinicalQualityMeasure tbody").find("tr").remove();
                            $("#pnliTrackeCQMs #divBatchClinicalQualityMeasurePaging").remove();

                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    BatchClinicalQualityMeasureExport: function () {

        if ($("#pnliTrackeCQMs #pnliTrack_eCQMs_Result").css("display") === "none")
            $("#pnliTrackeCQMs #pnliTrack_eCQMs_Result").show();

        var self = $("#pnliTrackeCQMs #pnliTrack_eCQMs_Search");
        var myJson = self.getMyJSON();

        if ($("#dgvBatchClinicalQualityMeasure tr").length > 1) {
            iTrack_eCQMs.BatchClinicalQualityMeasureExport_(myJson).done(function (response) {
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

    ClinicalQualityMeasureDetail: function (mode, cqmid, providerId, measure, iP, event, measureNumber) {
        if (event != null)
            event.stopPropagation();

        var self = $("#pnliTrackeCQMs #pnliTrack_eCQMs_Search");
        var myJsonByName = self.getMyJSONByName();
        var objData = JSON.parse(myJsonByName);

        var AgeInYears = $('#' + iTrack_eCQMs.params.PanelID + ' #txtAgeValueYear').val() * 12;
        var AgeInMonths = $('#' + iTrack_eCQMs.params.PanelID + ' #txtAgeValueMonth').val();
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
        objData["ProviderId"] = $('#' + iTrack_eCQMs.params.PanelID + ' #hfProviderId').val();
        objData["PatientId"] = $('#' + iTrack_eCQMs.params.PanelID + ' #hfPatientId').val();
        objData["PatientAccountNumber"] = $("#pnliTrackeCQMs #hfPatientAccntNum").val();
        var problemData = [];
        $("#pnliTrackeCQMs #ulCQMProblemList li").each(function (index, item) {
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
        params["MeasureNumber"] = measureNumber;
        params["ParentCtrl"] = "iTrack_eCQMs";
        params["dateFrom"] = dateFrom;
        params["dateTo"] = dateTo;
        params["iP"] = iP;
        params["CQMSearchData"] = myJson;
        if (iP == "0") {
            utility.DisplayMessages("There is nothing to Show.", 3);
        } else {
            LoadActionPan("iTrack_eCQMsDetail", params);
        }
    },

    BatchClinicalQualityMeasureGridLoad: function (response) {
        $("#pnliTrackeCQMs #dgvBatchClinicalQualityMeasure").dataTable().fnDestroy();
        $("#pnliTrackeCQMs #pnliTrack_eCQMs_Result #dgvBatchClinicalQualityMeasure tbody").find("tr").remove();

        if (response.BatchClinicalQualityMeasureCount > 0) {
            var batchClinicalQualityMeasureLoadJson = JSON.parse(response.BatchClinicalQualityMeasureLoad_JSON);
            $.each(batchClinicalQualityMeasureLoadJson, function (i, item) {
                if (item.CQMID != "") {

                    var $row = $("<tr/>");

                    $row.attr("id", "gvBatchClinicalQualityMeasure_row" + item.CQMID);
                    $row.attr("onclick", "iTrack_eCQMs.ClinicalQualityMeasureDetail('Edit','" + item.CQMID + "','" + response.ProviderId + "','" + item.Measure + "','" + item.InitialPopulation + "',event,'" + item.MeasureNumber + "');utility.SelectGridRow($(this));");

                    $row.attr("CQMID", item.CQMID);

                    $row.append("<td style=\"display:none;\">" + item.CQMID + "</td><td>" + item.MeasureNumber + "</td><td>" + item.Measure + "</td><td>" + item.InitialPopulation + "</td><td>" + item.Denominator + "</td><td>" + item.Numerator + "</td><td>" + item.DenominatorExclusion + "</td><td>" + item.DenominatorException + "</td><td>" + item.PerfromanceRate1 + "</td>");
                    $("#pnliTrackeCQMs #pnliTrack_eCQMs_Result #dgvBatchClinicalQualityMeasure tbody").last().append($row);
                }
            });
            if ($("#dtpDateFrom").val() == "") {
                $("#dtpDateFrom").datepicker("setDate", response.DateFrom);
            }
            if ($("#dtpDateTo").val() == "") {
                $("#dtpDateTo").datepicker("setDate", response.DateTo);
            }
        }
        else {
            $("#pnliTrackeCQMs #dgvBatchClinicalQualityMeasure").DataTable({
                "language": {
                    "emptyTable": "No Measure Found"
                },
                "autoWidth": false,
                "bLengthChange": false,
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#pnliTrackeCQMs #dgvBatchClinicalQualityMeasure"));
        else
            $("#pnliTrackeCQMs #pnliTrack_eCQMs_Result #dgvBatchClinicalQualityMeasure").DataTable({
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });

        var table = $("#pnliTrackeCQMs #pnliTrack_eCQMs_Result #dgvBatchClinicalQualityMeasure").DataTable();

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
        var providerId = $('#' + iTrack_eCQMs.params.PanelID + ' #hfProviderId').val();
        var appointmentDateFrom = $("#dtpDateFrom").val();
        var appointmentDateTo = $("#dtpDateTo").val();

        var data = "BatchClinicalQualityMeasureData=" + batchClinicalQualityMeasureData + "&CQMID=" + cqmid + "&providerId=" + providerId + "&AppointmentDateFrom=" + appointmentDateFrom + "&AppointmentDateTo=" + appointmentDateTo;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "EXPORT_CQM_MEASURES");
    },

    UnLoadTab: function () {
        RemoveAdminTab();
    }
}
