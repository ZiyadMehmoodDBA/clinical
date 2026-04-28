/**
Author: Muhammad Irfan
Created Date: 04/01/2016
Overview: This file is created for pdf view of face sheet
**/

Clinical_FaceSheetView = {
    bIsFirstLoad: true,
    params: [],
    pdf: "",
    Load: function (params) {
        Clinical_FaceSheetView.params = params;
        if (Clinical_FaceSheetView.params != null && Clinical_FaceSheetView.params.PanelID != "Clinical_FaceSheetView") {
            Clinical_FaceSheetView.params["PanelID"] = Clinical_FaceSheetView.params["PanelID"] + ' #Clinical_FaceSheetView';
        }
        else {
            Clinical_FaceSheetView.params = [];
            Clinical_FaceSheetView.params["PanelID"] = "Clinical_FaceSheetView"
        }

        if (Clinical_FaceSheetView.bIsFirstLoad) {
            Clinical_FaceSheetView.bIsFirstLoad = false;
            var self = $('#' + Clinical_FaceSheetView.params["PanelID"]);
            self.loadDropDowns(true).done(function () {

                Clinical_FaceSheetView.faceSheetPreview(Clinical_FaceSheetView.params.PatientId, Clinical_FaceSheetView.params.UserId);
            });
        }


    },
    /**
    Author: Muhammad Irfan
    Created Date: 04/01/2016
    Overview: This function is wraper to call face sheet service call
    **/
    faceSheetPreview: function (patientID, userID) {
        Clinical_FaceSheetView.previewFaceSheet(patientID, userID).done(function (response) {
            response = JSON.parse(response);
            Clinical_FaceSheetView.pdf = response.FaceSheetHTML;
            utility.PDFViewer(response.FaceSheetHTML, false, 'Clinical_FaceSheetView #PreviewFaceSheetForm', true);

        });
    },
    /**
    Author: Muhammad Irfan
    Created Date: 04/01/2016
    Overview: This function will print the face sheet
    **/
    PrintReports: function () {
        AppPrivileges.GetFormPrivileges("FaceSheet_Face Sheet", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_FaceSheetView.OpenFaceSheetComponentSelection();

                //$("#" + Clinical_FaceSheetView.params["PanelID"] + " #PreviewFaceSheetForm")[0].contentWindow.focus();
                //$("#" + Clinical_FaceSheetView.params["PanelID"] + " #PreviewFaceSheetForm")[0].contentWindow.print();
               
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    OpenFaceSheetComponentSelection: function () {
        var params = [];        
        params["PatientId"] = Clinical_FaceSheetView.params.PatientId;
        params["RefForm"] = "frmFaceSheetView";
        params["CheckBoxes"] = Clinical_FaceSheet.Components;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_FaceSheetView";        
        LoadActionPan('Clinical_FaceSheetComponentSelection', params);
    },

    /**
    Author: Muhammad Irfan
    Created Date: 04/01/2016
    Overview: This function calls api service to bring data
    **/
    previewFaceSheet: function (patientID, userID) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["commandType"] = "PREVIEW_FACESHEET";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "FaceSheet", "FaceSheet");
    },

    /**
    Author: Muhammad Irfan
    Created Date: 04/01/2016
    Overview: This function unload the pdf view of face sheet
    **/
    UnLoad: function () {

        if (Clinical_FaceSheetView.params != null && Clinical_FaceSheetView.params.ParentCtrl) {
            if (Clinical_FaceSheetView.params.ParentCtrl == "Clinical_FaceSheet") {
                var parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                UnloadActionPan(Clinical_FaceSheetView.params.ParentCtrl, "Clinical_FaceSheetView", null, parentPanelId);
            } else {
                UnloadActionPan(Clinical_FaceSheetView.params.ParentCtrl);
            }
        }
        else {
            UnloadActionPan();
        }
    }
    ,
    sendAsFax: function () {
        var params = [];
        params["PDFBase64"] = Clinical_FaceSheetView.pdf;
        params["ParentCtrl"] = 'Clinical_FaceSheetView';
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        LoadActionPan("Batch_FaxSend", params);
    }
}
