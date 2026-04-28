Treatment_ProblemListGrid = {
    params: [],
    EditableGrid: null,
    myGrid: null,
    ProblemListsSearch: function (ProblemListId, PageNo, rpp) {
        var dfd = $.Deferred();
        Treatment_ProblemListGrid.SearchProblemList(ProblemListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#" + Clinical_Treatment.params.PanelID + " #dgvProblemListsT th#SelectRecord").remove();
                $.when(Treatment_ProblemListGrid.responseSearch(response, PageNo, rpp)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd;
    },
    responseSearch: function (response, PageNo, rpp) {
        var dfd = $.Deferred();
        $.when(Treatment_ProblemListGrid.ProblemListGridLoadNew(response)).then(function () {
            dfd.resolve();
        });
        var TableControl = Clinical_Treatment.params.PanelID + " #dgvProblemListsT";
        var PagingPanelControlID = Clinical_Treatment.params.PanelID + " #dgvProblemLists_PagingT";
        var ClassControlName = "Treatment_ProblemListGrid";

        var RecordsPerPage = rpp != null ? rpp : 15;
        var CurrentPage = PageNo != null ? PageNo : 1;
        var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
        if (response.ProblemListCount > 0 && PageNo == null) {
            utility.GetCustomPaging("dgvProblemLists_PagingT", response.iTotalDisplayRecords, PagesToShow, "Treatment_ProblemListGrid", CurrentPage, RecordsPerPage);
            $('#pnlClinicalTreatment #pnlProblemLists_ResultT #dgvProblemLists_PagingT').css('display', 'inline');
        }
        var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
        var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
        $("#pnlClinicalTreatment #dgvProblemLists_PagingT #divShowingEntries").text(showingText);
        $("#pnlClinicalProgressNote #pnlClinicalTreatment #dgvProblemLists_PagingT li").each(function () {
            if ($(this).text() == CurrentPage) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        return dfd;
    },
    SearchProblemList: function (ProblemListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();

        objData["PatientId"] = Clinical_Treatment.params.PatientId;
        objData["IsActive"] = "1";
        objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        objData["NoteId"] = 0;

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");


    },
    ProblemListGridLoadNew: function (response, mode) {
        var dfd = $.Deferred();
        // get Actions
        var actions = "";
        $("#" + Clinical_Treatment.params.PanelID + " #dgvProblemListsT tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, "#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT");
                }
            }
        });

        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT").dataTable().fnClearTable();
        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT").dataTable().fnDestroy();
        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody").find("tr").remove();

        var totalProblems = 0;
        if (response.ProblemListCount > 0) {
            totalProblems = response.ProblemListCount;
            var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
            var ProblemListHistoryLoadJSONData = JSON.parse(response.ProblemListHistoryLoad_JSON);

            //tem array to hold rows and childs
            var arraTemp = [];

            $.each(ProblemListLoadJSONData, function (i, item) {
                var ProblemDescription = (item.ICD9 != "" ? item.ICD9 + " - " : "") + (item.ICD10 != "" ? item.ICD10 + " - " : "") + (item.ProblemName != "" ? item.ProblemName : "");
                var $infoButtonrow = "";
                if (item.Description != "") {
                    if (typeof item.Description !== 'undefined' && typeof item.Description.split(' - ')[1] !== 'undefined') {
                        var searchstr = item.Description.split('-')[0].trim();
                        if (item.Description.split(" - ")[2] != undefined) {
                            item.Description = item.Description.substring(item.Description.indexOf(" - ") + 2);
                        }
                        $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(searchstr, "Clinical_Treatment", 2);
                    }
                }
                var $row = $('<tr/>');
                $row.attr("onclick", "Clinical_Treatment.SelectProblem($(this)," + item.ProblemListId + ",\"" + ProblemDescription + "\",event)");
                $row.attr("id", item.ProblemListId);

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
                        var commentsMethod = "Treatment_ProblemListGrid.AddComments('" + item.ProblemListId + "');";
                        //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                        comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                    }
                }
                var ProblemListId = item.ProblemListId;
                var ChildHistory_ProblemList = $.grep(ProblemListHistoryLoadJSONData, function (n, i) {
                    return n.ProblemId == ProblemListId;
                });



                if (item.ProblemName == "No Known Problems") {
                    $row.append('<td class="actions" id="' + item.ProblemListId + '" ></td><td>' + item.ProblemName + $infoButtonrow + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none"></td>');
                }
                else {
                    $row.append('<td class="actions Actions_" id="' + item.ProblemListId + '" >' + actions + '</td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none">' + item.Comments + '</td>');

                }

                if (item.IsActive == "True") {
                    // $($row).find('a.edit-row').removeAttr('disabled', false);
                    $($row).find('a.edit-row').removeClass('disableAll')
                } else {
                    $($row).find('a.edit-row').addClass('disableAll')
                    //  $($row).find('a.edit-row').attr('disabled', 'disabled')
                }


                $("#" + Clinical_Treatment.params.PanelID + " #dgvProblemListsT tbody").last().append($row);
                if (totalProblems > 1) {
                    $('#' + Clinical_Treatment.params.PanelID + ' #dgvProblemListsT tbody tr').each(function () {
                        if ($(this).text().indexOf("No Known Problems") > -1) {
                            $(this).remove();
                        }
                    });
                }

                var CurrentRowchilds = $();
                arraTemp.push({ row: $row, childs: CurrentRowchilds });

            });

            //Inalize grid
            var PanelGrid = "#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT";
            var GridId = "#" + Clinical_Treatment.params.PanelID + " #dgvProblemListsT";

            // Below line comment out inorder to remove duplicate grid search
            if (Treatment_ProblemListGrid.myGrid != null) {
                if ($.fn.dataTable.isDataTable(Treatment_ProblemListGrid.myGrid)) {
                    Treatment_ProblemListGrid.myGrid.$table.dataTable().fnDestroy();
                } else {
                    Treatment_ProblemListGrid.myGrid = null;
                }
                if ($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT")) {
                    $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT").dataTable().fnDestroy();
                }
            }
            // Below line comment out inorder to remove duplicate grid search
            Treatment_ProblemListGrid.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Treatment_ProblemListGrid, 0, false, true, false, true, false, null);
            $(PanelGrid).css("display", "block");
            if (!($('#pnlProblemLists_ResultT').closest('section').hasClass('active')))
                $(PanelGrid).css("display", "none");
            //rander childs
            $.each(arraTemp, function (i, item) {

                if (Treatment_ProblemListGrid.myGrid != null) {
                    var row = Treatment_ProblemListGrid.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });
            //Sorting removed from first column of grid
            if ($('#' + Clinical_Treatment.params.PanelID + ' #dgvProblemListsT').length > 0) {
                $('#' + Clinical_Treatment.params.PanelID + ' #dgvProblemListsT').dataTable().fnSettings().aoColumns[0].bSortable = false;
                $('#' + Clinical_Treatment.params.PanelID + ' #dgvProblemListsT').dataTable().fnSort([[8, "desc"]]);
            }
            //Sorting removed from first column of grid

        }
        else {
            if ($('#' + Clinical_Treatment.params.PanelID + ' div#divShowHistory').hasClass("hidden") == false) {
                $('#' + Clinical_Treatment.params.PanelID + ' div#divShowHistory').addClass("hidden");
            }
            $('#pnlClinicalTreatment #pnlProblemLists_ResultT #dgvProblemLists_PagingT').css('display', 'none');
            $('#' + Clinical_Treatment.params.PanelID + ' #pnlProblemLists_ResultT #dgvProblemListsT').DataTable({
                "language": {
                    "emptyTable": "No Problem List Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true, "info": false, "paging": false
            });
            if (response.ProblemListCount == 0) {
                $("#pnlProblemLists_ResultT #btnNoKnownProblems").css("display", "");
            } else {
                $("#pnlProblemLists_ResultT #btnNoKnownProblems").hide();
            }

            /* End of Code for making No Known Problem List hyperlink inline with checkbox and search box.
            *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
            */

        }

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        $('#' + Clinical_Treatment.params.PanelID + ' #dgvProblemListsT thead tr th:first-child').removeClass('sorting_asc');
        //select first treatment default
        if (Clinical_Treatment.params.TreatmentProblems && Clinical_Treatment.params.TreatmentProblems.length > 0) {
            $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody").find("tr#" + Clinical_Treatment.params.TreatmentProblems[0].ProblemId).trigger("click");
        }
        //reload problem dropdown for procedure
        var self = $('#' + Clinical_Treatment.params.PanelID);
        self.find('.Diagnosis > select').attr('ddlist', 'LookupProblemLists');
        var data = "IsActive=&ID=" + $('#PatientProfile #hfPatientId').val();
        self.find('.Diagnosis').loadDropDowns(true, data).done(function () {
        });
        dfd.resolve();
        return dfd;
    },
    checkUncheckAllProblemsList: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_Treatment.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", true);
        }
        else {
            $("#" + Clinical_Treatment.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", false);
        }

        $("#" + Clinical_Treatment.params.PanelID + " #dgvProblemListsT tbody").find('input[type="checkbox"]').each(function () {
            Treatment_ProblemListGrid.enableAddProbList(this);
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
        var PanelID = "";
        params["ParentCtrl"] = "Clinical_Treatment";
        PanelID = Clinical_Treatment.params.PanelID;
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        params["ProblemListId"] = ProblemListId;
        params["FromAdmin"] = "0";
        LoadActionPan('Clinical_ProblemListsComments', params, PanelID);
    },
    ActiveProblemSearch: function (objThis) {


        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        Treatment_ProblemListGrid.ProblemListsSearch();
    },
    ShowHistory: function (problemListId) {
        EMRUtility.showCurrentItemHistory(Clinical_Treatment.params.PanelID, null, problemListId, "ProblemList", Clinical_Treatment.params.OrderSetId, "Treatment_ProblemListGrid", null);

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

            Treatment_ProblemListGrid.UpdateProblemListsRow(myJSON, id).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    Treatment_ProblemListGrid.ProblemListsSearch();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }
        Clinical_Treatment.ImmunizationSearch('', 1, 15);
    },

    rowDetail: function ($row, ClassName) {
        var currentProblemListId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProblemListId > 0) {
            Treatment_ProblemListGrid.ShowHistory(currentProblemListId);
        }
    },

    rowHistory: function ($row, ClassName) {
        var currentProblemListId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProblemListId > 0) {
            Treatment_ProblemListGrid.ShowHistory(currentProblemListId);
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
                Treatment_ProblemListGrid.DeleteProblemList(selectedValue, description, $row.find("td:nth-child(7)").html()).done(function (response) {
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
                    Treatment_ProblemListGrid.ProblemListsSearch();

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
        IsActive = $('#' + Clinical_Treatment.params.PanelID + ' #pnlProblemLists_ResultT #divSwitch #switchActive').attr('isactive');      
        utility.myConfirm('3', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var IsActiveRecord = null;
                IsActiveRecord = $('#' + Clinical_Treatment.params.PanelID + ' #pnlProblemLists_ResultT #divSwitch #switchActive').attr('isactive');
                if (IsActiveRecord == "1") {
                    var params = [];
                    var PanelID = "";
                    params["ParentCtrl"] = "Clinical_Treatment";
                    PanelID = Clinical_Treatment.params.PanelID;

                    params["ProblemListId"] = selectedValue;
                    params["FromAdmin"] = "0";
                    params["OrderSetId"] = Clinical_Treatment.params.OrderSetId;
                    LoadActionPan('Clinical_ProblemListInActive', params, PanelID);
                } else {
                    IsActiveRecord = "0";
                    Treatment_ProblemListGrid.InActiveProblemList(selectedValue, null, null).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Treatment_ProblemListGrid.ProblemListsSearch();
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
        IsCheckedIn = $('#' + Clinical_Treatment.params.PanelID + ' #pnlProblemLists_ResultT #divSwitch #switchActive').attr('isactive');

        var IsActiveRecord = null;
        IsActiveRecord = $('#' + Clinical_Treatment.params.PanelID + ' #pnlProblemLists_ResultT #divSwitch #switchActive').attr('isactive');
        if (IsActiveRecord == "1")
            IsActiveRecord = "0";
        else if (IsActiveRecord == "0")
            IsActiveRecord = "1";

        var orderSetId = Clinical_Treatment.params.OrderSetId;

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
        Clinical_Treatment.ImmunizationSearch('', 1, 15);
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

        var VitalsGridId = "#" + Clinical_Treatment.params.PanelID + " #dgvProblemListsT";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = Treatment_ProblemListGrid.EditableGrid.datatable.row(parentRow);

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
        isactive = $('#' + Clinical_Treatment.params.PanelID + ' #pnlProblemLists_ResultT #divSwitch #switchActive').attr('isactive');
        var objData = JSON.parse(ProblemListData);
        objData["ProblemListId"] = ProblemListId;
        objData["Comments"] = objData["hfComments"];
        objData["IsActive"] = "1";
        objData["commandType"] = "UPDATE_PROBLEMLIST";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    //
    buildHistoryRows: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems) {
        var row = Treatment_ProblemListGrid.EditableGrid.datatable.row(CurrentRow);
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
        objData["PatientId"] = Clinical_Treatment.params.PatientId;
        objData["Description"] = Description;
        objData["StartDate"] = StartDate;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#dgvProblemLists_PagingT li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Treatment_ProblemListGrid.ProblemListsSearch(null, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#dgvProblemLists_PagingT li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Treatment_ProblemListGrid.ProblemListsSearch(null, currentPageNo, 15);
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#dgvProblemLists_PagingT li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Treatment_ProblemListGrid.ProblemListsSearch(null, currentPageNo, 15);
        }
    },

}