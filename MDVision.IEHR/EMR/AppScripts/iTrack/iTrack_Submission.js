iTrack_Submission = {
    params: [],
    bIsFirstLoad: true,
    listSearchLength: 0,
    measureTable: null,
    graphObj: [],
    Load: function (params) {
        iTrack_Submission.params = params;
        iTrack_Submission.measureTable = null;
        if (iTrack_Submission.params.PanelID != 'pnliTrackSubmission') {
            iTrack_Submission.params.PanelID = iTrack_Submission.params.PanelID + ' #pnliTrackSubmission';
        } else {
            iTrack_Submission.params.PanelID = 'pnliTrackSubmission';
        }
        var self = $("#pnliTrackSubmission");
        self.loadDropDowns(true).done(function () {

        });

    },

}