Admin_Drug_Code_Cost = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_Drug_Code_Cost.params = params;

        if (Admin_Drug_Code_Cost.params["FromAdmin"] == "0" && Admin_Drug_Code_Cost.params["PanelID"] == 'pnlAdminSpecialty')
            Admin_Drug_Code_Cost.params["FromAdmin"] = "1";

        if (Admin_Drug_Code_Cost.bIsFirstLoad) {
            Admin_Drug_Code_Cost.bIsFirstLoad = false;
        }
        var self;
        if (Admin_Drug_Code_Cost.params["PanelID"] != 'pnlAdminDrugCodeCost') {
            self = $('#' + Admin_Drug_Code_Cost.params["PanelID"] + ' #pnlAdminDrugCodeCost');
        } else {
            self = $('#pnlAdminDrugCodeCost');

        }


        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find('#ddlEntity').attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {
            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#pnlSpecialty_Search #divSpecialty_Entity").css("display", "none");
            //    $("#pnlSpecialty_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find('#ddlEntity').val(globalAppdata["SeletedEntityId"]);
            }
            Admin_Drug_Code_Cost.DrugCodeCostSearch();
        });
    },

    DrugCodeCostAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("DrugCodeCost", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["DrugCodeCostId"] = null;
                params["mode"] = "Add";
                params["ParentCtrl"] = "Admin_Drug_Code_Cost";
                // unwanted
                //if (Admin_Drug_Code_Cost.params["ParentCtrl"] && Admin_Drug_Code_Cost.params["ParentCtrl"].indexOf('Referrals') >= 0)
                //    params["IsFromReferrals"] = true;
                //else
                //    params["IsFromReferrals"] = false;

                LoadActionPan('Admin_DrugCodeCost_Detail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DrugCodeCostEdit: function (CPTCodeCostID, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvCPTCodeCost' + CPTCodeCostID));
        AppPrivileges.GetFormPrivileges("DrugCodeCost", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = CPTCodeCostID;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["CPTCodeCostID"] = selectedValue;
                    params["mode"] = "Edit";
                    params["ParentCtrl"] = "Admin_Drug_Code_Cost";
                    // unwanted
                    //if (Admin_Drug_Code_Cost.params["ParentCtrl"] &&  Admin_Drug_Code_Cost.params["ParentCtrl"].indexOf('Referrals') >= 0)
                    //    params["IsFromReferrals"] = true;
                    //else
                    //    params["IsFromReferrals"] = false;

                    LoadActionPan('Admin_DrugCodeCost_Detail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DrugCodeCostDelete: function (DrugCodeCostId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvCPTCodeCost' + DrugCodeCostId));
        AppPrivileges.GetFormPrivileges("Specialty", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = DrugCodeCostId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_Drug_Code_Cost.DeleteDrugCodeCost(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvDrugCodeCost').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                //CacheManager.BindCodes('GetSpecialty', true);
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

    GrugCodeCostActiveInactive: function (DrugCodeCostId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("DrugCodeCost", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = DrugCodeCostId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_DrugCodeCost_Detail.UpdateDrugCodeCostActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_Drug_Code_Cost.DrugCodeCostSearch('0');
                               // CacheManager.BindCodes('GetSpecialty', true);
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

    DrugCodeCostSearch: function (SpecialtyId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("DrugCodeCost", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminDrugCodeCost #pnlDrugCodeCost_Result").css("display") == "none") {
                    $("#pnlAdminDrugCodeCost #pnlDrugCodeCost_Result").show(); 
                }

                var self = $("#pnlDrugCodeCost_Search");
                var myJSON = self.getMyJSON();

                if (Admin_Drug_Code_Cost.params.ParentCtrl && Admin_Drug_Code_Cost.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail") {
                    $('#divSpecialty_Entity').hide()
                    myJSON = JSON.parse(myJSON);
                    delete myJSON.ddlEntity;
                    myJSON = JSON.stringify(myJSON);
                }

                Admin_Drug_Code_Cost.SearchDrugCodeCost(myJSON, SpecialtyId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_Drug_Code_Cost.DrugCodeCostGridLoad(response);    //this will append table data in table body and create datatables instance
                        var TableControl = "pnlAdminDrugCodeCost #dgvDrugCodeCost"; //Table ID
                        var PagingPanelControlID = "pnlAdminDrugCodeCost #divDurgCodeCostPaging"; //Table Pagination ID
                        var ClassControlName = "Admin_Drug_Code_Cost";  //Javascipt Class Name for this form
                        var PagesToDisplay = 5; //Number of pages you need to display
                        var iTotalDisplayRecords = response.iTotalDisplayRecords; //Total number of records to display (Count)
                        //Setting Time out so that datatables instance is fully created.
                        setTimeout(
                            CreatePagination(response.DrugCodeCostCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                            //Anonymous  function is for Pagination Call Backs
                            function (PrimaryID, PageNumber, ResultPerPage) {
                                Admin_Drug_Code_Cost.DrugCodeCostSearch(PrimaryID, PageNumber, ResultPerPage);
                            }),
                        10);
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

    DrugCodeCostGridLoad: function (response) {
        $("#dgvDrugCodeCost").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#pnlDrugCodeCost_Result #dgvDrugCodeCost tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.DrugCodeCostCount > 0) {
            var DrugCodeCostCountLoad_JSON = JSON.parse(response.DrugCodeCostCountLoad_JSON); //Parsing array to JSON
            $.each(DrugCodeCostCountLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_Drug_Code_Cost.DrugCodeCostEdit('" + item.CPTCodeCostID + "',event);");
                $row.attr("id", "gvCPTCodeCost" + item.CPTCodeCostID);
                $row.attr("CPTCodeCostID", item.CPTCodeCostID);

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

                var selectCPTCodeCost = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                if (Admin_Drug_Code_Cost.params["FromAdmin"] == "0") {
                    if (Admin_Drug_Code_Cost.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail" || Admin_Drug_Code_Cost.params.ParentCtrl == "OrderSet_Patient_Referrals_Outgoing_Detail") {
                        var selectMethod = "Admin_Drug_Code_Cost.FillSpecialtyName('" + item.CPTCodeCostID + "','" + item.Cost + "',event);"
                    } else {
                        var selectMethod = "Admin_Drug_Code_Cost.FillSpecialtyName('" + item.CPTCodeCostID + "','" + item.CPTCode + "',event);"
                    }
                    //var selectMethod = "Admin_Drug_Code_Cost.FillSpecialtyName('" + item.SpecialtyId + "','" + item.ShortName + "',event);"
                    selectFacility = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';

                    if (ClassDisabled != "disabled")
                        $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_Drug_Code_Cost.DrugCodeCostEdit('" + item.CPTCodeCostID + "',event);");
                }
                $row.attr("onclick", "Admin_Drug_Code_Cost.DrugCodeCostEdit('" + item.CPTCodeCostID + "',event);");
                $row.append('<td style="display:none;">' + item.CPTCodeCostID + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_Drug_Code_Cost.DrugCodeCostDelete(\'' + item.CPTCodeCostID + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Drug_Code_Cost.DrugCodeCostEdit(\'' + item.CPTCodeCostID + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Drug_Code_Cost.GrugCodeCostActiveInactive(\'' + item.CPTCodeCostID + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.CPTCode + '</td><td>' + item.Cost + '</td>');

                $("#pnlDrugCodeCost_Result #dgvDrugCodeCost tbody").last().append($row);
            });
        }
        else {
            $('#pnlDrugCodeCost_Result #dgvDrugCodeCost').DataTable({
                "language": {
                    "emptyTable": "No Drug Code Cost Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Creating Data Table Instance
        if ($.fn.dataTable.isDataTable('#pnlDrugCodeCost_Result #dgvDrugCodeCost'))
            ;
        else {
            $("#pnlDrugCodeCost_Result #dgvDrugCodeCost").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            //$("#pnlSpecialty_Result #dgvSpecialty").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

        }
    },

    SearchDrugCodeCost: function (DrugCodeCostData, DrugCodeCostId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "DrugCodeCostData=" + DrugCodeCostData + "&DrugCodeCostID=" + DrugCodeCostId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // search parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_DRUG_CODE_COST", "SEARCH_DRUG_CODE_COST");
    },

    DeleteDrugCodeCost: function (DrugCodeCostId) {
        var data = "DrugCodeCostID=" + DrugCodeCostId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_DRUGCODECOST_DETAIL", "DELETE_DRUGCODECOST");
    },

    BindAutoComplete: function (element) {

        var entityId = $('#pnlAdminDrugCodeCost #ddlBasicFeeGroup option:selected').attr('refvalue');
        var hiddenCrtl = $("#DrugCodeCostDetail #hfCPTCode");
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, entityId, true, -1, "CPT", true, "pnlAdminDrugCodeCost", null, true);

        

    },
    DrugCodeCostValidateCode: function (obj) {

        //var entityId = $('#basicFeeScheduleDetail #ddlBasicFeeGroup option:selected').attr('refvalue');
        //utility.ValidateCode(obj, 'CPT', 'basicFeeScheduleDetail #hfCPTCode', entityId);
        utility.ValidateAutoComplete(obj, 'pnlAdminDrugCodeCost #hfCPTCode', true, 1);
    },

    UnLoadTab: function (Tab) {
        if (Admin_Drug_Code_Cost.params["FromAdmin"] == "0") {


            if (Admin_Drug_Code_Cost.params != null && Admin_Drug_Code_Cost.params.ParentCtrl != null && Admin_Drug_Code_Cost.params.PanelID != 'pnlAdminSpecialty') {
                UnloadActionPan(Admin_Drug_Code_Cost.params.ParentCtrl, 'Admin_Drug_Code_Cost', null, Admin_Drug_Code_Cost.params.PanelID);
            }

            else if (Admin_Drug_Code_Cost.params != null && Admin_Drug_Code_Cost.params.ParentCtrl != null) {
                UnloadActionPan(Admin_Drug_Code_Cost.params.ParentCtrl, 'Admin_Drug_Code_Cost');
            }

            else
                UnloadActionPan(null, 'Admin_Drug_Code_Cost');
        }
        else {
            RemoveAdminTab();
        }
    },

    FillSpecialtyName: function (SpecialtyId, SpecialtyName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = " #txtFacility";
        var RefHiddenIdCtrl = " #hfFacility";
        if (Admin_Drug_Code_Cost.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_Drug_Code_Cost.params["RefCtrl"];
        }
        if (Admin_Drug_Code_Cost.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_Drug_Code_Cost.params["RefHiddenIdCtrl"];
        }

        //$('#' + Admin_Drug_Code_Cost.params["PanelID"] + RefCtrl).val(FacilityName).focus();
        $('#' + Admin_Drug_Code_Cost.params["PanelID"] + RefHiddenIdCtrl).val(SpecialtyId);
        $('#' + Admin_Drug_Code_Cost.params["PanelID"] + RefCtrl).val(SpecialtyName).focus();

        if (Admin_Drug_Code_Cost.params != null && Admin_Drug_Code_Cost.params.ParentCtrl != null && Admin_Drug_Code_Cost.params.PanelID != 'pnlAdminDrugCodeCost') {
            if (Admin_Drug_Code_Cost.params.ParentCtrl == "ERADetail") {
                UnloadActionPan(Admin_Drug_Code_Cost.params.ParentCtrl, 'Admin_Drug_Code_Cost')
            }
            else {
                UnloadActionPan(Admin_Drug_Code_Cost.params.ParentCtrl, 'Admin_Drug_Code_Cost', null, Admin_Drug_Code_Cost.params.PanelID);
            }
        }
        else
            UnloadActionPan(Admin_Drug_Code_Cost.params["ParentCtrl"], "Admin_Drug_Code_Cost");

        $('#' + Admin_Drug_Code_Cost.params["PanelID"] + RefCtrl).focus();
    },

}