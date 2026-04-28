
Admin_CPTCode = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_CPTCode.params = params;

        if (Admin_CPTCode.params["FromAdmin"] == "0" && Admin_CPTCode.params["PanelID"] == 'pnlAdminCPTCode')
            Admin_CPTCode.params["FromAdmin"] = "1";

        if (Admin_CPTCode.bIsFirstLoad) {
            Admin_CPTCode.bIsFirstLoad = false;

            var self = "";
            if (Admin_CPTCode.params["PanelID"] != "pnlAdminCPTCode") {
                self = $('#' + Admin_CPTCode.params["PanelID"] + " #pnlAdminCPTCode")
            }
            else
                self = $('#' + Admin_CPTCode.params["PanelID"]);

            if (Admin_CPTCode.params["ParentCtrl"] == "mstrTabReports") {
                self.find('#btn_CPTAdd').hide();

            } else {
                self.find('#btn_CPTAdd').show();
            }
            self.loadDropDowns(true).done(function () {
                //if (globalAppdata['IsAdmin'] != "True") {
                //    $('#' + Admin_CPTCode.params["PanelID"] + ' #divCPTCode_Entity').css("display", "none");
                //    $('#' + Admin_CPTCode.params["PanelID"] + ' #ddlEntity').val(globalAppdata["SeletedEntityId"]);
                //}


                ////Set Search Parameters if CPT Open from other.
                //if (Admin_CPTCode.params["EntityId"] && Admin_CPTCode.params["PanelID"] != "pnlAdminCPTCode") {
                //    $('#' + Admin_CPTCode.params["PanelID"] + ' #ddlEntity').val(Admin_CPTCode.params["EntityId"]);
                //    $('#' + Admin_CPTCode.params["PanelID"] + ' #ddlEntity').attr("disabled", "disabled");
                //    //Change Specialty against Entity
                //    Admin_CPTCode.LoadEntityBasedData(Admin_CPTCode.params["EntityId"]);

                //}

            });

            $('#' + Admin_CPTCode.params["PanelID"] + ' #pnlCPTCode_Result #ColumnAction').addClass("size95");

            if (Admin_CPTCode.params != null) {
                if (Admin_CPTCode.params.ParentCtrl != null) {
                    if (Admin_CPTCode.params.ParentCtrl == "Patient_PreAuthorization") {
                        $('#' + Admin_CPTCode.params["PanelID"] + ' #pnlCPTCode_Result #ColumnAction').removeClass("size95").addClass("size130");
                    }
                }
            }
            



            AppPrivileges.GetFormPrivileges("CPT", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_CPTCode.CPTCodeSearch();
                }
            });
        }
    },
    BindAutoComplete: function () {

        CacheManager.BindAutoCompleteText('#' + Admin_CPTCode.params["PanelID"] + ' #txtCPTCode', 'GetCPTCode', true, null, "");

    },
    CPTCodeAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("CPT", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["CPTCodeId"] = "-1";
                params["EntityId"] = Admin_CPTCode.params["EntityId"];
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_CPTCode.params["FromAdmin"];

                if (Admin_CPTCode.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_CPTCode';
                }
                LoadActionPan('cptcodeDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    CPTCodeEdit: function (CPTCodeId,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	utility.SelectGridRow($('#gvCPTCode_row' + CPTCodeId));
        AppPrivileges.GetFormPrivileges("CPT", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = CPTCodeId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["CPTCodeId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["EntityId"] = Admin_CPTCode.params["EntityId"];
                    params["FromAdmin"] = Admin_CPTCode.params["FromAdmin"];

                    if (Admin_CPTCode.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Admin_CPTCode';
                    }
                    LoadActionPan('cptcodeDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    CPTCodeDelete: function (CPTCodeId,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	utility.SelectGridRow($('#gvCPTCode_row' + CPTCodeId));
        AppPrivileges.GetFormPrivileges("CPT", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = CPTCodeId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_CPTCode.DeleteCPTCode(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvCPTCode').DataTable();
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

    CPTCodeActiveInactive: function (CPTCodeId, IsActive,event) {
        PageNo = null;
        rpp = null;
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	
        AppPrivileges.GetFormPrivileges("CPT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = CPTCodeId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        cptcodeDetail.UpdateCPTCodeActiveInactive(selectedValue, IsActive, PageNo, rpp).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_CPTCode.CPTCodeSearch('0');
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

    CPTCodeSearch: function (CPTCodeId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("CPT", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Admin_CPTCode.params["PanelID"] + ' #pnlCPTCode_Result').css("display") == "none") {
                    $('#' + Admin_CPTCode.params["PanelID"] + ' #pnlCPTCode_Result').show();
                }
                var self = "";
                if (Admin_CPTCode.params["PanelID"] != "pnlAdminCPTCode") {
                    self = $('#' + Admin_CPTCode.params["PanelID"] + " #pnlAdminCPTCode #pnlCPTCode_Search")
                }
                else
                    self = $('#' + Admin_CPTCode.params["PanelID"] + " #pnlCPTCode_Search");
                // var self = $('#' + Admin_CPTCode.params["PanelID"] + ' #pnlCPTCode_Search');
                var myJSON = self.getMyJSON();

                Admin_CPTCode.SearchCPTCode(myJSON, CPTCodeId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_CPTCode.CPTCodeGridLoad(response);
                        if (response.CPTCount > 0) {
                            $("#" + Admin_CPTCode.params["PanelID"] + " #divCPTPaging").css("display", "inline");
                            var TableControl = Admin_CPTCode.params["PanelID"] + " #dgvCPTCode";
                            var PagingPanelControlID = Admin_CPTCode.params["PanelID"] + " #divCPTPaging";
                            var ClassControlName = "Admin_CPTCode";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(CreatePagination(response.CPTCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Admin_CPTCode.CPTCodeSearch(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);
                            //Showing 1 to 15 of 15 entries
                            //var RecordsPerPage = rpp != null ? rpp : 15;
                            //var CurrentPage = PageNo != null ? PageNo : 1;
                            //var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            //var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            //if (PageNo == null) {
                            //    utility.GetCustomPaging("divCPTPaging", response.iTotalDisplayRecords, 5, "Admin_CPTCode", CurrentPage, RecordsPerPage);
                            //}
                            //var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            //var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            //$("#" + Admin_CPTCode.params["PanelID"] + " #divCPTPaging #divShowingEntries").text(showingText);
                            //// Change Background Color to Black for selected page
                            //self.find("li").each(function () {
                            //    if ($(this).text() == CurrentPage) {
                            //        $(this).attr("class", "active");
                            //    }
                            //    else
                            //        $(this).removeAttr("class");
                            //});
                        }
                        else {
                            $("#" + Admin_CPTCode.params["PanelID"] + " #divCPTPaging").css("display", "none");
                        }
                      
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

    CPTCodeGridLoad: function (response) {
        $('#' + Admin_CPTCode.params["PanelID"] + ' #dgvCPTCode').dataTable().fnDestroy();
        $('#' + Admin_CPTCode.params["PanelID"] + ' #pnlCPTCode_Result #dgvCPTCode tbody').find("tr").remove();
        if (response.CPTCount > 0) {
            var CPTCodeLoadJSONData = JSON.parse(response.CPTLoad_JSON);
            $.each(CPTCodeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                
                $row.attr("id", "gvCPTCode_row" + item.CPTCodeId);
                $row.attr("CPTCodeId", item.CPTCodeId);
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
                var selectCPT = "";
                if (Admin_CPTCode.params["FromAdmin"] == "0") {
                    var selectMethod = "Admin_CPTCode.FillCPTCode('" + item.CPTCodeId + "','" + item.CPTCode + "','" + item.Description + "');";
                    selectCPT = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_CPTCode.CPTCodeEdit('" + item.CPTCodeId + "',event);");
                }
                $row.append('<td style="display:none;">' + item.CPTCodeId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_CPTCode.CPTCodeDelete(' + item.CPTCodeId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_CPTCode.CPTCodeEdit(' + item.CPTCodeId + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_CPTCode.CPTCodeActiveInactive(' + item.CPTCodeId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectCPT + '</td><td>' + item.CPTCode + '</td><td>' + item.Description + '</td><td>' + item.TypeOfServiceCode + '</td><td>' + item.SpecialtyName + '</td><td>' + item.Discontinued + '</td>');

                $('#' + Admin_CPTCode.params["PanelID"] + ' #pnlCPTCode_Result #dgvCPTCode tbody').last().append($row);
            });
        }
        else {
            $('#' + Admin_CPTCode.params["PanelID"] + ' #dgvCPTCode').DataTable({
                "language": {
                    "emptyTable": "No CPTCode Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_CPTCode.params["PanelID"] + ' #dgvCPTCode'))
            ;
        else
            $('#' + Admin_CPTCode.params["PanelID"] + ' #pnlCPTCode_Result #dgvCPTCode').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    //BindCPTCode: function () {

    //    $("input#txtCPTCode").autocomplete({
    //        //source: AllPatients, // pass an array (without a comma)
    //        source: function (request, response) {

    //            var AccountNo=  $('#appointmentDetail #txtAccountNo').val();
    //            if (AccountNo.length > 2) {
    //                // serach parameter , class name, command name of class 
    //                appointmentDetail.LoadActivePatients(AccountNo).done(function (responseData) {
    //                    if (responseData.status != false) {
    //                        if (responseData.PatientCount > 0) {
    //                            var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
    //                            var AllPatients = [];
    //                            $.each(PatientLoadJSONData, function (i, item) {
    //                                AllPatients.push({ id: item.PatientId, value: item.AccountNumber + " - " + item.FullName });
    //                            });
    //                            response(AllPatients);
    //                        }
    //                    }
    //                });
    //            }
    //        },
    //        select: function (event, ui) {
    //            appointmentDetail.FillPatientAccount(ui.item.id);
    //        }
    //    });
    //},

    LoadEntityBasedData: function (entityID) {

        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#' + Admin_CPTCode.params["PanelID"] + ' #lstSpeciality', 'GetSpecialty', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#' + Admin_CPTCode.params["PanelID"] + ' #lstSpeciality', 'GetSpecialty', true, entityID);

        }

        //$('#' + Admin_CPTCode.params["PanelID"] + ' #txtCPTCode').val("");
        //CacheManager.BindAutoCompleteText('#' + Admin_CPTCode.params["PanelID"] + ' #txtCPTCode', 'GetCPTCode', true, null, entityID);

    },
    FillCPTCode: function (CPTCodeId, CPTCode, Description) {

        var RefCtrl = " #txtCPTCode";
        var RefHiddenIdCtrl = " #hfcptcode";
        var RefHiddenCtrl = " #hfCPTCode";
        var RefCtrlDescription = " #hfCPTDescription";

        if (Admin_CPTCode.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_CPTCode.params["RefCtrl"];
        }
        if (Admin_CPTCode.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_CPTCode.params["RefHiddenIdCtrl"];
        }
        if (Admin_CPTCode.params["RefHiddenCtrl"] != null) {
            RefHiddenCtrl = " #" + Admin_CPTCode.params["RefHiddenCtrl"];
        }
        if (Admin_CPTCode.params["RefCtrlDescription"] != null) {
            RefCtrlDescription = " #" + Admin_CPTCode.params["RefCtrlDescription"];
        }

        $('#' + Admin_CPTCode.params["PanelID"] + RefCtrl).val(CPTCode);
        $('#' + Admin_CPTCode.params["PanelID"] + RefHiddenIdCtrl).val(CPTCodeId);
        $('#' + Admin_CPTCode.params["PanelID"] + RefHiddenCtrl).val(CPTCode);
        $('#' + Admin_CPTCode.params["PanelID"] + RefCtrlDescription).val(Description);

        UnloadActionPan(Admin_CPTCode.params["ParentCtrl"], "Admin_CPTCode");

        $('#' + Admin_CPTCode.params["PanelID"] + RefCtrl).focus();
    },
    ValidateCPTCode: function (CPTCode, EntityId) {

        return Admin_CPTCode.LoadCPTCode(CPTCode, EntityId).done(function (response) {

            if (response.length == 0) {
                utility.DisplayMessages("Invalid CPT Code.", 3);
            }
        });
    },
    CptCodeValidate: function (CPTCode, EntityId) {
        return Admin_CPTCode.LoadCPTCodes(CPTCode, EntityId).done(function (response) {
            if (response.length == 0) {
                utility.DisplayMessages("Invalid CPT Code.", 3);
            }
        });
    },
    LoadCPTCode: function (CPTCode, EntityId) {
        var data = "cptCode=" + CPTCode + "&entityId=" + EntityId;

        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "COMMON_CODE", "CHECK_CPT_CODE");
    },
    LoadCPTCodes: function (CPTCode, EntityId) {
        var data = "cptCode=" + CPTCode + "&entityId=" + EntityId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "COMMON_CODE", "VALIDATE_CPT_CODE");
    },

    SearchCPTCode: function (CPTCodeData, CPTCodeId, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "CPTCodeData=" + CPTCodeData + "&CPTCodeID=" + CPTCodeId + "&PageNo=" + PageNo + "&rpp=" + rpp;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE", "SEARCH_CPT_CODE");
    },
    SearchHPCSCode: function (CPTCodeData, CPTCodeId, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "CPTCodeData=" + CPTCodeData + "&CPTCodeID=" + CPTCodeId + "&PageNo=" + PageNo + "&rpp=" + rpp;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE", "SEARCH_HPCS_CODE");
    },

    DeleteCPTCode: function (CPTCodeId) {
        var data = "CPTCodeID=" + CPTCodeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "DELETE_CPT_CODE");
    },

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#" + Admin_CPTCode.params["PanelID"] + " li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Admin_CPTCode.CPTCodeSearch(null, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Admin_CPTCode.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_CPTCode.CPTCodeSearch(null, currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Admin_CPTCode.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_CPTCode.CPTCodeSearch(null, currentPageNo, 15);
        }
    },

    UnLoadTab: function () {
        if (Admin_CPTCode.params["FromAdmin"] == "0") {
            if (Admin_CPTCode.params != null && Admin_CPTCode.params.ParentCtrl != null) {
                UnloadActionPan(Admin_CPTCode.params.ParentCtrl, 'Admin_CPTCode');
            }
            else
                UnloadActionPan(null, 'Admin_CPTCode');

        }
        else {
            RemoveAdminTab();
        }
    },
}