Admin_Specialty = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_Specialty.params = params;

        if (Admin_Specialty.params["FromAdmin"] == "0" && Admin_Specialty.params["PanelID"] == 'pnlAdminSpecialty')
            Admin_Specialty.params["FromAdmin"] = "1";

        if (Admin_Specialty.bIsFirstLoad) {
            Admin_Specialty.bIsFirstLoad = false;
        }
        var self;
        if (Admin_Specialty.params["PanelID"] != 'pnlAdminSpecialty') {
            self = $('#' + Admin_Specialty.params["PanelID"] + ' #pnlAdminSpecialty');
        } else {
            self = $('#pnlAdminSpecialty');

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
            Admin_Specialty.SpecialtySearch();
        });
    },

    SpecialtyAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Specialty", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["SpecialtyId"] = null;
                params["mode"] = "Add";
                params["ParentCtrl"] = "Admin_Specialty";

                if (Admin_Specialty.params["ParentCtrl"] && Admin_Specialty.params["ParentCtrl"].indexOf('Referrals') >= 0)
                    params["IsFromReferrals"] = true;
                else
                    params["IsFromReferrals"] = false;

                LoadActionPan('specialtyDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SpecialtyEdit: function (SpecialtyId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvSpecialty_row' + SpecialtyId));
        AppPrivileges.GetFormPrivileges("Specialty", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = SpecialtyId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["SpecialtyId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["ParentCtrl"] = "Admin_Specialty";

                    if (Admin_Specialty.params["ParentCtrl"] && Admin_Specialty.params["ParentCtrl"].indexOf('Referrals') >= 0)
                        params["IsFromReferrals"] = true;
                    else
                        params["IsFromReferrals"] = false;

                    LoadActionPan('specialtyDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SpecialtyDelete: function (SpecialtyId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvSpecialty_row' + SpecialtyId));
        AppPrivileges.GetFormPrivileges("Specialty", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = SpecialtyId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_Specialty.DeleteSpecialty(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvSpecialty').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                CacheManager.BindCodes('GetSpecialty', true);
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

    SpecialtyActiveInactive: function (SpecialtyId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Specialty", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = SpecialtyId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        specialtyDetail.UpdateSpecialtyActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_Specialty.SpecialtySearch('0');
                                CacheManager.BindCodes('GetSpecialty', true);
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

    SpecialtySearch: function (SpecialtyId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Specialty", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminSpecialty #pnlSpecialty_Result").css("display") == "none") {
                    $("#pnlAdminSpecialty #pnlSpecialty_Result").show();
                }

                var self = $("#pnlSpecialty_Search");
                var myJSON = self.getMyJSON();

                if (Admin_Specialty.params.ParentCtrl && Admin_Specialty.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail") {
                    $('#divSpecialty_Entity').hide()
                    myJSON = JSON.parse(myJSON);
                    delete myJSON.ddlEntity;
                    myJSON = JSON.stringify(myJSON);
                }

                Admin_Specialty.SearchSpecialty(myJSON, SpecialtyId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_Specialty.SpecialtyGridLoad(response);    //this will append table data in table body and create datatables instance
                        var TableControl = "pnlAdminSpecialty #dgvSpecialty"; //Table ID
                        var PagingPanelControlID = "pnlAdminSpecialty #divSpecialtyPaging"; //Table Pagination ID
                        var ClassControlName = "Admin_Specialty";  //Javascipt Class Name for this form
                        var PagesToDisplay = 5; //Number of pages you need to display
                        var iTotalDisplayRecords = response.iTotalDisplayRecords; //Total number of records to display (Count)
                        //Setting Time out so that datatables instance is fully created.
                        setTimeout(
                            CreatePagination(response.SpecialtyCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                            //Anonymous  function is for Pagination Call Backs
                            function (PrimaryID, PageNumber, ResultPerPage) {
                                Admin_Specialty.SpecialtySearch(PrimaryID, PageNumber, ResultPerPage);
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

    SpecialtyGridLoad: function (response) {
        $("#dgvSpecialty").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#pnlSpecialty_Result #dgvSpecialty tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.SpecialtyCount > 0) {
            var SpecialtyLoadJSONData = JSON.parse(response.SpecialtyLoad_JSON); //Parsing array to JSON
            $.each(SpecialtyLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_Specialty.SpecialtyEdit('" + item.SpecialtyId + "',event);");
                $row.attr("id", "gvSpecialty_row" + item.SpecialtyId);
                $row.attr("SpecialtyId", item.SpecialtyId);

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
                if (Admin_Specialty.params["FromAdmin"] == "0") {
                    if (Admin_Specialty.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail" || Admin_Specialty.params.ParentCtrl == "OrderSet_Patient_Referrals_Outgoing_Detail") {
                        var selectMethod = "Admin_Specialty.FillSpecialtyName('" + item.SpecialtyId + "','" + item.Description + "',event);"
                    } else {
                        var selectMethod = "Admin_Specialty.FillSpecialtyName('" + item.SpecialtyId + "','" + item.ShortName + "',event);"
                    }
                    //var selectMethod = "Admin_Specialty.FillSpecialtyName('" + item.SpecialtyId + "','" + item.ShortName + "',event);"
                    selectFacility = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';

                    if (ClassDisabled != "disabled")
                        $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_Specialty.SpecialtyEdit('" + item.SpecialtyId + "',event);");
                }

                $row.append('<td style="display:none;">' + item.SpecialtyId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_Specialty.SpecialtyDelete(\'' + item.SpecialtyId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Specialty.SpecialtyEdit(\'' + item.SpecialtyId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Specialty.SpecialtyActiveInactive(\'' + item.SpecialtyId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectFacility + '</td><td>' + item.ShortName + '</td><td>' + item.EntityName + '</td><td>' + item.Description + '</td>');

                $("#pnlSpecialty_Result #dgvSpecialty tbody").last().append($row);
            });
        }
        else {
            $('#pnlSpecialty_Result #dgvSpecialty').DataTable({
                "language": {
                    "emptyTable": "No Specialty Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Creating Data Table Instance
        if ($.fn.dataTable.isDataTable('#pnlSpecialty_Result #dgvSpecialty'))
            ;
        else {
            $("#pnlSpecialty_Result #dgvSpecialty").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            //$("#pnlSpecialty_Result #dgvSpecialty").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

        }
    },

    SearchSpecialty: function (SpecialtyData, SpecialtyId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "SpecialtyData=" + SpecialtyData + "&SpecialtyID=" + SpecialtyId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // search parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SPECIALTY", "SEARCH_SPECIALTY");
    },

    DeleteSpecialty: function (SpecialtyId) {
        var data = "SpecialtyID=" + SpecialtyId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SPECIALTY_DETAIL", "DELETE_SPECIALTY");
    },

    //UnLoadTab: function (Tab) {
    //    RemoveAdminTab(Tab);
    //},

    UnLoadTab: function (Tab) {
        if (Admin_Specialty.params["FromAdmin"] == "0") {


            if (Admin_Specialty.params != null && Admin_Specialty.params.ParentCtrl != null && Admin_Specialty.params.PanelID != 'pnlAdminSpecialty') {
                UnloadActionPan(Admin_Specialty.params.ParentCtrl, 'Admin_Specialty', null, Admin_Specialty.params.PanelID);
            }

            else if (Admin_Specialty.params != null && Admin_Specialty.params.ParentCtrl != null) {
                UnloadActionPan(Admin_Specialty.params.ParentCtrl, 'Admin_Specialty');
            }

            else
                UnloadActionPan(null, 'Admin_Specialty');
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
        if (Admin_Specialty.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_Specialty.params["RefCtrl"];
        }
        if (Admin_Specialty.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_Specialty.params["RefHiddenIdCtrl"];
        }

        //$('#' + Admin_Specialty.params["PanelID"] + RefCtrl).val(FacilityName).focus();
        $('#' + Admin_Specialty.params["PanelID"] + RefHiddenIdCtrl).val(SpecialtyId);
        $('#' + Admin_Specialty.params["PanelID"] + RefCtrl).val(SpecialtyName);
        if ($('#' + Admin_Specialty.params["PanelID"] + RefCtrl).data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#' + Admin_Specialty.params["PanelID"] + RefCtrl), SpecialtyName, $('#' + Admin_Specialty.params["PanelID"] + RefHiddenIdCtrl), SpecialtyId);
        else
            utility.SetAutoCompleteSource($('#' + Admin_Specialty.params["PanelID"] + RefCtrl), $('#' + Admin_Specialty.params["PanelID"] + RefHiddenIdCtrl));

        if (Admin_Specialty.params != null && Admin_Specialty.params.ParentCtrl != null && Admin_Specialty.params.PanelID != 'pnlAdminSpecialty') {
            if (Admin_Specialty.params.ParentCtrl == "ERADetail") {
                UnloadActionPan(Admin_Specialty.params.ParentCtrl, 'Admin_Specialty')
            }
            else {
                UnloadActionPan(Admin_Specialty.params.ParentCtrl, 'Admin_Specialty', null, Admin_Specialty.params.PanelID);
            }
        }
        else
            UnloadActionPan(Admin_Specialty.params["ParentCtrl"], "Admin_Specialty");

        $('#' + Admin_Specialty.params["PanelID"] + RefCtrl).focus();
    },

}