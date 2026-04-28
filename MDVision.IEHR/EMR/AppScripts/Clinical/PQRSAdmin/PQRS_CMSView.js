//Author: Azhar Shahzad
//Date: 7/25/2016
//This file will handle all actions performed to view PDF
PQRS_CMSView = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        PQRS_CMSView.params = params;
        if (PQRS_CMSView.params != null && PQRS_CMSView.params.PanelID != "PQRS_CMSView") {
            PQRS_CMSView.params["PanelID"] = PQRS_CMSView.params["PanelID"] + ' #PQRS_CMSView';
        }
        else {
            PQRS_CMSView.params = [];
            PQRS_CMSView.params["PanelID"] = "PQRS_CMSView"
        }

        if (PQRS_CMSView.bIsFirstLoad) {
            PQRS_CMSView.bIsFirstLoad = false;
            if (PQRS_CMSView.params["viewMode"] == "Measures") {
                PQRS_CMSView.PQRS_CMSView(PQRS_CMSView.params.MeasureId);
            } else if (PQRS_CMSView.params["viewMode"] == "MeasuresByMeasureNumber") {
                PQRS_CMSView.PQRS_CMSViewByMeasureNumber(PQRS_CMSView.params.MeasureNumber);
            }
            
        }
    },
    //Function Name: PQRS_CMSView
    //Author Name: Azhar Shahzad
    //Created Date: 7/25/2016
    //Description: Creates PDF to view CMS PQRS
    PQRS_CMSView: function (MeasureId) {
        PQRS_CMSView.previewMeasures(MeasureId).done(function (response) {
            response = JSON.parse(response);
            utility.PDFViewer(response.CMS_ViewHTML, false, 'PQRS_CMSView #PreviewPQRS_CMSForm', true);

        });
    },

    PQRS_CMSViewByMeasureNumber: function (MeasureId) {
        PQRS_CMSView.previewMeasuresByMeasureNumber(MeasureId).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                utility.PDFViewer(response.CMS_ViewHTML, false, 'PQRS_CMSView #PreviewPQRS_CMSForm', true);
            }
            else {
                utility.DisplayMessages('No Document Found', 2);
            }

        });
    },

    //Function Name: printLabResult
    //Author Name: Azhar Shahzad
    //Created Date: 7/25/2016
    //Description: Prints PDF
    printPQRS_CMS: function () {
     //   AppPrivileges.GetFormPrivileges(" Face Sheet", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
       //     if (strMessage == "") {
                $("#" + PQRS_CMSView.params["PanelID"] + " #PreviewPQRS_CMSForm")[0].contentWindow.focus();
                $("#" + PQRS_CMSView.params["PanelID"] + " #PreviewPQRS_CMSForm")[0].contentWindow.print();

       //     }
         //   else {
         ///       utility.DisplayMessages(strMessage, 2);
        //    }
       // });
    },

    //Function Name: previewLabResult
    //Author Name: Azhar Shahzad
    //Created Date: 7/25/2016
    //Description: DB call to view PDF
    previewMeasures: function (MeasureId) {

        var objData = {};
        objData["MeasureId"] = MeasureId;
        objData["commandType"] = "preview_cmsdocument";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_Measure");
    },

    previewMeasuresByMeasureNumber: function (MeasureNumber) {

        var objData = {};
        objData["MeasureNumber"] = MeasureNumber;
        objData["commandType"] = "preview_cmsdocumentByMeasureNumber";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_Measure");
    },


    UnLoad: function () {

        if (PQRS_CMSView.params != null && PQRS_CMSView.params.ParentCtrl) {
            UnloadActionPan(PQRS_CMSView.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    },
}
