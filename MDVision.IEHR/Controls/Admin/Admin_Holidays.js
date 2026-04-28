Admin_Holidays = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_Holidays.bIsFirstLoad) {
            Admin_Holidays.bIsFirstLoad = false;
            var self = $('#pnlAdminHolidays');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {               
                //if (globalAppdata['IsAdmin'] != "True") {
                //    $("#pnlHolidays_Search #divHoliday_Entity").css("display", "none");
                //    $("#pnlHolidays_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                   
                //}
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }

                utility.CreateDatePicker('pnlAdminHolidays #holidayDate', function () {
                    //on-change callback method 
                },false);

            });
            AppPrivileges.GetFormPrivileges("Holidays", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_Holidays.HolidaysSearch();
                }
            });
        }
    },

    HolidaysAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Holidays", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["HolidaysId"] = null;
                params["mode"] = "Add";

                LoadActionPan('holidaysDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    HolidaysEdit: function (HolidaysId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvHolidays_row' + HolidaysId));
        AppPrivileges.GetFormPrivileges("Holidays", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = HolidaysId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["HolidaysId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('holidaysDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    HolidaysDelete: function (HolidaysId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvHolidays_row' + HolidaysId));
        AppPrivileges.GetFormPrivileges("Holidays", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = HolidaysId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_Holidays.DeleteHolidays(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvHolidays').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_Holidays.HolidaysSearch();
                                utility.DisplayMessages(response.Message, 1);
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

    HolidaysActiveInactive: function (HolidaysId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Holidays", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = HolidaysId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        holidaysDetail.UpdateScheduleHolidaysActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_Holidays.HolidaysSearch('0');
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

    HolidaysSearch: function (HolidaysId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Holidays", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminHolidays #pnlHolidays_Result").css("display") == "none") {
                    $("#pnlAdminHolidays #pnlHolidays_Result").show();
                }

                var self = $("#pnlHolidays_Search");
                var myJSON = self.getMyJSON();

                Admin_Holidays.SearchHolidays(myJSON, HolidaysId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_Holidays.HolidaysGridLoad(response);
                        var TableControl = "pnlAdminHolidays #dgvHolidays";
                        var PagingPanelControlID = "pnlAdminHolidays #divHolidaysPaging";
                        var ClassControlName = "Admin_Holidays";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.HolidaysCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_Holidays.HolidaysSearch(PrimaryID, PageNumber, ResultPerPage);
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
    },

    HolidaysGridLoad: function (response) {
        $("#dgvHolidays").dataTable().fnDestroy();
        $("#pnlHolidays_Result #dgvHolidays tbody").find("tr").remove();
        if (response.HolidaysCount > 0) {
            var HolidaysLoadJSONData = JSON.parse(response.HolidaysLoad_JSON);
            $.each(HolidaysLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_Holidays.HolidaysEdit('" + item.ScheduleHolidayId + "',event);");
                $row.attr("id", "gvHolidays_row" + item.ScheduleHolidayId);
                $row.attr("HolidaysId", item.ScheduleHolidayId);

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

                $row.append('<td style="display:none;">' + item.ScheduleHolidayId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_Holidays.HolidaysDelete(' + item.ScheduleHolidayId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Holidays.HolidaysEdit("' + item.ScheduleHolidayId + '",event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Holidays.HolidaysActiveInactive(' + item.ScheduleHolidayId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.HolidayOn.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.HolidayDescription + '</td><td>' + item.EntityName + '</td>');

                $("#pnlHolidays_Result #dgvHolidays tbody").last().append($row);
            });
        }
        else {
            $('#pnlHolidays_Result #dgvHolidays').DataTable({
                "language": {
                    "emptyTable": "No Holidays Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlHolidays_Result #dgvHolidays'))
            ;
        else {
            $("#pnlHolidays_Result #dgvHolidays").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
           // $("#pnlHolidays_Result #dgvHolidays").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

        }
    },

    SearchHolidays: function (HolidaysData, HolidaysId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "HolidaysData=" + HolidaysData + "&HolidaysID=" + HolidaysId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_HOLIDAYS", "SEARCH_HOLIDAYS");
    },

    DeleteHolidays: function (HolidaysId) {
        var data = "HolidaysID=" + HolidaysId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_HOLIDAYS_DETAIL", "DELETE_HOLIDAYS");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}