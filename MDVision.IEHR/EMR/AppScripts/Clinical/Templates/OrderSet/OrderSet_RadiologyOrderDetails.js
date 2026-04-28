OrderSet_RadiologyOrderDetails = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    FavListName: "RadiologyOrderDetail",
    checkedProblems: [],

    Load: function (params) {
        BackgroundLoaderShow(true);

        OrderSet_RadiologyOrderDetails.params = params;

        OrderSet_RadiologyOrderDetails.ddlSpecimen = [];
        OrderSet_RadiologyOrderDetails.ddlAlternativeSpecimen = [];



        if (OrderSet_RadiologyOrderDetails.params.PanelID != 'pnlOrderSetRadiologyOrderDetails') {
            OrderSet_RadiologyOrderDetails.params.PanelID = OrderSet_RadiologyOrderDetails.params.PanelID + ' #pnlOrderSetRadiologyOrderDetails';
        } else {
            OrderSet_RadiologyOrderDetails.params.PanelID = 'pnlOrderSetRadiologyOrderDetails';
        }

        //set patient id in hidden field
        $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #hfPatientId").val($("div#PatientProfile #hfPatientId").val());

        var self = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #divRadiologyOrderInformation");
        var resetButton = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #btnResetRadiologyOrder");

        if (parseInt(OrderSet_RadiologyOrderDetails.params.RadiologyOrderId) > -1) {
            resetButton.hide();
        }
        else {
            resetButton.show();
        }
        var PanelRadiologyGrid = "#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #pnlRadiology_Result";
        var RadiologyGridId = "#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #dgvRadiology";
        $(RadiologyGridId + " tbody tr").remove();
        OrderSet_RadiologyOrderDetails.EditableGrid = EMRUtility.MakeEditableGrid(PanelRadiologyGrid, RadiologyGridId, OrderSet_RadiologyOrderDetails, "0", false, false, false, false);

        //Start//18-03-2016//Abid Ali //Load All Patient Insurances

        utility.CreateDatePicker(OrderSet_RadiologyOrderDetails.params.PanelID + ' #frmOrderSetRadiologyOrderDetails input[id="txtOrderDate"]', function () {
            //on-change callback method
        }, true);

        $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #frmOrderSetRadiologyOrderDetails input[id="txtOrderTime"]').timepicker({
            defaultTime: new Date()
        });

        //Start//18-03-2016//Abid Ali //Load All Patient Insurances
        OrderSet_RadiologyOrderDetails.loadAllAutocomplete();
        // Set Title Explicitly if it's passed as Parameter
        if (OrderSet_RadiologyOrderDetails.params.Title != null)
            $("#" + OrderSet_RadiologyOrderDetails.params["PanelID"] + " #headingTitle").text(OrderSet_RadiologyOrderDetails.params.Title);



        var loadDropDowns = function () {

            var dfd = $.Deferred();
            OrderSet_RadiologyOrderDetails.favoriteListSearch();

            //Start 02-03-2016 Humaira Yousaf to load dropdowns
            if (OrderSet_RadiologyOrderDetails.bIsFirstLoad == true) {


                OrderSet_RadiologyOrderDetails.validateRadiologyOrderDetail();
                OrderSet_RadiologyOrderDetails.bIsFirstLoad = false;

                self.loadDropDowns(true).done(function () {
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
            }
            //End 02-03-2016 Humaira Yousaf to load dropdowns

            dfd.promise();
        }
        var loadOrder = function () {
            //Start 13-04-2016 Abid Ali to load Laboratories
            Clinical_LabOrder.LoadLabs('ddlLabId', OrderSet_RadiologyOrderDetails.params.PanelID).done(function () {
                OrderSet_RadiologyOrderDetails.EnableDisableTestSearch($('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #ddlLabId"));
                $.when(CacheManager.BindDropDownsByID($('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #frmOrderSetRadiologyOrderDetails #ddlAssigneeId'), 'GetUsersFullName', true, 1)).done(function () {
                    $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #ddlAssigneeId option:first").text('- Select -');
                    OrderSet_RadiologyOrderDetails.loadProblemList();
                    $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails").data('serialize', $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails").serialize());

                });
                
            });
            //End 13-04-2016 Abid Ali to load Laboratories
        }
        $.when(loadDropDowns()).done(loadOrder());

        OrderSet_RadiologyOrderDetails.Init();
        //Check buttons
        OrderSet_RadiologyOrderDetails.enableDisableRadiologyOrderButtons();


    },

    //Author: Abid Ali
    //Date :  07-03-2016
    //Reason: Clear/Set gurantor/relation values
    setGurantorRelationValues: function (setValues) {

        var guarantorName = OrderSet_RadiologyOrderDetails.params.GuarantorName;
        var guarantorId = OrderSet_RadiologyOrderDetails.params.GuarantorId;
        var relationName = OrderSet_RadiologyOrderDetails.params.RelationName;
        var relationId = OrderSet_RadiologyOrderDetails.params.RelationId;

        var self = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID);

        if (setValues) {
            self.find("#txtGuarantor").val(guarantorName);
            self.find('#hfGuarantorId').val(guarantorId);
            self.find('#txtRadiologyRelationShipId').val(relationName);
            self.find('#hfRalationId').val(relationId);
        }
        else {
            self.find("#txtGuarantor").val("");
            self.find('#hfGuarantorId').val("");
            self.find('#txtRadiologyRelationShipId').val("");
            self.find('#hfRalationId').val("");
        }
    },

    //Author: Muhammad Arshad
    //Date :  21-04-2016
    //This function will handle Add/Edit of RadiologyResult
    RadiologyResultAddEdit: function (RadiologyResultId, RadiologyOrderId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (RadiologyResultId != null && parseInt(RadiologyResultId) > 0) {
                    params["RadiologyResultId"] = RadiologyResultId;
                    params["mode"] = "Edit";
                }
                else {
                    params["RadiologyResultId"] = -1;
                    params["mode"] = "Add";
                }
                params["RadiologyOrderId"] = OrderSet_RadiologyOrderDetails.params["RadiologyOrderId"];
                params["FromAdmin"] = OrderSet_RadiologyOrderDetails.params["FromAdmin"];
                params["ParentCtrl"] = 'OrderSet_RadiologyOrderDetails';
                LoadActionPan('ClinicalRadiologyResultDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //Author: Abid Ali
    //Date :  06-05-2016
    //Reason: Reset The Entire form
    radiologyOrderDetailReset: function () {

        utility.myConfirm('22', function () {

            //selectors
            var form = "#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails";
            var $problems = $(form + " #divRadiologyAssociatedProblems");
            // var $testInformation = $(form + ' #divTestInformation');
            var $billingInformation = $(form + " #divRadiologyBillingInformation");
            var $orderInfomation = $(form + " #divRadiologyOrderInformation");

            $problems.resetAllControls(null);
            //  $testInformation.resetAllControls(null);
            $billingInformation.resetAllControls(null);
            $orderInfomation.resetAllControls(null);

            //Clear and draw the data table
            OrderSet_RadiologyOrderDetails.EditableGrid.datatable.clear().draw();

            //Disable Radiology Order Detail button.
            OrderSet_RadiologyOrderDetails.enableDisableRadiologyOrderButtons();

            //revalidate the required fields
            $(form).bootstrapValidator('revalidateField', 'LabId').bootstrapValidator('revalidateField', 'Provider').bootstrapValidator('revalidateField', 'Facility');

        }, function () { },
            '22'
        );
    },


    //Author: Muhammad Arshad
    //Date :  25-03-2016
    //Reason: Function to disable Radiology Order Controls
    disableRadiologyOrderControls: function (IsSigned) {
        var detailsDivs = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #divRadiologyOrderInformation,#divRadiologyBillingInformation,#divTestInformation,#divRadiologyAssociatedProblems");
        var detailsButtons = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #btnSaveOrder");
        if (IsSigned == true) {
            detailsDivs.addClass("disableAll");
            detailsButtons.addClass("hidden");
        }
        else {
            detailsDivs.removeClass("disableAll");
            detailsButtons.removeClass("hidden");
        }
    },

    enableDisableRadiologyOrderButtons: function () {

        var isDisable = $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #dgvRadiology tbody tr").length > 0 && $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #dgvRadiology tbody tr").find('.dataTables_empty').length == 0 ? false : true;

        var detailsButtons = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #btnSaveOrder");
        if (isDisable) {
            detailsButtons.addClass("disableAll");
        }
        else {
            detailsButtons.removeClass("disableAll");
        }
    },

    //Author: Muhammad Arshad
    //Date :  24-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteListSearch: function () {
        Favorite_ProcedureOrder.searchFavoriteList_DBCall("RadiologyOrder", null, 1, 5000).done(function (response) {

            response = JSON.parse(response);

            var $ddl = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlFavoriteListRadiologyOrder');
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
                          }));
                    }
                });
            }
        });

    },

    //Author: Muhammad Arshad
    //Date :  25-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    selectAllFavoriteListContent: function () {

        $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ulFavoriteListRadiologyOrderContent li').each(function (i, item) {
            var cptCode = $(item).attr('CPTCode')
            var procDesc = $(item).attr('procedureDescription');
            var cptDescription = $(item).attr('cptDescription');

            var isTestAlreadySelected = OrderSet_RadiologyOrderDetails.isTestExists(procDesc);

            if (isTestAlreadySelected != true) {
                OrderSet_RadiologyOrderDetails.AddNewRadiologyRow(null, null, null, cptCode, procDesc, cptDescription);
            }
            else {
                utility.DisplayMessages("Test is already selected", 2);
                // return false;
            }
        });

    },

    //Author: Muhammad Arshad
    //Date :  24-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    loadfavoriteListContent: function (obj, favListIds) {
        var $uL = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ulFavoriteListRadiologyOrderContent');
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            var selectedOptionValue = selectedOption.attr("id");
            //Start 12-07-2016 Muhammad Arshad Bug#EMR-1486 Lab order -> Favorite list -> "Select ALL " functionality is not working on 46 server ,please see attached video and resolve the issue
            var divSelectAllFavorites = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #divSelectAllRadiologyOrderFavorites');
            if (selectedOptionValue > 0) {
                divSelectAllFavorites.removeClass("disableAll");

                OrderSet_RadiologyOrderDetails.favoriteList_CPTSearch(selectedOptionValue);
            }
            else {
                $uL.empty();
                if (divSelectAllFavorites.hasClass("disableAll") == false) {
                    divSelectAllFavorites.addClass("disableAll");
                }
            }
            //End 12-07-2016 Muhammad Arshad Bug#EMR-1486 Lab order -> Favorite list -> "Select ALL " functionality is not working on 46 server ,please see attached video and resolve the issue

        }
        else {
            if (favListIds != null) {
                //clear the list
                // var $uL = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ulFavoriteListRadiologyOrderContent');
                $uL.empty();

                $.each(favListIds, function (index, item) {
                    //load all favList
                    OrderSet_RadiologyOrderDetails.favoriteList_CPTSearch(item, false);
                });
            }
        }
    },

    //Author: Muhammad Arshad
    //Date :  24-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteList_CPTSearch: function (FavoriteListId, isEmptyUl) {
        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(null, FavoriteListId, 1, 5000).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var objData = JSON.parse(response.FavoriteListCPTJSON);
                var $uL = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ulFavoriteListRadiologyOrderContent');
                isEmptyUl = isEmptyUl == null ? true : false;
                if (isEmptyUl) {
                    $uL.empty();
                }
                $.each(objData, function (i, item) {
                    if (item.CPTCodeDescription != "") {
                        var onclick = 'OrderSet_RadiologyOrderDetails.BindProcedureGridItem(\'' + item.CPTCode + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + String(item.CPTCodeDescription) + '\')';
                        var $li = $(document.createElement('li'));
                        $li.attr('onclick', onclick);
                        $li.attr('CPTCode', item.CPTCode);
                        $li.attr('procedureDescription', String(item.CPTCodeDescription));
                        $li.attr('cptDescription', String(item.CPTCodeDescription));
                        $li.append(item.CPTCodeDescription);
                        $uL.append($li);
                    }
                });


                //Favorite_ProcedureOrder.FavoriteListCPTGridLoad(response);
            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Bind Guaranter
    bindGuarantor: function () {
        var shortName = $('#pnlOrderSetRadiologyOrderDetails #txtGuarantor').val();
        utility.GetGuarontorArray(shortName).done(function (response) {

            $('#pnlOrderSetRadiologyOrderDetails #txtGuarantor').autocomplete({
                //source: AllPatients, // pass an array (without a comma)
                autoFocus: true,
                source: response,
                select: function (event, ui) {

                    setTimeout(function () {

                        $("#pnlOrderSetRadiologyOrderDetails #hfRadiologyGuarantorId").val(ui.item.id); // add the selected id
                        if ($("#pnlOrderSetRadiologyOrderDetails #lnkGuarantorEdit").css("display") == "none") {
                            $("#pnlOrderSetRadiologyOrderDetails #lnkGuarantorEdit").css("display", "inline");
                            $("#pnlOrderSetRadiologyOrderDetails #lblGuarantor").css("display", "none");
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
        params["ParentCtrl"] = "OrderSet_RadiologyOrderDetails";
        params["RefCtrl"] = "txtGuarantor";
        LoadActionPan('Patient_Guarantor', params);
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: open Guaranter
    openGuarantorDetail: function () {
        //Patient_Guarantor.GuarantorEdit($('#pnlDemographic #hfGuarantor').val(), 'patTabDemographic');
        var params = [];
        params["GuarantorId"] = $('#pnlOrderSetRadiologyOrderDetails #hfRadiologyGuarantorId').val();
        params["mode"] = "Edit";
        params["RefCtrl"] = "txtGuarantor";
        params["ParentCtrl"] = 'OrderSet_RadiologyOrderDetails';
        LoadActionPan('guarantorDetail', params);
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Load auto complete for provider and Facility
    loadAllAutocomplete: function () {
        //CacheManager.BindCodes('GetProvider', false).done(function (result) {
        //    $("#frmOrderSetRadiologyOrderDetails #txtProvider").autocomplete({
        //        autoFocus: true,
        //        source: Providers,
        //        select: function (event, ui) {

        //            setTimeout(function () {
        //                //if ($('#' + ClinicalProcedureOrderDetail.params.PanelID + " #lnkProcedureProviderEdit").css("display") == "none") {
        //                //    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #lnkProcedureProviderEdit").css("display", "");
        //                //    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #lblProcedureProvider").css("display", "none");
        //                //}
        //                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #txtProvider").attr("Provider", ui.item.id);
        //                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #hfProvider").val(ui.item.id);
        //            }, 100);
        //        }
        //    });
        //});

        //CacheManager.BindCodes('GetFacility', false).done(function (result) {
        //    $("#frmOrderSetRadiologyOrderDetails #txtFacility").autocomplete({
        //        autoFocus: true,
        //        source: Facilities,
        //        select: function (event, ui) {

        //            setTimeout(function () {
        //                //if ($('#' + Clinical_SurgicalHx.params.PanelID + " #lnkProcedureAssigneeEdit").css("display") == "none") {
        //                //    $('#' + Clinical_SurgicalHx.params.PanelID + " #lnkProcedureAssigneeEdit").css("display", "");
        //                //    $('#' + Clinical_SurgicalHx.params.PanelID + " #lblProcedureAssignee").css("display", "none");
        //                //}
        //                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #txtFacility").attr("Facility", ui.item.id);
        //                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #hfFacility").val(ui.item.id);
        //            }, 100);
        //        }
        //    });
        //});
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
            OrderSet_RadiologyOrderDetails.enableDisableDropdownList(ddlIds, true);
        else {
            ddlIds.push('ddlPrimaryInsuraceId');
            ddlIds.push('ddlSecondaryInsuraceId');
            ddlIds.push('ddlTertiaryInsuraceId');
            OrderSet_RadiologyOrderDetails.enableDisableDropdownList(ddlIds, false);
        }
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Enable disable dropdownlists
    enableDisableDropdownList: function (listOfDdlIds, isHide) {

        $.each(listOfDdlIds, function (index, item) {
            if (isHide) {
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #' + item).prop('disabled', true);
            }
            else {
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #' + item).removeClass('disabled', false);
            }
        });
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Populate patient insurances in ddl
    populateInsuranceDropDownList: function (ddlTypeId, options) {
        var $ddl = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #' + ddlTypeId);

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

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: Function to load problem list
    loadProblemList: function () {
        OrderSet_RadiologyOrderDetails.SearchProblemList().done(function (response) {
            var obj = JSON.parse(response);
            if (obj.status != false) {
                var loadProblems = function () {
                    if (obj.ProblemListCount > 0) {
                        var ProblemListLoadJSONData = JSON.parse(obj.ProblemListLoad_JSON);
                        var ProblemListHistoryLoadJSONData = JSON.parse(obj.ProblemListHistoryLoad_JSON);
                        var finalTr = '';
                        var counter = 2;
                        $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #ulProblemLists tbody tr").remove();
                        $.each(ProblemListLoadJSONData, function (i, item) {
                            if (counter % 2 == 0) {
                                finalTr = finalTr + '<tr>';
                            }
                            finalTr = finalTr + '<td><div class="p-xs col-xs-12"><div class="checkbox-custom">';

                            //$(finalTr).find('input[id*="chk"]').trigger('click');

                            //Start//Abid Ali//For bug# EMR-1590
                            var checked = OrderSet_RadiologyOrderDetails.isCheckedProblem('chk' + item.ProblemListId + '') ? "checked" : "";
                            finalTr = finalTr + '<input ' + checked + ' type="checkbox" onclick ="OrderSet_RadiologyOrderDetails.pushProblems(this);" name="' + item.Code + '" value="' + item.ProblemListId + '"  id="chk' + item.ProblemListId + '">';
                            //End//Abid Ali//For bug# EMR-1590

                            finalTr = finalTr + '   <label class="control-label">' + item.Description + '</label></div></div></td>';

                            if (counter % 2 == 1) {
                                finalTr = finalTr + '</tr>';
                            }
                            counter++;
                            var color = "";
                        });
                        $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #ulProblemLists tbody").append(finalTr);
                    }
                }
                var loadOrder = function () {

                    var radiologyOrderId = OrderSet_RadiologyOrderDetails.params.RadiologyOrderId;
                    radiologyOrderId = typeof radiologyOrderId != 'undefined' && radiologyOrderId > 0 ? radiologyOrderId : -1
                    OrderSet_RadiologyOrderDetails.loadRadiologyOrder(radiologyOrderId);
                    $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #hfRadiologyOrderId").val(radiologyOrderId);
                }
                $.when(loadProblems()).then(loadOrder());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
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
            if (!($.inArray(id, OrderSet_RadiologyOrderDetails.checkedProblems) != -1)) {
                OrderSet_RadiologyOrderDetails.checkedProblems.push(id);
            }
        }
        else {
            //Remove from checked problems
            OrderSet_RadiologyOrderDetails.checkedProblems.splice($.inArray(id, OrderSet_RadiologyOrderDetails.checkedProblems), 1);
        }

    },
    //Author: Abid Ali
    //Date :  15-07-2016
    //Reason: function to check problem in array
    isCheckedProblem: function (problemId) {

        return $.inArray(problemId, OrderSet_RadiologyOrderDetails.checkedProblems) == -1 ? false : this;

    },
    //Author: Abid Ali
    //Date :  17-03-2016
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
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");
    },

    //Author: Abid Ali
    //Date :  21-03-2016
    //Reason: Edit radiologyOrder
    radiologyEdit: function (radiologyOrderId, event) {
        //if icon is clicked then  popup the window

        Clinical_RadiologyOrder.RadiologyOrderAddEdit(radiologyOrderId);
        event.stopPropagation();

    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: Function to load problem list
    SearchProblemList: function () {

        var PageNumber = 1;
        var RowsPerPage = 2000;
        var objData = new Object();
        objData["OrderSetId"] = OrderSet_RadiologyOrderDetails.params.OrderSetId;
        objData["IsActive"] = '1';
        // objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        //objData["NoteId"] = Clinical_ProblemLists.params.NotesId == null ? 0 : Clinical_ProblemLists.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "ProblemList");

    },

    // -------------- Provider ---------------------
    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: open provider form
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmOrderSetRadiologyOrderDetails";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_RadiologyOrderDetails";
        LoadActionPan('Admin_Provider', params);
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: open provider detail
    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#pnlClinicalNotes #hfProvider').val(),'clinicalTabNotes');
        var params = [];
        params["ProviderId"] = $('#pnlOrderSetRadiologyOrderDetails #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'OrderSet_RadiologyOrderDetails';
        LoadActionPan('providerDetail', params);
    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: open facility form
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmOrderSetRadiologyOrderDetails";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_RadiologyOrderDetails";
        LoadActionPan('Admin_Facility', params);
    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: open facility detail form
    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#pnlOrderSetRadiologyOrderDetails #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'OrderSet_RadiologyOrderDetails';
        LoadActionPan('facilityDetail', params);
    },

    HideProviderLink: function () {
        $('#pnlOrderSetRadiologyOrderDetails #txtProvider').attr("ProviderId", "-1");
        $('#pnlOrderSetRadiologyOrderDetails #hfProvider').val("-1");
        $("#pnlOrderSetRadiologyOrderDetails #lnkProviderEdit").css("display", "none");
        $("#pnlOrderSetRadiologyOrderDetails #lblProvider").css("display", "inline");
    },
    // -------------- End Provider -----------------

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: Function to apply bootstrap validations
    validateRadiologyOrderDetail: function () {
        $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails")
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
                   Provider: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Facility: {
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
            if (e.type == "success") {

                var $form = $(e.target);
                var $button = $form.data('bootstrapValidator').getSubmitButton();
                switch ($button.attr('id')) {
                    //Start 28-11-2016 Humaira Yousaf for associated problems validation
                    case 'btnSaveOrder':
                        //var isProblemAdded = OrderSet_RadiologyOrderDetails.problemAdded();
                        //if (isProblemAdded == true) {
                        OrderSet_RadiologyOrderDetails.RadiologyOrderSave('save');
                        //}
                        //else {
                        //    utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}
                        break;
                        //End 28-11-2016 Humaira Yousaf for associated problems validation
                }

            }
            e.type = "";
        });

    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: Binding numpad with height, weight, systolic and diastolic fields
    Init: function () {

        //Start//22-03-2016//Ahmad Raza//Logic to select insurance dropdowns on priority base
        $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlBillingTypeId').on('change', function () {

            if ($(this).val() == 3) {

                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', false);
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', false);
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', false);
                var selectedVal1 = '';
                var selectedVal2 = '';
                var selectedVal3 = '';
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId option').each(function () {
                    if ($(this).attr('Priority') == "1") {
                        selectedVal1 = $(this).val();
                    }

                });
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId').val(selectedVal1);

                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId option').each(function () {
                    if ($(this).attr('Priority') == "2") {
                        selectedVal2 = $(this).val();
                    }
                });
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId').val(selectedVal2);

                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId option').each(function () {
                    if ($(this).attr('Priority') == "3") {
                        selectedVal3 = $(this).val();
                    }
                });
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId').val(selectedVal3);

                //Start//07-03-2016//Abid Ali//set gurantor and relation vlaues
                OrderSet_RadiologyOrderDetails.setGurantorRelationValues(true);
                //End//07-03-2016//Abid Ali//set gurantor and relation vlaues
            }
            else {

                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', true);
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', true);
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', true);


                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId').val('0');
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId').val('0');
                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId').val('0');

                //Start//07-03-2016//Abid Ali//Clear gurantor and relation vlaues
                OrderSet_RadiologyOrderDetails.setGurantorRelationValues(false);
                //End//07-03-2016//Abid Ali//Clear gurantor and relation vlaues
            }

        });
        //End//22-03-2016//Ahmad Raza//Logic to select insurance dropdowns on priority base

        $('.toggleHorSmallLeft section').click(function (e) {
            $(this).parent().toggleClass("toggled");
            OrderSet_RadiologyOrderDetails.toggleHorSmallLeftIcon($(this));

        });

        $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #frmOrderSetRadiologyOrderDetails [data-plugin-keyboard-numpad]').keyboard({
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



    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: Loading ICD Codes for Problem List AutoComplete
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "OrderSet_RadiologyOrderDetails", null, false);
    },


    UnLoad: function (caller) {

        OrderSet_RadiologyOrderDetails.checkedProblems = [];

        var saveButtonisHidden = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #btnSaveOrder").hasClass("hidden");
        var form = '#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails";
        if (caller == 'saveExit' || saveButtonisHidden == true) {
            if (OrderSet_RadiologyOrderDetails.params["ParentCtrl"] == "Clinical_RadiologyOrder") {
                UnloadActionPan(OrderSet_RadiologyOrderDetails.params["ParentCtrl"], "OrderSet_RadiologyOrderDetails", null, OrderSet_RadiologyOrderDetails.params["ParentCtrlPanelID"]);
            }
            else {
                UnloadActionPan(OrderSet_RadiologyOrderDetails.params["ParentCtrl"], "OrderSet_RadiologyOrderDetails");

            }
        }
        else {
            utility.UnLoadDialog(form, function () {
                if (OrderSet_RadiologyOrderDetails.params["ParentCtrl"] == "Clinical_RadiologyOrder") {
                    UnloadActionPan(OrderSet_RadiologyOrderDetails.params["ParentCtrl"], "OrderSet_RadiologyOrderDetails", null, OrderSet_RadiologyOrderDetails.params["ParentCtrlPanelID"]);
                }
                else {
                    UnloadActionPan(OrderSet_RadiologyOrderDetails.params["ParentCtrl"], "OrderSet_RadiologyOrderDetails");
                    //Start 11-08-2016 Edit By Humaira Yousaf Bug# EMR-1927
                    Clinical_RadiologyOrder.radiologyOrderSearch(null, null, null, "RadiologyOrderDetail");
                    //End 11-08-2016 Edit By Humaira Yousaf Bug# EMR-1927
                }
            }, function () {
                if (OrderSet_RadiologyOrderDetails.params["ParentCtrl"] == "Clinical_RadiologyOrder") {
                    UnloadActionPan(OrderSet_RadiologyOrderDetails.params["ParentCtrl"], "OrderSet_RadiologyOrderDetails", null, OrderSet_RadiologyOrderDetails.params["ParentCtrlPanelID"]);
                }
                else {
                    UnloadActionPan(OrderSet_RadiologyOrderDetails.params["ParentCtrl"], "OrderSet_RadiologyOrderDetails");

                }
            });
        }
        OrderSet_ProblemListGrid.ProblemListsSearch();
        $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
    },

    //Function Name: RadiologyOrderSave
    //Author Name: Humaira Yousaf
    //Created Date: 02-03-2016
    //Description: Saves RadiologyOrder
    RadiologyOrderSave: function (sender) {

        var RadiologyOrderId = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #hfRadiologyOrderId").val() != "" ? $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #hfRadiologyOrderId").val() : "-1";
        if (parseInt(RadiologyOrderId) > 0) {
            OrderSet_RadiologyOrderDetails.params.mode = "Edit";
        }
        else {
            OrderSet_RadiologyOrderDetails.params.mode = "Add";
        }

        var self = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails");
        //Start 03-03-2016 Humaira Yousaf to validate title
        var title = self.find("#txtRadiologyOrderTitle").val();
        if (true) {
            //End 03-03-2016 Humaira Yousaf to validate title
            var mainErrorMessage = "";


            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);

            var RadiologyOrderProblemList = [];
            $(self).find("#ulProblemLists td").each(function (index, item) {
                if ($(this).find("input:checkbox").prop("checked")) {
                    var objProblem = {
                        ProblemId: $(this).find("input:checkbox").val(),
                        Description: $(this).text()
                    };
                    objProblem.Description = (objProblem.Description).replace(/%/g, "%25");
                    RadiologyOrderProblemList.push(objProblem);
                }
            });
            objData["RadiologyOrderProblemList"] = RadiologyOrderProblemList;
            //Start 22-03-2016 Humaira Yousaf for status
            if (sender == 'signorder' || sender == 'signprintorder')
                objData["Status"] = 'Signed';
            else if (sender == 'save')
                objData["Status"] = 'Pending';
            //End 22-03-2016 Humaira Yousaf for status
            //objData["CPTSNOMEDCodeId"] = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #hfCPTSNOMEDCode').val();
            //objData["CPTSNOMEDDescription"] = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #hfCPTSNOMEDDescription').val();

            myJSON = JSON.stringify(objData);

            //------------------------------------------------------------
            var RadiologyTestIds = $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #dgvRadiology tbody tr:not([id*=Child]").map(function () {
                return this.id.replace("id", "");
            }).get().join(',');
            $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #pnlRadiology_Result #hfRadiologyTestIds").val(RadiologyTestIds);
            var selfRadiologyTest = $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #pnlRadiology_Result");
            var myJSONRadiologyTest = selfRadiologyTest.getMyJSON();
            var objRad = new Object();
            objRad["RadiologyTestIds"] = RadiologyTestIds;
            JSONToSave = utility.MergeJSON(myJSON, myJSONRadiologyTest);
            var data = JSON.stringify(objRad);
            JSONToSave = utility.MergeJSON(data, JSONToSave);
            JSONToSave = decodeURIComponent(JSONToSave);
            //--------------------------------------------------------------

            OrderSet_RadiologyOrderDetails.cacheRadiologyOrderData();

            if (OrderSet_RadiologyOrderDetails.params.mode == "Add") {

                AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        OrderSet_RadiologyOrderDetails.saveRadiologyOrder(JSONToSave).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {


                                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
                                var isFavListOpened = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #favSectionDiv").hasClass("toggled");
                                EMRUtility.insertUpdateFavListStatus(OrderSet_RadiologyOrderDetails.FavListName, isFavListOpened);
                                //End 31-05-2016 Abid Ali for favorite list setting for all favLists

                                if (OrderSet_RadiologyOrderDetails.params.ParentCtrl == "clinicalTabProgressNote") {
                                    OrderSet_RadiologyOrderDetails.getLatestRadiologyOrderByPatientId(true);
                                }
                                //Start 07-03-2016 Humaira Yousaf to save RadiologyOrder Id
                                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #hfRadiologyOrderId").val(response.radiologicalOrderId);
                                //End 07-03-2016 Humaira Yousaf to save RadiologyOrder Id

                                utility.DisplayMessages(response.message, 1);
                                Clinical_OrderSetDetails.radiologyOrderSearch();
                                OrderSet_RadiologyOrderDetails.UnLoad('saveExit');
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
            else if (OrderSet_RadiologyOrderDetails.params.mode == "Edit") {

                AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        OrderSet_RadiologyOrderDetails.updateRadiologyOrder(JSONToSave, RadiologyOrderId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists

                                var isFavListOpened = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #favSectionDiv").hasClass("toggled");
                                EMRUtility.insertUpdateFavListStatus(OrderSet_RadiologyOrderDetails.FavListName, isFavListOpened);

                                //End 31-05-2016 Abid Ali for favorite list setting for all favLists

                                if (OrderSet_RadiologyOrderDetails.params.ParentCtrl == "clinicalTabProgressNote") {
                                    OrderSet_RadiologyOrderDetails.getLatestRadiologyOrderByPatientId();
                                }
                                //Start 07-03-2016 Humaira Yousaf to save RadiologyOrder Id
                                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #hfRadiologyOrderId").val(response.radiologicalOrderId);
                                //End 07-03-2016 Humaira Yousaf to save RadiologyOrder Id
                                utility.DisplayMessages(response.message, 1);
                                Clinical_OrderSetDetails.radiologyOrderSearch();
                                OrderSet_RadiologyOrderDetails.UnLoad('saveExit');

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
            //      utility.DisplayMessages("Title is required.", 3);
        }
    },

    //Author Name: Abid Ali
    //Created Date: 13-06-2016
    //Description: Saves Radiology order detail data
    cacheRadiologyOrderData: function () {
        //Clinical_RadiologyOrder.params["ProviderName"] = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #txtProvider").val();
        //Clinical_RadiologyOrder.params["ProviderId"] = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #hfProvider").val();

        Clinical_RadiologyOrder.params["AssigneeName"] = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #ddlAssigneeId").val();
        Clinical_RadiologyOrder.params["AssigneeId"] = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #hfAssignee").val();

        Clinical_RadiologyOrder.params["Problems"] = OrderSet_RadiologyOrderDetails.RadiologyOrderProblems;

        Clinical_RadiologyOrder.params["LabId"] = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #ddlLabId").val();
        Clinical_RadiologyOrder.params["BillingTypeId"] = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #ddlBillingTypeId").val();

        //Clinical_RadiologyOrder.params["FacilityName"] = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #txtFacility").val();
        //Clinical_RadiologyOrder.params["FacilityId"] = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #hfFacility").val();


        Clinical_RadiologyOrder.params["CurrentPatientId"] = $('#PatientProfile #hfPatientId').val();
        Clinical_RadiologyOrder.params["hasData"] = true;
    },


    //Function Name: checkRadiologyOrderExists
    //Author Name: Ahmad Raza
    //Created Date: 24-03-2016
    //Description: To check whether Radiology order exists or not
    checkRadiologyOrderExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="RadiologyOrderComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_radiologyorder title="Radiology Order"  id="clinicalMenu_Orders_Radiology" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Radiology\',\'clinicalMenu_Orders_Radiology\',' + Clinical_ProgressNote.params.NotesId + ');" title="Diagnostic Imaging Order">Diagnostic Imaging Order</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Radiology Order\',\'clinicalMenu_Orders_Radiology\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_radiologyorder> </header></li>');
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

    //Function Name: RadiologyOrderSave
    //Author Name: Humaira Yousaf
    //Created Date: 02-03-2016
    //Description: Saves RadiologyOrder
    //Params: RadiologyOrderData
    saveRadiologyOrder: function (RadiologyOrderData) {
        var objData = JSON.parse(RadiologyOrderData);
        // objData["PatientId"] = hfPatientId
        objData["commandType"] = "save_RadiologyOrder";
        objData["OrderSetId"] = OrderSet_RadiologyOrderDetails.params.OrderSetId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");
    },

    //Function Name: loadRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 04-03-2016
    //Description: Loads RadiologyOrder data
    loadRadiologyOrder: function (radiologyOrderId) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (OrderSet_RadiologyOrderDetails.params.mode == "Edit") {
                    $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #hfRadiologyOrderId").val(radiologyOrderId);
                    var self = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID);

                    OrderSet_RadiologyOrderDetails.fillRadiologyOrder(radiologyOrderId).done(function (response) {
                        if (response != "") {
                            var data = JSON.parse(response);
                            if (data.status != false) {
                                var detailJSON = JSON.parse(data.radiologyFill_JSON);

                                var radiologyOrderDetail = JSON.parse(data.radiologyOrderFill_JSON);

                                radiologyOrderDetail = radiologyOrderDetail[0];

                                if (radiologyOrderDetail != null && radiologyOrderDetail.OrderDate != null && radiologyOrderDetail.OrderDate != '') {
                                    function pad(s) { return (s < 10) ? '0' + s : s; }
                                    var d = new Date(radiologyOrderDetail.OrderDate);
                                    radiologyOrderDetail.OrderDate = [pad(d.getMonth() + 1), pad(d.getDate()), d.getFullYear()].join('/');
                                }
                                //Start//25-03-2016//Abid Ali//For bug# EMR-552
                                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #txtRadiologyOrderNo").val(radiologyOrderDetail.OrderNo);
                                $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #txtRadiologyOrderNo").parent().css('display', 'block');
                                //End//25-03-2016//Abid Ali//For bug# EMR-552


                                utility.bindMyJSONByName(true, radiologyOrderDetail, false, self).done(function () {

                                    $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #ddlAssigneeId").val(radiologyOrderDetail.AssigneeId);

                                    OrderSet_RadiologyOrderDetails.EnableDisableTestSearch($('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #ddlLabId"));

                                    var billingType = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlBillingTypeId option:selected').text();

                                    if (billingType.indexOf('Insurance') >= 0) {
                                        OrderSet_RadiologyOrderDetails.enableDisableInsurances(true);
                                    }
                                    else {
                                        OrderSet_RadiologyOrderDetails.enableDisableInsurances(false);
                                    }

                                    OrderSet_RadiologyOrderDetails.RadiologyOrderTestGridLoad(response);

                                });

                                if (data.radiologyOrderProblems_JSON != null) {

                                    data.radiologyOrderProblems_JSON = JSON.parse(data.radiologyOrderProblems_JSON);

                                    for (var index in data.radiologyOrderProblems_JSON) {
                                        $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #ulProblemLists td #chk" + data.radiologyOrderProblems_JSON[index].ProblemId).attr("checked", "checked");
                                    }
                                }

                                if (OrderSet_RadiologyOrderDetails.params.mode == "Edit") {
                                    if (radiologyOrderDetail.Status.toLowerCase() == "signed") {
                                        OrderSet_RadiologyOrderDetails.disableRadiologyOrderControls(true);
                                    }
                                    else {
                                        OrderSet_RadiologyOrderDetails.disableRadiologyOrderControls(false);
                                    }
                                }
                                else {
                                    OrderSet_RadiologyOrderDetails.disableRadiologyOrderControls(false);
                                }

                            }
                        }
                        if (OrderSet_RadiologyOrderDetails.params.mode == "Add") {
                            OrderSet_RadiologyOrderDetails.disableRadiologyOrderControls(false);
                        }
                    });
                }
                //else {
                //    //populate prev saved/updated order details
                //    OrderSet_RadiologyOrderDetails.populateRadiologyOrderSavedData();
                //}


                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
                if (EMRUtility.getFavListStatus(OrderSet_RadiologyOrderDetails.FavListName))
                    $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #favSectionDiv").removeClass("toggled");
                else
                    $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails #favSectionDiv").addClass("toggled");
                //End 10-06-2016 Abid Ali for favorite list setting for all favLists
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    //Author Name: Abid Ali
    //Created Date: 13-06-2016
    //Description: Gets Radiology order detail data
    populateRadiologyOrderSavedData: function () {

        var $form = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #frmOrderSetRadiologyOrderDetails");

        if (Clinical_RadiologyOrder.params["hasData"] == true) {

            //$form.find("#txtProvider").val(Clinical_RadiologyOrder.params["ProviderName"]);
            //$form.find("#hfProvider").val(Clinical_RadiologyOrder.params["ProviderId"]);
            $form.find("#ddlAssigneeId").val(Clinical_RadiologyOrder.params["AssigneeName"]);
            $form.find("#hfAssignee").val(Clinical_RadiologyOrder.params["AssigneeId"]);
            $form.find("#ddlLabId").val(Clinical_RadiologyOrder.params["LabId"]);
            $form.find("#ddlLabId").trigger("onchange");
            $form.find("#ddlBillingTypeId").val(Clinical_RadiologyOrder.params["BillingTypeId"]);

            //Start//Abid Ali/ For bug# EMR-751
            var billingType = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + '  #frmOrderSetRadiologyOrderDetails #ddlBillingTypeId option:selected').text();
            if (billingType.indexOf('Insurance') >= 0) {
                OrderSet_RadiologyOrderDetails.enableDisableInsurances(true);
            }
            else {
                OrderSet_RadiologyOrderDetails.enableDisableInsurances(false);
            }
            //End//Abid Ali/ For bug# EMR-751

            //$form.find("#txtFacility").val(Clinical_RadiologyOrder.params["FacilityName"]);
            //$form.find("#hfFacility").val(Clinical_RadiologyOrder.params["FacilityId"]);

        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To Disable inurances
    enableDisableInsurances: function (enable) {
        if (enable) {
            $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', false);
            $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', false);
            $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', false);

            //Start//Abid Ali//For bug# EMR-1253
            OrderSet_RadiologyOrderDetails.setGurantorRelationValues(true);
            //End//Abid Ali//For bug# EMR-1253
        }
        else {
            $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', true);
            $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', true);
            $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', true);

            //Start//Abid Ali//For bug# EMR-1253
            OrderSet_RadiologyOrderDetails.setGurantorRelationValues(false);
            //End//Abid Ali//For bug# EMR-1253
        }
    },

    //Function Name: fillRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 04-03-2016
    //Description: Fills RadiologyOrder
    //Params: RadiologyOrderId
    fillRadiologyOrder: function (radiologyOrderId) {

        var objData = {};
        objData["commandType"] = "fill_radiologyorder";
        objData["RadiologyOrderId"] = radiologyOrderId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");
    },

    //Function Name: updateRadiologyOrder
    //Author Name: Humaira Yousaf5
    //Created Date: 07-03-2016
    //Description: Updates RadiologyOrder
    //Params: RadiologyOrderData, RadiologyOrderId
    updateRadiologyOrder: function (RadiologyOrderData, RadiologyOrderId) {

        var objData = JSON.parse(RadiologyOrderData);
        objData["RadiologyOrderId"] = RadiologyOrderId;
        objData["OrderSetId"] = OrderSet_RadiologyOrderDetails.params.OrderSetId;
        objData["commandType"] = "save_RadiologyOrder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");

    },

    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: to show RadiologyOrder Alert on Dashboard
    showRadiologyOrderAlert: function (triggerLocation) {

        OrderSet_RadiologyOrderDetails.showRadiologyOrderAlertDBCall(triggerLocation).done(function (response) {
            response = JSON.parse(response);
            //Start//09-03-2016//Ahmad Raza//setting hiddenField values
            $(" #mainForm  li#RadiologyOrderAlert input").val('');
            $(" #mainForm  li#RadiologyOrderAlert input").val(function (i, val) {
                return val + (val ? ', ' : '') + response.RadiologyOrderIDs;
            });
            //End//09-03-2016//Ahmad Raza//setting hiddenField values
            if (response.status != false) {

                if (response.alertCount > 0) {
                    $(" #mainForm  li#RadiologyOrderAlert span").text(response.alertCount);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: DBCall to show RadiologyOrder Alert on Dashboard
    showRadiologyOrderAlertDBCall: function (triggerLocation) {
        var objData = new Object();
        if (triggerLocation == 'FaceSheet') {
            objData["RadiologyOrderTriggerLocation"] = '1';
        }
        else if (triggerLocation == 'Notes') {
            objData["RadiologyOrderTriggerLocation"] = '2';
        }
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "show_RadiologyOrder_alert";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");
    },

    bindAutoComplete: function (element) {
        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "OrderSet_RadiologyOrderDetails", null, true);

    },

    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_RadiologyOrderDetails";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = OrderSet_RadiologyOrderDetails.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, OrderSet_RadiologyOrderDetails.params.PanelID);
    },

    isTestExists: function (procDesc) {
        var currentRow = $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #dgvRadiology tbody tr");
        var isTestAlreadySelected = false;
        $.each(currentRow, function (i, item) {

            //Start//08-06-2016//Modified by Abid Ali
            var test = $(item).find("input[id*='Procedure']").val();
            var testCPT = $(item).find("input[id*='CPTCode']").val();
            var testDescription = $(item).find("input[id*='CPTDescription']").val();

            if (testDescription != null) {
                if (procDesc.toLowerCase().toLowerCase().replace(/\-/gi, '').trim() == testDescription.toLowerCase().replace(/\-/gi, '').trim()) {
                    isTestAlreadySelected = true;
                    return false;
                }
            }

        });
        return isTestAlreadySelected;
    },

    BindProcedureGridItem: function (cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription) {

        var cptCode = cptCode;
        var procDesc = procedureDescription;
        var isTestAlreadySelected = OrderSet_RadiologyOrderDetails.isTestExists(procDesc);

        if (isTestAlreadySelected != true) {
            OrderSet_RadiologyOrderDetails.AddNewRadiologyRow(null, null, null, cptCode, procDesc, cptDescription, SNOMEDId, SNOMEDDescription);
            setTimeout(function () {
                $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #txtRadiologyCPTCode").val('');
            }, 200);
        }
        else {
            utility.DisplayMessages("Test is already selected", 2);
        }
    },

    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div></div>");

        var ddlSpecimen = "<div class='col-xs-2'><label class='control-label'>Specimen</label><select id='Specimen" + ParentRowId + "' name='Specimen' class='form-control' ></select></div>";
        var PatientInstructions = "<div class='col-xs-3'><label class='control-label'>Patient Instructions</label><input type='text' maxlength='500' class='form-control' onkeypress='OrderSet_RadiologyOrderDetails.validateSpecialCharacters(event)'  id='PatientInstructions" + ParentRowId + "'  name='PatientInstructions'></input></div>";
        var Volume = "<div class='col-xs-1'><label class='control-label'>Volume</label><input type='number' onkeydown='OrderSet_RadiologyOrderDetails.validateVolume(event,this)' class='form-control' id='VolumeText" + ParentRowId + "' name='VolumeText'></input></div>";
        var ddlVolume = "<div class='col-xs-2'><label class='control-label'></label><select id='VolumeDDL" + ParentRowId + "' name='VolumeDDL' class='form-control' ddlist='GetVolume'></select></div>";
        var FillerInstructions = "<div class='col-xs-3'><label class='control-label'>Filler Instructions</label><input type='text' maxlength='500' class='form-control' onkeypress='OrderSet_RadiologyOrderDetails.validateSpecialCharacters(event)' id='FillerInstructions" + ParentRowId + "' name='FillerInstructions'></input></div>";


        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(ddlSpecimen, Volume, ddlVolume, PatientInstructions, FillerInstructions);
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
    //Function Name: validateVolume
    //Description: This function will validate the volume input
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
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ChargeCapId"] = ChargeCapId;
                params["mode"] = "Edit";
                //Edited by Azeem Raza Tayyab on 16-Feb-2016 to fix bug#: PMS-3871
                if (OrderSet_RadiologyOrderDetails.params.TabID == 'chargeBatchDetail' || OrderSet_RadiologyOrderDetails.params.TabID == 'billTabUnClaimedAppointment' || OrderSet_RadiologyOrderDetails.params.TabID == 'Bill_ChargeSearch' || OrderSet_RadiologyOrderDetails.params.TabID == 'Patient_Case_Detail' || OrderSet_RadiologyOrderDetails.params.TabID == 'schTabCalendar' || OrderSet_RadiologyOrderDetails.params.TabID == 'batchTabEncounter' || OrderSet_RadiologyOrderDetails.params.TabID == 'Bill_FollowUpPatientAR_Detail' || OrderSet_RadiologyOrderDetails.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || OrderSet_RadiologyOrderDetails.params.TabID == "billTabClaimSubmission" || OrderSet_RadiologyOrderDetails.params.TabID == "Bill_PaymentPosting" || OrderSet_RadiologyOrderDetails.params.TabID == "EDIClaimViewDetail")
                    params["ParentCtrl"] = 'OrderSet_RadiologyOrderDetails';
                else
                    params["ParentCtrl"] = OrderSet_RadiologyOrderDetails.params["TabID"];
                LoadActionPan('chargeSearchDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EnableDisableTestSearch: function (ddlLab) {

        if ($(ddlLab).val() != '') {
            $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #txtRadiologyCPTCode').removeClass('disableAll');
            $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #favSectionDiv').removeClass('disableAll');
        }
        else {
            $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #txtRadiologyCPTCode').addClass('disableAll');
            $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + ' #favSectionDiv').removeClass('disableAll');
        }

    },

    rowRemove: function ($row, obj) {
        utility.myConfirm('1', function () {

            if ($row.hasClass('adding')) {
            }
            //var _self = obj;
            //_self.datatable.row($row.get(0)).remove().draw();
            if (parseInt($row.attr("id")) > 0) {
                $.when(OrderSet_RadiologyOrderDetails.DeleteRadiologyOrderTest($row.attr("id"), $row, obj)).then(function () {
                    OrderSet_RadiologyOrderDetails.enableDisableRadiologyOrderButtons();
                });
            }
            else {
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
                utility.DisplayMessages("Successfully Deleted", 1);
                OrderSet_RadiologyOrderDetails.enableDisableRadiologyOrderButtons();
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

    AddNewRadiologyRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription, SNOMEDId, SNOMEDDescription) {

        var dfdObject = $.Deferred();
        var RadiologyGridId = "#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #dgvRadiology";

        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (OrderSet_RadiologyOrderDetails.params.ParentCtrl != null) {
                CurrentRow = OrderSet_RadiologyOrderDetails.EditableGrid.rowAdd(RowId, "");
            }
            else {
                CurrentRow = OrderSet_RadiologyOrderDetails.EditableGrid.rowAdd(RowId, OrderSet_RadiologyOrderDetails.params.RadiologyId);
            }

        }
        else {
            var TemplateRow = $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #dgvRadiology tbody tr[id*='-']").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            CurrentRow = OrderSet_RadiologyOrderDetails.EditableGrid.rowAdd(TemplateRowId - 1, "");
        }
        var rowId = CurrentRow.attr("id");

        var cptCodeHtml = '<input type="hidden" id="CPTCode' + rowId + '"  name="CPTCode" value="' + cptCode + '" />';
        var cptDescHtml = '<input type="hidden" id="CPTDescription' + rowId + '" name="CPTDescription"  value="' + procDesc + '"  />';
        $(CurrentRow).find('td:first').append(cptCodeHtml + cptDescHtml);

        var snomedCodeHtml = '<input type="hidden" id="CPTSNOMEDCodeId' + rowId + '"  name="CPTSNOMEDCodeId" value="' + SNOMEDId + '" />';
        var snomedDescHtml = '<input type="hidden" id="CPTSNOMEDDescription' + rowId + '" name="CPTSNOMEDDescription"  value="' + SNOMEDDescription + '"  />';
        $(CurrentRow).find('td:first').append(snomedCodeHtml + snomedDescHtml);

        var row = OrderSet_RadiologyOrderDetails.EditableGrid.datatable.row(CurrentRow);
        row.child(OrderSet_RadiologyOrderDetails.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child.hide();
        //, cptCode, procDesc, cptDescription
        if (cptCode != null && cptCode != "") {
            $(CurrentRow).find('input[id*="RadiologyProcedure"]').val(cptCode + " " + procDesc);
        }
        else if (procDesc != null) {
            $(CurrentRow).find('input[id*="RadiologyProcedure"]').val(procDesc.trim());
        }

        OrderSet_RadiologyOrderDetails.enableRemoveRow($(CurrentRow));
        OrderSet_RadiologyOrderDetails.enableDisableRadiologyOrderButtons();

        //Start Farooq Ahmad 07/14/2016 /EMR-588
        var dgvRadiology = $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #dgvRadiology");

        $(dgvRadiology).find('input[id*="dtpRadiologyDate"]').removeClass('size80');
        $(dgvRadiology).find('input[id*="dtpRadiologyDate"]').closest('div').removeClass('size80');

        $(dgvRadiology).find('input[id*="tpRadiologyTime"]').removeClass('size80');
        $(dgvRadiology).find('input[id*="tpRadiologyTime"]').closest('div').removeClass('size80');

        $(dgvRadiology).find('input[id*="RadiologyProcedure"]').addClass('size-min300');
        //End Farooq Ahmad 07/14/2016 /EMR-588

        $(CurrentRow).loadDropDowns(true).done(function () {

            if (row.child().length > 0) {

                row.child().loadDropDowns(true).done(function () {

                    row.child().find('select,datalist').each(function () {
                        var data = null;
                        if ($(this).attr('id').indexOf("Specimen") == 0) {
                            data = {
                                'StrID': cptCode, 'ID': -1, 'StrID2': 'Prefer', 'StrID3': $(this).attr('id')
                            };
                            ddlSpecimen = this;
                            OrderSet_RadiologyOrderDetails.ddlSpecimen.push(this);

                            return MDVisionService.lookups('GetSpecimen', true, data).done(function (results) {
                                results = JSON.parse(results["GetSpecimen"]);
                                if (results) {
                                    var l = null;
                                    for (var count in OrderSet_RadiologyOrderDetails.ddlSpecimen) {
                                        if ($(OrderSet_RadiologyOrderDetails.ddlSpecimen[count]).attr('id') == results.DropDownId) {
                                            l = $(OrderSet_RadiologyOrderDetails.ddlSpecimen[count]);
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
                                dfdObject.resolve(CurrentRow);
                            });
                        }
                        else {
                            return true;
                        }
                    });
                });
            }
            else {
                dfdObject.resolve(CurrentRow);
            }
        });

        return dfdObject.promise();
    },

    //Author: Farooq Ahmad
    //Date: 28-03-2016
    //Reason:to add more problem is Associated Problem list
    addProblem: function () {

        var params = [];
        params["OrderSetId"] = OrderSet_RadiologyOrderDetails.params.OrderSetId;
        params["RefForm"] = "frmOrderSetRadiologyOrderDetails";
        params["FromOrderDetail"] = "1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_RadiologyOrderDetails";
        LoadActionPan('OrderSet_Problems', params);
    },

    GetRadiologyRowsJSON: function () {

        var RadiologyTestIds = $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #dgvRadiology tbody tr:not([id*=Child]").map(function () {
            return this.id.replace("id", "");
        }).get().join(',');

        $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #pnlRadiology_Result #hfRadiologyTestIds").val(RadiologyTestIds);

        var self = $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #pnlRadiology_Result");
        var myJSON = self.getMyJSONByName();

        OrderSet_RadiologyOrderDetails.SaveRadiologyOrderTest(myJSON);

    },

    SaveRadiologyOrderTest: function (objRadiology) {
        OrderSet_RadiologyOrderDetails.RadiologyOrderTestSave_DBCall(objRadiology).done(function (response) {

            var response = JSON.parse(response);
            if (response.status != false) {

            } else {

            }


        });
    },

    RadiologyOrderTestSave_DBCall: function (RadiologyOrderData) {

        if (OrderSet_RadiologyOrderDetails.params.mode.toLowerCase() == "add") {

            var objData = JSON.parse(RadiologyOrderData);
            objData["commandType"] = "save_radiology_order_test";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");

        } else if (OrderSet_RadiologyOrderDetails.params.mode.toLowerCase() == "edit") {
            var objData = JSON.parse(RadiologyOrderData);
            objData["commandType"] = "save_radiology_order_test";
            objData["RadiologyOrderId"] = $("#" + OrderSet_RadiologyOrderDetails.params.PanelID + ' #hfRadiologyOrderId').val();
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");
        }





    },

    RadiologyOrderTestGridLoad: function (response) {
        var response = JSON.parse(response);
        var PanelRadiologyGrid = "#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #pnlRadiology_Result";
        var RadiologyGridId = "#" + OrderSet_RadiologyOrderDetails.params.PanelID + " #dgvRadiology";
        $(RadiologyGridId + " tbody tr").remove();


        if ($.fn.dataTable.isDataTable(RadiologyGridId)) {
            $(RadiologyGridId).dataTable().fnClearTable();
            $(RadiologyGridId).dataTable().fnDestroy();
        }

        OrderSet_RadiologyOrderDetails.EditableGrid.datatable.clear().draw();
        OrderSet_RadiologyOrderDetails.ddlSpecimen = [];
        OrderSet_RadiologyOrderDetails.ddlAlternativeSpecimen = [];
        var RadiologyOrderTestLoadJSONData = JSON.parse(response.radiologyOrderTest_JSON);

        $.each(RadiologyOrderTestLoadJSONData, function (i, item) {

            var dfd = $.Deferred();

            var CurrentRow = null;
            var newChildRow = null;

            var createCurrentRow = function (i, item, CurrentRow, newChildRow) {

                var RadiologyOrderTestId = item.RadiologyOrderTestId;

                OrderSet_RadiologyOrderDetails.AddNewRadiologyRow(RadiologyOrderTestId, null, null, item.CPTCode, item.CPTDescription, null, null, null).done(function (parentRow) {

                    CurrentRow = parentRow;
                    var row = OrderSet_RadiologyOrderDetails.EditableGrid.datatable.row(CurrentRow);
                    item.CurrentRow = CurrentRow;
                    newChildRow = row.child();
                    item.newChildRow = newChildRow;

                    dfd.resolve(CurrentRow, newChildRow, item);
                });
                return dfd.promise();
            }

            //Start Farooq Ahmad 03/28/2016 bind values to the table

            var BindFunction = function (CurrentRow, newChildRow, item) {
                if (CurrentRow == null) {
                    CurrentRow = item.CurrentRow;
                }
                if (newChildRow == null) {
                    newChildRow = item.newChildRow;
                }
                utility.bindMyJSONByName(true, item, false, $(CurrentRow)).done(function () {
                    utility.bindMyJSONByName(true, item, false, $(newChildRow));
                });
                //show button controls
                OrderSet_RadiologyOrderDetails.enableDisableRadiologyOrderButtons();
            }

            //End Farooq Ahmad 03/28/2016 bind values to the table
            $.when(createCurrentRow(i, item, CurrentRow, newChildRow)).done(function (CurrentRow, newChildRow, item) {
                BindFunction(CurrentRow, newChildRow, item);
            });
        });
    },

    DeleteRadiologyOrderTest: function (RadiologyTestId, $row, obj) {
        var dfd = $.Deferred();
        OrderSet_RadiologyOrderDetails.RadiologyOrderTest_DBCall(RadiologyTestId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
                dfd.resolve();
            } else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }

        });
        return dfd;
    },

    RadiologyOrderTest_DBCall: function (RadiologyTestId) {

        var objData = new Object();
        objData["RadiologyOrderTestId"] = RadiologyTestId;
        objData["commandType"] = "DELETE_RADIOLOGYORDER_TEST";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");
    },


    //Function Name: getLatestRadiologyOrderByPatientId
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: get Latest Radiology Order By PatientId
    getLatestRadiologyOrderByPatientId: function (hideAlertMessage) {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            OrderSet_RadiologyOrderDetails.getLatestRadiologyOrderByPatientIdDBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    OrderSet_RadiologyOrderDetails.createRadiologyOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
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

    //Function Name: createRadiologyOrderBodyHTML
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To create Radiology Order's Body HTML
    createRadiologyOrderBodyHTML: function (response, NoteHTMLCtrl, UnloadRadiologyOrder, hideAlertMessage) {
        OrderSet_RadiologyOrderDetails.checkRadiologyOrderExists();
        if (response.RadiologyOrderFill_JSON != null && response.RadiologyOrderFill_JSON != '') {
            var RadiologyOrderFill_Obj = JSON.parse(response.RadiologyOrderFill_JSON);
            var $mainDivRadiologyOrder = $(document.createElement('div'));

            var RadiologyOrderId = RadiologyOrderFill_Obj.RadiologyOrderId;
            if (RadiologyOrderId > 0) {
                var $SectionBodyRadiologyOrder = $(document.createElement('section'));
                $SectionBodyRadiologyOrder.attr('id', "Cli_RadiologyOrderDetail_Main" + RadiologyOrderId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_RadiologyOrderDetail_" + RadiologyOrderId);
                var $ListRadiologyOrder = $(document.createElement('ul'));

                $ListRadiologyOrder.attr('class', 'list-unstyled')

                $SectionBodyRadiologyOrder.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_RadiologyOrderDetail_" + RadiologyOrderId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_RadiologyOrderDetail_Main" + RadiologyOrderId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListRadiologyOrder.append("<li>" + RadiologyOrderFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListRadiologyOrder);
                $SectionBodyRadiologyOrder.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + RadiologyOrderId).length == 0) {
                    $mainDivRadiologyOrder.append($SectionBodyRadiologyOrder);
                    OrderSet_RadiologyOrderDetails.updateRadiologyOrderHtml($mainDivRadiologyOrder.html(), RadiologyOrderId, NoteHTMLCtrl, hideAlertMessage);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + RadiologyOrderId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_radiology').parent().parent().find('#Cli_RadiologyOrder_Main' + RadiologyOrderId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + RadiologyOrderId).html($SectionBodyRadiologyOrder.html());
                    $(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + RadiologyOrderId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText("Radiology Order", hideAlertMessage);
                    OrderSet_RadiologyOrderDetails.updateRadiologyOrderHtml("", RadiologyOrderId, NoteHTMLCtrl, hideAlertMessage);

                }

                if (UnloadRadiologyOrder == true) {
                    OrderSet_RadiologyOrderDetails.Unload(OrderSet_RadiologyOrderDetails.bNextPrev);
                }
            }
        }
    },

    //Function Name: detach_ComponentsRadiologyOrder
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To detach Components Radiology Order
    detach_ComponentsRadiologyOrder: function (ComponentName, IsUpdate, RadiologyOrderComponentRemove) {
        var radiologyOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().find('section[id*="Cli_RadiologyOrderDetail_Main"]').map(function () {
            return this.id.replace("Cli_RadiologyOrderDetail_Main", "");
        }).get().join(',');
        if (radiologyOrderIds == "" || radiologyOrderIds == "undefined") {
            Clinical_ProgressNote.saveComponentSOAPText("Radiology Order", true);
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {


            OrderSet_RadiologyOrderDetails.detachRadiologyOrderFromNotesDBCall(radiologyOrderIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText("Radiology Order", true);
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
        if (RadiologyOrderComponentRemove) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Radiology Order']").remove();
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().remove();
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().find('section[id*="Cli_RadiologyOrderDetail_Main"]').remove();
        }
    },

    //Function Name: detachRadiologyOrderFromNotes
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To detach Radiology Order from Notes
    detachRadiologyOrderFromNotes: function (RadiologyOrderId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = RadiologyOrderId.replace('Cli_RadiologyOrderDetail_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    OrderSet_RadiologyOrderDetails.detachRadiologyOrderFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + RadiologyOrderId).remove();
                            Clinical_ProgressNote.saveComponentSOAPText('Radiology Order');
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

    //Function Name: detachRadiologyOrderFromNotes_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to detach Radiology Order from Notes
    detachRadiologyOrderFromNotes_DBCall: function (RadiologyOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyOrderId"] = RadiologyOrderId;
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
        objData["commandType"] = "detach_radiologyorder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");
    },

    //Function Name: detachRadiologyOrderFromNotesDBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to detach Radiology Order From Notes
    detachRadiologyOrderFromNotesDBCall: function (RadiologyId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyOrderId"] = RadiologyId;
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
        objData["commandType"] = "detach_RadiologyOrder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");
    },

    //Function Name: updateRadiologyOrderHtml
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To update Radiology Order Html
    updateRadiologyOrderHtml: function (RadiologyOrderHtml, RadiologyOrderId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().addClass('initialVisitBody');
        if (RadiologyOrderHtml != '') {
            $(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().append(RadiologyOrderHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (RadiologyOrderHtml != '') {
            OrderSet_RadiologyOrderDetails.attachRadiologyOrderWithNotes(RadiologyOrderId, hideAlertMessage);
        }

    },

    //Function Name: attachRadiologyOrderWithNotes
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To attach Radiology Order With Notes
    attachRadiologyOrderWithNotes: function (RadiologyOrderId, hideAlertMessage) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var selectedValue = RadiologyOrderId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                OrderSet_RadiologyOrderDetails.attachRadiologyOrderWithNotesDBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //If Attached MedicalHx Made new inseration to MedicalHx Table than good ids should be attached to HTML
                        Clinical_ProgressNote.saveComponentSOAPText('Radiology Order', hideAlertMessage);
                        $('#' + RadiologyOrderId).remove();
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

    //Function Name: attachRadiologyOrderWithNotesDBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to attach Radiology Order With Notes
    attachRadiologyOrderWithNotesDBCall: function (RadiologyOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyOrderId"] = RadiologyOrderId;
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
        objData["commandType"] = "attach_RadiologyOrder_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");
    },

    //Function Name: getLatestRadiologyOrderByPatientIdDBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to get Latest Radiology Order By PatientId
    getLatestRadiologyOrderByPatientIdDBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_Radiologyorderby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "RadiologyOrder");
    },

    getRadiologyOrderInfo: function (radiologyOrderId) {
        OrderSet_RadiologyOrderDetails.fillRadiologyOrder(radiologyOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    OrderSet_RadiologyOrderDetails.createRadiologyOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },
    //Function Name: printRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: Creates PDF to view Radiology Order
    printRadiologyOrder: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = "OrderSet_RadiologyOrderDetails";
        params["RadiologyOrderId"] = $('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #hfRadiologyOrderId").val();
        LoadActionPan('Clinical_RadiologyOrderView', params);
    },

    //Author: Humaira Yousaf
    //Date :  28-11-2016
    //Description: Checks if any associated problems is selected on saving order
    problemAdded: function () {

        var problemsSelected = false;
        if ($('#' + OrderSet_RadiologyOrderDetails.params.PanelID + " #ulProblemLists input:checked").length > 0) {
            problemsSelected = true;
        }

        return problemsSelected;
    },
}