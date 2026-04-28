Treatment_ProcedureListGrid = {
    params: [],
    Procedures: [],
    ProceduresDetail: [],

    IsUpdate: false,
    EditableGrid: null,
    EditableGridAdd: null,
    myGrid: null,
    IsNotes: false,
    FavListName: "ClinicalProcedureDetail",
    PreviousComments: '',
    ProceduresSearch: function (ProcedureId, PageNo, rpp, fromPagination) {
        Treatment_ProcedureListGrid.SearchProcedures(ProcedureId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#" + Clinical_Treatment.params.PanelID + " #dgvProceduresT th#SelectRecord").remove();

                Treatment_ProcedureListGrid.ProceduresGridLoadNew(response, fromPagination);

                //Adding Pagination
                var TableControl = Clinical_Treatment.params.PanelID + " #dgvProceduresT";
                var PagingPanelControlID = Clinical_Treatment.params.PanelID + " #dgvProcedures_PagingT";
                var ClassControlName = "Treatment_ProcedureListGrid";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ProcedureCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Treatment_ProcedureListGrid.ProceduresSearch(PrimaryID, PageNumber, ResultPerPage, true);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ProceduresGridLoad: function (response) {
        if (response.ProcedureCount > 0) {
            Treatment_ProcedureListGrid.EditableGrid.datatable.clear().draw();

            var ProceduresLoadJSONData = JSON.parse(response.ProcedureLoad_JSON);

            $.each(ProceduresLoadJSONData, function (i, item) {
                var ProcedureId = item.ProcedureId;
                var CurrentRow = Treatment_ProcedureListGrid.AddNewProceduresRow(ProcedureId, "Edit", null);
                var self = $("#dgvProceduresT tr#" + ProcedureId);
                utility.bindMyJSONByName(true, item, false, self).done(function () {

                });

                var row = Treatment_ProcedureListGrid.EditableGrid.datatable.row(CurrentRow);

                /********************************/
                var newChildRow = row.child();

                /********************************/


                row.child().loadDropDowns(true).done(function () {
                    utility.bindMyJSON(true, item, false, $(newChildRow));

                });
            });
        }
        else {
            $("#" + Clinical_Treatment.params.PanelID + " #pnlProcedures_ResultT #dgvProceduresT").css("display", "none");

            $('#dgvProceduresT').DataTable({
                "language": {
                    "emptyTable": "No Procedure List Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [8] }]
            });

        }
    },

    SearchProcedures: function (ProcedureId, PageNumber, RowsPerPage) {

        var IsCheckedIn = null;
        IsCheckedIn = "1";
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();

        var ProcedureData = [];
        //var objData1 = {};

        objData["PatientId"] = Clinical_Treatment.params.PatientId;
        objData["ProcedureId"] = ProcedureId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["NotesId"] = 0;
        ProcedureData.push(objData);

        var objData1 = new Object();
        objData1["procedureDetailModel"] = ProcedureData;
        objData1["ProcedureId"] = ProcedureId;
        objData1["IsActive"] = IsCheckedIn;
        objData1["ShowEMCodes"] = "0";
        objData1["commandType"] = "search_procedures";
        var data = JSON.stringify(objData1);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

    },
    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div></div>");
        var txtProblem = "<div class='col-xs-1'><label class='control-label'>Problem</label><input  class='form-control'type='text' id='txtProblem" + ParentRowId + "' name='Problem" + ParentRowId + "' disabled></div>";
        var txtDiagnosis = "<div class='col-xs-1'><label class='control-label'>ICD-9 ICD-10 Description</label><input class='form-control' id='Diagnosis" + ParentRowId + "' name='Diagnosis" + ParentRowId + "' type='text' /></div>";
        var txtChronicityLevel = "<div class='col-xs-1'><label class='control-label'>Chronicity Level</label><input class='form-control' id='txtChronicity" + ParentRowId + "' name='Chronicity" + ParentRowId + "' type='text' /></div>";
        var txtSeverity = "<div class='col-xs-1  size-min100'><label class='control-label'>Severity</label><input class='form-control' id='txtSeverity" + ParentRowId + "' name='Severity" + ParentRowId + "' type='text' /></div>";
        var ddlNDCMeasurement = "<div class='col-xs-2'><label class='control-label'>NDC Measurement Code</label><select id='ddlNDCMeasurement" + ParentRowId + "' name='ddlNDCMeasurement" + ParentRowId + "' class='form-control' ddlist='GetNDCMeasurementCode'></select></div>";
        var LineNotes = "<div class='col-xs-2'><label class='control-label'>Line Notes</label><textarea spellcheck='true' class='form-control' rows='1' id='txtComments" + ParentRowId + "' name='txtComments" + ParentRowId + "'></textarea></div>";
        var chkHold = "<div class='col-xs-1 pt-lg'><div class='checkbox-custom checkbox-default'><input type='checkbox' onclick=OrderSet_Procedures.validateIsHold(this,'divHoldDays" + ParentRowId + "') id='chkHold" + ParentRowId + "' value name='chkHold" + ParentRowId + "'/><label class='control-label'>Is Hold</label></div></div>";
        var HoldDays = "<div id='divHoldDays" + ParentRowId + "' style='display:none' class='col-xs-1'><label class='control-label'>Hold Days</label><input type='text' class='form-control' onfocusout=OrderSet_Procedures.validateHoldDays(this,'chkHold" + ParentRowId + "') id='txtHoldDays" + ParentRowId + "' data-mask='9?99' name='txtHoldDays" + ParentRowId + "'/></div>";
        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(txtProblem, txtDiagnosis, txtChronicityLevel, txtSeverity, ddlNDCMeasurement, LineNotes, chkHold, HoldDays, spacer);
        return ChildHTML;

    },
    AddNewProceduresRow: function (RowId, mode, CurrRef, NotesId) {


        var CurrentRow = null;
        if (RowId && RowId > 0) {

            CurrentRow = Treatment_ProcedureListGrid.EditableGrid.rowAdd(RowId, Clinical_Treatment.params.VitalSignsId, null, null, null, null, NotesId);

        }
        else {
            var TemplateRow = $("#" + Clinical_Treatment.params.PanelID + " #pnlProcedures_ResultT #dgvProceduresT tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            CurrentRow = Treatment_ProcedureListGrid.EditableGrid.rowAdd(TemplateRowId - 1, Clinical_Treatment.params.VitalSignsId, null, null, null, null, null);

        }

        var row = Treatment_ProcedureListGrid.EditableGrid.datatable.row(CurrentRow);
        row.child(Treatment_ProcedureListGrid.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child.hide();
        Treatment_ProcedureListGrid.enableRemoveRow($(CurrentRow));
        return CurrentRow;
    },
    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");

    },



    rowSave: function ($row, obj) {
        var temp = $row.attr('id');
        var tempId = temp.split('-')[1];
        $row.attr('id', tempId);
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
        if (id && id > 0) {

            //Edit Record

            Treatment_ProcedureListGrid.UpdateProceduresRow(myJSON, id).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    utility.DisplayMessages(response.Message, 1);
                    Treatment_ProcedureListGrid.ProceduresSearch();

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        Clinical_Treatment.ImmunizationSearch('', 1, 15);
    },

    rowDetail: function ($row, ClassName) {
        var currentVitalSignId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentVitalSignId > 0) {
            Treatment_ProcedureListGrid.VitalsEdit(currentVitalSignId);
        }
    },

    rowHistory: function ($row, ClassName) {
        var temp = $row.attr('id');
        var tempId = temp.split('-')[1];
        $row.attr('id', tempId);
        var currentProcedureId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProcedureId > 0) {
            Treatment_ProcedureListGrid.ShowHistory(currentProcedureId);
        }
    },
    ShowHistory: function (ProcedureId) {
        EMRUtility.showCurrentItemHistory(Clinical_Treatment.params.PanelID, null, ProcedureId, "Procedures", Clinical_Treatment.params.OrderSetId, "Treatment_ProcedureListGrid", null);
    },

    rowAdd: function () {
        EditableGrid.rowAdd();
    },

    rowRemove: function ($row, obj) {
        var temp = $row.attr('id');
        var tempId = temp.split('-')[1];
        $row.attr('id', tempId);
        var strMessage = "";
        var id = $row.attr("id");

        utility.myConfirm('1', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else if (parseInt($row.attr("id")) < 0) {
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();

                var myJSON = $row.getMyJSONByName();
                var objData = JSON.parse(myJSON);

                Treatment_ProcedureListGrid.EnableFavProcedure(objData["ProcedureProcedure"]);
                utility.DisplayMessages("Successfully Deleted", 1);

            }
            else {

                var description;
                if ($row.find("td:nth-child(4)").html() != "") {
                    description = $row.find("td:nth-child(4)").html();
                }
                else {
                    description = $row.find("td:nth-child(3)").html();
                }
                Treatment_ProcedureListGrid.DeleteProcedure(selectedValue).done(function (response) {
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

                    //Start No Known Procedures hyperlink(link not visible when there is no problem list) issue fixed
                    Treatment_ProcedureListGrid.ProceduresSearch();
                    //End  No Known Procedures hyperlink(link not visible when there is no problem list) issue fixed

                });
            }
        }, function () { },
            '1'
        );
    },

    rowInactive: function ($row, obj) {
        var temp = $row.attr('id');
        var tempId = temp.split('-')[1];
        $row.attr('id', tempId);
        var strMessage = "";
        var id = $row.attr("id");      
        var IsActive = null;
        IsActive = $('#' + Clinical_Treatment.params.PanelID + ' #pnlProcedures_ResultT #divSwitch #switchActive').attr('isactive');

        utility.myConfirm('3', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {

                Treatment_ProcedureListGrid.InActiveProcedures(selectedValue, null, null).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        Treatment_ProcedureListGrid.ProceduresSearch();
                    }
                    else {
                        utility.DisplayMessages(response.message, 1);
                    }
                });

            }
        }, function () { },
             '3', null, null, null, IsActive
        );
    },

    InActiveProcedures: function (ProcedureId, comments, endDate) {

        var IsActiveRecord = null;
        IsActiveRecord = $('#' + Clinical_Treatment.params.PanelID + ' #pnlProcedures_ResultT #divSwitch #switchActive').attr('isactive');

        if (IsActiveRecord == null || IsActiveRecord == '1') {
            IsActiveRecord = false;
        } else {
            IsActiveRecord = true;
        }
        var objData = new Object();
        objData["ProcedureId"] = ProcedureId;
        objData["OrderSetId"] = Clinical_Treatment.params.OrderSetId;
        objData["IsActive"] = IsActiveRecord;
        objData["commandType"] = "INACTIVE_PROCEDURES";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "OrderSet", "Procedure");

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

        $row.find("#hfComments").val(Treatment_ProcedureListGrid.PreviousComments);
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

        var VitalsGridId = "#" + Clinical_Treatment.params.PanelID + " #dgvProceduresT";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = Treatment_ProcedureListGrid.EditableGrid.datatable.row(parentRow);

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
    UpdateProceduresRow: function (ProceduresData, ProcedureId) {
        var isactive = null;
        isactive = "1";
        var objData = {};
        var ProcedureDetail = [];
        var objData1 = JSON.parse(ProceduresData);
        objData1["ProcedureId"] = ProcedureId;
        objData1["Comments"] = $("#" + Clinical_Treatment.params.PanelID + " #dgvProceduresT tbody tr[id='" + ProcedureId + "']").find("#hfComments").val();//$('#' + Clinical_Treatment.params.PanelID + ' #hfGridComments').val();
        objData1["IsActive"] = isactive;
        objData1["ProblemListId_text"] = objData1["ProblemListId_text"] == "- Select -" ? "" : objData1["ProblemListId_text"];
        ProcedureDetail.push(objData1);
        objData["procedureDetailModel"] = ProcedureDetail;
        objData["ProcedureId"] = ProcedureId;
        objData["commandType"] = "update_procedure";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },
    buildHistoryRows: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems) {
        var row = Treatment_ProcedureListGrid.EditableGrid.datatable.row(CurrentRow);
        if (arrChildItems != null && arrChildItems.length > 0) {
            var CurrentRowchilds = $();
            $.each(arrChildItems, function (i, item) {
                var currentChildRow = $("#" + CurrentRow.attr("id")).clone();
                currentChildRow.attr("id", "Child" + item.ProcedureId);

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
    DeleteProcedure: function (ProcedureId) {

        var objData = new Object();
        objData["ProcedureId"] = ProcedureId;
        objData["commandType"] = "DELETE_Procedure";
        objData["PatientId"] = Clinical_Treatment.params.PatientId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

    },

    // problem list grid load as per admin

    ProceduresGridLoadNew: function (response, fromPagination) {

        var actions = "";
        $("#" + Clinical_Treatment.params.PanelID + " #dgvProceduresT tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, " #pnlProcedures_ResultT");
                }
            }
        });
        if ($.fn.dataTable.isDataTable("#" + Clinical_Treatment.params.PanelID + " #pnlProcedures_ResultT #dgvProceduresT")) {
            $("#" + Clinical_Treatment.params.PanelID + " #pnlProcedures_ResultT #dgvProceduresT").dataTable().fnClearTable();
            $("#" + Clinical_Treatment.params.PanelID + " #pnlProcedures_ResultT #dgvProceduresT").dataTable().fnDestroy();
            $("#" + Clinical_Treatment.params.PanelID + " #pnlProcedures_ResultT #dgvProceduresT tbody").find("tr").remove();
        }

        $("#" + Clinical_Treatment.params.PanelID + " #pnlProcedures_ResultT #dgvProceduresT tbody").find("tr").remove();

        if (response.ProcedureCount > 0) {
            var ProceduresLoadJSONData = JSON.parse(response.ProcedureLoad_JSON);
            var ProcedureHistoryLoadJSONData = JSON.parse(response.ProcedureHistoryLoad_JSON);

            var arraTemp = [];

            $.each(ProceduresLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", "txtpUnits-" + item.ProcedureId);
                $row.attr("ProceduresNotesId", item.NoteId);

                $row.attr("ProblemListId", item.ProblemListId);

                $row.attr("name", "Procedures");

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

                if (item.Comments != "") {
                    var commentsMethod = "Treatment_ProcedureListGrid.AddComments('" + item.ProcedureId + "');";
                    comments = '<label data-toggle="tooltip" data-placement="left" title data-original-title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                }
                var ProcedureId = item.ProcedureId;
                var ChildHistory_Procedures = $.grep(ProcedureHistoryLoadJSONData, function (n, i) {
                    return n.ProcedureId == ProcedureId;
                });
                var icd9String = '';
                if (item.ICD9) {
                    icd9String = item.ICD9 + ' - ' + item.ICD10 + ' - ' + item.ICD10_DESCRIPTION
                }
                var checkbox = '<td class="center" ><input class="CustomCheckBox" type="checkbox" onchange="Clinical_Treatment.CheckUncheck(this);" controlname="selectRecordProcedure" ArrayProperty="ProcedureIds" name="selectRecordProcedure" class="input-block text-center" coltype="checkbox" id="selectRecordProcedure_' + item.ProcedureId + '"/></td>';
                $row.append(checkbox + '<td class="actions Actions_" id="' + item.ProcedureId + '" >' + actions + '</td><td>' + item.CPTCode + ' ' + item.CPT_DESCRIPTION + '</td><td>' + item.Unit + '</td><td>' + item.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                var hfProceduresComments = $(' <input type="hidden" id="hfComments" name="Comments" value="' + item.Comments + '">');
                $row.append(hfProceduresComments);

                if (item.IsActive == "True") {
                    $($row).find('a.edit-row').removeClass('disableAll');
                } else {
                    $($row).find('a.edit-row').addClass('disableAll');
                }


                $("#" + Clinical_Treatment.params.PanelID + " #dgvProceduresT tbody").last().append($row);

                var CurrentRowchilds = $();
                arraTemp.push({ row: $row, childs: CurrentRowchilds });

            });

            //Inalize grid
            var PanelGrid = "#" + Clinical_Treatment.params.PanelID + " #pnlProcedures_ResultT";
            var GridId = "#" + Clinical_Treatment.params.PanelID + " #dgvProceduresT";

            // Below line comment out inorder to remove duplicate grid search
            if (Treatment_ProcedureListGrid.myGrid != null) {
                Treatment_ProcedureListGrid.myGrid.$table.dataTable().fnDestroy();
                $("#" + Clinical_Treatment.params.PanelID + " #pnlProcedures_ResultT #dgvProceduresT").dataTable().fnDestroy();
            }
            //Below line comment out inorder to remove duplicate grid search

            Treatment_ProcedureListGrid.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Treatment_ProcedureListGrid, 0, false, true, false, true, false, null);

            //rander childs
            $.each(arraTemp, function (i, item) {

                if (Treatment_ProcedureListGrid.myGrid != null) {
                    var row = Treatment_ProcedureListGrid.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });


        }
        else {


            $('#' + Clinical_Treatment.params.PanelID + ' #pnlProcedures_ResultT #dgvProceduresT').DataTable({
                "language": {
                    "emptyTable": "No Procedures List Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
            });
            if (response.ProcedureTotalCount == 0) {
                $("#pnlProcedures_ResultT #btnNoKnownProblems").css("display", "");
            } else {
                $("#pnlProcedures_ResultT #btnNoKnownProblems").hide();
            }

            /* End of Code for making No Known Problem List hyperlink inline with checkbox and search box.
            *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
            */
        }

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        //Sorting icon removed from first column of grid
        $('#' + Clinical_Treatment.params.PanelID + ' #dgvProceduresT thead tr th:first-child').removeClass('sorting_asc');
        EMRUtility.fixDataTableDuplication("#" + Clinical_Treatment.params.PanelID + " #pnlProcedures_ResultT");
        if (fromPagination) {
            Clinical_Treatment.params.IsExpand = false;
        }
        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT #dgvProblemListsT tbody tr.active").trigger("click");


    },

    checkUncheckAllProcedures: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_Treatment.params.PanelID + " [name='SelectCheckBoxProcedure']").prop("checked", true);
        }
        else {
            $("#" + Clinical_Treatment.params.PanelID + " [name='SelectCheckBoxProcedure']").prop("checked", false);
        }

        $("#" + Clinical_Treatment.params.PanelID + " #dgvProceduresT tbody").find('input[type="checkbox"]').each(function () {
            Treatment_ProcedureListGrid.enableAddProcedure(this);
        });
    },

    enableAddProcedure: function (obj) {
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.OSProcedures_ComponentIds) == -1) {
                Clinical_ProgressNote.OSProcedures_ComponentIds.push(obj.id);
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
            var index = Clinical_ProgressNote.OSProcedures_ComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.OSProcedures_ComponentIds.splice(index, 1);
            }
            //if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
            //    Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id);
            //}
        }


    },
    //Comments Update


    AddComments: function (ProcedureId) {


        var params = [];
        var PanelID = "";

        params["ParentCtrl"] = 'Clinical_Treatment';
        PanelID = 'pnlClinicalProgressNote #pnlClinicalTreatment';

        params["NotesId"] = -1;

        params["ProcedureID"] = ProcedureId;
        params["Comments"] = $("#" + Clinical_Treatment.params.PanelID + " #dgvProceduresT tbody tr[id='" + ProcedureId + "']").find("#hfComments").val();
        params["FromAdmin"] = "0";
        params["PatientId"] = Clinical_Treatment.params.PatientId;
        LoadActionPan('Clinical_ProceduresComments', params, PanelID);

    },

    ActiveProblemSearch: function (objThis) {


        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        Treatment_ProcedureListGrid.ProceduresSearch();
    },
}