PhysicalExamTemplatesRevamp = {
    bIsFirstLoad: true,
    params: [],
    normalSystemIdsGlobel: [],
    selectedPhyExamTempData: [],
    NewInsertedId: -1,
    selectedObservations: [],
    selectedSystems: [],
    selectedPETemp: null,
    ScrollTabObj: null,
    Content_style: '',
    selectedSystemID: 0,
    selectedTemplateSystemID: 0,
    Load: function (params) {
        BackgroundLoaderShow(true);
        PhysicalExamTemplatesRevamp.params = params;
        var isSelectedEntity = false
        PhysicalExamTemplatesRevamp.selectedPhyExamTempData = [];
        PhysicalExamTemplatesRevamp.selectedSystems = [];
        PhysicalExamTemplatesRevamp.Content_style = '';
        var self = $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #tblPhysicalExamTemplatesRevamp');
        self.loadDropDowns(true).done(function () {

            PhysicalExamTemplatesRevamp.InitTinymceControl(false, null);

            var objDeff1 = $.Deferred();
            var objDeff2 = $.Deferred();
            var objDeff3 = $.Deferred();

            objDeff1 = PhysicalExamTemplatesRevamp.buildSystemsAutoComplete();
            objDeff2 = PhysicalExamTemplatesRevamp.buildTemplateAutoComplete();
            objDeff3 = PhysicalExamTemplatesRevamp.bindobservationAutoComplete();

            if (PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId && PhysicalExamTemplatesRevamp.params.mode == "Edit") {
                $.when(objDeff1, objDeff2, objDeff3).then(function () {
                    PhysicalExamTemplatesRevamp.FillPhysicalExamTemplates();
                });
            }

            ScrollTabObj = null;
            PhysicalExamTemplatesRevamp.ScrollTabObj = $("#" + "ulSelectedPETemplates").scrollTabs();
            if (PhysicalExamTemplatesRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
                EMRUtility.appendPrevNext_NotesComponent_Btns(PhysicalExamTemplatesRevamp.params.PanelID, '', 'PhysicalExam', 'PhysicalExamTemplatesRevamp.unLoadTab();', null, true);
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

        CKEDITOR.instances['observationContent'].on('blur', function () {
            PhysicalExamTemplatesRevamp.updateSystemDescription();
        });
        CKEDITOR.instances['observationContent'].on('instanceReady', function () {
            $('.cke_button__cut')[0].setAttribute('title', 'Clear (Ctrl+X)');
        });
        CKEDITOR.config.startupFocus = 'end';
        return objDeffered;
    },
    addSystem: function (PESystemId, SystemName, PETemplateId, PETemplateSystemId) {
        var itemtoRemove = "system";

        var li = '<li id="' + PESystemId + '"  PETemplateSystemId="' + PETemplateSystemId + '" parentid="' + PETemplateId + '"  onclick="PhysicalExamTemplatesRevamp.loadObservations(' + PESystemId + ')" value="' + PESystemId + '" refvalue="" subcharacteristicexist=" " class="">' +
                    '<div class="checkbox-custom checkboxTiny checkbox-success">' +
                     '<input type="checkbox" id="chk' + PETemplateSystemId + '" name="' + PETemplateSystemId + '" class="pull-left  char" onclick="PhysicalExamTemplatesRevamp.ManageObservations(' + PESystemId + ', this,' + PETemplateSystemId + ');">'
                       + '<label id="lblName' + PESystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SystemName + '">' + SystemName + '</label><div id="divNameDetail' + PESystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PESystemId + '" onkeypress="" name="Name' + PESystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                       PESystemId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
        //<a href="#"><span class="removeIconListHover" onclick="PhysicalExamTemplatesRevamp.removeSystem(' + PETemplateSystemId + ',' + PESystemId + ',event);"><i class="fa fa-close"></i></span></a>
        return li;
    },

    addObservations: function (PEObservationId, ObservatioName, PESystemId, PESystemObservationId) {
        var a = PhysicalExamTemplatesRevamp.selectedObservations;
        ObservatioName = utility.decodeHtml(ObservatioName);
        var itemtoRemove = "observation";
        var desc = "";
        if (ObservatioName.indexOf("'") > -1) {
            desc = ObservatioName.replace(/\'/g, '@');
        } else {
            desc = ObservatioName;
        }
        var li = '<li id="' + PEObservationId + '" parentid="' + PESystemId + '" onclick="" value="' + PEObservationId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                 '<input type="checkbox" id="chk' + PEObservationId + '" name="' + PEObservationId + '" class="pull-left  char" ' +
                 'onclick="PhysicalExamTemplatesRevamp.PreviewObservations(' + PEObservationId + ',\'' + desc + '\', this, ' + PESystemId + ');">'
                 + '<label id="lblName' + PEObservationId + '" class="" data-toggle="tooltip" title="" data-original-title="' + ObservatioName + '">' + ObservatioName + '</label><div id="divNameDetail' + PEObservationId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PEObservationId + '" onkeypress="" name="Name' + PEObservationId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PEObservationId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="PhysicalExamTemplatesRevamp.removeObservation(' + PESystemObservationId + ',' + PEObservationId + ',event)"><i class="fa fa-close"></i></span></a></li>';
        return li;
    },

    removeItem: function (PESystemId, control, PEObservationId) {
        var PETemplateSystemId = $("#ulPhysicalExamSystems #" + PESystemId).attr('PETemplateSystemId')
        if (control == "system") {
            $("#ulPhysicalExamSystems #" + PESystemId).remove();
            if (PhysicalExamTemplatesRevamp.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamTemplatesRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplatesRevamp.selectedObservations[i].PETemplateSystemId == PETemplateSystemId) {
                        PhysicalExamTemplatesRevamp.selectedObservations[i].PETemplateSystemId = -1;
                    }
                }
            }
        }
        else if (control == "observation") {
            $("#ulPhysicalExamSystemSection #" + PEObservationId).remove();
            if (PhysicalExamTemplatesRevamp.selectedObservations[i].PETemplateSystemId == PETemplateSystemId) {
                PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationId = -1;
            }
        }


    },

    removeSystem: function (PETemplateSystemId, PESystemId, event) {
        if (event != null)
            event.stopPropagation();
        utility.myConfirm('28', function () {
            if (PETemplateSystemId) {
                PhysicalExamTemplatesRevamp.RemoveSystemDB_Call(PETemplateSystemId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $("#ulPhysicalExamSystems #" + PESystemId).remove();

                        if (PhysicalExamTemplatesRevamp.selectedObservations) {
                            for (var i = 0 ; i < PhysicalExamTemplatesRevamp.selectedObservations.length; i++) {
                                if (PhysicalExamTemplatesRevamp.selectedObservations[i].PETemplateSystemId == PETemplateSystemId) {
                                    PhysicalExamTemplatesRevamp.selectedObservations[i].PETemplateSystemId = -1;
                                }
                            }
                        }
                    }
                });
            }
            else {
                $("#ulPhysicalExamSystems #" + PESystemId).remove();
            }
        }, function () {
        },
        '1'
    );
    },

    removeObservation: function (PESystemObservationId, PEObservationId, event) {
        if (event != null)
            event.stopPropagation();
        utility.myConfirm('28', function () {
            if (PESystemObservationId) {
                PhysicalExamTemplatesRevamp.RemoveObservationDB_Call(PESystemObservationId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $("#ulPhysicalExamSystemSection #" + PEObservationId).remove();

                        if (PhysicalExamTemplatesRevamp.selectedObservations) {
                            var UpdatedArray = [];
                            UpdatedArray = jQuery.grep(PhysicalExamTemplatesRevamp.selectedObservations, function (value) {
                                return value.PESystemObservationId != PESystemObservationId;
                            });
                            PhysicalExamTemplatesRevamp.selectedObservations = UpdatedArray;
                        }
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                });
            }
            else {
                $("#ulPhysicalExamSystemSection #" + PEObservationId).remove();
            }
        }, function () {
        },
        '1'
     );

    },

    loadObservations: function (PESystemId, obj, PETemplateId) {
       PhysicalExamTemplatesRevamp.updateSystemDescription();
        var objDeffered = $.Deferred();
        PETemplateId = $('#divPhysicalExams').find('.active').val();
        var $obj = $('#ulPhysicalExamSystems li[id="' + PESystemId + '"]');
        PhysicalExamTemplatesRevamp.selectedSystemID = PESystemId;
        var PETemplateSystemId = $('#divPhysicalExamSystems li#' + PESystemId).attr('petemplatesystemid');
        PhysicalExamTemplatesRevamp.selectedTemplateSystemID = PETemplateSystemId;
        $('#ulPhysicalExamSystems li').removeClass('text-success');
        var $Sysli = $('#ulPhysicalExamSystems li[id="' + PESystemId + '"]');
        if ($Sysli) {
            var checked = $($Sysli).find('input[type="checkbox"]').is(':checked');
            if (checked)
                $("#ulPhysicalExamSystems li[id='" + PESystemId + "']").addClass('text-success');
        }
        $("#SystemSections").removeClass('hidden');
        var desr = "";
        var isSelected = false;
        var isExist = false;
        if (PhysicalExamTemplatesRevamp.selectedObservations) {
            for (var i = 0 ; i < PhysicalExamTemplatesRevamp.selectedObservations.length; i++) {
                if (PhysicalExamTemplatesRevamp.selectedObservations[i].PETemplateSystemId == PETemplateSystemId) {
                    isExist = true;
                    break;
                }
            }
        }
        if (!isExist) {
            PhysicalExamTemplatesRevamp.PhysicalExamSystemObservationsLoad(PESystemId, PETemplateId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $("#ulPhysicalExamSystemSection li").remove();
                    if (response.PEObservationCount > 0) {
                        var res = JSON.parse(response.PEObservation_JSON);
                        var resSystems = JSON.parse(res);
                        var selectedTempSys = null;

                        var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + PESystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                            + 'onclick="PhysicalExamTemplatesRevamp.selectAllCharsNotes(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        $("#ulPhysicalExamSystemSection").append(selectAll);

                        $.each(resSystems, function (i, item) {
                            selectedTempSys = item.PETemplateSystemId;
                            var li = PhysicalExamTemplatesRevamp.addObservations(item.PEObservationId, item.Name, PESystemId, item.PESystemObservationId);
                            $("#ulPhysicalExamSystemSection").append(li);
                            var isSelected = item.IsObservationSelected == "True" ? true : false;
                            var objSelectedObservations = { PESystemId: PESystemId, IsSelected: item.IsObservationSelected == "True" ? true : false, ObservationId: item.PEObservationId, ObservationName: item.Name, IsSystemChecked: false, PETemplateSystemId: item.PETemplateSystemId };
                            PhysicalExamTemplatesRevamp.selectedObservations.push(objSelectedObservations);
                            if (isSelected)
                                $("#ulPhysicalExamSystemSection li[id=" + item.PEObservationId + "]").addClass('text-success');
                            else
                                $("#ulPhysicalExamSystemSection li[id=" + item.PEObservationId + "]").removeClass('text-success');
                            $("#ulPhysicalExamSystemSection li #chk" + item.PEObservationId + "").prop("checked", isSelected);

                        });
                        var desc = "";
                        var resNotes = JSON.parse(response.PENotesObservation_JSON);
                        var resNotesObs = JSON.parse(resNotes);
                        var selectedNotesObs = null;
                        $(resNotesObs).filter(function (ind, i) {
                            if (i.PETemplateSystemId == selectedTempSys) {
                                selectedNotesObs = i;
                            }
                        });
                        if (selectedNotesObs && selectedNotesObs.Desc) {
                            desc = selectedNotesObs.Desc;

                            CKEDITOR.instances['observationContent'].setData(utility.decodeHtml(desc));
                        }
                        else {
                            var observations = []
                            var cloneResSystems = resSystems;
                            cloneResSystems = PhysicalExamTemplateDetailRevamp.SortJSONArray(resSystems, "ObservationOrder", "123");
                            $.each(cloneResSystems, function (i, item) {
                                if (parseInt(item.PESystemId) == parseInt(PESystemId) && item.IsObservationSelected == "True")
                                    observations.push(item.Name);
                            });
                            desc = observations.join(", ");
                            CKEDITOR.instances['observationContent'].setData(desc);

                        }
                        $.each(PhysicalExamTemplatesRevamp.selectedSystems, function (i, item) {
                            if (item.PESystemId == PESystemId && item.PETemplateId == PETemplateId) {
                                item.SystemDescription = desc;
                            }
                        });

                        PhysicalExamTemplatesRevamp.FillObservationContent(PETemplateSystemId);
                        objDeffered.resolve();
                    }
                }

            });
        }
        else {
            $("#ulPhysicalExamSystemSection li").remove();
            if (PhysicalExamTemplatesRevamp.selectedObservations) {

                var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + PESystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                    + 'onclick="PhysicalExamTemplatesRevamp.selectAllCharsNotes(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                $("#ulPhysicalExamSystemSection").append(selectAll);

                var IfSelectAll = 0;

                for (var i = 0 ; i < PhysicalExamTemplatesRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplatesRevamp.selectedObservations[i].PETemplateSystemId == PETemplateSystemId) {
                        var li = PhysicalExamTemplatesRevamp.addObservations(PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationId, PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationName, PESystemId, PhysicalExamTemplatesRevamp.selectedObservations[i].PESystemObservationId);

                        if (PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationId != "" && PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationId != -1) {
                            if ($("#ulPhysicalExamSystemSection #" + PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationId).length == 0) {
                                $("#ulPhysicalExamSystemSection").append(li);
                                if (PhysicalExamTemplatesRevamp.selectedObservations[i].IsSelected)
                                    IfSelectAll++;
                            }
                        }
                        var isSelected = PhysicalExamTemplatesRevamp.selectedObservations[i].IsSelected;
                        if (isSelected) {

                            $("#ulPhysicalExamSystemSection li[id=" + PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationId + "]").addClass('text-success');
                        }
                        else
                            $("#ulPhysicalExamSystemSection li[id=" + PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationId + "]").removeClass('text-success');

                        $("#ulPhysicalExamSystemSection li #chk" + PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationId + "").prop("checked", isSelected);

                    }
                }
                if (IfSelectAll == ($("#ulPhysicalExamSystemSection li").length - 1))
                    $("#ulPhysicalExamSystemSection li #chkboxSelectAllObservations").prop("checked", true);

            }
            //$.each(PhysicalExamTemplatesRevamp.selectedSystems, function (i, item) {
            //    if (item.PESystemId == PESystemId && item.PETemplateId == PETemplateId) {
            //        item.IsSelected = true;
            //        //$('#ulPhysicalExamSystems li #chk' + item.PETemplateSystemId).prop('checked', true);
            //    }
            //});
            PhysicalExamTemplatesRevamp.FillObservationContent(PETemplateSystemId);
            //var observations = []
            //$.each(PhysicalExamTemplatesRevamp.selectedObservations, function (i, item) {
            //    if (parseInt(item.PESystemId) == parseInt(PESystemId) && item.IsSelected == true)
            //        observations.push(item.ObservationName);
            //});
            //$("#observationContent").val(observations.join(", "));
            objDeffered.resolve();
        }
        return objDeffered;
    },

    selectAllCharsNotes: function (obj) {
        $('#observationContent div[id^=divSystem]').remove();
        if (obj) var sysId = $($($(obj).parent().parent())[0]).attr('parentid');

        if ($(obj).prop('checked') == true) {
            $("#SystemPreview").removeClass('hidden');
            $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);

                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3)
                    var observationName = $("#divPhysicalExamSystemSection #lblName" + id_).text();
                    var delimator = $("#delimator option:selected").text() + " ";


                    $("#ulPhysicalExamSystemSection li[id=" + id_ + "]").addClass('text-success');

                    PhysicalExamTemplatesRevamp.PreviewObservations(id_, observationName, obj, sysId);

                    //if ($("#observationContent #divSystem" + sysId + id_).length > 0) {
                    //    $('#observationContent #divSystem' + sysId + id_).remove();
                    //    var $newDiv = $("<div></div>");
                    //    $newDiv.attr("id", "divSystem" + sysId + id_);
                    //    $newDiv.attr("style", "display: inline;");

                    //    $("#observationContent").append($newDiv);
                    //    $('#observationContent #divSystem' + sysId + id_).show();
                    //    var txttoAppend = observationName;
                    //    if ($('#observationContent div').length > 1)
                    //        txttoAppend = delimator + observationName;

                    //    $("#observationContent #divSystem" + sysId + id_).append(txttoAppend);
                    //}
                    //else {
                    //    var $newDiv = $("<div></div>");
                    //    $newDiv.attr("id", "divSystem" + sysId + id_);
                    //    $newDiv.attr("style", "display: inline;");

                    //    $("#observationContent").append($newDiv);
                    //    $('#observationContent #divSystem' + sysId + id_).show();

                    //    var txttoAppend = observationName;
                    //    if ($('#observationContent div').length > 1)
                    //        txttoAppend = delimator + observationName;

                    //    $("#observationContent #divSystem" + sysId + id_).append(txttoAppend);
                    //}

                    //$("#ulPhysicalExamSystems #chk" + $("#ulPhysicalExamSystems #" + sysId).attr('PETemplateSystemId')).prop("checked", true);
                }
            });

            if (PhysicalExamTemplatesRevamp.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamTemplatesRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplatesRevamp.selectedObservations[i].PESystemId == sysId) {
                        PhysicalExamTemplatesRevamp.selectedObservations[i].IsChecked = true;
                        PhysicalExamTemplatesRevamp.selectedObservations[i].IsSystemChecked = true;
                        PhysicalExamTemplatesRevamp.selectedObservations[i].SystemDescription = CKEDITOR.instances['observationContent'].getData();// $('#observationContent #divSystem' + sysId).text();
                        PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationOrder = i;
                    }
                }
            }
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').removeClass("green");
            $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                    var system_id = $($($(obj).parent().parent())[0]).attr('parentid');

                    $("#ulPhysicalExamSystemSection li[id=" + id_ + "]").removeClass('text-success');
                    //  $("#observationContent #divSystem" + system_id + id_).remove();
                    //  $('#observationContent #divSystem' + PESystemId + observationId).remove();

                    // $("#ulPhysicalExamSystems #chk" + $("#ulPhysicalExamSystems #" + sysId).attr('PETemplateSystemId')).prop("checked", false);
                    var observationName = $("#divPhysicalExamSystemSection #lblName" + id_).text();

                    PhysicalExamTemplatesRevamp.PreviewObservations(id_, observationName, obj, sysId);

                }
            });

            if (PhysicalExamTemplatesRevamp.selectedObservations) {
                for (var i = 0 ; i < PhysicalExamTemplatesRevamp.selectedObservations.length; i++) {
                    if (PhysicalExamTemplatesRevamp.selectedObservations[i].PESystemId == sysId) {
                        PhysicalExamTemplatesRevamp.selectedObservations[i].IsChecked = false;
                        PhysicalExamTemplatesRevamp.selectedObservations[i].SystemDescription = CKEDITOR.instances['observationContent'].getData();//  $('#observationContent #divSystem' + sysId).text();
                        PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationOrder = -1;
                        //$('#observationContent div[id^=divSystem' + sysId + ']').hide();
                    }
                }
            }
        }
        PhysicalExamTemplatesRevamp.removeLastDelimiter(sysId);
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
        $("#observationContent #" + id).text(utility.decodeHtml(CKEDITOR.instances['observationContent'].getData()));
    },

    updateSystemDescription: function ($obj) {

        $(PhysicalExamTemplatesRevamp.selectedSystems).each(function (ind, i) {
            if (i.IsCurrent == true && i.PETemplateId == PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId) {
                i.SystemDescription = utility.decodeHtml(CKEDITOR.instances['observationContent'].getData());//$($obj).val();
            }
        });
    },
    FillObservationContent: function (PETemplateSystemId, fromSave) {
        $(PhysicalExamTemplatesRevamp.selectedSystems).each(function (ind, i) {
            i.IsCurrent = false;
        });
        $(PhysicalExamTemplatesRevamp.selectedSystems).each(function (ind, i) {
            if (i.PETemplateSystemId == PETemplateSystemId) {
                if (fromSave == true) {
                    // CKEDITOR.instances['observationContent'].setData(utility.decodeHtml(CKEDITOR.instances['observationContent'].getData()));
                } else {
                    CKEDITOR.instances['observationContent'].setData(utility.decodeHtml(i.SystemDescription));
                }
                i.IsCurrent = true;
            }
        });
        PhysicalExamTemplatesRevamp.InitTinymceControl(false, null);
    },
    ManageObservations: function (PESystemId, obj, PETemplateSystemId, event) {
        if (event != null)
            event.stopPropagation();
        var IsSelected = $(obj).is(':checked')
        $(PhysicalExamTemplatesRevamp.selectedSystems).filter(function (ind, i) {
            if (i.PETemplateSystemId == PETemplateSystemId) {
                i.IsSelected = IsSelected;
                i.IsCurrent = true;
            }
        });
        if (IsSelected) {
            $("#ulPhysicalExamSystems li[id='" + PESystemId + "']").addClass('text-success');
        }
        else
            $("#ulPhysicalExamSystems li[id='" + PESystemId + "']").removeClass('text-success');
    },

    selectAllChars: function (obj) {
        $("#SystemPreview").removeClass('hidden');

        if (obj) var sysId = $($($(obj).parent().parent())[0]).attr('parentid');
        $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #ulPhysicalExamSystemSection li .char').each(function () {
            if ($(this).attr("id")) {
                var id_ = $(this).attr("id")
                var observationName = $("#divPhysicalExamSystemSection #lblName" + id_).text();
                var delimator = $("#delimator option:selected").text() + " ";
                observationName + delimator;
                var desc = CKEDITOR.instances['observationContent'].getData() + observationName;
                CKEDITOR.instances['observationContent'].setData(desc);
                //$("#observationContent").val($("#observationContent").val() + observationName);
                //  $("#observationContent").text($("#observationContent").text() + observationName + delimator);
                var objSelectedObservations = { PESystemId: sysId, IsSelected: true, ObservationId: id_, ObservationName: observationName, IsModified: '1' };
                PhysicalExamTemplatesRevamp.selectedObservations.push(objSelectedObservations);
            }
        });
        //var objSelectedAll = { PESystemId: sysId, IsSelected: true, ObservationId: 'chkboxSelectAllObservations', ObservationName: 'Select All', IsModified: '1' };
        //PhysicalExamTemplatesRevamp.selectedObservations.push(objSelectedAll);

        if (PhysicalExamTemplatesRevamp.selectedObservations) {
            for (var i = 0 ; i < PhysicalExamTemplatesRevamp.selectedObservations.length; i++) {
                if (PhysicalExamTemplatesRevamp.selectedObservations[i].PESystemId == sysId) {
                    PhysicalExamTemplatesRevamp.selectedObservations[i].IsSelected = true;
                    PhysicalExamTemplatesRevamp.selectedObservations[i].IsSystemChecked = true;
                }
            }
        }

    },

    applyStyle: function (style) {
        if (style == 'bold') {
            if ($("#observationContent").css('font-weight') != 'bold')
                $("#observationContent").css('font-weight', 'bold');
            else {
                $("#observationContent").css('font-weight', 'normal');
            }

        }
        else if (style == 'italic') {
            if ($("#observationContent").css('font-style') != 'italic')
                $("#observationContent").css('font-style', 'italic');
            else
                $("#observationContent").css('font-style', 'normal');
        }
        else if (style == 'underline') {
            if ($("#observationContent").css('text-decoration') != 'underline')
                $("#observationContent").css('text-decoration', 'underline');
            else
                $("#observationContent").css('text-decoration', 'none');
        }
        else if (style == 'reset') {
            $("#observationContent").css('font-weight', 'normal');
            $("#observationContent").css('font-style', 'normal');
            $("#observationContent").css('text-decoration', 'none');
        }
        else if (style == 'clear') {
            $("#observationContent").css('font-weight', 'normal');
            $("#observationContent").css('font-style', 'normal');
            $("#observationContent").css('text-decoration', 'none');
            //$("#observationContent").val('');
            //$("#observationContent").val('');
            CKEDITOR.instances['observationContent'].setData('');

            $("#observationContent").trigger('blur');
        }
    },

    PreviewObservations: function (observationId, ObservationName, obj, PESystemId) {
        $("#SystemPreview").removeClass('hidden');
        var IsSelected = $(obj).is(':checked');
        var PETemplateSystemId = $("#ulPhysicalExamSystems #" + PESystemId).attr('petemplatesystemid');
        var $li = $("#ulPhysicalExamSystemSection #" + observationId);
        if (IsSelected) {
            if ($li && !$li.hasClass('text-success'))
                $li.addClass('text-success');
            var objSelectedObservations =
               {
                   PESystemId: PESystemId,
                   IsSelected: true,
                   ObservationId: observationId,
                   ObservationName: ObservationName,
                   IsModified: '1',
                   IsSystemChecked: false,
                   PETemplateSystemId: PETemplateSystemId
               };
            var deli = $("#delimator option:selected").text() + " ";
            ObservationName = utility.decodeHtml(ObservationName);
            ObservationName = ObservationName.replace(/\@/g, "'");
            // $("#observationContent").text($("#observationContent").text() + ObservationName + deli);
            if (CKEDITOR.instances['observationContent'].getData()) {
                CKEDITOR.instances['observationContent'].insertText(deli + ObservationName);

            }
            else {
                CKEDITOR.instances['observationContent'].insertText(CKEDITOR.instances['observationContent'].getData() + ObservationName);
            }
            PhysicalExamTemplatesRevamp.selectedObservations.push(objSelectedObservations);
            $("#observationContent").trigger('blur');
        }
        else {
            if ($li)
                $li.removeClass('text-success')


        }
        if (PhysicalExamTemplatesRevamp.selectedObservations) {
            for (var i = 0 ; i < PhysicalExamTemplatesRevamp.selectedObservations.length; i++) {
                if (PhysicalExamTemplatesRevamp.selectedObservations[i].PETemplateSystemId == PETemplateSystemId && PhysicalExamTemplatesRevamp.selectedObservations[i].ObservationId == observationId) {
                    PhysicalExamTemplatesRevamp.selectedObservations[i].IsSelected = IsSelected;
                    PhysicalExamTemplatesRevamp.selectedObservations[i].IsSystemChecked = IsSelected;
                }
            }
        }
    },

    savePhysicalExam: function () {
        PhysicalExamTemplatesRevamp.physicalExamTemplateSave();
    },

    buildAlreadyAssosiatedSystems: function (templateId) {
        PhysicalExamTemplatesRevamp.PhysicalExamTemplatesRevampLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var res = JSON.parse(response.PETemplateSystems_JSON);
                var resSystems = JSON.parse(res);
                var data = [];

                $.each(resSystems, function (i, item) {
                    data.push({ id: item.PESystemId, text: item.SystemName, expanded: true, spriteCssClass: "rootfolder" });
                    var li = PhysicalExamTemplatesRevamp.addSystem(item.PESystemId, item.SystemName, item.PETemplateId, item.PETemplateSystemId);
                    $("#ulPhysicalExamSystems").append(li);
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    buildSystemsAutoComplete: function (PETemplateId) {
        var objDeffered = $.Deferred();
        if (!PETemplateId && PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId) {
            PETemplateId = PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId
        }
        else {
            PhysicalExamTemplatesRevamp.params["PhysicalExamTemplateId"] = PETemplateId;
        }
        PhysicalExamTemplatesRevamp.PhysicalExamSystemsLoad(PETemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.PESystemsCount > 0) {
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
                            var PETemplateSystemId = "-1";
                            if (dataItem.id)
                                if ($("#ulPhysicalExamSystems").find('#' + dataItem.id).length == 0) {
                                    PhysicalExamTemplatesRevamp.addSystemTempAssosiation(PhysicalExamTemplatesRevamp.params["PhysicalExamTemplateId"], dataItem.id, dataItem.value);
                                }
                                else {
                                    utility.DisplayMessages(dataItem.value + " already exists.", 3);
                                }
                            $("#Systems").val('');
                            e.preventDefault();
                        },
                        footerTemplate: 'Total <strong>#: instance.dataSource.total() #</strong> items found'
                    });
                    $("#Systems").parent().addClass('size100per');
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            objDeffered.resolve();
        });
        return objDeffered.promise();
    },

    buildTemplateAutoComplete: function () {
        var PETemplateId = "";
        var objDeffered = $.Deferred();
        PhysicalExamTemplatesRevamp.PhysicalExamActiveTemplatesLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.PETemplateCount > 0) {
                    var res = JSON.parse(response.PETemplate_JSON);
                    var resTemplate = JSON.parse(res);
                    var data = [];
                    $.each(resTemplate, function (i, item) {
                        data.push({ id: item.PETemplateId, value: item.Name });
                        var li = PhysicalExamTemplatesRevamp.addTemplate(item.PETemplateId, item.Name);
                        $("#ulPhysicalExams").append(li);
                        if (PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId)
                            $("#ulPhysicalExams #PETemplate_" + PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId).addClass('active');
                    });

                    $("#Exams").kendoAutoComplete({
                        dataSource: data,
                        filter: "contains",
                        dataTextField: "value",
                        placeholder: "Select Exam...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulPhysicalExams").find('#PETemplate_' + dataItem.id).length == 0) {
                                var li = PhysicalExamTemplatesRevamp.addTemplate(dataItem.id, dataItem.value);
                                $("#ulPhysicalExams").append(li);
                            }
                            else {
                                utility.DisplayMessages(dataItem.value + " already exists.", 3);
                            }
                            $("#Exams").val('');
                            e.preventDefault();
                        },
                        footerTemplate: 'Total <strong>#: instance.dataSource.total() #</strong> items found'
                    });
                    $("#Exams").parent().addClass('size100per');
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            objDeffered.resolve();
        });
        return objDeffered.promise();
    },

    LoadPhysicalExamTemplatesRevamp: function (templateId) {
        PhysicalExamTemplatesRevamp.PhysicalExamTemplatesRevampLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                response.PhysicalExamTemplate = JSON.parse(response.PhysicalExamTemplate);

                if (response.PhysicalExamTemplate.length > 0) {
                    response.PhysicalExamTemplate = response.PhysicalExamTemplate[0];
                    PhysicalExamTemplatesRevamp.selectedPhyExamTempData = response.PhysicalExamTemplate.SysSecCharSubcharData;
                    //Start Farooq Ahmad 03-03-2016 mark green and checked the loaded System
                    for (var index in PhysicalExamTemplatesRevamp.selectedPhyExamTempData) {
                        //if ($.parseJSON(PhysicalExamTemplatesRevamp.selectedPhyExamTempData[index].IsSelected.toString().toLowerCase()))
                        //    $('#' + PhysicalExamTemplatesRevamp.params.PanelID + " #frmPhysicalExamTemplatesRevamp #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplatesRevamp.selectedPhyExamTempData[index].SystemId).addClass("green");
                        //else
                        //    $('#' + PhysicalExamTemplatesRevamp.params.PanelID + " #frmPhysicalExamTemplatesRevamp #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplatesRevamp.selectedPhyExamTempData[index].SystemId).removeClass("green");

                        $('#' + PhysicalExamTemplatesRevamp.params.PanelID + " #frmPhysicalExamTemplatesRevamp #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplatesRevamp.selectedPhyExamTempData[index].SystemId + " input:checkbox").prop("checked", $.parseJSON(PhysicalExamTemplatesRevamp.selectedPhyExamTempData[index].IsSelected.toString().toLowerCase()));
                        $('#' + PhysicalExamTemplatesRevamp.params.PanelID + " #frmPhysicalExamTemplatesRevamp #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplatesRevamp.selectedPhyExamTempData[index].SystemId + " label").attr("data-original-title", PhysicalExamTemplatesRevamp.selectedPhyExamTempData[index].SystemName);
                        $('#' + PhysicalExamTemplatesRevamp.params.PanelID + " #frmPhysicalExamTemplatesRevamp #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplatesRevamp.selectedPhyExamTempData[index].SystemId + " label").text(PhysicalExamTemplatesRevamp.selectedPhyExamTempData[index].SystemName);
                    }
                    //End Farooq Ahmad 03-03-2016 mark green and checked the loaded System
                    var self = $('#' + PhysicalExamTemplatesRevamp.params.PanelID + " #frmPhysicalExamTemplatesRevamp");

                    utility.bindMyJSONByName(true, response.PhysicalExamTemplate, false, self).done(function () {
                        $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #ddlPhysicalExamTemplateEntity').change();
                        PhysicalExamTemplatesRevamp.SpecialtyIds = response.PhysicalExamTemplate.SpecialtyIds;
                        PhysicalExamTemplatesRevamp.ProviderIds = response.PhysicalExamTemplate.ProviderIds;
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

    PhysicalExamSystemObservationsLoad: function (PESystemId, templateId) {
        var objData = new Object();
        objData["SystemId"] = PESystemId;
        objData["TemplateId"] = templateId;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "Load_PhyscialExam_System_Observations_Note";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    PhysicalExamTemplatesRevampLoad: function (templateId) {
        var objData = new Object();
        objData["TemplateId"] = templateId == undefined ? "0" : templateId;
        objData["commandType"] = "Load_PhyscialExam_Templates_ECW";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    PhysicalExamTemplatesRevampLoadOnDemand: function (templateId, systemId, sectionId, charId, commandType) {
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


    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
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
            currentId = PhysicalExamTemplatesRevamp.NewInsertedId--;
            var subcharacteristicExist = "";

            var saveMethodPE = "";

            if (itemType.toLowerCase() == "exam") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExams";
                ulControl = $('#' + PhysicalExamTemplatesRevamp.params.PanelID + " #frmPhysicalExamTemplatesRevamp #" + currentCtrlId);
                saveMethodPE = "PhysicalExamTemplatesRevamp.AddPhysicalExam(this, '" + currentId + "');";
            }
            else if (itemType.toLowerCase() == "system") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + PhysicalExamTemplatesRevamp.params.PanelID + " #frmPhysicalExamTemplatesRevamp #" + currentCtrlId);
                saveMethodPE = "PhysicalExamTemplatesRevamp.AddSystemPE(this, '" + currentId + "');";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "";
                currentCtrlId = "ulPhysicalExamSystemSection";
                ulControl = $('#' + PhysicalExamTemplatesRevamp.params.PanelID + " #frmPhysicalExamTemplatesRevamp #" + currentCtrlId);
                saveMethodPE = "PhysicalExamTemplatesRevamp.AddObservation(this, '" + currentId + "');";
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
                onClick = "";//"PhysicalExamTemplatesRevamp.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";
                var deleteFunction = "PhysicalExamTemplatesRevamp.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                liInnerText = '<div class="">' + deleteIcon + addSubCharIcon
                    + '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="">'
                    //+ '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="PhysicalExamTemplatesRevamp.selectParentControls(this);PhysicalExamTemplatesRevamp.toggleCheckBoxes(this);">'
                    + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                    + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                    + '<textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress="PhysicalExamTemplatesRevamp.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
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
        //rrrrrr
        var objData = new Object();
        objData["IsGlobal"] = false;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = true;
        objData["PETemplateID"] = $('#divPhysicalExams').find('.active').val();

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }
        else if (objData["PETemplateID"] == 0 || objData["PETemplateID"] == null || objData["PETemplateID"] == "" || objData["PETemplateID"] == undefined) {
            utility.DisplayMessages("Please select exam", 3);
            return;
        }

        PhysicalExamTemplatesRevamp.savePESystem_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);

                    var li = PhysicalExamTemplatesRevamp.addSystem(response.PESystemId, objData["Name"], '-1', response.PETemplateSystemId);
                    $("#ulPhysicalExamSystems").append(li);
                    $(li).trigger('click');

                    var objSelectedSystem = {
                        PESystemId: response.PESystemId, IsSelected: true, PETemplateId: objData["PETemplateID"],
                        SystemName: objData["Name"], PETemplateSystemId: response.PETemplateSystemId, SystemDescription: "",
                        PENotesObservationId: -1, IsCurrent: true
                    };

                    PhysicalExamTemplatesRevamp.selectedSystems.push(objSelectedSystem);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                $("#" + controlId).remove();
            }
        });
    },

    AddObservation: function (obj, controlId, autoCompName) {        //rrrrr

        //var PESystemId = PhysicalExamTemplatesRevamp.getCurrentSystemId();//$("#ulPhysicalExamSystems li.active")[0].id;
        //if (PESystemId == "")
        var PESystemId = PhysicalExamTemplatesRevamp.selectedSystemID;

        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        //objData["PETemplateSystemId"] = PhysicalExamTemplatesRevamp.getCurrentTemplateSystemId();
        objData["PETemplateSystemId"] = PhysicalExamTemplatesRevamp.selectedTemplateSystemID;

        if (autoCompName)
            objData["Name"] = autoCompName;
        else
            objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = 1;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }
        else if (objData["PESystemId"] == 0 || objData["PESystemId"] == null || objData["PESystemId"] == "" || objData["PESystemId"] == undefined) {
            utility.DisplayMessages("Please select system", 3);
            return;
        }
        else if (objData["PETemplateSystemId"] == 0 || objData["PETemplateSystemId"] == null || objData["PETemplateSystemId"] == "" || objData["PETemplateSystemId"] == undefined) {
            utility.DisplayMessages("PETemplateSystemId not found", 3);
            return;
        }

        var examId = $('#divPhysicalExamSystems').find('li').first().attr('parentid');
        objData["ExamID"] = examId;
        var PETemplateSystemId = $("#ulPhysicalExamSystems #" + PESystemId).attr('petemplatesystemid');
        PhysicalExamTemplatesRevamp.savePEObservation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    response = JSON.parse(response.PEObservation_JSON);
                    res = response[0];
                    PhysicalExamTemplatesRevamp.params.PEObservationId = res.PEObservationId;
                    if (res.PEObservationId) {

                        var li = PhysicalExamTemplatesRevamp.addObservations(res.PEObservationId, objData["Name"], PESystemId, null);
                        $("#ulPhysicalExamSystemSection").append(li);

                        var objSelectedObservations = {
                            PESystemId: PESystemId, IsSelected: true, ObservationId: res.PEObservationId,
                            ObservationName: objData["Name"], IsSystemChecked: true, PESystemObservationId: res.PESystemObservationId, PETemplateSystemId: PETemplateSystemId
                        };
                        PhysicalExamTemplatesRevamp.selectedObservations.push(objSelectedObservations);
                    }
                }
                else
                    utility.DisplayMessages(response.Message, 3);
                $("#" + controlId).remove();
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
            onClickFunction = onClickFunction.replace('this', "$('#" + PhysicalExamTemplatesRevamp.params.PanelID + " #" + ULID + " #" + ID + "')");
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
        if (ctrlId == "ulPhysicalExams") {
            $("#" + currentId).remove();

        }
        else if (ctrlId == "ulPhysicalExamSystems") {
            $("#" + currentId).remove();

        } else if (ctrlId == "ulPhysicalExamSystemSection") {
            $("#" + currentId).remove();
        }
    },

    validateSelectedTemplateData: function () {

        var isValid = true;

        if ($(PhysicalExamTemplatesRevamp.selectedPhyExamTempData).length > 0) {
            $.each(PhysicalExamTemplatesRevamp.selectedPhyExamTempData, function (i, item) {


                if (item.IsModified == "1" && $.parseJSON(item.IsSelected.toString().toLowerCase()) == true) {

                    //isValid = false;

                    //if () {

                    isValid = false;

                    if ($(item.Sections).length > 0) {

                        $.each(item.Sections, function (i, item) {

                            isValid = false;

                            if (item.IsModified == "1" && $.parseJSON(item.IsSelected.toString().toLowerCase()) == true) {


                                if ($(item.Characteristics).length > 0) {

                                    $.each(item.Characteristics, function (counter, item) {

                                        if (item.IsModified == "1" && $.parseJSON(item.IsSelected.toString().toLowerCase()) == true) {
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
        self = $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #frmPhysicalExamTemplatesRevamp');

        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        objData.SpecialtyIds = '';//objData.PhysicalExamTemplateSpecialty = PhysicalExamTemplatesRevamp.specialityCheckedIds.join();
        objData.ProviderIds = ''; //objData.PhysicalExamTemplateProvider = PhysicalExamTemplatesRevamp.providerCheckedIds.join();

        var isStillValid = false;

        //if (PhysicalExamTemplatesRevamp.validateSelectedTemplateData())
        //{
        myJSON = JSON.stringify(objData);
        PhysicalExamTemplatesRevamp.savePhysicalExamTemplate(myJSON).done(function (response) {
            if (response != null && response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    if (response.phyExamTemplateId != "") {
                        PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId = response.phyExamTemplateId;
                        for (var count in PhysicalExamTemplatesRevamp.selectedPhyExamTempData) {
                            PhysicalExamTemplatesRevamp.selectedPhyExamTempData[count];
                        }
                    }
                    //PhysicalExamTemplatesRevamp.loadHospitalizationHx();

                    PhysicalExamTemplatesRevamp.params.mode = "Edit";
                    if (PhysicalExamTemplatesRevamp.params.ParentCtrl == "clinicalTabProgressNote" && UnloadHospitalizationhx == true) {
                        PhysicalExamTemplatesRevamp.getHospitalizationHxInfo(HospitalizationHxType, UnloadHospitalizationhx);
                    }
                    $('#' + PhysicalExamTemplatesRevamp.params.PanelID + " #hfHospitalizationHxId").val(response.HospitalizationHxId);
                    $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #frmClinicalHospitalizationHx').data('serialize', $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #frmClinicalHospitalizationHx').serialize());
                    //

                    // Empty global variables
                    PhysicalExamTemplatesRevamp.selectedPhyExamTempData = [];

                    //Start Farooq Ahmad 16-03-2016 Unload the Physical Exam on Save
                    UnloadActionPan(PhysicalExamTemplatesRevamp.params["ParentCtrl"], "PhysicalExamTemplatesRevamp");
                    if (PhysicalExamTemplate != null) {
                        PhysicalExamTemplate.loadPhysicalExamTemplate();
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
        self = $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #frmPhysicalExamTemplatesRevamp');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if (PhysicalExamTemplatesRevamp.params["mode"] == "Edit") {
            objData["TemplateId"] = (PhysicalExamTemplatesRevamp.params["PhysicalExamTemplateId"]);
        }
        else {
            objData["TemplateId"] = '-1';
        }

        if (TemplateName != null) {

            var mainTemplateName = $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #frmPhysicalExamTemplatesRevamp #txtPhysicalExamTemplateName').val();

            if (objData["TemplateId"] == '-1') {
                objData["PhysicalExamTemplateName"] = TemplateName;
                IsActive = "1";
            }
            else {
                objData["PhysicalExamTemplateName"] = mainTemplateName != "" ? mainTemplateName : TemplateName;
            }

            objData["SaveAsTemplateName"] = TemplateName;
        }

        objData["SpecialtyIds"] = objData["PhysicalExamTemplateSpecialty"];
        objData["ProviderIds"] = objData["PhysicalExamTemplateProvider"];
        objData.SpecialtyIds = '';
        objData.ProviderIds = '';
        objData["commandType"] = "Save_PhyscialExam_ECW";
        objData["SystemObservationData"] = PhysicalExamTemplatesRevamp.selectedObservations;


        IsActive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }

        objData["IsActive"] = IsActive;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    validatePhysicalExamTemplate: function () {
        $('#' + PhysicalExamTemplatesRevamp.params.PanelID + ' #frmPhysicalExamTemplatesRevamp')
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
            PhysicalExamTemplatesRevamp.physicalExamTemplateSave();
        });
    },

    saveAsPhysicalExam: function () {
        var strMessage = "";
        var params = [];
        params["FromAdmin"] = '0';
        params["TabID"] = "PhysicalExamTemplateSaveAs";
        params["ParentCtrl"] = 'PhysicalExamTemplatesRevamp';


        LoadActionPan('PhysicalExamTemplateSaveAs', params, PhysicalExamTemplatesRevamp.params.PanelID);

    },

    unLoadTab: function (nextOrPre, controlToInvoke) {
        PhysicalExamTemplatesRevamp.controlToInvoke = controlToInvoke;
        if (PhysicalExamTemplatesRevamp.params.ParentCtrl == "clinicalTabProgressNote") {
            utility.myConfirmNote('1', function () {
                if (nextOrPre == true) {
                    PhysicalExamTemplatesRevamp.addPhysicalExamToNotes();
                    // Empty global variables
                    PhysicalExamTemplatesRevamp.resetForm();
                    UnloadActionPan(PhysicalExamTemplatesRevamp.params.ParentCtrl, 'PhysicalExamTemplatesRevamp');
                    if (PhysicalExamTemplatesRevamp.controlToInvoke != null) {

                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(PhysicalExamTemplatesRevamp.controlToInvoke.replace(/\s/g, ''));
                            PhysicalExamTemplatesRevamp.controlToInvoke = null;
                        }, 400);

                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds != "" || Clinical_ProgressNote.DetachedNoteComponentIds != "") {
                        PhysicalExamTemplatesRevamp.addPhysicalExamToNotes();
                        PhysicalExamTemplatesRevamp.resetForm();
                        UnloadActionPan(PhysicalExamTemplatesRevamp.params.ParentCtrl, 'PhysicalExamTemplatesRevamp');
                    } else {
                        PhysicalExamTemplatesRevamp.addPhysicalExamToNotes();
                        PhysicalExamTemplatesRevamp.resetForm();
                        UnloadActionPan(PhysicalExamTemplatesRevamp.params.ParentCtrl, 'PhysicalExamTemplatesRevamp');
                    }

                }
            },
            "",
            function () {
                // Empty global variables
                PhysicalExamTemplatesRevamp.resetForm();
                UnloadActionPan(PhysicalExamTemplatesRevamp.params.ParentCtrl, 'Clinical_Vitals');
                if (nextOrPre == true) {
                    if (PhysicalExamTemplatesRevamp.controlToInvoke != null) {

                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(PhysicalExamTemplatesRevamp.controlToInvoke.replace(/\s/g, ''));
                            PhysicalExamTemplatesRevamp.controlToInvoke = null;
                        }, 400);

                    }
                }
            });


        }
        else {
            // Empty global variables
            PhysicalExamTemplatesRevamp.resetForm();
            UnloadActionPan(PhysicalExamTemplatesRevamp.params["ParentCtrl"], "PhysicalExamTemplatesRevamp");
        }
    },

    PhysicalExamActiveTemplatesLoad: function (templateId) {
        var objData = new Object();
        objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId
        objData["commandType"] = "Load_PhyscialExam_ForProvider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    addTemplate: function (PETemplateId, TemplateName) {
        var itemtoRemove = "Template";
        var li = '<li id="PETemplate_' + PETemplateId + '" parentid="null" onclick="PhysicalExamTemplatesRevamp.loadSystems(' + PETemplateId + ',this,event)" value="' + PETemplateId + '" refvalue="" subcharacteristicexist=" "><div class="">' +
                 '<label id="lblTemplateName_' + PETemplateId + '" class="" data-toggle="tooltip" title="" data-original-title="' + TemplateName + '">' + TemplateName + '</label><div id="divNameDetail' + PETemplateId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PETemplateId + '" onkeypress="" name="Name' + PETemplateId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PETemplateId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
        return li;
    },
    RemoveTemplate: function (PETemplateId, event) {
        if (event && event != null)
            event.stopPropagation();
        PhysicalExamTemplatesRevamp.ScrollTabObj.removeTabs($("#ulSelectedPETemplates").find('#listPETemplate_' + PETemplateId));
        $(PhysicalExamTemplatesRevamp.selectedSystems).filter(function (ind, i) {
            if (i.PETemplateId == PETemplateId) {
                i.IsSelected = false;
                i.PENotesObservationId = "0";
            }
        });
        if ($("#ulSelectedPETemplates [id^='listPETemplate_']").length > 0) {
            if ($("#ulSelectedPETemplates [id^='listPETemplate_'].active").length == 0) {
                var $li = $("#ulSelectedPETemplates [id^='listPETemplate_']")[0];
                var id = $($li).attr('id');
                id = id.replace('listPETemplate_', "");
                $("#ulSelectedPETemplates").find('#listPETemplate_' + id).addClass('active');
                $("#ulPhysicalExams").find('#PETemplate_' + id).addClass('active');
                PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId = id;
                $("#ulPhysicalExams").find('#PETemplate_' + id).trigger('click');
            }
        }
        else
            PhysicalExamTemplatesRevamp.clearTemplateSelection();
    },
    removeSystemAndObservations: function (PETemplateId) {
        var result = PhysicalExamTemplatesRevamp.selectedSystems.filter(function (obj) {
            return obj.PETemplateId == PETemplateId;
        });
        PhysicalExamTemplatesRevamp.selectedSystems = PhysicalExamTemplatesRevamp.selectedSystems.filter(function (obj) {
            return obj.PETemplateId != PETemplateId;
        });
        if (result && result.length > 0) {
            PhysicalExamTemplatesRevamp.selectedObservations = PhysicalExamTemplatesRevamp.selectedObservations.filter(function (obj) {
                return obj.PESystemId != result[0].PESystemId;
            });
        }
    },
    loadSystems: function (PETemplateId, obj) {
        var PESystemId = PhysicalExamTemplatesRevamp.getCurrentSystemId();
        var PETemplateSystemId = PhysicalExamTemplatesRevamp.getCurrentTemplateSystemId();
        if (PETemplateSystemId)
            PhysicalExamTemplatesRevamp.FillObservationContent(PETemplateSystemId);
        PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId = PETemplateId;
        PhysicalExamTemplatesRevamp.clearTemplateSelection();
        var isExist = false;
        var isSelected = false;
        $("#ulSelectedPETemplates").find('#listPETemplate_' + PETemplateId).addClass('active');
        $("#ulPhysicalExams").find('#PETemplate_' + PETemplateId).addClass('active');
        if ($("#ulSelectedPETemplates").find('#listPETemplate_' + PETemplateId).length == 0) {
            var TemplateName = $(obj).find('#lblTemplateName_' + PETemplateId).text();
            // $("#ulSelectedPETemplates").append('<li id="listPETemplate_' + PETemplateId + '" onclick="PhysicalExamTemplatesRevamp.loadSystems(' + PETemplateId + ',this)" class="active"><a id="lnkPETemplate_' + PETemplateId + '" class="" href="#" data-toggle="tab">' + TemplateName + '</a></li>')
            PhysicalExamTemplatesRevamp.ScrollTabObj.addTab('<li id="listPETemplate_' + PETemplateId + '" onclick="PhysicalExamTemplatesRevamp.loadSystems(' + PETemplateId + ',this)" class="active"><a id="lnkPETemplate_' + PETemplateId + '" class="" href="#" data-toggle="tab">' + TemplateName + '</a><button type="button" onclick="PhysicalExamTemplatesRevamp.RemoveTemplate(' + PETemplateId + ',event)" class="closetbutton" aria-label="Close"><span aria-hidden="true">×</span></button></li>');
        }
        if (PhysicalExamTemplatesRevamp.selectedSystems) {
            for (var i = 0 ; i < PhysicalExamTemplatesRevamp.selectedSystems.length; i++) {
                if (PhysicalExamTemplatesRevamp.selectedSystems[i].PETemplateId == PETemplateId) {
                    isExist = true;
                    break;
                }
                PhysicalExamTemplatesRevamp.selectedSystems[i].IsCurrent = false;
            }
        }
        $("#System").removeClass('hidden');
        $("#ulPhysicalExamSystems li").remove();

        if (!isExist) {
            PhysicalExamTemplatesRevamp.PhysicalExamTemplatesRevampLoad(PETemplateId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.PETemplateSystemsCount > 0) {
                        var res = JSON.parse(response.PETemplateSystems_JSON);
                        var resSystems = JSON.parse(res);
                        var promises = [];

                        $.each(resSystems, function (i, item) {
                            //$(PhysicalExamTemplatesRevamp.selectedSystems).each(function (ind, i) {
                            //    i.IsCurrent = false;
                            //});
                            var li = PhysicalExamTemplatesRevamp.addSystem(item.PESystemId, item.SystemName, PETemplateId, item.PETemplateSystemId);
                            $("#ulPhysicalExamSystems").append(li);
                            var def = PhysicalExamTemplatesRevamp.loadObservations(item.PESystemId, $("#ulPhysicalExamSystems #" + item.PESystemId, PETemplateId));
                            isSelected = item.IsSelected == "True" ? true : false
                            var objSelectedSystem = {
                                PESystemId: item.PESystemId, IsSelected: item.IsSelected == "True" ? true : false, PETemplateId: PETemplateId, SystemName: item.SystemName, PETemplateSystemId: item.PETemplateSystemId,
                                SystemDescription: item.SystemDescription, PENotesObservationId: -1,
                                IsCurrent: i == 0 ? true : false
                            };
                            if (isSelected) {
                                $("#ulPhysicalExamSystems li[id='" + item.PESystemId + "']").addClass('text-success');
                            }
                            else
                                $("#ulPhysicalExamSystems li[id='" + item.PESystemId + "']").removeClass('text-success');

                            $("#ulPhysicalExamSystems li #chk" + item.PETemplateSystemId + "").prop("checked", isSelected);

                            PhysicalExamTemplatesRevamp.selectedSystems.push(objSelectedSystem);
                            promises.push(def);
                        });
                        $.when.apply(undefined, promises).done(function () {
                            PhysicalExamTemplatesRevamp.selectFirstSystem();
                        });
                    }
                }
            });
        }
        else {
            if (PhysicalExamTemplatesRevamp.selectedSystems) {
                var promises = [];

                for (var i = 0 ; i < PhysicalExamTemplatesRevamp.selectedSystems.length; i++) {
                    if (PhysicalExamTemplatesRevamp.selectedSystems[i].PETemplateId == PETemplateId) {
                        var li = PhysicalExamTemplatesRevamp.addSystem(PhysicalExamTemplatesRevamp.selectedSystems[i].PESystemId, PhysicalExamTemplatesRevamp.selectedSystems[i].SystemName, PETemplateId, PhysicalExamTemplatesRevamp.selectedSystems[i].PETemplateSystemId);

                        if (PhysicalExamTemplatesRevamp.selectedSystems[i].PESystemId != "" && PhysicalExamTemplatesRevamp.selectedSystems[i].PESystemId != -1)
                            $("#ulPhysicalExamSystems").append(li);
                        var isSelected = PhysicalExamTemplatesRevamp.selectedSystems[i].IsSelected;
                        if (isSelected) {
                            $("#ulPhysicalExamSystems li[id='" + PhysicalExamTemplatesRevamp.selectedSystems[i].PESystemId + "']").addClass('text-success');
                        }
                        else
                            $("#ulPhysicalExamSystems li[id='" + PhysicalExamTemplatesRevamp.selectedSystems[i].PESystemId + "']").removeClass('text-success');
                        $("#ulPhysicalExamSystems li #chk" + PhysicalExamTemplatesRevamp.selectedSystems[i].PETemplateSystemId + "").prop("checked", isSelected);
                        var def = PhysicalExamTemplatesRevamp.loadObservations(PhysicalExamTemplatesRevamp.selectedSystems[i].PESystemId, $("#ulPhysicalExamSystems #" + PhysicalExamTemplatesRevamp.selectedSystems[i].PESystemId, PhysicalExamTemplatesRevamp.selectedSystems[i].PETemplateId));
                        promises.push(def);
                    }
                    else {
                        var def = $.Deferred();
                        def.resolve();
                        promises.push(def);
                    }
                }
                $.when.apply(undefined, promises).done(function () {
                    PhysicalExamTemplatesRevamp.selectFirstSystem();
                });
            }
        }
    },
    RemoveSystemDB_Call: function (PETemplateSystemId) {
        var objData = new Object();
        objData["PETemplateSystemId"] = PETemplateSystemId;
        objData["commandType"] = "RemoveTemplate_System";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },
    RemoveObservationDB_Call: function (PESystemObservationId) {
        var objData = new Object();
        objData["PESystemObservationId"] = PESystemObservationId;
        objData["commandType"] = "delete_physicalexam_system_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },
    FillPhysicalExamTemplates: function () {
        // PhysicalExamTemplatesRevamp.loadSystems(PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId);
        $("#ulPhysicalExams li").removeClass('active');
        $("#ulSelectedPETemplates li").removeClass('active');
        PhysicalExamTemplatesRevamp.fillPhysicalExamForSoap(PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                // PE Note Templates
                PETemplate_JSON = JSON.parse(JSON.parse(response.PETemplate_JSON));
                var PETemplateId = "";
                $.each(PETemplate_JSON, function (i, item) {
                    if (item.NotesId && item.NotesId == PhysicalExamTemplatesRevamp.params.NotesId) {
                        PETemplateId = item.PETemplateId;
                        $("#ulPhysicalExams li").removeClass('active');
                        $("#ulSelectedPETemplates li").removeClass('active');
                        $("#ulSelectedPETemplates").find('#listPETemplate_' + PETemplateId).addClass('active');
                        $("#ulPhysicalExams").find('#PETemplate_' + PETemplateId).addClass('active');
                        if ($("#ulSelectedPETemplates").find('#listPETemplate_' + PETemplateId).length == 0) {
                            var TemplateName = item.TemplateName;
                            PhysicalExamTemplatesRevamp.ScrollTabObj.addTab('<li id="listPETemplate_' + PETemplateId + '" onclick="PhysicalExamTemplatesRevamp.loadSystems(' + PETemplateId + ',this)" class="active"><a id="lnkPETemplate_' + PETemplateId + '" class="" href="#" data-toggle="tab">' + TemplateName + '</a><button type="button" onclick="PhysicalExamTemplatesRevamp.RemoveTemplate(' + PETemplateId + ',event)" class="closetbutton" aria-label="Close"><span aria-hidden="true">×</span></button></li>')
                        }
                        PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId = PETemplateId;
                    }
                });

                // PE Systems
                PETemplateSystems_JSON = JSON.parse(JSON.parse(response.PETemplateSystems_JSON));
                var PESystemId = "";
                var PETemplateSystemId = "";
                if (response.PETemplateSystemsCount > 0) {
                    var resTemplateSystems = JSON.parse(response.PETemplateSystems_JSON);
                    var SystemData = JSON.parse(resTemplateSystems);
                    for (var i = 0; i < SystemData.length; i++) {

                        var isSelected = SystemData[i].IsSelected == "True" ? true : false;
                        var objSelectedSystem = {
                            PESystemId: SystemData[i].PESystemId, IsSelected: isSelected, PETemplateId: SystemData[i].PETemplateId, SystemName: SystemData[i].SystemName, PETemplateSystemId: SystemData[i].PETemplateSystemId, SystemDescription: SystemData[i].SystemDescription, PENotesObservationId: SystemData[i].PENotesObservationId, IsCurrent: false
                        };
                        PhysicalExamTemplatesRevamp.selectedSystems.push(objSelectedSystem);

                        if (SystemData[i].PETemplateId == PhysicalExamTemplatesRevamp.params.SelectedPETemplateId) {
                            var li = PhysicalExamTemplatesRevamp.addSystem(SystemData[i].PESystemId, SystemData[i].SystemName, SystemData[i].PETemplateId, SystemData[i].PETemplateSystemId);
                            $("#ulPhysicalExamSystems").append(li);
                            $("#ulPhysicalExamSystems #chk" + SystemData[i].PETemplateSystemId).prop("checked", true);
                            $("#ulPhysicalExamSystems #" + SystemData[i].PESystemId).addClass('active');
                            PESystemId = SystemData[i].PESystemId;
                            PETemplateSystemId = SystemData[i].PETemplateSystemId;
                        }

                    }

                    //PE Observations
                    if (response.PESysObservationsCount > 0) {
                        var resTemplateObservations = JSON.parse(response.PESysObservations_JSON);
                        var ObservationData = JSON.parse(resTemplateObservations);
                        for (var j = 0; j < ObservationData.length; j++) {

                            var objSelectedObservations = {};
                            if (ObservationData[j].IsSelected == "True") {
                                $("#ulPhysicalExamSystemSection #chk" + ObservationData[j].PEObservationId).prop("checked", true);
                                if (SystemData[i].IsSelectedSystem == "True")
                                    objSelectedObservations = { PESystemId: ObservationData[j].PESystemId, IsChecked: true, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: true, IsSystemDeleted: 0, IsObservationDeleted: 0, PETemplateSystemId: PETemplateSystemId };
                                else
                                    objSelectedObservations = { PESystemId: ObservationData[j].PESystemId, IsChecked: true, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0, PETemplateSystemId: PETemplateSystemId };
                            }
                            else {
                                $("#ulPhysicalExamSystemSection #chk" + ObservationData[j].PEObservationId).prop("checked", false);
                                if (SystemData[i].IsSelectedSystem == "True")
                                    objSelectedObservations = { PESystemId: ObservationData[j].PESystemId, IsChecked: false, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: true, IsSystemDeleted: 0, IsObservationDeleted: 0, PETemplateSystemId: PETemplateSystemId };
                                else
                                    objSelectedObservations = { PESystemId: ObservationData[j].PESystemId, IsChecked: false, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0, PETemplateSystemId: PETemplateSystemId };
                            }

                            PhysicalExamTemplatesRevamp.selectedObservations.push(objSelectedObservations);

                            if (PETemplateSystemId == ObservationData[j].PETemplateSystemId) {
                                var li = PhysicalExamTemplatesRevamp.addObservations(ObservationData[j].PEObservationId, ObservationData[j].ObservationName, ObservationData[j].PESystemId, PESystemObservationId);
                                $("#ulPhysicalExamSystemSection").append(li);
                            }
                        }
                    }

                }
                if (PhysicalExamTemplatesRevamp.params.SelectedPETemplateId) {
                    $("#ulPhysicalExams li").removeClass('active');
                    $("#ulSelectedPETemplates li").removeClass('active');
                    $("#ulSelectedPETemplates").find('#listPETemplate_' + PhysicalExamTemplatesRevamp.params.SelectedPETemplateId).addClass('active');
                    $("#ulPhysicalExams").find('#PETemplate_' + PhysicalExamTemplatesRevamp.params.SelectedPETemplateId).addClass('active');
                    $("#ulSelectedPETemplates").find('#listPETemplate_' + PhysicalExamTemplatesRevamp.params.SelectedPETemplateId).trigger('click');
                }
                else {
                    $("#ulSelectedPETemplates").find('#listPETemplate_' + PETemplateId).trigger('click');
                }

                //var deli = $("#delimator option:selected").text() + " ";
                //$("#observationContent").val(PhysicalExamTemplatesRevamp.selectedSystems[PhysicalExamTemplatesRevamp.selectedSystems.length - 1].SystemDescription);
                //PhysicalExamTemplatesRevamp.selectedSystems[PhysicalExamTemplatesRevamp.selectedSystems.length - 1].IsCurrent = true;

            }
        });
    },
    ///########### Start Progress Notes################///

    addPhysicalExamToNotes: function (isCallFromNote) {
        PhysicalExamTemplatesRevamp.updateSystemDescription();
        var PESysId = PhysicalExamTemplatesRevamp.getCurrentSystemId();
        var PETemplateSystemId = PhysicalExamTemplatesRevamp.getCurrentTemplateSystemId();
        if (PETemplateSystemId)
            PhysicalExamTemplatesRevamp.FillObservationContent(PETemplateSystemId, true);
        var PETemplateSystems = [];
        for (var i = 0 ; i < PhysicalExamTemplatesRevamp.selectedSystems.length; i++) {
            if (PhysicalExamTemplatesRevamp.selectedSystems[i].IsSelected == true) {
                var action = PhysicalExamTemplatesRevamp.selectedSystems[i].PENotesObservationId == "-1" ? "-1" : "0";//-1:for Insert,0 for update , 1=delete
                var objPETempSysObservation = {
                    PENotesObservationId: PhysicalExamTemplatesRevamp.selectedSystems[i].PENotesObservationId,
                    PETemplateSystemId: PhysicalExamTemplatesRevamp.selectedSystems[i].PETemplateSystemId,
                    NotesId: Clinical_ProgressNote.params.NotesId,
                    Descr: utility.decodeHtml(PhysicalExamTemplatesRevamp.selectedSystems[i].SystemDescription),
                    Action: action
                }
                PETemplateSystems.push(objPETempSysObservation);
            }
            if (PhysicalExamTemplatesRevamp.selectedSystems[i].PENotesObservationId != "-1" && PhysicalExamTemplatesRevamp.selectedSystems[i].IsSelected == false) {
                var objPETempSysObservation = {
                    PENotesObservationId: PhysicalExamTemplatesRevamp.selectedSystems[i].PENotesObservationId,
                    PETemplateSystemId: PhysicalExamTemplatesRevamp.selectedSystems[i].PETemplateSystemId,
                    NotesId: Clinical_ProgressNote.params.NotesId,
                    Descr: utility.decodeHtml(PhysicalExamTemplatesRevamp.selectedSystems[i].SystemDescription),
                    Action: "1"
                }
                PETemplateSystems.push(objPETempSysObservation);
            }
        }
        var PETempSysObservationIds = $(PETemplateSystems).map(function () {
            return this.PETempSysObservationId;
        }).get().join(',');
        if (PETemplateSystems.length > 0) {
            PhysicalExamTemplatesRevamp.addPhysicalExamToNotesDB_Call(PETempSysObservationIds, PETemplateSystems, Clinical_ProgressNote.params.NotesId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    PhysicalExamTemplatesRevamp.Content_style = $("#" + PhysicalExamTemplatesRevamp.params.PanelID + " #observationContent").attr('style');
                    PhysicalExamTemplatesRevamp.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');

                    PhysicalExamTemplatesRevamp.selectedPhyExamTempData = [];
                    PhysicalExamTemplatesRevamp.ProviderIds = "";
                    PhysicalExamTemplatesRevamp.selectedObservations = [];
                    PhysicalExamTemplatesRevamp.selectedSystems = [];
                    PhysicalExamTemplatesRevamp.ProviderIds = "";
                    PhysicalExamTemplatesRevamp.normalSystemIdsGlobel = [];
                    PhysicalExamTemplatesRevamp.NewInsertedId = -1;
                    PhysicalExamTemplatesRevamp.selectedSystems = [];

                    UnloadActionPan(PhysicalExamTemplatesRevamp.params["ParentCtrl"], "PhysicalExamTemplatesRevamp");

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            utility.DisplayMessages("There is no data to Add.", 2);
        }
    },

    addPhysicalExamToNotesDB_Call: function (PETempSysObservationIds, PETemplateSystems, NotesId) {
        var objV = new Object();
        objV["PETempSysObservationIds"] = PETempSysObservationIds;
        objV["NotesObservationList"] = PETemplateSystems;
        objV["NotesId"] = NotesId;
        objV["commandType"] = "Save_PhysExam_NotesObservation";
        var datav = JSON.stringify(objV);
        return MDVisionService.APIService(datav, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    checkPhysicalExamExists: function () {
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML section[id*="Cli_PhysicalExam_Main"]').parent().remove();
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML section[id*="Cli_PhysicalExam_Main"]').remove();
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="PhysicalExamComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_PhysicalExam title="Physical Exam"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'PhysicalExam\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="PhysicalExam">Physical Exam</a> ' +
                       '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'PhysicalExam\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'PhysicalExam\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_PhysicalExam> </header></li>');
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
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());

    },

    detachPhysicalExamFromNotes: function (PhysicalExamId) {
        var strMessage = "";
        if (strMessage == "") {
            utility.myConfirm('29', function () {
                EMRUtility.scrollToPNcomponent('clinical_physicalexam');
                var selectedValue = PhysicalExamId.replace('Cli_PhysicalExam_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    PhysicalExamTemplatesRevamp.DetachPhysicalExamFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + PhysicalExamId).remove();
                            Clinical_ProgressNote.saveComponentSOAPText('Physical Exam');
                            Clinical_ProgressNote.hoverFunction();
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () {
            },
                '29'
            );
        }
        else
            utility.DisplayMessages(strMessage, 2);
    },
    DetachPhysicalExamFromNotes_DBCall: function (PhysicalExamId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        if (!PhysicalExamId)
            PhysicalExamId = 0;
        objData["PETemplateId"] = PhysicalExamId;
        objData["commandType"] = "Detach_PE_From_Notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    //This Function detach Physical Exam From progress note
    detach_ComponentsPhysicalExam: function (ComponentName, IsUpdate, PhysicalExamComponentRemove, PETemplateId) {

        var Clinical_PhysicalExamIds = "";
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .PhysicalExamComponent').attr('NoteComponentId');
        if (PETemplateId) {
            Clinical_PhysicalExamIds = PETemplateId;
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').parent().parent().find('section[id="Cli_PhysicalExam_Main' + PETemplateId + '"]').remove();
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').parent().parent().find('section[id="Cli_PhysicalExam_Main_Sys' + PETemplateId + '"]').remove();
        }
        else {
            Clinical_PhysicalExamIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').parent().parent().find('section[id*="Cli_PhysicalExam_Main"]').map(function () {
                return this.id.replace("Cli_PhysicalExam_Main", "");
            }).get().join(',');

            if (PhysicalExamComponentRemove) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Physical Exam']").remove();
                if (Clinical_ProgressNote.params["TemplateName"])
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').parent().parent().addClass('hidden');
                else
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').parent().parent().remove();
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').parent().parent().find('section[id*="Cli_PhysicalExam_Main"]').remove();
            } else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').parent().parent().find('section[id*="Cli_PhysicalExam_Main"]').remove();
            }
        }
        if (Clinical_PhysicalExamIds == "" || Clinical_PhysicalExamIds == "undefined") {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_PhysicalExam').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Physical Exam', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_PhysicalExam').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            Clinical_ProgressNote.hoverFunction();
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            PhysicalExamTemplatesRevamp.DetachPhysicalExamFromNotes_DBCall(PETemplateId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        if (PhysicalExamComponentRemove) {
                            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                                var promise = [];
                                if (Clinical_ProgressNote.params["TemplateName"]) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_PhysicalExam').parent().parent().addClass('hidden');
                                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Physical Exam', true))
                                }
                                else
                                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                                $.when.apply($, promise).done(function () {
                                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_PhysicalExam').parent().parent().remove();
                                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                                });
                            }
                        } else {
                            Clinical_ProgressNote.saveComponentSOAPText('Physical Exam', true);
                            Clinical_ProgressNote.hoverFunction();
                        }
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //this function will get Physical Exam Soap Text and attach that to Progress note
    getPhysicalExamInfo: function (PhysicalExamType, UnloadPhysicalExam, physicalExamId) {
        PhysicalExamTemplatesRevamp.fillPhysicalExamForSoap(physicalExamId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    //PhysicalExamTemplatesRevamp.Content_style = $("#" + PhysicalExamTemplatesRevamp.params.PanelID + " #observationContent").attr('style');
                    PhysicalExamTemplatesRevamp.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadPhysicalExam);
                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },
    fillPhysicalExamForSoap: function (physicalExamId) {
        var objData = new Object();
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "Fill_PE_For_Notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },
    createPhysicalExamBodyHTML: function (response, NoteHTMLCtrl, UnloadPhysicalExam, bNotSaveCompt) {
        PhysicalExamTemplatesRevamp.checkPhysicalExamExists();
        if (response.PETemplate_JSON != null & response.PETemplate_JSON != '') {
            var $mainDivVital = $(document.createElement('section'));
            $mainDivVital.attr('class', 'pl-none');
            PETemplate_JSON = JSON.parse(JSON.parse(response.PETemplate_JSON));
            PETemplateSystems_JSON = JSON.parse(JSON.parse(response.PETemplateSystems_JSON));
            $.each(PETemplate_JSON, function (i, item) {
                if (item.NotesId && item.NotesId == Clinical_ProgressNote.params.NotesId) {
                    var $ListVital = $(document.createElement('ul'));
                    $ListVital.attr('class', 'list-unstyled');
                    var PETemplateId = item.PETemplateId;
                    var $SectionBodyVital = $(document.createElement('section'));
                    $SectionBodyVital.attr('id', "Cli_PhysicalExam_Main" + PETemplateId);
                    var display = "";
                    (globalAppdata["IsPETemplateNameRequired"] && globalAppdata["IsPETemplateNameRequired"].toLowerCase() == "true") ? display = "" : display = "none";
                    $($ListVital).append('<li><header id="PhysicalExamSoapSystems" class="pl-none">' +
                                         '<Cli_PhysicalExam_' + PETemplateId + ' title="Physical Exam"  id="' + PETemplateId + '">' +
                                         '<a class="btn btn-link btn-xs pl-none" style="color:#bd0e09 !important; display:' + display + '"  onclick="Clinical_ProgressNote.OpenPhysicalExamTemplateAddEditMK(' + PETemplateId + ');" title="PhysicalExam">' + item.TemplateName + '</a> ' +
  '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'PhysicalExam\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ',null,null,' + PETemplateId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                    '</Cli_PhysicalExam_' + PETemplateId + '> </header><section style="display:none;"></section></li>');
                    $SectionBodyVital.append($ListVital);
                    $mainDivVital.append($SectionBodyVital);
                }
                $.each(PETemplateSystems_JSON, function (i, item) {
                    if (item.SystemDescription && item.SystemDescription != "") {
                        var PESystemId = item.PESystemId;
                        if (item.PETemplateId == PETemplateId) {
                            var $SysSec = $(document.createElement('section'));
                            $SysSec.attr('id', "Cli_PhysicalExam_Main_Sys" + PETemplateId);
                            var $Listsys = $(document.createElement('ul'));
                            $Listsys.attr('class', 'list-unstyled')
                            //$SysSec.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_PhysicalExam_" + PETemplateId + '"><i class="fa fa-edit"></i></a>' +
                            //                        '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_PhysicalExam_Main_Sys" + PETemplateId + '"  ><i class="fa fa-times"></i></a></div> ');
                            $Listsys.append('<li id="Comments_PhysicalExam"><div><a class="pull-left pl-none" style="color:#51b451 !important;margin-right: 5px;" onclick="Clinical_ProgressNote.OpenPhysicalExamSysObservationDetail(this,' + item.PETemplateSystemId + ',' + PESystemId + ');"'
                                + 'title="Physical Exam System">' + item.SystemName + ': </a> <span  id="Cli_PhysicalExam_NO_' + item.PENotesObservationId + '">' + utility.decodeHtml(item.SystemDescription) + '</span></div></li>');
                            $SysSec.append($Listsys);
                            $SysSec.append("<div class='clearfix'></div>");
                            $mainDivVital.append($SysSec);
                        }
                    }
                });
            });

            $(NoteHTMLCtrl + ' clinical_physicalexam').parent().parent().addClass('initialVisitBody  ml-none');
            $(NoteHTMLCtrl + ' clinical_physicalexam').parent().parent().append($mainDivVital);
            if (!bNotSaveCompt)
                Clinical_ProgressNote.saveComponentSOAPText('Physical Exam');
            Clinical_ProgressNote.hoverFunction();
        } else {
            if (!bNotSaveCompt)
                Clinical_ProgressNote.saveComponentSOAPText('Physical Exam');
            Clinical_ProgressNote.hoverFunction();
        }
    },

    updatePhysicalExamHtml: function (PhysicalExamHtml, PhysicalExamId, NoteHTMLCtrl) {
        $(NoteHTMLCtrl + ' clinical_PhysicalExam').parent().parent().addClass('initialVisitBody');
        var Edithandler = function (e) {
            Clinical_ProgressNote.EditNotesComments_ComponentAttached($(this).parent().next());
        };
        $(NoteHTMLCtrl + ' #Cli_PhysicalExam_Main' + PhysicalExamId + ' .btnPNC_Edit').unbind("click", Edithandler).bind("click", Edithandler);

        var Removehandler = function (e) {
            Clinical_ProgressNote.RemoveComponentAttached($(this).attr("name"));
        };
        $(NoteHTMLCtrl + ' #Cli_PhysicalExam_Main' + PhysicalExamId + ' .btnPNC_Remove').unbind("click", Removehandler).bind("click", Removehandler);

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();


    },

    ///########### End Progress Notes################///
    bindobservationAutoComplete: function () {
        var objDeffered = $.Deferred();
        PhysicalExamTemplatesRevamp.lookupPEObservation_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Observation_JSON = JSON.parse(response.Observation_JSON);
                    $('#' + PhysicalExamTemplatesRevamp.params.PanelID + " #Observations").kendoAutoComplete({
                        dataSource: Observation_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Observation...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulPhysicalExamSystemSection").find('#' + dataItem.id).length == 0) {
                                var PESystemId = PhysicalExamTemplatesRevamp.getCurrentSystemId();
                                var PETemplateSystemId = PhysicalExamTemplatesRevamp.getCurrentTemplateSystemId();
                                PhysicalExamTemplatesRevamp.addObservationSystemAssosiation(dataItem.PEObservationId, PESystemId, dataItem.Name, PETemplateSystemId);
                                $("#Observations").val('');
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
            objDeffered.resolve();
        });
        return objDeffered.promise();
    },
    bindObservationToHTML: function (item, PESystemObservationId, IsFromLoad) {
        //1- set observation into grid
        $ul = $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #ulObservation");
        if ($ul.find("#Observation_" + item.PEObservationId).length <= 0) {
            delete_ = '<a href="#" class="rightbtn"><span class="removeIconListHover" onclick="Clinical_PhysicalExamSystemsDetail.removeObservation(this,\'' + item.PEObservationId + '\', event);">'
                                        + '<i class="fa fa-times"></i></span> </a>'
            li_ = '<li PEObservationId="' + item.PEObservationId + '" id="Observation_' + item.PEObservationId + '" SystemObservationId="' + PESystemObservationId + '" data-lab="' + item.Name + '"><span id="spnLabName">' + item.Name + '</span>' + delete_ + '</li>';

            $ul.append(li_);

            //2- set its hidden value
            if (IsFromLoad != true) {
                var array = $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #hfPEObservationIds").val().split(',');
                var index_ = array.indexOf(item.PEObservationId);
                if (index_ < 0) {
                    array.push(item.PEObservationId);
                    $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #hfPEObservationIds").val(array.join());
                }
            }
        }
    },
    lookupPEObservation_DBCall: function () {

        var objData = new Object();
        objData["IsActive"] = 1;
        objData["commandType"] = "lookup_physicalexam_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },
    getCurrentSystemId: function () {
        var PESystemId = "";
        $(PhysicalExamTemplatesRevamp.selectedSystems).each(function (ind, i) {
            if (i.IsCurrent == true && i.PETemplateId == PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId) {
                PESystemId = i.PESystemId;
            }
        });
        return PESystemId;
    },

    getCurrentTemplateSystemId: function () {
        var PETemplateSystemId = "";
        $(PhysicalExamTemplatesRevamp.selectedSystems).each(function (ind, i) {
            if (i.IsCurrent == true && i.PETemplateId == PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId) {
                PETemplateSystemId = i.PETemplateSystemId;
            }
        });
        return PETemplateSystemId;
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
                PhysicalExamTemplatesRevamp.params.PEObservationId = observationId;
                if (observationId) {
                    $("#Observations").val('');
                    //var li = PhysicalExamTemplatesRevamp.addObservations(response.PEObservationId, objData["Name"], PESystemId, null);
                    //$("#ulPhysicalExamSystemSection").append(li);

                    var objSelectedObservations = { PESystemId: PESystemId, IsSelected: true, ObservationId: observationId, ObservationName: Name, IsSystemChecked: true, PETemplateSystemId: PETemplateSystemId };
                    PhysicalExamTemplatesRevamp.selectedObservations.push(objSelectedObservations);
                    var li = PhysicalExamTemplatesRevamp.addObservations(observationId, Name, PESystemId, "");
                    $("#ulPhysicalExamSystemSection").append(li);
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
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    getCurrentSelectedTemplateId: function () {
        var selectedPETemp = "";
        selectedPETemp = PhysicalExamTemplatesRevamp.params.PhysicalExamTemplateId;

        return selectedPETemp;
    },
    addSystemTempAssosiation: function (PETemplateId, PESystemId, Name) {
        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["TemplateId"] = PETemplateId;

        if (!objData["PESystemId"] || !objData["TemplateId"]) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }
        PhysicalExamTemplatesRevamp.addSystemTempAssosiation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                utility.DisplayMessages(response.Message, 1);

                var resSystems = JSON.parse(response.PETemplateSystem_JSON);
                resItem = resSystems[0];
                if (resItem.PESystemId) {
                    $("#Systems").val('');
                    var li = PhysicalExamTemplatesRevamp.addSystem(PESystemId, Name, PhysicalExamTemplatesRevamp.params["PhysicalExamTemplateId"], resItem.PETemplateSystemId);
                    $("#ulPhysicalExamSystems").append(li);
                    var objSelectedSystem = {
                        PESystemId: resItem.PESystemId, IsSelected: true, PETemplateId: PETemplateId, SystemName: Name, PETemplateSystemId: resItem.PETemplateSystemId, SystemDescription: "", PENotesObservationId: -1, IsCurrent: true
                    };
                    PhysicalExamTemplatesRevamp.selectedSystems.push(objSelectedSystem);
                }
            }
        });
    },

    addSystemTempAssosiation_DBCall: function (data) {
        var objData = new Object();
        if (data)
            objData = data;
        objData["commandType"] = "Insert_Template_System_Assosication";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },
    clearTemplateSelection: function () {
        $("#ulPhysicalExamSystems li").remove();
        $("#ulPhysicalExamSystemSection li").remove();
        //$("#observationContent").val('');
        CKEDITOR.instances['observationContent'].setData('');
        $("#ulPhysicalExams li").removeClass('active');
        $("#ulSelectedPETemplates li").removeClass('active');
    },

    AddPhysicalExam: function (obj, controlId) {
        var objData = new Object();
        objData["PhysicalExamTemplateName"] = $("#txtName" + controlId).val();
        objData["IsActive"] = 1;

        if (objData["PhysicalExamTemplateName"] == null || objData["PhysicalExamTemplateName"] == "" || objData["PhysicalExamTemplateName"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        PhysicalExamTemplatesRevamp.savePhysicalExam_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    var li = PhysicalExamTemplatesRevamp.addTemplate(response.PETemplateId, objData["PhysicalExamTemplateName"]);
                    $("#ulPhysicalExams").append(li);
                    $("#" + controlId).remove();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                $("#" + controlId).remove();
            }
        });
    },
    savePhysicalExam_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;
        objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId;
        objData["commandType"] = "Save_PhyscialExam_For_Provider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },
    LoadPhysicalExamForNotes: function (response, bNotSaveCompt) {

        var NoteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        PhysicalExamTemplatesRevamp.Content_style = $("#" + PhysicalExamTemplatesRevamp.params.PanelID + " #observationContent").attr('style');
        PhysicalExamTemplatesRevamp.createPhysicalExamBodyHTML(response, NoteHTMLCtrl, null, bNotSaveCompt);
    },

    physicalExamsDBCall: function (templateID) {
        var objData = new Object();
        objData["TemplateId"] = templateID;
        objData["commandType"] = "Load_PhyscialExam_For_SOAP_Note";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },
    resetForm: function () {
        // Empty global variables
        PhysicalExamTemplatesRevamp.selectedPhyExamTempData = [];
        PhysicalExamTemplatesRevamp.ProviderIds = "";
        PhysicalExamTemplatesRevamp.selectedObservations = [];
        PhysicalExamTemplatesRevamp.selectedSystems = [];
        PhysicalExamTemplatesRevamp.ProviderIds = "";
        PhysicalExamTemplatesRevamp.normalSystemIdsGlobel = [];
        PhysicalExamTemplatesRevamp.NewInsertedId = -1;
        PhysicalExamTemplatesRevamp.selectedSystems = [];
    },
    setActiveClass: function (observationId, ObservationName, obj, PESystemId) {
        if (obj && !$(obj).hasClass('text-success')) {
            $(obj).addClass('text-success')
        }
    },
    selectFirstSystem: function () {
        if ($("#ulPhysicalExamSystems li").length > 0)
            $($("#ulPhysicalExamSystems li")[0]).trigger('click');
    },
    ResetSystems: function (obj) {
        var TemplateId = PhysicalExamTemplatesRevamp.getCurrentSelectedTemplateId();
        $(PhysicalExamTemplatesRevamp.selectedSystems).filter(function (ind, i) {
            if (i.PETemplateId == TemplateId) {
                i.IsSelected = false;
                var chk = "chk" + i.PETemplateSystemId;
                $('#' + chk).prop('checked', false);
                $("#ulPhysicalExamSystems li[id='" + i.PESystemId + "']").removeClass('text-success');
            }
        });
    },
}