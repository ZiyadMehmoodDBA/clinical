ROSTemplateDetailRevamp = {
    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This file will handle all actions performed for Physical Exam Template
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    FamilyMembers: [],
    ExamDetails: {},
    SelectedSystem: '',
    array: [],
    myArr: [],
    parentCtrlGlobel: null,
    mainSelected: null,
    SectionNormalInfo: [],
    //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
    selectedcharacteristicsIds: [],
    characteristicsWithData: [],
    selectedsubcharacteristicsIds: [],
    subcharacteristicsWithData: [],
    selectedData: null,
    isNormalTriggred: false,
    isBothUnCheck: false,
    specialityCheckedIds: [],
    providerSelectedIds: [],
    providerCheckedIds: [],
    normalSystemIdsGlobel: [],
    selectedPhyExamTempData: [],
    SpecialtyIds: '',
    ProviderIds: '',
    NewInsertedId: -1,
    selectedObservations: [],
    specialtyIdMKMK: "",
    ProviderIdMKMK: "",
    CharacteristicsInfo: [],
    CharacteristicsDetailsInfo: [],
    numOfCharsChecked: 0,
    CharcSystemInfo: [],
    PositiveNegativeDetail: [],
    IsNornalAllSystems: [],
    ROSCharatristicsDetail: [],
    ROSSystemDetail: [],
    SystemAllPositiveNegative: [],
    orthopedicSpecialitySelected : false,
    Load: function (params) {

        ROSTemplateDetailRevamp.params = params;
        var isSelectedEntity = false
        ROSTemplateDetailRevamp.selectedPhyExamTempData = [];
        ROSTemplateDetailRevamp.selectedObservations = [];
        var self = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #tblROSTemplateDetailRevamp');

        self.loadDropDowns(true).done(function () {
            if (ROSTemplateDetailRevamp.params["mode"] == "Edit") {
                var dfd = new $.Deferred();
                ROSTemplateDetailRevamp.loadDropDowns(dfd);
                dfd.done(function (n) {
                    ROSTemplateDetailRevamp.LoadROS();
                });
            }
            else {
                ROSTemplateDetailRevamp.loadDropDowns();
            }
            ROSTemplateDetailRevamp.ROSLookUps();
            ROSTemplateDetailRevamp.domReadyFunction();
            ROSTemplateDetailRevamp.buildSystemsAutoComplete();
            ROSTemplateDetailRevamp.bindCharacteristicsAutoComplete();

            ROSTemplateDetailRevamp.validateROSTemplate();

            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
            // ROSTemplateDetailRevamp.toggleControls();

        });
    },

    loadDropDowns: function (dfd) {
        var self = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #tblROSTemplateDetailRevamp');
        if (globalAppdata.AppUserName.toLowerCase() != DefaultUser.toLowerCase()) {
            self.find("#ddlPhysicalExamTemplateEntity").val(globalAppdata["SeletedEntityId"]);
            if (self.find("div#divEntity").hasClass("hidden") == false)
                self.find("div#divEntity").addClass("hidden");
            isSelectedEntity = true;
        }
        else {
            self.find("div#divEntity").removeClass("hidden");
        }
        ROSTemplateDetailRevamp.loadEntitySpecialty(globalAppdata["SeletedEntityId"], dfd);
        if (ROSTemplateDetailRevamp.params.Title != null)
            $("#" + ROSTemplateDetailRevamp.params["PanelID"] + " #headingTitle").text(ROSTemplateDetailRevamp.params.Title);
    },

    loadEntitySpecialtyMK: function (entityID) {
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty_').empty();
                    var data = [];

                    $.each(spacialties, function (i, item) {
                        data.push({
                            id: item.SpecialtyId, value: item.SpecialtyId, text: item.ShortName, expanded: true, spriteCssClass: "rootfolder"
                        });
                    });
                    $("#ddlPhysicalExamTemplateSpecialty_").kendoAutoComplete({
                        dataSource: data,
                        filter: "contains",
                        dataTextField: "text",
                        placeholder: "Select Specialty...",
                        select: function (e) {
                            ROSTemplateDetailRevamp.specialityCheckedIds = [];
                            var dataItem = this.dataItem(e.item.index());
                            ROSTemplateDetailRevamp.specialityCheckedIds.push(dataItem.id);
                            ROSTemplateDetailRevamp.loadSpecialityProviders(dataItem.id);
                            $("#ddlPhysicalExamTemplateSpecialty_").val(dataItem.text);
                            e.preventDefault();
                        },
                        footerTemplate: 'Total <strong>#: instance.dataSource.total() #</strong> items found'
                    });

                }
            });
        }
    },

    loadSpecialityProviders: function (SpecialityId) {
        var data = [];
        if (ROSTemplateDetailRevamp.specialityCheckedIds.length > 0) {
            var objData = new Object();
            objData["IsActive"] = true;
            objData["SpecialtyIds"] = SpecialityId;

            ROSTemplateDetailRevamp.GetSpecialityProvider_DBCall(objData).done(function (response) {
                if (response.status != false) {
                    response = JSON.parse(response);
                    response = JSON.parse(response.PEProvider_JSON);
                    response = JSON.parse(response);
                    for (var i = 0; i < response.length; i++)
                        data.push({ id: response[i].ProviderId, value: response[i].ProviderId, text: response[i].Name });

                    $("#ddlPhysicalExamTemplateProvider_").kendoMultiSelect({
                        dataTextField: "text",
                        dataValueField: "value",
                        dataSource: data,
                        select: function (e) {
                            var dataItem1 = e.dataItem;
                            var dataItem = this.dataItem(e.item.index());
                            //alert(kendo.stringify(dataItem));
                        },
                    });
                }
            });
        }
    },

    GetSpecialityProvider_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "Load_PhyscialExam_SpecialtyProvider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    //loadEntitySpecialtyMK: function (entityID) {
    //    if (entityID != null && entityID > 0) {

    //        providerDetail.FillSpecialty(entityID).done(function (response) {
    //            if (response.status != false) {

    //                var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
    //                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').empty();
    //                var data = [];

    //                $.each(spacialties, function (i, item) {
    //                    data.push({
    //                        id: item.SpecialtyId, value: item.SpecialtyId, text: item.ShortName, expanded: true, spriteCssClass: "rootfolder"
    //                    });
    //                });
    //                $("#ddlPhysicalExamTemplateSpecialty").kendoMultiSelect({
    //                    dataTextField: "text",
    //                    dataValueField: "value",
    //                    dataSource: data,
    //                    select: function (e) {
    //                        var dataItem1 = e.dataItem;
    //                        var dataItem = this.dataItem(e.item.index());
    //                        alert(kendo.stringify(dataItem));

    //                    },
    //                });

    //            }
    //        });
    //    }
    //},

    //toggleControls: function () {
    //    $("#btnBold").click(function () {
    //        $('#observationContent div[id^=divSystem]').toggleClass("bold");
    //    });

    //    $("#btnItalic").click(function () {
    //        $('#observationContent div[id^=divSystem]').toggleClass("italic");
    //    });

    //    $("#btnUnderline").click(function () {
    //        $('#observationContent div[id^=divSystem]').toggleClass("underline");
    //    });

    //},

    LoadROS: function () {
        $("#observationContent").text('');
        $("#SystemPreview").removeClass('hidden');
        ROSTemplateDetailRevamp.LoadROSTempSysCharateristics(ROSTemplateDetailRevamp.params.PhysicalExamTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.PETemplateCount > 0) {
                    var res = JSON.parse(response.PETemplate_JSON);
                    var templateData = JSON.parse(utility.decodeHtml(res));
                    if (templateData.length > 0 && templateData[0] != null) {

                        $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #txtPhysicalExamTemplateName").val(templateData[0].TemplateName);
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #observationContent").text(templateData[0].TemplatePreview);
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #txtComments").text(templateData[0].TemplateComments);
                        if (templateData[0].BodyPartId) {
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #divROSTemplateBodyPart').removeClass('hidden');
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ddlBodyParts').val(templateData[0].BodyPartId);
                        }
                        else {
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #divROSTemplateBodyPart').addClass('hidden');
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ddlBodyParts').val("");
                        }
                        var arrSpecialtyIds = templateData[0]['SpecialtyIds'].split(',');
                        utility.callbackAfterAllDOMLoaded(function () {
                            $.each(arrSpecialtyIds, function (i, itm) {
                                var OptionText = $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #ddlPhysicalExamTemplateSpecialty option[value='" + itm + "']").text();
                                if (OptionText.toLowerCase() == "orth surg")
                                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #divROSTemplateBodyPart').removeClass('hidden');
                            });
                        });
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('clearSelection', false);
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('updateButtonText');
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + " #ddlPhysicalExamTemplateProvider").val(templateData[0]['ProviderIds'].split(','));
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect("refresh");
                        //Start 16-10-2017 Humaira Yousaf Bug# EMR-4972
                        if (templateData[0]['ProviderIds'] != "") {
                            ROSTemplateDetailRevamp.providerCheckedIds = templateData[0]['ProviderIds'].split(',');
                        }
                        //End 16-10-2017 Humaira Yousaf Bug# EMR-4972
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('clearSelection', false);
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('updateButtonText');
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + " #ddlPhysicalExamTemplateSpecialty").val(templateData[0]['SpecialtyIds'].split(','));
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect("refresh");
                        //Start 16-10-2017 Humaira Yousaf Bug# EMR-4972
                        if (templateData[0]['SpecialtyIds'] != "") {
                            ROSTemplateDetailRevamp.specialityCheckedIds = templateData[0]['SpecialtyIds'].split(',');
                        }
                        //End 16-10-2017 Humaira Yousaf Bug# EMR-4972
                    }
                }

                if (response.PETemplateSystemsCount > 0) {
                    $("#observationContent div").remove();
                    $("#ulPhysicalExamSystems li").remove();
                    var resTemplateSystems = JSON.parse(response.PETemplateSystems_JSON);
                    var SystemData = JSON.parse(resTemplateSystems);

                    for (var i = 0; i < SystemData.length; i++) {
                        if (SystemData[i].ROSSystemId != "") {
                            var li = ROSTemplateDetailRevamp.addSystem(SystemData[i].ROSSystemId, SystemData[i].SystemName);
                            $("#ulPhysicalExamSystems").append(li);

                            if (SystemData[i].IsSelectedSystem == "True") {
                                $("#ulPhysicalExamSystems #chk" + SystemData[i].ROSSystemId).prop("checked", true);
                            }

                            if (response.PESysObservationsCount > 0) {
                                var resTemplateObservations = JSON.parse(response.PESysObservations_JSON);
                                var ObservationData = JSON.parse(resTemplateObservations);

                                for (var j = 0; j < ObservationData.length; j++) {
                                    if (SystemData[i].ROSSystemId == ObservationData[j].ROSSystemId) {
                                        var li = ROSTemplateDetailRevamp.addCharatristics(ObservationData[j].ROSCharacteristicsId, ObservationData[j].CharacteristicsName, ObservationData[j].ROSSystemId);
                                        $("#ulPhysicalExamSystemSection").append(li);

                                        var objSelectedObservations = {
                                        };
                                        if (ObservationData[j].TempSysCharIsSelected == "True") {
                                            $("#ulPhysicalExamSystemSection #chk" + ObservationData[j].ROSCharacteristicsId).prop("checked", true);
                                            if (SystemData[i].IsSelectedSystem == "True") {
                                                objSelectedObservations = {
                                                    ROSSystemId: ObservationData[j].ROSSystemId, IsChecked: true, ROSCharacteristicsId: ObservationData[j].ROSCharacteristicsId, ROSCharacteristicsName: ObservationData[j].CharacteristicsName, IsSystemChecked: true, IsSystemDeleted: 0, IsObservationDeleted: 0
                                                };
                                            }
                                            else {
                                                objSelectedObservations = {
                                                    ROSSystemId: ObservationData[j].ROSSystemId, IsChecked: true, ROSCharacteristicsId: ObservationData[j].ROSCharacteristicsId, ROSCharacteristicsName: ObservationData[j].CharacteristicsName, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0
                                                };
                                            }

                                            //ROSTemplateDetailRevamp.handleDelimiter(ObservationData[j].ROSSystemId, ObservationData[j].ROSCharacteristicsId, ObservationData[j].ROSCharacteristicsName, true);
                                        }
                                        else {
                                            $("#ulPhysicalExamSystemSection #chk" + ObservationData[j].ROSCharacteristicsId).prop("checked", false);
                                            if (SystemData[i].IsSelectedSystem == "True") {
                                                objSelectedObservations = {
                                                    ROSSystemId: ObservationData[j].ROSSystemId, IsChecked: false, ROSCharacteristicsId: ObservationData[j].ROSCharacteristicsId, ROSCharacteristicsName: ObservationData[j].CharacteristicsName, IsSystemChecked: true, IsSystemDeleted: 0, IsObservationDeleted: 0
                                                };
                                            }
                                            else {
                                                objSelectedObservations = {
                                                    ROSSystemId: ObservationData[j].ROSSystemId, IsChecked: false, ROSCharacteristicsId: ObservationData[j].ROSCharacteristicsId, ROSCharacteristicsName: ObservationData[j].CharacteristicsName, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0
                                                };
                                            }

                                            //ROSTemplateDetailRevamp.handleDelimiter(ObservationData[j].ROSSystemId, ObservationData[j].ROSCharacteristicsId, ObservationData[j].ROSCharacteristicsName, false);
                                        }


                                        ROSTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
                                        // caching the detail of charateristics
                                        var chardetail = JSON.parse(response.ROSSysCharaDetail);
                                        chardetail = JSON.parse(chardetail);
                                        if (chardetail.length > 0) {
                                            var ind = false;
                                            var detail = {
                                                PreviousHistory: "",
                                                ROSCharacteristicsDetailStatusId: "",
                                                Onset: "",
                                                Duration: "",
                                                ROSCharacteristicsDetailDurationId: "",
                                                ROSCharacteristicsDetailPatternId: "",
                                                ROSCharacteristicsDetailSeverityId: "",
                                                ROSCharacteristicsDetailCourseId: "",
                                                ROSCharacteristicsDetailRadiationId: "",
                                                ROSCharacteristicsDetailFrequencyId: "",
                                                ROSCharacteristicsDetailContextId: "",
                                                ROSCharacteristicsDetailCharacterCSZId: "",
                                                ROSCharacteristicsDetailAggravedById: "",
                                                ROSCharacteristicsDetailRelievedById: "",
                                                Location: "",
                                                PrecipitatedBY: "",
                                                AssociatedWith: "",
                                                ROSCharacteristicsDetailsId: ""
                                            };

                                            for (var z = 0 ; z < chardetail.length ; z++) {
                                                if (chardetail[z].ROSSystemId == SystemData[i].ROSSystemId && chardetail[z].ROSCharacteristicsId == ObservationData[j].ROSCharacteristicsId) {
                                                    if (chardetail[z].LookupTypeName == "Location") {
                                                        detail.Location = chardetail[z].Value;
                                                    }
                                                    if (chardetail[z].LookupTypeName == "AssociatedBy") {
                                                        detail.AssociatedWith = chardetail[z].Value;
                                                    }
                                                    if (chardetail[z].LookupTypeName == "PrecipitatedBy") {
                                                        detail.PrecipitatedBY = chardetail[z].Value;
                                                    }
                                                    if (chardetail[z].LookupTypeName == "PreviousHistory") {
                                                        detail.PreviousHistory = chardetail[z].Value;
                                                    }
                                                    if (chardetail[z].LookupTypeName == "ROSDuration") {
                                                        detail.Duration = chardetail[z].Value;
                                                    }
                                                    if (chardetail[z].LookupTypeName == "ROSOnset") {
                                                        detail.Onset = chardetail[z].Value;
                                                    }
                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailStatus") {
                                                        detail.ROSCharacteristicsDetailStatusId = chardetail[z].LookUpId;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailDuration") {
                                                        detail.ROSCharacteristicsDetailDurationId = chardetail[z].LookUpId;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailPattern") {
                                                        detail.ROSCharacteristicsDetailPatternId = chardetail[z].LookUpId;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailSeverity") {
                                                        detail.ROSCharacteristicsDetailSeverityId = chardetail[z].LookUpId;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailCourse") {
                                                        detail.ROSCharacteristicsDetailCourseId = chardetail[z].LookUpId;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailRadiation") {
                                                        detail.ROSCharacteristicsDetailRadiationId = chardetail[z].LookUpId;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailFrequency") {
                                                        detail.ROSCharacteristicsDetailFrequencyId = chardetail[z].LookUpId;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailContext") {
                                                        detail.ROSCharacteristicsDetailContextId = chardetail[z].LookUpId;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailCharacterCSZ") {
                                                        detail.ROSCharacteristicsDetailCharacterCSZId = chardetail[z].LookUpId;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailAggravedBy") {
                                                        detail.ROSCharacteristicsDetailAggravedById = chardetail[z].LookUpId;
                                                    }
                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailRelievedBy") {
                                                        detail.ROSCharacteristicsDetailRelievedById = chardetail[z].LookUpId;
                                                    }
                                                    ind = true;

                                                }

                                            }
                                            if (ind == true) {
                                                var DetailData_jason = JSON.stringify(detail)
                                                var CharsDetailInfo = {
                                                    Id: ObservationData[j].ROSCharacteristicsId,
                                                    SystemId: ObservationData[j].ROSSystemId,
                                                    CharcId: ObservationData[j].ROSCharacteristicsId,
                                                    DetailInfo: DetailData_jason
                                                };
                                                ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.push(CharsDetailInfo);
                                            }

                                        }


                                        // var DetailData = JSON.stringify(ObservationData[j])
                                        //DetailData = '{' +DetailData.substring(DetailData.indexOf('"PreviousHistory":'), DetailData.indexOf(',"ROSTempSysCharId":')) + '}';

                                        //ROSTemplateDetailRevamp.populateDetailAgainstCharacteristic(ObservationData[j]);
                                        // ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.push(CharsDetailInfo);
                                        //ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(false);
                                        var isPositive = false;
                                        var IsNegative = false;
                                        if (ObservationData[j].isPositive == "True") {
                                            isPositive = true;
                                            IsNegative = false;
                                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').addClass('green');
                                        }
                                        else if (ObservationData[j].isPositive == "False") {
                                            isPositive = false;
                                            IsNegative = true;
                                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').addClass('green');
                                        }
                                        var CharsPosNegDetail = {
                                            Id: ObservationData[j].ROSSystemId + '-' + ObservationData[j].ROSCharacteristicsId,
                                            SystemId: ObservationData[j].ROSSystemId,
                                            CharcId: ObservationData[j].ROSCharacteristicsId,
                                            CharcComments: ObservationData[j].TempSysCharComments,
                                            IsPositive: isPositive,
                                            IsNegative: IsNegative

                                        };
                                        ROSTemplateDetailRevamp.PositiveNegativeDetail.push(CharsPosNegDetail);
                                    }
                                }
                            }


                            if (SystemData[i].IsNormal == "True") {
                                var SystemIsNormal = {
                                    Id: SystemData[i].ROSSystemId,
                                    SystemId: SystemData[i].ROSSystemId,
                                    IsNormalComments: SystemData[i].NormalComments,
                                    IsNormal: true,


                                };
                                ROSTemplateDetailRevamp.IsNornalAllSystems.push(SystemIsNormal);
                                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #btnOpenDetailIsNormalAll').removeClass('disableAll');

                            }
                            var IsSystemPositive = null;
                            var IsSystemNegative = null;
                            if (SystemData[i].IsSystemSelectAll == "True") {
                                IsSystemPositive = true;
                                IsSystemNegative = false;
                                //  $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').('green');
                                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                            }
                            else if (SystemData[i].IsSystemSelectAll == "False") {
                                IsSystemPositive = false;
                                IsSystemNegative = true;
                                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
                                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').addClass('red');
                            }
                            else {
                                IsSystemPositive = null;
                                IsSystemNegative = null;
                                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
                                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                            }
                            var IsPosNegAll = {
                                SystemId: SystemData[i].ROSSystemId,
                                IspositiveAll: IsSystemPositive,
                                IsnegativeAll: IsSystemNegative
                            };
                            ROSTemplateDetailRevamp.SystemAllPositiveNegative.push(IsPosNegAll);

                        }
                    }
                    $('#ulPhysicalExamSystems #' + SystemData[0].ROSSystemId).click();

                }
                $('#frmROSTemplateDetailRevamp').data('serialize', $('#frmROSTemplateDetailRevamp').serialize());
            }
        });
    },

    handleDelimiter: function (ROSSystemId, ROSCharacteristicsId, ROSCharacteristicsName, IsChecked) {
        //$("#observationContent").text('');
        if (IsChecked) {
            var delimator = $("#delimator option:selected").text() + " ";
            if ($("#observationContent #divSystem" + ROSSystemId + ROSCharacteristicsId).length > 0) {
                $('#observationContent #divSystem' + ROSSystemId + ROSCharacteristicsId).remove();
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + ROSSystemId + ROSCharacteristicsId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + ROSSystemId + ROSCharacteristicsId).show();
                var txtToAppend = ROSCharacteristicsName;

                if ($('#observationContent div').length > 1)
                    txtToAppend = delimator + ROSCharacteristicsName;

                $("#observationContent #divSystem" + ROSSystemId + ROSCharacteristicsId).append(txtToAppend);
            }
            else {
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + ROSSystemId + ROSCharacteristicsId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + ROSSystemId + ROSCharacteristicsId).show();
                var txtToAppend = ROSCharacteristicsName;

                if ($('#observationContent div').length > 1)
                    txtToAppend = delimator + ROSCharacteristicsName;

                $("#observationContent #divSystem" + ROSSystemId + ROSCharacteristicsId).append(txtToAppend);
            }
        }
    },

    LoadROSTempSysCharateristics: function (templateId) {
        var objData = new Object();
        objData["TemplateId"] = templateId;
        objData["commandType"] = ""; objData["commandType"] = "select_reviewofsystem_template";//Load_PhyscialExam_TemplatesFill_ECW
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ReviewofSystemRevampTemplate");
    },

    addSystem: function (ROSSystemId, SystemName) {
        var itemtoRemove = "system";

        for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
            if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId && ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted != 1) {
                utility.DisplayMessages(SystemName + " already associated with the template", 3);
                return;
            }
        }

        for (var i = 0; i < $("#ulPhysicalExamSystems li").length; i++) {
            if ($($("#ulPhysicalExamSystems li label")[i]).text() == SystemName) {
                utility.DisplayMessages(SystemName + " already associated with the template", 3);
                return;
            }

        }

        var li = '<li id="' + ROSSystemId + '" parentid="null" onclick="ROSTemplateDetailRevamp.loadCharatristics(' + ROSSystemId + ')" value="' + ROSSystemId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                 '<input type="checkbox" id="chk' + ROSSystemId + '" name="' + ROSSystemId + '" class="pull-left  char" onclick="ROSTemplateDetailRevamp.ManageCharateristics(' + ROSSystemId + ', this);">' +

                 '<label id="lblName' + ROSSystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SystemName + '">' +
                 '<span><b><a style="color:black !important" href="javascript:void(0);" onclick="ROSTemplateDetailRevamp.ClearInfo(event, this,' + ROSSystemId + ',\'' + SystemName + '\');"  > <span title="Reset" class="glyphicon glyphicon-refresh">&nbsp</span> </a></b></span>' +
                 '' + SystemName + '</label><div id="divNameDetail' + ROSSystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + ROSSystemId + '" onkeypress="" name="Name' + ROSSystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 ROSSystemId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="ROSTemplateDetailRevamp.removeSystem(' + ROSSystemId + ', event)";><i class="fa fa-close"></i></span></a></li>';
        // ROSTemplateDetailRevamp.removeSystem(' + ROSSystemId + ')  zia html
        return li;
    },

    addCharatristics: function (ROSCharacteristicsId, CharatristicName, ROSSystemId) {
        var a = ROSTemplateDetailRevamp.selectedObservations;
        var itemtoRemove = "observation";
        CharatristicName = CharatristicName.trim();
        var li = '<li id="' + ROSCharacteristicsId + '" parentid="' + ROSSystemId + '" onclick="ROSTemplateDetailRevamp.loadSysPatCharcDetail(this);" value="' + ROSCharacteristicsId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                 '<input type="checkbox" id="chk' + ROSCharacteristicsId + '" name="' + ROSCharacteristicsId + '" class="pull-left  char" ' +
                 'onclick="ROSTemplateDetailRevamp.PreviewCharateristics(' + ROSCharacteristicsId + ',\'' + CharatristicName + '\', this, ' + ROSSystemId + ');"><label id="lblName' + ROSCharacteristicsId + '" class="" data-toggle="tooltip" title="" data-original-title="' + CharatristicName + '">' +
                '<input type="hidden" id="isSystemNormal" name="isSystemNormal" value="" /><input type="hidden" id="systemNormalDescription" name="systemNormalDescription" value="" /><input type="hidden" id="systemPatientID" name="systemPatientID" value="" /><input type="hidden" id="isCharacteristicsExists" name="isCharacteristicsExists" value="false"" />' +
                 '<button type="button" id="btnOpenDetail' + ROSCharacteristicsId + '"  data-toggle="tooltip" data-placement="top"  onclick="ROSTemplateDetailRevamp.openCharacteristicDetail(this,' + ROSCharacteristicsId + ');" class="btn btn-link btn-xs pull-left"><i class="fa fa-book"></i></button>' +
                 //'<span><a href="javascript:void(0);" onclick="ROSTemplateDetailRevamp.openCharacteristicDetail(this,' + ROSCharacteristicsId + ');"  value="" class="" style="margin-right:5px;color:black"> <i class="fa fa-book"></i> </a></span>' +
                 '<span><b><a id="Negative" href="javascript:void(0);" onclick="ROSTemplateDetailRevamp.checkUncheckPositiveNegative(this,' + ROSSystemId + ');"  value="" class="" style="margin-right:5px;color:black"> (-) </a></b></span>' +
                 '<span><b><a id="Positive"  href="javascript:void(0);" onclick="ROSTemplateDetailRevamp.checkUncheckPositiveNegative(this,' + ROSSystemId + ');"  value="" class="" style="margin-right:5px;color:black"> (+) </a></b></span>' +
                 '</label><span>' + CharatristicName + '</span><div id="divNameDetail' + ROSCharacteristicsId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + ROSCharacteristicsId + '" onkeypress="" name="Name' + ROSCharacteristicsId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 ROSCharacteristicsId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="ROSTemplateDetailRevamp.removeCharateristics(' + ROSSystemId + ', ' + ROSCharacteristicsId + ')"><i class="fa fa-close"></i></span></a>' +
                 '<div class="clearfix"</div><div id="divDetail' + ROSCharacteristicsId + '" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetail' + ROSCharacteristicsId + '" name="CharacteristicDetail' + ROSCharacteristicsId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="ROSTemplateDetailRevamp.setfoucustoarea(this)" ></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + ROSCharacteristicsId + '" onclick="ROSTemplateDetailRevamp.validateMaxLength(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a><div class="clearfix"></div></li>';
        return li;  // zia html 2
    },
    validateMaxLength: function (obj) {
        //var maxLength = 250;
        //if (obj.value.length > maxLength) {

        //    obj.value = obj.value.substring(0, 250);
        //}
        var SystemID = $(obj).parents('li').attr('parentid');
        var CharID = $(obj).parents('li').attr('id');
        indexes = $.map(ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
            if (item.CharcId == CharID && item.SystemId == SystemID) {
                return index;
            }
        })


        var CharPosNegDetailIndex = indexes[0];
        ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].CharcComments = $(obj).parents('#divDetail' + CharID + '').find('textarea').val();

        if ($(obj).parents('li').attr('parentid') == SystemID) {

            if ($(obj).parents('#divDetail' + CharID + '').find('textarea').val() == '') {

                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + CharID + '"][parentid="' + SystemID + '"]').find('i.fa-book').addClass('green');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + CharID + '"][parentid="' + SystemID + '"]').find('i.fa-book').removeClass('blue');

                //$(obj).parents('li').find('i.fa-book').addClass('bule');
                //$(obj).parents('li').find('i.fa-book').removeClass('green');

            }
            else {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + CharID + '"][parentid="' + SystemID + '"]').find('i.fa-book').addClass('green');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + CharID + '"][parentid="' + SystemID + '"]').find('i.fa-book').removeClass('blue');

                // $(obj).parents('li').find('i.fa-book').addClass('green');
                //$(obj).parents('li').find('i.fa-book').removeClass('blue');
            }
        }
        $(obj).closest("li").find('div [id*="divDetail"]').addClass('hidden');

    },
    setfoucustoarea: function (obj) {
        //obj.focus();
        //return;
    },
    showHideToggleDetails: function (isToShow) {
        var toggleDetailsDiv = $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamDataTemplateDetail div#divTogglePhysicalExamDetails");
        if (isToShow == false) {
            if (toggleDetailsDiv.hasClass("hidden") == false) {
                toggleDetailsDiv.addClass("hidden");
                $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails").addClass('hidden');
            }
        }
        else {

            toggleDetailsDiv.removeClass("hidden");
        }

    },
    openCharacteristicDetail: function (objButton, detailParentId) {
        if (objButton != null && detailParentId != null) {
            var liObject = $(objButton).parents('li');
            // var characteristicType = ROSTemplateDetailRevamp.getCharacteristicType(liObject);
            if (!$(objButton).parents('li').siblings().find('div [id^="divDetail"]').hasClass(':not(hidden)')) {
                $(objButton).parents('li').siblings().find('div [id^="divDetail"]').addClass('hidden')
            }
            var pkid = $(liObject).prop("id");
            var type = $(liObject).closest("ul").prop("id");
            //$(objButton).parents('li').find('#Negative').css('pointer-events', 'none');
            //$(objButton).parents('li').find('#Positive').css('pointer-events', 'none');
            var isParentChecked = ROSTemplateDetailRevamp.isSystemChecked(null, liObject);
            if (isParentChecked == true) {
                var SystemDetailDiv = $(objButton).parents('li').find("div#divDetail" + detailParentId);
                if (SystemDetailDiv.hasClass("hidden") == true) {
                    SystemDetailDiv.removeClass("hidden");

                    $(SystemDetailDiv).find('textarea').focus();


                }
                else {
                    SystemDetailDiv.addClass("hidden")
                    if ($(SystemDetailDiv).find('textarea').val() != '') {
                        //$(objButton).find('i.fa-book').addClass('green');
                        //$(objButton).find('i.fa-book').removeClass('blue');
                        // $(objButton).parents('label').siblings('span').addClass('green')
                    }
                    else {
                        //$(objButton).find('i.fa-book').removeClass('green');
                        //$(objButton).parents('label').siblings('span').removeClass('green')
                    }

                    //  var comments = $(SystemDetailDiv).find('textarea').val();
                    // ROSTemplateDetailRevamp.updateTheCommentsOfCharAndSubChar(comments, pkid, type);
                    $(SystemDetailDiv).find('textarea').focusout();
                }
            }
            else {
                utility.DisplayMessages("Please mark this characteristic as +ve/-ve to add details", 3);
            }
            //.removeClass("hidden");
        }
    },
    isSystemChecked: function (objulSystem, liObject) {
        var DetailExists = false;
        if (liObject != null && liObject != "") {
            $(liObject).find("a[value='true']").each(function (i, chkitem) {
                // if ($(chkitem).prop("checked") == true && DetailExists == false) {
                DetailExists = true;
                // }
            });
        }
        else {
            objulSystem.find("li").each(function (i, item) {
                $(item).find("a[value='true']").each(function (i, chkitem) {
                    //  if ($(chkitem).prop("checked") == true && DetailExists == false) {
                    DetailExists = true;
                    //  }
                });
            });
        }


        return DetailExists
    },
    getCharacteristicType: function (liObject) {
        var isSubcharacteristic = $(liObject).parent().attr("id").indexOf("SubCharacteristics") > -1 ? true : false;
        var characteristicType = isSubcharacteristic == true ? "Sub-Characteristic" : "Characteristic";
        return characteristicType;
    },
    domReadyFunction: function () {
        $(function () {
            setTimeout(function () { ROSTemplateDetailRevamp.countWidth(); }, 1000); //405
            $('#' + ROSTemplateDetailRevamp.params.PanelID + " .toggleVertical div.toggle").click(function (e) {
                if ($(this).children().hasClass("active")) {
                    $(this).prev().removeClass("hidden");

                    ROSTemplateDetailRevamp.countWidth(e);
                    $(this).parent().parent().scrollLeft(1000);
                }
                else {
                    $(this).prev().addClass("hidden");
                    ROSTemplateDetailRevamp.countWidth(e);
                    $(this).parent().parent().scrollLeft(0);
                }
            });
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp').on('change', function () {
                $("#" + ROSTemplateDetailRevamp.params["PanelID"] + ' #frmROSTemplateDetailRevamp #hfIsFormHasChange').val('true');
            });


        });


    },
    countWidth: function (obj) {
        var panelChildrens = null;
        var currentPanel = null;
        var applyWidth = null;
        if (obj != null) {
            currentPanel = $(obj.delegateTarget).parent();
            panelChildrens = currentPanel.children("section.panel");
            applyWidth = currentPanel;
            ROSTemplateDetailRevamp.countWidthApply(currentPanel, panelChildrens, applyWidth);
        }
        else {
            $('.toggleVertical').each(function (e) {
                currentPanel = $(this);
                panelChildrens = currentPanel.children().children("section.panel");
                applyWidth = currentPanel.children();
                ROSTemplateDetailRevamp.countWidthApply(currentPanel, panelChildrens, applyWidth);
            });
        }
    },

    // Apply the calculated width
    countWidthApply: function (currentPanel, panelChildrens, applyWidth) {
        var totalWidth = 0;
        var hidden = 0;
        //find total width of panels
        panelChildrens.each(function (e) {
            totalWidth += $(this).outerWidth(true);
        });
        //find total width of hidden panel
        currentPanel.find(":hidden.panel").each(function (e) {
            hidden += $(this).outerWidth(true);
        });
        //apply width to div
        document.getElementById("frmROSTemplateDetailRevamp").getElementsByClassName("frmROSTemplateDetailRevampPhysicalExam")[0].setAttribute("style", "width:" + (totalWidth - hidden + 50) + "px");
    },

    clearSectionInfo: function (event, obj, SystemID, SystemName) {
        event.stopPropagation();
        //if (Clinical_ReviewofSystems.CharcSystemInfo[SystemName] != null && $(obj).parent().find('span:first-child').hasClass('green')) {
        if (($('#' + ROSTemplateDetailRevamp.params.PanelID + " #divReviewofSystemsSystemSection").find('input[type=checkbox]:checked').length > 0 || $(obj).closest("li").find('span').hasClass('green'))) {
            var message = 'This will reset all values in ' + SystemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?';
            var sysPatID = $(obj).closest("li").find("#systemPatientID").val();
            var sysPatNormal = $(obj).closest("li").find("#isSystemNormal").val();
            var CharcID = $(obj).closest("li").find("#isCharacteristicsExists").val();
            utility.myConfirm(message, function () {

                if ((typeof CharcID != 'undefined' && CharcID != '') || (typeof sysPatNormal != 'undefined' && sysPatNormal == 'True' || sysPatNormal == true)) {

                    ROSTemplateDetailRevamp.rOSDataSystemReset_DBCall(sysPatID).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            ROSTemplateDetailRevamp.clearInfoForSystemReset(obj, SystemID, SystemName);
                        }
                    });
                } else {
                    ROSTemplateDetailRevamp.clearInfoForSystemReset(obj, SystemID, SystemName);
                }
            }, function () {
            }, 'Reset Confirmation');

        }
    },
    toggleCheckBoxes: function (chkObject) {

        if ($(chkObject).attr('value') == '') {
            $(chkObject).parents('li').find('a[value]').attr('value', '');
            $(chkObject).attr('value', true);
            $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp .toggle").removeClass('disableAll');
            $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #detailHeading ").text($(chkObject).closest('li').text() + " Detail");
            $(chkObject).css({ "color": "red" });
            $(chkObject).attr('value', true);
            if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                ROSTemplateDetailRevamp.toggleDetailsDiv();
                var CharacteristicId = $(chkObject).closest('li').attr("id");

            }
            else {
                ROSTemplateDetailRevamp.toggleDetailsDiv();
                var SubCharacteristicId = $(chkObject).closest('li').attr("id");

            }

        }
        else {
            $(chkObject).attr('value', '');
            var isbothUnCheck = true;
            $(chkObject).parents('li').find('a[value]').each(function () {
                if ($(this).attr('value') == 'true') {
                    isbothUnCheck = false;
                }
            });
            if (isbothUnCheck) {
                setTimeout(function (chkObject) {
                    $(chkObject).closest('li').removeClass("active");
                    // $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp section#CharacteristicsSubCharacteristics");

                    //if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                    //    $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp  #CharacteristicsSubCharacteristics").addClass('hidden');;
                    //}
                    ROSTemplateDetailRevamp.toggleDetailsDiv();
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp .toggle[data-plugin-toggle]").addClass('disableAll');
                    $(chkObject).css({ "color": "black" });

                }, 100, chkObject);
            }
            else {
                ROSTemplateDetailRevamp.toggleDetailsDiv();
                $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp div#divTogglePhysicalExamDetails").removeClass('hidden');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #detailHeading ").text($(chkObject).closest('li').text() + " Detail");

                if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                    ROSTemplateDetailRevamp.toggleDetailsDiv();
                    var CharacteristicId = $(chkObject).closest('li').attr("id");

                }
                else {
                    ROSTemplateDetailRevamp.toggleDetailsDiv();
                    var SubCharacteristicId = $(chkObject).closest('li').attr("id");

                }
            }
        }

    },
    ClearInfo: function (event, obj, SystemID, SystemName) {
        event.stopPropagation();
        //  if (Clinical_ReviewofSystems.CharcSystemInfo[SystemName] != null && $(obj).parent().find('span:first-child').hasClass('green')) {
        //  if (($('#' + ROSTemplateDetailRevamp.params.PanelID + " #divPhysicalExamSystemSection ul li").find("a[style='margin-right: 5px; color: rgb(255, 0, 0);']") > 0 || $(obj).closest("li").find('span').hasClass('green'))) {
        if ($(obj).closest("li.green").length > 0) {
            var message = 'This will reset all values in ' + SystemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?';
            var sysPatID = $(obj).closest("li").find("#systemPatientID").val();
            var sysPatNormal = $(obj).closest("li").find("#isSystemNormal").val();
            var CharcID = $(obj).closest("li").find("#isCharacteristicsExists").val();
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
            utility.myConfirm(message, function () {

                if ((typeof CharcID != 'undefined' && CharcID != '') || (typeof sysPatNormal != 'undefined' && sysPatNormal == 'True' || sysPatNormal == true)) {

                    ROSTemplateDetailRevamp.rOSDataSystemReset_DBCall(sysPatID).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            ROSTemplateDetailRevamp.clearInfoForSystemReset(obj, SystemID, SystemName);
                        }
                    });
                } else {
                    ROSTemplateDetailRevamp.clearInfoForSystemReset(obj, SystemID, SystemName);
                    //  remove caching...
                    indexIsSystemExist = $.map(ROSTemplateDetailRevamp.IsNornalAllSystems, function (item, index) {
                        if (item.Id == SystemID && item.SystemId == SystemID) {
                            return index;
                        }
                    })
                    if (indexIsSystemExist.length > 0) {

                        ROSTemplateDetailRevamp.IsNornalAllSystems.splice(indexIsSystemExist[0], 1);
                    }

                    indexPosNeg = $.map(ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                        if (item.SystemId == SystemID) {
                            // ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg[i-1], 1);
                            return index;
                        }
                    })
                    if (indexPosNeg.length > 0) {
                        // ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg,1 );
                        for (var i = indexPosNeg.length; i > 0 ; i--) {

                            ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg[i - 1], 1);
                        }
                    }
                    indexAllsysPostNeg = $.map(ROSTemplateDetailRevamp.SystemAllPositiveNegative, function (item, index) {
                        if (item.SystemId == SystemID) {
                            return index;
                        }
                    })
                    if (indexAllsysPostNeg.length > 0) {

                        ROSTemplateDetailRevamp.SystemAllPositiveNegative.splice(indexAllsysPostNeg[0], 1);
                    }
                    for (var i = 0; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                        if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == SystemID) {
                            ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;

                        }

                    }
                }
            }, function () {
            }, 'Reset Confirmation');

        }
    },
    clearInfoForSystemReset: function (obj, SystemID, SystemName) {
        $(obj).closest("li").removeClass('green');
        // by Zia Mehmood
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').css('color', 'black');
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('i.fa-book').removeClass('green');
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('span.green').removeClass('green');
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('div[id^=divDetail] ').find('textarea').val('');
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').attr('value', '')
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam');
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam div.toggle').addClass('disableAll');
        // End Code By Zia
        //$('#' + ROSTemplateDetailRevamp.params.PanelID + ' input[id^=Charc_Pos_' + SystemID + '],#' + ROSTemplateDetailRevamp.params.PanelID + ' input[id^=Charc_Neg_' + SystemID + '],' + '#' + ROSTemplateDetailRevamp.params.PanelID +
        //    ' input[id=selectAllNegative_' + SystemID + '],#' + ROSTemplateDetailRevamp.params.PanelID + 'input[id=selectAllPositive_' + SystemID + ']').prop('checked', false);
        //Clinical_ReviewofSystems.bookIconClassToggle($('#' + ROSTemplateDetailRevamp.params.PanelID + ' input[id^=Charc_Pos_' + SystemID + ']').closest('ul').find('[id=bookIcon]'), true);

        //$('#' + ROSTemplateDetailRevamp.params.PanelID + ' [id^=Charc_Pos_').parent().parent().removeClass('red');
        //$('#' + ROSTemplateDetailRevamp.params.PanelID + ' input[id^=Charc_Desc_]').val('');
        // $('#' + ROSTemplateDetailRevamp.params.PanelID + ' input[id^=selectAllPositive_]').prop('checked', false).parent().parent().removeClass('red')
        $(obj).parent().find('span:first-child').removeClass('green');
        var detailObj = $.grep(ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
            return (item.SystemId == SystemID);
        });
        ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.splice(detailObj, 1);
        ROSTemplateDetailRevamp.CharcSystemInfo[SystemName] = null;
        $(obj).closest("li").find("#isSystemNormal").val('');
        $(obj).closest("li").find("#systemNormalDescription").val('');

    },
    toggleDetailsDiv: function (liId, isReplace) {
        var objDeffered = $.Deferred();
        var sectionDetails = "";
        var self = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #divExamCharacteristics').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
            ROSTemplateDetailRevamp.resetControlValue($(this));
        });
        objDeffered.resolve();
        return objDeffered;
    },
    removeItem: function (ROSSystemId, control, ROSCharacteristicsId) {
        if (control == "system") {
            $("#ulPhysicalExamSystems #" + ROSSystemId).remove();

            if (ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 1;
                        ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                    }
                }
            }

        }
        else if (control == "observation") {
            $("#ulPhysicalExamSystemSection #" + ROSCharacteristicsId).remove();

            if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId && ROSTemplateDetailRevamp.selectedObservations[i].ObservationId == ROSCharacteristicsId) {
                ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 1;
                ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
            }
        }


    },
    resetControlValue: function (obj) {
        var currentElementTagName = obj.tagName != null ? obj.tagName : obj.prop("tagName");
        if ($(obj).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea' || $(obj).attr('type') == 'hidden')
            $(obj).val('');
        if ($(obj).attr('type') == 'checkbox' || $(obj).attr('type') == 'radio') {

            if ($(obj).attr('type') == 'radio') {
                obj.checked = false;
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
                var groupRadBtn = $("input[name='" + $(obj).attr('name') + "']");
                if (groupRadBtn.length > 1) {
                    $.each(groupRadBtn, function (i, item) {
                        if ($(item).attr("id").toLowerCase().indexOf("no") > -1) {
                            $(item).trigger("click");
                        }
                    });
                }
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
            }
            else {
                obj.checked = false;
            }
        }

        if (currentElementTagName.toLowerCase() == 'select') {
            $(obj).find('option:selected').removeAttr('selected');
            //$(this).attr('selectedIndex', '-1');
            $(obj).find('option:eq(0)').attr('selected', 'selected');
        }
        if (currentElementTagName.toLowerCase() == 'ul') {
            $(obj).find('li.active').removeClass('active');
        }
    },

    removeSystem: function (ROSSystemId, event) {
        $("#ulPhysicalExamSystems #" + ROSSystemId).remove();
        $("#ulPhysicalExamSystemSection").hide();
        if (ROSTemplateDetailRevamp.selectedObservations) {
            for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                    ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 1;
                    ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 1;
                    ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                    ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = false;
                    ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                }
            }
        }

        indexIsSystemExist = $.map(ROSTemplateDetailRevamp.IsNornalAllSystems, function (item, index) {
            if (item.Id == ROSSystemId) {
                return index;
            }
        })
        if (indexIsSystemExist.length > 0) {

            ROSTemplateDetailRevamp.IsNornalAllSystems.splice(indexIsSystemExist[0], 1);
        }

        indexPosNeg = $.map(ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
            if (item.SystemId == ROSSystemId) {
                // ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg[i-1], 1);
                return index;
            }
        })
        if (indexPosNeg.length > 0) {
            // ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg,1 );
            for (var i = indexPosNeg.length; i > 0 ; i--) {

                ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg[i - 1], 1);
            }
        }
        indexAllsysPostNeg = $.map(ROSTemplateDetailRevamp.SystemAllPositiveNegative, function (item, index) {
            if (item.SystemId == ROSSystemId) {
                return index;
            }
        })
        if (indexAllsysPostNeg.length > 0) {

            ROSTemplateDetailRevamp.SystemAllPositiveNegative.splice(indexAllsysPostNeg[0], 1);
        }
        for (var i = 0; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
            if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;

            }

        }


        $("#observationContent div").remove();
        event.stopPropagation();
    },

    removeCharateristics: function (ROSSystemId, ROSCharacteristicsId) {
        $("#ulPhysicalExamSystemSection #" + ROSCharacteristicsId).remove();
        $("#observationContent #divSystem" + ROSSystemId + ROSCharacteristicsId).remove();
        ROSTemplateDetailRevamp.removeLastDelimiter(ROSSystemId);

        if (ROSTemplateDetailRevamp.selectedObservations) {
            for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId && ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId == ROSCharacteristicsId) {
                    ROSTemplateDetailRevamp.selectedObservations.splice([i], 1);
                    //ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 1;
                    //ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                    //ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                }
            }
        }
    },

    loadCharatristics: function (ROSSystemId) {
        $("#ulPhysicalExamSystemSection").show();
        var isExist = false;

        if (ROSTemplateDetailRevamp.selectedObservations) {
            for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                    isExist = true;
                    break;
                }
            }
        }

        if (!$('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam div.toggle').hasClass('disableAll')) {
            ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(true);
        }
        $('#frmROSTemplateDetailRevamp  #divAddNewSubSystem').removeClass('disableAll');
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #txtCharacteristic').removeClass('disableAll').parent('span').removeClass('disableAll');
        if (!isExist) {
            ROSTemplateDetailRevamp.ROSCharatristicsLoad(ROSSystemId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.ObservationCount > 0) {
                        var resSystems = JSON.parse(response.PEObservation_JSON);

                        $("#SystemSections").removeClass('hidden');
                        $("#observationContent div").hide();
                        //$('#observationContent div[id^=divSystem]').hide();
                        $("#ulPhysicalExamSystemSection li").remove();

                        $("#ulPhysicalExamSystems li").removeClass('active');
                        $("#ulPhysicalExamSystems li[id=" + ROSSystemId + "]").addClass('active');

                        //var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + ROSSystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                        //    + 'onclick="ROSTemplateDetailRevamp.selectAllChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';


                        //var IsNormalAll = '<li id="IsNormalAll" parentid="' + ROSSystemId + '" value="IsNormalAll" refvalue="IsNormalAll"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="IsNormalAllChar" name="IsNormal" class="pull-left" ' +
                        //    'onclick="ROSTemplateDetailRevamp.NormalAllChars(this);">' +
                        //   '<button type="button" id="btnOpenDetailIsNormalAll"  data-toggle="tooltip" data-placement="top"  onclick="ROSTemplateDetailRevamp.openCharacteristicDetail(this);" class="btn btn-link btn-xs pull-left"><i class="fa fa-book"></i></button>' +
                        //   '<label id="lblIsNormalAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Normal</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        //$("#ulPhysicalExamSystemSection").append(IsNormalAll);
                        //$("#ulPhysicalExamSystemSection").append(selectAll);
                        var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + ROSSystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                     + 'onclick="ROSTemplateDetailRevamp.selectAllChars(this);">' +
                     '<label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">' +
                        '<span><b><a id="NegativeAll" href="javascript:void(0);" onclick="ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,false);"  value="" class="" style="margin-right:5px;color:black"> (-) </a></b></span>' +
                     '<span><b><a id="PositiveAll"  href="javascript:void(0);" onclick="ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,true);"  value="" class="" style="margin-right:5px;color:black"> (+) </a></b></span>' +
                        'Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        var IsNormalAll = '<li id="IsNormalAll" parentid="' + ROSSystemId + '" value="IsNormalAll" refvalue="IsNormalAll"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="IsNormalAllChar" name="IsNormal" class="pull-left " ' +
                                     'onclick="ROSTemplateDetailRevamp.NormalAllChars(this);">' +

                                    '<label id="lblIsNormalAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">' +
                                   '<span id="btnOpenDetailIsNormalAll"  data-toggle="tooltip" data-placement="top"  onclick="ROSTemplateDetailRevamp.isNormalComments(this);" class="btn btn-link btn-xs pull-left disableAll" style="margin-top: -3px;"><i class="fa fa-book"></i></span>' +
                                    'Normal</label><div class="clearfix"></div><div class="clearfix"></div></div>' +
                                  // '<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="ROSTemplateDetailRevamp.validateMaxLengthandCaching(this)" onkeypress="PhysicalExamDataTemplateDetail.saveDetailComments(event,this)"></textarea><div class="clearfix"></div></li>';
                        '<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="" ></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail" onclick="ROSTemplateDetailRevamp.validateMaxLengthandCaching(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a><div class="clearfix"></div></li>';
                        $("#ulPhysicalExamSystemSection").append(IsNormalAll);
                        $("#ulPhysicalExamSystemSection").append(selectAll);

                        $.each(resSystems, function (i, item) {
                            var li = ROSTemplateDetailRevamp.addCharatristics(item.ROSCharacteristicsId, item.Name, ROSSystemId);
                            $("#ulPhysicalExamSystemSection").append(li);


                            var objSelectedObservations = {
                                ROSSystemId: ROSSystemId, IsChecked: false, ROSCharacteristicsId: item.ROSCharacteristicsId, ROSCharacteristicsName: item.Name, IsSystemChecked: false, SystemDescription: "", IsSystemDeleted: 0, IsObservationDeleted: 0
                            };
                            ROSTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);

                        });
                    }
                    else {
                        // $('#observationContent div[id^=divSystem]').hide();
                        $("#ulPhysicalExamSystemSection li").remove();

                        $("#ulPhysicalExamSystems li").removeClass('active');
                        $("#ulPhysicalExamSystems li[id=" + ROSSystemId + "]").addClass('active');
                        var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + ROSSystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                                         + 'onclick="ROSTemplateDetailRevamp.selectAllChars(this);">' +
                                         '<label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">' +
                                            '<span><b><a id="NegativeAll" href="javascript:void(0);" onclick="ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,false);"  value="" class="" style="margin-right:5px;color:black"> (-) </a></b></span>' +
                                         '<span><b><a id="PositiveAll"  href="javascript:void(0);" onclick="ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,true);"  value="" class="" style="margin-right:5px;color:black"> (+) </a></b></span>' +
                                            'Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        var IsNormalAll = '<li id="IsNormalAll" parentid="' + ROSSystemId + '" value="IsNormalAll" refvalue="IsNormalAll"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="IsNormalAllChar" name="IsNormal" class="pull-left " ' +
                                     'onclick="ROSTemplateDetailRevamp.NormalAllChars(this);">' +

                                    '<label id="lblIsNormalAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">' +
                                   '<span id="btnOpenDetailIsNormalAll"  data-toggle="tooltip" data-placement="top"  onclick="ROSTemplateDetailRevamp.isNormalComments(this);" class="btn btn-link btn-xs pull-left disableAll" style="margin-top: -3px;"><i class="fa fa-book"></i></span>' +
                                    'Normal</label><div class="clearfix"></div><div class="clearfix"></div></div>' +
                                  // '<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="ROSTemplateDetailRevamp.validateMaxLengthandCaching(this)" onkeypress="PhysicalExamDataTemplateDetail.saveDetailComments(event,this)"></textarea><div class="clearfix"></div></li>';
                        '<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="" ></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail" onclick="ROSTemplateDetailRevamp.validateMaxLengthandCaching(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a><div class="clearfix"></div></li>';
                        $("#ulPhysicalExamSystemSection").append(IsNormalAll);
                        $("#ulPhysicalExamSystemSection").append(selectAll);
                    }
                }
            });
        }
        else {
            $("#SystemSections").removeClass('hidden');
            $("#ulPhysicalExamSystemSection li").remove();

            var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + ROSSystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                        + 'onclick="ROSTemplateDetailRevamp.selectAllChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">' +
                                       '<span><b><a id="NegativeAll" href="javascript:void(0);" onclick="ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,false);"  value="" class="" style="margin-right:5px;color:black"> (-) </a></b></span>' +
                     '<span><b><a id="PositiveAll"  href="javascript:void(0);" onclick="ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,true);"  value="" class="" style="margin-right:5px;color:black"> (+) </a></b></span>' +
                        'Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
            var IsNormalAll = '<li id="IsNormalAll" parentid="' + ROSSystemId + '" value="IsNormalAll" refvalue="IsNormalAll"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="IsNormalAllChar" name="IsNormal" class="pull-left disableAll" ' +
                         'onclick="ROSTemplateDetailRevamp.NormalAllChars(this);">' +

                        '<label id="lblIsNormalAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">' +
                        '<span id="btnOpenDetailIsNormalAll"  data-toggle="tooltip" data-placement="top"  onclick="ROSTemplateDetailRevamp.isNormalComments(this);" class="btn btn-link btn-xs pull-left disableAll " style="margin-top: -3px;"> <i class="fa fa-book"></i></span>' +
                        'Normal</label><div class="clearfix"></div><div class="clearfix"></div></div>' +
                        //'<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="ROSTemplateDetailRevamp.validateMaxLengthandCaching(this)" onkeypress="PhysicalExamDataTemplateDetail.saveDetailComments(event,this)"></textarea><div class="clearfix"></div></li>';
                         '<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="" ></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail" onclick="ROSTemplateDetailRevamp.validateMaxLengthandCaching(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a><div class="clearfix"></div></li>';
            $("#ulPhysicalExamSystemSection").append(IsNormalAll);
            $("#ulPhysicalExamSystemSection").append(selectAll);

            if (ROSTemplateDetailRevamp.selectedObservations) {
                //$('#observationContent div[id^=divSystem]').hide();
                $("#observationContent").text('');
                for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        var li = ROSTemplateDetailRevamp.addCharatristics(ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId, ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsName, ROSSystemId);

                        if (ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId != "")
                            $("#ulPhysicalExamSystemSection").append(li);

                        $("#ulPhysicalExamSystems li").removeClass('active');
                        $("#ulPhysicalExamSystems li[id=" + ROSSystemId + "]").addClass('active');

                        if (ROSTemplateDetailRevamp.selectedObservations[i].IsChecked) {

                            if (ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted == 0) {
                                $("#ulPhysicalExamSystemSection #chk" + ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId).prop("checked", true);
                                $('#observationContent #divSystem' + ROSSystemId + ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId).show();
                                ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;
                                ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 0;
                                ROSTemplateDetailRevamp.handleDelimiter(ROSSystemId, ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId, ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsName, true);
                            }
                            else {
                                $("#ulPhysicalExamSystemSection #chk" + ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId).prop("checked", false);
                                ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;
                                ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 0;
                                ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                            }
                        }
                        else {
                            $("#ulPhysicalExamSystemSection #chk" + ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId).prop("checked", false);
                            // ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;
                            ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 0;
                            ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        }
                    }
                }
                for (var i = 0 ; i < ROSTemplateDetailRevamp.PositiveNegativeDetail.length; i++) {
                    if (ROSTemplateDetailRevamp.PositiveNegativeDetail[i].SystemId == ROSSystemId) {
                        var charid = ROSTemplateDetailRevamp.PositiveNegativeDetail[i].CharcId;
                        if (ROSTemplateDetailRevamp.PositiveNegativeDetail[i].IsPositive == true) {
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #SystemSections #divPhysicalExamSystemSection ul li[id=' + charid + ']').find('#Positive').css('color', 'green');
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #SystemSections #divPhysicalExamSystemSection ul li[id=' + charid + ']').find('#Positive').attr('value', true)
                        }
                        if (ROSTemplateDetailRevamp.PositiveNegativeDetail[i].IsNegative == true) {
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #SystemSections #divPhysicalExamSystemSection ul li[id=' + charid + ']').find('#Negative').css('color', 'red');
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #SystemSections #divPhysicalExamSystemSection ul li[id=' + charid + ']').find('#Negative').attr('value', true)
                        }
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #SystemSections #divPhysicalExamSystemSection ul li[id=' + charid + ']').find('div[id^=divDetail]').find('textarea').val(ROSTemplateDetailRevamp.PositiveNegativeDetail[i].CharcComments);
                    }
                }
            }
            indexIsSystemExist = $.map(ROSTemplateDetailRevamp.IsNornalAllSystems, function (item, index) {
                if (item.Id == $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystems ul li.active').attr('id')) {
                    return index;
                }
            })
            if (indexIsSystemExist.length > 0) {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="IsNormalAll"]').find('#IsNormalAllChar').attr("checked", true);
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li:not([id="IsNormalAll"])').addClass('disableAll');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').css('color', 'black');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('i.fa-book').removeClass('green');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('span.green').removeClass('green');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('div[id^=divDetail] ').find('textarea').val('');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').attr('value', '')
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam div.toggle').addClass('disableAll');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#btnOpenDetailIsNormalAll').removeClass('disableAll');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystems ul li.active').find('[type="checkbox"]').addClass('disableAll');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #txtCharacteristic').addClass('disableAll');
                $('#frmROSTemplateDetailRevamp  #divAddNewSubSystem').addClass('disableAll');
                if (ROSTemplateDetailRevamp.IsNornalAllSystems[indexIsSystemExist[0]].IsNormalComments) {
                    $('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #btnOpenDetailIsNormalAll').addClass('green');
                }
                else {
                    $('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #btnOpenDetailIsNormalAll').removeClass('green');
                }
            }
            var sysid = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').attr('Id')
            indexes = $.map(ROSTemplateDetailRevamp.SystemAllPositiveNegative, function (item, index) {
                if (item.SystemId == sysid) {
                    return index;

                }
            });
            if (indexes.length > 0) {
                var indexs = indexes[0];
                var IsPositiveind = ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IspositiveAll;
                var IsNegativeind = ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IsnegativeAll;
                if (IsNegativeind == true) {
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').addClass('red');
                }
                else if (IsPositiveind == true) {
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').addClass('green');

                }
                else {
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                }
            }

            ROSTemplateDetailRevamp.ChangeColorPostiveNegativeAll();

            // for Select All
            if ($('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').length == $('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find(':checkbox:checked').length) {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', true);
            }
            else {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', false);
            }
            // for load only active charateristics
            for (var i = 0; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == sysid && ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted == 1) {
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId + '"]').remove();

                }

                indexesforcharposNeg = $.map(ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                    if (item.CharcId == ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                        return index;
                    }
                })

                if (indexesforcharposNeg.length > 0) {
                    var detialIndexForPostitiveNegative = indexesforcharposNeg[0];
                    if (ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].CharcComments) {
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId + '"][parentid="' + ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId + '"]').find('i.fa-book').addClass('green');
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId + '"][parentid="' + ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId + '"]').find('i.fa-book').removeClass('blue');

                    }
                    else {
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId + '"][parentid="' + ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId + '"]').find('i.fa-book').addClass('bule');
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId + '"][parentid="' + ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId + '"]').find('i.fa-book').removeClass('green');
                    }



                }


            }

        }
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#IsNormalAllChar').removeClass('disableAll');
    },
    validateMaxLengthandCaching: function (obj) {
        var SystemID = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').attr('id');
        indexes = $.map(ROSTemplateDetailRevamp.IsNornalAllSystems, function (obj, index) {
            if (obj.SystemId == SystemID) {
                return index;
            }
        })
        var CharPosNegDetailIndex = indexes[0];
        ROSTemplateDetailRevamp.IsNornalAllSystems[CharPosNegDetailIndex].IsNormalComments = $(obj).parents('li').find('textarea').val();;
        $(obj).parents('#divDetailIsNormal').addClass('hidden');
        if ($(obj).parents('li').find('textarea').val()) {
            $('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #btnOpenDetailIsNormalAll').addClass('green');
        }
        else {
            $('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #btnOpenDetailIsNormalAll').removeClass('green');
        }
    },

    ManageCharateristics: function (ROSSystemId, obj) {
        $("#ulPhysicalExamSystems li").removeClass('active');
        $("#ulPhysicalExamSystems li[id=" + ROSSystemId + "]").addClass('active');

        if (!$(obj).is(':checked')) {
            $('#frmROSTemplateDetailRevamp #divPhysicalExamSystemSection  #chkboxSelectAllObservations :input').prop("checked", false);
            $(obj).parent().parent().removeClass('active');

            for (var i = 0; i < $("#ulPhysicalExamSystemSection li").length; i++) {
                $("#ulPhysicalExamSystemSection #chk" + $("#ulPhysicalExamSystemSection li")[i].id).prop('checked', false);
            }
            if (ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = false;
                        ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                        //break;
                    }
                }

            }
        }
        else {

            if (ROSTemplateDetailRevamp.selectedObservations) {
                setTimeout(function () { $('#frmROSTemplateDetailRevamp #divPhysicalExamSystemSection  #chkboxSelectAllObservations :input').prop("checked", true); }, 10);


                for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = true;
                        ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                        //break;
                    }
                }

            }
        }
    },

    checkUncheckPositiveNegativeAll: function (obj, Ispositive)  // cheeta
    {
        var IsPositiveind = false; //
        var IsNegativeind = false;
        var sysid = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').attr('Id')
        if (Ispositive == true) {
            IsNegativeind = false;
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Negative').css('color', 'black');
            if ($(obj).hasClass('green') == true) {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').removeClass('green');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Positive').css('color', 'black');
                $(obj).removeClass('green');
                IsPositiveind = false;
            }
            else {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').addClass('green');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Positive').css('color', 'green');
                $(obj).addClass('green');
                IsPositiveind = true;
            }

        }
        else {
            IsPositiveind = false;
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Positive').css('color', 'black');
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
            if ($(obj).hasClass('red') == true) {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').removeClass('green');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Negative').css('color', 'black');
                $(obj).removeClass('red');
                IsNegativeind = false;
            }
            else {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').addClass('green');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Negative').css('color', 'red');
                $(obj).addClass('red');
                IsNegativeind = true;
            }
        }
        //SystemAllPositiveNegative: [],
        // var sysid = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').attr('Id')
        indexes = $.map(ROSTemplateDetailRevamp.SystemAllPositiveNegative, function (item, index) {
            if (item.SystemId == sysid) {
                return index;

            }
        });
        if (indexes.length > 0) {
            var indexs = indexes[0];
            ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IspositiveAll = IsPositiveind;
            ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IsnegativeAll = IsNegativeind;
        }
        else {
            var IsPosNegAll =
                {
                    SystemId: sysid,
                    IspositiveAll: IsPositiveind,
                    IsnegativeAll: IsNegativeind
                };
            ROSTemplateDetailRevamp.SystemAllPositiveNegative.push(IsPosNegAll);
        }


        for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
            if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == sysid) {
                indexesforcharposNeg = $.map(ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                    if (item.CharcId == ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                        return index;

                    }
                })

                if (indexesforcharposNeg.length > 0) {
                    var detialIndexForPostitiveNegative = indexesforcharposNeg[0];
                    ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsPositive = IsPositiveind;
                    ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsNegative = IsNegativeind;
                    // ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].CharcComments = "";
                }
                else {
                    var CharsPosNegDetail = {
                        Id: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId + '-' + ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                        SystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                        CharcId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                        CharcComments: "",
                        IsPositive: IsPositiveind,
                        IsNegative: IsNegativeind

                    };
                    ROSTemplateDetailRevamp.PositiveNegativeDetail.push(CharsPosNegDetail);
                }
            }

            ROSTemplateDetailRevamp.loadCharatristics(sysid);

        }


    },
    selectAllChars: function (obj) {
        $('#observationContent div[id^=divSystem]').remove();
        if (obj) var sysId = $($($(obj).parent().parent())[0]).attr('parentid');
        if ($(obj).prop('checked') == true) {
            $("#SystemPreview").removeClass('hidden');
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);

                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3)
                    var observationName = $("#divPhysicalExamSystemSection #lblName" + id_).text();
                    var delimator = $("#delimator option:selected").text() + " ";

                    if ($("#observationContent #divSystem" + sysId + id_).length > 0) {
                        $('#observationContent #divSystem' + sysId + id_).remove();
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSystem" + sysId + id_);
                        $newDiv.attr("style", "display: inline;");

                        $("#observationContent").append($newDiv);
                        $('#observationContent #divSystem' + sysId + id_).show();
                        var txttoAppend = observationName;
                        if ($('#observationContent div').length > 1)
                            txttoAppend = delimator + observationName;

                        $("#observationContent #divSystem" + sysId + id_).append(txttoAppend);
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSystem" + sysId + id_);
                        $newDiv.attr("style", "display: inline;");

                        $("#observationContent").append($newDiv);
                        $('#observationContent #divSystem' + sysId + id_).show();

                        var txttoAppend = observationName;
                        if ($('#observationContent div').length > 1)
                            txttoAppend = delimator + observationName;

                        $("#observationContent #divSystem" + sysId + id_).append(txttoAppend);
                    }

                    $("#ulPhysicalExamSystems #chk" + sysId).prop("checked", true);
                }
            });

            if (ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == sysId) {
                        ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = true;
                        ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + sysId).text();
                    }
                }
            }
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').removeClass("green");
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                    var system_id = $($($(obj).parent().parent())[0]).attr('parentid');
                    $("#observationContent #divSystem" + system_id + id_).remove();
                }
                $("#ulPhysicalExamSystems #chk" + sysId).prop("checked", false);
            });

            if (ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == sysId) {
                        ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = false;
                        ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + sysId).text();
                        //$('#observationContent div[id^=divSystem' + sysId + ']').hide();
                    }
                }
            }
        }
        ROSTemplateDetailRevamp.removeLastDelimiter(sysId);
    },

    applyStyle: function (style) {
        if (style == 'bold') {
            $("#observationContent").css('font-weight', 'bold');
        }
        else if (style == 'italic') {
            $("#observationContent").css('font-style', 'italic');
        }
        else if (style == 'underline') {
            $("#observationContent").css('text-decoration', 'underline');
        }
        else if (style == 'reset') {
            $("#observationContent").attr("style", " ");
        }
        else if (style == 'clear') {
            $("#observationContent").text('');
        }
    },
    isNormalComments: function (obj) {
        if ($(obj).parents('li').find('#divDetailIsNormal').hasClass('hidden') == true) {
            $(obj).parents('li').find('#divDetailIsNormal').removeClass('hidden');
            $(obj).parents('li').find('#divDetailIsNormal').find('textarea').focus();
            var SystemID = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').attr('id');
            indexes = $.map(ROSTemplateDetailRevamp.IsNornalAllSystems, function (obj, index) {
                if (obj.SystemId == SystemID) {
                    return index;
                }
            })

            if (indexes.length > 0) {
                var index = indexes[0];
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #txtCharacteristicDetailDetailIsNormal').val(ROSTemplateDetailRevamp.IsNornalAllSystems[index].IsNormalComments);
            }
        }
        else {

            $(obj).parents('li').find('#divDetailIsNormal').addClass('hidden');
        }

    },
    NormalAllChars: function (obj) {
        $(obj).parents('li').find('#divDetailIsNormal').addClass('hidden');
        var SystemID = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').attr('id');
        if ($(obj).prop('checked') == true) {
            var SystemName = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #System ul li.active').find('label').text().split(" ( + ) ").join("");

            var message = 'This will reset all values in ' + SystemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?';
            utility.myConfirm(message, function () {
                // Caching info

                var detailIsNormal = $.grep(ROSTemplateDetailRevamp.IsNornalAllSystems, function (item, index) {
                    return (item.SystemId == SystemID);
                });




                if (detailIsNormal.length == 0) {
                    var SystemIsNormal = {
                        Id: SystemID,
                        SystemId: SystemID,
                        IsNormalComments: '',
                        IsNormal: true,


                    };
                    ROSTemplateDetailRevamp.IsNornalAllSystems.push(SystemIsNormal);



                }
                //indexes = $.map(ROSTemplateDetailRevamp.IsNornalAllSystems, function (obj, index) {
                //    if (obj.SystemId == SystemID) {
                //        return index;
                //    }
                //})
                //var CharPosNegDetailIndex = indexes[0];
                //ROSTemplateDetailRevamp.IsNornalAllSystems[CharPosNegDetailIndex]=


                //ROSTemplateDetailRevamp.clearInfoForSystemReset(obj, SystemID, SystemName);
                // var sysid = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').attr('Id')
                indexes = $.map(ROSTemplateDetailRevamp.SystemAllPositiveNegative, function (item, index) {
                    if (item.SystemId == SystemID) {
                        return index;

                    }
                });
                if (indexes.length > 0) {
                    var indexs = indexes[0];
                    ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IspositiveAll = false;
                    ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IsnegativeAll = false;



                }
                $('#frmROSTemplateDetailRevamp  #divAddNewSubSystem').addClass('disableAll');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #txtCharacteristic').addClass('disableAll');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
                $(obj).parents('li').siblings().addClass('disableAll');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').css('color', 'black');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('i.fa-book').removeClass('green');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('span.green').removeClass('green');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('div[id^=divDetail] ').find('textarea').val('');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').attr('value', '')
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam div.toggle').addClass('disableAll');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#btnOpenDetailIsNormalAll').removeClass('disableAll');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').find('#btnOpenDetailIsNormalAll').removeClass('disableAll');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').find('input').addClass('disableAll');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').removeClass('green');

                // remove caching

                for (var i = 0; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {


                    indexes1 = $.map(ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
                        if (item.SystemId == SystemID && item.CharcId == ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId) {
                            return index;

                        }
                    });
                    if (indexes1.length > 0) {
                        var indexs1 = indexes1[0];
                        ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.splice(indexs1, 1);



                    }


                    if (SystemID == ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                        indexesforcharposNeg = $.map(ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                            if (item.CharcId == ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                                return index;
                            }
                        })

                        if (indexesforcharposNeg.length > 0) {
                            var detialIndexForPostitiveNegative = indexesforcharposNeg[0];
                            ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].CharcComments = '';
                            ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsNegative = false;
                            ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsPositive = false;
                        }
                    }
                }
            }, function () {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="IsNormalAll"]').find('#IsNormalAllChar').attr("checked", false);
            }, 'Reset Confirmation');


        }
        else {
            indexes = $.map(ROSTemplateDetailRevamp.IsNornalAllSystems, function (obj, index) {
                if (obj.SystemId == SystemID) {
                    return index;
                }
            })
            var CharPosNegDetailIndex = indexes[0];
            ROSTemplateDetailRevamp.IsNornalAllSystems.splice(CharPosNegDetailIndex, 1);
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #txtCharacteristic').removeClass('disableAll').parent('span').removeClass('disableAll');
            $('#frmROSTemplateDetailRevamp  #divAddNewSubSystem').removeClass('disableAll');
            $(obj).parents('li').siblings().removeClass('disableAll');
            $(obj).parents('li').find('#btnOpenDetailIsNormalAll').addClass('disableAll');
            $(obj).parents('li').find('#IsNormalAllChar').removeClass('disableAll');
            $('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #btnOpenDetailIsNormalAll').removeClass('green');
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').find('input').removeClass('disableAll');
            var sysid = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').attr('Id')
            indexPosNeg = $.map(ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                if (item.SystemId == sysid) {
                    return index;
                }
            })
            if (indexPosNeg.length > 0) {
                for (var i = indexPosNeg.length; i > 0 ; i--) {

                    ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg[i - 1], 1);
                }
            }

        }

    },
    loadSysPatCharcDetail: function (cntrl, isCharcDetailExists) {
        if (!$('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam').find('.toggle').hasClass('disableAll')) {
            ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(true);
        }
        ROSTemplateDetailRevamp.addActiveClass(cntrl);
        EMRUtility.resetControlValue($('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #divExamCharacteristics'));
        //ROSTemplateDetailRevamp.validateDurationDetails();
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #headingDetail').html($(cntrl).text() + ' Detail');
        var charcDetailId = '-1';// $(cntrl).find("input[type='hidden']:eq(1)").attr("id").split('_')[1];
        var detail = $.grep(ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
            return item.Id == cntrl.id;
        });
        if (isCharcDetailExists != null && isCharcDetailExists != '' && (detail == null || detail.length == 0)) {
            ROSTemplateDetailRevamp.getSystemPatientCharcDetail_DBCall(charcDetailId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var detail = JSON.parse(response.SystemCharacteristicsDetails_JSON);
                    ROSTemplateDetailRevamp.populateDetailAgainstCharacteristic(detail[0]);
                    ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(false);
                }
            });
        } else {
            if ((detail != null && detail.length != 0)) {
                ROSTemplateDetailRevamp.bindCacheCharacteristicDetailInfo(cntrl);
                ROSTemplateDetailRevamp.validateDurationDetails();
                //      }
            }
        }
    },
    CacheCharacteristicDetailInfo: function (resetCache) {
        if ($('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp  #ulPhysicalExamSystemSection > li.active").length > 0) {
            var systemid = $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp  #ulPhysicalExamSystemSection > li.active").attr('parentid');
            var charcId = $('#' + ROSTemplateDetailRevamp.params.PanelID + " #ulPhysicalExamSystemSection > li.active").attr('id');
            var charcDetailDiv = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics');
            var DetailData = unescape($(charcDetailDiv).getMyJSON());//kr escape.

            var detailDataParsed = JSON.parse(DetailData);
            if (detailDataParsed.Duration == '' || detailDataParsed.ROSCharacteristicsDetailDurationId_text == "- Select -") {
                detailDataParsed.Duration = '';
                detailDataParsed.ROSCharacteristicsDetailDurationId_text = '';
            }
            DetailData = JSON.stringify(detailDataParsed);

            if (resetCache != null && resetCache == true) {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics').resetAllControls();
            }
            ROSTemplateDetailRevamp.CharcDetailDivJSON = unescape($('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics').clone().getMyJSON());//kr escape.

            ROSTemplateDetailRevamp.CharcDetailDivJSON = unescape($('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics').clone().getMyJSON());//kr escape.

            var indexCh = -1;

            if (DetailData != ROSTemplateDetailRevamp.CharcDetailDivJSON) {

                $.grep(ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
                    if (item.Id == charcId && item.SystemId == systemid) {
                        indexCh = index;
                        return;
                    }
                });

                if (indexCh != -1) {
                    ROSTemplateDetailRevamp.CharacteristicsDetailsInfo[indexCh].DetailInfo = DetailData;
                }
                else {
                    var Ids = charcId.split('_');

                    var CharsDetailInfo = {
                        Id: charcId.split(' ').join(''),
                        SystemId: systemid,
                        CharcId: Ids.length > 0 ? Ids[2] : null,
                        DetailInfo: DetailData
                    };
                    ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.push(CharsDetailInfo);
                }
            }
        }
    },
    bindCacheCharacteristicDetailInfo: function (cntrl) {
        var charcId = cntrl.id;
        var detail = $.grep(ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
            return item.Id == charcId;
        });
        if ((detail != null && detail.length != 0)) {

            var filteredDetails = $.grep(detail, function (v) {
                return v.SystemId === $(cntrl).attr('parentid');
            });
            if (filteredDetails != null && filteredDetails.length != 0)
                detailJson = JSON.parse(filteredDetails[0].DetailInfo);

            ROSTemplateDetailRevamp.populateDetailAgainstCharacteristic(detailJson);
        }

    },
    populateDetailAgainstCharacteristic: function (detail) {
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #PreviousHistory').val(detail.PreviousHistory);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ROSCharacteristicsDetailStatusId').val(detail.ROSCharacteristicsDetailStatusId);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #Onset').val(detail.Onset);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #Duration').val(detail.Duration);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ROSCharacteristicsDetailDurationId').val(detail.ROSCharacteristicsDetailDurationId);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ROSCharacteristicsDetailPatternId').val(detail.ROSCharacteristicsDetailPatternId);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ROSCharacteristicsDetailSeverityId').val(detail.ROSCharacteristicsDetailSeverityId);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ROSCharacteristicsDetailCourseId').val(detail.ROSCharacteristicsDetailCourseId);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ROSCharacteristicsDetailRadiationId').val(detail.ROSCharacteristicsDetailRadiationId);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ROSCharacteristicsDetailFrequencyId').val(detail.ROSCharacteristicsDetailFrequencyId);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ROSCharacteristicsDetailContextId').val(detail.ROSCharacteristicsDetailContextId);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ROSCharacteristicsDetailCharacterCSZId').val(detail.ROSCharacteristicsDetailCharacterCSZId);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ROSCharacteristicsDetailAggravedById').val(detail.ROSCharacteristicsDetailAggravedById);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ROSCharacteristicsDetailRelievedById').val(detail.ROSCharacteristicsDetailRelievedById);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #Location').val(detail.Location);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #PrecipitatedBY').val(detail.PrecipitatedBY);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #AssociatedWith').val(detail.AssociatedWith);
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #hfROSCharacteristicsDetailsId').val(detail.ROSCharacteristicsDetailsId);
        ROSTemplateDetailRevamp.validateDurationDetails();
    },
    PreviewCharateristics: function (ROSCharacteristicsId, ROSCharacteristicsName, obj, ROSSystemId) {
        //if ($('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('[onClick^="ROSTemplateDetailRevamp.PreviewCharateristics"]').prop('checked') == true)
        //{
        //    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', true);
        //}
        $("#SystemPreview").removeClass('hidden');

        if ($(obj).prop('checked') == true) {
            $(this).find("[type='checkbox'][id*='chk']").prop("checked", true);
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').removeClass("green");
            $(this).find("[type='checkbox'][id*='chk']").prop("checked", false);
        }

        var isChk = $(obj).prop('checked') == true ? true : false;

        var objSelectedObservations =
        {
            ROSSystemId: ROSSystemId,
            IsChecked: isChk,
            ROSCharacteristicsId: ROSCharacteristicsId,
            ROSCharacteristicsName: ROSCharacteristicsName,
            IsModified: '1',
            IsSystemChecked: false,
            SystemDescription: ""
        };


        if ($(obj).prop('checked') == true) {
            var deli = $("#delimator option:selected").text() + " ";

            //$('#observationContent div[id^=divSystem]').remove();
            if ($("#observationContent #divSystem" + ROSSystemId + ROSCharacteristicsId).length > 0) {
                $('#observationContent #divSystem' + ROSSystemId + ROSCharacteristicsId).remove();
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + ROSSystemId + ROSCharacteristicsId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + ROSSystemId + ROSCharacteristicsId).show();
                var txttoAppend = ROSCharacteristicsName;
                if ($('#observationContent div').length > 1)
                    txttoAppend = deli + ROSCharacteristicsName;

                $("#observationContent #divSystem" + ROSSystemId + ROSCharacteristicsId).append(txttoAppend);
            }
            else {
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + ROSSystemId + ROSCharacteristicsId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + ROSSystemId + ROSCharacteristicsId).show();
                var txttoAppend = ROSCharacteristicsName;

                if ($('#observationContent div').length > 1)
                    txttoAppend = deli + ROSCharacteristicsName;

                $("#observationContent #divSystem" + ROSSystemId + ROSCharacteristicsId).append(txttoAppend);
            }

            if (ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId && ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId == ROSCharacteristicsId) {
                        ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = true;
                        ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                    }
                    if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                    }
                }
            }

            $("#ulPhysicalExamSystems #chk" + ROSSystemId).prop("checked", true);
            ROSTemplateDetailRevamp.loadCharatristics(ROSSystemId);
        }
        else if ($(obj).prop('checked') == false) {
            if (ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId && ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId == ROSCharacteristicsId) {
                        ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();

                    }
                    if ($('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find(':checkbox:checked').length == 0 && ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = false;

                    }
                }
            }
            var aa = $('#observationContent #divSystem' + ROSSystemId + ROSCharacteristicsId).text();
            $('#observationContent #divSystem' + ROSSystemId + ROSCharacteristicsId).remove();
            ROSTemplateDetailRevamp.removeLastDelimiter(ROSSystemId);
        }
        if ($('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find(':checkbox:checked').length < 1) {
            $('#ulPhysicalExamSystems >li.active :checkbox').prop("checked", false)
        }

        if ($('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').length == $('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find(':checkbox:checked').length) {
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', true);
        }
        else {
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', false);
        }
        ROSTemplateDetailRevamp.removeLastDelimiter(ROSSystemId);
        //if ($('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('[onClick^="ROSTemplateDetailRevamp.PreviewCharateristics"]').prop('checked') == true) {
        //   // $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', true);
        //}
        //else {
        ////    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', false);
        // //   $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').find('[type=checkbox]').prop('checked', false);

        //}
    },
    validateDurationDetails: function (obj) {
        var duration = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #divExamCharacteristics #Duration');
        var durationQualifier = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #divExamCharacteristics #ROSCharacteristicsDetailDurationId');
        if (duration.val() != null && duration.val() != '') {
            durationQualifier.prop("disabled", false);
        } else {
            durationQualifier.prop("disabled", true).find("option:first").attr("selected", true);
        }
    },

    saveROSTemplate: function () {
        ROSTemplateDetailRevamp.ROSTemplateSave();
    },
    addActiveClass: function (selectedLi) {
        $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection > li").removeClass('active');
        $(selectedLi).addClass('active');
        if (!$('#' + ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').prev().hasClass('hidden')) {
            $('#' + ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').prev().addClass('hidden');
            ROSTemplateDetailRevamp.countWidth();
            $('#' + ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
            $('#' + ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').removeClass('active');
        }
        ROSTemplateDetailRevamp.enableDisableCharcDetails(selectedLi);
    },
    checkUncheckPositiveNegative: function (checkedBox, systemNameWithID) {
        //event.stopPropagation();
        //  if (ROSTemplateDetailRevamp.checkDurationDetails()) {
        // Code for check detail 
        if (!$(checkedBox).parents('li ').find('div [id^=divDetail]').hasClass('hidden') == true) {
            //brake;
            return
        }
        var SystemID = $(checkedBox).parents('li').attr('parentid');
        var CharID = $(checkedBox).parents('li').attr('id');
        var detailPositiveNegative = $.grep(ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
            return (item.SystemId == SystemID && item.CharcId == CharID);
        });



        if (detailPositiveNegative.length == 0) {
            var CharsPosNegDetail = {
                Id: SystemID + '-' + CharID,
                SystemId: SystemID,
                CharcId: CharID,
                CharcComments: '',
                IsPositive: false,
                IsNegative: false

            };
            ROSTemplateDetailRevamp.PositiveNegativeDetail.push(CharsPosNegDetail);



        }
        indexes = $.map(ROSTemplateDetailRevamp.PositiveNegativeDetail, function (obj, index) {
            if (obj.CharcId == CharID && obj.SystemId == SystemID) {
                return index;
            }
        })
        var CharPosNegDetailIndex = indexes[0];

        if (!$("#ulPhysicalExamSystemSection").find('.toggle').prev().hasClass('hidden')) {
            $("#ulPhysicalExamSystemSection").find('.toggle').prev().addClass('hidden');
            ROSTemplateDetailRevamp.countWidth();
            $("#ulPhysicalExamSystemSection").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
            $("#ulPhysicalExamSystemSection").find('.toggle').removeClass('active');
        }

        if (!$("#ulPhysicalExamSystemSection").find('.toggle').hasClass('disableAll')) {
            ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(false);
        }

        if ($("#ulPhysicalExamSystemSection > li").hasClass('active'))
            $("#ulPhysicalExamSystemSection > li").removeClass('active');

        if (checkedBox != null) {
            ROSTemplateDetailRevamp.validateDurationDetails();
            var IsComments = false;
            if ($('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li.active ').find('[id^=divDetail]').find('textarea').val()) {
                IsComments = true;
            }
            if ($(checkedBox).attr('value') == '') {
                $(checkedBox).attr('value', true)
                if ($(checkedBox).html() == ' (+) ') {
                    $(checkedBox).css('color', 'green');
                    ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsPositive = true;
                    ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsNegative = false;
                }
                else {
                    $(checkedBox).css('color', 'red');
                    ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsNegative = true;
                    ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsPositive = false;
                }
                // $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').addClass('green')
                $(checkedBox).parents('li').find('i.fa-book').removeClass('active');
                // ROSTemplateDetailRevamp.bookIconClassToggle($(checkedBox).parents('li').find('i.fa-book'), false);
                if ($(checkedBox).parent().parent().siblings('span').find('a').attr('value') == 'true') {
                    // $(checkedBox).parent().siblings('.checkbox-icon').find('[type=checkbox]').prop('checked', false);
                    $(checkedBox).parent().parent().siblings('span').find('a').attr('value', '')
                    $(checkedBox).parent().parent().siblings('span').find('a').css('color', 'black')
                }
                $('[id^=selectAllNeg] ,[id^=selectAllPos').prop('checked', false);

                if (!$("#ReviewofSystems").find('.toggle').hasClass('disableAll')) {
                    ROSTemplateDetailRevamp.LoadCharacteristicDetailInfo(checkedBox);
                }
                if (IsComments == true) {
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul ').find('[id=' + CharID + ']').find('i.fa-book').removeClass('blue');
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul ').find('[id=' + CharID + ']').find('i.fa-book').addClass('green');
                }

            } else {
                if ($(checkedBox).html() == ' (+) ') {

                    ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsPositive = false;
                }
                else {

                    ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsNegative = false;
                }
                $(checkedBox).parents('li').find('label').find('i').removeClass('green');
                $(checkedBox).parents('li').find('div[id^=divDetail] ').find('textarea').val('');
                $(checkedBox).attr('value', '')
                $(checkedBox).css('color', 'black')
                if ($(checkedBox).parents('li').siblings().find("#Positive[style='margin-right: 5px; color: rgb(0, 128, 0);']").length == 0 && $(checkedBox).parents('li').siblings().find("#Negative[style='margin-right: 5px; color: rgb(255, 0, 0);']").length == 0) {
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').removeClass('green')
                }
                //ROSTemplateDetailRevamp.bookIconClassToggle($(checkedBox).parents('li').find('i.fa-book'), false);
                $(checkedBox).parents('li').find('i.fa-book').removeClass('active');
                $('[id^=selectAllNeg] ,[id^=selectAllPos').prop('checked', false);
                var charcId = $(checkedBox).closest('li').attr('id');

                var message = "Unselecting a characteristic will remove its detail. Are you sure you want to unselect?";
                var isCharcDetailExists = $(checkedBox).closest('li').find("[type=hidden][name=isCharcDetailExists]").val();

                //var systemPatientCharacteristicsID = $(checkedBox).parent().parent().find("[type=hidden][name=SystemPatientCharacteristicsID]").val();
                var systemPatientCharacteristicsID = $(checkedBox).closest('li').find("[type=hidden][name=SystemPatientCharacteristicsID]").attr('id');
                systemPatientCharacteristicsID = systemPatientCharacteristicsID != null ? systemPatientCharacteristicsID.split('_')[1] : systemPatientCharacteristicsID;
                if (systemPatientCharacteristicsID != -1 && systemPatientCharacteristicsID != '' && systemPatientCharacteristicsID != null) {
                    utility.myConfirm(message, function () {
                        ROSTemplateDetailRevamp.deleteROSDataSystemCharcDetail_DBCall(systemPatientCharacteristicsID).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                ROSTemplateDetailRevamp.uncheckCharacteristic($(checkedBox));
                            }
                        });

                    }, function () {
                        $(checkedBox).prop('checked', '');
                        ROSTemplateDetailRevamp.enableDisableCharcDetails();
                    });
                } else {
                    ROSTemplateDetailRevamp.uncheckCharacteristic($(checkedBox));
                }
                //  ROSTemplateDetailRevamp.hideTextAreaWhenBothChkBoxesUnchecked(checkedBox);
            }

            ROSTemplateDetailRevamp.enableDisableCharcDetails();
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' [name=ReviewofSystemsSectionNormal]').prop('checked', false);
            // ROSTemplateDetailRevamp.changeColorSystemOnCharcChange();
            ROSTemplateDetailRevamp.ChangeColorPostiveNegativeAll();

        }



    },
    ChangeColorPostiveNegativeAll: function () {

        setTimeout(function () {
            if ($('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find('#Positive').length == $('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find("#Positive[style='margin-right: 5px; color: rgb(0, 128, 0);']").length) {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').addClass('green');

            }
            else if ($('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find('#Positive').length == $('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find("#Negative[style='margin-right: 5px; color: rgb(255, 0, 0);']").length) {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').addClass('red');

            }
            else {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');


            }
            if ($('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find("#Positive[style='margin-right: 5px; color: rgb(0, 128, 0);']").length > 0 || $('#frmROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find("#Negative[style='margin-right: 5px; color: rgb(255, 0, 0);']").length > 0) {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystems  ul li.active').addClass('green');
            } else {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystems  ul li.active').removeClass('green');
            }

        }, 100);
    },
    hideTextAreaWhenBothChkBoxesUnchecked: function (chkBox) {
        if ($(chkBox).closest("li").find('input[type=checkbox]:checked').length == 0) {
            $(chkBox).closest("li").find("div.rightInnerAddon").remove();
        }
    },
    uncheckCharacteristic: function (checkedBox) {
        var $charcLI = $(checkedBox).parents('li');
        var detailObj = $.grep(ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
            if (item.Id == $charcLI.attr('id')) {
                ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.splice(index, 1);
            };
        });

        if (detailObj.length > 0) {
            ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.splice(detailObj, 1);
        }
        ROSTemplateDetailRevamp.bookIconClassToggle($charcLI.find('[id=bookIcon]'), true);
        $charcLI.find("[type=hidden]").val('');

        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics').resetAllControls();
        ROSTemplateDetailRevamp.enableDisableCharcDetails();
        ROSTemplateDetailRevamp.handleDetailToggleDiv();
    },
    handleDetailToggleDiv: function () {
        $('#' + ROSTemplateDetailRevamp.params.PanelID + " #ulPhysicalExamSystemSection").find('.toggle').prev().addClass('hidden');
        ROSTemplateDetailRevamp.countWidth();
        $("#ulPhysicalExamSystemSection").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
        $("#ulPhysicalExamSystemSection").find('.toggle').removeClass('active');
        $("#ulPhysicalExamSystemSection").find('.toggle').addClass('disableAll');
    },
    bookIconClassToggle: function (Control, IsDisabled) {
        var isCharC = $(Control).closest('li') || $(Control).closest('#divNormalGlobal');
        var charcComments = isCharC.find('[id^="Charc_Desc"]').val();
        if (charcComments == null || charcComments == '') {
            if (!IsDisabled) {
                $(Control).removeClass('disabled');
                $(Control).removeClass('gray');
                $(Control).addClass('blue');
                if ($(Control).find('i') != null && $(Control).find('i').length > 0) {
                    $(Control).find('i').removeClass('disabled');
                    $(Control).find('i').removeClass('gray');
                    $(Control).find('i').addClass('blue');
                }
            } else {
                ROSTemplateDetailRevamp.disableBookIcon(Control);
            }
        } else if (IsDisabled) {
            ROSTemplateDetailRevamp.disableBookIcon(Control);
        }
    },
    disableBookIcon: function (Control) {
        $(Control).removeClass('green');
        $(Control).removeClass('blue');
        $(Control).addClass('gray');
        $(Control).addClass('disabled');
        if ($(Control).find('i') != null && $(Control).find('i').length > 0) {
            $(Control).find('i').removeClass('green');
            $(Control).find('i').removeClass('blue');
            $(Control).find('i').addClass('gray');
            $(Control).find('i').addClass('disabled');
        }
    },
    LoadCharacteristicDetailInfo: function (checkedBox) {

        var charcId = $(checkedBox).parents('li').attr('id');
        var charcDetailDiv = $('#divExamCharacteristics');

        if (ROSTemplateDetailRevamp.CharacteristicsDetailsInfo != null && ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.length > 0) {

            var detailObj = $.grep(ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
                return (item.Id == charcId);
            });

            if (detailObj.length > 0) {
                utility.bindMyJSON(true, JSON.parse(detailObj[0].DetailInfo), false, $(charcDetailDiv));
                charcId = charcId.replace('/', '\\/');

                $(charcDetailDiv).data($(charcId), detailObj[0].DetailInfo);
                ROSTemplateDetailRevamp.validateDurationDetails();
            }
            else {
                $(charcDetailDiv).resetAllControls();
            }

        } else {
            $(charcDetailDiv).resetAllControls();

        }
    },
    changeColorSystemOnCharcChange: function () {
        var CurrentSystem = $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #PhysicalExam li.active");
        var charcSystemDiv = '#' + ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics';
        var addGreen = false;
        if (($(charcSystemDiv).find('input:checkbox') != null && $(charcSystemDiv).find('input:checkbox').length > 0)) {
            if ($(charcSystemDiv).find('input:checkbox:checked') != null && $(charcSystemDiv).find('input:checkbox:checked').length > 0) {
                addGreen = true;
            } else {
                addGreen = false;
            }
        }

        if (addGreen) {
            $(CurrentSystem).find('span:first-child').addClass("green");
        } else {
            //Start || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-471
            $(CurrentSystem).removeClass("green").find('span:first-child').removeClass("green");
            //End   || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-471
        }
        return addGreen;
    },
    enableDisableCharcDetails: function (selectedLi) {
        if (selectedLi != null) {
            var objList = $.grep($(selectedLi).find('a[value="true"]'), function (element) {
                if ($(element).is('a[value="true"]')) {
                    return element;
                }
            });
            if (objList.length > 0) {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').removeClass('disableAll');
            } else {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').addClass('disableAll');
            }
        } else {
            if ($('#' + ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('input:checkbox:not([id*=ReviewofSystemsSectionNormal])').is(':checked') == true) {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').removeClass('disableAll');
            }
            else if ($('#' + ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('input:checkbox[id*=ReviewofSystemsSectionNormal]')) {
                $('#' + ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').addClass('disableAll');
            }
        }

    },
    loadROSLookups_DBcall: function () {
        var objData = new Object();
        objData["commandType"] = "load_reviewofsystem_lookups";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ReviewofSystemRevampTemplate");
    },
    ROSLookUps: function (pageNo, rpp) {
        ROSTemplateDetailRevamp.loadROSLookups_DBcall().done(function (response) {
            // var data_ = { data: [], total: 0 };
            if (response != "") {
                var resposeData = JSON.parse(response);
                if (resposeData.status != false && resposeData.ROSLookupsCount > 0) {
                    if (resposeData.ROSLookupsCount_JSON != undefined)
                        var lookupsCount_JSON = jQuery.parseJSON(resposeData.ROSLookupsCount_JSON);

                    var lookupsCount_JSON = jQuery.parseJSON(resposeData.ROSLookupsCount_JSON);
                    for (var i = 0; i < lookupsCount_JSON.length; i++) {
                        if (lookupsCount_JSON[i].CharateristicsDetailName == "Location") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#hfLocation').val(lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId);
                            }
                        }
                        if (lookupsCount_JSON[i].CharateristicsDetailName == "AssociatedBy") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#hfAssociatedBy').val(lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId);
                            }
                        }
                        if (lookupsCount_JSON[i].CharateristicsDetailName == "PrecipitatedBy") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#hfPrecipitatedBy').val(lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId);
                            }
                        }
                        if (lookupsCount_JSON[i].CharateristicsDetailName == "PreviousHistory") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#hfPreviousHistory').val(lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId);
                            }
                        }
                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSDuration") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#hfROSDuration').val(lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId);
                            }
                        }
                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSOnset") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#hfROSOnset').val(lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId);
                            }
                        }
                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSCharacteristicsDetailStatus") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#ROSCharacteristicsDetailStatusId').append('<option value=' + lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId + ' > ' + lookupsCount_JSON[i].CharatristicsDetail[j].LookupValueName + '</option>')
                            }
                        }

                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSCharacteristicsDetailDuration") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#ROSCharacteristicsDetailDurationId').append('<option value=' + lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId + ' > ' + lookupsCount_JSON[i].CharatristicsDetail[j].LookupValueName + '</option>')
                            }
                        }

                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSCharacteristicsDetailPattern") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#ROSCharacteristicsDetailPatternId').append('<option value=' + lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId + ' >' + lookupsCount_JSON[i].CharatristicsDetail[j].LookupValueName + '</option>')
                            }
                        }

                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSCharacteristicsDetailSeverity") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#ROSCharacteristicsDetailSeverityId').append('<option value=' + lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId + ' >' + lookupsCount_JSON[i].CharatristicsDetail[j].LookupValueName + '</option>')
                            }
                        }

                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSCharacteristicsDetailCourse") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#ROSCharacteristicsDetailCourseId').append('<option value=' + lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId + ' >' + lookupsCount_JSON[i].CharatristicsDetail[j].LookupValueName + '</option>')
                            }
                        }

                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSCharacteristicsDetailRadiation") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#ROSCharacteristicsDetailRadiationId').append('<option value=' + lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId + ' >' + lookupsCount_JSON[i].CharatristicsDetail[j].LookupValueName + '</option>')
                            }
                        }

                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSCharacteristicsDetailFrequency") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#ROSCharacteristicsDetailFrequencyId').append('<option value=' + lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId + ' >' + lookupsCount_JSON[i].CharatristicsDetail[j].LookupValueName + '</option>')
                            }
                        }

                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSCharacteristicsDetailContext") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#ROSCharacteristicsDetailContextId').append('<option value=' + lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId + ' >' + lookupsCount_JSON[i].CharatristicsDetail[j].LookupValueName + '</option>')
                            }
                        }

                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSCharacteristicsDetailCharacterCSZ") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#ROSCharacteristicsDetailCharacterCSZId').append('<option value=' + lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId + ' >' + lookupsCount_JSON[i].CharatristicsDetail[j].LookupValueName + '</option>')
                            }
                        }

                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSCharacteristicsDetailAggravedBy") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#ROSCharacteristicsDetailAggravedById').append('<option value=' + lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId + ' >' + lookupsCount_JSON[i].CharatristicsDetail[j].LookupValueName + '</option>')
                            }
                        }
                        if (lookupsCount_JSON[i].CharateristicsDetailName == "ROSCharacteristicsDetailRelievedBy") {
                            for (var j = 0; j < lookupsCount_JSON[i].CharatristicsDetail.length; j++) {
                                $('#ROSCharacteristicsDetailRelievedById').append('<option value=' + lookupsCount_JSON[i].CharatristicsDetail[j].ROSCharacteristicsDetailLookupId + ' >' + lookupsCount_JSON[i].CharatristicsDetail[j].LookupValueName + '</option>')
                            }
                        }

                    }
                    $('#frmROSTemplateDetailRevamp').data('serialize', $('#frmROSTemplateDetailRevamp').serialize());
                    // data_.total = resposeData.iTotalDisplayRecords;
                    //e.success(data_);
                }
                else {
                    utility.DisplayMessages(resposeData.Message, 3);
                    //e.success(data_);
                }
            }
            else {
                e.error();
            }
        });



    },
    removeLastDelimiter: function (ROSSystemId) {

        var delii = $("#delimator option:selected").text();
        var str = "";
        if (delii == ",") str = $($('#observationContent div[id^=divSystem' + ROSSystemId + ']')[0]).text().replace(/,/g, "");
        if (delii == ".") str = $($('#observationContent div[id^=divSystem' + ROSSystemId + ']')[0]).text().replace(/./g, "");
        if (delii == ":") str = $($('#observationContent div[id^=divSystem' + ROSSystemId + ']')[0]).text().replace(/:/g, "");
        if (delii == ";") str = $($('#observationContent div[id^=divSystem' + ROSSystemId + ']')[0]).text().replace(/;/g, "");
        if (delii == "-") str = $($('#observationContent div[id^=divSystem' + ROSSystemId + ']')[0]).text().replace(/-/g, "");

        var id = $($('#observationContent div')[0]).attr('id');
        $("#observationContent #" + id).text(str);
    },

    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + ROSTemplateDetailRevamp.params["PanelID"];
        $.each(ddlCommaSeparatedIds, function (index, Item) {
            if (isHide) {
                $(parrentPanelId + " #" + Item).multiselect('disable');
            }
            else {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            }
        });
    },

    loadEntitySpecialty: function (entityID, dfd) {

        // Loads Spacialties Based on entityId
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (ROSTemplateDetailRevamp.SpecialtyIds != '') {

                        var Specialties = ROSTemplateDetailRevamp.SpecialtyIds.split(",");
                        ROSTemplateDetailRevamp.specialityCheckedIds = Specialties;
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').val(Specialties);
                    }
                    $('#frmROSTemplateDetailRevamp').data('serialize', $('#frmROSTemplateDetailRevamp').serialize());
                }

            }).then(function () {
                ROSTemplateDetailRevamp.IntializeMultiSelectDropDownSpecialties();
                //enable dropdownlist
                ROSTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty', false);
                ROSTemplateDetailRevamp.loadEntityProvider(globalAppdata["SeletedEntityId"], dfd);


            });
        }
        else {
            //Disable dropdownlist
            ROSTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty', true);
        }
    },

    loadEntityProvider: function (entityId, dfd) {

        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider');
                var $providerHiddenDdl = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlHiddenPhysicalExamTemplateProvider');

                //Empty both the providers ddls.
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
                if (ROSTemplateDetailRevamp.ProviderIds != '') {
                    var Providers = ROSTemplateDetailRevamp.ProviderIds.split(",");
                    ROSTemplateDetailRevamp.providerCheckedIds = Providers;
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').val(Providers);
                }
                $('#frmROSTemplateDetailRevamp').data('serialize', $('#frmROSTemplateDetailRevamp').serialize());
            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamTemplateSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                ROSTemplateDetailRevamp.IntializeMultiSelectDropDownProviders();
                $('#frmROSTemplateDetailRevamp').data('serialize', $('#frmROSTemplateDetailRevamp').serialize());
                if (dfd)
                    dfd.resolve();
            });
            //enable multiselect
            //ROSTemplateDetailRevamp.enableDisableDropDowLists('ddlPhysicalExamTemplateProvider', false);
        }
        else {
            //disable multiselect
            ROSTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateProvider', true);
        }
    },

    buildAlreadyAssosiatedSystems: function (templateId) {
        ROSTemplateDetailRevamp.ROSTemplateDetailRevampLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var res = JSON.parse(response.PETemplateSystems_JSON);
                var resSystems = JSON.parse(res);
                var data = [];

                $.each(resSystems, function (i, item) {
                    data.push({ id: item.ROSSystemId, text: item.SystemName, expanded: true, spriteCssClass: "rootfolder" });
                    var li = ROSTemplateDetailRevamp.addSystem(item.ROSSystemId, item.SystemName); //'<li id="' + item.ROSSystemId + '" parentid="null" onclick="" value="' + item.ROSSystemId + '" refvalue="" subcharacteristicexist=" " class="active"><div class="checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chk' + item.ROSSystemId + '" name="' + item.ROSSystemId + '" class="pull-left  char" onclick=""><label id="lblName' + item.ROSSystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + item.SystemName + '">' + item.SystemName + '</label><div id="divNameDetail' + item.ROSSystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" id="txtName' + item.ROSSystemId + '" onkeypress="" name="Name' + item.ROSSystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.ROSSystemId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                    //var li = '<li id="' + item.ROSSystemId + '" parentid="null" onclick="ROSTemplateDetailRevamp.showHideChildControls(this,"ulPhysicalExamSystems",' + item.ROSSystemId + ');" value="' + item.ROSSystemId + '" refvalue="" subcharacteristicexist=" " class="active"><div class="checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chk' + item.ROSSystemId + '" name="' + item.ROSSystemId + '" class="pull-left  char" onclick="ROSTemplateDetailRevamp.selectParentControls(this);ROSTemplateDetailRevamp.toggleCheckBoxes(this);"><label id="lblName' + item.ROSSystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + item.SystemName + '">' + item.SystemName + '</label><div id="divNameDetail' + item.ROSSystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" id="txtName' + item.ROSSystemId + '" onkeypress="ROSTemplateDetailRevamp.saveDetailComments(event,this)" name="Name' + item.ROSSystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.ROSSystemId + '" onclick="ROSTemplateDetailRevamp.saveTemplateSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                    $("#ulPhysicalExamSystems").append(li);
                    ROSTemplateDetailRevamp.loadCharatristics(item.ROSSystemId);
                });

                //$("#treeview").kendoTreeView({
                //    checkboxes: {
                //        checkChildren: true
                //    },
                //    check: onCheck,
                //    dataSource: data,
                //});

                //function checkedNodeIds(nodes, checkedNodes) {
                //    for (var i = 0; i < nodes.length; i++)
                //    {
                //        if (nodes[i].checked)
                //            checkedNodes.push(nodes[i].id);
                //                checkedNodeIds(nodes[i].children.view(), checkedNodes);
                //    }
                //}

                //function onCheck() {
                //    var checkedNodes = [],
                //        treeView = $("#treeview").data("kendoTreeView"),
                //        message;

                //    checkedNodeIds(treeView.dataSource.view(), checkedNodes);
                //    if (checkedNodes.length > 0)
                //        message = "IDs of checked nodes: " + checkedNodes.join(",");
                //     else
                //        message = "No nodes checked.";
                //    $("#result").html(message);
                //}
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    buildSystemsAutoComplete: function (templateId) {
        ROSTemplateDetailRevamp.ROSSystemsLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var resSystems = JSON.parse(response.PESystems_JSON);
                // var resSystems = JSON.parse(res);
                var data = [];
                $.each(resSystems, function (i, item) {
                    data.push({ id: item.ROSSystemId, value: item.Name });
                });

                $("#Systems").kendoAutoComplete({
                    dataSource: data,
                    filter: "contains",
                    dataTextField: "value",
                    placeholder: "Select System...",
                    select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        //alert(kendo.stringify(dataItem));
                        var li = ROSTemplateDetailRevamp.addSystem(dataItem.id, dataItem.value);
                        if (li != undefined)
                            $("#ulPhysicalExamSystems").append(li);
                        $("#Systems").val('');
                        ROSTemplateDetailRevamp.loadCharatristics(dataItem.id);
                        e.preventDefault();
                    },
                    footerTemplate: 'Total <strong>#: instance.dataSource.total() #</strong> items found'
                });

                $("#Systems").parent().addClass('size100per');
                $('#frmROSTemplateDetailRevamp').data('serialize', $('#frmROSTemplateDetailRevamp').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadROSTemplateDetailRevamp: function (templateId) {
        ROSTemplateDetailRevamp.ROSTemplateDetailRevampLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                response.PhysicalExamTemplate = JSON.parse(response.PhysicalExamTemplate);

                if (response.PhysicalExamTemplate.length > 0) {
                    response.PhysicalExamTemplate = response.PhysicalExamTemplate[0];
                    ROSTemplateDetailRevamp.selectedPhyExamTempData = response.PhysicalExamTemplate.SysSecCharSubcharData;
                    //Start Farooq Ahmad 03-03-2016 mark green and checked the loaded System
                    for (var index in ROSTemplateDetailRevamp.selectedPhyExamTempData) {
                        //if ($.parseJSON(ROSTemplateDetailRevamp.selectedPhyExamTempData[index].IsChecked.toString().toLowerCase()))
                        //    $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId).addClass("green");
                        //else
                        //    $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId).removeClass("green");

                        $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId + " input:checkbox").prop("checked", $.parseJSON(ROSTemplateDetailRevamp.selectedPhyExamTempData[index].IsChecked.toString().toLowerCase()));
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId + " label").attr("data-original-title", ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemName);
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId + " label").text(ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemName);
                    }
                    //End Farooq Ahmad 03-03-2016 mark green and checked the loaded System
                    var self = $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp");

                    utility.bindMyJSONByName(true, response.PhysicalExamTemplate, false, self).done(function () {
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateEntity').change();
                        ROSTemplateDetailRevamp.SpecialtyIds = response.PhysicalExamTemplate.SpecialtyIds;
                        ROSTemplateDetailRevamp.ProviderIds = response.PhysicalExamTemplate.ProviderIds;
                    });
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ROSSystemsLoad: function (templateId) {
        var objData = new Object();
        objData["TemplateId"] = templateId;
        objData["commandType"] = "load_ros_systems_lookup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSSystem");
    },

    ROSCharatristicsLoad: function (ROSSystemId) {
        var objData = new Object();
        objData["ROSSystemId"] = ROSSystemId;
        objData["commandType"] = "Load_ReviewofSystem_System_Charatristics";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSSystem");
    },

    ROSTemplateDetailRevampLoad: function (templateId) {
        var objData = new Object();
        objData["TemplateId"] = templateId == undefined ? "0" : templateId;
        objData["commandType"] = "Load_PhyscialExam_Templates_ECW";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    ROSTemplateDetailRevampLoadOnDemand: function (templateId, systemId, sectionId, charId, commandType) {
        var objData = new Object();
        objData["TemplateId"] = templateId;
        if (systemId != null)
            objData["SystemId"] = systemId;
        if (sectionId != null)
            objData["SectionId"] = sectionId;
        if (charId != null)
            objData["CharacteristicId"] = charId;

        objData["commandType"] = commandType;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('destroy');
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {

                ROSTemplateDetailRevamp.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                $.when(
                    ROSTemplateDetailRevamp.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (ROSTemplateDetailRevamp.ProviderIds != '') {
                        var Providers = ROSTemplateDetailRevamp.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                ROSTemplateDetailRevamp.providerCheckedIds = ROSTemplateDetailRevamp.removeFromArray(ROSTemplateDetailRevamp.providerCheckedIds, item);
                                ROSTemplateDetailRevamp.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').val(ROSTemplateDetailRevamp.providerCheckedIds);
                    ROSTemplateDetailRevamp.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (ROSTemplateDetailRevamp.SpecialtyIds != '') {
                    var spacialties = ROSTemplateDetailRevamp.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            ROSTemplateDetailRevamp.specialityCheckedIds = ROSTemplateDetailRevamp.removeFromArray(ROSTemplateDetailRevamp.specialityCheckedIds, item);
                            ROSTemplateDetailRevamp.specialityCheckedIds.push(item);
                        });
                    }
                }
                ROSTemplateDetailRevamp.setSpacialtiesByselectedProviderIds();
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('clearSelection', false);
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('updateButtonText');
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('select', ROSTemplateDetailRevamp.specialityCheckedIds);
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('refresh');
            },
        });
    },

    setSpacialtiesByselectedProviderIds: function () {

        $.each(ROSTemplateDetailRevamp.providerCheckedIds, function (index, item) {

            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {

                        ROSTemplateDetailRevamp.specialityCheckedIds = ROSTemplateDetailRevamp.removeFromArray(ROSTemplateDetailRevamp.specialityCheckedIds, $(this).attr('refname'));
                        ROSTemplateDetailRevamp.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('destroy');
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                ROSTemplateDetailRevamp.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
                // ROSTemplateDetailRevamp.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('refresh');
            },


        });
    },

    domReady: function () {

        $(document).ready(function () {

            ROSTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty,ddlPhysicalExamTemplateProvider', true);

            //callback function on change event of entity ddl
            ROSTemplateDetailRevamp.entityChanged();

            //Initialize when the document is ready for the first time (just for the good look).
            ROSTemplateDetailRevamp.IntializeMultiSelectDropDownSpecialties();
            ROSTemplateDetailRevamp.IntializeMultiSelectDropDownProviders();


            $(document).click(function (event) {



                var $Item = $(ROSTemplateDetailRevamp.selectedListItem);
                var id = $Item.attr('id');

                // var allItems = 

                var SystemDetailDiv = $Item.find("div#divNameDetail" + id);
                var SystemNameLabel = $Item.find("#lblName" + id);
                var txtSystemName = SystemDetailDiv.find("#txtName" + id);

                var isEqual = true;

                //if not matched
                isEqual = $(event.target).closest('li#' + id).length == 0 ? false : true;

                if (!isEqual) {

                    //if not matched with self
                    if ($(event.target).attr('id') != id) {
                        isEqual = false;
                    }
                }
                if (!isEqual) {

                    SystemDetailDiv.addClass("hidden");
                    SystemNameLabel.removeClass("hidden");
                }
            });


        });
    },

    entityChanged: function () {

        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateEntity').change(function () {
            //Get the selected entity
            selectedEntity = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateEntity :selected').val();

            $.when(ROSTemplateDetailRevamp.loadEntitySpecialty(selectedEntity)).then(function () {

                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('destroy').multiselect();
            });
            $.when(ROSTemplateDetailRevamp.loadEntityProvider(selectedEntity)).then(function () {

                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('destroy').multiselect();

            });
        });
    },

    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlHiddenPhysicalExamTemplateProvider';

        var providerContext = '#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider';
        $(providerContext).empty();

        if (ROSTemplateDetailRevamp.specialityCheckedIds.length > 0) {

            $.each(ROSTemplateDetailRevamp.specialityCheckedIds, function (index, specialtyId) {

                $(providerHiddenContext).find('option').each(function (index, option) {
                    if ($(option).attr('refname') == specialtyId) {
                        $(providerContext).append(
                         $('<option/>', {
                             value: $(option).val(),
                             html: $(option).html(),
                             refname: $(option).attr('refname'),
                             refvalue: $(option).attr('refvalue')

                         }));
                    }
                });
            });
        }
        else {
            $(providerHiddenContext).find('option').each(function (index, option) {
                $(providerContext).append(
                         $('<option/>', {
                             value: $(option).val(),
                             html: $(option).html(),
                             refname: $(option).attr('refname'),
                             refvalue: $(option).attr('refvalue')

                         }));
            });
        }
    },

    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamTemplateSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            ROSTemplateDetailRevamp.specialityCheckedIds = [];
            ROSTemplateDetailRevamp.providerCheckedIds = [];
            ROSTemplateDetailRevamp.ProviderIds = '';
            ROSTemplateDetailRevamp.SpecialtyIds = '';
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divROSTemplateBodyPart').addClass('hidden');
            $('#' +ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ddlBodyParts').val("");
            ROSTemplateDetailRevamp.orthopedicSpecialitySelected = false;
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {


                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    ROSTemplateDetailRevamp.specialityCheckedIds = ROSTemplateDetailRevamp.removeFromArray(ROSTemplateDetailRevamp.specialityCheckedIds, spacialityId);
                    ROSTemplateDetailRevamp.specialityCheckedIds.push(spacialityId);
                    if ($(option).text() == "ORTH SURG") {
                        ROSTemplateDetailRevamp.orthopedicSpecialitySelected = true;
                        if ($.inArray(spacialityId, ROSTemplateDetailRevamp.specialityCheckedIds) > -1)
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divROSTemplateBodyPart').removeClass('hidden');
                        else
                        {
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divROSTemplateBodyPart').addClass('hidden');
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ddlBodyParts').val("");
                        }
                            
                    }
                    else if (ROSTemplateDetailRevamp.orthopedicSpecialitySelected == false)
                    {
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ddlBodyParts').val("");
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divROSTemplateBodyPart').addClass('hidden');
                    }
                }
                else {

                    ROSTemplateDetailRevamp.specialityCheckedIds = ROSTemplateDetailRevamp.removeFromArray(ROSTemplateDetailRevamp.specialityCheckedIds, spacialityId);
                    if ($(option).text() == "ORTH SURG") {
                        if ($.inArray(spacialityId, ROSTemplateDetailRevamp.specialityCheckedIds) > -1)
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divROSTemplateBodyPart').removeClass('hidden');
                        else
                        {
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ddlBodyParts').val("");
                            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divROSTemplateBodyPart').addClass('hidden');
                        }
                    }
                    else if (ROSTemplateDetailRevamp.orthopedicSpecialitySelected == false)
                    {
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ddlBodyParts').val("");
                        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divROSTemplateBodyPart').addClass('hidden');
                    }
                }


            }
            else {
                ROSTemplateDetailRevamp.orthopedicSpecialitySelected = false;
                ROSTemplateDetailRevamp.specialityCheckedIds = [];
                $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    if ($(this).length > 0 && $(this).text() == "ORTH SURG" && $(this)[0].selected)
                        ROSTemplateDetailRevamp.orthopedicSpecialitySelected = true;
                    ROSTemplateDetailRevamp.specialityCheckedIds.push(spacialityId);
                });
                if (ROSTemplateDetailRevamp.orthopedicSpecialitySelected)
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divROSTemplateBodyPart').removeClass('hidden');
                else
                {
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ddlBodyParts').val("");
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #divROSTemplateBodyPart').addClass('hidden');
                }

            }
        }
    },

    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },

    checkSpecialtiesByProviderId: function (option, checked, select) {

        //provider context
        var providerContext = '#' + ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamTemplateProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        var allProviders = $(providerContext).find('.dropdown-menu').find('li:not(".filter,.multiselect-all")').length;
        var selectedProviders = $(providerContext).find('.dropdown-menu').find('li.active:not(".filter,.multiselect-all")').length;

        if (checkedProviderItems <= 0) {
            ROSTemplateDetailRevamp.providerCheckedIds = [];
            ROSTemplateDetailRevamp.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected && allProviders == selectedProviders) {
            ROSTemplateDetailRevamp.providerCheckedIds = [];
            $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider option').each(function () {
                var providerValue = $(this).val();
                ROSTemplateDetailRevamp.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                ROSTemplateDetailRevamp.providerCheckedIds = ROSTemplateDetailRevamp.removeFromArray(ROSTemplateDetailRevamp.providerCheckedIds, providerValue);
                ROSTemplateDetailRevamp.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                ROSTemplateDetailRevamp.providerCheckedIds = ROSTemplateDetailRevamp.removeFromArray(ROSTemplateDetailRevamp.providerCheckedIds, $(option).val());
            }

        }
    },

    addNewItem: function (itemType) {
        if (itemType != null && itemType != "") {

            var addSubCharIcon = "";

            charSelectAll = null;
            subCharSelectAll = null;

            var isSubCharacteristic = false;
            var ulControl = "";
            var currentLiClick = "";
            var currentCtrlId = "";
            var currentParentId = "";
            var currentId = "";
            currentId = ROSTemplateDetailRevamp.NewInsertedId--;
            var subcharacteristicExist = "";

            var saveMethodPE = "";

            if (itemType.toLowerCase() == "system") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #" + currentCtrlId);
                saveMethodPE = "ROSTemplateDetailRevamp.AddSystemROS(this, '" + currentId + "');";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystemSection";
                ulControl = $('#' + ROSTemplateDetailRevamp.params.PanelID + " #frmROSTemplateDetailRevamp #" + currentCtrlId);
                saveMethodPE = "ROSTemplateDetailRevamp.AddChartristics(this, '" + currentId + "');";
            }

            if (ulControl != null && ulControl != "") {
                var currentLiClass = "";

                var arrNewlyAddedLi = ulControl.find("li[id*='-']");

                if (itemType.toLowerCase() != "system") {
                    currentParentId = ulControl.find("li:last").attr("parentid");
                    if (currentParentId == null)
                        currentParentId = ulControl.attr("ParentId");
                }

                var onClick = "";
                onClick = "";//"ROSTemplateDetailRevamp.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";
                var deleteFunction = "ROSTemplateDetailRevamp.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                liInnerText = '<div class="checkbox-custom checkboxTiny checkbox-success">' + deleteIcon + addSubCharIcon
                    + '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="">'
                    //+ '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="ROSTemplateDetailRevamp.selectParentControls(this);ROSTemplateDetailRevamp.toggleCheckBoxes(this);">'
                    + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                    + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                    + '<textarea rows="1" spellcheck="true" id="txtName' + currentId + '" maxlength="150" onkeypress="ROSTemplateDetailRevamp.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
                    + '<div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodPE + '" class="btn btn-link btn-xs">'
                    + '<i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';

                var liTobeAdded = '<li id="' + currentId + '" ' + currentLiClass + ' parentid="' + currentParentId + '" onclick="' + onClick + '" value="' + currentId + '" refValue="' + currentId + '"' + subcharacteristicExist + '>' + liInnerText + '</li>';

                if (charSelectAll != null && charSelectAll.length > 0) {
                    $(liTobeAdded).insertAfter("#chkboxSelectAllChars");
                }
                else if (subCharSelectAll != null && subCharSelectAll.length > 0) {
                    $(liTobeAdded).insertAfter("#chkboxSelectAllSubChars");
                }
                else {
                    ulControl.prepend(liTobeAdded);
                }

                ulControl.find('li#' + currentId + ' #txtName' + currentId).focus()

            }

        }
    },

    AddSystemROS: function (obj, controlId) {

        var objData = new Object();
        objData["IsGlobal"] = false;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = true;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        ROSTemplateDetailRevamp.saveROSSystem_DBCall(objData).done(function (response) {

            response = JSON.parse(response);
            utility.DisplayMessages(response.Message, 1);
            if (response.status != false) {
                var li = ROSTemplateDetailRevamp.addSystem(response.ROSSystemId, objData["Name"]);
                $("#ulPhysicalExamSystems").append(li);

                //var objSelectedObservations = {
                //    ROSSystemId: response.ROSSystemId, IsChecked: false, ROSCharacteristicsId: '', ROSCharacteristicsName: '', IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0
                //};
                //ROSTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
                ROSTemplateDetailRevamp.loadCharatristics(response.ROSSystemId);
                $("#" + controlId).remove();
                ROSTemplateDetailRevamp.buildSystemsAutoComplete();
            }
            else {
                // response = JSON.parse(response);
                // utility.DisplayMessages(response.Message, 1);
            }
        });
    },

    AddChartristics: function (obj, controlId) {
        var ROSSystemId = $("#ulPhysicalExamSystems li.active")[0].id;
        var objData = new Object();
        objData["ROSSystemId"] = ROSSystemId;
        objData["Name"] = $("#txtName" + controlId).val();
        var CharatristicName = $("#txtName" + controlId).val();
        objData["IsActive"] = true;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        ROSTemplateDetailRevamp.saveROSCharatristics_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                // var res = JSON.parse(response.PEObservation_JSON);
                utility.DisplayMessages(response.Message, 1);
                if (response.status == true) {
                    ROSTemplateDetailRevamp.params.ROSCharacteristicsId = response.ROSCharacteristicsId;
                    var li = ROSTemplateDetailRevamp.addCharatristics(response.ROSCharacteristicsId, CharatristicName, ROSSystemId);
                    $("#ulPhysicalExamSystemSection").append(li);
                    var sysChecked = $("#chk" + $("#ulPhysicalExamSystems li.active").attr('id')).is(':checked');
                    var objSelectedObservations = {
                        ROSSystemId: ROSSystemId, IsChecked: false, ROSCharacteristicsId: response.ROSCharacteristicsId, ROSCharacteristicsName: CharatristicName, IsSystemChecked: sysChecked, IsSystemDeleted: 0, IsObservationDeleted: 0
                    };
                    ROSTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
                    $("#" + controlId).remove();
                }
            }
        });
    },

    saveDetailComments: function (event, obj) {
        if (event.which == 13) {
            event.preventDefault();
            if ($(obj).val() == '') {
                utility.DisplayMessages("Please enter some value", 3);
                return;
            }

            $(obj).focusout();
            $(obj).blur();

            this.currentIdOfText = $(obj).attr("id").replace("txtName", '');
            var onClickFunction = $(obj).parent().parent().find('.btn-link').attr("onclick");
            var ID = $(obj).parent().parent().find('.btn-link').attr("id");
            var ULID = $(obj).parent().parent().find('.btn-link').closest("ul").attr("id");
            onClickFunction = onClickFunction.replace('this', "$('#" + ROSTemplateDetailRevamp.params.PanelID + " #" + ULID + " #" + ID + "')");
            eval(onClickFunction);
        }
    },

    saveROSSystem_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "insert_reviewofsystem_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSSystem");
    },

    saveROSCharatristics_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "insert_reviewofsystem_charatristics_updatesystems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSCharatristics");
    },

    deleteItem: function (obj, ctrlId, currentId) {

        var itemId = $(obj).closest("li").attr('id');

        if (ctrlId == "ulPhysicalExamSystems") {
            $("#" + currentId).remove();

        } else if (ctrlId == "ulPhysicalExamSystemSection") {
            $("#" + currentId).remove();
        }
    },

    LoadProvider: function () {
        $("#ROSTemplateDetailRevamp #pnlLicenseDetail").removeClass('disableAll');
        if (ROSTemplateDetailRevamp.params.mode == "Add") {
            $('#ROSTemplateDetailRevamp #txtShortName').attr("enabled", "enabled");

            $("#ROSTemplateDetailRevamp #pnlLicenseDetail").addClass('disableAll');
            ROSTemplateDetailRevamp.ValidateProvider();

            //serialize Data after all controls loaded.
            $('#frmROSTemplateDetailRevamp').data('serialize', $('#frmROSTemplateDetailRevamp').serialize());

        }
        else if (ROSTemplateDetailRevamp.params.mode == "Edit") {
            $('#ROSTemplateDetailRevamp #txtShortName').attr("disabled", "disabled");
            ROSTemplateDetailRevamp.LoadProviderLicense().done(function (response) {
                if (response.status != false) {

                    ROSTemplateDetailRevamp.ProviderLicenseGridLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            ROSTemplateDetailRevamp.FillProvider(ROSTemplateDetailRevamp.params.ProviderId).done(function (response) {
                if (response.status != false) {
                    var provider_detail = JSON.parse(response.ProviderFill_JSON);
                    var self = $("#ROSTemplateDetailRevamp");
                    utility.bindMyJSON(true, provider_detail, false, self).done(function () {

                        if (provider_detail.chkActive == 'True')
                            $("#ROSTemplateDetailRevamp #chkActive").attr("checked", true);
                        else
                            $("#ROSTemplateDetailRevamp #chkActive").attr("checked", false);

                        if (provider_detail.chkSpecialist == 'True')
                            $("#ROSTemplateDetailRevamp #chkSpecialist").attr("checked", true);
                        else
                            $("#ROSTemplateDetailRevamp #chkSpecialist").attr("checked", false);

                        $("#ROSTemplateDetailRevamp #pnlLicenseDetail").removeClass('disableAll');

                        ROSTemplateDetailRevamp.ValidateProvider();
                        //serialize Data after all controls loaded.
                        $('#frmROSTemplateDetailRevamp').data('serialize', $('#frmROSTemplateDetailRevamp').serialize());

                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }
    },

    LoadEntityBasedData: function (entityID) {

        ROSTemplateDetailRevamp.LoadBasicFeeGroup(entityID).done(function () {

        });
        ROSTemplateDetailRevamp.LoadSupervisingProvider(entityID).done(function () {

        });
        ROSTemplateDetailRevamp.LoadSpecialty(entityID).done(function () {
            $('#frmROSTemplateDetailRevamp').bootstrapValidator('revalidateField', $('#frmROSTemplateDetailRevamp #ddlSpecialty').attr('name'));
        });
        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#tblROSTemplateDetailRevamp #ddlFeeGroup', 'GetFeeGroup', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#tblROSTemplateDetailRevamp #ddlFeeGroup', 'GetFeeGroup', true, null);
        }
    },

    LoadSupervisingProvider: function (entityID) {
        // Loads Entity Based Supervising Provider
        return ROSTemplateDetailRevamp.FillSupervisingProvider(entityID).done(function (response) {
            if (response.status != false) {
                var feegroup_detail = JSON.parse(response.SupervisingProviderLoad_JSON);
                $("#ROSTemplateDetailRevamp #ddlSupervisingProvider").empty();
                $("#ROSTemplateDetailRevamp #ddlSupervisingProvider").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(feegroup_detail, function (i, item) {
                    $("#ROSTemplateDetailRevamp #ddlSupervisingProvider").append(
                        $('<option/>', {
                            value: item.ProviderId,
                            html: item.ShortName
                        })
                    );
                });
            }

        });
    },

    LoadSpecialty: function (entityID) {
        if (entityID != null && entityID > 0) {

            // Loads Entity Based Specialty
            return ROSTemplateDetailRevamp.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {
                    var feegroup_detail = JSON.parse(response.SpecialtyLoad_JSON);
                    $("#ROSTemplateDetailRevamp #ddlSpecialty").empty();
                    $("#ROSTemplateDetailRevamp #ddlSpecialty").append($('<option/>', {
                        value: "",
                        html: "- SELECT -"
                    }));
                    $.each(feegroup_detail, function (i, item) {
                        $("#ROSTemplateDetailRevamp #ddlSpecialty").append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });
                }

            });
        }
    },

    validateSelectedTemplateData: function () {

        var isValid = true;

        if ($(ROSTemplateDetailRevamp.selectedPhyExamTempData).length > 0) {
            $.each(ROSTemplateDetailRevamp.selectedPhyExamTempData, function (i, item) {


                if (item.IsModified == "1" && $.parseJSON(item.IsChecked.toString().toLowerCase()) == true) {

                    //isValid = false;

                    //if () {

                    isValid = false;

                    if ($(item.Sections).length > 0) {

                        $.each(item.Sections, function (i, item) {

                            isValid = false;

                            if (item.IsModified == "1" && $.parseJSON(item.IsChecked.toString().toLowerCase()) == true) {


                                if ($(item.Characteristics).length > 0) {

                                    $.each(item.Characteristics, function (counter, item) {

                                        if (item.IsModified == "1" && $.parseJSON(item.IsChecked.toString().toLowerCase()) == true) {
                                            isValid = true;
                                        }
                                    });
                                }
                                else {
                                    isValid = false;
                                }
                            }
                        });
                    }
                    else {
                        isValid = false;
                    }
                    // }
                }
            });
        }
        else {
            isValid = false;
        }
        return isValid;
    },

    ROSTemplateSave: function () {
        // do caching 
        if (!$('#' + ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam div.toggle').hasClass('disableAll')) {
            ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(true);
        }

        var isValid = false;
        var self = null;
        self = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp');

        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        //objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = ROSTemplateDetailRevamp.specialityCheckedIds.join();
        //objData.ProviderIds = objData.PhysicalExamTemplateProvider = ROSTemplateDetailRevamp.providerCheckedIds.join();

        var isStillValid = false;

        //if (ROSTemplateDetailRevamp.validateSelectedTemplateData())
        //{
        myJSON = JSON.stringify(objData);
        ROSTemplateDetailRevamp.saveROSTemplate(myJSON).done(function (response) {
            if (response != null && response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    if (response.ROSTemplateId != "") {
                        ROSTemplateDetailRevamp.params.PhysicalExamTemplateId = response.ROSTemplateId;
                        for (var count in ROSTemplateDetailRevamp.selectedPhyExamTempData) {
                            ROSTemplateDetailRevamp.selectedPhyExamTempData[count];
                        }
                    }
                    //ROSTemplateDetailRevamp.loadHospitalizationHx();

                    ROSTemplateDetailRevamp.params.mode = "Edit";
                    //if (ROSTemplateDetailRevamp.params.ParentCtrl == "clinicalTabProgressNote" && UnloadHospitalizationhx == true) {
                    //    ROSTemplateDetailRevamp.getHospitalizationHxInfo(HospitalizationHxType, UnloadHospitalizationhx);
                    //}
                    //$('#' + ROSTemplateDetailRevamp.params.PanelID + " #hfHospitalizationHxId").val(response.HospitalizationHxId);
                    //$('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalHospitalizationHx').data('serialize', $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalHospitalizationHx').serialize());
                    //

                    // Empty global variables
                    ROSTemplateDetailRevamp.specialityCheckedIds = [];
                    ROSTemplateDetailRevamp.providerCheckedIds = [];
                    ROSTemplateDetailRevamp.providerSelectedIds = [];
                    ROSTemplateDetailRevamp.selectedPhyExamTempData = [];
                    ROSTemplateDetailRevamp.SpecialtyIds = "";
                    ROSTemplateDetailRevamp.ProviderIds = "";
                    ROSTemplateDetailRevamp.CharcSystemInfo = [];
                    ROSTemplateDetailRevamp.PositiveNegativeDetail = [];
                    ROSTemplateDetailRevamp.IsNornalAllSystems = [];
                    ROSTemplateDetailRevamp.ROSCharatristicsDetail = [];
                    ROSTemplateDetailRevamp.ROSSystemDetail = [];
                    ROSTemplateDetailRevamp.CharacteristicsDetailsInfo = [];
                    ROSTemplateDetailRevamp.SystemAllPositiveNegative = [];

                    //Start Farooq Ahmad 16-03-2016 Unload the Physical Exam on Save
                    UnloadActionPan(ROSTemplateDetailRevamp.params["ParentCtrl"], "ROSTemplateDetailRevamp");
                    if (ROSTemplateRevamp != null) {
                        ROSTemplateRevamp.loadROSTemplateMK();
                        ROSTemplateDetailRevamp.selectedObservations = [];
                    }
                    //End Farooq Ahmad 16-03-2016 Unload the Physical Exam on Save
                }
                else {
                    utility.DisplayMessages(response.Message, 3);

                    ROSTemplateDetailRevamp.ROSCharatristicsDetail = [];
                    ROSTemplateDetailRevamp.ROSSystemDetail = [];

                }
            }
        });
        //}
        //else {
        //    utility.DisplayMessages("Please select characteristic/Sub-characteristic", 3);
        //}
    },

    saveROSTemplate: function (PhysicalExamTemplateData, TemplateName) {
        var self = null, IsActive = null;
        self = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if (ROSTemplateDetailRevamp.params["mode"] == "Edit") {
            objData["TemplateId"] = (ROSTemplateDetailRevamp.params["PhysicalExamTemplateId"]);
        }
        else {
            objData["TemplateId"] = '-1';
        }
        objData["BodyPartId"] = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #ddlBodyParts').val();
        if (TemplateName == null || typeof (TemplateName) == "undefined") {

            var mainTemplateName = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #txtPhysicalExamTemplateName').val();

            if (objData["TemplateId"] == '-1') {
                objData["Name"] = TemplateName;
                IsActive = "1";
            }
            else {
                objData["Name"] = mainTemplateName != "" ? mainTemplateName : TemplateName;
            }

            objData["SaveAsName"] = TemplateName;
        }

        //objData["SpecialtyIds"] = objData["PhysicalExamTemplateSpecialty"];
        //objData["ProviderIds"] = objData["PhysicalExamTemplateProvider"];
        //objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = ROSTemplateDetailRevamp.specialityCheckedIds.join();
        //objData.ProviderIds = objData.PhysicalExamTemplateProvider = ROSTemplateDetailRevamp.providerCheckedIds.join();

        //------------------------------------------------------

        var SpecialtyIds = self.find('#ddlPhysicalExamTemplateSpecialty option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["SpecialtyIds"] = SpecialtyIds;

        var ProviderIds = self.find('#ddlPhysicalExamTemplateProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["ProviderIds"] = ProviderIds;

        var ROSCharaDetail = [];
        // Arrays For XML Detail
        var ROSCharaDetailGenral = [];
        arrPreviousHistory = [];
        arrROSCharacteristicsDetailStatusId = [];
        arrOnset = [];
        arrDuration = [];
        arrROSCharacteristicsDetailDurationI = [];
        arrROSCharacteristicsDetailPatternId = [];
        arrROSCharacteristicsDetailSeverityI = [];
        arrROSCharacteristicsDetailCourseId = [];
        arrROSCharacteristicsDetailRadiation = [];
        arrROSCharacteristicsDetailFrequency = [];
        arrROSCharacteristicsDetailContextId = [];
        arrROSCharacteristicsDetailCharacterCSZId = [];
        arrROSCharacteristicsDetailAggravedById = [];
        arrROSCharacteristicsDetailRelievedById = [];
        arrLocation = [];
        arrPrecipitatedBY = [];
        arrAssociatedWith = [];
        arrhfROSCharacteristicsDetailsId = [];

        //------------------------------------------------------
        // hijar
        for (var i = 0 ; i < ROSTemplateDetailRevamp.selectedObservations.length; i++) {
            if (ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted == 0 && ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted == 0) {

                indexesforchardetail = $.map(ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
                    if (item.Id == ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                        return index;
                    }
                })
                var detail = "";
                var details = "";
                if (indexesforchardetail.length > 0) {
                    var detialIndex = indexesforchardetail[0];
                    detail = ROSTemplateDetailRevamp.CharacteristicsDetailsInfo[detialIndex].DetailInfo;
                    details = JSON.parse(detail)
                }

                var IsCharPositive = null;
                var CharComments = null;

                indexesforcharposNeg = $.map(ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                    if (item.CharcId == ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                        return index;
                    }
                })

                if (indexesforcharposNeg.length > 0) {
                    var detialIndexForPostitiveNegative = indexesforcharposNeg[0];

                    if (ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsNegative == true) {
                        IsCharPositive = false;
                    }
                    else if (ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsPositive == true) {
                        IsCharPositive = true;
                    }
                    else {
                        IsCharPositive = null;
                    }
                    CharComments = ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].CharcComments;
                }

                if (details) {
                    var objSelectedObservationsWithDetailAll = {
                        ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,

                        ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                        PreviousHistory: details["PreviousHistory"],
                        ROSCharacteristicsDetailStatusId: details["ROSCharacteristicsDetailStatusId"],
                        Onset: details["Onset"],
                        Duration: details["Duration"],
                        ROSCharacteristicsDetailDurationId: details["ROSCharacteristicsDetailDurationId"],
                        ROSCharacteristicsDetailPatternId: details["ROSCharacteristicsDetailPatternId"],
                        ROSCharacteristicsDetailSeverityId: details["ROSCharacteristicsDetailSeverityId"],
                        ROSCharacteristicsDetailCourseId: details["ROSCharacteristicsDetailCourseId"],
                        ROSCharacteristicsDetailRadiationId: details["ROSCharacteristicsDetailRadiationId"],
                        ROSCharacteristicsDetailFrequencyId: details["ROSCharacteristicsDetailFrequencyId"],
                        ROSCharacteristicsDetailContextId: details["ROSCharacteristicsDetailContextId"],
                        ROSCharacteristicsDetailCharacterCSZId: details["ROSCharacteristicsDetailCharacterCSZId"],
                        ROSCharacteristicsDetailAggravedById: details["ROSCharacteristicsDetailAggravedById"],
                        ROSCharacteristicsDetailRelievedById: details["ROSCharacteristicsDetailRelievedById"],
                        Location: details["Location"],
                        PrecipitatedBY: details["PrecipitatedBY"],
                        AssociatedWith: details["AssociatedWith"],
                        hfROSCharacteristicsDetailsId: details["hfROSCharacteristicsDetailsId"]
                    };
                    var objSelectedObservations = {
                        ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                        IsChecked: ROSTemplateDetailRevamp.selectedObservations[i].IsChecked,
                        ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                        ROSCharacteristicsName: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsName,
                        IsSystemChecked: ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked,
                        IsSystemDeleted: ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted,
                        IsObservationDeleted: ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted,
                        IsPositive: IsCharPositive,
                        CharComments: CharComments,

                    };



                    ROSTemplateDetailRevamp.ROSCharatristicsDetail.push(objSelectedObservations);

                    if (details["PreviousHistory"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: $('#hfPreviousHistory').val(),

                            Value: details["PreviousHistory"],
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailStatusId"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: details["ROSCharacteristicsDetailStatusId"],
                            Value: "",
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["Onset"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: $('#hfROSOnset').val(),
                            Value: details["Onset"],
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["Duration"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,

                            LookupId: $('#hfROSDuration').val(),
                            Value: details["Duration"],
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailDurationId"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: details["ROSCharacteristicsDetailDurationId"],
                            Value: "",
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailPatternId"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: details["ROSCharacteristicsDetailPatternId"],
                            Value: "",
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailSeverityId"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: details["ROSCharacteristicsDetailSeverityId"],
                            Value: "",
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailCourseId"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: details["ROSCharacteristicsDetailCourseId"],
                            Value: "",
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailRadiationId"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: details["ROSCharacteristicsDetailRadiationId"],
                            Value: "",
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailFrequencyId"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: details["ROSCharacteristicsDetailFrequencyId"],
                            Value: "",
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailContextId"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: details["ROSCharacteristicsDetailContextId"],
                            Value: "",
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailCharacterCSZId"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: details["ROSCharacteristicsDetailCharacterCSZId"],
                            Value: "",
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailAggravedById"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: details["ROSCharacteristicsDetailAggravedById"],
                            Value: "",
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailRelievedById"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            LookupId: details["ROSCharacteristicsDetailRelievedById"],
                            Value: "",
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["Location"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,

                            LookupId: $('#hfLocation').val(),
                            Value: details["Location"],
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["PrecipitatedBY"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,

                            LookupId: $('#hfPrecipitatedBy').val(),
                            Value: details["PrecipitatedBY"],
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["AssociatedWith"] != "") {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,

                            LookupId: $('#hfAssociatedBy').val(),
                            Value: details["AssociatedWith"],
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }

                }
                else {
                    var objSelectedObservations = {
                        ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                        IsChecked: ROSTemplateDetailRevamp.selectedObservations[i].IsChecked,
                        ROSCharacteristicsId: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                        ROSCharacteristicsName: ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsName,
                        IsSystemChecked: ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked,
                        IsSystemDeleted: ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted,
                        IsObservationDeleted: ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted,
                        IsPositive: IsCharPositive,
                        CharComments: CharComments,

                    };


                    ROSTemplateDetailRevamp.ROSCharatristicsDetail.push(objSelectedObservations);

                }





                indexIsSystemExist = $.map(ROSTemplateDetailRevamp.ROSSystemDetail, function (item, index) {
                    if (item.ROSSystemId == ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                        return index;
                    }
                })
                if (indexIsSystemExist.length == 0) {
                    indexesforisnormalsys = $.map(ROSTemplateDetailRevamp.IsNornalAllSystems, function (item, index) {
                        if (item.SystemId == ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                            return index;
                        }
                    })
                    var IsNormalSystem = 'false';
                    var IsNormalComments = null;
                    if (indexesforisnormalsys.length > 0) {
                        var indexforIsNormal = indexesforisnormalsys[0];

                        if (ROSTemplateDetailRevamp.IsNornalAllSystems[indexforIsNormal].IsNormal == true) {
                            IsNormalSystem = "true";
                        }

                        IsNormalComments = ROSTemplateDetailRevamp.IsNornalAllSystems[indexforIsNormal].IsNormalComments;
                    }

                    var objSystemWithDetail = {
                        ROSSystemId: ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                        IsChecked: ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked,
                        IsNormalComments: IsNormalComments,
                        IsNormal: IsNormalSystem,
                    };
                    objSystemWithDetail.RosChartristicsDetail = new Array();
                    //for (i = 0; i < 5; i++) {
                    //    var obj1 = new Object();
                    //    obj1.pro1 = "1";
                    //    obj1.pro2 = "2";
                    //    obj1.pro3 = "3";
                    //    objSystemWithDetail.objSystemWithDetailarr.push(objSelectedObservationsWithDetail);
                    //}
                    objSystemWithDetail.RosChartristicsDetail.push(objSelectedObservations);
                    ROSTemplateDetailRevamp.ROSSystemDetail.push(objSystemWithDetail);

                }
                else {

                    objSystemWithDetail.RosChartristicsDetail.push(objSelectedObservations);
                }

            }
        }



        // For empty System
        if (ROSTemplateDetailRevamp.ROSSystemDetail.length != $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems   li').length) {
            var listofsystems = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li')
            var tempROSSystemDetail = [];
            tempROSSystemDetail = ROSTemplateDetailRevamp.ROSSystemDetail;
            ROSTemplateDetailRevamp.ROSSystemDetail = [];

            for (var i = 0 ; i < listofsystems.length; i++) {

                // for (var j = 0 ; j < ROSTemplateDetailRevamp.ROSSystemDetail.length; j++) {
                var ss = tempROSSystemDetail.filter(function (obj) {
                    return obj.ROSSystemId == listofsystems[i].id;
                });
                if (ss.length == 0) {
                    var objSystemWithDetail = {
                        ROSSystemId: listofsystems[i].id,
                        IsChecked: false,
                        IsNormalComments: null,
                        IsNormal: 'false',
                    };

                    ROSTemplateDetailRevamp.ROSSystemDetail.push(objSystemWithDetail);
                }
                else
                    ROSTemplateDetailRevamp.ROSSystemDetail.push(ss[0]);
                //  }
            }


        }



        //------------------------------------------------------


        //------------------------------------------------------//
        //---------------------detail values--------------------//


        //objData["objPreviousHistory"] = arrPreviousHistory;
        //objData["objROSCharacteristicsDetailStatusId"] = arrROSCharacteristicsDetailStatusId;
        //objData["objOnset"] = arrOnset;
        //objData["objDuration"] = arrDuration;
        //objData["objROSCharacteristicsDetailDurationI"] = arrROSCharacteristicsDetailDurationI;
        //objData["objROSCharacteristicsDetailPatternId"] = arrROSCharacteristicsDetailPatternId;
        //objData["objROSCharacteristicsDetailSeverityI"] = arrROSCharacteristicsDetailSeverityI;
        //objData["objROSCharacteristicsDetailCourseId"] = arrROSCharacteristicsDetailCourseId;
        //objData["objROSCharacteristicsDetailRadiation"] = arrROSCharacteristicsDetailRadiation;
        //objData["objROSCharacteristicsDetailFrequency"] = arrROSCharacteristicsDetailFrequency;
        //objData["objROSCharacteristicsDetailContextId"] = arrROSCharacteristicsDetailContextId;
        //objData["objROSCharacteristicsDetailCharacterCSZId"] = arrROSCharacteristicsDetailCharacterCSZId;
        //objData["objROSCharacteristicsDetailAggravedById"] = arrROSCharacteristicsDetailAggravedById;
        //objData["objROSCharacteristicsDetailRelievedById"] = arrROSCharacteristicsDetailRelievedById;
        //objData["objLocation"] = arrLocation;
        //objData["objPrecipitatedBY"] = arrPrecipitatedBY;
        objData["ROSCharaDetailGenral"] = ROSCharaDetailGenral;


        //--------------------end detail values------------------//
        objData["commandType"] = "insert_reviewofsystem_template";
        //objData["SystemObservationData"] = ROSTemplateDetailRevamp.selectedObservations;
        // objData["SystemChartristicsData"] = ROSTemplateDetailRevamp.ROSCharatristicsDetail;
        // objData["TemplatePreview"] = $("#observationContent").text();
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["SystemChartristicsdetail"] = ROSCharaDetail;
        objData["ROSSystems"] = ROSTemplateDetailRevamp.ROSSystemDetail;
        objData["Comments"] = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #txtComments').val();
        objData["Name"] = $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp #txtPhysicalExamTemplateName').val();


        IsActive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }

        objData["IsActive"] = IsActive;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ReviewofSystemRevampTemplate");
    },

    validateROSTemplate: function () {
        $('#' + ROSTemplateDetailRevamp.params.PanelID + ' #frmROSTemplateDetailRevamp')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   PhysicalExamTemplateName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PhysicalExamTemplateEntity: {
                       group: '.col-sm-3',
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
            ROSTemplateDetailRevamp.ROSTemplateSave();
        });
    },

    saveAsROSTemplate: function () {
        var strMessage = "";
        var params = [];
        params["FromAdmin"] = '0';
        params["TabID"] = "PhysicalExamTemplateSaveAs";
        params["ParentCtrl"] = 'ROSTemplateDetailRevamp';


        LoadActionPan('PhysicalExamTemplateSaveAs', params, ROSTemplateDetailRevamp.params.PanelID);

    },

    FillSupervisingProvider: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "FILL_SUPERVISING_PROVIDER");
    },

    FillSpecialty: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "FILL_SPECIALTY");
    },

    UnLoad: function () {

        // Empty global variables
        ROSTemplateDetailRevamp.specialityCheckedIds = [];
        ROSTemplateDetailRevamp.providerCheckedIds = [];
        ROSTemplateDetailRevamp.providerSelectedIds = [];
        ROSTemplateDetailRevamp.selectedPhyExamTempData = [];
        ROSTemplateDetailRevamp.SpecialtyIds = "";
        ROSTemplateDetailRevamp.ProviderIds = "";
        ROSTemplateDetailRevamp.CharcSystemInfo = [];
        ROSTemplateDetailRevamp.PositiveNegativeDetail = [];
        ROSTemplateDetailRevamp.IsNornalAllSystems = [];
        ROSTemplateDetailRevamp.ROSCharatristicsDetail = [];
        ROSTemplateDetailRevamp.ROSSystemDetail = [];
        ROSTemplateDetailRevamp.CharacteristicsDetailsInfo = [];
        ROSTemplateDetailRevamp.SystemAllPositiveNegative = [];

        utility.UnLoadDialog("frmROSTemplateDetailRevamp", function () {

            UnloadActionPan(ROSTemplateDetailRevamp.params["ParentCtrl"], "ROSTemplateDetailRevamp");
            if (PhysicalExamTemplate != null) {
                ROSTemplateRevamp.loadROSTemplateMK();
                ROSTemplateDetailRevamp.selectedObservations = [];
                ROSTemplateDetailRevamp.specialtyIdMKMK = [];
                ROSTemplateDetailRevamp.ProviderIdMKMK = [];
            }
        }, function () {

            UnloadActionPan(ROSTemplateDetailRevamp.params["ParentCtrl"], "ROSTemplateDetailRevamp");
        });
        if (ROSTemplateDetailRevamp.params.ParentCtrl == "EncounterChargeCapture") {
            var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
            var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
            EditableGrid.initialize(ChargeGridId, EncounterChargeCapture, "0", false, false, false, false, undefined, false);
        }

    },

    bindCharacteristicsAutoComplete: function () {
        ROSTemplateDetailRevamp.lookupCharacteristics_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Observation_JSON = JSON.parse(response.ObservationLoad_JSON);
                    $('#' + ROSTemplateDetailRevamp.params.PanelID + " #txtCharacteristic").kendoAutoComplete({
                        dataSource: Observation_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Characteristic...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulPhysicalExamSystemSection").find('#' + dataItem.ROSCharacteristicsId).length == 0) {
                                var ROSSystemId = $("#ulPhysicalExamSystems li.active")[0].id;
                                var li = ROSTemplateDetailRevamp.addCharatristics(dataItem.ROSCharacteristicsId, dataItem.Name, ROSSystemId);
                                $("#ulPhysicalExamSystemSection").append(li);
                                var objSelectedObservations = {
                                    ROSSystemId: ROSSystemId, IsChecked: false, ROSCharacteristicsId: dataItem.ROSCharacteristicsId, ROSCharacteristicsName: dataItem.Name, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0
                                };
                                ROSTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
                                $("#txtCharacteristic").val('');
                            }
                            else {
                                utility.DisplayMessages("A characteristic with same name already exists.", 3);
                                $("#txtCharacteristic").val('');
                            }
                            e.preventDefault();
                        },
                    });
                    $("#txtCharacteristic").parent().addClass('size100per');
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    lookupCharacteristics_DBCall: function () {
        var objData = new Object();
        objData["IsActive"] = 1;
        objData["commandType"] = "lookup_reviewofsystem_charatristics"; /* lookup_physicalexam_observation */
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSCharatristics");
    },

    CharacteristicsAlreadyExists: function (CharId, CharName) {

    },
}