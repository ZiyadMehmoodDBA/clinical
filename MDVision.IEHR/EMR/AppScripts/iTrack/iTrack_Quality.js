/// <reference path="Clinical_MipsGraph.js" />
iTrack_Quality = {
    params: [],
    bIsFirstLoad: true,
    listSearchLength: 0,
    measureTable: null,
    graphObj: [],
    Load: function (params) {
        iTrack_Quality.params = params;
        iTrack_Quality.measureTable = null;
        if (iTrack_Quality.params.PanelID != 'pnliTrackQuality') {
            iTrack_Quality.params.PanelID = iTrack_Quality.params.PanelID + ' #pnliTrackQuality';
        } else {
            iTrack_Quality.params.PanelID = 'pnliTrackQuality';
        }
        var self = $("#pnliTrackQuality");
        self.loadDropDowns(true).done(function () {

            utility.CreateDatePicker(iTrack_Quality.params.PanelID + " #dtpFromDate, #dtpToDate", function () { }, false);
            utility.ValidateFromToDate('frmiTrackQuality', 'dtpFromDate', 'dtpToDate', true);
            iTrack_Quality.BindProvider();
            iTrack_Quality.BindGroupProviders();
            $('#pnliTrackQuality #dtpFromDate').datepicker("setDate", '01/01/2018');
            $('#pnliTrackQuality #dtpToDate').datepicker("setDate", '12/31/2018');
            $('#pnliTrackQuality #dtpGroupFromDate').datepicker("setDate", '01/01/2018');
            $('#pnliTrackQuality #dtpGroupToDate').datepicker("setDate", '12/31/2018');
            $('#pnliTrackQuality #ddlMemberProviders').multiselect({
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

        });

    },
    BindGroupProviders: function () {
        var Ctrl = $("#pnliTrackQuality #frmiTrackQuality #txtGroupName");
        var func = function () { return iTrack_Quality.GetMIPSGroupProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnliTrackQuality #hfGroupId");
        var onChange = function () { iTrack_Quality.setGroupProviderIDs(); };
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
                        iTrack_Quality.params.Groups = ProviderLoadJSONData.Groups;
                        iTrack_Quality.params.GroupsDetails = ProviderLoadJSONData.GroupsDetail;
                        iTrack_Quality.params.MembersDetails = ProviderLoadJSONData.GroupDetail;

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
        var groupId = $("#pnliTrackQuality #hfGroupId").val();
        var groupDetails = $.grep(iTrack_Quality.params.Groups, function (a) {
            return a.GroupId == groupId;
        });

        $.each(groupDetails, function (i, item) {
            item.PerformanceYear = item.PerformanceYear == "" ? "2018" : item.PerformanceYear;
            $("#pnliTrackQuality #frmiTrackQuality #txtGroupTIN").val(item.TIN);
            $("#pnliTrackQuality #frmiTrackQuality #lblProviderName").text('');
        });

        var members = "";
        if (iTrack_Quality.params.MembersDetails && iTrack_Quality.params.MembersDetails.length > 0) {
            $.each(iTrack_Quality.params.MembersDetails, function (i, item) {

                members += "," + item.ProviderId;
                $('#pnliTrackQuality #ddlMemberProviders').append($('<option>', {
                    refvalue: item.ProviderName,
                    value: item.ProviderId,
                    text: item.ProviderName
                }));

            });
            $('#pnliTrackQuality #ddlMemberProviders').val(members.split(','));
            $('#pnliTrackQuality #ddlMemberProviders').multiselect("refresh");
            $('#pnliTrackQuality #ddlMemberProviders').multiselect('rebuild');
            //$('#pnliTrackQuality #ddlMemberProviders').find('input["checkbox"]').prop('disable', true);
            // $('#pnliTrackQuality #ddlMemberProviders').multiselect('disable');
        }
    },
    viewIndividualReporting: function () {
        $("#pnliTrackQuality #dvIndividualReporting").removeClass('hidden');
        $("#pnliTrackQuality #dvGroupReporting").addClass('hidden');
        $("#pnliTrackQuality #lblNPI").text(' NPI: ');
        $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #IsGroupSearched").addClass("hide");
        $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #IsProviderSearched").removeClass("hide");
        $("#" + iTrack_Quality.params["PanelID"] + " #dgviTrackQuality").dataTable().fnClearTable();
        $("#" + iTrack_Quality.params["PanelID"] + " #dgviTrackQuality").dataTable().fnDestroy();
        $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #dgviTrackQuality tbody").find("tr").remove();
        $("#" + iTrack_Quality.params["PanelID"] + " #diviTrackQualityPaging").addClass('hidden');
    },
    viewGroup: function () {
        $("#pnliTrackQuality #dvIndividualReporting").addClass('hidden');
        $("#pnliTrackQuality #dvGroupReporting").removeClass('hidden');
        $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #IsProviderSearched").addClass("hide");
        $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #IsGroupSearched").removeClass("hide");
        $("#pnliTrackQuality #lblNPI").text(' Group TIN: ');
        $("#" + iTrack_Quality.params["PanelID"] + " #dgviTrackQuality").dataTable().fnClearTable();
        $("#" +iTrack_Quality.params["PanelID"]+ " #dgviTrackQuality").dataTable().fnDestroy();
        $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #dgviTrackQuality tbody").find("tr").remove();
        $("#" + iTrack_Quality.params["PanelID"] + " #diviTrackQualityPaging").addClass('hidden');
    },
    HideGroupNameLink: function (value) {
        if (value == "") {
            $('#pnliTrackQuality #txtGroupName').attr("ProviderId", "-1");
            $('#pnliTrackQuality #hfGroupId').val("-1");
            $("#pnliTrackQuality #lnkGroupEdit").css("display", "none");
            $("#pnliTrackQuality #lblGroupName").css("display", "inline");
        } else {

            $("#pnliTrackQuality #lnkGroupEdit").css("display", "inline");
            $("#pnliTrackQuality #lblGroupName").css("display", "none");
        }
        iTrack_Quality.loadGroupData();
    },
    GroupPreferencesEdit: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS Preference_Group / Virtual Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["GroupId"] = $("#pnliTrackQuality #hfGroupId").val();
                params["mode"] = "Edit";
                params["FromAdmin"] = '0';
                params["ParentCtrl"] = 'iTrack_Quality';
                LoadActionPan('MIPS_AdminPreferenceGroupDetail', params);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    OpenProviderDetail: function () {

        var params = [];
        params["ProviderId"] = $('#' + iTrack_Quality.params.PanelID + ' #hfProviderId').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = 'txtProvider';
        params["ParentCtrl"] = 'iTrack_Quality';
        LoadActionPan('providerDetail', params);
    },
    HideProviderLink: function (value) {
        if (value == "") {
            $('#pnliTrackQuality #txtProvider').attr("ProviderId", "-1");
            $('#pnliTrackQuality #hfProviderId').val("-1");
            $("#pnliTrackQuality #lnkProviderEdit").css("display", "none");
            $("#pnliTrackQuality #lblProvider").css("display", "inline");
        } else {

            $("#pnliTrackQuality #lnkProviderEdit").css("display", "inline");
            $("#pnliTrackQuality #lblProvider").css("display", "none");
        }
    },
    QualityMeasureSearch: function (cqmid, pageNo, rpp) {

        if (!$("#pnliTrackQuality #dvIndividualReporting").hasClass('hidden') && ($("#pnliTrackQuality #txtProvider").val() == "" || $("#pnliTrackQuality #txtProvider").val() == null)) {
            utility.DisplayMessages("Please Select Provider", 2);
            return;
        } else if (!$("#pnliTrackQuality #dvGroupReporting").hasClass('hidden') && ($("#pnliTrackQuality #txtGroupName").val() == "" || $("#pnliTrackQuality #txtGroupName").val() == null)) {
            utility.DisplayMessages("Please Select Group", 2);
            return;
        }

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS_Quality", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #lblProviderName").text($("#" + iTrack_Quality.params.PanelID + " #txtProvider").val());
                $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #lblNPI").text($("#pnliTrackQuality #NPItxt").val());
                var year = "";
                if ($("#" + iTrack_Quality.params["PanelID"] + " #ddlPerformanceYear").val() == 1) {
                    year = 'Full Year | 01/01/2017 - 12/31/2017';
                } else if ($("#" + iTrack_Quality.params["PanelID"] + " #ddlPerformanceYear").val() == 2) {
                    year = 'Full Year | 01/01/2018 - 12/31/2018';
                }
                $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #lblPerformanceyear").text(year);

                if ($("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result").css("display") === "none")
                    $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result").show();

                var self = $("#" + iTrack_Quality.params["PanelID"] + " #pnlQualityMeasure_Search");
                var myJsonByName = self.getMyJSONByName();
                var objData = JSON.parse(myJsonByName);



                objData["ProviderId"] = $('#' + iTrack_Quality.params.PanelID + ' #hfProviderId').val();
                objData["GroupId"] = $('#' + iTrack_Quality.params.PanelID + ' #hfGroupIdId').val();
                objData["PatientId"] = $('#' + iTrack_Quality.params.PanelID + ' #hfPatientId').val();
                objData["Problems"] = [];
                var myJson = JSON.stringify(objData);

                if ($("#ddlprovider :selected").val() == "" && $("#dtpDateFrom").val() == "" && $("#dtpDateTo").val() == "") {
                    utility.DisplayMessages("Specify a Criteria to Filter", 3);
                } else {
                    iTrack_Quality.SearchQualityMeasure(myJson, cqmid, pageNo, rpp).done(function (response) {
                        if (response != null && response.status == null) {
                            response = JSON.parse(response);
                        }
                        if (response.status != false) {
                            iTrack_Quality.QualityMeasureGridLoad(response, response.iTotalDisplayRecords);

                            var tableControl = iTrack_Quality.params["PanelID"] + " #dgviTrackQuality";
                            var pagingPanelControlId = iTrack_Quality.params["PanelID"] + " #diviTrackQualityPaging";
                            var classControlName = "iTrack_Quality";
                            var pagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout
                            (
                                CreatePagination(response.BatchClinicalQualityMeasureCount, pageNo, 30, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords,
                                    function (primaryId, pageNumber, resultPerPage) {
                                        iTrack_Quality.QualityMeasureSearch(primaryId, pageNumber, 30);
                                    }
                                ), 10);
                        } else {
                            $("#" + iTrack_Quality.params["PanelID"] + " #dgviTrackQuality").dataTable().fnDestroy();
                            $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality #dgviTrackQuality tbody").find("tr").remove();
                            $("#" + iTrack_Quality.params["PanelID"] + " #diviTrackQualityPaging").remove();

                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    OnClickPreventDefault: function (event) {
        if (event != null)
            event.stopPropagation();
    },
    QualityMeasureGridLoad: function (response, count) {
        $("#" + iTrack_Quality.params["PanelID"] + " #dgviTrackQuality").dataTable().fnClearTable();
        $("#" + iTrack_Quality.params["PanelID"] + " #dgviTrackQuality").dataTable().fnDestroy();
        $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #dgviTrackQuality tbody").find("tr").remove();
        $("#" + iTrack_Quality.params["PanelID"] + " #diviTrackQualityPaging").removeClass('hidden');

        var TotalAcievementPoints = 0;
        var TotalBonusPoints = 0;
        var arraTemp = [];
        var counter = 0;
        if (response.BatchClinicalQualityMeasureCount > 0) {
            var QualityMeasureLoadJson = JSON.parse(response.BatchClinicalQualityMeasureLoad_JSON);
            iTrack_Quality.graphObj = [];
            var totalScore = 0;
            var arr = [];
            $.each(QualityMeasureLoadJson, function (i, item) {
                if (item.CQMID != "") {
                    Measures = {
                        'name': item.Measure,
                        'range': item.Lowerdecilebound + " - " + item.UpperDecileBound,
                        'PerformanceRate': item.PerfromanceRate1,
                        'MeasureNumber': item.MeasureNumber,
                    };
                    TotalAcievementPoints += parseFloat(item.Optional2);
                    TotalBonusPoints += Math.round(item.BonusPoint);
                    iTrack_Quality.graphObj.push(Measures);
                    var $row = $("<tr/>");

                    $row.attr("id", "gvBatchClinicalQualityMeasure_row" + item.CQMID);
                    $row.attr("class", "Parent");
                    $row.attr("onclick", "iTrack_Quality.QualityMeasureDetail('Edit','" + item.CQMID + "','" + response.ProviderId + "','" + item.Measure + "','" + item.InitialPopulation + "',event,'" + item.MeasureNumber + "','MainRow');utility.SelectGridRow($(this));");
                    $row.attr("CQMID", item.CQMID);
                    var barClass = 'info';
                    if (parseFloat(item.PerfromanceRate1) <= 0) {
                        barClass = 'danger';
                    } else if (parseFloat(item.PerfromanceRate1) > 0 && parseFloat(item.PerfromanceRate1) < parseFloat(item.Lowerdecilebound)) {
                        barClass = 'info';
                    } else if (parseFloat(item.PerfromanceRate1) >= parseFloat(item.Lowerdecilebound) && parseFloat(item.PerfromanceRate1) <= parseFloat(item.UpperDecileBound)) {
                        barClass = 'success';
                    } else if (parseFloat(item.PerfromanceRate1) > parseFloat(item.UpperDecileBound)) {
                        barClass = 'warning';
                    } else if (item.PerfromanceRate1 == "") {
                        barClass = 'danger';
                    }
                    if (item.CQMID.indexOf(')B') > -1 || item.CQMID.indexOf(')C') > -1) {
                        counter++;
                    }
                    if (item.CQMID.indexOf(')B') < 0 && item.CQMID.indexOf(')C') < 0 && parseInt(item.Optional2) > 0) {
                        totalScore += parseInt(item.Optional2);
                        totalScore += 1;
                    }
                    var performanceRate = parseInt(item.PerfromanceRate1);
                    var performancebaar = '';
                    if (barClass == "warning") {
                        performancebaar = '<div class="progress mb-xxs"><div style="background-color:#cccc00; width:' + performanceRate + '%" class="white progress-bar" role="progressbar" aria-valuenow="' + performanceRate + '"aria-valuemin="0" aria-valuemax="100" data-toggle="tooltip" data-original-title="Performance Met Range:' + item.Lowerdecilebound + ' - ' + item.UpperDecileBound + '"> ' + item.PerfromanceRate1 + '%</div></div>';
                    } else if (barClass == "info") {
                        performancebaar = '<div class="progress mb-xxs"><div class="white progress-bar progress-bar-' + barClass + '" role="progressbar" aria-valuenow="' + performanceRate + '"aria-valuemin="0" aria-valuemax="100" style="width:' + performanceRate + '%" data-toggle="tooltip" data-original-title="Performance Met Range:' + item.Lowerdecilebound + ' - ' + item.UpperDecileBound + '"> ' + item.PerfromanceRate1 + '%</div></div>';
                    }
                    else {
                        performancebaar = '<div class="progress mb-xxs"><div class="white progress-bar progress-bar-' + barClass + '" role="progressbar" aria-valuenow="' + performanceRate + '"aria-valuemin="0" aria-valuemax="100" style="width:100%" data-toggle="tooltip" data-original-title="Performance Met Range:' + item.Lowerdecilebound + ' - ' + item.UpperDecileBound + '"> ' + item.PerfromanceRate1 + '%</div></div>';

                    }
                    if (item.MeasureNumber == "CMS138v6(A)") {
                        item.MeasureNumber = "CMS138v6"
                    }
                    var icon = '<a  href="javacript:void(0);" class="on-editing expand-row font-xs" title="Expand/Collapse Record" onclick="iTrack_Quality.stopProp(this,event);"><i class="fa fa-caret-right"></i></a>';
                    var SelectionCheckBoxColumn = "";
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="iTrack_Quality.OnClickPreventDefault(event);" id="' + item.CQMID + '" name="SelectCheckBoxOrder"  class="input-block"/></td>';
                    $row.append(SelectionCheckBoxColumn + "<td style=\"display:none;\">" + item.CQMID + "</td><td id='MeasureNumber'>" + item.MeasureNumber + "</td><td id='Measure'>" + icon + " " + item.Measure + "</td><td id='Denominator'>" + item.Denominator + "</td><td id='Numerator'>" + item.Numerator + "</td><td id='Exclusion'>" + item.DenominatorExclusion + "</td><td id='Exception'>" + item.DenominatorException + "</td><td id='PerformanceRate'>" + performancebaar + "</td><td id='AchievementPoints'>" + item.Optional2 + "</td><td id='BonusPoints'>" + Math.round(item.BonusPoint) + "</td><td id='NonCompliance'><a class='btn-default pl-xs pr-xs btn btn-xs size85 active' onclick=\"iTrack_Quality.QualityMeasureDetail('Edit','" + item.CQMID + "','" + response.ProviderId + "','" + item.Measure + "','" + item.InitialPopulation + "',event,'" + item.MeasureNumber + "','NonCompliance');\">Patient(" + item.Optional1 + ")</a></td>");
                    $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #dgviTrackQuality tbody").last().append($row);

                    var CurrentRowchilds = $();
                    //var childRow = '<div class="col-xs-12 p-none pt-xs pb-xs"><table class="table-va-top table-space-td" width="100%"><tr class="text-uppercase"><th class="pb-default size100">MEASURE NUMBER</th><th class="pb-default size200">MEASURE DESCRIPTION</th><th class="pb-default size100">MEASURE TYPE</th><th class="pb-default size150">DATA SUBMISSION METHOD</th><th class="pb-default size150">SPECIALTY MEASURE SET</th><th class="pb-default size100">BENCHMARK</th></tr><tbody><tr><td><ul class="list-unstyled"><li>- eMeasure NQF: N/A</li><li>- NQF: N/A</li></ul></td><td><p>' + item.Measure + '<a href="https://qpp.cms.gov/mips/overview" target="_blank">LearnMore <i class="fa fa-external-link"></i></a></p></td><td><ul class="list-unstyled"><li><li>Process</li><b>NQS Domain</b></li><li>ECC</li></ul></td><td><ul class="list-unstyled"><li>- Claims</li><li>- EHR</li><li>- CMS Web Interface</li><li>- Registry</li></ul></td><td><ul class="list-unstyled"><li>- Internal Medicine</li><li>- Obstetrics/ Gynecology</li><li>- Preventive Medicine</li><li>- General Practice/ Family Medicine</li></ul> </td><td><ul class="list-unstyled"><li>' + item.BenchMark + '%</li></ul></td></tr></tbody></table></div>';

                    var submissionhtml = '';
                    var submissions = item.DataSubmissionMethod.split(',');
                    $.each(submissions, function (i, v) {
                        submissionhtml += '<li>- ' + v + '</li>';
                    });
                    var specialtyhtml = '';
                    var specialties = item.SpecialtyMeasureSet.split(',');
                    $.each(specialties, function (i, v) {
                        specialtyhtml += '<li>- ' + v + '</li>';
                    });
                    var NQSDomain = "ECC";
                    if (item.MeasureNumber == "SMS50v5") {
                        NQSDomain = "CCC";
                    } else if (item.MeasureNumber == "CMS68v6" || item.MeasureNumber == "CMS139v5" || item.MeasureNumber == "CMS156v5") {
                        NQSDomain = "PS";
                    } else if (item.MeasureNumber == "CMS127v5" || item.MeasureNumber == "CMS69v5" || item.MeasureNumber == "CMS147v6" || item.MeasureNumber == "CMS2v6" || item.MeasureNumber == "CMS22v5" || item.MeasureNumber == "CMS138v5") {
                        NQSDomain = "CPH";
                    }
                    var childRow = '<div class="col-xs-12 p-none pt-xs pb-xs"><table  id="childTable"class="table-va-top table-space-td" width="100%"><tr class="text-uppercase"><th class="pb-default size100">MEASURE NUMBER</th><th class="pb-default size200">MEASURE DESCRIPTION</th><th class="pb-default size100">MEASURE TYPE</th><th class="pb-default size150">DATA SUBMISSION METHOD</th><th class="pb-default size150">SPECIALTY MEASURE SET</th><th class="pb-default size100">BENCHMARK</th></tr><tbody><tr><td><ul class="list-unstyled"><li>- eMeasure NQF: N/A</li><li id="nqf">- NQF: ' + item.MIPsNQF + '</li><li id="qualityId">- Quality ID: ' + item.QualityID + '</li></ul></td><td><p id="MeasureDescription">' + item.MeasureDescription + '<a href="https://qpp.cms.gov/mips/overview" target="_blank"> LearnMore <i class="fa fa-external-link"></i></a></p></td><td><ul class="list-unstyled"><li><li id="MeasureType">' + item.MeasureType + '</li><b>NQS Domain</b></li><li id="NQSDomain">' + NQSDomain + '</li></ul></td><td id="Submission"><ul class="list-unstyled">' + submissionhtml + '</ul></td><td id="Specialty"><ul class="list-unstyled">' + specialtyhtml + '</ul> </td><td><ul class="list-unstyled"><li>' + item.BenchMark + '%</li></ul></td></tr></tbody></table></div>';

                    CurrentRowchilds = CurrentRowchilds.add(childRow);
                    arraTemp.push({ row: $row, childs: CurrentRowchilds });

                }
                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            });
         
            totalScore = parseInt(totalScore);
            var divider = count - counter;
            divider = parseInt(divider);
            divider *= 10;
            totalScore /= parseInt(divider);
            totalScore *= 100;
            totalScore = Math.round(totalScore);
            $("#" + iTrack_Quality.params["PanelID"] + " #headingProgressbarQuality").text("QUALITY Score - " + totalScore + " %");
            $("#" + iTrack_Quality.params["PanelID"] + " #progressbarQuality").attr('aria-valuenow', totalScore);
            $("#" + iTrack_Quality.params["PanelID"] + " #progressbarQuality").css('width', totalScore + "%");


            if ($("#dtpDateFrom").val() == "") {
                $("#dtpDateFrom").datepicker("setDate", response.DateFrom);
            }
            if ($("#dtpDateTo").val() == "") {
                $("#dtpDateTo").datepicker("setDate", response.DateTo);
            }
        }
        else {
            $("#" + iTrack_Quality.params["PanelID"] + " #dgviTrackQuality").DataTable({
                "language": {
                    "emptyTable": "No Measure Found"
                },
                "autoWidth": false,
                "bLengthChange": false,
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });

        }
        if ($.fn.dataTable.isDataTable("#" + iTrack_Quality.params["PanelID"] + " #dgviTrackQuality"));
        else {
            iTrack_Quality.measureTable = $('#' + iTrack_Quality.params.PanelID + ' #pnliTrackQuality_Result #dgviTrackQuality').DataTable({ "bInfo": false, "searching": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

                   $.each(arraTemp, function (i, item) {

                if (iTrack_Quality.measureTable != null) {
                    var row = iTrack_Quality.measureTable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }

                }

            });
            $('#' + iTrack_Quality.params.PanelID + ' #pnliTrackQuality_Result #dgviTrackQuality').off()
           .on('click', 'a.expand-row', function (e) {
               e.preventDefault();

               iTrack_Quality.rowExpand($(this).closest('tr'));
           });
        }

        var table = $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #dgviTrackQuality").DataTable();
        // Sort by column 1 and then re-draw
        table
        .order([1, 'asc'])
        .draw();
        var TotalRow = "<tr><td style='display:none;'></td><td colspan='8' ><label class='control-label'><strong>Total Score</strong> (Points) </label></td><td><strong>" + TotalAcievementPoints + "</strong> Points</td><td><strong>" + TotalBonusPoints + "</strong> Points</td><td></td></tr>";
        $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #dgviTrackQuality tbody").last().append(TotalRow);
    },
    stopProp: function (obj, event) {

        event.stopPropagation();
        iTrack_Quality.rowExpand($(obj).closest('tr'));

    },
    QualityMeasureDetail: function (mode, cqmid, providerId, measure, iP, event, measureNumber, from) {
        if (event != null)
            event.stopPropagation();

        var self = $("#" + iTrack_Quality.params["PanelID"] + " #pnlQualityMeasure_Search");
        var myJsonByName = self.getMyJSONByName();
        var objData = JSON.parse(myJsonByName);

        objData["Age"] = null;
        objData["CQMId"] = cqmid;
        objData["ProviderId"] = $('#' + iTrack_Quality.params.PanelID + ' #hfProviderId').val();
        objData["PatientId"] = $('#' + iTrack_Quality.params.PanelID + ' #hfPatientId').val();
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
        params["ParentCtrl"] = "iTrack_Quality";
        params["dateFrom"] = dateFrom;
        params["dateTo"] = dateTo;
        params["iP"] = iP;
        params["MeasureNumber"] = measureNumber;
        params["IsNonCompliance"] = from == "NonCompliance" ? "True" : "False";
        params["CQMSearchData"] = myJson;
        if (iP == "0") {
            utility.DisplayMessages("There is nothing to Show.", 3);
        } else {
            LoadActionPan("iTrack_QualityMeasureDetail", params);
        }
    },
    checkUncheckAllOrders: function (obj) {
        if ($(obj).is(':checked')) {
            $("#" + iTrack_Quality.params["PanelID"] + " #dgviTrackQuality input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#" + iTrack_Quality.params["PanelID"] + " #dgviTrackQuality input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }
    },
    ExportData: function (e) {

        var JSONData = [];
        $("#" + iTrack_Quality.params.PanelID + " #dgviTrackQuality tbody tr.Parent").each(function () {

            var obj = {
                MeasureNumber: $(this).find("#MeasureNumber").text().trim(),
                Measure: $(this).find("#Measure").text().trim(),
                Denominator: $(this).find("#Denominator").text().trim(),
                Numerator: $(this).find("#Numerator").text().trim(),
                Exclusion: $(this).find("#Exclusion").text().trim(),
                Exception: $(this).find("#Exception").text().trim(),
                AchievementPoints: $(this).find("#AchievementPoints").text().trim(),
                BonusPoints: $(this).find("#BonusPoints").text().trim(),
                PerformanceRate: $(this).find("#PerformanceRate").text().trim(),
                NonCompliance: $(this).find("#NonCompliance").text().trim(),

            }
            JSONData.push(obj);

        });
        iTrack_Quality.ExportDataToExcel(JSONData);
    },
    ExportDataToExcel: function (JSONData) {

        var ReportTitle = "MIPS Quality";
        var ShowLabel = true;
        var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

        var CSV = '';
        CSV += ReportTitle + '\r\n\n';
        if (ShowLabel) {
            var row = "";
            for (var index in arrData[0]) {

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

        var row = iTrack_Quality.measureTable.row($row);
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
        if ($("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #btnExpandAll i").hasClass('fa-caret-right')) {
            $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #dgviTrackQuality tbody tr").each(function () {
                var tr = $(this);
                tr.find('.fa-caret-right').addClass('fa-caret-down').removeClass('fa-caret-right');
                var row = iTrack_Quality.measureTable.row(tr);
                row.child.show();
            });
            $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #btnExpandAll i").removeClass('fa-caret-right').addClass('fa-caret-down');
        } else {
            $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #dgviTrackQuality tbody tr").each(function () {
                var tr = $(this);
                tr.find('.fa-caret-down').addClass('fa-caret-right').removeClass('fa-caret-down');
                var row = iTrack_Quality.measureTable.row(tr);
                row.child.hide();
            });
            $("#" + iTrack_Quality.params["PanelID"] + " #pnliTrackQuality_Result #btnExpandAll i").addClass('fa-caret-right').removeClass('fa-caret-down');
        }

    },
    SearchQualityMeasure: function (QualityMeasureData, cqmid, pageNumber, rowsPerPage) {

        var objData = new Object();
        if ($("#pnliTrackQuality #dvIndividualReporting").hasClass('hidden') == true) {

            objData["GroupId"] = $("#pnliTrackQuality #frmiTrackQuality #hfGroupId").val();
            objData["DateFrom"] = $("#pnliTrackQuality #frmiTrackQuality #dtpGroupFromDate").val();
            objData["DateTo"] = $("#pnliTrackQuality #frmiTrackQuality #dtpGroupToDate").val();
            objData["Year"] = $("#pnliTrackQuality #frmiTrackQuality #ddlPerformanceYearGroup").val();
            objData["TIN"] = $("#pnliTrackQuality #frmiTrackQuality #txtGroupTIN").val();
            objData["commandType"] = "loadgroupqualitymeasures";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "iTrack", "iTrackDashBoard");
        } else {
            
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
            a.GroupName = null;
            a.TIN = null;
            QualityMeasureData = JSON.stringify(a);

            var data = "BatchClinicalQualityMeasureData=" + QualityMeasureData + "&CQMID=" + cqmid + "&PageNumber=" + pageNumber + "&RowsPerPage=" + rowsPerPage;
            return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "SEARCH_CQM_MEASURES");
        }
    },
    ResetNPI: function () {
        $("#pnliTrackQuality #hfProviderId").val("");
        $("#pnliTrackQuality #NPItxt").val("");
    },
    ResetProvider: function () {
        $("#" + iTrack_Quality.params.PanelID + " #txtProvider").val('');
        $("#" + iTrack_Quality.params.PanelID + " #hfProviderId").val('');
        if ($("#pnliTrackQuality #frmiTrackQuality #lnkProviderEdit").css("display") == "inline") {
            $("#pnliTrackQuality #frmiTrackQuality #lnkProviderEdit").css("display", "none");
            $("#pnliTrackQuality #frmiTrackQuality #lblProvider").css("display", "inline");
        }
    },
    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["RefCtrlHidden"] = "hfProviderId";
        params["ParentCtrl"] = "iTrack_Quality";
        LoadActionPan('Admin_Provider', params);
    },
    BindProvider: function () {
        var Ctrl = $("#pnliTrackQuality #frmiTrackQuality #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnliTrackQuality #hfProviderId");
        var onSelect = function (e) {
            $("#pnliTrackQuality #hfProviderId").val(e.id);
            iTrack_Quality.setProviderName(); utility.FillProviderNPI('#pnliTrackQuality #frmiTrackQuality', '#hfProviderId', '#NPItxt');
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);


    },
    setProviderName: function () {

        $("#pnliTrackQuality #frmiTrackQuality #lblProviderName").text("Dr. " + $("#pnliTrackQuality #frmiTrackQuality #txtProvider").val() + "|");
    },
    viewGraph: function () {

        var params = [];
        params["ProviderText"] = $("#pnliTrackQuality #frmiTrackQuality #txtProvider").val();
        params["ProviderNPI"] = $("#pnliTrackQuality #NPItxt").val();
        params["PerformanceYear"] = $("#" + iTrack_Quality.params["PanelID"] + " #ddlPerformanceYear").val() == 1 ? "2017" : "2018";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "iTrack_Quality";
        params["GraphObj"] = iTrack_Quality.graphObj;
        LoadActionPan('iTrack_QualityGraph', params);

    },
    loadGroupData: function () {

        iTrack_Quality.loadGroupData_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var dta = JSON.parse(response.IndividualProCountLoad_JSON)

                var members = "";
                if (dta.GroupDetail && dta.GroupDetail.length > 0) {
                    $.each(dta.GroupDetail, function (i, item) {

                        members += "," + item.ProviderId;


                    });
                    var memberslist = members.split(',');
                    $('#' + iTrack_Quality.params["PanelID"] + ' #ddlMemberProvider').val(memberslist);
                    $('#' + iTrack_Quality.params["PanelID"] + ' #ddlMemberProvider').multiselect("refresh");

                }
                else {
                    $('#' + iTrack_Quality.params["PanelID"] + ' #ddlMemberProvider').val(memberslist);
                    $('#' + iTrack_Quality.params["PanelID"] + ' #ddlMemberProvider').multiselect("refresh");
                }
            }


        });
    },
    loadGroupData_DBCall: function () {
        var objData = new Object();
        objData["GroupName"] = $("#pnliTrackQuality #frmiTrackQuality #txtGroupName").val();
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["ReportingMethod"] = "MD Vision EHR";

        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 15;
        objData["commandType"] = "searchmimpsgrouppreferences";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    ClinicalQualityMeasureExport: function () {
        iTrack_Quality.ClinicalQualityMeasureExport_().done(function (response) {
            if (response.status != false) {
                download("data:application/octet-stream;base64," + response.DownloadFile, response.FileName + ".xml", "application/octet-stream");

            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    ClinicalQualityMeasureExport_: function () {

        var cqmid = "QRDA ||| ";

        var providerId = $('#' + iTrack_Quality.params.PanelID + ' #hfProviderId').val();
        var appointmentDateFrom = $("#dtpFromDate").val();
        var appointmentDateTo = $("#dtpToDate").val();

        var data = "CQMID=" + cqmid + "&providerId=" + providerId + "&AppointmentDateFrom=" + appointmentDateFrom + "&AppointmentDateTo=" + appointmentDateTo + "&FROM=" + "Main";
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "EXPORT_CQM_MEASURES");
    },
    /////////////End MIPS Section
}