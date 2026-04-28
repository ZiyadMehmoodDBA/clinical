Patient_MessageAdd = {
    bIsFirstLoad: true,
    panelID: "",
    params: [],

    Load: function (params) {
        Patient_MessageAdd.params = params;

        if (Patient_MessageAdd.bIsFirstLoad) {
            Patient_MessageAdd.bIsFirstLoad = false;

            if (Patient_MessageAdd.params.PanelID != "pnlPatientMessageAdd")
                Patient_MessageAdd.params.PanelID = '#' + Patient_MessageAdd.params.PanelID + ' #pnlPatientMessageAdd';

            var Tab = GetTab(Patient_MessageAdd.params["TabID"]);
            if (Tab["PanelID"] != "" && Tab["MasterTabID"] != "") {

                if (Tab["MasterTabID"] == "mstrTabPatient")
                    Patient_MessageAdd.SetDefaultDocument();
                else
                    Patient_MessageAdd.SetDocument(Tab);

            }
            // Patient_MessageAdd.panelID = '#' + Patient_MessageAdd.params.ActionPanContainer + ' #pnlPatientMessageAdd';
            var self = $(Patient_MessageAdd.params.PanelID);

            if (Patient_MessageAdd.params.ParentCtrl != null) {
                if (Patient_MessageAdd.params.ParentCtrl == "User_Message") {
                    self.find("#headingTitle").text("New User Message");
                } else if (Patient_MessageAdd.params.ParentCtrl == "User_Task") {
                    self.find("#headingTitle").text("New User Task");
                } else if (Patient_MessageAdd.params.ParentCtrl == 'Patient_MessageCompose' || Patient_MessageAdd.params.ParentCtrl == 'Patient_MessageCreate') {
                    self.find("#headingTitle").text("New Task");
                    self.find("#hfUserMessagesId").val(Patient_MessageAdd.params.UserMessageId);
                    if (Patient_MessageAdd.params.PatientName) {
                        self.find("#txtPatientName").val(Patient_MessageAdd.params.PatientName);
                        self.find("#txtPatientName").prop("disabled", false);
                    }
                    else {
                        self.find("#txtPatientName").prop("disabled", false);
                    }
                    self.find("#hfPatientId").val(Patient_MessageAdd.params.PatientId);
                    self.find("#txtMessage").val('\n\n\n\t' + "----Forward Message----" + '\n' + '"' + Patient_MessageAdd.params.MessageDetail + '"');
                } else {
                    self.find("#headingTitle").text("New Message");
                }
            } else if (Patient_MessageAdd.params.RefCtrl != null && Patient_MessageAdd.params.RefCtrl == "User_Task") {
                self.find("#headingTitle").text("New User Task");
            }
            self.loadDropDowns(true).done(function () {


                Patient_MessageAdd.BindPatientAccount();
                if (Patient_MessageAdd.params.AssignedToId != null) {
                    $(Patient_MessageAdd.params.PanelID + " #txtPatientName").removeAttr("disabled");
                    $(Patient_MessageAdd.params.PanelID + " #divSearchPatient").css("display", "");

                    if ($(Patient_MessageAdd.params.PanelID + " #ddlAssignedto option[value=" + Patient_MessageAdd.params.AssignedToId + "]").length > 0)
                        $(Patient_MessageAdd.params.PanelID + " #ddlAssignedto").val(Patient_MessageAdd.params.AssignedToId);

                    //$(Patient_MessageAdd.panelID + ' #frmPatientMessageAdd').data('bootstrapValidator').enableFieldValidators('PatientName', true);
                } else {
                    var patFullName = $('#PatientProfile #hfPatientFullName').val();
                    $(Patient_MessageAdd.params.PanelID + " #txtPatientName").val(patFullName);
                    //$(Patient_MessageAdd.params.PanelID + " #hfPatientId").val(Patient_MessageAdd.params.PatientId);
                }
                if (Patient_MessageAdd.params.UnloadRef == "UnloadWindows") {
                    $(Patient_MessageAdd.params.PanelID + " #txtPatientName").val(Patient_MessageAdd.params.PatientName);
                    $(Patient_MessageAdd.params.PanelID + " #hfPatientId").val(Patient_MessageAdd.params.PatientId);
                }
                $(Patient_MessageAdd.params.PanelID + ' #ddlStatus option').filter(function () { return $(this).html() == "Unresolved"; }).prop("selected", true);
                Patient_Message.ShowHideControls(Patient_MessageAdd.params.PanelID, '#ddlType')
                Patient_MessageAdd.ValidatePatientMessage(Patient_MessageAdd.params.PanelID + ' #frmPatientMessageAdd', 'Add');
                if (Patient_MessageAdd.params.AssignedToId != null || Patient_MessageAdd.params.UnloadRef == "UnloadWindows") {
                    $(Patient_MessageAdd.params.PanelID + ' #frmPatientMessageAdd').data('bootstrapValidator').enableFieldValidators('PatientName', true);
                }
                //By Default Assigned To is optional
                var objMessageAddForm = self.find('#frmPatientMessageAdd');
                var formValidation = objMessageAddForm.data("bootstrapValidator");
                formValidation.enableFieldValidators('Assignedto', false);
                objMessageAddForm.find("#spnAssignedTo").hide();
                if ((Patient_MessageAdd.params.ParentCtrl != null && Patient_MessageAdd.params.ParentCtrl == "User_Task") || (Patient_MessageAdd.params.RefCtrl && Patient_MessageAdd.params.RefCtrl == "User_Task")) {

                    formValidation.enableFieldValidators('Assignedto', true);
                    objMessageAddForm.find("#spnAssignedTo").show();
                }
                utility.callbackAfterAllDOMLoaded(function () {
                    objMessageAddForm.find("#ddlType option").each(function () {
                        if ($(this).text() == "Task") {
                            $(this).attr("selected", "selected");
                            formValidation.enableFieldValidators('PatientName', false);
                            objMessageAddForm.find("#spanPatientName").hide();

                        }
                    });
                    Patient_Message.ShowHideControls('#pnlPatientMessageAdd', '#ddlType');
                    
                });
                // For Set the Other Option BY DEFAULT
                $(Patient_MessageAdd.params.PanelID + ' #ddlType option').filter(function () { return $(this).html() == "Other"; }).prop("selected", true);
                Patient_Message.ShowHideControls(Patient_MessageAdd.params.PanelID, '#ddlType');
                if ($("#PatientProfile #banner_PatientName").length > 0) {
                    $(Patient_MessageAdd.params.PanelID + " #txtPatientName").val(localStorage.SelectedAccountNumber + " -" + $("#PatientProfile #banner_PatientName").text().trim());
                    $(Patient_MessageAdd.params.PanelID + " #hfPatientId").val(localStorage.SelectedPatientId);
                }
                utility.SetKendoAutoCompleteSourceforValidate($(Patient_MessageAdd.params.PanelID + " #txtPatientName"), $(Patient_MessageAdd.params.PanelID + " #txtPatientName").val(), $(Patient_MessageAdd.params.PanelID + " #hfPatientId"), $(Patient_MessageAdd.params.PanelID + " #hfPatientId").val());
                //serialize data
                $(Patient_MessageAdd.params.PanelID + ' #frmPatientMessageAdd').data('serialize', $(Patient_MessageAdd.params.PanelID + ' #frmPatientMessageAdd').serialize());
                //END
            });
        }
    },

    SetDefaultDocument: function () {

        $(Patient_MessageAdd.params["PanelID"] + " #divCommonControls").css("display", "none");
        $(Patient_MessageAdd.params.PanelID + " #hfPatientId").val(Patient_MessageAdd.params.PatientId);

        Patient_MessageAdd.params.ValidateAccountNumber = false;
    },

    SetDocument: function (Tab) {
        $(Patient_MessageAdd.params["PanelID"] + " #divCommonControls").css("display", "inline");

        Patient_MessageAdd.params.ValidateAccountNumber = true;
    },

    ResetHiddenValue: function () {
        if (Patient_MessageAdd.params.AssignedToId != null) {
            $(Patient_MessageAdd.params.PanelID + " #hfPatientId").val("-1");
        }
    },

    BindPatientAccount: function () {
        var Ctrl = $(Patient_MessageAdd.params.PanelID + " #txtPatientName");
        var hfCtrl = $(Patient_MessageAdd.params.PanelID + " #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        var onChange = function (valid) { Patient_MessageAdd.ResetHiddenValue(); };
        var func = function () {
            var AccountNo = Ctrl.val();
            if (AccountNo != "" && AccountNo.indexOf('-') >= 0) {
                var temp = AccountNo.split('-');
                AccountNo = temp[0];
            }
            return utility.GetPatientArray(AccountNo, 1, true);
        }
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect);
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_MessageAdd';
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $(Patient_MessageAdd.params.PanelID + " #hfPatientId").val(PatientId);
        $(Patient_MessageAdd.params.PanelID + " #txtPatientName").val(patFullName);
        appointmentDetail.FillPatientAccount(PatientId);
        //item.AccountNumber + " - " + item.FullName
        UnloadActionPan("Patient_MessageAdd");

        utility.RevalidateControl($(Patient_MessageAdd.params.PanelID + " #txtPatientName"));
        utility.InsertRecentPatient(PatientId);
        utility.SetKendoAutoCompleteSourceforValidate($(Patient_MessageAdd.params.PanelID + " #txtPatientName"), $(Patient_MessageAdd.params.PanelID + " #txtPatientName").val(), $(Patient_MessageAdd.params.PanelID + " #hfPatientId"), $(Patient_MessageAdd.params.PanelID + " #hfPatientId").val());
    },

    ValidatePatientMessage: function (control, actionType, ValidateAccountNumber) {

        if (ValidateAccountNumber)
            Patient_MessageAdd.params.ValidateAccountNumber = ValidateAccountNumber;

        $(control)
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    PatientName: {
                        group: '.col-sm-3',
                        enabled: false,
                        validators: {
                            notEmpty: {
                                message: ''
                            },
                        }
                    },
                    msgDetail: {
                        group: '.col-xs-12',
                        validators: {
                            notEmpty: {
                                message: ''
                            },
                        }
                    },
                    Assignedto: {
                        group: '.col-sm-3',
                        enabled: false,
                        validators: {
                            notEmpty: {
                                message: ''
                            },
                        }
                    },
                    AlertType: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            },
                        }
                    },
                    AccountNumber: {
                        group: '.col-sm-3',
                        enabled: Patient_MessageAdd.params.ValidateAccountNumber,
                        validators: {
                            notEmpty: {
                                message: ''
                            },
                        }
                    },
                    Type: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            },
                        }
                    },
                },



            })
            .on('success.form.bv', function (e) {
                e.preventDefault();
                if (actionType == "Add") {
                    Patient_MessageAdd.SavePatientMessage();
                } else if (actionType == "Edit") {
                    Patient_MessageEdit.UpdatePatientMessage();
                } else if (actionType == "Reply") {
                    Patient_MessageReply.SaveMessageReply();
                }
            });
    },

    SavePatientMessage: function () {
        var MessageType = $(Patient_MessageAdd.params.PanelID + " #ddlType option:selected").text();
        //if (MessageType != "Task") {//(MessageType == "Amendment" || MessageType == "Reminder") {
        //    if ($(Patient_MessageAdd.panelID + " #hfPatientId").val() == "-1") {
        //        utility.DisplayMessages("Patient not Valid", 2);
        //        return false;
        //    }
        //}
        //else
        //    $(Patient_MessageAdd.panelID + " #hfPatientId").val("");
        var self = $(Patient_MessageAdd.params.PanelID);
        var myJSON = self.getMyJSON();
        //$('#pnlDemographic').bootstrapValidator('revalidateField', 'DOB');
        if (Patient_MessageAdd.params.mode == "Add") {
            Patient_MessageAdd.PatientMessageSave(myJSON).done(function (response) {
                if (response.status != false) {
                    Patient_MessageAdd.params.MessageId = response.MessageId;
                    utility.DisplayMessages(response.message, 1);
                    if (Patient_MessageAdd.params.ParentCtrl == "User_Message") {
                        Patient_Message.SearchPatientMessage(Patient_MessageAdd.params.MessageId, 1, Patient_MessageAdd.params.AssignedToId);
                    } else if ((Patient_MessageAdd.params.ParentCtrl != null && Patient_MessageAdd.params.ParentCtrl == "User_Task") || (Patient_MessageAdd.params.RefCtrl && Patient_MessageAdd.params.RefCtrl == "User_Task")) {
                        User_Task.SearchUserTask(null, Patient_MessageAdd.params.AssignedToId, "Task", 2);
                    } else
                        Patient_Message.SearchPatientMessage(Patient_MessageAdd.params.MessageId);

                    if ($(Patient_MessageAdd.params.PanelID + ' #ddlAssignedto option:selected').text().toLowerCase() == globalAppdata['AppUserName'].toLowerCase()) {
                        if (MessageType != "Task") {
                            var newTotal = Number($("#spnMessageCount").text()) + 1;
                            $("#spnMessageCount").text(newTotal);
                        } else if (MessageType == "Task") {
                            var newTotal = Number($("#spnUserTasksCount").text()) + 1;
                            $("#spnUserTasksCount").text(newTotal);
                        }
                    }
                    //Patient_MessageAdd.UnLoad();
                    updateNotificationsCounts();
                    UnloadActionPan(Patient_MessageAdd.params.ParentCtrl, 'Patient_MessageAdd');
                    if (Patient_MessageAdd.params.UnloadRef == 'UnloadWindows') {
                        setTimeout(function () {
                            if (Patient_MessageAdd.params.FromPatModule == '1') {
                                UnloadActionPan("patTabUserMessages", 'pnlPatientMessageCreate');
                            } else if (Patient_MessageAdd.params.FromQuicklink == '1') {
                                UnloadActionPan("Patient_UserMessagesQuickLink", 'pnlPatientMessageCreate');
                            }
                            else {
                                UnloadActionPan("mstrTabDashBoard", 'pnlPatientMessageCreate');
                            }
                        }, 800);
                    }
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    //Begin Jan 6th, 2015, Author: Abdur Rehman Latif, PMS-3260
    AlertTypeSelection: function () {
        $($('#ddlAlertType option')[0]).prop('selected', true)

    },
    //End Jan 6th, 2015, Author: Abdur Rehman Latif, PMS-3260
    PatientMessageSave: function (PatientMessageData) {
        var data = "PatientMessageData=" + PatientMessageData + "&PatientID=" + $(Patient_MessageAdd.params.PanelID + " #hfPatientId").val();
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_MESSAGE", "SAVE_PATIENT_MESSAGE");
    },

    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_MessageAdd";
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNo) {

        $(Patient_MessageAdd.params["PanelID"] + ' #hfPatientId').val(PatientId);
        var temp = AccountNo.split('-');
        var accountno = temp[0].trim();
        $(Patient_MessageAdd.params["PanelID"] + ' #txtAccountNumber').val(accountno);
        $(Patient_MessageAdd.params["PanelID"] + " #txtPatientName").val(AccountNo);
        UnloadActionPan("Patient_MessageAdd");
        utility.RevalidateControl($(Patient_MessageAdd.params.PanelID + " #txtPatientName"));
        utility.InsertRecentPatient(PatientId);
        utility.SetKendoAutoCompleteSourceforValidate($(Patient_MessageAdd.params.PanelID + " #txtPatientName"), $(Patient_MessageAdd.params.PanelID + " #txtPatientName").val(), $(Patient_MessageAdd.params.PanelID + " #hfPatientId"), $(Patient_MessageAdd.params.PanelID + " #hfPatientId").val());
    },

    ValidateAutoComplete: function (obj) {

        utility.ValidateAutoComplete(obj, Patient_MessageAdd.params["PanelID"].replace('#', '') + ' #hfPatientId', false);
    },

    BindAutocomplete: function () {

        var AccountNo = $(Patient_MessageAdd.params["PanelID"] + ' #txtAccountNumber').val();
        var AllPatients = utility.GetPatientArray(AccountNo, 0).done(function (response) {

            $(Patient_MessageAdd.params["PanelID"] + " #txtAccountNumber").autocomplete({
                autoFocus: true,
                //source: AllPatients, // pass an array (without a comma)
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {
                        $(Patient_MessageAdd.params["PanelID"] + " #hfPatientId").val(ui.item.id);
                        $(Patient_MessageAdd.params["PanelID"] + " #txtPatientName").val(ui.item.FullName);
                        utility.InsertRecentPatient(ui.item.id);
                    }, 100);
                }
            });

            $(Patient_MessageAdd.params["PanelID"] + " #txtAccountNumber").autocomplete("search");

        });

    },

    UnLoad: function () {

        var dfd = new $.Deferred();
        utility.UnLoadDialog(Patient_MessageAdd.params.PanelID + ' #frmPatientMessageAdd', function () {
            if (Patient_MessageAdd.params != null && Patient_MessageAdd.params.ParentCtrl != null) {
                UnloadActionPan(Patient_MessageAdd.params.ParentCtrl, 'Patient_MessageAdd');
                dfd.resolve("ok");
            } else {
                UnloadActionPan(null, 'Patient_MessageAdd');
                dfd.resolve("ok");
            }

        }, function () {
            if (Patient_MessageAdd.params != null && Patient_MessageAdd.params.ParentCtrl != null) {
                UnloadActionPan(Patient_MessageAdd.params.ParentCtrl, 'Patient_MessageAdd');
                dfd.resolve("ok");
            } else {
                UnloadActionPan(null, 'Patient_MessageAdd');
                dfd.resolve("ok");
            }
        });
        dfd.then(function () {
            if (Patient_MessageAdd.params.UnloadRef == 'UnloadWindows') {
                setTimeout(function () {
                    if (Patient_MessageAdd.params.FromQuicklink == '1') {
                        UnloadActionPan("Patient_UserMessagesQuickLink", 'pnlPatientMessageCreate');
                    } else {
                        UnloadActionPan("mstrTabDashBoard", 'pnlPatientMessageCreate');
                    }

                }, 800);
            }
        });

    },

}
