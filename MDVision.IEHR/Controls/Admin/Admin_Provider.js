
Admin_Provider = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_Provider.params = params;

        if (Admin_Provider.params["FromAdmin"] == "0" && Admin_Provider.params["PanelID"] == 'pnlAdminProvider')
            Admin_Provider.params["FromAdmin"] = "1";

        if (Admin_Provider.bIsFirstLoad) {
            Admin_Provider.bIsFirstLoad = false;

            var self = "";
            if (Admin_Provider.params["PanelID"] != "pnlAdminProvider")
                self = $('#' + Admin_Provider.params["PanelID"] + ' #pnlAdminProvider')
            else
                self = $('#pnlAdminProvider');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                $("#" + Admin_Provider.params["PanelID"] + " #pnlProvider_Search #ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {
                //if (globalAppdata['IsAdmin'] != "True") {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    $("#" + Admin_Provider.params["PanelID"] + " #pnlProvider_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }

                //set Specialty for PMS-197
                if (Admin_Provider.params["Specialty"])
                    self.find("#ddlSpeciality option:contains('" + Admin_Provider.params["Specialty"] + "')").prop('selected', true);

                Admin_Provider.ProviderSearch();
            });

            // Set Title Explicitly if it's passed as Parameter
            if (Admin_Provider.params.Title != null)
                self.find("#headingTitle").text(Admin_Provider.params.Title);


        }
    },

    ProviderAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Provider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ProviderId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_Provider.params["FromAdmin"];
                if (Admin_Provider.params.ParentCtrl == "EncounterChargeCapture") {
                    params["IsFromEncounter"] = 1;
                }
                else {
                    params["IsFromEncounter"] = 0;
                }
                // params["ParentCtrl"] = Admin_Provider.params["ParentCtrl"];
                if (Admin_Provider.params["FromAdmin"] == "0") {
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
        utility.SelectGridRow($("#" + Admin_Provider.params["PanelID"] + ' #gvProvider_row' + ProviderId));
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
                    params["FromAdmin"] = Admin_Provider.params["FromAdmin"];
                    if (Admin_Provider.params.ParentCtrl == "EncounterChargeCapture") {
                        params["IsFromEncounter"] = 1;
                    }
                    else {
                        params["IsFromEncounter"] = 0;
                    }
                    // params["ParentCtrl"] = Admin_Provider.params["ParentCtrl"];
                    if (Admin_Provider.params["FromAdmin"] == "0") {
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

        utility.SelectGridRow($("#" + Admin_Provider.params["PanelID"] + ' #gvProvider_row' + ProviderId));
        AppPrivileges.GetFormPrivileges("Provider", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ProviderId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_Provider.DeleteProvider(selectedValue).done(function (response) {
                            if (response.status != false) {
                                // var table1 = $('#' + Admin_Provider.params["PanelID"] + ' #dgvProvider').DataTable();
                                //table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                CacheManager.BindCodes('GetProvider', true);
                                CacheManager.BindCodes('GetAllProviders', true);
                                Admin_Provider.ProviderSearch();
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
                                Admin_Provider.ProviderSearch('0');

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

    ProviderSearch: function (ProviderId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Provider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_Provider.params["PanelID"] + "  #pnlProvider_Result").css("display") == "none") {
                    $("#" + Admin_Provider.params["PanelID"] + " #pnlProvider_Result").show();
                }

                var self = $("#" + Admin_Provider.params["PanelID"] + " #pnlProvider_Search");
                var myJSON = self.getMyJSON();

                Admin_Provider.SearchProvider(myJSON, ProviderId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_Provider.ProviderGridLoad(response);
                        var TableControl = Admin_Provider.params["PanelID"] + " #dgvProvider";
                        var PagingPanelControlID = Admin_Provider.params["PanelID"] + " #divProviderPaging";
                        var ClassControlName = "Admin_Provider";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ProviderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_Provider.ProviderSearch(PrimaryID, PageNumber, ResultPerPage);
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
        if (Admin_Provider.params["PanelID"] == "pnlAdmin_CCMCareTeamDetail") {
            $("#" + Admin_Provider.params["Panel"] + " #dgvProvider").dataTable().fnDestroy();
            $("#" + Admin_Provider.params["Panel"] + " #pnlProvider_Result #dgvProvider tbody").find("tr").remove();
        }
        else {
            $("#" + Admin_Provider.params["PanelID"] + " #dgvProvider").dataTable().fnDestroy();
            $("#" + Admin_Provider.params["PanelID"] + " #pnlProvider_Result #dgvProvider tbody").find("tr").remove();
        }
        if (response.ProviderCount > 0) {
            var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);
            $.each(ProviderLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvProvider_row" + item.ProviderId);
                $row.attr("ProviderId", item.ProviderId);

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

                if (item.IsSpecialist == "True") {
                    specialist = "YES";
                }
                else {
                    specialist = "NO";
                }

                var selectProvider = "";//disabled
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                if (Admin_Provider.params["FromAdmin"] == "0") {
                    var FullName = item.LastName + ', ' + item.FirstName + ' ' + item.MiddleInitial;
                    var selectMethod = "Admin_Provider.FillProviderName('" + item.ProviderId + "','" + FullName + "','" + item.EntityName + "','" + item.ShortName + "','" + item.CLIA + "','" + item.NPI + "',event);"
                    selectProvider = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>';

                    if (ClassDisabled != "disabled")
                        $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_Provider.ProviderEdit('" + item.ProviderId + "',event,'" + item.EntityId + "');");
                }
                if (Admin_Provider.params["PanelID"] == "pnlReportsSSRSDashboard") {
                    $('#btn-add').hide();
                    //Start 07-09-2016 Humaira Yousaf for specialist
                    $row.append('<td style="display:none;">' + item.ProviderId + '</td><td>' + selectProvider + '</td><td>' + item.ShortName + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.SpecialtyName + '</td><td>' + item.NPI + '</td><td>' + item.EntityName + '</td><td>' + specialist + '</td>');
                    //End 07-09-2016 Humaira Yousaf for specialist
                } else {
                    $('#btn-add').show();
                    $row.append('<td style="display:none;">' + item.ProviderId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_Provider.ProviderDelete(\'' + item.ProviderId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Provider.ProviderEdit(\'' + item.ProviderId + '\',event,\'' + item.EntityId + '\');"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_Provider.ProviderActiveInactive(\'' + item.ProviderId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectProvider + '</td><td>' + item.ShortName + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.SpecialtyName + '</td><td>' + item.NPI + '</td><td>' + item.EntityName + '</td><td>' + specialist + '</td>');
                }

                $("#" + Admin_Provider.params["PanelID"] + " #pnlProvider_Result #dgvProvider tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_Provider.params["PanelID"] + ' #dgvProvider').DataTable({
                "language": {
                    "emptyTable": "No Provider Found"
                }, "autoWidth": false, "bLengthChange": false, "bSort": false, "aoColumnDefs": [{ "bSortable": false }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_Provider.params["PanelID"] + ' #dgvProvider'))
            ;
        else
            $("#" + Admin_Provider.params["PanelID"] + " #pnlProvider_Result #dgvProvider").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$("#" + Admin_Provider.params["PanelID"] + " #pnlProvider_Result #dgvProvider").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    FillProviderName: function (ProviderId, ProviderName, EntityName, shortname,CLIA ,NPI,event) {
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = " #txtProvider";
        var RefCtrlHidden = " #hfProvider";
        var RefCtrlLabel = " #lblProvider";
        var RefCtrlLink = " #lnkProviderEdit";
        if (Admin_Provider.params["RefCtrl"] != null)
            RefCtrl = " #" + Admin_Provider.params["RefCtrl"];
        if (Admin_Provider.params["RefCtrlHidden"] != null)
            RefCtrlHidden = " #" + Admin_Provider.params["RefCtrlHidden"];
        if (Admin_Provider.params["RefCtrlLabel"] != null)
            RefCtrlLabel = " #" + Admin_Provider.params["RefCtrlLabel"];
        if (Admin_Provider.params["RefCtrlLink"] != null)
            RefCtrlLink = " #" + Admin_Provider.params["RefCtrlLink"];

        if (Admin_Provider.params.ParentRefCtrl) {
            RefCtrl = " #" + Admin_Provider.params.ParentRefCtrl + RefCtrl;
            RefCtrlHidden = " #" + Admin_Provider.params.ParentRefCtrl + RefCtrlHidden;
            RefCtrlLabel = " #" + Admin_Provider.params.ParentRefCtrl + RefCtrlLabel;
            RefCtrlLink = " #" + Admin_Provider.params.ParentRefCtrl + RefCtrlLink;
        }

        if (Admin_Provider.params["PanelID"] == "blockHoursDetail") {
            if (globalAppdata.AppUserName == "MDVISION")
                $('#' + Admin_Provider.params["PanelID"] + RefCtrl).val(ProviderName + ' - ' + EntityName);//.focus();
            else
                $('#' + Admin_Provider.params["PanelID"] + RefCtrl).val(ProviderName);//.focus();
        }
        else if (Admin_Provider.params["PanelID"] == "resourcesDetail") {
            if (globalAppdata.AppUserName == "MDVISION") {
                $('#' + Admin_Provider.params["PanelID"] + RefCtrl).val(ProviderName + ' - ' + EntityName);
                ProviderName = ProviderName + ' - ' + EntityName;
            }
            else
                $('#' + Admin_Provider.params["PanelID"] + RefCtrl).val(ProviderName);
        }
        else {
            $('#' + Admin_Provider.params["PanelID"] + RefCtrl).val(ProviderName);//.focus();
            $("#pnlDashboard #NoteProviderText").val( ProviderName);
        }

        //$('#' + Admin_Provider.params["PanelID"] + RefCtrl).val(ProviderName).focus();
        $('#' + Admin_Provider.params["PanelID"] + RefCtrlHidden).val(ProviderId);
        $('#' + Admin_Provider.params["PanelID"] + RefCtrl).attr("providerid", ProviderId);
        if (Admin_Provider.params["ProviderNPI"])
        $('#' + Admin_Provider.params["PanelID"] + " #" + Admin_Provider.params["ProviderNPI"]).val(NPI);

        if ($('#' + Admin_Provider.params["PanelID"] + RefCtrl).data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#' + Admin_Provider.params["PanelID"] + RefCtrl), ProviderName, $('#' + Admin_Provider.params["PanelID"] + RefCtrlHidden), ProviderId);
        else
            utility.SetAutoCompleteSource($('#' + Admin_Provider.params["PanelID"] + RefCtrl), $('#' + Admin_Provider.params["PanelID"] + RefCtrlHidden));

        $('#' + Admin_Provider.params["PanelID"] + RefCtrlLabel).css("display", "none");
        $('#' + Admin_Provider.params["PanelID"] + RefCtrlLink).css("display", "inline");
        if (Admin_Provider.params["PanelID"] == "pnlClinicalNotes") {
            Clinical_Notes.UnlinkAppointment(this, 'pnlClinicalNotes', true);
        }
        if (Admin_Provider.params["PanelID"] == "pnlReportsSSRSDashboard") {
            Admin_Provider.params["ProviderId"] = ProviderId;
            $('#' + Admin_Provider.params["PanelID"] + RefCtrlLabel).css("display", "inline");
        } else
            if (Admin_Provider.params["IsOptional"] != null && Admin_Provider.params["RefForm"] != null && Admin_Provider.params["IsOptional"] == false && Admin_Provider.params.ParentCtrl != "ERADetail" && Admin_Provider.params.ParentCtrl != "Patient_Referrals_Outgoing_Detail" && Admin_Provider.params.ParentCtrl != "OrderSet_Patient_Referrals_Outgoing_Detail") {
                $('#' + Admin_Provider.params["PanelID"] + ' #' + Admin_Provider.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_Provider.params["PanelID"] + RefCtrl).attr("name"));
            }
        // Start 02/02/2016 Adnan Maqbool Khan for bug # 3247
        //if ($("#" + Admin_Provider.params["PanelID"] + " #" + Admin_Provider.params["RefForm"]).data('bootstrapValidator') != null && typeof $("#" + Admin_Provider.params["PanelID"] + " #" + Admin_Provider.params["RefForm"]).data('bootstrapValidator') != 'undefined') {
        //    $('#' + Admin_Provider.params["PanelID"] + ' #' + Admin_Provider.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_Provider.params["PanelID"] + RefCtrl).attr("name"));
        //}
        //end
        if (Admin_Provider.params["ParentCtrl"] == "iTrackTabMIPSummary") {
            $("#pnliTrackMIPSummary #txtIndividualProviderNPI").val(NPI);
            iTrack_MIPSummary.GetMIPSProvider(shortname, "ProviderPopUp");
        }
        UnloadActionPan(Admin_Provider.params["ParentCtrl"], "Admin_Provider");

        if (Admin_Provider.params.ParentCtrl == "Admin_CCMCareTeamDetail") {
            Admin_CCMCareTeamDetail.Grid("Provider", ProviderId, ProviderName);
        }
        if (Admin_Provider.params.ParentCtrl == "ClinicalLabOrderDetail") {
            ClinicalLabOrderDetail.EnableDisableTestSearch(null,true);
        }
        //if (Admin_Provider.params.ParentCtrl == "clinicalTabNotes") {
        //    $('#' + Admin_Provider.params["PanelID"] + RefCtrl).trigger('blur');
        //}
        if (Admin_Provider.params.ParentCtrl == "appointmentDetail") {
                appointmentDetail.OpenPatReferralSearch($('#appointmentDetail #ddlInsurancePlan').val());          

        }

        if (Admin_Provider.params["ParentCtrl"] == "Patient_Referral") {

            Patient_Referral.BindProvider(Admin_Provider.params["RefCtrl"], Admin_Provider.params["RefCtrlHidden"], false, shortname);
        }
        else if (Admin_Provider.params["ParentCtrl"] == "EncounterChargeCapture") {
            EncounterChargeCapture.BindCLIA(CLIA);
        }
       else if (Admin_Provider.params.ParentCtrl == "ClinicalProcedureOrderDetail") {
            ClinicalProcedureOrderDetail.favoriteListSearch();
       }
       else if (Admin_Provider.params.ParentCtrl == "ClinicalRadiologyResultDetail") {
           ClinicalRadiologyResultDetail.setFavSearch();
       }
        $('#' + Admin_Provider.params["PanelID"] + RefCtrl).focus();

    },

    SearchProvider: function (ProviderData, ProviderId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        if (Admin_Provider.params.ParentCtrl == 'Patient_Referrals_Outgoing_Detail' || Admin_Provider.params.ParentCtrl != "OrderSet_Patient_Referrals_Outgoing_Detail") {
            var data = "ProviderData=" + ProviderData + "&ProviderID=" + ProviderId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage + "&ParentCtrl=" + Admin_Provider.params.ParentCtrl;
        }
        else {
            var data = "ProviderData=" + ProviderData + "&ProviderID=" + ProviderId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;

        }
        // serach parameter , class name, command name of class 
        if (Admin_Provider.params.OnlyEntity)
            return MDVisionService.defaultService(data, "ADMIN_PROVIDER", "SEARCH_PROVIDER_ENTITYBASED");
        else
            return MDVisionService.defaultService(data, "ADMIN_PROVIDER", "SEARCH_PROVIDER");
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
        if (Admin_Provider.params["FromAdmin"] == "0") {

            if (Admin_Provider.params != null && Admin_Provider.params.ParentCtrl != null && Admin_Provider.params.PanelID != 'pnlAdminProvider') {
                UnloadActionPan(Admin_Provider.params.ParentCtrl, 'Admin_Provider', null, Admin_Provider.params.PanelID);
            }
            else if (Admin_Provider.params != null && Admin_Provider.params.ParentCtrl != null) {
                UnloadActionPan(Admin_Provider.params.ParentCtrl, 'Admin_Provider');
            }
            else
                UnloadActionPan(null, 'Admin_Provider');

        }
        else {
            RemoveAdminTab();
        }
    },
}
