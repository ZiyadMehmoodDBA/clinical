Clinical_NotesExtraInfo = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_NotesExtraInfo.params = params;
        if (Clinical_NotesExtraInfo.params.PanelID != 'pnlClinicalNotesExtraInfo') {
            Clinical_NotesExtraInfo.params.PanelID = Clinical_NotesExtraInfo.params.PanelID + ' #pnlClinicalNotesExtraInfo';
        } else {
            Clinical_NotesExtraInfo.params.PanelID = 'pnlClinicalNotesExtraInfo';
        }
       

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_NotesExtraInfo.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (Clinical_NotesExtraInfo.params.From && Clinical_NotesExtraInfo.params.From == "TransitionOfCare") {
            Clinical_NotesExtraInfo.HideField();
        }
        else {
            if (Clinical_NotesExtraInfo.params.ParentCtrl == "clinicalTabProgressNote") {
                EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_NotesExtraInfo.params.PanelID, 'Miscellaneous', 'NotesExtraInfo', 'Clinical_NotesExtraInfo.unLoadTab();', 'frmClinicalNotesExtraInfo');
            }
        }
        

        var self = $('#' + Clinical_NotesExtraInfo.params.PanelID);
        if (Clinical_NotesExtraInfo.bIsFirstLoad == true) {
            self.loadDropDowns(true).done(function () {
                Clinical_NotesExtraInfo.LoadNotesExtraInfo();
            });
        }

    },
    HideField: function () {
        
        $("#" + Clinical_NotesExtraInfo.params.PanelID + " #PageTitle").html("Transition of Care")
        $("#" + Clinical_NotesExtraInfo.params.PanelID + " .ForHide").addClass("hidden");
    },
    LoadNotesExtraInfo: function () {
        Clinical_NotesExtraInfo.LoadNotesExtraInfoDBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.Found != false) {
                    Clinical_NotesExtraInfo.params.mode = "Edit";
                    NotesExtraData = JSON.parse(response.NotesExtraData);
                    var self = $('#' + Clinical_NotesExtraInfo.params.PanelID + " #frmClinicalNotesExtraInfo");
                    utility.bindMyJSONByName(true, NotesExtraData, false, self).done(function () {
                        $('#' + Clinical_NotesExtraInfo.params.PanelID + ' #frmClinicalNotesExtraInfo').data('serialize', $('#' + Clinical_NotesExtraInfo.params.PanelID + ' #frmClinicalNotesExtraInfo').serialize());
                    });

                }
                else {
                    Clinical_NotesExtraInfo.params.mode = "Add";
                    $('#' + Clinical_NotesExtraInfo.params.PanelID + ' #frmClinicalNotesExtraInfo').data('serialize', $('#' + Clinical_NotesExtraInfo.params.PanelID + ' #frmClinicalNotesExtraInfo').serialize());
                }


            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    unLoadTab: function () {

        if (Clinical_NotesExtraInfo.params["FromAdmin"] == "0") {
            if (Clinical_NotesExtraInfo.params != null && Clinical_NotesExtraInfo.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_NotesExtraInfo.params.ParentCtrl, 'Clinical_NotesExtraInfo');
            }
            else
                UnloadActionPan(null, 'Clinical_NotesExtraInfo');
        }
        else {

            RemoveAdminTab();
        }

    },
    detach_NotesExtraInfo: function (ComponentName, IsUpdate, NotesExtraInfoRemove) {
        var Clinical_NotesExtraInfoIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #clinicalMenu_Miscellaneous_NotesExtraInfo').parent().parent().find('section[id*="Cli_NotesExtraInfo_Main"]').map(function () {
            return this.id.replace("Cli_NotesExtraInfo_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .NotesExtraInfoComponent').attr('NoteComponentId');
        if (NotesExtraInfoRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                $.when(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId)).then(function () {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Notes Extra Info']").remove();
                    if (Clinical_ProgressNote.params["TemplateName"]) {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .NotesExtraInfoComponent ').addClass('hidden');
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .NotesExtraInfoComponent ').attr('NoteComponentId', 'NCDummyId');
                    }
                    else
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .NotesExtraInfoComponent ').remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            } else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Notes Extra Info']").remove();
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .NotesExtraInfoComponent ').addClass('hidden');
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .NotesExtraInfoComponent ').attr('NoteComponentId', 'NCDummyId');
                }
                else
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .NotesExtraInfoComponent ').remove();

            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #clinicalMenu_Miscellaneous_NotesExtraInfo').parent().parent().find('section[id*="Cli_NotesExtraInfo_Main"]').remove();
        }
        if (Clinical_NotesExtraInfoIds == "" || Clinical_NotesExtraInfoIds == "undefined") {
            $.when(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId)).then(function () {
                utility.DisplayMessages('Successfully Deleted', 1);
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            });
        }
        else {
            Clinical_Cognitive.detachNotesExtraInfo_DBCall(Clinical_NotesExtraInfoIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {

                        Clinical_ProgressNote.saveComponentSOAPText('Notes Extra Info', true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
      
    },
    NotesExtraInfoSave: function () {
        if (EMRUtility.compareFormDataWithSerialized(Clinical_NotesExtraInfo.params.PanelID + ' #frmClinicalNotesExtraInfo')) {
            setTimeout(function () {
                var self = $('#' + Clinical_NotesExtraInfo.params.PanelID + " #frmClinicalNotesExtraInfo");
                var myJSON = self != null ? self.getMyJSONByName() : "{}";

                //myJSON = JSON.stringify(myJSON);
                if (Clinical_NotesExtraInfo.params.mode == "Add") {

                    //AppPrivileges.GetFormPrivileges("Orders_Consultation", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    //if (strMessage == "") {
                    Clinical_NotesExtraInfo.SaveNotesExtraInfoDBCall(myJSON).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_NotesExtraInfo.params.mode = "Edit";
                            utility.DisplayMessages(response.Message, 1);
                            $('#' + Clinical_NotesExtraInfo.params.PanelID + ' #frmClinicalNotesExtraInfo').data('serialize', $('#' + Clinical_NotesExtraInfo.params.PanelID + ' #frmClinicalNotesExtraInfo').serialize());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                    //}
                    //else
                    //  utility.DisplayMessages(strMessage, 2);
                    //});
                }

                if (Clinical_NotesExtraInfo.params.mode == "Edit") {

                    //AppPrivileges.GetFormPrivileges("Orders_Consultation", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    //if (strMessage == "") {
                    Clinical_NotesExtraInfo.UpdateNotesExtraInfoDBCall(myJSON).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            $('#' + Clinical_NotesExtraInfo.params.PanelID + ' #frmClinicalNotesExtraInfo').data('serialize', $('#' + Clinical_NotesExtraInfo.params.PanelID + ' #frmClinicalNotesExtraInfo').serialize());

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                    //}
                    //else
                    //  utility.DisplayMessages(strMessage, 2);
                    //});
                }
            }, 5);
        } else {
            utility.DisplayMessages("Please make any changes to save/update Notes Extra Info", 3);
            setTimeout(function () {
                $('#' + Clinical_NotesExtraInfo.params.PanelID + ' #frmClinicalNotesExtraInfo').data('serialize', $('#' + Clinical_NotesExtraInfo.params.PanelID + ' #frmClinicalNotesExtraInfo').serialize());
            }, 100);
        }
    },
    SaveNotesExtraInfoDBCall: function (JsonData) {
        var objData = JSON.parse(JsonData);
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["NoteId"] = Clinical_NotesExtraInfo.params.NotesId;
        objData["commandType"] = "save_NotesExtraInfo";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "NotesExtraInfo");
    },
    UpdateNotesExtraInfoDBCall: function (JsonData) {
        var objData = JSON.parse(JsonData);
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["NoteId"] = Clinical_NotesExtraInfo.params.NotesId;
        objData["commandType"] = "update_NotesExtraInfo";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "NotesExtraInfo");
    },

    LoadNotesExtraInfoDBCall: function () {
        var objData = {};
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["NoteId"] = Clinical_NotesExtraInfo.params.NotesId;
        objData["commandType"] = "search_NotesExtraInfo";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "NotesExtraInfo");
    },
    ResetNotesExtraInfo: function () {
        $('#' + Clinical_NotesExtraInfo.params.PanelID + " #frmClinicalNotesExtraInfo").find('select').each(function () {
            $(this).val('');
        });
    },
}