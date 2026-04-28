Patient_TaskDetail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        Patient_TaskDetail.params = params;

        Patient_TaskDetail.LoadUserTaskDetail();
    },

    LoadUserTaskDetail: function () {
        //var appointmentSummaryData = "";
        $('#pnlPatientTaskDetail #tblTaskDetail_Summary tbody tr').remove();

        Patient_TaskDetail.LoadUserTaskDetail_DBCall().done(function (response) {
            if (response.status != false) {
                Patient_TaskDetail.UserTaskGridLoad(response);

            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadUserTaskDetail_DBCall: function(){

        var objData = new Object();
        objData["UserMesgId"] = Patient_TaskDetail.params.UserMessageId;
        objData["CommandType"] = "load_tasks";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");

    },

    UserTaskGridLoad: function (response) {
        $("#dgvTaskDetail").dataTable().fnDestroy();
        $("#tblTaskDetail_Summary #dgvTaskDetail tbody").find("tr").remove();
        if (response.TaskCount > 0) {
            var TaskLoadLoadJSONData = JSON.parse(response.TaskLoad_JSON);
            $.each(TaskLoadLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvPatMsgId_row" + item.PatMsgId);
                $row.attr("PatMsgId", item.PatMsgId);
               
                $row.append('<td style="display:none;">' + item.PatMsgId + '</td><td><a class="btn btn-xs" href="#" onclick="Patient_TaskDetail.ViewTask(' + item.PatMsgId + ')" title="View Record"><i class="fa fa-edit black"></i></a></td><td> ' + item.MsgDetail + ' </td> <td> ' + item.MessageType + ' </td> <td> ' + item.MessageStatus + ' </td><td> ' + item.CreatedOn + ' </td>');
                $("#tblTaskDetail_Summary #dgvTaskDetail tbody").last().append($row);
            });
        }
        else {
            $('#dgvTaskDetail').DataTable({
                "language": {
                    "emptyTable": "No Task Found"
                }, "autoWidth": false, "bLengthChange": false, "bInfo": false, "bPaginate": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvTaskDetail'))
            ;
        else
            $("#tblTaskDetail_Summary #dgvTaskDetail").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    UnLoad: function () {

        UnloadActionPan(Patient_TaskDetail.params["ParentCtrl"], "pnlPatientTaskDetail");

    },


    ViewTask: function (MessageId) {

        var params = [];
        //params["UserMessageId"] = Patient_MessageCompose.params.UserMessageId;
        params["mode"] = 'Edit';
        params["ParentCtrl"] = "Patient_TaskDetail";
        params["AssignedToId"] = null;
        params["MessageId"] = MessageId;
        LoadActionPan('Patient_MessageEdit', params);

    },
}