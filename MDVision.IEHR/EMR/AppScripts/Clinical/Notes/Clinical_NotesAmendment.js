/*Author : Khaleel Ur Rehman.
Date : 16-03-2016
Purpose : To Handle Amendment when view a Note.
*/
Clinical_NotesAmendment = {
    Load: function (params) {
        Clinical_NotesAmendment.params = params;

        if (Clinical_NotesAmendment.params.PanelID != 'pnlClinical_NotesAmendment') {
            Clinical_NotesAmendment.params.PanelID = Clinical_NotesAmendment.params.PanelID + ' #pnlClinical_NotesAmendment';
        } else {
            Clinical_NotesAmendment.params.PanelID = 'pnlClinical_NotesAmendment';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_NotesAmendment.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        var self = $('#' + Clinical_NotesAmendment.params.PanelID);
        if (Clinical_NotesAmendment.params.ParentCtrl == 'mstrTabReports') {
            $("#" + Clinical_NotesAmendment.params.PanelID + " #AmendmentRequestedBy").hide();
            $("#" + Clinical_NotesAmendment.params.PanelID + " #AmendmentComments").hide();
            $("#" + Clinical_NotesAmendment.params.PanelID + " #btnAccepted").hide();
            $("#" + Clinical_NotesAmendment.params.PanelID + " #btnDenied").hide();
            $("#" + Clinical_NotesAmendment.params.PanelID + " #DivAmendforbilling").hide();
        }
        if (Clinical_NotesAmendment.params.Amendment == true) {
            Clinical_NotesAmendment.GetAmendmentDataForReport();
        } else {
            Clinical_NotesAmendment.GetAmendmentData();
            Clinical_NotesAmendment.GetNotesAmendmentHistory();
        }
        
        //Clinical_NotesAmendment.Action = "";
    },

    GetAmendmentData: function () {
        Clinical_NotesAmendment.GetAmendmentData_DBCALL().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.AmendmentNoteCount > 0) {

                    $("#" + Clinical_NotesAmendment.params.PanelID + " #ProblemOld").html("");
                    $("#" + Clinical_NotesAmendment.params.PanelID + " #CurrentSoap").html("");
                    $("#" + Clinical_NotesAmendment.params.PanelID + " #OldSoap").html("");
                    $("#" + Clinical_NotesAmendment.params.PanelID + " #ProblemCurrent").html("");
                    //for temp use
                    var NoAmendments = true;
                    if (response.AmendmentNote_JSON[0].CurrentProblemSoapText != response.AmendmentNote_JSON[0].OldProblemSoapText) {
                        if ($("#" + Clinical_NotesAmendment.params.PanelID + " #modelId").hasClass("modal-dialog-md")) {
                            $("#" + Clinical_NotesAmendment.params.PanelID + " #modelId").removeClass("modal-dialog-md");
                        }
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #modelId").addClass("modal-dialog-full");

                        $("#" + Clinical_NotesAmendment.params.PanelID + " #AmendmentData").removeClass("hidden");
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #Problems").removeClass("hidden");
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #Problemsheader").removeClass("hidden");
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #ProblemsDataRow").removeClass("hidden");
                        if (response.AmendmentNote_JSON[0].OldProblemSoapText != "") {
                            $("#" + Clinical_NotesAmendment.params.PanelID + " #ProblemOld").append($.parseHTML(response.AmendmentNote_JSON[0].OldProblemSoapText));
                            $("#" + Clinical_NotesAmendment.params.PanelID + " #ProblemOld header").remove();
                        }
                        if (response.AmendmentNote_JSON[0].CurrentProblemSoapText != "") {
                            $("#" + Clinical_NotesAmendment.params.PanelID + " #ProblemCurrent").append($.parseHTML(response.AmendmentNote_JSON[0].CurrentProblemSoapText));
                            $("#" + Clinical_NotesAmendment.params.PanelID + " #ProblemCurrent header").remove();
                        }
                        NoAmendments = false;
                    }
                    if (response.AmendmentNote_JSON[0].CurrentProcedureSoapText != response.AmendmentNote_JSON[0].OldProcedureSoapText) {
                        if ($("#" + Clinical_NotesAmendment.params.PanelID + " #modelId").hasClass("modal-dialog-md")) {
                            $("#" + Clinical_NotesAmendment.params.PanelID + " #modelId").removeClass("modal-dialog-md");
                        }
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #modelId").addClass("modal-dialog-full");

                        $("#" + Clinical_NotesAmendment.params.PanelID + " #AmendmentData").removeClass("hidden");
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #Procedures").removeClass("hidden");
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #Proceduresheader").removeClass("hidden");
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #ProceduresDataRow").removeClass("hidden");

                        if (response.AmendmentNote_JSON[0].OldProcedureSoapText != "") {
                            $("#" + Clinical_NotesAmendment.params.PanelID + " #ProcedureOld").append($.parseHTML(response.AmendmentNote_JSON[0].OldProcedureSoapText));
                            $("#" + Clinical_NotesAmendment.params.PanelID + " #ProcedureOld header").remove();
                        }
                        if (response.AmendmentNote_JSON[0].CurrentProcedureSoapText != "") {
                            $("#" + Clinical_NotesAmendment.params.PanelID + " #ProcedureCurrent").append($.parseHTML(response.AmendmentNote_JSON[0].CurrentProcedureSoapText));
                            $("#" + Clinical_NotesAmendment.params.PanelID + " #ProcedureCurrent header").remove();
                        }
                        NoAmendments = false;
                    }
                    if (NoAmendments) {
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #NoAmendmentsMsg").removeClass("hidden");
                    }
                }
                else {
                    $("#" + Clinical_NotesAmendment.params.PanelID + " #AmendmentData").hide();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    GetAmendmentData_DBCALL: function () {
        var objData = {};
        objData["NotesId"] = Clinical_NotesAmendment.params.NotesId;
        objData["commandType"] = "GET_AMENDMENT_DATA";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    GetNotesAmendmentHistory: function () {
        $("#" + Clinical_NotesAmendment.params.PanelID + " #AmendmentHistory").removeClass("hidden")
        $("#" + Clinical_NotesAmendment.params.PanelID + " #AmendmentHistory").empty();
        Clinical_NotesAmendment.GetAmendmentDataForReport_DBCALL().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Json = response.AmendmentNote_JSON;
                if (Json) {
                    Json = Json.reverse();
                    $.each(Json, function (i, item) {
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #AmendmentHistory").append(item.SOAPText + "</br>");
                    });
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    GetAmendmentDataForReport: function () {
        Clinical_NotesAmendment.GetAmendmentDataForReport_DBCALL().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var NoAmendments = true;
                if (response.AmendmentNoteCount > 0) {
                    $("#" + Clinical_NotesAmendment.params.PanelID + " #AmendmentReportData").removeClass("hidden");
                    $("#" + Clinical_NotesAmendment.params.PanelID + " #ProblemOld").html("");
                    $("#" + Clinical_NotesAmendment.params.PanelID + " #CurrentSoap").html("");
                    $("#" + Clinical_NotesAmendment.params.PanelID + " #OldSoap").html("");
                    $("#" + Clinical_NotesAmendment.params.PanelID + " #ProblemCurrent").html("");
                    //for temp use
                    var NoAmendments = false;
                    $.each(response.AmendmentNote_JSON, function (i, item) {
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #AmendmentReportData").last().append(item.SOAPText)
                    });
                }
                if (NoAmendments) {
                    $("#" + Clinical_NotesAmendment.params.PanelID + " #NoAmendmentsReportMsg").removeClass("hidden");
                }
                else {
                    $("#" + Clinical_NotesAmendment.params.PanelID + " #AmendmentData").hide();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    GetAmendmentDataForReport_DBCALL: function () {
        var objData = {};
        objData["NotesId"] = Clinical_NotesAmendment.params.NotesId;
        objData["commandType"] = "GET_AMENDMENT_DATA_FOR_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    ValidateClinical_NotesAmendment: function () {
        $('#frmClinical_NotesAmendment')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  /*TemplateLetterId: {
                      group: '.col-sm-10',
                      validators: {
                          notEmpty: {
                              message: 'Select a template to create the letter. '
                          },
                      }
                  },*/
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Clinical_NotesAmendment.CreateLetter();
       });
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        if (Clinical_NotesAmendment.params["FromAdmin"] == "0") {
            if (Clinical_NotesAmendment.params != null && Clinical_NotesAmendment.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_NotesAmendment.params.ParentCtrl, 'Clinical_NotesAmendment');
            }
            else
                UnloadActionPan(null, 'Clinical_NotesAmendment');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
    /*CoSignNotes: function () {
        var self = $(" #frmClinical_NotesAmendment");
        var myJSON = self.getMyJSON();
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Do you want to CoSign this record?', function () {
                    DashBoard.NotesUpdateCosign(Clinical_NotesView.params.NotesId, myJSON);
                }, function () { },
                            'Confirm CoSign'
                        );
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },*/
    AmendmentNotes: function (cntrl) {
        var action = $(cntrl).val();
        var self = $("#frmClinical_NotesAmendment");
        var myJSON = self.getMyJSON();
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (!$("#" + Clinical_NotesAmendment.params.PanelID + " #chkAmendmentForBilling").is(":checked")) {
                    utility.myConfirm('Do you want to Alert billing team to review this amendment?', function () {
                        $("#" + Clinical_NotesAmendment.params.PanelID + " #chkAmendmentForBilling").prop("checked", true);
                        Clinical_NotesAmendment.AmmendmentNoteFun(action, myJSON, '1');
                    }, function () {
                        Clinical_NotesAmendment.AmmendmentNoteFun(action, myJSON, '0');
                    },
                               'Confirm Amendment For Billing'
                           );
                }
                else {
                    Clinical_NotesAmendment.AmmendmentNoteFun(action, myJSON, '1');
                }

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },
    AmmendmentNoteFun: function (action, myJSON, IsAmendmentForBilling) {
        utility.myConfirm('Do you want Amendment of this record?', function () {
            if (Clinical_NotesAmendment.params.NotesId == null) {
                Clinical_NotesAmendment.params.NotesId = Clinical_Notes.params.NotesId;
            }
            //Start || 15 August, 2016 || ZeeshanAK || Fix for EMR-15
            DashBoard.NotesUpdateAmendment(Clinical_NotesAmendment.params.NotesId, action, myJSON, Clinical_NotesAmendment.params.ParentCtrl, IsAmendmentForBilling);
            //End   || 15 August, 2016 || ZeeshanAK || Fix for EMR-15
        }, function () { },
                               'Confirm Amendment'
                           );
    },
    /*NotesUpdateForCosignORAmendment: function (NotesID, jsondata) {
        var objDdata = {};
        objDdata["NotesID"] = NotesID;
        return MDVisionService.defaultService(data, "DASHBOARD", "UPDATE_NOTES_COSIGN");
    },
    NotesUpdateCosign: function (NotesId, jsonData) {
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Do you want to CoSign this record?', function () {
    
                    DashBoard.NotesUpdateForCosignORAmendment(NotesId, jsonData).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages("Successfully Signed!", 1);
                            DashBoard.DashBoardEncounterSearch();
                            // Clinical_Notes.UnLoad();
                            //UnloadActionPan(Clinical_Notes.params["ParentCtrl"]);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
    
                }, function () { },
                        'Confirm Sign'
                    );
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },*/


}