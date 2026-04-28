AOESysRadiologyObservationDetail = {
    bIsFirstLoad: true,
    params: [],
    array: [],
    myArr: [],
    NewInsertedId: -1,
    selectedObservations: [],
    AOENotesRadiologyObservationId: null,
    Load: function (params) {
        BackgroundLoaderShow(true);
        AOESysRadiologyObservationDetail.params = params;
        var isSelectedEntity = false
        var self = $('#' + AOESysRadiologyObservationDetail.params.PanelID + ' #tblAOESysRadiologyObservationDetail');
        self.loadDropDowns(true).done(function () {
            if (AOESysRadiologyObservationDetail.params["mode"] == "Edit") {
                AOESysRadiologyObservationDetail.bindobservationAutoComplete();
                AOESysRadiologyObservationDetail.loadObservations(AOESysRadiologyObservationDetail.params.AOETemplateSystemId);
            }
        });

    },
    switchNextPrevious: function (direction) {

        var templateArr = AOESysRadiologyObservationDetail.params.templateIDs;
        var current = $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #frmAOESysRadiologyObservationDetail #hfTemplateId").val();
        var next = templateArr[($.inArray(current, templateArr) + 1) % templateArr.length];
        var prev = templateArr[($.inArray(current, templateArr) - 1 + templateArr.length) % templateArr.length];
        var dataChanged = EMRUtility.compareFormDataWithSerialized(AOESysRadiologyObservationDetail.params.PanelID + ' #frmAOESysRadiologyObservationDetail');

        if (dataChanged == true) {

            AOESysRadiologyObservationDetail.UpdateObservationDesc(AOESysRadiologyObservationDetail.AOENotesRadiologyObservationId).done(function (response) {

                var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).length == 0
                                        ? $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .DiagnosticImagingOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"])
                                        : $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]);

                if (selectedNoteDesc) {

                    var radSection = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).length == 0
                                      ? $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .DiagnosticImagingOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).parents('section[id*="Cli_RadiologyOrderDetail_Main"]')
                                      : $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).parents('section[id*="Cli_RadiologyOrderDetail_Main"]');

                    if ($("#AOEobservationContent").val() != "") {
                        $(selectedNoteDesc).text($("#AOEobservationContent").val());
                        $(selectedNoteDesc).attr('style', $("#AOEobservationContent").attr('style'));
                    }
                    else {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).prev().remove();
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).remove();
                    }
                    Clinical_ProgressNote.saveComponentSOAPText('RadiologyOrder', true);

                    var radiologyOrderId = $(radSection).map(function () {
                        return this.id.replace('Cli_RadiologyOrderDetail_Main', '');
                    }).get(0);

                    if (radSection.length > 0) {
                        var patdocId = $(radSection).attr('patdocid');
                        if (patdocId > 0) {
                            Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(patdocId).done(function (responseDoc) {
                                if (responseDoc.status != false) {
                                    Patient_Document.DeleteDocument(patdocId);
                                    Clinical_ProgressNote.SaveAndAttachOrderReport("Radiology Order", radiologyOrderId, true);
                                }
                                else {
                                    utility.DisplayMessages(responseDoc.Message, 3);
                                }
                            });
                        }
                    }
                    Clinical_ProgressNote.hoverFunction();
                }
                if (direction == 'Next') {
                    AOESysRadiologyObservationDetail.loadObservations(next);
                } else if (direction == 'Previous') {
                    AOESysRadiologyObservationDetail.loadObservations(prev);
                }
            });

        } else {
            if (direction == 'Next') {



                for (var i in AOESysRadiologyObservationDetail.params.templatesStyle) {
                    if (AOESysRadiologyObservationDetail.params.templatesStyle[i][0] == AOESysRadiologyObservationDetail.params.AOETemplateSystemId && AOESysRadiologyObservationDetail.params.templatesStyle[i][1] != $("#AOEobservationContent").attr('style')) {
                        AOESysRadiologyObservationDetail.params.templatesStyle[i][1] = $("#AOEobservationContent").attr('style');

                        AOESysRadiologyObservationDetail.UpdateObservationDesc(AOESysRadiologyObservationDetail.AOENotesRadiologyObservationId).done(function (response) {
                            var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).length == 0
                                                    ? $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .DiagnosticImagingOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"])
                                                    : $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]);

                            if (selectedNoteDesc) {

                                var radSection = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).length == 0
                                                  ? $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .DiagnosticImagingOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).parents('section[id*="Cli_RadiologyOrderDetail_Main"]')
                                                  : $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).parents('section[id*="Cli_RadiologyOrderDetail_Main"]');

                                if ($("#AOEobservationContent").val() != "") {
                                    $(selectedNoteDesc).text($("#AOEobservationContent").val());
                                    $(selectedNoteDesc).attr('style', $("#AOEobservationContent").attr('style'));
                                }
                                else {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).prev().remove();
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).remove();
                                }
                                Clinical_ProgressNote.saveComponentSOAPText('RadiologyOrder', true);

                                var radiologyOrderId = $(radSection).map(function () {
                                    return this.id.replace('Cli_RadiologyOrderDetail_Main', '');
                                }).get(0);

                                if (radSection.length > 0) {
                                    var patdocId = $(radSection).attr('patdocid');
                                    if (patdocId > 0) {
                                        Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(patdocId).done(function (responseDoc) {
                                            if (responseDoc.status != false) {
                                                Patient_Document.DeleteDocument(patdocId);
                                                Clinical_ProgressNote.SaveAndAttachOrderReport("Radiology Order", radiologyOrderId, true);
                                            }
                                            else {
                                                utility.DisplayMessages(responseDoc.Message, 3);
                                            }
                                        });
                                    }
                                }
                                Clinical_ProgressNote.hoverFunction();
                            }

                        });


                    }
                }
                AOESysRadiologyObservationDetail.loadObservations(next);


            } else if (direction == 'Previous') {

                for (var i in AOESysRadiologyObservationDetail.params.templatesStyle) {
                    if (AOESysRadiologyObservationDetail.params.templatesStyle[i][0] == AOESysRadiologyObservationDetail.params.AOETemplateSystemId && AOESysRadiologyObservationDetail.params.templatesStyle[i][1] != $("#AOEobservationContent").attr('style')) {
                        AOESysRadiologyObservationDetail.params.templatesStyle[i][1] = $("#AOEobservationContent").attr('style');

                        AOESysRadiologyObservationDetail.UpdateObservationDesc(AOESysRadiologyObservationDetail.AOENotesRadiologyObservationId).done(function (response) {

                            var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).length == 0
                                                    ? $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .DiagnosticImagingOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"])
                                                    : $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]);


                            if (selectedNoteDesc) {

                                var radSection = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).length == 0
                                                  ? $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .DiagnosticImagingOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).parents('section[id*="Cli_RadiologyOrderDetail_Main"]')
                                                  : $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).parents('section[id*="Cli_RadiologyOrderDetail_Main"]');

                                if ($("#AOEobservationContent").val() != "") {
                                    $(selectedNoteDesc).text($("#AOEobservationContent").val());
                                    $(selectedNoteDesc).attr('style', $("#AOEobservationContent").attr('style'));
                                }
                                else {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).prev().remove();
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).remove();
                                }
                                Clinical_ProgressNote.saveComponentSOAPText('RadiologyOrder', true);

                                var radiologyOrderId = $(radSection).map(function () {
                                    return this.id.replace('Cli_RadiologyOrderDetail_Main', '');
                                }).get(0);

                                if (radSection.length > 0) {
                                    var patdocId = $(radSection).attr('patdocid');
                                    if (patdocId > 0) {
                                        Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(patdocId).done(function (responseDoc) {
                                            if (responseDoc.status != false) {
                                                Patient_Document.DeleteDocument(patdocId);
                                                Clinical_ProgressNote.SaveAndAttachOrderReport("Radiology Order", radiologyOrderId, true);
                                            }
                                            else {
                                                utility.DisplayMessages(responseDoc.Message, 3);
                                            }
                                        });
                                    }
                                }
                                Clinical_ProgressNote.hoverFunction();
                            }

                        });


                    }
                }
                AOESysRadiologyObservationDetail.loadObservations(prev);
            }
        }

        //Math.max.apply(Math, _array); // 3
        //Math.min.apply(Math, _array);
    },
    bindobservationAutoComplete: function () {

        PhysicalExamTemplatesRevamp.lookupPEObservation_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Observation_JSON = JSON.parse(response.Observation_JSON);
                    $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #Observations").kendoAutoComplete({
                        dataSource: Observation_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Observation...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulAOESystemSection").find('#' + dataItem.id).length == 0) {

                                var PESystemId = AOESysRadiologyObservationDetail.params.PESystemId;
                                var AOETemplateSystemId = AOESysRadiologyObservationDetail.params.AOETemplateSystemId;
                                if (PESystemId > 0 && AOETemplateSystemId > 0) {
                                    AOESysRadiologyObservationDetail.addObservationSystemAssosiation(dataItem.PEObservationId, PESystemId, dataItem.Name, AOETemplateSystemId);
                                    $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #Observations").val('');
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

    addObservationSystemAssosiation: function (observationId, PESystemId, Name, AOETemplateSystemId) {

        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["AOETemplateSystemId"] = AOETemplateSystemId;

        objData["ObservationId"] = observationId;
        if (!objData["ObservationId"] || !objData["PESystemId"]) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }
        AOESysRadiologyObservationDetail.addObservationSystemAssosiation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                utility.DisplayMessages(response.Message, 1);
                if (observationId) {
                    $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #Observations").val('');
                    var objSelectedObservations = { PESystemId: PESystemId, IsSelected: true, ObservationId: observationId, ObservationName: Name, IsSystemChecked: true };
                    var li = AOESysRadiologyObservationDetail.addObservations(observationId, Name, PESystemId);
                    $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #ulAOESystemSection").append(li);
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
        return MDVisionService.APIService(data, "AOETemplate", "AOETemplate");
    },
    addObservations: function (PEObservationId, ObservatioName, PESystemId) {
        var a = AOESysRadiologyObservationDetail.selectedObservations;

        var itemtoRemove = "observation";

        var li = '<li id="' + PEObservationId + '" parentid="' + PESystemId + '" onclick="AOESysRadiologyObservationDetail.PreviewObservations(' + PEObservationId + ',\'' + ObservatioName + '\', this, ' + PESystemId + ');" value="' + PEObservationId + '" refvalue="" subcharacteristicexist=" " class=""><div class="">' +
                 '<label id="lblName' + PEObservationId + '" class="" data-toggle="tooltip" title="" data-original-title="' + ObservatioName + '">' + ObservatioName + '</label><div id="divNameDetail' + PEObservationId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PEObservationId + '" onkeypress="" name="Name' + PEObservationId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PEObservationId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
        return li;
    },

    removeItem: function (PESystemId, control, PEObservationId) {
        if (control == "observation") {
            $("#ulAOESystemSection #" + PEObservationId).remove();

            if (AOESysRadiologyObservationDetail.selectedObservations[i].PESystemId == PESystemId && AOESysRadiologyObservationDetail.selectedObservations[i].ObservationId == PEObservationId) {
                AOESysRadiologyObservationDetail.selectedObservations[i].ObservationId = -1;
            }
        }
    },

    removeObservation: function (PESystemId, PEObservationId) {
        $("#ulAOESystemSection #" + PEObservationId).remove();

        if (AOESysRadiologyObservationDetail.selectedObservations) {
            for (var i = 0 ; i < AOESysRadiologyObservationDetail.selectedObservations.length; i++) {
                if (AOESysRadiologyObservationDetail.selectedObservations[i].PESystemId == PESystemId && AOESysRadiologyObservationDetail.selectedObservations[i].ObservationId == PEObservationId) {
                    AOESysRadiologyObservationDetail.selectedObservations[i].ObservationId = -1;
                }
            }
        }
    },

    loadObservations: function (AOETemplateSystemId) {
        AOESysRadiologyObservationDetail.PhysicalExamSystemObservationsLoad(AOETemplateSystemId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #frmAOESysRadiologyObservationDetail #hfTemplateId").val(AOETemplateSystemId);
                AOESysRadiologyObservationDetail.params["AOETemplateSystemId"] = AOETemplateSystemId;
                if (response.AOEObservationCount > 0) {
                    var res = JSON.parse(response.AOEObservation_JSON);
                    var resSystems = JSON.parse(res);
                    $("#AOESystemSections").removeClass('hidden');
                    $("#ulAOESystemSection li").remove();
                    $.each(resSystems, function (i, item) {
                        var li = AOESysRadiologyObservationDetail.addObservations(item.PEObservationId, item.ObservationName, item.PESystemId);
                        AOESysRadiologyObservationDetail.AOENotesRadiologyObservationId = item.PEObservationId;
                        $("#ulAOESystemSection").append(li);
                        var objSelectedObservations = { PESystemId: item.PESystemId, IsChecked: false, ObservationId: item.PEObservationId, ObservationName: item.ObservationName, IsSystemChecked: false };
                        AOESysRadiologyObservationDetail.selectedObservations.push(objSelectedObservations);
                        $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #frmAOESysRadiologyObservationDetail #AOEobservationContent").val(utility.decodeHtml(item.SystemDescription));
                        $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #frmAOESysRadiologyObservationDetail #AOEobservationContent").text(utility.decodeHtml(item.SystemDescription));
                        $.each(AOESysRadiologyObservationDetail.params.templatesStyle, function (ind, vl) {

                            if (vl[0] == AOETemplateSystemId) {
                                if (vl[1] == undefined || vl[1] == "") {
                                    vl[1] = 'font-weight: normal; font-style: normal; text-decoration: none';
                                }
                                $("#AOEobservationContent").attr('style', vl[1]);
                            }

                        });
                        //  $("#AOEobservationContent").attr('style', AOESysRadiologyObservationDetail.params["Content_style"]);
                        AOESysRadiologyObservationDetail.AOENotesRadiologyObservationId = item.AOENotesRadiologyObservationId;
                        $('#' + AOESysRadiologyObservationDetail.params.PanelID + ' #headingTitle').text('AOE Template - ' + item.SystemName)
                    });

                    $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #frmAOESysRadiologyObservationDetail").data('serialize', $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #frmAOESysRadiologyObservationDetail").serialize());

                }
            }
        });
    },

    selectAllChars: function (obj) {
        if (obj) var sysId = $($($(obj).parent().parent())[0]).attr('parentid');

        if ($(obj).prop('checked') == true) {


            $("#SystemPreview").removeClass('hidden');
            $('#' + AOESysRadiologyObservationDetail.params.PanelID + ' #ulAOESystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);

                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3)
                    var observationName = $("#divAOESystemSection #lblName" + id_).text();
                    var delimator = $("#delimator option:selected").text() + " ";

                    $("#AOEobservationContent").text(utility.decodeHtml($("#AOEobservationContent").text() + observationName + delimator));

                    var objSelectedObservations = { PESystemId: sysId, IsChecked: true, ObservationId: id_, ObservationName: observationName, IsModified: '1' };
                    AOESysRadiologyObservationDetail.selectedObservations.push(objSelectedObservations);

                    $("#ulPhysicalExamSystems #chk" + sysId).prop("checked", true);
                }
            });
            //var objSelectedAll = { PESystemId: sysId, IsChecked: true, ObservationId: 'chkboxSelectAllObservations', ObservationName: 'Select All', IsModified: '1' };
            //AOESysRadiologyObservationDetail.selectedObservations.push(objSelectedAll);

            if (AOESysRadiologyObservationDetail.selectedObservations) {
                for (var i = 0 ; i < AOESysRadiologyObservationDetail.selectedObservations.length; i++) {
                    if (AOESysRadiologyObservationDetail.selectedObservations[i].PESystemId == sysId) {
                        AOESysRadiologyObservationDetail.selectedObservations[i].IsChecked = true;
                        AOESysRadiologyObservationDetail.selectedObservations[i].IsSystemChecked = true;
                    }
                }
            }

        }
        else if ($(obj).prop('checked') == false) {
            $('#' + AOESysRadiologyObservationDetail.params.PanelID + ' #ulAOESystemSection li .char').removeClass("green");
            $('#' + AOESysRadiologyObservationDetail.params.PanelID + ' #ulAOESystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                }
            });

            if (AOESysRadiologyObservationDetail.selectedObservations) {
                for (var i = 0 ; i < AOESysRadiologyObservationDetail.selectedObservations.length; i++) {
                    if (AOESysRadiologyObservationDetail.selectedObservations[i].PESystemId == sysId) {
                        AOESysRadiologyObservationDetail.selectedObservations[i].IsChecked = false;
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

        if ($("#AOEobservationContent").val())
            $("#AOEobservationContent").val(utility.decodeHtml($("#AOEobservationContent").val() + deli + ObservationName));
        else
            $("#AOEobservationContent").val(utility.decodeHtml($("#AOEobservationContent").val() + ObservationName));
        AOESysRadiologyObservationDetail.selectedObservations.push(objSelectedObservations);

        if (AOESysRadiologyObservationDetail.selectedObservations) {
            for (var i = 0 ; i < AOESysRadiologyObservationDetail.selectedObservations.length; i++) {
                if (AOESysRadiologyObservationDetail.selectedObservations[i].PESystemId == PESystemId && AOESysRadiologyObservationDetail.selectedObservations[i].ObservationId == observationId) {
                    AOESysRadiologyObservationDetail.selectedObservations[i].IsChecked = true;
                    AOESysRadiologyObservationDetail.selectedObservations[i].IsSystemChecked = true;
                }
                if (AOESysRadiologyObservationDetail.selectedObservations[i].PESystemId == PESystemId)
                    AOESysRadiologyObservationDetail.selectedObservations[i].IsSystemChecked = true;
            }
        }
        $("#ulPhysicalExamSystems #chk" + PESystemId).prop("checked", true);
    },

    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + AOESysRadiologyObservationDetail.params["PanelID"];
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
                var $Item = $(AOESysRadiologyObservationDetail.selectedListItem);
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
            currentId = AOESysRadiologyObservationDetail.NewInsertedId--;
            var subcharacteristicExist = "";

            var saveMethodPE = "";

            if (itemType.toLowerCase() == "system") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #frmAOESysRadiologyObservationDetail #" + currentCtrlId);
                saveMethodPE = "AOESysRadiologyObservationDetail.AddSystemPE(this, '" + currentId + "');";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "";
                currentCtrlId = "ulAOESystemSection";
                ulControl = $('#' + AOESysRadiologyObservationDetail.params.PanelID + " #frmAOESysRadiologyObservationDetail #" + currentCtrlId);
                saveMethodPE = "AOESysRadiologyObservationDetail.AddObservation(this, '" + currentId + "');";
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
                onClick = "";//"AOESysRadiologyObservationDetail.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";
                var deleteFunction = "AOESysRadiologyObservationDetail.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                liInnerText = '<div class="">' + deleteIcon + addSubCharIcon
                    //+ '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="AOESysRadiologyObservationDetail.selectParentControls(this);AOESysRadiologyObservationDetail.toggleCheckBoxes(this);">'
                    + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                    + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                    + '<textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress="AOESysRadiologyObservationDetail.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
                    + '<div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodPE + '" class="btn btn-link btn-xs">'
                    + '<i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';

                var liTobeAdded = '<li id="' + currentId + '" ' + currentLiClass + ' parentid="' + currentParentId + '" onclick="' + onClick + '" value="' + currentId + '" refValue="' + currentId + '"' + subcharacteristicExist + '>' + liInnerText + '</li>';

                ulControl.prepend(liTobeAdded);
                ulControl.find('li#' + currentId + ' #txtName' + currentId).focus()

            }

        }
    },

    AddObservation: function (obj, controlId) {
        var PESystemId = AOESysRadiologyObservationDetail.params.PESystemId;

        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = 1;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        AOESysRadiologyObservationDetail.savePEObservation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    AOESysRadiologyObservationDetail.params.PEObservationId = response.PEObservationId;
                    var li = AOESysRadiologyObservationDetail.addObservations(response.PEObservationId, objData["Name"], PESystemId);
                    $("#ulAOESystemSection").append(li);

                    var objSelectedObservations = { PESystemId: PESystemId, IsChecked: false, ObservationId: response.PEObservationId, ObservationName: objData["Name"], IsSystemChecked: true };
                    AOESysRadiologyObservationDetail.selectedObservations.push(objSelectedObservations);
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

        } else if (ctrlId == "ulAOESystemSection") {
            $("#" + currentId).remove();
        }
    },

    validateSelectedTemplateData: function () {

        var isValid = true;

        if ($(AOESysRadiologyObservationDetail.selectedPhyExamTempData).length > 0) {
            $.each(AOESysRadiologyObservationDetail.selectedPhyExamTempData, function (i, item) {
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
        AOESysRadiologyObservationDetail.array = [];
        AOESysRadiologyObservationDetail.myArr = [];
        AOESysRadiologyObservationDetail.NewInsertedId = -1;
        AOESysRadiologyObservationDetail.selectedObservations = [];
        var AOENotesRadiologyObservationId = AOESysRadiologyObservationDetail.AOENotesRadiologyObservationId;
        AOESysRadiologyObservationDetail.UpdateObservationDesc(AOENotesRadiologyObservationId).done(function (response) {
            var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).length == 0
                                    ? $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .DiagnosticImagingOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"])
                                    : $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOESystem_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]);
             
            if (selectedNoteDesc) {
               
                var radSection = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).length == 0
                                  ? $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .DiagnosticImagingOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).parents('section[id*="Cli_RadiologyOrderDetail_Main"]')
                                  : $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrderComponent #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).parents('section[id*="Cli_RadiologyOrderDetail_Main"]');

                if ($("#AOEobservationContent").val() != "") {
                    $(selectedNoteDesc).text($("#AOEobservationContent").val());
                    $(selectedNoteDesc).attr('style', $("#AOEobservationContent").attr('style'));
                }
                else {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).prev().remove();
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #AOEElement_' + AOESysRadiologyObservationDetail.params["AOETemplateSystemId"]).remove();
                }
                Clinical_ProgressNote.saveComponentSOAPText('RadiologyOrder');

                var radiologyOrderId = $(radSection).map(function () {
                    return this.id.replace('Cli_RadiologyOrderDetail_Main', '');
                }).get(0);

                if (radSection.length > 0) {
                    var patdocId = $(radSection).attr('patdocid');
                    if (patdocId > 0) {
                        Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(patdocId).done(function (responseDoc) {
                            if (responseDoc.status != false) {
                                Patient_Document.DeleteDocument(patdocId);
                                Clinical_ProgressNote.SaveAndAttachOrderReport("Radiology Order", radiologyOrderId, true);
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
            UnloadActionPan(AOESysRadiologyObservationDetail.params["ParentCtrl"], "AOESysRadiologyObservationDetail");
        });

    },
    PhysicalExamSystemObservationsLoad: function (AOETemplateSystemId) {
        var objData = new Object();
        objData["AOETemplateSystemId"] = AOETemplateSystemId;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "Fill_AOE_Radiology_Observations_For_Notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AOETemplate", "AOETemplate");
    },
    applyStyle: function (style) {
        if (style == 'bold') {
            $("#AOEobservationContent").css('font-weight', 'bold');
        }
        else if (style == 'italic') {
            $("#AOEobservationContent").css('font-style', 'italic');
        }
        else if (style == 'underline') {
            $("#AOEobservationContent").css('text-decoration', 'underline');
        }
        else if (style == 'reset') {
            $("#AOEobservationContent").css('font-weight', 'normal');
            $("#AOEobservationContent").css('font-style', 'normal');
            $("#AOEobservationContent").css('text-decoration', 'none');
        }
        else if (style == 'clear') {
            $("#AOEobservationContent").text('');
            $("#AOEobservationContent").val('');
        }
    },
    UpdateObservationDesc: function (AOENotesRadiologyObservationId) {
        var dataObsUp = new Object();
        dataObsUp["AOENotesRadiologyObservationId"] = AOENotesRadiologyObservationId;
        dataObsUp["Descr"] = $("#AOEobservationContent").val();
        dataObsUp["commandType"] = "Update_AOE_Selected_Radiology_Observations_Desc_For_Notes";
        var dataO = JSON.stringify(dataObsUp);
        return MDVisionService.APIService(dataO, "AOETemplate", "AOETemplate");
    },
}