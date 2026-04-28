PhysicalExamSysObservationDetail = {
    bIsFirstLoad: true,
    params: [],
    array: [],
    myArr: [],
    NewInsertedId: -1,
    selectedObservations: [],
    PENotesObservationId: null,
    Load: function (params) {
        BackgroundLoaderShow(true);
        PhysicalExamSysObservationDetail.params = params;
        var isSelectedEntity = false
        var self = $('#' + PhysicalExamSysObservationDetail.params.PanelID + ' #tblPhysicalExamSysObservationDetail');
        self.loadDropDowns(true).done(function () {
            PhysicalExamSysObservationDetail.InitTinymceControl(false, null);
            if (PhysicalExamSysObservationDetail.params["mode"] == "Edit") {
                PhysicalExamSysObservationDetail.bindobservationAutoComplete();
                PhysicalExamSysObservationDetail.loadObservations(PhysicalExamSysObservationDetail.params.PETemplateSystemId);
            }
        });

    },
    InitTinymceControl: function (Isreadonly, ControlId, height) {
        if (CKEDITOR.instances['observationContent'])
            CKEDITOR.instances['observationContent'].destroy();

        var objDeffered = $.Deferred();
        CKEDITOR.replace('observationContent', {
            toolbar: [    // Line break - next group will be placed in new line.
                     { name: 'clipboard', items: ['Cut'] },
                     { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline'] }
            ],
            on: {
                pluginsLoaded: function (evt) {
                    var doc = CKEDITOR.document, ed = evt.editor;
                    if (!ed.getCommand('bold'))
                        doc.getById('exec-bold').hide();
                    if (!ed.getCommand('link'))
                        doc.getById('exec-link').hide();
                    objDeffered.resolve();

                }, 

            }
        });
        CKEDITOR.config.startupFocus = 'end';
        CKEDITOR.instances['observationContent'].on('instanceReady', function () {
            $('.cke_button__cut')[0].setAttribute('title', 'Clear (Ctrl+X)');
        });
        return objDeffered;
    },
    bindobservationAutoComplete: function () {

        PhysicalExamTemplatesRevamp.lookupPEObservation_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Observation_JSON = JSON.parse(response.Observation_JSON);
                    $('#' + PhysicalExamSysObservationDetail.params.PanelID + " #Observations").kendoAutoComplete({
                        dataSource: Observation_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Observation...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulPhysicalExamSystemSection").find('#' + dataItem.id).length == 0) {

                                var PESystemId = PhysicalExamSysObservationDetail.params.PESystemId;
                                var PETemplateSystemId = PhysicalExamSysObservationDetail.params.PETemplateSystemId;
                                if (PESystemId > 0 && PETemplateSystemId > 0) {
                                    PhysicalExamSysObservationDetail.addObservationSystemAssosiation(dataItem.PEObservationId, PESystemId, dataItem.Name, PETemplateSystemId);
                                    $('#' + PhysicalExamSysObservationDetail.params.PanelID + " #Observations").val('');
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

    addObservationSystemAssosiation: function (observationId, PESystemId, Name, PETemplateSystemId) {

        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["PETemplateSystemId"] = PETemplateSystemId;

        objData["ObservationId"] = observationId;
        if (!objData["ObservationId"] || !objData["PESystemId"]) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }
        PhysicalExamTemplatesRevamp.addObservationSystemAssosiation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                utility.DisplayMessages(response.Message, 1);
                if (observationId) {
                    $('#' + PhysicalExamSysObservationDetail.params.PanelID + " #Observations").val('');
                    var objSelectedObservations = { PESystemId: PESystemId, IsSelected: true, ObservationId: observationId, ObservationName: Name, IsSystemChecked: true };
                    var li = PhysicalExamSysObservationDetail.addObservations(observationId, Name, PESystemId);
                    $('#' + PhysicalExamSysObservationDetail.params.PanelID + " #ulPhysicalExamSystemSection").append(li);
                }
            }
        });
    },

    addObservations: function (PEObservationId, ObservatioName, PESystemId) {
        var a = PhysicalExamSysObservationDetail.selectedObservations;

        var itemtoRemove = "observation";

        var li = '<li id="' + PEObservationId + '" parentid="' + PESystemId + '" onclick="PhysicalExamSysObservationDetail.PreviewObservations(' + PEObservationId + ',\'' + ObservatioName + '\', this, ' + PESystemId + ');" value="' + PEObservationId + '" refvalue="" subcharacteristicexist=" " class=""><div class="">' +
                 '<label id="lblName' + PEObservationId + '" class="" data-toggle="tooltip" title="" data-original-title="' + ObservatioName + '">' + ObservatioName + '</label><div id="divNameDetail' + PEObservationId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PEObservationId + '" onkeypress="" name="Name' + PEObservationId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PEObservationId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
        return li;
    },

    removeItem: function (PESystemId, control, PEObservationId) {
        if (control == "observation") {
            $("#ulPhysicalExamSystemSection #" + PEObservationId).remove();

            if (PhysicalExamSysObservationDetail.selectedObservations[i].PESystemId == PESystemId && PhysicalExamSysObservationDetail.selectedObservations[i].ObservationId == PEObservationId) {
                PhysicalExamSysObservationDetail.selectedObservations[i].ObservationId = -1;
            }
        }
    },

    removeObservation: function (PESystemId, PEObservationId) {
        $("#ulPhysicalExamSystemSection #" + PEObservationId).remove();

        if (PhysicalExamSysObservationDetail.selectedObservations) {
            for (var i = 0 ; i < PhysicalExamSysObservationDetail.selectedObservations.length; i++) {
                if (PhysicalExamSysObservationDetail.selectedObservations[i].PESystemId == PESystemId && PhysicalExamSysObservationDetail.selectedObservations[i].ObservationId == PEObservationId) {
                    PhysicalExamSysObservationDetail.selectedObservations[i].ObservationId = -1;
                }
            }
        }
    },

    loadObservations: function (PETemplateSystemId) {
        PhysicalExamSysObservationDetail.PhysicalExamSystemObservationsLoad(PETemplateSystemId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.PEObservationCount > 0) {
                    var res = JSON.parse(response.PEObservation_JSON);
                    var resSystems = JSON.parse(res);
                    $("#SystemSections").removeClass('hidden');
                    $("#ulPhysicalExamSystemSection li").remove();
                    $.each(resSystems, function (i, item) {
                        var li = PhysicalExamSysObservationDetail.addObservations(item.PEObservationId, item.ObservationName, item.PESystemId);
                        $("#ulPhysicalExamSystemSection").append(li);
                        var objSelectedObservations = { PESystemId: item.PESystemId, IsChecked: false, ObservationId: item.PEObservationId, ObservationName: item.ObservationName, IsSystemChecked: false };
                        PhysicalExamSysObservationDetail.selectedObservations.push(objSelectedObservations);
                        CKEDITOR.instances['observationContent'].setData(utility.decodeHtml(item.SystemDescription));
                        PhysicalExamSysObservationDetail.PENotesObservationId = item.PENotesObservationId;
                        $('#' + PhysicalExamSysObservationDetail.params.PanelID + ' #headingTitle').text('Physical Examination - ' + item.SystemName)
                    });

                }
            }
            PhysicalExamSysObservationDetail.InitTinymceControl(false, null);
        });
    },

    selectAllChars: function (obj) {
        if (obj) var sysId = $($($(obj).parent().parent())[0]).attr('parentid');

        if ($(obj).prop('checked') == true) {


            $("#SystemPreview").removeClass('hidden');
            $('#' + PhysicalExamSysObservationDetail.params.PanelID + ' #ulPhysicalExamSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);

                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3)
                    var observationName = $("#divPhysicalExamSystemSection #lblName" + id_).text();
                    var delimator = $("#delimator option:selected").text() + " ";
                    CKEDITOR.instances['observationContent'].setData(CKEDITOR.instances['observationContent'].getData() + observationName + delimator);
                    var objSelectedObservations = { PESystemId: sysId, IsChecked: true, ObservationId: id_, ObservationName: observationName, IsModified: '1' };
                    PhysicalExamSysObservationDetail.selectedObservations.push(objSelectedObservations);

                    $("#ulPhysicalExamSystems #chk" + sysId).prop("checked", true);
                }
            });
            //var objSelectedAll = { PESystemId: sysId, IsChecked: true, ObservationId: 'chkboxSelectAllObservations', ObservationName: 'Select All', IsModified: '1' };
            //PhysicalExamSysObservationDetail.selectedObservations.push(objSelectedAll);

            if (PhysicalExamSysObservationDetail.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamSysObservationDetail.selectedObservations.length; i++) {
                    if (PhysicalExamSysObservationDetail.selectedObservations[i].PESystemId == sysId) {
                        PhysicalExamSysObservationDetail.selectedObservations[i].IsChecked = true;
                        PhysicalExamSysObservationDetail.selectedObservations[i].IsSystemChecked = true;
                    }
                }
            }

        }
        else if ($(obj).prop('checked') == false) {
            $('#' + PhysicalExamSysObservationDetail.params.PanelID + ' #ulPhysicalExamSystemSection li .char').removeClass("green");
            $('#' + PhysicalExamSysObservationDetail.params.PanelID + ' #ulPhysicalExamSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                }
            });

            if (PhysicalExamSysObservationDetail.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamSysObservationDetail.selectedObservations.length; i++) {
                    if (PhysicalExamSysObservationDetail.selectedObservations[i].PESystemId == sysId) {
                        PhysicalExamSysObservationDetail.selectedObservations[i].IsChecked = false;
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

        if (CKEDITOR.instances['observationContent'].getData()) {
            CKEDITOR.instances['observationContent'].insertText(deli + ObservationName);
        }
        else {
            CKEDITOR.instances['observationContent'].insertText(CKEDITOR.instances['observationContent'].getData() + ObservationName);
        }

        PhysicalExamSysObservationDetail.selectedObservations.push(objSelectedObservations);

        if (PhysicalExamSysObservationDetail.selectedObservations) {
            for (var i = 0 ; i < PhysicalExamSysObservationDetail.selectedObservations.length; i++) {
                if (PhysicalExamSysObservationDetail.selectedObservations[i].PESystemId == PESystemId && PhysicalExamSysObservationDetail.selectedObservations[i].ObservationId == observationId) {
                    PhysicalExamSysObservationDetail.selectedObservations[i].IsChecked = true;
                    PhysicalExamSysObservationDetail.selectedObservations[i].IsSystemChecked = true;
                }
                if (PhysicalExamSysObservationDetail.selectedObservations[i].PESystemId == PESystemId)
                    PhysicalExamSysObservationDetail.selectedObservations[i].IsSystemChecked = true;
            }
        }
        $("#ulPhysicalExamSystems #chk" + PESystemId).prop("checked", true);
    },

    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + PhysicalExamSysObservationDetail.params["PanelID"];
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
                var $Item = $(PhysicalExamSysObservationDetail.selectedListItem);
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
            currentId = PhysicalExamSysObservationDetail.NewInsertedId--;
            var subcharacteristicExist = "";

            var saveMethodPE = "";

            if (itemType.toLowerCase() == "system") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + PhysicalExamSysObservationDetail.params.PanelID + " #frmPhysicalExamSysObservationDetail #" + currentCtrlId);
                saveMethodPE = "PhysicalExamSysObservationDetail.AddSystemPE(this, '" + currentId + "');";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystemSection";
                ulControl = $('#' + PhysicalExamSysObservationDetail.params.PanelID + " #frmPhysicalExamSysObservationDetail #" + currentCtrlId);
                saveMethodPE = "PhysicalExamSysObservationDetail.AddObservation(this, '" + currentId + "');";
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
                onClick = "";//"PhysicalExamSysObservationDetail.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";
                var deleteFunction = "PhysicalExamSysObservationDetail.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                liInnerText = '<div class="">' + deleteIcon + addSubCharIcon
                    //+ '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="PhysicalExamSysObservationDetail.selectParentControls(this);PhysicalExamSysObservationDetail.toggleCheckBoxes(this);">'
                    + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                    + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                    + '<textarea rows="1" id="txtName' + currentId + '" onkeypress="PhysicalExamSysObservationDetail.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" spellcheck="true" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
                    + '<div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodPE + '" class="btn btn-link btn-xs">'
                    + '<i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';

                var liTobeAdded = '<li id="' + currentId + '" ' + currentLiClass + ' parentid="' + currentParentId + '" onclick="' + onClick + '" value="' + currentId + '" refValue="' + currentId + '"' + subcharacteristicExist + '>' + liInnerText + '</li>';

                ulControl.prepend(liTobeAdded);
                ulControl.find('li#' + currentId + ' #txtName' + currentId).focus()

            }

        }
    },

    AddObservation: function (obj, controlId) {
        var PESystemId = PhysicalExamSysObservationDetail.params.PESystemId;

        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = 1;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        PhysicalExamSysObservationDetail.savePEObservation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    PhysicalExamSysObservationDetail.params.PEObservationId = response.PEObservationId;
                    var li = PhysicalExamSysObservationDetail.addObservations(response.PEObservationId, objData["Name"], PESystemId);
                    $("#ulPhysicalExamSystemSection").append(li);

                    var objSelectedObservations = { PESystemId: PESystemId, IsChecked: false, ObservationId: response.PEObservationId, ObservationName: objData["Name"], IsSystemChecked: true };
                    PhysicalExamSysObservationDetail.selectedObservations.push(objSelectedObservations);
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

        } else if (ctrlId == "ulPhysicalExamSystemSection") {
            $("#" + currentId).remove();
        }
    },

    validateSelectedTemplateData: function () {

        var isValid = true;

        if ($(PhysicalExamSysObservationDetail.selectedPhyExamTempData).length > 0) {
            $.each(PhysicalExamSysObservationDetail.selectedPhyExamTempData, function (i, item) {
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
        PhysicalExamSysObservationDetail.array = [];
        PhysicalExamSysObservationDetail.myArr = [];
        PhysicalExamSysObservationDetail.NewInsertedId = -1;
        PhysicalExamSysObservationDetail.selectedObservations = [];
        var PENotesObservationId = PhysicalExamSysObservationDetail.PENotesObservationId;
        PhysicalExamSysObservationDetail.UpdateObservationDesc(PENotesObservationId).done(function (response) {
            var selectedNoteDesc = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #Cli_PhysicalExam_NO_' + PENotesObservationId);
            if (selectedNoteDesc) {
                if (CKEDITOR.instances['observationContent'].getData() != "") {
                    $(selectedNoteDesc).html(utility.decodeHtml(CKEDITOR.instances['observationContent'].getData()));
                    
                }
                else {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #Cli_PhysicalExam_NO_' + PENotesObservationId).closest('section').remove();
                }
                Clinical_ProgressNote.saveComponentSOAPText('Physical Exam');
                //Clinical_ProgressNote.updateProgressNoteHTML();
                Clinical_ProgressNote.hoverFunction();
            }
            UnloadActionPan(PhysicalExamSysObservationDetail.params["ParentCtrl"], "PhysicalExamSysObservationDetail");
        });

    },
    PhysicalExamSystemObservationsLoad: function (PETemplateSystemId) {
        var objData = new Object();
        objData["PETemplateSystemId"] = PETemplateSystemId;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "Fill_PE_Observations_For_Notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
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
            $("#observationContent").css('font-weight', 'normal');
            $("#observationContent").css('font-style', 'normal');
            $("#observationContent").css('text-decoration', 'none');
        }
        else if (style == 'clear') {
            CKEDITOR.instances['observationContent'].setData('');
        }
    },
    UpdateObservationDesc: function (PENotesObservationId) {
        var dataObsUp = new Object();
        dataObsUp["PENotesObservationId"] = PENotesObservationId;
        dataObsUp["Descr"] = CKEDITOR.instances['observationContent'].getData();
        dataObsUp["commandType"] = "Update_PE_Selected_Observations_Desc_For_Notes";
        var dataO = JSON.stringify(dataObsUp);
        return MDVisionService.APIService(dataO, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },
}