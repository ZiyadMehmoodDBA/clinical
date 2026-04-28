iTrack_ImprovementActivities = {
    params: [],
    bIsFirstLoad: true,
    listSearchLength: 0,
    measureTable: null,
    Load: function (params) {
        iTrack_ImprovementActivities.params = params;
        iTrack_ImprovementActivities.measureTable = null;
        if (iTrack_ImprovementActivities.params.PanelID != 'pnliTrackImprovementActivities') {
            iTrack_ImprovementActivities.params.PanelID = iTrack_ImprovementActivities.params.PanelID + ' #pnliTrackImprovementActivities';
        } else {
            iTrack_ImprovementActivities.params.PanelID = 'pnliTrackImprovementActivities';
        }
        var self = $("#pnliTrackImprovementActivities");
        self.loadDropDowns(true).done(function () {

            utility.CreateDatePicker(iTrack_ImprovementActivities.params.PanelID + " #dtpFromDate, #dtpToDate", function () { }, false);
            //utility.ValidateFromToDate('frmiTrackImprovementActivities', 'dtpFromDate', 'dtpToDate', true);
            iTrack_ImprovementActivities.BindProvider();
            $('#pnliTrackImprovementActivities #dtpFromDate').datepicker("setDate", '10/01/2018');
            $('#pnliTrackImprovementActivities #dtpToDate').datepicker("setDate", '12/31/2018');
            $('#pnliTrackImprovementActivities #dtpGroupFromDate').datepicker("setDate", '10/01/2018');
            $('#pnliTrackImprovementActivities #dtpGroupToDate').datepicker("setDate", '12/31/2018');
            $('#pnliTrackImprovementActivities #dtpFromDate').attr('disabled', 'disabled');
            $('#pnliTrackImprovementActivities #dtpToDate').attr('disabled', 'disabled');
            $('#pnliTrackImprovementActivities #dtpGroupFromDate').attr('disabled', 'disabled');
            $('#pnliTrackImprovementActivities #dtpGroupToDate').attr('disabled', 'disabled');
            iTrack_ImprovementActivities.BindGroupProviders();
            $('#pnliTrackImprovementActivities #ddlMemberProviders').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonTitle: function (options, select) {
                    var buttonTitle = "";
                    $.each(options, function (i, item) {
                        if (buttonTitle != "") {
                            buttonTitle += "," + $(item).attr("refvalue");
                        }
                        else {
                            buttonTitle += $(item).attr("refvalue");
                        }
                        $(item).prop("disabled", true);
                    });

                    return buttonTitle;
                }
            });
            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #IsGroupSearched").addClass("hide");
            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #IsProviderSearched").removeClass("hide");
        });

    },
    BindGroupProviders: function () {
        var Ctrl = $("#pnliTrackImprovementActivities #frmiTrackImprovementActivities #txtGroupName");
        var func = function () { return iTrack_ImprovementActivities.GetMIPSGroupProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnliTrackImprovementActivities #hfGroupId");
        var onChange = function () { iTrack_ImprovementActivities.setGroupProviderIDs(); };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, null, onChange);
    },

    GetMIPSGroupProviderArray: function (name) {
        var AllProviders = [];
        var IsValid = false;

        if (name != null && name.length > 1)
            IsValid = true;
        else
            IsValid = false;

        var dfd = new $.Deferred();

        if (IsValid) {
            iTrack_MIPSummary.Search_MIPSGroupPreferences_DBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    responseData = JSON.parse(responseData);
                    if (responseData.IndividualProCount > 0) {
                        var ProviderLoadJSONData = JSON.parse(responseData.IndividualProCountLoad_JSON);
                        iTrack_ImprovementActivities.params.Groups = ProviderLoadJSONData.Groups;
                        iTrack_ImprovementActivities.params.GroupsDetails = ProviderLoadJSONData.GroupsDetail;
                        iTrack_ImprovementActivities.params.MembersDetails = ProviderLoadJSONData.GroupDetail;

                        $.each(ProviderLoadJSONData.Groups, function (i, item) {

                            AllProviders.push({ id: item.GroupId, value: item.GroupName });

                        });


                    }
                }
                dfd.resolve(AllProviders);
            });
        }
        else {
            dfd.resolve(AllProviders);
        }

        return dfd.promise();

    },

    setGroupProviderIDs: function () {
        var groupId = $("#pnliTrackImprovementActivities #hfGroupId").val();
        var groupDetails = $.grep(iTrack_ImprovementActivities.params.Groups, function (a) {
            return a.GroupId == groupId;
        });

        $.each(groupDetails, function (i, item) {
            item.PerformanceYear = item.PerformanceYear == "" ? "2018" : item.PerformanceYear;
            $("#pnliTrackImprovementActivities #frmiTrackImprovementActivities #txtGroupTIN").val(item.TIN);
            $("#pnliTrackImprovementActivities #frmiTrackImprovementActivities #lblProviderName").text('');
        });

        var members = "";
        if (iTrack_ImprovementActivities.params.MembersDetails && iTrack_ImprovementActivities.params.MembersDetails.length > 0) {
            $.each(iTrack_ImprovementActivities.params.MembersDetails, function (i, item) {

                members += "," + item.ProviderId;
                $('#pnliTrackImprovementActivities #ddlMemberProviders').append($('<option>', {
                    refvalue: item.ProviderName,
                    value: item.ProviderId,
                    text: item.ProviderName
                }));
            });
            $('#pnliTrackImprovementActivities #ddlMemberProviders').val(members.split(','));
            $('#pnliTrackImprovementActivities #ddlMemberProviders').multiselect("refresh");
            $('#pnliTrackImprovementActivities #ddlMemberProviders').multiselect('rebuild');

        }
    },
    viewIndividualReporting: function () {
        $("#pnliTrackImprovementActivities #dvIndividualReporting").removeClass('hidden');
        $("#pnliTrackImprovementActivities #dvGroupReporting").addClass('hidden');
        $("#pnliTrackImprovementActivities #lblNPI").text(' NPI: ');
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #IsGroupSearched").addClass("hide");
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #IsProviderSearched").removeClass("hide");
         $("#" + iTrack_ImprovementActivities.params.PanelID + " #dgviTrackImprovementActivities").dataTable().fnClearTable();
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #dgviTrackImprovementActivities").dataTable().fnDestroy();
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #dgviTrackImprovementActivities tbody").find("tr").remove();
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #diviTrackImprovementActivitiesPaging").addClass('hidden');
    },
    viewGroup: function () {
        $("#pnliTrackImprovementActivities #dvIndividualReporting").addClass('hidden');
        $("#pnliTrackImprovementActivities #dvGroupReporting").removeClass('hidden');
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #IsProviderSearched").addClass("hide");
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #IsGroupSearched").removeClass("hide");
        $("#pnliTrackImprovementActivities #lblNPI").text(' Group TIN: ');
         $("#" +iTrack_ImprovementActivities.params.PanelID + " #dgviTrackImprovementActivities").dataTable().fnClearTable();
        $("#" + iTrack_ImprovementActivities.params["PanelID"]+ " #dgviTrackImprovementActivities").dataTable().fnDestroy();
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #dgviTrackImprovementActivities tbody").find("tr").remove();
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #diviTrackImprovementActivitiesPaging").addClass('hidden');
    },
    OpenProviderDetail: function () {

        var params = [];
        params["ProviderId"] = $('#' + iTrack_ImprovementActivities.params.PanelID + ' #hfProviderId').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = 'txtProvider';
        params["ParentCtrl"] = 'iTrack_ImprovementActivities';
        LoadActionPan('providerDetail', params);
    },
    GroupPreferencesEdit: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS Preference_Group / Virtual Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["GroupId"] = $("#pnliTrackImprovementActivities #hfGroupId").val();
                params["mode"] = "Edit";
                params["FromAdmin"] = '0';
                params["ParentCtrl"] = 'iTrack_ImprovementActivities';
                LoadActionPan('MIPS_AdminPreferenceGroupDetail', params);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    HideGroupNameLink: function (value) {
        if (value == "") {
            $('#pnliTrackImprovementActivities #txtGroupName').attr("ProviderId", "-1");
            $('#pnliTrackImprovementActivities #hfGroupId').val("-1");
            $("#pnliTrackImprovementActivities #lnkGroupEdit").css("display", "none");
            $("#pnliTrackImprovementActivities #lblGroupName").css("display", "inline");
        } else {

            $("#pnliTrackImprovementActivities #lnkGroupEdit").css("display", "inline");
            $("#pnliTrackImprovementActivities #lblGroupName").css("display", "none");
        }
    },
    HideProviderLink: function (value) {
        if (value == "" || value == null) {
            $('#pnliTrackImprovementActivities #txtProvider').attr("ProviderId", "-1");
            $('#pnliTrackImprovementActivities #hfProviderId').val("-1");
            $("#pnliTrackImprovementActivities #lnkProviderEdit").css("display", "none");
            $("#pnliTrackImprovementActivities #lblProvider").css("display", "inline");
        } else {

            $("#pnliTrackImprovementActivities #lnkProviderEdit").css("display", "inline");
            $("#pnliTrackImprovementActivities #lblProvider").css("display", "none");
        }
    },
    QualityMeasureSearch: function (cqmid, pageNo, rpp) {
        if ($('#pnliTrackImprovementActivities #txtProvider').val() == "") {
            utility.DisplayMessages('Please Select Provider', 2);
            return;
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS_IA (Improvement Activities)", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #lblProviderName").text($("#" + iTrack_ImprovementActivities.params.PanelID + " #txtProvider").val());
                $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #lblNPI").text($("#pnliTrackImprovementActivities #NPItxt").val());
                var year = "";
                if ($("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 1) {
                    year = '90 Days | 10/01/2017 - 12/31/2017';
                } else if ($("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 2) {
                    year = '90 Days | 10/01/2018 - 12/31/2018';
                }
                var from = $('#' + iTrack_ImprovementActivities.params.PanelID + " #dtpFromDate").val() ? $('#' + iTrack_ImprovementActivities.params.PanelID + " #dtpFromDate").val() : $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 1 ? "10/01/2017" : $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 2 ? "10/01/2018" : "";
                var to = $('#' + iTrack_ImprovementActivities.params.PanelID + " #dtpToDate").val() ? $('#' + iTrack_ImprovementActivities.params.PanelID + " #dtpToDate").val() : $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 1 ? "12/31/2017" : $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 2 ? "12/31/2018" : "";
                year = "90 Days | " + (from + " - " + to);
                $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #lblPerformanceyear").text(year);

                if ($("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result").css("display") === "none")
                    $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result").show();

                var self = $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnlImprovementActivitiesMeasure_Search");
                var myJsonByName = self.getMyJSONByName();
                var objData = JSON.parse(myJsonByName);



                objData["ProviderId"] = $('#' + iTrack_ImprovementActivities.params.PanelID + ' #hfProviderId').val();
                objData["PatientId"] = $('#' + iTrack_ImprovementActivities.params.PanelID + ' #hfPatientId').val();
                objData["Problems"] = [];
                var myJson = JSON.stringify(objData);

                if ($("#ddlprovider :selected").val() == "" && $("#dtpDateFrom").val() == "" && $("#dtpDateTo").val() == "") {
                    utility.DisplayMessages("Specify a Criteria to Filter", 3);
                } else {
                    iTrack_ImprovementActivities.SearchQualityMeasure(myJson, cqmid, pageNo, rpp).done(function (response) {

                        if (response.status != false) {
                            iTrack_ImprovementActivities.params.ProviderData = response.ProviderText;
                            iTrack_ImprovementActivities.params.PracticeData = response.PracticeText;
                            iTrack_ImprovementActivities.QualityMeasureGridLoad(response, false);

                            var tableControl = iTrack_ImprovementActivities.params["PanelID"] + " #dgviTrackImprovementActivities";
                            var pagingPanelControlId = iTrack_ImprovementActivities.params["PanelID"] + " #diviTrackImprovementActivitiesPaging";
                            var classControlName = "iTrack_ImprovementActivities";
                            var pagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout
                            (
                                CreatePagination(response.BatchClinicalQualityMeasureCount, pageNo, 30, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords,
                                    function (primaryId, pageNumber, resultPerPage) {
                                        iTrack_ImprovementActivities.QualityMeasureSearch(primaryId, pageNumber, 30);
                                    }
                                ), 10);
                        } else {
                            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #dgviTrackImprovementActivities").dataTable().fnDestroy();
                            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities #dgviTrackImprovementActivities tbody").find("tr").remove();
                            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #diviTrackImprovementActivitiesPaging").remove();

                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    QualityMeasureSearchGroup: function (cqmid, pageNo, rpp) {
        if ($('#pnliTrackImprovementActivities #txtGroupName').val() == "" || $('#pnliTrackImprovementActivities #txtGroupName').val() == null) {
            utility.DisplayMessages('Please Select Group', 2);
            return;
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS_IA (Improvement Activities)", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #lblProviderName").text($('#pnliTrackImprovementActivities #txtGroupName').val());
                $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #lblNPI").text($("#pnliTrackImprovementActivities #NPItxt").val());
                var year = "";
                if ($("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 1) {
                    year = '90 Days | 10/01/2017 - 12/31/2017';
                } else if ($("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 2) {
                    year = '90 Days | 10/01/2018 - 12/31/2018';
                }
                var from = $('#' + iTrack_ImprovementActivities.params.PanelID + " #dtpGroupFromDate").val() ? $('#' + iTrack_ImprovementActivities.params.PanelID + " #dtpGroupFromDate").val() : $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 1 ? "10/01/2017" : $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 2 ? "10/01/2018" : "";
                var to = $('#' + iTrack_ImprovementActivities.params.PanelID + " #dtpGroupToDate").val() ? $('#' + iTrack_ImprovementActivities.params.PanelID + " #dtpGroupToDate").val() : $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 1 ? "12/31/2017" : $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #ddlPerformanceYear").val() == 2 ? "12/31/2018" : "";
                year = "90 Days | " + (from + " - " + to);
                $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #lblPerformanceyear").text(year);

                if ($("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result").css("display") === "none")
                    $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result").show();

                var self = $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnlImprovementActivitiesMeasure_Search");
                var myJsonByName = self.getMyJSONByName();
                var objData = JSON.parse(myJsonByName);



                objData["GroupId"] = $('#' + iTrack_ImprovementActivities.params.PanelID + ' #hfGroupId').val();
                objData["Problems"] = [];
                var myJson = JSON.stringify(objData);

                if ($("#ddlprovider :selected").val() == "" && $("#dtpDateFrom").val() == "" && $("#dtpDateTo").val() == "") {
                    utility.DisplayMessages("Specify a Criteria to Filter", 3);
                } else {
                    iTrack_ImprovementActivities.SearchQualityMeasureGroup(myJson, cqmid, pageNo, rpp).done(function (response) {

                        if (response.status != false) {
                            iTrack_ImprovementActivities.params.ProviderData = response.ProviderText;
                            iTrack_ImprovementActivities.params.PracticeData = response.PracticeText;
                            iTrack_ImprovementActivities.QualityMeasureGridLoad(response, true);

                            var tableControl = iTrack_ImprovementActivities.params["PanelID"] + " #dgviTrackImprovementActivities";
                            var pagingPanelControlId = iTrack_ImprovementActivities.params["PanelID"] + " #diviTrackImprovementActivitiesPaging";
                            var classControlName = "iTrack_ImprovementActivities";
                            var pagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout
                            (
                                CreatePagination(response.BatchClinicalQualityMeasureCount, pageNo, 30, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords,
                                    function (primaryId, pageNumber, resultPerPage) {
                                        iTrack_ImprovementActivities.QualityMeasureSearchGroup(primaryId, pageNumber, 30);
                                    }
                                ), 10);
                        } else {
                            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #dgviTrackImprovementActivities").dataTable().fnDestroy();
                            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities #dgviTrackImprovementActivities tbody").find("tr").remove();
                            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #diviTrackImprovementActivitiesPaging").remove();

                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    QualityMeasureGridLoad: function (response, isGroup) {
        var arrMeasureActivity = [];
        $("#" + iTrack_ImprovementActivities.params.PanelID + " #dgviTrackImprovementActivities").dataTable().fnClearTable();
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #dgviTrackImprovementActivities").dataTable().fnDestroy();
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #dgviTrackImprovementActivities tbody").find("tr").remove();
        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #diviTrackImprovementActivitiesPaging").removeClass('hidden');
        var arraTemp = [];
        if (response.RecordCount > 0) {
            var loadJson = JSON.parse(response.ImprovementActivities_JSON);
            $.each(loadJson, function (i, item) {
                if (item.Id != "") {

                    var $row = $("<tr/>");

                    $row.attr("Id", "gvImprovementActivities_row" + item.Id);

                    $row.attr("Id", item.Id);
                    $row.attr("class", 'Parent');
                    //var switchClass = 'off';
                    //if (item.PerformanceStatus == "True") {
                    switchClass = 'on';
                    //}

                    var isEligibleForBonus = "Yes";
                    if (item.IsEligibleForBonus == "True") {
                        isEligibleForBonus = "Yes";
                    } else {
                        isEligibleForBonus = "No";
                    }
                    if ($.inArray(item.ActivityId, arrMeasureActivity) == -1)
                        arrMeasureActivity.push(item.ActivityId.trim());


                    var infoIcon = '<a data-toggle="tooltip" title="" data-original-title="Test tooltip" onclick="" href="javascript:void(0)" style=" margin-left: 10px;"><i class="fa fa-info btn btn-primary btn-xs" style="border-radius: 0px;"></i></a>';
                    var performanceStatus = '<div id="divSwitch"><span class="pr-xs">NOT STARTED</span><div class="btnWidgetSwitch switch switch-xs switch-success"><div class="ios-switch ' + switchClass + '"><div class="on-background background-fill"></div><div class="state-background background-fill"></div><div class="handle"></div></div><input id="switchActive" isactive="1" type="checkbox" checked="checked" name="switch" data-plugin-ios-switch="" style="display: none;" onchange=""></div><span class="pl-xs">COMPLETE</span></div>';
                    var icon = '<a  href="javacript:void(0);" class="on-editing expand-row font-xs" title="Expand/Collapse Record" onclick="iTrack_ImprovementActivities.stopProp(this,event);"><i class="fa fa-caret-right"></i></a>';
                    $row.append("<td style=\"display:none;\">" + item.Id + "</td><td id='ActivityId'>" + item.ActivityId + "</td><td id='ActivityDescription'>" + icon + " " + item.Activity + infoIcon + "</td><td id='ActivityWeighting'>" + item.ActivityWeighting + "</td><td id='PerformanceStatus'>" + performanceStatus + "</td>");
                    $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #dgviTrackImprovementActivities tbody").last().append($row);
                    var CurrentRowchilds = $();
                    var childRow = '<div class="col-xs-12 p-none pt-xs pb-xs"><table  id="childTable"class="table-va-top table-space-td" width="100%"><tr class="text-uppercase"><th class="pb-default size110"></th><th class="pb-default size200">ACTIVITY DESCRIPTION</th><th class="pb-default size120">SUBCATEGORY NAME</th><th class="pb-default size120">WEIGHTAGE POINTS</th><th class="pb-default size150">ELIGIBILE STATUS FOR ACI BONUS</th></tr><tbody><tr><td><ul class="list-unstyled"><li></li></ul></td><td><p id="MeasureDescription">' + item.ActivityDescription + '<a href="https://qpp.cms.gov/mips/improvement-activities" target="_blank"> LearnMore <i class="fa fa-external-link"></i></a></p></td><td><ul class="list-unstyled"><li><li id="SubCategoryName">' + item.SubCategoryName + '</li></ul></td><td id="WeightagePoints"><ul class="list-unstyled">' + item.WeightagePoints + '</ul></td><td id="WeightagePoints"><ul class="list-unstyled">' + isEligibleForBonus + '</ul></td></tr></tbody></table></div>';
                    CurrentRowchilds = CurrentRowchilds.add(childRow);
                    arraTemp.push({ row: $row, childs: CurrentRowchilds });
                    if (isGroup) {
                        var txt = "90 Days | " + ($("#" + iTrack_ImprovementActivities.params["PanelID"] + " #dtpGroupFromDate").val()) + " - " + ($("#" + iTrack_ImprovementActivities.params["PanelID"] + " #dtpGroupToDate").val());
                        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #lblPerformanceyear").text(txt);
                    }
                    else {
                        var txt = ("90 Days | ") + ($("#" + iTrack_ImprovementActivities.params["PanelID"] + " #dtpFromDate").val()) + " - " + ($("#" + iTrack_ImprovementActivities.params["PanelID"] + " #dtpToDate").val());
                        $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #lblPerformanceyear").text(txt);
                    }
                }
                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            });

            //if ($("#dtpDateFrom").val() == "") {
            //    $("#dtpDateFrom").datepicker("setDate", response.DateFrom);
            //}
            //if ($("#dtpDateTo").val() == "") {
            //    $("#dtpDateTo").datepicker("setDate", response.DateTo);
            //}
        }
        else {
            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #dgviTrackImprovementActivities").DataTable({
                "language": {
                    "emptyTable": "No Measure Found"
                },
                "autoWidth": false,
                "bLengthChange": false,
                "searching": false,
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });

        }
        if ($.fn.dataTable.isDataTable("#" + iTrack_ImprovementActivities.params["PanelID"] + " #dgviTrackImprovementActivities"));
        else {
            iTrack_ImprovementActivities.measureTable = $('#' + iTrack_ImprovementActivities.params.PanelID + ' #pnliTrackImprovementActivities_Result #dgviTrackImprovementActivities').DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

            $.each(arraTemp, function (i, item) {

                if (iTrack_ImprovementActivities.measureTable != null) {
                    var row = iTrack_ImprovementActivities.measureTable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }

                }
            });
            $('#' + iTrack_ImprovementActivities.params.PanelID + ' #pnliTrackImprovementActivities_Result #dgviTrackImprovementActivities').off()
           .on('click', 'a.expand-row', function (e) {
               e.preventDefault();

               iTrack_ImprovementActivities.rowExpand($(this).closest('tr'));
           });
        }

        var table = $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #dgviTrackImprovementActivities").DataTable();

        // Sort by column 1 and then re-draw
        table
        .order([1, 'asc'])
        .draw();
        $('#' + iTrack_ImprovementActivities.params.PanelID + ' #pnliTrackImprovementActivities_Result #lblMeasureActivityCount').html(arrMeasureActivity.length);
    },
    stopProp: function (obj, event) {

        event.stopPropagation();
        iTrack_ImprovementActivities.rowExpand($(obj).closest('tr'));

    },
    QualityMeasureDetail: function (mode, cqmid, providerId, measure, iP, event, measureNumber) {
        if (event != null)
            event.stopPropagation();

        var self = $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnlImprovementActivitiesMeasure_Search");
        var myJsonByName = self.getMyJSONByName();
        var objData = JSON.parse(myJsonByName);

        objData["Age"] = null;
        objData["CQMId"] = cqmid;
        objData["ProviderId"] = $('#' + iTrack_ImprovementActivities.params.PanelID + ' #hfProviderId').val();
        objData["PatientId"] = $('#' + iTrack_ImprovementActivities.params.PanelID + ' #hfPatientId').val();
        objData["PatientAccountNumber"] = null;

        objData["Problems"] = [];
        var myJson = JSON.stringify(objData);

        var temp = JSON.parse(myJson);
        temp["Sex_text"] = null;
        temp["AgeCondition_text"] = null;
        temp["ProviderTypeId_text"] = null;
        myJson = JSON.stringify(temp);

        var dateFrom = $("#dtpFromDate").val();
        var dateTo = $("#dtpToDate").val();
        var params = [];
        params["mode"] = mode;
        params["CQMID"] = cqmid;
        params["providerId"] = providerId;
        params["Measure"] = measure;
        params["ParentCtrl"] = "iTrack_ImprovementActivities";
        params["dateFrom"] = dateFrom;
        params["dateTo"] = dateTo;
        params["iP"] = iP;
        params["MeasureNumber"] = measureNumber;
        params["CQMSearchData"] = myJson;
        if (iP == "0") {
            utility.DisplayMessages("There is nothing to Show.", 3);
        } else {
            LoadActionPan("iTrack_ImprovementActivitiesMeasureDetail", params);
        }
    },
    ExportData: function (e) {

        var JSONData = [];
        $("#" + iTrack_ImprovementActivities.params.PanelID + " #dgviTrackImprovementActivities tbody tr.Parent").each(function () {

            var obj = {
                ActivityId: $(this).find("#ActivityId").text().trim(),
                ActivityName: $(this).find("#ActivityDescription").text().trim(),
                ActivityWeightage: $(this).find("#ActivityWeighting").text().trim(),
                PerformanceStatus: "COMPLETED"

            }
            JSONData.push(obj);

        });
        iTrack_ImprovementActivities.ExportDataToExcel(JSONData);
    },
    ExportDataToExcel: function (JSONData) {

        var ReportTitle = "Improvement Activities";
        var ShowLabel = true;
        var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

        var CSV = '';
        CSV += ReportTitle + '\r\n\n';
        if (ShowLabel) {
            var row = "";
            for (var index in arrData[0]) {
                if (index == "ActivityId") {
                    index = "Activity Id";
                } else if (index == "ActivityName") {
                    index = "Activity Name";
                } else if (index == "ActivityWeightage") {
                    index = "Activity Weightage";
                } else if (index == "PerformanceStatus") {
                    index = "Performance Status";
                }
                row += index + ',';
            }
            row = row.slice(0, -1);
            CSV += row + '\r\n';
        }

        for (var i = 0; i < arrData.length; i++) {
            var row = "";
            for (var index in arrData[i]) {
                row += '"' + arrData[i][index] + '",';
            }
            row.slice(0, row.length - 1);
            CSV += row + '\r\n';
        }

        if (CSV == '') {
            alert("Invalid data");
            return;
        }
        var fileName = "";
        fileName += ReportTitle.replace(/ /g, "_");
        var csvData = new Blob([CSV], { type: 'text/csv' }); //new way
        var csvUrl = URL.createObjectURL(csvData);
        var link = document.createElement("a");
        link.href = csvUrl;
        link.style = "visibility:hidden";
        link.download = fileName + ".csv";
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    },
    rowExpand: function ($row) {

        var row = iTrack_ImprovementActivities.measureTable.row($row);
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td .fa-caret-down").attr("class", "fa fa-caret-right");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td .fa-caret-right").attr("class", "fa fa-caret-down");
            // Open this row
            row.child.show();
            if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
            }
        }
    },
    rowExpandCollapseAll: function () {
        if ($("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #btnExpandAll i").hasClass('fa-caret-right')) {
            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #dgviTrackImprovementActivities tbody tr").each(function () {
                var tr = $(this);
                tr.find('.fa-caret-right').addClass('fa-caret-down').removeClass('fa-caret-right');
                var row = iTrack_ImprovementActivities.measureTable.row(tr);
                row.child.show();
            });
            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #btnExpandAll i").removeClass('fa-caret-right').addClass('fa-caret-down');
        } else {
            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #dgviTrackImprovementActivities tbody tr").each(function () {
                var tr = $(this);
                tr.find('.fa-caret-down').addClass('fa-caret-right').removeClass('fa-caret-down');
                var row = iTrack_ImprovementActivities.measureTable.row(tr);
                row.child.hide();
            });
            $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #btnExpandAll i").addClass('fa-caret-right').removeClass('fa-caret-down');
        }

    },
    SearchQualityMeasure: function (QualityMeasureData, cqmid, pageNumber, rowsPerPage) {
        if (pageNumber == null)
            pageNumber = 1;
        if (rowsPerPage == null)
            rowsPerPage = 30;
        if (cqmid == undefined) {
            cqmid = "";
        }
        var a = JSON.parse(QualityMeasureData);
        if (a.AgeCondition_text == "- Select -") {
            a.AgeCondition_text = "";
        }
        if (a.Sex_text == "- Select -") {
            a.Sex_text = "";
        }
        QualityMeasureData = JSON.stringify(a);

        var data = "FieldsData=" + QualityMeasureData + "&CQMID=" + cqmid + "&PageNumber=" + pageNumber + "&RowsPerPage=" + rowsPerPage;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "SEARCH_IMPROVEMENTACTIVITIES_MEASURES");
    },
    SearchQualityMeasureGroup: function (QualityMeasureData, cqmid, pageNumber, rowsPerPage) {
        if (pageNumber == null)
            pageNumber = 1;
        if (rowsPerPage == null)
            rowsPerPage = 30;
        if (cqmid == undefined) {
            cqmid = "";
        }
        var a = JSON.parse(QualityMeasureData);
        if (a.AgeCondition_text == "- Select -") {
            a.AgeCondition_text = "";
        }
        if (a.Sex_text == "- Select -") {
            a.Sex_text = "";
        }
        QualityMeasureData = JSON.stringify(a);

        var data = "FieldsData=" + QualityMeasureData + "&CQMID=" + cqmid + "&PageNumber=" + pageNumber + "&RowsPerPage=" + rowsPerPage;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "SEARCH_IMPROVEMENTACTIVITIES_MEASURES_GROUP");
    },
    ResetNPI: function () {
        $("#pnliTrackImprovementActivities #hfProviderId").val("");
        $("#pnliTrackImprovementActivities #NPItxt").val("");
    },
    ResetProvider: function () {
        $("#" + iTrack_ImprovementActivities.params.PanelID + " #txtProvider").val('');
        $("#" + iTrack_ImprovementActivities.params.PanelID + " #hfProviderId").val('');
        if ($("#pnliTrackImprovementActivities #frmiTrackImprovementActivities #lnkProviderEdit").css("display") == "inline") {
            $("#pnliTrackImprovementActivities #frmiTrackImprovementActivities #lnkProviderEdit").css("display", "none");
            $("#pnliTrackImprovementActivities #frmiTrackImprovementActivities #lblProvider").css("display", "inline");
        }
    },
    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["RefCtrlHidden"] = "hfProviderId";
        params["ProviderNPI"] = 'NPItxt';
        params["ParentCtrl"] = "iTrack_ImprovementActivities";
        LoadActionPan('Admin_Provider', params);
    },
    BindProvider: function () {

        var Ctrl = $("#pnliTrackImprovementActivities #frmiTrackImprovementActivities #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnliTrackImprovementActivities #hfProviderId");
        var onSelect = function (e) {
            $("#pnliTrackImprovementActivities #hfProviderId").val(e.id);
            iTrack_ImprovementActivities.setProviderName(); utility.FillProviderNPI('#pnliTrackImprovementActivities #frmiTrackImprovementActivities', '#hfProviderId', '#NPItxt');
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);

    },
    setProviderName: function () {

        $("#pnliTrackImprovementActivities #frmiTrackImprovementActivities #lblProviderName").text("Dr. " + $("#pnliTrackImprovementActivities #frmiTrackImprovementActivities #txtProvider").val() + "|");
    },
    viewGraph: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "iTrack_ImprovementActivities";
        LoadActionPan('iTrack_ImprovementActivitiesGraph', params);

    },
    printPreview: function () {
        var providerData = "";
        if (iTrack_ImprovementActivities.params.ProviderData != 'undefined' && iTrack_ImprovementActivities.params.ProviderData != null)
            providerData = iTrack_ImprovementActivities.params.ProviderData.split('<br/>');
        var newProviderText = '';
        for (var i = 0; i < providerData.length; i++) {
            if ($.trim(providerData[i]) != '') {
                newProviderText += '<li class="text-left">' + providerData[i] + '</li>';
            }
        }
        $('#' + iTrack_ImprovementActivities.params.PanelID + " #printcall #ProviderList").html(newProviderText);

        var practiceData = "";
        if (iTrack_ImprovementActivities.params.PracticeData != 'undefined' && iTrack_ImprovementActivities.params.PracticeData != null)
            practiceData = iTrack_ImprovementActivities.params.PracticeData.split('<br/>');
        var newPracticeText = '';
        for (var i = 0; i < practiceData.length; i++) {
            if ($.trim(practiceData[i]) != '') {
                newPracticeText += '<li class="text-right">' + practiceData[i] + '</li>';
            }
        }
        $('#' + iTrack_ImprovementActivities.params.PanelID + " #printcall #PracticeList").html(newPracticeText);

        var date = new Date();
        var day = date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();

        var mnth = day.split('/')[0];
        var dy = day.split('/')[1];
        var yr = day.split('/')[2];
        mnth = mnth.length == 1 ? "0" + mnth : mnth;
        dy = dy.length == 1 ? "0" + dy : dy;
        var curdate = mnth + "/" + dy + "/" + yr;
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        var time = strTime;
        var datetime = curdate + " " + time;

        $('#' + iTrack_ImprovementActivities.params.PanelID + " #printcall #liCurrentDate").text(datetime);
        params["UlContent"] = $("#" + iTrack_ImprovementActivities.params["PanelID"] + " #pnliTrackImprovementActivities_Result #dgviTrackImprovementActivities")[0].outerHTML;
        $('#' + iTrack_ImprovementActivities.params.PanelID + " #printcall #ulContent").append(params["UlContent"]);
        iTrack_ImprovementActivities.PrintReports();
    },
    PrintReports: function () {
        $('#' + iTrack_ImprovementActivities.params.PanelID + " #printcall").removeClass('hidden');
        kendo.drawing.drawDOM('#' + iTrack_ImprovementActivities.params["PanelID"] + " #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "7mm",
                right: "10mm",
                bottom: "15mm"
            },
            template: kendo.template($('#' + iTrack_ImprovementActivities.params["PanelID"] + " #page-templateLegacy").html())
        }).then(function (group) {

            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                params["PrintPDFDataURL"] = PrintPDFDataURL;
                params["PreviewPdf"] = true;
                utility.PDFViewer(params["PrintPDFDataURL"], true, null, null, true);
                $('#' + iTrack_ImprovementActivities.params.PanelID + " #printcall").addClass('hidden');
                $('#' + iTrack_ImprovementActivities.params.PanelID + " #printcall #ulContent").html("");
            });

        });
    },

}