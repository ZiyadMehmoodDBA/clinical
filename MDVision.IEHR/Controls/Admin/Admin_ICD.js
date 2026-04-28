
Admin_ICD = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        Admin_ICD.params = params;

        if (Admin_ICD.params["FromAdmin"] == "0" && Admin_ICD.params["PanelID"] == 'pnlAdminICD')
            Admin_ICD.params["FromAdmin"] = "1";

        if (Admin_ICD.bIsFirstLoad) {

            Admin_ICD.bIsFirstLoad = false;

            var self = "";
            if (Admin_ICD.params["PanelID"] != "pnlAdminICD") {
                self = $('#' + Admin_ICD.params["PanelID"] + " #pnlAdminICD")
            }
            else
                self = $('#' + Admin_ICD.params["PanelID"]);

            if (Admin_ICD.params["ParentCtrl"] == "mstrTabReports") {
                self.find('#btn_ICDAdd').hide();

            } else {
                self.find('#btn_ICDAdd').show();
            }
            self.loadDropDowns(true).done(function () {
                //if (globalAppdata['IsAdmin'] != "True") 
                //{
                //    $('#' + Admin_ICD.params["PanelID"] + ' #divICDCode_Entity').css("display", "none");
                //    $('#' + Admin_ICD.params["PanelID"] + ' #ddlEntity').val(globalAppdata["SeletedEntityId"]);
                //}

                //Set Search Parameters if CPT Open from other.
                //if (Admin_ICD.params["EntityId"] != null && Admin_ICD.params["EntityId"] != undefined && Admin_ICD.params["PanelID"] != "pnlAdminICD") {
                //    $('#' + Admin_ICD.params["PanelID"] + ' #ddlEntity').val(Admin_ICD.params["EntityId"]);
                //    $('#' + Admin_ICD.params["PanelID"] + ' #ddlEntity').attr("disabled", "disabled");
                //}
            });



            Admin_ICD.ICDSearch();
        }
    },

    ICDAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("ICD", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ICDId"] = null;
                params["EntityId"] = Admin_ICD.params["EntityId"];
                params["mode"] = "Add";

                if (Admin_ICD.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_ICD';
                }

                LoadActionPan('ICDDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ICDEdit: function (ICDId,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	utility.SelectGridRow($('#gvICD_row' + ICDId));
        AppPrivileges.GetFormPrivileges("ICD", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ICDId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ICDId"] = selectedValue;
                    params["EntityId"] = Admin_ICD.params["EntityId"];
                    params["mode"] = "Edit";

                    if (Admin_ICD.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Admin_ICD';
                    }

                    LoadActionPan('ICDDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ICDDelete: function (ICDId,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	utility.SelectGridRow($('#gvICD_row' + ICDId));
        AppPrivileges.GetFormPrivileges("ICD", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ICDId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_ICD.DeleteICD(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#' + Admin_ICD.params["PanelID"] + ' #dgvICD').DataTable();
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

    ICDActiveInactive: function (ICDId, IsActive,event) {
        PageNo = null;
        rpp = null;
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("ICD", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ICDId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        ICDDetail.UpdateICDActiveInactive(selectedValue, IsActive, PageNo, rpp).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_ICD.ICDSearch('0');
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

    ICDSearch: function (ICDId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("ICD", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Admin_ICD.params["PanelID"] + ' #pnlICD_Result').css("display") == "none") {
                    $('#' + Admin_ICD.params["PanelID"] + ' #pnlICD_Result').show();
                }
                var self = "";
                if (Admin_ICD.params["PanelID"] != "pnlAdminICD") {
                    self = $('#' + Admin_ICD.params["PanelID"] + " #pnlAdminICD #pnlICD_Search")
                }
                else
                    self = $('#' + Admin_ICD.params["PanelID"] + " #pnlICD_Search");

                //  var self = $('#' + Admin_ICD.params["PanelID"] + ' #pnlICD_Search');
                var myJSON = self.getMyJSON();

                Admin_ICD.SearchICD(myJSON, ICDId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_ICD.ICDGridLoad(response);
                        if (response.ICDCount > 0) {
                            $("#" + Admin_ICD.params["PanelID"] + " #divICDPaging").css("display", "inline");
                            var TableControl = Admin_ICD.params["PanelID"] + " #dgvICD";
                            var PagingPanelControlID = Admin_ICD.params["PanelID"] + " #divICDPaging";
                            var ClassControlName = "Admin_ICD";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(CreatePagination(response.ICDCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Admin_ICD.ICDSearch(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);
                            //Showing 1 to 15 of 15 entries
                            //var RecordsPerPage = rpp != null ? rpp : 15;
                            //var CurrentPage = PageNo != null ? PageNo : 1;
                            //var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            //var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            //if (PageNo == null) {
                            //    utility.GetCustomPaging("divICDPaging", response.iTotalDisplayRecords, 5, "Admin_ICD", CurrentPage, RecordsPerPage);
                            //}
                            //var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            //var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            //$("#" + Admin_ICD.params["PanelID"] + " #divICDPaging #divShowingEntries").text(showingText);
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
                            $("#" + Admin_ICD.params["PanelID"] + " #divICDPaging").css("display", "none");
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

    ICDGridLoad: function (response) {
        $('#' + Admin_ICD.params["PanelID"] + ' #dgvICD').dataTable().fnDestroy();
        $('#' + Admin_ICD.params["PanelID"] + ' #pnlICD_Result #dgvICD tbody').find("tr").remove();
        if (response.ICDCount > 0) {
            var ICDLoadJSONData = JSON.parse(response.ICDLoad_JSON);
            $.each(ICDLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                
                $row.attr("id", "gvICD_row" + item.ICDId);
                $row.attr("ICDId", item.ICDId);

                //var ActivelyUsed;
                //if (item.ActivelyUsed == "True")
                //    ActivelyUsed = "True";
                //else
                //    ActivelyUsed = "False";

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
                var selectICD = "";
                if (Admin_ICD.params["FromAdmin"] == "0") {
                    var selectMethod = "Admin_ICD.FillICDCode('" + item.ICDId + "','" + item.ICD9 + "','" + item.Description + "' ,'" + item.ICD10 + "' , '" + item.ICD10Description + "' ,'" + item.SNOMEDId + "','" + item.SNOMEDDescription + "');"
                    selectICD = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    //Admin_ICD.FillICDCode(4, '00.09', 'OTHER THERAPEUTIC ULTRASOUND' , 'S12.01XA , 'Stable burst fracture of first cervical vertebra, initial encounter for closed fracture' , '269063003''+ , 'Closed fracture of first cervical vertebra without mention of spinal cord injury');
                    $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_ICD.ICDEdit('" + item.ICDId + "',event);");
                }

                $row.append('<td style="display:none;">' + item.ICDId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_ICD.ICDDelete(' + item.ICDId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ICD.ICDEdit(' + item.ICDId + ',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_ICD.ICDActiveInactive(' + item.ICDId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectICD + '</td><td>' + item.ICD9 + '</td><td>' + item.Description + '</td><td>' + item.ICD10 + '</td><td>' + item.ICD10Description + '</td><td>' + item.SNOMEDId + '</td><td>' + item.SNOMEDDescription + '</td><td>' + item.ActivelyUsed + '</td><td>' + item.Valid + '</td>');

                $('#' + Admin_ICD.params["PanelID"] + ' #pnlICD_Result #dgvICD tbody').last().append($row);
            });
        }
        else {
            $('#dgvICD').DataTable({
                "language": {
                    "emptyTable": "No ICD Found"
                }
                , "bLengthChange": false, "autoWidth": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#dgvICD'))
            ;
        else
            $('#' + Admin_ICD.params["PanelID"] + ' #pnlICD_Result #dgvICD').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },


    FillICDCode: function (ICDId, ICDCode, ICDDescription, ICD10Code, ICD10Description, SNOMEDCode, SNOMEDDescription, LexiCode) {
        var RefCtrl = " #txtICDCode";
        var RefHiddenIdCtrl = " #hficdcode";
        var RefHiddenCtrl = " hfICDCode";
        if (Admin_ICD.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_ICD.params["RefCtrl"];
        }
        if (Admin_ICD.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_ICD.params["RefHiddenIdCtrl"];
        }
        if (Admin_ICD.params["RefHiddenCtrl"] != null) {
            RefHiddenCtrl = Admin_ICD.params["RefHiddenCtrl"];
        }
        $('#' + Admin_ICD.params["PanelID"] + RefCtrl).val(ICD10Code);
        $('#' + Admin_ICD.params["PanelID"] + RefHiddenIdCtrl).val(ICDId);
        //
        var RefHiddenCtrlArray = [];
        RefHiddenCtrlArray = RefHiddenCtrl.split(",");
        if (RefHiddenCtrlArray.length > 1) {
            $('#' + Admin_ICD.params["PanelID"] + " #" + RefHiddenCtrlArray[0]).val(ICDCode);
            $('#' + Admin_ICD.params["PanelID"] + " #" + RefHiddenCtrlArray[1]).val(ICDDescription);
            $('#' + Admin_ICD.params["PanelID"] + " #" + RefHiddenCtrlArray[2]).val(ICD10Code);
            $('#' + Admin_ICD.params["PanelID"] + " #" + RefHiddenCtrlArray[3]).val(ICD10Description);
            $('#' + Admin_ICD.params["PanelID"] + " #" + RefHiddenCtrlArray[4]).val(SNOMEDCode);
            $('#' + Admin_ICD.params["PanelID"] + " #" + RefHiddenCtrlArray[5]).val(SNOMEDDescription);
        }
        else {
            if (Admin_ICD.params.ParentCtrl == "SupperBillDetail") {
                $("#SupperBillDetail #txtICD_" + Admin_ICD.params["RefId"]).val(ICDCode);
                $("#SupperBillDetail #txtICDDescription_" + Admin_ICD.params["RefId"]).val(ICDDescription);
                $("#SupperBillDetail #txtICD10_" + Admin_ICD.params["RefId"]).val(ICD10Code);
                $("#SupperBillDetail #hfICD10Description_" + Admin_ICD.params["RefId"]).val(ICD10Description);
                $("#SupperBillDetail #hfSNOMED_" + Admin_ICD.params["RefId"]).val(SNOMEDCode);
                $("#SupperBillDetail #hfSNOMEDDescription_" + Admin_ICD.params["RefId"]).val(SNOMEDDescription);
                $("#SupperBillDetail #hfLexiCode_" + Admin_ICD.params["RefId"]).val(LexiCode);
            }
            else if (Admin_ICD.params.ParentCtrl == "mstrTabReports") {
                $('#' + Admin_ICD.params["PanelID"] + RefCtrl).val(ICDCode);
                $('#' + Admin_ICD.params["PanelID"] + " #"+RefHiddenCtrl).val(ICDCode);
            }
        }


        UnloadActionPan(Admin_ICD.params["ParentCtrl"], "Admin_ICD");

        $('#' + Admin_ICD.params["PanelID"] + RefCtrl).focus();
    },

    SearchICD: function (ICDData, ICDId, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "ICDData=" + ICDData + "&ICDID=" + ICDId + "&PageNo=" + PageNo + "&rpp=" + rpp;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ICD", "SEARCH_ICD");
    },

    ValidateICDCode: function (ICDCode, EntityId) {

        return Admin_ICD.LoadICDCode(ICDCode, EntityId).done(function (response) {

            if (response.length == 0) {
                utility.DisplayMessages("Invalid ICD Code.", 3);
            }
        });
    },
    LoadICDCode: function (ICDCode, EntityId) {
        var data = "icdCode=" + ICDCode + "&entityId=" + EntityId;

        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "COMMON_CODE", "CHECK_ICD_CODE");
    },

    DeleteICD: function (ICDId) {
        var data = "ICDID=" + ICDId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ICD_DETAIL", "DELETE_ICD");
    },

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#" + Admin_ICD.params["PanelID"] + " li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Admin_ICD.ICDSearch(null, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Admin_ICD.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_ICD.ICDSearch(null, currentPageNo, 15);
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Admin_ICD.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_ICD.ICDSearch(null, currentPageNo, 15);
        }
    },

    UnLoadTab: function (Tab) {

        if (Admin_ICD.params["FromAdmin"] == "0") {
            if (Admin_ICD.params != null && Admin_ICD.params.ParentCtrl != null) {
                UnloadActionPan(Admin_ICD.params.ParentCtrl, 'Admin_ICD');
            }
            else
                UnloadActionPan(null, 'Admin_ICD');

        }
        else {
            RemoveAdminTab();
        }
    }
}
