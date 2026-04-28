ProcedureSysObservationDetail = {
    bIsFirstLoad: true,
    params: [],
    array: [],
    myArr: [],
    NewInsertedId: -1,
    selectedObservations: [],
    ProcedureNotesObservationId: null,
    Load: function (params) {
        BackgroundLoaderShow(true);
        ProcedureSysObservationDetail.params = params;
        var isSelectedEntity = false
        var self = $('#' + ProcedureSysObservationDetail.params.PanelID + ' #tblProcedureSysObservationDetail');
        self.loadDropDowns(true).done(function () {
            if (ProcedureSysObservationDetail.params["mode"] == "Edit") {
                ProcedureSysObservationDetail.bindobservationAutoComplete();
                ProcedureSysObservationDetail.loadObservations(ProcedureSysObservationDetail.params.ProcedureTemplateSystemId);
            }
        });

    },

    bindobservationAutoComplete: function () {

        PhysicalExamTemplatesRevamp.lookupPEObservation_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Observation_JSON = JSON.parse(response.Observation_JSON);
                    $('#' + ProcedureSysObservationDetail.params.PanelID + " #Observations").kendoAutoComplete({
                        dataSource: Observation_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Observation...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulProcedureSystemSection").find('#' + dataItem.id).length == 0) {

                                var PESystemId = ProcedureSysObservationDetail.params.PESystemId;
                                var ProcedureTemplateSystemId = ProcedureSysObservationDetail.params.ProcedureTemplateSystemId;
                                if (PESystemId > 0 && ProcedureTemplateSystemId > 0) {
                                    ProcedureSysObservationDetail.addObservationSystemAssosiation(dataItem.PEObservationId, PESystemId, dataItem.Name, ProcedureTemplateSystemId);
                                    $('#' + ProcedureSysObservationDetail.params.PanelID + " #Observations").val('');
                                }
                                else
                                    utility.DisplayMessages("SystemId or TemplateSystemId not found.", 3);

                            }
                            else {
                                utility.DisplayMessages(dataItem.value + " already exists.", 3);
                            }

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

    GetProcedureTemplateSoapText: function (ProcedureId) {
        var params = {};
        params["ProcedureId"] = ProcedureId;
        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
        params["commandType"] = "Get_Procedure_Template_SoapText";
        var data = JSON.stringify(params);
        return MDVisionService.APIServiceSync(data, "ProcedureTemplate", "ProcedureTemplate");
    },
    addObservationSystemAssosiation: function (observationId, PESystemId, Name, ProcedureTemplateSystemId) {

        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["ProcedureTemplateSystemId"] = ProcedureTemplateSystemId;

        objData["ObservationId"] = observationId;
        if (!objData["ObservationId"] || !objData["PESystemId"]) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }
        ProcedureSysObservationDetail.addObservationSystemAssosiation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                utility.DisplayMessages(response.Message, 1);
                if (observationId) {
                    $('#' + ProcedureSysObservationDetail.params.PanelID + " #Observations").val('');
                    var objSelectedObservations = { PESystemId: PESystemId, IsSelected: true, ObservationId: observationId, ObservationName: Name, IsSystemChecked: true };
                    var li = ProcedureSysObservationDetail.addObservations(observationId, Name, PESystemId);
                    $('#' + ProcedureSysObservationDetail.params.PanelID + " #ulProcedureSystemSection").append(li);
                }
            }
        });
    },
    addObservationSystemAssosiation_DBCall: function (data) {
        var objData = new Object();
        if (data)
            objData = data;
        objData["commandType"] = "Insert_Observation_System_Assosication";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureTemplate", "ProcedureTemplate");
    },
    addObservations: function (PEObservationId, ObservatioName, PESystemId) {
        var a = ProcedureSysObservationDetail.selectedObservations;

        var itemtoRemove = "observation";

        var li = '<li id="' + PEObservationId + '" parentid="' + PESystemId + '" onclick="ProcedureSysObservationDetail.PreviewObservations(' + PEObservationId + ',\'' + ObservatioName + '\', this, ' + PESystemId + ');" value="' + PEObservationId + '" refvalue="" subcharacteristicexist=" " class=""><div class="">' +
                 '<label id="lblName' + PEObservationId + '" class="" data-toggle="tooltip" title="" data-original-title="' + ObservatioName + '">' + ObservatioName + '</label><div id="divNameDetail' + PEObservationId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PEObservationId + '" onkeypress="" name="Name' + PEObservationId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PEObservationId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
        return li;
    },

    removeItem: function (PESystemId, control, PEObservationId) {
        if (control == "observation") {
            $("#ulProcedureSystemSection #" + PEObservationId).remove();

            if (ProcedureSysObservationDetail.selectedObservations[i].PESystemId == PESystemId && ProcedureSysObservationDetail.selectedObservations[i].ObservationId == PEObservationId) {
                ProcedureSysObservationDetail.selectedObservations[i].ObservationId = -1;
            }
        }
    },

    removeObservation: function (PESystemId, PEObservationId) {
        $("#ulProcedureSystemSection #" + PEObservationId).remove();

        if (ProcedureSysObservationDetail.selectedObservations) {
            for (var i = 0 ; i < ProcedureSysObservationDetail.selectedObservations.length; i++) {
                if (ProcedureSysObservationDetail.selectedObservations[i].PESystemId == PESystemId && ProcedureSysObservationDetail.selectedObservations[i].ObservationId == PEObservationId) {
                    ProcedureSysObservationDetail.selectedObservations[i].ObservationId = -1;
                }
            }
        }
    },
    switchNextPrevious: function (direction) {

        var templateArr = ProcedureSysObservationDetail.params.templateIDs;
        var current = $('#' + ProcedureSysObservationDetail.params.PanelID + " #frmProcedureSysObservationDetail #hfTemplateId").val();
        var next = templateArr[($.inArray(current, templateArr) + 1) % templateArr.length];
        var prev = templateArr[($.inArray(current, templateArr) - 1 + templateArr.length) % templateArr.length];
        var dataChanged = EMRUtility.compareFormDataWithSerialized(ProcedureSysObservationDetail.params.PanelID + ' #frmProcedureSysObservationDetail');

        if (dataChanged == true) {

            ProcedureSysObservationDetail.UpdateObservationDesc(ProcedureSysObservationDetail.ProcedureNotesObservationId).done(function (response) {

                var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureSystem_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]);
                if (selectedNoteDesc) {
                    if ($("#ProcedureobservationContent").val() != "") {
                        $(selectedNoteDesc).text($("#ProcedureobservationContent").val());
                        $(selectedNoteDesc).attr('style', $("#ProcedureobservationContent").attr('style'));
                    }
                    else {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureElement_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]).prev('br').remove();
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureElement_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]).remove();
                    }
                    Clinical_ProgressNote.saveComponentSOAPText('Procedures', true);
                    Clinical_ProgressNote.SaveAndAttachProcedureReport($('#PatientProfile #hfPatientId').val(), Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, true);

                    //Clinical_ProgressNote.updateProgressNoteHTML();
                    Clinical_ProgressNote.hoverFunction();
                }
                if (direction == 'Next') {
                    ProcedureSysObservationDetail.loadObservations(next);
                } else if (direction == 'Previous') {
                    ProcedureSysObservationDetail.loadObservations(prev);
                }
            });

        } else {
            if (direction == 'Next') {



                for (var i in ProcedureSysObservationDetail.params.templatesStyle) {
                    if (ProcedureSysObservationDetail.params.templatesStyle[i][0] == ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"] && ProcedureSysObservationDetail.params.templatesStyle[i][1] != $("#ProcedureobservationContent").attr('style')) {
                        ProcedureSysObservationDetail.params.templatesStyle[i][1] = $("#ProcedureobservationContent").attr('style');

                        ProcedureSysObservationDetail.UpdateObservationDesc(ProcedureSysObservationDetail.ProcedureNotesObservationId).done(function (response) {

                            var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureSystem_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]);
                            if (selectedNoteDesc) {
                                if ($("#ProcedureobservationContent").val() != "") {
                                    $(selectedNoteDesc).text($("#ProcedureobservationContent").val());
                                    $(selectedNoteDesc).attr('style', $("#ProcedureobservationContent").attr('style'));
                                }
                                else {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureElement_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]).prev('br').remove();
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureElement_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]).remove();
                                }
                                Clinical_ProgressNote.saveComponentSOAPText('Procedures',true);

                                Clinical_ProgressNote.SaveAndAttachProcedureReport($('#PatientProfile #hfPatientId').val(), Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, true);

                                //Clinical_ProgressNote.updateProgressNoteHTML();
                                Clinical_ProgressNote.hoverFunction();
                            }

                        });


                    }
                }
                ProcedureSysObservationDetail.loadObservations(next);


            } else if (direction == 'Previous') {

                for (var i in ProcedureSysObservationDetail.params.templatesStyle) {
                    if (ProcedureSysObservationDetail.params.templatesStyle[i][0] == ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"] && ProcedureSysObservationDetail.params.templatesStyle[i][1] != $("#ProcedureobservationContent").attr('style')) {
                        ProcedureSysObservationDetail.params.templatesStyle[i][1] = $("#ProcedureobservationContent").attr('style');

                        ProcedureSysObservationDetail.UpdateObservationDesc(ProcedureSysObservationDetail.ProcedureNotesObservationId).done(function (response) {

                            var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureSystem_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]);
                            if (selectedNoteDesc) {
                                if ($("#ProcedureobservationContent").val() != "") {
                                    $(selectedNoteDesc).text($("#ProcedureobservationContent").val());
                                    $(selectedNoteDesc).attr('style', $("#ProcedureobservationContent").attr('style'));
                                }
                                else {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureElement_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]).prev('br').remove();
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureElement_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]).remove();
                                }
                                Clinical_ProgressNote.saveComponentSOAPText('Procedures',true);

                                Clinical_ProgressNote.SaveAndAttachProcedureReport($('#PatientProfile #hfPatientId').val(), Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, true);

                                //Clinical_ProgressNote.updateProgressNoteHTML();
                                Clinical_ProgressNote.hoverFunction();
                            }

                        });


                    }
                }
                ProcedureSysObservationDetail.loadObservations(prev);
            }
        }
    },
    loadObservations: function (ProcedureTemplateSystemId) {
        ProcedureSysObservationDetail.PhysicalExamSystemObservationsLoad(ProcedureTemplateSystemId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $('#' + ProcedureSysObservationDetail.params.PanelID + " #frmProcedureSysObservationDetail #hfTemplateId").val(ProcedureTemplateSystemId);
                ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"] = ProcedureTemplateSystemId;
                if (response.ProcedureObservationCount > 0) {
                    var res = JSON.parse(response.ProcedureObservation_JSON);
                    var resSystems = JSON.parse(res);
                    $("#ProcedureSystemSections").removeClass('hidden');
                    $("#ulProcedureSystemSection li").remove();
                    $.each(resSystems, function (i, item) {
                        var li = ProcedureSysObservationDetail.addObservations(item.PEObservationId, item.ObservationName, item.PESystemId);
                        ProcedureSysObservationDetail.ProcedureNotesObservationId = item.PEObservationId;
                        $("#ulProcedureSystemSection").append(li);
                        var objSelectedObservations = { PESystemId: item.PESystemId, IsChecked: false, ObservationId: item.PEObservationId, ObservationName: item.ObservationName, IsSystemChecked: false };
                        ProcedureSysObservationDetail.selectedObservations.push(objSelectedObservations);
                        $("#ProcedureobservationContent").val(item.SystemDescription);
                        $("#ProcedureobservationContent").text(item.SystemDescription);
                        $.each(ProcedureSysObservationDetail.params.templatesStyle, function (ind, vl) {

                            if (vl[0] == ProcedureTemplateSystemId) {
                                if (vl[1] == undefined || vl[1] == "") {
                                    vl[1] = 'font-weight: normal; font-style: normal; text-decoration: none';
                                }
                                $("#ProcedureobservationContent").attr('style', vl[1]);
                            }

                        });
                        //$("#ProcedureobservationContent").attr('style', ProcedureSysObservationDetail.params["Content_style"]);
                        ProcedureSysObservationDetail.ProcedureNotesObservationId = item.ProcedureNotesObservationId;
                        $('#' + ProcedureSysObservationDetail.params.PanelID + ' #headingTitle').text('Procedure Template - ' + item.SystemName)
                    });

                    $('#' + ProcedureSysObservationDetail.params.PanelID + " #frmProcedureSysObservationDetail").data('serialize', $('#' + ProcedureSysObservationDetail.params.PanelID + " #frmProcedureSysObservationDetail").serialize());
                }
            }
        });
    },

    selectAllChars: function (obj) {
        if (obj) var sysId = $($($(obj).parent().parent())[0]).attr('parentid');

        if ($(obj).prop('checked') == true) {


            $("#SystemPreview").removeClass('hidden');
            $('#' + ProcedureSysObservationDetail.params.PanelID + ' #ulProcedureSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);

                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3)
                    var observationName = $("#divProcedureSystemSection #lblName" + id_).text();
                    var delimator = $("#delimator option:selected").text() + " ";

                    $("#ProcedureobservationContent").text($("#ProcedureobservationContent").text() + observationName + delimator);

                    var objSelectedObservations = { PESystemId: sysId, IsChecked: true, ObservationId: id_, ObservationName: observationName, IsModified: '1' };
                    ProcedureSysObservationDetail.selectedObservations.push(objSelectedObservations);

                    $("#ulPhysicalExamSystems #chk" + sysId).prop("checked", true);
                }
            });
            //var objSelectedAll = { PESystemId: sysId, IsChecked: true, ObservationId: 'chkboxSelectAllObservations', ObservationName: 'Select All', IsModified: '1' };
            //ProcedureSysObservationDetail.selectedObservations.push(objSelectedAll);

            if (ProcedureSysObservationDetail.selectedObservations) {
                for (var i = 0 ; i < ProcedureSysObservationDetail.selectedObservations.length; i++) {
                    if (ProcedureSysObservationDetail.selectedObservations[i].PESystemId == sysId) {
                        ProcedureSysObservationDetail.selectedObservations[i].IsChecked = true;
                        ProcedureSysObservationDetail.selectedObservations[i].IsSystemChecked = true;
                    }
                }
            }

        }
        else if ($(obj).prop('checked') == false) {
            $('#' + ProcedureSysObservationDetail.params.PanelID + ' #ulProcedureSystemSection li .char').removeClass("green");
            $('#' + ProcedureSysObservationDetail.params.PanelID + ' #ulProcedureSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                }
            });

            if (ProcedureSysObservationDetail.selectedObservations) {
                for (var i = 0 ; i < ProcedureSysObservationDetail.selectedObservations.length; i++) {
                    if (ProcedureSysObservationDetail.selectedObservations[i].PESystemId == sysId) {
                        ProcedureSysObservationDetail.selectedObservations[i].IsChecked = false;
                    }
                }
            }
        }
    },

    PreviewObservations: function (observationId, ObservationName, obj, PESystemId) {
        $("#SystemPreview").removeClass('hidden');

        var objSelectedObservations =
        {
            PESystemId: PESystemId,
            IsChecked: true,
            ObservationId: observationId,
            ObservationName: ObservationName,
            IsModified: '1',
            IsSystemChecked: false
        };

        var deli = $("#delimator option:selected").text() + " ";

        if ($("#ProcedureobservationContent").val())
            $("#ProcedureobservationContent").val($("#ProcedureobservationContent").val() + deli + ObservationName);
        else
            $("#ProcedureobservationContent").val($("#ProcedureobservationContent").val() + ObservationName);
        ProcedureSysObservationDetail.selectedObservations.push(objSelectedObservations);

        if (ProcedureSysObservationDetail.selectedObservations) {
            for (var i = 0 ; i < ProcedureSysObservationDetail.selectedObservations.length; i++) {
                if (ProcedureSysObservationDetail.selectedObservations[i].PESystemId == PESystemId && ProcedureSysObservationDetail.selectedObservations[i].ObservationId == observationId) {
                    ProcedureSysObservationDetail.selectedObservations[i].IsChecked = true;
                    ProcedureSysObservationDetail.selectedObservations[i].IsSystemChecked = true;
                }
                if (ProcedureSysObservationDetail.selectedObservations[i].PESystemId == PESystemId)
                    ProcedureSysObservationDetail.selectedObservations[i].IsSystemChecked = true;
            }
        }
        $("#ulPhysicalExamSystems #chk" + PESystemId).prop("checked", true);
    },

    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + ProcedureSysObservationDetail.params["PanelID"];
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
                var $Item = $(ProcedureSysObservationDetail.selectedListItem);
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
            currentId = ProcedureSysObservationDetail.NewInsertedId--;
            var subcharacteristicExist = "";

            var saveMethodPE = "";

            if (itemType.toLowerCase() == "system") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + ProcedureSysObservationDetail.params.PanelID + " #frmProcedureSysObservationDetail #" + currentCtrlId);
                saveMethodPE = "ProcedureSysObservationDetail.AddSystemPE(this, '" + currentId + "');";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "";
                currentCtrlId = "ulProcedureSystemSection";
                ulControl = $('#' + ProcedureSysObservationDetail.params.PanelID + " #frmProcedureSysObservationDetail #" + currentCtrlId);
                saveMethodPE = "ProcedureSysObservationDetail.AddObservation(this, '" + currentId + "');";
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
                onClick = "";//"ProcedureSysObservationDetail.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";
                var deleteFunction = "ProcedureSysObservationDetail.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                liInnerText = '<div class="">' + deleteIcon + addSubCharIcon
                    //+ '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="ProcedureSysObservationDetail.selectParentControls(this);ProcedureSysObservationDetail.toggleCheckBoxes(this);">'
                    + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                    + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                    + '<textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress="ProcedureSysObservationDetail.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
                    + '<div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodPE + '" class="btn btn-link btn-xs">'
                    + '<i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';

                var liTobeAdded = '<li id="' + currentId + '" ' + currentLiClass + ' parentid="' + currentParentId + '" onclick="' + onClick + '" value="' + currentId + '" refValue="' + currentId + '"' + subcharacteristicExist + '>' + liInnerText + '</li>';

                ulControl.prepend(liTobeAdded);
                ulControl.find('li#' + currentId + ' #txtName' + currentId).focus()

            }

        }
    },

    AddObservation: function (obj, controlId) {
        var PESystemId = ProcedureSysObservationDetail.params.PESystemId;

        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = 1;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        ProcedureSysObservationDetail.savePEObservation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    ProcedureSysObservationDetail.params.PEObservationId = response.PEObservationId;
                    var li = ProcedureSysObservationDetail.addObservations(response.PEObservationId, objData["Name"], PESystemId);
                    $("#ulProcedureSystemSection").append(li);

                    var objSelectedObservations = { PESystemId: PESystemId, IsChecked: false, ObservationId: response.PEObservationId, ObservationName: objData["Name"], IsSystemChecked: true };
                    ProcedureSysObservationDetail.selectedObservations.push(objSelectedObservations);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                $("#" + controlId).remove();
            }
        });
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

        } else if (ctrlId == "ulProcedureSystemSection") {
            $("#" + currentId).remove();
        }
    },

    validateSelectedTemplateData: function () {

        var isValid = true;

        if ($(ProcedureSysObservationDetail.selectedPhyExamTempData).length > 0) {
            $.each(ProcedureSysObservationDetail.selectedPhyExamTempData, function (i, item) {
                if (item.IsModified == "1" && $.parseJSON(item.IsChecked.toString().toLowerCase()) == true) {
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
                }
            });
        }
        else {
            isValid = false;
        }
        return isValid;
    },

    UnLoad: function () {
        // Empty global variables
        ProcedureSysObservationDetail.array = [];
        ProcedureSysObservationDetail.myArr = [];
        ProcedureSysObservationDetail.NewInsertedId = -1;
        ProcedureSysObservationDetail.selectedObservations = [];
        var ProcedureNotesObservationId = ProcedureSysObservationDetail.ProcedureNotesObservationId;
        ProcedureSysObservationDetail.UpdateObservationDesc(ProcedureNotesObservationId).done(function (response) {
            var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureSystem_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]);
            if (selectedNoteDesc) {
                if ($("#ProcedureobservationContent").val() != "") {
                    $(selectedNoteDesc).text($("#ProcedureobservationContent").val());
                    $(selectedNoteDesc).attr('style', $("#ProcedureobservationContent").attr('style'));
                }
                else {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureElement_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]).prev('br').remove();
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureElement_' + ProcedureSysObservationDetail.params["ProcedureTemplateSystemId"]).remove();
                }
                Clinical_ProgressNote.saveComponentSOAPText('Procedures');

                Clinical_ProgressNote.SaveAndAttachProcedureReport($('#PatientProfile #hfPatientId').val(), Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, true);

                //Clinical_ProgressNote.updateProgressNoteHTML();
                Clinical_ProgressNote.hoverFunction();
            }
            UnloadActionPan(ProcedureSysObservationDetail.params["ParentCtrl"], "ProcedureSysObservationDetail");
        });

    },
    PhysicalExamSystemObservationsLoad: function (ProcedureTemplateSystemId) {
        var objData = new Object();
        objData["ProcedureTemplateSystemId"] = ProcedureTemplateSystemId;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProcedureId"] = ProcedureSysObservationDetail.params.ProcedureId;
        objData["commandType"] = "Fill_Procedure_Observations_For_Notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureTemplate", "ProcedureTemplate");
    },
    applyStyle: function (style) {
        if (style == 'bold') {
            $("#ProcedureobservationContent").css('font-weight', 'bold');
        }
        else if (style == 'italic') {
            $("#ProcedureobservationContent").css('font-style', 'italic');
        }
        else if (style == 'underline') {
            $("#ProcedureobservationContent").css('text-decoration', 'underline');
        }
        else if (style == 'reset') {
            $("#ProcedureobservationContent").css('font-weight', 'normal');
            $("#ProcedureobservationContent").css('font-style', 'normal');
            $("#ProcedureobservationContent").css('text-decoration', 'none');
        }
        else if (style == 'clear') {
            $("#ProcedureobservationContent").text('');
            $("#ProcedureobservationContent").val('');
        }
    },
    UpdateObservationDesc: function (ProcedureNotesObservationId) {
        var dataObsUp = new Object();
        dataObsUp["ProcedureNotesObservationId"] = ProcedureNotesObservationId;
        dataObsUp["Descr"] = $("#ProcedureobservationContent").val();
        dataObsUp["commandType"] = "Update_Procedure_Selected_Observations_Desc_For_Notes";
        var dataO = JSON.stringify(dataObsUp);
        return MDVisionService.APIService(dataO, "ProcedureTemplate", "ProcedureTemplate");
    },
}