ReviewOfSystemsTemplate = {

    //Author: ZeeshanAK
    //Date: 29-02-2016
    //This file will handle all actions performed for Review Of Systems Template
    bIsFirstLoad: true,
    params: [],


    Load: function (params) {
        ReviewOfSystemsTemplate.params = params;
        if (ReviewOfSystemsTemplate.params.PanelID != 'pnlReviewOfSystemsTemplate') {
            ReviewOfSystemsTemplate.params.PanelID = ReviewOfSystemsTemplate.params.PanelID + ' #pnlReviewOfSystemsTemplate';
        } else {
            ReviewOfSystemsTemplate.params.PanelID = 'pnlReviewOfSystemsTemplate';
        }

        ReviewOfSystemsTemplate.rosTemplateSearch();
    },

    ReviewOfSystemsTemplateAddEdit: function (ReviewOfSystemsTemplateId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template_Review of System", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
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
                params["FromAdmin"] = ReviewOfSystemsTemplate.params["FromAdmin"];
                params["ParentCtrl"] = 'adminTabReviewOfSystemsTemplate';
                LoadActionPan('ReviewOfSystemsTemplateDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    /*
    Author: Muhammad Azhar Shahzad
    Purpose: for Grid Load of Ros template
    Creation Date: March 02,2016 */
    rosTemplateSearch: function (ROSTemplateId, PageNo, rpp) {

        ReviewOfSystemsTemplate.searchROSTemplate_DBCall(ROSTemplateId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ReviewOfSystemsTemplate.rosTemplateGridLoad(response);
                //Adding Pagination on 04 Dec 2015 by Azhar
                var TableControl = ReviewOfSystemsTemplate.params.PanelID + " #dgvROSTemplate";
                var PagingPanelControlID = ReviewOfSystemsTemplate.params.PanelID + " #dgvROSTemplate_Paging";
                var ClassControlName = "ReviewOfSystemsTemplate";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ROSTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    ReviewOfSystemsTemplate.rosTemplateSearch(PrimaryID, PageNumber, ResultPerPage);
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
    rosTemplateGridLoad: function (response) {
        var isactive = $('#' + ReviewOfSystemsTemplate.params.PanelID + ' #pnlReviewOfSystemsTemplate_Result #divSwitch #switchActive').attr('isactive');
        if ($.fn.dataTable.isDataTable('#' + ReviewOfSystemsTemplate.params.PanelID + " #dgvROSTemplate")) {
            $('#' + ReviewOfSystemsTemplate.params.PanelID + " #dgvROSTemplate").dataTable().fnDestroy();
            $('#' + ReviewOfSystemsTemplate.params.PanelID + " #dgvROSTemplate tbody").find("tr").remove();
        }

        if (response.ROSTemplateCount > 0) {
            var ROSTemplateLoadJSONData = JSON.parse(response.ROSTemplateLoad_JSON);
            $.each(ROSTemplateLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvROSTemplate_row" + item.ROSTemplateId + "'));");
                $row.attr("id", "gvROSTemplate_row" + item.ROSTemplateId);
                $row.attr("ROSTemplateId", item.ROSTemplateId);
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
                } else {
                    $row.attr("onclick", "ReviewOfSystemsTemplate.rosTemplateEditRow(" + item.ROSTemplateId + ", event);");
                }

                $row.append('<td>' +
                    '<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="ReviewOfSystemsTemplate.rosTemplateDelete(' + item.ROSTemplateId + ((item.NotesId != "" && item.NotesId != null) ? ',' + item.NotesId : '') + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;' +
                    '<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="ReviewOfSystemsTemplate.rosTemplateEdit(' + item.ROSTemplateId + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;' +
                    '<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="ReviewOfSystemsTemplate.rosTemplateActiveInactive(' + item.ROSTemplateId + ', ' + isEventactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass +
                    '"></i></a>&nbsp;</td>' +
                    '<td>' + item.TemplateName + '</td><td>' + (item.IsSpecialtyAll == "True" ? "All" : item.SpecialtyNames) + '</td><td>' + item.ModifiedOn.replace(/(.*)\D\d+/, '$1') + ' ' + item.ModifiedByName + '</td>');
                $('#' + ReviewOfSystemsTemplate.params.PanelID + " #dgvROSTemplate tbody").last().append($row);
            });
            var checked = '';
            if (isactive == "0" || isactive == 0) {
            } else if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "1";
                checked = 'checked="checked"';
            }
            if ($.fn.dataTable.isDataTable('#' + ReviewOfSystemsTemplate.params.PanelID + " #dgvROSTemplate"))
                ;
            else {
                $('#' + ReviewOfSystemsTemplate.params.PanelID + " #dgvROSTemplate").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

            }
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                        '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="ReviewOfSystemsTemplate.activeROSTemplateSearch(this);">' +
                         '</div><span class="pl-xs">Active</span>' +
         '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

            $("#" + ReviewOfSystemsTemplate.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }
        else {
            if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } 
            $('#' + ReviewOfSystemsTemplate.params.PanelID + " #dgvROSTemplate").DataTable({
                "language": {
                    "emptyTable": "No Template is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                      '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="ReviewOfSystemsTemplate.activeROSTemplateSearch(this);">' +
                       '</div><span class="pl-xs">Active</span>' +
       '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

            $("#" + ReviewOfSystemsTemplate.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }

        EMRUtility.SwicthWidgetInializatoin();

    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Ros template for active/ inactive records 
   Creation Date: March 02,2016 */
    activeROSTemplateSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }
        ReviewOfSystemsTemplate.rosTemplateSearch();
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Ros template to delete records
   Creation Date: March 02,2016 */
    rosTemplateDelete: function (ROSTemplateId, NotesId) {
        if (NotesId != null && NotesId != "") {
            utility.DisplayMessages('This template is currently associated with Provider Notes and cannot be deleted.', 3);
        } else {
            utility.myConfirm('30', function () {
                if (ROSTemplateId > 0) {
                    ReviewOfSystemsTemplate.rosTemplateDelete_DbCall(ROSTemplateId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            ReviewOfSystemsTemplate.rosTemplateSearch();
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
    rosTemplateEditRow: function (ROSTemplateId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ((event.srcElement instanceof HTMLAnchorElement || event.srcElement.nodeName.toLowerCase() == 'i') != true) {
            ReviewOfSystemsTemplate.rosTemplateEdit(ROSTemplateId, event);
        }
    },
    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Ros template for edit records
   Creation Date: March 02,2016 */
    rosTemplateEdit: function (ROSTemplateId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Template_Review of System", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                
                if (ROSTemplateId == "" || ROSTemplateId == "undefined") {
                }
                else {
                    var params = [];
                    params["ROSTemplateId"] = ROSTemplateId;
                    params["mode"] = "Edit";
                    LoadActionPan('ReviewOfSystemsTemplateDetail', params);
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
    rosTemplateActiveInactive: function (ROSTemplateId, IsActive, event) {
        utility.myConfirm('3', function () {
            if (ROSTemplateId == "" || ROSTemplateId == "undefined") {
            }
            else {
                ReviewOfSystemsTemplate.updateRosTemplateActiveInactive_Dbcall(ROSTemplateId, IsActive).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        ReviewOfSystemsTemplate.rosTemplateSearch();
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
    searchROSTemplate_DBCall: function (ROSTemplateId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var IsActive = null;
        IsActive = $('#' + ReviewOfSystemsTemplate.params.PanelID + ' #pnlReviewOfSystemsTemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }
        var objData = {};
        objData["ROSTemplateId"] = ROSTemplateId == null ? -1 : ROSTemplateId;
        objData["PageNumber"] = PageNumber;
        objData["IsActive"] = IsActive;
        objData["RowsPerPage"] = RowsPerPage;
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = null;
        } else {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        }
        objData["commandType"] = "search_ros_systems_template";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemTemplate", "ReviewOfSystemTemplate");
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Ros template to delete records
   Creation Date: March 02,2016 */
    rosTemplateDelete_DbCall: function (ROSTemplateId) {
        var objData = {};
        objData["ROSTemplateId"] = ROSTemplateId;
        objData["commandType"] = "DELETE_CLINICAL_ROSTEMPLATE";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "ReviewOfSystemTemplate", "ReviewOfSystemTemplate");
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: to change active / in active records of Grid of Ros template 
   Creation Date: March 02,2016 */
    updateRosTemplateActiveInactive_Dbcall: function (ROSTemplateId, IsActive) {
        var objData = {};
        objData["ROSTemplateId"] = ROSTemplateId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_CLINICAL_ROSTEMPLATE_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        //  var data = "NotesId=" + NotesId + "&IsActive=" + IsActive;
        // sNotesch parameter , class name, command name of class 
        return MDVisionService.APIService(data, "ReviewOfSystemTemplate", "ReviewOfSystemTemplate");
    }

    //--------------------------------- DbCall Functions of ROS Template  END----------------------
}