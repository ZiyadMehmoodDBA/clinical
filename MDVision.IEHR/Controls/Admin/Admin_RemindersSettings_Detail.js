remindersSettingsDetail = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {

        remindersSettingsDetail.params = params;

        if (remindersSettingsDetail.params.PanelID != 'pnlRemindersSettingDetail') {
            remindersSettingsDetail.params.PanelID = remindersSettingsDetail.params.PanelID + ' #pnlRemindersSettingDetail';
        } else {
            remindersSettingsDetail.params.PanelID = 'pnlRemindersSettingDetail';
        }

        var self = $('#' + remindersSettingsDetail.params.PanelID);
        remindersSettingsDetail.ValidateRemindersTemplateSettings();


        $.when(remindersSettingsDetail.LoadingDropDowns(), self.loadDropDowns(false)).then(function () {

            remindersSettingsDetail.IntializeMultiSelectDropDownFacility("Voice", "VoiceSettingsFacility", "facilityVoicediv");
            remindersSettingsDetail.IntializeMultiSelectDropDownFacility("Text", "TextSettingsFacility", "facilityTextdiv");
            remindersSettingsDetail.IntializeMultiSelectDropDownFacility("Email", "EmailSettingsFacility", "facilityEmaildiv");

            if (remindersSettingsDetail.params.mode == "Add") {

                var dfd = new $.Deferred();
                remindersSettingsDetail.fillTemplateDDL(dfd);

                //$('#' + remindersSettingsDetail.params.PanelID + ' #frmRemindersTemplateDetail').data('serialize', $('#' + remindersSettingsDetail.params.PanelID + ' #frmRemindersTemplateDetail').serialize());
            }
            else if (remindersSettingsDetail.params.mode == "Edit") {
                var dfd = new $.Deferred();
                remindersSettingsDetail.fillTemplateDDL(dfd);
                dfd.then(function () {
                    setTimeout(function () { remindersSettingsDetail.LoadRemindersSettings(); }, 1000);

                });

            }
        });
    },
    LoadRemindersSettings: function () {

        if (remindersSettingsDetail.params.mode == "Edit") {
            $('#' + remindersSettingsDetail.params.PanelID + ' #frmRemindersSettingDetail #ddlType').val(remindersSettingsDetail.params.Type);
            $('#' + remindersSettingsDetail.params.PanelID + ' #frmRemindersSettingDetail #ddlType').trigger("change");
            $('#' + remindersSettingsDetail.params.PanelID + ' #frmRemindersSettingDetail #ddlType').prop('disabled', true);
            $('#' + remindersSettingsDetail.params.PanelID + ' #frmRemindersSettingDetail #divDDLReminderType').css('display', 'none');
            if (remindersSettingsDetail.params.RemindersSettingsId != undefined) {
                remindersSettingsDetail.FillRemindersSettings(remindersSettingsDetail.params.RemindersSettingsId).done(function (response) {
                    if (response.status != false) {
                        var RemindersTemplate_detail = JSON.parse(response.RemindersTemplateFill_JSON);
                        //  ProviderIdsmultiselect = remindersSettingsDetail.ProviderIds;
                        var self = $("#pnlRemindersSettingDetail");
                        utility.bindMyJSONByName(true, RemindersTemplate_detail, false, self).done(function () {


                            if (remindersSettingsDetail.params.Type == "text") {
                                $('#' + remindersSettingsDetail.params.PanelID + ' #headerId').text('Text Reminder Settings');
                                if (RemindersTemplate_detail.DisableVoiceReminderFailover == "False") {
                                    $("#rdFailoverFalse").prop("checked", true);
                                    $("#rdFailoverTrue").prop("checked", false);
                                } else {
                                    $("#rdFailoverTrue").prop("checked", true);
                                    $("#rdFailoverFalse").prop("checked", false);
                                }
                                if (RemindersTemplate_detail.TextDelivery && RemindersTemplate_detail.TextDelivery != "" && RemindersTemplate_detail.TextDelivery != 'undefined') {
                                    var DurationType = RemindersTemplate_detail.TextDelivery.split(' ');
                                    $("#frmRemindersSettingDetail #TextDurationType").val(DurationType[0]);
                                    $("#frmRemindersSettingDetail #ddlTextDelivery").val(DurationType[1]);
                                }
                                ///$("#frmRemindersSettingDetail #spnCancelMsg").html((105) - (RemindersTemplate_detail.AppCancellationMessage.length));
                                $("#frmRemindersSettingDetail #spnConfirmationMsg").html((160) - (RemindersTemplate_detail.AppConfirmationMessage.length));
                                $("#frmRemindersSettingDetail #spnTxtMsg").html((160) - (RemindersTemplate_detail.AppReminderMessage.length));

                                $('#' + remindersSettingsDetail.params.PanelID + " #TextSettingsFacility").val(RemindersTemplate_detail.FacilityIds.split(','));
                                $('#' + remindersSettingsDetail.params.PanelID + " #TextSettingsFacility").multiselect("refresh");
                                remindersSettingsDetail.params["TextFacilityIds"] = RemindersTemplate_detail.FacilityIds;

                            } else if (remindersSettingsDetail.params.Type == "voice") {
                                $('#' + remindersSettingsDetail.params.PanelID + ' #headerId').text('Voice Reminder Settings');
                                if (RemindersTemplate_detail.VoiceDelivery && RemindersTemplate_detail.VoiceDelivery != "" && RemindersTemplate_detail.VoiceDelivery != 'undefined') {
                                    var DurationType = RemindersTemplate_detail.VoiceDelivery.split(' ');
                                    $("#frmRemindersSettingDetail #VoiceDurationType").val(DurationType[0]);
                                    $("#frmRemindersSettingDetail #ddlVoiceDelivery").val(DurationType[1]);
                                }
                                if (RemindersTemplate_detail.RepeatMessage == "False") {

                                    $("#IsRepeatMessage").prop("checked", false);
                                } else {
                                    $("#IsRepeatMessage").prop("checked", true);

                                }

                                $('#' + remindersSettingsDetail.params.PanelID + " #VoiceSettingsFacility").val(RemindersTemplate_detail.FacilityIds.split(','));
                                $('#' + remindersSettingsDetail.params.PanelID + " #VoiceSettingsFacility").multiselect("refresh");
                                remindersSettingsDetail.params["VoiceFacilityIds"] = RemindersTemplate_detail.FacilityIds;

                            } else {
                                $('#' + remindersSettingsDetail.params.PanelID + ' #headerId').text('Email Reminder Settings');
                                if (RemindersTemplate_detail.EmailDelivery && RemindersTemplate_detail.EmailDelivery != "" && RemindersTemplate_detail.EmailDelivery != 'undefined') {
                                    var DurationType = RemindersTemplate_detail.EmailDelivery.split(' ');
                                    $("#frmRemindersSettingDetail #EmailDurationType").val(DurationType[0]);
                                    $("#frmRemindersSettingDetail #ddlEmailDelivery").val(DurationType[1]);
                                }

                                $('#' + remindersSettingsDetail.params.PanelID + " #EmailSettingsFacility").val(RemindersTemplate_detail.FacilityIds.split(','));
                                $('#' + remindersSettingsDetail.params.PanelID + " #EmailSettingsFacility").multiselect("refresh");
                                remindersSettingsDetail.params["EmailFacilityIds"] = RemindersTemplate_detail.FacilityIds;
                            }

                            //Serialize data
                            $('#' + remindersSettingsDetail.params.PanelID + ' #frmRemindersSettingDetail').data('serialize', $('#' + remindersSettingsDetail.params.PanelID + ' #frmRemindersSettingDetail').serialize());


                        });

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }

    },


    FillRemindersSettings: function (RemindersSettingsId) {

        //  var type = $('#' + remindersSettingsDetail.params.PanelID + " #ddlType").val();
        if (remindersSettingsDetail.params.Type == "email") {
            var objData = new JSON.constructor();
            objData["commandType"] = "fill_email_settings";
            objData["ReminderEmailSettingId"] = RemindersSettingsId;
            var data = JSON.stringify(objData);
            return MDVisionService.PMSAPIService(data, "Reminders", "Settings");
        }
        else if (remindersSettingsDetail.params.Type == "text") {
            var objData = new JSON.constructor();
            objData["commandType"] = "fill_text_settings";
            objData["ReminderTextSettingId"] = RemindersSettingsId;
            var data = JSON.stringify(objData);
            return MDVisionService.PMSAPIService(data, "Reminders", "Settings");
        } else if (remindersSettingsDetail.params.Type == "voice") {
            var objData = new JSON.constructor();
            objData["commandType"] = "fill_voice_settings";
            objData["ReminderVoiceSettingId"] = RemindersSettingsId;
            var data = JSON.stringify(objData);
            return MDVisionService.PMSAPIService(data, "Reminders", "Settings");
        }
    },

    UnLoadTab: function () {
        UnloadActionPan();
    },
    ValidateRemindersTemplateSettings: function () {

        $('#frmRemindersSettingDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  CallIDNumber: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                          regexp: {
                              regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                              message: 'Email not Valid'
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
                  },
                  TextDurationType: {
                      group: '.col-md-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  VoiceDurationType: {
                      group: '.col-md-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  EmailDurationType: {
                      group: '.col-md-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  deliverreminders: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  TextSettingsProvider: {
                      group: '.col-md-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Provider: {
                      group: '.col-md-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ProviderVoiceSetting: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  //CallIDNumberforVoice: {
                  //    group: '.col-md-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  //SMSCallerName: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  AppReminderMessage: {
                      group: '.col-sm-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  AppConfirmationMessage: {
                      group: '.col-sm-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  //AppCancellationMessage: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  TextDelivery: {
                      group: '.col-lg-7',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  VoiceDelivery: {
                      group: '.col-lg-5',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  VoiceTimeZone: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  //TextTimeZone: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  EmailDelivery: {
                      group: '.col-lg-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           remindersSettingsDetail.SaveRemindersSettings();

       });

    },//

    LoadingDropDowns: function () {
        var methodName = ['GetFacility'];
        var dfd = new $.Deferred();
        BackgroundLoaderShow(true);
        MDVisionService.lookups(methodName, true, "").done(function (results) {
            var htmlRace = '';
            var htmlFacility = '';
            var facilities = JSON.parse(results['GetFacility']);
            $.each(facilities, function (j, result) {
                if (result.Name != "- Select -")
                    htmlFacility += '<option value="' + result.Value + '">' + result.Name + '</option>';
            });
            $('#' + remindersSettingsDetail.params.PanelID + ' #divVoiceSetting #VoiceSettingsFacility').html(htmlFacility);
            $('#' + remindersSettingsDetail.params.PanelID + ' #divTextSetting #TextSettingsFacility').html(htmlFacility);
            $('#' + remindersSettingsDetail.params.PanelID + ' #divEmailSetting #EmailSettingsFacility').html(htmlFacility);
            BackgroundLoaderShow(false);
            dfd.resolve();
        });
        return dfd;
    },

    IntializeMultiSelectDropDownFacility: function (type, Ctr, Ctrdiv) {
        $('#' + remindersSettingsDetail.params.PanelID + ' #' + Ctr).multiselect('destroy');
        $('#' + remindersSettingsDetail.params.PanelID + ' #' + Ctr).multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            onChange: function (element, checked) {
                var self = $('#' + remindersSettingsDetail.params.PanelID);
                var options = $(element).parent().find('option');
                var Selectedoptions = $(element).parent().find('option:selected');

                $('#' + remindersSettingsDetail.params.PanelID + " #" + Ctr).multiselect('updateButtonText');
                var FacilityIds = self.find('#' + Ctrdiv + ' ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                    if (this.value != "multiselect-all")
                        return this.value;
                }).get().join(',');

                if (type == "Voice")
                    remindersSettingsDetail.params["VoiceFacilityIds"] = FacilityIds;
                else if (type == "Text")
                    remindersSettingsDetail.params["TextFacilityIds"] = FacilityIds;
                else if (type == "Email")
                    remindersSettingsDetail.params["EmailFacilityIds"] = FacilityIds;

                if (FacilityIds != '')
                    remindersSettingsDetail.validateFacility(2, Ctrdiv);
                else
                    remindersSettingsDetail.validateFacility(1, Ctrdiv);
            }
        });
        $('#' + remindersSettingsDetail.params.PanelID + " #" + Ctr).val("");

        $('#' + remindersSettingsDetail.params.PanelID).find('#' + Ctrdiv + ' ul.multiselect-container li input[type=checkbox]').each(function () {
            if ($(this).attr('refval') == "hidden") {
                $(this).parent().addClass('text-bold');
            }
        });
    },

    validateFacility: function (operationid, type_) {

        $("#" + remindersSettingsDetail.params.PanelID + " #" + type_ + " label").find("i").remove();
        if (operationid == 1) {
            $("#" + remindersSettingsDetail.params.PanelID + " #" + type_ + " .multiselect").css("border-color", "#cc2724");
            $("#" + remindersSettingsDetail.params.PanelID + " #" + type_ + "").find(".control-label").css("color", "#cc2724");
            $("#" + remindersSettingsDetail.params.PanelID + " #" + type_ + "").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#" + remindersSettingsDetail.params.PanelID + " #" + type_ + " .multiselect").css("border-color", "#3c763d");
            $("#" + remindersSettingsDetail.params.PanelID + " #" + type_ + "").find(".control-label").css("color", "#3c763d");
            $("#" + remindersSettingsDetail.params.PanelID + " #" + type_ + "").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#" + remindersSettingsDetail.params.PanelID + " #" + type_ + " .multiselect").css("border-color", "#ccc");
            $("#" + remindersSettingsDetail.params.PanelID + " #" + type_ + "").find(".control-label").css("color", "#000000");
        }
    },

    SaveRemindersSettings: function () {


        var strMessage = "";
        var self = $('#' + remindersSettingsDetail.params.PanelID + " #frmRemindersSettingDetail");
        var myJSON = self.getMyJSONByName();

        var selected_ddlType = self.find('#ddlType option:selected').text();

        var form_name = "";
        var type_ = "";
        var FacilityIds = "";
        switch (selected_ddlType) {
            case "Text":
                form_name = "Reminder Settings_Text"; FacilityIds = remindersSettingsDetail.params["TextFacilityIds"]; type_ = "facilityTextdiv";
                break;
            case "Email":
                form_name = "Reminder Settings_Email"; FacilityIds = remindersSettingsDetail.params["EmailFacilityIds"]; type_ = "facilityEmaildiv";
                break;
            case "Voice":
                form_name = "Reminder Settings_Voice"; FacilityIds = remindersSettingsDetail.params["VoiceFacilityIds"]; type_ = "facilityVoicediv";
                break;
            case "- Select -":
                form_name = "no form"
                break;
        }

        if (FacilityIds) {
            remindersSettingsDetail.validateFacility(2, type_);

            if (remindersSettingsDetail.params.mode == "Add") {
                AppPrivileges.GetFormPrivileges(form_name, "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        remindersSettingsDetail.RemindersSettingsSave(myJSON, FacilityIds).done(function (response) {
                            if (response.status != false) {
                                if (selected_ddlType == "Text") {
                                    Admin_RemindersSettings.RemindersSettingsSearch();
                                } else if (selected_ddlType == "Email") {
                                    Admin_RemindersSettings.RemindersSettingsSearchEmail();
                                } else if (selected_ddlType = "Voice") {
                                    Admin_RemindersSettings.RemindersSettingsSearchVoice();
                                }
                                utility.DisplayMessages(response.Message, 1);
                                UnloadActionPan();

                                //$('#' + remindersTemplatesDetail.params.PanelID + ' #frmRemindersTemplateDetail').data('serialize', $('#' + remindersTemplatesDetail.params.PanelID + ' #frmRemindersTemplateDetail').serialize());
                                remindersSettingsDetail.params["ReminderEmailSettingId"] = response.ReminderEmailSettingId;
                                remindersSettingsDetail.params.mode = "Edit";
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
            else if (remindersSettingsDetail.params.mode == "Edit") {

                AppPrivileges.GetFormPrivileges(form_name, "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        remindersSettingsDetail.RemindersSettingsUpdate(myJSON, FacilityIds).done(function (response) {
                            // response = JSON.parse(response);
                            if (response.status != false) {
                                if (selected_ddlType == "Text") {
                                    Admin_RemindersSettings.RemindersSettingsSearch();
                                } else if (selected_ddlType == "Email") {
                                    Admin_RemindersSettings.RemindersSettingsSearchEmail();
                                } else if (selected_ddlType = "Voice") {
                                    Admin_RemindersSettings.RemindersSettingsSearchVoice();
                                }
                                utility.DisplayMessages(response.Message, 1);
                                UnloadActionPan();

                                // remindersTemplatesDetail.TemplateContent = tinyMCE.activeEditor.getContent();
                                $('#' + remindersSettingsDetail.params.PanelID + ' #frmRemindersSettingDetail').data('serialize', $('#' + remindersSettingsDetail.params.PanelID + ' #frmRemindersSettingDetail').serialize());

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

        }
        else {
            remindersSettingsDetail.validateFacility(1, type_);
        }


    },
    RemindersSettingsUpdate: function (SettingsData, FacilityIds) {

        var objData = JSON.parse(SettingsData);
        if (remindersSettingsDetail.params.Type == "email") {
            objData["commandType"] = "update_email_settings";
            objData["ReminderEmailSettingId"] = remindersSettingsDetail.params.RemindersSettingsId;
            objData["EmailDelivery"] = $('#' + remindersSettingsDetail.params.PanelID + ' #EmailDurationType').val() + ' ' + $('#' + remindersSettingsDetail.params.PanelID + ' #ddlEmailDelivery').val();

        }
        else if (remindersSettingsDetail.params.Type == "text") {
            objData["commandType"] = "update_text_settings";
            objData["ReminderTextSettingId"] = remindersSettingsDetail.params.RemindersSettingsId;
            objData["TextDelivery"] = $('#' + remindersSettingsDetail.params.PanelID + ' #TextDurationType').val() + ' ' + $('#' + remindersSettingsDetail.params.PanelID + ' #ddlTextDelivery').val();
            if ($('#rdFailoverFalse').is(':checked'))
                objData["DisableVoiceReminderFailover"] = "false";
            else
                objData["DisableVoiceReminderFailover"] = "true";
        }
        else if (remindersSettingsDetail.params.Type == "voice") {
            objData["commandType"] = "update_voice_settings";
            objData["ReminderVoiceSettingId"] = remindersSettingsDetail.params.RemindersSettingsId;
            objData["VoiceDelivery"] = $('#' + remindersSettingsDetail.params.PanelID + ' #VoiceDurationType').val() + ' ' + $('#' + remindersSettingsDetail.params.PanelID + ' #ddlVoiceDelivery').val();
            if ($("#IsRepeatMessage").prop('checked'))
                objData["RepeatMessage"] = "true";
            else
                objData["RepeatMessage"] = "false";
        }

        objData["FacilityIds"] = FacilityIds;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "Settings");

    },
    RemindersSettingsSave: function (TemplateData, FacilityIds) {
        var type = $('#' + remindersSettingsDetail.params.PanelID + " #ddlType").val();
        var objData = JSON.parse(TemplateData);
        if (type == "email") {
            objData["commandType"] = "save_email_settings";
            objData["EmailDelivery"] = $('#' + remindersSettingsDetail.params.PanelID + ' #EmailDurationType').val() + ' ' + $('#' + remindersSettingsDetail.params.PanelID + ' #ddlEmailDelivery').val();
        }
        else if (type == "text") {
            if ($('#rdFailoverFalse').is(':checked'))
                objData["DisableVoiceReminderFailover"] = "false";
            else
                objData["DisableVoiceReminderFailover"] = "true";
            objData["commandType"] = "save_text_settings";
            objData["TextDelivery"] = $('#' + remindersSettingsDetail.params.PanelID + ' #TextDurationType').val() + ' ' + $('#' + remindersSettingsDetail.params.PanelID + ' #ddlTextDelivery').val();
        }
        else if (type == "voice") {
            if ($("#IsRepeatMessage").prop('checked'))
                objData["RepeatMessage"] = "true";
            else
                objData["RepeatMessage"] = "false";
            objData["commandType"] = "save_voice_settings";
            objData["VoiceDelivery"] = $('#' + remindersSettingsDetail.params.PanelID + ' #VoiceDurationType').val() + ' ' + $('#' + remindersSettingsDetail.params.PanelID + ' #ddlVoiceDelivery').val();

        }

        objData["FacilityIds"] = FacilityIds;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "Settings");

    },
    fillTemplateDDL: function (dfd) {


        if (remindersSettingsDetail.params.mode == "Edit") {
            if (remindersSettingsDetail.params["Type"] == "text") {
                CacheManager.BindDropDownsByEntityID('#pnlRemindersSettingDetail #DDLAppReminderMessage', 'GetRemindersTemplateType', true, 'text').done(function (response) {

                    CacheManager.BindDropDownsByEntityID('#pnlRemindersSettingDetail #DDLAppConfirmationMessage', 'GetRemindersTemplateType', false, 'text');
                    //CacheManager.BindDropDownsByEntityID('#pnlRemindersSettingDetail #DDLAppCancellationMessage', 'GetRemindersTemplateType', false, 'text');

                });
            }
            else if (remindersSettingsDetail.params["Type"] == "voice") {
                CacheManager.BindDropDownsByEntityID('#pnlRemindersSettingDetail #ddlVoiceTemplate', 'GetRemindersTemplateType', true, 'voice');
            }
            else if (remindersSettingsDetail.params["Type"] == "email") {
                CacheManager.BindDropDownsByEntityID('#pnlRemindersSettingDetail #ddlEmailTemplate', 'GetRemindersTemplateType', true, 'email');
            }
        }
        else {

            CacheManager.BindDropDownsByEntityID('#pnlRemindersSettingDetail #DDLAppReminderMessage', 'GetRemindersTemplateType', true, 'text').done(function (response) {

                CacheManager.BindDropDownsByEntityID('#pnlRemindersSettingDetail #DDLAppConfirmationMessage', 'GetRemindersTemplateType', false, 'text');
                //CacheManager.BindDropDownsByEntityID('#pnlRemindersSettingDetail #DDLAppCancellationMessage', 'GetRemindersTemplateType', false, 'text');

            });

            CacheManager.BindDropDownsByEntityID('#pnlRemindersSettingDetail #ddlEmailTemplate', 'GetRemindersTemplateType', true, 'email');
            CacheManager.BindDropDownsByEntityID('#pnlRemindersSettingDetail #ddlVoiceTemplate', 'GetRemindersTemplateType', true, 'voice');
        }

        dfd.resolve("ok");
        return dfd;
    },

    ShowHideSettings: function (obj) {

        var type = $('#' + remindersSettingsDetail.params.PanelID + " #ddlType").val();

        switch (type) {
            case "text":
                $('#' + remindersSettingsDetail.params.PanelID + " #divTextSetting").css("display", "inline");
                $('#' + remindersSettingsDetail.params.PanelID + " #divEmailSetting").css("display", "none");
                $('#' + remindersSettingsDetail.params.PanelID + " #divVoiceSetting").css("display", "none");
                $('#' + remindersSettingsDetail.params.PanelID + " #divHTMLTemplate").css("display", "none");
                $('#' + remindersSettingsDetail.params.PanelID + " #divTextSettingsMessage").css("display", "inline");
                $('#' + remindersSettingsDetail.params.PanelID + " #divTextSettingsDDLs").css("display", "inline");
                break;
            case "email":
                $('#' + remindersSettingsDetail.params.PanelID + " #divTextSetting").css("display", "none");
                $('#' + remindersSettingsDetail.params.PanelID + " #divEmailSetting").css("display", "inline");
                $('#' + remindersSettingsDetail.params.PanelID + " #divVoiceSetting").css("display", "none");
                $('#' + remindersSettingsDetail.params.PanelID + " #divHTMLTemplate").css("display", "inline");
                $('#' + remindersSettingsDetail.params.PanelID + " #divTextSettingsMessage").css("display", "none");
                $('#' + remindersSettingsDetail.params.PanelID + " #divTextSettingsDDLs").css("display", "none");
                break;
            case "voice":
                $('#' + remindersSettingsDetail.params.PanelID + " #divTextSetting").css("display", "none");
                $('#' + remindersSettingsDetail.params.PanelID + " #divEmailSetting").css("display", "none");
                $('#' + remindersSettingsDetail.params.PanelID + " #divVoiceSetting").css("display", "inline");
                $('#' + remindersSettingsDetail.params.PanelID + " #divHTMLTemplate").css("display", "inline");
                $('#' + remindersSettingsDetail.params.PanelID + " #divTextSettingsMessage").css("display", "none");
                $('#' + remindersSettingsDetail.params.PanelID + " #divTextSettingsDDLs").css("display", "none");
                break;
        }


    },

    fillTextTemplate: function (type) {

        switch (type) {
            case "text":
                $('#' + remindersSettingsDetail.params.PanelID + " #txtTemplate").val("");
                var newText = $('#' + remindersSettingsDetail.params.PanelID + " #ddlTextTemplate option:selected").attr("refvalue");

                $('#' + remindersSettingsDetail.params.PanelID + " #txtTemplate").val(newText);
                break;
            case "email":
                $('#' + remindersSettingsDetail.params.PanelID + " #txtTemplate").val("");
                var newText = $('#' + remindersSettingsDetail.params.PanelID + " #ddlEmailTemplate option:selected").attr("refvalue");

                $('#' + remindersSettingsDetail.params.PanelID + " #txtTemplate").val(newText);
                break;
            case "voice":
                $('#' + remindersSettingsDetail.params.PanelID + " #txtTemplate").val("");
                var newText = $('#' + remindersSettingsDetail.params.PanelID + " #ddlVoiceTemplate option:selected").attr("refvalue");

                $('#' + remindersSettingsDetail.params.PanelID + " #txtTemplate").val(newText);
                break;
        }

        if ($('#frmRemindersSettingDetail').data('bootstrapValidator') != null && typeof $('#frmRemindersSettingDetail').data('bootstrapValidator') != 'undefined') {
            $('#frmRemindersSettingDetail').bootstrapValidator('revalidateField', 'HTMLTemplate');
        }

    },

    fillTextTemplateTextMessage: function (type) {
        switch (type) {
            case "appointment":
                $('#' + remindersSettingsDetail.params.PanelID + " #txtAppReminderMessage").val("");
                var newText = $('#' + remindersSettingsDetail.params.PanelID + " #DDLAppReminderMessage option:selected").attr("refvalue");
                newText = newText.substr(0, 160);
                $('#' + remindersSettingsDetail.params.PanelID + " #txtAppReminderMessage").val(newText);

                $("#frmRemindersSettingDetail #spnTxtMsg").html((160) - (newText.length));
                if ($('#frmRemindersSettingDetail').data('bootstrapValidator') != null && typeof $('#frmRemindersSettingDetail').data('bootstrapValidator') != 'undefined') {
                    $('#frmRemindersSettingDetail').bootstrapValidator('revalidateField', 'AppReminderMessage');
                }
                break;
            case "confirmation":
                $('#' + remindersSettingsDetail.params.PanelID + " #txtAppConfirmationMessage").val("");
                var newText = $('#' + remindersSettingsDetail.params.PanelID + " #DDLAppConfirmationMessage option:selected").attr("refvalue");
                newText = newText.substr(0, 160);
                $('#' + remindersSettingsDetail.params.PanelID + " #txtAppConfirmationMessage").val(newText);

                $("#frmRemindersSettingDetail #spnConfirmationMsg").html((160) - (newText.length));
                if ($('#frmRemindersSettingDetail').data('bootstrapValidator') != null && typeof $('#frmRemindersSettingDetail').data('bootstrapValidator') != 'undefined') {
                    $('#frmRemindersSettingDetail').bootstrapValidator('revalidateField', 'AppConfirmationMessage');
                }
                break;
                //case "cancellation":
                //    $('#' + remindersSettingsDetail.params.PanelID + " #txtAppCancellationMessage").val("");
                //    var newText = $('#' + remindersSettingsDetail.params.PanelID + " #DDLAppCancellationMessage option:selected").attr("refvalue");
                //    newText = newText.substr(0, 105);
                //    $('#' + remindersSettingsDetail.params.PanelID + " #txtAppCancellationMessage").val(newText);
                //    $("#frmRemindersSettingDetail #spnCancelMsg").html((105) - (newText.length));

                //    if ($('#frmRemindersSettingDetail').data('bootstrapValidator') != null && typeof $('#frmRemindersSettingDetail').data('bootstrapValidator') != 'undefined') {
                //        $('#frmRemindersSettingDetail').bootstrapValidator('revalidateField', 'AppCancellationMessage');
                //    }
                //    break;
        }

    },

    msgLengthSet: function () {
        var maxLength = parseInt(160);
        var textBoxVal = $("#frmRemindersSettingDetail #txtAppReminderMessage").val();
        var textBoxCharLength = parseInt(textBoxVal.length);
        var remainingChars = maxLength - textBoxCharLength;
        if (remainingChars < 0) {
            $("#frmRemindersSettingDetail #spnTxtMsg").html(0);
            $("#frmRemindersSettingDetail #txtAppReminderMessage").val(textBoxVal.substr(0, 160));
        } else {
            $("#frmRemindersSettingDetail #spnTxtMsg").html(remainingChars);
        }

        if (remainingChars <= 0)
            $("#frmRemindersSettingDetail #txtAppReminderMessage").attr('maxlength', 160);
        else
            $("#frmRemindersSettingDetail #txtAppReminderMessage").attr('maxlength', "");

    },

    cancelMsgLengthSet: function () {
        var maxLength = parseInt(160);
        var textBoxVal = $("#frmRemindersSettingDetail #txtAppConfirmationMessage").val();
        var textBoxCharLength = parseInt(textBoxVal.length);
        var remainingChars = maxLength - textBoxCharLength;
        if (remainingChars < 0) {
            $("#frmRemindersSettingDetail #spnConfirmationMsg").html(0);
            $("#frmRemindersSettingDetail #txtAppConfirmationMessage").val(textBoxVal.substr(0, 160));
        } else {
            $("#frmRemindersSettingDetail #spnConfirmationMsg").html(remainingChars);
        }

        if (remainingChars <= 0)
            $("#frmRemindersSettingDetail #txtAppConfirmationMessage").attr('maxlength', 160);
        else
            $("#frmRemindersSettingDetail #txtAppConfirmationMessage").attr('maxlength', "");
    },

    //confirmationMsgLengthSet: function () {
    //    var maxLength = parseInt(105);
    //    var textBoxVal = $("#frmRemindersSettingDetail #txtAppCancellationMessage").val();
    //    var textBoxCharLength = parseInt(textBoxVal.length);
    //    var remainingChars = maxLength - textBoxCharLength;
    //    $("#frmRemindersSettingDetail #spnCancelMsg").html(remainingChars);

    //    if (remainingChars <= 0)
    //        $("#frmRemindersSettingDetail #txtAppCancellationMessage").attr('maxlength', textBoxCharLength);
    //    else
    //        $("#frmRemindersSettingDetail #txtAppCancellationMessage").attr('maxlength', "");
    //}
}