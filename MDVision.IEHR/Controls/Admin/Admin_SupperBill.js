Admin_SupperBill = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {

        if (Admin_SupperBill.bIsFirstLoad) {
            Admin_SupperBill.bIsFirstLoad = false;

            var self = $('#pnlAdminSupperBill');
            self.loadDropDowns(true).done(function () {

                AppPrivileges.GetFormPrivileges("Super Bill", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Admin_SupperBill.SupperBillSearch();
                    }
                });
            }); 

           
        }
    },

    SupperBillAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Super Bill", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["BillId"] = "-1";
                params["mode"] = "Add";
                LoadActionPan('SupperBillDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SupperBillSearch: function (SuperBillID, PageNo, rpp) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Super Bill", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminSupperBill #pnlSupperBill_Result").css("display") == "none") {
                    $("#pnlAdminSupperBill #pnlSupperBill_Result").show();
                }

                var self = $("#pnlAdminSupperBill #frmSupperBill");
                var myJSON = self.getMyJSON();

                Admin_SupperBill.SearchSupperBill(myJSON, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_SupperBill.SupperBillGridLoad(response);
                        var TableControl = "pnlAdminSupperBill #dgvSupperBill";
                        var PagingPanelControlID = "pnlAdminSupperBill #divSupperBillPaging";
                        var ClassControlName = "Admin_SupperBill";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.BillCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_SupperBill.SupperBillSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
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

    SupperBillGridLoad: function (response) {

        $("#pnlAdminSupperBill #dgvSupperBill").dataTable().fnDestroy();
        $("#pnlAdminSupperBill #pnlSupperBill_Result #dgvSupperBill tbody").find("tr").remove();

        if (response.BillCount > 0) {

            var BillLoadJSONData = JSON.parse(response.BillLoad_JSON);
            $.each(BillLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_SupperBill.SupperBillEdit('" + item.SuperBillId + "',event);");
                $row.attr("id", "gvSupperBill_row" + item.SuperBillId);
                $row.attr("BillId", item.SuperBillId);
                $row.attr("ShortName", item.ShortName);
                $row.attr("Description", item.Description);
                $row.attr("Practice", item.PracticeName);

                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                $row.append('<td style="display:none;">' + item.SuperBillId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_SupperBill.SupperBillDelete(\'' + item.SuperBillId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_SupperBill.SupperBillEdit(\'' + item.SuperBillId + '\',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_SupperBill.SupperBillActiveInactive(\'' + item.SuperBillId + '\',' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PracticeName + '</td>');

                $("#pnlSupperBill_Result #dgvSupperBill tbody").last().append($row);
            });
        }
        else {
            $('#pnlAdminSupperBill #dgvSupperBill').DataTable({
                "language": {
                    "emptyTable": "No Super Bill Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#pnlAdminSupperBill #dgvSupperBill')) {
            
        }
        else
            $("#pnlAdminSupperBill #pnlSupperBill_Result #dgvSupperBill").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SupperBillDelete:function (BillId,event)
    {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#pnlAdminSupperBill #gvSupperBill_row' + BillId));
        AppPrivileges.GetFormPrivileges("Super Bill", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                    utility.myConfirm('1', function () {
                        var selectedValue = BillId;
                        var oTable = $('#dgvSupperBill').DataTable();
                        var ind = $(this).index();
                        var idx = oTable.row(this).index();
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Admin_SupperBill.DeleteSupperBill(selectedValue).done(function (response) {
                                if (response.status != false) {
                                    var table1 = $('#pnlAdminSupperBill #dgvSupperBill').DataTable();
                                    table1.row('.active').remove().draw(false);
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () {

                    },
                        '1'
                    );
             
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SupperBillEdit: function (BillId,event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#pnlAdminSupperBill #gvSupperBill_row' + BillId));
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Super Bill", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = BillId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["BillId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('SupperBillDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SupperBillActiveInactive: function (BillId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Super Bill", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                    utility.myConfirm('3', function () {
                        var selectedValue = BillId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {

                            Admin_SupperBill.UpdateBillActiveInactive(selectedValue, IsActive).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    Admin_SupperBill.SupperBillSearch();
                                    UnloadActionPan();
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { },
                        '3', null, null, null, IsActive
                    );
                
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    UpdateBillActiveInactive: function (BillID, IsActive) {
        var data = "BillID=" + BillID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL", "UPDATE_SUPPER_BILL_ACTIVE_INACTIVE");
    },

    DeleteSupperBill: function (BillId) {
        var data = "BillId=" + BillId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL", "DELETE_SUPPER_BILL");
    },

    SearchSupperBill: function (SupperBillData, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "SupperBillData=" + SupperBillData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL", "SEARCH_SUPPER_BILL");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },

};



