iTrack_Cost = {
    params: [],
    bIsFirstLoad: true,
    listSearchLength: 0,
    measureTable: null,
    graphObj: [],
    Load: function (params) {
        iTrack_Cost.params = params;
        iTrack_Cost.measureTable = null;
        if (iTrack_Cost.params.PanelID != 'pnliTrackCost') {
            iTrack_Cost.params.PanelID = iTrack_Cost.params.PanelID + ' #pnliTrackCost';
        } else {
            iTrack_Cost.params.PanelID = 'pnliTrackCost';
        }
        var self = $("#pnliTrackCost");
        self.loadDropDowns(true).done(function () {

            //utility.CreateDatePicker(iTrack_Cost.params.PanelID + " #dtpFromDate, #dtpToDate", function () { }, false);
            //utility.ValidateFromToDate('frmiTrackQuality', 'dtpFromDate', 'dtpToDate', true);
            //iTrack_Cost.BindProvider();
            //iTrack_Cost.BindGroupProviders();
            //$('#pnliTrackCost #dtpFromDate').datepicker("setDate", '01/01/2018');
            //$('#pnliTrackCost #dtpToDate').datepicker("setDate", '12/31/2018');
            //$('#pnliTrackCost #dtpGroupFromDate').datepicker("setDate", '01/01/2018');
            //$('#pnliTrackCost #dtpGroupToDate').datepicker("setDate", '12/31/2018');
            //$('#pnliTrackCost #ddlMemberProviders').multiselect({
            //    includeSelectAllOption: true,
            //    enableFiltering: true,
            //    enableCaseInsensitiveFiltering: true,
            //    buttonTitle: function (options, select) {
            //        var buttonTitle = "";
            //        $.each(options, function (i, item) {
            //            if (buttonTitle != "") {
            //                buttonTitle += "," + $(item).attr("refvalue");
            //            }
            //            else {
            //                buttonTitle += $(item).attr("refvalue");
            //            }
            //            $(item).prop("disabled", true);
            //        });

            //        return buttonTitle;
            //    }
            //});

        });

    },
   
}