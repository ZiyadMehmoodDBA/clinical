Patient_MessageCompose = {
    bIsFirstLoad: true,
    params: [],
    TemplateContent: "",
    FilesContainer: { Files: undefined, Name: "Uploaded_Document" },
    xmlByteData: null,
    ImportFileData:'',
    ImportFileType: '',
    ImportFileName: '',
    Load: function (params) {
        Patient_MessageCompose.params = params;
        Patient_MessageCompose.FilesContainer = { Files: undefined, Name: "Uploaded_Document" };
        if (Patient_MessageCompose.params["PanelID"] != 'pnlPatientMessageCompose') {
            Patient_MessageCompose.params["PanelID"] = Patient_MessageCompose.params["PanelID"] + ' #pnlPatientMessageCompose';
        }
        if (Patient_MessageCompose.bIsFirstLoad) {
            Patient_MessageCompose.bIsFirstLoad = false;
            var self = $('#' + Patient_MessageCompose.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
                Patient_MessageCompose.LoadAllAutocomplete();
                $('#frmPatientMessageCompose').data('serialize', $('#frmPatientMessageCompose').serialize());
            });

        }

        //Patient_MessageCompose.InitTinymceControl(false);
        Patient_MessageCompose.ValidateMessageCompose();
        Patient_MessageCompose.Documentready();
        if (Patient_MessageCompose.params.mode == 'Add') {
            $("#" + Patient_MessageCompose.params["PanelID"] + " #btnReply,#btnForward,#btnTask").css("display", "none");
            $("#" + Patient_MessageCompose.params["PanelID"] + " #btnSend").css("display", "inline");
            $("#" + Patient_MessageCompose.params["PanelID"] + " #divDownloadLink").css("display", "none");
            $("#" + Patient_MessageCompose.params["PanelID"] + " #divDownloadLinkHTML").css("display", "none");
            $("#" + Patient_MessageCompose.params["PanelID"] + " #divPrintMessage").css("display", "none");
            //adnan maqbool, EMR-910
            $("#" + Patient_MessageCompose.params["PanelID"] + " #btnSend").prop("disabled", true);

            //Start 24-08-2016 Humaira Yousaf for referral message
            if (Patient_MessageCompose.params.ParentCtrl == "mstrTabDashBoard") {
                $("#" + Patient_MessageCompose.params["PanelID"] + " #hftxtTo").val(Patient_MessageCompose.params.AssignedToId);
                $("#" + Patient_MessageCompose.params["PanelID"] + " #txtTo").val(Patient_MessageCompose.params.AssignedName);
                $("#" + Patient_MessageCompose.params["PanelID"] + " #hfPatientId").val(Patient_MessageCompose.params.PatientId);
                $("#" + Patient_MessageCompose.params["PanelID"] + " #txtMessageDtl").html(Patient_MessageCompose.params.Message);
                $("#" + Patient_MessageCompose.params["PanelID"] + " #txtSubject").val(Patient_MessageCompose.params.MsgSubject);
                $("#" + Patient_MessageCompose.params["PanelID"] + " #btnSend").prop("disabled", false);

                $("#" + Patient_MessageCompose.params["PanelID"] + " #divSwitch").addClass('hidden');
            }
            else {
                $("#" + Patient_MessageCompose.params["PanelID"] + " #divSwitch").removeClass('hidden');
            }
            //End 24-08-2016 Humaira Yousaf for referral message
        } else if (Patient_MessageCompose.params.mode == 'Edit') {
            $("#" + Patient_MessageCompose.params["PanelID"] + " #btnReply,#btnForward,#btnTask").css("display", "inline");
            $("#" + Patient_MessageCompose.params["PanelID"] + " #btnSend").css("display", "none");
            $("#" + Patient_MessageCompose.params["PanelID"] + " #lblMessageTo").html('From');
            $("#" + Patient_MessageCompose.params["PanelID"] + " #divAttatchFile").css("display", "none");
            $("#" + Patient_MessageCompose.params["PanelID"] + " #divPrintMessage").css("display", "inline");
            if (Patient_MessageCompose.params.UserMessageId != null) {
                Patient_MessageCompose.FillUserMessage();
            }
        }


        if (Patient_MessageCompose.params.ParentCtrl == "Patient_Message") {
            $("#" + Patient_MessageCompose.params["PanelID"] + " #divPatientSearch").css("display", "none");
            $("#" + Patient_MessageCompose.params["PanelID"] + " #hfPatientId").val($("#PatientProfile #hfPatientId").val());
        } else {
            $("#" + Patient_MessageCompose.params["PanelID"] + " #divPatientSearch").css("display", "inline");
        }
        Patient_MessageCompose.ImportFileData = "";
        Patient_MessageCompose.ImportFileType = "";
        Patient_MessageCompose.ImportFileName = "";
        //Patient_MessageCompose.Documentready();
    },

    OpenImportDocument: function (ThisCTRL) {
        var strMessage = "";

        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["RefCtrl"] = "Patient_MessageCompose";
                params["FromAdmin"] = "0";
                params["mode"] = "ImportDoc";
                Patient_MessageCompose.ImportFileData = $(ThisCTRL).attr("data-base64str");
                Patient_MessageCompose.ImportFileType = $(ThisCTRL).attr("data-filetypestr");
                Patient_MessageCompose.ImportFileName = $(ThisCTRL).attr("data-fileNamestr");
                params["ParentCtrl"] = 'Patient_MessageCompose';
                //params["BatchID"] = $('#pnlChargeBatchDetail #hfBatchID').val();
                LoadActionPan('Document_Import', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    SetImportCCDA: function (ThisCTRL, event) {
        if (event) {
            event.preventDefault();
        }
        Patient_MessageCompose.xmlByteData = $(ThisCTRL).attr("data-base64str");
        UnloadActionPan(null, 'pnlPatientMessageCompose');
        SelectTab('mstrTabBatch', 'false');
        if (!$("#batchTabImportCCDA").parent().hasClass('tab_selected')) {
            SelectTab('batchTabImportCCDA', 'true');
        }

    },

    LoadAllAutocomplete: function () {
        CacheManager.BindCodes('GetUsers', true, 1).done(function (result) {
            $("#" + Patient_MessageCompose.params["PanelID"] + " #txtTo").autocomplete({
                autoFocus: true,
                source: Users, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Patient_MessageCompose.params["PanelID"] + " #hftxtTo").val(ui.item.id); // add the selected id
                        //adnan maqbool, EMR-910
                        $("#" + Patient_MessageCompose.params["PanelID"] + " #btnSend").prop("disabled", false);
                    }, 100);
                }
            });
        });

    },
    Documentready: function (obj) {
        (function ($) {
            'use strict';
            $(function () {
                $('#pnlPatientMessageCompose [data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);

        $('#' + Patient_MessageCompose.params["PanelID"] + ' #divPatient').hide();
        $('#' + Patient_MessageCompose.params["PanelID"] + ' #divphiMailAddress').hide();
        $('#' + Patient_MessageCompose.params["PanelID"] + ' #isPatSelected').val("0");
    },
    ChangeStatus: function (obj) {
        if ($(obj).attr('status') == '1') {

            $(obj).attr('status', 0);
            $('#frmPatientMessageCompose .btnWidgetSwitch').find('div [class="state-background background-fill"]').css({ "background": "#47a447", "border-color": "#47a447" });
            $('#' + Patient_MessageCompose.params["PanelID"] + ' #divPatient').show();
            $('#' + Patient_MessageCompose.params["PanelID"] + ' #divProvider').hide();
            $('#' + Patient_MessageCompose.params["PanelID"] + ' #isPatSelected').val("1");
            if ($('#frmPatientMessageCompose').data('bootstrapValidator') != null && typeof $('#frmproviderscheduleDetail').data('bootstrapValidator') != 'undefined') {
                $('#frmPatientMessageCompose').data('bootstrapValidator').enableFieldValidators('AttatchPatient', true);
                $('#frmPatientMessageCompose').data('bootstrapValidator').enableFieldValidators('MessageTo', false);
            }
        } else {
            $(obj).attr('status', 1);
            $('#' + Patient_MessageCompose.params["PanelID"] + ' #divPatient').hide();
            $('#' + Patient_MessageCompose.params["PanelID"] + ' #divProvider').show();
            $('#' + Patient_MessageCompose.params["PanelID"] + ' #isPatSelected').val("0");

            if ($('#frmPatientMessageCompose').data('bootstrapValidator') != null && typeof $('#frmproviderscheduleDetail').data('bootstrapValidator') != 'undefined') {
                $('#frmPatientMessageCompose').data('bootstrapValidator').enableFieldValidators('AttatchPatient', true);
                $('#frmPatientMessageCompose').data('bootstrapValidator').enableFieldValidators('MessageTo', false);
            }

        }
    },

    BindPatientAccount: function () {
        var AccountNo = $('#' + Patient_MessageCompose.params["PanelID"] + ' #txtAttatchPatient').val();
        utility.Keyupdelay(function () {
            var AllPatients = utility.GetPatientArray(AccountNo, 1).done(function (response) {
                $('#' + Patient_MessageCompose.params["PanelID"] + ' #txtAttatchPatient').autocomplete({
                    autoFocus: true,
                    source: response,
                    open: function (event, ui) { disable = true },
                    close: function (event, ui) {
                        disable = false; $(this).focus();
                    },
                    select: function (event, ui) {
                        setTimeout(function () {
                            $('#' + Patient_MessageCompose.params["PanelID"] + ' #txtAttatchPatient').val(ui.item.value);
                            $('#' + Patient_MessageCompose.params["PanelID"] + ' #hfPatientId').val(ui.item.id);
                            if ($('#frmPatientMessageCompose').data('bootstrapValidator') != null && typeof $('#frmPatientMessageCompose').data('bootstrapValidator') != 'undefined') {
                                $('#frmPatientMessageCompose').bootstrapValidator('revalidateField', 'AttatchPatient');
                            }
                        }, 100);
                    }
                }).blur(function () {
                    setTimeout(function () {
                        utility.ValidateAutoComplete($('#' + Patient_MessageCompose.params["PanelID"] + ' #txtAttatchPatient'), "frmPatientMessageCompose #hfPatientId", false, null, null, null);
                    }, 200);
                });


                $('#' + Patient_MessageCompose.params["PanelID"] + ' #txtAttatchPatient').autocomplete("search");
                $('#' + Patient_MessageCompose.params["PanelID"] + ' #txtAttatchPatient').focus();
            });
        });

    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_MessageCompose';
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNumber, FullName, event) {
        if (event != null) {
            event.stopPropagation();
            if (event.target.type == "checkbox") {
                $(':checkbox', this).trigger('click');
                return;
            }
        }
        $('#' + Patient_MessageCompose.params["PanelID"] + ' #txtAttatchPatient').val(AccountNumber.trim() + " - " + FullName.trim());
        $('#' + Patient_MessageCompose.params["PanelID"] + ' #hfPatientId').val(PatientId);


        UnloadActionPan("Patient_MessageCompose");
        if ($('#frmPatientMessageCompose').data('bootstrapValidator') != null && typeof $('#frmPatientMessageCompose').data('bootstrapValidator') != 'undefined') {
            $('#frmPatientMessageCompose').bootstrapValidator('revalidateField', 'AttatchPatient');
        }
    },

    FillUserMessage: function () {
        AppPrivileges.GetFormPrivileges("Messages", "Add", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_MessageCompose.FillUserMessage_DBCall().done(function (response) {
                    if (response.status != false) {
                        var messageDetal = JSON.parse(response.userMessagesFill_JSON);
                        var self = $("#" + Patient_MessageCompose.params["PanelID"]);

                        utility.bindMyJSONByName(true, messageDetal, false, self).done(function () {
                            if (messageDetal.AttatchedPatientId == "") {
                                $("#" + Patient_MessageCompose.params["PanelID"] + " #hfPatientId").val('');
                                $("#" + Patient_MessageCompose.params["PanelID"] + " #divPatientSearch").css('display', 'none');
                            } else {

                            }
                            if (messageDetal.Documents != "") {
                                var docs = JSON.parse(messageDetal.Documents)

                                $.each(docs, function (index, element) {
                                    if (element[1].toLowerCase().indexOf("xml") > -1) {
                                        $("#divfilesicons").append('<div class=" col-sm-4"><a class="btn btn-success btn-xs" id="linkDownload"   href="#" data-base64str=' + element[0] + ' title="Download File" onclick="Patient_MessageCompose.SetImportCCDA(this,event);"><i class="fa fa-download"></i>' + element[2] + '</a> </div>');
                                    } else {
                                        $("#divfilesicons").append('<div class=" col-sm-4"><a class="btn btn-success btn-xs" id="linkDownload" download=' + element[2] + ' href="data:' + element[1] + ';base64,' + element[0] + '"title="Download File"><i class="fa fa-download"></i>' + element[2] + '</a> <i class="glyphicon glyphicon-import btn-xs btn-success" onclick="Patient_MessageCompose.OpenImportDocument(this);" data-base64str=' + element[0] + ' data-filetypestr=' + element[1] + ' data-fileNamestr=' + element[2] + '></i></div>');
                                    }
                                });

                            } else {
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #divDownloadLink").css("display", "none");
                                $("#" + Patient_MessageCreate.params["PanelID"] + " #divDownloadLinkHTML").css("display", "none");

                            }

                            $("#" + Patient_MessageCompose.params["PanelID"] + " #txtTo").val('');
                            $("#" + Patient_MessageCompose.params["PanelID"] + " #divProvider").hide();
                            $("#" + Patient_MessageCompose.params["PanelID"] + " #divPatient").hide();
                            $("#" + Patient_MessageCompose.params["PanelID"] + " #divphiMailAddress").show();
                            $("#" + Patient_MessageCompose.params["PanelID"] + " #phiMAilAddress").prop("disabled", true);
                            $("#frmPatientMessageCompose #divSwitch").hide();
                            if (Patient_MessageCompose.params.MessageType == "DIRECT") {
                                $('#frmPatientMessageCompose #divButtons').hide();
                                $('#frmPatientMessageCompose #divAttachPatientwithPractice').hide();
                                $('#frmPatientMessageCompose #ddlPriority').parent().hide();

                            }
                        });

                    } else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    FillUserMessage_DBCall: function () {

        var objData = new Object();
        objData["UserMesgId"] = Patient_MessageCompose.params.UserMessageId;
        objData["CommandType"] = "fill_direct_message";
        objData[""] = "";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");

    },

    UserMessageCount: function () {

        var objData = new Object();
        objData["CommandType"] = "get_messages_count";
        //objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");

    },

    AddTask: function () {
        var params = [];
        params["PatientId"] = $('#' + Patient_MessageCompose.params["PanelID"] + ' #hfPatientId').val();
        params["PatientName"] = $('#' + Patient_MessageCompose.params["PanelID"] + ' #txtAttatchPatient').val();
        params["UserMessageId"] = Patient_MessageCompose.params.UserMessageId;
        params["mode"] = 'Add';
        params["ParentCtrl"] = "Patient_MessageCompose";
        params["AssignedToId"] = null;
        params["MessageDetail"] = $('#' + Patient_MessageCompose.params["PanelID"] + ' #txtMessageDtl').val();
        LoadActionPan('Patient_MessageAdd', params);
    },

    ValidateMessageCompose: function () {
        $('#frmPatientMessageCompose')
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
                   AttatchPatient: {
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
            Patient_MessageCompose.SaveComposeMessage();
        });
    },

    SaveComposeMessage: function () {

        //var data = new FormData();

        //$.each(Patient_MessageCompose.FilesContainer.Files, function (key, value) {
        //    data.append(key, value);
        //});
        //Patient_MessageCompose.FilesContainer = { Files: undefined, Name: "Uploaded_Document" };
        var dfd = new $.Deferred();
        var files = [];
        var count = 0;


        if (document.getElementById("Upload_Import_file").files.length > 0) {
            $.each(document.getElementById("Upload_Import_file").files, function (key, value) {
                var oFReader = new FileReader();
                oFReader.readAsDataURL(document.getElementById("Upload_Import_file").files[0]);
                oFReader.onload = function (oFREvent) {
                    //data:image/jpeg;base64,
                    var file_ = oFREvent.target.result.split('base64,');
                    files.push(key, file_[1]);
                    count++;
                    //if (document.getElementById("Upload_Import_file").files.length == count)
                    dfd.resolve('ok');
                    //else
                    //  dfd.resolve('ok');
                };

            });
        } else if (document.getElementById("Upload_Import_file").files.length == 0) {
            dfd.resolve('ok');
        }
        dfd.then(function () {

            var strMessage = "";
            var self = $('#' + Patient_MessageCompose.params["PanelID"]);
            var myJSON = self.getMyJSONByName();

            //var objData = JSON.parse(myJSON);



            //AppPrivileges.GetFormPrivileges("Message Reply", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {

            if (Patient_MessageCompose.params.mode = "Add") {
                Patient_MessageCompose.SaveComposeMessage_DBCall(myJSON, files).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages("Successfully sent!", 1);
                        if (Patient_MessageCompose.params.ParentCtrl == "mstrTabDashBoard" && Patient_MessageCompose.params.Caller != "Referrals") {
                            DashBoard.DashBoardMessagesSearch();
                        }
                        UnloadActionPan(Patient_MessageCompose.params.ParentCtrl, 'pnlPatientMessageCompose');

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

    SaveComposeMessage_DBCall: function (messagesData, data) {
        var objData = JSON.parse(messagesData);
        objData["MessageDtl123"] = $("#" + Patient_MessageCompose.params.PanelID + " #txtMessageDtl").val();
        objData["CommandType"] = "save_message";
        objData["Files"] = data;
        if (Patient_MessageCompose.FilesContainer["Files"] != undefined) {
            objData["FileType"] = Patient_MessageCompose.FilesContainer["Files"][0].type;
            objData["FilePath"] = Patient_MessageCompose.FilesContainer["Files"][0].name;
        }
        var isPat = '0';
        if ($('#' + Patient_MessageCompose.params["PanelID"] + ' #isPatSelected').val() == "0") {
            isPat = '0';
        } else if ($('#' + Patient_MessageCompose.params["PanelID"] + ' #isPatSelected').val() == "1") {
            isPat = '1';
        }

        objData["IsPatientMessage"] = isPat;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");

    },


    ReplyMessage: function () {
        //tinyMCE.activeEditor.setContent('');
        $("#" + Patient_MessageCompose.params["PanelID"] + " #btnReply,#btnForward,#btnTask").css("display", "none");
        $("#" + Patient_MessageCompose.params["PanelID"] + " #btnSend").css("display", "inline");
        Patient_MessageCompose.params.mode = 'Add';
        var subjectRep = $("#" + Patient_MessageCompose.params["PanelID"] + " #txtSubject").val();
        //$("#" + Patient_MessageCompose.params["PanelID"] + " #lblMessageTo").html('To');
        $("#" + Patient_MessageCompose.params["PanelID"] + " #titleMessage").html('Reply');
        $("#" + Patient_MessageCompose.params["PanelID"] + " #txtSubject").val('Re: ' + subjectRep);

        $("#" + Patient_MessageCompose.params["PanelID"] + " #divDownloadLink").css("display", "none");
        $("#" + Patient_MessageCompose.params["PanelID"] + " #divAttatchFile").css("display", "inline");

        $("#" + Patient_MessageCompose.params["PanelID"] + " #txtTo").prop("disabled", true);
        $("#" + Patient_MessageCompose.params["PanelID"] + " #txtMessageDtl").val('');


        if ($('#' + Patient_MessageCompose.params["PanelID"] + ' #isPatSelected').val == "0") {
            $("#" + Patient_MessageCompose.params["PanelID"] + " #lblMessageToProv").text("To");
        } else if ($('#' + Patient_MessageCompose.params["PanelID"] + ' #isPatSelected').val == "1") {
            $("#" + Patient_MessageCompose.params["PanelID"] + " #lblMessageToPat").text("To");
        }

    },

    ForwardMessage: function () {

        $("#" + Patient_MessageCompose.params["PanelID"] + " #btnReply,#btnForward,#btnTask").css("display", "none");
        $("#" + Patient_MessageCompose.params["PanelID"] + " #btnSend").css("display", "inline");
        Patient_MessageCompose.params.mode = 'Add';
        var subjectFwd = $("#" + Patient_MessageCompose.params["PanelID"] + " #txtSubject").val();
        $("#" + Patient_MessageCompose.params["PanelID"] + " #titleMessage").html('Forward');
        $("#" + Patient_MessageCompose.params["PanelID"] + " #txtSubject").val('Fw: ' + subjectFwd);
        $("#" + Patient_MessageCompose.params["PanelID"] + " #divDownloadLink").css("display", "none");
        $("#" + Patient_MessageCompose.params["PanelID"] + " #divAttatchFile").css("display", "inline");
        var message = $("#" + Patient_MessageCompose.params["PanelID"] + " #txtMessageDtl").val();
        $("#" + Patient_MessageCompose.params["PanelID"] + " #txtMessageDtl").val('\n\n\n\t' + "----Forward Message----" + '\n\n' + '"' + message + '"');

        $("#" + Patient_MessageCompose.params["PanelID"] + " #lblMessageToProv").html('To');
        $("#" + Patient_MessageCompose.params["PanelID"] + " #lblMessageToPat").html('To');

        $("#" + Patient_MessageCompose.params["PanelID"] + " #rdPatient").prop("disabled", false);
        //$("#" + Patient_MessageCompose.params["PanelID"] + " #rdProvider").prop("disabled", false);
        $("#" + Patient_MessageCompose.params["PanelID"] + " .btnWidgetSwitch").removeClass('disableAll')
        $("#" + Patient_MessageCompose.params["PanelID"] + " #txtAttatchPatient").prop("disabled", false);
        $("#" + Patient_MessageCompose.params["PanelID"] + " #txtTo").prop("disabled", false);
        $("#" + Patient_MessageCompose.params["PanelID"] + " #lnkPatientAccount").prop("disabled", false);

    },

    UnLoad: function () {

        //utility.UnLoadDialog('frmPatientMessageCompose', function () {
        //    if (Patient_MessageCompose.params != null && Patient_MessageCompose.params.ParentCtrl != null) {
        //        UnloadActionPan(Patient_MessageCompose.params.ParentCtrl, 'pnlPatientMessageCompose');
        //    }
        //    else
        //        UnloadActionPan(null, 'pnlPatientMessageCompose');
        //}, function () {
        //    if (Patient_MessageCompose.params != null && Patient_MessageCompose.params.ParentCtrl != null) {
        //        UnloadActionPan(Patient_MessageCompose.params.ParentCtrl, 'pnlPatientMessageCompose');
        //    }
        //    else
        UnloadActionPan(null, 'pnlPatientMessageCompose');
        if (Patient_MessageCompose.params.MessageType == 'DIRECT') {
            DashBoard.DashBoardDirectMessagesSearch();
        }
        //});

        //if (Patient_MessageCompose.params.ParentCtrl == "mstrTabDashBoard" && Patient_MessageCompose.params.Caller != "Referrals") {
        //    DashBoard.DashBoardMessagesSearch();
        //}

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
        var toReturn = true;

        if (obj.files && obj.files.length != 0) {
            Patient_MessageCompose.ValidateUploadedFiles();
            Patient_MessageCompose.FilesContainer.Files = obj.files;

        }
        else {
            delete Patient_MessageCompose.FilesContainer.Files;
            Patient_MessageCompose.TruncateFileControl();
            toReturn = false;
        }
        return toReturn;

    },

    ValidateUploadedFiles: function () {
        var fileName = "";
        var files = $('#Upload_Import_file').get(0).files;
        for (var i = 0 ; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType != "application/pdf" && fileType != "image/jpeg" && fileType != "image/png" && fileType != "image/jpg" && fileType != "image/gif" && fileType != "image/bmp") {
                utility.DisplayMessages("File Type is Invalid", 4);
                Document_Import.TruncateFileControl();
                return false;
            }
            if (Document_Import.ValidateFileSize(files) > Number(globalAppdata['FileSize'])) {
                utility.DisplayMessages("Maximum " + Number(globalAppdata['FileSize']) + "MB  is allowed", 4);
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

    TruncateFileControl: function () {
        $("#" + Patient_MessageCompose.params.PanelID + " #uploadFilePH").val('');
        $('#' + Patient_MessageCompose.params.PanelID + ' #totalFiles').text("0 file(s) selected");
        $('#' + Patient_MessageCompose.params.PanelID + ' #Upload_Import_file').val('');
    },

    PrintMessage: function () {
        $("#" + Patient_MessageCompose.params["PanelID"] + " #txtMessageDtl").printMe();
    },

    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_MessageCompose';
        LoadActionPan('Patient_Search', params);
    },
}