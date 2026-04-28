Clinical_HPISymFindingDetail = {
    bIsFirstLoad: true,
    params: [],
    array: [],
    myArr: [],
    NewInsertedId: -1,
    selectedFindings: [],
    HPINotesFindingsId: null,
    Load: function (params) {
        BackgroundLoaderShow(true);
        Clinical_HPISymFindingDetail.params = params;
        var isSelectedEntity = false
        var self = $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #tblClinicalHPISymFindingDetail');
        self.loadDropDowns(true).done(function () {
            Clinical_HPISymFindingDetail.IntializeMultiSelectDropDown();
            $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #headingFamilyMember').text(Clinical_HPISymFindingDetail.params.SymptomName);
            if (Clinical_HPISymFindingDetail.params["mode"] == "Edit") {
                Clinical_HPISymFindingDetail.buildFindingsAutoComplete();
                Clinical_HPISymFindingDetail.loadFindings(Clinical_HPISymFindingDetail.params.HPISymptomsId, Clinical_HPISymFindingDetail.params.HPITemplateId);
                Clinical_HPISymFindingDetail.bindDetailsChangeEvents();
            }
        });

    },

    buildFindingsAutoComplete: function () {

        Clinical_HPISymFindingDetail.lookupHPIFindings_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Findings_JSON = response.listFindings;
                    $('#' + Clinical_HPISymFindingDetail.params.PanelID + " #Findings").kendoAutoComplete({
                        dataSource: Findings_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Finding...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulHPISymptomFindings").find('#' + dataItem.HPIFindingsId).length == 0) {
                                var HPISymptomsId = Clinical_HPISymFindingDetail.params.HPISymptomsId;
                                var HPITemplateSymptomId = Clinical_HPISymFindingDetail.params.HPITemplateSymptomId;
                                Clinical_HPISymFindingDetail.addFindingSymptomAssosiation(dataItem.HPIFindingsId, HPISymptomsId, dataItem.Name, HPITemplateSymptomId);
                                $('#' + Clinical_HPISymFindingDetail.params.PanelID + " #Findings").val('');
                            }
                            else {
                                utility.DisplayMessages(dataItem.Name + " already exists.", 3);
                                $('#' + Clinical_HPISymFindingDetail.params.PanelID + " #Findings").val('');
                            }
                            e.preventDefault();
                        },
                    });
                    $('#' + Clinical_HPISymFindingDetail.params.PanelID + " #Findings").parent().addClass('size100per');
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            }

        });

    },

    lookupHPIFindings_DBCall: function () {

        var objData = new Object();
        objData["IsActive"] = 1;
        objData["commandType"] = "lookup_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },

    addFindingSymptomAssosiation: function (findingId, HPISymptomsId, Name, HPITemplateSymptomId) {

        var objData = new Object();
        objData["HPISymptomsId"] = HPISymptomsId;
        objData["HPITemplateSymptomId"] = HPITemplateSymptomId;

        objData["HPIFindingsIds"] = findingId;
        if (!objData["HPIFindingsIds"] || !objData["HPISymptomsId"]) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }
        Clinical_HPISymFindingDetail.addFindingSymptomAssosiation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                utility.DisplayMessages(response.Message, 1);
                Clinical_HPISymFindingDetail.params.HPIFindingId = findingId;
                if (findingId) {
                    $('#' + Clinical_HPISymFindingDetail.params.PanelID + " #Findings").val('');

                    var objselectedFindings = {
                        HPISymptomsId: HPISymptomsId, IsSelected: true,
                        FindingId: findingId, FindingName: Name, IsSymptomChecked: true, HPITemplateSymptomId: HPITemplateSymptomId, HPISymptomsAnswersId: true, IsSymptomsDetail: false,
                        SymptomDescription: "", HPISymptoms_SeverityId: 0, HPISymptoms_LocationIds: '', HPISymptoms_RadiationId: 0,
                        HPISymptoms_QualityId: 0, Onset: '', AssociatedWith: '', HPISymptoms_AggravatedById: 0, HPISymptoms_RelievedById: 0, DetailComments:''
                    };
                    Clinical_HPISymFindingDetail.selectedFindings.push(objselectedFindings);

                    var li = Clinical_HPISymFindingDetail.addFindings(findingId, Name, HPISymptomsId, "");
                    $('#' + Clinical_HPISymFindingDetail.params.PanelID + " #ulHPISymptomFindings").append(li);
                }
            }
        });
    },

    addFindingSymptomAssosiation_DBCall: function (data) {
        var objData = new Object();
        if (data)
            objData = data;
        objData["commandType"] = "Insert_Findings_Symptom_Assosication";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    addFindings: function (HPIFindingId, FindingName, HPISymptomsId, HPISymptomFindingId) {
        var a = Clinical_HPISymFindingDetail.selectedFindings;

        var itemtoRemove = "finding";

        var li = '<li id="' + HPIFindingId + '" parentid="' + HPISymptomsId + '" onclick="Clinical_HPISymFindingDetail.PreviewFindings(' + HPIFindingId + ',\'' + FindingName + '\', this, ' + HPISymptomsId + ');" value="' + HPIFindingId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                '<input type="checkbox" id="chk' + HPIFindingId + '" name="' + HPIFindingId + '" class="pull-left  char" ' +
                'onclick="Clinical_HPISymFindingDetail.PreviewFindings(' + HPIFindingId + ',\'' + FindingName + '\', this, ' + HPISymptomsId + ', event);">' +
                '<label id="lblName' + HPIFindingId + '" class="" data-toggle="tooltip" title="" data-original-title="' + FindingName + '">' + FindingName + '</label><div id="divNameDetail' + HPIFindingId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea spellcheck="true" rows="1" id="txtName' + HPIFindingId + '" onkeypress="" name="Name' + HPIFindingId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                HPIFindingId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="Clinical_HPISymFindingDetail.removeFinding(' + HPISymptomFindingId + ', ' + HPIFindingId + ',event)"><i class="fa fa-close"></i></span></a></li>';
        return li;
    },

    IntializeMultiSelectDropDown: function () {


        $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #sectionComplaintDetails #ddlLocation').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247

        });

        $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #sectionFindingDetails #ddlLocation').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247

        });
    },

    removeFinding: function (HPISymptomsId, HPIFindingId) {
        $("#ulHPISymptomFindings #" + HPIFindingId).remove();

        if (Clinical_HPISymFindingDetail.selectedFindings) {
            for (var i = 0 ; i < Clinical_HPISymFindingDetail.selectedFindings.length; i++) {
                if (Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptomsId == HPISymptomsId && Clinical_HPISymFindingDetail.selectedFindings[i].FindingId == HPIFindingId) {
                    Clinical_HPISymFindingDetail.selectedFindings[i].FindingId = -1;
                }
            }
        }
    },

    loadFindings: function (HPISymptomsId, HPITemplateId) {
        $("#SymptomSections").removeClass('hidden');
        Clinical_HPISymFindingDetail.HPISymptomFindingsLoad(HPISymptomsId, HPITemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.HPIFindingCount > 0) {
                    var HPIFinding_JSON = JSON.parse(response.HPIFinding_JSON);
                    $("#ulHPISymptomFindings li").remove();
                    var selectedTempSym = null;
                    $.each(HPIFinding_JSON, function (i, item) {
                        selectedTempSym = item.HPITemplateSymptomId;
                        if (item.HPIFindingId != "0") {
                            var li = Clinical_HPISymFindingDetail.addFindings(item.HPIFindingId, item.FindingName, item.HPISymptomId, item.HPISymptomFindingsId);
                            $("#ulHPISymptomFindings").append(li);
                        }
                        var isSelected = item.IsFindingSelected == "True" ? true : false;
                        var objselectedFindings = {
                            HPISymptomId: item.HPISymptomId, IsSelected: item.IsFindingSelected == "True" ? true : false,
                            FindingId: item.HPIFindingId, FindingName: item.FindingName, IsSymptomChecked: false, HPITemplateSymptomId:
                                item.HPITemplateSymptomId, HPISymptomFindingsId: item.HPISymptomFindingsId, HPISymptomsAnswersId: null, IsSymptomsDetail: item.IsSymptomsDetail, HPISymptoms_SeverityId: item.HPISymptoms_SeverityId, HPISymptoms_LocationIds: item.HPISymptoms_LocationIds, HPISymptoms_RadiationId: item.HPISymptoms_RadiationId,
                                HPISymptoms_QualityId: item.HPISymptoms_QualityId, Onset: item.Onset, AssociatedWith: item.AssociatedWith, HPISymptoms_AggravatedById: item.HPISymptoms_AggravatedById, HPISymptoms_RelievedById: item.HPISymptoms_RelievedById, DetailComments:''
                        };

                        Clinical_HPISymFindingDetail.selectedFindings.push(objselectedFindings);
                        if (isSelected)
                            $("#ulHPISymptomFindings li[id=" + item.HPIFindingId + "]").addClass('text-success');
                        else
                            $("#ulHPISymptomFindings li[id=" + item.HPIFindingId + "]").removeClass('text-success');
                        $("#ulHPISymptomFindings li #chk" + item.HPIFindingId + "").prop("checked", isSelected);

                    });
                    var desc = "";
                    var TemplatePreview = "<div>" + HPIFinding_JSON[0].TemplatePreview + "</div>";
                    var HPINotesFinding_JSON = JSON.parse(response.HPINotesFinding_JSON);
                    Clinical_HPISymFindingDetail.HPINotesFindingsId = HPINotesFinding_JSON[0].HPINotesFindingsId;
                    var selectedNotesFind = null;
                    $(HPINotesFinding_JSON).filter(function (ind, i) {
                        if (i.HPITemplateSymptomId == selectedTempSym) {
                            selectedNotesFind = i;
                        }
                    });
                    //Start 29-09-2017 Edit By Humaira Yousaf Bug# EMR-4851
                    if ($(TemplatePreview).find('[id*="Finding"]').length > 0) {
                        $(TemplatePreview).find('[id*="Finding"]').each(function () {
                            desc += $(this)[0].outerHTML;
                        });
                        if (desc) {
                            $("#findingContent").html(selectedNotesFind.Desc);
                            Clinical_HPISymFindingDetail.FillDetailComments();                            
                        }
                        else {
                            var findings = []
                            $.each(HPIFinding_JSON, function (i, item) {
                                if (parseInt(item.HPISymptomId) == parseInt(HPISymptomId) && item.IsSelected == "True")
                                    findings.push(item.FindingName);
                            });
                            desc = findings.join(", ");
                            $("#findingContent").html(findings.join(", "));
                        }
                    }
                    else {
                        $("#findingContent").html(selectedNotesFind.Desc);
                        Clinical_HPISymFindingDetail.FillDetailComments();
                    }
                    //End 29-09-2017 Edit By Humaira Yousaf Bug# EMR-4851
                    var HPISymptomId = $("#ulHPISymptomFindings li").attr('parentid');
                    $("#ulHPISymptomFindings li").each(function () {
                        var HPIFindingId = $(this).attr('id');
                        var found = $("#findingContent").find('[id*="divSymptom' + HPISymptomId + 'Finding' + HPIFindingId + '"]').length;
                        var isSelected = true;
                        if (found) {
                            isSelected = true;
                            $("#ulHPISymptomFindings li[id=" + HPIFindingId + "]").addClass('text-success');
                        }
                        else {
                            isSelected = false;
                            $("#ulHPISymptomFindings li[id=" + HPIFindingId + "]").removeClass('text-success');
                        }
                        $("#ulHPISymptomFindings li #chk" + HPIFindingId + "").prop("checked", isSelected);
                    });
                }
                //Start 29-09-2017 Edit By Humaira Yousaf Bug# EMR-4851
                else {
                    var HPINotesFinding_JSON = JSON.parse(response.HPINotesFinding_JSON);
                    if (HPINotesFinding_JSON.length > 0) {
                        Clinical_HPISymFindingDetail.HPINotesFindingsId = HPINotesFinding_JSON[0].HPINotesFindingsId;
                        $("#findingContent").html(HPINotesFinding_JSON[0].Desc);
                    }
                }
                //End 29-09-2017 Edit By Humaira Yousaf Bug# EMR-4851
                
                if (HPINotesFinding_JSON[0].Desc.indexOf('Denies:') > 0) {
                    $('#SymptomSections').addClass('disableAll');
                    $('#sectionFindingDetails').addClass('disableAll');
                }
                else {
                    $('#SymptomSections').removeClass('disableAll');
                    $('#sectionFindingDetails').removeClass('disableAll');
                }
            }
        });
    },
    FillDetailComments: function () {
        var HPISymptomId = $("#ulHPISymptomFindings li").attr('parentid');
        var detailComments = $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments');
        if (detailComments.length > 0) {
            $('#txtDetailComments').val($(detailComments).text().trim().slice(0,-1));
        }
        else {
            $('#txtDetailComments').val('');
        }
    },
    HPISymptomFindingsLoad: function (HPISymptomsId, HPITemplateId) {
        var objData = new Object();
        objData["HPISymptomId"] = HPISymptomsId;
        objData["HPITemplateId"] = HPITemplateId;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "load_hpi_symptom_findings_note";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    PreviewFindings: function (findingId, findingName, obj, HPISymptomId, event) {
        //if (event) {
        //    event.stopPropagation();
        //}
        var prevFinding = $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ulHPISymptomFindings li.active').attr('id');
        if (prevFinding) {
            Clinical_HPISymFindingDetail.addFindingsDetails(prevFinding);
        }
        $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');
        Clinical_HPISymFindingDetail.loadFindingsDetails(findingId);
        $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ulHPISymptomFindings li').removeClass('active');
        if ($(obj).attr('type') == "checkbox") {
            $(obj).parents('li').addClass('active');
            if ($(obj).prop('checked') == true) {
                $(obj).parents('li').addClass('text-success');
            }
            else {
                $(obj).parents('li').removeClass('text-success');
            }
        }
        else {
            $(obj).addClass('active');
        }

        if ($(obj).prop('checked') == true) {

            $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').remove();

            var deli = $("#delimiter option:selected").text() + " ";

            if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).length > 0) {
                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).remove();
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + findingId);
                $newDiv.attr("style", "display: inline;");

                $("#findingContent").append($newDiv);
                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).show();
                var txttoAppend = findingName;

                if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length > 1)
                    txttoAppend = findingName + deli;

                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).append(txttoAppend);
            }
            else {

                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + findingId);
                $newDiv.attr("style", "display: inline;");

                var txttoAppend = findingName;
                txttoAppend = findingName + deli;

                $("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").last().after($newDiv);

                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).show();

                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).append(txttoAppend);
            }

            if (Clinical_HPISymFindingDetail.selectedFindings) {
                for (var i = 0 ; i < Clinical_HPISymFindingDetail.selectedFindings.length; i++) {
                    if (Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptomId == HPISymptomId && Clinical_HPISymFindingDetail.selectedFindings[i].FindingId == findingId) {
                        Clinical_HPISymFindingDetail.selectedFindings[i].IsChecked = true;
                        Clinical_HPISymFindingDetail.selectedFindings[i].IsSymptomChecked = true;
                        Clinical_HPISymFindingDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                    }
                    if (Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        Clinical_HPISymFindingDetail.selectedFindings[i].IsSymptomChecked = true;
                        Clinical_HPISymFindingDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                    }
                }
            }
        }
        else if ($(obj).prop('checked') == false) {
            if (Clinical_HPISymFindingDetail.selectedFindings) {
                for (var i = 0 ; i < Clinical_HPISymFindingDetail.selectedFindings.length; i++) {
                    if (Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptomId == HPISymptomId && Clinical_HPISymFindingDetail.selectedFindings[i].FindingId == findingId) {
                        Clinical_HPISymFindingDetail.selectedFindings[i].IsChecked = false;
                        Clinical_HPISymFindingDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                    }
                }
            }
            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).remove();
        }
    },

    loadFindingsDetails: function (findingId) {

        if (Clinical_HPISymFindingDetail.selectedFindings) {
            for (var i = 0 ; i < Clinical_HPISymFindingDetail.selectedFindings.length; i++) {
                if (Clinical_HPISymFindingDetail.selectedFindings[i].FindingId == findingId) {
                    $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #sectionFindingDetails').resetAllControls(null);
                    $('#' + Clinical_HPISymFindingDetail.params.PanelID + " ddlLocation option").removeAttr("selected");
                    $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation').multiselect('clearSelection');
                    $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation').multiselect("refresh");
                    var self = $('#' + Clinical_HPISymFindingDetail.params.PanelID + " #sectionFindingDetails");

                    utility.bindMyJSONByName(true, Clinical_HPISymFindingDetail.selectedFindings[i], false, self).done(function () {
                        if (Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_LocationIds) {
                            $('#' + Clinical_HPISymFindingDetail.params.PanelID + " #ddlLocation").val(Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_LocationIds.split(','));
                            $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation').multiselect("refresh");
                        }
                        //Start 04-10-2017 Edit By Humaira Yousaf IMP-1172
                        var symptomId = Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptomId;
                        var detailComments = $("#findingContent #divSymptom" + symptomId + 'Finding' + findingId + 'txtDetailComments');
                        if (detailComments.length > 0) {
                            $('#txtDetailComments').val($(detailComments).text().trim().slice(0, -1));
                        }
                        //End 04-10-2017 Edit By Humaira Yousaf IMP-1172
                    });
                }
            }
        }

    },

    addFindingsDetails: function (findingId) {

        var LocationIDS = $('#ddlLocation option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var severity = $("#ddlSeverity option:Selected").val();
        var radiation = $("#ddlRadiation option:Selected").val();
        var quality = $("#ddlQuality option:Selected").val();
        var onset = $("#txtOnset").val();
        var associatedwith = $("#txtAssociatedWith").val();
        var aggravated = $("#ddlAggravatedBy option:Selected").val();
        var relieved = $("#ddlRevieledBy option:Selected").val();
        var detailComments = $("#txtDetailComments").val();

        for (var i = 0 ; i < Clinical_HPISymFindingDetail.selectedFindings.length; i++) {
            if (Clinical_HPISymFindingDetail.selectedFindings[i].FindingId == findingId) {
                Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_SeverityId = severity;
                Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_LocationIds = LocationIDS;
                Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_RadiationId = radiation;
                Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_QualityId = quality;
                Clinical_HPISymFindingDetail.selectedFindings[i].Onset = onset;
                Clinical_HPISymFindingDetail.selectedFindings[i].AssociatedWith = associatedwith;
                Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_AggravatedById = aggravated;
                Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_RelievedById = relieved;
                Clinical_HPISymFindingDetail.selectedFindings[i].IsSymptomsDetail = false;
                Clinical_HPISymFindingDetail.selectedFindings[i].DetailComments = detailComments;

            }
        }

        $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #sectionFindingDetails').resetAllControls(null);
        $('#' + Clinical_HPISymFindingDetail.params.PanelID + " ddlLocation option").removeAttr("selected");
        $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation').multiselect('clearSelection');
        $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation').multiselect("refresh");
    },

    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + Clinical_HPISymFindingDetail.params["PanelID"];
        $.each(ddlCommaSeparatedIds, function (index, Item) {
            if (isHide) {
                $(parrentPanelId + " #" + Item).multiselect('disable');
            }
            else {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            }
        });
    },

    domReady: function () {

        $(document).ready(function () {
            $(document).click(function (event) {
                var $Item = $(Clinical_HPISymFindingDetail.selectedListItem);
                var id = $Item.attr('id');
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

    addNewItem: function (itemType) {

        var addSubCharIcon = "";

        charSelectAll = null;
        subCharSelectAll = null;

        var isSubCharacteristic = false;
        var ulControl = "";
        var currentLiClick = "";
        var currentCtrlId = "";
        var currentParentId = "";
        var currentId = "";
        currentId = Clinical_HPISymFindingDetail.NewInsertedId--;
        var subcharacteristicExist = "";

        var saveMethodHPI = "";

        currentLiClick = "";
        currentCtrlId = "ulHPISymptomFindings";
        ulControl = $('#' + Clinical_HPISymFindingDetail.params.PanelID + " #frmClinicalHPISymFindingDetail #" + currentCtrlId);
        saveMethodHPI = "Clinical_HPISymFindingDetail.AddFinding(this, '" + currentId + "');";

        if (ulControl != null && ulControl != "") {
            var currentLiClass = "";

            var arrNewlyAddedLi = ulControl.find("li[id*='-']");

            if (itemType.toLowerCase() != "system") {
                currentParentId = ulControl.find("li:last").attr("parentid");
                if (currentParentId == null)
                    currentParentId = ulControl.attr("ParentId");
            }

            var onClick = "";
            onClick = "";//"Clinical_HPISymFindingDetail.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";
            var deleteFunction = "Clinical_HPISymFindingDetail.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

            var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

            liInnerText = '<div class="">' + deleteIcon + addSubCharIcon
                //+ '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="Clinical_HPISymFindingDetail.selectParentControls(this);Clinical_HPISymFindingDetail.toggleCheckBoxes(this);">'
                + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                + '<textarea spellcheck="true" rows="1" id="txtName' + currentId + '" name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
                + '<div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodHPI + '" class="btn btn-link btn-xs">'
                + '<i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';

            var liTobeAdded = '<li id="' + currentId + '" ' + currentLiClass + ' parentid="' + currentParentId + '" onclick="' + onClick + '" value="' + currentId + '" refValue="' + currentId + '"' + subcharacteristicExist + '>' + liInnerText + '</li>';

            ulControl.prepend(liTobeAdded);
            ulControl.find('li#' + currentId + ' #txtName' + currentId).focus()

        }
    },

    AddFinding: function (obj, controlId) {
        var HPISymptomsId = Clinical_HPISymFindingDetail.params.HPISymptomsId;

        var objData = new Object();
        objData["HPISymptomsId"] = HPISymptomsId;
        objData["HPITemplateSymptomId"] = Clinical_HPISymFindingDetail.params.HPITemplateSymptomId;
        objData["HPITemplateId"] = Clinical_HPISymFindingDetail.params.HPITemplateId;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = 1;

        if (!objData["Name"]) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        Clinical_HPISymFindingDetail.saveHPIFindings_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_HPISymFindingDetail.params.HPIFindingId = response.HPIFindingsId;
                    var li = Clinical_HPISymFindingDetail.addFindings(response.HPIFindingsId, objData["Name"], HPISymptomsId);
                    $("#ulHPISymptomFindings").append(li);

                    var objselectedFindings = {
                        HPISymptomId: HPISymptomsId, IsChecked: false, FindingId: response.HPIFindingsId, FindingName: objData["Name"], IsSymptomChecked: false,
                        SymptomDescription: "", IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: null, IsSymptomsDetail: false, HPISymptoms_SeverityId: 0, HPISymptoms_LocationIds: '', HPISymptoms_RadiationId: 0,
                        HPISymptoms_QualityId: 0, Onset: '', AssociatedWith: '', HPISymptoms_AggravatedById: 0, HPISymptoms_RelievedById: 0, DetailComments:''
                    };
                    Clinical_HPISymFindingDetail.selectedFindings.push(objselectedFindings);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                $("#" + controlId).remove();
            }
        });
    },

    saveHPIFindings_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "insert_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },

    deleteItem: function (obj, ctrlId, currentId) {

        var itemId = $(obj).closest("li").attr('id');

        if (ctrlId == "ulHPISymptomFindings") {
            $("#" + currentId).remove();
        }
    },

    bindDetailsChangeEvents: function () {

        $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #sectionFindingDetails').on('input', function (e) {
            var controlId = $(e.target).attr('id');

            var HPISymptomId = $("#ulHPISymptomFindings li").attr('parentid');
            var HPIFindingId = 0;
            if ($("#ulHPISymptomFindings li.active").length > 0) {
                HPIFindingId = $("#ulHPISymptomFindings li.active").attr('id');
            }

            var delimiter = $("#delimiter option:selected").text() + " ";

            if (HPIFindingId) {
                if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).length > 0) {
                    var txtToAppend = '';

                    if (controlId == 'ddlSeverity') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').length > 0) {
                            txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimiter;
                            if ($("#ddlSeverity").val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimiter;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').append(txtToAppend);

                        }
                    }
                    else if (controlId == 'ddlRadiation') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').length > 0) {
                            txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimiter;
                            if ($('#ddlRadiation').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimiter;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'ddlQuality') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').length > 0) {
                            txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimiter;
                            if ($('#ddlQuality').val() == 0) {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimiter;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'txtOnset') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').length > 0) {
                            txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimiter;
                            if ($('#txtOnset').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimiter;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'txtAssociatedWith') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').length > 0) {
                            txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimiter;
                            if ($('#txtAssociatedWith').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimiter;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'ddlAggravatedBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').length > 0) {
                            txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimiter;
                            if ($('#ddlAggravatedBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimiter;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'ddlRevieledBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').length > 0) {
                            txtToAppend = 'Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimiter;
                            if ($('#ddlRevieledBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimiter;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').append(txtToAppend);
                        }
                    }
                    //Start 03-10-2017 Edit By Humaira Yousaf IMP-1172
                    else if (controlId == 'txtDetailComments') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').length > 0) {
                            txtToAppend = ' ' + $('#txtDetailComments').val() + delimiter;
                            if ($('#txtDetailComments').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments');
                            $newDiv.attr("style", "display: inline;");

                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = ' ' + $('#txtDetailComments').val() + delimiter;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').append(txtToAppend);
                        }
                    }
                    //End 03-10-2017 Edit By Humaira Yousaf IMP-1172
                }
            }
            else {
                if ($("#findingContent #divSymptom" + HPISymptomId).length > 0) {
                    var txtToAppend = '';
                    Clinical_HPISymFindingDetail.resetFindings(HPISymptomId);
                    if (controlId == 'ddlSeverity') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').length > 0) {
                            txtToAppend = ' Severity: ' + $('#ddlSeverity option:selected').text() + delimiter;
                            if ($("#ddlSeverity").val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlSeverity');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Severity: ' + $('#ddlSeverity option:selected').text() + delimiter;;
                            }
                            else {
                                txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimiter;
                            }

                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').append(txtToAppend);

                        }
                    }
                    else if (controlId == 'ddlRadiation') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').length > 0) {
                            txtToAppend = ' Radiation: ' + $('#ddlRadiation option:selected').text() + delimiter;
                            if ($('#ddlRadiation').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlRadiation');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Radiation: ' + $('#ddlRadiation option:selected').text() + delimiter;

                            }
                            else {
                                txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimiter;
                            }

                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'ddlQuality') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').length > 0) {
                            txtToAppend = ' Quality: ' + $('#ddlQuality option:selected').text() + delimiter;
                            if ($('#ddlQuality').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlQuality');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Quality: ' + $('#ddlQuality option:selected').text() + delimiter;
                            }
                            else {
                                txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimiter;
                            }

                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'txtOnset') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').length > 0) {
                            txtToAppend = ' Onset: ' + $('#txtOnset').val() + delimiter;
                            if ($('#txtOnset').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'txtOnset');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Onset: ' + $('#txtOnset').val() + delimiter;
                            }
                            else {
                                txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimiter;
                            }
                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'txtAssociatedWith') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').length > 0) {
                            txtToAppend = ' Associated With: ' + $('#txtAssociatedWith').val() + delimiter;
                            if ($('#txtAssociatedWith').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'txtAssociatedWith');
                            $newDiv.attr("style", "display: inline;");
                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Associated With: ' + $('#txtAssociatedWith').val() + delimiter;
                            }
                            else {
                                txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimiter;
                            }

                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'ddlAggravatedBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').length > 0) {
                            txtToAppend = ' Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimiter;
                            if ($('#ddlAggravatedBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlAggravatedBy');
                            $newDiv.attr("style", "display: inline;");
                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimiter;
                            }
                            else {
                                txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimiter;
                            }
                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'ddlRevieledBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').length > 0) {
                            txtToAppend = ' Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimiter;
                            if ($('#ddlRevieledBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlRevieledBy');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimiter;
                            }
                            else {
                                txtToAppend = 'Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimiter;
                            }
                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').append(txtToAppend);
                        }
                    }
                    //Start 03-10-2017 Edit By Humaira Yousaf IMP-1172
                    else if (controlId == 'txtDetailComments') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').length > 0) {
                            txtToAppend = '  ' + $('#txtDetailComments').val() + delimiter;
                            if ($('#txtDetailComments').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'txtDetailComments');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = '  ' + $('#txtDetailComments').val() + delimiter;
                            }
                            else {
                                txtToAppend = ' ' + $('#txtDetailComments').val() + delimiter;
                            }
                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').append(txtToAppend);
                        }
                    }
                    //End 03-10-2017 Edit By Humaira Yousaf IMP-1172
                }
            }
        });


        $('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation').on('change', function () {
            var HPISymptomId = $("#ulHPISymptomFindings li").attr('parentid');
            var HPIFindingId = 0;
            if ($("#ulHPISymptomFindings li.active").length > 0) {
                HPIFindingId = $("#ulHPISymptomFindings li.active").attr('id');
            }

            var delimiter = $("#delimiter option:selected").text() + " ";

            if (HPIFindingId) {
                if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).length > 0) {
                    var txtToAppend = '';
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').length > 0) {
                        txtToAppend = 'Location: ' + Clinical_HPISymFindingDetail.getLocationText($('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation')) + delimiter; // $('#ddlLocation option:selected').text();
                        if (!$("#ddlLocation").val()) {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation');
                        $newDiv.attr("style", "display: inline;");
                        if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                            txtToAppend = ' Location: ' + Clinical_HPISymFindingDetail.getLocationText($('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation')) + delimiter; //$('#ddlLocation option:selected').text();
                        }
                        else {
                            txtToAppend = 'Location: ' + Clinical_HPISymFindingDetail.getLocationText($('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation')) + delimiter; //$('#ddlLocation option:selected').text();
                        }
                        //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').append(txtToAppend);

                    }
                }
            }
            else {
                if ($("#findingContent #divSymptom" + HPISymptomId).length > 0) {
                    var txtToAppend = '';
                    Clinical_HPISymFindingDetail.resetFindings(HPISymptomId);

                    if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').length > 0) {
                        txtToAppend = ' Location: ' + Clinical_HPISymFindingDetail.getLocationText($('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation')) + delimiter;
                        if (!$("#ddlLocation").val()) {
                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlLocation');
                        $newDiv.attr("style", "display: inline;");
                        if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                            txtToAppend = ' Location: ' + Clinical_HPISymFindingDetail.getLocationText($('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation')) + delimiter;
                        }
                        else {
                            txtToAppend = 'Location: ' + Clinical_HPISymFindingDetail.getLocationText($('#' + Clinical_HPISymFindingDetail.params.PanelID + ' #ddlLocation')) + delimiter;
                        }
                        //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                        Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                        $('#findingContent #divSymptom' + HPISymptomId).show();

                        $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').append(txtToAppend);

                    }
                }
            }
        });

    },

    getLocationText: function (ddlLocation) {
        var str = '';
        var selectedText = '';
        var selectedValues = ddlLocation.find('option:selected').map(function () {
            return this.text
        });
        if (selectedValues.length > 0) {
            var selectedText = $.makeArray(selectedValues).join();
            selectedText = selectedText.replace(/,/g, ", ");
            str += selectedText.replace(/,([^,]*)$/, ' and $1');
        }
        return str;
    },

    resetFindings: function (HPISymptomId) {
        if (Clinical_HPISymFindingDetail.selectedFindings) {
            for (var i = 0; i < Clinical_HPISymFindingDetail.selectedFindings.length; i++) {
                if (Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                    var findingid = Clinical_HPISymFindingDetail.selectedFindings[i].FindingId;
                    $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingid).remove();

                    Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_SeverityId = '';
                    Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_LocationIds = '';
                    Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_RadiationId = '';
                    Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_QualityId = '';
                    Clinical_HPISymFindingDetail.selectedFindings[i].Onset = '';
                    Clinical_HPISymFindingDetail.selectedFindings[i].AssociatedWith = '';
                    Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_AggravatedById = '';
                    Clinical_HPISymFindingDetail.selectedFindings[i].HPISymptoms_RelievedById = '';
                    Clinical_HPISymFindingDetail.selectedFindings[i].IsChecked = false;
                    Clinical_HPISymFindingDetail.selectedFindings[i].DetailComments = '';
                }
            }
        }

        for (var i = 0; i < $("#ulHPISymptomFindings li").length; i++) {
            $("#ulHPISymptomFindings #chk" + $("#ulHPISymptomFindings li")[i].id).prop('checked', false);
        }

    },

    UnLoad: function () {
        // Empty global variables
        Clinical_HPISymFindingDetail.array = [];
        Clinical_HPISymFindingDetail.myArr = [];
        Clinical_HPISymFindingDetail.NewInsertedId = -1;
        Clinical_HPISymFindingDetail.selectedFindings = [];
        var HPINotesFindingsId = Clinical_HPISymFindingDetail.HPINotesFindingsId;
        Clinical_HPISymFindingDetail.UpdateFindingDesc(HPINotesFindingsId).done(function (response) {
            var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #Cli_HPICompl_HPINFindingsId' + HPINotesFindingsId);
            if (selectedNoteDesc) {
                if ($("#findingContent").text() != "") {
                    $(selectedNoteDesc).text($("#findingContent").text().trim());
                    $(selectedNoteDesc).attr('style', $("#findingContent").attr('style'));
                }
                else {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #Cli_HPICompl_HPINFindingsId' + HPINotesFindingsId).closest('ul').remove();
                }
                Clinical_ProgressNote.saveComponentSOAPText('Complaints');
                Clinical_ProgressNote.hoverFunction();
            }
            UnloadActionPan(Clinical_HPISymFindingDetail.params["ParentCtrl"], "Clinical_HPISymFindingDetail");
        });

    },

    applyStyle: function (style) {
        if (style == 'bold') {
            $("#findingContent").css('font-weight', 'bold');
        }
        else if (style == 'italic') {
            $("#findingContent").css('font-style', 'italic');
        }
        else if (style == 'underline') {
            $("#findingContent").css('text-decoration', 'underline');
        }
        else if (style == 'reset') {
            $("#findingContent").css('font-weight', 'normal');
            $("#findingContent").css('font-style', 'normal');
            $("#findingContent").css('text-decoration', 'none');
        }
        else if (style == 'clear') {
            $("#findingContent").html('');
        }
    },

    UpdateFindingDesc: function (HPINotesFindingsId) {
        var objData = new Object();
        objData["HPINotesFindingsId"] = HPINotesFindingsId;
        objData["Desc"] = $("#findingContent").html();
        objData["commandType"] = "Update_HPI_Selected_Findings_Desc_For_Notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    AppendControlDiv: function (divToAdd, HPISymptomId, HPIFindingId) {
        if (HPIFindingId) {
            var commentsDiv = $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments');
            if (commentsDiv.length > 0) {
                $(commentsDiv).before(divToAdd);
            }
            else {
                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append(divToAdd);
            }
        }
        else {
            var commentsDiv = $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments');
            if (commentsDiv.length > 0) {
                $(commentsDiv).before(divToAdd);
            }
            else {
                $("#findingContent #divSymptom" + HPISymptomId).append(divToAdd);
            }
        }
    },
}