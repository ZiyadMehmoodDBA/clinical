OrderSet_Procedures = {
    bIsFirstLoad: true,
    params: [],
    Procedures: [],
    ProceduresDetail: [],

    IsUpdate: false,
    EditableGrid: null,
    EditableGridAdd: null,
    myGrid: null,
    FavListName: "ClinicalProcedureDetail",
    PreviousComments: '',
    Load: function (params) {
        OrderSet_Procedures.params = params;
        OrderSet_Procedures.params.mode = "Add";
        OrderSet_Procedures.favoriteListSearch();
        if (OrderSet_Procedures.params.PanelID != 'pnlOrderSetProcedures') {
            OrderSet_Procedures.params.PanelID = OrderSet_Procedures.params.PanelID + ' #pnlOrderSetProcedures';
        } else {
            OrderSet_Procedures.params.PanelID = 'pnlOrderSetProcedures';
        }

        if (OrderSet_Procedures.params.OrderSetId != "") {
            $("#" + OrderSet_Procedures.params.PanelID + " #hfOrderSetId").val(OrderSet_Procedures.params.OrderSetId);
        }

        var PanelProcedureGrid = "#" + OrderSet_Procedures.params.PanelID + " #pnlProcedure_ResultAdd";
        var ProcedureGridId = "#" + OrderSet_Procedures.params.PanelID + " #dgvProcedureAdd";
        $(ProcedureGridId + " tbody tr").remove();
        OrderSet_Procedures.EditableGridAdd = utility.MakeEditableGrid(PanelProcedureGrid, ProcedureGridId, OrderSet_Procedures, "0", false, false, false, false);

        if (!$("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedureAdd_wrapper").hasClass('no-footer height-max150 overflowY')) {
            $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedureAdd_wrapper").addClass('no-footer height-max150 overflowY')
        }



        OrderSet_Procedures.domReadyFunc();
        var self = $('#' + OrderSet_Procedures.params.PanelID);
        //AST - 74 By:MAhmad
        self.find('.Diagnosis > select').attr('ddlist', 'LookupProblemListsForOrderSet');
        //AST - 74 By:MAhmad
        var data = "IsActive=&ID=" + OrderSet_Procedures.params.OrderSetId;

        var self = $('#' + OrderSet_Procedures.params.PanelID);
        if (OrderSet_Procedures.bIsFirstLoad == true) {
            //for Favorites toggle
            EMRUtility.setFavoriteSectionStyle(OrderSet_Procedures.params.PanelID);
            self.find('.Diagnosis').loadDropDowns(true, data).done(function () {
                OrderSet_Procedures.ProceduresSearch();
                OrderSet_Procedures.bIsFirstLoad = false;
                //Serialization
                $('#' + OrderSet_Procedures.params.PanelID + ' #frmOrderSetProcedures').data('serialize', $('#' + OrderSet_Procedures.params.PanelID + ' #frmOrderSetProcedures').serialize());
                OrderSet_Procedures.domReadyFunction();
            });
        }

        OrderSet_Procedures.Procedures = [];
        OrderSet_Procedures.ProceduresDetail = [];

        if ($.fn.dataTable.isDataTable("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedure_ResultAdd #dgvProcedureAdd")) {
            $("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedure_ResultAdd #dgvProcedureAdd").dataTable().fnClearTable();
            $("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedure_ResultAdd #dgvProcedureAdd").dataTable().fnDestroy();
        }

        $("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedure_ResultAdd #dgvProcedureAdd tbody").find("tr").remove();


        //for favorite list setting for all favLists
        if (EMRUtility.getFavListStatus(OrderSet_Procedures.FavListName))
            $('#' + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures #favSectionDiv").removeClass("toggled");
        else
            $('#' + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures #favSectionDiv").addClass("toggled");
        //for favorite list setting for all favLists
    },

    //Implimented ready function which run at load time
    domReadyFunc: function () {

        $('#' + OrderSet_Procedures.params.PanelID + ' #sectionProceduresDetails').on('keydown', '#UnitId', function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });

    },

    //Reason: Function to load procedure Order Fav List
    favoriteListSearch: function () {
        Favorite_ProcedureOrder.searchFavoriteList_DBCall("Procedure", null, 1, 5000).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $ddl = $('#' + OrderSet_Procedures.params.PanelID + ' #ddlFavoriteListProcedure');
                var favouriteProcedures = JSON.parse(response.FavoriteListJSON)
                $ddl.empty();
                $ddl.append(
                         $('<option/>', {
                             value: "",
                             html: "- Select -",
                         })
                       );
                $.each(favouriteProcedures, function (i, item) {
                    if (item.Name != "") {
                        $ddl.append(
                          $('<option/>', {
                              id: item.FavoriteListId,
                              value: item.FavoriteListId,
                              html: item.Name,
                          })
                        );
                    }

                });
                if (favouriteProcedures.length > 0) {
                    $ddl.trigger("onchange");
                }

            }

        });

    },

    //Reason: Function to load procedure Order Fav List Test while selection is done
    loadfavoriteListContent: function (obj) {
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            if (selectedOption.val() != "") {
                OrderSet_Procedures.favoriteList_CPTSearch(selectedOption.attr("id"));
            }
            else {
                var $UL = $('#' + OrderSet_Procedures.params.PanelID + ' #ulFavoriteListProcedureContent');
                $UL.empty();
            }
        }
    },

    //Reason: Function to load procedure Order Fav List Test while selection is done
    selectAllFavoriteListContent: function () {

        $('#' + OrderSet_Procedures.params.PanelID + ' #ulFavoriteListProcedureContent li').each(function (i, item) {
            $(item).trigger("click");
        });


    },

    //Reason: Function to load procedure Order Fav List
    favoriteList_CPTSearch: function (FavoriteListId) {
        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(null, FavoriteListId, 1, 5000).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $UL = $('#' + OrderSet_Procedures.params.PanelID + ' #ulFavoriteListProcedureContent');
                var objData = JSON.parse(response.FavoriteListCPTJSON);
                $UL.empty();

                $.each(objData, function (i, item) {
                    if (item.CPTCodeDescription != "") {
                        var onclick = 'OrderSet_Procedures.BindProcedureGridItem(\'' + item.CPTCode + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + item.SNOMEDID + '\',\'' + String(item.SNOMED_DESCRIPTION) + '\')';
                        $UL.append('<li id="' + item.CPTCode + '" onclick="' + onclick + '">' + item.CPTCode + " " + item.CPTCodeDescription + '</li>');
                    }
                });



            }
        });
    },
    DisableFavProcedure: function (ProcedureId) {

        $('#' + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures ul#ulFavoriteListProcedureContent li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == ProcedureId) {
                $(this).addClass("disableAll");
            }
        });
    },
    EnableFavProcedure: function (ProcedureDes) {
        ProcedureDes = ProcedureDes.split("(")[0].trim();
        $('#' + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures ul#ulFavoriteListProcedureContent li").each(function (i, item) {
            var text = $(this).html();
            if ($(this).html() == ProcedureDes) {
                $(this).removeClass("disableAll");
            }
        });
    },
    EnableAllFavProcedure: function () {
        $('#' + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures ul#ulFavoriteListProcedureContent li").each(function (i, item) {

            if ($(this).hasClass('disableAll')) {
                $(this).removeClass("disableAll");
            }
        });
    },

    BindProcedureGridItem: function (cptCode, procedureDescription, cptDescription, SnomedID, SnomedDescription) {

        var cptCode = cptCode;
        var procDesc = procedureDescription;
        var cptDesc = cptDescription;

        var currentRow = $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedureAdd tbody tr");
        var isTestAlreadySelected = false;
        OrderSet_Procedures.DisableFavProcedure(cptCode);
        $.each(currentRow, function (i, item) {
            var currentRowCPTDescription = $(item).find("input[id*='ProcedureProcedure']").val() != null ? $(item).find("input[id*='ProcedureProcedure']").val().replace(cptCode, "").trim() : "";
            if (currentRowCPTDescription.indexOf("(SCT") > 0) {
                currentRowCPTDescription = currentRowCPTDescription.substring(0, currentRowCPTDescription.indexOf("(SCT")).trim();
            }
            if (cptDescription.toLowerCase().trim() == currentRowCPTDescription.toLowerCase()) {
                isTestAlreadySelected = true;
                return;
            }
        });

        if (isTestAlreadySelected != true) {

            OrderSet_Procedures.AddNewProcedureRow(null, null, null, cptCode, procDesc, cptDescription, SnomedID, SnomedDescription);
            setTimeout(function () {
                $("#" + OrderSet_Procedures.params.PanelID + " #txtProcedureCPTCode").val('');
                $('input[id*=txtUnit]').on("input", function () {
                    var val = Math.abs(parseInt(this.value, 10) || 1);
                    this.value = val > 999 ? 999 : val;
                });
            }, 200);
        }
        else {


            utility.myConfirm('This code already exists, do you want to add this code again?', function () {
                OrderSet_Procedures.AddNewProcedureRow(null, null, null, cptCode, procDesc, cptDescription, SnomedID, SnomedDescription);
                setTimeout(function () {
                    $("#" + OrderSet_Procedures.params.PanelID + " #txtProcedureCPTCode").val('');
                    $('input[id*=txtUnit]').on("input", function () {
                        var val = Math.abs(parseInt(this.value, 10) || 1);
                        this.value = val > 999 ? 999 : val;
                    });
                }, 200);

            },
            function () {
                $("#" + OrderSet_Procedures.params.PanelID + " #txtProcedureCPTCode").val('');
                return false;
            },
            'Confirmation duplicate CPT');

        }
    },

    validateUnits: function (value, id) {

        var val = Math.abs(parseInt(value, 10) || 1);
        value = val > 999 ? 999 : val;
        if ($(id).attr('id').indexOf('txtModifiertxtpUnits') > -1) {

        }
        else {
            $(id).val(value);
        }


    },

    resetValues: function () {
        $('#' + OrderSet_Procedures.params.PanelID + ' #sectionProceduresDetails').resetAllControls(null);
        utility.CreateDatePicker('frmOrderSetProcedures #dpStartDate', function () {
            //on-change callback method
        });
        utility.CreateDatePicker('frmOrderSetProcedures #dpEndDate', function () {
            //on-change callback method
        });


    },


    bindAutoComplete: function (element) {


        var hiddenCrtl = $('#' + OrderSet_Procedures.params.PanelID + ' #txtCPTCode');
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "OrderSet_Procedures", null, true);

    },

    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_Procedures";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = OrderSet_Procedures.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, OrderSet_Procedures.params.PanelID);
    },

    validateProcedure: function () {


        var cptDescription = $('#' + OrderSet_Procedures.params.PanelID + ' #hfCPTDescription').val();
        var cptCode = $('#' + OrderSet_Procedures.params.PanelID + ' #hfCPTCode').val();
        var $txtCptCode = $('#' + OrderSet_Procedures.params.PanelID + ' #txtCPTCode');
        if ($txtCptCode.val() == "") {
            return true;
        }
        //Start Logic to compare txtCPT values with IMO
        if (cptCode == "") {
            if (cptDescription != $txtCptCode.val()) {
                utility.DisplayMessages("Procedure not Valid", 2);
                $txtCptCode.val('');
                return false;
            }
        }
            //End added Logic to compare txtCPT values with IMO
        else if (cptCode + " - " + cptDescription != $txtCptCode.val()) {
            utility.DisplayMessages("Procedure not Valid", 2);
            $txtCptCode.val('');
            return false;
        }
        else
            return true;

    },

    DownloadProblems: function () {
        var objData = [];
        objData["commandType"] = "download_data";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "OrderSet", "downloadProblemList");
    },

    BindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "OrderSet_Procedures", null, false);
    },

    // Validate and save/edit functions

    ValidateProcedures: function () {
        $('#frmOrderSetProcedures')
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
            OrderSet_Procedures.ProceduresSave();
        });
    },


    ProceduresSave: function () {
        var objDeffered = $.Deferred();

        if ($("#" + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures #pnlProcedure_ResultAdd tbody tr").length != 0) {
            if ($("#" + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures #pnlProcedure_ResultAdd tbody tr td").length != 1) {

                $("#" + OrderSet_Procedures.params.PanelID + " #hfOrderSetId").val(OrderSet_Procedures.params.OrderSetId);
                var self = $("#" + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures");
                if (OrderSet_Procedures.params.mode == "Add") {
                    var hfProblemText = $("#" + OrderSet_Procedures.params.PanelID + " #hfIMOProblem").val();
                    var changesProblemText = $("#" + OrderSet_Procedures.params.PanelID + " #txtProblems").val();

                    OrderSet_Procedures.SaveProcedures().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            OrderSet_Procedures.SaveFavToggelStatus();

                            OrderSet_Procedures.ProceduresSearch();

                            utility.DisplayMessages(response.Message, 1);
                            $('#' + OrderSet_Procedures.params.PanelID + ' #frmOrderSetProcedures').resetAllControls(null);
                            $("#" + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures #pnlProcedure_ResultAdd tbody tr").each(function () {
                                OrderSet_Procedures.EditableGridAdd.datatable.row($(this).get(0)).remove().draw();
                            });
                            $("#" + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures #pnlProcedure_ResultAdd tbody tr").remove();
                            var $UL = $('#' + OrderSet_Procedures.params.PanelID + ' #ulFavoriteListProcedureContent');
                            $UL.empty();
                            OrderSet_Procedures.ProceduresDetail = [];
                            OrderSet_Procedures.Procedures = [];
                            OrderSet_Procedures.EnableAllFavProcedure();
                            objDeffered.resolve();

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else if (OrderSet_Procedures.params.mode == "Edit") {

                    OrderSet_Procedures.UpdateProcedures(myJSON, OrderSet_Procedures.params.ProcedureId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            OrderSet_Procedures.ProceduresSearch();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
            }
            else {
                utility.DisplayMessages("There is no Procedure to Add", 3);
            }
        }
        else {
            utility.DisplayMessages("There is no Procedure to Add", 3);
        }
        return objDeffered;
    },

    SaveProcedures: function () {
        var objData = {};
        objData["procedureDetailModel"] = OrderSet_Procedures.GetAllNewProcedures();
        objData["commandType"] = "save_procedures";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Procedure");
    },
    GetAllNewProcedures: function () {
        $("#" + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures #pnlProcedure_ResultAdd tbody tr:not([id*=Child])").each(function () {
            var myJSON = $(this).getMyJSONByName();
            var objDetail = JSON.parse(myJSON);
            var RowId = $(this).attr("id");
            var ChildData = $("#" + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures #pnlProcedure_ResultAdd tbody tr#Child" + RowId).getMyJSONByName();
            var parseChildData = JSON.parse(ChildData);
            objDetail["Comments"] = parseChildData["Comments"];
            $.grep(OrderSet_Procedures.Procedures, function (item, index) {
                if (item.ProcedureId == RowId) {
                    objDetail["CPTCode"] = item.CPTCode;
                    objDetail["CPT_DESCRIPTION"] = item.CPT_DESCRIPTION;
                    objDetail["ProcedureId"] = item.ProcedureId;
                    objDetail["SNOMEDID"] = item.SNOMEDID;
                    objDetail["SNOMED_DESCRIPTION"] = item.SNOMED_DESCRIPTION;
                    return;
                }
            });
            objDetail["OrderSetId"] = OrderSet_Procedures.params.OrderSetId;

            OrderSet_Procedures.ProceduresDetail.push(objDetail);
        });
        return OrderSet_Procedures.ProceduresDetail;
    },
    NoKnownProblem: function () {
        var strMessage = "";
        var self = $("#" + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures");
        var myJSON = self.getMyJSONByName();
        OrderSet_Procedures.SaveProcedures(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#pnlProcedures_Result #btnNoKnownProblems").css("display", "none");
                utility.DisplayMessages(response.message, 1);
                OrderSet_Procedures.ProceduresSearch();
                $('#' + OrderSet_Procedures.params.PanelID + ' #frmOrderSetProcedures').resetAllControls(null);
            }
            else {
                //utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    callJs: function () {
        var objData = [];
        objData["commandType"] = "download_data";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "OrderSet", "callJs");
    },
    UpdateProcedures: function (ProceduresData, ProcedureId) {

        var isactive = null;
        isactive = $('#' + OrderSet_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(ProceduresData);
        if (objData.OrderSetId == '') {
            objData.OrderSetId = OrderSet_Procedures.params.OrderSetId;
        }
        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_VITALS";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "OrderSet", "Vitals");

    },

    // End Validate and save/edit functions

    // Search/Grid Load Functions
    //Adding Pagination
    ProceduresSearch: function (ProcedureId, PageNo, rpp) {
        if ($("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result").css("display") == "none") {
            $("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result").show();
        }

        OrderSet_Procedures.SearchProcedures(ProcedureId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures th#SelectRecord").remove();

                OrderSet_Procedures.ProceduresGridLoadNew(response);
                //Adding Pagination 
                var TableControl = OrderSet_Procedures.params.PanelID + " #dgvProcedures";
                var PagingPanelControlID = OrderSet_Procedures.params.PanelID + " #dgvProcedures_Paging";
                var ClassControlName = "OrderSet_Procedures";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ProcedureCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    OrderSet_Procedures.ProceduresSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);

                setTimeout(function () {
                    $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures th[controlname='LastUpdatedDate']").click();
                    $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures th[controlname='LastUpdatedDate']").click();
                }, 20);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ProceduresGridLoad: function (response) {
        if (response.ProcedureCount > 0) {
            OrderSet_Procedures.EditableGrid.datatable.clear().draw();

            var ProceduresLoadJSONData = JSON.parse(response.ProcedureLoad_JSON);

            $.each(ProceduresLoadJSONData, function (i, item) {
                var ProcedureId = item.ProcedureId;
                var CurrentRow = OrderSet_Procedures.AddNewProceduresRow(ProcedureId, "Edit", null);
                var self = $("#dgvProcedures tr#" + ProcedureId);
                utility.bindMyJSONByName(true, item, false, self).done(function () {

                });

                var row = OrderSet_Procedures.EditableGrid.datatable.row(CurrentRow);

                /********************************/
                var newChildRow = row.child();

                /********************************/


                row.child().loadDropDowns(true).done(function () {
                    utility.bindMyJSON(true, item, false, $(newChildRow));

                });
            });
        }
        else {
            $("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures").css("display", "none");

            $('#dgvProcedures').DataTable({
                "language": {
                    "emptyTable": "No Procedure List Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [8] }]
            });

        }
    },

    SearchProcedures: function (ProcedureId, PageNumber, RowsPerPage) {


        var IsCheckedIn = null;
        IsCheckedIn = $('#' + OrderSet_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');
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
        objData["OrderSetId"] = OrderSet_Procedures.params.OrderSetId;
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

    //Reason: Intailize toggle of favorites
    domReadyFunction: function () {
        $('#' + OrderSet_Procedures.params.PanelID + ' .toggleHorSmallLeft section').unbind('click').bind("click", function (e) {
            $(this).parent().toggleClass("toggled");
            ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));

        });

    },
    // Search/Grid Load Functions
    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div></div>");
        //AST - 74 By:MAhmad

        var Comments = "<div class='col-xs-12 pl-xlg mb-tiny'><label class='control-label'>Comments</label><textarea type='text' class='form-control' spellcheck='true' id='Comments" + ParentRowId + "'  name='Comments'></textarea></div>";


        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(Comments);
        return ChildHTML;
        //AST - 74 By:MAhmad
    },
    AddNewProceduresRow: function (RowId, mode, CurrRef, NotesId) {


        var CurrentRow = null;
        if (RowId && RowId > 0) {

            CurrentRow = OrderSet_Procedures.EditableGrid.rowAdd(RowId, OrderSet_Procedures.params.VitalSignsId, null, null, null, null, NotesId);

        }
        else {
            var TemplateRow = $("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            CurrentRow = OrderSet_Procedures.EditableGrid.rowAdd(TemplateRowId - 1, OrderSet_Procedures.params.VitalSignsId, null, null, null, null, null);

        }

        var row = OrderSet_Procedures.EditableGrid.datatable.row(CurrentRow);
        row.child(OrderSet_Procedures.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child.hide();
        OrderSet_Procedures.enableRemoveRow($(CurrentRow));
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

            OrderSet_Procedures.UpdateProceduresRow(myJSON, id).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    utility.DisplayMessages(response.Message, 1);
                    OrderSet_Procedures.ProceduresSearch();

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
            OrderSet_Procedures.VitalsEdit(currentVitalSignId);
        }
    },

    rowHistory: function ($row, ClassName) {
        var temp = $row.attr('id');
        var tempId = temp.split('-')[1];
        $row.attr('id', tempId);
        var currentProcedureId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProcedureId > 0) {
            OrderSet_Procedures.ShowHistory(currentProcedureId);
        }
    },
    ShowHistory: function (ProcedureId) {
        EMRUtility.showCurrentItemHistory(OrderSet_Procedures.params.PanelID, null, ProcedureId, "Procedures", OrderSet_Procedures.params.OrderSetID, "OrderSet_Procedures", null);
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

                OrderSet_Procedures.EnableFavProcedure(objData["ProcedureProcedure"]);
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
                OrderSet_Procedures.DeleteProcedure(selectedValue).done(function (response) {
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
                    OrderSet_Procedures.ProceduresSearch();
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
        IsActive = $('#' + OrderSet_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');
        utility.myConfirm('3', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {

                OrderSet_Procedures.InActiveProcedures(selectedValue, null, null).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        OrderSet_Procedures.ProceduresSearch();
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
        IsActiveRecord = $('#' + OrderSet_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');

        if (IsActiveRecord == null || IsActiveRecord == '1') {
            IsActiveRecord = false;
        } else {
            IsActiveRecord = true;
        }
        var objData = new Object();
        objData["ProcedureId"] = ProcedureId;
        objData["OrderSetId"] = OrderSet_Procedures.params.OrderSetId;
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

        $row.find("#hfComments").val(OrderSet_Procedures.PreviousComments);

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

        var VitalsGridId = "#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = OrderSet_Procedures.EditableGrid.datatable.row(parentRow);

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
        isactive = $('#' + OrderSet_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');
        var objData = {};
        var ProcedureDetail = [];
        var objData1 = JSON.parse(ProceduresData);
        objData1["ProcedureId"] = ProcedureId;
        objData1["Comments"] = $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures tbody tr[id='" + ProcedureId + "']").find("#hfComments").val();//$('#' + OrderSet_Procedures.params.PanelID + ' #hfGridComments').val();
        objData1["IsActive"] = isactive;
        ProcedureDetail.push(objData1);
        objData["procedureDetailModel"] = ProcedureDetail;
        objData["ProcedureId"] = ProcedureId;
        objData["commandType"] = "update_procedure";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Procedure");

    },
    buildHistoryRows: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems) {
        var row = OrderSet_Procedures.EditableGrid.datatable.row(CurrentRow);
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
        objData["OrderSetId"] = OrderSet_Procedures.params.OrderSetId;



        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Procedure");

    },

    // problem list grid load as per admin

    ProceduresGridLoadNew: function (response) {
        // get Actions
        var actions = "";
        $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, " #pnlProcedures_Result");
                }
            }
        });
        if ($.fn.dataTable.isDataTable("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures")) {
            $("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures").dataTable().fnClearTable();
            $("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures").dataTable().fnDestroy();
            $("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures tbody").find("tr").remove();
        }

        $("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures tbody").find("tr").remove();

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
                    var commentsMethod = "OrderSet_Procedures.AddComments('" + item.ProcedureId + "');";
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

                if (ChildHistory_Procedures.length > 0) {
                    $row.append('<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions" id="' + item.ProcedureId + '" >' + actions + '</td><td>' + item.CPTCode + ' ' + item.CPT_DESCRIPTION + '</td><td>' + item.Unit + '</td><td>' + item.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td>');
                    $row.attr("isHistory", '1');

                } else {
                    $row.append('<td></td><td class="actions" id="' + item.ProcedureId + '" >' + actions + '</td><td>' + item.CPTCode + ' ' + item.CPT_DESCRIPTION + '</td><td>' + item.Unit + '</td><td>' + item.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedByName + '</td><td align="center"> ' + comments + ' </td>');

                }

                var hfProceduresComments = $(' <input type="hidden" id="hfComments" name="Comments" value="' + item.Comments + '">');
                $row.append(hfProceduresComments);

                if (item.IsActive == "True") {
                    $($row).find('a.edit-row').removeClass('disableAll');
                } else {
                    $($row).find('a.edit-row').addClass('disableAll');
                }


                $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures tbody").last().append($row);

                var CurrentRowchilds = $();

                if (ChildHistory_Procedures.length > 0) {
                    $.each(ChildHistory_Procedures, function (i, item1) {
                        if (item1.Comments != "") {
                            var commentsMethod = "OrderSet_Procedures.AddComments('" + item1.ProcedureId + "');";
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
            var PanelGrid = "#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result";
            var GridId = "#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures";

            // Below line comment out inorder to remove duplicate grid search
            if (OrderSet_Procedures.myGrid != null) {
                OrderSet_Procedures.myGrid.$table.dataTable().fnDestroy();
                $("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures").dataTable().fnDestroy();
            }
            //Below line comment out inorder to remove duplicate grid search

            OrderSet_Procedures.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, OrderSet_Procedures, 0, false, true, false, true, false, null);

            //rander childs
            $.each(arraTemp, function (i, item) {

                if (OrderSet_Procedures.myGrid != null) {
                    var row = OrderSet_Procedures.myGrid.datatable.row(item.row);
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


            $('#' + OrderSet_Procedures.params.PanelID + ' #pnlProcedures_Result #dgvProcedures').DataTable({
                "language": {
                    "emptyTable": "No Procedure Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
            });
          if (response.ProcedureTotalCount == 0) {
                $("#pnlProcedures_Result #btnNoKnownProblems").css("display", "");
            } else {
                $("#pnlProcedures_Result #btnNoKnownProblems").hide();
            }

            /* End of Code for making No Known Problem List hyperlink inline with checkbox and search box.
            *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
            */
        }


        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        //Sorting icon removed from first column of grid        
        $('#' + OrderSet_Procedures.params.PanelID + ' #dgvProcedures thead tr th:first-child').removeClass('sorting_asc');

        EMRUtility.fixDataTableDuplication("#" + OrderSet_Procedures.params.PanelID + " #pnlProcedures_Result");

    },

    //
    UnLoadTab: function () {

        if (OrderSet_Procedures.params["FromAdmin"] == "0") {
            if (!$.isEmptyObject(OrderSet_Procedures.ProceduresDetail)) {
                utility.myConfirm('12', function () {
                    $.when(OrderSet_Procedures.ProceduresSave()).then(function () {
                        UnloadActionPan(OrderSet_Procedures.params.ParentCtrl, 'OrderSet_Procedures');

                    });
                },
            function () {
                UnloadActionPan(OrderSet_Procedures.params.ParentCtrl, 'OrderSet_Procedures');
                OrderSet_Procedures.ProceduresDetail = [];
            });

            }
            else
                UnloadActionPan(OrderSet_Procedures.params.ParentCtrl, 'OrderSet_Procedures');
        }
        else {
            RemoveAdminTab();
        }
        OrderSet_ProceduresGrid.ProceduresSearch();
    },

    //Comments Update

    AddComments: function (ProcedureId) {
        var params = [];
        params["ParentCtrl"] = 'OrderSet_Procedures';
        params["ProcedureID"] = ProcedureId;
        params["Comments"] = $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures tbody tr[id='" + ProcedureId + "']").find("#hfComments").val();
        params["FromAdmin"] = "0";
        params["OrderSetId"] = OrderSet_Procedures.params.OrderSetId;
        LoadActionPan('Clinical_ProceduresComments', params, OrderSet_Procedures.params.PanelID);
    },

    ActiveProblemSearch: function (objThis) {


        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        OrderSet_Procedures.ProceduresSearch();
    },

    ResetDiagnosis: function () {
        if ($("#" + OrderSet_Procedures.params.PanelID + " #txtProblems").val() == "") {
            $('#' + OrderSet_Procedures.params.PanelID + ' #frmOrderSetProcedures').resetAllControls(null);
            $("#" + OrderSet_Procedures.params.PanelID + " #txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate,#txtComments").prop("disabled", true);
        }
    },


    //Purpose Save/update favList Status
    SaveFavToggelStatus: function () {

        var isFavListOpened = $('#' + OrderSet_Procedures.params.PanelID + " #frmOrderSetProcedures #favSectionDiv").hasClass("toggled");
        EMRUtility.insertUpdateFavListStatus(OrderSet_Procedures.FavListName, isFavListOpened);
    },



    //START Charge Editable Grid Code

    AddNewProcedureRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription, SnomedID, SnomedDescription) {

        var ProcedureGridId = "#" + OrderSet_Procedures.params.PanelID + " #dgvProcedureAdd";
        var TemplateRowId = 0;
        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (OrderSet_Procedures.params.ParentCtrl != null) {
                CurrentRow = OrderSet_Procedures.EditableGridAdd.rowAdd(RowId, "");
            }
            else {
                CurrentRow = OrderSet_Procedures.EditableGridAdd.rowAdd(RowId, OrderSet_Procedures.params.ProcedureId);
            }

        }
        else {
            var TemplateRow = $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedureAdd tbody tr[id*=-]").last();
            TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            CurrentRow = OrderSet_Procedures.EditableGridAdd.rowAdd(TemplateRowId - 1, "");
        }
        var item = {}
        item["ProcedureId"] = TemplateRowId - 1;
        item["CPTCode"] = cptCode;
        item["CPT_DESCRIPTION"] = procDesc;
        item["SNOMEDID"] = SnomedID;
        item["SNOMED_DESCRIPTION"] = SnomedDescription;
        OrderSet_Procedures.Procedures.push(item);
        var dtDOSFromId = String($(CurrentRow).find("input[id*=dtpStartDate]").attr('id'));
        var dtDOSToId = String($(CurrentRow).find("input[id*=dtpEndDate]").attr("id"));
        var txtUnit = String($(CurrentRow).find("input[id*=txtUnit]").attr("id"));
        $('#' + OrderSet_Procedures.params.PanelID + ' #dgvProcedureAdd').on('keydown', '#' + txtUnit, function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });
        $(CurrentRow).find('input[id*="txtUnit"]').bind("paste", function (e) {
            e.preventDefault();
        });
        utility.CreateDatePicker('frmOrderSetProcedures #' + dtDOSFromId, function () {
            //on-change callback method
        }, true);
        utility.CreateDatePicker('frmOrderSetProcedures #' + dtDOSToId, function () {
            //on-change callback method
        }, true);

        OrderSet_Procedures.ValidateFromToDate('frmOrderSetProcedures', dtDOSFromId, dtDOSToId, true);



        var row = OrderSet_Procedures.EditableGridAdd.datatable.row(CurrentRow);


        row.child(OrderSet_Procedures.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));
        row.child.hide();


        if (cptDescription.indexOf(" (SCT: " + SnomedID + ")") >= 0) {
            $(CurrentRow).find('input[id*="ProcedureProcedure"]').val(cptCode + " " + procDesc);
        }

        else {
            $(CurrentRow).find('input[id*="ProcedureProcedure"]').val(cptCode + " " + procDesc + (SnomedID != "" ? " (SCT: " + SnomedID + ")" : ""));
        }


        OrderSet_Procedures.enableRemoveRow($(CurrentRow));
        var $DId = $(CurrentRow).find('select[id*="ProblemListId"]').attr('id');
        CacheManager.BindDropDownsByID("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedureAdd #" + $DId, 'LookupProblemListsForOrderSet', true, OrderSet_Procedures.params.OrderSetId);
        return CurrentRow;
    },
    ValidateFromToDate: function (FormId, CtrlFromDateId, CtrlToDateId, IsOptional, onFromDateChangeCallback, onToDateChangeCallback) {

        var CtrlForm = "#" + FormId;
        var CtrlFromDate = CtrlForm + " #" + CtrlFromDateId;
        var CtrlToDate = CtrlForm + " #" + CtrlToDateId;
        var CtrFromDateName = $(CtrlToDate).attr("name");
        var CtrToDateName = $(CtrlToDate).attr("name");
        var startDate = new Date('01/01/1700');
        var endDate = new Date('01/01/' + Number(new Date().getFullYear() + 35));

        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];

        $(CtrlToDate).attr('maxlength', '10');
        $(CtrlFromDate).attr('maxlength', '10');
        $(CtrlFromDate).datepicker({
            todayHighlight: true,
            format: date_format,
            todayBtn: 'linked',
        }).change(function (e) {
            if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                var fromDate = new Date($(CtrlFromDate).val());
                var toDate = new Date($(CtrlToDate).val());

                if (fromDate <= toDate && fromDate != '') {
                    $(CtrlToDate).val($(CtrlToDate).val()).datepicker('update');
                } else {
                    $(this).val('');
                    utility.DisplayMessages("From date is greater than To date", 3);
                }
            }

            $(CtrlToDate).attr('disabled', false);


            var inputDate = $(CtrlFromDate).datepicker('getDate');

            var date_format = 'dd/mm/yyyy';
            //set default Date Formate
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }

            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(CtrlToDate).attr("name");
            }
            if (typeof CtrFromDateName == 'undefined') {
                CtrFromDateName = $(CtrlFromDate).attr("name");
            }
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);

            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrFromDateName);
            if (onFromDateChangeCallback != null && typeof (onFromDateChangeCallback) == 'function') {
                setTimeout(onFromDateChangeCallback, 50);
            }

        }).on('keypress', function (e) {
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        $(CtrlToDate).datepicker({
            todayHighlight: true,
            // startDate: inputDate,
            format: date_format,
            //todayBtn: 'linked',
        }).change(function (e) {
            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(this).attr("name");
            }
            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);

            if (onToDateChangeCallback != null && typeof (onToDateChangeCallback) == 'function') {
                setTimeout(onToDateChangeCallback, 50);
            }
            var CurrentDatepicker = this;
            setTimeout(function () {
                if ($(CurrentDatepicker).val().length == date_format.length) {
                    if (!utility.isValidDate($(CurrentDatepicker).val())) {
                        $(CurrentDatepicker).val('');
                        utility.DisplayMessages("Please enter valid date", 3);
                        $(CtrlForm).bootstrapValidator('revalidateField', CurrentDatepicker.name);
                    }
                    if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                        var fromDate = new Date($(CtrlFromDate).val());
                        var toDate = new Date($(CtrlToDate).val());
                        if (fromDate > toDate) {
                            $(CurrentDatepicker).val('');
                            utility.DisplayMessages("To date is smaller than from date", 3)
                        }
                    }
                }
            }, 50);
        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        var DateNewFormat = date_format.replace('dd', '99');
        DateNewFormat = DateNewFormat.replace('mm', '99');
        DateNewFormat = DateNewFormat.replace('yyyy', '9999');

        $(CtrlFromDate).on('blur', function (e) {
            setTimeout(
               function () {

                   if ($(CtrlFromDate).val() != "")
                       utility.ValidateDate(CtrlFromDate);
               }, 100);
        });
        $(CtrlToDate).on('blur', function (e) {
            setTimeout(function () {

                if ($(CtrlToDate).val() != "")
                    utility.ValidateDate(CtrlToDate)
            }, 100);
        });
    },
 
  

    //------------------ end M Ahmad Imran Code for editable grid




    setCommentsField: function (procedureId, comments) {
        $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures tbody tr[id='" + procedureId + "']").find("#hfComments").val(comments);
    },

    logViewProcedures: function (procedureId) {


        var objData = new Object();
        objData["OrderSetId"] = OrderSet_Procedures.params.OrderSetId;
        objData["ProcedureId"] = procedureId;
        objData["commandType"] = "search_procedures";
        objData["NotesId"] = OrderSet_Procedures.params.NotesId == null ? 0 : OrderSet_Procedures.params.NotesId;
        objData["IsActive"] = true;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Procedure");
    },
}