Clinical_Question = {

    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Clinical_Question.params = params;
        if (Clinical_Question.bIsFirstLoad) {
            Clinical_Question.bIsFirstLoad = false;
            var self = "";
            if (Clinical_Question.params["PanelID"] != "pnlClinicalQuestion") {
                self = $("#pnlClinicalQuestion");
                Clinical_Question.params["PanelID"] = "pnlClinicalQuestion";
            }
            else
                self = $('#' + Clinical_Question.params["PanelID"]);
            // self = $('#pnlClinicalQuestion');
            self.loadDropDowns(true).done(function () {
                //AppPrivileges.GetFormPrivileges("Question", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                //    if (strMessage == "") {
                Clinical_Question.QuestionSearch();
                //    }
            });
        }
    },

    QuestionAdd: function () {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Question", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        var params = [];
        params["QuestionId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('questionDetail', params);
        //LoadActionPan('userDetail', params);
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    QuestionSearch: function (QuestionId, PageNo, rpp) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Question", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        // if (strMessage == "") {
        //   if ($("#pnlClinicalQuestion #pnlQuestion_Result").css("display") == "none") {
        //      $("#pnlClinicalQuestion #pnlQuestion_Result").show();
        //  }

        var self = $("#pnlClinicalQuestion form");
        var myJSON = self.getMyJSONByName();

        Clinical_Question.SearchQuestion(myJSON, QuestionId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.QuestionCount > 0) {
                    $("#" + Clinical_Question.params["PanelID"] + " #divQuestionlistPaging").css("display", "inline");
                    //Showing 1 to 15 of 15 entries
                    var RecordsPerPage = rpp != null ? rpp : 15;
                    var CurrentPage = PageNo != null ? PageNo : 1;
                    var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                    var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                    if (PageNo == null) {
                        utility.GetCustomPaging("divQuestionlistPaging", response.iTotalDisplayRecords, 5, "Clinical_Question", CurrentPage, RecordsPerPage);
                    }
                    var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                    var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                    $("#" + Clinical_Question.params["PanelID"] + " #divQuestionlistPaging #divShowingEntries").text(showingText);
                    // Change Background Color to Black for selected page
                    $("#" + Clinical_Question.params["PanelID"] + " li").each(function () {
                        if ($(this).text() == CurrentPage) {
                            $(this).attr("class", "active");
                        }
                        else
                            $(this).removeAttr("class");
                    });
                }
                else {
                    $("#" + Clinical_Question.params["PanelID"] + " #divQuestionlistPaging").css("display", "none");
                }
                Clinical_Question.QuestionGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        // }
        // else
        //     utility.DisplayMessages(strMessage, 2);
        //});
    },
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#" + Clinical_Question.params["PanelID"] + " li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Clinical_Question.QuestionSearch(null, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        var CurrentCursor;
        $("#" + Clinical_Question.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                CurrentCursor = $(this);
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Clinical_Question.QuestionSearch(null, currentPageNo, 15);

        } else {
            CurrentCursor.addClass("active");
        }
        PageNo = currentPageNo;
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        var CurrentCursor;
        $("#" + Clinical_Question.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                CurrentCursor = $(this);
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0 && currentPageNo <= TotalPages) {
            Clinical_Question.QuestionSearch(null, currentPageNo, 15);
        } else {
            CurrentCursor.addClass("active");
        }
        PageNo = currentPageNo;
    },
    //SearchQuestion: function (QuestionData, questionID) {
    //    var data = "QuestionData=" + QuestionData + "&questionID=" + questionID;

    SearchQuestion: function (QuestionData, questionID, PageNo, rpp) {

        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }

        var objData = JSON.parse(QuestionData);
        objData["questionID"] = questionID;
        objData["PageNo"] = PageNo;
        objData["rpp"] = rpp;
        objData["commandType"] = "SEARCH_QUESTION";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TEMPLATEBUILDER", "ClinicalQuestion");
    },
    SearchQuestion1: function (QuestionData, questionID, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "QuestionData=" + QuestionData + "&questionID=" + questionID + "&PageNo=" + PageNo + "&rpp=" + rpp;
        return MDVisionService.APIService(QuestionData, "TEMPLATEBUILDER", "ClinicalQuestion", "SEARCH_QUESTION");
    },

    QuestionGridLoad: function (response, PageNo, rpp) {
        $("#pnlClinicalQuestion #dgvQuestion").dataTable().fnDestroy();
        $("#pnlClinicalQuestion #pnlQuestion_Result #dgvQuestion tbody").find("tr").remove();
        if (response.QuestionCount > 0) {
            var QuestionLoadJSONData = JSON.parse(response.QuestionLoad_JSON);
            $.each(QuestionLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvQuestion_row" + item.QuestionId + "'))");
                $row.attr("id", "gvQuestion_row" + item.QuestionId);
                $row.attr("QuestionId", item.QuestionId);
                //$row.attr("QuestionName", item.Description);
                //$row.attr("ShorttName", item.ShortName);
                //$row.attr("Type", item.QuestionTypeId);
                //$row.attr("QuestionType", item.Description1);

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
                //var EditMethod = "Clinical_Question.QuestionAddEdit(" + item.QuestionId + ",'Edit');";

                //   $row.append('<td style="display:none;">' + item.QuestionId + '</td><td><a class="btn  btn-xs" href="#" onclick="Clinical_Question.QuestionDelete(' + item.QuestionId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="questionDetail.FillQuestion(' + item.QuestionId + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_Question.QuestionActiveInactive(' + item.QuestionId + "," + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td>' + '</td><td>' + item.Description1 + '</td>');
                $row.append('<td style="display:none;">' + item.QuestionId + '</td><td><a class="btn  btn-xs" href="#" onclick="Clinical_Question.QuestionDelete(' + item.QuestionId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Question.QuestionEdit(' + item.QuestionId + ",'Edit'," + item.QuestionTypeId + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_Question.QuestionActiveInactive(' + item.QuestionId + "," + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td>' + '</td><td>' + item.Description1 + '</td>');
                $('#' + Clinical_Question.params["PanelID"] + " #pnlQuestion_Result #dgvQuestion tbody").last().append($row);
            });
            // $("#" + Clinical_Question.params["PanelID"] + " #divQuestionlistPaging").css("display", "inline");

        }
        else {
            $('#pnlClinicalQuestion #dgvQuestion').DataTable({
                "language": {
                    "emptyTable": "No Questions Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#' + Clinical_Question.params["PanelID"] + ' #dgvQuestion'))
            ;
        else
            $('#' + Clinical_Question.params["PanelID"] + ' #pnlQuestion_Result #dgvQuestion').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    QuestionActiveInactive: function (QuestionId, IsActive) {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Question", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        // if (strMessage == "") {
        //if (IsUAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser)
        //{ utility.DisplayMessages("Admin User can't be Active / In Active !", 3); }
        //else
        //{
        utility.myConfirm('3', function () {
            var selectedValue = QuestionId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {

                Clinical_Question.UpdateQuestionActiveInactive(selectedValue, IsActive).done(function (response) {
                    //response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        Clinical_Question.QuestionSearch('0');
                        UnloadActionPan();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
           '3', null, null, null, IsActive
        );
        //}
        // }
        // else
        //   utility.DisplayMessages(strMessage, 2);
        // });
    },

    UpdateQuestionActiveInactive: function (QuestionID, IsActive) {
        var data = "QuestionID=" + QuestionID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "CLINICAL_QUESTION", "UPDATE_QUESTION_ACTIVE_INACTIVE");
    },

    QuestionEdit: function (QuestionId, mode, QuestionTypeID) {
        // var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Question", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //if (strMessage == "") {
        //var selectedValue = UserId;
        // var selectedAdmin = IsUAdmin;
        // if (selectedValue == "" || selectedValue == "undefined") {
        // }
        // else {

        var params = [];
        params["QuestionId"] = QuestionId;
        params["mode"] = mode;
        params["QuestionTypeId"] = QuestionTypeID;
        LoadActionPan('questionDetail', params);
        //LoadActionPan('userDetail', params);
        //   }
        //}
        // else
        //     utility.DisplayMessages(strMessage, 2);
        //});
    },

    QuestionDelete: function (QuestionId) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Question", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //if (strMessage == "") {

        utility.myConfirm('1', function () {
            var selectedValue = QuestionId;
            var oTable = $('#dgvQuestion').DataTable();
            var ind = $(this).index();
            var idx = oTable.row(this).index();
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Question.DeleteQuestion(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var table1 = $('#dgvQuestion').DataTable();
                        table1.row('.active').remove().draw(false);
                        utility.DisplayMessages(response.Message, 1);
                        //utility.DisplayMessages("Record Deleted Successfully.", 1);
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

        //}
        //else
        //    utility.DisplayMessages(strMessage, 2);
        //});
    },

    DeleteQuestion: function (questionID) {
        var objData = new Object();
        objData["questionID"] = questionID;
        objData["commandType"] = "DELETE_QUESTION";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TEMPLATEBUILDER", "ClinicalQuestion");
    },

    SelectGridRow: function (obj) {
        if (obj.className != 'active') {
            $(obj).parent().children().removeClass('active');
            $(obj).addClass('active');
        }
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    }
}