Patient_MessageCreate = {
    bIsFirstLoad: true,
    params: [],
    TemplateContent: "",
    Patitname: "",
    printdate: "",
    printpriority: "",
    printsubject: "",
    createdby: "",
    UniqueNumber: "",
    UserMessageId: "",
    PatientLetterId: "",
    LetterTemplateName: "",
    LetterStatus: "",
    AttachPatientId: "",
    FilesContainer: { Files: undefined, Name: "Uploaded_Document" },
    Load: function (params) {
        Patient_MessageCreate.params = params;
        Patient_MessageCreate.FilesContainer = { Files: [], Name: "Uploaded_Document" };
        if (Patient_MessageCreate.params["PanelID"] != 'pnlPatientMessageCreate') {
            Patient_MessageCreate.params["PanelID"] = Patient_MessageCreate.params["PanelID"] + ' #pnlPatientMessageCreate';
        }
        if (Patient_MessageCreate.bIsFirstLoad) {
            Patient_MessageCreate.bIsFirstLoad = false;
            var self = $('#' + Patient_MessageCreate.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
                // Patient_MessageCreate.LoadAllAutocomplete();
                $('#frmPatientMessageCreate').data('serialize', $('#frmPatientMessageCreate').serialize());
            });
            if (Patient_MessageCreate.params.mode == 'Add') {
                Patient_MessageCreate.BindUserName();
            }
            Patient_MessageCreate.BindPatientAccount();
        }

        //Patient_MessageCreate.InitTinymceControl(false);
      
        Patient_MessageCreate.Documentready();
        if (Patient_MessageCreate.params.mode == 'Add') {
            Patient_MessageCreate.ValidateMessageCreate();
            $("#" + Patient_MessageCreate.params["PanelID"] + " #btnReply,#btnForward,#btnViewTask,#btnTask,#linkPrint").css("display", "none");
            $("#" + Patient_MessageCreate.params["PanelID"] + " #btnSend").css("display", "inline");
            $("#" + Patient_MessageCreate.params["PanelID"] + " #divDownloadLink").css("display", "none");
            $("#" + Patient_MessageCreate.params["PanelID"] + " #divDownloadLinkHTML").css("display", "none");
            $("#" + Patient_MessageCreate.params["PanelID"] + " #divPrintMessage").css("display", "none");
            // $("#" + Patient_MessageCreate.params["PanelID"] + " #linkPrint").prop("disabled", true);
            //adnan maqbool, EMR-910
            $("#" + Patient_MessageCreate.params["PanelID"] + " #btnSend").prop("disabled", true);
            $("#" + Patient_MessageCreate.params["PanelID"] + " #titleMessage").text('Compose Message');
            UniqueNumber = Patient_MessageCreate.GenerateUUID();
            //Start 24-08-2016 Humaira Yousaf for referral message
            if (Patient_MessageCreate.params.ParentCtrl == "mstrTabDashBoard") {
                $("#" + Patient_MessageCreate.params["PanelID"] + " #hftxtTo").val(Patient_MessageCreate.params.AssignedToId);
                $("#" + Patient_MessageCreate.params["PanelID"] + " #txtTo").val(Patient_MessageCreate.params.AssignedName);
                $("#" + Patient_MessageCreate.params["PanelID"] + " #hfPatientId").val(Patient_MessageCreate.params.PatientId);
                $("#" + Patient_MessageCreate.params["PanelID"] + " #txtMessageDtl").html(Patient_MessageCreate.params.Message);
                //Start 14-10-2016 Humaira Yousaf for Referral Reminder
                if (Patient_MessageCreate.params.Caller == "Referrals") {
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #txtSubject").val(Patient_MessageCreate.params.MsgSubject);
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #btnSend").prop("disabled", false);
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #divAttachPatientwithPractice").addClass('hidden');
                }
                else {
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #divAttachPatientwithPractice").removeClass('hidden');
                }
                //End 14-10-2016 Humaira Yousaf for Referral Reminder
                //$("#" + Patient_MessageCreate.params["PanelID"] + " #divSwitch").addClass('hidden');
            }
            else {
                //$("#" + Patient_MessageCreate.params["PanelID"] + " #divSwitch").removeClass('hidden');
            }
            //End 24-08-2016 Humaira Yousaf for referral message
        } else if (Patient_MessageCreate.params.mode == 'Edit') {

            $("#" + Patient_MessageCreate.params["PanelID"] + " #btnReply,#btnForward,#btnTask").css("display", "inline");
            $("#" + Patient_MessageCreate.params["PanelID"] + " #btnSend,#btnTask,#Patientinputtext").css("display", "none");
            // $("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageToPat").html('Attached Patient');
            $("#" + Patient_MessageCreate.params["PanelID"] + " #divAttatchFile,#linkDownload,#linkDownloadHTML").css("display", "none");
            $("#" + Patient_MessageCreate.params["PanelID"] + " #divPrintMessage").css("display", "inline");
            $("#" + Patient_MessageCreate.params["PanelID"] + " #ddlPriority").prop("disabled", true);
            UniqueNumber = "";
            if (Patient_MessageCreate.params.UserMessageId != null) {
                Patient_MessageCreate.FillUserMessage();
            }
        }
        if (Patient_MessageCreate.params.MessageType == "Patient") {
            $("#" + Patient_MessageCreate.params["PanelID"] + " #divAttachPatientwithPractice,#divProvider").css("display", "none");
            $("#" + Patient_MessageCreate.params["PanelID"] + " #divPatient").css("display", "inline");
        }
        if (Patient_MessageCreate.params.ParentCtrl == "Patient_Message") {
            $("#" + Patient_MessageCreate.params["PanelID"] + " #divPatientSearch").css("display", "none");
            $("#" + Patient_MessageCreate.params["PanelID"] + " #hfPatientId").val($("#PatientProfile #hfPatientId").val());
        } else {
            $("#" + Patient_MessageCreate.params["PanelID"] + " #divPatientSearch").css("display", "inline");
        }
        if (Patient_MessageCreate.params.PatientId)
        { $("#" + Patient_MessageCreate.params["PanelID"] + " #frmPatientMessageCreate #hfPatientId").val(Patient_MessageCreate.params.PatientId); }
        //Patient_MessageCreate.Documentready();
        $('#frmPatientMessageCreate').data('serialize', $('#frmPatientMessageCreate').serialize());
    },


    ViewHtmlForXmlFile: function (fileName) {

        var f;
        for (var i = 0; i < Patient_MessageCreate.FilesContainer.Files.length; i++) {
            if (Patient_MessageCreate.FilesContainer.Files[i].name == fileName)
                f = Patient_MessageCreate.FilesContainer.Files[i];
        }

        if (f) {

            var r = new FileReader();
            r.onload = function (e) {
                var contents = e.target.result;
                contents = contents.replace(/&amp;/g, 'and');
                data = "XMLContent=" + contents;

                MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "XML_TO_HTML").done(function (response) {
                    if (response.status) {
                        if (response.data) {
                            var params = [];
                            params["FromAdmin"] = "0";
                            params["ParentCtrl"] = 'Patient_MessageCreate';
                            params["filePath"] = response.data;
                            LoadActionPan('Patient_MessageXmlView', params);
                        }
                    }
                });

            }
            r.readAsText(f);
        }



    },



    ViewUserTask: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_MessageCreate';

        params["UserMessageId"] = Patient_MessageCreate.UserMessageId;
        LoadActionPan('Patient_TaskDetail', params);

    },
    ViewUserLetter: function () {
        var params = [];
        params["ParentCtrl"] = "Patient_MessageCreate";
        params["Status"] = Patient_MessageCreate.LetterStatus;
        params["FromAdmin"] = 0;
        params["Patient_Letter_Id"] = Patient_MessageCreate.PatientLetterId;
        params["TemplateLetterText"] = Patient_MessageCreate.LetterTemplateName;
        params["mode"] = "Edit";
        params["PatientId"] = Patient_MessageCreate.AttachPatientId;
        LoadActionPan("Create_Letter", params);


    },
    LoadAllAutocomplete: function () {
        CacheManager.BindCodes('GetUsers', true, 1).done(function (result) {
            $("#" + Patient_MessageCreate.params["PanelID"] + " #txtTo").autocomplete({
                autoFocus: true,
                source: Users, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Patient_MessageCreate.params["PanelID"] + " #hftxtTo").val(ui.item.id); // add the selected id
                        //adnan maqbool, EMR-910
                        $("#" + Patient_MessageCreate.params["PanelID"] + " #btnSend").prop("disabled", false);
                    }, 100);
                }
            });
        });

    },
    Documentready: function (obj) {
        (function ($) {
            'use strict';
            $(function () {
                $('#pnlPatientMessageCreate [data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);

        $('#' + Patient_MessageCreate.params["PanelID"] + ' #divPatient').hide();
        $('#' + Patient_MessageCreate.params["PanelID"] + ' #divphiMailAddress').hide();
        $('#' + Patient_MessageCreate.params["PanelID"] + ' #isPatSelected').val("0");
    },
    ChangeStatus: function (obj) {
        if ($(obj).attr('status') == '1') {

            $(obj).attr('status', 0);
            $('#frmPatientMessageCreate .btnWidgetSwitch').find('div [class="state-background background-fill"]').css({ "background": "#47a447", "border-color": "#47a447" });
            $('#' + Patient_MessageCreate.params["PanelID"] + ' #divPatient').show();
            $('#' + Patient_MessageCreate.params["PanelID"] + ' #divProvider').hide();
            $('#' + Patient_MessageCreate.params["PanelID"] + ' #isPatSelected').val("1");
            if ($('#frmPatientMessageCreate').data('bootstrapValidator') != null && typeof $('#frmproviderscheduleDetail').data('bootstrapValidator') != 'undefined') {
                $('#frmPatientMessageCreate').data('bootstrapValidator').enableFieldValidators('AttatchPatient', true);
                $('#frmPatientMessageCreate').data('bootstrapValidator').enableFieldValidators('MessageTo', false);
            }
        } else {
            $(obj).attr('status', 1);
            $('#' + Patient_MessageCreate.params["PanelID"] + ' #divPatient').hide();
            $('#' + Patient_MessageCreate.params["PanelID"] + ' #divProvider').show();
            $('#' + Patient_MessageCreate.params["PanelID"] + ' #isPatSelected').val("0");

            if ($('#frmPatientMessageCreate').data('bootstrapValidator') != null && typeof $('#frmproviderscheduleDetail').data('bootstrapValidator') != 'undefined') {
                $('#frmPatientMessageCreate').data('bootstrapValidator').enableFieldValidators('AttatchPatient', true);
                $('#frmPatientMessageCreate').data('bootstrapValidator').enableFieldValidators('MessageTo', false);
            }

        }
    },

    BindUserName: function () {
        var Ctrl = $('#' + Patient_MessageCreate.params["PanelID"] + ' #txtTo');
        var hfCtrl = $('#' + Patient_MessageCreate.params["PanelID"] + ' #hftxtTo');
        var func = function () { return utility.GetUserArraywithPractice(Ctrl.val(), 1, 1) };
        var onSelect = function (e) {
            $("#" + Patient_MessageCreate.params["PanelID"] + " #btnSend").prop("disabled", false);
            if ($('#frmPatientMessageCreate').data('bootstrapValidator') != null && typeof $('#frmPatientMessageCreate').data('bootstrapValidator') != 'undefined') {
                $('#frmPatientMessageCreate').bootstrapValidator('revalidateField', 'MessageTo');
            }
        };
        var onChange = function (valid) {
            if (Ctrl.val()) {

                $('#pnlPatientMessageCreate #btnSend').prop("disabled", false);
            } else {
                $('#pnlPatientMessageCreate #btnSend').prop("disabled", true);

            }
        }
        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", null, func, hfCtrl, onSelect, onChange);

        var UserName = $('#' + Patient_MessageCreate.params["PanelID"] + ' #txtTo').val();
        if (UserName) {
            utility.Keyupdelay(function () {
                var AllPatients = utility.GetUserArraywithPractice(UserName, 1, 1).done(function (response) {

                    $('#' + Patient_MessageCreate.params["PanelID"] + ' #txtTo').autocomplete({
                        autoFocus: true,
                        source: response,
                        minLength: 0,
                        open: function (event, ui) { disable = true },
                        close: function (event, ui) {
                            disable = false; $(this).focus();
                        },
                        select: function (event, ui) {
                            setTimeout(function () {
                                $('#' + Patient_MessageCreate.params["PanelID"] + ' #txtTo').val(ui.item.value);
                                $('#' + Patient_MessageCreate.params["PanelID"] + ' #hftxtTo').val(ui.item.id);
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #btnSend").prop("disabled", false);
                                if ($('#frmPatientMessageCreate').data('bootstrapValidator') != null && typeof $('#frmPatientMessageCreate').data('bootstrapValidator') != 'undefined') {
                                    $('#frmPatientMessageCreate').bootstrapValidator('revalidateField', 'MessageTo');
                                }
                            }, 100);
                        }
                    }).blur(function () {
                        setTimeout(function () {
                            utility.ValidateAutoComplete($('#' + Patient_MessageCreate.params["PanelID"] + ' #txtTo'), "frmPatientMessageCreate #hftxtTo", false, null, null, null);
                            setTimeout(function () {
                                if ($('#' + Patient_MessageCreate.params["PanelID"] + ' #txtTo').val()) {

                                    $('#pnlPatientMessageCreate #btnSend').prop("disabled", false);
                                } else {
                                    $('#pnlPatientMessageCreate #btnSend').prop("disabled", true);

                                }
                            }, 200);
                        }, 200);
                    });


                    $('#' + Patient_MessageCreate.params["PanelID"] + ' #txtTo').autocomplete("search", "");
                });
            });
        }
        
    },

    LoadUsers: function (userstring) {

        var objData = new Object();
        objData["Username"] = userstring;
        objData["CommandType"] = "get_users";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");

    },

    BindPatientAccount: function () {
        if (Patient_MessageCreate.params.MessageType == "Patient") {
            var Ctrl = $('#' + Patient_MessageCreate.params["PanelID"] + ' #divPatient #txtAttatchPatient');
        } else if (Patient_MessageCreate.params.MessageType == "Practice") {
            var Ctrl = $('#' + Patient_MessageCreate.params["PanelID"] + ' #divAttachPatientwithPractice #txtAttatchPatient');
        } else {
            var Ctrl = $('#' + Patient_MessageCreate.params["PanelID"] + '  #txtAttatchPatient');
        }
        if (Patient_MessageCreate.params.MessageType == "Patient") {
            var func = function () { return Patient_MessageCreate.GetPatientArrayByNameArray(Ctrl.val(), 1) };
        } else {
            var func = function () { return utility.GetPatientArrayByName(Ctrl.val(), 1) };
        }

        var hfCtrl = $('#' + Patient_MessageCreate.params["PanelID"] + ' #hfPatientId');
        var onSelect = function (e) { $("#" + Patient_MessageCreate.params["PanelID"] + " #btnSend").prop("disabled", false); };
        var onChange = function () {
            if (Patient_MessageCreate.params.MessageType == "Patient") {
                if ($('#' + Patient_MessageCreate.params["PanelID"] + ' #divPatient #txtAttatchPatient').val()) {

                    $('#pnlPatientMessageCreate #btnSend').prop("disabled", false);
                } else {
                    $('#pnlPatientMessageCreate #btnSend').prop("disabled", true);

                }
            }
        }
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect, onChange);
        if (Patient_MessageCreate.params.mode == "Add") {
            if ($(Ctrl).data('kendoAutoComplete')) {
                utility.SetKendoAutoCompleteSourceforValidate($(Ctrl), localStorage.SelectedAccountNumber + ' - ' + $("#PatientProfile #banner_PatientName").html(), $('#' + Patient_MessageCreate.params["PanelID"] + ' #hfPatientId'), localStorage.SelectedPatientId);
            }
        }
    },

    GetPatientArrayByNameArray: function (name) {
        var dfd = new $.Deferred();
        utility.GetPatientArrayByName(name, 1).done(function (response) {
            response = $.grep(response, function (envy) {
                return envy.PatientPortalStatus == '1';
            });
            dfd.resolve(response);
        });
        return dfd.promise();
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_MessageCreate';
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNumber, FullName, event) {
        if (event != null) {
            event.stopPropagation();
            event.stopPropagation();
            if (event.target.type == "checkbox") {
                $(':checkbox', this).trigger('click');
                return;
            }
        }
        var Ctrl = $('#' + Patient_MessageCreate.params["PanelID"] + ' #divAttachPatientwithPractice #txtAttatchPatient');
        Ctrl.val(AccountNumber.trim() + " - " + FullName.trim());
        $('#' + Patient_MessageCreate.params["PanelID"] + ' #hfPatientId').val(PatientId);
        utility.SetKendoAutoCompleteSourceforValidate(Ctrl, AccountNumber.trim() + " - " + FullName, null, PatientId);

        UnloadActionPan("Patient_MessageCreate");
    },
    GetDayName: function (d) {
        var weekday = new Array(7);
        weekday[0] = "Sunday";
        weekday[1] = "Monday";
        weekday[2] = "Tuesday";
        weekday[3] = "Wednesday";
        weekday[4] = "Thursday";
        weekday[5] = "Friday";
        weekday[6] = "Saturday";

        var n = weekday[d.getDay()];
        return n;
    },
    FillUserMessage: function () {
        AppPrivileges.GetFormPrivileges("Messages", "Add", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_MessageCreate.FillUserMessage_DBCall().done(function (response) {


                    if (response.status != false) {
                        var messageDetal = JSON.parse(response.userMessagesFill_JSON);
                        var self = $("#" + Patient_MessageCreate.params["PanelID"]);
                        Patitname = messageDetal.txtnewPatientName;
                        printdate = messageDetal.CreatedOn;
                        printpriority = messageDetal.Priorityname;
                        printsubject = messageDetal.Subject;
                        createdby = messageDetal.CreatedBy;
                        UniqueNumber = messageDetal.UniqueNumber;
                        UserMessageId = messageDetal.UserMessageId;
                        if (printdate) {
                            var a = new Date(printdate);

                            var daydate = a.toDateString();
                            var trimmeddate = daydate.substring(3);
                            var settime = a.toLocaleTimeString();

                            var dayname = Patient_MessageCreate.GetDayName(a);

                            printdate = dayname + ", " + trimmeddate + ", " + settime;
                        }



                        utility.bindMyJSONByName(true, messageDetal, false, self).done(function () {
                            if (messageDetal.Priority) {
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #ddlPriority").val(messageDetal.Priority);
                                if (messageDetal.Priority == 1) {
                                    $("#" + Patient_MessageCreate.params["PanelID"] + " #ddlPriority option:eq(0)").parent().css('color', 'red');
                                } else if (messageDetal.Priority == 2) {
                                    $("#" + Patient_MessageCreate.params["PanelID"] + " #ddlPriority option:eq(0)").parent().css('color', 'orange');
                                } else {
                                    $("#" + Patient_MessageCreate.params["PanelID"] + " #ddlPriority option:eq(0)").parent().css('color', 'green');
                                }
                                //$("#" + Patient_MessageCreate.params["PanelID"] + " #ddlPriority option:eq(1)").css('color', 'red');

                                //$("#" + Patient_MessageCreate.params["PanelID"] + " #ddlPriority option:eq(2)").css('color', 'orange');

                                //$("#" + Patient_MessageCreate.params["PanelID"] + " #ddlPriority option:eq(3)").css('color', 'green');
                            }
                            if (messageDetal.IsTask == 'True') {
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #btnViewTask").css('display', 'inline');
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #btnTask").css('display', 'none');
                            } else {
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #btnViewTask").css('display', 'none');
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #btnTask").css('display', 'inline');
                            }
                            if (Patient_MessageCreate.params.MessageType == 'Practice') {
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #txtTo").val(messageDetal.UserNameWithPracticeFrom);
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #txtTo").prop("disabled", true);
                                if (messageDetal.PatientLetterId) {
                                    $("#" + Patient_MessageCreate.params["PanelID"] + " #btnViewLetter").css('display', 'inline');
                                    Patient_MessageCreate.PatientLetterId = messageDetal.PatientLetterId;
                                    Patient_MessageCreate.LetterTemplateName = messageDetal.LetterTemplateName;
                                    Patient_MessageCreate.LetterStatus = messageDetal.LetterStatus;
                                    Patient_MessageCreate.AttachPatientId = messageDetal.AttatchedPatientId;

                                }
                                if (messageDetal.AttatchedPatientId) {
                                    $("#" + Patient_MessageCreate.params["PanelID"] + " #hfPatientId").val(messageDetal.AttatchedPatientId);
                                    $("#" + Patient_MessageCreate.params["PanelID"] + " #divPatientSearch").css('display', 'none');
                                    $("#" + Patient_MessageCreate.params["PanelID"] + " #txtTo").val(messageDetal.UserNameWithPracticeFrom);

                                    Patient_Demographic.FillDemographic(messageDetal.AttatchedPatientId).done(function (response) {
                                        var parentctrl = 'Patient_MessageCreate';
                                        var PatientProfileInfo = JSON.parse(response.DemographicFill_JSON);
                                        $("#" + Patient_MessageCreate.params["PanelID"] + " #Patientlink").append('<a href="#" data-toggle="tooltip" onclick="utility.PatientDemographics(' + messageDetal.AttatchedPatientId + ', \'' + parentctrl + '\', event);" title="">' + PatientProfileInfo.FullName + '</a>');
                                        $("#" + Patient_MessageCreate.params["PanelID"] + " #Patientlink").css('display:inline');
                                        $("#" + Patient_MessageCreate.params["PanelID"] + " #Patientlink").show();
                                        var PatientProfileInfo = JSON.parse(response.DemographicFill_JSON);
                                        Patient_MessageCreate.Createpatientinfopan(PatientProfileInfo, messageDetal.hfRace, messageDetal.hfEthnicity);
                                        //Set ToolTip for Comments.
                                        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
                                        $('#frmPatientMessageCreate').data('serialize', $('#frmPatientMessageCreate').serialize());
                                    });
                                } else {

                                    $("#" + Patient_MessageCreate.params["PanelID"] + " #divAttachPatientwithPractice #lblMessageToPat").css('display', 'none');

                                }

                                if (messageDetal.Subject == "Document Privacy Check") {
                                    $("#" + Patient_MessageCreate.params["PanelID"] + " #frmPatientMessageCreate #linkPrint").hide();
                                } else {
                                    $("#" + Patient_MessageCreate.params["PanelID"] + " #frmPatientMessageCreate #linkPrint").show();
                                }
                            } else if (Patient_MessageCreate.params.MessageType == 'Patient') {
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #divProvider #txttopractice").css('display', 'none');
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #divPatient").css('display', 'inline');
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #divAttachPatientwithPractice").css('display', 'none');
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #hfPatientId").val(messageDetal.AttatchedPatientId);
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #divPatient #txtAttatchPatient").prop("disabled", true);
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #divPatient #txtAttatchPatient").val(messageDetal.txtaccountnumber + " - " + messageDetal.txtnewPatientName);
                            } else if (Patient_MessageCreate.params.MessageType == 'DIRECT') {
                                $('#frmPatientMessageCreate #divButtons').hide();
                                $('#frmPatientMessageCreate #divAttachPatientwithPractice').hide();
                                $('#frmPatientMessageCreate #ddlPriority').parent().hide();
                            }

                            //Start || 24 May, 2016 || ZeeshanAK || Changes for showing HTML file with XML for phiMail messages
                            if (messageDetal.Documents != "") {
                                var docs = JSON.parse(messageDetal.Documents)

                                $.each(docs, function (index, element) {
                                    $("#divfilesicons").append('<div class=" col-sm-4"><a class="btn btn-success btn-xs btn-and-anchor size100per" id="linkDownload" download=' + element[2] + ' href="data:' + element[1] + ';base64,' + element[0] + '"title="Download File"><i class="fa fa-download"></i><span class="size-max90per ellipses pull-left">' + element[2] + '</a></div>');
                                });

                            } else {
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #divDownloadLink").css("display", "none");
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #divDownloadLinkHTML").css("display", "none");

                            }


                            if (messageDetal.ProviderId != "" || messageDetal.IsPatientMessage != "") {
                                //$("#" + Patient_MessageCreate.params["PanelID"] + " #rdPatient").trigger("click");
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #txtAttatchPatient").val(messageDetal.CreatedBy);
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #txtAttatchPatient").prop("disabled", true);
                                //$("#" + Patient_MessageCreate.params["PanelID"] + " #rdProvider").prop("disabled", true);
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #lnkPatientAccount").prop("disabled", true);
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #divPatient").show();
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #divProvider").hide();
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageToPat").text("From");
                                $("#" + Patient_MessageCreate.params["PanelID"] + " .btnWidgetSwitch").children().first().removeClass('ios-switch on').addClass('ios-switch off');
                                $("#" + Patient_MessageCreate.params["PanelID"] + " .btnWidgetSwitch").addClass('disableAll')
                                $('#' + Patient_MessageCreate.params["PanelID"] + ' #isPatSelected').val("1");
                                $('#' + Patient_MessageCreate.params["PanelID"] + ' .btnWidgetSwitch').find('div [class="state-background background-fill"]').css({ "background": "#47a447", "border-color": "#47a447" });
                                //$('#' + Patient_MessageCreate.params["PanelID"] + ' #switchVisit').attr('status', '0');

                            } else if (messageDetal.ProviderId == "" && messageDetal.IsPatientMessage == "") {
                                //$("#" + Patient_MessageCreate.params["PanelID"] + " #rdProvider").trigger("click");

                                //if (messageDetal.hfMessageTo == "") {
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #txtTo").val('');
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #divProvider").hide();
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #divPatient").hide();
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #divphiMailAddress").show();
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #phiMAilAddress").prop("disabled", true);
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #btnForward").prop("disabled", true);
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #btnReply").prop("disabled", true);

                                //} else {
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #divPatient").hide();
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #divProvider").show();
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #divphiMailAddress").hide();
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #txtTo").val(messageDetal.CreatedBy);
                                //    $("#" + Patient_MessageCreate.params["PanelID"] + " #txtTo").prop("disabled", true);
                                //}
                                //End   || 24 May, 2016 || ZeeshanAK || Changes for showing HTML file with XML for phiMail messages

                                //$("#" + Patient_MessageCreate.params["PanelID"] + " #rdPatient").prop("disabled", true);
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageToProv").text("From Practice/User");
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageToPat").text("From Patient");
                                $("#" + Patient_MessageCreate.params["PanelID"] + " .btnWidgetSwitch").children().first().removeClass('ios-switch on').addClass('ios-switch on');
                                $("#" + Patient_MessageCreate.params["PanelID"] + " .btnWidgetSwitch").addClass('disableAll')
                                $('#' + Patient_MessageCreate.params["PanelID"] + ' #isPatSelected').val("0");
                                //  $('#' + Patient_MessageCreate.params["PanelID"] + ' #switchVisit').attr('status', '1');
                            }

                            $("#" + Patient_MessageCreate.params["PanelID"] + ' #ddlPriority').val(messageDetal.Priority);
                            $('#frmPatientMessageCreate').data('serialize', $('#frmPatientMessageCreate').serialize());
                        });

                    } else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                    $('#frmPatientMessageCreate').data('serialize', $('#frmPatientMessageCreate').serialize());
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    Createpatientinfopan: function (Patientinforesponse, race, ethinicity) {
        var Name = Patientinforesponse.FirstName == "" ? "" : '<b>Name:</b> ' + Patientinforesponse.LastName + ', ' + Patientinforesponse.FirstName + '<br>';
        var DOB = Patientinforesponse.DOB == "" ? "" : '<b>DOB: </b>' + Patientinforesponse.DOB.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '<br>'
        var Sex = Patientinforesponse.Sex == "" ? "" : '<b>Sex:</b> ' + Patientinforesponse.Sex + '<br>'
        var AccountNo = Patientinforesponse.AccountNo == "" ? "" : '<b>Account No:</b> ' + Patientinforesponse.AccountNo + '<br>'
        var MaritalStatus = Patientinforesponse.MaritalStatus == "" ? "" : '<b>Marital Status:</b> ' + Patientinforesponse.MaritalStatus + '<br>'
        var Race = Patientinforesponse.Race == "" ? "" : '<b>Race:</b> ' + race + '<br>'
        var Ethnicity = Patientinforesponse.Ethnicity == "" ? "" : '<b>Ethinicity:</b> ' + ethinicity + '<br>'
        var PrefLanguage = Patientinforesponse.PrefLanguage == "" ? "" : '<b>Prefered Language:</b> ' + Patientinforesponse.PrefLanguage + '<br>'
        var Address1 = Patientinforesponse.Address1 == "" ? "" : '<b>Address:</b> ' + Patientinforesponse.Address1 + '<br>'
        var City = Patientinforesponse.City == "" ? "" : '<b>City:</b> ' + Patientinforesponse.City + '<br>'
        var State = Patientinforesponse.State == "" ? "" : '<b>State:</b> ' + Patientinforesponse.State + '<br>'
        var Zip = Patientinforesponse.Zip == "" ? "" : '<b>Zip:</b> ' + Patientinforesponse.Zip + '<br>'
        var HomeTel = Patientinforesponse.HomeTel == "" ? "" : '<b>Home Tel:</b> ' + Patientinforesponse.HomeTel + '<br>'
        var Cell = Patientinforesponse.Cell == "" ? "" : '<b>Cell:</b> ' + Patientinforesponse.Cell + '<br>'
        var Email = Patientinforesponse.Email == "" ? "" : '<b>Email:</b> ' + Patientinforesponse.Email + '<br>'
        var WorkTel = Patientinforesponse.WorkTel == "" ? "" : '<b>Work Tel:</b> ' + Patientinforesponse.WorkTel + '<br>'

        var popover;
        popover = '<p style="width:180px;" >'
                               + Name
                               + DOB
                               + Sex
                               + AccountNo
                               + MaritalStatus
                               + Race
                               + Ethnicity
                               + PrefLanguage
                               + Address1
                               + City
                               + State
                               + Zip
                               + HomeTel
                               + WorkTel
                               + Cell
                               + Email;
        $("#" + Patient_MessageCreate.params["PanelID"] + " #Patientlink").tooltip({
            title: popover,
            html: true,
            placement: "right"
        });
    },
    FillUserMessage_DBCall: function () {

        var objData = new Object();
        objData["UserMesgId"] = Patient_MessageCreate.params.UserMessageId;
        objData["CommandType"] = "fill_practice_message";
        objData["MessageType"] = Patient_MessageCreate.params.MessageType;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");

    },

    AddTask: function (messageid) {
        var params = [];
        params["PatientId"] = $('#' + Patient_MessageCreate.params["PanelID"] + ' #hfPatientId').val();
        params["PatientName"] = $('#' + Patient_MessageCreate.params["PanelID"] + ' #txtAttatchPatient').val();
        params["UserMessageId"] = Patient_MessageCreate.UserMessageId;
        params["mode"] = 'Add';
        params["UnloadRef"] = 'UnloadWindows';
        params["FromPatModule"] = Patient_MessageCreate.params.FromPatModule;
        params["FromQuicklink"] = Patient_MessageCreate.params.FromQuicklink;
        params["ParentCtrl"] = "Patient_MessageCreate";
        params["AssignedToId"] = null;
        params["MessageDetail"] = $('#' + Patient_MessageCreate.params["PanelID"] + ' #txtMessageDtl').val();
        LoadActionPan('Patient_MessageAdd', params);
    },

    ValidateMessageCreate: function () {
        $('#frmPatientMessageCreate')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   MessageTo: {
                       group: '.provider',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Subject: {
                       group: '.col-sm-12',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   MessageDtl: {
                       group: '.col-sm-12',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   toattachptn: {
                       group: '.patient',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Patient_MessageCreate.SaveCreateMessage();
        });
    },
    enabledisablesend: function (obj) {
        if ($(obj).val()) {
            $('#pnlPatientMessageCreate #btnSend').prop("disabled", false);
        } else {
            $('#pnlPatientMessageCreate #btnSend').prop("disabled", true);

        }
    },
    SaveCreateMessage: function () {

        //var data = new FormData();

        //$.each(Patient_MessageCreate.FilesContainer.Files, function (key, value) {
        //    data.append(key, value);
        //});
        //Patient_MessageCreate.FilesContainer = { Files: undefined, Name: "Uploaded_Document" };
        var dfd = new $.Deferred();
        var files = [];
        var FilePath = [];
        var FileType = [];
        var count = 0;

        if (Patient_MessageCreate.FilesContainer.Files != undefined) {
            if (Patient_MessageCreate.FilesContainer.Files.length > 0) {
                $.each(Patient_MessageCreate.FilesContainer.Files, function (key, value) {
                    var oFReader = new FileReader();
                    oFReader.readAsDataURL(Patient_MessageCreate.FilesContainer.Files[key]);
                    oFReader.onload = function (oFREvent) {
                        //data:image/jpeg;base64,
                        var file_ = oFREvent.target.result.split('base64,');
                        files.push(file_[1]);
                        FilePath.push(Patient_MessageCreate.FilesContainer.Files[key].name);
                        FileType.push(Patient_MessageCreate.FilesContainer.Files[key].type);
                        count++;
                        if (Patient_MessageCreate.FilesContainer.Files.length == count)
                            dfd.resolve('ok');
                        //else
                        //  dfd.resolve('ok');
                    };

                });
            }
            else if (Patient_MessageCreate.FilesContainer.Files.length == 0) {
                dfd.resolve('ok');
            }
        }

        dfd.then(function () {

            var strMessage = "";
            var self = $('#' + Patient_MessageCreate.params["PanelID"]);
            var myJSON = self.getMyJSONByName();

            //var objData = JSON.parse(myJSON);



            //AppPrivileges.GetFormPrivileges("Message Reply", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {

            if (Patient_MessageCreate.params.mode = "Add") {
                var filestype = JSON.stringify(FileType);
                var filespath = JSON.stringify(FilePath);
                Patient_MessageCreate.SaveCreateMessage_DBCall(myJSON, files, filestype, filespath).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages("Successfully sent!", 1);
                        if (response.IsMUAlertUpdated && ($("#MissingDataAlertsLabel").css('display') == 'inline') || $("#MissingDataAlertsLabel").css('display') == 'block')
                        {
                            var number_ = $("#MissingDataAlertsLabel").html().split('M')[1].split('-')[1].trim();
                            if (number_ && parseInt(number_) > 0)
                                utility.toggelMU3Alerts(true, parseInt(number_) - 1);
                        }

                        if ((Patient_MessageCreate.params.ParentCtrl == "mstrTabDashBoard" && Patient_MessageCreate.params.Caller != "Referrals") || Patient_MessageCreate.params.ParentCtrl == "patTabUserMessages" || Patient_MessageCreate.params.ParentCtrl == "Patient_UserMessagesQuickLink") {
                            $('#pnlPatientMessageCreate #ddlPriority').val("");
                            Patient_MessageCreate.UserMessageId = response.UserMessageId;
                            // $('#pnlPatientMessageCreate #ddlPriority').val(""); 
                            if (Patient_MessageCreate.params.MessageType == "Patient") {
                                if (Patient_MessageCreate.params.FromPatModule == "1") {
                                    Patient_UserMessages.SearchPatientMessage();
                                } else {
                                    if (Patient_MessageCreate.params.FromQuicklink == "1") {
                                        Patient_UserMessagesQuickLink.SearchPatientMessage();
                                    }
                                    DashBoard.DashBoardPatientMessagesSearch();
                                }

                            } else {
                                if (Patient_MessageCreate.params.FromPatModule == "1") {
                                    Patient_UserMessages.SearchPracticeMessage();
                                } else {
                                    if (Patient_MessageCreate.params.FromQuicklink == "1") {
                                        Patient_UserMessagesQuickLink.SearchPracticeMessage();
                                    }
                                    DashBoard.DashBoardMessagesSearch();
                                }

                            }

                        }
                        //Start 14-10-2016 Humaira Yousaf for Referral Reminder
                        if (Patient_MessageCreate.params.Isopentask != "1" && Patient_MessageCreate.params.Caller != "Referrals") {
                            //End 14-10-2016 Humaira Yousaf for Referral Reminder
                            utility.myConfirm('38', function () {

                                Patient_MessageCreate.AddTask();
                                //UnloadActionPan(Patient_MessageCreate.params.ParentCtrl, 'pnlPatientMessageCreate');
                            }, function () {
                                UnloadActionPan(Patient_MessageCreate.params.ParentCtrl, 'pnlPatientMessageCreate');
                            },
            '38'
                    );
                        } else {
                            UnloadActionPan(Patient_MessageCreate.params.ParentCtrl, 'pnlPatientMessageCreate');
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            //    }
            //    else
            //        utility.DisplayMessages(strMessage, 2);
            //});

        });


        //---------------------------------



    },

    SaveCreateMessage_DBCall: function (messagesData, filedata, filetype, filepath) {
        var objData = JSON.parse(messagesData);
        objData["MessageDtl123"] = $("#" + Patient_MessageCreate.params.PanelID + " #txtMessageDtl").val();
        objData["CommandType"] = "save_practice_message";
        objData["MessageType"] = Patient_MessageCreate.params.MessageType;
        objData["Files"] = filedata;
        objData["UniqueNumber"] = UniqueNumber;
        objData["FileType"] = filetype;
        objData["FilePath"] = filepath;
        var usernamewithpractice = globalAppdata.AppUserFirstName + ' ' + globalAppdata.AppUserLastName;
        if (globalAppdata.DefaultPracticeName && globalAppdata.DefaultPracticeName != "undefined" && globalAppdata.DefaultPracticeName != null)
            usernamewithpractice = globalAppdata.AppUserFirstName + ' ' + globalAppdata.AppUserLastName + ' - ' + globalAppdata.DefaultPracticeName;
        objData["UsernameFrom"] = usernamewithpractice;
        //if (Patient_MessageCreate.FilesContainer["Files"] != undefined) {
        //    objData["FileType"] = Patient_MessageCreate.FilesContainer["Files"][0].type;
        //    objData["FilePath"] = Patient_MessageCreate.FilesContainer["Files"][0].name;
        //}


        //if (Patient_MessageCreate.FilesContainer["Files"] != undefined) {
        //    $.each(document.getElementById("Upload_Import_file").files, function (i, value) {
        //        objData["FileType"] += ", " + Patient_MessageCreate.FilesContainer["Files"][i].type;
        //        objData["FilePath"] += ", " + Patient_MessageCreate.FilesContainer["Files"][i].name;
        //    });
        //}
        //var isPat = '0';
        //if ($('#' + Patient_MessageCreate.params["PanelID"] + ' #isPatSelected').val() == "0") {
        //    isPat = '0';
        //} else if ($('#' + Patient_MessageCreate.params["PanelID"] + ' #isPatSelected').val() == "1") {
        //    isPat = '1';
        //}

        //objData["IsPatientMessage"] = isPat;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");

    },
    FIllReplyThread: function () {

        AppPrivileges.GetFormPrivileges("Messages", "Add", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_MessageCreate.FIllReplyThread_DBCall().done(function (response) {

                    //  $("#divfilesicons").remove();
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #btnReply,#btnForward,#btnViewTask").css("display", "none");
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #btnSend").css("display", "inline");
                    Patient_MessageCreate.params.mode = 'Add';
                    var subjectRep = $("#" + Patient_MessageCreate.params["PanelID"] + " #txtSubject").val();
                    //$("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageTo").html('To');
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #titleMessage").html('Reply');
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #txtSubject").val('Re: ' + subjectRep);
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageToProv").text("To Practice");
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageToPat").text("To Patient");
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #divDownloadLink").css("display", "none");
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #divAttatchFile").css("display", "inline");
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #ddlPriority").prop("disabled", false);
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #txtTo").prop("disabled", true);
                    $("#" + Patient_MessageCreate.params["PanelID"] + " #txtMessageDtl").val('');
                    if (!Patitname) {
                        $("#" + Patient_MessageCreate.params["PanelID"] + " #divAttachPatientwithPractice,#lblMessageToPat,#Patientinputtext").show();
                    }

                    if ($('#' + Patient_MessageCreate.params["PanelID"] + ' #isPatSelected').val == "0") {
                        $("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageToProv").text("To Practice");
                    } else if ($('#' + Patient_MessageCreate.params["PanelID"] + ' #isPatSelected').val == "1") {
                        $("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageToPat").text("To");
                    }
                    if (response.status != false) {
                        var PatientChatInfo = JSON.parse(response.ChatThreadLoad_JSON);
                        var popover = ""
                        $.each(PatientChatInfo, function (i, value) {
                            popover += '\n\n----------------------------------\nFrom: ' + PatientChatInfo[i].AssignedFromFullName +
                            '\nTo: ' + PatientChatInfo[i].AssignedToFullName +

                        '\nDate: ' + PatientChatInfo[i].Date +
                        '\nSubject: ' + PatientChatInfo[i].Subject +
                        '\n\n' + PatientChatInfo[i].MessageSent;

                        });
                        $("#" + Patient_MessageCreate.params["PanelID"] + " #txtMessageDtl").val(popover);
                        $("#" + Patient_MessageCreate.params["PanelID"] + " #txtMessageDtl").focus();
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    FIllReplyThread_DBCall: function () {


        var objData = new Object();
        objData["UserMesgId"] = Patient_MessageCreate.params.UserMessageId;
        objData["CommandType"] = "fill_chat_thread";
        objData["MessageType"] = Patient_MessageCreate.params.MessageType;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");

    },
    ReplyMessage: function () {
        //tinyMCE.activeEditor.setContent('');
        Patient_MessageCreate.FIllReplyThread();


    },

    ForwardMessage: function () {

        $("#" + Patient_MessageCreate.params["PanelID"] + " #btnReply,#btnForward,#btnTask").css("display", "none");
        $("#" + Patient_MessageCreate.params["PanelID"] + " #btnSend").css("display", "inline");
        Patient_MessageCreate.params.mode = 'Add';
        var subjectFwd = $("#" + Patient_MessageCreate.params["PanelID"] + " #txtSubject").val();
        $("#" + Patient_MessageCreate.params["PanelID"] + " #titleMessage").html('Forward');
        $("#" + Patient_MessageCreate.params["PanelID"] + " #txtSubject").val('Fw: ' + subjectFwd);
        $("#" + Patient_MessageCreate.params["PanelID"] + " #divDownloadLink").css("display", "none");
        $("#" + Patient_MessageCreate.params["PanelID"] + " #divAttatchFile").css("display", "inline");
        var message = $("#" + Patient_MessageCreate.params["PanelID"] + " #txtMessageDtl").val();
        $("#" + Patient_MessageCreate.params["PanelID"] + " #txtMessageDtl").val('\n\n\n\t' + "----Forward Message----" + '\n\n' + '"' + message + '"');
        $("#" + Patient_MessageCreate.params["PanelID"] + " #ddlPriority").prop("disabled", false);
        $("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageToProv").html('To Practice');
        $("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageToPat").text("To Patient");
        //$("#" + Patient_MessageCreate.params["PanelID"] + " #lblMessageToPat").html('To');
        UniqueNumber = Patient_MessageCreate.GenerateUUID();
        if (!Patitname) {
            $("#" + Patient_MessageCreate.params["PanelID"] + " #divAttachPatientwithPractice,#lblMessageToPat,#Patientinputtext").show();
        }
        $("#" + Patient_MessageCreate.params["PanelID"] + " #rdPatient").prop("disabled", false);
        //$("#" + Patient_MessageCreate.params["PanelID"] + " #rdProvider").prop("disabled", false);
        $("#" + Patient_MessageCreate.params["PanelID"] + " .btnWidgetSwitch").removeClass('disableAll')
        $("#" + Patient_MessageCreate.params["PanelID"] + " #txtAttatchPatient").prop("disabled", false);
        $("#" + Patient_MessageCreate.params["PanelID"] + " #txtTo").prop("disabled", false);
        $("#" + Patient_MessageCreate.params["PanelID"] + " #lnkPatientAccount").prop("disabled", false);

    },

    UnLoad: function () {

        utility.UnLoadDialog('frmPatientMessageCreate', function () {
            if (Patient_MessageCreate.params != null && Patient_MessageCreate.params.ParentCtrl != null) {
                UnloadActionPan(Patient_MessageCreate.params.ParentCtrl, 'pnlPatientMessageCreate');
            }
            else
                UnloadActionPan(null, 'pnlPatientMessageCreate');
        }, function () {
            if (Patient_MessageCreate.params != null && Patient_MessageCreate.params.ParentCtrl != null) {
                UnloadActionPan(Patient_MessageCreate.params.ParentCtrl, 'pnlPatientMessageCreate');
            }
            else
                UnloadActionPan(null, 'pnlPatientMessageCreate');
        });

        if ((Patient_MessageCreate.params.ParentCtrl == "mstrTabDashBoard" && Patient_MessageCreate.params.Caller != "Referrals") || Patient_MessageCreate.params.ParentCtrl == "patTabUserMessages" || Patient_MessageCreate.params.ParentCtrl == "Patient_UserMessagesQuickLink") {
            if (Patient_MessageCreate.params.MessageType == "Patient") {
                if (Patient_MessageCreate.params.FromPatModule == "1") {
                    Patient_UserMessages.SearchPatientMessage();
                } else {
                    if (Patient_MessageCreate.params.FromQuicklink == "1") {
                        Patient_UserMessagesQuickLink.SearchPracticeMessage();
                    }
                    DashBoard.DashBoardPatientMessagesSearch();
                }

            } else {
                if (Patient_MessageCreate.params.FromPatModule == "1") {
                    Patient_UserMessages.SearchPracticeMessage();
                } else {
                    if (Patient_MessageCreate.params.FromQuicklink == "1") {
                        Patient_UserMessagesQuickLink.SearchPracticeMessage();
                    }
                    DashBoard.DashBoardMessagesSearch();
                }

            }
        }

    },

    InitTinymceControl: function (Isreadonly) {
        if (typeof tinymce.activeEditor != 'undefined') {
            tinymce.EditorManager.execCommand('mceRemoveEditor', true, "txtMessageDtl");
        }
        tinymce.init({
            selector: "textarea#txtMessageDtl",
            theme: "modern",
            readonly: Isreadonly,
            height: 400,
            plugins: [
                "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
                "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                "table contextmenu directionality emoticons template textcolor paste fullpage textcolor colorpicker textpattern"
            ],
            add_unload_trigger: false,
            paste_data_images: true, //enable drag drop image pasting
            toolbar1: "undo redo | styleselect  | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons table",
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
            contextmenu: "link image inserttable | cell row column deletetable | CreateNewDataField InsertDataField"
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
    },

    BufferFile: function (obj) {
        event.stopPropagation();
        var toReturn = true,
            nameHtml;

        if (!(Patient_MessageCreate.FilesContainer.Files.length == 5) && !(Patient_MessageCreate.FilesContainer.Files.length + obj.files.length > 5) && obj.files && obj.files.length != 0 && obj.files.length <= 5) {
            if (Patient_MessageCreate.ValidateUploadedFiles()) {
                for (var i = 0; i < obj.files.length; i++) {
                    Patient_MessageCreate.FilesContainer.Files.push(obj.files[i]);
                    nameHtml = "'" + obj.files[i].name + "'";
                    var fileType = obj.files[i].type;
                    if (Patient_MessageCreate.params.MessageType == "Patient") {
                        if (fileType != "application/xml" && fileType != "text/xml") {
                            $("#divfilesicons").append('<div class="col-sm-4 mt-xs pl-none"><span class="btn btn-success btn-xs btn-and-anchor size100per"><i class="fa fa-file pull-left mt-tiny"></i>' + "<span class='size-max90per ellipses pull-left'> " + obj.files[i].name + '</span><a id="' + i + '"href="#"  onclick="Patient_MessageCreate.DeleteFile(this);"><i class="fa fa-times"></i> </a></span></div>');
                        }
                        else {
                            $("#divfilesicons").append('<div class="col-sm-4 mt-xs pl-none"><span class="btn btn-success btn-xs btn-and-anchor size100per"><i class="fa fa-file pull-left mt-tiny"></i>' + "<span class='size-max90per ellipses pull-left'> " + obj.files[i].name + '</span><a id="' + i + '"href="#"  onclick="Patient_MessageCreate.DeleteFile(this);"><i class="fa fa-times"></i> </a></span></div><div class="pull-left"><a id="xml_' + obj.files[i].name.replace(/[\W_]/g, "") + '" class="btn btn-link btn xs" href="#" onclick="Patient_MessageCreate.ViewHtmlForXmlFile(' + nameHtml + ')">View Html</a></div>');
                        }
                    }
                    else {
                        $("#divfilesicons").append('<div class="col-sm-4 mt-xs pl-none"><span class="btn btn-success btn-xs btn-and-anchor size100per"><i class="fa fa-file pull-left mt-tiny"></i>' + "<span class='size-max90per ellipses pull-left'> " + obj.files[i].name + '</span><a id="' + i + '"href="#"  onclick="Patient_MessageCreate.DeleteFile(this);"><i class="fa fa-times"></i> </a></span></div>');
                    }



                }
            }
        }
        else {
            utility.DisplayMessages("Cannot attach more than 5 files.", 3)
            //Patient_MessageCreate.FilesContainer.Files = [];
            //Patient_MessageCreate.TruncateFileControl();
            toReturn = false;
        }
        return toReturn;

    },

    DeleteFile: function (obj) {
        var selectedFile = $(obj).parent().text();
        var filteredArray = Patient_MessageCreate.FilesContainer.Files.filter(function (a) { return a.name != $(obj).parent().text().trim() });
        Patient_MessageCreate.FilesContainer.Files = filteredArray;
        $(obj).parent().parent().remove();
        var relatedXml = "#pnlPatientMessageCreate #xml_" + selectedFile.replace(/[\W_]/g, "");
        $(relatedXml).remove();
        $('input[type="file"]').val(null);
    },
    GenerateUUID: function () {
        var d = new Date().getTime();
        if (window.performance && typeof window.performance.now === "function") {
            d += performance.now(); //use high-precision timer if available
        }
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    },
    ValidateUploadedFiles: function () {
        var fileName = "";
        var size = 0;
        var files = $('#Upload_Import_file').get(0).files;
        $.each(Patient_MessageCreate.FilesContainer.Files, function (index, file) {
            size = size + Number((Patient_MessageCreate.FilesContainer.Files[index].size / 1024 * 1024).toFixed(2));

        });
        for (var i = 0 ; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType != "application/xml" && fileType != "text/xml" && fileType != "application/pdf" && fileType != "image/jpeg" && fileType != "image/png" && fileType != "image/jpg" && fileType != "image/gif" && fileType != "image/bmp") {
                utility.DisplayMessages("File Type is Invalid", 4);
                Document_Import.TruncateFileControl();
                return false;
            }
            if (Document_Import.ValidateFileSize(files) > Number(globalAppdata['FileSize']) || size > (1048576 * Number(globalAppdata['FileSize']))) {
                utility.DisplayMessages("Maximum " + Number(globalAppdata['FileSize']) + "MB  is allowed", 4);
                Document_Import.TruncateFileControl();
                return false;
            }

            if (Patient_MessageCreate.isFileAlreadyAttached(files[i].name)) {
                utility.DisplayMessages("File already attached with same name!", 4);
                Document_Import.TruncateFileControl();
                return false;
            }
            if (files[i].name.length > 256) {
                utility.DisplayMessages("File Name should have maximun 256 Characters", 4);
                Document_Import.TruncateFileControl();
                return false;
            }
            fileName = fileName + files[i].name + ',';
        }
        fileName = fileName.slice(0, fileName.length - 1);
        document.getElementById("uploadFilePH").value = fileName;
        $('#totalFiles').text(files.length + " file(s) selected");
        return true;
    },

    isFileAlreadyAttached: function (fileName) {
        for (var i = 0; i < Patient_MessageCreate.FilesContainer.Files.length; i++) {
            if (Patient_MessageCreate.FilesContainer.Files[i].name == fileName)
                return true;
        }
        return false;
    },

    TruncateFileControl: function () {
        $("#" + Patient_MessageCreate.params.PanelID + " #uploadFilePH").val('');
        $('#' + Patient_MessageCreate.params.PanelID + ' #totalFiles').text("0 file(s) selected");
        $('#' + Patient_MessageCreate.params.PanelID + ' #Upload_Import_file').val('');
    },

    PrintMessage: function () {
        AppPrivileges.GetFormPrivilegesByModule("Messages", "PRINT", "Dash Board", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE_BY_MODULE", function (strMessage) {
            if (strMessage == "") {
                var printtemplate = "";
                if (Patient_MessageCreate.params.MessageType == "Practice") {
                    printtemplate = '<div><h2 style="margin:5px;">' + globalAppdata.AppUserNameFullName + '</h2><hr style="border:2px solid #468cec;"/>'
                + '<div style="width:25%;float:left"><strong>From:</strong></div>'
                + '<div style="width:75%;float:left">' + globalAppdata.AppUserNameFullName + '</div>'

                + '<br><div style="width:25%;float:left"><strong>Date:</strong></div>'
                + '<div style="width:75%;float:left">' + printdate + '</div>'

                + '<br><div style="width:25%;float:left"><strong>To:</strong></div>'
                + '<div style="width:75%;float:left">' + $('#' + Patient_MessageCreate.params["PanelID"] + ' #txtTo').val() + '</div>'

                + '<br><div style="width:25%;float:left"><strong>Attached Patient:</strong></div>'
                + '<div style="width:75%;float:left">' + Patitname + '</div>'

                + '<br><div style="width:25%;float:left"><strong>Priority:</strong></div>'
                + '<div style="width:75%;float:left">' + printpriority + '</div>'

                + '<br><div style="width:25%;float:left"><strong>Subject:</strong></div>'
                + '<div style="width:75%;float:left">' + printsubject + '</div>'

                + '<br><hr style="border:2px solid #468cec;"/>'
                 + $('#' + Patient_MessageCreate.params["PanelID"] + ' #txtMessageDtl').text() + '</div>'

                } else if (Patient_MessageCreate.params.MessageType == "Patient") {
                    printtemplate = '<div><h2>' + globalAppdata.AppUserNameFullName +
          '</h2><hr style="border:2px solid #468cec;"/><strong>From:</strong>' + "               " + globalAppdata.AppUserNameFullName +
          '<br><strong>Date:</strong>' + "               " + printdate +
          '<br><strong>To:</strong>' + "               " + Patitname +
          '<br><strong>Attached Patient:</strong>' + "               " +
          '<br><strong>Priority:</strong>' + "               " + printpriority +
          '<br><strong>Subject:</strong>' + "               " + printsubject +
          '<br><hr style="border:2px solid #468cec;"/>'
           + $('#' + Patient_MessageCreate.params["PanelID"] + ' #txtMessageDtl').text() + '</div>'
                }

                $(printtemplate).printMe();
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },

    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_MessageCreate';
        LoadActionPan('Patient_Search', params);
    },
}