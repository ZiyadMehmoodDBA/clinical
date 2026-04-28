ParticipentProviderDetail = {

    params: [],
    Load: function (params) {
        ParticipentProviderDetail.params = params;
        var self = null;
        if (ParticipentProviderDetail.params.PanelID != 'ParticipentProviderDetail')
            self = $('#' + ParticipentProviderDetail.params.PanelID + ' #ParticipentProviderDetail')
        else
            self = $('#ParticipentProviderDetail');
        self.loadDropDowns(true).done(function () {
            ParticipentProviderDetail.LoadProviderParticipant();
            $('#frmParticipentProviderDetail').data('serialize', $('#ParticipentProviderDetail').serialize());
        });
        utility.CreateDatePicker(ParticipentProviderDetail.params.PanelID + ' #frm_ParticipentProviderDetail #dpAssingStartDate', function (ev) { }, true);
        ParticipentProviderDetail.LoadAllAutocomplete();
        ParticipentProviderDetail.ValidateParticipentProvider();
    },
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $("#ParticipentProviderDetail #hfProvider").val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'ParticipentProviderDetail';
        LoadActionPan('providerDetail', params);
    },
    OpenProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["IsOptional"] = false;
        params["RefForm"] = "frm_ParticipentProviderDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'ParticipentProviderDetail';
        LoadActionPan('Admin_Provider', params);
    },
    LoadAllAutocomplete: function () {
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            $("input#txtProvider").autocomplete({
                autoFocus: true,
                source: Providers, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        if ($("#frm_ParticipentProviderDetail #lnkProviderEdit").css("display") == "none") {
                            $("#frm_ParticipentProviderDetail #lnkProviderEdit").css("display", "");
                            $("#frm_ParticipentProviderDetail #lblProvider").css("display", "none");
                        }
                        $("#frm_ParticipentProviderDetail #txtProvider").attr("ProviderId", ui.item.id); // add the selected id
                        $("#frm_ParticipentProviderDetail #hfProvider").val(ui.item.id);
                    }, 100);
                }
            });
        });
    },
    ValidateParticipentProvider: function () {
        $('#ParticipentProviderDetail #frm_ParticipentProviderDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  Provider: {
                      group: '.col-md-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Assignment: {
                      group: '.col-md-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  ProviderParticipantStatusId: {
                      group: '.col-md-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  }
              }
          })

       .on('success.form.bv', function (e) {
           e.preventDefault();
           ParticipentProviderDetail.ParticipantProviderSave();
       });
    },
    LoadProviderParticipant: function () {
        if (ParticipentProviderDetail.params.mode == "Add") {
            $('#ParticipentProviderDetail #txtProvider').attr("enabled", "enabled");
        }
        else if (ParticipentProviderDetail.params.mode == "Edit") {
            $('#ParticipentProviderDetail #txtProvider').attr("disabled", "disabled");
            ParticipentProviderDetail.FillParticipantProvider(ParticipentProviderDetail.params.ParticipantProviderId).done(function (response) {
                if (response.status != false) {
                    var participantProvider_detail = JSON.parse(response.ParticipantFill_JSON);
                    var self = $("#ParticipentProviderDetail");
                    utility.bindMyJSON(true, participantProvider_detail, false, self);
                    $('#frm_ParticipentProviderDetail').data('serialize', $('#frm_ParticipentProviderDetail').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ParticipantProviderSave: function () {
        var strMessage = "";
        var self = $("#ParticipentProviderDetail");
        var myJSON = self.getMyJSON();
        if (ParticipentProviderDetail.params.mode == "Add") {
          //  AppPrivileges.GetFormPrivileges("ParticipentProvider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

               // if (strMessage == "") {
                    ParticipentProviderDetail.SaveParticipantProvider(myJSON).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            ParticipentProviderDetail.UnloadPan();
                            Admin_ParticipentProvider.ParticipantProviderSearch(response.ParticipantProviderId);
                            CacheManager.BindCodes('GetProviderParticipentStatus', true);
                        }
                        else {
                            if (response.Message.indexOf('duplicate key') > -1) {
                                utility.DisplayMessages("Participant Provider already exists", 3);
                            } else
                                utility.DisplayMessages(response.Message, 3);
                        }
                    });
               // }
                //else
                //    utility.DisplayMessages(strMessage, 2);
           // });
        }
        else if (ParticipentProviderDetail.params.mode == "Edit") {
          //  AppPrivileges.GetFormPrivileges("ParticipentProvider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
              //  if (strMessage == "") {
                    ParticipentProviderDetail.UpdateParticipantProvider(myJSON, ParticipentProviderDetail.params.ParticipantProviderId).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            ParticipentProviderDetail.UnloadPan();
                            Admin_ParticipentProvider.ParticipantProviderSearch(ParticipentProviderDetail.params.ParticipantProviderId);
                            CacheManager.BindCodes('GetProviderParticipentStatus', true);
                        }
                        else {
                            if (response.Message.indexOf('duplicate key') > -1) {
                                utility.DisplayMessages("Participant Provider already exists for this Entity", 3);
                            } else
                                utility.DisplayMessages(response.Message, 3);
                        }
                    });
               // }
                //else
                //    utility.DisplayMessages(strMessage, 2);
           // });
        }
    },
    SaveParticipantProvider: function (ParticipantProviderData) {
        var data = "ParticipantProviderData=" + ParticipantProviderData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PARTICIPANT_PROVIDER_DETAIL", "SAVE_PARTICIPANT_PROVIDER");
    },

    UpdateParticipantProvider: function (ParticipantProviderData, ParticipantProviderId) {
        var data = "ParticipantProviderData=" + ParticipantProviderData + "&ParticipantProviderId=" + ParticipantProviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PARTICIPANT_PROVIDER_DETAIL", "UPDATE_PARTICIPANT_PROVIDER");
    },

    FillParticipantProvider: function (ParticipantProviderId) {
        var data = "ParticipantProviderId=" + ParticipantProviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PARTICIPANT_PROVIDER_DETAIL", "FILL_Participant_PROVIDER");
    },
    UpdateParticipantProviderActiveInactive: function (ParticipantProviderId, IsActive) {
        var data = "ParticipantProviderId=" + ParticipantProviderId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PARTICIPANT_PROVIDER_DETAIL", "UPDATE_PARTICIPANT_PROVIDER_ACTIVE_INACTIVE");
    },

    UnLoad: function () {
        utility.UnLoadDialog("frmParticipentProviderDetail", function () {
            ParticipentProviderDetail.UnloadPan();
        }, function () {
            ParticipentProviderDetail.UnloadPan();
        });
    },

    UnloadPan: function () {

        if (ParticipentProviderDetail.params != null && ParticipentProviderDetail.params.ParentCtrl != null) {
            UnloadActionPan(ParticipentProviderDetail.params.ParentCtrl, 'ParticipentProviderDetail');
        }
        else
            UnloadActionPan();
    },
}