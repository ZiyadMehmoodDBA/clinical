//// ┌────────────────────────────────────────────────────────────┐ \\
//// │                                                            │ \\
//// ├────────────────────────────────────────────────────────────┤ \\
//// │ Author Dmitry Baranovskiy (http://dmitry.baranovskiy.com/) │ \\
//// └────────────────────────────────────────────────────────────┘ \\
Clinical_Immunization = {
    bIsFirstLoad: true,
    params: [],
    HXTabId: "tabHx",
    Birth_2_yearsTabId: "tab_Birth_2_years",
    GridResultId: "",
    GriddgvId: "",
    VaccineHxIdsForPrintOrGenerateHL7: "",
    myGrid: null,
    myGridForTherapeuticInjection: null,
    VaccineHxCount: 0,
    Load: function (params) {
        Clinical_Immunization.params = params;
        Clinical_Immunization.VaccineHxCount = 0;
        if (Clinical_Immunization.params["ParentCtrl"] == 'clinicalTabFaceSheet') {
            Clinical_Immunization.params.patientID = Clinical_Immunization.params.PatientId;
        } 
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#pnlClinicalImmunization #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            Clinical_Immunization.params.patientID = $('#PatientProfile #hfPatientId').val();
        }
        if (Clinical_Immunization.params.ParentCtrl == "Clinical_FaceSheet") {
            Clinical_Immunization.params.patientID = Clinical_FaceSheet.params.patientID;
        }
        if (Clinical_Immunization.params.PanelID != 'pnlClinicalImmunization') {
            Clinical_Immunization.params.PanelID = Clinical_Immunization.params.PanelID + ' #pnlClinicalImmunization';
        } else {
            Clinical_Immunization.params.PanelID = 'pnlClinicalImmunization';
        }

        $('#' + Clinical_Immunization.params.PanelID + ' #tpHistoryForecastTime').timepicker('setTime', new Date().toLocaleTimeString());
        Clinical_Immunization.domReadyFunc();
        $.when(Result = Clinical_Immunization.ActiveTab()).then(function () {
            if (typeof Result.response != typeof undefined) {
                Clinical_Immunization.LoadSchedulerChat(Result.response);
            }
        });
        Clinical_Immunization.ImmunizationQuerySearch('', 1, 15);
        Clinical_Immunization.ImmunizationQueryResponseSearch('', 1, 15);
        if (globalAppdata["isTransmittoImmunizationRegistries"] && globalAppdata["isTransmittoImmunizationRegistries"].toLowerCase() == "false") {
            $('#' + Clinical_Immunization.params.PanelID + " #ulSocialHxTabsItems #listHistoryForecast").addClass("hidden");
            $("#" + Clinical_Immunization.params.PanelID + " #pnlHxHistoryForecast_Result").addClass("hidden");
            $("#" + Clinical_Immunization.params.PanelID + " #HistoryForecastHxHeading").addClass("hidden");
        }
        else {
             $('#' +Clinical_Immunization.params.PanelID + " #ulSocialHxTabsItems #listHistoryForecast").removeClass("hidden");
             $("#" + Clinical_Immunization.params.PanelID + " #pnlHxHistoryForecast_Result").removeClass("hidden");
             $("#" + Clinical_Immunization.params.PanelID + " #HistoryForecastHxHeading").removeClass("hidden");
        }
       
    },
    LoadSchedulerChat: function (TabId) {
        Clinical_Immunization.LoadSchedlerData(TabId);
    },
    ActiveTab: function () {
        var dfd = $.Deferred();
        var TabId = "";
        Clinical_Immunization.FillDemographic(Clinical_Immunization.params.patientID).done(function (response) {

            if (response.status != false) {
                var PatientInfoJSON = JSON.parse(response.DemographicFill_JSON);

                if (PatientInfoJSON.MRN && PatientInfoJSON.MRN != "") {
                    if ($('#' + Clinical_Immunization.params.PanelID + " #MrnStateIDRegisteryDataDiv").hasClass("hidden")) {
                        $('#' + Clinical_Immunization.params.PanelID + " #MrnStateIDRegisteryDataDiv").removeClass("hidden");
                    }
                    $('#' + Clinical_Immunization.params.PanelID + " #ddlMrnStateIDRegisteryId").val(1);
                    $('#' + Clinical_Immunization.params.PanelID + " #txtMrnStateIDRegisteryData").val(PatientInfoJSON.MRN);
                    Clinical_Immunization.params.MRN = PatientInfoJSON.MRN;
                }
                else {
                    Clinical_Immunization.params.MRN = "";
                }
                var ActualAge = PatientInfoJSON.Age.split(",");
                var Days = 0;
                //var Year = ActualAge[0].replace("Year(s)", "").trim();
                //var Month = ActualAge[1].replace("Mounth(s)", "").trim();
                //var Days = ActualAge[2].replace("Day(s)", "").trim();
                var Year = parseInt(ActualAge[0]);
                var Month = parseInt(ActualAge[1]);
                var Days = parseInt(ActualAge[2]);

                Clinical_Immunization.params.Years = Year;
                Clinical_Immunization.params.AccountNo = PatientInfoJSON.AccountNo;
                Clinical_Immunization.params.DOB = PatientInfoJSON.DOB;
                Clinical_Immunization.params.Gender = PatientInfoJSON.Sex;
                Clinical_Immunization.params.Name = PatientInfoJSON.LastName + ', ' + PatientInfoJSON.FirstName;

                if (Year < 2) {
                    TabId = "Birth - 2 Years";
                }
                else if (Year == 2) {
                    if (Month == 0 && Days == 0) {
                        TabId = "Birth - 2 Years";
                    }
                    else if (Month > 0 || Days > 0) {
                        TabId = "2 - 18 Years";
                    }
                }
                else if (Year < 18 && Year > 2) {
                    TabId = "2 - 18 Years";
                }
                else if (Year == 18) {
                    if (Month == 0 && Days == 0) {
                        TabId = "2 - 18 Years";
                    }
                    else if (Month > 0 || Days > 0) {
                        TabId = "Adult";
                    }
                }
                else if (Year > 18) {
                    TabId = "Adult";
                }
                dfd.response = TabId;
                Clinical_Immunization.LoadSchedlerAndActiveTabData(TabId, false);
                dfd.resolve();

            } else {
                dfd.resolve();
            }



        });
        return dfd;
    },
    FillDemographic: function (PatientID) {

        var objData = new Object();
        objData["PatientID"] = PatientID;
        objData["CommandType"] = "fill_patient_demographic";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },
    LoadSchedlerAndActiveTabData: function (TabId, ComeFromHtml) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var TabId1 = TabId;
                if (TabId == "Birth - 2 Years") {
                    TabId1 = "Birth_2_years";
                }
                else if (TabId == "2 - 18 Years") {
                    TabId1 = "2_18_years";
                }
                if (TabId != "") {
                    $('#' + Clinical_Immunization.params.PanelID).find('[id*="list"]').removeClass('active');
                    $('#' + Clinical_Immunization.params.PanelID).find('[id*="tab_"]').removeClass('active');
                    $('#' + Clinical_Immunization.params.PanelID + ' #list' + TabId1).addClass('active');
                    $('#' + Clinical_Immunization.params.PanelID + ' #tab_' + TabId1).addClass('active');
                }
                if (ComeFromHtml && TabId !== "Hx" && TabId != "HistoryForecast" && TabId != "TherapeuticInjection") {
                    Clinical_Immunization.LoadSchedlerData(TabId);
                }
                
                else if (ComeFromHtml && TabId == "Hx") {
                    Clinical_Immunization.ImmunizationQuerySearch('', 1, 15);
                    $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                        
                    });

                }
                if (TabId == "HistoryForecast") {
                    Clinical_Immunization.ImmunizationQueryResponseSearch('', 1, 15);
                    $('#pnlClinicalImmunization #tab_HistoryForecast #HistoryForecastTime').timepicker({
                        setTime: new Date(),
                        minuteStep: 1
                    });
                }
                if (TabId == "TherapeuticInjection") {
                    Clinical_Immunization.ImmunizationTherapeuticInjectionSearch('', 1, 15);
                }
                if (TabId !== "Hx") {
                    $('#' + Clinical_Immunization.params.PanelID + " #AddButton").addClass("disableAll");
                }
                else {
                    $('#' + Clinical_Immunization.params.PanelID + " #AddButton").removeClass("disableAll");
                }
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });

    },
    LoadSchedlerData: function (TabId) {
        var dfd = $.Deferred();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_Immunization.SearchSchedlerData_DBCall(TabId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $.when(Clinical_Immunization.SchedulerGridLoad(response, TabId)).then(function () {
                            dfd.resolve();
                        });
                    }
                    else {

                        utility.DisplayMessages(response.Message, 3);
                        dfd.resolve();
                    }
                });
            } else {
                dfd.resolve();
                utility.DisplayMessages(strMessage, 2);
            }
        });
        return dfd;
    },
    SchedulerGridLoad: function (response, TabId) {
        var dfd = $.Deferred();
        $.when(Clinical_Immunization.setGridIds(TabId)).then(function () {
            $("#" + Clinical_Immunization.params.PanelID + ' #' + Clinical_Immunization.GriddgvId + ' tbody').empty();
            if (response.SchedulerCount > 0) {
                var SchedulerLoad_JSON = JSON.parse(response.SchedulerLoad_JSON);
                $.each(SchedulerLoad_JSON, function (i, item) {
                    var $row;
                    if (item.VaccineHxId != null && item.VaccineHxId != "") {
                        var AddParameters = "'Edit','" + item.Category + "'," + item.VaccineScheduleId + ",'" + TabId + "'," + item.VaccineHxId + ",'" + item.Type + "',null,'" + item.VaccineGroupID + "'";
                        $row = $('<tr onclick="Clinical_Immunization.ClickOnAddButtonFromSchedulerGrid(' + AddParameters + ')"/>');
                    }
                    else {
                        $row = $('<tr/>');
                    }
                    var AlertData = "<td></td>";
                    if (item.AlertDescription != null || item.AlertDescription != '') {
                        if (item.AlertDescription == "Normal") {
                            AlertData = '<td class="text-success"><span style="font-weight:bold">' + item.AlertDescription + '</span></td>';
                        }
                        else if (item.AlertDescription == "Overdue") {
                            AlertData = '<td class="text-danger"><span style="font-weight:bold">' + item.AlertDescription + '</span></td>';
                        }
                        else if (item.AlertDescription == "Due") {
                            AlertData = '<td class="text-warning"><span style="font-weight:bold">' + item.AlertDescription + '</span></td>';
                        }
                    }
                    $row.attr("id", item.VaccineScheduleId);
                    $row.append('<td style="text-align:center" ><strong>' + item.Schedule + '</strong></td>');
                    $row.append('<td style="text-align:center" ><strong>' + item.Category + '</strong></td>');
                    var AddParameters = "'Add','" + item.Category + "'," + item.VaccineScheduleId + ",'" + TabId + "',null,null,null,'" + item.VaccineGroupID + "'";
                    var AddButton = '<button type="button" class="btn btn-success btn-sm" onclick="Clinical_Immunization.ClickOnAddButtonFromSchedulerGrid(' + AddParameters + ')"><i class="fa fa-plus-circle"></i></button>';
                    var Type = "";
                    var color = "";

                    if (item.VaccineHxId != null && item.VaccineHxId != "") {
                        if (item.Type != null && item.Type.indexOf("ADMINISTER") >= 0) {
                            Type = "Administered";
                            color = "67f74d";
                        }
                        else if (item.Type == "DOCUMENTHX") {
                            Type = "Document Hx";
                            color = "ffff7c";
                        }
                        else if (item.Type == "REFUSAL") {
                            Type = "Refused";
                            color = "e2dbdb";
                        }
                        if (item.Type != null && item.Type.indexOf("ADMINISTER") >= 0) {
                            $row.append('<td style="background-color:#' + color + ';text-align:center">' + item.VaccineName + "<br>" + (item.Dose != "" ? (item.Dose + " " + item.Amount) : "") + '</td>');
                        }
                        else {
                            $row.append('<td style="background-color:#' + color + ';text-align:center">' + item.VaccineName + "<br>" + (item.Dose != "" ? (item.Dose + " " + item.Amount) : "") + '</td>');
                        }
                        $row.append('<td style="text-align:center">' + item.AdministrationDate + '</td>');
                        $row.append('<td style="text-align:center">' + item.GivenByName + '</td>');


                        $row.append('<td style="text-align:center">' + Type + '</td>');
                        if (TabId != "Recurring") {
                            $row.append(AlertData);
                        }

                    }
                    else {
                        $row.append('<td style="text-align:center">' + AddButton + '</td>');
                        $row.append('<td>' + '</td>');
                        $row.append('<td>' + '</td>');
                        $row.append('<td>' + '</td>');
                        if (TabId != "Recurring") {
                            $row.append(AlertData);
                        }
                    }


                    $("#" + Clinical_Immunization.params.PanelID + ' #' + Clinical_Immunization.GriddgvId + ' tbody').last().append($row);
                });
                dfd.resolve();
            }

        });

        return dfd;
    },
    ClickOnAddButtonFromSchedulerGrid: function (Mode, VaccineCategoryId, VaccineScheduleId, TabId, VaccineHxId, Type, event, VaccineCategory, OrderSetId) {
        if (event != null) {
            if ($(event.target).is('i[class*="fa-plus-square"]') || $(event.target).is('input[type=checkbox]') || $(event.target).is('i[class*="fa-minus-square"]') || $(event.target).is('input[type=file]') || $(event.target).is('a[class*="AcknowToolTip"]')) {
                return;
            }
        }

        var MODE = "ADD";
        if (Mode == "Edit") {
            MODE = "EDIT";
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", MODE, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];

                var PanelID = "";

                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                    params["from"] = 'clinicalTabProgressNote';
                    params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                    params["ParentPanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                }
                else if (Clinical_Immunization.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Immunization.params.ParentCtrl == "Clinical_FaceSheet") {
                    params["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                    params["from"] = 'clinicalTabFaceSheet';
                    params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                    params["ParentPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                }
                else {
                    params["ParentCtrl"] = 'clinicalTabImmunization';
                    PanelID = 'pnlClinicalImmunization';
                    params["from"] = 'clinicalTabImmunization';
                    params["PanelID"] = 'pnlClinicalImmunization';
                    params["ParentPanelID"] = 'pnlClinicalImmunization';
                }

                // params["VaccineHxId"] = vaccineHxId;
                if (typeof OrderSetId != typeof undefined && OrderSetId != null) {
                    params["OrderSetId"] = OrderSetId;
                }
                else {
                    params["OrderSetId"] = "";
                }
                params["FromAdmin"] = 0;
                params["VaccineScheduleId"] = VaccineScheduleId;
                params["CategoryId"] = VaccineCategoryId;//category Name
                params["Category"] = VaccineCategory;
                if (Mode == "Edit") {
                    params["VaccineHxId"] = VaccineHxId;
                    params["Type"] = Type;
                }
                params["TabId"] = TabId;
                params["mode"] = Mode;
                params["patientID"] = Clinical_Immunization.params["patientID"];
                LoadActionPan('Clinical_ImmunizationDetail', params, PanelID);
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    setGridIds: function (TabId) {
        if (TabId == "Birth - 2 Years") {
            Clinical_Immunization.GridResultId = "pnlVaccineSchedule_Birth_2_years_Result";
            Clinical_Immunization.GriddgvId = "dgvVaccineSchedule_Birth_2_years";
        }
        else if (TabId == "2 - 18 Years") {
            Clinical_Immunization.GridResultId = "pnlVaccineSchedule_2_18_years_Result";
            Clinical_Immunization.GriddgvId = "dgvVaccineSchedule_2_18_years";
        }
        else if (TabId == "Adult") {
            Clinical_Immunization.GridResultId = "pnlVaccineSchedule_Adult_Result";
            Clinical_Immunization.GriddgvId = "dgvVaccineSchedule_Adult";
        }
        else if (TabId == "Recurring") {
            Clinical_Immunization.GridResultId = "pnlVaccineSchedule_Recurrinng_Result";
            Clinical_Immunization.GriddgvId = "dgvVaccineSchedule_Recurring";
        }

    },
    SearchSchedlerData_DBCall: function (TabId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["TabId"] = TabId;
        objData["PatientId"] = Clinical_Immunization.params.patientID;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "load_SearchSchedlerData";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Immunization");
    },
    OpenForPrint: function (AgeLimit) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                var PanelID = "";
                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                    //params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                }
                else if (Clinical_Immunization.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Immunization.params.ParentCtrl == "Clinical_FaceSheet") {
                    params["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                    // params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                }
                else {
                    params["ParentCtrl"] = 'clinicalTabImmunization';
                    PanelID = 'pnlClinicalImmunization';
                    //params["PanelID"] = 'pnlClinicalImmunization';
                }

                params["UserId"] = globalAppdata['AppUserId'];
                params["PatientId"] = Clinical_Immunization.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();

                params["AgeLimit"] = AgeLimit;
                LoadActionPan('Clinical_SchedulerView', params, PanelID);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    ImmunizationSearch: function (PrimaryID, PageNo, rpp) {
        var dfd = $.Deferred();

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_Immunization.VaccineHxCount = 0;
                Clinical_Immunization.searchImmunization(PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
                            if ($("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx thead tr #SelectRecord").length == 0) {
                                $("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx thead tr")
                                    .prepend(' <th id="SelectRecord" class="size10 center" coltype="checkbox"> <input type="checkbox" id="chkHeaderImmunization" onchange="Clinical_Immunization.SelectUnselectAllRecords(this,1);"   class="input-block" coltype="checkbox"/> </th>');
                            }
                        } else {
                            //if ($("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx thead tr #SelectRecord").length == 0) {
                            //    $("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx thead tr")
                            //        .prepend(' <th id="SelectRecord" class="size10 center" coltype="checkbox"> <input type="checkbox" onchange="Clinical_Immunization.SelectUnselectAllRecords(this);"  class="input-block" coltype="checkbox"/> </th>');
                            //}
                        }

                        if ($("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx thead th #ColumnForPrintOrGenerate #CheckBoxForPrintOrGenerate").length == 0) {
                            $("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx th #ColumnForPrintOrGenerate")
                                .append('<input id="CheckBoxForPrintOrGenerate" type="checkbox" onchange="Clinical_Immunization.SelectUnselectAllRecords(this,1);"  class="input-block" coltype="checkbox"/>');
                        }
                        Clinical_Immunization.VaccineHxCount = response.iTotalDisplayRecords;
                        Clinical_Immunization.VaccineHxGridLoad(response);
                        var TableControl = Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result #dgvImmunizationVaccineHx";
                        var PagingPanelControlID = Clinical_Immunization.params.PanelID + " #divImmunizationVaccineHx_Paging";
                        var ClassControlName = "Clinical_Immunization";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;

                        setTimeout(CreatePagination(response.ParentImmunizationCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_Immunization.ImmunizationSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                        setTimeout(function () {
                            // Added by Zia Mehmood

                            if ($("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx tbody tr").length == 1) {
                                if ($($("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx tbody tr")[0]).find("td").length == 1) {
                                    if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNote").length == 0) {
                                        $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Clinical_Immunization.addImmunizationToNotes();" disabled id="btnAddImmunizationToNote">Add on Note</button>').insertAfter("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging .pagination")
                                    }
                                }
                                else {
                                    if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNote").length == 1) {
                                        $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNote").remove();
                                    }
                                    if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNote").length == 0) {
                                        $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Clinical_Immunization.addImmunizationToNotes();" disabled id="btnAddImmunizationToNote">Add on Note</button>').insertAfter("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging .pagination")
                                    }
                                }
                            }
                            else {
                                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNotes").length == 1) {
                                    $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNotes").remove();
                                }
                                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNote").length == 0) {
                                    $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Clinical_Immunization.addImmunizationToNotes();" disabled id="btnAddImmunizationToNote">Add on Note</button>').insertAfter("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging .pagination")
                                }
                            }


                            dfd.resolve();
                        }, 11);
                        //setTimeout(function () {
                        //    $('#' + Clinical_Immunization.params.PanelID + " #VaccineBtn").click();

                        //}, 13);
                    } else {
                        Clinical_Immunization.VaccineHxCount = 0;
                        $('#' + Clinical_Immunization.params.PanelID + ' #pnlVaccineHx_Result #dgvImmunizationVaccineHx').DataTable({
                            "language": {
                                "emptyTable": "No Vaccine found."
                            },
                            "autoWidth": false,
                            "bLengthChange": false,
                            "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }],
                            "bDestroy": true
                        });
                        setTimeout(function () {
                            //$('#' + Clinical_Immunization.params.PanelID + " #VaccineBtn").click();
                            dfd.resolve();
                        }, 5);
                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
                dfd.resolve();
            }
        });
        //==============================});
    },
    searchImmunization: function (pageNumber, rowsPerPage) {

        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }

        var IsActive = null;
        IsActive = $("#" + Clinical_Immunization.params.PanelID + ' #pnlVaccineHx_Result #divSwitch #switchActive').attr('IsActive');

        if (IsActive == null) {
            IsActive = "1";
        }

        var objData = new Object();
        objData["PatientId"] = Clinical_Immunization.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = IsActive;
        objData["NotesId"] = Clinical_Immunization.params.NotesId == null ? 0 : Clinical_Immunization.params.NotesId;
        objData["pageNumber"] = pageNumber;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_Immunization";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },
    VaccineHxGridLoad: function (response) {
        Clinical_Immunization.params.VaccineHxGridArray = [];
        var isactive = $('#' + Clinical_Immunization.params.PanelID + ' #pnlVaccineHx_Result #divSwitch #switchActive').attr('isactive');

        // get Actions
        var actions = "";

        if ($.fn.dataTable.isDataTable("#" + Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result #dgvImmunizationVaccineHx")) {
            $("#" + Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result #dgvImmunizationVaccineHx").dataTable().fnClearTable();
            $("#" + Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result #dgvImmunizationVaccineHx").dataTable().fnDestroy();
            $("#" + Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result #dgvImmunizationVaccineHx tbody").find("tr").remove();
        } else {
            //for stop make dublicate Datatables
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == 'dgvImmunizationVaccineHx') {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result #dgvImmunizationVaccineHx tbody").find("tr").remove();
                    $("#" + Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result #dgvImmunizationVaccineHx").parent().parent().find('div.row').remove();
                }
            })
        }




        //tem array to hold rows and childs
        var arraTemp = [];
        var immunizationParentChildLoadJSONData = JSON.parse(response.ImmunizationParentChildLoad_JSON);
        if (immunizationParentChildLoadJSONData.length > 0) {

            if ($.fn.dataTable.isDataTable("#" + Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result #dgvImmunizationVaccineHx")) {
                $("#" + Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result #dgvImmunizationVaccineHx").dataTable().fnDestroy();
            }

            $.each(immunizationParentChildLoadJSONData, function (i, item) {

                var parentRow = item.ParentImmunization;
                var childRows = item.ChildImmunizationList;

                var vaccineId = parentRow.VaccineId;
                var vaccineHxId = parentRow.VaccineHxId;
                var $row = $('<tr/>');

                $row.attr("id", vaccineId);
                $row.attr("vaccineHxId", vaccineHxId);
                var AddParameters = "'Edit','" + parentRow.Category + "','" + parentRow.VaccineScheduleId + "'," + (parentRow.IsHistoryDose ? "'HistoryDose'" : "'Hx'") + ",'" + parentRow.VaccineHxId + "','" + parentRow.Type + "',event,'','" + parentRow.OrderSetId + "'";

                //  $row.click(function () {
                var MethodMode = 'Clinical_Immunization.ClickOnAddButtonFromSchedulerGrid(' + AddParameters + ');';
                //});
                $row.attr("onclick", MethodMode);
                //  var link = $('<a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>');
                //link.attr("onclick", MethodMode);
                // selectAction = link[0].outerHTML + '&nbsp;';
                //$row.prop("onclick", "Clinical_Immunization.loadImmunizationDetails($(this),event)");


                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (parentRow.IsNoteLinked == "True") {
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(parentRow.VaccineHxId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(parentRow.VaccineHxId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }
                var UploadIcon = '<span class="btn-xs ml-xxs btn-file inline-block" style="cursor: pointer;"><i class="fa fa-upload"></i><input type="file" class="btn btn-file" name="Import_Acknowledgements_file" accept="application/txt" id="Import_Acknowledgements_file" allowfiles="txt" onchange="Clinical_Immunization.UploadAcknowledgements( ' + parentRow.VaccineHxId + ',this);"></span>';
                //'<a class="btn  btn-xs" href="#" onclick="Clinical_Immunization.UploadAcknowledgements(' + parentRow.VaccineHxId + ',1);" title="Upload Acknowledgements"><i class="fa fa-upload"></i></a>';


                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
                    // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_Immunization.enableAddImmunization(this);" id="' + parentRow.VaccineHxId + '" name="SelectCheckBoxImmunization" ' + Checked + ' class="input-block text-center"/></td>';
                    // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                } else {
                    SelectionCheckBoxColumn = '';
                }
                var SelectionCheckBoxColumnForPrintOrGenerate = '<td class="sorting_1 center">' + UploadIcon + '<div class="inline-bloack pull-right"><input type="checkbox" class="text-center" id="ForPrintOrGenerate_' + parentRow.VaccineHxId + '"  class="input-block text-center" onclick="Clinical_Immunization.CheckOrUncheckImmunizationForPrintOrGenerateHL7(this,' + parentRow.VaccineHxId + ')"/></div></td>';

                var ParentType = "";
                if (parentRow.Type != null && parentRow.Type.indexOf("ADMINISTER") >= 0) {
                    ParentType = "Administered";
                }
                else if (parentRow.Type == "DOCUMENTHX") {
                    ParentType = "DocumentHx";
                }
                else if (parentRow.Type == "REFUSAL") {
                    ParentType = "Refusal";
                }
                var Acknowledgement = "";
                var AcknowledgmentMessage1 = "";
                var AcknowledgmentMessage2 = "";
                var Class="";
                if (parentRow.AcknowledgementCode.toLowerCase() == "aa") {
                    Acknowledgement = "Successful";
                    AcknowledgmentMessage1 = "Application Accept";
                    AcknowledgmentMessage2 = "Accept";
                    Class = "fa fa-bell pull-right";
                }
                else if (parentRow.AcknowledgementCode.toLowerCase() == "ae") {
                    Acknowledgement = "Error";
                    AcknowledgmentMessage1 = "Application Error";
                    AcknowledgmentMessage2 = "Error";
                    Class = "fa fa-exclamation-triangle pull-right";
                }
                else if (parentRow.AcknowledgementCode.toLowerCase() == "ar") {
                    Acknowledgement = "Error";
                    AcknowledgmentMessage1 = "Application Rejected";
                    AcknowledgmentMessage2 = "Reject";
                    Class = "fa fa-exclamation-triangle pull-right";
                }
                else {
                    AcknowledgmentMessage1 = "";
                    AcknowledgmentMessage2 = "";
                    
                }
                var obj = {};
                obj.VaccineHxId = parentRow.VaccineHxId;
                obj.Message1 = AcknowledgmentMessage1;
                obj.Message2 = AcknowledgmentMessage2;
                obj.AcknowledgementCode = parentRow.AcknowledgementCode;
                Clinical_Immunization.params.VaccineHxGridArray.push(obj);

                var acknowledgementDiv = '<div style="padding:2px;">  <a href="#" class="AcknowToolTip"    id=' + parentRow.VaccineHxId + '_ToolTipId onclick="Clinical_Immunization.DownloadImmAcknowledgement('+parentRow.VaccineHxId+ ')">' +Acknowledgement + '</a><i class="'+Class+'" ></i></div>';
                //Start || 22 April, 2016 || ZeeshanAK || Changes for audit button
                if (childRows.length > 0) {
                    $row.append(SelectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td>' + SelectionCheckBoxColumnForPrintOrGenerate + '<td>' + parentRow.CategoryName + '</td><td>' + parentRow.VaccineName + '</td><td>' + parentRow.Dose + '</td><td>' + parentRow.AdministrationDate + '</td><td>' + parentRow.ProviderName + '</td><td>' + parentRow.GivenByName + '</td><td>' + parentRow.Location + '</td><td>' + parentRow.LotNumber + '</td><td> ' + ParentType + ' </td><td>' + acknowledgementDiv + '</td>');
                } else {
                    $row.append(SelectionCheckBoxColumn + '<td></td>' + SelectionCheckBoxColumnForPrintOrGenerate + '<td>' + parentRow.CategoryName + '</td><td>' + parentRow.VaccineName + '</td><td>' + parentRow.Dose + '</td><td>' + parentRow.AdministrationDate + '</td><td>' + parentRow.ProviderName + '</td><td >' + parentRow.GivenByName + '</td><td>' + parentRow.Location + '</td><td>' + parentRow.LotNumber + '</td><td> ' + ParentType + ' </td><td>' + acknowledgementDiv + '</td>');
                }
                //End   || 22 April, 2016 || ZeeshanAK || Changes for audit button

                // Append parent row to the table body
                $("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx tbody").last().append($row);

                //Child Records
                var CurrentRowchilds = $();

                if (childRows.length > 0) {


                    $.each(childRows, function (i, item) {


                        var SelectionCheckBoxColumn = "";
                        var Checked = "";
                        if (item.IsNoteLinked == "True") {
                            if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.VaccineHxId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                                Checked = " ";
                            } else {
                                Checked = " checked";
                            }
                        } else {
                            if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.VaccineHxId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                                Checked = " checked";
                            } else {
                                Checked = "";
                            }
                        }

                        if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
                            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                            SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_Immunization.enableAddImmunization(this);" id="' + item.VaccineHxId + '" name="SelectCheckBoxImmunization" ' + Checked + ' class="input-block text-center"/></td>';
                            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                        } else {
                            SelectionCheckBoxColumn = '';
                        }
                        var UploadIcon = '<span class="btn-xs ml-xxs btn-file inline-block" style="cursor: pointer;"><i class="fa fa-upload"></i><input type="file" class="btn btn-file" name="Import_Acknowledgements_file" accept="application/txt" id="Import_Acknowledgements_file" allowfiles="txt" onchange="Clinical_Immunization.UploadAcknowledgements( ' + item.VaccineHxId + ',this);"></span>';
                        var SelectionCheckBoxColumnForPrintOrGenerate = '<td class="sorting_1 center">' + UploadIcon + '<div class="inline-bloack pull-right"><input type="checkbox" class="text-center" id="ForPrintOrGenerate_' + item.VaccineHxId + '"  class="input-block text-center" onclick="Clinical_Immunization.CheckOrUncheckImmunizationForPrintOrGenerateHL7(this,' + item.VaccineHxId + ')"/></div></td>';

                        

                        var AddParameters = "'Edit','" + item.Category + "'," + item.VaccineScheduleId + "," + (item.IsHistoryDose ? "'HistoryDose'" : "'Hx'") + "," + item.VaccineHxId + ",'" + item.Type + "'" + ",event(),'','" + item.OrderSetId + "'";

                        var ChildType = "";
                        if (item.Type != null && item.Type.indexOf("ADMINISTER") >= 0) {
                            ChildType = "Administered";
                        }
                        else if (item.Type == "DOCUMENTHX") {
                            ChildType = "DocumentHx";
                        }
                        else if (item.Type == "REFUSAL") {
                            ChildType = "Refusal";
                        }
                        var ChildClass = "";
                        var Acknowledgement1 = "";
                        if (item.AcknowledgementCode.toLowerCase() == "aa") {
                            Acknowledgement1 = "Successful";
                            AcknowledgmentMessage1 = "Application Accept";
                            AcknowledgmentMessage2 = "Accept";
                            ChildClass = "fa fa-bell pull-right";
                        }
                        else if (item.AcknowledgementCode.toLowerCase() == "ae") {
                            Acknowledgement1 = "Error";
                            AcknowledgmentMessage1 = "Application Error";
                            AcknowledgmentMessage2 = "Error";
                            ChildClass = "fa fa-exclamation-triangle pull-right";
                        }
                        else if (item.AcknowledgementCode.toLowerCase() == "ar") {
                            Acknowledgement1 = "Error";
                            AcknowledgmentMessage1 = "Application Rejected";
                            AcknowledgmentMessage2 = "Reject";
                            ChildClass = "fa fa-exclamation-triangle pull-right";
                        }
                        else {
                            AcknowledgmentMessage1 = "";
                            AcknowledgmentMessage2 = "";
                        }
                        obj = {};
                        obj.VaccineHxId = item.VaccineHxId;
                        obj.Message1 = AcknowledgmentMessage1;
                        obj.Message2 = AcknowledgmentMessage2;
                        obj.AcknowledgementCode = item.AcknowledgementCode;
                        Clinical_Immunization.params.VaccineHxGridArray.push(obj);

                        acknowledgementDiv = '<div style="padding:2px;"> <a href="#" class="AcknowToolTip" id=' + item.VaccineHxId + '_ToolTipId onclick="Clinical_Immunization.DownloadImmAcknowledgement(' + item.VaccineHxId + ')">' + Acknowledgement1 + '</a><i class="' + ChildClass + '" ></i></div>';

                        //Start || 22 April, 2016 || ZeeshanAK || Changes for audit button
                        var childRow = '<tr class="childRow-bg" onclick="Clinical_Immunization.ClickOnAddButtonFromSchedulerGrid(' + AddParameters + ')" >' + SelectionCheckBoxColumn + '<td></td>' + SelectionCheckBoxColumnForPrintOrGenerate +'<td>' + parentRow.CategoryName + '</td><td>' + item.VaccineName + '</td><td>' + item.Dose + '</td><td>' + item.AdministrationDate + '</td><td>' + item.ProviderName + '</td><td>' + item.GivenByName + '</td><td>' + item.Location + '</td><td>' + item.LotNumber + '</td><td>' + ChildType + '</td><td>' + acknowledgementDiv + '</td></tr>';
                        //End   || 22 April, 2016 || ZeeshanAK || Changes for audit button

                        CurrentRowchilds = CurrentRowchilds.add(childRow);
                    });
                }
                arraTemp.push({ row: $row, childs: CurrentRowchilds });
            });

            //Clear Data Table and draw

            //Inalize grid


            var PanelGrid = "#" + Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result";
            var GridId = "#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx";

            //Start By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search
            if (Clinical_Immunization.myGrid != null) {
                //  Clinical_ProblemLists.myGrid.$table.find("tbody tr").remove();
                // Clinical_ProblemLists.myGrid.$table.dataTable().fnClearTable()
                if ($.fn.dataTable.isDataTable(Clinical_Immunization.myGrid)) {
                    Clinical_Immunization.myGrid.$table.dataTable().fnDestroy();
                } else {
                    Clinical_Immunization.myGrid = null;
                }
                //  Clinical_ProblemLists.myGrid.datatable.clear().draw();
                if ($.fn.dataTable.isDataTable("#" + Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result #dgvImmunizationVaccineHx")) {
                    $("#" + Clinical_Immunization.params.PanelID + " #pnlVaccineHx_Result #dgvImmunizationVaccineHx").dataTable().fnDestroy();
                }
            }

            Clinical_Immunization.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Clinical_Immunization, 0, false, true, false, true, false, null);


            //Clinical_Immunization.EditableGrid.datatable.clear().draw();


            //push parent/childs rows to the datatable
            $.each(arraTemp, function (i, item) {

                if (Clinical_Immunization.myGrid != null) {

                    var row = Clinical_Immunization.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {

                        row.child(item.childs);
                    } else {
                    }
                }

            });

            if ($('#' + Clinical_Immunization.params.PanelID + ' #pnlVaccineHx_Result').length > 0) {
                $('#' + Clinical_Immunization.params.PanelID + ' #dgvImmunizationVaccineHx').dataTable().fnSettings().aoColumns[0].bSortable = false;
            }

            var checked = '';
            if (isactive == "0") {
            } else if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "1";
                checked = 'checked="checked"';
            }

            /*var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                         '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_Immunization.ActiveImmunizationSearch(this);">' +
                          '</div><span class="pl-xs">Active</span>';*/
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_Immunization.ActiveImmunizationSearch(this);">' +
                '</div><span class="pl-xs">Active</span>';

            $("#" + Clinical_Immunization.params.PanelID + ' #tab_Hx .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');

        } else {

            var Message = "No Active Immunization";
            if (isactive == "0") {
                Message = "No Inactive Immunization";
            }

            $('#' + Clinical_Immunization.params.PanelID + ' #pnlVaccineHx_Result #dgvImmunizationVaccineHx').DataTable({
                "language": {
                    "emptyTable": Message
                },
                "autoWidth": false,
                "bLengthChange": false,
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }],
                "bDestroy": true
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
                '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_Immunization.ActiveImmunizationSearch(this);">' +
                '</div><span class="pl-xs">Active</span>';

            $("#" + Clinical_Immunization.params.PanelID + ' #tab_Hx .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
            //if (response.ProblemListTotalCount == 0) {
            //    $("#pnlProblemLists_Result #btnNoKnownProblems").css("display", "");
            //} else {
            //    $("#pnlProblemLists_Result #btnNoKnownProblems").hide();
            //}
        }

        EMRUtility.SwicthWidgetInializatoin();

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        //Start//22//02//2016//Muhammad Ahmad Imran//Sorting removed from first column of grid
        $('#' + Clinical_Immunization.params.PanelID + ' #dgvImmunizationVaccineHx thead tr th:first-child').removeClass('sorting_asc');
        if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_Immunization.params.PanelID + ' #dgvImmunizationVaccineHx thead tr th:nth-child(2)').removeClass('sorting');
        }
        Clinical_Immunization.MakeToolTip();
        //End//22//02//2016//Muhammad Ahmad Imran//Sorting removed from first column of grid
    },
    MakeToolTip: function () {
        $.each(Clinical_Immunization.params.VaccineHxGridArray, function (i, item) {
            var popover = '<h5 class="pull-left m-none pr-xxs">' + Clinical_Immunization.params.Name + '</h5> <span>' + Clinical_Immunization.params.Years + ' Y,' + Clinical_Immunization.params.Gender + '; '
                                   + '<b>Account:</b> ' + Clinical_Immunization.params.AccountNo + '; '
                                   + '<b>DOB: </b> ' + Clinical_Immunization.params.DOB + '</span>'
                                   + '<hr class="stooltip " >'

                                   + '<ul class="list-unstyled pl-none">'

                                   + '<li style="width:100%;margin-bottom:8px;"><strong>' + (item.AcknowledgementCode != "" ? item.AcknowledgementCode.toUpperCase() : "") + ':</strong> ' + item.Message1 + '</li>'
                                   + '<li style="width:100%"><strong class="noWordBreak">Application Acknowledgement:</strong>' + item.Message2 + '</li>'
            + '</ul>'
            $('#' + Clinical_Immunization.params.PanelID + ' #' + item.VaccineHxId + "_ToolTipId").tooltipster({
                theme: 'tooltipster-shadow',
                content: $(popover),
                functionReady: function (instance, helper) {
                    var posTop = $(helper)[0].getBoundingClientRect().top;
                    var anchorBottom = $(this)[0].getBoundingClientRect().bottom;
                    if (posTop < 0) {
                        $('.tooltipster-base').css("top", (anchorBottom + 13) + "px");
                        $('.tooltipster-arrow').removeClass("tooltipster-arrow-top").addClass("tooltipster-arrow-bottom");
                    }
                }

            });
        });
    },
    CheckOrUncheckImmunizationForPrintOrGenerateHL7: function (obj, VaccineHxId) {
        //if ($(obj).prop("checked")) {
        //    if (Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7 != "") {
        //        var SplitedVaccineHxIdsForPrintOrGenerateHL7 = Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7.split(',');
        //        var alreadyExists = false;
        //        $.each(SplitedVaccineHxIdsForPrintOrGenerateHL7, function (i, item) {
        //            if (item == VaccineHxId) {
        //                alreadyExists = true;
        //            }
        //        });
        //        if (alreadyExists == false) {
        //            Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7 = Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7 + "," + VaccineHxId;
        //        }
        //    }
        //    else {
        //        Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7 = VaccineHxId;
        //    }
        //}
        //else {
        //    if (Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7 != "") {
        //        var SplitedVaccineHxIdsForPrintOrGenerateHL7 = Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7.split(',');
        //        $.each(SplitedVaccineHxIdsForPrintOrGenerateHL7, function (i, item) {
        //            if (item != VaccineHxId) {
        //                if (i == 0) {
        //                    Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7 = item;
        //                } else {
        //                    Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7 = Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7 + "," + VaccineHxId;
        //                }
        //            }
        //        });

        //    }
        //}
    },
    ActiveImmunizationSearch: function (objThis) {

        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        } else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        Clinical_Immunization.ImmunizationSearch();
    },
    domReadyFunc: function () {

        $(document).ready(function () {
            // $("#pnlClinicalImmunization #dgvImmunizationVaccineHx thead tr[role='row'] th#SelectRecord input[type='checkbox']").on('change', function () {
            //    Clinical_Immunization.SelectUnselectAllRecords();
            //  });
            $('.toggleHorSmallLeft section').click(function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));
            });
        });

    },
    rowExpand: function ($row, obj) {
        var _self = obj,
            $actions,
            values = [];
        var row = _self.datatable.row($row);

        if (Clinical_Immunization.params.ActionPanContainer == "actionPanClinicalProgressNote") {
            if (row.child.isShown()) {
                // This row is already open - close it
                $row.find("td:nth-child(2) .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();
            } else {
                $row.find("td:nth-child(2) .fa-plus-square").attr("class", "fa fa-minus-square");
                // Open this row
                row.child.show();
                if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                    row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
                }
            }
        } else {
            if (row.child.isShown()) {
                // This row is already open - close it
                $row.find("td:nth-child(1) .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();
                //tr.removeClass('shown');
            } else {
                $row.find("td:nth-child(1) .fa-plus-square").attr("class", "fa fa-minus-square");
                // Open this row
                row.child.show();
                if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                    row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
                }
            }
        }
    },
    PreviewImmunization: function () {
        //if (Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7 != "") {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                var PanelID = "";
                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                    // params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                }
                else if (Clinical_Immunization.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Immunization.params.ParentCtrl == "Clinical_FaceSheet") {
                    params["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                    //params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                }
                else {
                    params["ParentCtrl"] = 'clinicalTabImmunization';
                    PanelID = 'pnlClinicalImmunization';
                    //params["PanelID"] = 'pnlClinicalImmunization';
                }
                params["UserId"] = globalAppdata['AppUserId'];
                params["PatientId"] = Clinical_Immunization.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                params["ImmunizationIds"] = Clinical_Immunization.VaccineHxIdsForPrintOrGenerateHL7;
                LoadActionPan('Clinical_Immunization_Preview', params, PanelID);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        //}
    },

    SelectUnselectAllRecords: function (chkBox, From) {
        //If from 1 then VaccineHx,Else Therapeutic Injection
        if (From == 0) {
            $("#" + Clinical_Immunization.params.PanelID + " #dgvTherapeutic [name='SelectCheckBoxImmunization']").prop("checked", $(chkBox).is(':checked'));
            $("#" + Clinical_Immunization.params.PanelID + " #dgvTherapeutic tbody").find('input[name="SelectCheckBoxImmunization"]').each(function () {
                Clinical_Immunization.enableAddImmunization(this);
            });
        }
        else {
            $("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx [name='SelectCheckBoxImmunization']").prop("checked", $(chkBox).is(':checked'));
            $("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx tbody").find('input[name="SelectCheckBoxImmunization"]').each(function () {
                Clinical_Immunization.enableAddImmunization(this);
            });
        }
    },


    //    //This Function enable/disable add to note button
    enableAddImmunization: function (obj) {
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id);
            }
            if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id);
                if (index > -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {
            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-283
            $("#" + Clinical_ProgressNote.params.PanelID + ' #pnlVaccineHx_Result #chkHeaderImmunization').prop('checked', false);
            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-283
            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id);
            }
        }

        if (Clinical_ProgressNote.AttachedNoteComponentIds.length > 0 || Clinical_ProgressNote.DetachedNoteComponentIds.length > 0) {
            $("#" + Clinical_Immunization.params.PanelID + " #btnAddImmunizationToNotes").prop('disabled', false);
            $("#" + Clinical_Immunization.params.PanelID + " #btnAddImmunizationToNote").prop('disabled', false);
        } else {
            $("#" + Clinical_Immunization.params.PanelID + " #btnAddImmunizationToNotes").prop('disabled', true);
            $("#" + Clinical_Immunization.params.PanelID + " #btnAddImmunizationToNote").prop('disabled', true);
        }

    },

    // added on April 13,2016 by Muhammad Ahmad Imran
    //    //Call Back function to add component to Progress Note
    addImmunizationToNotes: function () {
        //var SelectedImmunization = $("#" + Clinical_Immunization.params.PanelID + ' [name=SelectCheckBoxProbList]:checked:not(:disabled)').map(function () { return this.id; }).get().join();
        //if (SelectedImmunization != null && SelectedImmunization != '') {
        //    Clinical_Immunization.getProblemListsInfo(SelectedImmunization);
        //}
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        var strMessage = "";
        //Start//22/12/2015//Ahmad Raza//Logic implemented for Privileges
        // AppPrivileges.GetFormPrivileges("Medical_Problems List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //if (strMessage == "") {


        var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
        if (detachedvalues.join() != '') {

            var splitAdministerVaccineId = detachedvalues;
            var AdministerVaccineIdForImmunization = [];
            var AdministerVaccineIdForTherapeuticInjection = [];
            $.each(detachedvalues, function (i, item) {
                if (item.indexOf("thera") > -1) {
                    AdministerVaccineIdForTherapeuticInjection.push(item.replace("thera", ""));
                }
                else {
                    AdministerVaccineIdForImmunization.push(item);
                }
            });
            if (AdministerVaccineIdForImmunization != "") {
                Clinical_ImmunizationDetail.detachImmunizationFromNotes_DBCall(AdministerVaccineIdForImmunization.join()).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (AdministerVaccineIdForTherapeuticInjection != "") {
                            Clinical_ImmunizationDetail.detachTherapeuticInjectionFromNotes_DBCall(AdministerVaccineIdForTherapeuticInjection.join()).done(function (response1) {
                                response1 = JSON.parse(response1);
                                if (response1.status != false) {
                                    for (var i = 0; i < detachedvalues.length; i++) {
                                        var IMMid = detachedvalues[i];
                                        var HeadingNotRemoved = true;
                                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllAdministerVaccine") {
                                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllAdministerVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineAdminister').remove();
                                                HeadingNotRemoved = false;
                                            }
                                        }
                                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllTherapeuticInjection") {
                                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllTherapeuticInjection').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_TherapeuticInjection').remove();
                                                HeadingNotRemoved = false;
                                            }
                                        }
                                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllTherapeuticInjectionHistory") {
                                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllTherapeuticInjectionHistory').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_TherapeuticInjectionHistory').remove();
                                                HeadingNotRemoved = false;
                                            }
                                        }
                                        if (HeadingNotRemoved) {
                                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllDocumentHxVaccine") {
                                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllDocumentHxVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineDocumentHx').remove();
                                                    HeadingNotRemoved = false;
                                                }
                                            }
                                        }
                                        if (HeadingNotRemoved) {
                                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllRefusalVaccine") {
                                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllRefusalVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineRefusal').remove();
                                                    HeadingNotRemoved = false;
                                                }
                                            }
                                        }
                                        if (HeadingNotRemoved) {
                                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllVoidDoseVaccine") {
                                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllVoidDoseVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineVoidDose').remove();
                                                    HeadingNotRemoved = false;
                                                }
                                            }
                                        }
                                        if (HeadingNotRemoved) {
                                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).remove();
                                        }
                                    }

                                    $.when(Clinical_ProgressNote.saveComponentSOAPText("Immunization", true)).then(function () {




                                        var SelectedImmunization = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
                                        var def = [];
                                        if (SelectedImmunization != null && SelectedImmunization != '') {
                                            for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                                                var IMMid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).length != 0) {
                                                    var index = SelectedImmunization.indexOf(IMMid);
                                                    if (index > -1) {
                                                        SelectedImmunization.splice(index, 1);
                                                    }
                                                }
                                            }
                                            if (SelectedImmunization.join() != null && SelectedImmunization.join() != '') {
                                                var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
                                                var isGridUpdate = false;
                                                if (detachedvalues.join() == '') {
                                                    isGridUpdate = true;
                                                }
                                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(SelectedImmunization.join(), "", (Clinical_Immunization.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val()), true, false)).then(function () {
                                                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                                });
                                            }
                                        }
                                        else {
                                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                            if (Clinical_Immunization.params != null && Clinical_Immunization.params.PanelID.indexOf('pnlClinicalImmunization') != -1) {
                                                Clinical_Immunization.ImmunizationSearch();
                                                Clinical_Immunization.ImmunizationTherapeuticInjectionSearch();
                                            }
                                        }
                                    });

                                }
                            });
                        }
                        else {
                            for (var i = 0; i < detachedvalues.length; i++) {
                                var IMMid = detachedvalues[i];
                                var HeadingNotRemoved = true;
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllAdministerVaccine") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllAdministerVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineAdminister').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllTherapeuticInjection") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllTherapeuticInjection').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_TherapeuticInjection').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllTherapeuticInjectionHistory") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllTherapeuticInjectionHistory').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_TherapeuticInjectionHistory').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                                if (HeadingNotRemoved) {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllDocumentHxVaccine") {
                                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllDocumentHxVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineDocumentHx').remove();
                                            HeadingNotRemoved = false;
                                        }
                                    }
                                }
                                if (HeadingNotRemoved) {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllRefusalVaccine") {
                                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllRefusalVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineRefusal').remove();
                                            HeadingNotRemoved = false;
                                        }
                                    }
                                }
                                if (HeadingNotRemoved) {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllVoidDoseVaccine") {
                                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllVoidDoseVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineVoidDose').remove();
                                            HeadingNotRemoved = false;
                                        }
                                    }
                                }
                                if (HeadingNotRemoved) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).remove();
                                }
                            }

                            $.when(Clinical_ProgressNote.saveComponentSOAPText("Immunization", true)).then(function () {
                                var SelectedImmunization = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
                                var def = [];
                                if (SelectedImmunization != null && SelectedImmunization != '') {
                                    for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                                        var IMMid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).length != 0) {
                                            var index = SelectedImmunization.indexOf(IMMid);
                                            if (index > -1) {
                                                SelectedImmunization.splice(index, 1);
                                            }
                                        }
                                    }
                                    if (SelectedImmunization.join() != null && SelectedImmunization.join() != '') {
                                        var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
                                        var isGridUpdate = false;
                                        if (detachedvalues.join() == '') {
                                            isGridUpdate = true;
                                        }
                                        $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(SelectedImmunization.join(), "", (Clinical_Immunization.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val()), true, false)).then(function () {
                                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                        });

                                    }
                                }
                                else {
                                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                    if (Clinical_Immunization.params != null && Clinical_Immunization.params.PanelID.indexOf('pnlClinicalImmunization') != -1) {
                                        Clinical_Immunization.ImmunizationSearch();
                                        Clinical_Immunization.ImmunizationTherapeuticInjectionSearch();
                                    }
                                }

                            });

                        }

                        //   utility.DisplayMessages(response.Message, 1);
                    } else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else if (AdministerVaccineIdForTherapeuticInjection != "") {
                Clinical_ImmunizationDetail.detachTherapeuticInjectionFromNotes_DBCall(AdministerVaccineIdForTherapeuticInjection.join()).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        for (var i = 0; i < detachedvalues.length; i++) {
                            var IMMid = detachedvalues[i];
                            var HeadingNotRemoved = true;
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllAdministerVaccine") {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllAdministerVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineAdminister').remove();
                                    HeadingNotRemoved = false;
                                }
                            }
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllTherapeuticInjection") {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllTherapeuticInjection').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_TherapeuticInjection').remove();
                                    HeadingNotRemoved = false;
                                }
                            }
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllTherapeuticInjectionHistory") {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllTherapeuticInjectionHistory').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_TherapeuticInjectionHistory').remove();
                                    HeadingNotRemoved = false;
                                }
                            }

                            if (HeadingNotRemoved) {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllDocumentHxVaccine") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllDocumentHxVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineDocumentHx').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                            }
                            if (HeadingNotRemoved) {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllRefusalVaccine") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllRefusalVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineRefusal').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                            }
                            if (HeadingNotRemoved) {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).parent().parent().attr("id") == "AllVoidDoseVaccine") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllVoidDoseVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineVoidDose').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                            }
                            if (HeadingNotRemoved) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).remove();
                            }
                        }

                        $.when(Clinical_ProgressNote.saveComponentSOAPText("Immunization", true)).then(function () {
                            var SelectedImmunization = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
                            var def = [];
                            if (SelectedImmunization != null && SelectedImmunization != '') {
                                for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                                    var IMMid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).length != 0) {
                                        var index = SelectedImmunization.indexOf(IMMid);
                                        if (index > -1) {
                                            SelectedImmunization.splice(index, 1);
                                        }
                                    }
                                }
                                if (SelectedImmunization.join() != null && SelectedImmunization.join() != '') {
                                    var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
                                    var isGridUpdate = false;
                                    if (detachedvalues.join() == '') {
                                        isGridUpdate = true;
                                    }
                                    $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(SelectedImmunization.join(), "", (Clinical_Immunization.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val()), true, false)).then(function () {
                                        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                    });
                                }
                            }
                            else {
                                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                if (Clinical_Immunization.params != null && Clinical_Immunization.params.PanelID.indexOf('pnlClinicalImmunization') != -1) {
                                    Clinical_Immunization.ImmunizationSearch();
                                    Clinical_Immunization.ImmunizationTherapeuticInjectionSearch();
                                }
                            }

                        });

                    }
                });
            }

        }
        else {
            var SelectedImmunization = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
            var def = [];
            if (SelectedImmunization != null && SelectedImmunization != '') {
                for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                    var IMMid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + IMMid).length != 0) {
                        var index = SelectedImmunization.indexOf(IMMid);
                        if (index > -1) {
                            SelectedImmunization.splice(index, 1);
                        }
                    }
                }
                if (SelectedImmunization.join() != null && SelectedImmunization.join() != '') {
                    var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
                    var isGridUpdate = false;
                    if (detachedvalues.join() == '') {
                        isGridUpdate = true;
                    }
                    Clinical_ImmunizationDetail.getAdministerVaccineInfo(SelectedImmunization.join(), "", (Clinical_Immunization.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val()), true, false);

                }
            }

        }


        //When User has attached Allergies with notes than add on note button should be disabled
        $("#" + Clinical_Immunization.params.PanelID + "  #divTherapeutic_Paging #btnAddImmunizationToNotes").prop('disabled', true);
        utility.DisplayMessages("Successfully Updated", 1);
        //}
        // else {
        //    utility.DisplayMessages(strMessage, 2);
        //  }
        //});
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
        if (Clinical_Immunization.params && Clinical_Immunization.params.ParentCtrl && Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
            UnloadActionPan(Clinical_Immunization.params.ParentCtrl, 'Clinical_Immunization');
        }
    },

    OpenCatchupScheduler: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                var PanelID = "";
                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                    //params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                }
                else if (Clinical_Immunization.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Immunization.params.ParentCtrl == "Clinical_FaceSheet") {
                    params["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                    // params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                }
                else {
                    params["ParentCtrl"] = 'clinicalTabImmunization';
                    PanelID = 'pnlClinicalImmunization';
                    //params["PanelID"] = 'pnlClinicalImmunization';
                }

                params["patientID"] = Clinical_Immunization.params["patientID"];
                LoadActionPan('Immunization_CatchupScheduler', params, PanelID);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });

    },

    unLoadTab: function () {
        var parentPanelId = null;
        if (Clinical_Immunization.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Immunization.params.ParentCtrl == "Clinical_FaceSheet") {
            if (Clinical_Immunization.params.ParentCtrl == "Clinical_FaceSheet") {
                parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                Clinical_FaceSheet.params.ChildPanelID = null;
                UnloadActionPan(Clinical_Immunization.params.ParentCtrl, 'Clinical_Immunization', null, parentPanelId);
            } else {
                UnloadActionPan(Clinical_Immunization.params.ParentCtrl, 'Clinical_Immunization');
            }
            Clinical_FaceSheet.loadFaceSheet();
        }
        else if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
            var exist = false;
            if ($("#" + Clinical_Immunization.params.PanelID + " #listHx").hasClass('active')) {
                $("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx tbody").find('input[type="checkbox"]').each(function () {
                    if (this.checked) {
                        exist = true;
                    }
                    if (exist) {
                        return false;
                    }
                });
            }
            if (exist) {
                utility.myConfirmNote('1', function () {
                    if ($("#" + Clinical_Immunization.params.PanelID + " #listHx").hasClass('active')) {
                        Clinical_Immunization.addImmunizationToNotes();
                    }
                    UnloadActionPan(Clinical_Immunization.params.ParentCtrl, 'Clinical_Immunization');
                }, "", function () {
                    UnloadActionPan(Clinical_Immunization.params.ParentCtrl, 'Clinical_Immunization');
                });
            }
            else {
                UnloadActionPan(Clinical_Immunization.params.ParentCtrl, 'Clinical_Immunization');
            }
        }
        else {
            if (Clinical_Immunization.params["FromAdmin"] == "0") {
                if (Clinical_Immunization.params != null && Clinical_Immunization.params.ParentCtrl != null) {
                    if ($("#" + Clinical_Immunization.params.PanelID + " #listHx").hasClass('active')) {
                        Clinical_Immunization.addImmunizationToNotes();
                    }
                    UnloadActionPan(Clinical_Immunization.params.ParentCtrl, 'Clinical_Immunization');
                } else
                    UnloadActionPan(null, 'Clinical_Immunization');
            } else {
                RemoveAdminTab();
            }
        }
    },



    //Function Name: Generate_HL7Immunization_Records
    //Author Name: Talha Tanweer
    //Created Date : 18-04-2016
    //Modified Date: 21-09-2016
    //Description: Generate HL7 Immunization Messages for Selected Records
    Generate_HL7Immunization_Records: function () {
        var checkedElements = $("#" + Clinical_Immunization.params.PanelID + " #dgvImmunizationVaccineHx tbody input[id*='ForPrintOrGenerate_']:checked");
        var arrayCheckedItemsVaccineHxIdsValues = new Array(checkedElements.length);

        $(checkedElements).each(function (index, element) {
            // arrayCheckedItemsVaccineHxIdsValues[index] = $(element).prop("id");
            arrayCheckedItemsVaccineHxIdsValues[index] = $(element).prop("id").split('_')[1];
        });

        var objData = new Object();
        var csvVaccineHxIds = arrayCheckedItemsVaccineHxIdsValues.join(",");

        if (csvVaccineHxIds !== "") {
            objData["commandType"] = "generate_hl7_immunization";
            objData["VaccineHxIds"] = csvVaccineHxIds;
            var data = JSON.stringify(objData);
            MDVisionService.APIService(data, "MEDICAL", "Immunization").done(function (response) {
                response = JSON.parse(response);
                if (response != null) {
                    utility.DisplayMessages(response.Message, 1);
                    var patientId = Clinical_Immunization.params.patientID;
                    var uri = '';
                    var dt = new Date();
                    var strMimeType = "application/octet-stream";
                    var dateString = dt.getFullYear() + '/' + dt.getMonth() + '/' + dt.getDate() + '/' + dt.getHours() + '/' + dt.getMinutes() + '/' + dt.getSeconds();
                    download(uri + response.HL7Message, +patientId + "[" + csvVaccineHxIds + "]-" + dateString.replace(/\//g, '') + ".txt", strMimeType);
                }
            });
        } else {
            utility.DisplayMessages("Please Select atleast 1 record", 3);
        }

    },

    AddTherapeuticInjection: function () {
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                PARAMS = {}
                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
                    PARAMS["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                    PARAMS["from"] = 'clinicalTabProgressNote';
                    PARAMS["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                    PARAMS["ParentPanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                }
                else if (Clinical_Immunization.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Immunization.params.ParentCtrl == "Clinical_FaceSheet") {
                    PARAMS["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                    PARAMS["from"] = 'clinicalTabFaceSheet';
                    PARAMS["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                    PARAMS["ParentPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                }
                else {
                    PARAMS["ParentCtrl"] = 'clinicalTabImmunization';
                    PanelID = 'pnlClinicalImmunization';
                    PARAMS["from"] = 'clinicalTabImmunization';
                    PARAMS["PanelID"] = 'pnlClinicalImmunization';
                    PARAMS["ParentPanelID"] = 'pnlClinicalImmunization';
                }
                PARAMS["PatientId"] = Clinical_Immunization.params.patientID;
                PARAMS["FromAdmin"] = 0;
                PARAMS["NotesId"] = Clinical_Immunization.params.NotesId == null ? 0 : Clinical_Immunization.params.NotesId;
                PARAMS["mode"] = "Add";
                LoadActionPan("Immunization_TherapeuticInjection", PARAMS, PanelID);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    ImmunizationTherapeuticInjectionSearch: function (PrimaryID, PageNo, rpp, ActiveTab) {
        if (ActiveTab) {
            $('#' + Clinical_Immunization.params.PanelID).find('[id*="list"]').removeClass('active');
            $('#' + Clinical_Immunization.params.PanelID).find('[id*="tab_"]').removeClass('active');
            $('#' + Clinical_Immunization.params.PanelID + ' #listTherapeuticInjection').addClass('active');
            $('#' + Clinical_Immunization.params.PanelID + ' #tab_TherapeuticInjection').addClass('active');
        }
        

        var dfd = $.Deferred();

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Clinical_Immunization.searchImmunizationTherapeuticInjection(PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
                            if ($("#" + Clinical_Immunization.params.PanelID + " #dgvTherapeutic thead tr #SelectRecord").length == 0) {
                                $("#" + Clinical_Immunization.params.PanelID + " #dgvTherapeutic thead tr")
                                    .prepend(' <th id="SelectRecord" class="size10 center" coltype="checkbox"> <input type="checkbox" id="chkHeaderImmunization" onchange="Clinical_Immunization.SelectUnselectAllRecords(this,0);"   class="input-block" coltype="checkbox"/> </th>');
                            }
                        } else {
                            //if ($("#" + Clinical_Immunization.params.PanelID + " #dgvTherapeutic thead tr #SelectRecord").length == 0) {
                            //    $("#" + Clinical_Immunization.params.PanelID + " #dgvTherapeutic thead tr")
                            //        .prepend(' <th id="SelectRecord" class="size10 center" coltype="checkbox"> <input type="checkbox" onchange="Clinical_Immunization.SelectUnselectAllRecords(this);"  class="input-block" coltype="checkbox"/> </th>');
                            //}
                        }


                        Clinical_Immunization.TherapeuticInjectionGridLoad(response);
                        var TableControl = Clinical_Immunization.params.PanelID + " #pnlTherapeutic_Result #dgvTherapeutic";
                        var PagingPanelControlID = Clinical_Immunization.params.PanelID + " #divTherapeutic_Paging";
                        var ClassControlName = "Clinical_Immunization";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;

                        setTimeout(CreatePagination(response.TherapeuticInjectionCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_Immunization.ImmunizationTherapeuticInjectionSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                        setTimeout(function (TherapeuticInjectionCount) {
                            if ($("#" + Clinical_Immunization.params.PanelID + " #dgvTherapeutic tbody tr").length == 1) {
                                if ($($("#" + Clinical_Immunization.params.PanelID + " #dgvTherapeutic tbody tr")[0]).find("td").length == 1) {
                                    if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNotes").length == 0) {
                                        $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Clinical_Immunization.addImmunizationToNotes();" disabled id="btnAddImmunizationToNotes">Add on Note</button>').insertAfter("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging .pagination")
                                    }
                                }
                                else {
                                    if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNotes").length == 1) {
                                        $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNotes").remove();
                                    }
                                    if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Immunization.params.PanelID + "  #divTherapeutic_Paging #btnAddImmunizationToNotes").length == 0) {
                                        $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Clinical_Immunization.addImmunizationToNotes();" disabled id="btnAddImmunizationToNotes">Add on Note</button>').insertAfter("#" + Clinical_Immunization.params.PanelID + "  #divTherapeutic_Paging .pagination")
                                    }
                                }
                            }
                            else {
                                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNotes").length == 1) {
                                    $("#" + Clinical_Immunization.params.PanelID + "  #divImmunizationVaccineHx_Paging #btnAddImmunizationToNotes").remove();
                                }
                                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Immunization.params.PanelID + "  #divTherapeutic_Paging #btnAddImmunizationToNotes").length == 0) {
                                    $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Clinical_Immunization.addImmunizationToNotes();" disabled id="btnAddImmunizationToNotes">Add on Note</button>').insertAfter("#" + Clinical_Immunization.params.PanelID + "  #divTherapeutic_Paging .pagination")
                                }
                            }

                            dfd.resolve();
                        }, 11, response.iTotalDisplayRecords);
                        //setTimeout(function () {
                        //    $('#' + Clinical_Immunization.params.PanelID + " #VaccineBtn").click();

                        //}, 13);

                    } else {

                        $('#' + Clinical_Immunization.params.PanelID + ' #pnlTherapeutic_Result #dgvTherapeutic').DataTable({
                            "language": {
                                "emptyTable": "No Therapeutic Injection found."
                            },
                            "autoWidth": false,
                            "bLengthChange": false,
                            "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }],
                            "bDestroy": true
                        });
                        setTimeout(function () {
                            //$('#' + Clinical_Immunization.params.PanelID + " #VaccineBtn").click();
                            dfd.resolve();
                        }, 5);

                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
                dfd.resolve();
            }
        });
        //==============================});
        return dfd;
    },
    searchImmunizationTherapeuticInjection: function (pageNumber, rowsPerPage) {

        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }

        //var IsActive = null;
        //IsActive = $("#" + Clinical_Immunization.params.PanelID + ' #pnlVaccineHx_Result #divSwitch #switchActive').attr('IsActive');

        //if (IsActive == null) {
        //    IsActive = "1";
        //}

        var objData = new Object();
        objData["PatientId"] = Clinical_Immunization.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        //objData["IsActive"] = IsActive;
        objData["NoteId"] = Clinical_Immunization.params.NotesId == null ? 0 : Clinical_Immunization.params.NotesId;
        objData["pageNumber"] = pageNumber;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_Immunization_Therapeutic_Injection";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "IMMUNIZATIONTHERAPEUTICINJECTION");

    },
    TherapeuticInjectionGridLoad: function (response) {
        $("#" + Clinical_Immunization.params.PanelID + " #pnlTherapeutic_Result #dgvTherapeutic").dataTable().fnDestroy();
        $("#" + Clinical_Immunization.params.PanelID + " #pnlTherapeutic_Result #dgvTherapeutic tbody").find("tr").remove();
        var TherapeuticInjectionLoad_JSON = JSON.parse(response.TherapeuticInjectionLoad_JSON);
        if (TherapeuticInjectionLoad_JSON.length > 0) {
            $.each(TherapeuticInjectionLoad_JSON, function (i, item) {
                var immTherInjectionId = item.ImmTherInjectionId;
                var $row = $('<tr/>');
                $row.attr("id", immTherInjectionId);
                var AddParameters = "'Edit'," + item.ImmTherInjectionId + ",event,'" + item.Type + "'";
                var MethodMode = 'Clinical_Immunization.ClickOnTherapeuticInjectionGrid(' + AddParameters + ');';
                $row.attr("onclick", MethodMode);
                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ImmTherInjectionId + "thera", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ImmTherInjectionId + "thera", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }
                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_Immunization.enableAddImmunization(this);" id="' + item.ImmTherInjectionId + 'thera" name="SelectCheckBoxImmunization" ' + Checked + ' class="input-block text-center"/></td>';
                } else {
                    SelectionCheckBoxColumn = '';
                }
                $row.append('<td style="display:none">' + item.ModifiedOn + '</td>' + SelectionCheckBoxColumn + '<td>' + item.TherapeuticInjection + '</td><td>' + (item.Dose != "" ? item.Dose + " " + item.Amount : "") + '</td><td>' + item.AdministrationDate + '</td><td >' + item.ProviderName + '</td><td >' + item.GivenByName + '</td><td>' + item.Type + '</td>');
                $("#" + Clinical_Immunization.params.PanelID + " #dgvTherapeutic tbody").append($row);
            });


            var TherapeuticRows = $("#" + Clinical_Immunization.params.PanelID + " #pnlTherapeutic_Result #dgvTherapeutic tbody").find("tr");

            if (TherapeuticRows.length < 1) {
                $("#" + Clinical_Immunization.params.PanelID + " #pnlTherapeutic_Result #dgvTherapeutic").DataTable({
                    "language": {
                        "emptyTable": "No Therapeutic Injection Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }, { "targets": [7], "visible": false }]
                });

                $("#pnlClinicalNotes #divClinicalDraftNotesPaging").css("display", "none");
            }
        }
        else {
            $("#" + Clinical_Immunization.params.PanelID + " #divTherapeutic_Paging").css("display", "none");


            $("#" + Clinical_Immunization.params.PanelID + " #pnlTherapeutic_Result #dgvTherapeutic").DataTable({
                "language": {
                    "emptyTable": "No Therapeutic Injection Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
    },

    ClickOnTherapeuticInjectionGrid: function (Mode, ImmTherInjectionId, event, Type) {

        if (event != null) {
            if ($(event.target).is('input[type=checkbox]')) {
                return;
            }
        }

        PARAMS = {}
        if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
            PARAMS["ParentCtrl"] = 'Clinical_Immunization';
            PanelID = 'pnlClinicalProgressNote #pnlClinicalImmunization';
            PARAMS["from"] = 'clinicalTabProgressNote';
            PARAMS["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
            PARAMS["ParentPanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
        }
        else if (Clinical_Immunization.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Immunization.params.ParentCtrl == "Clinical_FaceSheet") {
            PARAMS["ParentCtrl"] = 'Clinical_Immunization';
            PanelID = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
            PARAMS["from"] = 'clinicalTabFaceSheet';
            PARAMS["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
            PARAMS["ParentPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
        }
        else {
            PARAMS["ParentCtrl"] = 'clinicalTabImmunization';
            PanelID = 'pnlClinicalImmunization';
            PARAMS["from"] = 'clinicalTabImmunization';
            PARAMS["PanelID"] = 'pnlClinicalImmunization';
            PARAMS["ParentPanelID"] = 'pnlClinicalImmunization';
        }
        PARAMS["Type"] = Type;
        PARAMS["PatientId"] = Clinical_Immunization.params.patientID;
        PARAMS["FromAdmin"] = 0;
        PARAMS["NotesId"] = Clinical_Immunization.params.NotesId == null ? 0 : Clinical_Immunization.params.NotesId;
        PARAMS["mode"] = Mode;
        PARAMS["ImmTherInjectionId"] = ImmTherInjectionId;

        LoadActionPan("Immunization_TherapeuticInjection", PARAMS, PanelID);

    },
    maxLengthCheck: function (event) {
        var t = event.target;
        if (t.hasAttribute('maxlength'))
            t.value = t.value.slice(0, t.getAttribute('maxlength'));
    },
    SendQuery: function () {
        var self = $("#" + Clinical_Immunization.params.PanelID + " #tab_HistoryForecast");
        var myJSON = self.getMyJSONByName();

        Clinical_Immunization.SendQuery_DBCall(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                //Clinical_OrderSetDetails.MedicationSearch();

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    SendQuery_DBCall: function (myJSON) {
        var objData = JSON.parse(myJSON);
        objData.PatientId = Clinical_Immunization.params.patientID;
        objData["commandType"] = "Send_Query";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },


    ImmunizationQuerySearch: function (PrimaryID, PageNo, rpp) {
        var dfd = $.Deferred();

        Clinical_Immunization.searchImmunizationQuery(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                Clinical_Immunization.ImmQueryGridLoad(response);
                var TableControl = Clinical_Immunization.params.PanelID + " #pnlHxHistoryForecast_Result #dgvHxHistoryForecast";
                var PagingPanelControlID = Clinical_Immunization.params.PanelID + " #divHxHistoryForecast_Paging";
                var ClassControlName = "Clinical_Immunization";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;

                setTimeout(CreatePagination(response.ImmQueryCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Clinical_Immunization.ImmunizationQuerySearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);


            } else {
                Clinical_Immunization.ImmQueryCount = 0;
                $('#' + Clinical_Immunization.params.PanelID + ' #pnlHxHistoryForecast_Result #dgvHxHistoryForecast').DataTable({
                    "language": {
                        "emptyTable": "No Query found."
                    },
                    "autoWidth": false,
                    "bLengthChange": false,
                    "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }],
                    "bDestroy": true
                });
                setTimeout(function () {
                    dfd.resolve();
                }, 5);
            }
        });

    },
    searchImmunizationQuery: function (pageNumber, rowsPerPage, QueryId) {

        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        if (QueryId && QueryId > 0) {
            objData["QueryId"] = QueryId;
        }
        else {
            objData["QueryId"] = "0";
        }
        objData["PatientId"] = Clinical_Immunization.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["pageNumber"] = pageNumber;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_Immunization_Query";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },
    ImmQueryGridLoad: function (response) {
        if ($.fn.dataTable.isDataTable("#" + Clinical_Immunization.params.PanelID + " #pnlHxHistoryForecast_Result #dgvHxHistoryForecast")) {
            $("#" + Clinical_Immunization.params.PanelID + " #pnlHxHistoryForecast_Result #dgvHxHistoryForecast").dataTable().fnClearTable();
            $("#" + Clinical_Immunization.params.PanelID + " #pnlHxHistoryForecast_Result #dgvHxHistoryForecast").dataTable().fnDestroy();
            $("#" + Clinical_Immunization.params.PanelID + " #pnlHxHistoryForecast_Result #dgvHxHistoryForecast tbody").find("tr").remove();
        } else {
            //for stop make dublicate Datatables
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == 'dgvHxHistoryForecast') {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Clinical_Immunization.params.PanelID + " #pnlHxHistoryForecast_Result #dgvHxHistoryForecast tbody").find("tr").remove();
                    $("#" + Clinical_Immunization.params.PanelID + " #pnlHxHistoryForecast_Result #dgvHxHistoryForecast").parent().parent().find('div.row').remove();
                }
            })
        }

        var ImmQuery_JSON = response.ImmQuery_JSON;
        if (response.ImmQueryCount > 0) {
            $.each(ImmQuery_JSON, function (i, item) {
                var immQueryId = item.QueryId;
                var $row = $('<tr/>');
                $row.attr("id", immQueryId);
                var UploadIcon = '<span class="btn-xs ml-xxs btn-file" style="cursor: pointer;"><i class="fa fa-upload"></i><input type="file" class="btn btn-file" name="Import_Acknowledgements_file" accept="application/txt" id="Import_Acknowledgements_file" allowfiles="txt" onchange="Clinical_Immunization.UploadQueryResponseFile( ' + item.QueryId + ',this);"/></span>';
                $row.append('<td style="display:none">' + item.RequestDateTime + '</td><td>' + UploadIcon + '<a class="btn  btn-xs" href="#" onclick="Clinical_Immunization.DeleteImmQuery(' + item.QueryId + ');" title="Delete Record"><i class="fa fa-close red"></i></a><a class="btn  btn-xs" href="#" onclick="Clinical_Immunization.DownloadImmQuery(' + item.QueryId + ');" title="Download Record"><i class="fa fa-download"></i></a></td><td>Unknown</td><td>' + item.RequestDateTime + '</td><td >' + item.GivenBy + '</td><td >Successful</td>');
                $("#" + Clinical_Immunization.params.PanelID + " #dgvHxHistoryForecast tbody").append($row);
            });


            var ImmQueryRows = $("#" + Clinical_Immunization.params.PanelID + " #pnlHxHistoryForecast_Result #dgvHxHistoryForecast tbody").find("tr");

            if (ImmQueryRows.length < 1) {
                $("#" + Clinical_Immunization.params.PanelID + " #pnlHxHistoryForecast_Result #dgvHxHistoryForecast").DataTable({
                    "language": {
                        "emptyTable": "No Query Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }, { "targets": [7], "visible": false }]
                });

                $("#pnlClinicalImmunization #divHxHistoryForecast_Paging").css("display", "none");
            }
        }
        else {
            $('#' + Clinical_Immunization.params.PanelID + ' #pnlHxHistoryForecast_Result #dgvHxHistoryForecast').DataTable({
                "language": {
                    "emptyTable": "No Query Found"
                },
                "autoWidth": false,
                "bLengthChange": false,
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }],
                "bDestroy": true
            });
        }



        // get Actions

    },
    DownloadImmQuery: function (QueryId) {
        Clinical_Immunization.searchImmunizationQuery(1, 1, QueryId).done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                var ImmQuery_JSON = response.ImmQuery_JSON;
                if (ImmQuery_JSON.length > 0) {
                    var uri = '';
                    var dt = new Date();
                    var strMimeType = "application/octet-stream";
                    var dateString = dt.getFullYear() + '/' + dt.getMonth() + '/' + dt.getDate() + '/' + dt.getHours() + '/' + dt.getMinutes() + '/' + dt.getSeconds();
                    download(uri + ImmQuery_JSON[0].HL7Message, +Clinical_Immunization.params.patientID + "[" + QueryId + "]-" + "ImmQuery_" + dateString.replace(/\//g, '') + ".txt", strMimeType);
                }
                else {
                    utility.DisplayMessages("No Record Found", 3);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
    },
    DownloadImmQueryResponse: function (ImmunizationQueryResponseId) {
        Clinical_Immunization.searchImmunizationQueryResponse(1, 1, ImmunizationQueryResponseId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var ImmQueryResponse_JSON = response.ImmQueryResponse_JSON;
                if (ImmQueryResponse_JSON.length > 0) {
                    var uri = '';
                    var dt = new Date();
                    var strMimeType = "application/octet-stream";
                    var dateString = dt.getFullYear() + '/' + dt.getMonth() + '/' + dt.getDate() + '/' + dt.getHours() + '/' + dt.getMinutes() + '/' + dt.getSeconds();
                    download(uri + ImmQueryResponse_JSON[0].File, +Clinical_Immunization.params.patientID + "[" + ImmunizationQueryResponseId + "]-" + "ImmQuery_" + dateString.replace(/\//g, '') + ".txt", strMimeType);
                }
                else {
                    utility.DisplayMessages("No Record Found", 3);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
    },
    DeleteImmQueryResponse: function (ImmunizationQueryResponseId) {
        var row = $('#' + Clinical_Immunization.params.PanelID + ' #dgvQueryResonse_HistoryForecast tbody tr#' + ImmunizationQueryResponseId);
        var id = $(row).attr("id");
        utility.myConfirm('1', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Immunization.ImmQueryResponseDelete_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_Immunization.ImmunizationQueryResponseSearch();
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
    DeleteImmQuery: function (QueryId) {
        var row = $('#' + Clinical_Immunization.params.PanelID + ' #dgvHxHistoryForecast tbody tr#' + QueryId);
        var id = $(row).attr("id");
        utility.myConfirm('1', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Immunization.ImmQueryDelete_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_Immunization.ImmunizationQuerySearch();
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

    ImmQueryResponseDelete_DBCall: function (ImmunizationQueryResponseId) {
        var objData = new Object();
        objData["ImmunizationQueryResponseId"] = ImmunizationQueryResponseId;
        objData["commandType"] = "DELETE_ImmunizationQueryResponse";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },
    ImmQueryDelete_DBCall: function (QueryId) {
        var objData = new Object();
        objData["QueryId"] = QueryId;
        objData["commandType"] = "DELETE_ImmunizationQuery";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },
    ValidateFileType: function (fName, input) {
        // Use a regular expression to trim everything before final dot
        var extension = fName.replace(/^.*\./, '');
        // Iff there is no dot anywhere in filename, we would have extension == filename,
        // so we account for this possibility now
        if (extension == fName) {
            extension = '';
        }
        else {
            // if there is an extension, we convert to lower case
            extension = extension.toLowerCase();
        }

        try {

            var exs = $(input).attr("allowfiles");
            var allowfiles = exs.split(",");
            if (allowfiles.indexOf(extension) >= 0)
                return true;
            else
                return false;

        } catch (ex) {
            console.log(ex);
            return false;
        }
    },
    UploadQueryResponseFile: function (QueryId,input) {
        if (input.files) {
            var fName = input.files[0];
            fName = fName.name;
            //if (fName.length <= 100) {
            if (Clinical_Immunization.ValidateFileType(fName, input)) {

                var reader = new FileReader();
                reader.onload = function (event) {
                    var data = reader.result;
                    Clinical_Immunization.SaveImportImmResponseHL7_DB_CALL(fName, data, QueryId).done(function (response) {
                        var extension = fName.replace(/^.*\./, '');
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            $(input).val(null);
                            Clinical_Immunization.ImmunizationQueryResponseSearch('', 1, 15);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                };
                reader.onerror = function () {
                    utility.DisplayMessages('Unable to read file data', 3);
                };
                reader.readAsText(input.files[0]);
            }
            else {
                utility.DisplayMessages("Only " + $(input).attr("allowfiles") + " files are allowed", 2);
                $(input).val(null);
            }
            //}
            //else {
            //    utility.DisplayMessages("File name should be 20 characters long.", 2);
            //}
        }
    },

    SaveImportImmResponseHL7_DB_CALL: function (fileName, data,QueryId) {
        var data_ = utility.replaceSpecialCharacters(data);
        var objData = new Object();
        objData["PatientId"] = Clinical_Immunization.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["File"] = data_;
        objData["QueryId"] = QueryId;
        objData["fileName"] = fileName;
        objData["commandType"] = "save_immresponse_file";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    ImmunizationQueryResponseSearch: function (PrimaryID, PageNo, rpp) {
        var dfd = $.Deferred();

        Clinical_Immunization.searchImmunizationQueryResponse(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                Clinical_Immunization.ImmQueryResponseGridLoad(response);
                var TableControl = Clinical_Immunization.params.PanelID + " #pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast";
                var PagingPanelControlID = Clinical_Immunization.params.PanelID + " #divQueryResponseHistoryForecast_Paging";
                var ClassControlName = "Clinical_Immunization";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;

                setTimeout(CreatePagination(response.ImmQueryCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Clinical_Immunization.ImmunizationQueryResponseSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);


            } else {
                $('#' + Clinical_Immunization.params.PanelID + ' #pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast').DataTable({
                    "language": {
                        "emptyTable": "No Query Response found."
                    },
                    "autoWidth": false,
                    "bLengthChange": false,
                    "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }],
                    "bDestroy": true
                });
                setTimeout(function () {
                    dfd.resolve();
                }, 5);
            }
        });

    },
    searchImmunizationQueryResponse: function (pageNumber, rowsPerPage, ImmunizationQueryResponseId) {

        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        if (ImmunizationQueryResponseId && ImmunizationQueryResponseId > 0) {
            objData["ImmunizationQueryResponseId"] = ImmunizationQueryResponseId;
        }
        else {
            objData["ImmunizationQueryResponseId"] = "0";
        }
        objData["PatientId"] = Clinical_Immunization.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["pageNumber"] = pageNumber;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_Immunization_Query_Response";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },
    ImmQueryResponseGridLoad: function (response) {
        if ($.fn.dataTable.isDataTable("#" + Clinical_Immunization.params.PanelID + " #pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast")) {
            $("#" + Clinical_Immunization.params.PanelID + " #pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast").dataTable().fnClearTable();
            $("#" + Clinical_Immunization.params.PanelID + " #pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast").dataTable().fnDestroy();
            $("#" + Clinical_Immunization.params.PanelID + " #pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast tbody").find("tr").remove();
        } else {
            //for stop make dublicate Datatables
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == 'dgvQueryResonse_HistoryForecast') {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Clinical_Immunization.params.PanelID + " #pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast tbody").find("tr").remove();
                    $("#" + Clinical_Immunization.params.PanelID + " #pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast").parent().parent().find('div.row').remove();
                }
            })
        }



        var ImmQueryResponse_JSON = response.ImmQueryResponse_JSON;
        if (response.ImmQueryResponseCount > 0) {
            $.each(ImmQueryResponse_JSON, function (i, item) {

                var $row = $('<tr/>');
                $row.attr("id", item.ImmunizationQueryResponseId);

                var PatFirstName = "";
                var PatLastName = "";
                if (item.PatientName) {
                    var patSplit = item.PatientName.split(',');
                    if (patSplit.length == 2) {
                        PatFirstName = patSplit[1].trim();
                        PatLastName = patSplit[0].trim();
                    }
                }
                var viewDetail = '';
                if (item.ResponseType == "Single (Exact) Match Found") {
                    viewDetail = '<a id="lnklblResponseView" class="btn btn-link pull-left btn-xs"target="_blank" title="View Detail" onclick=" Clinical_Immunization.OpenQueryResponseDetail(' + item.ImmunizationQueryResponseId + ');">View Detail</a>';
                }
                $row.append('<td style="display:none">' + item.CreatedOn + '</td><td><a class="btn  btn-xs" href="#" onclick="Clinical_Immunization.DeleteImmQueryResponse(' + item.ImmunizationQueryResponseId + ');" title="Delete Record"><i class="fa fa-close red"></i></a><a class="btn  btn-xs" href="#" onclick="Clinical_Immunization.DownloadImmQueryResponse(' + item.ImmunizationQueryResponseId + ');" title="Download Record"><i class="fa fa-download"></i></a></td><td>' + PatFirstName + '</td><td>' + PatLastName + '</td><td >' + item.CreatedOn + '</td><td >' + item.DOB + '</td><td >' + item.ResponseType + '</td><td>' + viewDetail + '</td>');
                $("#" + Clinical_Immunization.params.PanelID + " #dgvQueryResonse_HistoryForecast tbody").append($row);
            });


            var ImmQueryRows = $("#" + Clinical_Immunization.params.PanelID + " #pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast tbody").find("tr");

            if (ImmQueryRows.length > 0) {
                var IsDataTable = $.fn.dataTable.isDataTable('#pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast')
                if (!IsDataTable) {
                    $("#" + Clinical_Immunization.params.PanelID + " #pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
                }
            }
        }
        else {
            $('#' + Clinical_Immunization.params.PanelID + ' #pnlQueryResonse_HistoryForecast #dgvQueryResonse_HistoryForecast').DataTable({
                "language": {
                    "emptyTable": "No Query Response Found"
                },
                "autoWidth": false,
                "bLengthChange": false,
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }],
                "bDestroy": true
            });
        }



        // get Actions

    },
    OpenQueryResponseDetail: function (ImmunizationQueryResponseId) {



        PARAMS = {}
        if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
            PARAMS["ParentCtrl"] = 'Clinical_Immunization';
            PanelID = 'pnlClinicalProgressNote #pnlClinicalImmunization';
            PARAMS["from"] = 'clinicalTabProgressNote';
            PARAMS["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
            PARAMS["ParentPanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
        }
        else if (Clinical_Immunization.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Immunization.params.ParentCtrl == "Clinical_FaceSheet") {
            PARAMS["ParentCtrl"] = 'Clinical_Immunization';
            PanelID = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
            PARAMS["from"] = 'clinicalTabFaceSheet';
            PARAMS["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
            PARAMS["ParentPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
        }
        else {
            PARAMS["ParentCtrl"] = 'clinicalTabImmunization';
            PanelID = 'pnlClinicalImmunization';
            PARAMS["from"] = 'clinicalTabImmunization';
            PARAMS["PanelID"] = 'pnlClinicalImmunization';
            PARAMS["ParentPanelID"] = 'pnlClinicalImmunization';
        }
        PARAMS["PatientId"] = Clinical_Immunization.params.patientID;
        PARAMS["FromAdmin"] = 0;
        PARAMS["NotesId"] = Clinical_Immunization.params.NotesId == null ? 0 : Clinical_Immunization.params.NotesId;
        PARAMS["mode"] = "View";
        PARAMS["ImmunizationQueryResponseId"] = ImmunizationQueryResponseId;

        LoadActionPan("Immunization_QueryResponseDetail", PARAMS, PanelID);
    },
    MrnStateIDRegisteryChange: function (obj) {
        if ($(obj).val() != "") {
            if ($('#' + Clinical_Immunization.params.PanelID + " #MrnStateIDRegisteryDataDiv").hasClass("hidden")) {
                $('#' + Clinical_Immunization.params.PanelID + " #MrnStateIDRegisteryDataDiv").removeClass("hidden");
            }
            $('#' + Clinical_Immunization.params.PanelID + " #txtMrnStateIDRegisteryData").val("");
            if ($(obj).val() == 1) {
                $('#' + Clinical_Immunization.params.PanelID + " #txtMrnStateIDRegisteryData").val(Clinical_Immunization.params.MRN);
            }
        }
        else {
            if (!$('#' + Clinical_Immunization.params.PanelID + " #MrnStateIDRegisteryDataDiv").hasClass("hidden")) {
                $('#' + Clinical_Immunization.params.PanelID + " #MrnStateIDRegisteryDataDiv").addClass("hidden");
            }
        }
    },
    UploadAcknowledgements: function (VaccineHxId, input,fromBatch) {

        if (input.files) {
            var fName = input.files[0];
            fName = fName.name;
            //if (fName.length <= 100) {
            if (Clinical_Immunization.ValidateFileType(fName, input)) {

                var reader = new FileReader();
                reader.onload = function (event) {
                    var data = reader.result;
                    Clinical_Immunization.SaveImportedVXUAcknowledgements_DB_CALL(data, VaccineHxId).done(function (response) {
                        var extension = fName.replace(/^.*\./, '');
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            $(input).val(null);
                            if (fromBatch) {
                                Batch_ImportHL7ImmunizationBatch.HL7ImmunizationGridTabChange('OutBound');
                            }
                            else {
                                Clinical_Immunization.LoadSchedlerAndActiveTabData('Hx', true);
                            }
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                };
                reader.onerror = function () {
                    utility.DisplayMessages('Unable to read file data', 3);
                };
                reader.readAsText(input.files[0]);
            }
            else {
                utility.DisplayMessages("Only " + $(input).attr("allowfiles") + " files are allowed", 2);
                $(input).val(null);
            }
            //}
            //else {
            //    utility.DisplayMessages("File name should be 20 characters long.", 2);
            //}
        }
    },
    SaveImportedVXUAcknowledgements_DB_CALL: function (data, VaccineHxId) {
        var data_ = utility.replaceSpecialCharacters(data);
        var objData = new Object();
        objData["File"] = data_;
        objData["VaccineHxId"] = VaccineHxId;
        objData["commandType"] = "save_immacknow_file";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },
    DownloadImmAcknowledgement: function (VaccineHxId) {
        Clinical_Immunization.searchVaccineHxById(VaccineHxId).done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                var ImmAcknow_JSON = response.ImmAcknow_JSON;
                if (ImmAcknow_JSON.length > 0) {
                    var uri = '';
                    var dt = new Date();
                    var strMimeType = "application/octet-stream";
                    var dateString = dt.getFullYear() + '/' + dt.getMonth() + '/' + dt.getDate() + '/' + dt.getHours() + '/' + dt.getMinutes() + '/' + dt.getSeconds();
                    download(uri + ImmAcknow_JSON[0].AcknowledgementFile, +Clinical_Immunization.params.patientID + "[VaccineHx#" + VaccineHxId + "]-" + "ImmAcknowledgement_" + dateString.replace(/\//g, '') + ".txt", strMimeType);
                }
                else {
                    utility.DisplayMessages("No Record Found", 3);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
    },
    searchVaccineHxById: function (VaccineHxId) {

        var objData = {};
        objData["VaccineHxId"] = VaccineHxId;
        objData["commandType"] = "SEARCH_Vaccine_HX_Id";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },
    AddHistoryDose: function (MODE) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", MODE, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];

                var PanelID = "";

                if (Clinical_Immunization.params.ParentCtrl == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                    params["from"] = 'clinicalTabProgressNote';
                    params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                    params["ParentPanelID"] = 'pnlClinicalProgressNote #pnlClinicalImmunization';
                }
                else if (Clinical_Immunization.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Immunization.params.ParentCtrl == "Clinical_FaceSheet") {
                    params["ParentCtrl"] = 'Clinical_Immunization';
                    PanelID = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                    params["from"] = 'clinicalTabFaceSheet';
                    params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                    params["ParentPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalImmunization';
                }
                else {
                    params["ParentCtrl"] = 'clinicalTabImmunization';
                    PanelID = 'pnlClinicalImmunization';
                    params["from"] = 'clinicalTabImmunization';
                    params["PanelID"] = 'pnlClinicalImmunization';
                    params["ParentPanelID"] = 'pnlClinicalImmunization';
                }
                params["OrderSetId"] = "";
                params["FromAdmin"] = 0;
                params["TabId"] = "HistoryDose";
                params["VaccineScheduleId"] = 0;
                params["mode"] = MODE;
                params["patientID"] = Clinical_Immunization.params["patientID"];
                LoadActionPan('Clinical_ImmunizationDetail', params, PanelID);
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },
}

