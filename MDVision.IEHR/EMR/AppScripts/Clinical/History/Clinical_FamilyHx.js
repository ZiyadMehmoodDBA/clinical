//Author: Muhammad Irfan
//Date: 14-01-2016
//This file will handle all actions performed for Family History and it's child handling
//Once FamilyHx will be created then it's child can be created then.
Clinical_FamilyHx = {
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    currentSelectedDisease: [],
    currentSelectedMember: [],
    FamilyMembers: [],
    bNextPrev: false,
    controlToInvoke: null,
    lastSelectedType: null,
    previousSelectedDisease: [],
    FavListName: 'ClinicalFamilyHx',
    familyHxJSON: '',
    familyDate: '',
    unremarkable: false,
    overallComments: '',
    //Author: Muhammad Irfan
    //Date: 20/01/2016
    //This function will be called once tab is clicked, it expects parameters to be used for FamilyHx
    Load: function (params) {
        Clinical_FamilyHx.params = params;

        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        if (Clinical_FamilyHx.params.mode == null) {
            Clinical_FamilyHx.params.mode = "Add";
        }
        if (Clinical_FamilyHx.params.PanelID != 'pnlClinicalFamilyHx') {
            Clinical_FamilyHx.params.PanelID = Clinical_FamilyHx.params.PanelID + ' #pnlClinicalFamilyHx';
        } else {
            Clinical_FamilyHx.params.PanelID = 'pnlClinicalFamilyHx';
            Clinical_FamilyHx.params.CurrentNotesProviderId = "";
        }
        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_FamilyHx.params.PanelID.indexOf("pnlClinicalProgressNote") < 0) {
            Clinical_FamilyHx.params.PanelID = "pnlClinicalProgressNote #" + Clinical_FamilyHx.params.PanelID;
        }
        Clinical_FamilyHx.ResetFormData();
        //if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabNotes" ) {
        //    Clinical_FamilyHx.bIsFirstLoad = true;
        //    $('#divViewHistorySummary').addClass('hidden');
        //    $(' #pnlClinicalFamilyHx').removeClass('row');
        //}
        //    if (Clinical_FamilyHx.bIsFirstLoad) {
        EMRUtility.setFavoriteSectionStyle(Clinical_FamilyHx.params.PanelID);
        Clinical_FamilyHx.favoriteListSearch();

        var FamilyHxId = "";
        if (Clinical_FamilyHx.params.mode == "Add" || Clinical_FamilyHx.params.FamilyHxId == null || Clinical_FamilyHx.params.FamilyHxId == "" || Clinical_FamilyHx.params.FamilyHxId == "-1") {
            FamilyHxId = "-1";
        }
        else if (Clinical_FamilyHx.params.mode == "Edit") {
            FamilyHxId = Clinical_FamilyHx.params.FamilyHxId;
            //Clinical_FamilyHx.FamilyHxEdit(FamilyHxId);
        }

        //Load Dropdown
        //if (Clinical_FamilyHx.bIsFirstLoad) { // start of if condition
        //    Clinical_FamilyHx.bIsFirstLoad = false;
        var self = $('#' + Clinical_FamilyHx.params.PanelID);

        self.loadDropDowns(true).done(function () {
            Clinical_FamilyHx.loadFamilyHx();
            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx').data('serialize', $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx').serialize());

            if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Clinical_FamilyHx.params.PanelID + ' #btnFamilySave').addClass('hidden');
                $('#' + Clinical_FamilyHx.params.PanelID + ' #btnAddVitalsOnNote').addClass('hidden');

                var details = $('#' + Clinical_FamilyHx.params.PanelID + " #FamilyMemberDetails").clone();
                $(details).resetAllControls(null);
                Clinical_FamilyHx.familyHxJSON = $(details).getMyJSONByName();

                //
                var updatJSON = Clinical_FamilyHx.familyHxJSON;
                var RadYesRelativeDied = $(details).find('#RadYesRelativeDied').prop("checked");
                var RadNoRelativeDied = $(details).find('#RadNoRelativeDied').prop("checked");
                var RadRelativeDied = "";
                if (RadYesRelativeDied) {
                    RadRelativeDied = true;
                }
                else if (RadNoRelativeDied) {
                    RadRelativeDied = false;
                }

                updatJSON = JSON.parse(updatJSON);
                updatJSON.RadRelativeDied = RadRelativeDied;
                updatJSON = JSON.stringify(updatJSON);
                Clinical_FamilyHx.familyHxJSON = updatJSON;
                //




            }
            else {
                $('#' + Clinical_FamilyHx.params.PanelID + ' #btnFamilySave').removeClass('hidden');
            }
        });
        //Start//26/01/2016//Ahmad Raza//calling ready function where checkbox check/uncheck logic is implimented
        Clinical_FamilyHx.readyFunction();
        //End//26/01/2016//Ahmad Raza//calling ready function where checkbox check/uncheck logic is implimented
        //end Load Dropdown
        utility.CreateDatePicker(Clinical_FamilyHx.params.PanelID + '  #dtFamilyHxDate', function () {
        }, true);

        //Start//17-02-2016//Ahmad Raza//Fixed Bug#335
        //utility.CreateDatePicker(Clinical_FamilyHx.params.PanelID + '  #dtpYearofBirth', function () {

        //}, true);
        //End//17-02-2016//Ahmad Raza//Fixed Bug#335

        //29/03/2016//Abid Ali//For bug# EMR-529
        //Start Sep 25, 2017 Edit By Humaira Yousaf Bug# EMR-4802
        EMRUtility.CreateYearViewDatePicker(Clinical_FamilyHx.params.PanelID + '  section#sectionFamilyMemberDetails div#FamilyMemberDetails #dtpYearofBirth', function () {

            Clinical_FamilyHx.reValidateAges();
        }, false, null, null, true);
        //End Sep 25, 2017 Edit By Humaira Yousaf Bug# EMR-4802
        //29/03/2016//Abid Ali//For bug# EMR-529

        if ($('#' + Clinical_FamilyHx.params.PanelID + ' #PatientProfile #hfPatientId').val() != "") {
            $('#' + Clinical_FamilyHx.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        //22/12/2015//AhmadRaza//Form Serialization
        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx').data('serialize', $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx').serialize());


        //Code for progress note navigation
        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_FamilyHx.params.PanelID + ' #pnlClinicalFamilyHx').removeClass('row');
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_FamilyHx.params.PanelID, 'History', 'FamilyHx', 'Clinical_FamilyHx.unLoadTab(true);', null, true);
            $('#' + Clinical_FamilyHx.params.PanelID + ' #btnAddVitalsOnNote').show();
            //  $('#' + Clinical_FamilyHx.params.PanelID + '  #dtFamilyHxDate').prop('disabled', true);
            //$('#' + Clinical_FamilyHx.params.PanelID + '  #btnFamilySave').hide();
            $('#' + Clinical_FamilyHx.params.PanelID + '  #btnFamilyHxSave').hide();
            //Start//28/01/2016//Ahmad Raza//logic to disable member section and member detail section, when no disease is selected
            var MemberDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#memberList');
            //MemberDiv.addClass("disableAll");
            //var MemberDetailDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails');
            //MemberDetailDiv.addClass("disableAll");
            //End//28/01/2016//Ahmad Raza//logic to disable member section and member detail section, when no disease is selected

        } else {

            //Start//28/01/2016//Ahmad Raza//logic to disable member section and member detail section, when no disease is selected
            var MemberDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#memberList');
            //MemberDiv.addClass("disableAll");
            //var MemberDetailDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails');
            //MemberDetailDiv.addClass("disableAll");
            //End//28/01/2016//Ahmad Raza//logic to disable member section and member detail section, when no disease is selected
            $('#' + Clinical_FamilyHx.params.PanelID + ' #btnAddVitalsOnNote').hide();
            $('#' + Clinical_FamilyHx.params.PanelID + '  #dtFamilyHxDate').prop('disabled', false);
            $('#' + Clinical_FamilyHx.params.PanelID + ' #btnFamilyHxSave').hide();
        }
        //} // end of if condition


        Clinical_FamilyHx.domReadyFunction();
        if (Clinical_FamilyHx.params.ParentCtrl != "clinicalTabProgressNote") {
            Clinical_FamilyHx.bIsFirstLoad = false;
        }
        //  }


        if (EMRUtility.getFreeTextStatus("Clinical_FamilyHx")) {
            var panel = "#pnlClinicalFamilyHx";
            if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalFamilyHx";
            }

            $(panel + " #DivICDAutoComplete").addClass("hidden");

            if ($(panel + " #DivFreeText").hasClass("hidden")) {
                $(panel + " #DivFreeText").removeClass("hidden");
            }
            $(panel + " #rdofreetext").prop('checked', 'checked');
        }
        else {
            var panel = "#pnlClinicalFamilyHx";
            if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalFamilyHx";
            }

            $(panel + " #DivICDAutoComplete").removeClass("hidden");
            if (!$(panel + " #DivFreeText").hasClass("hidden")) {
                $(panel + " #DivFreeText").addClass("hidden");
            }
            $(panel + " #rdodiagnose").prop('checked', 'checked');
        }

        $('#' + Clinical_FamilyHx.params.PanelID + ' #txtFreeText').bind("keypress", function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Clinical_FamilyHx.createFreeTextLi();
            }
        });


        //if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
        //    $('#' + Clinical_FamilyHx.params.PanelID + ' #btnFamilySave').addClass('hidden');
        //    $('#' + Clinical_FamilyHx.params.PanelID + ' #btnAddVitalsOnNote').addClass('hidden');

        //    var details = $('#' + Clinical_FamilyHx.params.PanelID + " #FamilyMemberDetails").clone();
        //    $(details).resetAllControls(null);
        //    Clinical_FamilyHx.familyHxJSON = $(details).getMyJSONByName();
        //}
        //else {
        //    $('#' + Clinical_FamilyHx.params.PanelID + ' #btnFamilySave').removeClass('hidden');
        //}

        if (EMRUtility.getFavListStatus(Clinical_FamilyHx.FavListName)) {
            $('#' + Clinical_FamilyHx.params.PanelID + " #favSectionDiv").addClass("toggledHor");
            $('#' + Clinical_FamilyHx.params.PanelID + " #FormDiv").addClass("toggleHorContainer");
        }
        else {
            $('#' + Clinical_FamilyHx.params.PanelID + " #favSectionDiv").removeClass("toggledHor");
            $('#' + Clinical_FamilyHx.params.PanelID + " #FormDiv").removeClass("toggleHorContainer");
        }
    },
    ResetFormData: function () {
        var details = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx");
        $(details).resetAllControls(null);
        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ulFamilyHxDisease").html("");
        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease").removeClass('disableAll');
    },

    changeDiagnoseField: function () {

        if ($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #rdofreetext").prop("checked") == true) {
            //  Clinical_FamilyHx.lastSelectedType = "FreeText";
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #DivICDAutoComplete").addClass('hidden');
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #btnsearchicd").addClass('hidden');
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #txtFreeText").removeClass('hidden');
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #DivFreeText").removeClass('hidden');

        } else {
            //  Clinical_FamilyHx.lastSelectedType = "ICD";
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #DivICDAutoComplete").removeClass('hidden');
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #btnsearchicd").removeClass('hidden');
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #txtFreeText").addClass('hidden');
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #DivFreeText").addClass('hidden');
        }
        Clinical_FamilyHx.SaveFreeTextStatus();


    },

    createFreeTextLi: function () {

        var diseaseText = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #txtFreeText").val();
        if (diseaseText) {
            diseaseText = diseaseText.trim()
        }
        if (diseaseText != null && diseaseText != '') {
            var currId = -1;

            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyHxDisease li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            //Start 04-11-2016 Humaira Yousaf to change onclick event
            var li = "<li  id=" + currId + " onclick='Clinical_FamilyHx.fillCurrentMemberDiseasesDetail(this, event);' onmouseover='Clinical_FamilyHx.showIcon(this);' onmouseout='Clinical_FamilyHx.hideIcon(this);' MemberDetailId=" + currId + "><a href='#'>" + diseaseText + "<span class='removeIconListHover' onclick='Clinical_FamilyHx.deleteFamilyHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            //End 04-11-2016 Humaira Yousaf to change onclick event

            var IsAlreadyExist = false;
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #Clinical_FamilyHx li").each(function () {
                if ($(this).text().toLowerCase() == diseaseText.toLowerCase()) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ulFamilyHxDisease").append(li);
                $(li).trigger('click');
                $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #txtFreeText").val('');

                if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    var diseaseId = currId;// $('#' + Clinical_FamilyHx.params.PanelID + " #ulFamilyHxDisease > li.active").attr('id');
                    var memberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
                    var disease = $(li).get(0).outerHTML;
                    var detailsdata = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').clone();
                    $(detailsdata).resetAllControls(null);
                    var diseaseData = $(detailsdata).getMyJSONByName();


                    //
                    var updatJSON = diseaseData;
                    var RadYesRelativeDied = $(detailsdata).find('#RadYesRelativeDied').prop("checked");
                    var RadNoRelativeDied = $(detailsdata).find('#RadNoRelativeDied').prop("checked");
                    var RadRelativeDied = "";
                    if (RadYesRelativeDied) {
                        RadRelativeDied = true;
                    }
                    else if (RadNoRelativeDied) {
                        RadRelativeDied = false;
                    }

                    updatJSON = JSON.parse(updatJSON);
                    updatJSON.RadRelativeDied = RadRelativeDied;
                    updatJSON = JSON.stringify(updatJSON);
                    diseaseData = updatJSON;
                    //




                    Clinical_FamilyHx.cacheFamilyHxJSON(memberId, diseaseId, diseaseData, disease);

                    $('#' + Clinical_FamilyHx.params.PanelID + " #ulFamilyMember li.active").find("a").css("color", "green");
                }
            }
            else {
                utility.DisplayMessages('Diagnosis already added', 2);

                $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #txtDisease").val('');
            }
        }
        else {
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #txtFreeText").val('');
        }

    },



    //Author: Ahmad Raza
    //Date: 12-28-2015
    // checkbox check/uncheck logic is implimented
    //Begin 26-01-2016 Muhammad Irfan Bug# EMR-159 Family History Clinical Module -> Save
    readyFunction: function () {
        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails input:checkbox').on('click', function () {
            // in the handler, 'this' refers to the box clicked on
            var $box = $(this);
            if ($box.is(":checked")) {

                var group = "input:checkbox[name='" + $box.attr("name") + "']";
                $(group).prop("checked", false);

                $box.prop("checked", true);
                //Start//Abid Ali 10-2-2016/ for bug# 295
                if ($box.prop("id") == "RadYesRelativeDied") {
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', false);
                }
                //End//Abid Ali 10-2-2016/ for bug# 295
            } else {
                $box.prop("checked", false);
            }
        });
        //End//26/01/2016//Ahmad Raza//function where checkbox check/uncheck logic is implimented

    },

    //Function Name: disableEnterKey
    //Author: Ahmad Raza
    //Date: 17-05-2016
    //Description: To prevent submition of form on Enter Press
    disableEnterKey: function (e) {

        if (e.which == 13) // Enter key = keycode 13
        {
            e.preventDefault();
        }
    },

    favoriteListSearch: function () {
        var ProviderId = null;
        if (Clinical_FamilyHx.params.CurrentNotesProviderId != "undefined" && Clinical_FamilyHx.params.CurrentNotesProviderId && Clinical_FamilyHx.params.CurrentNotesProviderId != "")
            ProviderId = Clinical_FamilyHx.params.CurrentNotesProviderId;
        Favorite_ProcedureOrder.searchFavoriteList_DBCall("FamilyHistory", null, 1, 5000, ProviderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $ddl = $('#' + Clinical_FamilyHx.params.PanelID + ' #ddlFavoriteListFamilyHx');
                var favouriteProcedures = JSON.parse(response.FavoriteListJSON)
                $ddl.empty();
                $ddl.append($('<option/>', {
                    id: -1,
                    value: -1,
                    html: "- Select -"
                }));
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
                    EMRUtility.getFavListValue(Clinical_FamilyHx.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "" && response1.favListVal != "-1") {
                                if ($("#" + Clinical_FamilyHx.params.PanelID + " #ddlFavoriteListFamilyHx option[value='" + response1.favListVal + "']").length > 0) {
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
                //  $ddl.trigger("onchange");

            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });

    },

    showFamilyHxHistory: function (familyHxId) {

        var parentCtrlId = Clinical_FamilyHx.params.TabID;
        var grantParentCtrlId = null;
        if (Clinical_FamilyHx.params.TabID == "clinicalTabProgressNote") {

            parentCtrlId = "Clinical_HistorySummary";
            grantParentCtrlId = "pnlClinicalProgressNote";
        }
        EMRUtility.showCurrentItemHistory(Clinical_FamilyHx.params.PanelID, null, null, "FamilyHx,FamilyHx_Disease,FamilyHx_FamilyMemberDetail", null, parentCtrlId, familyHxId, grantParentCtrlId);
    },



    ChangeCurrentPast: function (obj, PrimaryID, PageNumber, ResultPerPage) {

        if ($(obj).attr('status') == '1' || obj == 1) {
            $(obj).attr('status', 0);
            $('#' + Clinical_FamilyHx.params.PanelID + " #pnlCurrent ").addClass("hidden");
            $('#' + Clinical_FamilyHx.params.PanelID + " #pnlPast ").removeClass("hidden");

            Clinical_FamilyHx.fillhxLog(PrimaryID, PageNumber, ResultPerPage).done(function (response) {
                if (response != "") {
                    var json = JSON.parse(response);
                    Clinical_FamilyHx.gridLoad(response);
                    var TableControl = Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvPastFamilyHx";
                    var PagingPanelControlID = Clinical_FamilyHx.params.PanelID + " #dgvPastFamilyHx_Paging";
                    var ClassControlName = "Clinical_FamilyHx";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = json.iTotalDisplayRecords;
                    setTimeout(
                        CreatePagination(json.HxLogSoapCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_FamilyHx.ChangeCurrentPast(1, PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                }
            });

        } else {
            $(obj).attr('status', 1);

            $('#' + Clinical_FamilyHx.params.PanelID + " #pnlPast").addClass("hidden");
            $('#' + Clinical_FamilyHx.params.PanelID + " #pnlCurrent  ").removeClass("hidden");
        }

    },

    fillhxLog: function (PrimaryID, PageNumber, ResultPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (ResultPerPage == null) {
            ResultPerPage = 15;
        }
        var objData = {};
        objData["HxId"] = Clinical_FamilyHx.params.HxTypeId;
        objData["HxType"] = "FamilyHx";
        objData["Status"] = "All";
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = ResultPerPage;
        objData["commandType"] = "get_hx_log";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");
    },
    //Author: Muhammad Irfan
    //Date: 21/01/2016
    //This function will handle Initialization of KeyPad control

    domReadyFunction: function () {
        $(function () {

            (function ($) {
                'use strict';
                $(function () {
                    $('#' + Clinical_FamilyHx.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);


            $('#' + Clinical_FamilyHx.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
            //EMR-70 Bug number Resolution
            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx [data-plugin-keyboard-numpad]').keyboard({
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
                restrictInput: true, // Prevent keys not in the displayed keyboard from being typed in
                preventPaste: true,  // prevent ctrl-v and right click
                usePreview: false,
                autoAccept: true,
                tabNavigation: true
            })
                  .addTyping();
            //Start//28/01/2016//Ahmad Raza//logic to disable Yes Checkbox and Age at death field when selected health status is Alive
            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #ddlHealthStatus').change(function () {
                var selectedStatus = this.value;
                if (selectedStatus == 1) {
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop('disabled', true);
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', true);
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').val('');


                    // Start || 29 August, 2016 || Talha Tanweer || EMR-748
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', true);
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#EEEEEE");
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #cccccc");

                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked", false);
                    // End   || 29 August, 2016 || Talha Tanweer || EMR-748

                }
                else {

                    // Start || 29 August, 2016 || Talha Tanweer || EMR-748
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', false);
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#fff");
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #ccc");

                    // End   || 29 August, 2016 || Talha Tanweer || EMR-748


                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop('disabled', false);
                    //Start//12-02-2016//Ahmad Raza// fixed EMR Bug#EMR-323
                    //if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadNoRelativeDied').prop("checked") == true) {
                    //    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', true);
                    //    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').val('');
                    //}
                    //else {
                    //    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', false);
                    //}
                    //End//12-02-2016//Ahmad Raza// fixed EMR Bug#EMR-323

                }

            });
            //End//28/01/2016//Ahmad Raza//logic to disable Yes Checkbox and Age at death field when selected health status is Alive

            //Start//03/02/2016//Ahmad Raza//Enable/Disable age at death field on checkbox check
            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadNoRelativeDied').change(function () {
                if (this.checked) {
                    //$('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', true).val("");

                }
                else {
                    if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #ddlHealthStatus').val() != "1")
                        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', false);

                }
            });

            //End//03/02/2016//Ahmad Raza//Enable/Disable age at death field on checkbox check

            //var date = new Date();
            //var currentMonth = date.getMonth();
            //var currentDate = date.getDate();
            //var currentYear = date.getFullYear();
            //var date_format = 'dd/mm/yyyy';

            //$('#' + Clinical_FamilyHx.params.PanelID + ' #dtpYearofBirth').datepicker({
            //    //pickTime: false,
            //    format: date_format,
            //    endDate: '+0d',
            //    todayHighlight: true,
            //});

            $('.toggleHorSmallLeft section').unbind('click').bind("click", function (e) {
                $(this).parent().toggleClass("toggled");
                Clinical_FamilyHx.toggleHorSmallLeftIcon($(this));

            });

        });

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

    selectAllFavoriteListContent: function () {

        $('#' + Clinical_FamilyHx.params.PanelID + ' #ulFavoriteListFamilyHxContent li').each(function (i, item) {
            $(item).trigger("click");
        });


    },
    AutoSearchFavFamilyHx: function () {
        utility.Keyupdelay(function () {
            Clinical_FamilyHx.loadfavoriteListContent();
        });
    },
    loadfavoriteListContent: function (obj) {
        if (typeof obj == typeof undefined || obj == null) {
            obj = $('#' + Clinical_FamilyHx.params.PanelID + ' #ddlFavoriteListFamilyHx');
        }
        var SearchData = $('#' + Clinical_FamilyHx.params.PanelID + ' #FavSearchBox').val();
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            //Start 01-07-2016 Humaira Yousaf to disable Select All link
            if (selectedOption.attr("id") != "-1") {
                Clinical_FamilyHx.favoriteList_CPTSearch(selectedOption.attr("id"), SearchData);
            }
            else {
                $('#' + Clinical_FamilyHx.params.PanelID + ' #ulFavoriteListFamilyHxContent').empty();
                $('#' + Clinical_FamilyHx.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
            }
            //End 01-07-2016 Humaira Yousaf to disable Select All link
        }
    },

    favoriteList_CPTSearch: function (FavoriteListId, SearchData) {
        var $UL = $('#' + Clinical_FamilyHx.params.PanelID + ' #ulFavoriteListFamilyHxContent');
        Clinical_FamilyHx.searchFavoriteList_ICD_DBCall(null, FavoriteListId, 1, 5000, SearchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                $UL.empty();

                if (response.FavoriteListICDCount > 0) {
                    $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #ulFavCompliantDisease li").remove();
                    var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);

                    if (FavoriteListJSON.length > 0) {
                        $('#' + Clinical_FamilyHx.params.PanelID + ' #favSelectAllLink').removeClass('disableAll');
                    }
                    else {
                        $('#' + Clinical_FamilyHx.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
                    }

                    var li = "";
                    $.each(FavoriteListJSON, function (i, item) {
                        if (typeof item.ICD9Code == 'undefined' || item.ICD9Code == null) { item.ICD9Code = ''; }
                        if (typeof item.ICD10Code == 'undefined' || item.ICD10Code == null) { item.ICD10Code = ''; }
                        if (typeof item.SNOMEDID == 'undefined' || item.SNOMEDID == null) { item.SNOMEDID = ''; }
                        if (typeof item.ICD10CodeDescription == 'undefined' || item.ICD10CodeDescription == null) { item.ICD10CodeDescription = ''; }
                        var diagnosis = item.ICD9Code + " - " + item.ICD10Code + " - " + item.SNOMEDID + " - " + item.ICD10CodeDescription;
                        var ICD9Code = "" + item.ICD9Code + "";
                        var ICD10Code = "" + item.ICD10Code + "";
                        var ICD9CodeDescription = "" + item.ICD9CodeDescription + "";
                        var ICD10CodeDescription = "" + item.ICD10CodeDescription + "";
                        var SNOMEDID = "" + item.SNOMEDID + "";
                        var SNOMEDDescription = "" + item.SNOMEDDescription + "";

                        var onclick = 'Clinical_FamilyHx.BindFamilyHxUl(\'' + item.ICD9Code + '\',\'' + item.ICD9CodeDescription + '\',\'' + item.ICD10Code + '\',\'' + item.ICD10CodeDescription + '\',\'' + item.SNOMEDID + '\',\'' + item.SNOMEDDescription + '\',this)';

                        var LiId = item.ICD9CodeDescription + '-' + item.SNOMEDID;

                        var isFound = Clinical_FamilyHx.isFavoriteHistoryFound(LiId, item.ICD9CodeDescription);
                        if (isFound == true) {
                            $UL.append('<li class="disableAll" onclick="' + onclick + '" id="' + LiId + '">' + item.ICD9CodeDescription + '</li>');
                        }
                        else {
                            $UL.append('<li onclick="' + onclick + '" id="' + LiId + '">' + item.ICD9CodeDescription + '</li>');
                        }

                        // $UL.append('<li onclick="' + onclick + '">' + item.ICD9CodeDescription + ' - ' + item.SNOMEDID + '</li>');

                    });
                }
            }
            else {
                $UL.empty();
            }
            //if (response.status != false) {
            //    var $UL = $('#' + Clinical_FamilyHx.params.PanelID + ' #ulFavoriteListFamilyHxContent');
            //    var objData = JSON.parse(response.FavoriteListCPTJSON);
            //    $UL.empty();

            //    $.each(objData, function (i, item) {
            //        if (item.CPTCodeDescription != "") {
            //            var onclick = 'Clinical_FamilyHx.BindFamilyHxUl(\'' + item.CPTCode + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + String(item.CPTCodeDescription) + '\')';
            //            $UL.append('<li onclick="' + onclick + '">' + item.CPTCodeDescription + '</li>');
            //        }
            //    });
            //}

        });
    },

    searchFavoriteList_ICD_DBCall: function (FavoriteListICDId, FavoriteListId, PageNumber, RowsPerPage, SearchData) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["IsActive"] = $('#' + Favorite_Problems.params.PanelID + ' #divSwitch #switchActive').is(':checked');
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["FavoriteListICDId"] = FavoriteListICDId == null ? 0 : FavoriteListICDId;
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = 0;
        }
        else {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        }
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["SearchData"] = SearchData;
        objData["commandType"] = "load_favoritelist_icd";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    BindFamilyHxUl: function (icd9Code, icd9Description, icd10Code, icd10Description, snomedCode, snomedDescription, sender) {

        var selectedMember = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active");
        if (selectedMember.length > 0) {

            var currId = -1;
            $("#pnlClinicalFamilyHx #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyHxDisease li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            //Start 19-10-2017 Humaira Yousaf MU3-5
            var info = icd10Code + " - (" + snomedCode + ") - \n" + icd10Description;
            globalAppdata["isMU3FamilyHistory"] && globalAppdata["isMU3FamilyHistory"].toLowerCase() == "false" ? info = "" : "";
            var li = "<li  title = \"" + info + "\" id=" + currId + " onclick='Clinical_FamilyHx.fillCurrentMemberDiseasesDetail(this, event);' onmouseover='Clinical_FamilyHx.showIcon(this);' onmouseout='Clinical_FamilyHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\" MemberDetailId=\"" + currId + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_FamilyHx.deleteFamilyHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>";
            //End 19-10-2017 Humaira Yousaf MU3-5

            var IsAlreadyExist = false;
            $('#pnlClinicalFamilyHx #ulFamilyHxDisease li').each(function () {
                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                    $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                    $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                $('#pnlClinicalFamilyHx #ulFamilyHxDisease').append(li);
                $(li).trigger('click');
                $('#pnlClinicalFamilyHx #txtDisease').val('');
                $(sender).addClass('disableAll');

                if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    var diseaseId = currId;// $('#' + Clinical_FamilyHx.params.PanelID + " #ulFamilyHxDisease > li.active").attr('id');
                    var memberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
                    var disease = $(li).get(0).outerHTML;
                    var detailsdata = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').clone();
                    $(detailsdata).resetAllControls(null);
                    var diseaseData = $(detailsdata).getMyJSONByName();


                    //
                    var updatJSON = diseaseData;
                    var RadYesRelativeDied = $(detailsdata).find('#RadYesRelativeDied').prop("checked");
                    var RadNoRelativeDied = $(detailsdata).find('#RadNoRelativeDied').prop("checked");
                    var RadRelativeDied = "";
                    if (RadYesRelativeDied) {
                        RadRelativeDied = true;
                    }
                    else if (RadNoRelativeDied) {
                        RadRelativeDied = false;
                    }

                    updatJSON = JSON.parse(updatJSON);
                    updatJSON.RadRelativeDied = RadRelativeDied;
                    updatJSON = JSON.stringify(updatJSON);
                    diseaseData = updatJSON;
                    //



                    Clinical_FamilyHx.cacheFamilyHxJSON(memberId, diseaseId, diseaseData, disease);

                    $('#' + Clinical_FamilyHx.params.PanelID + " #ulFamilyMember li.active").find("a").css("color", "green");
                }
            }
            else {
                utility.DisplayMessages('Diagnose already added', 2);

                $('#pnlClinicalFamilyHx #txtDisease').val('');
            }
        }
        else {
            utility.DisplayMessages('Select some Family Member and try again', 2);
        }

    },

    AddNewLabRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription) {

        var familyHxDiseaseUl = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ulFamilyHxDisease");

        // var rowId = CurrentRow.attr("id");

        var li = "<li  id=" + currId + " onclick='Clinical_FamilyHx.fillCurrentMemberDiseasesDetail(this, event);' onmouseover='Clinical_FamilyHx.showIcon(this);' onmouseout='Clinical_FamilyHx.hideIcon(this);' icd9Code=\"" + cptCode + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + procDesc + " - " + cptCode + "<span class='removeIconListHover' onclick='Clinical_FamilyHx.deleteFamilyHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>";

        //var cptCodeHtml = '<input type="hidden" id="CPTCode' + rowId + '"  name="CPTCode" value="' + cptCode + '" />';
        //var cptDescHtml = '<input type="hidden" id="CPTDescription' + rowId + '" name="CPTDescription"  value="' + procDesc + '"  />';

        $(familyHxDiseaseUl).append(li);
        //add cptCode and description to the test row

        // return CurrentRow;
    },
    //Author: Muhammad Irfan
    //Date: 21/01/2016
    //This function will handle filtering of FamilyHx Member
    filterFamilyMember: function (obj) {
        if (obj != null) {
            var strSearch = $(obj).val();
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ulFamilyMember li").each(function () {
                var currentLiText = $(this).text();
                var showCurrentLi = currentLiText.toLowerCase().indexOf(strSearch.toLowerCase()) > -1 ? true : false;
                $(this).toggle(showCurrentLi);
            });

        }
    },

    loadFamilyHxStatuses: function (crtl, methodName, reload, statusType) {
        //Start//26/01/2016//Ahmad Raza//resetting detail form when disease changes
        Clinical_FamilyHx.resetFamilyMemberControls();
        //End//26/01/2016//Ahmad Raza//resetting detail form when disease changes
        var currentLiClass = "";
        var currentLiClick = "";
        var ParentDiv = "";


        return MDVisionService.lookups(methodName, reload).done(function (result) {
            result = JSON.parse(result[methodName]);
            if ($(crtl).length > 0)
                l = $(crtl);

            l.empty();

            var isFirstLi = true;
            $.each(result, function (j, item) {
                if (item.Value != "") {
                    if (isFirstLi == true) {
                        currentLiClass = "";
                        isFirstLi = false;
                    }
                    else {
                        currentLiClass = "";
                    }

                    //var onClick = "Clinical_FamilyHx.markStatusActive('ulFamilyMember', " + item.Value + ");"
                    //item.Value = item.Value == "" ? 0 : item.Value;
                    //Start 19-10-2017 Humaira Yousaf MU3-5
                    var snomed = "";
                    if (item.RefValue != "") {
                        snomed = "SNOMED: " + item.RefValue;
                    }
                    var toolTip = "";
                    globalAppdata["isMU3FamilyHistory"] && globalAppdata["isMU3FamilyHistory"].toLowerCase() == "false" ? "" : toolTip = "title='" + snomed + "'";
                    l.append('<li ' + toolTip + ' id="' + item.Value + '" ' + currentLiClass + ' onclick="Clinical_FamilyHx.markStatusActive(this);"  value=' + item.Value + ' refValue="' + item.RefValue + '" memberDetailId = " ' + "-" + j + ' "><a class="text-bold" href="#' + ParentDiv + '">' + item.Name + '</a></li>');
                    //End 19-10-2017 Humaira Yousaf MU3-5
                }

            });
        });
    },

    //Author: Muhammad Irfan
    //Date: 20/01/2016
    //Overview: Following function is created to search ICD-9, ICD-10, Diagnosis in Clinical_FamilyHx

    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        var panel = "";
        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
            panel = "pnlClinicalProgressNote #pnlClinicalFamilyHx";
        }
        else {
            panel = "pnlClinicalFamilyHx";
        }
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_FamilyHx", null, false, panel);


    },

    //Author: Muhammad Irfan
    //Date: 21/01/2016
    //This function will handle load of diseases
    loadFamilyHxDiseases: function (crtl, result, statusType) {
        var currentLiClass = "";
        var currentLiClick = "";

        if ($(crtl).length > 0)
            l = $(crtl);
        l.empty();
        var isFirstLi = true;
        $.each(result, function (j, item) {
            var li = "";

            if (item.FreeTextICD != "") {
                if (item.No_DeseaseExists == "True") {
                    Clinical_FamilyHx.fillCurrentMemberDiseasesDetail('', '', item.DiseaseId);
                } else {
                    li = "<li  id=\"" + item.DiseaseId + "\" onclick='Clinical_FamilyHx.fillCurrentMemberDiseasesDetail(this, event);' onmouseover='Clinical_FamilyHx.showIcon(this);' onmouseout='Clinical_FamilyHx.hideIcon(this);' MemberDetailId=\"" + item.MemberDetailId + "\" ><a href='#'>" + item.FreeTextICD + "<span  class='removeIconListHover' onclick='Clinical_FamilyHx.deleteFamilyHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></li>"
                }
            } else {
                //Start 19-10-2017 Humaira Yousaf MU3-5
                var info = item.ICD10Code + " - (" + item.SNOMEDID + ") - \n" + item.ICD10CodeDescription;
                globalAppdata["isMU3FamilyHistory"] && globalAppdata["isMU3FamilyHistory"].toLowerCase() == "false" ? info = "" : "";
                li = "<li title = \"" + info + "\" id=\"" + item.DiseaseId + "\" onclick='Clinical_FamilyHx.fillCurrentMemberDiseasesDetail(this, event);' onmouseover='Clinical_FamilyHx.showIcon(this);' onmouseout='Clinical_FamilyHx.hideIcon(this);' icd9Code=\"" + item.ICD9Code + "\" icd9Desc=\"" + item.ICD9CodeDescription + "\" icd10Code=\"" + item.ICD10Code + "\" icd10Desc=\"" + item.ICD10CodeDescription + "\" snomedCode=\"" + item.SNOMEDID + "\" snomedDesc=\"" + item.SNOMEDDescription + "\" MemberDetailId=\"" + item.MemberDetailId + "\"><a href='#'>" + item.ICD9CodeDescription + "<span  class='removeIconListHover' onclick='Clinical_FamilyHx.deleteFamilyHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></li>"
                //End 19-10-2017 Humaira Yousaf MU3-5
            }
            //var li = "<li  id=\"" + item.DiseaseId + "\" onclick='Clinical_MedicalHx.fillMedicalHxDisease(this, event);' onmouseover='Clinical_MedicalHx.showIcon(this);' onmouseout='Clinical_MedicalHx.hideIcon(this);' icd9Code=\"" + item.ICD9Code + "\" icd9Desc=\"" + item.ICD9CodeDescription + "\" icd10Code=\"" + item.ICD10Code + "\" icd10Desc=\"" + item.ICD10CodeDescription + "\" snomedCode=\"" + item.SNOMEDID + "\" snomedDesc=\"" + item.SNOMEDDescription + "\"><a href='#'>" + item.ICD9CodeDescription + "<div id='deleteIcon' style='display:none' class='pull-right' onclick='Clinical_MedicalHx.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close red'></i></div></a></li>"
            l.append(li);
        });

        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.FamilyHx != null) {
            var memberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
            if (Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail != null && Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail.length > 0) {
                $(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail).each(function () {
                    if (($(this)[0].DiseaseLi != null || $(this)[0].DiseaseLi != undefined) && $(this)[0].MemberId == memberId) {
                        l.append($(this)[0].DiseaseLi);
                    }
                });
            }
        }

        var previouslySelectedDisease = Clinical_FamilyHx.previousSelectedDisease["diseaseId"];
        //var previouslySelectedMember = Clinical_FamilyHx.currentSelectedDisease["MemberId"];
        Clinical_FamilyHx.isTriggerManually = true;
        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyHxDisease ul#ulFamilyHxDisease li#" + previouslySelectedDisease).trigger('click');
    },

    //Author: Muhammad Irfan
    //Date: 21/01/2016
    //This function will handle fill of Familyhx component

    loadFamilyHxComponent: function () {
        var prevActiveMemberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
        Clinical_FamilyHx.loadFamilyHxStatuses('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyDisease #ulFamilyMember", "GetFamilyHxFamilyMember", true).done(function () {
            Clinical_FamilyHx.fillFamilyHx().done(function (response) {
                if (prevActiveMemberId != null && typeof prevActiveMemberId != "undefined") {
                    if (!$('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li#" + prevActiveMemberId).hasClass("active")) {
                        $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li#" + prevActiveMemberId).addClass("active")
                    }
                }

                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        var diseaseLoadDetails = JSON.parse(response.DiseaseLoad_JSON);

                        var membersLoadDetails = JSON.parse(response.MemberLoad_JSON);

                        var memberHasDisease = JSON.parse(response.MemberHasDisease_JSON || "[]");

                        //   Clinical_FamilyHx.loadFamilyHxDiseases('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyDisease #ulFamilyHxDisease", diseaseLoadDetails);



                        //Begin 12-28-2015 Muhammad Irfan Bug# EMR-161 Family History Clinical Module -> Date
                        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            /* Start 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                            // $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #dtFamilyHxDate").addClass("disableAll");
                            /* End 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                        }
                        //End 12-28-2015 Muhammad Irfan Bug# EMR-161 Family History Clinical Module -> Date

                        var familyHxDetail = JSON.parse(response.FamilyHxFill_JSON);
                        if (familyHxDetail.FamilyHxId > 0) {
                            Clinical_FamilyHx.params.HxTypeId = familyHxDetail.FamilyHxId;

                            if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                $('#' + Clinical_FamilyHx.params.PanelID + " #dtFamilyHxDate").removeClass('disableAll');
                            }
                            else {
                                $('#' + Clinical_FamilyHx.params.PanelID + " #dtFamilyHxDate").addClass('disableAll');
                            }

                            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #aHistory").removeClass('hidden');
                            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #aHistory").attr('onclick', 'Clinical_FamilyHx.showFamilyHxHistory(' + familyHxDetail.FamilyHxId + ')');

                            Clinical_FamilyHx.BindSoapTextGrid(familyHxDetail, diseaseLoadDetails.length > 0);
                        }
                        else {
                            var $row = $('<tr/>');
                            $row.append('<td>&nbsp;</td><td>No Known Family History</td><td></td>');
                            $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvFamilyHx tbody").html($row);
                            $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #divSwitch").addClass('disableAll');
                        }
                        $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val(familyHxDetail.FamilyHxId);
                        var self = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx");


                        utility.bindMyJSONByName(true, familyHxDetail, false, self).done(function () {
                            //Start//02/02/2016//Ahmad Raza// changed the implementation way of setDate on datepicker for Bug # EMR-225
                            var upperDate = self.find('input[name*="FamilyHxDate"]');

                            if (upperDate.val() == "") {
                                upperDate.datepicker('setDate', new Date());
                            } else {

                                //$(this).datepicker('setDate', $.datepicker.parseDate('yy-mm-dd', $(this).val()));
                                var date_format = 'mm/dd/yyyy';
                                //set default Date Formate
                                if (globalAppdata['DateFormat'])
                                    date_format = globalAppdata['DateFormat'];
                                //  $(this).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), $(this).val()));
                                upperDate.datepicker({ date_format: date_format.replace('yy', '') }).val(upperDate.val());
                                upperDate.datepicker("setDate", upperDate.val());
                            }

                            //End//02/02/2016//Ahmad Raza// changed the implementation way of setDate on datepicker for Bug # EMR-225

                            Clinical_FamilyHx.familyDate = $('#' + Clinical_FamilyHx.params.PanelID + " #dtFamilyHxDate").val();

                            if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.FamilyHx != null) {
                                $('#' + Clinical_FamilyHx.params.PanelID + " #dtFamilyHxDate").val(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyHxDate);
                            }

                            if (Clinical_HistorySummary.HistoryCacheList.FamilyHx != null && Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyOverallComments != '') {
                                $('#' + Clinical_FamilyHx.params.PanelID + " #txtFamilyOverallComments").val(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyOverallComments);
                            }
                            else {
                                $('#' + Clinical_FamilyHx.params.PanelID + " #txtFamilyOverallComments").val(familyHxDetail.FamilyOverallComments);
                                Clinical_FamilyHx.overallComments = $('#' + Clinical_FamilyHx.params.PanelID + " #txtFamilyOverallComments").val();
                            }

                            Clinical_FamilyHx.unremarkable = familyHxDetail.FamilyHxUnremarkable == "False" || familyHxDetail.FamilyHxUnremarkable == null ? false : true;

                            if (Clinical_HistorySummary.HistoryCacheList.FamilyHx != null) {
                                $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #chkFamilyHxUnremarkable").prop("checked", Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyHxUnremarkable);
                            }

                            Clinical_FamilyHx.params.mode = "Edit";


                        });
                        if (familyHxDetail.FamilyHxUnremarkable != null && typeof familyHxDetail.FamilyHxUnremarkable !== 'undefined') {
                            if (familyHxDetail.FamilyHxUnremarkable.toLowerCase() != "true") {
                                //var self = null;
                                //var familyHxDetail = null;
                                //Clinical_FamilyHx.bindCurrentTabJSON(FamilyHxType.toLowerCase(), response.TobaccoHxFill_JSON, "div#Tobacco", "#ulSmokingStatus");
                            }
                            else {
                                var chkUnremarkable = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #chkFamilyHxUnremarkable")
                                Clinical_FamilyHx.unRemarkableFamilyHx(chkUnremarkable, "1");
                            }
                        }

                        Clinical_FamilyHx.fillMembersDropDown(Clinical_FamilyHx.params.HxTypeId, memberHasDisease);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                }
                else {

                }

            });
        });
    },


    BindSoapTextGrid: function (familyHxDetail, isDiseaseLoad) {
        var $row = $('<tr/>');
        if (isDiseaseLoad) {

            $row.append('<td style="display:none;">' + familyHxDetail.FamilyHxId + '</td><td>' + familyHxDetail.IsCreatedOrModified + '</td><td>' + familyHxDetail.SoapText + '</td><td>' + familyHxDetail.LastUpdated + '</td>');
        }
        else {
            $row.append('<td>&nbsp;</td><td>No Known Family History</td><td></td>');
        }


        $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvFamilyHx tbody").html($row);
    },

    //Author: Muhammad Irfan
    //Date: 21/01/2016
    //This function will handle fill of Familyhx
    loadFamilyHx: function () {

        Clinical_FamilyHx.loadFamilyHxComponent();

    },

    //Author: Muhammad Irfan
    //Date: 21/01/2016
    //This function will handle fill of FamilyMember Details as specified by Given JSON

    fillCurrentMemberDiseases: function (famlyHxId, familyMemberId) {
        Clinical_FamilyHx.fillMemberDiseaseDB_Call(famlyHxId, familyMemberId).done(function (response) {
            //Start//26/01/2016//Ahmad Raza//resetting detail form when disease changes
            Clinical_FamilyHx.resetFamilyMemberControls();
            //End//26/01/2016//Ahmad Raza//resetting detail form when disease changes
            response = JSON.parse(response);

            if (response.status != false) {
                //response = JSON.parse(response);
                if (response.DiseaseLoad_JSON != null) {

                    var diseaseLoadDetails = JSON.parse(response.DiseaseLoad_JSON);
                    Clinical_FamilyHx.loadFamilyHxDiseases('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyDisease #ulFamilyHxDisease", diseaseLoadDetails);

                    if ($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyHxDisease ul#ulFamilyHxDisease li").length > 0) {
                        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyHxDisease ul#ulFamilyHxDisease li:first").click();
                    }
                    else {
                        // $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails').addClass("disableAll");
                    }
                } else {
                    $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyDisease #ulFamilyHxDisease").empty();

                    if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.FamilyHx != null) {
                        var ctrl = '#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyDisease #ulFamilyHxDisease";

                        var memberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
                        if (Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail != null && Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail.length > 0) {
                            $(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail).each(function () {
                                if (($(this)[0].DiseaseLi != null || $(this)[0].DiseaseLi != undefined) && $(this)[0].MemberId == memberId) {
                                    $(ctrl).append($(this)[0].DiseaseLi);
                                }
                            });
                        }

                        if ($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyHxDisease ul#ulFamilyHxDisease li").length > 0) {
                            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyHxDisease ul#ulFamilyHxDisease li:first").click();
                        }

                        if (Clinical_HistorySummary.HistoryCacheList.FamilyHx && Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease
                            && Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail) {
                            var diseases = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyDisease #ulFamilyHxDisease li").length;
                            if (diseases == 0) {
                                var cachedJSON = Clinical_FamilyHx.getCacheFamilyHxJSON(memberId, -1);
                                if (cachedJSON != '') {
                                    Clinical_FamilyHx.fillCurrentMemberDiseasesDetail('', '', -1);
                                }
                            }
                        }
                    }
                    // $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails').addClass("disableAll");
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    fillCurrentMemberDiseasesDetail: function (obj, event, deseaseID) {

        var diseaseId = "";
        if (obj != "") {
            diseaseId = $(obj).attr('id');
        }
        else {
            diseaseId = deseaseID;
        }
        var memberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
        var memberIndex = 0;

        var diseaseIndex = 0
        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_FamilyHx.familyHxJSON != '') {
            var updatedJSON = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').getMyJSONByName();

            var RadYesRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked");
            var RadNoRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked");
            var RadRelativeDied = "";
            if (RadYesRelativeDied) {
                RadRelativeDied = true;
            }
            else if (RadNoRelativeDied) {
                RadRelativeDied = false;
            }

            updatedJSON = JSON.parse(updatedJSON);
            updatedJSON.RadRelativeDied = RadRelativeDied;
            updatedJSON = JSON.stringify(updatedJSON);
            if (Clinical_FamilyHx.familyHxJSON != updatedJSON) {
                memberDiseaseaId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyHxDisease li.active").attr('id');
                if (memberDiseaseaId != null) {
                    Clinical_FamilyHx.cacheFamilyHxJSON(memberId, memberDiseaseaId, updatedJSON);
                }
            }
        }

        Clinical_FamilyHx.fillMemberDetailDB_Call(memberId, diseaseId).done(function (response) {
            //Start//26/01/2016//Ahmad Raza//resetting detail form when disease changes
            Clinical_FamilyHx.resetFamilyMemberControls();
            //End//26/01/2016//Ahmad Raza//resetting detail form when disease changes

            response = JSON.parse(response);

            if (response.status != false) {


                if (response.FamilyHxFill_JSON != null) {

                    var membersFillJSON = JSON.parse(response.FamilyHxFill_JSON);

                    //response = JSON.parse(response);

                    if (obj != "") {
                        $(obj).attr("MemberDetailId", response.MemberDetailIds);
                    }
                    else {
                        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfMemberdetailid").val(response.MemberDetailIds);
                    }

                    //var membersFillJSON = JSON.parse(response.FamilyHxFill_JSON);
                    var self = $('#' + Clinical_FamilyHx.params.PanelID + ' #sectionFamilyMemberDetails');
                    var detailsJSON = '';
                    if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                        var cachedJSON = Clinical_FamilyHx.getCacheFamilyHxJSON(memberId, diseaseId);
                        if (cachedJSON != '') {
                            detailsJSON = JSON.parse(cachedJSON);
                        }
                        else {
                            detailsJSON = membersFillJSON
                        }
                    }
                    else {
                        detailsJSON = membersFillJSON
                    }

                    utility.bindMyJSONByName(true, detailsJSON, false, self).done(function () {

                        //Clinical_FamilyHx.fillFamilyHxDisease(obj,event);

                        //Start//11-02-2016//Ahmad Raza//Fixed EMR Bug#318
                        var yearOfBirth = self.find('input[name*="YearofBirth"]');

                        if (yearOfBirth.val() == "") {
                            // yearOfBirth.datepicker('setDate', new Date());
                        } else {

                            //$(this).datepicker('setDate', $.datepicker.parseDate('yy-mm-dd', $(this).val()));
                            var dateFormat = 'mm/dd/yyyy';
                            //set default Date Formate
                            if (globalAppdata['DateFormat'])
                                dateFormat = globalAppdata['DateFormat'];
                            //  $(this).datepicker("setDate", $.datepicker.formatDate(dateFormat.replace('yy', ''), $(this).val()));
                            yearOfBirth.datepicker({ dateFormat: dateFormat.replace('yy', '') }).val(yearOfBirth.val());
                            yearOfBirth.datepicker("setDate", yearOfBirth.val());
                        }
                        //End//11-02-2016//Ahmad Raza//Fixed EMR Bug#318
                        Clinical_FamilyHx.params.mode = "Edit";
                        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').prop('disabled', false);
                        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').css("background", "#fff");
                        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').css("border", "1px solid #ccc");
                        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtFamilyMemberComments').prop('disabled', false);

                        //Start//28/01/2016//Ahmad Raza//when member detail is filled and if Health status is Alive, disabling the Yes CheckBox and age at death field
                        if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease #ddlHealthStatus option:selected').val() == 1) {
                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop('disabled', true);
                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', true);
                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').val('');



                            // Start || 29 August, 2016 || Talha Tanweer || EMR-748
                            //$('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', true);
                            //$('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#EEEEEE");
                            //$('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #cccccc");

                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked", false);
                            // End   || 29 August, 2016 || Talha Tanweer || EMR-748

                        }
                        //End//28/01/2016//Ahmad Raza//when member detail is filled and if Health status is Alive, disabling the Yes CheckBox and age at death field
                        if (detailsJSON.RadRelativeDied.toLowerCase() == "true") {
                            $('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #RadYesRelativeDied").prop("checked", true);
                            $('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #RadNoRelativeDied").prop("checked", false);

                            // Start || 29 August, 2016 || Talha Tanweer || EMR-748
                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', false);
                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#fff");
                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #ccc");

                            // End   || 29 August, 2016 || Talha Tanweer || EMR-748

                        }
                        else if (detailsJSON.RadRelativeDied.toLowerCase() == "false") {

                            //Start//10/02/2016//Abid Ali//, for emr bug# 295
                            //$('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #txtAgeAtDeath").prop("disabled", true)
                            //End//10/02/2016//Abid Ali//, for emr bug# 295
                            $('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #RadNoRelativeDied").prop("checked", true);

                            // Start || 29 August, 2016 || Talha Tanweer || EMR-748
                            //$('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', true);
                            //$('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#EEEEEE");
                            //$('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #cccccc");

                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked", false);
                            // End   || 29 August, 2016 || Talha Tanweer || EMR-748



                        }
                        else {
                            //Start//10/02/2016//Abid Ali//, for emr bug# 295
                            $('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #txtAgeAtDeath").prop("disabled", false)
                            //End//10/02/2016//Abid Ali//, for emr bug# 295
                            $('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #RadNoRelativeDied,#RadYesRelativeDied").prop("checked", false);


                            // Start || 29 August, 2016 || Talha Tanweer || EMR-748
                            if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease #ddlHealthStatus option:selected').val() == "1") {

                                $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', true);
                                $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#EEEEEE");
                                $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #cccccc");

                                $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked", false);


                            }
                            else {

                                $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', false);
                                $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#fff");
                                $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #ccc");
                            }
                            // End   || 29 August, 2016 || Talha Tanweer || EMR-748

                        }
                        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            Clinical_FamilyHx.familyHxJSON = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').getMyJSONByName();


                            //
                            var updatJSON = Clinical_FamilyHx.familyHxJSON;
                            var RadYesRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked");
                            var RadNoRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadNoRelativeDied').prop("checked");
                            var RadRelativeDied = "";
                            if (RadYesRelativeDied) {
                                RadRelativeDied = true;
                            }
                            else if (RadNoRelativeDied) {
                                RadRelativeDied = false;
                            }

                            updatJSON = JSON.parse(updatJSON);
                            updatJSON.RadRelativeDied = RadRelativeDied;
                            updatJSON = JSON.stringify(updatJSON);
                            Clinical_FamilyHx.familyHxJSON = updatJSON;
                            //

                        }
                    });
                }

                else {
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').prop('disabled', false);
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').css("background", "#fff");
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').css("border", "1px solid #ccc");
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtFamilyMemberComments').prop('disabled', false);

                    // Start   || 29 August, 2016 || Talha Tanweer || EMR-748
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', false);
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#fff");
                    $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #ccc");
                    // End   || 29 August, 2016 || Talha Tanweer || EMR-748

                    if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {

                        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            Clinical_FamilyHx.familyHxJSON = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').getMyJSONByName();

                            //
                            var updatJSON = Clinical_FamilyHx.familyHxJSON;
                            var RadYesRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked");
                            var RadNoRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadNoRelativeDied').prop("checked");
                            var RadRelativeDied = "";
                            if (RadYesRelativeDied) {
                                RadRelativeDied = true;
                            }
                            else if (RadNoRelativeDied) {
                                RadRelativeDied = false;
                            }

                            updatJSON = JSON.parse(updatJSON);
                            updatJSON.RadRelativeDied = RadRelativeDied;
                            updatJSON = JSON.stringify(updatJSON);
                            Clinical_FamilyHx.familyHxJSON = updatJSON;
                            //
                        }

                        var cachedJSON = Clinical_FamilyHx.getCacheFamilyHxJSON(memberId, diseaseId);
                        if (cachedJSON != '') {
                            detailsJSON = JSON.parse(cachedJSON);
                            if (detailsJSON != null) {
                                var self = $('#' + Clinical_FamilyHx.params.PanelID + ' #sectionFamilyMemberDetails');
                                utility.bindMyJSONByName(true, detailsJSON, false, self).done(function () {
                                    if (detailsJSON.RadRelativeDied.toLowerCase() == "true") {
                                        $('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #RadYesRelativeDied").prop("checked", true);
                                        $('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #RadNoRelativeDied").prop("checked", false);

                                        // Start || 29 August, 2016 || Talha Tanweer || EMR-748
                                        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', false);
                                        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#fff");
                                        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #ccc");

                                        // End   || 29 August, 2016 || Talha Tanweer || EMR-748

                                    }
                                    else if (detailsJSON.RadRelativeDied.toLowerCase() == "false") {

                                        //Start//10/02/2016//Abid Ali//, for emr bug# 295
                                        //$('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #txtAgeAtDeath").prop("disabled", true)
                                        //End//10/02/2016//Abid Ali//, for emr bug# 295
                                        $('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #RadNoRelativeDied").prop("checked", true);

                                        // Start || 29 August, 2016 || Talha Tanweer || EMR-748
                                        //$('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', true);
                                        //$('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#EEEEEE");
                                        //$('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #cccccc");

                                        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked", false);
                                        // End   || 29 August, 2016 || Talha Tanweer || EMR-748



                                    }
                                    else {
                                        //Start//10/02/2016//Abid Ali//, for emr bug# 295
                                        $('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #txtAgeAtDeath").prop("disabled", false)
                                        //End//10/02/2016//Abid Ali//, for emr bug# 295
                                        $('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails #RadNoRelativeDied,#RadYesRelativeDied").prop("checked", false);


                                        // Start || 29 August, 2016 || Talha Tanweer || EMR-748
                                        if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease #ddlHealthStatus option:selected').val() == "1") {

                                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', true);
                                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#EEEEEE");
                                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #cccccc");

                                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked", false);


                                        }
                                        else {

                                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', false);
                                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("background", "#fff");
                                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').css("border", "1px solid #ccc");
                                        }
                                        // End   || 29 August, 2016 || Talha Tanweer || EMR-748

                                    }

                                });
                            }

                        }

                        var updatedJSON = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').getMyJSONByName();


                        //
                        var updatJSON = updatedJSON;
                        var RadYesRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked");
                        var RadNoRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadNoRelativeDied').prop("checked");
                        var RadRelativeDied = "";
                        if (RadYesRelativeDied) {
                            RadRelativeDied = true;
                        }
                        else if (RadNoRelativeDied) {
                            RadRelativeDied = false;
                        }

                        updatJSON = JSON.parse(updatJSON);
                        updatJSON.RadRelativeDied = RadRelativeDied;
                        updatJSON = JSON.stringify(updatJSON);
                        updatedJSON = updatJSON;
                        //

                        if (Clinical_FamilyHx.familyHxJSON != updatedJSON) {
                            if (diseaseId != null) {
                                Clinical_FamilyHx.cacheFamilyHxJSON(memberId, diseaseId, updatedJSON);
                            }
                        }
                    }
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3)
            }

            if (obj != "") {
                Clinical_FamilyHx.fillFamilyHxDisease(obj, event);
            }
            else {
                Clinical_FamilyHx.fillFamilyHxDisease("", "", diseaseId);
            }
        });
    },
    //Author: Muhammad Irfan
    //Date: 21/01/2016
    //This function will handle fill of FamilyMember Details as specified by Given JSON

    resetFamilyMemberControls: function () {
        var objDeffered = $.Deferred();
        var self = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease section#sectionFamilyMemberDetails div#FamilyMemberDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
            Clinical_FamilyHx.resetControlValue(this);
        });
        objDeffered.resolve();
        //Start//29/01/2016//Ahmad Raza//Enable/Disable fields on member selection change
        if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #ddlHealthStatus').val() == 1) {
            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop('disabled', true);
            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', true);
            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').val('');
        }
        else {
            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop('disabled', false);
            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDeath').prop('disabled', false);

        }
        //End//29/01/2016//Ahmad Raza//Enable/Disable fields on member selection change

        return objDeffered;
    },

    //Author: Muhammad Irfan
    //Date: 22/01/2016
    //This function will handle active state of current li item on the basis of li's Id
    markStatusActive: function (obj) {
        //if (ulId != null && ulId != "") {

        var currentId = $(obj).attr('id');

        //  Clinical_FamilyHx.currentSelectedMember["memberId"] = currentId;

        for (var index in Clinical_FamilyHx.currentSelectedDisease) {
            var selectedDisease = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfPreviousSelectedDisease").val();
            if (Clinical_FamilyHx.currentSelectedDisease[index].diseaseId == selectedDisease) {
                var isExists = false;
                if (Clinical_FamilyHx.currentSelectedDisease[index].Members != null) {
                    for (var memIndex in Clinical_FamilyHx.currentSelectedDisease[index].Members) {
                        if (Clinical_FamilyHx.currentSelectedDisease[index].Members[memIndex].memberId = currentId) {
                            isExists = true;

                        }
                    }
                }
                else {
                    Clinical_FamilyHx.currentSelectedDisease[index].Members = [];
                }
                if (isExists == false) {

                    Clinical_FamilyHx.currentSelectedDisease[index].Members.push(
                    {
                        memberId: currentId

                    });
                }

            }
        }
        var currentMemberDetailId = $(obj).attr('memberdetailid');
        var diseaseId = $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease li.active").attr('id');
        Clinical_FamilyHx.currentSelectedDisease.MemberId = currentId;

        //Clinical_FamilyHx.fillCurrentMember(currentMemberDetailId, currentId, diseaseId);

        var familyHxId = Clinical_FamilyHx.params.HxTypeId;


        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_FamilyHx.familyHxJSON != '') {
            var updatedJSON = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').getMyJSONByName();

            var RadYesRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadYesRelativeDied').prop("checked");
            var RadNoRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #RadNoRelativeDied').prop("checked");
            var RadRelativeDied = "";
            if (RadYesRelativeDied) {
                RadRelativeDied = true;
            }
            else if (RadNoRelativeDied) {
                RadRelativeDied = false;
            }

            updatedJSON = JSON.parse(updatedJSON);
            updatedJSON.RadRelativeDied = RadRelativeDied;
            updatedJSON = JSON.stringify(updatedJSON);

            if (Clinical_FamilyHx.familyHxJSON != updatedJSON) {
                var memberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
                memberDiseaseaId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyHxDisease li.active").attr('id');
                Clinical_FamilyHx.cacheFamilyHxJSON(memberId, memberDiseaseaId, updatedJSON);
            }
        }

        Clinical_FamilyHx.fillCurrentMemberDiseases(familyHxId, currentId);

        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyMember li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == currentId) {
                if ($(this).hasClass("active") == false) {
                    $(this).addClass("active");
                    var diagnosisDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#DiagnosisList');
                    diagnosisDiv.removeClass("disableAll");
                    //var memberDetailDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails');
                    //memberDetailDiv.removeClass("disableAll");
                    Clinical_FamilyHx.resetFamilyMemberControls();
                }
            }
            else {
                if ($(this).hasClass("active") == true) {
                    $(this).removeClass("active");
                }
            }
        });
        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails').removeClass("disableAll");
        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').prop('disabled', true);
        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').css("background", "#EEEEEE");
        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').css("border", "1px solid #cccccc");
        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails #txtFamilyMemberComments").prop('disabled', true);


    },

    //Author: Muhammad Irfan
    //Date: 22/01/2016
    //This function will handle unremarkable of FamilyHx
    unRemarkableFamilyHx: function (obj, isFromLoad) {
        var isRemarkable = $(obj).prop("checked");
        if (isRemarkable == true) {
            $('#' + Clinical_FamilyHx.params.PanelID + ' #FamilyDisease').resetAllControls(null);
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ulFamilyMember").find("a").css("color", "gray");
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ulFamilyMember li").removeClass('active');
            if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Clinical_FamilyHx.params.PanelID + ' #btnFamilyHxSave').hide();
            }
            else {
                $('#' + Clinical_FamilyHx.params.PanelID + ' #btnFamilyHxSave').show();
            }
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease").addClass('disableAll');

            $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease").empty();
            Clinical_FamilyHx.enableDisbaleUlItems('ulFavoriteListFamilyHxContent', false);
            if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.FamilyHx != null) {
                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease = [];
                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail = [];
            }
        }
        else {
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease").removeClass('disableAll');
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #DiagnosisList").addClass('disableAll');
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyMemberDetails").addClass('disableAll');
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #rdofreetext").prop("checked", true);
            $('#' + Clinical_FamilyHx.params.PanelID + ' #btnFamilyHxSave').hide();
            Clinical_FamilyHx.enableDisbaleUlItems('ulFavoriteListFamilyHxContent', true);
        }
    },

    //Author: Muhammad Irfan
    //Date: 20/01/2016
    //This function will clear value of given control as specified by obj

    resetControlValue: function (obj) {
        var currentElementTagName = obj.tagName != null ? obj.tagName : obj.prop("tagName");
        if ($(obj).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea')
            $(obj).val('');
        if ($(obj).attr('type') == 'checkbox' || $(obj).attr('type') == 'radio') {

            if ($(obj).attr('type') == 'radio') {
                obj.checked = false;
                //Begin 28-12-2015 Muhammad Irfan Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
                var groupRadBtn = $("input[name='" + $(obj).attr('name') + "']");
                if (groupRadBtn.length > 1) {
                    $.each(groupRadBtn, function (i, item) {
                        if ($(item).attr("id").toLowerCase().indexOf("no") > -1) {
                            $(item).trigger("click");
                        }
                    });
                }
                //End 28-12-2015 Muhammad Irfan Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
            }
            else {
                obj.checked = false;
            }
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

    //Author: Abid Ali
    //Date: 22/01/2016
    //This function will calculate age in years
    calculateAgeInYearsTillNow: function (obj, fromDateId) {

        //if (fromDateId == "#dtpYearofBirth" && $(fromDateId).val() == "") {
        //    fromDateId = "#txtAgeAtDeath";
        //}
        var fromDate = new Date($(fromDateId).val());
        var toDate = new Date();
        var Totalyears = "";
        if ($(fromDateId).val() != "") {
            var second = 1000;
            var minute = second * 60;
            var hour = minute * 60;
            var day = hour * 24;
            var week = day * 7;

            var timediff = toDate - fromDate;
            var years = toDate.getFullYear() - fromDate.getFullYear();
            var months = (toDate.getFullYear() * 12 + toDate.getMonth()) - (fromDate.getFullYear() * 12 + fromDate.getMonth());
            var days = Math.floor(timediff / day);
            var hours = Math.floor(timediff / hour);
            var minutes = Math.floor(timediff / minute);
            var seconds = Math.floor(timediff / second);
            var weeks = Math.floor(timediff / week);
            var age = ~~((Date.now() - fromDate) / (31557600000));

            diff = new Date(
            toDate.getFullYear() - fromDate.getFullYear(),
            toDate.getMonth() - fromDate.getMonth(),
            toDate.getDate() - fromDate.getDate()
            );
            var ageInYears = diff.getYear();
            var ageInMonths = diff.getMonth();
            var ageInDays = diff.getDate();

            ageInYears = ageInYears + (toDate.getMonth() + 1) / 12;

        }
        var errorMessage = "Please Enter valid " + $(obj).parent().parent().find('label').text();

        var ageAtDeath = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #txtAgeAtDeath").val();
        var ageAtDiagnosis = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #txtAgeAtDiagnosis").val();

        if (ageAtDeath == 0 && ageAtDeath != "" && $(obj).attr('id') == "txtAgeAtDeath") {
            errorMessage = "Age at Death cannot be equal to zero.";
            utility.DisplayMessages(errorMessage, 3);
            $(obj).val("");

        }
        if (ageAtDiagnosis == 0 && ageAtDiagnosis != "" && $(obj).attr('id') == "txtAgeAtDiagnosis") {
            errorMessage = "Age at Diagnosis cannot be equal to zero.";
            utility.DisplayMessages(errorMessage, 3);
            $(obj).val("");

        }
        var ageEntered = parseInt($(obj).val());
        ageEntered = parseFloat($(obj).val());

        if (ageInYears != null && !(ageInYears >= 0 && ageEntered <= ageInYears)) {
            if ($(obj).val() != "")
                if ($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #dtpYearofBirth").val() == "" && $(obj).attr('id') == "txtAgeAtDeath") {

                } else if ($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #dtpYearofBirth").val() == "" && $(obj).attr('id') == "txtAgeAtDiagnosis") {
                    if (parseFloat(ageAtDiagnosis) > parseFloat(ageAtDeath)) {
                        utility.DisplayMessages(errorMessage, 3);
                        $(obj).val("");
                    }
                }
                else {
                    utility.DisplayMessages(errorMessage, 3);
                    $(obj).val("");
                }
        }
            //Start/Abid Ali, for bug# EMR-336
        else if (ageAtDeath != "" && typeof ageAtDeath != "undefined" && $(obj).attr('id') == "txtAgeAtDiagnosis") {

            if (parseFloat(ageEntered) > parseFloat(ageAtDeath)) {
                errorMessage = "Age at Diagnosis cannot be more recent that Age at Death.";
                utility.DisplayMessages(errorMessage, 3);
                // clear textbox value.
                $(obj).val("");
            }
        }
            //End/Abid Ali, for bug# EMR-336

            //Start/Abid Ali, for bug# EMR-857
        else if (ageAtDiagnosis != "" && typeof ageAtDiagnosis != "undefined" && $(obj).attr('id') == "txtAgeAtDeath") {

            if (parseFloat(ageEntered) < parseFloat(ageAtDiagnosis)) {
                errorMessage = "Age at Death cannot be less than Age at Diagnosis.";
                utility.DisplayMessages(errorMessage, 3);
                // clear textbox value.
                $(obj).val("");
            }
        }
        //End/Abid Ali, for bug# EMR-857

    },

    reValidateAges: function () {

        Clinical_FamilyHx.calculateAgeInYearsTillNow(('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #txtAgeAtDiagnosis"), '#dtpYearofBirth');
        Clinical_FamilyHx.calculateAgeInYearsTillNow(('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #txtAgeAtDeath"), '#dtpYearofBirth');

    },

    //Author: Muhammad Irfan
    //Date: 12-28-2015
    //This function will check if details has any value for selected status
    //Begin 12-28-2015 Muhammad Irfan Bug# EMR-159 Family History Clinical Module -> Save
    isDetailExists: function (tabType) {
        var detailExists = false;
        var sectionDetails = "";
        var self = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease section#sectionFamilyMemberDetails div#FamilyMemberDetails").find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
            if ($(this).prop("disabled") != true && detailExists == false) {
                var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {
                    detailExists = true;
                }
                if ($(this).attr('type') == 'checkbox' && this.checked == true) {
                    detailExists = true;
                }
                if ($(this).attr('type') == 'radio' && $(this).attr('id').toLowerCase().indexOf("no") > -1 && this.checked == true) {
                    detailExists = false;
                }
                if (currentElementTagName.toLowerCase() == 'select' && $(this).val() != null && $(this).val() != "") {
                    detailExists = true;
                }
                //if (currentElementTagName.toLowerCase() == 'ul') {
                //    $(this).find('li.active').removeClass('active');
                //}
            }
        });

        return detailExists;

    },
    //Author: Muhammad Irfan
    //Date: 12-28-2015
    //This function will save Family Hx
    familyHxSave: function (familyHxType, unloadFamilyhx, attachToNote) {

        var familyHxId = $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val() != "" ? $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val() : "-1";
        if (parseInt(familyHxId) > 0) {
            Clinical_FamilyHx.params.mode = "Edit";
        }
        else {
            Clinical_FamilyHx.params.mode = "Add";
        }
        //Start//29/01/2016//Ahmad Raza//logic to check if data exists in detail section

        //Start//02/02/2016//Ahmad Raza// fixed bug#294
        //Start//26/12/2016//Zain ul abdin// IMP-112
        //var overallComments = $('#' + Clinical_FamilyHx.params.PanelID + " #txtFamilyOverallComments").val();
        //if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote" && overallComments != "") {

        //    var detailExists = true;
        //}
        //else {
        //    detailExists = Clinical_FamilyHx.isDetailExists(familyHxType.toLowerCase());
        //}
        ////End//02/02/2016//Ahmad Raza// fixed bug#294

        //if ($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #chkFamilyHxUnremarkable").is(':checked')) {
        //    detailExists = true;
        //}
        //if (overallComments != "") {
        //    detailExists = true;
        //}
        //End//26/12/2016//Zain ul abdin// IMP-112

        var detailExists = true;
        if (detailExists == true) {
            var strMessage = "";
            var self = null;
            self = $('#' + Clinical_FamilyHx.params.PanelID + " section#sectionFamilyMemberDetails");
            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);

            var unremarkValue = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #chkFamilyHxUnremarkable").prop("checked");
            if (unremarkValue == false) {
                if ($('#' + Clinical_FamilyHx.params.PanelID + " #ulFamilyHxDisease li").length == 0 && objData.AgeAtDeath == "" && objData.AgeAtDiagnosis == "" && objData.FamilyMemberComments == "" && objData.HealthStatus == "" && objData.RadRelativeDied == false && objData.YearofBirth == "" && $('#' + Clinical_FamilyHx.params.PanelID + " #RadYesRelativeDied").prop("checked") == false && $('#' + Clinical_FamilyHx.params.PanelID + " #RadNoRelativeDied").prop("checked") == false) {
                    utility.DisplayMessages("Please select family member detail", 3);
                    return;
                }
            }

            var selectedDisease = $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyDisease ul#ulFamilyHxDisease li.active");
            if (selectedDisease.length > 0) {

                objData["DiseaseId"] = selectedDisease.attr("id");
                objData["Disease_text"] = selectedDisease.text();
                objData["MemberDetailId"] = selectedDisease.attr("MemberDetailId");

            }
            else {
                if ($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfDiseaseId").val() != "") {
                    objData["DiseaseId"] = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfDiseaseId").val()
                } else {
                    objData["DiseaseId"] = "-1";
                }

                objData["Disease_text"] = "";
                if ($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfMemberdetailid").val() != "") {
                    objData["MemberDetailId"] = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfMemberdetailid").val();
                }
                else {
                    objData["MemberDetailId"] = 0;
                }
            }

            if (selectedDisease.length > 0) {

                if (selectedDisease.attr('icd9desc') && selectedDisease.attr('icd9desc') != '') {//($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #rdofreetext").prop("checked") == false) {
                    objData["ICD9Code"] = selectedDisease.attr("icd9code");
                    objData["ICD9CodeDescription"] = selectedDisease.attr("icd9desc");
                    objData["ICD10Code"] = selectedDisease.attr("icd10code");
                    objData["ICD10CodeDescription"] = selectedDisease.attr("icd10desc");
                    objData["SNOMEDID"] = selectedDisease.attr("snomedcode");
                    objData["SNOMEDDescription"] = selectedDisease.attr("snomeddesc");

                } else {
                    objData["FreeTextICD"] = selectedDisease.text() != "" ? selectedDisease.text() : "-1";
                    objData["ICD9Code"] = "";
                    objData["ICD9CodeDescription"] = "";
                    objData["ICD10Code"] = "";
                    objData["ICD10CodeDescription"] = "";
                    objData["SNOMEDID"] = "";
                    objData["SNOMEDDescription"] = "";
                }
            }
            else {
                objData["FreeTextICD"] = "-1";
            }

            //-------------------------------------------------------------------
            objData["FamilyHxId"] = $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val();
            objData["FamilyHxDate"] = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #dtFamilyHxDate").val();
            objData["FamilyHxUnremarkable"] = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #chkFamilyHxUnremarkable").prop("checked");
            objData["FamilyOverallComments"] = $('#' + Clinical_FamilyHx.params.PanelID + " #txtFamilyOverallComments").val();

            //-----------------------------------------------------------------------

            var selectedMember = $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyDisease ul#ulFamilyMember li.active");
            objData["MemberId"] = selectedMember.attr("id");
            objData["Disease_text"] = selectedMember.text();


            //---------------------------------------------------------------------

            myJSON = JSON.stringify(objData);
            //return false;
            if (Clinical_FamilyHx.params.mode == "Add") {
                //Start//21/01/2016//Muhammad Irfan//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("History_Family Hx", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Clinical_FamilyHx.saveFamilyHx(myJSON).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Clinical_FamilyHx.SaveFavToggelStatus($('#' + Clinical_FamilyHx.params.PanelID + ' #ddlFavoriteListFamilyHx').val());
                                Clinical_FamilyHx.params.HxTypeId = response.FamilyHxId;
                                $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #divSwitch").removeClass('disableAll');
                                Clinical_FamilyHx.ChangeCurrentPast(1, null, null, null);
                                if (attachToNote != false) {
                                    Clinical_FamilyHx.BindCurrentFamilySoapText(response);
                                    if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote" && unloadFamilyhx == true) {
                                        Clinical_FamilyHx.getFamilyHxInfo(familyHxType, unloadFamilyhx, null, true);
                                    }
                                }
                                utility.DisplayMessages(response.message, 1);
                                $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val(response.FamilyHxId);
                                Clinical_FamilyHx.params.mode = "Edit";
                                //Start//26/01/2016//Ahmad Raza//setting Id's returned in response
                                if (response.diseaseId > 0) {
                                    $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease li.active").attr('id', response.diseaseId);
                                    Clinical_FamilyHx.previousSelectedDisease['diseaseId'] = response.diseaseId;
                                    $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfDiseaseId").val(response.diseaseId);
                                }
                                if (response.memberdetailid > 0) {
                                    if ($('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease li.active").length > 0) {
                                        $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease li.active").attr('memberdetailid', response.memberdetailid)
                                        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfMemberdetailid").val(response.memberdetailid);
                                    }
                                    else {
                                        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfMemberdetailid").val(response.memberdetailid);
                                    }
                                    $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ulFamilyMember li.active").find("a").css("color", "green");
                                }
                                //End//26/01/2016//Ahmad Raza//setting Id's returned in response
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
                //End//21/01/2016//Muhammad Irfan//Logic implemented to check privileges
            }
            else if (Clinical_FamilyHx.params.mode == "Edit") {
                //Start//21/01/2016//Muhammad Irfan//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("History_Family Hx", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Clinical_FamilyHx.updateFamilyHx(myJSON, Clinical_FamilyHx.params.FamilyHxId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Clinical_FamilyHx.SaveFavToggelStatus($('#' + Clinical_FamilyHx.params.PanelID + ' #ddlFavoriteListFamilyHx').val());
                                $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #divSwitch").removeClass('disableAll');
                                Clinical_FamilyHx.ChangeCurrentPast(1, null, null, null);
                                if (attachToNote != false) {
                                    Clinical_FamilyHx.BindCurrentFamilySoapText(response);
                                    if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote" && unloadFamilyhx == true) {
                                        Clinical_FamilyHx.getFamilyHxInfo(familyHxType, unloadFamilyhx);
                                    } else {
                                        utility.DisplayMessages(response.message, 1);
                                    }
                                }
                                //Clinical_FamilyHx.AppointmentStatusSearch(Clinical_FamilyHx.params.FamilyHxignsId);

                                //Start//26/01/2016//Ahmad Raza//setting Id's returned in response
                                if (response.diseaseId > 0) {
                                    $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease li.active").attr('id', response.diseaseId);
                                    Clinical_FamilyHx.previousSelectedDisease['diseaseId'] = response.diseaseId;
                                    $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfDiseaseId").val(response.diseaseId);
                                }
                                if (response.memberDetailId > 0) {
                                    if ($('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease li.active").length > 0) {
                                        $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease li.active").attr('memberdetailid', response.memberDetailId)
                                        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfMemberdetailid").val(response.memberDetailId);
                                    }
                                    else {
                                        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfMemberdetailid").val(response.memberDetailId);
                                    }



                                    //$('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ulFamilyMember li.active").attr('memberdetailid', response.memberDetailId)
                                    $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ulFamilyMember li.active").find("a").css("color", "green");
                                }
                                //End//26/01/2016//Ahmad Raza//setting Id's returned in response
                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
                //End//21/01/2016//Muhammad Irfan//Logic implemented to check privileges
            }
        }
        else {
            utility.DisplayMessages("Please enter any value", 3);
        }
        //End//29/01/2016//Ahmad Raza//logic to check if data exists in detail section
    },

    SaveFavToggelStatus: function (FavListVal) {
        var isFavListOpened = $('#' + Clinical_FamilyHx.params.PanelID + " #favSectionDiv").hasClass("toggledHor");
        $.when(EMRUtility.insertUpdateFavListStatus(Clinical_FamilyHx.FavListName, isFavListOpened)).then(function () {
            EMRUtility.insertUpdateFavListVal(Clinical_FamilyHx.FavListName, FavListVal);
        });
    },

    BindCurrentFamilySoapText: function (resopnse) {


        var $row = $('<tr/>');
        $row.append('<td style="display:none;">' + resopnse.FamilyHxId + '</td><td>' + resopnse.IsCreatedOrModified + '</td><td>' + resopnse.SoapText + '</td><td>' + resopnse.LastUpdated + '</td>');
        $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvFamilyHx tbody").html($row);
        if ($('#' + Clinical_FamilyHx.params.PanelID + ' #pnlFamilyHx_Result #divSwitch #switchVisit').attr('status') == '1') {
            $('#' + Clinical_FamilyHx.params.PanelID + ' #pnlCurrent').removeClass('hidden');
            $('#' + Clinical_FamilyHx.params.PanelID + ' #pnlPast').addClass('hidden');
        }
    },
    //Author: Muhammad Irfan
    //Date: 20/01/2016
    //This function will handle load of FamilyHx and it's childs as specified by FamilyHxType
    //It represents service call to API
    fillFamilyHx: function (familyHxType, familyHxId) {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["FamilyHxId"] = familyHxId != null ? familyHxId : 0;
        objData["commandType"] = "FILL_FamilyHx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");
    },

    //Author: Muhammad Irfan
    //Date: 20/01/2016
    //This function will handle Add of FamilyHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous)
    //It represents service call to API
    saveFamilyHx: function (familyHxData) {
        var objData = JSON.parse(familyHxData);
        if (Clinical_FamilyHx.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_FamilyHx.params.patientID;
        }
        objData["commandType"] = "SAVE_FamilyHx";

        //Start//26/01/2016//Ahmad Raza//setting checkbox values
        if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #RadYesRelativeDied').is(':checked')) {
            objData["RadRelativeDied"] = "True";
        }
        else if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #RadNoRelativeDied').is(':checked')) {
            objData["RadRelativeDied"] = "False";
        }
        else {
            objData["RadRelativeDied"] = "";
        }
        //End//26/01/2016//Ahmad Raza//setting checkbox values

        //Start//27/01/2016//Ahmad Raza//setting text values for soap text
        objData["DiseaseText"] = $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease li.active").text();
        objData["FamilyMemberText"] = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").text();
        objData["HealthStatusText"] = ($("#" + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ddlHealthStatus option:selected").text() == "- Select -" ? "" : $("#" + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ddlHealthStatus option:selected").text());
        //End//27/01/2016//Ahmad Raza//setting text values for soap text
        objData["FamilyMemberId"] = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");
    },

    //Author: Muhammad Irfan
    //Date: 20/01/2016
    //This function will handle Edit of FamilyHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous)
    //It represents service call to API
    updateFamilyHx: function (familyHxData, familyHxId) {

        var objData = JSON.parse(familyHxData);
        if (Clinical_FamilyHx.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_FamilyHx.params.patientID;
        }
        objData["commandType"] = "UPDATE_FamilyHx";

        //Start//26/01/2016//Ahmad Raza//setting checkbox values
        if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #RadYesRelativeDied').is(':checked')) {
            objData["RadRelativeDied"] = "True";
        }
        else if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #RadNoRelativeDied').is(':checked')) {
            objData["RadRelativeDied"] = "False";
        }
        else {
            objData["RadRelativeDied"] = "";
        }
        //End//26/01/2016//Ahmad Raza//setting checkbox values

        //Start//26/01/2016//Ahmad Raza//setting text values for soap text
        objData["DiseaseText"] = $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease li.active").text();
        objData["FamilyMemberText"] = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").text();
        objData["HealthStatusText"] = ($("#" + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ddlHealthStatus option:selected").text() == "- Select -" ? "" : $("#" + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #ddlHealthStatus option:selected").text());
        //End//26/01/2016//Ahmad Raza//setting text values for soap text

        objData["FamilyMemberId"] = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
        var data = JSON.stringify(objData);

        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");

    },

    //Author: Muhammad Irfan
    //Date: 20/01/2016
    //This function will handle Unload of FamilyHx Tab

    unLoadTab: function (nextOrPre, controlToInvoke) {

        Clinical_FamilyHx.bIsFirstLoad = true;
        //Start//11-02-2016//Ahmad Raza//EMR Bug#298 resolved
        var familyHxType = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfFamilyHxType").val();
        var detailExists = Clinical_FamilyHx.isDetailExists(familyHxType.toLowerCase());
        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
            Clinical_FamilyHx.controlToInvoke = controlToInvoke;

            var diseaseAdded = $('#' + Clinical_FamilyHx.params.PanelID + " #ulFamilyHxDisease > li").filter(function () {
                return $(this).attr("id") < 0;
            });

            if (EMRUtility.compareFormDataWithSerialized(Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx') || diseaseAdded.length > 0) {

                utility.myConfirmNote('1', function () {
                    var familyHxType = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfFamilyHxType").val();
                    if (familyHxType == "" || familyHxType == "undefined") {
                        Clinical_FamilyHx.unloadFamilyHistory(nextOrPre);
                        if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalFamilyHx") {
                            UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                            Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabFamilyHx', 'FamilyHx');
                        }
                    }
                    else {
                        Clinical_FamilyHx.bNextPrev = true;
                        Clinical_FamilyHx.addFamilyHxToNotes();
                    }
                }, function () {
                    var familyHxType = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfFamilyHxType").val();
                    if (familyHxType == "" || familyHxType == "undefined") {
                        Clinical_FamilyHx.unloadFamilyHistory(nextOrPre);
                        if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalFamilyHx") {
                            UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                            Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabFamilyHx', 'FamilyHx');
                        }
                    }
                    else {
                        Clinical_FamilyHx.bNextPrev = true;
                        Clinical_FamilyHx.familyHxSave(familyHxType, true, false);
                        Clinical_FamilyHx.unloadFamilyHistory(nextOrPre);
                    }
                }, function () {
                    Clinical_FamilyHx.unloadFamilyHistory(nextOrPre);
                    if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalFamilyHx") {
                        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                        Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabFamilyHx', 'FamilyHx');
                    }
                },
      ''
      );


                // utility.myConfirm('19', function () {
                //     var familyHxType = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfFamilyHxType").val();
                //     if (familyHxType == "" || familyHxType == "undefined") {
                //         Clinical_FamilyHx.unloadFamilyHistory(nextOrPre);
                //         if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalFamilyHx") {
                //             UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                //             Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabFamilyHx', 'FamilyHx');
                //         }
                //     }
                //     else {
                //         Clinical_FamilyHx.bNextPrev = true;
                //         Clinical_FamilyHx.familyHxSave(familyHxType, true);
                //     }
                // }, function () {
                //     Clinical_FamilyHx.unloadFamilyHistory(nextOrPre);
                //     if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalFamilyHx") {
                //         UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                //         Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabFamilyHx', 'FamilyHx');
                //     }
                // },
                //'1'
                //);
            }
            else {
                Clinical_FamilyHx.unloadFamilyHistory();
                if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalFamilyHx") {
                    UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                    Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabFamilyHx', 'FamilyHx');
                }
            }
        } else {
            Clinical_FamilyHx.unloadFamilyHistory();
        }
    },
    //End//11-02-2016//Ahmad Raza//EMR Bug#298 resolved

    //Author: Muhammad Irfan
    //Date: 12-28-2015
    //This function will unload FamilyHx Form
    unloadFamilyHistory: function (nextOrPre) {
        if (Clinical_FamilyHx.params["FromAdmin"] == "0") {
            if (Clinical_FamilyHx.params != null && Clinical_FamilyHx.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_FamilyHx.params.ParentCtrl, 'Clinical_FamilyHx');
                if (Clinical_FamilyHx.controlToInvoke != null) {
                    setTimeout(function () {
                        Clinical_ProgressNote.SelectNotesComponentTab(Clinical_FamilyHx.controlToInvoke);
                        Clinical_FamilyHx.controlToInvoke = null;
                    }, 400);
                }
            }
            else {
                UnloadActionPan(null, 'Clinical_FamilyHx');
                if (Clinical_FamilyHx.controlToInvoke != null)
                    setTimeout(function () {
                        Clinical_ProgressNote.SelectNotesComponentTab(Clinical_FamilyHx.controlToInvoke);
                        Clinical_FamilyHx.controlToInvoke = null;
                    }, 400);
            }
        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_FamilyHx").remove();
            RemoveAdminTab();
        }
        EMRUtility.scrollToPNcomponent('clinical_familyhx');
    },

    //-----------------Progress Note-------------
    // Reason: These functions are used for Progress Note Soap Attachment, creation and detachment


    //Author: Ahmad Raza
    //Date: 02-02-2016
    //Call Back function to add component to Progress Note
    addFamilyHxToNotes: function () {
        var familyHxId = Clinical_FamilyHx.params.FamilyHxId;
        var familyHxType = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfFamilyHxType").val();
        Clinical_FamilyHx.familyHxSave(familyHxType, true);
    },

    //Author: Ahmad Raza
    //Date: 02-02-2016
    //this function will get Family History Soap Text and attach that to Progress note
    getFamilyHxInfo: function (familyHxType, unloadFamilyhx, familyHxId, hideAlertMessage) {
        Clinical_FamilyHx.fillFamilyHx(familyHxType, familyHxId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_FamilyHx.createFamilyHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', unloadFamilyhx, hideAlertMessage);

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //Author: Ahmad Raza
    //Date: 02-02-2016
    //This Function will check, if Family History Soap is already attached in Progress note, if Family History is not attached than it will create main divs to attach allergy
    checkFamilyHxExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Familyhx').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="FamilyHxComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_Familyhx title="Family Hx"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'FamilyHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="FamilyHx">Family Hx</a> ' +
                       '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'FamilyHx\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'FamilyHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_Familyhx> </header></li>');
           
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Familyhx').parent().parent().removeClass('hidden');
         Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    createFamilyHxBodyHTMLFromNotes: function (FamilyHistory, noteHTMLCtrl, unloadFamilyhx, hideAlertMessage) {
        Clinical_FamilyHx.checkFamilyHxExists();
        if (FamilyHistory && FamilyHistory.FamilyHxId && FamilyHistory.FamilyHxId > 0) {
            var familyHxFillObj = FamilyHistory;
            //  $.each(familyHxFillObj, function (i,item) {
            var $mainDivFamilyHx = $(document.createElement('div'));

            var familyHxId = familyHxFillObj.FamilyHxId;
            if (familyHxId != null && familyHxId > 0) {
                var SoapTextHtml = familyHxFillObj.SoapText
                SoapTextHtml = SoapTextHtml.replace(/&quot;/g, '"');
                SoapTextHtml = SoapTextHtml.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                SoapTextHtml = SoapTextHtml.replace(/&nbsp;/g, '');
                var $sectionBodyFamilyHx = $(document.createElement('section'));
                $sectionBodyFamilyHx.attr('id', "Cli_FamilyHx_Main" + familyHxId);
                var $detailsDiv = $(document.createElement('div'));
                $detailsDiv.attr('id', "Cli_FamilyHx_" + familyHxId);
                var $listFamilyHx = $(document.createElement('ul'));

                $listFamilyHx.attr('class', 'list-unstyled')

                $sectionBodyFamilyHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_FamilyHx_" + familyHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_FamilyHx_Main" + familyHxId + '"  ><i class="fa fa-times"></i></a></div> ');

                $listFamilyHx.append("<li>" + SoapTextHtml + "</li>");
                $detailsDiv.append($listFamilyHx);
                $sectionBodyFamilyHx.append($detailsDiv);

                if ($(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId).length == 0) {
                    $mainDivFamilyHx.append($sectionBodyFamilyHx);
                    //Clinical_FamilyHx.updateFamilyHxHtml($mainDivFamilyHx.html(), familyHxId, noteHTMLCtrl, hideAlertMessage);
                    var familyHxHtml = $mainDivFamilyHx.html();
                    if (familyHxHtml != '') {
                        $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().addClass('initialVisitBody');

                        $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().append(familyHxHtml);
                    }
                    return familyHxId;
                } else {

                    var commentHTML = "";
                    var commentsID = $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId + ' ul li:Last').attr('id');
                    if (commentsID != null && commentsID.indexOf("Comments") >= 0) {
                        commentHTML = $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId).html($sectionBodyFamilyHx.html());
                    $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId + ' ul').append(commentHTML);


                }

            }
        }
    },
    //Author: Ahmad Raza
    //Date: 02-02-2016
    //This Function is used to create SOAP html and append it to  Progress note
    createFamilyHxBodyHTML: function (response, noteHTMLCtrl, unloadFamilyhx, hideAlertMessage) {
        Clinical_FamilyHx.checkFamilyHxExists();
        if (response.FamilyHxFill_JSON != '' && response.FamilyHxFill_JSON != null) {
            var familyHxFillObj = JSON.parse(response.FamilyHxFill_JSON);
            //  $.each(familyHxFillObj, function (i,item) {
            var $mainDivFamilyHx = $(document.createElement('div'));

            var familyHxId = familyHxFillObj.FamilyHxId;
            if (familyHxId != null && familyHxId > 0) {
                var SoapTextHtml = familyHxFillObj.SoapText
                SoapTextHtml = SoapTextHtml.replace(/&quot;/g, '"');
                SoapTextHtml = SoapTextHtml.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                SoapTextHtml = SoapTextHtml.replace(/&nbsp;/g, '');
                var $sectionBodyFamilyHx = $(document.createElement('section'));
                $sectionBodyFamilyHx.attr('id', "Cli_FamilyHx_Main" + familyHxId);
                var $detailsDiv = $(document.createElement('div'));
                $detailsDiv.attr('id', "Cli_FamilyHx_" + familyHxId);
                var $listFamilyHx = $(document.createElement('ul'));

                $listFamilyHx.attr('class', 'list-unstyled')

                $sectionBodyFamilyHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_FamilyHx_" + familyHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_FamilyHx_Main" + familyHxId + '"  ><i class="fa fa-times"></i></a></div> ');

                $listFamilyHx.append("<li>" + SoapTextHtml + "</li>");
                $detailsDiv.append($listFamilyHx);
                $sectionBodyFamilyHx.append($detailsDiv);

                if ($(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId).length == 0) {
                    $mainDivFamilyHx.append($sectionBodyFamilyHx);
                    Clinical_FamilyHx.updateFamilyHxHtml($mainDivFamilyHx.html(), familyHxId, noteHTMLCtrl, hideAlertMessage);
                } else {

                    var commentHTML = "";
                    var commentsID = $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId + ' ul li:Last').attr('id');
                    if (commentsID != null && commentsID.indexOf("Comments") >= 0) {
                        commentHTML = $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId).html($sectionBodyFamilyHx.html());
                    $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId + ' ul').append(commentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText("FamilyHx", hideAlertMessage);
                    Clinical_FamilyHx.updateFamilyHxHtml("", familyHxId, noteHTMLCtrl, hideAlertMessage);
                }
                //});
                if (unloadFamilyhx == true) {
                    Clinical_FamilyHx.unloadFamilyHistory(Clinical_FamilyHx.bNextPrev);
                }
            }
        }
    },

    createFamilyHxBodyHTMLFromNote: function (response, noteHTMLCtrl, unloadFamilyhx, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_FamilyHx.checkFamilyHxExists();
        if (response) {
            var familyHxFillObj = response;
            //  $.each(familyHxFillObj, function (i,item) {
            var $mainDivFamilyHx = $(document.createElement('div'));

            var familyHxId = familyHxFillObj.FamilyHxId;
            if (familyHxId != null && familyHxId > 0) {

                var $sectionBodyFamilyHx = $(document.createElement('section'));
                $sectionBodyFamilyHx.attr('id', "Cli_FamilyHx_Main" + familyHxId);
                var $detailsDiv = $(document.createElement('div'));
                $detailsDiv.attr('id', "Cli_FamilyHx_" + familyHxId);
                var $listFamilyHx = $(document.createElement('ul'));

                $listFamilyHx.attr('class', 'list-unstyled');

                $sectionBodyFamilyHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_FamilyHx_" + familyHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_FamilyHx_Main" + familyHxId + '"  ><i class="fa fa-times"></i></a></div> ');

                $listFamilyHx.append("<li>" + familyHxFillObj.FamilyHxSoapText + "</li>");
                $detailsDiv.append($listFamilyHx);
                $sectionBodyFamilyHx.append($detailsDiv);

                if ($(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId).length == 0) {
                    $mainDivFamilyHx.append($sectionBodyFamilyHx);
                    var familyHxHtml = $mainDivFamilyHx.html();
                    if ($mainDivFamilyHx != '') {
                        $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().addClass('initialVisitBody');
                        $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().append(familyHxHtml);
                    }

                } else {

                    var commentHTML = "";
                    var commentsID = $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId + ' ul li:Last').attr('id');
                    if (commentsID != null && commentsID.indexOf("Comments") >= 0) {
                        commentHTML = $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId).html($sectionBodyFamilyHx.html());
                    $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().find('#Cli_FamilyHx_Main' + familyHxId + ' ul').append(commentHTML);

                }

            }
        }
        dfd.resolve();
        return dfd;
    },

    //Author: Ahmad Raza
    //Date: 11-02-2016
    //This Function is called by Progress Notes (Fill FamilyHx Func, CopyAllNotesCategories)
    updateFamilyHxHtml: function (familyHxHtml, familyHxId, noteHTMLCtrl, hideAlertMessage) {
        $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().addClass('initialVisitBody');
        if (familyHxHtml != '') {
            $(noteHTMLCtrl + ' clinical_Familyhx').parent().parent().append(familyHxHtml);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (familyHxHtml != '') {
            Clinical_FamilyHx.attachFamilyHxFromNotes(familyHxId, hideAlertMessage);
        }

    },

    //Author: Ahmad Raza
    //Date: 11-02-2016
    //This Function detach Family History From progress note
    detach_ComponentsFamilyHx: function (componentName, isUpdate, familyHxComponentRemove) {
        var clinicalFamilyHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Familyhx').parent().parent().find('section[id*="Cli_FamilyHx_Main"]').map(function () {
            return this.id.replace("Cli_FamilyHx_Main", "");
        }).get().join(',');

        if (familyHxComponentRemove) {

            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Familyhx').parent().parent().attr('NoteComponentId');
            //Start//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Family Hx']").remove();
            //End//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            if (Clinical_ProgressNote.params["TemplateName"])
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Familyhx').parent().parent().addClass('hidden');
            else
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Familyhx').parent().parent().remove();

            var hxComponents = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .HxComponent').length;

            if (NoteComponentId && NoteComponentId != "NCDummyId" && hxComponents == 0) {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Familyhx').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('FamilyHx', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Familyhx').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                Clinical_ProgressNote.ShowHideComponetsHeaders();
            }
        }
        else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Familyhx').parent().parent().find('section[id*="Cli_FamilyHx_Main"]').remove();
        }

        if (clinicalFamilyHxIds == "" || clinicalFamilyHxIds == "undefined") {
            //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
            Clinical_ProgressNote.saveComponentSOAPText("FamilyHx", true);
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            Clinical_FamilyHx.detachFamilyHxFromNotes_DBCall(clinicalFamilyHxIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (isUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText("FamilyHx", true);
                        //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
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

    //Author: Ahmad Raza
    //Date: 11-02-2016
    //This Functions ask for Detaching Family Hx from Progress Note for current Patient Selected
    detachFamilyHxFromNotes: function (familyHxId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_familyhx');
                var selectedValue = familyHxId.replace('Cli_FamilyHx_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_FamilyHx.detachFamilyHxFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + familyHxId).remove();
                            Clinical_ProgressNote.Add_NoText();
                            Clinical_ProgressNote.saveComponentSOAPText("FamilyHx", true);

                            //Clinical_ProgressNote.updateProgressNoteHTML();
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

    //Author: Ahmad Raza
    //Date: 11-02-2016
    //This Functions attached Family Hx to Progress Note for current Patient Selected
    attachFamilyHxFromNotes: function (familyHxId, hideAlertMessage) {
        if (familyHxId == "" || familyHxId == "undefined") {
        }
        else {
            Clinical_FamilyHx.attachFamilyHxFromNotes_DBCall(familyHxId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    //If Attached FamilyHx Made new inseration to FamilyHx Table than good ids should be attached to HTML
                    Clinical_ProgressNote.saveComponentSOAPText("FamilyHx", hideAlertMessage);
                    $('#' + familyHxId).remove();
                    // utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //Author: Ahmad Raza
    //Date: 11-02-2016
    //If FamilyHx Component which is dropeed in Progress note has no FamilyHx attached, than it will call for Latest FamilyHx for this patient
    getLatestFamilyHxByPatientId: function (hideAlertMessage, droppedComponent) {

        Clinical_FamilyHx.getLatestClinical_FamilyHxByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_FamilyHx.createFamilyHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    getAutoPopulateSetting_DBCall: function () {

        var objData = new Object();
        objData["UserId"] = globalAppdata["AppUserId"];
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["ComponentType"] = "FamilyHistory";
        objData["commandType"] = "getautopopulatesetting";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");

    },
    //-----Server calls of Notes----------
    //Author: Ahmad Raza
    //Date: 11-02-2016
    //DB Call to detach family hx from notes
    detachFamilyHxFromNotes_DBCall: function (familyHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["FamilyHxId"] = familyHxId;
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
        objData["commandType"] = "detach_Familyhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");
    },

    //Author: Ahmad Raza
    //Date: 11-02-2016
    //DB Call to attach family hx with notes
    attachFamilyHxFromNotes_DBCall: function (familyHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["FamilyHxId"] = familyHxId;
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
        objData["commandType"] = "attach_familyhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");
    },

    /*
    Author: Farooq Ahmad
    Date: 02/02/2016
    Overview: This function will show cross button on hover li
    */
    openSearchPopup: function (searchType, ctrl, hiddenCtrl) {
        var controlToLoad = "";
        if (searchType == "ICD") {
            controlToLoad = "Admin_IMOICD";
        }
        else if (searchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (searchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }
        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #txtDisease").attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";
        //params["Parentctrl"] = Clinical_MedicalHx.params["TabID"];
        if (Clinical_FamilyHx.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Clinical_FamilyHx';
        }
        else
            params["ParentCtrl"] = Clinical_FamilyHx.params["TabID"];

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Clinical_FamilyHx.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },
    //End///Farooq changes

    //Author: Ahmad Raza
    //Date: 11-02-2016
    //DB Call to get latest familyhx soap text
    getLatestClinical_FamilyHxByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_familyhxby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");
    },

    /* Start Functions by Muhammad Irfan */

    //Author: Muhammad Irfan
    //Date: 25-01-2016
    //Filling diseases of FamilyHx
    fillFamilyHxDisease: function (obj, ev, diseaseId) {

        //  ev.stopPropagation();
        if (obj != "") {
            Clinical_FamilyHx.previousSelectedDisease["diseaseId"] = $(obj).attr('id');
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfPreviousSelectedDisease").val($(obj).attr('id'));
        }
        else {
            Clinical_FamilyHx.previousSelectedDisease["diseaseId"] = diseaseId;
            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfPreviousSelectedDisease").val(diseaseId);
        }

        var isExists = false;
        for (var index in Clinical_FamilyHx.currentSelectedDisease) {
            if (obj != "" && Clinical_FamilyHx.currentSelectedDisease[index].diseaseId == $(obj).attr('id')) {
                isExists = true;
                if (Clinical_FamilyHx.currentSelectedDisease[index].Members != null) {
                    var memberId = Clinical_FamilyHx.currentSelectedDisease[index].Members[0].memberId;
                }
                // $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx ul#ulFamilyMember li#" + memberId).trigger('click');
            }
            else if (diseaseId != "" && diseaseId && Clinical_FamilyHx.currentSelectedDisease[index].diseaseId == diseaseId) {
                isExists = true;
                if (Clinical_FamilyHx.currentSelectedDisease[index].Members != null) {
                    var memberId = Clinical_FamilyHx.currentSelectedDisease[index].Members[0].memberId;
                }
            }
        }
        if (!isExists) {
            Clinical_FamilyHx.currentSelectedDisease.push({
                diseaseId: obj != "" ? $(obj).attr('id') : diseaseId

            });
        }

        //Start//28/01/2016//Ahmad Raza//when no disease is selected, disabling member and member detail section
        var memberDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#memberList');
        //var memberDetailDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails');
        // memberDiv.removeClass("disableAll");
        var previousSelectedMemberId = Clinical_FamilyHx.currentSelectedMember["memberId"];
        // $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx ul#ulFamilyMember li#" + previousSelectedMemberId).trigger('click');
        //memberDetailDiv.removeClass("disableAll");
        //End//28/01/2016//Ahmad Raza//when no disease is selected, disabling member and member detail section
        var diseaseId = obj != "" ? $(obj).attr('id') : diseaseId;
        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfDiseaseId").val(diseaseId);
        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #btnFamilyHxSave').hide();
        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #memberList").removeClass('disableAll');

        $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyHxDisease li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == diseaseId) {
                if ($(this).hasClass("active") == false) {
                    $(this).addClass("active");
                    $(this).find('div').css('display', '');
                }
            }
            else {
                $(this).removeClass("active");
                $(this).find('div').css('display', 'none');
            }
        });

        $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx').data('serialize', $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx').serialize());

        //  Clinical_FamilyHx.fillMembersDropDown(diseaseId);
    },

    /*
    Author: MuhammadIrfan
    Date: 12/01/2016
    Overview: This function will show cross button on hover li
    */
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },
    /*
    Author: MuhammadIrfan
    Date: 12/01/2016
    Overview: This function will hide cross button on hover li
    */
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },
    /*
    Author: MuhammadIrfan
    Date: 12/01/2016
    Overview: This function delete the selected li from disease
    */
    deleteFamilyHxDisease: function (obj, ev) {

        ev.stopPropagation();
        var diseaseId = $(obj).attr('id');
        var familyMemberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
        //var isExists = false;
        //for (var index in Clinical_FamilyHx.currentSelectedDisease) {
        //    if (Clinical_FamilyHx.currentSelectedDisease[index].diseaseId == $(obj).attr('id')) {
        //        isExists = true;

        //        if (Clinical_FamilyHx.currentSelectedDisease[index].Members != null && Clinical_FamilyHx.currentSelectedDisease[index].Members[0] != null) {
        //            var memberId = Clinical_FamilyHx.currentSelectedDisease[index].Members[0].memberId;
        //            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx ul#ulFamilyMember li#" + memberId).trigger('click');
        //        }

        //    }
        //}
        //if (!isExists) {
        //    Clinical_FamilyHx.currentSelectedDisease.push({
        //        diseaseId: $(obj).attr('id')

        //    });
        //}

        AppPrivileges.GetFormPrivileges("History_Family Hx", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('23', function () {
                    var selectedValue = diseaseId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    if (diseaseId < 0) {
                        //Start 01-07-2016 Humaira Yousaf
                        Clinical_FamilyHx.enableFavoriteList($(obj));
                        //Start 01-07-2016 Humaira Yousaf

                        $(obj).remove();
                        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            Clinical_FamilyHx.deleteCacheDisease(diseaseId, familyMemberId);
                        }
                        if ($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyDisease #ulFamilyHxDisease li").length <= 0) {
                            var memberDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#memberList');
                            //var memberDetailDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails');
                            //memberDiv.addClass("disableAll");
                            //memberDetailDiv.removeClass("disableAll");

                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails').removeClass("disableAll");
                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').prop('disabled', true);
                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').css("background", "#EEEEEE");
                            $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').css("border", "1px solid #cccccc");
                            $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails #txtFamilyMemberComments").prop('disabled', true);

                            utility.DisplayMessages("Successfully Deleted", 1);
                            Clinical_FamilyHx.loadFamilyHxComponent();
                        }

                    }
                    else {
                        Clinical_FamilyHx.familyHxDiseaseDelete(selectedValue, familyMemberId).done(function (response) {
                            var memberDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#memberList');
                            //var memberDetailDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails');
                            response = JSON.parse(response);
                            if (response.status != false) {
                                //Start 01-07-2016 Humaira Yousaf
                                Clinical_FamilyHx.enableFavoriteList($(obj));
                                //Start 01-07-2016 Humaira Yousaf
                                $(obj).remove();
                                //Start//28/01/2016//Ahmad Raza//when disease is deleted and no disease is selected, disabling member and member detail section
                                //memberDiv.addClass("disableAll");
                                //memberDetailDiv.addClass("disableAll");
                                if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    Clinical_FamilyHx.deleteCacheDisease(diseaseId, familyMemberId);
                                }
                                $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails').removeClass("disableAll");
                                $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').prop('disabled', true);
                                $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').css("background", "#EEEEEE");
                                $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails #txtAgeAtDiagnosis').css("border", "1px solid #cccccc");
                                $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails #txtFamilyMemberComments").prop('disabled', true);
                                //End//28/01/2016//Ahmad Raza//when disease is deleted and no disease is selected, disabling member and member detail section
                                utility.DisplayMessages(response.Message, 1);

                                Clinical_FamilyHx.loadFamilyHxComponent();
                                //Clinical_FamilyHx.loadFamilyHxStatuses('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyDisease #ulFamilyMember", "GetFamilyHxFamilyMember", true);

                                Clinical_FamilyHx.refereshFamilyHxGridSoapText();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '23'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        // }
    },

    deleteCacheDisease: function (diseaseId, memberId) {

        var diseaseIndex = -1;

        if (Clinical_HistorySummary.HistoryCacheList.FamilyHx != null) {
            var familymember = $.grep(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease, function (item, index) {
                if (item.FamilyMemberId == memberId && item.DiseaseId == diseaseId) {
                    diseaseIndex = index;
                }
            });

            if (diseaseIndex != -1) {
                if (Clinical_HistorySummary.HistoryCacheList.FamilyHx != null && Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail != null) {
                    var diseaseDetailIndex = -1;
                    var familydisease = $.grep(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail, function (item, index) {
                        if (item.MemberId == memberId && item.DiseaseId == diseaseId) {
                            diseaseDetailIndex = index;
                        }
                    });

                    if (diseaseDetailIndex != -1) {
                        Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail.splice(diseaseDetailIndex, 1);
                    }
                }
                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease.splice(diseaseIndex, 1);
            }
        }

    },

    //Abid Ali// For refereshing FamilyHx SoapText Grid
    refereshFamilyHxGridSoapText: function () {

        Clinical_FamilyHx.fillFamilyHx().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    var diseaseLoadDetails = JSON.parse(response.DiseaseLoad_JSON);
                    var familyHxDetail = JSON.parse(response.FamilyHxFill_JSON);

                    if (familyHxDetail.FamilyHxId > 0) {

                        Clinical_FamilyHx.BindSoapTextGrid(familyHxDetail, diseaseLoadDetails.length > 0);
                    }
                    else {
                        var $row = $('<tr/>');
                        $row.append('<td>&nbsp;</td><td>No Known Family History</td><td></td>');
                        $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvFamilyHx tbody").html($row);
                        $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #divSwitch").addClass('disableAll');
                    }
                }
            }
        });
    },

    //Author: Ahmad Raza
    //Date: 26-01-2016
    //method to delete member details
    deleteFamilyHxMemberDetail: function (obj, ev) {

        ev.stopPropagation();
        var memberId = $(obj).attr('memberdetailid');
        if (memberId < 0) {
            utility.DisplayMessages("This member has no detail to delete!", 3);
            $(obj).attr('memberdetailid', -1);
            $(obj).find("a").css("color", "gray");
        } else {
            AppPrivileges.GetFormPrivileges("History_Family Hx", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('25', function () {
                        var selectedValue = memberId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            var diseaseId = $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease li.active").attr('id');
                            Clinical_FamilyHx.familyHxMemberDetailDelete(selectedValue, diseaseId).done(function (response) {
                                var memberDetailDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#FamilyMemberDetails');
                                var memberDiv = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyDisease div#memberList');
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $(obj).remove();
                                    //Start//28/01/2016//Ahmad Raza//when memberdetail is deleted and no member is selected, disabling member and member detail section
                                    memberDetailDiv.addClass("disableAll");
                                    memberDiv.addClass("disableAll");
                                    //End//28/01/2016//Ahmad Raza//when memberdetail is deleted and no member is selected, disabling member and member detail section
                                    utility.DisplayMessages(response.Message, 1);
                                    Clinical_FamilyHx.loadFamilyHxStatuses('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyDisease #ulFamilyMember", "GetFamilyHxFamilyMember", true);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { },
                        '25'
                    );
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    //Author: Ahmad Raza
    //Date: 26-01-2016
    //method to delete disease
    familyHxDiseaseDelete: function (diseaseId, familyMemberId) {
        var objData = new Object();
        objData["DiseaseId"] = diseaseId;
        var familyHxId = $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val();
        objData["FamilyHxId"] = familyHxId;
        objData["FamilyMemberId"] = familyMemberId;
        objData["PatientId"] = Clinical_FamilyHx.params.patientID || $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "DELETE_FAMILYHXDISEASE";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");
    },

    //Author: Ahmad Raza
    //Date: 26-01-2016
    //method to delete member details
    familyHxMemberDetailDelete: function (memberId, diseaseId) {
        var objData = new Object();
        objData["MemberDetailId"] = memberId;
        objData["DiseaseId"] = diseaseId;
        var familyHxId = $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val();
        objData["FamilyHxId"] = familyHxId;
        objData["PatientId"] = $("div#PatientProfile #hfPatientId").val();
        objData["commandType"] = "DELETE_FAMILYMEMBERDETAIL";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");
    },
    /*
    Author: MuhammadIrfan
    Date: 12/01/2016
    Overview: This function checks if member has data
    */
    fillMembersDropDown: function (familyHxId, memberHasDisease) {
        // Clinical_FamilyHx.loadFamilyHxStatuses('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyDisease #ulFamilyMember", "GetFamilyHxFamilyMember", true).done(function () {
        if (familyHxId > 0) {


            $.each(memberHasDisease, function (i, item) {

                $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyMember li").each(function (j, memberItem) {
                    if ($(this).attr("id") != null && $(this).attr("id") == item.MemberId && item.HasDisease == "1") {
                        $(this).find("a").css("color", "green");
                        return;
                        //$(this).attr('memberdetailid', item.MemberDetailId);

                    }
                    else {

                    }
                });
            });

            //Clinical_FamilyHx.fillMembersDropDownDB_Call(familyHxId).done(function (response) {

            //        if (response != "") {
            //            response = JSON.parse(response);

            //            if (response.status != false) {

            //                var memberLoadData = JSON.parse(response.MemberLoad_JSON);

            //                $.each(memberLoadData, function (i, item) {


            //                    $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyMember li").each(function (j, memberItem) {
            //                        if ($(this).attr("id") != null && $(this).attr("id") == item.MemberId) {

            //                            $(this).find("a").css("color", "green");
            //                            $(this).attr('memberdetailid', item.MemberDetailId);

            //                        }
            //                        else {

            //                        }
            //                    });
            //                });

            //            } else {

            //            }
            //            if (Clinical_FamilyHx.currentSelectedDisease["MemberId"] != null) {
            //                var previouslySelectedMember = Clinical_FamilyHx.currentSelectedDisease["MemberId"];
            //                $(" #frmClinicalFamilyHx #ulFamilyMember li#" + previouslySelectedMember).trigger('click');
            //            }

            //        }


            //    });
        }
        else {

            // Clinical_FamilyHx.loadFamilyHxStatuses('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx div#FamilyDisease #ulFamilyMember", "GetFamilyHxFamilyMember", true);
        }

        //    });

        if (Clinical_HistorySummary.HistoryCacheList.FamilyHx != null && Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail.length > 0) {
            $.each(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail, function (i, item) {
                $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyMember li").each(function (j, memberItem) {
                    if ($(this).attr("id") != null && $(this).attr("id") == item.FamilyMemberId) {
                        if (!$(this).find("a").hasClass('green')) {
                            $(this).find("a").css("color", "green");
                        }
                        return;
                    }
                    else {

                    }
                });
            });
        }

    },
    /*
    Author: MuhammadIrfan
    Date: 12/01/2016
    Overview: This function call api service
    */
    fillMembersDropDownDB_Call: function (familyHxId) {
        var objData = {};
        //objData["DiseaseId"] = diseaseID;
        objData["FamilyHxId"] = familyHxId;
        objData["commandType"] = "fill_members";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");
    },
    /*
    Author: MuhammadIrfan
    Date: 12/01/2016
    Overview: This  function call api service to fill member detail
    */
    fillMemberDetailDB_Call: function (memberId, diseaseId) {
        var objData = {};
        //objData["MemberDetailId"] = currentMemberDetailId;
        objData["MemberId"] = memberId;
        objData["DiseaseId"] = diseaseId;
        objData["FamilyHxId"] = Clinical_FamilyHx.params.HxTypeId;
        objData["commandType"] = "fill_members_detail";
        objData["PatientId"] = Clinical_FamilyHx.params.patientID
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");
    },

    /* End Functions by Muhammad Irfan */


    fillMemberDiseaseDB_Call: function (familyHxId, familyMemberHxId) {
        var objData = {};
        objData["FamilyMemberId"] = familyMemberHxId;
        objData["FamilyHxId"] = Clinical_FamilyHx.params.HxTypeId;
        objData["commandType"] = "fill_members_disease";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");
    },


    //Function Name: isFavoriteHistoryFound
    //Author Name: Humaira Yousaf
    //Created Date: 01-07-2016
    //Description: Checks if Favorite History is found
    isFavoriteHistoryFound: function (favICDCode, favCPTDesc) {

        var isFound = false;
        $("#" + Clinical_FamilyHx.params.PanelID + " #ulFamilyHxDisease li").each(function (index, item) {
            if ($(item).attr('icd9desc') != null) {
                var currentRowCPTCode = $(item).text() != null ? $(item).attr('icd9desc') + '-' + $(item).attr('snomedcode') : "";
                if (currentRowCPTCode.trim() == favICDCode.trim()) {
                    isFound = true;
                }
            }
            //else {
            //    var currentRowCPTCode = $(item).find("input[id*='ProcedureProcedure']").val() != null ? $(item).find("input[id*='ProcedureProcedure']").val() : "";
            //    if (currentRowCPTCode == favCPTCode) {
            //        isFound = true;
            //    }
            //}
        });

        return isFound;
    },


    gridLoad: function (response) {
        var isactive = $('#' + Clinical_FamilyHx.params.PanelID + ' #pnlFamilyHx_Result #divSwitch #switchActive').attr('isactive');

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvPastFamilyHx")) {
            $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvPastFamilyHx").dataTable().fnClearTable();
            $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvPastFamilyHx").dataTable().fnDestroy();
            $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvPastFamilyHx tbody").find("tr").remove();
        }
        var logCount = JSON.parse(response);
        if (logCount.HxLogSoapCount > 0) {
            var LoadJSONData = JSON.parse(logCount.HxLogSoap_JSON); //Parsing array to JSON
            var counter = null;
            for (var i = 0; i < LoadJSONData.length; i++) {
                // $.each(LoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                // $row.attr("onclick", "Clinical_FamilyHx.CDSEdit('" + item.CDSId + "',event);");
                //$row.attr("id", "gvCDS_row" + item.CDSId);
                var text = LoadJSONData[i].SoapText;

                counter = i;
                $row.append('<td style="display:none;">' + counter + '</td><td>' + LoadJSONData[i].Action + '</td><td id="sptxt">' + $('<a/>').html($('<a/>').html(text).text()).text() + '</td><td>' + LoadJSONData[i].ModifiedOn + " " + LoadJSONData[i].ModifiedBy + '</td>');
                $row.find('#sptxt').html($('<a/>').html(text).text());

                $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvPastFamilyHx tbody").last().append($row);
                //  });
            }
        }
        else {
            $("#" + Clinical_FamilyHx.params.PanelID + ' #pnlFamilyHx_Result #dgvPastFamilyHx').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Known Family History"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bInfo": false, "bPaginate": false, "bSortable": false, "aTargets": [0] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_FamilyHx.params.PanelID + ' #pnlFamilyHx_Result #dgvPastFamilyHx'))
            ;
        else {
            $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvPastFamilyHx").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[0, "asc"]], "aoColumnDefs": [{ "bInfo": false, "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        }

        $("#" + Clinical_FamilyHx.params.PanelID + " #pnlFamilyHx_Result #dgvPastFamilyHx_filter").remove();
    },

    //Function Name: enableFavoriteList
    //Author Name: Humaira Yousaf
    //Created Date: 01-07-2016
    //Description: Enables Favorite List
    enableFavoriteList: function (deleteRow) {

        $('#' + Clinical_FamilyHx.params.PanelID + ' #ulFavoriteListFamilyHxContent li').each(function (index, item) {
            if ($(deleteRow).attr('icd9desc') != null) {
                var deleteRowCPTCode = $(deleteRow).text() != null ? $(deleteRow).attr('icd9desc') + '-' + $(deleteRow).attr('snomedcode') : "";
                if (deleteRowCPTCode == $(item).attr("id")) {
                    $(item).removeClass('disableAll');
                }
            }
            else {
                $(item).removeClass('disableAll');

                //var deleteRowCPTCode = $(deleteRow).find("input[id*='ProcedureProcedure']").val() != null ? $(deleteRow).find("input[id*='ProcedureProcedure']").val() : "";
                //if (deleteRowCPTCode == $(item).attr("id")) {
                //    $(item).removeClass('disableAll');
                //}
            }
        });
    },

    enableDisbaleUlItems: function (ulId, isEnable) {

        $('#' + Clinical_FamilyHx.params.PanelID + ' #' + ulId + ' li').each(function () {

            if (isEnable) {
                $(this).removeClass('disableAll');
            }
            else {
                $(this).addClass('disableAll');
            }
        });
    },
    SaveFreeTextStatus: function () {
        if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
            panel = "#pnlClinicalProgressNote #pnlClinicalFamilyHx";
        }
        else {
            panel = "#pnlClinicalFamilyHx";
        }
        var IsFreeText = false;
        $('input[name=rdodiagnose]:checked', panel).val() == 1 ? IsFreeText = true : IsFreeText = false;
        EMRUtility.insertUpdateFreeTextStatus("Clinical_FamilyHx", IsFreeText);
    },

    cacheFamilyHxJSON: function (memberId, diseaseId, diseaseData, diseaseLi) {

        diseaseData = JSON.parse(diseaseData);
        var RadRelativeDiedValue = "";
        if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #RadYesRelativeDied').is(':checked')) {
            RadRelativeDiedValue = "True";
        }
        else if ($('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #RadNoRelativeDied').is(':checked')) {
            RadRelativeDiedValue = "False";
        }
        else {
            RadRelativeDiedValue = "";
        }
        diseaseData.RadRelativeDied = RadRelativeDiedValue;
        diseaseData = JSON.stringify(diseaseData);

        var dfd = $.Deferred();
        var diseaseIndex = -1;
        var isFavListOpened = $('#' + Clinical_FamilyHx.params.PanelID + " #favSectionDiv").hasClass("toggledHor");
        var FavListVal = $('#' + Clinical_FamilyHx.params.PanelID + ' #ddlFavoriteListFamilyHx').val();
        if (Clinical_HistorySummary.HistoryCacheList.FamilyHx == null) {

            var patientId;

            if (Clinical_FamilyHx.params.patientID == null) {
                patientId = $('#PatientProfile #hfPatientId').val();
            } else {
                patientId = Clinical_FamilyHx.params.patientID;
            }
            var noteId = Clinical_ProgressNote.params.NotesId;

            var familyHxData = {
                FamilyHxId: $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val(),
                FamilyHxDate: $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #dtFamilyHxDate").val(),
                FamilyHxUnremarkable: $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #chkFamilyHxUnremarkable").prop("checked"),
                FamilyOverallComments: $('#' + Clinical_FamilyHx.params.PanelID + " #txtFamilyOverallComments").val(),
                PatientId: patientId,
                NotesId: noteId,
                DiseaseId: diseaseId,
                FamilyMemberId: memberId,
                FamilyMemberDisease: [],
                FamilyMemberDiseaseDetail: [],
                FavListName: Clinical_FamilyHx.FavListName,
                isFavListOpened: isFavListOpened,
                FavListVal: FavListVal
            }

            Clinical_HistorySummary.HistoryCacheList.FamilyHx = familyHxData;
        }
        else {
            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyHxId = $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val();
            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyHxDate = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #dtFamilyHxDate").val();
            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyHxUnremarkable = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #chkFamilyHxUnremarkable").prop("checked");
            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyOverallComments = $('#' + Clinical_FamilyHx.params.PanelID + " #txtFamilyOverallComments").val();
            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FavListName = Clinical_FamilyHx.FavListName;
            Clinical_HistorySummary.HistoryCacheList.FamilyHx.isFavListOpened = isFavListOpened;
            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FavListVal = FavListVal;
        }

        if (diseaseId != null) {

            $.grep(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease, function (item, index) {
                if (item.DiseaseId == diseaseId && item.FamilyMemberId == memberId) {
                    diseaseIndex = index;
                    return;
                }
            });

            var selectedMember = $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyDisease ul#ulFamilyMember li#" + memberId);
            var selectedDisease = $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyDisease ul#ulFamilyHxDisease li#" + diseaseId);

            var memberDetailId;
            if ($(selectedDisease).attr('memberdetailid') != "") {
                memberDetailId = $(selectedDisease).attr('memberdetailid');
            }
            else {
                memberDetailId = 0;
            }



            var familyData = JSON.parse(diseaseData);

            var diseaseText = $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyHxDisease ul#ulFamilyHxDisease li#" + diseaseId).text();
            var familyMemberText = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li#" + memberId).text();
            var healthStatusText = familyData.HealthStatus_text == "- Select -" ? "" : familyData.HealthStatus_text;

            if (diseaseIndex != -1) {

                var diseaseDetailIndex = -1;

                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].FamilyMemberId = memberId;
                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].DiseaseId = diseaseId;
                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].FamilyHxId = $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val();

                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].LexiCode = familyData.LexiCode;
                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].LexiCodeDescription = familyData.LexiCodeDescription;

                if (selectedDisease.attr('icd9desc') && selectedDisease.attr('icd9desc') != '') {

                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].ICD9Code = selectedDisease.attr("icd9code");
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].ICD9CodeDescription = selectedDisease.attr("icd9desc");
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].ICD10Code = selectedDisease.attr("icd10code");
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].ICD10CodeDescription = selectedDisease.attr("icd10desc");
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].SNOMEDID = selectedDisease.attr("snomedcode");
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].SNOMEDDescription = selectedDisease.attr("snomeddesc");

                } else {
                    var freeTextICD = selectedDisease.text() != "" ? selectedDisease.text() : "-1";
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].FreeTextICD = freeTextICD;
                }

                //Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].ICD9Code = selectedDisease.attr("icd9code");
                //Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].ICD9CodeDescription = selectedDisease.attr("icd9desc");
                //Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].ICD10Code = selectedDisease.attr("icd10code");
                //Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].ICD10CodeDescription = selectedDisease.attr("icd10desc");
                //Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].SNOMEDID = selectedDisease.attr("snomedcode");
                //Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].SNOMEDDescription = selectedDisease.attr("snomeddesc");

                $.grep(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail, function (item, index) {
                    if (item.FamilyMemberId == memberId && item.DiseaseId == diseaseId) {
                        diseaseDetailIndex = index;
                        return;
                    }
                });

                if (diseaseDetailIndex != -1) {
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].DiseaseId = diseaseId;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].MemberId = memberId;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].MemberDetailId = memberDetailId;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].FamilyHxId = $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val();
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].MedicalDiseaseComments = familyData.MedicalDiseaseComments;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].DiseaseText = diseaseText;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].FamilyMemberText = familyMemberText;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].HealthStatusText = healthStatusText;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].FamilyMemberId = memberId;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].YearofBirth = familyData.YearofBirth;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].AgeAtDeath = familyData.AgeAtDeath;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].AgeAtDiagnosis = familyData.AgeAtDiagnosis;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].FamilyMemberComments = familyData.FamilyMemberComments;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].RadRelativeDied = RadRelativeDiedValue;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].HealthStatus = familyData.HealthStatus;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].HealthStatus_text = familyData.HealthStatus_text;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].HealthStatus_RefValue = familyData.HealthStatus_RefValue;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].JSON = diseaseData;

                }
                else {
                    var diseaseList = {
                        DiseaseId: diseaseId,
                        DiseaseLi: diseaseLi,
                        MemberId: memberId,
                        JSON: diseaseData,
                        FamilyHxId: $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val(),
                        MedicalDiseaseComments: familyData.MedicalDiseaseComments,
                        DiseaseText: diseaseText,
                        FamilyMemberText: familyMemberText,
                        HealthStatusText: healthStatusText,
                        FamilyMemberId: memberId,
                        YearofBirth: familyData.YearofBirth,
                        AgeAtDeath: familyData.AgeAtDeath,
                        AgeAtDiagnosis: familyData.AgeAtDiagnosis,
                        FamilyMemberComments: familyData.FamilyMemberComments,
                        RadRelativeDied: RadRelativeDiedValue,
                        HealthStatus: familyData.HealthStatus,
                        HealthStatus_text: familyData.HealthStatus_text,
                        HealthStatus_RefValue: familyData.HealthStatus_RefValue
                    }

                    Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail.push(diseaseList);

                }
            }
            else {

                var IsFreeText = selectedDisease.attr('icd9desc') && selectedDisease.attr('icd9desc') != '' ? false : true;
                var freeTextICD = '';

                if (IsFreeText) {
                    freeTextICD = selectedDisease.text() != "" ? selectedDisease.text() : "-1";
                }
                else {
                    freeTextICD = null;
                }

                var disease = {
                    FamilyMemberId: memberId,
                    DiseaseId: diseaseId,
                    FamilyHxId: $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val(),
                    LexiCode: familyData.LexiCode,
                    LexiCodeDescription: familyData.LexiCodeDescription,
                    FreeTextICD: freeTextICD,
                    ICD9Code: IsFreeText == false ? selectedDisease.attr("icd9code") : "",
                    ICD9CodeDescription: IsFreeText == false ? selectedDisease.attr("icd9desc") : "",
                    ICD10Code: IsFreeText == false ? selectedDisease.attr("icd10code") : "",
                    ICD10CodeDescription: IsFreeText == false ? selectedDisease.attr("icd10desc") : "",
                    SNOMEDID: IsFreeText == false ? selectedDisease.attr("snomedcode") : "",
                    SNOMEDDescription: IsFreeText == false ? selectedDisease.attr("snomeddesc") : "",
                    HealthStatusText: healthStatusText,
                }

                var diseaseDetail = {
                    DiseaseId: diseaseId,
                    DiseaseLi: diseaseLi,
                    MemberId: memberId,
                    MemberDetailId: memberDetailId,
                    JSON: diseaseData,
                    FamilyHxId: $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val(),
                    MedicalDiseaseComments: familyData.MedicalDiseaseComments,
                    DiseaseText: diseaseText,
                    FamilyMemberText: familyMemberText,
                    HealthStatusText: healthStatusText,
                    FamilyMemberId: memberId,
                    FreeTextICD: freeTextICD,
                    YearofBirth: familyData.YearofBirth,
                    AgeAtDeath: familyData.AgeAtDeath,
                    AgeAtDiagnosis: familyData.AgeAtDiagnosis,
                    FamilyMemberComments: familyData.FamilyMemberComments,
                    RadRelativeDied: RadRelativeDiedValue,
                    HealthStatus: familyData.HealthStatus,
                    HealthStatus_text: familyData.HealthStatus_text,
                    HealthStatus_RefValue: familyData.HealthStatus_RefValue
                }

                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease.push(disease);
                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail.push(diseaseDetail);
            }
        }
        else {
            var diseases = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyHxDisease li").length;

            if (diseases == 0) {
                var details = $('#' + Clinical_FamilyHx.params.PanelID + " #FamilyMemberDetails").clone();
                $(details).resetAllControls(null);
                var json = $(details).getMyJSONByName();


                var updatedJSON = $('#' + Clinical_FamilyHx.params.PanelID + " #FamilyMemberDetails").getMyJSONByName();

                //
                var updatJSON1 = updatedJSON;
                var RadYesRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #FamilyMemberDetails #RadYesRelativeDied').prop("checked");
                var RadNoRelativeDied = $('#' + Clinical_FamilyHx.params.PanelID + ' #FamilyMemberDetails #RadNoRelativeDied').prop("checked");
                var RadRelativeDied = "";
                if (RadYesRelativeDied) {
                    RadRelativeDied = true;
                }
                else if (RadNoRelativeDied) {
                    RadRelativeDied = false;
                }

                updatJSON1 = JSON.parse(updatJSON1);
                updatJSON1.RadRelativeDied = RadRelativeDied;
                updatJSON1 = JSON.stringify(updatJSON1);
                updatedJSON = updatJSON1;
                //



                if (json != updatedJSON) {

                    var addedDisease = -1;
                    if ($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfDiseaseId").val() != "") {
                        addedDisease = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfDiseaseId").val()
                    }
                    else {
                        addedDisease = "-1";
                    }

                    var diseaseIndex = -1;

                    $.grep(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease, function (item, index) {
                        if (item.DiseaseId == addedDisease && item.FamilyMemberId == memberId) {
                            diseaseIndex = index;
                            return;
                        }
                    });

                    var selectedMember = $('#' + Clinical_FamilyHx.params.PanelID + " div#FamilyDisease ul#ulFamilyMember li#" + memberId);
                    var memberDetailId;
                    if ($('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfMemberdetailid").val() != "") {
                        memberDetailId = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #hfMemberdetailid").val();
                    }
                    else {
                        memberDetailId = 0;
                    }



                    var familyData = JSON.parse(diseaseData);

                    var freeTextICD = "-1";

                    var familyMemberText = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li#" + memberId).text();
                    var healthStatusText = familyData.HealthStatus_text == "- Select -" ? "" : familyData.HealthStatus_text;

                    if (diseaseIndex != -1) {

                        var diseaseDetailIndex = -1;

                        Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].FamilyMemberId = memberId;
                        Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].DiseaseId = addedDisease;
                        Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].FamilyHxId = $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val();

                        Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].LexiCode = familyData.LexiCode;
                        Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].LexiCodeDescription = familyData.LexiCodeDescription;
                        Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease[diseaseIndex].FreeTextICD = freeTextICD;

                        $.grep(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail, function (item, index) {
                            if (item.FamilyMemberId == memberId && item.DiseaseId == addedDisease) {
                                diseaseDetailIndex = index;
                                return;
                            }
                        });

                        if (diseaseDetailIndex != -1) {
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].DiseaseId = addedDisease;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].MemberId = memberId;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].MemberDetailId = memberDetailId;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].FamilyHxId = $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val();
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].MedicalDiseaseComments = familyData.MedicalDiseaseComments;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].FamilyMemberText = familyMemberText;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].HealthStatusText = healthStatusText;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].FamilyMemberId = memberId;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].YearofBirth = familyData.YearofBirth;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].AgeAtDeath = familyData.AgeAtDeath;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].AgeAtDiagnosis = familyData.AgeAtDiagnosis;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].FamilyMemberComments = familyData.FamilyMemberComments;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].RadRelativeDied = RadRelativeDiedValue;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].HealthStatus = familyData.HealthStatus;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].HealthStatus_text = familyData.HealthStatus_text;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].HealthStatus_RefValue = familyData.HealthStatus_RefValue;
                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail[diseaseDetailIndex].JSON = diseaseData;

                        }
                        else {
                            var diseaseList = {
                                DiseaseId: addedDisease,
                                MemberId: memberId,
                                JSON: diseaseData,
                                FamilyHxId: $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val(),
                                MedicalDiseaseComments: familyData.MedicalDiseaseComments,
                                FamilyMemberText: familyMemberText,
                                HealthStatusText: healthStatusText,
                                FamilyMemberId: memberId,
                                YearofBirth: familyData.YearofBirth,
                                AgeAtDeath: familyData.AgeAtDeath,
                                AgeAtDiagnosis: familyData.AgeAtDiagnosis,
                                FamilyMemberComments: familyData.FamilyMemberComments,
                                RadRelativeDied: RadRelativeDiedValue,
                                HealthStatus: familyData.HealthStatus,
                                HealthStatus_text: familyData.HealthStatus_text,
                                HealthStatus_RefValue: familyData.HealthStatus_RefValue
                            }

                            Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail.push(diseaseList);

                        }
                    }
                    else {
                        var disease = {
                            FamilyMemberId: memberId,
                            DiseaseId: addedDisease,
                            FamilyHxId: $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val(),
                            LexiCode: familyData.LexiCode,
                            LexiCodeDescription: familyData.LexiCodeDescription,
                            FreeTextICD: freeTextICD,
                            HealthStatusText: healthStatusText,
                        }

                        var diseaseDetail = {
                            DiseaseId: addedDisease,
                            MemberId: memberId,
                            MemberDetailId: memberDetailId,
                            JSON: diseaseData,
                            FamilyHxId: $('#' + Clinical_FamilyHx.params.PanelID + " #hfFamilyHxId").val(),
                            MedicalDiseaseComments: familyData.MedicalDiseaseComments,
                            FamilyMemberText: familyMemberText,
                            HealthStatusText: healthStatusText,
                            FamilyMemberId: memberId,
                            FreeTextICD: freeTextICD,
                            YearofBirth: familyData.YearofBirth,
                            AgeAtDeath: familyData.AgeAtDeath,
                            AgeAtDiagnosis: familyData.AgeAtDiagnosis,
                            FamilyMemberComments: familyData.FamilyMemberComments,
                            RadRelativeDied: RadRelativeDiedValue,
                            HealthStatus: familyData.HealthStatus,
                            HealthStatus_text: familyData.HealthStatus_text,
                            HealthStatus_RefValue: familyData.HealthStatus_RefValue
                        }

                        Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease.push(disease);
                        Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail.push(diseaseDetail);
                    }
                }
            }
        }

        dfd.resolve();
        return dfd;
    },


    getCacheFamilyHxJSON: function (memberId, diseaseId) {
        var diseaseIndex = -1;

        if (Clinical_HistorySummary.HistoryCacheList.FamilyHx != null) {
            var familymember = $.grep(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease, function (item, index) {
                if (item.FamilyMemberId == memberId && item.DiseaseId == diseaseId) {
                    diseaseIndex = index;
                }
            });

            if (diseaseIndex != -1) {
                if (Clinical_HistorySummary.HistoryCacheList.FamilyHx != null && Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail != null) {
                    var familydisease = $.grep(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail, function (item, index) {
                        if (item.MemberId == memberId && item.DiseaseId == diseaseId) {
                            return item;
                        }
                    });

                    if (familydisease.length > 0) {
                        return familydisease[0].JSON;
                    }
                }
            }
        }

        return '';
    },
}