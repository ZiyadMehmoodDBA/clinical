Scheduling_Search = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Scheduling_Search.bIsFirstLoad) {
            Scheduling_Search.bIsFirstLoad = false;
            var self = $('#pnlScheduleSearch');
            self.loadDropDowns(true).done(function () {

                //Scheduling_Search.LoadDefaultData();
                Scheduling_Search.LoadAllAutocomplete();
                Scheduling_Search.ValidateScheduleSearch();
                if ($("#PatientProfile #hfPatientId").val() != "") {
                    Scheduling_Search.FillPatientInfoFromSearch($("#PatientProfile #hfPatientId").val());
                }
            });

        }

        Scheduling_Search.validateSearchDate();
        $('#pnlScheduleSearch #tpFromTime').val('');
        $('#pnlScheduleSearch #tpToTime').val('');
    },

    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            $("#frmSchedulingSearch input#txtProvider").autocomplete({
                autoFocus: true,
                source: Providers, // pass an array
                select: function (event, ui) {

                    setTimeout(function () {
                        $("#frmSchedulingSearch #hfProvider").val(ui.item.id);
                        if ($("#pnlScheduleSearch #lnkProviderEdit").css("display") == "none") {
                            $("#pnlScheduleSearch #lnkProviderEdit").css("display", "inline");
                            $("#pnlScheduleSearch #lblProvider").css("display", "none");
                        }
                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            $("#pnlScheduleSearch #frmSchedulingSearch input#txtFacility").autocomplete({
                autoFocus: true,
                source: Facilities, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        var facid = ui.item.id;
                        $('#pnlScheduleSearch #frmSchedulingSearch #hfFacilityId').val(facid);
                        $('#pnlScheduleSearch #frmSchedulingSearch #txtPractice').val(ui.item.Practice);
                        $('#pnlScheduleSearch #frmSchedulingSearch #hfPractice').val(ui.item.PracticeId);
                        if ($("#pnlScheduleSearch #lnkFacilityEdit").css("display") == "none") {
                            $("#pnlScheduleSearch #lnkFacilityEdit").css("display", "inline");
                            $("#pnlScheduleSearch #lblFacility").css("display", "none");
                        }
                        $("#pnlScheduleSearch #frmSchedulingSearch").bootstrapValidator('revalidateField', 'Facility');
                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetResources', false).done(function (result) {
            $("#frmSchedulingSearch input#txtResource").autocomplete({
                autoFocus: true,
                source: Resources, // pass an array
                select: function (event, ui) {

                    setTimeout(function () {
                        $("#frmSchedulingSearch #hfResource").val(ui.item.id);
                        if ($("#pnlScheduleSearch #lnkResourceEdit").css("display") == "none") {
                            $("#pnlScheduleSearch #lnkResourceEdit").css("display", "inline");
                            $("#pnlScheduleSearch #lblResource").css("display", "none");
                        }
                    }, 100);
                }
            });
        });

    },

    BindPatientAccount: function () {

        var AccountNo = $('#pnlScheduleSearch #txtAccountNo').val();
        var AllPatients = utility.GetPatientArray(AccountNo, 1).done(function (response) {

            $("#pnlScheduleSearch #txtAccountNo").autocomplete({
                autoFocus: true,
                source: response, // pass an array (without a comma)
                select: function (event, ui) {
                    setTimeout(function () {
                        var patientid = ui.item.id;
                        $('#frmSchedulingSearch #hfPatientid').val(patientid);
                        $('#frmSchedulingSearch #txtAccountNo').val(ui.item.value);
                        utility.InsertRecentPatient(ui.item.id);
                    }, 100);

                }
            });

            $("#pnlScheduleSearch #txtAccountNo").autocomplete("search");
            $("#pnlScheduleSearch #txtAccountNo").focus();
        });

    },

    // -------------- Facility ---------------------
    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "schTabSearch";
        params["IsOptional"] = true;
        params["RefForm"] = "frmSchedulingSearch";
        LoadActionPan('Admin_Facility', params);
    },


    // -------------- Patient ---------------------

    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'schTabSearch';
        LoadActionPan('Patient_Search', params);
    },

    // -------------- Provider ---------------------

    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "schTabSearch";
        LoadActionPan('Admin_Provider', params);
    },

    // -------------- Resource ---------------------

    OpenResource: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmSchedulingSearch";
        params["ResourceId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "schTabSearch";
        LoadActionPan('Admin_Resources', params);
    },

    // -------------- Practice ---------------------

    OpenPractice: function () {
        var params = [];
        //params["ResourceId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "schTabSearch";
        LoadActionPan('Admin_Practice', params);
    },


    ResetPatientValue: function () {
        $('#pnlScheduleSearch #hfPatientid').val("0");
    },

    ResetProviderValue: function () {
        $('#pnlScheduleSearch #hfProvider').val("0");
    },

    ResetResourceValue: function () {
        $('#pnlScheduleSearch #hfResource').val("0");
    },

    ResetFacilityValue: function () {
        $('#pnlScheduleSearch #hfFacilityId').val("0");
        $('#pnlScheduleSearch #hfPractice').val("0");

        $('#pnlScheduleSearch #txtPractice').val("");
    },

    ResetPracticeValue: function () {
        $('#pnlScheduleSearch #hfPractice').val("0");
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNo, FName, LName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        Scheduling_Search.FillPatientAccount(PatientId);
        //$('#frmSchedulingSearch #hfPatientid').val(PatientId);
        //$('#frmSchedulingSearch #txtAccountNo').val(AccountNo + " - " + FName + ", " + LName);
        UnloadActionPan("schTabSearch");
        utility.InsertRecentPatient(PatientId);
        //$('#frmappointmentDetail').bootstrapValidator('revalidateField', 'PatientAccount');
    },

    FillPatientAccount: function (PatientId) {
        var dfd = new $.Deferred();
        Scheduling_Search.FillPatient(PatientId).done(function (response) {
            if (response.status != false) {
                var patient_detail = JSON.parse(response.PatientFill_JSON);
                var self = $("#pnlScheduleSearch");
                utility.bindMyJSON(true, patient_detail, false, self);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd.promise();
    },

    BlockSlot: function () {

        var selected = [];
        var slotids = "";
        var $row = $('<tr/>');
        $('#dgvScheduleSearch input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                selected.push($(this).attr('id').replace('undefined', ''));
            }
        });

        for (var w = 0; w < selected.length; w++) {
            slotids += selected[w] + ',';

            slotids = slotids.replace('undefined', '');
        }

        var table = $('#dgvScheduleSearch').DataTable();
        slotids = slotids.replace('chkAll', '');

        if (table.data().length == 0) {
            utility.DisplayMessages("No data present.", 3);
            return false;
        }

        else if (table.data().length > 0) {
            if (slotids != "") {
                params["ParentCtrl"] = "schTabSearch";
                params["slotids"] = slotids;
                params["Status"] = "Blocked";
                LoadActionPan('blckreasonDetail', params);


            }
            else {
                utility.DisplayMessages("Please Select a slot.", 3);
            }

        }

    },

    UnBlockSlot: function () {

        var selected = [];
        var slotids = "";
        var $row = $('<tr/>');
        $('#dgvScheduleSearch input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                selected.push($(this).attr('id').replace('undefined', ''));
            }
        });

        for (var w = 0; w < selected.length; w++) {
            slotids += selected[w] + ',';

            slotids = slotids.replace('undefined', '');
        }

        var table = $('#dgvScheduleSearch').DataTable();
        slotids = slotids.replace('chkAll', '');

        if (table.data().length == 0) {
            utility.DisplayMessages("No data present.", 3);
            return false;
        }

        else if (table.data().length > 0) {
            if (slotids != "") {
                params["ParentCtrl"] = "schTabSearch";
                params["slotids"] = slotids;
                params["Status"] = "UnBlocked";
                LoadActionPan('blckreasonDetail', params);


            }
            else {
                utility.DisplayMessages("Please Select a slot.", 3);
            }
        }

    },

    OpenChangeFacility: function () {

        var selected = [];
        var slotids = "";
        var $row = $('<tr/>');
        $('#dgvScheduleSearch input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                selected.push($(this).attr('id').replace('undefined', ''));
            }
        });

        for (var w = 0; w < selected.length; w++) {
            slotids += selected[w] + ',';

            slotids = slotids.replace('undefined', '');
        }

        slotids = slotids.replace('chkAll', '');
        var table = $('#dgvScheduleSearch').DataTable();

        if (table.data().length == 0) {
            utility.DisplayMessages("No data present.", 3);
            return false;
        }

        else if (table.data().length > 0) {



            if (slotids != "") {


                Scheduling_Search.params = [];
                params["ParentCtrl"] = "schTabSearch";
                params["SlotIDs"] = slotids;
                //params["CurrentPage"] = Scheduling_Search.params.CurrentPage;chkAll
                LoadActionPan('schChangeFacility', params);


            }
            else {
                utility.DisplayMessages("Please Select a slot.", 3);
            }


        }

    },


    SelectAllChkBoxes: function () {

        var selected = [];
        var slotids;
        $('#dgvScheduleSearch input[type=checkbox]').each(function () {

            $(this).attr('checked', true);

            selected.push($(this).attr('id').replace('undefined', ''));
        });

        for (var w = 0; w < selected.length; w++) {
            slotids += selected[w] + ',';

            slotids = slotids.replace('undefined', '');
        }

    },


    UnSelectAllChkBoxes: function () {

        var selected = [];
        var slotids;
        $('#dgvScheduleSearch input[type=checkbox]').each(function () {

            $(this).attr('checked', false);
        });
    },


    EditSlot: function (TimeslotDetailid, ProviderId, ResourceId) {

        var criteria = $('#pnlScheduleCalendar #daydate').text().trim();
        params: [];
        params["ParentCtrl"] = "schTabSearch";
        params["TimeslotDetailid"] = TimeslotDetailid;
        params["ProviderId"] = ProviderId;
        params["ResourceId"] = ResourceId;
        LoadActionPan('schEditSlot', params);
    },

    ScheduleSearch: function (PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Scheduler Search", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //var criteria;
            //var criteria1 = 0;
            //var start = blockHoursDetail.ConvertTimeformat("24", $("#pnlScheduleSearch #tpFromTime").val());
            //var end = blockHoursDetail.ConvertTimeformat("24", $("#pnlScheduleSearch #tpToTime").val());

            //var array = start.split(":");
            //var array1 = end.split(":");
            //var x = array[0];
            //var y = array1[0];

            //if (parseInt(x) < parseInt(y))
            //    criteria = 'true';
            //else if (parseInt(x) == parseInt(y)) {
            //    var w = array[1];
            //    var n = array1[1];
            //    if (parseInt(w) == parseInt(n))
            //        criteria = 'false';
            //    if (parseInt(w) < parseInt(n))
            //        criteria = 'true';
            //    if (parseInt(w) > parseInt(n))
            //        criteria = 'false';

            //}
            //else if (parseInt(x) > parseInt(y))
            //    criteria = 'false';

            //if (criteria == 'true')
            //{


            var startDate = new Date($('#pnlScheduleSearch #tpFromTime').val());
            var endDate = new Date($('#pnlScheduleSearch #tpToTime').val());

            if (startDate.getTime() > endDate.getTime()) {
                // Do something
            }

            if (strMessage == "") {
                if ($("#pnlScheduleSearch #pnlScheduleSearch_Result").css("display") == "none") {
                    $("#pnlScheduleSearch #pnlScheduleSearch_Result").show();
                }

                var self = $("#pnlSchedule_Search");
                var myJSON = self.getMyJSON();

                Scheduling_Search.SearchBatchSchedule(myJSON, PageNo, rpp).done(function (response) {
                    if (response.status != false) {

                        //-----------------

                        if (response.ScheduleSearchCount > 0) {
                            $("#pnlScheduleSearch #divSchedulingPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 Record(s)
                            var RecordsPerPage = rpp != null ? rpp : 100;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            var self = $("#pnlSchedule_Search");
                            var myJSON = self.getMyJSON();

                            //params: [];
                            //params["CurrentPage"] = CurrentPage;
                            //params["RecordsPerPage"] = RecordsPerPage;

                            //params["myJSON"] = myJSON;

                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging("divSchedulingPaging", response.iTotalDisplayRecords, 5, "Scheduling_Search", CurrentPage, RecordsPerPage);
                            }
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $("#pnlScheduleSearch #divSchedulingPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $("#pnlScheduleSearch li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else
                                    $(this).removeAttr("class");
                            });
                        }
                        else {
                            $("#pnlScheduleSearch #divSchedulingPaging").css("display", "none");
                        }


                        //--------------------
                        Scheduling_Search.BatchScheduleGridLoad(response);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);

                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
            //}
            //else
            //    utility.DisplayMessages('From Time Criteria must be greater then To Time Criteria.', 3);



        });
    },

    BatchScheduleGridLoad: function (response) {
        $("#pnlScheduleSearch #pnlScheduleSearch_Result #dgvScheduleSearch").dataTable().fnDestroy();
        $("#pnlScheduleSearch #pnlScheduleSearch_Result #dgvScheduleSearch tbody").find("tr").remove();
        if (response.ScheduleSearchCount > 0) {
            var ScheduleSearchJSONData = JSON.parse(response.ScheduleSearch_JSON);
            $.each(ScheduleSearchJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvScheduling_row" + item.TimeSlotDtlId + "'))");
                $row.attr("id", "gvScheduling_row" + item.TimeSlotDtlId);
                $row.attr("TimeSlotDtlId", item.TimeSlotDtlId);

                if (ScheduleSearchJSONData[i].SlotStatusId == '1') {
                    //blocked
                    $row.append('<td style="display:none;"></td><td><input type="checkbox" name="checkbox" id="' + item.TimeSlotDtlId + '"></td><td>' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td onclick="Scheduling_Search.EditSlot(' + item.TimeSlotDtlId + ',\'' + item.ProviderId + '\',\'' + item.ResourceId + '\');" width="63"><a  class="white" href="#">' + item.FromTimeSlots + '</a></td><td>' + item.Day + '</td><td>' + item.Minutes + '</td><td>' + item.BookedPatients + '</td><td>' + item.Provider + '</td><td>' + item.Resources + '</td><td>' + item.Reasons + '</td><td>' + item.OverBooked + '</td><td>' + item.Facility + '</td>');

                    $row.css('color', 'white');
                    $row.css('background-color', '#f88379');
                }
                else {


                    if (parseInt(ScheduleSearchJSONData[i].AppCounts) == 0) {
                        //open
                        $row.append('<td style="display:none;"></td><td><input type="checkbox" name="checkbox" id="' + item.TimeSlotDtlId + '"></td><td>' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td onclick="Scheduling_Search.EditSlot(' + item.TimeSlotDtlId + ',\'' + item.ProviderId + '\',\'' + item.ResourceId + '\');" width="63"><a  href="#">' + item.FromTimeSlots + '</a></td><td>' + item.Day + '</td><td>' + item.Minutes + '</td><td>' + item.BookedPatients + '</td><td>' + item.Provider + '</td><td>' + item.Resources + '</td><td>' + item.Reasons + '</td><td>' + item.OverBooked + '</td><td>' + item.Facility + '</td>');
                        // $row.css('color', '#777777');
                        $row.css('background-color', '#fff');
                    }
                    else {

                        if (parseInt(ScheduleSearchJSONData[i].AppCounts) > parseInt(ScheduleSearchJSONData[i].PatientAllowed)) {

                            //Over Booked
                            $row.append('<td style="display:none;"></td><td><input type="checkbox" name="checkbox" id="' + item.TimeSlotDtlId + '"></td><td>' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td onclick="Scheduling_Search.EditSlot(' + item.TimeSlotDtlId + ',\'' + item.ProviderId + '\',\'' + item.ResourceId + '\');" width="63"><a class="white" href="#">' + item.FromTimeSlots + '</a></td><td>' + item.Day + '</td><td>' + item.Minutes + '</td><td>' + item.BookedPatients + '</td><td>' + item.Provider + '</td><td>' + item.Resources + '</td><td>' + item.Reasons + '</td><td>' + item.OverBooked + '</td><td>' + item.Facility + '</td>');
                            $row.css('background-color', '#CC6666');
                            $row.css('color', 'white');
                        }
                        else if (parseInt(ScheduleSearchJSONData[i].AppCounts) < parseInt(ScheduleSearchJSONData[i].PatientAllowed)) {
                            //Booked
                            $row.append('<td style="display:none;"></td><td><input type="checkbox" name="checkbox" id="' + item.TimeSlotDtlId + '"></td><td>' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td onclick="Scheduling_Search.EditSlot(' + item.TimeSlotDtlId + ',\'' + item.ProviderId + '\',\'' + item.ResourceId + '\');" width="63"><a class="white" href="#">' + item.FromTimeSlots + '</a></td><td>' + item.Day + '</td><td>' + item.Minutes + '</td><td>' + item.BookedPatients + '</td><td>' + item.Provider + '</td><td>' + item.Resources + '</td><td>' + item.Reasons + '</td><td>' + item.OverBooked + '</td><td>' + item.Facility + '</td>');
                            $row.css('background-color', '#76B007');
                            $row.css('color', 'white');
                        }
                        else if (parseInt(ScheduleSearchJSONData[i].PatientAllowed) == parseInt(ScheduleSearchJSONData[i].AppCounts)) {

                            //full
                            $row.append('<td style="display:none;"></td><td><input type="checkbox" name="checkbox" id="' + item.TimeSlotDtlId + '"></td><td>' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td onclick="Scheduling_Search.EditSlot(' + item.TimeSlotDtlId + ',\'' + item.ProviderId + '\',\'' + item.ResourceId + '\');" width="63"><a class="white" href="#">' + item.FromTimeSlots + '</a></td><td>' + item.Day + '</td><td>' + item.Minutes + '</td><td>' + item.BookedPatients + '</td><td>' + item.Provider + '</td><td>' + item.Resources + '</td><td>' + item.Reasons + '</td><td>' + item.OverBooked + '</td><td>' + item.Facility + '</td>');
                            $row.css('background-color', '#FE7510');
                            $row.css('color', 'white');

                        }

                    }
                }

                $("#pnlScheduleSearch_Result #dgvScheduleSearch tbody").last().append($row);
            });
        }
        else {
            $("#pnlScheduleSearch #divSchedulingPaging").css("display", "none");
            $('#pnlScheduleSearch #dgvScheduleSearch').DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlScheduleSearch #pnlScheduleSearch_Result #dgvScheduleSearch'))
            ;
        else
            $("#pnlScheduleSearch #pnlScheduleSearch_Result #dgvScheduleSearch").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchBatchSchedule: function (SchedulingSearchData, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 100;
        }

        var data = "SchedulingSearchData=" + SchedulingSearchData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_SEARCH", "SEARCH_BATCH_SCHEDULING");
    },

    FillPatient: function (PatientID) {
        var data = "PatientID=" + PatientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_SEARCH", "FILL_PATIENT");
    },

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlScheduleSearch_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Scheduling_Search.ScheduleSearch(PageNo, 100);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlScheduleSearch_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Scheduling_Search.ScheduleSearch(currentPageNo, 100);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlScheduleSearch_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Scheduling_Search.ScheduleSearch(currentPageNo, 100);
        }
    },

    UnLoad: function () {

        if (Scheduling_Search.params != null && Scheduling_Search.params.ParentCtrl != null) {
            UnloadActionPan(Scheduling_Search.params.ParentCtrl, 'Scheduling_Search');
        }
        else
            UnloadActionPan(null, 'Scheduling_Search');

    },

    //------------------
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#pnlScheduleSearch #hfFacilityId').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'schTabSearch';
        LoadActionPan('facilityDetail', params);
    },

    ResetTimeDropDown: function () {
        var from = $('#pnlScheduleSearch #tpFromTime').val();
        var to = $('#pnlScheduleSearch #tpToTime').val();

        var fromRes = from.substring(6, 8);
        var toRes = to.substring(6, 8);

        if (fromRes == 'AM' && toRes == 'AM') {
            $("#pnlScheduleSearch #time1").val("AM");
        }
        else if (fromRes == 'PM' && toRes == 'PM') {
            $("#pnlScheduleSearch #time1").val("PM");
        }
        else {
            $("#pnlScheduleSearch #time1").val("ALL");
        }
    },

    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#pnlScheduleSearch #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'schTabSearch';
        LoadActionPan('providerDetail', params);
    },

    OpenResourceDetail: function () {
        var params = [];
        params["ResourceId"] = $('#pnlScheduleSearch #hfResource').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtResource";
        params["ParentCtrl"] = 'schTabSearch';
        LoadActionPan('resourcesDetail', params);
    },

    ResetLink: function () {
        if ($("#pnlScheduleSearch #hfProvider").val() == "") {
            $("#pnlScheduleSearch #lblProvider").css("display", "inline");
            $("#pnlScheduleSearch #lnkProviderEdit").css("display", "none");
        }

        if ($("#pnlScheduleSearch #hfResource").val() == "") {
            $("#pnlScheduleSearch #lblResource").css("display", "inline");
            $("#pnlScheduleSearch #lnkResourceEdit").css("display", "none");
        }
    },

    // Open Global Appointments

    OpenPatientAppointments: function () {
        var params = [];
        //params["patientID"] = Patient_Demographic.params.patientID;
        //params["ParentCtrl"] = "patTabDemographic";
        LoadActionPan('Scheduling_AppointmentSearch', params);
    },

    OpenProviderNotes: function () {
        var params = [];
        LoadActionPan('Clinical_NotesSearch', params);
    },

    ValidateScheduleSearch: function () {
        $('#pnlScheduleSearch #frmSchedulingSearch')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  //Facility: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Scheduling_Search.ScheduleSearch();
       });
    },
    //Azam Aftab Dated Jan 15,2015 PMS-3340
    validateSearchDate: function () {
        utility.ValidateFromToDate('pnlScheduleSearch #frmSchedulingSearch', 'dpfromDate', 'dptoDate', true);
    },
    //end Dated Jan 15,2015 PMS-3340

    ResetAllControls: function () {

        var self = $('#frmSchedulingSearch');
        //self.resetAllControls();
        self.find('[type=hidden],[type=text],[type=password], textarea, ul').each(function () {
            $(this).val('');
        });
        self.find('[type=checkbox]').each(function () {
            this.checked = false;
        });
        self.find('select').each(function () {
            $(this).find('option:selected').removeAttr('selected');
            $(this).find('option:eq(0)').attr('selected', 'selected');
        });
        if ($('#pnlScheduleSearch #frmSchedulingSearch').data('bootstrapValidator') != null && typeof $('#pnlScheduleSearch #frmSchedulingSearch').data('bootstrapValidator') != 'undefined') {
            $("#pnlScheduleSearch #frmSchedulingSearch").bootstrapValidator('revalidateField', 'Facility');
        }
        if ($("#pnlScheduleSearch #lnkFacilityEdit").css("display") == "inline") {
            $("#pnlScheduleSearch #lnkFacilityEdit").css("display", "none");
            $("#pnlScheduleSearch #lblFacility").css("display", "inline");
        }
    },
}