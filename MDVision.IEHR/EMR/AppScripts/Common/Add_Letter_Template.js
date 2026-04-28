// Created By:  Muhammad Ahmad Imran
// Created Date: 1/3/2016
Add_Letter_Template = {
    bIsFirstLoad: true,
    params: [],
    imageSize: 0,
    TemplateContent: "",
    SpecialtyIds: '',
    ProviderIds: '',
    specialityCheckedIds: [],
    providerCheckedIds: [],
    providerSelectedIds: [],
    Load: function (params) {
        Add_Letter_Template.params = params;

        if (Add_Letter_Template.params.PanelID != 'pnlAddLetterTemplate') {
            Add_Letter_Template.params.PanelID = Add_Letter_Template.params.PanelID + ' #pnlAddLetterTemplate';
        } else {
            Add_Letter_Template.params.PanelID = 'pnlAddLetterTemplate';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Add_Letter_Template.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }

        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate #divEntity').show();
        }

        var self = $('#' + Add_Letter_Template.params.PanelID);
        self.loadDropDowns(true).done(function () {

            if (Add_Letter_Template.params.mode == "Add") {

                // $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate #Active').prop("disabled", true);
                //intialize TinyMCE instance on textarea control
                EMRUtility.InitTinymceControl(false, null, 310);

                //serialize data
                //$('#frmAddLetterTemplate').data('serialize', $('#frmAddLetterTemplate').serialize());
                $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').serialize());
                setTimeout(function () {
                    Add_Letter_Template.TemplateContent = tinyMCE.activeEditor.getContent();
                    $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').serialize());
                }, 200);
                Add_Letter_Template.loadDropDowns();
            }
            else if (Add_Letter_Template.params.mode == "Edit") {
                //$('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate #Active').prop("disabled", false);
                $('#' + Add_Letter_Template.params.PanelID + ' #headerId').html("Edit Letter Template");

                //Start 13-10-2017 Humaira Yousaf EMR-4959      
                Add_Letter_Template.LoadTemplateLetter();
                //End 13-10-2017 Humaira Yousaf EMR-4959 
            }
            Add_Letter_Template.ValidateTemplateLetter();

            //Start || 14 July, 2016 || Talha Tanweer || select "All" option in category dropdown by default
            $("#" + Add_Letter_Template.params.PanelID + " #ddlCategory").val("4");
            //End   || 14 July, 2016 || Talha Tanweer || select "All" option in category dropdown by default
        });
        //InitTinymceControl(true);
    },
    ValidateTemplateLetter: function () {

        $('#frmAddLetterTemplate')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  Name: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: 'Specify a Name for the Template and try again. '
                          },
                      }
                  },
                  CategoryId: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: 'Specify a Category for the Template and try again. '
                          },
                      }
                  },
                  EntityId: {
                      enable: false,
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: 'Specify a Entity for the Template and try again. '
                          },
                      }
                  }
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Add_Letter_Template.SaveLetter();

       });

    },
    //Start//3/3/2016//M Ahmad Imran//Implimented Call to Controller for Get Template letter Detail
    LoadTemplateLetter: function () {

        if (Add_Letter_Template.params.mode == "Edit") {

            Add_Letter_Template.FillTemplateLetter().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    //Start 13-10-2017 Humaira Yousaf EMR-4959 
                    var LetterData = JSON.parse(response.LetterTemplatesLoad_JSON);                    
                    $('#' + Add_Letter_Template.params.PanelID + ' #lstEntityId').val(LetterData.EntityId);
                    Add_Letter_Template.loadDropDowns();
                    //End 13-10-2017 Humaira Yousaf EMR-4959 

                    if (response.LetterTemplatesCount != 0) {
                        var self = $("#" + Add_Letter_Template.params.PanelID);

                        utility.bindMyJSONByName(true, JSON.parse(response.LetterTemplatesLoad_JSON), false, self);

                        if (JSON.parse(response.LetterTemplatesLoad_JSON)['IsActive'] == 'True')
                            $("#" + Add_Letter_Template.params.PanelID + ' #IsActive').attr("checked", true);
                        else
                            $("#" + Add_Letter_Template.params.PanelID + ' #IsActive').attr("checked", false);
                        ////intialize TinyMCE instance on textarea control and enabled tinymce

                        EMRUtility.InitTinymceControl(false, null, 310).done(function () {
                            tinymce.execCommand('mceInsertContent', false, LetterData.elm1);
                            Add_Letter_Template.TemplateContent = tinyMCE.activeEditor.getContent();
                            $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').serialize());
                        });
                        //Start 10-10-2017 Edit By Humaira Yousaf IMP-1189
                        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterProvider').multiselect('clearSelection', false);
                        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterProvider').multiselect('updateButtonText');
                        $('#' + Add_Letter_Template.params.PanelID + " #ddlLetterProvider").val(LetterData.ProviderIds.split(','));
                        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterProvider').multiselect("refresh");
                        //Start Humaira Yousaf 12-10-2017 Bug# EMR-4958
                        if (LetterData.ProviderIds != "") {
                            Add_Letter_Template.providerCheckedIds = LetterData.ProviderIds.split(',');
                        }
                        //End Humaira Yousaf 12-10-2017 Bug# EMR-4958

                        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').multiselect('clearSelection', false);
                        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').multiselect('updateButtonText');
                        $('#' + Add_Letter_Template.params.PanelID + " #ddlLetterSpecialty").val(LetterData.SpecialtyIds.split(','));
                        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').multiselect("refresh");
                        //Start Humaira Yousaf 12-10-2017 Bug# EMR-4958
                        if (LetterData.SpecialtyIds != '') {
                            Add_Letter_Template.specialityCheckedIds = LetterData.SpecialtyIds.split(',');
                        }
                        //End Humaira Yousaf 12-10-2017 Bug# EMR-4958
                        //End 10-10-2017 Edit By Humaira Yousaf IMP-1189
                    } else {
                        utility.DisplayMessages(response.Message, 3);
                    }


                }
                else {

                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    //Start//3/3/2016//M Ahmad Imran//Implimented Call to Controller for Get Template letter Detail
    FillTemplateLetter: function () {
        var objData = {};
        objData["TemplateLetterId"] = Add_Letter_Template.params.TemplateLetterId;
        objData["commandType"] = "Get_Detail_Of_TEMPLATE_LETTER";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ClinicalLetterTemplate");
    },
    UnLoadTab: function () {
        //Start 10-10-2017 Edit By Humaira Yousaf IMP-1189
        Add_Letter_Template.specialityCheckedIds = [];
        Add_Letter_Template.providerCheckedIds = [];
        Add_Letter_Template.providerSelectedIds = [];
        Add_Letter_Template.SpecialtyIds = "";
        Add_Letter_Template.ProviderIds = "";
        //End 10-10-2017 Edit By Humaira Yousaf IMP-1189
        if (EMRUtility.compareFormDataWithSerialized(Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate') || Add_Letter_Template.TemplateContent != tinyMCE.activeEditor.getContent()) {
            utility.myConfirm('2', function () {
                Add_Letter_Template.UnloadTemplateLetter();
            },
           '1'
           );
        } else {
            Add_Letter_Template.UnloadTemplateLetter();
        }
    },
    UnloadTemplateLetter: function () {
        if (Add_Letter_Template.params["FromAdmin"] == "0") {
            if (Add_Letter_Template.params != null && Add_Letter_Template.params.ParentCtrl != null) {
                UnloadActionPan(Add_Letter_Template.params.ParentCtrl, 'Add_Letter_Template');
            }
            else
                UnloadActionPan(null, 'Add_Letter_Template');
        }
        else {

            RemoveAdminTab();
        }
    },
    //Start//1/03/2016//M Ahmad Imran//Implimented TagCategoryChange function which Load Tagname dropdown 
    TagCategoryChange: function () {
        if ($('#' + Add_Letter_Template.params.PanelID + ' #ddlTagCategory').val() != "") {
            $('#' + Add_Letter_Template.params.PanelID + ' #TagName').prop('disabled', false);
            var self = $('#' + Add_Letter_Template.params.PanelID);
            self.find('.TagName > select').attr('ddlist', 'GetLetterTagName');
            var data = "IsActive=&ID=" + $('#' + Add_Letter_Template.params.PanelID + ' #ddlTagCategory').val();
            self.find('.TagName').loadDropDowns(true, data).done(function () {
            });
        }
        else {
            var self = $('#' + Add_Letter_Template.params.PanelID);
            $('#' + Add_Letter_Template.params.PanelID + ' #TagName').prop('disabled', true);
            self.find('#' + Add_Letter_Template.params.PanelID + ' #TagName').attr('ddlist', '');
            self.find('#' + Add_Letter_Template.params.PanelID + ' #TagName').val("");
            //
        }
    },
    //Start//1/03/2016//M Ahmad Imran//Implimented InsertTag function which Insert Tag in Template letter.
    InsertTag: function () {
        var TagCategory = $('#' + Add_Letter_Template.params.PanelID + ' #ddlTagCategory option:selected').text();
        var TagNameId = $('#' + Add_Letter_Template.params.PanelID + ' #TagName').val();
        var TagCategoryId = $('#' + Add_Letter_Template.params.PanelID + ' #ddlTagCategory').val();
        var TagNameText = TagCategory + " " + $('#' + Add_Letter_Template.params.PanelID + ' #TagName option:selected').text();

        if (TagNameId != "" && TagCategoryId != "") {
            if (typeof tinyMCE != 'undefined') {
                //var InsertFieldInput = '<input type="text" class="TagInserted" readonly id="' + TagNameId + '" value="{{ ' + TagNameText + ' }}" style="min-width: 10px; border: none;width:' + (((TagNameText.length + 4) * 7) - 12) + 'px;"/>';
                var InsertFieldInput = '<span class="TagInserted" id="' + TagNameId + '" > {{ ' + TagNameText + ' }} </span>';

                if (TagNameText == "Appointment Comments") {
                    InsertFieldInput = '<span class="TagInserted" id="199" > {{ ' + TagNameText + ' }} </span>';

                }
                if (TagCategory.toLowerCase().trim() == "clinical") {
                    InsertFieldInput = '<span class="TagInserted" readonly id="' + TagNameId + '" > {{ ' + TagNameText + ' }} </span>';                    
                }
                tinymce.execCommand('mceInsertContent', false, InsertFieldInput);
            }
        }
        else {
            if ($('#' + Add_Letter_Template.params.PanelID + ' #ddlTagCategory').val() != "") {
                utility.DisplayMessages("Select Tag Name", 3);
            }
            else {
                utility.DisplayMessages("Select Tag Category", 3);
            }
        }
    },
    //Start//2/03/2016//M Ahmad Imran//Implimented GetAllTagIDs function which return all Tag Ids from template content
    GetAllTagIDs: function (TemplateLetterHtml) {
        var TagIds = [];
        //var isIDExist = true;

        $($(TemplateLetterHtml).find('span.TagInserted')).each(function () {

            //if (isIDExist && $(this).parent().attr('id') != undefined) {
            //    isIDExist = false;
            //    TagIds = $(this).parent().attr('id');
            //}

            if ($(this).attr("id") != undefined) {
                TagIds.push($(this).attr("id"));
            }
        });

        return utility.unique(TagIds).toString();
    },

    //Start//1/03/2016//M Ahmad Imran//Implimented SaveLetter function which Save Template letter.
    SaveLetter: function () {


        if (globalAppdata['AppUserName'] == DefaultUser) {
            $("#" + Add_Letter_Template.params.PanelID + " #frmAddLetterTemplate #lstEntityId").enable = true;
            $("#" + Add_Letter_Template.params.PanelID + " #frmAddLetterTemplate").bootstrapValidator('revalidateField', 'EntityId');
        }
        if (EMRUtility.compareFormDataWithSerialized(Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate') || Add_Letter_Template.TemplateContent != tinyMCE.activeEditor.getContent()) {
            setTimeout(function () {

                var strMessage = "";
                var self = $('#' + Add_Letter_Template.params.PanelID + " #frmAddLetterTemplate");
                var myJSON = self != null ? self.getMyJSONByName() : "{}";
                myJSON = JSON.parse(myJSON);
                myJSON.TemplateContent = tinyMCE.activeEditor.getContent();
                myJSON.TagIds = Add_Letter_Template.GetAllTagIDs(tinyMCE.activeEditor.getContent());
                if (globalAppdata['AppUserName'] != DefaultUser) {
                    myJSON.EntityId = -1;
                }
                //Start 10-10-2017 Edit By Humaira Yousaf IMP-1189
                var SpecialtyIds = self.find('#ddlLetterSpecialty option:Selected').map(function () {
                    return this.value;
                }).get().join(',');
                myJSON.SpecialtyIds = SpecialtyIds;
               
                var ProviderIds = self.find('#ddlLetterProvider option:Selected').map(function () {
                    return this.value;
                }).get().join(',');
                myJSON.ProviderIds = ProviderIds;
                //End 10-10-2017 Edit By Humaira Yousaf IMP-1189
                myJSON = JSON.stringify(myJSON);
                if (Add_Letter_Template.params.mode == "Add") {
                    AppPrivileges.GetFormPrivileges("Template_Letter", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Add_Letter_Template.TemplateLetterSave(myJSON).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Letter_Template.letterTemplatesSearch();

                                    utility.DisplayMessages(response.Message, 1);
                                    Add_Letter_Template.TemplateContent = tinyMCE.activeEditor.getContent();
                                    $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').serialize());
                                    Add_Letter_Template.params["TemplateLetterId"] = response.TemplateLetterId;
                                    Add_Letter_Template.params.mode = "Edit";


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
                else if (Add_Letter_Template.params.mode == "Edit") {
                    AppPrivileges.GetFormPrivileges("Template_Letter", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {

                            Add_Letter_Template.TemplateLetterUpdate(myJSON).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Letter_Template.letterTemplatesSearch();
                                    utility.DisplayMessages(response.Message, 1);
                                    Add_Letter_Template.TemplateContent = tinyMCE.activeEditor.getContent();
                                    $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').serialize());


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
            }, 5);
        } else {
            utility.DisplayMessages("Please make any changes to save/update Letter Template", 3);
            setTimeout(function () {
                $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').serialize());
            }, 100);
        }

    },
    //Start//2/3/2016//M Ahmad Imran//Implimented Call to Controller for Save Template letter Detail
    TemplateLetterSave: function (TemplateLetterData) {
        var objData = JSON.parse(TemplateLetterData);
        objData["commandType"] = "SAVE_TEMPLATE_LETTER";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ClinicalLetterTemplate");
    },

    //Start//2/3/2016//M Ahmad Imran//Implimented Call to Controller for Update Template letter Detail
    TemplateLetterUpdate: function (TemplateLetterData) {
        var objData = JSON.parse(TemplateLetterData);
        objData["commandType"] = "Update_TEMPLATE_LETTER";
        objData["TemplateLetterId"] = Add_Letter_Template.params.TemplateLetterId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ClinicalLetterTemplate");
    },
    //Start 10-10-2017 Edit By Humaira Yousaf IMP-1189
    loadDropDowns: function () {
        //Start 13-10-2017 Humaira Yousaf EMR-4959 
        Add_Letter_Template.specialityCheckedIds = [];
        Add_Letter_Template.providerCheckedIds =[];
        Add_Letter_Template.providerSelectedIds =[];
        Add_Letter_Template.SpecialtyIds = "";
        Add_Letter_Template.ProviderIds = "";

        var dfd = new $.Deferred();
        var isSAdmin = Add_Letter_Template.isSuperAdmin();
        if (isSAdmin) {           
            Add_Letter_Template.enableDisableDropDownLists('ddlLetterSpecialty,ddlLetterProvider', true); 
        }
        else {
            Add_Letter_Template.enableDisableDropDownLists('ddlLetterSpecialty,ddlLetterProvider', false);
        }

        var selectedEntity = $('#' + Add_Letter_Template.params.PanelID + ' #lstEntityId option:selected').val();
        if (isSAdmin == false) {
            selectedEntity = globalAppdata["SeletedEntityId"];
        }

        Add_Letter_Template.loadEntitySpecialty(selectedEntity, dfd);
    //End 13-10-2017 Humaira Yousaf EMR-4959 
    },

    loadEntitySpecialty: function (entityID, dfd) {
        // Loads Spacialties Based on entityId
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (Add_Letter_Template.SpecialtyIds != '') {

                        var Specialties = Add_Letter_Template.SpecialtyIds.split(",");
                        Add_Letter_Template.specialityCheckedIds = Specialties;
                        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').val(Specialties);
                    }
                    $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').serialize());
                }

            }).then(function () {
                Add_Letter_Template.IntializeMultiSelectDropDownSpecialties();
                //enable dropdownlist
                Add_Letter_Template.enableDisableDropDownLists('ddlLetterSpecialty', false);
                Add_Letter_Template.loadEntityProvider(entityID, dfd);
                $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').serialize());
            });
        }
        else {
            //Disable dropdownlist
            Add_Letter_Template.enableDisableDropDownLists('ddlLetterSpecialty', true);
            dfd.resolve();
        }
    },

    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').multiselect('destroy');
        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Add_Letter_Template.checkProvidersBySpecialityIds(option, checked, select);
            },
            onDropdownHide: function (event) {
                $.when(
                    Add_Letter_Template.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (Add_Letter_Template.ProviderIds != '') {
                        var Providers = Add_Letter_Template.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                Add_Letter_Template.providerCheckedIds = Add_Letter_Template.removeFromArray(Add_Letter_Template.providerCheckedIds, item);
                                Add_Letter_Template.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterProvider').val(Add_Letter_Template.providerCheckedIds);
                    Add_Letter_Template.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Add_Letter_Template.SpecialtyIds != '') {
                    var spacialties = Add_Letter_Template.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Add_Letter_Template.specialityCheckedIds = Add_Letter_Template.removeFromArray(Add_Letter_Template.specialityCheckedIds, item);
                            Add_Letter_Template.specialityCheckedIds.push(item);
                        });
                    }
                }
                Add_Letter_Template.setSpacialtiesByselectedProviderIds();
                $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').multiselect('clearSelection', false);
                $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').multiselect('updateButtonText');
                $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').multiselect('select', Add_Letter_Template.specialityCheckedIds);
                $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty').multiselect('refresh');
            },
        });
    },
    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + Add_Letter_Template.params["PanelID"];
        $.each(ddlCommaSeparatedIds, function (index, Item) {
            if (isHide) {
                $(parrentPanelId + " #" + Item).multiselect('disable');
            }
            else {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            }
        });
    },

    loadEntityProvider: function (entityId, dfd) {

        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterProvider');
                var $providerHiddenDdl = $('#' + Add_Letter_Template.params.PanelID + ' #ddlHiddenLetterProvider');

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
                if (Add_Letter_Template.ProviderIds != '') {
                    var Providers = Add_Letter_Template.ProviderIds.split(",");
                    Add_Letter_Template.providerCheckedIds = Providers;
                    $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterProvider').val(Providers);
                }
                $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').serialize());
            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + Add_Letter_Template.params.PanelID + ' #divLetterSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                Add_Letter_Template.IntializeMultiSelectDropDownProviders();
                Add_Letter_Template.enableDisableDropDownLists('ddlLetterProvider', false);
                $('#' +Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').data('serialize', $('#' +Add_Letter_Template.params.PanelID + ' #frmAddLetterTemplate').serialize());

                if (dfd)
                    dfd.resolve();
            });
            //enable multiselect           
        }
        else {
            //disable multiselect
            Add_Letter_Template.enableDisableDropDownLists('ddlLetterProvider', true);
            dfd.resolve();
        }
    },

    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + Add_Letter_Template.params.PanelID + ' #divLetterSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            Add_Letter_Template.specialityCheckedIds = [];
            Add_Letter_Template.providerCheckedIds = [];
            Add_Letter_Template.ProviderIds = '';
            Add_Letter_Template.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && specialtyItems != checkedSpecialtyItems) {
                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    Add_Letter_Template.specialityCheckedIds = Add_Letter_Template.removeFromArray(Add_Letter_Template.specialityCheckedIds, spacialityId);
                    Add_Letter_Template.specialityCheckedIds.push(spacialityId);
                }
                else {

                    Add_Letter_Template.specialityCheckedIds = Add_Letter_Template.removeFromArray(Add_Letter_Template.specialityCheckedIds, spacialityId);
                }
            }
            else {

                Add_Letter_Template.specialityCheckedIds = [];
                $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    Add_Letter_Template.specialityCheckedIds.push(spacialityId);
                });
            }
        }
    },

    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + Add_Letter_Template.params.PanelID + ' #ddlHiddenLetterProvider';

        var providerContext = '#' + Add_Letter_Template.params.PanelID + ' #ddlLetterProvider';
        $(providerContext).empty();

        if (Add_Letter_Template.specialityCheckedIds.length > 0) {

            $.each(Add_Letter_Template.specialityCheckedIds, function (index, specialtyId) {

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
    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },


    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterProvider').multiselect('destroy');
        $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Add_Letter_Template.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
            },
        });
    },

    checkSpecialtiesByProviderId: function (option, checked, select) {

        //provider context
        var providerContext = '#' + Add_Letter_Template.params.PanelID + ' #divLetterProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        var allProviders = $(providerContext).find('.dropdown-menu').find('li:not(".filter,.multiselect-all")').length;
        var selectedProviders = $(providerContext).find('.dropdown-menu').find('li.active:not(".filter,.multiselect-all")').length;


        if (checkedProviderItems <= 0) {
            Add_Letter_Template.providerCheckedIds = [];
            Add_Letter_Template.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected && allProviders == selectedProviders) {
            Add_Letter_Template.providerCheckedIds = [];
            $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterProvider option').each(function () {
                var providerValue = $(this).val();
                Add_Letter_Template.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                Add_Letter_Template.providerCheckedIds = Add_Letter_Template.removeFromArray(Add_Letter_Template.providerCheckedIds, providerValue);
                Add_Letter_Template.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                Add_Letter_Template.providerCheckedIds = Add_Letter_Template.removeFromArray(Add_Letter_Template.providerCheckedIds, $(option).val());
            }
        }
    },

    setSpacialtiesByselectedProviderIds: function () {

        $.each(Add_Letter_Template.providerCheckedIds, function (index, item) {
            $('#' + Add_Letter_Template.params.PanelID + ' #ddlLetterProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {
                        Add_Letter_Template.specialityCheckedIds = Add_Letter_Template.removeFromArray(Add_Letter_Template.specialityCheckedIds, $(this).attr('refname'));
                        Add_Letter_Template.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },
    //End 10-10-2017 Edit By Humaira Yousaf IMP-1189
    //Start 13-10-2017 Humaira Yousaf EMR-4959 
    isSuperAdmin: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Add_Letter_Template.params.PanelID + ' #divEntity').show();
            return true;
        } else {
            $('#' + Add_Letter_Template.params.PanelID + ' #divEntity').hide();
            return false;
        }
    },
    //End 13-10-2017 Humaira Yousaf EMR-4959 
}