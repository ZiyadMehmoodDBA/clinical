//Author: Ahmad Raza
//Date: 02-12-2015
//This file will handle all actions performed for Allergy and it's child handling
Clinical_Allergies = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    isViewed: false,
    controlToInvoke: null,
    bNextPrev: false,
    //Start//Ahmad Raza//02/12/2015//This function will be called once tab is clicked, it expects parameters to be used for Allergy
    Load: function (params) {
        Clinical_Allergies.params = params;
        if (Clinical_Allergies.params.ParentCtrl == "clinicalTabFaceSheet") {
            Clinical_Allergies.params.patientID = Clinical_Allergies.params.PatientId;
        } else if (Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet") {
            Clinical_Allergies.params.patientID = Clinical_FaceSheet.params.patientID;
        }

        Clinical_Allergies.params.mode = "Add";

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#pnlClinicalAllergies #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet") {
            $("#pnlClinicalAllergies #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        var self = $('#pnlClinicalAllergies');
        if (Clinical_Allergies.params.PanelID != 'pnlClinicalAllergies') {
            Clinical_Allergies.params.PanelID = Clinical_Allergies.params.PanelID + ' #pnlClinicalAllergies';
            self = $('#' + Clinical_Allergies.params.PanelID);
        }
        utility.CreateDatePicker(Clinical_Allergies.params.PanelID + ' #frmClinicalAllergies #dtpOnsetDate', function () {
            //on-change callback method
        });

        Clinical_Allergies.allergiesSearch();
        if (Clinical_Allergies.bIsFirstLoad == true) {
            self.loadDropDowns(true).done(function () {

                //Start//04/12/2015//Ahmad Raza //to enable multiselect
                var isMultiselected = true;
                $('#' + Clinical_Allergies.params.PanelID + ' #txtReaction').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true
                });
                //End//04/12/2015//Ahmad Raza //to enable multiselect
                Clinical_Allergies.validateAllergies();
                Clinical_Allergies.bIsFirstLoad = false;
                //Start//11/12/2015//Ahmad Raza//Serializing form
                $('#' + Clinical_Allergies.params.PanelID + ' #frmClinicalAllergies').data('serialize', $('#' + Clinical_Allergies.params.PanelID + ' #frmClinicalAllergies').serialize());
                //End//11/12/2015//Ahmad Raza//Serializing form
            });
        }

        Clinical_Allergies.domReadyFunction();
        if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_Allergies.params.PanelID, 'Medical', 'Allergies', 'Clinical_Allergies.unLoadTab(true);', null, true);
            $('#' + Clinical_Allergies.params.PanelID + " div#actionsBtnAddToNotes").show();
        } else {
            $('#' + Clinical_Allergies.params.PanelID + " div#actionsBtnAddToNotes").hide();
        }
        if (Clinical_Allergies.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_Allergies.params.PanelID + " div#FaceSheetPager", Clinical_Allergies.params.FaceSheetComponents, 'allergies');
        } else if (Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #pnlClinicalAllergies' + " div#FaceSheetPager", Clinical_Allergies.params.FaceSheetComponents, 'allergies');
        }

        utility.callbackAfterAllDOMLoaded(function () {

            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1356px");
            }
        });
    },
    //End//Ahmad Raza//02/12/2015//This function will be called once tab is clicked, it expects parameters to be used for Allergy

    //Start//04/12/2015//Ahmad Raza//Plugin method for ToggleSwitch
    openDrFirst: function () {
        var IsPatientRegisteredOnDrFirst;
        $.when(IsUserHaveRcopiaRights = EMRUtility.CheckUserHaveRcopiaRights(globalAppdata["AppUserId"])).then(function () {
            if (IsUserHaveRcopiaRights.response == true) {
                $.when(IsPatientRegisteredOnDrFirst = EMRUtility.CheckPatientIsRegisteredOnDrFirst(Clinical_Allergies.params.patientID)).then(function () {
                    if (IsPatientRegisteredOnDrFirst.response) {
                        var params = [];
                        var PanelID = "";
                        if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                            PanelID = 'pnlClinicalProgressNote #pnlClinicalAllergies';
                            params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalAllergies';
                            params["PrPanelID"] = 'pnlClinicalProgressNote #pnlClinicalAllergies';
                            params["NotesId"] = Clinical_Allergies.params.NotesId == null ? 0 : Clinical_Allergies.params.NotesId;
                        }
                        else if (Clinical_Allergies.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet") {
                            PanelID = 'pnlClinicalFaceSheet #pnlClinicalAllergies';
                            params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalAllergies';
                            params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalAllergies';
                        }
                        else {
                            PanelID = 'pnlClinicalAllergies';
                            params["PanelID"] = 'pnlClinicalAllergies';
                            params["PrPanelID"] = 'pnlClinicalAllergies';
                        }
                        params["ComeFromModuleName"] = "Allergies"
                        params["StartupScreen"] = "manage_allergies";
                        params["PatientId"] = Clinical_Allergies.params.patientID;
                        params["ParentCtrl"] = (Clinical_Allergies.params.ParentCtrl != "clinicalTabProgressNote" && Clinical_Allergies.params.ParentCtrl != "Clinical_FaceSheet" && Clinical_Allergies.params.ParentCtrl != "clinicalTabFaceSheet") ? Clinical_Allergies.params.TabID : "Clinical_Allergies";
                        params["FromAdmin"] = 0;
                        LoadActionPan("DRFirst", params, PanelID);
                    }
                    else {
                        utility.DisplayMessages("Patient Is Not Registered On DrFirst", 3);
                    }
                });
            }
            else {
                utility.DisplayMessages("You are not authorized to access this feature.", 2);
            }
        });
    },
    domReadyFunction: function () {
        $('#' + Clinical_Allergies.params.PanelID + ' [data-plugin-toggle]').each(function () {
            var $this = $(this),
                opts = {};

            var pluginOptions = $this.data('plugin-options');
            if (pluginOptions)
                opts = pluginOptions;

            $this.themePluginToggle(opts);
        });

        (function ($) {
            'use strict';
            $(function () {
                $('#' + Clinical_Allergies.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },
    //End//04/12/2015//Ahmad Raza//method for ToggleSwitch

    //Start//Ahmad Raza//02/12/2015//This function will unload Allergy
    unLoadTab: function (nextOrPre, controlToInvoke) {
        var parentPanelId = null;
        Clinical_Allergies.controlToInvoke = controlToInvoke;

        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_Allergies.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet") {
            //adnan maqbool, EMR-834

            if (Clinical_Allergies.params["FromAdmin"] == "0") {
                if (Clinical_Allergies.params != null && Clinical_Allergies.params.ParentCtrl != null) {
                    if (Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet") {
                        parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                        Clinical_FaceSheet.params.ChildPanelID = null;
                        UnloadActionPan(Clinical_Allergies.params.ParentCtrl, 'Clinical_Allergies', null, parentPanelId);
                    } else {
                        UnloadActionPan(Clinical_Allergies.params.ParentCtrl, 'Clinical_Allergies');
                    }
                }
                else
                    UnloadActionPan(null, 'Clinical_Allergies');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_Allergies").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
            EMRUtility.scrollToPNcomponent('clinical_allergies');
            Clinical_FaceSheet.loadFaceSheet();

        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
            var exist = false;
            $("#" + Clinical_Allergies.params.PanelID + " #dgvAllergies tbody").find('input[type="checkbox"]').each(function () {
                if (this.checked) {
                    exist = true;
                }
                if (exist) {
                    return false;
                }
            });
            if (exist) {
                utility.myConfirmNote('1', function () {
                    $.when(Clinical_Allergies.addAllergyToNotes()).then(function () {
                        if (Clinical_Allergies.params["FromAdmin"] == "0") {
                            if (Clinical_Allergies.params != null && Clinical_Allergies.params.ParentCtrl != null) {

                                if (nextOrPre == true) {
                                    UnloadActionPan(Clinical_Allergies.params.ParentCtrl, 'Clinical_Allergies');
                                    if (Clinical_Allergies.controlToInvoke != null) {
                                        setTimeout(function () {
                                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Allergies.controlToInvoke.replace(/\s/g, ''));
                                            Clinical_Allergies.controlToInvoke = null;
                                        }, 600);
                                    }
                                }
                                else {
                                    UnloadActionPan(Clinical_ReviewofSystems.params.ParentCtrl, 'Clinical_ReviewofSystems');
                                }
                            }
                            else
                                UnloadActionPan(null, 'Clinical_Allergies');
                        }
                        else {
                            $("#mstrDivMedical #clinicalMenu_Medical_Allergies").remove();
                            RemoveAdminTab();
                        }
                        objDeffered.resolve();
                        EMRUtility.scrollToPNcomponent('clinical_allergies');
                    });
                }, "", function () {
                    if (Clinical_Allergies.params["FromAdmin"] == "0") {
                        if (Clinical_Allergies.params != null && Clinical_Allergies.params.ParentCtrl != null) {
                            UnloadActionPan(Clinical_Allergies.params.ParentCtrl, 'Clinical_Allergies');
                            if (Clinical_Allergies.controlToInvoke != null) {

                                setTimeout(function () {
                                    Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Allergies.controlToInvoke.replace(/\s/g, ''));
                                    Clinical_Allergies.controlToInvoke = null;
                                }, 600);

                            }
                        }
                        else
                            UnloadActionPan(null, 'Clinical_Allergies');
                    }
                    else {
                        $("#mstrDivMedical #clinicalMenu_Medical_Allergies").remove();
                        RemoveAdminTab();
                    }
                    objDeffered.resolve();
                    EMRUtility.scrollToPNcomponent('clinical_allergies');
                });
            }
            else {
                if (Clinical_Allergies.params["FromAdmin"] == "0") {
                    if (Clinical_Allergies.params != null && Clinical_Allergies.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_Allergies.params.ParentCtrl, 'Clinical_Allergies');
                        if (Clinical_Allergies.controlToInvoke != null) {

                            setTimeout(function () {
                                Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Allergies.controlToInvoke.replace(/\s/g, ''));
                                Clinical_Allergies.controlToInvoke = null;
                            }, 600);

                        }
                    }
                    else
                        UnloadActionPan(null, 'Clinical_Allergies');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_Allergies").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
                EMRUtility.scrollToPNcomponent('clinical_allergies');
            }
        }
        else {
            if (Clinical_Allergies.params["FromAdmin"] == "0") {
                if (Clinical_Allergies.params != null && Clinical_Allergies.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_Allergies.params.ParentCtrl, 'Clinical_Allergies');
                }
                else
                    UnloadActionPan(null, 'Clinical_Allergies');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_Allergies").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
            EMRUtility.scrollToPNcomponent('clinical_allergies');
        }
        return objDeffered;
    },
    //End//Ahmad Raza//02/12/2015//This function will unload Allergy

    //Start//Ahmad Raza//02/12/2015//This function will validate Allergy form
    validateAllergies: function () {
        $('#' + Clinical_Allergies.params.PanelID + ' #frmClinicalAllergies')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   //Start//28/12/2015//Ahmad Raza//Validation implemented for Allergen field
                   Allergen: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //End//28/12/2015//Ahmad Raza//Validation implemented for Allergen field
                   //name: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   //Color: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //}
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Clinical_Allergies.allergiesSave();
            ////Start//04/12/2015//Ahmad Raza//clear multiselect after saving records
            //$('#' + Clinical_Allergies.params.PanelID + ' #txtProvider').multiselect('deselectAll', false);
            //$('#' + Clinical_Allergies.params.PanelID + ' #txtProvider').multiselect('refresh');
            //$('#' + Clinical_Allergies.params.PanelID + ' #txtReaction').multiselect('clearSelection');
            //End//04/12/2015//Ahmad Raza//clear multiselect after saving records


        });
    },
    //End//Ahmad Raza//02/12/2015//This function will validate Allergy form

    //Start//Ahmad Raza//02/12/2015//This function will search Allergies if user have search privileges
    allergiesSearch: function (AllergyId, PageNo, rpp) {
        var dfd = $.Deferred();
        var strMessage = "";
        if ($("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result").css("display") == "none") {
            $("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result").show();
        }
        //Start//11/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Medical_Allergies", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_Allergies.searchAllergy(AllergyId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_Allergies.isViewed = false;
                        if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                            if ($("#" + Clinical_Allergies.params.PanelID + " #dgvAllergies thead tr #SelectRecord").length == 0) {
                                // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-275
                                $("#" + Clinical_Allergies.params.PanelID + " #dgvAllergies thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important; width: 0px;" coltype="checkbox">  <input type="checkbox" id="chkHeaderAllergies" onchange="Clinical_Allergies.checkUncheckAllAllergies(this);"   class="input-block" coltype="checkbox"/> </th>');

                                // End Date  Edit By Muhammad Ahmad Imran Bug # EMR-275
                            }

                        } else {
                            $("#" + Clinical_Allergies.params.PanelID + " #dgvAllergies th#SelectRecord").remove();
                        }

                        $.when(Clinical_Allergies.allergiesGridLoadNew(response)).then(function () {
                            dfd.resolve();
                        });
                        //Start//05/12/2015//Ahmad Raza//server side pagination Logic

                        var TableControl = Clinical_Allergies.params.PanelID + " #dgvAllergies";
                        var PagingPanelControlID = Clinical_Allergies.params.PanelID + " #divAllergies_Paging";
                        var ClassControlName = "Clinical_Allergies";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.allergiesCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                            Clinical_Allergies.allergiesSearch(PrimaryID, pageNumber, resultPerPage);
                        }), 10);

                        //setTimeout(function () {
                        //    if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Allergies.params.PanelID + " #btnAddAllergyToNotes").length == 0) {
                        //        $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Clinical_Allergies.addAllergyToNotes();" disabled id="btnAddAllergyToNotes">Add on Note</button>').insertAfter("#" + Clinical_Allergies.params.PanelID + "  #divAllergies_Paging .pagination")
                        //    }
                        //}, 11);
                        //End//05/12/2015//Ahmad Raza//server side pagination Logic
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                    if (response.allergyReview_JSON != null) {
                        Clinical_Allergies.insertReviewedInfoOnTop(response.allergyReview_JSON);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        return dfd;
        //End//11/12/2015//Ahmad Raza//Privileges logic implemented
    },
    //End//Ahmad Raza//02/12/2015//This function will search Allergies if user have search privileges

    //Start//Ahmad Raza//02/12/2015//This function will be save allergy if user have save privileges
    allergiesSave: function () {
        //Start//13/01/2016//Ahmad Raza//fixed bug#EMR-212
        $("#" + Clinical_Allergies.params.PanelID + " #frmClinicalAllergies").bootstrapValidator('revalidateField', 'Allergen');
        if ($("#" + Clinical_Allergies.params.PanelID + " #frmClinicalAllergies #txtAllergen").val() != "") {
            //End//13/01/2016//Ahmad Raza//fixed bug#EMR-212
            var CurrentID = this.id;
            var strMessage = "";
            //Start//15/12/2015//Ahmad Raza//Setting PatientId
            $("#" + Clinical_Allergies.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            if (Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet") {
                $("#" + Clinical_Allergies.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
            }
            //End//15/12/2015//Ahmad Raza//Setting PatientId
            var self = $("#" + Clinical_Allergies.params.PanelID + " #frmClinicalAllergies");
            var myJSON = self.getMyJSONByName();
            if (Clinical_Allergies.params.mode == "Add") {
                //Start//11/12/2015//Ahmad Raza//Privileges Logic Implemented
                AppPrivileges.GetFormPrivileges("Medical_Allergies", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Clinical_Allergies.saveAllergies(myJSON).done(function (response) {

                            response = JSON.parse(response);
                            if (response.status != false) {

                                if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                                    Clinical_Allergies.getAllergiesInfo(response.AllergyId, true);
                                } else {
                                    Clinical_Allergies.allergiesSearch();
                                }
                                utility.DisplayMessages(response.message, 1);

                                $("#" + Clinical_Allergies.params.PanelID + ' #frmClinicalAllergies').resetAllControls(null);
                                $("#" + Clinical_Allergies.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
                                if (Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet") {
                                    $("#" + Clinical_Allergies.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
                                }
                                $('#' + Clinical_Allergies.params.PanelID + ' #frmClinicalAllergies').data('bootstrapValidator').enableFieldValidators('Allergen', true);
                                ////Start//04/12/2015//Ahmad Raza//clear multiselect after saving records
                                $('#' + Clinical_Allergies.params.PanelID + ' #txtReaction').multiselect('deselectAll', false);
                                $('#' + Clinical_Allergies.params.PanelID + ' #txtReaction').multiselect('refresh');
                                ////End//04/12/2015//Ahmad Raza//clear multiselect after saving records

                                //if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                                //    Clinical_Allergies.getAllergiesInfo(response.AllergyId);
                                //}
                                //Start//11/12/2015//Serializing the form
                                $('#' + Clinical_Allergies.params.PanelID + ' #frmClinicalAllergies').data('serialize', $('#' + Clinical_Allergies.params.PanelID + ' #frmClinicalAllergies').serialize());
                                //End//11/12/2015//Serializing the form

                            }
                            else {
                                //utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
                //End//11/12/2015//Ahmad Raza//Privileges Logic Implemented

            }
            else if (Clinical_Allergies.params.mode == "Edit") {
                Clinical_Allergies.updateAllergies(myJSON, Clinical_Allergies.params.AllergyId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });

            }
        }
    },
    //End//Ahmad Raza//02/12/2015//This function will be save allergy if user have save privileges

    //Start//Ahmad Raza//02/12/2015//This function will will call db for allergy data insertion
    saveAllergies: function (allergiesData) {

        var objData = JSON.parse(allergiesData);
        if (objData.PatientId == '') {
            objData.PatientId = Clinical_Allergies.params.patientID;
        }

        //Start//04/12/2015//Ahmad Raza///Multiselect Value selection
        objData["Reaction"] = $('#' + Clinical_Allergies.params.PanelID + ' #txtReaction option[value!=select]:selected').map(function (a, item) { return item.value; }).get().join();
        //End//04/12/2015//Ahmad Raza///Multiselect Value selection

        objData["commandType"] = "SAVE_ALLERGY";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "Allergy");

    },
    //End//Ahmad Raza//02/12/2015//This function will will call db for allergy data insertion

    //Start//Ahmad Raza//02/12/2015//This function will update the record in database
    updateAllergies: function (allergiesData, AllergyId) {

        var IsActive = null;
        IsActive = $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #divSwitch #switchActive').attr('IsActive');

        var objData = JSON.parse(allergiesData);
        if (objData.PatientId == '') {
            objData.PatientId = Clinical_Allergies.params.patientID;
        }
        objData["IsActive"] = IsActive;
        objData["commandType"] = "UPDATE_ALLERGY";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "Allergies");

    },
    //End//Ahmad Raza//02/12/2015//This function will update the record in database

    //Start//Ahmad Raza//02/12/2015//This function will handle NoKnownAllergies issue(when there is no allergy exists for a patient)
    noKnownAllergy: function () {
        var strMessage = "";
        var self = $("#" + Clinical_Allergies.params.PanelID + " #frmClinicalAllergies");
        var myJSON = self.getMyJSONByName();
        Clinical_Allergies.saveAllergies(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #btnNoKnownAllergies").css("display", "none");
                utility.DisplayMessages(response.message, 1);
                Clinical_Allergies.allergiesSearch();
                $("#" + Clinical_Allergies.params.PanelID + ' #frmClinicalAllergies').resetAllControls(null);
            }
            else {
                //utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    //End//Ahmad Raza//02/12/2015//This function will handle NoKnownAllergies issue(when there is no allergy exists for a patient)

    //Start//Ahmad Raza//02/12/2015//This function will call database to load allergies for specific patient
    searchAllergy: function (AllergyId, pageNumber, rowsPerPage) {

        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }

        /////
        var IsActive = null;
        IsActive = $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #divSwitch #switchActive').attr('IsActive');

        if (IsActive == null) {
            IsActive = "1";
        }

        var objData = new Object();
        objData["PatientId"] = Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = IsActive;
        objData["NoteId"] = Clinical_Allergies.params.NotesId == null ? 0 : Clinical_Allergies.params.NotesId;
        objData["pageNumber"] = pageNumber;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_ALLERGY";
        //Start 26-10-2016 Humaira Yousaf for logging of view action
        objData["isViewed"] = Clinical_Allergies.isViewed;
        //End 26-10-2016 Humaira Yousaf for logging of view action
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");

    },
    //End//Ahmad Raza//02/12/2015//This function will call database to load allergies for specific patient

    //Start//Ahmad Raza//02/12/2015//This function will bind the allergy data with Data Grid view
    allergiesGridLoadNew: function (response) {

        var dfd = $.Deferred();
        //Start By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search

        var isactive = $('#' + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #divSwitch #switchActive').attr('isactive');

        if ($.fn.dataTable.isDataTable("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies")) {
            $("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies").dataTable().fnClearTable();
            $("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies").dataTable().fnDestroy();
        }
        //  $("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies").dataTable().fnDestroy();
        $("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies tbody").find("tr").remove();

        //End By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search



        //if ($.fn.dataTable.isDataTable("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies")) {
        //    $("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies").dataTable().fnDestroy();
        //}


        if (response.allergiesCount > 0) {

            $("#pnlAllergies_Result #btnNoKnownAllergies").hide();


            var allergyLoadJSONData = JSON.parse(response.allergiesLoad_JSON);
            var allergyHistoryLoadJSONData = JSON.parse(response.allergyHistoryLoad_JSON);
            // get Actions
            var actions = "";
            $("#" + Clinical_Allergies.params.PanelID + " #dgvAllergies tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EMREditableGrid.GetActions(arrActionType);
                    }
                }
            });



            //tem array to hold rows and childs
            var arraTemp = [];

            $.each(allergyLoadJSONData, function (i, item) {



                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.AllergyId);
                //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
                $row.attr("allergynotesid", item.NoteId);
                //End//31/12/2015//Ahmad Raza//Bug#178 fixed

                //Start By Babur on 1/19/2016 - show deleted allergies
                var strModifiedByWithDeleted = "By " + item.ModifiedBy;

                if (item.IsActive == "False" && item.InActiveCheckBoxValue == "Deleted") {
                    strModifiedByWithDeleted += " (Deleted)";
                }

                //End By Babur on 1/19/2016 - show deleted allergies



                var color = "";


                if (item.Severity == "Mild") {
                    color = 'style = "color:green;font-weight:bold"'
                }
                if (item.Severity == "Sever") {
                    color = 'style = "color:red;font-weight:bold"'
                }
                if (item.Severity == "Moderate") {
                    color = 'style = "color:orange;font-weight:bold"'
                }


                var comments = "";

                if (item.Comments != "") {

                    var commentsMethod = "Clinical_Allergies.AddComments('" + item.AllergyId + "');";
                    //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                    comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';

                }
                var AllergyId = item.AllergyId;
                var ChildHistory_Allergy = $.grep(allergyHistoryLoadJSONData, function (n, i) {
                    return n.AllergyId == AllergyId;
                });
                //Start//15/12/2015//Ahmad Raza//grid row sequence issue resolved when NoKnownAllergies, in ProgressNotes
                if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote" && item.Allergen == "No Known Allergies") {

                    //$row.append('<td></td><td><input type="checkbox" id="' + AllergyId + '"  onchange="Clinical_Allergies.enableAddAllergiesWithNotes(this);" name="SelectCheckBoxAllergy" ' + checked + ' class="input-block"/></td><td class="actions" id="' + item.AllergyId + '" ></td><td>' + item.Allergen + '</td><td>' + item.Reaction + '</td><td>' + item.Type + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.OnSetDate) + '</td><td>' + item.LastModified + ' <br>' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                    //Start//15/12/2015//Ahmad Raza//Adding check box with noKnownAllergies row
                    var selectionCheckBoxColumn = "";
                    var controldisabledClass = "";
                    var checked = "";
                    if (item.IsNoteLinked == "True") {
                        if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.AllergyId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                            checked = " ";
                        } else {
                            checked = " checked";
                        }
                    } else {
                        if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.AllergyId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                            checked = " checked";
                        } else {
                            checked = "";
                        }
                    }
                    if (checked == " checked") {
                        Clinical_ProgressNote.AttachedNoteComponentIds.push(item.AllergyId);
                    }
                    if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-275
                        selectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" id="' + AllergyId + '"  onchange="Clinical_Allergies.enableAddAllergiesWithNotes(this);" name="SelectCheckBoxAllergy" ' + checked + ' class="input-block"/></td>';
                        // End Date  Edit By Muhammad Ahmad Imran Bug # EMR-275
                    } else {
                        selectionCheckBoxColumn = "";
                    }
                    if (ChildHistory_Allergy.length > 0) {
                        $row.append(selectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions" id="' + item.AllergyId + '" >' + actions + '</td><td>' + item.Allergen + '</td><td>' + item.Reaction + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.OnSetDate) + '</td><td>' + item.LastModified + '<br> ' + strModifiedByWithDeleted + '</td><td align="center"> ' + comments + ' </td>');
                    } else {
                        $row.append(selectionCheckBoxColumn + '<td></td><td class="actions" id="' + item.AllergyId + '" >' + actions + '</td><td>' + item.Allergen + '</td><td>' + item.Reaction + '</td><td>' + utility.RemoveTimeFromDate(null, item.OnSetDate) + '</td><td>' + item.LastModified + '<br> ' + strModifiedByWithDeleted + '</td><td>' + item.Severity + '</td>');
                    }
                    //End//15/12/2015//Ahmad Raza//Adding check box with noKnownAllergies row

                }
                else {
                    if (item.Allergen == "No Known Allergies") {
                        $row.append('<td></td><td></td><td>' + item.Allergen + '</td><td>' + item.Reaction + '</td><td>' + utility.RemoveTimeFromDate(null, item.OnSetDate) + '</td><td>' + item.LastModified + ' <br>' + strModifiedByWithDeleted + '</td><td>' + item.Severity + '</td>');
                    }

                    else {
                        var selectionCheckBoxColumn = "";
                        var controldisabledClass = "";
                        var checked = "";
                        if (item.IsNoteLinked == "True") {
                            if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.AllergyId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                                checked = " ";
                            } else {
                                checked = " checked";
                            }
                        } else {
                            if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.AllergyId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                                checked = " checked";
                            } else {
                                checked = "";
                            }
                        }
                        if (checked == " checked") {
                            Clinical_ProgressNote.AttachedNoteComponentIds.push(item.AllergyId);
                        }
                        if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-275
                            selectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" id="' + AllergyId + '"  onchange="Clinical_Allergies.enableAddAllergiesWithNotes(this);" name="SelectCheckBoxAllergy" ' + checked + ' class="input-block"/></td>';
                            // End Date  Edit By Muhammad Ahmad Imran Bug # EMR-275
                        } else {
                            selectionCheckBoxColumn = "";
                        }
                        if (ChildHistory_Allergy.length > 0) {
                            $row.append(selectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions" id="' + item.AllergyId + '" >' + actions + '</td><td>' + item.Allergen + '</td><td>' + item.Reaction + '</td><td>' + utility.RemoveTimeFromDate(null, item.OnSetDate) + '</td><td>' + item.LastModified + '<br> ' + strModifiedByWithDeleted + '</td><td>' + item.Severity + '</td>');
                        } else {
                            $row.append(selectionCheckBoxColumn + '<td></td><td class="actions" id="' + item.AllergyId + '" >' + actions + '</td><td>' + item.Allergen + '</td><td>' + item.Reaction + '</td><td>' + utility.RemoveTimeFromDate(null, item.OnSetDate) + '</td><td>' + item.LastModified + '<br> ' + strModifiedByWithDeleted + '</td><td>' + item.Severity + '</td>');
                        }
                        //End//15/12/2015//Ahmad Raza//grid row sequence issue resolved when NoKnownAllergies, in ProgressNotes

                    }
                }
                if (item.IsActive == "True") {

                    $($row).find('a.edit-row').removeClass('disableAll')
                } else {

                    $($row).find('a.edit-row').addClass('disableAll')
                }

                $("#" + Clinical_Allergies.params.PanelID + " #dgvAllergies tbody").last().append($row);


                var currentRowchilds = $();

                if (ChildHistory_Allergy.length > 0) {
                    $.each(ChildHistory_Allergy, function (i, item) {

                        //Start By Babur on 1/19/2016 - show deleted allergies
                        var strModifiedByWithDeletedHistory = "By " + item.ModifiedBy;

                        if (item.IsActive == "False" && item.InActiveCheckBoxValue == "Deleted") {
                            strModifiedByWithDeletedHistory += " (Deleted)";
                        }

                        //End By Babur on 1/19/2016 - show deleted allergies

                        if (item.Comments != "") {

                            var commentsMethod = "Clinical_Allergies.AddComments('" + item.AllergyId + "');";
                            //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                            comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                        }
                        var title_Tooltip = "Inactive Reason: " + item.InActiveCheckBoxValue + (item.InActiveReason != '' ? " <br/>Comments: " + item.InActiveReason : "");
                        var isActiveText = "";
                        if (item.IsActive == "True") {
                            isActiveText = "<Label>[Active]</Label>";
                        } else {
                            isActiveText = "<Label data-toggle='tooltip' data-placement='right' title='" + title_Tooltip + "'>[Inactive]</Label>";
                        }

                        // Start 2/12/2015 Ahmad Raza //Change color of severity
                        var colorChild = "";
                        if (item.Severity == "Sever") {
                            colorChild = 'style = "color:red;font-weight:bold"'
                        }
                        if (item.Severity == "Mild") {
                            colorChild = 'style = "color:green;font-weight:bold"'
                        }
                        if (item.Severity == "Moderate") {
                            colorChild = 'style = "color:orange;font-weight:bold"'
                        }
                        // End 2/12/2015 Ahmad Raza// Change color of severity

                        //Start//Ahmad Raza//14/12/2015//Row Sequence issue in History Grid, fixed
                        if (Clinical_Allergies.params.ActionPanContainer == "actionPanClinicalProgressNote") {

                            var currentHistory = '<tr class="childRow-bg"><td></td><td></td><td class="actions" id="' + item.AllergyId + '" ></td><td class="actions" id="' + item.AllergyId + '" >' + item.Allergen + '</td><td>' + item.Reaction + '</td><td>' + utility.RemoveTimeFromDate(null, item.OnSetDate) + '</td><td>' + item.LastModified + '<br>' + strModifiedByWithDeletedHistory + '</td></tr>';
                        }
                        else {
                            var currentHistory = '<tr class="childRow-bg"><td></td><td class="actions" id="' + item.AllergyId + '" ></td><td class="actions" id="' + item.AllergyId + '" >' + item.Allergen + '</td><td>' + item.Reaction + '</td><td>' + utility.RemoveTimeFromDate(null, item.OnSetDate) + '</td><td>' + item.LastModified + '<br>' + strModifiedByWithDeletedHistory + '</td></tr>';

                        }
                        //End//Ahmad Raza//14/12/2015//Row Sequence issue in History Grid, fixed

                        currentRowchilds = currentRowchilds.add(currentHistory);

                    });
                }

                if (currentRowchilds.length > 0) {

                }

                arraTemp.push({ row: $row, childs: currentRowchilds });
            });


            //Inalize grid
            var panelGrid = "#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result";
            var gridId = "#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies";

            //Start By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search

            if ($.fn.dataTable.isDataTable(gridId) || $(gridId).parent().parent().hasClass("dataTables_wrapper"))
                ;
            else
                Clinical_Allergies.myGrid = EMRUtility.MakeEditableGrid(panelGrid, gridId, Clinical_Allergies, 0, false, true, false, true, false, null);
            //  $(gridId).DataTable({ bDestroy: true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown


            //if (Clinical_Allergies.myGrid != null) {
            //    //  Clinical_Allergies.myGrid.$table.find("tbody tr").remove();
            //  //  Clinical_Allergies.myGrid.$table.dataTable().fnClearTable()
            //    Clinical_Allergies.myGrid.$table.dataTable().fnDestroy();

            //    //  Clinical_Allergies.myGrid.datatable.clear().draw();
            //    $("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies").dataTable().fnDestroy();
            //}

            //End By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search

            // var sDom = 'l<"mystuff col-sm-5">ftip';

            // $("#" + Clinical_Allergies.params.PanelID + " div.mystuff").attr('id', 'divSwitch');


            //Babur commented below line
            //if ($('#' + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies_wrapper").length == 0) {
            //    Clinical_Allergies.myGrid = EMRUtility.MakeEditableGrid(panelGrid, gridId, Clinical_Allergies, 0, false, true, false, true, false, null);
            //}



            //   }

            //rander childs
            $.each(arraTemp, function (i, item) {

                if (Clinical_Allergies.myGrid != null) {
                    var row = Clinical_Allergies.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });




            var checked = '';
            if (isactive == "0") {
            } else if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "1";
                checked = 'checked="checked"';
            }

            var HtmlOfSwitchA = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                           '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_Allergies.activeAllergySearch(this);">' +
                            '</div><span class="pl-xs">Active</span>' +
            '&nbsp;&nbsp;&nbsp;&nbsp;<a id="btnNoKnownAllergies" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_Allergies.noKnownAllergy();">No Known Allergies</a>';

            $("#" + Clinical_Allergies.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitchA + '</div>');
        }
        else {
            if ($('#' + Clinical_Allergies.params.PanelID + ' div#divShowHistory').hasClass("hidden") == false) {
                $('#' + Clinical_Allergies.params.PanelID + ' div#divShowHistory').addClass("hidden");
            }
            //if (response.allergiesTotalCount == 0) {
            //    $("#pnlAllergies_Result #btnNoKnownAllergies").css("display", "");
            //} else {
            //    $("#pnlAllergies_Result #btnNoKnownAllergies").hide();
            //}
            ///*


            //Start By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search

            //$("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #dgvAllergies').DataTable({
            //    // "sDom": 'l<"mystuff col-sm-5">ftip',
            //    "language": {
            //        "emptyTable": "No Allergy List Found."
            //    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }], "bDestroy": true
            //});

            var emptyTableMsg = "No Allergy List Found.";

            if (!$("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #dgvAllergies').parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #dgvAllergies').DataTable({
                    bDestroy: true,
                    "language": {
                        "emptyTable": emptyTableMsg
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="6" class="center" >' + emptyTableMsg + '</td>');
                $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #dgvAllergies tbody').last().append($row);
            }

            //End By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search



            var checked = '';
            if (isactive == "0") {
            } else if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "1";
                checked = 'checked="checked"';
            }
            var HtmlOfSwitchA = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                        '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_Allergies.activeAllergySearch(this);">' +
                         '</div><span class="pl-xs">Active</span>' +
         '<a id="btnNoKnownAllergies" class="ml-md btn btn-link btn-xs" style="display:none" onclick="Clinical_Allergies.noKnownAllergy();">No Known Allergies</a>';

            $("#" + Clinical_Allergies.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitchA + '</div>');
            //if (response.allergiesTotalCount == 0) {
            //    // By Babur on 1/14/2016, commented below line due to DrFirst integration
            //    // $("#pnlAllergies_Result #btnNoKnownAllergies").css("display", "");
            //} else {
            //    $("#pnlAllergies_Result #btnNoKnownAllergies").hide();
            //}

            /* End of Code for making No Known Problem List hyperlink inline with checkbox and search box.
            *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
            */
        }

        EMRUtility.SwicthWidgetInializatoin();


        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        //Start//22//02//2016//Muhammad Ahmad Imran//Sorting removed from first column of grid
        if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_Allergies.params.PanelID + ' #dgvAllergies thead tr th:first-child').removeClass('sorting_asc');
        }
        //End//22//02//2016//Muhammad Ahmad Imran//Sorting removed from first column of grid
        dfd.resolve();
        return dfd;
    },

    //Start by Khaleel Ur Rehman to check uncheck all Allergies by a checkBox in header. Date: 22 Jan 2016.
    checkUncheckAllAllergies: function (chkBox) {
        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_Allergies.params.PanelID + " [name='SelectCheckBoxAllergy']").prop("checked", true);
        } else {
            $("#" + Clinical_Allergies.params.PanelID + " [name='SelectCheckBoxAllergy']").prop("checked", false);
        }
        $("#" + Clinical_Allergies.params.PanelID + " #dgvAllergies tbody").find('input[type="checkbox"]').each(function () {
            Clinical_Allergies.enableAddAllergiesWithNotes(this);
        });
    },
    //End by Khaleel Ur Rehman to check uncheck all Allergies by a checkBox in header. Date: 22 Jan 2016.
    //End//Ahmad Raza//02/12/2015//This function will bind the allergy data with Data Grid view

    //Start//Ahmad Raza//02/12/2015//This function will load the popup for allergy comments in case of Allergy Edit
    AddComments: function (AllergyId) {

        var params = [];
        var PanelID = "";
        //Start//23/12/2015//Ahmad Raza//Fixed EMR Bug#149
        if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
            params["ParentCtrl"] = 'Clinical_Allergies';
            PanelID = 'pnlClinicalProgressNote #pnlClinicalAllergies';
        }

        else {
            params["ParentCtrl"] = 'clinicalTabAllergies';
            PanelID = 'pnlClinicalAllergies';
        }
        params["AllergyId"] = AllergyId;
        params["FromAdmin"] = "0";
        params["PatientId"] = Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        LoadActionPan('Clinical_AllergiesComments', params, PanelID);
        //End//23/12/2015//Ahmad Raza//Fixed EMR Bug#149
    },
    //End//Ahmad Raza//02/12/2015//This function will load the popup for allergy comments in case of Allergy Edit

    //Start//Ahmad Raza//02/12/2015//This function will search Allergies with Active Status
    activeAllergySearch: function (objThis) {


        var IsActive = $(objThis).attr('IsActive');

        if (IsActive == '1') {
            $(objThis).attr('IsActive', '0');
        }
        else if (IsActive == '0') {
            $(objThis).attr('IsActive', '1');
        }

        Clinical_Allergies.allergiesSearch();
    },
    //End//Ahmad Raza//02/12/2015//This function will search Allergies with Active Status

    //Start//Ahmad Raza//02/12/2015//This function will delete the selected allergy from the database
    deleteAllergy: function (AllergyId) {

        var objData = new Object();
        objData["AllergyId"] = AllergyId;
        objData["commandType"] = "DELETE_ALLERGY";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");

    },
    //End//Ahmad Raza//02/12/2015//This function will delete the selected allergy from the database

    //Start//Ahmad Raza//02/12/2015//This function will bind the allergy history records in Grid view
    buildHistoryRows: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems) {
        var row = Clinical_Allergies.EditableGrid.datatable.row(CurrentRow);
        if (arrChildItems != null && arrChildItems.length > 0) {
            var currentRowchilds = $();
            $.each(arrChildItems, function (i, item) {
                var currentChildRow = $("#" + CurrentRow.attr("id")).clone();
                currentChildRow.attr("id", "Child" + item.AllergyId);
                currentChildRow.attr("parentallergyid", ParentRowId);
                currentChildRow.addClass("childRow-bg");
                $(currentChildRow).find("td:nth-child(1)").html("");
                $(currentChildRow).find("td:nth-child(2)").html("");
                $(currentChildRow).find("td:nth-child(3)").html("");
                currentRowchilds = currentRowchilds.add(currentChildRow);
            });
            row.child(currentRowchilds).show();
            setTimeout(function () {
                row.child.hide();
            }, 100);

        }
        else {
            $(CurrentRow).find("td:nth-child(1)").html("");
        }

        return row.child();
    },
    //End//Ahmad Raza//02/12/2015//This function will bind the allergy history records in Grid view

    //Start//Ahmad Raza//02/12/2015//This function will update the allergy record in database
    updateAllergiesRow: function (AllergyData, AllergyId) {

        var IsActive = null;
        IsActive = $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #divSwitch #switchActive').attr('IsActive');

        var objData = JSON.parse(AllergyData);
        objData["AllergyId"] = AllergyId;
        objData["Comments"] = $("#" + Clinical_Allergies.params.PanelID + ' #hfGridComments').val();
        objData["IsActive"] = IsActive;
        objData["commandType"] = "UPDATE_ALLERGY";

        var data = JSON.stringify(objData);


        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");

    },
    //End//Ahmad Raza//02/12/2015//This function will update the allergy record in database

    //Start//Ahmad Raza//02/12/2015//This function will show square/minus sign with the allergy record that has child rows
    showHideEditableGridRows: function (isShow) {

        var allergiesgridId = "#" + Clinical_Allergies.params.PanelID + " #dgvAllergies";
        var dataTable = $(allergiesgridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = Clinical_Allergies.EditableGrid.datatable.row(parentRow);

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
    //End//Ahmad Raza//02/12/2015//This function will show square/minus sign with the allergy record that has child rows

    //Start//Ahmad Raza//02/12/2015//This function will show child rows and change the icon at most left to plus/minus sign icon
    rowExpand: function ($row, obj) {
        var _self = obj,
            $actions,
            values = [];
        var row = _self.datatable.row($row);

        //Start//14/12/2015//Ahmad Raza//Logic implemented for plus,minus sign on row expand/ row collapse, when form opened in notes
        if (Clinical_Allergies.params.ActionPanContainer == "actionPanClinicalProgressNote") {
            if (row.child.isShown()) {
                // This row is already open - close it
                $row.find("td:nth-child(2) .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();
                //tr.removeClass('shown');
            }
            else {
                $row.find("td:nth-child(2) .fa-plus-square").attr("class", "fa fa-minus-square");
                // Open this row
                row.child.show();
                if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                    row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
                }
            }
        }
        else {
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
                if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                    row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
                }
            }
        }

        //End//14/12/2015//Ahmad Raza//Logic implemented for plus,minus sign on row expand/ row collapse, when form opened in notes

    },
    //End//Ahmad Raza//02/12/2015//This function will show child rows and change the icon at most left to plus/minus sign icon

    //Start//Ahmad Raza//02/12/2015//This function will draw a new row in grid
    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },
    //End//Ahmad Raza//02/12/2015//This function will draw a new row in grid

    //Start//Ahmad Raza//02/12/2015//This function will be called when user is editing the row of grid and then cancel the updation
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
    },
    //End//Ahmad Raza//02/12/2015//This function will be called when user is editing the row of grid and then cancel the updation

    //Start//Ahmad Raza//02/12/2015//This function will change the status of Allergy to InActive with InActive reason in database
    inActiveAllergy: function (AllergyId, comments) {

        var IsActive = null;
        IsActive = $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #divSwitch #switchActive').attr('IsActive');

        var IsActiveRecord = null;
        IsActiveRecord = $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #divSwitch #switchActive').attr('IsActive');
        if (IsActiveRecord == "1")
            IsActiveRecord = "0";
        else if (IsActiveRecord == "0")
            IsActiveRecord = "1";

        var patientId = Clinical_Allergies.params.PatientId;

        var objData = new Object();
        objData["AllergyId"] = AllergyId;
        objData["PatientId"] = patientId;
        objData["InActiveChkBoxValue"] = null;
        objData["InActiveReason"] = null;
        objData["EndDate"] = null;
        objData["IsActive"] = IsActive;
        objData["IsActiveRecord"] = IsActiveRecord;
        objData["commandType"] = "INACTIVE_ALLERGY";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "Allergy");

    },
    //End//Ahmad Raza//02/12/2015//This function will change the status of Allergy to InActive with InActive reason in database

    //Start//Ahmad Raza//02/12/2015//This function will call the inActiveAllergy function after confirming User privileges and taking reasons to inActive the allergy in popup
    rowInactive: function ($row, obj) {

        var id = $row.attr("id");
        //Start//21/12/2015//Ahmad Raza//Implimented Privileges check logic
        var IsActive = null;
        IsActive = $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #divSwitch #switchActive').attr('IsActive');           
        AppPrivileges.GetFormPrivileges("Medical_Allergies", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {

                        var IsActiveRecord = null;
                        IsActiveRecord = $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #divSwitch #switchActive').attr('IsActive');
                        if (IsActiveRecord == "1") {
                            //Start//23/12/2015//Ahmad Raza//Fixed EMR Bug#150
                            var params = [];
                            var PanelID = "";

                            if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                                params["ParentCtrl"] = 'Clinical_Allergies';
                                PanelID = 'pnlClinicalProgressNote #pnlClinicalAllergies';
                            }
                            else {
                                params["ParentCtrl"] = 'clinicalTabAllergies';
                                PanelID = 'pnlClinicalAllergies';
                            }
                            params["AllergyId"] = selectedValue;
                            //Start//15/12/2015//Ahmad raza//Inactive Popup issue in ProgressNote fixed
                            params["FromAdmin"] = "0";
                            // params["ParentCtrl"] = "Clinical_Allergies";
                            //End//15/12/2015//Ahmad raza//Inactive Popup issue in ProgressNote fixed
                            params["PatientId"] = Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                            LoadActionPan('Clinical_AllergyInActive', params, PanelID);
                            //End//23/12/2015//Ahmad Raza//Fixed EMR Bug#149
                        } else {
                            IsActiveRecord = "0";
                            Clinical_Allergies.inActiveAllergy(selectedValue, null).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    Clinical_Allergies.allergiesSearch();
                                }
                                else {
                                    utility.DisplayMessages(response.message, 1);
                                }
                            });
                        }




                    }
                }, function () { },
                   '3', null, null, null, IsActive
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//21/12/2015//Ahmad Raza//Implimented Privileges check logic
    },
    //End//Ahmad Raza//02/12/2015//This function will call the inActiveAllergy function after confirming User privileges and taking reasons to inActive the allergy in popup

    //Start//Ahmad Raza//02/12/2015//This function will call deleteAllergy function after confirmation of privileges
    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        //Start//11/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Medical_Allergies", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Clinical_Allergies.deleteAllergy(selectedValue).done(function (response) {
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

                            //Start//28/12/2015//Ahmad Raza//  EMR  Bug# 147 fixed
                            Clinical_Allergies.allergiesSearch();
                            //End//28/12/2015//Ahmad Raza//  EMR  Bug# 147 fixed

                        });
                    }



                }, function () { },
                   '1'
        );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//11/12/2015//Ahmad Raza//Privileges logic implemented
    },
    //End//Ahmad Raza//02/12/2015//This function will call deleteAllergy function after confirmation of privileges

    //Start//Ahmad Raza//02/12/2015//This function is to show row detail in editable mode
    rowDetail: function ($row, ClassName) {
        var currentAllergyId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentAllergyId > 0) {
            //Clinical_Allergies.AllergiesEdit(currentAllergyId);
            Clinical_Allergies.ShowHistory(currentAllergyId);
        }
    },
    rowHistory: function ($row, ClassName) {
        var currentAllergyId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentAllergyId > 0) {
            Clinical_Allergies.ShowHistory(currentAllergyId);
        }
    },
    ShowHistory: function (AllergyId) {
        EMRUtility.showCurrentItemHistory(Clinical_Allergies.params.PanelID, null, AllergyId, "Allergy", Clinical_Allergies.params.patientID, ((Clinical_Allergies.params.ParentCtrl != "clinicalTabProgressNote") && (Clinical_Allergies.params.ParentCtrl != "clinicalTabFaceSheet" && Clinical_Allergies.params.ParentCtrl != "Clinical_FaceSheet")) ? Clinical_Allergies.params.TabID : "Clinical_Allergies", null);
    },
    //End//Ahmad Raza//02/12/2015//This function is to show row detail in editable mode

    //Start//Ahmad Raza//02/12/2015//This function will add row in grid in editable mode
    rowAdd: function () {
        //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
        AppPrivileges.GetFormPrivileges("Medical_Allergies", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd();
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
    },
    //End//Ahmad Raza//02/12/2015//This function will add row in grid in editable mode

    //Start//Ahmad Raza//02/12/2015//This function will call updateAllergiesRow function to update record in DB after checking user Privileges
    rowSave: function ($row, obj) {

        //if (obj.rowValidate($row)) {

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
        //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
        var NotesId = $row.attr("allergynotesid");
        //End//31/12/2015//Ahmad Raza//Bug#178 fixed
        if (id && id > 0) {

            //Edit Record
            var strMessage = "";
            //Start//11/12/2015//Ahmad Raza//Privileges logic implemented
            AppPrivileges.GetFormPrivileges("Medical_Allergies", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Clinical_Allergies.updateAllergiesRow(myJSON, id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            //Start//11/12/2015//Ahmad Raza//Serializing form
                            //    $('#' + Clinical_Allergies.params.PanelID + ' #frmClinicalAllergies').data('serialize', $('#' + Clinical_Allergies.params.PanelID + ' #frmClinicalAllergies').serialize());
                            //End//11/12/2015//Ahmad Raza//Serializing form

                            //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
                            //Start//31/12/2015//Ahmad Raza//Logic to update against current Note only
                            if (Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote" && NotesId.indexOf(Clinical_Allergies.params.NotesId) > -1) {
                                //End//31/12/2015//Ahmad Raza//Logic to update against current Note only
                                Clinical_Allergies.getAllergiesInfo(id);
                            }
                                //End//31/12/2015//Ahmad Raza//Bug#178 fixed
                            else {
                                utility.DisplayMessages(response.message, 1);
                                Clinical_Allergies.allergiesSearch();
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
            //End//11/12/2015//Ahmad Raza//Privileges logic implemented
        }

    },
    //End//Ahmad Raza//02/12/2015//This function will call updateAllergiesRow function to update record in DB after checking user Privileges

    //Start//Ahmad Raza//02/12/2015//This function is to set remove icon as active(visible)
    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
    },
    //End//Ahmad Raza//02/12/2015//This function is to set remove icon as active(visible)

    //Start//Ahmad Raza//02/12/2015//This function is used to build html for child rows in grid
    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div></div>");
        var txtAllergen = "<div class='col-xs-1'><label class='control-label'>Allergen</label><input  class='form-control'type='text' id='txtAllergen" + ParentRowId + "' name='Allergen" + ParentRowId + "'></div>";
        //var txtType = "<div class='col-xs-1  size-min100'><label class='control-label'>Type</label><input class='form-control' id='txtType" + ParentRowId + "' name='Type" + ParentRowId + "' type='text' /></div>";
        var txtReaction = "<div class='col-xs-1  size-min100'><label class='control-label'>Reaction</label><input class='form-control' id='txtReaction" + ParentRowId + "' name='Reaction" + ParentRowId + "' type='text' /></div>";
        //var txtSeverity = "<div class='col-xs-1  size-min100'><label class='control-label'>Severity</label><input class='form-control' id='txtSeverity" + ParentRowId + "' name='Severity" + ParentRowId + "' type='text' /></div>";
        var ddlType = "<div class='col-xs-2'><label class='control-label'>Type</label><select id='ddlAllergenType" + ParentRowId + "' name='AllergenType" + ParentRowId + "' class='form-control'></select></div>";
        var ddlSeverity = "<div class='col-xs-2'><label class='control-label'>Severity</label><select id='ddlSeverity" + ParentRowId + "' name='Severity" + ParentRowId + "' class='form-control'></select></div>";

        //var LineNotes = "<div class='col-xs-2'><label class='control-label'>Line Notes</label><textarea class='form-control' rows='1' id='txtComments" + ParentRowId + "' name='txtComments" + ParentRowId + "'></textarea></div>";
        //var chkHolds = "<div class='col-xs-1 pt-lg'><div class='checkbox-custom checkbox-default'><input type='checkbox' onclick=EncounterChargeCapture.validateIsHold(this,'divHoldDays" + ParentRowId + "') id='chkHold" + ParentRowId + "' value name='chkHold" + ParentRowId + "'/><label class='control-label'>Is Hold</label></div></div>";
        //var HoldDays = "<div id='divHoldDays" + ParentRowId + "' style='display:none' class='col-xs-1'><label class='control-label'>Hold Days</label><input type='text' class='form-control' onfocusout=EncounterChargeCapture.validateHoldDays(this,'chkHold" + ParentRowId + "') id='txtHoldDays" + ParentRowId + "' data-mask='9?99' name='txtHoldDays" + ParentRowId + "'/></div>";
        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(txtAllergen, txtReaction, ddlType, ddlSeverity, spacer);
        return ChildHTML;

    },
    //End//Ahmad Raza//02/12/2015//This function is used to build html for child rows in grid

    //Start//Ahmad Raza//02/12/2015//This function will bind new grid row
    addNewAllergiesRow: function (RowId, mode, CurrRef, NotesId) {

        var CurrentRow = null;
        if (RowId && RowId > 0) {
            //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
            CurrentRow = Clinical_Allergies.EditableGrid.rowAdd(RowId, Clinical_Allergies.params.AllergyId, null, null, null, null, NotesId);
            //DEnd//31/12/2015//Ahmad Raza//Bug#178 fixed
        }
        else {
            var TemplateRow = $("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
            CurrentRow = Clinical_Allergies.EditableGrid.rowAdd(TemplateRowId - 1, Clinical_Allergies.params.AllergyId, null, null, null, null, null);
            //End//31/12/2015//Ahmad Raza//Bug#178 fixed
        }

        var row = Clinical_Allergies.EditableGrid.datatable.row(CurrentRow);
        row.child(Clinical_Allergies.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child.hide();
        Clinical_Allergies.enableRemoveRow($(CurrentRow));
        return CurrentRow;
    },
    //End//Ahmad Raza//02/12/2015//This function will bind new grid row



    //-----------------Progress Note-------------
    // added on Dec 04,2015 by Muhammad Azhar Shahzad
    //Call Back function to add component to Progress Note
    addAllergyToNotes: function () {
        var dfd = $.Deferred();
        //var SelectedAllergies = $("#" + Clinical_Allergies.params.PanelID + ' [name=SelectCheckBoxAllergy]:checked:not(:disabled)').map(function () { return this.id; }).get().join();
        //if (SelectedAllergies != null && SelectedAllergies != '') {
        //    Clinical_Allergies.getAllergiesInfo(SelectedAllergies);
        //}
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Medical_Allergies", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var SelectedAllergies = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
                /*if (SelectedAllergies != null && SelectedAllergies != '') {
                    for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                        var ALid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Allergies_Main' + ALid).length != 0) {
                            var index = SelectedAllergies.indexOf(ALid);
                            if (index > -1) {
                                SelectedAllergies.splice(index, 1);
                            }
                        }
                    }
                }*/

                var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;

                if (detachedvalues.join() != null && detachedvalues.join() != '') {
                    Clinical_Allergies.detachAllergiesFromNotes(detachedvalues).done(function () {
                        if (SelectedAllergies.join() != null && SelectedAllergies.join() != '') {
                            $.when(Clinical_Allergies.attachAllergiesFromNotes(SelectedAllergies)).then(function () {
                                if (Clinical_Allergies.params && Clinical_Allergies.params.ParentCtrl && Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                                    UnloadActionPan(Clinical_Allergies.params.ParentCtrl, 'Clinical_Allergies');
                                }
                                dfd.resolve();
                            });
                        } else {
                            $.when(Clinical_Allergies.AppendReviewedSoapText()).then(function () {
                                $.when(Clinical_ProgressNote.saveComponentSOAPText('Allergies')).then(function () {
                                    if (Clinical_Allergies.params && Clinical_Allergies.params.ParentCtrl && Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                                        UnloadActionPan(Clinical_Allergies.params.ParentCtrl, 'Clinical_Allergies');
                                    }
                                    dfd.resolve();
                                });
                            });
                        }
                    });
                }
                else if (SelectedAllergies.join() != null && SelectedAllergies.join() != '') {
                    $.when(Clinical_Allergies.attachAllergiesFromNotes(SelectedAllergies)).then(function () {
                        if (Clinical_Allergies.params && Clinical_Allergies.params.ParentCtrl && Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                            UnloadActionPan(Clinical_Allergies.params.ParentCtrl, 'Clinical_Allergies');
                        }
                        dfd.resolve();
                    });
                }
                else {
                    $.when(Clinical_Allergies.AppendReviewedSoapText()).then(function () {
                        if (Clinical_Allergies.params && Clinical_Allergies.params.ParentCtrl && Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                            UnloadActionPan(Clinical_Allergies.params.ParentCtrl, 'Clinical_Allergies');
                        }
                        dfd.resolve();
                    });
                }

                //When User has attached Allergies with notes than add on note button should be disabled
                $("#" + Clinical_Allergies.params.PanelID + " #btnAddAllergyToNotes").prop('disabled', true);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
                if (Clinical_Allergies.params && Clinical_Allergies.params.ParentCtrl && Clinical_Allergies.params.ParentCtrl == "clinicalTabProgressNote") {
                    UnloadActionPan(Clinical_Allergies.params.ParentCtrl, 'Clinical_Allergies');
                }
                dfd.resolve();
            }
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
        return dfd;
    },

    attachAllergiesFromNotes: function (attachedAlergiesId) {
        var dfd = $.Deferred();
        Clinical_Allergies.getAllergiesInfo(attachedAlergiesId.join()).done(function () {
            setTimeout(function () {
                $.when(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val())).then(function () {
                    if (Clinical_Allergies.params != null && Clinical_Allergies.params.length > 0 && Clinical_Allergies.params.PanelID.indexOf('pnlClinicalAllergies') != -1) {
                        Clinical_Allergies.allergiesSearch();
                        dfd.resolve();
                    }
                    else {
                        dfd.resolve();
                    }
                });

            }, 5);
        });
        return dfd;
    },

    detachAllergiesFromNotes: function (detachedAlergiesId) {
        var dfd = new $.Deferred();
        Clinical_Allergies.detachAllergiesFromNotes_DBCall(detachedAlergiesId.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedAlergiesId.length; i++) {
                    var ALid = detachedAlergiesId[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Allergies_Main' + ALid).remove();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },

    resetAllergies: function () {
        $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result [name=SelectCheckBoxAllergy]').prop('checked', false);
        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-282
        $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #chkHeaderAllergies').prop('checked', false);
        // End Date  Edit By Muhammad Ahmad Imran Bug # EMR-282
    },

    //this function will get allergies Soap Text and attach that to Progress note
    getAllergiesInfo: function (AllergiesId, hideAlertMessage) {
        var dfd = new $.Deferred();
        if (AllergiesId == null || AllergiesId == '') {
            dfd.response = false;
            dfd.resolve();
        }

        Clinical_Allergies.get_Allergies_ForSOAP_DBCall(AllergiesId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.AllergySoapCount > 0) {
                        $.when(Clinical_Allergies.createAllergyBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', AllergiesId, hideAlertMessage)).then(function () {
                            dfd.resolve('ok');
                        });
                    }
                    else {
                        dfd.resolve('ok');
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    dfd.resolve('ok');
                }
            }
            else {
                dfd.resolve('ok');
            }

        });
        return dfd.promise();
    },

    //This Function will check, if Allergies Soap is already attached in Progress note, if allergies is not attached than it will create main divs to attach allergy
    checkAllergyExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_allergies').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="AllergiesComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_allergies title="Allergies"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Allergies\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Allergies">Allergies</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Allergies\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Allergies\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_allergies> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_allergies').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },
    //This Function is used to create SOAP html and append it to  Progress note
    createAllergyBodyHTMLFromNotes: function (Allergies, NoteHTMLCtrl, AllergiesId, hideAlertMessage,bNotSaveCompt) {
        var dfd = new $.Deferred();
        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-280
        if ($(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main_NKDA').length != 0) {
            $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main_NKDA').remove();
        }
        // End Date  Edit By Muhammad Ahmad Imran Bug # EMR-280
        Clinical_Allergies.checkAllergyExists();
        if (!Allergies || Allergies.length == 0) {
            return "";
        }

        var $mainDivAllergy = $(document.createElement('div'));

        var allergns = "";

        var AListId = [];
        $.each(Allergies, function (index, element) {
            var color = "";


            if (element.Severity == "Mild") {
                color = 'style = "color:green;font-weight:bold"'
            }
            if (element.Severity == "Sever") {
                color = 'style = "color:red;font-weight:bold"'
            }
            if (element.Severity == "Moderate") {
                color = 'style = "color:orange;font-weight:bold"'
            }

            var ALid = element.AllergyId;
            var $SectionBodyAllergy = $(document.createElement('section'));
            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-280
            if (element.Allergen == "No Known Allergies") {
                $SectionBodyAllergy.attr('id', "Cli_Allergies_Main_NKDA");
            }
            else {
                $SectionBodyAllergy.attr('id', "Cli_Allergies_Main" + ALid);
            }
            // End Date  Edit By Muhammad Ahmad Imran Bug # EMR-280
            var $DetailsDiv = $(document.createElement('div'));
            $DetailsDiv.attr('id', "Cli_Allergy_" + ALid);
            var $ListAllergy = $(document.createElement('ul'));

            $ListAllergy.attr('class', 'list-unstyled')

            $SectionBodyAllergy.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Allergy_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Allergies_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
            if (element.IsActive == "True") {
                $ListAllergy.append("<li>" + (element.Allergen === 'No Known Allergies' ? 'No Known Drug Allergies (NKDA)' : "<strong>" + element.Allergen + "</strong>") + (element.Severity == "" ? "" : "(<span " + color + '>' + element.Severity + "</span>) ") +
                ((element.OnSetDate == null || typeof (element.OnSetDate) == undefined || element.OnSetDate == "") ? "" : ", Onset: " + moment(element.OnSetDate).format("MM/DD/YYYY")) +
                ((element.Reaction == null || typeof (element.Reaction) == undefined || element.Reaction == "") ? "" : ", Reaction:  " + element.Reaction)
               );
            } else {
                $ListAllergy.append("<li> <strong>" + element.Allergen + " </strong> " + "(<span style = 'color:red;'>Inactive</span>) " + (element.Severity == "" ? "" : "(<span " + color + '>' + element.Severity + "</span>) ") +

                ((element.OnSetDate == null || typeof (element.OnSetDate) == undefined || element.OnSetDate == "") ? "" : ", Onset: " + moment(element.OnSetDate).format("MM/DD/YYYY")) +
                ((element.Reaction == null || typeof (element.Reaction) == undefined || element.Reaction == "") ? "" : ", Reaction:  " + element.Reaction)
               );
            }

            $ListAllergy.append((element.Comments == ""||element.Comments==null ? "" : "<li>Comments: " + element.Comments));
            $DetailsDiv.append($ListAllergy);


            $SectionBodyAllergy.append($DetailsDiv);
            if ($(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main' + ALid).length == 0) {
                AListId.push(ALid);
                $mainDivAllergy.append($SectionBodyAllergy);
            } else {

                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main' + ALid + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main' + ALid).html($SectionBodyAllergy.html());
                $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main' + ALid + ' ul').append(CommentHTML);;
            }

            allergns = element.Allergen;
        });
        $.when(reviewedString = Clinical_Allergies.GetReviewedAllergyInfo()).then(function () {


            if (reviewedString.response != "") {
                if ($(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#ReviewByAllergy').length == 0) {
                    $mainDivAllergy.append("<section id='Cli_Allergies_ReviewByAllergy'><li id='ReviewByAllergy'>" + reviewedString.response + "</li></section>");
                }
                else {
                    $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#ReviewByAllergy').remove();
                    $mainDivAllergy.append("<section id='Cli_Allergies_ReviewByAllergy'><li id='ReviewByAllergy'>" + reviewedString.response + "</li></section>");
                }
            }
            //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
            if (AListId.join(",") != "") {
                AllergiesId = AListId.join(",");
            }
            var AllergyHtml = $mainDivAllergy.html();
            if (AllergyHtml) {
                $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().addClass('initialVisitBody');

                $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().append(AllergyHtml);


                //Binding Hovering and onClick functions to Progress Note HTML
                Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
                return AllergiesId;
            }
            dfd.resolve();

        });


        return dfd;
    },



    //This Function is used to create SOAP html and append it to  Progress note
    createAllergyBodyHTML: function (response, NoteHTMLCtrl, AllergiesId, hideAlertMessage) {
        var dfd = new $.Deferred();
        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-280
        if ($(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main_NKDA').length != 0) {
            $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main_NKDA').remove();
        }
        // End Date  Edit By Muhammad Ahmad Imran Bug # EMR-280
        Clinical_Allergies.checkAllergyExists();
        if (response.AllergySoapCount > 0) {
            var Allergiesoap_JSON = JSON.parse(response.AllergySoap_JSON);
            var $mainDivAllergy = $(document.createElement('div'));

            var allergns = "";
            if (Allergiesoap_JSON == null || Allergiesoap_JSON.length == 0) {
                return "";
            }

            var AListId = [];
            $.each(Allergiesoap_JSON, function (index, element) {
                var color = "";


                if (element.Severity == "Mild") {
                    color = 'style = "color:green;font-weight:bold"'
                }
                if (element.Severity == "Sever") {
                    color = 'style = "color:red;font-weight:bold"'
                }
                if (element.Severity == "Moderate") {
                    color = 'style = "color:orange;font-weight:bold"'
                }

                var ALid = element.AllergyId;
                var $SectionBodyAllergy = $(document.createElement('section'));
                // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-280
                if (element.Allergen == "No Known Allergies") {
                    $SectionBodyAllergy.attr('id', "Cli_Allergies_Main_NKDA");
                }
                else {
                    $SectionBodyAllergy.attr('id', "Cli_Allergies_Main" + ALid);
                }
                // End Date  Edit By Muhammad Ahmad Imran Bug # EMR-280
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Allergy_" + ALid);
                var $ListAllergy = $(document.createElement('ul'));

                $ListAllergy.attr('class', 'list-unstyled')
                if (element.Allergen == "No Known Allergies") {
                    $SectionBodyAllergy.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Allergy_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                                            '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="Cli_Allergies_Main_NKDA"><i class="fa fa-times"></i></a></div> ');

                }
                else {
                    $SectionBodyAllergy.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Allergy_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                        '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Allergies_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
                }

                if (element.IsActive == "True") {
                    $ListAllergy.append("<li>" + (element.Allergen === 'No Known Allergies' ? 'No Known Drug Allergies (NKDA)' : "<strong>" + element.Allergen + "</strong>") + (element.Severity == "" ? "" : "(<span " + color + '>' + element.Severity + "</span>) ") +

                    ((element.OnSetDate == null || typeof (element.OnSetDate) == undefined || element.OnSetDate == "") ? "" : ", Onset: " + moment(element.OnSetDate).format("MM/DD/YYYY")) +
                    ((element.Reaction == null || typeof (element.Reaction) == undefined || element.Reaction == "") ? "" : ", Reaction:  " + element.Reaction)
                    );
                } else {
                    $ListAllergy.append("<li> <strong>" + element.Allergen + " </strong> " + "(<span style = 'color:red;'>Inactive</span>) " + (element.Severity == "" ? "" : "(<span " + color + '>' + element.Severity + "</span>) ") +
                       ((element.OnSetDate == null || typeof (element.OnSetDate) == undefined || element.OnSetDate == "") ? "" : ", Onset: " + moment(element.OnSetDate).format("MM/DD/YYYY")) +
                       ((element.Reaction == null || typeof (element.Reaction) == undefined || element.Reaction == "") ? "" : ", Reaction:  " + element.Reaction)
                   );
                }

                $ListAllergy.append((element.Comments == "" ? "" : "<li>Comments: " + element.Comments));
                $DetailsDiv.append($ListAllergy);


                $SectionBodyAllergy.append($DetailsDiv);
                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_allergies').parent().parent().find('#Cli_Allergies_Main' + ALid).length == 0) {
                if ($(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main' + ALid).length == 0) {
                    AListId.push(ALid);
                    $mainDivAllergy.append($SectionBodyAllergy);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main' + ALid).html($SectionBodyAllergy.html());
                    $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#Cli_Allergies_Main' + ALid + ' ul').append(CommentHTML);;
                }

                allergns = element.Allergen;
            });
            $.when(reviewedString = Clinical_Allergies.GetReviewedAllergyInfo()).then(function () {


                if (reviewedString.response != "") {
                    if ($(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#ReviewByAllergy').length == 0) {
                        $mainDivAllergy.append("<section id='Cli_Allergies_ReviewByAllergy'><li id='ReviewByAllergy'>" + reviewedString.response + "</li></section>");
                    }
                    else {
                        $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#ReviewByAllergy').remove();
                        $mainDivAllergy.append("<section id='Cli_Allergies_ReviewByAllergy'><li id='ReviewByAllergy'>" + reviewedString.response + "</li></section>");
                    }
                }
                //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                if (AListId.join(",") != "") {
                    AllergiesId = AListId.join(",");
                }
                //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                if ($mainDivAllergy.html() != '') {
                    $.when(Clinical_Allergies.updateAllergyHtml($mainDivAllergy.html(), AllergiesId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                        dfd.resolve();
                    });
                } else {
                    $.when(Clinical_Allergies.updateAllergyHtml('', AllergiesId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                        Clinical_ProgressNote.saveComponentSOAPText('Allergies', true);
                        dfd.resolve();
                    });
                }


            });

        } else {
            if (typeof response.AllergyReviewCount != typeof undefined && response.AllergyReviewCount != null && response.AllergyReviewCount > 0) {
                var $mainDivAllergy = $(document.createElement('div'));
                $.when(reviewedString = Clinical_Allergies.GetReviewedAllergyInfo()).then(function () {


                    if (reviewedString.response != "") {
                        if ($(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#ReviewByAllergy').length == 0) {
                            $mainDivAllergy.append("<section id='Cli_Allergies_ReviewByAllergy'><li id='ReviewByAllergy'>" + reviewedString.response + "</li></section>");
                        }
                        else {
                            $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#ReviewByAllergy').remove();
                            $mainDivAllergy.append("<section id='Cli_Allergies_ReviewByAllergy'><li id='ReviewByAllergy'>" + reviewedString.response + "</li></section>");
                        }
                    }

                    //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if ($mainDivAllergy.html() != '') {
                        $.when(Clinical_Allergies.updateAllergyHtml($mainDivAllergy.html(), "", NoteHTMLCtrl, hideAlertMessage)).then(function () {
                            dfd.resolve();
                        });
                    } else {
                        $.when(Clinical_Allergies.updateAllergyHtml('', "", NoteHTMLCtrl, hideAlertMessage)).then(function () {
                            Clinical_ProgressNote.saveComponentSOAPText('Allergies', true);
                            dfd.resolve();
                        });
                    }


                });
            }
            else {
                $.when(Clinical_ProgressNote.saveComponentSOAPText('Allergies', true)).then(function () {
                    dfd.resolve();
                });
            }
        }
        return dfd;
    },

    AppendReviewedSoapText: function () {
        var dfd = $.Deferred();
        if ($("#" + Clinical_Allergies.params.PanelID + " #reviewedByTop").text() != "") {
            Clinical_Allergies.checkAllergyExists();
            var NoteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
            var $mainDivAllergy = $(document.createElement('div'));
            $.when(reviewedString = Clinical_Allergies.GetReviewedAllergyInfo()).then(function () {
                if (reviewedString.response != "") {
                    if ($(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#ReviewByAllergy').length == 0) {
                        $mainDivAllergy.append("<section id='Cli_Allergies_ReviewByAllergy'><li id='ReviewByAllergy'>" + reviewedString.response + "</li></section>");
                    }
                    else {
                        $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().find('#ReviewByAllergy').remove();
                        $mainDivAllergy.append("<section id='Cli_Allergies_ReviewByAllergy'><li id='ReviewByAllergy'>" + reviewedString.response + "</li></section>");
                    }
                }

                //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                if ($mainDivAllergy.html() != '') {
                    $.when(Clinical_Allergies.updateAllergyHtml($mainDivAllergy.html(), "", NoteHTMLCtrl, false)).then(function () {
                        $.when(Clinical_ProgressNote.saveComponentSOAPText('Allergies')).then(function () {
                            dfd.resolve();
                        });
                    });

                } else {
                    $.when(Clinical_Allergies.updateAllergyHtml('', "", NoteHTMLCtrl, true)).then(function () {
                        Clinical_ProgressNote.saveComponentSOAPText('Allergies', true);
                        dfd.resolve();
                    });
                }
            });
        }
        else {
            dfd.resolve();
        }

        return dfd;
    },

    // This Function is called by Progress Notes (Fill Allergys Func, CopyAllNotesCategories)
    updateAllergyHtml: function (AllergyHtml, AllergyId, NoteHTMLCtrl, hideAlertMessage) {
        var dfd = new $.Deferred();
        $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().addClass('initialVisitBody');
        if (AllergyHtml != '') {
            $(NoteHTMLCtrl + ' clinical_allergies').parent().parent().append(AllergyHtml);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (AllergyHtml != '' && AllergyId != null && AllergyId != '') {
            $.when(Clinical_Allergies.attachAllergyignFromNotes(AllergyId, hideAlertMessage)).then(function () {
                dfd.resolve();
            });
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },
    //This Function detach Allergy From progress note

    detach_ComponentsAllergy: function (ComponentName, IsUpdate, AllergyComponentRemove) {
        var Clinical_AllergyIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_allergies').parent().parent().find('section[id*="Cli_Allergies_Main"]').map(function () {
            return this.id.replace("Cli_Allergies_Main", "");
        }).get().join(',');
        if (Clinical_AllergyIds == "_NKDA") {
            Clinical_AllergyIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_allergies').parent().parent().find('div[id*="Cli_Allergy_"]').map(function () {
                return this.id.replace("Cli_Allergy_", "");
            }).get().join(',');
        }
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .AllergiesComponent').attr('NoteComponentId');

        if (AllergyComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_allergies').parent().parent().addClass('hidden');
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .AllergiesComponent').find('section[id*="Cli_Allergies_Main"]').remove();
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Allergies', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_allergies').parent().parent().remove();
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .AllergiesComponent').find('section[id*="Cli_Allergies_Main"]').remove();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            } else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Allergies']").remove();
                if (Clinical_ProgressNote.params["TemplateName"])
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_allergies').parent().parent().addClass('hidden');
                else
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_allergies').parent().parent().remove();
            }
        }
        else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_allergies').parent().parent().find('section[id*="Cli_Allergies_Main"]').remove();
        }
        Clinical_AllergyIds = Clinical_AllergyIds.replace("_NKDA", "");
        if (Clinical_AllergyIds == "" || Clinical_AllergyIds == "undefined") {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_allergies').parent().parent().addClass('hidden');
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .AllergiesComponent').find('section[id*="Cli_Allergies_Main"]').remove();
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Allergies', true))
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
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_allergies').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                utility.DisplayMessages('Successfully Deleted', 1);
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            });
        }
        else {
            Clinical_Allergies.detachAllergiesFromNotes_DBCall(Clinical_AllergyIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText('Allergies', true);
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

    //This Functions ask for Detaching Allergy sign from Progress Note for current Patient Selected
    detachAllergyFromNotes: function (AllergyId) {
        var strMessage = "";
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Medical_Allergies", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    EMRUtility.scrollToPNcomponent('clinical_allergies');
                    var selectedValue = AllergyId.replace('Cli_Allergies_Main', '');
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        if (selectedValue == "_NKDA") {
                            selectedValue = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML a[name="Cli_Allergies_Main_NKDA"]').parent().parent().parent().find("section#Cli_Allergies_Main_NKDA").find('div[id*="Cli_Allergy_"]').attr("id").replace("Cli_Allergy_", "");
                        }
                        Clinical_Allergies.detachAllergiesFromNotes_DBCall(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                $('#' + AllergyId).remove();
                                Clinical_ProgressNote.saveComponentSOAPText('Allergies');
                                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 15);
                                //   utility.DisplayMessages(response.Message, 1);
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
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    //This Functions attached Allergy sign to Progress Note for current Patient Selected
    attachAllergyignFromNotes: function (AllergyId, hideAlertMessage) {
        var dfd = new $.Deferred();
        Clinical_Allergies.attachAllergiesWithNotes_DBCall(AllergyId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //var attachedAllergys = JSON.parse(response.AllergysLoad_JSON);

                //If Attached Allergys Made new inseration to Allergys Table than good ids should be attached to HTML
                // Clinical_ProgressNote.ChangeHTML(response);
                $.when(Clinical_ProgressNote.saveComponentSOAPText('Allergies', true)).then(function () {
                    dfd.resolve();
                });
                // Grid row was removing which was attaching to Note
                //    $('#' + AllergyId).remove();

                // utility.DisplayMessages(response.Message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },

    //This Function enable/disable add to note button
    enableAddAllergiesWithNotes: function (obj) {

        if ($(obj).is(':checked')) {

            if ($.inArray(obj.id, Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id);
            } if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id);
                if (index > -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {
            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-283
            $("#" + Clinical_Allergies.params.PanelID + ' #pnlAllergies_Result #chkHeaderAllergies').prop('checked', false);
            // End Date  Edit By Muhammad Ahmad Imran Bug # EMR-283
            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id);
            }
        }
        if (Clinical_ProgressNote.AttachedNoteComponentIds.length > 0 || Clinical_ProgressNote.DetachedNoteComponentIds.length > 0) {
            $("#" + Clinical_Allergies.params.PanelID + " #btnAddAllergyToNotes").prop('disabled', false);
        } else {
            $("#" + Clinical_Allergies.params.PanelID + " #btnAddAllergyToNotes").prop('disabled', true);
        }
    },

    //If Allergies Component which is dropeed in Progress note has no Allergies attached, than it will call for Latest Allergies for this patient
    getLatestAllergyByPatientId: function (AllergyId, hideAlertMessage, droppedComponent) {
        var dfd = $.Deferred();
        Clinical_Allergies.getLatestClinical_AllergyByPatientId_DBCall(AllergyId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Allergies.createAllergyBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', '', hideAlertMessage);
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
            dfd.resolve();
        });
        return dfd;
    },

    getRcopiaInformaionForAllergySoap: function (AllergyId) {
        var dfd = $.Deferred();
        $.when(Clinical_Allergies.getLatestAllergyByPatientId(AllergyId)).then(function () {
            dfd.resolve();
        });
        return dfd;
    },
    //-----Server calls of Notes----------
    attachAllergiesWithNotes_DBCall: function (AllergyId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["AllergyId"] = AllergyId;
        objData["commandType"] = "attach_allergies_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");
    },

    detachAllergiesFromNotes_DBCall: function (AllergyId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["AllergyId"] = AllergyId;
        objData["commandType"] = "detach_allergies_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");
    },

    get_Allergies_ForSOAP_DBCall: function (AllergiesId) {
        var objData = new Object();
        objData["AllergyId"] = AllergiesId;
        //objData["PatientId"] = Clinical_Allergies.params.patientID;
        objData["commandType"] = "get_Allergies_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");
    },

    getLatestClinical_AllergyByPatientId_DBCall: function (AllergyId) {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        if (AllergyId == "" || (typeof AllergyId === "undefined")) {
            objData["AllergyId"] = "";
        } else {
            objData["AllergyId"] = AllergyId;
        }
        objData["UserId"] = globalAppdata["AppUserId"];
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["commandType"] = "getlatest_allergiesby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");
    },
    //--------------end progress Note-----------
    //Wasim Malik......Show reviewed on the top of allergies
    insertReviewedInfoOnTop: function (allergyReview_JSON) {
        var AllergySOAPJSON = JSON.parse(allergyReview_JSON);
        if (AllergySOAPJSON.length > 0) {
            var dateFormat = AllergySOAPJSON[0].ReviewedOn;
            var Day = new Date(dateFormat);
            var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);

            $("#" + Clinical_Allergies.params.PanelID + " #reviewedByTop").html((AllergySOAPJSON[0].ReviewedBy == '' ? "" : "Allergies reviewed by " + AllergySOAPJSON[0].ReviewedBy) +
                " on " + utility.GetDayNameFromDayNumber(Day.getDay()) + ", " + ReviewedOndateFormat);
        }

    },
    GetReviewedAllergyInfo: function () {
        var dfd = $.Deferred();
        Clinical_Allergies.GetReviewedAllergyInfo_DB_CALL().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.AllergyReviewTotalCount > 0) {
                    var AllergySOAPJSON = JSON.parse(response.allergyReview_JSON);
                    var dateFormat = AllergySOAPJSON[0].ReviewedOn;
                    var Day = new Date(dateFormat);
                    var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);

                    dfd.response = (AllergySOAPJSON[0].ReviewedBy == '' ? "" : "Allergies reviewed by " + AllergySOAPJSON[0].ReviewedBy) +
                        " on " + utility.GetDayNameFromDayNumber(Day.getDay()) + ", " + ReviewedOndateFormat;
                    dfd.resolve();
                }
                else {
                    dfd.response = "";
                    dfd.resolve();
                }
            }
            else {
                dfd.response = "";
                utility.DisplayMessages(strMessage, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    GetReviewedAllergyInfo_DB_CALL: function () {
        var objData = new Object();
        if (Clinical_Allergies.params.patientID != null && typeof Clinical_Allergies.params.patientID != typeof undefined) {
            objData["PatientId"] = Clinical_Allergies.params.patientID;
        }
        else {
            objData["PatientId"] = Clinical_Allergies.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        }
        objData["commandType"] = "loadAllergiesReviewedBy";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");
    },
}