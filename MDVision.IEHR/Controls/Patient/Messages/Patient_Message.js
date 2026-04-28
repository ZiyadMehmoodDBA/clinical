Patient_Message = {
    bIsFirstLoad: true,
    params: [],
    GridPatientMessages: "dgvPatientMessage",

    Load: function (paramerters) {

        Patient_Message.params = paramerters;

        if (Patient_Message.params["PanelID"] != 'pnlPatientMessage')
            Patient_Message.params["PanelID"] = Patient_Message.params["PanelID"] + ' #pnlPatientMessage';

        var Tab = GetTab(Patient_Message.params["TabID"]);
        if (Tab["PanelID"] != "" && Tab["MasterTabID"] != "") {

            Patient_Message.params["PanelID"] = Tab["Container"] + ' #' + Tab["PanelID"];

            if (Tab["MasterTabID"] == "mstrTabPatient")
                Patient_Message.SetDefaultDocument();
            else
                Patient_Message.SetDocument(Tab);

        }

        if (Patient_Message.params.IsUserMessages != null) {
            $('#' + Patient_Message.params["PanelID"] + " #frmPatientMessage").css("display", "none");
            $('#' + Patient_Message.params["PanelID"] + " #headingTitle").text("User Messages");
        }
        else {
            var self = $('#' + Patient_Message.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
                Patient_Message.SearchPatientMessage();
            });
        }

        //Patient_Demographic.FillPatientInfo(Patient_Demographic.params);

        utility.CreateDatePicker(Patient_Message.params["PanelID"] + ' #dtpCalledDate, #dtpEntryDate', function () {
            //  on-change callback method
        });

        //if (Patient_Message.bIsFirstLoad) {
        //    Patient_Message.bIsFirstLoad = false;
        //    if (Patient_Message.params.IsUserMessages != null) {
        //        $('#' + Patient_Message.params["PanelID"] + " #frmPatientMessage").css("display", "none");
        //        $('#' + Patient_Message.params["PanelID"] + " #headingTitle").text("User Messages");
        //    }
        //    else {
        //        var self = $('#' + Patient_Message.params["PanelID"]);
        //        self.loadDropDowns(true).done(function () {
        //            Patient_Message.SearchPatientMessage();
        //        });
        //    }
        //    Patient_Demographic.FillPatientInfo(Patient_Message.params);
        //}
        if (params.PreviousTab.TabID == "patTabDemographic") {
            Patient_Demographic.isChangeInDemographic(Patient_Demographic.params.mode);
        }
        else if (params.PreviousTab.TabID == "patTabInsurance") {
            Patient_Insurance.isChangeInInsurance(Patient_Insurance.params.mode);
        }
        Patient_Message.BindAutocomplete();

    },

    SetDefaultDocument: function () {

        $('#' + Patient_Message.params["PanelID"] + " #divCommonControls").css("display", "none");
        Patient_Message.params.GridPatientMessages = "dgvPatientMessage";

        $('#' + Patient_Message.params["PanelID"]).find("#dgvPatientMessageBatch").each(function () {
            var id_ = $(this).attr('id');
            $(this).attr('id', id_.replace('Batch', ''));
        });

        $("#" + Patient_Message.params["PanelID"] + " #hfPatientId").val(Patient_Message.params.patientID);
        $("#" + Patient_Message.params["PanelID"] + " #txtAccountNumber").val(Patient_Message.params.PatientAccountNo);

    },

    SetDocument: function (Tab) {

        $("#" + Tab["Container"]).find("#actionPanPatientMessage").attr('id', Tab['ActionPanContainer']);
        $("#" + Tab["Container"]).find("#pnlPatientMessage").attr('id', Tab['PanelID']);
        Patient_Message.params["PanelID"] = Tab['PanelID'];
        $('#' + Patient_Message.params["PanelID"]).css("display", "inline");
        $('#' + Patient_Message.params["PanelID"] + " #formpanelheading").css('display', 'inline');

        $('#' + Patient_Message.params["PanelID"] + " #divCommonControls").css("display", "inline");

        Patient_Message.params.GridPatientMessages = "dgvPatientMessageBatch";

        $('#' + Patient_Message.params["PanelID"]).find("#dgvPatientMessage").each(function () {
            var id_ = $(this).attr('id');
            $(this).attr('id', id_ + "Batch");
        });
    },

    MessageAddEdit: function (MessageId, mode, AssignedToId, StatusId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        if (mode == "Edit" || mode == "Reply") {
            utility.SelectGridRow($("#gvPatientMessage_row" + MessageId));
        }

        AppPrivileges.GetFormPrivileges("Tasks", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["MessageId"] = MessageId;
                params["PatientId"] = $("#" + Patient_Message.params["PanelID"] + " #hfPatientId").val();
                params["mode"] = mode;
                params["ParentCtrl"] = Patient_Message.params["TabID"];
                if (mode == "Add") {
                    LoadActionPan('Patient_MessageAdd', params);
                    //LoadActionPan('Patient_MessageReply', params);
                }
                else if (mode == "Edit") {
                    LoadActionPan('Patient_MessageEdit', params);
                }
                else if (mode == "Reply") {
                    params["AssignedToId"] = AssignedToId;
                    params["StatusId"] = StatusId;
                    LoadActionPan('Patient_MessageReply', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ShowHideControls: function (Panel, ddlType) {
        var formValidation = $("#frmPatientMessageAdd").data("bootstrapValidator");
        if (formValidation != null && formValidation.enableFieldValidators) {
            // Disable AlertType Validation by default
            //Hide Red * near AlertType
            formValidation.enableFieldValidators('AlertType', false);
            $("#pnlPatientMessageAdd #spanAlertType").hide();

            // Disable Assignedto Validation by default
            //Hide Red * near Assignedto
            formValidation.enableFieldValidators('Assignedto', false);
            $(Panel + " #spnAssignedTo").hide();
        }

        var selectedType = $(Panel + " " + ddlType + " option:selected").text();
        switch (selectedType) {

            case "Amendment": {
                formValidation.enableFieldValidators('PatientName', true);
                $("#pnlPatientMessageAdd #spanPatientName").show();
                $(Panel + " #ddlAmendmentSource").prop("disabled", false);
                $(Panel + " #ddlAlertType,#ddlMedication,#ddlPharmacy,#ddlLab,#ddlLabOrder").prop("disabled", true);
            }
                break;
            case "Reminder": {
                formValidation.enableFieldValidators('PatientName', true);
                $("#pnlPatientMessageAdd #spanPatientName").show();
                $(Panel + " #ddlAlertType").prop("disabled", false);
                $(Panel + " #ddlAmendmentSource,#ddlMedication,#ddlPharmacy,#ddlLab,#ddlLabOrder").prop("disabled", true);
                if (formValidation != null && formValidation.enableFieldValidators) {
                    formValidation.enableFieldValidators('AlertType', true);
                    $("#pnlPatientMessageAdd #spanAlertType").show();
                }
            }
                break;

            case "Task": {
                formValidation.enableFieldValidators('PatientName', false);
                $("#pnlPatientMessageAdd #spanPatientName").hide();
                $(Panel + " #ddlAlertType").prop("disabled", true);
                $(Panel + " #ddlAmendmentSource,#ddlMedication,#ddlPharmacy,#ddlLab,#ddlLabOrder").prop("disabled", true);
                if (formValidation != null && formValidation.enableFieldValidators) {
                    formValidation.enableFieldValidators('Assignedto', true);
                    $(Panel + " #spnAssignedTo").show();
                }
            }
                break;
            case "Medication": {
                formValidation.enableFieldValidators('PatientName', true);
                $("#pnlPatientMessageAdd #spanPatientName").show();
                $(Panel + " #ddlMedication,#ddlPharmacy").prop("disabled", false);
                $(Panel + " #ddlAlertType,#ddlAmendmentSource,#ddlLab,#ddlLabOrder").prop("disabled", true);
            }
                break;
            case "Laboratory": {
                formValidation.enableFieldValidators('PatientName', true);
                $("#pnlPatientMessageAdd #spanPatientName").show();
                $(Panel + " #ddlLab,#ddlLabOrder").prop("disabled", false);
                $(Panel + " #ddlAlertType,#ddlAmendmentSource,#ddlMedication,#ddlPharmacy").prop("disabled", true);
            }
                break;
            default: {
                $(Panel + " #ddlAlertType,#ddlAmendmentSource,#ddlMedication,#ddlPharmacy,#ddlLab,#ddlLabOrder").prop("disabled", true);
                //$(Panel + " #ddlAlertType").prop("disabled", true);
            }
                break;

        }
        //if (selectedType == "") {
        //    $(ctrToHide).css("display", "none");
        //}
        //else if ($(ddl + "option:selected").val() == Type && Type=="Amendment") {
        //    //$(ddl)
        //}
    },


    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = Patient_Message.params["TabID"];
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (AccountNo, PatientId,event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ($('#' + Patient_Message.params["PanelID"] + ' #txtAccountNumber').data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#' + Patient_Message.params["PanelID"] + ' #txtAccountNumber'), AccountNo, $('#' + Patient_Message.params["PanelID"] + ' #hfPatientId'), PatientId);
        UnloadActionPan(Patient_Message.params["TabID"]);
        utility.InsertRecentPatient(PatientId);
    },

    BindAutocomplete: function () {
        var Ctrl = $("#" + Patient_Message.params["PanelID"] + " #txtAccountNumber");
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var hfCtrl = $("#" + Patient_Message.params["PanelID"] + " #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "value", "contains", null, func, hfCtrl, onSelect);
    },

    ValidateAutoComplete: function (obj) {

        utility.ValidateAutoComplete(obj, Patient_Message.params["PanelID"] + ' #hfPatientId', false);
    },

    SearchPatientMessage: function (MessageId, IsUserMessages, AssignedToId, PageNo, rpp, msgStatusID) {
                var PatientID = null;
                var AssignedToUserId = null;
                if (IsUserMessages != null) {
                    $("#" + Patient_Message.params["PanelID"] + " #ddlStatus option[text='Unresolved']").attr('selected', true);
                    PatientID = null;
                    //myJSON = null;
                    MessageId = null;
                    AssignedToUserId = AssignedToId;
                }
                else {
                    PatientID = $("#" + Patient_Message.params["PanelID"] + " #hfPatientId").val();
                    //PMS-1737
                    if (PatientID==null || PatientID=="")
                    {
                        PatientID = $('#PatientProfile #hfPatientId').val();
                    }
                    AssignedToUserId = null;
                    //myJSON = null;
                }
                var self = $("#" + Patient_Message.params["PanelID"]);
                var myJSON = self.getMyJSON();
                if (msgStatusID == null) {

                    msgStatusID = self.find('#ddlStatus').val();
                }
                Patient_Message.MessageSearch(myJSON, PatientID, MessageId, AssignedToUserId, PageNo, rpp, msgStatusID).done(function (response) {
                    if (response.status != false) {
                        if ($("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result").css("display") == "none") {
                            $("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result").show();
                        }
                        if (IsUserMessages != null) {
                            if (response.MessageCount > 0) { }
                            else {
                                $("#pnlUserMessage #divUserMsgsPaging").css("display", "none");
                            }
                            User_Message.UserMessageGridLoad(response, PageNo, rpp);
                        }
                        else {
                            if (response.MessageCount > 0) { }
                            else {
                                $("#" + Patient_Message.params["PanelID"] + " #divMessagesPaging").css("display", "none");
                            }
                            Patient_Message.MessageGridLoad(response, PageNo, rpp);
                        }
                    }
                    else {
                        // Immediate Patch
                        if (!(response.Message == "the given key was not present in the dictionary."))
                            utility.DisplayMessages(response.Message, 3);
                    }
                });
    },

    MessageGridLoad: function (response, PageNo, rpp) {
        $("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result #" + Patient_Message.params.GridPatientMessages).dataTable().fnDestroy();
        $("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result #" + Patient_Message.params.GridPatientMessages + " tbody").find("tr").remove();
        if (response.MessageCount > 0) {
            var MessageLoadJSONData = JSON.parse(response.MessageLoad_JSON);
            $.each(MessageLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvPatientMessage_row" + item.PatMsgId);
                $row.attr("MessageId", item.PatMsgId);

                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var EditMethod = "Patient_Message.MessageAddEdit('" + item.PatMsgId.trim() + "','Edit',null,null,event);";
                $row.attr("onclick", EditMethod);
                var ActiveInacvtiveMethod = "Patient_Message.ActiveInactivePatientMessage('" + item.PatMsgId.trim() + "'," + isactive + ",event);";
                var AssignedToId = "";
                var MsgStatusId = "";
                if (item.AssignedToId != null) {
                    AssignedToId = item.AssignedToId;
                }
                if (item.MsgStatusId != null) {
                    MsgStatusId = item.MsgStatusId;
                }
                var AddMessageReplyMethod = "Patient_Message.MessageAddEdit('" + item.PatMsgId.trim() + "','Reply','" + AssignedToId + "','" + MsgStatusId + "',event);";

                $row.append('<td style="display:none;">' + item.PatMsgId + '</td><td><a class="btn btn-xs" href="#" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a><a class="btn  btn-xs" href="#" onclick="' + AddMessageReplyMethod + '" title="Reply"><i class="fa fa-reply"></i></a>&nbsp;</td><td class="size-max200 ellipses" data-toggle="tooltip" data-placement="right" title="' + item.MsgDetail + '">' + item.MsgDetail + '</td><td>' + item.MessageStatus + '</td><td>' + item.MessageType + '</td><td>' + item.AssigneeName + '</td><td>' + item.CreatedBy + '</td><td>' + item.CreatedOn + '</td>');

                $("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result #" + Patient_Message.params.GridPatientMessages + " tbody").last().append($row);
            });

            //----------------- Patient Messages Paging----
            $("#" + Patient_Message.params["PanelID"] + " #divMessagesPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging(Patient_Message.params["PanelID"] + " #divMessagesPaging", response.iTotalDisplayRecords, 5, "Patient_Message", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#" + Patient_Message.params["PanelID"] + " #divMessagesPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#" + Patient_Message.params["PanelID"] + " li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }

        else {
            $("#" + Patient_Message.params["PanelID"] + " #divMessagesPaging").css("display", "none");
            $("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result #" + Patient_Message.params.GridPatientMessages).DataTable({
                "language": {
                    "emptyTable": "No Message Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });

        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });


        if ($.fn.dataTable.isDataTable("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result #" + Patient_Message.params.GridPatientMessages))
            ;
        else
            $("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result #" + Patient_Message.params.GridPatientMessages).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown


       // $("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result #" + Patient_Message.params.GridPatientMessages + " th")[0].click();
    },

    ActiveInactivePatientMessage: function (MessageId, IsActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('3', function () {
            var selectedValue = MessageId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Patient_Message.MessageUpdateActiveInactive(selectedValue, IsActive).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        Patient_Message.SearchPatientMessage(selectedValue);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { }, '3', null, null, null, IsActive);
    },

    DeletePatientMessage: function (MessageId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#gvPatientMessage_row" + MessageId));
        utility.myConfirm('1', function () {
            var selectedValue = MessageId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Patient_Message.MessageDelete(selectedValue).done(function (response) {
                    if (response.status != false) {
                        var table1 = $('#' + Patient_Message.params["PanelID"] + ' #pnlPatientMessage_Result #' + Patient_Message.params.GridPatientMessages).DataTable();
                        table1.row('.active').remove().draw(false);
                        Patient_Message.SearchPatientMessage();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { }, '1');
    },

    MessageSearch: function (PatientMessageData, PatientID, MessageId, AssignedToId, PageNumber, RowsPerPage, msgStatusID) {
        if (PatientMessageData == null) {
            PatientMessageData = "";
        }
        if (PatientID == null) {
            PatientID = 0;
        }
        if (MessageId == null) {
            MessageId = 0;
        }
        if (AssignedToId == null) {
            AssignedToId = 0;
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        if (msgStatusID == null)
            msgStatusID = "2";  //Unresolved
        var data = "PatientMessageData=" + PatientMessageData + "&PatientID=" + PatientID + "&MessageId=" + MessageId + "&AssignedToId=" + AssignedToId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage + "&MsgStatusId=" + msgStatusID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_MESSAGE", "SEARCH_PATIENT_MESSAGE");
    },

    MessageUpdateActiveInactive: function (MessageID, IsActive) {
        var data = "MessageId=" + MessageID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_MESSAGE", "UPDATE_PATIENT_MESSAGE_ACTIVE_INACTIVE");
    },

    MessageDelete: function (MessageID) {
        var data = "MessageId=" + MessageID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_MESSAGE", "DELETE_PATIENT_MESSAGE");
    },

    UnLoad: function () {
        if (Patient_Message.params != null && Patient_Message.params.ParentCtrl != null) {
            UnloadActionPan(Patient_Message.params.ParentCtrl, 'Patient_Message');
        }
        else
            UnloadActionPan(null, 'Patient_Message');
    },

    UnLoadTab: function () {
        if (Patient_Message.params["FromAdmin"] == "0") {
            if (Patient_Message.params != null && Patient_Message.params.ParentCtrl != null) {
                UnloadActionPan(Patient_Message.params.ParentCtrl, 'Patient_Message');
            }
            else
                UnloadActionPan(null, 'Patient_Message');
        }
        else {
            RemoveAdminTab();
        }
    },

    //-----------Pagination Functions--------------
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Patient_Message.SearchPatientMessage(null, null, null, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Patient_Message.SearchPatientMessage(null, null, null, currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#" + Patient_Message.params["PanelID"] + " #pnlPatientMessage_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Patient_Message.SearchPatientMessage(null, null, null, currentPageNo, 15);
        }
    },
    //-----------------------------------------------
    RefreshCount: function () {

        IsBackgroundLoaderShow = false;
        Patient_Message.RefreshMessageAndTaskCount().done(function (response) {
            IsBackgroundLoaderShow = true;
            if (response.status != false) {
                if (parseInt(response.messageCount) > 0) {
                    $('#spnMessageCount').text(response.messageCount);
                }
              //  AST - 406
                //if (parseInt(response.appointmentCount) > 0) {
                //    $('#spnAppCount').text(response.appointmentCount);
                //}
                if (parseInt(response.taskCount) > 0) {
                    $('#spnUserTasksCount').text(response.taskCount);
                }
                if (parseInt(response.pendingDocCount) > 0) {
                    $('#spnPendingDocumentsCount').text(response.pendingDocCount)
                    $('#pnlDashboard div.wDocuments .badge').text(response.pendingDocCount);
                }
                if (parseInt(response.notesCount) > 0) {
                    $('#spnNotesCount').text(response.notesCount);
                    $('#wpanel .slick-track div').each(function (i) {
                        if ($(this).find('span:first').text() == 'Notes') {
                            $(this).find('span:last').text(response.notesCount);
                            $(this).find('span:last').show();
                        }
                    });
                }
                if (parseInt(response.refillPrescriptionCount) > 0) {
                    $('#spnPrescriptionsRefillCount').text(response.refillPrescriptionCount);
                }
                if (parseInt(response.pendingPrescriptionCount) > 0) {
                    $('#spnPendingPrescriptionsCount').text(response.pendingPrescriptionCount);
                }
                // Start 30/11/2015 Muhammad Irfan to refresh span count globally
                //$('#spnAppCount').text(response.appointmentCount);
                //$('#spnNotesCount').text(response.notesCount);
                //// End 30/11/2015 Muhammad Irfan to refresh span count globally
                //$('#spnPrescriptionsRefillCount').text(response.refillPrescriptionCount);
                //$('#spnPendingPrescriptionsCount').text(response.pendingPrescriptionCount);
            }

        });
    },

    RefreshMessageAndTaskCount: function () {
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(null, "PATIENT_MESSAGE", "REFRESH_COUNT");
    },
}
