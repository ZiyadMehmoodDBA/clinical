Patient_MessageEdit = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Patient_MessageEdit.params = params;

        if (Patient_MessageEdit.bIsFirstLoad) {
            Patient_MessageEdit.bIsFirstLoad = false;

            if (Patient_MessageEdit.params.PanelID != "pnlPatientMessageEdit")
                Patient_MessageEdit.params.PanelID = Patient_MessageEdit.params.PanelID + ' #pnlPatientMessageEdit';

            var Tab = GetTab(Patient_MessageEdit.params["TabID"]);
            if (Tab["PanelID"] != "" && Tab["MasterTabID"] != "") {
                //Patient_MessageEdit.params.PanelID = Tab["Container"] + ' #' + Tab["PanelID"];
                if (Tab["MasterTabID"] == "mstrTabPatient")
                    Patient_MessageEdit.SetDefaultDocument();
                else
                    Patient_MessageEdit.SetDocument(Tab);
            }

            var self = $('#' + Patient_MessageEdit.params.PanelID);
            if (Patient_MessageEdit.params.ParentCtrl != null) {
                if (Patient_MessageEdit.params.ParentCtrl == "Patient_MessageAlert") {
                    self.find("#headingTitle").text("Edit Alert");
                }
                else if (Patient_MessageEdit.params.ParentCtrl == "User_Message") {
                    self.find("#headingTitle").text("Edit User Message");
                }
                else if (Patient_MessageEdit.params.ParentCtrl == "User_Task") {
                    self.find("#headingTitle").text("Edit User Task");
                }
                else {
                    self.find("#headingTitle").text("Edit Message");
                }
            }
            self.loadDropDowns(true).done(function () {
                Patient_MessageEdit.FillMessage(Patient_MessageEdit.params.MessageId);
                setTimeout(function () {
                    //Patient_Message.ShowHideControls('#' + Patient_MessageEdit.params.ActionPanContainer + ' #pnlPatientMessageEdit', '#ddlType')
                }, 100);

                Patient_MessageAdd.ValidatePatientMessage('#' + Patient_MessageEdit.params.PanelID + ' #frmPatientMessageEdit', 'Edit', Patient_MessageEdit.params.ValidateAccountNumber);
  
                //serialize data
                $('#frmPatientMessageEdit').data('serialize', $('#frmPatientMessageEdit').serialize());
            });
        }
    },

    SetDefaultDocument: function () {

        $("#" + Patient_MessageEdit.params["PanelID"] + " #divCommonControls").css("display", "none");
        $("#" + Patient_MessageEdit.params.PanelID + " #hfPatientId").val(Patient_MessageEdit.params.PatientId);

        Patient_MessageEdit.params.ValidateAccountNumber = false;
    },
    SetDocument: function (Tab) {
        $("#" + Patient_MessageEdit.params["PanelID"] + " #divCommonControls").css("display", "inline");

        Patient_MessageEdit.params.ValidateAccountNumber = true;
    },

    DocumentEdit: function (patDocId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientID"] = $("#" + Patient_MessageEdit.params["PanelID"] + " #hfPatientId").val();
                params["PatDocID"] = patDocId;//$('#' + Patient_MessageEdit.params.ActionPanContainer + " #pnlPatientMessageEdit #hfPatDocId").val();
                params["MessageId"] = Patient_MessageEdit.params.MessageId;
                params["mode"] = "Edit";
                params["FromAdmin"] = Patient_Document.params["FromAdmin"];
                params["ParentCtrl"] = 'Patient_MessageEdit';
                LoadActionPan('Document_Viewer', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ActiveInactivePatientDocument: function (PatientDocumentId, IsActive) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = PatientDocumentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Document.UpdateDocumentActiveInactive(Patient_Document.params.patientID, selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_MessageEdit.FillMessage(Patient_MessageEdit.params.MessageId);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                      '3', null, null, null, IsActive
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DocumentDelete: function (DocumentId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = DocumentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Document.DeleteDocument(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#' + Patient_MessageEdit.params["PanelID"] + ' #pnlPatientMessageEdit_Result #dgvPatientMessageEdit').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                Patient_MessageEdit.FillMessage(Patient_MessageEdit.params.MessageId);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DocumentReview: function (patDocId, IsReviewed, IsSigned) {
        var strMessage = "";
        if (IsSigned == null) {
            IsSigned = 0;
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var MessageType = "6";
                if (IsReviewed == "1") {
                    MessageType = "6";
                }
                if (IsSigned == "1") {
                    MessageType = "7";
                }
                utility.myConfirm(MessageType, function () {
                    var selectedValue = patDocId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $('#' + Patient_MessageEdit.params.PanelID);
                        var myJSON = self.getMyJSON();
                        var patientId = $("#" + Patient_MessageEdit.params["PanelID"] + " #hfPatientId").val();
                        Patient_MessageEdit.PatientDocumentUpdate(myJSON, patientId, selectedValue, 1, IsReviewed, IsSigned).done(function (response) {
                            if (response.status != false) {
                                Patient_MessageEdit.FillMessage(Patient_MessageEdit.params.MessageId);
                                utility.DisplayMessages(response.message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    MessageType
                );

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    MessageReplyAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Tasks", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["MessageId"] = Patient_MessageEdit.params.MessageId;
                params["ParentCtrl"] = "Patient_MessageEdit";
                params["AssignedToId"] = $('#' + Patient_MessageEdit.params.PanelID + " #ddlAssignedto option:selected").val(); //AssignedToId;
                params["StatusId"] = $('#' + Patient_MessageEdit.params.PanelID + " #ddlStatus option:selected").val();//StatusId;
                LoadActionPan('Patient_MessageReply', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FillMessage: function (MessageId) {
        Patient_MessageEdit.PatientMessageFill(MessageId).done(function (response) {
            if (response.status != false) {
                var message_detail = JSON.parse(response.MessageFill_JSON);
                var self = $('#' + Patient_MessageEdit.params.PanelID);
                var EditDocument = " <a href='#' onclick=Patient_Document.DocumentEdit('" + message_detail.hfPatientId + "'  ,  '" + message_detail.hfPatDocId + "');   title='Edit Record'>" + message_detail.hfFilePath + "</a>";
                var msgDetail = message_detail.txtMessage.replace(message_detail.hfFilePath, EditDocument);
                utility.bindMyJSON(true, message_detail, false, self);
                if (message_detail.chkVisibleToPatient == 'True')
                    $("#" + Patient_MessageEdit.params.PanelID + " #chkVisibleToPatient").attr("checked", true);
                else
                    $("#" + Patient_MessageEdit.params.PanelID + " #chkVisibleToPatient").attr("checked", false);
                var arrPatDocIds = message_detail.hfPatDocId.split(',');
                var arrPatDocNames = message_detail.hfFilePath.split(',');
                $('#' + Patient_MessageEdit.params.PanelID + " #lblMessage").find("a").each(function () {
                    $(this).remove();
                });
                for (var i = 0; i < arrPatDocIds.length; i++) {
                    var lnkAttachment = "<a id='lnkDoc'" + arrPatDocIds[i] + " href='#' onclick='Patient_MessageEdit.DocumentEdit(" + arrPatDocIds[i] + ");' title='View " + arrPatDocNames[i] + "'>" + arrPatDocNames[i] + "</a> ";
                    $('#' + Patient_MessageEdit.params.PanelID + " #lblMessage").append(lnkAttachment);
                }

                // Disable Assignedto Validation by default
                //Hide Red * near Assignedto
                var objMessageEditForm = $('#' + Patient_MessageEdit.params.PanelID + ' #frmPatientMessageEdit');
                var formValidation = objMessageEditForm.data("bootstrapValidator");
                if (objMessageEditForm.find("#ddlType option:selected").text() == "Task") {
                    formValidation.enableFieldValidators('Assignedto', true);
                    objMessageEditForm.find("#spnAssignedTo").show();
                    if (Patient_MessageEdit.params.FromDashboard == "1") {
                        $('#pnlPatientMessageEdit #divCommonControls #lblPatientName span').hide();
                        formValidation.enableFieldValidators('AccountNumber', false);
                    }
                }

                if (message_detail.hfPatDocId != "")
                    Patient_MessageEdit.LoadMessageAttachment(message_detail.hfPatientId, message_detail.hfPatDocId)
                //if (message_detail.hfFilePath != "") {
                //    $('#' + Patient_MessageEdit.params.ActionPanContainer + " #pnlPatientMessageEdit #lnkAttachment").css("display", "inline");
                //    $('#' + Patient_MessageEdit.params.ActionPanContainer + " #pnlPatientMessageEdit #lnkAttachment").text(message_detail.hfFilePath);
                //}
                //else
                //    $('#' + Patient_MessageEdit.params.ActionPanContainer + " #pnlPatientMessageEdit #lnkAttachment").css("display", "none");

                Patient_MessageEdit.FillMessageReplies(response.ReplyLoad_JSON);
                Patient_MessageEdit.BindAutocomplete();
                if ($("#" + Patient_MessageEdit.params["PanelID"] + " #txtPatientName").val()) {
                    if ($("#" + Patient_MessageEdit.params["PanelID"] + " #lnkPatientNameEdit").css("display") == "none") {
                        $("#" + Patient_MessageEdit.params["PanelID"] + " #lnkPatientNameEdit").css("display", "inline");
                        $("#" + Patient_MessageEdit.params["PanelID"] + " #lblPatientName").css("display", "none");
                    }
                }
                $('#' + Patient_MessageEdit.params.ActionPanContainer + ' #frmPatientMessageEdit').data('serialize', $('#' + Patient_MessageEdit.params.ActionPanContainer + ' #frmPatientMessageEdit').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadMessageAttachment: function (patientId, patDocIds) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Messages", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        if ($("#" + Patient_MessageEdit.params["PanelID"] + " #pnlPatientMessageEdit_Result").css("display") == "none") {
            $("#" + Patient_MessageEdit.params["PanelID"] + " #pnlPatientMessageEdit_Result").show();
        }

        var self = $("#" + Patient_MessageEdit.params["PanelID"]);
        var myJSON = self.getMyJSON();
        Patient_MessageEdit.SearchDocument(null, patientId, patDocIds).done(function (response) {
            if (response.status != false) {
                Patient_MessageEdit.MessageDocGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        //}
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    MessageDocGridLoad: function (response) {
        $("#" + Patient_MessageEdit.params["PanelID"] + " #pnlPatientMessageEdit_Result #dgvPatientMessageEdit").dataTable().fnDestroy();
        $("#" + Patient_MessageEdit.params["PanelID"] + " #pnlPatientMessageEdit_Result #dgvPatientMessageEdit tbody").find("tr").remove();
        if (response.DocumentCount > 0) {
            var DocumentLoad_JSONData = JSON.parse(response.DocumentLoad_JSON);
            $.each(DocumentLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#dgvPatientMessageEdit_row" + item.PatDocId + "'))");
                $row.attr("id", "dgvPatientMessageEdit_row" + item.PatDocId);
                $row.attr("DocumentId", item.PatDocId);
                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }

                var ReviewDocument = "";
                var SignDocument = "";
                if (item.ReviewDate == "") {
                    ReviewDocument = '&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_MessageEdit.DocumentReview(' + item.PatDocId + ', 1,0);" title="Review Record"><i class="fa fa-repeat"></i></a>';
                }
                if (item.SignDate == "") {
                    SignDocument = '&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_MessageEdit.DocumentReview(' + item.PatDocId + ', 0,1);" title="Sign Record"><i class="fa fa-sign-in"></i></a>';
                }
                $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><a class="btn  btn-xs" href="#" onclick="Patient_MessageEdit.DocumentDelete(' + item.PatDocId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_MessageEdit.DocumentEdit(' + item.PatDocId + '   );"   title="Edit Record"><i class="fa fa-edit black"></i></a>' + ReviewDocument + SignDocument + '</td><td>' + item.DocumentName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedBy + '</td><td>' + item.ViewBy + '</td><td>' + item.ReviewBy + '</td><td>' + item.Comments + '</td>');

                $("#" + Patient_MessageEdit.params["PanelID"] + " #pnlPatientMessageEdit_Result #dgvPatientMessageEdit tbody").last().append($row);
            });
        }
        else {
            $("#" + Patient_MessageEdit.params["PanelID"] + " #pnlPatientMessageEdit_Result #dgvPatientMessageEdit").DataTable({
                "language": {
                    "emptyTable": "No Document Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Patient_MessageEdit.params["PanelID"] + " #pnlPatientMessageEdit_Result #dgvPatientMessageEdit"))
            ;
        else
            $("#" + Patient_MessageEdit.params["PanelID"] + " #pnlPatientMessageEdit_Result #dgvPatientMessageEdit").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    FillMessageReplies: function (response) {
        var RepliesText = "";
        var message_Replies = JSON.parse(response);
        var DivReplies = $('#' + Patient_MessageEdit.params.PanelID + " #divReplies");
        if (message_Replies.length == 0) {
            DivReplies.append('<div class="text-center" id=noRecordDiv > No Reply Found </div>');
        }
        else {
            if ($('#' + Patient_MessageEdit.params.PanelID + " #divReplies #noRecordDiv").length > 0) {
                $('#' + Patient_MessageEdit.params.PanelID + " #divReplies #noRecordDiv").remove();
            }
            var divReplyTemplate = $('#' + Patient_MessageEdit.params.PanelID + " #divReplyTemplate").detach();
            DivReplies.empty().append(divReplyTemplate);
            // Show Replies in Descending Order
            for (var i = message_Replies.length - 1; i >= 0 ; i--) {
                var NewReplyDiv = divReplyTemplate.clone();
                var divId = "divReply" + message_Replies[i].MsgrId;
                NewReplyDiv.attr("id", divId);
                NewReplyDiv.css("display", "inline");
                NewReplyDiv.find("h5:first").text(message_Replies[i].CreatedBy);
                NewReplyDiv.find("h6:first").text(message_Replies[i].CreatedOn);
                NewReplyDiv.find("p:first").text(message_Replies[i].MsgDetail);
                DivReplies.append(NewReplyDiv);
                //RepliesText += 'Reply From: ' + message_Replies[i].CreatedBy + '  Date: ' + message_Replies[i].CreatedOn + '<br/>' + message_Replies[i].MsgDetail + '<br/>';
            }
            //$.each(message_Replies, function (i, item) {
            //    RepliesText += 'Reply From: ' + item.CreatedBy + '  Date: ' + item.CreatedOn + '<br/>' + item.MsgDetail + '<br/>';
            //});
            $('#' + Patient_MessageEdit.params.PanelID + " #lblMessageReplies").html(RepliesText);
        }
    },

    UpdatePatientMessage: function () {
        var self = $('#' + Patient_MessageEdit.params.PanelID);
        var myJSON = self.getMyJSON();
        //$('#pnlDemographic').bootstrapValidator('revalidateField', 'DOB');
        if (Patient_MessageEdit.params.mode == "Edit") {
            Patient_MessageEdit.PatientMessageUpdate(myJSON, Patient_MessageEdit.params.MessageId).done(function (response) {
                if (response.status != false) {
                    if (Patient_MessageEdit.params.ParentCtrl == "Patient_MessageAlert") {
                        var patientId = $("#" + Patient_MessageEdit.params["PanelID"] + " #hfPatientId").val();
                        Patient_MessageAlert.MessageAlertSearch(patientId, 2, 2);
                    }
                    else
                        Patient_Message.SearchPatientMessage(Patient_MessageEdit.params.MessageId);
                    utility.DisplayMessages(response.message, 1);
                    //Patient_MessageEdit.UnLoad();
                    //UnloadActionPan(null, 'Patient_MessageEdit');
                    if (Patient_MessageEdit.params != null && Patient_MessageEdit.params.ParentCtrl != null) {
                        // Refresh User Tasks
                        if (Patient_MessageEdit.params.ParentCtrl == "User_Task") {
                            User_Task.SearchUserTask(null, Patient_MessageEdit.params.AssignedToId, "Task", 2, 1, 15);//pageNo=1;Rpp=15
                        }
                        if (Patient_MessageEdit.params.FromDashboard == "1") {
                            DashBoard.DashBoardTasksSearch(null, null, null);
                        }
                        UnloadActionPan(Patient_MessageEdit.params.ParentCtrl, 'Patient_MessageEdit');
                    }
                    else {
                        UnloadActionPan(null, 'Patient_MessageEdit');
                    }
                    updateNotificationsCounts();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    PatientDocumentUpdate: function (PatientDocumentData, PatientID, DocumentID, IsMessage, IsReviewed, IsSigned) {
        if (PatientDocumentData == null) {
            PatientDocumentData = "";
        }
        if (PatientID == null) {
            PatientID = 0;
        }
        if (IsReviewed == null) {
            IsReviewed = 0;
        }
        if (IsMessage == null) {
            IsMessage = 0;
        }
        if (IsSigned == null) {
            IsSigned = 0;
        }
        var data = "PatientDocumentData=" + PatientDocumentData + "&PatientID=" + PatientID + "&DocumentID=" + DocumentID + "&IsReviewed=" + IsReviewed + "&IsMessage=" + IsMessage + "&IsSigned=" + IsSigned;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "UPDATE_PATIENT_DOCUMENT");
    },

    PatientMessageFill: function (MessageId) {
        var data = "MessageId=" + MessageId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_MESSAGE", "FILL_PATIENT_MESSAGE");
    },

    PatientMessageUpdate: function (PatientMessageData, MessageId) {
        var data = "PatientMessageData=" + PatientMessageData + "&PatientID=" + $("#" + Patient_MessageEdit.params["PanelID"] + " #hfPatientId").val() + "&MessageId=" + MessageId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_MESSAGE", "UPDATE_PATIENT_MESSAGE");
    },

    SearchDocument: function (DocumentData, PatientID, PatDocIDs) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "PatientDocumentData=" + DocumentData + "&PatientID=" + PatientID + "&PatDocIds=" + PatDocIDs + "&bFileStream=0";
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "SEARCH_PATIENT_DOCUMENT");
    },

    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_MessageEdit";
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNo,event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#" + Patient_MessageEdit.params["PanelID"] + ' #hfPatientId').val(PatientId);
        var temp = AccountNo.split('-');
        var accountno = temp[0].trim();
        var name = temp[1];
        $("#" + Patient_MessageEdit.params["PanelID"] + ' #txtAccountNumber').val(accountno);
        $("#" + Patient_MessageEdit.params["PanelID"] + " #txtPatientName").val(name);
        if ($("#" + Patient_MessageEdit.params["PanelID"] + " #lnkPatientNameEdit").css("display") == "none") {
            $("#" + Patient_MessageEdit.params["PanelID"] + " #lnkPatientNameEdit").css("display", "inline");
            $("#" + Patient_MessageEdit.params["PanelID"] + " #lblPatientName").css("display", "none");
        }
        UnloadActionPan("Patient_MessageEdit");
        utility.InsertRecentPatient(PatientId);
    },

    ValidateAutoComplete: function (obj) {

        utility.ValidateAutoComplete(obj, Patient_MessageEdit.params["PanelID"] + ' #hfPatientId', false);
    },

    BindAutocomplete: function () {

        var AccountNo = $("#" + Patient_MessageEdit.params["PanelID"] + ' #txtAccountNumber').val();
        var AllPatients = utility.GetPatientArray(AccountNo, 0).done(function (response) {

            $("#" + Patient_MessageEdit.params["PanelID"] + " #txtAccountNumber").autocomplete({
                autoFocus: true,
                //source: AllPatients, // pass an array (without a comma)
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Patient_MessageEdit.params["PanelID"] + " #hfPatientId").val(ui.item.id);
                        $("#" + Patient_MessageEdit.params["PanelID"] + " #txtPatientName").val(ui.item.FullName);
                        if ($("#" + Patient_MessageEdit.params["PanelID"] + " #lnkPatientNameEdit").css("display") == "none") {
                            $("#" + Patient_MessageEdit.params["PanelID"] + " #lnkPatientNameEdit").css("display", "inline");
                            $("#" + Patient_MessageEdit.params["PanelID"] + " #lblPatientName").css("display", "none");
                        }
                        utility.InsertRecentPatient(ui.item.id);
                    }, 100);
                }
            });

          //  $("#" + Patient_MessageEdit.params["PanelID"] + " #txtAccountNumber").autocomplete("search");

        });

    },

    UnLoad: function () {

        utility.UnLoadDialog('frmPatientMessageEdit', function () {
            if (Patient_MessageEdit.params != null && Patient_MessageEdit.params.ParentCtrl != null) {
                UnloadActionPan(Patient_MessageEdit.params.ParentCtrl, 'Patient_MessageEdit');
            }
            else
                UnloadActionPan(null, 'Patient_MessageEdit');
        }, function () {
            if (Patient_MessageEdit.params != null && Patient_MessageEdit.params.ParentCtrl != null) {
                UnloadActionPan(Patient_MessageEdit.params.ParentCtrl, 'Patient_MessageEdit');
            }
            else
                UnloadActionPan(null, 'Patient_MessageEdit');
        });


    },
}
