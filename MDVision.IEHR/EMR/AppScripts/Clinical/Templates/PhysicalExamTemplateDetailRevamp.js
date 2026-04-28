PhysicalExamTemplateDetailRevamp = {
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
    deleteObservations: [],
    specialtyIdMKMK: "",
    ProviderIdMKMK: "",
    OrthopedicSpecialitySelected: false,

    Load: function (params) {

        PhysicalExamTemplateDetailRevamp.params = params;
        var isSelectedEntity = false
        PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData = [];
        PhysicalExamTemplateDetailRevamp.selectedObservations = [];
        PhysicalExamTemplateDetailRevamp.deleteObservations = [];
        var self = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #tblPhysicalExamTemplateDetailRevamp');

        self.loadDropDowns(true).done(function () {
            if (PhysicalExamTemplateDetailRevamp.params["mode"] == "Edit") {
                var dfd = new $.Deferred();
                PhysicalExamTemplateDetailRevamp.loadDropDowns(dfd);
                dfd.done(function (n) {
                    PhysicalExamTemplateDetailRevamp.LoadPE();
                });
            }
            else {
                PhysicalExamTemplateDetailRevamp.loadDropDowns();
            }
            PhysicalExamTemplateDetailRevamp.buildSystemsAutoComplete();
            // PRD-5 ,Dev By:MAhmad 
            PhysicalExamTemplateDetailRevamp.bindobservationAutoComplete();
            // PRD-5 ,Dev By:MAhmad 
            PhysicalExamTemplateDetailRevamp.validatePhysicalExamTemplate();
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
            PhysicalExamTemplateDetailRevamp.toggleControls();
            //serialize Data after all controls loaded.
            $('#frmPhysicalExamTemplateDetailRevamp').data('serialize', $('#frmPhysicalExamTemplateDetailRevamp').serialize());
        });
    },

    loadDropDowns: function (dfd) {
        var self = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #tblPhysicalExamTemplateDetailRevamp');
        if (globalAppdata.AppUserName.toLowerCase() != DefaultUser.toLowerCase()) {
            self.find("#ddlPhysicalExamTemplateEntity").val(globalAppdata["SeletedEntityId"]);
            if (self.find("div#divEntity").hasClass("hidden") == false)
                self.find("div#divEntity").addClass("hidden");
            isSelectedEntity = true;
        }
        else {
            self.find("div#divEntity").removeClass("hidden");
        }
        PhysicalExamTemplateDetailRevamp.loadEntitySpecialty(globalAppdata["SeletedEntityId"], dfd);
        if (PhysicalExamTemplateDetailRevamp.params.Title != null)
            $("#" + PhysicalExamTemplateDetailRevamp.params["PanelID"] + " #headingTitle").text(PhysicalExamTemplateDetailRevamp.params.Title);
    },

    loadEntitySpecialtyMK: function (entityID) {
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty_').empty();
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
                            PhysicalExamTemplateDetailRevamp.specialityCheckedIds = [];
                            var dataItem = this.dataItem(e.item.index());
                            PhysicalExamTemplateDetailRevamp.specialityCheckedIds.push(dataItem.id);
                            PhysicalExamTemplateDetailRevamp.loadSpecialityProviders(dataItem.id);
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
        if (PhysicalExamTemplateDetailRevamp.specialityCheckedIds.length > 0) {
            var objData = new Object();
            objData["IsActive"] = true;
            objData["SpecialtyIds"] = SpecialityId;

            PhysicalExamTemplateDetailRevamp.GetSpecialityProvider_DBCall(objData).done(function (response) {
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
    //                $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').empty();
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

    toggleControls: function () {
        $("#btnBold").click(function () {
            $('#observationContent div[id^=divSystem]').toggleClass("bold");
        });

        $("#btnItalic").click(function () {
            $('#observationContent div[id^=divSystem]').toggleClass("italic");
        });

        $("#btnUnderline").click(function () {
            $('#observationContent div[id^=divSystem]').toggleClass("underline");
        });

    },

    LoadPE: function () {
        $("#observationContent").text('');
        $("#SystemPreview").removeClass('hidden');
        PhysicalExamTemplateDetailRevamp.LoadPETempSysObservations(PhysicalExamTemplateDetailRevamp.params.PhysicalExamTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.PETemplateCount > 0) {
                    var res = JSON.parse(response.PETemplate_JSON);
                    var templateData = JSON.parse(utility.decodeHtml(res));
                    if (templateData.length > 0 && templateData[0] != null) {

                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamTemplateDetailRevamp #txtPhysicalExamTemplateName").val(templateData[0].TemplateName);
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamTemplateDetailRevamp #observationContent").text(templateData[0].TemplatePreview);
                        if (templateData[0].BodyPartId) {
                            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #divFavBodyPartPhysicalExamTemplate').removeClass('hidden')
                            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #ddlBodyParts').val(templateData[0].BodyPartId);
                        }
                        else {
                            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #divFavBodyPartPhysicalExamTemplate').addClass('hidden')
                            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #ddlBodyParts').val("");
                        }
                        var arrSpecialtyIds = templateData[0]['SpecialtyIds'].split(',');
                        utility.callbackAfterAllDOMLoaded(function () {
                            $.each(arrSpecialtyIds, function (i, itm) {
                                var OptionText = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamTemplateDetailRevamp #ddlPhysicalExamTemplateSpecialty option[value='" + itm + "']").text();
                                if (OptionText.toLowerCase() == "orth surg")
                                    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #divFavBodyPartPhysicalExamTemplate').removeClass('hidden');
                            });
                        });
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('clearSelection', false);
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('updateButtonText');
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #ddlPhysicalExamTemplateProvider").val(templateData[0]['ProviderIds'].split(','));
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect("refresh");
                        //Start 16-10-2017 Humaira Yousaf Bug# EMR-4972
                        if (templateData[0]['ProviderIds'] != "") {
                            PhysicalExamTemplateDetailRevamp.providerCheckedIds = templateData[0]['ProviderIds'].split(',');
                        }
                        //End 16-10-2017 Humaira Yousaf Bug# EMR-4972
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('clearSelection', false);
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('updateButtonText');
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #ddlPhysicalExamTemplateSpecialty").val(templateData[0]['SpecialtyIds'].split(','));
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect("refresh");
                        //Start 16-10-2017 Humaira Yousaf Bug# EMR-4972
                        if (templateData[0]['SpecialtyIds'] != "") {
                            PhysicalExamTemplateDetailRevamp.specialityCheckedIds = templateData[0]['SpecialtyIds'].split(',');
                        }
                    }
                    //End 16-10-2017 Humaira Yousaf Bug# EMR-4972
                }

                if (response.PETemplateSystemsCount > 0) {
                    $("#observationContent div").remove();
                    $("#ulPhysicalExamSystems li").remove();
                    var resTemplateSystems = JSON.parse(response.PETemplateSystems_JSON);
                    var SystemData = JSON.parse(resTemplateSystems);

                    for (var i = 0; i < SystemData.length; i++) {
                        if (SystemData[i].PESystemId != "") {
                            var li = PhysicalExamTemplateDetailRevamp.addSystem(SystemData[i].PESystemId, SystemData[i].SystemName);
                            $("#ulPhysicalExamSystems").append(li);

                            if (SystemData[i].IsSelectedSystem == "True") {
                                $("#ulPhysicalExamSystems #chk" + SystemData[i].PESystemId).prop("checked", true);
                            }

                            if (response.PESysObservationsCount > 0) {
                                var resTemplateObservations = JSON.parse(response.PESysObservations_JSON);
                                var ObservationData = JSON.parse(resTemplateObservations);

                                for (var j = 0; j < ObservationData.length; j++) {
                                    if (SystemData[i].PESystemId == ObservationData[j].PESystemId) {
                                        var li = PhysicalExamTemplateDetailRevamp.addObservations(ObservationData[j].PEObservationId, ObservationData[j].ObservationName, ObservationData[j].PESystemId);
                                        $("#ulPhysicalExamSystemSection").append(li);

                                        var objSelectedObservations = {
                                        };
                                        if (ObservationData[j].IsSelected == "True") {
                                            $("#ulPhysicalExamSystemSection #chk" + ObservationData[j].PEObservationId).prop("checked", true);
                                            if (SystemData[i].IsSelectedSystem == "True") {
                                                objSelectedObservations = {
                                                    PESystemId: ObservationData[j].PESystemId, IsChecked: true, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: true, IsSystemDeleted: 0, IsObservationDeleted: 0, ObservationOrder: ObservationData[j].ObservationOrder
                                                };
                                            }
                                            else {
                                                objSelectedObservations = {
                                                    PESystemId: ObservationData[j].PESystemId, IsChecked: true, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0, ObservationOrder: ObservationData[j].ObservationOrder
                                                };
                                            }

                                            //PhysicalExamTemplateDetailRevamp.handleDelimiter(ObservationData[j].PESystemId, ObservationData[j].PEObservationId, ObservationData[j].ObservationName, true);
                                        }
                                        else {
                                            $("#ulPhysicalExamSystemSection #chk" + ObservationData[j].PEObservationId).prop("checked", false);
                                            if (SystemData[i].IsSelectedSystem == "True") {
                                                objSelectedObservations = {
                                                    PESystemId: ObservationData[j].PESystemId, IsChecked: false, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: true, IsSystemDeleted: 0, IsObservationDeleted: 0, ObservationOrder: ObservationData[j].ObservationOrder
                                                };
                                            }
                                            else {
                                                objSelectedObservations = {
                                                    PESystemId: ObservationData[j].PESystemId, IsChecked: false, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0, ObservationOrder: ObservationData[j].ObservationOrder
                                                };
                                            }

                                            //PhysicalExamTemplateDetailRevamp.handleDelimiter(ObservationData[j].PESystemId, ObservationData[j].PEObservationId, ObservationData[j].ObservationName, false);
                                        }
                                        PhysicalExamTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
                                    }
                                }
                            }
                        }
                    }
                    if (SystemData)
                        $('#ulPhysicalExamSystems #' + SystemData[0].PESystemId).click();
                }
                //serialize Data after all controls loaded.
                $('#frmPhysicalExamTemplateDetailRevamp').data('serialize', $('#frmPhysicalExamTemplateDetailRevamp').serialize());
            }
        });
    },

    handleDelimiter: function (PESystemId, PEObservationId, ObservationName, IsChecked) {
        //$("#observationContent").text('');
        var desc = "";
        ObservationName = utility.decodeHtml(ObservationName);
        if (ObservationName.indexOf("'") > -1) {
            desc = ObservationName.replace(/\'/g, '@');
        } else {
            desc = ObservationName;
        }
        if (IsChecked) {
            var delimator = $("#delimator option:selected").text() + " ";
            if ($("#observationContent #divSystem" + PESystemId + PEObservationId).length > 0) {
                $('#observationContent #divSystem' + PESystemId + PEObservationId).remove();
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + PESystemId + PEObservationId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + PESystemId + PEObservationId).show();
                var txtToAppend = desc;

                if ($('#observationContent div').length > 1)
                    txtToAppend = delimator + desc;

                $("#observationContent #divSystem" + PESystemId + PEObservationId).append(txtToAppend.replace(/\@/g, "'"));
            }
            else {
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + PESystemId + PEObservationId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + PESystemId + PEObservationId).show();
                var txtToAppend = desc;

                if ($('#observationContent div').length > 1)
                    txtToAppend = delimator + desc;

                $("#observationContent #divSystem" + PESystemId + PEObservationId).append(txtToAppend.replace(/\@/g, "'"));
            }
        }
    },

    SortJSONArray: function (data, key, way) {
        return data.sort(function (a, b) {
            var x = a[key]; var y = b[key];
            if (way === '123') { return ((x < y) ? -1 : ((x > y) ? 1 : 0)); }
            if (way === '321') { return ((x > y) ? -1 : ((x < y) ? 1 : 0)); }
        });
    },

    LoadPETempSysObservations: function (templateId) {
        var objData = new Object();
        objData["TemplateId"] = templateId;
        objData["commandType"] = "Load_PhyscialExam_TemplatesFill_ECW";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    addSystem: function (PESystemId, SystemName) {
        var itemtoRemove = "system";

        for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
            if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId && PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted != 1) {
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
        var li = '<li id="' + PESystemId + '" parentid="null" onclick="PhysicalExamTemplateDetailRevamp.loadObservations(' + PESystemId + ')" value="' + PESystemId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                 '<input type="checkbox" id="chk' + PESystemId + '" name="' + PESystemId + '" class="pull-left  char" onclick="PhysicalExamTemplateDetailRevamp.ManageObservations(' + PESystemId + ', this);"><label id="lblName' + PESystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SystemName + '">' + SystemName + '</label><div id="divNameDetail' + PESystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PESystemId + '" onkeypress="" name="Name' + PESystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PESystemId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="PhysicalExamTemplateDetailRevamp.removeSystem(' + PESystemId + ', event)";><i class="fa fa-close"></i></span></a></li>';
        // PhysicalExamTemplateDetailRevamp.removeSystem(' + PESystemId + ')
        return li;
    },

    addObservations: function (PEObservationId, ObservatioName, PESystemId) {
        var a = PhysicalExamTemplateDetailRevamp.selectedObservations;
        ObservatioName = utility.decodeHtml(ObservatioName);
        var itemtoRemove = "observation";

        var desc = "";
        if (ObservatioName.indexOf("'") > -1) {
            desc = ObservatioName.replace(/\'/g, '@');
        } else {
            desc = ObservatioName;
        }


        var li = '<li id="' + PEObservationId + '" parentid="' + PESystemId + '" onclick="PhysicalExamTemplateDetailRevamp.PreviewObservations(' + PEObservationId + ',\'' + desc + '\', this, ' + PESystemId + ');" value="' + PEObservationId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                 '<input type="checkbox" id="chk' + PEObservationId + '" name="' + PEObservationId + '" class="pull-left  char" ' +
                 'onclick="PhysicalExamTemplateDetailRevamp.PreviewObservations(' + PEObservationId + ',\'' + desc + '\', this, ' + PESystemId + ');"><label id="lblName' + PEObservationId + '" class="" data-toggle="tooltip" title="" data-original-title="' + ObservatioName + '">' + ObservatioName + '</label><div id="divNameDetail' + PEObservationId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PEObservationId + '" onkeypress="" name="Name' + PEObservationId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PEObservationId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="PhysicalExamTemplateDetailRevamp.removeObservation(' + PESystemId + ', ' + PEObservationId + ')"><i class="fa fa-close"></i></span></a></li>';

        return li;
    },

    removeItem: function (PESystemId, control, PEObservationId) {
        if (control == "system") {
            $("#ulPhysicalExamSystems #" + PESystemId).remove();

            if (PhysicalExamTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId) {
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 1;
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                    }
                }
            }

        }
        else if (control == "observation") {
            $("#ulPhysicalExamSystemSection #" + PEObservationId).remove();

            if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId && PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId == PEObservationId) {
                PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 1;
                PhysicalExamTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationOrder = -1;
            }
        }
    },

    removeSystem: function (PESystemId, event) {
        $("#ulPhysicalExamSystems #" + PESystemId).remove();
        $("#ulPhysicalExamSystemSection").hide();
        if (PhysicalExamTemplateDetailRevamp.selectedObservations) {
            for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId) {
                    PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 1;
                    PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 1;
                    PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                    PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = false;
                    PhysicalExamTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                    PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationOrder = -1;
                }
            }
        }

        $("#observationContent div").remove();
        event.stopPropagation();
    },

    removeObservation: function (PESystemId, PEObservationId) {
        $("#ulPhysicalExamSystemSection #" + PEObservationId).remove();
        $("#observationContent #divSystem" + PESystemId + PEObservationId).remove();
        PhysicalExamTemplateDetailRevamp.removeLastDelimiter(PESystemId);

        if (PhysicalExamTemplateDetailRevamp.selectedObservations) {
            for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId && PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId == PEObservationId) {
                    PhysicalExamTemplateDetailRevamp.deleteObservations.push(PhysicalExamTemplateDetailRevamp.selectedObservations[i]);
                    PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 1;
                    PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                    PhysicalExamTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                    PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationOrder = -1;
                }
            }
        }
    },

    loadObservations: function (PESystemId) {
        $("#ulPhysicalExamSystemSection").show();
        var isExist = false;
        if (PhysicalExamTemplateDetailRevamp.deleteObservations) {
            for (var d = 0 ; d < PhysicalExamTemplateDetailRevamp.deleteObservations.length; d++) {
                for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {

                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i] && PhysicalExamTemplateDetailRevamp.deleteObservations[d] && (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PhysicalExamTemplateDetailRevamp.deleteObservations[d].PESystemId && PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId == PhysicalExamTemplateDetailRevamp.deleteObservations[d].ObservationId)) {
                        PhysicalExamTemplateDetailRevamp.selectedObservations.splice([i], 1);
                    }
                }
            }
        }

        if (PhysicalExamTemplateDetailRevamp.selectedObservations) {
            for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId) {
                    isExist = true;
                    break;
                }
            }
        }

        if ($("#ulPhysicalExamSystemSection li").length > 1) {
            isExist = true;
        }

        if (!isExist) {
            PhysicalExamTemplateDetailRevamp.PhysicalExamSystemObservationsLoad(PESystemId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    PhysicalExamTemplateDetailRevamp.deleteObservations = [];
                    if (response.PEObservationCount > 0) {
                        var res = JSON.parse(response.PEObservation_JSON);
                        var resSystems = JSON.parse(res);
                        $("#SystemSections").removeClass('hidden');
                        $("#observationContent div").hide();
                        //$('#observationContent div[id^=divSystem]').hide();
                        $("#ulPhysicalExamSystemSection li").remove();

                        $("#ulPhysicalExamSystems li").removeClass('active');
                        $("#ulPhysicalExamSystems li[id=" + PESystemId + "]").addClass('active');

                        var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + PESystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                            + 'onclick="PhysicalExamTemplateDetailRevamp.selectAllChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        $("#ulPhysicalExamSystemSection").append(selectAll);

                        $.each(resSystems, function (i, item) {
                            var li = PhysicalExamTemplateDetailRevamp.addObservations(item.PEObservationId, item.Name, PESystemId);
                            $("#ulPhysicalExamSystemSection").append(li);


                            var objSelectedObservations = {
                                PESystemId: PESystemId, IsChecked: false, ObservationId: item.PEObservationId, ObservationName: item.Name, IsSystemChecked: false, SystemDescription: "", IsSystemDeleted: 0, IsObservationDeleted: 0, ObservationOrder: -1
                            };
                            PhysicalExamTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);

                        });
                    }
                    else {
                        // $('#observationContent div[id^=divSystem]').hide();
                        $("#ulPhysicalExamSystemSection li").remove();

                        $("#ulPhysicalExamSystems li").removeClass('active');
                        $("#ulPhysicalExamSystems li[id=" + PESystemId + "]").addClass('active');
                    }
                }
            });
        }
        else {
            $("#SystemSections").removeClass('hidden');
            $("#ulPhysicalExamSystemSection li").remove();

            var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + PESystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                        + 'onclick="PhysicalExamTemplateDetailRevamp.selectAllChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
            $("#ulPhysicalExamSystemSection").append(selectAll);
            if (PhysicalExamTemplateDetailRevamp.deleteObservations) {
                for (var d = 0 ; d < PhysicalExamTemplateDetailRevamp.deleteObservations.length; d++) {
                    for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {

                        if (PhysicalExamTemplateDetailRevamp.selectedObservations[i] && PhysicalExamTemplateDetailRevamp.deleteObservations[d] && (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PhysicalExamTemplateDetailRevamp.deleteObservations[d].PESystemId && PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId == PhysicalExamTemplateDetailRevamp.deleteObservations[d].ObservationId)) {
                            PhysicalExamTemplateDetailRevamp.selectedObservations.splice([i], 1);
                        }
                    }
                }
            }
            if (PhysicalExamTemplateDetailRevamp.selectedObservations) {
                //$('#observationContent div[id^=divSystem]').hide();
                $("#observationContent").text('');
                var observationsListOrdered;
                for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId) {
                        var li = PhysicalExamTemplateDetailRevamp.addObservations(PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId, PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationName, PESystemId);

                        if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId != "")
                            $("#ulPhysicalExamSystemSection").append(li);

                        $("#ulPhysicalExamSystems li").removeClass('active');
                        $("#ulPhysicalExamSystems li[id=" + PESystemId + "]").addClass('active');

                        var name = [PESystemId];
                        var res = $.grep(PhysicalExamTemplateDetailRevamp.selectedObservations, function (v) {
                            return name.indexOf(parseInt(v.PESystemId)) > -1 && v.IsChecked;
                        });

                        observationsListOrdered = PhysicalExamTemplateDetailRevamp.SortJSONArray(res, "ObservationOrder", "123");

                        if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked) {
                            if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted == 0) {
                                $("#ulPhysicalExamSystemSection #chk" + PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId).prop("checked", true);
                                $('#observationContent #divSystem' + PESystemId + PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId).show();
                                PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;
                                PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 0;
                                //PhysicalExamTemplateDetailRevamp.handleDelimiter(PESystemId, PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId, PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationName, true);
                            }
                            else {
                                $("#ulPhysicalExamSystemSection #chk" + PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId).prop("checked", false);
                                PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;
                                PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 0;
                                PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                            }

                            var noOfFindings = $.grep(PhysicalExamTemplateDetailRevamp.selectedObservations, function (item, index) {
                                return item.PESystemId == PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId;
                            });
                            if (noOfFindings.length == $("#ulPhysicalExamSystemSection li").find('input:checked').length) {
                                $("#chkboxSelectAllObservations").find('input:checkbox').prop('checked', true);
                            }
                            else {
                                $("#chkboxSelectAllObservations").find('input:checkbox').prop('checked', false);
                            }
                        }
                        else {
                            $("#ulPhysicalExamSystemSection #chk" + PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId).prop("checked", false);
                            PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsObservationDeleted = 0;
                            PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemDeleted = 0;
                            PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        }
                    }
                }
                if (observationsListOrdered) {
                    for (var j = 0; j < observationsListOrdered.length; j++) {
                        PhysicalExamTemplateDetailRevamp.handleDelimiter(PESystemId, observationsListOrdered[j].ObservationId, observationsListOrdered[j].ObservationName, true);
                    }
                }

            }
        }
    },

    ManageObservations: function (PESystemId, obj) {
        $("#ulPhysicalExamSystems li").removeClass('active');
        $("#ulPhysicalExamSystems li[id=" + PESystemId + "]").addClass('active');

        if (!$(obj).is(':checked')) {
            $(obj).parent().parent().removeClass('active');

            for (var i = 0; i < $("#ulPhysicalExamSystemSection li").length; i++) {
                $("#ulPhysicalExamSystemSection #chk" + $("#ulPhysicalExamSystemSection li")[i].id).prop('checked', false);
            }
            if (PhysicalExamTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId) {
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = false;
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationOrder = -1;
                        //break;
                    }
                }
            }
        }
        else {
            if (PhysicalExamTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId) {
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                        break;
                    }
                }
            }
        }
    },

    selectAllChars: function (obj) {

        if (obj) var sysId = $($($(obj).parent().parent())[0]).attr('parentid');

        if ($(obj).prop('checked') == true) {
            $("#SystemPreview").removeClass('hidden');
            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').each(function () {
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

                        $("#observationContent #divSystem" + sysId + id_).append(txttoAppend.replace(/\@/g, "'"));
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

                        $("#observationContent #divSystem" + sysId + id_).append(txttoAppend.replace(/\@/g, "'"));
                    }

                    $("#ulPhysicalExamSystems #chk" + sysId).prop("checked", true);
                }
            });

            if (PhysicalExamTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == sysId) {
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked = true;
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + sysId).text();
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationOrder = i;
                    }
                }
            }
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').removeClass("green");
            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                    var system_id = $($($(obj).parent().parent())[0]).attr('parentid');
                    $("#observationContent #divSystem" + system_id + id_).remove();
                }
            });

            if (PhysicalExamTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == sysId) {
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + sysId).text();
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationOrder = -1;
                        //$('#observationContent div[id^=divSystem' + sysId + ']').hide();
                    }
                }
            }
        }
        PhysicalExamTemplateDetailRevamp.removeLastDelimiter(sysId);
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

    PreviewObservations: function (observationId, ObservationName, obj, PESystemId) {

        $("#SystemPreview").removeClass('hidden');
        ObservationName = ObservationName.replace(/\@/g, "'");
        if ($(obj).prop('checked') == true) {
            $(this).find("[type='checkbox'][id*='chk']").prop("checked", true);
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').removeClass("green");
            $(this).find("[type='checkbox'][id*='chk']").prop("checked", false);
            $("#chkboxSelectAllObservations").find('input:checkbox').prop('checked', false);
        }

        var isChk = $(obj).prop('checked') == true ? true : false;

        var objSelectedObservations =
        {
            PESystemId: PESystemId,
            IsChecked: isChk,
            ObservationId: observationId,
            ObservationName: ObservationName,
            IsModified: '1',
            IsSystemChecked: false,
            SystemDescription: "",
            ObservationOrder: -1

        };


        if ($(obj).prop('checked') == true) {
            var deli = $("#delimator option:selected").text() + " ";

            //$('#observationContent div[id^=divSystem]').remove();
            if ($("#observationContent #divSystem" + PESystemId + observationId).length > 0) {
                $('#observationContent #divSystem' + PESystemId + observationId).remove();
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + PESystemId + observationId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + PESystemId + observationId).show();
                var txttoAppend = ObservationName.replace(/\@/g, "'");
                if ($('#observationContent div').length > 1)
                    txttoAppend = deli + ObservationName.replace(/\@/g, "'");

                $("#observationContent #divSystem" + PESystemId + observationId).append(txttoAppend.replace(/\@/g, "'"));
            }
            else {
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + PESystemId + observationId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + PESystemId + observationId).show();
                var txttoAppend = ObservationName.replace(/\@/g, "'");

                if ($('#observationContent div').length > 1)
                    txttoAppend = deli + ObservationName.replace(/\@/g, "'");

                $("#observationContent #divSystem" + PESystemId + observationId).append(txttoAppend.replace(/\@/g, "'"));
            }


            var mk = 0, observationsListOrdered;
            for (var i = 0; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId) {
                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId && PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId == observationId) {
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked = true;
                    }
                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked) {
                        var name = [PESystemId];
                        var res = $.grep(PhysicalExamTemplateDetailRevamp.selectedObservations, function (v) {
                            return name.indexOf(parseInt(v.PESystemId)) > -1 && v.IsChecked;
                        });

                        observationsListOrdered = PhysicalExamTemplateDetailRevamp.SortJSONArray(res, "ObservationOrder", "321");

                        mk = parseInt(observationsListOrdered[0].ObservationOrder) + 1;
                    }
                }
            }

            if (PhysicalExamTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId && PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId == observationId) {
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked = true;
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationOrder = mk;
                    }
                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId) {
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsSystemChecked = true;
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                    }
                }
            }

            $("#ulPhysicalExamSystems #chk" + PESystemId).prop("checked", true);
            PhysicalExamTemplateDetailRevamp.loadObservations(PESystemId);
        }
        else if ($(obj).prop('checked') == false) {
            if (PhysicalExamTemplateDetailRevamp.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamTemplateDetailRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplateDetailRevamp.selectedObservations[i].PESystemId == PESystemId && PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationId == observationId) {
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].IsChecked = false;
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                        PhysicalExamTemplateDetailRevamp.selectedObservations[i].ObservationOrder = -1;
                    }
                }
            }
            var aa = $('#observationContent #divSystem' + PESystemId + observationId).text();
            $('#observationContent #divSystem' + PESystemId + observationId).remove();
            PhysicalExamTemplateDetailRevamp.removeLastDelimiter(PESystemId);
        }
        PhysicalExamTemplateDetailRevamp.removeLastDelimiter(PESystemId);
    },

    savePhysicalExam: function () {
        PhysicalExamTemplateDetailRevamp.physicalExamTemplateSave();
    },

    removeLastDelimiter: function (PESystemId) {

        var delii = $("#delimator option:selected").text();
        var str = "";
        if (delii == ",") str = $($('#observationContent div[id^=divSystem' + PESystemId + ']')[0]).text().replace(/,/g, "");
        if (delii == ".") str = $($('#observationContent div[id^=divSystem' + PESystemId + ']')[0]).text().replace(/./g, "");
        if (delii == ":") str = $($('#observationContent div[id^=divSystem' + PESystemId + ']')[0]).text().replace(/:/g, "");
        if (delii == ";") str = $($('#observationContent div[id^=divSystem' + PESystemId + ']')[0]).text().replace(/;/g, "");
        if (delii == "-") str = $($('#observationContent div[id^=divSystem' + PESystemId + ']')[0]).text().replace(/-/g, "");

        var id = $($('#observationContent div')[0]).attr('id');
        $("#observationContent #" + id).text(str);
    },

    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + PhysicalExamTemplateDetailRevamp.params["PanelID"];
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
                    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (PhysicalExamTemplateDetailRevamp.SpecialtyIds != '') {

                        var Specialties = PhysicalExamTemplateDetailRevamp.SpecialtyIds.split(",");
                        PhysicalExamTemplateDetailRevamp.specialityCheckedIds = Specialties;
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').val(Specialties);
                    }
                    //serialize Data after all controls loaded.
                    $('#frmPhysicalExamTemplateDetailRevamp').data('serialize', $('#frmPhysicalExamTemplateDetailRevamp').serialize());
                }

            }).then(function () {
                PhysicalExamTemplateDetailRevamp.IntializeMultiSelectDropDownSpecialties();
                //enable dropdownlist
                PhysicalExamTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty', false);
                PhysicalExamTemplateDetailRevamp.loadEntityProvider(globalAppdata["SeletedEntityId"], dfd);


            });
        }
        else {
            //Disable dropdownlist
            PhysicalExamTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty', true);
        }
    },

    loadEntityProvider: function (entityId, dfd) {

        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider');
                var $providerHiddenDdl = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlHiddenPhysicalExamTemplateProvider');

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
                if (PhysicalExamTemplateDetailRevamp.ProviderIds != '') {
                    var Providers = PhysicalExamTemplateDetailRevamp.ProviderIds.split(",");
                    PhysicalExamTemplateDetailRevamp.providerCheckedIds = Providers;
                    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').val(Providers);
                }
                //serialize Data after all controls loaded.
                $('#frmPhysicalExamTemplateDetailRevamp').data('serialize', $('#frmPhysicalExamTemplateDetailRevamp').serialize());
            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamTemplateSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                PhysicalExamTemplateDetailRevamp.IntializeMultiSelectDropDownProviders();
                //serialize Data after all controls loaded.
                $('#frmPhysicalExamTemplateDetailRevamp').data('serialize', $('#frmPhysicalExamTemplateDetailRevamp').serialize());
                if (dfd)
                    dfd.resolve();
            });
            //enable multiselect
            //PhysicalExamTemplateDetailRevamp.enableDisableDropDowLists('ddlPhysicalExamTemplateProvider', false);
        }
        else {
            //disable multiselect
            PhysicalExamTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateProvider', true);
        }
    },

    buildAlreadyAssosiatedSystems: function (templateId) {
        PhysicalExamTemplateDetailRevamp.PhysicalExamTemplateDetailRevampLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var res = JSON.parse(response.PETemplateSystems_JSON);
                var resSystems = JSON.parse(res);
                var data = [];

                $.each(resSystems, function (i, item) {
                    data.push({ id: item.PESystemId, text: item.SystemName, expanded: true, spriteCssClass: "rootfolder" });
                    var li = PhysicalExamTemplateDetailRevamp.addSystem(item.PESystemId, item.SystemName); //'<li id="' + item.PESystemId + '" parentid="null" onclick="" value="' + item.PESystemId + '" refvalue="" subcharacteristicexist=" " class="active"><div class="checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chk' + item.PESystemId + '" name="' + item.PESystemId + '" class="pull-left  char" onclick=""><label id="lblName' + item.PESystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + item.SystemName + '">' + item.SystemName + '</label><div id="divNameDetail' + item.PESystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" id="txtName' + item.PESystemId + '" onkeypress="" name="Name' + item.PESystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.PESystemId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                    //var li = '<li id="' + item.PESystemId + '" parentid="null" onclick="PhysicalExamTemplateDetailRevamp.showHideChildControls(this,"ulPhysicalExamSystems",' + item.PESystemId + ');" value="' + item.PESystemId + '" refvalue="" subcharacteristicexist=" " class="active"><div class="checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chk' + item.PESystemId + '" name="' + item.PESystemId + '" class="pull-left  char" onclick="PhysicalExamTemplateDetailRevamp.selectParentControls(this);PhysicalExamTemplateDetailRevamp.toggleCheckBoxes(this);"><label id="lblName' + item.PESystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + item.SystemName + '">' + item.SystemName + '</label><div id="divNameDetail' + item.PESystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" id="txtName' + item.PESystemId + '" onkeypress="PhysicalExamTemplateDetailRevamp.saveDetailComments(event,this)" name="Name' + item.PESystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.PESystemId + '" onclick="PhysicalExamTemplateDetailRevamp.saveTemplateSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                    $("#ulPhysicalExamSystems").append(li);
                    PhysicalExamTemplateDetailRevamp.loadObservations(item.PESystemId);
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
        PhysicalExamTemplateDetailRevamp.PhysicalExamSystemsLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var res = JSON.parse(response.PESystems_JSON);
                var resSystems = JSON.parse(res);
                var data = [];
                $.each(resSystems, function (i, item) {
                    data.push({ id: item.PESystemId, value: item.Name });
                });

                $("#Systems").kendoAutoComplete({
                    dataSource: data,
                    filter: "contains",
                    dataTextField: "value",
                    placeholder: "Select System...",
                    select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        //alert(kendo.stringify(dataItem));
                        var li = PhysicalExamTemplateDetailRevamp.addSystem(dataItem.id, dataItem.value);
                        if (li != undefined)
                            $("#ulPhysicalExamSystems").append(li);
                        $("#Systems").val('');
                        PhysicalExamTemplateDetailRevamp.loadObservations(dataItem.id);
                        e.preventDefault();
                    },
                    footerTemplate: 'Total <strong>#: instance.dataSource.total() #</strong> items found'
                });

                $("#Systems").parent().addClass('size100per');
                //serialize Data after all controls loaded.
                $('#frmPhysicalExamTemplateDetailRevamp').data('serialize', $('#frmPhysicalExamTemplateDetailRevamp').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadPhysicalExamTemplateDetailRevamp: function (templateId) {
        PhysicalExamTemplateDetailRevamp.PhysicalExamTemplateDetailRevampLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                response.PhysicalExamTemplate = JSON.parse(response.PhysicalExamTemplate);

                if (response.PhysicalExamTemplate.length > 0) {
                    response.PhysicalExamTemplate = response.PhysicalExamTemplate[0];
                    PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData = response.PhysicalExamTemplate.SysSecCharSubcharData;
                    //Start Farooq Ahmad 03-03-2016 mark green and checked the loaded System
                    for (var index in PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData) {
                        //if ($.parseJSON(PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData[index].IsChecked.toString().toLowerCase()))
                        //    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId).addClass("green");
                        //else
                        //    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId).removeClass("green");

                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId + " input:checkbox").prop("checked", $.parseJSON(PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData[index].IsChecked.toString().toLowerCase()));
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId + " label").attr("data-original-title", PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData[index].SystemName);
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamTemplateDetailRevamp #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData[index].SystemId + " label").text(PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData[index].SystemName);
                    }
                    //End Farooq Ahmad 03-03-2016 mark green and checked the loaded System
                    var self = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamTemplateDetailRevamp");

                    utility.bindMyJSONByName(true, response.PhysicalExamTemplate, false, self).done(function () {
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateEntity').change();
                        PhysicalExamTemplateDetailRevamp.SpecialtyIds = response.PhysicalExamTemplate.SpecialtyIds;
                        PhysicalExamTemplateDetailRevamp.ProviderIds = response.PhysicalExamTemplate.ProviderIds;
                    });
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    PhysicalExamSystemsLoad: function (templateId) {
        var objData = new Object();
        objData["TemplateId"] = templateId;
        objData["commandType"] = "Load_PhyscialExam_Systems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    PhysicalExamSystemObservationsLoad: function (PESystemId) {
        var objData = new Object();
        objData["SystemId"] = PESystemId;
        objData["commandType"] = "Load_PhyscialExam_System_Observations";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    PhysicalExamTemplateDetailRevampLoad: function (templateId) {
        var objData = new Object();
        objData["TemplateId"] = templateId == undefined ? "0" : templateId;
        objData["commandType"] = "Load_PhyscialExam_Templates_ECW";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    PhysicalExamTemplateDetailRevampLoadOnDemand: function (templateId, systemId, sectionId, charId, commandType) {
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
        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('destroy');
        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {

                PhysicalExamTemplateDetailRevamp.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                $.when(
                    PhysicalExamTemplateDetailRevamp.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (PhysicalExamTemplateDetailRevamp.ProviderIds != '') {
                        var Providers = PhysicalExamTemplateDetailRevamp.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                PhysicalExamTemplateDetailRevamp.providerCheckedIds = PhysicalExamTemplateDetailRevamp.removeFromArray(PhysicalExamTemplateDetailRevamp.providerCheckedIds, item);
                                PhysicalExamTemplateDetailRevamp.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').val(PhysicalExamTemplateDetailRevamp.providerCheckedIds);
                    PhysicalExamTemplateDetailRevamp.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (PhysicalExamTemplateDetailRevamp.SpecialtyIds != '') {
                    var spacialties = PhysicalExamTemplateDetailRevamp.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            PhysicalExamTemplateDetailRevamp.specialityCheckedIds = PhysicalExamTemplateDetailRevamp.removeFromArray(PhysicalExamTemplateDetailRevamp.specialityCheckedIds, item);
                            PhysicalExamTemplateDetailRevamp.specialityCheckedIds.push(item);
                        });
                    }
                }
                PhysicalExamTemplateDetailRevamp.setSpacialtiesByselectedProviderIds();
                $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('clearSelection', false);
                $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('updateButtonText');
                $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('select', PhysicalExamTemplateDetailRevamp.specialityCheckedIds);
                $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect("refresh");

            },
        });
    },

    setSpacialtiesByselectedProviderIds: function () {

        $.each(PhysicalExamTemplateDetailRevamp.providerCheckedIds, function (index, item) {

            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {

                        PhysicalExamTemplateDetailRevamp.specialityCheckedIds = PhysicalExamTemplateDetailRevamp.removeFromArray(PhysicalExamTemplateDetailRevamp.specialityCheckedIds, $(this).attr('refname'));
                        PhysicalExamTemplateDetailRevamp.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('destroy');
        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                PhysicalExamTemplateDetailRevamp.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
                // PhysicalExamTemplateDetailRevamp.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('refresh');
            },


        });
    },

    domReady: function () {

        $(document).ready(function () {

            PhysicalExamTemplateDetailRevamp.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty,ddlPhysicalExamTemplateProvider', true);

            //callback function on change event of entity ddl
            PhysicalExamTemplateDetailRevamp.entityChanged();

            //Initialize when the document is ready for the first time (just for the good look).
            PhysicalExamTemplateDetailRevamp.IntializeMultiSelectDropDownSpecialties();
            PhysicalExamTemplateDetailRevamp.IntializeMultiSelectDropDownProviders();


            $(document).click(function (event) {



                var $Item = $(PhysicalExamTemplateDetailRevamp.selectedListItem);
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

        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateEntity').change(function () {
            //Get the selected entity
            selectedEntity = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateEntity :selected').val();

            $.when(PhysicalExamTemplateDetailRevamp.loadEntitySpecialty(selectedEntity)).then(function () {

                $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('destroy').multiselect();
            });
            $.when(PhysicalExamTemplateDetailRevamp.loadEntityProvider(selectedEntity)).then(function () {

                $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('destroy').multiselect();

            });
        });
    },

    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlHiddenPhysicalExamTemplateProvider';

        var providerContext = '#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider';
        $(providerContext).empty();

        if (PhysicalExamTemplateDetailRevamp.specialityCheckedIds.length > 0) {

            $.each(PhysicalExamTemplateDetailRevamp.specialityCheckedIds, function (index, specialtyId) {

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
        var specialtyContext = '#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamTemplateSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            PhysicalExamTemplateDetailRevamp.specialityCheckedIds = [];
            PhysicalExamTemplateDetailRevamp.providerCheckedIds = [];
            PhysicalExamTemplateDetailRevamp.ProviderIds = '';
            PhysicalExamTemplateDetailRevamp.SpecialtyIds = '';
            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divFavBodyPartPhysicalExamTemplate').addClass('hidden');
            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #ddlBodyParts').val("");
            PhysicalExamTemplateDetailRevamp.orthopedicSpecialitySelected = false;
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {
                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    PhysicalExamTemplateDetailRevamp.specialityCheckedIds = PhysicalExamTemplateDetailRevamp.removeFromArray(PhysicalExamTemplateDetailRevamp.specialityCheckedIds, spacialityId);
                    PhysicalExamTemplateDetailRevamp.specialityCheckedIds.push(spacialityId);
                    if ($(option).text() == "ORTH SURG") {
                        PhysicalExamTemplateDetailRevamp.orthopedicSpecialitySelected = true;
                        if ($.inArray(spacialityId, PhysicalExamTemplateDetailRevamp.specialityCheckedIds) > -1)
                            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divFavBodyPartPhysicalExamTemplate').removeClass('hidden');
                        else {
                            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #ddlBodyParts').val("");
                            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divFavBodyPartPhysicalExamTemplate').addClass('hidden');
                        }
                    }
                    else if (PhysicalExamTemplateDetailRevamp.orthopedicSpecialitySelected == false) {
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #ddlBodyParts').val("");
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divFavBodyPartPhysicalExamTemplate').addClass('hidden');
                    }
                }
                else {
                    PhysicalExamTemplateDetailRevamp.specialityCheckedIds = PhysicalExamTemplateDetailRevamp.removeFromArray(PhysicalExamTemplateDetailRevamp.specialityCheckedIds, spacialityId);
                    if ($(option).text() == "ORTH SURG") {
                        if ($.inArray(spacialityId, PhysicalExamTemplateDetailRevamp.specialityCheckedIds) > -1)
                            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divFavBodyPartPhysicalExamTemplate').removeClass('hidden');
                        else {
                            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #ddlBodyParts').val("");
                            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divFavBodyPartPhysicalExamTemplate').addClass('hidden');
                        }
                    }
                    else if (PhysicalExamTemplateDetailRevamp.orthopedicSpecialitySelected == false) {
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #ddlBodyParts').val("");
                        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divFavBodyPartPhysicalExamTemplate').addClass('hidden');
                    }
                }
            }
            else {
                PhysicalExamTemplateDetailRevamp.orthopedicSpecialitySelected = false;
                PhysicalExamTemplateDetailRevamp.specialityCheckedIds = [];
                $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    if ($(this).length > 0 && $(this).text() == "ORTH SURG" && $(this)[0].selected)
                        PhysicalExamTemplateDetailRevamp.orthopedicSpecialitySelected = true;
                    PhysicalExamTemplateDetailRevamp.specialityCheckedIds.push(spacialityId);
                });
                if (PhysicalExamTemplateDetailRevamp.orthopedicSpecialitySelected)
                    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divFavBodyPartPhysicalExamTemplate').removeClass('hidden');
                else {
                    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #ddlBodyParts').val("");
                    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divFavBodyPartPhysicalExamTemplate').addClass('hidden');
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
        var providerContext = '#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #divPhysicalExamTemplateProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        var allProviders = $(providerContext).find('.dropdown-menu').find('li:not(".filter,.multiselect-all")').length;
        var selectedProviders = $(providerContext).find('.dropdown-menu').find('li.active:not(".filter,.multiselect-all")').length;

        if (checkedProviderItems <= 0) {
            PhysicalExamTemplateDetailRevamp.providerCheckedIds = [];
            PhysicalExamTemplateDetailRevamp.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected && allProviders == selectedProviders) {
            PhysicalExamTemplateDetailRevamp.providerCheckedIds = [];
            $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #ddlPhysicalExamTemplateProvider option').each(function () {
                var providerValue = $(this).val();
                PhysicalExamTemplateDetailRevamp.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                PhysicalExamTemplateDetailRevamp.providerCheckedIds = PhysicalExamTemplateDetailRevamp.removeFromArray(PhysicalExamTemplateDetailRevamp.providerCheckedIds, providerValue);
                PhysicalExamTemplateDetailRevamp.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                PhysicalExamTemplateDetailRevamp.providerCheckedIds = PhysicalExamTemplateDetailRevamp.removeFromArray(PhysicalExamTemplateDetailRevamp.providerCheckedIds, $(option).val());
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
            currentId = PhysicalExamTemplateDetailRevamp.NewInsertedId--;
            var subcharacteristicExist = "";

            var saveMethodPE = "";

            if (itemType.toLowerCase() == "system") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamTemplateDetailRevamp #" + currentCtrlId);
                saveMethodPE = "PhysicalExamTemplateDetailRevamp.AddSystemPE(this, '" + currentId + "');";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystemSection";
                ulControl = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #frmPhysicalExamTemplateDetailRevamp #" + currentCtrlId);
                saveMethodPE = "PhysicalExamTemplateDetailRevamp.AddObservation(this, '" + currentId + "');";
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
                onClick = "";//"PhysicalExamTemplateDetailRevamp.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";
                var deleteFunction = "PhysicalExamTemplateDetailRevamp.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                liInnerText = '<div class="checkbox-custom checkboxTiny checkbox-success">' + deleteIcon + addSubCharIcon
                    + '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="">'
                    //+ '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="PhysicalExamTemplateDetailRevamp.selectParentControls(this);PhysicalExamTemplateDetailRevamp.toggleCheckBoxes(this);">'
                    + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                    + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                    + '<textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress="PhysicalExamTemplateDetailRevamp.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
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

    AddSystemPE: function (obj, controlId) {

        var objData = new Object();
        objData["IsGlobal"] = false;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = true;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        PhysicalExamTemplateDetailRevamp.savePESystem_DBCall(objData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                utility.DisplayMessages(response.Message, 1);

                var li = PhysicalExamTemplateDetailRevamp.addSystem(response.PESystemId, objData["Name"]);
                $("#ulPhysicalExamSystems").append(li);

                //var objSelectedObservations = {
                //    PESystemId: response.PESystemId, IsChecked: false, ObservationId: '', ObservationName: '', IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0
                //};
                //PhysicalExamTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
                PhysicalExamTemplateDetailRevamp.loadObservations(response.PESystemId);
                $("#" + controlId).remove();
            }
            else {

                utility.DisplayMessages(response.Message, 1);
            }
        });
    },

    AddObservation: function (obj, controlId) {
        var PESystemId = $("#ulPhysicalExamSystems li.active")[0].id;
        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = true;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        PhysicalExamTemplateDetailRevamp.savePEObservation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                // PRD-5 ,Dev By:MAhmad 
                if (response.status != false) {
                    var res = JSON.parse(response.PEObservation_JSON);
                    utility.DisplayMessages(response.Message, 1);
                    PhysicalExamTemplateDetailRevamp.params.PEObservationId = res[0].PEObservationId;
                    var li = PhysicalExamTemplateDetailRevamp.addObservations(res[0].PEObservationId, objData["Name"], PESystemId);
                    $("#ulPhysicalExamSystemSection").append(li);
                    var sysChecked = $("#chk" + $("#ulPhysicalExamSystems li.active").attr('id')).is(':checked');
                    var objSelectedObservations = {
                        PESystemId: PESystemId, IsChecked: false, ObservationId: res[0].PEObservationId, ObservationName: objData["Name"], IsSystemChecked: sysChecked, IsSystemDeleted: 0, IsObservationDeleted: 0, ObservationOrder: -1
                    };
                    PhysicalExamTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
                    $("#" + controlId).remove();
                }
                else {
                    if (response.Message == "Observation with same name already exists.") {
                        utility.DisplayMessages("An observation with same name already exists.", 3);
                    }
                }
                // PRD-5 ,Dev By:MAhmad 
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
            onClickFunction = onClickFunction.replace('this', "$('#" + PhysicalExamTemplateDetailRevamp.params.PanelID + " #" + ULID + " #" + ID + "')");
            eval(onClickFunction);
        }
    },

    savePESystem_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "insert_physicalexam_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamSystem");
    },

    savePEObservation_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "insert_physicalexam_observation_";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
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
        $("#PhysicalExamTemplateDetailRevamp #pnlLicenseDetail").removeClass('disableAll');
        if (PhysicalExamTemplateDetailRevamp.params.mode == "Add") {
            $('#PhysicalExamTemplateDetailRevamp #txtShortName').attr("enabled", "enabled");

            $("#PhysicalExamTemplateDetailRevamp #pnlLicenseDetail").addClass('disableAll');
            PhysicalExamTemplateDetailRevamp.ValidateProvider();

            //serialize Data after all controls loaded.
            $('#frmPhysicalExamTemplateDetailRevamp').data('serialize', $('#frmPhysicalExamTemplateDetailRevamp').serialize());

        }
        else if (PhysicalExamTemplateDetailRevamp.params.mode == "Edit") {
            $('#PhysicalExamTemplateDetailRevamp #txtShortName').attr("disabled", "disabled");
            PhysicalExamTemplateDetailRevamp.LoadProviderLicense().done(function (response) {
                if (response.status != false) {

                    PhysicalExamTemplateDetailRevamp.ProviderLicenseGridLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            PhysicalExamTemplateDetailRevamp.FillProvider(PhysicalExamTemplateDetailRevamp.params.ProviderId).done(function (response) {
                if (response.status != false) {
                    var provider_detail = JSON.parse(response.ProviderFill_JSON);
                    var self = $("#PhysicalExamTemplateDetailRevamp");
                    utility.bindMyJSON(true, provider_detail, false, self).done(function () {

                        if (provider_detail.chkActive == 'True')
                            $("#PhysicalExamTemplateDetailRevamp #chkActive").attr("checked", true);
                        else
                            $("#PhysicalExamTemplateDetailRevamp #chkActive").attr("checked", false);

                        if (provider_detail.chkSpecialist == 'True')
                            $("#PhysicalExamTemplateDetailRevamp #chkSpecialist").attr("checked", true);
                        else
                            $("#PhysicalExamTemplateDetailRevamp #chkSpecialist").attr("checked", false);

                        $("#PhysicalExamTemplateDetailRevamp #pnlLicenseDetail").removeClass('disableAll');

                        PhysicalExamTemplateDetailRevamp.ValidateProvider();
                        //serialize Data after all controls loaded.
                        $('#frmPhysicalExamTemplateDetailRevamp').data('serialize', $('#frmPhysicalExamTemplateDetailRevamp').serialize());

                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }
    },

    LoadEntityBasedData: function (entityID) {

        PhysicalExamTemplateDetailRevamp.LoadBasicFeeGroup(entityID).done(function () {

        });
        PhysicalExamTemplateDetailRevamp.LoadSupervisingProvider(entityID).done(function () {

        });
        PhysicalExamTemplateDetailRevamp.LoadSpecialty(entityID).done(function () {
            $('#frmPhysicalExamTemplateDetailRevamp').bootstrapValidator('revalidateField', $('#frmPhysicalExamTemplateDetailRevamp #ddlSpecialty').attr('name'));
        });
        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#tblPhysicalExamTemplateDetailRevamp #ddlFeeGroup', 'GetFeeGroup', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#tblPhysicalExamTemplateDetailRevamp #ddlFeeGroup', 'GetFeeGroup', true, null);
        }
    },

    LoadSupervisingProvider: function (entityID) {
        // Loads Entity Based Supervising Provider
        return PhysicalExamTemplateDetailRevamp.FillSupervisingProvider(entityID).done(function (response) {
            if (response.status != false) {
                var feegroup_detail = JSON.parse(response.SupervisingProviderLoad_JSON);
                $("#PhysicalExamTemplateDetailRevamp #ddlSupervisingProvider").empty();
                $("#PhysicalExamTemplateDetailRevamp #ddlSupervisingProvider").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(feegroup_detail, function (i, item) {
                    $("#PhysicalExamTemplateDetailRevamp #ddlSupervisingProvider").append(
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
            return PhysicalExamTemplateDetailRevamp.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {
                    var feegroup_detail = JSON.parse(response.SpecialtyLoad_JSON);
                    $("#PhysicalExamTemplateDetailRevamp #ddlSpecialty").empty();
                    $("#PhysicalExamTemplateDetailRevamp #ddlSpecialty").append($('<option/>', {
                        value: "",
                        html: "- SELECT -"
                    }));
                    $.each(feegroup_detail, function (i, item) {
                        $("#PhysicalExamTemplateDetailRevamp #ddlSpecialty").append(
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

        if ($(PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData).length > 0) {
            $.each(PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData, function (i, item) {


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

    physicalExamTemplateSave: function () {
        var isValid = false;
        var self = null;
        self = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp');

        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        //objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = PhysicalExamTemplateDetailRevamp.specialityCheckedIds.join();
        //objData.ProviderIds = objData.PhysicalExamTemplateProvider = PhysicalExamTemplateDetailRevamp.providerCheckedIds.join();

        var isStillValid = false;

        //if (PhysicalExamTemplateDetailRevamp.validateSelectedTemplateData())
        //{
        myJSON = JSON.stringify(objData);
        PhysicalExamTemplateDetailRevamp.savePhysicalExamTemplate(myJSON).done(function (response) {
            if (response != null && response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    PhysicalExamTemplateDetailRevamp.deleteObservations = [];
                    utility.DisplayMessages(response.message, 1);
                    if (response.phyExamTemplateId != "") {
                        PhysicalExamTemplateDetailRevamp.params.PhysicalExamTemplateId = response.phyExamTemplateId;
                        for (var count in PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData) {
                            PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData[count];
                        }
                    }
                    //PhysicalExamTemplateDetailRevamp.loadHospitalizationHx();

                    PhysicalExamTemplateDetailRevamp.params.mode = "Edit";
                    if (PhysicalExamTemplateDetailRevamp.params.ParentCtrl == "clinicalTabProgressNote" && UnloadHospitalizationhx == true) {
                        PhysicalExamTemplateDetailRevamp.getHospitalizationHxInfo(HospitalizationHxType, UnloadHospitalizationhx);
                    }
                    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #hfHospitalizationHxId").val(response.HospitalizationHxId);
                    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmClinicalHospitalizationHx').data('serialize', $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmClinicalHospitalizationHx').serialize());
                    //

                    // Empty global variables
                    PhysicalExamTemplateDetailRevamp.specialityCheckedIds = [];
                    PhysicalExamTemplateDetailRevamp.providerCheckedIds = [];
                    PhysicalExamTemplateDetailRevamp.providerSelectedIds = [];
                    PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData = [];
                    PhysicalExamTemplateDetailRevamp.SpecialtyIds = "";
                    PhysicalExamTemplateDetailRevamp.ProviderIds = "";

                    //Start Farooq Ahmad 16-03-2016 Unload the Physical Exam on Save
                    UnloadActionPan(PhysicalExamTemplateDetailRevamp.params["ParentCtrl"], "PhysicalExamTemplateDetailRevamp");
                    if (PhysicalExamTemplate != null) {
                        PhysicalExamTemplate.loadPhysicalExamTemplateMK();
                        PhysicalExamTemplateDetailRevamp.selectedObservations = [];
                    }
                    //End Farooq Ahmad 16-03-2016 Unload the Physical Exam on Save
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });
        //}
        //else {
        //    utility.DisplayMessages("Please select characteristic/Sub-characteristic", 3);
        //}
    },

    savePhysicalExamTemplate: function (PhysicalExamTemplateData, TemplateName) {
        var self = null, IsActive = null;
        self = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        objData["BodyPartId"] = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #ddlBodyParts').val();
        if (PhysicalExamTemplateDetailRevamp.params["mode"] == "Edit") {
            objData["TemplateId"] = (PhysicalExamTemplateDetailRevamp.params["PhysicalExamTemplateId"]);
        }
        else {
            objData["TemplateId"] = '-1';
        }

        if (TemplateName != null) {

            var mainTemplateName = $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp #txtPhysicalExamTemplateName').val();

            if (objData["TemplateId"] == '-1') {
                objData["PhysicalExamTemplateName"] = TemplateName;
                IsActive = "1";
            }
            else {
                objData["PhysicalExamTemplateName"] = mainTemplateName != "" ? mainTemplateName : TemplateName;
            }

            objData["SaveAsTemplateName"] = TemplateName;
        }

        //objData["SpecialtyIds"] = objData["PhysicalExamTemplateSpecialty"];
        //objData["ProviderIds"] = objData["PhysicalExamTemplateProvider"];
        //objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = PhysicalExamTemplateDetailRevamp.specialityCheckedIds.join();
        //objData.ProviderIds = objData.PhysicalExamTemplateProvider = PhysicalExamTemplateDetailRevamp.providerCheckedIds.join();

        //------------------------------------------------------

        var SpecialtyIds = self.find('#ddlPhysicalExamTemplateSpecialty option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["SpecialtyIds"] = SpecialtyIds;

        var ProviderIds = self.find('#ddlPhysicalExamTemplateProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["ProviderIds"] = ProviderIds;

        //------------------------------------------------------

        objData["commandType"] = "Save_PhyscialExam_ECW";
        objData["SystemObservationData"] = PhysicalExamTemplateDetailRevamp.selectedObservations;
        objData["TemplatePreview"] = $("#observationContent").text();
        objData["PhysicalExamTemplateEntity"] = globalAppdata["SeletedEntityId"];

        IsActive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }

        objData["IsActive"] = IsActive;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    validatePhysicalExamTemplate: function () {
        $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + ' #frmPhysicalExamTemplateDetailRevamp')
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
            PhysicalExamTemplateDetailRevamp.physicalExamTemplateSave();
        });
    },

    saveAsPhysicalExam: function () {
        var strMessage = "";
        var params = [];
        params["FromAdmin"] = '0';
        params["TabID"] = "PhysicalExamTemplateSaveAs";
        params["ParentCtrl"] = 'PhysicalExamTemplateDetailRevamp';


        LoadActionPan('PhysicalExamTemplateSaveAs', params, PhysicalExamTemplateDetailRevamp.params.PanelID);

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
        PhysicalExamTemplateDetailRevamp.specialityCheckedIds = [];
        PhysicalExamTemplateDetailRevamp.providerCheckedIds = [];
        PhysicalExamTemplateDetailRevamp.providerSelectedIds = [];
        PhysicalExamTemplateDetailRevamp.selectedPhyExamTempData = [];
        PhysicalExamTemplateDetailRevamp.SpecialtyIds = "";
        PhysicalExamTemplateDetailRevamp.ProviderIds = "";

        utility.UnLoadDialog("frmPhysicalExamTemplateDetailRevamp", function () {

            UnloadActionPan(PhysicalExamTemplateDetailRevamp.params["ParentCtrl"], "PhysicalExamTemplateDetailRevamp");
            if (PhysicalExamTemplate != null) {
                PhysicalExamTemplate.loadPhysicalExamTemplateMK();
                PhysicalExamTemplateDetailRevamp.selectedObservations = [];
                PhysicalExamTemplateDetailRevamp.specialtyIdMKMK = [];
                PhysicalExamTemplateDetailRevamp.ProviderIdMKMK = [];
            }
        }, function () {

            UnloadActionPan(PhysicalExamTemplateDetailRevamp.params["ParentCtrl"], "PhysicalExamTemplateDetailRevamp");
        });
        if (PhysicalExamTemplateDetailRevamp.params.ParentCtrl == "EncounterChargeCapture") {
            var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
            var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
            EditableGrid.initialize(ChargeGridId, EncounterChargeCapture, "0", false, false, false, false, undefined, false);
        }

    },
    // PRD-5 ,Dev By:MAhmad 
    bindobservationAutoComplete: function () {

        PhysicalExamTemplateDetailRevamp.lookupPEObservation_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Observation_JSON = JSON.parse(response.Observation_JSON);
                    $('#' + PhysicalExamTemplateDetailRevamp.params.PanelID + " #Observations").kendoAutoComplete({
                        dataSource: Observation_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Observation...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulPhysicalExamSystemSection").find('#' + dataItem.PEObservationId).length == 0) {
                                var PESystemId = $("#ulPhysicalExamSystems li.active")[0].id;
                                PhysicalExamTemplateDetailRevamp.params.PEObservationId = dataItem.PEObservationId;
                                var li = PhysicalExamTemplateDetailRevamp.addObservations(dataItem.PEObservationId, dataItem.Name, PESystemId);
                                $("#ulPhysicalExamSystemSection").append(li);
                                var sysChecked = $("#chk" + $("#ulPhysicalExamSystems li.active").attr('id')).is(':checked');
                                var objSelectedObservations = {
                                    PESystemId: PESystemId, IsChecked: false, ObservationId: dataItem.PEObservationId, ObservationName: dataItem.Name, IsSystemChecked: sysChecked, IsSystemDeleted: 0, IsObservationDeleted: 0, ObservationOrder: -1
                                };
                                PhysicalExamTemplateDetailRevamp.selectedObservations.push(objSelectedObservations);
                                $("#Observations").val('');

                            }
                            else {
                                utility.DisplayMessages("An observation with same name already exists.", 3);
                                $("#Observations").val('');
                            }
                            e.preventDefault();
                            //PhysicalExamTemplatesRevamp.selectedObservations.push(objSelectedObservations);
                        },
                    });
                    $("#Observations").parent().addClass('size100per');
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    lookupPEObservation_DBCall: function () {

        var objData = new Object();
        objData["IsActive"] = 1;
        objData["commandType"] = "lookup_physicalexam_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },
    // PRD-5 ,Dev By:MAhmad 
}