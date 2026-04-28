


OrderSet_Problems = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    icdsValues: {},
    LastSctBaseSearch: '',
    FavListName: "ClinicalProblemList",
    //IsProblemFirstLoad: true,

    Load: function (params) {
        OrderSet_Problems.params = params;
        if (OrderSet_Problems.params.PanelID != 'pnlOrderSetProblemLists') {
            OrderSet_Problems.params.PanelID = OrderSet_Problems.params.PanelID + ' #pnlOrderSetProblemLists';
        } else {
            OrderSet_Problems.params.PanelID = 'pnlOrderSetProblemLists';
        }
        if (OrderSet_Problems.bIsFirstLoad) {
            OrderSet_Problems.bIsFirstLoad = false;
            var self = $('#' + OrderSet_Problems.params["PanelID"]);
            //---problems
            OrderSet_Problems.domReadyFunc();
            //for Favorites toggle
            EMRUtility.setFavoriteSectionStyle(OrderSet_Problems.params.PanelID);

            utility.ValidateFromToDate('frmOrderSetProblemLists', 'dpStartDate', 'dpEndDate', true);


            try {
                $("#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result").css('display', 'inline');
                $("#" + OrderSet_Problems.params.PanelID + " #formpanelheading").css('display', '');
                if (OrderSet_Problems.params.FromOrderDetail != null && OrderSet_Problems.params.FromOrderDetail == "1") {
                    $("#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result").css('display', 'none');
                    $("#" + OrderSet_Problems.params.PanelID + " #formpanelheading").css('display', 'none');
                }
            } catch (ex) {
                console.log(ex);
            }

            var FromOrderDetail = false;
            try {
                FromOrderDetail = (OrderSet_Problems.params.FromOrderDetail != null && OrderSet_Problems.params.FromOrderDetail == "1");
            } catch (ex) {
                console.log(ex);
            }

            if (!FromOrderDetail) {
                OrderSet_Problems.ProblemListsSearch();
            }


            self.loadDropDowns(true).done(function () {
                OrderSet_Problems.ValidateProblemLists();
                //Serialization
                $('#' + OrderSet_Problems.params.PanelID + ' #frmOrderSetProblemLists').data('serialize', $('#' + OrderSet_Problems.params.PanelID + ' #frmOrderSetProblemLists').serialize());

                //for favorite list setting for all favLists
                if (EMRUtility.getFavListStatus(OrderSet_Problems.FavListName))
                    $('#' + OrderSet_Problems.params.PanelID + " #frmOrderSetProblemLists #favSectionDiv").removeClass("toggled");
                else
                    $('#' + OrderSet_Problems.params.PanelID + " #frmOrderSetProblemLists #favSectionDiv").addClass("toggled");
                //end for favorite list setting for all favLists

            });
            OrderSet_Problems.params.mode = "Add";
        }
    },

    //----------------------------------------------------//
    //-------------Problems Functions Start---------------//
    //----------------------------------------------------//
    ProblemListsSearch: function (ProblemListId, PageNo, rpp) {
        OrderSet_Problems.SearchProblemList(ProblemListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#" + OrderSet_Problems.params.PanelID + " #dgvOrderSetProblemLists th#SelectRecord").remove();
                OrderSet_Problems.ProblemListGridLoadNew(response);
                var TableControl = OrderSet_Problems.params.PanelID + " #dgvOrderSetProblemLists";
                var PagingPanelControlID = OrderSet_Problems.params.PanelID + " #dgvOrderSetProblemLists_Paging";
                var ClassControlName = "OrderSet_Problems";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ProblemListCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    OrderSet_Problems.ProblemListsSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    SearchProblemList: function (ProblemListId, PageNumber, RowsPerPage, getInActiveProblems) {
        var IsCheckedIn = null;
        IsCheckedIn = $('#' + OrderSet_Problems.params.PanelID + ' #pnlOrderSetProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }


        // Patch for getting InActiveProblems // fixMe
        if (getInActiveProblems)
            IsCheckedIn = getInActiveProblems;

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["OrderSetId"] = OrderSet_Problems.params.OrderSetId;
        objData["IsActive"] = IsCheckedIn;
        objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        objData["NoteId"] = 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProblemList");

    },

    ProblemListGridLoadNew: function (response) {
        // get Actions
        var actions = "";
        $("#" + OrderSet_Problems.params.PanelID + " #dgvOrderSetProblemLists tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, "#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result");
                }
            }
        });
        if ($.fn.dataTable.isDataTable("#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result #dgvOrderSetProblemLists")) {
            $("#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result #dgvOrderSetProblemLists").dataTable().fnClearTable();
            $("#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result #dgvOrderSetProblemLists").dataTable().fnDestroy();
            $("#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result #dgvOrderSetProblemLists tbody").find("tr").remove();
        }
        var totalProblems = 0;
        if (response.ProblemListCount > 0) {
            totalProblems = response.ProblemListCount;
            var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
            var ProblemListHistoryLoadJSONData = JSON.parse(response.ProblemListHistoryLoad_JSON);

            if ($.fn.dataTable.isDataTable("#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result #dgvOrderSetProblemLists")) {
                $("#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result #dgvOrderSetProblemLists").dataTable().fnDestroy();
            }
            //tem array to hold rows and childs
            var arraTemp = [];

            $.each(ProblemListLoadJSONData, function (i, item) {
                var $infoButtonrow = "";
                if (item.Description != "") {
                    if (typeof item.Description !== 'undefined' && typeof item.Description.split(' - ')[1] !== 'undefined') {
                        var searchstr = item.Description.split('-')[0].trim();
                        if (item.Description.split(" - ")[2] != undefined) {
                            item.Description = item.Description.substring(item.Description.indexOf(" - ") + 2);
                        }
                        $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(searchstr, "OrderSet_Problems", 2);
                    }
                }
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.ProblemListId);
                $row.attr("ProblemListNotesId", item.NoteId);

                $row.attr("name", "Problems");

                var color = "";

                if (item.Severity == "Mild Intermittent" || item.Severity == "Mild Persistent") {
                    color = 'style = "color:green;font-weight:bold"'
                }
                if (item.Severity == "Severe Persistent" || item.Severity == "Unspecified Severity") {
                    color = 'style = "color:red;font-weight:bold"'
                }
                if (item.Severity == "Moderate Persistent") {
                    color = 'style = "color:orange;font-weight:bold"'
                }

                var comments = "";

                if (item.IsActive.toLowerCase() == "false") {
                    if (item.InActiveReason.length > 0) {
                        comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.InActiveChkBoxValue + ": " + item.InActiveReason + '"><i class="fa fa-commenting blue"></i></label>';
                    } else {
                        comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.InActiveChkBoxValue + '"><i class="fa fa-commenting blue"></i></label>';
                    }
                }
                else {
                    if (item.Comments != "") {
                        var commentsMethod = "OrderSet_Problems.AddComments('" + item.ProblemListId + "');";
                        //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                        comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                    }
                }
                var ProblemListId = item.ProblemListId;
                var ChildHistory_ProblemList = $.grep(ProblemListHistoryLoadJSONData, function (n, i) {
                    return n.ProblemId == ProblemListId;
                });
                if (OrderSet_Problems.params.ParentCtrl == "clinicalTabProgressNote" && item.ProblemName == "No Known Problems") {
                    //$row.append('<td></td><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + item.ProblemName + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                    //Start//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row

                    var Checked = "";
                    if (item.IsNoteLinked == "True") {
                        if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                            Checked = " ";
                        } else {
                            Checked = " checked";
                        }
                    } else {
                        if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                            Checked = " checked";
                        } else {
                            Checked = "";
                        }
                    }
                    //AST-14 BY:MAhmad
                    if (ChildHistory_ProblemList.length > 0) {
                        $row.append('<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none">' + item.Comments + '</td>');
                    } else {
                        $row.append('<td></td><td></td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none">' + item.Comments + '</td>');
                    }
                    //AST-14 BY:MAhmad
                    //End//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row
                }
                else {


                    if (item.ProblemName == "No Known Problems") {
                        //AST-14 BY:MAhmad
                        $row.append('<td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + item.ProblemName + $infoButtonrow + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none">' + item.Comments + '</td>');
                        //AST-14 BY:MAhmad
                    }
                    else {
                        //adding checkboxes column and disabling that row, if problem list already binded with notes

                        var Checked = "";
                        if (item.IsNoteLinked == "True") {
                            if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                                Checked = " ";
                            } else {
                                Checked = " checked";
                            }
                        } else {
                            if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                                Checked = " checked";
                            } else {
                                Checked = "";
                            }
                        }
                        //AST-14 BY:MAhmad

                        if (ChildHistory_ProblemList.length > 0) {
                            $row.append('<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions" id="' + item.ProblemListId + '" >' + actions + '</td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none">' + item.Comments + '</td>');
                        } else {
                            $row.append('<td></td><td class="actions" id="' + item.ProblemListId + '" >' + actions + '</td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none">' + item.Comments + '</td>');
                        }
                        //AST-14 BY:MAhmad
                    }
                }
                if (item.IsActive == "True") {
                    // $($row).find('a.edit-row').removeAttr('disabled', false);
                    $($row).find('a.edit-row').removeClass('disableAll')
                } else {
                    $($row).find('a.edit-row').addClass('disableAll')
                    //  $($row).find('a.edit-row').attr('disabled', 'disabled')
                }


                $("#" + OrderSet_Problems.params.PanelID + " #dgvOrderSetProblemLists tbody").last().append($row);
                if (totalProblems > 1) {
                    $('#' + OrderSet_Problems.params.PanelID + ' #dgvOrderSetProblemLists tbody tr').each(function () {
                        if ($(this).text().indexOf("No Known Problems") > -1) {
                            $(this).remove();
                        }
                    });
                }

                var CurrentRowchilds = $();

                if (ChildHistory_ProblemList.length > 0) {
                    $.each(ChildHistory_ProblemList, function (i, item) {
                        // if (item.ProblemId == ProblemListId) {
                        //arrProblemListHistory.push(item);

                        comments = "";
                        if (item.IsActive.toLowerCase() == "false") {
                            if (item.InActiveReason.length > 0) {
                                comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.InActiveChkBoxValue + ": " + item.InActiveReason + '"><i class="fa fa-commenting blue"></i></label>';
                            } else {
                                comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.InActiveChkBoxValue + '"><i class="fa fa-commenting blue"></i></label>';
                            }
                        }
                        else {
                            var commentsMethod = "OrderSet_Problems.AddComments('" + item.ProblemListId + "');";
                            //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                            comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                        }

                        var Title_Tooltip = "Inactive Reason: " + item.InActiveChkBoxValue + (item.EndDate != '' ? " <br/>End Date: " + utility.RemoveTimeFromDate(null, item.EndDate) : "") + (item.InActiveReason != '' ? " <br/>Comments: " + item.InActiveReason : "");
                        var IsActiveText = "";
                        if (item.IsActive == "True") {
                            IsActiveText = "<Label>[Active]</Label>";
                        } else {
                            IsActiveText = "<Label data-toggle='tooltip' data-placement='right' title='" + Title_Tooltip + "'>[Inactive]</Label>";
                        }

                        // Irfan Change color of severity

                        var colorChild = "";

                        if (item.Severity == "Mild Intermittent" || item.Severity == "Mild Persistent") {
                            colorChild = 'style = "color:green;font-weight:bold"'
                        }
                        if (item.Severity == "Severe Persistent" || item.Severity == "Unspecified Severity") {
                            colorChild = 'style = "color:red;font-weight:bold"'
                        }
                        if (item.Severity == "Moderate Persistent") {
                            colorChild = 'style = "color:orange;font-weight:bold"'
                        }

                        // end Change color of severity

                        //Start Row Sequence issue in History Grid, fixed
                        //AST-14 BY:MAhmad
                        var currentHistory = '<tr class="childRow-bg"><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + IsActiveText + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + colorChild + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none">' + item.Comments + '</td></tr>';
                        //AST-14 BY:MAhmad
                        //End Row Sequence issue in History Grid, fixed

                        CurrentRowchilds = CurrentRowchilds.add(currentHistory);

                    });
                }

                if (CurrentRowchilds.length > 0) {

                }

                arraTemp.push({ row: $row, childs: CurrentRowchilds });
            });

            //Inalize grid
            var PanelGrid = "#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result";
            var GridId = "#" + OrderSet_Problems.params.PanelID + " #dgvOrderSetProblemLists";

            // Below line comment out inorder to remove duplicate grid search
            if (OrderSet_Problems.myGrid != null) {
                if ($.fn.dataTable.isDataTable(OrderSet_Problems.myGrid)) {
                    OrderSet_Problems.myGrid.$table.dataTable().fnDestroy();
                } else {
                    OrderSet_Problems.myGrid = null;
                }
                if ($.fn.dataTable.isDataTable("#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result #dgvOrderSetProblemLists")) {
                    $("#" + OrderSet_Problems.params.PanelID + " #pnlOrderSetProblemLists_Result #dgvOrderSetProblemLists").dataTable().fnDestroy();
                }
            }
            // Below line comment out inorder to remove duplicate grid search
            OrderSet_Problems.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, OrderSet_Problems, 0, false, true, false, true, false, null);

            //rander childs
            $.each(arraTemp, function (i, item) {

                if (OrderSet_Problems.myGrid != null) {
                    var row = OrderSet_Problems.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });
            //Sorting removed from first column of grid
            if ($('#' + OrderSet_Problems.params.PanelID + ' #dgvOrderSetProblemLists').length > 0) {
                $('#' + OrderSet_Problems.params.PanelID + ' #dgvOrderSetProblemLists').dataTable().fnSettings().aoColumns[0].bSortable = false;
                $('#' + OrderSet_Problems.params.PanelID + ' #dgvOrderSetProblemLists').dataTable().fnSort([[8, "desc"]]);
            }
            //Sorting removed from first column of grid


        }
        else {
            if ($('#' + OrderSet_Problems.params.PanelID + ' div#divShowHistory').hasClass("hidden") == false) {
                $('#' + OrderSet_Problems.params.PanelID + ' div#divShowHistory').addClass("hidden");
            }

            $('#' + OrderSet_Problems.params.PanelID + ' #pnlOrderSetProblemLists_Result #dgvOrderSetProblemLists').DataTable({
                "language": {
                    "emptyTable": "No Problem List Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
            });
            if (response.ProblemListCount == 0) {
                $("#pnlOrderSetProblemLists_Result #btnNoKnownProblems").css("display", "");
            } else {
                $("#pnlOrderSetProblemLists_Result #btnNoKnownProblems").hide();
            }

            /* End of Code for making No Known Problem List hyperlink inline with checkbox and search box.
            *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
            */

        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        $('#' + OrderSet_Problems.params.PanelID + ' #dgvOrderSetProblemLists thead tr th:first-child').removeClass('sorting_asc');


    },
    //Comments Update
    AddComments: function (ProblemListId) {
        var params = [];
        params["ParentCtrl"] = 'OrderSet_Problems';
        params["ProblemListId"] = ProblemListId;
        params["FromAdmin"] = "0";
        params["OrderSetId"] = OrderSet_Problems.params.OrderSetId;
        LoadActionPan('Clinical_ProblemListsComments', params);
    },
    //
    ActiveProblemSearch: function (objThis) {


        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        OrderSet_Problems.ProblemListsSearch();
    },

    ValidateProblemLists: function () {
        $('#' + OrderSet_Problems.params.PanelID + ' #frmOrderSetProblemLists')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ProblemName: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            OrderSet_Problems.ProblemListsSave();
        });
    },

    SaveFavToggelStatus: function () {

        var isFavListOpened = $('#' + OrderSet_Problems.params.PanelID + " #frmOrderSetProblemLists #favSectionDiv").hasClass("toggled");
        EMRUtility.insertUpdateFavListStatus(OrderSet_Problems.FavListName, isFavListOpened);
    },

    ProblemListsSave: function () {
        var tPlist = $("#" + OrderSet_Problems.params.PanelID + " #frmOrderSetProblemLists #txtProblems").val();

        if (tPlist != null && tPlist.indexOf(' - ') > -1) {
            var strArray = tPlist.split(' - ');
            $("#" + OrderSet_Problems.params.PanelID + " #frmOrderSetProblemLists #txtProblems").val(strArray[strArray.length - 1].trim());
        }
        $("#" + OrderSet_Problems.params.PanelID + " #frmOrderSetProblemLists").bootstrapValidator('revalidateField', 'ProblemName');
        if ($("#" + OrderSet_Problems.params.PanelID + " #frmOrderSetProblemLists #txtProblems").val() != "") {
            var strMessage = "";
            $("#" + OrderSet_Problems.params.PanelID + " #hfOrderSetId").val(OrderSet_Problems.params.OrderSetId);
            var self = $("#" + OrderSet_Problems.params.PanelID + " #frmOrderSetProblemLists");
            var myJSON = self.getMyJSONByName();
            var problem = $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").val();

            var problemExists = false;
            var ICDCodeANDDescription = "";
            var icdCode = "";
            var icdDescription = "";
            var problemCode = "";
            if (problem) if (problem.indexOf('-') > 1) problemCode = problem.split('-')[0].trim();

            var ArrayProblems = new Array();
            var columnIndex = $(" #dgvProblemLists thead tr th:contains('ICD (Diagnosis)')").index() + 1;

            $("#" + OrderSet_Problems.params.PanelID + " #dgvProblemLists tbody tr td:nth-child(" + columnIndex + ")").each(function (i) {

                if ($(this).text()) {
                    ICDCodeANDDescription = $(this).text();
                    if (ICDCodeANDDescription.indexOf('-') > 1) {
                        icdCode = ICDCodeANDDescription.split('-')[0].trim();
                        icdDescription = ICDCodeANDDescription.split('-')[1].trim();
                        ArrayProblems.push(icdCode);
                    }
                }
                else
                    if ($("#" + OrderSet_Problems.params.PanelID + " #dgvProblemLists tbody tr").text().indexOf(problem) > -1)
                        problemExists = true;
            });

            var IsThisActive = $('#' + OrderSet_Problems.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive') == null ? "1" : "0";

            OrderSet_Problems.SearchProblemList(null, null, null, IsThisActive).done(function (response) {
                var qresponse = JSON.parse(response);
                if (qresponse.status != false) {
                    if (qresponse.ProblemListLoad_JSON) {
                        response = JSON.parse(qresponse.ProblemListLoad_JSON);
                        $.each(response, function (i, item) {
                            ArrayProblems.push(item.ICD10);
                        });

                        if (ArrayProblems.includes(problemCode))
                            problemExists = true;

                        if (problemExists == true) {
                            utility.DisplayMessages("Problem Already Exists.", 3);
                            return;
                        }
                    }
                }
                //End 25-08-2016 Humaira Yousaf to preventing duplicate problem for referrals
                //return false;
                if (OrderSet_Problems.params.mode == "Add") {
                    var hfProblemText = $("#" + OrderSet_Problems.params.PanelID + " #hfIMOProblem").val();
                    var changesProblemText = $("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val();
                    if (hfProblemText.toString() == changesProblemText.toString()) {

                        OrderSet_Problems.SaveProblemLists(myJSON).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                OrderSet_Problems.SaveFavToggelStatus();
                                OrderSet_Problems.LastSctBaseSearch = "";
                                $('#' + OrderSet_Problems.params.PanelID + ' #ulFavCompliantDisease li').remove();


                                if (Clinical_ProblemLists.params.FromOrderDetail != null && Clinical_ProblemLists.params.FromOrderDetail == "1") {
                                    OrderSet_Problems.UnLoad();
                                }
                                var FromOrderDetail = false;
                                try {
                                    FromOrderDetail = (OrderSet_Problems.params.FromOrderDetail != null && OrderSet_Problems.params.FromOrderDetail == "1");
                                } catch (ex) {
                                    console.log(ex);
                                }
                                if (!FromOrderDetail)
                                    OrderSet_Problems.ProblemListsSearch();
                                

                                utility.DisplayMessages(response.message, 1);
                                $('#' + OrderSet_Problems.params.PanelID + ' #frmOrderSetProblemLists').resetAllControls(null);
                                $("#" + OrderSet_Problems.params.PanelID + " #hfOrderSetId").val(OrderSet_Problems.params.OrderSetId);
                                $('#' + OrderSet_Problems.params.PanelID + ' #frmOrderSetProblemLists').data('bootstrapValidator').enableFieldValidators('ProblemName', true);
                                $("#" + OrderSet_Problems.params.PanelID + " #txtComments,#txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate").prop("disabled", true);
                                $("#" + OrderSet_Problems.params.PanelID + " #ulProblemDisease").empty();
                                $('#' + OrderSet_Problems.params.PanelID + ' #frmOrderSetProblemLists').data('serialize', $('#' + OrderSet_Problems.params.PanelID + ' #frmOrderSetProblemLists').serialize());



                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });

                    } else {
                        utility.DisplayMessages("Please Enter Valid Problem", 3);
                        $("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val('');
                        $('#' + OrderSet_Problems.params.PanelID + ' #frmOrderSetProblemLists').data('bootstrapValidator').enableFieldValidators('ProblemName', true);
                    }

                }
                else if (OrderSet_Problems.params.mode == "Edit") {
                    OrderSet_Problems.UpdateProblemLists(myJSON, OrderSet_Problems.params.ProblemListId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            OrderSet_Problems.LastSctBaseSearch = "";
                            utility.DisplayMessages(response.message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                }
            });
        }
    },

    UpdateProblemLists: function (ProblemListsData, ProblemListId) {

        var isactive = null;
        isactive = $('#' + OrderSet_Problems.params.PanelID + ' #pnlOrderSetProblemLists_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(ProblemListsData);
        if (objData.OrderSetId == '') {
            objData.OrderSetId = OrderSet_Problems.params.OrderSetId;
        }
        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_VITALS";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "OrderSet", "Vitals");

    },

    SaveProblemLists: function (ProblemListsData) {

        var objData = JSON.parse(ProblemListsData);
        if (objData.OrderSetId == '' || typeof objData.OrderSetId == 'undefined') {
            objData.OrderSetId = OrderSet_Problems.params.OrderSetId;
        }
        /*objData["ICD9"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("icd9code");
        objData["ICD10"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("icd10code");
        objData["ICD9_Description"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("icd9desc");
        objData["ICD10_Description"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("icd10desc");
        objData["SNOMEDID"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("snomedcode");
        objData["SNOMED_DESCRIPTION"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("snomeddesc");*/
        //-------------------------
        if (OrderSet_Problems.icdsValues != null && typeof OrderSet_Problems.icdsValues["ICD9"] != 'undefined'
            && OrderSet_Problems.icdsValues["ICD9"] != null && OrderSet_Problems.icdsValues["ICD9"] != '') {
            objData["ICD9"] = OrderSet_Problems.icdsValues["ICD9"];
            objData["ICD10"] = OrderSet_Problems.icdsValues["ICD10"];
            objData["ICD9_Description"] = OrderSet_Problems.icdsValues["ICD9_Description"];
            objData["ICD10_Description"] = OrderSet_Problems.icdsValues["ICD10_Description"];
            objData["SNOMEDID"] = OrderSet_Problems.icdsValues["SNOMEDID"];
            objData["SNOMED_DESCRIPTION"] = OrderSet_Problems.icdsValues["SNOMED_DESCRIPTION"];
        }
        else {
            objData["ICD9"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("icd9code");
            objData["ICD10"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("icd10code");
            objData["ICD9_Description"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("icd9desc");
            objData["ICD10_Description"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("icd10desc");
            objData["SNOMEDID"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("snomedcode");
            objData["SNOMED_DESCRIPTION"] = $('#' + OrderSet_Problems.params.PanelID + " #ulProblemDisease li").attr("snomeddesc");
        }

        //Start for wrong snomed code
        if (OrderSet_Problems.LastSctBaseSearch != "") {


            if (OrderSet_Problems.LastSctBaseSearch == "981000124106" && objData["SNOMEDID"] != "981000124106") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "981000124106")
                }
                objData["SNOMEDID"] = "981000124106";
                objData["SNOMED_DESCRIPTION"] = "Moderate left ventricular systolic dysfunction";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "385093006" && objData["SNOMEDID"] != "385093006") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "385093006")
                }
                objData["SNOMEDID"] = "385093006";
                objData["SNOMED_DESCRIPTION"] = "Community Acquired Pneumonia";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "5281000124103" && objData["SNOMEDID"] != "5281000124103") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "5281000124103")
                }
                objData["SNOMEDID"] = "5281000124103";
                objData["SNOMED_DESCRIPTION"] = "Persistent asthma";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "195967001" && objData["SNOMEDID"] != "195967001") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "195967001")
                }
                objData["SNOMEDID"] = "195967001";
                objData["SNOMED_DESCRIPTION"] = "Asthma";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "233604007" && objData["SNOMEDID"] != "233604007") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "233604007")
                }
                objData["SNOMEDID"] = "233604007";
                objData["SNOMED_DESCRIPTION"] = "Pneumonia";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "59621000" && objData["SNOMEDID"] != "59621000") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "59621000")
                }
                objData["SNOMEDID"] = "59621000";
                objData["SNOMED_DESCRIPTION"] = "Essential hypertension";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "95891005" && objData["SNOMEDID"] != "95891005") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "95891005")
                }
                objData["SNOMEDID"] = "95891005";
                objData["SNOMED_DESCRIPTION"] = "Flu-like symptoms";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "363746003" && objData["SNOMEDID"] != "363746003") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "363746003")
                }
                objData["SNOMEDID"] = "363746003";
                objData["SNOMED_DESCRIPTION"] = "Acute pharyngitis";
            }

            else if (OrderSet_Problems.LastSctBaseSearch == "53741008" && objData["SNOMEDID"] != "53741008") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "53741008")
                }
                objData["SNOMEDID"] = "53741008";
                objData["SNOMED_DESCRIPTION"] = "Coronary arteriosclerosis";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "266569009" && objData["SNOMEDID"] != "266569009") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "266569009")
                }
                objData["SNOMEDID"] = "266569009";
                objData["SNOMED_DESCRIPTION"] = "Benign prostatic hyperplasia";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "49436004" && objData["SNOMEDID"] != "49436004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "49436004")
                }
                objData["SNOMEDID"] = "49436004";
                objData["SNOMED_DESCRIPTION"] = "Atrial fibrillation";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "230572002" && objData["SNOMEDID"] != "230572002") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "230572002")
                }
                objData["SNOMEDID"] = "230572002";
                objData["SNOMED_DESCRIPTION"] = "Diabetic neuropathy";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "5281000124103" && objData["SNOMEDID"] != "5281000124103") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "5281000124103")
                }
                objData["SNOMEDID"] = "5281000124103";
                objData["SNOMED_DESCRIPTION"] = "Persistent asthma";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "981000124106" && objData["SNOMEDID"] != "981000124106") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "981000124106")
                }
                objData["SNOMEDID"] = "981000124106";
                objData["SNOMED_DESCRIPTION"] = "Moderate left ventricular systolic dysfunction";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "426749004" && objData["SNOMEDID"] != "426749004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "426749004")
                }
                objData["SNOMEDID"] = "426749004";
                objData["SNOMED_DESCRIPTION"] = "Chronic atrial fibrillation";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "991000124109" && objData["SNOMEDID"] != "991000124109") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "991000124109")
                }
                objData["SNOMEDID"] = "991000124109";
                objData["SNOMED_DESCRIPTION"] = "Severe left ventricular systolic dysfunction";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "64109004" && objData["SNOMEDID"] != "64109004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "64109004")
                }
                objData["SNOMEDID"] = "64109004";
                objData["SNOMED_DESCRIPTION"] = "Costal Chondritis";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "195967001" && objData["SNOMEDID"] != "195967001") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "195967001")
                }
                objData["SNOMEDID"] = "195967001";
                objData["SNOMED_DESCRIPTION"] = "Asthma";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "53741008" && objData["SNOMEDID"] != "53741008") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "53741008")
                }
                objData["SNOMEDID"] = "53741008";
                objData["SNOMED_DESCRIPTION"] = "Coronary arteriosclerosis";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "266569009" && objData["SNOMEDID"] != "266569009") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "266569009")
                }
                objData["SNOMEDID"] = "266569009";
                objData["SNOMED_DESCRIPTION"] = "Benign prostatic hyperplasia";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "49436004" && objData["SNOMEDID"] != "49436004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "49436004")
                }
                objData["SNOMEDID"] = "49436004";
                objData["SNOMED_DESCRIPTION"] = "Atrial fibrillation";
            }
            else if (OrderSet_Problems.LastSctBaseSearch == "230572002" && objData["SNOMEDID"] != "230572002") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "230572002")
                }
                objData["SNOMEDID"] = "230572002";
                objData["SNOMED_DESCRIPTION"] = "Diabetic neuropathy";
            }


        }
        //End for wrong snomed code
        objData["ICD9_Description"] = OrderSet_Problems.RemoveDashSignFromStr(objData["ICD9_Description"]);
        objData["ICD10_Description"] = OrderSet_Problems.RemoveDashSignFromStr(objData["ICD10_Description"]);
        objData["IMOProblem"] = OrderSet_Problems.RemoveDashSignFromStr(objData["IMOProblem"]);
        objData["ProblemName"] = OrderSet_Problems.RemoveDashSignFromStr(objData["ProblemName"]);

        //--------------------------
        objData["commandType"] = "SAVE_PROBLEMLIST";
        objData["IsChiefComplaint"] = 0;


        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "OrderSet", "ProblemList");
    },
    RemoveDashSignFromStr: function (str) {
        if (str != null && str.indexOf(' - ') > -1) {
            var strArray = str.split(' - ');
            return strArray[strArray.length - 1].trim();
        } else {
            return str;
        }

    },
    NoKnownProblem: function () {
        var strMessage = "";
        var self = $("#" + OrderSet_Problems.params.PanelID + " #frmOrderSetProblemLists");
        var myJSON = '{}';
        OrderSet_Problems.SaveProblemLists(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#pnlOrderSetProblemLists_Result #btnNoKnownProblems").css("display", "none");
                utility.DisplayMessages(response.message, 1);
                OrderSet_Problems.ProblemListsSearch();
                $('#' + OrderSet_Problems.params.PanelID + ' #frmOrderSetProblemLists').resetAllControls(null);
            }
            else {
                //utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    domReadyFunc: function () {

        $(document).ready(function () {

            $('#' + OrderSet_Problems.params.PanelID + ' .toggleHorSmallLeft section').unbind('click').bind("click", function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));

            });

            //for autocomplete zIndex fix
            $('#' + OrderSet_Problems.params.PanelID + " #txtProblems").on("autocompleteopen", function (event, ui) {
                if ($(this).closest(".modal-dialog").length == 0)
                    $(this).autocomplete('widget').zIndex("1018");
            });
        });

    },

    ResetDiagnosis: function () {

        if ($("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val() == "") {
            $('#' + OrderSet_Problems.params.PanelID + ' #frmOrderSetProblemLists').resetAllControls(null);
            $("#" + OrderSet_Problems.params.PanelID + " #txtComments,#txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate").prop("disabled", true);
        }
    },
    BindICD9AutoComplete: function (element) {
        if ($(element).val().length > 3) {
            if ($(element).val().substring(0, 3).toLowerCase() == "sct") {
                OrderSet_Problems.LastSctBaseSearch = $(element).val().substring(3, $(element).val().length);
            }
            else {
                OrderSet_Problems.LastSctBaseSearch = "";
            }
        }
        else {
            OrderSet_Problems.LastSctBaseSearch = "";
        }
        $('#' + OrderSet_Problems.params.PanelID + ' #txtProblems').attr('data-popupunload', 'false');
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "OrderSet_Problems", null, false);
    },

    OpenSearchPopup: function () {
        var controlToLoad = "";
        controlToLoad = "Admin_IMOICD";
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_Problems";
        params["RefCtrl"] = "txtProblems";
        HiddenCtrl = 'hfICD1-1,hfICDDescription1-1,hfICD101-1,hfICD10Description1-1,hfSNOMED1-1,hfSNOMEDDescription1-1';
        params["RefHiddenCtrl"] = HiddenCtrl;
        $('#' + OrderSet_Problems.params.PanelID + ' #txtProblems').attr('data-popupunload', 'true');
        LoadActionPan(controlToLoad, params);
    },
    ShowHistory: function (problemListId) {
        EMRUtility.showCurrentItemHistory(OrderSet_Problems.params.PanelID, null, problemListId, "ProblemList", OrderSet_Problems.params.OrderSetId, "OrderSet_Problems", null);

    },
    BindFavProblems: function () {
        var FavoriteListId = $('#' + OrderSet_Problems.params.PanelID + ' #ddlFavProblems').val();
        if (FavoriteListId != "") {
            Favorite_Problems.searchFavoriteList_ICD_DBCall(null, FavoriteListId, null, null).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.FavoriteListICDCount > 0) {
                        $('#' + OrderSet_Problems.params.PanelID + " #frmOrderSetProblemLists #ulFavCompliantDisease li").remove();
                        var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
                        var li = "";
                        $.each(FavoriteListJSON, function (i, item) {
                            if (typeof item.ICD9Code == 'undefined' || item.ICD9Code == null) { item.ICD9Code = ''; }
                            if (typeof item.ICD10Code == 'undefined' || item.ICD10Code == null) { item.ICD10Code = ''; }
                            if (typeof item.SNOMEDID == 'undefined' || item.SNOMEDID == null) { item.SNOMEDID = ''; }
                            if (typeof item.ICD10CodeDescription == 'undefined' || item.ICD10CodeDescription == null) { item.ICD10CodeDescription = ''; }
                            var diagnosis = item.ICD10Code + " - " + item.ICD10CodeDescription;
                            var ICD9Code = "" + item.ICD9Code + "";
                            var ICD10Code = "" + item.ICD10Code + "";
                            var ICD9CodeDescription = "" + item.ICD9CodeDescription + "";
                            var ICD10CodeDescription = "" + item.ICD10CodeDescription + "";
                            var SNOMEDID = "" + item.SNOMEDID + "";
                            var SNOMEDDescription = "" + item.SNOMEDDescription + "";


                            li += "<li  id=" + item.FavoriteListICDId + " onclick='OrderSet_Problems.PopulateFields(this,\"" + diagnosis + "\",\"" + ICD9Code + "\",\"" + ICD10Code + "\",\"" + ICD9CodeDescription + "\",\"" + ICD10CodeDescription + "\",\"" + SNOMEDID + "\",\"" + SNOMEDDescription + "\");' ><a href='#' class='pr-sm'>" + item.ICD10Code + " - " + item.ICD10CodeDescription + "</a></li>";
                        });
                        $('#' + OrderSet_Problems.params.PanelID + ' #ulFavCompliantDisease').append(li);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        //on change, set controls to default state.
        $("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val('');
        $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").val('');
        if ($("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val() == "") {
            $("#" + OrderSet_Problems.params.PanelID + " #txtComments,#txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate").prop("disabled", true);
        }
        if ($('#' + OrderSet_Problems.params.PanelID + ' #ddlFavProblems').val() == '' || $('#' + OrderSet_Problems.params.PanelID + ' #ddlFavProblems').val() == '- Select -') {
            $('#' + OrderSet_Problems.params.PanelID + ' #ulFavCompliantDisease li').remove();
        }
    },
    PopulateFields: function (cntrl, diagnosis, ICD9Code, ICD10Code, ICD9CodeDescription, ICD10CodeDescription, SNOMEDID, SNOMEDDescription) {
        $('#' + OrderSet_Problems.params.PanelID + ' #txtProblems').val(ICD10CodeDescription);
        $('#' + OrderSet_Problems.params.PanelID + ' #txtDiagnosis').val(diagnosis);

        var lii = "<li icd9Code=\"" + ICD9Code + "\" icd9Desc=\"" + ICD9CodeDescription + "\" icd10Code=\"" + ICD10Code + "\" icd10Desc=\"" + ICD10CodeDescription + "\" snomedCode=\"" + SNOMEDID + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#' class='pr-sm'>" + ICD10CodeDescription + "</a></li>"
        $('#' + OrderSet_Problems.params.PanelID + ' #ulProblemDisease').html(lii);

        if (typeof $("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val() != 'undefined'
            && $("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val() != null
            && $("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val() != ""
            && typeof $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").val() != 'undefined'
            && $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").val() != null
            && $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").val() != "") {
            $("#" + OrderSet_Problems.params.PanelID + " #txtComments,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate").prop("disabled", false);
            $('#hfIMOProblem').val(ICD10CodeDescription);

            $("#" + OrderSet_Problems.params.PanelID + " #frmOrderSetProblemLists").bootstrapValidator('revalidateField', 'ProblemName');
        }
    },

    //Row Functions
    rowSave: function ($row, obj) {
        var _self = obj,
        $actions,
        values = [];

        if ($row.hasClass('adding')) {
            $row.removeClass('adding');
        }

        values = $row.find('td').map(function () {

            var $this = $(this);

            if ($this.hasClass('expand')) {
                return '<a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
            }
            else if ($this.hasClass('actions')) {

                return _self.datatable.cell(this).data();
            }
            else if ($this.hasClass('ddl')) {
                return $.trim($this.find('select').val());

            } else {
                $obj_ = $this.find('input');

                if ($obj_.attr('type') == "checkbox") {
                    if ($obj_.prop('checked'))
                        return $.trim("True");
                    else
                        return $.trim("False");
                }
                else
                    return $.trim($obj_.val());
            }
        });

        var id = $row.attr("id");

        var myJSON = $row.getMyJSONByName();
        var NotesId = $row.attr("problemlistnotesid");
        var objData = JSON.parse(myJSON);
        $row.find("select[id*=ddlDiagnosis] option").each(function () {
            var opVal = $(this).val();
            var selVal = objData["Description"];
            if ($(this).val() == objData["Description"]) {
                objData["ICD9"] = $(this).attr("icd9code");
                objData["ICD10"] = $(this).attr("icd10code");
                objData["ICD9_Description"] = $(this).attr("icd9desc");
                objData["ICD10_Description"] = $(this).attr("icd10desc");
                objData["SNOMEDID"] = $(this).attr("snomedcode");
                objData["SNOMED_DESCRIPTION"] = $(this).attr("snomeddesc");
                return false;
            }

        });

        myJSON = JSON.stringify(objData);

        if (id && id > 0) {
            //Edit Record

            OrderSet_Problems.UpdateProblemListsRow(myJSON, id).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    OrderSet_Problems.ProblemListsSearch();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }

    },

    rowDetail: function ($row, ClassName) {
        var currentProblemListId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProblemListId > 0) {
            OrderSet_Problems.ShowHistory(currentProblemListId);
        }
    },

    rowHistory: function ($row, ClassName) {
        var currentProblemListId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProblemListId > 0) {
            OrderSet_Problems.ShowHistory(currentProblemListId);
        }
    },
    rowAdd: function () {


        EditableGrid.rowAdd();


    },

    rowRemove: function ($row, obj) {

        var id = $row.attr("id");

        utility.myConfirm('1', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var description;
                if ($row.find("td:nth-child(4)").html() != "") {
                    description = $row.find("td:nth-child(4)").html();
                }
                else {
                    description = $row.find("td:nth-child(3)").html();
                }
                OrderSet_Problems.DeleteProblemList(selectedValue, description, $row.find("td:nth-child(7)").html()).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        if ($row.hasClass('adding')) {
                        }
                        var _self = obj;
                        _self.datatable.row($row.get(0)).remove().draw();

                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                    OrderSet_Problems.ProblemListsSearch();

                });
            }
        }, function () { },
            '1'
        );


    },

    rowInactive: function ($row, obj) {
        var strMessage = "";
        var id = $row.attr("id");

        var IsActive = null;
        IsActive = $('#' + OrderSet_Problems.params.PanelID + ' #pnlOrderSetProblemLists_Result #divSwitch #switchActive').attr('isactive');       
        utility.myConfirm('3', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var IsActiveRecord = null;
                IsActiveRecord = $('#' + OrderSet_Problems.params.PanelID + ' #pnlOrderSetProblemLists_Result #divSwitch #switchActive').attr('isactive');
                if (IsActiveRecord == "1") {
                    var params = [];
                    var PanelID = "";
                    params["ParentCtrl"] = "OrderSet_Problems";
                    PanelID = OrderSet_Problems.params.PanelID;

                    params["ProblemListId"] = selectedValue;
                    params["FromAdmin"] = "0";
                    params["OrderSetId"] = OrderSet_Problems.params.OrderSetId;
                    LoadActionPan('Clinical_ProblemListInActive', params, PanelID);
                } else {
                    IsActiveRecord = "0";
                    OrderSet_Problems.InActiveProblemList(selectedValue, null, null).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            OrderSet_Problems.ProblemListsSearch();
                        }
                        else {
                            utility.DisplayMessages(response.message, 1);
                        }
                    });
                }
            }
        }, function () { },
              '3', null, null, null, IsActive
        );

    },

    InActiveProblemList: function (ProblemListId, comments, endDate) {
        var IsCheckedIn = null;
        IsCheckedIn = $('#' + OrderSet_Problems.params.PanelID + ' #pnlOrderSetProblemLists_Result #divSwitch #switchActive').attr('isactive');

        var IsActiveRecord = null;
        IsActiveRecord = $('#' + OrderSet_Problems.params.PanelID + ' #pnlOrderSetProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if (IsActiveRecord == "1")
            IsActiveRecord = "0";
        else if (IsActiveRecord == "0")
            IsActiveRecord = "1";

        var orderSetId = OrderSet_Problems.params.OrderSetId;

        var objData = new Object();
        objData["ProblemListId"] = ProblemListId;
        objData["OrderSetId"] = orderSetId;
        objData["InActiveChkBoxValue"] = null;
        objData["InActiveReason"] = null;
        objData["EndDate"] = null;
        objData["IsActive"] = IsCheckedIn;
        objData["IsActiveRecord"] = IsActiveRecord;
        objData["commandType"] = "INACTIVE_PROBLEMLIST";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "OrderSet", "ProblemList");

    },

    rowCancel: function ($row, obj) {
        var _self = obj,
            $actions,
            i,
            data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                _self.rowSetActionsDefault($row);
            }

            _self.datatable.draw();
        }
    },

    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },

    rowExpand: function ($row, obj) {
        var _self = obj,
            $actions,
            values = [];
        var row = _self.datatable.row($row);
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
        }
        else {
            $row.find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
            if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
            }
        }
    },

    ShowHideEditableGridRows: function (isShow) {

        var VitalsGridId = "#" + OrderSet_Problems.params.PanelID + " #dgvOrderSetProblemLists";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = OrderSet_Problems.EditableGrid.datatable.row(parentRow);

            if (isShow == true) {

                row.child.show();
                $(parentRow).find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");

            }
            else {

                $(parentRow).find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();

            }

        });

    },

    // end editable grid functions
    UpdateProblemListsRow: function (ProblemListData, ProblemListId) {

        var isactive = null;
        isactive = $('#' + OrderSet_Problems.params.PanelID + ' #pnlOrderSetProblemLists_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(ProblemListData);
        objData["ProblemListId"] = ProblemListId;
        objData["Comments"] = objData["hfComments"];
        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_PROBLEMLIST";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProblemList");

    },
    //
    buildHistoryRows: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems) {
        var row = OrderSet_Problems.EditableGrid.datatable.row(CurrentRow);
        if (arrChildItems != null && arrChildItems.length > 0) {
            var CurrentRowchilds = $();
            $.each(arrChildItems, function (i, item) {
                var currentChildRow = $("#" + CurrentRow.attr("id")).clone();
                currentChildRow.attr("id", "Child" + item.ProblemId);
                currentChildRow.attr("parentvitalid", ParentRowId);
                currentChildRow.addClass("childRow-bg");
                $(currentChildRow).find("td:nth-child(1)").html("");
                $(currentChildRow).find("td:nth-child(2)").html("");
                $(currentChildRow).find("td:nth-child(3)").html("");
                CurrentRowchilds = CurrentRowchilds.add(currentChildRow);
            });
            row.child(CurrentRowchilds).show();
            setTimeout(function () {
                row.child.hide();
            }, 100);

        }
        else {
            $(CurrentRow).find("td:nth-child(1)").html("");
        }

        return row.child();
    },
    DeleteProblemList: function (ProblemListId, Description, StartDate) {

        var objData = new Object();
        objData["ProblemListId"] = ProblemListId;
        objData["commandType"] = "DELETE_PROBLEMLIST";
        objData["OrderSetId"] = OrderSet_Problems.params.OrderSetId;
        objData["Description"] = Description;
        objData["StartDate"] = StartDate;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProblemList");
    },

    AddComments: function (ProblemListId) {

        var PanelID = "";
        params["ParentCtrl"] = "OrderSet_Problems";
        PanelID = OrderSet_Problems.params.PanelID;

        params["ProblemListId"] = ProblemListId;
        params["FromAdmin"] = "0";
        params["OrderSetId"] = OrderSet_Problems.params.OrderSetId;
        LoadActionPan('Clinical_ProblemListsComments', params, PanelID);
    },
    //----------------------------------------------------//
    //-------------Problems Functions END---------------//
    //----------------------------------------------------//

    UnLoad: function () {
        if (OrderSet_Problems.params != null && OrderSet_Problems.params.ParentCtrl && OrderSet_Problems.params.ParentCtrlPanelID) {
            UnloadActionPan(OrderSet_Problems.params.ParentCtrl, "OrderSet_Problems", null, OrderSet_Problems.params.ParentCtrlPanelID);
        }
        else if (OrderSet_Problems.params != null && OrderSet_Problems.params.ParentCtrl) {
            if (OrderSet_Problems.params.ParentCtrl == "OrderSet_LabOrderDetails") {
                UnloadActionPan(OrderSet_Problems.params.ParentCtrl, "OrderSet_Problems");
                OrderSet_LabOrderDetails.loadProblemList();
            }
            else if (OrderSet_Problems.params.ParentCtrl == "OrderSet_ProcedureOrderDetails") {
                UnloadActionPan(OrderSet_Problems.params.ParentCtrl, "OrderSet_Problems");
                OrderSet_ProcedureOrderDetails.loadProblemList();
            }
            else if (OrderSet_Problems.params.ParentCtrl == "OrderSet_RadiologyOrderDetails") {
                UnloadActionPan(OrderSet_Problems.params.ParentCtrl, "OrderSet_Problems");
                OrderSet_RadiologyOrderDetails.loadProblemList();
            }
            else {
                UnloadActionPan(OrderSet_Problems.params.ParentCtrl, "OrderSet_Problems");
            }
        }
        else {
            UnloadActionPan(null, "OrderSet_Problems");
        }
        var FromOrderDetail = false;
        try {
            FromOrderDetail = (OrderSet_Problems.params.FromOrderDetail != null && OrderSet_Problems.params.FromOrderDetail == "1");
        } catch (ex) {
            console.log(ex);
        }

        if (!FromOrderDetail) {
            OrderSet_ProblemListGrid.ProblemListsSearch();
        }
        
        Clinical_OrderSetDetails.LoadOrderSetPatientEducation();
        //AST - 74 BY:MAhmad
        Clinical_OrderSetDetails.loadProblemLookUp();
        //AST - 74 BY:MAhmad
    }
}