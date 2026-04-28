
Admin_ReferringProvider = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_ReferringProvider.params = params;

        if (Admin_ReferringProvider.params["FromAdmin"] == "0" && Admin_ReferringProvider.params["PanelID"] == 'pnlAdminReferringProvider')
            Admin_ReferringProvider.params["FromAdmin"] = "1";

        if (Admin_ReferringProvider.params.ParentCtrl) {
            $("#pnlAdminReferringProvider #modaldialog").addClass("modal-dialog modal-dialog-full");
        } else {
            $("#pnlAdminReferringProvider #modaldialog").removeClass("modal-dialog modal-dialog-full");
        }

        if (Admin_ReferringProvider.bIsFirstLoad) {
            Admin_ReferringProvider.bIsFirstLoad = false;
            var self = "";
            if (Admin_ReferringProvider.params["PanelID"] != "pnlAdminReferringProvider")
                self = $('#' + Admin_ReferringProvider.params["PanelID"] + ' #pnlAdminReferringProvider');
            else
                self = $('#pnlAdminReferringProvider');

            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {
                //if (globalAppdata['IsAdmin'] != "True") {
                //    $("#pnlReferringProvider_Search #divReferringProvider_Entity").css("display", "none");
                //    $("#pnlReferringProvider_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                //}
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    //Start 22-12-2016 Humaira Yousaf for outgoing referrals
                    if (Admin_ReferringProvider.params.ParentCtrl == 'Patient_Referral' || Admin_ReferringProvider.params.ParentCtrl == 'Patient_Referrals_Outgoing_Detail' || Admin_ReferringProvider.params.ParentCtrl == "OrderSet_Patient_Referrals_Outgoing_Detail") {
                        self.find("#ddlEntity").val('');
                    }
                    else {
                        self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                    }
                    //End 22-12-2016 Humaira Yousaf for outgoing referrals
                }
                Admin_ReferringProvider.ReferringProviderSearch();
            });

            if (Admin_ReferringProvider.params.Title != null)
                self.find("#headingTitle").text(Admin_ReferringProvider.params.Title);


        }
    },

    ReferringProviderAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referring Provider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ReferringProviderId"] = null;
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_ReferringProvider.params["FromAdmin"];
                if (Admin_ReferringProvider.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_ReferringProvider';
                }
                if (Admin_ReferringProvider.params.Title != null && Admin_ReferringProvider.params.Title == "Search PCP Provider") {
                    params["Title"] = "PCP";
                }

                LoadActionPan('referringproviderDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ReferringProviderEdit: function (ReferringProviderId, EntityId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#" + Admin_ReferringProvider.params["PanelID"] + ' #gvReferringProvider_row' + ReferringProviderId));
        AppPrivileges.GetFormPrivileges("Referring Provider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ReferringProviderId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ReferringProviderId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Admin_ReferringProvider.params["FromAdmin"];
                    params["EntityId"] = EntityId;

                    if (Admin_ReferringProvider.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Admin_ReferringProvider';
                    }
                    if (Admin_ReferringProvider.params.Title != null && Admin_ReferringProvider.params.Title == "Search PCP Provider") {
                        params["Title"] = "PCP";
                    }

                    LoadActionPan('referringproviderDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ReferringProviderDelete: function (ReferringProviderId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        utility.SelectGridRow($("#" + Admin_ReferringProvider.params["PanelID"] + ' #gvReferringProvider_row' + ReferringProviderId));
        AppPrivileges.GetFormPrivileges("Referring Provider", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ReferringProviderId;
                    var oTable = $('#' + Admin_ReferringProvider.params["PanelID"] + ' #dgvReferringProvider').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_ReferringProvider.DeleteReferringProvider(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#' + Admin_ReferringProvider.params["PanelID"] + ' #dgvReferringProvider').DataTable();
                                //table1.row('.active').remove().draw(false);
                                CacheManager.BindCodes('GetRefProviders', true);
                                utility.DisplayMessages(response.Message, 1);
                                // Begin: Jan 27th, 2016 Author: Abdur Rehman Latif, PMS-3055
                                Admin_ReferringProvider.ReferringProviderSearch();
                                // End: Jan 27th, 2016 Author: Abdur Rehman Latif, PMS-3055
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

    ReferringProviderActiveInactive: function (ReferringProviderId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Referring Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ReferringProviderId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        referringproviderDetail.UpdateReferringProviderActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                CacheManager.BindCodes('GetRefProviders', true);
                                Admin_ReferringProvider.ReferringProviderSearch('0');
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

    ReferringProviderSearch: function (ReferringProviderId, PageNo, rpp, ComeFromSearchBtn) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referring Provider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_ReferringProvider.params["PanelID"] + " #pnlReferringProvider_Result").css("display") == "none") {
                    $("#" + Admin_ReferringProvider.params["PanelID"] + " #pnlReferringProvider_Result").show();
                }

                var self = $("#" + Admin_ReferringProvider.params["PanelID"] + " #pnlReferringProvider_Search");
                var myJSON = self.getMyJSON();

                Admin_ReferringProvider.SearchReferringProvider(myJSON, ReferringProviderId, PageNo, rpp, ComeFromSearchBtn).done(function (response) {
                    if (response.status != false) {
                        Admin_ReferringProvider.ReferringProviderGridLoad(response);
                        var TableControl = Admin_ReferringProvider.params["PanelID"] + " #dgvReferringProvider";
                        var PagingPanelControlID = Admin_ReferringProvider.params["PanelID"] + " #divReferringProviderPaging";
                        var ClassControlName = "Admin_ReferringProvider";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ReferringProviderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_ReferringProvider.ReferringProviderSearch(PrimaryID, PageNumber, ResultPerPage);
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

    ReferringProviderGridLoad: function (response) {
        $("#" + Admin_ReferringProvider.params["PanelID"] + " #dgvReferringProvider").dataTable().fnDestroy();
        $("#" + Admin_ReferringProvider.params["PanelID"] + " #pnlReferringProvider_Result #dgvReferringProvider tbody").find("tr").remove();
        if (response.ReferringProviderCount > 0) {
            var ReferringProviderLoadJSONData = JSON.parse(response.ReferringProviderLoad_JSON);
            $.each(ReferringProviderLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvReferringProvider_row" + item.ReferringProviderId);
                $row.attr("ReferringProviderId", item.ReferringProviderId);
                $row.attr("entityid", item.EntityId);

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
                var selectRefProvider = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                if (Admin_ReferringProvider.params["FromAdmin"] == "0") {
                    var selectMethod = "";
                    if (Admin_ReferringProvider.params["RefCtrl"] == "txtRefProvider" || Admin_ReferringProvider.params["RefCtrl"] == "txtRefProviderReferral" ||
                        Admin_ReferringProvider.params["RefCtrl"] == "ddlReferringFrom" || Admin_ReferringProvider.params["RefCtrl"] == "ddlReferringTo" ||
                        Admin_ReferringProvider.params["RefCtrl"] == "ddlOutgoingReferringFrom" || Admin_ReferringProvider.params["RefCtrl"] == "ddlOutgoingReferringTo") { // Fill Referring Provider
                        //Start 22-12-2016 Humaira Yousaf for outgoing referrals
                        if (Admin_ReferringProvider.params["ParentCtrl"] == "Patient_Referrals_Outgoing_Detail" || Admin_ReferringProvider.params["ParentCtrl"] == "OrderSet_Patient_Referrals_Outgoing_Detail") {
                            selectMethod = "Admin_ReferringProvider.FillRefProviderName('" + item.ReferringProviderId + "','" + item.LastName + ", " + item.FirstName + " (" + item.EntityName + ")','" + item.NPI + "', event);"
                        } else if (Admin_ReferringProvider.params["ParentCtrl"] == "Batch_FaxSend") {
                            selectMethod = "Admin_ReferringProvider.FillRefProviderNameForFax('" + item.Fax + "','" + item.LastName + ", " + item.FirstName + "', event);"
                        }
                        else {
                            selectMethod = "Admin_ReferringProvider.FillRefProviderName('" + item.ReferringProviderId + "','" + item.LastName + ", " + item.FirstName + "','" + item.NPI + "', event);"
                        }
                        //End 22-12-2016 Humaira Yousaf for outgoing referrals
                        selectRefProvider = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    }
                    else if (Admin_ReferringProvider.params["RefCtrl"] == "txtProvider" && (Admin_ReferringProvider.params["ParentCtrl"] == "Patient_Referrals_Outgoing_Detail" || Admin_ReferringProvider.params["ParentCtrl"] == "OrderSet_Patient_Referrals_Outgoing_Detail" || Admin_ReferringProvider.params["ParentCtrl"] == "Patient_Referrals" || Admin_ReferringProvider.params["ParentCtrl"] == "patTabReferrals")) { // Fill Referring Provider
                        selectMethod = "Admin_ReferringProvider.FillRefProviderName('" + item.ReferringProviderId + "','" + item.LastName + ", " + item.FirstName + "','" + item.NPI + "', event);"
                        selectRefProvider = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    }
                    else { // Fill PCP
                        selectMethod = "Admin_ReferringProvider.FillPCPName('" + item.ReferringProviderId + "','" + item.LastName + ", " + item.FirstName + "', event);"
                        selectRefProvider = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';

                    }

                    if (ClassDisabled != "disabled")
                        $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_ReferringProvider.ReferringProviderEdit('" + item.ReferringProviderId + "','" + item.EntityId + "',event);");
                }
                if (Admin_ReferringProvider.params["PanelID"] == "pnlReportsSSRSDashboard") {
                    $('#btn-add').hide();
                    $row.append('<td style="display:none;">' + item.ReferringProviderId + '</td><td>' + selectRefProvider + '</td><td>' + item.FirstName + '</td><td>' + item.LastName + '</td><td class="text-center">' + item.PhoneNo + '</td><td class="text-center">' + item.Fax + '</td><td class="text-center">' + item.NPI + '</td>');
                } else {
                    $('#btn-add').show();
                    $row.append('<td style="display:none;">' + item.ReferringProviderId + '</td><td><a class="btn  btn-xs" href="#"onclick="Admin_ReferringProvider.ReferringProviderDelete(\'' + item.ReferringProviderId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ReferringProvider.ReferringProviderEdit(\'' + item.ReferringProviderId + '\',\'' + item.EntityId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Admin_ReferringProvider.ReferringProviderActiveInactive(\'' + item.ReferringProviderId + '\',' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectRefProvider + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.SpecialityName + '</td><td class="text-center">' + item.PhoneNo + '</td><td class="text-center">' + item.Fax + '</td><td class="text-center">' + item.NPI + '</td><td class="text-center" >' + item.EntityName + '</td>');
                }
                $("#" + Admin_ReferringProvider.params["PanelID"] + " #pnlReferringProvider_Result #dgvReferringProvider tbody").last().append($row);
            });
        }
        else {

            if (Admin_ReferringProvider.params.Title != null && Admin_ReferringProvider.params.Title == "Search PCP Provider") {
                var emptytablemsg = "No PCP Provider Found"
            }
            else {
                var emptytablemsg = "No Referring Provider Found"
            }
            $('#' + Admin_ReferringProvider.params["PanelID"] + ' #dgvReferringProvider').DataTable({
                "language": {
                    "emptyTable": emptytablemsg
                },
                "autoWidth": false, "bLengthChange": false, "bSort": false, "aoColumnDefs": [{ "bSortable": false }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_ReferringProvider.params["PanelID"] + ' #dgvReferringProvider'))
            ;
        else
            $("#" + Admin_ReferringProvider.params["PanelID"] + " #pnlReferringProvider_Result #dgvReferringProvider").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //     $("#" + Admin_ReferringProvider.params["PanelID"] + " #pnlReferringProvider_Result #dgvReferringProvider").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page 
    },

    SearchReferringProvider: function (ReferringProviderData, ReferringProviderID, PageNumber, RowsPerPage, ComeFromSearchBtn) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        if (utility.IsNullOrEmptyString(ComeFromSearchBtn))
            ComeFromSearchBtn = '';
        //Start 22-12-2016 Humaira Yousaf for outgoing referrals
        if (Admin_ReferringProvider.params.ParentCtrl == 'Patient_Referrals_Outgoing_Detail' || Admin_ReferringProvider.params.ParentCtrl == "OrderSet_Patient_Referrals_Outgoing_Detail") {
            var data = "ReferringProviderData=" + ReferringProviderData + "&ReferringProviderID=" + ReferringProviderID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage + "&ParentCtrl=" + Admin_ReferringProvider.params.ParentCtrl + "&ComeFromSearchBtn=" + ComeFromSearchBtn;
        }
        else {
            var data = "ReferringProviderData=" + ReferringProviderData + "&ReferringProviderID=" + ReferringProviderID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage + "&ComeFromSearchBtn=" + ComeFromSearchBtn;
        }
        //End 22-12-2016 Humaira Yousaf for outgoing referrals
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REFERRING_PROVIDER", "SEARCH_REFERRING_PROVIDER");
    },

    DeleteReferringProvider: function (ReferringProviderID) {
        var data = "ReferringProviderID=" + ReferringProviderID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REFERRING_PROVIDER_DETAIL", "DELETE_REFERRING_PROVIDER");
    },

    SelectGridRow: function (obj) {
        if (obj.className != 'active') {
            $(obj).parent().children().removeClass('active');
            $(obj).addClass('active');
        }
    },

    FillPCPName: function (PCPId, PCPName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $('#' + Admin_ReferringProvider.params["PanelID"] + ' #txtPCP').val(PCPName);
        $('#' + Admin_ReferringProvider.params["PanelID"] + ' #hfPCP').val(PCPId);
        $('#' + Admin_ReferringProvider.params["PanelID"] + ' #lblPCP').css("display", "none");
        $('#' + Admin_ReferringProvider.params["PanelID"] + ' #lnkPCPEdit').css("display", "inline");
        utility.SetKendoAutoCompleteSourceforValidate($('#' + Admin_ReferringProvider.params["PanelID"] + ' #txtPCP'), PCPName, $('#' + Admin_ReferringProvider.params["PanelID"] + ' #hfPCP'), PCPId);
        if (Admin_ReferringProvider.params["IsOptional"] != null && Admin_ReferringProvider.params["RefForm"] != null && Admin_ReferringProvider.params["IsOptional"] == false) {
            $('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + Admin_ReferringProvider.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_ReferringProvider.params["PanelID"] + ' #txtPCP').attr("name"));
        }
        if (Admin_ReferringProvider.params["ParentCtrl"] == "Batch_FaxContacts") {
            Batch_FaxContacts.getContacts(Batch_FaxContacts.params.Id, Batch_FaxContacts.params.Type).done(function (resp) {
                if (resp.status != false) {
                    var contacts = [];
                    if (Batch_FaxContacts.params.Type == "Provider")
                        contacts = JSON.parse(resp.FaxProviderContacts);
                    else if (Batch_FaxContacts.params.Type == "Facility")
                        contacts = JSON.parse(resp.FaxFacilityContacts);
                    Batch_FaxContacts.FillReferringProvider_DbCall(PCPId).done(function (response) {
                        if (response.status != false) {
                            var ReferringProvider_detail = JSON.parse(response.ReferringProviderFill_JSON);
                            if (ReferringProvider_detail.txtFax != "") {
                                var cleanFaxNo = ReferringProvider_detail.txtFax.replace(/\D/g, "");
                                var arr = jQuery.grep(contacts, function (a) {
                                    return a.FaxNumber && a.FaxNumber.replace(/\D/g, "") == cleanFaxNo;
                                });
                                if (arr.length == 0) {
                                    Batch_FaxContacts.SaveReceipient(ReferringProvider_detail.txtFirstName + ' ' + ReferringProvider_detail.txtLastName, ReferringProvider_detail.txtFax, Batch_FaxContacts.params.Id, Batch_FaxContacts.params.Type).done(function () {
                                        UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"], "Admin_ReferringProvider");
                                        Batch_FaxContacts.loadContacts();
                                    });
                                }
                                else {
                                    utility.DisplayMessages("Selected Provider already exist in the list.", 4);
                                    $('#' + Batch_FaxContacts.params.PanelID + ' #formpanelheading').resetAllControls(null);
                                }
                            }
                            else {
                                utility.DisplayMessages("Fax number missing for the selected Provider.", 4);
                            }
                        }
                        else
                            utility.DisplayMessages(response.Message, 3);
                    });
                }
                else {
                    Batch_FaxContacts.FillReferringProvider_DbCall(PCPId).done(function (response) {
                        if (response.status != false) {
                            var ReferringProvider_detail = JSON.parse(response.ReferringProviderFill_JSON);
                            if (ReferringProvider_detail.txtFax != "") {
                                Batch_FaxContacts.SaveReceipient(ReferringProvider_detail.txtFirstName + ' ' + ReferringProvider_detail.txtLastName, ReferringProvider_detail.txtFax, Batch_FaxContacts.params.Id, Batch_FaxContacts.params.Type).done(function () {
                                    Batch_FaxContacts.loadContacts();
                                    UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"], "Admin_ReferringProvider");
                                });
                            }
                            else {
                                utility.DisplayMessages("Fax number missing for the selected Provider.", 4);
                            }
                        }
                        else
                            utility.DisplayMessages(response.Message, 3);
                    });
                }
            });
        }
        if (Admin_ReferringProvider.params["ParentCtrl"] != "Batch_FaxContacts") {
            UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"], "Admin_ReferringProvider");
        }


        if (Admin_ReferringProvider.params.ParentCtrl == "Admin_CCMCareTeamDetail") {
            Admin_CCMCareTeamDetail.Grid("PCP", PCPId, PCPName);
        }
    },

    FillRefProviderName: function (RefProviderId, RefProviderName, NPI , event) {
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = "txtRefProvider";
        var RefCtrlHidden = "hfRefProvider";
        var RefNPI = "RefProviderNPI";
        if (Admin_ReferringProvider.params["RefCtrl"] != null)
            RefCtrl = Admin_ReferringProvider.params["RefCtrl"];
        if (Admin_ReferringProvider.params["RefCtrlHidden"] != null)
            RefCtrlHidden = Admin_ReferringProvider.params["RefCtrlHidden"];
        if (Admin_ReferringProvider.params["RefProviderNPI"] != null)
            RefNPI = Admin_ReferringProvider.params["RefProviderNPI"];


        $('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + RefCtrl).val(RefProviderName);//.focus();  //PMS 5600, Calls validation before Binding. Focused field after Binding
        $('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + RefCtrlHidden).val(RefProviderId);
        $('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + RefNPI).val(NPI);

        if (Admin_ReferringProvider.params["PanelID"] != "pnlPatientReferralsIncomingDetail" && Admin_ReferringProvider.params["PanelID"] != "pnlPatientReferralsOutgoingDetail" && Admin_ReferringProvider.params["PanelID"] != "pnlOrderSetPatientReferralsOutgoingDetail" && Admin_ReferringProvider.params["PanelID"] != "pnlPatientReferrals") {
            $('#' + Admin_ReferringProvider.params["PanelID"] + ' #lblRefProvider').css("display", "none");
            $('#' + Admin_ReferringProvider.params["PanelID"] + ' #lnkRefProviderEdit').css("display", "inline");
        }
        if (Admin_ReferringProvider.params["PanelID"] == "pnlReportsSSRSDashboard") {
            $('#' + Admin_ReferringProvider.params["PanelID"] + ' #lblRefProvider').css("display", "inline");
            Admin_ReferringProvider.params["ReferringProviderId"] = RefProviderId;
        } else
            if (Admin_ReferringProvider.params["IsOptional"] != null && Admin_ReferringProvider.params["RefForm"] != null && Admin_ReferringProvider.params["IsOptional"] == false) {
                $('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + Admin_ReferringProvider.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + RefCtrl).attr("name"));
            }
        UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"]);
        if ($('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + RefCtrl).data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + RefCtrl), RefProviderName, $('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + RefCtrlHidden), RefProviderId);
        else
            utility.SetAutoCompleteSource($('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + RefCtrl), $('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + RefCtrlHidden));

        $('#' + Admin_ReferringProvider.params["PanelID"] + ' #' + RefCtrl).focus();
    },
    UnLoadTab: function (Tab) {
        if (Admin_ReferringProvider.params["FromAdmin"] == "0") {


            if (Admin_ReferringProvider.params != null && Admin_ReferringProvider.params.ParentCtrl != null && Admin_ReferringProvider.params.PanelID != "pnlAdminReferringProvider") {
                UnloadActionPan(Admin_ReferringProvider.params.ParentCtrl, 'Admin_ReferringProvider', null, Admin_ReferringProvider.params.PanelID);
            }
            else if (Admin_ReferringProvider.params != null && Admin_ReferringProvider.params.ParentCtrl != null) {
                UnloadActionPan(Admin_ReferringProvider.params.ParentCtrl, 'Admin_ReferringProvider');
            }

            else
                UnloadActionPan(null, 'Admin_ReferringProvider');
        }
        else {
            RemoveAdminTab();
        }
    },

    FillRefProviderNameForFax: function (FaxNum, RefProviderName, event) {

        if (FaxNum) {
            var $outgoingRefsDdl = $('#' + Batch_FaxSend.params.PanelID + ' #ddlNotesOutgroinRefs');
            //var isProviderExist = false;

            var $outgoingRefDdlOptions = $outgoingRefsDdl.find('option');
            $outgoingRefDdlOptions.each(function (i, e) {
                if (RefProviderName == e.text) {
                    //isProviderExist = true;
                    $(e).remove();
                    return false;
                }
            });

            //if (!isProviderExist) {
            UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"]);

            var length = $outgoingRefDdlOptions.length + 1;
            $outgoingRefsDdl.append(
                $('<option/>', {
                    value: length,
                    html: RefProviderName,
                    refvalue: FaxNum
                })
            );
            $outgoingRefsDdl.multiselect('rebuild');
            $outgoingRefsDdl.multiselect('select', length, true);

            //} else {
            //    utility.DisplayMessages("Selected Provider already exist in the list.", 4);
            //}
        } else {
            utility.DisplayMessages("Fax number missing for the selected Provider.", 4);
        }

    },
}

