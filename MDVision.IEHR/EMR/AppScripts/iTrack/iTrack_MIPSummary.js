/// <reference path="Clinical_MipsGraph.js" />
iTrack_MIPSummary = {
    params: [],
    bIsFirstLoad: true,
    listSearchLength: 0,
    isReporting: false,
    Load: function (params) {
        iTrack_MIPSummary.params = params;
        iTrack_MIPSummary.params.Groups = [];
        iTrack_MIPSummary.params.GroupsDetails = [];
        iTrack_MIPSummary.isReporting = false;
        if (iTrack_MIPSummary.params.PanelID != 'pnliTrackMIPSummary') {
            iTrack_MIPSummary.params.PanelID = iTrack_MIPSummary.params.PanelID + ' #pnliTrackMIPSummary';
        } else {
            iTrack_MIPSummary.params.PanelID = 'pnliTrackMIPSummary';
        }
        var self = $("#pnliTrackMIPSummary");
        self.loadDropDowns(true).done(function () {

            utility.CreateDatePicker(iTrack_MIPSummary.params.PanelID + " #dtpFromDate, #dtpToDate", function () {
            }, false);
            utility.ValidateFromToDate('frmiTrackMIPSummary', 'dtpFromDate', 'dtpToDate', true);
            utility.CreateDatePicker(iTrack_MIPSummary.params.PanelID + " #dtpGroupFromDate, #dtpGroupToDate", function () {
            }, false);
            utility.ValidateFromToDate('frmiTrackMIPSummary', 'dtpGroupFromDate', 'dtpGroupToDate', true);
            //$('#dtpFromDate').datepicker("setDate", '01/01/2018');
            //$('#dtpToDate').datepicker("setDate", '12/31/2018');
            //$('#dtpGroupFromDate').datepicker("setDate", '01/01/2018');
            //$('#dtpGroupToDate').datepicker("setDate", '12/31/2018');
            if (params.FromDashBoard == true) {
                params.FromDashBoard = false;

                if (params.NotReportingReason) {
                    iTrack_MIPSummary.isReporting = false;
                    $("#pnliTrackMIPSummary #MIPSGraphs").addClass('hidden');
                    $("#pnliTrackMIPSummary #dvNotReporting").removeClass('hidden');
                    $("#pnliTrackMIPSummary #dvNotReporting #r1").text(params.NotReportingReason);
                    $("#pnliTrackMIPSummary #frmiTrackMIPSummary #spnReportingVia").text(' | Not Reporting');
                    $("#pnliTrackMIPSummary #infoLabels #isIndividual").text(' | Individual Clinician');
                    $("#pnliTrackMIPSummary #infoLabels #lblProviderName").text('Provider: ' + globalAppdata["DefaultProviderName"] );
                    $("#pnliTrackMIPSummary #infoLabels #lblinfoPerformanceYear").text('| Performance Year: ' + $("#pnliTrackMIPSummary #IndividualReporting #ddlPerformanceYear :selected").text());

                    iTrack_MIPSummary.BindProvider(true);
                    $Ctrl_gr = $('#pnliTrackMIPSummary #txtIndividualProvider');
                    $hfCtrl_gr = $("#pnliTrackMIPSummary #hfProviderId");
                    var providerName = globalAppdata["DefaultProviderName"];
                    var providerId = globalAppdata["DefaultProviderId"];
                    utility.SetKendoAutoCompleteSourceforValidate($Ctrl_gr, providerName, $hfCtrl_gr, providerId);
                    $("#pnliTrackMIPSummary #txtIndividualProviderNPI").val(params.NPI);
                    $('#frmiTrackMIPSummary #dtpFromDate').val('01/01/2018');
                    $('#frmiTrackMIPSummary #dtpToDate').val('12/31/2018');
                    return;
                } else {

                    if (params.GroupId) {
                        iTrack_MIPSummary.BindGroupProviders();
                        $Ctrl_gr = $('#pnliTrackMIPSummary #txtGroupName');
                        $hfCtrl_gr = $('#pnliTrackMIPSummary #hfGroupId');
                        var groupName = params.GroupName;
                        var groupId = params.GroupId;
                        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_gr, groupName, $hfCtrl_gr, groupId);

                        $("#pnliTrackMIPSummary #frmiTrackMIPSummary #dtpGroupFromDate").val('01/01/2018');
                        $("#pnliTrackMIPSummary #frmiTrackMIPSummary #dtpGroupToDate").val('12/31/2018');
                        $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtGroupTIN").val(params.GroupTIN);
                        $("#pnliTrackMIPSummary #IndividualReporting").addClass('hidden');
                        $("#pnliTrackMIPSummary #GroupReporting").removeClass('hidden');

                        $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtGroupTIN").val(params.GroupTIN);
                        $("#pnliTrackMIPSummary #infoLabels #lblinfoPerformanceYear").text(" | Performance Year: " + $("#pnliTrackMIPSummary #GroupReporting #ddlPerformanceYearGroup").val());
                        $("#pnliTrackMIPSummary #infoLabels #lblProviderName").text('Group: ' + params.GroupName + " |");
                        $("#pnliTrackMIPSummary #infoLabels #isIndividual").text('');


                        iTrack_MIPSummary.isReporting = true;
                        iTrack_MIPSummary.searchMIPScore('GroupProvider');
                    } else {

                        $("#pnliTrackMIPSummary #IndividualReporting").removeClass('hidden');
                        $("#pnliTrackMIPSummary #GroupReporting").addClass('hidden');

                        $("#pnliTrackMIPSummary #infoLabels #isIndividual").text(' | Individual Clinician');
                        $("#pnliTrackMIPSummary #infoLabels #lblProviderName").text('Provider: ' + globalAppdata["DefaultProviderName"] + " |");
                        $("#pnliTrackMIPSummary #infoLabels #lblinfoPerformanceYear").text('| Performance Year: ' + $("#pnliTrackMIPSummary #IndividualReporting #ddlPerformanceYear").val());
                        $("#pnliTrackMIPSummary #infoLabels #spnReportingVia").text(' | Reporting Via ' + $("#pnliTrackMIPSummary #IndividualReporting #ddlReportingVia").val());

                        iTrack_MIPSummary.BindProvider(true);
                        $Ctrl_gr = $('#pnliTrackMIPSummary #txtIndividualProvider');
                        $hfCtrl_gr = $("#pnliTrackMIPSummary #hfProviderId");
                        var providerName = globalAppdata["DefaultProviderName"];
                        var providerId = globalAppdata["DefaultProviderId"];
                        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_gr, providerName, $hfCtrl_gr, providerId);
                        $("#pnliTrackMIPSummary #txtIndividualProviderNPI").val(params.NPI);
                        $('#frmiTrackMIPSummary #dtpFromDate').val('01/01/2018');
                        $('#frmiTrackMIPSummary #dtpToDate').val('12/31/2018');
                        iTrack_MIPSummary.isReporting = true;
                        iTrack_MIPSummary.searchMIPScore('IndProvider');
                    }

                }
            } 

            iTrack_MIPSummary.changeYearLabel($("#pnliTrackMIPSummary #IndividualReporting #ddlPerformanceYear").val());
            if (iTrack_MIPSummary.isReporting == true) {
                iTrack_MIPSummary.reportingVia($("#pnliTrackMIPSummary #IndividualReporting #ddlReportingVia").val());

            }
            iTrack_MIPSummary.BindProvider();
            iTrack_MIPSummary.BindGroupProviders();
            iTrack_MIPSummary.progressCircleNew();
            iTrack_MIPSummary.readyFunction();
            iTrack_MIPSummary.params.CostAchievedPoints = 0;
            iTrack_MIPSummary.params.ImprovementActivitiesAchievedPoints = 0;
            iTrack_MIPSummary.params.PIAchievedPoints = 0;
            iTrack_MIPSummary.params.qualityAchievedPoints = 0;
            $('#pnliTrackMIPSummary #ddlMemberProviders').multiselect({
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
    setPeriodsFrom: function (value) {

        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodFromPI').text(value);
        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodFromIA').text(value);

    },
    setPeriodsTo: function (value) {

        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodToPI').text(value);
        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodToIA').text(value);

    },
    OpenProviderDetail: function () {

        var params = [];
        params["ProviderId"] = $('#' + iTrack_MIPSummary.params.PanelID + ' #hfProviderId').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = 'txtIndividualProvider';
        params["ParentCtrl"] = 'iTrackTabMIPSummary';
        LoadActionPan('providerDetail', params);
    },
    HideProviderLink: function (value) {
        if (value == "") {
            $('#pnliTrackMIPSummary #txtIndividualProvider').attr("ProviderId", "-1");
            $('#pnliTrackMIPSummary #hfProviderId').val("-1");
            $("#pnliTrackMIPSummary #lnkProviderEdit").css("display", "none");
            $("#pnliTrackMIPSummary #lblProvider").css("display", "inline");
        } else {

            $("#pnliTrackMIPSummary #lnkProviderEdit").css("display", "inline");
            $("#pnliTrackMIPSummary #lblProvider").css("display", "none");
        }
    },
    HideGroupNameLink: function (value) {
        if (value == "") {
            $('#pnliTrackMIPSummary #txtGroupName').attr("ProviderId", "-1");
            $('#pnliTrackMIPSummary #hfGroupId').val("-1");
            $("#pnliTrackMIPSummary #lnkGroupEdit").css("display", "none");
            $("#pnliTrackMIPSummary #lblGroupName").css("display", "inline");
        } else {

            $("#pnliTrackMIPSummary #lnkGroupEdit").css("display", "inline");
            $("#pnliTrackMIPSummary #lblGroupName").css("display", "none");
       
        }
    },
    changeYearLabel: function (value) {
        if (value == 1)
            value = '2017';
        else if (value == 2)
            value = '2018';
        else if (value == '-select-')
            value = '';
        $("#pnliTrackMIPSummary #MIPSGraphs #lblyearQuality").text("Year: " + value);
        $("#pnliTrackMIPSummary #MIPSGraphs #lblyearIA").text("Year: " + value);
        $("#pnliTrackMIPSummary #MIPSGraphs #lblYearCompositeScore").text("Year: " + value);
        $("#pnliTrackMIPSummary #infoLabels #lblinfoPerformanceYear").text(" Performance Year: " + value);

    },
    GroupPreferencesEdit: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS Preference_Group / Virtual Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["GroupId"] = $("#pnliTrackMIPSummary #hfGroupId").val();
                params["mode"] = "Edit";
                params["FromAdmin"] = '0';
                params["ParentCtrl"] = 'iTrackTabMIPSummary';
                LoadActionPan('MIPS_AdminPreferenceGroupDetail', params);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtIndividualProvider";
        params["RefCtrlHidden"] = "hfProviderId";
        params["ParentCtrl"] = "iTrackTabMIPSummary";
        LoadActionPan('Admin_Provider', params);
    },
    readyFunction: function () {
        $("#pnliTrackMIPSummary #chkNotReporting").on('change', function () {

            if (this.checked) {
                $("#pnliTrackMIPSummary #MIPSGraphs").addClass('hidden');
                $("#pnliTrackMIPSummary #dvNotReporting").removeClass('hidden');
                //$("#pnliTrackMIPSummary #dvNotReporting #r1").text('Dr. ' + $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtProvider").val() + ' does not fulfill the eligibility criteria for reporting MIPS program.');
                //$("#pnliTrackMIPSummary #dvNotReporting #r2").text('He might report with any virtual group but he would not be performing Individual Reporting.');

            } else {
                $("#pnliTrackMIPSummary #MIPSGraphs").removeClass('hidden');
                $("#pnliTrackMIPSummary #dvNotReporting").addClass('hidden');
            }
        });

    },
    viewGroup: function (isFromSearch) {
         $('#pnliTrackMIPSummary #ddlMemberProviders').empty();
         $('#pnliTrackMIPSummary #ddlMemberProviders').multiselect("refresh");
         $('#pnliTrackMIPSummary #ddlMemberProviders').multiselect('rebuild');
         var ctrl = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtGroupName").data('kendoAutoComplete');
         if (ctrl) {
             ctrl.value('');
         }
        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodFromPI').text('');
        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodToPI').text('');
        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodFromIA').text('');
        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodToIA').text('');
        $("#pnliTrackMIPSummary #IndividualReporting").addClass('hidden');
        $("#pnliTrackMIPSummary #GroupReporting").removeClass('hidden');
        $("#pnliTrackMIPSummary #dvNotReporting").addClass('hidden');
        $("#pnliTrackMIPSummary #MIPSGraphs").removeClass('hidden');
        if (!isFromSearch) {
            $("#pnliTrackMIPSummary #txtGroupName").val('');
            $("#pnliTrackMIPSummary #txtGroupTIN").val('');
            $("#pnliTrackMIPSummary #hfGroupId").val('');
        }
        $("#pnliTrackMIPSummary #infoLabels #isIndividual").text('');
        $("#pnliTrackMIPSummary #infoLabels #lblProviderName").text('Group: | ');
        $("#pnliTrackMIPSummary #infoLabels #lblinfoPerformanceYear").text(' | Performance Year: | ');
        $("#pnliTrackMIPSummary #infoLabels #spnReportingVia").text(' | Reporting Via ');
        iTrack_MIPSummary.changeYearLabel($("#pnliTrackMIPSummary #GroupReporting #ddlPerformanceYearGroup").val());
        iTrack_MIPSummary.reportingVia($("#pnliTrackMIPSummary #GroupReporting #ddlReportingViaGroup").val());
        $("#pnliTrackMIPSummary #infoLabels #isIndividual").text('');

        $("#pnliTrackMIPSummary #MIPSGraphs #dvTotal").html("");
        $("#pnliTrackMIPSummary #MIPSGraphs #dvTotal").attr('data-percent', parseInt(25));
        $("#pnliTrackMIPSummary #MIPSGraphs #dvPromotingInteroperability").html("");
        $("#pnliTrackMIPSummary #MIPSGraphs #dvPromotingInteroperability").attr('data-percent', parseInt(0));
        $("#pnliTrackMIPSummary #MIPSGraphs #dvQuality").html("");
        $("#pnliTrackMIPSummary #MIPSGraphs #dvQuality").attr('data-percent', parseInt(0));
        iTrack_MIPSummary.progressCircleNew();

    },
    viewIndividualReporting: function (isFromSearch) {
        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodFromPI').text('');
        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodToPI').text('');
        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodFromIA').text('');
        $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodToIA').text('');
        $("#pnliTrackMIPSummary #hfGroupId").val('');
      $("#pnliTrackMIPSummary #txtGroupName").val('')
        $("#pnliTrackMIPSummary #IndividualReporting").removeClass('hidden');
        $("#pnliTrackMIPSummary #GroupReporting").addClass('hidden');
        $("#pnliTrackMIPSummary #dvNotReporting").addClass('hidden');
        $("#pnliTrackMIPSummary #MIPSGraphs").removeClass('hidden');
        if (!isFromSearch) {
            $("#pnliTrackMIPSummary #txtIndividualProvider").val('');
            $("#pnliTrackMIPSummary #hfProviderId").val('');
            $("#pnliTrackMIPSummary #txtIndividualProviderNPI").val('');
        }
        $("#pnliTrackMIPSummary #infoLabels #isIndividual").text(' | Individual Clinician');
        $("#pnliTrackMIPSummary #infoLabels #lblProviderName").text('Provider: | ');
        $("#pnliTrackMIPSummary #infoLabels #lblinfoPerformanceYear").text('Performance Year: | ');
        $("#pnliTrackMIPSummary #infoLabels #spnReportingVia").text(' | Reporting Via ');

        iTrack_MIPSummary.changeYearLabel($("#pnliTrackMIPSummary #IndividualReporting #ddlPerformanceYear").val());
        iTrack_MIPSummary.reportingVia($("#pnliTrackMIPSummary #IndividualReporting #ddlReportingVia").val());
        iTrack_MIPSummary.setProviderName();

        $("#pnliTrackMIPSummary #MIPSGraphs #dvTotal").html("");
        $("#pnliTrackMIPSummary #MIPSGraphs #dvTotal").attr('data-percent', parseInt(25));
        $("#pnliTrackMIPSummary #MIPSGraphs #dvPromotingInteroperability").html("");
        $("#pnliTrackMIPSummary #MIPSGraphs #dvPromotingInteroperability").attr('data-percent', parseInt(0));
        $("#pnliTrackMIPSummary #MIPSGraphs #dvQuality").html("");
        $("#pnliTrackMIPSummary #MIPSGraphs #dvQuality").attr('data-percent', parseInt(0));
        iTrack_MIPSummary.progressCircleNew();
    },
    viewGraph: function () {
        var params = [];
        if ($("#pnliTrackMIPSummary #IndividualReporting").hasClass('hidden') != true) {
            params["IsIndividual"] = "Individual Clinician";
            params["ProviderText"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtIndividualProvider").val();
            params["PerformanceYear"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #IndividualReporting #ddlPerformanceYear").val() == 1 ? "2017" : "2018";
        } else {
            params["IsIndividual"] = "Group Clinician";
            params["PerformanceYear"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #ddlPerformanceYearGroup").val() == 1 ? "2017" : "2018";
            params["ProviderText"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtGroupName").val();
        }

        params["FromAdmin"] = "0";
        params["qualityAchievedPoints"] = iTrack_MIPSummary.params.qualityAchievedPoints;
        params["PIAchievedPoints"] = iTrack_MIPSummary.params.PIAchievedPoints;
        params["ImprovementActivitiesAchievedPoints"] = iTrack_MIPSummary.params.ImprovementActivitiesAchievedPoints;
        params["CostAchievedPoints"] = iTrack_MIPSummary.params.CostAchievedPoints;
        params["ParentCtrl"] = "iTrackMIPSummary";
        LoadActionPan('iTrack_MIPSGraph', params);

    },
    progressCircleNew: function () {
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        var el = document.getElementsByClassName('progress-bar-circle-new'); // get canvas
        $(el).each(function (count) {
            var options = {
                percent: this.getAttribute('data-percent') || 0,
                textBefore: this.getAttribute('data-text-before') || "",
                textAfter: this.getAttribute('data-text-after') || "",
                isPercentIcon: this.getAttribute('data-is-percent') || true,
                size: this.getAttribute('data-size') || 100,
                lineWidth: this.getAttribute('data-line') || 100,
                rotate: this.getAttribute('data-rotate') || 0,
                primaryClr: "#0088cc",
                dangerClr: '#d2322d',
                successClr: '#47a447',
                defaultClr: '#555555',
                remainCircleClor: '#efefef'
            }


            $(this).append('<canvas id="' + count + '"></canvas>');
            var canvasID = $(this).children('canvas').attr("id");
            var canvas = document.getElementById(canvasID);


            var sectionContainer = document.createElement('section');
            var divTop = document.createElement('div');
            var divMiddle = document.createElement('div');
            var divBottom = document.createElement('div');


            divTop.textContent = options.textBefore;
            if ($(this).attr("id") == 'dvCost') {
                divMiddle.textContent = "10";
            } else if ($(this).attr("id") == 'dvImprovementActivities') {
                divMiddle.textContent = "15";
            } else if ($(this).attr("id") == 'dvTotal') {
                divMiddle.textContent = options.percent == "25" ? "25" : options.percent;
            } else {
                divMiddle.textContent = ($(this).attr("id") == "dvImprovementActivities" && options.percent != "0" ? "15" : options.percent);
            }
            divBottom.textContent = options.textAfter;

            sectionContainer.appendChild(divTop);
            sectionContainer.appendChild(divMiddle);
            sectionContainer.appendChild(divBottom);

            if (typeof (G_vmlCanvasManager) !== 'undefined') {
                G_vmlCanvasManager.initElement(canvas);
            }
            //if (canvas.getContext) {
            var tempCtrl = document.createElement('canvas');
            $(tempCtrl).attr('id', canvas.id);
            canvas = tempCtrl;
            //var ctx = tempCtrl.getContext('2d');
            var ctx = canvas.getContext('2d');

            canvas.width = canvas.height = options.size;

            this.appendChild(sectionContainer);
            this.lastElementChild.style.padding = options.lineWidth + "px";
            this.appendChild(canvas);

            //settings
            $(this).height(options.size);
            $(this).width(options.size);
            //settings for span
            $(this).children("span").css("line-height", options.size + "px");
            $(this).children("span").width(options.size);

            ctx.translate(options.size / 2, options.size / 2); // change center
            ctx.rotate((-1 / 2 + options.rotate / 180) * Math.PI); // rotate -90 deg
            //imd = ctx.getImageData(0, 0, 240, 240);
            var radius = (options.size - options.lineWidth) / 2;

            var drawCircle = function (color, lineWidth, percent) {
                percent = Math.min(Math.max(0, percent || 1), 1);
                ctx.beginPath();
                ctx.arc(0, 0, radius, 0, Math.PI * 2 * percent, false);
                ctx.strokeStyle = color;
                ctx.lineCap = 'square'; // butt, round or square
                ctx.lineWidth = lineWidth
                ctx.stroke();
            };
            drawCircle(options.remainCircleClor, options.lineWidth, 100 / 100);
            //circle themes color
            if (options.percent === "0") {
                return;
            }
            else if ($(this).hasClass("success") == true) {
                if ($(this).attr("id") == 'dvPromotingInteroperability')
                    drawCircle(options.successClr, options.lineWidth, options.percent / 25);
                else if ($(this).attr("id") == 'dvQuality')
                    drawCircle(options.successClr, options.lineWidth, options.percent / 50);
                else
                    drawCircle(options.successClr, options.lineWidth, options.percent / 100);
            }
            else if ($(this).hasClass("danger") == true) {
                if ($(this).attr("id") == 'dvPromotingInteroperability')
                    drawCircle(options.dangerClr, options.lineWidth, options.percent / 25);
                else if($(this).attr("id") == 'dvQuality')
                    drawCircle(options.dangerClr, options.lineWidth, options.percent / 50);
                else
                    drawCircle(options.dangerClr, options.lineWidth, options.percent / 100);
            }
            else if ($(this).hasClass("primary") == true) {
                if ($(this).attr("id") == 'dvPromotingInteroperability')
                    drawCircle(options.primaryClr, options.lineWidth, options.percent / 25);
                else if ($(this).attr("id") == 'dvQuality')
                    drawCircle(options.primaryClr, options.lineWidth, options.percent / 50);
                else
                    drawCircle(options.primaryClr, options.lineWidth, options.percent / 100);  
            }
            else {
                if ($(this).attr("id") == 'dvPromotingInteroperability')
                    drawCircle(options.defaultClr, options.lineWidth, options.percent / 25);
                else if ($(this).attr("id") == 'dvQuality')
                    drawCircle(options.defaultClr, options.lineWidth, options.percent / 50);
                else
                    drawCircle(options.defaultClr, options.lineWidth, options.percent / 100);
            }
            //}
        });//each function
    },
    ResetProvider: function () {
        $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtIndividualProvider").val('');
        $("#pnliTrackMIPSummary #hfProviderId").val('');
        if ($("#pnliTrackMIPSummary #frmiTrackMIPSummary #lnkProviderEdit").css("display") == "inline") {
            $("#pnliTrackMIPSummary #frmiTrackMIPSummary #lnkProviderEdit").css("display", "none");
            $("#pnliTrackMIPSummary #frmiTrackMIPSummary #lblProvider").css("display", "inline");
        }
    },
    ResetNPI: function () {
        $("#pnliTrackMIPSummary #hfProviderId").val("");
        $("#pnliTrackMIPSummary #txtIndividualProviderNPI").val("");
    },
    BindProvider: function () {
        var Ctrl = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtIndividualProvider");
        var func = function () { return iTrack_MIPSummary.GetMIPSProvider(Ctrl.val()) };
        var hfCtrl = $("#pnliTrackMIPSummary #hfProviderId");
        var onSelect = function (e) {
            $("#pnliTrackMIPSummary #hfProviderId").val(e.id);
            iTrack_MIPSummary.setProviderName();
            utility.FillProviderNPI('#pnliTrackMIPSummary #frmiTrackMIPSummary', '#hfProviderId', '#txtIndividualProviderNPI');
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);

    },
    BindGroupProviders: function () {
        var Ctrl = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtGroupName");
        var func = function () { return iTrack_MIPSummary.GetMIPSGroupProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnliTrackMIPSummary #hfGroupId");
        var onSelect = function () {
            iTrack_MIPSummary.setGroupProviderIDs();
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, null, onSelect);
    },
    resetGroup: function(){

        $("#pnliTrackMIPSummary #hfGroupId").val("");
        $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtGroupTIN").val("");
        $('#pnliTrackMIPSummary #ddlMemberProviders').val('');
        $('#pnliTrackMIPSummary #ddlMemberProviders').multiselect("refresh");
        $('#pnliTrackMIPSummary #ddlMemberProviders').multiselect('rebuild');

    },
    setGroupProviderIDs: function () {
        var groupId = $("#pnliTrackMIPSummary #hfGroupId").val();
        var groupDetails = $.grep(iTrack_MIPSummary.params.Groups, function (a) {
            return a.GroupId == groupId;
        });

        $.each(groupDetails, function (i, item) {
            item.PerformanceYear = item.PerformanceYear == "" ? "2018" : item.PerformanceYear;
            $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtGroupTIN").val(item.TIN);
            $("#pnliTrackMIPSummary #infoLabels #lblinfoPerformanceYear").text(" | Performance Year: " + item.PerformanceYear );
            $("#pnliTrackMIPSummary #infoLabels #lblProviderName").text('Group: ' + item.GroupName);
            $("#pnliTrackMIPSummary #infoLabels #isIndividual").text('');

        });
        if (groupDetails.length > 0 && groupDetails[0].IsReporting != "True") {
            iTrack_MIPSummary.isReporting = false;
            //$("#pnliTrackMIPSummary #infoLabels #btnOpenPreference").removeClass('hidden');
            $("#pnliTrackMIPSummary #MIPSGraphs").addClass('hidden');
            $("#pnliTrackMIPSummary #dvNotReporting").removeClass('hidden');
            $("#pnliTrackMIPSummary #dvNotReporting #r1").text(groupDetails[0].JoiningComments);
            $("#pnliTrackMIPSummary #frmiTrackMIPSummary #spnReportingVia").text(' | Not Reporting');

        } else {
            iTrack_MIPSummary.isReporting = true;
            $("#pnliTrackMIPSummary #infoLabels #btnOpenPreference").addClass('hidden');
            $("#pnliTrackMIPSummary #MIPSGraphs").removeClass('hidden');
            $("#pnliTrackMIPSummary #dvNotReporting").addClass('hidden');
        }
        $.each(iTrack_MIPSummary.params.GroupsDetails, function (i, item) {

            if (item.CategoryName == "Promoting Interoperability (Formally ACI)") {
                $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodFromPI').text(item.StartDate);
                $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodToPI').text(item.EndDate);
            } else if (item.CategoryName == "IA (Improvement Activities)") {
                $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodFromIA').text(item.StartDate);
                $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodToIA').text(item.EndDate);
            }

        });
        var members = "";
        if (iTrack_MIPSummary.params.MembersDetails && iTrack_MIPSummary.params.MembersDetails.length > 0 && groupId != "") {
            $.each(iTrack_MIPSummary.params.MembersDetails, function (i, item) {

                members += "," + item.ProviderId;
                $('#pnliTrackMIPSummary #ddlMemberProviders').append($('<option>', {
                    refvalue: item.ProviderName,
                    value: item.ProviderId,
                    text: item.ProviderName
                }));

            });
          
            $('#pnliTrackMIPSummary #ddlMemberProviders').val(members.split(','));
            $('#pnliTrackMIPSummary #ddlMemberProviders').multiselect("refresh");
            $('#pnliTrackMIPSummary #ddlMemberProviders').multiselect('rebuild');

        }
    },
    setProviderName: function () {

        $("#pnliTrackMIPSummary #frmiTrackMIPSummary #lblProviderName").text("Dr. " + $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtIndividualProvider").val() + "|");

        var providerId = $("#pnliTrackMIPSummary #hfProviderId").val();
        var providerDetails = $.grep(iTrack_MIPSummary.params.Groups, function (a) {
            return a.ProviderId == providerId;
        });
        var groupsDetails = $.grep(iTrack_MIPSummary.params.GroupsDetails, function (a) {
            return a.ProviderId == providerId;
        });
        $.each(providerDetails, function (i, item) {
            item.PerformanceYear = item.PerformanceYear == "" ? "2018" : item.PerformanceYear;
            $("#pnliTrackMIPSummary #frmiTrackMIPSummary #lblProviderName").text(item.ShortName);
            $("#pnliTrackMIPSummary #frmiTrackMIPSummary #spnReportingVia").text(" | Reporting Via " + item.ReportingMethod);
            $("#pnliTrackMIPSummary #infoLabels #lblinfoPerformanceYear").text(" | Performance Year: " + item.PerformanceYear );

        });
        if (providerDetails.length > 0 && providerDetails[0].IsReporting != "True") {
            iTrack_MIPSummary.isReporting = false;
            //$("#pnliTrackMIPSummary #infoLabels #btnOpenPreference").removeClass('hidden');
            $("#pnliTrackMIPSummary #MIPSGraphs").addClass('hidden');
            $("#pnliTrackMIPSummary #dvNotReporting").removeClass('hidden');
            $("#pnliTrackMIPSummary #dvNotReporting #r1").text(providerDetails[0].JoiningComments);
            $("#pnliTrackMIPSummary #frmiTrackMIPSummary #spnReportingVia").text(' | Not Reporting');

        } else {
            iTrack_MIPSummary.isReporting = true;
            $("#pnliTrackMIPSummary #infoLabels #btnOpenPreference").addClass('hidden');
            $("#pnliTrackMIPSummary #MIPSGraphs").removeClass('hidden');
            $("#pnliTrackMIPSummary #dvNotReporting").addClass('hidden');
            if (providerDetails.length > 0)
                $("#pnliTrackMIPSummary #IndividualPracticeType").text(" Practice Type: " + providerDetails[0].PracticeType);
        }
        $.each(groupsDetails, function (i, item) {

            if (item.CategoryName == "Promoting Interoperability (Formally ACI)") {
                $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodFromPI').text(item.StartDate);
                $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodToPI').text(item.EndDate);
            } else if (item.CategoryName == "IA (Improvement Activities)") {
                $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodFromIA').text(item.StartDate);
                $('#pnliTrackMIPSummary #MIPSGraphs #lblPeriodToIA').text(item.EndDate);
            }

        });
    },
    openPreferenceDetail: function () {
        if (!$("#pnliTrackMIPSummary #IndividualReporting").hasClass('hidden')) {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("MIPS Preference_Individual Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var params = [];
                    params["PreferenceId"] = null;
                    params["ProviderId"] = $('#pnliTrackMIPSummary #hfProviderId').val();
                    params["mode"] = "Edit";
                    params["FromAdmin"] = "0";
                    params["ParentCtrl"] = 'iTrackTabMIPSummary';
                    LoadActionPan('iTrack_AdminIPPreference', params);
                } else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
        } else {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("MIPS Preference_Group / Virtual Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var params = [];
                    params["GroupId"] = $('#pnliTrackMIPSummary #hfGroupId').val();;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = "0";
                    params["ParentCtrl"] = 'iTrackTabMIPSummary';
                    LoadActionPan('MIPS_AdminPreferenceGroupDetail', params);
                } else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
        }



    },
    reportingVia: function (value) {
        if (value == 1)
            value = " | Reporting Via MD Vision EHR";
        else if (value == 2)
            value = " | Reporting Via Sovereign Health Registry";
        else if (value == 3)
            value = " | Reporting Via Sovereign QCDR";
        else if (value == "-select-")
            value = "";
        $("#pnliTrackMIPSummary #frmiTrackMIPSummary #infoLabels #spnReportingVia").text(value);
    },
    searchMIPScore: function (tab) {
        if (!$("#pnliTrackMIPSummary #IndividualReporting").hasClass('hidden') && $("#pnliTrackMIPSummary #txtIndividualProvider").val() == "") {
            utility.DisplayMessages("Please Select Provider", 2);
            return;
        } else if (!$("#pnliTrackMIPSummary #GroupReporting").hasClass('hidden') && $("#pnliTrackMIPSummary #txtGroupName").val() == "") {
            utility.DisplayMessages("Please Select Group", 2);
            return;
        }
        if (!iTrack_MIPSummary.isReporting) {
            return;
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS_MIPS Summary", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                iTrack_MIPSummary.searchMIPScoreDB_Call().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var categoryValues = JSON.parse(response.MIPSKPIsLoad_JSON);

                        var totalScore = 0;
                        $.each(categoryValues, function (i, item) {
                            if (item.Category == "Quality") {
                                $("#pnliTrackMIPSummary #MIPSGraphs #dvQuality").html("");
                                $("#pnliTrackMIPSummary #MIPSGraphs #dvQuality").attr('data-percent', parseInt(item.value));
                                iTrack_MIPSummary.params.qualityAchievedPoints = parseInt(item.value);
                            } else if (item.Category == "PI") {
                                $("#pnliTrackMIPSummary #MIPSGraphs #dvPromotingInteroperability").html("");
                                $("#pnliTrackMIPSummary #MIPSGraphs #dvPromotingInteroperability").attr('data-percent', parseInt(item.value));
                                iTrack_MIPSummary.params.PIAchievedPoints = parseInt(item.value);
                            } else if (item.Category == "IA") {
                                $("#pnliTrackMIPSummary #MIPSGraphs #dvImprovementActivities").html("");
                                $("#pnliTrackMIPSummary #MIPSGraphs #dvImprovementActivities").attr('data-percent', parseInt("100"));
                                iTrack_MIPSummary.params.ImprovementActivitiesAchievedPoints = parseInt("15");
                            } else if (item.Category == "Cost") {
                                $("#pnliTrackMIPSummary #MIPSGraphs #dvCost").html("");
                                $("#pnliTrackMIPSummary #MIPSGraphs #dvCost").attr('data-percent', parseInt("100"));
                                iTrack_MIPSummary.params.CostAchievedPoints = parseInt("10");
                            }
                            if (item.Category != "Cost" && item.Category != "IA") {
                                totalScore += parseInt(item.value);
                            }
                        });
                        totalScore += parseInt('10');
                        totalScore += parseInt('15');
                        $("#pnliTrackMIPSummary #MIPSGraphs #dvTotal").html("");
                        $("#pnliTrackMIPSummary #MIPSGraphs #dvTotal").attr('data-percent', parseInt(totalScore));
                        iTrack_MIPSummary.progressCircleNew();
                        //if (tab == 'IndProvider') {
                        //    iTrack_MIPSummary.viewIndividualReporting(true);
                        //} else {
                        //    iTrack_MIPSummary.viewGroup(true);
                        //}
                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    searchMIPScoreDB_Call: function () {

        var objData = new Object();
        if ($("#pnliTrackMIPSummary #IndividualReporting").hasClass('hidden') != true) {
            objData["ProviderId"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #hfProviderId").val();
            objData["DateFrom"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #IndividualReporting #dtpFromDate").val();
            objData["DateTo"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #IndividualReporting #dtpToDate").val();
            objData["Year"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #IndividualReporting #ddlPerformanceYear").val();
            objData["ReportingMethod"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #IndividualReporting #ddlReportingVia").val();
        } else {
            objData["GroupId"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #hfGroupId").val();
            objData["DateFrom"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #dtpGroupFromDate").val();
            objData["DateTo"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #dtpGroupToDate").val();
            objData["Year"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #ddlPerformanceYearGroup").val();
            objData["ReportingMethod"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #ddlGroupReportingVia").val();
            objData["TIN"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #txtGroupTIN").val();
        }
        objData["commandType"] = "loadmipskpis";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackDashBoard");
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
                        iTrack_MIPSummary.params.Groups = ProviderLoadJSONData.Groups;
                        //$("#pnliTrackMIPSummary #hfGroupId").val(iTrack_MIPSummary.params.Groups[0].GroupId);
                        //$("#pnliTrackMIPSummary #txtGroupTIN").val(iTrack_MIPSummary.params.Groups[0].TIN);
                        iTrack_MIPSummary.params.GroupsDetails = ProviderLoadJSONData.GroupsDetail;
                        iTrack_MIPSummary.params.MembersDetails = ProviderLoadJSONData.GroupDetail;

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
    GetMIPSProvider: function (name, from) {
        var AllProviders = [];
        var IsValid = false;

        if (name != null && name.length > 1)
            IsValid = true;
        else
            IsValid = false;

        var dfd = new $.Deferred();

        if (IsValid) {
            iTrack_MIPSummary.Search_MIPSProviderPreferences_DBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    responseData = JSON.parse(responseData);
                    if (responseData.IndividualProCount > 0) {
                        var ProviderLoadJSONData = JSON.parse(responseData.IndividualProCountLoad_JSON);
                        iTrack_MIPSummary.params.Groups = ProviderLoadJSONData.Groups;
                        iTrack_MIPSummary.params.GroupsDetails = ProviderLoadJSONData.GroupsDetail;

                        $.each(ProviderLoadJSONData.Groups, function (i, item) {

                            AllProviders.push({ id: item.ProviderId, value: item.ShortName });

                        });


                    }
                }
                dfd.resolve(AllProviders);
                if (from == "ProviderPopUp") {
                    iTrack_MIPSummary.setProviderName();
                }
                
            });
        }
        else {
            dfd.resolve(AllProviders);
        }

        return dfd.promise();

    },
    Search_MIPSGroupPreferences_DBCall: function (name) {


        var objData = new Object();
        objData["EntityId"] = globalAppdata.SeletedEntityId;
        objData["GroupName"] = name;
        objData["PerformanceYear"] = $("#pnliTrackMIPSummary #frmiTrackMIPSummary #ddlPerformanceYearGroup").val() == 1 ? "2017" : "2018";
        objData["IsActive"] = "True";
        objData["pageNumber"] = 1;
        objData["RowsPerPage"] = 15;
        objData["commandType"] = "searchmimpsgrouppreferences";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    Search_MIPSProviderPreferences_DBCall: function (name) {


        var objData = new Object();
        objData["EntityId"] = globalAppdata.SeletedEntityId;
        objData["ShortName"] = name;
        objData["IsActive"] = "True";
        objData["commandType"] = "searchmipsproviderpreferences";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },

}