Patient_MessageAlert = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Patient_MessageAlert.params = params;

        if (Patient_MessageAlert.bIsFirstLoad) {
            Patient_MessageAlert.bIsFirstLoad = false;

            if (Patient_MessageAlert.params.PanelID != "pnlPatientMessageAlert")
                Patient_MessageAlert.params.PanelID = Patient_MessageAlert.params.PanelID + ' #pnlPatientMessageAlert';
            else
                Patient_MessageAlert.params.PanelID = 'pnlPatientMessageAlert';
            //var Tab = GetTab(Patient_MessageAlert.params["TabID"]);
            //if (Tab["PanelID"] != "" && Tab["MasterTabID"] != "") {
            //    Patient_MessageAlert.params.PanelID = Tab["Container"] + ' #' + Tab["PanelID"];

            //}

            $("#" + Patient_MessageAlert.params["PanelID"] + " #hfPatientId").val(Patient_MessageAlert.params.patientID);

            // Ensure it should must contain PatientId even if it is not provided.
            var PatientId = Patient_MessageAlert.params.patientID;
            if (!PatientId || PatientId == '') {
                PatientId = $("#PatientProfile #hfPatientId").val()
            }

            Patient_MessageAlert.MessageAlertSearch(PatientId, 2, 2, 1, 1000);
        }
    },

    MessageAlertSearch: function (PatientId, TypeId, StatusId, PageNumber, RowsPerPage) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Tasks", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                $("#" + Patient_MessageAlert.params.PanelID + " #ddlType").val(TypeId);
                $("#" + Patient_MessageAlert.params.PanelID + " #ddlStatus").val(StatusId);
                var self = $("#" + Patient_MessageAlert.params.PanelID);
                var myJSON = self.getMyJSON();
                if (PageNumber == undefined)
                    PageNumber = 1;
                if (RowsPerPage == undefined)
                    RowsPerPage = 1000;
                Patient_MessageAlert.SearchMessageAlert(myJSON, PatientId, 0, TypeId, StatusId, PageNumber, RowsPerPage).done(function (response) {
                    if (response.status != false) {
                        if (response.MessageCount > 0) {
                            var arrAlertTypes = [];
                            var MessageLoadJSONData = JSON.parse(response.MessageLoad_JSON);
                            $.each(MessageLoadJSONData, function (i, item) {
                                if ($.inArray(item.AlertType, arrAlertTypes) == -1) {
                                    arrAlertTypes.push(item.AlertType);
                                }
                            });
                            var divAlerts = $("#" + Patient_MessageAlert.params.PanelID + " #divPatientAlerts");
                            var divAlertTemplate = $("#pnlPatientMessageEdit #divAlertTemplate");
                            for (var i = 0; i < arrAlertTypes.length; i++) {
                                $("#divAlert" + arrAlertTypes[i]).css("display", "inline");

                                if ($("#" + Patient_MessageAlert.params.PanelID + " #pnl" + arrAlertTypes[i] + "Alert_Result").css("display") == "none") {
                                    $("#" + Patient_MessageAlert.params.PanelID + "  #pnl" + arrAlertTypes[i] + "Alert_Result").show();
                                }
                                Patient_MessageAlert.MessageAlertGridLoad(response, "#divAlert" + arrAlertTypes[i] + " #pnl" + arrAlertTypes[i] + "Alert_Result #dgv" + arrAlertTypes[i] + "Alert", arrAlertTypes[i]);
                                //var NewAlertDiv = divAlertTemplate.clone();
                                //var divId = "divAlert" + arrAlertTypes[i];
                                //NewAlertDiv.attr("id", divId);
                                //NewAlertDiv.css("display", "inline");
                                //NewAlertDiv.find("#lblAlertTemplate").attr("id", "lblAlert" + arrAlertTypes[i]).text("Patient has " + arrAlertTypes[i] + " alert(s)");
                                //NewAlertDiv.find("#pnlAlertTemplate_Result").attr("id", "pnlAlert" + arrAlertTypes[i] + "_Result");
                                //NewAlertDiv.find("#ClinicalAlert_SelectedDataKeys").attr("id", arrAlertTypes[i] + "Alert_SelectedDataKeys");
                                //NewAlertDiv.find("#dgvAlertTemplate").attr("id", "dgv" + arrAlertTypes[i] + "Alert");
                                //divAlerts.append(NewAlertDiv);
                            }
                        }
                        else {
                            Patient_MessageAlert.UnLoad();
                        }
                        //Patient_MessageAlert.MessageAlertGridLoad(response, "#divAlertClinical #pnlClinicalAlert_Result #dgvClinicalAlert", "Clinical");
                        //Patient_MessageAlert.MessageAlertGridLoad(response, "#divAlertFinancial #pnlFinancialAlert_Result #dgvFinancialAlert", "Financial");
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    MessageAddEdit: function (MessageId, mode, AssignedToId, StatusId) {
        var strMessage = "";
        utility.SelectGridRow($("#gvPatientMessage_row" + MessageId));
        AppPrivileges.GetFormPrivileges("Tasks", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["MessageId"] = MessageId;
                params["PatientId"] = $("#" + Patient_MessageAlert.params["PanelID"] + " #hfPatientId").val();
                params["mode"] = mode;
                params["ParentCtrl"] = "Patient_MessageAlert";
                if (mode == "Edit") {
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

    MessageAlertGridLoad: function (response, gridId, AlertType) {
        $("#" + Patient_MessageAlert.params.PanelID + " " + gridId).dataTable().fnDestroy();
        $("#" + Patient_MessageAlert.params.PanelID + " " + gridId + " tbody").find("tr").remove();
        if (response.MessageCount > 0) {
            var MessageLoadJSONData = JSON.parse(response.MessageLoad_JSON);
            $.each(MessageLoadJSONData, function (i, item) {
                if (item.AlertType == AlertType) {
                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#gvPatientMessage_row" + item.PatMsgId + "'))");
                    $row.attr("id", "gvPatientMessage_row" + item.PatMsgId);
                    $row.attr("MessageId", item.PatMsgId);

                    if (item.IsActive == "True") {
                        isactive = 0;
                        tglclass = "fa fa-toggle-on green";
                    }
                    else {
                        isactive = 1;
                        tglclass = "fa fa-toggle-on red";
                    }

                    var EditMethod = "Patient_MessageAlert.MessageAddEdit('" + item.PatMsgId.trim() + "','Edit');";
                    var ActiveInacvtiveMethod = "Patient_Message.ActiveInactivePatientMessage('" + item.PatMsgId.trim() + "','" + isactive + "');";
                    var AddMessageReplyMethod = "Patient_MessageAlert.MessageAddEdit('" + item.PatMsgId.trim() + "','Reply','" + item.AssignedToId + "','" + item.MsgStatusId + "');";

                    $row.append('<td style="display:none;">' + item.PatMsgId + '</td><td><a class="btn btn-xs" href="#" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + AddMessageReplyMethod + '" title="Reply"><i class="fa fa-reply"></i></a>&nbsp;</td><td>' + item.MsgDetail + '</td><td>' + item.MessageStatus + '</td><td>' + item.EntryDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.UserFullName + '</td>');

                    $("#" + Patient_MessageAlert.params.PanelID + " " + gridId + " tbody").last().append($row);
                }

            });
        }
        else {
            $("#" + Patient_MessageAlert.params.PanelID + " " + gridId).DataTable({
                "language": {
                    "emptyTable": "No " + AlertType + " Alert Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Patient_MessageAlert.params.PanelID + " " + gridId))
            ;
        else
            $("#" + Patient_MessageAlert.params.PanelID + " " + gridId).DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchMessageAlert: function (PatientMessageData, PatientId, MessageId, MsgTypeId, MsgStatusId, PageNumber, RowsPerPage) {
        if (MessageId == null) {
            MessageId = 0;
        }
        if (MsgTypeId == null) {
            MsgTypeId = 0;
        }
        if (MsgStatusId == null) {
            MsgStatusId = 0;
        }
        if (PageNumber == undefined) {
            PageNumber = 1;
        }
        if (RowsPerPage == undefined) {
            RowsPerPage = 1000;
        }
        var data = "PatientID=" + PatientId + "&MessageId=" + MessageId + "&MsgTypeId=" + MsgTypeId + "&MsgStatusId=" + MsgStatusId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_MESSAGE", "SEARCH_PATIENT_MESSAGE");
    },

    UnLoad: function () {
        /*******/
        if (Patient_MessageAlert.params["ParentCtrl"] == "Bill_PaymentPosting" && Patient_MessageAlert.params.PaymentRef != null)
            UnloadActionPan(Patient_MessageAlert.params.ParentCtrl, "Patient_MessageAlert", null, Patient_MessageAlert.params.PaymentRef);

        else {

            if (Patient_MessageAlert.params != null && Patient_MessageAlert.params.ParentCtrl != null) {
                UnloadActionPan(Patient_MessageAlert.params.ParentCtrl, 'Patient_MessageAlert');
            }
            else
                UnloadActionPan(null, 'Patient_MessageAlert');

            if (Patient_Demographic.isDocExpiryAlert)
                Patient_Demographic.FillPatientDocumentExpiryAlert(Patient_MessageAlert.params.patientID);
        }
    },
}