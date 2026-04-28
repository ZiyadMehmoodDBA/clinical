OrderSet_ProceduresGrid = {
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
    ProceduresSearch: function (ProcedureId, PageNo, rpp, mode, IsNotes) {
        OrderSet_ProceduresGrid.IsNotes = IsNotes;
        OrderSet_ProceduresGrid.SearchProcedures(ProcedureId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS th#SelectRecord").remove();

                OrderSet_ProceduresGrid.ProceduresGridLoadNew(response, mode);

                //Adding Pagination
                var TableControl = Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS";
                var PagingPanelControlID = Clinical_OrderSetDetails.params.PanelID + " #dgvProcedures_PagingOS";
                var ClassControlName = "OrderSet_ProceduresGrid";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ProcedureCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    OrderSet_ProceduresGrid.ProceduresSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);

                setTimeout(function () {
                    //$("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS th[controlname='LastUpdatedDate']").click();
                    //$("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS th[controlname='LastUpdatedDate']").click();
                }, 20);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ProceduresGridLoad: function (response) {
        if (response.ProcedureCount > 0) {
            OrderSet_ProceduresGrid.EditableGrid.datatable.clear().draw();

            var ProceduresLoadJSONData = JSON.parse(response.ProcedureLoad_JSON);

            $.each(ProceduresLoadJSONData, function (i, item) {
                var ProcedureId = item.ProcedureId;
                var CurrentRow = OrderSet_ProceduresGrid.AddNewProceduresRow(ProcedureId, "Edit", null);
                var self = $("#dgvProceduresOS tr#" + ProcedureId);
                utility.bindMyJSONByName(true, item, false, self).done(function () {

                });

                var row = OrderSet_ProceduresGrid.EditableGrid.datatable.row(CurrentRow);

                /********************************/
                var newChildRow = row.child();

                /********************************/


                row.child().loadDropDowns(true).done(function () {
                    utility.bindMyJSON(true, item, false, $(newChildRow));

                });
            });
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS #dgvProceduresOS").css("display", "none");

            $('#dgvProceduresOS').DataTable({
                "language": {
                    "emptyTable": "No Procedure List Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [8] }]
            });

        }
    },

    SearchProcedures: function (ProcedureId, PageNumber, RowsPerPage) {


        var IsCheckedIn = null;
        IsCheckedIn = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProcedures_ResultOS #divSwitch #switchActive').attr('isactive');
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

        var ProcedureData = [];
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        objData["ProcedureId"] = ProcedureId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        ProcedureData.push(objData);

        var objData1 = new Object();
        objData1["procedureDetailModel"] = ProcedureData;
        objData1["ProcedureId"] = ProcedureId;
        objData1["IsActive"] = IsCheckedIn;
        objData1["commandType"] = "search_procedures";
        var data = JSON.stringify(objData1);
        return MDVisionService.APIService(data, "OrderSet", "Procedure");

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

            CurrentRow = OrderSet_ProceduresGrid.EditableGrid.rowAdd(RowId, Clinical_OrderSetDetails.params.VitalSignsId, null, null, null, null, NotesId);

        }
        else {
            var TemplateRow = $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS #dgvProceduresOS tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            CurrentRow = OrderSet_ProceduresGrid.EditableGrid.rowAdd(TemplateRowId - 1, Clinical_OrderSetDetails.params.VitalSignsId, null, null, null, null, null);

        }

        var row = OrderSet_ProceduresGrid.EditableGrid.datatable.row(CurrentRow);
        row.child(OrderSet_ProceduresGrid.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child.hide();
        OrderSet_ProceduresGrid.enableRemoveRow($(CurrentRow));
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

            OrderSet_ProceduresGrid.UpdateProceduresRow(myJSON, id).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    utility.DisplayMessages(response.Message, 1);
                    OrderSet_ProceduresGrid.ProceduresSearch();

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }

    },

    rowDetail: function ($row, ClassName) {
        var currentVitalSignId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentVitalSignId > 0) {
            OrderSet_ProceduresGrid.VitalsEdit(currentVitalSignId);
        }
    },

    rowHistory: function ($row, ClassName) {
        var temp = $row.attr('id');
        var tempId = temp.split('-')[1];
        $row.attr('id', tempId);
        var currentProcedureId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProcedureId > 0) {
            OrderSet_ProceduresGrid.ShowHistory(currentProcedureId);
        }
    },
    ShowHistory: function (ProcedureId) {
        EMRUtility.showCurrentItemHistory(Clinical_OrderSetDetails.params.PanelID, null, ProcedureId, "Procedures", Clinical_OrderSetDetails.params.OrderSetId, "OrderSet_ProceduresGrid", null);
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

                OrderSet_ProceduresGrid.EnableFavProcedure(objData["ProcedureProcedure"]);
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
                OrderSet_ProceduresGrid.DeleteProcedure(selectedValue).done(function (response) {
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
                    OrderSet_ProceduresGrid.ProceduresSearch();
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
        IsActive = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProcedures_ResultOS #divSwitch #switchActive').attr('isactive');      
        utility.myConfirm('3', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {

                OrderSet_ProceduresGrid.InActiveProcedures(selectedValue, null, null).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        OrderSet_ProceduresGrid.ProceduresSearch();
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
        IsActiveRecord = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProcedures_ResultOS #divSwitch #switchActive').attr('isactive');

        if (IsActiveRecord == null || IsActiveRecord == '1') {
            IsActiveRecord = false;
        } else {
            IsActiveRecord = true;
        }
        var objData = new Object();
        objData["ProcedureId"] = ProcedureId;
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
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

        $row.find("#hfComments").val(OrderSet_ProceduresGrid.PreviousComments);

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

        var VitalsGridId = "#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = OrderSet_ProceduresGrid.EditableGrid.datatable.row(parentRow);

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
        isactive = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProcedures_ResultOS #divSwitch #switchActive').attr('isactive');
        var objData = {};
        var ProcedureDetail = [];
        var objData1 = JSON.parse(ProceduresData);
        objData1["ProcedureId"] = ProcedureId;
        objData1["Comments"] = $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS tbody tr[id='" + ProcedureId + "']").find("#hfComments").val();//$('#' + Clinical_OrderSetDetails.params.PanelID + ' #hfGridComments').val();
        objData1["IsActive"] = isactive;
        ProcedureDetail.push(objData1);
        objData["procedureDetailModel"] = ProcedureDetail;
        objData["ProcedureId"] = ProcedureId;
        objData["commandType"] = "update_procedure";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Procedure");

    },
    buildHistoryRows: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems) {
        var row = OrderSet_ProceduresGrid.EditableGrid.datatable.row(CurrentRow);
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
        objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;



        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Procedure");

    },

    // problem list grid load as per admin

    ProceduresGridLoadNew: function (response, mode) {
        if ($("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS #dgvProceduresOS thead tr #SelectRecord").length == 0 && Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS #dgvProceduresOS thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox" name="SelectCheckBoxProcedure" id="chkHeaderProcedures" onchange="OrderSet_ProceduresGrid.checkUncheckAllProcedures(this);"  class="input-block" coltype="checkbox"/> </th>');
        }
        // get Actions
        var actions = "";
        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, " #pnlProcedures_ResultOS");
                }
            }
        });
        if ($.fn.dataTable.isDataTable("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS #dgvProceduresOS")) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS #dgvProceduresOS").dataTable().fnClearTable();
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS #dgvProceduresOS").dataTable().fnDestroy();
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS #dgvProceduresOS tbody").find("tr").remove();
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS #dgvProceduresOS tbody").find("tr").remove();

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
                    var commentsMethod = "OrderSet_ProceduresGrid.AddComments('" + item.ProcedureId + "');";
                    comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                }
                var ProcedureId = item.ProcedureId;
                var ChildHistory_Procedures = $.grep(ProcedureHistoryLoadJSONData, function (n, i) {
                    return n.ProcedureId == ProcedureId;
                });
                var icd9String = '';
                if (item.ICD9) {
                    icd9String = item.ICD9 + ' - ' + item.ICD10 + ' - ' + item.ICD10_DESCRIPTION
                }
                var Checked = "";
                var SelectionCheckBoxColumn = '';
                if (Clinical_OrderSetDetails.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="pull-left mt-default" onchange="OrderSet_ProceduresGrid.enableAddProcedure(this);" id="' + item.ProcedureId + '" name="SelectCheckBoxProcedure" ' + Checked + ' class="input-block text-center"/></td>';
                }


                if (ChildHistory_Procedures.length > 0) {
                    $row.append(SelectionCheckBoxColumn+'<td><a id="Expand" href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions Actions_" id="' + item.ProcedureId + '" >' + actions + '</td><td>' + item.CPTCode + ' ' + item.CPT_DESCRIPTION + '</td><td>' + item.Unit + '</td><td>' + item.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td>');
                    $row.attr("isHistory", '1');

                } else {
                    $row.append(SelectionCheckBoxColumn + '<td></td><td class="actions Actions_" id="' + item.ProcedureId + '" >' + actions + '</td><td>' + item.CPTCode + ' ' + item.CPT_DESCRIPTION + '</td><td>' + item.Unit + '</td><td>' + item.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td>');

                }

                var hfProceduresComments = $(' <input type="hidden" id="hfComments" name="Comments" value="' + item.Comments + '">');
                $row.append(hfProceduresComments);

                if (item.IsActive == "True") {
                    $($row).find('a.edit-row').removeClass('disableAll');
                } else {
                    $($row).find('a.edit-row').addClass('disableAll');
                }


                $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS tbody").last().append($row);

                var CurrentRowchilds = $();

                if (ChildHistory_Procedures.length > 0) {
                    $.each(ChildHistory_Procedures, function (i, item1) {
                        if (item1.Comments != "") {
                            var commentsMethod = "OrderSet_ProceduresGrid.AddComments('" + item1.ProcedureId + "');";
                            comments = '<label data-toggle="tooltip" data-placement="left" title="' + item1.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                        }
                        else {
                            comments = "";
                        }
                        icd9String = '';
                        if (item1.ICD9) {
                            icd9String = item1.ICD9 + ' - ' + item1.ICD10 + ' - ' + item1.ICD10_DESCRIPTION
                        }
                        var Title_Tooltip = "Inactive Reason: " + item1.InActiveChkBoxValue + (item1.EndDate != '' ? " <br/>End Date: " + utility.RemoveTimeFromDate(null, item1.EndDate) : "") + (item1.InActiveReason != '' ? " <br/>Comments: " + item1.InActiveReason : "");
                        var IsActiveText = "";
                        if (item1.IsActive == "True") {
                            IsActiveText = "<Label>[Active]</Label>";
                        } else {
                            IsActiveText = "<Label data-toggle='tooltip' data-placement='right' title='" + Title_Tooltip + "'>[Inactive]</Label>";
                        }

                        
                        var currentHistory = '<tr class="childRow-bg"><td></td><td class="actions" id="' + item1.ProcedureId + '" ></td><td>' + IsActiveText + '</td><td>' + item1.Unit + '</td><td>' + item1.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item1.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item1.EndDate) + '</td><td>' + item1.ModifiedOn + ' ' + item1.ModifiedByName + '</td><td align="center"> ' + comments + ' </td>';

                        CurrentRowchilds = CurrentRowchilds.add(currentHistory);

                    });
                }

                if (CurrentRowchilds.length > 0) {

                }

                arraTemp.push({ row: $row, childs: CurrentRowchilds });

            });

            //Inalize grid
            var PanelGrid = "#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS";
            var GridId = "#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS";

            // Below line comment out inorder to remove duplicate grid search
            if (OrderSet_ProceduresGrid.myGrid != null) {
                OrderSet_ProceduresGrid.myGrid.$table.dataTable().fnDestroy();
                $("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS #dgvProceduresOS").dataTable().fnDestroy();
            }
            //Below line comment out inorder to remove duplicate grid search

            OrderSet_ProceduresGrid.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, OrderSet_ProceduresGrid, 0, false, true, false, true, false, null);

            //rander childs
            $.each(arraTemp, function (i, item) {

                if (OrderSet_ProceduresGrid.myGrid != null) {
                    var row = OrderSet_ProceduresGrid.myGrid.datatable.row(item.row);
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


            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProcedures_ResultOS #dgvProceduresOS').DataTable({
                "language": {
                    "emptyTable": "No Procedure Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
            });
            if (response.ProcedureTotalCount == 0) {
                $("#pnlProcedures_ResultOS #btnNoKnownProblems").css("display", "");
            } else {
                $("#pnlProcedures_ResultOS #btnNoKnownProblems").hide();
            }

            /* End of Code for making No Known Problem List hyperlink inline with checkbox and search box.
            *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
            */
        }

        if (mode && mode == "View") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS").find('a').addClass('disableAll');
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS").find('#Expand').removeClass('disableAll');
            
        }

        if (OrderSet_ProceduresGrid.IsNotes == true) {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProceduresOS').find('td.Actions_').addClass('hidden');
            $('#' + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS").css('display', '');
        }


        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        //Sorting icon removed from first column of grid
        $('#' + Clinical_OrderSetDetails.params.PanelID + ' #dgvProceduresOS thead tr th:first-child').removeClass('sorting_asc');
        EMRUtility.fixDataTableDuplication("#" + Clinical_OrderSetDetails.params.PanelID + " #pnlProcedures_ResultOS");

    },

    checkUncheckAllProcedures: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxProcedure']").prop("checked", true);
        }
        else {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " [name='SelectCheckBoxProcedure']").prop("checked", false);
        }

        $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS tbody").find('input[type="checkbox"]').each(function () {
            OrderSet_ProceduresGrid.enableAddProcedure(this);
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
        params["ParentCtrl"] = 'Clinical_OrderSetDetails';
        params["ProcedureID"] = ProcedureId;
        params["Comments"] = $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS tbody tr[id='" + ProcedureId + "']").find("#hfComments").val();
        params["FromAdmin"] = "0";
        params["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        LoadActionPan('Clinical_ProceduresComments', params, Clinical_OrderSetDetails.params.PanelID);
    },

    ActiveProblemSearch: function (objThis) {


        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        OrderSet_ProceduresGrid.ProceduresSearch();
    },
}