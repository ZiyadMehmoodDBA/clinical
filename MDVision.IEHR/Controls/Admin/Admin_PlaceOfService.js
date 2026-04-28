
Admin_PlaceOfService = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_PlaceOfService.bIsFirstLoad) {
            Admin_PlaceOfService.bIsFirstLoad = false;

            AppPrivileges.GetFormPrivileges("Place Of Service", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_PlaceOfService.PlaceOfServiceSearch();
                }
            });
        }
    },

    PlaceOfServiceAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Place Of Service", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PlaceOfServiceId"] = "-1";
                params["mode"] = "Add";
                LoadActionPan('placeOfServiceDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PlaceOfServiceEdit: function (PlaceOfServiceId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPlaceOfService_row' + PlaceOfServiceId));
        AppPrivileges.GetFormPrivileges("Place Of Service", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = PlaceOfServiceId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["PlaceOfServiceId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('placeOfServiceDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PlaceOfServiceDelete: function (PlaceOfServiceId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPlaceOfService_row' + PlaceOfServiceId));
        AppPrivileges.GetFormPrivileges("Place Of Service", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = PlaceOfServiceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_PlaceOfService.DeletePlaceOfService(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvPlaceOfService').DataTable();
                                table1.row('.active').remove().draw(false);
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

    PlaceOfServiceActiveInactive: function (PlaceOfServiceId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Place Of Service", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = PlaceOfServiceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        placeOfServiceDetail.UpdatePlaceOfServiceActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_PlaceOfService.PlaceOfServiceSearch('0');
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

    PlaceOfServiceSearch: function (PlaceOfServiceId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Place Of Service", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminPlaceOfService #pnlPlaceOfService_Result").css("display") == "none") {
                    $("#pnlAdminPlaceOfService #pnlPlaceOfService_Result").show();
                }

                var self = $("#pnlPlaceOfService_Search");
                var myJSON = self.getMyJSON();

                Admin_PlaceOfService.SearchPlaceOfService(myJSON, PlaceOfServiceId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_PlaceOfService.PlaceOfServiceGridLoad(response);
                        var TableControl =  "pnlAdminPlaceOfService #dgvPlaceOfService";
                        var PagingPanelControlID = "pnlAdminPlaceOfService #divPlaceOfServicePaging";
                        var ClassControlName = "Admin_PlaceOfService";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.PlaceOfServiceCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_PlaceOfService.PlaceOfServiceSearch(PrimaryID, PageNumber, ResultPerPage);
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

    PlaceOfServiceGridLoad: function (response) {
        $("#dgvPlaceOfService").dataTable().fnDestroy();
        $("#pnlPlaceOfService_Result #dgvPlaceOfService tbody").find("tr").remove();
        if (response.PlaceOfServiceCount > 0) {
            var PlaceOfServiceLoadJSONData = JSON.parse(response.PlaceOfServiceLoad_JSON);
            $.each(PlaceOfServiceLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_PlaceOfService.PlaceOfServiceEdit('" + item.POSId + "',event);");
                $row.attr("id", "gvPlaceOfService_row" + item.POSId);
                $row.attr("PlaceOfServiceId", item.POSId);


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

                $row.append('<td style="display:none;">' + item.POSId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_PlaceOfService.PlaceOfServiceDelete(\'' + item.POSId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_PlaceOfService.PlaceOfServiceEdit(\'' + item.POSId + '\',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_PlaceOfService.PlaceOfServiceActiveInactive(\'' + item.POSId + '\', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.POSCode + '</td><td>' + item.Description + '</td>');

                $("#pnlPlaceOfService_Result #dgvPlaceOfService tbody").last().append($row);
            });
        }
        else {
            $('#dgvPlaceOfService').DataTable({
                "language": {
                    "emptyTable": "No Place Of Service Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvPlaceOfService'))
            ;
        else
            $("#pnlPlaceOfService_Result #dgvPlaceOfService").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchPlaceOfService: function (PlaceOfServiceData, PlaceOfServiceId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "PlaceOfServiceData=" + PlaceOfServiceData + "&PlaceOfServiceID=" + PlaceOfServiceId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE", "SEARCH_PLACE_OF_SERVICE");
    },

    DeletePlaceOfService: function (PlaceOfServiceId) {
        var data = "PlaceOfServiceID=" + PlaceOfServiceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "DELETE_PLACE_OF_SERVICE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}