Outstanding_Visit = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Outstanding_Visit.params = params;
        if (Outstanding_Visit.bIsFirstLoad) {
            Outstanding_Visit.bIsFirstLoad = false;
            if (Outstanding_Visit.params.PanelID != null && Outstanding_Visit.params.PanelID != 'pnlOutstandingVisit')
                Outstanding_Visit.params.PanelID = Outstanding_Visit.params.PanelID + ' #pnlOutstandingVisit';
            else
                Outstanding_Visit.params.PanelID = "pnlOutstandingVisit";

            var self = $('#' + Outstanding_Visit.params.PanelID);

            if (Outstanding_Visit.params.PatientOutstanding == "1") {
                self.find("#headingTitle").text("Patient Outstanding Visits");
            }
            else if (Outstanding_Visit.params.PatientOutstanding == "0") {
                self.find("#headingTitle").text("Insurance Outstanding Visits");
            }

            //self.loadDropDowns(true).done(function () {

            //});


            //Outstanding_Visit.LoadAllAutocomplete();
        }
        Outstanding_Visit.OutstandingVisitSearch(0);
    },

    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            $('#' + Outstanding_Visit.params["PanelID"] + " input#txtProvider").autocomplete({
                autoFocus: true,
                source: Providers, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Outstanding_Visit.params["PanelID"] + " #hfProvider").val(ui.item.id); // add the selected id
                        if ($("#" + Outstanding_Visit.params["PanelID"] + " #lnkProviderEdit").css("display") == "none") {
                            $("#" + Outstanding_Visit.params["PanelID"] + " #lnkProviderEdit").css("display", "inline");
                            $("#" + Outstanding_Visit.params["PanelID"] + " #lblProvider").css("display", "none");
                        }
                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            $('#' + Outstanding_Visit.params["PanelID"] + " input#txtFacility").autocomplete({
                autoFocus: true,
                source: Facilities, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Outstanding_Visit.params["PanelID"] + " #hfFacility").val(ui.item.id); // add the selected id
                        if ($("#" + Outstanding_Visit.params["PanelID"] + " #lnkFacilityEdit").css("display") == "none") {
                            $("#" + Outstanding_Visit.params["PanelID"] + " #lnkFacilityEdit").css("display", "inline");
                            $("#" + Outstanding_Visit.params["PanelID"] + " #lblFacility").css("display", "none");
                        }
                    }, 100);
                }
            });
        });
    },

    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "patTabEncounter";
        if (Outstanding_Visit.params.PanelID != 'pnlEncounter')
            LoadActionPan('Admin_Provider', params, Outstanding_Visit.params.PanelID);
        else
            LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#' + demographicDetail.params.PanelID + ' #hfProvider').val(), "demographicDetail");
        var params = [];
        params["ProviderId"] = $('#' + Outstanding_Visit.params["PanelID"] + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'patTabEncounter';
        LoadActionPan('providerDetail', params);
    },

    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "patTabEncounter";
        if (Outstanding_Visit.params.PanelID != 'pnlEncounter')
            LoadActionPan('Admin_Facility', params, Outstanding_Visit.params.PanelID);
        else
            LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#' + Outstanding_Visit.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'patTabEncounter';
        LoadActionPan('facilityDetail', params);
    },

    OutstandingVisitSearch: function (VisitId, PageNo, rpp, VisitStatus) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Encounter", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Outstanding_Visit.params["PanelID"] + " #pnlOutstandingVisit_Result").css("display") == "none") {
                    $("#" + Outstanding_Visit.params["PanelID"] + " #pnlOutstandingVisit_Result").css("display", "inline");
                }

                var self = $("#" + Outstanding_Visit.params["PanelID"]);
                var myJSON = self.getMyJSON();
                //Outstanding_Visit.OutstandingVisitGridLoad(null, PageNo, rpp);
                Outstanding_Visit.SearchVisits(Outstanding_Visit.params.patientID, Outstanding_Visit.params.PatientOutstanding, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Outstanding_Visit.OutstandingVisitGridLoad(response, PageNo, rpp);
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

    VisitEdit: function (VisitId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Encounter", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = VisitId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["PatientId"] = Outstanding_Visit.params.patientID;
                    params["VisitId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('basicFeeScheduleDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OutstandingVisitGridLoad: function (response, PageNo, rpp) {

        $("#" + Outstanding_Visit.params["PanelID"] + " #pnlOutstandingVisit_Result #dgvOutstandingVisit").dataTable().fnDestroy();

        $("#" + Outstanding_Visit.params["PanelID"] + " #pnlOutstandingVisit_Result #dgvOutstandingVisit tbody").find("tr").remove();

        if (response.VisitsCount > 0) {
            var VisitsLoadJSONData = JSON.parse(response.VisitsLoad_JSON);
            $.each(VisitsLoadJSONData, function (i, item) {
                var gridId = "";
                if (item.Status.toLowerCase() != "checkout")
                    gridRowId = "gvOutstandingVisit_row";
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#" + gridId + item.VisitId + "'))");
                $row.attr("id", gridId + item.VisitId);
                $row.attr("VisitId", item.VisitId);

                if (item.IsActive == 1) {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var EditMethod = "";//"SelectTab('EncounterTabVisit_Detail', false, true, '" + item.VisitId + "','" + utility.RemoveTimeFromDate(null, item.CreatedOn) + "')";

                var patientBalance = item.PatBal != "" ? parseFloat(item.PatBal).toFixed(2) : 0;
                patientBalance = patientBalance < 0 ? "(" + patientBalance + ")" : patientBalance;

                var insuranceBalance = item.InsBal != "" ? parseFloat(item.InsBal).toFixed(2) : 0;
                insuranceBalance = insuranceBalance < 0 ? "(" + insuranceBalance + ")" : insuranceBalance;

                var ActiveInacvtiveMethod = "Outstanding_Visit.VisitActiveInactive('" + item.VisitId.trim() + "'," + isactive + ",event);";
                actions = '<td style="display:none;"><a class="btn  btn-xs" href="#" onclick="Outstanding_Visit.VisitDelete(\'' + item.VisitId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="' + EditMethod + '" title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;</td>';
                $row.append('<td style="display:none;">' + item.VisitId + '</td>' + actions + '<td>' + item.ClaimNumber + '</td><td>' + utility.RemoveTimeFromDate(null, item.AppointmentDate) + '</td><td>' + item.FacilityName + '</td><td>' + item.ProviderName + '</td><td>' + item.Status + '</td><td>' + item.CheckInDateTime + '<td></td>' + item.CheckInBy + '</td><td>' + patientBalance + "</td><td>" + insuranceBalance + "</td>");

                //if (item.Status == "CheckOut") 
                //    $("#" + Outstanding_Visit.params["PanelID"] + " div#pnlVisits_Result div#CloseVisits #dgvCloseVisitsDetail tbody").last().append($row);
                // else
                //if (item.Status.toLowerCase() != "checkout")
                $("#" + Outstanding_Visit.params["PanelID"] + " #pnlOutstandingVisit_Result #dgvOutstandingVisit tbody").last().append($row);

            });

            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            //var PagesToShow = Math.ceil(response.OpenVisitsCount / RecordsPerPage);
            //var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            //if (PageNo == null) {
            //    utility.GetCustomPaging("" + Outstanding_Visit.params["PanelID"] + " #divOpenVisitsPaging", response.OpenVisitsCount, 5, "Outstanding_Visit", CurrentPage, RecordsPerPage);
            //}
            //var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.OpenVisitsCount ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.OpenVisitsCount;
            //var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.OpenVisitsCount + " Record(s)";
            //$("#" + Outstanding_Visit.params["PanelID"] + " #divOpenVisitsPaging #divShowingEntries").text(showingText);
            //// Change Background Color to Black for selected page
            //$("#" + Outstanding_Visit.params["PanelID"] + " #pnlVisits_Result #divOpenVisitsPaging li").each(function () {
            //    if ($(this).text() == CurrentPage) {
            //        $(this).attr("class", "active");
            //    }
            //    else
            //        $(this).removeAttr("class");
            //});
            //------------End Pagination-------

        }
        else {
            $("#" + Outstanding_Visit.params["PanelID"] + " #pnlOutstandingVisit_Result #dgvOutstandingVisit").DataTable({
                "language": {
                    "emptyTable": "No Outstanding Visit Found"
                }, "autoWidth": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Outstanding_Visit.params["PanelID"] + " #pnlOutstandingVisit_Result #dgvOutstandingVisit"))
            ;
        else
            $("#" + Outstanding_Visit.params["PanelID"] + " #pnlOutstandingVisit_Result #dgvOutstandingVisit").DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "bFilter": true, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    VisitActiveInactive: function (VisitId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Encounter", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = VisitId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Outstanding_Visit.UpdateActiveInactiveVisits(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Outstanding_Visit.OutstandingVisitSearch();
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

    VisitDelete: function (VisitId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Encounter", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = VisitId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Outstanding_Visit.DeleteVisit(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $("#" + Outstanding_Visit.params["PanelID"] + " div#pnlVisits_Result div#OpenVisits #dgvOpenVisitsDetail").DataTable();
                                var table2 = $("#" + Outstanding_Visit.params["PanelID"] + " div#pnlVisits_Result div#CloseVisits #dgvCloseVisitsDetail").DataTable();
                                table1.row('.active').remove().draw(false);
                                table2.row('.active').remove().draw(false);
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

    SearchVisits: function (PatientID, PatientOutstanding, PageNumber, RowsPerPage) {
        if (PatientOutstanding == null) {
            PatientOutstanding = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "PatientID=" + PatientID + "&PatientOutstanding=" + PatientOutstanding + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_OUTSTANDING_VISIT", "SEARCH_OUTSTANDING_VISITS");
    },

    SaveVisit: function (VisitData, PatientID) {
        var data = "VisitData=" + VisitData + "&PatientID=" + PatientID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_Outstanding_Visit", "SAVE_VISIT");
    },

    UpdateVisit: function (VisitData, VisitID, PatientID) {
        var data = "VisitData=" + VisitData + "&VisitID=" + VisitID + "&PatientID=" + PatientID;//Outstanding_Visit.params.patientID
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_Outstanding_Visit", "UPDATE_VISIT");
    },

    UpdateActiveInactiveVisits: function (VisitID, IsActive) {
        var data = "VisitID=" + VisitID + "&PatientID=" + Outstanding_Visit.params.patientID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_Outstanding_Visit", "UPDATE_VISIT_ACTIVE_INACTIVE");
    },

    DeleteVisit: function (VisitID) {
        var data = "VisitID=" + VisitID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_Outstanding_Visit", "DELETE_VISITS");
    },

    removeDialogClasses: function () {
        $('#' + Outstanding_Visit.params.PanelID + ' .modal-header').hide();
        $('#' + Outstanding_Visit.params.PanelID + ' #modalbody').removeClass('panel-body');
        $('#' + Outstanding_Visit.params.PanelID + ' .modal-content').removeClass('modal-content');
        $('#' + Outstanding_Visit.params.PanelID + ' .modal-dialog-full').removeAttr('class');
    },

    UnLoad: function () {
        if (Outstanding_Visit.params != null && Outstanding_Visit.params.ParentCtrl != null) {
            UnloadActionPan(Outstanding_Visit.params.ParentCtrl, 'Outstanding_Visit');
        }
        else
            UnloadActionPan(null, 'Outstanding_Visit');
    },

    //--------------Pagination functions-----

    SelectedPageClick: function (PageNo, objPage, TotalRecords, rpp, pagingDivId) {
        // Change Background Color to Black for selected page

        //if (pagingDivId == "divCloseVisitsPaging") {
        //    var current_li = $(objPage).closest("li");
        //    $("#pnlVisits_Result #divCloseVisitsPaging li").each(function () {
        //        if ($(objPage).closest("li") == current_li) {
        //            current_li.addClass('active');
        //        }
        //        else
        //            $(this).removeAttr("class");

        //    });
        //}
        //var current_li = $(objPage).closest("li");
        //current_li.addClass('active');

        if (pagingDivId == Outstanding_Visit.params["PanelID"] + " #divCloseVisitsPaging") {
            Outstanding_Visit.OutstandingVisitSearch(0, PageNo, 15, '1');
        }
        else if (pagingDivId == Outstanding_Visit.params["PanelID"] + " #divOpenVisitsPaging") {
            Outstanding_Visit.OutstandingVisitSearch(0, PageNo, 15, '0');
        }

    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Outstanding_Visit.params["PanelID"] + " #pnlVisits_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            if (pagingDivId == Outstanding_Visit.params["PanelID"] + " #divCloseVisitsPaging") {
                Outstanding_Visit.OutstandingVisitSearch(0, currentPageNo, 15, '1');
            }
            else if (pagingDivId == Outstanding_Visit.params["PanelID"] + " #divOpenVisitsPaging") {
                Outstanding_Visit.OutstandingVisitSearch(0, currentPageNo, 15, '0');
            }
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Outstanding_Visit.params["PanelID"] + " #pnlVisits_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            if (pagingDivId == Outstanding_Visit.params["PanelID"] + " #divCloseVisitsPaging") {
                Outstanding_Visit.OutstandingVisitSearch(0, currentPageNo, 15, '1');
            }
            else if (pagingDivId == Outstanding_Visit.params["PanelID"] + " #divOpenVisitsPaging") {
                Outstanding_Visit.OutstandingVisitSearch(0, currentPageNo, 15, '0');
            }
        }
    },

    //---------------Open Pagination-------
}