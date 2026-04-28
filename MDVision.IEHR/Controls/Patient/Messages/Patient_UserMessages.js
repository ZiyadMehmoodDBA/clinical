Patient_UserMessages = {

    bIsFirstLoad: true,
    params: [],

    Load: function (paramerters) {

        Patient_UserMessages.params = paramerters;

        if (Patient_UserMessages.params["PanelID"] != 'pnlPatientUserMessages')
            Patient_UserMessages.params["PanelID"] = Patient_UserMessages.params["PanelID"] + ' #pnlPatientUserMessages';

        var Tab = GetTab(Patient_UserMessages.params["TabID"]);
        if (Tab["PanelID"] != "" && Tab["MasterTabID"] != "") {

            Patient_UserMessages.params["PanelID"] = Tab["Container"] + ' #' + Tab["PanelID"];


        }
        var self = $('#' + Patient_UserMessages.params["PanelID"]);
        self.loadDropDowns(true).done(function () {
            if (globalAppdata["DefaultTabMessages"] == 1) {

                if (globalAppdata["RecentMessagesTab"] == "Patient") {
                    $("#" + Patient_UserMessages.params["PanelID"] + ' #Lipatmessages').trigger('click');
                } else {
                    $("#" + Patient_UserMessages.params["PanelID"] + ' #Lipracmessages').trigger('click');
                }

            } else if (globalAppdata["DefaultTabMessages"] == 2) {
                $("#" + Patient_UserMessages.params["PanelID"] + ' #Lipatmessages').trigger('click');
            } else if (globalAppdata["DefaultTabMessages"] == 3) {
                $("#" + Patient_UserMessages.params["PanelID"] + ' #Lipracmessages').trigger('click');
            } else if (globalAppdata["DefaultTabMessages"] == 4) {
                $("#" + Patient_UserMessages.params["PanelID"] + ' #LiLogMessages').trigger('click');
            }

        });
        if (params.PreviousTab.TabID == "patTabDemographic") {
            Patient_Demographic.isChangeInDemographic(Patient_Demographic.params.mode);
        }
        else if (params.PreviousTab.TabID == "patTabInsurance") {
            Patient_Insurance.isChangeInInsurance(Patient_Insurance.params.mode);
        }

        utility.CreateDatePicker(Patient_UserMessages.params["PanelID"] + ' #dtpMsgDate', function () {
            //  on-change callback method
        });


    },

    UnLoad: function () {
        if (Patient_UserMessages.params != null && Patient_UserMessages.params.ParentCtrl != null) {
            UnloadActionPan(Patient_UserMessages.params.ParentCtrl, 'Patient_UserMessages');
        }
        else
            UnloadActionPan(null, 'Patient_UserMessages');
    },

    UnLoadTab: function () {
        if (Patient_UserMessages.params["FromAdmin"] == "0") {
            if (Patient_UserMessages.params != null && Patient_UserMessages.params.ParentCtrl != null) {
                UnloadActionPan(Patient_UserMessages.params.ParentCtrl, 'Patient_UserMessages');
            }
            else
                UnloadActionPan(null, 'Patient_UserMessages');
        }
        else {
            RemoveAdminTab();
        }
    },

    ComposePracMessage: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = Patient_UserMessages.params["TabID"];
        params["mode"] = 'Add';
        params["FromPatModule"] = '1';
        params["MessageType"] = 'Practice';
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        LoadActionPan('Patient_MessageCreate', params);

    },

    ComposePatMessage: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = Patient_UserMessages.params["TabID"];
        params["mode"] = 'Add';
        params["FromPatModule"] = '1';
        params["MessageType"] = 'Patient';
        LoadActionPan('Patient_MessageCreate', params);

    },

    SelectAllUserMessagesChkBx: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }
        var objCheck = $(obj);

        if ((objCheck).is(':checked')) {

            $('#dgvPracMessagesGrid input[type="checkbox"]').prop('checked', 'checked');
        }

        else {
            $('#dgvPracMessagesGrid input[type=checkbox]').each(function () {

                $(this).attr('checked', false);
            });
        }

    },
    SelectAllPatUserMessagesChkBx: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }
        var objCheck = $(obj);

        if ((objCheck).is(':checked')) {

            $('#dgvPatMessagesGrid input[type="checkbox"]').prop('checked', 'checked');
        }

        else {
            $('#dgvPatMessagesGrid input[type=checkbox]').each(function () {

                $(this).attr('checked', false);
            });
        }

    },
    DeleteUserMessage: function (UserMessageIds, event, type) {
        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('1', function () {
            Patient_UserMessages.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
                if (response.status != false) {
                    if (type == 'practice') {
                        var table1 = $('#dgvPracMessagesGrid').DataTable();
                        table1.row('.active').remove().draw(false);
                        Patient_UserMessages.SearchPracticeMessage();
                    } else if (type == 'patient') {
                        var table1 = $('#dgvPatMessagesGrid').DataTable();
                        table1.row('.active').remove().draw(false);
                        Patient_UserMessages.SearchPatientMessage();
                    }

                    utility.DisplayMessages(response.Message, 1);
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }, function () { },
                    '1'
                );



    },
    DeleteSelectedUserMessages: function (UserMessageIds) {

        Patient_UserMessages.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
            if (response.status != false) {
               

                utility.DisplayMessages(response.Message, 1);
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
            var activeTab;
            $("#" + Patient_UserMessages.params["PanelID"] + ' .tabs-custom li').each(function (index, element) {
                if ($(element).hasClass('active')) {
                    activeTab = $(element);
                }
            });
            var attrId = activeTab[0];
            attrId = $(attrId).find('a').attr('id');
            if (attrId == 'Lipracmessages') {
                var table1 = $('#dgvPracMessagesGrid').DataTable();
                table1.row('.active').remove().draw(false);
                Patient_UserMessages.SearchPracticeMessage();
            } else if (attrId == 'Lipatmessages') {
                var table1 = $('#dgvPatMessagesGrid').DataTable();
                table1.row('.active').remove().draw(false);
                Patient_UserMessages.SearchPatientMessage();
            }
            $("#" + Patient_UserMessages.params["PanelID"] + ' #dgvPracMessagesGrid thead tr #selectMessages').prop('checked', false);
            $("#" + Patient_UserMessages.params["PanelID"] + ' #dgvPatMessagesGrid thead tr #selectMessages').prop('checked', false);
        });

    },

    DeleteUserMessage_DBCall: function (UserMessageIds) {

        var objData = new Object();
        objData["CommandType"] = "delete_message";
        objData["UserMessagesIds"] = UserMessageIds;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");


    },

    DeleteMultipleUserMessages: function () {


        var selected = [];
        var msgIds;
        $('#dgvPracMessagesGrid > tbody > tr').each(function () {
            if ($(this).find('input:checked').is(':checked')) {
                selected.push($(this).attr('id'));
                //selected.push($(this).attr('id').replace('checkbox', ''));
            }
        });

        for (var w = 0; w <= selected.length; w++) {
            msgIds += selected[w] + ',';
        }
        msgIds = msgIds.replace('undefined,', '');
        msgIds = msgIds.replace('undefined', '');
        if (msgIds != "") {

            Patient_UserMessages.DeleteMultipleUserMessages_DBCall(msgIds);

        } else {
            utility.DisplayMessages("Please Select message.", 3);
        }

    },

    DeleteMultiplePatUserMessages: function () {


        var selected = [];
        var msgIds;
        $('#dgvPatMessagesGrid > tbody > tr').each(function () {
            if ($(this).find('input:checked').is(':checked')) {
                selected.push($(this).attr('id'));
                //selected.push($(this).attr('id').replace('checkbox', ''));
            }
        });

        for (var w = 0; w <= selected.length; w++) {
            msgIds += selected[w] + ',';
        }
        msgIds = msgIds.replace('undefined,', '');
        msgIds = msgIds.replace('undefined', '');
        if (msgIds != "") {

            Patient_UserMessages.DeleteMultiplePatUserMessages_DBCall(msgIds);

        } else {
            utility.DisplayMessages("Please Select message.", 3);
        }

    },

    DeleteMultiplePatUserMessages_DBCall: function (UserMessageIds) {

        utility.myConfirm('40', function () {
            Patient_UserMessages.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
                if (response.status != false) {
                    var table1 = $('#dgvPatMessagesGrid').DataTable();
                    table1.row('.active').remove().draw(false);
                    Patient_UserMessages.SearchPatientMessage();

                    utility.DisplayMessages(response.Message, 1);
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }, function () { },
                    'Confirm Delete'
                );
    },
    DeleteMultipleUserMessages_DBCall: function (UserMessageIds) {

        utility.myConfirm('40', function () {
            Patient_UserMessages.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
                if (response.status != false) {
                    var table1 = $('#dgvPracMessagesGrid').DataTable();
                    table1.row('.active').remove().draw(false);
                    Patient_UserMessages.SearchPracticeMessage();

                    utility.DisplayMessages(response.Message, 1);
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }, function () { },
                    'Confirm Delete'
                );
    },

    //ViewUserMessage: function (UserMessageId) {

    //    var params = [];
    //    params["FromAdmin"] = "0";
    //    params["ParentCtrl"] = Patient_UserMessages.params["TabID"];
    //    params["mode"] = 'Edit';
    //    params["UserMessageId"] = UserMessageId;
    //    LoadActionPan('Patient_MessageCompose', params);

    //},
    UserMessageCount: function () {

        var objData = new Object();
        objData["CommandType"] = "get_messages_count";
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");

    },
    MessageCount: function () {
        Patient_UserMessages.UserMessageCount().done(function (response) {
            if (response.status != false) {
                var MessageCountJSONData = JSON.parse(response.MessageCount_JSON);
                //var totalCount = 0;
                //$.each(MessageCountJSONData, function (index, element) {
                //    totalCount += parseInt(element.MessageCounts);
                //});


                //if (totalCount == 0) {
                //    $('#wpanel #Messages .badge').hide();
                //    $('.notifications #Messages .badge').hide();
                //} else {
                //    $('#wpanel #Messages .badge').show();
                //    $('.notifications #Messages .badge').show();
                //    $('#wpanel #Messages .badge').text(totalCount);
                //    $('.notifications #Messages .badge').text(totalCount);
                //}

                //$('#wpanel .badge').eq(22).text(totalCount);
                $('#Lipracmessages span').text(MessageCountJSONData[1].MessageCounts == 0 ? '' : MessageCountJSONData[1].MessageCounts);
                $('#Lipatmessages span').text(MessageCountJSONData[2].MessageCounts == 0 ? '' : MessageCountJSONData[2].MessageCounts);
                // $('#liMsgsDirect span').text(MessageCountJSONData[0].MessageCounts == 0 ? '' : MessageCountJSONData[0].MessageCounts);
                Patient_MessageCompose.UserMessageCount().done(function (response) {
                    if (response.status != false) {
                        var MessageCountsJSONData = JSON.parse(response.MessageCount_JSON);
                        var totalCount = 0;
                        $.each(MessageCountsJSONData, function (index, element) {
                            if (element.MessageType != "Task") {
                                totalCount += parseInt(element.MessageCounts);
                            }

                        });


                        if (totalCount == 0) {
                            $('#wpanel div.wMessages .badge').hide();
                            $('.notifications #Messages .badge').hide();
                        } else {
                            $('#wpanel div.wMessages .badge').show();
                            $('.notifications #Messages .badge').show();
                            $('#wpanel div.wMessages .badge').text(totalCount);
                            $('.notifications #Messages .badge').text(totalCount);
                        }

                        $('#spnMessageCount').text(MessageCountsJSONData[3].MessageCounts == 0 ? '' : MessageCountsJSONData[3].MessageCounts);
                        $('#liMsgsPractice span').text(MessageCountsJSONData[1].MessageCounts == 0 ? '' : MessageCountsJSONData[1].MessageCounts);
                        $('#liMsgsPatient span').text(MessageCountsJSONData[2].MessageCounts == 0 ? '' : MessageCountsJSONData[2].MessageCounts);
                        $('#liMsgsDirect span').text(MessageCountsJSONData[0].MessageCounts == 0 ? '' : MessageCountsJSONData[0].MessageCounts);
                    }
                });
            }
        });
    },
    ViewUserMessage: function (UserMessageId, UserId, event, Type) {
        if (Type == "Patient" && !UserId) {
            var params = [];
            params["UserMessageId"] = UserMessageId;
            params["ParentCtrl"] = Patient_UserMessages.params["TabID"];
            LoadActionPan('Patient_MessageKey', params);
            Patient_UserMessages.OpenEncryptedMessage_DBCall(UserMessageId).done(function (response) {
                if (response.status) {
                    console.log(response.message);
                } else {
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }
        else {
            var params = [];
            params["FromAdmin"] = "0";
            params["ParentCtrl"] = Patient_UserMessages.params["TabID"];
            params["mode"] = 'Edit';
            params["Isopentask"] = '1';
            params["FromPatModule"] = '1';
            params["MessageType"] = Type;
            params["UserMessageId"] = UserMessageId;
            LoadActionPan('Patient_MessageCreate', params);
        }

    },
    MessageLogGridLoad: function (response, PageNo, rpp) {
        $("#dgvPatientMessageLog").dataTable().fnDestroy();
        $("#msgsPatLog #dgvPatientMessageLog tbody").find("tr").remove();
        if (response.MessageLogCount > 0) {
            var MessageLogLoadJSONData = JSON.parse(response.MessageLogLoad_JSON);
            $.each(MessageLogLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", item.UserMessagesId);
                $row.attr("PatMsgId", item.UserMessagesId);

                $row.append('<td>' + item.Date + '</td><td>' + item.from + '</td><td>' + item.to + '</td><td>' + item.Subject + '</td><td>' + item.messagetype + '</td>');
                $("#msgsPatLog #dgvPatientMessageLog tbody").last().append($row);
            });
            $("#" + Patient_UserMessages.params["PanelID"] + " #divLogsPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging(Patient_UserMessages.params["PanelID"] + " #divLogsPaging", response.iTotalDisplayRecords, 5, "Patient_UserMessages", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#" + Patient_UserMessages.params["PanelID"] + " #divLogsPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#" + Patient_UserMessages.params["PanelID"] + " #divLogsPaging li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#" + Patient_UserMessages.params["PanelID"] + " #divLogsPaging").css("display", "none");
            $('#dgvPatientMessageLog').DataTable({
                "language": {
                    "emptyTable": "No Log Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvPatientMessageLog'))
            ;
        else
            $("#msgsPatLog #dgvPatientMessageLog").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
    },

    SearchPracticeMessage: function (PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Tasks", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_UserMessages.MessageSearch(PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Patient_UserMessages.PracMessageGridLoad(response, PageNo, rpp);
                        Patient_UserMessages.MessageCount();
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
    SearchLogMessages: function (PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Tasks", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_UserMessages.LoadLogMessages(PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Patient_UserMessages.MessageLogGridLoad(response, PageNo, rpp);
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
    LoadLogMessages: function (PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new Object();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "load_message_log";
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },
    SearchPatientMessage: function (PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Tasks", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_UserMessages.PatientMessageSearch(PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Patient_UserMessages.PatMessageGridLoad(response, PageNo, rpp);
                        Patient_UserMessages.MessageCount();
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
    DeleteSelectedMessages: function (gridId) {
        var tableId = gridId;
        var messageIDs = "";
        if ($("#" + Patient_UserMessages.params["PanelID"] + " #" + tableId + " tbody tr input:checked").length == 0) {
            utility.DisplayMessages('Please select any message to delete', 4);
        } else {
            $("#" + Patient_UserMessages.params["PanelID"] + " #" + tableId + " tbody tr input:checked").each(function (i, item) {

                messageIDs += "," + $(item).attr('id');
            });

            utility.myConfirm('66', function () {
                Patient_UserMessages.DeleteSelectedUserMessages(messageIDs);

            }, function () { },
                '1'
            );
        }


    },
    checkUncheckAllMessages: function (obj) {
        var tableId = $(obj.parentNode.parentNode.parentNode.parentNode).attr("id");
        if ($(obj).is(':checked')) {
            $("#" + Patient_UserMessages.params["PanelID"] + " #" + tableId + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#" + Patient_UserMessages.params["PanelID"] + " #" + tableId + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }

    },
    checkUncheckMessage: function (event) {
        event.stopPropagation();
    },
    PatMessageGridLoad: function (response, PageNo, rpp) {
        globalAppdata.RecentMessagesTab = "Patient"
        $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPatMessagesGrid").dataTable().fnDestroy();
        $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPatMessagesGrid tbody").find("tr").remove();
        if ($("#" + Patient_UserMessages.params["PanelID"] + " #dgvPatMessagesGrid thead tr #selectMessages").length == 0) {
            $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPatMessagesGrid thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Patient_UserMessages.checkUncheckAllMessages(this);" controlname="selectMessages" id="selectMessages" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
        } else {
            $("#" + Patient_UserMessages.params["PanelID"] + ' #dgvPracMessagesGrid thead tr #selectMessages').prop('checked', false);
            $("#" + Patient_UserMessages.params["PanelID"] + ' #dgvPatMessagesGrid thead tr #selectMessages').prop('checked', false);
        }
        if (response.MessageCount > 0) {
            var MessageLoadJSONData = JSON.parse(response.MessageLoad_JSON);
            $.each(MessageLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", item.UserMessagesId);
                $row.attr("MessageId", item.UserMessagesId);
                //$row.attr("onclick", "Patient_UserMessages.ViewUserMessage(" + item.UserMessagesId + "," + item.UserId + ",event,'Patient');");
                var color = "";
                if (item.Priority == "High") {
                    color = 'style = "color:red"'
                } else if (item.Priority == "Medium") {
                    color = 'style = "color:orange"'
                } else if (item.Priority == "Low") {
                    color = 'style = "color:green"'
                }
                var messageisread = "";
                if (item.IsRead == "False") {
                    messageisread = "bg-info active";

                }
                var onclick = 'onclick="Patient_UserMessages.ViewUserMessage(\'' + item.UserMessagesId + '\',\'' + item.UserId + '\',event,\'Patient\');"';
                var UserId = item.UserId ? item.UserId : "0";
                $row.addClass(messageisread);
                var SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Patient_UserMessages.checkUncheckMessage(event);" id="' + item.UserMessagesId + '" name="SelectCheckBoxOrder" class="input-block"/></td>';
                $row.append(SelectionCheckBoxColumn + '<td ' + onclick + '>' + item.AssignedFrom + '</td><td ' + onclick + '>' + item.Subject + '</td><td ' + onclick + '>' + item.CreatedOn + '</td><td ' + onclick + ' ' + color + '>' + item.Priority + '</td><td ' + onclick + '> <div title="' + item.MessageHashZ + '" class="ellipses size-max350">' + item.MessageHashZ + '</div></td>');
                $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPatMessagesGrid tbody").last().append($row);
            });

            //----------------- Patient Messages Paging----
            $("#" + Patient_UserMessages.params["PanelID"] + " #divPatMessagesPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging(Patient_UserMessages.params["PanelID"] + " #divPatMessagesPaging", response.iTotalDisplayRecords, 5, "Patient_UserMessages", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#" + Patient_UserMessages.params["PanelID"] + " #divPatMessagesPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#" + Patient_UserMessages.params["PanelID"] + " #divPatMessagesPaging li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }

        else {
            $("#" + Patient_UserMessages.params["PanelID"] + " #divPatMessagesPaging").css("display", "none");
            $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPatMessagesGrid").DataTable({
                "language": {
                    "emptyTable": "No Message Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, 'order': [], "aoColumnDefs": [{ "orderable": false, "aTargets": [0, 1] }]
            });

        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });


        if ($.fn.dataTable.isDataTable("#" + Patient_UserMessages.params["PanelID"] + " #dgvPatMessagesGrid"))
            ;
        else
            $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPatMessagesGrid").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{ "orderable": false, "aTargets": [0, 1] }] }); // to remove records per page dropdown

    },
    PracMessageGridLoad: function (response, PageNo, rpp) {

        globalAppdata.RecentMessagesTab = "Practice"
        $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPracMessagesGrid").dataTable().fnDestroy();
        $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPracMessagesGrid tbody").find("tr").remove();

        if ($("#" + Patient_UserMessages.params["PanelID"] + " #dgvPracMessagesGrid thead tr #selectMessages").length == 0) {
            $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPracMessagesGrid thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Patient_UserMessages.checkUncheckAllMessages(this);" controlname="selectMessages" id="selectMessages" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
        } else {
            $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPracMessagesGrid thead tr #selectMessages").prop('checked', false);
        }
        if (response.MessageCount > 0) {
            var MessageLoadJSONData = JSON.parse(response.MessageLoad_JSON);
            $.each(MessageLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", item.UserMessagesId);
                $row.attr("MessageId", item.UserMessagesId);
                //$row.attr("onclick", "Patient_UserMessages.ViewUserMessage(" + item.UserMessagesId + ", null ,event,'Practice');");
                var color = "";
                if (item.Priority == "High") {
                    color = 'style = "color:red"'
                } else if (item.Priority == "Medium") {
                    color = 'style = "color:orange"'
                } else if (item.Priority == "Low") {
                    color = 'style = "color:green"'
                }
                var messageisread = "";
                if (item.IsRead == "False") {
                    messageisread = "bg-info active";

                }
                
                var onclick = 'onclick="Patient_UserMessages.ViewUserMessage(\'' + item.UserMessagesId + '\',null,event,\'Practice\');"';
                $row.addClass(messageisread);
                var SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Patient_UserMessages.checkUncheckMessage(event);" id="' + item.UserMessagesId + '" name="SelectCheckBoxOrder" class="input-block"/></td>';
                $row.append(SelectionCheckBoxColumn + '<td ' + onclick + '>' + item.AssignedFrom + '</td><td ' + onclick + '>' + item.Subject + '</td><td ' + onclick + '>' + item.CreatedOn + '</td><td ' + onclick + ' ' + color + '>' + item.Priority + '</td>');
                $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPracMessagesGrid tbody").last().append($row);
            });

            //----------------- Patient Messages Paging----
            $("#" + Patient_UserMessages.params["PanelID"] + " #divPracMessagesPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging(Patient_UserMessages.params["PanelID"] + " #divPracMessagesPaging", response.iTotalDisplayRecords, 5, "Patient_UserMessages", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#" + Patient_UserMessages.params["PanelID"] + " #divPracMessagesPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#" + Patient_UserMessages.params["PanelID"] + " #divPracMessagesPaging li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }

        else {
            $("#" + Patient_UserMessages.params["PanelID"] + " #divPracMessagesPaging").css("display", "none");
            $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPracMessagesGrid").DataTable({
                "language": {
                    "emptyTable": "No Message Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "order": [], "aoColumnDefs": [{ "orderable": false, "aTargets": [0, 1] }]
            });

        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });


        if ($.fn.dataTable.isDataTable("#" + Patient_UserMessages.params["PanelID"] + " #dgvPracMessagesGrid"))
            ;
        else
            $("#" + Patient_UserMessages.params["PanelID"] + " #dgvPracMessagesGrid").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{ "orderable": false, "aTargets": [0, 1] }] }); // to remove records per page dropdown

    },

    PatientMessageSearch: function (PageNumber, RowsPerPage) {
        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;
        var objData = new Object();


        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["MessageName"] = $("#pnlPatientUserMessages #msgsPat #txtMsgsName").val();
        objData["Priority"] = $("#pnlPatientUserMessages #msgsPat #ddlPriorityPatientMessages").val();
        objData["MessageDate"] = $("#pnlPatientUserMessages #msgsPat #dtpMsgDate").val();
        objData["CommandType"] = "Load_Messages";
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["MessageType"] = "Patient";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");

    },

    MessageSearch: function (PageNumber, RowsPerPage) {
        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;
        var objData = new Object();


        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["MessageName"] = $("#pnlPatientUserMessages #msgsPrac #txtMsgsName").val();
        objData["Priority"] = $("#pnlPatientUserMessages #msgsPrac #ddlPriority").val();
        objData["MessageDate"] = $("#pnlPatientUserMessages #msgsPrac #dtpMsgDate").val();
        objData["CommandType"] = "Load_Messages";
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["MessageType"] = "Practice";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");

    },
    OpenEncryptedMessage_DBCall: function (UserMessageId) {
        var objData = new Object();
        objData["UserMesgId"] = UserMessageId;
        objData["PatientId"] = $("#hfPatientId").val();
        objData["CommandType"] = "open_encrypted_message";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");
    },
}