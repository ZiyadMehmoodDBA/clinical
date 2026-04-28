/*Author : Khaleel Ur Rehman.
Date : 16-03-2016
Purpose : To Handle CoSign when view a Note.
*/

Clinical_NotesCoSign = {
    Load: function (params) {
        Clinical_NotesCoSign.params = params;

        if (Clinical_NotesCoSign.params.PanelID != 'pnlClinical_NotesCoSign') {
            Clinical_NotesCoSign.params.PanelID = Clinical_NotesCoSign.params.PanelID + ' #pnlClinical_NotesCoSign';
        } else {
            Clinical_NotesCoSign.params.PanelID = 'pnlClinical_NotesCoSign';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_NotesCoSign.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        var self = $('#' + Clinical_NotesCoSign.params.PanelID);
        Clinical_NotesCoSign.LoadAutocomplete();

    },
    LoadAutocomplete: function () {
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            $("input#txtProvider").autocomplete({
                autoFocus: true,
                source: Providers, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlClinical_NotesCoSign #txtProvider").attr("ProviderId", ui.item.id); // add the selected id
                        $("#pnlClinical_NotesCoSign #hfProvider").val(ui.item.id); // add the selected id
                        if ($("#pnlClinical_NotesCoSign #lnkProviderEdit").css("display") == "none") {
                            $("#pnlClinical_NotesCoSign #lnkProviderEdit").css("display", "inline");
                            $("#pnlClinical_NotesCoSign #lblProvider").css("display", "none");
                        }
                    }, 100);
                }

            });
            $("input#txtProvider").autocomplete("option", "appendTo", "#frmClinical_NotesCoSign");
        });
    },
    OpenProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["IsOptional"] = true;
        params["RefForm"] = "frmClinical_NotesCoSign";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Clinical_NotesCoSign';
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $("#pnlClinical_NotesCoSign #ProviderId").val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'Clinical_NotesCoSign';
        LoadActionPan('providerDetail', params);
    },
    ValidateClinical_NotesCoSign: function () {
        $('#frmClinical_NotesCoSign')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  /*TemplateLetterId: {
                      group: '.col-sm-10',
                      validators: {
                          notEmpty: {
                              message: 'Select a template to create the letter. '
                          },
                      }
                  },*/
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Clinical_NotesCoSign.CreateLetter();
       });
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        if (Clinical_NotesCoSign.params["FromAdmin"] == "0") {
            if (Clinical_NotesCoSign.params != null && Clinical_NotesCoSign.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_NotesCoSign.params.ParentCtrl, 'Clinical_NotesCoSign');
            }
            else
                UnloadActionPan(null, 'Clinical_NotesCoSign');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
    CoSignNotes: function () {
        var self = $("#frmClinical_NotesCoSign");
        var myJSON = self.getMyJSON();
        var radioVal = '';
        $('#frmClinical_NotesCoSign input:radio').each(function () {
            if ($(this).is(':checked')) {
                radioVal = $(this).val();
            }
        });
        var coSignedProviderId = self.find("#ProviderId").val();
        if (coSignedProviderId && coSignedProviderId != "") {
            AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('Do you want to Co-Sign this record?', function () {
                        //-------------------------

                        if (Clinical_NotesCoSign.params.NotesId == null) {
                            Clinical_NotesCoSign.params.NotesId = Clinical_Notes.params.NotesId;
                        }
                        //-------------------------
                        //Start || 15 August, 2016 || ZeeshanAK || Fix for EMR-14
                        DashBoard.NotesUpdateCosign(Clinical_NotesCoSign.params.NotesId, radioVal, myJSON, coSignedProviderId, Clinical_NotesCoSign.params.ParentCtrl);
                        var ProviderName = "";
                        //Start || 27 July, 2016 || Talha Tanweer || Story EMR-86
                        var IsWaterMarkApplied = 0;
                        providerDetail.FillProvider(coSignedProviderId, IsWaterMarkApplied).done(function (response) {
                            if (response.status != false) {
                                if (Clinical_NotesCoSign.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var provider_detail = JSON.parse(response.ProviderFill_JSON);
                                    //   var ProviderShortName = provider_detail.txtShortName;
                                    ProviderName = provider_detail.txtShortName;
                                    var eSignature_image_Src = provider_detail.imgeSignature;
                                    var Is_eSignatured = provider_detail.chkIs_eSignatured && provider_detail.chkIs_eSignatured != "" ? JSON.parse(provider_detail.chkIs_eSignatured.toLowerCase()) : false;
                                    var Clinical_progressNotes_formSelector = Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote";

                                    if (eSignature_image_Src != "" && Is_eSignatured) {

                                        var isBrowserIE = providerDetail.GetIEVersion() > 0;
                                        if (isBrowserIE) {
                                            eSignature_image_Src = eSignature_image_Src.replace("System.Byte[]", "image/gif");
                                        }

                                        var imgeSignatureHtml = '<div class="CoSignComponent" NoteComponentId="NCDummyId" style="max-height:350px; overflow-y:auto;margin-top:15px;" >' +
                                                                    '<img id="img_eSignatureCoS_ProgressNotes" src="' + eSignature_image_Src + '" ' +
                                                                         'alt="" style="height: 125px; width: 315px;border:none;" ' +
                                                                         'class="img-responsive img-center mt-lg img-thumbnail"/>'
                                        '</div>';
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append(imgeSignatureHtml);
                                    }
                                    myJSON = JSON.parse(myJSON);
                                    var signeDateTime = moment().format("dddd, MMMM DD, YYYY") + ' at ' + moment().format("HH:mm:ss a");
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append('<ul class="list-unstyled CosignSignature CoSignComponent" NoteComponentId="NCDummyId"><li id="coSignedBy" >_____________________________________________________________________________________________________</li><li>Electronically co-signed by: ' + globalAppdata["AppUserLastName"] + ", " + globalAppdata["AppUserFirstName"] + ' on ' + signeDateTime + '</li><li>"' + radioVal + '"</li><li>Co-Sign Comments: ' + myJSON.txtCommentsCosign + '</li></ul>');
                                }
                            }

                        });
                        //End   || 15 August, 2016 || ZeeshanAK || Fix for EMR-14

                    }, function () { },
                                'Confirm Co-Sign'
                            );
                }
                else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });

        } else {
            Clinical_NotesCoSign.ValidateCoSign("pnlClinical_NotesCoSign #frmClinical_NotesCoSign");
        }
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    ValidateCoSign: function (FrmCtrl) {
        $('#' + Clinical_NotesCoSign.params.PanelID + ' #frmClinical_NotesCoSign').bootstrapValidator('destroy');

        $('#' + FrmCtrl)
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
                            group: '.col-sm-12',
                            validators: {
                                notEmpty: {
                                    message: ''
                                },
                            }
                        }
                    }
                }).on('success.form.bv', function (e) {
                    e.preventDefault();
                });

    },
    /*AmendmentNotes: function () {
        var self = $("#frmClinical_NotesAmendment");
        var myJSON = self.getMyJSON();
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Do you want Amendment of this record?', function () {

                    DashBoard.NotesUpdateAmendment(Clinical_NotesView.params.NotesId, myJSON);
                }, function () { },
                            'Confirm Amendment'
                        );
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },*/
    /*NotesUpdateForCosignORAmendment: function (NotesID, jsondata) {
        var objDdata = {};
        objDdata["NotesID"] = NotesID;
        return MDVisionService.defaultService(data, "DASHBOARD", "UPDATE_NOTES_COSIGN");
    },
    NotesUpdateCosign: function (NotesId, jsonData) {
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Do you want to CoSign this record?', function () {

                    DashBoard.NotesUpdateForCosignORAmendment(NotesId, jsonData).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages("Successfully Signed!", 1);
                            DashBoard.DashBoardEncounterSearch();
                            // Clinical_Notes.UnLoad();
                            //UnloadActionPan(Clinical_Notes.params["ParentCtrl"]);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }, function () { },
                        'Confirm Sign'
                    );
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },*/


}