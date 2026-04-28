Bill_ERALinkedCharge_History = {
    params: null,
    bIsFirstLoad: true,
    self: null,
    Load: function (params) {
        Bill_ERALinkedCharge_History.params = params;

        //if (Bill_ERACharge_Detail.params.PanelID != "pnlBillERAChargeDetailSearch") {
        //    Bill_ERACharge_Detail.self = $('#' + Bill_ERACharge_Detail.params.PanelID + ' #pnlBillERAChargeDetailSearch');
        //    Bill_ERACharge_Detail.params.PanelID = Bill_ERACharge_Detail.params.PanelID + ' #pnlBillERAChargeDetailSearch';
        //}
        //else
        //    Bill_ERACharge_Detail.self = $('#' + Bill_ERACharge_Detail.params.PanelID);
        //Bill_ERACharge_Detail.ActiveControls(false);
        Bill_ERALinkedCharge_History.ERALinkedChargeHistoryLoad();

        //if (Bill_ERACharge_Detail.params['ParentCtrl'] == 'Bill_ERA_Summary')
        //    $("#" + Bill_ERACharge_Detail.params.PanelID + " #btnLinkChange").css("display", "none");


    },
    ERALinkedChargeHistoryLoad: function ()
    {
        AppPrivileges.GetFormPrivileges("ERA", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Bill_ERALinkedCharge_History.LoadERALinkedChargeHistory(Bill_ERALinkedCharge_History.params.ERADetailID).done(function (response) {
                    if (response.status != false) {
                        $("#" + Bill_ERALinkedCharge_History.params["PanelID"] + " #dgvLinkedChargeHistory").dataTable().fnDestroy();
                        $("#" + Bill_ERALinkedCharge_History.params["PanelID"] + " #dgvLinkedChargeHistory tbody").find("tr").remove();
                       
                        var responseData = JSON.parse(response.LinkedHistoryLoad_JSON);
                        if (responseData.length > 0) {

                            $.each(responseData, function (i, item) {
                                var $row = $('<tr/>');
                                $row.attr("onclick", "utility.SelectGridRow($('#dgvLinkedChargeHistory_row" + i + "'))");
                                $row.attr("id", "dgvLinkedChargeHistory_row" + i);

                                $row.append('<td>' + item.Action + '</td><td>' + item.User + '</td><td>' + item.Date + '</td><td>' + item.ClaimNumber + '</td><td>' + item.CPT + '</td>');

                                $("#" + Bill_ERALinkedCharge_History.params["PanelID"] + " #dgvLinkedChargeHistory tbody").last().append($row);
                            });
                        }
                        else {
                            $("#" + Bill_ERALinkedCharge_History.params["PanelID"] + " #dgvLinkedChargeHistory").DataTable({
                                "language": {
                                    "emptyTable": "No Record Found"
                                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                            });
                        }
                        //Set ToolTip for Comments.
                        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
                        if ($.fn.dataTable.isDataTable("#" + Bill_ERALinkedCharge_History.params["PanelID"] + " #dgvLinkedChargeHistory"))
                            ;
                        else
                            $("#" + Bill_ERALinkedCharge_History.params["PanelID"] + " #pnlERALinkedChargeActivityLog_Result #dgvLinkedChargeHistory").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
                             $("#pnlERALinkedChargeActivityLog_Result div.table-responsive").css("overflow", "auto");

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

    LoadERALinkedChargeHistory : function(ERAId)
    {
        var objData = new JSON.constructor();
        objData["ERADetailID"] = ERAId;
        objData["CommandType"] = "Load_Linked_History";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");
    },

    UnLoad: function () {
        UnloadActionPan(Bill_ERALinkedCharge_History.params.ParentCtrl, "Bill_ERALinkedCharge_History");
    },
}