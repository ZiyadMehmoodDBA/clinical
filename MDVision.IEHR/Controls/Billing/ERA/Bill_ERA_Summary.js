Bill_ERA_Summary = {
    params: [],
    bIsFirstLoad: true,

    Load: function (params) {
        Bill_ERA_Summary.params = params;

        if (Bill_ERA_Summary.bIsFirstLoad) {
            Bill_ERA_Summary.bIsFirstLoad = false;

            Bill_ERA_Summary.LoadERASummary();
        }

        $("#pnlERASummary #chkAllVisits").change(function () {

            if ($(this).prop('checked'))
                $(this).attr('title', 'Unselect all');
            else
                $(this).attr('title', 'Select all');

            $("#pnlERASummary #dgvERASummary").find("input[type='checkbox']")
                .prop('checked', this.checked);
        });
    },

    LoadERASummary: function () {
    
        $("#pnlERASummary #dgvERASummary").dataTable().fnDestroy();
        $("#pnlERASummary #dgvERASummary tbody").find("tr").remove();
        if (Bill_ERA_Summary.params.UnPostedChargesCount > 0) {

            var UnPostedChargesJSONData = JSON.parse(Bill_ERA_Summary.params.UnPostedCharges_JSON);
            $.each(UnPostedChargesJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#dgvERASummary_row" + item.ERADtlId + "'))");
                $row.attr("id", "gvERASummary_row" + item.ERADtlId);
                $row.attr("ERADtlId", item.ERADtlId);

                var VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'Bill_ERA_Summary', event);";

                $row.append('<td style="display:none;">' + item.ERADtlId + '</td>'
                           + '<td><input onclick="Bill_ERA_Summary.CheckedVisit(this,event)"  type="checkbox" name="checkbox" id="' + item.ERADtlId + '"></td>'
                           + '<td><a class="btn  btn-xs btn-element" href="#" onclick="Bill_ERA_Summary.OpenPaymentPosting(' + "'" + item.ClaimNumber + "'" + ',' + "'" + item.ChargeId + "'" + ');" title="Payment Posting"><i class="fa fa-credit-card green"></i></a>'
                           + '<a class="btn  btn-xs btn-element" href="#" onclick="Bill_ERA_Summary.PostCharge(this,' + item.ERADtlId + ',' + item.ERAId + ',' + item.InsBalance + ');" title="Post Payment"><i class="fa fa-dollar green"></i></a></td>'
                           + '<td><a class="btn  btn-xs btn-element" href="#" onclick="Bill_ERA_Summary.OpenChargeDetail(' + item.ERADtlId + ',' + item.ERAId + ');" >' + item.CheckNo + '</a></td>'
                           + '<td> <a class="btn  btn-xs btn-element" href="#" onclick="' + VisitDetail + '" >' + item.ClaimNumber + '</a></td>'
                           + '<td>' + item.CPTCode + '</td>'
                           + '<td>' + utility.convertToFigure(item.InsBalance, true) + '</td>'
                           + '<td>' + utility.convertToFigure(item.PaidAmount, true) + '</td>'
                           + '<td>' + utility.convertToFigure(item.WriteOff, true) + '</td>'
                           + '<td>' + utility.convertToFigure(item.DeductableAmount, true) + '</td>'
                           + '<td>' + utility.convertToFigure(item.CoInsuranceAmount, true) + '</td>'
                           + '<td>' + utility.convertToFigure(item.Copayment, true) + '</td>'
                           + '<td>' + utility.convertToFigure(item.ChargeCopay, true) + '</td>'
                           + '<td>' + utility.convertToFigure(item.PatientResponsibility, true) + '</td>'
                           + '<td class="size-max200 ellipses" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '" >' + item.Comments + '</td>');

                $("#pnlERASummary #dgvERASummary tbody").last().append($row);
            });
        }
        else {
            $("#pnlERASummary #dgvERASummary").DataTable({
                "language": {
                    "emptyTable": "No UnPosted Charge Found"
                }, "autoWidth": false, "bLengthChange": false, "paging": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#pnlERASummary #dgvERASummary"))
            ;
        else
            $("#pnlERASummary #dgvERASummary").DataTable({ "bLengthChange": false, "autoWidth": false, "paging": false });

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

    },

    PostPayment: function () {

        var charges = Bill_ERA_Summary.GetSelectedCharges();
        if (charges.length > 0) {

            utility.myConfirm('13', function () {

                AppPrivileges.GetFormPrivileges("ERA", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        ERADetail.Post_Charges_Payment(Bill_ERA_Summary.params.ERAId, charges).done(function (response) {

                            if (response.status != false)
                            {
                                utility.DisplayMessages(response.Message, 1);
                                Bill_ERA_Summary.UnLoad();

                                //load grid again.
                                if (Bill_ERA_Summary.params.ParentCtrl != "billTabERA")
                                    ERADetail.LoadERADetail();

                                Bill_ERA.ERASearch();

                                
                            }
                            else
                                utility.DisplayMessages(response.Message, 3);

                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);

                });}, function () { },
                       '13'
                   );
           
        }
        else {
            utility.DisplayMessages("Please a select charge.", 2);
        }

    },

    PostCharge: function (obj, DetailId, ERAId, InsBalance) {

        var dfd = new $.Deferred();
        if (Number(InsBalance).toFixed(globalAppdata.DecimalPlaces) < 0) {

            utility.myConfirm('31', function () {

                //ok
                dfd.resolve("ok");

            }, function () { }, '31', "Continue posting", "Cancel");
        }
        else {
            dfd.resolve("ok");
        }

        $.when(dfd).then(function () {

            AppPrivileges.GetFormPrivileges("ERA", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    ERADetail.Post_Charges_Payment(ERAId, DetailId).done(function (response) {
                        if (response.status != false) {

                            $(obj).addClass("disabled");
                            $("#pnlERASummary #dgvERASummary").find("#" + DetailId).addClass("disabled");

                            utility.DisplayMessages(response.Message, 1);

                            //load grid again.
                            if (Bill_ERA_Summary.params.ParentCtrl != "billTabERA")
                                ERADetail.LoadERADetail();

                            Bill_ERA.ERASearch();

                        } else {

                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });

        });

    },

    GetSelectedCharges: function () {

        var Charges = [];
        $("#pnlERASummary #dgvERASummary").find("input[type='checkbox']").each(function (index) {
            if (index > 0 && $(this).prop('checked'))
                Charges.push($(this).attr("id"));
        });
        return Charges.toString();
    },

    CheckedVisit: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }


        if (!$(obj).prop('checked')) {
            $("#pnlERASummary #chkAllVisits").prop('checked', false);
            $("#pnlERASummary #chkAllVisits").attr('title', 'Select all');
        }
        else {

            var selected = [];
            $("#pnlERASummary #dgvERASummary tbody").find("input[type='checkbox']").each(function () {

                if (!$(this).is(":checked")) {
                    selected.push(this);
                }
            });

            if (selected.length > 0) {
                $("#pnlERASummary #chkAllVisits").prop('checked', false);
                $("#pnlERASummary #chkAllVisits").attr('title', 'Select all');

            }
            else {
                $("#pnlERASummary #chkAllVisits").prop('checked', true);
                $("#pnlERASummary #chkAllVisits").attr('title', 'Unselect all');
            }
        }
    },

    OpenChargeDetail: function (ERADtlId, ERAId) {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'Bill_ERA_Summary';
        params["ERAId"] = ERAId;
        params["ERADtId"] = ERADtlId;
        LoadActionPan('Bill_ERACharge_Detail', params);
    },
    OpenClaimDetail: function (VisitId) {


    },

    OpenPaymentPosting: function (ClaimNumber, ChargeId, IsSearchCharges) {

        if (IsSearchCharges == "" || IsSearchCharges == undefined || IsSearchCharges == null)
            IsSearchCharges = false;

        if (IsSearchCharges == true)
            ChargeId = 0;

        AppPrivileges.GetFormPrivileges("Payment Posting", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_ERA_Summary';
                params["ClaimNumber"] = ClaimNumber;
                params["ChargeId"] = ChargeId;
                params["IsSearchCharges"] = IsSearchCharges;
                params["PaymentRef"] = "Bill_ERA_Summary";

                LoadActionPan('Bill_PaymentPosting', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });


    },

    UnLoad: function () {

        UnloadActionPan(Bill_ERA_Summary.params["ParentCtrl"]);
    },
};