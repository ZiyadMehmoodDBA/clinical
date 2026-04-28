
Clinical_Question_Group = {

    bIsFirstLoad: true,
    params: [],
   
    Load: function (params) {
        Clinical_Question_Group.params = params;

        if (Clinical_Question_Group.bIsFirstLoad) {
            Clinical_Question_Group.bIsFirstLoad = false;

            var self = "";
            if (Clinical_Question_Group.params["PanelID"] != "pnlClinicalQuestion_Group") {
                self = $('#' + Clinical_Question_Group.params["PanelID"] + " #pnlClinicalQuestion_Group")
            }
            else
                self = $('#' + Clinical_Question_Group.params["PanelID"]);

            self.loadDropDowns(true).done(function () {               
                Clinical_Question_Group.QuestionGroupSearch();
            });

           

            ////AppPrivileges.GetFormPrivileges("CPT", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {
                 //   Clinical_Question_Group.QuestionGroupSearch();
            //    }
            //});
        }
    },
    QuestionGroupAdd: function () {
        var params = [];
        params["QuestionGroupId"] = "-1";
        params["mode"] = "Add";
    //    params["ParentCtrl"] = 'Clinical_Question_Group';
        LoadActionPan('questionGroupDetail', params);
        //var strMessage = "";
        ////AppPrivileges.GetFormPrivileges("CPT", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        //        var params = [];
        //        params["QuestionGroupId"] = "-1";
        //      //  params["EntityId"] = Clinical_Question_Group.params["EntityId"];
        //        params["mode"] = "Add";

        //        params["FromAdmin"] = "0";// Clinical_Question_Group.params["FromAdmin"];
              
        //     //   if (Clinical_Question_Group.params["FromAdmin"] == "0") {
        //            params["ParentCtrl"] = 'Clinical_Question_Group';
        //       // }
        //            LoadActionPan('pnlClinicalQuestionGroupDetail', params);
        //    }
        ////    else
        ////        utility.DisplayMessages(strMessage, 2);
        ////});
    },
   
    UnLoadTab: function () {
        if (Clinical_Question_Group.params["FromAdmin"] == "0") {
            if (Clinical_Question_Group.params != null && Clinical_Question_Group.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_Question_Group.params.ParentCtrl, 'Clinical_Question_Group');
            }
            else
                UnloadActionPan(null, 'Clinical_Question_Group');

        }
        else {
            RemoveAdminTab();
        }
    },
QuestionGroupEdit: function (QuestionGroupId,event) {
    var strMessage = "";
    if (event != null) {
            event.stopPropagation();
        }
    //AppPrivileges.GetFormPrivileges("CPT", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var selectedValue = QuestionGroupId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var params = [];
                params["QuestionGroupId"] = selectedValue;
                params["mode"] = "Edit";
                params["EntityId"] = Clinical_Question_Group.params["EntityId"];
                params["FromAdmin"] = Clinical_Question_Group.params["FromAdmin"];

                if (Clinical_Question_Group.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Clinical_Question_Group';
                }
                LoadActionPan('questionGroupDetail', params);
            }
        }
    //    else
    //        utility.DisplayMessages(strMessage, 2);
    //});
},

QuestionGroupDelete: function (QuestionGroupId,event) {
    var strMessage = "";
    if (event != null) {
            event.stopPropagation();
        }
    //AppPrivileges.GetFormPrivileges("CPT", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = QuestionGroupId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_Question_Group.DeleteQuestionGroup(selectedValue).done(function (response) {
                        if (response.status != false) {
                            var table1 = $('#dgvQuestionGroup').DataTable();
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
    //    else
    //        utility.DisplayMessages(strMessage, 2);
    //});
},

    
DeleteQuestionGroup: function (QuestionGroupID) {
    var data = "QuestionGroupID=" + QuestionGroupID;
        // serach parameter , class name, command name of class 
    return MDVisionService.defaultService(data, "Clinical_Question_Group", "DELETE_QUESTION_GROUP");
    },
QuestionGroupActiveInactive: function (QuestionGroupId, IsActive,event) {
    PageNo = null;
    rpp = null;
    var strMessage = "";
    if (event != null) {
            event.stopPropagation();
        }
    //AppPrivileges.GetFormPrivileges("CPT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('3', function () {
                var selectedValue = QuestionGroupId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_Question_Group.UpdateQuestionGroupActiveInactive(selectedValue, IsActive,PageNo, rpp).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Clinical_Question_Group.QuestionGroupSearch('0', PageNo, rpp);
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
    //    else
    //        utility.DisplayMessages(strMessage, 2);
    //});
},


UpdateQuestionGroupActiveInactive: function (QUESTIONGROUPID, IsActive, PageNo, rpp) {
    if (PageNo == null) {
        PageNo = 1;
    }
    if (rpp == null) {
        rpp = 15;
    }
    var data = "QUESTIONGROUPID=" + QUESTIONGROUPID + "&IsActive=" + IsActive + "&PageNo=" + PageNo + "&rpp=" + rpp;
    // serach parameter , class name, command name of class 
    return MDVisionService.defaultService(data, "Clinical_Question_Group", "UPDATE_QUESTION_GROUP_ACTIVE_INACTIVE");
},

QuestionGroupSearch: function (QuestionGroupId, PageNo, rpp,event) {
    var strMessage = "";
    if (event != null) {
            event.stopPropagation();
        }
    //AppPrivileges.GetFormPrivileges("CPT", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + Clinical_Question_Group.params["PanelID"] + ' #pnlQuestionGroup_Result').css("display") == "none") {
                $('#' + Clinical_Question_Group.params["PanelID"] + ' #pnlQuestionGroup_Result').show();
            }

            var self = $('#' + Clinical_Question_Group.params["PanelID"] + ' #pnlQuestionGroup_Search');
            var myJSON = self.getMyJSON();

            Clinical_Question_Group.SearchQuestionGroup(myJSON, QuestionGroupId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    if (response.QuestionGroupCount > 0) {
                        $("#" + Clinical_Question_Group.params["PanelID"] + " #divQuestionGroupPaging").css("display", "inline");
                        //Showing 1 to 15 of 15 entries
                        var RecordsPerPage = rpp != null ? rpp : 15;
                        var CurrentPage = PageNo != null ? PageNo : 1;
                        var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                        var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                        if (PageNo == null) {
                            utility.GetCustomPaging("divQuestionGroupPaging", response.iTotalDisplayRecords, 5, "Clinical_Question_Group", CurrentPage, RecordsPerPage);
                        }
                        var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                        var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                        $("#" + Clinical_Question_Group.params["PanelID"] + " #divQuestionGroupPaging #divShowingEntries").text(showingText);
                        // Change Background Color to Black for selected page
                        $("#" + Clinical_Question_Group.params["PanelID"] + " li").each(function () {
                            if ($(this).text() == CurrentPage) {
                                $(this).attr("class", "active");
                            }
                            else
                                $(this).removeAttr("class");
                        });
                    }
                    else {

                        $("#" + Clinical_Question_Group.params["PanelID"] + " #divQuestionGroupPaging").css("display", "none");
                    }
                    Clinical_Question_Group.QuestionGroupGridLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                
            });
        }
        else
            utility.DisplayMessages(strMessage, 2);
    //});
},
    

SearchQuestionGroup: function (QuestionGroupData, QuestionGroupId, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "QuestionGroupData=" + QuestionGroupData + "&QuestionGroupId=" + QuestionGroupId + "&PageNo=" + PageNo + "&rpp=" + rpp;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Clinical_Question_Group", "SEARCH_QUESTION_GROUP");
    },

QuestionGroupGridLoad: function (response) {
    $('#' + Clinical_Question_Group.params["PanelID"] + ' #dgvQuestionGroup').dataTable().fnDestroy();
    $('#' + Clinical_Question_Group.params["PanelID"] + ' #pnlQuestionGroup_Result #dgvQuestionGroup tbody').find("tr").remove();
    if (response.QuestionGroupCount > 0) {
        var QuestionGroupLoadJSONData = JSON.parse(response.QuestionGroupFill_JSON);
        $.each(QuestionGroupLoadJSONData, function (i, item) {
            var $row = $('<tr/>');

            $row.attr("id", "gvQuestionGroup_row" + item.QuestionGroupID);
            $row.attr("QuestionGroupId", item.QuestionGroupID);
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
                var selectMethod = "";
            if (Clinical_Question_Group.params["FromAdmin"] == "0") {
                    selectMethod = "Clinical_Question_Group.FillQuestionGroup('" + item.QuestionGroupID + "','" + item.QuestionGroup + "');"
                    selectCPT = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "utility.SelectGridRow($('#gvQuestionGroup_row" + item.QuestionGroupID + "'))");
                }
                $row.append('<td style="display:none;">' + item.QuestionGroupID + '</td><td><a class="btn  btn-xs" href="#" onclick="Clinical_Question_Group.QuestionGroupDelete(' + item.QuestionGroupID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Question_Group.QuestionGroupEdit(' + item.QuestionGroupID + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_Question_Group.QuestionGroupActiveInactive(' + item.QuestionGroupID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectCPT + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.SpecialtyShortName + '</td><td>' + item.Description + '</td><td>' + item.BodySystemShortName + '</td><td>' + item.IsActive + '</td>');

            $('#' + Clinical_Question_Group.params["PanelID"] + ' #pnlQuestionGroup_Result #dgvQuestionGroup tbody').last().append($row);
        });
    }
    else {
        $('#' + Clinical_Question_Group.params["PanelID"] + ' #pnlQuestionGroup_Result #dgvQuestionGroup tbody').html('');
        $('#' + Clinical_Question_Group.params["PanelID"] + ' #dgvQuestionGroup').DataTable({
            "language": {
                "emptyTable": "No Question Group Found"
            }, "autoWidth": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
        });
        var showingText = "Showing 0 to 0 of 0 Record(s)";
        $("#" + Clinical_Question_Group.params["PanelID"] + " #divQuestionGroupPaging #divShowingEntries").text(showingText);

    }
    if ($.fn.dataTable.isDataTable('#' + Clinical_Question_Group.params["PanelID"] + ' #dgvQuestionGroup'))
        ;
    else
        $('#' + Clinical_Question_Group.params["PanelID"] + ' #pnlQuestionGroup_Result #dgvQuestionGroup').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
},

SelectedPageClick: function (PageNo, objPage) {
    // Change Background Color to Black for selected page
    $("#" + Clinical_Question_Group.params["PanelID"] + " li").each(function () {
        if ($(this).text() == PageNo) {
            $(this).attr("class", "active");
        }
        else
            $(this).removeAttr("class");
    });
    Clinical_Question_Group.QuestionGroupSearch(null, PageNo, 15);
},

PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
    var currentPageNo = "";
    $("#" + Clinical_Question_Group.params["PanelID"] + " li").each(function () {
        if ($(this).attr("class") == "active") {
            $(this).removeAttr("class");
            currentPageNo = parseInt($(this).text());
        }

    });
    currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

    if (currentPageNo != "" && currentPageNo > 0) {
        Clinical_Question_Group.QuestionGroupSearch(null, currentPageNo, 15);

    }
    PageNo = currentPageNo;
},

NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
    var currentPageNo = "";
    $("#" + Clinical_Question_Group.params["PanelID"] + " li").each(function () {
        if ($(this).attr("class") == "active") {
            $(this).removeAttr("class");
            currentPageNo = parseInt($(this).text());
        }
    });
    currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
    if (currentPageNo != "" && currentPageNo > 0) {
        Clinical_Question_Group.QuestionGroupSearch(null, currentPageNo, 15);
    }
    PageNo = currentPageNo;
},
}
