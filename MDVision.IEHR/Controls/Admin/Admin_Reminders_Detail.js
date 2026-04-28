remindersDetail = {
    bIsFirstLoad: true,
    params: [],
    imageSize: 0,
    Content: "",
    Iferror: false,
    providerCheckedIds: [],
    ProviderIds: '',

    Load: function (params) {

        remindersDetail.params = params;

        if (remindersDetail.params.PanelID != 'pnlRemindersDetail') {
            remindersDetail.params.PanelID = remindersDetail.params.PanelID + ' #pnlRemindersDetail';
        } else {
            remindersDetail.params.PanelID = 'pnlRemindersDetail';
        }


        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #divEntity').show();
        }

        var self = $('#' + remindersDetail.params.PanelID);
        utility.CreateDatePicker(remindersDetail.params.PanelID + ' #frmRemindersDetail #dtpDeliveryDate', function (ev) { });

        //set ReminderCall_CalleeName
        self.find("#txtSMSCalleeName").val(remindersDetail.params["FacilityPhoneNo"]);
        self.find("#txtVoiceCalleeName").val(remindersDetail.params["FacilityPhoneNo"]);


        self.loadDropDowns(true).done(function () {
            $('#pnlRemindersDetail #txtfromname').val(globalAppdata['AppUserFirstName'] + " " + globalAppdata['AppUserLastName']);
            $('#pnlRemindersDetail #txtEmailTo').val(remindersDetail.params["PatientEmail"]);
        });
        remindersDetail.SelectReminderType();
        remindersDetail.fillTemplateDDL();
        remindersDetail.ValidateRemindersTemplateSettings();
        utility.CreateDatePicker(remindersDetail.params["PanelID"] + ' #deliveryDate', function () {
        });

    },

    SelectReminderType: function () {

        if (remindersDetail.params.ScreenType == "SMS") {
            $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #txtSMSPhoneNumber').val(remindersDetail.params.patientNumber);
            $('#' + remindersDetail.params.PanelID + ' #headerId').html("SMS Reminder ");
            $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #SMSreminder').show();

            $('#' + remindersDetail.params.PanelID + ' #txtTemplate').attr("maxlength", 160);

            $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #divSpanCount').css("display", "inline");

            if (remindersDetail.params.patientGuarantorId == "") {
                $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #divSMSGuarantor').css("display", "none");
            } else {
                $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #divSMSGuarantor').css("display", "inline");
                $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #txtSMSGuarantorPhoneNumber').val(remindersDetail.params.patientGuarantorNumber);
            }

        } else if (remindersDetail.params.ScreenType == "VOICE") {
            $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #txtVoicePhoneNumber').val(remindersDetail.params.patientNumber);
            $('#' + remindersDetail.params.PanelID + ' #headerId').html("Voice Reminder ");
            $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #voicereminder').show();
            $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #divSpanvoiceCount').css("display", "inline");
            if (remindersDetail.params.patientGuarantorId == "") {
                $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #divVoiceGuarantor').css("display", "none");
            } else {
                $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #divVoiceGuarantor').css("display", "inline");
                $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #txtVoiceGuarantorPhoneNumber').val(remindersDetail.params.patientGuarantorNumber);
            }
            $('#' + remindersDetail.params.PanelID + ' #txtTemplate').attr("maxlength", "");
        } else if (remindersDetail.params.ScreenType == "EMAIL") {
            $('#' + remindersDetail.params.PanelID + ' #headerId').html("Email Reminder ");
            $('#' + remindersDetail.params.PanelID + ' #frmRemindersDetail #emailreminder').show();
            $("#" + remindersDetail.params.PanelID + " #frmRemindersDetail #divDelivery").addClass("hidden");
            $('#' + remindersDetail.params.PanelID + ' #txtTemplate').attr("maxlength", "");
            //  remindersDetail.fillEmailFrom();
        }
    },

    QuickReminderSave: function (Data) {

        var deliveryMinutes = $('#' + remindersDetail.params.PanelID + " #ddlDelivery").val();


        if (remindersDetail.params.ScreenType == "EMAIL") {
            var objData = JSON.parse(Data);
            objData["ProviderId"] = remindersDetail.params.ProviderId;
            objData["AppointmentId"] = remindersDetail.params.AppointmentId;
            objData["PatientId"] = remindersDetail.params.PatientId;
            objData["commandType"] = "send_quick_email";
            objData["PatientName"] = remindersDetail.params.PatientName;
            objData["ProviderName"] = remindersDetail.params.ProviderName;
            objData["AppointmentDate"] = remindersDetail.params.AppointmentDate;
            objData["DeliveryMinutes"] = deliveryMinutes;
            var data = JSON.stringify(objData);
            return MDVisionService.PMSAPIService(data, "Reminders", "QuickReminder");
        }
        else if (remindersDetail.params.ScreenType == "SMS") {
            var objData = JSON.parse(Data);
            objData["ProviderId"] = remindersDetail.params.ProviderId;
            objData["AppointmentId"] = remindersDetail.params.AppointmentId;
            objData["PatientId"] = remindersDetail.params.PatientId;
            objData["commandType"] = "send_quick_sms";
            objData["DeliveryMinutes"] = deliveryMinutes;
            var data = JSON.stringify(objData);
            return MDVisionService.PMSAPIService(data, "Reminders", "QuickReminder");
        }
        else if (remindersDetail.params.ScreenType == "VOICE") {
            var objData = JSON.parse(Data);
            objData["ProviderId"] = remindersDetail.params.ProviderId;
            objData["AppointmentId"] = remindersDetail.params.AppointmentId;
            objData["PatientId"] = remindersDetail.params.PatientId;
            objData["DeliveryMinutes"] = deliveryMinutes;
            objData["commandType"] = "send_quick_voice";
            var data = JSON.stringify(objData);
            return MDVisionService.PMSAPIService(data, "Reminders", "QuickReminder");
        }

    },
    //emailFromFill: function () {
    //    var objData = new Object();
    //    objData["ProviderId"] = remindersDetail.params.ProviderId;
    //    objData["commandType"] = "fill_quick_email";
    //    var data = JSON.stringify(objData);
    //    return MDVisionService.PMSAPIService(data, "Reminders", "QuickReminder");



    //},
    //fillEmailFrom: function () {
    //    Patient_Demographic.emailFromFill().done(function () {
    //        $('#frmRemindersDetail').data('serialize', $('#frmRemindersDetail').serialize());


    //    });
    //},

    UnLoadTab: function () {
        //remindersTemplatesDetail.UnloadTemplate();
        UnloadActionPan();
    },

    //fillTextTemplate: function (type) {

    //    switch (type) {
    //        case "text":
    //            $('#' + remindersDetail.params.PanelID + " #txtTemplate").val("");
    //            var newText = $('#' + remindersDetail.params.PanelID + " #ddlTextTemplate option:selected").attr("refvalue");
    //            $('#' + remindersDetail.params.PanelID + " #txtTemplate").val(newText);
    //            break;
    //        case "voice":
    //            $('#' + remindersDetail.params.PanelID + " #txtTemplate").val("");
    //            var newText = $('#' + remindersDetail.params.PanelID + " #ddlVoiceTemplate option:selected").attr("refvalue");
    //            $('#' + remindersDetail.params.PanelID + " #txtTemplate").val(newText);
    //            break;
    //    }

    //},

    fillTextTemplate: function (type) {
        var tempType = type;
        switch (type) {
            case "text":
                $('#' + remindersDetail.params.PanelID + " #txtTemplate").val("");
                var newText = $('#' + remindersDetail.params.PanelID + " #ddlTextTemplate option:selected").attr("refvalue");
                $('#' + remindersDetail.params.PanelID + " #txtTemplate").val(newText);
                break;
            case "voice":
                $('#' + remindersDetail.params.PanelID + " #txtTemplate").val("");
                var newText = $('#' + remindersDetail.params.PanelID + " #ddlVoiceTemplate option:selected").attr("refvalue");
                $('#' + remindersDetail.params.PanelID + " #txtTemplate").val(newText);
                break;
            case "email":
                $('#' + remindersDetail.params.PanelID + " #txtTemplate").val("");
                var newText = $('#' + remindersDetail.params.PanelID + " #ddlEmailTemplate option:selected").attr("refvalue");
                $('#' + remindersDetail.params.PanelID + " #txtTemplate").val(newText);
                if ($('#pnlRemindersDetail #frmRemindersDetail').data("bootstrapValidator") != null) {
                    $('#pnlRemindersDetail #frmRemindersDetail').bootstrapValidator('revalidateField', 'HTMLTemplate');
                }
        }

        remindersDetail.fillHTMLTagValues().done(function (response) {
            if (response.status != false) {
                if (tempType == "text") {
                    $('#' + remindersDetail.params.PanelID + " #txtTemplate").val(response.newHTMLTemplate.substr(0, 160));
                    $("#frmRemindersDetail #spnCount").html(160 - parseInt($('#' + remindersDetail.params.PanelID + " #txtTemplate").val().length));
                } else {
                    $('#' + remindersDetail.params.PanelID + " #txtTemplate").val(response.newHTMLTemplate);
                }
                if ($('#frmRemindersDetail').data('bootstrapValidator') != null && typeof $('#frmRemindersDetail').data('bootstrapValidator') != 'undefined') {
                    $('#frmRemindersDetail').bootstrapValidator('revalidateField', 'HTMLTemplate');
                }
            }

        });

    },

    fillHTMLTagValues: function () {
        var objData = new Object();
        objData["ProviderId"] = remindersDetail.params.ProviderId;
        objData["AppointmentId"] = remindersDetail.params.AppointmentId;
        objData["PatientId"] = remindersDetail.params.PatientId;
        objData["HTMLTemplate"] = $('#' + remindersDetail.params.PanelID + " #txtTemplate").val();
        objData["commandType"] = "get_html_string";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "QuickReminder");
    },

    fillTemplateDDL: function () {

        CacheManager.BindDropDownsByEntityID('#pnlRemindersDetail #ddlTextTemplate', 'GetRemindersTemplateType', true, 'text').done(function () {
            if ($('#pnlRemindersDetail #ddlTextTemplate > option').length == 1)
                $('#pnlRemindersDetail #SelectTemplatedropdownSMS').addClass('hidden');
        });
        
        CacheManager.BindDropDownsByEntityID('#pnlRemindersDetail #ddlVoiceTemplate', 'GetRemindersTemplateType', true, 'voice');
        CacheManager.BindDropDownsByEntityID('#pnlRemindersDetail #ddlEmailTemplate', 'GetRemindersTemplateType', true, 'email');

    },

    ValidateRemindersTemplateSettings: function () {

        $('#frmRemindersDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  VoiceCalleeName: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  VoicePhoneNumber: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  SMSCalleeName: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  SMSPhoneNumber: {
                      group: '.col-sm-4',
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
                  },
                  MessageVoice: {
                      group: '.col-sm-4',
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
                  TextTimeZone: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  SMSGuarantorPhoneNumber: {
                      group: '.col-sm-4',
                      //enabled: false,
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  FromName: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  EmailTo: {
                      group: '.col-sm-4',
                      validators: {
                          regexp: {
                              regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                              message: 'Email not Valid'
                          },
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Subject: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  delivery: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  EmailTimeZone: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  VoiceGuarantorPhoneNumber: {
                      group: '.col-sm-4',
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
           remindersDetail.SaveQuickReminder();

       });
    },
    EnableValidation: function (obj) {

        var objfacility = $('#pnlRemindersDetail #frmRemindersDetail');
        var formValidation = objfacility.data("bootstrapValidator");
        if ($(obj).val() != "") {
            formValidation.enableFieldValidators($(obj).attr("name"), true);
        }
        else {
            formValidation.enableFieldValidators($(obj).attr("name"), false);
        }

    },
    SaveQuickReminder: function () {
        var strMessage = "";
        var self = $('#' + remindersDetail.params.PanelID + " #frmRemindersDetail");
        var myJSON = self.getMyJSONByName();

        if (remindersDetail.params.mode == "Add") {

            remindersDetail.QuickReminderSave(myJSON).done(function (response) {
                if (response.status != false) {

                    utility.DisplayMessages(response.Message, 1);
                    UnloadActionPan();

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });



        }


    },

    getDeliveryDateTime: function (obj) {
        if ($('#' + remindersDetail.params.PanelID + " #ddlDelivery").val() == "other") {
            $('#' + remindersDetail.params.PanelID + " #divDeliveryDate").css("display", "inline");
        } else {
            $('#' + remindersDetail.params.PanelID + " #divDeliveryDate").css("display", "none");
        }
    },

    setMessageCount: function () {

        if (remindersDetail.params.ScreenType == "SMS") {
            var maxLength = parseInt(160);
            var textBoxVal = $("#frmRemindersDetail #txtTemplate").val();
            var textBoxCharLength = parseInt(textBoxVal.length);
            var remainingChars = maxLength - textBoxCharLength;
            $("#frmRemindersDetail #spnCount").html(remainingChars);
            if (remainingChars <= 0)
                $("#frmRemindersDetail #txtTemplate").attr('maxlength', textBoxCharLength);
            else
                $("#frmRemindersDetail #txtTemplate").attr('maxlength', "");
            // EMR-1756 
            if (remainingChars < 0) {
                $("#frmRemindersDetail #txtTemplate").val($("#frmRemindersDetail #txtTemplate").val().substring(0, maxLength));
                $("#frmRemindersDetail #spnCount").html(0);
            }
            //
        }
    },
}