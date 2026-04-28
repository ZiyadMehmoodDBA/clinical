claimViewDetail = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        claimViewDetail.params = params;


        if (claimViewDetail.bIsFirstLoad) {

            claimViewDetail.bIsFirstLoad = false;

        }

        claimViewDetail.LoadClaimViewDetail(claimViewDetail.params.BatchNumber, claimViewDetail.params.BatchId);
    },



    LoadClaimViewDetail: function (BatchNumber, BatchId) {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        if ($("#claimViewDetail #pnlBillclaimViewDetail_Result").css("display") == "none") {
            $("#claimViewDetail #pnlBillclaimViewDetail_Result").show();
        }


        claimViewDetail.ClaimViewDetailLoad(BatchNumber, BatchId).done(function (response) {
            if (response.status != false) {
                claimViewDetail.ClaimViewDetailGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    ClaimViewDetailGridLoad: function (response) {
        $("#dgvClaimView").dataTable().fnDestroy();
        //$("#pnlSchAppointmentStatus_Result #dgvScheduleSearch tbody").find("tr").remove();
        if (response.ClaimDetailCount > 0) {
            var ClaimDetailJSONData = JSON.parse(response.ClaimDetail_JSON);
            $.each(ClaimDetailJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "utility.SelectGridRow($('#gvAppointmentStatus_row" + item.AppointmentId + "'))");
                //$row.attr("id", "gvAppointmentStatus_row" + item.AppointmentId);
                //$row.attr("AppointmentId", item.AppointmentId);
                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'claimViewDetail', event);";
                var VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'claimViewDetail', event);";

                $row.append('<td>' + item.DOSFrom.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td><a href="#" onclick="' + VisitDetail + '"  title="View Claim Detail">' + item.ClaimNumber + '</a></td><td>' + item.Facility + '</td><td>' + item.Provider + '</td><td>' + item.Plan + '</td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.Account + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.LastName + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.FirstName + '</a></td><td class="text-right">' + utility.convertToFigure(item.Fee, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PlanAmount, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PlanBalance, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PATAmount, true) + '</td><td class="text-right">' + utility.convertToFigure(item.CopayAmount, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PATPaid, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PATBalance, true) + '</td><td data-toggle="tooltip" data-placement="left" title="' + item.Status + '">' + item.Status + '</td><td>' + item.SubmittedDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.Case + '</td><td class="size-max50 ellipses" data-toggle="tooltip" data-placement="left" title="' + item.EntryDate + '">' + item.EntryDate + '</td><td>' + item.CreatedByName + '</td>');


                $("#pnlBillclaimViewDetail_Result #dgvClaimView tbody").last().append($row);
            });
        }
        else {
            $('#dgvClaimView').DataTable({
                "language": {
                    "emptyTable": "No Claim Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#dgvClaimView'))
            ;
        else
            $("#pnlBillclaimViewDetail_Result #dgvClaimView").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    ClaimViewDetailLoad: function (BatchNumber, BatchId) {
        var data = "BatchNumber=" + BatchNumber + "&BatchId=" + BatchId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_CLAIMVIEW_DETAIL", "SEARCH_CLAIMVIEW_DETAIL");
    },


    UnLoad: function () {
        //if ($('#frmClaimDetail').serialize() != $('#frmClaimDetail').data('serialize')) {
        //    utility.myConfirm('2', function () {
        if (claimViewDetail.params != null && claimViewDetail.params.ParentCtrl != null) {
            UnloadActionPan(claimViewDetail.params.ParentCtrl, "claimViewDetail");
        }
            //  }, function () { },
            //  '2'
            //  );
            // }
        else {
            UnloadActionPan(null, 'claimViewDetail');
        }

    }
}
