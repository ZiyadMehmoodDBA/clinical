ProcedureOrderSysObservationDetail = {
    bIsFirstLoad: true,
    params: [],
    array: [],
    myArr: [],
    NewInsertedId: -1,
    selectedObservations: [],
    ProcedureOrderNotesObservationId: null,
    Load: function (params) {
        BackgroundLoaderShow(true);
        ProcedureOrderSysObservationDetail.params = params;
        var isSelectedEntity = false
        var self = $('#' + ProcedureOrderSysObservationDetail.params.PanelID + ' #tblProcedureOrderSysObservationDetail');
        self.loadDropDowns(true).done(function () {
            if (ProcedureOrderSysObservationDetail.params["mode"] == "Edit") {
                ProcedureOrderSysObservationDetail.bindobservationAutoComplete();
                ProcedureOrderSysObservationDetail.loadObservations(ProcedureOrderSysObservationDetail.params.ProcedureTemplateSystemId);
            }
        });

    },
    switchNextPrevious: function (direction) {

        var templateArr = ProcedureOrderSysObservationDetail.params.templateIDs;
        var current = $('#' + ProcedureOrderSysObservationDetail.params.PanelID + " #frmProcedureOrderSysObservationDetail #hfTemplateId").val();
        var next = templateArr[($.inArray(current, templateArr) + 1) % templateArr.length];
        var prev = templateArr[($.inArray(current, templateArr) - 1 + templateArr.length) % templateArr.length];
        var dataChanged = EMRUtility.compareFormDataWithSerialized(ProcedureOrderSysObservationDetail.params.PanelID + ' #frmProcedureOrderSysObservationDetail');

        if (dataChanged == true) {

            ProcedureOrderSysObservationDetail.UpdateObservationDesc(ProcedureOrderSysObservationDetail.ProcedureOrderNotesObservationId).done(function (response) {
                var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderSystem_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]);
                if (selectedNoteDesc) {
                    var procSection = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderSystem_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).parents('section[id*="Cli_ProcedureOrderDetail_Main"]');

                    if ($("#ProcedureOrderobservationContent").val() != "") {
                        $(selectedNoteDesc).text($("#ProcedureOrderobservationContent").val());
                        $(selectedNoteDesc).attr('style', $("#ProcedureOrderobservationContent").attr('style'));
                    }
                    else {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderElement_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).prev('br').remove();
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderElement_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).remove();
                    }
                    Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder',true);

                    var procOrderId = $(procSection).map(function () {
                        return this.id.replace('Cli_ProcedureOrderDetail_Main', '');
                    }).get(0);

                    if (procOrderId.length > 0) {
                        var patdocId = $(procSection).attr('patdocid');
                        if (patdocId > 0) {
                            Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(patdocId).done(function (responseDoc) {
                                if (responseDoc.status != false) {
                                    Patient_Document.DeleteDocument(patdocId);
                                    Clinical_ProgressNote.SaveAndAttachOrderReport("Procedure Order", procOrderId, true);
                                }
                                else {
                                    utility.DisplayMessages(responseDoc.Message, 3);
                                }
                            });
                        }
                    }


                    //Clinical_ProgressNote.updateProgressNoteHTML();
                    Clinical_ProgressNote.hoverFunction();
                }
                if (direction == 'Next') {
                    ProcedureOrderSysObservationDetail.loadObservations(next);
                } else if (direction == 'Previous') {
                    ProcedureOrderSysObservationDetail.loadObservations(prev);
                }
            });

        } else {
            if (direction == 'Next') {



                for (var i in ProcedureOrderSysObservationDetail.params.templatesStyle) {
                    if (ProcedureOrderSysObservationDetail.params.templatesStyle[i][0] == ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"] && ProcedureOrderSysObservationDetail.params.templatesStyle[i][1] != $("#ProcedureOrderobservationContent").attr('style')) {
                        ProcedureOrderSysObservationDetail.params.templatesStyle[i][1] = $("#ProcedureOrderobservationContent").attr('style');

                        ProcedureOrderSysObservationDetail.UpdateObservationDesc(ProcedureOrderSysObservationDetail.ProcedureOrderNotesObservationId).done(function (response) {

                            var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderSystem_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]);
                            if (selectedNoteDesc) {
                                var procSection = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderSystem_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).parents('section[id*="Cli_ProcedureOrderDetail_Main"]');

                                if ($("#ProcedureOrderobservationContent").val() != "") {
                                    $(selectedNoteDesc).text($("#ProcedureOrderobservationContent").val());
                                    $(selectedNoteDesc).attr('style', $("#ProcedureOrderobservationContent").attr('style'));
                                }
                                else {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderElement_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).prev('br').remove();
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderElement_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).remove();
                                }
                                Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder',true);

                                var procOrderId = $(procSection).map(function () {
                                    return this.id.replace('Cli_ProcedureOrderDetail_Main', '');
                                }).get(0);

                                if (procOrderId != null && procOrderId.length > 0) {
                                    var patdocId = $(procSection).attr('patdocid');
                                    if (patdocId > 0) {
                                        Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(patdocId).done(function (responseDoc) {
                                            if (responseDoc.status != false) {
                                                Patient_Document.DeleteDocument(patdocId);
                                                Clinical_ProgressNote.SaveAndAttachOrderReport("Procedure Order", procOrderId, true);
                                            }
                                            else {
                                                utility.DisplayMessages(responseDoc.Message, 3);
                                            }
                                        });
                                    }
                                }


                                //Clinical_ProgressNote.updateProgressNoteHTML();
                                Clinical_ProgressNote.hoverFunction();
                            }

                        });


                    }
                }
                ProcedureOrderSysObservationDetail.loadObservations(next);


            } else if (direction == 'Previous') {

                for (var i in ProcedureOrderSysObservationDetail.params.templatesStyle) {
                    if (ProcedureOrderSysObservationDetail.params.templatesStyle[i][0] == ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"] && ProcedureOrderSysObservationDetail.params.templatesStyle[i][1] != $("#ProcedureOrderobservationContent").attr('style')) {
                        ProcedureOrderSysObservationDetail.params.templatesStyle[i][1] = $("#ProcedureOrderobservationContent").attr('style');

                        ProcedureOrderSysObservationDetail.UpdateObservationDesc(ProcedureOrderSysObservationDetail.ProcedureOrderNotesObservationId).done(function (response) {

                            var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderSystem_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]);
                            if (selectedNoteDesc) {
                                var procSection = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderSystem_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).parents('section[id*="Cli_ProcedureOrderDetail_Main"]');

                                if ($("#ProcedureOrderobservationContent").val() != "") {
                                    $(selectedNoteDesc).text($("#ProcedureOrderobservationContent").val());
                                    $(selectedNoteDesc).attr('style', $("#ProcedureOrderobservationContent").attr('style'));
                                }
                                else {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderElement_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).prev('br').remove();
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderElement_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).remove();
                                }
                                Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder');

                                var procOrderId = $(procSection).map(function () {
                                    return this.id.replace('Cli_ProcedureOrderDetail_Main', '');
                                }).get(0);

                                if (procOrderId != null && procOrderId.length > 0) {
                                    var patdocId = $(procSection).attr('patdocid');
                                    if (patdocId > 0) {
                                        Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(patdocId).done(function (responseDoc) {
                                            if (responseDoc.status != false) {
                                                Patient_Document.DeleteDocument(patdocId);
                                                Clinical_ProgressNote.SaveAndAttachOrderReport("Procedure Order", procOrderId, true);
                                            }
                                            else {
                                                utility.DisplayMessages(responseDoc.Message, 3);
                                            }
                                        });
                                    }
                                }


                                //Clinical_ProgressNote.updateProgressNoteHTML();
                                Clinical_ProgressNote.hoverFunction();
                            }

                        });


                    }
                }
                ProcedureOrderSysObservationDetail.loadObservations(prev);
            }
        }
    },
    bindobservationAutoComplete: function () {

        PhysicalExamTemplatesRevamp.lookupPEObservation_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Observation_JSON = JSON.parse(response.Observation_JSON);
                    $('#' + ProcedureOrderSysObservationDetail.params.PanelID + " #Observations").kendoAutoComplete({
                        dataSource: Observation_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Observation...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulProcedureOrderSystemSection").find('#' + dataItem.id).length == 0) {

                                var PESystemId = ProcedureOrderSysObservationDetail.params.PESystemId;
                                var ProcedureTemplateSystemId = ProcedureOrderSysObservationDetail.params.ProcedureTemplateSystemId;
                                if (PESystemId > 0 && ProcedureTemplateSystemId > 0) {
                                    ProcedureOrderSysObservationDetail.addObservationSystemAssosiation(dataItem.PEObservationId, PESystemId, dataItem.Name, ProcedureTemplateSystemId);
                                    $('#' + ProcedureOrderSysObservationDetail.params.PanelID + " #Observations").val('');
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

    addObservationSystemAssosiation: function (observationId, PESystemId, Name, ProcedureTemplateSystemId) {

        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["ProcedureTemplateSystemId"] = ProcedureTemplateSystemId;

        objData["ObservationId"] = observationId;
        if (!objData["ObservationId"] || !objData["PESystemId"]) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }
        ProcedureOrderSysObservationDetail.addObservationSystemAssosiation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                utility.DisplayMessages(response.Message, 1);
                if (observationId) {
                    $('#' + ProcedureOrderSysObservationDetail.params.PanelID + " #Observations").val('');
                    var objSelectedObservations = { PESystemId: PESystemId, IsSelected: true, ObservationId: observationId, ObservationName: Name, IsSystemChecked: true };
                    var li = ProcedureOrderSysObservationDetail.addObservations(observationId, Name, PESystemId);
                    $('#' + ProcedureOrderSysObservationDetail.params.PanelID + " #ulProcedureOrderSystemSection").append(li);
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
        var a = ProcedureOrderSysObservationDetail.selectedObservations;

        var itemtoRemove = "observation";

        var li = '<li id="' + PEObservationId + '" parentid="' + PESystemId + '" onclick="ProcedureOrderSysObservationDetail.PreviewObservations(' + PEObservationId + ',\'' + ObservatioName + '\', this, ' + PESystemId + ');" value="' + PEObservationId + '" refvalue="" subcharacteristicexist=" " class=""><div class="">' +
                 '<label id="lblName' + PEObservationId + '" class="" data-toggle="tooltip" title="" data-original-title="' + ObservatioName + '">' + ObservatioName + '</label><div id="divNameDetail' + PEObservationId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PEObservationId + '" onkeypress="" name="Name' + PEObservationId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PEObservationId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
        return li;
    },

    removeItem: function (PESystemId, control, PEObservationId) {
        if (control == "observation") {
            $("#ulProcedureOrderSystemSection #" + PEObservationId).remove();

            if (ProcedureOrderSysObservationDetail.selectedObservations[i].PESystemId == PESystemId && ProcedureOrderSysObservationDetail.selectedObservations[i].ObservationId == PEObservationId) {
                ProcedureOrderSysObservationDetail.selectedObservations[i].ObservationId = -1;
            }
        }
    },

    removeObservation: function (PESystemId, PEObservationId) {
        $("#ulProcedureOrderSystemSection #" + PEObservationId).remove();

        if (ProcedureOrderSysObservationDetail.selectedObservations) {
            for (var i = 0 ; i < ProcedureOrderSysObservationDetail.selectedObservations.length; i++) {
                if (ProcedureOrderSysObservationDetail.selectedObservations[i].PESystemId == PESystemId && ProcedureOrderSysObservationDetail.selectedObservations[i].ObservationId == PEObservationId) {
                    ProcedureOrderSysObservationDetail.selectedObservations[i].ObservationId = -1;
                }
            }
        }
    },

    loadObservations: function (ProcedureTemplateSystemId) {
        ProcedureOrderSysObservationDetail.PhysicalExamSystemObservationsLoad(ProcedureTemplateSystemId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $('#' + ProcedureOrderSysObservationDetail.params.PanelID + " #frmProcedureOrderSysObservationDetail #hfTemplateId").val(ProcedureTemplateSystemId);
                ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"] = ProcedureTemplateSystemId;

                if (response.ProcedureOrderObservationCount > 0) {
                    var res = JSON.parse(response.ProcedureOrderObservation_JSON);
                    var resSystems = JSON.parse(res);
                    $("#ProcedureOrderSystemSections").removeClass('hidden');
                    $("#ulProcedureOrderSystemSection li").remove();
                    $.each(resSystems, function (i, item) {
                        var li = ProcedureOrderSysObservationDetail.addObservations(item.PEObservationId, item.ObservationName, item.PESystemId);
                        ProcedureOrderSysObservationDetail.ProcedureOrderNotesObservationId = item.PEObservationId;
                        $("#ulProcedureOrderSystemSection").append(li);
                        var objSelectedObservations = { PESystemId: item.PESystemId, IsChecked: false, ObservationId: item.PEObservationId, ObservationName: item.ObservationName, IsSystemChecked: false };
                        ProcedureOrderSysObservationDetail.selectedObservations.push(objSelectedObservations);
                        $("#ProcedureOrderobservationContent").val(item.SystemDescription);
                        $("#ProcedureOrderobservationContent").text(item.SystemDescription);
                        $.each(ProcedureOrderSysObservationDetail.params.templatesStyle, function (ind, vl) {

                            if (vl[0] == ProcedureTemplateSystemId) {
                                if (vl[1] == undefined || vl[1] == "") {
                                    vl[1] = 'font-weight: normal; font-style: normal; text-decoration: none';
                                }
                                $("#ProcedureOrderobservationContent").attr('style', vl[1]);
                            }

                        });
                       // $("#ProcedureOrderobservationContent").attr('style', ProcedureOrderSysObservationDetail.params["Content_style"]);
                        ProcedureOrderSysObservationDetail.ProcedureOrderNotesObservationId = item.ProcedureOrderNotesObservationId;
                        $('#' + ProcedureOrderSysObservationDetail.params.PanelID + ' #headingTitle').text('ProcedureOrder Template - ' + item.SystemName)
                    });
                    $('#' + ProcedureOrderSysObservationDetail.params.PanelID + " #frmProcedureOrderSysObservationDetail").data('serialize', $('#' + ProcedureOrderSysObservationDetail.params.PanelID + " #frmProcedureOrderSysObservationDetail").serialize());
                }
            }
        });
    },

    selectAllChars: function (obj) {
        if (obj) var sysId = $($($(obj).parent().parent())[0]).attr('parentid');

        if ($(obj).prop('checked') == true) {


            $("#SystemPreview").removeClass('hidden');
            $('#' + ProcedureOrderSysObservationDetail.params.PanelID + ' #ulProcedureOrderSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);

                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3)
                    var observationName = $("#divProcedureOrderSystemSection #lblName" + id_).text();
                    var delimator = $("#delimator option:selected").text() + " ";

                    $("#ProcedureOrderobservationContent").text($("#ProcedureOrderobservationContent").text() + observationName + delimator);

                    var objSelectedObservations = { PESystemId: sysId, IsChecked: true, ObservationId: id_, ObservationName: observationName, IsModified: '1' };
                    ProcedureOrderSysObservationDetail.selectedObservations.push(objSelectedObservations);

                    $("#ulPhysicalExamSystems #chk" + sysId).prop("checked", true);
                }
            });
            //var objSelectedAll = { PESystemId: sysId, IsChecked: true, ObservationId: 'chkboxSelectAllObservations', ObservationName: 'Select All', IsModified: '1' };
            //ProcedureOrderSysObservationDetail.selectedObservations.push(objSelectedAll);

            if (ProcedureOrderSysObservationDetail.selectedObservations) {
                for (var i = 0 ; i < ProcedureOrderSysObservationDetail.selectedObservations.length; i++) {
                    if (ProcedureOrderSysObservationDetail.selectedObservations[i].PESystemId == sysId) {
                        ProcedureOrderSysObservationDetail.selectedObservations[i].IsChecked = true;
                        ProcedureOrderSysObservationDetail.selectedObservations[i].IsSystemChecked = true;
                    }
                }
            }

        }
        else if ($(obj).prop('checked') == false) {
            $('#' + ProcedureOrderSysObservationDetail.params.PanelID + ' #ulProcedureOrderSystemSection li .char').removeClass("green");
            $('#' + ProcedureOrderSysObservationDetail.params.PanelID + ' #ulProcedureOrderSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                }
            });

            if (ProcedureOrderSysObservationDetail.selectedObservations) {
                for (var i = 0 ; i < ProcedureOrderSysObservationDetail.selectedObservations.length; i++) {
                    if (ProcedureOrderSysObservationDetail.selectedObservations[i].PESystemId == sysId) {
                        ProcedureOrderSysObservationDetail.selectedObservations[i].IsChecked = false;
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

        if ($("#ProcedureOrderobservationContent").val())
            $("#ProcedureOrderobservationContent").val($("#ProcedureOrderobservationContent").val() + deli + ObservationName);
        else
            $("#ProcedureOrderobservationContent").val($("#ProcedureOrderobservationContent").val() + ObservationName);
        ProcedureOrderSysObservationDetail.selectedObservations.push(objSelectedObservations);

        if (ProcedureOrderSysObservationDetail.selectedObservations) {
            for (var i = 0 ; i < ProcedureOrderSysObservationDetail.selectedObservations.length; i++) {
                if (ProcedureOrderSysObservationDetail.selectedObservations[i].PESystemId == PESystemId && ProcedureOrderSysObservationDetail.selectedObservations[i].ObservationId == observationId) {
                    ProcedureOrderSysObservationDetail.selectedObservations[i].IsChecked = true;
                    ProcedureOrderSysObservationDetail.selectedObservations[i].IsSystemChecked = true;
                }
                if (ProcedureOrderSysObservationDetail.selectedObservations[i].PESystemId == PESystemId)
                    ProcedureOrderSysObservationDetail.selectedObservations[i].IsSystemChecked = true;
            }
        }
        $("#ulPhysicalExamSystems #chk" + PESystemId).prop("checked", true);
    },

    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + ProcedureOrderSysObservationDetail.params["PanelID"];
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
                var $Item = $(ProcedureOrderSysObservationDetail.selectedListItem);
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
            currentId = ProcedureOrderSysObservationDetail.NewInsertedId--;
            var subcharacteristicExist = "";

            var saveMethodPE = "";

            if (itemType.toLowerCase() == "system") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + ProcedureOrderSysObservationDetail.params.PanelID + " #frmProcedureOrderSysObservationDetail #" + currentCtrlId);
                saveMethodPE = "ProcedureOrderSysObservationDetail.AddSystemPE(this, '" + currentId + "');";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "";
                currentCtrlId = "ulProcedureOrderSystemSection";
                ulControl = $('#' + ProcedureOrderSysObservationDetail.params.PanelID + " #frmProcedureOrderSysObservationDetail #" + currentCtrlId);
                saveMethodPE = "ProcedureOrderSysObservationDetail.AddObservation(this, '" + currentId + "');";
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
                onClick = "";//"ProcedureOrderSysObservationDetail.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";
                var deleteFunction = "ProcedureOrderSysObservationDetail.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                liInnerText = '<div class="">' + deleteIcon + addSubCharIcon
                    //+ '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="ProcedureOrderSysObservationDetail.selectParentControls(this);ProcedureOrderSysObservationDetail.toggleCheckBoxes(this);">'
                    + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                    + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                    + '<textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress="ProcedureOrderSysObservationDetail.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
                    + '<div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodPE + '" class="btn btn-link btn-xs">'
                    + '<i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';

                var liTobeAdded = '<li id="' + currentId + '" ' + currentLiClass + ' parentid="' + currentParentId + '" onclick="' + onClick + '" value="' + currentId + '" refValue="' + currentId + '"' + subcharacteristicExist + '>' + liInnerText + '</li>';

                ulControl.prepend(liTobeAdded);
                ulControl.find('li#' + currentId + ' #txtName' + currentId).focus()

            }

        }
    },

    AddObservation: function (obj, controlId) {
        var PESystemId = ProcedureOrderSysObservationDetail.params.PESystemId;

        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = 1;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        ProcedureOrderSysObservationDetail.savePEObservation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    ProcedureOrderSysObservationDetail.params.PEObservationId = response.PEObservationId;
                    var li = ProcedureOrderSysObservationDetail.addObservations(response.PEObservationId, objData["Name"], PESystemId);
                    $("#ulProcedureOrderSystemSection").append(li);

                    var objSelectedObservations = { PESystemId: PESystemId, IsChecked: false, ObservationId: response.PEObservationId, ObservationName: objData["Name"], IsSystemChecked: true };
                    ProcedureOrderSysObservationDetail.selectedObservations.push(objSelectedObservations);
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

        } else if (ctrlId == "ulProcedureOrderSystemSection") {
            $("#" + currentId).remove();
        }
    },

    validateSelectedTemplateData: function () {

        var isValid = true;

        if ($(ProcedureOrderSysObservationDetail.selectedPhyExamTempData).length > 0) {
            $.each(ProcedureOrderSysObservationDetail.selectedPhyExamTempData, function (i, item) {
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
        ProcedureOrderSysObservationDetail.array = [];
        ProcedureOrderSysObservationDetail.myArr = [];
        ProcedureOrderSysObservationDetail.NewInsertedId = -1;
        ProcedureOrderSysObservationDetail.selectedObservations = [];
        var ProcedureOrderNotesObservationId = ProcedureOrderSysObservationDetail.ProcedureOrderNotesObservationId;
        ProcedureOrderSysObservationDetail.UpdateObservationDesc(ProcedureOrderNotesObservationId).done(function (response) {
            var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderSystem_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]);
            if (selectedNoteDesc) {
                var procSection = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderSystem_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).parents('section[id*="Cli_ProcedureOrderDetail_Main"]');

                if ($("#ProcedureOrderobservationContent").val() != "") {
                    $(selectedNoteDesc).text($("#ProcedureOrderobservationContent").val());
                    $(selectedNoteDesc).attr('style', $("#ProcedureOrderobservationContent").attr('style'));
                }
                else {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderElement_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).prev('br').remove();
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProcedureOrderElement_' + ProcedureOrderSysObservationDetail.params["ProcedureTemplateSystemId"]).remove();
                }
                Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder');

                var procOrderId = $(procSection).map(function () {
                    return this.id.replace('Cli_ProcedureOrderDetail_Main', '');
                }).get(0);

                if (procOrderId != null && procOrderId.length > 0) {
                    var patdocId = $(procSection).attr('patdocid');
                    if (patdocId > 0) {
                        Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(patdocId).done(function (responseDoc) {
                            if (responseDoc.status != false) {
                                Patient_Document.DeleteDocument(patdocId);
                                Clinical_ProgressNote.SaveAndAttachOrderReport("Procedure Order", procOrderId, true);
                            }
                            else {
                                utility.DisplayMessages(responseDoc.Message, 3);
                            }
                        });
                    }
                }


                //Clinical_ProgressNote.updateProgressNoteHTML();
                Clinical_ProgressNote.hoverFunction();
            }
            UnloadActionPan(ProcedureOrderSysObservationDetail.params["ParentCtrl"], "ProcedureOrderSysObservationDetail");
        });

    },
    PhysicalExamSystemObservationsLoad: function (ProcedureTemplateSystemId) {
        var objData = new Object();
        objData["ProcedureTemplateSystemId"] = ProcedureTemplateSystemId;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProcedureOrderId"] = ProcedureOrderSysObservationDetail.params.ProcedureOrderId;
        objData["commandType"] = "Fill_ProcedureOrder_Observations_For_Notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureTemplate", "ProcedureTemplate");
    },
    applyStyle: function (style) {
        if (style == 'bold') {
            $("#ProcedureOrderobservationContent").css('font-weight', 'bold');
        }
        else if (style == 'italic') {
            $("#ProcedureOrderobservationContent").css('font-style', 'italic');
        }
        else if (style == 'underline') {
            $("#ProcedureOrderobservationContent").css('text-decoration', 'underline');
        }
        else if (style == 'reset') {
            $("#ProcedureOrderobservationContent").css('font-weight', 'normal');
            $("#ProcedureOrderobservationContent").css('font-style', 'normal');
            $("#ProcedureOrderobservationContent").css('text-decoration', 'none');
        }
        else if (style == 'clear') {
            $("#ProcedureOrderobservationContent").val('');
            $("#ProcedureOrderobservationContent").text('');
        }
    },
    UpdateObservationDesc: function (ProcedureOrderNotesObservationId) {
        var dataObsUp = new Object();
        dataObsUp["ProcedureOrderNotesObservationId"] = ProcedureOrderNotesObservationId;
        dataObsUp["Descr"] = $("#ProcedureOrderobservationContent").val();
        dataObsUp["commandType"] = "Update_ProcedureOrder_Selected_Observations_Desc_For_Notes";
        var dataO = JSON.stringify(dataObsUp);
        return MDVisionService.APIService(dataO, "ProcedureTemplate", "ProcedureTemplate");
    },
}