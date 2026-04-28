ReviewOfSystemsDataTemplate = {

    //Author: ZeeshanAK
    //Date: 30-03-2016
    //This file will handle all actions performed for Review Of Systems Data Template
    bIsFirstLoad: true,
    params: [],


    Load: function (params) {
        ReviewOfSystemsDataTemplate.params = params;
        if (ReviewOfSystemsDataTemplate.params.PanelID != 'pnlReviewOfSystemsDataTemplate') {
            ReviewOfSystemsDataTemplate.params.PanelID = ReviewOfSystemsDataTemplate.params.PanelID + ' #pnlReviewOfSystemsDataTemplate';
        } else {
            ReviewOfSystemsDataTemplate.params.PanelID = 'pnlReviewOfSystemsDataTemplate';
        }

        ReviewOfSystemsDataTemplate.rosDataTemplateSearch();
        if (ReviewOfSystemsDataTemplate.bIsFirstLoad) {
            ReviewOfSystemsDataTemplate.bIsFirstLoad = false;
            //  ReviewOfSystemsDataTemplate.validateROSTemplate();
        }
    },

    ReviewOfSystemsDataTemplateAddEdit: function (ReviewOfSystemsTemplateId) {
        var strMessage = "";
        var mode = "";
        if (ReviewOfSystemsTemplateId != null && parseInt(ReviewOfSystemsTemplateId) > 0) {
            mode = "EDIT";
        } else {
            mode = "ADD";
        }
        AppPrivileges.GetFormPrivileges("Clinical_Data Template_Review of System", mode, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (ReviewOfSystemsTemplateId != null && parseInt(ReviewOfSystemsTemplateId) > 0) {
                    params["ReviewOfSystemsTemplateId"] = ReviewOfSystemsTemplateId;
                    params["mode"] = "Edit";
                }
                else {
                    params["ReviewOfSystemsTemplateId"] = -1;
                    params["mode"] = "Add";
                }
                params["ReviewOfSystemsTemplateId"] = ReviewOfSystemsTemplateId;
                params["FromAdmin"] = ReviewOfSystemsDataTemplate.params["FromAdmin"];
                params["ParentCtrl"] = 'adminTabReviewOfSystemsDataTemplate';
                LoadActionPan('ReviewOfSystemsDataTemplateDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    /*
    Author: Muhammad Azhar Shahzad
    Purpose: for Grid Load of Ros template
    Creation Date: March 02,2016 */
    rosDataTemplateSearch: function (ROSDataTemplateId, PageNo, rpp, ROSTemplateId) {

        ReviewOfSystemsDataTemplate.searchROSDataTemplate_DBCall(ROSDataTemplateId, ROSTemplateId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ReviewOfSystemsDataTemplate.rosDataTemplateGridLoad(response);
                //Adding Pagination on 04 Dec 2015 by Azhar
                var TableControl = ReviewOfSystemsDataTemplate.params.PanelID + " #dgvROSDataTemplate";
                var PagingPanelControlID = ReviewOfSystemsDataTemplate.params.PanelID + " #dgvROSDataTemplate_Paging";
                var ClassControlName = "ReviewOfSystemsDataTemplate";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ROSDataTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    ReviewOfSystemsDataTemplate.rosDataTemplateSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Ros template rows html
   Creation Date: March 02,2016 */
    rosDataTemplateGridLoad: function (response) {
        var isGridactive = $('#' + ReviewOfSystemsDataTemplate.params.PanelID + ' #pnlReviewOfSystemsDataTemplate_Result #divSwitch #switchActive').attr('isactive');
        if ($.fn.dataTable.isDataTable('#' + ReviewOfSystemsDataTemplate.params.PanelID + " #dgvROSDataTemplate")) {
            $('#' + ReviewOfSystemsDataTemplate.params.PanelID + " #dgvROSDataTemplate").dataTable().fnDestroy();
            $('#' + ReviewOfSystemsDataTemplate.params.PanelID + " #dgvROSDataTemplate tbody").find("tr").remove();
        }
        var checked = '';
        if (isGridactive == "0" || isGridactive == 0) {
        } else if (isGridactive == null) {
            isGridactive = "1";
            checked = 'checked="checked"';
        } else {
            isGridactive = "1";
            checked = 'checked="checked"';
        }
        if (response.ROSDataTemplateCount > 0) {
            var ROSDataTemplateLoad_JSON = JSON.parse(response.ROSDataTemplateLoad_JSON);
            $.each(ROSDataTemplateLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvROSDataTemplate_row" + item.ROSDataTemplateId + "')); ReviewOfSystemsDataTemplate.rosDataTemplateEditRow(" + item.ROSDataTemplateId + ", " + item.ROSTemplateId + ", " + item.ROSDataTempInfoId + ", event);");
                $row.attr("id", "gvROSDataTemplate_row" + item.ROSDataTemplateId);
                $row.attr("ROSTemplateId", item.ROSTemplateId);
                $row.attr("ROSDataTemplateId", item.ROSDataTemplateId);
                $row.attr("Active", item.IsActive);

                if (item.IsActive == "True") {
                    isactive = 1;
                    isEventactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 0;
                    isEventactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var Isdisabled = "";
                if (item.IsDefault == "True") {
                    Isdisabled = "disabled =true";
                }

                $row.append('<td>' +
                    '<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="ReviewOfSystemsDataTemplate.rosDataTemplateDelete(' + item.ROSDataTemplateId + "," + item.ROSTemplateId + ((item.NotesId != "" && item.NotesId != null) ? ',' + item.NotesId : '') + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;' +
                    '<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="ReviewOfSystemsDataTemplate.rosDataTemplateEdit(' + item.ROSDataTemplateId + "," + item.ROSTemplateId + "," + item.ROSDataTempInfoId + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;' +
                    '<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="ReviewOfSystemsDataTemplate.rosDataTemplateActiveInactive(' + item.ROSDataTemplateId + ', ' + isEventactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass +
                    '"></i></a>&nbsp;</td>' +
                    '<td>' + item.DataTemplateName + '</td><td>' + item.TemplateName + '</td><td>' + (item.IsSpecialtyAll == "True" ? "All" : item.SpecialtyNames) + '</td><td>' + item.ModifiedOn.replace(/(.*)\D\d+/, '$1') + ' ' + item.ModifiedByName + '</td>');
                $('#' + ReviewOfSystemsDataTemplate.params.PanelID + " #dgvROSDataTemplate tbody").last().append($row);
            });

            if ($.fn.dataTable.isDataTable('#' + ReviewOfSystemsDataTemplate.params.PanelID + " #dgvROSDataTemplate"))
                ;
            else {
                $('#' + ReviewOfSystemsDataTemplate.params.PanelID + " #dgvROSDataTemplate").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

            }
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                        '<input id="switchActive" isactive="' + isGridactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="ReviewOfSystemsDataTemplate.activeROSDataTemplateSearch(this);">' +
                         '</div><span class="pl-xs">Active</span>' +
         '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

            $("#" + ReviewOfSystemsDataTemplate.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }
        else {
            $('#' + ReviewOfSystemsDataTemplate.params.PanelID + " #dgvROSDataTemplate").DataTable({
                "language": {
                    "emptyTable": "No Template is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                      '<input id="switchActive" isactive="' + isGridactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="ReviewOfSystemsDataTemplate.activeROSDataTemplateSearch(this);">' +
                       '</div><span class="pl-xs">Active</span>' +
       '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

            $("#" + ReviewOfSystemsDataTemplate.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }

        EMRUtility.SwicthWidgetInializatoin();

    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Ros template for active/ inactive records 
   Creation Date: March 02,2016 */
    activeROSDataTemplateSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }
        ReviewOfSystemsDataTemplate.rosDataTemplateSearch();
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Ros template to delete records
   Creation Date: March 02,2016 */
    rosDataTemplateDelete: function (ROSDataTemplateId, ROSTemplateId, NotesId) {
        if (NotesId != null && NotesId != "") {
            utility.DisplayMessages('This data template is currently associated with Provider Notes and cannot be deleted.', 3);
        } else {
            utility.myConfirm('30', function () {
                if (ROSDataTemplateId > 0 && ROSTemplateId > 0) {
                    ReviewOfSystemsDataTemplate.rosDataTemplateDelete_DbCall(ROSDataTemplateId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            ReviewOfSystemsDataTemplate.rosDataTemplateSearch();
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            });
        }
    },
    rosDataTemplateEditRow: function (ROSDataTemplateId, ROSTemplateId, ROSDataTempInfoId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ((event.srcElement instanceof HTMLAnchorElement || event.srcElement.nodeName.toLowerCase() == 'i') != true) {
            ReviewOfSystemsDataTemplate.rosDataTemplateEdit(ROSDataTemplateId , ROSTemplateId , ROSDataTempInfoId, event);
        }
    },
    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Ros template for edit records
   Creation Date: March 02,2016 */
    rosDataTemplateEdit: function (ROSDataTemplateId, ROSTemplateId, ROSDataTempInfoId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Clinical_Data Template_Review of System", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (ROSDataTemplateId == "" || ROSDataTemplateId == "undefined") {
                }
                else {
                    var params = [];
                    params["ROSDataTemplateId"] = ROSDataTemplateId;
                    params["ROSTemplateId"] = ROSTemplateId;
                    params["ROSDataTempInfoId"] = ROSDataTempInfoId
                    params["mode"] = "Edit";
                    LoadActionPan('ReviewOfSystemsDataTemplateDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: to change active / in active records of Grid of Ros template 
   Creation Date: March 02,2016 */
    rosDataTemplateActiveInactive: function (ROSDataTemplateId, IsActive, event) {
        utility.myConfirm('3', function () {
            if (ROSDataTemplateId == "" || ROSDataTemplateId == "undefined") {
            }
            else {
                ReviewOfSystemsDataTemplate.updateRosDataTemplateActiveInactive_Dbcall(ROSDataTemplateId, IsActive).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        ReviewOfSystemsDataTemplate.rosDataTemplateSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
                         '3', null, null, null, IsActive
                    );
    },

    //--------------------------------- DbCall Functions of ROS Template  start----------------------
    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Ros template
   Creation Date: March 02,2016 */
    searchROSDataTemplate_DBCall: function (ROSDataTemplateId, ROSTemplateId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var IsActive = null;
        IsActive = $('#' + ReviewOfSystemsDataTemplate.params.PanelID + ' #pnlReviewOfSystemsDataTemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }
        var objData = {};
        objData["ROSDataTemplateId"] = ROSDataTemplateId == null ? -1 : ROSDataTemplateId;
        objData["ROSTemplateId"] = ROSTemplateId == null ? -1 : ROSTemplateId;
        objData["PageNumber"] = PageNumber;
        objData["IsActive"] = IsActive;
        objData["RowsPerPage"] = RowsPerPage;
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = null;
        } else {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        }
        objData["commandType"] = "SEARCH_ROS_DATA_TEMPLATE";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemDataTemplate", "ReviewOfSystemDataTemplate");
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Ros template to delete records
   Creation Date: March 02,2016 */
    rosDataTemplateDelete_DbCall: function (ROSDataTemplateId) {
        var objData = {};
        objData["ROSDataTemplateId"] = ROSDataTemplateId;
        objData["commandType"] = "DELETE_CLINICAL_ROS_DATA_ROSTEMPLATE";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "ReviewOfSystemDataTemplate", "ReviewOfSystemDataTemplate");
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: to change active / in active records of Grid of Ros template 
   Creation Date: March 02,2016 */
    updateRosDataTemplateActiveInactive_Dbcall: function (ROSDataTemplateId, IsActive) {
        var objData = {};
        objData["ROSDataTemplateId"] = ROSDataTemplateId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_CLINICAL_ROSDATATEMPLATE_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        //  var data = "NotesId=" + NotesId + "&IsActive=" + IsActive;
        // sNotesch parameter , class name, command name of class 
        return MDVisionService.APIService(data, "ReviewOfSystemDataTemplate", "ReviewOfSystemDataTemplate");
    }

    //--------------------------------- DbCall Functions of ROS Template  END----------------------
}