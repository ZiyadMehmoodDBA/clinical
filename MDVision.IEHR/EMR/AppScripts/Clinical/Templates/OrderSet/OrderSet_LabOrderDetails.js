OrderSet_LabOrderDetails = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    LabOrderProblems: [],
    FavListName: "LabOrderDetail",
    checkedProblems: [],
    CPTCodeQA: [],
    ArrayValidation: [],
    AOEsExists: false,
    selectedTestCode: null,
    OrderedTestsCount: 0,
    IsVitalAxisCPT: false,
    selectedTestDescription: null,
    Load: function (params) {
        BackgroundLoaderShow(true);
        //OrderSet_LabOrderDetails.params["TabID"] = 'OrderSet_LabOrderDetails';
        OrderSet_LabOrderDetails.params = params;
        OrderSet_LabOrderDetails.ddlSpecimen = [];
        OrderSet_LabOrderDetails.OrderedTestsCount = 0;
        ClinicalLabOrderDetail.AOEsExists = false;
        OrderSet_LabOrderDetails.IsVitalAxisCPT = false;
        OrderSet_LabOrderDetails.ddlAlternativeSpecimen = [];
        if (OrderSet_LabOrderDetails.params.PanelID != 'pnlOrderSetLabOrderDetails') {
            OrderSet_LabOrderDetails.params.PanelID = OrderSet_LabOrderDetails.params.PanelID + ' #pnlOrderSetLabOrderDetails';
        } else {
            OrderSet_LabOrderDetails.params.PanelID = 'pnlOrderSetLabOrderDetails';
        }

        //Start//31-03-2016//Abid Ali//Logic search LabOrder FavList
        OrderSet_LabOrderDetails.favoriteListSearch();
        //End//31-03-2016//Abid Ali//Logic search LabOrder FavList


        $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        $("#" + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder,#btnResetLabOrder").addClass('disableAll');



        var self = $('#' + OrderSet_LabOrderDetails.params.PanelID);

        //For Data Grid
        var PanelLabGrid = "#" + OrderSet_LabOrderDetails.params.PanelID + " #pnlLab_ResultOrderDetail";
        var LabGridId = "#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail";
        $(LabGridId + " tbody tr").remove();

        OrderSet_LabOrderDetails.EditableGrid = EMRUtility.MakeEditableGrid(PanelLabGrid, LabGridId, OrderSet_LabOrderDetails, "0", false, false, false, false);

        utility.CreateDatePicker(OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id="txtOrderDate"]', function () {
            //on-change callback method
        }, true);

        $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id="txtOrderTime"]').timepicker({
            defaultTime: new Date()
        }).on('changeTime.timepicker', function (e) {
            var h = e.time.hours;
            var m = e.time.minutes;
            var mer = e.time.meridian;
            //convert hours into minutes
            m += h * 60;
            //10:15 = 10h*60m + 15m = 615 min
            if (m < 0)
                $(this).timepicker('setTime', '09:15 AM');
        });

        $('#' + OrderSet_LabOrderDetails.params.PanelID + " #txtLabOrderNo").prop('disabled', true);

        OrderSet_LabOrderDetails.loadAllAutocomplete();

        // Set Title Explicitly if it's passed as Parameter
        if (OrderSet_LabOrderDetails.params.Title != null)
            $("#" + OrderSet_LabOrderDetails.params["PanelID"] + " #headingTitle").text(OrderSet_LabOrderDetails.params.Title);


        if (OrderSet_LabOrderDetails.bIsFirstLoad == true) {
            OrderSet_LabOrderDetails.bIsFirstLoad = true;

            OrderSet_LabOrder.LoadLabs('ddlLabId', OrderSet_LabOrderDetails.params.PanelID).done(function () {

                self.loadDropDowns(true).done(function () {
                    $.when(CacheManager.BindDropDownsByID($('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails #ddlAssigneeId'), 'GetUsersFullName', true, 1)).then(function () {
                        $("#" + OrderSet_LabOrderDetails.params.PanelID + " #ddlAssigneeId option:first").text('- Select -');
                        $.when(OrderSet_LabOrderDetails.loadProblemList()).then(function () {
                            var LabOrderId = OrderSet_LabOrderDetails.params.LabOrderId;
                            LabOrderId = typeof LabOrderId != 'undefined' && LabOrderId > 0 ? LabOrderId : -1
                            OrderSet_LabOrderDetails.loadLabOrder(LabOrderId);
                            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfLabOrderId").val(LabOrderId);
                            OrderSet_LabOrderDetails.validateLabOrderDetail();
                            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails").data('serialize', $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails").serialize());
                        });
                    });
                });

            });


        }

        if (OrderSet_LabOrderDetails.params.mode == "Add") {
            // fill provider if logged in user and provider has same names
            //OrderSet_LabOrderDetails.selectProvider();
            // fill data if order is placed before
            $("#" + OrderSet_LabOrderDetails.params.PanelID + ' #headingTitle').text('Add Lab Order');
        } else {
            $("#" + OrderSet_LabOrderDetails.params.PanelID + ' #headingTitle').text('Edit Lab Order');
        }
        OrderSet_LabOrderDetails.domReadyFunction();
    },

    //Author: Abid Ali
    //Date :  07-03-2016
    //Reason: Clear/Set gurantor/relation values
    setGurantorRelationValues: function (setValues) {

        var guarantorName = OrderSet_LabOrderDetails.params.GuarantorName;
        var guarantorId = OrderSet_LabOrderDetails.params.GuarantorId;
        var relationName = OrderSet_LabOrderDetails.params.RelationName;
        var relationId = OrderSet_LabOrderDetails.params.RelationId;

        if (setValues) {
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #txtGuarantor").val(guarantorName);
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails #hfGuarantorId').val(guarantorId);
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails #txtRadiologyRelationShipId').val(relationName);
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails #hfRalationId').val(relationId);
        }
        else {
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #txtGuarantor").val("");
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails #hfGuarantorId').val("");
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails #txtRadiologyRelationShipId').val("");
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails #hfRalationId').val("");
        }
    },

    //Function Name: selectProvider
    //Author Name: Humaira Yousaf
    //Created Date: 06-04-2016
    //Description: Auto selects provider if same as logged in user
    selectProvider: function (providerId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
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
                                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmOrderSetLabOrderDetails #txtProvider").val(decodeHtmlEntity(item.LastName + ', ' + item.FirstName + ' ' + item.MiddleInitial));
                                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmOrderSetLabOrderDetails #hfProvider").val(decodeHtmlEntity(item.ProviderId));
                                }
                            });
                        }
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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to disable Lab Order Controls
    disableLabOrderControls: function (IsSigned) {
        var detailsDivs = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #divLabOrderInformation,#divTestInformation,#divLabAssociatedProblems");
        var detailsButtons = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder,#btnResetLabOrder");
        var printRequisitionButton = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnPrintOrder");

        var addLabResult = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnAddLabResult");
        var editLabResult = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnEditLabResult");
        if (IsSigned == true) {
            detailsDivs.addClass("disableAll");
            detailsButtons.addClass("hidden");
            // enable only if insurance is selected (ABN)
            var ins = $('#ddlBillingTypeId option').filter(':selected').text();

            printRequisitionButton.removeClass("hidden");
            //Check if labResult already exists then show edit result instead of addd result button
            addLabResult.removeClass("hidden");
            editLabResult.addClass("hidden");
        }
        else {
            detailsDivs.removeClass("disableAll");
            detailsButtons.removeClass("hidden");
            printRequisitionButton.addClass("hidden");
            //Hide if Order is not signed yet
            addLabResult.addClass("hidden");
            editLabResult.addClass("hidden");
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load Lab Order Fav List
    favoriteListSearch: function () {
        Favorite_ProcedureOrder.searchFavoriteList_DBCall("LabOrder", null, 1, 5000).done(function (response) {
            response = JSON.parse(response);
            var $ddl = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlFavoriteListLabOrder');
            $ddl.empty();
            $ddl.append($('<option/>', {
                value: "",
                html: "- Select -"
            }));

            if (response.status != false) {

                var favouriteProcedures = JSON.parse(response.FavoriteListJSON);

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
                //    $ddl.trigger("onchange");

            }
        });

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    selectAllFavoriteListContent: function () {

        $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ulFavoriteListLabOrderContent li').each(function (i, item) {
            $(item).trigger("click");
        });
    },

    //Author: Abid Ali
    //Date :  07-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    loadfavoriteListContent: function (obj, favListIds) {
        var $uL = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ulFavoriteListLabOrderContent');
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            var selectedOptionValue = selectedOption.attr("id");

            var divSelectAllFavorites = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #divSelectAllLabOrderFavorites');
            if (selectedOptionValue > 0) {
                divSelectAllFavorites.removeClass("disableAll");
                OrderSet_LabOrderDetails.favoriteList_CPTSearch(selectedOptionValue);
            }
            else {
                $uL.empty();
                if (divSelectAllFavorites.hasClass("disableAll") == false) {
                    divSelectAllFavorites.addClass("disableAll");
                }
            }

        }
        else {
            if (favListIds != null) {
                //clear the list
                $uL.empty();
                $.each(favListIds, function (index, item) {
                    //load all favList
                    OrderSet_LabOrderDetails.favoriteList_CPTSearch(item, false);
                });
            }
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteList_CPTSearch: function (FavoriteListId, isEmptyUl) {
        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(null, FavoriteListId, 1, 5000).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var objData = JSON.parse(response.FavoriteListCPTJSON);
                var $uL = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ulFavoriteListLabOrderContent');
                isEmptyUl = isEmptyUl == null ? true : false;
                if (isEmptyUl) {
                    $uL.empty();
                }
                $.each(objData, function (i, item) {
                    var mod = item.Modifier != "" ? "<strong> - Mod : " + item.Modifier + "</strong>" : "";
                    if (item.CPTCodeDescription != "") {
                        var onclick = 'OrderSet_LabOrderDetails.BindLabOrderGridItem(\'' + item.Modifier + '\',\'' + item.CPTCode + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + String(item.CPTCodeDescription) + '\')';
                        $uL.append('<li onclick="' + onclick + '">' + item.CPTCodeDescription + mod + '</li>');
                    }
                });
            }

        });
    },


    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Bind Guaranter
    bindGuarantor: function () {
        var shortName = $('#pnlOrderSetLabOrderDetails #txtGuarantor').val();
        utility.GetGuarontorArray(shortName).done(function (response) {

            $('#pnlOrderSetLabOrderDetails #txtGuarantor').autocomplete({
                //source: AllPatients, // pass an array (without a comma)
                autoFocus: true,
                source: response,
                select: function (event, ui) {

                    setTimeout(function () {

                        $("#pnlOrderSetLabOrderDetails #hfLabGuarantorId").val(ui.item.id); // add the selected id
                        if ($("#pnlOrderSetLabOrderDetails #lnkGuarantorEdit").css("display") == "none") {
                            $("#pnlOrderSetLabOrderDetails #lnkGuarantorEdit").css("display", "inline");
                            $("#pnlOrderSetLabOrderDetails #lblGuarantor").css("display", "none");
                        }
                    }, 100);

                }
            });

        });

    },


    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Open Guaranter
    openGuarantor: function () {
        var params = [];
        params["GuarantorId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_LabOrderDetails";
        params["RefCtrl"] = "txtGuarantor";
        LoadActionPan('Patient_Guarantor', params);
    },


    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: open Guaranter
    openGuarantorDetail: function () {
        var params = [];
        params["GuarantorId"] = $('#pnlOrderSetLabOrderDetails #hfLabGuarantorId').val();
        params["mode"] = "Edit";
        params["RefCtrl"] = "txtGuarantor";
        params["ParentCtrl"] = 'OrderSet_LabOrderDetails';
        LoadActionPan('guarantorDetail', params);
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Load auto complete for provider and Facility
    loadAllAutocomplete: function () {
        /*CacheManager.BindCodes('GetProvider', false).done(function (result) {
            $("#frmOrderSetLabOrderDetails #txtProvider").autocomplete({
                autoFocus: true,
                source: Providers,
                select: function (event, ui) {

                    setTimeout(function () {
                        $('#' + OrderSet_LabOrderDetails.params.PanelID + " #txtProvider").attr("Provider", ui.item.id);
                        $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfProvider").val(ui.item.id);
                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            $("#frmOrderSetLabOrderDetails #txtFacility").autocomplete({
                autoFocus: true,
                source: Facilities,
                select: function (event, ui) {

                    setTimeout(function () {
                        $('#' + OrderSet_LabOrderDetails.params.PanelID + " #txtFacility").attr("Facility", ui.item.id);
                        $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfFacility").val(ui.item.id);
                    }, 100);
                }
            });
        });*/
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Enable disable Patient Insurances
    showHidePatientInsurances: function (primaryInsurance, secondaryInsurance, tertiaryInsurance) {
        //Disable DropdownLists if Insurance not exists
        var ddlIds = [];
        if (primaryInsurance.length <= 0)
            ddlIds.push('ddlPrimaryInsuraceId');
        if (secondaryInsurance.length <= 0)
            ddlIds.push('ddlSecondaryInsuraceId');
        if (tertiaryInsurance.length <= 0)
            ddlIds.push('ddlTertiaryInsuraceId');

        //Hide/Show Patient Insurance dropDownLists
        if (ddlIds.length > 0)
            OrderSet_LabOrderDetails.enableDisableDropdownList(ddlIds, true);
        else {
            ddlIds.push('ddlPrimaryInsuraceId');
            ddlIds.push('ddlSecondaryInsuraceId');
            ddlIds.push('ddlTertiaryInsuraceId');
            OrderSet_LabOrderDetails.enableDisableDropdownList(ddlIds, false);
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Enable disable dropdownlists
    enableDisableDropdownList: function (listOfDdlIds, isHide) {

        $.each(listOfDdlIds, function (index, item) {
            if (isHide) {
                $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #' + item).prop('disabled', true);
            }
            else {
                $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #' + item).removeClass('disabled', false);
            }
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Populate patient insurances in ddl
    populateInsuranceDropDownList: function (ddlTypeId, options) {
        var $ddl = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #' + ddlTypeId);

        $ddl.empty();
        $ddl.append($('<option/>', {
            value: "",
            html: "- select -"
        }));
        $.each(options, function (i, item) {
            $ddl.append(
                $('<option/>', {
                    value: item.InsuranceId,
                    html: item.InsurancePlanName
                })
            );
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load problem list
    loadProblemList: function () {
        var dfd = new $.Deferred();
        OrderSet_LabOrderDetails.SearchProblemList().done(function (response) {
            var obj = JSON.parse(response);
            if (obj.status != false) {
                if (obj.ProblemListCount > 0) {
                    var ProblemListLoadJSONData = JSON.parse(obj.ProblemListLoad_JSON);
                    var ProblemListHistoryLoadJSONData = JSON.parse(obj.ProblemListHistoryLoad_JSON);
                    var finalTr = '';
                    var counter = 2;
                    $("#" + OrderSet_LabOrderDetails.params.PanelID + " #ulProblemLists tbody tr").remove();
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

                        var checked = OrderSet_LabOrderDetails.isCheckedProblem('chk' + item.ProblemListId + '') ? "checked" : "";
                        finalTr = finalTr + '<input ' + checked + ' type="checkbox" onclick ="OrderSet_LabOrderDetails.pushProblems(this);" name="' + item.Code + '" value="' + item.ProblemListId + '"  id="chk' + item.ProblemListId + '">';
                        finalTr = finalTr + '   <label class="control-label">' + item.Description + '</label></div></div></td>';

                        if (counter % 2 == 1) {
                            finalTr = finalTr + '</tr>';
                        }
                        counter++;
                        var color = "";
                    });
                    $("#" + OrderSet_LabOrderDetails.params.PanelID + " #ulProblemLists tbody").append(finalTr);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve();
        });
        return dfd.promise();
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
            if (!($.inArray(id, OrderSet_LabOrderDetails.checkedProblems) != -1)) {
                OrderSet_LabOrderDetails.checkedProblems.push(id);
            }
        }
        else {
            //Remove from checked problems
            OrderSet_LabOrderDetails.checkedProblems.splice($.inArray(id, OrderSet_LabOrderDetails.checkedProblems), 1);
        }

    },
    //Author: Abid Ali
    //Date :  15-07-2016
    //Reason: function to check problem in array
    isCheckedProblem: function (problemId) {

        return $.inArray(problemId, OrderSet_LabOrderDetails.checkedProblems) == -1 ? false : true;

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Db_Call for search patient's Insurances
    searchPatientInsurance: function (patientID, PatientInsuranceId) {
        if (patientID == null) {
            patientID = Patient_Insurance.params.patientID;
        }
        if (PatientInsuranceId == null || PatientInsuranceId == "" || PatientInsuranceId <= 0) {
            PatientInsuranceId = "";
        }
        var data = "PatientID=" + patientID + "&PatientInsuranceId=" + PatientInsuranceId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "SEARCH_PATIENT_INSURANCE");
    },

    loadPatientGuarantor: function (patientID) {
        if (patientID == null) {
            patientID = $('#PatientProfile #hfPatientId').val();
        }
        var objData = new Object();
        objData["PatientId"] = patientID;

        objData["commandType"] = "SEARCH_Guarantor";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Edit LabOrder
    LabEdit: function (LabOrderId, event) {
        //if icon is clicked then  popup the window
        OrderSet_LabOrder.LabOrderAddEdit(LabOrderId);
        event.stopPropagation();

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load problem list
    SearchProblemList: function () {
        var objData = new Object();
        objData["OrderSetId"] = OrderSet_LabOrderDetails.params.OrderSetId;
        objData["IsActive"] = '1';
        objData["ProblemListId"] = null;
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 2000;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        objData["NoteId"] = 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProblemList");

    },

    // -------------- Provider ---------------------
    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open provider form
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmOrderSetLabOrderDetails";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_LabOrderDetails";
        LoadActionPan('Admin_Provider', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open provider detail
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#pnlOrderSetLabOrderDetails #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'OrderSet_LabOrderDetails';
        LoadActionPan('providerDetail', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open facility form
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmOrderSetLabOrderDetails";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_LabOrderDetails";
        LoadActionPan('Admin_Facility', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open facility detail form
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#pnlOrderSetLabOrderDetails #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'OrderSet_LabOrderDetails';
        LoadActionPan('facilityDetail', params);
    },

    HideProviderLink: function () {
        $('#pnlOrderSetLabOrderDetails #txtProvider').attr("ProviderId", "-1");
        $('#pnlOrderSetLabOrderDetails #hfProvider').val("-1");
        $("#pnlOrderSetLabOrderDetails #lnkProviderEdit").css("display", "none");
        $("#pnlOrderSetLabOrderDetails #lblProvider").css("display", "inline");
    },
    // -------------- End Provider -----------------

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to apply bootstrap validations
    validateLabOrderDetail: function () {
        $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails")
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   LabId: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //Provider: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   //Facility: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (e.type == "success") {
                var $form = $(e.target);
                var $button = $form.data('bootstrapValidator').getSubmitButton();
                switch ($button.attr('id')) {
                    case 'btnSaveOrder':
                        //var isProblemAdded = OrderSet_LabOrderDetails.problemAdded();
                        //if (isProblemAdded == true) {
                        OrderSet_LabOrderDetails.LabOrderSave('save');
                        //}
                        //else {
                        //    utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}
                        break;
                    case 'btnSignOrder':
                        //var isProblemAdded = OrderSet_LabOrderDetails.problemAdded();
                        //if (isProblemAdded == true) {
                        OrderSet_LabOrderDetails.LabOrderSave('signorder');
                        OrderSet_LabOrderDetails.disableLabOrderControls(true);
                        //}
                        //else {
                        //    utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}
                        break;
                    case 'btnPrintOrder':
                        utility.myConfirm('Would you like to print the Specimen Label for this order?', function () {
                            OrderSet_LabOrderDetails.printLabOrder(true);
                            OrderSet_LabOrderDetails.disableLabOrderControls(true);
                        }, function () {
                            OrderSet_LabOrderDetails.printLabOrder(false);
                            OrderSet_LabOrderDetails.disableLabOrderControls(true);
                        },
                       'Specimen Label Printing');

                        break;

                    case 'btnSignPrintOrder':
                        //var isProblemAdded = OrderSet_LabOrderDetails.problemAdded();
                        //if (isProblemAdded == true) {
                        utility.myConfirm('Would you like to print the Specimen Label for this order?', function () {
                            OrderSet_LabOrderDetails.LabOrderSave('signprintorder', true);
                            OrderSet_LabOrderDetails.disableLabOrderControls(true);
                        }, function () {
                            OrderSet_LabOrderDetails.LabOrderSave('signprintorder', false);
                            OrderSet_LabOrderDetails.disableLabOrderControls(true);
                        },
                        'Specimen Label Printing');
                        //}
                        //else {
                        //    utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}
                        break;
                    case 'btnAddLabResult':
                        OrderSet_LabOrder.LabResultAddEdit(-1, OrderSet_LabOrderDetails.params.LabOrderId, true, "AddResult")
                        break;
                    case 'btnEditLabResult':
                        break;
                    default:
                        //var isProblemAdded = OrderSet_LabOrderDetails.problemAdded();
                        //if (isProblemAdded == true) {
                        OrderSet_LabOrderDetails.LabOrderSave('save');
                        //}
                        //else {
                        //    utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}
                        break;
                }
            }
            e.type = "";
        });

    },


    EnableDisableTestSearch: function (ddlLab) {

        if ($(ddlLab).val() != '') {
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #txtLabCPTCode').removeClass('disableAll');
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #favSectionDiv').removeClass('disableAll');
        }
        else {
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #txtLabCPTCode').addClass('disableAll');
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #favSectionDiv').removeClass('disableAll');
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Binding numpad with height, weight, systolic and diastolic fields
    domReadyFunction: function () {


        $(document).ready(function () {

            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlBillingTypeId').on('change', function () {

                if ($(this).val() == 3) {
                    //   OrderSet_LabOrderDetails.billingtype = 3;
                    OrderSet_LabOrderDetails.enableDisableInsurances(true);

                    var selectedVal1 = '';
                    var selectedVal2 = '';
                    var selectedVal3 = '';
                    $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "1") {
                            selectedVal1 = $(this).val();

                        }

                    });
                    $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId').val(selectedVal1);

                    $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "2") {
                            selectedVal2 = $(this).val();
                        }
                    });
                    $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId').val(selectedVal2);

                    $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "3") {
                            selectedVal3 = $(this).val();
                        }
                    });
                    $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId').val(selectedVal3);

                    OrderSet_LabOrderDetails.setGurantorRelationValues(true);
                }
                else {

                    OrderSet_LabOrderDetails.enableDisableInsurances(false);

                    $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId').val('0');
                    $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId').val('0');
                    $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId').val('0');

                    OrderSet_LabOrderDetails.setGurantorRelationValues(false);
                }

            });

            $('.toggleHorSmallLeft section').click(function (e) {
                $(this).parent().toggleClass("toggled");
                OrderSet_LabOrderDetails.toggleHorSmallLeftIcon($(this));

            });
        });
        $(function () {
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails [data-plugin-keyboard-numpad]').keyboard({
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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To Disable inurances
    enableDisableInsurances: function (enable) {
        if (enable) {
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', false);
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', false);
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', false);

            OrderSet_LabOrderDetails.setGurantorRelationValues(true);
        }
        else {
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', true);
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', true);
            $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', true);

            OrderSet_LabOrderDetails.setGurantorRelationValues(true);
        }
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

    //Author: Abid Ali
    //Date :  31-03-2016
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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To validate BSA
    calculateBSA: function (objWeight, objHeightInFeet, TargetCtrl) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #txtBSA");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var weightInKG = OrderSet_LabOrderDetails.convertWeight(weightInLbs);
        var heightIn_cm = OrderSet_LabOrderDetails.convertHeightTo_cm(heightInFeet);
        var result = 0.007184 * Math.pow(heightIn_cm, 0.725) * Math.pow(weightInKG, 0.425);
        var BSA = result.toFixed(2)
        if (WeightInLbs != "" && HeightInFeet != "")
            CtrlName.val(BSA);
        else
            CtrlName.val('');
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To validate Weight
    convertWeight: function (pounds) {
        return pounds / 2.20462262185;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To validate height
    convertHeightTo_cm: function (feet) {
        return feet * 12 * 2.54;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To calculate BMI on the basis of height and weight
    calculateBMI: function (objWeight, objHeightInFeet, TargetCtrl) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #txtBMI");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var heightInInches = OrderSet_LabOrderDetails.convertHeightInches(heightInFeet);

        var result = (weightInLbs / (heightInInches * heightInInches)) * 703;
        var BMI = result.toFixed(2);
        if (WeightInLbs != "" && HeightInFeet != "" && BMI != "Infinity")
            CtrlName.val(BMI);
        else
            CtrlName.val('');
    },

    //Author: Abid Ali
    //Date :  31-03-2016
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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Loading ICD Codes for Problem List AutoComplete
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "OrderSet_LabOrderDetails", null, false);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
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
        if (OrderSet_LabOrderDetails.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'OrderSet_LabOrderDetails';
        }

        else {
            params["ParentCtrl"] = OrderSet_LabOrderDetails.params["TabID"];

        }
        params["PanelID"] = OrderSet_LabOrderDetails.params["PanelID"];

        params["ActionPanContainer"] = OrderSet_LabOrderDetails.params["ActionPanContainer"];
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (OrderSet_LabOrderDetails.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params, OrderSet_LabOrderDetails.params.PanelID);
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: deleting Lis of Problem list
    deleteProblemList: function (obj, ev) {
        ev.stopPropagation();
        var problemListId = $(obj).attr('id');
        if (problemListId < 0) {
            $(obj).remove();
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: deleting Lis of Allergies list
    deleteAllergy: function (obj, ev) {
        ev.stopPropagation();
        var allergyId = $(obj).attr('id');
        if (allergyId < 0) {
            $(obj).remove();
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: deleting Lis of Medications list
    deleteMedication: function (obj, ev) {
        ev.stopPropagation();
        var medicationId = $(obj).attr('id');
        if (medicationId < 0) {
            $(obj).remove();
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: to show delete icon on hover
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: to hide delete icon on mouse leave
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },

    UnLoad: function (caller) {
        OrderSet_LabOrderDetails.checkedProblems = [];
        ClinicalLabOrderDetail.CPTCodeQA = [];
        var form = '#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails";
        var saveButtonisHidden = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder").hasClass("hidden");

        if (caller == 'saveExit' || saveButtonisHidden == true) {
            if (OrderSet_LabOrderDetails.params["ParentCtrl"] == "OrderSet_LabOrder") {
                UnloadActionPan(OrderSet_LabOrderDetails.params["ParentCtrl"], "OrderSet_LabOrderDetails", null, OrderSet_LabOrderDetails.params["ParentCtrlPanelID"]);
            }
            else {
                UnloadActionPan(OrderSet_LabOrderDetails.params["ParentCtrl"], "OrderSet_LabOrderDetails");

            }
        }
        else {
            if (OrderSet_LabOrderDetails.params["ParentCtrl"] == "OrderSet_LabOrder") {
                UnloadActionPan(OrderSet_LabOrderDetails.params["ParentCtrl"], "OrderSet_LabOrderDetails", null, OrderSet_LabOrderDetails.params["ParentCtrlPanelID"]);
            }
            else {
                UnloadActionPan(OrderSet_LabOrderDetails.params["ParentCtrl"], "OrderSet_LabOrderDetails");

            }
        }
        OrderSet_ProblemListGrid.ProblemListsSearch();
        $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Saves LabOrder
    LabOrderSave: function (sender, includeSpecimen, UnLoadOrNot) {

        if (!$('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder").hasClass('disableAll')) {
            var dfd = $.Deferred();
            if (ClinicalLabOrderDetail.ArrayValidation.length > 0) {

                var Test = ClinicalLabOrderDetail.ArrayValidation[0];
                utility.DisplayMessages("AOE (Asked Order Entry) information is missing " + Test.Test, 3);
                return;
            }
            else if (ClinicalLabOrderDetailAOE.UnAnsweredSelections.length > 0) { // Multiselect unanswered
                utility.DisplayMessages("AOE (Asked Order Entry) information is missing ", 3);
                return;
            }
            var LabOrderId = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfLabOrderId").val() != "" ? $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfLabOrderId").val() : "-1";
            if (parseInt(LabOrderId) > 0) {
                OrderSet_LabOrderDetails.params.mode = "Edit";
            }
            else {
                OrderSet_LabOrderDetails.params.mode = "Add";
            }
            if (OrderSet_LabOrderDetails.OrderedTestsCount == 0) {
                utility.DisplayMessages("Please add some tests first", 2);
                return;
            }
            var self = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails");

            var title = self.find("#txtLabOrderTitle").val();

            var mainErrorMessage = "";

            if ($('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails #ddlLabOrderAgeCondition').val() != "") {
                var ageConditionMsg = OrderSet_LabOrderDetails.isAgeConditionValid();
                if (ageConditionMsg != "") {
                    mainErrorMessage = ageConditionMsg;
                }
            }

            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);

            var LabOrderProblemList = [];
            $(self).find("#ulProblemLists td").each(function (index, item) {
                if ($(this).find("input:checkbox").prop("checked")) {
                    var objProblem = {
                        ProblemId: $(this).find("input:checkbox").val(),
                        Description: $(this).text()
                    };
                    objProblem.Description = (objProblem.Description).replace(/%/g, "%25");
                    LabOrderProblemList.push(objProblem);
                }
            });
            objData["LabOrderProblemList"] = LabOrderProblemList;

            //Start 06-04-2016 Humaira Yousaf for problems list
            OrderSet_LabOrderDetails.LabOrderProblems = LabOrderProblemList;
            //End 06-04-2016 Humaira Yousaf for problems list

            if (sender == 'signorder' || sender == 'signprintorder')
                objData["Status"] = 'Signed';
            else if (sender == 'save')
                objData["Status"] = 'Pending';

            if (ClinicalLabOrderDetail.AOEsExists) {
                if (ClinicalLabOrderDetail.CPTCodeQA.length == 0 && ClinicalLabOrderDetail.params.mode != "Edit") {
                    utility.DisplayMessages("AOE (Asked Order Entry) information is missing ", 3);
                    return;
                }
            }
            if (ClinicalLabOrderDetail.CPTCodeQA.length > 0) {
                objData['CPTCodeQuestionsAnswers'] = ClinicalLabOrderDetail.CPTCodeQA;
            }


            myJSON = JSON.stringify(objData);

            //------------------------------------------------------------
            var LabTestIds = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr:not([id*=Child]").map(function () {
                return this.id.replace("id", "");
            }).get().join(',');
            $("#" + OrderSet_LabOrderDetails.params.PanelID + " #pnlLab_ResultOrderDetail #hfLabTestIds").val(LabTestIds);
            var selfLabTest = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #pnlLab_ResultOrderDetail");
            var myJSONLabTest = selfLabTest.getMyJSON();

            var objRad = new Object();
            objRad["LabTestIds"] = LabTestIds;
            JSONToSave = utility.MergeJSON(myJSON, myJSONLabTest);

            //Start//01-04-2016//Abid Ali//To save child Rows Data in JSON//
            $("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr:not([id*=Child]").each(function (index, item) {
                var childExists = OrderSet_LabOrderDetails.EditableGrid.datatable.row(index).child();

                if (typeof childExists != 'undefined') {
                    var childRow = OrderSet_LabOrderDetails.EditableGrid.datatable.row(index).child().get();
                    if ($(childRow).length > 0) {

                        var childRowDataJson = $(childRow).getMyJSON();
                        JSONToSave = utility.MergeJSON(JSONToSave, childRowDataJson)
                    }
                }
            });
            //End//01-04-2016//Abid Ali//To save child Rows Data in JSON//


            var data = JSON.stringify(objRad);
            JSONToSave = utility.MergeJSON(data, JSONToSave);
            JSONToSave = decodeURIComponent(JSONToSave);
            //--------------------------------------------------------------

            OrderSet_LabOrderDetails.cacheLabOrderData();
            // var isSavedSuccessfully = false;
            if (OrderSet_LabOrderDetails.params.mode == "Add") {

                AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        OrderSet_LabOrderDetails.saveLabOrder(JSONToSave).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                //Save/update favList Status
                                var isFavListOpened = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #favSectionDiv").hasClass("toggled");
                                EMRUtility.insertUpdateFavListStatus(OrderSet_LabOrderDetails.FavListName, isFavListOpened);

                                if (OrderSet_LabOrderDetails.params.ParentCtrl == "clinicalTabProgressNote") {
                                    OrderSet_LabOrderDetails.getLatestLabOrderByPatientId(true);
                                }

                                $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfLabOrderId").val(response.radiologicalOrderId);

                                $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfOrderNumber").val(response.orderNo);

                                utility.DisplayMessages(response.message, 1);

                                OrderSet_LabOrder.LabOrderSearch(null, null, null, "LabOrderDetail");
                                OrderSet_LabOrder.LabResultsSearch(null, null, null, "LabResultDetail");
                                // Download HL7 Lab Order Message
                                if (sender == 'signprintorder' || sender == 'signorder') {
                                    var uri = '';//'data:application/vnd.ms-excel;base64,';
                                    if (response.orderNo != "") {
                                        download(uri + response.HL7MessageContent, response.orderNo + "LabOrder.txt", "application/octet-stream");
                                    } else {
                                        download(uri + response.HL7MessageContent, response.radiologicalOrderId + "LabOrder.txt", "application/octet-stream");
                                    }
                                }
                                if (sender == 'signprintorder') {
                                    OrderSet_LabOrderDetails.params.LabOrderId = response.radiologicalOrderId;

                                    //includeSecimenLabel
                                    OrderSet_LabOrderDetails.printLabOrder(includeSpecimen);
                                }
                                else {
                                    if (UnLoadOrNot != false) {
                                        OrderSet_LabOrderDetails.UnLoad('saveExit');
                                    }

                                    $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                                }
                                Clinical_OrderSetDetails.LabOrderSearch(null, null, null, null, "Pending");
                                dfd.resolve();
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
            else if (OrderSet_LabOrderDetails.params.mode == "Edit") {

                AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        OrderSet_LabOrderDetails.updateLabOrder(JSONToSave, LabOrderId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                //Save/update favList Status
                                var isFavListOpened = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #favSectionDiv").hasClass("toggled");
                                EMRUtility.insertUpdateFavListStatus(OrderSet_LabOrderDetails.FavListName, isFavListOpened);

                                if (OrderSet_LabOrderDetails.params.ParentCtrl == "clinicalTabProgressNote") {
                                    OrderSet_LabOrderDetails.getLatestLabOrderByPatientId();
                                }

                                $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfLabOrderId").val(response.radiologicalOrderId);
                                $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfOrderNumber").val(response.orderNo);
                                utility.DisplayMessages(response.message, 1);

                                OrderSet_LabOrder.LabOrderSearch(null, null, null, "LabOrderDetail");
                                OrderSet_LabOrder.LabResultsSearch(null, null, null, "LabResultDetail");

                                if (sender == 'signprintorder') {
                                    OrderSet_LabOrderDetails.printLabOrder(includeSpecimen);
                                    //includeSecimenLabel
                                }
                                else {
                                    if (UnLoadOrNot != false) {
                                        OrderSet_LabOrderDetails.UnLoad('saveExit');
                                    }
                                    $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                                }
                                ////Start 31-05-2016 Humaira Yousaf for favorite list toggle
                                //if (response.labFavList == true)
                                //    SetGlobalAppData('LabFavListOpened', 'True');
                                //else
                                //    SetGlobalAppData('LabFavListOpened', 'False');
                                ////End 11-04-2016 Humaira Yousaf for favorite list toggle
                                Clinical_OrderSetDetails.LabOrderSearch(null, null, null, null, "Pending");
                                dfd.resolve();
                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                            }
                            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails").data('serialize', $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails").serialize());
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });

            }
        }
    },

    //Function Name: cacheProcedureOrderData
    //Author Name: Humaira Yousaf
    //Created Date: 06-04-2016
    //Description: Saves procedure order detail data
    cacheLabOrderData: function () {
        //OrderSet_LabOrder.params["ProviderName"] = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #txtProvider").val();
        //OrderSet_LabOrder.params["ProviderId"] = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #hfProvider").val();

        OrderSet_LabOrder.params["AssigneeName"] = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ddlAssigneeId").val();
        OrderSet_LabOrder.params["AssigneeId"] = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #hfAssignee").val();

        OrderSet_LabOrder.params["Problems"] = OrderSet_LabOrderDetails.LabOrderProblems;

        OrderSet_LabOrder.params["LabId"] = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ddlLabId").val();
        OrderSet_LabOrder.params["BillingTypeId"] = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ddlBillingTypeId").val();

        //OrderSet_LabOrder.params["FacilityName"] = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #txtFacility").val();
        //OrderSet_LabOrder.params["FacilityId"] = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #hfFacility").val();


        OrderSet_LabOrder.params["CurrentPatientId"] = $('#PatientProfile #hfPatientId').val();
        OrderSet_LabOrder.params["hasData"] = true;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To check whether Lab order exists or not
    checkLabOrderExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_LabOrders').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="LabOrdersComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<OrderSet_LabOrders title="Lab Orders"  id="clinicalMenu_Orders_Lab" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Lab\',\'clinicalMenu_Orders_Lab\',' + Clinical_ProgressNote.params.NotesId + ');" title="Lab Orders">Lab Orders</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Lab Orders\',\'clinicalMenu_Orders_Lab\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</OrderSet_LabOrders> </header></li>');
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
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Saves LabOrder
    //Params: LabOrderData
    saveLabOrder: function (LabOrderData) {
        var objData = JSON.parse(LabOrderData);
        objData["OrderSetId"] = OrderSet_LabOrderDetails.params.OrderSetId;
        objData["commandType"] = "save_LabOrder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },
    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To show LabOrder Search in Popup
    openLabOrderAlert: function () {

        if ($(" #mainForm  li#LabOrderAlert span").text() != '' && $('#PatientProfile #hfPatientId').val() != '') {
            BackgroundLoaderShow(true);
            var params = [];


            params["FromAdmin"] = 0;
            //   params["StartupScreen"] = "message";
            LoadActionPan("OrderSet_LabOrder", params);
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Loads LabOrder data
    loadLabOrder: function (LabOrderId) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (OrderSet_LabOrderDetails.params.mode == "Edit") {
                    $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfLabOrderId").val(LabOrderId);
                    var self = $('#' + OrderSet_LabOrderDetails.params.PanelID);

                    OrderSet_LabOrderDetails.fillLabOrder(LabOrderId).done(function (response) {
                        if (response != "") {
                            var data = JSON.parse(response);
                            if (data.status != false) {

                                var detailJSON = JSON.parse(data.LabFill_JSON);
                                if (detailJSON.bResultAcknowledged == "True") {
                                    $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnAddLabResult").text("View Result");
                                }
                                else if (detailJSON.bResultExists == "True") {
                                    $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnAddLabResult").text("Edit Result");
                                }
                                else {
                                    $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnAddLabResult").text("Add Result");
                                }

                                var LabOrderDetail = JSON.parse(data.LabOderFill_JSON);

                                LabOrderDetail = LabOrderDetail[0];
                                if (LabOrderDetail != null && LabOrderDetail.OrderDate != null && LabOrderDetail.OrderDate != '') {
                                    function pad(s) { return (s < 10) ? '0' + s : s; }
                                    var d = new Date(LabOrderDetail.OrderDate);
                                    LabOrderDetail.OrderDate = [pad(d.getMonth() + 1), pad(d.getDate()), d.getFullYear()].join('/');
                                }

                                //  setTimeout(function () {
                                utility.bindMyJSONByName(true, LabOrderDetail, false, self).done(function () {

                                    $('#' + OrderSet_LabOrderDetails.params.PanelID + " #txtLabOrderNo").val(LabOrderDetail.OrderNo);
                                    $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfOrderNumber").val(LabOrderDetail.OrderNo);
                                    $('#' + OrderSet_LabOrderDetails.params.PanelID + " #ddlLabId").val(LabOrderDetail.LabId);

                                    OrderSet_LabOrderDetails.EnableDisableTestSearch($('#' + OrderSet_LabOrderDetails.params.PanelID + " #ddlLabId"));

                                    var billingType = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlBillingTypeId option:selected').text();
                                    if (billingType.indexOf('Insurance') >= 0) {
                                        OrderSet_LabOrderDetails.enableDisableInsurances(true);

                                    }
                                    else {
                                        OrderSet_LabOrderDetails.enableDisableInsurances(false);
                                    }
                                    OrderSet_LabOrderDetails.LabOrderTestGridLoad(response);

                                });
                                //   }, 1000);



                                //Load Problems
                                if (data.LabOrderProblems_JSON != null) {

                                    data.LabOrderProblems_JSON = JSON.parse(data.LabOrderProblems_JSON);

                                    for (var index in data.LabOrderProblems_JSON) {
                                        $('#' + OrderSet_LabOrderDetails.params.PanelID + " #ulProblemLists td #chk" + data.LabOrderProblems_JSON[index].ProblemId).attr("checked", "checked");
                                    }
                                }

                                if (OrderSet_LabOrderDetails.params.mode == "Edit") {
                                    if (LabOrderDetail.Status.toLowerCase() == "signed") {
                                        OrderSet_LabOrderDetails.disableLabOrderControls(true);
                                    }
                                    else {
                                        OrderSet_LabOrderDetails.disableLabOrderControls(false);
                                    }
                                }
                                else {
                                    OrderSet_LabOrderDetails.disableLabOrderControls(false);
                                }

                            }
                        }
                        if (OrderSet_LabOrderDetails.params.mode == "Add") {
                            OrderSet_LabOrderDetails.disableLabOrderControls(false);
                        }
                    });
                }
                //else {
                //    OrderSet_LabOrderDetails.populateLabOrderSavedData();
                //}
                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
                if (EMRUtility.getFavListStatus(OrderSet_LabOrderDetails.FavListName))
                    $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #favSectionDiv").removeClass("toggled");
                else
                    $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #favSectionDiv").addClass("toggled");
                //End 10-06-2016 Abid Ali for favorite list setting for all favLists

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    //Function Name: populateLabOrderSavedData
    //Author Name: Humaira Yousaf
    //Created Date: 06-04-2016
    //Description: Gets Lab order detail data
    populateLabOrderSavedData: function () {
        if (OrderSet_LabOrder.params["hasData"] == true) {
            //$('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #txtProvider").val(OrderSet_LabOrder.params["ProviderName"]);
            //$('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #hfProvider").val(OrderSet_LabOrder.params["ProviderId"]);

            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ddlAssigneeId").val(OrderSet_LabOrder.params["AssigneeName"]);
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #hfAssignee").val(OrderSet_LabOrder.params["AssigneeId"]);

            //if (OrderSet_LabOrder.params["Problems"] != null) {
            //    $(OrderSet_LabOrder.params["Problems"]).each(function (index, item) {
            //        $('#' + OrderSet_LabOrderDetails.params.PanelID + " #ulProblemLists td #chk" + item.ProblemId).attr("checked", "checked");
            //    });
            //}

            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ddlLabId").val(OrderSet_LabOrder.params["LabId"]);
            OrderSet_LabOrderDetails.EnableDisableTestSearch($('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ddlLabId"));

            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ddlBillingTypeId").val(OrderSet_LabOrder.params["BillingTypeId"]);

            //Start//Abid Ali/ For bug# EMR-751
            var billingType = $('#' + OrderSet_LabOrderDetails.params.PanelID + '  #frmOrderSetLabOrderDetails #ddlBillingTypeId option:selected').text();
            if (billingType.indexOf('Insurance') >= 0) {
                OrderSet_LabOrderDetails.enableDisableInsurances(true);
            }
            else {
                OrderSet_LabOrderDetails.enableDisableInsurances(false);
            }
            //End//Abid Ali/ For bug# EMR-751

            //$('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #txtFacility").val(OrderSet_LabOrder.params["FacilityName"]);
            //$('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #hfFacility").val(OrderSet_LabOrder.params["FacilityId"]);




        }
    },
    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Fills LabOrder
    //Params: LabOrderId
    fillLabOrder: function (LabOrderId) {

        var objData = {};
        objData["commandType"] = "fill_LabOrder";
        objData["LabOrderId"] = LabOrderId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Updates LabOrder
    //Params: LabOrderData, LabOrderId
    updateLabOrder: function (LabOrderData, LabOrderId) {
        var objData = JSON.parse(LabOrderData);
        objData["LabOrderId"] = LabOrderId;
        objData["commandType"] = "save_LabOrder";
        objData["OrderSetId"] = OrderSet_LabOrderDetails.params.OrderSetId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Shows respective cotrols based on selected age condition
    addAgeConditionValues: function () {
        var ageCondition = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ddlLabOrderAgeCondition").val();
        //Start 1-03-2016 Humaira Yousaf
        if (ageCondition == "") {
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ageConditionRange").addClass("hidden");
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ageConditionValue").addClass("hidden");
            var ageToValue = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderAgeValue]').val('');
            var ageToValue = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderAgeFrom]').val('');
            var ageToValue = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderAgeTo]').val('');
        }
            //End 1-03-2016 Humaira Yousaf
        else if (ageCondition == "6") {
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ageConditionRange").removeClass("hidden");
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ageConditionValue").addClass("hidden");
        }
        else {
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ageConditionValue").removeClass("hidden");
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ageConditionRange").addClass("hidden");
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Validates reminder
    isReminderValid: function () {
        var Message = "";

        var reminder = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderReminderLength]');
        var stayLength = $(reminder).val();
        var ddlVal = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails #ddlLabOrderReminderPeriod').val();
        if (stayLength != null && stayLength != '') {
            if (ddlVal == null || ddlVal == '') {
                $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails #ddlLabOrderReminderPeriod').focus();
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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: HideShow Vitals,problems list etc divs on editing
    hideShowDataDivs: function () {
        var selectedValue = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ddlLabOrderRuleType option:selected");
        var selected = [];
        $(selectedValue).each(function (index, val) {
            selected.push($(this).text());
        });
        var unSelect = '';
        $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails .comList").addClass('hidden');
        $(selected).each(function (i, item) {
            var sectionName = item;
            unSelect += sectionName + ',';
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #dv" + item.replace(/\s/g, '')).removeClass('hidden');
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: to show LabOrder Alert on Dashboard
    showLabOrderAlert: function (triggerLocation) {

        OrderSet_LabOrderDetails.showLabOrderAlertDBCall(triggerLocation).done(function (response) {
            response = JSON.parse(response);
            //Start//09-03-2016//Ahmad Raza//setting hiddenField values
            $(" #mainForm  li#LabOrderAlert input").val('');
            $(" #mainForm  li#LabOrderAlert input").val(function (i, val) {
                return val + (val ? ', ' : '') + response.LabOrderIDs;
            });
            //End//09-03-2016//Ahmad Raza//setting hiddenField values
            if (response.status != false) {

                if (response.alertCount > 0) {
                    $(" #mainForm  li#LabOrderAlert span").text(response.alertCount);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: DBCall to show LabOrder Alert on Dashboard
    showLabOrderAlertDBCall: function (triggerLocation) {
        var objData = new Object();
        if (triggerLocation == 'FaceSheet') {
            objData["LabOrderTriggerLocation"] = '1';
        }
        else if (triggerLocation == 'Notes') {
            objData["LabOrderTriggerLocation"] = '2';
        }
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "show_LabOrder_alert";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Loads LabOrder Medications
    //Params: LabOrderMedications
    loadLabOrderMedications: function (LabOrderMedications) {
        $(LabOrderMedications).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li id=" + item.drugId + " onclick='' \"><div><a href='#'>" + item.medicationName + "<span class='removeIconListHover' onclick='OrderSet_LabOrderDetails.deleteMedication($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li id=" + item.drugId + " onclick='' \"><div><select id='ddlMedications" + item.drugId + "' name = 'LabOrderMedications" + item.drugId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.medicationName + "<span class='removeIconListHover' onclick='OrderSet_LabOrderDetails.deleteMedication($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ulLabOrderMedications").append(li);

            if (index != 0) {
                if (item.medicationOperator == 'AND') {
                    $($('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ulLabOrderMedications li#" + item.drugId).find("#ddlMedications" + item.drugId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ulLabOrderMedications li#" + item.drugId).find("#ddlMedications" + item.drugId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #txtLabOrderMedications").val('');
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Loads LabOrder Allergies
    //Params: LabOrderAllergies
    loadLabOrderAllergies: function (LabOrderAllergies) {
        $(LabOrderAllergies).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li id=" + item.AllergyId + " onclick='' \"><div ><a href='#'>" + item.Allergen + "<span class='removeIconListHover' onclick='OrderSet_LabOrderDetails.deleteAllergy($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li id=" + item.AllergyId + " onclick='' \"><div><select id='ddlAllergies" + item.AllergyId + "' name = 'LabOrderMedications" + item.AllergyId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.Allergen + "<span class='removeIconListHover' onclick='OrderSet_LabOrderDetails.deleteAllergy($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ulLabOrderAllergies").append(li);

            if (index != 0) {
                if (item.AllergyOperator == 'AND') {
                    $($('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ulLabOrderAllergies li#" + item.AllergyId).find("#ddlAllergies" + item.AllergyId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ulLabOrderAllergies li#" + item.AllergyId).find("#ddlAllergies" + item.AllergyId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #txtLabOrderAllergies").val('');
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Params: LabOrderProblems
    loadLabOrderProblems: function (LabOrderProblems) {
        $(LabOrderProblems).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li name='" + item.Problem + "' onclick='' \"><div ><a href='#'>" + item.Problem + "<span class='removeIconListHover' onclick='OrderSet_LabOrderDetails.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li name='" + item.Problem + "' onclick='' \"><div><select id='ddlProblemList" + item.Problem + "' name = 'LabOrderProblemList" + item.ProblemId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.Problem + "<span class='removeIconListHover' onclick='OrderSet_LabOrderDetails.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ulLabOrderProblemList").append(li);

            if (index != 0) {
                if (item.ProblemOperator == 'AND') {
                    $($('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ulLabOrderProblemList li#" + item.ProblemId).find("#ddlProblemList" + item.ProblemId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ulLabOrderProblemList li#" + item.ProblemId).find("#ddlProblemList" + item.ProblemId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #txtLabOrderProblemList").val('');
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Validates Age Condition
    isAgeConditionValid: function () {
        var Message = "";
        var ddlAgeConditionVal = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails #ddlLabOrderAgeCondition').val();

        if (ddlAgeConditionVal == '6') {

            var ageFromValue = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderAgeFrom]').val();
            var ageToValue = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderAgeTo]').val();

            if ((ageFromValue == null || ageFromValue == '') && (ageToValue == null || ageToValue == '')) {
                $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderAgeFrom]').focus();
                $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderAgeTo]').focus();
                Message = "Please enter From Age and To Age.";

            }
            else {
                if (ageFromValue != null || ageFromValue != '') {
                    if (ageToValue == null || ageToValue == '') {
                        $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderAgeTo]').focus();
                        Message = "Please enter To Age.";
                    }
                }

                if (ageToValue != null || ageToValue != '') {
                    if (ageFromValue == null || ageFromValue == '') {
                        $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderAgeFrom]').focus();
                        Message = "Please enter From Age.";
                    }
                }
            }

        }
        else {
            var ageValue = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderAgeValue]').val();
            if (ageValue == null || ageValue == '') {
                $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #frmOrderSetLabOrderDetails input[id*=txtLabOrderAgeValue]').focus();
                Message = "Please enter Age.";
            }
        }

        return Message;
    },



    bindAutoComplete: function (element) {
        //var hiddenCrtl = $(element);
        // utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "OrderSet_LabOrderDetails", null, true);

        var CodeSystemType = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlLabId option:selected').attr('CodeSystem');
        var labId = $('#' + OrderSet_LabOrderDetails.params.PanelID + ' #ddlLabId').val();
        EMRUtility.BindLOINCCodes(element, "OrderSet_LabOrderDetails", labId, '', CodeSystemType);

    },

    openCPTCode: function () {
        var params = [];
        //params["FromAdmin"] = "0";
        //params["ParentCtrl"] = "OrderSet_LabOrderDetails";
        //params["RefHiddenCtrl"] = "hfCPTCode";
        //params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = OrderSet_LabOrderDetails.params.PanelID;
        //LoadActionPan('Admin_IMOCPT', params, OrderSet_LabOrderDetails.params.PanelID);
        params["FromAdmin"] = OrderSet_LabOrderDetails.params["FromAdmin"];
        params["ParentCtrl"] = 'OrderSet_LabOrderDetails';

        LoadActionPan('Clinical_LOINC', params, OrderSet_LabOrderDetails.params.PanelID);
    },


    //Called from LOINIC Control to pass code and description as Json obj
    pushLOINCAsCpt: function (JsonObj) {

        var observation = JsonObj["Observation"];
        var LOINCCOde = JsonObj['LOINICCODE'];
        var LOINCDescription = JsonObj['LOINICDescription'];
        var SampleStorage = JsonObj['SampleStorage'];

        if (LOINCDescription.trim().indexOf('-') == 0) {
            LOINCDescription = LOINCDescription.trim().substr(1).trim();
        }

        OrderSet_LabOrderDetails.BindLabOrderGridItem("", LOINCCOde, LOINCDescription, LOINCDescription, SampleStorage);


    },

    BindLabOrderGridItem: function (modifier, cptCode, procedureDescription, cptDescription, SampleStorage) {

        var labName = $('#frmOrderSetLabOrderDetails #ddlLabId option:selected').text();

        if (labName == "VitalAxis") {
            if (OrderSet_LabOrderDetails.OrderedTestsCount >= 1) {// VitalAxis only accepts a single test in order

                if ((cptCode != '84154' || cptCode != '84403' || cptCode != '84153' || cptCode != '84439' || cptCode != '84443') && (OrderSet_LabOrderDetails.IsVitalAxisCPT == false)) {
                    utility.DisplayMessages("The lab selected currently processes only one Test per Order.", 2);
                    return;
                }
            }
            if ((cptCode == '84154' || cptCode == '84403' || cptCode == '84153' || cptCode == '84439' || cptCode == '84443')) {
                OrderSet_LabOrderDetails.IsVitalAxisCPT = true;
            }
            else {
                OrderSet_LabOrderDetails.IsVitalAxisCPT = false;
            }
            if (!OrderSet_LabOrderDetails.IsVitalAxisCPT && OrderSet_LabOrderDetails.OrderedTestsCount >= 1) {
                utility.DisplayMessages("This combination of Tests cannot be ordered to VitalAxis.", 2);
                return;
            }
        }

        var currentRow = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr");
        var isTestAlreadySelected = false;
        $.each(currentRow, function (i, item) {
            var currentRowCPTDescription = $(item).find("input[id*='Procedure']").val() != null ? $(item).find("input[id*='Procedure']").val().replace(cptCode, "").trim() : "";
            if (cptDescription.toLowerCase().replace(/\-/gi, '').trim() == currentRowCPTDescription.toLowerCase().replace(/\-/gi, '').trim()) {
                isTestAlreadySelected = true;
                return;
            }
        });

        if (isTestAlreadySelected != true) {
            OrderSet_LabOrderDetails.AddNewLabRow(modifier, null, null, null, cptCode, procedureDescription, cptDescription, null, SampleStorage);
            setTimeout(function () {
                $("#" + OrderSet_LabOrderDetails.params.PanelID + " #txtLabCPTCode").val('');
            }, 200);
        }
        else {
            utility.DisplayMessages("Test is already selected", 2);
        }

        //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
        if ($("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr").length > 0 && $("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr").find('.dataTables_empty').length == 0) {
            $("#" + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder,#btnResetLabOrder").removeClass('disableAll');
        }
        else {
            $("#" + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder,#btnResetLabOrder").addClass('disableAll');
        }

        OrderSet_LabOrderDetails.OrderedTestsCount++;
        //End 05-04-2016 Humaira Yousaf to enable/disable action buttons
    },

    buildRowChild: function (obj, ParentRowId, SampleStorage) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div class='col-xs-12 p-none pb-default pt-default'></div>");

        var ddlSpecimen = "<div class='col-sm-4'><label class='control-label'>Prefer Specimen</label><select id='Specimen" + ParentRowId + "' name='Specimen' class='form-control' ></select></div>";

        var txtSampleStorage = '';

        if (SampleStorage != null)
            txtSampleStorage = "<div class='col-sm-4'><label class='control-label'>Sample Storage</label><select id='ddlSampleStorage" + ParentRowId + "' name='SampleStorage' class='form-control'><option value='' >- Select -</option><option value='" + SampleStorage + "'>" + SampleStorage + "</option></select></div>";
        else
            txtSampleStorage = "<div class='col-sm-4'><label class='control-label'>Sample Storage</label><select id='ddlSampleStorage" + ParentRowId + "' name='SampleStorage' class='form-control'><option value='' >- Select -</option></select></div>";

        var ddlAlternativeSpecimen = "<div class='col-sm-4'><label class='control-label'>Alternative Specimen</label><select id='AlternativeSpecimen" + ParentRowId + "' name='AlternativeSpecimen' class='form-control'></select></div>";

        var Volume = "<div class='col-sm-4'><label class='control-label'>Volume</label><input type='number'  class='form-control' onkeydown='OrderSet_LabOrderDetails.validateVolume(event,this)' id='VolumeText" + ParentRowId + "' name='VolumeText'></input></div>";

        //var Volume = "<div class='col-sm-8 p-none'><label class='control-label col-xs-12'>Volume</label><div class='col-sm-6'><input type='number'  class='form-control' onkeydown='OrderSet_LabOrderDetails.validateVolume(event,this)' id='VolumeText" + ParentRowId + "' name='VolumeText'></input></div>";

        //var ddlVolume = "<div class='col-sm-6'><input type='text'  class='form-control'  id='VolumeDDL" + ParentRowId + "'' name='VolumeDDL'></input></div></div>";
        var ddlVolume = "<div class='col-sm-4'><label class='control-label'>Unit</label><input type='text'  class='form-control' maxlength = '20'  id='VolumeDDL" + ParentRowId + "'' name='VolumeDDL'></input</div>";

        Volume += ddlVolume;
        // Faizan ameen.

        //
        var PatientInstructions = "<div class='col-sm-4'><label class='control-label'>Patient Instructions</label><input type='text' onkeypress='OrderSet_LabOrderDetails.validateSpecialCharacters(event)' class='form-control' maxlength = '500'   id='PatientInstructions" + ParentRowId + "'  name='PatientInstructions'></input></div>";

        var FillerInstructions = "<div class='col-sm-4'><label class='control-label'>Filler Instructions</label><input type='text' onkeypress='OrderSet_LabOrderDetails.validateSpecialCharacters(event)' class='form-control'  maxlength = '500' id='FillerInstructions" + ParentRowId + "' name='FillerInstructions'></input></div>";

        ChildHTML.append(ddlSpecimen, ddlAlternativeSpecimen, txtSampleStorage, Volume, PatientInstructions, FillerInstructions);

        return ChildHTML;

    },
    //Author: Ahmad Raza
    //Date :  21-06-2016
    //Function Name: validateSpecialCharacters
    //Description: This function will validate the special characters
    validateSpecialCharacters: function (event) {
        var valid = (event.which == 8 || event.which == 9 || event.which == 32) || (event.which >= 48 && event.which <= 57) || (event.which >= 65 && event.which <= 90) || (event.which >= 97 && event.which <= 122);
        if (!valid) {
            event.preventDefault();
        }
    },

    //Author: Ahmad Raza
    //Date :  21-06-2016
    //Function Name: validateSpecialCharacters
    //Description: This function will validate the special characters
    validateVolume: function (event, obj) {

        if ($(obj).val().length === 3) {
            event.preventDefault();
        }
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

    rowDetail: function ($row, obj) {
        var ChargeCapId = Number($row.attr("id"));
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ChargeCapId"] = ChargeCapId;
                params["mode"] = "Edit";
                //Edited by Azeem Raza Tayyab on 16-Feb-2016 to fix bug#: PMS-3871
                if (OrderSet_LabOrderDetails.params.TabID == 'chargeBatchDetail' || OrderSet_LabOrderDetails.params.TabID == 'billTabUnClaimedAppointment' || OrderSet_LabOrderDetails.params.TabID == 'Bill_ChargeSearch' || OrderSet_LabOrderDetails.params.TabID == 'Patient_Case_Detail' || OrderSet_LabOrderDetails.params.TabID == 'schTabCalendar' || OrderSet_LabOrderDetails.params.TabID == 'batchTabEncounter' || OrderSet_LabOrderDetails.params.TabID == 'Bill_FollowUpPatientAR_Detail' || OrderSet_LabOrderDetails.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || OrderSet_LabOrderDetails.params.TabID == "billTabClaimSubmission" || OrderSet_LabOrderDetails.params.TabID == "Bill_PaymentPosting" || OrderSet_LabOrderDetails.params.TabID == "EDIClaimViewDetail")
                    params["ParentCtrl"] = 'OrderSet_LabOrderDetails';
                else
                    params["ParentCtrl"] = OrderSet_LabOrderDetails.params["TabID"];
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
                OrderSet_LabOrderDetails.DeleteLabOrderTest($row.attr("id"), $row, obj);
            }
            else {
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
                utility.DisplayMessages("Successfully Deleted", 1);

                //Start 05-04-2016 Humaira Yousaf to enable/disable action buttonss
                if ($("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr").length > 0 && $("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr").find('.dataTables_empty').length == 0) {
                    $("#" + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder,#btnResetLabOrder").removeClass('disableAll');
                }
                else {
                    $("#" + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder,#btnResetLabOrder").addClass('disableAll');
                }
                //End 05-04-2016 Humaira Yousaf to enable/disable action buttons
            }
            OrderSet_LabOrderDetails.OrderedTestsCount--;
            ClinicalLabOrderDetail.AOEsExists = false;

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

    AddNewLabRow: function (modifier, RowId, mode, CurrRef, cptCode, procDesc, cptDescription, response, SampleStorage) {

        var LabGridId = "#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail";

        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (OrderSet_LabOrderDetails.params.ParentCtrl != null) {
                CurrentRow = OrderSet_LabOrderDetails.EditableGrid.rowAdd(RowId, "");
            }
            else {
                CurrentRow = OrderSet_LabOrderDetails.EditableGrid.rowAdd(RowId, OrderSet_LabOrderDetails.params.LabId);
            }

        }
        else {
            var TemplateRow = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr[id*='-']").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            CurrentRow = OrderSet_LabOrderDetails.EditableGrid.rowAdd(TemplateRowId - 1, "", 'OrderSet_LabOrderDetails');
        }

        //add cptCode and description to the test row
        var rowId = CurrentRow.attr("id");
        var modifierHtml = '<input type="hidden" id="Modifier' + rowId + '"  name="Modifier" value="' + modifier + '" />';
        var cptCodeHtml = '<input type="hidden" id="CPTCode' + rowId + '"  name="CPTCode" value="' + cptCode + '" />';
        var cptDescHtml = '<input type="hidden" id="CPTDescription' + rowId + '" name="CPTDescription"  value="' + procDesc + '"  />';

        $(CurrentRow).find('td:first').append(cptCodeHtml + cptDescHtml + modifierHtml);

        //Start//For Questions and Answer Of Loinc Code
        var isAnswerExists = false;
        var isQuestionExists = false;

        // ----- commenting AOE logic for PRD-291

        //if (response != null && RowId > 0) {

        //    isQuestionExists = response.AOEExists == "1" ? true : false;
        //    isAnswerExists = response.AnswerExists == "1" ? true : false;
        //    //var object = {
        //    //    Answer: "Hello Response From Database",
        //    //    CPTCode: response.CPTCode,
        //    //    //FirstLoad:true
        //    //};
        //    //OrderSet_LabOrderDetails.pusCPTCodeQA(object,true);

        //    OrderSet_LabOrderDetails.setQuestionAndAnswerIcon(isQuestionExists, isAnswerExists, $(CurrentRow), response.CPTCode, response.LabOrderTestId, false, response.CPTDescription);
        //}
        //else {

        //    OrderSet_LabOrderDetails.getLabOrderQuestionAnswer(cptCode).done(function (quesAnsObj) {
        //        if (quesAnsObj != null) {

        //            isQuestionExists = quesAnsObj.IsQuestionExists;

        //        }
        //        OrderSet_LabOrderDetails.setQuestionAndAnswerIcon(isQuestionExists, isAnswerExists, $(CurrentRow), cptCode, null, true, procDesc);
        //    });
        //}

        //End//For Questions and Answer Of Loinc Code


        //add cptCode and description to the test row
        var row = OrderSet_LabOrderDetails.EditableGrid.datatable.row(CurrentRow);
        row.child(OrderSet_LabOrderDetails.buildRowChild(row.data(), CurrentRow.attr("id"), SampleStorage)).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child().loadDropDowns(true).done(function () {

            $(CurrentRow).find('select[id*="Urgency"]').val('1');
        });

        row.child().find('select,datalist').each(function () {
            var data = null;
            if ($(this).attr('id').indexOf("Specimen") == 0) {
                data = {
                    'StrID': cptCode, 'ID': $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ddlLabId").val(), 'StrID2': 'Prefer', 'StrID3': $(this).attr('id')
                };

                ddlSpecimen = this;
                OrderSet_LabOrderDetails.ddlSpecimen.push(this);

                return MDVisionService.lookups('GetSpecimen', true, data).done(function (results) {
                    results = JSON.parse(results["GetSpecimen"]);
                    if (results) {
                        var l = null;
                        for (var count in OrderSet_LabOrderDetails.ddlSpecimen) {
                            if ($(OrderSet_LabOrderDetails.ddlSpecimen[count]).attr('id') == results.DropDownId) {
                                l = $(OrderSet_LabOrderDetails.ddlSpecimen[count]);
                            }
                        }
                        if (l != null) {
                            results = JSON.parse(results.data);
                            l.empty();
                            $.each(results, function (j, result) {
                                l.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                            });
                        }

                    }
                });
            }
            else if ($(this).attr('id').indexOf("AlternativeSpecimen") == 0) {
                data = {
                    'StrID': cptCode, 'ID': $('#' + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #ddlLabId").val(), 'StrID2': 'Alternative', 'StrID3': $(this).attr('id')
                };
                ddlAlternativeSpecimen = this;
                OrderSet_LabOrderDetails.ddlAlternativeSpecimen.push(this);
                return MDVisionService.lookups('GetSpecimen', true, data).done(function (results) {
                    results = JSON.parse(results["GetSpecimen"]);
                    if (results) {
                        var l = null;
                        for (var count in OrderSet_LabOrderDetails.ddlAlternativeSpecimen) {
                            if ($(OrderSet_LabOrderDetails.ddlAlternativeSpecimen[count]).attr('id') == results.DropDownId) {
                                l = $(OrderSet_LabOrderDetails.ddlAlternativeSpecimen[count]);
                            }
                        }

                        if (l != null) {
                            results = JSON.parse(results.data);
                            l.empty();
                            $.each(results, function (j, result) {
                                l.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                            });
                        }

                    }
                });
            }
            else {
                return true;
            }
        });

        row.child.hide();
        //, cptCode, procDesc, cptDescription
        $(CurrentRow).find('input[id*="LabProcedure"]').val(cptCode + " " + procDesc);

        // $(CurrentRow).find('input[id*="LabProcedure"]').val(procDesc);

        OrderSet_LabOrderDetails.enableRemoveRow($(CurrentRow));

        //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
        if ($("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr").length > 0 && $("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr").find('.dataTables_empty').length == 0) {
            $("#" + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder,#btnResetLabOrder").removeClass('disableAll');
        }
        else {
            $("#" + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder,#btnResetLabOrder").addClass('disableAll');
        }
        //End 05-04-2016 Humaira Yousaf to enable/disable action buttons
        //Start Farooq Ahmad 07/14/2016 /EMR-588
        var dgvLabOrderDetail = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail");

        $(dgvLabOrderDetail).find('input[id*="dtpLabDate"]').removeClass('size80');
        $(dgvLabOrderDetail).find('input[id*="dtpLabDate"]').closest('div').removeClass('size80');

        $(dgvLabOrderDetail).find('input[id*="tpLabTime"]').removeClass('size80');
        $(dgvLabOrderDetail).find('input[id*="tpLabTime"]').closest('div').removeClass('size80');

        $(dgvLabOrderDetail).find('input[id*="LabProcedure"]').addClass('size-min300 mb-tiny');
        //End Farooq Ahmad 07/14/2016 /EMR-588

        return CurrentRow;
    },

    setQuestionAndAnswerIcon: function (isQuestionExists, isAnswerExists, $CurrentRow, cptCode, labOrderTestId, fromNewRow, CPTDescription) {
        if (isQuestionExists) {

            var quesAnsIcon = "";
            var commentIconColor = 'blue';
            if (isAnswerExists) {
                commentIconColor = 'green';
            }
            labOrderTestId = labOrderTestId == null ? 0 : labOrderTestId;
            var onClick = "OrderSet_LabOrderDetails.loadQuestionAndAnswers(this,'" + cptCode + "','" + labOrderTestId + "','" + CPTDescription + "');";
            quesAnsIcon = '<a href="#" onClick ="' + onClick + '" class="btn-xs mr-none btn" title="View Questions And Anwers"><i class="fa fa-comments ' + commentIconColor + '" aria-hidden="true"></i></a>';
            $CurrentRow.find('td[class*="actions"]').append(quesAnsIcon);
            if (fromNewRow == true) {
                OrderSet_LabOrderDetails.loadQuestionAndAnswers(this, cptCode, labOrderTestId, CPTDescription);
            }
        }
    },

    getLabOrderQuestionAnswer: function (CPTCode) {

        var quesAnsObj = new Object();
        var deffered = new $.Deferred();
        quesAnsObj.IsQuestionExists = false;
        if (CPTCode != null) {

            OrderSet_LabOrderDetails.getLabOrderQuestionAnswerDbCall(CPTCode).done(function (response) {

                if (response != "") {
                    response = JSON.parse(response);
                    if (response.LabOrderAOE_Count > 0) {
                        quesAnsObj.IsQuestionExists = true;
                    }
                }
                deffered.resolve(quesAnsObj);
            });
        }
        return deffered.promise();
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason:to add more problem is Associated Problem list
    addProblem: function () {
        var params = [];
        params["OrderSetId"] = OrderSet_LabOrderDetails.params.OrderSetId;
        params["RefForm"] = "frmOrderSetLabOrderDetails";
        params["FromOrderDetail"] = "1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_LabOrderDetails";
        LoadActionPan('OrderSet_Problems', params);
    },

    GetLabRowsJSON: function () {

        //var myArray = [];
        //var objLab = new Object();
        //$.each($("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr:not([id*=Child]"), function (i, item) {

        //    var $row = $(this).closest("tr");
        //    var rowId = $row.attr("id");
        //    var self = $row;
        //    var myJSON = self.getMyJSONByName();
        //    var childRow = OrderSet_LabOrderDetails.EditableGrid.datatable.row($row).child();
        //    var childRowsJSON = childRow.getMyJSONByName();
        //    JSONToSave = utility.MergeJSON(myJSON, childRowsJSON);
        //    //objLab["LabOrderId" + rowId] = JSONToSave;
        //    objLab["LabOrderId"] = JSONToSave;
        //    //myArray.push(myJSON);
        //});
        //objLab["commandType"] = "save_Lab_order_test";

        var LabTestIds = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr:not([id*=Child]").map(function () {
            return this.id.replace("id", "");
        }).get().join(',');

        $("#" + OrderSet_LabOrderDetails.params.PanelID + " #pnlLab_ResultOrderDetail #hfLabTestIds").val(LabTestIds);

        var self = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #pnlLab_ResultOrderDetail");
        var myJSON = self.getMyJSONByName();


        OrderSet_LabOrderDetails.SaveLabOrderTest(myJSON);
        //console.log(objLab);
    },

    SaveLabOrderTest: function (objLab) {
        OrderSet_LabOrderDetails.LabOrderTestSave_DBCall(objLab).done(function (response) {

            var response = JSON.parse(response);
            if (response.status != false) {

            } else {

            }


        });
    },

    LabOrderTestSave_DBCall: function (LabOrderData) {

        if (OrderSet_LabOrderDetails.params.mode.toLowerCase() == "add") {

            var objData = JSON.parse(LabOrderData);
            objData["commandType"] = "save_Lab_order_test";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "OrderSet", "LabOrder");

        } else if (OrderSet_LabOrderDetails.params.mode.toLowerCase() == "edit") {
            var objData = JSON.parse(LabOrderData);
            objData["commandType"] = "save_Lab_order_test";
            objData["LabOrderId"] = $("#" + OrderSet_LabOrderDetails.params.PanelID + ' #hfLabOrderId').val();
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "OrderSet", "LabOrder");
        }





    },

    LabOrderTestGridLoad: function (response) {
        var response = JSON.parse(response);
        var PanelLabGrid = "#" + OrderSet_LabOrderDetails.params.PanelID + " #pnlLab_ResultOrderDetail";
        var LabGridId = "#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail";
        $(LabGridId + " tbody tr").remove();


        if ($.fn.dataTable.isDataTable(LabGridId)) {
            $(LabGridId).dataTable().fnClearTable();
            $(LabGridId).dataTable().fnDestroy();
        }

        OrderSet_LabOrderDetails.EditableGrid.datatable.clear().draw();

        var LabOrderTestLoadJSONData = JSON.parse(response.LabOrderTest_JSON);
        OrderSet_LabOrderDetails.ddlSpecimen = [];
        OrderSet_LabOrderDetails.ddlAlternativeSpecimen = [];
        $.each(LabOrderTestLoadJSONData, function (i, item) {

            var callbacks = $.Callbacks();
            var CurrentRow = null;
            var newChildRow = null;
            var createCurrentRow = function (i, item, CurrentRow, newChildRow) {
                var LabOrderTestId = item.LabOrderTestId;
                var arrChildLab = [];
                arrChildLab.push(item);
                CurrentRow = OrderSet_LabOrderDetails.AddNewLabRow(LabOrderTestId, null, null, item.CPTCode, item.CPTDescription, null, item, item.SampleStorage);
                OrderSet_LabOrderDetails.buildRowChild(CurrentRow, CurrentRow.attr("id"), null, null, arrChildLab);
                var self = $("#" + OrderSet_LabOrderDetails.params.PanelID + " tr#" + LabOrderTestId);
                var row = OrderSet_LabOrderDetails.EditableGrid.datatable.row(CurrentRow);
                item.CurrentRow = CurrentRow;

                newChildRow = row.child();
                item.newChildRow = newChildRow;
                var LabTestTable = $("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail");
            }

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

            $.when(createCurrentRow(i, item, CurrentRow, newChildRow)).then(BindFunction(counter, item, CurrentRow, newChildRow));
            OrderSet_LabOrderDetails.OrderedTestsCount++;
        });

    },

    DeleteLabOrderTest: function (LabTestId, $row, obj) {

        OrderSet_LabOrderDetails.LabOrderTest_DBCall(LabTestId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();

                //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
                if ($("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr").length > 0 && $("#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr").find('.dataTables_empty').length == 0) {
                    $("#" + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder,#btnResetLabOrder").removeClass('disableAll');
                }
                else {
                    $("#" + OrderSet_LabOrderDetails.params.PanelID + " #frmOrderSetLabOrderDetails #btnSaveOrder,#btnResetLabOrder").addClass('disableAll');
                }
                //End 05-04-2016 Humaira Yousaf to enable/disable action buttons

            } else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },

    LabOrderTest_DBCall: function (LabTestId) {

        var objData = new Object();
        objData["LabOrderTestId"] = LabTestId;
        objData["commandType"] = "DELETE_LabORDER_TEST";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },


    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: get Latest Lab Order By PatientId
    getLatestLabOrderByPatientId: function (hideAlertMessage) {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            OrderSet_LabOrderDetails.getLatestLabOrderByPatientIdDBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    OrderSet_LabOrderDetails.createLabOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To create Lab Order's Body HTML
    createLabOrderBodyHTML: function (response, NoteHTMLCtrl, UnloadLabOrder, hideAlertMessage) {
        OrderSet_LabOrderDetails.checkLabOrderExists();
        if (response.LabOrderFill_JSON != null && response.LabOrderFill_JSON != '') {
            var LabOrderFill_Obj = JSON.parse(response.LabOrderFill_JSON);
            var $mainDivLabOrder = $(document.createElement('div'));

            var LabOrderId = LabOrderFill_Obj.LabOrderId;
            if (LabOrderId > 0) {
                var $SectionBodyLabOrder = $(document.createElement('section'));
                $SectionBodyLabOrder.attr('id', "Cli_LabOrderDetail_Main" + LabOrderId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_LabOrderDetail_" + LabOrderId);
                var $ListLabOrder = $(document.createElement('ul'));

                $ListLabOrder.attr('class', 'list-unstyled')

                $SectionBodyLabOrder.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_LabOrderDetail_" + LabOrderId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_LabOrderDetail_Main" + LabOrderId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListLabOrder.append("<li>" + LabOrderFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListLabOrder);
                $SectionBodyLabOrder.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' OrderSet_LabOrders').parent().parent().find('#Cli_LabOrderDetail_Main' + LabOrderId).length == 0) {
                    $mainDivLabOrder.append($SectionBodyLabOrder);
                    OrderSet_LabOrderDetails.updateLabOrderHtml($mainDivLabOrder.html(), LabOrderId, NoteHTMLCtrl, hideAlertMessage);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' OrderSet_LabOrders').parent().parent().find('#Cli_LabOrderDetail_Main' + LabOrderId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' OrderSet_LabOrders').parent().parent().find('#Cli_LabOrder_Main' + LabOrderId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' OrderSet_LabOrders').parent().parent().find('#Cli_LabOrderDetail_Main' + LabOrderId).html($SectionBodyLabOrder.html());
                    $(NoteHTMLCtrl + ' OrderSet_LabOrders').parent().parent().find('#Cli_LabOrderDetail_Main' + LabOrderId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText('Lab Order', hideAlertMessage);
                    OrderSet_LabOrderDetails.updateLabOrderHtml("", LabOrderId, NoteHTMLCtrl, hideAlertMessage);

                }

                if (UnloadLabOrder == true) {
                    OrderSet_LabOrderDetails.Unload(OrderSet_LabOrderDetails.bNextPrev);
                }
            }
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To detach Components Lab Order
    detach_ComponentsLabOrder: function (ComponentName, IsUpdate, LabOrdersComponentRemove) {
        var LabOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_LabOrders').parent().parent().find('section[id*="Cli_LabOrderDetail_Main"]').map(function () {
            return this.id.replace("Cli_LabOrderDetail_Main", "");
        }).get().join(',');
        if (LabOrderIds == "" || LabOrderIds == "undefined") {
            Clinical_ProgressNote.saveComponentSOAPText('Lab Order', true);
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            OrderSet_LabOrderDetails.detachLabOrderFromNotesDBCall(LabOrderIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText('Lab Order', true);
                        //Start 01-12-2016 Humaira Yousaf to hide show eSuperbill link
                        Clinical_ProgressNote.HideShowBillingInfo();
                        //End 01-12-2016 Humaira Yousaf to hide show eSuperbill link
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        if (LabOrdersComponentRemove) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Lab Orders']").remove();
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_LabOrders').parent().parent().remove();
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_LabOrders').parent().parent().find('section[id*="Cli_LabOrderDetail_Main"]').remove();
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To detach Lab Order from Notes
    detachLabOrderFromNotes: function (LabOrderId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = LabOrderId.replace('Cli_LabOrderDetail_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    OrderSet_LabOrderDetails.detachLabOrderFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + LabOrderId).remove();

                            Clinical_ProgressNote.saveComponentSOAPText('Lab Order');
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to detach Lab Order from Notes
    detachLabOrderFromNotes_DBCall: function (LabOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabOrderId"] = LabOrderId;
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
        objData["commandType"] = "detach_Laborder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to detach Lab Order From Notes
    detachLabOrderFromNotesDBCall: function (LabId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabOrderId"] = LabId;
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
        objData["commandType"] = "detach_LabOrder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To update Lab Order Html
    updateLabOrderHtml: function (LabOrderHtml, LabOrderId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' OrderSet_LabOrders').parent().parent().addClass('initialVisitBody');
        if (LabOrderHtml != '') {
            $(NoteHTMLCtrl + ' OrderSet_LabOrders').parent().parent().append(LabOrderHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (LabOrderHtml != '') {
            OrderSet_LabOrderDetails.attachLabOrderWithNotes(LabOrderId, hideAlertMessage);
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To attach Lab Order With Notes
    attachLabOrderWithNotes: function (LabOrderId, hideAlertMessage) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var selectedValue = LabOrderId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                OrderSet_LabOrderDetails.attachLabOrderWithNotesDBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //If Attached MedicalHx Made new inseration to MedicalHx Table than good ids should be attached to HTML
                        Clinical_ProgressNote.saveComponentSOAPText('Lab Order', hideAlertMessage);
                        $('#' + LabOrderId).remove();
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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to attach Lab Order With Notes
    attachLabOrderWithNotesDBCall: function (LabOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabOrderId"] = LabOrderId;
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
        objData["commandType"] = "attach_LabOrder_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to get Latest Lab Order By PatientId
    getLatestLabOrderByPatientIdDBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["NoteId"] = Clinical_Notes.params.NotesId;
        objData["commandType"] = "getlatest_Laborderby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },

    getLabOrderInfo: function (labOrderId) {
        OrderSet_LabOrderDetails.fillLabOrder(labOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    OrderSet_LabOrderDetails.createLabOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    generateBarcode: function () {
        var value = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfOrderNumber").val();
        var btype = 'code39';
        var renderer = 'css';
        var quietZone = false;
        var settings = {
            output: renderer,
            bgColor: '#FFFFFF',
            color: '#000000',
            barWidth: '1',
            barHeight: '50',
            moduleSize: '5',
            posX: '10',
            posY: '20',
            addQuietZone: '1'
        };
        $('#' + OrderSet_LabOrderDetails.params.PanelID + " #barcodeTarget").html("").show().barcode(value, btype, settings);

    },

    //Author: Humaira Yousaf
    //Date :  25-04-2016
    //Description: Creates PDF to view Lab Order Result
    printLabOrder: function (includeBarCode) {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        if (includeBarCode) {
            params["BarCodeHtml"] = 'true';
            params["IsSpecimen"] = true;
            $('#' + OrderSet_LabOrderDetails.params.PanelID + " #barcodeTarget").hide();
        }
        else {
            params["BarCodeHtml"] = "";
        }

        params["ParentCtrl"] = "OrderSet_LabOrderDetails";
        params["LabOrderId"] = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfLabOrderId").val();
        LoadActionPan('OrderSet_LabOrderView', params);
    },

    printLabOrderABN: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();


        params["ParentCtrl"] = "OrderSet_LabOrderDetails";
        params["LabOrderId"] = $('#' + OrderSet_LabOrderDetails.params.PanelID + " #hfLabOrderId").val();
        LoadActionPan('OrderSet_LabOrderABNDetail', params);
    },
    getLabOrderQuestionAnswerDbCall: function (CPTCode, LabOrderTestId) {

        var objData = new Object();
        objData["CPTCode"] = CPTCode;
        objData["LabOrderTestId"] = LabOrderTestId;
        objData["commandType"] = "get_question_answer_by_cptcode";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },

    loadQuestionAndAnswers: function (obj, cptCode, LabOrderTestId, CPTDescription) {
        var strMessage = "";
        var params = [];
        var $currentRow = $(obj).parent().parent();
        params["CPTCode"] = cptCode;
        params["CPTDescription"] = CPTDescription;
        params["LabOrderTestId"] = LabOrderTestId != null && LabOrderTestId != '0' ? LabOrderTestId : $currentRow.attr("id");//LabOrderTestId;

        params["FromAdmin"] = OrderSet_LabOrderDetails.params["FromAdmin"];

        params["ParentCtrl"] = 'OrderSet_LabOrderDetails';
        params["ParentCtrlPanelID"] = OrderSet_LabOrderDetails.params.PanelID;
        LoadActionPan('ClinicalLabOrderDetailAOE', params, OrderSet_LabOrderDetails.params.PanelID);
    },
    getCPTCodeQA: function (CPTCode) {

        CPTCodesQA = [];
        $.each(ClinicalLabOrderDetail.CPTCodeQA, function (index, item) {
            if (item.CPTCode == CPTCode) {
                CPTCodesQA.push(ClinicalLabOrderDetail.CPTCodeQA[index]);
            }
        });
        return CPTCodeQA;
    },
    pusCPTCodeQA: function (CPTCodeQA, onloadByPass) {


        if (onloadByPass != null) {

            ClinicalLabOrderDetail.CPTCodeQA.push(CPTCodeQA);
        }
        else {
            if (ClinicalLabOrderDetail.CPTCodeQA.length > 0) {

                isCPTCodeQAExists = false;
                $.each(ClinicalLabOrderDetail.CPTCodeQA, function (index, item) {
                    if (item.CPTCode == CPTCodeQA['CPTCode'] && item.Question == CPTCodeQA['Question']) {
                        isCPTCodeQAExists = true;
                        ClinicalLabOrderDetail.CPTCodeQA[index] = CPTCodeQA;
                    }
                });
                if (!isCPTCodeQAExists) {
                    ClinicalLabOrderDetail.CPTCodeQA.push(CPTCodeQA);
                }
            }
            else {
                ClinicalLabOrderDetail.CPTCodeQA.push(CPTCodeQA);
            }
        }

    },

    colorCPTCodeAOEIcon: function () {

        var LabGridId = "#" + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody";


        var answerIndex = -1;
        $(LabGridId).find('tr').each(function (index, gridItem) {

            var isAnswerExists = false;

            var gridCPTCode = $(gridItem).find('td:first').find("input[id*='CPTCode']").val();


            $.each(ClinicalLabOrderDetail.CPTCodeQA, function (index, item) {

                if (item.CPTCode == gridCPTCode && item.Answer != "") {
                    isAnswerExists = true;

                    answerIndex = index;
                }
            });
            if (isAnswerExists) {

                $(gridItem).find('td[class*="actions"]').find("a").find("i.fa-comments").removeClass('blue').addClass('green');
            }
            else {
            }

        });

    },

    //Author: Humaira Yousaf
    //Date :  28-11-2016
    //Description: Checks if any associated problems is selected on saving order
    problemAdded: function () {

        var problemsSelected = false;
        if ($('#' + OrderSet_LabOrderDetails.params.PanelID + " #ulProblemLists input:checked").length > 0) {
            problemsSelected = true;
        }

        return problemsSelected;
    },

    TestAdded: function () {
        var TestSelected = false;
        if ($('#' + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr").length == 1) {
            if ($('#' + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr td").length > 1) {
                TestSelected = true;
            }
        }
        else if ($('#' + OrderSet_LabOrderDetails.params.PanelID + " #dgvLabOrderDetail tbody tr").length > 1) {
            TestSelected = true;
        }
        return TestSelected;
    },
}
