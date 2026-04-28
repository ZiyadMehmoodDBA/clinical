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
                } else {
                    self.find("#headingTitle").text("New Message");
                }
            } else if (Patient_MessageAdd.params.RefCtrl != null && Patient_MessageAdd.params.RefCtrl == "User_Task") {
                self.find("#headingTitle").text("New User Task");
            }
            self.loadDropDowns(true).done(function () {


                if (Patient_MessageAdd.params.AssignedToId != null) {
                    $(Patient_MessageAdd.params.PanelID + " #txtPatientName").removeAttr("disabled");
                    $(Patient_MessageAdd.params.PanelID + " #divSearchPatient").css("display", "");
                    Patient_MessageAdd.BindPatientAccount();
                    $(Patient_MessageAdd.params.PanelID + " #ddlAssignedto").val(Patient_MessageAdd.params.AssignedToId);
                    //$(Patient_MessageAdd.panelID + ' #frmPatientMessageAdd').data('bootstrapValidator').enableFieldValidators('PatientName', true);
                } else {
                    var patFullName = $('#PatientProfile #hfPatientFullName').val();
                    $(Patient_MessageAdd.params.PanelID + " #txtPatientName").val(patFullName);
                    //$(Patient_MessageAdd.params.PanelID + " #hfPatientId").val(Patient_MessageAdd.params.PatientId);
                }

                $(Patient_MessageAdd.params.PanelID + ' #ddlStatus option').filter(function () { return $(this).html() == "Unresolved"; }).prop("selected", true);
                Patient_Message.ShowHideControls(Patient_MessageAdd.params.PanelID, '#ddlType')
                Patient_MessageAdd.ValidatePatientMessage(Patient_MessageAdd.params.PanelID + ' #frmPatientMessageAdd', 'Add');
                if (Patient_MessageAdd.params.AssignedToId != null) {
                    $(Patient_MessageAdd.params.PanelID + ' #frmPatientMessageAdd').data('bootstrapValidator').enableFieldValidators('PatientName', true);
                }
                //By Default Assigned To is optional
                var objMessageAddForm = self.find('#frmPatientMessageAdd');
                var formValidation = objMessageAddForm.data("bootstrapValidator");
                formValidation.enableFieldValidators('Assignedto', false);
                objMessageAddForm.find("#spnAssignedTo").hide();
                if ((Patient_MessageAdd.params.ParentCtrl != null && Patient_MessageAdd.params.ParentCtrl == "User_Task") || (Patient_MessageAdd.params.RefCtrl && Patient_MessageAdd.params.RefCtrl == "User_Task")) {
                    objMessageAddForm.find("#ddlType option").each(function () {
                        if ($(this).text() == "Task") {
                            $(this).attr("selected", "selected");
                            formValidation.enableFieldValidators('PatientName', false);
                            objMessageAddForm.find("#spanPatientName").hide();

                        }
                    });
                    formValidation.enableFieldValidators('Assignedto', true);
                    objMessageAddForm.find("#spnAssignedTo").show();
                }

                // For Set the Other Option BY DEFAULT
                $(Patient_MessageAdd.params.PanelID + ' #ddlType option').filter(function () { return $(this).html() == "Other"; }).prop("selected", true);
                Patient_Message.ShowHideControls(Patient_MessageAdd.params.PanelID, '#ddlType');

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
        var AccountNo = $(Patient_MessageAdd.params.PanelID + ' #txtPatientName').val();
        if (AccountNo != "" && AccountNo.indexOf('-')>=0) {
            var temp = AccountNo.split('-');
            AccountNo = temp[0];
        }
        

        var AllPatients = utility.GetPatientArray(AccountNo, 1,true);
        $(Patient_MessageAdd.params.PanelID + " #txtPatientName").autocomplete({
            //source: AllPatients, // pass an array (without a comma)
            source: AllPatients,

            select: function (event, ui) {

                //$("#appointmentDetail #txtAccountNo").val(ui.item.id); // add the selected id
                //$("#appointmentDetail #txtFullName").val(ui.item.patientName);
                setTimeout(function () {
                    $(Patient_MessageAdd.params.PanelID + " #hfPatientId").val(ui.item.id);
                }, 100);

                //$("#hfpatientid").val(ui.item.id);
            }
        });
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_MessageAdd';
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, patFullName) {
        $(Patient_MessageAdd.params.PanelID + " #hfPatientId").val(PatientId);
        $(Patient_MessageAdd.params.PanelID + " #txtPatientName").val(patFullName);
        appointmentDetail.FillPatientAccount(PatientId);
        //item.AccountNumber + " - " + item.FullName
        UnloadActionPan("Patient_MessageAdd");
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
                        group: '.col-sm-2',
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
                        group: '.col-sm-2',
                        enabled: false,
                        validators: {
                            notEmpty: {
                                message: ''
                            },
                        }
                    },
                    AlertType: {
                        group: '.col-sm-2',
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
                }
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
        var strMessage = "";
        var self = $(Patient_MessageAdd.params.PanelID);
        var myJSON = self.getMyJSON();
        //$('#pnlDemographic').bootstrapValidator('revalidateField', 'DOB');
        if (Patient_MessageAdd.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Messages", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
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

                            UnloadActionPan(Patient_MessageAdd.params.ParentCtrl, 'Patient_MessageAdd');
                        } else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                } else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

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
    },

    ValidateAutoComplete: function (obj) {

        utility.ValidateAutoComplete(obj, Patient_MessageAdd.params["PanelID"].replace('#', '') + ' #hfPatientId', false);
    },

    BindAutocomplete: function () {

        var AccountNo = $(Patient_MessageAdd.params["PanelID"] + ' #txtAccountNumber').val();
        var AllPatients = utility.GetPatientArray(AccountNo, 0);
        $(Patient_MessageAdd.params["PanelID"] + " #txtAccountNumber").autocomplete({
            //source: AllPatients, // pass an array (without a comma)
            source: AllPatients,
            select: function (event, ui) {
                setTimeout(function () {
                    $(Patient_MessageAdd.params["PanelID"] + " #hfPatientId").val(ui.item.id);
                    $(Patient_MessageAdd.params["PanelID"] + " #txtPatientName").val(ui.item.FullName);
                }, 100);
            }
        });
    },

    UnLoad: function () {

        if ($(Patient_MessageAdd.params.PanelID + ' #frmPatientMessageAdd').serialize() != $(Patient_MessageAdd.params.PanelID + ' #frmPatientMessageAdd').data('serialize')) {
            utility.myConfirm('2', function () {

                if (Patient_MessageAdd.params != null && Patient_MessageAdd.params.ParentCtrl != null) {
                    UnloadActionPan(Patient_MessageAdd.params.ParentCtrl, 'Patient_MessageAdd');
                } else
                    UnloadActionPan(null, 'Patient_MessageAdd');

            }, function () { });
        } else {
            if (Patient_MessageAdd.params != null && Patient_MessageAdd.params.ParentCtrl != null) {
                UnloadActionPan(Patient_MessageAdd.params.ParentCtrl, 'Patient_MessageAdd');
            } else
                UnloadActionPan(null, 'Patient_MessageAdd');
        }

    }
}