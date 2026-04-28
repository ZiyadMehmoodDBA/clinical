appointmentHistory = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {


        BackgroundLoaderShow(true);

        appointmentHistory.params = params;

        if (appointmentHistory.bIsFirstLoad) {
            appointmentDetail.bIsFirstLoad = false;
        }

        appointmentHistory.LoadActivityLogs();
      
     
    },

    

   
    UnLoad: function () {
        if (appointmentHistory.params["ParentCtrl"] == null) {
            appointmentHistory.params["ParentCtrl"] = 'schTabCalendar';
        }
        UnloadActionPan(appointmentHistory.params.ParentCtrl);
       


    },
    LoadActivityLogs: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                

                appointmentHistory.ActivityLogsLoad().done(function (response) {
                    if (response.status != false) {
                        var NewEntryResponse = "";
                        if (response.RecordCount > 0) {
                            NewEntryResponse = JSON.parse(response.AppointmentLoad_JSON);
                            appointmentHistory.params.UserData = JSON.parse(response.UserEntry);
                            appointmentHistory.params.FieldsData = JSON.parse(response.ChangedFields);
                        }
                        else {
                            appointmentHistory.UserGridLoad(response.RecordCount);
                            appointmentHistory.FieldsGridLoad(response.RecordCount,null);
                        }
                            
                            appointmentHistory.NewEntryGridLoad(NewEntryResponse, response.RecordCount, "NewEntry", "#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_NewEntry #dgvNewEntry");

                        
                            

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
    ActivityLogsLoad: function (PatientId, DBAuditId, ColumnKeyId, ProfileName, EntryDate, ActivityType, DBTableName, ParentKeyColumnId, CPTCodeCostID, UserId) {
        if (DBAuditId == null) {
            DBAuditId = "";
        }
        if (ColumnKeyId == null) {
            ColumnKeyId = "";
        }
        if (ProfileName == null) {
            ProfileName = "";
        }
        if (EntryDate == null) {
            EntryDate = "";
        }
        if (ActivityType == null) {
            ActivityType = "";
        }
        if (PatientId == null) {
            PatientId = "";
        }
        if (DBTableName == null) {
            DBTableName = "";
        }
        if (ParentKeyColumnId == null) {
            ParentKeyColumnId = "";
        }

        var data = null;
        // serach parameter , class name, command name of class
      var AppointmentId=  appointmentHistory.params.AppointmentId;


      data = "AppointmentId=" + AppointmentId + "&CreatedOn=" + "";
          //  return MDVisionService.defaultService(data, "PATIENT_ACTIVITY_LOG", "LOAD_DRUGCODECOST_ACTIVITY_LOG");
            return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_HISTORY", "SELECT_APPOINTMENT_HISTORY");
        
        

    },
    NewEntryGridLoad: function (response, count, gridType, gridId) {

        if ($.fn.dataTable.isDataTable(gridId))
            $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();

        var emptyTableMsg = "";
   
            emptyTableMsg = "No New Entry Found";
            if ($("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_NewEntry").css("display") == "none") {
                $("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_NewEntry").show();
            }
       
        if (count != null && count > 0) {
            var firstRowId = "";
          //  var PatientLoadJSONData = JSON.parse(response);
        
                var $row = $('<tr/>');
                var _rowId = "";
                var emptyTableMsg = "";
            
                    _rowId = "dgvNewEntry_row0" ;
                    $row.append('<td style="display:none;">' + response[0].DBAuditAppointmentId + '</td><td>' + response[0].ProfileName + '</td><td class="ellipses size-max90" title="' + response[0].UserName + '">' + response[0].UserName + '</td><td>' + response[0].CreatedDate + '</td>');
                        
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); appointmentHistory.UserGridLoad("+count+");");
                    

                
              
                    if (firstRowId == "") {
                        firstRowId = _rowId;
                    }
                    $row.attr("id", _rowId);
                

                
                    $(gridId + " tbody").last().append($row);
       


         

            if ($.fn.dataTable.isDataTable(gridId) || $(gridId).parent().parent().hasClass("dataTables_wrapper"))
                ;
            else
                $(gridId).DataTable({ bDestroy: true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

            $("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_NewEntry #dgvNewEntry tbody tr:first").addClass("active");
            $("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_NewEntry #dgvNewEntry tbody tr:first").trigger('click');
        }
        else {
           

            if (!$(gridId).parent().parent().hasClass("dataTables_wrapper")) {
                $(gridId).DataTable({
                    bDestroy: true,
                    "language": {
                        "emptyTable": emptyTableMsg
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="4" class="center" >' + emptyTableMsg + '</td>');
                $(gridId + " tbody").last().append($row);
            }
        }

    },
    UserGridLoad: function (count) {
        var emptyTableMsg = "";

        emptyTableMsg = "No User Found";

        var gridId = "#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_User #dgvUser";
        if ($.fn.dataTable.isDataTable(gridId))
            $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();
        var response = appointmentHistory.params.UserData

        if ($("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_User").css("display") == "none") {
            $("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_User").show();
        }
        if ($("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #UserHeading").css("display") == "none") {
            $("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #UserHeading").show();
        }
        if (count > 0) {
            $.each(response, function (i, item) {
                var $row = $('<tr/>');
                var _rowId = "";
                var firstRowId = "";
                _rowId = "dgvUser_row" + i;
                if (firstRowId == "") {
                    firstRowId = _rowId;
                }
                $row.attr("id", _rowId);

                $row.append('<td style="display:none;">' + item.DBAuditAppointmentId + '</td><td>' + item.ProfileName + '</td><td>' + item.DBAuditAction + '</td><td class="ellipses size-max90" title="' + item.UserName + '">' + item.UserName + '</td><td>' + item.CreatedDate + '</td>');
                if (item.Actions == 'Deleted') {
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); appointmentHistory.FieldsGridLoad('" + count + "','" + item.CreatedDate + "')");
                } else {

                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); appointmentHistory.FieldsGridLoad('" + count + "','" + item.CreatedDate + "');");
                }
                $(gridId + " tbody").last().append($row);
            });
            $("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_User #dgvUser tbody tr:first").addClass("active");
            $("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_User #dgvUser tbody tr:first").trigger('click');

            if ($.fn.dataTable.isDataTable(gridId) || $(gridId).parent().parent().hasClass("dataTables_wrapper"))
                ;
            else
                $(gridId).DataTable({ bDestroy: true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
        else {
            if (!$(gridId).parent().parent().hasClass("dataTables_wrapper")) {
                $(gridId).DataTable({
                    bDestroy: true,
                    "language": {
                        "emptyTable": emptyTableMsg
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="4" class="center" >' + emptyTableMsg + '</td>');
                $(gridId + " tbody").last().append($row);
            }

        }
       

    },
    FieldsGridLoad: function (count, createdDate) {

       
        var emptyTableMsg = "";

        emptyTableMsg = "No Field Found";
        var gridId = "#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_Changes #dgvChanges";
        if ($.fn.dataTable.isDataTable(gridId))
            $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();
        
        if ($("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #ChangesHeading").css("display") == "none") {
            $("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #ChangesHeading").show();
        }
        if ($("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_Changes").css("display") == "none") {
            $("#" + appointmentHistory.params["PanelID"] + " #frmAppointmentHistory #pnlActivityLog_Changes").show();
        }
        
        
        if (count > 0) {
            var response = appointmentHistory.params.FieldsData
            if (createdDate == null)
            {
                //response = response.filter(f => f.DBAuditAction == "Insert");

                response = response.filter(function (f) {
                    return f.DBAuditAction == "Insert";
                });
            }
            else
            {
                //response = response.filter(f => f.CreatedDate == createdDate)
                response = response.filter(function (f) {
                    return f.CreatedDate == createdDate;
                });

            }
            $.each(response, function (i, item) {
                var $row = $('<tr/>');
                var firstRowId = "";
                var _rowId = "";
                _rowId = "dgvChanges_row" + i;
                if (firstRowId == "") {
                    firstRowId = _rowId;
                }
                $row.attr("id", _rowId);
               // if(item.DisplayName!="AppointmentStatus")
                $row.append('<td style="display:none;">' + item.DBAuditAppointmentId + '</td><td>' + item.DisplayName + '</td><td>' + item.OriginalValue + '</td><td >' + item.CurrentValue + '</td>');
                if (item.Actions == 'Deleted') {
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); appointmentHistory.GridLoad(null, null, 'Field','#" + Activity_Log.params["PanelID"] + " #frmActivityLog #pnlActivityLog_Changes #dgvChanges');");
                } else {

                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "'))");
                }
                $(gridId + " tbody").last().append($row);
            });

            if ($.fn.dataTable.isDataTable(gridId) || $(gridId).parent().parent().hasClass("dataTables_wrapper"))
                ;
            else
                $(gridId).DataTable({ bDestroy: true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
        else {
            if (!$(gridId).parent().parent().hasClass("dataTables_wrapper")) {
                $(gridId).DataTable({
                    bDestroy: true,
                    "language": {
                        "emptyTable": emptyTableMsg
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="4" class="center" >' + emptyTableMsg + '</td>');
                $(gridId + " tbody").last().append($row);
            }

        }
    },
    
}