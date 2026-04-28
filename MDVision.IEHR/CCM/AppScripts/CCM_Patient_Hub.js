CCM_Patient_Hub = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    icdsValues: {},
    LastSctBaseSearch: '',
    FavListName: "ClinicalProblemList",
    IsProblemFirstLoad: true,
    IshubChanged: false,

    Load: function (params) {
        CCM_Patient_Hub.params = params;
        if (CCM_Patient_Hub.params != null && CCM_Patient_Hub.params.PanelID != "pnlCCM_Patient_Hub") {
            CCM_Patient_Hub.params["PanelID"] = CCM_Patient_Hub.params["PanelID"] + ' #pnlCCM_Patient_Hub';
        } else {
            CCM_Patient_Hub.params["PanelID"] = "pnlCCM_Patient_Hub";
        }
        if (CCM_Patient_Hub.params.PatientId == undefined || CCM_Patient_Hub.params.PatientId == "undefined" || CCM_Patient_Hub.params.PatientId == null || CCM_Patient_Hub.params.PatientId == "")
            CCM_Patient_Hub.params.PatientId = CCM_Patient_Hub.params.patientID;

        if (CCM_Patient_Hub.bIsFirstLoad) {
            CCM_Patient_Hub.bIsFirstLoad = false;
        }
        var self = $('#' + CCM_Patient_Hub.params.PanelID + " #divPatientHub");

        var This = $('#pnlCCM_Patient_Hub');
        var data = "IsActive=&ID=" + CCM_Patient_Hub.params.EnrollmentInfoId;

        if (CCM_Patient_Hub.params.TabID == "patTabDemographic") {
            self.loadDropDownsWithTitle(true, data).done(function () {

                self.loadDropDowns(true).done(function () {
                    if (CCM_Patient_Hub.params.IsFromNote) {
                        CCM_Patient_Hub.ShowPanel("ProgramUpdates", 1);
                    } else {
                        CCM_Patient_Hub.ShowPanel('PatientHub');
                    }
                });
            });
        }
        else {
            self.loadDropDowns(true, data).done(function () {
                self.loadDropDowns(true).done(function () {
                    if (CCM_Patient_Hub.params.IsFromNote) {
                        CCM_Patient_Hub.ShowPanel("ProgramUpdates", 1);
                    } else {
                        CCM_Patient_Hub.ShowPanel('PatientHub');
                    }
                });
            });
        }

        $('#pnlCCM_Patient_Hub #ddlProvider').change(function () {
            var providerId = $('#pnlCCM_Patient_Hub #ddlProvider option:selected').attr('value');
            CCM_Patient_Hub.CareTeamSelect(providerId);
        });

        if (!$("#txtfinalScore").hasClass("form-control")) {
            $("#txtfinalScore").addClass("form-control");
            $("#txtfinalScore").attr("disabled", "disabled");
        }

        CCM_Patient_Hub.IshubChanged = false;
    },

    checkDecimal: function (Score) {
        var ex = /^[0-9]+\.?[0-9]*$/;
        if (ex.test(Score) == false) {
            Score = Score.substring(0, Score.length - 1);
        }
    },

    SetControlValue: function (Obj, Id, RiskAssessmentId) {
        $("#txt" + RiskAssessmentId).attr('value', $("#txt" + RiskAssessmentId).val());
        var Score = $("#txt" + RiskAssessmentId).val();
        var objData = new Object();
        objData["RiskAssessTemptId"] = Id;
        objData["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
        objData["AssessHTML"] = "";
        objData["RiskScore"] = Score;

        if ($.isNumeric(Score)) {
            //    CCM_Patient_Hub.PatientHubRiskAssessmentScoreTemplateInsertUpadte(objData).done(function (response) {
            //        if (response.status != false) {
            if (Score.indexOf('.') > 0) {
                if (Score.split(".")[1].length > 2) {
                    $("#txt" + RiskAssessmentId).val(Score.substring(0, Score.length - 1));
                }
            }
            CCM_Patient_Hub.SetFinalScore();
            //}
            //else
            //    $("div_" + Id).remove();
            //});
        } else {
            if (Score) {
                $("#txt" + RiskAssessmentId).val(Score.substring(0, Score.length - 1));
                if (Score.length == 0)
                    $("#txtfinalScore").val('');
                CCM_Patient_Hub.SetFinalScore();
            } else {
                $("#txtfinalScore").val('');
            }
        }
        CCM_Patient_Hub.SetFinalScore();
    },

    SetFinalScore: function myfunction() {
        var finalScore = parseFloat(0);
        for (var i = 0; i < $("#divstoAppend input").length - 1; i++) {
            var vall = $($("#divstoAppend input")[i]).val();
            vall = vall == "" ? 0 : vall;
            finalScore = (parseFloat(finalScore) + parseFloat(vall));
        }
        $("#txtfinalScore").val(finalScore.toFixed(2));
        $("#txtfinalScore").attr("disabled", "disabled");
        CCM_Patient_Hub.IshubChanged = true;
    },

    AddTemplateField: function (Id, Name, riskScore_) {
        if (Name != '- Select -') {
            var objData = new Object();
            objData["RiskAssessTemptId"] = Id;
            objData["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
            objData["RiskScore"] = $("#txt" + Id).val();

            if (objData["RiskScore"] == undefined || objData["RiskScore"] == 'undefined' || objData["RiskScore"] == null || objData["RiskScore"] == "")
                objData["RiskScore"] = 0.0;

            riskScore_ = riskScore_ == "" ? 0 : riskScore_;
            if ($("#divstoAppend").find("#div_" + Id).length <= 0) {
                var RiskAssessmentId = Id;
                $("#divfinalScores").remove();
                $("#divstoAppend").append('<div class="col-sm-12" id=div_' + RiskAssessmentId + ' onmouseover="CCM_Patient_Hub.showIcon(this, ' + RiskAssessmentId + ');" onmouseout="CCM_Patient_Hub.hideIcon(this, ' + RiskAssessmentId + ');"><label for="" class="control-label" id=lbl' + RiskAssessmentId + '>' +
                                          Name + '</label><a href="#" id=anchor' + RiskAssessmentId + ' onclick="CCM_Patient_Hub.DeletePatientHubRiskAssessmentScoreTemplate(' + RiskAssessmentId + ')" class="removeIconListHover"><i class="fa fa-times red"></i></a><input id=txt' +
                                          RiskAssessmentId + ' type="text" oninput="CCM_Patient_Hub.SetControlValue(this, ' + Id + ',' + RiskAssessmentId + ');" class="form-control" value=' + riskScore_ + ' /></div>');
                $("#anchor" + RiskAssessmentId).hide();

                var htmlFinalScore = '<div class="col-sm-12" id="divfinalScores"><label for="" class="control-label">Final Score</label><input type="text" id="txtfinalScore" name="finalScore" class="form-control" value=""></div>';
                $("#divstoAppend").append(htmlFinalScore);
                CCM_Patient_Hub.SetFinalScore();
                CCM_HRAssessment.searchHRAssessment();
            }
        }
    },

    //PatientHubRiskAssessmentScoreTemplateDelete: function (objData) {

    //    var data = JSON.stringify(objData);
    //    return MDVisionService.CCMAPIService(data, "Patient_Hub", "DeleteRiskAssessmentScoreTemplate");
    //},

    DeletePatientHubRiskAssessmentScoreTemplate: function (RiskAssessmentId) {
        var objData = new Object();
        objData["RiskAssessmentId"] = RiskAssessmentId;

        CCM_Patient_Hub.PatientHubRiskAssessmentScoreTemplateDelete(objData).done(function (response) {
            if (response.status != false) {
                $("#div_" + RiskAssessmentId).remove();
                utility.DisplayMessages("Successfully Deleted", 3);

                if (CCM_HRAssessment != null)
                    CCM_HRAssessment.searchHRAssessment();

                if ($($("#divstoAppend div[id^='div_'] a")).length == 0)
                    $("#divfinalScores").remove();
                else
                    CCM_Patient_Hub.SetFinalScore();
            }
        });
    },

    PatientHubRiskAssessmentScoreTemplateDelete: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "DeleteRiskAssessmentScoreTemplate");
    },

    ShowPanel: function (tabCCM) {
        $('#' + CCM_Patient_Hub.params["PanelID"] + ' div[name="CCM_Module"]').hide();
        $('#' + CCM_Patient_Hub.params["PanelID"] + ' #div' + tabCCM).show();
        switch (tabCCM) {
            case "PatientHub":
                CCM_Patient_Hub.LoadPatientHub(this);
                $("#tabCCM_ a").removeClass('active');
                $("#tabCCMHub").addClass('active');
                break;
            case "HRA":
                CCM_HRAssessment.HRAssessmentLoad();
                $("#tabCCM_ a").removeClass('active');
                $("#tabCCMHRA").addClass('active');
                break;
            case "Problems":
                CCM_Patient_Hub.LoadProblem();
                $("#tabCCM_ a").removeClass('active');
                $("#tabCCMProblems").addClass('active');
                break;
            case "CarePlan":
                CCM_CarePlan.CarePlanLoad();
                $("#tabCCM_ a").removeClass('active');
                $("#tabCCMCarePlan").addClass('active');
                break;
            case "ProgramUpdates":
                CCMProgramUpdate.Load(CCM_Patient_Hub.params, 1);
                $("#tabCCM_ a").removeClass('active');
                $("#tabCCMProgramUpdate").addClass('active');
                break;
            default:
                utility.DisplayMessages("Please select CCM module", 2);
        }
    },

    showIcon: function (obj, Id) {
        $(obj).find('div').css('display', '');
        $("#anchor" + Id).show();
    },

    hideIcon: function (obj, Id) {
        if ($(obj).hasClass("active") == false)
            $(obj).find('div').css('display', 'none');

        $("#anchor" + Id).hide();
    },

    LoadProblem: function () {
        if (CCM_Patient_Hub.IsProblemFirstLoad) {
            CCM_Patient_Hub.IsProblemFirstLoad = false;
            var self = $('#' + CCM_Patient_Hub.params["PanelID"]);
            //---problems
            CCM_Patient_Hub.domReadyFunc();
            //for Favorites toggle
            EMRUtility.setFavoriteSectionStyle(CCM_Patient_Hub.params.PanelID);

            utility.ValidateFromToDate('frmClinicalProblemLists', 'dpStartDate', 'dpEndDate', true);

            CCM_Patient_Hub.ProblemListsSearch();
            self.loadDropDowns(true).done(function () {
                CCM_Patient_Hub.ValidateProblemLists();
                //Serialization
                $('#' + CCM_Patient_Hub.params.PanelID + ' #frmClinicalProblemLists').data('serialize', $('#' + CCM_Patient_Hub.params.PanelID + ' #frmClinicalProblemLists').serialize());

                //for favorite list setting for all favLists
                if (EMRUtility.getFavListStatus(CCM_Patient_Hub.FavListName))
                    $('#' + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists #favSectionDiv").removeClass("toggled");
                else
                    $('#' + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists #favSectionDiv").addClass("toggled");
                //end for favorite list setting for all favLists
            });
            CCM_Patient_Hub.params.ProblemMode = "Add";
        }
    },

    //----------------------------------------------------//
    //-------------Problems Functions Start---------------//
    //----------------------------------------------------//
    ProblemListsSearch: function (ProblemListId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Problems List", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                CCM_Patient_Hub.SearchProblemList(ProblemListId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $("#" + CCM_Patient_Hub.params.PanelID + " #dgvProblemLists th#SelectRecord").remove();

                        CCM_Patient_Hub.ProblemListGridLoadNew(response);
                        var TableControl = CCM_Patient_Hub.params.PanelID + " #dgvProblemLists";
                        var PagingPanelControlID = CCM_Patient_Hub.params.PanelID + " #dgvProblemLists_Paging";
                        var ClassControlName = "CCM_Patient_Hub";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ProblemListCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            CCM_Patient_Hub.ProblemListsSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                    } else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SearchProblemList: function (ProblemListId, PageNumber, RowsPerPage, getInActiveProblems) {
        var IsCheckedIn = null;
        IsCheckedIn = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }

        // Patch for getting InActiveProblems // fixMe
        if (getInActiveProblems)
            IsCheckedIn = getInActiveProblems;

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["PatientId"] = CCM_Patient_Hub.params.PatientId;
        objData["IsActive"] = IsCheckedIn;
        objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        objData["NoteId"] = 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    ProblemListGridLoadNew: function (response) {
        var isactive = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');

        // get Actions
        var actions = "";
        $("#" + CCM_Patient_Hub.params.PanelID + " #dgvProblemLists tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, "#" + CCM_Patient_Hub.params.PanelID + " #pnlProblemLists_Result");
                }
            }
        });
        if ($.fn.dataTable.isDataTable("#" + CCM_Patient_Hub.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists")) {
            $("#" + CCM_Patient_Hub.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnClearTable();
            $("#" + CCM_Patient_Hub.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnDestroy();
            $("#" + CCM_Patient_Hub.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists tbody").find("tr").remove();
        }
        var totalProblems = 0;
        if (response.ProblemListCount > 0) {
            totalProblems = response.ProblemListCount;
            var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
            var ProblemListHistoryLoadJSONData = JSON.parse(response.ProblemListHistoryLoad_JSON);

            if ($.fn.dataTable.isDataTable("#" + CCM_Patient_Hub.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists")) {
                $("#" + CCM_Patient_Hub.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnDestroy();
            }
            //tem array to hold rows and childs
            var arraTemp = [];

            $.each(ProblemListLoadJSONData, function (i, item) {
                var $infoButtonrow = "";
                if (item.Description != "") {
                    if (typeof item.Description !== 'undefined' && typeof item.Description.split('-')[1] !== 'undefined') {
                        var searchstr = item.Description.split('-')[1].trim();
                        if (Clinical_InfoButtonView != null) {
                            $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(searchstr, "CCM_Patient_Hub", 2);
                        }
                    }
                }
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.ProblemListId);
                $row.attr("ProblemListNotesId", item.NoteId);

                $row.attr("name", "Problems");

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

                if (item.IsActive.toLowerCase() == "false") {
                    if (item.InActiveReason.length > 0) {
                        comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.InActiveChkBoxValue + ": " + item.InActiveReason + '"><i class="fa fa-commenting blue"></i></label>';
                    } else {
                        comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.InActiveChkBoxValue + '"><i class="fa fa-commenting blue"></i></label>';
                    }
                } else {
                    if (item.Comments != "") {
                        var commentsMethod = "CCM_Patient_Hub.AddComments('" + item.ProblemListId + "');";
                        //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                        comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                    }
                }
                var ProblemListId = item.ProblemListId;
                var ChildHistory_ProblemList = $.grep(ProblemListHistoryLoadJSONData, function (n, i) {
                    return n.ProblemId == ProblemListId;
                });
                if (CCM_Patient_Hub.params.ParentCtrl == "clinicalTabProgressNote" && item.ProblemName == "No Known Problems") {
                    //$row.append('<td></td><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + item.ProblemName + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                    //Start//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row
                    var Checked = "";
                    if (item.IsNoteLinked == "True") {
                        if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                            Checked = " ";
                        } else {
                            Checked = " checked";
                        }
                    } else {
                        if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                            Checked = " checked";
                        } else {
                            Checked = "";
                        }
                    }

                    if (ChildHistory_ProblemList.length > 0) {
                        $row.append('<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none"></td>');
                    } else {
                        $row.append('<td></td><td></td><td>' + item.ProblemName + $infoButtonrow + '</a></td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none"></td>');
                    }
                    //End//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row
                } else {
                    if (item.ProblemName == "No Known Problems") {
                        $row.append('<td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + item.ProblemName + $infoButtonrow + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none"></td>');
                    } else {
                        //adding checkboxes column and disabling that row, if problem list already binded with notes
                        var Checked = "";
                        if (item.IsNoteLinked == "True") {
                            if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                                Checked = " ";
                            } else {
                                Checked = " checked";
                            }
                        } else {
                            if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                                Checked = " checked";
                            } else {
                                Checked = "";
                            }
                        }

                        if (ChildHistory_ProblemList.length > 0) {
                            $row.append('<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions" id="' + item.ProblemListId + '" >' + actions + '</td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none"></td>');
                        } else {
                            $row.append('<td></td><td class="actions" id="' + item.ProblemListId + '" >' + actions + '</td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none"></td>');
                        }
                    }
                }
                if (item.IsActive == "True") {
                    // $($row).find('a.edit-row').removeAttr('disabled', false);
                    $($row).find('a.edit-row').removeClass('disableAll')
                } else {
                    $($row).find('a.edit-row').addClass('disableAll')
                    //  $($row).find('a.edit-row').attr('disabled', 'disabled')
                }

                $("#" + CCM_Patient_Hub.params.PanelID + " #dgvProblemLists tbody").last().append($row);
                if (totalProblems > 1) {
                    $('#' + CCM_Patient_Hub.params.PanelID + ' #dgvProblemLists tbody tr').each(function () {
                        if ($(this).text().indexOf("No Known Problems") > -1) {
                            $(this).remove();
                        }
                    });
                }

                var CurrentRowchilds = $();

                if (ChildHistory_ProblemList.length > 0) {
                    $.each(ChildHistory_ProblemList, function (i, item) {
                        // if (item.ProblemId == ProblemListId) {
                        //arrProblemListHistory.push(item);
                        comments = "";
                        if (item.IsActive.toLowerCase() == "false") {
                            if (item.InActiveReason.length > 0) {
                                comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.InActiveChkBoxValue + ": " + item.InActiveReason + '"><i class="fa fa-commenting blue"></i></label>';
                            } else {
                                comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.InActiveChkBoxValue + '"><i class="fa fa-commenting blue"></i></label>';
                            }
                        } else {
                            var commentsMethod = "CCM_Patient_Hub.AddComments('" + item.ProblemListId + "');";
                            //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                            comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                        }

                        var Title_Tooltip = "Inactive Reason: " + item.InActiveChkBoxValue + (item.EndDate != '' ? " <br/>End Date: " + utility.RemoveTimeFromDate(null, item.EndDate) : "") + (item.InActiveReason != '' ? " <br/>Comments: " + item.InActiveReason : "");
                        var IsActiveText = "";
                        if (item.IsActive == "True") {
                            IsActiveText = "<Label>[Active]</Label>";
                        } else {
                            IsActiveText = "<Label data-toggle='tooltip' data-placement='right' title='" + Title_Tooltip + "'>[Inactive]</Label>";
                        }

                        // Irfan Change color of severity

                        var colorChild = "";

                        if (item.Severity == "Mild Intermittent" || item.Severity == "Mild Persistent") {
                            colorChild = 'style = "color:green;font-weight:bold"'
                        }
                        if (item.Severity == "Severe Persistent" || item.Severity == "Unspecified Severity") {
                            colorChild = 'style = "color:red;font-weight:bold"'
                        }
                        if (item.Severity == "Moderate Persistent") {
                            colorChild = 'style = "color:orange;font-weight:bold"'
                        }

                        // end Change color of severity

                        //Start Row Sequence issue in History Grid, fixed
                        var currentHistory = '<tr class="childRow-bg"><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + IsActiveText + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + colorChild + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none"></td></tr>';

                        //End Row Sequence issue in History Grid, fixed

                        CurrentRowchilds = CurrentRowchilds.add(currentHistory);
                    });
                }

                if (CurrentRowchilds.length > 0) {
                }

                arraTemp.push({ row: $row, childs: CurrentRowchilds });
            });

            //Inalize grid
            var PanelGrid = "#" + CCM_Patient_Hub.params.PanelID + " #pnlProblemLists_Result";
            var GridId = "#" + CCM_Patient_Hub.params.PanelID + " #dgvProblemLists";

            // Below line comment out inorder to remove duplicate grid search
            if (CCM_Patient_Hub.myGrid != null) {
                if ($.fn.dataTable.isDataTable(CCM_Patient_Hub.myGrid)) {
                    CCM_Patient_Hub.myGrid.$table.dataTable().fnDestroy();
                } else {
                    CCM_Patient_Hub.myGrid = null;
                }
                if ($.fn.dataTable.isDataTable("#" + CCM_Patient_Hub.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists")) {
                    $("#" + CCM_Patient_Hub.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnDestroy();
                }
            }
            // Below line comment out inorder to remove duplicate grid search
            CCM_Patient_Hub.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, CCM_Patient_Hub, 0, false, true, false, true, false, null);
            $(PanelGrid).css('display', '');
            //rander childs
            $.each(arraTemp, function (i, item) {
                if (CCM_Patient_Hub.myGrid != null) {
                    var row = CCM_Patient_Hub.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    } else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }
            });
            //Sorting removed from first column of grid
            if ($('#' + CCM_Patient_Hub.params.PanelID + ' #dgvProblemLists').length > 0) {
                $('#' + CCM_Patient_Hub.params.PanelID + ' #dgvProblemLists').dataTable().fnSettings().aoColumns[0].bSortable = false;
                $('#' + CCM_Patient_Hub.params.PanelID + ' #dgvProblemLists').dataTable().fnSort([[8, "desc"]]);
            }
            //Sorting removed from first column of grid

            /* Start of Code for making No Known Problem List hyperlink inline with checkbox and search box.
             *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
             */
            var checked = '';
            if (isactive == "0") {
            } else if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "1";
                checked = 'checked="checked"';
            }

            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                               '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="CCM_Patient_Hub.ActiveProblemSearch(this);">' +
                               '</div><span class="pl-xs">Active</span>' +
                               '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="CCM_Patient_Hub.NoKnownProblem();">No Known Problems</a>';

            $("#" + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        } else {
            if ($('#' + CCM_Patient_Hub.params.PanelID + ' div#divShowHistory').hasClass("hidden") == false) {
                $('#' + CCM_Patient_Hub.params.PanelID + ' div#divShowHistory').addClass("hidden");
            }

            $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result #dgvProblemLists').DataTable({
                "language": {
                    "emptyTable": "No Problem List Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
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
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                               '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="CCM_Patient_Hub.ActiveProblemSearch(this);">' +
                               '</div><span class="pl-xs">Active</span>' +
                               ' <a id="btnNoKnownProblems" class="ml-md btn btn-link btn-xs" style="display:none" onclick="CCM_Patient_Hub.NoKnownProblem();">No Known Problems</a>';

            $("#" + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
            if (response.ProblemListCount == 0 && $("#switchActive").attr('isactive') != "0") {
                $("#pnlProblemLists_Result #btnNoKnownProblems").css("display", "");
            } else {
                $("#pnlProblemLists_Result #btnNoKnownProblems").hide();
            }
            /* End of Code for making No Known Problem List hyperlink inline with checkbox and search box.
             *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
             */
        }

        EMRUtility.SwicthWidgetInializatoin();
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        $('#' + CCM_Patient_Hub.params.PanelID + ' #dgvProblemLists thead tr th:first-child').removeClass('sorting_asc');
    },
    //Comments Update
    AddComments: function (ProblemListId) {
        var params = [];
        params["ParentCtrl"] = 'CCM_Patient_Hub';
        params["ProblemListId"] = ProblemListId;
        params["FromAdmin"] = "0";
        params["PatientId"] = CCM_Patient_Hub.params.PatientId;
        LoadActionPan('Clinical_ProblemListsComments', params);
    },
    //
    ActiveProblemSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        } else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        CCM_Patient_Hub.ProblemListsSearch();
    },

    ValidateProblemLists: function () {
        $('#' + CCM_Patient_Hub.params.PanelID + ' #frmClinicalProblemLists')
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
                CCM_Patient_Hub.ProblemListsSave();
            });
    },

    SaveFavToggelStatus: function () {
        var isFavListOpened = $('#' + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists #favSectionDiv").hasClass("toggled");
        EMRUtility.insertUpdateFavListStatus(CCM_Patient_Hub.FavListName, isFavListOpened);
    },

    ProblemListsSave: function () {
        var tPlist = $("#" + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists #txtProblems").val();

        if (tPlist != null && tPlist.indexOf('-') > -1) {
            var strArray = tPlist.split('-');
            $("#" + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists #txtProblems").val(strArray[strArray.length - 1].trim());
        }

        $("#" + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists").bootstrapValidator('revalidateField', 'ProblemName');
        if ($("#" + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists #txtProblems").val() != "") {
            var strMessage = "";
            $("#" + CCM_Patient_Hub.params.PanelID + " #hfPatientId").val(CCM_Patient_Hub.params.PatientId);
            var self = $("#" + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists");
            var myJSON = self.getMyJSONByName();
            var problem = $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").val();

            //var problemExists = false;
            //if ($("#" + CCM_Patient_Hub.params.PanelID + " #dgvProblemLists tbody tr").text().indexOf(problem) > -1) {
            //    problemExists = true;
            //}
            //if (problemExists == true) {
            //    utility.DisplayMessages("Problem Already Exists.", 3);
            //    return;
            //}

            var problemExists = false;
            var ICDCodeANDDescription = "";
            var icdCode = "";
            var icdDescription = "";
            var problemCode = "";
            if (problem) if (problem.indexOf('-') > 1) problemCode = problem.split('-')[0].trim();

            var ArrayProblems = new Array();
            var columnIndex = $(" #dgvProblemLists thead tr th:contains('ICD (Diagnosis)')").index() + 1;

            $("#" + CCM_Patient_Hub.params.PanelID + " #dgvProblemLists tbody tr td:nth-child(" + columnIndex + ")").each(function (i) {

                if ($(this).text()) {
                    ICDCodeANDDescription = $(this).text();
                    if (ICDCodeANDDescription.indexOf('-') > 1) {
                        icdCode = ICDCodeANDDescription.split('-')[0].trim();
                        icdDescription = ICDCodeANDDescription.split('-')[1].trim();
                        ArrayProblems.push(icdCode);
                    }
                }
                else
                    if ($("#" + CCM_Patient_Hub.params.PanelID + " #dgvProblemLists tbody tr").text().indexOf(problem) > -1)
                        problemExists = true;
            });

            var IsThisActive = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive') == null ? "1" : "0";

            CCM_Patient_Hub.SearchProblemList(null, null, null, IsThisActive).done(function (response) {
                var qresponse = JSON.parse(response);
                if (qresponse.status != false) {
                    if (qresponse.ProblemListLoad_JSON) {
                        response = JSON.parse(qresponse.ProblemListLoad_JSON);
                        $.each(response, function (i, item) {
                            ArrayProblems.push(item.ICD10);
                        });

                        if (ArrayProblems.includes(problemCode))
                            problemExists = true;

                        if (problemExists == true) {
                            utility.DisplayMessages("Problem Already Exists.", 3);
                            return;
                        }
                    }
                }


                //End 25-08-2016 Humaira Yousaf to preventing duplicate problem for referrals
                //return false;
                if (CCM_Patient_Hub.params.ProblemMode == "Add") {
                    var hfProblemText = $("#" + CCM_Patient_Hub.params.PanelID + " #hfIMOProblem").val();
                    var changesProblemText = $("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val();
                    if (hfProblemText.toString() == changesProblemText.toString()) {
                        AppPrivileges.GetFormPrivileges("Medical_Problems List", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                CCM_Patient_Hub.SaveProblemLists(myJSON).done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {
                                        CCM_Patient_Hub.SaveFavToggelStatus();
                                        CCM_Patient_Hub.LastSctBaseSearch = "";
                                        $('#' + CCM_Patient_Hub.params.PanelID + ' #ulFavCompliantDisease li').remove();

                                        CCM_Patient_Hub.ProblemListsSearch();

                                        utility.DisplayMessages(response.message, 1);
                                        $('#' + CCM_Patient_Hub.params.PanelID + ' #frmClinicalProblemLists').resetAllControls(null);
                                        $("#" + CCM_Patient_Hub.params.PanelID + " #hfPatientId").val(CCM_Patient_Hub.params.PatientId);
                                        $('#' + CCM_Patient_Hub.params.PanelID + ' #frmClinicalProblemLists').data('bootstrapValidator').enableFieldValidators('ProblemName', true);
                                        $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate,#txtComments").prop("disabled", true);
                                        $("#" + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease").empty();
                                        $('#' + CCM_Patient_Hub.params.PanelID + ' #frmClinicalProblemLists').data('serialize', $('#' + CCM_Patient_Hub.params.PanelID + ' #frmClinicalProblemLists').serialize());

                                        Clinical_ProblemLists.AddProblemOnDrFirst(response.ProblemListId);
                                    } else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            } else
                                utility.DisplayMessages(strMessage, 2);
                        });
                    } else {
                        utility.DisplayMessages("Please Enter Valid Problem", 3);
                        $("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val('');
                        $('#' + CCM_Patient_Hub.params.PanelID + ' #frmClinicalProblemLists').data('bootstrapValidator').enableFieldValidators('ProblemName', true);
                    }
                } else if (CCM_Patient_Hub.params.ProblemMode == "Edit") {
                    CCM_Patient_Hub.UpdateProblemLists(myJSON, CCM_Patient_Hub.params.ProblemListId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            CCM_Patient_Hub.LastSctBaseSearch = "";
                            utility.DisplayMessages(response.message, 1);
                        } else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                }
            });
        }
    },

    UpdateProblemLists: function (ProblemListsData, ProblemListId) {
        var isactive = null;
        isactive = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(ProblemListsData);
        if (objData.PatientId == '') {
            objData.PatientId = CCM_Patient_Hub.params.patientID;
        }
        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_VITALS";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "Vitals");
    },

    SaveProblemLists: function (ProblemListsData) {
        var objData = JSON.parse(ProblemListsData);
        if (objData.PatientId == '' || typeof objData.PatientId == 'undefined') {
            objData.PatientId = CCM_Patient_Hub.params.patientID;
        }
        /*objData["ICD9"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("icd9code");
        objData["ICD10"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("icd10code");
        objData["ICD9_Description"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("icd9desc");
        objData["ICD10_Description"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("icd10desc");
        objData["SNOMEDID"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("snomedcode");
        objData["SNOMED_DESCRIPTION"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("snomeddesc");*/
        //-------------------------
        if (CCM_Patient_Hub.icdsValues != null && typeof CCM_Patient_Hub.icdsValues["ICD9"] != 'undefined'
                                                         && CCM_Patient_Hub.icdsValues["ICD9"] != null && CCM_Patient_Hub.icdsValues["ICD9"] != '') {
            objData["ICD9"] = CCM_Patient_Hub.icdsValues["ICD9"];
            objData["ICD10"] = CCM_Patient_Hub.icdsValues["ICD10"];
            objData["ICD9_Description"] = CCM_Patient_Hub.icdsValues["ICD9_Description"];
            objData["ICD10_Description"] = CCM_Patient_Hub.icdsValues["ICD10_Description"];
            objData["SNOMEDID"] = CCM_Patient_Hub.icdsValues["SNOMEDID"];
            objData["SNOMED_DESCRIPTION"] = CCM_Patient_Hub.icdsValues["SNOMED_DESCRIPTION"];
        } else {
            objData["ICD9"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("icd9code");
            objData["ICD10"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("icd10code");
            objData["ICD9_Description"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("icd9desc");
            objData["ICD10_Description"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("icd10desc");
            objData["SNOMEDID"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("snomedcode");
            objData["SNOMED_DESCRIPTION"] = $('#' + CCM_Patient_Hub.params.PanelID + " #ulProblemDisease li").attr("snomeddesc");
        }

        //Start for wrong snomed code
        if (CCM_Patient_Hub.LastSctBaseSearch != "") {
            if (CCM_Patient_Hub.LastSctBaseSearch == "981000124106" && objData["SNOMEDID"] != "981000124106") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "981000124106")
                }
                objData["SNOMEDID"] = "981000124106";
                objData["SNOMED_DESCRIPTION"] = "Moderate left ventricular systolic dysfunction";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "385093006" && objData["SNOMEDID"] != "385093006") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "385093006")
                }
                objData["SNOMEDID"] = "385093006";
                objData["SNOMED_DESCRIPTION"] = "Community Acquired Pneumonia";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "5281000124103" && objData["SNOMEDID"] != "5281000124103") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "5281000124103")
                }
                objData["SNOMEDID"] = "5281000124103";
                objData["SNOMED_DESCRIPTION"] = "Persistent asthma";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "195967001" && objData["SNOMEDID"] != "195967001") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "195967001")
                }
                objData["SNOMEDID"] = "195967001";
                objData["SNOMED_DESCRIPTION"] = "Asthma";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "233604007" && objData["SNOMEDID"] != "233604007") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "233604007")
                }
                objData["SNOMEDID"] = "233604007";
                objData["SNOMED_DESCRIPTION"] = "Pneumonia";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "59621000" && objData["SNOMEDID"] != "59621000") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "59621000")
                }
                objData["SNOMEDID"] = "59621000";
                objData["SNOMED_DESCRIPTION"] = "Essential hypertension";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "95891005" && objData["SNOMEDID"] != "95891005") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "95891005")
                }
                objData["SNOMEDID"] = "95891005";
                objData["SNOMED_DESCRIPTION"] = "Flu-like symptoms";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "363746003" && objData["SNOMEDID"] != "363746003") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "363746003")
                }
                objData["SNOMEDID"] = "363746003";
                objData["SNOMED_DESCRIPTION"] = "Acute pharyngitis";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "53741008" && objData["SNOMEDID"] != "53741008") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "53741008")
                }
                objData["SNOMEDID"] = "53741008";
                objData["SNOMED_DESCRIPTION"] = "Coronary arteriosclerosis";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "266569009" && objData["SNOMEDID"] != "266569009") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "266569009")
                }
                objData["SNOMEDID"] = "266569009";
                objData["SNOMED_DESCRIPTION"] = "Benign prostatic hyperplasia";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "49436004" && objData["SNOMEDID"] != "49436004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "49436004")
                }
                objData["SNOMEDID"] = "49436004";
                objData["SNOMED_DESCRIPTION"] = "Atrial fibrillation";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "230572002" && objData["SNOMEDID"] != "230572002") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "230572002")
                }
                objData["SNOMEDID"] = "230572002";
                objData["SNOMED_DESCRIPTION"] = "Diabetic neuropathy";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "5281000124103" && objData["SNOMEDID"] != "5281000124103") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "5281000124103")
                }
                objData["SNOMEDID"] = "5281000124103";
                objData["SNOMED_DESCRIPTION"] = "Persistent asthma";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "981000124106" && objData["SNOMEDID"] != "981000124106") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "981000124106")
                }
                objData["SNOMEDID"] = "981000124106";
                objData["SNOMED_DESCRIPTION"] = "Moderate left ventricular systolic dysfunction";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "426749004" && objData["SNOMEDID"] != "426749004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "426749004")
                }
                objData["SNOMEDID"] = "426749004";
                objData["SNOMED_DESCRIPTION"] = "Chronic atrial fibrillation";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "991000124109" && objData["SNOMEDID"] != "991000124109") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "991000124109")
                }
                objData["SNOMEDID"] = "991000124109";
                objData["SNOMED_DESCRIPTION"] = "Severe left ventricular systolic dysfunction";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "64109004" && objData["SNOMEDID"] != "64109004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "64109004")
                }
                objData["SNOMEDID"] = "64109004";
                objData["SNOMED_DESCRIPTION"] = "Costal Chondritis";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "195967001" && objData["SNOMEDID"] != "195967001") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "195967001")
                }
                objData["SNOMEDID"] = "195967001";
                objData["SNOMED_DESCRIPTION"] = "Asthma";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "53741008" && objData["SNOMEDID"] != "53741008") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "53741008")
                }
                objData["SNOMEDID"] = "53741008";
                objData["SNOMED_DESCRIPTION"] = "Coronary arteriosclerosis";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "266569009" && objData["SNOMEDID"] != "266569009") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "266569009")
                }
                objData["SNOMEDID"] = "266569009";
                objData["SNOMED_DESCRIPTION"] = "Benign prostatic hyperplasia";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "49436004" && objData["SNOMEDID"] != "49436004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "49436004")
                }
                objData["SNOMEDID"] = "49436004";
                objData["SNOMED_DESCRIPTION"] = "Atrial fibrillation";
            } else if (CCM_Patient_Hub.LastSctBaseSearch == "230572002" && objData["SNOMEDID"] != "230572002") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "230572002")
                }
                objData["SNOMEDID"] = "230572002";
                objData["SNOMED_DESCRIPTION"] = "Diabetic neuropathy";
            }
        }
        //End for wrong snomed code
        objData["ICD9_Description"] = CCM_Patient_Hub.RemoveDashSignFromStr(objData["ICD9_Description"]);
        objData["ICD10_Description"] = CCM_Patient_Hub.RemoveDashSignFromStr(objData["ICD10_Description"]);
        objData["IMOProblem"] = CCM_Patient_Hub.RemoveDashSignFromStr(objData["IMOProblem"]);
        objData["ProblemName"] = CCM_Patient_Hub.RemoveDashSignFromStr(objData["ProblemName"]);
        //--------------------------
        objData["commandType"] = "SAVE_PROBLEMLIST";
        objData["IsChiefComplaint"] = 0;

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    RemoveDashSignFromStr: function (str) {
        if (str != null && str.indexOf('-') > -1) {
            var strArray = str.split('-');
            return strArray[strArray.length - 1].trim();
        } else {
            return str;
        }

    },

    NoKnownProblem: function () {
        var strMessage = "";
        var self = $("#" + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists");
        var myJSON = '{}';
        CCM_Patient_Hub.SaveProblemLists(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#pnlProblemLists_Result #btnNoKnownProblems").css("display", "none");
                utility.DisplayMessages(response.message, 1);
                CCM_Patient_Hub.ProblemListsSearch();
                $('#' + CCM_Patient_Hub.params.PanelID + ' #frmClinicalProblemLists').resetAllControls(null);
            } else {
                //utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    domReadyFunc: function () {
        $(document).ready(function () {
            $('#' + CCM_Patient_Hub.params.PanelID + ' .toggleHorSmallLeft section').unbind('click').bind("click", function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));
            });

            //for autocomplete zIndex fix
            $('#' + CCM_Patient_Hub.params.PanelID + " #txtProblems").on("autocompleteopen", function (event, ui) {
                if ($(this).closest(".modal-dialog").length == 0)
                    $(this).autocomplete('widget').zIndex("1018");
            });
        });
    },

    ResetDiagnosis: function () {
        //if ($("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val() == "") {
        $('#' + CCM_Patient_Hub.params.PanelID + ' #frmClinicalProblemLists').resetAllControls(null);
        $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate,#txtComments").prop("disabled", true);
        //}
    },

    BindICD9AutoComplete: function (element) {
        if ($(element).val().length > 3) {
            if ($(element).val().substring(0, 3).toLowerCase() == "sct") {
                CCM_Patient_Hub.LastSctBaseSearch = $(element).val().substring(3, $(element).val().length);
            } else {
                CCM_Patient_Hub.LastSctBaseSearch = "";
            }
        } else {
            CCM_Patient_Hub.LastSctBaseSearch = "";
        }
        $('#pnlCCM_Patient_Hub #txtProblems').attr("data-popupunload", "false");
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "CCM_Patient_Hub", null, false);
    },

    OpenSearchPopup: function () {
        var controlToLoad = "";
        controlToLoad = "Admin_IMOICD";
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "CCM_Patient_Hub";
        params["RefCtrl"] = "txtProblems";
        HiddenCtrl = 'hfICD1-1,hfICDDescription1-1,hfICD101-1,hfICD10Description1-1,hfSNOMED1-1,hfSNOMEDDescription1-1';
        params["RefHiddenCtrl"] = HiddenCtrl;
        LoadActionPan(controlToLoad, params);
    },

    ShowHistory: function (problemListId) {
        EMRUtility.showCurrentItemHistory(CCM_Patient_Hub.params.PanelID, null, problemListId, "ProblemList", CCM_Patient_Hub.params.PatientId, "CCM_Patient_Hub", null);
    },

    BindFavProblems: function () {
        var FavoriteListId = $('#' + CCM_Patient_Hub.params.PanelID + ' #ddlFavProblems').val();
        if (FavoriteListId != "") {
            Favorite_Problems.searchFavoriteList_ICD_DBCall(null, FavoriteListId, null, null).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.FavoriteListICDCount > 0) {
                        $('#' + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists #ulFavCompliantDisease li").remove();
                        var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
                        var li = "";
                        $.each(FavoriteListJSON, function (i, item) {
                            if (typeof item.ICD9Code == 'undefined' || item.ICD9Code == null) {
                                item.ICD9Code = '';
                            }
                            if (typeof item.ICD10Code == 'undefined' || item.ICD10Code == null) {
                                item.ICD10Code = '';
                            }
                            if (typeof item.SNOMEDID == 'undefined' || item.SNOMEDID == null) {
                                item.SNOMEDID = '';
                            }
                            if (typeof item.ICD10CodeDescription == 'undefined' || item.ICD10CodeDescription == null) {
                                item.ICD10CodeDescription = '';
                            }
                            var diagnosis = item.ICD10Code + " - " + item.ICD10CodeDescription;
                            var ICD9Code = "" + item.ICD9Code + "";
                            var ICD10Code = "" + item.ICD10Code + "";
                            var ICD9CodeDescription = "" + item.ICD9CodeDescription + "";
                            var ICD10CodeDescription = "" + item.ICD10CodeDescription + "";
                            var SNOMEDID = "" + item.SNOMEDID + "";
                            var SNOMEDDescription = "" + item.SNOMEDDescription + "";

                            li += "<li  id=" + item.FavoriteListICDId + " onclick='CCM_Patient_Hub.PopulateFields(this,\"" + diagnosis + "\",\"" + ICD9Code + "\",\"" + ICD10Code + "\",\"" + ICD9CodeDescription + "\",\"" + ICD10CodeDescription + "\",\"" + SNOMEDID + "\",\"" + SNOMEDDescription + "\");' ><a href='#' class='pr-sm'>" + ICD10Code + " - " + item.ICD10CodeDescription + "</a></li>";
                        });
                        $('#' + CCM_Patient_Hub.params.PanelID + ' #ulFavCompliantDisease').append(li);
                    }
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        //on change, set controls to default state.
        $("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val('');
        $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").val('');
        if ($("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val() == "") {
            $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate,#txtComments").prop("disabled", true);
        }
        if ($('#' + CCM_Patient_Hub.params.PanelID + ' #ddlFavProblems').val() == '' || $('#' + CCM_Patient_Hub.params.PanelID + ' #ddlFavProblems').val() == '- Select -') {
            $('#' + CCM_Patient_Hub.params.PanelID + ' #ulFavCompliantDisease li').remove();
        }
    },

    PopulateFields: function (cntrl, diagnosis, ICD9Code, ICD10Code, ICD9CodeDescription, ICD10CodeDescription, SNOMEDID, SNOMEDDescription) {
        $('#' + CCM_Patient_Hub.params.PanelID + ' #txtProblems').val($(cntrl).text());
        $('#' + CCM_Patient_Hub.params.PanelID + ' #txtDiagnosis').val(diagnosis);

        var lii = "<li icd9Code=\"" + ICD9Code + "\" icd9Desc=\"" + ICD9CodeDescription + "\" icd10Code=\"" + ICD10Code + "\" icd10Desc=\"" + ICD10CodeDescription + "\" snomedCode=\"" + SNOMEDID + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#' class='pr-sm'>" + ICD10CodeDescription + "</a></li>"
        $('#' + CCM_Patient_Hub.params.PanelID + ' #ulProblemDisease').html(lii);

        if (typeof $("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val() != 'undefined'
                   && $("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val() != null
                   && $("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val() != ""
                   && typeof $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").val() != 'undefined'
                             && $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").val() != null
                             && $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").val() != "") {
            $("#" + CCM_Patient_Hub.params.PanelID + " #ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate,#txtComments").prop("disabled", false);
            $('#hfIMOProblem').val($(cntrl).text());

            $("#" + CCM_Patient_Hub.params.PanelID + " #frmClinicalProblemLists").bootstrapValidator('revalidateField', 'ProblemName');
        }
    },

    //Row Functions
    UpdateProblemOnDrFirst: function (Command) {
        CCM_Patient_Hub.UpdateProblemOnDrFirst_DB_Call(Command).done(function (response1) {
            response1 = JSON.parse(response1);
            if (response1.status == false) {
                utility.DisplayMessages(response1.message, 3);
            }
        });
    },

    UpdateProblemOnDrFirst_DB_Call: function (Command) {
        var objData = {};
        objData["commandType"] = Command;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    rowSave: function ($row, obj) {
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
            } else if ($this.hasClass('actions')) {
                return _self.datatable.cell(this).data();
            } else if ($this.hasClass('ddl')) {
                return $.trim($this.find('select').val());
            } else {
                $obj_ = $this.find('input');

                if ($obj_.attr('type') == "checkbox") {
                    if ($obj_.prop('checked'))
                        return $.trim("True");
                    else
                        return $.trim("False");
                } else
                    return $.trim($obj_.val());
            }
        });

        var id = $row.attr("id");

        var myJSON = $row.getMyJSONByName();
        var NotesId = $row.attr("problemlistnotesid");
        var objData = JSON.parse(myJSON);
        $row.find("select[id*=ddlDiagnosis] option").each(function () {
            var opVal = $(this).val();
            var selVal = objData["Description"];
            if ($(this).val() == objData["Description"]) {
                objData["ICD9"] = $(this).attr("icd9code");
                objData["ICD10"] = $(this).attr("icd10code");
                objData["ICD9_Description"] = $(this).attr("icd9desc");
                objData["ICD10_Description"] = $(this).attr("icd10desc");
                objData["SNOMEDID"] = $(this).attr("snomedcode");
                objData["SNOMED_DESCRIPTION"] = $(this).attr("snomeddesc");
                return false;
            }
        });

        myJSON = JSON.stringify(objData);

        if (id && id > 0) {
            //Edit Record
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Medical_Problems List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    CCM_Patient_Hub.UpdateProblemListsRow(myJSON, id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            CCM_Patient_Hub.ProblemListsSearch();
                            CCM_Patient_Hub.UpdateProblemOnDrFirst("UpdateProblemInDrFirstForGrid");
                        } else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                } else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    rowDetail: function ($row, ClassName) {
        var currentProblemListId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProblemListId > 0) {
            CCM_Patient_Hub.ShowHistory(currentProblemListId);
        }
    },

    rowHistory: function ($row, ClassName) {
        var currentProblemListId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProblemListId > 0) {
            CCM_Patient_Hub.ShowHistory(currentProblemListId);
        }
    },

    rowAdd: function () {
        AppPrivileges.GetFormPrivileges("Medical_Problems List", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd();
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowRemove: function ($row, obj) {
        var strMessage = "";
        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("Medical_Problems List", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    } else {
                        var description;
                        if ($row.find("td:nth-child(4)").html() != "") {
                            description = $row.find("td:nth-child(4)").html();
                        } else {
                            description = $row.find("td:nth-child(3)").html();
                        }
                        CCM_Patient_Hub.DeleteProblemList(selectedValue, description, $row.find("td:nth-child(7)").html()).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if ($row.hasClass('adding')) {
                                }
                                var _self = obj;
                                _self.datatable.row($row.get(0)).remove().draw();

                                utility.DisplayMessages(response.Message, 1);
                                CCM_Patient_Hub.UpdateProblemOnDrFirst("DeleteProblemFromDrFirst");
                            } else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                            CCM_Patient_Hub.ProblemListsSearch();
                        });
                    }
                }, function () {
                },
                                  '1'
                    );
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowInactive: function ($row, obj) {
        var strMessage = "";
        var id = $row.attr("id");

        var IsActive = null;
        IsActive = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        AppPrivileges.GetFormPrivileges("Medical_Problems List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    } else {
                        var IsActiveRecord = null;
                        IsActiveRecord = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
                        if (IsActiveRecord == "1") {
                            var params = [];
                            var PanelID = "";
                            params["ParentCtrl"] = "CCM_Patient_Hub";
                            PanelID = CCM_Patient_Hub.params.PanelID;

                            params["ProblemListId"] = selectedValue;
                            params["FromAdmin"] = "0";
                            params["PatientId"] = CCM_Patient_Hub.params.PatientId;
                            LoadActionPan('Clinical_ProblemListInActive', params, PanelID);
                        } else {
                            IsActiveRecord = "0";
                            CCM_Patient_Hub.InActiveProblemList(selectedValue, null, null).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    CCM_Patient_Hub.ProblemListsSearch();
                                    CCM_Patient_Hub.UpdateProblemOnDrFirst("inactive_problemlistFromDrfirst");
                                } else {
                                    utility.DisplayMessages(response.message, 1);
                                }
                            });
                        }
                    }
                }, function () {
                },
                                   '3', null, null, null, IsActive
                    );
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    InActiveProblemList: function (ProblemListId, comments, endDate) {
        var IsCheckedIn = null;
        IsCheckedIn = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');

        var IsActiveRecord = null;
        IsActiveRecord = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if (IsActiveRecord == "1")
            IsActiveRecord = "0";
        else if (IsActiveRecord == "0")
            IsActiveRecord = "1";

        var patientId = CCM_Patient_Hub.params.PatientId;

        var objData = new Object();
        objData["ProblemListId"] = ProblemListId;
        objData["PatientId"] = patientId;
        objData["InActiveChkBoxValue"] = null;
        objData["InActiveReason"] = null;
        objData["EndDate"] = null;
        objData["IsActive"] = IsCheckedIn;
        objData["IsActiveRecord"] = IsActiveRecord;
        objData["commandType"] = "INACTIVE_PROBLEMLIST";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
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
        } else {
            $row.find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
            if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
            }
        }
    },

    ShowHideEditableGridRows: function (isShow) {
        var VitalsGridId = "#" + CCM_Patient_Hub.params.PanelID + " #dgvProblemLists";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {
            var row = CCM_Patient_Hub.EditableGrid.datatable.row(parentRow);

            if (isShow == true) {
                row.child.show();
                $(parentRow).find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
            } else {
                $(parentRow).find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();
            }
        });
    },

    // end editable grid functions
    UpdateProblemListsRow: function (ProblemListData, ProblemListId) {
        var isactive = null;
        isactive = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(ProblemListData);
        objData["ProblemListId"] = ProblemListId;
        objData["Comments"] = objData["hfComments"];
        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_PROBLEMLIST";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    buildHistoryRows: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems) {
        var row = CCM_Patient_Hub.EditableGrid.datatable.row(CurrentRow);
        if (arrChildItems != null && arrChildItems.length > 0) {
            var CurrentRowchilds = $();
            $.each(arrChildItems, function (i, item) {
                var currentChildRow = $("#" + CurrentRow.attr("id")).clone();
                currentChildRow.attr("id", "Child" + item.ProblemId);
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
        } else {
            $(CurrentRow).find("td:nth-child(1)").html("");
        }

        return row.child();
    },

    DeleteProblemList: function (ProblemListId, Description, StartDate) {
        var objData = new Object();
        objData["ProblemListId"] = ProblemListId;
        objData["commandType"] = "DELETE_PROBLEMLIST";
        objData["PatientId"] = CCM_Patient_Hub.params.PatientId;
        objData["Description"] = Description;
        objData["StartDate"] = StartDate;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    AddComments: function (ProblemListId) {
        var PanelID = "";
        params["ParentCtrl"] = "CCM_Patient_Hub";
        PanelID = CCM_Patient_Hub.params.PanelID;

        params["ProblemListId"] = ProblemListId;
        params["FromAdmin"] = "0";
        params["PatientId"] = CCM_Patient_Hub.params.PatientId;
        LoadActionPan('Clinical_ProblemListsComments', params, PanelID);
    },

    //----------------------------------------------------//
    //-------------Problems Functions END---------------//
    //----------------------------------------------------//

    UnLoad: function () {
        if (CCM_Patient_Hub.IshubChanged) {
            utility.myConfirm("2", function () {
                CCM_Patient_Hub.IsProblemFirstLoad = true;
                CCM_CarePlan.IsCarePlanFirstLoad = true;
                CCM_HRAssessment.IsHRAssessmentFirstLoad = true;
                if (CCM_Patient_Hub.params != null && CCM_Patient_Hub.params.ParentCtrl && CCM_Patient_Hub.params.ParentCtrlPanelID) {
                    UnloadActionPan(CCM_Patient_Hub.params.ParentCtrl, "CCM_Patient_Hub", null, CCM_Patient_Hub.params.ParentCtrlPanelID);
                } else if (CCM_Patient_Hub.params != null && CCM_Patient_Hub.params.ParentCtrl) {
                    UnloadActionPan(CCM_Patient_Hub.params.ParentCtrl, "CCM_Patient_Hub");
                } else {
                    UnloadActionPan(null, "CCM_Patient_Hub");
                }
            }, function () {
            });

        }
        else {
            CCM_Patient_Hub.IsProblemFirstLoad = true;
            CCM_CarePlan.IsCarePlanFirstLoad = true;
            CCM_HRAssessment.IsHRAssessmentFirstLoad = true;
            if (CCM_Patient_Hub.params != null && CCM_Patient_Hub.params.ParentCtrl && CCM_Patient_Hub.params.ParentCtrlPanelID) {
                UnloadActionPan(CCM_Patient_Hub.params.ParentCtrl, "CCM_Patient_Hub", null, CCM_Patient_Hub.params.ParentCtrlPanelID);
            } else if (CCM_Patient_Hub.params != null && CCM_Patient_Hub.params.ParentCtrl) {
                UnloadActionPan(CCM_Patient_Hub.params.ParentCtrl, "CCM_Patient_Hub");
            } else {
                UnloadActionPan(null, "CCM_Patient_Hub");
            }
        }


        //if (!$("#ccmTerminated").hasClass('hidden'))
        //    DashBoard.DashBoardCCMEnrollmentInfoSearch(null, null, null, null);
        //else
        CCMProgramUpdate.params.IsFromNote = false;
        DashBoard.DashBoardCCMEnrollmentInfoSearch(null, null, null, null);
    },

    // .............................................................. 'MK .......................................................\\

    AddGoals: function () {
        var params = [];
        params["ParentCtrl"] = 'CCM_Patient_Hub';
        params["ProblemListId"] = "";
        params["FromAdmin"] = "0";
        params["PatientId"] = CCM_Patient_Hub.params.PatientId;
        params["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
        params["mode"] = "Add";
        LoadActionPan('CCMEnrolledGoals', params);
    },

    LoadPatientHub: function () {
        var self = $('#pnlDashboard #pnlCCM_Patient_Hub #divPatientHub');
        var This = $('#pnlCCM_Patient_Hub');
        var data = "IsActive=&ID=" + CCM_Patient_Hub.params.EnrollmentInfoId;

        self.loadDropDownsWithTitle(true, data).done(function () {
            This.find('#ddlTemplatePatientHub').on('change', function () {
                var name = This.find('#ddlTemplatePatientHub').find(":selected").text();
                var Id = This.find('#ddlTemplatePatientHub').find(":selected").attr('value');
                var riskScore = This.find('#ddlTemplatePatientHub').find(":selected").attr('RefName');
                CCM_Patient_Hub.AddTemplateField(Id, name, riskScore);
            });
        });

        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });

        var objData = new Object();
        objData["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
        objData["PatientId"] = CCM_Patient_Hub.params.PatientId;
        objData["ProviderId"] = "0";
        objData["CareTeamId"] = "0";

        CCM_Patient_Hub.PatientHubStaticLoad(objData).done(function (response) {
            if (response.status != false) {
                $(document).ready(function () {
                    CCM_Patient_Hub.BindPatientHubStaticData(response, "PatientHub");
                });
            } else {
                utility.DisplayMessages("Issue encountered while opening CCM Hub.", 3);
                CCM_Patient_Hub.UnLoad();
            }
        });

        CCM_Patient_Hub.PatientHubProblemsLoad(objData).done(function (response) {
            if (response.status != false) {
                $(document).ready(function () {
                    CCM_Patient_Hub.BindPatientHubProblems(response);
                });
            } else {
                utility.DisplayMessages("Issue encountered while opening CCM Hub.", 3);
                CCM_Patient_Hub.UnLoad();
            }
        });

        //CCM_Patient_Hub.PatientHubRiskAssessmentScoreTemplateLoad(objData).done(function (response) {
        //    if (response.status != false) {
        //        CCM_Patient_Hub.BindPatientHubRiskAssessmentScoreTemplate(response);
        //    }
        //    else {
        //        utility.DisplayMessages("Issue encountered while loading Risk Assessment Score", 3);
        //        CCM_Patient_Hub.UnLoad();
        //    }
        //});

        CCMCareTeam.ProviderCareTeamLoad(objData).done(function (response) {
            if (response.status != false) {
                if (response.PHCount > 0) {
                    var response_ = JSON.parse(response.PHList_JSON);
                    CCM_Patient_Hub.params["ProviderName"] = response_[0].ProviderName;
                    $("#pnlCCM_Patient_Hub #tblPatientHubCareTeam tr").remove();
                    $("#pnlCCM_Patient_Hub #ddlProvider").val(response_[0].ProviderId).attr("selected", "selected");

                    tableHead = $('#pnlCCM_Patient_Hub #tblPatientHubCareTeam thead');
                    tableHead.append('<tr><th>Name</th><th>Role</th><th>Speciality</th><th>Contact Number</th></tr>');

                    table = $('#pnlCCM_Patient_Hub #tblPatientHubCareTeam tbody');
                    table.append('<tr id=' + response.CareTeamId + '><td>  ' + response_[0].CareManagerName + '</td><td>Care Manager</td><td></td><td>' + response_[0].CareManagerPhone + '</td></tr>');
                    table.append('<tr id=' + response.CareTeamId + '><td>  ' + response_[0].CareCoordinatorName + '</td><td>Care Coordinator</td><td></td><td>' + response_[0].CareCoordinatorPhone + '</td></tr>');
                    table.append('<tr id=' + response.CareTeamId + '><td>  ' + response_[0].CareGiverName + '</td><td>Care Giver</td><td></td><td>' + response_[0].CareGiverPhone + '</td></tr>');
                    table.append('<tr id=' + response.CareTeamId + '><td>  ' + response_[0].ProviderName + '</td><td>Provider</td><td>' + response_[0].Specialty + '</td><td>' + response_[0].ProviderPhone + '</td></tr>');
                }
            }
        });
        CCM_Patient_Hub.PatientHubGoals(objData);
    },

    PatientHubGoals: function (objData) {
        if (objData) {
            CCMEnrolledGoals.PatientHubGoalsLoad(objData).done(function (response) {
                if (response.status != false)
                    CCM_Patient_Hub.BindPatientHubGoals(response);
            });
        }
    },

    BindPatientHubStaticData: function (resp, tabCCM) {
        if (resp.PHCount > 0) {
            var response = JSON.parse(resp.PHList_JSON);

            if (tabCCM == "PatientHub") {
                if (response[0].PatientImage != "" && response[0].PatientImage != null)
                    $("#patImage").attr('src', response[0].PatientImage);

                $('#patientComments').text("Note: " + response[0].Comments)
                CCM_Patient_Hub.params["PatientName"] = response[0].PatientName;
                $("#lblPatientHUB_PatientName").text(response[0].PatientName);
                $("#lblPatientHUB_PatientAccountNumber").text("(" + response[0].AccountNumber + ")");
                $("#lblPatientHUB_PatientGender").text(response[0].Gender);
                $("#lblPatientHUB_PatientAge").text(response[0].Age + " Year(s)");
                $("#lblPatientHUB_PatientDOB").text(response[0].DateOfBirth != "" ? response[0].DateOfBirth.substring(0, response[0].DateOfBirth.indexOf(' ')) : "");
                $("#lblPatientHUB_CCMProgram").text(response[0].Program);
                CCM_Patient_Hub.params["Program"] = response[0].Program;

                $("#lblPatientHUB_PatientEC_Name").text(response[0].EmergencyContact);
                $("#lblPatientHUB_PatientEC_Phone").text(response[0].HomePhone);
                $("#lblPatientHUB_PatientEC_RelationShip").text(response[0].Relation);

                $("#lblPatientHUB_PatientAddress").text(response[0].Address1 + " " + response[0].City + " " + response[0].State + " " + response[0].ZIPCode);
                $("#lblPatientHUB_PatientContact").text(response[0].Patientphone);
                //!= "" ? response[0].NextAppointment.substring(0, response[0].NextAppointment.indexOf(' ')) : ""
                $("#lblPatientHUB_PatientNextAppointment").text(response[0].NextAppointment != null ? response[0].NextAppointment.substring(0, response[0].NextAppointment.indexOf(' ')) : "");
                $("#lblPatientHUB_PatientLastAppointment").text(response[0].LastAppointment != null ? response[0].LastAppointment.substring(0, response[0].LastAppointment.indexOf(' ')) : "");
                $("#lblPatientHUB_EnrollmentDate").text(utility.RemoveTimeFromDate(null, response[0].EnrollmentDate));

                if (response[0].CCMStatus == "Terminated") {
                    $("#lblCCMterminateProgram").text("Resume Program");
                    $("#ccmTerminated").attr("data-original-title", response[0].Reason);
                    $("#ccmTerminated").removeClass('hidden');
                } else {
                    $("#lblCCMterminateProgram").text("Terminate Program");
                    $("#ccmTerminated").attr("data-original-title", '');
                    $("#ccmTerminated").addClass('hidden');
                }
            } else if (tabCCM == "HRA") {
                $("#lblHRA_PatientName").text(response[0].PatientName);
                $("#lblPatientHUB_CCMProgram").text(response[0].Program);
                $("#lblHRA_PatientAccountNumber").text("(" + response[0].AccountNumber + ")");
                $("#lblHRA_PatientGender").text(response[0].Gender);
                $("#lblHRA_PatientAge").text(response[0].Age);
                $("#lblHRA_PatientDOB").text(response[0].DateOfBirth);

                $("#lblHRA_PatientEC_Name").text(response[0].EmergencyContact);
                $("#lblHRA_PatientEC_Phone").text(response[0].HomePhone);
                $("#lblHRA_PatientEC_RelationShip").text(response[0].Relation);

                $("#lblHRA_PatientAddress").text(response[0].Address1 + " " + response[0].City + " " + response[0].State + " " + response[0].ZIPCode);
                $("#lblHRA_PatientContact").text(response[0].Patientphone);

                $("#lblHRA_PatientLastAppointment").text(response[0].LastAppointment);
                $("#lblHRA_PatientNextAppointment").text(response[0].NextAppointment);
            }
        }
    },

    BindPatientHubProblems: function (resp) {
        if (resp.PHCount > 0) {
            var response = JSON.parse(resp.PHList_JSON);
            $('#divPatientHub #ulPatientHubProblemsList').empty();
            for (var i = 0; i < response.length; i++) {
                var liChilddiv = '<li id=' + response[i].Id + '><div class="ellipses size-max80per pull-left">' + response[i].ICD10_Description + '</div> <div class="pull-right size-max20per"><strong>' + response[i].ICD10 + '</strong></div><a href="#" onclick=CCM_Patient_Hub.DeletePatientHubProblems(' + response[i].Id + ') class="removeIconListHover"><i class="fa fa-times"></i></a><div class="clearfix"></div></li>';
                //var liChilddiv = '<li id=' + response[i].Id + '><div class="ellipses size-max80per pull-left">' + response[i].ICD10_Description + '</div> <div class="pull-right size-max20per"><strong>' + response[i].ICD10 + '</strong></div></li>';
                $('#divPatientHub #ulPatientHubProblemsList').append(liChilddiv);
            }
        }
    },

    BindPatientHubGoals: function (resp) {
        if (resp.PHCount > 0) {
            var response = JSON.parse(resp.PHList_JSON);
            $('#divPatientHub #ulPatientHubGoals').empty();
            $('#divPatientHub #ulPatientHubGoals').append('<li><a href="#" onclick="CCM_Patient_Hub.AddGoals();" class="col-xs-12 blue p-none"><span class="pull-left">Goals</span> <i class="fa fa-edit pull-right mt-xs"></i></a><div class="clearfix"></div></li>');

            for (var i = 0; i < response.length; i++) {
                var liChilddiv = '<li id=' + response[i].EnrolledGoalsICDId + '><div class="pull-left">' + response[i].CPTDescription + ' : ' + response[i].Instruction + '</div> <div class="pull-right size-max20per"></div><div class="clearfix"></div></li><li></li>';
                //var liChilddiv = '<li id=' + response[i].Id + '><div class="ellipses size-max80per pull-left">' + response[i].ICD10_Description + '</div> <div class="pull-right size-max20per"><strong>' + response[i].ICD10 + '</strong></div></li>';
                $('#divPatientHub #ulPatientHubGoals').append(liChilddiv);
                $("#p_PatientHubGoals").text("Added by " + response[i].ModifiedByName + " on " + response[i].ModifiedOn);
            }
        } else {
            $('#divPatientHub #ulPatientHubGoals').empty();
            $('#divPatientHub #ulPatientHubGoals').append('<li><a href="#" onclick="CCM_Patient_Hub.AddGoals();" class="col-xs-12 blue p-none"><span class="pull-left">Goals</span> <i class="fa fa-edit pull-right mt-xs"></i></a><div class="clearfix"></div></li>');
        }
    },

    BindPatientHubRiskAssessmentScoreTemplate: function (resp) {
        if (resp.PHCount > 0) {
            if (resp.PHList_JSON) {
                var response = JSON.parse(resp.PHList_JSON);

                var finalScore = parseFloat(0.0);
                $("#divstoAppend div").remove();
                for (var i = 0; i < resp.PHCount; i++) {
                    //if ($("#divstoAppend").find("#div_" + response[i].RiskAssessmentId).length <= 0) {
                    $("#divstoAppend").append('<div class="col-sm-12" id=div_' + response[i].RiskAssessmentId + ' onmouseover="CCM_Patient_Hub.showIcon(this, ' + response[i].RiskAssessmentId + ');" onmouseout="CCM_Patient_Hub.hideIcon(this, ' + response[i].RiskAssessmentId + ');"><label for="" class="control-label" id=lbl' + response[i].RiskAssessmentId + '>' +
                                              response[i].TemplateDescription + '</label><a href="#" id=anchor' + response[i].RiskAssessmentId + ' onclick="CCM_Patient_Hub.DeletePatientHubRiskAssessmentScoreTemplate(' + response[i].RiskAssessmentId + ')" class="removeIconListHover"><i class="fa fa-times red"></i></a><input id=txt' +
                                              response[i].RiskAssessmentId + ' type="text" oninput="CCM_Patient_Hub.SetControlValue(this, ' + response[i].TemplateId + ',' + response[i].RiskAssessmentId + ');" class="form-control" value="' + response[i].RiskScore + '" /></div>');
                    $("#anchor" + response[i].RiskAssessmentId).hide();
                    //$("#txt" + response[i].TemplateId).val(response[i].RiskScore);
                    var ss = response[i].RiskScore == "" ? "0" : response[i].RiskScore;
                    finalScore = (parseFloat(finalScore) + parseFloat(ss));
                    //}
                    //else {
                    //    utility.DisplayMessages("Duplicate Templates Not Allowed", 2);
                    //}
                }
                var htmlFinalScore = '<div class="col-sm-12" id="divfinalScores"><label for="" class="control-label">Final Score</label><input type="text" id="txtfinalScore" name="finalScore" class="form-control" value=""></div>';
                $("#divstoAppend").append(htmlFinalScore);
                $("#txtfinalScore").val(finalScore.toFixed(2));
                $("#txtfinalScore").attr("disabled", "disabled");
            }
        } else {
            $("#divstoAppend div").remove();
        }
    },

    SaveHUB: function () {
        var objs = [];
        for (var i = 0; i < $("#divstoAppend div").length - 1; i++) {
            var objData = new Object();
            var TemplateId = $($("#divstoAppend div[id^='div_'] input")[i]).attr('id').substring(3);
            var Score = $($("#divstoAppend div[id^='div_'] input")[i]).attr('value');
            var RiskAssessmentId = $($("#divstoAppend div[id^='div_'] a")[i]).attr('id').substring(6);

            objData["RiskAssessmentId"] = RiskAssessmentId;
            objData["RiskAssessTemptId"] = TemplateId;
            objData["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
            objData["AssessHTML"] = "";//$("#divstoAppend")[0].innerHTML;
            objData["RiskScore"] = Score != "" ? parseFloat(Score) : parseFloat(0);
            objs.push(objData);
        }

        CCM_Patient_Hub.PatientHubRiskAssessmentScoreTemplateInsertUpadte(objs).done(function (response) {
            utility.DisplayMessages("Successfully Updated", 1);
            CCM_HRAssessment.searchHRAssessment();
            CCM_Patient_Hub.IshubChanged = false;
            CCM_Patient_Hub.LoadPatientHub();
        });
    },

    PatientHubRiskAssessmentScoreTemplateInsertUpadte: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "InsertUpdateRiskAssessmentScoreTemplate");
    },

    PatientHubRiskAssessmentTemplateInsert: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "InsertCCMPatientHUBRiskAssessmentTemplate");
    },

    DeletePatientHubProblems: function (Id) {
        var objData = new Object();
        objData["ChronicProblemId"] = Id;
        objData["PatientId"] = CCM_Patient_Hub.params.PatientId;

        CCM_Patient_Hub.PatientHubProblemDelete(objData).done(function (response) {
            if (response.status != false) {
                $("#" + Id).remove();
                utility.DisplayMessages(response.Message, 3);
                CCM_Patient_Hub.ProblemListsSearch();
                CCM_Patient_Hub.IshubChanged = true;
            }
        });
    },

    PatientHubProblemDelete: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "DeleteChronicProblems");
    },

    PatientHubStaticLoad: function (objData) {
        $("#divProgramUpdates").hide();
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "loadPatientHubStatic");
    },

    PatientHubProblemsLoad: function (objData) {
        $("#divProgramUpdates").hide();
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "loadPatientHubProblems");
    },

    PatientHubRiskAssessmentScoreTemplateLoad: function (objData) {
        $("#divProgramUpdates").hide();
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "LoadRiskAssessmentScoreTemplate");
    },

    IsTerminateCCM: function (obj) {
        var params = [];
        var IsTerminate = "";
        if ($(obj).is(":checked")) {
            IsTerminate = true;
            params["Terminated"] = "Terminated";
        } else {
            IsTerminate = false;
            params["Terminated"] = "Accepted";
        }

        if ($("#lblCCMterminateProgram").text() == "Resume Program") {
            var Status = "Accepted";
            CCMTermination.SaveTerminationReason("Resume");
        } else {
            params["ParentCtrl"] = 'CCM_Patient_Hub';
            params["FromAdmin"] = "0";
            params["PatientId"] = CCM_Patient_Hub.params.PatientId;
            params["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
            params["IsTerminate"] = IsTerminate;
            LoadActionPan('CCMTermination', params);

            CCM_Patient_Hub.IshubChanged = false;
        }
    },

    CareTeamSelect: function (providerId) {
        var params = [];
        params["ParentCtrl"] = 'CCM_Patient_Hub';
        params["FromAdmin"] = "0";
        params["PatientId"] = CCM_Patient_Hub.params.PatientId;
        params["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
        params["ProviderId"] = providerId;
        LoadActionPan('CCMCareTeam', params);

        CCM_Patient_Hub.IshubChanged = true;
    },

    LoadHRA: function () {
        var objData = new Object();
        objData["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
        objData["PatientId"] = CCM_Patient_Hub.params.PatientId;

        CCM_Patient_Hub.PatientHubStaticLoad(objData).done(function (response) {
            if (response.status != false)
                CCM_Patient_Hub.BindPatientHubStaticData(response, "HRA");
            else {
                utility.DisplayMessages("Issue encountered while opening CCM Hub.", 3);
                CCM_Patient_Hub.UnLoad();
            }
        });
    },

    Export: function (isPrint) {
        var dom = "";
        if (isPrint)
            dom = "PatientHubPrint";
        else
            dom = "PatientHubExport";

        kendo.drawing.drawDOM("#" + CCMProgramUpdate.params["PanelID"] + " #" + dom, {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "5mm",
                top: "5mm",
                right: "5mm",
                bottom: "20mm"
            },
        }).then(function (group) {
            if (isPrint) {
                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                    var params = [];
                    params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                    params["PreviewPdf"] = true;
                    utility.PDFViewer(params["PrintPDFDataURL"], true, null, false, true);
                    //controls to hide
                });
            } else {
                kendo.drawing.pdf.saveAs(group, "CCMPatientHub.pdf");
            }
        });
    },

}