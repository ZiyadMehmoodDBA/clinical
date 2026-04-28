
Admin_Practice = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_Practice.params = params;

        if (Admin_Practice.params["FromAdmin"] == "0" && Admin_Practice.params["PanelID"] == 'pnlAdminPractice')
            Admin_Practice.params["FromAdmin"] = "1";

        if (Admin_Practice.bIsFirstLoad) {
            Admin_Practice.bIsFirstLoad = false;
            var practicePanelID = "pnlAdminPractice";
            if (Admin_Practice.params != null && Admin_Practice.params.PanelID != "pnlAdminPractice") {
                practicePanelID = Admin_Practice.params["PanelID"] + ' #pnlAdminPractice';
            }
            var self = $('#' + practicePanelID);
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find('#ddlEntity').attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find('#ddlEntity').val(globalAppdata["SeletedEntityId"]);
                }
                Admin_Practice.PracticeSearch();
            });
            $('#' + Admin_Practice.params["PanelID"] + ' #pnlPractice_Result #ColumnAction').addClass("size95");
            if (Admin_Practice.params != null) {
                if (Admin_Practice.params.ParentCtrl != null) {
                    if (Admin_Practice.params.ParentCtrl == "Scheduling_Search") {
                        $('#' + Admin_Practice.params["PanelID"] + ' #pnlPractice_Result #ColumnAction').removeClass("size95").addClass("size130");
                    }
                }
            }


        }
    },

    PracticeAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Practice", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PracticeId"] = null;
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_Practice.params["FromAdmin"];
                if (Admin_Practice.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_Practice';
                }
                LoadActionPan('practiceDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PracticeEdit: function (PracticeId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#" + Admin_Practice.params["PanelID"] + ' #gvPractice_row' + PracticeId));
        AppPrivileges.GetFormPrivileges("Practice", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = PracticeId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["PracticeId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Admin_Practice.params["FromAdmin"];
                    if (Admin_Practice.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Admin_Practice';
                    }
                    LoadActionPan('practiceDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PracticeDelete: function (PracticeId, event, obj) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        utility.SelectGridRow($("#" + Admin_Practice.params["PanelID"] + ' #gvPractice_row' + PracticeId));
        AppPrivileges.GetFormPrivileges("Practice", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = PracticeId;
                    var oTable = $('#dgvPractice').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_Practice.DeletePractice(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $("#" + Admin_Practice.params["PanelID"] + ' #dgvPractice').DataTable();
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

    PracticeActiveInactive: function (PracticeId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Practice", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = PracticeId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        practiceDetail.UpdatePracticeActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_Practice.PracticeSearch('0');
                                // UnloadActionPan();
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

    PracticeSearch: function (PracticeId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Admin_Practice.params["PanelID"] + ' #pnlPractice_Result').css("display") == "none") {
                    $('#' + Admin_Practice.params["PanelID"] + ' #pnlPractice_Result').show();
                }

                var self = $('#' + Admin_Practice.params["PanelID"] + ' #pnlPractice_Search');
                var myJSON = self.getMyJSON();

                Admin_Practice.SearchPractice(myJSON, PracticeId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_Practice.PracticeGridLoad(response);
                        var TableControl = Admin_Practice.params["PanelID"] + " #dgvPractice";
                        var PagingPanelControlID = Admin_Practice.params["PanelID"] + " #divPracticePaging";
                        var ClassControlName = "Admin_Practice";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.PracticeCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_Practice.PracticeSearch(PrimaryID, PageNumber, ResultPerPage);
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

    PracticeGridLoad: function (response) {
        $('#' + Admin_Practice.params["PanelID"] + ' #dgvPractice').dataTable().fnDestroy();
        $('#' + Admin_Practice.params["PanelID"] + ' #pnlPractice_Result #dgvPractice tbody').find("tr").remove();
        if (response.PracticeCount > 0) {
            var PracticeLoadJSONData = JSON.parse(response.PracticeLoad_JSON);
            $.each(PracticeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvPractice_row" + item.PracticeId);
                $row.attr("PracticeId", item.PracticeId);
                $row.attr("ShortName", item.ShortName);
                $row.attr("Description", item.DESCRIPTION);
                $row.attr("PhoneNo", item.PhoneNo);
                $row.attr("Address", item.Address);

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
                var selectPractices = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                if (Admin_Practice.params["FromAdmin"] == "0") {
                    var selectMethod = "Admin_Practice.FillPractices('" + item.PracticeId + "','" + item.ShortName + "');"
                    selectPractices = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_Practice.PracticeEdit('" + item.PracticeId + "',event);");
                }
                if (Admin_Provider.params["PanelID"] == "pnlReportsSSRSDashboard") {
                    $('#btn-add').hide();
                    $row.append('<td style="display:none;">' + item.PracticeId + '</td><td>' + selectPractices + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.EIN + '</td><td>' + item.EntityName + '</td>');
                } else {
                    $('#btn-add').show();
                    $row.append('<td style="display:none;">' + item.PracticeId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_Practice.PracticeDelete(\'' + item.PracticeId + '\',event,this);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Practice.PracticeEdit(\'' + item.PracticeId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_Practice.PracticeActiveInactive(\'' + item.PracticeId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectPractices + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.EIN + '</td><td>' + item.EntityName + '</td>');
                }
                $('#' + Admin_Practice.params["PanelID"] + ' #pnlPractice_Result #dgvPractice tbody').last().append($row);
            });
        }
        else {
            $('#' + Admin_Practice.params["PanelID"] + ' #dgvPractice').DataTable({
                "language": {
                    "emptyTable": "No Practice Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_Practice.params["PanelID"] + ' #dgvPractice'))
            ;
        else
            $('#' + Admin_Practice.params["PanelID"] + ' #pnlPractice_Result #dgvPractice').DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        // $('#' + Admin_Practice.params["PanelID"] + ' #pnlPractice_Result #dgvPractice').DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$('#pnlPractice_Result #dgvPractice_info').html("Total Records: " + response.PracticeCount);
    },

    FillPractices: function (PracticeId, ShortName) {

        var RefCtrl = " #txtPractice";
        var RefHiddenIdCtrl = " #hfPractice";
        if (Admin_Practice.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_Practice.params["RefCtrl"];
        }
        if (Admin_Practice.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_Practice.params["RefHiddenIdCtrl"];
        }
        $('#' + Admin_Practice.params["PanelID"] + RefCtrl).val(ShortName).focus();
        $('#' + Admin_Practice.params["PanelID"] + RefHiddenIdCtrl).val(PracticeId);
        $('#' + Admin_Practice.params["PanelID"] + ' #lblPractice').css("display", "none");
        $('#' + Admin_Practice.params["PanelID"] + ' #lnkPracticeEdit').css("display", "inline");

        if (Admin_Practice.params["PanelID"] == "pnlReportsSSRSDashboard") {
            Admin_Practice.params["ProviderId"] = PracticeId;
            $('#' + Admin_Practice.params["PanelID"] + ' #lblPractice').css("display", "inline");
        }
        utility.SetKendoAutoCompleteSourceforValidate($('#' + Admin_Practice.params["PanelID"] + RefCtrl), ShortName, $('#' + Admin_Practice.params["PanelID"] + RefHiddenIdCtrl), PracticeId);
        if (Admin_Practice.params != null && Admin_Practice.params.ParentCtrl != null && Admin_Practice.params.PanelID != 'pnlAdminPractice') {
            UnloadActionPan(Admin_Practice.params.ParentCtrl, 'Admin_Practice', null, Admin_Practice.params.PanelID);
        } else {

            UnloadActionPan(Admin_Practice.params["ParentCtrl"], "Admin_Practice");
        }
    },

    SearchPractice: function (PracticeData, practiceID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "PracticeData=" + PracticeData + "&practiceID=" + practiceID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRACTICE", "SEARCH_PRACTICE");
    },

    DeletePractice: function (practiceID) {
        var data = "PracticeID=" + practiceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRACTICE_DETAIL", "DELETE_PRACTICE");
    },

    LoadPracticeDBCall: function (ShortName) {
        var data = "ShortName=" + ShortName;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRACTICE", "LOAD_PRACTICE_LOOKUP");
    },

    UnLoadTab: function () {
        if (Admin_Practice.params["FromAdmin"] == "0") {
            if (Admin_Practice.params != null && Admin_Practice.params.ParentCtrl != null) {
                UnloadActionPan(Admin_Practice.params.ParentCtrl, 'Admin_Practice');
            }
            else
                UnloadActionPan(null, 'Admin_Practice');

        }
        else {
            RemoveAdminTab();
        }
    },
}

