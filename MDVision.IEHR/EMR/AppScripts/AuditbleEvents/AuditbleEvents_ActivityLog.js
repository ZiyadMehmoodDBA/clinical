AuditbleEvents_ActivityLog = {
    params: [],
    bIsFirstLoad: true,
    RequireSubmit: true,
    ActualVisitID: null,
    ActualChargeCaptureID: null,
    IsChargeCapture: null,
    Load: function (params) {

        AuditbleEvents_ActivityLog.params = params;
        AuditbleEvents_ActivityLog.ActualVisitID = null;
        AuditbleEvents_ActivityLog.ActualChargeCaptureID = null;
        AuditbleEvents_ActivityLog.IsChargeCapture = null;
        AuditbleEvents_ActivityLog.ValidateActivityLogs();
        AuditbleEvents_ActivityLog.LoadAllAutocomplete();
        AuditbleEvents_ActivityLog.Search_ActivityLog();



    },
    // Zia Mehmood
    //Validation function
    ValidateActivityLogs: function () {
        $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {


                  DateTo: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                          date: {
                              format: date_format.toUpperCase(),
                              message: ' '
                          }
                      }
                  },
                  DateFrom: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                          date: {
                              format: date_format.toUpperCase(),
                              message: ' '
                          }
                      }
                  },



              }
          })

       .on('success.form.bv', function (e) {

           e.preventDefault();
           $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateFrom');
           $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateTo');
           AuditbleEvents_ActivityLog.Search_ActivityLog();

       });
    },
    // End validation function
    // Start LoadAllFunction
    LoadAllAutocomplete: function () {
        utility.CreateDatePicker("pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog #dtpDOB", function () {
        }, false);


        utility.CreateDatePicker("pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog #dtpDateTo",
      function (ev) {

          if ($('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').data("bootstrapValidator") != null) {
              $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateTo');
          }

      }, true);
        $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog #dtpDateTo').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog #dtpDateFrom",
    function (ev) {

        if ($('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').data("bootstrapValidator") != null) {
            $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateFrom');

        }

        //on-change callback method
    }, true);
        $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('frmAuditbleEventsActivityLog', 'dtpDateFrom', 'dtpDateTo', true, null, null, true);

    },
    // End LoadAllFunctiln

    //start  Reset SearchLoadAuditbleEvents_ActivityLogs
    ClearSearch_ActivityLog: function () {

        $(' #pnlAuditbleEventsActivityLog').resetAllControls();
        $(' #pnlAuditbleEventsActivityLog #ddlActive').find('option:selected').removeAttr('selected');
        $(' #pnlAuditbleEventsActivityLog #ddlActive').val('1');
        $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateFrom');
        $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateTo');

    },
    //End Reset Search
    // Start-- search Activity Log
    Search_ActivityLog: function (pageNumber, rowsPerPage) {

        AuditbleEvents_ActivityLog.searchActivityLog_DBCall(pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogUser_JSON);
                AuditbleEvents_ActivityLog.GridLoad(ActivityLogUser_JSON, response.ActivityLogUserCount, "NewEntry", " #pnlAuditbleEventsActivityLog #dgvNewEntry");
                var TableControl = AuditbleEvents_ActivityLog.params.PanelID + " #dgvNewEntry";
                var PagingPanelControlID = AuditbleEvents_ActivityLog.params.PanelID + " #dgvActivityLogUser_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogUserCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    AuditbleEvents_ActivityLog.Search_ActivityLog(pageNumber, resultPerPage);
                }), 10);
                if (ActivityLogUser_JSON.length > 0) {
                    $('#pnlAuditbleEventsActivityLog #dgvNewEntry tbody tr:first').click();
                }
                else {                   
                    AuditbleEvents_ActivityLog.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "User", " #pnlAuditbleEventsActivityLog #dgvUser");
                    AuditbleEvents_ActivityLog.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                }


            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },
    searchActivityLog_DBCall: function (pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;

        //objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId
        objData["AccountNo"] = $(' #pnlAuditbleEventsActivityLog #txtAccountNo').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #txtAccountNo').val();
        objData["LastName"] = $(' #pnlAuditbleEventsActivityLog #txtLastName').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #txtLastName').val();
        objData["FirstName"] = $(' #pnlAuditbleEventsActivityLog #txtFirstName').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #txtFirstName').val();
        objData["DOB"] = $(' #pnlAuditbleEventsActivityLog #dtpDOB').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #dtpDOB').val();
        objData["SSN"] = $(' #pnlAuditbleEventsActivityLog #txtSSN').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #txtSSN').val();
        objData["Status"] = $(' #pnlAuditbleEventsActivityLog #ddlPatientStatus').val();
        objData["DateTo"] = $(' #pnlAuditbleEventsActivityLog #dtpDateTo').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #dtpDateTo').val();
        objData["DateFrom"] = $(' #pnlAuditbleEventsActivityLog #dtpDateFrom').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #dtpDateFrom').val();
        objData["EmergencyAccess"] = $(' #pnlAuditbleEventsActivityLog #chkEnergencyAccess').prop('checked') == true ? true : false;
        objData["commandType"] = "auditbleeventsactivitylog";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");

    },
    // End -- search Activity Log

    // Start -- Component Load
    ActivityLogsComponent: function (ProfileName,PatientId, DateAndTime, UserId, pageNumber, rowsPerPage) {
        AuditbleEvents_ActivityLog.searchActivityLogComponents_DBCall(ProfileName,PatientId, DateAndTime, UserId, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogCompLoad_JSON);
                AuditbleEvents_ActivityLog.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "User", " #pnlAuditbleEventsActivityLog #dgvUser");
                var TableControl = AuditbleEvents_ActivityLog.params.PanelID + " #dgvUser";
                var PagingPanelControlID = AuditbleEvents_ActivityLog.params.PanelID + " #dgvActivityLogComp_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogCompCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    AuditbleEvents_ActivityLog.ActivityLogsComponent($('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowprofilename').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowpatid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowdateid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowuserid').text(), pageNumber, resultPerPage);
                }), 10);
                if (ActivityLogUser_JSON.length > 0) {
                    $('#pnlAuditbleEventsActivityLog #dgvUser tbody tr:first').click();
                }
                else {
                    AuditbleEvents_ActivityLog.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    searchActivityLogComponents_DBCall: function (ProfileName,PatientId, DateAndTime, UserId, pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;
        objData["ProfileName"] = ProfileName;
        objData["PatientId"] = PatientId;
        objData["DateAndTime"] = DateAndTime;
        objData["UserId"] = UserId;
        objData["commandType"] = "auditbleeventsactivitylogcomponents";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");
    },
    // End -- Components load
    // Start -- ActivityLog Changes
    ActivityLogsChanges: function (ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage) {
        AuditbleEvents_ActivityLog.searchActivityLogChanges_DBCall(ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogChangesLoad_JSON);
                AuditbleEvents_ActivityLog.GridLoad(ActivityLogUser_JSON, response.ActivityLogChangesCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                var TableControl = AuditbleEvents_ActivityLog.params.PanelID + " #dgvChanges";
                var PagingPanelControlID = AuditbleEvents_ActivityLog.params.PanelID + " #dgvActivityLogChanges_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogChangesCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    AuditbleEvents_ActivityLog.ActivityLogsChanges($('#pnlAuditbleEventsActivityLog #dgvUser tr.active #colkey').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #profName').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #datetime').text(), pageNumber, resultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    searchActivityLogChanges_DBCall: function (ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;
        objData["ColumnKeyId"] = ColumnKeyId;
        objData["DateAndTime"] = DateAndTime;
        objData["ProfileName"] = ProfileName;
        objData["commandType"] = "auditbleeventsactivitylogChanges";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");

    },
    // ENd -- ActivityLog Changes
    GridLoad: function (ActivityLogUser_JSON, count, gridType, gridId) {
        $(gridId + " tbody").empty();
        $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();

        var emptyTableMsg = "No New Entry Found";

        if (count != null && count > 0) {
            var firstRowId = "";

            $.each(ActivityLogUser_JSON, function (i, item) {
                var $row = $('<tr/>');
                var _rowId = "";
                var emptyTableMsg = "";
                if (gridType == "NewEntry" || gridType == "NewEntryUser") {
                    _rowId = "dgvNewEntry_row" + i;
                    if (item.ColumnKeyId == null || item.ColumnKeyId == "") {
                        temp_colkey = null;
                    }
                    else {
                        temp_colkey = item.ColumnKeyId;
                    }
                    $row.append('<td id= "rowprofilename" style="display:none;">' + item.ModuleName + '</td><td id= "rowpatid" style="display:none;">' + item.PatientId + '</td><td id= "rowcolid" style="display:none;">' + temp_colkey + '</td><td id= "rowdateid" style="display:none;">' + item.DateAndTime + '</td><td id= "rowuserid" style="display:none;">' + item.UserId + '</td><td class="ellipses size-max90" title="' + item.Patient + '">' + item.Patient + '</td><td class="ellipses size-max90" title="' + item.AccountNo + '">' + item.AccountNo + '</td><td class="ellipses size-max90" title="' + item.User + '">' + item.User + '</td>><td class="ellipses size-max90" title="' + item.ModuleName + '">' + item.ModuleName + '</td><td>' + item.DateAndTime + '</td>');
                    //$row.append('<td style="display:none;">' + item.AuditbleEvents_ActivityLogId + '</td><td>' + item.ProfileName + '</td><td>' + item.User + '</td><td>' + item.EntryDate + '</td><td>' + item.Field + '</td><td>' + item.OriginalValue + '</td><td>' + item.CurrentValue + '</td>');


                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); AuditbleEvents_ActivityLog.ActivityLogsComponent('" + item.ModuleName + "'," + item.PatientId + ",'" + item.DateAndTime + "'," + item.UserId + ");");


                }
                else if (gridType == "User") {
                    _rowId = "dgvUser_row" + i;
                    var temp_colkey
                    if (item.ColumnKeyId == null || item.ColumnKeyId == "")
                    {
                        temp_colkey = null;
                    }
                    else {
                        temp_colkey = item.ColumnKeyId;
                    }
                    $row.append('<td id="colkey" style="display:none;">' + temp_colkey + '</td><td id="profName" style="display:none;">' + item.ProfileName + '</td><td id="datetime"style="display:none;">' + item.CreatedDateTime + '</td><td >' + item.ProfileName + '</td><td style="display:none;">' + item.SubProfileName + '</td><td>' + item.DBAuditAction + '</td><td>' + item.DateAndTime + '</td>');
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); AuditbleEvents_ActivityLog.ActivityLogsChanges(" + temp_colkey + ",'" + item.ProfileName + "','" + item.CreatedDateTime + "');");



                } else if (gridType == "Changes") {
                   //
                 
                    _rowId = "dgvDeletedCharges_row" + i;
                    var OriginalValue = item.PreviousValue;
                    var CurrentValue = item.CurrentValue;
                    if (item.Field == 'Note Text') {
                        CurrentValue = CurrentValue.replace(/&quot;/g, '"');
                        CurrentValue = CurrentValue.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                        CurrentValue = CurrentValue.replace(/&nbsp;/g, '');
                        OriginalValue = OriginalValue.replace(/&quot;/g, '"');
                        OriginalValue = OriginalValue.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                        OriginalValue = OriginalValue.replace(/&nbsp;/g, '');
                    }
                    if (item.Field == 'Patient Image') {

                        CurrentValue = CurrentValue != "" ? '<img id="imgCurrent" class="img-responsive img-center" src="' + CurrentValue + '" alt="Current Patient Image"/>' : "";
                        OriginalValue = OriginalValue != "" ? '<img id="imgCurrent" class="img-responsive img-center" src="' + OriginalValue + '" alt="Original Patient Image"/>' : "";

                    }
                    if (item.Field == 'SSN') {
                        if (globalAppdata.IsFullSSN.toLowerCase() === 'true') {
                        }
                        else {

                            if (CurrentValue != "") {
                                var last4digit = CurrentValue.slice(-4);
                                CurrentValue = "XXX-XX-" + last4digit;
                            }
                            if (OriginalValue != "") {
                                var last4digits = OriginalValue.slice(-4);
                                OriginalValue = "XXX-XX-" + last4digits;
                            }
                        }
                    }
                     
                    if (item.Field.indexOf("Date") >= 0 || item.Field.indexOf("DOB") >= 0 || item.Field == 'Started On' || item.Field == 'Stopped On')
                    {
                        CurrentValue = CurrentValue != "" ? utility.RemoveTimeFromDate(null, CurrentValue) : "";
                        OriginalValue = OriginalValue != "" ? utility.RemoveTimeFromDate(null, OriginalValue) : "";

                    }
                    if((item.Field == 'DOSTo')
                        || (item.Field == 'DOSFrom'))
                    {
                        if (CurrentValue != "")
                        {
                            CurrentValue = CurrentValue.replace(' 12:00AM', '');
                            CurrentValue = new Date(CurrentValue);
                            CurrentValue = $.datepicker.formatDate('mm/dd/yy', CurrentValue);
                        }
                        if (OriginalValue != "")
                        {
                            OriginalValue = OriginalValue.replace(' 12:00AM', '');
                            OriginalValue = new Date(OriginalValue);
                            OriginalValue = $.datepicker.formatDate('mm/dd/yy', OriginalValue);

                        }
                       
                    }
                   // $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.Field + '</td><td>' + OriginalValue + '</td><td>' + CurrentValue + '</td>');
                    // disabled the anchor click, because it's only for user to view
                    if (item.Field == 'Note Text') { $row.find('a').addClass('disableAll'); }
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "'));");
                    
                    
                    $row.append('<td style="display:none;">' + item.AuditbleEvents_ActivityLogId + '</td><td>' + item.Field + '</td><td>' + OriginalValue + '</td><td>' + CurrentValue + '</td>');


                }

                if (gridType == "User") {
                    if (firstRowId == "") {
                        firstRowId = _rowId;
                    }
                    $row.attr("id", _rowId);
                }
                else {
                    if (firstRowId == "") {
                        firstRowId = _rowId;
                    }
                    $row.attr("id", _rowId);
                }

                if (AuditbleEvents_ActivityLog.params.ParentCtrl == "userDetail" && $(gridId).attr("id") == "dgvUser")
                { $(gridId + " thead").find("th").eq(1).css("display", "none"); $(gridId + " tbody").last().append($row); }
                else
                    $(gridId + " tbody").last().append($row);
            });



        }
        else {


            $(gridId).DataTable({
                "language": {
                    "emptyTable": emptyTableMsg
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            if ($(gridId + "_wrapper").find(".datatables-footer").length == 1 && gridType == "User") {
                $("#dgvActivityLogComp_Paging").children().remove();
            }
            if ($(gridId + "_wrapper").find(".datatables-footer").length == 1 && gridType == "Changes") {
                $("#dgvActivityLogChanges_Paging").children().remove();
            }
        }


        if ($.fn.dataTable.isDataTable(gridId))
            ;
        else {
            var orderIndex;
            if (gridType == "NewEntry" || gridType == "NewEntryUser") {
                orderIndex = 9;
            } else if (gridType == "User") {
                orderIndex = 6;
            } else {

                orderIndex = 1;
            }
            $(gridId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": false, "order": [[orderIndex, "desc"]] }); // to remove records per page dropdown
        }

        if ($(gridId + "_wrapper").find(".datatables-footer").length > 1) {
            $(gridId + "_wrapper").find(".datatables-footer").last().remove();
            $(gridId + "_wrapper").find(".Of-a").first().removeClass("Of-a");
        }

    },







}
