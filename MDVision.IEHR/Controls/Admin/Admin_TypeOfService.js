
Admin_TypeOfService = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_TypeOfService.bIsFirstLoad) {
            Admin_TypeOfService.bIsFirstLoad = false;

            AppPrivileges.GetFormPrivileges("Type Of Service", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_TypeOfService.TypeOfServiceSearch();
                }
            });
        }
    },

    TypeOfServiceAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Type Of Service", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["TypeOfServiceId"] = "-1";
                params["mode"] = "Add";
                LoadActionPan('typeOfServiceDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    TypeOfServiceEdit: function (TypeOfServiceId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvTypeOfService_row' + TypeOfServiceId));
        AppPrivileges.GetFormPrivileges("Type Of Service", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = TypeOfServiceId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["TypeOfServiceId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('typeOfServiceDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    TypeOfServiceDelete: function (TypeOfServiceId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvTypeOfService_row' + TypeOfServiceId));
        AppPrivileges.GetFormPrivileges("Type Of Service", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = TypeOfServiceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_TypeOfService.DeleteTypeOfService(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvTypeOfService').DataTable();
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

    TypeOfServiceActiveInactive: function (TypeOfServiceId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Type Of Service", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = TypeOfServiceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        typeOfServiceDetail.UpdateTypeOfServiceActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_TypeOfService.TypeOfServiceSearch('0');
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

    TypeOfServiceSearch: function (TypeOfServiceId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Type Of Service", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminTypeOfService #pnlTypeOfService_Result").css("display") == "none") {
                    $("#pnlAdminTypeOfService #pnlTypeOfService_Result").show();
                }

                var self = $("#pnlTypeOfService_Search");
                var myJSON = self.getMyJSON();

                Admin_TypeOfService.SearchTypeOfService(myJSON, TypeOfServiceId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_TypeOfService.TypeOfServiceGridLoad(response);
                        var TableControl = "pnlAdminTypeOfService #dgvTypeOfService";
                        var PagingPanelControlID = "pnlAdminTypeOfService #divTypeOfServicePaging";
                        var ClassControlName = "Admin_TypeOfService";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.TypeOfServiceCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_TypeOfService.TypeOfServiceSearch(PrimaryID, PageNumber, ResultPerPage);
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

    TypeOfServiceGridLoad: function (response) {
        $("#dgvTypeOfService").dataTable().fnDestroy();
        $("#pnlTypeOfService_Result #dgvTypeOfService tbody").find("tr").remove();
        if (response.TypeOfServiceCount > 0) {
            var TypeOfServiceLoadJSONData = JSON.parse(response.TypeOfServiceLoad_JSON);
            $.each(TypeOfServiceLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_TypeOfService.TypeOfServiceEdit('" + item.TOSId + "',event);");
                $row.attr("id", "gvTypeOfService_row" + item.TOSId);
                $row.attr("TypeOfServiceId", item.TOSId);


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

                $row.append('<td style="display:none;">' + item.TOSId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_TypeOfService.TypeOfServiceDelete(\'' + item.TOSId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_TypeOfService.TypeOfServiceEdit(\'' + item.TOSId + '\',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_TypeOfService.TypeOfServiceActiveInactive(\'' + item.TOSId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.TypeOfServiceCode + '</td><td>' + item.Name + '</td><td>' + item.Description + '</td>');

                $("#pnlTypeOfService_Result #dgvTypeOfService tbody").last().append($row);
            });
        }
        else {
            $('#dgvTypeOfService').DataTable({
                "language": {
                    "emptyTable": "No Type Of Service Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvTypeOfService'))
            ;
        else
            $("#pnlTypeOfService_Result #dgvTypeOfService").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchTypeOfService: function (TypeOfServiceData, TypeOfServiceId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "TypeOfServiceData=" + TypeOfServiceData + "&TypeOfServiceID=" + TypeOfServiceId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE", "SEARCH_TYPE_OF_SERVICE");
    },

    DeleteTypeOfService: function (TypeOfServiceId) {
        var data = "TypeOfServiceID=" + TypeOfServiceId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "DELETE_TYPE_OF_SERVICE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
    ValidateTypeOfService: function (TypeOfServiceId) {
        return Admin_TypeOfService.SearchTypeOfService("", TypeOfServiceId).done(function (response) {
            if (response.iTotalDisplayRecords == 0) {
                utility.DisplayMessages("Invalid TOS", 3);
            }
        });
    },
}