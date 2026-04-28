Admin_RemindersSettings = {
    bIsFirstLoad: true,
    params: [],
    bIsFirstLoad: true,
    Switch: true,
    TextPrivilige: false,
    EmailPrivilige: false,
    VoicePrivilige: false,
    Load: function (params) {
        if (Admin_RemindersSettings.bIsFirstLoad) {
            Admin_RemindersSettings.bIsFirstLoad = false;
            var self = $('#pnlAdminRemindersSettings');
            self.loadDropDowns(true).done(function () {
                Admin_RemindersSettings.IntializeMultiSelectDropDown();
                Admin_RemindersSettings.IntializeMultiSelectDropDownProviders();

            });
            Admin_RemindersSettings.TabPriviliges()//.done(function (response) {
            //Admin_RemindersSettings.RemindersSettingsSearch();
            //});
        }
    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#pnlAdminRemindersSettings #ddlNotesTemplateProvider').multiselect('destroy');
        $('#pnlAdminRemindersSettings #ddlNotesTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            //   onChange: function (option, checked, select) {
            //   Admin_RemindersSettings.checkSpecialtiesByProviderId(option, checked, select);
            // },
            onDropdownHide: function (event) {
                // Clinical_Provider_Note_Template.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesSettingspecialty').multiselect('refresh');
            },


        });
    },
    IntializeMultiSelectDropDown: function () {


        $('#pnlAdminRemindersSettings #ddlNotesTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 116

        });

    },
    RemindersSettingsSearch: function (RemindersTemplateId, PageNo, rpp) {

        if ($("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").css("display") == "none") {
            $("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").show();
        }

        var self = $("#pnlRemindersSettings_Search");
        var myJSON = self.getMyJSONByName();

        Admin_RemindersSettings.SearchRemindersSettings(myJSON, RemindersTemplateId, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                Admin_RemindersSettings.RemindersTextSettingsGridLoad(response, PageNo, rpp);
                //Admin_RemindersSettings.RemindersEmailSettingsGridLoad(response, PageNo, rpp);
                ////var TableControl = "pnlAdminRemindersSettings #dgvRemindersSettingsEmail";
                ////var PagingPanelControlID = "pnlAdminRemindersSettings #divRemindersSettingsEmailPaging";
                ////var ClassControlName = "Admin_RemindersSettings";
                ////var PagesToDisplay = 5;
                ////var iTotalDisplayRecords = response.RemindersEmailSettingsCount;
                ////setTimeout(CreatePagination(response.RemindersEmailSettingsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                ////    function (PrimaryID, PageNumber, ResultPerPage) {
                ////        Admin_RemindersSettings.RemindersSettingsSearch(PrimaryID, PageNumber, ResultPerPage);
                ////    }), 10);
                //if (Admin_RemindersSettings.TextPrivilige == true) {
                //    Admin_RemindersSettings.RemindersSettingsSearch();
                //}
                //if (Admin_RemindersSettings.EmailPrivilige == true) {
                //    Admin_RemindersSettings.RemindersSettingsSearchEmail();
                //}
                //if (Admin_RemindersSettings.VoicePrivilige == true) {
                //    Admin_RemindersSettings.RemindersSettingsSearchVoice();
                //}

                ////var TableControl = "pnlAdminRemindersSettings #dgvRemindersTextSettings";
                ////var PagingPanelControlID = "pnlAdminRemindersSettings #divRemindersTextSettingsPaging";
                ////var ClassControlName = "Admin_RemindersSettings";
                ////var PagesToDisplay = 5;
                ////var iTotalDisplayRecords = response.RemindersTextSettingCount;
                ////setTimeout(CreatePagination(response.RemindersTextSettingCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                ////    function (PrimaryID, PageNumber, ResultPerPage) {
                ////        Admin_RemindersSettings.RemindersSettingsSearch(PrimaryID, PageNumber, ResultPerPage);
                ////    }), 10);
                ////Admin_RemindersSettings.RemindersVoiceSettingsGridLoad(response, PageNo, rpp);
                ////var TableControl = "pnlAdminRemindersSettings #dgvRemindersSettingsVoice";
                ////var PagingPanelControlID = "pnlAdminRemindersSettings #divRemindersSettingsVoicePaging";
                ////var ClassControlName = "Admin_RemindersSettings";
                ////var PagesToDisplay = 5;
                ////var iTotalDisplayRecords = response.RemindersVoiceSettingCount;
                ////setTimeout(CreatePagination(response.RemindersVoiceSettingCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                ////    function (PrimaryID, PageNumber, ResultPerPage) {
                ////        Admin_RemindersSettings.RemindersSettingsSearch(PrimaryID, PageNumber, ResultPerPage);
                ////    }), 10);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });


    },

    RemindersTextSettingsGridLoad: function (response, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Reminder Settings_Text", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var TableControl = "pnlAdminRemindersSettings #dgvRemindersTextSettings";
                var PagingPanelControlID = "pnlAdminRemindersSettings #divRemindersTextSettingsPaging";
                var ClassControlName = "Admin_RemindersSettings";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.RemindersTextSettingCount;
                setTimeout(CreatePagination(response.RemindersTextSettingCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                    function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_RemindersSettings.RemindersSettingsSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                $("#pnlRemindersTextSettings_Result #dgvRemindersTextSettings").dataTable().fnDestroy();
                $("#pnlRemindersTextSettings_Result #dgvRemindersTextSettings tbody").find("tr").remove();
                if (response.RemindersTextSettingCount > 0) {
                    var RemindersTextSettingsLoadJSONData = JSON.parse(response.RemindersTextSettingLoad_JSON);
                    $.each(RemindersTextSettingsLoadJSONData, function (i, item) {
                        var $row = $('<tr/>');
                        $row.attr("onclick", "Admin_RemindersSettings.RemindersTemplateEdit('" + item.ReminderTextSettingId + "',event,\'text\');");
                        $row.attr("id", "gvRemindersSettings_row" + item.ReminderTextSettingId);
                        $row.attr("ScheduleId", item.ReminderTextSettingId);

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

                        $row.append('<td style="display:none;">' + item.ReminderTextSettingId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_RemindersSettings.ReminderSettingsDelete(\'' + item.ReminderTextSettingId + '\',event,\'Text\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_RemindersSettings.RemindersTemplateEdit(\'' + item.ReminderTextSettingId + '\',event,\'text\');" title="View Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_RemindersSettings.ReminderSettingsActiveInactive(\'' + item.ReminderTextSettingId + '\',event,\'Text\',' + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;</td><td>' + item.ProviderName + '</td><td>' + item.ModifiedOn + '</td>');

                        $("#pnlRemindersTextSettings_Result #dgvRemindersTextSettings tbody").last().append($row);
                    });
                }
                else {
                    $('#pnlRemindersTextSettings_Result #dgvRemindersTextSettings').DataTable({
                        "language": {
                            "emptyTable": "No Settings Found"
                        }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }
                if ($.fn.dataTable.isDataTable('#pnlRemindersTextSettings_Result #dgvRemindersTextSettings'))
                    ;
                else {
                    $("#pnlRemindersTextSettings_Result #dgvRemindersTextSettings").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    RemindersEmailSettingsGridLoad: function (response, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Reminder Settings_Email", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var TableControl = "pnlAdminRemindersSettings #dgvRemindersSettingsEmail";
                var PagingPanelControlID = "pnlAdminRemindersSettings #divRemindersSettingsEmailPaging";
                var ClassControlName = "Admin_RemindersSettings";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.RemindersEmailSettingsCount;
                setTimeout(CreatePagination(response.RemindersEmailSettingsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                    function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_RemindersSettings.RemindersSettingsSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                $("#pnlRemindersSettingsEmail_Result #dgvRemindersSettingsEmail").dataTable().fnDestroy();
                $("#pnlRemindersSettingsEmail_Result #dgvRemindersSettingsEmail tbody").find("tr").remove();
                if (response.RemindersEmailSettingsCount > 0) {
                    var RemindersEmailSettingsLoadJSONData = JSON.parse(response.RemindersEmailSettingsLoad_JSON);
                    $.each(RemindersEmailSettingsLoadJSONData, function (i, item) {
                        var $row = $('<tr/>');
                        $row.attr("onclick", "Admin_RemindersSettings.RemindersTemplateEdit('" + item.ReminderEmailSettingId + "',event,\'email\');");
                        $row.attr("id", "gvRemindersSettings_row" + item.ReminderEmailSettingId);
                        $row.attr("ScheduleId", item.ReminderEmailSettingId);

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

                        $row.append('<td style="display:none;">' + item.ReminderEmailSettingId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_RemindersSettings.ReminderSettingsDelete(\'' + item.ReminderEmailSettingId + '\',event,\'email\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_RemindersSettings.RemindersTemplateEdit(\'' + item.ReminderEmailSettingId + '\',event,\'email\');" title="View Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_RemindersSettings.ReminderSettingsActiveInactive(\'' + item.ReminderEmailSettingId + '\',event,\'Email\',' + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;</td><td>' + item.ProviderName + '</td><td>' + item.ModifiedOn + '</td>');

                        $("#pnlRemindersSettingsEmail_Result #dgvRemindersSettingsEmail tbody").last().append($row);
                    });
                }
                else {
                    $('#pnlRemindersSettingsEmail_Result #dgvRemindersSettingsEmail').DataTable({
                        "language": {
                            "emptyTable": "No Settings Found"
                        }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }
                if ($.fn.dataTable.isDataTable('#pnlRemindersSettingsEmail_Result #dgvRemindersSettingsEmail'))
                    ;
                else {
                    $("#pnlRemindersSettingsEmail_Result #dgvRemindersSettingsEmail").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown


                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    RemindersVoiceSettingsGridLoad: function (response, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Reminder Settings_Voice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var TableControl = "pnlAdminRemindersSettings #dgvRemindersSettingsVoice";
                var PagingPanelControlID = "pnlAdminRemindersSettings #divRemindersSettingsVoicePaging";
                var ClassControlName = "Admin_RemindersSettings";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.RemindersVoiceSettingCount;
                setTimeout(CreatePagination(response.RemindersVoiceSettingCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                    function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_RemindersSettings.RemindersSettingsSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                $("#pnlRemindersSettingsVoice_Result #dgvRemindersSettingsVoice").dataTable().fnDestroy();
                $("#pnlRemindersSettingsVoice_Result #dgvRemindersSettingsVoice tbody").find("tr").remove();
                if (response.RemindersVoiceSettingCount > 0) {
                    var RemindersVoiceSettingsLoadJSONData = JSON.parse(response.RemindersVoiceSettingLoad_JSON);
                    $.each(RemindersVoiceSettingsLoadJSONData, function (i, item) {
                        var $row = $('<tr/>');
                        $row.attr("onclick", "Admin_RemindersSettings.RemindersTemplateEdit('" + item.ReminderVoiceSettingId + "',event,\'voice\');");
                        $row.attr("id", "gvRemindersSettings_row" + item.ReminderVoiceSettingId);
                        $row.attr("ScheduleId", item.ReminderVoiceSettingId);

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

                        $row.append('<td style="display:none;">' + item.ReminderVoiceSettingId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_RemindersSettings.ReminderSettingsDelete(\'' + item.ReminderVoiceSettingId + '\',event,\'voice\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_RemindersSettings.RemindersTemplateEdit(\'' + item.ReminderVoiceSettingId + '\',event,\'voice\');" title="View Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_RemindersSettings.ReminderSettingsActiveInactive(\'' + item.ReminderVoiceSettingId + '\',event,\'Voice\',' + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;</td><td>' + item.ProviderName + '</td><td>' + item.ModifiedOn + '</td>');

                        $("#pnlRemindersSettingsVoice_Result #dgvRemindersSettingsVoice tbody").last().append($row);
                    });
                }
                else {
                    $('#pnlRemindersSettingsVoice_Result #dgvRemindersSettingsVoice').DataTable({
                        "language": {
                            "emptyTable": "No Settings Found"
                        }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }
                if ($.fn.dataTable.isDataTable('#pnlRemindersSettingsVoice_Result #dgvRemindersSettingsVoice'))
                    ;
                else {
                    $("#pnlRemindersSettingsVoice_Result #dgvRemindersSettingsVoice").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ActiveSettingsSearch: function (obj) {


        if ($(obj).is(':checked')) {
            Admin_RemindersSettings.Switch = 1;
        }
        else {
            Admin_RemindersSettings.Switch = 0;
        }


        Admin_RemindersSettings.searchFromButton();
    },

    SearchRemindersSettings: function (RemindersSettings, Id, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new JSON.constructor();
        if (RemindersSettings)
            objData = JSON.parse(RemindersSettings);
        objData["RemindersTemplateId"] = Id;
        objData["RowsPerPage"] = PageNumber;
        objData["PageNumber"] = RowsPerPage;
        objData["IsActive"] = Admin_RemindersSettings.Switch;
        objData["commandType"] = "search_reminders_template";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "Settings");
    },
    ReminderSettingsDelete: function (reminderID, event, Text) {
        if (event != null) {
            event.stopPropagation();
        }
        var strMessage = "";
        var form_type = Text.toLowerCase();
        var form_name = "";
        switch (form_type) {
            case "text":
                form_name = "Reminder Settings_Text";
                break;
            case "email":
                form_name = "Reminder Settings_Email";
                break;
            case "voice":
                form_name = "Reminder Settings_Voice";
                break;
        }

        AppPrivileges.GetFormPrivileges(form_name, "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = reminderID;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_RemindersSettings.DeleteReminderSettings(reminderID, Text).done(function (response) {
                            if (response.status == true) {
                                if (form_type == "text") {
                                    Admin_RemindersSettings.RemindersSettingsSearch();
                                } else if (form_type == "voice") {
                                    Admin_RemindersSettings.RemindersSettingsSearchVoice();
                                } else if (form_type = "email") {
                                    Admin_RemindersSettings.RemindersSettingsSearchEmail();
                                }
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }

                        });
                    }
                }, function () { });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ReminderSettingsActiveInactive: function (reminderID, event, Text, IsActive) {

        if (event != null) {
            event.stopPropagation();
        }
        var strMessage = "";
        var form_type = Text.toLowerCase();
        var form_name = "";
        switch (form_type) {
            case "text":
                form_name = "Reminder Settings_Text";
                break;
            case "email":
                form_name = "Reminder Settings_Email";
                break;
            case "voice":
                form_name = "Reminder Settings_Voice";
                break;
        }


        AppPrivileges.GetFormPrivileges(form_name, "UPDATE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = reminderID;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_RemindersSettings.ReminderSettingsActiveInactive_DbCall(reminderID, form_type, IsActive).done(function (response) {
                            if (response.status == true) {
                                if (form_type == "text") {
                                    Admin_RemindersSettings.RemindersSettingsSearch();
                                } else if (form_type == "voice") {
                                    Admin_RemindersSettings.RemindersSettingsSearchVoice();
                                } else if (form_type = "email") {
                                    Admin_RemindersSettings.RemindersSettingsSearchEmail();
                                }
                                utility.DisplayMessages(response.Message, 1);
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

    ReminderSettingsActiveInactive_DbCall: function (settingID, FormName, IsActive) {


        var objData = new JSON.constructor();
        objData["CommandType"] = "update_reminders_settings_activeinactive";
        objData["SettingId"] = settingID;
        objData["FormName"] = FormName;
        objData["IsActive"] = IsActive;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "Settings");
    },

    DeleteReminderSettings: function (settingID, Text) {
        if (Text == "email") {
            var objData = new JSON.constructor();
            objData["CommandType"] = "delete_email_reminders_settings";
            objData["ReminderEmailSettingId"] = settingID;
            var data = JSON.stringify(objData);
            return MDVisionService.PMSAPIService(data, "Reminders", "Settings");
        }
        else if (Text == "Text") {
            var objData = new JSON.constructor();
            objData["CommandType"] = "delete_text_reminders_settings";
            objData["ReminderTextSettingId"] = settingID;
            var data = JSON.stringify(objData);
            return MDVisionService.PMSAPIService(data, "Reminders", "Settings");
        }
        else if (Text == "voice") {
            var objData = new JSON.constructor();
            objData["CommandType"] = "delete_voice_reminders_settings";
            objData["ReminderVoiceSettingId"] = settingID;
            var data = JSON.stringify(objData);
            return MDVisionService.PMSAPIService(data, "Reminders", "Settings");
        }
    },
    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },

    RemindersSettingsAdd: function () {

        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Provider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        var params = [];
        params["RemindersSettingId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('remindersSettingsDetail', params);
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },
    RemindersTemplateAdd: function () {

        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Provider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        var params = [];
        params["RemindersTemplateId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('remindersSettingsDetail', params);
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});

    },

    RemindersTemplateEdit: function (RemindersSettingsId, event, Type) {
        if (event != null) {
            event.stopPropagation();
        }
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Template_Provider Note Template", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {

        var params = [];
        params["RemindersSettingsId"] = RemindersSettingsId;
        params["Type"] = Type;
        params["mode"] = "Edit";
        params["FromAdmin"] = 0;
        LoadActionPan('remindersSettingsDetail', params);
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    RemindersSettingsSearchEmail: function (RemindersTemplateId, PageNo, rpp) {

        if ($("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").css("display") == "none") {
            $("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").show();
        }

        var self = $("#pnlRemindersSettings_Search");
        var myJSON = self.getMyJSONByName();

        Admin_RemindersSettings.SearchRemindersSettings(myJSON, RemindersTemplateId, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                Admin_RemindersSettings.RemindersEmailSettingsGridLoad(response, PageNo, rpp);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    RemindersSettingsSearchVoice: function (RemindersTemplateId, PageNo, rpp) {

        if ($("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").css("display") == "none") {
            $("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").show();
        }

        var self = $("#pnlRemindersSettings_Search");
        var myJSON = self.getMyJSONByName();

        Admin_RemindersSettings.SearchRemindersSettings(myJSON, RemindersTemplateId, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                Admin_RemindersSettings.RemindersVoiceSettingsGridLoad(response, PageNo, rpp);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    searchFromButton: function () {

        var ref_this = $("#pnlAdminRemindersSettings #divULTabs").find(".active");
        var type = ref_this.attr("type");
        if (type == "text") {
            Admin_RemindersSettings.RemindersSettingsSearch();
        } else if (type == "voice") {
            Admin_RemindersSettings.RemindersSettingsSearchVoice();
        } else if (type = "email") {
            Admin_RemindersSettings.RemindersSettingsSearchEmail();
        }

    },

    TabToggle: function (obj) {

        var type = $(obj).parent().attr('type');
        if (type == "text") {
            $('#pnlAdminRemindersSettings #divTxtSettings').show();
            $('#pnlAdminRemindersSettings #divEmailSettings').hide();
            $('#pnlAdminRemindersSettings #divVoiceSettings').hide();
        } else if (type == "voice") {
            $('#pnlAdminRemindersSettings #divTxtSettings').hide();
            $('#pnlAdminRemindersSettings #divEmailSettings').hide();
            $('#pnlAdminRemindersSettings #divVoiceSettings').show();
        } else if (type = "email") {
            $('#pnlAdminRemindersSettings #divTxtSettings').hide();
            $('#pnlAdminRemindersSettings #divEmailSettings').show();
            $('#pnlAdminRemindersSettings #divVoiceSettings').hide();
        }
    },

    TabPriviliges: function () {


        var Permissions = [];
        Permissions.push('SEARCH');
        var obj_text = {
            FormName: "Reminder Settings_Text",
            Permissions: Permissions
        };

        var obj_email = {
            FormName: "Reminder Settings_Email",
            Permissions: Permissions
        };
        var obj_voice = {
            FormName: "Reminder Settings_Voice",
            Permissions: Permissions
        };
        var data_ = [];
        data_.push(obj_text, obj_email, obj_voice);

        var data_s = "PrivilegeDate=" + JSON.stringify(data_);
        AppPrivileges.GetMultipleFormPrivileges(data_s, "FORM_PRIVILEGE", "GET_MULTIPLE_FORM_PRIVILEGE", function (response) {
            if (response.status != false) {

                var Privilege_Data = response.Privilege_JSON;
                $.each(Privilege_Data, function (i, item) {

                    if (item.FormName == "Reminder Settings_Email") {

                        if (item.PermissionsResponseModel[0].IsAccessible == true) {
                            $('#pnlAdminRemindersSettings #divULTabs #email').removeClass('hidden');
                        }
                        else {
                            $('#pnlAdminRemindersSettings #divULTabs #email').addClass('hidden');
                        }
                    }
                    else if (item.FormName == "Reminder Settings_Text") {

                        if (item.PermissionsResponseModel[0].IsAccessible == true) {
                            $('#pnlAdminRemindersSettings #divULTabs #text').removeClass('hidden');
                        }
                        else {
                            $('#pnlAdminRemindersSettings #divULTabs #text').addClass('hidden');
                        }
                    }
                    else if (item.FormName == "Reminder Settings_Voice") {

                        if (item.PermissionsResponseModel[0].IsAccessible == true) {
                            $('#pnlAdminRemindersSettings #divULTabs #email').removeClass('hidden');
                        }
                        else {
                            $('#pnlAdminRemindersSettings #divULTabs #email').addClass('hidden');
                        }
                    }
                });

                $selected_li = null;
                $('#pnlAdminRemindersSettings #divULTabs li').each(function (i, item) {
                    if ($selected_li == null && $(item).hasClass('hidden') == false) {
                        $selected_li = item;
                        $($selected_li).addClass('active');
                        $('#pnlAdminRemindersSettings ' + $($selected_li).find('a').attr('href')).show();
                    }
                    else {
                        $(item).removeClass('active');
                        $('#pnlAdminRemindersSettings ' + $(item).find('a').attr('href')).hide();
                    }

                });

                Admin_RemindersSettings.searchFromButton();

            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
        //AppPrivileges.GetFormPrivileges("Reminder Settings_Text", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        //        $('#pnlAdminRemindersSettings #divULTabs #text').removeClass('hidden');
        //        $('#pnlAdminRemindersSettings #divULTabs #text').addClass('active');
        //        $('#pnlAdminRemindersSettings #divULTabs #email').removeClass('active');
        //        $('#pnlAdminRemindersSettings #divULTabs #voice').removeClass('active');
        //        $('#pnlAdminRemindersSettings #divTxtSettings').show();
        //        $('#pnlAdminRemindersSettings #divEmailSettings').hide();
        //        $('#pnlAdminRemindersSettings #divVoiceSettings').hide();
        //        if ($("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").css("display") == "none") {
        //            $("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").show();
        //        }
        //        Admin_RemindersSettings.RemindersSettingsSearch();
        //    }
        //    else {
        //        $('#pnlAdminRemindersSettings #divULTabs #text').addClass('hidden');
        //    }
        //});
        //AppPrivileges.GetFormPrivileges("Reminder Settings_Email", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        //        $('#pnlAdminRemindersSettings #divULTabs #email').removeClass('hidden');
        //        $('#pnlAdminRemindersSettings #divULTabs #email').addClass('active');
        //        $('#pnlAdminRemindersSettings #divULTabs #voice').removeClass('active');
        //        $('#pnlAdminRemindersSettings #divULTabs #text').removeClass('active');
        //        $('#pnlAdminRemindersSettings #divTxtSettings').hide();
        //        $('#pnlAdminRemindersSettings #divEmailSettings').show();
        //        $('#pnlAdminRemindersSettings #divVoiceSettings').hide();
        //        if ($("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").css("display") == "none") {
        //            $("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").show();
        //        }
        //        Admin_RemindersSettings.RemindersSettingsSearchEmail();
        //    }
        //    else {
        //        $('#pnlAdminRemindersSettings #divULTabs #email').addClass('hidden');
        //    }
        //});
        //AppPrivileges.GetFormPrivileges("Reminder Settings_Voice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        //        $('#pnlAdminRemindersSettings #divULTabs #voice').removeClass('hidden');
        //        $('#pnlAdminRemindersSettings #divULTabs #voice').addClass('active');
        //        $('#pnlAdminRemindersSettings #divULTabs #email').removeClass('active');
        //        $('#pnlAdminRemindersSettings #divULTabs #text').removeClass('active');
        //        $('#pnlAdminRemindersSettings #divTxtSettings').hide();
        //        $('#pnlAdminRemindersSettings #divEmailSettings').hide();
        //        $('#pnlAdminRemindersSettings #divVoiceSettings').show();
        //        if ($("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").css("display") == "none") {
        //            $("#pnlAdminRemindersSettings #pnlRemindersSettings_Result").show();
        //        }
        //        Admin_RemindersSettings.RemindersSettingsSearchVoice();
        //    }
        //    else {
        //        $('#pnlAdminRemindersSettings #divULTabs #voice').addClass('hidden');
        //    }
        //});
    },
}
