TinymceEditor = {
    TemplateContent: "",
    imageSize: 0,
    Load: function (params) {
        TinymceEditor.params = params;
        if (TinymceEditor.params.mode == "Add") {
            var height = 310;
            if (TinymceEditor.params["ComponentName"] == "Treatment") {
                height = 400;
            }
            TinymceEditor.InitTinymceControl(false, null, height);
            $('#' + TinymceEditor.params.PanelID + ' #frmTinymceEditor').data('serialize', $('#' + TinymceEditor.params.PanelID + ' #frmTinymceEditor').serialize());
        }
        else if (TinymceEditor.params.mode == "Edit") {
            $('#' + TinymceEditor.params.PanelID + ' #headerId').html("Edit Content");
            var cntnt = "Content";
            if (TinymceEditor.params["ComponentName"]) {
                switch (TinymceEditor.params["ComponentName"]) {
                    case 'LabOrder':
                        cntnt = "Lab Order";
                        break;
                    case 'LabResults':
                        cntnt = "Lab Results";
                        break;
                    case 'MedicalHx':
                        cntnt = "Medical Hx";
                        break;
                    case 'SocialHx':
                        cntnt = "Social Hx";
                        break;
                    case 'FamilyHx':
                        cntnt = "Family Hx";
                        break;
                    case 'SurgicalHx':
                        cntnt = "Surgical Hx";
                        break;
                    case 'HospitalizationHx':
                        cntnt = "Hospitalization Hx";
                        break;
                    case 'BirthHx':
                        cntnt = "Birth Hx";
                        break;
                    case 'ReviewofSystems':
                        cntnt = "Review of Systems";
                        break;
                    case 'RadiologyResults':
                    case 'RadiologyResults':
                        cntnt = "Diagnostic Imaging Results";
                        break;
                    case 'ProcedureOrder':
                        cntnt = "Procedure Order";
                        break;
                    case 'DiagnosticImagingOrder':
                    case 'RadiologyOrder':
                        cntnt = "Diagnostic Imaging Order";
                        break;
                    case 'PatientEducation':
                        cntnt = "Patient Education";
                        break;
                    case 'FollowUp':
                        cntnt = "Follow Up";
                        break;
                    case 'PhysicalExam':
                        cntnt = "Physical Exam";
                        break;
                    case 'CarePlan':
                        cntnt = "Care Plan";
                        break;
                    case 'FunctionalAndCognitive':
                        cntnt = "Functional And Cognitive";
                        break;
                    default:
                        cntnt = TinymceEditor.params["ComponentName"];
                        break;
                }
            }
            $('#' + TinymceEditor.params.PanelID + ' #headerId').html("Edit " + cntnt);
            var contents = "";
            if (TinymceEditor.params["Contents"]) {
                contents = TinymceEditor.params["Contents"];
            }
            TinymceEditor.LoadContent(contents);
        }

        //Macros 
        TinymceEditor.GetMacros().done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                $('#' + TinymceEditor.params.PanelID + " #ulMacroDetails").val('');

                $.each(response.MacroDetails, function (i, item) {
                    item.Description = item.Description.replace(/[']/g, "#quote#");
                    item.Description = item.Description.replace(/["]/g, "#doublequote#");
                    var li = TinymceEditor.addMacros(item.MacroId, item.MacroName, item.Description, "", item.CreatedBy)

                    $('#' + TinymceEditor.params.PanelID + " #ulMacroDetails").append(li);
                });
                $('#' + TinymceEditor.params.PanelID + ' #macros').bind('keyup', function () {

                    var searchString = $(this).val();

                    $('#' + TinymceEditor.params.PanelID + ' #ulMacroDetails li').each(function (index, value) {

                        currentName = $(value).text()
                        if (currentName.toUpperCase().indexOf(searchString.toUpperCase()) > -1) {
                            $(value).show();
                        } else {
                            $(value).hide();
                        }
                    });
                });
            }
        });
        $('#' + TinymceEditor.params.PanelID).click(function (ev) { if (ev.target.id == "MacroDescDetails") { return; } else { $('#MacroDescDetails').hide(); } });
        $($('iframe')[0]).contents().click(function (ev) { if (ev.target.id == "MacroDescDetails") { return; } else { $('#MacroDescDetails').hide(); } });
    },
    openQuickAdd: function (MacroId, MacroName, MacroKeyword, MacroDescription, Mode) {
        var params = {};
        if (MacroId != null) {
            params["MacroId"] = MacroId;
        }
        if (Mode == null) {
            params["mode"] = "Add";
        }
        else {
            params["mode"] = "Edit";
        }
        params["MacroName"] = MacroName;
        params["MacroKeyword"] = MacroKeyword;
        params["MacroDescription"] = MacroDescription;
        params["ParentCtrl"] = "TinymceEditor";
        LoadActionPan("Clinical_MacroQuickAddDetail", params);
    },
    addMacros: function (MacroId, MacroName, MacroDescription, IsNew, CreatedBy) {
        //objData["Description"].replace(/\r?\n/gi, "<br>");
        var Description = MacroDescription;
        Description = MacroDescription.replace(new RegExp("#quote#", 'g'), "");
        Description = Description.replace(new RegExp("#doublequote#", 'g'), '');
        var actions = "";
        actions = '<span class="removeIconListHover"><a class="btn  btn-xs pull-right" href="#" onclick="TinymceEditor.deleteMacro(\'' + MacroId + '\',event);" title="Delete Record"><i class="fa fa-times red"></i></a>  <a class="btn  btn-xs  pull-right" href="#" onclick="TinymceEditor.editMacroDetails(\'' + MacroId + '\',event);" title="Edit Record"><i class="fa fa-edit blue"></i></a> </span>'
        // actions = ' <span class="removeIconListHover"><a class="btn  btn-xs" href="#" onclick="TinymceEditor.editMacroDetails('MacroId',event);" title="Edit Record"><i class="fa fa-edit blue"></i></a><a class="btn  btn-xs" href="#" onclick="TinymceEditor.deleteMacro('10073',event);" title="Delete Record"><i class="fa fa-times red"></i></a></span>';
        if (!IsNew) {
            // if current user then show edit delete button
            if (globalAppdata.AppUserName.toString() == CreatedBy.toString() || globalAppdata.AppUserName.toString().toLowerCase() == "mdvision") {
                var li = '<li id="' + MacroId + '" onclick="TinymceEditor.BindMacros(\'' + MacroId +'\',\'' + String(MacroDescription.replace(/\r?\n/gi, "<br>")) + '\', this,event);" value="' + MacroId + '" refvalue="" title="' + Description + '"  subcharacteristicexist=" " class=""><div class="">' +
                    '<label id="lblName' + MacroId + '" class="" data-toggle="tooltip" title="" data-original-title="' + MacroName + '">' + MacroName + '</label><div id="divNameDetail' + MacroId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + MacroId + '" onkeypress="" name="Name' + MacroId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                    MacroId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div>' + actions + '<div class="clearfix"></div><div class="clearfix"></div></div></li>';
                return li;
            }
            else {
                var li = '<li id="' + MacroId + '" onclick="TinymceEditor.BindMacros(\'' + MacroId + '\',\''  + String(MacroDescription.replace(/\r?\n/gi, "<br>")) + '\', this,event);" value="' + MacroId + '" refvalue="" title="' + Description + '"  subcharacteristicexist=" " class=""><div class="">' +
                     '<label id="lblName' + MacroId + '" class="" data-toggle="tooltip" title="" data-original-title="' + MacroName + '">' + MacroName + '</label><div id="divNameDetail' + MacroId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + MacroId + '" onkeypress="" name="Name' + MacroId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                     MacroId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div> <div class="clearfix"></div><div class="clearfix"></div></div></li>';
                return li;
            }

        }
        else {
            var li = '<li id="' + MacroId + '" onclick="TinymceEditor.BindMacros(\'' + MacroId + '\',\'' + String(MacroDescription.replace(/\r?\n/gi, "<br>")) + '\', this,event);" value="' + MacroId + '" refvalue="" title="' + Description + '"  subcharacteristicexist=" " class=""><div class="">' +
                    '<label id="lblName' + MacroId + '" class="" data-toggle="tooltip" title="" data-original-title="' + MacroName + '">' + MacroName + '</label><div id="divNameDetail' + MacroId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + MacroId + '" onkeypress="" name="Name' + MacroId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                    MacroId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div>' + actions + '<div class="clearfix"></div><div class="clearfix"></div></div></li>';
            return li;

        }

    },
    BindMacros: function (MacroId, Description, obj, e) {

        Description = Description.replace(new RegExp("#quote#", 'g'), "'");
        Description = Description.replace(new RegExp("#doublequote#", 'g'), '"');
        if (e != null) {
            if ($(e.target).is('i[class*="fa-times"]') || $(e.target).is('i[class*="fa-edit"]')) {
                return;
            }
        }

        Description = Description.replace("#qoute", "'");
        Description = Description.replace("#doublequote", '"');
        if (Description != "") {
            tinymce.execCommand('mceInsertContent', false, Description);
        }
    },
    PostMacros: function (MacroId, MacroName, obj, MacroDescription) {
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
        if (AOESysObservationDetail.selectedObservations) {
            for (var i = 0 ; i < AOESysObservationDetail.selectedObservations.length; i++) {
                if (AOESysObservationDetail.selectedObservations[i].PESystemId == PESystemId && AOESysObservationDetail.selectedObservations[i].ObservationId == observationId) {
                    AOESysObservationDetail.selectedObservations[i].IsChecked = true;
                    AOESysObservationDetail.selectedObservations[i].IsSystemChecked = true;
                }
                if (AOESysObservationDetail.selectedObservations[i].PESystemId == PESystemId)
                    AOESysObservationDetail.selectedObservations[i].IsSystemChecked = true;
            }
        }
        $("#ulPhysicalExamSystems #chk" + PESystemId).prop("checked", true);
    },
    MacroDescriptions: function (item, Keyword) {
        var description;
       
      
        item.Description = item.Description.replace(/[']/g, "#quote#");
        item.Description = item.Description.replace(/["]/g, "#doublequote#");
        description = item.Description.replace(new RegExp('#quote#', 'g'), '');
        description = description.replace(new RegExp('#doublequote#', 'g'), '');
        //description = description.replace(/[<]/g, " < ");
        return '<li><button onclick="TinymceEditor.BindDescriptions(\'' + item.Description.replace(/\r?\n/gi, "<br>") + '\',\'' + Keyword + '\');" type="button">' + description + '</button></li>';
    },
    BindDescriptions: function (description, Keyword) {
        
        description = description.replace(new RegExp('#quote#', 'g'), "'");
        description = description.replace(new RegExp('#doublequote#', 'g'), '"');
        tinyMCE.activeEditor.setContent(tinymce.activeEditor.getBody().innerHTML.replace(Keyword + '..<span id="marker"></span>', description));
        // move cursor at end
        tinyMCE.activeEditor.selection.select(tinyMCE.activeEditor.getBody(), true);
        tinyMCE.activeEditor.selection.collapse(false);
    },
    DetectTrigger: function (obj) {
        // remove existing marker ids from iframe
        while ($(tinymce.activeEditor.getBody()).find('#marker').length) {

            $(tinymce.activeEditor.getBody()).find('#marker').remove();
        }
        tinymce.execCommand('mceInsertContent', false, "<span id='marker'></span>"); // Create a bookmark
        if (tinymce.activeEditor.getBody().innerHTML.charAt(tinymce.activeEditor.getBody().innerHTML.indexOf('<span id="marker">') - 2) == ".") // 2 dots are detected
        {
            var editorContent = tinymce.activeEditor.getBody().innerHTML.substring(0, tinymce.activeEditor.getBody().innerHTML.indexOf('<span id="marker">') - 2);
            editorContent = editorContent.replace(/&nbsp;/g, ' ');
            var splitted = editorContent.split("").reverse();
            var keyword = [];
            for (i = 0; i < splitted.length; i++) {
                if (splitted[i] != " " && splitted[i] != ">") {
                    keyword.push(splitted[i]);
                }
                else { break; }
            }
            keyword = keyword.reverse().join("");
            if (keyword == "&lt;") {
                keyword = "<";
            }
            else if (keyword == "&gt;") {
                keyword = ">";
            }
            //dropdown triggered here
            TinymceEditor.GetMacros(null, null, keyword).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var iframetinyMCE = document.getElementById('elm1_ifr'),
                            iframeCords = iframetinyMCE.getBoundingClientRect(),
                            iframeLeftPos = iframeCords.left,
                            iframeW = iframeCords.width,
                            markerElm = iframetinyMCE.contentWindow.document.getElementById('marker'),
                            elmPosTop = markerElm.getBoundingClientRect().top,
                            elmPosLeft = markerElm.getBoundingClientRect().left,
                            widowWidth = $(window).width(),
                            macroDescDetails = $('#MacroDescDetails');
                            if (keyword == "<")
                            { keyword = "&lt;" }
                            else if (keyword == ">") {
                                keyword = "&gt;";
                            }
                        if (response.MacroDetails.length > 0) {
                            if (response.MacroDetails.length == 1) {
                               
                                TinymceEditor.BindDescriptions(response.MacroDetails[0].Description.replace(/\r?\n/gi, "<br>"), keyword);

                            }
                            else {
                                $('#MacroDescriptions').empty();
                                $.each(response.MacroDetails, function (i, item) {
                                    $('#MacroDescriptions').append(TinymceEditor.MacroDescriptions(item, keyword));
                                });
                                var tooltipWidth = $('#MacroDescDetails').width();
                                var cursorPos = elmPosLeft + tooltipWidth + iframeLeftPos + 15;

                                if (cursorPos > widowWidth) {
                                    macroDescDetails.css("left", "auto");
                                    macroDescDetails.css("right", (iframeW - elmPosLeft));
                                }
                                else {
                                    macroDescDetails.css("right", "auto");
                                    macroDescDetails.css("left", elmPosLeft + 20);
                                }
                                macroDescDetails.css("top", elmPosTop - 15);

                                macroDescDetails.show("slow");
                                macroDescDetails.click();
                                macroDescDetails.focusout(function () { macroDescDetails.hide(); });
                            }
                        }
                        else {
                            $(tinymce.activeEditor.getBody()).find('#marker').remove();
                        }
                    }
                }
            });
        }
        else {
            $(tinymce.activeEditor.getBody()).find('#marker').remove();
        }
    },
    GetMacros: function (MacroId, MacroName, Keyword) {
        var data = {};
        if (MacroId != null) {
            data["MacroId"] = MacroId;
        }
        if (MacroName != null) {
            data["MacroName"] = MacroName;
        }
        data["Keyword"] = Keyword;
        data["UserId"] = globalAppdata.AppUserId;
        data["NoteComponentName"] = TinymceEditor.params["ComponentName"];
        if (TinymceEditor.params.ComponentName && TinymceEditor.params.ComponentName == "Complaints") {

            var componentid = $(TinymceEditor.params["Control"]).attr('notecomponentid');
            if (componentid)
            { data["NoteComponentId"] = componentid }
            else {
                data["NoteComponentId"] = $(TinymceEditor.params["Control"]).closest("li").attr("notecomponentid");
            }
        }
        else { data["NoteComponentId"] = $(TinymceEditor.params["Control"]).attr('notecomponentid'); }
        TinymceEditor.params["NoteComponentId"] = data["NoteComponentId"];
        data["commandType"] = "Search_MacroDetailsForNotes";
        var obj = JSON.stringify(data);
        return MDVisionService.APIService(obj, "Macro", "Macro");
    },
    LoadContent: function (contents) {
        var height = 310;
        if (TinymceEditor.params["ComponentName"] == "Treatment") {
            height = 400;
        }
        TinymceEditor.InitTinymceControl(false, null, height).done(function () {
            if ($(contents).text().split(' ').join('') != '')
                tinymce.execCommand('mceInsertContent', false, contents);
            $('#' + TinymceEditor.params.PanelID + ' #frmTinymceEditor').data('serialize', $('#' + TinymceEditor.params.PanelID + ' #frmTinymceEditor').serialize());
        });
    },
    UnLoadTab: function () {
        if (TinymceEditor.params["Name"]) {
            $("#" + 'Comments_' + TinymceEditor.params["Name"]).show();
            if ($(tinyMCE.activeEditor.getContent()).text().trim() == "") {
                if (TinymceEditor.params["Name"] == "Treatment") {
                    $("#" + 'Comments_' + TinymceEditor.params["Name"]).closest('section').remove();
                }
                else
                    $("#" + 'Comments_' + TinymceEditor.params["Name"]).html('');
            }
            else {
                Clinical_ProgressNote.RemoveSection($("#" + 'Comments_' + TinymceEditor.params["Name"]));
                $("#" + 'Comments_' + TinymceEditor.params["Name"]).html(tinyMCE.activeEditor.getContent());
            }
            Clinical_ProgressNote.saveComponentSOAPText(TinymceEditor.params["ComponentName"]);
        }
        TinymceEditor.UnloadTinymceEditor();
    },
    UnloadTinymceEditor: function () {
        if (TinymceEditor.params["FromAdmin"] == "0") {
            if (TinymceEditor.params != null && TinymceEditor.params.ParentCtrl != null) {
                UnloadActionPan(TinymceEditor.params.ParentCtrl, 'TinymceEditor');
            }
            else {
                UnloadActionPan(null, 'TinymceEditor');
            }
        }
        else {
            RemoveAdminTab();
        }
    },
    InitTinymceControl: function (Isreadonly, ControlId, height) {
        var objDeffered = $.Deferred();
        if (ControlId == null) {
            ControlId = "elm1";
        }
        var setHeight = "310";
        if (height != null) {
            setHeight = height.toString();
        }
        if (typeof tinymce.activeEditor != 'undefined') {

            tinymce.EditorManager.execCommand('mceRemoveEditor', true, ControlId);
        }
        var UpdateImageTiny;
        tinymce.init({
            selector: "#" + ControlId,
            theme: "modern",
            statusbar: false,
            readonly: Isreadonly,
            mode: "specific_textareas",
            browser_spellcheck: true,
            force_br_newlines: true,
            force_p_newlines: false,
            fontsize_formats: '12pt 14pt 16pt 18pt 20pt 24pt 36pt',
            content_style: ".mce-content-body {font-size:12pt;}",
            plugins: [
              "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
            "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
            "table directionality emoticons template textcolor paste fullpage textcolor colorpicker textpattern",
             "nonbreaking",
            ],
            menu: [],
            add_unload_trigger: false,
            paste_data_images: true, //enable drag drop image pasting
            toolbar1: "undo redo | styleselect  | bold underline italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | forecolor backcolor table | print preview",
            image_advtab: true,

            style_formats: [
            { title: 'Bold text', inline: 'strong' },
            { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
            { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
            { title: 'Badge', inline: 'span', styles: { display: 'inline-block', border: '1px solid #2276d2', 'border-radius': '5px', padding: '2px 5px', margin: '0 2px', color: '#2276d2' } },
            { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
            ],
            file_picker_callback: function (callback, value, meta) {
                UpdateImageTiny = callback;
                document.getElementById("ImageUploaderTinymce").click();
                if (meta.filetype == 'file') {
                }
                // Provide image and alt text for the image dialog
                if (meta.filetype == 'image') {
                    callback($('#EncodedImageString').val(), { alt: $('#ImageUploaderTinymce').val().split('\\').pop() });
                }
                // Provide alternative source and posted for the media dialog
                if (meta.filetype == 'media') {
                }
            },
            resize: false,
            height: setHeight,
            max_chars: "5",
            auto_focus: ControlId,
            setup: function (editor) {
                editor.on('init', function (e) {
                    //all your after init logics here.
                    //just to make sure, tinymce should be visibl
                    e.target.show();
                    setTimeout(function () {
                        tinymce.execCommand('mceFocus', false, ControlId);
                    }, 1000)
                    objDeffered.resolve();
                });
            },
            spellchecker_callback: function (method, words, callback) {
                if (method == "spellcheck") {
                    var suggestions = {
                    };

                    for (var i = 0; i < words.length; i++) {
                        suggestions[words[i]] = ["First", "second"];
                    }

                    callback(suggestions);
                }
            },
            init_instance_callback: function (editor) {
                editor.on('keyup', function (e) {
                    $('#MacroDescDetails').hide();
                    if (e.keyCode == 190 || e.which == 190 || e.keyCode == 110 || e.which == 110) // dot key is pressed
                    {
                        TinymceEditor.DetectTrigger(e);
                    }
                });
            },
            //contextmenu: "link image inserttable | cell row column deletetable | CreateNewDataField InsertDataField"
        });

        jQuery(function () {
            document.getElementById('ImageUploaderTinymce').addEventListener('change', function () {
                readImage(this);
            }, false);
        });
        function readImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#EncodedImageString').val(e.target.result);
                    if ($('#ImageUploaderTinymce').val() != "") {
                        if ((($('#EncodedImageString').val().length) / 1.37) / 1000000 > Number(globalAppdata['FileSize'])) {
                            utility.DisplayMessages("Please select Image less than 5 mb Size", 2);
                            return false;
                        } else {
                            UpdateImageTiny($('#EncodedImageString').val(), { alt: $('#ImageUploaderTinymce').val().split('\\').pop() });
                        }
                    }
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

        return objDeffered;
    },

    deleteMacro: function (MacroId) {
        //////////////////
        //Start//3/23/2016 by Babur //Delete logic implemented
        //  AppPrivileges.GetFormPrivileges("TinymceEditor", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //      if (strMessage == "") {
        utility.myConfirm('Do you want to delete this macro?', function () {
            TinymceEditor.deleteMacroDBCall(MacroId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    TinymceEditor.LoadMacros();

                }
                else {
                    utility.DisplayMessages(response.message, 3);
                }



            });

        }, function () { },
           'Alert'
);
        // }
        // else
        //    utility.DisplayMessages(strMessage, 2);
        // });

    },
    // delete Macro Db Call
    deleteMacroDBCall: function (MacroId) {
        var obj = new Object();
        obj["IdsToDelete"] = String(MacroId);
        obj["commandType"] = "Delete_Macro";
        var data = JSON.stringify(obj);
        return MDVisionService.APIService(data, "Macro", "Macro");
        //Clinical_Macro.Ids = [];
        //Clinical_Macro.Load();
    },

    editMacroDetails: function (MacroId) {
        var params = [];
        params["Mode"] = 'Edit';
        params["MacroId"] = MacroId;
        params["ParentCtrl"] = "TinymceEditor";
        params["NoteComponentId"] = TinymceEditor.params["NoteComponentId"];
        // data["NoteComponentIdFromNote"] = $(TinymceEditor.params["Control"]).attr('notecomponentid');
        LoadActionPan("Clinical_MacroQuickAddDetail", params);

    },

    // Load ALL MACROS
    LoadMacros: function () {

        // Get All Macros
        TinymceEditor.GetMacros().done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                // $('#' + TinymceEditor.params.PanelID + " #ulMacroDetails").val('');
                // will be remove all url

                $('#' + TinymceEditor.params.PanelID + " #ulMacroDetails").empty();

                $.each(response.MacroDetails, function (i, item) {
                    //var actions = "";
                    //actions = '&nbsp <a class="btn  btn-xs" href="#" onclick="TinymceEditor.VisitDelete(\'' + item.MacroId + '\',event);" title="Edit Record"><i class="fa fa-edit blue"></i></a> &nbsp<a class="btn  btn-xs" href="#" onclick="TinymceEditor.VisitDelete(\'' + item.MacroId + '\',event);" title="Delete Record"><i class="fa fa-times red"></i></a> '
                    item.Description = item.Description.replace(/[']/g, "#quote#");
                    item.Description = item.Description.replace(/["]/g, "#doublequote#");
                    var li = TinymceEditor.addMacros(item.MacroId, item.MacroName, item.Description, "", item.CreatedBy)

                    $('#' + TinymceEditor.params.PanelID + " #ulMacroDetails").append(li);
                });
                $('#' + TinymceEditor.params.PanelID + ' #Systems').bind('keyup', function () {

                    var searchString = $(this).val();

                    $('#' + TinymceEditor.params.PanelID + ' #ulMacroDetails li').each(function (index, value) {

                        currentName = $(value).text()
                        if (currentName.toUpperCase().indexOf(searchString.toUpperCase()) > -1) {
                            $(value).show();
                        } else {
                            $(value).hide();
                        }
                    });
                });
            }
        });
    }










}