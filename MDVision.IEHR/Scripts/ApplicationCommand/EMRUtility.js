EMRUtility = {

    LOINCControlName: null,
    LOINCParams: {},

    AddHistoryTabOnNote: function (ElementName, IsMenu, HistoryParams, btnCtrl) {

        var activeHistory = $(' #pnlClinicalHistorySummary #btnNoteHistoryTabs > ul > li > button.active').attr('title');

        var objDeffered = $.Deferred();

        if (Clinical_ProgressNote.params != null
                       && $("#pnlClinicalProgressNote").css('display') == 'block'
                       && UserRecentAccessedNoteComponent != ""
                       ) {

            Clinical_ProgressNote.getNoteComponentAccess(ElementName).done(function (response) {
                var pervious_component = activeHistory.replace(/\s/g, '');
                Clinical_ProgressNote.remove_UserNoteAccess(true, pervious_component).done(function (res) {
                });
                objDeffered.resolve(response);
            });

        }
        else {
            objDeffered.resolve(true);
        }

        objDeffered.done(function (res) {

            if (res == true) {
                if (activeHistory == "Medical Hx" && Clinical_MedicalHx.medicalHxJSON != '') {
                    var updatedJSON = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").getMyJSONByName();
                    var dataChanged = EMRUtility.compareFormDataWithSerialized(Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx');
                    if (Clinical_MedicalHx.medicalHxJSON != updatedJSON || dataChanged) {
                        var diseaseId = $('#' + Clinical_MedicalHx.params.PanelID + " #ulMedicalDisease > li.active").attr('id');
                        var diseaseData = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").getMyJSONByName();
                        Clinical_MedicalHx.cacheMedicalHxJSON(diseaseId, diseaseData);
                    }
                }
                else if (activeHistory == "Birth Hx") {
                    Clinical_BirthHx.CacheSectionsData(false);
                }
                else if (activeHistory == "Social, Psychological and Behavior Hx") {
                    Clinical_SocPsyandBehaviorHx.cacheSocPsyandBehaviorHxJSON(false);
                }
                else if (activeHistory == "Family Hx" && Clinical_FamilyHx.familyHxJSON != '') {
                    var memberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
                    var diseaseId = $('#' + Clinical_FamilyHx.params.PanelID + " #FamilyDisease ul#ulFamilyHxDisease li.active").attr('id');
                    var updatedJSON = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').getMyJSONByName();
                    //  var dataChanged = EMRUtility.compareFormDataWithSerialized(Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx');

                    var date = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #dtFamilyHxDate").val();
                    var unremarkable = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #chkFamilyHxUnremarkable").prop('checked');
                    var comments = $('#' + Clinical_FamilyHx.params.PanelID + " #txtFamilyOverallComments").val();

                    var dataChanged = Clinical_FamilyHx.familyDate != date || Clinical_FamilyHx.unremarkable != unremarkable || Clinical_FamilyHx.overallComments != comments;

                    if (Clinical_FamilyHx.familyHxJSON != updatedJSON || dataChanged) {
                        Clinical_FamilyHx.cacheFamilyHxJSON(memberId, diseaseId, updatedJSON);
                    }
                }
                if (activeHistory == "Surgical Hx" && Clinical_SurgicalHx.surgicalHxJSON != '') {
                    var updatedJSON = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").getMyJSONByName();
                    var date = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #dtSurgicalHxDate").val();
                    var unremarkable = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").prop('checked');
                    var comments = $('#' + Clinical_SurgicalHx.params.PanelID + " #txtSurgicalOverallComments").val();

                    var dataChanged = Clinical_SurgicalHx.surgicalDate != date || Clinical_SurgicalHx.unremarkable != unremarkable || Clinical_SurgicalHx.overallComments != comments;

                    if (Clinical_SurgicalHx.surgicalHxJSON != updatedJSON || dataChanged) {
                        var diseaseId = $('#' + Clinical_SurgicalHx.params.PanelID + " #ulSurgicalDisease > li.active").attr('id');
                        var diseaseData = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").getMyJSONByName();
                        Clinical_SurgicalHx.cacheSurgicalHxJSON(diseaseId, diseaseData);
                    }
                }
                if (activeHistory == "Hospitalization Hx" && Clinical_HospitalizationHx.hospitalizationHxJSON != '') {
                    var updatedJSON = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName();

                    var date = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #dtHospitalizationHxDate").val();
                    var unremarkable = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #chkHospitalizationHxUnremarkable").prop('checked');
                    var comments = $('#' + Clinical_HospitalizationHx.params.PanelID + " #txtHospitalizationOverallComments").val();

                    var dataChanged = Clinical_HospitalizationHx.hospitalizationDate != date || Clinical_HospitalizationHx.unremarkable != unremarkable || Clinical_HospitalizationHx.overallComments != comments;

                    if (Clinical_HospitalizationHx.hospitalizationHxJSON != updatedJSON || dataChanged) {
                        var diseaseId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease > li.active").attr('id');
                        var diseaseData = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName();
                        Clinical_HospitalizationHx.cacheHospitalizationHxJSON(diseaseId, diseaseData);
                    }
                }
                if (activeHistory == "Social Hx" && Clinical_SocialHx.socialHxJSON != '') {
                    var activeTab = $('#' + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems > li.active > a").text();
                    Clinical_SocialHx.cachePrevTabData(activeTab.toLowerCase());
                }

                var btnParentDiv = $(btnCtrl).closest('ul');
                $.each(btnParentDiv.find("button"), function (index, item) {
                    if ($(item).attr("id") == $(btnCtrl).attr("id")) {
                        $(item).addClass("active");
                    }
                    else {
                        $(item).removeClass("active");
                    }
                });
                var cmd = [];

                ElementName = ElementName.trim();
                ElementName = ElementName.charAt(0).toUpperCase() + ElementName.slice(1);
                ElementName == "Question_group" ? ElementName = "Question_Group" : ElementName = ElementName;
                cmd.TabID = "noteTab" + ElementName;
                cmd.PanelID = "pnlClinical" + ElementName;//panal id from opening html control
                cmd.MasterTabID = "mstrTabNotes";
                cmd.ParentTabID = "mstrTabNotes";

                if (ElementName == "SocialHx" || ElementName == "HistorySummary" || ElementName == "BirthHx" || ElementName == "MedicalHx" || ElementName == "FamilyHx" || ElementName == "SurgicalHx" || ElementName == "HospitalizationHx" || ElementName == "SocPsyandBehaviorHx") {
                    cmd.ParentChildTabID = "clinicalMenuHistroy";

                    cmd.ContainerControlID = "Clinical_" + ElementName;
                    cmd.Selected = true;
                    cmd.MasterTabID = "mstrTabNotes";
                    cmd.Container = "actionPanClinicalProgressNote #divViewHistorySummary";// in which container it will be open
                    cmd.Path = "./EMR/HTML/Clinical/History/Clinical_" + ElementName + ".html";
                    cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
                }


                var Tab = GetTab(cmd.TabID);
                var ClinicalTab = cmd;


                var html = utility.getTabHtml(cmd.TabID);
                if (html) {
                    var dfd = new $.Deferred();
                    dfd.resolve(html);
                    if (cmd.Container) {
                        $("#" + cmd.Container).html(html);
                        $("#" + cmd.Container).find('#formpanelheading').hide();
                        $("#" + cmd.Container).find('#formpanelheading').parent().removeClass('panel-featured')
                        eval(cmd.ContainerControlID + '.Load')(HistoryParams);
                        // SelectCurrentTab(cmd.TabID, IsMenu);
                    }
                    return dfd.promise();
                }
                else {
                    var ajax_get = $.get(cmd.Path, {
                        cache: false
                    }, function (content) {

                        var myDiv = $("<div></div>").append(content);


                        html = String(myDiv.first("div").html());

                        if (cmd.Container) {

                            var socialHx = $("#" + cmd.Container).find('div#pnlClinicalSocialHx').length;
                            if (socialHx > 0) {
                                if (ElementName != 'SocialHx') {
                                    $("#" + cmd.Container).find('div#pnlClinicalSocialHx').css('display', 'none');
                                }
                                else {
                                    $("#" + cmd.Container).find('div#pnlClinicalSocialHx').remove();
                                }
                            }
                            var SocPsyandBehaviorHx = $("#" + cmd.Container).find('div#pnlClinicalSocPsyandBehaviorHx').length;
                            if (SocPsyandBehaviorHx > 0) {
                                if (ElementName != 'SocPsyandBehaviorHx') {
                                    $("#" + cmd.Container).find('div#pnlClinicalSocPsyandBehaviorHx').css('display', 'none');
                                }
                                else {
                                    $("#" + cmd.Container).find('div#pnlClinicalSocPsyandBehaviorHx').remove();
                                }
                            }
                            var medicalHx = $("#" + cmd.Container).find('div#pnlClinicalMedicalHx').length;
                            if (medicalHx > 0) {
                                if (ElementName != 'MedicalHx') {
                                    $("#" + cmd.Container).find('div#pnlClinicalMedicalHx').css('display', 'none');
                                }
                                else {
                                    $("#" + cmd.Container).find('div#pnlClinicalMedicalHx').remove();
                                }
                            }

                            var familyHx = $("#" + cmd.Container).find('div#pnlClinicalFamilyHx').length;
                            if (familyHx > 0) {
                                if (ElementName != 'FamilyHx') {
                                    $("#" + cmd.Container).find('div#pnlClinicalFamilyHx').css('display', 'none');
                                }
                                else {
                                    $("#" + cmd.Container).find('div#pnlClinicalFamilyHx').remove();
                                }
                            }

                            var surgicalHx = $("#" + cmd.Container).find('div#pnlClinicalSurgicalHx').length;
                            if (surgicalHx > 0) {
                                if (ElementName != 'SurgicalHx') {
                                    $("#" + cmd.Container).find('div#pnlClinicalSurgicalHx').css('display', 'none');
                                }
                                else {
                                    $("#" + cmd.Container).find('div#pnlClinicalSurgicalHx').remove();
                                }
                            }

                            var birthHx = $("#" + cmd.Container).find('div#pnlClinicalBirthHx').length;
                            if (birthHx > 0) {
                                if (ElementName != 'BirthHx') {
                                    $("#" + cmd.Container).find('div#pnlClinicalBirthHx').css('display', 'none');
                                }
                                else {
                                    $("#" + cmd.Container).find('div#pnlClinicalBirthHx').remove();
                                }
                            }

                            var hospitalizationHx = $("#" + cmd.Container).find('div#pnlClinicalHospitalizationHx').length;
                            if (hospitalizationHx > 0) {
                                if (ElementName != 'HospitalizationHx') {
                                    $("#" + cmd.Container).find('div#pnlClinicalHospitalizationHx').css('display', 'none');
                                }
                                else {
                                    $("#" + cmd.Container).find('div#pnlClinicalHospitalizationHx').remove();
                                }
                            }

                            $("#" + cmd.Container).find('section:first').css('display', 'none');
                            $("#" + cmd.Container).append(html);
                            // $("#" + cmd.Container).empty().append(html);
                            $("#" + cmd.Container).find('#formpanelheading').hide();
                            $("#" + cmd.Container).find('#formpanelheading').parent().removeClass('panel-featured')
                            eval(cmd.ContainerControlID + '.bIsFirstLoad = true');

                            var params = [];
                            $.extend(params, HistoryParams);
                            if (params.PanelID.indexOf("#") > -1) {
                                params.PanelID = params.ActionPanContainer.replace("actionPan", "pnl");
                            }
                            eval(cmd.ContainerControlID + '.Load')(params);

                            //if (HistoryParams.PanelID.indexOf("#") > -1) {
                            //    HistoryParams.PanelID = HistoryParams.ActionPanContainer.replace("actionPan", "pnl");
                            //}
                            //eval(cmd.ContainerControlID + '.Load')(HistoryParams);
                        }
                    }, "html");
                }
            }
        });



        // Register and Remove User access for History Summary Components IMP-724
        if (Clinical_ProgressNote.params != null
                       && $("#pnlClinicalProgressNote").css('display') == 'block'
                       && UserRecentAccessedNoteComponent != ""
                       ) {
            Clinical_ProgressNote.remove_UserNoteAccess(true, activeHistory).done(function (res) {
                Clinical_ProgressNote.registerNoteComponentAccess(ElementName);
                UserRecentAccessedNoteComponent = ElementName;
            });

        }




    },

    //Author Name: Muhammad Arshad
    //Created Date: 01-09-2016
    //Description: This function will validate the special characters
    validateSpecialCharacters: function (event) {
        var valid = (event.which >= 48 && event.which <= 57) || (event.which >= 65 && event.which <= 90) || (event.which >= 97 && event.which <= 122);
        if (!valid) {
            event.preventDefault();
        }

    },


    //Begin 21-09-2016 Added By Abid Ali Bug#QAC1-27
    ValidateLength: function (obj, event, length) {

        if ($(obj).val().length > length) {
            $(obj).val($(obj).val().substr(0, length));
            return false;
        }
        return true;
    },
    //End 21-09-2016 Added By Abid Ali Bug#QAC1-27

    //Author Name: Muhammad Arshad
    //Created Date: 22-08-2016
    //Description: Loads Attachments for RefModuleName from ParentCtrl through CommandType against PatientId for specific TransitionId to be displayed in given ul ctrl
    loadAttachments: function (ParentCtrl, RefModuleName, CommandType, PatientId, TransitionId, ctrlMenuViewAttachment) {

        EMRUtility.loadAttachments_DbCall(RefModuleName, CommandType, PatientId, TransitionId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var ulAttachment = $(ctrlMenuViewAttachment);
                $(ulAttachment).empty();
                if (RefModuleName == "Lab Result") {
                    if (response.OrderResultAttachmentCount > 0) {
                        var attachments = JSON.parse(response.OrderResultAttachmentLoad_JSON);
                        var containerControlId = "ClinicalLabResultDetail";
                        if (ParentCtrl != null && ParentCtrl == "mstrTabDashBoard") {
                            containerControlId = "DashBoard";
                        }
                        var count = 1;
                        $(attachments).each(function (index, item) {
                            //if (ParentCtrl != null) {
                            //    $(ulAttachment).append('<li><a href="#" id="' + item.PatDocId + '" onclick="ClinicalLabResultDetail.showAttachment(\'' + item.PatDocId + '\',event)">' + item.ModifiedOn + '</a></li>');
                            //}
                            //else {

                            //}
                            $(ulAttachment).append('<li><a href="#" DocCount="' + count + '" id="' + item.PatDocId + '" onclick="Clinical_LabOrder.showAttachment(\'' + ParentCtrl + '\',\'' + PatientId + '\',\'' + item.PatDocId + '\',event,this)">' + item.ModifiedOn + '</a></li>');
                            count++;
                        });

                    }
                    else {
                        $(ulAttachment).append('<li><a href="#">No Attachment Found</a></li>');
                    }
                }


            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    //Author Name: Muhammad Arshad
    //Created Date: 22-08-2016
    //Description: shows Attachments from ParentCtrl against PatientId for specific PatDocID
    showAttachment: function (ParentCtrl, PatientId, PatDocID, event, obj) {
        var strMessage = "";
        $(obj).attr('class', 'active');
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientID"] = PatientId;//$('#PatientProfile #hfPatientId').val();
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                //if ($('#' + ParentCtrl).length > 0)
                //    params["ParentCtrl"] = ParentCtrl;//'mstrTabDashBoard';
                //else
                //{
                //    if (ParentCtrl.indexOf("Tab") > -1) {

                //        params["ParentCtrl"] = ParentCtrl;
                //    }
                //    params["ParentCtrl"] = "Clinical_LabOrder"
                //}
                params["ParentCtrl"] = ParentCtrl;


                LoadActionPan('Document_Viewer', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    //Author Name: Muhammad Arshad
    //Created Date: 22-08-2016
    //Description: DbCall to Loads Attachments for RefModuleName through CommandType against PatientId for specific TransitionId
    loadAttachments_DbCall: function (RefModuleName, CommandType, PatientId, TransitionId) {

        var objData = {};

        objData["TranisitionId"] = TransitionId;//$('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val();
        objData["RefModuleName"] = RefModuleName;//"Lab Result";
        objData["PatientId"] = PatientId;//$("div#PatientProfile #hfPatientId").val();

        objData["commandType"] = CommandType;//"load_attachments";
        var data = JSON.stringify(objData);
        var moduleName = "";
        var subModuleName = "";
        if (RefModuleName == "Lab Result") {
            moduleName = "LabResult";
            subModuleName = "LabResult";
        }
        return MDVisionService.APIService(data, moduleName, subModuleName);
    },

    //Author Name: Muhammad Arshad
    //Created Date: 22-08-2016
    //Description: open Document Import Screen from ParentCtrl for given Module Name (RefModuleName)
    documentImport: function (ParentCtrl, RefModuleName, PatientId, PatientName, TransitionId, AccountNumber) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];

                //var AccountNo = $("#PatientProfile #hfAccountNo").val();
                //var PatientFullName = $("#PatientProfile #hfPatientFullName").val();
                //var PatientId = $("#PatientProfile #hfPatientId").val();
                //var PatientName = "";
                if (PatientName.length > 0) {
                    var Firstname = PatientName.split(" ")[1];
                    var Lastname = PatientName.split(" ")[0];
                    var MiddleInitial = '';
                    try { MiddleInitial = PatientName.split(" ")[2]; } catch (ex) {
                        console.log(ex);
                    }

                    PatientName = Lastname + " " + Firstname + " " + MiddleInitial;
                }
                params["patientId"] = PatientId;
                params["RefCtrl"] = "LabResult";
                if (RefModuleName == "Lab Result") {
                    params['LabResultId'] = TransitionId;
                }
                params['RefModuleName'] = "Lab Result";
                params['TransitionId'] = TransitionId;
                params["FromAdmin"] = "0";
                params["mode"] = "Add";
                params["AccountNumber"] = AccountNumber;

                if ($('#' + ParentCtrl).length > 0)
                    params["ParentCtrl"] = ParentCtrl;//'mstrTabDashBoard';
                else
                    params["ParentCtrl"] = "Clinical_LabOrder"

                params["PatientName"] = PatientName;//ClinicalLabResultDetail.params.PatientName;
                if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote" && ParentCtrl != "mstrTabDashBoard")
                    LoadActionPan('Document_Import', params, "pnlClinicalProgressNote");

                else
                    LoadActionPan('Document_Import', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });


    },

    //Author Name: Muhammad Arshad
    //Created Date: 22-08-2016
    //Description: open Document Scan Screen from ParentCtrl for given Module Name (RefModuleName)
    documentScan: function (ParentCtrl, RefModuleName, PatientId, PatientName, TransitionId) {
        AppPrivileges.GetFormPrivileges("Documents", "SCAN", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var param = [];
                param["mode"] = "Scan";
                param["RefCtrl"] = "LabResult";
                if (RefModuleName == "Lab Result") {
                    param['LabResultId'] = TransitionId;
                }
                param['RefModuleName'] = RefModuleName;//"Lab Result";
                param['TransitionId'] = TransitionId;
                param['patientID'] = PatientId;
                if ($('#' + ParentCtrl).length > 0)
                    params["ParentCtrl"] = ParentCtrl;//'mstrTabDashBoard';
                else
                    params["ParentCtrl"] = "Clinical_LabOrder"
                LoadActionPan('Document_Scan', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },

    //Author Name: Muhammad Arshad
    //Created Date: 22-08-2016
    //Description: shows Attachments
    viewHL7PDF: function (ParentCtrl, PatientId, LabOrderId, LabResultId) {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = PatientId;//$('#PatientProfile #hfPatientId').val();
        //Start 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1023
        params["ParentCtrl"] = ParentCtrl;//"ClinicalLabResultDetail";
        //End 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1023
        params["LabOrderId"] = LabOrderId;
        params["LabResultId"] = LabResultId;
        params["Caller"] = "viewpdf";
        LoadActionPan('Clinical_LabResultView', params);

    },
    GetIEVersion: function () {
        var sAgent = window.navigator.userAgent;
        var Idx = sAgent.indexOf("MSIE");

        // If IE, return version number.
        if (Idx > 0)
            return parseInt(sAgent.substring(Idx + 5, sAgent.indexOf(".", Idx)));

            // If IE 11 then look for Updated user agent string.
        else if (!!navigator.userAgent.match(/Trident\/7\./))
            return 11;

        else
            return 0; //It is not IE
    },

    checkCurrentUserType: function (TypeColletion, UserLastName, UserFirstName) {

        var myPrv = $.grep(TypeColletion, function (item, index) {
            return item.value.trim().toLowerCase() == UserLastName.trim().toLowerCase() + ", " + UserFirstName.trim().toLowerCase();
        });
        if (myPrv.length > 0) {
            return myPrv[0];
        }
        else {
            return null;
        }

    },

    //Author: Muhammad Azhar Hussain
    //Verified By: Muhammad Arshad
    //Date :  26-07-2016
    //Fix Favorites Section Style using class toggleHorDefault
    setFavoriteSectionStyle: function (panelID) {
        $("#" + panelID + " .toggleHorDefault").each(function (i) {
            $(this).parent().addClass("p-none");
            $(this).prepend('<span class="toggleHorIcon"><i class="fa fa-angle-left"></i><i class="fa fa-angle-right"></i></span> <span class="toggleHorButton" onClick="EMRUtility.toggleHorButton(this);"></span>');
        });
        $("#" + panelID + " .toggleHorButton").mouseover(function (e) {
            $(this).parent();
            $(this).parent().addClass("toggleHover");
        });
        $("#" + panelID + " .toggleHorButton").mouseleave(function (e) {
            $(this).parent();
            $(this).parent().removeClass("toggleHover");
        });

    },
    //for favorities clik
    toggleHorButton: function (e) {
        var $parent = $(e).parent();
        $parent.toggleClass("toggledHor");
        $parent.next().toggleClass("toggleHorContainer");
        var $height = $parent.parent().outerHeight();
        $parent.css('height', ($height - 1) + 'px');
    },

    //Author: Muhammad Arshad
    //Date :  27-05-2016
    //Fix duplicate of Data Table in whole EMR on basis of given Grid Id
    fixDataTableDuplication: function (gridPanelId) {
        if (gridPanelId != null) {
            var tableDivs = $(gridPanelId).find("div[id*=_wrapper]");
            var duplicatedTablesCount = tableDivs.length;
            if (duplicatedTablesCount > 1) {
                var OriginalTable = tableDivs.last();
                tableDivs.first().html(OriginalTable);
            }
        }
    },

    //Author: Muhammad Arshad
    //Date :  11-04-2016
    //Show Activity Log for specific Module
    showCurrentItemHistory: function (PanelID, VisitId, ColumnKeyId, DBTableNames, PatientId, TabId, ParentKeyColumnId, ParentCtrlPanelID) {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("ERA Adjustment Codes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        // if (strMessage == "") {
        var params = [];
        params["PanelID"] = PanelID;
        params["VisitId"] = VisitId;
        params["ColumnKeyId"] = ColumnKeyId;
        params["DBTableName"] = DBTableNames;
        if (ParentKeyColumnId != null && ParentKeyColumnId > 0) {
            params["patientID"] = null;
            if (DBTableNames == "RadiologyOrderResult,RadiologyOrderResultDetail" || DBTableNames == "LabOrderResult,LabOrderResultDetail") {
                params["patientID"] = PatientId;
            }
            //If this ColumnKeyId has some child, then ParentKeyColumnId=ColumnKeyId should be specified otherwise no history will be fetched :(
            params['ParentKeyColumnId'] = ParentKeyColumnId;
        }
        else {
            params["patientID"] = PatientId;
            params['ParentKeyColumnId'] = null;
        }

        params['ParentCtrl'] = TabId;
        params['ActivityLogType'] = "CurrentItemActivityLog";

        //Modified by Abid for opening in notes
        if (ParentCtrlPanelID != null) {


            params['ParentCtrlPanelID'] = ParentCtrlPanelID;
            LoadActionPan('Activity_Log', params, ParentCtrlPanelID);
        }
        else {
            LoadActionPan('Activity_Log', params);
        }
        // }
        // else
        //    utility.DisplayMessages(strMessage, 2);
        // });
    },

    appendPrevNext_NotesComponent_Btns: function (PanelId, ParentComponent, CurrentComponent, unloadFunc, FormId, IsDelayLoad) {
        var PrevComponent = '';
        var NextComponent = '';
        $('#NotesComponentList li:not(.nav-parent)').each(function (item, elem) {
            var compareelement = 'clinicalMenu' + ((ParentComponent != '' && ParentComponent != null) ? '_' + ParentComponent : "") +
                ((CurrentComponent != '' && CurrentComponent != null) ? '_' + CurrentComponent : "");
            if (elem.id == compareelement) {
                var PrevObj = $($('#NotesComponentList li:not(.nav-parent)')[item - 1]);
                var NextObj = $($('#NotesComponentList li:not(.nav-parent)')[item + 1]);
                /*
                    Reason:Medication and Prescription is on same form, so I have made this check, so form should not unload and load again for same navigations
                    Change Implemented by: Muhammad Azhar Shahzad
                    Date: Januaray 21, 2016
                */
                if (CurrentComponent == 'Prescription' && PrevObj.text() == 'Medications') {
                    PrevComponent = PrevObj.text();
                    PrevComponentOnclick = 'Clinical_Medications.showMedicationPrescriptionTab(\'Medications\');' + "EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_Medications.params.PanelID, 'Medical', 'Medications', 'Clinical_Medications.unLoadTab();');";
                } else if (PrevObj.length > 0) {

                    PrevComponent = PrevObj.text();
                    //Start 16-01-2017 Humaira Yousaf for IMP-224
                    if (PrevComponent == "Lab" || PrevComponent == "Problems") {
                        if (PrevComponent == "Lab") {
                            PrevComponent = "LabOrders";
                        }
                        if (CurrentComponent == "Prescription") {
                            CurrentComponent = "Medications";
                        }
                    }
                    //End 16-01-2017 Humaira Yousaf for IMP-224
                    // Added by Abid Ali
                    if (IsDelayLoad) {
                        if (CurrentComponent == "Radiology") {
                            CurrentComponent = "ClinicalRadiologyResultDetail";
                            unloadFuncPrev = "ClinicalRadiologyResultDetail" + ".unLoadTab(true,'" + PrevComponent.replace(" ", "") + "')";
                        }
                        else {
                            unloadFuncPrev = "Clinical_" + CurrentComponent + ".unLoadTab(true,'" + PrevComponent.replace(" ", "") + "')";
                        }
                        PrevComponentOnclick = 'setTimeout(function(){' + unloadFuncPrev + '},400)';
                    }
                    else {
                        if (PrevComponent == "Functional And Cognitive") {
                            PrevComponentOnclick = 'setTimeout(function(){' + unloadFunc + "setTimeout(function(){ " + (PrevObj.find('a').attr('href') == '#' ? '' : PrevObj.find('a').attr('href').replace('javascript:', '')) + '},600);},400)';
                        }
                        else {
                            PrevComponentOnclick = 'setTimeout(function(){' + unloadFunc + "setTimeout(function(){ " + (PrevObj.find('a').attr('href') == '#' ? '' : PrevObj.find('a').attr('href').replace('javascript:', '').replace(';', '')) + ';},600);},400)';
                        }
                    }
                    if (PrevComponentOnclick.indexOf("ClinicalRadiologyResultDetail.unLoadTab" > -1)) {
                        PrevComponentOnclick = PrevComponentOnclick.replace("ClinicalRadiologyResultDetail.unLoadTab", "ClinicalRadiologyResultDetail.UnLoad");
                    }
                }
                /*
                  Reason:Medication and Prescription is on same form, so I have made this check, so form should not unload and load again for same navigations
                  Change Implemented by: Muhammad Azhar Shahzad
                  Date: Januaray 21, 2016
              */
                if (CurrentComponent == 'Medications' && NextObj.text() == 'Prescription') {
                    NextComponent = NextObj.text();
                    NextComponentOnclick = 'Clinical_Medications.showMedicationPrescriptionTab(\'Prescription\');' + "EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_Medications.params.PanelID, 'Medical', 'Prescription', 'Clinical_Medications.unLoadTab();');";
                } else
                    if (NextObj.length > 0) {

                        NextComponent = NextObj.text();
                        //Start 16-01-2017 Humaira Yousaf for IMP-224
                        if (NextComponent == "Lab" || NextComponent == "Problems") {
                            if (NextComponent == "Lab") {
                                NextComponent = "LabOrders";
                            }
                            if (CurrentComponent == "Prescription") {
                                CurrentComponent = "Medications";
                            }
                        }
                        //End 16-01-2017 Humaira Yousaf for IMP-224
                        // Added by Abid Ali
                        if (IsDelayLoad) {
                            if (CurrentComponent == "Radiology") {
                                CurrentComponent = "ClinicalRadiologyResultDetail";
                                unloadFuncNext = "ClinicalRadiologyResultDetail" + ".unLoadTab(true,'" + NextComponent.replace(" ", "") + "')";
                            }
                            else {
                                unloadFuncNext = "Clinical_" + CurrentComponent + ".unLoadTab(true,'" + NextComponent.replace(" ", "") + "')";
                            }
                            NextComponentOnclick = 'setTimeout(function(){' + unloadFuncNext + '},400)';
                        }
                        else {
                            if (NextComponent == "Functional And Cognitive") {
                                NextComponentOnclick = 'setTimeout(function(){' + unloadFunc + "setTimeout(function(){ " + (NextObj.find('a').attr('href') == '#' ? '' : NextObj.find('a').attr('href').replace('javascript:', '')) + '},600);},400)';
                            }
                            else {
                                NextComponentOnclick = 'setTimeout(function(){' + unloadFunc + "setTimeout(function(){ " + (NextObj.find('a').attr('href') == '#' ? '' : NextObj.find('a').attr('href').replace('javascript:', '').replace(';', '')) + ';},600);},400)';
                            }
                        }
                        if (NextComponentOnclick.indexOf("Clinical_ClinicalRadiologyResultDetail.unLoadTab") > -1) {
                            NextComponentOnclick = NextComponentOnclick.replace("Clinical_ClinicalRadiologyResultDetail.unLoadTab", "ClinicalRadiologyResultDetail.UnLoad");
                        }
                    }
            }

        });
        //Start 16-01-2017 Humaira Yousaf for IMP-224
        if (NextComponent == "LabOrders") {
            NextComponent = "Lab";
        }
        //End 16-01-2017 Humaira Yousaf for IMP-224
        $('#' + PanelId + ' .modal-footer').html('<div class="btn-block">' +
               ((PrevComponent == '' || PrevComponent == null) ? '' : ' <button class="btn btn-primary btn-sm pull-left" type="button" id="btnPrevC" onclick="EMRUtility.addToNotefunc(\'' + PanelId + '\',\'' + FormId + '\');' + PrevComponentOnclick + '">' + PrevComponent + '</button>') +
        ((NextComponent == '' || NextComponent == null) ? '' : ' <button class="btn btn-primary btn-sm pull-right" type="button" id="btnNextC" onclick="EMRUtility.addToNotefunc(\'' + PanelId + '\',\'' + FormId + '\');' + NextComponentOnclick + '">' + NextComponent + '</button>') +
          ' </div>');
    },

    addToNotefunc: function (PanelId, FormId) {
        if (FormId != null && $('#' + PanelId + ' #' + FormId).is(":not(:disabled)")) {
            //this code is commented temporary to discuss business logic with BA (zain) | empty form should allowed to be inserted or not
            //     $('#' + PanelId + ' #' + FormId).trigger('submit');
        }
    },

    buildAllergyLi: function (lookupId, allergyName, lookupId) {
        var currId = -1;
        $("#ClinicalCDSDetail #frmClinicalCDSDetail ul#ulCDSAllergies li[id*='-']").each(function (i, item) {

            currId = $(this).attr("id");

        });
        currId = parseInt(currId) + (-1);
        //Start//16-03-2016//Ahmad Raza//logic to add AllergyOperator dropdown
        if ($('#ClinicalCDSDetail #ulCDSAllergies li:first').length > 0) {
            var li = "<li AllergenId='" + lookupId + "'  id=" + lookupId + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'><select id='ddlAllergies" + lookupId + "' name = 'CDSAllergies" + lookupId + "' class='form-control'><option value='OR'>OR</option><option value='AND'>AND</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + allergyName + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteAllergy($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
        }
        else {
            li = "<li AllergenId='" + lookupId + "' id=" + lookupId + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'></div><div class='col-sm-8 col-lg-10'><a href='#'>" + allergyName + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteAllergy($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
        }
        //End//16-03-2016//Ahmad Raza//logic to add AllergyOperator dropdown
        var IsAlreadyExist = false;
        $('#ClinicalCDSDetail #ulCDSAllergies li').each(function () {
            if ($(this).attr('id') == lookupId) {

                IsAlreadyExist = true;
            }
        });

        if (!IsAlreadyExist) {
            $('#ClinicalCDSDetail #ulCDSAllergies').append(li);
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#ClinicalCDSDetail #txtCDSAllergies').val('');
            ////    Clinical_Complaints.AddInArray(currId, icd10Description, true);
            //if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab)
            //    Admin_IMOICD.UnLoadTab();

            var isUnload = "false";
            var txt = $('#ClinicalCDSDetail #txtCDSAllergies');
            if (txt.is('[data-popupunload]')) {
                isUnload = txt.attr('data-popupunload');
            }

            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                txt.attr("data-popupunload", "false");
                Admin_IMOICD.UnLoadTab();
            }
        }
        else {
            utility.DisplayMessages('Allergy already added', 2);

            $('#ClinicalCDSDetail #txtCDSAllergies').val('');
        }
    },
    buildMedicationLi: function (DrugId, medicationName, rxnormId) {

        var currId = -1;
        $("#ClinicalCDSDetail #frmClinicalCDSDetail ul#ulCDSMedications li[id*='-']").each(function (i, item) {

            currId = $(this).attr("id");

        });
        //Start 10-03-2016 Humaira Yousaf for operator dropdown
        var li = '';
        currId = parseInt(currId) + (-1);
        if ($('#ClinicalCDSDetail #ulCDSMedications li:first').length > 0) {
            li = "<li id=" + DrugId + " rxnormid =" + rxnormId + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'><select id='ddlMedications" + DrugId + "' name = 'CDSMedications" + DrugId + "' class='form-control'><option value='OR'>OR</option><option value='AND'>AND</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + medicationName + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteMedication($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
        }
        else {
            li = "<li id=" + DrugId + " rxnormid =" + rxnormId + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'></div><div class='col-sm-8 col-lg-10'><a href='#'>" + medicationName + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteMedication($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
        }
        //End 10-03-2016 Humaira Yousaf for operator dropdown

        var IsAlreadyExist = false;
        $('#ClinicalCDSDetail #ulCDSMedications li').each(function () {
            if ($(this).attr('id') == DrugId) {

                IsAlreadyExist = true;
            }
        });

        if (!IsAlreadyExist) {
            $('#ClinicalCDSDetail #ulCDSMedications').append(li);
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#ClinicalCDSDetail #frmClinicalCDSDetail #txtCDSMedications').val('');
           // $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSMedications").val('');
            //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

            var isUnload = "false";
            var txt = $('#ClinicalCDSDetail #txtCDSMedications');
            if (txt.is('[data-popupunload]')) {
                isUnload = txt.attr('data-popupunload');
            }

            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                txt.attr("data-popupunload", "false");
                Admin_IMOICD.UnLoadTab();
            }
        }
        else {
            utility.DisplayMessages('Medication already added', 2);

            $('#ClinicalCDSDetail #txtCDSMedications').val('');
        }

    },
    buildLabResultLi: function (icdCodeAndDescription, labResultName) {

        var currId = -1;
        $("#ClinicalCDSDetail #frmClinicalCDSDetail ul#ulCDSLabResults li[id*='-']").each(function (i, item) {

            currId = $(this).attr("id");

        });
        //Start 10-03-2016 Humaira Yousaf for operator dropdown
        var li = '';
        currId = parseInt(currId) + (-1);
        if ($('#ClinicalCDSDetail #ulCDSLabResults li:first').length > 0) {
            li = "<li id=" + icdCodeAndDescription + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'><select id='ddlLabResults" + icdCodeAndDescription + "' name = 'CDSLabResults" + icdCodeAndDescription + "' class='form-control'><option value='OR'>OR</option><option value='AND'>AND</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + labResultName + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteLabResult($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
        }
        else {
            li = "<li id=" + icdCodeAndDescription + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'></div><div class='col-sm-8 col-lg-10'><a href='#'>" + labResultName + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteLabResult($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
        }
        //End 10-03-2016 Humaira Yousaf for operator dropdown

        var IsAlreadyExist = false;
        $('#ClinicalCDSDetail #ulCDSLabResults li').each(function () {
            if ($(this).attr('id') == icdCodeAndDescription) {

                IsAlreadyExist = true;
            }
        });

        if (!IsAlreadyExist) {
            $('#ClinicalCDSDetail #ulCDSLabResults').append(li);
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#ClinicalCDSDetail #txtCDSLabResults').val('');
            //     Clinical_Complaints.AddInArray(currId, icd10Description, true);
            var isUnload = "false";
            var txt = $('#ClinicalCDSDetail #txtCDSLabResults');
            if (txt.is('[data-popupunload]')) {
                isUnload = txt.attr('data-popupunload');
            }

            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                txt.attr("data-popupunload", "false");
                Admin_IMOICD.UnLoadTab();
            }
        }
        else {
            utility.DisplayMessages('Lab Result already added', 2);

            $('#ClinicalCDSDetail #txtCDSLabResults').val('');
        }

    },

    GetMedicationsFromDrFirst: function (Crtl) {
        var Med = $(Crtl).val();
        var AllDrug = [];
        var IsValid = false;
        if (Med != null && Med.length > 1)
            IsValid = true;
        else
            IsValid = false;
        var dfd = new $.Deferred();
        if (IsValid) {
            var objData = new Object();
            objData["DrugName"] = Med;
            objData["commandType"] = "Get_Drug_Array";
            var data = JSON.stringify(objData);
            MDVisionService.APIService(data, "OrderSet", "MEDICATION").done(function (responseData) {
                if (responseData.status != false) {
                    responseData = JSON.parse(responseData)
                    var DrugsData = JSON.parse(responseData.DrugsData);
                    if (DrugsData.length == 0)
                        utility.DisplayMessages("No Record Found", 2);
                    else {
                        var id = -2;
                        $.each(DrugsData, function (i, item) {
                            AllDrug.push({ id: id, value: item.BrandName + " " + item.Strength, NDCID: item.NDCID, BrandName: item.BrandName, GenericName: item.GenericName, Form: item.Form, Strength: item.Strength });
                            id--;
                        });
                    }
                }
                dfd.resolve(AllDrug);
            });
        }
        else {
            utility.DisplayMessages("Please enter minimum  2 characters in the search field", 2);
            dfd.resolve(AllDrug);
        }
        return dfd.promise();
    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: CDS Detail Allergies Search
    bindAutoCompleteAllergies: function (Crtl) {
        BackgroundLoaderShow(false);
        var AllData = [];
        var text = $(Crtl).val();
        if (true) {

            BackgroundLoaderShow(true);
            var objData = new Object();
            objData["commandType"] = "search_allergy_for_cds";
            var data = JSON.stringify(objData);

            //   var data = "text=" + text + "&entityID=" + entityID
            // serach parameter , class name, command name of class
            MDVisionService.APIService(data, "MEDICAL", "Allergy").done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    AllData = [];
                    if (response.allergiesLoad_JSON != null) {
                        var objResponse = JSON.parse(response.allergiesLoad_JSON);
                        $.each(objResponse, function (j, item) {
                            if (item.Name.toUpperCase() != "- SELECT -")
                                //var rxNormId = item.RxnormID != "" ? item.RxnormID + " - " : "";
                                var found_names = $.grep(AllData, function (v) {
                                    return v.Name === item.Name;
                                });
                            if (found_names.length < 1) {
                                AllData.push({ id: item.LookupId, value: item.Name, Name: item.Name, RefValue: item.LookupId });
                            }

                        });

                        $(Crtl).autocomplete({
                            autoFocus: true,
                            source: AllData,

                            select: function (event, ui) {
                                BackgroundLoaderShow(false);
                                setTimeout(function () {
                                    $(Crtl).val(ui.item.value);
                                    ParentControl = "ClinicalCDSDetail";
                                    EMRUtility.buildAllergyLi(ui.item.id, ui.item.value, ui.item.RefValue);
                                    //SupperBillDetail.OpenIMODetail(ui.item.value, ui.item.id, ParentControl, "", "", text + $(Crtl).selector.split('#')[2]);
                                }, 10);
                                //$(Crtl).text(ui.item.RefValue);
                            }
                        });

                        BackgroundLoaderShow(false);
                    }
                }

            });
        }
    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: CDS Detail Medications Search
    bindAutoCompleteMedications: function (Crtl) {
        BackgroundLoaderShow(false);
        var AllData = [];
        var text = $(Crtl).val();
        if (true) {
            BackgroundLoaderShow(true);
            var objData = new Object();
            objData["isFromCDS"] = true;
            objData["commandType"] = "search_medications_for_cds";
            var data = JSON.stringify(objData);

            //   var data = "text=" + text + "&entityID=" + entityID
            // serach parameter , class name, command name of class
            MDVisionService.APIService(data, "MEDICAL", "Medications").done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    AllData = [];
                    if (response.MedicationLoad_JSON != null) {
                        var objResponse = JSON.parse(response.MedicationLoad_JSON);

                        $.each(objResponse, function (j, item) {
                            if (item.MedicationName.toUpperCase() != "- SELECT -")
                                //  var rxNormId = item.RxnormID != "" ? item.RxnormID + " - " : "";     //code removed from start by Ahmad Cheema
                                var rxNormId = "";
                            var found_names = $.grep(AllData, function (v) {
                                return v.Name === item.MedicationName;
                            });
                            if (found_names.length < 1) {
                                AllData.push({ id: item.DrugId, value: item.MedicationName, Name: item.MedicationName, rxnormId: item.RxnormID });
                            }
                        });

                        $(Crtl).autocomplete({
                            autoFocus: true,
                            //source: AllCode, // pass an array (without a comma)
                            source: AllData,

                            select: function (event, ui) {
                                BackgroundLoaderShow(false);
                                setTimeout(function () {
                                    $(Crtl).val(ui.item.value);
                                    ParentControl = "ClinicalCDSDetail";
                                    EMRUtility.buildMedicationLi(ui.item.id, ui.item.value, ui.item.rxnormId);
                                    // SupperBillDetail.OpenIMODetail(ui.item.value, ui.item.id, ParentControl, "", "", text + $(Crtl).selector.split('#')[2]);
                                }, 10);
                                //$(Crtl).text(ui.item.RefValue);
                            }
                        });

                        BackgroundLoaderShow(false);
                    }
                }

            });


        }
    },

    //Author: Ahmad Raza
    //Date :  14-05-2016
    //Reason: CDS Detail Lab Results Search
    bindAutoCompleteLabResults: function (Crtl) {
        BackgroundLoaderShow(false);
        var AllData = [];
        var text = $(Crtl).val();
        if (true) {
            BackgroundLoaderShow(true);
            var objData = new Object();
            // objData["isFromCDS"] = true;
            objData["commandType"] = "search_labresults";
            var data = JSON.stringify(objData);

            //   var data = "text=" + text + "&entityID=" + entityID
            // serach parameter , class name, command name of class
            MDVisionService.APIService(data, "LabResult", "LabResult").done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    AllData = [];
                    var objResponse = JSON.parse(response.LabResultDetailFill_JSON);
                    $.each(objResponse, function (j, item) {
                        if (item.LOINCDescription.toUpperCase() != "- SELECT -")
                            var loincId = item.LOINC != "" ? item.LOINC + " - " : "";
                        var found_names = $.grep(AllData, function (v) {
                            return v.Name === item.LOINCDescription;
                        });
                        if (found_names.length < 1) {
                            AllData.push({ id: item.LabOrderResultDetailId, value: loincId + item.LOINCDescription, Name: item.LOINCDescription });
                        }
                    });

                    $(Crtl).autocomplete({
                        autoFocus: true,
                        //source: AllCode, // pass an array (without a comma)
                        source: AllData,

                        select: function (event, ui) {
                            BackgroundLoaderShow(false);
                            setTimeout(function () {
                                $(Crtl).val(ui.item.value);
                                ParentControl = "ClinicalCDSDetail";
                                EMRUtility.buildLabResultLi(ui.item.id, ui.item.value);
                                //SupperBillDetail.OpenIMODetail(ui.item.value, ui.item.id, ParentControl, "", "", text + $(Crtl).selector.split('#')[2]);
                            }, 10);
                            //$(Crtl).text(ui.item.RefValue);
                        }
                    });

                    BackgroundLoaderShow(false);
                }

            });


        }
    },


    BindOrganismCodes: function (element, Ctrl) {
        var controlName = eval(Ctrl);


        var controlId = $(element).attr('id');

        $(element).autocomplete({

            autoFocus: true,
            source: function (request, response) {
                // utility.Keyupdelay(function () {
                var AccountNo = $(element).val();
                if (AccountNo.length > 0) {

                    var entityID = globalAppdata["SeletedEntityId"];
                    var data = "text=" + AccountNo + "&entityID=" + entityID + "&iscode=CPT";

                    Clinical_LOINC.loadLabResultsOrganisms(AccountNo).done(function (responseData) {
                        responseData = JSON.parse(responseData)
                        if (responseData.status != false) {
                            if (responseData.LabResultOrganismCount > 0) {
                                var LabResultOrganismLoadJSONData = JSON.parse(responseData.LabResultOrganismLoad_JSON);
                                var AllOrganism = [];
                                $.each(LabResultOrganismLoadJSONData, function (i, item) {

                                    AllOrganism.push({ id: item.Code, value: item.Code + " " + item.Description, OrganismDescription: item.Description });


                                });
                                response(AllOrganism);
                            }
                            else {
                                if (Ctrl == "Clinical_LabOrder" && parentDivId == "PatientLabOrderSent") {
                                    $(element).val("No record found");
                                }
                            }
                        }
                    });

                }
            },
            select: function (event, ui) {

                setTimeout(function () {

                    var obj = {};

                    obj["Organism"] = ui.item.value;
                    obj['Code'] = ui.item.id;
                    obj['Description'] = ui.item.OrganismDescription;


                    ClinicalLabResultDetail.pushOrganisms(obj, element);

                });
            }
        });
    },

    BindLOINCCodes: function (element, Ctrl, labId, parentDivId, CodeSystemType) {
        var controlName = eval(Ctrl);
        EMRUtility.LOINCParams["Element"] = element;
        EMRUtility.LOINCControlName = Ctrl;

        var controlId = $(element).attr('id');
        if (controlId == "txtCPTCode" && Ctrl == "Clinical_LabOrder") {
            controlId = parentDivId + " #" + controlId;
        }
        //By Ahmad Imran
        if ($(element).parent().parent().parent().attr("id") == "LabOrderDiv") {
            controlId = "ReportParamaters #" + controlId;
        }

        $("#" + controlName.params.PanelID + " #" + controlId).autocomplete({

            autoFocus: true,
            source: function (request, response) {
                // utility.Keyupdelay(function () {
                var AccountNo = $("#" + controlName.params.PanelID + " #" + controlId).val();
                if (AccountNo.length > 0) {
                    if (CodeSystemType == '1') {  // If Lab uses CPT Codes
                        var entityID = globalAppdata["SeletedEntityId"];
                        var data = "text=" + AccountNo + "&entityID=" + entityID + "&iscode=CPT";

                        MDVisionService.defaultService(data, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE').done(function (result) {
                            var AllLOINC = [];
                            var counter_ = 1;
                            $.each(result, function (j, item) {
                                var LexiCode = "", CPT = "", ICD = "", SNOMED = "", _CPT = "", _CPTDescription = "", _ICD = "", _ICDDescription = "", _SNOMED = "", _SNOMEDDescription = "", _LexiCode = "";

                                var _ConcatinatedString = utility.decodeHtml(item.Name);

                                // In IMO case it would be true, IN MDVision Database Case it will fall in 'else' Block
                                if (_ConcatinatedString.indexOf("!") >= 0) {

                                    LexiCode = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("!"));
                                    CPT = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("!") + 1, _ConcatinatedString.lastIndexOf("@"));
                                    ICD = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("@") + 1, _ConcatinatedString.lastIndexOf("#"));
                                    SNOMED = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("#") + 1);
                                    _LexiCode = LexiCode;

                                    var cptIndex = CPT.indexOf('+');
                                    _CPT = CPT.substring(0, cptIndex);
                                    _CPTDescription = CPT.substring((cptIndex + 1), CPT.length);
                                    _ICD = ICD.split("+")[0];
                                    _ICDDescription = ICD.split("+")[1];
                                    _SNOMED = SNOMED.split("+")[0];
                                    _SNOMEDDescription = SNOMED.split("+")[1];
                                }
                                else {
                                    CPT = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("^"));
                                    _CPT = CPT.split("-")[0].trim();
                                    _CPTDescription = CPT.split("-")[1].trim();
                                }
                                _CPTDescription = _CPTDescription.replace(/&quot;/g, '"').replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&#39;/g, "'").replace(/&amp;/g, "&").replace(/\/\equal/g, '=');
                                AllLOINC.push({ id: item.Value, value: _CPT + ' - ' + _CPTDescription, RefValue: item.RefValue, OrderTestLOINCId: counter_ });
                                counter_++;

                            });
                            response(AllLOINC);
                        });
                    }
                    else if (CodeSystemType == '-1') {//get both
                        var entityID = globalAppdata["SeletedEntityId"];
                        var data = "text=" + AccountNo + "&entityID=" + entityID + "&iscode=CPT";

                        MDVisionService.defaultService(data, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE').done(function (result) {
                            var AllLOINC = [];
                            var counter_ = 1;
                            $.each(result, function (j, item) {
                                var LexiCode = "", CPT = "", ICD = "", SNOMED = "", _CPT = "", _CPTDescription = "", _ICD = "", _ICDDescription = "", _SNOMED = "", _SNOMEDDescription = "", _LexiCode = "";

                                var _ConcatinatedString = utility.decodeHtml(item.Name);

                                // In IMO case it would be true, IN MDVision Database Case it will fall in 'else' Block
                                if (_ConcatinatedString.indexOf("!") >= 0) {

                                    LexiCode = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("!"));
                                    CPT = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("!") + 1, _ConcatinatedString.lastIndexOf("@"));
                                    ICD = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("@") + 1, _ConcatinatedString.lastIndexOf("#"));
                                    SNOMED = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("#") + 1);
                                    _LexiCode = LexiCode;

                                    var cptIndex = CPT.indexOf('+');
                                    _CPT = CPT.substring(0, cptIndex);
                                    _CPTDescription = CPT.substring((cptIndex + 1), CPT.length);
                                    _ICD = ICD.split("+")[0];
                                    _ICDDescription = ICD.split("+")[1];
                                    _SNOMED = SNOMED.split("+")[0];
                                    _SNOMEDDescription = SNOMED.split("+")[1];
                                }
                                else {
                                    CPT = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("^"));
                                    _CPT = CPT.split("-")[0].trim();
                                    _CPTDescription = CPT.split("-")[1].trim();
                                }
                                _CPTDescription = _CPTDescription.replace(/&quot;/g, '"').replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&#39;/g, "'").replace(/&amp;/g, "&").replace(/\/\equal/g, '=');
                                AllLOINC.push({ id: item.Value, value: _CPT + ' - ' + _CPTDescription, RefValue: item.RefValue, OrderTestLOINCId: counter_ });
                                counter_++;

                            });

                            Clinical_LOINC.loadLabResultsLOINC(null, AccountNo, labId).done(function (responseData) {
                                responseData = utility.decodeHtml(responseData);
                                responseData = JSON.parse(responseData)
                                if (responseData.status != false) {
                                    if (responseData.LabResultLOINCCount > 0) {
                                        var LabResultLOINCLoadJSONData = JSON.parse(responseData.LabResultLOINCLoad_JSON);
                                        $.each(LabResultLOINCLoadJSONData, function (i, item) {

                                            AllLOINC.push({ id: item.LOINCCode, value: item.LOINCCode + " " + item.LOINCDescription, LOINCDescription: item.LOINCDescription, LabId: item.LabId, UoM: item.UoM, Range: item.Range, IsActive: item.IsActive, SampleStorage: item.SampleStorage, OrderTestLOINCId: item.OrderTestLOINCId });


                                        });
                                        response(AllLOINC);
                                    }
                                    else {
                                        response(AllLOINC);
                                        //comment against bug no #EMR-6516 
                                        //if (Ctrl == "Clinical_LabOrder" && parentDivId == "PatientLabOrderSent") {
                                        //    $("#" + controlName.params.PanelID + " #" + controlId).val("No record found");
                                        //}
                                    }
                                } else {
                                    response(AllLOINC);
                                }
                            });
                        });
                    }
                    else {
                        Clinical_LOINC.loadLabResultsLOINC(null, AccountNo, labId).done(function (responseData) {
                            responseData = utility.decodeHtml(responseData);
                            responseData = JSON.parse(responseData)
                            if (responseData.status != false) {
                                if (responseData.LabResultLOINCCount > 0) {
                                    var LabResultLOINCLoadJSONData = JSON.parse(responseData.LabResultLOINCLoad_JSON);
                                    var AllLOINC = [];
                                    $.each(LabResultLOINCLoadJSONData, function (i, item) {

                                        AllLOINC.push({ id: item.LOINCCode, value: item.LOINCCode + " " + item.LOINCDescription, LOINCDescription: item.LOINCDescription, LabId: item.LabId, UoM: item.UoM, Range: item.Range, IsActive: item.IsActive, SampleStorage: item.SampleStorage, OrderTestLOINCId: item.OrderTestLOINCId });


                                    });
                                    response(AllLOINC);
                                }
                                else {
                                    //comment against bug no #EMR-6516 
                                    //if (Ctrl == "Clinical_LabOrder" && parentDivId == "PatientLabOrderSent") {
                                    //    $("#" + controlName.params.PanelID + " #" + controlId).val("No record found");
                                    //}
                                }
                            }
                        });
                    }
                }

                //});
            },
            select: function (event, ui) {

                setTimeout(function () {

                    var obj = {};
                    if (CodeSystemType == '1') { // in case Lab uses CPT Codes
                        obj["Observation"] = ui.item.value;
                        obj['LOINICCODE'] = ui.item.id;
                        obj['OrderTestLOINCId'] = ui.item.OrderTestLOINCId;
                        descIndex = ui.item.value.indexOf(" - ") + 3;
                        obj['LOINICDescription'] = ui.item.value.substring(descIndex, ui.item.value.length);
                    }
                    else {             // In case Lab uses custom codes
                        obj["Observation"] = ui.item.value;
                        obj['LOINICCODE'] = ui.item.id;
                        obj['OrderTestLOINCId'] = ui.item.OrderTestLOINCId;
                        obj['LOINICDescription'] = ui.item.LOINCDescription;
                    }
                    if (EMRUtility.LOINCControlName.indexOf('Favorite_LabOrderDetail') > -1) {

                        Favorite_LabOrderDetail.pushLOINCAsCpt(obj);
                        $("#" + Favorite_LabOrderDetail.params.PanelID + " #" + controlId).val("");
                    }
                    else if (EMRUtility.LOINCControlName.indexOf('Favorite_RadiologyOrderDetail') > -1) {

                        Favorite_RadiologyOrderDetail.pushLOINCAsCpt(obj);
                        $("#" + Favorite_RadiologyOrderDetail.params.PanelID + " #" + controlId).val("");
                    }
                    else if (EMRUtility.LOINCControlName.indexOf('ClinicalLabOrderDetail') > -1) {

                        if (ui.item.SampleStorage) {
                            obj['SampleStorage'] = ui.item.SampleStorage;
                        }

                        ClinicalLabOrderDetail.pushLOINCAsCpt(obj);
                        $("#" + ClinicalLabOrderDetail.params.PanelID + " #" + controlId).val("");
                    }
                    else if (EMRUtility.LOINCControlName.indexOf('AOETemplateDetail') > -1) {
                        AOETemplateDetail.pushLOINCAsCpt(obj);
                    }
                    else if (EMRUtility.LOINCControlName.indexOf('ProcedureTemplateDetail') > -1) {
                        ProcedureTemplateDetail.pushLOINCAsCpt(obj);
                    }
                    else if (EMRUtility.LOINCControlName.indexOf('ClinicalLabResultDetail') > -1) {

                        ClinicalLabResultDetail.pushLOINCAsCpt(obj);
                        $("#" + ClinicalLabResultDetail.params.PanelID + " #" + controlId).val("");
                    }
                    else if (EMRUtility.LOINCControlName.indexOf('Clinical_LabOrder') > -1) {

                        Clinical_LabOrder.pushLOINCAsCpt(obj, EMRUtility.LOINCParams["Element"]);
                    }
                    else if (EMRUtility.LOINCControlName.indexOf('ClinicalLabTestAttributes') > -1) {

                        ClinicalLabTestAttributes.pushLOINCAsCpt(obj, EMRUtility.LOINCParams["Element"]);
                    }
                    else if (EMRUtility.LOINCControlName.indexOf('OrderSet_LabOrderDetails') > -1) {
                        if (ui.item.SampleStorage) {
                            obj['SampleStorage'] = ui.item.SampleStorage;
                        }
                        OrderSet_LabOrderDetails.pushLOINCAsCpt(obj);
                        $("#" + OrderSet_LabOrderDetails.params.PanelID + " #" + controlId).val("");
                    }
                    else if (EMRUtility.LOINCControlName.indexOf('OrderSet_RadiologyOrderDetails') > -1) {

                        if (ui.item.SampleStorage) {
                            obj['SampleStorage'] = ui.item.SampleStorage;
                        }

                        OrderSet_RadiologyOrderDetails.pushLOINCAsCpt(obj);
                        $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #" + controlId).val("");
                    }
                    else if (EMRUtility.LOINCControlName.indexOf('ClinicalCDSDetail') > -1) {

                        ClinicalCDSDetail.pushLOINCAsCpt(obj);
                        $("#" + ClinicalCDSDetail.params.PanelID + " #" + controlId).val("");
                    }
                }, 100);
            }
        });
    },

    //Author: Abid Ali
    //Date :  08-04-2016
    //Reason: AutoComplete for LookUps
    loadAutoComplete: function (obj, formId, hiddenFieldId, ctrlName, lookupName) {

        var controlName = eval(ctrlName);
        var txtBoxId = $(obj).attr('id');
        var data = [];
        var value = $(obj).val();
        var isValid = false;
        if (value.length > 1) {
            isValid = true;
        }
        if (isValid) {

            CacheManager.BindCodes(lookupName, true).done(function (result) {

                switch (lookupName) {
                    case 'GetLabs':
                        data = Labs;
                        break;
                }
                $('#' + controlName.params.PanelID + " #" + formId + " #" + txtBoxId).autocomplete({
                    autoFocus: true,
                    source: data,
                    select: function (event, ui) {

                        setTimeout(function () {

                            $('#' + controlName.params.PanelID + " #" + formId + " #" + hiddenFieldId).val(ui.item.id);

                        }, 100);
                    }
                });
            });
        }
    },

    //Author: Ahmad Raza
    //Date :  16-12-2015
    //Reason: Method to compare form data with serialized data
    compareFormDataWithSerialized: function (formId) {

        if (formId.indexOf("#") == 0)
            formId = formId;
        else
            formId = "#" + formId;

        var formCtr = $(formId);

        if (formCtr.serialize() != formCtr.data('serialize')) {

            return true;
        }
        else {

            return false;
        }

    },
    //End//16/12/2015//Ahmad Raza//Method to compare form data with serialized data
    setPercentage: function (event, obj) {
        var Cntrlvalue = 0;
        if (obj != null) {
            Cntrlvalue = parseInt($(obj).val());
        }
        if (Cntrlvalue != null && event != null) {
            if (Cntrlvalue == 0) {
                $(obj).val('');
            }
            else if (Cntrlvalue > 100) {
                $(obj).val(100);
            }
            else if (Cntrlvalue == '-') {
                $(obj).val('');
            }
        }
    },

    ValidateHeight: function (event, obj) {

        var Ctrlvalue = "";
        if (obj != null) {
            Ctrlvalue = $(obj).val();
        }

        if (Ctrlvalue.indexOf('.') != -1) {

            var arr = Ctrlvalue.split('.');

            if (arr.length > 0) {
                var inches = arr[1];
                var feet = arr[0];
                var secondinch = inches.substring(0, 1);
                if (inches > 11) {
                    //Begin 11-27-2015 Muhammad Arshad Bug # EMR-96
                    var heightval = feet <= 9 ? arr[0] + '.' + secondinch : 9 + '.' + secondinch;
                    //End 11-27-2015 Muhammad Arshad Bug # EMR-96
                    $(obj).val(heightval);
                }
                    //Begin 11-27-2015 Muhammad Arshad Bug # EMR-96
                else {
                    var heightval = feet <= 9 ? arr[0] + '.' + inches : 9 + '.' + inches;
                    $(obj).val(heightval);
                }
                //End 11-27-2015 Muhammad Arshad Bug # EMR-96
            }
        }
        else {
            //Begin 11-27-2015 Muhammad Arshad Bug # EMR-96
            var heightval = Ctrlvalue <= 9 ? Ctrlvalue : 9;
            //End 11-27-2015 Muhammad Arshad Bug # EMR-96
            $(obj).val(heightval);
        }
        //Begin 18-04-2016 Abid Ali Bug # EMR-716
        if ($(obj).closest('form') != null && $(obj).closest('form').attr('id') == "frmClinicalVitals") {

            Clinical_Vitals.calculateBMI(null, null, null);
        }
        //End 18-04-2016 Abid Ali Bug # EMR-716
    },

    ValidateWeight: function (event, obj) {
        var Ctrlvalue = "";
        if (obj != null) {
            Ctrlvalue = $(obj).val();
        }

        if (Ctrlvalue.indexOf('.') != -1) {

            var arr = Ctrlvalue.split('.');

            if (arr.length > 0) {
                var inches = arr[1];
                var intVal = arr[0];
                //Begin 11-27-2015 Muhammad Arshad Bug # EMR-97 As suggested by Zain
                inches = inches <= 99 ? inches : 99;
                var weightval = intVal <= 999 ? intVal + '.' + inches : 999 + '.' + inches;
                $(obj).val(weightval);
                //End 11-27-2015 Muhammad Arshad Bug # EMR-97 As suggested by Zain
            }
        }
        else {
            //Begin 11-27-2015 Muhammad Arshad Bug # EMR-97 As suggested by Zain
            var weightval = Ctrlvalue <= 9999 ? Ctrlvalue : 9999;
            //End 11-27-2015 Muhammad Arshad Bug # EMR-97 As suggested by Zain
            $(obj).val(weightval);
        }

    },
    // added new paramater for DOM addition in Datatables(sDom)
    MakeEditableGrid: function (GridPanel, GridId, ClassName, AddDefaultRow, bInfo, bFilter, bPaginate, bSort, iPageSize, sDom) {
        $(GridPanel).css("display", "inline");
        //if ($.fn.dataTable.isDataTable(GridId))
        //    ;
        //else {
        //$(GridId + ' tbody').find("tr").remove();
        EMREditableGrid.initialize(GridId, ClassName, AddDefaultRow, bInfo, bFilter, bPaginate, bSort, iPageSize, sDom);
        $(GridId + "_length").remove();
        //}

        return EMREditableGrid;
    },

    //Author: Muhammad Arshad
    //Date: 15-12-2015
    //This function will handle showing paging on popup on click of ViewAll

    MakeFaceSheetPager: function (DestinationDiv, ArrFaceSheetComponents, currentComponentName) {
        if (Clinical_FaceSheet.FaceSheetModels.indexOf(DestinationDiv) < 0)
            Clinical_FaceSheet.FaceSheetModels.push(DestinationDiv);

        var FaceSheetPagerHTML = "";
        currentComponentName = currentComponentName != null ? currentComponentName : "";
        $.each(ArrFaceSheetComponents, function (i, item) {
            if (item.toLowerCase().indexOf("history") < 0 && item.toLowerCase() != currentComponentName.toLowerCase()) {
                var componentType = item;
                if (item.toLowerCase().indexOf("problem") > -1) {
                    componentType = "ProblemList";
                }
                //if (componentType.toLowerCase() == "complaints") {
                //    FaceSheetPagerHTML += '<div><button type="button" class="tab_space btn btn-default btn-sm" title="' + item + '" id="btn' + componentType + '" onclick="">' + item + '</button></div>';
                //}
                //else {
                FaceSheetPagerHTML += '<div><button type="button" class="tab_space btn btn-default btn-sm" title="' + item + '" id="btn' + componentType + '" onclick="Clinical_FaceSheet.OpenViewAllRecords(\'' + componentType + '\');">' + item + '</button></div>';
                // }

            }
            // it works only for slick as commented below
            //FaceSheetPagerHTML += '<div><button type="button" class="tab_space btn btn-default btn-sm" title="' + item + '" id="btn' + componentType + '" onclick="Clinical_FaceSheet.OpenViewAllRecords(\'' + componentType + '\');">' + item + '</button></div>';
        });
        if (DestinationDiv != null && DestinationDiv != "") {
            $(DestinationDiv).empty();
            if ($(DestinationDiv).hasClass("mb-sm") == false) {
                $(DestinationDiv).addClass("mb-sm");
            }
            if ($(DestinationDiv).hasClass("ml-default mr-default") == false) {
                $(DestinationDiv).addClass("ml-default mr-default");
            }
            $(DestinationDiv).append(FaceSheetPagerHTML);
            //$(DestinationDiv).scrollTabs();
            var innerDiv = $(DestinationDiv).find("div.scroll_tab_inner");
            if (innerDiv.hasClass("text-center") == false) {
                innerDiv.addClass("text-center");
            }
            var a = '"';

            $(DestinationDiv).css({ "padding-left": "45px", "float": "left", "padding-right": "45px", "position": "relative" });
            var itemLength = $(DestinationDiv + ' > div button').length - 1;

            $(DestinationDiv).slick({

                infinite: false,
                dots: true,
                variableWidth: true,

                prevArrow: "<div id='PreOneItem' onclick='EMRUtility.slickPrevious(" + a + DestinationDiv + a + ")' class='prev1-next1 prev1' style='margin-left: -18px;margin-top: -32px;'><i class='fa fa-angle-left sm-font'></i></div><div id='StartItem' class='prev1-next1 prev1 ' style='margin-top: -32px;margin-left: -40px;' onclick='EMRUtility.slickMoveToStart(" + a + DestinationDiv + a + ")'><i class='fa fa-angle-double-left sm-font'></i></div>",
                nextArrow: "<div id='NextOneItem' onclick='EMRUtility.slickNext(" + a + DestinationDiv + a + ")' class='prev1-next1 next1' style='margin-right: -28px; margin-top: -32px;'><i class='fa fa-angle-right sm-font'></i></div><div id='EndItem' class='prev1-next1 next1 ' style='margin-top: -32px;' onclick='EMRUtility.slickMoveToEnd(" + a + DestinationDiv + a + ")' ><i class='fa fa-angle-double-right sm-font'</i></div> ",

                // 05/01/2016 //Start// Added by Abid Ali

                onInit: function () {
                    // init code goes here
                },
                onAfterChange: function (slide, index) {

                    if (index == 0) {

                        $(DestinationDiv + " #PreOneItem").addClass("disableAll");
                        $(DestinationDiv + " #StartItem").addClass("disableAll");

                        $(DestinationDiv + " #NextOneItem").removeClass("disableAll");
                        $(DestinationDiv + " #EndItem").removeClass("disableAll");
                    }
                    else if (index == itemLength) {
                        $(DestinationDiv + " #NextOneItem").addClass("disableAll");
                        $(DestinationDiv + " #EndItem").addClass("disableAll");

                        $(DestinationDiv + " #PreOneItem").removeClass("disableAll");
                        $(DestinationDiv + " #StartItem").removeClass("disableAll");
                    }
                },

                onBeforeChange: function (event, slick, currentSlide, nextSlide) {
                    // code goes here
                }

            });
        }

    },
    slickMoveToStart: function (DestinationDiv) {
        $(DestinationDiv + " #StartItem").addClass("disableAll");
        $(DestinationDiv + " #PreOneItem").addClass("disableAll");
        $(DestinationDiv + " #NextOneItem").removeClass("disableAll");
        $(DestinationDiv + " #EndItem").removeClass("disableAll");
        var countDiv = $(DestinationDiv + " div button").length;
        $(DestinationDiv).slickGoTo(countDiv - countDiv);
    },
    slickMoveToEnd: function (DestinationDiv) {

        $(DestinationDiv + " #NextOneItem").addClass("disableAll");
        $(DestinationDiv + " #EndItem").addClass("disableAll");

        $(DestinationDiv + " #StartItem").removeClass("disableAll");
        $(DestinationDiv + " #PreOneItem").removeClass("disableAll");

        var countDiv = $(DestinationDiv + " div button").length;
        $(DestinationDiv).slickGoTo(parseInt(countDiv - 1));

    },
    slickPrevious: function (DestinationDiv) {

        // $(DestinationDiv).slick('slickPrev');

        $(DestinationDiv + " #NextOneItem").removeClass("disableAll");
        $(DestinationDiv + " #EndItem").removeClass("disableAll");
    },
    slickNext: function (DestinationDiv) {
        $(DestinationDiv + " #StartItem").removeClass("disableAll");
        $(DestinationDiv + " #PreOneItem").removeClass("disableAll");
    },

    //Start//06/01/2016//Abid Ali//Method to show/hide slick slider buttons on resize event
    PopUpResize: function (tabId) {

        // Facesheet section
        if (tabId == "clinicalTabFaceSheet") {

            // Get actionPanContainer value
            var actionPanContainer = GetTab(tabId).ActionPanContainer;
            var activePopedUpComponentsPagerdiv = Clinical_FaceSheet.FaceSheetModels;

            $('#' + actionPanContainer).resize(function () {

                $.each(activePopedUpComponentsPagerdiv, function (index, faceSheetPager) {

                    EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetPager);
                    // Move to zero index item in the slider
                    EMRUtility.slickMoveToStart(faceSheetPager)
                });
            });
        }


    },
    //End//06/01/2016//Abid Ali//Method to show/hide slick slider buttons on resize event

    //Start//06/01/2016//Abid Ali//Method to show/hide slick slider buttons
    //Input Argument Facesheet pager div
    HideShowFaceSheetPagerBtnControls: function (faceSheetPager) {

        var faceSheetButtonsWidth = 0;
        var faceSheetButtons = $(faceSheetPager).find('button');

        if (faceSheetButtons != null || typeof faceSheetButtons != 'undefined') {

            faceSheetButtons.each(function () {
                faceSheetButtonsWidth += $(this).outerWidth(true);
            });
        }
        var contentWidth = $(faceSheetPager).parent().width();
        var emptySpace = contentWidth - faceSheetButtonsWidth;
        if (emptySpace > 30) {
            $(faceSheetPager).find('.slick-track').css("width", "");
            $(faceSheetPager).find('#PreOneItem').css("display", "none");
            $(faceSheetPager).find('#NextOneItem').css("display", "none");
            $(faceSheetPager).find('#StartItem').css("display", "none");
            $(faceSheetPager).find('#EndItem').css("display", "none");
        }
        else {
            $(faceSheetPager).css("float", "");
            $(faceSheetPager).find('.slick-track').css("width", "");
            $(faceSheetPager).find('#PreOneItem').css("display", "block");
            $(faceSheetPager).find('#NextOneItem').css("display", "block");
            $(faceSheetPager).find('#StartItem').css("display", "block");
            $(faceSheetPager).find('#EndItem').css("display", "block");
        }
    },
    //End//06/01/2016//Abid Ali//Method to show/hide slick slider buttons

    getDate: function (operator, days, month, year) {

        var currentDate = new Date();

        if (operator != null) {

            operator = operator.trim();

            switch (operator) {
                case '-':
                    if (days != null) {
                        currentDate.setDate(currentDate.getDate() - days);
                    }
                    if (month != null) {
                        currentDate.setMonth(currentDate.getMonth() - month);
                    }

                    break;
                case '+':
                    if (days != null) {
                        currentDate.setDate(currentDate.getDate() + days);
                    }
                    if (month != null) {
                        currentDate.setMonth(currentDate.getMonth() + month);
                    }
                    break;
            }
        }
        return currentDate;
    },



    /**
 * detect IE
 * returns version of IE or false, if browser is not Internet Explorer
 */
    detectIE: function () {
        var ua = window.navigator.userAgent;

        var msie = ua.indexOf('MSIE ');
        if (msie > 0) {
            // IE 10 or older => return version number
            return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
        }

        var trident = ua.indexOf('Trident/');
        if (trident > 0) {
            // IE 11 => return version number
            var rv = ua.indexOf('rv:');
            return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
        }

        var edge = ua.indexOf('Edge/');
        if (edge > 0) {
            // Edge (IE 12+) => return version number
            return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
        }

        // other browser
        return false;
    },
    //This Widget will be used for Switch checkbox
    SwicthWidgetInializatoin: function () {
        (function ($) {
            'use strict';
            $(function () {
                $('[data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },
    /*Added: Muhammad Azhar Shahzad
    Date: Jan 07, 2016
    purpose: Reset all values of any control container*/
    resetControlValue: function (obj) {
        $(obj).find('[data-plugin-datepicker]').datepicker("setDate", new Date());
        $(obj).resetAllControls();
        var currentElementTagName = obj.tagName != null ? obj.tagName : obj.prop("tagName");
        if ($(obj).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea')
            $(obj).val('');
        if ($(obj).attr('type') == 'checkbox' || $(obj).attr('type') == 'radio') {
            obj.checked = false;
        }

        if (currentElementTagName.toLowerCase() == 'select') {
            $(obj).find('option:selected').removeAttr('selected');
            //$(this).attr('selectedIndex', '-1');
            $(obj).find('option:eq(0)').attr('selected', 'selected');
        }
    },

    slicefunc: function (arrayValue, indexValueCheck, min, max) {
        var returnArray = []
        if (arrayValue != null && arrayValue.length > 0) {
            $.each(arrayValue, function (index, element) {
                if (element.indexOf(indexValueCheck) > -1) {
                    returnArray.push(element.slice(min, max));
                }
            });
        }
        return returnArray;
    },

    /*Added: Abid Ali
    Date: Jan 07, 2016
    purpose: modified for error messages */
    ValidateFromToDate: function (FormId, CtrlFromDateId, CtrlToDateId, IsOptional, onFromDateChangeCallback, onToDateChangeCallback, optionalMessage) {

        var CtrlForm = "#" + FormId;
        var CtrlFromDate = CtrlForm + " #" + CtrlFromDateId;
        var CtrlToDate = CtrlForm + " #" + CtrlToDateId;
        var CtrFromDateName = $(CtrlToDate).attr("name");
        var CtrToDateName = $(CtrlToDate).attr("name");
        var startDate = new Date('01/01/1700');
        var endDate = new Date('01/01/' + Number(new Date().getFullYear() + 35));

        // message to be modified accordingly
        var fromFieldName;
        var toFieldName;

        var fromFieldName = $(CtrlFromDate).parent().parent().find('label').text();
        if ($(CtrlFromDate).parent().length > 0 && $(CtrlFromDate).parent().parent().length > 0 && $(CtrlFromDate).parent().parent().find('label').length > 0) {
            fromFieldName = $(CtrlFromDate).parent().parent().find('label').text();
        }
        else
            fromFieldName = "From Date";

        if ($(CtrlToDate).parent().length > 0 && $(CtrlToDate).parent().parent().length > 0 && $(CtrlToDate).parent().parent().find('label').length > 0) {
            toFieldName = $(CtrlToDate).parent().parent().find('label').text();
        }
        else
            toFieldName = "To Date";



        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];

        //$(CtrlForm + ' ' + CtrlFromDate + ',' + CtrlToDate).datepicker({
        //    todayHighlight: true,
        //    format: date_format,
        //});
        $(CtrlToDate).attr('disabled', true);
        $(CtrlToDate).attr('maxlength', '10');
        $(CtrlFromDate).attr('maxlength', '10');
        $(CtrlFromDate).datepicker({
            todayHighlight: true,
            format: date_format,
            todayBtn: 'linked',
        }).change(function (e) {
            if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                var fromDate = new Date($(CtrlFromDate).val());
                var toDate = new Date($(CtrlToDate).val());

                if (fromDate <= toDate && fromDate != '') {
                    $(CtrlToDate).val($(CtrlToDate).val()).datepicker('update');
                } else {
                    $(this).val('');
                    if (typeof optionalMessage != "undefined") {
                        utility.DisplayMessages(optionalMessage, 3)
                    }
                    else
                        utility.DisplayMessages(fromFieldName + " is greater than " + toFieldName, 3);
                }
            }

            $(CtrlToDate).attr('disabled', false);
            // $(CtrlToDate).datepicker('remove');


            var inputDate = $(CtrlFromDate).datepicker('getDate');

            var date_format = 'dd/mm/yyyy';
            //set default Date Formate
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }

            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(CtrlToDate).attr("name");
            }
            if (typeof CtrFromDateName == 'undefined') {
                CtrFromDateName = $(CtrlFromDate).attr("name");
            }
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);

            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrFromDateName);
            if (onFromDateChangeCallback != null && typeof (onFromDateChangeCallback) == 'function') {
                setTimeout(onFromDateChangeCallback, 50);
            }

        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        $(CtrlToDate).datepicker({
            todayHighlight: true,
            // startDate: inputDate,
            format: date_format,
            //todayBtn: 'linked',
        }).change(function (e) {
            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(this).attr("name");
            }
            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);

            if (onToDateChangeCallback != null && typeof (onToDateChangeCallback) == 'function') {
                setTimeout(onToDateChangeCallback, 50);
            }
            var CurrentDatepicker = this;
            setTimeout(function () {
                if ($(CurrentDatepicker).val().length == date_format.length) {
                    if (!utility.isValidDate($(CurrentDatepicker).val())) {
                        $(CurrentDatepicker).val('');
                        utility.DisplayMessages("Please enter valid date", 3);
                        $(CtrlForm).bootstrapValidator('revalidateField', CurrentDatepicker.name);
                    }
                    if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                        var fromDate = new Date($(CtrlFromDate).val());
                        var toDate = new Date($(CtrlToDate).val());
                        if (fromDate > toDate) {
                            $(CurrentDatepicker).val('');
                            if (typeof optionalMessage != "undefined") {
                                utility.DisplayMessages(optionalMessage, 3)
                            }
                            else
                                utility.DisplayMessages(toFieldName + " is smaller than " + fromFieldName, 3)
                        }
                    }
                }
            }, 50);
        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        var DateNewFormat = date_format.replace('dd', '99');
        DateNewFormat = DateNewFormat.replace('mm', '99');
        DateNewFormat = DateNewFormat.replace('yyyy', '9999');

        //$(CtrlFromDate).inputmask({
        //    mask: DateNewFormat
        //});
        //$(CtrlToDate).inputmask({
        //    mask: DateNewFormat
        //});
        $(CtrlFromDate).on('blur', function (e) {
            setTimeout(
               function () {

                   if ($(CtrlFromDate).val() != "")
                       utility.ValidateDate(CtrlFromDate);
               }, 100);
        });
        $(CtrlToDate).on('blur', function (e) {
            setTimeout(function () {

                if ($(CtrlToDate).val() != "")
                    utility.ValidateDate(CtrlToDate)
            }, 100);
        });
    },

    ValidateDateMonthView: function (Control) {
        if ($(Control).val().length == 7) {
            //$(Control).val(DateValued);
            //$(Control).datepicker("setDate", DateValued);
        } else {
            $(Control).val('');
            utility.DisplayMessages("Please enter valid date", 3);

        }
    },


    selectOptionsByCommaSeprateValue: function (Control, Value) {

        var obj = Control;
        $.each(Value.split(","), function (i, item) {
            obj.find('option[value=' + item + ']').prop('selected', true);
        });
    },
    // this function is used to scroll user to tab where control exists
    scrollToPNcomponent: function (component) {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + " " + component).length > 0) {
            window.scrollTo(0, $('#' + Clinical_ProgressNote.params["PanelID"] + " " + component).offset().top - ($('#StickyPNSection').outerHeight() + $('#ActionsInitialOfficeVisit').outerHeight() + 149));
            setTimeout(function () {
                window.scrollTo(0, $('#' + Clinical_ProgressNote.params["PanelID"] + " " + component).offset().top - ($('#StickyPNSection').outerHeight() + $('#ActionsInitialOfficeVisit').outerHeight() + 149));
            }, 10);
        }
    },
    // This is widget initializatoin for Tiny MCE Editor
    // Created BY: Azhar Shahzad
    // Date: March 28, 2016
    //InitTinymceControl: function (Isreadonly) {
    //    if (typeof tinymce.activeEditor != 'undefined') {
    //        tinymce.EditorManager.execCommand('mceRemoveEditor', true, "elm1");
    //    }
    //    tinymce.init({
    //        selector: "textarea#elm1",
    //        theme: "modern",
    //        readonly: Isreadonly,
    //        height: 400,
    //        plugins: [
    //            "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
    //            "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
    //            "table contextmenu directionality emoticons template textcolor paste fullpage textcolor colorpicker textpattern"
    //        ],

    //        add_unload_trigger: false,
    //        paste_data_images: true, //enable drag drop image pasting
    //        toolbar1: "undo redo | styleselect  | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons table",
    //        image_advtab: true,
    //        elementpath: false, // removed element path showing on status bar EMR-518 bug fix by Azhar Siyal
    //        style_formats: [
    //        { title: 'Bold text', inline: 'strong' },
    //        { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
    //        { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
    //        { title: 'Badge', inline: 'span', styles: { display: 'inline-block', border: '1px solid #2276d2', 'border-radius': '5px', padding: '2px 5px', margin: '0 2px', color: '#2276d2' } },
    //        { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
    //        ],
    //        file_picker_callback: function (callback, value, meta) {
    //            UpdateImageTiny = callback;
    //            document.getElementById("ImageUploaderTinymce").click();
    //            if (meta.filetype == 'file') {
    //            }
    //            // Provide image and alt text for the image dialog
    //            if (meta.filetype == 'image') {
    //                callback($('#EncodedImageString').val(), { alt: $('#ImageUploaderTinymce').val().split('\\').pop() });
    //            }
    //            // Provide alternative source and posted for the media dialog
    //            if (meta.filetype == 'media') {
    //            }
    //        },
    //        contextmenu: "link image inserttable | cell row column deletetable | CreateNewDataField InsertDataField"
    //    });

    //    jQuery(function () {
    //        document.getElementById('ImageUploaderTinymce').addEventListener('change', function () {
    //            readImage(this);
    //        }, false);
    //    });
    //    function readImage(input) {
    //        if (input.files && input.files[0]) {
    //            var reader = new FileReader();
    //            reader.onload = function (e) {
    //                $('#EncodedImageString').val(e.target.result);
    //                if ($('#ImageUploaderTinymce').val() != "") {
    //                    if ((($('#EncodedImageString').val().length) / 1.37) / 1000000 > Number(globalAppdata['FileSize'])) {
    //                        utility.DisplayMessages("Please select Image less than 5 mb Size", 2);
    //                        return false;
    //                    } else {
    //                        UpdateImageTiny($('#EncodedImageString').val(), { alt: $('#ImageUploaderTinymce').val().split('\\').pop() });
    //                    }
    //                }
    //            }
    //            reader.readAsDataURL(input.files[0]);
    //        }
    //    }
    //},

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
            forced_root_block: "div",
            force_br_newlines: true,
            force_p_newlines: false,
            remove_linebreaks: false,
            // editor_selector: "myTextEditorNotes",
            height: setHeight,
            fontsize_formats: '8pt 10pt 12pt 14pt 16pt 18pt 20pt 24pt 36pt',
            plugins: [
            "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
            "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
            "table directionality emoticons template textcolor paste fullpage textcolor colorpicker textpattern"
            ],
            add_unload_trigger: false,
            paste_data_images: true, //enable drag drop image pasting
            toolbar1: "undo redo | styleselect  | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons table | fontsizeselect",
            image_advtab: true,
            elementpath: false, // removed element path showing on status bar EMR-518 bug fix by Azhar Siyal
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
            setup: function (editor) {
                editor.on('init', function (e) {
                    //all your after init logics here.
                    //just to make sure, tinymce should be visible
                    e.target.show();
                    setTimeout(function () {
                        tinymce.execCommand('mceFocus', false, ControlId);
                    }, 1000)
                    objDeffered.resolve();
                });
            },
            //contextmenu: "link image inserttable | cell row column deletetable | CreateNewDataField InsertDataField"
        });

        jQuery(function () {
            if (document.getElementById('ImageUploaderTinymce')) {
                document.getElementById('ImageUploaderTinymce').addEventListener('change', function () {
                    readImage(this);
                }, false);
            }
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
    InitTinymceControlWithMacro: function (Isreadonly, ControlId, height, PanelID) {
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
            forced_root_block: "div",
            force_br_newlines: true,
            force_p_newlines: false,
            remove_linebreaks: false,
            // editor_selector: "myTextEditorNotes",
            height: setHeight,
            fontsize_formats: '8pt 10pt 12pt 14pt 16pt 18pt 20pt 24pt 36pt',
            plugins: [
            "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
            "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
            "table directionality emoticons template textcolor paste fullpage textcolor colorpicker textpattern"
            ],
            add_unload_trigger: false,
            paste_data_images: true, //enable drag drop image pasting
            toolbar1: "undo redo | styleselect  | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons table | fontsizeselect",
            image_advtab: true,
            elementpath: false, // removed element path showing on status bar EMR-518 bug fix by Azhar Siyal
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
            setup: function (editor) {
                editor.on('init', function (e) {
                    //all your after init logics here.
                    //just to make sure, tinymce should be visible
                    e.target.show();
                    setTimeout(function () {
                        tinymce.execCommand('mceFocus', false, ControlId);
                    }, 1000)
                    objDeffered.resolve();
                });
            },
            init_instance_callback: function (editor) {
                editor.on('keyup', function (e) {

                    $("#" + PanelID + ' #MacroDescDetails').hide();
                    if (e.keyCode == 190 || e.which == 190 || e.keyCode == 110 || e.which == 110) // dot key is pressed
                    {
                        Create_Letter.DetectTrigger(e);
                    }
                });

            },
            //contextmenu: "link image inserttable | cell row column deletetable | CreateNewDataField InsertDataField"
        });

        jQuery(function () {
            if (document.getElementById('ImageUploaderTinymce')) {
                document.getElementById('ImageUploaderTinymce').addEventListener('change', function () {
                    readImage(this);
                }, false);
            }
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

    //Author: Abid Ali
    //Date : 29-03-2016
    //Modified Datepicker For bug# EMR-529
    CreateDatePicker: function (controlId, onChangeCallback, isCurrentDate, FormId, IsOptional, isFutureDateEnable) {
        var startDate = new Date('01/01/1700');
        var endDate = new Date('01/01/' + Number(new Date().getFullYear() + 35));
        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];
        var DateNewFormat = date_format.replace('dd', '99');
        DateNewFormat = DateNewFormat.replace('mm', '99');
        DateNewFormat = DateNewFormat.replace('yyyy', '9999');

        //modified Part
        var futureDateEnable = isFutureDateEnable ? '+0d' : null;

        $('#' + controlId).attr('maxlength', '10');
        $('#' + controlId).datepicker({
            todayHighlight: true,
            format: date_format,
            todayBtn: 'linked',
            //modified Part
            endDate: futureDateEnable

        }).on('changeDate', function (e) {

            if (typeof (onChangeCallback) == 'function') {
                setTimeout(onChangeCallback, 50);
            }
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }
            //$(this).val(e.target.value);
            if (FormId != null && IsOptional != null && !IsOptional) {
                // to Handle Multiple Date Controls Revalidation
                $('#' + controlId).each(function () {
                    var CtrlName = $(this).attr("name");
                    if (CtrlName != null) {
                        $('#' + FormId).bootstrapValidator('revalidateField', CtrlName);
                    }
                });

            }
            $(this).datepicker('hide');
        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
            //alert(e.target.value);

        }).on('blur', function (e) {
            var datepickerID = e.currentTarget;
            setTimeout(
               function () {
                   if ($(datepickerID).val() != "")
                       utility.ValidateDate(datepickerID);
               }, 100);

        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        if (isCurrentDate)
            $('#' + controlId).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
    },


    //Author: Abid Ali
    //Date : 10-06-2016
    //Get FavList status
    getFavListStatus: function (favListName) {

        var isFavListExists = false;
        if (globalAppdata['FavListNames'] != null) {

            var favListNames = globalAppdata['FavListNames'].split(',');
            isFavListExists = ($.inArray(favListName, favListNames) !== -1);
        }
        return isFavListExists;
    },

    //Author: M Ahmad Imran
    //Date : 4-1-2016
    //Get FreeText status
    getFreeTextStatus: function (FreeTextName) {

        var isFreeTextExists = false;
        if (globalAppdata['FreeTextNames'] != null) {

            var FreeTextNames = globalAppdata['FreeTextNames'].split(',');
            isFreeTextExists = ($.inArray(FreeTextName, FreeTextNames) !== -1);
        }
        return isFreeTextExists;
    },

    //Author: Abid Ali
    //Date : 10-06-2016
    //Set FavList status
    setFavListStatus: function (favListName, isOpen) {

        var commaSeparatedFavListNames = globalAppdata['FavListNames'];
        var comma = ",";
        var favListNames = globalAppdata['FavListNames'] != null ? globalAppdata['FavListNames'].split(',') : [];
        var isExists = ($.inArray(favListName, favListNames) !== -1);

        if (isOpen) {

            //Add if FavListName not exists
            commaSeparatedFavListNames = isExists == false ? commaSeparatedFavListNames + comma + favListName : commaSeparatedFavListNames;
            globalAppdata['FavListNames'] = commaSeparatedFavListNames;
        }
        else {
            //Remove if FavListName exists
            if (isExists) {

                var filteredFavListNames = $.grep(favListNames, function (item, index) {
                    return (favListName != item);
                });
                globalAppdata['FavListNames'] = filteredFavListNames.join();
            }
        }
        return globalAppdata['FavListNames'];
    },

    //Author: M Ahmad Imran
    //Date : 10-06-2016
    //Set FavList status
    setFreeTextStatus: function (FreeTextName, isOpen) {

        var commaSeparatedFreeTextNames = globalAppdata['FreeTextNames'];
        var comma = ",";
        var FreeTextNames = globalAppdata['FreeTextNames'] != null ? globalAppdata['FreeTextNames'].split(',') : [];
        var isExists = ($.inArray(FreeTextName, FreeTextNames) !== -1);

        if (isOpen) {

            //Add if FavListName not exists
            commaSeparatedFreeTextNames = isExists == false ? commaSeparatedFreeTextNames + comma + FreeTextName : commaSeparatedFreeTextNames;
            globalAppdata['FreeTextNames'] = commaSeparatedFreeTextNames;
        }
        else {
            //Remove if FavListName exists
            if (isExists) {

                var filteredFreeTextNames = $.grep(FreeTextNames, function (item, index) {
                    return (FreeTextName != item);
                });
                globalAppdata['FreeTextNames'] = filteredFreeTextNames.join();
            }
        }
        return globalAppdata['FreeTextNames'];
    },

    //Author: Abid Ali
    //Date : 10-06-2016
    //Insert update favList status in globalApp and in db table UserEntityOption.
    insertUpdateFavListStatus: function (favListName, isFavListOpened, FavListVal) {
        var dfd = $.Deferred();
        //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
        var commSeparatedFavListsStatus = "";
        if (isFavListOpened == true) {

            commSeparatedFavListsStatus = EMRUtility.setFavListStatus(favListName, true);
        }
        else {
            commSeparatedFavListsStatus = EMRUtility.setFavListStatus(favListName, false);
        }
        //End 10-06-2016 Abid Ali for favorite list setting for all favLists

        //Db call to insert/update favLists
        var objData = {};
        objData['FavListNames'] = commSeparatedFavListsStatus;
        objData["commandType"] = "insert_update_favlist_toggle_status";
        var data = JSON.stringify(objData);

        $.when(MDVisionService.APIService(data, "LabOrder", "LabOrder")).done(function (response) {
            dfd.resolve();
        });
        return dfd;
    },

    insertUpdateFavListVal: function (favListName, FavListVal) {
        var dfd = $.Deferred();
        var objData = {};
        objData['FavoriteListName'] = favListName;
        objData['FavListVal'] = FavListVal;
        objData["commandType"] = "insert_update_favlist_Value";
        var data = JSON.stringify(objData);
        $.when(MDVisionService.APIService(data, "FavoriteList", "FavoriteList")).done(function (response) {
            dfd.resolve();
        });
        return dfd;
    },
    getFavListValue: function (FavListName) {
        var objData = new Object();
        objData["FavoriteListName"] = FavListName;
        objData["commandType"] = "getFavListValue";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },



    //Author: M Ahmad Imran
    //Date : 4-1-2017
    //Insert update favList status in globalApp and in db table UserEntityOption.
    insertUpdateFreeTextStatus: function (FreeTextName, isFreeText) {

        //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
        var commSeparatedFreeTextStatus = "";
        commSeparatedFreeTextStatus = EMRUtility.setFreeTextStatus(FreeTextName, isFreeText);
        //End 10-06-2016 Abid Ali for favorite list setting for all favLists

        //Db call to insert/update favLists
        var objData = {};
        objData['FreeTextNames'] = commSeparatedFreeTextStatus;
        objData["commandType"] = "insert_update_freetext_status";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "LabOrder", "LabOrder");

    },

    unSelectOtherTabs: function (TabId, IsMenu) {
        var Tab = GetTab(TabId);

        for (var i = 0; i < TabsArray.length; i++) {
            if (TabsArray[i].MasterTabID == Tab.MasterTabID && TabsArray[i].ParentTabID == Tab.ParentTabID && TabsArray[i].TabID != TabId) {
                TabsArray[i]["Selected"] = false;
                TabsArray[i]["Active"] = false;

                var UnselectedtabObj = $(document.getElementById(TabsArray[i]["TabID"]));
                var UnselectedContObj = $(document.getElementById(TabsArray[i]["Container"]));

                var UnselectedpnlObj;

                if (TabsArray[i]["PanelID"] != "" && TabsArray[i]["MasterTabID"] != "")
                    UnselectedpnlObj = $('#' + TabsArray[i]["Container"] + ' #' + TabsArray[i]["PanelID"]);
                else
                    UnselectedpnlObj = $(document.getElementById(TabsArray[i]["PanelID"]));

                UnSelectedMenuAndTab(TabsArray[i], IsMenu);

                if (UnselectedpnlObj.css('display') != 'none') {
                    UnselectedpnlObj.hide('fade', { direction: 'left', easing: 'easeInOutElastic' }, 200, showCurrentTab);
                }
                $(UnselectedContObj).css("display", "none");
            } else if (TabsArray[i].TabID == TabId) TabsArray[i].Selected = true;

        }
    },
    //generic function to create note from Dashboard, Global link and Scheduler
    //Created By Azhar Shahzad on 29/06/2016
    CreateNote: function (mode, PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, ResourceId, ResourceName, ResourceproviderId, ResourceproviderName, VisitTypeId, PatientTypeId, AppointmentTimeFrom, AppointmentTimeTo) {
        params["QuickAddPatient"] = true;
        if (params["patientID"] != PatientId) {
            $.when(setPatientBanner(PatientId, "1")).then(function () {
                Patient_Demographic.isFinanicialAlert = true;
                EMRUtility.loadNotesTab(mode, PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, ResourceId, ResourceName, ResourceproviderId, ResourceproviderName, VisitTypeId, PatientTypeId, AppointmentTimeFrom, AppointmentTimeTo);
            });
        } else {
            Patient_Demographic.isFinanicialAlert = false;
            EMRUtility.loadNotesTab(mode, PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, ResourceId, ResourceName, ResourceproviderId, ResourceproviderName, VisitTypeId, PatientTypeId, AppointmentTimeFrom, AppointmentTimeTo);
        }
    },

    loadNotesTab: function (mode, PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, ResourceId, ResourceName, ResourceproviderId, ResourceproviderName, VisitTypeId, PatientTypeId, AppointmentTimeFrom, AppointmentTimeTo) {
        params["mode"] = mode;
        params["NotesProviderId"] = ProviderId;
        params["PatientId"] = PatientId;
        params["AppointmentId"] = AppointmentId;
        params["AppointmentVisitId"] = VisitId;
        params["NotesVisitTime"] = AppointmentTime;
        params["NotesVisitId"] = VisitId;
        params["ParentCntrlLoadid"] = ParentCntrlLoadid;
        params["NotesProviderId"] = ProviderId;
        params["NotesProviderName"] = ProviderName
        params["NotesVisitDate"] = VisitDate;
        params['ScheduleReason'] = Reason;
        params['NotesRoom'] = Room;
        params['AppointmentTimeFrom'] = AppointmentTimeFrom;
        params['AppointmentTimeTo'] = AppointmentTimeTo;
        params['NotesFacilityName'] = FacilityName;
        params['NotesFacilityId'] = FacilityId;
        params["RefProviderName"] = (RefProviderName == "undefined" || RefProviderName == null) ? null : RefProviderName;
        params["RefProviderId"] = (RefProviderId == "undefined" || RefProviderId == null) ? null : RefProviderId;
        params["ResourceName"] = (ResourceName == "undefined" || ResourceName == null) ? null : ResourceName;
        params["ResourceId"] = (ResourceId == "undefined" || ResourceId == null) ? null : ResourceId;
        params["ResourceproviderName"] = (ResourceproviderName == "undefined" || ResourceproviderName == null) ? null : ResourceproviderName;
        params["ResourceproviderId"] = (ResourceproviderId == "undefined" || ResourceproviderId == null) ? null : ResourceproviderId;
        params['ForProgressNote'] = ForProgressNote;
        params['VisitTypeId'] = (VisitTypeId == "undefined" || VisitTypeId == null) ? null : VisitTypeId;;
        params['PatientTypeId'] = (PatientTypeId == "undefined" || PatientTypeId == null || PatientTypeId == "") ? null : PatientTypeId;;
        if (NotesId != null && NotesId != '' && NotesId > 0) {

            params['NotesId'] = NotesId;
        } else {
            params['NotesId'] = null;
        }
        var IsProgressNoteSelected = false;
        if (GetSelectedTab("mstrTabClinical").ContainerControlID == 'Clinical_ProgressNote' && (params["patientID"] != PatientId)) {
            IsProgressNoteSelected = true;
        }
        //Loading Notes to edit

        $("ul li[id*=mstrMenu]").hide();

        if ($("html").hasClass("sidebar-left-collapsed")) {
            $("html").removeClass("sidebar-left-collapsed");
        }

        $("#anchorMainMenu").show();
        $("div[id*=mstrDiv]").hide();

        $.when(ClinicalMenuSettings.ClinicalMenuSettingsSearch(null)).then(function () {

            $('#mstrTabClinical').siblings().removeClass('active');
            $('#mstrTabClinical').addClass('active');
            $('#ClinicalUL li').removeClass('nav-expanded nav-active');
            $('#ClinicalUL li#clinicalMenuNotes').addClass('nav-expanded nav-active');
            $('#ctrlPanDashBoard').hide();

            EMRUtility.unSelectOtherTabs('mstrTabClinical', 'false');


            javascript: ClinicalMenuClick(window.event, function () {
                $.when(ClinicalMenuSettings.TopButtons('clinicalMenuNotes')).then(function () {
                    ClinicalMenuSettings.selectClinicalMenu('clinicalMenuNotes');
                    // SelectTab("clinicalTabPhoneEncounter", "false");
                    if (($("#mstrDivNotes").length == 0 || IsProgressNoteSelected) && (Clinical_ProgressNote.params.NotesId != params['NotesId'])) {
                        SelectTab("clinicalTabNotes", "false");
                    }
                    //if (params['NotesId'] == null) {
                    //    setTimeout(function () {
                    //        //Patient_Demographic.FillPatientAlertsCount();
                    //        $('#pnlClinicalPhoneEncounter #txtVisitReason').val('Transitional Care Management');
                    //        $("#pnlClinicalPhoneEncounter #NoteTemplate option").each(function () {
                    //            if ($(this).text().toLowerCase() == "phone encounter tcm") {
                    //                $(this).attr('selected', 'selected');
                    //            }
                    //        });
                    //    }, 3000);
                    //} else {
                    setTimeout(function () { Patient_Demographic.FillPatientAlertsCount(); }, 3000);
                    //}
                });
            }, 0, this, 'clinicalMenuNotes', 'li');
        });
    },
    IsNoteUnSign: function (UserId) {
        var dfd = $.Deferred();
        EMRUtility.IsNoteUnSign_DB_Call(UserId).done(function (response) {
            //response = JSON.parse(response);
            if (response.status != false) {
                if (response.Message == "yes") {
                    dfd.response = true;
                    dfd.resolve();

                    //return true;
                }
                else {
                    dfd.response = false;
                    dfd.resolve();
                    //return false;
                }
            }
            else {
                utility.DisplayMessages(response.Message, 2);
                dfd.response = false;
                dfd.resolve();
                //return false;
            }
        });
        return dfd;
    },


    //Check Patient Is Registered On Drfirst or Not
    //Created By M Ahmad Imran on 13/7/2016
    CheckPatientIsRegisteredOnDrFirst: function (PatientId) {
        var dfd = $.Deferred();
        EMRUtility.CheckPatientIsRegisteredOnDrFirst_DB_Call(PatientId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.Message == "yes") {
                    dfd.response = true;
                    dfd.resolve();

                    //return true;
                }
                else {
                    dfd.response = false;
                    dfd.resolve();
                    //return false;
                }
            }
            else {
                utility.DisplayMessages(response.Message, 2);
                dfd.response = false;
                dfd.resolve();
                //return false;
            }
        });
        return dfd;
    },

    //Check User has Rcopia Rights or Not
    //Created By M Ahmad Imran on 19/10/2016
    CheckUserHaveRcopiaRights: function (UserId) {
        var dfd = $.Deferred();
        EMRUtility.CheckUserHaveRcopiaRights_DB_Call(UserId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.Message == "yes") {
                    dfd.response = true;
                    dfd.resolve();

                    //return true;
                }
                else {
                    dfd.response = false;
                    dfd.resolve();
                    //return false;
                }
            }
            else {
                utility.DisplayMessages(strMessage, 2);
                dfd.response = false;
                dfd.resolve();
                //return false;
            }
        });
        return dfd;
    },

    ValidateDateYearView: function (Control) {
        if ($(Control).val().length == 4) {
            //$(Control).val(DateValued);
            //$(Control).datepicker("setDate", DateValued);
        } else {
            $(Control).val('');
            utility.DisplayMessages("Please enter valid year", 3);

        }
    },

    CreateYearViewDatePicker: function (controlId, onChangeCallback, isCurrentDate, FormId, IsOptional) {
        var startDate = new Date('01/01/1700');
        var endDate = new Date('01/01/' + Number(new Date().getFullYear() + 35));
        var date_format = ' yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];


        var DateNewFormat = date_format.split(date_format.substring(date_format.indexOf("d"), date_format.indexOf("d") + 3)).join('');
        DateNewFormat = DateNewFormat.replace('mm', '99');
        DateNewFormat = DateNewFormat.replace('yyyy', '9999');
        //$('#' + controlId).inputmask({
        //    mask: DateNewFormat
        //});
        //  $('#' + controlId).attr('data-mask', DateNewFormat);
        $('#' + controlId).attr('maxlength', '10');
        $('#' + controlId).datepicker({
            todayHighlight: true,
            format: date_format.split(date_format.substring(date_format.indexOf("m"), date_format.indexOf("m") + 6)).join(''),
            todayBtn: 'linked',
            viewMode: "years",
            minViewMode: "years"
        }).on('changeDate', function (e) {

            if (typeof (onChangeCallback) == 'function') {
                setTimeout(onChangeCallback, 50);
            }
            date_format = date_format.substring(date_format.indexOf("y"), date_format.indexOf("y") + 4);
            if ($(this).val().length == date_format.length) {
                if (!EMRUtility.isValidYearDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid year", 3);
                }
            }
            //$(this).val(e.target.value);

            $(this).datepicker('hide');
            if (FormId != null && IsOptional != null && !IsOptional) {
                // to Handle Multiple Date Controls Revalidation
                $('#' + controlId).each(function () {
                    var CtrlName = $(this).attr("name");
                    if (CtrlName != null) {
                        $('#' + FormId).bootstrapValidator('revalidateField', CtrlName);
                    }
                });

            }
        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
            //alert(e.target.value);

        }).on('blur', function (e) {
            var datepickerID = e.currentTarget.id;
            setTimeout(
               function () {
                   if ($('#' + datepickerID).val() != "")
                       EMRUtility.ValidateDateYearView('#' + datepickerID);
               }, 100);

        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                EMRUtility.YearViewAutoCompleteDate(this, startDate, endDate);
            }
        });

        if (isCurrentDate)
            $('#' + controlId).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
    },


    YearViewAutoCompleteDate: function (obj, startDate, endDate) {
        var DateLength = $(obj).val().length;
        var CtrlValue = $(obj).val();
        if (CtrlValue.length > 0) {
            var minDate = new Date();
            var maxDate = new Date();
            if (startDate != null)
                minDate = new Date(startDate);
            if (endDate != null)
                maxDate = new Date(endDate);

            if (CtrlValue.length > 4) {
                var DateValue = new Date(CtrlValue);
                if (DateValue < minDate || DateValue > maxDate) {
                    $(obj).val("");
                    //alert("value reset");
                }
            }

        }
    },

    isValidYearDate: function (str) {
        if (str == "" || str == null) { return false; }
        var m = null;
        var ret = true;
        var maxYear = new Date().getFullYear();  //MAX YEAR;
        var minYear = 1800; //MIN YEAR
        var mmIndex, ddIndex, yyIndex;
        var sign = date_format.replace(/[a-zA-Z0-9]/g, '')[0];
        str = str.replace(sign, '-');
        str = str.replace(sign, '-');
        var helperFomat = date_format.replace(sign, '-');
        helperFomat = helperFomat.replace(sign, '-');
        helperFomat = helperFomat.match(/(\w{2})-(\w{2})-(\w{4})/);
        m = str.match(/(\w{4})/);
        mmIndex = helperFomat.indexOf('mm');
        ddIndex = helperFomat.indexOf('dd');
        yyIndex = helperFomat.indexOf('yyyy');
        if (m === null || typeof m !== 'object') { return false; }

        if (typeof m !== 'object' && m !== null && m.size !== 3) { return false; }

        // YEAR CHECK
        if ((m[1].length < 4) || m[1] < minYear || m[1] > maxYear) { ret = false; }
        // MONTH CHECK
        //if ((m[mmIndex].length < 2) || m[mmIndex] < 1 || m[mmIndex] > 12) { ret = false; }
        //// DAY CHECK
        //if ((m[ddIndex].length < 2) || m[ddIndex] < 1 || m[ddIndex] > 31) { ret = false; }
        return ret;
    },

    IsNoteUnSign_DB_Call: function (UserId) {
        var data = "UserID=" + UserId;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "CHECK_USER_PERMISSIONS_OF_UNSIGN_NOTE");
    },

    CheckPatientIsRegisteredOnDrFirst_DB_Call: function (PatientId) {
        var objData = {};
        objData["PatientId"] = PatientId;
        objData["commandType"] = "Check_Patient_Register_On_DrFirst";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Rcopia");
    },

    CheckUserHaveRcopiaRights_DB_Call: function (UserId) {
        var objData = {};
        objData["UserId"] = UserId;
        objData["commandType"] = "Check_User_Have_Rcopia_Rights";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Rcopia");
    },

    getUpdatedHtml: function (div) {
        div.find("input, select, textarea").each(function () {
            var $this = $(this);
            if ($this.is("[type='radio']") || $this.is("[type='checkbox']")) {
                if ($this.prop("checked")) {
                    $this.attr("checked", "checked");
                }
            } else {
                if ($this.is("select")) {
                    $this.find(":selected").attr("selected", "selected");
                } else {
                    $this.attr("value", $this.val());
                }
            }
        });
        return div.html();
    },

    changeIcon: function (e) {
        for (var i = 0; i < e.length; i++) {
            if ($(e[i]).parent().prev().hasClass('active') == true) {
                $(e[i]).html('');
                $(e[i]).html('<i class="fa fa-angle-up"></i>');
            }
            else {
                $(e[i]).html('');
                $(e[i]).html('<i class="fa fa-angle-down"></i>');
            };
        }
    },

    CheckPnlOpen: function (ctrlId) {
        if ($('[id*=' + ctrlId + ']:visible').length > 0) {
            return true;
        } else {
            return false;
        }
    },

    AutoKeyWordPopulate: function (PanelId, Ctrl, ComponentName, NoteComponentId) {
        var text = $("#" + PanelId + " #" + Ctrl).text();
        if (text && text.length > 2 && text.slice(-2) === "..") {
            // add marker for autocomplete
            var txt = text + '<span id=marker></span>';
            $("#" + PanelId + " #" + Ctrl).append('<span id=marker></span>');
            var splitted = text.slice(0, -2);
            // splitted = splitted.replace(/\r?\n/gi, " ");
            splitted = splitted.split("").reverse();
            var keyword = [];
            for (i = 0; i < splitted.length; i++) {
                if (splitted[i] != " " && splitted[i] != ">") {
                    keyword.push(splitted[i]);
                }
                else { break; }
            }
            keyword = keyword.reverse().join("");

            EMRUtility.GetMacros(null, null, keyword, ComponentName, NoteComponentId).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var pressedkey = keyword;

                        if (response.MacroDetails.length > 0) {

                            if (response.MacroDetails.length == 1) {
                                var existingString = $("#" + PanelId + " #" + Ctrl).text();
                                existingString = existingString.replace(keyword + "..", response.MacroDetails[0].Description);
                                $("#" + PanelId + " #" + Ctrl).text(existingString);
                                $("#" + PanelId + " #" + Ctrl).find('#marker').remove();
                            }
                            else {
                                $("#" + PanelId + " #MacroDescriptions").empty();
                                $.each(response.MacroDetails, function (i, item) {

                                    $("#" + PanelId + " #MacroDescriptions").append('<li><button onclick="EMRUtility.BindDescriptions(' + "'" + item.Description.replace(/\r?\n/gi, " ") + "','" + pressedkey + "','" + PanelId + "','" + Ctrl + "'" + ');" type="button">' + item.Description + '</button></li>');
                                    //$("#" + PanelId + " #MacroDescriptions").append('<li><button onclick="EMRUtility.BindDescriptions(\'' + item.Description.replace(/\r?\n/gi, "<br>") + '\',\'' + pressedkey + '\',\'' + PanelId + '\',\'' + Ctrl +'\''+ '\');" type="button">' + item.Description + '</button></li>');
                                });
                                var childPos = $("#" + PanelId + " #" + Ctrl).find('#marker').offset();
                                var parentPos = $("#" + PanelId + " #" + Ctrl).find('#marker').parent().offset();
                                var childOffset = {
                                    top: childPos.top - parentPos.top,
                                    left: childPos.left - parentPos.left
                                }
                                $("#" + PanelId + " #MacroDescDetails").css("top", childOffset.top + 25);
                                $("#" + PanelId + " #MacroDescDetails").css("left", childOffset.left + 20);
                                $("#" + PanelId + " #MacroDescDetails").show("slow");
                                $("#" + PanelId + " #MacroDescDetails").click();
                                $("#" + PanelId + " #MacroDescDetails").focusout(function () { $("#" + PanelId + " #MacroDescDetails").hide(); });
                            }
                        }
                        else {

                            $("#" + PanelId + " #" + Ctrl).find('#marker').remove();
                        }
                    }
                }
            });
        }
    },
    GetMacros: function (MacroId, MacroName, Keyword, ComponentName, NoteComponentId) {
        var data = {};
        if (MacroId != null) {
            data["MacroId"] = MacroId;
        }
        if (MacroName != null) {
            data["MacroName"] = MacroName;
        }
        data["Keyword"] = Keyword;
        data["UserId"] = globalAppdata.AppUserId;
        data["NoteComponentsNames"] = ComponentName;
        data["NoteComponentId"] = NoteComponentId;
        data["commandType"] = "Search_MacroDetailsForNotes";
        var obj = JSON.stringify(data);
        return MDVisionService.APIService(obj, "Macro", "Macro");
    },
    BindDescriptions: function (Description, Keyword, PanelId, Ctrl) {
        var existingString = $("#" + PanelId + " #" + Ctrl).text();
        existingString = existingString.replace(Keyword + "..", Description);
        $("#" + PanelId + " #" + Ctrl).text(existingString);
        $("#" + PanelId + " #" + Ctrl).find("#marker").remove();
        $("#" + PanelId + " #MacroDescDetails").hide();
    },


    callAutopopulationOrNot: function (PanelId, Ctrl) {
        var html = $("#" + PanelId + " #" + Ctrl).html();
        var index = html.indexOf('<span id="marker"></span>');
        if (index > 2 && html[index - 1] == "." && html[index - 2] == ".") {
            return true;
        }
        else {
            return false;
        }
    },
    getKeyWord: function (PanelId, Ctrl) {
        var keyWord = [];
        var html = $("#" + PanelId + " #" + Ctrl).html();
        html = html.replace(/\r?\n/gi, "<br>");
        var index = html.indexOf('<span id="marker"></span>');
        for (var a = index - 1; a > -1; a--) {
            if (html[a] == " " || html[a] == ">") {
                break;
            }
            else {
                if (html[a] != ".") {
                    keyWord.push(html[a]);
                }

            }
        }
        keyWord = keyWord.reverse();
        return keyWord.join("");
    },
    AutoKeyWordPopulateForDiv: function (PanelId, Ctrl, ComponentName, NoteComponentId) {

        var keyword = EMRUtility.getKeyWord(PanelId, Ctrl);
        if (keyword != "") {
            EMRUtility.GetMacros(null, null, keyword, ComponentName, NoteComponentId).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        if (response.MacroDetails.length > 0) {

                            if (response.MacroDetails.length == 1) {
                                EMRUtility.pasteHtmlAtCaret(response.MacroDetails[0].Description.replace(/\r?\n/gi, "<br>"));
                                var divhtml = $("#" + PanelId + " #" + Ctrl).html();
                                $("#" + PanelId + " #" + Ctrl).html(divhtml.replace(keyword + "..", ""));
                                $("#" + PanelId + " #" + Ctrl).find("#marker").remove();
                                EMRUtility.placeCaretAtEnd(Ctrl);
                            }
                            else {
                                $("#" + PanelId + " #MacroDescriptions").empty();
                                $.each(response.MacroDetails, function (i, item) {

                                    $("#" + PanelId + " #MacroDescriptions").append('<li><button onclick="EMRUtility.BindDescriptionsForDiv(\'' + keyword + '\',\'' + PanelId + '\',\'' + Ctrl + '\',this);" type="button">' + item.Description + '</button><span class="hidden">' + item.Description.replace(/\r?\n/gi, "<br>") + '<span></li>');
                                });
                                var childPos = $("#" + PanelId + " #" + Ctrl).find('#marker').offset();
                                var parentPos = $("#" + PanelId + " #" + Ctrl).find('#marker').parent().offset();
                                var childOffset = {
                                    top: childPos.top - parentPos.top,
                                    left: childPos.left - parentPos.left
                                }
                                $("#" + PanelId + " #MacroDescDetails").css("top", childOffset.top + 25);
                                $("#" + PanelId + " #MacroDescDetails").css("left", childOffset.left + 20);
                                $("#" + PanelId + " #MacroDescDetails").show("slow");
                                $("#" + PanelId + " #MacroDescDetails").click();
                                $("#" + PanelId + " #MacroDescDetails").focusout(function () { $("#" + PanelId + " #MacroDescDetails").hide(); });
                            }
                        }
                        else {
                            $("#" + PanelId + " #" + Ctrl).find('#marker').remove();
                        }
                    }
                }
            });
        }
    },
    BindDescriptionsForDiv: function (Keyword, PanelId, Ctrl, obj) {

        EMRUtility.pasteHtmlAtCaret($(obj).parent().find("span").html());
        var divhtml = $("#" + PanelId + " #" + Ctrl).html();
        $("#" + PanelId + " #" + Ctrl).html(divhtml.replace(Keyword + "..", ""));
        $("#" + PanelId + " #" + Ctrl).find("#marker").remove();
        $("#" + PanelId + " #MacroDescDetails").hide();
        EMRUtility.placeCaretAtEnd(Ctrl);
    },
    pasteHtmlAtCaret: function (html) {
        var sel, range;
        if (window.getSelection) {
            // IE9 and non-IE
            sel = window.getSelection();
            if (sel.getRangeAt && sel.rangeCount) {
                range = sel.getRangeAt(0);
                range.deleteContents();

                // Range.createContextualFragment() would be useful here but is
                // non-standard and not supported in all browsers (IE9, for one)
                var el = document.createElement("div");
                el.innerHTML = html;
                var frag = document.createDocumentFragment(), node, lastNode;
                while ((node = el.firstChild)) {
                    lastNode = frag.appendChild(node);
                }
                range.insertNode(frag);

                // Preserve the selection
                if (lastNode) {
                    range = range.cloneRange(); range.setStartAfter(lastNode); range.collapse(true); sel.removeAllRanges(); sel.addRange(range);
                }
            }
        } else if (document.selection && document.selection.type != "Control") { // IE < 9 
            document.selection.createRange().pasteHTML(html);
        }
    },
    //'#' + Clinical_Vitals.params.PanelID + ' #txtComment'
    placeCaretAtEnd: function (Ctrl) {
        var el = document.getElementById(Ctrl)
        el.focus();
        if (typeof window.getSelection != "undefined"
        && typeof document.createRange != "undefined") {
            var range = document.createRange();
            range.selectNodeContents(el);
            range.collapse(false);
            var sel = window.getSelection();
            sel.removeAllRanges();
            sel.addRange(range);
        } else if (typeof document.body.createTextRange != "undefined") {
            var textRange = document.body.createTextRange();
            textRange.moveToElementText(el);
            textRange.collapse(false);
            textRange.select();
        }
    }
};

$(document).on('focusin', function (e) {
    if ($(e.target).closest(".mce-window").length) {
        e.stopImmediatePropagation();
    }
});
//////-------------Editable Data Grid Scripts Strats-----------------------

EMREditableGrid = {

    options: {
        table: '#dgvICD',
        dialog: {
            wrapper: '#dialog',
            cancelButton: '#dialogCancel',
            confirmButton: '#dialogConfirm',
        }
    },
    // added new paramater for DOM addition in Datatables(sDom)
    initialize: function (GridId, ClassName, AddDefaultRow, bInfo, bFilter, bPaginate, bSort, iPageSize, sDom) {
        EMREditableGrid.options.table = GridId;
        EMREditableGrid.setVars()
        EMREditableGrid.build(bInfo, bFilter, bPaginate, bSort, iPageSize, sDom)
        EMREditableGrid.events(ClassName);
        if (AddDefaultRow != "0") {
            EMREditableGrid.rowAdd("-1");
        }
    },

    setVars: function () {
        EMREditableGrid.$table = $(EMREditableGrid.options.table);

        // dialog
        EMREditableGrid.dialog = {};
        EMREditableGrid.dialog.$wrapper = $(EMREditableGrid.options.dialog.wrapper);
        EMREditableGrid.dialog.$cancel = $(EMREditableGrid.options.dialog.cancelButton);
        EMREditableGrid.dialog.$confirm = $(EMREditableGrid.options.dialog.confirmButton);

        return EMREditableGrid;
    },
    // added new paramater for DOM addition in Datatables(sDom)
    build: function (bInfo, bFilter, bPaginate, bSort, iPageSize, sDom) {
        var TotalColumn = [];//['','','',''];
        if (sDom != null) {
            TotalColumn[0] = { "bInfo": (bInfo != null ? bInfo : true), "bFilter": (bFilter != null ? bFilter : true), "bPaginate": (bPaginate != null ? bPaginate : true), "bSort": (bSort != null ? bSort : false), "iDisplayLength": (iPageSize != null ? iPageSize : 10), "bDestroy": true, "sDom": sDom };
        } else {
            TotalColumn[0] = { "bInfo": (bInfo != null ? bInfo : true), "bFilter": (bFilter != null ? bFilter : true), "bPaginate": (bPaginate != null ? bPaginate : true), "bSort": (bSort != null ? bSort : false), "iDisplayLength": (iPageSize != null ? iPageSize : 10), "bDestroy": true };
        }


        //TotalColumn[1] = null;
        var count = 1;
        $(EMREditableGrid.options.table + " tr th").each(function () {
            TotalColumn[count] = null;
            count++;
        });

        EMREditableGrid.datatable = EMREditableGrid.$table.DataTable(TotalColumn[0], TotalColumn);

        window.dt = EMREditableGrid.datatable;

        return EMREditableGrid;
    },

    events: function (ClassName) {
        var _self = EMREditableGrid;

        EMREditableGrid.$table.off()
            .on('click', 'a.expand-row', function (e) {
                e.preventDefault();
                //_self.rowExpand($(this).closest('tr'), ClassName);
                EMREditableGrid.ChangeCurrentSelectedGridObjRowExpand($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.save-row', function (e) {
                e.preventDefault();

                // _self.rowSave($(this).closest('tr'), ClassName);
                EMREditableGrid.ChangeCurrentSelectedGridObjRowSave($(this).closest('tr'), ClassName)
            })
            .on('click', 'a.cancel-row', function (e) {
                e.preventDefault();
                _self.rowCancel($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.up-row', function (e) {
                e.preventDefault();
                _self.rowUp($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.down-row', function (e) {
                e.preventDefault();
                _self.rowDown($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.title-row', function (e) {
                e.preventDefault();
                _self.rowTitle($(this).closest('tr'));
            })
            .on('click', 'a.edit-row', function (e) {
                e.preventDefault();

                //_self.rowEdit($(this).closest('tr'));
                EMREditableGrid.ChangeCurrentSelectedGridObjRowEdit($(this).closest('tr'));
            }).on('click', 'a.row-detail', function (e) {
                e.preventDefault();
                _self.rowDetail($(this).closest('tr'), ClassName);
            }).on('click', 'a.row-history', function (e) {
                e.preventDefault();
                _self.rowHistory($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.remove-row', function (e) {
                e.preventDefault();

                var $row = $(this).closest('tr');

                //  _self.rowRemove($row, ClassName);
                EMREditableGrid.ChangeCurrentSelectedGridObjRowRemove($row, ClassName);
                //$.magnificPopup.open({
                //	items: {
                //		src: '#dialog',
                //		type: 'inline'
                //	},
                //	preloader: false,
                //	modal: true,
                //	callbacks: {
                //		change: function() {
                //			_self.dialog.$confirm.on( 'click', function( e ) {
                //				e.preventDefault();

                //				_self.rowRemove( $row );
                //				$.magnificPopup.close();
                //			});
                //		},
                //		close: function() {
                //			_self.dialog.$confirm.off( 'click' );
                //		}
                //	}
                //});
            })
            .on('click', 'a.remove-active', function (e) {
                e.preventDefault();

                var $row = $(this).closest('tr');

                _self.rowInactive($row, ClassName);

            }).on('click', 'a.add-child-row', function (e) {
                e.preventDefault();

                _self.rowAddChild($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.row-Payment', function (e) {
                e.preventDefault();
                _self.rowPayment($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.row-PaymentDetail', function (e) {
                e.preventDefault();
                _self.rowPaymentDetail($(this).closest('tr'), ClassName);
            })
            //EMR-1268 fix by azhar on 6/28/2016 4:12pm
            .on('click', 'th', function (e) {
                e.stopPropagation();
                //e.preventDefault();
                var iS_sort = $(this).hasClass('sorting');
                var iS_sort_asc = $(this).hasClass('sorting_asc');
                var iS_sort_desc = $(this).hasClass('sorting_desc');
                var item = this;
                if (iS_sort || iS_sort_desc || iS_sort_asc) {
                    $(this).parent().find('th').each(function (index, element) {
                        $(element).removeClass('sorting_asc');
                        $(element).removeClass('sorting_desc');
                        $(element).removeClass('sorting');
                        if (item != element) {
                            $(element).addClass('sorting');
                        }
                    });
                    if (iS_sort || iS_sort_desc) {
                        $(item).addClass('sorting_asc');
                    } else if (iS_sort_asc) {
                        $(item).addClass('sorting_desc');
                    }
                }
                //_self.rowUp($(this).closest('tr'), ClassName);
            });

        


        EMREditableGrid.dialog.$cancel.on('click', function (e) {
            e.preventDefault();
            $.magnificPopup.close();
        });

        return this;
    },



    // ==========================================================================================
    // ROW FUNCTIONS
    // ==========================================================================================

    rowAddChild: function ($row, ClassName) {
        ClassName.rowAddChild($row, EMREditableGrid);
    },

    rowAdd: function (RowId, VisitId, VitalLatestBPId, VitalLatestPulseId, VitalLatestTempId, VitalLatestRespId, VitalNotesId, NegationReasonId, BPNegationReasonId) {

        var actions = [],
            data,
            $row;
        var AppendColumn = [];//['','','',''];
        //AppendColumn[0] = '<a href="#" class="hidden on-editing title-row" title="Save Title" ><i class="fa fa-plus-square-o"></i></a>';
        var count = 0;
        $(EMREditableGrid.options.table + " tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var ActionCount = 0;
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                }

                if ($.inArray("Cancel", arrActionType) != -1) {
                    actions[ActionCount] = '<a class="btn-xs hidden on-editing cancel-row mr-none btn" title="Cancel Record"><i class="fa fa-close black"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Save", arrActionType) != -1) {
                    actions[ActionCount] = '<a href="#" class="btn-xs hidden on-editing save-row mr-none btn" title="Save Record" ><i class="fa fa-save green"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("SaveTitle", arrActionType) != -1) {

                    actions[ActionCount] = '<a href="#" class="btn-xs hidden on-editing title-row mr-none btn" title="Save Title" ><i class="fa fa-paste black"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Delete", arrActionType) != -1) {

                    actions[ActionCount] = '<a href="#" class="btn-xs on-default remove-row mr-none btn" title="Delete Record" ><i class="fa fa-close red"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Edit", arrActionType) != -1) {

                    actions[ActionCount] = '<a href="#" class="btn-xs on-default edit-row mr-none btn" title="Edit Record" ><i class="fa fa-edit black"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Up", arrActionType) != -1) {

                    actions[ActionCount] = '<a href="#" class="btn-xs on-default up-row mr-none btn" title="Up Record" ><i class="fa fa-arrow-up black"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Down", arrActionType) != -1) {

                    actions[ActionCount] = '<a href="#" class="btn-xs on-default down-row mr-none btn" title="Down Record" ><i class="fa fa-arrow-down black"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Detail", arrActionType) != -1) {
                    if (RowId > 0) {
                        actions[ActionCount] = '<a href="#" class="btn-xs on-editing row-detail mr-none btn" title="Record Detail" ><i class="fa fa-book blue"></i></a>';
                        ActionCount++;
                    }
                }
                if ($.inArray("History", arrActionType) != -1) {
                    if (RowId > 0) {
                        actions[ActionCount] = '<a href="#" class="btn-xs on-editing row-history mr-none btn" title="Activity Log" ><i class="fa fa-history blue"></i></a>';
                        ActionCount++;
                    }
                }
                if ($.inArray("Active", arrActionType) != -1) {

                    var IsCheckedIn = null;
                    IsCheckedIn = $('#pnlClinicalProblemLists #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');

                    var toggleClass = "";
                    if (IsCheckedIn == "1") {
                        toggleClass = 'class = "fa fa-toggle-on green"'
                    } else {
                        toggleClass = 'class = "fa fa-toggle-on red"'
                    }

                    actions[ActionCount] = '<a href="#" class="btn-xs on-default remove-active mr-none btn" title="Record Active/Inactive" ><i ' + toggleClass + '></i></a>';
                    ActionCount++;
                }
                AppendColumn[count] = actions.join(' ');
            }
            else if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "expand") {
                AppendColumn[count] = '<div class="spacer3"></div><a href="#" class="btn-xs hidden on-editing expand-row  mr-none btn" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
            }
            else
                AppendColumn[count] = '';
            count++;
        });

        data = EMREditableGrid.datatable.row.add(AppendColumn);
        $row = EMREditableGrid.datatable.row(data[0]).nodes().to$();
        $row.addClass('adding');
        $row.attr("visitid", VisitId);
        if (VitalLatestBPId != null) {
            $row.attr("VitalLatestBPId", VitalLatestBPId);
        }
        if (VitalLatestPulseId != null) {
            $row.attr("VitalLatestPulseId", VitalLatestPulseId);
        }
        if (VitalLatestTempId != null) {
            $row.attr("VitalLatestTempId", VitalLatestTempId);
        }
        if (VitalLatestRespId != null) {
            $row.attr("VitalLatestRespId", VitalLatestRespId);
        }
        if (VitalNotesId != null) {
            $row.attr("VitalNotesId", VitalNotesId);
        }
        if (NegationReasonId != null) {
            $row.attr("NegationReasonId", NegationReasonId);
        }
        if (NegationReasonId != null) {
            $row.attr("BPNegationReasonId", BPNegationReasonId);



        }
        if (RowId) {
            $row.attr("id", RowId);
        }
        else
            $row.attr("id", "-1");
        EMREditableGrid.rowEdit($row);
        EMREditableGrid.datatable.order([0, 'dsc']).draw(); // always show fields
        return $row;
    },

    rowCancel: function ($row, ClassName) {
        ClassName.rowCancel($row, EMREditableGrid);
    },

    rowEdit: function ($row) {
        var _self = this,
            data;
        data = this.datatable.row($row.get(0)).data();
        var RowId = $($row).attr("id");
        var VisitId = $($row).attr("visitid");
        if (!RowId) {
            RowId = "-1";//0 means it's new row
            $($row).attr("id", "-1");
        }

        $(EMREditableGrid.options.table + " tr th").each(function (i) {
            var $this = $row.find('td:nth-child(' + (i + 1) + ')');
            var controlId = $(this).attr("controlid");
            if (!controlId) {
                controlId = "";
            }
            var controlName = $(this).attr("controlname");
            var appendidaftername = ($(this).attr("appendidaftername") != null && $(this).attr("appendidaftername") == "1") ? "1" : "0";
            if (!controlName) {
                controlName = "";
            }
            var controldisabledClass = $(this).attr("iscontroldisabled") == "1" ? "disabled" : "";
            var coltype = $(this).attr("coltype");
            var isoptional = $(this).attr("isoptional");
            var subcols = $(this).attr("subcols");
            var onfocussout = $(this).attr("onfocusout");//$(this).attr("onfocusout") != null ? $(this).attr("onfocusout") : ($(this).attr("onblur") != null ? $(this).attr("onblur") : "");
            if (onfocussout != null && $(this).attr("onblur")) {
                onfocussout = $(this).attr("onblur");
            }
            var onkeypress = $(this).attr("onkeypress");
            if (!isoptional) {
                isoptional = "1";
            }
            var PlaceHolder = $(this).attr("placeholder");
            $this.attr("isoptional", isoptional);
            if (coltype && coltype.toLowerCase() == "expand") {
                $this.html('<div class="spacer3"></div><a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>');
                $this.addClass('expand');
            }
            else if (coltype && coltype.toLowerCase() == "action") {
                _self.rowSetActionsEditing($row);
                //$row.addClass('adding')
                //$row.find('td:first').addClass('actions')
                $this.addClass('actions');
            }
            else if (coltype && coltype.toLowerCase() == "textbox") {
                var controlwidth = $(this).attr("controlwidth");
                if (controlwidth != null)
                    ;
                else
                    controlwidth = "size20 p-none";
                if (subcols && subcols != "") {
                    $this.attr("subcols", subcols);
                    var TextBoxes = "";
                    for (var k = 1; k <= subcols ; k++) {
                        var CtrlId = controlId + k + "" + RowId;
                        var CtrlName = controlName + k;
                        if (k != 1) {// to make all controls as required except first control
                            isoptional = "1";
                        }
                        var inputFocussOut = "";
                        if (onfocussout && onfocussout != "") {
                            var body = onfocussout.substring(1, onfocussout.lastIndexOf("("));
                            var Parameters = onfocussout.substring(onfocussout.lastIndexOf("(") + 1, onfocussout.lastIndexOf(")"));
                            if (Parameters != "") {
                                var arrParameters = Parameters.split(',');
                                // conflict with previous paramater bug fix by azhar on 10/12/2015
                                for (var m = 0; m < arrParameters.length; m++) {
                                    arrParameters[m] = "'" + String(arrParameters[m]).trim().replace(/'/gi, "") + k + "" + RowId + "'";
                                }
                                body += "(" + arrParameters.join(',') + ")";
                            }
                            else
                                body += "()";
                        }
                        if (!onkeypress) {
                            onkeypress = "";
                        }
                        var attrAutoComplete = $(this).attr("autocompletemethod");
                        var AutoCompleteSearch = "";
                        if (attrAutoComplete && attrAutoComplete != "") {
                            AutoCompleteSearch = EMREditableGrid.getAutoCompleteSearch(attrAutoComplete, CtrlId);
                            if (attrAutoComplete == "Modifier") {
                                body = "utility.ValidateCode(this, 'Modifier','" + CtrlId + "');";
                            }

                            else if (attrAutoComplete == "icd") {
                                body = "utility.ValidateCode(this, 'icd','" + CtrlId + "');";
                            }

                            else if (attrAutoComplete == "cpt") {
                                body = "utility.ValidateCode(this, 'cpt','" + CtrlId + "');";
                            }
                            else if (attrAutoComplete == "POS") {
                                body = "utility.ValidateAutoComplete(this, '" + CtrlId + "', false, '1');";
                            }



                        }
                        //var hiddenField = "";
                        //var hfcontrolid = $(this).attr("hfcontrolid");
                        //var hiddenfieldId = "";
                        //if (hfcontrolid != null) {
                        //    hiddenfieldId = hfcontrolid;
                        //    hiddenField = "<input type='hidden' id='"+hfcontrolid+"' value=0 />";
                        //}

                        var arrHiddenFields = $(this).attr("hfcontrolid");
                        var arrHiddenFieldsNames = $(this).attr("hfcontrolname") != null ? $(this).attr("hfcontrolname") : "";
                        var currencyicon = $(this).attr("currencyicon") != null ? $(this).attr("currencyicon") : "";
                        var hiddenField = "";
                        if (arrHiddenFields != null && arrHiddenFields != "") {
                            if (arrHiddenFields.indexOf(",") > -1) {
                                arrHiddenFields = arrHiddenFields.split(",");
                                arrHiddenFieldsNames = arrHiddenFieldsNames.split(",");
                                // conflict with previous paramater bug fix by azhar on 10/12/2015
                                for (var n = 0; n < arrHiddenFields.length; n++) {
                                    var hfcontrolid = arrHiddenFields[n];
                                    var hiddenfieldId = "";
                                    if (hfcontrolid != null) {
                                        hiddenfieldId = hfcontrolid;
                                        var hfcontrolname = arrHiddenFieldsNames[n];
                                        var hiddenfieldname = "";
                                        if (hfcontrolname != null) {
                                            hiddenfieldname = hfcontrolname;
                                        }
                                        hiddenField += "<input type='hidden' id='" + (hfcontrolid + k + RowId) + "' name='" + hiddenfieldname + "' value=0 />";
                                    }
                                }
                            }
                            else {
                                var hfcontrolid = arrHiddenFields;
                                var hiddenfieldId = "";
                                var hiddenfieldName = "";
                                if (hfcontrolid != null) {
                                    hiddenfieldId = hfcontrolid;
                                    if (arrHiddenFieldsNames != null) {
                                        hiddenfieldName = arrHiddenFieldsNames;
                                    }
                                    hiddenField += "<input type='hidden' id='" + (hfcontrolid + k + RowId) + "' name='" + hiddenfieldName + "' value=0 />";
                                }
                            }

                        }

                        var CurrencyStartHTML = "";
                        var CurrencyEndHTML = "";
                        if (currencyicon != "") {
                            CurrencyStartHTML = "<div class='input-group'><span class='input-group-addon xxs-font pl-tiny pr-tiny'><i class='" + currencyicon + "'></i></span>";
                            CurrencyEndHTML = "</div>";
                        }
                        if (PlaceHolder) {
                            PlaceHolder = " placeholder=" + PlaceHolder;
                        } else {
                            PlaceHolder = "";
                        }
                        TextBoxes += CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" type="text" ' + controldisabledClass + ' oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control pull-left ' + controlwidth + ' mr-xxs" value="' + data[i] + '" ' + PlaceHolder + '/>' + CurrencyEndHTML + hiddenField;
                    }
                    $this.append(TextBoxes);
                }
                else {
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;
                    if (appendidaftername == "1") {
                        CtrlName = controlName + RowId;
                    }

                    var inputFocussOut = "";
                    if (onfocussout && onfocussout != "") {
                        var body = onfocussout.substring(0, onfocussout.lastIndexOf("("));
                        if (body == null)
                            body = "";
                        var Parameters = onfocussout.substring(onfocussout.lastIndexOf("(") + 1, onfocussout.lastIndexOf(")"));
                        if (Parameters != "") {
                            var arrParameters = Parameters.split(',');
                            // conflict with previous paramater bug fix by azhar on 10/12/2015
                            for (var p = 0; p < arrParameters.length; p++) {
                                arrParameters[p] = "'" + String(arrParameters[p]).trim().replace(/'/gi, "") + RowId + "'";
                            }
                            body += "(" + arrParameters.join(',') + ")";
                        }
                        else
                            body += "()";
                    }

                    if (!onkeypress) {
                        onkeypress = "";
                    }
                    var attrAutoComplete = $(this).attr("autocompletemethod");
                    var AutoCompleteSearch = "";
                    if (attrAutoComplete) {
                        AutoCompleteSearch = EMREditableGrid.getAutoCompleteSearch(attrAutoComplete, CtrlId);
                        if (attrAutoComplete == "Modifier") {
                            body = "utility.ValidateCode(this, 'Modifier','" + CtrlId + "');";
                        }

                        else if (attrAutoComplete == "icd") {
                            body = " utility.ValidateCode(this, 'icd','" + CtrlId + "');";
                        }

                        else if (attrAutoComplete == "cpt") {
                            body = " utility.ValidateCode(this, 'cpt','" + CtrlId + "');";
                        }
                        else if (attrAutoComplete == "POS") {
                            body = " utility.ValidateAutoComplete(this, '" + CtrlId + "', false, '1');";
                        }
                    }
                    var arrHiddenFields = $(this).attr("hfcontrolid");
                    var arrHiddenFieldsNames = $(this).attr("hfcontrolname") != null ? $(this).attr("hfcontrolname") : "";
                    var currencyicon = $(this).attr("currencyicon") != null ? $(this).attr("currencyicon") : "";
                    var hiddenField = "";
                    if (arrHiddenFields != null && arrHiddenFields != "") {
                        if (arrHiddenFields.indexOf(",") > -1) {
                            arrHiddenFields = arrHiddenFields.split(",");
                            arrHiddenFieldsNames = arrHiddenFieldsNames.split(",");
                            // conflict with previous paramater bug fix by azhar on 10/12/2015
                            for (var q = 0; q < arrHiddenFields.length; q++) {
                                var hfcontrolid = arrHiddenFields[q];
                                var hiddenfieldId = "";
                                if (hfcontrolid != null) {
                                    hiddenfieldId = hfcontrolid;
                                    var hfcontrolname = arrHiddenFieldsNames[q];
                                    var hiddenfieldname = "";
                                    if (hfcontrolname != null) {
                                        hiddenfieldname = hfcontrolname;
                                    }
                                    hiddenField += "<input type='hidden' id='" + (hfcontrolid + RowId) + "' name='" + hiddenfieldname + "' value=0 />";
                                }
                            }
                        }
                        else {
                            var hfcontrolid = arrHiddenFields;
                            var hiddenfieldId = "";
                            var hiddenfieldName = "";
                            if (hfcontrolid != null) {
                                hiddenfieldId = hfcontrolid;
                                if (arrHiddenFieldsNames != null) {
                                    hiddenfieldName = arrHiddenFieldsNames;
                                }
                                hiddenField += "<input type='hidden' id='" + (hfcontrolid + RowId) + "' name='" + hiddenfieldName + "' value=0 />";
                            }
                        }

                    }
                    var CurrencyStartHTML = "";
                    var CurrencyEndHTML = "";
                    if (currencyicon != "") {
                        CurrencyStartHTML = "<div class='input-group'><span class='input-group-addon xxs-font pl-tiny pr-tiny'><i class='" + currencyicon + "'></i></span>";
                        CurrencyEndHTML = "</div>";
                    }
                    if (PlaceHolder) {
                        PlaceHolder = " placeholder=" + PlaceHolder;
                    } else {
                        PlaceHolder = "";
                    }
                    if (CtrlId.indexOf('TaskDuration') >= 0) {
                        var duration = parseFloat(data[i].split(' ')[0]);
                        var hours = parseInt(duration / 60);
                        duration = duration % 60;
                        var mints = parseInt(duration);
                        var secs = Math.round((duration - mints) * 60);
                        var mstr = '<div class="col-xs-12 recording-time recording-small">' +
                           '<div id="divTaskHours" class="col-xs-4"><input type="number" value="' + hours + '" id="txtTaskHours" name="TaskHours"  min="0" max="24" placeholder="H" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '"></div>' +
                           '<div id="divTaskMinutes" class="col-xs-4 dot"><input type="number" value="' + mints + '" id="txtTaskMinutes" name="TaskMinutes" min="0" max="59" placeholder="M" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '"></div>' +
                           '<div id="divTaskSeconds" class="col-xs-4"><input type="number" value="' + secs + '" id="txtTaskSeconds" name="TaskSeconds" min="10" max="59" placeholder="S" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '"></div>' +
                           '</div>';
                        $this.html(mstr);
                        // $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block size-min75 p-tiny" value="' + data[i] + '" ' + PlaceHolder + '/>' + CurrencyEndHTML + hiddenField);
                    }
                    else if (CtrlId.indexOf('CallDetailsDuration') >= 0) {
                        var val = data[i].split(' ');
                        var duration = parseFloat(val[0]).toFixed(2);
                        //var unit = val[1];
                        var mstr = '<div class="row">' +
                             '<div class="col-sm-5">' +
                                 '<input id="txtTaskDuration" name="Duration" value="' + duration + '" type="number" min="0" class="form-control">' +
                             '</div>' +
                             '<div class="col-sm-7">' +
                                 '<select id="ddlTaskDurationUnit" name="DurationUnit" class="form-control">' +
                                     '<option value="seconds">Second(s)</option>' +
                                     '<option selected="selected" value="minutes">Minute(s)</option>' +
                                     '<option value="hours">Hour(s)</option>' +
                                 '</select>' +
                             '</div>' +
                         '</div>';
                        $this.html(mstr);

                        //$("#ddlTaskDurationUnit > [value=" + unit + "]").attr("selected", "true");

                        // $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block size-min75 p-tiny" value="' + data[i] + '" ' + PlaceHolder + '/>' + CurrencyEndHTML + hiddenField);
                    }
                    else if (CtrlId.indexOf('txtTotalFEE') >= 0)
                        $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block size-min75 p-tiny" value="' + data[i] + '" ' + PlaceHolder + '/>' + CurrencyEndHTML + hiddenField);

                    else if (CtrlId.indexOf('txtUnits') >= 0)
                        $this.html('<input id="' + CtrlId + '" name="' + CtrlName + '" type="text" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block size50 p-tiny" value="' + data[i] + '"/>' + hiddenField);

                    else if (CtrlId.indexOf('txtModifier') >= 0) {
                        $this.html('<input id="' + CtrlId + '" name="' + CtrlName + '" type="text" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block size50 p-tiny" value="' + data[i] + '"/>' + hiddenField);
                        CtrlId = CtrlId.replace("txtpUnits", "");
                    }
                    else if (CtrlId.indexOf('txtINSCharges') >= 0 || CtrlId.indexOf('txtPATCharges') >= 0)
                        $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block size-min75 p-tiny" value="' + data[i] + '" ' + PlaceHolder + '/>' + CurrencyEndHTML + hiddenField);

                    else if (CtrlId.indexOf('txtPOS') >= 0)
                        $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control size30 p-tiny mb-xxs" value="' + data[i] + '" ' + PlaceHolder + '/>' + CurrencyEndHTML + hiddenField);
                    else if (CtrlId.indexOf('txtBPSystolic') >= 0) {
                        $this.html(CurrencyStartHTML + '<div class="input-group size50" style="position:inherit;"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i id="systolicwarningicon" class="fa fa-exclamation-triangle hidden red" style="border-radius:0px;" data-toggle="tooltip" data-html="true" title="" data-original-title="Blood Pressure recorded is higher than the normal parameters as per the standards defined under MIPS Reporting eCQMs applicable only for patients aged 18 years and older:</br></br>i) <b>CMS22V6:</B> [Systolic < 120mmHg || Diastolic < 80 mmHg] </br>ii) <b>CMS65v7:</B> [Systolic < 140 mmHg]</br>iii) <b>CMS165v6:</B> [Systolic < 140 mmHg || Diastolic < 90 mmHg]"></i></span><input id="' + CtrlId + '" spellcheck="true" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block p-tiny" value="' + data[i] + '" ' + PlaceHolder + '/></div>' + CurrencyEndHTML + hiddenField);
                    } else if (CtrlId.indexOf('txtBPDiastolic') >= 0) {
                        $this.html(CurrencyStartHTML + '<div class="input-group size50" style="position:inherit;"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i id="diastolicwarningicon" class="fa fa-exclamation-triangle hidden red" style="border-radius:0px;" data-toggle="tooltip" data-html="true" title="" data-original-title="Blood Pressure recorded is higher than the normal parameters as per the standards defined under MIPS Reporting eCQMs applicable only for patients aged 18 years and older:</br></br>i) <b>CMS22V6:</B> [Systolic < 120mmHg || Diastolic < 80 mmHg] </br>ii) <b>CMS65v7:</B> [Systolic < 140 mmHg]</br>iii) <b>CMS165v6:</B> [Systolic < 140 mmHg || Diastolic < 90 mmHg]"></i></span><input id="' + CtrlId + '" spellcheck="true" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block p-tiny" value="' + data[i] + '" ' + PlaceHolder + '/></div>' + CurrencyEndHTML + hiddenField);
                    } else if (CtrlId.indexOf('txtBMI') >= 0) {
                        $this.html(CurrencyStartHTML + '<div class="input-group size50"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i id="bmiwarningicon" class="fa fa-exclamation-triangle hidden red" style="border-radius:0px;" data-toggle="tooltip" data-html="true" title="" data-original-title="BMI recorded is higher than the normal parameters as per the standards defined under MIPS Reporting eCQMs applicable only for patients aged 18 years and older: <b></br>CMS69v6: [=> 18.5 kg/m<sup>2</sup> || < 25 kg/m<sup>2</sup></b>]"></i></span><input id="' + CtrlId + '" spellcheck="true" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block p-tiny" value="' + data[i] + '" ' + PlaceHolder + '/></div>' + CurrencyEndHTML + hiddenField);
                    }
                    else
                        $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" spellcheck="true" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block p-tiny" value="' + data[i] + '" ' + PlaceHolder + '/>' + CurrencyEndHTML + hiddenField);

                    if (CtrlName.indexOf('Unit') >= 0 && ($row.parent().parent().attr("id")) == "dgvProcedures") {
                        $('#' + Clinical_Procedures.params.PanelID + ' #dgvProcedures').on('keydown', '#' + CtrlId, function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });
                    }
                    if (CtrlId.indexOf('txtpUnits') >= 0) {
                        var id = "$('#" + CtrlId.split('-')[1] + "')";
                        $this.html('<input id="' + CtrlId + '" name="' + CtrlName + '" type="text"  oninput="" onblur="Clinical_Procedures.validateUnits(this)" isoptional="' + isoptional + '" required class="form-control input-block size50 p-tiny" value="' + data[i] + '"/>' + hiddenField);
                    }
                    if (controlName == "StartDueDate" || controlName == "EndOverDueDate" || controlName == "Maxage") {
                        $('#pnlImmunization_ScheduleSetup' + ' #' + CtrlId).attr("maxlength", "6");
                        $('#pnlImmunization_ScheduleSetup').on('keydown', '#' + CtrlId, function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });
                    }
                }
            }
            else if (coltype && coltype.toLowerCase() == "datetime") {
                var subcols = $(this).attr("subcols");
                var controlwidth = $(this).attr("controlwidth");
                if (controlwidth != null)
                    ;
                else
                    controlwidth = "size70";
                if (subcols && subcols != "") {
                    $this.attr("subcols", subcols);
                    var calendar = '<div class="input-group"><span class="input-group-addon pl-tiny pr-tiny xxs-font"><i class="fa fa-calendar"></i></span><input id="dtpDOSFrom" class="form-control " type="text" data-plugin-datepicker=""></div>';
                    var calendars = "";
                    for (var k = 1; k <= subcols ; k++) {
                        var CtrlId = controlId + k + "" + RowId;
                        var CtrlName = controlName + k;
                        if (k != 1) {// to make all controls as required except first control
                            isoptional = "1";
                        }
                        calendars += '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" isoptional="' + isoptional + '" required class="form-control ' + controlwidth + ' p-tiny" value="' + data[i] + '" /></div>';
                    }
                    $this.append(calendars);
                }
                else {
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;
                    $this.html('<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" isoptional="' + isoptional + '" required class="form-control ' + controlwidth + ' p-tiny" value="' + data[i] + '" /></div>');
                    if ((EMREditableGrid.options.table.indexOf("#dgvProblemLists") > -1)) {
                        $('#' + CtrlId).datepicker('setStartDate', $("#banner_PatientDOB").html());
                    }
                }
            }
            else if (coltype && coltype.toLowerCase() == "time") {
                var subcols = $(this).attr("subcols");
                var controlwidth = $(this).attr("controlwidth");
                if (controlwidth != null)
                    ;
                else
                    controlwidth = "size70";
                if (subcols && subcols != "") {
                    $this.attr("subcols", subcols);
                    var calendar = '<div class="input-group"><span class="input-group-addon pl-tiny pr-tiny xxs-font"><i class="fa fa-calendar"></i></span><input id="dtpDOSFrom" class="form-control " type="text" data-plugin-datepicker="" /></div>';
                    var calendars = "";
                    for (var k = 1; k <= subcols ; k++) {
                        var CtrlId = controlId + k + "" + RowId;
                        var CtrlName = controlName + k;
                        if (k != 1) {// to make all controls as required except first control
                            isoptional = "1";
                        }
                        calendars += '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" isoptional="' + isoptional + '" required class="form-control ' + controlwidth + ' p-tiny" value="' + data[i] + '" data-plugin-timepicker /></div>';
                    }
                    $this.append(calendars);
                }
                else {
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;
                    $this.html('<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" isoptional="' + isoptional + '" required class="form-control ' + controlwidth + ' p-tiny" value="' + data[i] + '" data-plugin-timepicker /></div>');
                }
            }
            else if (coltype && coltype.toLowerCase() == "autocomplete") {
                //var subcols = $(this).attr("subcols");
                var controlId = "";
                var CrlId = $(this).attr("controlid");
                if (CrlId != null) {
                    controlId = CrlId;
                }
                var controlwidth = $(this).attr("controlwidth");
                if (controlwidth != null)
                    ;
                else
                    controlwidth = "size50";
                var hfcontrolid = $(this).attr("hfcontrolid");
                var hiddenfieldId = "";
                if (hfcontrolid != null) {
                    hiddenfieldId = hfcontrolid;
                    hiddenfieldId = hiddenfieldId.split(",");
                }

                if (subcols && subcols != "") {
                    $this.attr("subcols", subcols);
                    var Autocompletes = "";
                    for (var k = 1; k <= subcols ; k++) {
                        var attrAutoComplete = $(this).attr("autocompletemethod");
                        var attrSearchMethod = $(this).attr("searchmethod");
                        var attrparentctrl = $(this).attr("parentctrl") != null ? $(this).attr("parentctrl") : "";
                        var attrcontainerctrl = $(this).attr("containerctrl") != null ? $(this).attr("containerctrl") : "";
                        var iscode = $(this).attr("iscode");
                        var CtrlId = controlId + k + "" + RowId;
                        var CtrlName = controlName + k;
                        var CurrentParentCtrl = attrparentctrl + VisitId;
                        if (k != 1) {// to make all controls as required except first control
                            isoptional = "1";
                        }

                        if (attrAutoComplete == "ICD") {
                            var HiddentCtrlId = [];
                            $this.addClass('size-min260');
                        }
                        else {
                            var HiddentCtrlId = "";
                        }
                        var arrICD_ = [];
                        for (var ii_ = 0; ii_ < hiddenfieldId.length; ii_++) {
                            HiddentCtrlId.push(hiddenfieldId[ii_] + k + "" + RowId);
                        }

                        Autocompletes += _self.addAutoCompleteField(CtrlId, data[i], controlwidth, HiddentCtrlId, attrAutoComplete, attrSearchMethod, isoptional, CtrlName, iscode, CurrentParentCtrl, attrcontainerctrl);
                    }
                    $this.append(Autocompletes);
                }
                else {
                    var attrAutoComplete = $(this).attr("autocompletemethod");
                    var attrSearchMethod = $(this).attr("searchmethod");
                    var attrparentctrl = $(this).attr("parentctrl") != null ? $(this).attr("parentctrl") : "";
                    var attrcontainerctrl = $(this).attr("containerctrl") != null ? $(this).attr("containerctrl") : "";
                    var iscode = $(this).attr("iscode");
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;
                    var HiddentCtrlId = hiddenfieldId + RowId;
                    var CurrentParentCtrl = attrparentctrl + VisitId;
                    $this.html(_self.addAutoCompleteField(CtrlId, data[i], controlwidth, HiddentCtrlId, attrAutoComplete, attrSearchMethod, isoptional, CtrlName, iscode, CurrentParentCtrl, attrcontainerctrl));
                    if (controlName == "Vaccine") {
                        Immunization_VaccineCrosswalk.BindVaccineByForGridCategory($this);
                    }
                }
            }
            else if (coltype && coltype.toLowerCase() == "datalist") {
                if (controlName == "Reason") {
                    $this.append('<input placeholder="- Select -" autocomplete="off" type="text" list="browsers' + RowId + '" name="Reason" id="Reason' + RowId + '"><datalist id="browsers' + RowId + '"><option value="coronary artery disease"><option value="hypertension"><option value="joint pain"><option value="LBP"><option value="monitoring medication"><option value="rheumatoid arthritis"><option value="screening"></datalist>');
                }
            }
            else if (coltype && coltype.toLowerCase() == "dropdown") {

                //Start//04//01/2016//Ahmad Raza//ICD DropDown filling issue fixed
                var textToFind = null;
                if (Clinical_ProblemLists.params.ParentCtrl == 'clinicalTabProgressNote') {
                    textToFind = data[3];
                }
                else {
                    if (controlName == "VaccineGroupID") {
                        textToFind = data[1];
                    }
                    else
                        textToFind = data[2];
                }

                //End//04//01/2016//Ahmad Raza//ICD DropDown filling issue fixed

                if (controlName == "Description") {

                    if (subcols && subcols != "") {
                        $this.attr("subcols", subcols);
                        var DropDowns = "";
                        for (var k = 1; k <= subcols ; k++) {
                            var CtrlId = controlId + k + "" + RowId;
                            var CtrlName = controlName + k;
                            if (k != 1) {// to make all controls as required except first control
                                isoptional = "1";
                            }
                            DropDowns += _self.addDropDownFieldIMO(CtrlId, data[i], $(this).attr("ddlist"), isoptional, CtrlName, $row.find('td[icd10]').attr("icd10"));
                        }
                        $this.append(DropDowns);
                    }
                    else {
                        var CtrlId = controlId + RowId;
                        var CtrlName = controlName;
                        $this.html(_self.addDropDownFieldIMO(CtrlId, data[i], $(this).attr("ddlist"), isoptional, CtrlName, $row.find('td[icd10]').attr("icd10")));
                    }
                }
                else if (controlName == "Procedure_Diagnosis") {
                    var diagnosisCopy = $("#pnlOrderSetProcedures #ddlDiagnosis").clone();
                    var DiagnosisValue = $this.html();

                    var SelectedValue = $($row).attr("ProblemListId");
                    if (DiagnosisValue != "") {
                        $(diagnosisCopy).val(SelectedValue);
                    }
                    else {
                        $(diagnosisCopy).val("");
                    }

                    $this.html('').append(diagnosisCopy[0]);
                    //By KR on 26-April-2016, removed spaces and dashes for comparison.
                    $this.find('select').find('option').each(function () {
                        if (typeof $(this).val() != 'undefined' && $(this).val() != null && typeof DiagnosisValue != 'undefined' && DiagnosisValue != null) {
                            if ($(this).val().replace(/\s+/g, '').replace(/-/g, "") == DiagnosisValue.replace(/\s+/g, '').replace(/-/g, "")) {
                                $(this).attr('selected', 'selected');
                            }
                        }
                    });
                }
                    //AST-15 BY:MAhmad
                    //AST - 74 By:MAhmad
                else if (controlName == "Procedure_Diagnosis") {
                    var diagnosisCopy = $('#pnlOrderSetProcedures #DiagnosisDiv #ddlDiagnosis').clone();
                    var DiagnosisValue = $this.html();

                    var SelectedValue = $($row).attr("ProblemListId");
                    if (DiagnosisValue != "") {
                        $(diagnosisCopy).val(SelectedValue);
                    }
                    else {
                        $(diagnosisCopy).val("");
                    }

                    $this.html('').append(diagnosisCopy[0]);
                    $this.find('select').find('option').each(function () {
                        if (typeof $(this).val() != 'undefined' && $(this).val() != null && typeof DiagnosisValue != 'undefined' && DiagnosisValue != null) {
                            if ($(this).val().replace(/\s+/g, '').replace(/-/g, "") == DiagnosisValue.replace(/\s+/g, '').replace(/-/g, "")) {
                                $(this).attr('selected', 'selected');
                            }
                        }
                    });
                }
                    //AST - 74 By:MAhmad
                else if (controlName == "Procedure_DiagnosisOS") {
                    var diagnosisCopy = $('#pnlClinicalOrderSetDetails #OrderSetProcedureDiv #DiagnosisDiv #ddlDiagnosis').clone();
                    var DiagnosisValue = $this.html();

                    var SelectedValue = $($row).attr("ProblemListId");
                    if (DiagnosisValue != "") {
                        $(diagnosisCopy).val(SelectedValue);
                    }
                    else {
                        $(diagnosisCopy).val("");
                    }

                    $this.html('').append(diagnosisCopy[0]);
                    $this.find('select').find('option').each(function () {
                        if (typeof $(this).val() != 'undefined' && $(this).val() != null && typeof DiagnosisValue != 'undefined' && DiagnosisValue != null) {
                            if ($(this).val().replace(/\s+/g, '').replace(/-/g, "") == DiagnosisValue.replace(/\s+/g, '').replace(/-/g, "")) {
                                $(this).attr('selected', 'selected');
                            }
                        }
                    });
                }
                    //AST-15 BY:MAhmad
                else if (controlName == "Treatment_Diagnosis") {
                    var diagnosisCopy = $('#pnlClinicalTreatment #ddlDiagnosis').clone();
                    var DiagnosisValue = $this.html();

                    var SelectedValue = $($row).attr("ProblemListId");
                    if (DiagnosisValue != "") {
                        $(diagnosisCopy).val(SelectedValue);
                    }
                    else {
                        $(diagnosisCopy).val("");
                    }

                    $this.html('').append(diagnosisCopy[0]);
                    $this.find('select').find('option').each(function () {
                        if (typeof $(this).val() != 'undefined' && $(this).val() != null && typeof DiagnosisValue != 'undefined' && DiagnosisValue != null) {
                            if ($(this).val().replace(/\s+/g, '').replace(/-/g, "") == DiagnosisValue.replace(/\s+/g, '').replace(/-/g, "")) {
                                $(this).attr('selected', 'selected');
                            }
                        }
                    });
                }
                else if (controlName == "Surgical") {
                    var surgicalCopy = $('#pnlClinicalProcedures #ddlSurgical').clone();
                    var SurgicalValue = $this.html();

                    var SelectedValue = $($row).attr("SurgicalId");
                    if (SurgicalValue != "") {
                        $(surgicalCopy).val(SelectedValue);
                    }
                    else {
                        $(surgicalCopy).val("0");
                    }

                    $this.html('').append(surgicalCopy[0]);
                    $this.find('select').find('option').each(function () {
                        if (typeof $(this).val() != 'undefined' && $(this).val() != null && typeof SurgicalValue != 'undefined' && SurgicalValue != null) {
                            if ($(this).val().replace(/\s+/g, '').replace(/-/g, "") == SurgicalValue.replace(/\s+/g, '').replace(/-/g, "")) {
                                $(this).attr('selected', 'selected');
                            }
                        }
                    });
                }

                else if (controlName == "VaccineGroupID" || controlName == "CategoryId") {
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;
                    var onChangeEvent = $(this).attr("onchange");
                    $this.html(_self.addDropDownFieldCategory(CtrlId, data[i], "GetAdministerVaccine_Category", isoptional, CtrlName, onChangeEvent));


                }
                else if (controlName == "ScheduleId") {
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;
                    var scheduleTypeId = $(this).attr("schtypeid");
                    $this.html(_self.addDropDownFieldSchedule(CtrlId, data[i], "GetImmunizationSchedule", isoptional, CtrlName, scheduleTypeId));
                }
                else if (controlName == "Caller") {
                    var CtrlId_local = controlId + RowId;
                    var CtrlName_local = controlName;
                    var enrollmentinfoid = $row.attr("enrollmentinfoid");
                    var method_local = $(this).attr("ddlist");
                    var ddl_local = document.createElement("select");
                    ddl_local.setAttribute("class", "form-control");
                    ddl_local.setAttribute("id", CtrlId_local);
                    ddl_local.setAttribute("name", CtrlName_local);
                    ddl_local.setAttribute("isoptional", isoptional);
                    $(ddl_local).empty();

                    var dataValue = "IsActive=&ID=" + enrollmentinfoid;
                    MDVisionService.lookups(method_local, true, dataValue).done(function (results) {
                        results = JSON.parse(results[method_local]);
                        if (results) {
                            $.each(results, function (j, result) {
                                $(ddl_local).append($("<option />").val(result.Value).text(result.Name.split("~_~")[0]).attr("RefValue", result.Name.split("~_~")[1]).attr("RefName", result.Name.split("~_~")[1]));
                            });
                            var selected_value = data[7];
                            $(ddl_local).find('option').each(function () {
                                if ($.trim($(this).text().toLowerCase()) == selected_value.toLowerCase())
                                { $(ddl_local).val(this.value); }
                            });
                        }
                    });

                    $this.html(ddl_local);

                }
                    //---------------
                else if (controlName == "TaskReason") {
                    var CtrlId_local = controlId + RowId;
                    var CtrlName_local = controlName;
                    var method_local = $(this).attr("ddlist");
                    var ddl_local = document.createElement("select");
                    ddl_local.setAttribute("class", "form-control");
                    ddl_local.setAttribute("id", CtrlId_local);
                    ddl_local.setAttribute("name", CtrlName_local);
                    ddl_local.setAttribute("isoptional", isoptional);
                    $(ddl_local).empty();

                    MDVisionService.lookups(method_local, true).done(function (results) {
                        results = JSON.parse(results[method_local]);
                        if (results) {
                            $.each(results, function (j, result) {
                                $(ddl_local).append($("<option />").val(result.Value).text(result.Name.split("~_~")[0]).attr("RefValue", result.Name.split("~_~")[1]).attr("RefName", result.Name.split("~_~")[1]));
                            });
                            var selected_value = data[3];
                            $(ddl_local).find('option').each(function () {
                                if ($.trim($(this).text().toLowerCase()) == selected_value.toLowerCase())
                                { $(ddl_local).val(this.value); }
                            });
                        }
                    });

                    $this.html(ddl_local);

                }
                else if (controlName == "CallReason") {
                    var CtrlId_local = controlId + RowId;
                    var CtrlName_local = controlName;
                    var method_local = $(this).attr("ddlist");
                    var ddl_local = document.createElement("select");
                    ddl_local.setAttribute("class", "form-control");
                    ddl_local.setAttribute("id", CtrlId_local);
                    ddl_local.setAttribute("name", CtrlName_local);
                    ddl_local.setAttribute("isoptional", isoptional);
                    $(ddl_local).empty();
                    MDVisionService.lookups(method_local, true).done(function (results) {
                        results = JSON.parse(results[method_local]);
                        if (results) {
                            $.each(results, function (j, result) {
                                $(ddl_local).append($("<option />").val(result.Value).text(result.Name.split("~_~")[0]).attr("RefValue", result.Name.split("~_~")[1]).attr("RefName", result.Name.split("~_~")[1]));
                            });
                            var selected_value = data[4];
                            $(ddl_local).find('option').each(function () {
                                if ($.trim($(this).text().toLowerCase()) == selected_value.toLowerCase())
                                { $(ddl_local).val(this.value); }
                            });
                        }
                    });

                    $this.html(ddl_local);

                }
                else if (controlName == "ReasonId") {
                    var Value = $this.html().trim();
                    var CtrlId_local = controlId + RowId;
                    var CtrlName_local = controlName;
                    var method_local = $(this).attr("ddlist");
                    var ddl_local = document.createElement("select");
                    ddl_local.setAttribute("class", "form-control");
                    ddl_local.setAttribute("id", CtrlId_local);
                    ddl_local.setAttribute("name", CtrlName_local);
                    ddl_local.setAttribute("isoptional", isoptional);
                    $(ddl_local).empty();
                    MDVisionService.lookups(method_local, true).done(function (results) {
                        results = JSON.parse(results[method_local]);
                        if (results) {
                            $.each(results, function (j, result) {
                                $(ddl_local).append($("<option />").val(result.Value).text(result.Name.split("~_~")[0]).attr("RefValue", result.Name.split("~_~")[1]).attr("RefName", result.Name.split("~_~")[1]));
                            });
                            var selected_value = Value;
                            $(ddl_local).find('option').each(function () {
                                if ($.trim($(this).text().toLowerCase()) == selected_value.toLowerCase())
                                { $(ddl_local).val(this.value); }
                            });
                        }
                    });

                    $this.html(ddl_local);

                }
                else if (controlName == "CQMEncounterTypeId") {
                    var Value = $this.html().trim();
                    var CtrlId_local = controlId + RowId;
                    var CtrlName_local = controlName;
                    var method_local = $(this).attr("ddlist");
                    var ddl_local = document.createElement("select");
                    ddl_local.setAttribute("class", "form-control");
                    ddl_local.setAttribute("id", CtrlId_local);
                    ddl_local.setAttribute("name", CtrlName_local);
                    ddl_local.setAttribute("isoptional", isoptional);
                    $(ddl_local).empty();
                    MDVisionService.lookups(method_local, true).done(function (results) {
                        results = JSON.parse(results[method_local]);
                        if (results) {
                            $.each(results, function (j, result) {
                                $(ddl_local).append($("<option />").val(result.Value).text(result.Name.split("~_~")[0]).attr("RefValue", result.Name.split("~_~")[1]).attr("RefName", result.Name.split("~_~")[1]));
                            });
                            var selected_value = Value;
                            $(ddl_local).find('option').each(function () {
                                if ($.trim($(this).text().toLowerCase()) == selected_value.toLowerCase())
                                { $(ddl_local).val(this.value); }
                            });
                        }
                    });

                    $this.html(ddl_local);

                }
                else if (controlName == "BPNegationReason" || controlName == "NegationReason") {

                    var Value = "";
                    if (controlName == "NegationReason") {
                        Value = $($row).attr("NegationReasonId");
                    }
                    else if (controlName == "BPNegationReason") {
                        Value = $($row).attr("BPNegationReasonId");
                    }

                    var CtrlId_local = controlId + RowId;
                    var CtrlName_local = controlName;
                    var method_local = $(this).attr("ddlist");
                    var ddl_local = document.createElement("select");
                    ddl_local.setAttribute("class", "form-control");
                    ddl_local.setAttribute("id", CtrlId_local);
                    ddl_local.setAttribute("name", CtrlName_local);
                    ddl_local.setAttribute("isoptional", isoptional);
                    $(ddl_local).empty();
                    MDVisionService.lookups(method_local, true).done(function (results) {
                        results = JSON.parse(results[method_local]);
                        if (results) {
                            $.each(results, function (j, result) {
                                $(ddl_local).append($("<option />").val(result.Value).text(result.Name.split("~_~")[0]).attr("RefValue", result.Name.split("~_~")[1]).attr("RefName", result.Name.split("~_~")[1]));
                            });
                            var selected_value = Value;
                            $(ddl_local).find('option').each(function () {
                                if ($(this).val() == selected_value)
                                { $(ddl_local).val(this.value); }
                            });
                        }
                    });

                    $this.html(ddl_local);

                }
                else if (controlName == "Urgency") {
                    var Value = "";
                    var CtrlId_local = controlId + RowId;
                    var CtrlName_local = controlName;
                    if ($(this).attr('RowNumberRequired')) {
                        CtrlName_local = controlName + "_" + RowId;
                    }
                    var method_local = $(this).attr("ddlist");
                    var ddl_local = document.createElement("select");
                    ddl_local.setAttribute("class", "form-control");
                    ddl_local.setAttribute("id", CtrlId_local);
                    ddl_local.setAttribute("name", CtrlName_local);
                    ddl_local.setAttribute("isoptional", isoptional);
                    $(ddl_local).empty();
                    MDVisionService.lookups(method_local, true).done(function (results) {
                        results = JSON.parse(results[method_local]);
                        if (results) {
                            $.each(results, function (j, result) {
                                $(ddl_local).append($("<option />").val(result.Value).text(result.Name.split("~_~")[0]).attr("RefValue", result.Name.split("~_~")[1]).attr("RefName", result.Name.split("~_~")[1]));
                            });
                            if (controlName == "Urgency")
                                Value = $($row).attr("UrgencyId");
                            var selected_value = Value;
                            $(ddl_local).find('option').each(function () {
                                if ($(this).val() == selected_value)
                                { $(ddl_local).val(this.value); }
                            });
                        }
                    });
                    $this.html(ddl_local);
                }


                    //------------------
                else {
                    if (subcols && subcols != "") {
                        $this.attr("subcols", subcols);
                        var DropDowns = "";
                        for (var k = 1; k <= subcols ; k++) {
                            var CtrlId = controlId + k + "" + RowId;
                            var CtrlName = controlName + k;
                            if (k != 1) {// to make all controls as required except first control
                                isoptional = "1";
                            }
                            DropDowns += _self.addDropDownField(CtrlId, data[i], $(this).attr("ddlist"), isoptional, CtrlName);
                        }
                        $this.append(DropDowns);
                    }
                    else {
                        var CtrlId = controlId + RowId;
                        var CtrlName = controlName;

                        if ($(this).attr('RowNumberRequired')) {
                            CtrlName = controlName + "_" + RowId;
                        }
                        $this.html(_self.addDropDownField(CtrlId, data[i], $(this).attr("ddlist"), isoptional, CtrlName));
                    }
                }
            }
            else if (coltype && coltype.toLowerCase() == "checkbox") {

                var cntrols = [];
                if (subcols && subcols != "") {
                    $this.attr("subcols", subcols);
                    var CheckBoxes = "";
                    for (var k = 1; k <= subcols ; k++) {
                        var CtrlId = controlId + k + "" + RowId;
                        var CtrlName = controlName + k;
                        if (k != 1) {// to make all controls as required except first control
                            isoptional = "1";
                        }
                        if (data[i] && data[i].toLowerCase() == "true" || data[i] && data[i].toLowerCase() == "yes")
                            cntrols.push({ Id: CtrlId, Value: true });
                        else
                            cntrols.push({ Id: CtrlId, Value: false });

                        CheckBoxes += '<input type="checkbox" id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' isoptional="' + isoptional + '" class="input-block"/>';
                    }
                    $this.append(CheckBoxes);
                }
                else {
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;

                    //Start//06/01/2016//Ahmad Raza//While editing the record associated with Note, Checkbox unchecked issue fixed
                    if ($(data[i]).is(':checked') == true) {
                        $this.html('<input type="checkbox" id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' isoptional="' + isoptional + '" checked="checked" class="input-block"/>');
                    }
                    else {
                        $this.html('<input type="checkbox" id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' isoptional="' + isoptional + '" class="input-block"/>');
                    }
                    //End//06/01/2016//Ahmad Raza//While editing the record associated with Note, Checkbox unchecked issue fixed

                    if (data[i] && data[i].toLowerCase() == "true" || data[i] && data[i].toLowerCase() == "yes")
                        cntrols.push({ Id: CtrlId, Value: true });
                    else
                        cntrols.push({ Id: CtrlId, Value: false });
                }

                //set all checkboxs prop checked or not
                for (var i in cntrols) {
                    $("#" + cntrols[i].Id).prop("checked", cntrols[i].Value);
                }
            }

                // Start 27/11/2015 Muhammad Irfan

            else if (coltype && coltype.toLowerCase() == "button") {
                var CtrlId = controlId + RowId;
                var CtrlName = controlName;
                var commentsMethod = "Clinical_ProblemLists.AddComments('" + RowId + "');";
                $this.html('<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title=""><i class="fa fa-commenting"></i></a>');
            }


                // End 27/11/2015 Muhammad Irfan
            else if (coltype && coltype.toLowerCase() == "btnccmproblemcomments") {
                var CtrlId = controlId + RowId;
                var CtrlName = controlName;
                var commentsMethod = "CCM_Patient_Hub.AddComments('" + RowId + "');";
                $this.html('<a href="javascript:void(0);" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title=""><i class="fa fa-commenting"></i></a>');
            }
            else if (coltype && coltype.toLowerCase() == "btnosproblemcomments") {
                var CtrlId = controlId + RowId;
                var CtrlName = controlName;
                var commentsMethod = "OrderSet_Problems.AddComments('" + RowId + "');";
                $this.html('<a href="javascript:void(0);" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title=""><i class="fa fa-commenting"></i></a>');
            } else if (coltype && coltype.toLowerCase() == "btnproblemcommentsos") {
                var CtrlId = controlId + RowId;
                var CtrlName = controlName;
                var commentsMethod = "OrderSet_ProblemListGrid.AddComments('" + RowId + "');";
                $this.html('<a href="javascript:void(0);" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title=""><i class="fa fa-commenting"></i></a>');
            }

            else if (coltype && coltype.toLowerCase() == "btnproblemcommentst") {
                var CtrlId = controlId + RowId;
                var CtrlName = controlName;
                var commentsMethod = "Treatment_ProblemListGrid.AddComments('" + RowId + "');";
                $this.html('<a href="javascript:void(0);" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title=""><i class="fa fa-commenting"></i></a>');
            }
                //Start //30/11/2015 Ahmad Raza
            else if (coltype && coltype.toLowerCase() == "btncomments") {
                var CtrlId = controlId + RowId;
                var CtrlName = controlName;
                var commentsMethod = "Clinical_Allergies.AddComments('" + RowId + "');";
                $this.html('<a href="javascript:void(0)" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title=""><i class="fa fa-commenting"></i></a>');
            }
            else if (coltype && coltype.toLowerCase() == "btncommentsprocedures") {
                var CtrlId = controlId + RowId;
                var CtrlName = controlName;
                var commentsMethod = "Clinical_Procedures.AddComments('" + RowId + "');";
                $this.html('<a href="javascript:void(0)" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title=""><i class="fa fa-commenting"></i></a>');
                //Start 26-09-2016 Humaira Yousaf for comments
                Clinical_Procedures.PreviousComments = $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures tbody tr[id='" + RowId + "']").find("#hfComments").val();
                //End 26-09-2016 Humaira Yousaf for comments
            }
            else if (coltype && coltype.toLowerCase() == "btncommentsproceduresos") {
                var CtrlId = controlId + RowId;
                var CtrlName = controlName;
                var commentsMethod = "OrderSet_Procedures.AddComments('" + RowId + "');";
                $this.html('<a href="javascript:void(0)" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title=""><i class="fa fa-commenting"></i></a>');
                Clinical_Procedures.PreviousComments = $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures tbody tr[id='" + RowId + "']").find("#hfComments").val();
            }
                //AST-15 BY:MAhmad
            else if (coltype && coltype.toLowerCase() == "btncommentsproceduresgridos") {
                var CtrlId = controlId + RowId;
                var CtrlName = controlName;
                var commentsMethod = "Clinical_OrderSetDetails.AddComments('" + RowId + "');";
                $this.html('<a href="javascript:void(0)" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title=""><i class="fa fa-commenting"></i></a>');
                Clinical_OrderSetDetails.PreviousComments = $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS tbody tr[id='" + RowId + "']").find("#hfComments").val();
            }
                //AST-15 BY:MAhmad
            else if (coltype && coltype.toLowerCase() == "btncommentsproceduresgrid") {
                var CtrlId = controlId + RowId;
                var CtrlName = controlName;
                var commentsMethod = "Treatment_ProcedureListGrid.AddComments('" + RowId + "');";
                $this.html('<a href="javascript:void(0)" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" data-original-title=""><i class="fa fa-commenting"></i></a>');
                Treatment_ProcedureListGrid.PreviousComments = $("#" + Clinical_Treatment.params.PanelID + " #dgvProceduresT tbody tr[id='" + RowId + "']").find("#hfComments").val();
            }
            else if (coltype && coltype.toLowerCase() == "lastmodifydate") {
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
                var CurrentDate = (fullDate.getMonth() + 1) + "/" + fullDate.getDate() + "/" + fullDate.getFullYear() + " " + (fullDate.getHours() % 12) + ":" + fullDate.getMinutes() + ":" + fullDate.getSeconds() + " " + mid;
                var CurrentModifyDate = CurrentDate + " " + globalAppdata['AppUserName'];

                $this.html(CurrentModifyDate);
            }

            //End //30/11/2015 Ahmad Raza


            // my new code goes here IrFan diagnosis

            //else if (coltype && coltype.toLowerCase() == "diagnosis") {

            //    var textToFind = data[2];

            //    if (subcols && subcols != "") {
            //        $this.attr("subcols", subcols);
            //        var DropDowns = "";
            //        for (var k = 1; k <= subcols ; k++) {
            //            var CtrlId = controlId + k + "" + RowId;
            //            var CtrlName = controlName + k;
            //            if (k != 1) {// to make all controls as required except first control
            //                isoptional = "1";
            //            }
            //            DropDowns += _self.addDropDownFieldIMO(CtrlId, data[i], $(this).attr("ddlist"), isoptional, CtrlName, textToFind);
            //        }
            //        $this.append(DropDowns);
            //    }
            //    else {
            //        var CtrlId = controlId + RowId;
            //        var CtrlName = controlName;
            //        $this.html(_self.addDropDownFieldIMO(CtrlId, data[i], $(this).attr("ddlist"), isoptional, CtrlName, textToFind));
            //    }

            //}

            // end my code
        });

        $row.find("input[id*=dtp]").each(function () {

            var date_format = 'dd/mm/yyyy';
            //set default Date Formate
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];

            if ($(this).attr('id').indexOf('dtpFrom') > -1) {
                var dtDOSFromId = $row.find("input[id*=dtpFrom]").attr('id');
                var dtDOSToId = $row.find("input[id*=dtpTo]").attr("id");
                utility.ValidateFromToDate($($row).attr('id'), dtDOSFromId, dtDOSToId, true, null, null, true);
            } else
                if (($row.find("input[id*=dtpDOSFrom]").length > 0 && $row.find("input[id*=dtpDOSFrom]").attr('id').indexOf('dtpDOSFrom') > -1) || ($row.find("input[id*=dtpDOSTo]").length > 0 && $row.find("input[id*=dtpDOSTo]").attr("id").indexOf('dtpDOSTo') > -1)) {
                    //var dtDOSFromId = $row.find("input[id*=dtpDOSFrom]").attr('id');
                    //var dtDOSToId = $row.find("input[id*=dtpDOSTo]").attr("id");
                    //utility.ValidateFromToDate('frmEncounterChargeCapture', dtDOSFromId, dtDOSToId, false);


                    //if ($(this).attr('id').indexOf('dtpDOSFrom') != -1)
                    //    $row.find("input[id*=dtpDOSTo]").val($(this).val());

                }
                else {


                    $(this).datepicker({
                        todayHighlight: true,
                        format: date_format,
                    }).on('changeDate', function (e) {

                        $(this).datepicker('hide');

                    });
                    //Begin 4/27/16  Edit By M Ahmad Imran
                    if ($(this).attr('id').indexOf('dtpToProblemDate') < 0) {


                        if ($(this).val() == "" || $(this).val() == null) {
                            $(this).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
                        }
                    }
                    //End 4/27/16  Edit By M Ahmad Imran
                }
        });

        $row.find("input[id^=tp]").each(function () {
            $(this).timepicker().on('changeTime.timepicker', function (e) {
                disableFocus: false
            });
            $(this).timepicker('setTime', new Date());
        });

        if (EMREditableGrid.options.table.indexOf("dgvProcedureReferral") != -1) {
            $row.loadDropDowns(true).done(function () {
                //var selector = EMREditableGrid.options.table;

                //if ($(selector).parents("#pnlClinicalFaceSheet").length) {
                //    selector = "#pnlClinicalFaceSheet " + selector;
                //} else if ($(selector).parents("#pnlClinicalProgressNote").length) {
                //    selector = "#pnlClinicalProgressNote " + selector;
                //}
                //$(selector + ' tr:last td:last > select > option:contains("Normal")').prop('selected', 'selected');
            });
        } else {
            $row.loadDropDowns(true);
        }

        if ($row.attr("problemListnotesid") == null) {

        }
        else {
            $row.find("td a.expand-row i").attr("class", "hideExpand");
        }
        //Start 27-10-2016 Humaira Yousaf to log view action for problem lists and procedures on edit
        var name = $($row).attr("name");
        if (name == "Problems") {
            Clinical_ProblemLists.logViewProblem(RowId);
        }
        if (name == "Procedures") {
            var id = $($row).attr('id').replace('txtpUnits-', '').trim();
            Clinical_Procedures.logViewProcedures(id);
        }
        //End 27-10-2016 Humaira Yousaf to log view action for problem lists and procedures on edit
    },
    rowPayment:function($row,ClassName){
        ClassName.rowPayment($row, EMREditableGrid);
    },
    rowPaymentDetail:function($row,ClassName){
        ClassName.rowPaymentDetail($row, EMREditableGrid);
    },
    addDropDownField: function (id, selected_value, method, isoptional, CtrlName) {

        if (CtrlName == 'ChronicityLevel') {
            var ddl = document.createElement("select");
            ddl.setAttribute("class", "form-control");
            ddl.setAttribute("id", id);
            ddl.setAttribute("name", CtrlName);
            ddl.setAttribute("ddlist", method);
            ddl.setAttribute("isoptional", isoptional);

            if (selected_value == "") {

                $(ddl).append('<option value="">- Select -</option>');//kr
                $(ddl).append('<option value="Chronic">Chronic</option>');
                $(ddl).append('<option value="Acute">Acute</option>');
                $(ddl).append('<option value="Self-Limiting">Self-Limiting</option>');
                $(ddl).append('<option value="Intermittent">Intermittent</option>');
                $(ddl).append('<option value="Recurrent">Recurrent</option>');
                $(ddl).append('<option value="Distressful">Distressful</option>');
            }
            else {
                if (selected_value == "Chronic")
                    $(ddl).append('<option selected="selected" value="Chronic">Chronic</option>');
                else
                    $(ddl).append('<option value="Chronic">Chronic</option>');
                //---
                if (selected_value == "Acute")
                    $(ddl).append('<option selected="selected" value="Acute">Acute</option>');
                else
                    $(ddl).append('<option value="Acute">Acute</option>');
                //--
                if (selected_value == "Self-Limiting")
                    $(ddl).append('<option selected="selected" value="Self-Limiting">Self-Limiting</option>');
                else
                    $(ddl).append('<option value="Self-Limiting">Self-Limiting</option>');
                //--
                if (selected_value == "Intermittent")
                    $(ddl).append('<option selected="selected" value="Intermittent">Intermittent</option>');
                else
                    $(ddl).append('<option value="Intermittent">Intermittent</option>');
                //--
                if (selected_value == "Recurrent")
                    $(ddl).append('<option selected="selected" value="Recurrent">Recurrent</option>');
                else
                    $(ddl).append('<option value="Recurrent">Recurrent</option>');
                //--
                if (selected_value == "Distressful")
                    $(ddl).append('<option selected="selected" value="Distressful">Distressful</option>');
                else
                    $(ddl).append('<option value="Distressful">Distressful</option>');
                //--
                $(ddl).append('<option value="">- Select -</option>');//kr
            }


            return ddl
        } else if (CtrlName == 'Severity') {

            var ddl = document.createElement("select");
            ddl.setAttribute("class", "form-control");
            ddl.setAttribute("id", id);
            ddl.setAttribute("name", CtrlName);
            ddl.setAttribute("ddlist", method);
            ddl.setAttribute("isoptional", isoptional);

            if (selected_value == "") {

                $(ddl).append('<option value="">- Select -</option>');//kr
                $(ddl).append('<option value="Unspecified Severity">Unspecified Severity</option>');
                $(ddl).append('<option value="Severe Persistent">Severe Persistent</option>');
                $(ddl).append('<option value="Moderate Persistent">Moderate Persistent</option>');
                $(ddl).append('<option value="Mild Persistent">Mild Persistent</option>');
                $(ddl).append('<option value="Mild Intermittent">Mild Intermittent</option>');
                $(ddl).append('<option value="Moderate">Moderate</option>');

            } else {
                if (selected_value == "Unspecified Severity")
                    $(ddl).append('<option selected="selected" value="Unspecified Severity">Unspecified Severity</option>');
                else
                    $(ddl).append('<option value="Unspecified Severity">Unspecified Severity</option>');
                //--
                if (selected_value == "Severe Persistent")
                    $(ddl).append('<option selected="selected" value="Severe Persistent">Severe Persistent</option>');
                else
                    $(ddl).append('<option value="Severe Persistent">Severe Persistent</option>');
                //--
                if (selected_value == "Moderate Persistent")
                    $(ddl).append('<option selected="selected" value="Moderate Persistent">Moderate Persistent</option>');
                else
                    $(ddl).append('<option value="Moderate Persistent">Moderate Persistent</option>');
                //--
                if (selected_value == "Mild Persistent")
                    $(ddl).append('<option selected="selected" value="Mild Persistent">Mild Persistent</option>');
                else
                    $(ddl).append('<option value="Mild Persistent">Mild Persistent</option>');
                //--
                if (selected_value == "Mild Intermittent")
                    $(ddl).append('<option selected="selected" value="Mild Intermittent">Mild Intermittent</option>');
                else
                    $(ddl).append('<option value="Mild Intermittent">Mild Intermittent</option>');
                if (selected_value == "Moderate")
                    $(ddl).append('<option selected="selected" value="Moderate">Moderate</option>');
                else
                    $(ddl).append('<option value="Moderate">Moderate</option>');
                //--
                $(ddl).append('<option value="">- Select -</option>');//kr
            }

            return ddl

        }
            //Start //30/11/2015 Ahmad Raza //Logic for dropdowns in Grid

        else if (CtrlName == 'AllergySeverityType') {

            var ddl = document.createElement("select");
            ddl.setAttribute("class", "form-control");
            ddl.setAttribute("id", id);
            ddl.setAttribute("name", CtrlName);
            ddl.setAttribute("ddlist", method);
            ddl.setAttribute("isoptional", isoptional);

            if (selected_value == "") {

                //Start//03/12/2015//Ahmad Raza//Severity dropdown filling logic improved
                var ddlSeverityHTML = $("#pnlClinicalAllergies #ddlSeverity").html();
                $(ddl).append(ddlSeverityHTML);
                //End//03/12/2015//Ahmad Raza//Severity dropdown filling logic improved

            } else {
                if (selected_value == "Mild")
                    $(ddl).append('<option selected="selected" value="Mild">Mild</option>');
                else
                    $(ddl).append('<option value="Mild">Mild</option>');
                //--
                if (selected_value == "Moderate")
                    $(ddl).append('<option selected="selected" value="Moderate">Moderate</option>');
                else
                    $(ddl).append('<option value="Moderate">Moderate</option>');
                //--
                if (selected_value == "Sever")
                    $(ddl).append('<option selected="selected" value="Sever">Sever</option>');
                else
                    $(ddl).append('<option value="Sever">Sever</option>');
                //--
                if (selected_value == "UnKnown")
                    $(ddl).append('<option selected="selected" value="UnKnown">UnKnown</option>');
                else
                    $(ddl).append('<option value="UnKnown">UnKnown</option>');
                //--

            }

            return ddl

        }
        else if (CtrlName == 'AllergyType') {

            var ddl = document.createElement("select");
            ddl.setAttribute("class", "form-control");
            ddl.setAttribute("id", id);
            ddl.setAttribute("name", CtrlName);
            ddl.setAttribute("ddlist", method);
            ddl.setAttribute("isoptional", isoptional);

            if (selected_value == "") {

                //Start//03/12/2015//Ahmad Raza//AllergyType dropdown filling logic improved

                var ddlAllergyTypeHTML = $("#pnlClinicalAllergies #ddlAllergenType").html();
                $(ddl).append(ddlAllergyTypeHTML);

                //End//03/12/2015//Ahmad Raza//AllergyType dropdown filling logic improved

            } else {
                if (selected_value == "Drug Allergy")
                    $(ddl).append('<option selected="selected" value="Drug Allergy">Drug Allergy</option>');
                else
                    $(ddl).append('<option value="Drug Allergy">Drug Allergy</option>');
                //--
                if (selected_value == "Food Allergy")
                    $(ddl).append('<option selected="selected" value="Food Allergy">Food Allergy</option>');
                else
                    $(ddl).append('<option value="Food Allergy">Food Allergy</option>');
                //--
                if (selected_value == "Environment Allergy")
                    $(ddl).append('<option selected="selected" value="Environment Allergy">Environment Allergy</option>');
                else
                    $(ddl).append('<option value="Environment Allergy">Environment Allergy</option>');
                //--
                if (selected_value == "Others")
                    $(ddl).append('<option selected="selected" value="Others">Others</option>');
                else
                    $(ddl).append('<option value="Others">Others</option>');
                //--

            }
            return ddl
            //End //30/11/2015 Ahmad Raza//Logic for dropdowns in Grid
        } else {
            var ddl = document.createElement("select");

            ddl.setAttribute("class", "form-control");
            ddl.setAttribute("id", id);
            ddl.setAttribute("name", CtrlName);
            ddl.setAttribute("ddlist", method);
            ddl.setAttribute("isoptional", isoptional);
            return ddl
        }
    },

    addAutoCompleteField: function (id, value, width, hfcontrolid, autoCompleteMethod, searchMethod, isoptional, CtrlName, iscode, ParentCtrl, ContainerCtrl) {

        var hfControl = "";

        if (Object.prototype.toString.call(hfcontrolid) === '[object Array]') {
            if (hfcontrolid != "") {
                for (var i = 0; i < hfcontrolid.length; i++) {
                    hfControl += '<input type="hidden" id="' + hfcontrolid[i] + '" />';
                }
            }
        }
        else {
            if (hfcontrolid != "") {
                hfControl = '<input type="hidden" id="' + hfcontrolid + '" />';
            }
        }

        var AutoCompleteSearch = "";
        var ClickToSearch = "";
        if (searchMethod && searchMethod != "") {
            ClickToSearch = searchMethod.substring(0, searchMethod.lastIndexOf(")")) + ",'" + id + "','" + hfcontrolid + "');";
            //ClickToSearch = searchMethod;

        }
        var onFocusOutMethod = "utility.ValidateAutoComplete(this, '" + hfcontrolid + "', true);";

        var classForICD = "";
        if (autoCompleteMethod && autoCompleteMethod == "ICD") {

            classForICD = "pull-left size70 mr-tiny"

            // MK
            //if (globalAppdata['IMO_ID'] == "") {
            //    AutoCompleteSearch = " CacheManager.BindAutoCompleteText(this, 'GetICDCode', true, '#" + hfcontrolid + "', '');";
            //}
            //else {
            AutoCompleteSearch = "utility.BindIMOAutoCompleteText(this, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', '#" + hfcontrolid + "', '',true,'','ICD',true,'" + ParentCtrl + "','" + ContainerCtrl + "', true);";
            //}
            // -MK

            // AutoCompleteSearch = " CacheManager.BindAutoCompleteText(this, 'GetICDCode', true, '#" + hfcontrolid + "', '');";
            //ClickToSearch = "EMREditableGrid.OpenSearchPopup('ICD');";
            //            onFocusOutMethod = "utility.ValidateAutoComplete(this, '" + hfcontrolid + "', true,1);";

            //MK
            //onFocusOutMethod = "";//"utility.ValidateCode(this, 'ICD','" + hfcontrolid + "');";
            onFocusOutMethod = "utility.ValidateIsEmpty(this, 'ICD','" + hfcontrolid + "', '" + ParentCtrl + "' );EncounterChargeCapture.ValidateICDCode(this);";
        }
        else if (autoCompleteMethod && autoCompleteMethod == "CPT") {
            //MK//if (globalAppdata['IMO_ID'] == "") {
            //    AutoCompleteSearch = " CacheManager.BindAutoCompleteText(this, 'GetCPTCode', true, '#" + hfcontrolid + "', '');";
            //}
            //else {
            //    AutoCompleteSearch = "utility.BindAutoCompleteText(this, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#" + hfcontrolid + "', '');";
            //MK//}
            //AutoCompleteSearch = " CacheManager.BindAutoCompleteText(this, 'GetCPTCode', true, '#" + hfcontrolid + "', '');";
            //onFocusOutMethod = "utility.ValidateAutoComplete(this, '" + hfcontrolid + "', true,1);";
            AutoCompleteSearch = "utility.BindIMOAutoCompleteText(this, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#" + hfcontrolid + "', '',true,'','CPT',true,'" + ParentCtrl + "','" + ContainerCtrl + "', true);";
            onFocusOutMethod = "";//"utility.ValidateCode(this, 'CPT','" + hfcontrolid + "');";

        }
        else if (autoCompleteMethod && autoCompleteMethod == "Modifier") {
            //if (globalAppdata['IMO_ID'] == "") {
            //    AutoCompleteMethod = " CacheManager.BindAutoCompleteText(this, 'GetCPTCode', true, '#" + hfcontrolid + "', '');";
            //}
            //else {
            AutoCompleteSearch = "utility.BindAutoCompleteText(this, 'COMMON_CODE', 'GET_MODIFIER_CODE', '#" + hfcontrolid + "', '',false,0," + iscode + ");";
            onFocusOutMethod = "utility.ValidateCode(this, 'MODIFIER','" + hfcontrolid + "');";

            //}
            //ClickToSearch = "EMREditableGrid.OpenSearchPopup('Modifier');";

        }
        else if (autoCompleteMethod && autoCompleteMethod == "InsurancePlan") {
            AutoCompleteSearch = "utility.BindInsurancePlan(this, '" + hfcontrolid + "','" + ContainerCtrl + "');";
        }
        return autoField = '<div class="' + classForICD + '"><div class="input-group">' +
            '<input id="' + id + '" class="form-control ' + width + '" isoptional="' + isoptional + '" name = "' + CtrlName + '"  placeholder="Start typing to search" value="' + value + '" type="text" oninput="' + AutoCompleteSearch + '" onblur="' + onFocusOutMethod + '">' +
                           '<div class="input-group-btn">' +
                              '<button class="btn btn-primary btn-xs pl-xxs pr-xxs" type="button"  onclick="' + ClickToSearch + '"><i class="glyphicon glyphicon-search tiny-font"></i></button>' +
                           '</div></div><div>' + hfControl + '</div></div>';
    },



    getAutoCompleteSearch: function (autoCompleteMethod, hfcontrolid) {
        var AutoCompleteSearch = "";
        if (autoCompleteMethod && autoCompleteMethod == "Modifier") {
            //if (globalAppdata['IMO_ID'] == "") {
            //    AutoCompleteMethod = " CacheManager.BindAutoCompleteText(this, 'GetCPTCode', true, '#" + hfcontrolid + "', '');";
            //}
            //else {
            AutoCompleteSearch = "utility.BindAutoCompleteText(this, 'COMMON_CODE', 'GET_MODIFIER_CODE', '#" + hfcontrolid + "', '',false,0);";
            //}
            //ClickToSearch = "EMREditableGrid.OpenSearchPopup('Modifier');";

        } else
            if (autoCompleteMethod && autoCompleteMethod == "POS") {
                AutoCompleteSearch = "utility.POSAutoComplete(this,  '#" + hfcontrolid + "');";
            }

        return AutoCompleteSearch;
    },

    rowExpand: function ($row, ClassName) {
        ClassName.rowExpand($row, EMREditableGrid);
    },

    rowSave: function ($row, ClassName) {
        ClassName.rowSave($row, EMREditableGrid);
    },

    rowRemove: function ($row, ClassName) {
        ClassName.rowRemove($row, EMREditableGrid);
        //this.datatable.row($row.get(0)).remove().draw();
    },
    rowInactive: function ($row, ClassName) {
        ClassName.rowInactive($row, EMREditableGrid);
        //this.datatable.row($row.get(0)).remove().draw();
    },

    rowSetActionsEditing: function ($row) {
        $row.find('.on-editing').removeClass('hidden');
        $row.find('.on-default').addClass('hidden');
    },

    rowSetActionsDefault: function ($row) {
        $row.find('td.actions a.on-editing').addClass('hidden');
        $row.find('.on-default').removeClass('hidden');
    },

    rowValidate: function ($row) {

        var firstInvalid = false;

        var isValidRow = true;
        var isValidRowChild = true;
        $row.find('td').map(function () {
            var $this = $(this);

            var Tags = $this.find('[type=hidden],[type=text],[type=password], textarea, select');//'[type=hidden],[type=text], textarea, ul'
            Tags.each(function () {
                if ($(this).attr("isoptional") && $(this).attr("isoptional") == "0" && $(this).val() == "") {
                    $(this).css("border", "1px solid red");
                    if (firstInvalid == false) {
                        firstInvalid = true;
                        $(this).focus();
                    }
                    if (isValidRow != false) {
                        isValidRow = false;
                    }
                }
                else
                    $(this).css("border", "1px solid #ccc");
            });
        });

        var childRow = EMREditableGrid.datatable.row($row).child();
        if (childRow) {
            childRow.find('td').map(function () {
                var $this = $(this);

                var Tags = $this.find('[type=hidden],[type=text],[type=password], textarea, select');//'[type=hidden],[type=text], textarea, ul'
                Tags.each(function () {
                    if ($(this).attr("isoptional") && $(this).attr("isoptional") == "0" && $(this).val() == "") {
                        $(this).css("border", "1px solid red");
                        if (firstInvalid == false) {
                            firstInvalid = true;
                            $(this).focus();
                        }
                        if (isValidRowChild != false) {
                            isValidRowChild = false;
                        }
                    }
                    else
                        $(this).css("border", "1px solid #ccc");
                });
            });
        }


        return isValidRow && isValidRowChild;
    },

    rowDown: function ($row, ClassName) {
        ClassName.rowDown($row, EMREditableGrid);
    },
    rowDetail: function ($row, ClassName) {
        ClassName.rowDetail($row, EMREditableGrid);
    },
    rowHistory: function ($row, ClassName) {
        ClassName.rowHistory($row, EMREditableGrid);
    },
    rowUp: function ($row, ClassName) {
        ClassName.rowUp($row, EMREditableGrid);
    },
    rowTitle: function ($row) { },

    GetActions: function (arrActionType, gridId) {
        var actions = "";
        if ($.inArray("Cancel", arrActionType) != -1) {
            actions += '<a class="btn-xs hidden on-editing cancel-row" title="Cancel Record"><i class="fa fa-close black"></i></a>';
        }
        if ($.inArray("Save", arrActionType) != -1) {
            actions += '<a href="#" class="btn-xs hidden on-editing save-row" title="Save Record" ><i class="fa fa-save green"></i></a>';
        }
        if ($.inArray("SaveTitle", arrActionType) != -1) {

            actions += '<a href="#" class="btn-xs hidden on-editing title-row" title="Save Title" ><i class="fa fa-paste black"></i></a>';
        }
        if ($.inArray("Delete", arrActionType) != -1) {

            actions += '<a href="#" class="btn-xs on-default remove-row" title="Delete Record" ><i class="fa fa-close red"></i></a>';
        }
        if ($.inArray("Edit", arrActionType) != -1) {

            actions += '<a href="#" class="btn-xs on-default edit-row" title="Edit Record" ><i class="fa fa-edit black"></i></a>';
        }
        if ($.inArray("Up", arrActionType) != -1) {

            actions += '<a href="#" class="btn-xs on-default up-row" title="Up Record" ><i class="fa fa-arrow-up black"></i></a>';
        }
        if ($.inArray("Down", arrActionType) != -1) {

            actions += '<a href="#" class="btn-xs on-default down-row" title="Down Record" ><i class="fa fa-arrow-down black"></i></a>';
        }
        if ($.inArray("Detail", arrActionType) != -1) {
            actions += '<a href="#" class="btn-xs on-editing row-detail mr-none btn" title="Record Detail" ><i class="fa fa-book blue"></i></a>';
        }
        if ($.inArray("History", arrActionType) != -1) {
            //Start 17-10-2016 Edit by Humaira Yousaf Bug# QAC2-311
            actions += '<a href="#" class="btn-xs row-history mr-none btn" title="Activity Log" ><i class="fa fa-history blue"></i></a>';
            //End 17-10-2016 Edit by Humaira Yousaf Bug# QAC2-311
        }
        if ($.inArray("Active", arrActionType) != -1) {

            var IsCheckedIn = null;
            if (gridId != null) {
                IsCheckedIn = $(gridId + ' #switchActive').attr('isactive');
            } else {
                IsCheckedIn = $('#pnlClinicalProblemLists #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
            }
            var toggleClass = "";
            if (IsCheckedIn == "1" || IsCheckedIn == null) {
                toggleClass = 'class = "fa fa-toggle-on green"'
            } else {
                toggleClass = 'class = "fa fa-toggle-on red"'
            }

            actions += '<a href="#" class="btn-xs on-default remove-active mr-none btn" title="Record Active/Inactive" ><i ' + toggleClass + '></i></a>';

        }

        //Start //30/11/2015 Ahmad Raza //ToggleSwitch
        if ($.inArray("ActiveAllergy", arrActionType) != -1) {

            var IsActive = null;
            IsActive = $('#pnlClinicalAllergies #pnlAllergies_Result #divSwitch #switchActive').attr('isactive');

            var toggleClass = "";
            if (IsActive == "1" || IsActive == null) {
                toggleClass = 'class = "fa fa-toggle-on green"'
            } else {
                toggleClass = 'class = "fa fa-toggle-on red"'
            }

            actions += '<a href="javascript:void(0)" class="btn-xs on-default remove-active mr-none btn" title="Record Active/Inactive" ><i ' + toggleClass + '></i></a>';

        }
        //End //30/11/2015 Ahmad Raza //ToggleSwitch
        if ($.inArray("ActiveImmunizationCategory", arrActionType) != -1) {
            var IsActive = null;
            IsActive = $("#pnlImmunization_Category #frmImmunizationCategory #ddlActiveSearch").val();
            var toggleClass = "";
            if (IsActive == "1" || IsActive == null) {
                toggleClass = 'class = "fa fa-toggle-on green"'
            } else {
                toggleClass = 'class = "fa fa-toggle-on red"'
            }
            actions += '<a href="javascript:void(0)" class="btn-xs on-default remove-active mr-none btn" title="Record Active/Inactive" ><i ' + toggleClass + '></i></a>';
        }
        if ($.inArray("VacCrosswalkActive", arrActionType) != -1) {
            var IsActive = null;
            IsActive = $('#pnlImmunization_VaccineCrosswalk #frmImmunizationVaccineCrosswalk #divSwitch #switchActive').is(':checked');
            var toggleClass = "";
            if (IsActive == "1" || IsActive == null) {
                toggleClass = 'class = "fa fa-toggle-on green"'
            } else {
                toggleClass = 'class = "fa fa-toggle-on red"'
            }
            actions += '<a href="javascript:void(0)" class="btn-xs on-default remove-active mr-none btn" title="Record Active/Inactive" ><i ' + toggleClass + '></i></a>';
        }
        if ($.inArray("VacScheduleSetUpActive", arrActionType) != -1) {
            var IsActive = null;
            IsActive = $('#pnlImmunization_ScheduleSetup #frmImmunizationScheduleSetup #divSwitch #switchActive').is(':checked');
            var toggleClass = "";
            if (IsActive == "1" || IsActive == null) {
                toggleClass = 'class = "fa fa-toggle-on green"'
            } else {
                toggleClass = 'class = "fa fa-toggle-on red"'
            }
            actions += '<a href="javascript:void(0)" class="btn-xs on-default remove-active mr-none btn" title="Record Active/Inactive" ><i ' + toggleClass + '></i></a>';
        }
        if ($.inArray("Payments", arrActionType) != -1) {
            actions += '<a href="#" class="btn-xs btn row-Payment" title="Apply Payments" ><i class="fa fa-usd green"></i></a>';
            
        }
        if ($.inArray("PaymentDetail", arrActionType) != -1) {
            actions += '<a href="#" class="btn-xs btn row-PaymentDetail" title="Payment Detail" ><i class="fa fa-credit-card green"></i></a>';

        }
        return actions;
    },

    addDropDownFieldIMO: function (id, selected_value, method, isoptional, CtrlName, textToFind) {

        var ddl = document.createElement("select");
        ddl.setAttribute("class", "form-control");
        ddl.setAttribute("id", id);
        ddl.setAttribute("name", CtrlName);
        ddl.setAttribute("ddlist", method);
        ddl.setAttribute("isoptional", isoptional);
        // var textToFind = textToFind.substring(0, index);
        if (CtrlName != "Description") {
            $(ddl).append('<option value="">- Select -</option>');
        }



        Admin_IMOICD.SearchICD("", textToFind, 1, 100).done(function (response) {
            //setTimeout(function () {
            if (response.status != false) {
                if (response.ICDCount > 0) {
                    var ICDLoadJSONData = JSON.parse(response.ICDLoad_JSON);
                    $('#pnlClinicalProblemLists #ulProblemDisease').html("");
                    $.each(ICDLoadJSONData, function (i, item) {

                        //Start || 11 April, 2016 || ZeeshanAK || Fix for showing SNOMED ID with ICD9 and 10 under Problem List screen
                        if (item.ICD10 + ' - ' + item.ICD10Description == selected_value) {
                            $(ddl).append('<option id=' + item.ICDId + ' icd9Code=\'' + item.ICD9 + '\' icd9Desc=\'' + item.Description + '\' icd10Code=\'' + item.ICD10 + '\' icd10Desc=\'' + item.ICD10Description + '\' snomedCode=\'' + item.SNOMEDId + '\' snomedDesc=\'' + item.SNOMEDDescription + '\' selected="selected" value="' + item.ICD9 + ' - ' + item.ICD10 + ' - ' + item.SNOMEDId + ' - ' + item.ICD10Description + '">' + item.ICD10 + ' - ' + item.SNOMEDId + ' - ' + item.ICD10Description + '</option>');
                        }

                        else
                            $(ddl).append('<option id=' + item.ICDId + ' icd9Code=\'' + item.ICD9 + '\' icd9Desc=\'' + item.Description + '\' icd10Code=\'' + item.ICD10 + '\' icd10Desc=\'' + item.ICD10Description + '\' snomedCode=\'' + item.SNOMEDId + '\' snomedDesc=\'' + item.SNOMEDDescription + '\' value="' + item.ICD9 + ' - ' + item.ICD10 + ' - ' + item.SNOMEDId + ' - ' + item.ICD10Description + '">' + item.ICD10 + ' - ' + item.SNOMEDId + ' - ' + item.ICD10Description + '</option>');
                        //End   || 11 April, 2016 || ZeeshanAK || Fix for showing SNOMED ID with ICD9 and 10 under Problem List screen

                    });
                    //return ddl;
                }
            }
            //}, 130);
        });

        return ddl;
    },
    addDropDownFieldCategory: function (id, selected_value, method, isoptional, CtrlName, OnChange) {
        var ddl = document.createElement("select");
        ddl.setAttribute("class", "form-control");
        ddl.setAttribute("id", id);
        ddl.setAttribute("name", CtrlName);
        ddl.setAttribute("isoptional", isoptional);
        ddl.setAttribute("onchange", OnChange);
        $(ddl).empty();
        data = "StrID='1'";
        MDVisionService.lookups(method, true, data).done(function (results) {
            results = JSON.parse(results[method]);
            if (results) {
                $.each(results, function (j, result) {
                    $(ddl).append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                });
                $(ddl).find('option').each(function () {
                    if ($.trim($(this).text().toLowerCase()) == selected_value.toLowerCase())
                    { $(ddl).val(this.value); }
                });
            }
        });

        return ddl;
    },
    addDropDownFieldSchedule: function (id, selected_value, method, isoptional, CtrlName, schtypeid) {
        var ddl = document.createElement("select");
        ddl.setAttribute("class", "form-control");
        ddl.setAttribute("id", id);
        ddl.setAttribute("name", CtrlName);
        ddl.setAttribute("isoptional", isoptional);
        $(ddl).empty();
        data = "ID=" + schtypeid;
        MDVisionService.lookups(method, true, data).done(function (results) {
            results = JSON.parse(results[method]);
            if (results) {
                $.each(results, function (j, result) {
                    $(ddl).append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                });
                $(ddl).find('option').each(function () {
                    if ($.trim($(this).text().toLowerCase()) == selected_value.toLowerCase())
                    { $(ddl).val(this.value); }
                });
            }
        });

        return ddl;
    },

    ChangeCurrentSelectedGridObjRowEdit: function ($row) {
        var CurrentRow = $row;
        var selectedRowTable = $($row).closest('table').attr("id");
        var selectedGridPanel = $(EMREditableGrid.options.table).closest("section").attr("id");
        if (EMREditableGrid.options.table.substring(0, EMREditableGrid.options.table.indexOf("#dgv")).trim() == "#pnlClinicalProgressNote #pnlClinicalTreatment" || EMREditableGrid.options.table.substring(0, EMREditableGrid.options.table.indexOf("#dgv")).trim() == "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails")
        { EMREditableGrid.options.table = EMREditableGrid.options.table.substring(0, EMREditableGrid.options.table.indexOf("#dgv")); }
        var PanelGrid = EMREditableGrid.options.table + "#" + selectedGridPanel;
        var GridId = EMREditableGrid.options.table + "#" + selectedRowTable;
        if (GridId.indexOf("#dgvProceduresT") >= 0) {
            GridId = "#pnlClinicalProgressNote #pnlClinicalTreatment #dgvProceduresT"
            PanelGrid = "#pnlClinicalProgressNote #pnlClinicalTreatment #pnlProcedures_ResultT";
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, Treatment_ProcedureListGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowEdit(CurrentRow);
                });

            });

        }
        else if (GridId.indexOf("#dgvProblemListsT") >= 0) {
            GridId = "#pnlClinicalProgressNote #pnlClinicalTreatment #dgvProblemListsT"
            PanelGrid = "#pnlClinicalProgressNote #pnlClinicalTreatment #problemSection";
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, Treatment_ProblemListGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowEdit(CurrentRow);
                });
            });

        }
            //AST-15 BY:MAhmad
        else if (GridId == "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #dgvProceduresOS") {
            PanelGrid = "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #pnlProcedures_ResultOS";
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, OrderSet_ProceduresGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowEdit(CurrentRow);
                });
            });

        }
            //AST-15 BY:MAhmad
            //AST-14 BY:MAhmad
        else if (GridId == "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #dgvProblemListsOS") {
            PanelGrid = "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #pnlProblemLists_ResultOS";
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, OrderSet_ProblemListGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowEdit(CurrentRow);
                });
            });

        }
        else if (GridId.indexOf("dgvPaymentPostDetail")>=0) {
            Bill_Insurance_Payment_Detail.rowEdit(CurrentRow);
        }
            //AST-14 BY:MAhmad
        else {
            EMREditableGrid.rowEdit(CurrentRow);
        }
    },
    ChangeCurrentSelectedGridObjRowSave: function ($row, ClassName) {
        var CurrentRow = $row;
        var CurrentClassName = ClassName;
        var selectedRowTable = $($row).closest('table').attr("id");
        var selectedGridPanel = $(EMREditableGrid.options.table).closest("section").attr("id");
        if (EMREditableGrid.options.table.substring(0, EMREditableGrid.options.table.indexOf("#dgv")).trim() == "#pnlClinicalProgressNote #pnlClinicalTreatment" || EMREditableGrid.options.table.substring(0, EMREditableGrid.options.table.indexOf("#dgv")).trim() == "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails")
        { EMREditableGrid.options.table = EMREditableGrid.options.table.substring(0, EMREditableGrid.options.table.indexOf("#dgv")); }
        var PanelGrid = EMREditableGrid.options.table + "#" + selectedGridPanel;
        var GridId = EMREditableGrid.options.table + "#" + selectedRowTable;
        if (GridId == "#pnlClinicalProgressNote #pnlClinicalTreatment #dgvProceduresT") {
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, Treatment_ProcedureListGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowSave(CurrentRow, CurrentClassName);
                });

            });

        }
        else if (GridId == "#pnlClinicalProgressNote #pnlClinicalTreatment #dgvProblemListsT") {
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, Treatment_ProblemListGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowSave(CurrentRow, ClassName);
                });
            });

        }
            //AST-15 BY:MAhmad
        else if (GridId == "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #dgvProceduresOS") {
            PanelGrid = "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #pnlProcedures_ResultOS";
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, OrderSet_ProceduresGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowSave(CurrentRow, ClassName);
                });
            });

        }
            //AST-15 BY:MAhmad
            //AST-14 BY:MAhmad
        else if (GridId == "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #dgvProblemListsOS") {
            PanelGrid = "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #pnlProblemLists_ResultOS";
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, OrderSet_ProblemListGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowSave(CurrentRow, ClassName);
                });
            });

        }
            //AST-14 BY:MAhmad
        else {
            EMREditableGrid.rowSave(CurrentRow, ClassName);
        }
    },
    ChangeCurrentSelectedGridObjRowRemove: function ($row, ClassName) {
        var CurrentRow = $row;
        var CurrentClassName = ClassName;
        var selectedRowTable = $($row).closest('table').attr("id");
        var selectedGridPanel = $(EMREditableGrid.options.table).closest("section").attr("id");

        if (EMREditableGrid.options.table.substring(0, EMREditableGrid.options.table.indexOf("#dgv")).trim() == "#pnlClinicalProgressNote #pnlClinicalTreatment" || EMREditableGrid.options.table.substring(0, EMREditableGrid.options.table.indexOf("#dgv")).trim() == "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails")
        { EMREditableGrid.options.table = EMREditableGrid.options.table.substring(0, EMREditableGrid.options.table.indexOf("#dgv")); }
        var PanelGrid = EMREditableGrid.options.table + "#" + selectedGridPanel;
        var GridId = EMREditableGrid.options.table + "#" + selectedRowTable;
        if (GridId == "#pnlClinicalProgressNote #pnlClinicalTreatment #dgvProceduresT") {
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, Treatment_ProcedureListGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowRemove(CurrentRow, CurrentClassName);
                });

            });

        }
        else if (GridId == "#pnlClinicalProgressNote #pnlClinicalTreatment #dgvProblemListsT") {
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, Treatment_ProblemListGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowRemove(CurrentRow, ClassName);
                });
            });

        }
            //AST-15 BY:MAhmad
        else if (GridId == "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #dgvProceduresOS") {
            PanelGrid = "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #pnlProcedures_ResultOS";
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, OrderSet_ProceduresGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowRemove(CurrentRow, ClassName);
                });
            });

        }
            //AST-15 BY:MAhmad
            //AST-14 BY:MAhmad
        else if (GridId == "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #dgvProblemListsOS") {
            PanelGrid = "#pnlClinicalOrderSets #pnlClinicalOrderSetDetails #pnlProblemLists_ResultOS";
            $.when(EMRUtility.MakeEditableGrid(PanelGrid, GridId, OrderSet_ProblemListGrid, 0, false, true, false, true, false, null)).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EMREditableGrid.rowRemove(CurrentRow, ClassName);
                });
            });

        }
            //AST-14 BY:MAhmad
        else {
            EMREditableGrid.rowRemove(CurrentRow, ClassName);
        }
    },
    ChangeCurrentSelectedGridObjRowExpand: function ($row, ClassName) {
        var CurrentRow = $row;
        var CurrentClassName = ClassName;
        var selectedRowTable = $($row).closest('table').attr("id");

        if (EMREditableGrid.options.table.substring(0, EMREditableGrid.options.table.indexOf("#dgv")).trim() == "#pnlClinicalProgressNote #pnlClinicalTreatment")
        { EMREditableGrid.options.table = EMREditableGrid.options.table.substring(0, EMREditableGrid.options.table.indexOf("#dgv")); }

        var GridId = EMREditableGrid.options.table + "#" + selectedRowTable;
        if (GridId == "#pnlClinicalProgressNote #pnlClinicalTreatment #dgvImmunizationVaccineHxT") {
            var selectedGridPanel = "pnlVaccineHx_ResultT";
            var PanelGrid = EMREditableGrid.options.table + "#" + selectedGridPanel;
            if ($(EMREditableGrid.$table).attr("id") != "dgvImmunizationVaccineHxT") {
                $.when(Clinical_Treatment.ImmunizationmyGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Clinical_Treatment, 0, false, true, false, true, false, null)).then(function () {
                    utility.callbackAfterAllDOMLoaded(function () {
                        EMREditableGrid.rowExpand(CurrentRow, ClassName);
                    });
                });
            }
            else {
                EMREditableGrid.rowExpand(CurrentRow, ClassName);
            }

        }
        else {
            EMREditableGrid.rowExpand(CurrentRow, ClassName);
        }
    }

};
//////-------------Editable Data Grid Scripts Ends-----------------------
