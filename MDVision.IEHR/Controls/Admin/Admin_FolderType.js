
Admin_FolderType = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Admin_FolderType.params = params;

        //if ( Admin_FolderType.params["PanelID"] != 'pnlAdminFolderType')
        //    Admin_FolderType.params["PanelID"] = Admin_FolderType.params["PanelID"] + ' #pnlAdminFolderType';


        if (Admin_FolderType.bIsFirstLoad) {
            Admin_FolderType.bIsFirstLoad = false;


            //if (typeof Admin_FolderType.params["PanelID"] != 'pnlAdminFolderType')
            //    self = $('#' + Admin_FolderType.params["PanelID"] + ' #pnlAdminFolderType');
            //else
            //    self = $('#pnlAdminFolderType');
            var self = null;
            if (Admin_FolderType.params.PanelID == "pnlAdminFolderType")
                self = $('#pnlAdminFolderType');
            else
                self = $('#' + Admin_FolderType.params.PanelID + ' #pnlAdminFolderType');

            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }

            self.loadDropDowns(true).done(function () {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_FolderType.FolderTypeSearch();

            });
        }
    },

    FolderTypeAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("FolderType", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FolderTypeId"] = null;
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_FolderType.params["FromAdmin"];
                //if (Admin_FolderType.params["FromAdmin"] == "0") {
                //    params["ParentCtrl"] = 'Admin_FolderType';
                //}
                LoadActionPan('folderTypeDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FolderTypeEdit: function (DoctypeId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvAdminFolderType_row' + DoctypeId));
        AppPrivileges.GetFormPrivileges("FolderType", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = DoctypeId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["FolderTypeId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Admin_FolderType.params["FromAdmin"];
                    //if (Admin_FolderType.params["FromAdmin"] == "0") {
                    //    params["ParentCtrl"] = 'Admin_FolderType';
                    //}
                    LoadActionPan('folderTypeDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FolderTypeDelete: function (DoctypeId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvAdminFolderType_row' + DoctypeId));
        AppPrivileges.GetFormPrivileges("FolderType", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = DoctypeId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_FolderType.DeleteFolderType(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#' + Admin_FolderType.params["PanelID"] + ' #dgvAdminFolderType').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_FolderType.FolderTypeSearch();
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

    FolderTypeActiveInactive: function (DoctypeId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("FolderType", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = DoctypeId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        folderTypeDetail.UpdateFolderTypeActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_FolderType.FolderTypeSearch('0');
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

    FolderTypeSearch: function (DocTypeId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("FolderType", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_FolderType.params["PanelID"] + " #pnlAdminFolderType_Result").css("display") == "none") {
                    $("#" + Admin_FolderType.params["PanelID"] + " #pnlAdminFolderType_Result").show();
                }

                var self = $("#" + Admin_FolderType.params["PanelID"]);
                var myJSON = self.getMyJSON();

                // Admin_FolderType.FolderTypeGridLoad();
                Admin_FolderType.SearchFolderType(myJSON, DocTypeId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_FolderType.FolderTypeGridLoad(response);
                        var TableControl = Admin_FolderType.params["PanelID"] + " #dgvAdminFolderType";
                        var PagingPanelControlID = Admin_FolderType.params["PanelID"] + " #DivAdminFolderTypePaging";
                        var ClassControlName = "Admin_FolderType";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.DocumentTypeCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_FolderType.FolderTypeSearch(PrimaryID, PageNumber, ResultPerPage);
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

    FolderTypeGridLoad: function (response) {
        $("#" + Admin_FolderType.params["PanelID"] + " #dgvAdminFolderType").dataTable().fnDestroy();
        $("#" + Admin_FolderType.params["PanelID"] + " #pnlAdminFolderType_Result #dgvAdminFolderType tbody").find("tr").remove();
        if (response.DocumentTypeCount >= 1) {
            var FolderTypeLoadJSONData = JSON.parse(response.FolderTypeLoad_JSON);
            $.each(FolderTypeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_FolderType.FolderTypeEdit('" + item.DoctypeId + "',event);");
                $row.attr("id", "dgvAdminFolderType_row" + item.DoctypeId);
                $row.attr("DocTypeId", item.DoctypeId);
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
                $row.append('<td style="display:none;">' + item.DoctypeId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_FolderType.FolderTypeDelete(' + item.DoctypeId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_FolderType.FolderTypeEdit(' + item.DoctypeId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_FolderType.FolderTypeActiveInactive(' + item.DoctypeId + ',' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.EntityName + '</td>');

                $("#" + Admin_FolderType.params["PanelID"] + " #pnlAdminFolderType_Result #dgvAdminFolderType tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_FolderType.params["PanelID"] + ' #dgvAdminFolderType').DataTable({
                "language": {
                    "emptyTable": "No Folder Type Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_FolderType.params["PanelID"] + ' #dgvAdminFolderType'))
            ;
        else
            $("#" + Admin_FolderType.params["PanelID"] + " #pnlAdminFolderType_Result #dgvAdminFolderType").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchFolderType: function (FolderTypeData, DocTypeId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "FolderTypeData=" + FolderTypeData + "&DocTypeId=" + DocTypeId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER_TYPE", "SEARCH_FOLDER_TYPE");
    },

    DeleteFolderType: function (DocTypeId) {
        var data = "DocTypeId=" + DocTypeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER_TYPE", "DELETE_FOLDER_TYPE");
    },

    UnLoadTab: function () {
        if (Admin_FolderType.params["FromAdmin"] == "0") {
            if (Admin_FolderType.params != null && Admin_FolderType.params.ParentCtrl != null) {
                UnloadActionPan(Admin_FolderType.params.ParentCtrl, 'Admin_FolderType');
            }
            else
                UnloadActionPan(null, 'Admin_FolderType');
        }
        else {
            RemoveAdminTab();
        }
    },
}