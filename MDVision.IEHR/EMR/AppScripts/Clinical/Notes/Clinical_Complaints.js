// Created By:  Muhammad Ahmad Imran
// Created Date: 10/02/2016
Clinical_Complaints = {
    bIsFirstLoad: true,
    params: [],
    Complaints: [],
    ComplaintsDetail: [],
    ComplaintsDetailForDx: [],
    ComplaintSelectedPrevious: 0,
    ComplaintSelectedCurrent: 0,
    FavComplaintsId: -1000,
    ComplaintId: 0,
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


    Load: function (params) {

        Clinical_Complaints.Complaints = [];
        Clinical_Complaints.ComplaintsDetail = [];
        Clinical_Complaints.ComplaintSelectedPrevious = 0;
        Clinical_Complaints.ComplaintSelectedCurrent = 0;
        Clinical_Complaints.IsDelete = false;
        Clinical_Complaints.FavComplaintsId = -1000;
        Clinical_Complaints.LastSctBaseSearch = '';
        Clinical_Complaints.ComplaintId = 0;



        Clinical_Complaints.params = params;

        Clinical_Complaints.params.mode = "Add";
        Clinical_Complaints.pComplaintIndex = 0;
        if (Clinical_Complaints.params.PanelID != 'pnlClinicalComplaints') {
            Clinical_Complaints.params.PanelID = Clinical_Complaints.params.PanelID + ' #pnlClinicalComplaints';
        } else {
            Clinical_Complaints.params.PanelID = 'pnlClinicalComplaints';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_Complaints.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        utility.CreateDatePicker(Clinical_Complaints.params.PanelID + '  #dtComplaintDate', function () {
        }, true);
        Clinical_Complaints.domReadyFunc();

        if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote") {
            // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_Complaints.params.PanelID, 'Medical', 'Complaints', 'Clinical_Complaints.unLoadTab(true);', null, true);
            // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491

        }
        $('#' + Clinical_Complaints.params.PanelID + " #txtComplaintId").val(0);
        var self = $('#' + Clinical_Complaints.params.PanelID);
        var data = "IsActive=1"
        if (Clinical_Complaints.params.CurrentNotesProviderId && Clinical_Complaints.params.CurrentNotesProviderId != "undefined")
            data = data + "&StrID=" + Clinical_Complaints.params.CurrentNotesProviderId;
        self.loadDropDowns(true).done(function () {
            self.find('#ddlFavComplaint').attr('ddlist', 'GetFavComplaint');
            self.find('#favSectionDiv').loadDropDowns(true, data).done(function () {
                EMRUtility.setFavoriteSectionStyle(Clinical_Complaints.params.PanelID);
                //Start 2-07-2016 M Ahmad Imran for favorite list setting for all favLists
                if (EMRUtility.getFavListStatus(Clinical_Complaints.FavListName)) {
                    $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #favSectionDiv").addClass("toggledHor");
                    $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #FormDiv").addClass("toggleHorContainer");
                }
                else {
                    $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #favSectionDiv").removeClass("toggledHor");
                    $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #FormDiv").removeClass("toggleHorContainer");
                }

                //End 2-07-2016 M Ahmad Imran for favorite list setting for all favLists
                Clinical_Complaints.SetFavListVal($('#' + Clinical_Complaints.params.PanelID + ' #ddlFavComplaint'));
            });

            Clinical_Complaints.loadCiefComplaintComponent(null);
            Clinical_Complaints.IntializeMultiSelectDropDown();
        });

        if (Clinical_Complaints.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_Complaints.params.PanelID + " div#FaceSheetPager", Clinical_Complaints.params.FaceSheetComponents, 'Complaints');
        }

        if (EMRUtility.getFreeTextStatus("Clinical_Complaints")) {
            var panel = "#pnlClinicalComplaints";
            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalComplaints";
            }

            $(panel + " #txtComplaints").addClass('hidden');
            $(panel + " #btnSearchCPT").addClass('hidden');
            if ($(panel + " #txtFreeText").hasClass("hidden")) {
                $(panel + " #txtFreeText").removeClass("hidden");
            }
            $(panel + " #radioFreetext").attr('checked', 'checked');
        }
        else {
            var panel = "#pnlClinicalComplaints";
            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalComplaints";
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

        var FavOptionLength = $("#" + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #ddlFavComplaint option").length;
        if (FavOptionLength > 1) {
            EMRUtility.getFavListValue(Clinical_Complaints.FavListName).done(function (response1) {
                response1 = JSON.parse(response1);
                if (response1.status != false) {
                    if (response1.favListVal != "") {
                        if ($("#" + Clinical_Complaints.params.PanelID + " #ddlFavComplaint option[value='" + response1.favListVal + "']").length > 0) {
                            $ddl.val(response1.favListVal);
                            $ddl.trigger("onchange");
                        }
                        else {
                            if (FavOptionLength == 2) {
                                $ddl.val($("#" + Clinical_Complaints.params.PanelID + " #ddlFavComplaint option:nth-child(2)").val());
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
                            $ddl.val($("#" + Clinical_Complaints.params.PanelID + " #ddlFavComplaint option:nth-child(2)").val());
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
            $('#' + Clinical_Complaints.params.PanelID + ' #frmClinicalComplaints #txtComment').on("keyup", function (e) {

                if (e.keyCode == 190 || e.keyCode == 110) // dot key is pressed
                {
                    e.preventDefault();
                    EMRUtility.pasteHtmlAtCaret('<span id=marker></span>');
                    if (EMRUtility.callAutopopulationOrNot(Clinical_Complaints.params.PanelID, "txtComment")) {
                        $('#' + Clinical_Complaints.params.PanelID + ' #frmClinicalComplaints  #txtComment').focus();
                        EMRUtility.AutoKeyWordPopulateForDiv(Clinical_Complaints.params.PanelID, "txtComment", "Complaints", 0);
                    }
                    else {
                        $('#' + Clinical_Complaints.params.PanelID + ' #frmClinicalComplaints  #txtComment').find("#marker").remove();
                    }
                }

            });
        });

        $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails').on('keydown', '#txtDuration', function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });

        $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails').on('input', function () {
            Clinical_Complaints.IsUpdate = true;
        });
        $('#' + Clinical_Complaints.params.PanelID + ' #chkChiefComplaints').on('change', function () {
            Clinical_Complaints.IsUpdate = true;
        });
        $('#' + Clinical_Complaints.params.PanelID + ' #ddlLocation').on('change', function () {
            Clinical_Complaints.IsUpdate = true;
        });
        $('#' + Clinical_Complaints.params.PanelID + ' #ddlCharacter').on('change', function () {
            Clinical_Complaints.IsUpdate = true;
        });
        $('#' + Clinical_Complaints.params.PanelID).on('click', function (event) {

            Clinical_Complaints.WhichIdIsClick = event.target.id;
        });


    },
    //Start//17/02/2016//M Ahmad Imran//Implimented Duration Validation
    ChangeClickId: function (id) {
        Clinical_Complaints.WhichIdIsClick = id;
    },
    CheckDurationSelect: function () {
        Clinical_Complaints.WhichIdIsClick = "ddlDuration"
        if ($('#' + Clinical_Complaints.params.PanelID + ' #ddlDuration').val() != "0") {
            if ($('#' + Clinical_Complaints.params.PanelID + ' #txtDuration').val() == "") {
                if (Clinical_Complaints.WhichIdIsClick != "txtDuration") {
                    Clinical_Complaints.IsDurationValidation = false;
                    Clinical_Complaints.ValidationErrorMessage = "Enter Value In Duration";
                    utility.DisplayMessages("Enter Value In Duration", 3);
                    $('#' + Clinical_Complaints.params.PanelID + ' #txtDuration').focus();
                }
                else {
                    Clinical_Complaints.IsDurationValidation = true;
                    Clinical_Complaints.ValidationErrorMessage = "";
                }
            }
            else {
                Clinical_Complaints.IsDurationValidation = true;
                Clinical_Complaints.ValidationErrorMessage = "";
            }
        }
        else {
            Clinical_Complaints.IsDurationValidation = true;
            Clinical_Complaints.ValidationErrorMessage = "";
        }


    },
    CheckDuration: function () {
        setTimeout(function () {
            if ($('#' + Clinical_Complaints.params.PanelID + ' #ddlDuration').val() != "0") {
                if ($('#' + Clinical_Complaints.params.PanelID + ' #txtDuration').val() == "") {
                    if (Clinical_Complaints.WhichIdIsClick != "txtDuration" && Clinical_Complaints.WhichIdIsClick != "ddlDuration") {
                        $('#' + Clinical_Complaints.params.PanelID + ' #txtDuration').focus();
                        Clinical_Complaints.IsDurationValidation = false;
                        Clinical_Complaints.ValidationErrorMessage = "Enter Value In Duration";
                        utility.DisplayMessages("Enter Value In Duration", 3);
                    }
                    else {
                        Clinical_Complaints.IsDurationValidation = true;
                        Clinical_Complaints.ValidationErrorMessage = "";
                    }
                }
                else {
                    Clinical_Complaints.IsDurationValidation = true;
                    Clinical_Complaints.ValidationErrorMessage = "";
                }
            }
            else {
                if ($('#' + Clinical_Complaints.params.PanelID + ' #txtDuration').val() != "") {
                    //$('#' + Clinical_Complaints.params.PanelID + ' #txtDuration').addEventListener('click', $('#' + Clinical_Complaints.params.PanelID + ' #ddlDuration').focus(), false);
                    if (Clinical_Complaints.WhichIdIsClick != "ddlDuration") {
                        //if (Clinical_Complaints.WhichIdIsClick != "txtDuration") {
                        $('#' + Clinical_Complaints.params.PanelID + ' #ddlDuration').focus();
                        Clinical_Complaints.clickOnDuration = false;
                        Clinical_Complaints.IsDurationValidation = false;
                        Clinical_Complaints.ValidationErrorMessage = "Select Duration";
                        utility.DisplayMessages("Select Duration", 3);
                    }
                    else {
                        Clinical_Complaints.IsDurationValidation = true;
                        Clinical_Complaints.ValidationErrorMessage = "";
                    }


                }
                else {
                    Clinical_Complaints.IsDurationValidation = true;
                    Clinical_Complaints.ValidationErrorMessage = "";
                }

            }
        }, 500);
    },
    CheckDurationSelectFocusOut: function () {
        setTimeout(function () {
            if ($('#' + Clinical_Complaints.params.PanelID + ' #ddlDuration').val() != "0" && !($('#' + Clinical_Complaints.params.PanelID + ' #txtDuration').is(':focus'))) {
                if ($('#' + Clinical_Complaints.params.PanelID + ' #txtDuration').val() == "") {
                    if (Clinical_Complaints.WhichIdIsClick != "txtDuration") {
                        $('#' + Clinical_Complaints.params.PanelID + ' #txtDuration').focus();
                        Clinical_Complaints.IsDurationValidation = false;
                        Clinical_Complaints.ValidationErrorMessage = "Enter Value In Duration";
                        utility.DisplayMessages("Enter Value In Duration", 3);
                    }
                    else {
                        Clinical_Complaints.IsDurationValidation = true;
                        Clinical_Complaints.ValidationErrorMessage = "";
                    }
                }
                else {
                    Clinical_Complaints.IsDurationValidation = true;
                    Clinical_Complaints.ValidationErrorMessage = "";
                }
            }
            else {
                if ($('#' + Clinical_Complaints.params.PanelID + ' #txtDuration').val() != "") {
                    //$('#' + Clinical_Complaints.params.PanelID + ' #txtDuration').addEventListener('click', $('#' + Clinical_Complaints.params.PanelID + ' #ddlDuration').focus(), false);
                    //if (Clinical_Complaints.WhichIdIsClick != "ddlDuration") {
                    if (Clinical_Complaints.WhichIdIsClick != "txtDuration") {
                        $('#' + Clinical_Complaints.params.PanelID + ' #ddlDuration').focus();
                        Clinical_Complaints.clickOnDuration = false;
                        Clinical_Complaints.IsDurationValidation = false;
                        Clinical_Complaints.ValidationErrorMessage = "Select Duration";
                        utility.DisplayMessages("Select Duration", 3);
                    }
                    else {
                        Clinical_Complaints.IsDurationValidation = true;
                        Clinical_Complaints.ValidationErrorMessage = "";
                    }

                }
                else {
                    Clinical_Complaints.IsDurationValidation = true;
                    Clinical_Complaints.ValidationErrorMessage = "";
                }

            }
        }, 500);
    },
    //End M Ahmad Imran 17/02/2016
    IntializeMultiSelectDropDown: function () {


        $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails #ddlLocation').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247

        });
        $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails #ddlCharacter').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 116

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

        $('#pnlClinicalComplaints #txtComplaints').attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";
        //params["ParentCtrl"] = Clinical_Complaints.params["TabID"];
        if (Clinical_Complaints.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Clinical_Complaints';
        }

        else {
            params["ParentCtrl"] = Clinical_Complaints.params["TabID"];
        }


        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Clinical_Complaints.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }
    },
    BindICD9AutoComplete: function (element) {
        if ($(element).val().length > 3) {
            if ($(element).val().substring(0, 3).toLowerCase() == "sct") {
                Clinical_Complaints.LastSctBaseSearch = $(element).val().substring(3, $(element).val().length);
            }
            else {
                Clinical_Complaints.LastSctBaseSearch = "";
            }
        }
        else {
            Clinical_Complaints.LastSctBaseSearch = "";
        }
        var descriptionCrtl = $(element);
        $('#pnlClinicalComplaints #txtComplaints').attr("data-popupunload", "false");
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_Complaints", null, false);

    },
    fillChiefComplaints: function (obj, ev) {

        if (!Clinical_Complaints.IsDurationValidation) {
            //utility.DisplayMessages(Clinical_Complaints.ValidationErrorMessage, 3);
        }
        else {
            Clinical_Complaints.IsDurationValidation = true;
            Clinical_Complaints.ValidationErrorMessage = "";
            if (ev != null) {
                ev.stopPropagation();
            }

            $("#" + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints").bootstrapValidator('resetForm', true);
            var diseaseId = $(obj).attr('id');

            //$('#' + Clinical_Complaints.params.PanelID + ' #frmClinicalComplaints #btnMedicalHxSave').hide();
            $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #sectionComplaintDetails").removeClass('disableAll');

            $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints ul#ulCompliantDisease li").each(function (i, item) {
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

            //Clinical_Complaints.loadCiefComplaintComponent();

            Clinical_Complaints.SaveComplaintDetailInJsonArray();



            //alert('called');
        }

    },
    //Start//12/02/2016//M Ahmad Imran//Implimented Save Complaint Detail info in json array and bind it
    SaveComplaintDetailInJsonArray: function () {

        var SelectedComplaintId = $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li.active").attr("id") != "" ? $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li.active").attr("id") : 0;
        var NotFoundForPre = true;
        var NotFoundForCur = true;
        if (Clinical_Complaints.ComplaintSelectedPrevious == 0) {
            Clinical_Complaints.ComplaintSelectedPrevious = SelectedComplaintId;
            Clinical_Complaints.ComplaintSelectedCurrent = SelectedComplaintId;


            $.grep(Clinical_Complaints.ComplaintsDetail, function (item, index) {
                if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedPrevious) {
                    var self = $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails');
                    utility.bindMyJSONByName(true, item, false, self).done(function () {
                        if (item.Complaint_LocationIds != '') {
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlLocation").val(item.Complaint_LocationIds.split(','));
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlLocation").multiselect("refresh");
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlLocation").multiselect({
                                includeSelectAllOption: true,
                                enableFiltering: true,
                                enableCaseInsensitiveFiltering: true,
                                maxHeight: 247
                            });
                        }
                        if (item.Complaint_CharacterIds != '') {
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlCharacter").val(item.Complaint_CharacterIds.split(','));
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlCharacter").multiselect("refresh");
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlCharacter").multiselect({
                                includeSelectAllOption: true,
                                enableFiltering: true,
                                enableCaseInsensitiveFiltering: true,
                                maxHeight: 116
                            });
                        }
                        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtComment").html(item.Comments);
                    });
                    NotFoundForPre = false;
                    return;
                }
            });


            if (NotFoundForPre) {

                var self = $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails');
                var myJSON = self != null ? self.getMyJSONByName() : "{}";
                var objDetail = JSON.parse(myJSON);
                objDetail["ComplaintDetailId"] = Clinical_Complaints.ComplaintSelectedCurrent;
                $.grep(Clinical_Complaints.Complaints, function (item, index) {
                    if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedCurrent) {
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

                var CharacterIDS = self.find('#ddlCharacter option:Selected').map(function () {
                    return this.value;
                }).get().join(',');
                objDetail["Complaint_CharacterIds"] = CharacterIDS;

                var CharactersText = self.find('#ddlCharacter option:Selected').map(function () {
                    return this.text;
                }).get().join(', ');
                objDetail["Complaint_CharactersText"] = CharactersText;

                objDetail["Comments"] = $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtComment").html();

                Clinical_Complaints.ComplaintsDetail.push(objDetail);
                Clinical_Complaints.resetValues();
            }
            //else {
            //    $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails').data('serialize', $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails').serialize());
            //}




        }
        else {
            Clinical_Complaints.ComplaintSelectedPrevious = Clinical_Complaints.ComplaintSelectedCurrent;
            Clinical_Complaints.ComplaintSelectedCurrent = SelectedComplaintId;

            //Previous Complaint Work
            if (Clinical_Complaints.ComplaintSelectedPrevious != Clinical_Complaints.ComplaintSelectedCurrent) {

                $.grep(Clinical_Complaints.ComplaintsDetail, function (item, index) {
                    if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedPrevious) {
                        NotFoundForPre = false;
                        return;
                    }
                });


                if (NotFoundForPre) {
                    var self = $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails');
                    var myJSON = self != null ? self.getMyJSONByName() : "{}";
                    var objDetail = JSON.parse(myJSON);
                    objDetail["ComplaintDetailId"] = Clinical_Complaints.ComplaintSelectedPrevious;
                    $.grep(Clinical_Complaints.Complaints, function (item, index) {
                        if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedPrevious) {
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

                    var CharacterIDS = self.find('#ddlCharacter option:Selected').map(function () {
                        return this.value;
                    }).get().join(',');
                    objDetail["Complaint_CharacterIds"] = CharacterIDS;

                    var CharactersText = self.find('#ddlCharacter option:Selected').map(function () {
                        return this.text;
                    }).get().join(', ');
                    objDetail["Complaint_CharactersText"] = CharactersText;

                    objDetail["Comments"] = $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtComment").html();

                    Clinical_Complaints.ComplaintsDetail.push(objDetail);
                    Clinical_Complaints.resetValues();
                }
                else {


                    var ComplaintsDetail_Copy = [];
                    $.grep(Clinical_Complaints.ComplaintsDetail, function (item, index) {
                        if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedPrevious) {
                            var self = $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails');
                            var myJSON = self != null ? self.getMyJSONByName() : "{}";
                            var objDetail = JSON.parse(myJSON);
                            objDetail["ComplaintDetailId"] = Clinical_Complaints.ComplaintSelectedPrevious;
                            $.grep(Clinical_Complaints.Complaints, function (item, index) {
                                if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedPrevious) {
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

                            var CharacterIDS = self.find('#ddlCharacter option:Selected').map(function () {
                                return this.value;
                            }).get().join(',');
                            objDetail["Complaint_CharacterIds"] = CharacterIDS;

                            var CharactersText = self.find('#ddlCharacter option:Selected').map(function () {
                                return this.text;
                            }).get().join(', ');
                            objDetail["Complaint_CharactersText"] = CharactersText;

                            objDetail["IsUpdate"] = Clinical_Complaints.IsUpdate;
                            objDetail["Comments"] = $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtComment").html();
                            ComplaintsDetail_Copy.push(objDetail);
                        }
                        else {
                            ComplaintsDetail_Copy.push(item);
                        }
                    });
                    Clinical_Complaints.ComplaintsDetail = ComplaintsDetail_Copy;
                    Clinical_Complaints.resetValues();
                }
            }

            //Current Complaint Work
            $.grep(Clinical_Complaints.ComplaintsDetail, function (item, index) {
                if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedCurrent) {
                    var self = $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails');
                    NotFoundForCur = false;
                    utility.bindMyJSONByName(true, item, false, self).done(function () {
                        if (item.Complaint_LocationIds != '') {
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlLocation").val(item.Complaint_LocationIds.split(','));
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlLocation").multiselect("refresh");
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlLocation").multiselect({
                                includeSelectAllOption: true,
                                enableFiltering: true,
                                enableCaseInsensitiveFiltering: true,
                                maxHeight: 247
                            });
                        }
                        if (item.Complaint_CharacterIDS != '') {
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlCharacter").val(item.Complaint_CharacterIds.split(','));
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlCharacter").multiselect("refresh");
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlCharacter").multiselect({
                                includeSelectAllOption: true,
                                enableFiltering: true,
                                enableCaseInsensitiveFiltering: true,
                                maxHeight: 116
                            });
                        }
                        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtComment").html(item.Comments);
                    });
                    return;
                }
            });
            if (NotFoundForCur) {
                var self = $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails');
                var myJSON = self != null ? self.getMyJSONByName() : "{}";
                var objDetail = JSON.parse(myJSON);
                objDetail["ComplaintDetailId"] = Clinical_Complaints.ComplaintSelectedCurrent;
                $.grep(Clinical_Complaints.Complaints, function (item, index) {
                    if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedCurrent) {
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

                var CharacterIDS = self.find('#ddlCharacter option:Selected').map(function () {
                    return this.value;
                }).get().join(',');
                objDetail["Complaint_CharacterIds"] = CharacterIDS;

                var CharactersText = self.find('#ddlCharacter option:Selected').map(function () {
                    return this.text;
                }).get().join(', ');
                objDetail["Complaint_CharactersText"] = CharactersText;

                objDetail["Comments"] = $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtComment").html();
                Clinical_Complaints.ComplaintsDetail.push(objDetail);
                Clinical_Complaints.resetValues();
            }
            //else {
            //    $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails').data('serialize', $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails').serialize());
            //}
        }
        Clinical_Complaints.IsUpdate = false;
    },
    //End M Ahmad Imran 12/02/2016

    //Start//12/02/2016//M Ahmad Imran//Implimented Load Complaint Detail
    loadCiefComplaintComponent: function (CallBack) {
        Clinical_Complaints.IsDurationValidation = true;
        Clinical_Complaints.ValidationErrorMessage = "";
        Clinical_Complaints.IsUpdate = false;
        Clinical_Complaints.Complaints = [];
        Clinical_Complaints.ComplaintsDetail = [];
        Clinical_Complaints.ComplaintSelectedPrevious = 0;
        Clinical_Complaints.ComplaintSelectedCurrent = 0;
        Clinical_Complaints.IsDelete = false;
        Clinical_Complaints.IntializeMultiSelectDropDown();
        Clinical_Complaints.resetValues();
        $("#txtOverallComments").val("");
        $('#' + Clinical_Complaints.params.PanelID + " #txtComplaintId").val(0);
        utility.CreateDatePicker(Clinical_Complaints.params.PanelID + '  #dtComplaintDate', function () {
        }, true);

        $('#pnlClinicalComplaints #ulCompliantDisease').html("");

        Clinical_Complaints.LoadComplaints().done(function (response) {
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
                    if (Clinical_Complaints.ComplaintsDetailForDx.length > 0) {
                        if (i < Clinical_Complaints.ComplaintsDetailForDx.length) {
                            item.ComplaintDetailId ? Clinical_Complaints.ComplaintsDetailForDx[i].ComplaintDetailId = item.ComplaintDetailId : Clinical_Complaints.ComplaintsDetailForDx[i].ComplaintDetailId = "";
                            item.ProblemListId ? Clinical_Complaints.ComplaintsDetailForDx[i]["ProblemListId"] = item.ProblemListId : Clinical_Complaints.ComplaintsDetailForDx[i]["ProblemListId"] = "";
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
                            Clinical_Complaints.ComplaintsDetailForDx.push(complaint);
                        }
                    }

                    var liDescription = item.ComplaintDescription;
                    if (item.ICD10 != "" && liDescription.indexOf(item.ICD10 + " - ") > -1) {
                        liDescription = liDescription;
                    }
                    else {
                        liDescription = (item.ICD10 != "" ? item.ICD10 + " - " : "") + liDescription;
                    }

                    var li = "<li id=" + item.ComplaintDetailId + " onclick='Clinical_Complaints.fillChiefComplaints(this, event);' ComplaintDetailId=\"" + item.ComplaintDetailId + "\"icd9Code=\"" + item.ICD9 + "\" icd9desc=\"" + item.ICD9Description + "\" icd10Code=\"" + item.ICD10 + "\" icd10Desc=\"" + item.ICD10Description + "\" snomedCode=\"" + item.SNOMED + "\" snomedDesc=\"" + item.SNOMEDDescription + "\">" +
	                    "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + liDescription + "</span></a>" +
		                    "<span class='removeIconListHover'>" +
                                "<a href='#' onclick='Clinical_Complaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                    "<a href='#' onclick='Clinical_Complaints.deleteChiefComplaint(" + item.ComplaintDetailId + ", event);'><i class='fa fa-times red'></i></a>" +
		                    "</span>" +
                        "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_Complaints.updateComplaintsText(this, event);'/>" +
                        "<div class='clearfix'></div>" +
	                "</li>";

                    $('#pnlClinicalComplaints #ulCompliantDisease').append(li);
                    $('.modal-backdrop').removeClass('in');
                    $('.modal-backdrop').addClass('out');
                    $('.modal-backdrop').hide();
                    $('#pnlClinicalComplaints #txtComplaints').val('');

                    //Clinical_Complaints.ComplaintsDetail.push(item);
                    var self = $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails');
                    utility.bindMyJSONByName(true, item, false, self).done(function () {
                        if (item.Complaint_LocationIds != '') {
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlLocation").val(item.Complaint_LocationIds.split(','));
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlLocation").multiselect("refresh");
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlLocation").multiselect({
                                includeSelectAllOption: true,
                                enableFiltering: true,
                                enableCaseInsensitiveFiltering: true,
                                maxHeight: 247
                            });
                        }
                        if (item.Complaint_CharacterIds != '') {
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlCharacter").val(item.Complaint_CharacterIds.split(','));
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlCharacter").multiselect("refresh");
                            $('#' + Clinical_Complaints.params.PanelID + " #ddlCharacter").multiselect({
                                includeSelectAllOption: true,
                                enableFiltering: true,
                                enableCaseInsensitiveFiltering: true,
                                maxHeight: 116
                            });

                        }
                        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtComment").html(item.Comments);
                        var myJSON = self != null ? self.getMyJSONByName() : "{}";
                        var objDetail = JSON.parse(myJSON);
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

                        var CharacterIDS = self.find('#ddlCharacter option:Selected').map(function () {
                            return this.value;
                        }).get().join(',');
                        objDetail["Complaint_CharacterIds"] = CharacterIDS;

                        var CharactersText = self.find('#ddlCharacter option:Selected').map(function () {
                            return this.text;
                        }).get().join(', ');
                        objDetail["Complaint_CharactersText"] = CharactersText;
                        objDetail["IsUpdate"] = false;
                        Clinical_Complaints.ComplaintsDetail.push(objDetail);

                    });
                    // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-473
                    Clinical_Complaints.resetValues();
                    // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-473


                });
                $.each(ComplainteDetailLoad_JSON, function (i, item) {
                    if (i == 0) {
                        Clinical_Complaints.AddInArray(item.ComplaintDetailId, item.ComplaintDescription, true);
                    }
                    else {
                        Clinical_Complaints.AddInArray(item.ComplaintDetailId, item.ComplaintDescription, false);
                    }
                });
                $.each(ComplainteLoad_JSON, function (i, item) {
                    $('#' + Clinical_Complaints.params.PanelID + " #dtComplaintDate").val(response.CapturedDateDate);
                    $('#' + Clinical_Complaints.params.PanelID + " #txtOverallComments").val(item.OverallComments);
                    $('#' + Clinical_Complaints.params.PanelID + " #txtComplaintId").val(item.ComplaintId != '' ? item.ComplaintId : 0);
                    Clinical_Complaints.ComplaintId = item.ComplaintId != '' ? item.ComplaintId : 0;
                });
                if (CallBack != null) {
                    CallBack();
                }
                else {
                    if ($('#' + Clinical_Complaints.params.PanelID + ' #ulCompliantDisease li').length > 0) {
                        $('#' + Clinical_Complaints.params.PanelID + ' #ulCompliantDisease li:eq(0)').trigger('click');
                    }
                }
                Clinical_Complaints.GlobalDate = $('#' + Clinical_Complaints.params.PanelID + " #dtComplaintDate").val();
                Clinical_Complaints.GlobalOverAllComment = $('#' + Clinical_Complaints.params.PanelID + " #txtOverallComments").val();
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
            Clinical_Complaints.deleteChiefComplaintConfirmed(ComplainDetailId, event)
        });
    },

    deleteChiefComplaintConfirmed: function (ComplainDetailId, event) {

        event.stopPropagation();

        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li#" + ComplainDetailId + "").remove();
        var ComplaintNotFromFavComplaint = true;
        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints ul#ulFavCompliantDisease li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == ComplainDetailId) {
                if ($(this).hasClass("active") == false) {
                    $(this).removeClass("disableAll");
                }
                ComplaintNotFromFavComplaint = false;
                Clinical_Complaints.deleteComplaintFromCasha(ComplainDetailId);
            }
        });

        if ($('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints ul#ulFavCompliantDisease li").length == 0) {
            Clinical_Complaints.IsUpdate = false;
        }
        if (ComplainDetailId > 0 && ComplaintNotFromFavComplaint) {
            $.when(Clinical_Complaints.ComplaintsSave(true, false)).then(function () {
                Clinical_Complaints.DeleteComplaint(ComplainDetailId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        Clinical_Complaints.IsDelete = true;

                        //Remove ChefComplaint Problems
                        Clinical_Complaints.removeChiefComplaintProblems(Clinical_Complaints.ComplaintId, ComplainDetailId);

                        Clinical_Complaints.loadCiefComplaintComponent(function () {
                            // Clinical_Complaints.getLatestComplaintByPatientId();
                            // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
                            $.when(Clinical_Complaints.createComplaintBodyHTML($('#' + Clinical_Complaints.params.PanelID + ' #txtOverallComments').val(), Clinical_Complaints.ComplaintsDetail, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true)).then(function () {
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

            Clinical_Complaints.deleteComplaintFromCasha(ComplainDetailId);
        }
        if (!ComplaintNotFromFavComplaint) {
            Clinical_Complaints.IsDelete = false;
        }

    },

    deleteComplaintFromCasha: function (ComplainDetailId) {
        var ComplaintsCopy = [];
        var ComplaintsDetailCopy = [];

        $.grep(Clinical_Complaints.ComplaintsDetail, function (item, index) {
            if (item.ComplaintDetailId == ComplainDetailId) {

            }
            else {
                ComplaintsDetailCopy.push(item);
            }
        });
        $.grep(Clinical_Complaints.Complaints, function (item, index) {
            if (item.ComplaintDetailId == ComplainDetailId) {

            }
            else {
                ComplaintsCopy.push(item);
            }
        });
        Clinical_Complaints.Complaints = [];
        Clinical_Complaints.ComplaintsDetail = [];
        Clinical_Complaints.Complaints = ComplaintsCopy;
        Clinical_Complaints.ComplaintsDetail = ComplaintsDetailCopy;
        if (Clinical_Complaints.ComplaintsDetail.length > 0) {

            if (Clinical_Complaints.ComplaintSelectedPrevious == ComplainDetailId) {
                Clinical_Complaints.ComplaintSelectedPrevious = Clinical_Complaints.ComplaintSelectedCurrent;
            }
            if (Clinical_Complaints.ComplaintSelectedCurrent == ComplainDetailId) {
                var pre = Clinical_Complaints.ComplaintSelectedPrevious;

                Clinical_Complaints.ComplaintSelectedPrevious = 0;
                Clinical_Complaints.fillChiefComplaints($('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li#" + pre + ""), null);
            }

        }
        else {
            Clinical_Complaints.ComplaintSelectedCurrent = 0;
            Clinical_Complaints.ComplaintSelectedPrevious = 0;
        }
    },
    resetValues: function () {
        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #sectionComplaintDetails").find('[type=text],[type=password],[type=checkbox],textarea,[type=radio]').each(function () {
            $(this).val('');
        });
        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #sectionComplaintDetails").find('select').each(function () {
            $(this).val('0');
        });
        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #sectionComplaintDetails input:checkbox").removeAttr('checked');

        $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails #ddlLocation, #' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails #ddlCharacter').multiselect('rebuild');
        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtComment").html('');
        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtComments").val('');

    },
    ResetComplaint: function () {

        utility.myConfirm('27', function () {
            if ($('#pnlClinicalComplaints #txtComplaintId').val() != 0) {
                Clinical_Complaints.ComplaintReset(false, null).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_Complaints.IsReset = true;
                        Clinical_Complaints.ResetedComplaitId = $('#pnlClinicalComplaints #txtComplaintId').val();
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_Complaints.loadCiefComplaintComponent(function () {
                            Clinical_Complaints.ComplaintsSave(false, false);
                        });

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                Clinical_Complaints.IsDurationValidation = true;
                Clinical_Complaints.ValidationErrorMessage = "";
                Clinical_Complaints.IsUpdate = false;
                Clinical_Complaints.Complaints = [];
                Clinical_Complaints.ComplaintsDetail = [];
                Clinical_Complaints.ComplaintSelectedPrevious = 0;
                Clinical_Complaints.ComplaintSelectedCurrent = 0;
                Clinical_Complaints.IsDelete = false;
                Clinical_Complaints.IntializeMultiSelectDropDown();
                Clinical_Complaints.resetValues();
                $("#txtOverallComments").val("");
                $('#' + Clinical_Complaints.params.PanelID + " #txtComplaintId").val(0);
                Clinical_Complaints.ComplaintId = 0;
                utility.CreateDatePicker(Clinical_Complaints.params.PanelID + '  #dtComplaintDate', function () {
                }, true);
                $('#pnlClinicalComplaints #ulCompliantDisease').html("");
            }
        });

    },
    unLoadTab: function (nextOrPre, controlToInvoke) {
        // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
        Clinical_Complaints.controlToInvoke = controlToInvoke;

        if (Clinical_Complaints.IsReset == true) {
            utility.myConfirm('35', function () {

                //Clinical_Complaints.controlToInvoke = ComeFromLowerButton;
                $.when(Clinical_Complaints.ComplaintsSave(false, true)).then(function () {
                    var objDeffered = $.Deferred();
                    if (Clinical_Complaints.params["FromAdmin"] == "0") {
                        if (Clinical_Complaints.params != null && Clinical_Complaints.params.ParentCtrl != null) {
                            if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                                Clinical_Complaints.loadNoteComponent();
                            }
                            else {
                                UnloadActionPan(Clinical_Complaints.params.ParentCtrl, 'Clinical_Complaints');
                            }
                        }
                        else
                            UnloadActionPan(null, 'Clinical_Complaints');
                    }
                    else {

                        RemoveAdminTab();
                    }
                    objDeffered.resolve();
                    EMRUtility.scrollToPNcomponent('clinical_complaints');
                    return objDeffered;
                });
            }, function () {
                //Clinical_Complaints.controlToInvoke = ComeFromLowerButton;
                var objDeffered = $.Deferred();

                if (Clinical_Complaints.params["FromAdmin"] == "0") {
                    if (Clinical_Complaints.params != null && Clinical_Complaints.params.ParentCtrl != null) {
                        if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                            Clinical_Complaints.loadNoteComponent();
                        }
                        else {
                            UnloadActionPan(Clinical_Complaints.params.ParentCtrl, 'Clinical_Complaints');
                        }
                    }
                    else
                        UnloadActionPan(null, 'Clinical_Complaints');
                }
                else {

                    RemoveAdminTab();
                }
                objDeffered.resolve();
                EMRUtility.scrollToPNcomponent('clinical_complaints');
                return objDeffered;
            });
        }
        else {
            Clinical_Complaints.IsDurationValidation = true;

            var ComplaintsDetail_Copy = [];
            $.grep(Clinical_Complaints.ComplaintsDetail, function (item, index) {
                if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedCurrent) {
                    var self = $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails');
                    var myJSON = self != null ? self.getMyJSONByName() : "{}";
                    var objDetail = JSON.parse(myJSON);
                    objDetail["ComplaintDetailId"] = Clinical_Complaints.ComplaintSelectedCurrent;
                    $.grep(Clinical_Complaints.Complaints, function (item, index) {
                        if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedCurrent) {
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

                    var CharacterIDS = self.find('#ddlCharacter option:Selected').map(function () {
                        return this.value;
                    }).get().join(',');

                    objDetail["Complaint_CharacterIds"] = CharacterIDS;

                    var CharactersText = self.find('#ddlCharacter option:Selected').map(function () {
                        return this.text;
                    }).get().join(', ');
                    objDetail["Complaint_CharacterIds_text"] = CharactersText;

                    objDetail["IsUpdate"] = Clinical_Complaints.IsUpdate;
                    ComplaintsDetail_Copy.push(objDetail);

                }
                else {
                    ComplaintsDetail_Copy.push(item);
                }
            });
            Clinical_Complaints.ComplaintsDetail = ComplaintsDetail_Copy;

            var IsSave = false;
            $.grep(Clinical_Complaints.ComplaintsDetail, function (item, index) {

                if (item.IsUpdate == true || (Clinical_Complaints.GlobalDate != $('#' + Clinical_Complaints.params.PanelID + " #dtComplaintDate").val()) || (Clinical_Complaints.GlobalOverAllComment != $('#' + Clinical_Complaints.params.PanelID + " #txtOverallComments").val())) {
                    IsSave = true;
                }
            });
            if (IsSave || Clinical_Complaints.IsDelete) {
                utility.myConfirmNote('1', function () {
                    //Clinical_Complaints.controlToInvoke = ComeFromLowerButton;
                    $.when(Clinical_Complaints.ComplaintsSave(false, true)).then(function () {
                        var objDeffered = $.Deferred();
                        if (Clinical_Complaints.params["FromAdmin"] == "0") {
                            if (Clinical_Complaints.params != null && Clinical_Complaints.params.ParentCtrl != null) {
                                if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                                    Clinical_Complaints.loadNoteComponent();
                                }
                                else {
                                    UnloadActionPan(Clinical_Complaints.params.ParentCtrl, 'Clinical_Complaints');
                                }
                            }
                            else
                                UnloadActionPan(null, 'Clinical_Complaints');
                        }
                        else {

                            RemoveAdminTab();
                        }
                        objDeffered.resolve();
                        EMRUtility.scrollToPNcomponent('clinical_complaints');
                        return objDeffered;
                    });

                }, "", function () {
                    //Clinical_Complaints.controlToInvoke = ComeFromLowerButton;
                    var objDeffered = $.Deferred();

                    if (Clinical_Complaints.params["FromAdmin"] == "0") {
                        if (Clinical_Complaints.params != null && Clinical_Complaints.params.ParentCtrl != null) {
                            if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                                Clinical_Complaints.loadNoteComponent();
                            }
                            else {
                                UnloadActionPan(Clinical_Complaints.params.ParentCtrl, 'Clinical_Complaints');
                            }
                        }
                        else
                            UnloadActionPan(null, 'Clinical_Complaints');
                    }
                    else {

                        RemoveAdminTab();
                    }
                    objDeffered.resolve();
                    EMRUtility.scrollToPNcomponent('clinical_complaints');
                    return objDeffered;
                });

            }
            else {
                //Clinical_Complaints.controlToInvoke = ComeFromLowerButton;
                var objDeffered = $.Deferred();

                if (Clinical_Complaints.params["FromAdmin"] == "0") {
                    if (Clinical_Complaints.params != null && Clinical_Complaints.params.ParentCtrl != null) {
                        if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                            Clinical_Complaints.loadNoteComponent();
                        }
                        else {
                            UnloadActionPan(Clinical_Complaints.params.ParentCtrl, 'Clinical_Complaints');
                        }
                    }
                    else
                        UnloadActionPan(null, 'Clinical_Complaints');
                }
                else {

                    RemoveAdminTab();
                }
                objDeffered.resolve();
                EMRUtility.scrollToPNcomponent('clinical_complaints');
                return objDeffered;
            }
        }
        // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
    },
    AddCompalintsToNotes: function () {
        $.when(Clinical_Complaints.ComplaintsSave(false, false)).then(function () {
            var objDeffered = $.Deferred();
            if (Clinical_Complaints.params["FromAdmin"] == "0") {
                if (Clinical_Complaints.params && Clinical_Complaints.params.ParentCtrl) {
                    if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote") {
                        Clinical_Complaints.loadNoteComponent();
                    }
                    else {
                        UnloadActionPan(Clinical_Complaints.params.ParentCtrl, 'Clinical_Complaints');
                    }
                }
                else
                    UnloadActionPan(null, 'Clinical_Complaints');
            }
            else {

                RemoveAdminTab();
            }
            objDeffered.resolve();
            EMRUtility.scrollToPNcomponent('clinical_complaints');
            return objDeffered;
        });
    },
    //Start//12/02/2016//M Ahmad Imran//Implimented call to controller for Get Complaint Detail Data from DB
    LoadComplaints: function () {
        var objData = {};
        objData["NotesId"] = Clinical_Complaints.params.NotesId;
        objData["commandType"] = "Load_Complaint";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Complaint");
    },
    //End M Ahmad Imran 10/2/2016
    //Start//10/02/2016//M Ahmad Imran//Implimented validation on Complaint Detail field
    validateComplaint: function () {
        $('#' + Clinical_Complaints.params.PanelID + ' #frmClinicalComplaints')
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
                //Clinical_Complaints.ComplaintsSave();
            }

        });

    },
    //End M Ahmad Imran 10/2/2016

    //Start//10/02/2016//M Ahmad Imran//Implimented Save Complaint Detail
    ComplaintsSave: function (ComeFromDelete, ComeFromUnLoad) {
        var PreFavVal = $('#' + Clinical_Complaints.params.PanelID + ' #ddlFavComplaint').val();
        // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
        var objDeffered = $.Deferred();
        // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
        if (!Clinical_Complaints.IsDurationValidation) {
            //utility.DisplayMessages(Clinical_Complaints.ValidationErrorMessage, 3);
        }
        else {
            // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
            var CopyComplainDetail = CopyComplainDetail = Clinical_Complaints.ComplaintsDetail;
            var OverAllComments = $('#' + Clinical_Complaints.params.PanelID + ' #txtOverallComments').val();
            // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
            if (!ComeFromUnLoad) {
                var ComplaintsDetail_Copy = [];
                $.grep(Clinical_Complaints.ComplaintsDetail, function (item, index) {
                    if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedCurrent) {
                        var self = $('#' + Clinical_Complaints.params.PanelID + ' #sectionComplaintDetails');
                        if (self.find("#txtComment").text()) {
                            self.find("#txtComments").val(self.find("#txtComment").text()); // add comments in  existing field
                        }
                        else {
                            self.find("#txtComments").val("");
                        }
                        var myJSON = self != null ? self.getMyJSONByName() : "{}";
                        var objDetail = JSON.parse(myJSON);
                        objDetail["ComplaintDetailId"] = Clinical_Complaints.ComplaintSelectedCurrent;
                        $.grep(Clinical_Complaints.Complaints, function (item, index) {
                            if (item.ComplaintDetailId == Clinical_Complaints.ComplaintSelectedCurrent) {
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

                        var CharacterIDS = self.find('#ddlCharacter option:Selected').map(function () {
                            return this.value;
                        }).get().join(',');
                        objDetail["Complaint_CharacterIds"] = CharacterIDS;

                        var CharactersText = self.find('#ddlCharacter option:Selected').map(function () {
                            return this.text;
                        }).get().join(', ');
                        objDetail["Complaint_CharactersText"] = CharactersText;
                        objDetail["Complaint_CharacterIds_text"] = CharactersText;


                        objDetail["IsUpdate"] = Clinical_Complaints.IsUpdate;
                        ComplaintsDetail_Copy.push(objDetail);

                    }
                    else {
                        ComplaintsDetail_Copy.push(item);
                    }
                });
                Clinical_Complaints.ComplaintsDetail = ComplaintsDetail_Copy;
                // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
                CopyComplainDetail = CopyComplainDetail = Clinical_Complaints.ComplaintsDetail;
                OverAllComments = $('#' + Clinical_Complaints.params.PanelID + ' #txtOverallComments').val();
                // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
            }
            if (Clinical_Complaints.ComplaintsDetail.length != 0) {

                // fill to SAVE ICD's while saving Complaint Detail.
                $("#pnlClinicalComplaints #ulCompliantDisease li").each(function (i, e) {
                    $(this).attr('icd9code') && $(this).attr('icd9code') != 'undefined' ? Clinical_Complaints.ComplaintsDetail[i]["ICD9"] = $(this).attr('icd9code') : '';
                    $(this).attr('icd9desc') && $(this).attr('icd9desc') != 'undefined' ? Clinical_Complaints.ComplaintsDetail[i]["ICD9Description"] = $(this).attr('icd9desc') : '';
                    $(this).attr('icd10code') && $(this).attr('icd10code') != 'undefined' ? Clinical_Complaints.ComplaintsDetail[i]["ICD10"] = $(this).attr('icd10code') : '';
                    $(this).attr('icd10desc') && $(this).attr('icd10desc') != 'undefined' ? Clinical_Complaints.ComplaintsDetail[i]["ICD10Description"] = $(this).attr('icd10desc') : '';
                    $(this).attr('snomedcode') && $(this).attr('snomedcode') != 'undefined' ? Clinical_Complaints.ComplaintsDetail[i]["SNOMED"] = $(this).attr('snomedcode') : '';
                    $(this).attr('snomeddesc') && $(this).attr('snomeddesc') != 'undefined' ? Clinical_Complaints.ComplaintsDetail[i]["SNOMEDDescription"] = $(this).attr('snomeddesc') : '';
                });

                //var myJSON = JSON.stringify(Clinical_Complaints.ComplaintsDetail);
                Clinical_Complaints.SaveComplaint().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_Complaints.SaveFavToggelStatus(PreFavVal);

                        if (response.ComplaintId) {
                            $('#' + Clinical_Complaints.params.PanelID + " #txtComplaintId").val(response.ComplaintId);
                            Clinical_Complaints.ComplaintId = response.ComplaintId;
                        }


                        // fill to SAVE ICD's from ProblemList flow.
                        Clinical_Complaints.ComplaintsDetailForDx = Clinical_Complaints.ComplaintsDetail;
                        var ComplaintDetail_List = JSON.parse(response.ComplaintDetail_JSON);
                        $(ComplaintDetail_List).each(function (i, item) {

                            Clinical_Complaints.ComplaintsDetailForDx[i]["icd9code"] = item.ICD9;
                            Clinical_Complaints.ComplaintsDetailForDx[i]["icd9desc"] = item.ICD9Description;
                            Clinical_Complaints.ComplaintsDetailForDx[i]["icd10code"] = item.ICD10;
                            Clinical_Complaints.ComplaintsDetailForDx[i]["icd10desc"] = item.ICD10Description;
                            Clinical_Complaints.ComplaintsDetailForDx[i]["snomedcode"] = item.SNOMED;
                            Clinical_Complaints.ComplaintsDetailForDx[i]["snomeddesc"] = item.SNOMEDDescription;
                            Clinical_Complaints.ComplaintsDetailForDx[i]["ComplaintDetailId"] = item.ComplaintDetailId;
                            Clinical_Complaints.ComplaintsDetailForDx[i]["ComplaintId"] = item.ComplaintId;
                        });

                        if (!ComeFromDelete) {
                            Clinical_Complaints.loadCiefComplaintComponent(function () {
                                // Clinical_Complaints.getLatestComplaintByPatientId();
                                // Begin 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
                                $.when(Clinical_Complaints.createComplaintBodyHTML(OverAllComments, Clinical_Complaints.ComplaintsDetail, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true)).then(function () {
                                    Clinical_Complaints.IsReset = false;

                                    objDeffered.resolve();

                                });
                                // End 03/28/16  Edit By M Ahmad Imran Bug # EMR-491
                                Clinical_Complaints.AddComplaintsToProblemList(Clinical_Complaints.ComplaintsDetailForDx);
                            });
                            utility.DisplayMessages(response.Message, 1);
                            //Clinical_Complaints.AddComplaintsToProblemList();
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
                if (Clinical_Complaints.IsDelete && ComeFromUnLoad) {
                    $.when(Clinical_Complaints.createComplaintBodyHTML(OverAllComments, CopyComplainDetail, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML')).then(function () {
                        objDeffered.resolve();
                    });
                }
                else if (Clinical_Complaints.IsReset && ComeFromUnLoad) {
                    $.when(Clinical_Complaints.createComplaintBodyHTML(OverAllComments, CopyComplainDetail, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML')).then(function () {
                        Clinical_Complaints.IsReset = false;
                        objDeffered.resolve();
                    });
                }
                else if (Clinical_Complaints.IsReset) {
                    $.when(Clinical_Complaints.createComplaintBodyHTML(OverAllComments, CopyComplainDetail, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML')).then(function () {
                        Clinical_Complaints.IsReset = false;
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
            Clinical_Complaints.BindFavComplaints();
        });
    },
    BindFavComplaints: function () {
        var FavoriteListId = $('#' + Clinical_Complaints.params.PanelID + ' #ddlFavComplaint').val();
        var SearchData = $('#' + Clinical_Complaints.params.PanelID + ' #FavSearchBox').val();
        if (FavoriteListId != "") {
            Favorite_Complaints.searchFavoriteList_ICD_DBCall(null, FavoriteListId, null, null, SearchData).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (response.FavoriteListICDCount > 0) {
                        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #ulFavCompliantDisease li").remove();
                        var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
                        var li = "";
                        $.each(FavoriteListJSON, function (i, item) {
                            li += "<li icd9Code='" + item.ICD9Code + "' icd10Code='" + item.ICD10Code
                            + "' snomedCode='" + item.SNOMEDID
                            + "' icd9Desc='" + item.ICD9CodeDescription
                            + "' icd10Desc='" + item.ICD10CodeDescription
                            + "' snomedDesc='" + item.SNOMEDDescription +
                            "'id=" + Clinical_Complaints.FavComplaintsId + " onclick='Clinical_Complaints.PassToComplaints(this, event);'  ><a href='#' class='pr-sm'>" + item.ICD10CodeDescription + "</a></li>"
                            Clinical_Complaints.AddArrayInFavComplaints(Clinical_Complaints.FavComplaintsId, item.ICD10CodeDescription);
                            Clinical_Complaints.FavComplaintsId = Clinical_Complaints.FavComplaintsId - 1;
                        });
                        $('#pnlClinicalComplaints #ulFavCompliantDisease').append(li);

                    }
                    else {
                        $('#' + Clinical_HPIComplaints.params.PanelID + " #frmClinicalComplaints #ulFavCompliantDisease li").remove();
                    }
                }
                else {
                    $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #ulFavCompliantDisease li").remove();
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
            var li = "<li icd9Code='" + $(obj).attr('icd9Code') + "'  icd10Code='" + $(obj).attr('icd10Code') + "' snomedCode='" + $(obj).attr('snomedCode') + "' icd9Desc='" + $(obj).attr('icd9Desc') + "' icd10Desc='" + $(obj).attr('icd10Desc') + "' snomedDesc='" + $(obj).attr('snomedDesc') + "' id=" + diseaseId + " onclick='Clinical_Complaints.fillChiefComplaints(this, event);'>" +
                "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + ($(obj).attr('icd10Code') != "" ? $(obj).attr('icd10Code') + " - " : "") + $(obj).attr('icd10Desc') + "</span></a>" +
                    "<span class='removeIconListHover'>" +
                        "<a href='#' onclick='Clinical_Complaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			            "<a href='#' onclick='Clinical_Complaints.deleteChiefComplaint(" + diseaseId + ", event);'><i class='fa fa-times red'></i></a>" +
		            "</span>" +
                "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_Complaints.updateComplaintsText(this, event);'/>" +
                "<div class='clearfix'></div>" +
            "</li>";
            $('#pnlClinicalComplaints #ulCompliantDisease').append(li);
            Clinical_Complaints.AddInArray(diseaseId, $(obj).attr('icd10Desc'), true);
            Clinical_Complaints.IsUpdate = true;
        }
        else {
            utility.DisplayMessages('Disease already added', 2);
        }

    },
    //End M Ahmad Imran 23/03/2016


    //Start//24/03/2016//M Ahmad Imran//Implimented which pass all FavComplint to complaints list
    BindAllFavComplaints: function () {
        var diseaseId = 0;
        $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints ul#ulFavCompliantDisease li").each(function (i, item) {


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
                var li = "<li icd9Code='" + $(this).attr('icd9Code') + "'  icd10Code='" + $(this).attr('icd10Code') + "' snomedCode='" + $(this).attr('snomedCode') + "' icd9Desc='" + $(this).attr('icd9Desc') + "' icd10Desc='" + $(this).attr('icd10Desc') + "' snomedDesc='" + $(this).attr('snomedDesc') + "' id=" + diseaseId + " onclick='Clinical_Complaints.fillChiefComplaints(this, event);'>" +
                    "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + $(this).attr('icd10Desc') + "</span></a>" +
                        "<span class='removeIconListHover'>" +
                            "<a href='#' onclick='Clinical_Complaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                "<a href='#' onclick='Clinical_Complaints.deleteChiefComplaint(" + diseaseId + ", event);'><i class='fa fa-times red'></i></a>" +
		                "</span>" +
                    "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_Complaints.updateComplaintsText(this, event);'/>" +
                    "<div class='clearfix'></div>" +
                "</li>";
                $('#pnlClinicalComplaints #ulCompliantDisease').append(li);
                Clinical_Complaints.AddInArray(diseaseId, $(this).attr('icd10Desc'), true);
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
        var isFavListOpened = $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #favSectionDiv").hasClass("toggledHor");
        $.when(EMRUtility.insertUpdateFavListStatus(Clinical_Complaints.FavListName, isFavListOpened)).then(function () {
            EMRUtility.insertUpdateFavListVal(Clinical_Complaints.FavListName, FavListVal);
        });
    },
    // End 7/2/2016 Muhammad Ahmad Imran

    //Start//10/02/2016//M Ahmad Imran//Implimented Call to Controller for Save Complaint Detail
    SaveComplaint: function (updateFromProgressNote) {
        //var updateFromProgressNote = updateFromProgressNote ? JSON.parse(updateFromProgressNote) : "";
        var objData = {};
        var self = $('#pnlClinicalComplaints');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);
        objData["PatientId"] = Clinical_Complaints.params.PatientId;
        objData["DateCaptured"] = objDetail["DateCaptured"];
        objData["ComplaintId"] = updateFromProgressNote ? updateFromProgressNote[0].ComplaintId : objDetail["ComplaintId"];
        objData["OverallComments"] = objDetail["OverallComments"];
        objData["NotesId"] = Clinical_Complaints.params.NotesId;
        objData["ComplaintDetails"] = updateFromProgressNote ? updateFromProgressNote : Clinical_Complaints.ComplaintsDetail;
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
        objData["NotesId"] = Clinical_Complaints.params.NotesId;
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
            objData["ComplaintId"] = $('#pnlClinicalComplaints #txtComplaintId').val();
        }
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "RESET_Complaint";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Complaint");
    },
    //End M Ahmad Imran 10/2/2016

    //Start//10/02/2016//M Ahmad Imran//For record Complaint
    AddInArray: function (id, value, IsSelect) {
        var item = {}
        item["ComplaintDetailId"] = id;
        item["ComplaintDescription"] = value;
        item["Comments"] = "";
        Clinical_Complaints.Complaints.push(item);
        if (IsSelect) {
            Clinical_Complaints.fillChiefComplaints($('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li#" + id + ""), null);
            }

    },
    //End M Ahmad Imran 10/2/2016

    //Start//23/03/2016//M Ahmad Imran//For record Fav Complaint
    AddArrayInFavComplaints: function (id, value) {
        item = {}
        item["FavComplaintId"] = id;
        item["FavComplaintDescription"] = value;
        Clinical_Complaints.FavComplaints.push(item);
    },
    //End M Ahmad Imran 23/03/2016

    /* -----------------Progress Note-------------

    Reason: These functions are used for Progress Note Soap Attachment, creation and detachment */

    /* Call Back function to add component to Progress Note
       */
    addComplaintToNotes: function () {
        Clinical_Complaints.SaveComplaint();
    },

    /*  Wasim Malik
    Start/ 17/2/2016.
    This function will get Birth History Soap Text and attach that to Progress note
     */
    getComplaintInfo: function (unloadComplaint, complaintId) {
        if (unloadComplaint == null) {
            unloadComplaint = false;
        }
        Clinical_Complaints.fillBirthHx_DBCall(complaintId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Complaints.createComplaintBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', unloadComplaint);
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
            if (globalAppdata["IsDefaultHPI"] == "True" || globalAppdata["IsDefaultHPI"] == "1") {
                $(CompnentSelector).append(' <li class="ComplaintsComponent" NoteComponentId="NCDummyId"> <header>' +
                                        '<Clinical_Complaints title="Complaints"  id="' + Clinical_Complaints.ComplaintId + '" class="NotesComponent">' +
                                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'HPIComplaints\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Complaints">Complaints</a> ' +
                                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Complaints\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Complaints\',\'-1\',' + ComplaintId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                                   '</Clinical_Complaints> </header></li>');
            }
            else {
                $(CompnentSelector).append(' <li class="ComplaintsComponent" NoteComponentId="NCDummyId"> <header>' +
                            '<Clinical_Complaints title="Complaints"  id="' + Clinical_Complaints.ComplaintId + '" class="NotesComponent">' +
                            '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Complaints\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Complaints">Complaints</a> ' +
                            '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Complaints\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                            '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Complaints\',\'-1\',' + ComplaintId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                       '</Clinical_Complaints> </header></li>');
            }
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

            Clinical_Complaints.checkComplaintHxExists(Complaints.ComplaintDetailId);
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
            
            var ServerDateTime = $("#mainForm #userCurrentTime").html().split(" ")
            var d = new Date(ServerDateTime[1] + " " + ServerDateTime[2] + " " + ServerDateTime[3]);
            var UpdateDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
            var divId = '"Cli_Complaint_' + ComplaintHxId + '"';
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
                if (i == 0)
                    StartLine = "A " + patientInfo.age + " old " + patientInfo.gender + " presents with ";
                else if (i == ComplaintFill_Obj.length - 1)
                    StartLine = " and ";
                else
                    StartLine = "";
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
                //Clinical_Complaints.updateComplaintHtml($mainDivComplaintHx.append(), ComplaintHxId, noteHTMLCtrl, hideAlertMessage);

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
    createComplaintBodyHTML: function (OverAllComments, response, noteHTMLCtrl, unloadComplaint, hideAlertMessage) {


        var ComplaintHxId = $('#' + Clinical_Complaints.params.PanelID + " #txtComplaintId").val();
        if (typeof $('#' + Clinical_Complaints.params.PanelID + " #txtComplaintId").val() === "undefined") {
            if (response[0] != null) {
                ComplaintHxId = response[0].Expr1 ? response[0].Expr1 : $('#ProgressnoteHTML section [id^=Cli_Complaint]').attr('id').split('_')[2];
            } else {
                ComplaintHxId = null;
            }

        }
        if (Clinical_Complaints.IsReset) {
            ComplaintHxId = Clinical_Complaints.ResetedComplaitId;
        }
        var patientInfo = Clinical_Complaints.GetPatientInfo();

        Clinical_Complaints.checkComplaintHxExists(ComplaintHxId);
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
                $HistoryListComplaint.find('p').append(StartLine + $DetailsDiv
                //+ (OverAllComments != "" ? "</br></br>" + OverAllComments : "")
               );
            }

            //End 4/26/16  Edit By M Ahmad Imran Bug # EMR-423
            //if (ComplaintFill_Obj.length > (i + 1)) {
            //    $HistoryListComplaint.append("<br>");
            //}
        });

        if (response.length > 0) {
            if (OverAllComments) {
                var allComments = OverAllComments != "" ? OverAllComments : "";
                $HistoryListComplaint.find('p').append('<br>' + allComments);
            }
            else if (ComplaintFill_Obj[0].OverallComments) {
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
        //History

        // $HistoryDetailsDiv.html($HistoryListComplaint);

        // $SectionBodyComplaint.append($HistoryDetailsDiv);
        $SectionBodyComplaint.append($DetailsDiv);
        //
        $SectionBodyComplaint.attr('id', "Cli_Complaint_Main" + ComplaintHxId);


        if ($(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId).length == 0) {

            if ($(noteHTMLCtrl).find('#Section_Complaint').length > 0) {
                $(noteHTMLCtrl).find('#Section_Complaint').remove();
            }
            $mainDivComplaintHx.html($SectionBodyComplaint);
            Clinical_Complaints.updateComplaintHtml($mainDivComplaintHx.append(), ComplaintHxId, noteHTMLCtrl, hideAlertMessage);
        } else {

            var CommentHTML = "";
            var CommentsID = $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId + ' ul li:Last').attr('id');
            if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                CommentHTML = $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId + ' ul li:Last').get(0).outerHTML;
            }
            if ($(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId))
                $(noteHTMLCtrl).find('#Cli_Complaint_Main' + ComplaintHxId).remove();
            $mainDivComplaintHx.html($SectionBodyComplaint);

            Clinical_Complaints.updateComplaintHtml($mainDivComplaintHx.append(), ComplaintHxId, noteHTMLCtrl, hideAlertMessage);
        }
        //Clinical_ProgressNote.updateProgressNoteHTML();
        //Clinical_ProgressNote.InitilizeClickableList();
        Clinical_ProgressNote.initilizeEditableContent();
        if (unloadComplaint == true) {
            Clinical_Complaints.unloadComplaint();
        }



        //$(ComplaintFill_Obj).each(function (i, item) {
        //    if ($(this).attr("ComplaintId") != null)
        //        var ComplaintHxId = item.ComplaintId;//ComplaintFill_Obj[0]["ComplaintId"];

        //    $SectionBodyComplaint.attr('id', "Cli_Complaint_Main" + ComplaintHxId);
        //    var $DetailsDiv = $(document.createElement('div'));
        //    $DetailsDiv.attr('id', "Cli_Complaint_" + ComplaintHxId);

        //    var $HistoryDiv = $(document.createElement('div'));
        //    $HistoryDiv.attr('id', "Cli_Complaint_History" + ComplaintHxId);

        //    var $ListComplaintHistory = $(document.createElement('ul'));

        //    $ListComplaintHistory.attr('class', 'list-unstyled')

        //    var $ListComplaint = $(document.createElement('ul'));

        //    $ListComplaint.attr('class', 'list-unstyled')

        //    $SectionBodyComplaint.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Complaint_" + ComplaintHxId + '"><i class="fa fa-edit"></i></a>' +
        //        '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Complaint_Main" + ComplaintHxId + '"  ><i class="fa fa-times"></i></a></div> ');

        //    if (item.IsChiefComplaint = 1)
        //    {
        //        $ListComplaint.append("<li>" + item.ComplaintDescription + "(CC)" + "</li>");
        //        }
        //    else {
        //        $ListComplaint.append("<li>" + item.ComplaintDescription + "</li>");
        //    }
        //});
        //$ListComplaintHistory.append("<br/>" + "<h4> History of present illeness" + "</h4>");
        //$(ComplaintFill_Obj).each(function (i, item) {
        //    var ComplaintHxId = item.ComplaintId;
        //    $ListComplaintHistory.append("<br/>" + "<li>" + "The patint present with " + item.ComplaintDescription + "Previous History: Symptoms were identified" + item.Duration + "</li>");


        //    $DetailsDiv.append($ListComplaint);
        //    $HistoryDiv.append($ListComplaintHistory);
        //    $SectionBodyComplaint.append($DetailsDiv);
        //    $SectionBodyComplaint.append($HistoryDiv);
        //    if ($(noteHTMLCtrl + 'Clinical_Complaints').parent().parent().find('#Cli_Complaint_Main' + ComplaintHxId).length == 0) {
        //        $mainDivComplaintHx.append($SectionBodyComplaint);
        //        Clinical_Complaints.updateComplaintHtml($mainDivComplaintHx.append(), ComplaintHxId, noteHTMLCtrl);
        //    } else {

        //        var CommentHTML = "";
        //        var CommentsID = $(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().find('#Cli_Complaint_Main' + ComplaintHxId + ' ul li:Last').attr('id');
        //        if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
        //            CommentHTML = $(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().find('#Cli_Complaint_Main' + ComplaintHxId + ' ul li:Last').get(0).outerHTML;
        //        }
        //        $(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().find('#Cli_Complaint_Main' + ComplaintHxId).html($SectionBodyComplaint.html());
        //        $(noteHTMLCtrl + ' Clinical_Complaints').parent().parent().find('#Cli_Complaint_Main' + ComplaintHxId + ' ul').append(CommentHTML);
        //        Clinical_ProgressNote.updateProgressNoteHTML();
        //        Clinical_Complaints.updateComplaintHtml("", ComplaintHxId, noteHTMLCtrl);

        //    }



        //});

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
        if (Clinical_Complaints.params["FromAdmin"] == "0") {
            if (Clinical_Complaints.params != null && Clinical_Complaints.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_Complaints.params.ParentCtrl, 'Clinical_Complaints');
            }
            else
                UnloadActionPan(null, 'Clinical_Complaints');
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
        if (birthHxHtml != '' && Clinical_Complaints.IsReset == false) {
            Clinical_Complaints.attachComplaintFromNotes(ComplaintHxId, hideAlertMessage);
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
    detach_ComponentsComplaints: function (componentName, isUpdate, ComponentRemove) {


        var Clinical_ComplaintsIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Complaints').parent().parent().find('section[id*="Cli_Complaint_Main"]').map(function () {
            return this.id.replace("Cli_Complaint_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ComplaintsComponent').attr('NoteComponentId');

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

        if (Clinical_ComplaintsIds == "" || Clinical_ComplaintsIds == "undefined") {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Complaints'))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    Clinical_Complaints.removeChiefComplaintProblems(ComplaintId);
                    if (Clinical_ProgressNote.params["TemplateName"] == "" && ComponentRemove)
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            Clinical_Complaints.ComplaintReset(true, ComplaintId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (isUpdate) {
                        if (NoteComponentId && NoteComponentId != "NCDummyId") {
                            var promise = [];
                            if (Clinical_ProgressNote.params["TemplateName"]) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().addClass('hidden');
                                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Complaints'))
                            }
                            else
                                promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                            $.when.apply($, promise).done(function () {
                                Clinical_Complaints.removeChiefComplaintProblems(ComplaintId);
                                if (Clinical_ProgressNote.params["TemplateName"] == "" && ComponentRemove)
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').parent().parent().remove();
                                Clinical_ProgressNote.ShowHideComponetsHeaders();
                                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                            });
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
            EMRUtility.scrollToPNcomponent('clinical_complaint');
            var selectedValue = ComplaintHxId.replace('Cli_Complaint_Main', '');
            var ComplaintId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Complaints').attr('id');

            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Complaints.detachComplaintFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ComplaintsComponent').attr('NoteComponentId');
                        $('#' + ComplaintHxId).remove();
                        if ($('#' + Clinical_ProgressNote.params["PanelID"]).find('#Section_Complaint').length > 0) {
                            $('#' + Clinical_ProgressNote.params["PanelID"]).find('#Section_Complaint').remove();
                        }

                        Clinical_ProgressNote.params["IsBodyPart"] = false;
                        //Remove ChefComplaint Problems
                        Clinical_Complaints.removeChiefComplaintProblems(ComplaintId);

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
        if (selectedValue == "" || selectedValue == null ||  typeof selectedValue == "undefined") {
        }
        else {
            Clinical_Complaints.attachComplaintFromNotes_DBCall(selectedValue).done(function (response) {
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
        Clinical_Complaints.getLatestClinical_ComplaintsByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Complaints.createComplaintBodyHTML(null, JSON.parse(response.ComplaintSoap_JSON || "[]"), '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
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
    getLatestClinical_ComplaintsByPatientId_DBCall: function () {
        var objData = new Object();
        if ($('#' + Clinical_Complaints.params.PanelID + " #txtComplaintId").val() != "" && typeof $('#' + Clinical_Complaints.params.PanelID + " #txtComplaintId").val() !== "undefined") {
            objData["ComplaintId"] = $('#' + Clinical_Complaints.params.PanelID + " #txtComplaintId").val();
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
        //var noCC = $.grep(Clinical_Complaints.ComplaintsDetail, function (item, index) {
        //    return item.IsChiefComplaint != true;
        //});

        //$.each(ComplaintsDetail, function (i, e) {
        //    if (ComplaintsDetail[i].IsChiefComplaint) {
        //        Clinical_Complaints.SaveProblemLists(ComplaintsDetail[i]).done(function (response) {
        //            response = JSON.parse(response);
        //            if (response.status != false) {
        //                if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote") {
        //                    Clinical_Complaints.getProblemListsInfo(response.ProblemListId);
        //                }
        //            }
        //            else {
        //                utility.DisplayMessages(response.Message, 3);
        //            }
        //        });
        //    }
        //});
        $.each(ComplaintsDetail, function (i, e) {
            if (ComplaintsDetail[i].icd9code || ComplaintsDetail[i].icd10code) {
                Clinical_Complaints.SaveProblemLists(ComplaintsDetail[i]).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote") {
                            Clinical_Complaints.getProblemListsInfo(response.ProblemListId, true, ComplaintsDetail[i].ComplaintDetailId);
                        }
                    }
                        //Start 11-02-2017 Edit By Humaira Yousaf EMR-5081
                    else if (response.Message.indexOf('problem already exists') >= 0) {
                        var problemId = response.ProblemListId;
                        if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote") {
                            Clinical_Complaints.getProblemListsInfo(problemId, true, ComplaintsDetail[i].ComplaintDetailId);
                        }
                    }
                        //End 11-02-2017 Edit By Humaira Yousaf EMR-5081
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
        objData["ComplaintId"] = $('#' + Clinical_Complaints.params.PanelID + " #txtComplaintId").val();
        objData["PatientId"] = Clinical_Complaints.params.PatientId;
        objData["ICD9"] = obj.icd9code;
        objData["ICD10"] = obj.icd10code;
        objData["ICD9_Description"] = obj.icd9desc;
        objData["ICD10_Description"] = obj.icd10desc;
        objData["SNOMEDID"] = obj.snomedcode;
        objData["SNOMED_DESCRIPTION"] = obj.snomeddesc;
        //Start for wrong snomed code
        if (Clinical_Complaints.LastSctBaseSearch != "") {
            if (Clinical_Complaints.LastSctBaseSearch == "981000124106" && objData["SNOMEDID"] != "981000124106") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "981000124106")
                }
                objData["SNOMEDID"] = "981000124106";
                objData["SNOMED_DESCRIPTION"] = "Moderate left ventricular systolic dysfunction";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "385093006" && objData["SNOMEDID"] != "385093006") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "385093006")
                }
                objData["SNOMEDID"] = "385093006";
                objData["SNOMED_DESCRIPTION"] = "Community Acquired Pneumonia";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "5281000124103" && objData["SNOMEDID"] != "5281000124103") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "5281000124103")
                }
                objData["SNOMEDID"] = "5281000124103";
                objData["SNOMED_DESCRIPTION"] = "Persistent asthma";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "195967001" && objData["SNOMEDID"] != "195967001") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "195967001")
                }
                objData["SNOMEDID"] = "195967001";
                objData["SNOMED_DESCRIPTION"] = "Asthma";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "233604007" && objData["SNOMEDID"] != "233604007") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "233604007")
                }
                objData["SNOMEDID"] = "233604007";
                objData["SNOMED_DESCRIPTION"] = "Pneumonia";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "59621000" && objData["SNOMEDID"] != "59621000") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "59621000")
                }
                objData["SNOMEDID"] = "59621000";
                objData["SNOMED_DESCRIPTION"] = "Essential hypertension";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "95891005" && objData["SNOMEDID"] != "95891005") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "95891005")
                }
                objData["SNOMEDID"] = "95891005";
                objData["SNOMED_DESCRIPTION"] = "Flu-like symptoms";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "363746003" && objData["SNOMEDID"] != "363746003") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "363746003")
                }
                objData["SNOMEDID"] = "363746003";
                objData["SNOMED_DESCRIPTION"] = "Acute pharyngitis";
            }

            else if (Clinical_Complaints.LastSctBaseSearch == "53741008" && objData["SNOMEDID"] != "53741008") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "53741008")
                }
                objData["SNOMEDID"] = "53741008";
                objData["SNOMED_DESCRIPTION"] = "Coronary arteriosclerosis";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "266569009" && objData["SNOMEDID"] != "266569009") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "266569009")
                }
                objData["SNOMEDID"] = "266569009";
                objData["SNOMED_DESCRIPTION"] = "Benign prostatic hyperplasia";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "49436004" && objData["SNOMEDID"] != "49436004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "49436004")
                }
                objData["SNOMEDID"] = "49436004";
                objData["SNOMED_DESCRIPTION"] = "Atrial fibrillation";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "230572002" && objData["SNOMEDID"] != "230572002") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "230572002")
                }
                objData["SNOMEDID"] = "230572002";
                objData["SNOMED_DESCRIPTION"] = "Diabetic neuropathy";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "5281000124103" && objData["SNOMEDID"] != "5281000124103") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "5281000124103")
                }
                objData["SNOMEDID"] = "5281000124103";
                objData["SNOMED_DESCRIPTION"] = "Persistent asthma";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "981000124106" && objData["SNOMEDID"] != "981000124106") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "981000124106")
                }
                objData["SNOMEDID"] = "981000124106";
                objData["SNOMED_DESCRIPTION"] = "Moderate left ventricular systolic dysfunction";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "426749004" && objData["SNOMEDID"] != "426749004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "426749004")
                }
                objData["SNOMEDID"] = "426749004";
                objData["SNOMED_DESCRIPTION"] = "Chronic atrial fibrillation";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "991000124109" && objData["SNOMEDID"] != "991000124109") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "991000124109")
                }
                objData["SNOMEDID"] = "991000124109";
                objData["SNOMED_DESCRIPTION"] = "Severe left ventricular systolic dysfunction";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "64109004" && objData["SNOMEDID"] != "64109004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "64109004")
                }
                objData["SNOMEDID"] = "64109004";
                objData["SNOMED_DESCRIPTION"] = "Costal Chondritis";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "195967001" && objData["SNOMEDID"] != "195967001") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "195967001")
                }
                objData["SNOMEDID"] = "195967001";
                objData["SNOMED_DESCRIPTION"] = "Asthma";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "53741008" && objData["SNOMEDID"] != "53741008") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "53741008")
                }
                objData["SNOMEDID"] = "53741008";
                objData["SNOMED_DESCRIPTION"] = "Coronary arteriosclerosis";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "266569009" && objData["SNOMEDID"] != "266569009") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "266569009")
                }
                objData["SNOMEDID"] = "266569009";
                objData["SNOMED_DESCRIPTION"] = "Benign prostatic hyperplasia";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "49436004" && objData["SNOMEDID"] != "49436004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "49436004")
                }
                objData["SNOMEDID"] = "49436004";
                objData["SNOMED_DESCRIPTION"] = "Atrial fibrillation";
            }
            else if (Clinical_Complaints.LastSctBaseSearch == "230572002" && objData["SNOMEDID"] != "230572002") {
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

        if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote") {
            objData["NoteId"] = Clinical_Complaints.params.NotesId;
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
                        Clinical_Complaints.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', ProblemListsId, hideAlertMessage, ComplaintDetailId);
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
                $SectionBodyVital.attr('complaintId', Clinical_Complaints.ComplaintId);
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
                //Start//04//01//2015//Ahmad Raza//
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

        if ($('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #radioFreetext").prop("checked") == true) {
            $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtComplaints").addClass('hidden');
            $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #btnSearchCPT").addClass('hidden');
            $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtFreeText").removeClass('hidden');
        } else {
            $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtComplaints").removeClass('hidden');
            $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #btnSearchCPT").removeClass('hidden');
            $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtFreeText").addClass('hidden');
        }
        Clinical_Complaints.SaveFreeTextStatus();


    },
    createFreeTextLi: function () {
        var currId = -1;
        var diseaseText = $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtFreeText").val().trim();

        if (diseaseText != null && diseaseText != '') {
            $("#pnlClinicalComplaints #frmClinicalComplaints ul#ulCompliantDisease li[id*='-']").each(function (i, item) {
                currId = $(this).attr("id");
            });
            currId = parseInt(currId) + (-1);

            var IsAlreadyExist = false;
            $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li").each(function () {
                if ($(this).text().trim() == diseaseText.trim()) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                var li = "<li id='" + currId + "' freetext='" + diseaseText + "' onclick='Clinical_Complaints.fillChiefComplaints(this, event);'>" +
                    "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + diseaseText + "</span></a>" +
                        "<span class='removeIconListHover'>" +
                            "<a href='#' onclick='Clinical_Complaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                "<a href='#' onclick='Clinical_Complaints.deleteChiefComplaint(" + currId + ", event);'><i class='fa fa-times red'></i></a>" +
		                "</span>" +
                    "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_Complaints.updateComplaintsText(this, event);'/>" +
                    "<div class='clearfix'></div>" +
                "</li>";
                $('#pnlClinicalComplaints #ulCompliantDisease').append(li);
                Clinical_Complaints.AddInArray(currId, diseaseText, true);
                $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtFreeText").val('');
                Clinical_Complaints.IsUpdate = true;
            }
            else {
                utility.DisplayMessages('Disease already added', 2);
            }

        }
        else {
            $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #txtFreeText").val("");
            utility.DisplayMessages('Please enter valid text.', 2);
        }
    },
    // Start 4/1/2016 Muhammad Ahmad Imran
    //Purpose Save/update favList Status
    SaveFreeTextStatus: function () {
        if (Clinical_Complaints.params.ParentCtrl == "clinicalTabProgressNote") {
            panel = "#pnlClinicalProgressNote #pnlClinicalComplaints";
        }
        else {
            panel = "#pnlClinicalComplaints";
        }
        var IsFreeText = false;
        $('input[name=radioFreetext]:checked', panel).val() == "freetext" ? IsFreeText = true : IsFreeText = false;
        EMRUtility.insertUpdateFreeTextStatus("Clinical_Complaints", IsFreeText);
    },

    loadNoteComponent: function () {
        UnloadActionPan(Clinical_Complaints.params.ParentCtrl, 'Clinical_Complaints');
        if (Clinical_Complaints.controlToInvoke && Clinical_Complaints.controlToInvoke != false) {
            setTimeout(function () {
                Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Complaints.controlToInvoke.replace(/\s/g, ''));
                Clinical_Complaints.controlToInvoke = null;
            }, 600);
        }
    },

    editComplaintsText: function (obj, event) {
        if (event) {
            event.stopPropagation();
        }
        Clinical_Complaints.IsUpdate = true;
        var li = $(obj).closest('li');
        var desc = li.find(".complaint-text").text();
        li.find(".complaint-text").addClass("hidden");
        li.find(".removeIconListHover").addClass("hidden");
        li.find(".edit-complaint").removeClass("hidden").val(desc).focus();
        $('#' + Clinical_Complaints.params.PanelID + " #btnClose").addClass('disableAll');
        $('#' + Clinical_Complaints.params.PanelID + " #btnAdd").addClass('disableAll');
    },

    updateComplaintsText: function (obj, event) {
        if (event) {
            event.stopPropagation();
        }
        var li = $(obj).closest('li');
        var ComplainDetailId = li.attr('id');
        if ($(obj).val().trim() == "") {

            utility.myConfirm('1', function () {
                Clinical_Complaints.deleteChiefComplaintConfirmed(ComplainDetailId, event);
            }, function () {
                $(obj).addClass("hidden").closest('li').find(".complaint-text").removeClass("hidden");
                li.find(".removeIconListHover").removeClass("hidden");
            });

            $('#' + Clinical_Complaints.params.PanelID + " #btnClose").removeClass('disableAll');
            $('#' + Clinical_Complaints.params.PanelID + " #btnAdd").removeClass('disableAll');
        } else {
            var IsAlreadyExist = false;
            $('#' + Clinical_Complaints.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li").each(function () {
                if (ComplainDetailId != $(this).attr("id") && $(this).text().trim() == $(obj).val().trim()) {
                    IsAlreadyExist = true;
                }
            });
            if (!IsAlreadyExist) {
                $(obj).addClass("hidden").closest('li').find(".complaint-text").removeClass("hidden").text($(obj).val());
                li.find(".removeIconListHover").removeClass("hidden");
                $.grep(Clinical_Complaints.Complaints, function (item, index) {
                    if (item.ComplaintDetailId == ComplainDetailId) {
                        item.ComplaintDescription = $(obj).val();
                        return;
                    }
                });
                $.grep(Clinical_Complaints.ComplaintsDetail, function (item, index) {
                    if (item.ComplaintDetailId == ComplainDetailId) {
                        item.ComplaintDescription = $(obj).val();
                        return;
                    }
                });
                $('#' + Clinical_Complaints.params.PanelID + " #btnClose").removeClass('disableAll');
                $('#' + Clinical_Complaints.params.PanelID + " #btnAdd").removeClass('disableAll');
            } else {
                utility.DisplayMessages('Disease already added', 2);
                li.find(".edit-complaint").focus();
            }
        }
    },
}
