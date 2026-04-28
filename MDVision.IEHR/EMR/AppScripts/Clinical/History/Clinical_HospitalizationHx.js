Clinical_HospitalizationHx = {
    //Author: Muhammad Arshad
    //Date: 14-01-2016
    //This file will handle all actions performed for Hospitalization History and it's child handling
    //Once HospitalizationHx will be created then it's child can be created then.
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    bNextPrev: false,
    controlToInvoke: null,
    currentSelectedDisease: [],
    hospitalizationHxJSON: '',
    hospitalizationDate: '',
    unremarkable: false,
    overallComments: '',
    FavListName: 'ClinicalHospitalizationHx',

    //Author: Muhammad Abid Ali
    //Date: 21-01-2016
    //This function will be called once tab is clicked, it expects parameters to be used for HospitalizationHx
    Load: function (params) {
        Clinical_HospitalizationHx.params = params;

        $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        if (Clinical_HospitalizationHx.params.mode == null) {
            Clinical_HospitalizationHx.params.mode = "Add";
        }
        if (Clinical_HospitalizationHx.params.PanelID != 'pnlClinicalHospitalizationHx') {
            Clinical_HospitalizationHx.params.PanelID = Clinical_HospitalizationHx.params.PanelID + ' #pnlClinicalHospitalizationHx';
        } else {
            Clinical_HospitalizationHx.params.PanelID = 'pnlClinicalHospitalizationHx';
            Clinical_HospitalizationHx.params.CurrentNotesProviderId = "";
        }
        Clinical_HospitalizationHx.ResetFormData();
        var HospitalizationHxId = "";
        if (Clinical_HospitalizationHx.params.mode == "Add" || Clinical_HospitalizationHx.params.HospitalizationHxId == null || Clinical_HospitalizationHx.params.HospitalizationHxId == "" || Clinical_HospitalizationHx.params.HospitalizationHxId == "-1") {
            HospitalizationHxId = "-1";
        }
        else if (Clinical_HospitalizationHx.params.mode == "Edit") {

            HospitalizationHxId = Clinical_HospitalizationHx.params.HospitalizationHxId;
        }
        //if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabNotes" ) {
        //    Clinical_HospitalizationHx.bIsFirstLoad = true;
        //    $('#divViewHistorySummary').addClass('hidden');
        //    $(' #pnlClinicalHospitalizationHx').removeClass('row');
        //}
        if (Clinical_HospitalizationHx.bIsFirstLoad == true) {
            EMRUtility.setFavoriteSectionStyle(Clinical_HospitalizationHx.params.PanelID);
            Clinical_HospitalizationHx.favoriteListSearch();


            var self = $('#' + Clinical_HospitalizationHx.params.PanelID);
            self.loadDropDowns(true).done(function () {

                $.when(Clinical_HospitalizationHx.loadHospitalizationHxTabnUnserializeForm()).then(function () {
                    Clinical_HospitalizationHx.loadfavoriteListContent($("#" + Clinical_HospitalizationHx.params.PanelID + " #ddlFavoriteListHospitalizationHx"));

                });
                // Registers bootstrap validator in DOM
                Clinical_HospitalizationHx.validateHospitalizationHx();
                // loads HospitalizationHx
                Clinical_HospitalizationHx.loadHospitalizationHx('disease', true);
                if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnHospitalizationDiseaseSave').addClass('hidden');
                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnAddVitalsOnNote').addClass('hidden');
                    var details = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").clone();
                    $(details).resetAllControls(null);
                    Clinical_HospitalizationHx.hospitalizationHxJSON = $(details).getMyJSONByName();
                }
                else {
                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnHospitalizationDiseaseSave').removeClass('hidden');
                }
            });

            utility.CreateDatePicker(Clinical_HospitalizationHx.params.PanelID + ' #dtHospitalizationHxDate', function () {
            }, true);
            if (Clinical_HospitalizationHx.params.mode == "Edit") {
                if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #dtHospitalizationHxDate').removeClass('disableAll');
                }
                else {
                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #dtHospitalizationHxDate').addClass('disableAll');
                }

            }

            utility.CreateDatePicker(Clinical_HospitalizationHx.params.PanelID + ' section#sectionHospitalizationDetails div#HospitalizationDetails #dtpHospitalizationAdmissionDate', function () {

            }, false);

            utility.CreateDatePicker(Clinical_HospitalizationHx.params.PanelID + ' section#sectionHospitalizationDetails div#HospitalizationDetails #dtpHospitalizationDischargeDate', function () {
            }, false);

            EMRUtility.ValidateFromToDate('frmClinicalHospitalizationHx', 'dtpHospitalizationAdmissionDate', 'dtpHospitalizationDischargeDate', true);

            if ($('#' + Clinical_HospitalizationHx.params.PanelID + ' #PatientProfile #hfPatientId').val() != "") {
                $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            }

            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').data('serialize', $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').serialize());

            //Start//22/01/2016//Ahmad Raza//Code to add Add on Note Button and Next/Previous buttons when HospitalizationHx opened from Note
            if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #pnlClinicalHospitalizationHx').removeClass('row');
                EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_HospitalizationHx.params.PanelID, 'History', 'HospitalizationHx', 'Clinical_HospitalizationHx.unLoadTab(true);', null, true);
                //EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_HospitalizationHx.params.PanelID, 'History', 'HospitalizationHx', "$('#actionPanClinicalProgressNote #pnlClinicalHospitalizationHx').remove();");
                //EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_HospitalizationHx.params.PanelID, 'History', 'HospitalizationHx', '');
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnAddVitalsOnNote').show();

                if ($('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ulHospitalizationDisease li.active").length > 0) {
                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnAddVitalsOnNote').prop('disabled', false);
                }
                else {
                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnAddVitalsOnNote').prop('disabled', true);
                }

                //  $('#' + Clinical_HospitalizationHx.params.PanelID + '  #dtHospitalizationHxDate').prop('disabled', true);
            } else {
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnAddVitalsOnNote').hide();
                $('#' + Clinical_HospitalizationHx.params.PanelID + '  #dtHospitalizationHxDate').prop('disabled', false);
            }
            //End//22/01/2016//Ahmad Raza//Code to add Add on Note Button and Next/Previous buttons when HospitalizationHx opened from Note

            $.when(Clinical_HospitalizationHx.domReadyFunction()).then(function () {
                //Start || 15 July, 2016 || Talha Tanweer || trigger first disease detail open  EMR 1562
                //   $("#" + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease li:first").trigger("click");

                //End   || 15 July, 2016 || Talha Tanweer || trigger first disease detail open  EMR 1562
            });

            Clinical_HospitalizationHx.bIsFirstLoad = false;

        }
        Clinical_HospitalizationHx.toggleReadyFunction();

        if (EMRUtility.getFreeTextStatus("Clinical_HospitalizationHx")) {
            var panel = "#pnlClinicalHospitalizationHx";
            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalHospitalizationHx";
            }

            $(panel + " #diseaseAutoComplete").hide();
            if ($(panel + " #diseaseFreeText").hasClass("hidden")) {
                $(panel + " #diseaseFreeText").removeClass("hidden");
            }
            $(panel + " #rdSearchFreeText").prop('checked', 'checked');
        }
        else {
            var panel = "#pnlClinicalHospitalizationHx";
            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalHospitalizationHx";
            }

            $(panel + " #diseaseAutoComplete").show();
            if (!$(panel + " #diseaseFreeText").hasClass("hidden")) {
                $(panel + " #diseaseFreeText").addClass("hidden");
            }
            $(panel + " #rdSearchICD").prop('checked', 'checked');
        }

        //if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
        //    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnHospitalizationDiseaseSave').addClass('hidden');
        //    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnAddVitalsOnNote').addClass('hidden');
        //    var details = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").clone();
        //    $(details).resetAllControls(null);
        //    Clinical_HospitalizationHx.hospitalizationHxJSON = $(details).getMyJSONByName();
        //}
        //else {
        //    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnHospitalizationDiseaseSave').removeClass('hidden');
        //}
    },
    ResetFormData:function(){
        var details = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx");
        $(details).resetAllControls(null);
        Clinical_HospitalizationHx.bIsFirstLoad = true;
        Clinical_HospitalizationHx.ChangeCurrentPast(1, null, null, null);
        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ulHospitalizationDisease").html("");
        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #HospitalizationHxDisease").removeClass('disableAll');
    },
    
    loadHospitalizationHxTabnUnserializeForm: function () {
        var dfd = $.Deferred();
        $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx  #sectionDiseaseDetails').data('serialize', null);
        $.when(Clinical_HospitalizationHx.loadHospitalizationHx('disease', true)).then(function () {
            dfd.resolve();
        });
        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #hfHospitalizationHxType").val('disease');
        return dfd;
    },
    showHospitalizationHxHistory: function (hospitalizationHxId) {

        var grantParentCtrlId = null;
        var parentCtrlId = Clinical_HospitalizationHx.params.TabID;
        if (Clinical_HospitalizationHx.params.TabID == "clinicalTabProgressNote") {

            parentCtrlId = "Clinical_HistorySummary";
            grantParentCtrlId = "pnlClinicalProgressNote";
        }
        EMRUtility.showCurrentItemHistory(Clinical_HospitalizationHx.params.PanelID, null, null, "HospitalizationHx,HospitalizationHx_Disease", null, parentCtrlId, hospitalizationHxId, grantParentCtrlId);
    },
    SaveFavToggelStatus: function (FavListVal) {
        var isFavListOpened = $('#' + Clinical_HospitalizationHx.params.PanelID + " #favSectionDiv").hasClass("toggledHor");
        $.when(EMRUtility.insertUpdateFavListStatus(Clinical_HospitalizationHx.FavListName, isFavListOpened)).then(function () {
            EMRUtility.insertUpdateFavListVal(Clinical_HospitalizationHx.FavListName, FavListVal);
        });
    },
    favoriteListSearch: function () {
        var ProviderId = null;
        if (Clinical_HospitalizationHx.params.CurrentNotesProviderId != "undefined" && Clinical_HospitalizationHx.params.CurrentNotesProviderId && Clinical_HospitalizationHx.params.CurrentNotesProviderId!="")
            ProviderId = Clinical_HospitalizationHx.params.CurrentNotesProviderId;

        Favorite_ProcedureOrder.searchFavoriteList_DBCall("HospitalizationHistory", null, 1, 5000, ProviderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $ddl = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #ddlFavoriteListHospitalizationHx');
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
                    EMRUtility.getFavListValue(Clinical_HospitalizationHx.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "" && response1.favListVal != "-1") {
                                if ($("#" + Clinical_HospitalizationHx.params.PanelID + " #ddlFavoriteListHospitalizationHx option[value='" + response1.favListVal + "']").length > 0) {
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
                //   $ddl.trigger("onchange");

            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });

    },
    AutoSearchFavHospitalizationHx: function () {
        utility.Keyupdelay(function () {
            Clinical_HospitalizationHx.loadfavoriteListContent();
        });
    },
    loadfavoriteListContent: function (obj) {
        if (typeof obj == typeof undefined || obj == null) {
            obj = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #ddlFavoriteListHospitalizationHx');
        }
        var SearchData = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #FavSearchBox').val();
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            //Start 01-07-2016 Humaira Yousaf to disable Select All link
            if (selectedOption.length > 0 && selectedOption.attr("id") != "-1") {
                Clinical_HospitalizationHx.favoriteList_CPTSearch(selectedOption.attr("id"), SearchData);
            }
            else {
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #ulFavoriteListHospitalizationHxContent').empty();
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
            }
            //End 01-07-2016 Humaira Yousaf to disable Select All link
        }
    },

    favoriteList_CPTSearch: function (FavoriteListId, SearchData) {
        var $UL = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #ulFavoriteListHospitalizationHxContent');
        Clinical_HospitalizationHx.searchFavoriteList_ICD_DBCall(null, FavoriteListId, 1, 5000, SearchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

               
                $UL.empty();

                if (response.FavoriteListICDCount > 0) {
                    var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);

                    if (FavoriteListJSON.length > 0) {
                        $('#' + Clinical_HospitalizationHx.params.PanelID + ' #favSelectAllLink').removeClass('disableAll');
                    }
                    else {
                        $('#' + Clinical_HospitalizationHx.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
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

                        var onclick = 'Clinical_HospitalizationHx.BindHospitalizationHxUl(\'' + item.ICD9Code + '\',\'' + item.ICD9CodeDescription + '\',\'' + item.ICD10Code + '\',\'' + item.ICD10CodeDescription + '\',\'' + item.SNOMEDID + '\',\'' + item.SNOMEDDescription + '\',this)';

                        var LiId = item.ICD9CodeDescription + '-' + item.SNOMEDID;

                        var isFound = Clinical_HospitalizationHx.isFavoriteHistoryFound(LiId, item.ICD9CodeDescription);
                        if (isFound == true) {
                            $UL.append('<li class="disableAll" onclick="' + onclick + '" id="' + LiId + '">' + item.ICD9CodeDescription + '</li>');
                        }
                        else {
                            $UL.append('<li onclick="' + onclick + '" id="' + LiId + '">' + item.ICD9CodeDescription + '</li>');
                        }

                        //$UL.append('<li onclick="' + onclick + '">' + item.ICD9CodeDescription + ' - ' + item.SNOMEDID + '</li>');


                    });

                }
            }
            else {
                $UL.empty();
            }
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
    isFavoriteHistoryFound: function (favICDCode, favCPTDesc) {

        var isFound = false;
        $("#" + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease li").each(function (index, item) {
            if ($(item).attr('icd9desc') != null) {
                var currentRowCPTCode = $(item).text() != null ? $(item).attr('icd9desc') + '-' + $(item).attr('snomedcode') : "";
                if (currentRowCPTCode == favICDCode) {
                    isFound = true;
                }
            }
        });

        return isFound;
    },
    BindHospitalizationHxUl: function (icd9Code, icd9Description, icd10Code, icd10Description, snomedCode, snomedDescription, sender) {

        var currId = -1;
        $("#pnlClinicalHospitalizationHx #frmClinicalHospitalizationHx div#HospitalizationHxDisease #ulHospitalizationDisease li[id*='-']").each(function (i, item) {

            currId = $(this).attr("id");

        });

        currId = parseInt(currId) + (-1);

        var li = "<li  id=" + currId + " onclick='Clinical_HospitalizationHx.fillHospitalizationHxDisease(this, event);' onmouseover='Clinical_HospitalizationHx.showIcon(this);' onmouseout='Clinical_HospitalizationHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_HospitalizationHx.deleteHospitalizationHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>";


        var IsAlreadyExist = false;
        $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease li').each(function () {
            if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                IsAlreadyExist = true;
            }
        });

        if (!IsAlreadyExist) {
            $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease').append(li);
            $(li).trigger('click');
            $('#pnlClinicalHospitalizationHx #txtDisease').val('');
            if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                var diseaseId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease > li.active").attr('id');
                var disease = $(li).get(0).outerHTML;
                var diseaseDetails = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionDiseaseDetails").clone();
                $(diseaseDetails).resetAllControls(null);
                var diseaseData = $(diseaseDetails).getMyJSONByName();
                Clinical_HospitalizationHx.cacheHospitalizationHxJSON(diseaseId, diseaseData, disease);
            }
            $(sender).addClass('disableAll');
        }
        else {
            utility.DisplayMessages('Diagnosis already added', 2);

            $('#pnlClinicalHospitalizationHx #txtDisease').val('');
        }

    },
    selectAllFavoriteListContent: function () {

        $('#' + Clinical_HospitalizationHx.params.PanelID + ' #ulFavoriteListHospitalizationHxContent li').each(function (i, item) {
            $(item).trigger("click");
        });
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

    //Author: Muhammad Abid Ali
    //Date: 21-01-2016
    //This function will handle Initialization of KeyPad control
    domReadyFunction: function () {
        var dfd = new $.Deferred();
        $(function () {
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
            //EMR-70 Bug number Resolution
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx [data-plugin-keyboard-numpad]').keyboard({
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

            dfd.resolve();
        });
        dfd.resolve();
        return dfd.promise();
    },

    toggleReadyFunction: function () {
        $(function () {
            (function ($) {
                'use strict';
                $(function () {
                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);

        });


    },

    //Author: Abid Ali
    //Date: 21-01-2016
    //For Form submission and bootstrap validation
    validateHospitalizationHx: function () {
        $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   //HospitalizationDiseaseHospital: {
                   //    group: '.col-sm-6',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   HospitalizationDiseaseStayId: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   HospitalizationDiseaseStayDuration: {
                       group: '.col-xs-5',
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
                Clinical_HospitalizationHx.HospitalizationHxSave("Disease");
            }
            e.type = "";
        });

        Clinical_HospitalizationHx.enableDurationValidation();
    },


    //Author: Abid Ali
    //Date: 21-01-2016
    //This function will handle fill of HospitalizationHx and it's childs as specified by HospitalizationHxType
    loadHospitalizationHx: function (HospitalizationHxType, isLoadNew) {
        //if (HospitalizationHxType == null)
        //    HospitalizationHxType = "disease";

        if (isLoadNew == true) {
            Clinical_HospitalizationHx.loadHospitalizationHxComponent(HospitalizationHxType, undefined, undefined, true);

            $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").addClass('disableAll');
            //var self = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').
            //    find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,ul').each(function () {
            //        Clinical_HospitalizationHx.resetControlValue(this);
            //    });
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxType").val(HospitalizationHxType);
        } else {
            Clinical_HospitalizationHx.loadHospitalizationHxComponent(HospitalizationHxType);
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxType").val(HospitalizationHxType);
        }
        $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxType").val(HospitalizationHxType);
    },

    //Author: Farooq Ahmad
    //Date: 21/01/2016
    //This function will handle fill of HospitalizationHx
    loadHospitalizationHxComponent: function (HospitalizationHxType, diseaseId, isDiseaseFill, isNewLoad, isRefreshHistoryGrid) {
        BackgroundLoaderShow(true);
        Clinical_HospitalizationHx.fillHospitalizationHx(HospitalizationHxType, diseaseId).done(function (response) {
            if (response != "") {
                if (response.status != false) {

                    response = JSON.parse(response);



                    var DiseaseLoadDetails = JSON.parse(response.HospitalizationHxDiseaseLoad_JSON);
                    var Hospitalizationhx_detail = JSON.parse(response.HospitalizationHxFill_JSON);



                    if (Hospitalizationhx_detail.HospitalizationHxId > 0) {

                        Clinical_HospitalizationHx.params.HxTypeId = Hospitalizationhx_detail.HospitalizationHxId;
                        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #aHistory").removeClass('hidden');
                        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #aHistory").attr('onclick', 'Clinical_HospitalizationHx.showHospitalizationHxHistory(' + Hospitalizationhx_detail.HospitalizationHxId + ')');

                        if (isRefreshHistoryGrid == null) {
                            Clinical_HospitalizationHx.BindCurrentHospitalizationSoapText(response, DiseaseLoadDetails.length > 0);
                        }
                    }
                    else {
                        var $row = $('<tr/>');
                        $row.append('<td></td><td>No Known Hospitalization History</td><td></td>');
                        $("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #dgvHospitalizationHx tbody").html($row);
                        $("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #divSwitch").addClass('disableAll');

                    }


                    var Disease_detail = JSON.parse(response.DiseaseFill_JSON);

                    var self = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx");

                    if (isNewLoad == true) {
                        if ($('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ulHospitalizationDisease li").length > 0) {

                            if ($('#' + Clinical_HospitalizationHx.params.PanelID + " #hfSelectedDisease").val() != "" && $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfSelectedDisease").val() != "undefined") {


                                var list = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ulHospitalizationDisease li#" + $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfSelectedDisease").val());

                                $(list).addClass('active');
                                $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #sectionHospitalizationDetails").removeClass("disableAll");
                                list.trigger('click');

                                //Start/10-02-2016/Abid Ali/ Fill hospitalizationHx main table
                                Clinical_HospitalizationHx.fillHospitalizationHxMainTable(Hospitalizationhx_detail, self);
                                //End/10-02-2016/Abid Ali/ Fill hospitalizationHx main table

                                Clinical_HospitalizationHx.enableDischargeDate();
                                return;
                            }
                        }

                    }
                    if (isDiseaseFill != true) {
                        Clinical_HospitalizationHx.loadHospitalizationHxDiseases('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx div#HospitalizationHxDisease #ulHospitalizationDisease", DiseaseLoadDetails, "disease");

                        //Start/10-02-2016/Abid Ali/ Fill hospitalizationHx main table
                        Clinical_HospitalizationHx.fillHospitalizationHxMainTable(Hospitalizationhx_detail, self);
                        //End/10-02-2016/Abid Ali/ Fill hospitalizationHx main table
                    }

                    //Hospitalization History Clinical Module -> Date
                    if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                        /* To disable date control if mode is edit */
                        //$('#' + Clinical_HospitalizationHx.params.PanelID + " #dtHospitalizationHxDate").addClass("disableAll");
                        /* To disable date control if mode is edit */
                    }
                    if ($('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #chkHospitalizationHxUnremarkable").prop("checked") == false) {
                        Hospitalizationhx_detail.HospitalizationHxUnremarkable = 'false';
                    }
                    if (Hospitalizationhx_detail.HospitalizationHxUnremarkable != null && typeof Hospitalizationhx_detail.HospitalizationHxUnremarkable !== 'undefined') {
                        if (Hospitalizationhx_detail.HospitalizationHxUnremarkable.toLowerCase() != "true") {
                            var self = null;
                            var HospitalizationHx_Child_Detail = null;
                            if (HospitalizationHxType != null && HospitalizationHxType.toLowerCase() == "disease") {
                                if (isDiseaseFill == true) {
                                    if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                        var cachedJSON = Clinical_HospitalizationHx.getCacheHospitalizationHxJSON(diseaseId);
                                        if (cachedJSON != '') {
                                            Clinical_HospitalizationHx.bindCurrentTabJSON(HospitalizationHxType.toLowerCase(), cachedJSON, "#frmClinicalHospitalizationHx", "#ulHospitalizationDisease");
                                        }
                                        else {
                                            Clinical_HospitalizationHx.bindCurrentTabJSON(HospitalizationHxType.toLowerCase(), response.DiseaseFill_JSON, "#frmClinicalHospitalizationHx", "#ulHospitalizationDisease");
                                        }
                                    }
                                    else {
                                        Clinical_HospitalizationHx.bindCurrentTabJSON(HospitalizationHxType.toLowerCase(), response.DiseaseFill_JSON, "#frmClinicalHospitalizationHx", "#ulHospitalizationDisease");
                                    }
                                }
                                    //Start/25-1-2016/Abid Ali/ Disable section detail on form load
                                else {
                                    $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #sectionHospitalizationDetails").addClass('disableAll');
                                    if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null) {
                                        $.each(Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList, function (i, item) {
                                            Clinical_HospitalizationHx.bindCurrentTabJSON(HospitalizationHxType.toLowerCase(), item.JSON, "#frmClinicalHospitalizationHx", "#ulHospitalizationDisease")
                                        });
                                    }
                                }
                                //End/25-1-2016/Abid Ali/ Disable section detail on form load
                            }
                            else {
                                $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #sectionHospitalizationDetails").addClass('disableAll');
                                self = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx");
                            }
                        }
                        else {
                            var chkUnremarkable = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #chkHospitalizationHxUnremarkable")
                            if (isDiseaseFill != true) {
                                Clinical_HospitalizationHx.unRemarkableHospitalizationHx(chkUnremarkable, false);
                            }
                        }
                    }
                    //Serializing the form
                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').data('serialize', $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').serialize());
                    //Serializing the form
                    Clinical_HospitalizationHx.enableDischargeDate();

                    //if (isDiseaseFill == true && Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    //    Clinical_HospitalizationHx.hospitalizationHxJSON = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName();
                    //}
                    if ($('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #txtHospitalizationCPTCode").val() == '') {
                        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").val('');
                        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").prop("disabled", true);
                    } else {
                        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").prop("disabled", false);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {

            }
            BackgroundLoaderShow(false);
        });
    },
    cacheHospitalizationHxJSON: function (diseaseId, diseaseData, diseaseLi) {
        var dfd = $.Deferred();
        var diseaseIndex = -1;

        var patientId;

        if (Clinical_HospitalizationHx.params.patientID == null) {
            patientId = $('#PatientProfile #hfPatientId').val();
        } else {
            patientId = Clinical_HospitalizationHx.params.patientID;
        }
        var noteId = Clinical_ProgressNote.params.NotesId;

        if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx == null) {
            var hospitalizationHxData = {
                HospitalizationHxId: $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxId").val(),
                HospitalizationHxType: 'Disease',
                HospitalizationHxDate: $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #dtHospitalizationHxDate").val(),
                HospitalizationHxUnremarkable: $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #chkHospitalizationHxUnremarkable").prop("checked"),
                HospitalizationHxComments: $('#' + Clinical_HospitalizationHx.params.PanelID + " #txtHospitalizationOverallComments").val(),
                PatientId: patientId,
                NotesId: noteId,
                HospitalizationDiseaseList: []
            }
            Clinical_HistorySummary.HistoryCacheList.HospitalizationHx = hospitalizationHxData;
        }
        else {
            Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxId").val();
            Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxDate = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #dtHospitalizationHxDate").val();
            Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxUnremarkable = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #chkHospitalizationHxUnremarkable").prop("checked");
            Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxComments = $('#' + Clinical_HospitalizationHx.params.PanelID + " #txtHospitalizationOverallComments").val();
        }

        var procedureValid = Clinical_HospitalizationHx.validateProcedure();

        if (procedureValid == true) {
            if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null && diseaseId != null) {
                $.grep(Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList, function (item, index) {
                    if (item.DiseaseId == diseaseId) {
                        diseaseIndex = index;
                        return;
                    }
                });

                var diseaseObj = JSON.parse(diseaseData);

                if (diseaseIndex != -1) {
                    var selectedDisease = $('#' + Clinical_HospitalizationHx.params.PanelID + " div#HospitalizationHxDisease ul#ulHospitalizationDisease li#" + diseaseId);
                    $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #hfSelectedDisease").val(selectedDisease.attr("id"));

                    FreeText = selectedDisease.attr("FreeText") || selectedDisease.text();

                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].HospitalizationHxId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #hfHospitalizationHxId").val();
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].DiseaseId = diseaseId,
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].Disease_text = selectedDisease.text(),
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].FreeTextICD = FreeText,
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].ICD9Code = selectedDisease.attr("icd9code"),
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].ICD9CodeDescription = FreeText == '' ? null : selectedDisease.attr("icd9desc"),
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].ICD10Code = selectedDisease.attr("icd10code"),
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].ICD10CodeDescription = selectedDisease.attr("icd10desc"),
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].SNOMEDID = selectedDisease.attr("snomedcode"),
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].SNOMEDDescription = selectedDisease.attr("snomeddesc"),
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].CPTCode = diseaseObj.CPTCode;
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].CPTCodeId = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTCode').val();
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].CPTDescription = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTDescription').val();
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].CPTSNOMEDCodeId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfCPTSNOMEDCode").val();
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].CPTSNOMEDDescription = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfCPTSNOMEDDescription").val();
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].HospitalizationDiseaseComments = diseaseObj.HospitalizationDiseaseComments;
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].HospitalizationDiseaseDischargeDate = diseaseObj.HospitalizationDiseaseDischargeDate;
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].HospitalizationDiseaseStatus = diseaseObj.HospitalizationDiseaseStatus;
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].HospitalizationDiseaseStatus_RefValue = diseaseObj.HospitalizationDiseaseStatus_RefValue;
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].HospitalizationDiseaseStatus_text = diseaseObj.HospitalizationDiseaseStatus_text;
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].HospitalizationDiseaseStatusText = $('#' + Clinical_HospitalizationHx.params.PanelID + " section#sectionHospitalizationDetails #ddlHospitalizationStatus option:selected").text();
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].HospitalizationDiseaseStayText = $('#' + Clinical_HospitalizationHx.params.PanelID + " section#sectionHospitalizationDetails #ddlHospitalizationDiseaseStayId option:selected").text();
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].HospitalizationDiseaseAdmissionDate = diseaseObj.HospitalizationDiseaseAdmissionDate;
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].HospitalizationDiseaseHospital = diseaseObj.HospitalizationDiseaseHospital;
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList[diseaseIndex].JSON = diseaseData;
                }
                else {
                    var selectedDisease = $('#' + Clinical_HospitalizationHx.params.PanelID + " div#HospitalizationHxDisease ul#ulHospitalizationDisease li#" + diseaseId);
                    $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #hfSelectedDisease").val(selectedDisease.attr("id"));

                    var FreeTextAttr = selectedDisease.attr("FreeText");
                    var FreeText = '';
                    if (typeof FreeTextAttr !== typeof undefined) {
                        FreeText = selectedDisease.text();
                    }

                    var jsonData = {
                        DiseaseId: diseaseId,
                        DiseaseLi: diseaseLi,
                        Disease_text: selectedDisease.text(),
                        FreeTextICD: FreeText,
                        ICD9Code: selectedDisease.attr("icd9code"),
                        ICD9CodeDescription: FreeText == '' ? selectedDisease.attr("icd9desc") : FreeText,
                        ICD10Code: selectedDisease.attr("icd10code"),
                        ICD10CodeDescription: selectedDisease.attr("icd10desc"),
                        SNOMEDID: selectedDisease.attr("snomedcode"),
                        SNOMEDDescription: selectedDisease.attr("snomeddesc"),
                        HospitalizationHxId: $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #hfHospitalizationHxId").val(),
                        CPTCode: diseaseObj.CPTCode,
                        CPTCodeId: $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTCode').val(),
                        CPTDescription: $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTDescription').val(),
                        CPTSNOMEDCodeId: $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTSNOMEDCode').val(),
                        CPTSNOMEDDescription: $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfCPTSNOMEDDescription").val(),
                        HospitalizationDiseaseComments: diseaseObj.HospitalizationDiseaseComments,
                        HospitalizationDiseaseDischargeDate: diseaseObj.HospitalizationDiseaseDischargeDate,
                        HospitalizationDiseaseStatus: diseaseObj.HospitalizationDiseaseStatus,
                        HospitalizationDiseaseStatus_RefValue: diseaseObj.HospitalizationDiseaseStatus_RefValue,
                        HospitalizationDiseaseStatus_text: diseaseObj.HospitalizationDiseaseStatus_text,
                        HospitalizationDiseaseStatusText: $('#' + Clinical_HospitalizationHx.params.PanelID + " section#sectionHospitalizationDetails #ddlHospitalizationStatus option:selected").text(),
                        HospitalizationDiseaseStayText: $('#' + Clinical_HospitalizationHx.params.PanelID + " section#sectionHospitalizationDetails #ddlHospitalizationDiseaseStayId option:selected").text(),
                        HospitalizationDiseaseHospital: diseaseObj.HospitalizationDiseaseHospital,
                        HospitalizationDiseaseAdmissionDate: diseaseObj.HospitalizationDiseaseAdmissionDate,
                        JSON: diseaseData
                    };

                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList.push(jsonData);
                }
            }
        }

        dfd.resolve();
        return dfd;
    },

    getCacheHospitalizationHxJSON: function (diseaseId) {
        if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null && Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList.length > 0) {
            var hospitalizationDisease = $.grep(Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList, function (item, index) {
                if (item.DiseaseId == diseaseId) {
                    return item;
                }
            });

            if (hospitalizationDisease.length > 0) {

                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTCode').val(hospitalizationDisease[0].CPTCodeId);
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTDescription').val(hospitalizationDisease[0].CPTDescription);
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #hfCPTSNOMEDCode').val(hospitalizationDisease[0].CPTSNOMEDCodeId);
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #hfCPTSNOMEDDescription').val(hospitalizationDisease[0].CPTSNOMEDDescription);

                return hospitalizationDisease[0].JSON;
            }
        }

        return '';
    },
    //Author : Farooq Ahmad
    //Date: 20-01-2016
    //This function will bind ICD9 Code Auto Complete
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_HospitalizationHx", null, false);

    },

    //Author : Farooq Ahmad
    //Date   : 22-01-2016
    //This function will bind ICD Code Auto Complete
    bindAutoCompleteICD: function (element) {


        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', '#hfICDCode,#hfICDDescription', '', true, '', 'ICD', true, 'Clinical_HospitalizationHx', 'this', true);
    },

    //Author : Farooq Ahmad
    //Date: 20-01-2016
    //This function will bind IMO Code Auto Complete
    bindAutoComplete: function (element) {


        var hiddenCrtl = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #txtHospitalizationCPTCode');
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_HospitalizationHx", '#txtHospitalizationCPTCode', true);

    },

    bindFreeText: function () {
        var freeText = $.trim($('#txtDiseaseFreeText').val());
        if (freeText.length > 0) {
            var currId = -1;
            $("#pnlClinicalHospitalizationHx #frmClinicalHospitalizationHx #HospitalizationHxDisease ul#ulHospitalizationDisease li[id*='-']").each(function (i, item) {
                currId = $(this).attr("id");
            });
            currId = parseInt(currId) + (-1);

            var li = "<li  id=" + currId + " onclick='Clinical_HospitalizationHx.fillHospitalizationHxDisease(this, event);' onmouseover='Clinical_HospitalizationHx.showIcon(this);' onmouseout='Clinical_HospitalizationHx.hideIcon(this);' icd9Desc=\"" + freeText + "\" freeText=\"" + freeText + "\"><a href='#'>" + freeText + "<span class='removeIconListHover' onclick='Clinical_HospitalizationHx.deleteHospitalizationHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

            var IsAlreadyExist = false;
            $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease li').each(function () {
                if ($(this).attr('icd9Desc').toLowerCase() == freeText.toLowerCase()) {
                    IsAlreadyExist = true;
                }
            });
            if (!IsAlreadyExist) {
                $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease').append(li);
                $(li).trigger('click');

                var diseaseId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease > li.active").attr('id');
                var disease = $(li).get(0).outerHTML;
                var diseaseData = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName();
                Clinical_HospitalizationHx.cacheHospitalizationHxJSON(diseaseId, diseaseData, disease);

            }
            else {
                utility.DisplayMessages('Diagnosis already added', 2);
            }
        }
        $('#pnlClinicalHospitalizationHx #txtDiseaseFreeText').val('');
    },

    SearchTypeChange: function () {
        var IsFreeText = false;
        var RadioButtonVal = false;
        var panel = "";
        if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
            panel = "#pnlClinicalProgressNote #pnlClinicalHospitalizationHx";
        }
        else {
            panel = "#pnlClinicalHospitalizationHx";
        }
        $('input[name=HospitalizationHxSearchType]:checked', panel).val() == 1 ? IsFreeText = true : IsFreeText = false;
        if (IsFreeText) {
            $(panel + " #diseaseAutoComplete").hide();
            if ($(panel + " #diseaseFreeText").hasClass("hidden")) {
                $(panel + " #diseaseFreeText").removeClass("hidden");
            }
        }
        else {
            $(panel + " #diseaseAutoComplete").show();
            if (!$(panel + " #diseaseFreeText").hasClass("hidden")) {
                $(panel + " #diseaseFreeText").addClass("hidden");
            }
        }
        Clinical_HospitalizationHx.SaveFreeTextStatus();
    },

    SaveFreeTextStatus: function () {
        if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
            panel = "#pnlClinicalProgressNote #pnlClinicalHospitalizationHx";
        }
        else {
            panel = "#pnlClinicalHospitalizationHx";
        }
        var IsFreeText = false;
        $('input[name=HospitalizationHxSearchType]:checked', panel).val() == 1 ? IsFreeText = true : IsFreeText = false;
        EMRUtility.insertUpdateFreeTextStatus("Clinical_HospitalizationHx", IsFreeText);
    },


    //Author : Farooq Ahmad
    //Date: 20-01-2016
    //This function will open CPT Code dialog
    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_HospitalizationHx";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtHospitalizationCPTCode";
        LoadActionPan('Admin_IMOCPT', params);
    },
    /*
   Author: Farooq Ahmad
   Date: 21/01/2016
   Overview: This function will show cross button on hover li
   */
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },
    /*
    Author: Farooq Ahmad
    Date: 12/01/2016
    Overview: This function will hide cross button on hover li
    */
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },


    //Author:Farooq Ahmad
    //Date: 22-01-2016
    //This function will handle fill of HospitalizationHx's childs Tabs as specified by TabType
    bindCurrentTabJSON: function (TabType, currentTabJSON, CurrentTabContainerId, ulTabStatusId) {
        var alcoholhx_detail = JSON.parse(currentTabJSON);
        self = $('#' + Clinical_HospitalizationHx.params.PanelID + " " + CurrentTabContainerId);

        utility.bindMyJSONByName(true, alcoholhx_detail, false, self).done(function () {

            //Start//11/02/2016//Ahmad Raza// fixed EMR Bug#309
            var admissionDate = self.find('input[name*="HospitalizationDiseaseAdmissionDate"]');

            if (admissionDate.val() == "") {
                //Commented by Talha Tanweer to fix EMR 1562
                //   admissionDate.datepicker('setDate', new Date());
            } else {

                var date_format = 'mm/dd/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];
                admissionDate.datepicker({ date_format: date_format.replace('yy', '') }).val(admissionDate.val());
                admissionDate.datepicker("setDate", admissionDate.val());

            }

            var dischargeDate = self.find('input[name*="HospitalizationDiseaseDischargeDate"]');

            if (dischargeDate.val() == "") {
                //Commented by Talha Tanweer to fix EMR 1562
                // dischargeDate.datepicker('setDate', new Date());
            } else {

                var date_format = 'mm/dd/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];
                dischargeDate.datepicker({ date_format: date_format.replace('yy', '') }).val(dischargeDate.val());
                dischargeDate.datepicker("setDate", dischargeDate.val());

            }

            //End//11/02/2016//Ahmad Raza// fixed EMR Bug#309

            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #hfCPTCode").val(alcoholhx_detail.CPTCodeId || alcoholhx_detail.CPTCode);
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #hfCPTDescription").val(alcoholhx_detail.CPTDescription);

            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #hfCPTSNOMEDCode').val(alcoholhx_detail.CPTSNOMEDCodeId);
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #hfCPTSNOMEDDescription').val(alcoholhx_detail.CPTSNOMEDDescription);

            BackgroundLoaderShow(false);
        });

    },

    //Author:Abid Ali
    //Date: 10-02-2016
    //This function will handle fill of HospitalizationHx's main Tabs as specified by self context
    fillHospitalizationHxMainTable: function (Hospitalizationhx_detail, self) {
        //Populate main hospitalizationHx Table
        utility.bindMyJSONByName(true, Hospitalizationhx_detail, false, self).done(function () {


            //Start//02/02/2016//Ahmad Raza// changed the implementation way of setDate on datepicker for Bug # EMR-225

            var upperDate = self.find('input[name*="HospitalizationHxDate"]');

            if (upperDate.val() == "") {
                upperDate.datepicker('setDate', new Date());
            } else {

                var date_format = 'mm/dd/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];
                upperDate.datepicker({ date_format: date_format.replace('yy', '') }).val(upperDate.val());
                upperDate.datepicker("setDate", upperDate.val());

            }

            Clinical_HospitalizationHx.hospitalizationDate = $('#' + Clinical_HospitalizationHx.params.PanelID + " #dtHospitalizationHxDate").val();
            if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null) {
                $('#' + Clinical_HospitalizationHx.params.PanelID + " #dtHospitalizationHxDate").val(Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxDate);
            }

            //HospitalizationHxId: $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxId").val(),
            //HospitalizationHxType: 'Disease',
            //HospitalizationHxDate: $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #dtHospitalizationHxDate").val(),
            //HospitalizationHxUnremarkable: $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #chkHospitalizationHxUnremarkable").prop("checked"),
            //HospitalizationHxComments: $('#' + Clinical_HospitalizationHx.params.PanelID + " #txtHospitalizationOverallComments").val(),

            //End//02/02/2016//Ahmad Raza// changed the implementation way of setDate on datepicker for Bug # EMR-225
            if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null && Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxComments != '') {
                $('#' + Clinical_HospitalizationHx.params.PanelID + " #txtHospitalizationOverallComments").val(Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxComments);
            }
            else {
                $('#' + Clinical_HospitalizationHx.params.PanelID + " #txtHospitalizationOverallComments").val(Hospitalizationhx_detail.HospitalizationHxComments);
                Clinical_HospitalizationHx.overallComments = $('#' + Clinical_HospitalizationHx.params.PanelID + " #txtHospitalizationOverallComments").val();
            }

            $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxId").val(Hospitalizationhx_detail.HospitalizationHxId);
            //  $('#' + Clinical_HospitalizationHx.params.PanelID + " #dtHospitalizationHxDate").val(Hospitalizationhx_detail.HospitalizationHxDate);

            Clinical_HospitalizationHx.unremarkable = Hospitalizationhx_detail.HospitalizationHxUnremarkable == "False" || Hospitalizationhx_detail.HospitalizationHxUnremarkable == null ? false : true;

            if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null) {
                $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #chkHospitalizationHxUnremarkable").prop("checked", Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxUnremarkable);
            }
        });
    },

    //Author: Abid Ali
    //Date: 21-01-2016
    //This function will handle unremarkable feature for Hospitalization Hx
    unRemarkableHospitalizationHx: function (obj, isDiseaseLoad) {
        var isRemarkable = $(obj).prop("checked");
        if (isDiseaseLoad) {
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #HospitalizationHxDisease").removeClass('disableAll');
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnHospitalizationHxSave').hide();
        }
        if (isRemarkable == true) {

            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #sectionHospitalizationDetails").resetAllControls(null);
            if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnHospitalizationHxSave').hide();
            }
            else {
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnHospitalizationHxSave').show();
            }
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #HospitalizationHxDisease").addClass('disableAll');
            $('#' + Clinical_HospitalizationHx.params.PanelID + " div#HospitalizationHxDisease ul#ulHospitalizationDisease").empty();
            if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null) {
                Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList = [];
            }
        }
        else {
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #HospitalizationHxDisease").removeClass('disableAll');
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnHospitalizationHxSave').hide();
        }

    },

    //Author: Abid Ali
    //Date: 21-01-2016
    //This function will clear value of given control as specified by obj
    resetControlValue: function (obj) {
        var currentElementTagName = obj.tagName != null ? obj.tagName : obj.prop("tagName");
        if ($(obj).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea')
            $(obj).val('');
        if ($(obj).attr('type') == 'checkbox' || $(obj).attr('type') == 'radio') {

            if ($(obj).attr('type') == 'radio') {
                obj.checked = false;
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Hospitalization History Clinical Module -> Fields should be blank when select other status
                var groupRadBtn = $("input[name='" + $(obj).attr('name') + "']");
                if (groupRadBtn.length > 1) {
                    $.each(groupRadBtn, function (i, item) {
                        if ($(item).attr("id").toLowerCase().indexOf("no") > -1) {
                            $(item).trigger("click");
                        }
                    });
                }
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Hospitalization History Clinical Module -> Fields should be blank when select other status
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

    //Author: Farooq Ahmad
    //Date: 21-01-2016
    //This function will check if details has any value for selected status

    isDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";

        var self = $(TabType).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
            if ($(this).prop("disabled") != true && DetailExists == false) {
                var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea')) {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'checkbox' && this.checked == true) {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'radio' && $(this).attr('id').toLowerCase().indexOf("no") > -1 && this.checked == true) {
                    DetailExists = false;
                }
                if (currentElementTagName.toLowerCase() == 'select' && $(this).val() != null && $(this).val() != "") {
                    DetailExists = true;
                }

                //if (currentElementTagName.toLowerCase() == 'ul') {
                //    $(this).find('li.active').removeClass('active');
                //}
            }
        });

        //if (Clinical_HospitalizationHx.params.PanelID != "pnlClinicalHistorySummary #pnlClinicalHospitalizationHx") {
        //    var hopitalName = $(TabType).find('#txtHospitalizationHospitalName');
        //    if ($(hopitalName).val() == null || $(hopitalName).val() == "") {
        //        $(hopitalName).focus();
        //        DetailExists = false;
        //    }
        //}

        //var hopitalName = $(TabType).find('#txtHospitalizationHospitalName');
        //if ($(hopitalName).val() == null || $(hopitalName).val() == "") {
        //    $(hopitalName).focus();
        //    DetailExists = false;
        //}
        return DetailExists;


    },

    //Author: Abid Ali
    //Date: 21-01-2016
    //This function will handle Add/Edit of HospitalizationHx and it's childs, it expects HospitalizationHxType to be Add/Edit
    HospitalizationHxSave: function (HospitalizationHxType, UnloadHospitalizationhx, attachToNote) {

        //Start//17-02-2016//Ahmad Raza//Fixed Bug#341
        if (!$('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #chkHospitalizationHxUnremarkable").is(':checked')) {
            //$("#" + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx").bootstrapValidator('revalidateField', 'CPT');
            // $("#" + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx").bootstrapValidator('revalidateField', 'HospitalizationDiseaseHospital');
        }
        //if (($("#" + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #txtHospitalizationCPTCode").val() != "" && $("#" + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #txtHospitalizationHospitalName").val() != "") || $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #chkHospitalizationHxUnremarkable").is(':checked')) {
        //End//17-02-2016//Ahmad Raza//Fixed Bug#341
        if (UnloadHospitalizationhx != null && UnloadHospitalizationhx == true) {
            Clinical_HospitalizationHx.bNextPrev = false;
        }

        var HospitalizationHxId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxId").val() != "" ? $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxId").val() : "-1";
        if (parseInt(HospitalizationHxId) > 0) {
            Clinical_HospitalizationHx.params.mode = "Edit";
        }
        else {
            Clinical_HospitalizationHx.params.mode = "Add";
        }
        var selectedItem = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ulHospitalizationDisease li.active").text();
        //Start//11/02/2016//Abid Ali// fixed bug#315
        var overallComments = "";
        overallComments = $('#' + Clinical_HospitalizationHx.params.PanelID + " #txtHospitalizationOverallComments").val();
        overallComments = typeof overallComments == "undefined" ? "" : overallComments
        if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote" && overallComments != "") {

            var DetailExists = true;
        }
        else {


            DetailExists = Clinical_HospitalizationHx.isDetailExists('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #sectionHospitalizationDetails");
        }
        if ($('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #chkHospitalizationHxUnremarkable").is(':checked')) {
            DetailExists = true;
        }
        if (overallComments != "") {
            DetailExists = true;
        }
        //End//11/02/2016//Abid Ali// fixed bug#315

        if (DetailExists == true) {
            var strMessage = "";
            var self = null;
            if (HospitalizationHxType.toLowerCase() == "disease") {
                self = $('#' + Clinical_HospitalizationHx.params.PanelID + " section#sectionHospitalizationDetails");
            }
            else {
                $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #sectionHospitalizationDetails").addClass('disableAll');
            }
            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);
            //Data For Hospitalization Disease table
            //start 25/01/2016 Farooq Ahmad if the type is disease then post the data of disease
            if (HospitalizationHxType.toLowerCase() == "disease") {
                var selectedDisease = $('#' + Clinical_HospitalizationHx.params.PanelID + " div#HospitalizationHxDisease ul#ulHospitalizationDisease li.active");
                $('#' + Clinical_HospitalizationHx.params.PanelID + "  #hfSelectedDisease").val(selectedDisease.attr("id"));

                objData["DiseaseId"] = selectedDisease.attr("id");
                objData["Disease_text"] = selectedDisease.text();

                //-------------------
                var freeText = selectedDisease.attr("freeText");
                if (typeof freeText !== typeof undefined) {
                    objData["FreeTextICD"] = freeText;
                }
                else {
                    //objData["FreeTextICD"] = selectedDisease.text();
                    objData["ICD9Code"] = selectedDisease.attr("icd9code");
                    objData["ICD9CodeDescription"] = selectedDisease.attr("icd9desc");
                    objData["ICD10Code"] = selectedDisease.attr("icd10code");
                    objData["ICD10CodeDescription"] = selectedDisease.attr("icd10desc");
                    objData["SNOMEDID"] = selectedDisease.attr("snomedcode");
                    objData["SNOMEDDescription"] = selectedDisease.attr("snomeddesc");
                }

                objData["HospitalizationDiseaseStatusText"] = $('#' + Clinical_HospitalizationHx.params.PanelID + " section#sectionHospitalizationDetails #ddlHospitalizationStatus option:selected").text();
                objData["HospitalizationDiseaseStayText"] = $('#' + Clinical_HospitalizationHx.params.PanelID + " section#sectionHospitalizationDetails #ddlHospitalizationDiseaseStayId option:selected").text();

                //-------------------
            }
            //End 25/01/2016 Farooq Ahmad if the type is disease then post the data of disease
            // Data For Hospitalization main table
            objData["HospitalizationHxId"] = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxId").val();
            objData["HospitalizationHxType"] = HospitalizationHxType != null ? HospitalizationHxType : "";
            objData["HospitalizationHxDate"] = $('#' + Clinical_HospitalizationHx.params.PanelID + " #dtHospitalizationHxDate").val();
            objData["HospitalizationHxUnremarkable"] = $('#' + Clinical_HospitalizationHx.params.PanelID + " #chkHospitalizationHxUnremarkable").prop("checked");
            objData["HospitalizationHxComments"] = $('#' + Clinical_HospitalizationHx.params.PanelID + " #txtHospitalizationOverallComments").val();
            objData["CPTCodeId"] = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfCPTCode").val();
            objData["CPTSNOMEDID"] = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfCPTSNOMEDCode").val();
            objData["CPTSNOMEDDescription"] = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfCPTSNOMEDDescription").val();

            myJSON = JSON.stringify(objData);

            if (Clinical_HospitalizationHx.params.mode == "Add") {
                AppPrivileges.GetFormPrivileges("History_Hospitalization Hx", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        var result;
                        if (HospitalizationHxType.toLowerCase() == "disease")
                            result = Clinical_HospitalizationHx.validateProcedure();
                        else
                            result = true;
                        if (result != false) {
                            Clinical_HospitalizationHx.saveHospitalizationHx(myJSON).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_HospitalizationHx.SaveFavToggelStatus($('#' + Clinical_HospitalizationHx.params.PanelID + ' #ddlFavoriteListHospitalizationHx').val());
                                    Clinical_HospitalizationHx.params.HxTypeId = response.HospitalizationHxId;
                                    $("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #divSwitch").removeClass('disableAll');
                                    if (!attachToNote) {
                                        Clinical_HospitalizationHx.BindCurrentHospitalizationSoapText(response, true);
                                        Clinical_HospitalizationHx.ChangeCurrentPast(1, null, null, null);
                                    }

                                    if (response.diseaseId != "") {
                                        var diseaseResponse = JSON.parse(response.diseaseId);

                                        if (diseaseResponse.diseaseId > 0) {
                                            $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfSelectedDisease").val(diseaseResponse.diseaseId);
                                            $('#' + Clinical_HospitalizationHx.params.PanelID + " div#HospitalizationHxDisease ul#ulHospitalizationDisease li.active").attr('id', diseaseResponse.diseaseId);
                                            Clinical_HospitalizationHx.currentSelectedDisease["DiseaseId"] = diseaseResponse.diseaseId;
                                        }
                                    }
                                    //Clinical_HospitalizationHx.loadHospitalizationHx();

                                    Clinical_HospitalizationHx.params.mode = "Edit";
                                    if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote" && UnloadHospitalizationhx == true) {
                                        if (!attachToNote) {
                                            Clinical_HospitalizationHx.getHospitalizationHxInfo(HospitalizationHxType, UnloadHospitalizationhx, null, true);
                                        }
                                    }
                                    utility.DisplayMessages(response.message, 1);
                                    $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxId").val(response.HospitalizationHxId);
                                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').data('serialize', $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').serialize());
                                    //
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });

            }
            else if (Clinical_HospitalizationHx.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("History_Hospitalization Hx", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        var result;
                        if (selectedItem != "") {
                            if (HospitalizationHxType.toLowerCase() == "disease")

                                result = Clinical_HospitalizationHx.validateProcedure();
                            else
                                result = true;
                        }
                        if (result != false) {
                            Clinical_HospitalizationHx.updateHospitalizationHx(myJSON, Clinical_HospitalizationHx.params.HospitalizationHxId).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_HospitalizationHx.SaveFavToggelStatus($('#' + Clinical_HospitalizationHx.params.PanelID + ' #ddlFavoriteListHospitalizationHx').val());
                                    $("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #divSwitch").removeClass('disableAll');
                                    if (!attachToNote) {
                                        Clinical_HospitalizationHx.ChangeCurrentPast(1, null, null, null);
                                        Clinical_HospitalizationHx.BindCurrentHospitalizationSoapText(response, true);
                                    }
                                    if (response.diseaseId != "") {
                                        var diseaseResponse = JSON.parse(response.diseaseId);
                                        if (diseaseResponse.diseaseId > 0) {
                                            $('#' + Clinical_HospitalizationHx.params.PanelID + " div#HospitalizationHxDisease ul#ulHospitalizationDisease li.active").attr('id', diseaseResponse.diseaseId);
                                            $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfSelectedDisease").val(diseaseResponse.diseaseId);
                                            Clinical_HospitalizationHx.currentSelectedDisease["DiseaseId"] = diseaseResponse.diseaseId;
                                        }
                                    }
                                    //Clinical_HospitalizationHx.loadHospitalizationHxComponent(HospitalizationHxType);
                                    if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote" && UnloadHospitalizationhx == true) {
                                        if (!attachToNote) {
                                            Clinical_HospitalizationHx.getHospitalizationHxInfo(HospitalizationHxType, UnloadHospitalizationhx);
                                        }

                                    } else {
                                        //Clinical_HospitalizationHx.AppointmentStatusSearch(Clinical_HospitalizationHx.params.HospitalizationHxignsId);
                                        utility.DisplayMessages(response.message, 1);
                                    }

                                    // $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxId").val(response.HospitalizationHxId);
                                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').data('serialize', $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').serialize());

                                }
                                else {
                                    utility.DisplayMessages(response.message, 3);
                                }
                            });
                        }
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });

            }
        }
        else {

            utility.DisplayMessages("Please enter any value", 3);

        }
        //  }
    },

    //Author: Abid Ali
    //Date: 21-01-2016
    //This function will handle load of HospitalizationHx and it's childs as specified by HospitalizationHxType
    //It represents service call to API
    fillHospitalizationHx: function (HospitalizationHxType, DiseaseId, hospitalizationHxId) {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["HospitalizationHxType"] = HospitalizationHxType != null ? HospitalizationHxType : "disease";
        objData["commandType"] = "FILL_HospitalizationHx";
        objData["DiseaseId"] = DiseaseId != null ? DiseaseId : 0;
        objData["HospitalizationHxId"] = hospitalizationHxId != null ? hospitalizationHxId : 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "HospitalizationHx");
    },

    /*
 Author: Farooq Ahmad
 Date: 20/01/2016
 Overview: This function delete the selected li from disease
 */
    deleteHospitalizationHxDisease: function (obj, ev) {
        //Start/25-1-2016/Abid Ali/ init deffered object
        var dfd = new $.Deferred();
        //End/25-1-2016/Abid Ali/ init deffered object
        ev.stopPropagation();
        var diseaseId = $(obj).attr('id');
        if (diseaseId < 0) {
            $(obj).remove();
            Clinical_HospitalizationHx.resetValues();

            //if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
            //    Clinical_HospitalizationHx.deleteCacheDisease(diseaseId);
            //}

            //reset form validation
            $("#" + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx").bootstrapValidator('resetForm', true);
            //Start/25-1-2016/Abid Ali/ resolve deffered object
            dfd.resolve();
            //End/25-1-2016/Abid Ali/ resolve deffered object
        } else {
            AppPrivileges.GetFormPrivileges("History_Hospitalization Hx", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('23', function () {
                        var selectedValue = diseaseId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Clinical_HospitalizationHx.hospitalizationHxDiseaseDelete(selectedValue).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #sectionHospitalizationDetails').resetAllControls(null);
                                    $(obj).remove();
                                    //Start/25-1-2016/Abid Ali/ resolve deffered object
                                    dfd.resolve();
                                    //End/25-1-2016/Abid Ali/ resolve deffered object

                                    //if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    //    Clinical_HospitalizationHx.deleteCacheDisease(diseaseId);
                                    //}

                                    utility.DisplayMessages(response.Message, 1);
                                    //reset form validation
                                    Clinical_HospitalizationHx.loadHospitalizationHx('disease', true);

                                    $("#" + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx").bootstrapValidator('resetForm', true);
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
        }
        dfd.done(function () {
            //Start/25-1-2016/Abid Ali/ Disable section detail when ul is empty
            //var $diseases = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #HospitalizationHxDisease ul#ulHospitalizationDisease");
            // if (!$diseases.has('li').length)
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #sectionHospitalizationDetails").addClass('disableAll');
            //End/25-1-2016/Abid Ali/ Disable section detail when ul is empty
        });

    },

    /*Author: Abid Ali
    Date: 25/01/2016
    Overview: This function call api service of delete
    */
    hospitalizationHxDiseaseDelete: function (diseaseId) {
        var objData = new Object();
        objData["DiseaseId"] = diseaseId;
        objData["HospitalizationHxId"] = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxId").val();
        objData["PatientId"] = Clinical_HospitalizationHx.params.patientID  || $('#PatientProfile #hfPatientId').val()
        objData["commandType"] = "DELETE_HOSPITALIZATIONHXDISEASE";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "HospitalizationHx");
    },
    /*
       Author: Farooq Ahmad
       Date: 20/01/2016
       Overview: This function perform fill action on click disease li
       */
    fillHospitalizationHxDisease: function (obj, ev) {

        ev.stopPropagation();

        if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HospitalizationHx.hospitalizationHxJSON != '') {
            if (Clinical_HospitalizationHx.hospitalizationHxJSON != $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName()) {
                var diseaseId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease > li.active").attr('id');
                var diseaseData = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName();
                Clinical_HospitalizationHx.cacheHospitalizationHxJSON(diseaseId, diseaseData);
            }
        }

        Clinical_HospitalizationHx.currentSelectedDisease["DiseaseId"] = $(obj).attr('id');

        $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnAddVitalsOnNote').prop('disabled', false);
        $("#" + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx").bootstrapValidator('resetForm', true);
        Clinical_HospitalizationHx.validateHospitalizationHx();
        var diseaseId = $(obj).attr('id');
        if ($('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #txtHospitalizationCPTCode").val() == '') {
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").val('');
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").prop("disabled", true);
        } else {
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").prop("disabled", false);
        }
        Clinical_HospitalizationHx.resetValues();
        $('#' + Clinical_HospitalizationHx.params.PanelID + ' #btnHospitalizationHxSave').hide();
        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #sectionHospitalizationDetails").removeClass('disableAll');

        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #HospitalizationHxDisease ul#ulHospitalizationDisease li").each(function (i, item) {
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

        Clinical_HospitalizationHx.loadHospitalizationHxComponent('disease', diseaseId, true, null, false);

    },

    //Farooq Ahmad
    // Date: 21/01/2016
    loadHospitalizationHxDiseases: function (Crtl, result, StatusType) {
        var currentLiClass = "";
        var currentLiClick = "";
        var ParentDiv = "";
        if (StatusType != null && StatusType.toLowerCase() == "disease") {
            Crtl = '#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx div#HospitalizationHxDisease #ulHospitalizationDisease";
            //currentLiClick = "Clinical_HospitalizationHx.enableDisableTobaccoControls";
            ParentDiv = "HospitalizationHxDisease";
        }
        if ($(Crtl).length > 0)
            l = $(Crtl);

        l.empty();


        var isFirstLi = true;
        //$.each(result, function (j, item) {
        //    if (item.Value != "") {
        //        if (isFirstLi == true) {
        //            currentLiClass = 'class="active"';
        //            isFirstLi = false;
        //        }
        //        else {
        //            currentLiClass = "";
        //        }
        //        var onClick = currentLiClick == "" ? "" : currentLiClick + "(this,'" + String(item.Name) + "');";
        //        //item.Value = item.Value == "" ? 0 : item.Value;
        //        l.append('<li id="' + item.Value + '" ' + currentLiClass + ' onclick="' + onClick + '" value=' + item.Value + ' refValue="' + item.RefValue + '"><a href="#' + ParentDiv + '">' + item.Name + ' </a></li>');
        //    }

        //});
        $.each(result, function (j, item) {
            //item.Value = item.Value == "" ? 0 : item.Value;
            var li = "";
            if (item.FreeTextICD != "") {
                li = "<li  id=\"" + item.DiseaseId + "\" onclick='Clinical_HospitalizationHx.fillHospitalizationHxDisease(this, event);' icd9Desc=\"" + item.FreeTextICD + "\" freeText=\"" + item.FreeTextICD + "\" ><a href='#'>" + item.FreeTextICD + "<span class='removeIconListHover' onclick='Clinical_HospitalizationHx.deleteHospitalizationHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></li>";
            }
            else {
                li = "<li  id=\"" + item.DiseaseId + "\" onclick='Clinical_HospitalizationHx.fillHospitalizationHxDisease(this, event);' onmouseover='Clinical_HospitalizationHx.showIcon(this);' onmouseout='Clinical_HospitalizationHx.hideIcon(this);' icd9Code=\"" + item.ICD9Code + "\" icd9Desc=\"" + item.ICD9CodeDescription + "\" icd10Code=\"" + item.ICD10Code + "\" icd10Desc=\"" + item.ICD10CodeDescription + "\" snomedCode=\"" + item.SNOMEDID + "\" snomedDesc=\"" + item.SNOMEDDescription + "\"><a href='#'>" + item.ICD9CodeDescription + "<span  class='removeIconListHover' onclick='Clinical_HospitalizationHx.deleteHospitalizationHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></li>"
            }
            l.append(li);
        });
        //Start || 15 July, 2016 || Talha Tanweer || trigger first disease detail open  EMR 1562
        //$('#' + Clinical_HospitalizationHx.params.PanelID + ' #ulHospitalizationDisease li:first').trigger('click');
        //$('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").removeClass('disableAll');
        //End   || 15 July, 2016 || Talha Tanweer || trigger first disease detail open  EMR 1562
        if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null && Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList != null) {
            $(Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList).each(function () {
                if ($(this)[0].DiseaseLi != null || $(this)[0].DiseaseLi != undefined) {
                    l.append($(this)[0].DiseaseLi);
                }
            });
        }
        var previouslySelectedDisease = Clinical_HospitalizationHx.currentSelectedDisease["DiseaseId"];
        $(" div#HospitalizationHxDisease ul#ulHospitalizationDisease li#" + previouslySelectedDisease).trigger('click');

    },

    //Author : Farooq Ahmad
    //Date : 21/01/2016
    // This function will reset the hospitalization detail section
    resetValues: function () {
        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #sectionHospitalizationDetails").
                   find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,ul').each(function () {
                       Clinical_HospitalizationHx.resetControlValue(this);
                   });
        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #sectionHospitalizationDetails").find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
            $(this).val('');
        });

    },

    //Author: Abid Ali
    //Date: 21-01-2016
    //This function will handle Add of HospitalizationHx and it's childs
    //It represents service call to API
    saveHospitalizationHx: function (HospitalizationHxData) {
        var objData = JSON.parse(HospitalizationHxData);
        if (Clinical_HospitalizationHx.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_HospitalizationHx.params.patientID;
        }
        objData["commandType"] = "SAVE_HospitalizationHx";

        var data = JSON.stringify(objData);

        //var data = "HospitalizationHxignsData=" + HospitalizationHxignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HospitalizationHx");
    },

    //Author: Abid Ali
    //Date: 21-01-2016
    //This function will handle Edit of HospitalizationHx and it's childs
    //It represents service call to API
    updateHospitalizationHx: function (HospitalizationHxData, HospitalizationHxId) {

        var objData = JSON.parse(HospitalizationHxData);
        if (Clinical_HospitalizationHx.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_HospitalizationHx.params.patientID;
        }
        objData["commandType"] = "UPDATE_HospitalizationHx";


        var data = JSON.stringify(objData);

        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HospitalizationHx");

    },

    ///Author: Abid Ali
    //Date: 21-01-2016
    //Logic to compare txtCPT values with IMO
    validateProcedure: function () {



        if ($('#' + Clinical_HospitalizationHx.params.PanelID + ' #txtHospitalizationCPTCode').val() == "") {
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTCode').val('');
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTDescription').val('');
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #hfCPTSNOMEDCode').val('');
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #hfCPTSNOMEDDescription').val('');
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").val('');
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").prop("disabled", true);
            return true;
        }
        else if ($('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTCode').val() + " - " + $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTDescription').val() != $('#' + Clinical_HospitalizationHx.params.PanelID + ' #txtHospitalizationCPTCode').val()) {
            utility.DisplayMessages("Procedure not Valid", 2);
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #txtHospitalizationCPTCode').val('');
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").val('');
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").prop("disabled", true);
            return false;
        }
        else {
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").prop("disabled", false);
            return true;
        }

        //var cptDescription = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTDescription').val();
        //var cptCode = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #hfCPTCode').val();
        //var $txtCptCode = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #txtHospitalizationCPTCode');
        //if ($txtCptCode.val() == "") {
        //    return true;
        //}
        //if (cptCode == "") {
        //    if (cptDescription != $txtCptCode.val()) {
        //        utility.DisplayMessages("Procedure not Valid", 2);
        //        $txtCptCode.val('');
        //        return false;
        //    }
        //}
        //else if (cptCode + " - " + cptDescription != $txtCptCode.val()) {
        //    utility.DisplayMessages("Procedure not Valid", 2);
        //    $txtCptCode.val('');
        //    return false;
        //}
        //else
        //    return true;
    },

    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will handle Unload of HospitalizationHx

    saveHospitalizationHxOnUnload: function (TabType, attachToNotes) {
        //var hopitalName = $(TabType).find('#txtHospitalizationHospitalName');
        //if ($(hopitalName).val() == null || $(hopitalName).val() == "") {
        //    $(hopitalName).focus();
        //    $("#" + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx").bootstrapValidator('revalidateField', 'HospitalizationDiseaseHospital');
        //    return;
        //}


        var HospitalizationHxType = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxType").val();
        if (HospitalizationHxType == "" || HospitalizationHxType == "undefined") {
            Clinical_HospitalizationHx.UnloadHospitalizationHistory();
            if (Clinical_HospitalizationHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalHospitalizationHx") {
                UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabHospitalizationHx', 'HospitalizationHx');
            }
        }
        else {
            if (!attachToNotes) {
                Clinical_HospitalizationHx.HospitalizationHxSave(HospitalizationHxType, true);
            }
            else {
                Clinical_HospitalizationHx.HospitalizationHxSave(HospitalizationHxType, true, true);
            }

        }
    },
    unLoadTab: function (NextOrPre, controlToInvoke) {
        Clinical_HospitalizationHx.bIsFirstLoad = true;
        //Start//11-02-2016//Ahmad Raza//fixed EMR Bug#314
        var hospitalizationHxType = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxType").val();
        var TabType = "#pnlClinicalHistorySummary #pnlClinicalHospitalizationHx #frmClinicalHospitalizationHx #sectionHospitalizationDetails";
        var detailExists = Clinical_HospitalizationHx.isDetailExists(TabType)//(hospitalizationHxType.toLowerCase());


        if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
            Clinical_HospitalizationHx.controlToInvoke = controlToInvoke;

            var diseaseAdded = $('#' + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease > li").filter(function () {
                return $(this).attr("id") < 0;
            });

            if (EMRUtility.compareFormDataWithSerialized(Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx') || diseaseAdded.length > 0) {
                utility.myConfirmNote('1', function () {
                    Clinical_HospitalizationHx.saveHospitalizationHxOnUnload(TabType);

                }, function () {
                    //var hopitalName = $(TabType).find('#txtHospitalizationHospitalName');
                    //if ($(hopitalName).val() == null || $(hopitalName).val() == "") {
                    //    $(hopitalName).focus();
                    //   // $("#" + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx").bootstrapValidator('revalidateField', 'HospitalizationDiseaseHospital');
                    //    return;
                    //}
                    Clinical_HospitalizationHx.saveHospitalizationHxOnUnload(TabType, true);
                    Clinical_HospitalizationHx.UnloadHospitalizationHistory();
                    if (Clinical_HospitalizationHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalHospitalizationHx") {
                        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                        Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabHospitalizationHx', 'HospitalizationHx');
                    }
                }, function () {
                    Clinical_HospitalizationHx.UnloadHospitalizationHistory();
                    if (Clinical_HospitalizationHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalHospitalizationHx") {
                        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                        Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabHospitalizationHx', 'HospitalizationHx');
                    }
                });
            }
            else {
                Clinical_HospitalizationHx.UnloadHospitalizationHistory();
                if (Clinical_HospitalizationHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalHospitalizationHx") {
                    UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                    Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabHospitalizationHx', 'HospitalizationHx');
                }
            }
            //End//11-02-2016//Ahmad Raza//fixed EMR Bug#314
        } else {
            Clinical_HospitalizationHx.UnloadHospitalizationHistory();
        }
    },

    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will handle Unload of HospitalizationHx
    UnloadHospitalizationHistory: function (NextOrPre) {
        if (Clinical_HospitalizationHx.params["FromAdmin"] == "0") {
            if (Clinical_HospitalizationHx.params != null && Clinical_HospitalizationHx.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_HospitalizationHx.params.ParentCtrl, 'Clinical_HospitalizationHx');
                if (Clinical_HospitalizationHx.controlToInvoke != null) {
                    setTimeout(function () {
                        Clinical_ProgressNote.SelectNotesComponentTab(Clinical_HospitalizationHx.controlToInvoke);
                        Clinical_HospitalizationHx.controlToInvoke = null;
                    }, 400);
                }
            }
            else {
                UnloadActionPan(null, 'Clinical_HospitalizationHx');
                if (Clinical_HospitalizationHx.controlToInvoke != null) {
                    setTimeout(function () {
                        Clinical_ProgressNote.SelectNotesComponentTab(Clinical_HospitalizationHx.controlToInvoke);
                        Clinical_HospitalizationHx.controlToInvoke = null;
                    }, 400);
                }
            }

        }
        else {
            $("#mstrDivHospitalization #clinicalMenu_History_HospitalizationHx").remove();
            RemoveAdminTab();
        }
        EMRUtility.scrollToPNcomponent('clinical_hospitalizationhx');
    },

    ///Author: Abid Ali
    //Date: 21-01-2016
    //Logic to popup search from on procedure field of medicalhx disease
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
        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #txtDisease").attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0"
        if (Clinical_HospitalizationHx.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Clinical_HospitalizationHx';
        }

        else {
            params["ParentCtrl"] = Clinical_HospitalizationHx.params["TabID"];
        }

        params["ParentCtrlPanelID"] = Clinical_HospitalizationHx.params.PanelID;
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Clinical_HospitalizationHx.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params, Clinical_HospitalizationHx.params.PanelID);
        }

    },

    /*
    Author: Abid Ali
    Date: 26/01/2016
    This function will enable Discharge Date when Admission Date has some data
    */
    enableDischargeDate: function () {
        var admissionDate = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #dtpHospitalizationAdmissionDate").val();
        if (admissionDate != "" && typeof admissionDate != "undefined") {
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #dtpHospitalizationDischargeDate").prop("disabled", false);
        }
        else {
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #dtpHospitalizationDischargeDate").prop("disabled", true);
        }

    },

    ChangeCurrentPast: function (obj, PrimaryID, PageNumber, ResultPerPage) {

        if ($(obj).attr('status') == '1' || obj == 1) {
            $(obj).attr('status', 0);
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #pnlCurrent ").addClass("hidden");
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #pnlPast ").removeClass("hidden");
            Clinical_HospitalizationHx.fillhxLog(PrimaryID, PageNumber, ResultPerPage).done(function (response) {
                if (response != "") {
                    var json = JSON.parse(response);
                    Clinical_HospitalizationHx.gridLoad(response);
                    var TableControl = Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #dgvPastHospitalizationHx";
                    var PagingPanelControlID = Clinical_HospitalizationHx.params.PanelID + " #dgvPastHospitalizationHx_Paging";
                    var ClassControlName = "Clinical_HospitalizationHx";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = json.iTotalDisplayRecords;
                    setTimeout(
                        CreatePagination(json.HxLogSoapCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_HospitalizationHx.ChangeCurrentPast(1, PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                }
            });


        } else {
            $(obj).attr('status', 1);

            $('#' + Clinical_HospitalizationHx.params.PanelID + " #pnlPast").addClass("hidden");
            $('#' + Clinical_HospitalizationHx.params.PanelID + " #pnlCurrent  ").removeClass("hidden");
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
        objData["HxId"] = Clinical_HospitalizationHx.params.HxTypeId;
        objData["HxType"] = "HospitalizationHx";
        objData["Status"] = "All";
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = ResultPerPage;
        objData["commandType"] = "get_hx_log";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");
    },


    //-----------------Progress Note-------------

    // Reason: These functions are used for Progress Note Soap Attachment, creation and detachment

    //Call Back function to add component to Progress Note
    addHospitalizationHxToNotes: function () {
        var HospitalizationHxId = Clinical_HospitalizationHx.params.HospitalizationHxId;
        var HospitalizationHxType = $('#' + Clinical_HospitalizationHx.params.PanelID + " #hfHospitalizationHxType").val();
        Clinical_HospitalizationHx.HospitalizationHxSave(HospitalizationHxType, true);
    },

    //this function will get Hospitalization History Soap Text and attach that to Progress note
    getHospitalizationHxInfo: function (HospitalizationHxType, UnloadHospitalizationhx, hospitalizationHxId, hideAlertMessage) {
        Clinical_HospitalizationHx.fillHospitalizationHx(HospitalizationHxType, null, hospitalizationHxId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_HospitalizationHx.createHospitalizationHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadHospitalizationhx, hideAlertMessage);

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //This Function will check, if Hospitalization History Soap is already attached in Progress note, if Hospitalization History is not attached than it will create main divs to attach allergy
    checkHospitalizationHxExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Hospitalizationhx').length == 0) {


            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="HospitalizationHxComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_Hospitalizationhx title="Hospitalization Hx"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'HospitalizationHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="HospitalizationHx">Hospitalization Hx</a> ' +
                      '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'HospitalizationHx\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'HospitalizationHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_Hospitalizationhx> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Hospitalizationhx').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    createHospitalizationHxBodyHTMLFromNotes: function (HospitalizationHistory, NoteHTMLCtrl, UnloadHospitalizationhx, hideAlertMessage) {
        Clinical_HospitalizationHx.checkHospitalizationHxExists();
        if (HospitalizationHistory && HospitalizationHistory.HospitalizationHxId && HospitalizationHistory.HospitalizationHxId > 0) {
            var HospitalizationHxFill_Obj = HospitalizationHistory;
            var $mainDivHospitalizationHx = $(document.createElement('div'));

            var HospitalizationHxId = HospitalizationHxFill_Obj.HospitalizationHxId;

            var $SectionBodyHospitalizationHx = $(document.createElement('section'));
            $SectionBodyHospitalizationHx.attr('id', "Cli_HospitalizationHx_Main" + HospitalizationHxId);
            var $DetailsDiv = $(document.createElement('div'));
            $DetailsDiv.attr('id', "Cli_HospitalizationHx_" + HospitalizationHxId);
            var $ListHospitalizationHx = $(document.createElement('ul'));

            $ListHospitalizationHx.attr('class', 'list-unstyled')

            $SectionBodyHospitalizationHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_HospitalizationHx_" + HospitalizationHxId + '"><i class="fa fa-edit"></i></a>' +
                '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_HospitalizationHx_Main" + HospitalizationHxId + '"  ><i class="fa fa-times"></i></a></div> ');


            $ListHospitalizationHx.append("<li>" + HospitalizationHxFill_Obj.SoapText + "</li>");
            $DetailsDiv.append($ListHospitalizationHx);
            $SectionBodyHospitalizationHx.append($DetailsDiv);
            if ($(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId).length == 0) {
                $mainDivHospitalizationHx.append($SectionBodyHospitalizationHx);

                var HospitalizationHxHtml = $mainDivHospitalizationHx.html();
                if (HospitalizationHxHtml != '') {
                    $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().addClass('initialVisitBody');

                    $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().append(HospitalizationHxHtml);
                }

                return HospitalizationHxId;
            } else {

                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId).html($SectionBodyHospitalizationHx.html());
                $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId + ' ul').append(CommentHTML);

            }


        }
    },


    //This Function is used to create SOAP html and append it to  Progress note
    createHospitalizationHxBodyHTML: function (response, NoteHTMLCtrl, UnloadHospitalizationhx, hideAlertMessage) {
        Clinical_HospitalizationHx.checkHospitalizationHxExists();
        if (response.HospitalizationHxFill_JSON != null && response.HospitalizationHxFill_JSON != '') {
            var HospitalizationHxFill_Obj = JSON.parse(response.HospitalizationHxFill_JSON);
            var $mainDivHospitalizationHx = $(document.createElement('div'));

            var HospitalizationHxId = HospitalizationHxFill_Obj.HospitalizationHxId;
            if (HospitalizationHxId > 0) {
                var $SectionBodyHospitalizationHx = $(document.createElement('section'));
                $SectionBodyHospitalizationHx.attr('id', "Cli_HospitalizationHx_Main" + HospitalizationHxId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_HospitalizationHx_" + HospitalizationHxId);
                var $ListHospitalizationHx = $(document.createElement('ul'));

                $ListHospitalizationHx.attr('class', 'list-unstyled')

                $SectionBodyHospitalizationHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_HospitalizationHx_" + HospitalizationHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_HospitalizationHx_Main" + HospitalizationHxId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListHospitalizationHx.append("<li>" + HospitalizationHxFill_Obj.HospitalizationHxSoapText + "</li>");
                $DetailsDiv.append($ListHospitalizationHx);
                $SectionBodyHospitalizationHx.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId).length == 0) {
                    $mainDivHospitalizationHx.append($SectionBodyHospitalizationHx);
                    Clinical_HospitalizationHx.updateHospitalizationHxHtml($mainDivHospitalizationHx.html(), HospitalizationHxId, NoteHTMLCtrl, hideAlertMessage);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId).html($SectionBodyHospitalizationHx.html());
                    $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText("HospitalizationHx", hideAlertMessage);
                    Clinical_HospitalizationHx.updateHospitalizationHxHtml("", HospitalizationHxId, NoteHTMLCtrl, hideAlertMessage);

                }

                if (UnloadHospitalizationhx == true) {
                    Clinical_HospitalizationHx.UnloadHospitalizationHistory();
                }
            }
        }
    },

    createHospitalizationHxBodyHTMLFromNote: function (response, NoteHTMLCtrl, UnloadHospitalizationhx, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_HospitalizationHx.checkHospitalizationHxExists();
        if (response) {
            var HospitalizationHxFill_Obj = response;
            var $mainDivHospitalizationHx = $(document.createElement('div'));

            var HospitalizationHxId = HospitalizationHxFill_Obj.HospitalizationHxId;

            if (HospitalizationHxId > 0) {
                var $SectionBodyHospitalizationHx = $(document.createElement('section'));
                $SectionBodyHospitalizationHx.attr('id', "Cli_HospitalizationHx_Main" + HospitalizationHxId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_HospitalizationHx_" + HospitalizationHxId);
                var $ListHospitalizationHx = $(document.createElement('ul'));

                $ListHospitalizationHx.attr('class', 'list-unstyled')

                $SectionBodyHospitalizationHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_HospitalizationHx_" + HospitalizationHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_HospitalizationHx_Main" + HospitalizationHxId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListHospitalizationHx.append("<li>" + HospitalizationHxFill_Obj.HospitalizationHxSoapText + "</li>");
                $DetailsDiv.append($ListHospitalizationHx);
                $SectionBodyHospitalizationHx.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId).length == 0) {
                    $mainDivHospitalizationHx.append($SectionBodyHospitalizationHx);
                    var HospitalizationHtml = $mainDivHospitalizationHx.html();
                    if (HospitalizationHtml != '') {
                        $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().addClass('initialVisitBody');
                        $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().append(HospitalizationHtml);
                    }
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId).html($SectionBodyHospitalizationHx.html());
                    $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().find('#Cli_HospitalizationHx_Main' + HospitalizationHxId + ' ul').append(CommentHTML);
                }
            }
        }
        dfd.resolve();
        return dfd;
    },

    IsNullReturnSoapValue: function (SoapValue) {
        return (SoapValue == "") ? "" : SoapValue + ",";
    },

    // This Function is called by Progress Notes (Fill HospitalizationHx Func, CopyAllNotesCategories)
    updateHospitalizationHxHtml: function (HospitalizationHxHtml, HospitalizationHxId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().addClass('initialVisitBody');
        if (HospitalizationHxHtml != '') {
            $(NoteHTMLCtrl + ' clinical_Hospitalizationhx').parent().parent().append(HospitalizationHxHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (HospitalizationHxHtml != '') {
            Clinical_HospitalizationHx.AttachHospitalizationHxFromNotes(HospitalizationHxId, hideAlertMessage);
        }

    },

    //This Function detach Hospitalization History From progress note
    detach_ComponentsHospitalizationHx: function (ComponentName, IsUpdate, HospitalizationHxComponentRemove) {

        var Clinical_HospitalizationHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Hospitalizationhx').parent().parent().find('section[id*="Cli_HospitalizationHx_Main"]').map(function () {
            return this.id.replace("Cli_HospitalizationHx_Main", "");
        }).get().join(',');

        if (HospitalizationHxComponentRemove) {

            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Hospitalizationhx').parent().parent().attr('NoteComponentId');
            //Start//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Hospitalization Hx']").remove();
            //End//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            if (Clinical_ProgressNote.params["TemplateName"])
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Hospitalizationhx').parent().parent().addClass('hidden');
            else
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Hospitalizationhx').parent().parent().remove();
            var hxComponents = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .HxComponent').length;

            if (NoteComponentId && NoteComponentId != "NCDummyId" && hxComponents == 0) {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Hospitalizationhx').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('HospitalizationHx', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Hospitalizationhx').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                Clinical_ProgressNote.ShowHideComponetsHeaders();
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Hospitalizationhx').parent().parent().find('section[id*="Cli_HospitalizationHx_Main"]').remove();
        }

        if (Clinical_HospitalizationHxIds == "" || Clinical_HospitalizationHxIds == "undefined") {
            Clinical_ProgressNote.saveComponentSOAPText("HospitalizationHx", true);
            //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            Clinical_HospitalizationHx.DetachHospitalizationHxFromNotes_DBCall(Clinical_HospitalizationHxIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText("HospitalizationHx", true);
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

    //This Functions ask for Detaching Hospitalization Hx from Progress Note for current Patient Selected
    detachHospitalizationHxFromNotes: function (HospitalizationHxId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_hospitalizationhx');
                var selectedValue = HospitalizationHxId.replace('Cli_HospitalizationHx_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_HospitalizationHx.DetachHospitalizationHxFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + HospitalizationHxId).remove();
                            Clinical_ProgressNote.Add_NoText();
                            Clinical_ProgressNote.saveComponentSOAPText("HospitalizationHx", true);
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

    //This Functions attached Hospitalization Hx to Progress Note for current Patient Selected
    AttachHospitalizationHxFromNotes: function (HospitalizationHxId, hideAlertMessage) {
        if (HospitalizationHxId == "" || HospitalizationHxId == "undefined") {
        }
        else {
            Clinical_HospitalizationHx.AttachHospitalizationHxFromNotes_DBCall(HospitalizationHxId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    //If Attached HospitalizationHx Made new inseration to HospitalizationHx Table than good ids should be attached to HTML
                    Clinical_ProgressNote.saveComponentSOAPText("HospitalizationHx", hideAlertMessage);
                    $('#' + HospitalizationHxId).remove();
                    // utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //If HospitalizationHx Component which is dropeed in Progress note has no HospitalizationHx attached, than it will call for Latest HospitalizationHx for this patient
    getLatestHospitalizationHxByPatientId: function (hideAlertMessage) {
        Clinical_HospitalizationHx.getLatestClinical_HospitalizationHxByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_HospitalizationHx.createHospitalizationHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });

    },

    getAutoPopulateSetting_DBCall: function () {

        var objData = new Object();
        objData["UserId"] = globalAppdata["AppUserId"];
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["ComponentType"] = "HospitalizationHistory";
        objData["commandType"] = "getautopopulatesetting";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");

    },

    //Author: Farooq Ahmad
    //Date: 02/01/2016
    //This function will calculate the days between Admission date and Discharge Date
    calculateDays: function () {
        var AdmissionDate = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #dtpHospitalizationAdmissionDate').val();
        var DischargeDate = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #dtpHospitalizationDischargeDate').val();
        if (AdmissionDate != null && AdmissionDate != "" && DischargeDate != null && DischargeDate != "") {
            AdmissionDate = new Date(AdmissionDate);
            DischargeDate = new Date(DischargeDate);
            var Totalyears = "";
            if (DischargeDate != "") {
                var Totalyears = "";
                var diff = new Date(DischargeDate - AdmissionDate);
                var days = diff / 1000 / 60 / 60 / 24;
                days = Math.floor(days)
                if (days > 0) {
                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #txtHospitalizationStayLength').val(days);
                    $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #ddlHospitalizationDiseaseStayId').val("1");
                }
            }
        }
    },
    //Author: Farooq Ahmad
    //Date: 02/01/2016
    //This function will enable or disabled duration validation
    enableDurationValidation: function () {

        var stayLength = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #txtHospitalizationStayLength').val();
        var ddlVal = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #ddlHospitalizationDiseaseStayId').val();
        var AdmissionDate = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #dtpHospitalizationAdmissionDate').val();
        var DischargeDate = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #dtpHospitalizationDischargeDate').val();

        if (AdmissionDate != null && AdmissionDate != "" && DischargeDate != null && DischargeDate != "") {
            AdmissionDate = new Date(AdmissionDate);
            DischargeDate = new Date(DischargeDate);
            var diff = new Date(DischargeDate - AdmissionDate);
            var days = diff / 1000 / 60 / 60 / 24;
            days = Math.floor(days)

            if (days > 0) {
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #txtHospitalizationStayLength').val(days);
            }
            else {
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #txtHospitalizationStayLength').val("1");
            }
            //  $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #txtHospitalizationStayLength').prop("disabled", true)
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #ddlHospitalizationDiseaseStayId').val("1");
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #ddlHospitalizationDiseaseStayId').prop("disabled", true)

            //var Totalyears = "";
            //if (DischargeDate != "") {


            //    diff = new DischargeDate(end - AdmissionDate),
            //    days = diff / 1000 / 60 / 60 / 24;
            //    Math.floor(days)
            //    ; //=> 8.525845775462964
            //    var second = 1000;
            //    var minute = second * 60;
            //    var hour = minute * 60;
            //    var day = hour * 24;
            //    var week = day * 7;
            //    DischargeDate = new Date(DischargeDate)
            //    var timediff = DischargeDate - AdmissionDate;
            //    var years = DischargeDate.getFullYear() - AdmissionDate.getFullYear();
            //    var months = (DischargeDate.getFullYear() * 12 + DischargeDate.getMonth()) - (AdmissionDate.getFullYear() * 12 + AdmissionDate.getMonth());
            //    var days = Math.floor(timediff / day);
            //    var hours = Math.floor(timediff / hour);
            //    var minutes = Math.floor(timediff / minute);
            //    var seconds = Math.floor(timediff / second);
            //    var weeks = Math.floor(timediff / week);
            //    var age = ~~((Date.now() - AdmissionDate) / (31557600000));

            //    //Start//03/02/2016//Ahmad Raza//calculating age at surgery
            //    diff = new Date(
            //    DischargeDate.getFullYear() - AdmissionDate.getFullYear(),
            //    DischargeDate.getMonth() - AdmissionDate.getMonth(),
            //    DischargeDate.getDate() - AdmissionDate.getDate()
            //    );
            //    var durationInYears = diff.getYear();
            //    var durationInMonths = diff.getMonth();
            //    var durationInDays = diff.getDate();
            //    if (stayLength != null && stayLength != '') {
            //        if ($('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #ddlHospitalizationDiseaseStayId').val() == "4") {
            //            if (parseInt(durationInYears) <= parseInt(stayLength)) {
            //                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #txtHospitalizationStayLength').val('');
            //                utility.DisplayMessages("Duration of years should be less than or equal to admission and discharge date difference.", 3);
            //            }
            //        }
            //        if ($('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #ddlHospitalizationDiseaseStayId').val() == "3") {
            //            if (parseInt(durationInMonths) <= parseInt(stayLength)) {
            //                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #txtHospitalizationStayLength').val('');
            //                utility.DisplayMessages("Duration of months should be less than or equal to admission and discharge date difference.", 3);
            //            }
            //        }
            //        if ($('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #ddlHospitalizationDiseaseStayId').val() == "1") {
            //            if (parseInt(durationInDays) <= parseInt(stayLength)) {
            //                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #txtHospitalizationStayLength').val('');
            //                utility.DisplayMessages("Duration of days should be less than or equal to admission and discharge date difference.", 3);
            //            }
            //        }
            //        if ($('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #ddlHospitalizationDiseaseStayId').val() == "2") {
            //            if (parseInt(Math.floor(durationInDays)) <= parseInt(stayLength)) {
            //                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #txtHospitalizationStayLength').val('');
            //                utility.DisplayMessages("Duration of week should be less than or equal to admission and discharge date difference.", 3);
            //            }
            //        }
            //    }


            //}


        }
        if (stayLength != null && stayLength != '') {
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').data('bootstrapValidator').enableFieldValidators('HospitalizationDiseaseStayId', true);
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #lblDuration').html('Length of Stay<span class="required">*</span>');

            if (!(AdmissionDate != null && AdmissionDate != "" && DischargeDate != null && DischargeDate != "")) {
                //Start Abid Ali 10/02/2016, enable ddp value
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #ddlHospitalizationDiseaseStayId').prop("disabled", false);
                $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #txtHospitalizationStayLength').prop("disabled", false);
                //End Abid Ali 10/02/2016, enable ddp value
            }


        }

        else {
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').data('bootstrapValidator').enableFieldValidators('HospitalizationDiseaseStayId', false);
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #lblDuration').html('Length of Stay');

            //Start Abid Ali 10/02/2016, disable and reset selected ddp value
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #ddlHospitalizationDiseaseStayId').prop("disabled", true).find("option:first").attr("selected", true);
            //End Abid Ali 10/02/2016, disable and reset selected ddp value
        }
        if (ddlVal != null && ddlVal != '') {
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').data('bootstrapValidator').enableFieldValidators('HospitalizationDiseaseStayDuration', true);
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #lblDuration').html('Length of Stay<span class="required">*</span>');

        } else {
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx').data('bootstrapValidator').enableFieldValidators('HospitalizationDiseaseStayDuration', false);
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #lblDuration').html('Length of Stay');
        }



    },


    //Author: Farooq Ahmad
    //Date: 11/02/2016
    //This function will validate the admission date and discharge date and length of stay
    validateLengthOfStay: function () {
        var AdmissionDate = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #dtpHospitalizationAdmissionDate').val();
        var DischargeDate = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #frmClinicalHospitalizationHx #dtpHospitalizationDischargeDate').val();
        if (AdmissionDate != null && AdmissionDate != "" && DischargeDate != null && DischargeDate != "") {
            AdmissionDate = new Date(AdmissionDate);
            DischargeDate = new Date(DischargeDate);
            if (AdmissionDate > DischargeDate) {
                utility.DisplayMessages("Admission date should be less than discharge date.", 3);
                return false;
            }


        }
        return true;
    },

    //-----Server calls of Notes----------
    DetachHospitalizationHxFromNotes_DBCall: function (HospitalizationHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["HospitalizationHxId"] = HospitalizationHxId;
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
        objData["commandType"] = "detach_Hospitalizationhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HospitalizationHx");
    },

    AttachHospitalizationHxFromNotes_DBCall: function (HospitalizationHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["HospitalizationHxId"] = HospitalizationHxId;
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
        objData["commandType"] = "attach_Hospitalizationhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HospitalizationHx");
    },

    gridLoad: function (response) {
        var isactive = $('#' + Clinical_HospitalizationHx.params.PanelID + ' #pnlHospitalizationHx_Result #divSwitch #switchActive').attr('isactive');

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #dgvPastHospitalizationHx")) {
            $("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #dgvPastHospitalizationHx").dataTable().fnClearTable();
            $("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #dgvPastHospitalizationHx").dataTable().fnDestroy();
            $("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #dgvPastHospitalizationHx tbody").find("tr").remove();
        }
        var logCount = JSON.parse(response);
        if (logCount.HxLogSoapCount > 0) {
            var LoadJSONData = JSON.parse(logCount.HxLogSoap_JSON); //Parsing array to JSON
            var counter = null;
            for (var i = 0; i < LoadJSONData.length; i++) {
                // $.each(LoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                // $row.attr("onclick", "Clinical_HospitalizationHx.CDSEdit('" + item.CDSId + "',event);");
                //$row.attr("id", "gvCDS_row" + item.CDSId);
                var text = LoadJSONData[i].SoapText;

                counter = i;
                $row.append('<td style="display:none;">' + counter + '</td><td>' + LoadJSONData[i].Action + '</td><td id="sptxt">' + $('<a/>').html($('<a/>').html(text).text()).text() + '</td><td>' + LoadJSONData[i].ModifiedOn + " " + LoadJSONData[i].ModifiedBy + '</td>');
                $row.find('#sptxt').html($('<a/>').html(text).text());
                $("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #dgvPastHospitalizationHx tbody").last().append($row);
                // });
            }
        }
        else {
            $("#" + Clinical_HospitalizationHx.params.PanelID + ' #pnlHospitalizationHx_Result #dgvPastHospitalizationHx').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Known Hospitalization History"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bInfo": false, "bPaginate": false, "bSortable": false, "aTargets": [0] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_HospitalizationHx.params.PanelID + ' #pnlHospitalizationHx_Result #dgvPastHospitalizationHx'))
            ;
        else {
            $("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #dgvPastHospitalizationHx").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[0, "asc"]], "aoColumnDefs": [{ "bInfo": false, "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        }

        $("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #dgvPastHospitalizationHx_filter").remove();
    },

    getLatestClinical_HospitalizationHxByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_Hospitalizationhxby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "HospitalizationHx");
    },


    BindCurrentHospitalizationSoapText: function (resopnse, isDiseaseExists) {

        var $row = $('<tr/>');
        if (isDiseaseExists) {
            $row.append('<td style="display:none;">' + resopnse.HospitalizationHxId + '</td><td>' + resopnse.IsCreatedOrModified + '</td><td>' + resopnse.SoapText + '</td><td>' + resopnse.LastUpdated + '</td>');
        }
        else {
            $row.append('<td>&nbsp;</td><td>No Known Hospitalization History</td><td></td>');
        }


        //$row.append('<td style="display:none;">' + resopnse.HospitalizationHxId + '</td><td>' + resopnse.IsCreatedOrModified + '</td><td>' + resopnse.SoapText + '</td><td>' + resopnse.LastUpdated + '</td>');
        $("#" + Clinical_HospitalizationHx.params.PanelID + " #pnlHospitalizationHx_Result #dgvHospitalizationHx tbody").html($row);

        if ($('#' + Clinical_HospitalizationHx.params.PanelID + ' #pnlHospitalizationHx_Result #divSwitch #switchVisit').attr('status') == '1') {
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #pnlCurrent').removeClass('hidden');
            $('#' + Clinical_HospitalizationHx.params.PanelID + ' #pnlPast').addClass('hidden');
        }
    },
    //--------------end progress Note-----------

}