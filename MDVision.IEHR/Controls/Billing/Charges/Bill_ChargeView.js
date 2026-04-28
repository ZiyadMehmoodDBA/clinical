chargesViewDetail = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        chargesViewDetail.params = params;


        if (chargesViewDetail.bIsFirstLoad) {

            chargesViewDetail.bIsFirstLoad = false;


            var self = $('#chargesViewDetail');
            self.loadDropDowns(true).done(function () {
            });
        }

        chargesViewDetail.LoadChargesViewDetail(chargesViewDetail.params.BatchNumber, chargesViewDetail.params.BatchId);
    },


    LoadChargesViewDetail: function (BatchNumber, BatchId) {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        if ($("#chargesViewDetail #pnlBillchargesViewDetail_Result").css("display") == "none") {
            $("#chargesViewDetail #pnlBillchargesViewDetail_Result").show();
        }


        chargesViewDetail.ChargesViewDetailLoad(BatchNumber, BatchId).done(function (response) {
            if (response.status != false) {
                chargesViewDetail.ChargesViewDetailGridLoad(response);
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

    ChargesViewDetailGridLoad: function (response) {
        $("#dgvChargesView").dataTable().fnDestroy();
        //$("#pnlSchAppointmentStatus_Result #dgvScheduleSearch tbody").find("tr").remove();
        if (response.ChargeDetailCount > 0) {
            var ChargeDetailJSONData = JSON.parse(response.ChargeDetail_JSON);
            $.each(ChargeDetailJSONData, function (i, item) {
                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'chargesViewDetail', event);";
                var VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'chargesViewDetail', event);";

                var $row = $('<tr/>');
                //$row.attr("onclick", "utility.SelectGridRow($('#gvAppointmentStatus_row" + item.AppointmentId + "'))");
                //$row.attr("id", "gvAppointmentStatus_row" + item.AppointmentId);
                //$row.attr("AppointmentId", item.AppointmentId);


                $row.append('<td><a href="#" onclick="' + VisitDetail + '"  title="View Claim Detail">' + item.ClaimNumber + '</a></td><td>' + item.DOSFrom.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.LastName + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.FirstName + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.Account + '</a></td><td>' + item.Facility + '</td><td>' + item.Provider + '</td><td>' + item.CPT + '</td><td>' + item.ICD1 + '</td><td>' + item.ICD2 + '</td><td>' + item.ICD3 + '</td><td>' + item.ICD4 + '</td><td>' + item.Modifier1 + '</td><td>' + item.Modifier2 + '</td><td>' + item.Modifier3 + '</td><td>' + item.Modifier4 + '</td><td>' + utility.convertToFigure(item.Fee, true) + '</td><td class=txt-align-right> $' + parseFloat(item.PlanAmount).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class=txt-align-right> $' + parseFloat(item.PlanBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class=txt-align-right> $' + parseFloat(item.PATAmount).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class=txt-align-right>$' + parseFloat(item.CopayAmount).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class=txt-align-right>$' + parseFloat(item.PATPaid).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class=txt-align-right>$' + parseFloat(item.PATBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td data-toggle="tooltip" data-placement="left" title="' + item.Status + '">' + item.Status + '</td><td>' + item.SubmittedDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.Case + '</td><td class="size-max50 ellipses" data-toggle="tooltip" data-placement="left" title="' + item.EntryDate + '">' + item.EntryDate + '</td><td>' + item.CreatedByName + '</td>');


                $("#pnlBillchargesViewDetail_Result #dgvChargesView tbody").last().append($row);
            });
        }
        else {
            $('#dgvChargesView').DataTable({
                "language": {
                    "emptyTable": "No Charge Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#dgvChargesView'))
            ;
        else
            $("#pnlBillchargesViewDetail_Result #dgvChargesView").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        $("#pnlBillchargesViewDetail_Result div.table-responsive").css("overflow", "auto");
    },

    ChargesViewDetailLoad: function (BatchNumber, BatchId) {
        var data = "BatchNumber=" + BatchNumber + "&BatchId=" + BatchId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_CHARGEVIEW_DETAIL", "SEARCH_CHARGEVIEW_DETAIL");
    },

    UnLoad: function () {
        // if ($('#frmchargesViewDetail').serialize() != $('#frmchargesViewDetail').data('serialize')) {
        //   utility.myConfirm('2', function () {
        if (chargesViewDetail.params != null && chargesViewDetail.params.ParentCtrl != null) {
            UnloadActionPan(chargesViewDetail.params.ParentCtrl, "chargesViewDetail");
        }
            // }, function () { },
            //       '2'
            //);
            // }
        else {
            UnloadActionPan();
        }
    },
}