OrderSet_ProblemListGrid = {
    params: [],
    EditableGrid: null,
    myGrid: null,
    IsNotes: false,
    ProblemListsSearch: function (ProblemListId, PageNo, rpp, mode, IsNotes) {
        OrderSet_ProblemListGrid.IsNotes = IsNotes;
        OrderSet_ProblemListGrid.SearchProblemList(ProblemListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProblemListsOS th#SelectRecord").remove();
                OrderSet_ProblemListGrid.responseSearch(response, null, null, mode);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    responseSearch: function (response, PageNo, rpp, mode) {
        OrderSet_ProblemListGrid.ProblemListGridLoadNew(response, mode);
        var TableControl = Clinical_OrderSetDetails.params.PanelID + " #dgvProblemListsOS";
        var PagingPanelControlID = Clinical_OrderSetDetails.params.PanelID + " #dgvProblemLists_PagingOS";
        var ClassControlName = "OrderSet_ProblemListGrid";
        var PagesToDisplay = 5;
        var iTotalDisplayRecords = response.iTotalDisplayRecords;
        setTimeout(CreatePagination(response.ProblemListCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
            OrderSet_ProblemListGrid.ProblemListsSearch(PrimaryID, PageNumber, ResultPerPage);
        }), 10);
    },
    SearchProblemList: function (ProblemListId, PageNumber, RowsPerPage) {
        var IsCheckedIn = null;
        IsCheckedIn = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProblemLists_ResultOS #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["IsActive"] = IsCheckedIn;
        objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        objData["NoteId"] = 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProblemList");

    },

    ProblemListGridLoadNew: function (response, mode) {
        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS thead tr #SelectRecord").length == 0 && Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox" name="SelectCheckBoxProbList" id="chkHeaderProblemsList" onchange="OrderSet_ProblemListGrid.checkUncheckAllProblemsList(this);"  class="input-block" coltype="checkbox"/> </th>');
        }

        // get Actions
        var actions = "";
        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProblemListsOS tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, "#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS");
                }
            }
        });
        if ($.fn.dataTable.isDataTable("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS")) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS").dataTable().fnClearTable();
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS").dataTable().fnDestroy();
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS tbody").find("tr").remove();
        }
        else {
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == 'dgvProblemListsOS') {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS tbody").find("tr").remove();
                    $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS").parent().parent().find('div.row').remove();
                }
            });
        }
        var totalProblems = 0;
        if (response.ProblemListCount > 0) {
            totalProblems = response.ProblemListCount;
            var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
            var ProblemListHistoryLoadJSONData = JSON.parse(response.ProblemListHistoryLoad_JSON);

            if ($.fn.dataTable.isDataTable("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS")) {
                $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS").dataTable().fnDestroy();
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
                        $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(searchstr, "Clinical_OrderSetDetails", 2);
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
                        var commentsMethod = "OrderSet_ProblemListGrid.AddComments('" + item.ProblemListId + "');";
                        //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                        comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                    }
                }
                var ProblemListId = item.ProblemListId;
                var ChildHistory_ProblemList = $.grep(ProblemListHistoryLoadJSONData, function (n, i) {
                    return n.ProblemId == ProblemListId;
                });


                //AST-14 BY:MAhmad
                if (item.ProblemName == "No Known Problems") {
                    $row.append('<td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + item.ProblemName + $infoButtonrow + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none">' + item.Comments + '</td>');
                }
                else {
                    //adding checkboxes column and disabling that row, if problem list already binded with notes
                    var SelectionCheckBoxColumn = "";
                    if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                         SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="OrderSet_ProblemListGrid.enableAddProbList(this);" id="' + item.ProblemListId + '" name="SelectCheckBoxProbList"  class="input-block text-center"/></td>';
                    }


                    if (ChildHistory_ProblemList.length > 0) {
                        $row.append(SelectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions Actions_" id="' + item.ProblemListId + '" >' + actions + '</td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none">' + item.Comments + '</td>');
                    } else {
                        $row.append(SelectionCheckBoxColumn + '<td></td><td class="actions Actions_" id="' + item.ProblemListId + '" >' + actions + '</td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none">' + item.Comments + '</td>');
                    }
                }
                //AST-14 BY:MAhmad
                if (item.IsActive == "True") {
                    // $($row).find('a.edit-row').removeAttr('disabled', false);
                    $($row).find('a.edit-row').removeClass('disableAll')
                } else {
                    $($row).find('a.edit-row').addClass('disableAll')
                    //  $($row).find('a.edit-row').attr('disabled', 'disabled')
                }


                $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProblemListsOS tbody").last().append($row);
                if (totalProblems > 1) {
                    $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProblemListsOS tbody tr').each(function () {
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
                            var commentsMethod = "OrderSet_ProblemListGrid.AddComments('" + item.ProblemListId + "');";
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
                        var currentHistory = '<tr class="childRow-bg"><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + IsActiveText + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + colorChild + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none">' + item.Comments + '</td></tr>';
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
            var PanelGrid = "#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS";
            var GridId = "#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS";

            // Below line comment out inorder to remove duplicate grid search
            if (OrderSet_ProblemListGrid.myGrid != null) {
                if ($.fn.dataTable.isDataTable(OrderSet_ProblemListGrid.myGrid)) {
                    OrderSet_ProblemListGrid.myGrid.$table.dataTable().fnDestroy();
                } else {
                    OrderSet_ProblemListGrid.myGrid = null;
                }
                if ($.fn.dataTable.isDataTable("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS")) {
                    $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProblemLists_ResultOS #dgvProblemListsOS").dataTable().fnDestroy();
                }
            }
            // Below line comment out inorder to remove duplicate grid search
            OrderSet_ProblemListGrid.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, OrderSet_ProblemListGrid, 0, false, true, false, true, false, null);
            if (!($('#pnlProblemLists_ResultOS').closest('section').hasClass('active')))
                $(PanelGrid).css("display", "none");
            //rander childs
            $.each(arraTemp, function (i, item) {

                if (OrderSet_ProblemListGrid.myGrid != null) {
                    var row = OrderSet_ProblemListGrid.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });
            //Sorting removed from first column of grid
            if ($('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProblemListsOS').length > 0) {
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProblemListsOS').dataTable().fnSettings().aoColumns[0].bSortable = false;
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProblemListsOS').dataTable().fnSort([[8, "desc"]]);
            }
            //Sorting removed from first column of grid

        }
        else {
            if ($('#' + Clinical_OrderSetDetails.params.PanelID + ' div#divShowHistory').hasClass("hidden") == false) {
                $('#' + Clinical_OrderSetDetails.params.PanelID + ' div#divShowHistory').addClass("hidden");
            }

            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProblemLists_ResultOS #dgvProblemListsOS').DataTable({
                "language": {
                    "emptyTable": "No Problem List Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
            });
            if (response.ProblemListCount == 0) {
                $("#pnlProblemLists_ResultOS #btnNoKnownProblems").css("display", "");
            } else {
                $("#pnlProblemLists_ResultOS #btnNoKnownProblems").hide();
            }

            /* End of Code for making No Known Problem List hyperlink inline with checkbox and search box.
            *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
            */

        }
        if (mode && mode == "View") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProblemListsOS").find('a').addClass('disableAll');
        }

        if (OrderSet_ProblemListGrid.IsNotes == true)
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProblemListsOS').find('td.Actions_').addClass('hidden');


        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProblemListsOS thead tr th:first-child').removeClass('sorting_asc');



    },
    checkUncheckAllProblemsList: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProblemListsOS tbody").find('input[type="checkbox"]').each(function () {
            OrderSet_ProblemListGrid.enableAddProbList(this);
        });
    },

    enableAddProbList: function (obj) {
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.OSProblems_ComponentIds) == -1) {
                Clinical_ProgressNote.OSProblems_ComponentIds.push(obj.id);
            }
            //if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
            //    var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id);
            //    if (index > -1) {
            //        Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
            //    }
            //}
        } else {
            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-283
            //$("#" + Clinical_ProgressNote.params.PanelID + ' #pnlProblemLists_Result #chkHeaderProblemsList').prop('checked', false);
            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-283
            var index = Clinical_ProgressNote.OSProblems_ComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.OSProblems_ComponentIds.splice(index, 1);
            }
            //if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
            //    Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id);
            //}
        }


    },
    //Comments Update
    AddComments: function (ProblemListId) {
        var params = [];
        params["ParentCtrl"] = 'Clinical_OrderSetDetails';
        params["ProblemListId"] = ProblemListId;
        params["FromAdmin"] = "0";
        params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
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

        OrderSet_ProblemListGrid.ProblemListsSearch();
    },
    ShowHistory: function (problemListId) {
        EMRUtility.showCurrentItemHistory(Clinical_OrderSetDetails.params.PanelID, null, problemListId, "ProblemList", Clinical_OrderSetDetails.params.OrderSetId, "OrderSet_ProblemListGrid", null);

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

            OrderSet_ProblemListGrid.UpdateProblemListsRow(myJSON, id).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    OrderSet_ProblemListGrid.ProblemListsSearch();
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
            OrderSet_ProblemListGrid.ShowHistory(currentProblemListId);
        }
    },

    rowHistory: function ($row, ClassName) {
        var currentProblemListId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProblemListId > 0) {
            OrderSet_ProblemListGrid.ShowHistory(currentProblemListId);
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
                OrderSet_ProblemListGrid.DeleteProblemList(selectedValue, description, $row.find("td:nth-child(7)").html()).done(function (response) {
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
                    OrderSet_ProblemListGrid.ProblemListsSearch();

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
        IsActive = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProblemLists_ResultOS #divSwitch #switchActive').attr('isactive');      
        utility.myConfirm('3', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var IsActiveRecord = null;
                IsActiveRecord = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProblemLists_ResultOS #divSwitch #switchActive').attr('isactive');
                if (IsActiveRecord == "1") {
                    var params = [];
                    var PanelID = "";
                    params["ParentCtrl"] = "Clinical_OrderSetDetails";
                    PanelID = Clinical_OrderSetDetails.params.PanelID;

                    params["ProblemListId"] = selectedValue;
                    params["FromAdmin"] = "0";
                    params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
                    LoadActionPan('Clinical_ProblemListInActive', params, PanelID);
                } else {
                    IsActiveRecord = "0";
                    OrderSet_ProblemListGrid.InActiveProblemList(selectedValue, null, null).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            OrderSet_ProblemListGrid.ProblemListsSearch();
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
        IsCheckedIn = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProblemLists_ResultOS #divSwitch #switchActive').attr('isactive');

        var IsActiveRecord = null;
        IsActiveRecord = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProblemLists_ResultOS #divSwitch #switchActive').attr('isactive');
        if (IsActiveRecord == "1")
            IsActiveRecord = "0";
        else if (IsActiveRecord == "0")
            IsActiveRecord = "1";

        var orderSetId = Clinical_OrderSetDetails.params.OrderSetId;

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

        var VitalsGridId = "#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProblemListsOS";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = OrderSet_ProblemListGrid.EditableGrid.datatable.row(parentRow);

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
        isactive = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProblemLists_ResultOS #divSwitch #switchActive').attr('isactive');

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
        var row = OrderSet_ProblemListGrid.EditableGrid.datatable.row(CurrentRow);
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
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["Description"] = Description;
        objData["StartDate"] = StartDate;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProblemList");
    },

    AddComments: function (ProblemListId) {

        var PanelID = "";
        params["ParentCtrl"] = "Clinical_OrderSetDetails";
        PanelID = Clinical_OrderSetDetails.params.PanelID;

        params["ProblemListId"] = ProblemListId;
        params["FromAdmin"] = "0";
        params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        LoadActionPan('Clinical_ProblemListsComments', params, PanelID);
    },
}