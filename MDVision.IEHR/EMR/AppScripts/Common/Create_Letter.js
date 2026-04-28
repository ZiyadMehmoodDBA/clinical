Create_Letter = {
    bIsFirstLoad: true,
    params: [],
    imageSize: 0,
    LetterContent: "",
    FavListName: "PatientLetter",
    Load: function (params) {
        Create_Letter.params = params;
        $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #btnAssignLetter').addClass("disabled");
        if (Create_Letter.params.PanelID != 'pnlCreate_Letter') {
            Create_Letter.params.PanelID = Create_Letter.params.PanelID + ' #pnlCreate_Letter';
        } else {
            Create_Letter.params.PanelID = 'pnlCreate_Letter';
        }

        EMRUtility.setFavoriteSectionStyle(Create_Letter.params.PanelID);
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Create_Letter.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }

        $("#" + Create_Letter.params.PanelID + " #LetterName").html(Create_Letter.params.TemplateLetterText);
        Create_Letter.TemplateContent = "";
        var self = $('#' + Create_Letter.params.PanelID);

        self.loadDropDowns(true).done(function () {

            if (Create_Letter.params.Status != "Signed") {

                $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #btnDeletePatientLetter').removeClass("disabled");
                $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #btnSavePatientLetter').removeClass("disabled");
                $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #btnSignPatientLetter').removeClass("disabled");
            }
            else {
                $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #btnDeletePatientLetter').addClass("disabled");
                $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #btnSavePatientLetter').addClass("disabled");
                $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #btnSignPatientLetter').addClass("disabled");
            }

            if (Create_Letter.params.mode == "Add") {

                $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #btnDeletePatientLetter').addClass("disabled");
                $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #btnSendLetter').addClass("disabled");
                $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #btnAssignLetter').addClass("disabled");

                //intialize TinyMCE instance on textarea control
                EMRUtility.InitTinymceControlWithMacro(false, null, null, Create_Letter.params.PanelID).done(function () {
                    Create_Letter.TemplateContent = tinyMCE.activeEditor.getContent();
                    Create_Letter.LoadTemplateLetter();
                });

                $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmCreate_Letter').serialize());




            }
            else if (Create_Letter.params.mode == "Edit") {

                if (Create_Letter.params.Status != "Signed") {
                    $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #btnDeletePatientLetter').removeClass("disabled");
                }





                EMRUtility.InitTinymceControlWithMacro(false, null, null, Create_Letter.params.PanelID).done(function () {
                    Create_Letter.TemplateContent = tinyMCE.activeEditor.getContent();
                    Create_Letter.LoadTemplateLetter();
                });
                $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmCreate_Letter').serialize());
                //Create_Letter.LoadTemplateLetter();
            }
            //Create_Letter.ValidateTemplateLetter();
            if (EMRUtility.getFavListStatus(Create_Letter.FavListName)) {
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #favSectionDivLetter").addClass("toggledHor");
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #FormDivCreateLetter").addClass("toggleHorContainer");
            }
            else {
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #favSectionDivLetter").removeClass("toggledHor");
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #FormDivCreateLetter").removeClass("toggleHorContainer");
            }

        });
        //InitTinymceControl(true);
        Create_Letter.GetMacrosList();
        if (Create_Letter.params.Status == "Signed") {
            $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #macro').addClass("disableAll");
        }
        else { $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter #macro').removeClass("disableAll"); }
    },
    //Start//8/3/2016//M Ahmad Imran//Implimented LoadTemplateLetter funtion for Get Template letter Detail for specific Patient
    LoadTemplateLetter: function () {

        Create_Letter.FillLetter().done(function (response) {

            response = JSON.parse(response);

            if (response.status != false) {
                if (response.PatientLetterContentCount != 0) {

                    var strToReplaceByte = '';

                    var strToReplace = '<img id="imgProvidereSignature" alt="" />';
                    if (response.PatientLetterContent.indexOf('<input id="184" class="TagInserted" style="min-width: 10px; border: none; width: 240px;" readonly="readonly" type="text" value="{{ Primary Care Provider eSignature }}" />') > -1) {
                        strToReplace = '<input id="184" class="TagInserted" style="min-width: 10px; border: none; width: 240px;" readonly="readonly" type="text" value="{{ Primary Care Provider eSignature }}" />';
                    }

                    if (response.PatientLetterContent.indexOf('<input id="191" class="TagInserted" style="min-width: 10px; border: none; width: 128px;" readonly="readonly" type="text" value="9737397163STATE FARM INSURANCE" />') > -1) {
                        var str = response.PatientLetterContent.substring(response.PatientLetterContent.indexOf('<input id="191"'));
                        str = str.substring(0, str.indexOf('/>') + 2);
                        response.PatientLetterContent = response.PatientLetterContent.replace(str, $(str).css("width", "300px")[0].outerHTML);

                    }


                    if (response.PatientLetterContent.indexOf('<input id="192" class="TagInserted" style="min-width: 10px; border: none; width: 142px;" readonly="readonly" type="text" value="respiratory treatment and nebulizer" />') > -1) {
                        var str = response.PatientLetterContent.substring(response.PatientLetterContent.indexOf('<input id="192"'));
                        str = str.substring(0, str.indexOf('/>') + 2);
                        response.PatientLetterContent = response.PatientLetterContent.replace(str, $(str).css("width", "230px")[0].outerHTML);
                    }

                    var strToReplaceCurentProvider = '<img id="imgProvidereSignature" alt="" />';
                    if (response.PatientLetterContent.indexOf('<input id="185" class="TagInserted" style="min-width: 10px; border: none; width: 205px;" readonly="readonly" type="text" value="{{ Current Provider eSignature }}" />') > -1) {
                        strToReplaceCurentProvider = '<input id="185" class="TagInserted" style="min-width: 10px; border: none; width: 205px;" readonly="readonly" type="text" value="{{ Current Provider eSignature }}" />';
                    }

                    var strToReplaceMiscellaneous = '<img id="imgProvidereSignature" alt="" />';
                    if (response.PatientLetterContent.indexOf('<input id="186" class="TagInserted" style="min-width: 10px; border: none; width: 184px;" readonly="readonly" type="text" value="{{ Miscellaneous eSignature }}" />') > -1) {
                        strToReplaceMiscellaneous = '<input id="186" class="TagInserted" style="min-width: 10px; border: none; width: 184px;" readonly="readonly" type="text" value="{{ Miscellaneous eSignature }}" />';
                    }

                    if (response.eSignatureBase64 != "") {

                        var isIEBrowser = EMRUtility.GetIEVersion() > 0 ? true : false;
                        var base64Type = "image/png;";
                        if (isIEBrowser) {
                            base64Type = "image/gif";
                        }

                        strToReplaceByte = "<img id=\"imgProvidereSignature\" src=\"data:" + base64Type + ";base64," + response.eSignatureBase64 + "\" />";
                    }

                    response.PatientLetterContent = response.PatientLetterContent.replace(new RegExp(strToReplace, "gi"), strToReplaceByte);
                    response.PatientLetterContent = response.PatientLetterContent.replace(new RegExp(strToReplaceCurentProvider, "gi"), strToReplaceByte);
                    response.PatientLetterContent = response.PatientLetterContent.replace(new RegExp(strToReplaceMiscellaneous, "gi"), strToReplaceByte);
                    var isReadonly = false;
                    if (Create_Letter.params.Status == "Signed") {
                        isReadonly = true
                    }
                    EMRUtility.InitTinymceControlWithMacro(isReadonly, null, null, Create_Letter.params.PanelID).done(function () {
                        response.PatientLetterContent = response.PatientLetterContent.replace(/{{ Miscellaneous eSignature }}/g, strToReplaceByte);
                        response.PatientLetterContent = response.PatientLetterContent.replace(/{{ Primary Care Provider eSignature }}/g, strToReplaceByte);
                        response.PatientLetterContent = response.PatientLetterContent.replace(/{{ Current Provider eSignature }}/g, strToReplaceByte);

                        Create_Letter.LoadClinicalTagsContent(response.PatientLetterContent).done(function (templateTags) {

                            tinyMCE.activeEditor.setContent(templateTags, { format: 'raw' });

                            var style = '<style type="text/css">.list-unstyled {padding-left: 0;list-style: none;}.initialVisitBody {padding-left: 0; list-style: none;} a, .btn-link {color: #0088cc;}</style>';
                            var letterBody = $("#elm1_ifr").contents().find("#tinymce");
                            if (letterBody.length > 0) {
                                $(letterBody).append(style);
                                var a = $(letterBody).find('.mce-item-anchor');
                                if (a.length > 0) {
                                    $(a).removeClass('mce-item-anchor');
                                }
                            }

                            Create_Letter.TemplateContent = tinyMCE.activeEditor.getContent();
                            $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter').data('serialize', $('#' + Create_Letter.params.PanelID + ' #frmCreate_Letter').serialize());


                        });

                    });


                } else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {

                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    LoadClinicalTagsContent: function (templateTags) {

        var def = new $.Deferred();

        if (Create_Letter.params.GrandParent == "clinicalTabProgressNote" || Create_Letter.params.ParentCtrl == "clinicalTabProgressNote") {


            var NotesId = Clinical_ProgressNote.params.NotesId;
            var PatientId = Clinical_ProgressNote.params.patientID;
            var ProviderId = Clinical_ProgressNote.params.ProviderId;

            Create_Letter.GetNotesData(NotesId, PatientId, ProviderId).done(function (response) {
                if (response.status != false) {

                    var NotesLoad_JSON = JSON.parse(response.NotesLoad_JSON);

                    if (NotesLoad_JSON.length > 0 && response.NoteComponentListFill_JSON) {

                        templateTags = Create_Letter.RemoveEmptyTags(templateTags, response.NoteComponentListFill_JSON);

                        $.each(response.NoteComponentListFill_JSON, function (i, item) {
                            if (item.SOAPText.indexOf('Info') > -1) {
                                item.SOAPText = item.SOAPText.split('(Info)').join('');
                            }
                            if (templateTags.indexOf('{{ Clinical Vitals }}') > -1 && item.ComponentName == "Vitals") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Vitals }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Vitals }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Problems }}') > -1 && item.ComponentName == "Problems") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Problems }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Problems }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Allergies }}') > -1 && item.ComponentName == "Allergies") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Allergies }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Allergies }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Social Hx }}') > -1 && item.ComponentName == "SocialHx") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Social Hx }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Social Hx }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Medical Hx }}') > -1 && item.ComponentName == "MedicalHx") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Medical Hx }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Medical Hx }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Birth Hx }}') > -1 && item.ComponentName == "BirthHx") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Birth Hx }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Birth Hx }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Family Hx }}') > -1 && item.ComponentName == "FamilyHx") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Family Hx }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Family Hx }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Surgical Hx }}') > -1 && item.ComponentName == "SurgicalHx") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Surgical Hx }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Surgical Hx }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Hospitalization Hx }}') > -1 && item.ComponentName == "HospitalizationHx") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Hospitalization Hx }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Hospitalization Hx }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Complaints }}') > -1 && item.ComponentName == "Complaints") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Complaints }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Complaints }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Medication }}') > -1 && item.ComponentName == "Medications") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Medication }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Medication }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Prescription }}') > -1 && item.ComponentName == "Prescription") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Prescription }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Prescription }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Procedures }}') > -1 && item.ComponentName == "Procedures") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Procedures }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Procedures }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Review of System }}') > -1 && item.ComponentName == "Review of Systems") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Review of System }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Review of System }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Physical Exam }}') > -1 && item.ComponentName == "Physical Exam") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Physical Exam }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Physical Exam }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Diagnostic Imaging Order }}') > -1 && item.ComponentName == "Diagnostic Imaging Order") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Diagnostic Imaging Order }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Diagnostic Imaging Order }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Consultation Order }}') > -1 && item.ComponentName == "Consultation Order") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Consultation Order }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Consultation Order }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Procedure Order }}') > -1 && item.ComponentName == "Procedure Order") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Procedure Order }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Procedure Order }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Follow Up }}') > -1 && item.ComponentName == "Follow Up") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Follow Up }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Follow Up }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Notes Extra Info }}') > -1 && item.ComponentName == "Notes Extra Info") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Notes Extra Info }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Notes Extra Info }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Lab Results }}') > -1 && item.ComponentName == "Lab Results") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Lab Results }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Lab Results }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Lab Order }}') > -1 && item.ComponentName == "Lab Orders") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Lab Order }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Lab Order }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Diagnostic Imaging Results }}') > -1 && item.ComponentName == "Diagnostic Imaging Results") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Diagnostic Imaging Results }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Diagnostic Imaging Results }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Referrals }}') > -1 && item.ComponentName == "Referrals") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Referrals }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Referrals }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Patient Education }}') > -1 && item.ComponentName == "Patient Education") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Patient Education }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Patient Education }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Functional and Cognitive }}') > -1 && item.ComponentName == "Functional And Cognitive") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Functional and Cognitive }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Functional and Cognitive }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Immunization }}') > -1 && item.ComponentName == "Immunization") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Immunization }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Immunization }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Social, Psychological and Behavior Hx }}') > -1 && item.ComponentName == "SocPsyandBehaviorHx") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Social, Psychological and Behavior Hx }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Social, Psychological and Behavior Hx }}/g, '');
                                }
                            }

                            if (templateTags.indexOf('{{ Clinical Treatment Plan }}') > -1 && item.ComponentName == "Treatment") {
                                if ($(item.SOAPText).find('section').length > 0) {
                                    templateTags = templateTags.replace(/{{ Clinical Treatment Plan }}/g, item.SOAPText);
                                }
                                else {
                                    templateTags = templateTags.replace(/{{ Clinical Treatment Plan }}/g, '');
                                }
                            }

                        });

                    }

                    def.resolve(templateTags);
                }

            });
        }
        else {
            def.resolve(templateTags);
        }

        return def.promise();
    },

    RemoveEmptyTags: function (templateTags, noteTags) {

        var componentNames = $(noteTags).map(function () {
            return this.ComponentName;
        }).get();

        if (templateTags.indexOf('{{ Clinical Vitals }}') > -1 && componentNames.indexOf("Vitals") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Vitals }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Problems }}') > -1 && componentNames.indexOf("Problems") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Problems }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Allergies }}') > -1 && componentNames.indexOf("Allergies") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Allergies }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Social Hx }}') > -1 && componentNames.indexOf("SocialHx") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Social Hx }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Medical Hx }}') > -1 && componentNames.indexOf("MedicalHx") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Medical Hx }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Birth Hx }}') > -1 && componentNames.indexOf("BirthHx") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Birth Hx }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Family Hx }}') > -1 && componentNames.indexOf("FamilyHx") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Family Hx }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Surgical Hx }}') > -1 && componentNames.indexOf("SurgicalHx") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Surgical Hx }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Hospitalization Hx }}') > -1 && componentNames.indexOf("HospitalizationHx") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Hospitalization Hx }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Complaints }}') > -1 && componentNames.indexOf("Complaints") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Complaints }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Medication }}') > -1 && componentNames.indexOf("Medications") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Medication }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Prescription }}') > -1 && componentNames.indexOf("Prescription") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Prescription }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Procedures }}') > -1 && componentNames.indexOf("Procedures") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Procedures }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Review of System }}') > -1 && componentNames.indexOf("Review of Systems") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Review of System }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Physical Exam }}') > -1 && componentNames.indexOf("Physical Exam") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Physical Exam }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Diagnostic Imaging Order }}') > -1 && componentNames.indexOf("Diagnostic Imaging Order") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Diagnostic Imaging Order }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Consultation Order }}') > -1 && componentNames.indexOf("Consultation Order") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Consultation Order }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Procedure Order }}') > -1 && componentNames.indexOf("Procedure Order") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Procedure Order }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Follow Up }}') > -1 && componentNames.indexOf("Follow Up") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Follow Up }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Notes Extra Info }}') > -1 && componentNames.indexOf("Notes Extra Info") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Notes Extra Info }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Lab Results }}') > -1 && componentNames.indexOf("Lab Results") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Lab Results }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Lab Order }}') > -1 && componentNames.indexOf("Lab Orders") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Lab Order }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Diagnostic Imaging Results }}') > -1 && componentNames.indexOf("Diagnostic Imaging Results") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Diagnostic Imaging Results }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Referrals }}') > -1 && componentNames.indexOf("Referrals") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Referrals }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Patient Education }}') > -1 && componentNames.indexOf("Patient Education") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Patient Education }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Functional and Cognitive }}') > -1 && componentNames.indexOf("Functional And Cognitive") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Functional and Cognitive }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Immunization }}') > -1 && componentNames.indexOf("Immunization") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Immunization }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Social, Psychological and Behavior Hx }}') > -1 && componentNames.indexOf("SocPsyandBehaviorHx") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Social, Psychological and Behavior Hx }}/g, '');
        }
        if (templateTags.indexOf('{{ Clinical Treatment Plan }}') > -1 && componentNames.indexOf("Treatment") == -1) {
            templateTags = templateTags.replace(/{{ Clinical Treatment Plan }}/g, '');
        }

        return templateTags;
    },
    GetNotesData: function (NotesID, PatientId, ProviderId) {
        var data = "NotesID=" + NotesID + "&PatientId=" + PatientId + "&ProviderId=" + ProviderId + "&FormName=Notes";
        return MDVisionService.defaultService(data, "DASHBOARD", "PREVIEW_NOTES");
    },

    //Start//8/3/2016//M Ahmad Imran//Implimented Call to Controller for Get Template letter Detail for specific Patient
    FillLetter: function () {
        var objData = {};
        if (Create_Letter.params.mode == "Edit") {
            objData["Patient_Letter_Id"] = Create_Letter.params.Patient_Letter_Id;
        }
        objData["TemplateLetterId"] = Create_Letter.params.TemplateLetterId;
        objData["PatientId"] = Create_Letter.params.PatientId;
        objData["Mode"] = Create_Letter.params.mode;
        objData["commandType"] = "Get_Content_Of_LETTER";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");
    },
    deletePatientLetter: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Letter", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (typeof Create_Letter.params.Patient_Letter_Id != 'undefined' && Create_Letter.params.Patient_Letter_Id != null && Create_Letter.params.Patient_Letter_Id != '') {
                    Patient_Letter.PatientLetterDelete(Create_Letter.params.Patient_Letter_Id);
                }
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    printPatientLetter: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Letter", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                // Adding Header Footer to Report, If Selected Provider of patient has any Report Header | Change Implmeneted by Azhar Shahzad on Aug08/11/016
                var ProviderId = '';
                if (Create_Letter.params.ParentCtrl && Create_Letter.params.ParentCtrl == "clinicalTabProgressNote")
                    ProviderId = Clinical_ProgressNote.params.CurrentNotesProviderId;
                else
                    ProviderId = $('#PatientProfile #hfPatientProviderId').val();
                Clinical_ReportHeader.ReportHeaderPrint_DbCall(ProviderId, Create_Letter.params.PatientId, 'Letters').done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var contents = $(tinyMCE.activeEditor.getBody()).html();

                        while (contents.indexOf("<div>&nbsp;</div>") > -1) {
                            contents = contents.replace("<div>&nbsp;</div>", "");
                        }

                        contents = response.Header + contents + response.Footer;
                        Create_Letter.getPrintnotePDF(response, contents);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    UnLoadTab: function (ComeFrom) {
        var objDeffered = $.Deferred();
        if (ComeFrom == "CloseButton") {

            // EMR-3235.
            // Faizan Ameen.
            // Dated: 21-March-2017.

            var contents = tinyMCE.activeEditor.getContent();
            while (contents.indexOf("<div>&nbsp;</div>") > -1) {
                contents = contents.replace("<div>&nbsp;</div>", "");

            }
            while (Create_Letter.TemplateContent.indexOf("<div>&nbsp;</div>") > -1) {
                Create_Letter.TemplateContent = Create_Letter.TemplateContent.replace("<div>&nbsp;</div>", "");

            }
            if (Create_Letter.TemplateContent.replace(/\r?\n|\r/g, "") != contents.replace(/\r?\n|\r/g, "")) {
                // if (Create_Letter.TemplateContent != tinyMCE.activeEditor.getContent()) {

                utility.myConfirm('Are you sure you want to save the changes?', function () {
                    Create_Letter.SaveFavToggelStatus();
                    Create_Letter.SavePatientLetter('Draft', 'unload');
                    Create_Letter.UnLoad().done(function () { objDeffered.resolve(); });
                }, function () {
                    Create_Letter.UnLoad().done(function () { objDeffered.resolve(); });
                },
                    'Save Changes'
                    );
            } else {
                Create_Letter.UnLoad().done(function () { objDeffered.resolve(); });
            }
        }
        else {
            Create_Letter.UnLoad().done(function () { objDeffered.resolve(); });
        }
        return objDeffered;
    },
    UnLoad: function () {
        var objDeffered = $.Deferred();

        if (Create_Letter.params["FromAdmin"] == "0") {
            if (Create_Letter.params != null && Create_Letter.params.ParentCtrl != null) {
                if (Create_Letter.params["ParentCtrlPanelID"])
                    UnloadActionPan(Create_Letter.params["ParentCtrl"], "Create_Letter", null, Create_Letter.params["ParentCtrlPanelID"]);
                else
                    UnloadActionPan(Create_Letter.params.ParentCtrl, 'Create_Letter');
            }
            else
                UnloadActionPan(null, 'Create_Letter');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
    //Start//1/03/2016//M Ahmad Imran//Implimented SaveLetter function which Save Template letter.
    SavePatientLetter: function (status, from) {
        if (typeof status != 'undefined' && status != '' && status != null && (status).toLowerCase() == 'signed') {
            Create_Letter.SavePatientLetterWithStatus(status, from);
        }
        else {
            if (Create_Letter.TemplateContent != tinyMCE.activeEditor.getContent() || Create_Letter.TemplateContent != '') {
                Create_Letter.SavePatientLetterWithStatus(status, from);
            }
            else {
                utility.DisplayMessages("Please make any changes to save/update Patient Letter ", 3);
            }
        }
    },
    SaveFavToggelStatus: function () {
        var isFavListOpened = $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #favSectionDivLetter").hasClass("toggledHor");
        EMRUtility.insertUpdateFavListStatus(Create_Letter.FavListName, isFavListOpened);
    },
    SavePatientLetterWithStatus: function (status, from) {
        var objDeffered = $.Deferred();
        if (Create_Letter.params.mode == "Add") {
            Create_Letter.convertHtmlToBase64(status).done(function (Base64String) {
                Create_Letter.PatientLetterSave(status, Base64String).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (from != 'unload') {
                            Create_Letter.SaveFavToggelStatus();
                        }
                        if (Create_Letter.params.ParentCtrl == "clinicalTabProgressNote") {
                            if (response.LetterId)
                                Create_Letter.getPatLetterInfo(response.LetterId, false);
                        }
                        Create_Letter.UnLoadTab("Save").done(function () {
                            SelectLetter_Template.UnLoadTab();
                            Patient_Letter.patientTemplateLetterSearch();
                            if ((status).toLowerCase() == 'signed') {
                                utility.DisplayMessages("Successfully Signed", 1);
                            } else {
                                utility.DisplayMessages(response.Message, 1);
                            }
                            objDeffered.resolve();
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        objDeffered.resolve();
                    }
                });
            });

        }
        else if (Create_Letter.params.mode == "Edit") {
            Create_Letter.convertHtmlToBase64(status).done(function (Base64String) {
                Create_Letter.PatientLetterUpdate(status, Base64String).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (from != 'unload') {
                            Create_Letter.SaveFavToggelStatus();
                        }
                        if (Create_Letter.params.ParentCtrl == "clinicalTabProgressNote") {
                            if (response.LetterId)
                                Create_Letter.getPatLetterInfo(response.LetterId, false);
                        }
                        Create_Letter.UnLoadTab("Save").done(function () {
                            UnloadActionPan("Patient_Letter", 'SelectLetter_Template');
                            Patient_Letter.patientTemplateLetterSearch();
                            if ((status).toLowerCase() == 'signed') {
                                utility.DisplayMessages("Successfully Signed", 1);
                            } else {
                                utility.DisplayMessages(response.Message, 1);
                            }
                            objDeffered.resolve();
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        objDeffered.resolve();
                    }
                });
            });
        }
        else
            objDeffered.resolve();
        return objDeffered;

    },
    //Start//2/3/2016//M Ahmad Imran//Implimented Call to Controller for Save Patient letter Detail
    PatientLetterSave: function (Status, Base64string) {


        var objData = {};
        var Sign = "";
        if (Status == "Signed") {
            Sign = Create_Letter.SignPatientLetter();
        }

        objData["Name"] = $("#" + Create_Letter.params.PanelID + " #LetterName").text();
        objData["Status"] = Status;
        objData["PatientLetterContent"] = tinyMCE.activeEditor.getContent() + Sign;
        objData["commandType"] = "SAVE_PATIENT_LETTER";
        objData["PatientId"] = Create_Letter.params.PatientId;
        objData["TemplateLetterId"] = Create_Letter.params.TemplateLetterId;
        objData["Base64String"] = Base64string;
        if (Create_Letter.params.GrandParent == "clinicalTabProgressNote" || Create_Letter.params.ParentCtrl == "clinicalTabProgressNote") {
            objData["VisitDate"] = $("#" + Clinical_ProgressNote.params["PanelID"]).find("#dtpVisitDate").val();
            objData["VisitTime"] = $("#" + Clinical_ProgressNote.params["PanelID"]).find("#VisitTime").val();
            objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");

    },
    //Start//2/3/2016//M Ahmad Imran//Implimented Call to Controller for Update Patient letter Detail
    PatientLetterUpdate: function (Status, Base64string) {
        var objData = {};
        var Sign = "";
        if (Status == "Signed") {
            Sign = Create_Letter.SignPatientLetter();
        }

        objData["Name"] = $("#" + Create_Letter.params.PanelID + " #LetterName").text();
        objData["Status"] = Status;
        objData["PatientLetterContent"] = tinyMCE.activeEditor.getContent() + Sign;
        objData["commandType"] = "UPDATE_PATIENT_LETTER";
        objData["PatientId"] = Create_Letter.params.PatientId;
        objData["Patient_Letter_Id"] = Create_Letter.params.Patient_Letter_Id;
        objData["Base64String"] = Base64string;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");

    },
    //Start//10/3/2016//M Ahmad Imran//Implimented Sign the patient's Letter.

    //Start 19-01-2017 Edited by Humaira Yousaf for EMR-2354
    getPrintnotePDF: function (response, contents) {
        if ($('#' + Create_Letter.params["PanelID"] + ' #printcall:visible').length == 0) {
            $('#' + Create_Letter.params["PanelID"] + ' #printcall').css('display', '');
        }
        $('#' + Create_Letter.params["PanelID"] + " #printcall #ulContent").html(contents);
        var imgLength = $('#' + Create_Letter.params["PanelID"] + " #printcall img").length;
        var images = $('#' + Create_Letter.params["PanelID"] + " #printcall img");

        var Logo = '';
        var logoHTML = $(response.Header);
        Logo = logoHTML.find('img').prop('src');

        var HeaderLogo = '';
        $.each(images, function (i, item) {
            if (item.src == Logo) {
                HeaderLogo = item.src;
            }

        });

        var FooterText = $('#' + Create_Letter.params["PanelID"] + " #printcall footer").text().split('Generated by: ').join('');

        if (HeaderLogo !== null && HeaderLogo !== "") {
            var CustomLogo = '<img id="ClinicalReportsHeaderLogo" src="' + HeaderLogo + '   " class="img-responsive" style="width: 100px;">';
            var insideHTML = $('#' + Create_Letter.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2354
            $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', HeaderLogo).css({ "max-width": "350px", "max-height": "140px" });
            //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2354
            $('#' + Create_Letter.params["PanelID"] + " #page-template").html(PageTemp);
        }
        else {
            var defaultLogo = 'content/images/SHS-nav-logo-small-100.png';
            var insideHTML = $('#' + Create_Letter.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', defaultLogo);
            $('#' + Create_Letter.params["PanelID"] + " #page-template").html(PageTemp);
            var patientData = JSON.parse(response.HeaderPatientData);
            var providerData = JSON.parse(response.HeaderProviderData);
            var practiceData = JSON.parse(response.HeaderPracticeData);
            if (patientData.length > 0) {
                var patientAccount = patientData[0].AccountNumber != "" ? "Acc. #: " + patientData[0].AccountNumber : "";
                var patientCell = patientData[0].CellNo != "" ? "Ph: " + patientData[0].CellNo : "";
                var patientName = (patientData[0].FirstName != "" ? patientData[0].FirstName : "") + (patientData[0].LastName != "" ? " " + patientData[0].LastName : "")
                var age = (patientData[0].Age != "" ? patientData[0].Age + " Y," : "") + " " + patientData[0].Gender + ", DOB: " + utility.RemoveTimeFromDate(null, patientData[0].DOB);
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #PatientName").html(patientName);
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #PatientAge").html(age);
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #PatientAccount").html(patientAccount);
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #PatientPhone").html(patientCell);
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #PatientAddress").html(patientData[0].Address1);
            }
            if (providerData.length > 0) {
                var providerName = (providerData[0].FirstName != "" ? providerData[0].FirstName : "") + (providerData[0].LastName != "" ? " " + providerData[0].LastName : "")
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #ProviderName").html(providerName);
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #Specialty").html(providerData[0].SpecialtyName);
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #LetterDateTime").html(utility.RemoveTimeFromDate(null, new Date().toString()));
            }
            if (practiceData.length > 0) {
                var city = practiceData[0].City;
                city += practiceData[0].State != "" ? ", " + practiceData[0].State : "";
                city += practiceData[0].ZIPCode != "" ? ", " + practiceData[0].ZIPCode : "";
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #PracticeName").html(practiceData[0].ShortName);
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #PracticeAddress").html(practiceData[0].Address);
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #PracticeCity").html(city);
                $('#' + Create_Letter.params.PanelID + " #frmCreate_Letter #PracticePhone").html(practiceData[0].PhoneNo);
            }
        }

        // ----- footer 
        if (FooterText !== null && FooterText !== "") {
            var insideHTML = $('#' + Create_Letter.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsFooter').text(FooterText);
            $('#' + Create_Letter.params["PanelID"] + " #page-template").html(PageTemp);

        }
        else {
            var footerText = $('#' + Create_Letter.params["PanelID"] + " #printcall  footer").text().split('Generated by: ').join('');
            var insideHTML = $('#' + Create_Letter.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            if (footerText == null || footerText == '') {
                footerText = 'MDVISION PM EMR'
            }
            $(PageTemp).find('#ClinicalReportsFooter').text(footerText);
            $('#' + Create_Letter.params["PanelID"] + " #page-template").html(PageTemp);

        }
        //PracticeDiv start

        var practiceHTML = '';
        if ($('#' + Create_Letter.params["PanelID"] + " #printcall #PracticeList").length > 0) {

            $('#' + Create_Letter.params["PanelID"] + " #printcall #PracticeList > li").each(function () {
                if ($(this).html().split(' ').join('') == '') {
                    $(this).remove();
                }
            });

            if ($('#' + Create_Letter.params["PanelID"] + " #printcall #PracticeList > li").length > 7) {
                $('#' + Create_Letter.params["PanelID"] + " #printcall #PracticeList > li:nth-child(7)").nextAll("li").remove();
            }
            practiceHTML = $('#' + Create_Letter.params["PanelID"] + " #printcall #PracticeList").length == 1 ? $('#' + Create_Letter.params["PanelID"] + " #printcall #PracticeList")[0].outerHTML : $('#' + Create_Letter.params["PanelID"] + " #printcall #PracticeList")[1].outerHTML;
        }
        var insideHTML = $('#' + Create_Letter.params["PanelID"] + " #page-template").html();
        var PageTemp = $(insideHTML);
        $(PageTemp).find('#PracticeDiv').html(practiceHTML.split('#').join('\\#'));
        $('#' + Create_Letter.params["PanelID"] + " #page-template").html(PageTemp);

        //if ($('#' + Create_Letter.params["PanelID"] + " #printcall #PracticeList").length > 0) {
        //    var practiceHTML = $('#' + Create_Letter.params["PanelID"] + " #printcall #PracticeList").length == 1 ? $('#' + Create_Letter.params["PanelID"] + " #printcall #PracticeList")[0].outerHTML : $('#' + Create_Letter.params["PanelID"] + " #printcall #PracticeList")[1].outerHTML;
        //    var insideHTML = $('#' + Create_Letter.params["PanelID"] + " #page-template").html();
        //    var PageTemp = $(insideHTML);
        //    $(PageTemp).find('#PracticeDiv').html(practiceHTML.split('#').join('\\#'));
        //    $('#' + Create_Letter.params["PanelID"] + " #page-template").html(PageTemp);
        //}
        //PracticeDiv end
        //PatientList start
        var patientHTML = '';
        if ($('#' + Create_Letter.params["PanelID"] + " #printcall #PatientList").length > 0) {
            $('#' + Create_Letter.params["PanelID"] + " #printcall #PatientList > li").each(function () {
                if ($(this).html().split(' ').join('') == '') {
                    $(this).remove();
                }
            });

            if ($('#' + Create_Letter.params["PanelID"] + " #printcall #PatientList > li").length > 7) {
                $('#' + Create_Letter.params["PanelID"] + " #printcall #PatientList > li:nth-child(7)").nextAll("li").remove();
            }

            patientHTML = $('#' + Create_Letter.params["PanelID"] + " #printcall #PatientList").length == 1 ? $('#' + Create_Letter.params["PanelID"] + " #printcall #PatientList")[0].outerHTML : $('#' + Create_Letter.params["PanelID"] + " #printcall #PatientList")[1].outerHTML;
        }

        var insideHTML = $('#' + Create_Letter.params["PanelID"] + " #page-template").html();
        var PageTemp = $(insideHTML);
        $(PageTemp).find('#PatientDiv').html(patientHTML.split('#').join('\\#'));
        $('#' + Create_Letter.params["PanelID"] + " #page-template").html(PageTemp);


        //if ($('#' + Create_Letter.params["PanelID"] + " #printcall #PatientList").length > 0) {
        //    var patientHTML = $('#' + Create_Letter.params["PanelID"] + " #printcall #PatientList").length == 1 ? $('#' + Create_Letter.params["PanelID"] + " #printcall #PatientList")[0].outerHTML : $('#' + Create_Letter.params["PanelID"] + " #printcall #PatientList")[1].outerHTML;
        //    var insideHTML = $('#' + Create_Letter.params["PanelID"] + " #page-template").html();
        //    var PageTemp = $(insideHTML);
        //    $(PageTemp).find('#PatientDiv').html(patientHTML.split('#').join('\\#'));
        //    $('#' + Create_Letter.params["PanelID"] + " #page-template").html(PageTemp);
        //}
        //PatientList end
        //ProviderDiv start
        var providerHTML = '';
        if ($('#' + Create_Letter.params["PanelID"] + " #printcall #ProviderList").length > 0) {

            $('#' + Create_Letter.params["PanelID"] + " #printcall #ProviderList > li").each(function () {
                if ($(this).html().split(' ').join('') == '') {
                    $(this).remove();
                }
            });

            if ($('#' + Create_Letter.params["PanelID"] + " #printcall #ProviderList > li").length > 7) {
                $('#' + Create_Letter.params["PanelID"] + " #printcall #ProviderList > li:nth-child(7)").nextAll("li").remove();
            }

            providerHTML = $('#' + Create_Letter.params["PanelID"] + " #printcall #ProviderList").length == 1 ? $('#' + Create_Letter.params["PanelID"] + " #printcall #ProviderList")[0].outerHTML : $('#' + Create_Letter.params["PanelID"] + " #printcall #ProviderList")[1].outerHTML;
        }
        var insideHTML = $('#' + Create_Letter.params["PanelID"] + " #page-template").html();;
        var PageTemp = $(insideHTML);
        $(PageTemp).find('#ProviderDiv').html(providerHTML.split('#').join('\\#'));
        $('#' + Create_Letter.params["PanelID"] + " #page-template").html(PageTemp);


        //if ($('#' + Create_Letter.params["PanelID"] + " #printcall #ProviderList").length > 0) {
        //    var providerHTML = $('#' + Create_Letter.params["PanelID"] + " #printcall #ProviderList").length == 1 ? $('#' + Create_Letter.params["PanelID"] + " #printcall #ProviderList")[0].outerHTML : $('#' + Create_Letter.params["PanelID"] + " #printcall #ProviderList")[1].outerHTML;
        //    var insideHTML = $('#' + Create_Letter.params["PanelID"] + " #page-template").html();;
        //    var PageTemp = $(insideHTML);
        //    $(PageTemp).find('#ProviderDiv').html(providerHTML.split('#').join('\\#'));
        //    $('#' + Create_Letter.params["PanelID"] + " #page-template").html(PageTemp);
        //}

        //ProviderDiv end

        var img = new Image();
        img.src = $(PageTemp).find('#ClinicalReportsHeaderLogo').attr('src');
        var srcWidth = img.width;
        var srcHeight = img.height;
        var maxWidth = 350;
        var maxHeight = 140;

        var headerImgHeight = 0;

        var topMargin = 0;
        var height = 0;

        var pracHeight = $(PageTemp).find('#PracticeDiv ul li:not(:empty)').length * 4.68;
        var logoHeight = 0;
        if (srcHeight > maxHeight) {
            headerImgHeight = Create_Letter.calculateHeaderImageSize(srcWidth, srcHeight, maxWidth, maxHeight).height;
            logoHeight = headerImgHeight * 0.26;
        }
        else {
            logoHeight = 140 * 0.26;
        }

        if (pracHeight < logoHeight) {
            pracHeight = logoHeight;
        }

        var ProvHeight = $(PageTemp).find('#ProviderDiv ul li:not(:empty)').length * 4.68;
        var patHeight = $(PageTemp).find('#PatientDiv ul li:not(:empty)').length * 4.68;
        if (patHeight > ProvHeight) {
            height += pracHeight + patHeight;
        } else {
            height += pracHeight + ProvHeight;
        }

        if ($(PageTemp).find('#ProviderDiv ul li').length == 7 || $(PageTemp).find('#PatientDiv ul li').length == 7) {
            height = height - 5;
        }

        topMargin = height;

        //var height = 0;
        //var ProvHeight = $(PageTemp).find('#ProviderDiv ul li:not(:empty)').length * 6;
        //var pracHeight = $(PageTemp).find('#PracticeDiv ul li:not(:empty)').length * 6;
        //var patHeight = $(PageTemp).find('#PatientDiv ul li:not(:empty)').length * 6;
        //if (patHeight > ProvHeight) {
        //    height += pracHeight + (patHeight / 2) + $(PageTemp).find('#PatientDiv ul li:not(:empty)').length - 2;
        //} else {
        //    height += pracHeight + (ProvHeight / 2) + $(PageTemp).find('#ProviderDiv ul li:not(:empty)').length - 2;
        //}
        //if (height <= 18) {
        //    $(PageTemp).find('.splitter:not(first)').hide();
        //} else {
        //    $(PageTemp).find('.splitter:not(first)').show();
        //}
        //var topMargin = height <= 18 ? 18 : height + 3;
        ////Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
        //topMargin += 5;
        ////End Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
        $('#' + Create_Letter.params["PanelID"] + " #printcall #PatientInfo").hide();
        if ($('#' + Create_Letter.params["PanelID"] + " #page-template .blueBorderPrint").length >= 1) {
            $('#' + Create_Letter.params["PanelID"] + " #printcall .form-group").hide();
            $('#' + Create_Letter.params["PanelID"] + " #printcall footer").hide();
        }
        topMargin = topMargin - 10;
        // --------------------------------------------- start Download functionality--------------------------------------------------------------
        kendo.drawing.drawDOM('#' + Create_Letter.params["PanelID"] + " #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            // margin: "2cm 3cm ",
            margin: {
                left: "10mm",
                top: topMargin + "mm",
                right: "10mm",
                bottom: "30mm"
            },
            template: $('#' + Create_Letter.params["PanelID"] + " #page-template").html()
        }).then(function (group) {
            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                params["PreviewPdf"] = true;
                $('#' + Create_Letter.params["PanelID"] + " #printcall #PatientInfo").hide();
                $('#' + Create_Letter.params["PanelID"] + " #printcall footer").hide();
                $('#' + Create_Letter.params["PanelID"] + " #printcall").hide();
                Create_Letter.PDFViewer(params["PrintPDFDataURL"], true, '#pnlCreate_Letter #PreviewReportPrint', false, true);
            });
        });

        // ------------------------------------- End Download functionality--------------------------------------
    },

    PDFViewer: function (base64, IsnewTab, ObjectControlID, IsIframe, IsPrint) {
        if (utility.UserBrowser() == "IE") {
            utility.SaveOrOpenBlob(base64, IsnewTab, ObjectControlID, IsIframe, IsPrint);
        } else {
            utility.PDFViewer(base64, IsnewTab, ObjectControlID, IsIframe, IsPrint);
        }
    },



    calculateHeaderImageSize: function (srcWidth, srcHeight, maxWidth, maxHeight) {

        var ratio = Math.min(maxWidth / srcWidth, maxHeight / srcHeight);
        return { width: srcWidth * ratio, height: srcHeight * ratio };
    },
    //End 19-01-2017 Edited by Humaira Yousaf for EMR-2354

    SignPatientLetter: function () {
        //var LetterContent = tinyMCE.activeEditor.getContent();
        var fullDate = new Date();
        var hours = fullDate.getHours();
        var hours = (hours + 24 - 2) % 24;
        var mid = 'AM';
        if (hours == 0) { //At 00 hours we need to show 12 am
            hours = 12;
        }
        else if (hours > 12) {
            hours = hours % 12;
            mid = 'PM';
        }

        var daysOfWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        var d = new Date();
        //    document.write("The current month is " + monthNames[d.getMonth()]);

        var CurrentDate = daysOfWeek[fullDate.getDay()] + ", " + monthNames[d.getMonth()] + " " + fullDate.getDate() + ", " + fullDate.getFullYear() + " at " + fullDate.getHours() + ":" + fullDate.getMinutes() + " " + mid;
        var SignedText = "electronically signed by: " + globalAppdata['AppUserNameFullName'] + " on " + CurrentDate;
        var SignedLength = globalAppdata['AppUserName'].length + 33; a
        var InsertFieldInput = '<br><input type="text" readonly value="' + SignedText + '" style="min-width: 10px; margin: 0 5px; margin-right:5px; border: none;padding: 0 0px;width:' + (SignedText.length + 4) * 7 + 'px;"/>';
        return InsertFieldInput;
    },
    SignPatientLetterForNote: function () {
        var fullDate = new Date();
        var hours = fullDate.getHours();
        var hours = (hours + 24 - 2) % 24;
        var mid = 'AM';
        if (hours == 0) //At 00 hours we need to show 12 am
            hours = 12;
        else if (hours > 12) {
            hours = hours % 12;
            mid = 'PM';
        }
        var daysOfWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var d = new Date();
        var CurrentDate = daysOfWeek[fullDate.getDay()] + ", " + monthNames[d.getMonth()] + " " + fullDate.getDate() + ", " + fullDate.getFullYear() + " at " + fullDate.getHours() + ":" + fullDate.getMinutes() + " " + mid;
        var SignedText = "electronically signed by: " + globalAppdata['AppUserNameFullName'] + " on " + CurrentDate;
        var SignedLength = globalAppdata['AppUserName'].length + 33; a
        var InsertFieldInput = '<br><div>' + SignedText + '</div>';
        return InsertFieldInput;
    },
    SendPatientLetter: function () {
        var params = [];
        params["patientId"] = Create_Letter.params.PatientId;
        params["mode"] = "Add";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Create_Letter';
        params["FromLetter"] = "1";
        params["Patient_Letter_Id"] = Create_Letter.params.Patient_Letter_Id;
        LoadActionPan('Document_AssignedTo', params);

    },
    convertHtmlToBase64: function (status) {
        var def = $.Deferred();
        if ((status).toLowerCase() == "signed") {
            var Sign = "";
            if ((status).toLowerCase() == "signed")
                Sign = Create_Letter.SignPatientLetter();
            $('#sendHtml').html(tinyMCE.activeEditor.getContent() + Sign);
            kendo.drawing.drawDOM("#sendHtml", {
                landscape: false,
                scale: 0.6,
                paperSize: "A4",
                // margin: "2cm 3cm ",
                margin: {
                    left: "10mm",
                    top: "10" + "mm",
                    right: "10mm",
                    bottom: "10mm"
                },
            }).then(function (group) {
                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                    $('#sendHtml').hide();
                    $('#pnlCreate_Letter #sendHtml').html('');
                    $('#pnlCreate_Letter #sendHtml').show();
                    def.resolve(dataURL);
                });
            });
        }
        else
            def.resolve(null);

        return def.promise();
    },
    sendAsFax: function () {
        $('#sendHtml').html(tinyMCE.activeEditor.getContent());


        kendo.drawing.drawDOM("#sendHtml", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            // margin: "2cm 3cm ",
            margin: {
                left: "10mm",
                top: "10" + "mm",
                right: "10mm",
                bottom: "10mm"
            },

        }).then(function (group) {
            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                $('#sendHtml').hide();
                var params = [];
                //Start 29-09-2017 Edit By Humaira Yousaf Bug# EMR-4861
                params["PDFBase64"] = dataURL.split('data:application/pdf;base64,').join('');
                //End 29-09-2017 Edit By Humaira Yousaf Bug# EMR-4861
                params["ParentCtrl"] = 'Create_Letter';
                params["PatientId"] = Create_Letter.params.PatientId;
                params["Subject"] = Create_Letter.params.TemplateLetterText;
                LoadActionPan("Batch_FaxSend", params);
                $('#pnlCreate_Letter #sendHtml').html('');
                $('#pnlCreate_Letter #sendHtml').show();
            });
        });
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

            //dropdown triggered here
            Create_Letter.GetMacros(null, null, keyword).done(function (response) {
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
                            macroDescDetails = $("#" + Create_Letter.params.PanelID + ' #MacroDescDetails');

                        if (response.MacroDetails.length > 0) {
                            if (response.MacroDetails.length == 1) {
                                Create_Letter.BindDescriptions(response.MacroDetails[0].Description.replace(/\r?\n/gi, "<br>"), keyword);

                            }
                            else {
                                $("#" + Create_Letter.params.PanelID + ' #MacroDescriptions').empty();
                                $.each(response.MacroDetails, function (i, item) {
                                    $("#" + Create_Letter.params.PanelID + ' #MacroDescriptions').append(Create_Letter.MacroDescriptions(item, keyword));
                                });
                                var tooltipWidth = $("#" + Create_Letter.params.PanelID + ' #MacroDescDetails').width();
                                var cursorPos = elmPosLeft + tooltipWidth + iframeLeftPos + 15;

                                if (cursorPos > widowWidth) {
                                    macroDescDetails.css("left", "auto");
                                    macroDescDetails.css("right", (iframeW - elmPosLeft));
                                }
                                else {
                                    macroDescDetails.css("right", "auto");
                                    macroDescDetails.css("left", elmPosLeft + 20);
                                }
                                macroDescDetails.css("top", elmPosTop + 115);

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
        //data["NoteComponentName"] = Create_Letter.params["ComponentName"];
        //data["NoteComponentId"] = $(Create_Letter.params["Control"]).attr('notecomponentid');
        //Create_Letter.params["NoteComponentId"] = data["NoteComponentId"];
        data["commandType"] = "Search_MacroDetailsForNotes";
        var obj = JSON.stringify(data);
        return MDVisionService.APIService(obj, "Macro", "Macro");
    },
    MacroDescriptions: function (item, Keyword) {
        var description;


        item.Description = item.Description.replace(/[']/g, "#quote#");
        item.Description = item.Description.replace(/["]/g, "#doublequote#");
        description = item.Description.replace(/[#quote#]/g, "");
        description = description.replace(/[#doublequote#]/g, '');
        return '<li><button onclick="Create_Letter.BindDescriptions(\'' + item.Description.replace(/\r?\n/gi, "<br>") + '\',\'' + Keyword + '\');" type="button">' + description + '</button></li>';
    },
    BindDescriptions: function (description, Keyword) {
        description = description.replace(new RegExp("#quote#", 'g'), "'");
        description = description.replace(new RegExp("#doublequote#", 'g'), '"');
        tinyMCE.activeEditor.setContent(tinymce.activeEditor.getBody().innerHTML.replace(Keyword + '..<span id="marker"></span>', description));
        // move cursor at end
        tinyMCE.activeEditor.selection.select(tinyMCE.activeEditor.getBody(), true);
        tinyMCE.activeEditor.selection.collapse(false);
        $("#" + Create_Letter.params.PanelID + ' #MacroDescDetails').hide();
    },
    GetMacrosList: function () {
        Create_Letter.GetMacros().done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                $('#' + Create_Letter.params.PanelID + " #ulMacroDetails").val('');

                $.each(response.MacroDetails, function (i, item) {
                    item.Description = item.Description.replace(/[']/g, "#quote#");
                    item.Description = item.Description.replace(/["]/g, "#doublequote#");
                    var li = Create_Letter.addMacros(item.MacroId, item.MacroName, item.Description, "", item.CreatedBy)

                    $('#' + Create_Letter.params.PanelID + " #ulMacroDetails").append(li);
                });
                $('#' + Create_Letter.params.PanelID + ' #macros').bind('keyup', function () {

                    var searchString = $(this).val();

                    $('#' + Create_Letter.params.PanelID + ' #ulMacroDetails li').each(function (index, value) {

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
    },
    addMacros: function (MacroId, MacroName, MacroDescription, IsNew, CreatedBy) {
        var Description = MacroDescription;
        Description = MacroDescription.replace(new RegExp("#quote#", 'g'), "");
        Description = Description.replace(new RegExp("#doublequote#", 'g'), '');

        var actions = "";
        actions = '<span class="removeIconListHover"><a class="btn  btn-xs pull-right" href="#" onclick="Create_Letter.deleteMacro(\'' + MacroId + '\',event);" title="Delete Record"><i class="fa fa-times red"></i></a>  <a class="btn  btn-xs  pull-right" href="#" onclick="Create_Letter.editMacroDetails(\'' + MacroId + '\',event);" title="Edit Record"><i class="fa fa-edit blue"></i></a> </span>'
        // actions = ' <span class="removeIconListHover"><a class="btn  btn-xs" href="#" onclick="Create_Letter.editMacroDetails('MacroId',event);" title="Edit Record"><i class="fa fa-edit blue"></i></a><a class="btn  btn-xs" href="#" onclick="Create_Letter.deleteMacro('10073',event);" title="Delete Record"><i class="fa fa-times red"></i></a></span>';
        if (!IsNew) {
            // if current user then show edit delete button
            if (globalAppdata.AppUserName.toString() == CreatedBy.toString() || globalAppdata.AppUserName.toString().toLowerCase() == "mdvision") {
                var li = '<li id="' + MacroId + '" onclick="Create_Letter.BindMacros(\'' + MacroId + '\',\'' + String(MacroDescription.replace(/\r?\n/gi, "<br>")) + '\', this,event);" value="' + MacroId + '" refvalue="" title="' + Description + '"  subcharacteristicexist=" " class=""><div class="">' +
                    '<label id="lblName' + MacroId + '" class="" data-toggle="tooltip" title="" data-original-title="' + MacroName + '">' + MacroName + '</label><div id="divNameDetail' + MacroId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + MacroId + '" onkeypress="" name="Name' + MacroId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                    MacroId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div>' + '<div class="clearfix"></div><div class="clearfix"></div></div></li>';
                return li;
            }
            else {
                var li = '<li id="' + MacroId + '" onclick="Create_Letter.BindMacros(\'' + MacroId + '\',\'' + String(MacroDescription.replace(/\r?\n/gi, "<br>")) + '\', this,event);" value="' + MacroId + '" refvalue="" title="' + Description + '"  subcharacteristicexist=" " class=""><div class="">' +
                     '<label id="lblName' + MacroId + '" class="" data-toggle="tooltip" title="" data-original-title="' + MacroName + '">' + MacroName + '</label><div id="divNameDetail' + MacroId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + MacroId + '" onkeypress="" name="Name' + MacroId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                     MacroId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div> <div class="clearfix"></div><div class="clearfix"></div></div></li>';
                return li;
            }

        }
        else {
            var li = '<li id="' + MacroId + '" onclick="Create_Letter.BindMacros(\'' + MacroId + '\',\'' + String(MacroDescription.replace(/\r?\n/gi, "<br>")) + '\', this,event);" value="' + MacroId + '" refvalue="" title="' + Description + '"  subcharacteristicexist=" " class=""><div class="">' +
                    '<label id="lblName' + MacroId + '" class="" data-toggle="tooltip" title="" data-original-title="' + MacroName + '">' + MacroName + '</label><div id="divNameDetail' + MacroId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + MacroId + '" onkeypress="" name="Name' + MacroId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                    MacroId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div>' + '<div class="clearfix"></div><div class="clearfix"></div></div></li>';
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

    addPatLetterToNotes: function () {
        var AttachedSelectedPatLetterIds = [];
        var DettachedSelectedPatLetterIds = [];
        AttachedSelectedPatLetterIds = EMRUtility.slicefunc(Clinical_ProgressNote.AttachedNoteComponentIds.slice(), "PatLetter", 0, -9);
        DettachedSelectedPatLetterIds = EMRUtility.slicefunc(Clinical_ProgressNote.DetachedNoteComponentIds.slice(), "PatLetter", 0, -9);
        var detachedvalues = DettachedSelectedPatLetterIds;
        if (detachedvalues.join() != null && detachedvalues.join() != '') {
            Create_Letter.detachPatLetter(detachedvalues).done(function () {
                if (AttachedSelectedPatLetterIds.join() != null && AttachedSelectedPatLetterIds.join() != '')
                    Create_Letter.attachPatLetter(AttachedSelectedPatLetterIds, false);
                else
                    Clinical_ProgressNote.saveComponentSOAPText("Letter", false);
            });
        }
        else if (AttachedSelectedPatLetterIds.join() != null && AttachedSelectedPatLetterIds.join() != '')
            Create_Letter.attachPatLetter(AttachedSelectedPatLetterIds, false);
    },
    attachPatLetter: function (SelectedLetterIds, hideAlertMessage) {
        Create_Letter.getPatLetterInfo(SelectedLetterIds.join(), hideAlertMessage);
    },
    getPatLetterInfo: function (LetterId, hideAlertMessage) {
        var dfd = new $.Deferred();
        Create_Letter.get_PatLetter_ForSOAP(LetterId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false)
                    Create_Letter.createPatLetterBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', LetterId, hideAlertMessage);
                else
                    utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    get_PatLetter_ForSOAP: function (LetterIds) {
        var objData = new Object();
        objData["TemplateLetterIds"] = LetterIds;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Patient_Letter.params.patientID;
        objData["commandType"] = "get_patletter_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ClinicalLetterTemplate");
    },
    checkPatLetterTagExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML patient_letter').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #MiscellaneousNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';
            $(CompnentSelector).append(' <li class="LetterComponent initialVisitBody" NoteComponentId="NCDummyId"> <header>' +
            '<patient_letter title="Letter" class="NotesComponent">' +
            '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'PatientLetterPreview\');" title="Letter">Letter</a> ' +
             '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Letter\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                            '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Letter\');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                       '</patient_letter> </header></li>');
            Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
    },
    createPatLetterBodyHTML: function (response, NoteHTMLCtrl, LetterIds, hideAlertMessage) {
        Create_Letter.checkPatLetterTagExists();
        PatLetterSOAPJSON = JSON.parse(response.PatLetterSoap_JSON);
        var $mainDivLetter = $(document.createElement('div'));
        var arrSections = [];
        $.each(PatLetterSOAPJSON, function (index, element) {
            var ALid = element.Patient_Letter_Id;
            var section = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML patient_letter').parent().parent().find('section[id="Cli_PatientLetter_Main' + ALid + '"]')
            if (section.length > 0)
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML patient_letter').parent().parent().find('section[id="Cli_PatientLetter_Main' + ALid + '"]').remove();

            $mainDivLetter.append(' <section LetterName="' + element.LetterName + '" id="' + "Cli_PatientLetter_Main" + ALid + '" class="pl-default text-info"> <header>'
            + '<a class="btn btn-link btn-xs" onclick="Patient_Letter.RowEditFromNotes(\'' + ALid + '\',\'' + element.LetterName + '\', \'' + element.Status + '\',event);" title="' + element.LetterName + '">' + element.LetterName + '</a>'
           + '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove pull-right hidden" title="Remove" name="' + "Cli_PatientLetter_Main" + ALid + '"  ><i class="fa fa-times"></i></a>'
                    + '</header></section>');

        });
        if ($mainDivLetter.html() != '')
            Create_Letter.updateLetterHtml($mainDivLetter.html(), LetterIds, NoteHTMLCtrl, hideAlertMessage);
        else
            Create_Letter.updateLetterHtml('', LetterIds, NoteHTMLCtrl, hideAlertMessage);
    },
    updateLetterHtml: function (PatLetterHTML, LetterIds, noteHTMLCtrl, hideAlertMessage) {
        $(noteHTMLCtrl + ' patient_letter').parent().parent().addClass('initialVisitBody');
        if (PatLetterHTML != '')
            $(noteHTMLCtrl + ' patient_letter').parent().parent().append(PatLetterHTML);
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (PatLetterHTML != '' && LetterIds && LetterIds != '0')
            Create_Letter.attachPatLetterFromNotes(LetterIds, hideAlertMessage);
        else
            Clinical_ProgressNote.saveComponentSOAPText("Letter", hideAlertMessage);
        UnloadActionPan(Patient_Letter.params.ParentCtrl, 'Patient_Letter');
    },
    attachPatLetterFromNotes: function (LetterIds, hideAlertMessage) {
        Create_Letter.attachLetterWithNotes_DBCall(LetterIds).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false)
                Clinical_ProgressNote.saveComponentSOAPText('Letter');
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    attachLetterWithNotes_DBCall: function (LetterIds) {
        var objData = new Object();
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["TemplateLetterIds"] = LetterIds;
        objData["commandType"] = "attach_pat_letter_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ClinicalLetterTemplate");
    },

    detach_ComponentsPatLetter: function (ComponentName, IsUpdate, ComponentRemove) {
        var PatletterIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML patient_letter').parent().parent().find('section[id*="Cli_PatientLetter_Main"]').map(function () {
            return this.id.replace("Cli_PatientLetter_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .LetterComponent').attr('NoteComponentId');
        if (!NoteComponentId)
            ComponentRemove = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML patient_letter').parent().parent().attr("NoteComponentId");
        if (ComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' patient_letter').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Letter']").remove();
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Letter']").remove();
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' patient_letter').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            }
        } else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML patient_letter').parent().parent().find('section[id*="Cli_PatientLetter_Main"]').remove();

        if (PatletterIds) {
            Create_Letter.detachedPatLetter(PatletterIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML patient_letter').parent().parent().find('section').remove();
                    Clinical_ProgressNote.saveComponentSOAPText('Letter');
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }
        else {
            if (NoteComponentId && NoteComponentId != "NCDummyId")
                Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId);
            Clinical_ProgressNote.ShowHideComponetsHeaders();
            Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Letter']").remove();
            utility.DisplayMessages('Successfully Deleted', 1);
        }
    },
    detachPatLetterFromNotes: function (PatletterId) {
        if (PatletterId) {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('patient_letter');
                var selectedValue = PatletterId.replace('Cli_PatientLetter_Main', '');
                Create_Letter.detachedPatLetter(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $('#' + PatletterId).remove();
                        Clinical_ProgressNote.saveComponentSOAPText('Letter');
                        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                });
            }, function () { },
            '1'
        );
        }
    },
    detachPatLetter: function (detachedvalues) {
        var dfd = new $.Deferred();
        Create_Letter.detachedPatLetter(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var ALid = detachedvalues[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_PatientLetter_Main' + ALid).remove();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    detachedPatLetter: function (PatletterIds) {
        var objData = new Object();
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["TemplateLetterIds"] = PatletterIds;
        objData["commandType"] = "detach_pat_letter_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ClinicalLetterTemplate");
    },
}