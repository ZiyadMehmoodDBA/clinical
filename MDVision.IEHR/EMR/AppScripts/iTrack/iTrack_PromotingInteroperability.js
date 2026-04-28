/// <reference path="Clinical_MipsGraph.js" />
iTrack_PromotingInteroperability = {
    params: [],
    bIsFirstLoad: true,
    listSearchLength: 0,
    measureTable: null,
    MUdata_Patient: [],
    graphObj: [],
    Load: function (params) {
        iTrack_PromotingInteroperability.params = params;
        iTrack_PromotingInteroperability.measureTable = null;
        if (iTrack_PromotingInteroperability.params.PanelID != 'pnliTrackPromotingInteroperability') {
            iTrack_PromotingInteroperability.params.PanelID = iTrack_PromotingInteroperability.params.PanelID + ' #pnliTrackPromotingInteroperability';
        } else {
            iTrack_PromotingInteroperability.params.PanelID = 'pnliTrackPromotingInteroperability';
        }
        var self = $("#pnliTrackPromotingInteroperability");
        self.loadDropDowns(true).done(function () {
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            utility.CreateDatePicker(iTrack_PromotingInteroperability.params.PanelID + " #dtpFromDate, #dtpToDate", function () { }, false);
            utility.ValidateFromToDate('frmiTrackPromotingInteroperability', 'dtpFromDate', 'dtpToDate', true);
            utility.CreateDatePicker(iTrack_PromotingInteroperability.params.PanelID + " #dtpGroupFromDate, #dtpGroupToDate", function () {
            }, false);
            utility.ValidateFromToDate('frmiTrackPromotingInteroperability', 'dtpGroupFromDate', 'dtpGroupToDate', true);
            iTrack_PromotingInteroperability.BindProvider();
            iTrack_PromotingInteroperability.BindGroupProviders();
            //$('#pnliTrackPromotingInteroperability #dtpFromDate').datepicker("setDate", '10/01/2018');
            //$('#pnliTrackPromotingInteroperability #dtpToDate').datepicker("setDate", '12/31/2018');
            //$('#pnliTrackPromotingInteroperability #dtpGroupFromDate').datepicker("setDate", '10/01/2018');
            //$('#pnliTrackPromotingInteroperability #dtpGroupToDate').datepicker("setDate", '12/31/2018');
            $('#pnliTrackPromotingInteroperability #ddlMemberProviders').multiselect({
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
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #IsGroupSearched").addClass("hide");
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #IsProviderSearched").removeClass("hide");
        });

    },

    viewIndividualReporting: function () {
        $("#pnliTrackPromotingInteroperability #IndividualReporting").removeClass('hidden');
        $("#pnliTrackPromotingInteroperability #GroupReporting").addClass('hidden');
        $("#pnliTrackPromotingInteroperability #NpiText").text(' NPI: ');
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #IsGroupSearched").addClass("hide");
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #IsProviderSearched").removeClass("hide");
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability").dataTable().fnClearTable();
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability").dataTable().fnDestroy();
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability tbody").find("tr").remove();
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #diviTrackPromotingInteroperabilityPaging").addClass('hidden');


    },
    viewGroup: function () {
        $("#pnliTrackPromotingInteroperability #IndividualReporting").addClass('hidden');
        $("#pnliTrackPromotingInteroperability #GroupReporting").removeClass('hidden');
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #IsProviderSearched").addClass("hide");
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #IsGroupSearched").removeClass("hide");
        $("#pnliTrackPromotingInteroperability #NpiText").text(' Group TIN: ');
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability").dataTable().fnClearTable();
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability").dataTable().fnDestroy();
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability tbody").find("tr").remove();
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #diviTrackPromotingInteroperabilityPaging").addClass('hidden');


    },

    GroupPreferencesEdit: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS Preference_Group / Virtual Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["GroupId"] = $("#pnliTrackPromotingInteroperability #hfGroupId").val();
                params["mode"] = "Edit";
                params["FromAdmin"] = '0';
                params["ParentCtrl"] = 'iTrack_PromotingInteroperability';
                LoadActionPan('MIPS_AdminPreferenceGroupDetail', params);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    HideGroupNameLink: function (value) {
        if (value == "") {
            $('#pnliTrackPromotingInteroperability #txtGroupName').attr("ProviderId", "-1");
            $('#pnliTrackPromotingInteroperability #hfGroupId').val("-1");
            $("#pnliTrackPromotingInteroperability #lnkGroupEdit").css("display", "none");
            $("#pnliTrackPromotingInteroperability #lblGroupName").css("display", "inline");
        } else {

            $("#pnliTrackPromotingInteroperability #lnkGroupEdit").css("display", "inline");
            $("#pnliTrackPromotingInteroperability #lblGroupName").css("display", "none");
        }
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
                        iTrack_PromotingInteroperability.params.Groups = ProviderLoadJSONData.Groups;
                        iTrack_PromotingInteroperability.params.GroupsDetails = ProviderLoadJSONData.GroupsDetail;
                        iTrack_PromotingInteroperability.params.MembersDetails = ProviderLoadJSONData.GroupDetail;

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
    BindGroupProviders: function () {
        var Ctrl = $("#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability #txtGroupName");
        var func = function () { return iTrack_PromotingInteroperability.GetMIPSGroupProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnliTrackPromotingInteroperability #hfGroupId");
        var onSelect = function (e) {
            iTrack_PromotingInteroperability.setGroupProviderIDs();
            $("#pnliTrackPromotingInteroperability #hfGroupId").val(e.id);
            iTrack_PromotingInteroperability.GetPIDatesByGroupId(e.id).done(function (response) {
                var response = JSON.parse(response);
                if (response.status != false) {
                    response.StartDate ? $('#pnliTrackPromotingInteroperability #dtpGroupFromDate').datepicker("setDate", utility.RemoveTimeFromDate(null, response.StartDate)) : "";
                    response.EndDate ? $('#pnliTrackPromotingInteroperability #dtpGroupToDate').datepicker("setDate", utility.RemoveTimeFromDate(null, response.EndDate)) : "";
                }
            });

        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },

    GetMIPSProvider: function (name) {
        var AllProviders = [];
        var IsValid = false;

        if (name != null && name.length > 1)
            IsValid = true;
        else
            IsValid = false;

        var dfd = new $.Deferred();

        if (IsValid) {
            iTrack_PromotingInteroperability.Search_MIPSProviderPreferences_DBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    responseData = JSON.parse(responseData);
                    if (responseData.IndividualProCount > 0) {
                        var ProviderLoadJSONData = JSON.parse(responseData.IndividualProCountLoad_JSON);
                        iTrack_PromotingInteroperability.params.Groups = ProviderLoadJSONData.Groups;
                        iTrack_PromotingInteroperability.params.GroupsDetails = ProviderLoadJSONData.GroupsDetail;

                        $.each(ProviderLoadJSONData.Groups, function (i, item) {

                            AllProviders.push({ id: item.ProviderId, value: item.ShortName });

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
        var groupId = $("#pnliTrackPromotingInteroperability #hfGroupId").val();
        var groupDetails = $.grep(iTrack_PromotingInteroperability.params.Groups, function (a) {
            return a.GroupId == groupId;
        });

        $.each(groupDetails, function (i, item) {
            item.PerformanceYear = item.PerformanceYear == "" ? "2018" : item.PerformanceYear;
            $("#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability #txtGroupTIN").val(item.TIN);
            $("#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability #lblProviderName").text('');
        });

        var members = "";
        if (iTrack_PromotingInteroperability.params.MembersDetails && iTrack_PromotingInteroperability.params.MembersDetails.length > 0) {
            $.each(iTrack_PromotingInteroperability.params.MembersDetails, function (i, item) {

                members += "," + item.ProviderId;
                $('#pnliTrackPromotingInteroperability #ddlMemberProviders').append($('<option>', {
                    refvalue: item.ProviderName,
                    value: item.ProviderId,
                    text: item.ProviderName
                }));
            });
            $('#pnliTrackPromotingInteroperability #ddlMemberProviders').val(members.split(','));
            $('#pnliTrackPromotingInteroperability #ddlMemberProviders').multiselect("refresh");
            $('#pnliTrackPromotingInteroperability #ddlMemberProviders').multiselect('rebuild');

        }
    },

    OpenProviderDetail: function () {

        var params = [];
        params["ProviderId"] = $('#' + iTrack_PromotingInteroperability.params.PanelID + ' #hfProviderId').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = 'txtProvider';
        params["ParentCtrl"] = 'iTrack_PromotingInteroperability';
        LoadActionPan('providerDetail', params);
    },
    HideProviderLink: function (value) {
        if (value == "") {
            $('#pnliTrackPromotingInteroperability #txtProvider').attr("ProviderId", "-1");
            $('#pnliTrackPromotingInteroperability #hfProviderId').val("-1");
            $("#pnliTrackPromotingInteroperability #lnkProviderEdit").css("display", "none");
            $("#pnliTrackPromotingInteroperability #lblProvider").css("display", "inline");
        } else {

            $("#pnliTrackPromotingInteroperability #lnkProviderEdit").css("display", "inline");
            $("#pnliTrackPromotingInteroperability #lblProvider").css("display", "none");
        }
    },
    printPreview: function () {
        var providerData = "";
        if (iTrack_PromotingInteroperability.params.ProviderData != 'undefined' && iTrack_PromotingInteroperability.params.ProviderData != null)
            providerData = iTrack_PromotingInteroperability.params.ProviderData.split('<br/>');
        var newProviderText = '';
        for (var i = 0; i < providerData.length; i++) {
            if ($.trim(providerData[i]) != '') {
                newProviderText += '<li class="text-left">' + providerData[i] + '</li>';
            }
        }
        $('#' + iTrack_PromotingInteroperability.params.PanelID + " #printcall #ProviderList").html(newProviderText);
        $('#' + iTrack_PromotingInteroperability.params.PanelID + " #printcall #ulContent #MeasureName").append(iTrack_PromotingInteroperability.params.Measure);
        var practiceData = "";
        if (iTrack_PromotingInteroperability.params.PracticeData != 'undefined' && iTrack_PromotingInteroperability.params.PracticeData != null)
            practiceData = iTrack_PromotingInteroperability.params.PracticeData.split('<br/>');
        var newPracticeText = '';
        for (var i = 0; i < practiceData.length; i++) {
            if ($.trim(practiceData[i]) != '') {
                newPracticeText += '<li class="text-right">' + practiceData[i] + '</li>';
            }
        }
        $('#' + iTrack_PromotingInteroperability.params.PanelID + " #printcall #PracticeList").html(newPracticeText);


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
        $('#' + iTrack_PromotingInteroperability.params.PanelID + " #printcall #liCurrentDate").text(datetime);
        params["UlContent"] = $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability")[0].outerHTML;
        $('#' + iTrack_PromotingInteroperability.params.PanelID + " #printcall #ulContent").append(params["UlContent"]);
        iTrack_PromotingInteroperability.PrintReports();
    },
    PrintReports: function () {
        $('#' + iTrack_PromotingInteroperability.params.PanelID + " #printcall").removeClass('hidden');
        kendo.drawing.drawDOM('#' + iTrack_PromotingInteroperability.params["PanelID"] + " #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "7mm",
                right: "10mm",
                bottom: "15mm"
            },
            template: kendo.template($('#' + iTrack_PromotingInteroperability.params["PanelID"] + " #page-templateLegacy").html())
        }).then(function (group) {

            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                params["PrintPDFDataURL"] = PrintPDFDataURL;
                params["PreviewPdf"] = true;
                utility.PDFViewer(params["PrintPDFDataURL"], true, null, null, true);
                $('#' + iTrack_PromotingInteroperability.params.PanelID + " #printcall").addClass('hidden');
                $('#' + iTrack_PromotingInteroperability.params.PanelID + " #printcall #ulContent").html("");
            });

        });
    },
    QualityMeasureSearch: function (cqmid, pageNo, rpp) {
        if ($('#pnliTrackPromotingInteroperability #txtProvider').val() == "") {
            utility.DisplayMessages('Please Select Provider', 2);
            return;
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS_MPromoting Interoperability (Formally ACI)", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #lblProviderName").text($("#" + iTrack_PromotingInteroperability.params.PanelID + " #txtProvider").val());
                $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #lblNPI").text($("#pnliTrackPromotingInteroperability #NPItxt").val());
                var year = "";
                if ($("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #ddlPerformanceYear").val() == 1) {
                    year = '90 Days | 10/01/2017 - 12/31/2017';
                } else if ($("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #ddlPerformanceYear").val() == 2) {
                    year = '90 Days | 10/01/2018 - 12/31/2018';
                }
                $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #lblPerformanceyear").text(year);

                if ($("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result").css("display") === "none")
                    $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result").show();

                var self = $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnlPromotingInteroperabilityMeasure_Search");
                var myJsonByName = self.getMyJSONByName();
                var objData = JSON.parse(myJsonByName);



                objData["ProviderId"] = $('#' + iTrack_PromotingInteroperability.params.PanelID + ' #hfProviderId').val();
                objData["PatientId"] = $('#' + iTrack_PromotingInteroperability.params.PanelID + ' #hfPatientId').val();
                objData["Problems"] = [];
                var myJson = JSON.stringify(objData);

                if ($("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #hfProviderId ").val() == "" || $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dtpDateFrom").val() == "" || $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dtpDateTo").val() == "") {
                    utility.DisplayMessages("Specify a Criteria to Filter", 3);
                } else {
                    iTrack_PromotingInteroperability.SearchQualityMeasure(myJson, cqmid, pageNo, rpp).done(function (response) {

                        if (response.status != false) {
                            iTrack_PromotingInteroperability.params.ProviderData = response.ProviderText;
                            iTrack_PromotingInteroperability.params.PracticeData = response.PracticeText;
                            iTrack_PromotingInteroperability.QualityMeasureGridLoad(response);

                            var txt = ("90 Days | ") + $('#' + iTrack_PromotingInteroperability.params.PanelID + " #dtpFromDate").val() + " - " + $('#' + iTrack_PromotingInteroperability.params.PanelID + " #dtpToDate").val();
                            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #lblPerformanceyear").text(txt);
                            var tableControl = iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability";
                            var pagingPanelControlId = iTrack_PromotingInteroperability.params["PanelID"] + " #diviTrackPromotingInteroperabilityPaging";
                            var classControlName = "iTrack_PromotingInteroperability";
                            var pagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout
                            (
                                CreatePagination(response.BatchClinicalQualityMeasureCount, pageNo, 30, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords,
                                    function (primaryId, pageNumber, resultPerPage) {
                                        iTrack_PromotingInteroperability.QualityMeasureSearch(primaryId, pageNumber, 30);
                                    }
                                ), 10);
                        } else {
                            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability").dataTable().fnDestroy();
                            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability #dgviTrackPromotingInteroperability tbody").find("tr").remove();
                            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #diviTrackPromotingInteroperabilityPaging").remove();

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
        if ($('#pnliTrackPromotingInteroperability #txtGroupName').val() == "") {
            utility.DisplayMessages('Please Select Group', 2);
            return;
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS_MPromoting Interoperability (Formally ACI)", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #lblProviderName").text($("#" + iTrack_PromotingInteroperability.params.PanelID + " #txtGroupName").val());
                $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #lblNPI").text($("#" + iTrack_PromotingInteroperability.params.PanelID + " #txtGroupTIN").val());
                var year = "";
                if ($("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #ddlPerformanceYearGroup").val() == 1) {
                    year = '90 Days | 10/01/2017 - 12/31/2017';
                } else if ($("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #ddlPerformanceYearGroup").val() == 2) {
                    year = '90 Days | 10/01/2018 - 12/31/2018';
                }
                $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #lblPerformanceyear").text(year);

                if ($("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result").css("display") === "none")
                    $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result").show();

                var self = $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnlPromotingInteroperabilityMeasure_Search");
                var myJsonByName = self.getMyJSONByName();
                var objData = JSON.parse(myJsonByName);

                objData["DateFrom"] = $('#' + iTrack_PromotingInteroperability.params.PanelID + ' #dtpGroupFromDate').val();
                objData["DateTo"] = $('#' + iTrack_PromotingInteroperability.params.PanelID + ' #dtpGroupToDate').val();
                objData["GroupId"] = $('#' + iTrack_PromotingInteroperability.params.PanelID + ' #hfGroupId').val();
                objData["PatientId"] = $('#' + iTrack_PromotingInteroperability.params.PanelID + ' #hfPatientId').val();
                objData["Problems"] = [];
                var myJson = JSON.stringify(objData);
                iTrack_PromotingInteroperability.SearchQualityMeasureGroup(myJson, cqmid, pageNo, rpp).done(function (response) {

                    if (response.status != false) {
                        iTrack_PromotingInteroperability.params.ProviderData = response.ProviderText;
                        iTrack_PromotingInteroperability.params.PracticeData = response.PracticeText;

                        iTrack_PromotingInteroperability.QualityMeasureGridLoadGroup(response);

                        var txt = ("90 Days | ") + $('#' + iTrack_PromotingInteroperability.params.PanelID + " #dtpGroupFromDate").val() + " - " + $('#' + iTrack_PromotingInteroperability.params.PanelID + " #dtpGroupToDate").val();
                        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #lblPerformanceyear").text(txt);

                        var tableControl = iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability";
                        var pagingPanelControlId = iTrack_PromotingInteroperability.params["PanelID"] + " #diviTrackPromotingInteroperabilityPaging";
                        var classControlName = "iTrack_PromotingInteroperability";
                        var pagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout
                        (
                            CreatePagination(response.BatchClinicalQualityMeasureCount, pageNo, 30, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords,
                                function (primaryId, pageNumber, resultPerPage) {
                                    iTrack_PromotingInteroperability.QualityMeasureSearchGroup(primaryId, pageNumber, 30);
                                }
                            ), 10);
                    } else {
                        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability").dataTable().fnDestroy();
                        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability #dgviTrackPromotingInteroperability tbody").find("tr").remove();
                        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #diviTrackPromotingInteroperabilityPaging").remove();

                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    applySort: function (key, data) {
        return data.sort(function (a, b) {
            var x = parseInt(a[key]);
            var y = parseInt(b[key]);
            return ((x < y) ? -1 : ((x > y) ? 1 : 0));
        });
    },
    QualityMeasureGridLoad: function (response) {
        iTrack_PromotingInteroperability.MUdata_Patient = JSON.parse(response.MU_JSON_PatientWise);
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability").dataTable().fnClearTable();
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability").dataTable().fnDestroy();
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability tbody").find("tr").remove();
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #diviTrackPromotingInteroperabilityPaging").removeClass('hidden');
        var baseMeasures = '<tr><td colspan="8" id="cehrt1"><div class="panel-heading"> <span class="pull-left"> Base Measures </span><div class="clearfix"></div></div></td></tr>';
        var PerformanceMeasures = '<tr><td colspan="7" id="cehrt1"><div class="panel-heading"> <span class="pull-left"> Performance Measures </span><div class="clearfix"></div></div></td></tr>';
        var BonusMeasures = '<tr><td colspan="7" id="cehrt1"><div class="panel-heading"> <span class="pull-left"> Bonus Measures </span><div class="clearfix"></div></div></td></tr>';
        var CEHRT1 = '<td colspan="2"><div class="checkbox-custom checkbox-default" id="dvchkCEHRT1"><input disabled = "disabled" checked="checked" id="chkCEHRT1" type="checkbox" value="False"/><label class="bold" for="chkCEHRT1">Mark as Complete</label></div></td><td style="display:none"></td>';
        var infoIcon = '<a data-toggle="tooltip" title="" data-original-title="In the 2018 performance period, clinicians and groups that exclusively report the Promoting Interoperability Objectives and Measures will earn a 10% bonus for using only 2015 Edition CEHRT." onclick="" href="javascript:void(0)" style=" margin-left: 10px;"><i class="fa fa-info btn btn-primary btn-xs" style="border-radius: 0px;"></i></a>';
        var chrt1row = "<tr><td style='display:none;'></td><td style='display:none;'></td><td colspan='2' id='cehrt1'><label class='control-label'>Using CEHRT Certified to the 2015 edition  " + infoIcon + "</label></td>" + CEHRT1 + "<td><div class='progress mb-xxs'><div class='white progress-bar progress-bar-success' role='progressbar' aria-valuenow='aria-valuemin='0' aria-valuemax='100' style='width:100%' data-toggle='tooltip'>10%</div></div></td><td><label class='label-chip size40'>10%</label></td><td><label class='label-chip size85'>N/A</label></td><td id='MeasureType'style='display:none;'>Bonus Measures</td></tr>";
        var CEHRT2 = '<div class="panel-heading"> <span class="pull-left"> Bonus Measures </span></div>';
        var count = 0;
        var arraTemp = [];
        if (response.MURecordCount > 0) {
            var QualityMeasureLoadJson = JSON.parse(response.MU_JSON);
            QualityMeasureLoadJson = iTrack_PromotingInteroperability.applySort('MeasureOrder', QualityMeasureLoadJson);
            var Score = 10;
            iTrack_PromotingInteroperability.graphObj = [];
            $.each(QualityMeasureLoadJson, function (i, item) {
                if (item.ID != "") {
                    var SelectionCheckBoxColumn = "";
                    var strNonCompliance = "";
                    var checkBoxChecked = "";
                    var strTarget = "<td id='Target'>" + item.RequiredTarget + "%</td>";
                    //var wdth = parseInt(Math.round(((item.Numerator ? item.Numerator : 0) / (item.Denominator ? item.Denominator : 0)) * 100)) || 0;
                    if (item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_IACEHRT_1") {
                        item.PerfromanceRate1 = "100";
                        wdth = "100";
                    }
                    var barClass = 'success';
                    if (!item.PerfromanceRate1)
                        item.PerfromanceRate1 = "0.00";
                    if (!(item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_IACEHRT_1")) {
                        if (parseInt(item.PerfromanceRate1) <= 0) {
                            barClass = 'danger';
                            wdth = "100";
                        }
                        else if (parseInt(item.PerfromanceRate1) > 0 && parseInt(item.PerfromanceRate1) < item.RequiredTarget) {
                            barClass = 'info';
                            wdth = parseInt(item.PerfromanceRate1);
                        } else if (parseInt(item.PerfromanceRate1) >= item.RequiredTarget) {
                            barClass = 'success';
                            wdth = "100";
                        }
                    }
                    var performanceScoreWeigthage = "10%";
                    if (item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_EP_1")
                        performanceScoreWeigthage = "N/A";
                    if (item.MeasureId == "PI_PHCDRR_4" || item.MeasureId == "PI_PHCDRR_3" || item.MeasureId == "PI_PHCDRR_5" || item.MeasureId == "PI_PHCDRR_2") {
                        performanceScoreWeigthage = "5%";
                    }
                    var performanceRate = parseInt(item.PerfromanceRate1) == 0 ? "0.00" : parseInt(item.PerfromanceRate1);
                    var calculatedPerformance = parseFloat(performanceRate / 10) || 0;
                    var trgt = parseInt(item.RequiredTarget);
                    if (parseInt(item.PerfromanceRate1) >= trgt && item.MeasureType != "Bonus") {
                        calculatedPerformance = 10;
                        item.PerfromanceRate1 = "100";
                    }
                    Score = Score + calculatedPerformance;
                    var performancebaar = '<div class="progress mb-xxs"><div class="white progress-bar progress-bar-' + barClass + '" role="progressbar" aria-valuenow="' + performanceRate + ' "aria-valuemin="0" aria-valuemax="100" style="width:' + wdth + '%" "> ' + calculatedPerformance + '%</div></div>';
                    var icon = '<a  href="javacript:void(0);" class="on-editing expand-row font-xs" title="Expand/Collapse Record" onclick="iTrack_PromotingInteroperability.stopProp(this,event);"><i class="fa fa-caret-right"></i></a>';
                    if (item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_IACEHRT_1")
                        checkBoxChecked = "checked='checked'";
                    var disableCheckBox = '';
                    if (item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_IACEHRT_1")
                        disableCheckBox = 'disabled = "disabled"';
                    SelectionCheckBoxColumn = '<td colspan="2"><div class="checkbox-custom checkbox-default"><input ' + disableCheckBox + ' type="checkbox"  id="chk_' + item.ID + '" name="SelectCheckBoxOrder_' + item.ID + ' class="input-block" ' + checkBoxChecked + '/><label class="bold" for="chk_' + item.ID + '">Mark Complete</label></div></td>';

                    var strNumenatorDenominator = "<td id='Numerator'>" + (item.Numerator ? item.Numerator : 0) + '/' + (item.Denominator ? item.Denominator : 0) + "</td>";
                    var $row = $("<tr/>");
                    $row.attr("id", "gvBatchClinicalQualityMeasure_row" + item.ID);
                    $row.attr("onclick", "iTrack_PromotingInteroperability.QualityMeasureDetail(" + item.ID + ",'" + item.MeasureName + "','Compliance',event)");
                    $row.attr("class", "Parent");
                    $row.attr("ID", item.ID);
                    var PatientCount = 0;
                    var ListPatient = [];
                    var patients = JSON.parse(response.MU_JSON_PatientWise);
                    $.each(patients, function (i, dataItem) {
                        if (dataItem.ID == item.ID && dataItem.Numerator == "0") {
                            ListPatient.push(dataItem);
                        }
                    });
                    PatientCount = ListPatient.length;
                    if (item.MeasureType == "Bonus" || item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_PHCDRR_4") {
                        strNonCompliance = "<td id='NonCompliance'><label class='label-chip size85'>N/A</label></td>";
                        strNumenatorDenominator = SelectionCheckBoxColumn;
                        strTarget = "<td style='display:none'></td>";
                    }
                    else
                        strNonCompliance = "<td id='NonCompliance'><a class='btn-default pl-xs pr-xs btn btn-xs size85 active' onclick=\"iTrack_PromotingInteroperability.QualityMeasureDetail(" + item.ID + ",'" + item.MeasureName + "','NonCompliance',event);\">Patient(" + PatientCount + ")</a></td>";
                    Measures = {
                        'name': item.MeasureId,
                        'MeasureName': item.MeasureName,
                        'PerformanceRate': parseFloat(item.PerfromanceRate1 / 10) || 0,
                        'MeasureType': item.MeasureType
                    };
                    if (item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_EP_1" || item.MeasureId == "PI_PEA_1" || item.MeasureId == "PI_HIE_2" || item.MeasureId == "PI_HIE_1" || item.MeasureId == "PI_HIE_3" || item.MeasureId == "PI_PEA_2" || item.MeasureId == "PI_CCTPE_2" || item.MeasureId == "PI_CCTPE_1" || item.MeasureId == "PI_CCTPE_3" || item.MeasureId == "PI_IACEHRT_1")
                        iTrack_PromotingInteroperability.graphObj.push(Measures);
                    $row.append("<td style=\"display:none;\">" + item.ID + "</td><td id='MeasureNumber'>" + item.MeasureId + "</td><td id='MeasureDescription'>" + icon + " " + item.MeasureName + "</td>" + strNumenatorDenominator + strTarget + "<td id='PerformanceRate'>" + performancebaar + "</td><td id='PerformancePercentage'><label class='label-chip size40'>" + performanceScoreWeigthage + "</label></td>" + strNonCompliance + "<td id='MeasureType'style='display:none;'>" + item.MeasureType + " Measures" + "</td>");

                    $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability tbody").last().append($row);

                    count++;
                    var CurrentRowchilds = $();
                    var childRow = '<div class="col-xs-12 p-none pt-xs pb-xs"><table class="table-va-top table-space-td" width="100%"><tr class="text-uppercase"><th class="pb-default size30"></th> <th class="pb-default size200">MEASURE DESCRIPTION</th><th class="pb-default size30">Objective Name</th></tr><tbody><tr><td></td><td><p>' + item.MeasureDescription + '<a href="https://qpp.cms.gov/mips/overview" target="_blank">LearnMore <i class="fa fa-external-link"></i></a></p></td><td><ul class="list-unstyled"><li><li id="ObjectiveName">' + item.ObjectiveName + '</li></ul></td></tr></tbody></table></div>';

                    CurrentRowchilds = CurrentRowchilds.add(childRow);
                    arraTemp.push({ row: $row, childs: CurrentRowchilds });

                }
                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            });
            Score = Math.round(Score);
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #divScore").attr("aria-valuenow", Score);
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #divScore").css("width", Score + "%");
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #SpanScoreNow").text(Score);


            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability tbody").last().append(chrt1row);
            Measures = {
                'name': "Using 2015 Certified CEHRT",
                'MeasureName': "Using 2015 Certified CEHRT",
                'PerformanceRate': "10",
                'MeasureType': 'Bonus'
            };
            iTrack_PromotingInteroperability.graphObj.push(Measures);
        }
        else {
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability").DataTable({
                "language": {
                    "emptyTable": "No Measure Found"
                },
                "autoWidth": false,
                "bLengthChange": false,

                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true,

            });

        }
        if ($.fn.dataTable.isDataTable("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability"));
        else {
            var group = '';
            iTrack_PromotingInteroperability.measureTable = $('#' + iTrack_PromotingInteroperability.params.PanelID + ' #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability').DataTable({
                "bInfo": false, "searching": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "sorting": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false }, { "sClass": "text-center", "aTargets": [6, 7] }], "drawCallback": function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                    api.column(8, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                            '<tr class="group"><td colspan="8" style="border-radius: 0;background: #468aea;color: #FFFFFF;">' + group + '</td></tr>'
                            );

                            last = group;
                        }
                    });
                }
            }
                ); // to remove records per page dropdown

            $.each(arraTemp, function (i, item) {

                if (iTrack_PromotingInteroperability.measureTable != null) {
                    var row = iTrack_PromotingInteroperability.measureTable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }

                }
            });
            $('#' + iTrack_PromotingInteroperability.params.PanelID + ' #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability').off()
           .on('click', 'a.expand-row', function (e) {
               e.preventDefault();

               iTrack_PromotingInteroperability.rowExpand($(this).closest('tr'));
           });
        }

        var table = $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability").DataTable();

        // Sort by column 1 and then re-draw
        //table
        //.order([1, 'asc'])
        //.draw();
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({
            container: 'body'
        });
    },

    QualityMeasureGridLoadGroup: function (response) {
        iTrack_PromotingInteroperability.MUdata_Patient = JSON.parse(response.MU_JSON_PatientWise);
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability").dataTable().fnDestroy();
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability tbody").find("tr").remove();
        $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #diviTrackPromotingInteroperabilityPaging").removeClass('hidden');
        var baseMeasures = '<tr><td colspan="8" id="cehrt1"><div class="panel-heading"> <span class="pull-left"> Base Measures </span><div class="clearfix"></div></div></td></tr>';
        var PerformanceMeasures = '<tr><td colspan="7" id="cehrt1"><div class="panel-heading"> <span class="pull-left"> Performance Measures </span><div class="clearfix"></div></div></td></tr>';
        var BonusMeasures = '<tr><td colspan="7" id="cehrt1"><div class="panel-heading"> <span class="pull-left"> Bonus Measures </span><div class="clearfix"></div></div></td></tr>';
        var CEHRT1 = '<td colspan="2"><div class="checkbox-custom checkbox-default" id="dvchkCEHRT1"><input disabled = "disabled" checked="checked" id="chkCEHRT1" type="checkbox" value="False"/><label class="bold" for="chkCEHRT1">Mark as Complete</label></div></td><td style="display:none"></td>';
        var infoIcon = '<a data-toggle="tooltip" title="In the 2018 performance period, clinicians and groups that exclusively report the Promoting Interoperability Objectives and Measures will earn a 10% bonus for using only 2015 Edition CEHRT." data-original-title="In the 2018 performance period, clinicians and groups that exclusively report the Promoting Interoperability Objectives and Measures will earn a 10% bonus for using only 2015 Edition CEHRT." onclick="" href="javascript:void(0)" style=" margin-left: 10px;"><i class="fa fa-info btn btn-primary btn-xs" style="border-radius: 0px;"></i></a>';
        var chrt1row = "<tr><td style='display:none;'></td><td style='display:none;'></td><td colspan='2' id='cehrt1'><label class='control-label'>Using CEHRT Certified to the 2015 edition  " + infoIcon + "</label></td>" + CEHRT1 + "<td><div class='progress mb-xxs'><div class='white progress-bar progress-bar-success' role='progressbar' aria-valuenow='aria-valuemin='0' aria-valuemax='100' style='width:100%' data-toggle='tooltip'>10%</div></div></td><td><label class='label-chip size40'>10%</label></td><td><label class='label-chip size85'>N/A</label></td><td id='MeasureType'style='display:none;'>Bonus Measures</td></tr>";
        var CEHRT2 = '<div class="panel-heading"> <span class="pull-left"> Bonus Measures </span></div>';
        var count = 0;
        var arraTemp = [];
        if (response.MURecordCount > 0) {
            var QualityMeasureLoadJson = JSON.parse(response.MU_JSON);
            QualityMeasureLoadJson = iTrack_PromotingInteroperability.applySort('MeasureOrder', QualityMeasureLoadJson);
            var Score = 0;
            iTrack_PromotingInteroperability.graphObj = [];
            $.each(QualityMeasureLoadJson, function (i, item) {
                if (item.ID != "") {
                    var SelectionCheckBoxColumn = "";
                    var strNonCompliance = "";
                    var checkBoxChecked = "";
                    var strTarget = "<td id='Target'>" + item.RequiredTarget + "%</td>";
                    //var wdth = parseInt(Math.round(((item.Numerator ? item.Numerator : 0) / (item.Denominator ? item.Denominator : 0)) * 100)) || 0;
                    if (item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_IACEHRT_1") {
                        item.PerfromanceRate1 = "100";
                        wdth = "100";
                    }
                    var barClass = 'success';
                    if (!item.PerfromanceRate1)
                        item.PerfromanceRate1 = "0.00";
                    if (!(item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_IACEHRT_1")) {
                        if (parseInt(item.PerfromanceRate1) <= 0) {
                            barClass = 'danger';
                            wdth = "100";
                        }
                        else if (parseInt(item.PerfromanceRate1) > 0 && parseInt(item.PerfromanceRate1) < item.RequiredTarget) {
                            barClass = 'info';
                            wdth = parseInt(item.PerfromanceRate1);
                        } else if (parseInt(item.PerfromanceRate1) >= item.RequiredTarget) {
                            barClass = 'success';
                            wdth = "100";
                        }
                    }
                    var performanceScoreWeigthage = "10%";
                    if (item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_EP_1")
                        performanceScoreWeigthage = "N/A";
                    if (item.MeasureId == "PI_PHCDRR_4" || item.MeasureId == "PI_PHCDRR_3" || item.MeasureId == "PI_PHCDRR_5" || item.MeasureId == "PI_PHCDRR_2") {
                        performanceScoreWeigthage = "5%";
                    }
                    var performanceRate = parseInt(item.PerfromanceRate1) == 0 ? "0.00" : parseInt(item.PerfromanceRate1);
                    var calculatedPerformance = parseFloat(performanceRate / 10) || 0;
                    var trgt = parseInt(item.RequiredTarget);
                    if (parseInt(item.PerfromanceRate1) >= trgt && item.MeasureType != "Bonus") {
                        calculatedPerformance = 10;
                        item.PerfromanceRate1 = "100";
                    }
                    Score = Score + calculatedPerformance;
                    Score = Math.round(Score);
                    var performancebaar = '<div class="progress mb-xxs"><div class="white progress-bar progress-bar-' + barClass + '" role="progressbar" aria-valuenow="' + performanceRate + ' "aria-valuemin="0" aria-valuemax="100" style="width:' + wdth + '%" "> ' + calculatedPerformance + '%</div></div>';
                    var icon = '<a  href="javacript:void(0);" class="on-editing expand-row font-xs" title="Expand/Collapse Record" onclick="iTrack_PromotingInteroperability.stopProp(this,event);"><i class="fa fa-caret-right"></i></a>';
                    if (item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_IACEHRT_1")
                        checkBoxChecked = "checked='checked'";
                    var disableCheckBox = '';
                    if (item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_IACEHRT_1")
                        disableCheckBox = 'disabled = "disabled"';
                    SelectionCheckBoxColumn = '<td colspan="2"><div class="checkbox-custom checkbox-default"><input ' + disableCheckBox + ' type="checkbox"  id="chk_' + item.ID + '" name="SelectCheckBoxOrder_' + item.ID + ' class="input-block" ' + checkBoxChecked + '/><label class="bold" for="chk_' + item.ID + '">Mark Complete</label></div></td>';

                    var strNumenatorDenominator = "<td id='Numerator'>" + (item.Numerator ? item.Numerator : 0) + '/' + (item.Denominator ? item.Denominator : 0) + "</td>";
                    var $row = $("<tr/>");
                    $row.attr("id", "gvBatchClinicalQualityMeasure_row" + item.ID);
                    $row.attr("onclick", "iTrack_PromotingInteroperability.QualityMeasureDetail(" + item.ID + ",'" + item.MeasureName + "','Compliance',event)");
                    $row.attr("class", "Parent");
                    $row.attr("ID", item.ID);
                    var PatientCount = 0;
                    var ListPatient = [];
                    var patients = JSON.parse(response.MU_JSON_PatientWise);
                    $.each(patients, function (i, dataItem) {
                        if (dataItem.ID == item.ID && dataItem.Numerator == "0") {
                            ListPatient.push(dataItem);
                        }
                    });
                    PatientCount = ListPatient.length;
                    if (item.MeasureType == "Bonus" || item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_PHCDRR_4") {
                        strNonCompliance = "<td id='NonCompliance'><label class='label-chip size85'>N/A</label></td>";
                        strNumenatorDenominator = SelectionCheckBoxColumn;
                        strTarget = "<td style='display:none'></td>";
                    }
                    else
                        strNonCompliance = "<td id='NonCompliance'><a class='btn-default pl-xs pr-xs btn btn-xs size85 active' onclick=\"iTrack_PromotingInteroperability.QualityMeasureDetail(" + item.ID + ",'" + item.MeasureName + "','NonCompliance',event);\">Patient(" + PatientCount + ")</a></td>";
                    Measures = {
                        'name': item.MeasureId,
                        'MeasureName': item.MeasureName,
                        'PerformanceRate': parseFloat(item.PerfromanceRate1 / 10) || 0,
                        'MeasureType': item.MeasureType
                    };
                    if (item.MeasureId == "PI_PPHI_1" || item.MeasureId == "PI_EP_1" || item.MeasureId == "PI_PEA_1" || item.MeasureId == "PI_HIE_2" || item.MeasureId == "PI_HIE_1" || item.MeasureId == "PI_HIE_3" || item.MeasureId == "PI_PEA_2" || item.MeasureId == "PI_CCTPE_2" || item.MeasureId == "PI_CCTPE_1" || item.MeasureId == "PI_CCTPE_3" || item.MeasureId == "PI_IACEHRT_1")
                        iTrack_PromotingInteroperability.graphObj.push(Measures);
                    $row.append("<td style=\"display:none;\">" + item.ID + "</td><td id='MeasureNumber'>" + item.MeasureId + "</td><td id='MeasureDescription'>" + icon + " " + item.MeasureName + "</td>" + strNumenatorDenominator + strTarget + "<td id='PerformanceRate'>" + performancebaar + "</td><td id='PerformancePercentage'><label class='label-chip size40'>" + performanceScoreWeigthage + "</label></td>" + strNonCompliance + "<td id='MeasureType'style='display:none;'>" + item.MeasureType + " Measures" + "</td>");

                    $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability tbody").last().append($row);

                    count++;
                    var CurrentRowchilds = $();
                    var childRow = '<div class="col-xs-12 p-none pt-xs pb-xs"><table class="table-va-top table-space-td" width="100%"><tr class="text-uppercase"><th class="pb-default size30"></th> <th class="pb-default size200">MEASURE DESCRIPTION</th><th class="pb-default size30">Objective Name</th></tr><tbody><tr><td></td><td><p>' + item.MeasureDescription + '<a href="https://qpp.cms.gov/mips/overview" target="_blank">LearnMore <i class="fa fa-external-link"></i></a></p></td><td><ul class="list-unstyled"><li><li id="ObjectiveName">' + item.ObjectiveName + '</li></ul></td></tr></tbody></table></div>';

                    CurrentRowchilds = CurrentRowchilds.add(childRow);
                    arraTemp.push({ row: $row, childs: CurrentRowchilds });

                }
                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            });
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #divScore").attr("aria-valuenow", Score);
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #divScore").css("width", Score + "%");
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #SpanScoreNow").text(Score);


            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability tbody").last().append(chrt1row);
            Measures = {
                'name': "Using 2015 Certified CEHRT",
                'MeasureName': "Using 2015 Certified CEHRT",
                'PerformanceRate': "10",
                'MeasureType': 'Bonus'
            };
            iTrack_PromotingInteroperability.graphObj.push(Measures);
        }
        else {
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability").DataTable({
                "language": {
                    "emptyTable": "No Measure Found"
                },
                "autoWidth": false,
                "bLengthChange": false,

                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true,

            });

        }
        if ($.fn.dataTable.isDataTable("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #dgviTrackPromotingInteroperability"));
        else {
            var group = '';
            iTrack_PromotingInteroperability.measureTable = $('#' + iTrack_PromotingInteroperability.params.PanelID + ' #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability').DataTable({
                "bInfo": false, "searching": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "sorting": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false }, { "sClass": "text-center", "aTargets": [6, 7] }], "drawCallback": function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                    api.column(8, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                            '<tr class="group"><td colspan="8" style="border-radius: 0;background: #468aea;color: #FFFFFF;">' + group + '</td></tr>'
                            );

                            last = group;
                        }
                    });
                }
            }
                ); // to remove records per page dropdown

            $.each(arraTemp, function (i, item) {

                if (iTrack_PromotingInteroperability.measureTable != null) {
                    var row = iTrack_PromotingInteroperability.measureTable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }

                }
            });
            $('#' + iTrack_PromotingInteroperability.params.PanelID + ' #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability').off()
           .on('click', 'a.expand-row', function (e) {
               e.preventDefault();

               iTrack_PromotingInteroperability.rowExpand($(this).closest('tr'));
           });
        }

        var table = $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability").DataTable();

        // Sort by column 1 and then re-draw
        //table
        //.order([1, 'asc'])
        //.draw();
    },
    stopProp: function (obj, event) {

        event.stopPropagation();
        iTrack_PromotingInteroperability.rowExpand($(obj).closest('tr'));

    },
    QualityMeasureDetail: function (ID, Measure, isCompliance, event) {
        if (event != null)
            event.stopPropagation();

        var params = [];
        params["ParentCtrl"] = "iTrack_PromotingInteroperability";
        params["FromAdmin"] = 0;
        params["ID"] = ID;
        params["Measure"] = Measure;
        params["IsCompliance"] = isCompliance;
        params["resultSet"] = iTrack_PromotingInteroperability.MUdata_Patient;
        params["ProviderText"] = iTrack_PromotingInteroperability.params.ProviderData;
        params["PracticeText"] = iTrack_PromotingInteroperability.params.PracticeData;
        LoadActionPan("iTrack_PromotingInteroperabilityDetail", params);
    },
    ExportData: function (e) {

        var JSONData = [];
        $("#" + iTrack_PromotingInteroperability.params.PanelID + " #dgviTrackPromotingInteroperability tbody tr.Parent").each(function () {

            var obj = {
                MeasureNumber: $(this).find("#MeasureNumber").text().trim(),
                MeasureDescription: $(this).find("#MeasureDescription").text().trim(),
                Numerator: $(this).find("#Numerator").text().trim().split('/')[0],
                Denominator: $(this).find("#Numerator").text().trim().split('/')[1],
                Target: $(this).find("#Target").text().trim(),
                PerformanceRate: $(this).find("#PerformanceRate").text().trim(),
                PerformancePercentage: $(this).find("#PerformancePercentage").text().trim(),
                NonCompliance: $(this).find("#NonCompliance").text().trim(),

            }
            JSONData.push(obj);

        });
        iTrack_PromotingInteroperability.ExportDataToExcel(JSONData);
    },
    ExportDataToExcel: function (JSONData) {

        var ReportTitle = "Promoting Interoperability";
        var ShowLabel = true;
        var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

        var CSV = '';
        CSV += ReportTitle + '\r\n\n';
        if (ShowLabel) {
            var row = "";
            for (var index in arrData[0]) {
                if (index == "MeasureNumber") {
                    index = "Measure Number";
                } else if (index == "MeasureDescription") {
                    index = "Measure Description";
                } else if (index == "PerformanceRate") {
                    index = "Performance Rate";
                } else if (index == "PerformancePercentage") {
                    index = "Performance Percentage"
                } else if (index == "NonCompliance") {
                    index = "Non Compliance";
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

        var row = iTrack_PromotingInteroperability.measureTable.row($row);
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
        if ($("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #btnExpandAll i").hasClass('fa-caret-right')) {
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability tbody tr").each(function () {
                var tr = $(this);
                tr.find('.fa-caret-right').addClass('fa-caret-down').removeClass('fa-caret-right');
                var row = iTrack_PromotingInteroperability.measureTable.row(tr);
                row.child.show();
            });
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #btnExpandAll i").removeClass('fa-caret-right').addClass('fa-caret-down');
        } else {
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #dgviTrackPromotingInteroperability tbody tr").each(function () {
                var tr = $(this);
                tr.find('.fa-caret-down').addClass('fa-caret-right').removeClass('fa-caret-down');
                var row = iTrack_PromotingInteroperability.measureTable.row(tr);
                row.child.hide();
            });
            $("#" + iTrack_PromotingInteroperability.params["PanelID"] + " #pnliTrackPromotingInteroperability_Result #btnExpandAll i").addClass('fa-caret-right').removeClass('fa-caret-down');
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

        var data = "BatchClinicalQualityMeasureData=" + QualityMeasureData + "&CQMID=" + cqmid + "&PageNumber=" + pageNumber + "&RowsPerPage=" + rowsPerPage;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "SEARCH_ACI_MEASURES");
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
        if (a.AgeCondition_text && a.AgeCondition_text == "- Select -") {
            a.AgeCondition_text = "";
        }
        if (a.Sex_tex && a.Sex_text == "- Select -") {
            a.Sex_text = "";
        }
        QualityMeasureData = JSON.stringify(a);

        var data = "BatchClinicalQualityMeasureData=" + QualityMeasureData + "&CQMID=" + cqmid + "&PageNumber=" + pageNumber + "&RowsPerPage=" + rowsPerPage;
        return MDVisionService.defaultService(data, "BATCH_CLINICALQUALITYMEASURE", "SEARCH_ACI_MEASURESGROUP");
    },

    ResetNPI: function () {
        $("#pnliTrackPromotingInteroperability #hfProviderId").val("");
        $("#pnliTrackPromotingInteroperability #NPItxt").val("");
    },
    ResetProvider: function () {
        $("#" + iTrack_PromotingInteroperability.params.PanelID + " #txtProvider").val('');
        $("#" + iTrack_PromotingInteroperability.params.PanelID + " #hfProviderId").val('');
        if ($("#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability #lnkProviderEdit").css("display") == "inline") {
            $("#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability #lnkProviderEdit").css("display", "none");
            $("#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability #lblProvider").css("display", "inline");
        }
    },
    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["RefCtrlHidden"] = "hfProviderId";
        params["ParentCtrl"] = "iTrack_PromotingInteroperability";
        LoadActionPan('Admin_Provider', params);
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
    GetMIPSProvider: function (name) {
        var AllProviders = [];
        var IsValid = false;

        if (name != null && name.length > 1)
            IsValid = true;
        else
            IsValid = false;

        var dfd = new $.Deferred();

        if (IsValid) {
            iTrack_PromotingInteroperability.Search_MIPSProviderPreferences_DBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    responseData = JSON.parse(responseData);
                    if (responseData.IndividualProCount > 0) {
                        var ProviderLoadJSONData = JSON.parse(responseData.IndividualProCountLoad_JSON);
                        iTrack_PromotingInteroperability.params.Groups = ProviderLoadJSONData.Groups;
                        iTrack_PromotingInteroperability.params.GroupsDetails = ProviderLoadJSONData.GroupsDetail;

                        $.each(ProviderLoadJSONData.Groups, function (i, item) {

                            AllProviders.push({ id: item.ProviderId, value: item.ShortName });

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
    BindProvider: function () {

        var Ctrl = $("#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability #txtProvider");
        var func = function () { return iTrack_PromotingInteroperability.GetMIPSProvider(Ctrl.val()) };
        var hfCtrl = $("#pnliTrackPromotingInteroperability #hfProviderId");
        var onSelect = function (e) {
            $("#pnliTrackPromotingInteroperability #hfProviderId").val(e.id);
            iTrack_PromotingInteroperability.setProviderName();

            utility.FillProviderNPI('#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability', '#hfProviderId', '#NPItxt');
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);

    },
    GetPIDatesByProviderId(ProviderId) {
        var objData = new Object();
        objData["ProviderId"] = ProviderId;
        objData["commandType"] = "load_pi_datesbyproviderid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackDashBoard");
    },
    GetPIDatesByGroupId(GroupId) {
        var objData = new Object();
        objData["GroupId"] = GroupId;
        objData["commandType"] = "load_pi_datesbygroupid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackDashBoard");
    },
    //setProviderName: function () {

    //    $("#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability #lblProviderName").text("Dr. " + $("#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability #txtProvider").val() + "|");
    //},
    setProviderName: function () {

        $("#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability #lblProviderName").text("Dr. " + $("#pnliTrackPromotingInteroperability #frmiTrackPromotingInteroperability #txtProvider").val() + "|");

        var providerId = $("#pnliTrackPromotingInteroperability #hfProviderId").val();
        var providerDetails = $.grep(iTrack_PromotingInteroperability.params.Groups, function (a) {
            return a.ProviderId == providerId;
        });
        var groupsDetails = $.grep(iTrack_PromotingInteroperability.params.GroupsDetails, function (a) {
            return a.ProviderId == providerId;
        });

        $.each(groupsDetails, function (i, item) {

            if (item.CategoryName == "Promoting Interoperability (Formally ACI)") {
                $('#pnliTrackPromotingInteroperability #dtpFromDate').datepicker("setDate", utility.RemoveTimeFromDate(null, item.StartDate));
                $('#pnliTrackPromotingInteroperability #dtpToDate').datepicker("setDate", utility.RemoveTimeFromDate(null, item.EndDate));
            }

        });
    },
    viewGraph: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["GraphObj"] = iTrack_PromotingInteroperability.graphObj;
        params["ParentCtrl"] = "iTrack_PromotingInteroperability";
        params["label"] = $("#pnliTrackPromotingInteroperability #pnliTrackPromotingInteroperability_Result #divInformation").html();

        LoadActionPan('iTrack_PromotingInteroperabilityGraph', params);

    },
    /////////////End MIPS Section
}