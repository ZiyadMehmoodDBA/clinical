Clinical_ROSTemplateDetailRevamp = {
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
    TemplateId: "",
    ROSTempSysCharDetailOldLookupValues: {},
    Load: function (params) {
        $('#ListSection').addClass('hidden');
        Clinical_ROSTemplateDetailRevamp.params = params;
        Clinical_ROSTemplateDetailRevamp.params.PanelID = "pnlClinicalROSTemplateDetailRevamp";
        Clinical_ROSTemplateDetailRevamp.selectedObservations = [];


        var self = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID);
    
        if (Clinical_ROSTemplateDetailRevamp.bIsFirstLoad == true) {
            Clinical_ROSTemplateDetailRevamp.loadROSTemplateMK();
        }
   
        if (Clinical_ROSTemplateDetailRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_ROSTemplateDetailRevamp.params.PanelID, 'Medical', 'Clinical_ROSTemplateDetailRevamp', 'Clinical_ROSTemplateDetailRevamp.UnLoadTab();', 'frmPhysicalExamTemplate');
        }
        if (Clinical_ROSTemplateDetailRevamp.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " div#FaceSheetPager", Clinical_ROSTemplateDetailRevamp.params.FaceSheetComponents, 'problem list');
        }

  
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #pnlProvider_Result #dgvProvider').DataTable({
            "language": {
                "emptyTable": "No Record Found."
            }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
        });
       

        
        var HtmlOfSwitch = '<div id="divSwitch"><span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                         '<input id="switchActive" isactive="1" type="checkbox" checked="checked" name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="">' +
                          '</div><span class="pl-xs">Active</span> </div>';

        $("#" + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' .datatables-header div:first').html(HtmlOfSwitch);
        Clinical_ROSTemplateDetailRevamp.readyFunction();
       
        // for detail section
        Clinical_ROSTemplateDetailRevamp.loadLookupsandSys("Add");
        if (Clinical_ROSTemplateDetailRevamp.params.isShowTemplate == false)
        {
            $('#templatesection').removeClass('hidden');
            $('#btnPhysicalExamSave').parent().removeClass('hidden');
            Clinical_ROSTemplateDetailRevamp.domReadyFunction(true);
            setTimeout (function (){
                utility.SelectGridRow($('#gvTemplates_row' + Clinical_ROSTemplateDetailRevamp.params.ROSTemplateId));
                $('#frmClinicalROSTemplateDetailRevamp').data('serialize', $('#frmClinicalROSTemplateDetailRevamp').serialize());
            }, 1000);
            Clinical_ROSTemplateDetailRevamp.LoadROS(Clinical_ROSTemplateDetailRevamp.params.ROSTemplateId);
        }
        else
        {
            $('#ListSection').removeClass('hidden'); 
        }
    },
    showROSTemplatesTab:function()
    {
        $("#ListSection").removeClass('hidden');
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #btnPhysicalExamSave ').parent().addClass('hidden');
        $("#templatesection").addClass('hidden');
    
    },
    loadLookupsandSys: function(mode)
    {
        Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData = [];
        if (Clinical_ROSTemplateDetailRevamp.params.ComeFrom == 'ROSSystemDetail') {
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #Systems').hide();
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divAddNewSystem').hide();
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #LiROSTemplates').hide();
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divNormalGlobalParent').addClass('hidden');
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #spacer10').remove();
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #spacer20').remove();
        }
        else {
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #LiROSTemplates').show();
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divNormalGlobalParent').removeClass('hidden');
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #Systems').show();
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divAddNewSystem').show();
            Clinical_ROSTemplateDetailRevamp.buildSystemsAutoComplete();
        }
        Clinical_ROSTemplateDetailRevamp.bindCharacteristicsAutoComplete();
        Clinical_ROSTemplateDetailRevamp.ROSLookUps();
        Clinical_ROSTemplateDetailRevamp.validateROSTemplate();
        var $dtReviewofSystemsDate = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + '  #dtReviewofSystemsDate');
        if ($dtReviewofSystemsDate.val() == '') {
            $dtReviewofSystemsDate.datepicker("setDate", new Date());
        } else {
            $dtReviewofSystemsDate.datepicker("setDate", new Date($dtReviewofSystemsDate.val()));
        }
    },
    readyFunction: function () {

        (function ($) {
            'use strict';
            $(function () {
                $('[data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },
    loadDropDowns: function (dfd) {
        var self = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #tblClinical_ROSTemplateDetailRevamp');
        if (globalAppdata.AppUserName.toLowerCase() != DefaultUser.toLowerCase())
        {
            self.find("#ddlPhysicalExamTemplateEntity").val(globalAppdata["SeletedEntityId"]);
            if (self.find("div#divEntity").hasClass("hidden") == false)
                self.find("div#divEntity").addClass("hidden");
            isSelectedEntity = true;
        }
        else {
            self.find("div#divEntity").removeClass("hidden");
        }
        Clinical_ROSTemplateDetailRevamp.loadEntitySpecialty(globalAppdata["SeletedEntityId"], dfd);
        if (Clinical_ROSTemplateDetailRevamp.params.Title != null)
            $("#" + Clinical_ROSTemplateDetailRevamp.params["PanelID"] + " #headingTitle").text(Clinical_ROSTemplateDetailRevamp.params.Title);
    },

    loadEntitySpecialtyMK: function (entityID) {
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty_').empty();
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
                            Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = [];
                            var dataItem = this.dataItem(e.item.index());
                            Clinical_ROSTemplateDetailRevamp.specialityCheckedIds.push(dataItem.id);
                            Clinical_ROSTemplateDetailRevamp.loadSpecialityProviders(dataItem.id);
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
        if (Clinical_ROSTemplateDetailRevamp.specialityCheckedIds.length > 0) {
            var objData = new Object();
            objData["IsActive"] = true;
            objData["SpecialtyIds"] = SpecialityId;

            Clinical_ROSTemplateDetailRevamp.GetSpecialityProvider_DBCall(objData).done(function (response) {
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
    //                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').empty();
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

    LoadROS: function (ROSTemplateId) {
        $("#observationContent").text('');
        $("#SystemPreview").removeClass('hidden');
        Clinical_ROSTemplateDetailRevamp.LoadROSTempSysCharateristics(ROSTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.PETemplateCount > 0) {
                    var res = JSON.parse(response.PETemplate_JSON);
                    var templateData = JSON.parse(res);
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #txtPhysicalExamTemplateName").val(templateData[0].TemplateName);
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #observationContent").text(templateData[0].TemplatePreview);
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #txtComments").text(templateData[0].TemplateComments);
                    if (templateData[0].IsNormalAll == "True") {
                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #chkReviewofSystemssNormal').prop('checked', true);
                    } else {

                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #chkReviewofSystemssNormal').prop('checked', false);
                    }

                    //if (templateData[0].SpecialityIds != "") {
                    //    //if (Clinical_ROSTemplateDetailRevamp.ProviderIdMKMK.length > 0) {
                    //    EMRUtility.selectOptionsByCommaSeprateValue($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider'), templateData[0].SpecialityIds);
                    //    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect("refresh");
                    //    //}
                    //    //Clinical_ROSTemplateDetailRevamp.specialtyIdMKMK = templateData[0].SpecialityIds

                    //}
                    //if (templateData[0].ProviderIds != "") {
                    //    //Clinical_ROSTemplateDetailRevamp.ProviderIdMKMK = templateData[0].ProviderIds
                    //    //if (Clinical_ROSTemplateDetailRevamp.specialtyIdMKMK.length > 0) {
                    //    EMRUtility.selectOptionsByCommaSeprateValue($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty'), templateData[0].ProviderIds);
                    //    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect("refresh");
                    //    //}
                    //}


                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('clearSelection', false);
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('updateButtonText');
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #ddlPhysicalExamTemplateProvider").val(templateData[0]['ProviderIds'].split(','));
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect("refresh");
                    Clinical_ROSTemplateDetailRevamp.providerCheckedIds = templateData[0]['ProviderIds'].split(',');

                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('clearSelection', false);
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('updateButtonText');
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #ddlPhysicalExamTemplateSpecialty").val(templateData[0]['SpecialtyIds'].split(','));
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect("refresh");
                    Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = templateData[0]['SpecialtyIds'].split(',');

                }

                if (response.PETemplateSystemsCount > 0) {
                    $("#observationContent div").remove();
                    $("#ulPhysicalExamSystems li").remove();
                    var resTemplateSystems = JSON.parse(response.PETemplateSystems_JSON);
                    var SystemData = JSON.parse(resTemplateSystems);

                    for (var i = 0; i < SystemData.length; i++) {
                        if (SystemData[i].ROSSystemId != "") {
                            var li = Clinical_ROSTemplateDetailRevamp.addSystem(SystemData[i].ROSSystemId, SystemData[i].SystemName);
                            $("#ulPhysicalExamSystems").append(li);

                            if (SystemData[i].IsSelectedSystem == "True") {
                                $("#ulPhysicalExamSystems #chk" + SystemData[i].ROSSystemId).prop("checked", true);
                            }

                            if (response.PESysObservationsCount > 0) {
                                var resTemplateObservations = JSON.parse(response.PESysObservations_JSON);
                                var ObservationData = JSON.parse(resTemplateObservations);

                                for (var j = 0; j < ObservationData.length; j++) {
                                    if (SystemData[i].ROSSystemId == ObservationData[j].ROSSystemId) {
                                        var li = Clinical_ROSTemplateDetailRevamp.addCharatristics(ObservationData[j].ROSCharacteristicsId, ObservationData[j].CharacteristicsName, ObservationData[j].ROSSystemId);
                                        $("#ulPhysicalExamSystemSection").append(li);

                                        var objSelectedObservations = {
                                        };
                                        if (ObservationData[j].TempSysCharIsSelected == "True") {
                                            $("#ulPhysicalExamSystemSection #chk" + ObservationData[j].ROSCharacteristicsId).prop("checked", true);
                                            if (SystemData[i].IsSelectedSystem == "True") {
                                                objSelectedObservations = {
                                                    ROSSystemId: ObservationData[j].ROSSystemId, IsChecked: true, ROSCharacteristicsId: ObservationData[j].ROSCharacteristicsId, ROSCharacteristicsName: ObservationData[j].CharacteristicsName, IsSystemChecked: true, IsSystemDeleted: 0, IsObservationDeleted: 0, SystemName: SystemData[i].SystemName,
                                                };
                                            }
                                            else {
                                                objSelectedObservations = {
                                                    ROSSystemId: ObservationData[j].ROSSystemId, IsChecked: true, ROSCharacteristicsId: ObservationData[j].ROSCharacteristicsId, ROSCharacteristicsName: ObservationData[j].CharacteristicsName, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0, SystemName: SystemData[i].SystemName,
                                                };
                                            }

                                            //Clinical_ROSTemplateDetailRevamp.handleDelimiter(ObservationData[j].ROSSystemId, ObservationData[j].ROSCharacteristicsId, ObservationData[j].ROSCharacteristicsName, true);
                                        }
                                        else {
                                            $("#ulPhysicalExamSystemSection #chk" + ObservationData[j].ROSCharacteristicsId).prop("checked", false);
                                            if (SystemData[i].IsSelectedSystem == "True") {
                                                objSelectedObservations = {
                                                    ROSSystemId: ObservationData[j].ROSSystemId, IsChecked: false, ROSCharacteristicsId: ObservationData[j].ROSCharacteristicsId, ROSCharacteristicsName: ObservationData[j].CharacteristicsName, IsSystemChecked: true, IsSystemDeleted: 0, IsObservationDeleted: 0, SystemName: SystemData[i].SystemName,
                                                };
                                            }
                                            else {
                                                objSelectedObservations = {
                                                    ROSSystemId: ObservationData[j].ROSSystemId, IsChecked: false, ROSCharacteristicsId: ObservationData[j].ROSCharacteristicsId, ROSCharacteristicsName: ObservationData[j].CharacteristicsName, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0, SystemName: SystemData[i].SystemName,
                                                };
                                            }

                                            //Clinical_ROSTemplateDetailRevamp.handleDelimiter(ObservationData[j].ROSSystemId, ObservationData[j].ROSCharacteristicsId, ObservationData[j].ROSCharacteristicsName, false);
                                        }


                                        Clinical_ROSTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
                                        // caching the detail of charateristics
                                        var chardetail = JSON.parse(response.ROSSysCharaDetail);
                                        chardetail = JSON.parse(chardetail);
                                        if (chardetail.length > 0) {
                                            var ind = false;
                                            var detail = {
                                                PreviousHistory: "",
                                                ROSCharacteristicsDetailStatusId: "",
                                                ROSCharacteristicsDetailStatusId: "",
                                                Onset: "",
                                                Duration: "",
                                                ROSCharacteristicsDetailDurationId: "",
                                                ROSCharacteristicsDetailDurationId_text: "",
                                                ROSCharacteristicsDetailPatternId: "",
                                                ROSCharacteristicsDetailPatternId_text: "",
                                                ROSCharacteristicsDetailSeverityId: "",
                                                ROSCharacteristicsDetailSeverityId_text: "",
                                                ROSCharacteristicsDetailCourseId: "",
                                                ROSCharacteristicsDetailCourseId_text: "",
                                                ROSCharacteristicsDetailRadiationId: "",
                                                ROSCharacteristicsDetailRadiationId_text: "",
                                                ROSCharacteristicsDetailFrequencyId: "",
                                                ROSCharacteristicsDetailFrequencyId_text: "",
                                                ROSCharacteristicsDetailContextId: "",
                                                ROSCharacteristicsDetailContextId_text: "",
                                                ROSCharacteristicsDetailCharacterCSZId: "",
                                                ROSCharacteristicsDetailCharacterCSZId_text: "",
                                                ROSCharacteristicsDetailAggravedById: "",
                                                ROSCharacteristicsDetailAggravedById_text: "",
                                                ROSCharacteristicsDetailRelievedById: "",
                                                ROSCharacteristicsDetailRelievedById_text: "",
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
                                                        detail.ROSCharacteristicsDetailStatusId_text = chardetail[z].LookupName;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailDuration") {
                                                        detail.ROSCharacteristicsDetailDurationId = chardetail[z].LookUpId;
                                                        detail.ROSCharacteristicsDetailDurationId_text = chardetail[z].LookupName;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailPattern") {
                                                        detail.ROSCharacteristicsDetailPatternId = chardetail[z].LookUpId;
                                                        detail.ROSCharacteristicsDetailPatternId_text = chardetail[z].LookupName;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailSeverity") {
                                                        detail.ROSCharacteristicsDetailSeverityId = chardetail[z].LookUpId;
                                                        detail.ROSCharacteristicsDetailSeverityId_text = chardetail[z].LookupName;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailCourse") {
                                                        detail.ROSCharacteristicsDetailCourseId = chardetail[z].LookUpId;
                                                        detail.ROSCharacteristicsDetailCourseId_text = chardetail[z].LookupName;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailRadiation") {
                                                        detail.ROSCharacteristicsDetailRadiationId = chardetail[z].LookUpId;
                                                        detail.ROSCharacteristicsDetailRadiationId_text = chardetail[z].LookupName;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailFrequency") {
                                                        detail.ROSCharacteristicsDetailFrequencyId = chardetail[z].LookUpId;
                                                        detail.ROSCharacteristicsDetailFrequencyId_text = chardetail[z].LookupName;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailContext") {
                                                        detail.ROSCharacteristicsDetailContextId = chardetail[z].LookUpId;
                                                        detail.ROSCharacteristicsDetailContextId_text = chardetail[z].LookupName;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailCharacterCSZ") {
                                                        detail.ROSCharacteristicsDetailCharacterCSZId = chardetail[z].LookUpId;
                                                        detail.ROSCharacteristicsDetailCharacterCSZId_text = chardetail[z].LookupName;
                                                    }

                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailAggravedBy") {
                                                        detail.ROSCharacteristicsDetailAggravedById = chardetail[z].LookUpId;
                                                        detail.ROSCharacteristicsDetailAggravedById_text = chardetail[z].LookupName;
                                                    }
                                                    if (chardetail[z].LookupTypeName == "ROSCharacteristicsDetailRelievedBy") {
                                                        detail.ROSCharacteristicsDetailRelievedById = chardetail[z].LookUpId;
                                                        detail.ROSCharacteristicsDetailRelievedById_text = chardetail[z].LookupName;
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
                                                Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.push(CharsDetailInfo);
                                            }

                                        }


                                        // var DetailData = JSON.stringify(ObservationData[j])
                                        //DetailData = '{' +DetailData.substring(DetailData.indexOf('"PreviousHistory":'), DetailData.indexOf(',"ROSTempSysCharId":')) + '}';

                                        //Clinical_ROSTemplateDetailRevamp.populateDetailAgainstCharacteristic(ObservationData[j]);
                                        // Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.push(CharsDetailInfo);
                                        //Clinical_ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(false);
                                        var isPositive = false;
                                        var IsNegative = false;
                                        if (ObservationData[j].isPositive == "True") {
                                            isPositive = true;
                                            IsNegative = false;
                                            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').addClass('green');
                                        }
                                        else if (ObservationData[j].isPositive == "False") {
                                            isPositive = false;
                                            IsNegative = true;
                                            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').addClass('green');
                                        }
                                        var CharsPosNegDetail = {
                                            Id: ObservationData[j].ROSSystemId + '-' + ObservationData[j].ROSCharacteristicsId,
                                            SystemId: ObservationData[j].ROSSystemId,
                                            CharcId: ObservationData[j].ROSCharacteristicsId,
                                            CharcComments: ObservationData[j].TempSysCharComments,
                                            IsPositive: isPositive,
                                            IsNegative: IsNegative

                                        };
                                        Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail.push(CharsPosNegDetail);
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
                                Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems.push(SystemIsNormal);
                                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #btnOpenDetailIsNormalAll').removeClass('disableAll');

                            }
                            var IsSystemPositive = null;
                            var IsSystemNegative = null;
                            if (SystemData[i].IsSystemSelectAll == "True") {
                                IsSystemPositive = true;
                                IsSystemNegative = false;
                                //  $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').('green');
                                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                            }
                            else if (SystemData[i].IsSystemSelectAll == "False") {
                                IsSystemPositive = false;
                                IsSystemNegative = true;
                                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
                                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').addClass('red');
                            }
                            else {
                                IsSystemPositive = null;
                                IsSystemNegative = null;
                                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
                                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                            }
                            var IsPosNegAll = {
                                SystemId: SystemData[i].ROSSystemId,
                                IspositiveAll: IsSystemPositive,
                                IsnegativeAll: IsSystemNegative
                            };
                            Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative.push(IsPosNegAll);

                        }
                    }
                    if (SystemData[0].ROSSystemId) {
                        $('#ulPhysicalExamSystems #' + SystemData[0].ROSSystemId).click();
                    }
                    if ($('#templatesection').hasClass('hidden'))
                    {
                    $('#ListSection').addClass('hidden');
                    
                    $('#templatesection').removeClass('hidden');
                    $('#btnPhysicalExamSave').parent().removeClass('hidden');
                    Clinical_ROSTemplateDetailRevamp.domReadyFunction();
                    }
                    Clinical_ROSTemplateDetailRevamp.domReadyFunction();
                    
                }
                $('#frmClinicalROSTemplateDetailRevamp').data('serialize', $('#frmClinicalROSTemplateDetailRevamp').serialize());
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
        objData["NoteId"] = Clinical_ROSTemplateDetailRevamp.params.NotesId;
        if (Clinical_ROSTemplateDetailRevamp.params.ComeFrom == 'ROSSystemDetail') {
            objData["ROSSystemId"] = Clinical_ROSTemplateDetailRevamp.params.ROSSystemId;
        }
        if (Clinical_ROSTemplateDetailRevamp.params.LoadFromNote == "LoadFromNote")
        {
            Clinical_ROSTemplateDetailRevamp.params.LoadFromNote = "";
            objData["commandType"] = "select_reviewofsystem_template_note";
        } else {
            objData["commandType"] = "select_reviewofsystem_template";//select_reviewofsystem_template
        }
        
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ReviewofSystemRevampTemplate");
    },

    addSystem: function (ROSSystemId, SystemName) {
        var itemtoRemove = "system";

        for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
            if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId && Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted != 1) {
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

        var li = '<li id="' + ROSSystemId + '" parentid="null" onclick="Clinical_ROSTemplateDetailRevamp.loadCharatristics(' + ROSSystemId + ')" value="' + ROSSystemId + '" refvalue="" subcharacteristicexist=" " class="">' +
                // '<input type="checkbox" id="chk' + ROSSystemId + '" name="' + ROSSystemId + '" class="pull-left  char" onclick="Clinical_ROSTemplateDetailRevamp.ManageCharateristics(' + ROSSystemId + ', this);">' +
                 
                 '<label id="lblName' + ROSSystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SystemName + '">' +
                 '<span><b><a style="color:black !important" href="javascript:void(0);" onclick="Clinical_ROSTemplateDetailRevamp.ClearInfo(event, this,' + ROSSystemId + ',\'' + SystemName + '\');"  > <span title="Reset" class="glyphicon glyphicon-refresh">&nbsp</span> </a></b></span>' +
                 '' + SystemName + '</label><div id="divNameDetail' + ROSSystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + ROSSystemId + '" onkeypress="" name="Name' + ROSSystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 ROSSystemId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover hidden" onclick="Clinical_ROSTemplateDetailRevamp.removeSystem(' + ROSSystemId + ', event)";><i class="fa fa-close"></i></span></a></li>';
        // Clinical_ROSTemplateDetailRevamp.removeSystem(' + ROSSystemId + ')  
        return li;
    },

    addCharatristics: function (ROSCharacteristicsId, CharatristicName, ROSSystemId) {
        var a = Clinical_ROSTemplateDetailRevamp.selectedObservations;

        var itemtoRemove = "observation";

        var li = '<li id="' + ROSCharacteristicsId + '" parentid="' + ROSSystemId + '" onclick="Clinical_ROSTemplateDetailRevamp.loadSysPatCharcDetail(this);" value="' + ROSCharacteristicsId + '" refvalue="" subcharacteristicexist=" " class="">' +
            
            
                // '<input type="checkbox" id="chk' + ROSCharacteristicsId + '" name="' + ROSCharacteristicsId + '" class="pull-left  char" ' +
                // 'onclick="Clinical_ROSTemplateDetailRevamp.PreviewCharateristics(' + ROSCharacteristicsId + ',\'' + CharatristicName + '\', this, ' + ROSSystemId + ');">' +
                 '<label id="lblName' + ROSCharacteristicsId + '" class="" data-toggle="tooltip" title="" data-original-title="' + CharatristicName + '">' +
                '<input type="hidden" id="isSystemNormal" name="isSystemNormal" value="" /><input type="hidden" id="systemNormalDescription" name="systemNormalDescription" value="" /><input type="hidden" id="systemPatientID" name="systemPatientID" value="" /><input type="hidden" id="isCharacteristicsExists" name="isCharacteristicsExists" value="false"" />' +
                 '<button type="button" id="btnOpenDetail' + ROSCharacteristicsId + '"  data-toggle="tooltip" data-placement="top"  onclick="Clinical_ROSTemplateDetailRevamp.openCharacteristicDetail(this,' + ROSCharacteristicsId + ');" class="btn btn-link btn-xs pull-left"><i class="fa fa-book"></i></button>' +
                 //'<span><a href="javascript:void(0);" onclick="Clinical_ROSTemplateDetailRevamp.openCharacteristicDetail(this,' + ROSCharacteristicsId + ');"  value="" class="" style="margin-right:5px;color:black"> <i class="fa fa-book"></i> </a></span>' +
                 '<span><b><a id="Negative" href="javascript:void(0);" onclick="Clinical_ROSTemplateDetailRevamp.checkUncheckPositiveNegative(this,' + ROSSystemId + ');"  value="" class="" style="margin-right:5px;color:black"> (-) </a></b></span>' +
                 '<span><b><a id="Positive"  href="javascript:void(0);" onclick="Clinical_ROSTemplateDetailRevamp.checkUncheckPositiveNegative(this,' + ROSSystemId + ');"  value="" class="" style="margin-right:5px;color:black"> (+) </a></b></span>' +

                 '</label><span>' + CharatristicName + '</span><div id="divNameDetail' + ROSCharacteristicsId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + ROSCharacteristicsId + '" onkeypress="" name="Name' + ROSCharacteristicsId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 ROSCharacteristicsId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover hidden" onclick="Clinical_ROSTemplateDetailRevamp.removeCharateristics(' + ROSSystemId + ', ' + ROSCharacteristicsId + ')"><i class="fa fa-close"></i></span></a>'+
                 '<div class="clearfix"</div><div id="divDetail' + ROSCharacteristicsId + '" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetail' + ROSCharacteristicsId + '" name="CharacteristicDetail' + ROSCharacteristicsId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="Clinical_ROSTemplateDetailRevamp.setfoucustoarea(this)" ></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + ROSCharacteristicsId + '" onclick="Clinical_ROSTemplateDetailRevamp.validateMaxLength(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a><div class="clearfix"></div></li>';
        return li;  // zia html 2
    },
    validateMaxLength: function (obj) {
        //var maxLength = 250;
        //if (obj.value.length > maxLength) {

        //    obj.value = obj.value.substring(0, 250);
        //}
        var SystemID = $(obj).parents('li').attr('parentid');
        var CharID = $(obj).parents('li').attr('id');
        indexes = $.map(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
            if (item.CharcId == CharID && item.SystemId == SystemID) {
                return index;
            }
        })


        var CharPosNegDetailIndex = indexes[0];
        Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].CharcComments = $(obj).parents('#divDetail' + CharID + '').find('textarea').val();
        if ($(obj).parents('#divDetail' + CharID + '').find('textarea').val() == '') {
            $(obj).parents('li').find('i.fa-book').addClass('bule');
            $(obj).parents('li').find('i.fa-book').removeClass('green');

        }
        else {

            $(obj).parents('li').find('i.fa-book').addClass('green');
            $(obj).parents('li').find('i.fa-book').removeClass('blue');
        }
        $(obj).closest("li").find('div [id*="divDetail"]').addClass('hidden');

    },
    setfoucustoarea: function (obj) {
        //obj.focus();
        //return;
    },
    showHideToggleDetails: function (isToShow) {
        var toggleDetailsDiv = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamDataTemplateDetail div#divTogglePhysicalExamDetails");
        if (isToShow == false) {
            if (toggleDetailsDiv.hasClass("hidden") == false) {
                toggleDetailsDiv.addClass("hidden");
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails").addClass('hidden');
            }
        }
        else {

            toggleDetailsDiv.removeClass("hidden");
        }

    },
    openCharacteristicDetail: function (objButton, detailParentId) {
        if (objButton != null && detailParentId != null) {
            var liObject = $(objButton).parents('li');
            // var characteristicType = Clinical_ROSTemplateDetailRevamp.getCharacteristicType(liObject);
            if (!$(objButton).parents('li').siblings().find('div [id^="divDetail"]').hasClass(':not(hidden)'))
            {
                $(objButton).parents('li').siblings().find('div [id^="divDetail"]').addClass('hidden')
            }
            var pkid = $(liObject).prop("id");
            var type = $(liObject).closest("ul").prop("id");
            //$(objButton).parents('li').find('#Negative').css('pointer-events', 'none');
            //$(objButton).parents('li').find('#Positive').css('pointer-events', 'none');
            var isParentChecked = Clinical_ROSTemplateDetailRevamp.isSystemChecked(null, liObject);
            if (isParentChecked == true ) {
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
                else
                {
                    //$(objButton).find('i.fa-book').removeClass('green');
                    //$(objButton).parents('label').siblings('span').removeClass('green')
                }
             
                //  var comments = $(SystemDetailDiv).find('textarea').val();
                // Clinical_ROSTemplateDetailRevamp.updateTheCommentsOfCharAndSubChar(comments, pkid, type);
                $(SystemDetailDiv).find('textarea').focusout();
            }
            }
            else {
                utility.DisplayMessages("Please mark this characteristic as +ve/-ve to add details", 3);
            }
            //.removeClass("hidden");
        }
    },
    toggleAttribute: function (control, IsPositive, TemplateId, SystemId, CharatristicsId, NoteId, event) {
        if (event != null) {
            event.preventDefault();
            event.stopPropagation();
        }

        Clinical_ProgressNote.IsNoteComponentAvaliable(false, "ReviewofSystems").done(function (res) {
            if (res == true) {

                Clinical_ROSTemplateDetailRevamp.toggleAttribute_DbCall(!IsPositive, TemplateId, SystemId, CharatristicsId, NoteId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (IsPositive) {
                            $(control).text(' ( - ) ');
                            $(control).attr('onclick', 'Clinical_ROSTemplateDetailRevamp.toggleAttribute(this,false,' + TemplateId + ',' + SystemId + ',' + CharatristicsId + ',' + NoteId + ',event);')
                        } else {
                            $(control).html(' ( + ) ');
                            $(control).attr('onclick', 'Clinical_ROSTemplateDetailRevamp.toggleAttribute(this,true,' + TemplateId + ',' + SystemId + ',' + CharatristicsId + ',' + NoteId + ',event);')
                        }
                        $(control).toggleClass('red');
                        $(control).toggleClass('green');
                        Clinical_ProgressNote.saveComponentSOAPText('Review of Systems');
                        Clinical_ProgressNote.hoverFunction();
                    }
                });


            }
        });


    },
    toggleAttribute_DbCall: function (IsPositive, TemplateId, SystemId, CharatristicsId, NoteId) {
        var objData = new Object();
        objData["IsPositive"] = IsPositive;
        objData["TemplateId"] = TemplateId;
        objData["ROSSystemId"] = SystemId;
        objData["ROSCharacteristicsId"] = CharatristicsId; 
        objData["NoteId"] = NoteId;
        objData["commandType"] = "toggle_characteristics_note";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ReviewofSystemRevampTemplate");
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
    domReadyFunction: function (istrue) {
        $(function () {
            if (istrue == true)
            {
                setTimeout(function () { Clinical_ROSTemplateDetailRevamp.countWidth(); }, 300);
            } else {
                Clinical_ROSTemplateDetailRevamp.countWidth(); //405
                //setTimeout(function () { countWidth(); }, 1000);

            }
          
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " .toggleVertical div.toggle").click(function (e) {
                if ($(this).children().hasClass("active")) {
                    $(this).prev().removeClass("hidden");

                    Clinical_ROSTemplateDetailRevamp.countWidth(e);
                    $(this).parent().parent().scrollLeft(1000);
                }
                else {
                    $(this).prev().addClass("hidden");
                    Clinical_ROSTemplateDetailRevamp.countWidth(e);
                    $(this).parent().parent().scrollLeft(0);
                }
            });
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp').on('change', function () {
                $("#" + Clinical_ROSTemplateDetailRevamp.params["PanelID"] + ' #frmClinicalROSTemplateDetailRevamp #hfIsFormHasChange').val('true');
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
            Clinical_ROSTemplateDetailRevamp.countWidthApply(currentPanel, panelChildrens, applyWidth);
        }
        else {
            $('.toggleVertical').each(function (e) {
                currentPanel = $(this);
                panelChildrens = currentPanel.children().children("section.panel");
                applyWidth = currentPanel.children();
                Clinical_ROSTemplateDetailRevamp.countWidthApply(currentPanel, panelChildrens, applyWidth);
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
        document.getElementById("frmClinicalROSTemplateDetailRevamp").getElementsByClassName("frmClinicalROSTemplateDetailRevampPhysicalExam")[0].setAttribute("style", "width:" + (totalWidth - hidden + 50) + "px");
        //applyWidth.width((totalWidth - hidden + 50) + "px");
    },

    showTextArea: function (cntrl) {

        var txtArea = "";
        txtArea = '<div class="rightInnerAddon">' +
            '<textarea id="" maxlength="5000" spellcheck="true" class="form-control pr-xlg height-max105 size100per textAreaScroll"   onkeydown=\"Clinical_ROSTemplateDetailRevamp.setValueInHiddenDescription(event,this,0);\" onblur=\"Clinical_ROSTemplateDetailRevamp.textAreaFocusOut(event,this,1);\"></textarea><div class="clearfix"></div>'
            + '</div>';
           
            $(cntrl).closest('div:not(.checkbox-custom)').find('textarea').remove();
            $(cntrl).closest('div:not(.checkbox-custom)').append(txtArea);
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #divNormalGlobal").parent().find("textarea").html($('#divNormalGlobal').find("input:hidden").val().replace(/<br\s*[\/]?>/gi, "\n"));
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #divNormalGlobal").parent().find("textarea").focus();
         
            $('.textAreaScroll').slimScroll({
                position: 'right',
                height: '100%',
            }).on('input', function () {
                Clinical_ROSTemplateDetailRevamp.increaseRowsTextarea(this);
            }).on('focus', function () {
                Clinical_ROSTemplateDetailRevamp.increaseRowsTextarea(this);
            });

            $(cntrl).closest('li').find("textarea").focus();
            $('#frmClinicalROSTemplateDetailRevamp #divNormalGlobal').parent().find("textarea").val($('#frmClinicalROSTemplateDetailRevamp #divNormalGlobal').find("input:hidden").val());
    },
    setValueInHiddenDescription: function (e, cntrl, btnClicked) {

        var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
        if (btnClicked == 1) {

            $(cntrl).parent().parent().find("textarea").attr('flag', '1')

            if ($(cntrl).parent().parent().parent().find('#divNormalGlobal').length > 0) {

                var Comments = $('#frmClinicalROSTemplateDetailRevamp #divNormalGlobal').parent().find("textarea").val().replace(/\n/g, '<br/>');
                $('#frmClinicalROSTemplateDetailRevamp #divNormalGlobal').find("input:hidden").val(Comments);
                //$('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divReviewofSystemsSystems').find("li.active").find('[name=systemNormalDescription]').val(Comments);

               
                

            }



          // $(cntrl).closest("div.rightInnerAddon").remove();
        }
    },
    textAreaFocusOut: function (e, cntl,ind) {
        if ($(cntl).attr('flag') != '1') {
            
            if ($(cntl).length > 0) {
                
                var control = $(cntl);
                Clinical_ROSTemplateDetailRevamp.setValueInHiddenDescription(e, control, 1);
                if (ind == 1)
                {
                    $(control).closest("div.rightInnerAddon").remove();
                }
            }

        } else {
            $(cntl).attr('flag', '0');
        }
    },
    increaseRowsTextarea: function (obj) {
        while (
      obj.rows > 1 &&
      obj.scrollHeight < obj.offsetHeight
  ) {
            obj.rows--;
        }
        var h = 0;
        while (obj.scrollHeight > obj.offsetHeight && h !== obj.offsetHeight) {
            h = obj.offsetHeight;
            obj.rows++;
        }
    },
    clearSectionInfo: function (event, obj, SystemID, SystemName) {
        event.stopPropagation();
        //if (Clinical_ROSTemplateDetailRevamp.CharcSystemInfo[SystemName] != null && $(obj).parent().find('span:first-child').hasClass('green')) {
        if (($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #divReviewofSystemsSystemSection").find('input[type=checkbox]:checked').length > 0 || $(obj).closest("li").find('span').hasClass('green'))) {
            var message = 'This will reset all values in ' + SystemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?';
            var sysPatID = $(obj).closest("li").find("#systemPatientID").val();
            var sysPatNormal = $(obj).closest("li").find("#isSystemNormal").val();
            var CharcID = $(obj).closest("li").find("#isCharacteristicsExists").val();
            utility.myConfirm(message, function () {

                if ((typeof CharcID != 'undefined' && CharcID != '') || (typeof sysPatNormal != 'undefined' && sysPatNormal == 'True' || sysPatNormal == true)) {

                    Clinical_ROSTemplateDetailRevamp.rOSDataSystemReset_DBCall(sysPatID).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_ROSTemplateDetailRevamp.clearInfoForSystemReset(obj, SystemID, SystemName);
                        }
                    });
                } else {
                    Clinical_ROSTemplateDetailRevamp.clearInfoForSystemReset(obj, SystemID, SystemName);
                }
            }, function () {
            }, 'Reset Confirmation');

        }
    },
    toggleCheckBoxes: function (chkObject) {

        if ($(chkObject).attr('value') == '') {
            $(chkObject).parents('li').find('a[value]').attr('value', '');
            $(chkObject).attr('value', true);
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp .toggle").removeClass('disableAll');
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #detailHeading ").text($(chkObject).closest('li').text() + " Detail");
            $(chkObject).css({ "color": "red" });
            $(chkObject).attr('value',true);
            if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                Clinical_ROSTemplateDetailRevamp.toggleDetailsDiv();
                var CharacteristicId = $(chkObject).closest('li').attr("id");

            }
            else {
                Clinical_ROSTemplateDetailRevamp.toggleDetailsDiv();
                var SubCharacteristicId = $(chkObject).closest('li').attr("id");

            }

        }
        else {
            $(chkObject).attr('value', '');
            var isbothUnCheck = true;
            $(chkObject).parents('li').find('a[value]').each(function () {
                if ($(this).attr('value')=='true') {
                    isbothUnCheck = false;
                }
            });
            if (isbothUnCheck) {
                setTimeout(function (chkObject) {
                    $(chkObject).closest('li').removeClass("active");
                   // $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp section#CharacteristicsSubCharacteristics");

                    //if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                    //    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp  #CharacteristicsSubCharacteristics").addClass('hidden');;
                    //}
                    Clinical_ROSTemplateDetailRevamp.toggleDetailsDiv();
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp .toggle[data-plugin-toggle]").addClass('disableAll');
                    $(chkObject).css({ "color": "black" });
                    
                }, 100, chkObject);
            }
            else {
                Clinical_ROSTemplateDetailRevamp.toggleDetailsDiv();
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp div#divTogglePhysicalExamDetails").removeClass('hidden');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #detailHeading ").text($(chkObject).closest('li').text() + " Detail");

                if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                    Clinical_ROSTemplateDetailRevamp.toggleDetailsDiv();
                    var CharacteristicId = $(chkObject).closest('li').attr("id");

                }
                else {
                    Clinical_ROSTemplateDetailRevamp.toggleDetailsDiv();
                    var SubCharacteristicId = $(chkObject).closest('li').attr("id");

                }
            }
        }

    },
    ClearInfo: function (event, obj, SystemID, SystemName)
    {
        event.stopPropagation();
      //  if (Clinical_ROSTemplateDetailRevamp.CharcSystemInfo[SystemName] != null && $(obj).parent().find('span:first-child').hasClass('green')) {
      //  if (($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #divPhysicalExamSystemSection ul li").find("a[style='margin-right: 5px; color: rgb(255, 0, 0);']") > 0 || $(obj).closest("li").find('span').hasClass('green'))) {
        if ($(obj).closest("li.green").length > 0) {
            var message = 'This will reset all values in ' + SystemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?';
                var sysPatID = $(obj).closest("li").find("#systemPatientID").val();
                var sysPatNormal = $(obj).closest("li").find("#isSystemNormal").val();
                var CharcID = $(obj).closest("li").find("#isCharacteristicsExists").val();
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                utility.myConfirm(message, function () {

                    if ((typeof CharcID != 'undefined' && CharcID != '') || (typeof sysPatNormal != 'undefined' && sysPatNormal == 'True' || sysPatNormal == true)) {

                        Clinical_ROSTemplateDetailRevamp.rOSDataSystemReset_DBCall(sysPatID).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Clinical_ROSTemplateDetailRevamp.clearInfoForSystemReset(obj, SystemID, SystemName);
                            }
                        });
                } else {
                        Clinical_ROSTemplateDetailRevamp.clearInfoForSystemReset(obj, SystemID, SystemName);
                        //  remove caching...
                        indexIsSystemExist = $.map(Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems, function (item, index) {
                            if (item.Id == SystemID) {
                                return index;
                            }
                        })
                        if (indexIsSystemExist.length > 0) {

                            Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems.splice(indexIsSystemExist[0], 1);
                        }

                        indexPosNeg = $.map(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                            if (item.SystemId == SystemID) {
                                // Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg[i-1], 1);
                                return index;
                            }
                        })
                        if (indexPosNeg.length > 0) {
                            // Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg,1 );
                            for (var i = indexPosNeg.length; i > 0 ; i--) {

                                Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg[i - 1], 1);
                            }
                        }
                        indexAllsysPostNeg = $.map(Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative, function (item, index) {
                            if (item.SystemId == SystemID) {
                                return index;
                            }
                        })
                        if (indexAllsysPostNeg.length > 0) {

                            Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative.splice(indexAllsysPostNeg[0], 1);
                        }
                        for (var i = 0; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                            if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == SystemID) {
                                Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;

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
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').css('color', 'black');
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('i.fa-book').removeClass('green');
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('span.green').removeClass('green');
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('div[id^=divDetail] ').find('textarea').val('');
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').attr('value', '')
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam');
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam div.toggle').addClass('disableAll');
        // End Code By Zia
        //$('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' input[id^=Charc_Pos_' + SystemID + '],#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' input[id^=Charc_Neg_' + SystemID + '],' + '#' + Clinical_ROSTemplateDetailRevamp.params.PanelID +
        //    ' input[id=selectAllNegative_' + SystemID + '],#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + 'input[id=selectAllPositive_' + SystemID + ']').prop('checked', false);
        //Clinical_ROSTemplateDetailRevamp.bookIconClassToggle($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' input[id^=Charc_Pos_' + SystemID + ']').closest('ul').find('[id=bookIcon]'), true);

        //$('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' [id^=Charc_Pos_').parent().parent().removeClass('red');
        //$('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' input[id^=Charc_Desc_]').val('');
        // $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' input[id^=selectAllPositive_]').prop('checked', false).parent().parent().removeClass('red')
        $(obj).parent().find('span:first-child').removeClass('green');
        var detailObj = $.grep(Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
            return (item.SystemId == SystemID);
        });
        Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.splice(detailObj, 1);
        Clinical_ROSTemplateDetailRevamp.CharcSystemInfo[SystemName] = null;
        $(obj).closest("li").find("#isSystemNormal").val('');
        $(obj).closest("li").find("#systemNormalDescription").val('');

    },
    toggleDetailsDiv: function (liId, isReplace) {
        var objDeffered = $.Deferred();
        var sectionDetails = "";
        var self = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #divExamCharacteristics').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
            Clinical_ROSTemplateDetailRevamp.resetControlValue($(this));
        });
        objDeffered.resolve();
        return objDeffered;
    },
    removeItem: function (ROSSystemId, control, ROSCharacteristicsId) {
        if (control == "system") {
            $("#ulPhysicalExamSystems #" + ROSSystemId).remove();

            if (Clinical_ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 1;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                    }
                }
            }

        }
        else if (control == "observation") {
            $("#ulPhysicalExamSystemSection #" + ROSCharacteristicsId).remove();

            if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId && Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ObservationId == ROSCharacteristicsId) {
                Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 1;
                Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
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
        if (Clinical_ROSTemplateDetailRevamp.selectedObservations) {
            for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                    Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 1;
                    Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 1;
                    Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                    Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = false;
                    Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                }
            }
        }

        indexIsSystemExist = $.map(Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems, function (item, index) {
            if (item.Id == ROSSystemId) {
                return index;
            }
        })
        if (indexIsSystemExist.length > 0) {

            Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems.splice(indexIsSystemExist[0], 1);
        }

        indexPosNeg = $.map(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
            if (item.SystemId == ROSSystemId) {
               // Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg[i-1], 1);
                return index;
            }
        })
        if (indexPosNeg.length > 0) {
           // Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg,1 );
            for (var i = indexPosNeg.length; i > 0 ; i--) {

                Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg[i-1], 1);
            }
        }
        indexAllsysPostNeg = $.map(Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative, function (item, index) {
            if (item.SystemId == ROSSystemId) {
                return index;
            }
        })
        if (indexAllsysPostNeg.length > 0) {

            Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative.splice(indexAllsysPostNeg[0], 1);
        }
        for (var i = 0; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
            if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;

            }

        }
        
       
        $("#observationContent div").remove();
        event.stopPropagation();
    },

    removeCharateristics: function (ROSSystemId, ROSCharacteristicsId) {
        $("#ulPhysicalExamSystemSection #" + ROSCharacteristicsId).remove();
        $("#observationContent #divSystem" + ROSSystemId + ROSCharacteristicsId).remove();
        Clinical_ROSTemplateDetailRevamp.removeLastDelimiter(ROSSystemId);

        if (Clinical_ROSTemplateDetailRevamp.selectedObservations) {
            for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId && Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId == ROSCharacteristicsId) {
                    Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 1;
                    Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                    Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                }
            }
        }
    },

    loadCharatristics: function (ROSSystemId) {
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam div.toggle').addClass('disableAll');
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').prev().addClass('hidden');
        countWidth();
        Clinical_ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(true);
        $("#ulPhysicalExamSystemSection").show();
        var isExist = false;

        if (Clinical_ROSTemplateDetailRevamp.selectedObservations) {
            for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                    isExist = true;
                    break;
                }
            }
        }

        if (!$('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam div.toggle').hasClass('disableAll')) {
            Clinical_ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(true);
        }
        $('#frmClinicalROSTemplateDetailRevamp  #divAddNewSubSystem').removeClass('disableAll');
        if (!isExist) {
            Clinical_ROSTemplateDetailRevamp.ROSCharatristicsLoad(ROSSystemId).done(function (response) {
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
                        //    + 'onclick="Clinical_ROSTemplateDetailRevamp.selectAllChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                       
                       
                        //var IsNormalAll = '<li id="IsNormalAll" parentid="' + ROSSystemId + '" value="IsNormalAll" refvalue="IsNormalAll"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="IsNormalAllChar" name="IsNormal" class="pull-left" ' +
                        //    'onclick="Clinical_ROSTemplateDetailRevamp.NormalAllChars(this);">' +
                        //   '<button type="button" id="btnOpenDetailIsNormalAll"  data-toggle="tooltip" data-placement="top"  onclick="Clinical_ROSTemplateDetailRevamp.openCharacteristicDetail(this);" class="btn btn-link btn-xs pull-left"><i class="fa fa-book"></i></button>' +
                        //   '<label id="lblIsNormalAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Normal</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        //$("#ulPhysicalExamSystemSection").append(IsNormalAll);
                        //$("#ulPhysicalExamSystemSection").append(selectAll);
                        var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + ROSSystemId + '" value="Select All" refvalue="Select All">' +
                            //'<div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left hidden" '
                    // + 'onclick="Clinical_ROSTemplateDetailRevamp.selectAllChars(this);">'+
                     '<label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">'+
                        '<span><b><a id="NegativeAll" href="javascript:void(0);" onclick="Clinical_ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,false);"  value="" class="" style="margin-right:5px;color:black"> (-) </a></b></span>' +
                     '<span><b><a id="PositiveAll"  href="javascript:void(0);" onclick="Clinical_ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,true);"  value="" class="" style="margin-right:5px;color:black"> (+) </a></b></span>' +
                        'Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        var IsNormalAll = '<li id="IsNormalAll" parentid="' + ROSSystemId + '" value="IsNormalAll" refvalue="IsNormalAll"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="IsNormalAllChar" name="IsNormal" class="pull-left " ' +
                                     'onclick="Clinical_ROSTemplateDetailRevamp.NormalAllChars(this);">' +
                                      
                                    '<label id="lblIsNormalAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">' +
                                   '<span id="btnOpenDetailIsNormalAll"  data-toggle="tooltip" data-placement="top"  onclick="Clinical_ROSTemplateDetailRevamp.isNormalComments(this);" class="btn btn-link btn-xs pull-left disableAll" style="margin-top: -3px;"><i class="fa fa-book"></i></span>' +
                                    'Normal</label><div class="clearfix"></div><div class="clearfix"></div></div>' +
                                  // '<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="Clinical_ROSTemplateDetailRevamp.validateMaxLengthandCaching(this)" onkeypress="PhysicalExamDataTemplateDetail.saveDetailComments(event,this)"></textarea><div class="clearfix"></div></li>';
                        '<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="" ></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail" onclick="Clinical_ROSTemplateDetailRevamp.validateMaxLengthandCaching(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a><div class="clearfix"></div></li>';
                        $("#ulPhysicalExamSystemSection").append(IsNormalAll);
                        $("#ulPhysicalExamSystemSection").append(selectAll);
                       
                        $.each(resSystems, function (i, item) {
                            var li = Clinical_ROSTemplateDetailRevamp.addCharatristics(item.ROSCharacteristicsId, item.Name, ROSSystemId);
                            $("#ulPhysicalExamSystemSection").append(li);


                            var objSelectedObservations = { //IsChecked: false to true only for note
                                ROSSystemId: ROSSystemId, IsChecked: true, ROSCharacteristicsId: item.ROSCharacteristicsId, ROSCharacteristicsName: item.Name, IsSystemChecked: false, SystemDescription: "", IsSystemDeleted: 0, IsObservationDeleted: 0, SystemName: $("#frmClinicalROSTemplateDetailRevamp #PhysicalExam #divPhysicalExamSystems ul").find("li[id=" + ROSSystemId + "]").find("label[id=lblName" + ROSSystemId + "]").text().trim()
                            };
                            Clinical_ROSTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);

                        });
                    }
                    else {
                        // $('#observationContent div[id^=divSystem]').hide();
                        $("#ulPhysicalExamSystemSection li").remove();

                        $("#ulPhysicalExamSystems li").removeClass('active');
                        $("#ulPhysicalExamSystems li[id=" + ROSSystemId + "]").addClass('active');
                        var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + ROSSystemId + '" value="Select All" refvalue="Select All">' +
                            //'<div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left hidden" '
                              //           + 'onclick="Clinical_ROSTemplateDetailRevamp.selectAllChars(this);">' +
                                         '<label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">' +
                                            '<span><b><a id="NegativeAll" href="javascript:void(0);" onclick="Clinical_ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,false);"  value="" class="" style="margin-right:5px;color:black"> (-) </a></b></span>' +
                                         '<span><b><a id="PositiveAll"  href="javascript:void(0);" onclick="Clinical_ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,true);"  value="" class="" style="margin-right:5px;color:black"> (+) </a></b></span>' +
                                            'Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        var IsNormalAll = '<li id="IsNormalAll" parentid="' + ROSSystemId + '" value="IsNormalAll" refvalue="IsNormalAll"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="IsNormalAllChar" name="IsNormal" class="pull-left " ' +
                                     'onclick="Clinical_ROSTemplateDetailRevamp.NormalAllChars(this);">' +

                                    '<label id="lblIsNormalAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">' +
                                   '<span id="btnOpenDetailIsNormalAll"  data-toggle="tooltip" data-placement="top"  onclick="Clinical_ROSTemplateDetailRevamp.isNormalComments(this);" class="btn btn-link btn-xs pull-left disableAll" style="margin-top: -3px;"><i class="fa fa-book"></i></span>' +
                                    'Normal</label><div class="clearfix"></div><div class="clearfix"></div></div>' +
                                  // '<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="Clinical_ROSTemplateDetailRevamp.validateMaxLengthandCaching(this)" onkeypress="PhysicalExamDataTemplateDetail.saveDetailComments(event,this)"></textarea><div class="clearfix"></div></li>';
                        '<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="" ></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail" onclick="Clinical_ROSTemplateDetailRevamp.validateMaxLengthandCaching(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a><div class="clearfix"></div></li>';
                        $("#ulPhysicalExamSystemSection").append(IsNormalAll);
                        $("#ulPhysicalExamSystemSection").append(selectAll);
                    }
                }
            });
        }
        else {
            $("#SystemSections").removeClass('hidden');
            $("#ulPhysicalExamSystemSection li").remove();

            var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + ROSSystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success ">' +
                //'<input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left hidden" '
                 //       + 'onclick="Clinical_ROSTemplateDetailRevamp.selectAllChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">'+
                                       '<span><b><a id="NegativeAll" href="javascript:void(0);" onclick="Clinical_ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,false);"  value="" class="" style="margin-right:5px;color:black"> (-) </a></b></span>' +
                     '<span><b><a id="PositiveAll"  href="javascript:void(0);" onclick="Clinical_ROSTemplateDetailRevamp.checkUncheckPositiveNegativeAll(this,true);"  value="" class="" style="margin-right:5px;color:black"> (+) </a></b></span>' +
                        'Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
            var IsNormalAll = '<li id="IsNormalAll" parentid="' + ROSSystemId + '" value="IsNormalAll" refvalue="IsNormalAll"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="IsNormalAllChar" name="IsNormal" class="pull-left disableAll" ' +
                         'onclick="Clinical_ROSTemplateDetailRevamp.NormalAllChars(this);">' +
                        
                        '<label id="lblIsNormalAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">'+
                        '<span id="btnOpenDetailIsNormalAll"  data-toggle="tooltip" data-placement="top"  onclick="Clinical_ROSTemplateDetailRevamp.isNormalComments(this);" class="btn btn-link btn-xs pull-left disableAll " style="margin-top: -3px;"> <i class="fa fa-book"></i></span>' +
                        'Normal</label><div class="clearfix"></div><div class="clearfix"></div></div>'+
                        //'<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="Clinical_ROSTemplateDetailRevamp.validateMaxLengthandCaching(this)" onkeypress="PhysicalExamDataTemplateDetail.saveDetailComments(event,this)"></textarea><div class="clearfix"></div></li>';
                         '<div class="clearfix"</div><div id="divDetailIsNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetailDetailIsNormal" name="CharacteristicDetailDetailIsNormalDetailIsNormal" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="" ></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail" onclick="Clinical_ROSTemplateDetailRevamp.validateMaxLengthandCaching(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a><div class="clearfix"></div></li>';
            $("#ulPhysicalExamSystemSection").append(IsNormalAll);
            $("#ulPhysicalExamSystemSection").append(selectAll);

            if (Clinical_ROSTemplateDetailRevamp.selectedObservations) {
                //$('#observationContent div[id^=divSystem]').hide();
                $("#observationContent").text('');
                for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        var li = Clinical_ROSTemplateDetailRevamp.addCharatristics(Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId, Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsName, ROSSystemId);

                        if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId != "")
                            $("#ulPhysicalExamSystemSection").append(li);

                        $("#ulPhysicalExamSystems li").removeClass('active');
                        $("#ulPhysicalExamSystems li[id=" + ROSSystemId + "]").addClass('active');

                        if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked) {
                         
                            if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted == 0) {
                                $("#ulPhysicalExamSystemSection #chk" + Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId).prop("checked", true);
                                $('#observationContent #divSystem' + ROSSystemId + Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId).show();
                                Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;
                                Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 0;
                                Clinical_ROSTemplateDetailRevamp.handleDelimiter(ROSSystemId, Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId, Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsName, true);
                            }
                            else {
                                $("#ulPhysicalExamSystemSection #chk" + Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId).prop("checked", false);
                                Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;
                                Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 0;
                                Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                            }
                        }
                        else {
                            $("#ulPhysicalExamSystemSection #chk" + Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId).prop("checked", false);
                           // Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;
                            Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 0;
                            Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        }
                    }
                }
                for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail.length; i++) {
                    if (Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[i].SystemId == ROSSystemId) {
                        var charid = Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[i].CharcId;
                        if (charid) {
                            if (Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[i].IsPositive == true) {
                                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #SystemSections #divPhysicalExamSystemSection ul li[id=' + charid + ']').find('#Positive').css('color', 'green');
                                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #SystemSections #divPhysicalExamSystemSection ul li[id=' + charid + ']').find('#Positive').attr('value', true)
                            }
                            if (Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[i].IsNegative == true) {
                                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #SystemSections #divPhysicalExamSystemSection ul li[id=' + charid + ']').find('#Negative').css('color', 'red');
                                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #SystemSections #divPhysicalExamSystemSection ul li[id=' + charid + ']').find('#Negative').attr('value', true)
                            }

                            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #SystemSections #divPhysicalExamSystemSection ul li[id=' + charid + ']').find('div[id^=divDetail]').find('textarea').val(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[i].CharcComments);
                        }
                    }
                }
            }
            indexIsSystemExist = $.map(Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems, function (item, index) {
                if (item.Id == $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystems ul li.active').attr('id')) {
                    return index;
                }
            })
            if (indexIsSystemExist.length>0)
            {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="IsNormalAll"]').find('#IsNormalAllChar').attr("checked", true);
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li:not([id="IsNormalAll"])').addClass('disableAll');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').css('color', 'black');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('i.fa-book').removeClass('green');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('span.green').removeClass('green');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('div[id^=divDetail] ').find('textarea').val('');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').attr('value', '')
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam div.toggle').addClass('disableAll');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#btnOpenDetailIsNormalAll').removeClass('disableAll');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystems ul li.active').find('[type="checkbox"]').addClass('disableAll');
                $('#frmClinicalROSTemplateDetailRevamp  #divAddNewSubSystem').addClass('disableAll');
                if(Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems[indexIsSystemExist[0]].IsNormalComments)
                {
                    $('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #btnOpenDetailIsNormalAll').addClass('green');
                }
                else {
                    $('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #btnOpenDetailIsNormalAll').removeClass('green');
                }
            }
             var sysid = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').attr('Id')
            indexes = $.map(Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative, function (item, index) {
                if (item.SystemId == sysid) {
                    return index;

                }
            });
            if (indexes.length > 0) {
                var indexs = indexes[0];
                var IsPositiveind = Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IspositiveAll ;
                var IsNegativeind = Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IsnegativeAll;
                if (IsNegativeind == true) {
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').addClass('red');
                }
                else if (IsPositiveind == true) {
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').addClass('green');

                }
                else {
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                }
            }
            
            Clinical_ROSTemplateDetailRevamp.ChangeColorPostiveNegativeAll();
            
            // for Select All
            if ($('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').length == $('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find(':checkbox:checked').length) {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', true);
            }
            else {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', false);
            }
            // for load only active charateristics
            for (var i = 0; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++)
            {
                if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == sysid && Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted == 1)
                {
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId+ '"]').remove();

                }
              
                indexesforcharposNeg = $.map(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                    if (item.CharcId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                        return index;
                    }
                })

                if (indexesforcharposNeg.length > 0) {
                    var detialIndexForPostitiveNegative = indexesforcharposNeg[0];
                    if (Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].CharcComments) {
                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId + '"]').find('i.fa-book').addClass('green');
                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId + '"]').find('i.fa-book').removeClass('blue');
                      
                    }
                    else {
                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId + '"]').find('i.fa-book').addClass('bule');
                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="' + Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId + '"]').find('i.fa-book').removeClass('green');
                    }

                
                   
                }
       

            }
          
        }
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#IsNormalAllChar').removeClass('disableAll'); 
    },
    validateMaxLengthandCaching: function(obj)
    {
        var SystemID = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').attr('id');
        indexes = $.map(Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems, function (obj, index) {
            if (obj.SystemId == SystemID) {
                return index;
            }
        })
        var CharPosNegDetailIndex = indexes[0];
        Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems[CharPosNegDetailIndex].IsNormalComments = $(obj).parents('li').find('textarea').val();;
        $(obj).parents('#divDetailIsNormal').addClass('hidden');
        if ($(obj).parents('li').find('textarea').val())
        {
            $('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #btnOpenDetailIsNormalAll').addClass('green');
        }
        else {
            $('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #btnOpenDetailIsNormalAll').removeClass('green');
        }
    },
    validateMaxLengthandCachingGloble: function (obj) {
     
        $(obj).parents('#divDetailIsNormalGloble').addClass('hidden');
        if ($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #txtCharacteristicDetailDetailIsNormalGlobale').val()) {
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #bookIconNormalGlobal').addClass('green');
        }
        else {
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #bookIconNormalGlobal').removeClass('green');
        }
    },

    ManageCharateristics: function (ROSSystemId, obj) {
        $("#ulPhysicalExamSystems li").removeClass('active');
        $("#ulPhysicalExamSystems li[id=" + ROSSystemId + "]").addClass('active');

        if (!$(obj).is(':checked')) {
            $('#frmClinicalROSTemplateDetailRevamp #divPhysicalExamSystemSection  #chkboxSelectAllObservations :input').prop("checked", false);
            $(obj).parent().parent().removeClass('active');

            for (var i = 0; i < $("#ulPhysicalExamSystemSection li").length; i++) {
                $("#ulPhysicalExamSystemSection #chk" + $("#ulPhysicalExamSystemSection li")[i].id).prop('checked', false);
            }
            if (Clinical_ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = false;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                        //break;
                    }
                }
                
            }
        }
        else {
            
            if (Clinical_ROSTemplateDetailRevamp.selectedObservations) {
                setTimeout(function () { $('#frmClinicalROSTemplateDetailRevamp #divPhysicalExamSystemSection  #chkboxSelectAllObservations :input').prop("checked", true); }, 10);

                
                for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = true;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                        //break;
                    }
                }
                
            }
        }
    },
   
    checkUncheckPositiveNegativeAll: function(obj,Ispositive)  // cheeta
    {
        var IsPositiveind = false; //
        var IsNegativeind = false;
        var sysid=$('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').attr('Id')
        if (Ispositive == true) {
            IsNegativeind = false;
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Negative').css('color', 'black');
            if ($(obj).hasClass('green') == true) {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').removeClass('green');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Positive').css('color', 'black');
                $(obj).removeClass('green');
                IsPositiveind = false;
            }
            else {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').addClass('green');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Positive').css('color', 'green');
                $(obj).addClass('green');
                IsPositiveind = true;
            }

        }
        else {
            IsPositiveind = false;
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Positive').css('color', 'black');
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
            if ($(obj).hasClass('red') == true) {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').removeClass('green');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Negative').css('color', 'black');
                $(obj).removeClass('red');
                 IsNegativeind = false;
            }
            else {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').addClass('green');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#Negative').css('color', 'red');
                $(obj).addClass('red');
                IsNegativeind = true;
            }
        }
        //SystemAllPositiveNegative: [],
       // var sysid = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').attr('Id')
            indexes = $.map(Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative, function (item, index) {
                if (item.SystemId == sysid) {
                    return index;

                }
            });
            if (indexes.length > 0) {
                var indexs = indexes[0];
                Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IspositiveAll = IsPositiveind;
                Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IsnegativeAll = IsNegativeind;
            }
            else {
                var IsPosNegAll =
                    {
                        SystemId: sysid,
                        IspositiveAll: IsPositiveind,
                        IsnegativeAll: IsNegativeind
                    };
                Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative.push(IsPosNegAll);
            }
        

        for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
            if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == sysid) {
                indexesforcharposNeg = $.map(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                    if (item) {
                        if (item.CharcId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                            return index;

                        } 
                    }                                
                })

                if (indexesforcharposNeg.length > 0) {
                    var detialIndexForPostitiveNegative = indexesforcharposNeg[0];
                    Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsPositive = IsPositiveind;
                    Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsNegative = IsNegativeind;
                   // Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].CharcComments = "";
                }
                else {
                    var CharsPosNegDetail = {
                        Id: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId + '-' + Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                        SystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                        CharcId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                        CharcComments: "",
                        IsPositive: IsPositiveind,
                        IsNegative: IsNegativeind

                    };
                    Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail.push(CharsPosNegDetail);
                }
            }
            
            Clinical_ROSTemplateDetailRevamp.loadCharatristics(sysid);
            
        }


    },
    selectAllChars: function (obj) {
        $('#observationContent div[id^=divSystem]').remove();
        if (obj) var sysId = $($($(obj).parent().parent())[0]).attr('parentid');
        if ($(obj).prop('checked') == true) {
            $("#SystemPreview").removeClass('hidden');
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').each(function () {
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

            if (Clinical_ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == sysId) {
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = true;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + sysId).text();
                    }
                }
            }
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').removeClass("green");
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                    var system_id = $($($(obj).parent().parent())[0]).attr('parentid');
                    $("#observationContent #divSystem" + system_id + id_).remove();
                }
                $("#ulPhysicalExamSystems #chk" + sysId).prop("checked", false);
            });

            if (Clinical_ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == sysId) {
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = false;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + sysId).text();
                        //$('#observationContent div[id^=divSystem' + sysId + ']').hide();
                    }
                }
            }
        }
        Clinical_ROSTemplateDetailRevamp.removeLastDelimiter(sysId);
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
    isNormalComments: function(obj) 
    {
        if ($(obj).parents('li').find('#divDetailIsNormal').hasClass('hidden') == true) {
            $(obj).parents('li').find('#divDetailIsNormal').removeClass('hidden');
            $(obj).parents('li').find('#divDetailIsNormal').find('textarea').focus();
            var SystemID = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').attr('id');
            indexes = $.map(Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems, function (obj, index) {
                if (obj.SystemId == SystemID) {
                    return index;
                }
            })

            if (indexes.length > 0) {
                var index = indexes[0];
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #txtCharacteristicDetailDetailIsNormal').val(Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems[index].IsNormalComments);
            }
        }
        else
        {

            $(obj).parents('li').find('#divDetailIsNormal').addClass('hidden');
        }

    },
    isNormalCommentsGloble: function (obj) {
        if ($(obj).parents('#divNormalGlobalParent').find('#divDetailIsNormalGloble').hasClass('hidden') == true) {
            $(obj).parents('#divNormalGlobalParent').find('#divDetailIsNormalGloble').removeClass('hidden');
            $(obj).parents('#divNormalGlobalParent').find('#divDetailIsNormalGloble').find('textarea').focus();
         
        }
        else {

            $(obj).parents('div').find('#divDetailIsNormalGloble').addClass('hidden');
        }

    },
    NormalAllChars: function (obj )
    {   
        $(obj).parents('div').find('#divDetailIsNormalGloble').addClass('hidden');
        $(obj).parents('li').find('#divDetailIsNormal').addClass('hidden');
        var SystemID = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').attr('id');
        if ($(obj).prop('checked') == true) {
            var SystemName = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #System ul li.active').find('label').text().split(" ( + ) ").join("");
            
            var message = 'This will reset all values in ' + SystemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?';
            utility.myConfirm(message, function () {
                // Caching info
               
                var detailIsNormal = $.grep(Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems, function (item, index) {
                    return (item.SystemId == SystemID );
                });




                if (detailIsNormal.length == 0) {
                    var SystemIsNormal = {
                        Id: SystemID ,
                        SystemId: SystemID,
                        IsNormalComments: '',
                        IsNormal: true,
                       

                    };
                    Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems.push(SystemIsNormal);



                }
                
                indexes = $.map(Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative, function (item, index) {
                    if (item.SystemId == SystemID) {
                        return index;

                    }
                });
                if (indexes.length > 0) {
                    var indexs = indexes[0];
                    Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IspositiveAll=false;
                    Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IsnegativeAll=false;
                    
          
                    
                }
                $('#frmClinicalROSTemplateDetailRevamp  #divAddNewSubSystem').addClass('disableAll');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
                $('#frmClinicalROSTemplateDetailRevamp #IsNormalAllChar').parents('li').siblings().addClass('disableAll');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').css('color', 'black');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('i.fa-book').removeClass('green');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('span.green').removeClass('green');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('div[id^=divDetail] ').find('textarea').val('');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('a[value]').attr('value', '')
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam div.toggle').addClass('disableAll');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#btnOpenDetailIsNormalAll').removeClass('disableAll');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').find('#btnOpenDetailIsNormalAll').removeClass('disableAll');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').find('input').addClass('disableAll');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').removeClass('green');
                if (Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems.length == $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystems ul li').length)
                {
                   
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #bookIconNormalGlobal').removeClass('disabled gray');
                    var obj = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #chkReviewofSystemssNormal');
                    
                    //setTimeout(function () { $(obj).attr('checked', true); }, 100);
                    $(obj).prop('checked', true);
                    

                }
                // remove caching

                for (var i = 0; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {

                    if (SystemID == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                        indexesforcharposNeg = $.map(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                            if (item.CharcId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                                return index;
                            }
                        })

                        if (indexesforcharposNeg.length > 0) {
                            var detialIndexForPostitiveNegative = indexesforcharposNeg[0];
                            Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].CharcComments = '';
                            Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsNegative = false;
                            Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsPositive = false;
                        }
                        indexesforchardetailinfo = $.map(Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
                            if (item.Id == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                                return index;
                            }
                        })
                        if (indexesforchardetailinfo.length > 0) {
                            Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.splice(indexesforchardetailinfo[0], 1)
                        }

                    }
                }
            }, function () {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li[id="IsNormalAll"]').find('#IsNormalAllChar').attr("checked", false);
            }, 'Reset Confirmation');

      
        }
        else
        {
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #bookIconNormalGlobal').removeClass('green');
            $('#frmClinicalROSTemplateDetailRevamp #bookIconNormalGlobal').addClass('disabled gray');
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #txtCharacteristicDetailDetailIsNormalGlobale').val('');
            $('#frmClinicalROSTemplateDetailRevamp #chkReviewofSystemssNormal').attr("checked", false);
            indexes = $.map(Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems, function (obj, index) {
                if (obj.SystemId == SystemID) {
                    return index;
                }
            })
            var CharPosNegDetailIndex = indexes[0];
            Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems.splice(CharPosNegDetailIndex, 1);
            $('#frmClinicalROSTemplateDetailRevamp  #divAddNewSubSystem').removeClass('disableAll');
            $(obj).parents('li').siblings().removeClass('disableAll');
            $(obj).parents('li').find('#btnOpenDetailIsNormalAll').addClass('disableAll');
            $(obj).parents('li').find('#IsNormalAllChar').removeClass('disableAll');
            $('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #btnOpenDetailIsNormalAll').removeClass('green');
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').find('input').removeClass('disableAll');
            var sysid = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').attr('Id')
            indexPosNeg = $.map(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                if (item.SystemId == sysid) {
                    return index;
                }
            })
            if (indexPosNeg.length > 0) {
                for (var i = indexPosNeg.length; i > 0 ; i--) {

                    Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail.splice(indexPosNeg[i - 1], 1);
                }
            }
            
        }

    },
    markSectionsAsNormal: function (obj) {
        $(obj).parents('li').find('#divDetailIsNormal').addClass('hidden');
        var SystemID = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').attr('id');
        if ($(obj).prop('checked') == true) {
            var SystemName = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #System ul li.active').find('label').text().split(" ( + ) ").join("");

            var message = 'This will reset all values in ' + SystemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?';
            utility.myConfirm(message, function () {
                // Caching info
                $('#frmClinicalROSTemplateDetailRevamp #bookIconNormalGlobal').removeClass('disabled gray');
                for (var i = 0; i < $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #System ul li').length; i++) {
                    var ROSSystemID = null;
                    ROSSystemID = i + 1;
                    var detailIsNormal = $.grep(Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems, function (item, index) {
                     
                        return (item.SystemId == $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #System ul  li:nth-child(' + ROSSystemID + ')').attr('id'));
                    });




                    if (detailIsNormal.length == 0) {
                        var SystemIsNormal = {
                            Id: $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #System ul  li:nth-child(' + ROSSystemID + ')').attr('id'),
                            SystemId: $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #System ul  li:nth-child(' + ROSSystemID + ')').attr('id'),
                            IsNormalComments: '',
                            IsNormal: true,


                        };
                        Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems.push(SystemIsNormal);
                    }

                    indexes = $.map(Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative, function (item, index) {
                        if (item.SystemId == $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #System ul  li:nth-child(' + ROSSystemID + ')').attr('id')) {
                            return index;

                        }
                    });
                    if (indexes.length > 0) {
                        var indexs = indexes[0];
                        Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IspositiveAll = false;
                        Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative[indexs].IsnegativeAll = false;



                    }
                }
                
                // remove caching
                for (var i = 0; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
               

                   
                        indexesforcharposNeg = $.map(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                            if (item.CharcId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                                return index;
                            }
                        })

                        if (indexesforcharposNeg.length > 0) {
                            var detialIndexForPostitiveNegative = indexesforcharposNeg[0];
                            Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].CharcComments = '';
                            Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsNegative = false;
                            Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsPositive = false;
                        }

                }
                Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo = [];
                Clinical_ROSTemplateDetailRevamp.loadCharatristics(SystemID);
               
            }, function () {
                $(obj).attr("checked", false);
            }, 'Reset Confirmation');


        }
        else {
            
            $('#frmClinicalROSTemplateDetailRevamp #bookIconNormalGlobal').addClass('disabled gray');
            Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail = [];
            Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems = [];
            Clinical_ROSTemplateDetailRevamp.loadCharatristics(SystemID);
            $(obj).parents('div').find('#divDetailIsNormalGloble').addClass('hidden');
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #txtCharacteristicDetailDetailIsNormalGlobale').val('');
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #bookIconNormalGlobal').removeClass('green');
        }

    },

    loadSysPatCharcDetail: function (cntrl, isCharcDetailExists) {
        if (!$('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam').find('.toggle').hasClass('disableAll')) {
            Clinical_ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(true);
        }
        Clinical_ROSTemplateDetailRevamp.addActiveClass(cntrl);
        EMRUtility.resetControlValue($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #divExamCharacteristics'));
        //Clinical_ROSTemplateDetailRevamp.validateDurationDetails();
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #headingDetail').html($(cntrl).text() + ' Detail');
        var charcDetailId = '-1';// $(cntrl).find("input[type='hidden']:eq(1)").attr("id").split('_')[1];
        var detail = $.grep(Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
            return item.Id == cntrl.id;
        });
        if (isCharcDetailExists != null && isCharcDetailExists != '' && (detail == null || detail.length == 0)) {
            Clinical_ROSTemplateDetailRevamp.getSystemPatientCharcDetail_DBCall(charcDetailId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var detail = JSON.parse(response.SystemCharacteristicsDetails_JSON);
                    Clinical_ROSTemplateDetailRevamp.populateDetailAgainstCharacteristic(detail[0]);
                    Clinical_ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(false);
                }
            });
        }
        else {
            if ((detail != null && detail.length != 0)) {
                Clinical_ROSTemplateDetailRevamp.bindCacheCharacteristicDetailInfo(cntrl);
                Clinical_ROSTemplateDetailRevamp.validateDurationDetails();
            }
        }
        Clinical_ROSTemplateDetailRevamp.storeOldLookupROSTempSysCharDetailValues();
    },

    storeOldLookupROSTempSysCharDetailValues:function(){
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["PreviousHistory"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfPreviousHistory').val() ? $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfPreviousHistory').val() : "";
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["StatusId"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailStatusId').val();
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["Onset"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfROSOnset').val() ? $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfROSOnset').val() : "";
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["Duration"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfROSDuration').val() ? $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfROSDuration').val() : "";
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["DurationId"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailDurationId').val();
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["PatternId"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailPatternId').val();
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["SeverityId"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailSeverityId').val();
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["CourseId"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailCourseId').val();
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["RadiationId"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailRadiationId').val();
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["FrequencyId"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailFrequencyId').val();
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["ContextId"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailContextId').val();
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["CharacterCSZId"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailCharacterCSZId').val();
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["AggravedById"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailAggravedById').val();
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["RelievedById"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailRelievedById').val();
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["Location"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfLocation').val() ? $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfLocation').val() : "";
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["PrecipitatedBY"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfPrecipitatedBy').val() ? $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfPrecipitatedBy').val() : "";
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["AssociatedWith"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfAssociatedBy').val() ? $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfAssociatedBy').val() : "";
        Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues["hfROSCharacteristicsDetailsId"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #hfROSCharacteristicsDetailsId').val();
    },

    CacheCharacteristicDetailInfo: function (resetCache) {
        if ($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp  #ulPhysicalExamSystemSection > li.active").length > 0) {
            var systemid = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp  #ulPhysicalExamSystemSection > li.active").attr('parentid');
            var charcId = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #ulPhysicalExamSystemSection > li.active").attr('id');
            var charcDetailDiv = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics');
            var DetailData = unescape($(charcDetailDiv).getMyJSON());//kr escape.

            var detailDataParsed = JSON.parse(DetailData);
            if (detailDataParsed.Duration == '' || detailDataParsed.ROSCharacteristicsDetailDurationId_text == "- Select -") {
                detailDataParsed.Duration = '';
                detailDataParsed.ROSCharacteristicsDetailDurationId_text = '';
            }
            DetailData = JSON.stringify(detailDataParsed);

            if (resetCache != null && resetCache == true) {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics').resetAllControls();
            }
            Clinical_ROSTemplateDetailRevamp.CharcDetailDivJSON = unescape($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics').clone().getMyJSON());//kr escape.

            Clinical_ROSTemplateDetailRevamp.CharcDetailDivJSON = unescape($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics').clone().getMyJSON());//kr escape.

            var indexCh = -1;

            if (DetailData != Clinical_ROSTemplateDetailRevamp.CharcDetailDivJSON) {

                $.grep(Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
                    if (item.Id == charcId) {
                        indexCh = index;
                        return;
                    }
                });

                if (indexCh != -1) {
                    Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo[indexCh].DetailInfo = DetailData;
                }
                else {
                    var Ids = charcId.split('_');

                    var CharsDetailInfo = {
                        Id: charcId.split(' ').join(''),
                        SystemId: systemid,
                        CharcId: Ids.length > 0 ? Ids[2] : null,
                        DetailInfo: DetailData
                    };
                    Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.push(CharsDetailInfo); 
                }
            }
        }
    },
    bindCacheCharacteristicDetailInfo: function (cntrl) {
        var charcId = cntrl.id;
        var detail = $.grep(Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
            return item.Id == charcId;
        });
        if ((detail != null && detail.length != 0)) {
            detailJson = JSON.parse(detail[0].DetailInfo);

            Clinical_ROSTemplateDetailRevamp.populateDetailAgainstCharacteristic(detailJson);
        }

    },
    populateDetailAgainstCharacteristic: function (detail) {
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #PreviousHistory').val(detail.PreviousHistory);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailStatusId').val(detail.ROSCharacteristicsDetailStatusId);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #Onset').val(detail.Onset);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #Duration').val(detail.Duration);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailDurationId').val(detail.ROSCharacteristicsDetailDurationId);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailPatternId').val(detail.ROSCharacteristicsDetailPatternId);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailSeverityId').val(detail.ROSCharacteristicsDetailSeverityId);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailCourseId').val(detail.ROSCharacteristicsDetailCourseId);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailRadiationId').val(detail.ROSCharacteristicsDetailRadiationId);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailFrequencyId').val(detail.ROSCharacteristicsDetailFrequencyId);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailContextId').val(detail.ROSCharacteristicsDetailContextId);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailCharacterCSZId').val(detail.ROSCharacteristicsDetailCharacterCSZId);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailAggravedById').val(detail.ROSCharacteristicsDetailAggravedById);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #ROSCharacteristicsDetailRelievedById').val(detail.ROSCharacteristicsDetailRelievedById);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #Location').val(detail.Location);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #PrecipitatedBY').val(detail.PrecipitatedBY);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #AssociatedWith').val(detail.AssociatedWith);
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #hfROSCharacteristicsDetailsId').val(detail.ROSCharacteristicsDetailsId);
        Clinical_ROSTemplateDetailRevamp.validateDurationDetails();
    },
    PreviewCharateristics: function (ROSCharacteristicsId, ROSCharacteristicsName, obj, ROSSystemId) {
        //if ($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('[onClick^="Clinical_ROSTemplateDetailRevamp.PreviewCharateristics"]').prop('checked') == true)
        //{
        //    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', true);
        //}
        $("#SystemPreview").removeClass('hidden');

        if ($(obj).prop('checked') == true) {
            $(this).find("[type='checkbox'][id*='chk']").prop("checked", true);
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').removeClass("green");
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

            if (Clinical_ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId && Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId == ROSCharacteristicsId) {
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = true;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                    }
                    if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                    }
                }
            }

            $("#ulPhysicalExamSystems #chk" + ROSSystemId).prop("checked", true);
            Clinical_ROSTemplateDetailRevamp.loadCharatristics(ROSSystemId); 
        }
        else if ($(obj).prop('checked') == false) {
            if (Clinical_ROSTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId && Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId == ROSCharacteristicsId) {
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + ROSSystemId).text();
                      
                    }
                    if ($('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find(':checkbox:checked').length == 0 && Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId == ROSSystemId) {
                        Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = false;

                    }
                }
            }
            var aa = $('#observationContent #divSystem' + ROSSystemId + ROSCharacteristicsId).text();
            $('#observationContent #divSystem' + ROSSystemId + ROSCharacteristicsId).remove();
            Clinical_ROSTemplateDetailRevamp.removeLastDelimiter(ROSSystemId);
        }
        if ($('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find(':checkbox:checked').length < 1)
        {
            $('#ulPhysicalExamSystems >li.active :checkbox').prop("checked", false)
        }

        if ($('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').length == $('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find(':checkbox:checked').length) {
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', true);
        }
        else {
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', false);
        }
        Clinical_ROSTemplateDetailRevamp.removeLastDelimiter(ROSSystemId);
        //if ($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('[onClick^="Clinical_ROSTemplateDetailRevamp.PreviewCharateristics"]').prop('checked') == true) {
        //   // $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', true);
        //}
        //else {
        ////    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li #chkboxSelectAllObservations').prop('checked', false);
        // //   $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam ul li.active').find('[type=checkbox]').prop('checked', false);

        //}
    },
    validateDurationDetails: function (obj) {
        var duration = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #divExamCharacteristics #Duration');
        var durationQualifier = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #divExamCharacteristics #ROSCharacteristicsDetailDurationId');
        if (duration.val() != null && duration.val() != '') {
            durationQualifier.prop("disabled", false);
        } else {
            durationQualifier.prop("disabled", true).find("option:first").attr("selected", true);
        }
    },

    saveROSTemplate: function () {
        Clinical_ROSTemplateDetailRevamp.ROSTemplateSave();
    },
    addActiveClass: function (selectedLi) {
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection > li").removeClass('active');
        $(selectedLi).addClass('active');
        if (!$('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').prev().hasClass('hidden')) {
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').prev().addClass('hidden');
            countWidth();
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').removeClass('active');
        }
        Clinical_ROSTemplateDetailRevamp.enableDisableCharcDetails(selectedLi);
    },
    checkUncheckPositiveNegative: function (checkedBox, systemNameWithID) {
        //event.stopPropagation();
        //  if (Clinical_ROSTemplateDetailRevamp.checkDurationDetails()) {
        // Code for check detail 
        if (!$(checkedBox).parents('li ').find('div [id^=divDetail]').hasClass('hidden') == true)
        {
            //brake;
            return
        }
        var SystemID = $(checkedBox).parents('li').attr('parentid');
        var CharID = $(checkedBox).parents('li').attr('id');
        var detailPositiveNegative = $.grep(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
            return (item.SystemId == SystemID && item.CharcId == CharID);
        });
      

       
        if (detailPositiveNegative.length == 0)
        {
            var CharsPosNegDetail = {
                Id: SystemID+'-'+CharID,
                SystemId: SystemID,
                CharcId: CharID,
                CharcComments: '',
                IsPositive: false,
                IsNegative:false

            };
            Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail.push(CharsPosNegDetail);
       
            

        }
        indexes = $.map(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail, function (obj, index) {
            if (obj.CharcId == CharID && obj.SystemId == SystemID) {
                return index;
            }
        })
        var CharPosNegDetailIndex = indexes[0];

        if (!$("#ulPhysicalExamSystemSection").find('.toggle').prev().hasClass('hidden')) {
            $("#ulPhysicalExamSystemSection").find('.toggle').prev().addClass('hidden');
            countWidth();
            $("#ulPhysicalExamSystemSection").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
            $("#ulPhysicalExamSystemSection").find('.toggle').removeClass('active');
        }

        if (!$("#ulPhysicalExamSystemSection").find('.toggle').hasClass('disableAll')) {
            Clinical_ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(false);
        }

        if ($("#ulPhysicalExamSystemSection > li").hasClass('active'))
            $("#ulPhysicalExamSystemSection > li").removeClass('active');

        if (checkedBox != null) {
            Clinical_ROSTemplateDetailRevamp.validateDurationDetails();
              var IsComments = false;
        if ($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li.active ').find('[id^=divDetail]').find('textarea').val()) {
            IsComments = true;
        }
            if ($(checkedBox).attr('value') == '' ) {
                $(checkedBox).attr('value', true)
                if ($(checkedBox).html() == ' (+) ') {
                    $(checkedBox).css('color', 'green');
                    Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsPositive = true;
                    Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsNegative = false;
                }
                else
                {
                    $(checkedBox).css('color', 'red');
                    Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsNegative = true;
                    Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsPositive = false;
                }
               // $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').addClass('green')
                $(checkedBox).parents('li').find('i.fa-book').removeClass('active');
               // Clinical_ROSTemplateDetailRevamp.bookIconClassToggle($(checkedBox).parents('li').find('i.fa-book'), false);
                if ($(checkedBox).parent().parent().siblings('span').find('a').attr('value')=='true') {
                    // $(checkedBox).parent().siblings('.checkbox-icon').find('[type=checkbox]').prop('checked', false);
                    $(checkedBox).parent().parent().siblings('span').find('a').attr('value','')
                    $(checkedBox).parent().parent().siblings('span').find('a').css('color','black')
                }
                $('[id^=selectAllNeg] ,[id^=selectAllPos').prop('checked', false);

                if (!$("#ReviewofSystems").find('.toggle').hasClass('disableAll')) {
                    Clinical_ROSTemplateDetailRevamp.LoadCharacteristicDetailInfo(checkedBox);
                }
                if (IsComments == true) {
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul ').find('[id=' + CharID + ']').find('i.fa-book').removeClass('blue');
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul ').find('[id=' + CharID + ']').find('i.fa-book').addClass('green');
                }

            } else {
                if ($(checkedBox).html() == ' (+) ') {
                   
                    Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsPositive = false;
                }
                else {
                   
                    Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[CharPosNegDetailIndex].IsNegative = false;
                }
                $(checkedBox).parents('li').find('label').find('i').removeClass('green');
                $(checkedBox).parents('li').find('div[id^=divDetail] ').find('textarea').val('');
                $(checkedBox).attr('value', '')
                $(checkedBox).css('color', 'black')
                if ($(checkedBox).parents('li').siblings().find("#Positive[style='margin-right: 5px; color: rgb(0, 128, 0);']").length == 0 && $(checkedBox).parents('li').siblings().find("#Negative[style='margin-right: 5px; color: rgb(255, 0, 0);']").length == 0)
                {
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li.active').removeClass('green')
                }
                //Clinical_ROSTemplateDetailRevamp.bookIconClassToggle($(checkedBox).parents('li').find('i.fa-book'), false);
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
                        Clinical_ROSTemplateDetailRevamp.deleteROSDataSystemCharcDetail_DBCall(systemPatientCharacteristicsID).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Clinical_ROSTemplateDetailRevamp.uncheckCharacteristic($(checkedBox));
                            }
                        });

                    }, function () {
                        $(checkedBox).prop('checked','');
                        Clinical_ROSTemplateDetailRevamp.enableDisableCharcDetails();
                    });
                } else {
                    Clinical_ROSTemplateDetailRevamp.uncheckCharacteristic($(checkedBox));
                }
              //  Clinical_ROSTemplateDetailRevamp.hideTextAreaWhenBothChkBoxesUnchecked(checkedBox);
            }

            Clinical_ROSTemplateDetailRevamp.enableDisableCharcDetails();
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' [name=ReviewofSystemsSectionNormal]').prop('checked', false);
            // Clinical_ROSTemplateDetailRevamp.changeColorSystemOnCharcChange();
            Clinical_ROSTemplateDetailRevamp.ChangeColorPostiveNegativeAll();
            
        }
       
        
       
    },
    ChangeColorPostiveNegativeAll: function () {

        setTimeout(function () {
            if ($('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find('#Positive').length == $('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find("#Positive[style='margin-right: 5px; color: rgb(0, 128, 0);']").length) {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').addClass('green');
                
            }
            else if ($('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find('#Positive').length == $('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find("#Negative[style='margin-right: 5px; color: rgb(255, 0, 0);']").length) {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').addClass('red');
                
            }
            else {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#NegativeAll').removeClass('red');
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystemSection ul li').find('#PositiveAll').removeClass('green');
               

            }
            if ($('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find("#Positive[style='margin-right: 5px; color: rgb(0, 128, 0);']").length > 0 || $('#frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystemSection  #chkboxSelectAllObservations').nextAll('li').find("#Negative[style='margin-right: 5px; color: rgb(255, 0, 0);']").length > 0) {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystems  ul li.active').addClass('green');
            } else {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamSystems  ul li.active').removeClass('green');
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
        var detailObj = $.grep(Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
            if (item.Id == $charcLI.attr('id'))
            {
                Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.splice(index, 1);
            };
        });

        if (detailObj.length > 0) {
            Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.splice(detailObj, 1);
        }
        Clinical_ROSTemplateDetailRevamp.bookIconClassToggle($charcLI.find('[id=bookIcon]'), true);
        $charcLI.find("[type=hidden]").val('');

        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics').resetAllControls();
        Clinical_ROSTemplateDetailRevamp.enableDisableCharcDetails();
        Clinical_ROSTemplateDetailRevamp.handleDetailToggleDiv();
    },
    handleDetailToggleDiv: function () {
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #ulPhysicalExamSystemSection").find('.toggle').prev().addClass('hidden');
        countWidth();
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
                Clinical_ROSTemplateDetailRevamp.disableBookIcon(Control);
            }
        } else if (IsDisabled) {
            Clinical_ROSTemplateDetailRevamp.disableBookIcon(Control);
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

        if (Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo != null && Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo.length > 0) {

            var detailObj = $.grep(Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
                return (item.Id == charcId);
            });

            if (detailObj.length > 0) {
                utility.bindMyJSON(true, JSON.parse(detailObj[0].DetailInfo), false, $(charcDetailDiv));
                charcId = charcId.replace('/', '\\/');

                $(charcDetailDiv).data($(charcId), detailObj[0].DetailInfo);
                Clinical_ROSTemplateDetailRevamp.validateDurationDetails();
            }
            else {
                $(charcDetailDiv).resetAllControls();
            }

        } else {
            $(charcDetailDiv).resetAllControls();

        }
    },
    changeColorSystemOnCharcChange: function () {
        var CurrentSystem = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #PhysicalExam li.active");
        var charcSystemDiv = '#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divExamCharacteristics';
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
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').removeClass('disableAll');
            } else {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').addClass('disableAll');
            }
        } else {
            if ($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('input:checkbox:not([id*=ReviewofSystemsSectionNormal])').is(':checked') == true) {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').removeClass('disableAll');
            }
            else if ($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('input:checkbox[id*=ReviewofSystemsSectionNormal]')) {
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #PhysicalExam").find('.toggle').addClass('disableAll');
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
        Clinical_ROSTemplateDetailRevamp.loadROSLookups_DBcall().done(function (response) {
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


                    // data_.total = resposeData.iTotalDisplayRecords;
                    //e.success(data_);
                }
                else {
                    utility.DisplayMessages(resposeData.Message, 3);
                    //e.success(data_);
                }
                $('#frmClinicalROSTemplateDetailRevamp').data('serialize', $('#frmClinicalROSTemplateDetailRevamp').serialize());
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
        var parrentPanelId = "#" + Clinical_ROSTemplateDetailRevamp.params["PanelID"];
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
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (Clinical_ROSTemplateDetailRevamp.SpecialtyIds != '') {

                        var Specialties = Clinical_ROSTemplateDetailRevamp.SpecialtyIds.split(",");
                        Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = Specialties;
                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').val(Specialties);
                    }
                }

            }).then(function () {
                Clinical_ROSTemplateDetailRevamp.IntializeMultiSelectDropDownSpecialties();
                //enable dropdownlist
                Clinical_ROSTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty', false);
                Clinical_ROSTemplateDetailRevamp.loadEntityProvider(globalAppdata["SeletedEntityId"], dfd);


            });
        }
        else {
            //Disable dropdownlist
            Clinical_ROSTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty', true);
        }
    },

    loadEntityProvider: function (entityId, dfd) {

        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider');
                var $providerHiddenDdl = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlHiddenPhysicalExamTemplateProvider');

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
                if (Clinical_ROSTemplateDetailRevamp.ProviderIds != '') {
                    var Providers = Clinical_ROSTemplateDetailRevamp.ProviderIds.split(",");
                    Clinical_ROSTemplateDetailRevamp.providerCheckedIds = Providers;
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamTemplateSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                Clinical_ROSTemplateDetailRevamp.IntializeMultiSelectDropDownProviders();
                if (dfd)
                    dfd.resolve();
            });
            //enable multiselect
            //Clinical_ROSTemplateDetailRevamp.enableDisableDropDowLists('ddlPhysicalExamTemplateProvider', false);
        }
        else {
            //disable multiselect
            Clinical_ROSTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateProvider', true);
        }
    },

    buildAlreadyAssosiatedSystems: function (templateId) {
        Clinical_ROSTemplateDetailRevamp.Clinical_ROSTemplateDetailRevampLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var res = JSON.parse(response.PETemplateSystems_JSON);
                var resSystems = JSON.parse(res);
                var data = [];

                $.each(resSystems, function (i, item) {
                    data.push({ id: item.ROSSystemId, text: item.SystemName, expanded: true, spriteCssClass: "rootfolder" });
                    var li = Clinical_ROSTemplateDetailRevamp.addSystem(item.ROSSystemId, item.SystemName); //'<li id="' + item.ROSSystemId + '" parentid="null" onclick="" value="' + item.ROSSystemId + '" refvalue="" subcharacteristicexist=" " class="active"><div class="checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chk' + item.ROSSystemId + '" name="' + item.ROSSystemId + '" class="pull-left  char" onclick=""><label id="lblName' + item.ROSSystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + item.SystemName + '">' + item.SystemName + '</label><div id="divNameDetail' + item.ROSSystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" id="txtName' + item.ROSSystemId + '" onkeypress="" name="Name' + item.ROSSystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.ROSSystemId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                    //var li = '<li id="' + item.ROSSystemId + '" parentid="null" onclick="Clinical_ROSTemplateDetailRevamp.showHideChildControls(this,"ulPhysicalExamSystems",' + item.ROSSystemId + ');" value="' + item.ROSSystemId + '" refvalue="" subcharacteristicexist=" " class="active"><div class="checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chk' + item.ROSSystemId + '" name="' + item.ROSSystemId + '" class="pull-left  char" onclick="Clinical_ROSTemplateDetailRevamp.selectParentControls(this);Clinical_ROSTemplateDetailRevamp.toggleCheckBoxes(this);"><label id="lblName' + item.ROSSystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + item.SystemName + '">' + item.SystemName + '</label><div id="divNameDetail' + item.ROSSystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" id="txtName' + item.ROSSystemId + '" onkeypress="Clinical_ROSTemplateDetailRevamp.saveDetailComments(event,this)" name="Name' + item.ROSSystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.ROSSystemId + '" onclick="Clinical_ROSTemplateDetailRevamp.saveTemplateSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                    $("#ulPhysicalExamSystems").append(li);
                    Clinical_ROSTemplateDetailRevamp.loadCharatristics(item.ROSSystemId);
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
        Clinical_ROSTemplateDetailRevamp.ROSSystemsLoad(templateId).done(function (response) {
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
                        var li = Clinical_ROSTemplateDetailRevamp.addSystem(dataItem.id, dataItem.value);
                        if (li != undefined)
                            $("#ulPhysicalExamSystems").append(li);
                        $("#Systems").val('');
                        Clinical_ROSTemplateDetailRevamp.loadCharatristics(dataItem.id);
                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #chkReviewofSystemssNormal').prop('checked', false);
                        e.preventDefault();
                    },
                    footerTemplate: 'Total <strong>#: instance.dataSource.total() #</strong> items found'
                });

                $("#Systems").parent().addClass('size100per');
                $('#frmClinicalROSTemplateDetailRevamp').data('serialize', $('#frmClinicalROSTemplateDetailRevamp').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadClinical_ROSTemplateDetailRevamp: function (templateId) {
        Clinical_ROSTemplateDetailRevamp.Clinical_ROSTemplateDetailRevampLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                response.PhysicalExamTemplate = JSON.parse(response.PhysicalExamTemplate);

                if (response.PhysicalExamTemplate.length > 0) {
                    response.PhysicalExamTemplate = response.PhysicalExamTemplate[0];
                    Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData = response.PhysicalExamTemplate.SysSecCharSubcharData;
                    //Start Farooq Ahmad 03-03-2016 mark green and checked the loaded System
                    for (var index in Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData) {
                        //if ($.parseJSON(Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData[index].IsChecked.toString().toLowerCase()))
                        //    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId).addClass("green");
                        //else
                        //    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId).removeClass("green");

                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId + " input:checkbox").prop("checked", $.parseJSON(Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData[index].IsChecked.toString().toLowerCase()));
                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId + " label").attr("data-original-title", Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemName);
                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId + " label").text(Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData[index].SystemName);
                    }
                    //End Farooq Ahmad 03-03-2016 mark green and checked the loaded System
                    var self = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp");

                    utility.bindMyJSONByName(true, response.PhysicalExamTemplate, false, self).done(function () {
                        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateEntity').change();
                        Clinical_ROSTemplateDetailRevamp.SpecialtyIds = response.PhysicalExamTemplate.SpecialtyIds;
                        Clinical_ROSTemplateDetailRevamp.ProviderIds = response.PhysicalExamTemplate.ProviderIds;
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

    Clinical_ROSTemplateDetailRevampLoad: function (templateId) {
        var objData = new Object();
        objData["TemplateId"] = templateId == undefined ? "0" : templateId;
        objData["commandType"] = "Load_PhyscialExam_Templates_ECW";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    Clinical_ROSTemplateDetailRevampLoadOnDemand: function (templateId, systemId, sectionId, charId, commandType) {
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
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('destroy');
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {

                Clinical_ROSTemplateDetailRevamp.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                $.when(
                    Clinical_ROSTemplateDetailRevamp.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (Clinical_ROSTemplateDetailRevamp.ProviderIds != '') {
                        var Providers = Clinical_ROSTemplateDetailRevamp.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                Clinical_ROSTemplateDetailRevamp.providerCheckedIds = Clinical_ROSTemplateDetailRevamp.removeFromArray(Clinical_ROSTemplateDetailRevamp.providerCheckedIds, item);
                                Clinical_ROSTemplateDetailRevamp.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').val(Clinical_ROSTemplateDetailRevamp.providerCheckedIds);
                    Clinical_ROSTemplateDetailRevamp.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Clinical_ROSTemplateDetailRevamp.SpecialtyIds != '') {
                    var spacialties = Clinical_ROSTemplateDetailRevamp.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = Clinical_ROSTemplateDetailRevamp.removeFromArray(Clinical_ROSTemplateDetailRevamp.specialityCheckedIds, item);
                            Clinical_ROSTemplateDetailRevamp.specialityCheckedIds.push(item);
                        });
                    }
                }
                Clinical_ROSTemplateDetailRevamp.setSpacialtiesByselectedProviderIds();
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('select', Clinical_ROSTemplateDetailRevamp.specialityCheckedIds);
            },
        });
    },

    setSpacialtiesByselectedProviderIds: function () {

        $.each(Clinical_ROSTemplateDetailRevamp.providerCheckedIds, function (index, item) {

            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {

                        Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = Clinical_ROSTemplateDetailRevamp.removeFromArray(Clinical_ROSTemplateDetailRevamp.specialityCheckedIds, $(this).attr('refname'));
                        Clinical_ROSTemplateDetailRevamp.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('destroy');
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_ROSTemplateDetailRevamp.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
                // Clinical_ROSTemplateDetailRevamp.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('refresh');
            },


        });
    },

    domReady: function () {

        $(document).ready(function () {

            Clinical_ROSTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty,ddlPhysicalExamTemplateProvider', true);

            //callback function on change event of entity ddl
            Clinical_ROSTemplateDetailRevamp.entityChanged();

            //Initialize when the document is ready for the first time (just for the good look).
            Clinical_ROSTemplateDetailRevamp.IntializeMultiSelectDropDownSpecialties();
            Clinical_ROSTemplateDetailRevamp.IntializeMultiSelectDropDownProviders();


            $(document).click(function (event) {



                var $Item = $(Clinical_ROSTemplateDetailRevamp.selectedListItem);
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

        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateEntity').change(function () {
            //Get the selected entity
            selectedEntity = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateEntity :selected').val();

            $.when(Clinical_ROSTemplateDetailRevamp.loadEntitySpecialty(selectedEntity)).then(function () {

                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('destroy').multiselect();
            });
            $.when(Clinical_ROSTemplateDetailRevamp.loadEntityProvider(selectedEntity)).then(function () {

                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('destroy').multiselect();

            });
        });
    },

    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlHiddenPhysicalExamTemplateProvider';

        var providerContext = '#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider';
        $(providerContext).empty();

        if (Clinical_ROSTemplateDetailRevamp.specialityCheckedIds.length > 0) {

            $.each(Clinical_ROSTemplateDetailRevamp.specialityCheckedIds, function (index, specialtyId) {

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
        var specialtyContext = '#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamTemplateSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = [];
            Clinical_ROSTemplateDetailRevamp.providerCheckedIds = [];
            Clinical_ROSTemplateDetailRevamp.ProviderIds = '';
            Clinical_ROSTemplateDetailRevamp.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {


                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = Clinical_ROSTemplateDetailRevamp.removeFromArray(Clinical_ROSTemplateDetailRevamp.specialityCheckedIds, spacialityId);
                    Clinical_ROSTemplateDetailRevamp.specialityCheckedIds.push(spacialityId);
                }
                else {

                    Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = Clinical_ROSTemplateDetailRevamp.removeFromArray(Clinical_ROSTemplateDetailRevamp.specialityCheckedIds, spacialityId);

                }


            }
            else {

                Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = [];
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    Clinical_ROSTemplateDetailRevamp.specialityCheckedIds.push(spacialityId);
                });

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
        var providerContext = '#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamTemplateProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        if (checkedProviderItems <= 0) {
            Clinical_ROSTemplateDetailRevamp.providerCheckedIds = [];
            Clinical_ROSTemplateDetailRevamp.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected) {
            Clinical_ROSTemplateDetailRevamp.providerCheckedIds = [];
            $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider option').each(function () {
                var providerValue = $(this).val();
                Clinical_ROSTemplateDetailRevamp.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                Clinical_ROSTemplateDetailRevamp.providerCheckedIds = Clinical_ROSTemplateDetailRevamp.removeFromArray(Clinical_ROSTemplateDetailRevamp.providerCheckedIds, providerValue);
                Clinical_ROSTemplateDetailRevamp.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                Clinical_ROSTemplateDetailRevamp.providerCheckedIds = Clinical_ROSTemplateDetailRevamp.removeFromArray(Clinical_ROSTemplateDetailRevamp.providerCheckedIds, $(option).val());
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
            currentId = Clinical_ROSTemplateDetailRevamp.NewInsertedId--;
            var subcharacteristicExist = "";

            var saveMethodPE = "";

            if (itemType.toLowerCase() == "system") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #" + currentCtrlId);
                saveMethodPE = "Clinical_ROSTemplateDetailRevamp.AddSystemROS(this, '" + currentId + "');";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystemSection";
                ulControl = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #frmClinicalROSTemplateDetailRevamp #" + currentCtrlId);
                saveMethodPE = "Clinical_ROSTemplateDetailRevamp.AddChartristics(this, '" + currentId + "');";
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
                onClick = "";//"Clinical_ROSTemplateDetailRevamp.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";
                var deleteFunction = "Clinical_ROSTemplateDetailRevamp.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                liInnerText = '<div class="checkbox-custom checkboxTiny checkbox-success">' + deleteIcon + addSubCharIcon
                    + '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="">'
                    //+ '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="Clinical_ROSTemplateDetailRevamp.selectParentControls(this);Clinical_ROSTemplateDetailRevamp.toggleCheckBoxes(this);">'
                    + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                    + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                    + '<textarea rows="1" maxlength="150" spellcheck="true" id="txtName' + currentId + '" onkeypress="Clinical_ROSTemplateDetailRevamp.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
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

        Clinical_ROSTemplateDetailRevamp.saveROSSystem_DBCall(objData).done(function (response) {
           
                response = JSON.parse(response);
                utility.DisplayMessages(response.Message, 1);
                if (response.status != false) {
                var li = Clinical_ROSTemplateDetailRevamp.addSystem(response.ROSSystemId, objData["Name"]);
                $("#ulPhysicalExamSystems").append(li);

                //var objSelectedObservations = {
                //    ROSSystemId: response.ROSSystemId, IsChecked: false, ROSCharacteristicsId: '', ROSCharacteristicsName: '', IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0
                //};
                //Clinical_ROSTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
                Clinical_ROSTemplateDetailRevamp.loadCharatristics(response.ROSSystemId);
                $("#" + controlId).remove();
                Clinical_ROSTemplateDetailRevamp.buildSystemsAutoComplete();
                $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #chkReviewofSystemssNormal').prop('checked',false);
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
        objData["TemplateId"] = Clinical_ROSTemplateDetailRevamp.params.ROSTemplateId;
        var CharatristicName = $("#txtName" + controlId).val();
        objData["IsActive"] = true;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        Clinical_ROSTemplateDetailRevamp.saveROSCharatristics_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
               // var res = JSON.parse(response.PEObservation_JSON);
                utility.DisplayMessages(response.Message, 1);
                if (response.status == true) {
                    Clinical_ROSTemplateDetailRevamp.params.ROSCharacteristicsId = response.ROSCharacteristicsId;
                    var li = Clinical_ROSTemplateDetailRevamp.addCharatristics(response.ROSCharacteristicsId, CharatristicName, ROSSystemId);
                    $("#ulPhysicalExamSystemSection").append(li);
                    var sysChecked = $("#chk" + $("#ulPhysicalExamSystems li.active").attr('id')).is(':checked');
                    var objSelectedObservations = {
                        ROSSystemId: ROSSystemId, IsChecked: true, ROSCharacteristicsId: response.ROSCharacteristicsId, ROSCharacteristicsName: CharatristicName, IsSystemChecked: true, IsSystemDeleted: 0, IsObservationDeleted: 0, SystemName: $("#frmClinicalROSTemplateDetailRevamp #PhysicalExam #divPhysicalExamSystems ul").find("li[id="+ROSSystemId+"]").find("label[id=lblName" + ROSSystemId + "]").text().trim()
                    };
                    Clinical_ROSTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
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
            onClickFunction = onClickFunction.replace('this', "$('#" + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #" + ULID + " #" + ID + "')");
            eval(onClickFunction);
        }
    },

    saveROSSystem_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;
        objData["TemplateId"] = Clinical_ROSTemplateDetailRevamp.params.ROSTemplateId;
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
        $("#Clinical_ROSTemplateDetailRevamp #pnlLicenseDetail").removeClass('disableAll');
        if (Clinical_ROSTemplateDetailRevamp.params.mode == "Add") {
            $('#Clinical_ROSTemplateDetailRevamp #txtShortName').attr("enabled", "enabled");

            $("#Clinical_ROSTemplateDetailRevamp #pnlLicenseDetail").addClass('disableAll');
            Clinical_ROSTemplateDetailRevamp.ValidateProvider();

            //serialize Data after all controls loaded.
            $('#frmClinicalROSTemplateDetailRevamp').data('serialize', $('#frmClinicalROSTemplateDetailRevamp').serialize());

        }
        else if (Clinical_ROSTemplateDetailRevamp.params.mode == "Edit") {
            $('#Clinical_ROSTemplateDetailRevamp #txtShortName').attr("disabled", "disabled");
            Clinical_ROSTemplateDetailRevamp.LoadProviderLicense().done(function (response) {
                if (response.status != false) {

                    Clinical_ROSTemplateDetailRevamp.ProviderLicenseGridLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            Clinical_ROSTemplateDetailRevamp.FillProvider(Clinical_ROSTemplateDetailRevamp.params.ProviderId).done(function (response) {
                if (response.status != false) {
                    var provider_detail = JSON.parse(response.ProviderFill_JSON);
                    var self = $("#Clinical_ROSTemplateDetailRevamp");
                    utility.bindMyJSON(true, provider_detail, false, self).done(function () {

                        if (provider_detail.chkActive == 'True')
                            $("#Clinical_ROSTemplateDetailRevamp #chkActive").attr("checked", true);
                        else
                            $("#Clinical_ROSTemplateDetailRevamp #chkActive").attr("checked", false);

                        if (provider_detail.chkSpecialist == 'True')
                            $("#Clinical_ROSTemplateDetailRevamp #chkSpecialist").attr("checked", true);
                        else
                            $("#Clinical_ROSTemplateDetailRevamp #chkSpecialist").attr("checked", false);

                        $("#Clinical_ROSTemplateDetailRevamp #pnlLicenseDetail").removeClass('disableAll');

                        Clinical_ROSTemplateDetailRevamp.ValidateProvider();
                        //serialize Data after all controls loaded.
                        $('#frmClinicalROSTemplateDetailRevamp').data('serialize', $('#frmClinicalROSTemplateDetailRevamp').serialize());

                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }
    },

    LoadEntityBasedData: function (entityID) {

        Clinical_ROSTemplateDetailRevamp.LoadBasicFeeGroup(entityID).done(function () {

        });
        Clinical_ROSTemplateDetailRevamp.LoadSupervisingProvider(entityID).done(function () {

        });
        Clinical_ROSTemplateDetailRevamp.LoadSpecialty(entityID).done(function () {
            $('#frmClinicalROSTemplateDetailRevamp').bootstrapValidator('revalidateField', $('#frmClinicalROSTemplateDetailRevamp #ddlSpecialty').attr('name'));
        });
        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#tblClinical_ROSTemplateDetailRevamp #ddlFeeGroup', 'GetFeeGroup', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#tblClinical_ROSTemplateDetailRevamp #ddlFeeGroup', 'GetFeeGroup', true, null);
        }
    },

    LoadSupervisingProvider: function (entityID) {
        // Loads Entity Based Supervising Provider
        return Clinical_ROSTemplateDetailRevamp.FillSupervisingProvider(entityID).done(function (response) {
            if (response.status != false) {
                var feegroup_detail = JSON.parse(response.SupervisingProviderLoad_JSON);
                $("#Clinical_ROSTemplateDetailRevamp #ddlSupervisingProvider").empty();
                $("#Clinical_ROSTemplateDetailRevamp #ddlSupervisingProvider").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(feegroup_detail, function (i, item) {
                    $("#Clinical_ROSTemplateDetailRevamp #ddlSupervisingProvider").append(
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
            return Clinical_ROSTemplateDetailRevamp.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {
                    var feegroup_detail = JSON.parse(response.SpecialtyLoad_JSON);
                    $("#Clinical_ROSTemplateDetailRevamp #ddlSpecialty").empty();
                    $("#Clinical_ROSTemplateDetailRevamp #ddlSpecialty").append($('<option/>', {
                        value: "",
                        html: "- SELECT -"
                    }));
                    $.each(feegroup_detail, function (i, item) {
                        $("#Clinical_ROSTemplateDetailRevamp #ddlSpecialty").append(
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

        if ($(Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData).length > 0) {
            $.each(Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData, function (i, item) {


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
        if (!$('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #PhysicalExam div.toggle').hasClass('disableAll')) {
            Clinical_ROSTemplateDetailRevamp.CacheCharacteristicDetailInfo(true);
        }

        var isValid = false;
        var self = null;
        self = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp');

        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        //objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = Clinical_ROSTemplateDetailRevamp.specialityCheckedIds.join();
        //objData.ProviderIds = objData.PhysicalExamTemplateProvider = Clinical_ROSTemplateDetailRevamp.providerCheckedIds.join();

        var isStillValid = false;

        //if (Clinical_ROSTemplateDetailRevamp.validateSelectedTemplateData())
        //{
        myJSON = JSON.stringify(objData);
        Clinical_ROSTemplateDetailRevamp.saveROSTemplate(myJSON).done(function (response) {
            if (response != null && response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    if (response.ROSTemplateId != "") {
                        Clinical_ROSTemplateDetailRevamp.params.PhysicalExamTemplateId = response.ROSTemplateId;
                        for (var count in Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData) {
                            Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData[count];
                        }
                    }
                    //Clinical_ROSTemplateDetailRevamp.loadHospitalizationHx();

                    Clinical_ROSTemplateDetailRevamp.params.mode = "Edit";
                    // Binding SOAP Text
                    if (Clinical_ROSTemplateDetailRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
                        Clinical_ROSTemplateDetailRevamp.getReviewofSystemsInfo(response.ROSSOAPText, true);
                    } else {
                        utility.DisplayMessages(response.Message, 1);
                    }


                    // Empty global variables
                    Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = [];
                    Clinical_ROSTemplateDetailRevamp.providerCheckedIds = [];
                    Clinical_ROSTemplateDetailRevamp.providerSelectedIds = [];
                    Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData = [];
                    Clinical_ROSTemplateDetailRevamp.SpecialtyIds = "";
                    Clinical_ROSTemplateDetailRevamp.ProviderIds = "";
                    Clinical_ROSTemplateDetailRevamp.CharcSystemInfo = [];
                    Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail = [];
                    Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems = [];
                    Clinical_ROSTemplateDetailRevamp.ROSCharatristicsDetail = [];
                    Clinical_ROSTemplateDetailRevamp.ROSSystemDetail = [];
                    Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo = [];
                    Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative = [];
                    Clinical_ROSTemplateDetailRevamp.selectedObservations = [];

                    
                    UnloadActionPan(Clinical_ROSTemplateDetailRevamp.params.ParentCtrl, 'Clinical_ROSTemplateDetailRevamp');
                    if (Clinical_ROSTemplateDetailRevamp != null) {
                        Clinical_ROSTemplateDetailRevamp.loadROSTemplateMK();
                        Clinical_ROSTemplateDetailRevamp.selectedObservations = [];
                    }
                    //End Farooq Ahmad 16-03-2016 Unload the Physical Exam on Save
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                   
                    Clinical_ROSTemplateDetailRevamp.ROSCharatristicsDetail = [];
                    Clinical_ROSTemplateDetailRevamp.ROSSystemDetail = [];
                    
                }
            }
        });
        //}
        //else {
        //    utility.DisplayMessages("Please select characteristic/Sub-characteristic", 3);
        //}
    },
    getReviewofSystemsInfo: function (soapTextROS, UnloadReviewofSystems, reviewOfSystemId) {
        var ROSSystemInfoID = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #hfROSSystemInfoID').val();
        Clinical_ROSTemplateDetailRevamp.createReviewofSystemsBodyHTML(soapTextROS, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadReviewofSystems, ROSSystemInfoID);
    },
    createReviewofSystemsBodyHTML: function (soapTextROS, createReviewofSystemsBodyHTML, UnloadReviewofSystems, ROSSystemInfoID) {
        createReviewofSystemsBodyHTML = "#pnlClinicalProgressNote #ProgressnoteHTML";
        ROSSystemInfoID = "-1";
        Clinical_ROSTemplateDetailRevamp.checkReviewofSystemsExists();
        NoteHTMLCtrl = "#pnlClinicalProgressNote #ProgressnoteHTML";
        var $RosCntrlOnNotes = $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID);
        var $mainDivReviewofSystems = $(document.createElement('div'));
        var $SectionBodyReviewofSystems = $(document.createElement('section'));
        var $ListReviewofSystems = $(document.createElement('ul'));
        var $DetailsDiv = $(document.createElement('div'));

        $SectionBodyReviewofSystems.attr('id', "Cli_ReviewofSystems_Main" + ROSSystemInfoID);
        $DetailsDiv.attr('id', "Cli_ReviewofSystems_" + ROSSystemInfoID);
        $ListReviewofSystems.attr('class', 'list-unstyled')

        $SectionBodyReviewofSystems.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ReviewofSystems_" + ROSSystemInfoID + '"><i class="fa fa-edit"></i></a>' +
            '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_ReviewofSystems_Main" + ROSSystemInfoID + '"  ><i class="fa fa-times"></i></a></div> ');

        if (soapTextROS == null) {
            soapTextROS = '';
            Clinical_ProgressNote.saveComponentSOAPText('Review of Systems', true);
            return;
        }

        $ListReviewofSystems.append("<li>" + soapTextROS + "</li>");
        $DetailsDiv.append($ListReviewofSystems);
        $SectionBodyReviewofSystems.append($DetailsDiv);


        if ($RosCntrlOnNotes.length == 0) {

            $mainDivReviewofSystems.html($SectionBodyReviewofSystems);
            Clinical_ROSTemplateDetailRevamp.updateReviewofSystemsHtml($mainDivReviewofSystems.html(), ROSSystemInfoID, NoteHTMLCtrl);

        } else {

            var CommentHTML = "";
            var $CommentsID = $(NoteHTMLCtrl + ' Clinical_ROSTemplateDetailRevamp').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID + ' ul li:Last');
            if ($CommentsID.attr('id') != null && $CommentsID.attr('id').indexOf("Comments") >= 0) {
                CommentHTML = $CommentsID.get(0).outerHTML;
            }
            $RosCntrlOnNotes.html($SectionBodyReviewofSystems.html());
            $RosCntrlOnNotes.find('ul').append(CommentHTML);

            Clinical_ProgressNote.saveComponentSOAPText('Review of Systems', true);
            Clinical_ROSTemplateDetailRevamp.updateReviewofSystemsHtml("", ROSSystemInfoID, NoteHTMLCtrl);
            Clinical_ProgressNote.hoverFunction();
        }

        if (UnloadReviewofSystems == true) {
            Clinical_ROSTemplateDetailRevamp.UnloadReviewofSystems(true);
        }
    },
    updateReviewofSystemsHtml: function (ReviewofSystemsHtml, ROSSystemInfoID, NoteHTMLCtrl) {
        var $RosControlOnNotes = $(NoteHTMLCtrl + ' Clinical_Reviewofsystems').parent().parent(); //$(NoteHTMLCtrl + ' Clinical_Reviewofsystems').parent().parent().length
        $RosControlOnNotes.addClass('initialVisitBody');
        if (ReviewofSystemsHtml != '') {
            if ($RosControlOnNotes.find('section').length > 0) {
                $RosControlOnNotes.find('section').remove();
                $RosControlOnNotes.append(ReviewofSystemsHtml);

            } else {
                $RosControlOnNotes.append(ReviewofSystemsHtml);
            }

        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ReviewofSystemsHtml != '') {
            Clinical_ProgressNote.saveComponentSOAPText('Review of Systems', true );
            Clinical_ProgressNote.hoverFunction();
        }

    },
    createReviewofSystemsBodyHTMLFromnotes: function (soapTextROS, NoteHTMLCtrl, UnloadReviewofSystems, ROSSystemInfoID) {
        Clinical_ROSTemplateDetailRevamp.checkReviewofSystemsExists(ROSSystemInfoID);
        if (soapTextROS == null) {
            soapTextROS = '';
            return;
        }

        var $mainDivReviewofSystems = $(document.createElement('div'));
        var $SectionBodyReviewofSystems = $(document.createElement('section'));
        var $DetailsDiv = $(document.createElement('div'));
        var $ListReviewofSystems = $(document.createElement('ul'));

        $DetailsDiv.attr('id', "Cli_ReviewofSystems_" + ROSSystemInfoID);
        $SectionBodyReviewofSystems.attr('id', "Cli_ReviewofSystems_Main" + ROSSystemInfoID);
        $ListReviewofSystems.attr('class', 'list-unstyled')

        $SectionBodyReviewofSystems.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ReviewofSystems_" + ROSSystemInfoID + '"><i class="fa fa-edit"></i></a>' +
            '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_ReviewofSystems_Main" + ROSSystemInfoID + '"  ><i class="fa fa-times"></i></a></div> ');

        $ListReviewofSystems.append("<li>" + soapTextROS + "</li>");
        $DetailsDiv.append($ListReviewofSystems);
        $SectionBodyReviewofSystems.append($DetailsDiv);
        if ($(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID).length == 0) {
            $mainDivReviewofSystems.html($SectionBodyReviewofSystems);

            var ReviewofSystemsHtml = $mainDivReviewofSystems.html();
            if (ReviewofSystemsHtml != '') {
                $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().addClass('initialVisitBody');
                if ($(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('section').length > 0) {
                    $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('section').remove();
                    $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().append(ReviewofSystemsHtml);

                } else {
                    $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().append(ReviewofSystemsHtml);
                }
            }

        } else {

            var CommentHTML = "";
            var CommentsID = $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID + ' ul li:Last').attr('id');
            if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                CommentHTML = $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID + ' ul li:Last').get(0).outerHTML;
            }
            $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID).html($SectionBodyReviewofSystems.html());
            $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID + ' ul').append(CommentHTML);

        }

        Clinical_ProgressNote.hoverFunction();
    },
    checkReviewofSystemsExists: function (ROSTemplateId) {
        if (ROSTemplateId)
        {
            Clinical_ROSTemplateDetailRevamp.params.TemplateId = ROSTemplateId;
        }
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').length > 0) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML section[id*="Cli_ReviewofSystems_Main"]').remove();
        }
        var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
        if (Clinical_ProgressNote.params["TemplateName"] != '')
            CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').length == 0) {
            $(CompnentSelector).append(' <li class="ReviewofSystemsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_ReviewofSystems title="Review of Systems"  id="' + this.id + '" class="NotesComponent">' +
                        '<a id= "ReviewofSystemsRevmapHeaderFromSOAP" class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'ReviewofSystemsRevmap\',' + Clinical_ROSTemplateDetailRevamp.params.TemplateId + ',' + Clinical_ProgressNote.params.NotesId + ',\'LoadFromNote\');" title="Review of Systems">Review of Systems</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'ReviewofSystems\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'ReviewofSystemsRevmap\',' + Clinical_ROSTemplateDetailRevamp.params.TemplateId + ',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_ReviewofSystems> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });


        }
        else {
            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').parent().parent("li").hasClass('hidden'))
            {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').find("#ReviewofSystemsRevmapHeaderFromSOAP").attr('onClick', "Clinical_ProgressNote.SelectNotesComponentTab('ReviewofSystemsRevmap'," + Clinical_ROSTemplateDetailRevamp.params.TemplateId + "," + Clinical_ProgressNote.params.NotesId + ",'LoadFromNote');");
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems a.closeBtn').attr('onClick', "Clinical_ProgressNote.RemoveComponentTab('ReviewofSystemsRevmap'," + Clinical_ROSTemplateDetailRevamp.params.TemplateId + "," + Clinical_ProgressNote.params.NotesId + ");");
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').parent().parent().removeClass('hidden');
            }
            else {
                var htmlString = '<clinical_ReviewofSystems title="Review of Systems"  id="' + this.id + '" class="NotesComponent">' +
                        '<a id= "ReviewofSystemsRevmapHeaderFromSOAP" class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'ReviewofSystemsRevmap\',' + Clinical_ROSTemplateDetailRevamp.params.TemplateId + ',' + Clinical_ProgressNote.params.NotesId + ',\'LoadFromNote\');" title="Review of Systems">Review of Systems</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'ReviewofSystems\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'ReviewofSystemsRevmap\',' + Clinical_ROSTemplateDetailRevamp.params.TemplateId + ',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                 '</clinical_ReviewofSystems>';
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').replaceWith(htmlString);
            }   
        }
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },
    UnloadReviewofSystems: function (nextOrPre) {

        //Clinical_ReviewofSystems.clearCachingROS();

        if (Clinical_ROSTemplateDetailRevamp.params["FromAdmin"] == "0") {

            if (Clinical_ROSTemplateDetailRevamp.params != null && Clinical_ReviewofSystems.params.ParentCtrl != null) {

                if (Clinical_ROSTemplateDetailRevamp.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {

                    UnloadActionPan(Clinical_ROSTemplateDetailRevamp.params.ParentCtrl, 'Clinical_ROSTemplateDetailRevamp');

                    if (Clinical_ROSTemplateDetailRevamp.controlToInvoke != null) {

                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_ROSTemplateDetailRevamp.controlToInvoke.replace(/\s/g, ''));
                            Clinical_ROSTemplateDetailRevamp.controlToInvoke = null;
                        }, 600);

                    }

                }
                else {
                    UnloadActionPan(Clinical_ROSTemplateDetailRevamp.params.ParentCtrl, 'Clinical_ROSTemplateDetailRevamp');
                }
            }
            else
                UnloadActionPan(null, 'Clinical_ReviewofSystems');
        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_ReviewofSystems").remove();
            RemoveAdminTab();
        }
        EMRUtility.scrollToPNcomponent('clinical_reviewofsystems');
    },
    loadROSTemplateMK: function () {
        Clinical_ROSTemplateDetailRevamp.ROSTemplateLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var ROSTemplateJSONData = JSON.parse(response.ROSTemplate); //Parsing array to JSON
                ROSTemplateJSONData = JSON.parse(ROSTemplateJSONData)
                $.each(ROSTemplateJSONData, function (index, item) {
                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#gvTemplates_row" + item.ROSTemplateId + "'));Clinical_ROSTemplateDetailRevamp.loadROSAgainstTemplate(" + item.ROSTemplateId + ");");
                    $row.attr("id", "gvTemplates_row" + item.ROSTemplateId);
                    $row.append('<td style="display:none;">' + item.ROSTemplateId + '</td><td><a class="btn  btn-xs" href="#"  title="Select Template"><i class="fa fa-check black"></i></a>&nbsp;</td><td>' + item.Name + '</td>');
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #dgvClinicalNotesTemplates tbody').last().append($row);
                });
                //Clinical_ROSTemplateDetailRevamp.ROSTemplateGridLoadMK(response);
                //var TableControl = Clinical_ROSTemplateDetailRevamp.params.PanelID + " #pnlClinical_ROSTemplateDetailRevamp_Result #dgvPhysExamTemplates";
                //var PagingPanelControlID = Clinical_ROSTemplateDetailRevamp.params.PanelID + " #divProviderPaging";
                //var ClassControlName = "Clinical_ROSTemplateDetailRevamp";
                //var PagesToDisplay = 5;
                //var iTotalDisplayRecords = response.iTotalDisplayRecords;
                ////setTimeout(
                ////    CreatePagination(response.PhysExamTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                ////        Clinical_ROSTemplateDetailRevamp.PhysExamTemplateSearch(PrimaryID, PageNumber, ResultPerPage);
                ////    }), 10);
                $('#frmClinicalROSTemplateDetailRevamp').data('serialize', $('#frmClinicalROSTemplateDetailRevamp').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    DeleteROSTemplateFromNote: function (TemplateId, NotesId) {
        var objData = new Object();
        objData["TemplateId"] = TemplateId;
        objData["NoteId"] = NotesId;
        objData["commandType"] = "delete_reviewofsystem_template_note"; //DELETE_PhysicalExamTemplate
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ReviewofSystemRevampTemplate");
    },
    //ROSTemplateDeleteFromNote: function (TemplateId, NotesId, event) {
    //    Clinical_ROSTemplateDetailRevamp.DeleteROSTemplateFromNote(TemplateId, NotesId).done(function (response) {
    //                response = JSON.parse(response);
    //                if (response.status != false) {
    //                    return true;
                       
    //                }
    //                else {
    //                    return false;
    //                }
    //            });
           


    //        event.stopPropagation();
        
       
     
    //},
    ROSTemplateDeleteFromNote: function (ComponentName, IsUpdate, ReviewofSystemsComponentRemove, TemplateId, NotesId) {

        utility.myConfirm('28', function () {
            Clinical_ROSTemplateDetailRevamp.DeleteROSTemplateFromNote(TemplateId, NotesId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ReviewofSystemsComponent').attr('NoteComponentId');
                    if (ReviewofSystemsComponentRemove) {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Review of Systems']").remove();
                        if (Clinical_ProgressNote.params["TemplateName"])
                        {
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').parent().parent().addClass('hidden');
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').find("#ReviewofSystemsRevmapHeaderFromSOAP").attr('onClick', "Clinical_ProgressNote.SelectNotesComponentTab('ReviewofSystemsRevmap',-1," + NotesId + ",'LoadFromNote');");
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').find("a#ReviewofSystemsRevmapHeaderFromSOAP").attr('onClick', "Clinical_ProgressNote.SelectNotesComponentTab('ReviewofSystemsRevmap',-1," + NotesId + ",'LoadFromNote');");
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').find("a#ReviewofSystemsRevmapHeaderFromSOAP").attr('onClick', "Clinical_ProgressNote.SelectNotesComponentTab('ReviewofSystemsRevmap',-1," + NotesId + ",'LoadFromNote');");
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems a.closeBtn').attr('onClick', "Clinical_ProgressNote.RemoveComponentTab('ReviewofSystemsRevmap',-1," + NotesId + ");");
                        }
                        else
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').parent().parent().remove();
                    } else {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').parent().parent().find('section[id*="Cli_ReviewofSystems_Main"]').remove();
                    }
                    if (IsUpdate) {
                        if (NoteComponentId && NoteComponentId != "NCDummyId") {
                            var promise = [];
                            if (Clinical_ProgressNote.params["TemplateName"]) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_ReviewofSystems').parent().parent().addClass('hidden');
                                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Review of Systems', true))
                            }
                            else
                                promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                            $.when.apply($, promise).done(function () {
                                if (Clinical_ProgressNote.params["TemplateName"] == "")
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_ReviewofSystems').parent().parent().remove();
                                Clinical_ProgressNote.ShowHideComponetsHeaders();
                                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                            });
                        }
                        Clinical_ProgressNote.hoverFunction();
                    }
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }, function () {
        }, '1');
    },
    disAssociateSystemsAgainstNoteId: function () {
        var objData = {};
        objData["NotesId"] = Clinical_Notes.params.NotesId;
        objData["ROSSystemInfoID"] = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hfROSSystemInfoID').val();
        objData["commandType"] = "disAssociate_Systems_AgainstNoteId";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewofSystem", "ReviewofSystems");
    },
    loadROSAgainstTemplate: function (ROSTemplateId) {
        var PrevROSTemplateId = Clinical_ROSTemplateDetailRevamp.params.ROSTemplateId;
        
        var message = "Would you like to change & overwrite the existing ROS Template ?";
        if ( PrevROSTemplateId > 0 && ROSTemplateId != PrevROSTemplateId) {
            utility.myConfirm(message, function () {
                Clinical_ROSTemplateDetailRevamp.clearCachingROS();
                Clinical_ROSTemplateDetailRevamp.LoadROS(ROSTemplateId);
                Clinical_ROSTemplateDetailRevamp.params.ROSTemplateId = ROSTemplateId;
                Clinical_ROSTemplateDetailRevamp.hidepan();

            }, "", "Confirm Changes");
        } else if (PrevROSTemplateId == 0 && ROSTemplateId != PrevROSTemplateId)
        {
            Clinical_ROSTemplateDetailRevamp.params.ROSTemplateId = ROSTemplateId;
            Clinical_ROSTemplateDetailRevamp.LoadROS(ROSTemplateId);
            Clinical_ROSTemplateDetailRevamp.hidepan();
        } else {
            Clinical_ROSTemplateDetailRevamp.hidepan();
        }
    },
    hidepan: function () {
        $("#ListSection").addClass('hidden');
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #btnPhysicalExamSave ').parent().removeClass('hidden');
        $("#templatesection").removeClass('hidden');
    
    },
    clearCachingROS: function()
    {
        Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = [];
        Clinical_ROSTemplateDetailRevamp.providerCheckedIds = [];
        Clinical_ROSTemplateDetailRevamp.providerSelectedIds = [];
        Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData = [];
        Clinical_ROSTemplateDetailRevamp.SpecialtyIds = "";
        Clinical_ROSTemplateDetailRevamp.ProviderIds = "";
        Clinical_ROSTemplateDetailRevamp.CharcSystemInfo = [];
        Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail = [];
        Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems = [];
        Clinical_ROSTemplateDetailRevamp.ROSCharatristicsDetail = [];
        Clinical_ROSTemplateDetailRevamp.ROSSystemDetail = [];
        Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo = [];
        Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative = [];
        Clinical_ROSTemplateDetailRevamp.selectedObservations = [];
    },
    ROSTemplateLoad: function () {
        var objData = new Object();
        var IsActive = null;
        IsActive = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #pnlClinical_ROSTemplateDetailRevamp_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }
        var entityId = 0;
        if (globalAppdata.AppUserName.toLowerCase() != DefaultUser.toLowerCase()) {
            entityId = globalAppdata["SeletedEntityId"];
        }
        objData["NoteId"] = Clinical_ROSTemplateDetailRevamp.params.NotesId;
        objData["IsActive"] = IsActive;
        objData["EntityId"] = entityId;
        objData["commandType"] = "load_reviewofsystem_patientinfo";// "Load_ROSRevamp_Template";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ReviewofSystemRevampTemplate");
    },
    ROSTemplateGridLoadMK: function (response) {
        var isactivebtn = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #pnlClinical_ROSTemplateDetailRevamp_Result #divSwitch #switchActive').attr('isactive');
        try {
            $("#" + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #pnlClinical_ROSTemplateDetailRevamp_Result #dgvPhysExamTemplates").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        } catch (ex) {
            console.log(ex);
        }
        $("#" + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.PhysicalExamTemplateCount > 0) {
            var PhysicalExamTemplateJSONData = JSON.parse(response.PhysicalExamTemplate); //Parsing array to JSON
            PhysicalExamTemplateJSONData = JSON.parse(PhysicalExamTemplateJSONData)
            $.each(PhysicalExamTemplateJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "Clinical_ROSTemplateDetailRevamp.ROSTemplateEdit('" + item.TemplateId + "'," + item.IsDefault.toLowerCase() + ",event);");
                $row.attr("id", "dgvPhysExamTemplates" + item.ROSTemplateId);
                $row.attr("TemplateId", item.ROSTemplateId);
                var IsDefault = false;

                var editCall = "Clinical_ROSTemplateDetailRevamp.ROSTemplateEdit('" + item.ROSTemplateId + "'," + IsDefault + ",event);";
                $row.attr("onclick", editCall);
                if (item.IsActive == "True") {
                    isactive = 1;
                    isEventactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isEventactive = 1;
                    isactive = 0;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var dateInNumberFormat = Number(new Date(item.ModifiedOn));

                $row.append('<td class="hidden">' + dateInNumberFormat + '</td><td style="display:none;">' + item.ROSTemplateId + '</td><td>&nbsp;&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_ROSTemplateDetailRevamp.ROSTemplateActiveInactive(\'' + item.ROSTemplateId + '\',' + IsDefault + ', ' + isEventactive + ',event);"><i class="fa fa-check black"></i></a></td><td>' + item.Name + '</td>');

                $("#" + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates').DataTable({
                "language": {
                    "emptyTable": "No ROS Template Found"
                }, "autoWidth": false, "bLengthChange": false, "order": [[0, "desc"]]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates'))
            ;
        else {
            $("#" + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #pnlROSTemplateRevamp_Result #dgvPhysExamTemplates").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]] });
        }
        var checked = '';
        if (isactivebtn == "0" || isactivebtn == 0) {
        } else if (isactivebtn == null) {
            isactivebtn = "1";
            checked = 'checked="checked"';
        } else {
            isactivebtn = "1";
            checked = 'checked="checked"';
        }
        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                       '<input id="switchActive" isactive="' + isactivebtn + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: none;" onchange="Clinical_ROSTemplateDetailRevamp.activeROSTemplateSearch(this);">' +
                        '</div><span class="pl-xs">Active</span>';

      //  $("#" + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();

    },
    validateROSTemplate: function () {
        $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp')
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
            Clinical_ROSTemplateDetailRevamp.ROSTemplateSave();
        });
    },

    saveROSTemplate: function (PhysicalExamTemplateData, TemplateName) {
        var self = null, IsActive = null;
        self = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        
        objData["TemplateId"] = (Clinical_ROSTemplateDetailRevamp.params["PhysicalExamTemplateId"]);

        if (TemplateName == null || typeof (TemplateName) == "undefined") {
            var mainTemplateName = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #txtPhysicalExamTemplateName').val();

            if (objData["TemplateId"] == '-1') {
                objData["Name"] = TemplateName;
                IsActive = "1";
            }
            else {
                objData["Name"] = mainTemplateName != "" ? mainTemplateName : TemplateName;
            }

            objData["SaveAsName"] = TemplateName;
        }

        //------------------------------------------------------

        var SpecialtyIds = self.find('#ddlPhysicalExamTemplateSpecialty option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["SpecialtyIds"] = SpecialtyIds;

        var ProviderIds = self.find('#ddlPhysicalExamTemplateProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["ProviderIds"] = ProviderIds;
        Clinical_ROSTemplateDetailRevamp.params.TemplateId = Clinical_ROSTemplateDetailRevamp.params.ROSTemplateId;
        
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
        for (var i = 0 ; i < Clinical_ROSTemplateDetailRevamp.selectedObservations.length; i++) {
            if (Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted == 0 && Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted == 0) {

                indexesforchardetail = $.map(Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo, function (item, index) {
                    if (item.Id == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                        return index;
                    }
                });
                var detail = "";
                var details = "";
                if (indexesforchardetail.length > 0) {
                    var detialIndex = indexesforchardetail[0];
                    detail = Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo[detialIndex].DetailInfo;
                    details = JSON.parse(detail)
                }

                var IsCharPositive = null;
                var CharComments = null;

                indexesforcharposNeg = $.map(Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail, function (item, index) {
                    if (item.CharcId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId && item.SystemId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                        return index;
                    }
                })

                if (indexesforcharposNeg.length > 0) {
                    var detialIndexForPostitiveNegative = indexesforcharposNeg[0];

                    if (Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsNegative == true) {
                        IsCharPositive = false;
                    }
                    else if (Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].IsPositive == true) {
                        IsCharPositive = true;
                    }
                    else {
                        IsCharPositive = null;
                    }
                    CharComments = Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail[detialIndexForPostitiveNegative].CharcComments;
                }

                if (details) {
                    var objSelectedObservationsWithDetailAll = {
                        ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                        ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
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
                        ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                        IsChecked: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked,
                        ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                        ROSCharacteristicsName: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsName,
                        IsSystemChecked: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked,
                        IsSystemDeleted: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted,
                        IsObservationDeleted: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted,
                        IsPositive: IsCharPositive,
                        CharComments: CharComments,

                    };

                    Clinical_ROSTemplateDetailRevamp.ROSCharatristicsDetail.push(objSelectedObservations);

                    if (details["PreviousHistory"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.PreviousHistory,
                            LookupId: $('#hfPreviousHistory').val(),
                            Value: details["PreviousHistory"],
                            LookupTypeName: 'PreviousHistory',
                            Text: "",
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.PreviousHistory,
                            LookupId: $('#hfPreviousHistory').val(),
                            Value: details["PreviousHistory"],
                            LookupTypeName: 'PreviousHistory',
                            Text: "",
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailStatusId"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.StatusId,
                            LookupId: details["ROSCharacteristicsDetailStatusId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailStatus',
                            Text: "",
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.StatusId,
                            LookupId: details["ROSCharacteristicsDetailStatusId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailStatus',
                            Text: "",
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["Onset"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.Onset,
                            LookupId: $('#hfROSOnset').val(),
                            Value: details["Onset"],
                            LookupTypeName: 'ROSOnset',
                            Text: "",
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.Onset,
                            LookupId: $('#hfROSOnset').val(),
                            Value: details["Onset"],
                            LookupTypeName: 'ROSOnset',
                            Text: "",
                            IsDeleted:  1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["Duration"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.Duration,
                            LookupId: $('#hfROSDuration').val(),
                            Value: details["Duration"],
                            LookupTypeName: 'ROSDuration',
                            Text: "",
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.Duration,
                            LookupId: $('#hfROSDuration').val(),
                            Value: details["Duration"],
                            LookupTypeName: 'ROSDuration',
                            Text: "",
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["ROSCharacteristicsDetailDurationId"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.DurationId,
                            LookupId: details["ROSCharacteristicsDetailDurationId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailDuration',
                            Text: details["ROSCharacteristicsDetailDurationId_text"],
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.DurationId,
                            LookupId: details["ROSCharacteristicsDetailDurationId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailDuration',
                            Text: details["ROSCharacteristicsDetailDurationId_text"],
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["ROSCharacteristicsDetailPatternId"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.PatternId,
                            LookupId: details["ROSCharacteristicsDetailPatternId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailPattern',
                            Text: details['ROSCharacteristicsDetailPatternId_text'],
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.PatternId,
                            LookupId: details["ROSCharacteristicsDetailPatternId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailPattern',
                            Text: details['ROSCharacteristicsDetailPatternId_text'],
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["ROSCharacteristicsDetailSeverityId"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.SeverityId,
                            LookupId: details["ROSCharacteristicsDetailSeverityId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailSeverity',
                            Text: details["ROSCharacteristicsDetailSeverityId_text"],
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.SeverityId,
                            LookupId: details["ROSCharacteristicsDetailSeverityId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailSeverity',
                            Text: details["ROSCharacteristicsDetailSeverityId_text"],
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["ROSCharacteristicsDetailCourseId"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.CourseId,
                            LookupId: details["ROSCharacteristicsDetailCourseId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailCourse',
                            Text: details["ROSCharacteristicsDetailCourseId_text"],
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.CourseId,
                            LookupId: details["ROSCharacteristicsDetailCourseId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailCourse',
                            Text: details["ROSCharacteristicsDetailCourseId_text"],
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    if (details["ROSCharacteristicsDetailRadiationId"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.RadiationId,
                            LookupId: details["ROSCharacteristicsDetailRadiationId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailRadiation',
                            Text: details["ROSCharacteristicsDetailRadiationId_text"],
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.RadiationId,
                            LookupId: details["ROSCharacteristicsDetailRadiationId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailRadiation',
                            Text: details["ROSCharacteristicsDetailRadiationId_text"],
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["ROSCharacteristicsDetailFrequencyId"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.FrequencyId,
                            LookupId: details["ROSCharacteristicsDetailFrequencyId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailFrequency',
                            Text: details["ROSCharacteristicsDetailFrequencyId_text"],
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.FrequencyId,
                            LookupId: details["ROSCharacteristicsDetailFrequencyId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailFrequency',
                            Text: details["ROSCharacteristicsDetailFrequencyId_text"],
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["ROSCharacteristicsDetailContextId"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.ContextId,
                            LookupId: details["ROSCharacteristicsDetailContextId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailContext',
                            Text: details["ROSCharacteristicsDetailContextId_text"],
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.ContextId,
                            LookupId: details["ROSCharacteristicsDetailContextId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailContext',
                            Text: details["ROSCharacteristicsDetailContextId_text"],
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["ROSCharacteristicsDetailCharacterCSZId"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.CharacterCSZId,
                            LookupId: details["ROSCharacteristicsDetailCharacterCSZId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailCharacterCSZ',
                            Text: details["ROSCharacteristicsDetailCharacterCSZId_text"],
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.CharacterCSZId,
                            LookupId: details["ROSCharacteristicsDetailCharacterCSZId"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailCharacterCSZ',
                            Text: details["ROSCharacteristicsDetailCharacterCSZId_text"],
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["ROSCharacteristicsDetailAggravedById"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.AggravedById,
                            LookupId: details["ROSCharacteristicsDetailAggravedById"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailAggravedBy',
                            Text: details["ROSCharacteristicsDetailAggravedById_text"],
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.AggravedById,
                            LookupId: details["ROSCharacteristicsDetailAggravedById"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailAggravedBy',
                            Text: details["ROSCharacteristicsDetailAggravedById_text"],
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["ROSCharacteristicsDetailRelievedById"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.RelievedById,
                            LookupId: details["ROSCharacteristicsDetailRelievedById"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailRelievedBy',
                            Text: details["ROSCharacteristicsDetailRelievedById_text"],
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.RelievedById,
                            LookupId: details["ROSCharacteristicsDetailRelievedById"],
                            Value: "",
                            LookupTypeName: 'ROSCharacteristicsDetailRelievedBy',
                            Text: details["ROSCharacteristicsDetailRelievedById_text"],
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["Location"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.Location,
                            LookupId: $('#hfLocation').val(),
                            Value: details["Location"],
                            LookupTypeName: 'Location',
                            Text: "",
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.Location,
                            LookupId: $('#hfLocation').val(),
                            Value: details["Location"],
                            LookupTypeName: 'Location',
                            Text: "",
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["PrecipitatedBY"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.PrecipitatedBY,
                            LookupId: $('#hfPrecipitatedBy').val(),
                            Value: details["PrecipitatedBY"],
                            LookupTypeName: 'PrecipitatedBy',
                            Text: "",
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.PrecipitatedBY,
                            LookupId: $('#hfPrecipitatedBy').val(),
                            Value: details["PrecipitatedBY"],
                            LookupTypeName: 'PrecipitatedBy',
                            Text: "",
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                    if (details["AssociatedWith"]) {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.AssociatedWith,
                            LookupId: $('#hfAssociatedBy').val(),
                            Value: details["AssociatedWith"],
                            LookupTypeName: 'AssociatedBy',
                            Text: "",
                            IsDeleted: 0
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
                    }
                    else {
                        var objSelectedObservationsWithDetail = {
                            ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                            ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                            OldLookupId: Clinical_ROSTemplateDetailRevamp.ROSTempSysCharDetailOldLookupValues.AssociatedWith,
                            LookupId: $('#hfAssociatedBy').val(),
                            Value: details["AssociatedWith"],
                            LookupTypeName: 'AssociatedBy',
                            Text: "",
                            IsDeleted: 1
                        };
                        ROSCharaDetailGenral.push(objSelectedObservationsWithDetail);
}
                }
                else {
                    var objSelectedObservations = {
                        ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                        IsChecked: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsChecked,
                        ROSCharacteristicsId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsId,
                        ROSCharacteristicsName: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSCharacteristicsName,
                        IsSystemChecked: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked,
                        IsSystemDeleted: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted,
                        IsObservationDeleted: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted,
                        IsPositive: IsCharPositive,
                        CharComments: CharComments,
                    };

                    Clinical_ROSTemplateDetailRevamp.ROSCharatristicsDetail.push(objSelectedObservations);
                }

                indexIsSystemExist = $.map(Clinical_ROSTemplateDetailRevamp.ROSSystemDetail, function (item, index) {
                    if (item.ROSSystemId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                        return index;
                    }
                })
                if (indexIsSystemExist.length == 0) {
                    indexesforisnormalsys = $.map(Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems, function (item, index) {
                        if (item.SystemId == Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId) {
                            return index;
                        }
                    })
                    var IsNormalSystem = 'false';
                    var IsNormalComments = null;
                    if (indexesforisnormalsys.length > 0) {
                        var indexforIsNormal = indexesforisnormalsys[0];

                        if (Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems[indexforIsNormal].IsNormal == true) {
                            IsNormalSystem = "true";
                        }

                        IsNormalComments = Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems[indexforIsNormal].IsNormalComments;
                    }

                    var objSystemWithDetail = {
                        ROSSystemId: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].ROSSystemId,
                        IsChecked: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].IsSystemChecked,
                        IsNormalComments: IsNormalComments,
                        IsNormal: IsNormalSystem,
                        SystemName: Clinical_ROSTemplateDetailRevamp.selectedObservations[i].SystemName,
                    };

                    objSystemWithDetail.RosChartristicsDetail = new Array();
                    objSystemWithDetail.RosChartristicsDetail.push(objSelectedObservations);
                    Clinical_ROSTemplateDetailRevamp.ROSSystemDetail.push(objSystemWithDetail);
                }
                else {
                    objSystemWithDetail.RosChartristicsDetail.push(objSelectedObservations);
                }
            }
        }

        // For empty System
        if (Clinical_ROSTemplateDetailRevamp.ROSSystemDetail.length != $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems   li').length) {
            var listofsystems = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystems li')

            for (var i = 0 ; i < listofsystems.length; i++) {
                var ss = Clinical_ROSTemplateDetailRevamp.ROSSystemDetail.filter(function (obj) {
                    return obj.ROSSystemId == listofsystems[i].id;
                });
                if (ss.length == 0) {
                    var objSystemWithDetail = {
                        ROSSystemId: listofsystems[i].id,
                        IsChecked: false,
                        IsNormalComments: null,
                        IsNormal: 'false',
                    };

                    Clinical_ROSTemplateDetailRevamp.ROSSystemDetail.push(objSystemWithDetail);
                }
            }
        }

        var IsNormalGlobal = null;
        var IsNormalGlobalComments = null;

        if ($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #chkReviewofSystemssNormal').prop('checked') == true) {
            IsNormalGlobal = true;
        }
        else {
            IsNormalGlobal = false;
            IsNormalGlobalComments = "";
        }

        objData["IsNormalAll"] = IsNormalGlobal;
        objData["ROSCharaDetailGenral"] = ROSCharaDetailGenral;
        //--------------------end detail values------------------//
        objData["commandType"] = "insert_reviewofsystem_template_note";
        objData["NoteId"] = Clinical_ROSTemplateDetailRevamp.params.NotesId;
        if ($('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #dgvClinicalNotesTemplates tr.active').attr('id')) {
            objData["TemplateId"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #dgvClinicalNotesTemplates tr.active').attr('id').split('gvTemplates_row').join("");
        }
        else {
            //when user selects single system, we get this id from Clinical_ProgressNote.OpenROSSysCharcDetail()
            objData["TemplateId"] = Clinical_ROSTemplateDetailRevamp.params.ROSTemplateId;
        }
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["SystemChartristicsdetail"] = ROSCharaDetail;
        objData["ROSSystems"] = Clinical_ROSTemplateDetailRevamp.ROSSystemDetail;
        objData["Comments"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #txtComments').val();
        objData["Name"] = $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + ' #frmClinicalROSTemplateDetailRevamp #txtPhysicalExamTemplateName').val();

        IsActive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }
        objData["IsActive"] = IsActive;
        if (Clinical_ROSTemplateDetailRevamp.params.ComeFrom == 'ROSSystemDetail')
            objData["IsRecordDelete"] = false;
        else
            objData["IsRecordDelete"] = true;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ReviewofSystemRevampTemplate");
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
        Clinical_ROSTemplateDetailRevamp.specialityCheckedIds = [];
        Clinical_ROSTemplateDetailRevamp.providerCheckedIds = [];
        Clinical_ROSTemplateDetailRevamp.providerSelectedIds = [];
        Clinical_ROSTemplateDetailRevamp.selectedPhyExamTempData = [];
        Clinical_ROSTemplateDetailRevamp.SpecialtyIds = "";
        Clinical_ROSTemplateDetailRevamp.ProviderIds = "";
        Clinical_ROSTemplateDetailRevamp.CharcSystemInfo = [];
        Clinical_ROSTemplateDetailRevamp.PositiveNegativeDetail = [];
        Clinical_ROSTemplateDetailRevamp.IsNornalAllSystems = [];
        Clinical_ROSTemplateDetailRevamp.ROSCharatristicsDetail = [];
        Clinical_ROSTemplateDetailRevamp.ROSSystemDetail = [];
        Clinical_ROSTemplateDetailRevamp.CharacteristicsDetailsInfo = [];
        Clinical_ROSTemplateDetailRevamp.SystemAllPositiveNegative = [];
        Clinical_ROSTemplateDetailRevamp.selectedObservations = []

        utility.UnLoadDialog("frmClinicalROSTemplateDetailRevamp", function () {

            UnloadActionPan(Clinical_ROSTemplateDetailRevamp.params["ParentCtrl"], "Clinical_ROSTemplateDetailRevamp");
            if (PhysicalExamTemplate != null) {
                Clinical_ROSTemplateDetailRevamp.loadROSTemplateMK();
                Clinical_ROSTemplateDetailRevamp.selectedObservations = [];
                Clinical_ROSTemplateDetailRevamp.specialtyIdMKMK = [];
                Clinical_ROSTemplateDetailRevamp.ProviderIdMKMK = [];
            }
        }, function () {

            UnloadActionPan(Clinical_ROSTemplateDetailRevamp.params["ParentCtrl"], "Clinical_ROSTemplateDetailRevamp");
        });
        if (Clinical_ROSTemplateDetailRevamp.params.ParentCtrl == "EncounterChargeCapture") {
            var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
            var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
            EditableGrid.initialize(ChargeGridId, EncounterChargeCapture, "0", false, false, false, false, undefined, false);
        }

    },
    bindCharacteristicsAutoComplete: function () {
        Clinical_ROSTemplateDetailRevamp.lookupCharacteristics_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Observation_JSON = JSON.parse(response.ObservationLoad_JSON);
                    $('#' + Clinical_ROSTemplateDetailRevamp.params.PanelID + " #txtCharacteristic").kendoAutoComplete({
                        dataSource: Observation_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Characteristic...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulPhysicalExamSystemSection").find('#' + dataItem.ROSCharacteristicsId).length == 0) {
                                var ROSSystemId = $("#ulPhysicalExamSystems li.active")[0].id;
                                var li = Clinical_ROSTemplateDetailRevamp.addCharatristics(dataItem.ROSCharacteristicsId, dataItem.Name, ROSSystemId);
                                $("#ulPhysicalExamSystemSection").append(li);
                                var objSelectedObservations = {
                                    ROSSystemId: ROSSystemId, IsChecked: false, ROSCharacteristicsId: dataItem.ROSCharacteristicsId, ROSCharacteristicsName: dataItem.Name, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0
                                };
                                Clinical_ROSTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
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