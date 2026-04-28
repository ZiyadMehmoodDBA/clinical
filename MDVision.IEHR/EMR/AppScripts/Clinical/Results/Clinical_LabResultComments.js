Clinical_LabResultComments = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_LabResultComments.params = params;
        var LabResultId = Clinical_LabResultComments.params.LabResultId;
        Clinical_LabResultComments.PopulateResultComment(LabResultId);
    },
    PopulateResultComment: function (LabResultId) {
        var dgvId = "dgvLabResult";
        var pnlId = "pnlLabResult_Result";
        var mainPnlId = "#" + Clinical_LabResultComments.params.PanelID;
        if (Clinical_LabResultComments.params.ParentCtrl == 'mstrTabDashBoard') {
            dgvId = "dgvLabOrderResult";
            pnlId = "pnlDashboard #LabResult";
            mainPnlId = "";
        }
        $('#' + Clinical_LabResultComments.params.PanelID + ' #txtComments').val($(mainPnlId + ' #' + pnlId + ' #' + dgvId + ' #hfComments' + LabResultId).val());
    },

    SaveComments: function () {
        var LabResultId = Clinical_LabResultComments.params.LabResultId;
        var comments = $('#' + Clinical_LabResultComments.params.PanelID + ' #txtComments').val();
        Clinical_LabResultComments.CommentsSave(LabResultId, comments).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var dgvId = "dgvLabResult";
                var pnlId = "pnlLabResult_Result";
                var mainPnlId = "#" + Clinical_LabResultComments.params.PanelID;
                if (Clinical_LabResultComments.params.ParentCtrl == 'mstrTabDashBoard') {
                    dgvId = "dgvLabOrderResult";
                    pnlId = "pnlDashboard #LabResult";
                    mainPnlId = "";
                }
                var commentsIcon = $(mainPnlId + ' #' + pnlId + ' #' + dgvId + ' tbody tr[LabOrderResultId="' + LabResultId + '"] a[id="comment_' + LabResultId + '"]');
                $(commentsIcon).attr('data-original-title', comments);
                $(mainPnlId + ' #' + pnlId + ' #' + dgvId + ' tbody tr[LabOrderResultId="' + LabResultId + '"] #hfComments' + LabResultId).find('#hfComments' + LabResultId).val(comments);
                if (Clinical_LabResultComments.params.ParentCtrl == 'mstrTabDashBoard') {
                    var PageNo = $("#pnlDashboard #dgvLabResultDashboard_Paging ul li.active a").html();
                    DashBoard.DashBoardLabResultGridLoad(null, null, PageNo);
                }
                else {
                    var PageNo = $(mainPnlId + ' #' + pnlId + ' #dgvLabResult_Paging ul li.active a').html();
                    Clinical_LabOrder.LabResultsSearch(0, PageNo, null);
                }
                utility.DisplayMessages(response.Message, 1);
                Clinical_LabResultComments.UnLoad();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    CommentsSave: function (LabResultId, Comment) {
        var objData = new Object();
        objData["LabResultId"] = LabResultId;
        objData["Comments"] = Comment;
        objData["commandType"] = "update_labresultcomments";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    UnLoad: function () {
        if (Clinical_LabResultComments.params.ParentCtrl == 'clinicalTabProcedures')
            UnloadActionPan(Clinical_LabResultComments.params["ParentCtrl"], "Clinical_LabResultComments");
        else
            UnloadActionPan(Clinical_LabResultComments.params.ParentCtrl, 'Clinical_LabResultComments', null, Clinical_LabResultComments.params.PanelID);
    },

}