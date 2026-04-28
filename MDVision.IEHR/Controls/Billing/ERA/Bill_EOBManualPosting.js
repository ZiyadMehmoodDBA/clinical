Bill_EOBManualPosting = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {

        Bill_EOBManualPosting.params = params;
        if (Bill_EOBManualPosting.bIsFirstLoad) {
            Bill_EOBManualPosting.bIsFirstLoad = false;
            if (Bill_EOBManualPosting.params != null && Bill_EOBManualPosting.params.PanelID != "pnlBillEOBManualPosting") {
                Bill_EOBManualPosting.params["PanelID"] = Bill_EOBManualPosting.params["PanelID"] + ' #pnlBillEOBManualPosting';
            }
            else {
                Bill_EOBManualPosting.params["PanelID"] = "pnlBillEOBManualPosting"
            }

            var self = $('#' + Bill_EOBManualPosting.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
            });

            CacheManager.BindCodes('GetFacility', false).done(function (result) {
                var Ctrl = $("#" + Bill_EOBManualPosting.params["PanelID"] + " input#txtFacility");
                var hfCtrl = $("#" + Bill_EOBManualPosting.params["PanelID"] + " #hfFacility");
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
            });
        }
        utility.ValidateFromToDate('frmBillEOBManualPosting', 'dpFrmEntryDate', 'dpToEntryDate', true,null,null,null,true);
        utility.ValidateFromToDate('frmBillEOBManualPosting', 'dpFrmCheckDate', 'dpToCheckDate', true);
        Bill_EOBManualPosting.SearchEOBManualPosting();
    },
    SearchEOBManualPosting: function () {
        var self = $("#" + Bill_EOBManualPosting.params.PanelID + " #frmBillEOBManualPosting");
        
        var myJSON = self.getMyJSONByName()
        Bill_EOBManualPosting.EOBManualPostingSearch(myJSON).done(function (response) {
            Bill_EOBManualPosting.EOBManualPostingGrid(response);
        });
    },
    EOBManualPostingGrid: function (response) {

        //Bind Data in Table
        $("#" + Bill_EOBManualPosting.params.PanelID + " #pnlBillManualPosting_Result #dgvManualPosting").dataTable().fnDestroy();
        $("#" + Bill_EOBManualPosting.params.PanelID + " #pnlBillManualPosting_Result #dgvManualPosting tbody").find("tr").remove();
       

        if (response.EOBManualPostingData.length > 0) {
            $.each(response.EOBManualPostingData, function (i, item) {
                var $row = $('<tr/>');
                var EditMethod = "Bill_EOBManualPosting.ManualPostingEdit(" + item.Id + ",event);";
                var PostPaymentMethod = "Bill_EOBManualPosting.PostPayment(" + item.Id + ",event);";
                var DeleteManualPost = "Bill_EOBManualPosting.EOBManualPostDelete(" + item.Id + ",'" + item.CheckNo + "'" + ",event);";
                $row.append('<td>' + "<a href='#' class='btn btn-xs' href='#' onclick=" + DeleteManualPost + " title='Delete Payment'><i class='fa fa-remove red'></i></a><a href='#' class='btn btn-xs' href='#' onclick=" + EditMethod + " title='Edit Record'><i class='fa fa-edit black'></i></a>"
                   + "<a href='#' class='btn btn-xs' href='#' onclick=" + PostPaymentMethod + " title='Post Payment'><i class='fa fa-usd green'></i></a>" + '</td><td>' + item.PayerName + '</td><td>'
                    + item.CheckNo + '</td><td>' + item.CheckAmount + '</td><td>'
                    + "Paid Amount" + '</td><td>' + item.StatusName + '</td><td>' 
                    + utility.RemoveTimeFromDate(null, item.CheckDate)  + '</td><td>' + item.CreatedOn + '</td>');

                $("#" + Bill_EOBManualPosting.params.PanelID + " #pnlBillManualPosting_Result #dgvManualPosting tbody").last().append($row);
            });

        }
        else {

            $("#" + Bill_EOBManualPosting.params.PanelID + " #pnlBillManualPosting_Result #dgvManualPosting").DataTable({
                "language": {
                    "emptyTable": "No Record Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#pnlBillManualPosting_Result #dgvManualPosting'))
            ;
        else
            $("#pnlBillManualPosting_Result #dgvManualPosting").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": true, "bPaginate": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    EOBManualPostingSearch:function(jsonDate){
        var data = "EOBPostingData=" + jsonDate ;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "EOB_MANUAL_POSTING_SEARCH");
    },
    ManualPostingEdit:function(Id,event){
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["mode"] = "Edit";
        params["EOBId"] = Id;
        params["ParentCtrl"] = 'Bill_EOBManualPosting';
        params["FromAdmin"] = "0";
        LoadActionPan('Bill_Insurance_Payment_Detail', params);
    },
    PostPayment:function(Id,evnent){
        if (event != null) {
            event.stopPropagation();
        }
        Bill_Insurance_Payment_Detail.PostEOBManualPaymentData(Id, 0).done(function (response) {
            if (response.status != 'false') {
                utility.DisplayMessages(response.message, 1);
            }
        });;
    },
    EOBManualPostDelete: function (id,checkNumber, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm("Do you want to delete insurance payment of EOB Check # <b>" + checkNumber+"</b>", function () {
            Bill_EOBManualPosting.DeleteEOBManualPost(id).done(function (response) {

                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    Bill_EOBManualPosting.SearchEOBManualPosting();
                }
                else {
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }, function () { },
                'Confirm Delete'
            );
    },
    DeleteEOBManualPost: function (EOBId) {
        var data = "EOBId=" + EOBId;
        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "DELETE_MANUAL_PAYMENT");
    },
    Bill_EOBReset: function () {
      
        $('#' + Bill_EOBManualPosting.params["PanelID"] + ' #frmBillEOBManualPosting').resetAllControls();
        $('#' + Bill_EOBManualPosting.params["PanelID"] + ' #frmBillEOBManualPosting [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
        $('#' + Bill_EOBManualPosting.params["PanelID"] + ' #frmBillEOBManualPosting #dpToEntryDate').attr('disabled', true);
        $('#' + Bill_EOBManualPosting.params["PanelID"] + ' #frmBillEOBManualPosting #dpToCheckDate').attr('disabled', true);
        $('#' + Bill_EOBManualPosting.params["PanelID"] + ' #frmBillEOBManualPosting #txtCheckAmount').val("");
        if ($('#' + Bill_EOBManualPosting.params["PanelID"] + ' #frmBillEOBManualPosting #lnkFacilityEdit').is(":visible")) {
            $('#' + Bill_EOBManualPosting.params["PanelID"] + ' #frmBillEOBManualPosting #lnkFacilityEdit').hide();
            $('#' + Bill_EOBManualPosting.params["PanelID"] + ' #frmBillEOBManualPosting #lblFacility').show();
        }
        $('#' + Bill_EOBManualPosting.params["PanelID"] + ' #frmBillEOBManualPosting #ddlStatus').val(2);
    },
    OpenInsurancePaymentDetail:function(){
        var params = [];
        
        
        params["mode"] = "Add";
        params["ParentCtrl"] = 'Bill_EOBManualPosting';
        params["FromAdmin"] = "0";
        LoadActionPan('Bill_Insurance_Payment_Detail', params);

    },
    UnLoadTab: function (Tab) {

        if (Bill_EOBManualPosting.params["ParentCtrl"] == "Bill_EOBManualPosting") {

            UnloadActionPan("Bill_ERA");

        }

        else {
            RemoveAdminTab(Tab);
        }

    },

}