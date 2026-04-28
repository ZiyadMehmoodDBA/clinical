
Admin_EDIReceiver = {
    bIsFirstLoad: true,
    params:null,
    Load: function (params) {
        Admin_EDIReceiver.params = params;
        if (Admin_EDIReceiver.bIsFirstLoad) {
            Admin_EDIReceiver.bIsFirstLoad = false;
            var self = "";
            if (Admin_EDIReceiver.params["PanelID"] != 'pnlAdminEDIReceiver')
                self = $('#' + Admin_EDIReceiver.params["PanelID"] + ' #pnlAdminEDIReceiver');
            else
                self = $('#pnlAdminEDIReceiver');

            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }

            self.loadDropDowns(true).done(function () {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_EDIReceiver.ReceiverSetupSearch();

            });
        }
    },

    ReceiverSetupAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Receiver", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["EDIReceiverId"] = "-1";
                params["EDIReceiverX12Id"] = "-1";
                params["mode"] = "Add";
                params["X12mode"] = "Add";
                LoadActionPan('EDIReceiverDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ReceiverSetupEdit: function (ReceiverSetupId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvReceiverSetup_row' + ReceiverSetupId));
        AppPrivileges.GetFormPrivileges("EDI Receiver", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ReceiverSetupId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["EDIReceiverId"] = selectedValue;
                    params["EDIReceiverX12Id"] = null;
                    params["mode"] = "Edit";
                    params["X12mode"] = "Edit";
                    LoadActionPan('EDIReceiverDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ReceiverSetupDelete: function (ReceiverSetupId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvReceiverSetup_row' + ReceiverSetupId));
        AppPrivileges.GetFormPrivileges("EDI Receiver", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ReceiverSetupId;
                    var oTable = $('#dgvReceiverSetup').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_EDIReceiver.DeleteReceiverSetup(selectedValue).done(function (response) {
                            if (response.status != false) {
                                Admin_EDIReceiver.ReceiverSetupSearch();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ReceiverSetupActiveInactive: function (ReceiverSetupId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("EDI Receiver", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ReceiverSetupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        EDIReceiverDetail.UpdateEDIReceiverActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_EDIReceiver.ReceiverSetupSearch('0');
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

    ReceiverSetupSearch: function (ReceiverSetupId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Receiver", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminEDIReceiver #pnlReceiverSetup_Result").css("display") == "none") {
                    $("#pnlAdminEDIReceiver #pnlReceiverSetup_Result").show();
                }

                var self = $("#pnlReceiverSetup_Search");
                var myJSON = self.getMyJSON();

                Admin_EDIReceiver.SearchReceiverSetup(myJSON, ReceiverSetupId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_EDIReceiver.ReceiverSetupGridLoad(response);
                        var TableControl = "pnlAdminEDIReceiver #dgvReceiverSetup";
                        var PagingPanelControlID = "pnlAdminEDIReceiver #divReceiverSetupPaging";
                        var ClassControlName = "Admin_EDIReceiver";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ReceiverSetupCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_EDIReceiver.ReceiverSetupSearch(PrimaryID, PageNumber, ResultPerPage);
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

    ReceiverSetupGridLoad: function (response) {
        $("#dgvReceiverSetup").dataTable().fnDestroy();
        $("#pnlReceiverSetup_Result #dgvReceiverSetup tbody").find("tr").remove();
        if (response.ReceiverSetupCount > 0) {
            var ReceiverSetupLoadJSONData = JSON.parse(response.ReceiverSetupLoad_JSON);
            $.each(ReceiverSetupLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_EDIReceiver.ReceiverSetupEdit('" + item.EDIReceiverSetupId + "',event);utility.SelectGridRow($(this));");
                $row.attr("id", "gvReceiverSetup_row" + item.EDIReceiverSetupId);
                $row.attr("ReceiverSetupId", item.EDIReceiverSetupId);
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
                $row.append('<td style="display:none;">' + item.EDIReceiverSetupId + '</td><td><a class="btn  btn-xs" href="#"onclick="Admin_EDIReceiver.ReceiverSetupDelete(' + item.EDIReceiverSetupId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_EDIReceiver.ReceiverSetupEdit(' + item.EDIReceiverSetupId + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Admin_EDIReceiver.ReceiverSetupActiveInactive(' + item.EDIReceiverSetupId + ',' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.SubmitterID + '</td><td>' + item.ANSI5010 + '</td><td>' + item.ReceiverId + '</td><td>' + item.ClearingHouseName + '</td>');

                $("#pnlReceiverSetup_Result #dgvReceiverSetup tbody").last().append($row);
            });
        }
        else {
            $('#dgvReceiverSetup').DataTable({
                "language": {
                    "emptyTable": "No EDI Receiver Setup found"
                },
                "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvReceiverSetup'))
            ;
        else
            $("#pnlReceiverSetup_Result #dgvReceiverSetup").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchReceiverSetup: function (ReceiverSetupData, ReceiverSetupID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ReceiverSetupData=" + ReceiverSetupData + "&ReceiverSetupID=" + ReceiverSetupID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RECEIVER_SETUP", "SEARCH_RECEIVER_SETUP");
    },

    DeleteReceiverSetup: function (ReceiverSetupID) {
        var data = "ReceiverSetupID=" + ReceiverSetupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RECEIVER_SETUP_DETAIL", "DELETE_RECEIVER_SETUP");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}

