
Admin_Facility = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Admin_Facility.params = params;

        if (Admin_Facility.params["FromAdmin"] == "0" && Admin_Facility.params["PanelID"] == 'pnlAdminFacility')
            Admin_Facility.params["FromAdmin"] = "1";

        //if ( Admin_Facility.params["PanelID"] != 'pnlAdminFacility')
        //    Admin_Facility.params["PanelID"] = Admin_Facility.params["PanelID"] + ' #pnlAdminFacility';


        if (Admin_Facility.bIsFirstLoad) {
            Admin_Facility.bIsFirstLoad = false;

            var self = "";//$('#pnlAdminFacility');
            if (Admin_Facility.params["PanelID"] != 'pnlAdminFacility')
                self = $('#' + Admin_Facility.params["PanelID"] + ' #pnlAdminFacility');
            else
                self = $('#pnlAdminFacility');

            self.loadDropDowns(true).done(function () {
                Admin_Facility.FacilitySearch();
            });
        }
    },

    FacilityAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Facility", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FacilityId"] = null;
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_Facility.params["FromAdmin"];
                if (Admin_Facility.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_Facility';
                }
                LoadActionPan('facilityDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FacilityEdit: function (FacilityId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvFacility_row' + FacilityId));
        AppPrivileges.GetFormPrivileges("Facility", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = FacilityId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["FacilityId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Admin_Facility.params["FromAdmin"];
                    if (Admin_Facility.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Admin_Facility';
                    }
                    LoadActionPan('facilityDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FacilityDelete: function (FacilityId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvFacility_row' + FacilityId));
        utility.SelectGridRow($("#" + Admin_Facility.params["PanelID"] + ' #gvFacility_row' + FacilityId));
        AppPrivileges.GetFormPrivileges("Facility", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = FacilityId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_Facility.DeleteFacility(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#' + Admin_Facility.params["PanelID"] + ' #dgvFacility').DataTable();
                                //table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                CacheManager.BindCodes('GetFacility', true);
                                Admin_Facility.FacilitySearch();
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

    FacilityActiveInactive: function (FacilityId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Facility", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = FacilityId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        facilityDetail.UpdateFacilityActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_Facility.FacilitySearch('0');
                                CacheManager.BindCodes('GetFacility', true);
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

    FacilitySearch: function (FacilityId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Facility", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_Facility.params["PanelID"] + " #pnlFacility_Result").css("display") == "none") {
                    $("#" + Admin_Facility.params["PanelID"] + " #pnlFacility_Result").show();
                }

                var self = $("#" + Admin_Facility.params["PanelID"] + " #pnlFacility_Search");
                var myJSON = self.getMyJSON();
                var Parms = JSON.parse(myJSON);
                Parms["RefForm"] = Admin_Facility.params.RefForm;
                if (Admin_Facility.params.RefForm == "frmClinicalRadiologyOrderDetail" && Admin_Facility.params.ProviderId && parseInt(Admin_Facility.params.ProviderId) > 0)
                    Parms["ProviderId"] = Admin_Facility.params.ProviderId;
                if (Admin_Facility.params.LoadAllFacility != null) {
                    Parms["LoadAllFacility"] = Admin_Facility.params.LoadAllFacility;
                }
                myJSON = JSON.stringify(Parms);

                Admin_Facility.SearchFacility(myJSON, FacilityId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_Facility.FacilityGridLoad(response);
                        var TableControl = Admin_Facility.params["PanelID"] + " #dgvFacility";
                        var PagingPanelControlID = Admin_Facility.params["PanelID"] + " #divFacilityPaging";
                        var ClassControlName = "Admin_Facility";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.FacilityCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_Facility.FacilitySearch(PrimaryID, PageNumber, ResultPerPage);
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

    FacilityGridLoad: function (response) {
        $("#" + Admin_Facility.params["PanelID"] + " #dgvFacility").dataTable().fnDestroy();
        $("#" + Admin_Facility.params["PanelID"] + " #pnlFacility_Result #dgvFacility tbody").find("tr").remove();
        if (response.FacilityCount > 0) {
            var FacilityLoadJSONData = JSON.parse(response.FacilityLoad_JSON);
            $.each(FacilityLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvFacility_row" + item.FacilityId);
                $row.attr("FacilityId", item.FacilityId);
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

                var selectFacility = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                if (Admin_Facility.params["FromAdmin"] == "0") {
                    if ((Admin_Facility.params.ParentCtrl == "Patient_Referrals_Incoming_Detail" || Admin_Facility.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail" || Admin_Facility.params.ParentCtrl == "OrderSet_Patient_Referrals_Outgoing_Detail") || Admin_Facility.params.ParentCtrl == "patTabDemographic" || Admin_Facility.params.ParentCtrl == "appointmentDetail" || Admin_Facility.params.ParentCtrl == "Scheduling_RescheduleSearch") {
                        var selectMethod = "Admin_Facility.FillFacilityName('" + item.FacilityId + "','" + item.Description + "','" + item.PracticeId + "','" + item.PracticeName + "','" + item.EntityName + "','" + item.LocationName + "',event);"
                    } else {
                        var selectMethod = "Admin_Facility.FillFacilityName('" + item.FacilityId + "','" + item.ShortName + "','" + item.PracticeId + "','" + item.PracticeName + "','" + item.EntityName + "','" + item.LocationName + "',event);"
                    }
                    selectFacility = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';

                    if (ClassDisabled != "disabled")
                        $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_Facility.FacilityEdit('" + item.FacilityId + "',event);");
                }
                if (Admin_Facility.params["PanelID"] == "pnlReportsSSRSDashboard") {
                    $('#btn-add').hide();
                    $row.append('<td style="display:none;">' + item.FacilityId + '</td><td>' + selectFacility + '</td><td>' + item.ShortName + '</td><td>' + item.LocationName + '</td><td>' + item.Description + '</td><td>' + item.PracticeName + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                } else {
                    $('#btn-add').show();
                    $row.append('<td style="display:none;">' + item.FacilityId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_Facility.FacilityDelete(' + item.FacilityId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Facility.FacilityEdit(' + item.FacilityId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_Facility.FacilityActiveInactive(' + item.FacilityId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectFacility + '</td><td>' + item.ShortName + '</td><td>' + item.LocationName + '</td><td>' + item.Description + '</td><td>' + item.PracticeName + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                }
                $("#" + Admin_Facility.params["PanelID"] + " #pnlFacility_Result #dgvFacility tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_Facility.params["PanelID"] + ' #dgvFacility').DataTable({
                "language": {
                    "emptyTable": "No Facility Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_Facility.params["PanelID"] + ' #dgvFacility'))
            ;
        else
            $("#" + Admin_Facility.params["PanelID"] + " #pnlFacility_Result #dgvFacility").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$("#" + Admin_Facility.params["PanelID"] + " #pnlFacility_Result #dgvFacility").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    FillFacilityName: function (FacilityId, FacilityName, PracticeId, PracticeName, EntityName, LocationName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = " #txtFacility";
        var RefHiddenIdCtrl = " #hfFacility";
        if (Admin_Facility.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_Facility.params["RefCtrl"];
        }
        if (Admin_Facility.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_Facility.params["RefHiddenIdCtrl"];
        }
        FacilityName = FacilityName.replace(/&quot;/g, '"').replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&#39;/g, "'").replace(/&amp;/g, "&").replace(/\/\equal/g, '=');
        if (Admin_Facility.params["PanelID"] == "blockHoursDetail") {
            if (globalAppdata.AppUserName == "MDVISION")
                $('#' + Admin_Facility.params["PanelID"] + RefCtrl).val(FacilityName + ' - ' + EntityName);//.focus();
            else
                $('#' + Admin_Facility.params["PanelID"] + RefCtrl).val(FacilityName);//.focus();
        }
        else {
            $('#' + Admin_Facility.params["PanelID"] + RefCtrl).val(FacilityName);//.focus();
        }
        //$('#' + Admin_Facility.params["PanelID"] + RefCtrl).val(FacilityName).focus();
        $('#' + Admin_Facility.params["PanelID"] + RefHiddenIdCtrl).val(FacilityId);

        if ($('#' + Admin_Facility.params["PanelID"] + RefCtrl).data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#' + Admin_Facility.params["PanelID"] + RefCtrl), FacilityName, $('#' + Admin_Facility.params["PanelID"] + RefHiddenIdCtrl), FacilityId);
        else
            utility.SetAutoCompleteSource($('#' + Admin_Facility.params["PanelID"] + RefCtrl), $('#' + Admin_Facility.params["PanelID"] + RefHiddenIdCtrl));

        if (Admin_Facility.params["PanelID"] == "pnlReportsSSRSDashboard") {
            Admin_Facility.params["FacilityId"] = FacilityId;
            Admin_Facility.params["PracticeId"] = PracticeId;
            $('#' + Admin_Facility.params["PanelID"] + ' #lblFacility').css("display", "inline");
        } else {
            if (Admin_Facility.params["IsOptional"] != null && Admin_Facility.params["RefForm"] != null && Admin_Facility.params["IsOptional"] == false) {
                if ($('#' + Admin_Facility.params["PanelID"] + ' #' + Admin_Facility.params["RefForm"]).data('bootstrapValidator') != null && typeof $('#' + Admin_Facility.params["PanelID"] + ' #' + Admin_Facility.params["RefForm"]).data('bootstrapValidator') != 'undefined') {
                    if (Admin_Facility.params["PanelID"] != "blockHoursDetail")
                        $('#' + Admin_Facility.params["PanelID"] + ' #' + Admin_Facility.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_Facility.params.PanelID + RefCtrl).attr("name"));
                }
            }
            $('#' + Admin_Facility.params.PanelID + ' #txtPractice').val(PracticeName);
            $('#' + Admin_Facility.params.PanelID + ' #hfPractice').val(PracticeId);
            $('#' + Admin_Facility.params.PanelID + ' #txtLocation').val(LocationName);
            if (Admin_Facility.params["ParentCtrl"] != "clinicalTabProgressNote") {
                $('#' + Admin_Facility.params["PanelID"] + ' #lblFacility').css("display", "none");
                $('#' + Admin_Facility.params["PanelID"] + ' #lnkFacilityEdit').css("display", "inline");
            }
        }
        if (Admin_Facility.params.PanelID == 'pnlDemographic' || Admin_Facility.params.PanelID == 'pnldemographicDetail' || Admin_Facility.params.PanelID == 'pnlDemographicQuick') {

            demographicDetail.ScanOCRPriviliges(PracticeId);
        }
        if (Admin_Facility.params != null && Admin_Facility.params.ParentCtrl != null && Admin_Facility.params.PanelID != 'pnlAdminFacility') {
            if (Admin_Facility.params.ParentCtrl == "ERADetail") {
                UnloadActionPan(Admin_Facility.params.ParentCtrl, 'Admin_Facility')
            }
            else {
                UnloadActionPan(Admin_Facility.params.ParentCtrl, 'Admin_Facility', null, Admin_Facility.params.PanelID);
            }
        }
        else
            UnloadActionPan(Admin_Facility.params["ParentCtrl"], "Admin_Facility");

        if (Admin_Facility.params["ParentCtrl"] == "Scheduling_Force_Booking") {
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').bootstrapValidator('revalidateField', 'Facility');
        }
        if (Admin_Facility.params.PanelID == "appointmentDetail") {          
                $('#frmappointmentDetail').bootstrapValidator('revalidateField', 'Facility');
                appointmentDetail.OpenPatReferralSearch();          
        }
        //End 13-10-2017 Humaira Yousaf IMP-1195
        $('#' + Admin_Facility.params["PanelID"] + RefCtrl).focus();
    },

    SearchFacility: function (FacilityData, FacilityId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "FacilityData=" + FacilityData + "&FacilityID=" + FacilityId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FACILITY", "SEARCH_FACILITY");
    },

    DeleteFacility: function (facilityID) {
        var data = "FacilityID=" + facilityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FACILITY_DETAIL", "DELETE_FACILITY");
    },

    LoadFacilityDBCall: function (ShortName) {
        var data = "ShortName=" + ShortName;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FACILITY", "LOAD_FACILITY_LOOKUP");
    },

    LoadFacilityDescriptionDBCall: function (ShortName) {
        var data = "ShortName=" + ShortName;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FACILITY", "LOAD_FACILITY_DESCRIPTION_LOOKUP");
    },

    UnLoadTab: function (Tab) {
        if (Admin_Facility.params["FromAdmin"] == "0") {


            if (Admin_Facility.params != null && Admin_Facility.params.ParentCtrl != null && Admin_Facility.params.PanelID != 'pnlAdminFacility') {
                UnloadActionPan(Admin_Facility.params.ParentCtrl, 'Admin_Facility', null, Admin_Facility.params.PanelID);
            }

            else if (Admin_Facility.params != null && Admin_Facility.params.ParentCtrl != null) {
                UnloadActionPan(Admin_Facility.params.ParentCtrl, 'Admin_Facility');
            }

            else
                UnloadActionPan(null, 'Admin_Facility');
        }
        else {
            RemoveAdminTab();
        }
    },
}
