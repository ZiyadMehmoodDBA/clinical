
Admin_Resources = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_Resources.params = params;

        if (Admin_Resources.params["FromAdmin"] == "0" && Admin_Resources.params["PanelID"] == 'pnlAdminResources')
            Admin_Resources.params["FromAdmin"] = "1";

        if (Admin_Resources.bIsFirstLoad) {
            Admin_Resources.bIsFirstLoad = false;


            var self = "";
            if (Admin_Resources.params["PanelID"] != "pnlAdminResources")
                self = $('#' + Admin_Resources.params["PanelID"] + ' #pnlAdminResources')
            else
                self = $('#pnlAdminResources');

            self.loadDropDowns(true).done(function () {



            });

            $('#' + Admin_Resources.params["PanelID"] + ' #pnlResources_Result #ColumnAction').addClass("size95");

            if (Admin_Resources.params != null) {
                if (Admin_Resources.params.ParentCtrl != null) {
                    if (Admin_Resources.params.ParentCtrl == "Scheduling_Search") {
                        $('#' + Admin_Resources.params["PanelID"] + ' #pnlResources_Result #ColumnAction').removeClass("size95").addClass("size130");
                    }
                }
            }

            AppPrivileges.GetFormPrivileges("Resources", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_Resources.ResourcesSearch();
                }
            });
        }
    },

    ResourcesAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Resources", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ResourceId"] = null;
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_Resources.params["FromAdmin"];
                if (Admin_Resources.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_Resources';
                }
                LoadActionPan('resourcesDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ResourcesEdit: function (ResourceId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#" + Admin_Resources.params["PanelID"] + ' #gvResources_row' + ResourceId));
        AppPrivileges.GetFormPrivileges("Resources", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ResourceId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ResourceId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Admin_Resources.params["FromAdmin"];
                    if (Admin_Resources.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Admin_Resources';
                    }
                    LoadActionPan('resourcesDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ResourcesDelete: function (ResourceId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        utility.SelectGridRow($("#" + Admin_Resources.params["PanelID"] + ' #gvResources_row' + ResourceId));
        AppPrivileges.GetFormPrivileges("Resources", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ResourceId;
                    var oTable = $('#dgvResources').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_Resources.DeleteResources(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvResources').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                CacheManager.BindCodes('GetResources', true);
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

    ResourcesActiveInactive: function (ResourceId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Resources", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ResourceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        resourcesDetail.UpdateResourceActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_Resources.ResourcesSearch('0');
                                CacheManager.BindCodes('GetResources', true);
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

    ResourcesSearch: function (ResourceId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Resources", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Admin_Resources.params["PanelID"] + ' #pnlResources_Result').css("display") == "none") {
                    $('#' + Admin_Resources.params["PanelID"] + ' #pnlResources_Result').show();
                }

                var self = $('#' + Admin_Resources.params["PanelID"] + ' #pnlResources_Search');
                var myJSON = self.getMyJSON();

                Admin_Resources.SearchResources(myJSON, ResourceId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_Resources.ResourcesGridLoad(response);
                        var TableControl = Admin_Resources.params["PanelID"] + " #dgvResources";
                        var PagingPanelControlID = Admin_Resources.params["PanelID"] + " #divResourcePaging";
                        var ClassControlName = "Admin_Resources";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ResourcesCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_Resources.ResourcesSearch(PrimaryID, PageNumber, ResultPerPage);
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

    ResourcesGridLoad: function (response) {
        $('#' + Admin_Resources.params["PanelID"] + ' #dgvResources').dataTable().fnDestroy();
        $('#' + Admin_Resources.params["PanelID"] + ' #pnlResources_Result #dgvResources tbody').find("tr").remove();
        if (response.ResourcesCount > 0) {
            var ResourcesLoadJSONData = JSON.parse(response.ResourcesLoad_JSON);
            $.each(ResourcesLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvResources_row" + item.ResourceId);
                $row.attr("ResourceId", item.ResourceId);
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
                var selectResources = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                if (Admin_Resources.params["FromAdmin"] == "0") {
                    var selectMethod = "Admin_Resources.FillResources('" + item.ResourceId + "','" + item.ShortName + "','" + item.EntityName + "',event);"
                    selectResources = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_Resources.ResourcesEdit('" + item.ResourceId + "',event);");
                }
                $row.append('<td style="display:none;">' + item.ResourceId + '</td><td><a class="btn  btn-xs" href="#"onclick="Admin_Resources.ResourcesDelete(\'' + item.ResourceId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Resources.ResourcesEdit(\'' + item.ResourceId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Admin_Resources.ResourcesActiveInactive(\'' + item.ResourceId + '\',' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectResources + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.FacilityName + '</td><td>' + item.Duration + '</td>');

                $('#' + Admin_Resources.params["PanelID"] + ' #pnlResources_Result #dgvResources tbody').last().append($row);
            });
        }
        else {
            $('#' + Admin_Resources.params["PanelID"] + ' #pnlResources_Result #dgvResources').DataTable({
                "language": {
                    "emptyTable": "No Resource found"
                },
                "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_Resources.params["PanelID"] + ' #pnlResources_Result #dgvResources'))
            ;
        else
            $('#' + Admin_Resources.params["PanelID"] + ' #pnlResources_Result #dgvResources').DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$('#' + Admin_Resources.params["PanelID"] + ' #pnlResources_Result #dgvResources').DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page 

    },

    FillResources: function (ResourceId, ShortName, EntityName, event) {

        if (event != null) {
            event.stopPropagation();
        }

        if (Admin_Provider.params["PanelID"] == "blockHoursDetail") {
            if (globalAppdata.AppUserName == "MDVISION")
                $('#' + Admin_Resources.params["PanelID"] + ' #txtResource').val(ShortName + ' - ' + EntityName);
            else
                $('#' + Admin_Resources.params["PanelID"] + ' #txtResource').val(ShortName);
        }
        else {
            $('#' + Admin_Resources.params["PanelID"] + ' #txtResource').val(ShortName);
        }
        $('#' + Admin_Resources.params["PanelID"] + ' #hfResource').val(ResourceId);


        //--------------

        var RefCtrl = " #txtResource";
        var RefCtrlHidden = " #hfResource";
        var RefCtrlLabel = " #lblResource";
        var RefCtrlLink = " #lnkResourceEdit";
        if (Admin_Resources.params["RefCtrl"] != null)
            RefCtrl = " #" + Admin_Resources.params["RefCtrl"];
        if (Admin_Resources.params["RefCtrlHidden"] != null)
            RefCtrlHidden = " #" + Admin_Resources.params["RefCtrlHidden"];
        if (Admin_Resources.params["RefCtrlLabel"] != null)
            RefCtrlLabel = " #" + Admin_Resources.params["RefCtrlLabel"];
        if (Admin_Resources.params["RefCtrlLink"] != null)
            RefCtrlLink = " #" + Admin_Resources.params["RefCtrlLink"];

        $('#' + Admin_Resources.params["PanelID"] + RefCtrlLabel).css("display", "none");
        $('#' + Admin_Resources.params["PanelID"] + RefCtrlLink).css("display", "inline");

        $('#' + Admin_Resources.params["PanelID"] + RefCtrl).focus();
        //--------------

        if (Admin_Resources.params["IsOptional"] != null && Admin_Resources.params["RefForm"] != null && Admin_Resources.params["IsOptional"] == false) {
            //if ($('#' + Admin_Resources.params["PanelID"] + ' #' + Admin_Resources.params["RefForm"]).data('bootstrapValidator') != null && typeof $('#' + Admin_Resources.params["PanelID"] + ' #' + Admin_Resources.params["RefForm"]).data('bootstrapValidator') != 'undefined') {
            $('#' + Admin_Resources.params["PanelID"] + ' #' + Admin_Resources.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_Resources.params["PanelID"] + RefCtrl).attr("name"));
            //}
        }

        UnloadActionPan(Admin_Resources.params["ParentCtrl"], "Admin_Resources");
    },

    SearchResources: function (ResourcesData, resourceID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ResourcesData=" + ResourcesData + "&resourceID=" + resourceID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCES", "SEARCH_RESOURCES");
    },

    DeleteResources: function (resourceID) {
        var data = "ResourceID=" + resourceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCES_DETAIL", "DELETE_RESOURCES");
    },

    UnLoadTab: function () {
        if (Admin_Resources.params["FromAdmin"] == "0") {
            if (Admin_Resources.params != null && Admin_Resources.params.ParentCtrl != null) {
                UnloadActionPan(Admin_Resources.params.ParentCtrl, 'Admin_Resources');
            }
            else
                UnloadActionPan(null, 'Admin_Resources');

        }
        else {
            RemoveAdminTab();
        }
    },

    checkSubmit: function (e) {
        if (e && e.keyCode == 13) {
            Admin_Resources.ResourcesSearch('0');
        }
    },
}

