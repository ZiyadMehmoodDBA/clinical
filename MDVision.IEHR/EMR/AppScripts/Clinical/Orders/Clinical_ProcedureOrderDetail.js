ClinicalProcedureOrderDetail = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    ProcedureOrderProblems: [],
    FavListName: "ProcedureOrderDetail",
    checkedProblems: [],

    Load: function (params) {
        BackgroundLoaderShow(true);
        params["TabID"] = 'ClinicalProcedureOrderDetail';
        //params["PanelID"] = 'pnlClinicalProcedureOrderDetail';
        ClinicalProcedureOrderDetail.params = params;
        var PanelProcedureGrid = "#" + ClinicalProcedureOrderDetail.params.PanelID + " #pnlProcedure_Result";
        var ProcedureGridId = "#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure";
        //$(ChargeGridId).dataTable().fnDestroy();
        $(ProcedureGridId + " tbody tr").remove();
        //Start 05-04-2016 Humaira Yousaf to disable action buttons
        $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetProcedureOrder").addClass('disableAll');
        //End 05-04-2016 Humaira Yousaf to disable action buttons
        ClinicalProcedureOrderDetail.EditableGrid = EMRUtility.MakeEditableGrid(PanelProcedureGrid, ProcedureGridId, ClinicalProcedureOrderDetail, "0", false, false, false, false);
        $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfPatientId").val($("div#PatientProfile #hfPatientId").val());

        if (Clinical_ProcedureOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }

        var self = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail");

        // Set Title Explicitly if it's passed as Parameter
        if (ClinicalProcedureOrderDetail.params.Title != null)
            $("#" + ClinicalProcedureOrderDetail.params["PanelID"] + " #headingTitle").text(ClinicalProcedureOrderDetail.params.Title);
        //Start 05-04-2016 Humaira Yousaf to customize button text
        if (ClinicalProcedureOrderDetail.params.NoteId != null) {
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder").text('Add to Note');
        }
        else {
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder").text('Save');
        }
        //End 05-04-2016 Humaira Yousaf to customize button text
        //Start 02-03-2016 Humaira Yousaf to load dropdowns
        if (ClinicalProcedureOrderDetail.bIsFirstLoad == true) {
            ClinicalProcedureOrderDetail.bIsFirstLoad = false;
            //Strat 17-03-2016 Farooq AHmad load problem list
            ClinicalProcedureOrderDetail.loadProblemList();
            //End 17-03-2016 Farooq Ahmad load problem list
            //Strat 17-03-2016 Farooq AHmad validate Procedure OrderDetail
            ClinicalProcedureOrderDetail.validateProcedureOrderDetail();
            //End 17-03-2016 Farooq Ahmad validate Procedure OrderDetail

            ClinicalProcedureOrderDetail.loadAllAutocomplete();
            ClinicalProcedureOrderDetail.bindProvider();
            self.loadDropDowns(true).done(function () {
                //Start 11-04-2016 Humaira Yousaf
                // self.find('#ddlFavoriteListProcedureOrder').trigger('onchange');
                //End 11-04-2016 Humaira Yousaf
                //Start 04-03-2016 Humaira Yousaf to load ProcedureOrder
                ClinicalProcedureOrderDetail.loadProcedureOrder();
                //Start 06-04-2016 Humaira Yousaf to select provider
                if (ClinicalProcedureOrderDetail.params.mode == "Add") {
                    ClinicalProcedureOrderDetail.selectProvider();
                }
                //End 06-04-2016 Humaira Yousaf to select provider
                //End 04-03-2016 Humaira Yousaf to load ProcedureOrder

                //Start//04-03-2016//Ahmad Raza//Logic to show/hide divs  on multilist selection change
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ddlProcedureOrderRuleType").multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onChange: function (element, checked) {
                        var selectedValue = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ddlProcedureOrderRuleType option:selected");
                        var selected = [];
                        $(selectedValue).each(function (index, val) {
                            selected.push($(this).text());
                        });
                        var unSelect = '';
                        $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail .comList").addClass('hidden');
                        $(selected).each(function (i, item) {
                            var sectionName = item;
                            unSelect += sectionName + ',';
                            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #dv" + item.replace(/\s/g, '')).removeClass('hidden');
                        });
                    }
                });
                //End//04-03-2016//Ahmad Raza//Logic to show/hide divs  on multilist selection change
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").data('serialize', $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").serialize());
            });
        }

        //Start Farooq Ahmad 21/03/2016 This will create the date time picker

        utility.CreateDatePicker(ClinicalProcedureOrderDetail.params.PanelID + ' #OrderDate', function () {

        }, true);

        $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #OrderTime').timepicker({
            defaultTime: new Date()
        });

        //End Farooq Ahmad 21/03/2016 This will create the date time picker

        if (ClinicalProcedureOrderDetail.params.mode == "Add") {
            var Ctrl = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProvider");
            var hfCtrl = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfProvider");
            if (Clinical_ProcedureOrder.params["ProviderId"] != "" && Clinical_ProcedureOrder.params["ProviderName"] != "") {
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProvider").val(Clinical_ProcedureOrder.params["ProviderName"]);
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfProvider").val(Clinical_ProcedureOrder.params["ProviderId"]);
            }
            else {
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProvider").val(Patient_Demographic.params.PatientProvider);
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfProvider").val(Patient_Demographic.params.PatientProviderId);
            }
            utility.SetKendoAutoCompleteSourceforValidate(Ctrl, Ctrl.val(), hfCtrl, hfCtrl.val());
            ClinicalProcedureOrderDetail.favoriteListSearch();
        }
        ClinicalProcedureOrderDetail.domReadyFunction();
        $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").data('serialize', $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").serialize());


    },

    //Author: Muhammad Arshad
    //Date :  23-03-2016
    //Reason: Function to disable procedure Order Controls
    disableProcedureOrderControls: function (IsSigned) {
        //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
        var detailsDivs = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #divProcedureOrderInformation,#divProcedureTestInformation,#divProcedureAssociatedProblems");
        //var detailsButtons = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetProcedureOrder");
        var printRequisitionButton = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnPrintOrder");
        var detailsButtons = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder,#btnSignOrder,#btnResetProcedureOrder");
        var signPrintButton = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSignPrintOrder");
        if (IsSigned == true) {
            detailsDivs.addClass("disableAll");
            detailsButtons.addClass("disableAll");
            signPrintButton.addClass("hidden");
            printRequisitionButton.removeClass("hidden");
        }
        else {
            detailsDivs.removeClass("disableAll");
            if (ClinicalProcedureOrderDetail.params.mode == "Edit") {
                detailsButtons.removeClass("disableAll");
                signPrintButton.removeClass("disableAll");

            }
            signPrintButton.removeClass("hidden");
            printRequisitionButton.addClass("hidden");
        }
        //End 05-04-2016 Humaira Yousaf to customize button text
        //if (ClinicalProcedureOrderDetail.params.mode != "Add") {
        //    var detailsDivs = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #divProcedureOrderInformation,#divProcedureTestInformation,#divProcedureAssociatedProblems");
        //    //var detailsButtons = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetProcedureOrder");
        //    var detailsButtons = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder,#btnSignOrder,#btnResetProcedureOrder");
        //    var signPrintButton = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSignPrintOrder");
        //    var printRequisitionButton = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnPrintOrder");
        //    if (IsSigned == true) {
        //        detailsDivs.addClass("disableAll");
        //        detailsButtons.addClass("disableAll");
        //        signPrintButton.addClass("hidden");
        //        printRequisitionButton.removeClass("hidden");
        //    }
        //    else {
        //        detailsDivs.removeClass("disableAll");
        //        detailsButtons.removeClass("disableAll");
        //        signPrintButton.removeClass("hidden");
        //        printRequisitionButton.addClass("hidden");
        //    }
        //}

    },

    //Author: Farooq Ahmad
    //Date  : 25/03/2016
    //Reason: Function will reset the procedure order detail form
    resetProcedureOrderDetail: function () {
        utility.myConfirm('Are you sure to reset all controls?', function () {
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
                ClinicalProcedureOrderDetail.resetControlValue($(this));
            });

            $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #OrderDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #OrderTime').timepicker('setTime', new Date());

            var PanelProcedureGrid = "#" + ClinicalProcedureOrderDetail.params.PanelID + " #pnlProcedure_Result";
            var ProcedureGridId = "#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure";
            $(ProcedureGridId + " tbody tr").remove();
            if ($.fn.dataTable.isDataTable(ProcedureGridId)) {
                $(ProcedureGridId).dataTable().fnClearTable();
                $(ProcedureGridId).dataTable().fnDestroy();
            }
            ClinicalProcedureOrderDetail.EditableGrid.datatable.clear().draw();
        }, function () { }, 'Confirm Reset');
    },


    //Author: Farooq Ahmad
    //Date  : 25/03/2016
    //This function will clear value of given control as specified by obj
    resetControlValue: function (obj) {
        var currentElementTagName = obj.tagName != null ? obj.tagName : obj.prop("tagName");
        if ($(obj).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea' || $(obj).attr('type') == 'hidden')
            $(obj).val('');
        if ($(obj).attr('type') == 'checkbox' || $(obj).attr('type') == 'radio') {

            if ($(obj).attr('type') == 'radio') {
                obj.checked = false;

                //var groupRadBtn = $("input[name='" + $(obj).attr('name') + "']");
                //if (groupRadBtn.length > 1) {
                //    $.each(groupRadBtn, function (i, item) {
                //        if ($(item).attr("id").toLowerCase().indexOf("no") > -1) {
                //            $(item).trigger("click");
                //        }
                //    });
                //}

            }
                //Start//28-03-2016//Ahmad Raza//fixed bug# EMR-573
            else if ($(obj).attr('type') == 'checkbox') {
                $(obj).attr('checked', false);
            }
            //End//28-03-2016//Ahmad Raza//fixed bug# EMR-573
        }

        if (currentElementTagName.toLowerCase() == 'select') {
            $(obj).find('option:selected').removeAttr('selected');
            //$(this).attr('selectedIndex', '-1');
            $(obj).find('option:eq(0)').attr('selected', 'selected');
        }
        if (currentElementTagName.toLowerCase() == 'ul') {
            $(obj).find('li.active').removeClass('active');
        }
    },

    //Author: Muhammad Arshad
    //Date :  23-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteListSearch: function () {

        ClinicalProcedureOrderDetail.searchFavoriteList_DBCall("ProcedureOrder", null, 1, 5000).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $ddl = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #ddlFavoriteListProcedureOrder');
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
                    EMRUtility.getFavListValue(ClinicalProcedureOrderDetail.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "" && response1.favListVal != "-1") {
                                if ($("#" + ClinicalProcedureOrderDetail.params.PanelID + " #ddlFavoriteListProcedureOrder option[value='" + response1.favListVal + "']").length > 0) {
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
                            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").data('serialize', $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").serialize());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    var $uL = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #ulFavoriteListProcedureOrderContent');
                    var divSelectAllFavorites = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #divSelectAllProcedureOrderFavorites');
                    $uL.empty();
                    if (divSelectAllFavorites.hasClass("disableAll") == false) {
                        divSelectAllFavorites.addClass("disableAll");
                    }
                }
                var mystr = "";
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").data('serialize', $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").serialize());
            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });

    },

    searchFavoriteList_DBCall: function (ListType, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["ListType"] = ListType == null ? 'ProcedureOrder' : ListType;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        //Start Humaira Yousaf 24-03-2016 for active/inactive
        if (Favorite_ProcedureOrder.Switch == 1) {
            objData["IsActive"] = true
        }
        else {
            objData["IsActive"] = false;
        }
        //End Humaira Yousaf 24-03-2016 for active/inactive

        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["ProviderId"] = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #hfProvider').val();

        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    AutoSearchFavProcedureOrder: function () {
        utility.Keyupdelay(function () {
            ClinicalProcedureOrderDetail.loadfavoriteListContent();
        });
    },
    //Author: Muhammad Arshad
    //Date :  23-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    loadfavoriteListContent: function (obj, favListIds) {
        if ((typeof favListIds == typeof undefined || favListIds == null) && (typeof obj == typeof undefined || obj == null)) {
            obj = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #ddlFavoriteListProcedureOrder');
        }
        var SearchData = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #FavSearchBox').val();
        var $uL = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #ulFavoriteListProcedureOrderContent');
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            var selectedOptionValue = selectedOption.attr("id");
            //Start 12-07-2016 Muhammad Arshad Bug#EMR-1486 Lab order -> Favorite list -> "Select ALL " functionality is not working on 46 server ,please see attached video and resolve the issue
            var divSelectAllFavorites = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #divSelectAllProcedureOrderFavorites');
            if (selectedOptionValue > 0) {
                divSelectAllFavorites.removeClass("disableAll");

                ////Start 11-04-2016 Humaira Yousaf
                //if (selectedOption.attr("id") != "-1") {
                ClinicalProcedureOrderDetail.favoriteList_CPTSearch(selectedOptionValue, SearchData);
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
            //    $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #ulFavoriteListProcedureOrderContent').empty();
            //    $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
            //}
            //End 11-04-2016 Humaira Yousaf
        }
        else {
            if (favListIds != null) {
                //clear the list

                $uL.empty();

                $.each(favListIds, function (index, item) {
                    //load all favList
                    ClinicalProcedureOrderDetail.favoriteList_CPTSearch(item, false);
                });
            }
        }
    },

    //Author: Muhammad Arshad
    //Date :  23-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteList_CPTSearch: function (FavoriteListId, SearchData) {
        var $UL = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #ulFavoriteListProcedureOrderContent');
        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(null, FavoriteListId, 1, 5000, SearchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var objData = JSON.parse(response.FavoriteListCPTJSON);
                $UL.empty();
                //Start 11-04-2016 Humaira Yousaf
                if (objData.length > 0) {
                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #favSelectAllLink').removeClass('disableAll');
                }
                else {
                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
                }
                $.each(objData, function (i, item) {
                    if (item.CPTCodeDescription != "") {
                        var onclick = 'ClinicalProcedureOrderDetail.BindProcedureGridItem(\'' + item.CPTCode + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + String(item.CPTCodeDescription) + '\', this)';

                        var LiId = item.CPTCode + " " + item.CPTCodeDescription; //$('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #ddlFavoriteListProcedureOrder').find("option:selected").attr('id') + '-' + item.CPTCode;

                        var isFound = ClinicalProcedureOrderDetail.isFavoriteProcedureFound(LiId, item.CPTCodeDescription);
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
            else {
                $UL.empty();
            }
        });
    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: Function to load problem list
    loadProblemList: function () {
        var dfd = $.Deferred();
        ClinicalProcedureOrderDetail.SearchProblemList().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ProblemListCount > 0) {
                    var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
                    var ProblemListHistoryLoadJSONData = JSON.parse(response.ProblemListHistoryLoad_JSON);

                    var finalTr = '';
                    var counter = 2;
                    $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #ulProblemLists tbody tr").remove();
                    $.each(ProblemListLoadJSONData, function (i, item) {
                        if (item.Description.split("-")[2] != undefined) {
                            item.Description = item.Description.substring(item.Description.indexOf("-") + 2);
                        }
                        if (counter % 2 == 0) {
                            finalTr = finalTr + '<tr>';
                        }
                        finalTr = finalTr + '<td><div class="p-xs col-xs-12"><div class="checkbox-custom">';
                        //Start//Abid Ali//For bug# EMR-1574
                        var checked = ClinicalProcedureOrderDetail.isCheckedProblem('chk' + item.ProblemListId + '') ? "checked" : "";
                        finalTr = finalTr + '<input ' + checked + ' type="checkbox" onclick ="ClinicalProcedureOrderDetail.pushProblems(this);" name="' + item.Code + '" value="' + item.ProblemListId + '"  id="chk' + item.ProblemListId + '">';
                        //End//Abid Ali//For bug# EMR-1574

                        finalTr = finalTr + '   <label  for="chk' + item.ProblemListId + '" class="control-label">' + item.Description + '</label></div></div></td>';

                        if (counter % 2 == 1) {
                            finalTr = finalTr + '</tr>';
                        }
                        counter++;
                        var color = "";
                    });
                    $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #ulProblemLists tbody").append(finalTr);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").data('serialize', $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },

    //Author: Abid Ali
    //Date :  15-07-2016
    //Reason: function to push checked problems in array
    pushProblems: function (obj) {

        var $obj = $(obj);
        var isChecked = $obj.prop('checked');
        var id = $obj.attr('id');
        if (isChecked) {
            //Push in checked problems
            if (!($.inArray(id, ClinicalProcedureOrderDetail.checkedProblems) != -1)) {
                ClinicalProcedureOrderDetail.checkedProblems.push(id);
            }
        }
        else {
            //Remove from checked problems
            ClinicalProcedureOrderDetail.checkedProblems.splice($.inArray(id, ClinicalProcedureOrderDetail.checkedProblems), 1);
        }

    },
    //Author: Abid Ali
    //Date :  15-07-2016
    //Reason: function to check problem in array
    isCheckedProblem: function (problemId) {

        return $.inArray(problemId, ClinicalProcedureOrderDetail.checkedProblems) == -1 ? false : true;

    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: Function to load problem list
    SearchProblemList: function () {


        var IsCheckedIn = null;
        IsCheckedIn = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }

        var PageNumber = 1;
        var RowsPerPage = 2000;

        var objData = new Object();
        objData["PatientId"] = Clinical_ProcedureOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = '1';
        // objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        //objData["NoteId"] = Clinical_ProblemLists.params.NotesId == null ? 0 : Clinical_ProblemLists.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },
    //CallCityState: function (control, field1, field2) {
    //    var zipcode = $('#ClinicalProcedureOrderDetail ' + control).val();
    //    var cityname = null;
    //    var statename = null;
    //    ClinicalProcedureOrderDetail.FillCityState(zipcode, cityname, statename).done(function (response) {
    //        if (response.status != false) {
    //            var citystate = JSON.parse(response.CITYSTATE_JSON);
    //            $('#ClinicalProcedureOrderDetail ' + field1).val(citystate.txtCity);
    //            $('#ClinicalProcedureOrderDetail ' + field2).val(citystate.txtState);
    //            //var self = $("#ClinicalProcedureOrderDetail");
    //            ////self.bindMyJSON(true, citystate, true);
    //            //utility.bindMyJSON(true, citystate, true, self);
    //            //ClinicalProcedureOrderDetail.ValidateProvider();
    //        }
    //        else {
    //            $('#ClinicalProcedureOrderDetail ' + field1).val('');
    //            $('#ClinicalProcedureOrderDetail ' + field2).val('');
    //        }
    //    });
    //},


    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: Function to apply bootstrap validations
    validateProcedureOrderDetail: function () {
        $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ProcedureOrderTitle: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Provider: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }

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
                        ClinicalProcedureOrderDetail.ProcedureOrderSave('save');
                        break;
                    case 'btnSignOrder':
                        ClinicalProcedureOrderDetail.ProcedureOrderSave('signorder');
                        ClinicalProcedureOrderDetail.disableProcedureOrderControls(true);
                        break;
                    case 'btnSignPrintOrder':
                        ClinicalProcedureOrderDetail.ProcedureOrderSave('signprintorder');
                        ClinicalProcedureOrderDetail.disableProcedureOrderControls(true);
                        break;
                    case 'btnPrintOrder':
                        ClinicalProcedureOrderDetail.printProcedureOrder();
                        break;
                    default:
                        ClinicalProcedureOrderDetail.ProcedureOrderSave('save');
                        break;
                }
                //End 22-03-2016 Humaira Yousaf for multiple submit buttons
            }
            e.type = "";
        });

    },


    //Author: Farooq Ahmad
    //Date: 17-03-2016
    //Reason:to add more problem is Associated Problem list
    addProblem: function () {
        ClinicalProcedureOrderDetail.SaveLocalyCheckedProbems();
        var params = [];
        //Ast 357
        params["CurrentNotesProviderId"] = ClinicalProcedureOrderDetail.params["CurrentNotesProviderId"];
        params["IsFromNote"]=ClinicalProcedureOrderDetail.params["IsFromNote"];
        params["RefForm"] = "frmClinicalProcedureOrderDetail";
        params["FromOrderDetail"] = "1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalProcedureOrderDetail";
        LoadActionPan('Clinical_ProblemLists', params);
    },
    SaveLocalyCheckedProbems: function () {
        var self = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail");
        ClinicalProcedureOrderDetail.ProcedureOrderProblems = [];
        $(self).find("#ulProblemLists td").each(function (index, item) {
            if ($(this).find("input:checkbox").prop("checked")) {
                var objProblem = {
                    ProblemId: $(this).find("input:checkbox").val(),
                    Description: $(this).text()
                }
                ClinicalProcedureOrderDetail.ProcedureOrderProblems.push(objProblem);
            }
        });
    },

    CheckedPreviousProbems: function () {
        var dfd = $.Deferred();
        for (var index in ClinicalProcedureOrderDetail.ProcedureOrderProblems) {
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #ulProblemLists td #chk" + ClinicalProcedureOrderDetail.ProcedureOrderProblems[index].ProblemId).attr("checked", "checked");
        }
        dfd.resolve();
        return dfd.promise();
    },
    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: open facility form
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalProcedureOrderDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalProcedureOrderDetail";
        LoadActionPan('Admin_Facility', params);
    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: open facility detail form
    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#pnlClinicalProcedureOrderDetail #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'ClinicalProcedureOrderDetail';
        LoadActionPan('facilityDetail', params);
    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: open provider form
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalProcedureOrderDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalProcedureOrderDetail";
        LoadActionPan('Admin_Provider', params);
    },
    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: open provider form
    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#pnlClinicalNotes #hfProvider').val(),'clinicalTabNotes');
        var params = [];
        params["ProviderId"] = $('#pnlClinicalProcedureOrderDetail #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'ClinicalProcedureOrderDetail';
        LoadActionPan('providerDetail', params);
    },


    //Function Name: OpenAssignee
    //Author Name: Farooq Ahmad
    //Created Date: 21-03-2016
    //Description: auto complete for assignee
    OpenAssignee: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmClinicalProcedureOrderDetail";
        params["AssigneeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalProcedureOrderDetail";
        params["RefCtrl"] = "txtAssignee";
        params["RefCtrlHidden"] = "hfAssignee";
        params["RefCtrlLink"] = "lnkAssignee";
        LoadActionPan('Admin_Provider', params);
    },


    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: Binding numpad with height, weight, systolic and diastolic fields
    domReadyFunction: function () {

        $(document).ready(function () {

            $('.toggleHorSmallLeft section').click(function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));

            });
        });

        $(function () {
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail [data-plugin-keyboard-numpad]').keyboard({
                customLayout: {
                    'default': [
                        '7 8 9 {b}',
                        '4 5 6 {clear}',
                        '1 2 3 {t}',
                        '0   .  {a} {c} '
                    ]
                },
                change: function (e, keyboard, el) {
                    if (keyboard.$preview.attr('maxlength') != null && !keyboard.$preview.keyboard().getkeyboard().options.maxLength) {
                        keyboard.$preview.keyboard().getkeyboard().options.maxLength = keyboard.$preview.attr('maxlength');
                    }
                    if (keyboard.$preview.attr('oninput') != null) {
                        keyboard.$preview.trigger('oninput');
                    }
                    // Fix # EMR-96
                    if (keyboard.$preview.attr('name') == 'Height') {
                        if (keyboard.$preview.attr('onkeyup') != null) {
                            keyboard.$preview.trigger('onkeyup');
                            EMRUtility.ValidateHeight(e, keyboard.$preview);
                        }
                    } else if (keyboard.$preview.attr('onkeyup') != null) {
                        keyboard.$preview.trigger('onkeyup');
                    }

                },
                layout: 'custom',
                reposition: true,
                appendLocally: this,
                restrictInput: true,
                preventPaste: true,
                usePreview: false,
                autoAccept: true,
                tabNavigation: true
            })
                    .addTyping();
        });

    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To validate blood pressure
    ValidateBP: function (objSystolic, objDiastolic) {
        var systolicVal = 0;
        var diastolicVal = 0;
        if (objSystolic != null) {
            systolicVal = $(objSystolic).val();
        }
        else if (objDiastolic != null) {
            objSystolic = $($(objDiastolic).parent().parent().prevAll()[0]).find("input[id*='txtSystolic']");
            systolicVal = $(objSystolic).val();

        }
        if (objDiastolic != null) {
            diastolicVal = $(objDiastolic).val();
        }
        else if (objSystolic != null) {
            objDiastolic = $($(objSystolic).parent().parent().nextAll()[0]).find("input[id*='txtDiastolic']");
            diastolicVal = $(objDiastolic).val();
        }
        if ((diastolicVal != "" && systolicVal != "") && (parseInt(diastolicVal) >= parseInt(systolicVal))) {
            $(objDiastolic).css("border", "1px solid red");
            utility.DisplayMessages("Diastolic should be less than Systolic", 3);
        }
        else {
            if (systolicVal != "") {
                $(objSystolic).css("border", "1px solid #ccc");
                if (diastolicVal == "") {
                    $(objDiastolic).css("border", "1px solid red");
                }
                else {
                    $(objDiastolic).css("border", "1px solid #ccc");
                }

            }
            else if (diastolicVal != "") {
                $(objDiastolic).css("border", "1px solid #ccc");
                if (systolicVal == "") {
                    $(objSystolic).css("border", "1px solid red");
                }
                else {
                    $(objSystolic).css("border", "1px solid #ccc");
                }
            }
            else {
                $(objDiastolic).css("border", "1px solid #ccc");
                $(objSystolic).css("border", "1px solid #ccc");
            }
        }

    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To validate BSA
    calculateBSA: function (objWeight, objHeightInFeet, TargetCtrl) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #txtBSA");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var weightInKG = ClinicalProcedureOrderDetail.convertWeight(weightInLbs);
        var heightIn_cm = ClinicalProcedureOrderDetail.convertHeightTo_cm(heightInFeet);
        var result = 0.007184 * Math.pow(heightIn_cm, 0.725) * Math.pow(weightInKG, 0.425);
        var BSA = result.toFixed(2)
        if (WeightInLbs != "" && HeightInFeet != "")
            CtrlName.val(BSA);
        else
            CtrlName.val('');
    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To validate Weight
    convertWeight: function (pounds) {
        return pounds / 2.20462262185;
    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To validate height
    convertHeightTo_cm: function (feet) {
        return feet * 12 * 2.54;
    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To calculate BMI on the basis of height and weight
    calculateBMI: function (objWeight, objHeightInFeet, TargetCtrl) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #txtBMI");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var heightInInches = ClinicalProcedureOrderDetail.convertHeightInches(heightInFeet);

        var result = (weightInLbs / (heightInInches * heightInInches)) * 703;
        var BMI = result.toFixed(2);
        if (WeightInLbs != "" && HeightInFeet != "" && BMI != "Infinity")
            CtrlName.val(BMI);
        else
            CtrlName.val('');
    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To convert height to inches
    convertHeightInches: function (feet) {
        var newFeet = feet.toString();
        var a = newFeet.split(".");
        var fee = parseInt(a[0]);
        var inch = parseInt(a[1]);
        if (isNaN(inch))
            return (fee * 12);
        else
            return (fee * 12) + inch;
    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: Loading ICD Codes for Problem List AutoComplete
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "ClinicalProcedureOrderDetail", null, false);
    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: Loading ICD Codes for Popup
    OpenSearchPopup: function (SearchType, Ctrl, HiddenCtrl) {
        var controlToLoad = "";
        if (SearchType == "ICD") {

            controlToLoad = "Admin_IMOICD";
        }
        else if (SearchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (SearchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }
        var params = [];
        params["FromAdmin"] = "0";
        if (ClinicalProcedureOrderDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'ClinicalProcedureOrderDetail';
        }

        else {
            params["ParentCtrl"] = ClinicalProcedureOrderDetail.params["TabID"];

        }
        params["PanelID"] = ClinicalProcedureOrderDetail.params["PanelID"];

        params["ActionPanContainer"] = ClinicalProcedureOrderDetail.params["ActionPanContainer"];
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (ClinicalProcedureOrderDetail.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params, ClinicalProcedureOrderDetail.params.PanelID);
        }

    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: deleting Lis of Problem list
    deleteProblemList: function (obj, ev) {
        ev.stopPropagation();
        var problemListId = $(obj).attr('id');
        if (problemListId < 0) {
            $(obj).remove();
        }

    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: deleting Lis of Allergies list
    deleteAllergy: function (obj, ev) {
        ev.stopPropagation();
        var allergyId = $(obj).attr('id');
        if (allergyId < 0) {
            $(obj).remove();
        }

    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: deleting Lis of Medications list
    deleteMedication: function (obj, ev) {
        ev.stopPropagation();
        var medicationId = $(obj).attr('id');
        if (medicationId < 0) {
            $(obj).remove();
        }

    },
    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: to show delete icon on hover
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: to hide delete icon on mouse leave
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },
    UnLoad: function (caller) {
        if (ClinicalProcedureOrderDetail.params["CurrentNotesProviderId"])
            ClinicalProcedureOrderDetail.params["CurrentNotesProviderId"] = "";
        ClinicalProcedureOrderDetail.checkedProblems = [];

        var saveButtonisHidden = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder").hasClass("hidden");
        //Start 03-03-2016 Humaira Yousaf to close form after save
        if (caller == 'saveExit' || saveButtonisHidden == true) {
                UnloadActionPan(ClinicalProcedureOrderDetail.params["ParentCtrl"], "ClinicalProcedureOrderDetail", null, ClinicalProcedureOrderDetail.params["PanelID"]);
        }
            //End 03-03-2016 Humaira Yousaf to close form after save
            //Start 11-04-2016 Humaira Yousaf
        else if ($('#' + ClinicalProcedureOrderDetail.params.PanelID + " #btnPrintOrder").hasClass("hidden") == false) {
            UnloadActionPan(ClinicalProcedureOrderDetail.params["ParentCtrl"], "ClinicalProcedureOrderDetail", null, ClinicalProcedureOrderDetail.params["PanelID"]);
        }
        else {
            var detailAdded = ClinicalProcedureOrderDetail.isDetailAdded();
            if (ClinicalProcedureOrderDetail.params.mode == "Add" && detailAdded == false) {
                UnloadActionPan(ClinicalProcedureOrderDetail.params["ParentCtrl"], "ClinicalProcedureOrderDetail", null, ClinicalProcedureOrderDetail.params["PanelID"]);
            }
            else {
                utility.UnLoadDialog("frmClinicalProcedureOrderDetail", function () {
                    UnloadActionPan(ClinicalProcedureOrderDetail.params["ParentCtrl"], "ClinicalProcedureOrderDetail", null, ClinicalProcedureOrderDetail.params["PanelID"]);
                }, function () {
                    UnloadActionPan(ClinicalProcedureOrderDetail.params["ParentCtrl"], "ClinicalProcedureOrderDetail", null, ClinicalProcedureOrderDetail.params["PanelID"]);
                });
            }
        }
        //End 11-04-2016 Humaira Yousaf
        $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
    },

    //Function Name: ProcedureOrderSave
    //Author Name: Farooq Ahmad
    //Created Date: 18-03-2016
    //Description: Saves ProcedureOrder
    ProcedureOrderSave: function (sender) {
        var FavVal = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #ddlFavoriteListProcedureOrder').val();
        var strMessage = "";
        var ProcedureOrderId = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #hfProcedureOrderID").val() != "" ? $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #hfProcedureOrderID").val() : "-1";
        if (parseInt(ProcedureOrderId) > 0) {
            ClinicalProcedureOrderDetail.params.mode = "Edit";
        }
        else {
            ClinicalProcedureOrderDetail.params.mode = "Add";
        }

        var self = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail");
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
            ClinicalProcedureOrderDetail.ProcedureOrderProblems = ProcedureOrderProblemList;
            //End 06-04-2016 Humaira Yousaf for problems list
            //Start 22-03-2016 Humaira Yousaf for status
            if (sender == 'signorder' || sender == 'signprintorder')
                objData["Status"] = 'Signed';
            else if (sender == 'save')
                objData["Status"] = 'Draft';
            //End 22-03-2016 Humaira Yousaf for status
            if (objData.OrderDate != null) objData.ProcedureOderDate = objData.OrderDate;
            if (objData.OrderTime != null) objData.ProcedureOderTime = objData.OrderTime;

            // objData["CPTSNOMEDCodeId"] = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #hfCPTSNOMEDCode').val();
            // objData["CPTSNOMEDDescription"] = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #hfCPTSNOMEDDescription').val();

            myJSON = JSON.stringify(objData);

            //------------------------------------------------------------
            var ProcedureTestIds = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure tbody tr:not([id*=Child]").map(function () {
                return this.id.replace("id", "");
            }).get().join(',');
            $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #pnlProcedure_Result #hfProcedureTestIds").val(ProcedureTestIds);
            var selfProcedureTest = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #pnlProcedure_Result");
            var myJSONProcedureTest = selfProcedureTest.getMyJSON();
            var objRad = new Object();
            objRad["ProcedureTestIds"] = ProcedureTestIds;
            JSONToSave = utility.MergeJSON(myJSON, myJSONProcedureTest);
            //Start 08-03-2016 Humaira Yousaf to save test child row data
            $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure tbody tr:not([id*=Child]").each(function (index, item) {
                var childExists = ClinicalProcedureOrderDetail.EditableGrid.datatable.row(index).child();

                if (typeof childExists != 'undefined') {
                    var childRow = ClinicalProcedureOrderDetail.EditableGrid.datatable.row(index).child().get();
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
            ClinicalProcedureOrderDetail.cacheProcedureOrderData();
            //End 06-04-2016 Humaira Yousaf to save procedure order detail data


            // var isSavedSuccessfully = false;
            if (ClinicalProcedureOrderDetail.params.mode == "Add") {

                AppPrivileges.GetFormPrivileges("Orders and Results_Procedure", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        ClinicalProcedureOrderDetail.saveProcedureOrder(JSONToSave).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
                                var isFavListOpened = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #favSectionDiv").hasClass("toggled");
                                $.when(EMRUtility.insertUpdateFavListStatus(ClinicalProcedureOrderDetail.FavListName, !isFavListOpened)).then(function () {
                                    ClinicalProcedureOrderDetail.SaveFavListVal(FavVal);
                                });
                                //End 31-05-2016 Abid Ali for favorite list setting for all favLists

                                if (ClinicalProcedureOrderDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                    ClinicalProcedureOrderDetail.getLatestProcedureOrderByPatientId(true);
                                }
                                //Start 07-03-2016 Humaira Yousaf to save ProcedureOrder Id
                                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #hfProcedureOrderID").val(response.procedureOrderId);
                                //End 07-03-2016 Humaira Yousaf to save ProcedureOrder Id
                                utility.DisplayMessages(response.message, 1);
                                try {

                                    $.when(Clinical_ProcedureOrder.procedureOrderSearch(null, null, null, "ProcedureOrderDetail")).then(function () {
                                        if (ClinicalProcedureOrderDetail.params.ParentCtrl == "Clinical_ProcedureOrder" && Clinical_ProcedureOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                                            $("#pnlClinicalProgressNote #pnlClinicalProcedureOrder #dgvProcedureOrder input#" + response.procedureOrderId).prop("checked", true);
                                        }
                                    });

                                }
                                catch (ex) {
                                    console.log(ex);
                                }
                                //Start 22-03-2016 Humaira Yousaf to view PDF

                                if (sender == 'signprintorder') {
                                    ClinicalProcedureOrderDetail.printProcedureOrder();
                                }
                                else {
                                    //Start 03-03-2016 Humaira Yousaf to unload form on save
                                    ClinicalProcedureOrderDetail.UnLoad('saveExit');
                                    $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                                    //End 03-03-2016 Humaira Yousaf to unload form on save
                                }
                                //End 22-03-2016 Humaira Yousaf to view PDF
                                //Start 13-04-2016 Humaira Yousaf
                                //if (response.procedureFavList == true)
                                //    SetGlobalAppData('ProcedureFavListOpened', 'True');
                                //else
                                //    SetGlobalAppData('ProcedureFavListOpened', 'False');
                                // //Start 13-04-2016 Humaira Yousaf
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
            else if (ClinicalProcedureOrderDetail.params.mode == "Edit") {

                AppPrivileges.GetFormPrivileges("Orders and Results_Procedure", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        ClinicalProcedureOrderDetail.updateProcedureOrder(JSONToSave, ProcedureOrderId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
                                var isFavListOpened = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #favSectionDiv").hasClass("toggled");
                                $.when(EMRUtility.insertUpdateFavListStatus(ClinicalProcedureOrderDetail.FavListName, !isFavListOpened)).then(function () {
                                    ClinicalProcedureOrderDetail.SaveFavListVal(FavVal);
                                });
                                //End 31-05-2016 Abid Ali for favorite list setting for all favLists

                                if (ClinicalProcedureOrderDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                    ClinicalProcedureOrderDetail.getLatestProcedureOrderByPatientId();
                                }
                                //Start 07-03-2016 Humaira Yousaf to save ProcedureOrder Id
                                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #hfProcedureOrderID").val(response.procedureOrderId);
                                //End 07-03-2016 Humaira Yousaf to save ProcedureOrder Id
                                utility.DisplayMessages(response.message, 1);
                                try {
                                    $.when(Clinical_ProcedureOrder.procedureOrderSearch(null, null, null, "ProcedureOrderDetail")).then(function () {
                                        if (ClinicalProcedureOrderDetail.params.ParentCtrl == "Clinical_ProcedureOrder" && Clinical_ProcedureOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                                            $("#pnlClinicalProgressNote #pnlClinicalProcedureOrder #dgvProcedureOrder input#" + response.procedureOrderId).prop("checked", true);
                                        }
                                    });
                                }
                                catch (ex) {
                                    console.log(ex);
                                }
                                //Start 22-03-2016 Humaira Yousaf to view PDF
                                if (sender == 'signprintorder') {
                                    ClinicalProcedureOrderDetail.printProcedureOrder();
                                }
                                else {
                                    //Start 03-03-2016 Humaira Yousaf to unload form on save
                                    ClinicalProcedureOrderDetail.UnLoad('saveExit');
                                    $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                                    //End 03-03-2016 Humaira Yousaf to unload form on save

                                }
                                //End 22-03-2016 Humaira Yousaf to view PDF
                                //Start 13-04-2016 Humaira Yousaf
                                //if (response.procedureFavList == true)
                                //                SetGlobalAppData('ProcedureFavListOpened', 'True');
                                //            else
                                //                SetGlobalAppData('ProcedureFavListOpened', 'False');
                                //End 11-04-2016 Humaira Yousaf
                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });

            }
        }
        else {
            utility.DisplayMessages(mainErrorMessage, 3);
        }
    },
    SaveFavListVal: function (FavListVal) {
        EMRUtility.insertUpdateFavListVal(ClinicalProcedureOrderDetail.FavListName, FavListVal);
    },
    toggleHorSmallLeftIcon: function (e) {
        if (e === undefined) {
            var icon = $('.toggleHorSmallLeft');
            icon.each(function (i) {
                var $this = $(this).children("section").children();
                if ($(this).hasClass("toggled")) {
                    $this.append('<i class="fa fa-chevron-down"></i>');
                }
                else {
                    $this.append('<i class="fa fa-chevron-up"></i>');
                }
            });
        }
        else if (e != undefined) {
            var icon = $(e.children().children());
            if (icon.hasClass("fa-chevron-up")) {
                icon.toggleClass("fa-chevron-down fa-chevron-up")
            }
            else {
                icon.toggleClass("fa-chevron-up fa-chevron-down")
            }
        }
    },

    //Author:Farooq AHmad
    //Date: 18-03-2016
    //This function will handle autocomplete for Provider/Assignee
    loadAllAutocomplete: function () {


        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtAssignee");
            var hfCtrl = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfAssignee");
            var onSelect = function (dataItem) { $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtAssignee").attr("AssigneeId", dataItem.id); }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").data('serialize', $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").serialize());
        });
    },

    //Function Name: ProcedureOrderSave
    //Author Name: Farooq Ahmad
    //Created Date: 18-03-2016
    //Description: Saves ProcedureOrder
    //Params: ProcedureOrderData
    saveProcedureOrder: function (ProcedureOrderData) {
        var objData = JSON.parse(ProcedureOrderData);
        objData["commandType"] = "SAVE_PROCEDURE_ORDER";
        if (ClinicalProcedureOrderDetail.params.ParentCtrlPanelID == "pnlClinicalProgressNote #pnlClinicalProcedureOrder") {
            objData["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            objData["NoteId"] = "";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },
    //Function Name: openProcedureOrderAlert
    //Author Name: Ahmad Raza
    //Created Date: 09-03-2016
    //Description: To show ProcedureOrder Search in Popup
    openProcedureOrderAlert: function () {

        if ($(" #mainForm  li#ProcedureOrderAlert span").text() != '' && $('#PatientProfile #hfPatientId').val() != '') {
            BackgroundLoaderShow(true);
            var params = [];


            params["FromAdmin"] = 0;
            //   params["StartupScreen"] = "message";
            LoadActionPan("Clinical_ProcedureOrder", params);
        }
    },

    //Function Name: loadProcedureOrder
    //Author Name: Faroq Ahmad
    //Created Date: 21-03-2016
    //Description: Loads ProcedureOrder data
    loadProcedureOrder: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Procedure", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (ClinicalProcedureOrderDetail.params.mode == "Edit") {
                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #ClinicalProcedureOrderDetail #txtProcedureOrderTitle").attr("enabled", "enabled");
                    ClinicalProcedureOrderDetail.fillProcedureOrder(ClinicalProcedureOrderDetail.params.ProcedureOrderId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            ClinicalProcedureOrderDetail.ProcedureOrderTestGridLoad(response);
                            var ProcedureOrderDetail = JSON.parse(response.ProcedureLoad_JSON);
                            if (ProcedureOrderDetail.length > 0)
                                ProcedureOrderDetail = ProcedureOrderDetail[0];
                            if (ProcedureOrderDetail.Status.toLowerCase() == "signed") {
                                ClinicalProcedureOrderDetail.disableProcedureOrderControls(true);
                            }
                            else {
                                ClinicalProcedureOrderDetail.disableProcedureOrderControls(false);
                            }
                            var decodeHtmlEntity = function (str) {
                                return str.replace(/&#(\d+);/g, function (match, dec) {
                                    return String.fromCharCode(dec);
                                });
                            };
                            if (ProcedureOrderDetail.ProviderName != null) {
                                ProcedureOrderDetail.Provider = decodeHtmlEntity(ProcedureOrderDetail.ProviderName);
                                ProcedureOrderDetail.txtProvider = decodeHtmlEntity(ProcedureOrderDetail.ProviderName);
                                ProcedureOrderDetail.hfProvider = ProcedureOrderDetail.ProviderId;

                                ProcedureOrderDetail.Assignee = decodeHtmlEntity(ProcedureOrderDetail.AssigneeName);
                                ProcedureOrderDetail.txtAssignee = decodeHtmlEntity(ProcedureOrderDetail.AssigneeName);
                                ProcedureOrderDetail.hfAssignee = ProcedureOrderDetail.AssigneeId;


                            }
                            if (response.ProcedureOrderProblems_JSON != null) {

                                response.ProcedureOrderProblems_JSON = JSON.parse(response.ProcedureOrderProblems_JSON);

                                for (var index in response.ProcedureOrderProblems_JSON) {
                                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #ulProblemLists td #chk" + response.ProcedureOrderProblems_JSON[index].ProblemId).attr("checked", "checked");
                                    ClinicalProcedureOrderDetail.pushProblems($('#' + ClinicalProcedureOrderDetail.params.PanelID + " #ulProblemLists td #chk" + response.ProcedureOrderProblems_JSON[index].ProblemId));
                                }

                                //'#' + ClinicalProcedureOrderDetail.params.PanelID +

                                //$(self).find("#ulProblemLists td").each(function (index, item) {
                                //    if ($(this).find("input:checkbox").prop("checked")) {
                                //        var objProblem = {
                                //            ProblemId: $(this).find("input:checkbox").val(),
                                //            Description: $(this).text()
                                //        }
                                //        ProcedureOrderProblemList.push(objProblem);
                                //    }
                                //});
                            }

                            var self = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail");
                            utility.bindMyJSON(true, ProcedureOrderDetail, false, self);

                            var Ctrl_p = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProvider");
                            var hfCtrl_p = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfProvider");
                            utility.SetKendoAutoCompleteSourceforValidate(Ctrl_p, Ctrl_p.val(), hfCtrl_p, hfCtrl_p.val());

                            var Ctrl_a = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtAssignee");
                            var hfCtrl_a = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfAssignee");
                            utility.SetKendoAutoCompleteSourceforValidate(Ctrl_a, Ctrl_a.val(), hfCtrl_a, hfCtrl_a.val());

                            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #hfProcedureOrderID").val(ProcedureOrderDetail.ProcedureOrderId);


                            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").data('serialize', $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").serialize());

                            ClinicalProcedureOrderDetail.favoriteListSearch();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    ClinicalProcedureOrderDetail.disableProcedureOrderControls(false);
                    //Start 06-04-2016 Humaira Yousaf to get procedure order detail data
                    ClinicalProcedureOrderDetail.populateProcedureOrderSavedData();
                    //End 06-04-2016 Humaira Yousaf to get procedure order detail data
                }
                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
                if (EMRUtility.getFavListStatus(ClinicalProcedureOrderDetail.FavListName))
                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #favSectionDiv").removeClass("toggled");
                else
                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #favSectionDiv").addClass("toggled");
                //End 10-06-2016 Abid Ali for favorite list setting for all favLists
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").data('serialize', $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").serialize());
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    //Function Name: fillProcedureOrder
    //Author Name: Farooq Ahmad
    //Created Date: 21-03-2016
    //Description: Fills ProcedureOrder
    //Params: ProcedureOrderId
    fillProcedureOrder: function (ProcedureOrderId) {
        var objData = new Object();
        objData["ProcedureOrderId"] = ProcedureOrderId;
        objData["commandType"] = "fill_ProcedureOrder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },

    //Function Name: updateProcedureOrder
    //Author Name: Humaira Yousaf5
    //Created Date: 07-03-2016
    //Description: Updates ProcedureOrder
    //Params: ProcedureOrderData, ProcedureOrderId
    updateProcedureOrder: function (ProcedureOrderData, ProcedureOrderId) {

        var objData = JSON.parse(ProcedureOrderData);
        objData["ProcedureOrderId"] = ProcedureOrderId;
        objData["commandType"] = "Save_Procedure_Order";
        if (ClinicalProcedureOrderDetail.params.ParentCtrlPanelID == "pnlClinicalProgressNote #pnlClinicalProcedureOrder") {
            objData["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            objData["NoteId"] = "";
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");

    },

    //Function Name: addAgeConditionValues
    //Author Name: Humaira Yousaf
    //Created Date: 07-03-2016
    //Description: Shows respective cotrols based on selected age condition
    addAgeConditionValues: function () {
        var ageCondition = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ddlProcedureOrderAgeCondition").val();
        //Start 1-03-2016 Humaira Yousaf
        if (ageCondition == "") {
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ageConditionRange").addClass("hidden");
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ageConditionValue").addClass("hidden");
            var ageToValue = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderAgeValue]').val('');
            var ageToValue = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderAgeFrom]').val('');
            var ageToValue = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderAgeTo]').val('');
        }
            //End 1-03-2016 Humaira Yousaf
        else if (ageCondition == "6") {
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ageConditionRange").removeClass("hidden");
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ageConditionValue").addClass("hidden");
        }
        else {
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ageConditionValue").removeClass("hidden");
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ageConditionRange").addClass("hidden");
        }
    },

    //Function Name: isReminderValid
    //Author Name: Humaira Yousaf
    //Created Date: 07-03-2016
    //Description: Validates reminder
    isReminderValid: function () {
        var Message = "";

        var reminder = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderReminderLength]');
        var stayLength = $(reminder).val();
        var ddlVal = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail #ddlProcedureOrderReminderPeriod').val();
        if (stayLength != null && stayLength != '') {
            if (ddlVal == null || ddlVal == '') {
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail #ddlProcedureOrderReminderPeriod').focus();
                Message = "Please select Reminder Period.";
            }
        }

        if (ddlVal != null && ddlVal != '') {
            if (stayLength == null || stayLength == '') {
                $(reminder).focus();
                Message = "Please enter Reminder Period.";
            }
        }

        return Message;
    },
    //Function Name: hideShowDataDivs
    //Author Name: Humaira Yousaf
    //Created Date: 08-03-2016
    //Description: HideShow Vitals,problems list etc divs on editing
    hideShowDataDivs: function () {
        var selectedValue = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ddlProcedureOrderRuleType option:selected");
        var selected = [];
        $(selectedValue).each(function (index, val) {
            selected.push($(this).text());
        });
        var unSelect = '';
        $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail .comList").addClass('hidden');
        $(selected).each(function (i, item) {
            var sectionName = item;
            unSelect += sectionName + ',';
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #dv" + item.replace(/\s/g, '')).removeClass('hidden');
        });
    },
    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: to show ProcedureOrder Alert on Dashboard
    showProcedureOrderAlert: function (triggerLocation) {

        ClinicalProcedureOrderDetail.showProcedureOrderAlertDBCall(triggerLocation).done(function (response) {
            response = JSON.parse(response);
            //Start//09-03-2016//Ahmad Raza//setting hiddenField values
            $(" #mainForm  li#ProcedureOrderAlert input").val('');
            $(" #mainForm  li#ProcedureOrderAlert input").val(function (i, val) {
                return val + (val ? ', ' : '') + response.ProcedureOrderIDs;
            });
            //End//09-03-2016//Ahmad Raza//setting hiddenField values
            if (response.status != false) {

                if (response.alertCount > 0) {
                    $(" #mainForm  li#ProcedureOrderAlert span").text(response.alertCount);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: DBCall to show ProcedureOrder Alert on Dashboard
    showProcedureOrderAlertDBCall: function (triggerLocation) {
        var objData = new Object();
        if (triggerLocation == 'FaceSheet') {
            objData["ProcedureOrderTriggerLocation"] = '1';
        }
        else if (triggerLocation == 'Notes') {
            objData["ProcedureOrderTriggerLocation"] = '2';
        }
        objData["PatientId"] = Clinical_ProcedureOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "show_ProcedureOrder_alert";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },

    //Function Name: loadProcedureOrderMedications
    //Author Name: Humaira Yousaf
    //Created Date: 10-03-2016
    //Description: Loads ProcedureOrder Medications
    //Params: ProcedureOrderMedications
    loadProcedureOrderMedications: function (ProcedureOrderMedications) {
        $(ProcedureOrderMedications).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li id=" + item.drugId + " onclick='' \"><div><a href='#'>" + item.medicationName + "<span class='removeIconListHover' onclick='ClinicalProcedureOrderDetail.deleteMedication($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li id=" + item.drugId + " onclick='' \"><div><select id='ddlMedications" + item.drugId + "' name = 'ProcedureOrderMedications" + item.drugId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.medicationName + "<span class='removeIconListHover' onclick='ClinicalProcedureOrderDetail.deleteMedication($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ulProcedureOrderMedications").append(li);

            if (index != 0) {
                if (item.medicationOperator == 'AND') {
                    $($('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ulProcedureOrderMedications li#" + item.drugId).find("#ddlMedications" + item.drugId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ulProcedureOrderMedications li#" + item.drugId).find("#ddlMedications" + item.drugId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProcedureOrderMedications").val('');
        });
    },
    //Function Name: loadProcedureOrderAllergies
    //Author Name: Ahmad Raza
    //Created Date: 16-03-2016
    //Description: Loads ProcedureOrder Allergies
    //Params: ProcedureOrderAllergies
    loadProcedureOrderAllergies: function (ProcedureOrderAllergies) {
        $(ProcedureOrderAllergies).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li id=" + item.AllergyId + " onclick='' \"><div ><a href='#'>" + item.Allergen + "<span class='removeIconListHover' onclick='ClinicalProcedureOrderDetail.deleteAllergy($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li id=" + item.AllergyId + " onclick='' \"><div><select id='ddlAllergies" + item.AllergyId + "' name = 'ProcedureOrderMedications" + item.AllergyId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.Allergen + "<span class='removeIconListHover' onclick='ClinicalProcedureOrderDetail.deleteAllergy($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ulProcedureOrderAllergies").append(li);

            if (index != 0) {
                if (item.AllergyOperator == 'AND') {
                    $($('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ulProcedureOrderAllergies li#" + item.AllergyId).find("#ddlAllergies" + item.AllergyId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ulProcedureOrderAllergies li#" + item.AllergyId).find("#ddlAllergies" + item.AllergyId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProcedureOrderAllergies").val('');
        });
    },

    //Function Name: loadProcedureOrderProblems
    //Author Name: Ahmad Raza
    //Created Date: 16-03-2016
    //Description: Loads ProcedureOrder Problems
    //Params: ProcedureOrderProblems
    loadProcedureOrderProblems: function (ProcedureOrderProblems) {
        $(ProcedureOrderProblems).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li name='" + item.Problem + "' onclick='' \"><div ><a href='#'>" + item.Problem + "<span class='removeIconListHover' onclick='ClinicalProcedureOrderDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li name='" + item.Problem + "' onclick='' \"><div><select id='ddlProblemList" + item.Problem + "' name = 'ProcedureOrderProblemList" + item.ProblemId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.Problem + "<span class='removeIconListHover' onclick='ClinicalProcedureOrderDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ulProcedureOrderProblemList").append(li);

            if (index != 0) {
                if (item.ProblemOperator == 'AND') {
                    $($('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ulProcedureOrderProblemList li#" + item.ProblemId).find("#ddlProblemList" + item.ProblemId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #ulProcedureOrderProblemList li#" + item.ProblemId).find("#ddlProblemList" + item.ProblemId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProcedureOrderProblemList").val('');
        });
    },
    //Function Name: isAgeConditionValid
    //Author Name: Humaira Yousaf
    //Created Date: 11-03-2016
    //Description: Validates Age Condition
    isAgeConditionValid: function () {
        var Message = "";
        var ddlAgeConditionVal = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail #ddlProcedureOrderAgeCondition').val();

        if (ddlAgeConditionVal == '6') {

            var ageFromValue = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderAgeFrom]').val();
            var ageToValue = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderAgeTo]').val();

            if ((ageFromValue == null || ageFromValue == '') && (ageToValue == null || ageToValue == '')) {
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderAgeFrom]').focus();
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderAgeTo]').focus();
                Message = "Please enter From Age and To Age.";

            }
            else {
                if (ageFromValue != null || ageFromValue != '') {
                    if (ageToValue == null || ageToValue == '') {
                        $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderAgeTo]').focus();
                        Message = "Please enter To Age.";
                    }
                }

                if (ageToValue != null || ageToValue != '') {
                    if (ageFromValue == null || ageFromValue == '') {
                        $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderAgeFrom]').focus();
                        Message = "Please enter From Age.";
                    }
                }
            }

        }
        else {
            var ageValue = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderAgeValue]').val();
            if (ageValue == null || ageValue == '') {
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #frmClinicalProcedureOrderDetail input[id*=txtProcedureOrderAgeValue]').focus();
                Message = "Please enter Age.";
            }
        }

        return Message;
    },
    //Function Name: selectAllFavoriteList
    //Author Name: Ahmad Raza
    //Created Date: 24-03-2016
    //Description: this function triggers all favorite list's click event
    selectAllFavoriteList: function () {

        $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #ulFavoriteListProcedureOrderContent li').each(function (i, item) {
            $(item).trigger("click");

        });


    },




    //------------------ start Irfan Code for editable grid

    bindAutoComplete: function (element) {
        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "ClinicalProcedureOrderDetail", null, true);

    },

    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalProcedureOrderDetail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = ClinicalProcedureOrderDetail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, ClinicalProcedureOrderDetail.params.PanelID);
    },


    BindProcedureGridItem: function (cptCode, procedureDescription, cptDescription, sender, snomedcode, snomedDescription) {

        var cptCode = cptCode;
        var procDesc = procedureDescription;
        var cptDesc = cptDescription;

        var currentRow = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure tbody tr");
        var isTestAlreadySelected = false;
        $.each(currentRow, function (i, item) {
            var currentRowCPTDescription = $(item).find("input[id*='ProcedureProcedure']").val() != null ? $(item).find("input[id*='ProcedureProcedure']").val().replace(cptCode, "").trim() : "";
            if (cptDescription.toLowerCase().trim() == currentRowCPTDescription.toLowerCase().trim()) {
                isTestAlreadySelected = true;
                return;
            }
        });

        if (isTestAlreadySelected != true) {
            var CurrentRow = ClinicalProcedureOrderDetail.AddNewProcedureRow(null, null, null, cptCode, procDesc, cptDescription, snomedcode, snomedDescription);
            //to append dropdown in first row of procedure
            if ($(CurrentRow).attr('id') == '-1') {
                $(CurrentRow).loadDropDowns(true, null, 'dgvProcedure #-1').done(function () {
                });
            }
            //
            $(sender).addClass('disableAll');
            setTimeout(function () {
                $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #txtProcedureCPTCode").val('');
            }, 200);
        }
        else {
            //$(sender).addClass('disableAll');
            utility.DisplayMessages("Procedure is already selected", 2);
        }
        //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
        if ($("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure tbody tr").length > 0 && $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure tbody tr").find('.dataTables_empty').length == 0) {
            $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetProcedureOrder").removeClass('disableAll');
        }
        else {
            $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetProcedureOrder").addClass('disableAll');
        }
        //End 05-04-2016 Humaira Yousaf to enable/disable action buttons
    },

    //Function Name: isFavoriteProcedureFound
    //Author Name: Humaira Yousaf
    //Created Date: 12-04-2016
    //Description: Checks if Favorite Procedure is found
    isFavoriteProcedureFound: function (favCPTCode, favCPTDesc) {

        var isFound = false;
        $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure tbody tr").each(function (index, item) {
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
    //Function Name: enableFavoriteList
    //Author Name: Humaira Yousaf
    //Created Date: 12-04-2016
    //Description: Enables Favorite List
    enableFavoriteList: function (deleteRow) {

        $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #ulFavoriteListProcedureOrderContent li').each(function (index, item) {
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

    //Function Name: buildRowChild
    //Author: Humaira Yousaf
    //Date: 05-04-2016
    //Description: Builds child row for test information
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

    //START Charge Editable Grid Code

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

    rowDetail: function ($row, obj) {
        var ChargeCapId = Number($row.attr("id"));
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Procedure", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ChargeCapId"] = ChargeCapId;
                params["mode"] = "Edit";
                //Edited by Azeem Raza Tayyab on 16-Feb-2016 to fix bug#: PMS-3871
                if (ClinicalProcedureOrderDetail.params.TabID == 'chargeBatchDetail' || ClinicalProcedureOrderDetail.params.TabID == 'billTabUnClaimedAppointment' || ClinicalProcedureOrderDetail.params.TabID == 'Bill_ChargeSearch' || ClinicalProcedureOrderDetail.params.TabID == 'Patient_Case_Detail' || ClinicalProcedureOrderDetail.params.TabID == 'schTabCalendar' || ClinicalProcedureOrderDetail.params.TabID == 'batchTabEncounter' || ClinicalProcedureOrderDetail.params.TabID == 'Bill_FollowUpPatientAR_Detail' || ClinicalProcedureOrderDetail.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || ClinicalProcedureOrderDetail.params.TabID == "billTabClaimSubmission" || ClinicalProcedureOrderDetail.params.TabID == "Bill_PaymentPosting" || ClinicalProcedureOrderDetail.params.TabID == "EDIClaimViewDetail")
                    params["ParentCtrl"] = 'ClinicalProcedureOrderDetail';
                else
                    params["ParentCtrl"] = ClinicalProcedureOrderDetail.params["TabID"];
                LoadActionPan('chargeSearchDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowRemove: function ($row, obj) {
        utility.myConfirm('1', function () {
            if ($row.hasClass('adding')) {
            }
            //var _self = obj;
            //_self.datatable.row($row.get(0)).remove().draw();
            if (parseInt($row.attr("id")) > 0) {
                ClinicalProcedureOrderDetail.DeleteProcedureOrderTest($row.attr("id"), $row, obj);
            }
            else {
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
                utility.DisplayMessages("Successfully Deleted", 1);
                //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
                if ($("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure tbody tr").length > 0 && $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure tbody tr").find('.dataTables_empty').length == 0) {
                    $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetProcedureOrder").removeClass('disableAll');
                }
                else {
                    $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetProcedureOrder").addClass('disableAll');
                }
                //End 05-04-2016 Humaira Yousaf to enable/disable action buttons

                //Start 12-04-2016 Humaira Yousaf
                ClinicalProcedureOrderDetail.enableFavoriteList($row);
                //Start 12-04-2016 Humaira Yousaf
            }

        }, function () {
        },
                    '1'
    );

    },

    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
        //    .each(function () {
        //    $(this).removeclass('hidden')
        //});
    },

    AddNewProcedureRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription, snomedcode, snomedDescription) {

        var ProcedureGridId = "#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure";

        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (ClinicalProcedureOrderDetail.params.ParentCtrl != null) {
                CurrentRow = ClinicalProcedureOrderDetail.EditableGrid.rowAdd(RowId, "");
            }
            else {
                CurrentRow = ClinicalProcedureOrderDetail.EditableGrid.rowAdd(RowId, ClinicalProcedureOrderDetail.params.ProcedureId);
            }

        }
        else {
            var TemplateRow = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            CurrentRow = ClinicalProcedureOrderDetail.EditableGrid.rowAdd(TemplateRowId - 1, "");
        }

        var rowId = CurrentRow.attr("id");

        var cptCodeHtml = '<input type="hidden" id="CPTCode' + rowId + '"  name="CPTCode" value="' + cptCode + '" />';
        var cptDescHtml = '<input type="hidden" id="CPTDescription' + rowId + '" name="CPTDescription"  value="' + cptDescription + '"  />';

        $(CurrentRow).find('td:first').append(cptCodeHtml + cptDescHtml);


        var SnomedCodeHtml = '<input type="hidden" id="CPTSNOMEDCodeId' + rowId + '"  name="CPTSNOMEDCodeId" value="' + snomedcode + '" />';
        var SnomedDescHtml = '<input type="hidden" id="CPTSNOMEDDescription' + rowId + '" name="CPTSNOMEDDescription"  value="' + snomedDescription + '"  />';

        $(CurrentRow).find('td:first').append(SnomedCodeHtml + SnomedDescHtml);


        var row = ClinicalProcedureOrderDetail.EditableGrid.datatable.row(CurrentRow);
        //Start 05-04-2016 Humaira Yousaf to add child row
        row.child(ClinicalProcedureOrderDetail.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));
        row.child.hide();
        //End 05-04-2016 Humaira Yousaf to add child row
        $(CurrentRow).find('input[id*="ProcedureProcedure"]').val(cptCode + " " + cptDescription);
        //Start 12-04-2016 Humaira Yousaf
        var selectedFavListId = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #ddlFavoriteListProcedureOrder').find("option:selected").attr('id');
        //  $(CurrentRow).find('input[id*="ProcedureProcedure"]').attr("listCPTid", cptCode);
        if (cptCode != null && cptDescription != null) {
            $(CurrentRow).find('input[id*="ProcedureProcedure"]').attr("listCPTid", cptCode + " " + cptDescription);
        }
        else {
            $(CurrentRow).find('input[id*="ProcedureProcedure"]').attr("listCPTid", null);
        }
        //End 12-04-2016 Humaira Yousaf
        ClinicalProcedureOrderDetail.enableRemoveRow($(CurrentRow));


        //Start Farooq Ahmad 07/14/2016 /EMR-588
        var dgvProcedure = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure");

        $(dgvProcedure).find('input[id*="dtpProcedureDate"]').removeClass('size70');
        $(dgvProcedure).find('input[id*="dtpProcedureDate"]').closest('div').removeClass('size70');

        $(dgvProcedure).find('input[id*="tpProcedureTime"]').removeClass('size70');
        $(dgvProcedure).find('input[id*="tpProcedureTime"]').closest('div').removeClass('size70');

        $(dgvProcedure).find('input[id*="ProcedureProcedure"]').addClass('size-min300');
        //End Farooq Ahmad 07/14/2016 /EMR-588
        return CurrentRow;

    },

    ProcedureOrderTestGridLoad: function (response) {
        //var response = JSON.parse(response);
        var PanelProcedureGrid = "#" + ClinicalProcedureOrderDetail.params.PanelID + " #pnlProcedure_Result";
        var ProcedureGridId = "#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure";
        $(ProcedureGridId + " tbody tr").remove();
        if ($.fn.dataTable.isDataTable(ProcedureGridId)) {
            $(ProcedureGridId).dataTable().fnClearTable();
            $(ProcedureGridId).dataTable().fnDestroy();
        }
        ClinicalProcedureOrderDetail.EditableGrid.datatable.clear().draw();
        var ProcedureOrderTestLoadJSONData = JSON.parse(response.procedureOrderTest_JSON);
        //Start 05-04-2016 Humaira Yousaf to add child row
        $.each(ProcedureOrderTestLoadJSONData, function (i, item, CurrentRow, newChildRow) {
            var ProcedureOrderTestId = item.ProcedureOrderTestId;
            var arrChildProcedure = [];
            arrChildProcedure.push(item);
            CurrentRow = ClinicalProcedureOrderDetail.AddNewProcedureRow(ProcedureOrderTestId, null, null, item.CPTCode, item.CPTDescription, item.CPTDescription, null, null);
            ClinicalProcedureOrderDetail.buildRowChild(CurrentRow, CurrentRow.attr("id"), null, null, arrChildProcedure);

            var self = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " tr#" + ProcedureOrderTestId);
            var row = ClinicalProcedureOrderDetail.EditableGrid.datatable.row(CurrentRow);
            item.CurrentRow = CurrentRow;
            newChildRow = row.child();
            item.newChildRow = newChildRow;
            var ProcedureTestTable = $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure");

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
                        $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").data('serialize', $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").serialize());
                    });
                }
                //End 08-11-2016 Edit By Humaira Yousaf Bug# EMR-1925
            }
            BindFunction(counter, item, CurrentRow, newChildRow);
            //End 05-04-2016 Humaira Yousaf to add child row
            //End Farooq Ahmad 03/28/2016 bind values to the table
        });

    },

    DeleteProcedureOrderTest: function (ProcedureTestId, $row, obj) {

        ClinicalProcedureOrderDetail.ProcedureOrderTest_DBCall(ProcedureTestId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();

                //Start 08-04-2016 Humaira Yousaf to enable/disable action buttons
                if ($("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure tbody tr").length > 0 && $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure tbody tr").find('.dataTables_empty').length == 0) {
                    $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetProcedureOrder").removeClass('disableAll');
                }
                else {
                    $("#" + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetProcedureOrder").addClass('disableAll');
                }
                //End 08-04-2016 Humaira Yousaf to enable/disable action buttons
                //Start 12-04-2016 Humaira Yousaf
                ClinicalProcedureOrderDetail.enableFavoriteList($row);
                //End 12-04-2016 Humaira Yousaf

            } else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },

    ProcedureOrderTest_DBCall: function (ProcedureTestId) {

        var objData = new Object();
        objData["ProcedureOrderTestId"] = ProcedureTestId;
        objData["PatientId"] = Clinical_ProcedureOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "DELETE_PROCEDUREORDER_TEST";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },

    //------------------ end Irfan Code for editable grid


    //Function Name: getLatestProcedureOrderByPatientId
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To get Latest Procedure Order By PatientId
    getLatestProcedureOrderByPatientId: function (hideAlertMessage) {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            ClinicalProcedureOrderDetail.getLatestProcedureOrderByPatientIdDBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    ClinicalProcedureOrderDetail.createProcedureOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }

            });
        }
        else {
            utility.DisplayMessages(strMessage, 3);
        }
    },
    //Function Name: checkProcedureOrderExists
    //Author Name: Ahmad Raza
    //Created Date: 24-03-2016
    //Description: To check whether procedure order exists or not
    checkProcedureOrderExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedureorder').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ProcedureOrderComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_procedureorder title="Procedure Order"  id="clinicalMenu_Orders_Procedure" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'ProcedureOrder\',\'clinicalMenu_Orders_Procedure\',' + Clinical_ProgressNote.params.NotesId + ');" title="Procedure Order">Procedure Order</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'ProcedureOrder\',\'clinicalMenu_Orders_Procedure\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_procedureorder> </header></li>');
            Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedureorder').parent().parent().removeClass('hidden');
    },
    //Function Name: createProcedureOrderBodyHTML
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: creating HTML of Procedure Order
    createProcedureOrderBodyHTML: function (response, NoteHTMLCtrl, UnloadProcedureOrder, hideAlertMessage) {
        ClinicalProcedureOrderDetail.checkProcedureOrderExists();
        if (response.ProcedureOrderFill_JSON != null && response.ProcedureOrderFill_JSON != '') {
            var ProcedureOrderFill_Obj = JSON.parse(response.ProcedureOrderFill_JSON);
            var $mainDivProcedureOrder = $(document.createElement('div'));

            var ProcedureOrderId = ProcedureOrderFill_Obj.ProcedureOrderId;
            if (ProcedureOrderId > 0) {
                var $SectionBodyProcedureOrder = $(document.createElement('section'));
                $SectionBodyProcedureOrder.attr('id', "Cli_ProcedureOrderDetail_Main" + ProcedureOrderId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_ProcedureOrderDetail_" + ProcedureOrderId);
                var $ListProcedureOrder = $(document.createElement('ul'));

                $ListProcedureOrder.attr('class', 'list-unstyled')

                $SectionBodyProcedureOrder.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ProcedureOrderDetail_" + ProcedureOrderId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_ProcedureOrderDetail_Main" + ProcedureOrderId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListProcedureOrder.append("<li>" + ProcedureOrderFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListProcedureOrder);
                $SectionBodyProcedureOrder.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_procedureorder').parent().parent().find('#Cli_ProcedureOrderDetail_Main' + ProcedureOrderId).length == 0) {
                    $mainDivProcedureOrder.append($SectionBodyProcedureOrder);
                    ClinicalProcedureOrderDetail.updateProcedureOrderHtml($mainDivProcedureOrder.html(), ProcedureOrderId, NoteHTMLCtrl, hideAlertMessage);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_procedureorder').parent().parent().find('#Cli_ProcedureOrderDetail_Main' + ProcedureOrderId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_procedureorder').parent().parent().find('#Cli_ProcedureOrder_Main' + ProcedureOrderId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_procedureorder').parent().parent().find('#Cli_ProcedureOrderDetail_Main' + ProcedureOrderId).html($SectionBodyProcedureOrder.html());
                    $(NoteHTMLCtrl + ' clinical_procedureorder').parent().parent().find('#Cli_ProcedureOrderDetail_Main' + ProcedureOrderId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText("ProcedureOrder", hideAlertMessage);
                    ClinicalProcedureOrderDetail.updateProcedureOrderHtml("", ProcedureOrderId, NoteHTMLCtrl, hideAlertMessage);

                }

                if (UnloadProcedureOrder == true) {
                    ClinicalProcedureOrderDetail.Unload(ClinicalProcedureOrderDetail.bNextPrev);
                }
            }
        }
    },

    //Function Name: detachProcedureOrderFromNotes
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: detach Procedure Order From Notes
    detachProcedureOrderFromNotes: function (ProcedureOrderId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_procedureorder');
                var selectedValue = ProcedureOrderId.replace('Cli_ProcedureOrderDetail_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    ClinicalProcedureOrderDetail.detachProcedureOrderFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            var docId = $("#" + ProcedureOrderId).attr('patdocid');
                            if (docId > 0) {
                                Clinical_ProgressNote.DetachImageFromNotes_DBCall(docId).done(function (responseDoc) {
                                    if (responseDoc.status != false) {
                                        $('#' + ProcedureOrderId).remove();
                                        Patient_Document.DeleteDocument(docId);
                                        Clinical_ProgressNote.updateAttachDocumentButtonImg();
                                        Clinical_ProgressNote.saveComponentSOAPText("ProcedureOrder");
                                        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                    }
                                    else {
                                        utility.DisplayMessages(responseDoc.Message, 3);
                                    }
                                });
                            }
                            else {
                                $('#' + ProcedureOrderId).remove();
                                Clinical_ProgressNote.saveComponentSOAPText("ProcedureOrder");
                                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                            }

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
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    //Function Name: detachProcedureOrderFromNotes_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to detach Procedure Order From Notes
    detachProcedureOrderFromNotes_DBCall: function (ProcedureOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProcedureOrderId"] = ProcedureOrderId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "detach_procedureorder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },

    //Function Name: detach_ComponentsProcedureOrder
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To detach Procedure Order's Component From Notes
    detach_ComponentsProcedureOrder: function (ComponentName, IsUpdate, ProcedureOrderComponentRemove) {
        var procedureOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedureorder').parent().parent().find('section[id*="Cli_ProcedureOrderDetail_Main"]').map(function () {
            return this.id.replace("Cli_ProcedureOrderDetail_Main", "");
        }).get().join(',');

        var docIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedureorder').parent().parent().find('section[id*="Cli_ProcedureOrderDetail_Main"]').map(function () {
            return $(this).attr('patdocid');
        }).get().join(',');

        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ProcedureOrderComponent').attr('NoteComponentId');
        if (ProcedureOrderComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_procedureorder').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_procedureorder').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Procedure Order']").remove();
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Procedure Order']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_procedureorder').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder', true))
                }
                else {
                    if (NoteComponentId && NoteComponentId != "NCDummyId")
                        promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                    else {
                        var def = $.Deferred();
                        promise.push(def);
                        def.resolve();
                    }
                }
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_procedureorder').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedureorder').parent().parent().find('section[id*="Cli_ProcedureOrderDetail_Main"]').remove();
        }

        if (procedureOrderIds == "" || procedureOrderIds == "undefined") {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_procedureorder').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder', true))
            }
            else {
                if (NoteComponentId && NoteComponentId != "NCDummyId")
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                else {
                    var def = $.Deferred();
                    promise.push(def);
                    def.resolve();
                }
            }
            $.when.apply($, promise).done(function () {
                if (Clinical_ProgressNote.params["TemplateName"] == "")
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_procedureorder').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            });
        }
        else {
            ClinicalProcedureOrderDetail.detachProcedureOrderFromNotesDBCall(procedureOrderIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (docIds.length > 0) {
                        Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(docIds).done(function (responseDoc) {
                            if (responseDoc.status != false) {
                                Patient_Document.DeleteDocument(docIds);
                                if (IsUpdate) {
                                    Clinical_ProgressNote.saveComponentSOAPText("ProcedureOrder", true);
                                }
                                utility.DisplayMessages(response.Message, 1);
                                Clinical_ProgressNote.updateAttachDocumentButtonImg();
                                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                            }
                            else {
                                utility.DisplayMessages(responseDoc.Message, 3);
                            }
                        });
                    }
                    else {
                        if (IsUpdate) {

                            Clinical_ProgressNote.saveComponentSOAPText("ProcedureOrder", true);
                        }
                        utility.DisplayMessages(response.Message, 1);
                        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //Function Name: detachProcedureOrderFromNotesDBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call To detach Procedure Order's Component From Notes
    detachProcedureOrderFromNotesDBCall: function (ProcedureId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProcedureOrderId"] = ProcedureId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "detach_ProcedureOrder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },

    //Function Name: updateProcedureOrderHtml
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: update Procedure Order Html
    updateProcedureOrderHtml: function (ProcedureOrderHtml, ProcedureOrderId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' clinical_procedureorder').parent().parent().addClass('initialVisitBody');
        if (ProcedureOrderHtml != '') {
            $(NoteHTMLCtrl + ' clinical_procedureorder').parent().parent().append(ProcedureOrderHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ProcedureOrderHtml != '') {
            ClinicalProcedureOrderDetail.attachProcedureOrderWithNotes(ProcedureOrderId, hideAlertMessage);
        }
        else {
            var docIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Procedureorder').parent().parent().find('section[id*="Cli_ProcedureOrderDetail_Main"]').map(function () {
                return $(this).attr('patdocid');
            }).get().join(',');

            if (docIds.length > 0) {
                Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(docIds).done(function (responseDoc) {
                    responseDoc = JSON.parse(responseDoc);
                    if (responseDoc.status != false) {
                        Patient_Document.DeleteDocument(docIds).done(function (response) {
                            Clinical_ProgressNote.SaveAndAttachOrderReport("Procedure Order", ProcedureOrderId, false).done(function () {
                                Clinical_ProgressNote.saveComponentSOAPText("Procedure Order", true);
                            });
                        });
                    }
                    else {
                        utility.DisplayMessages(responseDoc.Message, 3);
                    }
                });
            }
        }
    },

    //Function Name: attachProcedureOrderWithNotes
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: attach Procedure Order With Notes
    attachProcedureOrderWithNotes: function (ProcedureOrderId, hideAlertMessage) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var selectedValue = ProcedureOrderId;
            if (selectedValue == "" || selectedValue == "undefined") {
                Clinical_ProgressNote.saveComponentSOAPText("ProcedureOrder", true);
                utility.DisplayMessages('Successfully Deleted', 1);
            }
            else {
                ClinicalProcedureOrderDetail.attachProcedureOrderWithNotesDBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //If Attached MedicalHx Made new inseration to MedicalHx Table than good ids should be attached to HTML
                        Clinical_ProgressNote.saveComponentSOAPText("ProcedureOrder", hideAlertMessage);
                        $('#' + ProcedureOrderId).remove();
                        // utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }


        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    //Function Name: attachProcedureOrderWithNotesDBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to attach Procedure Order With Notes
    attachProcedureOrderWithNotesDBCall: function (ProcedureOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProcedureOrderId"] = ProcedureOrderId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "attach_ProcedureOrder_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },

    //Function Name: getLatestProcedureOrderByPatientIdDBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to get Latest Procedure Order By PatientId
    getLatestProcedureOrderByPatientIdDBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_Procedureorderby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },

    printProcedureOrder: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = Clinical_ProcedureOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = ClinicalProcedureOrderDetail.params.TabID;//'mstrTabDashBoard';
        params["ProcedureOrderId"] = $('#' + ClinicalProcedureOrderDetail.params.PanelID + ' #hfProcedureOrderID').val();
        LoadActionPan('Clinical_ProcedureOrderView', params);
    },
    //Function Name: selectProvider
    //Author Name: Humaira Yousaf
    //Created Date: 06-04-2016
    //Description: Auto selects provider if same as logged in user
    selectProvider: function (providerId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Provider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                ClinicalProcedureOrderDetail.searchProvider(providerId).done(function (response) {
                    if (response.status != false) {
                        if (response.ProviderCount > 0) {
                            var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);
                            $.each(ProviderLoadJSONData, function (i, item) {
                                var name = new Array();
                                name = globalAppdata.AppUserNameFullName.split(',');
                                if (item.LastName == name[0] && item.FirstName == $.trim(name[1])) {
                                    var decodeHtmlEntity = function (str) {
                                        return str.replace(/&#(\d+);/g, function (match, dec) {
                                            return String.fromCharCode(dec);
                                        });
                                    };
                                    var Ctrl = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProvider");
                                    var hfCtrl = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfProvider");
                                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProvider").val(decodeHtmlEntity(item.LastName + ', ' + item.FirstName + ' ' + item.MiddleInitial));
                                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfProvider").val(decodeHtmlEntity(item.ProviderId));
                                    utility.SetKendoAutoCompleteSourceforValidate(Ctrl, Ctrl.val(), hfCtrl, hfCtrl.val());
                                }
                            });
                        }
                        $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").data('serialize', $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail").serialize());
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
    //Function Name: searchProvider
    //Author Name: Humaira Yousaf
    //Created Date: 06-04-2016
    //Description: Searches Providers
    searchProvider: function (providerId) {
        var data = "ProviderData=" + null + "&ProviderID=" + providerId + "&PageNumber=" + null + "&RowsPerPage=" + null;
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER", "SEARCH_PROVIDER");
    },
    //Function Name: cacheProcedureOrderData
    //Author Name: Humaira Yousaf
    //Created Date: 06-04-2016
    //Description: Saves procedure order detail data
    cacheProcedureOrderData: function () {
        Clinical_ProcedureOrder.params["ProviderName"] = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProvider").val();
        Clinical_ProcedureOrder.params["ProviderId"] = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfProvider").val();

        Clinical_ProcedureOrder.params["AssigneeName"] = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtAssignee").val();
        Clinical_ProcedureOrder.params["AssigneeId"] = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfAssignee").val();

        Clinical_ProcedureOrder.params["Problems"] = ClinicalProcedureOrderDetail.ProcedureOrderProblems;

        Clinical_ProcedureOrder.params["CurrentPatientId"] = Clinical_ProcedureOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        Clinical_ProcedureOrder.params["hasData"] = true;
    },

    getProcedureOrderInfo: function (procedureOrderId) {
        ClinicalProcedureOrderDetail.fillProcedureOrder(procedureOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    ClinicalProcedureOrderDetail.createProcedureOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //Function Name: populateProcedureOrderSavedData
    //Author Name: Humaira Yousaf
    //Created Date: 06-04-2016
    //Description: Gets procedure order detail data
    populateProcedureOrderSavedData: function () {
        if (Clinical_ProcedureOrder.params["hasData"] == true) {
            utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProvider"), Clinical_ProcedureOrder.params.ProviderName, $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfProvider"), Clinical_ProcedureOrder.params.ProviderId);
            utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtAssignee"), Clinical_ProcedureOrder.params.AssigneeName, $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #hfAssignee"), Clinical_ProcedureOrder.params.AssigneeId);
            if (Clinical_ProcedureOrder.params["Problems"] != null) {
                $(Clinical_ProcedureOrder.params["Problems"]).each(function (index, item) {
                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #ulProblemLists td #chk" + item.ProblemId).attr("checked", "checked");
                });
            }
        }
    },
    //Function Name: isDetailAdded
    //Author Name: Humaira Yousaf
    //Created Date: 12-04-2016
    //Description: Checks if detail data is entered
    isDetailAdded: function () {
        var isAdded = true;

        var assignee = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #txtAssignee").val();
        var emptyTests = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #dgvProcedure").find('.dataTables_empty').length;
        var noOfProblems = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #ulProblemLists").find('input:checkbox:checked').length;

        if (assignee == "" && emptyTests == 1 && noOfProblems == 0) {
            isAdded = false;
        }

        return isAdded;
    },

    bindProvider: function () {
        var Ctrl = $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrderDetail #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var onSelect = function (dataItem) {
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id);
            $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #hfProvider").val(dataItem.id);
        }
        var onChange = function (valid) {
            if (!valid)
                $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #hfProvider").val("-1");
            ClinicalProcedureOrderDetail.favoriteListSearch();
        }
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, null, onSelect, onChange);
    },


}