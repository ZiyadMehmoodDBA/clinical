Clinical_LabResultTrendsNotes = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_LabResultTrendsNotes.params = params;

        if (Clinical_LabResultTrendsNotes.params != null && Clinical_LabResultTrendsNotes.params.PanelID != "pnlClinical_LabResultTrendsNotes") {
            Clinical_LabResultTrendsNotes.params.PanelID = Clinical_LabResultTrendsNotes.params.PanelID + ' #pnlClinical_LabResultTrendsNotes';
        }
        else {
            Clinical_LabResultTrendsNotes.params.PanelID = "pnlClinical_LabResultTrendsNotes";
        }
        
        if (Clinical_LabResultTrendsNotes.bIsFirstLoad) {
            Clinical_LabResultTrendsNotes.bIsFirstLoad = false;
            Clinical_LabResultTrendsNotes.CommentsPreview();
        }
    },
    CommentsPreview: function () {
        $('#' + Clinical_LabResultTrendsNotes.params.PanelID + ' #modaldialog').removeClass("modal-dialog-lg");
        $('#' + Clinical_LabResultTrendsNotes.params.PanelID + ' #DateContent').html('<b>' + Clinical_LabResultTrendsNotes.params.Date + '</b>');
        $('#' + Clinical_LabResultTrendsNotes.params.PanelID + ' #ObsContent').text(Clinical_LabResultTrendsNotes.params.LOINCDescription);
        $('#' + Clinical_LabResultTrendsNotes.params.PanelID + ' #CommentContent').html(Clinical_LabResultTrendsNotes.params.Comments);
    },

    UnLoad: function () {
        UnloadActionPan(Clinical_LabResultTrendsNotes.params.ParentCtrl, "Clinical_LabResultTrendsNotes");
    },
}