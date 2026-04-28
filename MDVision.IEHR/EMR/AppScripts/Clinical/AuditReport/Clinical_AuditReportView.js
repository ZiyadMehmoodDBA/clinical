/**
Author: Ahmad Raza
Created Date: 15/04/2016
Overview: This file is created for pdf view of Audit Report
**/

Clinical_AuditReportView = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Clinical_AuditReportView.params = params;
        if (Clinical_AuditReportView.params != null && Clinical_AuditReportView.params.PanelID != "Clinical_AuditReportView") {
            Clinical_AuditReportView.params["PanelID"] = Clinical_AuditReportView.params["PanelID"] + ' #Clinical_AuditReportView';
        }
        else {
            Clinical_AuditReportView.params = [];
            Clinical_AuditReportView.params["PanelID"] = "Clinical_AuditReportView"
        }

        if (Clinical_AuditReportView.bIsFirstLoad) {
            Clinical_AuditReportView.bIsFirstLoad = false;
            var self = $('#' + Clinical_AuditReportView.params["PanelID"]);
            Clinical_AuditReportView.exportAuditReport();
        }


    },
    /**
    Author: Ahmad Raza
    Created Date: 15-04-2016
    Overview: function to export Audit Report
    **/
    exportAuditReport: function () {
        Clinical_AuditReportView.previewAuditReport().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                download("data:application/octet-stream;base64," + response.AuditReportHTML, "AuditReport.pdf", "application/octet-stream");
                utility.PDFViewer(response.AuditReportHTML, false, 'Clinical_AuditReportView #PreviewAuditReportForm', true);
            }
        });


    },
    // Author: Ahmad Raza
    // Created Date: 15-04-2016
    // Overview: function to export Audit Report
    previewAuditReport: function () {

        var self = $("#" + Clinical_AuditReport.params.PanelID + " #pnlAuditReport_Search");
        var myJSON = self.getMyJSONByName();
        var objData = JSON.parse(myJSON);

        objData["CreatedDateFrom"] = $("#" + Clinical_AuditReport.params.PanelID + " #dtpFromDate").val();
        objData["CreatedDateTo"] = $("#" + Clinical_AuditReport.params.PanelID + " #dtpToDate").val();

        var selectedOption = $("#" + Clinical_AuditReport.params.PanelID + " #ddlAuditUser option:selected");
        if ($(selectedOption).val() == "") {
            objData["AuditUserName"] = "";
        }
        else {
            objData["AuditUserName"] = $(selectedOption).text();
        }

        objData["commandType"] = "PREVIEW_AUDITREPORT";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "AuditReport", "AuditReport");
    },

    // Author: Ahmad Raza
    // Created Date: 15-04-2016
    // Overview: function for preview Audit Report
    UnLoad: function () {

        if (Clinical_AuditReportView.params != null && Clinical_AuditReportView.params.ParentCtrl) {
            UnloadActionPan(Clinical_AuditReportView.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    }
}
