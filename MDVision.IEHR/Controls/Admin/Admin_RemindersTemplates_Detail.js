remindersTemplatesDetail = {
    bIsFirstLoad: true,
    params: [],
    imageSize: 0,
    TemplateContent: "",
    Iferror: false,
    providerCheckedIds: [],
    ProviderIds: '',
    ProviderIdsmultiselect:'',

    Load: function (params) {

        remindersTemplatesDetail.params = params;

        if (remindersTemplatesDetail.params.PanelID != 'pnlRemindersTemplateDetail') {
            remindersTemplatesDetail.params.PanelID = remindersTemplatesDetail.params.PanelID + ' #pnlRemindersTemplateDetail';
        } else {
            remindersTemplatesDetail.params.PanelID = 'pnlRemindersTemplateDetail';
        }


        //if (globalAppdata['AppUserName'] == DefaultUser) {
        //    $('#' + remindersTemplatesDetail.params.PanelID + ' #frmRemindersTemplateDetail #divEntity').show();
        //}

        var self = $('#' + remindersTemplatesDetail.params.PanelID);

        self.loadDropDowns(true).done(function () {

            //if (remindersTemplatesDetail.isSuperAdmin()) {
            //    remindersTemplatesDetail.enableDisableDropDownLists('ddlNotesTemplateProvider', true);
            //} else {
            //    remindersTemplatesDetail.enableDisableDropDownLists('ddlNotesTemplateProvider', false);
            //}

            if (remindersTemplatesDetail.params.mode == "Add") {
                //remindersTemplatesDetail.isEntitySelected(remindersTemplatesDetail.isSuperAdmin());
                $('#' + remindersTemplatesDetail.params.PanelID + ' #frmRemindersTemplateDetail').data('serialize', $('#' + remindersTemplatesDetail.params.PanelID + ' #frmRemindersTemplateDetail').serialize());
            }
            else if (remindersTemplatesDetail.params.mode == "Edit") {
                $('#' + remindersTemplatesDetail.params.PanelID + ' #headerId').html("Edit Reminder Template");
                remindersTemplatesDetail.LoadRemindersTemplate();

            }
            remindersTemplatesDetail.ValidateRemindersTemplate();
        });


    },

    LoadRemindersTemplate: function () {

        if (remindersTemplatesDetail.params.mode == "Edit") {

            remindersTemplatesDetail.FillRemindersTemplate(remindersTemplatesDetail.params.RemindersTemplateId).done(function (response) {
                if (response.status != false) {
                    var RemindersTemplate_detail = JSON.parse(response.RemindersTemplateFill_JSON);
                    ProviderIdsmultiselect = RemindersTemplate_detail.ProviderIds;
                    var self = $("#pnlRemindersTemplateDetail");
                    utility.bindMyJSONByName(true, RemindersTemplate_detail, false, self).done(function () {
                        //remindersTemplatesDetail.isEntitySelected();
                       
                       // $("#placeOfServiceDetail #pnlPOSPlanInfo").removeClass('disableAll');
                        //remindersTemplatesDetail.ValidateRemindersTemplate();

                        //Serialize data
                        $('#' + remindersTemplatesDetail.params.PanelID + ' #frmRemindersTemplateDetail').data('serialize', $('#' + remindersTemplatesDetail.params.PanelID + ' #frmRemindersTemplateDetail').serialize());

                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }

    },

    FillRemindersTemplate: function (RemindersTemplateId) {

        var objData = new JSON.constructor();
        objData["CommandType"] = "fill_reminders_template";
        objData["RemindersTemplateId"] = RemindersTemplateId;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "Reminders");
    },

    isEntitySelected: function (isSuperAdmin) {
        var objDeffered = $.Deferred();
        selectedEntity = $('#' + remindersTemplatesDetail.params.PanelID + ' #ddlEntity option:selected').val();
        if (isSuperAdmin == false) {
            selectedEntity = globalAppdata["SeletedEntityId"];
        }
        $.when(remindersTemplatesDetail.loadEntityProvider(selectedEntity)).then(function () {

          //  remindersTemplatesDetail.IntializeMultiSelectDropDownProviders();

            objDeffered.resolve();

        });
        return objDeffered;
    },

    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },

    getRefValuefromDdl: function (ddlId, liId) {
        var $ddlOptions = $('#' + remindersTemplatesDetail.params.PanelID + " #" + ddlId).find('option');
        var value = null;
        $ddlOptions.each(function () {

            if ($(this).attr('value') == liId) {
                value = $(this).attr('refname');
                return false;
            }
        });
        return value;
    },

    showHideMultiSelectDdlOptions: function (isHide, ddlId, $multiselectLi, ddlOptionValue, calledBy, checkboxStatus) {
        if (isHide) {
            //populating provider
            if (calledBy.toLowerCase() == "provider") {
                if (!$multiselectLi.hasClass('filter') && !$multiselectLi.hasClass('multiselect-all')) {

                    $('#' + remindersTemplatesDetail.params.PanelID + " #" + ddlId).find('option[value="' + ddlOptionValue + '"]').prop('disabled', true);
                    $multiselectLi.prop('disabled', true);
                    $multiselectLi.hide();

                }
            }
        }
        else {

            $multiselectLi.show();
            if (typeof checkboxStatus != 'undefined') {
                if (calledBy.toLowerCase() == "provider" && checkboxStatus.toLowerCase() == 'uncheckall') {

                    if ($multiselectLi.hasClass('active')) {
                        $multiselectLi.find('input').trigger('click');
                    }
                }
            }
            //show the li
            $multiselectLi.prop('disabled', false);
            $('#' + remindersTemplatesDetail.params.PanelID + " #" + ddlId).find('option[value="' + ddlOptionValue + '"]').prop('disabled', false);

        }
    },

    loadEntityProvider: function (entityId) {

        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {

                var options = JSON.parse(result['GetEntityProvider']);
               var $providerDdl = $('#' + remindersTemplatesDetail.params.PanelID + ' #ddlNotesTemplateProvider');
                var $providerHiddenDdl = $('#' + remindersTemplatesDetail.params.PanelID + ' #ddlHiddenNotesTemplateProvider');

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
                if (remindersTemplatesDetail.ProviderIds != '') {
                    var Providers = remindersTemplatesDetail.ProviderIds.split(",");
                    remindersTemplatesDetail.providerCheckedIds = Providers;
                    $('#' + remindersTemplatesDetail.params.PanelID + ' #ddlNotesTemplateProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + remindersTemplatesDetail.params.PanelID + ' #divNotesTemplateSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
              //  remindersTemplatesDetail.IntializeMultiSelectDropDownProviders();

            });
            //enable multiselect
            remindersTemplatesDetail.enableDisableDropDownLists('ddlNotesTemplateProvider', false);
        }
        else {
            //disable multiselect
            remindersTemplatesDetail.enableDisableDropDownLists('ddlNotesTemplateProvider', true);
        }
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + remindersTemplatesDetail.params.PanelID + ' #ddlNotesTemplateProvider').multiselect('destroy');
        $('#' + remindersTemplatesDetail.params.PanelID + ' #ddlNotesTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {
              //  $('#' + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail #ddlNotesTemplateProvider").val(ProviderIdsmultiselect.split(','));
            },
            onDropdownHide: function (event) {
                //remindersTemplatesDetail.checkSpecialtiesByProviderId(option, checked, select);
               /// $('#' + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail #ddlNotesTemplateProvider").val(ProviderIdsmultiselect.split(','));
            },
            afterInit: function (container) {
                if (ProviderIdsmultiselect) {

                 //   $('#' + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail #ddlNotesTemplateProvider").val(ProviderIdsmultiselect.split(','));
                   // $('#' + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail #ddlNotesTemplateProvider").multiselect("refresh");
                }
            },



        });
    //    $('#' + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail #ddlNotesTemplateProvider").multiselect();
        //if (ProviderIdsmultiselect) {
        //    $('#' + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail #ddlNotesTemplateProvider option").each(function (index, element) {
        //        for (var i = 0; i < ProviderIdsmultiselect.split(",").length; i++) {
        //            if (index == ProviderIdsmultiselect.split(",")[i]) {
        //                $('#' + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail #ddlNotesTemplateProvider").multiselect('select', $(this).find('option[value='+index']').text());
        //            }
        //    }
        //        });

        //}
        if (ProviderIdsmultiselect) {
       
                for (var i = 0; i < ProviderIdsmultiselect.split(",").length; i++) {
                   
                    $('#' + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail #ddlNotesTemplateProvider").multiselect('select', $('#' + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail #ddlNotesTemplateProvider").find('option[value=' + ProviderIdsmultiselect.split(",")[i] + ']').text());
                 
                }
   

        }
        },

    isSuperAdmin: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + remindersTemplatesDetail.params.PanelID + ' #entityDDL').show();
            return true;
        } else {
            $('#' + remindersTemplatesDetail.params.PanelID + ' #entityDDL').hide();
            return false;
        }
    },

    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + remindersTemplatesDetail.params["PanelID"];
        $.each(ddlCommaSeparatedIds, function (index, Item) {
            if (isHide) {
                $(parrentPanelId + " #" + Item).multiselect('disable');
            }
            else {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            }
        });
    },

    ValidateRemindersTemplate: function () {

        $('#frmRemindersTemplateDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  TemplateName: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ReminderTemplateType: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  TemplateTypeId: {
                      group: '.col-xs-10',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  HTMLTemplate: {
               
                      group: '.col-sm-12',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }

              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           remindersTemplatesDetail.SaveRemindersTemplate();

       });

    },

    InsertTag: function () {
        var TagCategory = $('#' + remindersTemplatesDetail.params.PanelID + ' #ddlNoteTemplateTagCategory option:selected').text();
        var TagNameId = $('#' + remindersTemplatesDetail.params.PanelID + ' #NoteTemplatTagName').val();
        var TagCategoryId = $('#' + remindersTemplatesDetail.params.PanelID + ' #ddlNoteTemplateTagCategory').val();
        var TagNameText = " <" + TagCategory + " " + $('#' + remindersTemplatesDetail.params.PanelID + ' #NoteTemplatTagName option:selected').text() + "> ";
        var TagIds = "<" + TagCategory + " " + $('#' + remindersTemplatesDetail.params.PanelID + ' #NoteTemplatTagName option:selected').val() + ">";

        

        if (TagNameId != "") {
            var cursorPos = $('#' + remindersTemplatesDetail.params.PanelID + ' #txtTemplate').prop('selectionStart');
            var v = $('#' + remindersTemplatesDetail.params.PanelID + ' #txtTemplate').val();
            var textBefore = v.substring(0, cursorPos);
            var textAfter = v.substring(cursorPos, v.length);
            $('#' + remindersTemplatesDetail.params.PanelID + ' #txtTemplate').val(textBefore + TagNameText + textAfter);
            $('#' + remindersTemplatesDetail.params.PanelID + ' #hfHTMLTemplateWithIds').val(textBefore + TagIds + textAfter);
        }
        else {
            if ($('#' + remindersTemplatesDetail.params.PanelID + ' #ddlNoteTemplateTagCategory').val() != "") {
                utility.DisplayMessages("Select Tag Name", 3);
            }
            else {
                utility.DisplayMessages("Select Tag Category", 3);
            }
        }
    },

    UnLoadTab: function () {
        //remindersTemplatesDetail.UnloadTemplate();
        ProviderIdsmultiselect = "";
        UnloadActionPan();

    },

    UnloadTemplate: function () {
        UnloadActionPan();
    },

    TagCategoryChange: function () {
        if ($('#' + remindersTemplatesDetail.params.PanelID + ' #ddlNoteTemplateTagCategory').val() != "") {
            $('#' + remindersTemplatesDetail.params.PanelID + ' #NoteTemplatTagName').prop('disabled', false);
            var self = $('#' + remindersTemplatesDetail.params.PanelID);
            self.find('.NoteTemplatTagName > select').attr('ddlist', 'GetReminderTemplateTagName');
            var data = "IsActive=&ID=" + $('#' + remindersTemplatesDetail.params.PanelID + ' #ddlNoteTemplateTagCategory').val();
            self.find('.NoteTemplatTagName').loadDropDowns(true, data).done(function () {
            });
        }
        else {
            var self = $('#' + remindersTemplatesDetail.params.PanelID);
            $('#' + remindersTemplatesDetail.params.PanelID + ' #NoteTemplatTagName').prop('disabled', true);
            self.find('#' + remindersTemplatesDetail.params.PanelID + ' #NoteTemplatTagName').attr('ddlist', '');
        }
    },

    SaveRemindersTemplate: function () {
        //if (globalAppdata['AppUserName'] == DefaultUser) {
        //    $("#" + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail #ddlEntity").enable = true;
        //    $("#" + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail").bootstrapValidator('revalidateField', 'EntityId');
        //}

        var strMessage = "";
        var self = $('#' + remindersTemplatesDetail.params.PanelID + " #frmRemindersTemplateDetail");
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        myJSON = JSON.parse(myJSON);
        myJSON.IsActive = myJSON.IsActive == true ? 1 : 0;

        //var ProviderIds = self.find('#ddlNotesTemplateProvider option:Selected').map(function () {
        //    return this.value;
        //}).get().join(',');
        //myJSON.ProviderIds = ProviderIds;

        //var ProviderNames = self.find('#ddlNotesTemplateProvider option:Selected').map(function () {
        //    return this.text;
        //}).get().join(', ');
        //myJSON.ProviderNames = ProviderNames;


        var selected_ddlType = self.find('#ddlReminderType option:selected').text();

        var form_name = "";
        switch (selected_ddlType) {
            case "Text":
                form_name = "Reminder Templates_Text";
                break;
            case "Email":
                form_name = "Reminder Templates_Email";
                break;
            case "Voice":
                form_name = "Reminder Templates_Voice";
                break;
            case "- Select -":
                form_name = "no form"
                break;
        }

        myJSON = JSON.stringify(myJSON);
        if (remindersTemplatesDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges(form_name, "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    remindersTemplatesDetail.RemindersTemplateSave(myJSON).done(function (response) {
                        if (response.status != false) {
                            //Clinical_Provider_Note_Template.notesTemplateSearch();
                            Admin_RemindersTemplates.RemindersTemplatesSearch();
                            utility.DisplayMessages(response.Message, 1);
                            UnloadActionPan();
                            //$('#' + remindersTemplatesDetail.params.PanelID + ' #frmRemindersTemplateDetail').data('serialize', $('#' + remindersTemplatesDetail.params.PanelID + ' #frmRemindersTemplateDetail').serialize());
                            remindersTemplatesDetail.params["RemindersTemplateId"] = response.ReminderTemplateId;
                            remindersTemplatesDetail.params.mode = "Edit";
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }

                else
                    utility.DisplayMessages(strMessage, 2);
            });


        }
        else if (remindersTemplatesDetail.params.mode == "Edit") {

            AppPrivileges.GetFormPrivileges(form_name, "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
               if (strMessage == "") {
                    remindersTemplatesDetail.ProviderNoteTemplateUpdate(myJSON).done(function (response) {
                       // response = JSON.parse(response);
                        if (response.status != false) {
                            Admin_RemindersTemplates.RemindersTemplatesSearch();
                            utility.DisplayMessages(response.Message, 1);
                           // remindersTemplatesDetail.TemplateContent = tinyMCE.activeEditor.getContent();
                            $('#' + remindersTemplatesDetail.params.PanelID + ' #frmRemindersTemplateDetail').data('serialize', $('#' + remindersTemplatesDetail.params.PanelID + ' #frmRemindersTemplateDetail').serialize());

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
               }

               else
                   utility.DisplayMessages(strMessage, 2);
            });


        }
    },

    AddNewNoteType: function () {
        $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeDropDown").hide();
        $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeDTextBox").show();
        $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeText").focus();
    },
    HideNoteTypeTextbox: function () {
        setTimeout(function () {
            if (!remindersTemplatesDetail.Iferror) {
                $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeDropDown").show();
                $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeDTextBox").hide();
                $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeText").val("");
            }

        }, 300);
    },
    SaveNewNoteType: function () {

        if ($("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeText").val() != "") {
            remindersTemplatesDetail.Iferror = false;
            remindersTemplatesDetail.NewNoteTypeSave().done(function (response) {
                //response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    remindersTemplatesDetail.HideNoteTypeTextbox();
                    var self = $('#' + remindersTemplatesDetail.params.PanelID);
                    //self.find('#NoteTypeDropDown').loadDropDowns(true).done(function () {
                    //    $('#' + remindersTemplatesDetail.params.PanelID + " #ddlNoteTemplateType option").each(function () {
                    //        if ($(this).text() == $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeText").val()) {
                    //            $(this).attr('selected', 'selected');
                    //        }
                    //    });
                    //    $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeText").val("");
                    //});
                    CacheManager.BindCodes('GetRemindersType', true);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            remindersTemplatesDetail.Iferror = true;
            utility.DisplayMessages("Please enter value to save New Note Type", 3);
            setTimeout(function () {
                remindersTemplatesDetail.AddNewNoteType();
            }, 400);
        }
    },

    RemindersTemplateSave: function (TemplateData) {
        var objData = JSON.parse(TemplateData);
        objData["commandType"] = "save_template";
        //objData["ReminderTemplateType"] = remindersTemplatesDetail.params.ReminderTemplateType;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "RemindersTemplate");
    },
    KeyDownOnNoteTypeText: function (event) {
        event.stopPropagation();
        if (event.keyCode == 13) {
            remindersTemplatesDetail.SaveNewNoteType();
        }
    },

    ProviderNoteTemplateUpdate: function (RemindersTemplateData) {
        var objData = JSON.parse(RemindersTemplateData);
        objData["RemindersTemplateId"] = remindersTemplatesDetail.params.RemindersTemplateId;
        objData["commandType"] = "update_reminder_template";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "RemindersTemplate");
    },

    NewNoteTypeSave: function () {
        var objData = {};
        objData["NoteTypeText"] = $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeText").val();
        objData["commandType"] = "save_note_type";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "RemindersTemplate");
    },

    GetAllTagIDs: function (TemplateLetterHtml) {
        var TagIds = "";
        $($(TemplateLetterHtml).find(':input.TagInserted')).each(function () {
            if (TagIds == "") {
                TagIds = $(this).attr("id");
            }
            else {
                TagIds = TagIds + "," + $(this).attr("id");
            }

        });
        return TagIds;
    },

    AddNewNoteType: function () {
        $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeDropDown").hide();
        $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeDTextBox").show();
        $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeText").focus();
    },

    SaveNewNoteType: function () {

        if ($("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeText").val() != "") {
            remindersTemplatesDetail.Iferror = false;
            remindersTemplatesDetail.NewNoteTypeSave().done(function (response) {
                //response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    remindersTemplatesDetail.HideNoteTypeTextbox();
                    var self = $('#' + remindersTemplatesDetail.params.PanelID);
                    self.find('#NoteTypeDropDown').loadDropDowns(true).done(function () {
                        $('#' + remindersTemplatesDetail.params.PanelID + " #ddlNoteTemplateType option").each(function () {
                            if ($(this).text() == $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeText").val()) {
                                $(this).attr('selected', 'selected');
                            }
                        });
                        $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeText").val("");
                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            remindersTemplatesDetail.Iferror = true;
            utility.DisplayMessages("Please enter value to save New Note Type", 3);
            setTimeout(function () {
                remindersTemplatesDetail.AddNewNoteType();
            }, 400);
        }
    },



    HideNoteTypeTextbox: function () {
        setTimeout(function () {
            if (!remindersTemplatesDetail.Iferror) {
                $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeDropDown").show();
                $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeDTextBox").hide();
                $("#" + remindersTemplatesDetail.params.PanelID + " #NoteTypeText").val("");
            }

        }, 300);
    },

    KeyDownOnNoteTypeText: function (event) {
        event.stopPropagation();
        if (event.keyCode == 13) {
            remindersTemplatesDetail.SaveNewNoteType();
        }
    },

    setHiddenControl: function () {

        $("#" + remindersTemplatesDetail.params.PanelID + " #hfHTMLTemplateWithIds").val($("#" + remindersTemplatesDetail.params.PanelID + " #txtTemplate").val());

    },
}