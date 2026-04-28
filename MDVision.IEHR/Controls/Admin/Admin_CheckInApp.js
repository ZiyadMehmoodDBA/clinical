
Admin_CheckInApp = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_CheckInApp.params = params;

        if (Admin_CheckInApp.params["FromAdmin"] == "0" && Admin_CheckInApp.params["PanelID"] == 'pnlAdminCheckInApp')
            Admin_CheckInApp.params["FromAdmin"] = "1";

        if (Admin_CheckInApp.bIsFirstLoad) {
            Admin_CheckInApp.bIsFirstLoad = false;

            var self = "";
            if (Admin_CheckInApp.params["PanelID"] != "pnlAdminCheckInApp")
                self = $('#' + Admin_CheckInApp.params["PanelID"] + ' #pnlAdminCheckInApp')
            else
                self = $('#pnlAdminCheckInApp');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                $("#" + Admin_CheckInApp.params["PanelID"] + " #pnlProvider_Search #ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {
                //if (globalAppdata['IsAdmin'] != "True") {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    $("#" + Admin_CheckInApp.params["PanelID"] + " #pnlProvider_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }

                //set Specialty for PMS-197
                if (Admin_CheckInApp.params["Specialty"])
                    self.find("#ddlSpeciality option:contains('" + Admin_CheckInApp.params["Specialty"] + "')").prop('selected', true);

                Admin_CheckInApp.UserSearch();
                Admin_CheckInApp.BindFacility();
            });

            // Set Title Explicitly if it's passed as Parameter
            if (Admin_CheckInApp.params.Title != null)
                self.find("#headingTitle").text(Admin_CheckInApp.params.Title);


        }
    },
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmCheckInApp";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Admin_CheckInApp";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#pnlDemographic #hfFacility').val(), 'patTabDemographic');
        var params = [];
        params["FacilityId"] = $('#pnlAdminCheckInApp #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Admin_CheckInApp';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },
    BindFacility: function () {
        var Ctrl = $("#" + Admin_CheckInApp.params.PanelID + " #frmCheckInApp #txtFacility");
        var func = function () { return utility.GetFacilityDescriptionArray(Ctrl.val()) };
        var hfCtrl = $("#" + Admin_CheckInApp.params.PanelID + " #frmCheckInApp #hfFacility");
        var onSelect = function (e) {
            //$("#" + Admin_CheckInApp.params.PanelID + " #frmCheckInApp #txtPractice").val(e.Practice);
            //$("#" + Admin_CheckInApp.params.PanelID + " #frmCheckInApp #hfPractice").val(e.PracticeId);
        }
        var onChange = function () { /*Admin_CheckInApp.RemovePractice(Ctrl);*/ };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },
    HideFacilityLink: function () {
        $('#pnlAdminCheckInApp #txtFacility').attr("FacilityId", "-1");
        $('#pnlAdminCheckInApp #hfFacility').val("-1");
        $('#pnlAdminCheckInApp #lnkFacilityEdit').css("display", "none");
        $('#pnlAdminCheckInApp #lblFacility').css("display", "inline");
    },
    ProviderAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Provider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ProviderId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_CheckInApp.params["FromAdmin"];
                if (Admin_CheckInApp.params.ParentCtrl == "EncounterChargeCapture") {
                    params["IsFromEncounter"] = 1;
                }
                else {
                    params["IsFromEncounter"] = 0;
                }
                // params["ParentCtrl"] = Admin_CheckInApp.params["ParentCtrl"];
                if (Admin_CheckInApp.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_Provider';
                }

                LoadActionPan('providerDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ProviderEdit: function (ProviderId, event,entityID) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#" + Admin_CheckInApp.params["PanelID"] + ' #gvProvider_row' + ProviderId));
        AppPrivileges.GetFormPrivileges("Provider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ProviderId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ProviderId"] = selectedValue;
                    params["EntityId"] = entityID;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Admin_CheckInApp.params["FromAdmin"];
                    if (Admin_CheckInApp.params.ParentCtrl == "EncounterChargeCapture") {
                        params["IsFromEncounter"] = 1;
                    }
                    else {
                        params["IsFromEncounter"] = 0;
                    }
                    // params["ParentCtrl"] = Admin_CheckInApp.params["ParentCtrl"];
                    if (Admin_CheckInApp.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Admin_Provider';
                    }
                    LoadActionPan('providerDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ProviderDelete: function (ProviderId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        utility.SelectGridRow($("#" + Admin_CheckInApp.params["PanelID"] + ' #gvProvider_row' + ProviderId));
        AppPrivileges.GetFormPrivileges("Provider", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ProviderId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_CheckInApp.DeleteProvider(selectedValue).done(function (response) {
                            if (response.status != false) {
                                // var table1 = $('#' + Admin_CheckInApp.params["PanelID"] + ' #dgvProvider').DataTable();
                                //table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                CacheManager.BindCodes('GetProvider', true);
                                CacheManager.BindCodes('GetAllProviders', true);
                                Admin_CheckInApp.UserSearch();
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

    ProviderActiveInactive: function (ProviderId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ProviderId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        providerDetail.UpdateProviderActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_CheckInApp.UserSearch('0');

                                CacheManager.BindCodes('GetProvider', true);
                                CacheManager.BindCodes('GetAllProviders', true);
                                //UnloadActionPan();
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

    UserSearch: function (ProviderId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Check-In App", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_CheckInApp.params["PanelID"] + "  #pnlProvider_Result").css("display") == "none") {
                    $("#" + Admin_CheckInApp.params["PanelID"] + " #pnlProvider_Result").show();
                }

                var self = $("#" + Admin_CheckInApp.params["PanelID"] + " #pnlProvider_Search");
                var myJSON = self.getMyJSON();

                
                Admin_CheckInApp.SearchUser(myJSON, ProviderId, PageNo, rpp).done(function (response) {
                    var response = JSON.parse(response);
                    if (response.status != false) {
                        Admin_CheckInApp.ProviderGridLoad(response);
                        var TableControl = Admin_CheckInApp.params["PanelID"] + " #dgvUser";
                        var PagingPanelControlID = Admin_CheckInApp.params["PanelID"] + " #divUserPaging";
                        var ClassControlName = "Admin_CheckInApp";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ActivityLogChangesCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_CheckInApp.UserSearch(PrimaryID, PageNumber, ResultPerPage);
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

    ProviderGridLoad: function (response, PageNo, rpp) {
       
         
        
        $("#" + Admin_CheckInApp.params["PanelID"] + " #dgvUser").dataTable().fnDestroy();
        $("#" + Admin_CheckInApp.params["PanelID"] + " #dgvUser tbody").find("tr").remove();
        
            if (response.ActivityLogChangesCount > 0) {
            var ProviderLoadJSONData = JSON.parse(response.ActivityLogChangesLoad_JSON);
            $.each(ProviderLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
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
                $row.append('<td style="display:none;">' + item.UserId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_ClearingHouse.ClearingHouseActiveInactive(' + item.UserId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.UserName + '</td><td>' + item.FacilityName + '</td><td>' + item.DeviceId + '</td><td>' + item.EntityName + '</td>');
                $("#" + Admin_CheckInApp.params["PanelID"] + "  #dgvUser tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_CheckInApp.params["PanelID"] + ' #dgvUser').DataTable({
                "language": {
                    "emptyTable": "No Provider Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_CheckInApp.params["PanelID"] + ' #dgvUser'))
            ;
        else
            $("#" + Admin_CheckInApp.params["PanelID"] + "  #dgvUser").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "bSort": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$("#" + Admin_CheckInApp.params["PanelID"] + " #pnlProvider_Result #dgvProvider").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    FillProviderName: function (ProviderId, ProviderName, EntityName, shortname,CLIA ,NPI,event) {
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = " #txtProvider";
        var RefCtrlHidden = " #hfProvider";
        var RefCtrlLabel = " #lblProvider";
        var RefCtrlLink = " #lnkProviderEdit";
        if (Admin_CheckInApp.params["RefCtrl"] != null)
            RefCtrl = " #" + Admin_CheckInApp.params["RefCtrl"];
        if (Admin_CheckInApp.params["RefCtrlHidden"] != null)
            RefCtrlHidden = " #" + Admin_CheckInApp.params["RefCtrlHidden"];
        if (Admin_CheckInApp.params["RefCtrlLabel"] != null)
            RefCtrlLabel = " #" + Admin_CheckInApp.params["RefCtrlLabel"];
        if (Admin_CheckInApp.params["RefCtrlLink"] != null)
            RefCtrlLink = " #" + Admin_CheckInApp.params["RefCtrlLink"];

        if (Admin_CheckInApp.params.ParentRefCtrl) {
            RefCtrl = " #" + Admin_CheckInApp.params.ParentRefCtrl + RefCtrl;
            RefCtrlHidden = " #" + Admin_CheckInApp.params.ParentRefCtrl + RefCtrlHidden;
            RefCtrlLabel = " #" + Admin_CheckInApp.params.ParentRefCtrl + RefCtrlLabel;
            RefCtrlLink = " #" + Admin_CheckInApp.params.ParentRefCtrl + RefCtrlLink;
        }

        if (Admin_CheckInApp.params["PanelID"] == "blockHoursDetail") {
            if (globalAppdata.AppUserName == "MDVISION")
                $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).val(ProviderName + ' - ' + EntityName);//.focus();
            else
                $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).val(ProviderName);//.focus();
        }
        else if (Admin_CheckInApp.params["PanelID"] == "resourcesDetail") {
            if (globalAppdata.AppUserName == "MDVISION") {
                $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).val(ProviderName + ' - ' + EntityName);
                ProviderName = ProviderName + ' - ' + EntityName;
            }
            else
                $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).val(ProviderName);
        }
        else {
            $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).val(ProviderName);//.focus();
            $("#pnlDashboard #NoteProviderText").val( ProviderName);
        }

        //$('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).val(ProviderName).focus();
        $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrlHidden).val(ProviderId);
        $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).attr("providerid", ProviderId);
        if (Admin_CheckInApp.params["ProviderNPI"])
        $('#' + Admin_CheckInApp.params["PanelID"] + " #" + Admin_CheckInApp.params["ProviderNPI"]).val(NPI);

        if ($('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl), ProviderName, $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrlHidden), ProviderId);
        else
            utility.SetAutoCompleteSource($('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl), $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrlHidden));

        $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrlLabel).css("display", "none");
        $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrlLink).css("display", "inline");
        if (Admin_CheckInApp.params["PanelID"] == "pnlClinicalNotes") {
            Clinical_Notes.UnlinkAppointment(this, 'pnlClinicalNotes', true);
        }
        if (Admin_CheckInApp.params["PanelID"] == "pnlReportsSSRSDashboard") {
            Admin_CheckInApp.params["ProviderId"] = ProviderId;
            $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrlLabel).css("display", "inline");
        } else
            if (Admin_CheckInApp.params["IsOptional"] != null && Admin_CheckInApp.params["RefForm"] != null && Admin_CheckInApp.params["IsOptional"] == false && Admin_CheckInApp.params.ParentCtrl != "ERADetail" && Admin_CheckInApp.params.ParentCtrl != "Patient_Referrals_Outgoing_Detail" && Admin_CheckInApp.params.ParentCtrl != "OrderSet_Patient_Referrals_Outgoing_Detail") {
                $('#' + Admin_CheckInApp.params["PanelID"] + ' #' + Admin_CheckInApp.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).attr("name"));
            }
        // Start 02/02/2016 Adnan Maqbool Khan for bug # 3247
        //if ($("#" + Admin_CheckInApp.params["PanelID"] + " #" + Admin_CheckInApp.params["RefForm"]).data('bootstrapValidator') != null && typeof $("#" + Admin_CheckInApp.params["PanelID"] + " #" + Admin_CheckInApp.params["RefForm"]).data('bootstrapValidator') != 'undefined') {
        //    $('#' + Admin_CheckInApp.params["PanelID"] + ' #' + Admin_CheckInApp.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).attr("name"));
        //}
        //end
        UnloadActionPan(Admin_CheckInApp.params["ParentCtrl"], "Admin_Provider");

        if (Admin_CheckInApp.params.ParentCtrl == "Admin_CCMCareTeamDetail") {
            Admin_CCMCareTeamDetail.Grid("Provider", ProviderId, ProviderName);
        }
        if (Admin_CheckInApp.params.ParentCtrl == "ClinicalLabOrderDetail") {
            ClinicalLabOrderDetail.EnableDisableTestSearch(null,true);
        }
        //if (Admin_CheckInApp.params.ParentCtrl == "clinicalTabNotes") {
        //    $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).trigger('blur');
        //}
        if (Admin_CheckInApp.params.ParentCtrl == "appointmentDetail") {
                appointmentDetail.OpenPatReferralSearch($('#appointmentDetail #ddlInsurancePlan').val());          

        }

        if (Admin_CheckInApp.params["ParentCtrl"] == "Patient_Referral") {

            Patient_Referral.BindProvider(Admin_CheckInApp.params["RefCtrl"], Admin_CheckInApp.params["RefCtrlHidden"], false, shortname);
        }
        else if (Admin_CheckInApp.params["ParentCtrl"] == "EncounterChargeCapture") {
            EncounterChargeCapture.BindCLIA(CLIA);
        }
       else if (Admin_CheckInApp.params.ParentCtrl == "ClinicalProcedureOrderDetail") {
            ClinicalProcedureOrderDetail.favoriteListSearch();
       }
       else if (Admin_CheckInApp.params.ParentCtrl == "ClinicalRadiologyResultDetail") {
           ClinicalRadiologyResultDetail.setFavSearch();
       }
        $('#' + Admin_CheckInApp.params["PanelID"] + RefCtrl).focus();

    },

    SearchUser: function (ProviderData, ProviderId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["Username"] = $('#' + Admin_CheckInApp.params["PanelID"] + ' #txtUsername ').val();
        objData["Facility"] = $('#' + Admin_CheckInApp.params["PanelID"] + ' #txtFacility ').val();
        objData["EntityId"] = $('#' + Admin_CheckInApp.params["PanelID"] + ' #ddlEntity :selected ').val();
        objData["DeviceId"] = $('#' + Admin_CheckInApp.params["PanelID"] + ' #txtDeviceId ').val();
        objData["Status"] = $('#' + Admin_CheckInApp.params["PanelID"] + ' #chkActive :selected').val();
        objData["commandType"] = "checkinappuserload";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");
    },

    DeleteProvider: function (ProviderId) {
        var data = "ProviderID=" + ProviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "DELETE_PROVIDER");
    },

    LoadProviderDBCall: function (ShortName) {
        var data = "ShortName=" + ShortName;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER", "LOAD_PROVIDER_LOOKUP");
    },

    UnLoadTab: function () {
        if (Admin_CheckInApp.params["FromAdmin"] == "0") {

            if (Admin_CheckInApp.params != null && Admin_CheckInApp.params.ParentCtrl != null && Admin_CheckInApp.params.PanelID != 'pnlAdminCheckInApp') {
                UnloadActionPan(Admin_CheckInApp.params.ParentCtrl, 'Admin_Provider', null, Admin_CheckInApp.params.PanelID);
            }
            else if (Admin_CheckInApp.params != null && Admin_CheckInApp.params.ParentCtrl != null) {
                UnloadActionPan(Admin_CheckInApp.params.ParentCtrl, 'Admin_Provider');
            }
            else
                UnloadActionPan(null, 'Admin_Provider');

        }
        else {
            RemoveAdminTab();
        }
    },
}
