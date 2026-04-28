
Admin_PatientEligibilityService = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Admin_PatientEligibilityService.params = params;

        if (Admin_PatientEligibilityService.params["FromAdmin"] == "0" && Admin_PatientEligibilityService.params["PanelID"] == 'pnlAdminPatientEligibilityService')
            Admin_PatientEligibilityService.params["FromAdmin"] = "1";

        if (Admin_PatientEligibilityService.bIsFirstLoad) {
            Admin_PatientEligibilityService.bIsFirstLoad = false;

            var self = "";
            if (Admin_PatientEligibilityService.params["PanelID"] != 'pnlAdminPatientEligibilityService')
                self = $('#' + Admin_PatientEligibilityService.params["PanelID"] + ' #pnlAdminPatientEligibilityService');
            else
                self = $('#pnlAdminPatientEligibilityService');

            self.loadDropDowns(true).done(function () {
                Admin_PatientEligibilityService.PatientEligibilityServiceSearch();
            });
        }
    },

    PatientEligibilityServiceAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Patient Eligibility Service", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientEligibilityServiceID"] = null;
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_PatientEligibilityService.params["FromAdmin"];
                if (Admin_PatientEligibilityService.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_PatientEligibilityService';
                }
                LoadActionPan('PatientEligibilityServiceDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PatientEligibilityServiceEdit: function (PatientEligibilityServiceID,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPatientEligibilityService_row' + PatientEligibilityServiceID));
        AppPrivileges.GetFormPrivileges("Patient Eligibility Service", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = PatientEligibilityServiceID;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["PatientEligibilityServiceID"] = selectedValue;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Admin_PatientEligibilityService.params["FromAdmin"];
                    if (Admin_PatientEligibilityService.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Admin_PatientEligibilityService';
                    }
                    LoadActionPan('PatientEligibilityServiceDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PatientEligibilityServiceDelete: function (PatientEligibilityServiceID,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPatientEligibilityService_row' + PatientEligibilityServiceID));
        utility.SelectGridRow($("#" + Admin_PatientEligibilityService.params["PanelID"] + ' #gvPatientEligibilityService_row' + PatientEligibilityServiceID));
        AppPrivileges.GetFormPrivileges("Patient Eligibility Service", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = PatientEligibilityServiceID;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_PatientEligibilityService.DeletePatientEligibilityService(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#' + Admin_PatientEligibilityService.params["PanelID"] + ' #dgvPatientEligibilityService').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                //CacheManager.BindCodes('GetPatientEligibilityService', true);
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

    PatientEligibilityServiceActiveInactive: function (PatientEligibilityServiceID, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Patient Eligibility Service", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = PatientEligibilityServiceID;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        PatientEligibilityServiceDetail.UpdatePatientEligibilityServiceActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_PatientEligibilityService.PatientEligibilityServiceSearch('0');
                                //CacheManager.BindCodes('GetPatientEligibilityService', true);
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

    PatientEligibilityServiceSearch: function (PatientEligibilityServiceID, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Patient Eligibility Service", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_PatientEligibilityService.params["PanelID"] + " #pnlPatientEligibilityService_Result").css("display") == "none") {
                    $("#" + Admin_PatientEligibilityService.params["PanelID"] + " #pnlPatientEligibilityService_Result").show();
                }

                var self = $("#" + Admin_PatientEligibilityService.params["PanelID"] + " #pnlAdminPatientEligibilityService_Search");
                var myJSON = self.getMyJSON();

                Admin_PatientEligibilityService.SearchPatientEligibilityService(myJSON, PatientEligibilityServiceID, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_PatientEligibilityService.PatientEligibilityServiceGridLoad(response);
                        var TableControl = Admin_PatientEligibilityService.params["PanelID"] + " #dgvPatientEligibilityService";
                        var PagingPanelControlID = Admin_PatientEligibilityService.params["PanelID"] + " #divPatientEligibilityServicePaging";
                        var ClassControlName = "Admin_PatientEligibilityService";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.PatientEligibilityServiceCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_PatientEligibilityService.PatientEligibilityServiceSearch(PrimaryID, PageNumber, ResultPerPage);
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

    PatientEligibilityServiceGridLoad: function (response) {
        $("#" + Admin_PatientEligibilityService.params["PanelID"] + " #dgvPatientEligibilityService").dataTable().fnDestroy();
        $("#" + Admin_PatientEligibilityService.params["PanelID"] + " #pnlPatientEligibilityService_Result #dgvPatientEligibilityService tbody").find("tr").remove();
        if (response.PatientEligibilityServiceCount > 0) {
            var PatientEligibilityServiceLoad_JSON = JSON.parse(response.PatientEligibilityServiceLoad_JSON);
            $.each(PatientEligibilityServiceLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_PatientEligibilityService.PatientEligibilityServiceEdit('" + item.PatientEligibilityServiceID + "',event);utility.SelectGridRow($(this));");
                $row.attr("id", "gvPatientEligibilityService_row" + item.PatientEligibilityServiceID);
                $row.attr("PatientEligibilityServiceID", item.PatientEligibilityServiceID);
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

                var selectPatientEligibilityService = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                //if (Admin_PatientEligibilityService.params["FromAdmin"] == "0") {
                //    var selectMethod = "Admin_PatientEligibilityService.FillPatientEligibilityServiceName('" + item.PatientEligibilityServiceID + "','" + item.ShortName + "','" + item.PracticeId + "','" + item.PracticeName + "','" + item.EntityName + "',event);"
                //    selectPatientEligibilityService = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                //    $row.attr("onclick", selectMethod);
                //}
                //else {
                //    $row.attr("onclick", "Admin_PatientEligibilityService.PatientEligibilityServiceEdit('" + item.PatientEligibilityServiceID + "',event);");
                //}
                if (Admin_PatientEligibilityService.params["PanelID"] == "pnlReportsSSRSDashboard") {
                    $('#btn-add').hide();
                    //$row.append('<td style="display:none;">' + item.PatientEligibilityServiceID + '</td><td>' + selectPatientEligibilityService + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                    $row.append('<td style="display:none;">' + item.PatientEligibilityServiceID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_PatientEligibilityService.PatientEligibilityServiceDelete(' + item.PatientEligibilityServiceID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_PatientEligibilityService.PatientEligibilityServicet(' + item.PatientEligibilityServiceID + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_PatientEligibilityService.PatientEligibilityServiceActiveInactive(' + item.PatientEligibilityServiceID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + item.PatientEligibilityServiceID + '</td><td>' + item.EntityName + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.ScheduleDays + '</td><td>' + item.Mode + '</td><td>' + item.Time + '</td><td>' + item.IntervalHours + '</td><td>' + item.IntervalMinutes + '</td><td>' + item.IsActive + '</td>');

                } else {
                    $('#btn-add').show();
                    //$row.append('<td style="display:none;">' + item.PatientEligibilityServiceID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_PatientEligibilityService.PatientEligibilityServiceDelete(' + item.PatientEligibilityServiceID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_PatientEligibilityService.PatientEligibilityServicet(' + item.PatientEligibilityServiceID + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_PatientEligibilityService.PatientEligibilityServiceActiveInactive(' + item.PatientEligibilityServiceID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectPatientEligibilityService + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PracticeName + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                    $row.append('<td style="display:none;">' + item.PatientEligibilityServiceID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_PatientEligibilityService.PatientEligibilityServiceDelete(' + item.PatientEligibilityServiceID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_PatientEligibilityService.PatientEligibilityServiceEdit(' + item.PatientEligibilityServiceID + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_PatientEligibilityService.PatientEligibilityServiceActiveInactive(' + item.PatientEligibilityServiceID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.EntityName + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.ScheduleDays + '</td><td>' + item.Mode + '</td><td>' + item.Time + '</td><td>' + item.IntervalHours + '</td><td>' + item.IntervalMinutes + '</td><td>' + item.IsActive + '</td>');

                }
                $("#" + Admin_PatientEligibilityService.params["PanelID"] + " #pnlPatientEligibilityService_Result #dgvPatientEligibilityService tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_PatientEligibilityService.params["PanelID"] + ' #dgvPatientEligibilityService').DataTable({
                "language": {
                    "emptyTable": "No PatientEligibilityService Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_PatientEligibilityService.params["PanelID"] + ' #dgvPatientEligibilityService'))
            ;
        else
            $("#" + Admin_PatientEligibilityService.params["PanelID"] + " #pnlPatientEligibilityService_Result #dgvPatientEligibilityService").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$("#" + Admin_PatientEligibilityService.params["PanelID"] + " #pnlPatientEligibilityService_Result #dgvPatientEligibilityService").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    FillPatientEligibilityServiceName: function (PatientEligibilityServiceID, PatientEligibilityServiceName, PracticeId, PracticeName, EntityName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = " #txtPatientEligibilityService";
        var RefHiddenIdCtrl = " #hfPatientEligibilityService";
        if (Admin_PatientEligibilityService.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_PatientEligibilityService.params["RefCtrl"];
        }
        if (Admin_PatientEligibilityService.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_PatientEligibilityService.params["RefHiddenIdCtrl"];
        }

        if (Admin_Provider.params["PanelID"] == "blockHoursDetail") {
            if (globalAppdata.AppUserName == "MDVISION")
                $('#' + Admin_PatientEligibilityService.params["PanelID"] + RefCtrl).val(PatientEligibilityServiceName + ' - ' + EntityName).focus();
            else
                $('#' + Admin_PatientEligibilityService.params["PanelID"] + RefCtrl).val(PatientEligibilityServiceName).focus();
        }
        else {
            $('#' + Admin_PatientEligibilityService.params["PanelID"] + RefCtrl).val(PatientEligibilityServiceName).focus();
        }
        //$('#' + Admin_PatientEligibilityService.params["PanelID"] + RefCtrl).val(PatientEligibilityServiceName).focus();
        $('#' + Admin_PatientEligibilityService.params["PanelID"] + RefHiddenIdCtrl).val(PatientEligibilityServiceID);


        if (Admin_PatientEligibilityService.params["PanelID"] == "pnlReportsSSRSDashboard") {
            Admin_PatientEligibilityService.params["PatientEligibilityServiceID"] = PatientEligibilityServiceID;
            Admin_PatientEligibilityService.params["PracticeId"] = PracticeId;
            $('#' + Admin_PatientEligibilityService.params["PanelID"] + ' #lblPatientEligibilityService').css("display", "inline");
        } else {
            if (Admin_PatientEligibilityService.params["IsOptional"] != null && Admin_PatientEligibilityService.params["RefForm"] != null && Admin_PatientEligibilityService.params["IsOptional"] == false) {
                if ($('#' + Admin_PatientEligibilityService.params["PanelID"] + ' #' + Admin_PatientEligibilityService.params["RefForm"]).data('bootstrapValidator') != null && typeof $('#' + Admin_PatientEligibilityService.params["PanelID"] + ' #' + Admin_PatientEligibilityService.params["RefForm"]).data('bootstrapValidator') != 'undefined') {
                    $('#' + Admin_PatientEligibilityService.params["PanelID"] + ' #' + Admin_PatientEligibilityService.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_PatientEligibilityService.params.PanelID + RefCtrl).attr("name"));
                }
            }
            $('#' + Admin_PatientEligibilityService.params.PanelID + ' #txtPractice').val(PracticeName);
            $('#' + Admin_PatientEligibilityService.params.PanelID + ' #hfPractice').val(PracticeId);
            $('#' + Admin_PatientEligibilityService.params["PanelID"] + ' #lblPatientEligibilityService').css("display", "none");
            $('#' + Admin_PatientEligibilityService.params["PanelID"] + ' #lnkPatientEligibilityServiceEdit').css("display", "inline");
        }

        if (Admin_PatientEligibilityService.params != null && Admin_PatientEligibilityService.params.ParentCtrl != null && Admin_PatientEligibilityService.params.PanelID != 'pnlAdminPatientEligibilityService') {
            UnloadActionPan(Admin_PatientEligibilityService.params.ParentCtrl, 'Admin_PatientEligibilityService', null, Admin_PatientEligibilityService.params.PanelID);
        }
        else
            UnloadActionPan(Admin_PatientEligibilityService.params["ParentCtrl"], "Admin_PatientEligibilityService");

        $('#' + Admin_PatientEligibilityService.params["PanelID"] + RefCtrl).focus();
    },

    SearchPatientEligibilityService: function (PatientEligibilityServiceData, PatientEligibilityServiceID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "PatientEligibilityServiceData=" + PatientEligibilityServiceData + "&PatientEligibilityServiceID=" + PatientEligibilityServiceID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PATIENT_ELIGIBILITY_SERVICE", "SEARCH_PATIENT_ELIGIBILITY_SERVICE");
    },

    DeletePatientEligibilityService: function (PatientEligibilityServiceID) {
        var data = "PatientEligibilityServiceID=" + PatientEligibilityServiceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PATIENT_ELIGIBILITY_SERVICE", "DELETE_PATIENT_ELIGIBILITY_SERVICE");
    },

    UnLoadTab: function (Tab) {
        if (Admin_PatientEligibilityService.params["FromAdmin"] == "0") {


            if (Admin_PatientEligibilityService.params != null && Admin_PatientEligibilityService.params.ParentCtrl != null && Admin_PatientEligibilityService.params.PanelID != 'pnlAdminPatientEligibilityService') {
                UnloadActionPan(Admin_PatientEligibilityService.params.ParentCtrl, 'Admin_PatientEligibilityService', null, Admin_PatientEligibilityService.params.PanelID);
            }

            else if (Admin_PatientEligibilityService.params != null && Admin_PatientEligibilityService.params.ParentCtrl != null) {
                UnloadActionPan(Admin_PatientEligibilityService.params.ParentCtrl, 'Admin_PatientEligibilityService');
            }

            else
                UnloadActionPan(null, 'Admin_PatientEligibilityService');
        }
        else {
            RemoveAdminTab();
        }
    },
}