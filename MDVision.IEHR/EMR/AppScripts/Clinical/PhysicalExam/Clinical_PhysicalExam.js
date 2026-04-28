Clinical_PhysicalExam = {
    //Author: Muhammad Arshad
    //Date: 14-01-2016
    //This file will handle all actions performed for Family History and it's child handling
    //Once PhysicalExam will be created then it's child can be created then.
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
    normalSystemIdsGlobel: [],
    bNextPrev: false,
    controlToInvoke: null,
    isFirstLoad: true,
    DBDataJSON: [],
    charIds: [],
    subCharIds: [],
    TemplateDataFromProviderNotesJSON: [],
    DataJSON: [],
    DataJSONTemp: [],
    TemplateDataJSON: [],
    DBTemplateId: 0,
    oldTemplateId: null,
    currentTemplateId: null,
    TemplateType: '',
    NewInsertedId: -1,
    //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will be called once tab is clicked, it expects parameters to be used for PhysicalExam
    Load: function (params) {

        Clinical_PhysicalExam.charIds = [];
        Clinical_PhysicalExam.subCharIds = [];
        Clinical_PhysicalExam.DataJSON = [];
        Clinical_PhysicalExam.DBDataJSON = [];
        Clinical_PhysicalExam.TemplateDataJSON = [];
        Clinical_PhysicalExam.selectedData = [];
        Clinical_PhysicalExam.params = params;
        Clinical_PhysicalExam.loadPhysExamTemplates();
        Clinical_PhysicalExam.loadPhysExamDataTemplates();
        if (Clinical_PhysicalExam.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val(Clinical_PhysicalExam.params.NotesId);
        }
        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        if (Clinical_PhysicalExam.params.mode == null) {
            Clinical_PhysicalExam.params.mode = "Add";
        }
        if (Clinical_PhysicalExam.params.PanelID != 'pnlClinicalPhysicalExam') {
            Clinical_PhysicalExam.params.PanelID = Clinical_PhysicalExam.params.PanelID + ' #pnlClinicalPhysicalExam';
        } else {
            Clinical_PhysicalExam.params.PanelID = 'pnlClinicalPhysicalExam';
        }
        var PhysicalExamId = "";
        if (Clinical_PhysicalExam.params.mode == "Add" || Clinical_PhysicalExam.params.PhysicalExamId == null || Clinical_PhysicalExam.params.PhysicalExamId == "" || Clinical_PhysicalExam.params.PhysicalExamId == "-1") {
            PhysicalExamId = "-1";
        }
        else if (Clinical_PhysicalExam.params.mode == "Edit") {
            PhysicalExamId = Clinical_PhysicalExam.params.PhysicalExamId;
            //Clinical_PhysicalExam.PhysicalExamEdit(PhysicalExamId);
        }

        //Start//15-02-2016//Ahmad Raza//Loading dropdowns
        var self = $('#' + Clinical_PhysicalExam.params.PanelID);
        if (Clinical_PhysicalExam.bIsFirstLoad == true) {
            self.loadDropDowns(true);
        }
        //End//15-02-2016//Ahmad Raza//Loading dropdowns

        //$.when(Clinical_PhysicalExam.loadChildData(null, "mainpesystem")).then(function () {
        //    Clinical_PhysicalExam.toggleVerticalWidth();

        //});

        utility.CreateDatePicker(Clinical_PhysicalExam.params.PanelID + '  #dtPhysicalExamDate', function () {
        }, true);

        utility.CreateDatePicker(Clinical_PhysicalExam.params.PanelID + '  section#sectionFamilyMemberDetails div#FamilyMemberDetails #dtpYearofBirth', function () {
        }, false);

        if ($('#' + Clinical_PhysicalExam.params.PanelID + ' #PatientProfile #hfPatientId').val() != "") {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').data('serialize', $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').serialize());
        var self = $('#' + Clinical_PhysicalExam.params.PanelID + " #sectionPhysicalExamDetails");
        self.loadDropDowns(true).done(function (response) {
            var nmysres = "";
            var myResponse = response;

        });
        $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val();

        /*
           Change Implement BY: Muhammad Azhar Shahzad
           Reason:To Show navigation on Progress Note
           Created Date: Dec 15, 2015
       */
        //Code for progress note navigation
        if (Clinical_PhysicalExam.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_PhysicalExam.params.PanelID, '', 'PhysicalExam', 'Clinical_PhysicalExam.unLoadTab(true);', null, true);
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #btnAddVitalsOnNote').show();
            //$('#' + Clinical_PhysicalExam.params.PanelID + '  #dtPhysicalExamDate').prop('disabled', true);
        } else {
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #btnAddVitalsOnNote').hide();
            //$('#' + Clinical_PhysicalExam.params.PanelID + '  #dtPhysicalExamDate').prop('disabled', false);
        }
        //} // end of if condition
        //end change azhar Dec 15, 2015

        Clinical_PhysicalExam.domReadyFunction();
        if (!Clinical_PhysicalExam.params.isShowTemplate) {
            Clinical_PhysicalExam.loadTemplatePhysicalExam();
        }
    },

    //Author: Muhammad Arshad
    //Date:09/03/2016
    //This function will filter physical Exam Templates based on current ProviderNote's ProviderId/SpecialtyId

    isTemplateRelevant: function (currNotesProviderId, currentSpecialtyId, templateProviderIds, templateSpecialtyIds) {
        var isRelevant = false;
        if (templateProviderIds != null && templateProviderIds != "") {
            if (templateProviderIds.split(',').indexOf(currNotesProviderId) > -1) {
                isRelevant = true;
            }
            else {
                isRelevant = false;
            }
        }
        else if (templateSpecialtyIds != null && templateSpecialtyIds != "") {
            if (templateSpecialtyIds.split(',').indexOf(currentSpecialtyId) > -1) {
                isRelevant = true;
            }
            else {
                isRelevant = false;
            }
        }
        else if ((templateProviderIds == null || templateProviderIds == "") && (templateSpecialtyIds == null || templateSpecialtyIds == ""))
            isRelevant = true;
        return isRelevant;
    },

    //Author: Muhammad Arshad
    //Date:04/03/2016
    //This function will bind physical Exam Templates
    bindPhysExamTemplate: function (Crtl, arrComponents, providerId, specialtyId) {

        $(Crtl).find("tbody").find("tr").remove();
        var currentLiClass = "";
        var isFirstLi = true;
        var activeLiId;
        var activeLiComponentName;
        var ParentDiv = "view";

        $.each(arrComponents, function (i, item) {
            //Start 9 March 2016 Muhammad Arshad Filtering Templates
            //if (item.IsDefault.toLowerCase() == "true" || Clinical_PhysicalExam.isTemplateRelevant(providerId, specialtyId, item.ProviderIds, item.SpecialtyIds) == true) {

            if (item.IsActive == "True") {
                var $row = $('<tr/>');

                $row.attr("id", "gvPhysExamTemplate_row" + item.physExamTemplateId);
                $row.attr("physExamTemplateId", item.physExamTemplateId);
                $row.attr("isdefault", item.IsDefault);
                $row.attr("providerids", item.ProviderIds);
                $row.attr("specialtyids", item.SpecialtyIds);
                $row.attr("IsActive", item.IsActive);
                if (item.IsActive == "True") {
                    isactive = 0;
                    activeRecord = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeRecord = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }


                var actionTick = "Clinical_PhysicalExam.loadTemplateBasedPhysicalExam('" + item.physExamTemplateId + "','" + item.IsDefault + "');";
                $row.attr("onclick", "utility.SelectGridRow($('#gvPhysExamTemplate_row" + item.physExamTemplateId + "'));" + actionTick);
                var strAction = '<a class="btn btn-xs" href="#" onclick="' + actionTick + '" title="Select Record"><i class="fa fa-check black"></i></a>&nbsp;';
                $row.append('<td style="display:none;">' + item.physExamTemplateId + '</td><td>' + strAction + '</td><td>' + item.physExamTemplateName + '</td>');
                $(Crtl).find("tbody").last().append($row);
            }


            //}
            //End 9 March 2016 Muhammad Arshad Filtering Templates

        });
        Clinical_PhysicalExam.toggleVerticalWidth();
    },

    //Author: Ahmad Raza
    //Date: 23/06/2016
    //Function Name: bindPhysExamDataTemplate
    //This function will bind physical Exam Data Templates
    bindPhysExamDataTemplate: function (Crtl, arrComponents, providerId, specialtyId) {

        $(Crtl).find("tbody").find("tr").remove();
        var currentLiClass = "";
        var isFirstLi = true;
        var activeLiId;
        var activeLiComponentName;
        var ParentDiv = "view";

        $.each(arrComponents, function (i, item) {
            //Start 9 March 2016 Muhammad Arshad Filtering Templates
            //if (item.IsDefault.toLowerCase() == "true" || Clinical_PhysicalExam.isTemplateRelevant(providerId, specialtyId, item.ProviderIds, item.SpecialtyIds) == true) {
            var $row = $('<tr/>');

            $row.attr("id", "gvPhysExamDataTemplate_row" + item.physExamDataTemplateId);
            $row.attr("physExamDataTemplateId", item.physExamDataTemplateId);
            $row.attr("physExamTemplateId", item.physExamTemplateId);
            $row.attr("isdefault", item.IsDefault);
            $row.attr("providerids", item.ProviderIds);
            $row.attr("specialtyids", item.SpecialtyIds);
            $row.attr("IsActive", item.IsActive);
            if (item.IsActive == "True") {
                isactive = 0;
                activeRecord = "Active Record";
                tglclass = "fa fa-toggle-on green";
            }
            else {
                isactive = 1;
                activeRecord = "Inactive Record";
                tglclass = "fa fa-toggle-on red";
            }

            $row.attr("onclick", "utility.SelectGridRow($('#gvPhysExamDataTemplate_row" + item.physExamDataTemplateId + "'))");
            var actionTick = "Clinical_PhysicalExam.loadDataTemplateBasedPhysicalExam('" + item.physExamDataTemplateId + "','" + item.physExamTemplateId + "','" + item.IsDefault + "');";
            var strAction = '<a class="btn btn-xs" href="#" onclick="' + actionTick + '" title="Select Record"><i class="fa fa-check black"></i></a>&nbsp;';
            $row.append('<td style="display:none;">' + item.physExamDataTemplateId + '</td><td>' + strAction + '</td><td>' + item.physExamDataTemplateName + '</td>');
            $(Crtl).find("tbody").last().append($row);
            //}
            //End 9 March 2016 Muhammad Arshad Filtering Templates

        });
        Clinical_PhysicalExam.toggleVerticalWidth();
    },




    //Author: Muhammad Arshad
    //Date:04/03/2016
    //This function will load physical Exam Templates
    loadPhysExamTemplates: function () {
        var providerId = Clinical_PhysicalExam.params["CurrentNotesProviderId"];
        var specialtyId = "";
        Admin_Provider.SearchProvider(null, providerId, 1, 15).done(function (response) {
            if (response.status == true) {
                var myJSON = JSON.parse(response.ProviderLoad_JSON);
                specialtyId = myJSON[0].SpecialtyId;
                providerId = myJSON[0].ProviderId;
                Clinical_PhysicalExam.loadPhysicalExamTemplate(providerId, specialtyId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //Clinical_PhysicalExam.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadPhysicalExam);
                        var arrTemplates = JSON.parse(response.PhysExamTemplateFill_JSON);
                        var ctrlTemplates = $('#' + Clinical_PhysicalExam.params.PanelID + ' #dgvPhysExamTemplates');//$('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysExamTemplates');
                        Clinical_PhysicalExam.bindPhysExamTemplate(ctrlTemplates, arrTemplates, providerId, specialtyId);
                        if (Clinical_PhysicalExam.params.PhysicalExamId == "-1") {
                            var objtabPhysicalExamTemplates = $('#' + Clinical_PhysicalExam.params.PanelID + ' #LiPhysicalExamTemplates');
                            objtabPhysicalExamTemplates.find("a").trigger("click");
                        }
                        //Clinical_PhysicalExam.showHideAddButton();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        });
    },

    //Author: Ahmad Raza
    //Date: 23/06/2016
    //Function Name: loadPhysExamDataTemplates
    //This function will load physical Exam Data Templates
    loadPhysExamDataTemplates: function () {
        var providerId = Clinical_PhysicalExam.params["CurrentNotesProviderId"];
        var specialtyId = "";
        Admin_Provider.SearchProvider(null, providerId, 1, 15).done(function (response) {
            if (response.status == true) {
                var myJSON = JSON.parse(response.ProviderLoad_JSON);
                specialtyId = myJSON[0].SpecialtyId;
                providerId = myJSON[0].ProviderId;
                Clinical_PhysicalExam.loadPhysicalExamDataTemplate(providerId, specialtyId).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            //Clinical_PhysicalExam.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadPhysicalExam);
                            var arrTemplates = JSON.parse(response.PhysExamDataTemplateFill_JSON);
                            var ctrlTemplates = $('#' + Clinical_PhysicalExam.params.PanelID + ' #dgvPhysExamDataTemplates');//$('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysExamTemplates');
                            Clinical_PhysicalExam.bindPhysExamDataTemplate(ctrlTemplates, arrTemplates, providerId, specialtyId);
                            var objtabPhysicalExamDataTemplates = $('#' + Clinical_PhysicalExam.params.PanelID + ' #LiPhysicalExamDataTemplates');
                            objtabPhysicalExamDataTemplates.find("a").trigger("click");
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
        });
    },



    //Author: Muhammad Arshad
    //Date:07/03/2016
    //This function will show physical Exam Templates Tab
    showPhysExamTemplatesTab: function () {

        var PhysicalExamType = $('#' + Clinical_PhysicalExam.params.PanelID + "  #hfPhysicalExamType").val();
        var isDetailExists = Clinical_PhysicalExam.isDetailExists();

        if (isDetailExists && Clinical_PhysicalExam.oldTemplateId == Clinical_PhysicalExam.currentTemplateId) {

            utility.myConfirm('19', function () {

                Clinical_PhysicalExam.PhysicalExamSave(PhysicalExamType, true).done(function () {

                    Clinical_PhysicalExam.showHideAddButton();
                });

            }, function () {
                Clinical_PhysicalExam.showHideAddButton();
                //Clear  details
                Clinical_PhysicalExam.resetDetails();
            });
        }
        else {
            Clinical_PhysicalExam.showHideAddButton();
        }
    },

    resetDetails: function () {

        var self = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #PhysicalExam");
        var objCharacteristic = self.find("div#divExamCharacteristics");
        var objSubCharacteristic = self.find("section#CharacteristicsSubCharacteristics");

        objSubCharacteristic.find("ul#ulExamSubCharacteristics").empty();
        objCharacteristic.find("ul#ulExamCharacteristics").empty();
    },

    showHideAddButton: function () {

        Clinical_PhysicalExam.isFirstLoad = true;
        Clinical_PhysicalExam.TemplateDataJSON = [];
        var objtabPhysicalExamTemplates = $('#' + Clinical_PhysicalExam.params.PanelID + ' #LiPhysicalExamTemplates');
        objtabPhysicalExamTemplates.trigger("click");
        var objtabAddPhysExam = $('#' + Clinical_PhysicalExam.params.PanelID + ' #LiAddPhysicalExam');
        objtabAddPhysExam.addClass("hidden");
    },

    showPhysExamDataTemplatesTab: function () {

        Clinical_PhysicalExam.isFirstLoad = true;
        var objtabPhysicalExamDataTemplates = $('#' + Clinical_PhysicalExam.params.PanelID + ' #LiPhysicalExamDataTemplates');
        objtabPhysicalExamDataTemplates.trigger("click");
        var objtabAddPhysExam = $('#' + Clinical_PhysicalExam.params.PanelID + ' #LiAddPhysicalExam');
        objtabAddPhysExam.addClass("hidden");
    },

    //Author: Muhammad Arshad
    //Date:04/03/2016
    //This function will load physical Exam based on selected TemplateId
    loadTemplateBasedPhysicalExam: function (templateId, isDefaultTemplate) {

        Clinical_PhysicalExam.TemplateType = '';

        if (Clinical_PhysicalExam.isFirstLoad == false) {
            event.stopPropagation();
        }
        else {
            Clinical_PhysicalExam.SelectedTempId = templateId;
            if (isDefaultTemplate != null && isDefaultTemplate.toLowerCase() == "true") {
                templateId = -1;
            }

            Clinical_PhysicalExam.currentTemplateId = templateId;

            $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val(templateId);

            var objtabPhysicalExamTemplates = $('#' + Clinical_PhysicalExam.params.PanelID + ' #LiPhysicalExamTemplates');
            objtabPhysicalExamTemplates.removeClass("active");

            var objtabAddPhysicalExam = $('#' + Clinical_PhysicalExam.params.PanelID + ' #LiAddPhysicalExam');
            objtabAddPhysicalExam.removeClass("hidden");

            objtabAddPhysicalExam.find("a").trigger("click");

            //Start 11-02-2016 Humaira Yousaf to load physical exam systems after physical exam loads completely
            $.when(Clinical_PhysicalExam.loadPhysicalExam("", true)).done(function (selectedData) {

                Clinical_PhysicalExam.loadPhysicalExamSystem(selectedData);
            });
            //End 11-02-2016 Humaira Yousaf to load physical exam systems after physical exam loads completely
        }
    },

    //Author: Ahmad Raza
    //Date: 23/06/2016
    //Function Name: loadDataTemplateBasedPhysicalExam
    //This function will load physical Exam Data Templates based
    loadDataTemplateBasedPhysicalExam: function (dataTemplateId, templateId, isDefaultTemplate) {

        Clinical_PhysicalExam.TemplateType = "DataTemplate"
        if (isDefaultTemplate != null && isDefaultTemplate.toLowerCase() == "true") {
            dataTemplateId = -1;
            return false;
        }

        if (dataTemplateId > 0) {

            Clinical_PhysicalExam.clearPhysicalExamFieldsAndProperties();

            $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val(templateId);
            var objtabPhysicalExamTemplates = $('#' + Clinical_PhysicalExam.params.PanelID + ' #LiPhysicalExamDataTemplates');
            objtabPhysicalExamTemplates.removeClass("active");
            var objtabAddPhysicalExam = $('#' + Clinical_PhysicalExam.params.PanelID + ' #LiAddPhysicalExam');
            objtabAddPhysicalExam.removeClass("hidden");
            objtabAddPhysicalExam.find("a").trigger("click");

            Clinical_PhysicalExam.loadTemplateData(dataTemplateId).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);

                    if (response.status != false) {

                        var txtComments = $('#' + Clinical_PhysicalExam.params.PanelID + " #txtComments");
                        txtComments.val(response.Comments);
                        txtComments.text(response.Comments);

                        Clinical_PhysicalExam.TemplateDataJSON = JSON.parse(response.patientPhysicalExamSystems);

                        Clinical_PhysicalExam.fillPhysicalExam('').done(function (response) {
                            if (response != "") {
                                response = JSON.parse(response);
                                Clinical_PhysicalExam.loadPhysicalExamSystem(response.patientPhysicalExamSystemsFill_JSON);
                            }
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                }
            });
        }
    },

    loadDataTemplateBasedPhysicalExamFromProviderNotes: function (dataTemplateId, templateId, isDefaultTemplate) {
        var objDeffered = $.Deferred();
        Clinical_PhysicalExam.TemplateType = "DataTemplate"
        if (isDefaultTemplate != null && isDefaultTemplate.toLowerCase() == "true") {
            dataTemplateId = -1;
        }
        if (dataTemplateId > 0) {
            Clinical_PhysicalExam.loadTemplateData(dataTemplateId).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_PhysicalExam.TemplateDataFromProviderNotesJSON = JSON.parse(response.patientPhysicalExamSystems);
                        objDeffered.resolve();
                    }
                    else {
                        objDeffered.resolve();
                        utility.DisplayMessages(response.Message, 3);
                    }
                }
            });
        }
        else {
            objDeffered.resolve();
        }
        $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val(templateId);
        return objDeffered;
    },

    PhysicalExamSaveFromProviderNotes: function (PEId, PhysicalExamType, UnloadPhysicalExam, templateId, NotesID) {

        var dfd = $.Deferred();
        var PhysicalExamId = PEId;

        if (parseInt(PhysicalExamId) > 0) {
            Clinical_PhysicalExam.params.mode = "Edit";
        }
        else {
            Clinical_PhysicalExam.params.mode = "Add";
        }

        var obj = new Object();
        obj.TemplateId = templateId;
        //if (obj.TemplateId.toString() == '1') {
        //    obj.TemplateId = null;
        //}

        obj.PatientId = $('#PatientProfile #hfPatientId').val();
        obj.NotesId = NotesID;
        obj.Comments = "";
        obj.Systems = Clinical_PhysicalExam.TemplateDataFromProviderNotesJSON;
        obj.PatientPhysicalExamId = PhysicalExamId;
        obj.PatientPhysicalExamDate = new Date($.now());
        obj.NormalComments = "";
        obj.bNormal = false;

        if (Clinical_PhysicalExam.params.mode == "Add") {

            AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Clinical_PhysicalExam.savePhysicalExamFromNotes(obj).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val(response.PhysicalExamId);
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val(response.NoteId);
                            Clinical_PhysicalExam.params.mode = "Edit";

                            Clinical_PhysicalExam.getPhysicalExamInfoFromProvidersNote(PhysicalExamType, UnloadPhysicalExam, response.PhysicalExamId);

                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').data('serialize', $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').serialize());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                        dfd.resolve();
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (Clinical_PhysicalExam.params.mode == "Edit") {

            AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Clinical_PhysicalExam.updatePhysicalExam(obj, PhysicalExamId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            if (Clinical_PhysicalExam.params.ParentCtrl == "clinicalTabProgressNote" && UnloadPhysicalExam == true) {
                                Clinical_PhysicalExam.getPhysicalExamInfoFromProvidersNote(PhysicalExamType, UnloadPhysicalExam, response.PhysicalExamId);
                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                            }
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val(response.PhysicalExamId);
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val(response.NotesId);
                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').data('serialize', $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').serialize());
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                        dfd.resolve();
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        return dfd.promise();
    },

    fillPhysicalExamForProvidersNoteSoap: function (physicalExamId) {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        var PhysicalExamId = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val();
        objData["PatientPhysicalExamId"] = physicalExamId;

        objData["commandType"] = "FILL_PatientPhysicalExam";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },
    //Author: Farooq Ahmad
    //Date: 04/02/2016
    //This function will load PhysicalExamSystems
    loadPhysicalExamSystem: function (selectedData) {

        Clinical_PhysicalExam.fillPhysicalExamUserSystem().done(function (response) {
            if (response != "") {

                response = JSON.parse(response);
                if (response.status != false) {

                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #SystemSections').addClass('hidden');
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #SectionCharacteristics').addClass('hidden');
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #CharacteristicsSubCharacteristics').addClass('hidden');
                    Clinical_PhysicalExam.showHideToggleDetails(false);

                    if (Clinical_PhysicalExam.SelectedTempId && Clinical_PhysicalExam.DBTemplateId && Clinical_PhysicalExam.SelectedTempId != Clinical_PhysicalExam.DBTemplateId) {
                        if (Clinical_PhysicalExam.DBDataJSON.length > 0)
                            Clinical_PhysicalExam.DataJSONTemp = Clinical_PhysicalExam.DBDataJSON;

                        if (Clinical_PhysicalExam.DataJSONTemp.length == 0) {
                            Clinical_PhysicalExam.DataJSONTemp = Clinical_PhysicalExam.DataJSON;
                        }

                        Clinical_PhysicalExam.DBDataJSON = [];
                        Clinical_PhysicalExam.DataJSON = [];
                    }
                    else if (Clinical_PhysicalExam.SelectedTempId == Clinical_PhysicalExam.DBTemplateId && Clinical_PhysicalExam.DataJSONTemp.length > 0) {
                        Clinical_PhysicalExam.TemplateDataJSON = Clinical_PhysicalExam.DataJSON = Clinical_PhysicalExam.DataJSONTemp;
                        Clinical_PhysicalExam.DataJSONTemp = [];
                    }

                    if (Clinical_PhysicalExam.TemplateDataJSON.length == 0) {
                        if (Clinical_PhysicalExam.DBDataJSON.length > 0) {
                            Clinical_PhysicalExam.DataJSON = [];

                            for (var index in Clinical_PhysicalExam.DBDataJSON) {
                                if (Clinical_PhysicalExam.DBDataJSON[index].PhysicalExamSystemId != null && parseInt(Clinical_PhysicalExam.DBDataJSON[index].PhysicalExamSystemId) > 0) {
                                    Clinical_PhysicalExam.DataJSON.push(Clinical_PhysicalExam.DBDataJSON[index]);
                                }
                            }
                        }
                    }
                    else {

                        Clinical_PhysicalExam.DataJSON = [];
                        for (var index in Clinical_PhysicalExam.TemplateDataJSON) {
                            Clinical_PhysicalExam.DataJSON.push(Clinical_PhysicalExam.TemplateDataJSON[index]);
                        }
                    }

                    //Start//14-04-2016//Ahmad Raza//logic to load template based Physical Exam
                    var templateID = $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val();
                    if (templateID != null && templateID > 0 && Clinical_PhysicalExam.TemplateType != 'DataTemplate') {
                        Clinical_PhysicalExam.loadTemplateBasedPhysicalExam(templateID, 'False');
                        Clinical_PhysicalExam.isFirstLoad = false;
                    }
                    else if (templateID == "") {
                        Clinical_PhysicalExam.loadTemplateBasedPhysicalExam('1', 'True');
                    }

                    Clinical_PhysicalExam.isFirstLoad = false;

                    var arrComponents = JSON.parse(response.PhysicalExamSystem_JSON);

                    Clinical_PhysicalExam.selectedData = selectedData;
                    Clinical_PhysicalExam.bindPhysicalExamUserSystem($('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystems'), arrComponents, null, null, null, selectedData);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {
                $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystems').empty();
                $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #SystemSections,#SectionCharacteristics,#CharacteristicsSubCharacteristics,#sectionPhysicalExamDetails,#divTogglePhysicalExamDetails').addClass('hidden');
                $('#' + Clinical_PhysicalExam.params.PanelID + ' #btnResetPhysicalExam,#btnAddVitalsOnNote').addClass('hidden');
            }
        });
    },

    //Function Name: setIsNormalSystems
    //Author Name: Humaira Yousaf
    //Created Date: 11-02-2016
    //Description: Sets normal system green on load
    //Params: var normalSystemIds
    setIsNormalSystems: function (normalSystemIds) {
        Clinical_PhysicalExam.SectionNormalInfo = [];
        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystems li').each(function (index, item) {

            for (var i = 0; i < normalSystemIds.length; i++) {
                if ($(item).attr('id') == normalSystemIds[i]) {
                    $(item).addClass("green");
                    Clinical_PhysicalExam.SectionNormalInfo.push(parseInt(normalSystemIds[i]));
                    break;
                }
            }
        });
    },


    //Author: Farooq Ahmad
    //Date:04/02/2016
    //This function will bind user systems to form
    bindPhysicalExamUserSystem: function (Crtl, arrComponents, currentLiClick, ParentDiv, StatusType, selectedData) {

        if (typeof selectedData == "undefined" || selectedData == null)
            selectedData = Clinical_PhysicalExam.selectedData;

        var l = $(Crtl);
        l.empty();

        var systemsDetail = JSON.parse(selectedData);
        var currentLiClass;
        var isFirstLi = true;
        var activeLiId;
        var activeLiComponentName;

        $.each(arrComponents, function (j, item) {
            if (item.SystemOrder != "") {
                if (isFirstLi == true) {
                    currentLiClass = 'class="active"';

                    activeLiId = item.SystemOrder;
                    activeLiComponentName = item.ShortName.toLowerCase();

                    isFirstLi = false;
                }
                else {
                    currentLiClass = "";
                }

                var onClick = "";

                onClick = "Clinical_PhysicalExam.showHideChildControls('ulPhysicalExamSystems', '" + item.SystemId + "', event);"
                l.append('<li id="' + item.SystemId + '" ' + currentLiClass + ' onmouseover="Clinical_PhysicalExam.showDeleteIcon(this)" onmouseout="Clinical_PhysicalExam.hideDeleteIcon(this)"  onclick="' + onClick + '" value=' + item.SystemOrder + ' refValue="' + item.SystemOrder + '"><a href="#' + ParentDiv + '">' + item.ShortName + ' <span style="dispaly:none;" class="removeIconListHover" onclick="Clinical_PhysicalExam.physicalExamDelete(' + item.SystemId + ')"><i class="fa fa-times"></i></span></a></li>');
            }
        });

        Clinical_PhysicalExam.toggleVerticalWidth();

        //4/2/2016 Farooq Ahmad//For Sortable
        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystems').sortable({

            update: function (evt) {

                var obj = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystems').sortable('toArray');
                var OrderOfExams = '';

                $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystems li').each(function () {
                    OrderOfExams = $(this).attr("value") + ',' + OrderOfExams;
                });

                if (OrderOfExams.length > 0) {
                    OrderOfExams = OrderOfExams.substring(0, OrderOfExams.length - 1);
                    Clinical_PhysicalExam.updateSectionOrderSorting(OrderOfExams);
                }
            }
        });

        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystems').disableSelection();

        Clinical_PhysicalExam.addGreenClasses();
    },

    //Author: Farooq Ahmad
    //Date:04/02/2016
    //This function will show Delete Icon of li
    showDeleteIcon: function (li) {

        if ($(li).hasClass('green'))
            $(li).find(".removeIconListHover").show();
    },

    //Author: Farooq Ahmad
    //Date:04/02/2016
    //This function will hide Delete Icon of li
    hideDeleteIcon: function (li) {
        $(li).find(".removeIconListHover").hide();
    },

    //Author: Farooq Ahmad
    //Date:04/02/2016
    //This function will call Db to load user systems
    fillPhysicalExamUserSystem: function () {
        var objData = new Object();
        var PhysicalExamId = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val();
        //Start 11-02-2016 Humaira Yousaf to get normal systems of physical exam
        objData["PatientPhysicalExamId"] = (PhysicalExamId == "" || PhysicalExamId == null) ? -1 : PhysicalExamId;
        //End 11-02-2016 Humaira Yousaf to get normal systems of physical exam
        objData["commandType"] = "PhysicalExam_UserSystemLoad";

        var templateId = $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val();


        objData["TemplateId"] = (templateId == "" || templateId == 1) ? -1 : templateId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExamUserSystem");
    },


    // Date: 04/02/2016
    // Author: Farooq Ahmad
    //This function will handle sorting of user systems
    updateSectionOrderSorting: function (SectionSorted) {
        var strMessage = "";

        Clinical_PhysicalExam.updateSectionOrderSorting_Dbcall(SectionSorted).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var arrComponents = JSON.parse(response.PhysicalExamSystem_JSON);
                    Clinical_PhysicalExam.bindPhysicalExamUserSystem($('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystems'), arrComponents);

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });

    },


    // Date: 04/02/2016
    // Author: Farooq Ahmad
    //This function will handle db call to sort user systems
    updateSectionOrderSorting_Dbcall: function (SectionSorted) {
        var objData = new Object();
        objData["SystemCustomSorted"] = SectionSorted;
        objData["UserId"] = globalAppdata['AppUserId'];
        objData["commandType"] = "UPDATE_SECTIONORDERSORTING";
        objData["TemplateId"] = $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExamUserSystem");
    },

    loadTemplateData: function (dataTemplateId) {
        var objData = new Object();
        objData["DataTemplateId"] = dataTemplateId;
        // objData["UserId"] = globalAppdata['AppUserId'];
        objData["commandType"] = "load_physcialexam_template_data";
        // objData["TemplateId"] = $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    //Author: Muhammad Arshad
    //Date: 14-01-2016
    //This function will handle Initialization of KeyPad control
    domReadyFunction: function () {
        $(function () {


            $('#' + Clinical_PhysicalExam.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
            //EMR-70 Bug number Resolution
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam [data-plugin-keyboard-numpad]').keyboard({
                customLayout: {
                    'default': [
                        '7 8 9 {b}',
                        '4 5 6 {clear}',
                        '1 2 3 {t}',
                        '0   .  {a} {c} '
                    ]
                },
                change: function (e, keyboard, el) {
                    if (keyboard.$preview.attr('maxlength') != null && !keyboard.$preview.keyboard().getkeyboard().options.maxLength) {
                        keyboard.$preview.keyboard().getkeyboard().options.maxLength = keyboard.$preview.attr('maxlength');
                    }
                    if (keyboard.$preview.attr('oninput') != null) {
                        keyboard.$preview.trigger('oninput');
                    }
                    // Fix # EMR-96
                    if (keyboard.$preview.attr('name') == 'Height') {
                        if (keyboard.$preview.attr('onkeyup') != null) {
                            keyboard.$preview.trigger('onkeyup');
                            EMRUtility.ValidateHeight(e, keyboard.$preview);
                        }
                    } else if (keyboard.$preview.attr('onkeyup') != null) {
                        keyboard.$preview.trigger('onkeyup');
                    }

                },
                layout: 'custom',
                reposition: true,
                appendLocally: this,
                restrictInput: true, // Prevent keys not in the displayed keyboard from being typed in
                preventPaste: true,  // prevent ctrl-v and right click
                usePreview: false,
                autoAccept: true,
                tabNavigation: true
            })
                  .addTyping();

        });

        $(".toggleVertical div.toggle").click(function (e) {
            if ($(this).children().hasClass("active")) {
                $(this).prev().removeClass("hidden");

                Clinical_PhysicalExam.toggleVerticalWidth(e);
                $(this).parent().parent().scrollLeft(1000);
            }
            else {
                $(this).prev().addClass("hidden");
                Clinical_PhysicalExam.toggleVerticalWidth(e);
                $(this).parent().parent().scrollLeft(0);
            }
        });

        $('[data-plugin-toggle]').each(function () {
            var $this = $(this),
                opts = {};

            var pluginOptions = $this.data('plugin-options');
            if (pluginOptions)
                opts = pluginOptions;

            $this.themePluginToggle(opts);
        });


        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails').find('select,input,textarea').each(function () {

            $(this).change(function () {
                Clinical_PhysicalExam.UpdateJSONDetailSection();
                //alert('update JSON For detail');
            });


            //.on('change', function () {
            //                alert('update JSON For detail');
            //            });

        });

        $('#' + Clinical_ProgressNote.params.PanelID + ' #actionPanClinicalProgressNote').scroll(function () {
            if ($('.datepicker-dropdown').length > 0) {
                $('.datepicker-dropdown').remove();
                $('#' + Clinical_PhysicalExam.params.PanelID + ' #dtPhysicalExamDate').focusout();
                $('#' + Clinical_PhysicalExam.params.PanelID + ' #dtPhysicalExamDate').trigger("focusout");
            }
        });

    },

    //Author: Muhammad Arshad
    //Date: 18-01-2016
    //This function will handle filtering of PhysicalExam Characteristics/Sub Characteristics
    filterOptions: function (obj, ulId) {
        if (obj != null && ulId != null) {
            var strSearch = $(obj).val();
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #" + ulId + " li").each(function () {
                var currentLiText = $(this).text();
                var showCurrentLi = currentLiText.toLowerCase().indexOf(strSearch.toLowerCase()) > -1 ? true : false;
                $(this).toggle(showCurrentLi);
            });

        }
    },

    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will check if any characteristic/subcharacteristic is selected
    isCharacteristicChecked: function (objLi) {
        var DetailExists = false;
        objulSystem.find("li").each(function (i, item) {
            $(objLi).find("input[type='checkbox']").each(function (i, chkitem) {
                if ($(chkitem).prop("checked") == true && DetailExists == false) {
                    DetailExists = true;
                }
            });
        });

        return DetailExists
    },

    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will return Characteristic Type
    getCharacteristicType: function (liObject) {
        var isSubcharacteristic = $(liObject).parent().attr("id").indexOf("SubCharacteristics") > -1 ? true : false;
        var characteristicType = isSubcharacteristic == true ? "Sub-Characteristic" : "Characteristic";
        return characteristicType;
    },


    //Author: Farooq Ahmad
    //Date : 16-06-2016
    ///this function will Store the System Normal Comments in JSON
    saveSystemNormalCommentInJSON: function (txt) {
        var comments = $(txt).val();
        //var SystemId = $(txt).closest("ul").attr("parentId");
        //for (var index in Clinical_PhysicalExam.DataJSON) {
        //    if (Clinical_PhysicalExam.DataJSON[index].SystemId == SystemId) {
        //        Clinical_PhysicalExam.DataJSON[index].Comments = comments;
        //    }
        //}
    },

    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will handle show/hide of Detail Textbox
    openNormalExamDetail: function (objButton, detailParentId) {

        if (objButton != null) {

            if ($('#' + Clinical_PhysicalExam.params.PanelID + " #txtNormalExamsDetail").val() != "") {
                setTimeout(function () {
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails").addClass('green');
                    try {
                        if ($('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails").length > 0) {
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails")[0].style.setProperty('color', 'green', 'important');
                        }
                    }
                    catch (ex) {
                        console.log(ex);
                    }
                }, 100)
                setTimeout(function () {
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails").addClass('green');
                    try {
                        if ($('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails").length > 0) {
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails")[0].style.setProperty('color', 'green', 'important');
                        }
                    }
                    catch (ex) {
                        console.log(ex);
                    }
                }, 200)

            }
            else {
                $('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails").removeClass('green');
                try {
                    if ($('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails").length > 0) {
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails")[0].style.setProperty('color', '#0088cc', 'important');
                    }
                }
                catch (ex) {
                    console.log(ex);
                }
            }

            var self = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam")
            var isMainNormalChecked = self.find("input[id='chkPhysicalExamsNormal']").prop("checked");
            if (isMainNormalChecked == true) {
                var NormalExamsDetailDiv = self.find("div#divNormalExamsDetail");
                if (NormalExamsDetailDiv.hasClass("hidden") == true) {
                    NormalExamsDetailDiv.removeClass("hidden")
                }
                else {

                    NormalExamsDetailDiv.addClass("hidden")
                }
            }
            else {
                utility.DisplayMessages("Please check Normal Exam to view details", 3);
            }

        }
    },

    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will handle show/hide of Detail Textbox
    openCharacteristicDetail: function (objButton, detailParentId) {
        if (objButton != null && detailParentId != null) {
            var liObject = $(objButton).parent().parent();
            var characteristicType = Clinical_PhysicalExam.getCharacteristicType(liObject);
            var pkid = $(liObject).prop("id");
            var type = $(liObject).closest("ul").prop("id");
            var isParentChecked = Clinical_PhysicalExam.isSystemChecked(null, liObject);
            if (isParentChecked == true) {
                var SystemDetailDiv = $(objButton).parent().find("div#divDetail" + detailParentId);
                if (SystemDetailDiv.hasClass("hidden") == true) {
                    SystemDetailDiv.removeClass("hidden");
                    var comments = Clinical_PhysicalExam.FillCommentsFromJSON(pkid, type)
                    $(SystemDetailDiv).find('textarea').val(comments);
                    $(SystemDetailDiv).find('textarea').text(comments);
                    $(SystemDetailDiv).find('textarea').focus();
                }
                else {
                    SystemDetailDiv.addClass("hidden")
                    var comments = $(SystemDetailDiv).find('textarea').val();
                    Clinical_PhysicalExam.updateTheCommentsOfCharAndSubChar(comments, pkid, type);
                    $(SystemDetailDiv).find('textarea').focusout();
                }
            }
            else {
                utility.DisplayMessages("Please mark this " + characteristicType + " as +ve/-ve to view detail", 3);
            }
            //.removeClass("hidden");
        }
    },


    FillCommentsFromJSON: function (pkId, type) {

        if (type.toLowerCase() == "ulexamsubcharacteristics") {

            for (var index in Clinical_PhysicalExam.DataJSON) {
                for (var secIndex in Clinical_PhysicalExam.DataJSON[index].Sections) {
                    for (var charIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics) {
                        for (var subcharIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                            if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId == pkId) {
                                return Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].Comments;
                            }
                        }
                    }
                }
            }
        }
        else if (type.toLowerCase() == "ulexamcharacteristics") {

            for (var index in Clinical_PhysicalExam.DataJSON) {
                for (var secIndex in Clinical_PhysicalExam.DataJSON[index].Sections) {
                    for (var charIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics) {
                        if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == pkId) {
                            return Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].Comments;
                        }
                    }
                }
            }
        }
        return "";
    },
    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will handle save/update of Detail checkbox
    saveCharacteristicDetail: function (obj, ctrlDetail, parentLiId, isSubCharacteristic) {
        try {
            var txt = $(obj).closest("li").find('#' + ctrlDetail);
            var comments = $(txt).val();
            if (comments == '') {
                utility.DisplayMessages("please enter comments to save.", 3);
                return;
            }
            var pkid = $(txt).closest('li').attr('id');
            var type = $(txt).closest('ul').attr('id');
            Clinical_PhysicalExam.updateTheCommentsOfCharAndSubChar(comments, pkid, type);
            var PhysicalExamId = Clinical_PhysicalExam.params.PhysicalExamId;
            var PhysicalExamType = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamType").val();
            Clinical_PhysicalExam.PhysicalExamSave(PhysicalExamType, false);
            $(txt).closest('li').find("div[id*='divDetail']").addClass("hidden");
        } catch (ex) {
            console.log(ex);
        }

    },

    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will Save Normal system's inner detail
    saveExamSystemNormalDetail: function (obj, ctrlDetail, parentLiId) {
        Clinical_PhysicalExam.updateJSONArraySystem(parentLiId);
        var PhysicalExamId = Clinical_PhysicalExam.params.PhysicalExamId;
        var PhysicalExamType = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamType").val();
        Clinical_PhysicalExam.PhysicalExamSave(PhysicalExamType, false);
        Clinical_PhysicalExam.openNormalSectionDetail('toggle');
        if ($('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first textarea").val() != '') {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first i.fa-book").removeClass("blue");
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first i.fa-book").addClass("green");
        } else {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first i.fa-book").addClass("blue");
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first i.fa-book").removeClass("green");
        }
    },
    //End//18-02-2016//Ahmad Raza//Saving Normal system's inner detail

    //Author: Farooq Ahmad
    //Date: 13-06-2016
    //This function will handle show/hide of PhysicalExam child controls

    showHideChildControls: function (parentCtrl, liId, event) {

        //try {
        //    if (event.target.parentNode.className == "removeIconListHover") {
        //        //$('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamSystems  li#' + liId).removeClass('green');
        //        //$('#' + Clinical_PhysicalExam.params.PanelID + ' #SystemSections').addClass('hidden');
        //        //$('#' + Clinical_PhysicalExam.params.PanelID + ' #SectionCharacteristics').addClass('hidden');
        //        //$('#' + Clinical_PhysicalExam.params.PanelID + ' #CharacteristicsSubCharacteristics').addClass('hidden');
        //        //Clinical_PhysicalExam.showHideToggleDetails(false);
        //        return;
        //    }
        //}
        //catch (ex) {
        //console.log(ex);
        //}

        Clinical_PhysicalExam.parentCtrlGlobel = parentCtrl;

        //$('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #sectionPhysicalExamDetails").addClass('hidden');

        if (parentCtrl == "ulPhysicalExamSystems") {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divTogglePhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SystemSections").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SectionCharacteristics").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics").addClass('hidden');
        }
        if (parentCtrl == "ulPhysicalExamSystems" && liId != $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li.active").attr("id")) {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divTogglePhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SectionCharacteristics").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics").addClass('hidden');
        }
        else if ((parentCtrl == "ulPhysicalExamSystemSection" && liId != $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li.active").attr("id"))) {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divTogglePhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics").addClass('hidden');

        }

        if (parentCtrl != null && parentCtrl != "") {

            var childPartialId = "";
            var isSystemSectionCtrl = "";
            var isCharacteristicsCtrl = "";
            var isSubCharacteristicsCtrl = "";

            if (parentCtrl.toLowerCase() == "ulphysicalexamsystems") {
                isSystemSectionCtrl = "1";
                childPartialId = "System";
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section[id^='SectionCharacteristics']").addClass("hidden");

            }
            else if (parentCtrl.toLowerCase() == "ulphysicalexamsystemsection") {
                isSystemSectionCtrl = "1";
                childPartialId = "Section";
            }
            else if (parentCtrl.toLowerCase() == "ulexamcharacteristics") {
                isCharacteristicsCtrl = "1";
                childPartialId = "Characteristics";
            }
            else if (parentCtrl.toLowerCase() == "ulexamsubcharacteristics") {
                isSubCharacteristicsCtrl = "1";
                childPartialId = "SubCharacteristics";
            }

            if (liId != null && liId != "") {
                $('#' + Clinical_PhysicalExam.parentCtrlGlobel).find("li").each(function (i, item) {
                    if ($(this).attr("id") != null && $(this).attr("id") == liId) {
                        $(this).closest("ul").find("li").removeClass('active');
                        $(this).addClass('active');
                        var objCurrent = item;
                        $.when(Clinical_PhysicalExam.loadChildData(liId, childPartialId)).then(function () {
                            Clinical_PhysicalExam.toggleDetailsDiv();
                            var objectListItem = null;
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam").css('width', 'auto');
                            if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystems") {
                                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SystemSections").removeClass('hidden');
                                //Clinical_PhysicalExam.manageJsonData(liId, 'system');
                            }
                            else if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystemsection") {
                                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SectionCharacteristics").removeClass('hidden');
                            }
                            else if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics") {
                                objectListItem = $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamCharacteristics  li#' + liId);
                                var isbothUnCheck = ($(objectListItem).find('input[type=checkbox]:checked').length == 0);

                                var haveChild = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamSubCharacteristics li").length > 0;

                                if (!isbothUnCheck && haveChild)
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics").removeClass('hidden');
                                else
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics").addClass('hidden');

                                try {
                                    var detail = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails");

                                    Clinical_PhysicalExam.manageJsonData(liId, 'characteristics', detail);


                                } catch (ex) {
                                    console.log(ex);
                                }

                            }
                            else if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulexamsubcharacteristics") {
                                try {
                                    objectListItem = $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamSubCharacteristics  li#' + liId);
                                    var detail = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails");

                                    Clinical_PhysicalExam.manageJsonData(liId, 'subcharacteristics', detail);
                                } catch (ex) {
                                    console.log(ex);
                                }

                                isSubCharacteristicsCtrl = "1";
                                childPartialId = "SubCharacteristics";
                            }
                            if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics" || Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulexamsubcharacteristics") {

                                var hideDetailSection = true;
                                if ($(objectListItem).find('input[type=checkbox]:checked').length == 0) {
                                    // Clinical_PhysicalExam.showHideToggleDetails(false);
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #sectionPhysicalExamDetails").addClass("hidden");
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divTogglePhysicalExamDetails").addClass("hidden");
                                } else {
                                    Clinical_PhysicalExam.toggleDetailsDiv();
                                    Clinical_PhysicalExam.showHideToggleDetails(true);
                                    if ($('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divTogglePhysicalExamDetails .active").length > 0)
                                        hideDetailSection = false;
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #detailHeading ").text($(objectListItem).find('label').text() + " Detail");
                                    if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics") {
                                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsDetails").find("#hfCharacteristicId").val(liId);
                                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsDetails").find("#hfSubCharacteristicId").val('-1');
                                    }
                                    else {
                                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsDetails").find("#hfCharacteristicId").val('-1');
                                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsDetails").find("#hfSubCharacteristicId").val(liId);
                                    }

                                }
                                if (!hideDetailSection) {
                                    Clinical_PhysicalExam.toggleVerticalWidth();
                                    //$(".toggleVertical div.toggle").parent().parent().scrollLeft(1000);
                                }

                                Clinical_PhysicalExam.BindDetailsOfCharAndSubChar(liId, Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase());
                            }




                            Clinical_PhysicalExam.addGreenClasses();

                            if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystems") {

                                for (var index in Clinical_PhysicalExam.DataJSON) {
                                    if (Clinical_PhysicalExam.DataJSON[index].SystemId == liId && Clinical_PhysicalExam.DataJSON[index].IsNormal == true) {
                                        $('#' + Clinical_PhysicalExam.params.PanelID + ' #divPhysicalExamSystemSection #chkNormalSection').prop("checked", true);
                                        Clinical_PhysicalExam.markSectionAsNormal($('#' + Clinical_PhysicalExam.params.PanelID + ' #divPhysicalExamSystemSection #chkNormalSection'), true);
                                        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first').find('#txtNormalSectionDetail').val(Clinical_PhysicalExam.DataJSON[index].Comments);
                                        if (Clinical_PhysicalExam.DataJSON[index].Comments != null && Clinical_PhysicalExam.DataJSON[index].Comments.length > 0) {

                                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first').find('#btnNormalSectionDetails i').removeClass('blue').addClass('green');

                                        }
                                    }
                                }
                            }
                        });
                    }
                });
            }
        }
    },


    //Author : Farooq Ahmad
    //Date : 15/06/2016
    //This Function Will Bind the detail of Char And Subchar
    BindDetailsOfCharAndSubChar: function (pkId, type) {
        if (type == "ulexamcharacteristics") {
            for (var index in Clinical_PhysicalExam.DataJSON) {
                for (var secIndex in Clinical_PhysicalExam.DataJSON[index].Sections) {
                    for (var charIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics) {
                        if (pkId == Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId) {
                            var JSONDetail = Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicDetailModel;
                            if (JSONDetail != null && JSONDetail.CharacteristicId != null) {
                                var detailSection = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsDetails");
                                utility.bindMyJSONByName(true, JSONDetail, false, detailSection);
                            }
                        }
                    }
                }
            }
        }
        else if (type == "ulexamsubcharacteristics") {
            for (var index in Clinical_PhysicalExam.DataJSON) {
                for (var secIndex in Clinical_PhysicalExam.DataJSON[index].Sections) {
                    for (var charIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics) {
                        for (var subcharIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                            if (pkId == Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId) {
                                var JSONDetail = Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicDetailModel;
                                if (JSONDetail != null && JSONDetail.SubCharacteristicId != null) {
                                    var detailSection = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsDetails");
                                    utility.bindMyJSONByName(true, JSONDetail, false, detailSection);
                                }
                            }
                        }

                    }
                }
            }
        }
    },


    addGreenClasses: function () {
        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').find("#ulPhysicalExamSystems li,#ulPhysicalExamSystemSection li,#ulExamCharacteristics li,#ulExamSubCharacteristics li").removeClass('green');

        if (Clinical_PhysicalExam.DataJSON == null) {
            Clinical_PhysicalExam.DataJSON = [];
            return;
        }
        var detailExistsSystems, detailExistsSections, detailExistsCharc, detailExistsSubCharc;
        if (Clinical_PhysicalExam.DataJSON.length > 0) {
            detailExistsSystems = $.grep(Clinical_PhysicalExam.DataJSON, function (SystemElem) {
                return SystemElem.IsNormal || (SystemElem.Sections != null && SystemElem.Sections.length > 0);
            });
            $.each(detailExistsSystems, function (index, SysElem) {
                $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').find("#ulPhysicalExamSystems li#" + SysElem.SystemId).addClass('green');
                if (SysElem.Sections) {
                    detailExistsSections = $.grep(SysElem.Sections, function (SectionElem) {
                        return (SectionElem != null && SectionElem.Characteristics.length > 0);
                    });
                    $.each(detailExistsSections, function (index, SectionElem) {
                        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').find("#ulPhysicalExamSystemSection li#" + SectionElem.SectionId).addClass('green');
                        detailExistsCharc = $.grep(SectionElem.Characteristics, function (CharcElem) {
                            return ((CharcElem != null && (CharcElem.IsPositive || !CharcElem.IsPositive
                               || CharcElem.IsPositive.toString().toLowerCase() == "true"
                               || CharcElem.IsPositive.toString().toLowerCase() == "false")));
                        });
                        $.each(detailExistsCharc, function (index, CharcElem) {
                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').find("#ulExamCharacteristics li#" + CharcElem.SectionCharacteristicId).addClass('green');
                            detailExistsSubCharc = $.grep(CharcElem.SubCharacteristics, function (SubCharcElem) {
                                return ((SubCharcElem != null && (SubCharcElem.IsPositive || !SubCharcElem.IsPositive
                                   || SubCharcElem.IsPositive.toString().toLowerCase() == "true"
                                   || SubCharcElem.IsPositive.toString().toLowerCase() == "false")));
                            });
                            $.each(detailExistsSubCharc, function (index, SubCharcElem) {
                                $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').find("#ulExamSubCharacteristics li#" + SubCharcElem.SubCharacteristicId).addClass('green');
                            });
                            Clinical_PhysicalExam.addgreenSubChar(detailExistsSubCharc, CharcElem.SectionCharacteristicId);
                        });
                        if ($('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + SectionElem.Characteristics[0].SectionCharacteristicId).attr('parentid') == SectionElem.SectionId) {
                            Clinical_PhysicalExam.addgreenChar(detailExistsCharc);
                        }
                    });
                }
            });
        }
    },

    addgreenChar: function (detailExistsCharc) {
        var positiveCount = nagitiveCount = 0;
        for (var charIndex in detailExistsCharc) {
            var CharacteristicId = detailExistsCharc[charIndex].SectionCharacteristicId;
            if ($('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + CharacteristicId).is(':visible')) {
                if (detailExistsCharc[charIndex].IsPositive.toString().toLowerCase() == "true") {
                    positiveCount++;
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + CharacteristicId).find("input[type='checkbox'][id*='+ve']").prop("checked", true);
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + CharacteristicId).find("input[type='checkbox'][id*='-ve']").prop("checked", false);
                }
                else {
                    nagitiveCount++;
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + CharacteristicId).find("input[type='checkbox'][id*='+ve']").prop("checked", false);
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + CharacteristicId).find("input[type='checkbox'][id*='-ve']").prop("checked", true);
                }
                $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').find("#ulExamCharacteristics li#" + CharacteristicId).addClass('green');
            }
        }
        if (positiveCount == $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics').find('input[id*="+ve"].char').length) {
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics').find("input[type=checkbox][id*='+ve']").prop("checked", true);
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics').find("input[type=checkbox][id*='-ve']").prop("checked", false);
        }
        else if (nagitiveCount == $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics').find('input[id*="-ve"].char').length) {
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics').find("input[type=checkbox][id*='-ve']").prop("checked", true);
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics').find("input[type=checkbox][id*='+ve']").prop("checked", false);
        }
    },

    addgreenSubChar: function (detailExistsSubCharc, CharacteristicId) {
        var subPositiveCount = subNagitiveCount = 0;
        for (var subcharIndex in detailExistsSubCharc) {
            var SubCharacteristicId = detailExistsSubCharc[subcharIndex].SubCharacteristicId;
            if ($('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics li#' + SubCharacteristicId).is(':visible')) {
                if (detailExistsSubCharc[subcharIndex].IsPositive.toString().toLowerCase() == "true") {
                    subPositiveCount++;
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics li#' + SubCharacteristicId).find("input[type='checkbox'][id*='+ve']").prop("checked", true);
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics li#' + SubCharacteristicId).find("input[type='checkbox'][id*='-ve']").prop("checked", false);
                }
                else {
                    subNagitiveCount++;
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics li#' + SubCharacteristicId).find("input[type='checkbox'][id*='+ve']").prop("checked", false);
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics li#' + SubCharacteristicId).find("input[type='checkbox'][id*='-ve']").prop("checked", true);
                }

                $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').find("#ulExamSubCharacteristics li#" + SubCharacteristicId).addClass('green');
                if (detailExistsSubCharc[subcharIndex].Comments && detailExistsSubCharc[subcharIndex].Comments.length > 0) {
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics li#' + SubCharacteristicId).find("button[id*='btnOpenDetail']").addClass("green");
                }
            }
        }

        if (subPositiveCount == $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics').find('input[id*="+ve"].char').length) {
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics').find("input[type=checkbox][id*='+ve']").prop("checked", true);
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics').find("input[type=checkbox][id*='-ve']").prop("checked", false);
        }
        else if (subNagitiveCount == $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics').find('input[id*="-ve"].char').length) {
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics').find("input[type=checkbox][id*='-ve']").prop("checked", true);
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics').find("input[type=checkbox][id*='+ve']").prop("checked", false);
        }

    },

    //Author: Farooq Ahmad
    //Date: 09/06/2016
    //This Function will manage the JSON Array
    manageJsonData: function (pkId, type, detail, ISNORMAL, noramalDetail, isPositive) {
        if (Clinical_PhysicalExam.DataJSON == null) {
            Clinical_PhysicalExam.DataJSON = [];
            Clinical_PhysicalExam.getFromAlreadyAdded();
        }
        Clinical_PhysicalExam.DBTemplateId = Clinical_PhysicalExam.SelectedTempId;
        var selectedJSON = null;
        //if (detail != null && $(detail).css("visibility") != "hidden") {
        //    var myJSON = detail != null ? detail.getMyJSONByName() : "{}";
        //    selectedJSON = JSON.parse(myJSON);
        //}
        if (type.toLowerCase() == "system") {
            Clinical_PhysicalExam.updateJSONArraySystem(pkId);
        }
        else if (type.toLowerCase() == "characteristics") {
            isCharacteristicsCtrl = "1";
            Clinical_PhysicalExam.updateJSONArrayChar(pkId, selectedJSON);
        }
        else if (type.toLowerCase() == "subcharacteristics") {
            isSubCharacteristicsCtrl = "1";

            Clinical_PhysicalExam.updateJSONArrayForSubChar(pkId, selectedJSON);

        }
    },

    //Author :Farooq Ahmad
    //Date : 10-06-2016
    updateJSONArrayChar: function (pkId, selectedJSON) {
        var IsSystemExist = false, IsSectionExist = false, IsCharacteristicsExist = false;
        var sectionId = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + pkId).attr('parentid');
        var systemId = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li#' + sectionId).attr('parentid');
        if ($('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + pkId).find('input:checked').length > 0) {
            var IsPositive = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + pkId).find('input:checked').attr('id').indexOf('-ve') > -1 ? false : true;
            for (var index in Clinical_PhysicalExam.DataJSON) {
                if (Clinical_PhysicalExam.DataJSON[index].SystemId == systemId) {
                    IsSystemExist = true;
                    for (var secIndex in Clinical_PhysicalExam.DataJSON[index].Sections) {
                        if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].SectionId == sectionId) {
                            IsSectionExist = true;

                            for (var charIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics) {
                                if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == pkId) {
                                    IsCharacteristicsExist = true;
                                    if (selectedJSON != null)
                                        Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicDetailModel = selectedJSON;
                                    Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].IsPositive = IsPositive
                                }
                            }
                            if (!IsCharacteristicsExist) {
                                if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics == null) {
                                    Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics = [];
                                }
                                Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics.push({ SectionCharacteristicId: pkId, IsPositive: IsPositive, SubCharacteristics: [], SectionCharacteristicDetailModel: selectedJSON });
                            }
                        }
                    }
                    if (!IsSectionExist) {
                        if (Clinical_PhysicalExam.DataJSON[index].Sections == null) {
                            Clinical_PhysicalExam.DataJSON[index].Sections = [];
                        }
                        Clinical_PhysicalExam.DataJSON[index].Sections.push({ SectionId: sectionId, Characteristics: [{ SectionCharacteristicId: pkId, IsPositive: IsPositive, SubCharacteristics: [], SectionCharacteristicDetailModel: selectedJSON }] });
                    }
                }
            }
            if (!IsSystemExist) {
                var obj = {
                    SystemId: systemId,
                    Sections: [{ SectionId: sectionId, Characteristics: [{ SectionCharacteristicId: pkId, IsPositive: IsPositive, SubCharacteristics: [], SectionCharacteristicDetailModel: selectedJSON }] }]
                }
                if (Clinical_PhysicalExam.DataJSON == null) {
                    Clinical_PhysicalExam.DataJSON = [];
                }
                Clinical_PhysicalExam.DataJSON.push(obj);
            }
        }
        else {
            for (var index in Clinical_PhysicalExam.DataJSON) {
                if (Clinical_PhysicalExam.DataJSON[index].SystemId == systemId) {

                    for (var secIndex in Clinical_PhysicalExam.DataJSON[index].Sections) {
                        if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].SectionId == sectionId) {

                            for (var charIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics) {
                                if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == pkId) {
                                    Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics.splice(charIndex, 1);
                                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + pkId).find("#btnOpenDetail" + pkId).removeClass("green");
                                    break;
                                }
                            }

                            try {
                                if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics.length == 0) {
                                    Clinical_PhysicalExam.DataJSON[index].Sections.splice(secIndex, 1);
                                    break;
                                }
                            } catch (ex) {
                                console.log(ex);
                            }
                        }
                    }
                    try {
                        if (Clinical_PhysicalExam.DataJSON[index].Sections.length == 0) {
                            Clinical_PhysicalExam.DataJSON.splice(index, 1);
                            break;
                        }
                    } catch (ex) {
                        console.log(ex);
                    }
                }
            }
        }
    },

    //Author : Farooq Ahmad
    //Date : 10-06-2016
    updateJSONArrayForSubChar: function (pkId, selectedJSON) {
        var IsSystemExist = false, IsSectionExist = false, IsCharacteristicsExist = false, IsSubCharacteristicsExist = false;

        var characteristicId = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics li#' + pkId).attr('parentid');
        var sectionId = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + characteristicId).attr('parentid');
        var systemId = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li#' + sectionId).attr('parentid');

        if ($('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics li#' + pkId).find('input:checked').length > 0) {
            var IsPositive = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics li#' + pkId).find('input:checked').attr('id').indexOf('-ve') > -1 ? false : true;

            for (var index in Clinical_PhysicalExam.DataJSON) {
                if (Clinical_PhysicalExam.DataJSON[index].SystemId == systemId) {

                    for (var secIndex in Clinical_PhysicalExam.DataJSON[index].Sections) {
                        if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].SectionId == sectionId) {

                            for (var charIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics) {
                                if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == characteristicId) {

                                    for (var subcharIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                                        if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId == pkId) {
                                            IsSubCharacteristicsExist = true;
                                            if (selectedJSON != null)
                                                Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicDetailModel = selectedJSON;
                                            Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].IsPositive = IsPositive
                                        }
                                    }

                                    if (!IsSubCharacteristicsExist) {

                                        obj = {
                                            SubCharacteristicId: pkId,
                                            SubCharacteristicDetailModel: selectedJSON,
                                            IsPositive: IsPositive
                                        }
                                        if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics == null) {
                                            Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics = [];
                                        }
                                        Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics.push(obj);

                                    }

                                }
                            }

                        }
                    }

                }
            }

        }
        else {
            for (var index in Clinical_PhysicalExam.DataJSON) {
                if (Clinical_PhysicalExam.DataJSON[index].SystemId == systemId) {

                    for (var secIndex in Clinical_PhysicalExam.DataJSON[index].Sections) {
                        if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].SectionId == sectionId) {

                            for (var charIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics) {
                                if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == characteristicId) {

                                    for (var subcharIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                                        if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId == pkId) {
                                            Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics.splice(subcharIndex, 1);
                                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamSubCharacteristics li#' + pkId).find("#btnOpenDetail" + pkId).removeClass("green");
                                            break;
                                        }
                                    }

                                }
                            }

                        }
                    }

                }
            }
        }
    },

    //Author: Farooq Ahmad
    //Date: 13/06/2016
    //This Function will manage the JSON Array
    updateJSONArraySystem: function (systemId) {
        var isNormal = false, Comments = '';
        if ($('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first').attr('parentid') == systemId) {
            isNormal = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first').find('#chkNormalSection').prop("checked");
            //Comments = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first').find('#txtNormalSectionDetail').text();
        }
        var obj = {
            Id: 0,
            SystemId: systemId,
            IsNormal: isNormal,
            Comments: Comments,
            Sections: []
        };
        var isExist = false;
        for (var index in Clinical_PhysicalExam.DataJSON) {
            if (Clinical_PhysicalExam.DataJSON[index].SystemId == systemId) {
                isExist = true;

                Clinical_PhysicalExam.DataJSON[index].IsNormal = isNormal;
                if (isNormal) {
                    Clinical_PhysicalExam.DataJSON[index].Sections = [];
                }
                if (Clinical_PhysicalExam.DataJSON[index].Sections == null) {
                    Clinical_PhysicalExam.DataJSON[index].Sections = [];
                }
            }
        }

        if (!isExist) {
            Clinical_PhysicalExam.DataJSON.push(obj);
        }
    },

    //Author: Farooq Ahmad
    //Date: 09/06/2016
    //This Function will manage the JSON Array
    getFromAlreadyAdded: function () {
        var loadedData = JSON.parse(Clinical_PhysicalExam.selectedData);

        Clinical_PhysicalExam.DataJSON = [];

        for (var index in loadedData) {
            obj = {
                Id: loadedData[index].Id,
                SystemId: loadedData[index].SystemId,
                Sections: []
            };
            for (var secIndex in loadedData[index].Sections) {
                objSec = {
                    Id: loadedData[index].Sections[secIndex].Id,
                    SectionId: loadedData[index].Sections[secIndex].SectionId,
                    Characteristics: []
                };
                for (var charIndex in loadedData[index].Sections[secIndex].Characteristics) {

                    objchar = {
                        Id: loadedData[index].Sections[secIndex].Characteristics[charIndex].Id,
                        SectionCharacteristicId: loadedData[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId,
                        SectionCharacteristicDetailModel: loadedData[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicDetailModel,
                        IsPositive: loadedData[index].Sections[secIndex].Characteristics[charIndex].IsPositive,
                        SubCharacteristics: []
                    }
                    for (var subCharIndex in loadedData[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {


                        objsubchar = {
                            Id: loadedData[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subCharIndex].Id,
                            SubCharacteristicId: loadedData[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristicId,
                            SubCharacteristicDetailModel: loadedData[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subCharIndex].SubCharacteristicDetailModel,
                            IsSubCharacteristicPositive: loadedData[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subCharIndex].IsSubCharacteristicPositive
                        }
                        objchar.SubCharacteristics.push(objsubchar);
                    }
                    objSec.Characteristics.push(objchar);
                }

                obj.Sections.push(objSec);
            }
            Clinical_PhysicalExam.DataJSON.push(obj);
        }
    },

    //Author : Farooq Ahmad
    //Date : 15/06/2016
    //This Function will manage the detail section of characterisctics and subcharaterisctics
    UpdateJSONDetailSection: function () {
        var detailJSON = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails').getMyJSONByName();
        var detail = JSON.parse(detailJSON);

        if (parseInt(detail.CharacteristicId) > 0 && parseInt(detail.SubCharacteristicId) < 0) {
            Clinical_PhysicalExam.updateJSONArrayChar(detail.CharacteristicId, detail);
        }
        else if (parseInt(detail.SubCharacteristicId) > 0) {
            Clinical_PhysicalExam.updateJSONArrayForSubChar(detail.SubCharacteristicId, detail);
        }

    },

    //Author : Farooq Ahmad
    //Date : 15/06/2016
    //This Function will Delete the System From the JSON
    DeleteSystemFromJSON: function (systemId) {
        for (var index in Clinical_PhysicalExam.DataJSON) {
            if (Clinical_PhysicalExam.DataJSON[index].SystemId == systemId) {
                if (Clinical_PhysicalExam.DataJSON[index].PhysicalExamSystemId != null && parseInt(Clinical_PhysicalExam.DataJSON[index].PhysicalExamSystemId) > 0) {
                    var systemName = $('#' + Clinical_PhysicalExam.params.PanelID + "#ulPhysicalExamSystems li#" + systemId).find('a').text();

                    utility.myConfirm('This will reset all values in ' + systemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed? ', function (systemId) {
                        Clinical_PhysicalExam.DataJSON.splice(index, 1);
                        Clinical_PhysicalExam.addGreenClasses();
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:not(:first)").removeClass("disableAll");
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li#" + systemId).removeClass("green");
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #btnNormalSectionDetails").addClass("hidden");

                        $('#' + Clinical_PhysicalExam.params.PanelID + ' #divPhysicalExamSystemSection #chkNormalSection').prop("checked", false);

                        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first').find('#btnNormalSectionDetails i').addClass('blue').removeClass('green');



                    }, function () { }, 'Confirm Delete');

                } else {
                    Clinical_PhysicalExam.DataJSON.splice(index, 1);
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:not(:first)").removeClass("disableAll");
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li#" + systemId).removeClass("green");
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #btnNormalSectionDetails").addClass("hidden");
                }
                break;
            }
        }
    },


    //Author: Abid Ali
    //Date: 25-01-2016
    //This function check for null and undefined of an object
    isNullOrUndefined: function (object) {
        return object == null && typeof object == 'undefined' ? true : false;
    },

    //Author: Muhammad Arshad
    //Date: 18-01-2016
    //This function will handle select All Chracteristics
    selectAllCharacteristics: function (objcheckAll, IsPostive) {
        if (objcheckAll != null) {

            var pstivePartialId = "+ve";
            var ngtivePartialId = "-ve";
            var isChecked = $(objcheckAll).prop("checked");
            var parentUlCtrl = $(objcheckAll).closest("ul");

            if (IsPostive == true) {

                if (isChecked == true) {
                    $(parentUlCtrl).find(" input[id*='" + ngtivePartialId + "']").each(function () {
                        $(this).prop("checked", !isChecked);
                    });
                }

                $(parentUlCtrl).find(" input[id*='" + pstivePartialId + "']").each(function () {
                    $(this).prop("checked", isChecked);
                });
            }
            else if (IsPostive != true) {

                if (isChecked == true) {
                    $(parentUlCtrl).find(" input[id*='" + pstivePartialId + "']").each(function () {
                        $(this).prop("checked", !isChecked);
                    });
                }

                $(parentUlCtrl).find(" input[id*='" + ngtivePartialId + "']").each(function () {
                    $(this).prop("checked", isChecked);
                });
            }

            Clinical_PhysicalExam.parentCtrlGlobel = $(parentUlCtrl).attr('id');

            $(parentUlCtrl).find('li').each(function () {

                if ($(this).attr("id") != 'undefined') {

                    if (Clinical_PhysicalExam.parentCtrlGlobel == "ulExamCharacteristics") {
                        Clinical_PhysicalExam.updateJSONArrayChar($(this).attr("id"));
                    }
                    else if (Clinical_PhysicalExam.parentCtrlGlobel == "ulExamSubCharacteristics") {
                        Clinical_PhysicalExam.updateJSONArrayForSubChar($(this).attr("id"));
                    }
                }
            });

            if ($(parentUlCtrl).find('input[type=checkbox]:checked').length <= 0) {
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics").addClass('hidden');
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #sectionPhysicalExamDetails").addClass("hidden");
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divTogglePhysicalExamDetails").addClass("hidden");
            }

            Clinical_PhysicalExam.addGreenClasses();
        }
    },

    //Author: Ahmad Raza
    //Date: 25-01-2016
    //This function get number part of an object
    getNumberPart: function (obj) {
        var innernumericPart = 0;
        $.each(obj, function (i, item) {
            if (i.indexOf("SystemId") > -1) {
                innernumericPart = i.replace(/[^\d]+/, '');
                return innernumericPart;
            }
        });
        return innernumericPart;
    },

    //Author: Abid Ali
    //Date: 25-01-2016
    //This function get value of the inputed key of an object
    getValueFromObject: function (obj, key) {
        var value = null;
        $.each(obj, function (i, item) {
            if (i.indexOf(key) > -1) {
                value = item;
            }
        });
        return value;
    },

    //Author: Abid Ali
    //Date: 25-01-2016
    //This function check systems present in Clinical_PhysicalExam.myArr
    checkSystemInPhysicalExamMyArr: function (systemId) {

        var exists = false;
        $.each(Clinical_PhysicalExam.myArr, function (index, item) {
            if (systemId == Clinical_PhysicalExam.getValueFromObject(item, 'SystemId')) {
                exists = true;
                return exists;
            }
        });
        return exists;
    },

    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will check if any characteristic/subcharacteristic has data in details section
    toggleDetailsDiv: function (liId, isReplace) {
        var objDeffered = $.Deferred();
        var sectionDetails = "";
        var self = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
            Clinical_PhysicalExam.resetControlValue($(this));
        });;

        var detail = self.getMyJSONByName();



        //.find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
        ////Replace numeric part of id
        //var currentId = $(this).attr('id');
        //var currentName = $(this).attr('name');
        //var numericPart = $(this).attr('id').replace(/[^\d]+/, '');
        //if (numericPart != null && numericPart != "") {
        //    if (isReplace == true) {
        //        currentId = currentId.replace(numericPart, "");
        //        currentName = currentName.replace(numericPart, "");
        //    }
        //    else {
        //        currentId = currentId.replace(numericPart, liId);
        //        currentName = currentName.replace(numericPart, liId);
        //    }

        //}
        //else {
        //    currentId = currentId + liId;
        //    currentName = currentName + liId;
        //}
        //$(this).attr("id", currentId);
        //$(this).attr("name", currentName);
        //// Resets controls Value
        //    Clinical_PhysicalExam.resetControlValue($(this));

        //});
        objDeffered.resolve();
        return objDeffered;
    },

    //Author: Muhammad Arshad
    //Date: 21-01-2016
    //This function will handle toggling of +ve/-ve checkboxes
    toggleCheckBoxes: function (chkObject) {

        if ($(chkObject).prop('checked')) {
            $(chkObject).parent().parent().find("input[type=checkbox]").prop('checked', false);
            $(chkObject).prop('checked', true);
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam div#divTogglePhysicalExamDetails").removeClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #detailHeading ").text($(chkObject).closest('li').find('label').text() + " Detail");

            if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                Clinical_PhysicalExam.toggleDetailsDiv();
                var CharacteristicId = $(chkObject).closest('li').attr("id");

            }
            else {
                Clinical_PhysicalExam.toggleDetailsDiv();
                var SubCharacteristicId = $(chkObject).closest('li').attr("id");

            }

        }
        else {
            var isbothUnCheck = true;
            $(chkObject).parent().find("input[type=checkbox]").each(function () {
                if ($(this).prop('checked')) {
                    isbothUnCheck = false;
                }
            });
            if (isbothUnCheck) {
                setTimeout(function (chkObject) {
                    $(chkObject).closest('li').removeClass("active");
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics");

                    if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam  #CharacteristicsSubCharacteristics").addClass('hidden');;
                    }
                    Clinical_PhysicalExam.toggleDetailsDiv();
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam div#divTogglePhysicalExamDetails").addClass('hidden');

                }, 100, chkObject);
            }
            else {
                Clinical_PhysicalExam.toggleDetailsDiv();
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam div#divTogglePhysicalExamDetails").removeClass('hidden');
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #detailHeading ").text($(chkObject).closest('li').find('label').text() + " Detail");

                if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                    Clinical_PhysicalExam.toggleDetailsDiv();
                    var CharacteristicId = $(chkObject).closest('li').attr("id");

                }
                else {
                    Clinical_PhysicalExam.toggleDetailsDiv();
                    var SubCharacteristicId = $(chkObject).closest('li').attr("id");

                }
            }
        }



        if ($(chkObject).attr('id').indexOf('+ve') > -1) {
            var selectAll = $(chkObject).closest('ul').find('input[id*="+ve"].char:checked').length == $(chkObject).closest('ul').find('input[id*="+ve"].char').length;
            $(chkObject).closest('ul').find('li:first').find("input[type=checkbox][id*='+ve']").prop("checked", selectAll);

            //unCheck select All negative checkbox
            $(chkObject).closest('ul').find('li:first').find("input[type=checkbox][id*='-ve']").prop("checked", false);
        }
        else {
            var selectAll = $(chkObject).closest('ul').find('input[id*="-ve"].char:checked').length == $(chkObject).closest('ul').find('input[id*="-ve"].char').length;
            $(chkObject).closest('ul').find('li:first').find("input[type=checkbox][id*='-ve']").prop("checked", selectAll);

            //unCheck select All positive checkbox
            $(chkObject).closest('ul').find('li:first').find("input[type=checkbox][id*='+ve']").prop("checked", false);
        }

    },



    //Author: Abid Ali
    //Date: 14-03-2016
    //This function will return selected dataobject by passing partType and partId
    getComponentFromSelectedData: function (partType, partId) {
        var component = null;
        var isComponentFound = false;
        var selectedData = JSON.parse(Clinical_PhysicalExam.selectedData);

        for (var systemIndex = 0; systemIndex < selectedData.length; systemIndex++) {

            if (selectedData[systemIndex].SystemId == partId && partType.toLowerCase() == "system") {
                component = selectedData[systemIndex];
                isComponentFound = true;

            }
            if (partType.toLowerCase() == "section") {
                for (var sectionIndex = 0; sectionIndex < selectedData[systemIndex].Sections.length; sectionIndex++) {

                    if (selectedData[systemIndex].Sections[sectionIndex].SectionId == partId) {
                        component = selectedData[systemIndex].Sections[sectionIndex];
                        isComponentFound = true;
                        break;
                    }
                    if (partType.toLowerCase() == "char") {

                        for (var charIndex = 0; charIndex < selectedData[systemIndex].Sections[sectionIndex].Characteristics.length; charIndex++) {

                            if (selectedData[systemIndex].Sections[sectionIndex].Characteristics[charIndex].SectionCharacteristicId == partId) {
                                component = selectedData[systemIndex].Sections[sectionIndex].Characteristics[charIndex];
                                isComponentFound = true;
                                break;
                            }
                            if (partType.toLowerCase() == "subchar") {
                                for (var subCharIndex = 0; subCharIndex < selectedData[systemIndex].Sections[sectionIndex].Characteristics[charIndex].SubCharacteristics.length; subCharIndex++) {

                                    if (selectedData[systemIndex].Sections[sectionIndex].Characteristics[charIndex].SubCharacteristics[subCharIndex].CharSubCharacteristicId == partId) {
                                        component = selectedData[systemIndex].Sections[sectionIndex].Characteristics[charIndex].SubCharacteristics[subCharIndex];
                                        isComponentFound = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (isComponentFound) {
                break;
            }
        }
        return component;
    },

    //Author: Farooq Ahmad
    //Date:25-02-2016
    //This function will delete from JSon Object
    deleteFromJsonObject: function (type, selectedLiId) {
        var nameOfList = type;

        if (Clinical_PhysicalExam.selectedData != null && Clinical_PhysicalExam.selectedData != "") {
            var objSelectedDate = JSON.parse(Clinical_PhysicalExam.selectedData);


            isMatch = false;

            for (var SysIndex = 0; SysIndex < objSelectedDate.length; SysIndex++) {

                if (nameOfList.toLowerCase() == "ulphysicalexamsystems" && selectedLiId == objSelectedDate[SysIndex].SystemId) {
                    objSelectedDate.splice(SysIndex, 1);
                    break;
                }
                for (var secIndex = 0; secIndex < objSelectedDate[SysIndex].Sections.length; secIndex++) {

                    if (nameOfList.toLowerCase() == "ulPhysicalExamSystemSection" && selectedLiId == objSelectedDate[secIndex].SectionId) {
                        objSelectedDate[SysIndex].Sections.splice(secIndex, 1);
                        break;
                    }
                    for (var CharIndex = 0; CharIndex < objSelectedDate[SysIndex].Sections[secIndex].Characteristics.length; CharIndex++) {
                        if (nameOfList.toLowerCase() == "ulexamcharacteristics" && selectedLiId == objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex].SectionCharacteristicId.toString()) {
                            //delete objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex];
                            objSelectedDate[SysIndex].Sections[secIndex].Characteristics.splice(CharIndex, 1);
                            isMatch = true;
                            break;
                        }
                        else if (nameOfList.toLowerCase() == "ulexamsubcharacteristics") {
                            for (var SubCharIndex = 0; SubCharIndex < objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex].SubCharacteristics.length; SubCharIndex++) {
                                if (selectedLiId == objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex].SubCharacteristics[SubCharIndex].CharSubCharacteristicId.toString()) {
                                    // delete objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex].SubCharacteristics[SubCharIndex];
                                    objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex].SubCharacteristics.splice(SubCharIndex, 1);
                                    isMatch = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (isMatch) break;
                }
                if (isMatch) break;
            }
            Clinical_PhysicalExam.selectedData = JSON.stringify(objSelectedDate);

        }
        if (Clinical_PhysicalExam.myArr != null) {

            var RemoveIndexArray = [];
            for (var count in Clinical_PhysicalExam.myArr) {
                var num = Clinical_PhysicalExam.getNumberPart(Clinical_PhysicalExam.myArr[count]);
                if (num == selectedLiId && nameOfList.toLowerCase() == "ulexamcharacteristics") {
                    count = parseInt(count);
                    RemoveIndexArray.push(count);
                    //Clinical_PhysicalExam.myArr.splice(count, 1);
                }
                else if (Clinical_PhysicalExam.myArr[count]["SystemId" + num] == selectedLiId && nameOfList.toLowerCase() == "ulphysicalexamsystems") {
                    count = parseInt(count);
                    RemoveIndexArray.push(count);
                    //Clinical_PhysicalExam.myArr.splice(count, 1);
                }
            }
            try {
                for (var removeIndex = RemoveIndexArray.length - 1; removeIndex > -1; removeIndex--) {
                    Clinical_PhysicalExam.myArr.splice(removeIndex, 1);
                }
            } catch (ex) {
                console.log(ex);
            }

        }

        if (Clinical_PhysicalExam.ExamDetails[selectedLiId] != null) {
            if (nameOfList.toLowerCase() == "ulexamsubcharacteristics") {

                delete Clinical_PhysicalExam.ExamDetails[selectedLiId];
                Clinical_PhysicalExam.toggleDetailsDiv('', true);
                Clinical_PhysicalExam.isBothUnCheck = true;
            }
            else if (nameOfList.toLowerCase() == "ulexamcharacteristics") {
                var DeletingIndexes = [];
                for (var count in Clinical_PhysicalExam.ExamDetails) {
                    var jsonExamDetail = JSON.parse(Clinical_PhysicalExam.ExamDetails[count]);

                    var num = Clinical_PhysicalExam.getNumberPart(jsonExamDetail);
                    if (jsonExamDetail["CharacteristicId" + num] == selectedLiId) {
                        DeletingIndexes.push(count);

                    }
                }

                for (var count in DeletingIndexes) {
                    delete Clinical_PhysicalExam.ExamDetails[DeletingIndexes[count]];
                }
            }
            else if (nameOfList.toLowerCase() == "ulphysicalexamsystems") {
                var DeletingIndexes = [];
                for (var count in Clinical_PhysicalExam.ExamDetails) {
                    var jsonExamDetail = JSON.parse(Clinical_PhysicalExam.ExamDetails[count]);

                    var num = Clinical_PhysicalExam.getNumberPart(jsonExamDetail);
                    if (jsonExamDetail["SystemId" + num] == selectedLiId) {
                        DeletingIndexes.push(count);

                    }
                }

                for (var count in DeletingIndexes) {
                    delete Clinical_PhysicalExam.ExamDetails[DeletingIndexes[count]];
                }
            }
        }

        if (nameOfList.toLowerCase() == "ulphysicalexamsystems") {
            var index = 0;
            var RemoveDetailExamIndexes = [];
            for (var count in Clinical_PhysicalExam.ExamDetails) {
                var jsonExamDetail = JSON.parse(Clinical_PhysicalExam.ExamDetails[count]);
                var num = Clinical_PhysicalExam.getNumberPart(jsonExamDetail);
                if (jsonExamDetail["SystemId" + num] == selectedLiId && nameOfList.toLowerCase() == "ulphysicalexamsystems") {
                    RemoveDetailExamIndexes.push(num);
                    //delete Clinical_PhysicalExam.ExamDetails[num];
                    //break;
                }
                index++;
            }
            for (var index = RemoveDetailExamIndexes.length - 1; index > -1; index--) {
                delete Clinical_PhysicalExam.ExamDetails[RemoveDetailExamIndexes[index]];
            }
            Clinical_PhysicalExam.toggleDetailsDiv('', true);
            Clinical_PhysicalExam.isBothUnCheck = true;
            // $('#' + Clinical_PhysicalExam.params.PanelID + " #ulPhysicalExamSystemSection #chkNormalSection").removeAttr("checked");
            $('#' + Clinical_PhysicalExam.params.PanelID + " #ulphysicalexamsystems li#" + selectedLiId).trigger('click');
            Clinical_PhysicalExam.setHiddenFieldValues('ulphysicalexamsystems', selectedLiId);

        }
    },

    //Author: Farooq Ahmad
    //Date:22-02-2016
    //This function will set the values in hidden field
    setHiddenFieldValues: function (currentUlId, currentId, parentObj) {
        var systemId = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystems li.active').attr("id");
        var sectionId = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li.active').attr("id");
        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails input[id*="hfSystemId"]').val(systemId);
        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails input[id*="hfSectionId"]').val(sectionId);
        var characteristicId = "";
        var isCharacteristicPostive = false;
        var isSubCharacteristicPostive = false;
        var subCharacteristicId = "";
        if (currentUlId.toLowerCase() == "ulexamcharacteristics") {
            if (currentId != null && currentId.indexOf("+ve") > -1) {
                isCharacteristicPostive = true;
            }
            characteristicId = $(parentObj).parent().attr("id");
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails input[id*="hfCharacteristicId"]').val(characteristicId);
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails input[id*="hfIsCharacteristicPositive"]').val(isCharacteristicPostive);


        }
        else if (currentUlId.toLowerCase() == "ulexamsubcharacteristics") {
            if (currentId != null && currentId.indexOf("+ve") > -1) {
                isSubCharacteristicPostive = true;
            }
            characteristicId = $(parentObj).parent().attr("parentid");
            subCharacteristicId = $(parentObj).parent().attr("id");

            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails input[id*="hfCharacteristicId"]').val(characteristicId);

            var chkOfCharacteristics = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulExamCharacteristics li#' + characteristicId + '  input[type=checkbox]:checked').attr("id");
            if (chkOfCharacteristics != null && chkOfCharacteristics.indexOf("+ve") > -1) {
                isCharacteristicPostive = true;
            }
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails input[id*="hfSubCharacteristicId"]').val(subCharacteristicId);
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails input[id*="hfIsCharacteristicPositive"]').val(isCharacteristicPostive);
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails input[id*="hfIsSubCharacteristicPositive"]').val(isSubCharacteristicPostive);
        }
    },

    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will handle show/Hide of Details section
    showHideToggleDetails: function (isToShow) {
        var toggleDetailsDiv = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam div#divTogglePhysicalExamDetails");
        if (isToShow == false) {
            if (toggleDetailsDiv.hasClass("hidden") == false) {
                toggleDetailsDiv.addClass("hidden");
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails").addClass('hidden');
            }
        }
        else {

            toggleDetailsDiv.removeClass("hidden");
        }

    },

    //Author: Muhammad Arshad
    //Date: 21-01-2016
    //This function will handle show/Hide of section/Characteristics/Sub-Characteristics normal exam
    showHideSectionCharacteristics: function (isFromSystems) {
        if (isFromSystems == true) {
            var sectionDiv = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SystemSections");

            //Start 09-02-2016 Humaira Yousaf to reset selected checkboxes
            var characteristicDiv = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SectionCharacteristics");
            var subCharacteristicDiv = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics");

            $(characteristicDiv).find("#ulExamCharacteristics > li").find('input:checkbox').prop('checked', false);
            $(subCharacteristicDiv).find("#ulExamSubCharacteristics> li").find('input:checkbox').prop('checked', false);
            //End 09-02-2016 Humaira Yousaf to reset selected checkboxes

            // sectionDiv.find("ul#ulPhysicalExamSystemSection li input[id='chkNormalSection']").trigger("click");
            if (sectionDiv.hasClass("hidden") == false) {
                sectionDiv.find("ul#ulPhysicalExamSystemSection li").not(":first").each(function (i, item) {
                    $(item).removeClass("active");
                    $(item).addClass("disableAll");
                });
                sectionDiv.addClass("hidden");
            }
        }
        var characteristicDiv = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SectionCharacteristics");
        var subCharacteristicDiv = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics");
        if (characteristicDiv.hasClass("hidden") == false) {
            characteristicDiv.addClass("hidden");
        }

        if (subCharacteristicDiv.hasClass("hidden") == false) {
            subCharacteristicDiv.addClass("hidden");
        }

        Clinical_PhysicalExam.showHideToggleDetails(true);

    },


    //Author Farooq Ahmad
    //Date : 15-06-2016
    AllSystemNormarl: function (chk, source) {
        if ($(chk).prop("checked")) {
            if (source == true) {
                utility.myConfirm('This will mark all Systems as Normal and reset all values in Systems. Would you like to proceed?', function () {
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li").each(function () {
                        $(this).addClass("green");
                        $(this).addClass("disableAll");
                    });
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam").find("#SystemSections,#SectionCharacteristics,#CharacteristicsSubCharacteristics").addClass("hidden");
                    Clinical_PhysicalExam.DataJSON = [];
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divNormalExams").removeClass("hidden");
                }, function () {
                    $(chk).prop("checked", false);
                }, 'Confirm Mark Normal');
            }
            else {
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li").each(function () {
                    $(this).addClass("green");
                });
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam").find("#SystemSections,#SectionCharacteristics,#CharacteristicsSubCharacteristics,#sectionPhysicalExamDetails,#divTogglePhysicalExamDetails").addClass("hidden");
                Clinical_PhysicalExam.DataJSON = [];
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divNormalExams").removeClass("hidden");
            }


        }
        else {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li").each(function () {
                $(this).removeClass("green");
                $(this).removeClass("disableAll");
            });

            Clinical_PhysicalExam.DataJSON = [];
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divNormalExams").addClass("hidden");

            $('#' + Clinical_PhysicalExam.params.PanelID + " #txtNormalExamsDetail").val("");
            Clinical_PhysicalExam.unCheckMainNormalCheckbox();

        }
    },

    //Author: Muhammad Arshad
    //Date: 25-01-2016
    //This function will handle mark systems as Normal and hide Section/Characteristics
    markSectionAsNormal: function (obj, isForSection, isFormLoad, physExamNormalComments) {


        var parentid = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first").attr("parentid");
        if ($(obj).prop("checked")) {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:not(:first)").addClass("disableAll");
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li#" + parentid).addClass("green");
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #btnNormalSectionDetails").removeClass("hidden");
        }
        else {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:not(:first)").removeClass("disableAll");
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li#" + parentid).removeClass("green");
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #btnNormalSectionDetails").addClass("hidden");
            //Start 05-09-2016 Abid Ali For bug#EMR-31
            Clinical_PhysicalExam.updateJSONArraySystem(parentid);
            //End 05-09-2016 Abid Ali For bug#EMR-31
        }

        var $parentNormalCheckBox = $('#' + Clinical_PhysicalExam.params.PanelID + " #chkPhysicalExamsNormal");
        var systemsLength = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li").length;
        var normalSystemsLength = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li.green").length;

        if ($parentNormalCheckBox.prop('checked') && normalSystemsLength != systemsLength) {
            $parentNormalCheckBox.prop('checked', false);
        }

        var isExist = false;
        for (var index in Clinical_PhysicalExam.DataJSON) {
            if (Clinical_PhysicalExam.DataJSON[index].SystemId == parentid) {
                isExist = true;
                if (Clinical_PhysicalExam.DataJSON[index].Sections != null && Clinical_PhysicalExam.DataJSON[index].Sections.length > 0) {

                    var systemName = $("#ulPhysicalExamSystems li#" + parentid).find('a').text();
                    var msg = 'This will mark the entire ' + systemName + ' as Normal and reset all values in all sections of ' + systemName + '. Would you like to proceed?';
                    utility.myConfirm(msg, function () {
                        Clinical_PhysicalExam.updateJSONArraySystem(parentid);
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #SectionCharacteristics").addClass("hidden");
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsSubCharacteristics").addClass("hidden");
                        Clinical_PhysicalExam.showHideToggleDetails(false);
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam div#divTogglePhysicalExamDetails").addClass('hidden');
                        Clinical_PhysicalExam.addGreenClasses();
                    }, function () {
                        $(obj).prop("checked", false);
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:not(:first)").removeClass("disableAll");
                    }, 'Confirm Reset');
                }

            }
        }
        if (!isExist) {
            Clinical_PhysicalExam.updateJSONArraySystem(parentid);
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #SectionCharacteristics").addClass("hidden");
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsSubCharacteristics").addClass("hidden");
            Clinical_PhysicalExam.showHideToggleDetails(false);
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam div#divTogglePhysicalExamDetails").addClass('hidden');
        }
        Clinical_PhysicalExam.toggleVerticalWidth();


    },

    //Remove From Json Object
    deleteListItemsFromJson: function (ul, liIds) {

        $.each(liIds, function (index, id) {

            Clinical_PhysicalExam.deleteFromJsonObject(ul, id);

        });
    },

    //Author: Abid Ali
    //Date: 26-02-2016
    //This function will delete Data objects from DOM using list (ids) passed and resets the item color to green
    deleteDomObjectFromList: function (listType, ListIds) {
        if (listType != null && typeof listType != 'undefined') {
            var LiIds = [];
            if (jQuery.type(ListIds) === "string") {
                LiIds.push(ListIds);
            }
            else {
                LiIds = ListIds;
            }

            var divPhysicaExam = '#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam ";
            if (listType.toLowerCase() == '#ulPhysicalExamSystems'.toLowerCase()) {

                $.each(LiIds, function (index, item) {
                    $(divPhysicaExam + listType + " li#" + item).removeData("SystemSectionIds_" + item);//.removeClass('green active');
                });
            }
            else if (listType.toLowerCase() == '#ulPhysicalExamSystemSection'.toLowerCase()) {

                $.each(LiIds, function (index, item) {
                    $(divPhysicaExam + listType + " li#" + item).removeData("SystemCharacteristicsIds_" + item);
                    $(divPhysicaExam + listType + " li#" + item).removeData('SystemCharacteristicPk_' + item);
                });
            }
            else if (listType.toLowerCase() == '#ulExamCharacteristics'.toLowerCase()) {

                $.each(LiIds, function (index, item) {
                    $(divPhysicaExam + listType + " li#" + item).removeData("SystemSubCharacteristicsIds_" + item)
                    $(divPhysicaExam + listType + " li#" + item).removeData('SystemCharacteristicDetails_' + item)//.removeClass('green active');
                });
            }
            else if (listType.toLowerCase() == '#ulExamSubCharacteristics'.toLowerCase()) {

                $.each(LiIds, function (index, item) {
                    $(divPhysicaExam + listType + " li#" + item).removeData("SystemSubCharacteristicDetails_" + item).removeData('SystemSubCharacteristicPk_' + item)//.removeClass('green active');
                });
            }
        }
    },

    //Author: Muhammad Arshad
    //Date: 21-01-2016
    //This function will handle load of child of either Systems/Section/Characteristics
    loadChildData: function (parentId, parentType) {
        var objDeffered = $.Deferred();
        Clinical_PhysicalExam.loadPhysicalExamStatuses(parentId, parentType).done(function () {

            objDeffered.resolve();

        });
        return objDeffered;
    },

    //Author: Farooq Ahmad
    //Date: 07-03-2016
    //This function will handle normal comments of the sections
    saveNormalSectionDetail: function (event, val, txt) {

        var Comments = $(txt).val();
        var SystemId = $(txt).closest("ul").attr("parentId");
        if (SystemId == null) {
            SystemId = $(txt).closest("ul").find('li:first').attr("parentId");
        }
        Clinical_PhysicalExam.updateJSONArraySystem(SystemId);

        for (var index in Clinical_PhysicalExam.DataJSON) {
            if (Clinical_PhysicalExam.DataJSON[index].SystemId == SystemId) {
                Clinical_PhysicalExam.DataJSON[index].Comments = Comments;
            }
        }
        if (event.which == 13) {
            event.preventDefault();

            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection #textAreaNormal").addClass("hidden");
            if (Comments != '') {
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first i.fa-book").removeClass("blue");
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first i.fa-book").addClass("green");
            } else {
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first i.fa-book").addClass("blue");
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first i.fa-book").removeClass("green");
            }

            var onClickFunction = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam   #btnSaveNormalSystemDetail" + val).attr("onclick")
            eval(onClickFunction);

            //  Clinical_PhysicalExam.openNormalSectionDetail("toggle");
        }

    },

    //Author: Muhammad Arshad
    //Date: 12-09-2015
    //This function will handle fill of PhysicalExam Statuses like SmokingStatus,AlcoholStatus,DrugAbuseStatus,SexualHxStatus
    loadPhysicalExamStatuses: function (parentId, parentType, templateId) {
        var templateId = $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val();

        var data = "";
        var methodName = "";

        if (parentType != null && parentType.toLowerCase() == "mainpesystem") {
            methodName = "GetPhysicalExamSystem";
            Clinical_PhysicalExam.DataJSON = [];
        }
        else if (parentType != null && parentType.toLowerCase() == "system") {
            methodName = "GetPhysicalExamSectionBySystemId";
            data = "ID=" + parentId + "&ID2=" + templateId;
        }
        else if (parentType != null && parentType.toLowerCase() == "section") {
            methodName = "GetPhysicalExamCharcteristicBySectionId";
            data = "ID=" + parentId + "&ID2=" + templateId;
        }
        else if (parentType != null && parentType.toLowerCase() == "characteristics") {
            //Start 11-02-2016 Humaira Yousaf to get subCharacteristics
            methodName = "GetPhysicalExamSubCharcteristicByCharacteristicId";
            data = "ID=" + parentId + "&ID2=" + templateId;
            //End 11-02-2016 Humaira Yousaf to get subCharacteristics
        }
        else if (parentType != null && parentType.toLowerCase() == "subcharacteristics") {
            methodName = "GetSocialHxCounsellingPeriod";
        }
        else {
            data = "ID=" + parentId;
        }
        if (parentId > 0) {
            return MDVisionService.lookups(methodName, true, data).done(function (result) {
                result = JSON.parse(result[methodName]);
                Clinical_PhysicalExam.loadChildDataHTML(result, parentType, parentId);
            });
        } else {
            var dfd = $.Deferred();
            Clinical_PhysicalExam.loadChildDataHTML(JSON.parse('[]'), parentType, parentId);
            dfd.resolve();
            return dfd.promise();
        }

    },
    loadChildDataHTML: function (result, parentType, parentId) {
        var templateId = $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        var currentLiClass = "";
        var currentLiClick = "";
        var currentCtrlId = "";
        var ParentDiv = "";
        var data = "";

        var selectedData = Clinical_PhysicalExam.getObjectOfClickedElement(parentType, parentId);

        if (parentType != null && parentType.toLowerCase() == "mainpesystem") {
            Crtl = '#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems";
            currentLiClick = "Clinical_PhysicalExam.showHideChildControls";
            ParentDiv = "divPhysicalExamSystems";
            currentCtrlId = "ulPhysicalExamSystems";
            Clinical_PhysicalExam.DataJSON = [];

        }
        else if (parentType != null && parentType.toLowerCase() == "system") {
            Crtl = '#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection";
            currentLiClick = "Clinical_PhysicalExam.showHideChildControls";
            ParentDiv = "divPhysicalExamSystemSection";
            currentCtrlId = "ulPhysicalExamSystemSection";
        }
        else if (parentType != null && parentType.toLowerCase() == "section") {
            Crtl = '#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics";
            currentLiClick = "Clinical_PhysicalExam.showHideChildControls";
            ParentDiv = "divExamCharacteristics";
            currentCtrlId = "ulExamCharacteristics";
        }
        else if (parentType != null && parentType.toLowerCase() == "characteristics") {
            Crtl = '#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulExamSubCharacteristics";
            currentLiClick = "Clinical_PhysicalExam.showHideChildControls";
            ParentDiv = "divExamSubCharacteristics";
            //Start 11-02-2016 Humaira Yousaf to get subCharacteristics
            currentCtrlId = "ulExamSubCharacteristics";
            //End 11-02-2016 Humaira Yousaf to get subCharacteristics
        }
        else if (parentType != null && parentType.toLowerCase() == "subcharacteristics") {
            //  Crtl = '#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #CharacteristicsDetails";
            currentLiClick = "Clinical_PhysicalExam.showHideChildControls";
            ParentDiv = "amSubCharacteristics";
            currentCtrlId = "";
        }

        if ($(Crtl).length > 0)
            l = $(Crtl);
        if (parentType != null && parentType.toLowerCase() == "subcharacteristics") {
            return;
        }

        l.empty();
        var isFirstLi = true;
        var onClick = "";

        if (parentType.toLowerCase() == "section" || parentType.toLowerCase() == "characteristics" || parentType.toLowerCase() == "subcharacteristics") {
            //Start 11-02-2016 Humaira Yousaf to show select all checkboxes only if there is data
            if (result.length > 0) {
                //End 11-02-2016 Humaira Yousaf to show select all checkboxes only if there is data
                var liInnerText = "";
                liInnerText = '<div><input type="checkbox" id="chkSelectAll+ve" name="SelectAll+ve" class="ml-xlg pull-left" onclick="Clinical_PhysicalExam.selectAllCharacteristics(this,true);"><input type="checkbox" id="chkSelectAll-ve" name="SelectAll-ve" class="ml-sm pull-left" onclick="Clinical_PhysicalExam.selectAllCharacteristics(this,false);"><label class="control-label pull-left pl-xs">Select All</label><div class="clearfix"></div></div>';
                l.append('<li templateId="' + templateId + '" id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value=' + item.Value + ' refValue="' + item.RefValue + '">' + liInnerText + ' </li>');
            }
        }
        else if (parentType.toLowerCase() == "system") {
            var normalDetail = '<a id="btnNormalSectionDetails" onclick="Clinical_PhysicalExam.openNormalSectionDetail(this);" class="btnEffect pull-left ml-md hidden"><i class="fa fa-book blue"></i></a><div id="textAreaNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" id="txtNormalSectionDetail" spellcheck="true" class="form-control pl-sm pr-xlg height-max105 size100per textAreaScroll" onkeyup="Clinical_PhysicalExam.saveNormalSectionDetail(event,' + item.Value + ',this)"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveNormalSystemDetail' + item.Value + '" onclick="Clinical_PhysicalExam.saveExamSystemNormalDetail(this,\'textNormalSectionDetailNormal' + '\',\'' + parentId + '\');" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div></div>';

            liInnerText = '<div><input type="checkbox" id="chkNormalSection" name="NormalSection" class="pull-left" onclick="Clinical_PhysicalExam.markSectionAsNormal(this,true);"><label class="control-label pull-left pl-xs">Normal</label>' + normalDetail + '<div class="clearfix"></div></div>';
            //Start 09-02-2016 Humaira Yousaf for section normal details
            l.append('<li templateId="' + templateId + '" id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value=' + item.Value + ' refValue="' + item.RefValue + '">' + liInnerText + ' </li>');
            //End 09-02-2016 Humaira Yousaf for section normal details
        }

        $.each(result, function (j, item) {
            if (item.Value != "") {
                if (isFirstLi == true) {
                    //currentLiClass = 'class="active"';
                    isFirstLi = false;
                }
                else {
                    currentLiClass = "";
                }
                var physicalExamId = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val();
                var onClick = currentLiClick + "('" + currentCtrlId + "','" + String(item.Value) + "');";
                //Start//18-02-2016//Ahmad Raza//Delete system,section,Characteristics,SubCharacteristics detail
                var deleteClick = "";
                if (parentType.toLowerCase() == "system") {
                    // deleteClick = "Clinical_PhysicalExam.deleteSectionDetail('" + parentType + "'," + parentId + "," + physicalExamId + "," + item.Value + ")";
                }
                else if (parentType.toLowerCase() == "section") {
                    //  deleteClick = "Clinical_PhysicalExam.deleteCharacteristicDetail('" + parentType + "'," + parentId + "," + physicalExamId + "," + item.Value + ")";
                }
                else if (parentType.toLowerCase() == "characteristics") {
                    deleteClick = "Clinical_PhysicalExam.deleteSubCharacteristicDetail('" + parentType + "'," + parentId + "," + physicalExamId + "," + item.Value + ")";
                }
                //End//18-02-2016//Ahmad Raza//Delete system,section,Characteristics,SubCharacteristics detail
                //item.Value = item.Value == "" ? 0 : item.Value;
                var liInnerText = '<a href="#' + ParentDiv + '">' + item.Name + '</a>';
                if (parentType.toLowerCase() == "section" || parentType.toLowerCase() == "characteristics") {
                    var isSubCharacteristic = false;
                    if (parentType.toLowerCase() == "characteristics") {
                        isSubCharacteristic = true;
                    }

                    //Start 12-02-2016 Humaira Yousaf to open characteristic detail
                    if (item.RefName != "") {
                        liInnerText = '<div><button id="btnOpenDetail' + item.Value + '"  data-toggle="tooltip" data-placement="top"  onclick="Clinical_PhysicalExam.openCharacteristicDetail(this,\'' + item.Value + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-book"></i></button><div class="checkbox-icon checkbox-positive"><input type="checkbox" id="chk' + item.Value + '+ve" name="' + item.Value + '" class="ml-sm pull-left  char" onclick="Clinical_PhysicalExam.toggleCheckBoxes(this);"></div><div class="checkbox-icon checkbox-negative"><input type="checkbox" id="chk' + item.Value + '-ve" name="' + item.Value + '" class="ml-sm pull-left char" onclick="Clinical_PhysicalExam.toggleCheckBoxes(this);"></div><label class="control-label pull-left ml-xs size65per ellipses" data-toggle="tooltip"  title="" data-original-title="' + item.Name + '">' + item.Name + '</label><span id="btnShowSubCharacteristics' + item.Value + '" onclick="" class="pull-right" disabled="disabled"><i class="fa fa-caret-right blue"></i></span><div class="clearfix"></div><div id="divDetail' + item.Value + '" class="rightInnerAddon pb-xs hidden"><textarea rows="1" spellcheck="true" maxlength="5000" id="txtCharacteristicDetail' + item.Value + '" name="CharacteristicDetail' + item.Value + '" type="text" class="form-control pr-xs pr-xlg height-max105 size100per textAreaScroll commentbox" onblur="Clinical_PhysicalExam.validateMaxLength(this)"  onkeyup="Clinical_PhysicalExam.saveDetailComments(event,this)" onkeydown="Clinical_PhysicalExam.saveComments(event,this,\'txtCharacteristicDetail' + item.Value + '\',\'' + item.Value + '\',\'' + isSubCharacteristic + '\');">></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.Value + '" onclick="Clinical_PhysicalExam.saveCharacteristicDetail(this,\'txtCharacteristicDetail' + item.Value + '\',\'' + item.Value + '\',\'' + isSubCharacteristic + '\');" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div></div>';
                    }
                    else {
                        liInnerText = '<div><button id="btnOpenDetail' + item.Value + '"  data-toggle="tooltip" data-placement="top"  onclick="Clinical_PhysicalExam.openCharacteristicDetail(this,\'' + item.Value + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-book"></i></button><div class="checkbox-icon checkbox-positive"><input type="checkbox" id="chk' + item.Value + '+ve" name="' + item.Value + '" class="ml-sm pull-left  char" onclick="Clinical_PhysicalExam.toggleCheckBoxes(this);"></div><div class="checkbox-icon checkbox-negative"><input type="checkbox" id="chk' + item.Value + '-ve" name="' + item.Value + '" class="ml-sm pull-left char" onclick="Clinical_PhysicalExam.toggleCheckBoxes(this);"></div><label class="control-label pull-left ml-xs size65per ellipses" data-toggle="tooltip"  title="" data-original-title="' + item.Name + '">' + item.Name + '</label><div class="clearfix"></div><div id="divDetail' + item.Value + '" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="5000" spellcheck="true" id="txtCharacteristicDetail' + item.Value + '" name="CharacteristicDetail' + item.Value + '" type="text" class="form-control pr-xs pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="Clinical_PhysicalExam.validateMaxLength(this)"  onkeyup="Clinical_PhysicalExam.saveDetailComments(event,this)"  onkeydown="Clinical_PhysicalExam.saveComments(event,this,\'txtCharacteristicDetail' + item.Value + '\',\'' + item.Value + '\',\'' + isSubCharacteristic + '\');"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.Value + '" onclick="Clinical_PhysicalExam.saveCharacteristicDetail(this,\'txtCharacteristicDetail' + item.Value + '\',\'' + item.Value + '\',\'' + isSubCharacteristic + '\');" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div></div>';
                    }
                    //End 12-02-2016 Humaira Yousaf  to open characteristic detail
                }
                l.append('<li templateId="' + templateId + '" id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value="' + item.Value + '" refValue="' + item.RefValue + '" subCharacteristicExist="' + item.RefName + ' ">' + liInnerText + '</li>');
            }

        });

        //Start//25-02-2016//Ahmad Raza//Setting ToolTip for Characteristics and subCharacteristics .
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        //End//25-02-2016//Ahmad Raza//Setting ToolTip for Characteristics and subCharacteristics .

        //Start 09-02-2016 Humaira Yousaf for textarea
        $('.textAreaScroll').slimScroll({
            position: 'right',
            height: '100%',
        });
        //End 09-02-2016 Humaira Yousaf for textarea

    },
    saveComments: function (event, obj, ctrlDetail, parentLiId, isSubCharacteristic) {
        if (event.which == 13) {
            event.preventDefault();
            Clinical_PhysicalExam.saveCharacteristicDetail(obj, ctrlDetail, parentLiId, isSubCharacteristic);
        }
    },

    //Author : Ahmad Raza
    //Date :   09/03/2016
    //Reason:  validating maxlength on blur
    validateMaxLength: function (obj) {
        var maxLength = 5000;
        if (obj.value.length > maxLength) {

            obj.value = obj.value.substring(0, 5000);
        }
        if (obj.value.length > 0) {
            $(obj).closest("li").find('button[id*="btnOpenDetail"]').addClass("green");
        }
        else {
            $(obj).closest("li").find('button[id*="btnOpenDetail"]').removeClass("green");
        }
        $(obj).closest("li").find('button[id*="divDetail"]').addClass("hidden");

    },

    //Author :Farooq Ahmad
    //Date : 04/03/2016
    //This will save the comments of the characteristiscs and sub characteristices
    saveDetailComments: function (event, txt) {
        if (event.which == 13) {
            event.preventDefault();
            $(txt).closest("li").find('button[id*="divDetail"]').addClass("hidden");

        }
        else {
            var comments = $(txt).val();
            var pkid = $(txt).closest('li').attr('id');
            var type = $(txt).closest('ul').attr('id');
            Clinical_PhysicalExam.updateTheCommentsOfCharAndSubChar(comments, pkid, type);
        }
    },


    //Author: Farooq Ahmad
    ///Date : 14/06/2016
    //This function will update The Json With Comments
    updateTheCommentsOfCharAndSubChar: function (comments, pkid, type) {
        if (type.toLowerCase() == "ulexamsubcharacteristics") {

            for (var index in Clinical_PhysicalExam.DataJSON) {

                for (var secIndex in Clinical_PhysicalExam.DataJSON[index].Sections) {

                    for (var charIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics) {

                        for (var subcharIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                            if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId == pkid) {

                                Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].Comments = comments
                            }
                        }
                    }
                }
            }
        }
        else if (type.toLowerCase() == "ulexamcharacteristics") {

            for (var index in Clinical_PhysicalExam.DataJSON) {

                for (var secIndex in Clinical_PhysicalExam.DataJSON[index].Sections) {

                    for (var charIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics) {

                        if (Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == pkid) {

                            Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics[charIndex].Comments = comments
                        }
                    }
                }
            }
        }
    },

    //Author :  Abid Ali
    //Date : 19-2-2016
    //Description : Load data from database and mark the selected li's as green
    markActiveLis: function (result, selectedData, parentType, l) {


        $.each(result, function (j, item) {
            if (item.Value != "") {

                if (parentType != null && parentType.toLowerCase() == "system") {

                    if (selectedData != null) {
                        var firstLi = l.find('li').first();
                        if (selectedData.IsNormal) {

                            if (selectedData.Comments != "") {
                                //green the book icon and pleace comments in textarea field
                                firstLi.find('a#btnNormalSectionDetails > i').removeClass('blue').addClass('green');
                                firstLi.find('textarea').text(selectedData.Comments);
                            }
                            //mark disable each section
                            l.find('li#' + item.Value).addClass('disableAll');
                        }
                        else {

                            if (typeof selectedData.Sections != 'undefined' && selectedData.Sections.length > 0) {

                                var sections = selectedData.Sections;
                                $.each(sections, function (i, SectionComponent) {
                                    if (SectionComponent.SectionId == item.Value) {
                                        var currentSectionLi = l.find('li#' + item.Value);
                                        currentSectionLi.data('SystemCharacteristicsIds_' + item.Value, SectionComponent);
                                        currentSectionLi.data('SystemCharacteristicPk_' + item.Value, SectionComponent.Id);
                                        if (currentSectionLi.hasClass("green") == false) {
                                            currentSectionLi.addClass("green");
                                        }
                                        if (currentSectionLi.hasClass("active") == false) {
                                            currentSectionLi.addClass("active");
                                        }
                                        return;
                                    }
                                });
                            }
                            else {
                                firstLi.find('input[type="checkbox"]').prop('checked', false);
                            }
                        }
                    }
                }
                    // color and save data in characteristics lis
                else if (parentType != null && parentType.toLowerCase() == "section") {
                    var isCharactristicsExists = false;
                    if (selectedData != null) {
                        var characteristics = selectedData.Characteristics;
                        $.each(characteristics, function (i, charactristicsComponent) {
                            if (charactristicsComponent.SectionCharacteristicId == item.Value) {

                                isCharactristicsExists = true;
                                // Clinical_PhysicalExam.showHideToggleDetails(true);
                                l.find('li#' + item.Value).data('SystemSubCharacteristicsIds_' + item.Value, charactristicsComponent).addClass("green");
                                if (charactristicsComponent.IsDetailExists)
                                    l.find('li#' + item.Value).data('SystemCharacteristicDetails_' + item.Value, charactristicsComponent.SectionCharacteristicDetailModel);

                                if (charactristicsComponent.IsPositive) {
                                    var firstLi = l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '+ve"]');

                                    if (l.find('li#' + item.Value).attr('subcharacteristicexist').trim() != "") {

                                        firstLi.trigger('click');

                                    } else {
                                        firstLi.prop('checked', true);
                                    }

                                }
                                else {

                                    if (l.find('li#' + item.Value).attr('subcharacteristicexist').trim() != "") {
                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '-ve"]').trigger('click');
                                    } else {
                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '-ve"]').prop('checked', true);
                                    }
                                }
                                if (charactristicsComponent.Comments != "") {
                                    l.find('li#' + item.Value).find('button').first().addClass('green');
                                    l.find('li#' + item.Value).find('button').attr("data-original-title", charactristicsComponent.Comments);
                                    l.find('li#' + item.Value).find('button').attr("title", charactristicsComponent.Comments);
                                    l.find('li#' + item.Value).find('textarea').text(charactristicsComponent.Comments);
                                }
                                else {
                                    l.find('li#' + item.Value).find('button').first().removeClass('green');
                                    l.find('li#' + item.Value).find('textarea').text('');
                                }
                                // Registers on click with every element in li
                                // if (charactristicsComponent.IsDetailExists) {

                                l.find('li#' + item.Value).on('click', function () {
                                    var detailSection = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails");
                                    var dataDetail = {};
                                    liId = $(this).attr('id');
                                    var textForHeading = $(this).find('label').text() + ' Details';
                                    var currentObjectData = $(this).data('SystemCharacteristicDetails_' + item.Value);
                                    if (!Clinical_PhysicalExam.isNullOrUndefined(currentObjectData)) {
                                        $.each(currentObjectData, function (key, value) {
                                            dataDetail[key + liId] = value;
                                        });

                                        Clinical_PhysicalExam.showHideToggleDetails(true);
                                        $.when(Clinical_PhysicalExam.toggleDetailsDiv(liId)).then(function () {
                                            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #detailHeading").text(textForHeading);
                                            utility.bindMyJSONByName(true, dataDetail, false, detailSection);
                                        });
                                    }
                                    else {
                                        var detailSection = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails");
                                        var selectedText = $(this).find('label').text() + ' Details';
                                        liId = $(this).attr('id');
                                        var currentObjectData = Clinical_PhysicalExam.ExamDetails[liId];
                                        currentObjectData = (typeof currentObjectData != 'undefined' && currentObjectData != null) ? currentObjectData : "{}";
                                        if ($(this).find("input:checkbox:checked").length > 0) {
                                            Clinical_PhysicalExam.showHideToggleDetails(true);
                                            $.when(Clinical_PhysicalExam.toggleDetailsDiv(liId)).then(function () {
                                                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #sectionPhysicalExamDetails header.panel-heading h2").text(selectedText);
                                                utility.bindMyJSONByName(true, JSON.parse(currentObjectData), false, detailSection);
                                            });
                                        }
                                        else {
                                            Clinical_PhysicalExam.deleteFromJsonObject("ulExamCharacteristics", item.Value);
                                            Clinical_PhysicalExam.showHideToggleDetails(false);
                                            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam div#divTogglePhysicalExamDetails").addClass('hidden');
                                        }
                                    }

                                });

                                //   }
                                return;
                            }
                        });
                        Clinical_PhysicalExam.showHideToggleDetails(false);
                    }
                    if (!isCharactristicsExists) {

                        l.find('li#' + item.Value).on('click', function () {
                            var detailSection = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails");
                            var selectedText = $(this).find('label').text() + ' Details';
                            liId = $(this).attr('id');
                            var currentObjectData = Clinical_PhysicalExam.ExamDetails[liId];
                            currentObjectData = (typeof currentObjectData != 'undefined' && currentObjectData != null) ? currentObjectData : "{}";
                            if ($(this).find("input:checkbox:checked").length > 0) {
                                Clinical_PhysicalExam.showHideToggleDetails(true);
                                $.when(Clinical_PhysicalExam.toggleDetailsDiv(liId)).then(function () {
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #sectionPhysicalExamDetails header.panel-heading h2").text(selectedText);
                                    utility.bindMyJSONByName(true, JSON.parse(currentObjectData), false, detailSection);
                                });
                            }
                            else {
                                Clinical_PhysicalExam.showHideToggleDetails(false);
                                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam div#divTogglePhysicalExamDetails").addClass('hidden');
                            }

                        });

                    }
                }
                else if (parentType != null && parentType.toLowerCase() == "characteristics") {
                    var isSubCharactristicsExists = false;
                    if (selectedData != null) {
                        var subCharacteristics = selectedData.SubCharacteristics;
                        $.each(subCharacteristics, function (i, subCharcetristicsComponent) {
                            if (subCharcetristicsComponent.CharSubCharacteristicId == item.Value) {

                                isSubCharactristicsExists = true;

                                if (subCharcetristicsComponent.IsDetailExists)
                                    l.find('li#' + item.Value).data('SystemSubCharacteristicDetails_' + item.Value, subCharcetristicsComponent.SectionCharacteristicSubCharacteristicDetailModel);
                                l.find('li#' + item.Value).data('SystemSubCharacteristicPk_' + item.Value, subCharcetristicsComponent.Id);

                                l.find('li#' + item.Value).addClass("green");
                                if (subCharcetristicsComponent.IsPositive) {

                                    if (l.find('li#' + item.Value).attr('subcharacteristicexist').trim() != "") {

                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '+ve"]').trigger('click');
                                    }
                                    else {
                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '+ve"]').prop('checked', true);
                                    }
                                }
                                else {
                                    if (l.find('li#' + item.Value).attr('subcharacteristicexist').trim() != "")
                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '-ve"]').trigger('click');
                                    else
                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '-ve"]').prop('checked', true);
                                }
                                if (subCharcetristicsComponent.Comments != "") {
                                    l.find('li#' + item.Value).find('button').first().addClass('green');
                                    l.find('li#' + item.Value).find('textarea').text(subCharcetristicsComponent.Comments)
                                    l.find('li#' + item.Value).find('button').attr("data-original-title", subCharcetristicsComponent.Comments);
                                    l.find('li#' + item.Value).find('button').attr("title", subCharcetristicsComponent.Comments);
                                }
                                else {
                                    l.find('li#' + item.Value).find('button').first().removeClass('green');
                                }

                                //  if (subCharcetristicsComponent.IsDetailExists) {

                                l.find('li#' + item.Value).on('click', function () {
                                    var detailSection = $('#' + Clinical_PhysicalExam.params.PanelID + " #sectionPhysicalExamDetails");
                                    var dataDetail = {};
                                    liId = $(this).attr("id");
                                    var selectedText = $(this).find('label').text() + ' Details';
                                    var currentObjectData = $(this).data('SystemSubCharacteristicDetails_' + item.Value);
                                    if (!Clinical_PhysicalExam.isNullOrUndefined(currentObjectData)) {
                                        $.each(currentObjectData, function (key, value) {
                                            dataDetail[key + liId] = value;
                                        });

                                        if ($(this).find("input:checkbox:checked").length > 0) {
                                            Clinical_PhysicalExam.showHideToggleDetails(true);
                                            $.when(Clinical_PhysicalExam.toggleDetailsDiv(liId)).then(function () {
                                                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #sectionPhysicalExamDetails header.panel-heading h2").text(selectedText);
                                                utility.bindMyJSONByName(true, dataDetail, false, detailSection);
                                            });
                                        }
                                        else {
                                            Clinical_PhysicalExam.showHideToggleDetails(false);
                                            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam div#divTogglePhysicalExamDetails").addClass('hidden');
                                        }
                                    }
                                    else {
                                        var detailSection = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails");
                                        var selectedText = $(this).find('label').text() + ' Details';
                                        liId = $(this).attr('id');
                                        var currentObjectData = Clinical_PhysicalExam.ExamDetails[liId];
                                        currentObjectData = (typeof currentObjectData != 'undefined' && currentObjectData != null) ? currentObjectData : "{}";
                                        if ($(this).find("input:checkbox:checked").length > 0) {
                                            Clinical_PhysicalExam.showHideToggleDetails(true);
                                            if (!Clinical_PhysicalExam.isBothUnCheck) {
                                                Clinical_PhysicalExam.isBothUnCheck = false;
                                                $.when(Clinical_PhysicalExam.toggleDetailsDiv(liId)).then(function () {
                                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #sectionPhysicalExamDetails header.panel-heading h2").text(selectedText);
                                                    utility.bindMyJSONByName(true, JSON.parse(currentObjectData), false, detailSection);
                                                });
                                            }
                                        }
                                        else {
                                            Clinical_PhysicalExam.showHideToggleDetails(false);
                                            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam div#divTogglePhysicalExamDetails").addClass('hidden');
                                        }
                                    }
                                });
                                // }
                                return;
                            }
                        });
                    }
                    if (!isSubCharactristicsExists) {

                        l.find('li#' + item.Value).on('click', function () {
                            var detailSection = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails");
                            var selectedText = $(this).find('label').text() + ' Details';
                            liId = $(this).attr('id');
                            var currentObjectData = Clinical_PhysicalExam.ExamDetails[liId];
                            currentObjectData = (typeof currentObjectData != 'undefined' && currentObjectData != null) ? currentObjectData : "{}";
                            if ($(this).find("input:checkbox:checked").length > 0) {
                                Clinical_PhysicalExam.showHideToggleDetails(true);
                                if (!Clinical_PhysicalExam.isBothUnCheck) {
                                    Clinical_PhysicalExam.isBothUnCheck = false;
                                    $.when(Clinical_PhysicalExam.toggleDetailsDiv(liId)).then(function () {
                                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #sectionPhysicalExamDetails header.panel-heading h2").text(selectedText);
                                        utility.bindMyJSONByName(true, JSON.parse(currentObjectData), false, detailSection);
                                    });
                                }
                            }
                            else {
                                Clinical_PhysicalExam.showHideToggleDetails(false);
                                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam div#divTogglePhysicalExamDetails").addClass('hidden');
                            }

                        });

                    }
                }
            }

        });
        return;
    },

    //Author: Farooq Ahmad
    //Date: 25-02-2016
    //This function will Reset All Data In Physical Exam Form
    resetPhysicalExam: function () {

        utility.myConfirm('This will reset all values in all Systems and clear all information. This action is irreversible and cannot be undone. Would you like to proceed? ', function () {

            clearPhysicalExamFieldsAndProperties();

        }, function () {

        }, "Confirm Reset");
    },

    clearPhysicalExamFieldsAndProperties: function () {

        $('#' + Clinical_PhysicalExam.params.PanelID).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {

            Clinical_PhysicalExam.resetControlValue(this);
        });

        utility.CreateDatePicker(Clinical_PhysicalExam.params.PanelID + '  #dtPhysicalExamDate', function () { }, true);

        var physicalExamPanel = $('#' + Clinical_PhysicalExam.params.PanelID);

        physicalExamPanel.find('ul li').removeClass('green');
        physicalExamPanel.find('ul li').removeClass('active');

        Clinical_PhysicalExam.unCheckMainNormalCheckbox();

        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li").each(function () {
            $(this).removeClass("green");
            $(this).removeClass("disableAll");
        });

        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divNormalExams").addClass("hidden");

        $('#' + Clinical_PhysicalExam.params.PanelID + " #txtNormalExamsDetail").val("");

        $('#' + Clinical_PhysicalExam.params.PanelID + " #divNormalExams").addClass("hidden");

        physicalExamPanel.find('section#SystemSections,section#SectionCharacteristics,section#SectionCharacteristics,section#CharacteristicsSubCharacteristics,section#sectionPhysicalExamDetails').addClass('hidden');

        Clinical_PhysicalExam.myArr = [];
        Clinical_PhysicalExam.ExamDetails = {};
        Clinical_PhysicalExam.SectionNormalInfo = [];

        Clinical_PhysicalExam.selectedcharacteristicsIds = [];
        Clinical_PhysicalExam.characteristicsWithData = [];
        Clinical_PhysicalExam.selectedsubcharacteristicsIds = [];
        Clinical_PhysicalExam.subcharacteristicsWithData = [];
        Clinical_PhysicalExam.SectionNormalInfo = [];
        Clinical_PhysicalExam.DataJSON = [];
        Clinical_PhysicalExam.DBDataJSON = [];
        Clinical_PhysicalExam.DBTemplateId = 0;
        Clinical_PhysicalExam.DataJSONTemp = [];
        Clinical_PhysicalExam.addGreenClasses();

    },

    //Author: Abid Ali
    //Date: 19-02-2016
    //This function will get  data of the inpute li Id and listType
    getObjectOfClickedElement: function (parentType, parentId) {
        var objData = null;
        //retrieve data of sections from system li's
        if (parentType != null && parentType.toLowerCase() == "system") {
            var ctrl = '#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems";
            objData = $(ctrl).find('li#' + parentId).data("SystemSectionIds_" + parentId);
        }
            //retrieve data of characteristics from section li's
        else if (parentType != null && parentType.toLowerCase() == "section") {
            var ctrl = '#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection";
            objData = $(ctrl).find('li#' + parentId).data("SystemCharacteristicsIds_" + parentId);
        }
            //retrieve data of subCharacteristics from characteristics li's
        else if (parentType != null && parentType.toLowerCase() == "characteristics") {
            var ctrl = '#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics";
            objData = $(ctrl).find('li#' + parentId).data("SystemSubCharacteristicsIds_" + parentId);
        }
        return objData;
    },

    //Author: Farooq Ahmad
    //Date: 25-02-2016
    //This function will set icon visibilty to block
    showIcon: function (obj) {

        $(obj).find('span').css('display', '');

    },

    //Author: Farooq Ahmad
    //Date: 25-02-2016
    //This function will set icon visibilty to none
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('span').css('display', 'none');
        }

    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle fill of PhysicalExam and it's childs as specified by PhysicalExamType
    loadPhysicalExamComponent: function (PhysicalExamType) {
        //Start 11-02-2016 Humaira Yousaf
        var objDeffered = $.Deferred();
        //End 11-02-2016 Humaira Yousaf
        Clinical_PhysicalExam.fillPhysicalExam(PhysicalExamType).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);

                var chkPhysicalExamNormal = '#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #chkPhysicalExamsNormal";
                var self = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam');

                if (response.physicalExamIsActive != false) {

                    var PhysicalExam_detail = JSON.parse(response.PatientPhysicalExamFill_JSON);
                    utility.bindMyJSONByName(true, PhysicalExam_detail, false, self).done(function () {
                        Clinical_PhysicalExam.params.mode = "Edit";
                        if (Clinical_PhysicalExam.TemplateType != 'DataTemplate') {
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #txtComments").val(PhysicalExam_detail.Comments);
                        }
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val(PhysicalExam_detail.physicalExamId);


                        Clinical_PhysicalExam.AllSystemNormarl($('#' + Clinical_PhysicalExam.params.PanelID + " #chkPhysicalExamsNormal"));
                        if (PhysicalExam_detail.NormalExamsDetail != null && PhysicalExam_detail.NormalExamsDetail.length > 0 && $('#' + Clinical_PhysicalExam.params.PanelID + " #chkPhysicalExamsNormal").prop("checked")) {
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails").addClass('green');
                            Clinical_PhysicalExam.openNormalExamDetail('toggle');
                            try {
                                if ($('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails").length > 0) {
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails")[0].style.setProperty('color', 'green', 'important');
                                }
                            }
                            catch (ex) {
                                console.log(ex);
                            }
                        }

                        var upperDate = self.find('input[name*="PatientPhysicalExamDate"]');

                        if (upperDate.val() == "") {
                            upperDate.datepicker('setDate', new Date());
                        } else {

                            //$(this).datepicker('setDate', $.datepicker.parseDate('yy-mm-dd', $(this).val()));
                            var date_format = 'mm/dd/yyyy';
                            //set default Date Formate
                            if (globalAppdata['DateFormat'])
                                date_format = globalAppdata['DateFormat'];
                            //  $(this).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), $(this).val()));
                            upperDate.datepicker({ date_format: date_format.replace('yy', '') }).val(upperDate.val());
                            upperDate.datepicker("setDate", upperDate.val());

                            Clinical_PhysicalExam.markSectionAsNormal(chkPhysicalExamNormal, false, true, PhysicalExam_detail.NormalExamsDetail);
                        }

                        if (Clinical_PhysicalExam.SelectedTempId != Clinical_PhysicalExam.DBTemplateId) {
                            upperDate.datepicker('setDate', new Date());
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #chkPhysicalExamsNormal").prop('checked', false);
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #divNormalExams").addClass("hidden");
                        }

                        //Start 11-02-2016 Humaira Yousaf
                        objDeffered.resolve(response.patientPhysicalExamSystemsFill_JSON);
                        //End 11-02-2016 Humaira Yousaf

                        //if(PhysicalExam_detail.TemplateId.toString() == '1')
                        //    Clinical_PhysicalExam.loadTemplateBasedPhysicalExam('1', 'True');
                        //else
                        //    Clinical_PhysicalExam.loadTemplateBasedPhysicalExam(PhysicalExam_detail.TemplateId.toString(), 'False');

                    });

                }
                $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').data('serialize', $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').serialize());

            }
            else {
                // utility.DisplayMessages(response.Message, 3);
                var self = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam');
                var upperDate = self.find('input[name*="PatientPhysicalExamDate"]');
                upperDate.datepicker('setDate', new Date());
                $('#' + Clinical_PhysicalExam.params.PanelID + " #chkPhysicalExamsNormal").prop('checked', false);
                $('#' + Clinical_PhysicalExam.params.PanelID + " #divNormalExams").addClass("hidden");
                objDeffered.resolve(null);
            }
        });
        return objDeffered.promise();
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle fill of PhysicalExam and it's childs as specified by PhysicalExamType
    loadPhysicalExam: function (PhysicalExamType, isLoadNew) {

        if (isLoadNew == true) {
            //Start 11-02-2016 Humaira Yousaf for deffered object
            var objDeffered = $.Deferred();
            Clinical_PhysicalExam.loadPhysicalExamComponent(PhysicalExamType).done(function (response) {
                objDeffered.resolve(response);
            });
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #hfPhysicalExamType").val(PhysicalExamType);
            return objDeffered.promise();
            //End 11-02-2016 Humaira Yousaf for deffered object
        }

    },

    //Author: Muhammad Arshad
    //Date: 19-01-2016
    //This function will handle fill of FamilyMember Details as specified by Given JSON
    fillCurrentMember: function (MemberId) {
        var currentJSON = Clinical_PhysicalExam.FamilyMembers[MemberId]
        if (currentJSON != null && currentJSON != "") {
            //Clinical_PhysicalExam.FamilyMembers
            //currentJSON = Clinical_PhysicalExam.FamilyMembers[Id];
            var self = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam');
            var currentJSON_detail = JSON.parse(currentJSON);
            utility.bindMyJSONByName(true, currentJSON_detail, false, self).done(function () {
                //Clinical_SocialHx.params.mode = "Edit";
            });
        }
    },


    //Author: Muhammad Arshad
    //Date: 01-05-2016
    //This function will handle active state of current li item on the basis of li's Id
    markStatusActive: function (ulId, liId) {
        if (ulId != null && ulId != "") {

            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #PhysicalExam ul#" + ulId + " li").each(function (i, item) {
                if ($(this).attr("id") != null && $(this).attr("id") == liId) {
                    if ($(this).hasClass("active") == false) {
                        $(this).addClass("active");
                        $(this).children().removeClass("green");
                        var currentId = $(this).attr("id");
                    }
                }
                else {
                    if ($(this).hasClass("active") == true) {
                        $(this).removeClass("active");
                    }
                }
            });
        }

    },

    //Author: Muhammad
    //Date: 01-05-2016
    //This function will handle enabling/disabling of Exercises controls on Miscellanous Tab
    enableDisableExercisesControls: function (currentStatus) {
        if (currentStatus != null && currentStatus != "") {
            var self = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #Miscellaneous section#sectionMiscDetails div#ExercisesDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (currentStatus.toLowerCase() == "does not exercise") {
                    if ($(this).attr("id") != "txtExercisesComments") {
                        $(this).attr("disabled", "disabled");
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                }
                else {
                    $(this).removeAttr("disabled");
                }
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
                Clinical_PhysicalExam.resetControlValue(this);
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
            });
        }
    },

    //Author: Muhammad Arshad
    //Date: 01-05-2016
    //This function will handle enabling/disabling of Exercises controls on Miscellanous Tab
    enableDisableList: function (listId, isDisable) {
        if (listId != null && listId != "") {
            var self = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam ' + listId + " li").not(":first").each(function () {

                if (isDisable) {
                    if (!$(this).hasClass('disableAll'))
                        $(this).addClass('disableAll');
                }
                else
                    $(this).removeClass('disableAll');
            });

        }
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle unremarkable feature for Family Hx
    unRemarkablePhysicalExam: function (obj, isFromLoad) {
        var isRemarkable = $(obj).prop("checked");
        if (isRemarkable == true) {
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #btnPhysicalExamSave').show();

            /* Start 23/12/2015 Muhammad Irfan for bug # EMR-153 */
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamTabsItems').addClass('disableAll');
            /* End 23/12/2015 Muhammad Irfan for bug # EMR-153 */

            /* Start 07/01/2016 Muhammad Irfan for bug # EMR-153 */
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #listTobacco,#listAlcohol,#listDrugAbuse,#listSexualHx').removeClass('successLight');
            /* End 07/01/2016 Muhammad Irfan for bug # EMR-153 */

        }
        else {
            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #btnPhysicalExamSave').hide();
        }
        var self = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').find('div#Tobacco,div#Alcohol,div#DrugAbuse,div#SexualHx,div#Miscellaneous').each(function () {
            if (isRemarkable == true) {
                if ($(this).hasClass("disableAll") == false) {
                    $(this).addClass("disableAll");

                }
                $(this).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,ul').each(function () {
                    $(this).attr("disabled", "disabled");
                    Clinical_PhysicalExam.resetControlValue(this);
                });
                //Clinical_PhysicalExam.resetControlValue(this);
            }
            else {
                $(this).removeClass("disableAll");
                /* Start 23/12/2015 Muhammad Irfan for bug # EMR-153 */
                $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamTabsItems').removeClass('disableAll');
                /* End 23/12/2015 Muhammad Irfan for bug # EMR-153 */

                $(this).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,ul').each(function () {
                    $(this).removeAttr("disabled");
                    Clinical_PhysicalExam.resetControlValue(this);
                });
            }
        });
        if (!isRemarkable)
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #listTobacco a").trigger("click");


    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will clear value of given control as specified by obj
    resetControlValue: function (obj) {
        var currentElementTagName = obj.tagName != null ? obj.tagName : obj.prop("tagName");
        if ($(obj).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea' || $(obj).attr('type') == 'hidden')
            $(obj).val('');
        if ($(obj).attr('type') == 'checkbox' || $(obj).attr('type') == 'radio') {

            if ($(obj).attr('type') == 'radio') {
                obj.checked = false;
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
                var groupRadBtn = $("input[name='" + $(obj).attr('name') + "']");
                if (groupRadBtn.length > 1) {
                    $.each(groupRadBtn, function (i, item) {
                        if ($(item).attr("id").toLowerCase().indexOf("no") > -1) {
                            $(item).trigger("click");
                        }
                    });
                }
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
            }
            else {
                obj.checked = false;
            }
        }

        if (currentElementTagName.toLowerCase() == 'select') {
            $(obj).find('option:selected').removeAttr('selected');
            //$(this).attr('selectedIndex', '-1');
            $(obj).find('option:eq(0)').attr('selected', 'selected');
        }
        if (currentElementTagName.toLowerCase() == 'ul') {
            $(obj).find('li.active').removeClass('active');
        }
    },

    //Author: Muhammad Arshad
    //Date: 21-01-2016
    //This function will check if any characteristic/subcharacteristic is selected
    isSystemChecked: function (objulSystem, liObject) {
        var oldTemplateId = '-2';
        var DetailExists = false;
        if (liObject != null && liObject != "") {
            $(liObject).find("input[type='checkbox']").each(function (i, chkitem) {
                if ($(chkitem).prop("checked") == true && DetailExists == false) {
                    DetailExists = true;
                    oldTemplateId = $(chkitem).closest('li').attr("templateId");
                }
            });
        }
        else {
            objulSystem.find("li").each(function (i, item) {
                $(item).find("input[type='checkbox']").each(function (i, chkitem) {
                    if ($(chkitem).prop("checked") == true && DetailExists == false) {
                        DetailExists = true;
                        oldTemplateId = $(chkitem).closest('li').attr("templateId");
                    }
                });
            });
        }
        Clinical_PhysicalExam.oldTemplateId = oldTemplateId;

        return DetailExists
    },

    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will check if any characteristic/subcharacteristic has data in details section
    isDetailsHaveData: function () {
        var DetailExists = false;
        var sectionDetails = "";
        var self = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails').find('[type=hidden],[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
            if ($(this).prop("disabled") != true && DetailExists == false) {
                var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'checkbox' && this.checked == true) {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'radio' && $(this).attr('id').toLowerCase().indexOf("no") > -1 && this.checked == true) {
                    DetailExists = false;
                }
                if (currentElementTagName.toLowerCase() == 'select' && $(this).val() != null && $(this).val() != "") {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'hidden' && $(this).val() != "" && $(this).val() != "-1") {
                    DetailExists = true;
                }
            }
        });

        return DetailExists;
    },

    //Author: Muhammad Arshad
    //Date: 21-01-2016
    //This function will check if any characteristic/subcharacteristic has any value selected
    isDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";


        var self = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #PhysicalExam");
        var objCharacteristic = self.find("div#divExamCharacteristics");
        var objSubCharacteristic = self.find("section#CharacteristicsSubCharacteristics");

        if (objSubCharacteristic.hasClass("hidden") == false) {
            DetailExists = Clinical_PhysicalExam.isSystemChecked(objSubCharacteristic.find("ul#ulExamSubCharacteristics"));
            if (DetailExists == false) {
                DetailExists = Clinical_PhysicalExam.isSystemChecked(objCharacteristic.find("ul#ulExamCharacteristics"));
            }
        }
        else if (objCharacteristic.hasClass("hidden") == false) {
            DetailExists = Clinical_PhysicalExam.isSystemChecked(objCharacteristic.find("ul#ulExamCharacteristics"));
        }

        if (JSON.stringify(Clinical_PhysicalExam.DataJSON) === JSON.stringify(Clinical_PhysicalExam.DBDataJSON)) {

        } else {
            DetailExists = true;
        }
        return DetailExists;

    },

    //Author: Muhammad Arshad
    //Date: 03-02-2016
    //This function will return comma Separated Ids of either selected Characteristics/SubCharacteristics from Given JSON on basis of characteristics Type as parameter
    getCommaSeparatedIds: function (arrSelectedJSON, IsSubCharacteristic) {
        var selectedIds = "";
        var isFirstSelected = false;
        //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
        if (IsSubCharacteristic == true) {
            Clinical_PhysicalExam.subcharacteristicsWithData.length = 0;
            Clinical_PhysicalExam.selectedsubcharacteristicsIds.length = 0;
        }
        else if (IsSubCharacteristic == false) {
            Clinical_PhysicalExam.characteristicsWithData.length = 0;
            Clinical_PhysicalExam.selectedcharacteristicsIds.length = 0;
        }
        //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
        if (arrSelectedJSON != null)
            $.each(arrSelectedJSON, function (i, item) {
                var numericPart = 0;
                for (key in item) {
                    var myval = key;
                    if (key.indexOf("SubCharacteristicId") > -1) {
                        numericPart = key.replace(/[^\d]+/, '');
                    }
                }
                if (IsSubCharacteristic == true) {
                    var currentSubCharacteristicId = item["SubCharacteristicId" + numericPart];
                    var currentCharacteristicId = item["CharacteristicId" + numericPart];
                    if (currentSubCharacteristicId != null && currentSubCharacteristicId != "" && parseInt(currentSubCharacteristicId) > 0) {
                        if (isFirstSelected == false) {
                            selectedIds = currentSubCharacteristicId;
                            isFirstSelected = true;
                        }
                        else if (isFirstSelected == true) {
                            selectedIds += "," + currentSubCharacteristicId;
                        }

                        //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                        var num = Clinical_PhysicalExam.getNumberPart(item);
                        var Index = Clinical_PhysicalExam.selectedsubcharacteristicsIds.indexOf(num);

                        if (Index == -1 && item["CharacteristicId" + num] != num) {
                            Clinical_PhysicalExam.subcharacteristicsWithData.push(item);
                            Clinical_PhysicalExam.selectedsubcharacteristicsIds.push(num);
                        }
                        //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                    }
                }
                else {
                    var currentSubCharacteristicId = item["CharacteristicId" + numericPart];
                    if (item["CharacteristicId" + numericPart] != "") {
                        var currentCharacteristicId = item["CharacteristicId" + numericPart];
                        if (currentCharacteristicId != null && currentCharacteristicId != "" && parseInt(currentCharacteristicId) > 0) {
                            if (isFirstSelected == false) {
                                selectedIds = currentCharacteristicId;
                                isFirstSelected = true;
                            }
                            else if (isFirstSelected == true) {
                                selectedIds += "," + currentCharacteristicId;
                            }
                            var currentSubCharacteristicId = item["SubCharacteristicId" + numericPart];
                            if (currentSubCharacteristicId == "" || currentSubCharacteristicId == "-1") {
                                //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                                Clinical_PhysicalExam.characteristicsWithData.push(item);
                                Clinical_PhysicalExam.selectedcharacteristicsIds.push(currentCharacteristicId);
                                //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                            }

                        }
                    }
                }
            });

        return selectedIds;
    },

    //Author: Farooq AHmad
    //Date : 21-06-2016
    //This Function will check if Character or Sub Character is Selected
    CheckCharacterOrSubCharacterIsSelected: function () {
        var result = false;

        for (var index in Clinical_PhysicalExam.DataJSON) {

            if (Clinical_PhysicalExam.DataJSON[index].IsNormal == true)
                result = true;
            for (var secIndex in Clinical_PhysicalExam.DataJSON[index].Sections) {

                for (var charIndex in Clinical_PhysicalExam.DataJSON[index].Sections[secIndex].Characteristics) {

                    result = true;
                }
            }
        }

        return result;

    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Add/Edit of PhysicalExam and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous), it expects PhysicalExamType to be Add/Edit
    PhysicalExamSave: function (PhysicalExamType, UnloadPhysicalExam) {

        var dfd = $.Deferred();
        var PhysicalExamId = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val() != "" ? $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val() : "-1";

        if (parseInt(PhysicalExamId) > 0) {
            Clinical_PhysicalExam.params.mode = "Edit";
        }
        else {
            Clinical_PhysicalExam.params.mode = "Add";
        }


        if (!Clinical_PhysicalExam.CheckCharacterOrSubCharacterIsSelected() && !$('#' + Clinical_PhysicalExam.params.PanelID + " #chkPhysicalExamsNormal").prop('checked')) {
            utility.DisplayMessages("Please select characteristic to save.", 3);
            return;
        }

        var obj = new Object();
        obj.TemplateId = $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        if (obj.TemplateId.toString() == '1') {
            obj.TemplateId = null;
        }
        obj.PatientId = $('#PatientProfile #hfPatientId').val();
        obj.NotesId = Clinical_PhysicalExam.params.NotesId;
        obj.Comments = $('#' + Clinical_PhysicalExam.params.PanelID + " #txtComments").val();
        obj.Systems = Clinical_PhysicalExam.DataJSON;
        obj.PatientPhysicalExamId = PhysicalExamId;
        obj.PatientPhysicalExamDate = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #dtPhysicalExamDate").val();
        obj.NormalComments = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #txtNormalExamsDetail").val();
        obj.bNormal = $('#' + Clinical_PhysicalExam.params.PanelID + " #chkPhysicalExamsNormal").prop('checked');

        if (Clinical_PhysicalExam.params.mode == "Add") {

            AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Clinical_PhysicalExam.savePhysicalExam(obj).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val(response.PhysicalExamId);
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val(response.NoteId);
                            Clinical_PhysicalExam.params.mode = "Edit";
                            if (Clinical_PhysicalExam.params.ParentCtrl == "clinicalTabProgressNote" && UnloadPhysicalExam == true) {
                                Clinical_PhysicalExam.getPhysicalExamInfo(PhysicalExamType, UnloadPhysicalExam);
                            } else {
                                utility.DisplayMessages(response.message, 1);
                            }
                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').data('serialize', $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').serialize());

                            //if (UnloadPhysicalExam != null && UnloadPhysicalExam == false) {
                            //    //Unload Form upon save
                            //    Clinical_PhysicalExam.UnloadPhysicalExam();
                            //}


                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                        dfd.resolve();
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (Clinical_PhysicalExam.params.mode == "Edit") {

            AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Clinical_PhysicalExam.updatePhysicalExam(obj, PhysicalExamId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            if (Clinical_PhysicalExam.params.ParentCtrl == "clinicalTabProgressNote" && UnloadPhysicalExam == true) {
                                Clinical_PhysicalExam.getPhysicalExamInfo(PhysicalExamType, UnloadPhysicalExam);
                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                            }
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val(response.PhysicalExamId);
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val(response.NotesId);
                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').data('serialize', $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam').serialize());

                            //if (UnloadPhysicalExam != null && UnloadPhysicalExam == false) {
                            //    //Unload Form upon save
                            //    Clinical_PhysicalExam.UnloadPhysicalExam();
                            //}
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                        dfd.resolve();
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        return dfd.promise();
    },

    //Author : Farooq Ahmad
    //Date : 13-06-2016
    //This Function will return the Characteristics with data
    CharWithData: function () {
        var finalStrCharData = "";
        $.each(Clinical_PhysicalExam.characteristicsWithData, function (i, item) {
            var subCharNum = Clinical_PhysicalExam.getNumberPart(item);
            if (item['SubCharacteristicId' + subCharNum] == '' || item['SubCharacteristicId' + subCharNum] == '-1')
                item['CharacteristicId' + subCharNum] = subCharNum;
            finalStrCharData = finalStrCharData.concat(JSON.stringify(item)); //JSON.stringify(objData);
            finalStrCharData = finalStrCharData.replace("}{", ",");
        });
        return finalStrCharData;
    },

    //Author :  Farooq Ahmad
    //Date : 13-06-2016
    //This function will return the SubCharacteristics with data
    SubCharWithData: function () {
        var finalStrSubCharData = "";
        $.each(Clinical_PhysicalExam.subcharacteristicsWithData, function (i, item) {

            var subCharNum = Clinical_PhysicalExam.getNumberPart(item);
            item['SubCharacteristicId' + subCharNum] = subCharNum;

            finalStrSubCharData = finalStrSubCharData.concat(JSON.stringify(item));
            finalStrSubCharData = finalStrSubCharData.replace("}{", ",");
        });
        return finalStrSubCharData;
    },

    //Author : Farooq Ahmad
    //Date : 13-06-2016
    //This function will check the System , Section and Characterisctics are selected or not
    CheckForSysSecChar: function () {
        var self = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #sectionPhysicalExam");
        var errorMessage = "";
        var selectedSystemId = self.find("#ulPhysicalExamSystems li.active").attr("id");
        var selectedSectionId = self.find("#ulPhysicalExamSystemSection li.active").attr("id");
        var selectedCharacteristicId = self.find("#ulExamCharacteristics li.active").attr("id");
        var selectedSubCharacteristicId = 1;
        if (self.find("#CharacteristicsSubCharacteristics").hasClass("hidden") == false) {
            selectedSubCharacteristicId = self.find("#ulExamSubCharacteristics li.active").attr("id");
        }
        if ($('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #divTogglePhysicalExamDetails-1').hasClass('hidden') == true) {
            if (!(selectedSystemId != null && parseInt(selectedSystemId) > 0)) {
                errorMessage = "Please select any system.";
            }
            //Start 10-02-2016 Humaira Yousaf to check SectionNormalInfo
            if (Clinical_PhysicalExam.SectionNormalInfo == null || Clinical_PhysicalExam.SectionNormalInfo.length == 0) {
                //End 10-02-2016 Humaira Yousaf to check SectionNormalInfo
                if (!(selectedSectionId != null && parseInt(selectedSectionId) > 0)) {
                    errorMessage = "Please select any section.";
                }
                else if (!(selectedCharacteristicId != null && parseInt(selectedCharacteristicId) > 0)) {
                    errorMessage = "Please select any characteristic.";
                }
            }
        }
        return errorMessage;
    },

    //Author: Farooq Ahmad
    //Date: 26/02/2016
    //This will validate the detail section
    durationIsValid: function () {
        var Message = "";

        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails input[id*=txtPhysicalExamDurationLength]').each(function () {
            var num = $(this).attr('id').replace(/[^\d]+/, '');
            var stayLength = $(this).val();
            var ddlVal = $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails #ddlPhysicalExamDurationPeriod' + num).val();
            if (stayLength != null && stayLength != '') {
                if (ddlVal == null || ddlVal == '') {
                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #CharacteristicsDetails #ddlPhysicalExamDurationPeriod' + num).focus();
                    Message = "Please select Physical Exam Duration Period";
                }
            }

            if (ddlVal != null && ddlVal != '') {
                if (stayLength == null || stayLength == '') {
                    $(this).focus();
                    Message = "Please enter Physical Exam Duration Period";
                }
            }
        });

        return Message;


    },

    //Author: Muhammad Arshad
    //Date: 04-03-2016
    //This function will load PhysicalExam templates
    loadPhysicalExamTemplate: function (providerId, specialtyId) {
        var objData = new Object();
        objData["commandType"] = "load_physcialexam_default_template";
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["ProviderIds"] = providerId;
        objData["SpecialtyIds"] = specialtyId;
        objData["TemplateId"] = 1;
        objData["IsActive"] = 1;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    //Author: Ahmad Raza
    //Date: 23/06/2016
    //Function Name: loadPhysicalExamDataTemplate
    //DBCall to load physical Exam Data Templates
    loadPhysicalExamDataTemplate: function (providerId, specialtyId) {
        var objData = new Object();
        objData["commandType"] = "load_physcialexam_data_template";
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["ProviderIds"] = providerId;
        objData["SpecialtyIds"] = specialtyId;
        objData["TemplateId"] = 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle load of PhysicalExam and it's childs as specified by PhysicalExamType
    //It represents service call to API
    fillPhysicalExam: function (PhysicalExamType, physicalExamId) {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["PhysicalExamType"] = PhysicalExamType != null ? PhysicalExamType : "tobacco";
        var PhysicalExamId = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val();
        objData["PatientPhysicalExamId"] = (PhysicalExamId == "" || PhysicalExamId == null) ? -1 : PhysicalExamId;
        objData["NotesId"] = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val() == "" ? Clinical_PhysicalExam.params.NotesId : $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val();
        objData["commandType"] = "FILL_PatientPhysicalExam";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    fillPhysicalExamForSoap: function (physicalExamId) {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        var PhysicalExamId = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val();
        objData["PatientPhysicalExamId"] = (PhysicalExamId == "" || PhysicalExamId == null) ? -1 : PhysicalExamId;
        // objData["NotesId"] = Clinical_Notes.params.NotesId != null ? Clinical_Notes.params.NotesId : ($('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val() == "" ? Clinical_PhysicalExam.params.NotesId : $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val());
        objData["commandType"] = "FILL_PatientPhysicalExam";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Add of PhysicalExam
    //It represents service call to API
    savePhysicalExam: function (PhysicalExamData) {
        objData = PhysicalExamData;
        if (Clinical_PhysicalExam.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_PhysicalExam.params.patientID;
        }
        objData["TemplateId"] = $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        objData["NotesId"] = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val();
        objData["commandType"] = "SAVE_PatientPhysicalExam";

        var data = JSON.stringify(objData);

        ////var data = "PhysicalExamignsData=" + PhysicalExamignsData;
        //// serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },
    savePhysicalExamFromNotes: function (PhysicalExamData) {
        objData = PhysicalExamData;
        objData["commandType"] = "SAVE_PatientPhysicalExam";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    //Author : Abid Ali
    //Date: 08-02-2015
    //This function will handle Edit of PhysicalExam
    //It represents service call to API
    updatePhysicalExam: function (PhysicalExamData, PhysicalExamId) {

        objData = PhysicalExamData;
        if (Clinical_PhysicalExam.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_PhysicalExam.params.patientID;
        }
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        objData["TemplateId"] = $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        objData["NotesId"] = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val() == "" ? Clinical_PhysicalExam.params.NotesId : $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val();
        objData["commandType"] = "UPDATE_patientphysicalexam";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");

    },

    //Author : Muhammad Arshad
    //Date: 15-02-2016
    //This function will handle Insert/Update of Detail for Characteristic/SubCharacteristic
    //It represents service call to API
    saveDetailforCharacteristic: function (PhysicalExamData, PhysicalExamId) {

        var objData = JSON.parse(PhysicalExamData);
        if (Clinical_PhysicalExam.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_PhysicalExam.params.patientID;
        }
        objData["PatientPhysicalExamId"] = PhysicalExamId;

        if (PhysicalExamId > 0)
            objData["commandType"] = "UPDATE_DetailforCharacteristic";
        else
            objData["commandType"] = "SAVE_DetailforCharacteristic";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");

    },

    //Author : Muhammad Arshad
    //Date: 15-02-2016
    //This function will handle Insert/Update of Detail for SubCharacteristic
    //It represents service call to API
    saveDetailforSubCharacteristic: function (PhysicalExamData, PhysicalExamId) {

        var objData = JSON.parse(PhysicalExamData);
        if (Clinical_PhysicalExam.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_PhysicalExam.params.patientID;
        }
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        if (PhysicalExamId > 0)
            objData["commandType"] = "UPDATE_DetailforCharacteristic";
        else
            objData["commandType"] = "SAVE_DetailforCharacteristic";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");

    },

    //Author : Ahmad Raza
    //Date: 18-02-2016
    //This function will handle saving of Normal system's inner detail
    //It represents service call to API
    saveDetailforNormalSystem: function (PhysicalExamData, PhysicalExamId) {

        var objData = JSON.parse(PhysicalExamData);
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        //Start Farooq Ahmad 01/03/2016 Set the NotesId in Post Data
        objData["NotesId"] = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val() == "" ? Clinical_PhysicalExam.params.NotesId : $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val();
        //End Farooq Ahmad 01/03/2016 Set the NotesId in Post Data
        if (PhysicalExamId > 0)
            objData["commandType"] = "UPDATE_SYSTEM_NORMAL_DETAIL";
        else
            objData["commandType"] = "SAVE_SYSTEM_NORMAL_DETAIL";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");

    },


    //Author : Ahmad Raza
    //Date: 18-02-2016
    //This function will call DB to delete Characteristics detail with prompt
    deleteCharacteristicDetail: function (parentType, sectionId, physicalExamId, charId) {

        Clinical_PhysicalExam.deleteCharacteristicDetailDBCall(parentType, sectionId, physicalExamId, charId).done(function (response) {

            if (response == "Successfully Deleted") {

                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamCharacteristics > li#" + charId).find('input').attr('checked', false);
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamCharacteristics > li#" + charId).removeClass('green');
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamSubCharacteristics").find('li').attr('parentid', charId).find('input').attr('checked', false);
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamSubCharacteristics").find('li').attr('parentid', charId).removeClass('green');
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamCharacteristics > li#" + charId).removeData();
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection > li#" + sectionId).removeData();
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamSubCharacteristics  li").removeClass('green').removeClass('active');
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics ").addClass('hidden');

                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamCharacteristics > li#" + charId).find('#txtCharacteristicDetail' + charId).val('');
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamCharacteristics > li#" + charId).find('#btnOpenDetail' + charId).removeClass('green');
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamCharacteristics > li#" + charId).find('#btnOpenDetail' + charId).parent().find("div#divDetail" + charId).addClass('hidden');

            }
            else {
                utility.DisplayMessages(response, 3);
            }
        });
    },

    //Author : Ahmad Raza
    //Date: 18-02-2016
    //This function will call DB to delete Characteristics detail
    deleteCharacteristicDetailDBCall: function (parentType, sectionId, physicalExamId, charId) {

        if (!Clinical_PhysicalExam.isNullOrUndefined(sectionId)) {
            var objData = new Object();
            objData["patientId"] = $('#PatientProfile #hfPatientId').val();
            objData["patientPhysicaExamlId"] = physicalExamId;
            objData["sectionId"] = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection li#" + sectionId).data("SystemCharacteristicsIds_" + sectionId).Id;
            objData["characteristicId"] = charId;
            objData["commandType"] = "delete_Characteristics";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
        }

    },

    //Author : Abid Ali
    //Date: 15-03-2016
    //This function will delete SubCharacteristics detail from json and Dom
    deleteSubCharacteristicDetail: function (parentType, charId, physicalExamId, subCharPKId, subCharLiId) {

        //Delete corresponding char Li object
        var subCharData = Clinical_PhysicalExam.getObjectOfClickedElement('#ulExamSubCharacteristics', subCharLiId);

        if (!Clinical_PhysicalExam.isNullOrUndefined(subCharData)) {
            Clinical_PhysicalExam.deleteFromJsonObject('ulExamSubCharacteristics', subCharLiId)
            Clinical_PhysicalExam.deleteDomObjectFromList('#ulExamSubCharacteristics', subCharLiId);
        }

    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Unload of PhysicalExam Tab
    unLoadTab: function (nextOrPre, controlToInvoke) {

        var detailExists = Clinical_PhysicalExam.isDetailExists();
        if (Clinical_PhysicalExam.params.ParentCtrl == "clinicalTabProgressNote") {
            Clinical_PhysicalExam.controlToInvoke = controlToInvoke;
            if (detailExists == true || $('#' + Clinical_PhysicalExam.params.PanelID + " #txtComments").val() != "") {
                utility.myConfirm('19', function () {
                    var PhysicalExamType = $('#' + Clinical_PhysicalExam.params.PanelID + "  #hfPhysicalExamType").val();
                    if (typeof PhysicalExamType == "undefined" || PhysicalExamType == "") {
                        Clinical_PhysicalExam.UnloadPhysicalExam(nextOrPre);
                    }
                    else {

                        Clinical_PhysicalExam.bNextPrev = true;

                        Clinical_PhysicalExam.PhysicalExamSave(PhysicalExamType, true);
                    }
                }, function () {
                    Clinical_PhysicalExam.UnloadPhysicalExam(nextOrPre);
                },
               '1'
               );
            }
            else {
                Clinical_PhysicalExam.UnloadPhysicalExam(nextOrPre);
            }
        } else {

            Clinical_PhysicalExam.UnloadPhysicalExam();
        }
    },

    UnloadPhysicalExam: function (nextOrPre) {

        Clinical_PhysicalExam.selectedData = null;
        //Start Farooq Ahmad 18/02/2016 Empty Cache Array on Unload
        Clinical_PhysicalExam.myArr = [];
        Clinical_PhysicalExam.ExamDetails = {};
        Clinical_PhysicalExam.SectionNormalInfo = [];

        Clinical_PhysicalExam.selectedcharacteristicsIds = [];
        Clinical_PhysicalExam.characteristicsWithData = [];
        Clinical_PhysicalExam.selectedsubcharacteristicsIds = [];
        Clinical_PhysicalExam.subcharacteristicsWithData = [];
        Clinical_PhysicalExam.DataJSONTemp = [];
        Clinical_PhysicalExam.DBDataJSON = [];

        Clinical_PhysicalExam.DataJSON = [];
        Clinical_PhysicalExam.DataJSONTemp = [];
        Clinical_PhysicalExam.TemplateDataJSON = [];
        Clinical_PhysicalExam.DBTemplateId = 0;

        //End Farooq Ahmad 18/02/2016 Empty Cach Array On Unload

        if (Clinical_PhysicalExam.params["FromAdmin"] == "0") {
            if (Clinical_PhysicalExam.params != null && Clinical_PhysicalExam.params.ParentCtrl != null) {

                if (Clinical_PhysicalExam.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                    UnloadActionPan(Clinical_PhysicalExam.params.ParentCtrl, 'Clinical_PhysicalExam');
                    if (Clinical_PhysicalExam.controlToInvoke != null) {
                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_PhysicalExam.controlToInvoke.replace(/\s/g, ''));
                            Clinical_PhysicalExam.controlToInvoke = null;
                        }, 600);
                    }
                }
                else {
                    UnloadActionPan(Clinical_PhysicalExam.params.ParentCtrl, 'Clinical_PhysicalExam');
                    if (Clinical_PhysicalExam.controlToInvoke != null)
                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_PhysicalExam.controlToInvoke);
                            Clinical_PhysicalExam.controlToInvoke = null;
                        }, 600);
                }

            }
            else
                UnloadActionPan(null, 'Clinical_PhysicalExam');
        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_PhysicalExam").remove();
            RemoveAdminTab();
        }

        Clinical_PhysicalExam.SectionNormalInfo = [];
        EMRUtility.scrollToPNcomponent('clinical_physicalexam');
    },

    //-----------------Progress Note-------------
    // added on Dec 14,2015 by Muhammad Azhar Shahzad
    // Reason: These functions are used for Progress Note Soap Attachment, creation and detachment

    //Call Back function to add component to Progress Note
    addPhysicalExamToNotes: function () {
        var PhysicalExamId = Clinical_PhysicalExam.params.PhysicalExamId;
        var PhysicalExamType = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamType").val();

        Clinical_PhysicalExam.PhysicalExamSave(PhysicalExamType, true);
    },

    //this function will get Family History Soap Text and attach that to Progress note
    getPhysicalExamInfo: function (PhysicalExamType, UnloadPhysicalExam, physicalExamId) {
        Clinical_PhysicalExam.fillPhysicalExamForSoap(physicalExamId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_PhysicalExam.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadPhysicalExam);

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //Function name: getPhysicalExamSoapInfo
    //Author: Ahmad Raza
    //Date: 11-04-2016
    //Description: This function will handle load of PhysicalExam soap text
    getPhysicalExamSoapInfo: function (physicalExamId) {
        if (physicalExamId == null || physicalExamId == '') {
            return false;
        }
        Clinical_PhysicalExam.get_PhysicalExam_ForSOAP_DBCall(physicalExamId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_PhysicalExam.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', physicalExamId);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    getPhysicalExamInfoFromProvidersNote: function (PhysicalExamType, UnloadPhysicalExam, physicalExamId) {
        Clinical_PhysicalExam.fillPhysicalExamForProvidersNoteSoap(physicalExamId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_PhysicalExam.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadPhysicalExam);

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //Function name: get_PhysicalExam_ForSOAP_DBCall
    //Author: Ahmad Raza
    //Date: 11-04-2016
    //Description: This function will handle DB CAll for load of PhysicalExam soap text
    get_PhysicalExam_ForSOAP_DBCall: function (physicalExamId) {
        var objData = new Object();
        objData["PatientPhysicalExamId"] = physicalExamId;
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "get_physicalexam_for_soap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },
    //Function name:loadTemplatePhysicalExam
    //Author: Ahmad Raza
    //Date: 11-04-2016
    //This function will handle load of PhysicalExam tamplate
    loadTemplatePhysicalExam: function () {

        Clinical_PhysicalExam.fillPhysicalExam("tobacco").done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                var ExamDetail = JSON.parse(response.PatientPhysicalExamFill_JSON);


                if (response.status != false) {
                    Clinical_PhysicalExam.isFirstLoad = true;

                    if (parseInt(ExamDetail.physicalExamId) > 0 && (parseInt(ExamDetail.TemplateId) == -1 || ExamDetail.TemplateId == '')) {
                        ExamDetail.TemplateId = '1';
                    }
                    Clinical_PhysicalExam.DBTemplateId = ExamDetail.TemplateId;
                    Clinical_PhysicalExam.DBDataJSON = JSON.parse(response.patientPhysicalExamSystemsFill_JSON);
                    if (ExamDetail.TemplateId != null) {
                        Clinical_PhysicalExam.loadTemplateBasedPhysicalExam(ExamDetail.TemplateId, "false");
                    }



                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }

            //Clinical_PhysicalExam.showPhysExamTemplatesTab();
            Clinical_PhysicalExam.showHideAddButton();
        });

    },

    //This Function will check, if Family History Soap is already attached in Progress note, if Family History is not attached than it will create main divs to attach allergy
    checkPhysicalExamExists: function () {
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
            Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
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
    },

    //This Function is used to create SOAP html and append it to  Progress note
    createPhysicalExamBodyHTML: function (response, NoteHTMLCtrl, UnloadPhysicalExam) {
        Clinical_PhysicalExam.checkPhysicalExamExists();
        if (response.PatientPhysicalExamFill_JSON != null & response.PatientPhysicalExamFill_JSON != '') {
            var PhysicalExamFill_Obj = JSON.parse(response.PatientPhysicalExamFill_JSON);
            var $mainDivPhysicalExam = $(document.createElement('div'));

            var PhysicalExamId = PhysicalExamFill_Obj.physicalExamId;
            if (PhysicalExamId > 0) {
                var $SectionBodyPhysicalExam = $(document.createElement('section'));
                $SectionBodyPhysicalExam.attr('id', "Cli_PhysicalExam_Main" + PhysicalExamId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_PhysicalExam_" + PhysicalExamId);
                var $ListPhysicalExam = $(document.createElement('ul'));

                $ListPhysicalExam.attr('class', 'list-unstyled')

                $SectionBodyPhysicalExam.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_PhysicalExam_" + PhysicalExamId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_PhysicalExam_Main" + PhysicalExamId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListPhysicalExam.append("<li>" + PhysicalExamFill_Obj.physicalExamSoapText + "</li>");
                $DetailsDiv.append($ListPhysicalExam);
                $SectionBodyPhysicalExam.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_PhysicalExam').parent().parent().find('#Cli_PhysicalExam_Main' + PhysicalExamId).length == 0) {
                    $mainDivPhysicalExam.append($SectionBodyPhysicalExam);
                    Clinical_PhysicalExam.updatePhysicalExamHtml($mainDivPhysicalExam.html(), PhysicalExamId, NoteHTMLCtrl);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_PhysicalExam').parent().parent().find('#Cli_PhysicalExam_Main' + PhysicalExamId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_PhysicalExam').parent().parent().find('#Cli_PhysicalExam_Main' + PhysicalExamId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_PhysicalExam').parent().parent().find('#Cli_PhysicalExam_Main' + PhysicalExamId).html($SectionBodyPhysicalExam.html());
                    $(NoteHTMLCtrl + ' clinical_PhysicalExam').parent().parent().find('#Cli_PhysicalExam_Main' + PhysicalExamId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                    Clinical_PhysicalExam.updatePhysicalExamHtml("", PhysicalExamId, NoteHTMLCtrl);
                    Clinical_ProgressNote.hoverFunction();
                }

                if (UnloadPhysicalExam == true) {
                    Clinical_PhysicalExam.UnloadPhysicalExam(Clinical_PhysicalExam.bNextPrev);
                }
            } else {
                Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                Clinical_ProgressNote.hoverFunction();
            }
        } else {
            Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
            Clinical_ProgressNote.hoverFunction();
        }
    },

    IsNullReturnSoapValue: function (SoapValue) {
        return (SoapValue == "") ? "" : SoapValue + ",";
    },

    // This Function is called by Progress Notes (Fill PhysicalExam Func, CopyAllNotesCategories)
    updatePhysicalExamHtml: function (PhysicalExamHtml, PhysicalExamId, NoteHTMLCtrl) {
        $(NoteHTMLCtrl + ' clinical_PhysicalExam').parent().parent().addClass('initialVisitBody');
        if (PhysicalExamHtml != '') {
            $(NoteHTMLCtrl + ' clinical_PhysicalExam').parent().parent().append(PhysicalExamHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (PhysicalExamHtml != '') {
            Clinical_PhysicalExam.AttachPhysicalExamFromNotes(PhysicalExamId);
        }

    },

    //This Function detach Family History From progress note
    detach_ComponentsPhysicalExam: function (ComponentName, IsUpdate, PhysicalExamComponentRemove) {
        var Clinical_PhysicalExamIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').parent().parent().find('section[id*="Cli_PhysicalExam_Main"]').map(function () {
            return this.id.replace("Cli_PhysicalExam_Main", "");
        }).get().join(',');
        if (Clinical_PhysicalExamIds == "" || Clinical_PhysicalExamIds == "undefined") {
            Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
            Clinical_ProgressNote.hoverFunction();
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {


            Clinical_PhysicalExam.DetachPhysicalExamFromNotes_DBCall(Clinical_PhysicalExamIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {

                        Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                        Clinical_ProgressNote.hoverFunction();
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        if (PhysicalExamComponentRemove) {
            //Start//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Physical Exam']").remove();
            //End//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').parent().parent().remove();
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PhysicalExam').parent().parent().find('section[id*="Cli_PhysicalExam_Main"]').remove();
        }
    },

    //This Functions ask for Detaching Family Hx from Progress Note for current Patient Selected
    detachPhysicalExamFromNotes: function (PhysicalExamId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('29', function () {
                var selectedValue = PhysicalExamId.replace('Cli_PhysicalExam_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_PhysicalExam.DetachPhysicalExamFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + PhysicalExamId).remove();

                            Clinical_ProgressNote.updateProgressNoteHTML();
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
        // });
    },

    //This Functions attached Family Hx to Progress Note for current Patient Selected
    AttachPhysicalExamFromNotes: function (PhysicalExamId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var selectedValue = PhysicalExamId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_PhysicalExam.AttachPhysicalExamFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //If Attached PhysicalExam Made new inseration to PhysicalExam Table than good ids should be attached to HTML
                        Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                        $('#' + PhysicalExamId).remove();
                        Clinical_ProgressNote.hoverFunction();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }


        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    //If PhysicalExam Component which is dropeed in Progress note has no PhysicalExam attached, than it will call for Latest PhysicalExam for this patient
    getLatestPhysicalExamByPatientId: function () {

        Clinical_PhysicalExam.getLatestClinical_PhysicalExamByPatientId_DBCall().done(function (response) {
            //Start//02-03-2016//Ahmad Raza//Exception handled
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_PhysicalExam.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
                //End//02-03-2016//Ahmad Raza//Exception handled
            }

        });

    },

    //-----Server calls of Notes----------
    DetachPhysicalExamFromNotes_DBCall: function (PhysicalExamId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["PhysicalExamId"] = PhysicalExamId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "detach_PhysicalExam_from_notes";
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    AttachPhysicalExamFromNotes_DBCall: function (PhysicalExamId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["PhysicalExamId"] = PhysicalExamId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["TemplateId"] = $('#' + Clinical_PhysicalExam.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        objData["commandType"] = "attach_PhysicalExam_with_notes";
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    getLatestClinical_PhysicalExamByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["NotesId"] = Clinical_Notes.params.NotesId;
        objData["commandType"] = "getlatest_PhysicalExamby_patientid";
        objData["PatientPhysicalExamId"] = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val();

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },
    // end Azhar Change on Dec 15,2015
    //--------------end progress Note-----------

    //Author: Muhammad Arshad
    //Date: 26-01-2016
    //This function will handle setting/calculating width of PhysicalExam

    toggleVerticalWidth: function (obj) {


        var panelChildrens = null;
        var currentPanel = null;
        var applyWidth = null;
        if (obj != null) {
            currentPanel = $(obj.delegateTarget).parent();
            panelChildrens = currentPanel.children("section.panel");
            applyWidth = currentPanel;
            Clinical_PhysicalExam.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
        }
        else {
            $('.toggleVertical').each(function (e) {
                currentPanel = $(this);
                panelChildrens = currentPanel.children().children("section.panel");
                applyWidth = currentPanel.children();
                Clinical_PhysicalExam.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
            });


        }
    },

    //Author: Muhammad Arshad
    //Date: 26-01-2016
    //This function will handle calculating width of PhysicalExam

    toggleVerticalWidthCtrl: function (currentPanel, panelChildrens, applyWidth) {
        var totalWidth = 0;
        var hidden = 0;
        //find total width of panels
        panelChildrens.each(function (e) {
            totalWidth += $(this).outerWidth(true);
        });
        //find total width of hidden panel
        currentPanel.find("section.hidden").each(function (e) {
            hidden += $(this).outerWidth(true);
        });
        //apply width to div
        applyWidth.width(((totalWidth - hidden) + 60) + "px");

    },

    //Function Name: openNormalSectionDetail
    //Author Name: Humaira Yousaf
    //Created Date: 09-02-2016
    //Description: Opens detail textbox of sections
    //Params: var objButton
    //Params: var detailParentId
    openNormalSectionDetail: function (objButton, detailParentId) {
        if (objButton != null) {
            var self = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam")
            var isMainNormalChecked = self.find("input[id='chkNormalSection']").prop("checked");
            if (isMainNormalChecked == true) {
                var NormalExamsDetailDiv = self.find("div#textAreaNormal");
                if (NormalExamsDetailDiv.hasClass("hidden") == true) {
                    NormalExamsDetailDiv.removeClass("hidden")
                    self.find('#txtNormalSectionDetail').focus();
                }
                else {
                    NormalExamsDetailDiv.addClass("hidden")
                    self.find('#txtNormalSectionDetail').focusout();
                }
            }
            else {
                utility.DisplayMessages("Please check Normal Checkbox to view details", 3);
            }
        }
    },

    //Function Name: cacheNormalSystem
    //Author Name: Humaira Yousaf
    //Created Date: 09-02-2016
    //Description: Temporarily saves systems ids which are set as normal
    cacheNormalSystem: function () {
        var systemId = $(Clinical_PhysicalExam.SelectedSystem).attr('id');
        var indexNormal = -1;
        if (Clinical_PhysicalExam.SectionNormalInfo != null && Clinical_PhysicalExam.SectionNormalInfo.length > 0) {
            $.grep(Clinical_PhysicalExam.SectionNormalInfo, function (item, index) {
                if (item == parseInt(systemId)) {
                    indexNormal = index;
                    return;
                }
            });
            if (indexNormal == -1) {
                Clinical_PhysicalExam.SectionNormalInfo.push(systemId);
            }
        }
        else {
            Clinical_PhysicalExam.SectionNormalInfo.push(systemId);
        }
    },

    //Function Name: cacheNormalSystem
    //Author Name: Humaira Yousaf
    //Created Date: 09-02-2016
    //Description: Removes system from temporarily saved normal systems
    removeNormalSystem: function (sysId) {
        var systemId = $(Clinical_PhysicalExam.SelectedSystem).attr('id');

        //Start//29-03-2016//Abid Ali//removing sysIds from SectionNormalInfo array
        if (!Clinical_PhysicalExam.isNullOrUndefined(sysId)) {
            systemId = sysId;
        }
        //End//29-03-2016//Abid Ali//removing systemIds from SectionNormalInfo array

        var indexToDelete = -1;
        if (Clinical_PhysicalExam.SectionNormalInfo != null && Clinical_PhysicalExam.SectionNormalInfo.length > 0) {
            $.grep(Clinical_PhysicalExam.SectionNormalInfo, function (item, index) {
                if (item == parseInt(systemId)) {
                    indexToDelete = index;
                    return;
                }
            });
            Clinical_PhysicalExam.SectionNormalInfo.splice(indexToDelete, 1);
        }
    },

    //Function Name: physicalExamDelete
    //Author Name: Humaira Yousaf
    //Created Date: 18-02-2016
    //Description: Deletes physical exam
    //Params: var systemId
    physicalExamDelete: function (systemId) {

        var systemName = $("#ulPhysicalExamSystems li#" + systemId).text();
        try {
            Clinical_PhysicalExam.DeleteSystemFromJSON(systemId);
        } catch (ex) {
            console.log(ex);
        }
        try {
            Clinical_PhysicalExam.unCheckMainNormalCheckbox();
        } catch (ex) {
            console.log(ex);
        }
        try {
            Clinical_PhysicalExam.addGreenClasses();
        } catch (ex) {
            console.log(ex);
        }

    },

    //Author: Abid Ali
    //Date: 29-03-2016
    //This function will uncheck main Normal checkbox and will clear normal details.
    unCheckMainNormalCheckbox: function () {

        $('#' + Clinical_PhysicalExam.params.PanelID + " #chkPhysicalExamsNormal").prop('checked', false);
        $('#' + Clinical_PhysicalExam.params.PanelID + " #divNormalExams").find('#txtNormalExamsDetail').val('');
        $('#' + Clinical_PhysicalExam.params.PanelID + " #divNormalExams").find('#divNormalExamsDetail').addClass('hidden');
        $('#' + Clinical_PhysicalExam.params.PanelID + " #divNormalExams").addClass("hidden");

        $('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails").removeClass('green');
        if ($('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails").length > 0) {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #btnNormalExamDetails")[0].style.setProperty('color', '#0088cc', 'important');
        }
    },

    deletePhysicalExam: function (systemId) {


        var objData = new Object();
        objData["patientId"] = $('#PatientProfile #hfPatientId').val();
        objData["patientPhysicaExamlId"] = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamId").val();
        objData["systemId"] = systemId;
        objData["commandType"] = "delete_patientphysicalexam";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    //Function Name: deleteSectionDetail
    //Author Name: Humaira Yousaf
    //Created Date: 19-02-2016
    //Description: Deletes physical exam system section
    //Params: var parentType
    //Params: var parentId
    //Params: var physicalExamId
    //Params: var itemId
    //Params: var caller
    deleteSectionDetail: function (parentType, parentId, physicalExamId, itemId, caller) {

        // Start Humaira Yousaf 23-02-2016 to delete normal system data
        if (caller == 'markAsNormal') {
            Clinical_PhysicalExam.deletePhysicalExamSystemSection(parentId, physicalExamId, itemId).done(function (response) {
                if (response == "Successfully Deleted") {
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamCharacteristics li").removeClass('green').removeClass('active');
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamCharacteristics li").find('input:checkbox:checked').prop('checked', false);
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection li#" + itemId).removeClass('green').removeClass('active');

                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamSubCharacteristics li").find('input:checkbox:checked').prop('checked', false);
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamSubCharacteristics  li").removeClass('green').removeClass('active');
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics ").addClass('hidden');
                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SectionCharacteristics").addClass('hidden');

                    utility.DisplayMessages(response, 1);
                }
                else {
                    utility.DisplayMessages(response, 3);
                }

            });
        }
        else {

            var sectionName = $("#ulPhysicalExamSystemSection > li.active").find('a').text();
            utility.myConfirm('This will reset all values in ' + sectionName + ' section and clear all information. This action is irreversible and cannot be undone. Would you like to proceed? ', function () {
                Clinical_PhysicalExam.deletePhysicalExamSystemSection(parentId, physicalExamId, itemId).done(function (response) {
                    if (response == "Successfully Deleted") {
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamCharacteristics li").removeClass('green').removeClass('active');
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamCharacteristics li").find('input:checkbox:checked').prop('checked', false);
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection li#" + itemId).removeClass('green').removeClass('active');

                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamSubCharacteristics li").find('input:checkbox:checked').prop('checked', false);
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamSubCharacteristics  li").removeClass('green').removeClass('active');
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics ").addClass('hidden');
                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SectionCharacteristics").addClass('hidden');

                        utility.DisplayMessages(response, 1);
                    }
                    else {
                        utility.DisplayMessages(response, 3);
                    }

                });
            }, function () {

            }, "Confirm Delete");
        }
        // End Humaira Yousaf 23-02-2016 to delete normal system data
        // return dfd.promise();
    },

    //Function Name: deletePhysicalExamSystemSection
    //Author Name: Humaira Yousaf
    //Created Date: 19-02-2016
    //Description: Deletes physical exam system section
    //Params: var systemId
    //Params: var physicalExamId
    //Params: var sectionId
    deletePhysicalExamSystemSection: function (systemId, physicalExamId, sectionId) {

        var objData = new Object();
        objData["systemId"] = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems li#" + systemId).data("SystemSectionIds_" + systemId).Id;
        objData["patientPhysicaExamlId"] = physicalExamId;
        objData["sectionId"] = sectionId;
        objData["commandType"] = "delete_patientphysicalexamsystemsection";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    //Begin 26-02-2016 Added By Humaira Yousaf Bug# 377

    //Function Name: saveExamNormalDetail
    //Author Name: Humaira Yousaf
    //Created Date: 26-02-2016
    //Description: Saves normal exam detail
    //Params: var obj
    saveExamNormalDetail: function (obj) {
        var PhysicalExamId = Clinical_PhysicalExam.params.PhysicalExamId;
        var PhysicalExamType = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfPhysicalExamType").val();
        Clinical_PhysicalExam.PhysicalExamSave(PhysicalExamType, false);
        Clinical_PhysicalExam.openNormalExamDetail('toggle');
    },

    //Function Name: saveDetailforNormalExam
    //Author Name: Humaira Yousaf
    //Created Date: 26-02-2016
    //Description: Saves normal exam detail
    saveDetailforNormalExam: function (PhysicalExamData, PhysicalExamId) {

        var objData = JSON.parse(PhysicalExamData);
        if (Clinical_PhysicalExam.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_PhysicalExam.params.patientID;
        }
        //Start Farooq Ahmad 01/03/2016 Set the Note Id Value
        objData["NotesId"] = $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val() == "" ? Clinical_PhysicalExam.params.NotesId : $('#' + Clinical_PhysicalExam.params.PanelID + " #hfNotesId").val();
        //End Farooq Ahmad 01/03/2016 Set the Note Id Value
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        if (PhysicalExamId > 0)
            objData["commandType"] = "update_detailforexam";
        else
            objData["commandType"] = "save_detailforexam";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    //End 26-02-2016 Added By Humaira Yousaf Bug# 377

    toggleAttribute: function (control, IsPositive, pkid, event, type) {
        if (event != null) {
            event.preventDefault();
            event.stopPropagation();
        }
        Clinical_PhysicalExam.toggleAttribute_DbCall(IsPositive, pkid, type).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (IsPositive) {
                    $(control).text(' ( - ) ');
                    $(control).attr('onclick', 'Clinical_PhysicalExam.toggleAttribute(this,false,' + pkid + ',event,"' + type + '");')
                } else {
                    $(control).html(' ( + ) ');
                    $(control).attr('onclick', 'Clinical_PhysicalExam.toggleAttribute(this,true,' + pkid + ',event,"' + type + '");')
                }
                $(control).toggleClass('red');
                $(control).toggleClass('green');
                Clinical_ProgressNote.updateProgressNoteHTML();
                Clinical_ProgressNote.hoverFunction();
            }
        });
    },
    toggleAttribute_DbCall: function (IsPositive, pkid, type) {
        var objData = new Object();
        objData["IsPositive"] = IsPositive;
        objData["PrimaryKeyId"] = pkid;
        objData["type"] = type;
        objData["commandType"] = "toggle_PhysicalExamCharacteristics";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
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
            currentId = Clinical_PhysicalExam.NewInsertedId--;
            var subcharacteristicExist = "";
            if (itemType.toLowerCase() == "system") {
                currentLiClick = "Clinical_PhysicalExam.showHideChildControlsForTempt";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #" + currentCtrlId);

            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "Clinical_PhysicalExam.showHideChildControlsForTempt";
                currentCtrlId = "ulPhysicalExamSystemSection";
                ulControl = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #" + currentCtrlId);

            }
            else if (itemType.toLowerCase() == "characteristic") {

                subcharacteristicExist = 'subcharacteristicExist = "true"';
                var addSubCharItem = "Clinical_PhysicalExam.addSubCharItem(event,this,'" + currentId + "');";

                addSubCharIcon = '<a class="btn btn-xs pull-right" href="javascript:void(0);" onclick="' + addSubCharItem + '" title="Add SubCharacteristics"><i class="fa fa-plus blue"></i></a>';

                currentLiClick = "Clinical_PhysicalExam.showHideChildControlsForTempt";
                currentCtrlId = "ulExamCharacteristics";
                ulControl = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #" + currentCtrlId);

                charSelectAll = ulControl.find('li#chkboxSelectAllChars');

            }
            else if (itemType.toLowerCase() == "subcharacteristic") {
                currentLiClick = "Clinical_PhysicalExam.showHideChildControlsForTempt";
                currentCtrlId = "ulExamSubCharacteristics";
                isSubCharacteristic = true;
                ulControl = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #" + currentCtrlId);

                subCharSelectAll = ulControl.find('li#chkboxSelectAllSubChars');

            }
            if (ulControl != null && ulControl != "") {
                var currentLiClass = "";

                var arrNewlyAddedLi = ulControl.find("li[id*='-']");

                if (itemType.toLowerCase() != "system") {
                    currentParentId = ulControl.find("li:last").attr("parentid");
                    //Start Farooq Ahmad 16-03-2016 if no li is in ul
                    if (currentParentId == null)
                        currentParentId = ulControl.attr("ParentId");
                    //End Farooq Ahmad 16-03-2016 if no li is in ul
                }

                var onClick = "";
                //Start Farooq Ahmad 16-03-2016 set onclick prop
                onClick = "Clinical_PhysicalExam.showHideChildControlsForTempt('" + currentCtrlId + "','" + currentId + "',event);";

                var deleteFunction = "Clinical_PhysicalExam.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                liInnerText = '<div class="checkbox-custom checkboxTiny checkbox-success">' + deleteIcon + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label><div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs"><textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress="Clinical_PhysicalExam.AddNewLiInRespectiveUl(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xs pr-xlg height-max105 size100per textAreaScroll"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="Clinical_PhysicalExam.saveSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';

                var liTobeAdded = '<li id="' + currentId + '" ' + currentLiClass + ' parentid="' + currentParentId + '" value="' + currentId + '" refValue="' + currentId + '">' + liInnerText + '</li>';

                ulControl.prepend(liTobeAdded);

                ulControl.find('li#' + currentId + ' #txtName' + currentId).focus();
            }
        }
    },

    //            ulControl.find('li#' + currentId + ' #txtName' + currentId).focus()

    //        }
    // @Azahar Shahzad What is this ? :o
    //    }
    //},

    saveDetailOfSystem: function (event, obj) {
        if (event.which == 13) {
            event.preventDefault();


            if ($(obj).val() == '') {
                utility.DisplayMessages("Please enter some value", 3);
                return;
            }

            $(obj).focusout();
            $(obj).blur();

            this.currentIdOfText = $(obj).attr("id").replace("txtName", '');
            var onClickFunction = $(obj).closest('li').find('.btn-link').attr("onclick");
            var ID = $(obj).closest('li').find('.btn-link').attr("id");
            var ULID = $(obj).closest('li').find('.btn-link').closest("ul").attr("id");
            onClickFunction = onClickFunction.replace('this', "$('#" + Clinical_PhysicalExam.params.PanelID + " #" + ULID + " #" + ID + "')");
            eval(onClickFunction);
        }
    },
    saveTemplateSysSecCharSubchar: function (obj) {
        var MainId = '', Type = '';
        var objData = [];
        if ($(obj).closest("ul").attr("id") == "ulPhysicalExamSystems") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "PhysicalExamSystems";
        } else if ($(obj).closest("ul").attr("id") == "ulPhysicalExamSystemSection") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "PhysicalExamSystemSection";
        } else if ($(obj).closest("ul").attr("id") == "ulExamCharacteristics") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "ExamCharacteristics";
        } else if ($(obj).closest("ul").attr("id") == "ulExamSubCharacteristics") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "ExamSubCharacteristics";
        }


        Clinical_PhysicalExam.editName(obj, objData["MainId"], true);
    },
    FocusCommentTextBox: function (event) {
        event.stopPropagation();
    },

    addSubCharItem: function (event, obj, charId) {

        var $subChar = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics");
        $subChar.removeClass('hidden');

        if ($subChar.find('#ulExamSubCharacteristics').attr('parentid') != charId)
            $subChar.find('#ulExamSubCharacteristics').empty();

        $subChar.find('#ulExamSubCharacteristics').attr('parentid', charId);

        Clinical_PhysicalExam.toggleVerticalWidth();
        Clinical_PhysicalExam.addNewItem('subcharacteristic');
        event.stopPropagation();
    },


    //This function will add new System Section Characteristics or Subcharacteristics
    addNewSysSecCharSubchar: function (event, obj) {
        if (event.which == 13) {
            event.preventDefault();
            $(obj).focus();
            $(obj).closest("ul").attr("id");
        }
    },


    //This function will handle show/hide of Name Label/Textbox
    editName: function (objButton, detailParentId, changeLabel) {
        if (objButton != null && detailParentId != null) {
            var liObject = $(objButton).closest('li');
            var SystemDetailDiv = $(liObject).find("div#divNameDetail" + detailParentId);
            var SystemNameLabel = $(liObject).find("#lblName" + detailParentId);
            var txtSystemName = SystemDetailDiv.find("#txtName" + detailParentId);
            if (SystemDetailDiv.hasClass("hidden") == true) {
                SystemDetailDiv.removeClass("hidden");

                txtSystemName.val(SystemNameLabel.text());
                SystemNameLabel.addClass("hidden");
            }
            else {

                if (changeLabel != null) {
                    if (txtSystemName.val() != "") {
                        SystemNameLabel.text(txtSystemName.val());
                        SystemNameLabel.attr("data-original-title", txtSystemName.val());

                        txtSystemName.css({ "border": "1px solid #ccc" });
                    }
                    else {
                        //txtSystemName.css({ "border": "1px solid red" });
                        // return false;
                    }
                }

                SystemDetailDiv.addClass("hidden");
                SystemNameLabel.removeClass("hidden");

                //if (changeLabel != null) {
                //    Clinical_PhysicalExam.selectParentControls($(liObject).find('input:checkbox'));
                //}

            }
        }
    },
    //This function will handle show/hide of PhysicalExam child controls
    showHideChildControlsForTempt: function (parentCtrl, liId, event) {


        Clinical_PhysicalExam.parentCtrlGlobel = parentCtrl;

        //$('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #sectionPhysicalExamDetails").addClass('hidden');

        if (parentCtrl == "ulPhysicalExamSystems") {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divTogglePhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SystemSections").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SectionCharacteristics").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics").addClass('hidden');
        }
        if (parentCtrl == "ulPhysicalExamSystems" && liId != $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems li.active").attr("id")) {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divTogglePhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SectionCharacteristics").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics").addClass('hidden');
        }
        else if ((parentCtrl == "ulPhysicalExamSystemSection" && liId != $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li.active").attr("id"))) {
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divTogglePhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics").addClass('hidden');

        }

        if (parentCtrl != null && parentCtrl != "") {

            var childPartialId = "";
            var isSystemSectionCtrl = "";
            var isCharacteristicsCtrl = "";
            var isSubCharacteristicsCtrl = "";

            if (parentCtrl.toLowerCase() == "ulphysicalexamsystems") {
                isSystemSectionCtrl = "1";
                childPartialId = "System";
                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section[id^='SectionCharacteristics']").addClass("hidden");

            }
            else if (parentCtrl.toLowerCase() == "ulphysicalexamsystemsection") {
                isSystemSectionCtrl = "1";
                childPartialId = "Section";
            }
            else if (parentCtrl.toLowerCase() == "ulexamcharacteristics") {
                isCharacteristicsCtrl = "1";
                childPartialId = "Characteristics";
            }
            else if (parentCtrl.toLowerCase() == "ulexamsubcharacteristics") {
                isSubCharacteristicsCtrl = "1";
                childPartialId = "SubCharacteristics";
            }

            if (liId != null && liId != "") {
                $('#' + Clinical_PhysicalExam.parentCtrlGlobel).find("li").each(function (i, item) {
                    if ($(this).attr("id") != null && $(this).attr("id") == liId) {
                        $(this).closest("ul").find("li").removeClass('active');
                        $(this).addClass('active');
                        var objCurrent = item;


                        $.when(Clinical_PhysicalExam.loadChildData(liId, childPartialId)).then(function () {
                            Clinical_PhysicalExam.toggleDetailsDiv();
                            var objectListItem = null;
                            $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam").css('width', 'auto');
                            if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystems") {
                                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SystemSections").removeClass('hidden');
                                //Clinical_PhysicalExam.manageJsonData(liId, 'system');
                            }
                            else if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystemsection") {
                                $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#SectionCharacteristics").removeClass('hidden');
                            }
                            else if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics") {
                                objectListItem = $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamCharacteristics  li#' + liId);
                                var isbothUnCheck = ($(objectListItem).find('input[type=checkbox]:checked').length == 0);

                                var haveChild = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamSubCharacteristics li").length > 0;

                                if (!isbothUnCheck && haveChild)
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics").removeClass('hidden');
                                else
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#CharacteristicsSubCharacteristics").addClass('hidden');

                                try {
                                    var detail = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails");

                                    Clinical_PhysicalExam.manageJsonData(liId, 'characteristics', detail);


                                } catch (ex) {
                                    console.log(ex);
                                }

                            }
                            else if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulexamsubcharacteristics") {
                                try {
                                    objectListItem = $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamSubCharacteristics  li#' + liId);
                                    var detail = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExamDetails");

                                    Clinical_PhysicalExam.manageJsonData(liId, 'subcharacteristics', detail);
                                } catch (ex) {
                                    console.log(ex);
                                }

                                isSubCharacteristicsCtrl = "1";
                                childPartialId = "SubCharacteristics";
                            }
                            if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics" || Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulexamsubcharacteristics") {

                                var hideDetailSection = true;
                                if ($(objectListItem).find('input[type=checkbox]:checked').length == 0) {
                                    // Clinical_PhysicalExam.showHideToggleDetails(false);
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #sectionPhysicalExamDetails").addClass("hidden");
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divTogglePhysicalExamDetails").addClass("hidden");
                                } else {
                                    Clinical_PhysicalExam.toggleDetailsDiv();
                                    Clinical_PhysicalExam.showHideToggleDetails(true);
                                    if ($('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #divTogglePhysicalExamDetails .active").length > 0)
                                        hideDetailSection = false;
                                    $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #detailHeading ").text($(objectListItem).find('label').text() + " Detail");
                                    if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics") {
                                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsDetails").find("#hfCharacteristicId").val(liId);
                                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsDetails").find("#hfSubCharacteristicId").val('-1');
                                    }
                                    else {
                                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsDetails").find("#hfCharacteristicId").val('-1');
                                        $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #CharacteristicsDetails").find("#hfSubCharacteristicId").val(liId);
                                    }

                                }
                                if (!hideDetailSection) {
                                    Clinical_PhysicalExam.toggleVerticalWidth();
                                    //$(".toggleVertical div.toggle").parent().parent().scrollLeft(1000);
                                }

                                Clinical_PhysicalExam.BindDetailsOfCharAndSubChar(liId, Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase());
                            }




                            Clinical_PhysicalExam.addGreenClasses();

                            if (Clinical_PhysicalExam.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystems") {

                                for (var index in Clinical_PhysicalExam.DataJSON) {
                                    if (Clinical_PhysicalExam.DataJSON[index].SystemId == liId && Clinical_PhysicalExam.DataJSON[index].IsNormal == true) {
                                        $('#' + Clinical_PhysicalExam.params.PanelID + ' #divPhysicalExamSystemSection #chkNormalSection').prop("checked", true);
                                        Clinical_PhysicalExam.markSectionAsNormal($('#' + Clinical_PhysicalExam.params.PanelID + ' #divPhysicalExamSystemSection #chkNormalSection'), true);
                                        $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first').find('#txtNormalSectionDetail').val(Clinical_PhysicalExam.DataJSON[index].Comments);
                                        if (Clinical_PhysicalExam.DataJSON[index].Comments != null && Clinical_PhysicalExam.DataJSON[index].Comments.length > 0) {

                                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #frmClinicalPhysicalExam #ulPhysicalExamSystemSection li:first').find('#btnNormalSectionDetails i').removeClass('blue').addClass('green');

                                        }
                                    }
                                }
                            }
                        });
                    }
                });
            }
        }
    },

    //This function will mark parent control as checked
    selectParentControls: function (chkObject) {
        if (chkObject != null) {
            var isChecked = $(chkObject).prop("checked");
            //if (isChecked == true) {

            //Start Farooq Ahmad 02-03-2016 Store the Selected Items in Json Object
            if ($(chkObject).closest("ul").attr("id") == "ulPhysicalExamSystems") {
                var obj = {
                    SystemId: $(chkObject).closest("li").attr('id'),
                    IsChecked: isChecked,
                    SystemName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                    Sections: [],
                    IsModified: '1'
                };
                var isUpdated = false;
                for (var counter in Clinical_PhysicalExam.selectedPhyExamTempData) {
                    if (Clinical_PhysicalExam.selectedPhyExamTempData[counter].SystemId == obj.SystemId) {
                        Clinical_PhysicalExam.selectedPhyExamTempData[counter].IsModified = '1';
                        if (!isChecked) {
                            for (var innerIndex in Clinical_PhysicalExam.selectedPhyExamTempData[counter].Sections) {
                                Clinical_PhysicalExam.selectedPhyExamTempData[counter].Sections[innerIndex].IsChecked = isChecked;
                                Clinical_PhysicalExam.selectedPhyExamTempData[counter].Sections[innerIndex].IsModified = "1";
                                for (var mostInnerIndex in Clinical_PhysicalExam.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics) {
                                    Clinical_PhysicalExam.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].IsChecked = isChecked;
                                    Clinical_PhysicalExam.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].IsModified = "1";
                                    for (var innercounter in Clinical_PhysicalExam.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics) {
                                        Clinical_PhysicalExam.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[innercounter].IsChecked = isChecked;
                                        Clinical_PhysicalExam.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[innercounter].IsModified = "1";
                                    }
                                }
                            }
                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamSystemSection').find('input[type=checkbox]').prop("checked", false);
                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamCharacteristics').find('input[type=checkbox]').prop("checked", false);
                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamSubCharacteristics').find('input[type=checkbox]').prop("checked", false);

                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamSystemSection').find('li').removeClass('green');
                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamCharacteristics').find('li').removeClass('green');
                            $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamSubCharacteristics').find('li').removeClass('green');

                        }
                        obj.Sections = Clinical_PhysicalExam.selectedPhyExamTempData[counter].Sections;
                        Clinical_PhysicalExam.selectedPhyExamTempData[counter] = obj;
                        isUpdated = true;
                    }
                }

                if (!isUpdated) {
                    if (Clinical_PhysicalExam.selectedPhyExamTempData == null)
                        Clinical_PhysicalExam.selectedPhyExamTempData = [];
                    Clinical_PhysicalExam.selectedPhyExamTempData.push(obj);
                }


            } else if ($(chkObject).closest("ul").attr("id") == "ulPhysicalExamSystemSection") {

                var isParentExist = false;
                for (var index in Clinical_PhysicalExam.selectedPhyExamTempData) {
                    if (Clinical_PhysicalExam.selectedPhyExamTempData[index].SystemId == $(chkObject).closest("li").attr('parentid')) {
                        Clinical_PhysicalExam.selectedPhyExamTempData[index].IsModified = '1';
                        var obj = {
                            SystemId: $(chkObject).closest("li").attr('parentid'),
                            SectionId: $(chkObject).closest("li").attr('id'),
                            IsChecked: isChecked,
                            SectionName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                            Characteristics: [],
                            IsModified: '1'
                        };
                        var isUpdated = false;
                        for (var counter in Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections) {
                            if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[counter].SectionId == obj.SectionId) {
                                if (!isChecked) {

                                    for (var mostInnerIndex in Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[counter].Characteristics) {

                                        Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].IsChecked = isChecked;
                                        Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].IsModified = '1';

                                        for (var innercounter in Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].SubCharacteristics) {

                                            Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].SubCharacteristics[innercounter].IsChecked = isChecked;
                                            Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].SubCharacteristics[innercounter].IsModified = '1';
                                        }
                                    }

                                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamCharacteristics').find('input[type=checkbox]').prop("checked", false);
                                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamSubCharacteristics').find('input[type=checkbox]').prop("checked", false);


                                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamCharacteristics').find('li').removeClass('green');
                                    $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamSubCharacteristics').find('li').removeClass('green');

                                    if ($('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamSystemSection').find('input:checked').length == 0) {

                                        var $systemLi = $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamSystems').find('li#' + obj.SystemId);

                                        $systemLi.find('input[type=checkbox]').prop("checked", false);
                                        $systemLi.removeClass('green');
                                        //Remove system from Json
                                        Clinical_PhysicalExam.selectedPhyExamTempData[index].IsModified = "1";
                                        Clinical_PhysicalExam.selectedPhyExamTempData[index].IsChecked = false;
                                    }

                                }
                                obj.Characteristics = Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[counter].Characteristics;
                                Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[counter] = obj;
                                isUpdated = true;
                            }
                        }
                        if (!isUpdated) {
                            if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections == null)
                                Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections = [];
                            Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections.push(obj);
                        }

                        isParentExist = true;
                    }
                }
                if (!isParentExist) {
                    var sectionId = $(chkObject).closest("li").attr('id');
                    var systemId = $(chkObject).closest("li").attr('parentid');
                    var systemName = $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamSystems #lblName' + systemId).text();//  $(chkObject).closest("li").attr('parentid');
                    var system = {
                        SystemId: systemId,
                        IsChecked: isChecked,
                        SystemName: systemName,
                        Sections: [{
                            SystemId: $(chkObject).closest("li").attr('parentid'),
                            SectionId: sectionId,
                            IsChecked: isChecked,
                            SectionName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                            Characteristics: [],
                            IsModified: '1'
                        }],
                        IsModified: '1'
                    };
                    if (Clinical_PhysicalExam.selectedPhyExamTempData == null)
                        Clinical_PhysicalExam.selectedPhyExamTempData = [];
                    Clinical_PhysicalExam.selectedPhyExamTempData.push(system);
                }

            } else if ($(chkObject).closest("ul").attr("id") == "ulExamCharacteristics") {

                var isParentExist = false;
                for (var index in Clinical_PhysicalExam.selectedPhyExamTempData) {
                    for (var innerIndex in Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections) {
                        if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].SectionId == $(chkObject).closest("li").attr('parentid')) {

                            Clinical_PhysicalExam.selectedPhyExamTempData[index].IsModified = '1';
                            Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].IsModified = '1';

                            var obj = {
                                SectionId: $(chkObject).closest("li").attr('parentid'),
                                CharacteristicId: $(chkObject).closest("li").attr('id'),
                                CharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                IsChecked: isChecked,
                                SubCharacteristics: [],
                                IsModified: '1'
                            };
                            var isUpdated = false;
                            for (var counter in Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                                if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].CharacteristicId == obj.CharacteristicId) {


                                    if (!isChecked) {
                                        for (var innercounter in Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].SubCharacteristics) {
                                            Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].SubCharacteristics[innercounter].IsChecked = isChecked;
                                        }
                                        $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamSubCharacteristics').find('input[type=checkbox]').prop("checked", false);
                                        $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamSubCharacteristics').find('li').removeClass('green');
                                    }

                                    obj.SubCharacteristics = Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].SubCharacteristics;

                                    if (!isChecked) {

                                        //Assign 1 isModified to child data
                                        if (obj.SubCharacteristics != null) {

                                            $.each(obj.SubCharacteristics, function (subCharIndex, item) {
                                                item.IsModified = '1';
                                            });
                                        }
                                    }

                                    Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter] = obj;
                                    isUpdated = true;
                                }
                            }
                            if (!isUpdated) {
                                if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics == null)
                                    Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics = [];
                                Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics.push(obj);
                            }

                            isParentExist = true;
                        }
                    }
                }
                if (!isParentExist) {
                    var sectionId = $(chkObject).closest("li").attr('parentid');
                    var systemId = $('#' + Clinical_PhysicalExam.params.PanelID + " #ulPhysicalExamSystemSection li#" + sectionId).attr('parentid');
                    var systemName = $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamSystems #lblName' + systemId).text();
                    var sectionName = $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamSystemSection #lblName' + sectionId).text();
                    for (var index in Clinical_PhysicalExam.selectedPhyExamTempData) {
                        if (Clinical_PhysicalExam.selectedPhyExamTempData.SystemId == systemId) {
                            var section = {
                                SystemId: systemId,
                                SectionId: sectionId,
                                IsChecked: isChecked,
                                SectionName: sectionName,
                                Characteristics: [{
                                    SectionId: $(chkObject).closest("li").attr('parentid'),
                                    CharacteristicId: $(chkObject).closest("li").attr('id'),
                                    CharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                    IsChecked: isChecked,
                                    SubCharacteristics: [],
                                    IsModified: '1'
                                }],
                                IsModified: '1'
                            };
                            if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections == null)
                                Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections = [];
                            Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections.push(section);
                            isParentExist = true;
                        }
                    }
                    if (!isParentExist) {
                        var system = {
                            SystemId: systemId,
                            IsChecked: isChecked,
                            SystemName: systemName,
                            Sections: [{
                                SystemId: systemId,
                                SectionId: sectionId,
                                IsChecked: isChecked,
                                SectionName: sectionName,
                                Characteristics: [{
                                    SectionId: $(chkObject).closest("li").attr('parentid'),
                                    CharacteristicId: $(chkObject).closest("li").attr('id'),
                                    CharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                    IsChecked: isChecked,
                                    SubCharacteristics: [],
                                    IsModified: '1'
                                }],
                                IsModified: '1'
                            }],
                            IsModified: '1'
                        };
                        if (Clinical_PhysicalExam.selectedPhyExamTempData == null)
                            Clinical_PhysicalExam.selectedPhyExamTempData = [];
                        Clinical_PhysicalExam.selectedPhyExamTempData.push(system);
                    }
                }

            } else if ($(chkObject).closest("ul").attr("id") == "ulExamSubCharacteristics") {
                var isParentExist = false;
                for (var index in Clinical_PhysicalExam.selectedPhyExamTempData) {
                    for (var innerIndex in Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections) {
                        for (var mostInnerIndex in Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                            if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId == $(chkObject).closest("li").attr('parentid')) {

                                Clinical_PhysicalExam.selectedPhyExamTempData[index].IsModified = '1';
                                Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].IsModified = '1';
                                Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].IsModified = '1';

                                var obj = {
                                    CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                    SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                    SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                    IsChecked: isChecked,
                                    IsModified: '1'
                                };
                                var isUpdated = false;
                                for (var counter in Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics) {
                                    if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[counter].SubCharacteristicId == obj.SubCharacteristicId) {

                                        Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[counter] = obj;
                                        isUpdated = true;
                                    }
                                }
                                if (!isUpdated) {
                                    if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics == null)

                                        Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics = [];
                                    Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics.push(obj);
                                }

                                isParentExist = true;
                            }
                        }
                    }
                }
                if (!isParentExist) {
                    var characteristicId = $(chkObject).closest("li").attr('parentid');
                    var sectionId = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics li#" + characteristicId).attr('parentid');
                    var systemId = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection li#" + sectionId).attr('parentid');
                    var systemName = $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamSystems #lblName' + systemId).text();
                    var sectionName = $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulPhysicalExamSystemSection #lblName' + sectionId).text();
                    var charName = $('#' + Clinical_PhysicalExam.params.PanelID + ' #ulExamCharacteristics #lblName' + characteristicId).text();

                    for (var index in Clinical_PhysicalExam.selectedPhyExamTempData) {
                        for (var innerIndex in Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections) {
                            if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].SectionId == sectionId) {
                                var char = {
                                    SectionId: sectionId,
                                    CharName: charName,
                                    IsChecked: isChecked,
                                    CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                    SubCharacteristics: [{
                                        CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                        SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                        SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                        IsChecked: isChecked,
                                        IsModified: '1'
                                    }],
                                    IsModified: '1'
                                };
                                if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics == null)
                                    Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics = [];
                                Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics.push(char);
                                isParentExist = true;
                            }
                        }
                    }
                    if (!isParentExist) {
                        for (var index in Clinical_PhysicalExam.selectedPhyExamTempData) {
                            if (Clinical_PhysicalExam.selectedPhyExamTempData.SystemId == systemId) {
                                var section = {
                                    SystemId: systemId,
                                    SectionId: sectionId,
                                    IsChecked: isChecked,
                                    SectionName: sectionName,
                                    Characteristics: [{
                                        SectionId: sectionId,
                                        CharName: charName,
                                        IsChecked: isChecked,
                                        CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                        SubCharacteristics: [{
                                            CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                            SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                            SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                            IsChecked: isChecked,
                                            IsModified: '1'
                                        }],
                                        IsModified: '1'
                                    }],
                                    IsModified: '1'
                                };
                                if (Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections == null)
                                    Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections = [];
                                Clinical_PhysicalExam.selectedPhyExamTempData[index].Sections.push(section);
                                isParentExist = true;
                            }
                        }
                        if (!isParentExist) {
                            var system = {
                                SystemId: systemId,
                                IsChecked: isChecked,
                                SystemName: systemName,
                                Sections: [{
                                    SystemId: systemId,
                                    SectionId: sectionId,
                                    IsChecked: isChecked,
                                    SectionName: sectionName,
                                    Characteristics: [{
                                        SectionId: sectionId,
                                        CharName: charName,
                                        IsChecked: isChecked,
                                        CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                        SubCharacteristics: [{
                                            CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                            SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                            SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                            IsChecked: isChecked,
                                            IsModified: '1'
                                        }],
                                        IsModified: '1'
                                    }],
                                    IsModified: '1'
                                }],
                                IsModified: '1'
                            };
                            if (Clinical_PhysicalExam.selectedPhyExamTempData == null)
                                Clinical_PhysicalExam.selectedPhyExamTempData = [];
                            Clinical_PhysicalExam.selectedPhyExamTempData.push(system);
                        }

                    }
                }
            }

            var parentUlContrl = $(chkObject).parent().parent().parent();

            if (parentUlContrl != null && (parentUlContrl.attr("id") == "ulExamCharacteristics" || parentUlContrl.attr("id") == "ulExamSubCharacteristics")) {
                var currentParentId = $(chkObject).parent().parent().attr("parentid");

                if (parentUlContrl.attr("id") == "ulExamSubCharacteristics") {
                    var ParentCrtl = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulExamCharacteristics");
                    var parentLi = ParentCrtl.find("li#" + currentParentId);
                    parentLi.find("input[id*='chk']").prop("checked", true);
                    var parentSysId = parentLi.attr("parentid");
                    Clinical_PhysicalExam.selectParentSysControls(parentSysId);
                }
                else if (parentUlContrl.attr("id") == "ulExamCharacteristics") {
                    Clinical_PhysicalExam.selectParentSysControls(currentParentId);
                }
            }
        }
    },
    selectParentSysControls: function (ParentLiId) {
        if (ParentLiId != null) {
            var ParentCrtl = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystemSection");
            var parentLi = ParentCrtl.find("li#" + ParentLiId);
            parentLi.find("input[id*='chk']").prop("checked", true);

            var ParentSystCrtl = $('#' + Clinical_PhysicalExam.params.PanelID + " #frmClinicalPhysicalExam #ulPhysicalExamSystems");
            var parentSysLi = ParentSystCrtl.find("li#" + parentLi.attr("parentid"));
            parentSysLi.find("input[id*='chk']").prop("checked", true);
        }
    },
}