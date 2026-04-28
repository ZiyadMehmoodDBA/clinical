Scheduling_WaitList = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Scheduling_WaitList.params = params;

        var self = $('#pnlScheduleWaitList');
        self.loadDropDowns(true).done(function () {

            //serialize Data.
            $('#frmSchedulingWaitList').data('serialize', $('#frmSchedulingWaitList').serialize());
        });
        Scheduling_WaitList.BindPatientAccount();
        Scheduling_WaitList.WaitListSearch();
    },

    BindPatientAccount: function () {
        var Ctrl = $("#frmSchedulingWaitList #txtAccount");
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var hfCtrl = $('#frmSchedulingWaitList #hfpatientid');
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "value", "contains", null, func, hfCtrl, onSelect);
    },

    WaitListAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Wait List", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["AppointmentId"] = null;
                params["mode"] = "Add";
                LoadActionPan('schwaitlistdetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    // -------------- Patient ---------------------
    ResetPatientValue: function () {
        $('#pnlScheduleWaitList #hfPatientid').val("0");
    },

    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'schTabWaitList';
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, event) {
        if (event != null) {
            event.stopPropagation();
            if (event.target.type == "checkbox") {
                $(':checkbox', this).trigger('click');
                return;
            }
        }
        Scheduling_WaitList.FillPatientAccount(PatientId);
        UnloadActionPan("schTabWaitList");
        utility.InsertRecentPatient(PatientId);
        //$('#frmappointmentDetail').bootstrapValidator('revalidateField', 'patientAccount');
    },

    FillPatientAccount: function (PatientId) {
        var dfd = new $.Deferred();
        Scheduling_WaitList.FillPatient(PatientId).done(function (response) {
            if (response.status != false) {
                var patient_detail = JSON.parse(response.PatientFill_JSON);
                var self = $("#pnlScheduleWaitList");
                utility.bindMyJSON(true, patient_detail, false, self);
                utility.SetKendoAutoCompleteSourceforValidate($("#frmSchedulingWaitList #txtAccount"), patient_detail.txtAccount, $('#frmSchedulingWaitList #hfpatientid'), patient_detail.hfpatientid);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd.promise();
    },
    //----------------------------------------------
    WaitListActiveInactive: function (WaitListId, IsActive) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Wait List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = WaitListId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        schwaitlistdetail.UpdateWaitListActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Scheduling_WaitList.WaitListSearch('0');
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

    WaitListDelete: function (WaitListId, Status, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Wait List", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (Status == "Booked") {
                    utility.DisplayMessages("First Delete Appointment from Schedule", 2);
                }
                else {
                    utility.myConfirm('1', function () {
                        var selectedValue = WaitListId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Scheduling_WaitList.DeleteWaitList(selectedValue).done(function (response) {
                                if (response.status != false) {
                                    //var table1 = $('#dgvWaitList').DataTable();
                                    //table1.row('.active').remove().draw(false);
                                    utility.DisplayMessages(response.Message, 1);
                                    Scheduling_WaitList.WaitListSearch();
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

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    WaitListSearch: function (WaitListId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Wait List", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlScheduleWaitList #pnlWaitList_Result").css("display") == "none") {
                    $("#pnlScheduleWaitList #pnlWaitList_Result").show();
                }

                var self = $("#pnlScheduleWaitList");
                var myJSON = self.getMyJSON();

                Scheduling_WaitList.SearchWaitList(myJSON, WaitListId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {

                        //-----------------Pagination----------

                        if (response.WaitListCount > 0) {
                            $("#pnlScheduleWaitList #divWaitListPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 Record(s)
                            var RecordsPerPage = rpp != null ? rpp : 15;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging("divWaitListPaging", response.iTotalDisplayRecords, 5, "Scheduling_WaitList", CurrentPage, RecordsPerPage);
                            }
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $("#pnlScheduleWaitList #divWaitListPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $("#pnlScheduleWaitList li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else
                                    $(this).removeAttr("class");
                            });
                        }
                        else {
                            $("#pnlScheduleWaitList #divWaitListPaging").css("display", "none");
                        }


                        //--------------------End  Pagination------------

                        Scheduling_WaitList.WaitListGridLoad(response);
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

    EditWaitList: function (WaitListId, Status, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Wait List", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = WaitListId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["WaitListId"] = WaitListId;
                    params["Status"] = Status;
                    params["mode"] = "Edit";
                    LoadActionPan('schwaitlistdetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    WaitListGridLoad: function (response) {
        $('.modal-backdrop').remove();
        $("#dgvWaitList").dataTable().fnDestroy();
        $("#pnlWaitList_Result #dgvWaitList tbody").find("tr").remove();
        if (response.WaitListCount > 0) {
            var WaitListLoadJSONData = JSON.parse(response.WaitListLoad_JSON);
            $.each(WaitListLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvWaitList_row" + item.WaitListId + "')); Scheduling_WaitList.EditWaitList('" + item.WaitListId + "','" + item.Status + "',event);");
                $row.attr("id", "gvWaitList_row" + item.WaitListId);
                $row.attr("WaitListId", item.WaitListId);
                if (item.IsActive == "1") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                // $row.append('<td style="display:none;">' + item.WaitListId + '</td><td><a class="btn  btn-xs" href="#" onclick="Scheduling_WaitList.WaitListDelete(' + item.WaitListId + ',\'' + item.Status + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Scheduling_WaitList.EditWaitList(' + item.WaitListId + ',\'' + item.Status + '\');" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Scheduling_WaitList.WaitListActiveInactive(' + item.WaitListId + ', ' + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td> <td>' + item.AccountNumber + '</td> <td>' + item.FacilityName + '</td> <td>' + item.ProviderName + '</td> <td>' + item.ResourceName + '</td> <td>' + item.FromDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td> <td>' + item.PreferredDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td> <td>' + item.PreferedTime + '</td> <td>' + item.Status + '</td>');
                $row.append('<td style="display:none;">' + item.WaitListId + '</td><td><a class="btn  btn-xs" href="#" onclick="Scheduling_WaitList.WaitListDelete(' + item.WaitListId + ',\'' + item.Status + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Scheduling_WaitList.EditWaitList(' + item.WaitListId + ',\'' + item.Status + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;</td> <td>' + item.AccountNumber + '</td> <td>' + item.PatLastName + '</td><td>' + item.PatFirstName + '</td> <td>' + item.FacilityName + '</td> <td>' + item.ProviderName + '</td> <td>' + item.ResourceName + '</td> <td>' + item.FromDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ToDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td> <td>' + item.PreferredDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td> <td>' + item.PreferedTime + '</td> <td>' + item.Status + '</td><td>' + item.CreatedOn.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td>');
                $("#pnlWaitList_Result #dgvWaitList tbody").last().append($row);
            });
        }
        else {
            $("#pnlScheduleWaitList #divWaitListPaging").css("display", "none");
            $('#dgvWaitList').DataTable({
                "language": {
                    "emptyTable": "No Wait List Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvWaitList'))
            ;
        else
            $("#pnlWaitList_Result #dgvWaitList").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    FillPatient: function (PatientID) {
        var data = "PatientID=" + PatientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_WAITLIST", "FILL_PATIENT");
    },

    SearchWaitList: function (WaitListData, WaitListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "WaitListData=" + WaitListData + "&WaitListId=" + WaitListId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_WAITLIST", "SEARCH_WAIT_LIST");
    },

    LoadActivePatients: function (AccountNo) {
        var data = "AccountNo=" + AccountNo;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "SEARCH_PATIENT");
    },

    DeleteWaitList: function (WaitListId) {
        var data = "WaitListId=" + WaitListId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_WAITLIST_DETAIL", "DELETE_WAITLIST");
    },
    //-----------Pagination Functions-----------
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlWaitList_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Scheduling_WaitList.WaitListSearch(0, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlWaitList_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Scheduling_WaitList.WaitListSearch(0, currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlWaitList_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Scheduling_WaitList.WaitListSearch(0, currentPageNo, 15);
        }
    },

}