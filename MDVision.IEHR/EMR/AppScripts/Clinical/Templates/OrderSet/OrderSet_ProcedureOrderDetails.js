OrderSet_ProcedureOrderDetails = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    ProcedureOrderProblems: [],
    FavListName: "ProcedureOrderDetail",
    checkedProblems: [],
    Load: function (params) {
        BackgroundLoaderShow(true);
        OrderSet_ProcedureOrderDetails.params = params;
        if (OrderSet_ProcedureOrderDetails.params.PanelID != 'pnlOsProcedureOrderDetail') {
            OrderSet_ProcedureOrderDetails.params.PanelID = OrderSet_ProcedureOrderDetails.params.PanelID + ' #pnlOsProcedureOrderDetail';
        } else {
            OrderSet_ProcedureOrderDetails.params.PanelID = 'pnlOsProcedureOrderDetail';
        }

        //params["TabID"] = 'OrderSet_ProcedureOrderDetails';
        //OrderSet_ProcedureOrderDetails.params = params;
        var PanelProcedureGrid = "#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #pnlProcedure_Result";
        var ProcedureGridId = "#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure";
        $(ProcedureGridId + " tbody tr").remove();
        $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #btnSaveOrder").addClass('disableAll');
        OrderSet_ProcedureOrderDetails.EditableGrid = EMRUtility.MakeEditableGrid(PanelProcedureGrid, ProcedureGridId, OrderSet_ProcedureOrderDetails, "0", false, false, false, false);
        var self = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail");

        //if (OrderSet_ProcedureOrderDetails.params.Title != null)
        //    $("#" + OrderSet_ProcedureOrderDetails.params["PanelID"] + " #headingTitle").text(OrderSet_ProcedureOrderDetails.params.Title);
        //if (OrderSet_ProcedureOrderDetails.params.NoteId != null) {
        //    $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder").text('Add to Note');
        //}
        //else {
        //    $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder").text('Save');
        //}
        if (OrderSet_ProcedureOrderDetails.bIsFirstLoad == true) {
            OrderSet_ProcedureOrderDetails.bIsFirstLoad = false;
            //OrderSet_ProcedureOrderDetails.loadProblemList();
            OrderSet_ProcedureOrderDetails.validateProcedureOrderDetail();

            OrderSet_ProcedureOrderDetails.loadAllAutocomplete();

            self.loadDropDowns(true).done(function () {
                OrderSet_ProcedureOrderDetails.favoriteListSearch();
                $.when(OrderSet_ProcedureOrderDetails.loadProblemList()).then(function () {
                    OrderSet_ProcedureOrderDetails.loadProcedureOrder();
                });
                //
                //if (OrderSet_ProcedureOrderDetails.params.mode == "Add") {
                //    OrderSet_ProcedureOrderDetails.selectProvider();
                //}
                //$('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail").data('serialize', $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail").serialize());
                //$('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #ddlProcedureOrderRuleType").multiselect({
                //    includeSelectAllOption: true,
                //    enableFiltering: true,
                //    enableCaseInsensitiveFiltering: true,
                //    onChange: function (element, checked) {
                //        var selectedValue = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #ddlProcedureOrderRuleType option:selected");
                //        var selected = [];
                //        $(selectedValue).each(function (index, val) {
                //            selected.push($(this).text());
                //        });
                //        var unSelect = '';
                //        $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail .comList").addClass('hidden');
                //        $(selected).each(function (i, item) {
                //            var sectionName = item;
                //            unSelect += sectionName + ',';
                //            $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmClinicalProcedureOrderDetail #dv" + item.replace(/\s/g, '')).removeClass('hidden');
                //        });
                //    }
                //});
            });
        }
        OrderSet_ProcedureOrderDetails.domReadyFunction();
    },
    domReadyFunction: function () {

        $(document).ready(function () {
            $('.toggleHorSmallLeft section').click(function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));

            });
        });
    },
    loadProcedureOrder: function () {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Orders and Results_Procedure", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        if (OrderSet_ProcedureOrderDetails.params.mode == "Edit") {
            //$('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #ClinicalProcedureOrderDetail #txtProcedureOrderTitle").attr("enabled", "enabled");
            OrderSet_ProcedureOrderDetails.fillProcedureOrder(OrderSet_ProcedureOrderDetails.params.ProcedureOrderId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    OrderSet_ProcedureOrderDetails.ProcedureOrderTestGridLoad(response);
                    var ProcedureOrderDetail = JSON.parse(response.ProcedureLoad_JSON);
                    if (ProcedureOrderDetail.length > 0)
                        ProcedureOrderDetail = ProcedureOrderDetail[0];

                    var decodeHtmlEntity = function (str) {
                        return str.replace(/&#(\d+);/g, function (match, dec) {
                            return String.fromCharCode(dec);
                        });
                    };
                    if (ProcedureOrderDetail.AssigneeName != null) {
                        ProcedureOrderDetail.Assignee = decodeHtmlEntity(ProcedureOrderDetail.AssigneeName);
                        ProcedureOrderDetail.txtAssignee = decodeHtmlEntity(ProcedureOrderDetail.AssigneeName);
                        ProcedureOrderDetail.hfAssignee = ProcedureOrderDetail.AssigneeId;
                    }
                    if (response.ProcedureOrderProblems_JSON != null) {

                        response.ProcedureOrderProblems_JSON = JSON.parse(response.ProcedureOrderProblems_JSON);

                        for (var index in response.ProcedureOrderProblems_JSON) {
                            $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #ulProblemLists td #chk" + response.ProcedureOrderProblems_JSON[index].ProblemId).attr("checked", "checked");
                            OrderSet_ProcedureOrderDetails.pushProblems($('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #ulProblemLists td #chk" + response.ProcedureOrderProblems_JSON[index].ProblemId));
                        }
                    }

                    var self = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail");
                    utility.bindMyJSON(true, ProcedureOrderDetail, false, self);



                    $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #hfProcedureOrderID").val(ProcedureOrderDetail.ProcedureOrderId);


                    $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail").data('serialize', $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail").serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {

        }
        if (EMRUtility.getFavListStatus(OrderSet_ProcedureOrderDetails.FavListName))
            $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #favSectionDiv").removeClass("toggled");
        else
            $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #favSectionDiv").addClass("toggled");
    },
    fillProcedureOrder: function (ProcedureOrderId) {
        var objData = new Object();
        objData["ProcedureOrderId"] = ProcedureOrderId;
        objData["commandType"] = "fill_ProcedureOrder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProcedureOrder");
    },
    ProcedureOrderTestGridLoad: function (response) {
        //var response = JSON.parse(response);
        var PanelProcedureGrid = "#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #pnlProcedure_Result";
        var ProcedureGridId = "#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure";
        $(ProcedureGridId + " tbody tr").remove();
        if ($.fn.dataTable.isDataTable(ProcedureGridId)) {
            $(ProcedureGridId).dataTable().fnClearTable();
            $(ProcedureGridId).dataTable().fnDestroy();
        }
        OrderSet_ProcedureOrderDetails.EditableGrid.datatable.clear().draw();
        var ProcedureOrderTestLoadJSONData = JSON.parse(utility.decodeHtml(response.procedureOrderTest_JSON));
        //Start 05-04-2016 Humaira Yousaf to add child row
        $.each(ProcedureOrderTestLoadJSONData, function (i, item, CurrentRow, newChildRow) {
            var ProcedureOrderTestId = item.ProcedureOrderTestId;
            var arrChildProcedure = [];
            arrChildProcedure.push(item);
            CurrentRow = OrderSet_ProcedureOrderDetails.AddNewProcedureRow(ProcedureOrderTestId, null, null, item.CPTCode, item.CPTDescription, item.CPTDescription, null, null);
            OrderSet_ProcedureOrderDetails.buildRowChild(CurrentRow, CurrentRow.attr("id"), null, null, arrChildProcedure);

            var self = $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " tr#" + ProcedureOrderTestId);
            var row = OrderSet_ProcedureOrderDetails.EditableGrid.datatable.row(CurrentRow);
            item.CurrentRow = CurrentRow;
            newChildRow = row.child();
            item.newChildRow = newChildRow;
            var ProcedureTestTable = $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure");

            //Start Farooq Ahmad 03/28/2016 bind values to the table
            var counter = 0;
            var BindFunction = function (counter, item, CurrentRow, newChildRow) {
                if (CurrentRow == null) {
                    CurrentRow = item.CurrentRow;
                }
                if (newChildRow == null) {
                    newChildRow = item.newChildRow;
                }
                //Start 08-11-2016 Edit By Humaira Yousaf Bug# EMR-1925
                if (counter++ < 5 && $(CurrentRow).find('select option').length <= 1)
                    setTimeout(BindFunction, 1000, counter, item, CurrentRow, newChildRow);
                else {
                    utility.bindMyJSONByName(true, item, false, $(CurrentRow)).done(function () {
                        utility.bindMyJSONByName(true, item, false, $(newChildRow));
                    });
                }
                //End 08-11-2016 Edit By Humaira Yousaf Bug# EMR-1925
            }
            BindFunction(counter, item, CurrentRow, newChildRow);
            //End 05-04-2016 Humaira Yousaf to add child row
            //End Farooq Ahmad 03/28/2016 bind values to the table
        });
        if ($("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr").length > 0 && $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr").find('.dataTables_empty').length == 0) {
            $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #btnSaveOrder").removeClass('disableAll');
        }
        else {
            $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #btnSaveOrder").addClass('disableAll');
        }
    },
    bindAutoComplete: function (element) {
        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "OrderSet_ProcedureOrderDetails", null, true);

    },
    loadAllAutocomplete: function () {
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #txtAssignee");
            var hfCtrl = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #hfAssignee");
            var onSelect = function (dataItem) { $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #txtAssignee").attr("AssigneeId", dataItem.id); }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
        });
    },
    OpenAssignee: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmOSProcedureOrderDetail";
        params["AssigneeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_ProcedureOrderDetails";
        params["RefCtrl"] = "txtAssignee";
        params["RefCtrlHidden"] = "hfAssignee";
        params["RefCtrlLink"] = "lnkAssignee";
        LoadActionPan('Admin_Provider', params);
    },
    favoriteListSearch: function () {

        Favorite_ProcedureOrder.searchFavoriteList_DBCall("ProcedureOrder", null, 1, 5000).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $ddl = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #ddlFavoriteListProcedureOrder');
                var favouriteProcedures = JSON.parse(response.FavoriteListJSON)
                $ddl.empty();
                //Start 11-04-2016 Humaira Yousaf
                $ddl.append(
                   $('<option/>', {
                       id: -1,
                       value: -1,
                       html: '-Select-',
                   })
               );
                //End 11-04-2016 Humaira Yousaf
                $.each(favouriteProcedures, function (i, item) {

                    $ddl.append(
                      $('<option/>', {
                          id: item.FavoriteListId,
                          value: item.FavoriteListId,
                          html: item.Name,
                      })
                  );
                });
                if (favouriteProcedures.length > 0) {
                    EMRUtility.getFavListValue(OrderSet_ProcedureOrderDetails.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "" && response1.favListVal != "-1") {
                                if ($("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #ddlFavoriteListProcedureOrder option[value='" + response1.favListVal + "']").length > 0) {
                                    $ddl.val(response1.favListVal);
                                    $ddl.trigger("onchange");
                                }
                                else {
                                    if (favouriteProcedures.length == 1) {
                                        $ddl.val(favouriteProcedures[0].FavoriteListId);
                                        $ddl.trigger("onchange");
                                    }
                                    else if (favouriteProcedures.length > 1) {
                                        $ddl.trigger("onchange");
                                    }
                                }
                            }
                            else {
                                if (favouriteProcedures.length == 1) {
                                    $ddl.val(favouriteProcedures[0].FavoriteListId);
                                    $ddl.trigger("onchange");
                                }
                                else if (favouriteProcedures.length > 1) {
                                    $ddl.trigger("onchange");
                                }
                            }
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                var mystr = "";

            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });

    },
    selectAllFavoriteList: function () {
        $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #ulFavoriteListProcedureOrderContent li').each(function (i, item) {
            $(item).trigger("click");

        });
    },
    loadfavoriteListContent: function (obj, favListIds) {
        var $uL = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #ulFavoriteListProcedureOrderContent');
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            var selectedOptionValue = selectedOption.attr("id");
            //Start 12-07-2016 Muhammad Arshad Bug#EMR-1486 Lab order -> Favorite list -> "Select ALL " functionality is not working on 46 server ,please see attached video and resolve the issue
            var divSelectAllFavorites = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #divSelectAllProcedureOrderFavorites');
            if (selectedOptionValue > 0) {
                divSelectAllFavorites.removeClass("disableAll");

                ////Start 11-04-2016 Humaira Yousaf
                //if (selectedOption.attr("id") != "-1") {
                OrderSet_ProcedureOrderDetails.favoriteList_CPTSearch(selectedOptionValue);
            }
            else {
                $uL.empty();
                if (divSelectAllFavorites.hasClass("disableAll") == false) {
                    divSelectAllFavorites.addClass("disableAll");
                }
            }
            //End 12-07-2016 Muhammad Arshad Bug#EMR-1486 Lab order -> Favorite list -> "Select ALL " functionality is not working on 46 server ,please see attached video and resolve the issue

            //}
            //else {
            //    $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #ulFavoriteListProcedureOrderContent').empty();
            //    $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
            //}
            //End 11-04-2016 Humaira Yousaf
        }
        else {
            if (favListIds != null) {
                //clear the list

                $uL.empty();

                $.each(favListIds, function (index, item) {
                    //load all favList
                    OrderSet_ProcedureOrderDetails.favoriteList_CPTSearch(item, false);
                });
            }
        }
    },
    favoriteList_CPTSearch: function (FavoriteListId) {
        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(null, FavoriteListId, 1, 5000).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $UL = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #ulFavoriteListProcedureOrderContent');
                var objData = JSON.parse(response.FavoriteListCPTJSON);
                $UL.empty();
                //Start 11-04-2016 Humaira Yousaf
                if (objData.length > 0) {
                    $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #favSelectAllLink').removeClass('disableAll');
                }
                else {
                    $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
                }
                $.each(objData, function (i, item) {
                    if (item.CPTCodeDescription != "") {
                        var onclick = 'OrderSet_ProcedureOrderDetails.BindProcedureGridItem(\'' + item.CPTCode + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + String(item.CPTCodeDescription) + '\', this)';

                        var LiId = item.CPTCode + " " + item.CPTCodeDescription; //$('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #ddlFavoriteListProcedureOrder').find("option:selected").attr('id') + '-' + item.CPTCode;

                        var isFound = OrderSet_ProcedureOrderDetails.isFavoriteProcedureFound(LiId, item.CPTCodeDescription);
                        if (isFound == true) {
                            $UL.append('<li class="disableAll" onclick="' + onclick + '" id="' + LiId + '">' + item.CPTCodeDescription + '</li>');
                        }
                        else {
                            $UL.append('<li onclick="' + onclick + '" id="' + LiId + '">' + item.CPTCodeDescription + '</li>');
                        }
                    }
                });
                //End 11-04-2016 Humaira Yousaf

                //Favorite_ProcedureOrder.FavoriteListCPTGridLoad(response);
            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });
    },
    BindProcedureGridItem: function (cptCode, procedureDescription, cptDescription, sender, snomedcode, snomedDescription) {
        var cptCode = cptCode;
        var procDesc = procedureDescription;
        var cptDesc = cptDescription;

        var currentRow = $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr");
        var isTestAlreadySelected = false;
        $.each(currentRow, function (i, item) {
            var currentRowCPTDescription = $(item).find("input[id*='ProcedureProcedure']").val() != null ? $(item).find("input[id*='ProcedureProcedure']").val().replace(cptCode, "").trim() : "";
            if (cptDescription.toLowerCase().trim() == currentRowCPTDescription.toLowerCase().trim()) {
                isTestAlreadySelected = true;
                return;
            }
        });

        if (isTestAlreadySelected != true) {
            var CurrentRow = OrderSet_ProcedureOrderDetails.AddNewProcedureRow(null, null, null, cptCode, procDesc, cptDescription, snomedcode, snomedDescription);
            if ($(CurrentRow).attr('id') == '-1') {
                $(CurrentRow).loadDropDowns(true, null, 'dgvProcedure #-1').done(function () {
                });
            }
            //
            $(sender).addClass('disableAll');
            setTimeout(function () {
                $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #txtProcedureCPTCode").val('');
            }, 200);
        }
        else {
            utility.DisplayMessages("Procedure is already selected", 2);
        }
        if ($("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr").length > 0 && $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr").find('.dataTables_empty').length == 0) {
            $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #btnSaveOrder").removeClass('disableAll');
        }
        else {
            $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #btnSaveOrder").addClass('disableAll');
        }
    },
    AddNewProcedureRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription, snomedcode, snomedDescription) {

        var ProcedureGridId = "#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure";

        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (OrderSet_ProcedureOrderDetails.params.ParentCtrl != null) {
                CurrentRow = OrderSet_ProcedureOrderDetails.EditableGrid.rowAdd(RowId, "");
            }
            else {
                CurrentRow = OrderSet_ProcedureOrderDetails.EditableGrid.rowAdd(RowId, OrderSet_ProcedureOrderDetails.params.ProcedureId);
            }

        }
        else {
            var TemplateRow = $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            CurrentRow = OrderSet_ProcedureOrderDetails.EditableGrid.rowAdd(TemplateRowId - 1, "");
        }

        var rowId = CurrentRow.attr("id");

        var cptCodeHtml = '<input type="hidden" id="CPTCode' + rowId + '"  name="CPTCode" value="' + cptCode + '" />';
        var cptDescHtml = '<input type="hidden" id="CPTDescription' + rowId + '" name="CPTDescription"  value="' + cptDescription + '"  />';

        $(CurrentRow).find('td:first').append(cptCodeHtml + cptDescHtml);


        var SnomedCodeHtml = '<input type="hidden" id="CPTSNOMEDCodeId' + rowId + '"  name="CPTSNOMEDCodeId" value="' + snomedcode + '" />';
        var SnomedDescHtml = '<input type="hidden" id="CPTSNOMEDDescription' + rowId + '" name="CPTSNOMEDDescription"  value="' + snomedDescription + '"  />';

        $(CurrentRow).find('td:first').append(SnomedCodeHtml + SnomedDescHtml);


        var row = OrderSet_ProcedureOrderDetails.EditableGrid.datatable.row(CurrentRow);
        //Start 05-04-2016 Humaira Yousaf to add child row
        row.child(OrderSet_ProcedureOrderDetails.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));
        row.child.hide();
        //End 05-04-2016 Humaira Yousaf to add child row
        $(CurrentRow).find('input[id*="ProcedureProcedure"]').val(cptCode + " " + cptDescription);
        //Start 12-04-2016 Humaira Yousaf
        var selectedFavListId = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #ddlFavoriteListProcedureOrder').find("option:selected").attr('id');
        //  $(CurrentRow).find('input[id*="ProcedureProcedure"]').attr("listCPTid", cptCode);
        if (cptCode != null && cptDescription != null) {
            $(CurrentRow).find('input[id*="ProcedureProcedure"]').attr("listCPTid", cptCode + " " + cptDescription);
        }
        else {
            $(CurrentRow).find('input[id*="ProcedureProcedure"]').attr("listCPTid", null);
        }
        //End 12-04-2016 Humaira Yousaf
        OrderSet_ProcedureOrderDetails.enableRemoveRow($(CurrentRow));


        //Start Farooq Ahmad 07/14/2016 /EMR-588
        var dgvProcedure = $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure");

        $(dgvProcedure).find('input[id*="dtpProcedureDate"]').removeClass('size70');
        $(dgvProcedure).find('input[id*="dtpProcedureDate"]').closest('div').removeClass('size70');

        $(dgvProcedure).find('input[id*="tpProcedureTime"]').removeClass('size70');
        $(dgvProcedure).find('input[id*="tpProcedureTime"]').closest('div').removeClass('size70');

        $(dgvProcedure).find('input[id*="ProcedureProcedure"]').addClass('size-min300');
        //End Farooq Ahmad 07/14/2016 /EMR-588
        return CurrentRow;
    },
    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div></div>");

        var comments = "<div class='col-md-10 pb-sm pl-xlg'><label class='control-label'>Comments</label><input type='text' maxlength='500' class='form-control'  id='Comments" + ParentRowId + "'  name='Comments'></input></div>";
        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(comments);
        return ChildHTML;

    },
    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
    },
    rowExpand: function ($row, obj) {


        var _self = obj,
            $actions,
            values = [];
        var row = _self.datatable.row($row);
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
        }

    },
    rowRemove: function ($row, obj) {
        utility.myConfirm('1', function () {
            if ($row.hasClass('adding')) {
            }
            //var _self = obj;
            //_self.datatable.row($row.get(0)).remove().draw();
            if (parseInt($row.attr("id")) > 0) {
                OrderSet_ProcedureOrderDetails.DeleteProcedureOrderTest($row.attr("id"), $row, obj);
            }
            else {
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
                utility.DisplayMessages("Successfully Deleted", 1);
                //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
                if ($("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr").length > 0 && $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr").find('.dataTables_empty').length == 0) {
                    $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #btnSaveOrder").removeClass('disableAll');
                }
                else {
                    $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #btnSaveOrder").addClass('disableAll');
                }
                //End 05-04-2016 Humaira Yousaf to enable/disable action buttons

                //Start 12-04-2016 Humaira Yousaf
                OrderSet_ProcedureOrderDetails.enableFavoriteList($row);
                //Start 12-04-2016 Humaira Yousaf
            }

        }, function () {
        },
                    '1'
    );

    },
    enableFavoriteList: function (deleteRow) {
        $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #ulFavoriteListProcedureOrderContent li').each(function (index, item) {
            if ($(deleteRow).find("input[id*='ProcedureProcedure']").attr("listCPTid") != null) {
                var deleteRowCPTCode = $(deleteRow).find("input[id*='ProcedureProcedure']").val() != null ? $(deleteRow).find("input[id*='ProcedureProcedure']").attr('listCPTid') : "";
                if (deleteRowCPTCode == $(item).attr("id")) {
                    $(item).removeClass('disableAll');
                }
            }
            else {
                var deleteRowCPTCode = $(deleteRow).find("input[id*='ProcedureProcedure']").val() != null ? $(deleteRow).find("input[id*='ProcedureProcedure']").val() : "";
                if (deleteRowCPTCode == $(item).attr("id")) {
                    $(item).removeClass('disableAll');
                }
            }
        });
    },
    DeleteProcedureOrderTest: function (ProcedureTestId, $row, obj) {
        OrderSet_ProcedureOrderDetails.ProcedureOrderTest_DBCall(ProcedureTestId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
                if ($("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr").length > 0 && $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr").find('.dataTables_empty').length == 0) {
                    $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #btnSaveOrder").removeClass('disableAll');
                }
                else {
                    $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #btnSaveOrder").addClass('disableAll');
                }
                OrderSet_ProcedureOrderDetails.enableFavoriteList($row);
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ProcedureOrderTest_DBCall: function (ProcedureTestId) {
        var objData = new Object();
        objData["ProcedureOrderTestId"] = ProcedureTestId;
        objData["commandType"] = "DELETE_PROCEDUREORDER_TEST";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProcedureOrder");
    },
    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_ProcedureOrderDetails";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = OrderSet_ProcedureOrderDetails.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, OrderSet_ProcedureOrderDetails.params.PanelID);
    },
    loadProblemList: function () {
        var dfd = new $.Deferred();
        OrderSet_ProcedureOrderDetails.SearchProblemList().done(function (response) {
            var obj = JSON.parse(response);
            if (obj.status != false) {
                if (obj.ProblemListCount > 0) {
                    var ProblemListLoadJSONData = JSON.parse(obj.ProblemListLoad_JSON);
                    var ProblemListHistoryLoadJSONData = JSON.parse(obj.ProblemListHistoryLoad_JSON);
                    var finalTr = '';
                    var counter = 2;
                    $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #ulProblemLists tbody tr").remove();
                    $.each(ProblemListLoadJSONData, function (i, item) {
                        if (counter % 2 == 0) {
                            finalTr = finalTr + '<tr>';
                        }

                        try {
                            if (item.Description == null || item.Description == '') {
                                item.Description = '';
                                if (item.ICD9 != null && item.ICD9 != '') {
                                    item.Description = item.ICD9 + ' - ';
                                }
                                if (item.ICD10 != null && item.ICD10 != '') {
                                    item.Description = item.Description + item.ICD10 + ' - ';
                                }
                                if (item.SNOMEDID != null && item.SNOMEDID != '') {
                                    item.Description = item.Description + item.SNOMEDID + ' - ';
                                }
                                if (item.ICD10_Description != null && item.ICD10_Description != '') {
                                    item.Description = item.Description + item.ICD10_Description;
                                }
                                else if (item.ICD9_Description != null && item.ICD9_Description != '') {
                                    item.Description = item.Description + item.ICD9_Description;
                                }
                                else if (item.ProblemName != null && item.ProblemName != '') {
                                    item.Description = item.Description + item.ProblemName;
                                }

                            }
                        } catch (ex) {
                            console.log(ex);
                        }


                        finalTr = finalTr + '<td><div class="col-xs-12 p-xs"><div class="checkbox-custom">';

                        var checked = OrderSet_ProcedureOrderDetails.isCheckedProblem('chk' + item.ProblemListId + '') ? "checked" : "";
                        finalTr = finalTr + '<input ' + checked + ' type="checkbox" onclick ="OrderSet_ProcedureOrderDetails.pushProblems(this);" name="' + item.Code + '" value="' + item.ProblemListId + '"  id="chk' + item.ProblemListId + '">';
                        finalTr = finalTr + '   <label class="control-label">' + item.Description + '</label></div></div></td>';

                        if (counter % 2 == 1) {
                            finalTr = finalTr + '</tr>';
                        }
                        counter++;
                        var color = "";
                    });
                    $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #ulProblemLists tbody").append(finalTr);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve();
        });
        return dfd.promise();
    },
    SearchProblemList: function () {
        var objData = new Object();
        objData["OrderSetId"] = OrderSet_ProcedureOrderDetails.params.OrderSetId;
        objData["IsActive"] = '1';
        objData["ProblemListId"] = null;
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 2000;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        objData["NoteId"] = 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProblemList");

    },
    addProblem: function () {
        var params = [];
        params["OrderSetId"] = OrderSet_ProcedureOrderDetails.params.OrderSetId;
        params["RefForm"] = "frmOSProcedureOrderDetail";
        params["FromOrderDetail"] = "1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_ProcedureOrderDetails";
        LoadActionPan('OrderSet_Problems', params);
    },
    pushProblems: function (obj) {
        var $obj = $(obj);
        var isChecked = $obj.prop('checked');
        var id = $obj.attr('id');
        if (isChecked) {
            //Push in checked problems
            if (!($.inArray(id, OrderSet_ProcedureOrderDetails.checkedProblems) != -1)) {
                OrderSet_ProcedureOrderDetails.checkedProblems.push(id);
            }
        }
        else {
            //Remove from checked problems
            OrderSet_ProcedureOrderDetails.checkedProblems.splice($.inArray(id, OrderSet_ProcedureOrderDetails.checkedProblems), 1);
        }
    },
    isCheckedProblem: function (problemId) {
        return $.inArray(problemId, OrderSet_ProcedureOrderDetails.checkedProblems) == -1 ? false : true;
    },
    isFavoriteProcedureFound: function (favCPTCode, favCPTDesc) {

        var isFound = false;
        $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr").each(function (index, item) {
            if ($(item).find("input[id*='ProcedureProcedure']").attr("listCPTid") != null) {
                var currentRowCPTCode = $(item).find("input[id*='ProcedureProcedure']").val() != null ? $(item).find("input[id*='ProcedureProcedure']").attr('listCPTid') : "";
                if (currentRowCPTCode == favCPTCode) {
                    isFound = true;
                }
            }
            else {
                var currentRowCPTCode = $(item).find("input[id*='ProcedureProcedure']").val() != null ? $(item).find("input[id*='ProcedureProcedure']").val() : "";
                if (currentRowCPTCode == favCPTCode) {
                    isFound = true;
                }
            }
        });

        return isFound;
    },
    validateProcedureOrderDetail: function () {
        $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #frmOSProcedureOrderDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {


               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (e.type == "success") {

                //Start 22-03-2016 Humaira Yousaf for multiple submit buttons
                var $form = $(e.target);
                var $button = $form.data('bootstrapValidator').getSubmitButton();
                switch ($button.attr('id')) {
                    case 'btnSaveOrder':
                        OrderSet_ProcedureOrderDetails.ProcedureOrderSave('save');
                        break;
                    default:
                        break;
                }
                //End 22-03-2016 Humaira Yousaf for multiple submit buttons
            }
            e.type = "";
        });

    },
    ProcedureOrderSave: function (sender) {
        var FavVal = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #ddlFavoriteListProcedureOrder').val();
        var strMessage = "";
        var ProcedureOrderId = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #hfProcedureOrderID").val() != "" ? $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #hfProcedureOrderID").val() : "-1";
        if (parseInt(ProcedureOrderId) > 0) {
            OrderSet_ProcedureOrderDetails.params.mode = "Edit";
        }
        else {
            OrderSet_ProcedureOrderDetails.params.mode = "Add";
        }

        var self = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail");
        var mainErrorMessage = "";
        if (mainErrorMessage == "") {
            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);


            var ProcedureOrderProblemList = [];
            $(self).find("#ulProblemLists td").each(function (index, item) {
                if ($(this).find("input:checkbox").prop("checked")) {
                    var objProblem = {
                        ProblemId: $(this).find("input:checkbox").val(),
                        Description: $(this).text()
                    }
                    ProcedureOrderProblemList.push(objProblem);
                }
            });
            objData["ProcedureOrderProblemList"] = ProcedureOrderProblemList;
            //Start 06-04-2016 Humaira Yousaf for problems list
            OrderSet_ProcedureOrderDetails.ProcedureOrderProblems = ProcedureOrderProblemList;
            //End 06-04-2016 Humaira Yousaf for problems list
            //Start 22-03-2016 Humaira Yousaf for status
            if (sender == 'signorder' || sender == 'signprintorder')
                objData["Status"] = 'Signed';
            else if (sender == 'save')
                objData["Status"] = 'Draft';
            //End 22-03-2016 Humaira Yousaf for status
            if (objData.OrderDate != null) objData.ProcedureOderDate = objData.OrderDate;
            if (objData.OrderTime != null) objData.ProcedureOderTime = objData.OrderTime;

            // objData["CPTSNOMEDCodeId"] = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #hfCPTSNOMEDCode').val();
            // objData["CPTSNOMEDDescription"] = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + ' #hfCPTSNOMEDDescription').val();

            myJSON = JSON.stringify(objData);

            //------------------------------------------------------------
            var ProcedureTestIds = $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr:not([id*=Child]").map(function () {
                return this.id.replace("id", "");
            }).get().join(',');
            $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #pnlProcedure_Result #hfProcedureTestIds").val(ProcedureTestIds);
            var selfProcedureTest = $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #pnlProcedure_Result");
            var myJSONProcedureTest = selfProcedureTest.getMyJSON();
            var objRad = new Object();
            objRad["ProcedureTestIds"] = ProcedureTestIds;
            JSONToSave = utility.MergeJSON(myJSON, myJSONProcedureTest);
            //Start 08-03-2016 Humaira Yousaf to save test child row data
            $("#" + OrderSet_ProcedureOrderDetails.params.PanelID + " #dgvProcedure tbody tr:not([id*=Child]").each(function (index, item) {
                var childExists = OrderSet_ProcedureOrderDetails.EditableGrid.datatable.row(index).child();

                if (typeof childExists != 'undefined') {
                    var childRow = OrderSet_ProcedureOrderDetails.EditableGrid.datatable.row(index).child().get();
                    if ($(childRow).length > 0) {

                        var childRowDataJson = $(childRow).getMyJSON();
                        JSONToSave = utility.MergeJSON(JSONToSave, childRowDataJson)
                    }
                }
            });
            //End 08-03-2016 Humaira Yousaf to save test child row data

            var data = JSON.stringify(objRad);
            JSONToSave = utility.MergeJSON(data, JSONToSave);
            JSONToSave = decodeURIComponent(JSONToSave);
            //--------------------------------------------------------------
            //Start 06-04-2016 Humaira Yousaf to save procedure order detail data

            //End 06-04-2016 Humaira Yousaf to save procedure order detail data


            // var isSavedSuccessfully = false;
            if (OrderSet_ProcedureOrderDetails.params.mode == "Add") {

                //AppPrivileges.GetFormPrivileges("Orders and Results_Procedure", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                //    if (strMessage == "") {
                OrderSet_ProcedureOrderDetails.saveProcedureOrder(JSONToSave).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var isFavListOpened = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #favSectionDiv").hasClass("toggled");
                        $.when(EMRUtility.insertUpdateFavListStatus(OrderSet_ProcedureOrderDetails.FavListName, !isFavListOpened)).then(function () {
                            OrderSet_ProcedureOrderDetails.SaveFavListVal(FavVal);
                        });
                        $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #hfProcedureOrderID").val(response.procedureOrderId);
                        utility.DisplayMessages(response.message, 1);
                        try {
                            Clinical_OrderSetDetails.ProcedureOrderSearch(null, null, null);
                        }
                        catch (ex) {
                            console.log(ex);
                        }
                        OrderSet_ProcedureOrderDetails.UnLoad('saveExit');
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
                //    }
                //    else
                //        utility.DisplayMessages(strMessage, 2);
                //});
            }
            else if (OrderSet_ProcedureOrderDetails.params.mode == "Edit") {

                //AppPrivileges.GetFormPrivileges("Orders and Results_Procedure", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                //    if (strMessage == "") {
                OrderSet_ProcedureOrderDetails.updateProcedureOrder(JSONToSave, ProcedureOrderId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var isFavListOpened = $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #frmOSProcedureOrderDetail #favSectionDiv").hasClass("toggled");
                        $.when(EMRUtility.insertUpdateFavListStatus(OrderSet_ProcedureOrderDetails.FavListName, !isFavListOpened)).then(function () {
                            OrderSet_ProcedureOrderDetails.SaveFavListVal(FavVal);
                        });

                        $('#' + OrderSet_ProcedureOrderDetails.params.PanelID + " #hfProcedureOrderID").val(response.procedureOrderId);

                        utility.DisplayMessages(response.message, 1);
                        try {
                            Clinical_OrderSetDetails.ProcedureOrderSearch(null, null, null);
                        }
                        catch (ex) {
                            console.log(ex);
                        }

                        OrderSet_ProcedureOrderDetails.UnLoad('saveExit');
                        $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
                //    }
                //    else
                //        utility.DisplayMessages(strMessage, 2);
                //});

            }
        }
        else {
            utility.DisplayMessages(mainErrorMessage, 3);
        }
    },
    saveProcedureOrder: function (ProcedureOrderData) {
        var objData = JSON.parse(ProcedureOrderData);
        objData["commandType"] = "SAVE_PROCEDURE_ORDER";
        objData["OrderSetId"] = OrderSet_ProcedureOrderDetails.params.OrderSetId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProcedureOrder");
    },
    updateProcedureOrder: function (ProcedureOrderData, ProcedureOrderId) {

        var objData = JSON.parse(ProcedureOrderData);
        objData["ProcedureOrderId"] = ProcedureOrderId;
        objData["OrderSetId"] = OrderSet_ProcedureOrderDetails.params.OrderSetId;
        objData["commandType"] = "Save_Procedure_Order";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProcedureOrder");

    },
    SaveFavListVal: function (FavListVal) {
        EMRUtility.insertUpdateFavListVal(OrderSet_ProcedureOrderDetails.FavListName, FavListVal);
    },
    UnLoad: function () {
        OrderSet_ProcedureOrderDetails.checkedProblems = [];
        if (OrderSet_ProcedureOrderDetails.params != null && OrderSet_ProcedureOrderDetails.params.ParentCtrl && OrderSet_ProcedureOrderDetails.params.ParentCtrlPanelID) {
            UnloadActionPan(OrderSet_ProcedureOrderDetails.params.ParentCtrl, "OrderSet_ProcedureOrderDetails", null, OrderSet_ProcedureOrderDetails.params.ParentCtrlPanelID);
        }
        else if (OrderSet_ProcedureOrderDetails.params != null && OrderSet_ProcedureOrderDetails.params.ParentCtrl) {
            UnloadActionPan(OrderSet_ProcedureOrderDetails.params.ParentCtrl, "OrderSet_ProcedureOrderDetails");
        }
        else {
            UnloadActionPan(null, "OrderSet_ProcedureOrderDetails");
        }
        OrderSet_ProblemListGrid.ProblemListsSearch();
    },
}