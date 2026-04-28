
Admin_Folder = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Admin_Folder.params = params;

        //if ( Admin_Folder.params["PanelID"] != 'pnlAdminFolder')
        //    Admin_Folder.params["PanelID"] = Admin_Folder.params["PanelID"] + ' #pnlAdminFolder';


        if (Admin_Folder.bIsFirstLoad) {
            Admin_Folder.bIsFirstLoad = false;

            var self = $('#' + Admin_Folder.params["PanelID"]);
            //if (typeof Admin_Folder.params["PanelID"] != 'pnlAdminFolder')
            //    self = $('#' + Admin_Folder.params["PanelID"] + ' #pnlAdminFolder');
            //else
            //    self = $('#pnlAdminFolder');

            self.loadDropDowns(true);

            AppPrivileges.GetFormPrivileges("Folder", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_Folder.FolderSearch();
                }
            });
        }

    },

    FolderAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Folder", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FolderId"] = null;
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_Folder.params["FromAdmin"];
                //if (Admin_Folder.params["FromAdmin"] == "0") {
                //    params["ParentCtrl"] = 'Admin_Folder';
                //}
                LoadActionPan('folderDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FolderEdit: function (FolderId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvAdminFolder_row' + FolderId));
        AppPrivileges.GetFormPrivileges("Folder", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = FolderId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["FolderId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Admin_Folder.params["FromAdmin"];
                    //if (Admin_Folder.params["FromAdmin"] == "0") {
                    //    params["ParentCtrl"] = 'Admin_Folder';
                    //}
                    LoadActionPan('folderDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FolderDelete: function (FolderId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvAdminFolder_row' + FolderId));
        AppPrivileges.GetFormPrivileges("Folder", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = FolderId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_Folder.DeleteFolder(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#' + Admin_Folder.params["PanelID"] + ' #dgvAdminFolder').DataTable();
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

    FolderActiveInactive: function (FolderId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Folder", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = FolderId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_Folder.UpdateFolderActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_Folder.FolderSearch();
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

    FolderSearch: function (DocumentId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Folder", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_Folder.params["PanelID"] + " #pnlAdminFolder_Result").css("display") == "none") {
                    $("#" + Admin_Folder.params["PanelID"] + " #pnlAdminFolder_Result").show();
                }

                var self = $("#" + Admin_Folder.params["PanelID"]);
                var myJSON = self.getMyJSON();
                Admin_Folder.SearchFolder(myJSON, DocumentId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_Folder.DocumentGridLoad(response);
                        var TableControl = Admin_Folder.params["PanelID"] + " #dgvAdminFolder";
                        var PagingPanelControlID = Admin_Folder.params["PanelID"] + " #DivAdminFolderPaging";
                        var ClassControlName = "Admin_Folder";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.DocumentCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_Folder.FolderSearch(PrimaryID, PageNumber, ResultPerPage);
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

    DocumentGridLoad: function (response) {
        $("#" + Admin_Folder.params["PanelID"] + " #dgvAdminFolder").dataTable().fnDestroy();
        $("#" + Admin_Folder.params["PanelID"] + " #pnlAdminFolder_Result #dgvAdminFolder tbody").find("tr").remove();
        if (response.DocumentCount > 0) {
            var DocumentLoad_JSONData = JSON.parse(response.DocumentLoad_JSON);
            $.each(DocumentLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_Folder.FolderEdit('" + item.DocId + "',event);");
                $row.attr("id", "dgvAdminFolder_row" + item.DocId);
                $row.attr("FolderId", item.DocId);
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
                var selectDocument = "";
                var ActionAgainstFolderList = "";
                var FolderExists = Admin_Folder.SystemGeneratedFolderList(item.ShortName);
                if (FolderExists) {
                    ActionAgainstFolderList = '<td title="System generated, cannot modify!" data-toggle="tooltip"></td>';
                } else {
                    ActionAgainstFolderList = '<td><a class="btn  btn-xs" href="#" onclick="Admin_Folder.FolderDelete(' + item.DocId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Folder.FolderEdit(' + item.DocId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_Folder.FolderActiveInactive(' + item.DocId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' +selectDocument + '</td>';
                }

                $row.append('<td style="display:none;">' + item.DocId + '</td>' + ActionAgainstFolderList + '<td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.DocumentType + '</td><td>' + item.EntityName + '</td>');
                $("#" + Admin_Folder.params["PanelID"] + " #pnlAdminFolder_Result #dgvAdminFolder tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_Folder.params["PanelID"] + ' #dgvAdminFolder').DataTable({
                "language": {
                    "emptyTable": "No Folder Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#' + Admin_Folder.params["PanelID"] + ' #dgvAdminFolder'))
            ;
        else
            $("#" + Admin_Folder.params["PanelID"] + " #pnlAdminFolder_Result #dgvAdminFolder").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchFolder: function (FolderData, FolderId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "FolderData=" + FolderData + "&FolderID=" + FolderId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER", "SEARCH_FOLDER");
    },

    DeleteFolder: function (FolderID) {
        var data = "FolderID=" + FolderID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER", "DELETE_FOLDER");
    },

    UpdateFolderActiveInactive: function (FolderID, IsActive) {
        var data = "FolderID=" + FolderID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER", "UPDATE_FOLDER_ACTIVE_INACTIVE");
    },
  
    UnLoadTab: function () {
        if (Admin_Folder.params["FromAdmin"] == "0") {
            if (Admin_Folder.params != null && Admin_Folder.params.ParentCtrl != null) {
                UnloadActionPan(Admin_Folder.params.ParentCtrl, 'Admin_Folder');
            }
            else
                UnloadActionPan(null, 'Admin_Folder');
        }
        else {
            RemoveAdminTab();
        }
    },

    SystemGeneratedFolderList: function (FolderName) {
        var SystemGeneratedFolderList = ["Insurance Card", "Lab Orders", "Lab Order", "Lab Results", "Lab Result", "Patient ID Card", "Patient Letters", "Pat Education", "Progress Notes", "Phone Encounter", "Rad Ord Report", "Custom Form"]
        return (SystemGeneratedFolderList.indexOf(FolderName) > -1)
    },
}