Patient_UserMessagesQuickLink = {

    bIsFirstLoad: true,
    params: [],

    Load: function (paramerters) {

        Patient_UserMessagesQuickLink.params = paramerters;

        if (Patient_UserMessagesQuickLink.params["PanelID"] != 'pnlPatientUserMessagesQuickLink')
            Patient_UserMessagesQuickLink.params["PanelID"] = Patient_UserMessagesQuickLink.params["PanelID"] + ' #pnlPatientUserMessagesQuickLink';

        var Tab = GetTab(Patient_UserMessagesQuickLink.params["TabID"]);
        if (Tab["PanelID"] != "" && Tab["MasterTabID"] != "") {

            Patient_UserMessagesQuickLink.params["PanelID"] = Tab["Container"] + ' #' + Tab["PanelID"];


        }
        var self = $('#' + Patient_UserMessagesQuickLink.params["PanelID"]);
        self.loadDropDowns(true).done(function () {

            if (globalAppdata["DefaultTabMessages"] == 1) {

                if (globalAppdata["RecentMessagesTab"] == "Patient") {
                    $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + ' #LipatmessagesQuickLink').trigger('click');
                } else {
                    $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + ' #LipracmessagesQuickLink').trigger('click');
                }

            } else if (globalAppdata["DefaultTabMessages"] == 2) {
                $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + ' #LipatmessagesQuickLink').trigger('click');
            } else if (globalAppdata["DefaultTabMessages"] == 3) {
                $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + ' #LipracmessagesQuickLink').trigger('click');
            } else if (globalAppdata["DefaultTabMessages"] == 4) {
                $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + ' #LilogmessagesQuickLink').trigger('click');
            }

        });
        if (params.PreviousTab.TabID == "patTabDemographic") {
            Patient_Demographic.isChangeInDemographic(Patient_Demographic.params.mode);
        }
        else if (params.PreviousTab.TabID == "patTabInsurance") {
            Patient_Insurance.isChangeInInsurance(Patient_Insurance.params.mode);
        }

        utility.CreateDatePicker(Patient_UserMessagesQuickLink.params["PanelID"] + ' #dtpMsgDate', function () {
            //  on-change callback method
        });


    },

    UnLoad: function () {
        if (Patient_UserMessagesQuickLink.params != null && Patient_UserMessagesQuickLink.params.ParentCtrl != null) {
            UnloadActionPan(Patient_UserMessagesQuickLink.params.ParentCtrl, 'Patient_UserMessagesQuickLink');
        }
        else
            UnloadActionPan(null, 'Patient_UserMessagesQuickLink');
    },

    UnLoadTab: function () {

        if (Patient_UserMessagesQuickLink.params != null && Patient_UserMessagesQuickLink.params.ParentCtrl != null) {
            UnloadActionPan(Patient_UserMessagesQuickLink.params.ParentCtrl, 'Patient_UserMessagesQuickLink');
        }
        else
            UnloadActionPan(null, 'Patient_UserMessagesQuickLink');

    },

    ComposePracMessage: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_UserMessagesQuickLink';
        params["mode"] = 'Add';
        params["FromQuicklink"] = '1';
        params["MessageType"] = 'Practice';
        LoadActionPan('Patient_MessageCreate', params);

    },

    ComposePatMessage: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_UserMessagesQuickLink';
        params["mode"] = 'Add';
        params["FromQuicklink"] = '1';
        params["MessageType"] = 'Patient';
        LoadActionPan('Patient_MessageCreate', params);

    },

    SelectAllUserMessagesChkBx: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }
        var objCheck = $(obj);

        if ((objCheck).is(':checked')) {

            $('#dgvPracMessagesGridQuickLink input[type="checkbox"]').prop('checked', 'checked');
        }

        else {
            $('#dgvPracMessagesGridQuickLink input[type=checkbox]').each(function () {

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

            $('#dgvPatMessagesGridQuickLink input[type="checkbox"]').prop('checked', 'checked');
        }

        else {
            $('#dgvPatMessagesGridQuickLink input[type=checkbox]').each(function () {

                $(this).attr('checked', false);
            });
        }

    },
    DeleteUserMessage: function (UserMessageIds, event, type) {
        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('1', function () {
            Patient_UserMessagesQuickLink.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
                if (response.status != false) {
                    if (type == 'practice') {
                        var table1 = $('#dgvPracMessagesGridQuickLink').DataTable();
                        table1.row('.active').remove().draw(false);
                        Patient_UserMessagesQuickLink.SearchPracticeMessage();
                    } else if (type == 'patient') {
                        var table1 = $('#dgvPatMessagesGridQuickLink').DataTable();
                        table1.row('.active').remove().draw(false);
                        Patient_UserMessagesQuickLink.SearchPatientMessage();
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

    DeleteSelectedMessages: function (gridId) {
        var tableId = gridId;
        var messageIDs = "";
        if ($("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #" + tableId + " tbody tr input:checked").length == 0) {
            utility.DisplayMessages('Please select any message to delete', 4);
        } else {
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #" + tableId + " tbody tr input:checked").each(function (i, item) {

                messageIDs += "," + $(item).attr('id');
            });

            utility.myConfirm('66', function () {
                Patient_UserMessagesQuickLink.DeleteSelectedUserMessages(messageIDs);

            }, function () { },
                '1'
            );
        }


    },
    DeleteSelectedUserMessages: function (UserMessageIds) {

        Patient_UserMessagesQuickLink.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);

            } else {
                utility.DisplayMessages(response.Message, 3);
            }
            var activeTab;
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + ' .tabs-custom li').each(function (index, element) {
                if ($(element).hasClass('active')) {
                    activeTab = $(element);
                }
            });
            var attrId = activeTab[0];
            attrId = $(attrId).find('a').attr('id');
            if (attrId.indexOf('pracmessages') > -1) {
                var table1 = $('#dgvPracMessagesGridQuickLink').DataTable();
                table1.row('.active').remove().draw(false);
                Patient_UserMessagesQuickLink.SearchPracticeMessage();
            } else if (attrId.indexOf('patmessages') > -1) {
                var table1 = $('#dgvPatMessages').DataTable();
                table1.row('.active').remove().draw(false);
                Patient_UserMessagesQuickLink.SearchPatientMessage();
            }
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + ' #dgvUserMessagesGrid thead tr #selectMessages').prop('checked', false);
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + ' #dgvPatientMessagesGrid thead tr #selectMessages').prop('checked', false);
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
        $('#dgvPracMessagesGridQuickLink > tbody > tr').each(function () {
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

            Patient_UserMessagesQuickLink.DeleteMultipleUserMessages_DBCall(msgIds);

        } else {
            utility.DisplayMessages("Please Select message.", 3);
        }

    },

    DeleteMultiplePatUserMessages: function () {


        var selected = [];
        var msgIds;
        $('#dgvPatMessagesGridQuickLink > tbody > tr').each(function () {
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

            Patient_UserMessagesQuickLink.DeleteMultiplePatUserMessages_DBCall(msgIds);

        } else {
            utility.DisplayMessages("Please Select message.", 3);
        }

    },

    DeleteMultiplePatUserMessages_DBCall: function (UserMessageIds) {

        utility.myConfirm('40', function () {
            Patient_UserMessagesQuickLink.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
                if (response.status != false) {
                    var table1 = $('#dgvPatMessagesGridQuickLink').DataTable();
                    table1.row('.active').remove().draw(false);
                    Patient_UserMessagesQuickLink.SearchPatientMessage();

                    utility.callbackAfterAllDOMLoaded(function () {
                        utility.DisplayMessages(response.Message, 1);
                    });
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
            Patient_UserMessagesQuickLink.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
                if (response.status != false) {
                    var table1 = $('#dgvPracMessagesGridQuickLink').DataTable();
                    table1.row('.active').remove().draw(false);
                    Patient_UserMessagesQuickLink.SearchPracticeMessage();

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
    //    params["ParentCtrl"] = Patient_UserMessagesQuickLink.params["TabID"];
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
        Patient_UserMessagesQuickLink.UserMessageCount().done(function (response) {
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
                $('#LipracmessagesQuickLink span').text(MessageCountJSONData[1].MessageCounts == 0 ? '' : MessageCountJSONData[1].MessageCounts);
                $('#LipatmessagesQuickLink span').text(MessageCountJSONData[2].MessageCounts == 0 ? '' : MessageCountJSONData[2].MessageCounts);
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

                        $('#spnMessageCount').text(totalCount == 0 ? '' : totalCount);
                        $('#liMsgsPractice span').text(MessageCountsJSONData[1].MessageCounts == 0 ? '' : MessageCountsJSONData[1].MessageCounts);
                        $('#liMsgsPatient span').text(MessageCountsJSONData[2].MessageCounts == 0 ? '' : MessageCountsJSONData[2].MessageCounts);
                        $('#liMsgsDirect span').text(MessageCountsJSONData[0].MessageCounts == 0 ? '' : MessageCountsJSONData[0].MessageCounts);
                    }
                });
            }
        });
    },
    ViewUserMessage: function (UserMessageId, event, Type) {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_UserMessagesQuickLink';
        params["mode"] = 'Edit';
        params["Isopentask"] = '1';
        params["FromQuicklink"] = '1';
        params["MessageType"] = Type;
        params["UserMessageId"] = UserMessageId;
        LoadActionPan('Patient_MessageCreate', params);

    },
    MessageLogGridLoad: function (response, PageNo, rpp) {
        $("#dgvPatientMessageLogQuickLink").dataTable().fnDestroy();
        $("#msgsPatLog #dgvPatientMessageLogQuickLink tbody").find("tr").remove();
        if (response.MessageLogCount > 0) {
            var MessageLogLoadJSONData = JSON.parse(response.MessageLogLoad_JSON);
            $.each(MessageLogLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", item.UserMessagesId);
                $row.attr("PatMsgId", item.UserMessagesId);

                $row.append('<td>' + item.Date + '</td><td>' + item.from + '</td><td>' + item.to + '</td><td>' + item.Subject + '</td><td>' + item.messagetype + '</td>');
                $("#msgsPatLog #dgvPatientMessageLogQuickLink tbody").last().append($row);
            });
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divLogsPagingQuickLink").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging(Patient_UserMessagesQuickLink.params["PanelID"] + " #divLogsPagingQuickLink", response.iTotalDisplayRecords, 5, "Patient_UserMessagesQuickLink", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divLogsPagingQuickLink #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divLogsPagingQuickLink li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divLogsPagingQuickLink").css("display", "none");
            $('#dgvPatientMessageLogQuickLink').DataTable({
                "language": {
                    "emptyTable": "No Log Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvPatientMessageLogQuickLink'))
            ;
        else
            $("#msgsPatLog #dgvPatientMessageLogQuickLink").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
    },

    SearchPracticeMessage: function (PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Tasks", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_UserMessagesQuickLink.MessageSearch(PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Patient_UserMessagesQuickLink.PracMessageGridLoad(response, PageNo, rpp);
                        Patient_UserMessagesQuickLink.MessageCount();
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
                Patient_UserMessagesQuickLink.LoadLogMessages(PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Patient_UserMessagesQuickLink.MessageLogGridLoad(response, PageNo, rpp);
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
        objData["PatientId"] = "";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },
    SearchPatientMessage: function (PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Tasks", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_UserMessagesQuickLink.PatientMessageSearch(PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Patient_UserMessagesQuickLink.PatMessageGridLoad(response, PageNo, rpp);
                        Patient_UserMessagesQuickLink.MessageCount();
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
    PatMessageGridLoad: function (response, PageNo, rpp) {
        globalAppdata.RecentMessagesTab = "Patient"
        $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPatMessagesGridQuickLink").dataTable().fnDestroy();
        $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPatMessagesGridQuickLink tbody").find("tr").remove();
        if ($("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPatMessagesGridQuickLink thead tr #selectMessages").length == 0) {
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPatMessagesGridQuickLink thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Patient_UserMessagesQuickLink.checkUncheckAllMessages(this);" controlname="selectMessages" id="selectMessages" name="chkHeader" class="input-block pull-left" coltype="checkbox"/></th>');
        } else {
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + ' #dgvPatMessagesGridQuickLink thead tr #selectMessages').prop('checked', false);
        }
        if (response.MessageCount > 0) {
            var MessageLoadJSONData = JSON.parse(response.MessageLoad_JSON);
            $.each(MessageLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", item.UserMessagesId);
                $row.attr("MessageId", item.UserMessagesId);
                //$row.attr("onclick", "Patient_UserMessagesQuickLink.ViewUserMessage(" + item.UserMessagesId + ",event,'Patient');");
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

                var onclick = 'onclick="Patient_UserMessagesQuickLink.ViewUserMessage(\'' + item.UserMessagesId + '\',event,\'Patient\');"';
                $row.addClass(messageisread);
                var SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Patient_UserMessagesQuickLink.checkUncheckMessage(event);" id="' + item.UserMessagesId + '" name="SelectCheckBoxOrder" class="input-block"/></td>';
                $row.append(SelectionCheckBoxColumn + '<td ' + onclick + '>' + item.AssignedFrom + '</td><td ' + onclick + '>' + item.Subject + '</td><td ' + onclick + '>' + item.CreatedOn + '</td><td ' + onclick + ' ' + color + '>' + item.Priority + '</td>');
                $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPatMessagesGridQuickLink tbody").last().append($row);
            });

            //----------------- Patient Messages Paging----
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divPatMessagesPagingQuickLink").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging(Patient_UserMessagesQuickLink.params["PanelID"] + " #divPatMessagesPagingQuickLink", response.iTotalDisplayRecords, 5, "Patient_UserMessagesQuickLink", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divPatMessagesPagingQuickLink #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divPatMessagesPagingQuickLink li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }

        else {
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divPatMessagesPagingQuickLink").css("display", "none");
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPatMessagesGridQuickLink").DataTable({
                "language": {
                    "emptyTable": "No Message Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });

        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });


        if ($.fn.dataTable.isDataTable("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPatMessagesGridQuickLink"))
            ;
        else
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPatMessagesGridQuickLink").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },
    SelectedPageClick: function (PageNo, objPage, TotalRecords, rpp, pagingDivId) {
        var selecter = "";
        if (pagingDivId.indexOf("divLogsPagingQuickLink") >= 0) {
            Patient_UserMessagesQuickLink.SearchLogMessages(PageNo, 15);
            selecter = "#" + Patient_UserMessagesQuickLink.params.PanelID + " #pnlResultPatLogGridQuickLink li";
        }
        else if (pagingDivId.indexOf("divPatMessagesPagingQuickLink") >= 0) {
            Patient_UserMessagesQuickLink.SearchPatientMessage(PageNo, 15);
            selecter = "#" + Patient_UserMessagesQuickLink.params.PanelID + " #pnlResultPatMessagesGridQuickLink li";
        }
        else if (pagingDivId.indexOf("divPracMessagesPagingQuickLink") >= 0) {
            Patient_UserMessagesQuickLink.SearchPracticeMessage(PageNo, 15);
            selecter = "#" + Patient_UserMessagesQuickLink.params.PanelID + " #pnlResultPracMessagesGridQuickLink li";
        }
        // Change Background Color to Black for selected page
        $(selecter).each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var selecter = "";
        if (pagingDivId.indexOf("divLogsPagingQuickLink") >= 0)
            selecter = "#" + Patient_UserMessagesQuickLink.params.PanelID + " #pnlResultPatLogGridQuickLink li";
        else if (pagingDivId.indexOf("divPatMessagesPagingQuickLink") >= 0)
            selecter = "#" + Patient_UserMessagesQuickLink.params.PanelID + " #pnlResultPatMessagesGridQuickLink li";
        else if (pagingDivId.indexOf("divPracMessagesPagingQuickLink") >= 0)
            selecter = "#" + Patient_UserMessagesQuickLink.params.PanelID + " #pnlResultPracMessagesGridQuickLink li";

        var currentPageNo = "";
        $(selecter).each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {

            if (pagingDivId.indexOf("divLogsPagingQuickLink") >= 0)
                Patient_UserMessagesQuickLink.SearchLogMessages(currentPageNo, 15);
            else if (pagingDivId.indexOf("divPatMessagesPagingQuickLink") >= 0)
                Patient_UserMessagesQuickLink.SearchPatientMessage(currentPageNo, 15);
            else if (pagingDivId.indexOf("divPracMessagesPagingQuickLink") >= 0)
                Patient_UserMessagesQuickLink.SearchPracticeMessage(currentPageNo, 15);
        }
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var selecter = "";
        if (pagingDivId.indexOf("divLogsPagingQuickLink") >= 0)
            selecter = "#" + Patient_UserMessagesQuickLink.params.PanelID + " #pnlResultPatLogGridQuickLink li";
        else if (pagingDivId.indexOf("divPatMessagesPagingQuickLink") >= 0)
            selecter = "#" + Patient_UserMessagesQuickLink.params.PanelID + " #pnlResultPatMessagesGridQuickLink li";
        else if (pagingDivId.indexOf("divPracMessagesPagingQuickLink") >= 0)
            selecter = "#" + Patient_UserMessagesQuickLink.params.PanelID + " #pnlResultPracMessagesGridQuickLink li";

        var currentPageNo = "";
        $(selecter).each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {

            if (pagingDivId.indexOf("divLogsPagingQuickLink") >= 0)
                Patient_UserMessagesQuickLink.SearchLogMessages(currentPageNo, 15);
            else if (pagingDivId.indexOf("divPatMessagesPagingQuickLink") >= 0)
                Patient_UserMessagesQuickLink.SearchPatientMessage(currentPageNo, 15);
            else if (pagingDivId.indexOf("divPracMessagesPagingQuickLink") >= 0)
                Patient_UserMessagesQuickLink.SearchPracticeMessage(currentPageNo, 15);
        }
    },
    checkUncheckAllMessages: function (obj) {
        var tableId = $(obj.parentNode.parentNode.parentNode.parentNode).attr("id");
        if ($(obj).is(':checked')) {
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #" + tableId + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #" + tableId + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }

    },
    checkUncheckMessage: function (event) {
        event.stopPropagation();
    },
    PracMessageGridLoad: function (response, PageNo, rpp) {
        globalAppdata.RecentMessagesTab = "Practice"
        $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPracMessagesGridQuickLink").dataTable().fnDestroy();
        $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPracMessagesGridQuickLink tbody").find("tr").remove();
        if ($("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPracMessagesGridQuickLink thead tr #selectMessages").length == 0) {
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPracMessagesGridQuickLink thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Patient_UserMessagesQuickLink.checkUncheckAllMessages(this);" controlname="selectMessages" id="selectMessages" name="chkHeader" class="input-block pull-left" coltype="checkbox"/></th>');
        } else {
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPracMessagesGridQuickLink thead tr #selectMessages").prop('checked', false);
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPracMessagesGridQuickLink thead tr #selectMessages").prop('checked', false);
        }
        if (response.MessageCount > 0) {
            var MessageLoadJSONData = JSON.parse(response.MessageLoad_JSON);
            $.each(MessageLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", item.UserMessagesId);
                $row.attr("MessageId", item.UserMessagesId);
                //$row.attr("onclick", "Patient_UserMessagesQuickLink.ViewUserMessage(" + item.UserMessagesId + ",event,'Practice');");
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
                var onclick = 'onclick="Patient_UserMessagesQuickLink.ViewUserMessage(\'' + item.UserMessagesId + '\',event,\'Practice\');"';
                $row.addClass(messageisread);
                var SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Patient_UserMessagesQuickLink.checkUncheckMessage(event);" id="' + item.UserMessagesId + '" name="SelectCheckBoxOrder" class="input-block"/></td>';

                $row.append(SelectionCheckBoxColumn + '<td ' + onclick + '>' + item.AssignedFrom + '</td><td ' + onclick + '>' + item.Subject + '</td><td ' + onclick + '>' + item.CreatedOn + '</td><td ' + onclick + ' ' + color + '>' + item.Priority + '</td>');
                $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPracMessagesGridQuickLink tbody").last().append($row);
            });

            //----------------- Patient Messages Paging----
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divPracMessagesPagingQuickLink").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging(Patient_UserMessagesQuickLink.params["PanelID"] + " #divPracMessagesPagingQuickLink", response.iTotalDisplayRecords, 5, "Patient_UserMessagesQuickLink", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divPracMessagesPagingQuickLink #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divPracMessagesPagingQuickLink li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }

        else {
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #divPracMessagesPagingQuickLink").css("display", "none");
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPracMessagesGridQuickLink").DataTable({
                "language": {
                    "emptyTable": "No Message Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });

        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });


        if ($.fn.dataTable.isDataTable("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPracMessagesGridQuickLink"))
            ;
        else
            $("#" + Patient_UserMessagesQuickLink.params["PanelID"] + " #dgvPracMessagesGridQuickLink").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },

    PatientMessageSearch: function (PageNumber, RowsPerPage) {
        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;
        var objData = new Object();


        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["MessageName"] = $("#pnlPatientUserMessagesQuickLink #msgsPat #txtMsgsName").val();
        objData["Priority"] = $("#pnlPatientUserMessagesQuickLink #msgsPat #ddlPatientPriority").val();
        objData["MessageDate"] = $("#pnlPatientUserMessagesQuickLink #msgsPat #dtpMsgDate").val();
        objData["CommandType"] = "Load_Messages";
        objData["PatientId"] = "";
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
        objData["MessageName"] = $("#pnlPatientUserMessagesQuickLink #msgsPrac #txtMsgsName").val();
        objData["Priority"] = $("#pnlPatientUserMessagesQuickLink #msgsPrac #ddlPriority").val();
        objData["MessageDate"] = $("#pnlPatientUserMessagesQuickLink #msgsPrac #dtpMsgDate").val();
        objData["CommandType"] = "Load_Messages";
        objData["PatientId"] = "";
        objData["MessageType"] = "Practice";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");

    },

}