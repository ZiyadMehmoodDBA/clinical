ClinicalConsultationOrderDetail = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    problems: [],
    ConsultationOrderProblems: [],
    FavListName: 'ClinicalConsultationOrder',
    Load: function (params) {
        BackgroundLoaderShow(true);
        params["TabID"] = 'ClinicalConsultationOrderDetail';
        ClinicalConsultationOrderDetail.params = params;
        ClinicalConsultationOrderDetail.favoriteListSearch();

        var PanelConsultationGrid = "#" + ClinicalConsultationOrderDetail.params.PanelID + " #pnlConsultation_Result";
        var ConsultationGridId = "#" + ClinicalConsultationOrderDetail.params.PanelID + " #dgvConsultation";
        //$(ChargeGridId).dataTable().fnDestroy();
        $(ConsultationGridId + " tbody tr").remove();


        ClinicalConsultationOrderDetail.EditableGrid = EMRUtility.MakeEditableGrid(PanelConsultationGrid, ConsultationGridId, ClinicalConsultationOrderDetail, "0", false, false, false, false);



        $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        var self = $('#tblClinicalConsultationOrderDetail');

        // Set Title Explicitly if it's passed as Parameter
        if (ClinicalConsultationOrderDetail.params.Title != null)
            $("#" + ClinicalConsultationOrderDetail.params["PanelID"] + " #headingTitle").text(ClinicalConsultationOrderDetail.params.Title);
        //Start//03-03-2016//Ahmad Raza//Loading Allergies for AutoComplete
        var element = $("#" + ClinicalConsultationOrderDetail.params["PanelID"] + " #txtConsultationOrderAllergies");
        EMRUtility.bindAutoCompleteAllergies(element);
        //End//03-03-2016//Ahmad Raza//Loading Allergies for AutoComplete

        //Start//03-03-2016//Ahmad Raza//Loading Medications for AutoComplete
        var element = $("#" + ClinicalConsultationOrderDetail.params["PanelID"] + " #txtConsultationOrderMedications");
        EMRUtility.bindAutoCompleteMedications(element);
        //End//03-03-2016//Ahmad Raza//Loading Medications for AutoComplete


        //Start 02-03-2016 Humaira Yousaf to load dropdowns
        if (ClinicalConsultationOrderDetail.bIsFirstLoad == true) {
            ClinicalConsultationOrderDetail.bIsFirstLoad = false;
            //Strat 17-03-2016 Farooq AHmad load problem list

            //End 17-03-2016 Farooq Ahmad load problem list
            //Strat 17-03-2016 Farooq AHmad validate consultation order detail
            ClinicalConsultationOrderDetail.validateConsultationOrderDetail();
            //End 17-03-2016 Farooq AHmad validate consultation order detail

            //Strat 11-05-2016 Edit By Humaira Yousaf Bug# EMR-628
            ClinicalConsultationOrderDetail.loadAllAutocomplete();
            //End 11-05-2016 Edit By Humaira Yousaf Bug# EMR-628

            self.loadDropDowns(true).done(function () {

                $.when(ClinicalConsultationOrderDetail.loadProblemList()).done(function (response) {

                    if (response != "") {
                        //Start 04-03-2016 Humaira Yousaf to load ConsultationOrder
                        if (ClinicalConsultationOrderDetail.params.mode == "Edit")
                            ClinicalConsultationOrderDetail.loadConsultationOrder();
                        else {
                            ClinicalConsultationOrderDetail.disableConsultationOrderControls(false);
                            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #hfConsultationOrderID").val("1");
                        }
                        $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
                        //End 04-03-2016 Humaira Yousaf to load ConsultationOrder
                    }
                });

                //Start//04-03-2016//Ahmad Raza//Logic to show/hide divs  on multilist selection change
                $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ddlConsultationOrderRuleType").multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onChange: function (element, checked) {
                        var selectedValue = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ddlConsultationOrderRuleType option:selected");
                        var selected = [];
                        $(selectedValue).each(function (index, val) {
                            selected.push($(this).text());
                        });
                        var unSelect = '';
                        $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail .comList").addClass('hidden');
                        $(selected).each(function (i, item) {
                            var sectionName = item;
                            unSelect += sectionName + ',';
                            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #dv" + item.replace(/\s/g, '')).removeClass('hidden');
                        });
                    }
                });
                //End//04-03-2016//Ahmad Raza//Logic to show/hide divs  on multilist selection change

            });
        }
        //End 02-03-2016 Humaira Yousaf to load dropdowns

        //Start 03-03-2016 Humaira Yousaf to load dropdowns

        //Start 21-03-2016 Humaira Yousaf to create datepickers and timer
        utility.CreateDatePicker(ClinicalConsultationOrderDetail.params.PanelID + ' #dtOrderDate', function () {
            //on-change callback method
        }, true);
        $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #tmOrderTime').timepicker({
            defaultTime: new Date()
        });
        //End 21-03-2016 Humaira Yousaf to create datepickers and timer

        ClinicalConsultationOrderDetail.domReadyFunction();
        $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
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
    //toggleHorSmallLeftIcon();

    //Author: Muhammad Arshad
    //Date :  25-03-2016
    //Reason: Function to disable Consultation Order Controls
    disableConsultationOrderControls: function (IsSigned) {
        var detailsDivs = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #divConsultationOrderInformation,#divConsultationTestInformation,#divConsultationAssociatedProblems");
        var detailsButtons = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnConsultationReset");
        var printRequisitionButton = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #btnPrintOrder");
        if (IsSigned == true) {
            detailsDivs.addClass("disableAll");
            detailsButtons.addClass("hidden");
            printRequisitionButton.removeClass("hidden");
        }
        else {
            detailsDivs.removeClass("disableAll");
            detailsButtons.removeClass("hidden");
            printRequisitionButton.addClass("hidden");
        }
    },

    //Author: Muhammad Arshad
    //Date :  24-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteListSearch: function () {

        Favorite_ProcedureOrder.searchFavoriteList_DBCall("ConsultationOrder", null, 1, 5000).done(function (response) {
            response = JSON.parse(response);

            var $ddl = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #ddlFavoriteListConsultationOrder');
            $ddl.empty();
            $ddl.append($('<option/>', {
                value: "",
                html: "- Select -"
            }));
            if (response.status != false) {
                var favouriteProcedures = JSON.parse(response.FavoriteListJSON)
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
                    EMRUtility.getFavListValue(ClinicalConsultationOrderDetail.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "" && response1.favListVal != "-1") {
                                if ($("#" + ClinicalConsultationOrderDetail.params.PanelID + " #ddlFavoriteListConsultationOrder option[value='" + response1.favListVal + "']").length > 0) {
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
                        $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
                    });

                }
                var mystr = "";
                $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });

    },

    //Author: Muhammad Arshad
    //Date :  25-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    selectAllFavoriteListContent: function () {

        $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #ulFavoriteListConsultationOrderContent li').each(function (i, item) {
            $(item).trigger("click");
        });


    },
    AutoSearchFavConsultationOrder: function () {
        utility.Keyupdelay(function () {
            ClinicalConsultationOrderDetail.loadfavoriteListContent();
        });
    },
    //Author: Muhammad Arshad
    //Date :  24-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    loadfavoriteListContent: function (obj, favListIds) {
        if ((typeof favListIds == typeof undefined || favListIds == null) && (typeof obj == typeof undefined || obj == null)) {
            obj = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #ddlFavoriteListConsultationOrder');
        }
        var SearchData = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #FavSearchBox').val();
        var $uL = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #ulFavoriteListConsultationOrderContent');
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            var selectedOptionValue = selectedOption.attr("id");
            //Start 12-07-2016 Muhammad Arshad Bug#EMR-1486 Lab order -> Favorite list -> "Select ALL " functionality is not working on 46 server ,please see attached video and resolve the issue
            var divSelectAllFavorites = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #divSelectAllConsultationOrderFavorites');
            if (selectedOptionValue > 0) {
                divSelectAllFavorites.removeClass("disableAll");

                ClinicalConsultationOrderDetail.favoriteList_CPTSearch(selectedOptionValue, SearchData);
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

                $uL.empty();

                $.each(favListIds, function (index, item) {
                    //load all favList
                    ClinicalConsultationOrderDetail.favoriteList_CPTSearch(item, false);
                });
            }
        }
    },

    //Author: Muhammad Arshad
    //Date :  24-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteList_CPTSearch: function (FavoriteListId, SearchData) {
        var $UL = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #ulFavoriteListConsultationOrderContent');
        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(null, FavoriteListId, 1, 5000, SearchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var objData = JSON.parse(response.FavoriteListCPTJSON);
                $UL.empty();

                $.each(objData, function (i, item) {
                    if (item.CPTCodeDescription != "") {
                        var onclick = 'ClinicalConsultationOrderDetail.BindProcedureGridItem(\'' + item.CPTCode + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + String(item.CPTCodeDescription) + '\')';
                        $UL.append('<li onclick="' + onclick + '">' + item.CPTCodeDescription + '</li>');
                    }
                });

                $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
                //Favorite_ProcedureOrder.FavoriteListCPTGridLoad(response);
            }
            else {
                $UL.empty();
            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });
    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: Function to load problem list
    loadProblemList: function () {

        var dfd = $.Deferred();
        ClinicalConsultationOrderDetail.SearchProblemList().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ProblemListCount > 0) {
                    var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
                    var ProblemListHistoryLoadJSONData = JSON.parse(response.ProblemListHistoryLoad_JSON);

                    var finalTr = '';
                    var counter = 2;
                    $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #ulProblemLists tbody tr").remove();
                    $.each(ProblemListLoadJSONData, function (i, item) {
                        if (item.Description.split("-")[2] != undefined) {
                            item.Description = item.Description.substring(item.Description.indexOf("-") + 2);
                        }
                        if (counter % 2 == 0) {
                            finalTr = finalTr + '<tr>';
                        }
                        finalTr = finalTr + '<td><div class="p-xs col-xs-12"><div class="checkbox-custom">';

                        finalTr = finalTr + '<input type="checkbox" name="' + item.Code + '" value="' + item.ProblemListId + '" id="chk' + item.ProblemListId + '">';
                        finalTr = finalTr + '   <label for="chk' + item.ProblemListId + '" class="control-label">' + item.Description + '</label></div></div></td>';

                        if (counter % 2 == 1) {
                            finalTr = finalTr + '</tr>';
                        }
                        counter++;
                        var color = "";
                    });
                    $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #ulProblemLists tbody").append(finalTr);
                }
                $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
                //if (ClinicalConsultationOrderDetail.params.mode == "Add") {

                //    if (ClinicalConsultationOrderDetail.problems.length > 0) {
                //        for (var i = 0; i < ClinicalConsultationOrderDetail.problems.length; i++) {
                //            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #ulProblemLists td #chk" + ClinicalConsultationOrderDetail.problems[i]).attr("checked", "checked");
                //        }
                //    }
                //}
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            return dfd.resolve("ok");
        });
        return dfd.promise();
    },

    //Author: Farooq Ahmad
    //Date: 28-03-2016
    //Reason:to add more problem is Associated Problem list
    addProblem: function () {
        ClinicalConsultationOrderDetail.SaveLocalyCheckedProbems();
        var params = [];
        // Ast 357 
        params["IsFromNote"]= ClinicalConsultationOrderDetail.params["IsFromNote"];
        params["CurrentNotesProviderId"] = ClinicalConsultationOrderDetail.params["CurrentNotesProviderId"];
        params["RefForm"] = "frmClinicalConsultationOrderDetail";
        params["FromOrderDetail"] = "1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalConsultationOrderDetail";
        LoadActionPan('Clinical_ProblemLists', params);
    },

    SaveLocalyCheckedProbems: function () {
        var self = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail");
        ClinicalConsultationOrderDetail.ConsultationOrderProblems = [];
        $(self).find("#ulProblemLists td").each(function (index, item) {
            if ($(this).find("input:checkbox").prop("checked")) {
                var objProblem = {
                    ProblemId: $(this).find("input:checkbox").val(),
                    Description: $(this).text()
                }
                ClinicalConsultationOrderDetail.ConsultationOrderProblems.push(objProblem);
            }
        });
    },
    CheckedPreviousProbems: function () {
        var dfd = $.Deferred();
        for (var index in ClinicalConsultationOrderDetail.ConsultationOrderProblems) {
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #ulProblemLists td #chk" + ClinicalConsultationOrderDetail.ConsultationOrderProblems[index].ProblemId).attr("checked", "checked");
        }
        dfd.resolve();
        return dfd.promise();
    },
    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: Function to load problem list
    SearchProblemList: function () {


        var IsCheckedIn = null;
        IsCheckedIn = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }

        var PageNumber = 1;
        var RowsPerPage = 2000;

        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = '1';
        // objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        //objData["NoteId"] = Clinical_ProblemLists.params.NotesId == null ? 0 : Clinical_ProblemLists.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },

    //Start//23/03/2016//Abid Ali//Implimented Save Fav ConsultationOrder
    favConsultationOrderSave: function (callFromDelete, callFromUnLoad) {

        if (ClinicalConsultationOrderDetail.CPTData.length > 0) {
            //var myJSON = JSON.stringify(Favorite_ProcedureOrderDetail.FavProcedureOrder);
            //AppPrivileges.GetFormPrivileges("Medical_ProcedureOrder", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            // if (strMessage == "") {
            ClinicalConsultationOrderDetail.saveFavConsultationOrder().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (!callFromDelete) {
                        utility.DisplayMessages(response.Message, 1);
                    }

                }
                else {
                    if (!callFromUnLoad) {
                        utility.DisplayMessages(response.Message, 3);
                    }
                }
            });
        }
        else {
            utility.DisplayMessages("There is no Favorite Complaint to add", 3);/// ask from babur bhai
        }
    },
    //End Abid Ali /22/03/2016

    //Start//23/03/2016//Abid Ali//Implimented Call for Fav ConsultationOrder
    saveFavConsultationOrder: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#pnlClinicalConsultationOrderDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);
        objData["ListType"] = "ConsultationOrder";
        objData["EntityId"] = objDetail["EntityId"];
        objData["FavoriteListName"] = objDetail["FavoriteListName"];
        if (globalAppdata['AppUserName'] != DefaultUser) {
            objData["EntityId"] = -1;
        }
        objData["FavoriteListCPT"] = ClinicalConsultationOrderDetail.CPTData;
        objData["commandType"] = "SAVE_FavConsultationOrder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    //End Abid Ali /22/03/2016


    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: Function to apply bootstrap validations
    validateConsultationOrderDetail: function () {
        $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ConsultationOrderTitle: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Provider: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Facility: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Assignee: {
                       group: '.col-sm-3',
                       enabled: false,
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
                        ClinicalConsultationOrderDetail.consultationOrderSave('save');
                        break;
                    case 'btnSignOrder':
                        ClinicalConsultationOrderDetail.consultationOrderSave('signorder');
                        ClinicalConsultationOrderDetail.disableConsultationOrderControls(true);
                        break;
                    case 'btnPrintOrder':
                        ClinicalConsultationOrderDetail.printConsultationOrder();
                        break;
                    case 'btnSignPrintOrder':
                        ClinicalConsultationOrderDetail.consultationOrderSave('signprintorder');
                        ClinicalConsultationOrderDetail.disableConsultationOrderControls(true);
                        break;
                    default:
                        ClinicalConsultationOrderDetail.consultationOrderSave('save');
                        break;
                }
                //End 22-03-2016 Humaira Yousaf for multiple submit buttons
            }
            e.type = "";
        });

    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: open facility form
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalConsultationOrderDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalConsultationOrderDetail";
        LoadActionPan('Admin_Facility', params);
    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: open facility detail form
    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#pnlClinicalConsultationOrderDetail #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'ClinicalConsultationOrderDetail';
        LoadActionPan('facilityDetail', params);
    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: open provider form
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalConsultationOrderDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalConsultationOrderDetail";
        LoadActionPan('Admin_Provider', params);
    },

    //Function Name: OpenAssignee
    //Author Name: Humaira Yousaf
    //Created Date: 18-03-2016
    //Description: auto complete for assignee
    OpenAssignee: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmClinicalConsultationOrderDetail";
        params["AssigneeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalConsultationOrderDetail";
        params["RefCtrl"] = "txtAssignee";
        params["RefCtrlHidden"] = "hfAssignee";
        params["RefCtrlLink"] = "lnkAssignee";
        LoadActionPan('Admin_Provider', params);
    },

    //author: farooq ahmad
    //date :  17-03-2016
    //reason: open provider form
    openproviderdetail: function () {
        //admin_provider.provideredit($('#pnlclinicalnotes #hfprovider').val(),'clinicaltabnotes');
        var params = [];
        params["providerid"] = $('#pnlclinicalconsultationorderdetail #hfprovider').val();
        params["mode"] = "edit";
        params["fromadmin"] = "0";
        params["refctrl"] = "txtprovider";
        params["parentctrl"] = 'clinicalconsultationorderdetail';
        loadactionpan('providerdetail', params);
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
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail [data-plugin-keyboard-numpad]').keyboard({
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
            WeightInLbs = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #txtBSA");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var weightInKG = ClinicalConsultationOrderDetail.convertWeight(weightInLbs);
        var heightIn_cm = ClinicalConsultationOrderDetail.convertHeightTo_cm(heightInFeet);
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
            WeightInLbs = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #txtBMI");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var heightInInches = ClinicalConsultationOrderDetail.convertHeightInches(heightInFeet);

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
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "ClinicalConsultationOrderDetail", null, false);
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
        if (ClinicalConsultationOrderDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'ClinicalConsultationOrderDetail';
        }

        else {
            params["ParentCtrl"] = ClinicalConsultationOrderDetail.params["TabID"];

        }
        params["PanelID"] = ClinicalConsultationOrderDetail.params["PanelID"];

        params["ActionPanContainer"] = ClinicalConsultationOrderDetail.params["ActionPanContainer"];
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (ClinicalConsultationOrderDetail.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params, ClinicalConsultationOrderDetail.params.PanelID);
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
        var saveButtonisHidden = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #btnSaveOrder").hasClass("hidden");
        //Start 03-03-2016 Humaira Yousaf to close form after save
        if (caller == 'saveExit' || saveButtonisHidden == true) {
            if (ClinicalConsultationOrderDetail.params.ParentCtrlPanelID != "undefined" && ClinicalConsultationOrderDetail.params.ParentCtrlPanelID !="")
                UnloadActionPan(ClinicalConsultationOrderDetail.params["ParentCtrl"], "ClinicalConsultationOrderDetail", null, ClinicalConsultationOrderDetail.params.ParentCtrlPanelID);
            else
                UnloadActionPan(ClinicalConsultationOrderDetail.params["ParentCtrl"], "ClinicalConsultationOrderDetail");
        }
            //End 03-03-2016 Humaira Yousaf to close form after save
        else {
            utility.UnLoadDialog("frmClinicalConsultationOrderDetail", function () {
                UnloadActionPan(ClinicalConsultationOrderDetail.params["ParentCtrl"], "ClinicalConsultationOrderDetail", null, ClinicalConsultationOrderDetail.params["PanelID"]);
            }, function () {
                UnloadActionPan(ClinicalConsultationOrderDetail.params["ParentCtrl"], "ClinicalConsultationOrderDetail", null, ClinicalConsultationOrderDetail.params["PanelID"]);
            });
        }
        $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
    },



    //Function Name: consultationOrderSave
    //Author Name: Humaira Yousaf
    //Created Date: 18-03-2016
    //Description: Saves Consultation Order
    consultationOrderSave: function (sender) {
        var favVal = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #ddlFavoriteListConsultationOrder').val();
        var ConsultationOrderId = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #hfConsultationOrderID").val() != "" ? $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #hfConsultationOrderID").val() : "-1";
        if (parseInt(ConsultationOrderId) > 0) {
            ClinicalConsultationOrderDetail.params.mode = "Edit";
        }
        else {
            ClinicalConsultationOrderDetail.params.mode = "Add";
        }

        var self = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail");

        var mainErrorMessage = "";

        if (mainErrorMessage == "") {
            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);

            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
            var ConsultationOrderProblemList = [];
            $(self).find("#ulProblemLists td").each(function (index, item) {
                if ($(this).find("input:checkbox").prop("checked")) {
                    var objProblem = {
                        ProblemId: $(this).find("input:checkbox").val(),
                        Description: $(this).text()
                    }
                    ConsultationOrderProblemList.push(objProblem);
                }
            });
            objData["ConsultationOrderProblemList"] = ConsultationOrderProblemList;
            //Start 22-03-2016 Humaira Yousaf for status
            if (sender == 'signorder' || sender == 'signprintorder')
                objData["Status"] = 'Signed';
            else if (sender == 'save')
                objData["Status"] = 'Draft';
            //End 22-03-2016 Humaira Yousaf for status
            //objData["CPTSNOMEDCodeId"] = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #hfCPTSNOMEDCode').val();
            //objData["CPTSNOMEDDescription"] = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #hfCPTSNOMEDDescription').val();

            myJSON = JSON.stringify(objData);

            //------------------------------------------------------------
            var ConsultationTestIds = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #dgvConsultation tbody tr").map(function () {
                return this.id.replace("id", "");
            }).get().join(',');
            $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #pnlConsultation_Result #hfConsultationTestIds").val(ConsultationTestIds);
            var selfConsultationTest = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #pnlConsultation_Result");
            var myJSONConsultationTest = selfConsultationTest.getMyJSON();
            var objRad = new Object();
            objRad["ConsultationTestIds"] = ConsultationTestIds;
            JSONToSave = utility.MergeJSON(myJSON, myJSONConsultationTest);
            var data = JSON.stringify(objRad);
            JSONToSave = utility.MergeJSON(data, JSONToSave);
            JSONToSave = decodeURIComponent(JSONToSave);
            //--------------------------------------------------------------

            if (ClinicalConsultationOrderDetail.params.mode == "Add") {

                AppPrivileges.GetFormPrivileges("Orders and Results_Consultation", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        ClinicalConsultationOrderDetail.saveConsultationOrder(JSONToSave).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                var isFavListOpened = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #favSectionDiv").hasClass("toggled");
                                $.when(EMRUtility.insertUpdateFavListStatus(ClinicalConsultationOrderDetail.FavListName, !isFavListOpened)).then(function () {
                                    ClinicalConsultationOrderDetail.SaveFavListVal(favVal);
                                });

                                if (ClinicalConsultationOrderDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                    ClinicalConsultationOrderDetail.getLatestConsultationOrderByPatientId(true);
                                }
                                utility.DisplayMessages(response.Message, 1);
                                $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #hfConsultationOrderID").val(response.ConsultationOrderId);


                                Clinical_ConsultationOrder.consultationOrderSearch(null, null, null, "ConsultationOrderDetail");
                                //Start 23-03-2016 Humaira Yousaf for pdf
                                if (sender == 'signprintorder') {
                                    ClinicalConsultationOrderDetail.printConsultationOrder();
                                }
                                else {
                                    ClinicalConsultationOrderDetail.UnLoad('saveExit');
                                    $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                                    // AST-390 load provider.
                                    Clinical_ConsultationOrder.fillProvider();
                                }
                                //End 23-03-2016 Humaira Yousaf for pdf
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
            else if (ClinicalConsultationOrderDetail.params.mode == "Edit") {

                AppPrivileges.GetFormPrivileges("Orders and Results_Consultation", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        ClinicalConsultationOrderDetail.updateConsultationOrder(JSONToSave, ConsultationOrderId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                var isFavListOpened = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #favSectionDiv").hasClass("toggled");
                                $.when(EMRUtility.insertUpdateFavListStatus(ClinicalConsultationOrderDetail.FavListName, !isFavListOpened)).then(function () {
                                    ClinicalConsultationOrderDetail.SaveFavListVal(favVal);
                                });
                                if (ClinicalConsultationOrderDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                    ClinicalConsultationOrderDetail.getLatestConsultationOrderByPatientId();
                                } else {
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #hfConsultationOrderID").val(response.ConsultationOrderId);

                                Clinical_ConsultationOrder.consultationOrderSearch(null, null, null, "ConsultationOrderDetail");
                                //Start 23-03-2016 Humaira Yousaf for pdf
                                if (sender == 'signprintorder') {
                                    ClinicalConsultationOrderDetail.printConsultationOrder();
                                }
                                else {
                                    ClinicalConsultationOrderDetail.UnLoad('saveExit');
                                    $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                                    // AST-390 load provider.
                                    Clinical_ConsultationOrder.fillProvider();
                                }
                                //End 23-03-2016 Humaira Yousaf for pdf
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
        }
        else {
            utility.DisplayMessages(mainErrorMessage, 3);
        }
    },
    SaveFavListVal: function (FavListVal) {
        EMRUtility.insertUpdateFavListVal(ClinicalConsultationOrderDetail.FavListName, FavListVal);
    },


    //Function Name: ConsultationOrderSave
    //Author Name: Humaira Yousaf
    //Created Date: 02-03-2016
    //Description: Saves ConsultationOrder
    //Params: ConsultationOrderData
    saveConsultationOrder: function (ConsultationOrderData) {
        var objData = JSON.parse(ConsultationOrderData);
        objData["commandType"] = "save_consultationorder";
        if (ClinicalConsultationOrderDetail.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            objData["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            objData["NoteId"] = "";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },
    //Function Name: checkConsultationOrderExists
    //Author Name: Ahmad Raza
    //Created Date: 24-03-2016
    //Description: To check whether Consultation order exists or not
    checkConsultationOrderExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_consultationorder').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ConsultationOrderComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_consultationorder title="Consultation Order"  id="clinicalMenu_Orders_Consultation" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'ConsultationOrder\',\'clinicalMenu_Orders_Consultation\',' + Clinical_ProgressNote.params.NotesId + ');" title="Consultation Order">Consultation Order</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'ConsultationOrder\',\'clinicalMenu_Orders_Consultation\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_consultationorder> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_consultationorder').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
    },


    //Function Name: openConsultationOrderAlert
    //Author Name: Ahmad Raza
    //Created Date: 09-03-2016
    //Description: To show ConsultationOrder Search in Popup
    openConsultationOrderAlert: function () {

        if ($(" #mainForm  li#ConsultationOrderAlert span").text() != '' && $('#PatientProfile #hfPatientId').val() != '') {
            BackgroundLoaderShow(true);
            var params = [];


            params["FromAdmin"] = 0;
            //   params["StartupScreen"] = "message";
            LoadActionPan("Clinical_ConsultationOrder", params);
        }
    },

    //Function Name: loadConsultationOrder
    //Author Name: Humaira Yousaf
    //Created Date: 18-03-2016
    //Description: Loads ConsultationOrder data
    loadConsultationOrder: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Consultation", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                ClinicalConsultationOrderDetail.fillConsultationOrder(ClinicalConsultationOrderDetail.params.ConsultationOrderId).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            var ConsultationOrderDetail = JSON.parse(response.ConsultationOrderJSON);
                            ClinicalConsultationOrderDetail.ConsultationOrderTestGridLoad(response);
                            var self = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail");

                            if (response.ConsultationOrderProblems_JSON != null) {

                                response.ConsultationOrderProblems_JSON = JSON.parse(response.ConsultationOrderProblems_JSON);
                                for (var index in response.ConsultationOrderProblems_JSON) {
                                    ClinicalConsultationOrderDetail.problems.push(response.ConsultationOrderProblems_JSON[index].ProblemId);
                                    $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #ulProblemLists td #chk" + response.ConsultationOrderProblems_JSON[index].ProblemId).attr("checked", "checked");
                                }
                            }
                            utility.bindMyJSON(true, ConsultationOrderDetail, false, self);
                            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #hfConsultationOrderID").val(ConsultationOrderDetail.ConsultationOrderId);


                            if (ClinicalConsultationOrderDetail.params.mode == "Edit") {
                                if (ConsultationOrderDetail.Status.toLowerCase() == "signed") {
                                    ClinicalConsultationOrderDetail.disableConsultationOrderControls(true);
                                }
                                else {
                                    ClinicalConsultationOrderDetail.disableConsultationOrderControls(false);
                                }
                            }
                            else {
                                ClinicalConsultationOrderDetail.disableConsultationOrderControls(false);
                            }
                            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                    if (ClinicalConsultationOrderDetail.params.mode == "Add") {
                        ClinicalConsultationOrderDetail.disableConsultationOrderControls(false);
                    }
                    $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
                });
                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
                if (EMRUtility.getFavListStatus(ClinicalConsultationOrderDetail.FavListName))
                    $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #favSectionDiv").removeClass("toggled");
                else
                    $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #favSectionDiv").addClass("toggled");
                //End 10-06-2016 Abid Ali for favorite list setting for all favLists
                $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    //Function Name: fillConsultationOrder
    //Author Name: Humaira Yousaf
    //Created Date: 18-03-2016
    //Description: Fills ConsultationOrder
    //Params: ConsultationOrderId
    fillConsultationOrder: function (ConsultationOrderId) {
        var objData = new Object();
        objData["ConsultationOrderId"] = ConsultationOrderId;
        objData["commandType"] = "fill_consultationorder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    fillConsultationOrderDetail: function (ConsultationOrderId) {
        var objData = new Object();
        objData["ConsultationOrderId"] = ConsultationOrderId;
        objData["commandType"] = "getlatest_consultationorderby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    //Function Name: updateConsultationOrder
    //Author Name: Humaira Yousaf5
    //Created Date: 07-03-2016
    //Description: Updates ConsultationOrder
    //Params: ConsultationOrderData, ConsultationOrderId
    updateConsultationOrder: function (ConsultationOrderData, ConsultationOrderId) {

        var objData = JSON.parse(ConsultationOrderData);
        objData["ConsultationOrderId"] = ConsultationOrderId;
        objData["commandType"] = "save_consultationorder";
        if (ClinicalConsultationOrderDetail.params.ParentCtrlPanelID == "pnlClinicalProgressNote") {
            objData["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            objData["NoteId"] = "";
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");

    },

    //Function Name: addAgeConditionValues
    //Author Name: Humaira Yousaf
    //Created Date: 07-03-2016
    //Description: Shows respective cotrols based on selected age condition
    addAgeConditionValues: function () {
        var ageCondition = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ddlConsultationOrderAgeCondition").val();
        //Start 1-03-2016 Humaira Yousaf
        if (ageCondition == "") {
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ageConditionRange").addClass("hidden");
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ageConditionValue").addClass("hidden");
            var ageToValue = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderAgeValue]').val('');
            var ageToValue = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderAgeFrom]').val('');
            var ageToValue = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderAgeTo]').val('');
        }
            //End 1-03-2016 Humaira Yousaf
        else if (ageCondition == "6") {
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ageConditionRange").removeClass("hidden");
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ageConditionValue").addClass("hidden");
        }
        else {
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ageConditionValue").removeClass("hidden");
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ageConditionRange").addClass("hidden");
        }
    },

    //Function Name: isReminderValid
    //Author Name: Humaira Yousaf
    //Created Date: 07-03-2016
    //Description: Validates reminder
    isReminderValid: function () {
        var Message = "";

        var reminder = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderReminderLength]');
        var stayLength = $(reminder).val();
        var ddlVal = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail #ddlConsultationOrderReminderPeriod').val();
        if (stayLength != null && stayLength != '') {
            if (ddlVal == null || ddlVal == '') {
                $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail #ddlConsultationOrderReminderPeriod').focus();
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
        var selectedValue = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ddlConsultationOrderRuleType option:selected");
        var selected = [];
        $(selectedValue).each(function (index, val) {
            selected.push($(this).text());
        });
        var unSelect = '';
        $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail .comList").addClass('hidden');
        $(selected).each(function (i, item) {
            var sectionName = item;
            unSelect += sectionName + ',';
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #dv" + item.replace(/\s/g, '')).removeClass('hidden');
        });
    },
    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: to show ConsultationOrder Alert on Dashboard
    showConsultationOrderAlert: function (triggerLocation) {

        ClinicalConsultationOrderDetail.showConsultationOrderAlertDBCall(triggerLocation).done(function (response) {
            response = JSON.parse(response);
            //Start//09-03-2016//Ahmad Raza//setting hiddenField values
            $(" #mainForm  li#ConsultationOrderAlert input").val('');
            $(" #mainForm  li#ConsultationOrderAlert input").val(function (i, val) {
                return val + (val ? ', ' : '') + response.ConsultationOrderIDs;
            });
            //End//09-03-2016//Ahmad Raza//setting hiddenField values
            if (response.status != false) {

                if (response.alertCount > 0) {
                    $(" #mainForm  li#ConsultationOrderAlert span").text(response.alertCount);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: DBCall to show ConsultationOrder Alert on Dashboard
    showConsultationOrderAlertDBCall: function (triggerLocation) {
        var objData = new Object();
        if (triggerLocation == 'FaceSheet') {
            objData["ConsultationOrderTriggerLocation"] = '1';
        }
        else if (triggerLocation == 'Notes') {
            objData["ConsultationOrderTriggerLocation"] = '2';
        }
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "show_ConsultationOrder_alert";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    //Function Name: loadConsultationOrderMedications
    //Author Name: Humaira Yousaf
    //Created Date: 10-03-2016
    //Description: Loads ConsultationOrder Medications
    //Params: ConsultationOrderMedications
    loadConsultationOrderMedications: function (ConsultationOrderMedications) {
        $(ConsultationOrderMedications).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li id=" + item.drugId + " onclick='' \"><div><a href='#'>" + item.medicationName + "<span class='removeIconListHover' onclick='ClinicalConsultationOrderDetail.deleteMedication($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li id=" + item.drugId + " onclick='' \"><div><select id='ddlMedications" + item.drugId + "' name = 'ConsultationOrderMedications" + item.drugId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.medicationName + "<span class='removeIconListHover' onclick='ClinicalConsultationOrderDetail.deleteMedication($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ulConsultationOrderMedications").append(li);

            if (index != 0) {
                if (item.medicationOperator == 'AND') {
                    $($('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ulConsultationOrderMedications li#" + item.drugId).find("#ddlMedications" + item.drugId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ulConsultationOrderMedications li#" + item.drugId).find("#ddlMedications" + item.drugId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #txtConsultationOrderMedications").val('');
        });
    },
    //Function Name: loadConsultationOrderAllergies
    //Author Name: Ahmad Raza
    //Created Date: 16-03-2016
    //Description: Loads ConsultationOrder Allergies
    //Params: ConsultationOrderAllergies
    loadConsultationOrderAllergies: function (ConsultationOrderAllergies) {
        $(ConsultationOrderAllergies).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li id=" + item.AllergyId + " onclick='' \"><div ><a href='#'>" + item.Allergen + "<span class='removeIconListHover' onclick='ClinicalConsultationOrderDetail.deleteAllergy($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li id=" + item.AllergyId + " onclick='' \"><div><select id='ddlAllergies" + item.AllergyId + "' name = 'ConsultationOrderMedications" + item.AllergyId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.Allergen + "<span class='removeIconListHover' onclick='ClinicalConsultationOrderDetail.deleteAllergy($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ulConsultationOrderAllergies").append(li);

            if (index != 0) {
                if (item.AllergyOperator == 'AND') {
                    $($('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ulConsultationOrderAllergies li#" + item.AllergyId).find("#ddlAllergies" + item.AllergyId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ulConsultationOrderAllergies li#" + item.AllergyId).find("#ddlAllergies" + item.AllergyId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #txtConsultationOrderAllergies").val('');
        });
    },

    //Function Name: loadConsultationOrderProblems
    //Author Name: Ahmad Raza
    //Created Date: 16-03-2016
    //Description: Loads ConsultationOrder Problems
    //Params: ConsultationOrderProblems
    loadConsultationOrderProblems: function (ConsultationOrderProblems) {
        $(ConsultationOrderProblems).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li name='" + item.Problem + "' onclick='' \"><div ><a href='#'>" + item.Problem + "<span class='removeIconListHover' onclick='ClinicalConsultationOrderDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li name='" + item.Problem + "' onclick='' \"><div><select id='ddlProblemList" + item.Problem + "' name = 'ConsultationOrderProblemList" + item.ProblemId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.Problem + "<span class='removeIconListHover' onclick='ClinicalConsultationOrderDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ulConsultationOrderProblemList").append(li);

            if (index != 0) {
                if (item.ProblemOperator == 'AND') {
                    $($('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ulConsultationOrderProblemList li#" + item.ProblemId).find("#ddlProblemList" + item.ProblemId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #ulConsultationOrderProblemList li#" + item.ProblemId).find("#ddlProblemList" + item.ProblemId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #txtConsultationOrderProblemList").val('');
        });
    },
    //Function Name: isAgeConditionValid
    //Author Name: Humaira Yousaf
    //Created Date: 11-03-2016
    //Description: Validates Age Condition
    isAgeConditionValid: function () {
        var Message = "";
        var ddlAgeConditionVal = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail #ddlConsultationOrderAgeCondition').val();

        if (ddlAgeConditionVal == '6') {

            var ageFromValue = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderAgeFrom]').val();
            var ageToValue = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderAgeTo]').val();

            if ((ageFromValue == null || ageFromValue == '') && (ageToValue == null || ageToValue == '')) {
                $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderAgeFrom]').focus();
                $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderAgeTo]').focus();
                Message = "Please enter From Age and To Age.";

            }
            else {
                if (ageFromValue != null || ageFromValue != '') {
                    if (ageToValue == null || ageToValue == '') {
                        $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderAgeTo]').focus();
                        Message = "Please enter To Age.";
                    }
                }

                if (ageToValue != null || ageToValue != '') {
                    if (ageFromValue == null || ageFromValue == '') {
                        $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderAgeFrom]').focus();
                        Message = "Please enter From Age.";
                    }
                }
            }

        }
        else {
            var ageValue = $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderAgeValue]').val();
            if (ageValue == null || ageValue == '') {
                $('#' + ClinicalConsultationOrderDetail.params.PanelID + ' #frmClinicalConsultationOrderDetail input[id*=txtConsultationOrderAgeValue]').focus();
                Message = "Please enter Age.";
            }
        }

        return Message;
    },



    //------------------ start Irfan Code for editable grid

    bindAutoComplete: function (element) {
        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "ClinicalConsultationOrderDetail", null, true);

    },

    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalConsultationOrderDetail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = ClinicalConsultationOrderDetail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, ClinicalConsultationOrderDetail.params.PanelID);
    },

    BindProcedureGridItem: function (cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription) {

        var cptCode = cptCode;
        var procDesc = procedureDescription;
        var cptDesc = cptDescription;
        var currentRow = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #dgvConsultation tbody tr");
        var isTestAlreadySelected = false;
        $.each(currentRow, function (i, item) {
            var currentRowCPTDescription = $(item).find("input[id*='Procedure']").val() != null ? $(item).find("input[id*='Procedure']").val().replace(cptCode, "").trim() : "";
            if (cptDescription.toLowerCase() == currentRowCPTDescription.toLowerCase()) {
                isTestAlreadySelected = true;
                return;
            }
        });

        if (isTestAlreadySelected != true) {
            ClinicalConsultationOrderDetail.AddNewConsultationRow(null, null, null, cptCode, procDesc, cptDescription, SNOMEDId, SNOMEDDescription);
            setTimeout(function () {
                $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #txtConsultationCPTCode").val('');
            }, 200);
        }
        else {
            utility.DisplayMessages("Procedure is already selected", 2);
        }


    },

    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div></div>");

        var ddlSpecimen = "<div class='col-xs-2'><label class='control-label'>Specimen</label><select id='ddlSpecimen" + ParentRowId + "' name='ddlSpecimen" + ParentRowId + "' class='form-control' ddlist='GetProvider'></select></div>";
        var PatientInstructions = "<div class='col-xs-3'><label class='control-label'>Patient Instructions</label><textarea class='form-control' spellcheck='true' rows='1' id='txtPatientInstructions" + ParentRowId + "' maxlength='100' name='txtPatientInstructions" + ParentRowId + "'></textarea></div>";
        var Volume = "<div class='col-xs-3'><label class='control-label'>Volume</label><textarea class='form-control' spellcheck='true' rows='1' id='txtVolume" + ParentRowId + "' maxlength='100' name='txtVolume" + ParentRowId + "'></textarea></div>";
        var ddlVolume = "<div class='col-xs-2'><label class='control-label'></label><select id='ddlVolume" + ParentRowId + "' name='ddlVolume" + ParentRowId + "' class='form-control' ddlist='GetProvider'></select></div>";
        var FillerInstructions = "<div class='col-xs-3'><label class='control-label'>Filler Instructions</label><textarea class='form-control' spellcheck='true' rows='1' id='txtFillerInstructions" + ParentRowId + "' maxlength='100' name='txtFillerInstructions" + ParentRowId + "'></textarea></div>";


        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(ddlSpecimen, PatientInstructions, Volume, ddlVolume, FillerInstructions);
        return ChildHTML;

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
        AppPrivileges.GetFormPrivileges("Orders and Results_Consultation", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ChargeCapId"] = ChargeCapId;
                params["mode"] = "Edit";
                //Edited by Azeem Raza Tayyab on 16-Feb-2016 to fix bug#: PMS-3871
                if (ClinicalConsultationOrderDetail.params.TabID == 'chargeBatchDetail' || ClinicalConsultationOrderDetail.params.TabID == 'billTabUnClaimedAppointment' || ClinicalConsultationOrderDetail.params.TabID == 'Bill_ChargeSearch' || ClinicalConsultationOrderDetail.params.TabID == 'Patient_Case_Detail' || ClinicalConsultationOrderDetail.params.TabID == 'schTabCalendar' || ClinicalConsultationOrderDetail.params.TabID == 'batchTabEncounter' || ClinicalConsultationOrderDetail.params.TabID == 'Bill_FollowUpPatientAR_Detail' || ClinicalConsultationOrderDetail.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || ClinicalConsultationOrderDetail.params.TabID == "billTabClaimSubmission" || ClinicalConsultationOrderDetail.params.TabID == "Bill_PaymentPosting" || ClinicalConsultationOrderDetail.params.TabID == "EDIClaimViewDetail")
                    params["ParentCtrl"] = 'ClinicalConsultationOrderDetail';
                else
                    params["ParentCtrl"] = ClinicalConsultationOrderDetail.params["TabID"];
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
                ClinicalConsultationOrderDetail.DeleteConsultationOrderTest($row.attr("id"), $row, obj);
            }
            else {
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
                utility.DisplayMessages("Successfully Deleted", 1);
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

    AddNewConsultationRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription, SNOMEDId, SNOMEDDescription) {

        var ConsultationGridId = "#" + ClinicalConsultationOrderDetail.params.PanelID + " #dgvConsultation";

        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (ClinicalConsultationOrderDetail.params.ParentCtrl != null) {
                CurrentRow = ClinicalConsultationOrderDetail.EditableGrid.rowAdd(RowId, "");
            }
            else {
                CurrentRow = ClinicalConsultationOrderDetail.EditableGrid.rowAdd(RowId, ClinicalConsultationOrderDetail.params.ConsultationId);
            }
        }
        else {
            var TemplateRow = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #dgvConsultation tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            CurrentRow = ClinicalConsultationOrderDetail.EditableGrid.rowAdd(TemplateRowId - 1, "");
        }
        var rowId = CurrentRow.attr("id");

        var cptCodeHtml = '<input type="hidden" id="CPTCode' + rowId + '"  name="CPTCode" value="' + cptCode + '" />';
        var cptDescHtml = '<input type="hidden" id="CPTDescription' + rowId + '" name="CPTDescription"  value="' + cptDescription + '"  />';
        $(CurrentRow).find('td:first').append(cptCodeHtml + cptDescHtml);

        var snomedCodeHtml = '<input type="hidden" id="CPTSNOMEDCodeId' + rowId + '"  name="CPTSNOMEDCodeId" value="' + SNOMEDId + '" />';
        var snomedDescHtml = '<input type="hidden" id="CPTSNOMEDDescription' + rowId + '" name="CPTSNOMEDDescription"  value="' + SNOMEDDescription + '"  />';
        $(CurrentRow).find('td:first').append(snomedCodeHtml + snomedDescHtml);


        $(CurrentRow).find('input[id*="ConsultationProcedure"]').val(cptCode + " " + cptDescription);
        ClinicalConsultationOrderDetail.enableRemoveRow($(CurrentRow));
        //Start Farooq Ahmad 07/14/2016 /EMR-588
        var dgvConsultation = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #dgvConsultation");

        $(dgvConsultation).find('input[id*="dtpConsultationDate"]').removeClass('size80');
        $(dgvConsultation).find('input[id*="dtpConsultationDate"]').closest('div').removeClass('size80');

        $(dgvConsultation).find('input[id*="tpConsultationTime"]').removeClass('size80');
        $(dgvConsultation).find('input[id*="tpConsultationTime"]').closest('div').removeClass('size80');

        $(dgvConsultation).find('input[id*="ConsultationProcedure"]').addClass('size-min300');
        //End Farooq Ahmad 07/14/2016 /EMR-588
        return CurrentRow;
    },

    ConsultationOrderTestGridLoad: function (response) {
        //var response = JSON.parse(response);
        var PanelConsultationGrid = "#" + ClinicalConsultationOrderDetail.params.PanelID + " #pnlConsultation_Result";
        var ConsultationGridId = "#" + ClinicalConsultationOrderDetail.params.PanelID + " #dgvConsultation";
        $(ConsultationGridId + " tbody tr").remove();
        if ($.fn.dataTable.isDataTable(ConsultationGridId)) {
            $(ConsultationGridId).dataTable().fnClearTable();
            $(ConsultationGridId).dataTable().fnDestroy();
        }
        ClinicalConsultationOrderDetail.EditableGrid.datatable.clear().draw();
        var ConsultationOrderTestLoadJSONData = JSON.parse(response.consultationOrderTest_JSON);
        $.each(ConsultationOrderTestLoadJSONData, function (i, item) {
            var ConsultationOrderTestId = item.ConsultationOrderTestId;
            var CurrentRow = ClinicalConsultationOrderDetail.AddNewConsultationRow(ConsultationOrderTestId, null, null, item.CPTCode, item.CPTDescription, item.CPTDescription, null, null);
            var self = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " tr#" + ConsultationOrderTestId);
            var row = ClinicalConsultationOrderDetail.EditableGrid.datatable.row(CurrentRow);
            var ConsultationTestTable = $("#" + ClinicalConsultationOrderDetail.params.PanelID + " #dgvConsultation");

            //Start Farooq Ahmad 03/28/2016 bind values to the table
            var counter = 0;
            var BindFunction = function (counter, item, CurrentRow) {
                //Start 08-11-2016 Edit By Humaira Yousaf Bug# EMR-1925
                if (counter++ < 5 && $(CurrentRow).find('select option').length <= 1)
                    setTimeout(BindFunction, 300, counter, item, CurrentRow);
                else {
                    utility.bindMyJSONByName(true, item, false, $(CurrentRow)).done(function () {
                        $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
                    });
                }
                //End 08-11-2016 Edit By Humaira Yousaf Bug# EMR-1925
            }
            BindFunction(counter, item, CurrentRow);
            //End Farooq Ahmad 03/28/2016 bind values to the table

        });

    },

    DeleteConsultationOrderTest: function (ConsultationTestId, $row, obj) {

        ClinicalConsultationOrderDetail.ConsultationOrderTest_DBCall(ConsultationTestId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
            } else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },

    ConsultationOrderTest_DBCall: function (ConsultationTestId) {

        var objData = new Object();
        objData["ConsultationOrderTestId"] = ConsultationTestId;
        objData["commandType"] = "DELETE_CONSULTATIONORDER_TEST";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    getConsultationOrderInfo: function (consultationOrderId) {
        ClinicalConsultationOrderDetail.fillConsultationOrderDetail(consultationOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    ClinicalConsultationOrderDetail.createConsultationOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //------------------ end Irfan Code for editable grid

    //Function Name: getLatestConsultationOrderByPatientId
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: Loads latest Consultation Order by PatientId
    getLatestConsultationOrderByPatientId: function (hideAlertMessage) {
        ClinicalConsultationOrderDetail.getLatestConsultationOrderByPatientIdDBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ClinicalConsultationOrderDetail.createConsultationOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });
    },

    //Function Name: createConsultationOrderBodyHTML
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: creates Consultation Order's HTML
    createConsultationOrderBodyHTML: function (response, NoteHTMLCtrl, UnloadConsultationOrder, hideAlertMessage) {
        ClinicalConsultationOrderDetail.checkConsultationOrderExists();
        if (response.ConsultationOrderFill_JSON != null && response.ConsultationOrderFill_JSON != '') {
            var ConsultationOrderFill_Obj = JSON.parse(response.ConsultationOrderFill_JSON);
            var $mainDivConsultationOrder = $(document.createElement('div'));

            var ConsultationOrderId = ConsultationOrderFill_Obj.ConsultationOrderId;
            if (ConsultationOrderId > 0) {
                var $SectionBodyConsultationOrder = $(document.createElement('section'));
                $SectionBodyConsultationOrder.attr('id', "Cli_ConsultationOrderDetail_Main" + ConsultationOrderId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_ConsultationOrderDetail_" + ConsultationOrderId);
                var $ListConsultationOrder = $(document.createElement('ul'));

                $ListConsultationOrder.attr('class', 'list-unstyled')

                $SectionBodyConsultationOrder.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ConsultationOrderDetail_" + ConsultationOrderId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_ConsultationOrderDetail_Main" + ConsultationOrderId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListConsultationOrder.append("<li>" + ConsultationOrderFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListConsultationOrder);
                $SectionBodyConsultationOrder.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_consultationorder').parent().parent().find('#Cli_ConsultationOrderDetail_Main' + ConsultationOrderId).length == 0) {
                    $mainDivConsultationOrder.append($SectionBodyConsultationOrder);
                    ClinicalConsultationOrderDetail.updateConsultationOrderHtml($mainDivConsultationOrder.html(), ConsultationOrderId, NoteHTMLCtrl, hideAlertMessage);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_consultationorder').parent().parent().find('#Cli_ConsultationOrderDetail_Main' + ConsultationOrderId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_consultationorder').parent().parent().find('#Cli_ConsultationOrder_Main' + ConsultationOrderId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_consultationorder').parent().parent().find('#Cli_ConsultationOrderDetail_Main' + ConsultationOrderId).html($SectionBodyConsultationOrder.html());
                    $(NoteHTMLCtrl + ' clinical_consultationorder').parent().parent().find('#Cli_ConsultationOrderDetail_Main' + ConsultationOrderId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText("ConsultationOrder", hideAlertMessage);
                    ClinicalConsultationOrderDetail.updateConsultationOrderHtml("", ConsultationOrderId, NoteHTMLCtrl, hideAlertMessage);

                }

                if (UnloadConsultationOrder == true) {
                    ClinicalConsultationOrderDetail.Unload(ClinicalConsultationOrderDetail.bNextPrev);
                }
            } else {
                Clinical_ProgressNote.saveComponentSOAPText("ConsultationOrder", hideAlertMessage);
            }
        } else {
            Clinical_ProgressNote.saveComponentSOAPText("ConsultationOrder", hideAlertMessage);
        }
    },

    //Function Name: detachConsultationOrderFromNotes
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: detach Consultation Order From Notes
    detachConsultationOrderFromNotes: function (ConsultationOrderId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_consultationorder');
                var selectedValue = ConsultationOrderId.replace('Cli_ConsultationOrderDetail_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    ClinicalConsultationOrderDetail.detachConsultationOrderFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + ConsultationOrderId).remove();

                            Clinical_ProgressNote.saveComponentSOAPText("ConsultationOrder");
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

    //Function Name: detachConsultationOrderFromNotes
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to detach Consultation Order From Notes
    detachConsultationOrderFromNotes_DBCall: function (ConsultationOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ConsultationOrderId"] = ConsultationOrderId;
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
        objData["commandType"] = "detach_consultationorder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    //Function Name: detach_ComponentsConsultationOrder
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: detach Components of Consultation Order From Notes
    detach_ComponentsConsultationOrder: function (ComponentName, IsUpdate, ConsultationOrderComponentRemove) {

        var consultationOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_consultationorder').parent().parent().find('section[id*="Cli_ConsultationOrderDetail_Main"]').map(function () {
            return this.id.replace("Cli_ConsultationOrderDetail_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ConsultationOrderComponent').parent().parent().attr('NoteComponentId');
        if (ConsultationOrderComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_consultationorder').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('ConsultationOrder', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_consultationorder').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Consultation Order']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_consultationorder').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('ConsultationOrder', true))
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
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_consultationorder').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_consultationorder').parent().parent().find('section[id*="Cli_ConsultationOrderDetail_Main"]').remove();
        }

        if (consultationOrderIds == "" || consultationOrderIds == "undefined") {
            $.when(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId)).then(function () {
                utility.DisplayMessages('Successfully Deleted', 1);
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            });
        }
        else {
            ClinicalConsultationOrderDetail.detachConsultationOrderFromNotesDBCall(consultationOrderIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText("ConsultationOrder", true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //Function Name: detachConsultationOrderFromNotesDBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to detach Components of Consultation Order From Notes
    detachConsultationOrderFromNotesDBCall: function (ConsultationId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ConsultationOrderId"] = ConsultationId;
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
        objData["commandType"] = "detach_ConsultationOrder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    //Function Name: updateConsultationOrderHtml
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: updates Consultation Order Html
    updateConsultationOrderHtml: function (ConsultationOrderHtml, ConsultationOrderId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' clinical_consultationorder').parent().parent().addClass('initialVisitBody');
        if (ConsultationOrderHtml != '') {
            $(NoteHTMLCtrl + ' clinical_consultationorder').parent().parent().append(ConsultationOrderHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ConsultationOrderHtml != '') {
            ClinicalConsultationOrderDetail.attachConsultationOrderWithNotes(ConsultationOrderId, hideAlertMessage);
        }

    },

    //Function Name: attachConsultationOrderWithNotes
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: attach Consultation Order With Notes
    attachConsultationOrderWithNotes: function (ConsultationOrderId, hideAlertMessage) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var selectedValue = ConsultationOrderId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                ClinicalConsultationOrderDetail.attachConsultationOrderWithNotesDBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //If Attached MedicalHx Made new inseration to MedicalHx Table than good ids should be attached to HTML
                        Clinical_ProgressNote.saveComponentSOAPText("ConsultationOrder", hideAlertMessage);
                        $('#' + ConsultationOrderId).remove();
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

    //Function Name: attachConsultationOrderWithNotes
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to attach Consultation Order With Notes
    attachConsultationOrderWithNotesDBCall: function (ConsultationOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ConsultationOrderId"] = ConsultationOrderId;
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
        objData["commandType"] = "attach_ConsultationOrder_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    //Function Name: getLatestConsultationOrderByPatientIdDBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to get Latest Consultation Order By PatientId
    getLatestConsultationOrderByPatientIdDBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_consultationorderby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },
    GetConsultationJSON: function () {

        var myArray = [];
        var objConsultation = new Object();
        $.each($("#" + ClinicalConsultationOrderDetail.params.PanelID + " #dgvConsultation tbody tr"), function (i, item) {

            var $row = $(this).closest("tr");
            var rowId = $row.attr("id");
            var self = $row;
            var myJSON = self.getMyJSONByName();


            //var objData = JSON.parse(myJSON);
            //objData["ProcedureId"] = rowId;
            //jsonStr = JSON.stringify(objData);

            //myArray.push(jsonStr);
            objConsultation["ProcedureId" + rowId] = myJSON;
        });

        console.log(objConsultation);

    },
    //Function Name: printConsultationOrder
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: Creates PDF to view Consultation Order
    printConsultationOrder: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = ClinicalConsultationOrderDetail.params.TabID;//'mstrTabDashBoard';
        params["ConsultationOrderId"] = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #hfConsultationOrderID").val();
        LoadActionPan('Clinical_ConsultationOrderView', params);
    },

    //Function Name: loadAllAutocomplete
    //Author Name: Humaira Yousaf
    //Created Date: 11-05-2016
    //Description: Loads provider and assignee
    loadAllAutocomplete: function () {
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #txtProvider");
            var hfCtrl = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #hfProvider");
            var onSelect = function (dataItem) { $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #txtProvider").attr("ProviderId", dataItem.id); }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
        });

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #txtAssignee");
            var hfCtrl = $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #hfAssignee");
            var onSelect = function (dataItem) { $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail #txtAssignee").attr("AssigneeId", dataItem.id); }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
            $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").data('serialize', $('#' + ClinicalConsultationOrderDetail.params.PanelID + " #frmClinicalConsultationOrderDetail").serialize());
        });
    },
}