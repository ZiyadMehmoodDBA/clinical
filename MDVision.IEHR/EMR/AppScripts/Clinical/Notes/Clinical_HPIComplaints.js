
Clinical_HPIComplaints = {
    bIsFirstLoad: true,
    params: [],
    Complaints: [],
    ComplaintsDetail: [],
    ComplaintsDetailForDx: [],
    ComplaintSelectedPrevious: 0,
    ComplaintSelectedCurrent: 0,
    FavComplaintsId: -1000,
    FavComplaints: [],
    IsDurationValidation: true,
    ValidationErrorMessage: "",
    IsUpdate: false,
    IsDelete: false,
    WhichIdIsClick: "",
    controlToInvoke: null,
    IsReset: false,
    ResetedComplaitId: 0,
    FavListName: "Complaint",
    LastSctBaseSearch: '',
    GlobalDate: "",
    GlobalOverAllComment: "",
    selectedHPITempData: [],
    selectedSymptoms: [],
    selectedSymptomId: 0,
    selectedTemplateSymptomId: 0,
    selectedFindings: [],
    NewInsertedId: -1,
    Content_style: null,
    Load: function (params) {

        Clinical_HPIComplaints.Complaints = [];
        Clinical_HPIComplaints.ComplaintsDetail = [];
        Clinical_HPIComplaints.ComplaintSelectedPrevious = 0;
        Clinical_HPIComplaints.ComplaintSelectedCurrent = 0;
        Clinical_HPIComplaints.IsDelete = false;
        Clinical_HPIComplaints.FavComplaintsId = -1000;
        Clinical_HPIComplaints.LastSctBaseSearch = '';
        Clinical_HPIComplaints.selectedHPITempData = [];
        Clinical_HPIComplaints.selectedSymptoms = [];
        Clinical_HPIComplaints.selectedFindings = [];
        Clinical_HPIComplaints.ComplaintId = 0;


        Clinical_HPIComplaints.params = params;

        if (Clinical_HPIComplaints.params.mode != "Edit") {
            Clinical_HPIComplaints.params.mode = "Add";
        }
        Clinical_HPIComplaints.pComplaintIndex = 0;
        if (Clinical_HPIComplaints.params.PanelID != 'pnlClinicalHPIComplaints') {
            Clinical_HPIComplaints.params.PanelID = Clinical_HPIComplaints.params.PanelID + ' #pnlClinicalHPIComplaints';
        } else {
            Clinical_HPIComplaints.params.PanelID = 'pnlClinicalHPIComplaints';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_HPIComplaints.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        utility.CreateDatePicker(Clinical_HPIComplaints.params.PanelID + '  #dtComplaintDate', function () {
        }, true);
        Clinical_HPIComplaints.domReadyFunc();

        if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote") {
            // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_HPIComplaints.params.PanelID, 'Medical', 'Complaints', 'Clinical_HPIComplaints.unLoadTab(true);', null, true);
            // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491

        }
        $('#' + Clinical_HPIComplaints.params.PanelID + " #txtComplaintId").val(0);
        var self = $('#' + Clinical_HPIComplaints.params.PanelID);
        self.loadDropDowns(true).done(function () {
            var data = "IsActive=1"
            if (Clinical_HPIComplaints.params.CurrentNotesProviderId && Clinical_HPIComplaints.params.CurrentNotesProviderId != "undefined")
                data = data + "&StrID=" + Clinical_HPIComplaints.params.CurrentNotesProviderId;
            self.find('#ddlFavComplaint').attr('ddlist', 'GetFavComplaint');
            $.when(self.find('#favSectionDiv').loadDropDowns(true, data), $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails').loadDropDowns(true)).done(function () {

                EMRUtility.setFavoriteSectionStyle(Clinical_HPIComplaints.params.PanelID);
                Clinical_HPIComplaints.loadCiefComplaintComponent(null);
                Clinical_HPIComplaints.IntializeMultiSelectDropDown();

                //Start 2-07-2016 M Ahmad Imran for favorite list setting for all favLists
                if (EMRUtility.getFavListStatus(Clinical_HPIComplaints.FavListName)) {
                    $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #favSectionDiv").addClass("toggledHor");
                    $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #FormDiv").addClass("toggleHorContainer");
                }
                else {
                    $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #favSectionDiv").removeClass("toggledHor");
                    $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #FormDiv").removeClass("toggleHorContainer");
                }

                //End 2-07-2016 M Ahmad Imran for favorite list setting for all favLists
                Clinical_HPIComplaints.SetFavListVal($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlFavComplaint'));

                var objDeff1 = $.Deferred();
                var objDeff2 = $.Deferred();
                var objDeff3 = $.Deferred();
                objDeff1 = Clinical_HPIComplaints.buildSymptomsAutoComplete();
                objDeff2 = Clinical_HPIComplaints.buildTemplateAutoComplete();
                objDeff3 = Clinical_HPIComplaints.buildFindingsAutoComplete();
                Clinical_HPIComplaints.bindDetailsChangeEvents();

                Clinical_HPIComplaints.changeHPITemplate();
                var TempIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Complaints').parent().parent().find('section[id*="Cli_HPIComplaints_Temp"]').map(function () {
                    return this.id.replace("Cli_HPIComplaints_Temp", "");
                }).get().join(',');
                if (Clinical_HPIComplaints.params.HPITemplateId && Clinical_HPIComplaints.params.mode == "Edit") {
                    Clinical_HPIComplaints.changeHPITemplate();
                    $.when(objDeff1, objDeff2, objDeff3).then(function () {
                        Clinical_HPIComplaints.FillHPITemplates(Clinical_HPIComplaints.params.HPITemplateId);
                        $('#' + Clinical_ProgressNote.params["PanelID"] + " #radioTemplate").attr('checked', 'checked');
                        Clinical_HPIComplaints.changeHPITemplate();
                    });
                }
                Clinical_HPIComplaints.changeICDField();
            });
        });

        if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_HPIComplaints.params.PanelID + " div#FaceSheetPager", Clinical_HPIComplaints.params.FaceSheetComponents, 'Complaints');
        }

        if (EMRUtility.getFreeTextStatus("Clinical_HPIComplaints")) {
            var panel = "#pnlClinicalHPIComplaints";
            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalHPIComplaints";
            }

            $(panel + " #txtComplaints").addClass('hidden');
            $(panel + " #btnSearchCPT").addClass('hidden');
            if ($(panel + " #txtFreeText").hasClass("hidden")) {
                $(panel + " #txtFreeText").removeClass("hidden");
            }
            $(panel + " #radioFreetext").attr('checked', 'checked');
        }
        else {
            var panel = "#pnlClinicalHPIComplaints";
            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalHPIComplaints";
            }

            $(panel + " #txtComplaints").removeClass('hidden');
            $(panel + " #btnSearchCPT").removeClass('hidden');
            if (!$(panel + " #txtFreeText").hasClass("hidden")) {
                $(panel + " #txtFreeText").addClass("hidden");
            }
            $(panel + " #radioProcedure").attr('checked', 'checked');
        }

    },
    SetFavListVal: function ($ddl) {

        var FavOptionLength = $("#" + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ddlFavComplaint option").length;
        if (FavOptionLength > 1) {
            EMRUtility.getFavListValue(Clinical_HPIComplaints.FavListName).done(function (response1) {
                response1 = JSON.parse(response1);
                if (response1.status != false) {
                    if (response1.favListVal != "") {
                        if ($("#" + Clinical_HPIComplaints.params.PanelID + " #ddlFavComplaint option[value='" + response1.favListVal + "']").length > 0) {
                            $ddl.val(response1.favListVal);
                            $ddl.trigger("onchange");
                        }
                        else {
                            if (FavOptionLength == 2) {
                                $ddl.val($("#" + Clinical_HPIComplaints.params.PanelID + " #ddlFavComplaint option:nth-child(2)").val());
                                $ddl.trigger("onchange");
                            }
                            else if (FavOptionLength > 2) {
                                $ddl.trigger("onchange");
                            }
                            else {
                                $ddl.trigger("onchange");
                            }
                        }
                    }
                    else {
                        if (FavOptionLength == 2) {
                            $ddl.val($("#" + Clinical_HPIComplaints.params.PanelID + " #ddlFavComplaint option:nth-child(2)").val());
                            $ddl.trigger("onchange");
                        }
                        else if (FavOptionLength > 2) {
                            $ddl.trigger("onchange");
                        }
                        else {
                            $ddl.trigger("onchange");
                        }
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    //Start//17/02/2016//M Ahmad Imran//Implimented ready function which run at load time
    domReadyFunc: function () {

        $(document).ready(function () {

            $('.toggleHorSmallLeft section').click(function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));

            });
            $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').on("keyup", function (e) {

                if (e.keyCode == 190 || e.keyCode == 110) // dot key is pressed
                {
                    e.preventDefault();
                    EMRUtility.pasteHtmlAtCaret('<span id=marker></span>');
                    if (EMRUtility.callAutopopulationOrNot(Clinical_HPIComplaints.params.PanelID, "txtComment")) {
                        $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').focus();
                        EMRUtility.AutoKeyWordPopulateForDiv(Clinical_HPIComplaints.params.PanelID, "txtComment", "Complaints", 0);
                    }
                    else {
                        $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').find("#marker").remove();
                    }
                }
            });
        });

        $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails').on('keydown', '#txtDuration', function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });

        $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails').on('input', function () {
            Clinical_HPIComplaints.IsUpdate = true;
        });
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #chkChiefComplaints').on('change', function () {
            Clinical_HPIComplaints.IsUpdate = true;
        });
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation').on('change', function () {
            Clinical_HPIComplaints.IsUpdate = true;
        });
        $('#' + Clinical_HPIComplaints.params.PanelID).on('click', function (event) {

            Clinical_HPIComplaints.WhichIdIsClick = event.target.id;
        });

    },
    //Start//17/02/2016//M Ahmad Imran//Implimented Duration Validation
    ChangeClickId: function (id) {
        Clinical_HPIComplaints.WhichIdIsClick = id;
    },
    CheckDurationSelect: function () {
        Clinical_HPIComplaints.WhichIdIsClick = "ddlDuration"
        if ($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlDuration').val() != "0") {
            if ($('#' + Clinical_HPIComplaints.params.PanelID + ' #txtDuration').val() == "") {
                if (Clinical_HPIComplaints.WhichIdIsClick != "txtDuration") {
                    Clinical_HPIComplaints.IsDurationValidation = false;
                    Clinical_HPIComplaints.ValidationErrorMessage = "Enter Value In Duration";
                    utility.DisplayMessages("Enter Value In Duration", 3);
                    $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtDuration').focus();
                }
                else {
                    Clinical_HPIComplaints.IsDurationValidation = true;
                    Clinical_HPIComplaints.ValidationErrorMessage = "";
                }
            }
            else {
                Clinical_HPIComplaints.IsDurationValidation = true;
                Clinical_HPIComplaints.ValidationErrorMessage = "";
            }
        }
        else {
            Clinical_HPIComplaints.IsDurationValidation = true;
            Clinical_HPIComplaints.ValidationErrorMessage = "";
        }


    },
    CheckDuration: function () {
        setTimeout(function () {
            if ($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlDuration').val() != "0") {
                if ($('#' + Clinical_HPIComplaints.params.PanelID + ' #txtDuration').val() == "") {
                    if (Clinical_HPIComplaints.WhichIdIsClick != "txtDuration" && Clinical_HPIComplaints.WhichIdIsClick != "ddlDuration") {
                        $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtDuration').focus();
                        Clinical_HPIComplaints.IsDurationValidation = false;
                        Clinical_HPIComplaints.ValidationErrorMessage = "Enter Value In Duration";
                        utility.DisplayMessages("Enter Value In Duration", 3);
                    }
                    else {
                        Clinical_HPIComplaints.IsDurationValidation = true;
                        Clinical_HPIComplaints.ValidationErrorMessage = "";
                    }
                }
                else {
                    Clinical_HPIComplaints.IsDurationValidation = true;
                    Clinical_HPIComplaints.ValidationErrorMessage = "";
                }
            }
            else {
                if ($('#' + Clinical_HPIComplaints.params.PanelID + ' #txtDuration').val() != "") {
                    //$('#' + Clinical_HPIComplaints.params.PanelID + ' #txtDuration').addEventListener('click', $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlDuration').focus(), false);
                    if (Clinical_HPIComplaints.WhichIdIsClick != "ddlDuration") {
                        //if (Clinical_HPIComplaints.WhichIdIsClick != "txtDuration") {
                        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlDuration').focus();
                        Clinical_HPIComplaints.clickOnDuration = false;
                        Clinical_HPIComplaints.IsDurationValidation = false;
                        Clinical_HPIComplaints.ValidationErrorMessage = "Select Duration";
                        utility.DisplayMessages("Select Duration", 3);
                    }
                    else {
                        Clinical_HPIComplaints.IsDurationValidation = true;
                        Clinical_HPIComplaints.ValidationErrorMessage = "";
                    }


                }
                else {
                    Clinical_HPIComplaints.IsDurationValidation = true;
                    Clinical_HPIComplaints.ValidationErrorMessage = "";
                }

            }
        }, 500);
    },
    CheckDurationSelectFocusOut: function () {
        setTimeout(function () {
            if ($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlDuration').val() != "0" && !($('#' + Clinical_HPIComplaints.params.PanelID + ' #txtDuration').is(':focus'))) {
                if ($('#' + Clinical_HPIComplaints.params.PanelID + ' #txtDuration').val() == "") {
                    if (Clinical_HPIComplaints.WhichIdIsClick != "txtDuration") {
                        $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtDuration').focus();
                        Clinical_HPIComplaints.IsDurationValidation = false;
                        Clinical_HPIComplaints.ValidationErrorMessage = "Enter Value In Duration";
                        utility.DisplayMessages("Enter Value In Duration", 3);
                    }
                    else {
                        Clinical_HPIComplaints.IsDurationValidation = true;
                        Clinical_HPIComplaints.ValidationErrorMessage = "";
                    }
                }
                else {
                    Clinical_HPIComplaints.IsDurationValidation = true;
                    Clinical_HPIComplaints.ValidationErrorMessage = "";
                }
            }
            else {
                if ($('#' + Clinical_HPIComplaints.params.PanelID + ' #txtDuration').val() != "") {
                    //$('#' + Clinical_HPIComplaints.params.PanelID + ' #txtDuration').addEventListener('click', $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlDuration').focus(), false);
                    //if (Clinical_HPIComplaints.WhichIdIsClick != "ddlDuration") {
                    if (Clinical_HPIComplaints.WhichIdIsClick != "txtDuration") {
                        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlDuration').focus();
                        Clinical_HPIComplaints.clickOnDuration = false;
                        Clinical_HPIComplaints.IsDurationValidation = false;
                        Clinical_HPIComplaints.ValidationErrorMessage = "Select Duration";
                        utility.DisplayMessages("Select Duration", 3);
                    }
                    else {
                        Clinical_HPIComplaints.IsDurationValidation = true;
                        Clinical_HPIComplaints.ValidationErrorMessage = "";
                    }

                }
                else {
                    Clinical_HPIComplaints.IsDurationValidation = true;
                    Clinical_HPIComplaints.ValidationErrorMessage = "";
                }

            }
        }, 500);
    },
    //End M Ahmad Imran 17/02/2016
    IntializeMultiSelectDropDown: function () {


        $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails #ddlLocation').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247

        });

        $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails #ddlLocation').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247

        });
    },
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

        $('#pnlClinicalHPIComplaints #txtComplaints').attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";
        //params["ParentCtrl"] = Clinical_HPIComplaints.params["TabID"];
        if (Clinical_HPIComplaints.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Clinical_HPIComplaints';
        }

        else {
            params["ParentCtrl"] = Clinical_HPIComplaints.params["TabID"];
        }


        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Clinical_HPIComplaints.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }
    },
    BindICD9AutoComplete: function (element) {
        if ($(element).val().length > 3) {
            if ($(element).val().substring(0, 3).toLowerCase() == "sct") {
                Clinical_HPIComplaints.LastSctBaseSearch = $(element).val().substring(3, $(element).val().length);
            }
            else {
                Clinical_HPIComplaints.LastSctBaseSearch = "";
            }
        }
        else {
            Clinical_HPIComplaints.LastSctBaseSearch = "";
        }
        var descriptionCrtl = $(element);
        $('#pnlClinicalHPIComplaints #txtComplaints').attr("data-popupunload", "false");
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_HPIComplaints", null, false);

    },
    fillChiefComplaints: function (obj, ev) {

        if (!Clinical_HPIComplaints.IsDurationValidation) {
            //utility.DisplayMessages(Clinical_HPIComplaints.ValidationErrorMessage, 3);
        }
        else {
            Clinical_HPIComplaints.IsDurationValidation = true;
            Clinical_HPIComplaints.ValidationErrorMessage = "";
            if (ev != null) {
                ev.stopPropagation();
            }

            $("#" + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints").bootstrapValidator('resetForm', true);
            var diseaseId = $(obj).attr('id');

            //$('#' + Clinical_HPIComplaints.params.PanelID + ' #frmClinicalComplaints #btnMedicalHxSave').hide();
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #sectionComplaintDetails").removeClass('disableAll');

            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints ul#ulCompliantDisease li").each(function (i, item) {
                if ($(this).attr("id") != null && $(this).attr("id") == diseaseId) {
                    if ($(this).hasClass("active") == false) {
                        $(this).addClass("active");
                        //$(this).find('div').css('display', '');
                    }
                }
                else {
                    $(this).removeClass("active");
                    //$(this).find('div').css('display', 'none');
                }
            });

            //Clinical_HPIComplaints.loadCiefComplaintComponent();

            Clinical_HPIComplaints.SaveComplaintDetailInJsonArray();



            //alert('called');
        }

    },
    //Start//12/02/2016//M Ahmad Imran//Implimented Save Complaint Detail info in json array and bind it
    SaveComplaintDetailInJsonArray: function () {

        var SelectedComplaintId = $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li.active").attr("id") != "" ? $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li.active").attr("id") : 0;
        var NotFoundForPre = true;
        var NotFoundForCur = true;
        if (Clinical_HPIComplaints.ComplaintSelectedPrevious == 0) {
            Clinical_HPIComplaints.ComplaintSelectedPrevious = SelectedComplaintId;
            Clinical_HPIComplaints.ComplaintSelectedCurrent = SelectedComplaintId;


            $.grep(Clinical_HPIComplaints.ComplaintsDetail, function (item, index) {
                if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedPrevious) {
                    var self = $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails');
                    utility.bindMyJSONByName(true, item, false, self).done(function () {
                        if (item.Complaint_LocationIds != '') {
                            $('#' + Clinical_HPIComplaints.params.PanelID + " #ddlLocation").val(item.Complaint_LocationIds.split(','));
                            $('#' + Clinical_HPIComplaints.params.PanelID + " #ddlLocation").multiselect("refresh");
                            $('#' + Clinical_HPIComplaints.params.PanelID + " #ddlLocation").multiselect({
                                includeSelectAllOption: true,
                                enableFiltering: true,
                                enableCaseInsensitiveFiltering: true,
                                maxHeight: 247
                            });
                        }
                        $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').html(item.Comments);

                    });
                    NotFoundForPre = false;
                    return;
                }
            });


            if (NotFoundForPre) {

                var self = $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails');
                var myJSON = self != null ? self.getMyJSONByName() : "{}";
                var objDetail = JSON.parse(myJSON);
                objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').html();
                objDetail["ComplaintDetailId"] = Clinical_HPIComplaints.ComplaintSelectedCurrent;
                $.grep(Clinical_HPIComplaints.Complaints, function (item, index) {
                    if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedCurrent) {
                        objDetail["ComplaintDescription"] = item.ComplaintDescription;
                        return;
                    }
                });

                //objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtComments').val();

                var LocationIDS = self.find('#ddlLocation option:Selected').map(function () {
                    return this.value;
                }).get().join(',');
                objDetail["Complaint_LocationIds"] = LocationIDS;

                var LocationsText = self.find('#ddlLocation option:Selected').map(function () {
                    return this.text;
                }).get().join(', ');
                objDetail["Complaint_LocationsText"] = LocationsText;

                Clinical_HPIComplaints.ComplaintsDetail.push(objDetail);
                Clinical_HPIComplaints.resetValues();
            }
            //else {
            //    $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails').data('serialize', $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails').serialize());
            //}




        }
        else {
            Clinical_HPIComplaints.ComplaintSelectedPrevious = Clinical_HPIComplaints.ComplaintSelectedCurrent;
            Clinical_HPIComplaints.ComplaintSelectedCurrent = SelectedComplaintId;

            //Previous Complaint Work
            if (Clinical_HPIComplaints.ComplaintSelectedPrevious != Clinical_HPIComplaints.ComplaintSelectedCurrent) {

                $.grep(Clinical_HPIComplaints.ComplaintsDetail, function (item, index) {
                    if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedPrevious) {
                        NotFoundForPre = false;
                        return;
                    }
                });


                if (NotFoundForPre) {
                    var self = $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails');
                    var myJSON = self != null ? self.getMyJSONByName() : "{}";
                    var objDetail = JSON.parse(myJSON);
                    objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').html();
                    objDetail["ComplaintDetailId"] = Clinical_HPIComplaints.ComplaintSelectedPrevious;
                    $.grep(Clinical_HPIComplaints.Complaints, function (item, index) {
                        if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedPrevious) {
                            objDetail["ComplaintDescription"] = item.ComplaintDescription;
                            return;
                        }
                    });

                    //objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtComments').val();

                    var LocationIDS = self.find('#ddlLocation option:Selected').map(function () {
                        return this.value;
                    }).get().join(',');
                    objDetail["Complaint_LocationIds"] = LocationIDS;

                    var LocationsText = self.find('#ddlLocation option:Selected').map(function () {
                        return this.text;
                    }).get().join(', ');
                    objDetail["Complaint_LocationsText"] = LocationsText;

                    Clinical_HPIComplaints.ComplaintsDetail.push(objDetail);
                    Clinical_HPIComplaints.resetValues();
                }
                else {


                    var ComplaintsDetail_Copy = [];
                    $.grep(Clinical_HPIComplaints.ComplaintsDetail, function (item, index) {
                        if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedPrevious) {
                            var self = $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails');
                            var myJSON = self != null ? self.getMyJSONByName() : "{}";
                            var objDetail = JSON.parse(myJSON);
                            objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').html();
                            objDetail["ComplaintDetailId"] = Clinical_HPIComplaints.ComplaintSelectedPrevious;
                            $.grep(Clinical_HPIComplaints.Complaints, function (item, index) {
                                if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedPrevious) {
                                    objDetail["ComplaintDescription"] = item.ComplaintDescription;
                                    return;
                                }
                            });
                            //objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtComments').val();
                            var LocationIDS = self.find('#ddlLocation option:Selected').map(function () {
                                return this.value;
                            }).get().join(',');
                            objDetail["Complaint_LocationIds"] = LocationIDS;

                            var LocationsText = self.find('#ddlLocation option:Selected').map(function () {
                                return this.text;
                            }).get().join(', ');
                            objDetail["Complaint_LocationsText"] = LocationsText;

                            objDetail["IsUpdate"] = Clinical_HPIComplaints.IsUpdate;
                            ComplaintsDetail_Copy.push(objDetail);
                        }
                        else {
                            ComplaintsDetail_Copy.push(item);
                        }
                    });
                    Clinical_HPIComplaints.ComplaintsDetail = ComplaintsDetail_Copy;
                    Clinical_HPIComplaints.resetValues();
                }
            }

            //Current Complaint Work
            $.grep(Clinical_HPIComplaints.ComplaintsDetail, function (item, index) {
                if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedCurrent) {
                    var self = $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails');
                    NotFoundForCur = false;
                    utility.bindMyJSONByName(true, item, false, self).done(function () {
                        if (item.Complaint_LocationIds != '') {
                            $('#' + Clinical_HPIComplaints.params.PanelID + " #ddlLocation").val(item.Complaint_LocationIds.split(','));
                            $('#' + Clinical_HPIComplaints.params.PanelID + " #ddlLocation").multiselect("refresh");
                            $('#' + Clinical_HPIComplaints.params.PanelID + " #ddlLocation").multiselect({
                                includeSelectAllOption: true,
                                enableFiltering: true,
                                enableCaseInsensitiveFiltering: true,
                                maxHeight: 247
                            });
                        }

                        $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').html(item.Comments);

                    });
                    return;
                }
            });
            if (NotFoundForCur) {
                var self = $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails');
                var myJSON = self != null ? self.getMyJSONByName() : "{}";
                var objDetail = JSON.parse(myJSON);
                objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').html();
                objDetail["ComplaintDetailId"] = Clinical_HPIComplaints.ComplaintSelectedCurrent;
                $.grep(Clinical_HPIComplaints.Complaints, function (item, index) {
                    if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedCurrent) {
                        objDetail["ComplaintDescription"] = item.ComplaintDescription;
                        return;
                    }
                });
                //objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtComments').val();

                var LocationIDS = self.find('#ddlLocation option:Selected').map(function () {
                    return this.value;
                }).get().join(',');
                objDetail["Complaint_LocationIds"] = LocationIDS;

                var LocationsText = self.find('#ddlLocation option:Selected').map(function () {
                    return this.text;
                }).get().join(', ');
                objDetail["Complaint_LocationsText"] = LocationsText;

                Clinical_HPIComplaints.ComplaintsDetail.push(objDetail);
                Clinical_HPIComplaints.resetValues();
            }
            //else {
            //    $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails').data('serialize', $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails').serialize());
            //}
        }
        Clinical_HPIComplaints.IsUpdate = false;
    },
    //End M Ahmad Imran 12/02/2016

    //Start//12/02/2016//M Ahmad Imran//Implimented Load Complaint Detail
    loadCiefComplaintComponent: function (CallBack) {
        Clinical_HPIComplaints.IsDurationValidation = true;
        Clinical_HPIComplaints.ValidationErrorMessage = "";
        Clinical_HPIComplaints.IsUpdate = false;
        Clinical_HPIComplaints.Complaints = [];
        Clinical_HPIComplaints.ComplaintsDetail = [];
        Clinical_HPIComplaints.ComplaintSelectedPrevious = 0;
        Clinical_HPIComplaints.ComplaintSelectedCurrent = 0;
        Clinical_HPIComplaints.IsDelete = false;
        Clinical_HPIComplaints.IntializeMultiSelectDropDown();
        Clinical_HPIComplaints.resetValues();
        $("#txtOverallComments").val("");
        $('#' + Clinical_HPIComplaints.params.PanelID + " #txtComplaintId").val(0);
        utility.CreateDatePicker(Clinical_HPIComplaints.params.PanelID + '  #dtComplaintDate', function () {
        }, true);

        $('#pnlClinicalHPIComplaints #ulCompliantDisease').html("");

        Clinical_HPIComplaints.LoadComplaints().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var ComplainteLoad_JSON = [];
                var ComplainteDetailLoad_JSON = [];
                if (response.ComplaintTotalCount > 0) {
                    ComplainteLoad_JSON = JSON.parse(response.ComplainteLoad_JSON);
                } if (response.ComplaintDetailTotalCount > 0) {
                    ComplainteDetailLoad_JSON = JSON.parse(response.ComplainteDetailLoad_JSON);
                }

                $.each(ComplainteDetailLoad_JSON, function (i, item) {
                    //Start 20-10-2017 Humaira Yousaf EMR-5008
                    if (Clinical_HPIComplaints.ComplaintsDetailForDx.length > 0) {
                        if (i < Clinical_HPIComplaints.ComplaintsDetailForDx.length) {
                            item.ComplaintDetailId ? Clinical_HPIComplaints.ComplaintsDetailForDx[i].ComplaintDetailId = item.ComplaintDetailId : Clinical_HPIComplaints.ComplaintsDetailForDx[i].ComplaintDetailId = "";
                            item.ProblemListId ? Clinical_HPIComplaints.ComplaintsDetailForDx[i]["ProblemListId"] = item.ProblemListId : Clinical_HPIComplaints.ComplaintsDetailForDx[i]["ProblemListId"] = "";
                        }
                        else {
                            var complaint = {
                                icd9code: item.ICD9,
                                icd9desc: item.ICD9Description,
                                icd10code: item.ICD10,
                                icd10desc: item.ICD10Description,
                                snomedcode: item.SNOMED,
                                snomeddesc: item.SNOMEDDescription,
                                ComplaintDetailId: item.ComplaintDetailId,
                                ComplaintId: item.ComplaintId,
                            };
                            Clinical_HPIComplaints.ComplaintsDetailForDx.push(complaint);
                        }
                    }
                    //End 20-10-2017 Humaira Yousaf EMR-5008              

                    var liDescription = item.ComplaintDescription;
                    if (item.ICD10 != "" && liDescription.indexOf(item.ICD10 + " - ") > -1) {
                        liDescription = liDescription;
                    }
                    else {
                        liDescription = (item.ICD10 != "" ? item.ICD10 + " - " : "") + liDescription;
                    }
                    var li = "<li id=" + item.ComplaintDetailId + " onclick='Clinical_HPIComplaints.fillChiefComplaints(this, event);' ComplaintDetailId=\"" + item.ComplaintDetailId + "\"icd9Code=\"" + item.ICD9 + "\" icd9desc=\"" + item.ICD9Description + "\" icd10Code=\"" + item.ICD10 + "\" icd10Desc=\"" + item.ICD10Description + "\" snomedCode=\"" + item.SNOMED + "\" snomedDesc=\"" + item.SNOMEDDescription + "\">" +
	                    "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + liDescription + "</span></a>" +
		                    "<span class='removeIconListHover'>" +
                                "<a href='#' onclick='Clinical_HPIComplaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                    "<a href='#' onclick='Clinical_HPIComplaints.deleteChiefComplaint(" + item.ComplaintDetailId + ", event);'><i class='fa fa-times red'></i></a>" +
		                    "</span>" +
                        "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_HPIComplaints.updateComplaintsText(this, event);'/>" +
                        "<div class='clearfix'></div>" +
	                "</li>";

                    $('#pnlClinicalHPIComplaints #ulCompliantDisease').append(li);
                    $('.modal-backdrop').removeClass('in');
                    $('.modal-backdrop').addClass('out');
                    $('.modal-backdrop').hide();
                    $('#pnlClinicalHPIComplaints #txtComplaints').val('');

                    //Clinical_HPIComplaints.ComplaintsDetail.push(item);
                    var self = $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails');
                    utility.bindMyJSONByName(true, item, false, self).done(function () {
                        if (item.Complaint_LocationIds != '') {
                            $('#' + Clinical_HPIComplaints.params.PanelID + " #ddlLocation").val(item.Complaint_LocationIds.split(','));
                            $('#' + Clinical_HPIComplaints.params.PanelID + " #ddlLocation").multiselect("refresh");
                            $('#' + Clinical_HPIComplaints.params.PanelID + " #ddlLocation").multiselect({
                                includeSelectAllOption: true,
                                enableFiltering: true,
                                enableCaseInsensitiveFiltering: true,
                                maxHeight: 247
                            });
                        }
                        var myJSON = self != null ? self.getMyJSONByName() : "{}";
                        var objDetail = JSON.parse(myJSON);
                        objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').html();
                        objDetail["ComplaintDetailId"] = item.ComplaintDetailId;

                        objDetail["ComplaintDescription"] = item.ComplaintDescription;
                        var LocationIDS = self.find('#ddlLocation option:Selected').map(function () {
                            return this.value;
                        }).get().join(',');
                        objDetail["Complaint_LocationIds"] = LocationIDS;

                        var LocationsText = self.find('#ddlLocation option:Selected').map(function () {
                            return this.text;
                        }).get().join(', ');
                        objDetail["Complaint_LocationsText"] = LocationsText;

                        objDetail["IsUpdate"] = false;
                        $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtComments').val(item.Comments);
                        $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').text(item.Comments);
                        objDetail["Comments"] = item.Comments;
                        Clinical_HPIComplaints.ComplaintsDetail.push(objDetail);

                    });
                    // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-473
                    Clinical_HPIComplaints.resetValues();
                    // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-473


                });
                $.each(ComplainteDetailLoad_JSON, function (i, item) {
                    if (i == 0) {
                        Clinical_HPIComplaints.AddInArray(item.ComplaintDetailId, item.ComplaintDescription, true);
                    }
                    else {
                        Clinical_HPIComplaints.AddInArray(item.ComplaintDetailId, item.ComplaintDescription, false);
                    }
                });
                $.each(ComplainteLoad_JSON, function (i, item) {
                    $('#' + Clinical_HPIComplaints.params.PanelID + " #dtComplaintDate").val(response.CapturedDateDate);
                    $('#' + Clinical_HPIComplaints.params.PanelID + " #txtOverallComments").val(item.OverallComments);
                    $('#' + Clinical_HPIComplaints.params.PanelID + " #txtComplaintId").val(item.ComplaintId != '' ? item.ComplaintId : 0);
                    Clinical_HPIComplaints.ComplaintId = item.ComplaintId != '' ? item.ComplaintId : 0;

                });
                if (CallBack != null) {
                    CallBack();
                }
                else {
                    if ($('#' + Clinical_HPIComplaints.params.PanelID + ' #ulCompliantDisease li').length > 0) {
                        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtComments").val('');
                        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtComment").html('');
                        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulCompliantDisease li:eq(0)').trigger('click');
                    }
                }
                Clinical_HPIComplaints.GlobalDate = $('#' + Clinical_HPIComplaints.params.PanelID + " #dtComplaintDate").val();
                Clinical_HPIComplaints.GlobalOverAllComment = $('#' + Clinical_HPIComplaints.params.PanelID + " #txtOverallComments").val();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });



    },
    //End M Ahmad Imran 10/2/2016
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },
    deleteChiefComplaint: function (ComplainDetailId, event) {
        utility.myConfirm('1', function () {
            Clinical_HPIComplaints.deleteChiefComplaintConfirmed(ComplainDetailId, event)
        });
    },

    deleteChiefComplaintConfirmed: function (ComplainDetailId, event) {

        event.stopPropagation();

        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li#" + ComplainDetailId + "").remove();
        var ComplaintNotFromFavComplaint = true;
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints ul#ulFavCompliantDisease li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == ComplainDetailId) {
                if ($(this).hasClass("active") == false) {
                    $(this).removeClass("disableAll");
                }
                ComplaintNotFromFavComplaint = false;
                Clinical_HPIComplaints.deleteComplaintFromCasha(ComplainDetailId);
            }
        });

        if ($('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints ul#ulFavCompliantDisease li").length == 0) {
            Clinical_HPIComplaints.IsUpdate = false;
        }
        if (ComplainDetailId > 0 && ComplaintNotFromFavComplaint) {
            $.when(Clinical_HPIComplaints.ComplaintsSave(true, false)).then(function () {
                Clinical_HPIComplaints.DeleteComplaint(ComplainDetailId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        Clinical_HPIComplaints.IsDelete = true;

                        //Remove ChefComplaint Problems
                        Clinical_HPIComplaints.removeChiefComplaintProblems(Clinical_Complaints.ComplaintId, ComplainDetailId);

                        Clinical_HPIComplaints.loadCiefComplaintComponent(function () {
                            // Clinical_HPIComplaints.getLatestComplaintByPatientId();
                            // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
                            $.when(Clinical_HPIComplaints.createComplaintBodyHTML($('#' + Clinical_HPIComplaints.params.PanelID + ' #txtOverallComments').val(), Clinical_HPIComplaints.ComplaintsDetail, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true)).then(function () {
                            });
                            // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
                        });
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            });

        }
        else if (ComplainDetailId < 0 && ComplaintNotFromFavComplaint) {

            Clinical_HPIComplaints.deleteComplaintFromCasha(ComplainDetailId);
        }
        if (!ComplaintNotFromFavComplaint) {
            Clinical_HPIComplaints.IsDelete = false;
        }

    },

    deleteComplaintFromCasha: function (ComplainDetailId) {
        var ComplaintsCopy = [];
        var ComplaintsDetailCopy = [];

        $.grep(Clinical_HPIComplaints.ComplaintsDetail, function (item, index) {
            if (item.ComplaintDetailId == ComplainDetailId) {

            }
            else {
                ComplaintsDetailCopy.push(item);
            }
        });
        $.grep(Clinical_HPIComplaints.Complaints, function (item, index) {
            if (item.ComplaintDetailId == ComplainDetailId) {

            }
            else {
                ComplaintsCopy.push(item);
            }
        });
        Clinical_HPIComplaints.Complaints = [];
        Clinical_HPIComplaints.ComplaintsDetail = [];
        Clinical_HPIComplaints.Complaints = ComplaintsCopy;
        Clinical_HPIComplaints.ComplaintsDetail = ComplaintsDetailCopy;
        if (Clinical_HPIComplaints.ComplaintsDetail.length > 0) {

            if (Clinical_HPIComplaints.ComplaintSelectedPrevious == ComplainDetailId) {
                Clinical_HPIComplaints.ComplaintSelectedPrevious = Clinical_HPIComplaints.ComplaintSelectedCurrent;
            }
            if (Clinical_HPIComplaints.ComplaintSelectedCurrent == ComplainDetailId) {
                var pre = Clinical_HPIComplaints.ComplaintSelectedPrevious;

                Clinical_HPIComplaints.ComplaintSelectedPrevious = 0;
                Clinical_HPIComplaints.fillChiefComplaints($('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li#" + pre + ""), null);
            }

        }
        else {
            Clinical_HPIComplaints.ComplaintSelectedCurrent = 0;
            Clinical_HPIComplaints.ComplaintSelectedPrevious = 0;
        }
    },
    resetValues: function () {
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #sectionComplaintDetails").find('[type=text],[type=password],[type=checkbox],textarea,[type=radio]').each(function () {
            $(this).val('');
        });
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #sectionComplaintDetails").find('select').each(function () {
            $(this).val('0');
        });
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #sectionComplaintDetails input:checkbox").removeAttr('checked');

        $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails #ddlLocation').multiselect('rebuild');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtComment").html('');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtComments").val('');

    },
    ResetComplaint: function () {

        utility.myConfirm('27', function () {
            if ($('#pnlClinicalHPIComplaints #txtComplaintId').val() != 0) {
                Clinical_HPIComplaints.ComplaintReset(false, null).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_HPIComplaints.IsReset = true;
                        Clinical_HPIComplaints.ResetedComplaitId = $('#pnlClinicalHPIComplaints #txtComplaintId').val();
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_HPIComplaints.loadCiefComplaintComponent(function () {
                            Clinical_HPIComplaints.ComplaintsSave(false, false);
                        });

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                Clinical_HPIComplaints.IsDurationValidation = true;
                Clinical_HPIComplaints.ValidationErrorMessage = "";
                Clinical_HPIComplaints.IsUpdate = false;
                Clinical_HPIComplaints.Complaints = [];
                Clinical_HPIComplaints.ComplaintsDetail = [];
                Clinical_HPIComplaints.ComplaintSelectedPrevious = 0;
                Clinical_HPIComplaints.ComplaintSelectedCurrent = 0;
                Clinical_HPIComplaints.IsDelete = false;
                Clinical_HPIComplaints.IntializeMultiSelectDropDown();
                Clinical_HPIComplaints.resetValues();
                $("#txtOverallComments").val("");
                $('#' + Clinical_HPIComplaints.params.PanelID + " #txtComplaintId").val(0);
                Clinical_HPIComplaints.ComplaintId = 0;
                utility.CreateDatePicker(Clinical_HPIComplaints.params.PanelID + '  #dtComplaintDate', function () {
                }, true);
                $('#pnlClinicalHPIComplaints #ulCompliantDisease').html("");
            }
        });

    },
    unLoadTab: function (nextOrPre, controlToInvoke) {
        // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
        Clinical_HPIComplaints.controlToInvoke = controlToInvoke;

        if (Clinical_HPIComplaints.IsReset == true) {
            utility.myConfirm('35', function () {

                //Clinical_HPIComplaints.controlToInvoke = ComeFromLowerButton;
                $.when(Clinical_HPIComplaints.ComplaintsSave(false, true)).then(function () {
                    var objDeffered = $.Deferred();
                    if (Clinical_HPIComplaints.params["FromAdmin"] == "0") {
                        if (Clinical_HPIComplaints.params != null && Clinical_HPIComplaints.params.ParentCtrl != null) {
                            if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                                Clinical_HPIComplaints.loadNoteComponent();
                            }
                            else {
                                UnloadActionPan(Clinical_HPIComplaints.params.ParentCtrl, 'Clinical_HPIComplaints');
                            }
                        }
                        else
                            UnloadActionPan(null, 'Clinical_HPIComplaints');
                    }
                    else {

                        RemoveAdminTab();
                    }
                    objDeffered.resolve();
                    EMRUtility.scrollToPNcomponent('Clinical_HPIComplaints');
                    return objDeffered;
                });
            }, function () {
                //Clinical_HPIComplaints.controlToInvoke = ComeFromLowerButton;
                var objDeffered = $.Deferred();

                if (Clinical_HPIComplaints.params["FromAdmin"] == "0") {
                    if (Clinical_HPIComplaints.params != null && Clinical_HPIComplaints.params.ParentCtrl != null) {
                        if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                            Clinical_HPIComplaints.loadNoteComponent();
                        }
                        else {
                            UnloadActionPan(Clinical_HPIComplaints.params.ParentCtrl, 'Clinical_HPIComplaints');
                        }
                    }
                    else
                        UnloadActionPan(null, 'Clinical_HPIComplaints');
                }
                else {

                    RemoveAdminTab();
                }
                objDeffered.resolve();
                EMRUtility.scrollToPNcomponent('Clinical_HPIComplaints');
                return objDeffered;
            });
        }
        else {
            Clinical_HPIComplaints.IsDurationValidation = true;

            var ComplaintsDetail_Copy = [];
            $.grep(Clinical_HPIComplaints.ComplaintsDetail, function (item, index) {
                if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedCurrent) {
                    var self = $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails');
                    var myJSON = self != null ? self.getMyJSONByName() : "{}";
                    var objDetail = JSON.parse(myJSON);
                    objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').html();
                    objDetail["ComplaintDetailId"] = Clinical_HPIComplaints.ComplaintSelectedCurrent;
                    $.grep(Clinical_HPIComplaints.Complaints, function (item, index) {
                        if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedCurrent) {
                            objDetail["ComplaintDescription"] = item.ComplaintDescription;
                            return;
                        }
                    });

                    var LocationIDS = self.find('#ddlLocation option:Selected').map(function () {
                        return this.value;
                    }).get().join(',');

                    objDetail["Complaint_LocationIds"] = LocationIDS;

                    var LocationsText = self.find('#ddlLocation option:Selected').map(function () {
                        return this.text;
                    }).get().join(', ');
                    objDetail["Complaint_LocationIds_text"] = LocationsText;

                    objDetail["IsUpdate"] = Clinical_HPIComplaints.IsUpdate;
                    objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtComments').val();
                    ComplaintsDetail_Copy.push(objDetail);

                }
                else {
                    ComplaintsDetail_Copy.push(item);
                }
            });
            Clinical_HPIComplaints.ComplaintsDetail = ComplaintsDetail_Copy;

            var IsSave = false;
            $.grep(Clinical_HPIComplaints.ComplaintsDetail, function (item, index) {

                if (item.IsUpdate == true || (Clinical_HPIComplaints.GlobalDate != $('#' + Clinical_HPIComplaints.params.PanelID + " #dtComplaintDate").val()) || (Clinical_HPIComplaints.GlobalOverAllComment != $('#' + Clinical_HPIComplaints.params.PanelID + " #txtOverallComments").val())) {
                    IsSave = true;
                }
            });
            if (IsSave || Clinical_HPIComplaints.IsDelete) {
                utility.myConfirmNote('1', function () {
                    //Clinical_HPIComplaints.controlToInvoke = ComeFromLowerButton;
                    $.when(Clinical_HPIComplaints.ComplaintsSave(false, true)).then(function () {
                        var objDeffered = $.Deferred();
                        if (Clinical_HPIComplaints.params["FromAdmin"] == "0") {
                            if (Clinical_HPIComplaints.params != null && Clinical_HPIComplaints.params.ParentCtrl != null) {
                                if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                                    Clinical_HPIComplaints.loadNoteComponent();
                                }
                                else {
                                    UnloadActionPan(Clinical_HPIComplaints.params.ParentCtrl, 'Clinical_HPIComplaints');
                                }
                            }
                            else
                                UnloadActionPan(null, 'Clinical_HPIComplaints');
                        }
                        else {

                            RemoveAdminTab();
                        }
                        objDeffered.resolve();
                        EMRUtility.scrollToPNcomponent('Clinical_HPIComplaints');
                        return objDeffered;
                    });

                }, "", function () {
                    //Clinical_HPIComplaints.controlToInvoke = ComeFromLowerButton;
                    var objDeffered = $.Deferred();

                    if (Clinical_HPIComplaints.params["FromAdmin"] == "0") {
                        if (Clinical_HPIComplaints.params != null && Clinical_HPIComplaints.params.ParentCtrl != null) {
                            if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                                Clinical_HPIComplaints.loadNoteComponent();
                            }
                            else {
                                UnloadActionPan(Clinical_HPIComplaints.params.ParentCtrl, 'Clinical_HPIComplaints');
                            }
                        }
                        else
                            UnloadActionPan(null, 'Clinical_HPIComplaints');
                    }
                    else {

                        RemoveAdminTab();
                    }
                    objDeffered.resolve();
                    EMRUtility.scrollToPNcomponent('Clinical_HPIComplaints');
                    return objDeffered;
                });

            }
            else {
                //Clinical_HPIComplaints.controlToInvoke = ComeFromLowerButton;
                var objDeffered = $.Deferred();

                if (Clinical_HPIComplaints.params["FromAdmin"] == "0") {
                    if (Clinical_HPIComplaints.params != null && Clinical_HPIComplaints.params.ParentCtrl != null) {
                        if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                            Clinical_HPIComplaints.loadNoteComponent();
                        }
                        else {
                            UnloadActionPan(Clinical_HPIComplaints.params.ParentCtrl, 'Clinical_HPIComplaints');
                        }
                    }
                    else
                        UnloadActionPan(null, 'Clinical_HPIComplaints');
                }
                else {
                    RemoveAdminTab();
                }
                objDeffered.resolve();
                Clinical_HPIComplaints.resetForm();
                EMRUtility.scrollToPNcomponent('Clinical_HPIComplaints');
                return objDeffered;
            }
        }
        // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
        Clinical_HPIComplaints.resetForm();

    },
    AddCompalintsToNotes: function () {
        //if ($('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #radioTemplate").prop("checked") == true) {
        //    Clinical_HPIComplaints.addHPIComplaintToNote();
        //}
        //else {
        $.when(Clinical_HPIComplaints.ComplaintsSave(false, false)).then(function () {
            var objDeffered = $.Deferred();
            if (Clinical_HPIComplaints.params["FromAdmin"] == "0") {
                if (Clinical_HPIComplaints.params && Clinical_HPIComplaints.params.ParentCtrl) {
                    if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote") {
                        Clinical_HPIComplaints.loadNoteComponent();
                    }
                    else {
                        UnloadActionPan(Clinical_HPIComplaints.params.ParentCtrl, 'Clinical_HPIComplaints');
                    }
                }
                else
                    UnloadActionPan(null, 'Clinical_HPIComplaints');
            }
            else {

                RemoveAdminTab();
            }
            objDeffered.resolve();
            EMRUtility.scrollToPNcomponent('Clinical_HPIComplaints');
            return objDeffered;
        });
        //}
    },
    //Start//12/02/2016//M Ahmad Imran//Implimented call to controller for Get Complaint Detail Data from DB
    LoadComplaints: function () {
        var objData = {};
        objData["NotesId"] = Clinical_HPIComplaints.params.NotesId;
        objData["commandType"] = "Load_Complaint";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Complaint");
    },
    //End M Ahmad Imran 10/2/2016
    //Start//10/02/2016//M Ahmad Imran//Implimented validation on Complaint Detail field
    validateComplaint: function () {
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #frmClinicalComplaints')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (e.type == "success") {
                //Clinical_HPIComplaints.ComplaintsSave();
            }

        });

    },
    //End M Ahmad Imran 10/2/2016

    //Start//10/02/2016//M Ahmad Imran//Implimented Save Complaint Detail
    ComplaintsSave: function (ComeFromDelete, ComeFromUnLoad) {
        var PreFavVal = $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlFavComplaint').val();
        // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
        var objDeffered = $.Deferred();
        // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
        if (!Clinical_HPIComplaints.IsDurationValidation) {
            //utility.DisplayMessages(Clinical_HPIComplaints.ValidationErrorMessage, 3);
        }
        else {
            // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
            var CopyComplainDetail = CopyComplainDetail = Clinical_HPIComplaints.ComplaintsDetail;
            var OverAllComments = $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtOverallComments').val();
            // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
            if (!ComeFromUnLoad) {
                var ComplaintsDetail_Copy = [];
                $.grep(Clinical_HPIComplaints.ComplaintsDetail, function (item, index) {
                    if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedCurrent) {
                        var self = $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionComplaintDetails');
                        //if ($('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').html()) {
                        //    self.find("#txtComments").val($('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').html()); // add comments in  existing field
                        //}
                        //else {
                        //    self.find("#txtComments").val("");
                        //}
                        var myJSON = self != null ? self.getMyJSONByName() : "{}";
                        var objDetail = JSON.parse(myJSON);
                        objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + '  #frmClinicalComplaints #txtComment').html();
                        objDetail["ComplaintDetailId"] = Clinical_HPIComplaints.ComplaintSelectedCurrent;
                        $.grep(Clinical_HPIComplaints.Complaints, function (item, index) {
                            if (item.ComplaintDetailId == Clinical_HPIComplaints.ComplaintSelectedCurrent) {
                                objDetail["ComplaintDescription"] = item.ComplaintDescription;
                                return;
                            }
                        });

                        var LocationIDS = self.find('#ddlLocation option:Selected').map(function () {
                            return this.value;
                        }).get().join(',');
                        objDetail["Complaint_LocationIds"] = LocationIDS;

                        var LocationsText = self.find('#ddlLocation option:Selected').map(function () {
                            return this.text;
                        }).get().join(', ');
                        objDetail["Complaint_LocationsText"] = LocationsText;
                        objDetail["Complaint_LocationIds_text"] = LocationsText;

                        objDetail["IsUpdate"] = Clinical_HPIComplaints.IsUpdate;
                        //objDetail["Comments"] = $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtComments').val();
                        ComplaintsDetail_Copy.push(objDetail);

                    }
                    else {
                        ComplaintsDetail_Copy.push(item);
                    }
                });
                Clinical_HPIComplaints.ComplaintsDetail = ComplaintsDetail_Copy;
                // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
                CopyComplainDetail = CopyComplainDetail = Clinical_HPIComplaints.ComplaintsDetail;
                OverAllComments = $('#' + Clinical_HPIComplaints.params.PanelID + ' #txtOverallComments').val();
                // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
            }
            if (Clinical_HPIComplaints.ComplaintsDetail.length != 0 || Clinical_HPIComplaints.selectedSymptoms.length != 0) {

                // fill to SAVE ICD's while saving Complaint Detail.
                $("#pnlClinicalHPIComplaints #ulCompliantDisease li").each(function (i, e) {
                    $(this).attr('icd9code') && $(this).attr('icd9code') != 'undefined' ? Clinical_HPIComplaints.ComplaintsDetail[i]["ICD9"] = $(this).attr('icd9code') : '';
                    $(this).attr('icd9desc') && $(this).attr('icd9desc') != 'undefined' ? Clinical_HPIComplaints.ComplaintsDetail[i]["ICD9Description"] = $(this).attr('icd9desc') : '';
                    $(this).attr('icd10code') && $(this).attr('icd10code') != 'undefined' ? Clinical_HPIComplaints.ComplaintsDetail[i]["ICD10"] = $(this).attr('icd10code') : '';
                    $(this).attr('icd10desc') && $(this).attr('icd10desc') != 'undefined' ? Clinical_HPIComplaints.ComplaintsDetail[i]["ICD10Description"] = $(this).attr('icd10desc') : '';
                    $(this).attr('snomedcode') && $(this).attr('snomedcode') != 'undefined' ? Clinical_HPIComplaints.ComplaintsDetail[i]["SNOMED"] = $(this).attr('snomedcode') : '';
                    $(this).attr('snomeddesc') && $(this).attr('snomeddesc') != 'undefined' ? Clinical_HPIComplaints.ComplaintsDetail[i]["SNOMEDDescription"] = $(this).attr('snomeddesc') : '';
                });


                //var myJSON = JSON.stringify(Clinical_HPIComplaints.ComplaintsDetail);
                Clinical_HPIComplaints.SaveComplaint().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_HPIComplaints.SaveFavToggelStatus(PreFavVal);

                        if (response.ComplaintId) {
                            $('#' + Clinical_HPIComplaints.params.PanelID + " #txtComplaintId").val(response.ComplaintId);
                            Clinical_HPIComplaints.ComplaintId = response.ComplaintId;
                        }

                        // fill to SAVE ICD's from ProblemList flow.
                        Clinical_HPIComplaints.ComplaintsDetailForDx = Clinical_HPIComplaints.ComplaintsDetail;
                        var ComplaintDetail_List = JSON.parse(response.ComplaintDetail_JSON);
                        $(ComplaintDetail_List).each(function (i, item) {

                            Clinical_HPIComplaints.ComplaintsDetailForDx[i]["icd9code"] = item.ICD9;
                            Clinical_HPIComplaints.ComplaintsDetailForDx[i]["icd9desc"] = item.ICD9Description;
                            Clinical_HPIComplaints.ComplaintsDetailForDx[i]["icd10code"] = item.ICD10;
                            Clinical_HPIComplaints.ComplaintsDetailForDx[i]["icd10desc"] = item.ICD10Description;
                            Clinical_HPIComplaints.ComplaintsDetailForDx[i]["snomedcode"] = item.SNOMED;
                            Clinical_HPIComplaints.ComplaintsDetailForDx[i]["snomeddesc"] = item.SNOMEDDescription;
                            Clinical_HPIComplaints.ComplaintsDetailForDx[i]["ComplaintDetailId"] = item.ComplaintDetailId;
                            Clinical_HPIComplaints.ComplaintsDetailForDx[i]["ComplaintId"] = item.ComplaintId;
                        });

                        if (!ComeFromDelete) {
                            Clinical_HPIComplaints.loadCiefComplaintComponent(function () {
                                // Clinical_HPIComplaints.getLatestComplaintByPatientId();
                                // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
                                Clinical_HPIComplaints.createComplaintBodyHTML(OverAllComments, Clinical_HPIComplaints.ComplaintsDetail, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true).done(function () {
                                    Clinical_HPIComplaints.IsReset = false;
                                    $.when(Clinical_HPIComplaints.addHPIComplaintToNote()).then(function () {
                                        objDeffered.resolve();
                                    });
                                });
                                // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491

                                Clinical_HPIComplaints.AddComplaintsToProblemList(Clinical_HPIComplaints.ComplaintsDetailForDx);

                            });
                            utility.DisplayMessages(response.Message, 1);
                            //Clinical_HPIComplaints.AddComplaintsToProblemList();
                        }
                        else {
                            objDeffered.resolve();
                        }

                    }
                    else {
                        if (!ComeFromDelete) {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });


            }
            else {
                if (Clinical_HPIComplaints.IsDelete && ComeFromUnLoad) {
                    $.when(Clinical_HPIComplaints.createComplaintBodyHTML(OverAllComments, CopyComplainDetail, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML')).then(function () {
                        objDeffered.resolve();
                    });
                }
                else if (Clinical_HPIComplaints.IsReset && ComeFromUnLoad) {
                    $.when(Clinical_HPIComplaints.createComplaintBodyHTML(OverAllComments, CopyComplainDetail, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML')).then(function () {
                        Clinical_HPIComplaints.IsReset = false;
                        objDeffered.resolve();
                    });
                }
                else if (Clinical_HPIComplaints.IsReset) {
                    $.when(Clinical_HPIComplaints.createComplaintBodyHTML(OverAllComments, CopyComplainDetail, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML')).then(function () {
                        Clinical_HPIComplaints.IsReset = false;
                        objDeffered.resolve();
                    });
                }
                else {
                    utility.DisplayMessages("There is no Complaint to add", 3);/// ask from babur bhai
                }
            }

        }
        // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
        return objDeffered;
        // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491

    },
    //End M Ahmad Imran 10/2/2016
    //Start//23/03/2016//M Ahmad Imran//Implimented BindFavComplaints which bind all Complaints respected to FavComplaint Name
    AutoSearchFavComplaints: function () {
        utility.Keyupdelay(function () {
            Clinical_HPIComplaints.BindFavComplaints();
        });
    },
    BindFavComplaints: function () {
        var FavoriteListId = $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlFavComplaint').val();
        var searchData = $('#' + Clinical_HPIComplaints.params.PanelID + ' #FavSearchBox').val();
        if (FavoriteListId != "") {
            Favorite_Complaints.searchFavoriteList_ICD_DBCall(null, FavoriteListId, null, null, searchData).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (response.FavoriteListICDCount > 0) {
                        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulFavCompliantDisease li").remove();
                        var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
                        var li = "";
                        $.each(FavoriteListJSON, function (i, item) {
                            li += "<li icd9Code='" + item.ICD9Code + "' icd10Code='" + item.ICD10Code
                            + "' snomedCode='" + item.SNOMEDID
                            + "' icd9Desc='" + item.ICD9CodeDescription
                            + "' icd10Desc='" + item.ICD10CodeDescription
                            + "' snomedDesc='" + item.SNOMEDDescription +
                            "'id=" + Clinical_HPIComplaints.FavComplaintsId + " onclick='Clinical_HPIComplaints.PassToComplaints(this, event);'  ><a href='#' class='pr-sm'>" + item.ICD10CodeDescription + "</a></li>"
                            Clinical_HPIComplaints.AddArrayInFavComplaints(Clinical_HPIComplaints.FavComplaintsId, item.ICD10CodeDescription);
                            Clinical_HPIComplaints.FavComplaintsId = Clinical_HPIComplaints.FavComplaintsId - 1;
                        });
                        $('#pnlClinicalHPIComplaints #ulFavCompliantDisease').append(li);

                    }
                    else {
                        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulFavCompliantDisease li").remove();
                    }
                }
                else {
                    $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulFavCompliantDisease li").remove();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //End M Ahmad Imran 23/03/2016

    //Start//23/03/2016//M Ahmad Imran//Implimented which pass FavComplint to complaints list
    PassToComplaints: function (obj, ev) {
        var diseaseId = $(obj).attr('id');
        var IsAlreadyExist = false;
        $('#frmClinicalComplaints ul#ulCompliantDisease li').each(function () {
            if ($(this).attr('icd10Code') == $(obj).attr('icd10Code') && $(this).attr('icd10Desc') == $(obj).attr("icd10Desc") &&
                $(this).attr('snomedCode') == $(obj).attr('snomedCode') && $(this).attr('snomedDesc') == $(obj).attr("snomedDesc")) {
                IsAlreadyExist = true;
            }
            else {
                if ($(this).text().trim() == ($(obj).attr('icd10Code') != "" ? $(obj).attr('icd10Code') + " - " : "") + obj.innerText.trim()) {
                    IsAlreadyExist = true;
                }
            }
        });

        if (!IsAlreadyExist) {
            $(obj).addClass("disableAll");
            Icd10Description = $(this).find('a').html();
            var li = "<li icd9Code='" + $(obj).attr('icd9Code') + "'  icd10Code='" + $(obj).attr('icd10Code') + "' snomedCode='" + $(obj).attr('snomedCode') + "' icd9Desc='" + $(obj).attr('icd9Desc') + "' icd10Desc='" + $(obj).attr('icd10Desc') + "' snomedDesc='" + $(obj).attr('snomedDesc') + "' id=" + diseaseId + " onclick='Clinical_HPIComplaints.fillChiefComplaints(this, event);'>" +
                "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + ($(obj).attr('icd10Code') != "" ? $(obj).attr('icd10Code') + " - " : "") + $(obj).attr('icd10Desc') + "</span></a>" +
                    "<span class='removeIconListHover'>" +
                        "<a href='#' onclick='Clinical_HPIComplaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			            "<a href='#' onclick='Clinical_HPIComplaints.deleteChiefComplaint(" + diseaseId + ", event);'><i class='fa fa-times red'></i></a>" +
		            "</span>" +
                "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_HPIComplaints.updateComplaintsText(this, event);'/>" +
                "<div class='clearfix'></div>" +
            "</li>";
            $('#pnlClinicalHPIComplaints #ulCompliantDisease').append(li);
            Clinical_HPIComplaints.AddInArray(diseaseId, $(obj).attr('icd10Desc'), true);
            Clinical_HPIComplaints.IsUpdate = true;
        }
        else {
            utility.DisplayMessages('Disease already added', 2);
        }

    },
    //End M Ahmad Imran 23/03/2016


    //Start//24/03/2016//M Ahmad Imran//Implimented which pass all FavComplint to complaints list
    BindAllFavComplaints: function () {
        var diseaseId = 0;
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints ul#ulFavCompliantDisease li").each(function (i, item) {


            var IsAlreadyExist = false;
            $('#frmClinicalComplaints ul#ulCompliantDisease li').each(function (j, itemComp) {
                if (($(this).attr('icd9Code') == $(item).attr('icd9Code')) && ($(this).attr('icd9Desc') == $(item).attr("icd9Desc")) &&
                    ($(this).attr('icd10Code') == $(item).attr('icd10Code')) && ($(this).attr('icd10Desc') == $(item).attr("icd10Desc")) &&
                    ($(this).attr('snomedCode') == $(item).attr('snomedCode')) && ($(this).attr('snomedDesc') == $(item).attr("snomedDesc"))) {
                    IsAlreadyExist = true;
                }
                else {
                    if ($(item).text().trim() == itemComp.innerText.trim()) {
                        IsAlreadyExist = true;
                    }
                }
            });

            if (!IsAlreadyExist) {
                diseaseId = $(this).attr('id');
                $(this).addClass("disableAll");
                Icd10Description = $(this).find('a').html();
                var li = "<li icd9Code='" + $(this).attr('icd9Code') + "'  icd10Code='" + $(this).attr('icd10Code') + "' snomedCode='" + $(this).attr('snomedCode') + "' icd9Desc='" + $(this).attr('icd9Desc') + "' icd10Desc='" + $(this).attr('icd10Desc') + "' snomedDesc='" + $(this).attr('snomedDesc') + "' id=" + diseaseId + " onclick='Clinical_HPIComplaints.fillChiefComplaints(this, event);'>" +
                    "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + $(this).attr('icd10Desc') + "</span></a>" +
                        "<span class='removeIconListHover'>" +
                            "<a href='#' onclick='Clinical_HPIComplaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                "<a href='#' onclick='Clinical_HPIComplaints.deleteChiefComplaint(" + diseaseId + ", event);'><i class='fa fa-times red'></i></a>" +
		                "</span>" +
                    "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_HPIComplaints.updateComplaintsText(this, event);'/>" +
                    "<div class='clearfix'></div>" +
                "</li>";
                $('#pnlClinicalHPIComplaints #ulCompliantDisease').append(li);
                Clinical_HPIComplaints.AddInArray(diseaseId, $(this).attr('icd10Desc'), true);
            }
            else {
                utility.DisplayMessages('Disease already added', 2);
            }
        });
    },
    //End M Ahmad Imran 24/03/2016

    // Start 7/2/2016 Muhammad Ahmad Imran
    //Purpose Save/update favList Status
    SaveFavToggelStatus: function (FavListVal) {
        var isFavListOpened = $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #favSectionDiv").hasClass("toggledHor");
        $.when(EMRUtility.insertUpdateFavListStatus(Clinical_HPIComplaints.FavListName, isFavListOpened)).then(function () {
            EMRUtility.insertUpdateFavListVal(Clinical_HPIComplaints.FavListName, FavListVal);
        });
    },
    // End 7/2/2016 Muhammad Ahmad Imran

    //Start//10/02/2016//M Ahmad Imran//Implimented Call to Controller for Save Complaint Detail
    SaveComplaint: function (updateFromProgressNote) {
        //var updateFromProgressNote = updateFromProgressNote ? JSON.parse(updateFromProgressNote) : "";
        var objData = {};
        var self = $('#pnlClinicalHPIComplaints');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["DateCaptured"] = objDetail["DateCaptured"];
        objData["ComplaintId"] = updateFromProgressNote ? updateFromProgressNote[0].ComplaintId : objDetail["ComplaintId"];
        objData["OverallComments"] = objDetail["OverallComments"];
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ComplaintDetails"] = updateFromProgressNote ? updateFromProgressNote : Clinical_HPIComplaints.ComplaintsDetail;
        objData["commandType"] = "SAVE_Complaint";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Complaint");
    },
    //End M Ahmad Imran 10/2/2016
    UpdateComplaintFromNotes: function (updateFromProgressNote) {
        var objData = {};
        objData["ComplaintId"] = updateFromProgressNote[0].ComplaintId;
        objData["ComplaintDetails"] = updateFromProgressNote;
        objData["commandType"] = "update_complaint_from_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Complaint");
    },

    //Start//16/02/2016//M Ahmad Imran//Implimented Call to Controller for Delete Complaint Detail
    DeleteComplaint: function (ComplaintDetailId) {

        var ComplaintDetails = [];
        var item = {};
        item["ComplaintDetailId"] = ComplaintDetailId;
        ComplaintDetails.push(item);
        var objData = {};
        objData["ComplaintDetails"] = ComplaintDetails;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "DELETE_Complaint";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Complaint");
    },
    //End M Ahmad Imran 16/2/2016
    //Start//16/02/2016//M Ahmad Imran//Implimented Call to Controller for Reset Complaint
    ComplaintReset: function (ComesFromProgressNotes, ComplaintId) {
        var objData = {};
        if (ComesFromProgressNotes) {
            objData["ComplaintId"] = ComplaintId;
        }
        else {
            objData["ComplaintId"] = $('#pnlClinicalHPIComplaints #txtComplaintId').val();
        }
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "RESET_Complaint";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Complaint");
    },
    //End M Ahmad Imran 10/2/2016

    //Start//10/02/2016//M Ahmad Imran//For record Complaint
    AddInArray: function (id, value, IsSelect) {
        var item = {};
        item["ComplaintDetailId"] = id;
        item["ComplaintDescription"] = value;
        item["Comments"] = "";
        Clinical_HPIComplaints.Complaints.push(item);
        if (IsSelect) {
            Clinical_HPIComplaints.fillChiefComplaints($('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li#" + id + ""), null);
        }

    },
    //End M Ahmad Imran 10/2/2016

    //Start//23/03/2016//M Ahmad Imran//For record Fav Complaint
    AddArrayInFavComplaints: function (id, value) {
        item = {}
        item["FavComplaintId"] = id;
        item["FavComplaintDescription"] = value;
        Clinical_HPIComplaints.FavComplaints.push(item);
    },
    //End M Ahmad Imran 23/03/2016

    /* -----------------Progress Note-------------

    Reason: These functions are used for Progress Note Soap Attachment, creation and detachment */

    /* Call Back function to add component to Progress Note
       */
    addComplaintToNotes: function () {
        Clinical_HPIComplaints.SaveComplaint();
    },

    /*  Wasim Malik
    Start/ 17/2/2016.
    This function will get Birth History Soap Text and attach that to Progress note
     */
    getComplaintInfo: function (unloadComplaint, complaintId) {
        if (unloadComplaint == null) {
            unloadComplaint = false;
        }
        Clinical_HPIComplaints.fillBirthHx_DBCall(complaintId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_HPIComplaints.createComplaintBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', unloadComplaint);
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });
    },


    /*  Wasim Malik
    Start/ 17/2/2016.
    This Function will check, if Complaint SOAP is already attached in Progress note, if Birth History is not attached than it will create main divs to attach
      */
    checkComplaintHxExists: function (ComplaintId) {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ComplaintsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<Clinical_Complaints title="Complaints"  id="' + Clinical_HPIComplaints.ComplaintId + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'HPIComplaints\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Complaints">Complaints</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Complaints\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Complaints\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</Clinical_Complaints> </header></li>');
            Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    createComplaintBodyHTMLFromNotes: function (OverAllComments, Complaints, noteHTMLCtrl, unloadComplaint, hideAlertMessage) {

        if (Complaints && Complaints.ComplaintDetailId && Complaints.ComplaintDetailId > 0) {
            var ComplaintHxId = Complaints.ComplaintDetailId;

            Clinical_HPIComplaints.checkComplaintHxExists(Complaints.ComplaintDetailId);
            var ComplaintFill_Obj = Complaints;
            var $mainDivComplaintHx = $(document.createElement('div'));

            var $CCdiv = $(document.createElement('div'));
            var $SectionBodyComplaint = $(document.createElement('section'));
            var complaintsCC = "";
            var $ListComplaint = $(document.createElement('ul'));
            var $DetailsDiv = $(document.createElement('div'));
            var modifieddate = "";

            var ComplaintComments = $('#' + Clinical_ProgressNote.params["PanelID"]).find('#Comments_Cli_Complaint_' + ComplaintHxId);
            var $HistoryListComplaint = $(document.createElement('ul'));
            var $HistoryDetailsDiv = $(document.createElement('div'));
            $HistoryListComplaint.attr('class', 'list-unstyled');
            $ListComplaint.attr('class', 'list-unstyled');
            $mainDivComplaintHx.attr('id', "Section_Complaint");
            var divId = '"Cli_Complaint_' + ComplaintHxId + '"';
            var ServerDateTime = $("#mainForm #userCurrentTime").html().split(" ")
            var d = new Date(ServerDateTime[1] + " " + ServerDateTime[2] + " " + ServerDateTime[3]);
            var UpdateDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
            if (Complaints.length > 0) {
                $HistoryListComplaint.append("<li class='btn btn-link p-none' style='color:#428BCA; margin-top:5px; cursor:pointer;' onclick='Clinical_ProgressNote.addComment(" + divId + ")'>" + "History of Present Illness" + "</li>")
            }
            if (Complaints.length > 0) {
                $SectionBodyComplaint.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Complaint_" + ComplaintHxId + '"><i class="fa fa-edit"></i></a>' +
                  '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Complaint_Main" + ComplaintHxId + '"  onclick="javascript:void(0);"><i class="fa fa-times"></i></a></div> ');
            }
            $HistoryListComplaint.append("<li><p>");
            $(ComplaintFill_Obj).each(function (i, item) {
                if ($(this).attr("ComplaintId") != null)

                    modifieddate = item.ModifiedOn;
                if (item.IsChiefComplaint == false || item.IsChiefComplaint == "False") {
                    $ListComplaint.append("<li>" + "<span>" + item.ComplaintDescription + "</span>" + "</li>");
                    complaintsCC = item.ComplaintDescription;
                    var $DetailsDiv = "<span style='font-weight:bold'>" + complaintsCC + "</span>";
                }
                else {
                    $CCdiv.attr('style', 'color:red');
                    $CCdiv.html("Chief Complaint");
                    $ListComplaint.append("<li>" + "<span>" + item.ComplaintDescription + "</span>" + "<span> (Chief Complaint)</span>" + "</li>");
                    complaintsCC = item.ComplaintDescription;
                    var $DetailsDiv = "<span style='font-weight:bold'>" + complaintsCC + " (Chief Complaint)</span>";
                }
                var StartLine = "";
                if (i == 0) {
                    StartLine = "A " + patientInfo.age + " old " + patientInfo.gender + " presents with ";
                }
                else if(i == ComplaintFill_Obj.length - 1)
                    StartLine = " and ";
                else {
                    StartLine = "";
                }
                var endingLiteral = "";
                if (OverAllComments != null || item.OverallComments != "" || item.OverallComments != null) {
                    var detail = (item.PreviousHistory != "" ? " Previous History: " + item.PreviousHistory + "." : "") + ((item.Complaint_CaseId_text != "- Select -" && item.Complaint_CaseId_text != "") ? " Case is " + item.Complaint_CaseId_text + "." : "") + ((item.Complaint_LocationIds_text != "" && typeof item.Complaint_LocationIds_text != "undefined") ? " Location: " + item.Complaint_LocationIds_text + "." : "") + ((item.Complaint_RadiationId_text != "- Select -" && item.Complaint_RadiationId_text != "") ? " Radiates to: " + item.Complaint_RadiationId_text + "." : "") + ((item.Complaint_QualityId_text != "- Select -" && item.Complaint_QualityId_text != "") ? " Quality is " + item.Complaint_QualityId_text + "." : "") + ((item.Complaint_SeverityId_text != "- Select -" && item.Complaint_SeverityId_text != "") ? " Severity is " + item.Complaint_SeverityId_text + "." : "") + (item.Onset != "" ? " Onset: " + item.Onset + "." : "") + (item.Duration != "" ? " Duration is " + item.Duration : "") + ((item.Complaint_DurationId_text != "- Select -" && item.Complaint_DurationId_text != "") ? " " + item.Complaint_DurationId_text + "." : "") + ((item.Complaint_FrequencyId_text != "- Select -" && item.Complaint_FrequencyId_text != "") ? " Frequency is " + item.Complaint_FrequencyId_text + "." : "") + ((item.Complaint_ContextId_text != "- Select -" && item.Complaint_ContextId_text != "") ? " Context: " + item.Complaint_ContextId_text + "." : "") + ((item.Complaint_CharacterIds_text != "" && typeof item.Complaint_CharacterIds_text != "undefined") ? " Character is " + item.Complaint_CharacterIds_text + "." : "") + (item.AssociatedWith != "" ? " Associated with " + item.AssociatedWith + "." : "") + (item.PrecipitatedBy != "" ? " Precipitated by " + item.PrecipitatedBy + "." : "") + ((item.Complaint_AggravatedById_text != "- Select -" && item.Complaint_AggravatedById_text != "") ? " Aggravated by " + item.Complaint_AggravatedById_text + "." : "") + ((item.Complaint_RelievedById_text != "- Select -" && item.Complaint_RelievedById_text != "") ? " Relieved by " + item.Complaint_RelievedById_text + "." : "") + (item.Comments != "" ? " " + item.Comments : "")
                    var formattedDetail = "";
                    if (!(detail.trim() == ""))
                        formattedDetail = "<b> (</b>" + detail.trim() + "<b>)" + ((i == ComplaintFill_Obj.length - 1) ? "." : ";") + "</b>";
                    if (detail.trim() == "")
                        endingLiteral = ";";
                    else if (ComplaintFill_Obj.length > 1 && i == ComplaintFill_Obj.length - 1 && detail.trim() == "")
                        endingLiteral = ".";
                    if (i == ComplaintFill_Obj.length - 1 && detail.trim() == "")
                        endingLiteral = ".";
                    $HistoryListComplaint.find('p').append(" " + StartLine + $DetailsDiv + "<span style='font-weight:bold'>" + endingLiteral + "</span>" + formattedDetail);
                }
                else {
                    $HistoryListComplaint.find('p').append(StartLine + $DetailsDiv);
                }

            });

            if (Complaints.length > 0) {
                if (OverAllComments != null) {
                    var allComments = OverAllComments != "" ? OverAllComments : "";
                    $HistoryListComplaint.find('p').append('<br>' + allComments);
                }
                else if (ComplaintFill_Obj[0].OverallComments != "") {
                    var allComments = ComplaintFill_Obj[0].OverallComments != "" ? ComplaintFill_Obj[0].OverallComments : "";
                    $HistoryListComplaint.find('p').append('<br>' + allComments);
                }
                if (isNaN(UpdateDate) == false) {
                    $HistoryListComplaint.append("</br> Last Updated on " + UpdateDate);
                }
            }
            else {
                $HistoryListComplaint.html("");
            }
            $HistoryDetailsDiv.attr('id', "Cli_Complaint_History_" + ComplaintHxId);
            $DetailsDiv.attr('id', "Cli_Complaint_" + ComplaintHxId);
            $ListComplaint.append($HistoryListComplaint.html())
            if (ComplaintComments.html() != undefined) {
                $ListComplaint.append(ComplaintComments[0].outerHTML);
            }
            $DetailsDiv.html($ListComplaint);

            $SectionBodyComplaint.append($DetailsDiv);

            $SectionBodyComplaint.attr('id', "Cli_Complaint_Main" + ComplaintHxId);

            if ($(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId).length == 0) {

                if ($(noteHTMLCtrl).find('#Section_Complaint').length > 0) {
                    $(noteHTMLCtrl).find('#Section_Complaint').remove();
                }
                $mainDivComplaintHx.html($SectionBodyComplaint);
                //Clinical_HPIComplaints.updateComplaintHtml($mainDivComplaintHx.append(), ComplaintHxId, noteHTMLCtrl, hideAlertMessage);

                if (!$(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().hasClass('initialVisitBody')) {
                    $(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().addClass('initialVisitBody');
                }
                var complaintHTML = $mainDivComplaintHx.append();
                if (complaintHTML != '') {
                    $(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().append(complaintHTML);
                }
                return ComplaintHxId;
            } else {

                var CommentHTML = "";
                var CommentsID = $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId + ' ul li:Last').get(0).outerHTML;
                }
                if ($(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId))
                    $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId).remove();
                $mainDivComplaintHx.html($SectionBodyComplaint);

            }

        }
    },

    /*Wasim Malik
    Start/ 17/2/2016.
    This Function is used to create SOAP html and append it to  Progress note
     */
    createComplaintBodyHTML: function (OverAllComments, response, noteHTMLCtrl, unloadComplaint, hideAlertMessage, ComplaintHxId) {

        var defferd = $.Deferred();
        if (!ComplaintHxId)
            ComplaintHxId = $("#txtComplaintId").val();

        if (typeof $("#txtComplaintId").val() === "undefined") {
            if (response[0] != null) {
                ComplaintHxId = response[0].Expr1 ? response[0].Expr1 : $('#ProgressnoteHTML section [id^=Cli_Complaint]').attr('id').split('_')[2];
            } else {
                if (!ComplaintHxId)
                    ComplaintHxId = null;
            }

        }
        if (Clinical_HPIComplaints.IsReset) {
            ComplaintHxId = Clinical_HPIComplaints.ResetedComplaitId;
        }
        var patientInfo = Clinical_HPIComplaints.GetPatientInfo();

        Clinical_HPIComplaints.checkComplaintHxExists(ComplaintHxId);
        var ComplaintFill_Obj = response;//JSON.parse(response.ComplaintSoap_JSON);
        var $mainDivComplaintHx = $(document.createElement('div'));
        $mainDivComplaintHx.attr('id', "Section_Complaint");
        var $CCdiv = $(document.createElement('div'));
        var $SectionBodyComplaint = $(document.createElement('section'));
        var complaintsCC = "";
        //var complaintIDs = "";
        var $ListComplaint = $(document.createElement('ul'));
        var $DetailsDiv = $(document.createElement('div'));
        var modifieddate = "";

        var ComplaintComments = $('#' + Clinical_ProgressNote.params["PanelID"]).find('#Comments_Cli_Complaint_' + ComplaintHxId);
        var $HistoryListComplaint = $(document.createElement('ul'));
        var $HistoryDetailsDiv = $(document.createElement('div'));
        $HistoryListComplaint.attr('class', 'list-unstyled')
        $ListComplaint.attr('class', 'list-unstyled')
        var divId = '"Cli_Complaint_' + ComplaintHxId + '"';
        var ServerDateTime = $("#mainForm #userCurrentTime").html().split(" ")
        var d = new Date(ServerDateTime[1] + " " + ServerDateTime[2] + " " + ServerDateTime[3]);
        var UpdateDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
        if (response.length > 0) {
            $HistoryListComplaint.append("<li class='btn btn-link p-none' style='color:#428BCA; margin-top:5px; cursor:pointer;' onclick='Clinical_ProgressNote.addComment(" + divId + ")'>" + "History of Present Illness" + "</li>")
        }
        if (response.length > 0) {
            $SectionBodyComplaint.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Complaint_" + ComplaintHxId + '"><i class="fa fa-edit"></i></a>' +
              '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Complaint_Main" + ComplaintHxId + '"  onclick="javascript:void(0);"><i class="fa fa-times"></i></a></div> ');
        }
        $HistoryListComplaint.append("<li id='liHpi'><p>");
        $(ComplaintFill_Obj).each(function (i, item) {
            if ($(this).attr("ComplaintId") != null)

                modifieddate = item.ModifiedOn;
            if (item.IsChiefComplaint == false || item.IsChiefComplaint == "False") {
                //Begin 4/26/16  Edit By M Ahmad Imran Bug # EMR-423
                $ListComplaint.append("<li complaintdetailid = '" + item.ComplaintDetailId + "' id='" + i + "' class='sopTextEditable ui-sortable-handle editableContentli'>"
                    + "<span class='editable' style='display: block;'>" + item.ComplaintDescription + "</span>"
                    + "<textarea class='edit form-control' spellcheck='true' style='display: none;'></textarea>"
                    + "</li>");
                //End 4/26/16  Edit By M Ahmad Imran Bug # EMR-423
                complaintsCC = item.ComplaintDescription;
                var $DetailsDiv = "<span style='font-weight:bold'>" + complaintsCC + "</span>";
            }
            else {
                $CCdiv.attr('style', 'color:red')
                //Begin 4/26/16  Edit By M Ahmad Imran Bug # EMR-423
                $CCdiv.html("Chief Complaint");
                $ListComplaint.append("<li complaintdetailid = '" + item.ComplaintDetailId + "' id='" + i + "' class='sopTextEditable ui-sortable-handle editableContentli'>"
                    + "<span class='editable' style='display: block;'>" + item.ComplaintDescription + " (Chief Complaint) </span>"
                    + "<textarea class='edit form-control' spellcheck='true' style='display: none;'></textarea>"
                    + "</li>");

                //("<li>" + "<span>" + item.ComplaintDescription + "</span>" + "<span> (Chief Complaint)</span>" + "</li>");
                complaintsCC = item.ComplaintDescription;
                var $DetailsDiv = "<span style='font-weight:bold'>" + complaintsCC + " (Chief Complaint)</span>";
                //End 4/26/16  Edit By M Ahmad Imran Bug # EMR-423

            }
            var StartLine = "";
            if (i == 0)
                StartLine = "<span> A " + patientInfo.age + " old " + patientInfo.gender + " presents with ";
            else if (i == ComplaintFill_Obj.length - 1)
                StartLine = "<span> and ";
            else
                StartLine = "<span>";
            //Begin 4/26/16  Edit By M Ahmad Imran Bug # EMR-423
            var endingLiteral = "";
            if (OverAllComments != null || item.OverallComments != "" || item.OverallComments != null) {
                var detail = (item.PreviousHistory && item.PreviousHistory != "" ? " Previous History: " + item.PreviousHistory + "." : "")
                    + ((item.Complaint_CaseId_text && item.Complaint_CaseId_text != "- Select -" && item.Complaint_CaseId_text != "") ? " Case is " + item.Complaint_CaseId_text + "." : "")
                    + ((item.Complaint_LocationIds_text && item.Complaint_LocationIds_text != "" && typeof item.Complaint_LocationIds_text != "undefined") ? " Location: " + item.Complaint_LocationIds_text + "." : "")
                    + ((item.Complaint_RadiationId_text && item.Complaint_RadiationId_text != "- Select -" && item.Complaint_RadiationId_text != "") ? " Radiates to: " + item.Complaint_RadiationId_text + "." : "")
                    + ((item.Complaint_QualityId_text && item.Complaint_QualityId_text != "- Select -" && item.Complaint_QualityId_text != "") ? " Quality is " + item.Complaint_QualityId_text + "." : "")
                    + ((item.Complaint_SeverityId_text && item.Complaint_SeverityId_text != "- Select -" && item.Complaint_SeverityId_text != "") ? " Severity is " + item.Complaint_SeverityId_text + "." : "")
                    + (item.Onset && item.Onset != "" ? " Onset: " + item.Onset + "." : "")
                    + (item.Duration && item.Duration != "" ? " Duration is " + item.Duration : "")
                    + ((item.Complaint_DurationId_text && item.Complaint_DurationId_text != "- Select -" && item.Complaint_DurationId_text != "") ? " " + item.Complaint_DurationId_text + "." : "")
                    + ((item.Complaint_FrequencyId_text && item.Complaint_FrequencyId_text != "- Select -" && item.Complaint_FrequencyId_text != "") ? " Frequency is " + item.Complaint_FrequencyId_text + "." : "")
                    + ((item.Complaint_ContextId_text && item.Complaint_ContextId_text != "- Select -" && item.Complaint_ContextId_text != "") ? " Context: " + item.Complaint_ContextId_text + "." : "")
                    + ((item.Complaint_CharacterIds_text && item.Complaint_CharacterIds_text != "" && typeof item.Complaint_CharacterIds_text != "undefined") ? " Character is " + item.Complaint_CharacterIds_text + "." : "")
                    + (item.AssociatedWith && item.AssociatedWith != "" ? " Associated with " + item.AssociatedWith + "." : "")
                    + (item.PrecipitatedBy && item.PrecipitatedBy != "" ? " Precipitated by " + item.PrecipitatedBy + "." : "")
                    + ((item.Complaint_AggravatedById_text && item.Complaint_AggravatedById_text != "- Select -" && item.Complaint_AggravatedById_text != "") ? " Aggravated by " + item.Complaint_AggravatedById_text + "." : "")
                    + ((item.Complaint_RelievedById_text && item.Complaint_RelievedById_text != "- Select -" && item.Complaint_RelievedById_text != "") ? " Relieved by " + item.Complaint_RelievedById_text + "." : "")
                    + (item.Comments && item.Comments != "" ? " " + item.Comments : "")
                var formattedDetail = "";
                if (!(detail.trim() == ""))
                    formattedDetail = "<b> (</b>" + detail.trim() + "<b>)" + ((i == ComplaintFill_Obj.length - 1) ? "." : ";") + "</b>";
                if (detail.trim() == "")
                    endingLiteral = ";";
                else if (ComplaintFill_Obj.length > 1 && i == ComplaintFill_Obj.length - 1 && detail.trim() == "")
                    endingLiteral = ".";
                if (i == ComplaintFill_Obj.length - 1 && detail.trim() == "")
                    endingLiteral = ".";
                
                $HistoryListComplaint.find('p').append(" " + StartLine + $DetailsDiv + "<span style='font-weight:bold'>" + endingLiteral + "</span>" + formattedDetail
                    + '</span>'
               );
            }
            else {
                $HistoryListComplaint.find('p').append(StartLine + $DetailsDiv);
            }
        });

        if (response.length > 0) {
            if (Clinical_HPIComplaints.params.HPITemplateId <= 0) {
                if (OverAllComments != null) {
                    var allComments = OverAllComments != "" ? OverAllComments : "";
                    $HistoryListComplaint.find('p').append('<br>' + allComments);
                }
                else if (ComplaintFill_Obj[0].OverallComments) {
                    var allComments = ComplaintFill_Obj[0].OverallComments != "" ? ComplaintFill_Obj[0].OverallComments : "";
                    $HistoryListComplaint.find('p').append('<br>' + allComments);
                }
            }

            if (isNaN(UpdateDate) == false) {
                $HistoryListComplaint.append("</br> Last Updated on " + UpdateDate);
            }
        }
        else {
            Clinical_ProgressNote.params["IsBodyPart"] = false;
            $HistoryListComplaint.html("");
        }
        $HistoryDetailsDiv.attr('id', "Cli_Complaint_History_" + ComplaintHxId);
        $DetailsDiv.attr('id', "Cli_Complaint_" + ComplaintHxId);
        $ListComplaint.append($HistoryListComplaint.html())
        if (ComplaintComments.html() != undefined) {
            $ListComplaint.append(ComplaintComments[0].outerHTML);
        }
        $DetailsDiv.html($ListComplaint);
        //History
        $SectionBodyComplaint.append($DetailsDiv);
        //
        $SectionBodyComplaint.attr('id', "Cli_Complaint_Main" + ComplaintHxId);


        if ($(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId).length == 0 || Clinical_HPIComplaints.params.HPITemplateId > 0) {

            if ($(noteHTMLCtrl).find('#Section_Complaint')) {
                $(noteHTMLCtrl).find('[id="Section_Complaint"]').remove();
            }
            $mainDivComplaintHx.html($SectionBodyComplaint);
            Clinical_HPIComplaints.updateComplaintHtml($mainDivComplaintHx.append(), ComplaintHxId, noteHTMLCtrl, hideAlertMessage);
        } else {

            var CommentHTML = "";
            var CommentsID = $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId + ' ul li:Last').attr('id');
            if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                CommentHTML = $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId + ' ul li:Last').get(0).outerHTML;
            }
            if ($(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId))
                $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId).remove();
            if ($(noteHTMLCtrl).find('#Section_Complaint #Cli_Complaint_Main' + ComplaintHxId)) {
                $(noteHTMLCtrl).find('#Section_Complaint #Cli_Complaint_Main' + ComplaintHxId).remove();
            }
            if ($(noteHTMLCtrl).find('#allComments')) {
                $(noteHTMLCtrl).find('#allComments').remove();
            }

            $mainDivComplaintHx.html($SectionBodyComplaint);

            Clinical_HPIComplaints.updateComplaintHtml($mainDivComplaintHx.append(), ComplaintHxId, noteHTMLCtrl, hideAlertMessage);
        }
        Clinical_ProgressNote.initilizeEditableContent();
        if (unloadComplaint == true) {
            Clinical_HPIComplaints.unloadComplaint();
        }



        var tagsChangedInterval = 500;
        var ProcessDB = {
        };
        ProcessDB.def = defferd;
        ProcessDB.func = setInterval(function () {
            if (xhrPool.length <= 0) {
                clearInterval(ProcessDB.func);
                ProcessDB.def.resolve('ok');

            }
        }, tagsChangedInterval);

        return defferd;

    },
    GetPatientInfo: function () {

        var $patientInfoAndAddress = $("#lblPatientData span#banner_PatientInfoAndAddress");

        if ($patientInfoAndAddress && $patientInfoAndAddress.is("[data-original-title]")) {
            var information = $patientInfoAndAddress.attr("data-original-title");
            return { age: information.split(',')[1].trim().toLowerCase(), gender: information.split(',')[2].trim().toLowerCase() };
        }

        //return $("#lblPatientData span#banner_PatientInfoAndAddress").attr("data-original-title").split(',')[1].trim();
    },
    unloadComplaint: function () {
        if (Clinical_HPIComplaints.params["FromAdmin"] == "0") {
            if (Clinical_HPIComplaints.params != null && Clinical_HPIComplaints.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_HPIComplaints.params.ParentCtrl, 'Clinical_HPIComplaints');
            }
            else
                UnloadActionPan(null, 'Clinical_HPIComplaints');
        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_BirthHx").remove();
            RemoveAdminTab();
        }
    },
    /*  Wasim Malik
    Start/ 17/2/2016.
    This Function is called by Progress Notes (Fill Complaint Func, CopyAllNotesCategories)
      */
    updateComplaintHtml: function (birthHxHtml, ComplaintHxId, noteHTMLCtrl, hideAlertMessage) {
        if (!$(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().hasClass('initialVisitBody')) {
            $(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().addClass('initialVisitBody');
        }
        else {

        }

        if (birthHxHtml != '') {
            $(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().append(birthHxHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (birthHxHtml != '' && Clinical_HPIComplaints.IsReset == false) {
            Clinical_HPIComplaints.attachComplaintFromNotes(ComplaintHxId, hideAlertMessage);
        }

    },

    removeChiefComplaintProblems: function (ComplaintId, ComplainDetailId) {

        var $row = null;
        if (ComplainDetailId) {
            $row = $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML section[complaintDetailId='" + ComplainDetailId + "']");

        }
        else if (ComplaintId) {
            $row = $("#" + Clinical_ProgressNote.params["PanelID"] + " #ProgressnoteHTML section[complaintId='" + ComplaintId + "']");
        }

        if ($row) {
            $row.remove();

            $.when(Clinical_ProgressNote.saveComponentSOAPText('Problems', true)).then(function () {

                Clinical_ProgressNote.HideShowBillingInfo();
            });
        }


    },

    /*  Wasim Malik
    Start/ 17/2/2016.
    This Function detach Complaint From progress note
     */
    detach_ComponentsComplaints: function (componentName, isUpdate, ComponentRemove, hpiTemplateId) {

        var Clinical_HPIComplaintsIds = '';
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ComplaintsComponent').attr('NoteComponentId');

        if (hpiTemplateId) {
            Clinical_HPIComplaintsIds = hpiTemplateId;
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Complaints').parent().parent().find('section[id="Cli_HPIComplaints_Temp' + hpiTemplateId + '"]').remove();
            //  $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Complaints').parent().parent().find('section[id="Cli_PhysicalExam_Main_Sys' + PETemplateId + '"]').remove();
        }
        else {
            var Clinical_HPIComplaintsIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Complaints').parent().parent().find('section[id*="Cli_Complaint_Main"]').map(function () {
                return this.id.replace("Cli_Complaint_Main", "");
            }).get().join(',');
            var ComplaintId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').attr('id');

            if (ComponentRemove) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Complaints']").remove();
                if (Clinical_ProgressNote.params["TemplateName"])
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().addClass('hidden');
                else
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().remove();
            } else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().find('section[id*="Cli_Complaint_Main"]').remove();
            }
        }

        if (Clinical_HPIComplaintsIds == "" || Clinical_HPIComplaintsIds == "undefined") {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Complaints'))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    Clinical_HPIComplaints.removeChiefComplaintProblems(ComplaintId);
                    if (Clinical_ProgressNote.params["TemplateName"] == "" && ComponentRemove)
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            Clinical_HPIComplaints.ComplaintReset(true, Clinical_HPIComplaintsIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (isUpdate) {

                        if (ComponentRemove) {
                            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                                var promise = [];
                                if (Clinical_ProgressNote.params["TemplateName"]) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().addClass('hidden');
                                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Complaints'))
                                }
                                else
                                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                                $.when.apply($, promise).done(function () {
                                    Clinical_HPIComplaints.removeChiefComplaintProblems(ComplaintId);
                                    if (Clinical_ProgressNote.params["TemplateName"] == "" && ComponentRemove)
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().remove();
                                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                                });
                            }
                        } else {
                            Clinical_ProgressNote.saveComponentSOAPText('Complaints', true);
                            Clinical_ProgressNote.hoverFunction();
                        }
                    }
                    Clinical_ProgressNote.params["IsBodyPart"] = false;
                    utility.DisplayMessages('Successfully Deleted', 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }

    },


    /*  Wasim Malik
    Start/ 17/2/2016.
    This Functions ask for Detaching Complaint from Progress Note for current Patient Selected
    */
    detachComplaintFromNotes: function (ComplaintHxId) {
        utility.myConfirm('28', function () {
            var selectedValue = ComplaintHxId.replace('Cli_Complaint_Main', '');
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_HPIComplaints.detachComplaintFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ComplaintsComponent').attr('NoteComponentId');
                        $('#' + ComplaintHxId).remove();

                        Clinical_ProgressNote.params["IsBodyPart"] = false;
                        Clinical_ProgressNote.saveComponentSOAPText('Complaints');
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
    },

    /*  Wasim Malik
    Start/ 17/2/2016.
    This Functions attached Birth Hx to Progress Note for current Patient Selected
    */
    attachComplaintFromNotes: function (ComplaintHxId, hideAlertMessage) {
        var selectedValue = ComplaintHxId;
        if (selectedValue == "" || selectedValue == null || typeof selectedValue == "undefined") {
        }
        else {
            Clinical_HPIComplaints.attachComplaintFromNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    //If Attached BirthHx Made new inseration to BirthHx Table than good ids should be attached to HTML
                    Clinical_ProgressNote.saveComponentSOAPText('Complaints');
                    // Grid row was removing which was attaching to Note
                    //  $('#' + ComplaintHxId).remove();
                    // utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    /*  Wasim Malik
    Start/ 17/2/2016.
    If BirthHx Component which is dropeed in Progress note has no Complaint attached, than it will call for Latest Complaint for this patient
    */
    getLatestComplaintByPatientId: function (hideAlertMessage) {
        Clinical_HPIComplaints.getLatestClinical_HPIComplaintsByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_HPIComplaints.createComplaintBodyHTML(null, JSON.parse(response.ComplaintSoap_JSON || "[]"), '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }

        });
    },

    /*  Wasim Malik
    Start/ 17/2/2016.
    DB Call to detach Complaint from the Notes
    */
    detachComplaintFromNotes_DBCall: function (ComplaintHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ComplaintId"] = ComplaintHxId;
        objData["commandType"] = "detach_Complaints_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "Medical", "Complaint");
    },

    /*  Wasim Malik
    Start/ 17/2/2016.
    DB Call to attach Complaint from the Notes
    */
    attachComplaintFromNotes_DBCall: function (ComplaintHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        if (ComplaintHxId) {
            objData["ComplaintId"] = ComplaintHxId;
        }
        else if ($("#txtComplaintId").val()) {
            objData["ComplaintId"] = $("#txtComplaintId").val();
        }
        else
            objData["ComplaintId"] = 0;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }
        }

        objData["commandType"] = "attach_Complaints_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "Medical", "Complaint");
    },

    /*  Wasim Malik
    Start/ 17/2/2016.
    Retrieves latest Complaint data against the PatientId
    */
    getLatestClinical_HPIComplaintsByPatientId_DBCall: function () {
        var objData = new Object();
        if ($("#txtComplaintId").val() != "" && typeof $("#txtComplaintId").val() !== "undefined") {
            objData["ComplaintId"] = $("#txtComplaintId").val();
        }
        else {
            objData["ComplaintId"] = 0;
        }
        objData["commandType"] = "get_Complaints_forsoap";
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Complaint");
    },

    //--------------end progress Note-----------

    AddComplaintsToProblemList: function (ComplaintsDetail) {
        //AppPrivileges.GetFormPrivileges("Medical_Problems List", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        //        $("#pnlClinicalHPIComplaints #ulCompliantDisease li").each(function () {
        //            if ($(this).attr('icd9code') || $(this).attr('icd10code')) {
        //                Clinical_HPIComplaints.SaveProblemLists($(this)).done(function (response) {
        //                    response = JSON.parse(response);
        //                    if (response.status != false) {
        //                        if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote") {
        //                            Clinical_HPIComplaints.getProblemListsInfo(response.ProblemListId);
        //                        }
        //                    }
        //                    else {
        //                        utility.DisplayMessages(response.Message, 3);
        //                    }
        //                });
        //            }
        //        });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});

        $.each(ComplaintsDetail, function (i, e) {
            if (ComplaintsDetail[i].icd9code || ComplaintsDetail[i].icd10code) {
                Clinical_HPIComplaints.SaveProblemLists(ComplaintsDetail[i]).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote") {
                            Clinical_HPIComplaints.getProblemListsInfo(response.ProblemListId, true, ComplaintsDetail[i].ComplaintDetailId);

                        }
                    }
                    else if (response.Message.indexOf('problem already exists') >= 0) {
                        var problemId = response.ProblemListId;
                        if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote") {
                            Clinical_HPIComplaints.getProblemListsInfo(problemId, true, ComplaintsDetail[i].ComplaintDetailId);
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            } else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + ComplaintsDetail[i].ProblemListId).remove();
            }
        });

    },
    SaveProblemLists: function (obj) {
        var objData = {};

        objData["ComplaintDetailId"] = obj.ComplaintDetailId;
        objData["ComplaintId"] = $('#txtComplaintId').val();
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["ICD9"] = obj.icd9code;
        objData["ICD10"] = obj.icd10code;
        objData["ICD9_Description"] = obj.icd9desc;
        objData["ICD10_Description"] = obj.icd10desc;
        objData["SNOMEDID"] = obj.snomedcode;
        objData["SNOMED_DESCRIPTION"] = obj.snomeddesc;

        //Start for wrong snomed code
        if (Clinical_HPIComplaints.LastSctBaseSearch != "") {
            if (Clinical_HPIComplaints.LastSctBaseSearch == "981000124106" && objData["SNOMEDID"] != "981000124106") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "981000124106")
                }
                objData["SNOMEDID"] = "981000124106";
                objData["SNOMED_DESCRIPTION"] = "Moderate left ventricular systolic dysfunction";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "385093006" && objData["SNOMEDID"] != "385093006") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "385093006")
                }
                objData["SNOMEDID"] = "385093006";
                objData["SNOMED_DESCRIPTION"] = "Community Acquired Pneumonia";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "5281000124103" && objData["SNOMEDID"] != "5281000124103") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "5281000124103")
                }
                objData["SNOMEDID"] = "5281000124103";
                objData["SNOMED_DESCRIPTION"] = "Persistent asthma";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "195967001" && objData["SNOMEDID"] != "195967001") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "195967001")
                }
                objData["SNOMEDID"] = "195967001";
                objData["SNOMED_DESCRIPTION"] = "Asthma";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "233604007" && objData["SNOMEDID"] != "233604007") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "233604007")
                }
                objData["SNOMEDID"] = "233604007";
                objData["SNOMED_DESCRIPTION"] = "Pneumonia";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "59621000" && objData["SNOMEDID"] != "59621000") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "59621000")
                }
                objData["SNOMEDID"] = "59621000";
                objData["SNOMED_DESCRIPTION"] = "Essential hypertension";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "95891005" && objData["SNOMEDID"] != "95891005") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "95891005")
                }
                objData["SNOMEDID"] = "95891005";
                objData["SNOMED_DESCRIPTION"] = "Flu-like symptoms";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "363746003" && objData["SNOMEDID"] != "363746003") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "363746003")
                }
                objData["SNOMEDID"] = "363746003";
                objData["SNOMED_DESCRIPTION"] = "Acute pharyngitis";
            }

            else if (Clinical_HPIComplaints.LastSctBaseSearch == "53741008" && objData["SNOMEDID"] != "53741008") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "53741008")
                }
                objData["SNOMEDID"] = "53741008";
                objData["SNOMED_DESCRIPTION"] = "Coronary arteriosclerosis";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "266569009" && objData["SNOMEDID"] != "266569009") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "266569009")
                }
                objData["SNOMEDID"] = "266569009";
                objData["SNOMED_DESCRIPTION"] = "Benign prostatic hyperplasia";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "49436004" && objData["SNOMEDID"] != "49436004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "49436004")
                }
                objData["SNOMEDID"] = "49436004";
                objData["SNOMED_DESCRIPTION"] = "Atrial fibrillation";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "230572002" && objData["SNOMEDID"] != "230572002") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "230572002")
                }
                objData["SNOMEDID"] = "230572002";
                objData["SNOMED_DESCRIPTION"] = "Diabetic neuropathy";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "5281000124103" && objData["SNOMEDID"] != "5281000124103") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "5281000124103")
                }
                objData["SNOMEDID"] = "5281000124103";
                objData["SNOMED_DESCRIPTION"] = "Persistent asthma";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "981000124106" && objData["SNOMEDID"] != "981000124106") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "981000124106")
                }
                objData["SNOMEDID"] = "981000124106";
                objData["SNOMED_DESCRIPTION"] = "Moderate left ventricular systolic dysfunction";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "426749004" && objData["SNOMEDID"] != "426749004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "426749004")
                }
                objData["SNOMEDID"] = "426749004";
                objData["SNOMED_DESCRIPTION"] = "Chronic atrial fibrillation";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "991000124109" && objData["SNOMEDID"] != "991000124109") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "991000124109")
                }
                objData["SNOMEDID"] = "991000124109";
                objData["SNOMED_DESCRIPTION"] = "Severe left ventricular systolic dysfunction";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "64109004" && objData["SNOMEDID"] != "64109004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "64109004")
                }
                objData["SNOMEDID"] = "64109004";
                objData["SNOMED_DESCRIPTION"] = "Costal Chondritis";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "195967001" && objData["SNOMEDID"] != "195967001") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "195967001")
                }
                objData["SNOMEDID"] = "195967001";
                objData["SNOMED_DESCRIPTION"] = "Asthma";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "53741008" && objData["SNOMEDID"] != "53741008") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "53741008")
                }
                objData["SNOMEDID"] = "53741008";
                objData["SNOMED_DESCRIPTION"] = "Coronary arteriosclerosis";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "266569009" && objData["SNOMEDID"] != "266569009") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "266569009")
                }
                objData["SNOMEDID"] = "266569009";
                objData["SNOMED_DESCRIPTION"] = "Benign prostatic hyperplasia";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "49436004" && objData["SNOMEDID"] != "49436004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "49436004")
                }
                objData["SNOMEDID"] = "49436004";
                objData["SNOMED_DESCRIPTION"] = "Atrial fibrillation";
            }
            else if (Clinical_HPIComplaints.LastSctBaseSearch == "230572002" && objData["SNOMEDID"] != "230572002") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "230572002")
                }
                objData["SNOMEDID"] = "230572002";
                objData["SNOMED_DESCRIPTION"] = "Diabetic neuropathy";
            }


        }
        //End for wrong snomed code

        objData["Description"] = objData["ICD9"] + ' - ' + objData["ICD10"] + ' - ' + objData["SNOMEDID"] + ' - ' + objData["ICD10_Description"];
        objData["ProblemName"] = objData["ICD10_Description"];
        //--------------------------
        objData["commandType"] = "SAVE_PROBLEMLIST";

        if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote") {
            objData["NoteId"] = Clinical_HPIComplaints.params.NotesId;
        }

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },
    //this function will get Problem Lists Soap Text and attach that to Progress note
    getProblemListsInfo: function (ProblemListsId, hideAlertMessage, ComplaintDetailId) {
        if (ProblemListsId == null || ProblemListsId == '') {
            return false;
        }
        var dfd = new $.Deferred();
        Clinical_ProblemLists.get_ProblemLists_ForSOAP(ProblemListsId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.ProblemListSoapCount > 0) {
                        Clinical_HPIComplaints.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', ProblemListsId, hideAlertMessage, ComplaintDetailId);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    //This Function is used to create SOAP html and append it to  Progress note
    createProblemListBodyHTML: function (response, NoteHTMLCtrl, ProblemListsId, hideAlertMessage, ComplaintDetailId) {
        Clinical_ProblemLists.checkProblemListExists();
        var ProblemListSoap_JSON = JSON.parse(response.ProblemListSoap_JSON);
        var $mainDivVital = $(document.createElement('div'));
        if (ProblemListSoap_JSON == null || ProblemListSoap_JSON.length == 0) {
            Clinical_ProgressNote.saveComponentSOAPText('Problems', hideAlertMessage);
            return "";
        }
        var PListId = [];
        if (response.ProblemListSoapCount > 0) {
            $.each(ProblemListSoap_JSON, function (index, element) {
                var color = "";
                var $infoButtonrow = "";
                if (element.Description != "") {
                    var searchstr = element.Description.split('-')[0].trim();
                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(searchstr, "clinicalTabProgressNote", 2, "", "Clinical_ProblemLists");
                }
                // Start 26/11/2015 Muhammad Irfan Bug # EMR-80
                if (element.Severity == "Mild Intermittent" || element.Severity == "Mild Persistent") {
                    color = 'style = "color:green;font-weight:bold" ';
                }
                if (element.Severity == "Severe Persistent" || element.Severity == "Unspecified Severity") {
                    color = 'style = "color:red;font-weight:bold" ';
                }
                if (element.Severity == "Moderate Persistent") {
                    color = 'style = "color:orange;font-weight:bold" ';
                }
                var PLid = element.ProblemListId;
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Problems_Main" + PLid).attr('problemOrder', element.ProblemOrder);
                $SectionBodyVital.attr('complaintId', Clinical_HPIComplaints.ComplaintId);
                $SectionBodyVital.attr('complaintDetailId', ComplaintDetailId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_ProblemList_" + PLid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ProblemList_" + PLid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Problems_Main" + PLid + '"  ><i class="fa fa-times"></i></a><a href="javascript:void(0);" class="btn-xs on-default up-row" title="Up Record" onclick="Clinical_ProgressNote.problemUp(\'' + PLid + '\')"><i class="fa fa-arrow-up black"></i></a>'
                    + '<a href="javascript:void(0);" class="btn-xs on-default down-row" title="Down Record" onclick="Clinical_ProgressNote.problemDown(\'' + PLid + '\')"><i class="fa fa-arrow-down black"></i></a></div> ');

                var StartDateEndDate = "";
                if (element.StartDate != '' && element.StartDate != null && element.EndDate != '' && element.EndDate != null && utility.RemoveTimeFromDate(null, element.StartDate) != utility.RemoveTimeFromDate(null, element.EndDate)) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " Started on " + utility.RemoveTimeFromDate(null, element.StartDate)) + (element.EndDate == '' ? "" : " and ended on " + utility.RemoveTimeFromDate(null, element.EndDate) + ".");
                }
                else if ((element.StartDate == '' || element.StartDate == null) && (element.EndDate != '' && element.EndDate != null)) {
                    StartDateEndDate = (element.EndDate == '' ? "" : " ended on " + utility.RemoveTimeFromDate(null, element.EndDate) + ".");
                }
                else if ((element.EndDate == '' || element.EndDate == null) && (element.StartDate != '' && element.StartDate != null)) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " Started on " + utility.RemoveTimeFromDate(null, element.StartDate) + ".");
                }
                else if (element.EndDate == '' || element.EndDate == null && (element.StartDate == '' || element.StartDate == null)) {
                    StartDateEndDate = "";
                }
                else if (element.StartDate == element.EndDate) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " on " + utility.RemoveTimeFromDate(null, element.StartDate) + ".");
                }
                var inActiveText = "";
                if (element.IsActive != null && element.IsActive == "False") {
                    inActiveText = " : <span style = 'color:red;font-weight:bold'> (Inactive) </span>";
                }
                var CodeDescription = element.Description == '' ? "" : element.Description;
                $ListVital.append("<li><strong> " + CodeDescription + inActiveText + " " + $infoButtonrow + " </strong>" + (element.ChronicityLevel == '' ? "" : " Chronicity: " + element.ChronicityLevel) + ((element.Severity && element.ChronicityLevel) ? ", " : "") +
                    (element.Severity == '' ? "" : "<span " + color + '>Severity: ' + element.Severity + "</span> ") +
                    StartDateEndDate +
                    //------------------------------------
                    /*(element.StartDate == '' ? "" : ", Start on " + utility.RemoveTimeFromDate(null, element.StartDate)) +
                    (element.EndDate == '' ? "" : ", End on " + utility.RemoveTimeFromDate(null, element.EndDate)) +*/
                    //-----------------------------------
                    (element.ModifiedOn == '' ? "" : " Modified on " + utility.RemoveTimeFromDate(null, element.ModifiedOn) + ".")
                    );

                $ListVital.append(element.Comments == "" ? "" : "<li>Comments: " + element.Comments);
                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);
                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_ProblemList').parent().parent().find('#Cli_Problems_Main' + PLid).length == 0) {
                if ($(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid).length == 0) {
                    PListId.push(PLid);
                    $mainDivVital.append($SectionBodyVital);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid).html($SectionBodyVital.html());
                    $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul').append(CommentHTML);;
                }

            });
            //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
            if (PListId.join(",") != "") {
                ProblemListsId = PListId.join(",");
            }
            Clinical_ProgressNote.sort_problemsDesc();
            Clinical_ProgressNote.showHideUpDownProb();
            //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
            if ($mainDivVital.html() != '') {
                Clinical_ProblemLists.updateProblemListHtml($mainDivVital.html(), ProblemListsId, NoteHTMLCtrl, hideAlertMessage);
            } else {
                Clinical_ProblemLists.updateProblemListHtml('', ProblemListsId, NoteHTMLCtrl, hideAlertMessage);
                Clinical_ProgressNote.saveComponentSOAPText('Problems', hideAlertMessage);
            }
        } else {

            Clinical_ProgressNote.saveComponentSOAPText('Problems', hideAlertMessage);
        }
    },



    changeICDField: function () {

        if ($('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #radioFreetext").prop("checked") == true) {
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtComplaints").addClass('hidden');
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #btnSearchCPT").addClass('hidden');
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtFreeText").removeClass('hidden');

        } else {
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtComplaints").removeClass('hidden');
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #btnSearchCPT").removeClass('hidden');
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtFreeText").addClass('hidden');
        }

        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease").removeClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulHPITemplates").addClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtHPITemplate").addClass('hidden').parent("span").addClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #System").addClass('disableAll');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #SymptomFindings").addClass('disableAll');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #sectionFindingDetails").addClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #sectionComplaintDetails").removeClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #commentsDiv').removeClass('disableAll');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #findingContent").addClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #txtComment").removeClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #txtTemplateComments").addClass('hidden');
        Clinical_HPIComplaints.SaveFreeTextStatus();
    },
    createFreeTextLi: function () {
        var currId = -1;
        var diseaseText = $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtFreeText").val().trim();

        if (diseaseText != null && diseaseText != '') {
            $("#pnlClinicalHPIComplaints #frmClinicalComplaints ul#ulCompliantDisease li[id*='-']").each(function (i, item) {
                currId = $(this).attr("id");
            });
            currId = parseInt(currId) + (-1);

            var IsAlreadyExist = false;
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li").each(function () {
                if ($(this).text().trim() == diseaseText.trim()) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                var li = "<li id='" + currId + "' freetext='" + diseaseText + "' onclick='Clinical_HPIComplaints.fillChiefComplaints(this, event);'>" +
                    "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + diseaseText + "</span></a>" +
                        "<span class='removeIconListHover'>" +
                            "<a href='#' onclick='Clinical_HPIComplaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                "<a href='#' onclick='Clinical_HPIComplaints.deleteChiefComplaint(" + currId + ", event);'><i class='fa fa-times red'></i></a>" +
		                "</span>" +
                    "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_HPIComplaints.updateComplaintsText(this, event);'/>" +
                    "<div class='clearfix'></div>" +
                "</li>";
                $('#pnlClinicalHPIComplaints #ulCompliantDisease').append(li);
                Clinical_HPIComplaints.AddInArray(currId, diseaseText, true);
                $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtFreeText").val('');
                Clinical_HPIComplaints.IsUpdate = true;
            }
            else {
                utility.DisplayMessages('Disease already added', 2);
            }

        }
        else {
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtFreeText").val("");
            utility.DisplayMessages('Please enter valid text.', 2);
        }
    },
    // Start 4/1/2016 Muhammad Ahmad Imran
    //Purpose Save/update favList Status
    SaveFreeTextStatus: function () {
        if (Clinical_HPIComplaints.params.ParentCtrl == "clinicalTabProgressNote") {
            panel = "#pnlClinicalProgressNote #pnlClinicalHPIComplaints";
        }
        else {
            panel = "#pnlClinicalHPIComplaints";
        }
        var IsFreeText = false;
        $('input[name=radioFreetext]:checked', panel).val() == "freetext" ? IsFreeText = true : IsFreeText = false;
        EMRUtility.insertUpdateFreeTextStatus("Clinical_HPIComplaints", IsFreeText);
    },

    loadNoteComponent: function () {
        UnloadActionPan(Clinical_HPIComplaints.params.ParentCtrl, 'Clinical_HPIComplaints');
        if (Clinical_HPIComplaints.controlToInvoke && Clinical_HPIComplaints.controlToInvoke != false) {
            setTimeout(function () {
                Clinical_ProgressNote.SelectNotesComponentTab(Clinical_HPIComplaints.controlToInvoke.replace(/\s/g, ''));
                Clinical_HPIComplaints.controlToInvoke = null;
            }, 600);
        }
    },

    editComplaintsText: function (obj, event) {
        if (event) {
            event.stopPropagation();
        }
        Clinical_HPIComplaints.IsUpdate = true;
        var li = $(obj).closest('li');
        var desc = li.find(".complaint-text").text();
        li.find(".complaint-text").addClass("hidden");
        li.find(".removeIconListHover").addClass("hidden");
        li.find(".edit-complaint").removeClass("hidden").val(desc).focus();
        $('#' + Clinical_HPIComplaints.params.PanelID + " #btnClose").addClass('disableAll');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #btnAdd").addClass('disableAll');
    },

    updateComplaintsText: function (obj, event) {
        if (event) {
            event.stopPropagation();
        }
        var li = $(obj).closest('li');
        var ComplainDetailId = li.attr('id');
        if ($(obj).val().trim() == "") {

            utility.myConfirm('1', function () {
                Clinical_HPIComplaints.deleteChiefComplaintConfirmed(ComplainDetailId, event);
            }, function () {
                $(obj).addClass("hidden").closest('li').find(".complaint-text").removeClass("hidden");
                li.find(".removeIconListHover").removeClass("hidden");
            });

            $('#' + Clinical_HPIComplaints.params.PanelID + " #btnClose").removeClass('disableAll');
            $('#' + Clinical_HPIComplaints.params.PanelID + " #btnAdd").removeClass('disableAll');
        } else {
            var IsAlreadyExist = false;
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li").each(function () {
                if (ComplainDetailId != $(this).attr("id") && $(this).text().trim() == $(obj).val().trim()) {
                    IsAlreadyExist = true;
                }
            });
            if (!IsAlreadyExist) {
                $(obj).addClass("hidden").closest('li').find(".complaint-text").removeClass("hidden").text($(obj).val());
                li.find(".removeIconListHover").removeClass("hidden");
                $.grep(Clinical_HPIComplaints.Complaints, function (item, index) {
                    if (item.ComplaintDetailId == ComplainDetailId) {
                        item.ComplaintDescription = $(obj).val();
                        return;
                    }
                });
                $.grep(Clinical_HPIComplaints.ComplaintsDetail, function (item, index) {
                    if (item.ComplaintDetailId == ComplainDetailId) {
                        item.ComplaintDescription = $(obj).val();
                        return;
                    }
                });
                $('#' + Clinical_HPIComplaints.params.PanelID + " #btnClose").removeClass('disableAll');
                $('#' + Clinical_HPIComplaints.params.PanelID + " #btnAdd").removeClass('disableAll');
            } else {
                utility.DisplayMessages('Disease already added', 2);
                li.find(".edit-complaint").focus();
            }
        }
    },

    changeHPITemplate: function () {
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtComplaints").addClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #btnSearchCPT").addClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtFreeText").addClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #divHPITemplates").removeClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease").addClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulHPITemplates").removeClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #txtHPITemplate").removeClass('hidden').parent("span").removeClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #sectionComplaintDetails").addClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #sectionFindingDetails").removeClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #commentsDiv').addClass('disableAll');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #findingContent").removeClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #txtComment").addClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #txtTemplateComments").removeClass('hidden');

        if ($('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulHPITemplates li.active").length > 0 || Clinical_HPIComplaints.params.mode == "Edit") {
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #System").removeClass('disableAll');
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #SymptomFindings").removeClass('disableAll');
        }
        else {
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #System").addClass('disableAll');
            $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #SymptomFindings").addClass('disableAll');
        }
        //if (true) {

        //}
        //Clinical_HPIComplaints.buildSymptomsAutoComplete();
        //Clinical_HPIComplaints.buildTemplateAutoComplete();
        //  Clinical_HPIComplaints.bindFindingsAutoComplete();
    },

    buildSymptomsAutoComplete: function (templateId) {
        var objDeffered = $.Deferred();
        Clinical_HPIComplaints.HPITemplateSymptomsLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var resSymptoms = response.HPISymptoms_JSON;
                var data = [];
                $.each(resSymptoms, function (i, item) {
                    data.push({ id: item.HPISymptomId, value: item.Name, type: item.HPISymptomsAnswersId });
                });

                $("#Symptoms").kendoAutoComplete({
                    dataSource: data,
                    filter: "contains",
                    dataTextField: "value",
                    placeholder: "Select Symptom...",
                    select: function (e) {
                        var dataItem = this.dataItem(e.item.index());

                        var HPITemplateSymptomId = "-1";
                        if (dataItem.id)
                            if ($("#ulHPISymptoms").find('#' + dataItem.id).length == 0) {

                                Clinical_HPIComplaints.addSymptomTempAssosiation(Clinical_HPIComplaints.params["HPITemplateId"], dataItem.id, dataItem.value);
                            }
                            else {
                                utility.DisplayMessages(dataItem.value + " already exists.", 3);
                            }
                        $("#Systems").val('');

                        e.preventDefault();
                    },
                    footerTemplate: 'Total <strong>#: instance.dataSource.total() #</strong> items found'
                });

                $("#Systems").parent().addClass('size100per');
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            objDeffered.resolve();
        });
        return objDeffered.promise();
    },

    HPITemplateSymptomsLoad: function (templateId) {
        var objData = new Object();
        objData["HPITemplateId"] = templateId;
        objData["commandType"] = "Load_HPI_Symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    addSymptomTempAssosiation: function (HPITemplateId, HPISymptomId, Name) {
        var objData = new Object();
        objData["HPISymptomId"] = HPISymptomId;
        objData["HPITemplateId"] = HPITemplateId;
        objData["IsActive"] = true;
        objData["IsSelected"] = false;

        var symptomOrder = 1;
        if ($("#ulHPISymptoms tr").length > 1) {
            var lastOrder = $("#ulHPISymptoms tr").last().attr('symptomOrder');
            if (lastOrder != null) {
                symptomOrder = parseInt(lastOrder) + 1;
            }
            else {
                symptomOrder = 1;
            }
        }

        objData["SymptomOrder"] = symptomOrder;
        if (!objData["HPISymptomId"] || !objData["HPISymptomId"]) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }
        Clinical_HPIComplaints.addSymptomTempAssosiation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                utility.DisplayMessages(response.Message, 1);

                //var resSystems = JSON.parse(response.HPITemplateSystem_JSON);
                //resItem = resSystems[0];
                if (response.HPITemplateSymptomId) {
                    $("#Symptoms").val('');
                    var tr = Clinical_HPIComplaints.addSymptom(HPISymptomId, Name, Clinical_HPIComplaints.params["HPITemplateId"], response.HPITemplateSymptomId, "", symptomOrder);
                    $("#ulHPISymptoms tbody").append(tr);

                    var objSelectedSymptom = {
                        HPISymptomId: HPISymptomId, IsSelected: true, HPITemplateId: HPITemplateId, SymptomName: Name,
                        HPITemplateSymptomId: response.HPITemplateSymptomId,
                        SymptomDescription: '', HPINotesFindingsId: -1, HPISymptomsAnswersId: null,
                        IsCurrent: true, SymptomOrder: symptomOrder
                    };

                    Clinical_HPIComplaints.selectedSymptoms.push(objSelectedSymptom);
                }
            }
        });
    },

    addSymptomTempAssosiation_DBCall: function (data) {
        var objData = new Object();
        if (data)
            objData = data;
        objData["commandType"] = "Insert_Template_Symptom_Assosication";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    buildTemplateAutoComplete: function () {
        var PETemplateId = "";
        var objDeffered = $.Deferred();
        Clinical_HPIComplaints.HPIActiveTemplatesLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.HPITemplateCount > 0) {
                    var resTemplate = response.HPITemplate_JSON
                    var data = [];
                    $("#ulHPITemplates li").remove();
                    $.each(resTemplate, function (i, item) {
                        data.push({ id: item.HPITemplateId, value: item.Name });
                        var li = Clinical_HPIComplaints.addTemplate(item.HPITemplateId, item.Name);
                        $("#ulHPITemplates").append(li);
                        if (Clinical_HPIComplaints.params.HPITemplateId)
                            $("#ulHPITemplates #HPITemplate_" + Clinical_HPIComplaints.params.HPITemplateId).addClass('text-success');
                    });

                    $("#txtHPITemplate").kendoAutoComplete({
                        dataSource: data,
                        filter: "contains",
                        dataTextField: "value",
                        placeholder: "Select Template...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulHPITemplates").find('#HPITemplate_' + dataItem.id).length == 0) {
                                var li = Clinical_HPIComplaints.addTemplate(dataItem.id, dataItem.value);
                                $("#ulHPITemplates").append(li);
                            }
                            else {
                                utility.DisplayMessages(dataItem.value + " already exists.", 3);
                            }
                            $("#Exams").val('');
                            e.preventDefault();
                        },
                        footerTemplate: 'Total <strong>#: instance.dataSource.total() #</strong> items found'
                    });
                    $("#Exams").parent().addClass('size100per');
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            objDeffered.resolve();
        });
        return objDeffered.promise();
    },
    HPIActiveTemplatesLoad: function (templateId) {
        var objData = new Object();
        objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId
        objData["commandType"] = "load_hpitemplate_forprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    addTemplate: function (HPITemplateId, TemplateName) {
        var itemtoRemove = "Template";
        var li = '<li id="HPITemplate_' + HPITemplateId + '" parentid="null" onclick="Clinical_HPIComplaints.loadSymptoms(' + HPITemplateId + ',this)" value="' + HPITemplateId + '" refvalue="" subcharacteristicexist=" "><div class="">' +
                 '<label id="lblTemplateName_' + HPITemplateId + '" class="" data-toggle="tooltip" title="" data-original-title="' + TemplateName + '">' + TemplateName + '</label><div id="divNameDetail' + HPITemplateId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea spellcheck="true" rows="1" id="txtName' + HPITemplateId + '" onkeypress="" name="Name' + HPITemplateId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 HPITemplateId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';
        return li;
    },

    loadSymptoms: function (HPITemplateId, obj) {
        var HPISymptomId = Clinical_HPIComplaints.getCurrentSymptomId();
        var HPITemplateSymptomId = Clinical_HPIComplaints.getCurrentTemplateSymptomId();
        if (HPITemplateSymptomId)
            Clinical_HPIComplaints.FillFindingContent(HPITemplateSymptomId);
        Clinical_HPIComplaints.params.HPITemplateId = HPITemplateId;
        Clinical_HPIComplaints.clearTemplateSelection();
        var isExist = false;
        var isSelected = false;
        $("#ulHPITemplates").find('#HPITemplate_' + HPITemplateId).addClass('text-success');
        $("#ulHPITemplates li").removeClass('active');
        $("#ulHPITemplates").find('#HPITemplate_' + HPITemplateId).addClass('active');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #System").removeClass('disableAll');
        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #SymptomFindings").removeClass('disableAll');
        //if ($("#ulSelectedHPITemplates").find('#listHPITemplate_' + HPITemplateId).length == 0) {
        //    var TemplateName = $(obj).find('#lblTemplateName_' + HPITemplateId).text();
        //    Clinical_HPIComplaints.ScrollTabObj.addTab('<li id="listPETemplate_' + HPITemplateId + '" onclick="Clinical_HPIComplaints.loadSystems(' + HPITemplateId + ',this)" class="active"><a id="lnkPETemplate_' + HPITemplateId + '" class="" href="#" data-toggle="tab">' + TemplateName + '</a></li>');
        //}
        if (Clinical_HPIComplaints.selectedSymptoms) {
            for (var i = 0 ; i < Clinical_HPIComplaints.selectedSymptoms.length; i++) {

                if (Clinical_HPIComplaints.selectedSymptoms[i].HPITemplateId == HPITemplateId) {
                    isExist = true;
                    break;
                }
                Clinical_HPIComplaints.selectedSymptoms[i].IsCurrent = false;
            }
        }
        $("#Symptom").removeClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptoms tr:gt(0)').remove();
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptoms tbody').append('<tr><td class="size20">C/O</td><td class="size20">Denies</td><td></td></tr>');
        //$('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptoms tbody').append('<tr><td class="text-center"><input type="radio" symptomtype="1" id="" name="SelectAllSymptoms" onclick="Clinical_HPIComplaints.SelectAllSymptoms(1);"></td>' +
        //    '<td class="text-center"><input type="radio" symptomtype="2" id="" name="SelectAllSymptoms" onclick="Clinical_HPIComplaints.SelectAllSymptoms(2);"></td>' +
        //    '<td><label id="" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label></td></tr>');

        if (!isExist) {
            Clinical_HPIComplaints.Clinical_HPITempalteSymptomsLoad(HPITemplateId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.HPITemplateSymptomsCount > 0) {
                        var resSystems = response.HPITemplateSymptoms_JSON;
                        var promises = [];

                        if (resSystems.length > 0)
                            $("textarea#txtTemplateComments").val(resSystems[0].Comments);


                        $.each(resSystems, function (i, item) {
                            var tr = Clinical_HPIComplaints.addSymptom(item.HPISymptomId, item.SymptomName, HPITemplateId, item.HPITemplateSymptomId, item.HPISymptomsAnswersId, item.SymptomOrder);
                            $("#ulHPISymptoms").removeClass('hidden');
                            $("#ulHPISymptoms tbody").append(tr);
                            var def = Clinical_HPIComplaints.loadFindings(item.HPISymptomId, $("#ulHPISymptoms #" + item.HPISymptomId), HPITemplateId);
                            isSelected = item.IsSelected == "True" ? true : false
                            var objSelectedSymptom = {
                                HPISymptomId: item.HPISymptomId, IsSelected: item.IsSelected == "True" ? true : false, HPITemplateId: HPITemplateId, SymptomName: item.SymptomName, HPITemplateSymptomId: item.HPITemplateSymptomId,
                                SymptomDescription: item.SymptomDescription, HPINotesFindingsId: -1, HPISymptomsAnswersId: item.HPISymptomsAnswersId,
                                IsCurrent: i == 0 ? true : false, Comments: $("textarea#txtTemplateComments").val(), SymptomOrder: item.SymptomOrder
                            };
                            if (isSelected) {
                                $("#ulHPISymptoms tr[id='" + item.HPISymptomId + "']").addClass('text-success');
                            }
                            else
                                $("#ulHPISymptoms tr[id='" + item.HPISymptomId + "']").removeClass('text-success');

                            //    $("#ulHPISymptoms tr #chk" + item.HPITemplateSymptomId + "").prop("checked", isSelected);

                            Clinical_HPIComplaints.selectedSymptoms.push(objSelectedSymptom);
                            promises.push(def);
                        });
                        $.when.apply(undefined, promises).done(function () {
                            Clinical_HPIComplaints.selectFirstSymptom();
                        });
                    }
                }
            });
        }
        else {
            if (Clinical_HPIComplaints.selectedSymptoms) {
                var promises = [];

                for (var i = 0 ; i < Clinical_HPIComplaints.selectedSymptoms.length; i++) {
                    if (Clinical_HPIComplaints.selectedSymptoms[i].HPITemplateId == HPITemplateId) {
                        $("textarea#txtTemplateComments").val(Clinical_HPIComplaints.selectedSymptoms[i].Comments);
                        var tr = Clinical_HPIComplaints.addSymptom(Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId, Clinical_HPIComplaints.selectedSymptoms[i].SymptomName, HPITemplateId, Clinical_HPIComplaints.selectedSymptoms[i].HPITemplateSymptomId, Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomsAnswersId, Clinical_HPIComplaints.selectedSymptoms[i].SymptomOrder);
                        if (Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId != "" && Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId != -1)
                            $("#ulHPISymptoms").removeClass('hidden');
                        $("#ulHPISymptoms tbody").append(tr);

                        // var def = Clinical_HPIComplaints.loadFindings(item.HPISymptomId, $("#ulHPISymptoms #" + item.HPISymptomId), HPITemplateId);

                        var isSelected = Clinical_HPIComplaints.selectedSymptoms[i].IsSelected;
                        if (isSelected) {
                            $("#ulHPISymptoms tr[id='" + Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId + "']").addClass('text-success');
                        }
                        else
                            $("#ulHPISymptoms tr[id='" + Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId + "']").removeClass('text-success');

                        var def = Clinical_HPIComplaints.loadFindings(Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId, $("#ulHPISymptoms #" + Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId, Clinical_HPIComplaints.selectedSymptoms[i].HPITemplateId));

                        //  $("#ulHPISymptoms li #chk" + Clinical_HPIComplaints.selectedSymptoms[i].HPITemplateSymptomId + "").prop("checked", isSelected);
                        //   var def = Clinical_HPIComplaints.loadObservations(Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId, $("#ulPhysicalExamSystems #" + Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId, Clinical_HPIComplaints.selectedSymptoms[i].HPITemplateId));
                        promises.push(def);
                    }
                    else {
                        var def = $.Deferred();
                        def.resolve();
                        promises.push(def);
                    }
                }
                $.when.apply(undefined, promises).done(function () {
                    Clinical_HPIComplaints.selectFirstSymptom();
                });
            }
        }
    },

    SelectAllSymptoms: function (chkBox) {

        if (chkBox == "1") {
            $("#" + Clinical_HPIComplaints.params.PanelID + " #ulHPISymptoms tbody [symptomtype='1']").not(':checked').each(function (index, item) {
                $(this).prop("checked", true);
                $(this).trigger('click');
            });

            //  $("#" + Clinical_HPIComplaints.params.PanelID + " #ulHPISymptoms tbody [symptomtype='1']").prop("checked", true);
        }
        else {

            $("#" + Clinical_HPIComplaints.params.PanelID + " #ulHPISymptoms tbody [symptomtype='2']").not(':checked').each(function (index, item) {
                $(this).prop("checked", true);
                $(this).trigger('click');
            });

            //  $("#" + Clinical_HPIComplaints.params.PanelID + " #ulHPISymptoms tbody [symptomtype='2']").prop("checked", true);
        }
    },

    Clinical_HPITempalteSymptomsLoad: function (templateId) {
        var objData = new Object();
        objData["HPITemplateId"] = templateId == undefined ? "0" : templateId;
        objData["commandType"] = "Load_HPI_Template_Symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    addSymptom: function (HPISymptomsId, SymptomName, HPITemplateId, HPITemplateSymptomId, symptomType, symptomOrder) {
        var itemtoRemove = "symptom";
        var tr = '';
        if (symptomType == 1) {
            tr = '<tr symptomorder="' + symptomOrder + '" id="' + HPISymptomsId + '" HPITemplateSymptomId="' + HPITemplateSymptomId + '" parentid="' + HPITemplateId + '" onclick="Clinical_HPIComplaints.loadFindings(' + HPISymptomsId + ')" value="' + HPISymptomsId + '" refvalue="" subcharacteristicexist=" "><td class="text-center">' +
                '<input type="radio" checked="checked" symptomtype="1" id="chk' + HPITemplateSymptomId + '" name="SymptomsType' + HPITemplateSymptomId + '" onclick="Clinical_HPIComplaints.ManageFindings(' + HPISymptomsId + ', this,' + HPITemplateSymptomId + ',event);"></td><td class="text-center"><input type="radio" symptomtype="2" id="chk' + HPITemplateSymptomId + '" name="SymptomsType' + HPITemplateSymptomId + '" onclick="Clinical_HPIComplaints.ManageFindings(' + HPISymptomsId + ', this,' + HPITemplateSymptomId + ',event);"></td><td><label id="lblName' + HPISymptomsId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SymptomName + '">' + SymptomName + '</label></td></tr>';
        }
        else if (symptomType == 2) {
            tr = '<tr symptomorder="' + symptomOrder + '" id="' + HPISymptomsId + '" HPITemplateSymptomId="' + HPITemplateSymptomId + '" parentid="' + HPITemplateId + '" onclick="Clinical_HPIComplaints.loadFindings(' + HPISymptomsId + ')" value="' + HPISymptomsId + '" refvalue="" subcharacteristicexist=" "><td class="text-center">' +
               '<input type="radio" symptomtype="1" id="chk' + HPITemplateSymptomId + '" name="SymptomsType' + HPITemplateSymptomId + '" onclick="Clinical_HPIComplaints.ManageFindings(' + HPISymptomsId + ', this,' + HPITemplateSymptomId + ',event);"></td><td class="text-center"><input type="radio" checked="checked" symptomtype="2" id="chk' + HPITemplateSymptomId + '" name="SymptomsType' + HPITemplateSymptomId + '" onclick="Clinical_HPIComplaints.ManageFindings(' + HPISymptomsId + ', this,' + HPITemplateSymptomId + ',event);"></td><td><label id="lblName' + HPISymptomsId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SymptomName + '">' + SymptomName + '</label></td></tr>';
        }
        else {
            tr = '<tr symptomorder="' + symptomOrder + '" id="' + HPISymptomsId + '" HPITemplateSymptomId="' + HPITemplateSymptomId + '" parentid="' + HPITemplateId + '" onclick="Clinical_HPIComplaints.loadFindings(' + HPISymptomsId + ')" value="' + HPISymptomsId + '" refvalue="" subcharacteristicexist=" "><td class="text-center">' +
                    '<input type="radio" symptomtype="1" id="chk' + HPITemplateSymptomId + '" name="SymptomsType' + HPITemplateSymptomId + '" onclick="Clinical_HPIComplaints.ManageFindings(' + HPISymptomsId + ', this,' + HPITemplateSymptomId + ',event);"></td><td class="text-center"><input type="radio" symptomtype="2" id="chk' + HPITemplateSymptomId + '" name="SymptomsType' + HPITemplateSymptomId + '" onclick="Clinical_HPIComplaints.ManageFindings(' + HPISymptomsId + ', this,' + HPITemplateSymptomId + ',event);"></td><td><label id="lblName' + HPISymptomsId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SymptomName + '">' + SymptomName + '</label></td></tr>';
        }
        return tr;
    },

    getCurrentSymptomId: function () {
        var HPISymptomId = "";
        $(Clinical_HPIComplaints.selectedSymptoms).each(function (ind, i) {
            if (i.IsCurrent == true && i.HPITemplateId == Clinical_HPIComplaints.params.HPITemplateId) {
                HPISymptomId = i.HPISymptomId;
            }
        });
        return HPISymptomId;
    },

    getCurrentTemplateSymptomId: function () {
        var HPITemplateSymptomId = "";
        $(Clinical_HPIComplaints.selectedSymptoms).each(function (ind, i) {
            if (i.IsCurrent == true && i.HPITemplateId == Clinical_HPIComplaints.params.HPITemplateId) {
                HPITemplateSymptomId = i.HPITemplateSymptomId;
            }
        });
        return HPITemplateSymptomId;
    },
    FillFindingContent: function (HPITemplateSymptomId) {
        $(Clinical_HPIComplaints.selectedSymptoms).each(function (ind, i) {
            i.IsCurrent = false;
        });
        $(Clinical_HPIComplaints.selectedSymptoms).each(function (ind, i) {
            if (i.HPITemplateSymptomId == HPITemplateSymptomId) {
                // Patch to Handle Sympots Without Findings
                if (i.SymptomDescription == "" || i.SymptomDescription == null) {
                    if (i.HPISymptomsAnswersId == "1") {
                        var tpAppned = '<div id="divSymptom' + i.HPISymptomId + '">C/O ' + i.SymptomName + '</div>'; //"C/O" + " " + i.SymptomName;                       
                        $("#findingContent").html(tpAppned);
                    }
                        //Start 29-09-2017 Edit By Humaira Yousaf Bug# EMR-4851
                    else if (i.HPISymptomsAnswersId == "2") {
                        var tpAppned = '<div id="divSymptom' + i.HPISymptomId + '">Denies ' + i.SymptomName + '</div>'; //"Denies" + " " + i.SymptomName;                       
                        $("#findingContent").html(tpAppned);
                    }
                    else {
                        $("#findingContent").html("");
                    }
                    //End 29-09-2017 Edit By Humaira Yousaf Bug# EMR-4851
                    i.IsCurrent = true;
                }
                else {
                    $("#findingContent").html(i.SymptomDescription);
                    //Start 03-10-2017 Edit By Humaira Yousaf IMP-1172
                    var detailComments = $("#findingContent #divSymptom" + i.HPISymptomId + 'txtDetailComments');
                    if (detailComments.length > 0) {
                        $('#txtDetailComments').val($(detailComments).text().trim().slice(0, -1));
                    }
                    else {
                        $('#txtDetailComments').val('');
                    }
                    //End 03-10-2017 Edit By Humaira Yousaf IMP-1172
                    i.IsCurrent = true;
                }
            }
        });
    },
    clearTemplateSelection: function () {
        $("#ulHPISymptoms tr").remove();
        $("#ulHPISymptomFindings li").remove();
        $("#findingContent").html('');
    },

    selectFirstSymptom: function () {
        if ($("#ulHPISymptoms tr").length > 2)
            $($("#ulHPISymptoms tr")[2]).trigger('click');
    },

    loadFindings: function (HPISymptomId, obj, HPITemplateId) {



        if ($('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li').length > 0) {
            var prevTemplatesymptomid = $('#ulHPISymptoms tr.active').attr('hpitemplatesymptomid');
            var prevFindingId = $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li.active').attr('id');
            Clinical_HPIComplaints.addFindingsDetails(prevTemplatesymptomid, prevFindingId);
        }

        var objDeffered = $.Deferred();

        HPITemplateId = Clinical_HPIComplaints.params.HPITemplateId;//$('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPITemplates li.text-success').val();
        var $obj = $('#ulHPISymptoms tr[id="' + HPISymptomId + '"]');
        Clinical_HPIComplaints.selectedSymptomId = HPISymptomId;
        var HPITemplateSymptomId = $('#ulHPISymptoms tr#' + HPISymptomId).attr('hpitemplatesymptomid');
        Clinical_HPIComplaints.selectedTemplateSymptomId = HPITemplateSymptomId;
        $('#ulHPISymptoms li').removeClass('text-success');
        var $Sysli = $('#ulHPISymptoms tr[id="' + HPISymptomId + '"]');
        if ($Sysli) {
            var checked = $($Sysli).find('input[type="checkbox"]').is(':checked');
            if (checked)
                $("#ulHPISymptoms tr[id='" + HPISymptomId + "']").addClass('text-success');
        }
        $("#SymptomFindings").removeClass('hidden');
        var desr = "";
        var isSelected = false;
        var isExist = false;
        if (Clinical_HPIComplaints.selectedFindings) {
            for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                if (Clinical_HPIComplaints.selectedFindings[i].HPITemplateSymptomId == HPITemplateSymptomId) {
                    isExist = true;
                    break;
                }
            }
        }
        if (!isExist) {
            Clinical_HPIComplaints.HPISymptomFindingsLoad(HPISymptomId, HPITemplateId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (Clinical_HPIComplaints.selectedSymptoms) {
                        for (var j = 0; j < Clinical_HPIComplaints.selectedSymptoms.length; j++) {
                            if (Clinical_HPIComplaints.selectedSymptoms[j].HPITemplateSymptomId == HPITemplateSymptomId) {
                                //Clinical_HPIComplaints.selectedSymptoms[j].IsSelected = true;
                                var symtomType = Clinical_HPIComplaints.selectedSymptoms[j].HPISymptomsAnswersId;
                                if (symtomType == "1") {
                                    $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings').removeClass('disableAll');
                                    //$('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');
                                    $('#' + Clinical_HPIComplaints.params.PanelID + ' #hpiDetails').removeClass('disableAll');
                                }
                                else {
                                    $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings').addClass('disableAll');
                                    //$('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').addClass('disableAll');
                                    $('#' + Clinical_HPIComplaints.params.PanelID + ' #hpiDetails').addClass('disableAll');

                                }
                            }
                        }
                    }

                    $("#ulHPISymptomFindings li").remove();
                    $("#ulHPISymptoms tr").removeClass('active');
                    $("#ulHPISymptoms tr[id=" + HPISymptomId + "]").addClass('active');
                    if (response.HPIFindingCount > 0) {
                        var selectAll = '<li id="chkboxSelectAllFindings" parentid="' + HPISymptomId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllFindings" name="Select All" class="pull-left" '
                                                   + 'onclick="Clinical_HPIComplaints.selectAllFindings(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        $("#ulHPISymptomFindings").append(selectAll);

                        var resSystems = JSON.parse(response.HPIFinding_JSON);
                        var selectedTempSys = null;
                        $.each(resSystems, function (i, item) {
                            selectedTempSys = item.HPITemplateSymptomId;
                            if (item.HPIFindingId != "0") {
                                var li = Clinical_HPIComplaints.addFindings(item.HPIFindingId, item.FindingName, HPISymptomId, item.HPISymptomFindingsId, item.FindingOrder);
                                $("#ulHPISymptomFindings").append(li);
                            }
                            var isSelected = item.IsFindingSelected == "True" ? true : false;
                            var objselectedFindings = {
                                HPISymptomId: HPISymptomId, IsSelected: item.IsFindingSelected == "True" ? true : false,
                                FindingId: item.HPIFindingId, FindingName: item.FindingName, IsSymptomChecked: false, HPITemplateSymptomId:
                                    item.HPITemplateSymptomId, HPISymptomFindingsId: item.HPISymptomFindingsId, HPISymptomsAnswersId: null, IsSymptomsDetail: item.IsSymptomsDetail, HPISymptoms_SeverityId: item.HPISymptoms_SeverityId, HPISymptoms_LocationIds: item.HPISymptoms_LocationIds, HPISymptoms_RadiationId: item.HPISymptoms_RadiationId,
                                HPISymptoms_QualityId: item.HPISymptoms_QualityId, Onset: item.Onset, AssociatedWith: item.AssociatedWith, HPISymptoms_AggravatedById: item.HPISymptoms_AggravatedById, HPISymptoms_RelievedById: item.HPISymptoms_RelievedById, DetailComments: '', FindingOrder: item.FindingOrder
                            };

                            Clinical_HPIComplaints.selectedFindings.push(objselectedFindings);
                            if (isSelected)
                                $("#ulHPISymptomFindings li[id=" + item.HPIFindingId + "]").addClass('text-success');
                            else
                                $("#ulHPISymptomFindings li[id=" + item.HPIFindingId + "]").removeClass('text-success');
                            $("#ulHPISymptomFindings li #chk" + item.HPIFindingId + "").prop("checked", isSelected);

                        });
                        var desc = "";
                        var resNotesObs = JSON.parse(response.HPINotesFinding_JSON);
                        var selectedNotesObs = null;
                        $(resNotesObs).filter(function (ind, i) {
                            if (i.HPITemplateSymptomId == selectedTempSys) {
                                selectedNotesObs = i;
                            }
                        });
                        if (selectedNotesObs && selectedNotesObs.Desc) {
                            desc = selectedNotesObs.Desc;
                            $("#findingContent").html(selectedNotesObs.Desc);
                        }
                        else {
                            $("#findingContent").html(resSystems[0].TemplatePreview.trim());
                        }
                        $('#divFindingContents').html('');
                        $('#divFindingContents').append(resSystems[0].TemplatePreview.trim())
                        $.each(Clinical_HPIComplaints.selectedSymptoms, function (i, item) {
                            if (item.HPISymptomId == HPISymptomId && item.HPITemplateId == HPITemplateId && !selectedNotesObs) {
                                desc = Clinical_HPIComplaints.getSymptPreview($('#divFindingContents'), HPISymptomId);
                                item.SymptomDescription = desc;
                                item.TemplatePreview = resSystems[0].TemplatePreview.trim();
                            }
                            else if (item.HPISymptomId == HPISymptomId && item.HPITemplateId == HPITemplateId && selectedNotesObs) {
                                item.SymptomDescription = selectedNotesObs.Desc;
                            }
                        });

                        Clinical_HPIComplaints.FillFindingContent(HPITemplateSymptomId);
                        // $("#findingContent").html(desc);
                        objDeffered.resolve();
                    }
                    else {
                        Clinical_HPIComplaints.FillFindingContent(HPITemplateSymptomId);
                    }
                }

            });
        }
        else {
            $("#ulHPISymptomFindings li").remove();

            var selectAll = '<li id="chkboxSelectAllFindings" parentid="' + HPISymptomId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllFindings" name="Select All" class="pull-left" '
                                   + 'onclick="Clinical_HPIComplaints.selectAllFindings(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';


            if (Clinical_HPIComplaints.selectedSymptoms) {
                for (var j = 0; j < Clinical_HPIComplaints.selectedSymptoms.length; j++) {
                    if (Clinical_HPIComplaints.selectedSymptoms[j].HPITemplateSymptomId == HPITemplateSymptomId) {
                        var symtomType = Clinical_HPIComplaints.selectedSymptoms[j].HPISymptomsAnswersId;
                        //Clinical_HPIComplaints.selectedSymptoms[j].IsSelected = true;
                        if (symtomType == "1") {
                            $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings').removeClass('disableAll');
                            //$('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');
                            $('#' + Clinical_HPIComplaints.params.PanelID + ' #hpiDetails').removeClass('disableAll');

                        }
                        else {
                            $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings').addClass('disableAll');
                            //$('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').addClass('disableAll');
                            $('#' + Clinical_HPIComplaints.params.PanelID + ' #hpiDetails').addClass('disableAll');
                        }
                    }
                }
            }

            if (Clinical_HPIComplaints.selectedFindings) {
                for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                    if (Clinical_HPIComplaints.selectedFindings[i].HPITemplateSymptomId == HPITemplateSymptomId) {
                        if (Clinical_HPIComplaints.selectedFindings[i].IsSymptomsDetail == false || Clinical_HPIComplaints.selectedFindings[i].IsSymptomsDetail == null) {
                            $("#ulHPISymptoms tr").removeClass('active');
                            $("#ulHPISymptoms tr[id=" + HPISymptomId + "]").addClass('active');

                            var li = Clinical_HPIComplaints.addFindings(Clinical_HPIComplaints.selectedFindings[i].FindingId, Clinical_HPIComplaints.selectedFindings[i].FindingName, HPISymptomId, Clinical_HPIComplaints.selectedFindings[i].HPISymptomFindingsId, Clinical_HPIComplaints.selectedFindings[i].FindingOrder);

                            if (Clinical_HPIComplaints.selectedFindings[i].FindingId != "" && Clinical_HPIComplaints.selectedFindings[i].FindingId != -1 && Clinical_HPIComplaints.selectedFindings[i].FindingId != "0") {
                                if ($('#ulHPISymptomFindings li').find('#chkboxSelectAllFindings').length == 0) {
                                    $("#ulHPISymptomFindings").append(selectAll);
                                }
                                if ($("#ulHPISymptomFindings #" + Clinical_HPIComplaints.selectedFindings[i].FindingId).length == 0)
                                    $("#ulHPISymptomFindings").append(li);
                            }

                            var isSelected = Clinical_HPIComplaints.selectedFindings[i].IsSelected;
                            if (isSelected)
                                $("#ulHPISymptomFindings li[id=" + Clinical_HPIComplaints.selectedFindings[i].FindingId + "]").addClass('text-success');
                            else
                                $("#ulHPISymptomFindings li[id=" + Clinical_HPIComplaints.selectedFindings[i].FindingId + "]").removeClass('text-success');

                            $("#ulHPISymptomFindings li #chk" + Clinical_HPIComplaints.selectedFindings[i].FindingId + "").prop("checked", isSelected);
                        }
                        else {
                            Clinical_HPIComplaints.loadFindingsDetails(Clinical_HPIComplaints.selectedFindings[i].HPITemplateSymptomId, Clinical_HPIComplaints.selectedFindings[i].FindingId);
                        }
                    }
                }
            }

            Clinical_HPIComplaints.FillFindingContent(HPITemplateSymptomId);

            objDeffered.resolve();
        }
        return objDeffered;
    },

    HPISymptomFindingsLoad: function (HPISymptomId, HPITemplateId) {
        var objData = new Object();
        objData["HPISymptomId"] = HPISymptomId;
        objData["HPITemplateId"] = HPITemplateId;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "load_hpi_symptom_findings_note";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },


    addFindings: function (HPIFindingId, FindingName, HPISymptomId, HPISymptomFindingId, FindingOrder) {
        var a = Clinical_HPIComplaints.selectedFindings;

        var itemtoRemove = "finding";

        var li = '<li findingOrder="' + FindingOrder + '" id="' + HPIFindingId + '" parentid="' + HPISymptomId + '" onclick="Clinical_HPIComplaints.PreviewFindings(' + HPIFindingId + ',\'' + FindingName + '\', this, ' + HPISymptomId + ');" value="' + HPIFindingId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                 '<input type="checkbox" id="chk' + HPIFindingId + '" name="' + HPIFindingId + '" class="pull-left  char" ' +
                 'onclick="Clinical_HPIComplaints.PreviewFindings(' + HPIFindingId + ',\'' + FindingName + '\', this, ' + HPISymptomId + ');">' +
                 '<label id="lblName' + HPIFindingId + '" class="" data-toggle="tooltip" title="" data-original-title="' + FindingName + '">' + FindingName + '</label><div id="divNameDetail' + HPIFindingId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea spellcheck="true" rows="1" id="txtName' + HPIFindingId + '" onkeypress="" name="Name' + HPIFindingId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 HPIFindingId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="Clinical_HPIComplaints.removeFinding(' + HPISymptomFindingId + ', ' + HPIFindingId + ',event)"><i class="fa fa-close"></i></span></a></li>';
        return li;
    },

    resetForm: function () {
        // Empty global variables

        Clinical_HPIComplaints.selectedHPITempData = [];
        Clinical_HPIComplaints.selectedSymptoms = [];
        Clinical_HPIComplaints.selectedSymptomId = 0;
        Clinical_HPIComplaints.selectedTemplateSymptomId = 0;
        Clinical_HPIComplaints.selectedFindings = [];
    },


    addHPIComplaintToNote: function () {

        var objDeffered = $.Deferred();
        var prevFindingId = $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li.active').attr('id');
        var templatesymptomid = $("#ulHPISymptoms tr.active").attr('hpitemplatesymptomid');
        Clinical_HPIComplaints.addFindingsDetails(templatesymptomid, prevFindingId);

        var PESysId = Clinical_HPIComplaints.getCurrentSymptomId();
        var HPITemplateSymptomId = Clinical_HPIComplaints.getCurrentTemplateSymptomId();
        if (HPITemplateSymptomId)
            Clinical_HPIComplaints.FillFindingContent(HPITemplateSymptomId);
        var HPITemplateSymptoms = [];
        var selectedHPITempaltes = $("#ulHPITemplates li.text-success").map(function () {
            return this.value;
        }).get();

        for (var i = 0 ; i < Clinical_HPIComplaints.selectedSymptoms.length; i++) {
            var tpAppned = "";
            var templateId = Clinical_HPIComplaints.selectedSymptoms[i].HPITemplateId;
            if ($.inArray(parseInt(templateId), selectedHPITempaltes) >= 0 && (Clinical_HPIComplaints.selectedSymptoms[i].SymptomDescription == "" || Clinical_HPIComplaints.selectedSymptoms[i].SymptomDescription == null)) {
                if (Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomsAnswersId == "1") {
                    tpAppned = '<div id="divSymptom' + Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId + '">C/O ' + Clinical_HPIComplaints.selectedSymptoms[i].SymptomName + '</div>';
                }
                    //Start 29-09-2017 Edit By Humaira Yousaf Bug# EMR-4851
                else if (Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomsAnswersId == "2") {
                    tpAppned = '<div id="divSymptom' + Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId + '">Denies ' + Clinical_HPIComplaints.selectedSymptoms[i].SymptomName + '</div>';
                }
                    //End 29-09-2017 Edit By Humaira Yousaf Bug# EMR-4851
                else {
                    tpAppned = "";
                }
            }

            if (Clinical_HPIComplaints.selectedSymptoms[i].IsSelected == true) {
                var action = Clinical_HPIComplaints.selectedSymptoms[i].HPINotesFindingsId == "-1" ? "-1" : "0";//-1:for Insert,0 for update , 1=delete
                var objPETempSymFinding =
                {
                    HPINotesFindingsId: Clinical_HPIComplaints.selectedSymptoms[i].HPINotesFindingsId,
                    HPITemplateSymptomId: Clinical_HPIComplaints.selectedSymptoms[i].HPITemplateSymptomId,
                    NotesId: Clinical_ProgressNote.params.NotesId,
                    Desc: Clinical_HPIComplaints.selectedSymptoms[i].SymptomDescription == "" ? tpAppned : Clinical_HPIComplaints.selectedSymptoms[i].SymptomDescription,/*Clinical_HPIComplaints.getSymptPreview($(Clinical_HPIComplaints.selectedSymptoms[i].TemplatePreview), Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId),*/
                    Action: action,
                    HPISymptomsAnswersId: Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomsAnswersId,
                }
                HPITemplateSymptoms.push(objPETempSymFinding);
            }
            if (Clinical_HPIComplaints.selectedSymptoms[i].HPINotesFindingsId != "-1" && Clinical_HPIComplaints.selectedSymptoms[i].IsSelected == false) {
                var objPETempSymFinding = {
                    HPINotesFindingsId: Clinical_HPIComplaints.selectedSymptoms[i].HPINotesFindingsId,
                    HPITemplateSymptomId: Clinical_HPIComplaints.selectedSymptoms[i].HPITemplateSymptomId,
                    NotesId: Clinical_ProgressNote.params.NotesId,
                    Desc: Clinical_HPIComplaints.selectedSymptoms[i].SymptomDescription == "" ? tpAppned : Clinical_HPIComplaints.selectedSymptoms[i].SymptomDescription,/*Clinical_HPIComplaints.getSymptPreview($(Clinical_HPIComplaints.selectedSymptoms[i].TemplatePreview), Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId),*/
                    Action: "1",
                    HPISymptomsAnswersId: Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomsAnswersId,
                }
                HPITemplateSymptoms.push(objPETempSymFinding);
            }
        }
        var HPITempSymFindingIds = $(HPITemplateSymptoms).map(function () {
            return this.HPITempSymFindingId;
        }).get().join(',');
        if (HPITemplateSymptoms.length > 0) {
            Clinical_HPIComplaints.addHPIComplaintsToNotesDB_Call(HPITempSymFindingIds, HPITemplateSymptoms).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_HPIComplaints.Content_style = $("#" + Clinical_HPIComplaints.params.PanelID + " #findingContent").attr('style');
                    Clinical_HPIComplaints.createHPIComplaintBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null);

                    //Clinical_HPIComplaints.selectedPhyExamTempData = [];
                    //Clinical_HPIComplaints.ProviderIds = "";
                    //Clinical_HPIComplaints.selectedObservations = [];
                    //Clinical_HPIComplaints.selectedSymptoms = [];
                    //Clinical_HPIComplaints.ProviderIds = "";
                    //Clinical_HPIComplaints.normalSystemIdsGlobel = [];
                    //Clinical_HPIComplaints.NewInsertedId = -1;
                    //Clinical_HPIComplaints.selectedSymptoms = [];

                    Clinical_HPIComplaints.selectedHPITempData = [];
                    Clinical_HPIComplaints.selectedSymptoms = [];
                    Clinical_HPIComplaints.selectedSymptomId = 0;
                    Clinical_HPIComplaints.selectedTemplateSymptomId = 0;
                    Clinical_HPIComplaints.selectedFindings = [];
                    // UnloadActionPan(Clinical_HPIComplaints.params["ParentCtrl"], "Clinical_HPIComplaints");
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                objDeffered.resolve();
            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },

    addHPIComplaintsToNotesDB_Call: function (HPITempSymFindingIds, HPITemplateSymptoms) {
        var objV = new Object();
        objV["HPITempSymFindingIds"] = HPITempSymFindingIds;
        objV["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objV["NotesFindingsData"] = HPITemplateSymptoms;
        objV["commandType"] = "Save_hpi_Notesfinding";
        var datav = JSON.stringify(objV);
        return MDVisionService.APIService(datav, "HPI", "HPI");
    },

    createHPIComplaintBodyHTML: function (response, NoteHTMLCtrl, UnloadHPIComplaint, bNotSaveCompt) {
        Clinical_HPIComplaints.checkComplaintHxExists();
        $('[id^="Cli_HPIComplaints_Temp"]').parent().html('');
        $('[id^="Cli_HPIComplaints_Temp"]').remove();

        if (response.HPITemplateCount > 0) {
            var $SectionBodyComplaint = $(document.createElement('section'));
            $SectionBodyComplaint.attr('id', "Cli_Complaint_Main" + Clinical_HPIComplaints.ComplaintId);
            HPITemplate_JSON = JSON.parse(response.HPITemplate_JSON);
            HPISymptoms_JSON = JSON.parse(response.HPISymptoms_JSON);
            $.each(HPITemplate_JSON, function (i, item) {
                var HPITemplateId = item.HPITemplateId;
                var $mainDiv = $(document.createElement('section'));
                $mainDiv.attr('HPITemplateId', HPITemplateId);
                $mainDiv.attr('id', "Cli_HPIComplaints_Temp" + HPITemplateId);

                var $ListVital = $(document.createElement('ul'));
                $ListVital.attr('class', 'list-unstyled');
                var HPITemplateId = item.HPITemplateId;
                //var $SectionBodyVital = $(document.createElement('section'));
                //$SectionBodyVital.attr('id', "Cli_HPIComplaints_Temp" + HPITemplateId);

                $($ListVital).append('<li><header class="pl-none">' +
                                     '<Cli_HPIComplaints_' + HPITemplateId + ' title="Complaints"  id="' + HPITemplateId + '">' +
                                     '<a class="btn btn-link btn-xs pl-none" style="color:#bd0e09 !important"  onclick="Clinical_ProgressNote.OpenHPITemplateComplaintAddEdit(' + HPITemplateId + ');" title="Complaints">' + item.TemplateName + '</a> ' +
'<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'HPIComplaints\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ',null,null,' + HPITemplateId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                '</Cli_HPIComplaints_' + HPITemplateId + '> </header></li>');
                // $SectionBodyVital.append($ListVital);
                $mainDiv.append($ListVital);

                //var HPITemplateId = item.HPITemplateId;
                //var $mainDiv = $(document.createElement('section'));
                //$mainDiv.attr('HPITemplateId', HPITemplateId);
                //$mainDiv.attr('id', "Cli_HPIComplaints_Temp" + HPITemplateId);
                //var $mainList = $(document.createElement('ul'));
                //$mainList.attr('class', 'list-unstyled');
                //$($mainList).append('<li><span style="color:#4b0575">' + item.TemplateName + '</span></li>');
                //$mainDiv.append($mainList);
                $.each(HPISymptoms_JSON, function (i, item) {
                    if (item.HPITemplateId == HPITemplateId) {
                        var TemplatePreview = "<div>" + item.Desc + "</div>";
                        var $List = $(document.createElement('ul'));
                        $List.attr('class', 'list-unstyled');
                        var HPISymptomId = item.HPISymptomId;
                        $($List).append('<li><a class="btn btn-link btn-xs pl-none" style="color:green !important;margin-right: -5px;" onclick="Clinical_ProgressNote.OpenHPIComplaintSymFindingDetail(this,' + item.HPITemplateSymptomId + ',' + HPISymptomId + ',' + HPITemplateId + ');"'
                                        + 'title="HPI Template Symptom">' + item.SymptomName + '</a>'
                                        + '<br><span id="Cli_HPICompl_HPINFindingsId' + item.HPINotesFindingsId + '" style="' + Clinical_HPIComplaints.Content_style + '">' + item.Desc/*Clinical_HPIComplaints.getSymtopInnerContent(TemplatePreview, HPISymptomId)*/ + '</span   ></li>');

                        //$($List).append('<li class="initialVisitBody"><header class="pl-none"><cli_hpi_' + item.HPITemplateId + item.HPISymptomId + '  + title="HPI Template Symptom" id=' + item.HPITemplateSymptomId + '><a class="btn btn-link btn-xs pl-none" style="color:green !important;margin-right: -5px;" onclick="Clinical_ProgressNote.OpenHPIComplaintSymFindingDetail(this,' + item.HPITemplateSymptomId + ',' + HPISymptomId + ',' + HPITemplateId + ');"'
                        //    + 'title="HPI Template Symptom">' + item.SymptomName + '</a><a onclick="Clinical_ProgressNote.RemoveComponentTab(this,' + item.HPITemplateSymptomId + ',' + HPISymptomId + ',' + HPITemplateId + ');"'
                        //    + 'class="btn btn-link btn-xs closeBtn hidden" title="Remove"><i class="fa fa-times"></i></a><br><span id="Cli_HPICompl_HPINFindingsId' + item.HPINotesFindingsId + '" style="' + Clinical_HPIComplaints.Content_style + '">' + item.Desc + '</span></header></li>');
                        $mainDiv.append($List);

                    }
                });
                //Start 06-10-2017 Edit by Humaira Yousaf Bug# EMR-4913
                $mainDiv.append(HPITemplate_JSON[i].Comments);
                //End 06-10-2017 Edit by Humaira Yousaf Bug# EMR-4913
                $SectionBodyComplaint.append($mainDiv);
            });
            //   $($SectionBodyComplaint).append($('#' + Clinical_HPIComplaints.params["PanelID"] + ' #txtTemplateComments').val());

            $(NoteHTMLCtrl + ' Clinical_Complaints').parent().parent().addClass('initialVisitBody  ml-none');
            $(NoteHTMLCtrl + ' Clinical_Complaints').parent().parent().append($SectionBodyComplaint);
            $(NoteHTMLCtrl + ' Clinical_Complaints').parent().parent().find("span#allComments").remove();

            var overallComments = $('#' + Clinical_HPIComplaints.params["PanelID"] + ' #txtOverallComments').val();
            if (overallComments != null) {
                $(NoteHTMLCtrl + ' Clinical_Complaints').parent().parent().append('<span id="allComments">' + overallComments + '</span>');
            }
        }
        Clinical_ProgressNote.saveComponentSOAPText('Complaints', true);
        Clinical_ProgressNote.hoverFunction();
    },

    checkHPIComplaintExists: function () {
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML section[id*="Cli_HPIComplaints_Main"]').parent().remove();
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML section[id*="Cli_HPIComplaints_Main"]').remove();
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Complaints').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ComplaintsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<Clinical_Complaints title="Complaints"  id="' + Clinical_HPIComplaints.ComplaintId + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'HPIComplaints\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Complaints">Complaints</a> ' +
                       '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Complaints\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Complaints\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</Clinical_Complaints> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    loadFindingsDetails: function (HPITemplateSymptomId, findingId) {
        var isExist = false;

        if (Clinical_HPIComplaints.selectedFindings) {
            for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                if (Clinical_HPIComplaints.selectedFindings[i].HPITemplateSymptomId == HPITemplateSymptomId) {
                    isExist = true;
                    break;
                }
            }
        }

        if (Clinical_HPIComplaints.selectedFindings) {
            for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                if (Clinical_HPIComplaints.selectedFindings[i].HPITemplateSymptomId == HPITemplateSymptomId) {
                    if (Clinical_HPIComplaints.selectedFindings[i].FindingId == findingId) {
                        $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').resetAllControls(null);
                        $('#' + Clinical_HPIComplaints.params.PanelID + " ddlLocation option").removeAttr("selected");
                        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation').multiselect('clearSelection');
                        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation').multiselect("refresh");
                        var self = $('#' + Clinical_HPIComplaints.params.PanelID + " #sectionFindingDetails");

                        utility.bindMyJSONByName(true, Clinical_HPIComplaints.selectedFindings[i], false, self).done(function () {
                            if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_LocationIds) {
                                $('#' + Clinical_HPIComplaints.params.PanelID + " #ddlLocation").val(Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_LocationIds.split(','));
                                $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation').multiselect("refresh");
                            }

                            if ((findingId == -1 || findingId == "0") && Clinical_HPIComplaints.selectedFindings[i].HPISymptomsAnswersId == "1") {
                                Clinical_HPIComplaints.addFindingDetailInPreview(Clinical_HPIComplaints.selectedFindings[i].HPISymptomId, null);
                            }
                            //Start 03-10-2017 Edit By Humaira Yousaf IMP-1172
                            var symptomId = Clinical_HPIComplaints.selectedFindings[i].HPISymptomId;
                            var detailComments = $("#findingContent #divSymptom" + symptomId + 'Finding' + findingId + 'txtDetailComments');
                            if (detailComments.length > 0) {
                                $('#txtDetailComments').val($(detailComments).text().trim().slice(0, -1));
                            }
                            //End 03-10-2017 Edit By Humaira Yousaf IMP-1172
                        });
                    }
                }
            }
        }

    },

    HPISymptomFindingsDetailsLoad: function (HPISymptomDetailId, HPITemplateSymptomId) {
        var objData = new Object();
        objData["HPISymptomDetailId"] = HPISymptomDetailId;
        objData["HPITemplateSymptomId"] = HPITemplateSymptomId;
        objData["commandType"] = "Load_HPI_Symptom_Findings_Detail";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    PreviewFindings: function (findingId, findingName, obj, HPISymptomId) {

        var prevFinding = $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li.active').attr('id');
        if (prevFinding) {
            Clinical_HPIComplaints.addFindingsDetails(HPISymptomId, prevFinding);
        }
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');

        var HPITemplateSymptomId = $("#ulHPISymptoms #" + HPISymptomId).attr('hpitemplatesymptomid');
        Clinical_HPIComplaints.loadFindingsDetails(HPITemplateSymptomId, findingId);

        $("#SymptomPreview").removeClass('hidden');
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li').removeClass('active');
        $(obj).addClass('active');

        if ($(obj).prop('checked') == true) {
            $(this).find("[type='checkbox'][id*='chk']").prop("checked", true);
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li .char').removeClass("green");
            $(this).find("[type='checkbox'][id*='chk']").prop("checked", false);
        }

        var isChk = $(obj).prop('checked') == true ? true : false;

        var objSelectedFindings =
        {
            HPISymptomId: HPISymptomId,
            IsChecked: isChk,
            FindingId: findingId,
            FindingName: findingName,
            IsModified: '1',
            IsSymptomChecked: false,
            SymptomDescription: ""
        };

        if ($(obj).prop('checked') == true) {

            $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').remove();

            var deli = $("#delimator option:selected").text() + " ";

            if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).length > 0) {
                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).remove();
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + findingId);
                $newDiv.attr("style", "display: inline;");

                $("#findingContent").append($newDiv);
                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).show();
                var txttoAppend = findingName;
                //if ($('#findingContent div').length > 1)
                //    txttoAppend = deli + findingName;

                if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length > 1)
                    txttoAppend = findingName + deli;

                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).append(txttoAppend);
            }
            else {
                var symptomName = '';
                if ($("#findingContent #divSymptom" + HPISymptomId).length == 0) {
                    symptomName = $("#ulHPISymptoms tr[id=" + HPISymptomId + "] label").text();
                }
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + findingId);
                $newDiv.attr("style", "display: inline;");

                var txttoAppend = findingName;

                if ($("#findingContent").find("div[id='divSymptom" + HPISymptomId + "']").length > 0)
                    txttoAppend = findingName + deli;

                $("#findingContent").find("div[id='divSymptom" + HPISymptomId + "']").append($newDiv);

                // $("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").last().after($newDiv);

                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).show();
                //var txttoAppend = findingName;

                //if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length > 1)
                //    txttoAppend = deli + findingName;

                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).append(txttoAppend);

                Clinical_HPIComplaints.addFindingDetailInPreview(HPISymptomId, findingId);
            }

            if (Clinical_HPIComplaints.selectedFindings) {
                for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId && Clinical_HPIComplaints.selectedFindings[i].FindingId == findingId) {
                        Clinical_HPIComplaints.selectedFindings[i].IsChecked = true;
                        Clinical_HPIComplaints.selectedFindings[i].IsSymptomChecked = true;
                        Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                    }
                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        Clinical_HPIComplaints.selectedFindings[i].IsSymptomChecked = true;
                        Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                    }
                }
            }

            // $("#ulHPISymptoms #chk" + HPISymptomId).prop("checked", true);
            $("#ulHPISymptoms tr.active input[symptomtype = '1']").prop("checked", true);

            //  Clinical_HPIComplaints.loadFindings(HPISymptomId);
        }
        else if ($(obj).prop('checked') == false) {
            if (Clinical_HPIComplaints.selectedFindings) {
                for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId && Clinical_HPIComplaints.selectedFindings[i].FindingId == findingId) {
                        Clinical_HPIComplaints.selectedFindings[i].IsChecked = false;
                        Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                    }
                }
            }
            var aa = $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).text();
            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).remove();
            Clinical_HPIComplaints.removeLastDelimiter(HPISymptomId);
        }
        Clinical_HPIComplaints.removeLastDelimiter(HPISymptomId);
        $("#findingContent").trigger("contentchange");
    },

    //PreviewFindings: function (findingId, findingName, obj, HPISymptomId) {

    //    var prevFinding = $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li.active').attr('id');
    //    if (prevFinding) {
    //        Clinical_HPIComplaints.addFindingsDetails(HPISymptomId, prevFinding);
    //    }
    //    $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');
    //    Clinical_HPIComplaints.loadFindingsDetails(HPISymptomId, findingId);

    //    $("#SymptomPreview").removeClass('hidden');
    //    $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li').removeClass('active');
    //    $(obj).addClass('active');

    //    if ($(obj).prop('checked') == true) {
    //        $(this).find("[type='checkbox'][id*='chk']").prop("checked", true);
    //    }
    //    else if ($(obj).prop('checked') == false) {
    //        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li .char').removeClass("green");
    //        $(this).find("[type='checkbox'][id*='chk']").prop("checked", false);
    //    }

    //    var isChk = $(obj).prop('checked') == true ? true : false;

    //    var objSelectedFindings =
    //    {
    //        HPISymptomId: HPISymptomId,
    //        IsChecked: isChk,
    //        FindingId: findingId,
    //        FindingName: findingName,
    //        IsModified: '1',
    //        IsSymptomChecked: false,
    //        SymptomDescription: ""
    //    };

    //    if ($(obj).prop('checked') == true) {

    //        $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').remove();
    //        $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').remove();
    //        $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').remove();
    //        $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').remove();
    //        $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').remove();
    //        $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').remove();
    //        $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').remove();
    //        $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').remove();

    //        var deli = $("#delimator option:selected").text() + " ";

    //        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).length > 0) {
    //            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).remove();
    //            var $newDiv = $("<div></div>");
    //            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + findingId);
    //            $newDiv.attr("style", "display: inline;");

    //            $("#findingContent").append($newDiv);
    //            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).show();
    //            var txttoAppend = findingName;
    //            //if ($('#findingContent div').length > 1)
    //            //    txttoAppend = deli + findingName;

    //            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length > 1)
    //                txttoAppend = findingName + deli;

    //            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).append(txttoAppend);
    //        }
    //        else {
    //            var symptomName = '';
    //            if ($("#findingContent #divSymptom" + HPISymptomId).length == 0) {
    //                symptomName = $("#ulHPISymptoms tr[id=" + HPISymptomId + "] label").text();
    //            }
    //            var $newDiv = $("<div></div>");
    //            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + findingId);
    //            $newDiv.attr("style", "display: inline;");

    //            var txttoAppend = findingName;

    //            if ($('#findingContent div').length > 1)
    //                txttoAppend = findingName + deli;

    //            $("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").last().after($newDiv);

    //            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).show();
    //            //var txttoAppend = findingName;

    //            //if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length > 1)
    //            //    txttoAppend = deli + findingName;

    //            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).append(txttoAppend);
    //        }

    //        if (Clinical_HPIComplaints.selectedFindings) {
    //            for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
    //                if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId && Clinical_HPIComplaints.selectedFindings[i].FindingId == findingId) {
    //                    Clinical_HPIComplaints.selectedFindings[i].IsChecked = true;
    //                    Clinical_HPIComplaints.selectedFindings[i].IsSymptomChecked = true;
    //                    Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
    //                }
    //                if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId) {
    //                    Clinical_HPIComplaints.selectedFindings[i].IsSymptomChecked = true;
    //                    Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
    //                }
    //            }
    //        }

    //        // $("#ulHPISymptoms #chk" + HPISymptomId).prop("checked", true);
    //        $("#ulHPISymptoms tr.active input[symptomtype = '1']").prop("checked", true);

    //        //  Clinical_HPIComplaints.loadFindings(HPISymptomId);
    //    }
    //    else if ($(obj).prop('checked') == false) {
    //        if (Clinical_HPIComplaints.selectedFindings) {
    //            for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
    //                if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId && Clinical_HPIComplaints.selectedFindings[i].FindingId == findingId) {
    //                    Clinical_HPIComplaints.selectedFindings[i].IsChecked = false;
    //                    Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
    //                }
    //            }
    //        }
    //        var aa = $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).text();
    //        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).remove();
    //        Clinical_HPIComplaints.removeLastDelimiter(HPISymptomId);
    //    }
    //    Clinical_HPIComplaints.removeLastDelimiter(HPISymptomId);
    //    $("#findingContent").trigger("contentchange");
    //},


    FillHPITemplates: function (TempIds) {
        // $("#ulHPITemplates li").removeClass('text-success');
        //$("#ulSelectedHPITemplates li").removeClass('active');
        Clinical_HPIComplaints.selectedSymptoms = [];
        var HPITemplateId = "";
        Clinical_HPIComplaints.fillHPITempalteForSoap(Clinical_HPIComplaints.params.HPITemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                PETemplate_JSON = response.HPITemplate_JSON;
                $.each(PETemplate_JSON, function (i, item) {
                    if (item.NotesId && item.NotesId == Clinical_HPIComplaints.params.NotesId) {
                        HPITemplateId = item.HPITemplateId;
                        $("#ulHPITemplates").find('#HPITemplate_' + HPITemplateId).addClass('text-success');
                        Clinical_HPIComplaints.params.HPITemplateId = HPITemplateId;

                        //var isSelected = item.IsSelected == "True" ? true : false;
                        //var objSelectedSymptom = {
                        //    HPISymptomId: item.HPISymptomId, IsSelected: item.IsSelected == "True" ? true : false, HPITemplateId: item.HPITemplateId, SymptomName: item.SymptomName,
                        //    HPITemplateSymptomId: item.HPITemplateSymptomId, SymptomDescription: item.SymptomDescription, HPINotesFindingsId: -1, HPISymptomsAnswersId: item.HPISymptomsAnswersId,
                        //    IsCurrent: true, Comments: item.Comments
                        //};

                        //Clinical_HPIComplaints.selectedSymptoms.push(objSelectedSymptom);
                    }
                });


                var HPISymptomId = "";
                var HPITemplateSymptomId = "";
                if (response.HPITemplateSymptomsCount > 0) {
                    var SymptomData = response.HPITemplateSymptoms_JSON;

                    for (var i = 0; i < SymptomData.length; i++) {
                        var isSelected = SymptomData[i].IsSelected == "True" ? true : false;

                        var objSelectedSymptom = {
                            HPISymptomId: SymptomData[i].HPISymptomId, IsSelected: isSelected, HPITemplateId: SymptomData[i].HPITemplateId, SymptomName: SymptomData[i].SymptomName,
                            HPITemplateSymptomId: SymptomData[i].HPITemplateSymptomId, SymptomDescription: SymptomData[i].SymptomDescription, HPINotesFindingsId: SymptomData[i].HPINotesFindingsId,
                            HPISymptomsAnswersId: SymptomData[i].HPISymptomsAnswersId, IsCurrent: false, SymptomOrder: SymptomData[i].SymptomOrder
                        };

                        Clinical_HPIComplaints.selectedSymptoms.push(objSelectedSymptom);

                        if (SymptomData[i].HPITemplateId == Clinical_HPIComplaints.params.SelectedHPITemplateId) {
                            var li = Clinical_HPIComplaints.addSymptom(SymptomData[i].HPISymptomsId, SymptomData[i].SymptomName, SymptomData[i].HPITemplateId,
                                SymptomData[i].HPITemplateSymptomId, SymptomData[i].HPISymptomsAnswersId, SymptomData[i].SymptomOrder);

                            $("#ulHPISymptoms").append(li);
                            $("#ulHPISymptoms #" + SymptomData[i].HPISymptomId).addClass('active');
                            HPISymptomId = SymptomData[i].HPISymptomId;
                            HPITemplateSymptomId = SymptomData[i].HPITemplateSymptomId;
                        }
                    }
                }

                if (Clinical_HPIComplaints.params.SelectedHPITemplateId) {
                    $("#ulHPITemplates").find('#HPITemplate_' + Clinical_HPIComplaints.params.SelectedHPITemplateId).trigger('click');
                }
                else {
                    $("#ulHPITemplates").find('#HPITemplate_' + HPITemplateId).trigger('click');
                }
            }

        });
    },

    fillHPITempalteForSoap: function (hpitemplateId) {
        var objData = new Object();
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "fill_hpi_for_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },
    addNewItem: function (itemType) {
        if (itemType != null && itemType != "") {

            //  var addSubCharIcon = "";

            var symptomsSelectAll = null;
            var findingsSelectAll = null;

            // var isSubCharacteristic = false;
            var ulControl = "";
            var currentLiClick = "";
            var currentCtrlId = "";
            var currentParentId = "";
            var currentId = "";
            currentId = Clinical_HPIComplaints.NewInsertedId--;
            var isExist = "";

            var saveMethodHPI = "";

            if (itemType.toLowerCase() == "symptom") {
                currentLiClick = "";
                currentCtrlId = "ulHPISymptoms";
                ulControl = $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #" + currentCtrlId);
                saveMethodHPI = "Clinical_HPIComplaints.AddSymptomHPI(this, '" + currentId + "',event);";
            }
            else if (itemType.toLowerCase() == "finding") {
                currentLiClick = "";
                currentCtrlId = "ulHPISymptomFindings";
                ulControl = $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #" + currentCtrlId);
                saveMethodHPI = "Clinical_HPIComplaints.AddFindingHPI(this, '" + currentId + "',event);";
            }

            if (ulControl != null && ulControl != "") {
                var currentLiClass = "";

                var arrNewlyAddedLi = ulControl.find("li[id*='-']");

                if (itemType.toLowerCase() != "symptom") {
                    currentParentId = ulControl.find("li:last").attr("parentid");
                    if (currentParentId == null)
                        currentParentId = ulControl.attr("ParentId");
                }

                var onClick = "";
                var deleteFunction = "Clinical_HPIComplaints.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var trTobeAdded = '';
                var HPISymptomId = $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptoms tr.active').attr('id');

                if (itemType.toLowerCase() == "symptom") {
                    trTobeAdded = '<tr id="' + currentId + '" parentid="null" onclick="Clinical_HPIComplaints.loadFindings(' + currentId + ')" value="' + currentId + '" refvalue="" subcharacteristicexist=" "><td class="text-center">' +
                                      '<input type="radio" symptomtype="1" id="chk' + currentId + '" name="SymptomsType' + currentId + '" onclick="Clinical_HPIComplaints.ManageFindings(' + currentId + ', this,event);"></td><td class="text-center"><input type="radio" symptomtype="2" id="chk' + currentId + '" name="SymptomsType' + currentId + '" onclick="Clinical_HPIComplaints.ManageFindings(' + currentId + ', this,event);"></td><td>' +
                                      '<div class="col-xs-11 p-none"><textarea rows="1" id="txtName' + currentId + '" onkeypress=""  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
                                      + '<div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodHPI + '" class="btn btn-link btn-xs">'
                                      + '<i class="fa fa-save"></i></span></div></div><a href="#" class="removeIconListHover pull-right" onclick="' + deleteFunction + '"><i class="fa fa-close"></i></a>'
                                     + '</td></tr>';
                }

                else if (itemType.toLowerCase() == "finding") {
                    var trTobeAdded = '<li id="' + currentId + '" parentid="' + HPISymptomId + '" onclick="Clinical_HPIComplaints.PreviewFindings(' + currentId + ',\'' + '' + '\', this, ' + HPISymptomId + ');" value="' + currentId + '" refvalue="" subcharacteristicexist=" " class=""><div class="pull-left checkbox-custom checkboxTiny checkbox-success">' +
                   '<input type="checkbox" id="chk' + currentId + '" name="' + currentId + '" class="pull-left" ' +
                   'onclick="Clinical_HPIComplaints.PreviewFindings(' + currentId + ',\'' + '' + '\', this, ' + HPISymptomId + ');"><label></label></div><div class="col-xs-10 p-none"><textarea spellcheck="true" rows="1" id="txtName' + currentId + '" onkeypress=""  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 textAreaScroll"></textarea>' +
                   '<div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodHPI + '" class="btn btn-link btn-xs">'
                                      + '<i class="fa fa-save"></i></span></div></div><a href="#" class="removeIconList" onclick="' + deleteFunction + '"><i class="fa fa-close"></i></a><div class="clearfix"></div></li>';
                }

                if (symptomsSelectAll != null && symptomsSelectAll.length > 0) {
                    $(trTobeAdded).insertAfter("#chkboxSelectAllChars");
                }
                else if (findingsSelectAll != null && findingsSelectAll.length > 0) {
                    $(trTobeAdded).insertAfter("#chkboxSelectAllSubChars");
                }
                else {
                    if (itemType.toLowerCase() == "symptom") {
                        ulControl.find('tr:first').after(trTobeAdded);
                        $(ulControl).removeClass('hidden');
                    }
                    else {
                        if (ulControl.find('li:first').length == 0) {
                            var selectAll = '<li id="chkboxSelectAllFindings" parentid="' + HPISymptomId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllFindings" name="Select All" class="pull-left" '
                            + 'onclick="Clinical_HPIComplaints.selectAllFindings(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                            $(ulControl).append(selectAll);
                        }
                        ulControl.find('li:first').after(trTobeAdded);
                        $(ulControl).removeClass('hidden');

                    }
                }
                ulControl.find('tr#' + currentId + ' #txtName' + currentId).focus()
            }
        }
    },


    AddSymptomHPI: function (obj, controlId, event) {
        event.stopPropagation();
        var objData = new Object();
        objData["IsGlobal"] = false;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = true;
        objData["HPITemplateID"] = Clinical_HPIComplaints.params.HPITemplateId;//$("#ulHPITemplates").find('li.text-success').val();

        var symptomOrder = 1;
        if ($("#ulHPISymptoms tr").length > 1) {
            var lastOrder = $("#ulHPISymptoms tr").last().attr('symptomOrder');
            if (lastOrder != null) {
                symptomOrder = parseInt(lastOrder) + 1;
            }
            else {
                symptomOrder = 1;
            }
        }

        objData["SymptomOrder"] = symptomOrder;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }
        else if (objData["HPITemplateID"] == 0 || objData["HPITemplateID"] == null || objData["HPITemplateID"] == "" || objData["HPITemplateID"] == undefined) {
            utility.DisplayMessages("Please select template", 3);
            return;
        }

        Clinical_HPIComplaints.saveHPISDymptom_DBCall(objData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                var templateSymptom = response.TemplateSymptoms;

                $('#findingContent').html('');
                var li = Clinical_HPIComplaints.addSymptom(response.HPISymptomsId, objData["Name"], Clinical_HPIComplaints.params.HPITemplateId, templateSymptom[0].HPITemplateSymptomId, "", symptomOrder);
                $("#ulHPISymptoms").append(li);
                $("#ulHPISymptoms tr[id=" + response.HPISymptomsId + "]").addClass('active');
                $("#ulHPISymptoms tr[id=" + response.HPISymptomsId + "]").trigger('click');


                var objSelectedSymptom = {
                    HPISymptomId: response.HPISymptomsId, IsSelected: true, HPITemplateId: objData["HPITemplateID"], SymptomName: objData["Name"],
                    HPITemplateSymptomId: templateSymptom[0].HPITemplateSymptomId,
                    SymptomDescription: '', HPINotesFindingsId: -1, HPISymptomsAnswersId: null,
                    IsCurrent: true, SymptomOrder: symptomOrder
                };

                Clinical_HPIComplaints.selectedSymptoms.push(objSelectedSymptom);
                $("#" + controlId).remove();
            }
            else {
                utility.DisplayMessages(response.Message, 1);
            }
        });
    },
    saveHPISDymptom_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "insert_hpi_symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPISymptoms");
    },


    AddFindingHPI: function (obj, controlId, event) {
        event.stopPropagation();

        var hpiSymptomsId = $("#ulHPISymptoms tr.active")[0].id;
        var objData = new Object();
        objData["HPISymptomsId"] = hpiSymptomsId;
        objData["HPITemplateSymptomId"] = Clinical_HPIComplaints.selectedTemplateSymptomId;

        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = true;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        else if (objData["HPISymptomsId"] == 0 || objData["HPISymptomsId"] == null || objData["HPISymptomsId"] == "" || objData["HPISymptomsId"] == undefined) {
            utility.DisplayMessages("Please select symptom", 3);
            return;
        }
        else if (objData["HPITemplateSymptomId"] == 0 || objData["HPITemplateSymptomId"] == null || objData["HPITemplateSymptomId"] == "" || objData["HPITemplateSymptomId"] == undefined) {
            utility.DisplayMessages("HPITemplateSymptomId not found", 3);
            return;
        }

        var templateId = $('#divHPITemplates').find('li').first().attr('parentid');
        objData["HPITemplateId"] = templateId;
        var HPITemplateSymptomId = $("#ulHPISymptoms #" + hpiSymptomsId).attr('hpitemplatesymptomid');

        var findingOrder = 1;
        if ($("#ulHPISymptomFindings li").length > 1) {
            var lastOrder = $("#ulHPISymptomFindings li").last().attr('findingOrder');
            if (lastOrder != null) {
                findingOrder = parseInt(lastOrder) + 1;
            }
            else {
                findingOrder = 1;
            }
        }
        Clinical_HPIComplaints.saveHPIFindings_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var res = response;
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_HPIComplaints.params.HPIFindingId = res.HPIFindingsId;

                    Clinical_HPIComplaints.updateHPISymptoms_DBCall(res.HPIFindingsId, hpiSymptomsId, HPITemplateSymptomId, findingOrder).done(function (response) {
                        if (response != "") {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                var li = Clinical_HPIComplaints.addFindings(res.HPIFindingsId, objData["Name"], hpiSymptomsId, "", findingOrder);
                                $("#ulHPISymptomFindings").append(li);
                                var sysChecked = $("#chk" + $("#ulHPISymptoms tr.active").attr('id')).is(':checked');

                                var objselectedFindings = {
                                    HPISymptomId: hpiSymptomsId, IsChecked: false, FindingId: res.HPIFindingsId, FindingName: objData["Name"], IsSymptomChecked: sysChecked,
                                    SymptomDescription: "", IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: null, IsSymptomsDetail: false, HPISymptoms_SeverityId: 0, HPISymptoms_LocationIds: '', HPISymptoms_RadiationId: 0,
                                    HPISymptoms_QualityId: 0, Onset: '', AssociatedWith: '', HPISymptoms_AggravatedById: 0, HPISymptoms_RelievedById: 0, DetailComments: '', FindingOrder: findingOrder
                                };

                                Clinical_HPIComplaints.selectedFindings.push(objselectedFindings);
                                $("#" + controlId).remove();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        }
                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });
    },

    saveHPIFindings_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "insert_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },

    updateHPISymptoms_DBCall: function (HPIFindingsId, HPISymptomsId, HPITemplateSymptomId, findingOrder) {
        var objData = new Object();

        objData["HPIFindingsIds"] = HPIFindingsId;
        objData["HPISymptomsId"] = HPISymptomsId;
        objData["IsActive"] = true;
        objData["IsGlobal"] = true;
        objData["Name"] = $("#ulHPISymptoms tr.active").text();
        objData["HPITemplateSymptomId"] = HPITemplateSymptomId;
        objData["commandType"] = "update_hpi_symptoms";
        objData["FindingOrder"] = findingOrder;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPISymptoms");

    },
    buildFindingsAutoComplete: function () {
        var objDeffered = $.Deferred();
        Clinical_HPIComplaints.lookupHPIFindings_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Findings_JSON = response.listFindings;
                    $('#' + Clinical_HPIComplaints.params.PanelID + " #Findings").kendoAutoComplete({
                        dataSource: Findings_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Finding...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            if ($("#ulHPISymptomFindings").find('#' + dataItem.HPIFindingsId).length == 0) {
                                var HPISymptomId = Clinical_HPIComplaints.getCurrentSymptomId();
                                var HPITemplateSymptomId = Clinical_HPIComplaints.getCurrentTemplateSymptomId();
                                Clinical_HPIComplaints.addFindingSymptomAssosiation(dataItem.HPIFindingsId, HPISymptomId, dataItem.Name, HPITemplateSymptomId);
                                $("#Findings").val('');
                            }
                            else {
                                utility.DisplayMessages(dataItem.Name + " already exists.", 3);
                            }
                        },
                    });
                    $("#Findings").parent().addClass('size100per');
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            }
            objDeffered.resolve();
        });
        return objDeffered.promise();
    },

    lookupHPIFindings_DBCall: function () {

        var objData = new Object();
        objData["IsActive"] = 1;
        objData["commandType"] = "lookup_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },
    addFindingSymptomAssosiation: function (findingId, HPISymptomId, Name, HPITemplateSymptomId) {

        var objData = new Object();
        objData["HPISymptomsId"] = HPISymptomId;
        objData["HPITemplateSymptomId"] = HPITemplateSymptomId;

        objData["HPIFindingsIds"] = findingId;
        if (!objData["HPIFindingsIds"] || !objData["HPISymptomsId"]) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        var findingOrder = 1;
        if ($("#ulHPISymptomFindings li").length > 1) {
            var lastOrder = $("#ulHPISymptomFindings li").last().attr('findingOrder');
            if (lastOrder != null) {
                findingOrder = parseInt(lastOrder) + 1;
            }
            else {
                findingOrder = 1;
            }
        }

        objData["FindingOrder"] = findingOrder;

        Clinical_HPIComplaints.addFindingSymptomAssosiation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                utility.DisplayMessages(response.Message, 1);
                Clinical_HPIComplaints.params.HPIFindingId = findingId;
                if (findingId) {
                    $("#Findings").val('');

                    var objselectedFindings = {
                        HPISymptomId: HPISymptomId, IsSelected: true,
                        FindingId: findingId, FindingName: Name, IsSymptomChecked: true, HPITemplateSymptomId: HPITemplateSymptomId, HPISymptomsAnswersId: true, IsSymptomsDetail: false,
                        SymptomDescription: "", HPISymptoms_SeverityId: 0, HPISymptoms_LocationIds: '', HPISymptoms_RadiationId: 0,
                        HPISymptoms_QualityId: 0, Onset: '', AssociatedWith: '', HPISymptoms_AggravatedById: 0, HPISymptoms_RelievedById: 0, DetailComments: '', FindingOrder: findingOrder
                    };
                    Clinical_HPIComplaints.selectedFindings.push(objselectedFindings);

                    var li = Clinical_HPIComplaints.addFindings(findingId, Name, HPISymptomId, "", findingOrder);
                    $("#ulHPISymptomFindings").append(li);
                }
            }
        });
    },

    addFindingSymptomAssosiation_DBCall: function (data) {
        var objData = new Object();
        if (data)
            objData = data;
        objData["commandType"] = "Insert_Findings_Symptom_Assosication";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    selectAllFindings: function (obj) {

        if (obj) var symptomId = $($($(obj).parent().parent())[0]).attr('parentid');

        $("#findingContent #divSymptom" + symptomId + 'ddlSeverity').remove();
        $("#findingContent #divSymptom" + symptomId + 'ddlLocation').remove();
        $("#findingContent #divSymptom" + symptomId + 'ddlRadiation').remove();
        $("#findingContent #divSymptom" + symptomId + 'ddlQuality').remove();
        $("#findingContent #divSymptom" + symptomId + 'txtOnset').remove();
        $("#findingContent #divSymptom" + symptomId + 'txtAssociatedWith').remove();
        $("#findingContent #divSymptom" + symptomId + 'ddlAggravatedBy').remove();
        $("#findingContent #divSymptom" + symptomId + 'ddlRevieledBy').remove();
        $("#findingContent #divSymptom" + symptomId + 'txtDetailComments').remove();

        if ($(obj).prop('checked') == true) {
            $("#SymptomPreview").removeClass('hidden');
            $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);

                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3)
                    var findingName = $("#divHPISymptomFindings #lblName" + id_).text();
                    var delimator = $("#delimator option:selected").text() + " ";

                    if ($("#findingContent #divSymptom" + symptomId + 'Finding' + id_).length > 0) {
                        $('#findingContent #divSymptom' + symptomId + 'Finding' + id_).remove();
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + symptomId + 'Finding' + id_);
                        $newDiv.attr("style", "display: inline;");

                        $("#findingContent").find("div[id='divSymptom" + symptomId + "']").append($newDiv);
                        $('#findingContent #divSymptom' + symptomId + 'Finding' + id_).show();
                        var txttoAppend = findingName;

                        if ($("#findingContent").find("div[id^='divSymptom" + symptomId + "']").length > 1)
                            txttoAppend = findingName + delimator;

                        $("#findingContent #divSymptom" + symptomId + 'Finding' + id_).append(txttoAppend);
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + symptomId + 'Finding' + id_);
                        $newDiv.attr("style", "display: inline;");

                        var txttoAppend = findingName;

                        if ($("#findingContent").find("div[id='divSymptom" + symptomId + "']").length > 0)
                            txttoAppend = findingName + delimator;

                        $("#findingContent").find("div[id='divSymptom" + symptomId + "']").append($newDiv);
                        // $("#findingContent").find("div[id^='divSymptom" + symptomId + "']").last().after($newDiv);

                        $('#findingContent #divSymptom' + symptomId + 'Finding' + id_).show();

                        $("#findingContent #divSymptom" + symptomId + 'Finding' + id_).append(txttoAppend);

                    }
                }
            });

            if (Clinical_HPIComplaints.selectedFindings) {
                for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {

                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == symptomId) {
                        Clinical_HPIComplaints.selectedFindings[i].IsSelected = true;
                        Clinical_HPIComplaints.selectedFindings[i].IsSymptomChecked = true;
                        Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + symptomId).text();
                    }
                }
            }
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li .char').removeClass("green");
            $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                    var symptom_id = $($($(obj).parent().parent())[0]).attr('parentid');
                    $("#findingContent #divSymptom" + symptom_id + 'Finding' + id_).remove();
                }
            });

            if (Clinical_HPIComplaints.selectedFindings) {
                for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == symptomId) {

                        Clinical_HPIComplaints.selectedFindings[i].IsSelected = false;
                        Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = $('#observationContent #divSystem' + symptomId).text();
                    }
                }
            }
        }
        $("#findingContent").trigger("contentchange");
    },

    addFindingsDetails: function (HPITemplateSymptomId, findingId) {

        var LocationIDS = $('#ddlLocation option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var severity = $("#ddlSeverity option:Selected").val();
        var radiation = $("#ddlRadiation option:Selected").val();
        var quality = $("#ddlQuality option:Selected").val();
        var onset = $("#txtOnset").val();
        var associatedwith = $("#txtAssociatedWith").val();
        var aggravated = $("#ddlAggravatedBy option:Selected").val();
        var relieved = $("#ddlRevieledBy option:Selected").val();
        var detailComments = $("#txtDetailComments").val();

        if (Clinical_HPIComplaints.selectAllFindings) {
            if (findingId) {
                for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                    if (Clinical_HPIComplaints.selectedFindings[i].HPITemplateSymptomId == HPITemplateSymptomId) {
                        if (Clinical_HPIComplaints.selectedFindings[i].FindingId == findingId) {
                            Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_SeverityId = severity;
                            Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_LocationIds = LocationIDS;
                            Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RadiationId = radiation;
                            Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_QualityId = quality;
                            Clinical_HPIComplaints.selectedFindings[i].Onset = onset;
                            Clinical_HPIComplaints.selectedFindings[i].AssociatedWith = associatedwith;
                            Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_AggravatedById = aggravated;
                            Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RelievedById = relieved;
                            Clinical_HPIComplaints.selectedFindings[i].IsSymptomsDetail = false;
                            Clinical_HPIComplaints.selectedFindings[i].DetailComments = detailComments;

                        }
                    }
                }
            }
            else {
                var prevSymptomId = $("#ulHPISymptoms tr.active").attr('id');
                var prevTemplateSymptomId = $("#ulHPISymptoms tr.active").attr('hpitemplatesymptomid');

                var detailExists = false;
                if (LocationIDS) {
                    detailExists = true;
                }
                if (severity != '0') {
                    detailExists = true;
                }
                if (radiation != '0') {
                    detailExists = true;
                }
                if (quality != '0') {
                    detailExists = true;
                }
                if (onset != '') {
                    detailExists = true;
                }
                if (associatedwith != '') {
                    detailExists = true;
                }
                if (aggravated != '0') {
                    detailExists = true;
                }
                if (relieved != '0') {
                    detailExists = true;
                }
                if (detailComments != '') {
                    detailExists = true;
                }
                var findings = $('#' + Clinical_HPIComplaints.params.PanelID + ' #ulHPISymptomFindings li').length;


                if (detailExists || findings == 0) {
                    var indexSymptom = -1;

                    $.grep(Clinical_HPIComplaints.selectedFindings, function (item, index) {
                        if (item.HPITemplateSymptomId == prevTemplateSymptomId && item.IsSymptomsDetail == true) {
                            indexSymptom = index;
                            return;
                        }
                    });
                    if (indexSymptom == -1) {
                        var objSelectedFindings = {
                            HPISymptomId: prevSymptomId, HPITemplateSymptomId: prevTemplateSymptomId, IsChecked: false, FindingId: -1, FindingName: '', IsSymptomChecked: true, IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: $("#ulHPISymptoms tr.active  input:checked").attr('symptomtype'),
                            HPISymptoms_SeverityId: severity, HPISymptoms_LocationIds: LocationIDS, HPISymptoms_RadiationId: radiation, HPISymptoms_QualityId: quality, Onset: onset,
                            AssociatedWith: associatedwith, HPISymptoms_AggravatedById: aggravated, HPISymptoms_RelievedById: relieved, IsSymptomsDetail: true, DetailComments: detailComments
                        };

                        Clinical_HPIComplaints.selectedFindings.push(objSelectedFindings);
                    }
                    else {
                        Clinical_HPIComplaints.selectedFindings[indexSymptom].FindingId = -1;
                        Clinical_HPIComplaints.selectedFindings[indexSymptom].HPISymptoms_SeverityId = severity;
                        Clinical_HPIComplaints.selectedFindings[indexSymptom].HPISymptoms_LocationIds = LocationIDS;
                        Clinical_HPIComplaints.selectedFindings[indexSymptom].HPISymptoms_RadiationId = radiation;
                        Clinical_HPIComplaints.selectedFindings[indexSymptom].HPISymptoms_QualityId = quality;
                        Clinical_HPIComplaints.selectedFindings[indexSymptom].Onset = onset;
                        Clinical_HPIComplaints.selectedFindings[indexSymptom].AssociatedWith = associatedwith;
                        Clinical_HPIComplaints.selectedFindings[indexSymptom].HPISymptoms_AggravatedById = aggravated;
                        Clinical_HPIComplaints.selectedFindings[indexSymptom].HPISymptoms_RelievedById = relieved;
                        Clinical_HPIComplaints.selectedFindings[indexSymptom].HPISymptomsAnswersId = $("#ulHPISymptoms tr.active  input:checked").attr('symptomtype');
                        Clinical_HPIComplaints.selectedFindings[indexSymptom].DetailComments = detailComments;

                    }

                    for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                        if (Clinical_HPIComplaints.selectedFindings[i].HPITemplateSymptomId == prevTemplateSymptomId) {
                            if (Clinical_HPIComplaints.selectedFindings[i].IsSymptomsDetail == false) {
                                Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_SeverityId = '';
                                Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_LocationIds = '';
                                Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RadiationId = '';
                                Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_QualityId = '';
                                Clinical_HPIComplaints.selectedFindings[i].Onset = '';
                                Clinical_HPIComplaints.selectedFindings[i].AssociatedWith = '';
                                Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_AggravatedById = '';
                                Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RelievedById = '';
                                Clinical_HPIComplaints.selectedFindings[i].IsChecked = false;
                                Clinical_HPIComplaints.selectedFindings[i].DetailComments = '';
                            }
                        }
                    }
                }
                else {

                }
            }

            $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').resetAllControls(null);
            $('#' + Clinical_HPIComplaints.params.PanelID + " ddlLocation option").removeAttr("selected");
            $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation').multiselect('clearSelection');
            $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation').multiselect("refresh");
        }
    },

    getLocationText: function (ddlLocation) {
        var str = '';
        var selectedText = '';
        var selectedValues = ddlLocation.find('option:selected').map(function () {
            return this.text
        });
        if (selectedValues.length > 0) {
            var selectedText = $.makeArray(selectedValues).join();
            selectedText = selectedText.replace(/,/g, ", ");
            str += selectedText.replace(/,([^,]*)$/, ' and $1');
        }
        return str;
    },

    resetFindings: function (HPISymptomId) {
        if (Clinical_HPIComplaints.selectedFindings) {
            for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId) {
                    var findingid = Clinical_HPIComplaints.selectedFindings[i].FindingId;
                    $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingid).remove();

                    Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_SeverityId = '';
                    Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_LocationIds = '';
                    Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RadiationId = '';
                    Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_QualityId = '';
                    Clinical_HPIComplaints.selectedFindings[i].Onset = '';
                    Clinical_HPIComplaints.selectedFindings[i].AssociatedWith = '';
                    Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_AggravatedById = '';
                    Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RelievedById = '';
                    Clinical_HPIComplaints.selectedFindings[i].IsChecked = false;
                }
            }
        }

        for (var i = 0; i < $("#ulHPISymptomFindings li").length; i++) {
            $("#ulHPISymptomFindings #chk" + $("#ulHPISymptomFindings li")[i].id).prop('checked', false);
        }

    },

    CreateHPITemplateHTMLForNotes: function (HPINotesFindings, NotesHPITemplate, bNotSaveCompt) {

        Clinical_HPIComplaints.checkHPIComplaintExists();
        if (HPINotesFindings.length > 0) {
            Clinical_HPIComplaints.Content_style = $("#" + Clinical_HPIComplaints.params.PanelID + " #findingContent").attr('style');
            Clinical_HPIComplaints.createHPIComplaintBodyHTMLForNote(HPINotesFindings, NotesHPITemplate, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, bNotSaveCompt);
        }
    },

    createHPIComplaintBodyHTMLForNote: function (HPINotesFindings, NotesHPITemplate, NoteHTMLCtrl, UnloadHPIComplaint, bNotSaveCompt) {

        Clinical_HPIComplaints.checkComplaintHxExists();
        $('[id^="Cli_HPIComplaints_Temp"]').parent().html('');
        $('[id^="Cli_HPIComplaints_Temp"]').remove();

        if (NotesHPITemplate.length > 0) {
            var $SectionBodyComplaint = $(document.createElement('section'));
            $SectionBodyComplaint.attr('id', "Cli_Complaint_Main" + Clinical_HPIComplaints.ComplaintId);
            $.each(NotesHPITemplate, function (i, item) {
                var HPITemplateId = item.HPITemplateId;
                var $mainDiv = $(document.createElement('section'));
                $mainDiv.attr('HPITemplateId', HPITemplateId);
                $mainDiv.attr('id', "Cli_HPIComplaints_Temp" + HPITemplateId);

                var $ListVital = $(document.createElement('ul'));
                $ListVital.attr('class', 'list-unstyled');
                var HPITemplateId = item.HPITemplateId;

                $($ListVital).append('<li><header class="pl-none">' +
                                     '<Cli_HPIComplaints_' + HPITemplateId + ' title="Complaints"  id="' + HPITemplateId + '">' +
                                     '<a class="btn btn-link btn-xs pl-none" style="color:#bd0e09 !important"  onclick="Clinical_ProgressNote.OpenHPITemplateComplaintAddEdit(' + HPITemplateId + ');" title="Complaints">' + item.TemplateName + '</a> ' +
'<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'HPIComplaints\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ',null,null,' + HPITemplateId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                '</Cli_HPIComplaints_' + HPITemplateId + '> </header></li>');

                $mainDiv.append($ListVital);

                $.each(HPINotesFindings, function (i, item) {
                    if (item.HPITemplateId == HPITemplateId) {
                        var TemplatePreview = "<div>" + item.Desc + "</div>";
                        var $List = $(document.createElement('ul'));
                        $List.attr('class', 'list-unstyled');
                        var HPISymptomId = item.HPISymptomId;
                        $($List).append('<li><a class="btn btn-link btn-xs pl-none" style="color:green !important;margin-right: -5px;" onclick="Clinical_ProgressNote.OpenHPIComplaintSymFindingDetail(this,' + item.HPITemplateSymptomId + ',' + HPISymptomId + ',' + HPITemplateId + ');"'
                                        + 'title="HPI Template Symptom">' + item.SymptomName + '</a>'
                                        + '<br><span id="Cli_HPICompl_HPINFindingsId' + item.HPINotesFindingsId + '" style="' + Clinical_HPIComplaints.Content_style + '">' + item.Desc + '</span   ></li>');
                        $mainDiv.append($List);
                    }
                });

                $mainDiv.append(item.Comments);
                $SectionBodyComplaint.append($mainDiv);
            });

            $(NoteHTMLCtrl + ' Clinical_Complaints').parent().parent().addClass('initialVisitBody  ml-none');
            $(NoteHTMLCtrl + ' Clinical_Complaints').parent().parent().append($SectionBodyComplaint);
            $(NoteHTMLCtrl + ' Clinical_Complaints').parent().parent().find("span#allComments").remove();

            var overallComments = $('#' + Clinical_HPIComplaints.params["PanelID"] + ' #txtOverallComments').val();
            if (overallComments != null) {
                $(NoteHTMLCtrl + ' Clinical_Complaints').parent().parent().append('<span id="allComments">' + overallComments + '</span>');
            }
        }
        Clinical_ProgressNote.saveComponentSOAPText('Complaints', true);
        Clinical_ProgressNote.hoverFunction();
    },

    LoadHPITempSymFindings: function (templateId) {
        var objData = new Object();
        objData["HPITemplateId"] = templateId;
        objData["commandType"] = "Load_HPITemplate_Fill";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },
    applyStyle: function (style) {
        if (style == 'bold') {
            if ($("#findingContent").css('font-weight') != 'bold')
                $("#findingContent").css('font-weight', 'bold');
            else {
                $("#findingContent").css('font-weight', 'normal');
            }

        }
        else if (style == 'italic') {
            if ($("#findingContent").css('font-style') != 'italic')
                $("#findingContent").css('font-style', 'italic');
            else
                $("#findingContent").css('font-style', 'normal');
        }
        else if (style == 'underline') {
            if ($("#findingContent").css('text-decoration') != 'underline')
                $("#findingContent").css('text-decoration', 'underline');
            else
                $("#findingContent").css('text-decoration', 'none');
        }
        else if (style == 'reset') {
            $("#findingContent").css('font-weight', 'normal');
            $("#findingContent").css('font-style', 'normal');
            $("#findingContent").css('text-decoration', 'none');
        }
        else if (style == 'clear') {
            $("#findingContent").css('font-weight', 'normal');
            $("#findingContent").css('font-style', 'normal');
            $("#findingContent").css('text-decoration', 'none');
            $("#findingContent").html('');
        }
        $("#findingContent").trigger("contentchange");
    },
    updateSymptomsDescription: function ($obj) {

        $(Clinical_HPIComplaints.selectedSymptoms).each(function (ind, i) {
            if (i.IsCurrent == true && i.HPITemplateId == Clinical_HPIComplaints.params.HPITemplateId) {
                i.SymptomDescription = $($obj).html();
            }
        });

    },
    getSymptPreview: function (obj, symtomId) {
        var symptomDiv = $(obj).find($('div[id="divSymptom' + symtomId + '"]'))[0];
        var desc = '';
        if (symptomDiv) {
            desc = symptomDiv.outerHTML;
        }
        //$(obj).find($('div[id="divSymptom' + symtomId + '"]')).each(function () {
        //    desc += $(this)[0].outerHTML;
        //});

        return desc;
    },
    bindDetailsChangeEvents: function () {

        $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').on('input', function (e) {
            var controlId = $(e.target).attr('id');

            var HPISymptomId = $("#ulHPISymptoms tr.active").attr('id');
            var HPIFindingId = 0;
            if ($("#ulHPISymptomFindings li.active").length > 0) {
                HPIFindingId = $("#ulHPISymptomFindings li.active").attr('id');
            }

            var delimator = $("#delimator option:selected").text() + " ";

            if (HPIFindingId) {
                if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).length > 0) {
                    var txtToAppend = '';

                    if (controlId == 'ddlSeverity') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').length > 0) {
                            txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                            if ($("#ddlSeverity").val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').append(txtToAppend);

                        }
                    }
                    else if (controlId == 'ddlRadiation') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').length > 0) {
                            txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                            if ($('#ddlRadiation').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'ddlQuality') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').length > 0) {
                            txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            if ($('#ddlQuality').val() == 0) {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'txtOnset') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').length > 0) {
                            txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimator;
                            if ($('#txtOnset').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'txtAssociatedWith') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').length > 0) {
                            txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            if ($('#txtAssociatedWith').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'ddlAggravatedBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').length > 0) {
                            txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            if ($('#ddlAggravatedBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'ddlRevieledBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').length > 0) {
                            txtToAppend = 'Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            if ($('#ddlRevieledBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy');
                            $newDiv.attr("style", "display: inline;");

                            //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').append(txtToAppend);
                        }
                    }
                        //Start 03-10-2017 Edit By Humaira Yousaf IMP-1172
                    else if (controlId == 'txtDetailComments') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').length > 0) {
                            txtToAppend = ' ' + $('#txtDetailComments').val() + delimator;
                            if ($('#txtDetailComments').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments');
                            $newDiv.attr("style", "display: inline;");

                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = ' ' + $('#txtDetailComments').val() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').append(txtToAppend);
                        }
                    }
                    //End 03-10-2017 Edit By Humaira Yousaf IMP-1172
                }
            }
            else {
                if ($("#findingContent #divSymptom" + HPISymptomId).length > 0) {
                    var txtToAppend = '';
                    Clinical_HPIComplaints.resetFindings(HPISymptomId);
                    if (controlId == 'ddlSeverity') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').length > 0) {
                            txtToAppend = ' Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                            if ($("#ddlSeverity").val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlSeverity');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Severity: ' + $('#ddlSeverity option:selected').text() + delimator;;
                            }
                            else {
                                txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                            }

                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').append(txtToAppend);

                        }
                    }
                    else if (controlId == 'ddlRadiation') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').length > 0) {
                            txtToAppend = ' Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                            if ($('#ddlRadiation').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlRadiation');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;

                            }
                            else {
                                txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                            }

                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'ddlQuality') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').length > 0) {
                            txtToAppend = ' Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            if ($('#ddlQuality').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlQuality');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            }
                            else {
                                txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            }

                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'txtOnset') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').length > 0) {
                            txtToAppend = ' Onset: ' + $('#txtOnset').val() + delimator;
                            if ($('#txtOnset').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'txtOnset');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Onset: ' + $('#txtOnset').val() + delimator;
                            }
                            else {
                                txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimator;
                            }
                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'txtAssociatedWith') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').length > 0) {
                            txtToAppend = ' Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            if ($('#txtAssociatedWith').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'txtAssociatedWith');
                            $newDiv.attr("style", "display: inline;");
                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            }
                            else {
                                txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            }

                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'ddlAggravatedBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').length > 0) {
                            txtToAppend = ' Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            if ($('#ddlAggravatedBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlAggravatedBy');
                            $newDiv.attr("style", "display: inline;");
                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            }
                            else {
                                txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            }
                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'ddlRevieledBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').length > 0) {
                            txtToAppend = ' Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            if ($('#ddlRevieledBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlRevieledBy');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            }
                            else {
                                txtToAppend = 'Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            }
                            //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').append(txtToAppend);
                        }
                    }
                        //Start 03-10-2017 Edit By Humaira Yousaf IMP-1172
                    else if (controlId == 'txtDetailComments') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').length > 0) {
                            txtToAppend = '  ' + $('#txtDetailComments').val() + delimator;
                            if ($('#txtDetailComments').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'txtDetailComments');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = '  ' + $('#txtDetailComments').val() + delimator;
                            }
                            else {
                                txtToAppend = ' ' + $('#txtDetailComments').val() + delimator;
                            }
                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').append(txtToAppend);
                        }
                    }
                    //End 03-10-2017 Edit By Humaira Yousaf IMP-1172
                }
            }
            $("#findingContent").trigger("contentchange");
        });


        $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails #ddlLocation').on('change', function () {
            var HPISymptomId = $("#ulHPISymptoms tr.active").attr('id');
            var HPIFindingId = 0;
            if ($("#ulHPISymptomFindings li.active").length > 0) {
                HPIFindingId = $("#ulHPISymptomFindings li.active").attr('id');
            }

            var delimator = $("#delimator option:selected").text() + " ";

            if (HPIFindingId) {
                if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).length > 0) {
                    var txtToAppend = '';
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').length > 0) {
                        txtToAppend = 'Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator; // $('#ddlLocation option:selected').text();
                        if (!$("#ddlLocation").val()) {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation');
                        $newDiv.attr("style", "display: inline;");
                        if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                            txtToAppend = ' Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator; //$('#ddlLocation option:selected').text();
                        }
                        else {
                            txtToAppend = 'Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator; //$('#ddlLocation option:selected').text();
                        }
                        //$("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId, HPIFindingId);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').append(txtToAppend);

                    }
                }
            }
            else {
                if ($("#findingContent #divSymptom" + HPISymptomId).length > 0) {
                    var txtToAppend = '';
                    Clinical_HPIComplaints.resetFindings(HPISymptomId);

                    if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').length > 0) {
                        txtToAppend = ' Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator;
                        if (!$("#ddlLocation").val()) {
                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlLocation');
                        $newDiv.attr("style", "display: inline;");
                        if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                            txtToAppend = ' Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator;
                        }
                        else {
                            txtToAppend = 'Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator;
                        }
                        //$("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                        Clinical_HPIComplaints.AppendControlDiv($newDiv, HPISymptomId);
                        $('#findingContent #divSymptom' + HPISymptomId).show();

                        $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').append(txtToAppend);

                    }
                }
            }
            $("#findingContent").trigger("contentchange");
        });
        // set up the custom event handler
        $("#findingContent").bind("contentchange", function () {
            Clinical_HPIComplaints.updateSymptomsDescription($("#findingContent"));
        });

    },

    AppendControlDiv: function (divToAdd, HPISymptomId, HPIFindingId) {
        if (HPIFindingId) {
            var commentsDiv = $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments');
            if (commentsDiv.length > 0) {
                $(commentsDiv).before(divToAdd);
            }
            else {
                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append(divToAdd);
            }
        }
        else {
            var commentsDiv = $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments');
            if (commentsDiv.length > 0) {
                $(commentsDiv).before(divToAdd);
            }
            else {
                $("#findingContent #divSymptom" + HPISymptomId).append(divToAdd);
            }
        }
    },
    ManageFindings: function (HPISymptomId, obj, HPITemplateSymptomId, event) {

        $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').resetAllControls(null);
        $('#' + Clinical_HPIComplaints.params.PanelID + " ddlLocation option").removeAttr("selected");
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation').multiselect('clearSelection');
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation').multiselect("refresh");

        //event.stopPropagation();
        var symptomAnswerType = $(obj).attr('symptomtype');
        var IsSelected = $(obj).is(':checked')
        var unselectSymptom = false;
        Clinical_HPIComplaints.FillFindingContent(HPITemplateSymptomId);
        $(Clinical_HPIComplaints.selectedSymptoms).each(function (ind, i) {
            if (i.HPITemplateSymptomId == HPITemplateSymptomId) {
                i.HPISymptomsAnswersId = symptomAnswerType;
                i.IsSelected = IsSelected;
                i.IsCurrent = true;
            }
        });

        $("#ulHPISymptoms tr").removeClass('active');
        $("#ulHPISymptoms tr[id=" + HPISymptomId + "]").addClass('active');

        //$('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #hpiDetails').removeClass('disableAll');

        var symptomAnswerType = $(obj).attr('symptomtype');

        if (!$(obj).is(':checked')) {
            $(obj).parent().parent().removeClass('active');

            for (var i = 0; i < $("#ulHPISymptomFindings li").length; i++) {
                $("#ulHPISymptomFindings #chk" + $("#ulHPISymptomFindings li")[i].id).prop('checked', false);
            }
            if (Clinical_HPIComplaints.selectedFindings) {
                for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        Clinical_HPIComplaints.selectedFindings[i].IsSymptomChecked = false;
                        Clinical_HPIComplaints.selectedFindings[i].IsChecked = false;
                        Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                        Clinical_HPIComplaints.selectedFindings[i].HPISymptomsAnswersId = symptomAnswerType;
                    }
                }
            }
        }
        else {
            if (Clinical_HPIComplaints.selectedFindings) {
                for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        Clinical_HPIComplaints.selectedFindings[i].IsSymptomChecked = true;
                        Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                        Clinical_HPIComplaints.selectedFindings[i].HPISymptomsAnswersId = symptomAnswerType;

                    }
                }
            }
        }

        if (symptomAnswerType == "2") { // 'Denies'
            $("#ulHPISymptomFindings").addClass('disableAll');
            //$('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').addClass('disableAll');
            $('#' + Clinical_HPIComplaints.params.PanelID + ' #hpiDetails').addClass('disableAll');

            if (Clinical_HPIComplaints.selectedFindings) {
                for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        Clinical_HPIComplaints.selectedFindings[i].IsSymptomChecked = true;
                        Clinical_HPIComplaints.selectedFindings[i].IsChecked = false;
                        Clinical_HPIComplaints.selectedFindings[i].IsSelected = false;
                        Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                        Clinical_HPIComplaints.selectedFindings[i].HPISymptomsAnswersId = symptomAnswerType;
                    }
                }
            }
            $("#ulHPISymptomFindings input").prop('checked', false);
        }
        else {
            $("#ulHPISymptomFindings").removeClass('disableAll');
            // $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');
            $('#' + Clinical_HPIComplaints.params.PanelID + ' #hpiDetails').removeClass('disableAll');

        }

        $("#SymptomPreview").removeClass('hidden');
        var symptomName = $("#ulHPISymptoms tr[id=" + HPISymptomId + "] label").text();
        var deli = $("#delimator option:selected").text() + " ";

        if ($("#findingContent #divSymptom" + HPISymptomId).length > 0) {
            // $('#findingContent #divSymptom' + HPISymptomId).remove();
            //var $newDiv = $("<div></div>");
            //$newDiv.attr("id", "divSymptom" + HPISymptomId);
            //$newDiv.attr("style", "display: inline;");

            //$("#findingContent").append($newDiv);
            // $('#findingContent #divSymptom' + HPISymptomId).show();
            var txttoAppendType = '';
            var textAdded = $('#findingContent #divSymptom' + HPISymptomId).text();
            if (symptomAnswerType == "1") {
                if (textAdded.toLowerCase().indexOf('c/o') > -1) {
                    unselectSymptom = true;
                    Clinical_HPIComplaints.UnselectSymptom(HPISymptomId);
                    $(obj).prop('checked', false);
                }
                else {
                    txttoAppendType = 'C/O ' + symptomName + ' ';
                    $("#findingContent #divSymptom" + HPISymptomId).attr('style', 'display:inline');
                    var divCacheFinding = $("#divFindingContents #divSymptom" + HPISymptomId);
                    if (divCacheFinding.length > 0 && $(divCacheFinding).html().indexOf('C/O') > -1) {
                        $("#findingContent #divSymptom" + HPISymptomId).html($("#divFindingContents #divSymptom" + HPISymptomId).html());
                    }
                    else {
                        $("#findingContent #divSymptom" + HPISymptomId).text(txttoAppendType);
                    }
                }
            }
            else {
                if (textAdded.toLowerCase().indexOf('denies') > -1) {
                    unselectSymptom = true;
                    Clinical_HPIComplaints.UnselectSymptom(HPISymptomId);
                    $(obj).prop('checked', false);
                }
                else {
                    txttoAppendType = 'Denies: ' + symptomName;
                    if (Clinical_HPIComplaints.selectedFindings) {
                        for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                            if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId) {
                                var findingid = Clinical_HPIComplaints.selectedFindings[i].FindingId;
                                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingid).remove();
                            }
                        }
                    }
                    $("#findingContent #divSymptom" + HPISymptomId).attr('style', '');
                    $("#findingContent #divSymptom" + HPISymptomId).text(txttoAppendType);

                }
            }

            //if ($('#findingContent div').length > 1 && symptomAnswerType == "1")
            //    txttoAppendType = deli + txttoAppendType;

            //txttoAppendType = deli + txttoAppendType;
            //if($('<div>').append($('*', '#div')).html() != "")
            //$('#findingContent').html($('<div>').append($('*', '#div')).html());

            $('#findingContent').html($('#findingContent').children());
            $('#findingContent').find('*:not(div[id^="divSymptom' + HPISymptomId + '"])').remove();


            // $("#findingContent #divSymptom" + HPISymptomId).append(txttoAppendType);
        }
        else {
            var $newDiv = $("<div></div>");
            $newDiv.attr("id", "divSymptom" + HPISymptomId);
            $newDiv.attr("style", "display: inline;");

            $("#findingContent").append('<div></div>');

            $("#findingContent").append($newDiv);
            $('#findingContent #divSymptom' + HPISymptomId).show();
            var txttoAppendType = '';
            if (symptomAnswerType == "1") {
                txttoAppendType = 'C/O ' + symptomName + ' ';
            }
            else {
                txttoAppendType = 'Denies: ' + symptomName;
                if (Clinical_HPIComplaints.selectedFindings) {
                    for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                        if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId) {
                            var findingid = Clinical_HPIComplaints.selectedFindings[i].FindingId;
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingid).remove();
                        }
                    }
                }
                $('#findingContent #divSymptom' + HPISymptomId).removeAttr('style');
            }

            //if ($('#findingContent div').length > 1 && symptomAnswerType == "1")
            //    txttoAppendType = deli + txttoAppendType;

            //txttoAppendType = deli + txttoAppendType;

            $('#findingContent').html($('#findingContent').children());
            $('#findingContent').find('*:not("#divSymptom' + HPISymptomId + '")').remove();
            $("#findingContent #divSymptom" + HPISymptomId).append(txttoAppendType);


        }
        if (symptomAnswerType == "1") {
            // Clinical_HPIComplaints.addFindingDetailInPreview(HPISymptomId);
        }

        if (!unselectSymptom) {
            for (var i = 0; i < Clinical_HPIComplaints.selectedSymptoms.length; i++) {
                if (Clinical_HPIComplaints.selectedSymptoms[i].HPITemplateSymptomId == HPITemplateSymptomId) {
                    if (symptomAnswerType == "1") {
                        Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomsAnswersId = "1";
                    }
                    else if (symptomAnswerType == "2") {
                        Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomsAnswersId = "2";
                    }
                }
            }
        }

        $("#findingContent").trigger('contentchange');
    },
    UnselectSymptom: function (HPISymptomId) {
        $("#ulHPISymptomFindings").addClass('disableAll');
        $('#' + Clinical_HPIComplaints.params.PanelID + ' #sectionFindingDetails').addClass('disableAll');

        if (Clinical_HPIComplaints.selectedFindings) {
            for (var i = 0 ; i < Clinical_HPIComplaints.selectedFindings.length; i++) {
                if (Clinical_HPIComplaints.selectedFindings[i].HPISymptomId == HPISymptomId) {
                    Clinical_HPIComplaints.selectedFindings[i].IsSymptomChecked = false;
                    Clinical_HPIComplaints.selectedFindings[i].IsChecked = false;
                    Clinical_HPIComplaints.selectedFindings[i].SymptomDescription = '';
                    Clinical_HPIComplaints.selectedFindings[i].HPISymptomsAnswersId = null;
                }
            }
        }

        for (var i = 0 ; i < Clinical_HPIComplaints.selectedSymptoms.length; i++) {
            if (Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomId == HPISymptomId) {
                Clinical_HPIComplaints.selectedSymptoms[i].SymptomDescription = '';
                Clinical_HPIComplaints.selectedSymptoms[i].HPISymptomsAnswersId = null;
                Clinical_HPIComplaints.selectedSymptoms[i].IsSelected = false;

            }
        }

        $("#findingContent #divSymptom" + HPISymptomId).remove();
        $("#divFindingContents #divSymptom" + HPISymptomId).remove();
    },
    removeLastDelimiter: function (HPISymptomId) {

        var delii = $("#delimator option:selected").text();
        var str = "";
        if (delii == ",") str = $($('#findingContent div[id^=divSymptom' + HPISymptomId + ']')[0]).text().replace(/,/g, "");
        if (delii == ".") str = $($('#findingContent div[id^=divSymptom' + HPISymptomId + ']')[0]).text().replace(/./g, "");
        if (delii == ":") str = $($('#findingContent div[id^=divSymptom' + HPISymptomId + ']')[0]).text().replace(/:/g, "");
        if (delii == ";") str = $($('#findingContent div[id^=divSymptom' + HPISymptomId + ']')[0]).text().replace(/;/g, "");
        if (delii == "-") str = $($('#findingContent div[id^=divSymptom' + HPISymptomId + ']')[0]).text().replace(/-/g, "");

        var id = $($('#findingContent div')[0]).attr('id');
        //  $("#findingContent #" + id).text(str);
    },
    getSymtopInnerContent: function (obj, symtomId) {
        var desc = "";
        if ($(obj).find('#divSymptom' + symtomId).length > 0) {
            $(obj).find('#divSymptom' + symtomId).find('div').each(function () {
                desc += $(this).html();
            });
        }
        else {
            desc = $(obj).text();
        }
        return desc;
    },
    deleteItem: function (obj, ctrlId, currentId) {

        var itemId = $(obj).closest("li").attr('id');

        if (ctrlId == "ulHPISymptoms") {
            $("#" + currentId).remove();

        } else if (ctrlId == "ulHPISymptomFindings") {
            $("#" + currentId).remove();
        }
    },

    addFindingDetailInPreview: function (HPISymptomId, HPIFindingId) {
        var delimator = $("#delimator option:selected").text() + " ";

        var i = -1;
        $.grep(Clinical_HPIComplaints.selectedFindings, function (item, index) {
            if (item.HPISymptomId == HPISymptomId && item.FindingId == HPIFindingId) {
                i = index;
                return;
            }
        });
        if (i != -1) {
            if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).length > 0) {
                var txtToAppend = '';

                if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_SeverityId != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_SeverityId != "") {
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').length > 0) {
                        txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                        if ($("#ddlSeverity").val() == '0') {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity');
                        $newDiv.attr("style", "display: inline;");

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').append(txtToAppend);

                    }
                }
                if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RadiationId != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RadiationId != "") {
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').length > 0) {
                        txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                        if ($('#ddlRadiation').val() == '0') {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation');
                        $newDiv.attr("style", "display: inline;");

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').append(txtToAppend);
                    }
                }
                if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_QualityId != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_QualityId != "") {
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').length > 0) {
                        txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                        if ($('#ddlQuality').val() == 0) {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality');
                        $newDiv.attr("style", "display: inline;");

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').append(txtToAppend);
                    }
                }
                if (Clinical_HPIComplaints.selectedFindings[i].Onset != "") {
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').length > 0) {
                        txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimator;
                        if ($('#txtOnset').val() == '') {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset');
                        $newDiv.attr("style", "display: inline;");

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimator;
                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').append(txtToAppend);
                    }
                }
                if (Clinical_HPIComplaints.selectedFindings[i].AssociatedWith != "") {
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').length > 0) {
                        txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                        if ($('#txtAssociatedWith').val() == '') {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith');
                        $newDiv.attr("style", "display: inline;");

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').append(txtToAppend);
                    }
                }
                if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_AggravatedById != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_AggravatedById != "") {
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').length > 0) {
                        txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                        if ($('#ddlAggravatedBy').val() == '0') {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy');
                        $newDiv.attr("style", "display: inline;");

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').append(txtToAppend);
                    }
                }
                if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RelievedById != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RelievedById != "") {
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').length > 0) {
                        txtToAppend = 'Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                        if ($('#ddlRevieledBy').val() == '0') {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy');
                        $newDiv.attr("style", "display: inline;");

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        txtToAppend = 'Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').append(txtToAppend);
                    }
                }
                if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_LocationIds != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_LocationIds != "") {
                    var txtToAppend = '';
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').length > 0) {
                        txtToAppend = 'Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator; // $('#ddlLocation option:selected').text();
                        if (!$("#ddlLocation").val()) {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation');
                        $newDiv.attr("style", "display: inline;");
                        if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                            txtToAppend = ' Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator; //$('#ddlLocation option:selected').text();
                        }
                        else {
                            txtToAppend = 'Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator; //$('#ddlLocation option:selected').text();
                        }
                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').append(txtToAppend);

                    }
                }
                //Start 03-10-2017 Edit By Humaira Yousaf IMP-1172
                if (Clinical_HPIComplaints.selectedFindings[i].DetailComments != "") {
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').length > 0) {
                        txtToAppend = ' ' + $('#txtDetailComments').val() + delimator;
                        if ($('#txtDetailComments').val() == '') {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments');
                        $newDiv.attr("style", "display: inline;");

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        txtToAppend = ' ' + $('#txtDetailComments').val() + delimator;
                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtDetailComments').append(txtToAppend);
                    }
                }
                //End 03-10-2017 Edit By Humaira Yousaf IMP-1172
            }
        }
        else {
            var i = -1;
            $.grep(Clinical_HPIComplaints.selectedFindings, function (item, index) {
                if (item.HPISymptomId == HPISymptomId) {
                    i = index;
                    return;
                }
            });

            if (i != -1) {
                if ($("#findingContent #divSymptom" + HPISymptomId).length > 0) {
                    var txtToAppend = '';
                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_SeverityId != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_SeverityId != "") {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').length > 0) {
                            txtToAppend = ' Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                            if ($("#ddlSeverity").val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlSeverity');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Severity: ' + $('#ddlSeverity option:selected').text() + delimator;;
                            }
                            else {
                                txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                            }


                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);

                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').append(txtToAppend);

                        }
                    }
                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RadiationId != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RadiationId != "") {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').length > 0) {
                            txtToAppend = ' Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                            if ($('#ddlRadiation').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlRadiation');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;

                            }
                            else {
                                txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                            }

                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').append(txtToAppend);
                        }
                    }

                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_QualityId != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_QualityId != "") {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').length > 0) {
                            txtToAppend = ' Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            if ($('#ddlQuality').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlQuality');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            }
                            else {
                                txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            }

                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').append(txtToAppend);
                        }
                    }

                    if (Clinical_HPIComplaints.selectedFindings[i].Onset != "") {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').length > 0) {
                            txtToAppend = ' Onset: ' + $('#txtOnset').val() + delimator;
                            if ($('#txtOnset').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'txtOnset');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Onset: ' + $('#txtOnset').val() + delimator;
                            }
                            else {
                                txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimator;
                            }
                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').append(txtToAppend);
                        }
                    }
                    if (Clinical_HPIComplaints.selectedFindings[i].AssociatedWith != "") {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').length > 0) {
                            txtToAppend = ' Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            if ($('#txtAssociatedWith').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'txtAssociatedWith');
                            $newDiv.attr("style", "display: inline;");
                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            }
                            else {
                                txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            }

                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').append(txtToAppend);
                        }
                    }

                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_AggravatedById != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_AggravatedById != "") {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').length > 0) {
                            txtToAppend = ' Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            if ($('#ddlAggravatedBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlAggravatedBy');
                            $newDiv.attr("style", "display: inline;");
                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            }
                            else {
                                txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            }
                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').append(txtToAppend);
                        }
                    }

                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RelievedById != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_RelievedById != "") {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').length > 0) {
                            txtToAppend = ' Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            if ($('#ddlRevieledBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlRevieledBy');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            }
                            else {
                                txtToAppend = 'Relieved By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            }
                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').append(txtToAppend);
                        }
                    }

                    if (Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_LocationIds != "0" && Clinical_HPIComplaints.selectedFindings[i].HPISymptoms_LocationIds != "") {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').length > 0) {
                            txtToAppend = ' Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator;
                            if (!$("#ddlLocation").val()) {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlLocation');
                            $newDiv.attr("style", "display: inline;");
                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator;
                            }
                            else {
                                txtToAppend = 'Location: ' + Clinical_HPIComplaints.getLocationText($('#' + Clinical_HPIComplaints.params.PanelID + ' #ddlLocation')) + delimator;
                            }
                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').append(txtToAppend);

                        }
                    }
                    //Start 03-10-2017 Edit By Humaira Yousaf IMP-1172
                    if (Clinical_HPIComplaints.selectedFindings[i].DetailComments != "") {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').length > 0) {
                            txtToAppend = '  ' + $('#txtDetailComments').val() + delimator;
                            if ($('#txtDetailComments').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'txtDetailComments');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = '  ' + $('#txtDetailComments').val() + delimator;
                            }
                            else {
                                txtToAppend = ' ' + $('#txtDetailComments').val() + delimator;
                            }
                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'txtDetailComments').append(txtToAppend);
                        }
                    }
                    //End 03-10-2017 Edit By Humaira Yousaf IMP-1172
                }
            }
        }

        $("#findingContent").trigger("contentchange");

        // set up the custom event handler
        $("#findingContent").bind("contentchange", function () {
            Clinical_HPIComplaints.updateSymptomsDescription($("#findingContent"));
        });
    },
}