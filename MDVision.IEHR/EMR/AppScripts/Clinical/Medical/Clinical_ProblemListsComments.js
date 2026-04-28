Clinical_ProblemListsComments = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_ProblemListsComments.params = params;
        //serialize Data.
        //$('#frmProblemListsComments').data('serialize', $('#frmProblemListsComments').serialize());

        var problemID = Clinical_ProblemListsComments.params.ProblemListId;
        var patientID = Clinical_ProblemListsComments.params.PatientId;

        Clinical_ProblemListsComments.ProblemListsSearch(problemID, patientID);
    },

    // Fill comments function

    ProblemListsSearch: function (ProblemListId, PatientId) {
        var dgvId = "dgvProblemLists";
        var pnlId = "pnlProblemLists_Result";
        var mainPnlId = Clinical_ProblemLists.params.PanelID;
        if (Clinical_ProblemListsComments.params.ParentCtrl == "Clinical_Treatment") {
            dgvId = "dgvProblemListsT";
            pnlId = "pnlProblemLists_ResultT";
            mainPnlId = Clinical_Treatment.params.PanelID;
        }
            //AST-14 BY:MAhmad
        else if (Clinical_ProblemListsComments.params.ParentCtrl == "Clinical_OrderSetDetails") {
            dgvId = "dgvProblemListsOS";
            pnlId = "pnlProblemLists_ResultOS";
            mainPnlId = Clinical_OrderSetDetails.params.PanelID;

        }
        else if (Clinical_ProblemListsComments.params.ParentCtrl == "OrderSet_Problems") {
            dgvId = "dgvOrderSetProblemLists";
            pnlId = "pnlOrderSetProblemLists_Result";
            mainPnlId = OrderSet_Problems.params.PanelID;

        }
        //AST-14 BY:MAhmad
        if ($('#' + mainPnlId + ' #' + pnlId + ' #' + dgvId + ' #hfComments' + ProblemListId + ' input').val() == "" && Clinical_ProblemListsComments.params.ParentCtrl != "Clinical_OrderSetDetails"
            && Clinical_ProblemListsComments.params.ParentCtrl != "OrderSet_Problems") {
            Clinical_ProblemListsComments.SearchProblemList(ProblemListId, PatientId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var problemJSON = JSON.parse(response.ProblemListLoad_JSON);

                    var comments = "";

                    $.each(problemJSON, function (key, value) {
                        if (ProblemListId == value.ProblemListId)
                            comments = value.Comments;
                    });


                    $('#Clinical_ProblemListsComments #txtComments').val(comments);

                    var comments = $('#Clinical_ProblemListsComments #txtComments').val();
                    //$('#pnlClinicalProblemLists #hfGridComments').val(comments);
                    $('#' + mainPnlId + ' #' + pnlId + ' #' + dgvId + ' #hfComments' + ProblemListId + ' input').val(comments)
                    $('#frmProblemListsComments').data('serialize', $('#frmProblemListsComments').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            $('#Clinical_ProblemListsComments #txtComments').val($('#' + mainPnlId + ' #' + pnlId + ' #' + dgvId + ' #hfComments' + ProblemListId + ' input').val());
        }
    },

    SearchProblemList: function (ProblemListId, PatientId) {

        var objData = new Object();
        objData["ProblemListId"] = ProblemListId;
        objData["PatientId"] = PatientId;
        objData["IsActive"] = "1";
        objData["commandType"] = "SEARCH_PROBLEMLIST";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },
    // End Fill comments function


    SaveComments: function () {
        var comments = $('#Clinical_ProblemListsComments #txtComments').val();
        //$('#pnlClinicalProblemLists #hfGridComments').val(comments);
        if (Clinical_ProblemListsComments.params.ParentCtrl == "Clinical_OrderSetDetails") {
            $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProblemLists_ResultOS #dgvProblemListsOS #hfComments' + Clinical_ProblemListsComments.params.ProblemListId + ' input').val(comments);
        }
        else if (Clinical_ProblemListsComments.params.ParentCtrl == "Clinical_Treatment") {
            $('#' + Clinical_Treatment.params.PanelID + ' #pnlProblemLists_ResultT #dgvProblemListsT #hfComments' + Clinical_ProblemListsComments.params.ProblemListId + ' input').val(comments);
        } else if (Clinical_ProblemListsComments.params.ParentCtrl == "OrderSet_Problems") {
            $('#' + OrderSet_Problems.params.PanelID + ' #pnlOrderSetProblemLists_Result #dgvOrderSetProblemLists #hfComments' + Clinical_ProblemListsComments.params.ProblemListId + ' input').val(comments);
        } else
            if (Clinical_ProblemListsComments.params.ParentCtrl == "CCM_Patient_Hub") {
                $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlProblemLists_Result #dgvProblemLists #hfComments' + Clinical_ProblemListsComments.params.ProblemListId + ' input').val(comments);
            } else {
                $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #dgvProblemLists #hfComments' + Clinical_ProblemListsComments.params.ProblemListId + ' input').val(comments);
            }
        $('#frmProblemListsComments').data('serialize', $('#frmProblemListsComments').serialize());
        if (EMREditableGrid && Clinical_ProblemListsComments.params.ProblemListId) {
            if (Clinical_ProblemListsComments.params.ParentCtrl == "Clinical_Treatment") {
                Treatment_ProblemListGrid.rowSave($('#' + Clinical_Treatment.params.PanelID + ' #pnlProblemLists_ResultT #dgvProblemListsT tr#' + Clinical_ProblemListsComments.params.ProblemListId), EMREditableGrid);
            }
                //AST-14 BY:MAhmad
            else if (Clinical_ProblemListsComments.params.ParentCtrl == "Clinical_OrderSetDetails") {
                OrderSet_ProblemListGrid.rowSave($('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProblemLists_ResultOS #dgvProblemListsOS tr#' + Clinical_ProblemListsComments.params.ProblemListId), EMREditableGrid);
            }
                //AST-14 BY:MAhmad
                //IMP-2074
            else if (Clinical_ProblemListsComments.params.ParentCtrl == "clinicalTabProblemLists" || Clinical_ProblemListsComments.params.ParentCtrl == "Clinical_ProblemLists") {
                Clinical_ProblemListsComments.SaveProblemListComments();
            }
            else {
                Clinical_ProblemLists.rowSave($('#' + Clinical_ProblemListsComments.params.PanelID + ' #pnlProblemLists_Result #dgvProblemLists tr#' + Clinical_ProblemListsComments.params.ProblemListId), EMREditableGrid);
            }
        }
        Clinical_ProblemListsComments.UnLoad();
    },

    SaveProblemListComments: function () {
        Clinical_ProblemListsComments.CommentsSave().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var editingId = Clinical_ProblemListsComments.params.ProblemListId;
                var commentsIcon = $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tr#" + editingId + " a").get(8);
                var comments = $('#Clinical_ProblemListsComments #txtComments').val();
                $(commentsIcon).attr('data-original-title', comments);
                var deleterow = $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tr#" + editingId + " a").get(3);
                utility.DisplayMessages(response.message, 1);
                var isactive = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
                if (!$(deleterow).hasClass('hidden')) {
                    Clinical_ProblemLists.ProblemListsSearch();
                } else {
                    if (!$("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tr#" + editingId).hasClass('active')) {
                        Clinical_ProblemLists.ProblemListsSearch();
                    }
                }

                Clinical_ProblemListsComments.UnLoad();
            }
            else {
                utility.DisplayMessages(response.message, 3);
            }
        });
    },

    CommentsSave: function () {
        var ProblemListId = Clinical_ProblemListsComments.params.ProblemListId;
        var patientId = Clinical_ProblemListsComments.params.PatientId;
        var comments = $('#Clinical_ProblemListsComments #txtComments').val();

        var objData = new Object();
        objData["ProblemListId"] = ProblemListId;
        objData["PatientId"] = patientId;
        objData["Comments"] = comments;
        var IsThisActive = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive') == "1" ? "true" : "false";
        objData["IsActive"] = IsThisActive;
        objData["commandType"] = "UPDATE_PROBLEMLISTCOMMENTS";

        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },

    UnLoad: function () {

        //utility.UnLoadDialog('frmProblemListsComments', function () {
        //    UnloadActionPan(Clinical_ProblemListsComments.params["ParentCtrl"], "actionPanProblemListsComments");
        //}, function () {
        //    UnloadActionPan(Clinical_ProblemListsComments.params["ParentCtrl"], "actionPanProblemListsComments");
        //});

        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-144 */
        if (Clinical_ProblemListsComments.params.ParentCtrl == 'clinicalTabProblemLists') {
            UnloadActionPan(Clinical_ProblemListsComments.params["ParentCtrl"], "Clinical_ProblemListsComments");
        } else {
            UnloadActionPan(Clinical_ProblemListsComments.params.ParentCtrl, 'Clinical_ProblemListsComments', null, Clinical_ProblemListsComments.params.PanelID);
        }
        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-144 */
    },
}