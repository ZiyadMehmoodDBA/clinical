ReportsViewer_Detail = {
    params: [],
    Load: function (params) {

        ReportsViewer_Detail.params = params;
        ReportsViewer_Detail.LoadReportDetail();
        if (ReportsViewer_Detail.params.FormName == "Financial Analytics Reports/MonthlyScoreboardDetail" || ReportsViewer_Detail.params.FormName == "Financial Analytics Reports/MonthlyScoreboardDetailPayments") {
            $('#ReportsSSRSPrintViewDetail #btn-print').hide();
        } else {
            $('#ReportsSSRSPrintViewDetail #btn-print').show();
        }
    },

    LoadReportDetail: function () {

        var self = $('#ReportsSSRSPrintViewDetail #frmReportsSSRSPrintViewDetail');

        //AppPrivileges.GetFormPrivileges(ReportsViewer_Detail.params["FormName"], "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
                ReportsSSRSDashboard.SetQueryString(ReportsViewer_Detail.params["QueryString"], ReportsViewer_Detail.params["FormName"]).done(function (response) {
                    if (response.status != false) {

                        self.find("#ReportDetailDiv").css("display", "block");
                        self.find('#PreviewReportDetail').attr('src', ReportsViewer_Detail.params["reportpath"]);

                        //var iframe = document.getElementById('PreviewReportDetail');
                        //iframe.addEventListener('load', function () {
                        //    $($(document.getElementById('PreviewReportDetail').contentWindow.document).find('body')).on('click', function (ev) {
                        //        $('html').trigger('click');
                        //    });
                        //});
                    }
                });
        //    }
        //    else {
        //        self.find('#treeBasic').find('a').attr('disabled', false).css("cursor", "");;
        //        utility.DisplayMessages(strMessage, 2);
        //        self.find('#PreviewReportDetail').attr('src', null);
        //    }
        //});
    },

    PrintReport: function () {
        var params = [];
     
        params["PreviewPdf"] = false;
        if (ReportsViewer_Detail.params.FormName == "Charges and Payments Reports/ClaimStatusDashboardDetail") {
            params["ReportName"] = "Claim Status Dashboard";
        } else {
            params["ReportName"] = "Aging Detail Analysis";
        }
        params["ParentCtrl"] = "ReportsViewer_Detail";
        params["PanelID"] = "ReportsSSRSPrintViewDetail";
        LoadActionPan('ReportsSSRSPrintView', params);
    },

    UnLoad: function () {

        UnloadActionPan(ReportsViewer_Detail.params.ParentCtrl, 'ReportsViewer_Detail');
    },

};
