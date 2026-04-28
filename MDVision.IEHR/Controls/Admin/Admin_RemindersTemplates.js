Admin_RemindersTemplates = {
    bIsFirstLoad: true,
    params: [],
    bIsFirstLoad: true,
    Switch: true,
    Load: function (params) {
        if (Admin_RemindersTemplates.bIsFirstLoad) {
            Admin_RemindersTemplates.bIsFirstLoad = false;
            var self = $('#pnlAdminRemindersTemplates');
            self.loadDropDowns(true).done(function () {
                Admin_RemindersTemplates.IntializeMultiSelectDropDown();
                Admin_RemindersTemplates.IntializeMultiSelectDropDownProviders();
            });


            Admin_RemindersTemplates.RemindersTemplatesSearch();


        }
    },
    openreminder: function (obj) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Reminders Templates", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var screenid = $(obj).attr('id');
                var params = [];
                params["RemindersTemplateId"] = "-1";
                params["ScreenType"] = screenid;
                params["mode"] = "Add";
                LoadActionPan('remindersDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#pnlAdminRemindersTemplates #ddlNotesTemplateProvider').multiselect('destroy');
        $('#pnlAdminRemindersTemplates #ddlNotesTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            //   onChange: function (option, checked, select) {
            //   Admin_RemindersTemplates.checkSpecialtiesByProviderId(option, checked, select);
            // },
            onDropdownHide: function (event) {
                // Clinical_Provider_Note_Template.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('refresh');
            },


        });
    },
    IntializeMultiSelectDropDown: function () {


        $('#pnlAdminRemindersTemplates #ddlNotesTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 116

        });

    },
    RemindersTemplatesSearch: function (RemindersTemplateId, PageNo, rpp) {

        if ($("#pnlAdminRemindersTemplates #pnlRemindersTemplates_Result").css("display") == "none") {
            $("#pnlAdminRemindersTemplates #pnlProviderSchedule_Result").show();
        }

        var self = $("#pnlRemindersTemplates_Search");
        var myJSON = self.getMyJSONByName();
        var selected_ddlType = self.find('#ddlReminderType option:selected').text();

        var form_name = "";
        switch (selected_ddlType) {
            case "Text":
                form_name = "Reminder Templates_Text";
                break;
            case "Email":
                form_name = "Reminder Templates_Email";
                break;
            case "Voice":
                form_name = "Reminder Templates_Voice";
                break;
            case "- Select -":
                form_name = "no form"
                break;
        }
        if (form_name != "no form") {
            AppPrivileges.GetFormPrivileges(form_name, "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_RemindersTemplates.SearchRemindersTemplates(myJSON, RemindersTemplateId, PageNo, rpp).done(function (response) {
                        if (response.status != false) {
                            Admin_RemindersTemplates.RemindersTemplatesGridLoad(response);
                            var TableControl = "pnlAdminRemindersTemplates #dgvRemindersTemplates";
                            var PagingPanelControlID = "pnlAdminRemindersTemplates #divRemindersTemplatesPaging";
                            var ClassControlName = "Admin_RemindersTemplates";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(CreatePagination(response.RemindersTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                                function (PrimaryID, PageNumber, ResultPerPage) {
                                    Admin_RemindersTemplates.RemindersTemplatesSearch(PrimaryID, PageNumber, ResultPerPage);
                                }), 10);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else { 
            Admin_RemindersTemplates.SearchRemindersTemplates(myJSON, RemindersTemplateId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    Admin_RemindersTemplates.RemindersTemplatesGridLoad(response);
                    var TableControl = "pnlAdminRemindersTemplates #dgvRemindersTemplates";
                    var PagingPanelControlID = "pnlAdminRemindersTemplates #divRemindersTemplatesPaging";
                    var ClassControlName = "Admin_RemindersTemplates";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.RemindersTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                        function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_RemindersTemplates.RemindersTemplatesSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    RemindersTemplatesGridLoad: function (response) {
        $("#dgvRemindersTemplates").dataTable().fnDestroy();
        $("#pnlRemindersTemplates_Result #dgvRemindersTemplates tbody").find("tr").remove();
        if (response.RemindersTemplateCount > 0) {
            var RemindersTemplatesLoadJSONData = JSON.parse(response.RemindersTemplateLoad_JSON);
            $.each(RemindersTemplatesLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_RemindersTemplates.RemindersTemplateEdit('" + item.RemindersTemplateId + "',event);");
                $row.attr("id", "gvRemindersTemplates_row" + item.RemindersTemplateId);
                $row.attr("ScheduleId", item.RemindersTemplateId);

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

                $row.append('<td style="display:none;">' + item.RemindersTemplateId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_RemindersTemplates.ReminderTemplatesDelete(\'' + item.RemindersTemplateId + '\', \'' + item.ReminderTemplateType + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_RemindersTemplates.RemindersTemplateEdit(\'' + item.RemindersTemplateId + '\',event);" title="View Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_RemindersTemplates.RemindersTemplateActiveInactive(' + item.RemindersTemplateId + ', ' + isactive + ', \'' + item.ReminderTemplateType + '\',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;</td><td>' + item.RemindersTemplateName + '</td><td>' + item.TypeName + '</td><td>' + item.ModifiedOn + '</td>');

                $("#pnlRemindersTemplates_Result #dgvRemindersTemplates tbody").last().append($row);
            });
        }
        else {
            $('#pnlRemindersTemplates_Result #dgvRemindersTemplates').DataTable({
                "language": {
                    "emptyTable": "No Reminder Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlRemindersTemplates_Result #dgvRemindersTemplates'))
            ;
        else {
            $("#pnlRemindersTemplates_Result #dgvRemindersTemplates").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown


        }
        var isactive;
        var checked;
        if (Admin_RemindersTemplates.Switch == 1) {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "0";
            checked = '';
        }

        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                     '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Admin_RemindersTemplates.ActiveTemplatesSearch(this);">' +
                      '</div><span class="pl-xs">Active</span>';
        $('#pnlAdminRemindersTemplates .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        //-----------------------
        $('#pnlAdminRemindersTemplates #dgvRemindersTemplates').dataTable().fnSettings().aoColumns[0].bSortable = false;
        //----------------------------------
        utility.SwicthWidgetInializatoin();
    },

    RemindersTemplateActiveInactive: function (Id, IsActive, reminderType, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        var form_name = "";
        reminderType = reminderType.toLowerCase();
        switch (reminderType) {
            case "text":
                form_name = "Reminder Templates_Text";
                break;
            case "email":
                form_name = "Reminder Templates_Email";
                break;
            case "voice":
                form_name = "Reminder Templates_Voice";
                break;
        }
        AppPrivileges.GetFormPrivileges(form_name, "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = Id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_RemindersTemplates.UpdatePlaceOfServiceActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Admin_RemindersTemplates.RemindersTemplatesSearch('0');
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
    UpdatePlaceOfServiceActiveInactive: function (reminderID, IsActive) {

        var objData = new JSON.constructor();
        objData["CommandType"] = "activeinactive_reminders_template";
        objData["RemindersTemplateId"] = reminderID;
        objData["IsActive"] = IsActive;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "Reminders");
    },
    ActiveTemplatesSearch: function (obj) {


        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Admin_RemindersTemplates.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Admin_RemindersTemplates.Switch = 1;
        }

        Admin_RemindersTemplates.RemindersTemplatesSearch();
    },

    SearchRemindersTemplates: function (RemindersTemplates, Id, PageNumber, RowsPerPage) {

        //var reminderTempType = "";
        //if ($("#pnlAdminRemindersTemplates #ulTemplateTypeTabs #liTypeText").hasClass('active')) {
        //    reminderTempType = "text";
        //}
        //if ($("#pnlAdminRemindersTemplates #ulTemplateTypeTabs #liTypeVoice").hasClass('active')) {
        //    reminderTempType = "voice";
        //}
        //if ($("#pnlAdminRemindersTemplates #ulTemplateTypeTabs #liTypeEmail").hasClass('active')) {
        //    reminderTempType = "email";
        //}

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new JSON.constructor();
        if (RemindersTemplates)
            objData = JSON.parse(RemindersTemplates);
        objData["RemindersTemplateId"] = Id;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["IsActive"] = Admin_RemindersTemplates.Switch;
        //objData["ReminderTemplateType"] = reminderTempType;
        objData["commandType"] = "search_reminders_template";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "Reminders");
    },
    ReminderTemplatesDelete: function (reminderID, reminderType, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        var form_name = "";
        reminderType = reminderType.toLowerCase();
        switch (reminderType) {
            case "text":
                form_name = "Reminder Templates_Text";
                break;
            case "email":
                form_name = "Reminder Templates_Email";
                break;
            case "voice":
                form_name = "Reminder Templates_Voice";
                break;
        }

        AppPrivileges.GetFormPrivileges(form_name, "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = reminderID;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_RemindersTemplates.DeleteReminderTemplates(reminderID).done(function (response) {
                            if (response.status == true) {
                                Admin_RemindersTemplates.RemindersTemplatesSearch();
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
    DeleteReminderTemplates: function (reminderID) {

        var objData = new JSON.constructor();
        objData["CommandType"] = "delete_reminders_template";
        objData["RemindersTemplateId"] = reminderID;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Reminders", "Reminders");
    },
    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },

    RemindersTemplateAdd: function () {

        var reminderTempType = "";
        if ($("#pnlAdminRemindersTemplates #ulTemplateTypeTabs #liTypeText").hasClass('active')) {
            reminderTempType = "text";
        }
        if ($("#pnlAdminRemindersTemplates #ulTemplateTypeTabs #liTypeVoice").hasClass('active')) {
            reminderTempType = "voice";
        }
        if ($("#pnlAdminRemindersTemplates #ulTemplateTypeTabs #liTypeEmail").hasClass('active')) {
            reminderTempType = "email";
        }

        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Reminders Templates", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        var params = [];
        params["RemindersTemplateId"] = "-1";
        params["mode"] = "Add";
        params["ReminderTemplateType"] = reminderTempType;
        LoadActionPan('remindersTemplatesDetail', params);
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});

    },

    RemindersTemplateEdit: function (RemindersTemplateId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Reminders Templates", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {

        var params = [];
        params["RemindersTemplateId"] = RemindersTemplateId;
        params["mode"] = "Edit";
        params["FromAdmin"] = 0;
        LoadActionPan('remindersTemplatesDetail', params);
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },
}
