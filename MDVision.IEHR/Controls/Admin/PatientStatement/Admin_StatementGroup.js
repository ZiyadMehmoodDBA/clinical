Admin_StatementGroup = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {

        Admin_StatementGroup.params = params;

        if (Admin_StatementGroup.bIsFirstLoad) {
            Admin_StatementGroup.bIsFirstLoad = false;

            AppPrivileges.GetFormPrivileges("Statement Group", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_StatementGroup.SearchStatementGroup();
                }
            });


        }
    },

    StatementGroupAdd: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Statement Group", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["GroupId"] = "-1";
                params["mode"] = "Add";
                LoadActionPan('StatementGroupDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    SearchStatementGroup: function (StatementGroupId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Statement Group", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
               

                if ($("#pnlAdminStatementGroup #pnlStatementGroup_Result").css("display") == "none") {
                    $("#pnlAdminStatementGroup #pnlStatementGroup_Result").show();
                }

                // var StatementGroupId = null;

                var self = $("#pnlAdminStatementGroup");
                var myJSON = self.getMyJSON();


                Admin_StatementGroup.StatementGroupSearch(myJSON, StatementGroupId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {

                        //-----------------Pagination------------

                        if (response.StatementGroupCount > 0) {
                            $('#' + Admin_StatementGroup.params.PanelID + " #divStatementGroupPaging").css("display", "inline");
                            var TableControl = Admin_StatementGroup.params.PanelID + " #dgvStatementGroup";
                            var PagingPanelControlID = Admin_StatementGroup.params.PanelID + " #divStatementGroupPaging";
                            var ClassControlName = "Admin_StatementGroup";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(CreatePagination(response.StatementGroupCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Admin_StatementGroup.SearchStatementGroup(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);
                            //Showing 1 to 15 of 15 entries
                            //var RecordsPerPage = rpp != null ? rpp : 15;
                            //var CurrentPage = PageNo != null ? PageNo : 1;

                            //var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            //var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            //if (PageNo == null) {
                            //    utility.GetCustomPaging("divStatementGroupPaging", response.iTotalDisplayRecords, 5, "Admin_StatementGroup", CurrentPage, RecordsPerPage);
                            //}
                            //var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            //var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            //$('#' + Admin_StatementGroup.params.PanelID + " #divStatementGroupPaging #divShowingEntries").text(showingText);
                            //// Change Background Color to Black for selected page
                            //$('#' + Admin_StatementGroup.params.PanelID + " li").each(function () {
                            //    if ($(this).text() == CurrentPage) {
                            //        $(this).attr("class", "active");
                            //    }
                            //    else
                            //        $(this).removeAttr("class");
                            //});
                        }
                        else {
                            $('#' + Admin_StatementGroup.params.PanelID + " #divStatementGroupPaging").css("display", "none");
                        }

                        //--------------------End Pagination-------------------









                        Admin_StatementGroup.StatementGroupGridLoad(response);
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

    StatementGroupSearch: function (JSONstr, StatementGroupID, pageNo, recordPerPage) {
        if (JSONstr == null) {
            JSONstr = "";
        }

        if (StatementGroupID == null) {
            StatementGroupID = 0;
        }

        if (pageNo == null) {
            pageNo = 1;
        }
        if (recordPerPage == null) {
            recordPerPage = 15;
        }

        //FIXME
        var data = "StatementGroupData=" + JSONstr + "&StatementGroupID=" + StatementGroupID + "&pageNo=" + pageNo + "&recordPerPage=" + recordPerPage;

        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_GROUP", "SEARCH_STATEMENT_GROUP");
    },

    StatementGroupGridLoad: function (response) {
        $("#pnlAdminStatementGroup #pnlStatementGroup_Result #dgvStatementGroup").dataTable().fnDestroy();
        $("#pnlAdminStatementGroup #pnlStatementGroup_Result #dgvStatementGroup tbody").find("tr").remove();
        if (response.StatementGroupCount > 0) {
            var StatementGroupLoadJSONData = JSON.parse(response.StatementGroupLoad_JSON);


            $.each(StatementGroupLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_StatementGroup.StatementGroupEdit('" + item.PtStmtGrpId + "',event);");
                $row.attr("id", "dgvStatementGroup_row" + item.PtStmtGrpId);
                $row.attr("StmtMsgId", item.PtStmtGrpId);

                var selectMethod = "";

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


                $row.append('<td style="display:none;">' + item.PtStmtGrpId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_StatementGroup.StatementGroupDelete(' + item.PtStmtGrpId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_StatementGroup.StatementGroupEdit(' + item.PtStmtGrpId + ',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_StatementGroup.StatementGroupActiveInactive(' + item.PtStmtGrpId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Name + '">' + item.Name + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Description + '">' + item.Description + '</td><td>' + item.CycleDays + '</td><td>' + item.OutStandingDays + '</td><td>' + item.NoOfStatements + '</td><td>' + item.LetterName + '</td>');

                $("#pnlAdminStatementGroup #pnlStatementGroup_Result #dgvStatementGroup tbody").last().append($row);




            });



        }
        else {

            //FIXME
            if ($("#pnlAdminStatementGroup #pnlStatementGroup_Result").css("display") == "none") {
                $("#pnlAdminStatementGroup #pnlStatementGroup_Result").show();
            }

            $("#pnlAdminStatementGroup #pnlStatementGroup_Result #dgvStatementGroup").DataTable({
                "language": {
                    "emptyTable": "No Statement Groups Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });


        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#pnlAdminStatementGroup #pnlStatementGroup_Result #dgvStatementGroup"));
        else
            $("#pnlAdminStatementGroup #pnlStatementGroup_Result #dgvStatementGroup").DataTable({ "bLengthChange": false, "bSort": false, "bInfo": false, "bPaginate": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    StatementGroupActiveInactive: function (StatementGroupID, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Statement Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = StatementGroupID;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {

                        Admin_StatementGroup.UpdateStatementGroupActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_StatementGroup.SearchStatementGroup();

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

    UpdateStatementGroupActiveInactive: function (StatementGroupID, IsActive) {
        var data = "StatementGroupID=" + StatementGroupID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_GROUP", "UPDATE_STATEMENT_GROUP_ACTIVE_INACTIVE");
    },

    StatementGroupDelete: function (StatementGroupId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvStatementGroup_row' + StatementGroupId));
        AppPrivileges.GetFormPrivileges("Statement Group", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                utility.myConfirm('1', function () {
                    var selectedValue = StatementGroupId;
                    //var oTable = $('#dgvSupperBill').DataTable();
                    //var ind = $(this).index();
                    //var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_StatementGroup.DeleteStatementGroup(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvSupperBill').DataTable();
                                //table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                Admin_StatementGroup.SearchStatementGroup();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () {

                },
                    '1'
                );

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DeleteStatementGroup: function (StatementGroupId) {
        var data = "StatementGroupId=" + StatementGroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_GROUP", "DELETE_STATEMENT_GROUP");
    },


    StatementGroupEdit: function (StatementGroupId,event) {

        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvStatementGroup_row' + StatementGroupId));
        AppPrivileges.GetFormPrivileges("Statement Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = StatementGroupId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["StatementGroupId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('StatementGroupDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    /************PAGINATION FUNCTIONS**************/

    SelectedPageClick: function (PageNo, objPage, TotalRecords, rpp, pagingDivId) {
        Admin_StatementGroup.SearchStatementGroup(0, PageNo, 15);
    },
    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $('#' + Admin_StatementGroup.params.PanelID + " #pnlStatementGroup_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_StatementGroup.SearchStatementGroup(0, currentPageNo, 15);
        }
    },
    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $('#' + Admin_StatementGroup.params["PanelID"] + " #pnlStatementGroup_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_StatementGroup.SearchStatementGroup(0, currentPageNo, 15);
        }
    },
    /************************************/



    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },

};

