/*
Author : Khaleel Ur Rehman
Date : 22 May 2016
Purpose : To handle operations for MU1 report.
*/
MUStage1 = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        MUStage1.params = params;
        MUStage1.resultSet = params["resultSet"];
        MUStage1.heading = params["rptName"];
        MUStage1.ID = params["ID"];
        if (MUStage1.params.PanelID != 'pnlMUStage1') {
            MUStage1.params.PanelID = MUStage1.params.PanelID + ' #pnlMUStage1';
        } else {
            MUStage1.params.PanelID = 'pnlMUStage1';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + MUStage1.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        $('#' + MUStage1.params["PanelID"] + ' #headingTitle').text(MUStage1.heading);
        if (MUStage1.heading == "MU Stage 1 Report" || MUStage1.heading == "MU Stage 2 Report") {
            MUStage1.fillPatientData();
        } else if (MUStage1.heading == "MU Stage 2 Report Latest" || MUStage1.heading == "MU Stage 3 Report") {
            MUStage1.fillPatientDataNumerator();
        }
    },

    fillPatientData: function () {
        if (MUStage1.resultSet.length > 0 && MUStage1.ID > 0) {
            $('#' + MUStage1.params["PanelID"] + ' #tblMUData > tbody').html("");
            $.each(MUStage1.resultSet, function (i, item) {
                if (item.ID == MUStage1.ID && item.Numerator == "0") {
                    var MethodMode = "MUStage1.SelectPatient("+item.PatientID+", event);";
                    var $row = $('<tr/>');
                    $row.attr("onclick", MethodMode);
                    $row.append('<td>' + item.accountnumber + '</td><td>' + item.FirstName + '</td>' + '<td>' + item.LastName + '</td>'
                        + '<td>' + utility.RemoveTimeFromDate(null, item.DOB) + '</td>'
                        + '<td>' + item.Gender + '</td><td>' + item.Denominator + '</td><td>' + item.Numerator + '</td>');
                    $('#' + MUStage1.params["PanelID"] + ' #tblMUData > tbody').last().append($row);
                }
            });
        }
    },

    fillPatientDataNumerator: function () {
        if (MUStage1.resultSet.length > 0 && MUStage1.ID > 0) {
            $('#' + MUStage1.params["PanelID"] + ' #tblMUData > tbody').html("");
            $.each(MUStage1.resultSet, function (i, item) {
                if (item.ID == MUStage1.ID/* && item.Numerator == "1"*/) {
                    var MethodMode = "MUStage1.SelectPatient(" + item.PatientID + ", event);";
                    var $row = $('<tr/>');
                    $row.attr("onclick", MethodMode);
                    $row.append('<td>' + item.accountnumber + '</td><td>' + item.FirstName + '</td>' + '<td>' + item.LastName + '</td>'
                        + '<td>' + utility.RemoveTimeFromDate(null, item.DOB) + '</td>'
                        + '<td>' + item.Gender + '</td><td>' + item.Denominator + '</td><td>' + item.Numerator + '</td>');
                    $('#' + MUStage1.params["PanelID"] + ' #tblMUData > tbody').last().append($row);
                }
            });
        }
    },

    SelectPatient: function (PatientId, event) {
        MUStage1.UnLoadTab();
        if (event != null) {
            event.stopPropagation();
        }
        $.when(SelectPatient(PatientId, "")).done(function () {
            $('#patTabDemographic').click();
        });
    },
    UnLoadTab: function () {
        if (MUStage1.params["FromAdmin"] == "0") {
            if (MUStage1.params != null && MUStage1.params.ParentCtrl != null) {
                UnloadActionPan(MUStage1.params.ParentCtrl, 'MUStage1');
               
            }
            else
                UnloadActionPan(null, 'MUStage1');
        }
        else {

            RemoveAdminTab();
        }
    },
}