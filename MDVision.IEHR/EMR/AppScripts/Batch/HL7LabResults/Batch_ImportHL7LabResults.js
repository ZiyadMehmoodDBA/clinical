Batch_ImportHL7LabResults = { params: [],
    bIsFirstLoad: true,
    listSearchLength: 0,

    Load: function (params) {
        Batch_ImportHL7LabResults.params = params;
        if (Batch_ImportHL7LabResults.params.PanelID != 'pnlBatchImportHL7LabResults') {
            Batch_ImportHL7LabResults.params.PanelID = Batch_ImportHL7LabResults.params.PanelID + ' #pnlBatchImportHL7LabResults';
        } else {
            Batch_ImportHL7LabResults.params.PanelID = 'pnlBatchImportHL7LabResults';
        }
    },
    UnLoad: function () {
        if (Batch_ImportHL7LabResults.params != null && Batch_ImportHL7LabResults.params.ParentCtrl != null) {
            UnloadActionPan(Batch_ImportHL7LabResults.params.ParentCtrl, 'Batch_ImportHL7LabResults');
        }
        else {
            RemoveAdminTab('batchTabImportHL7LabResults');
        }
    },
    /*****************************************
   ************ Added by Azhar to Add HL7 Messages
   ************************************************/
    LabResultImportHL7: function () {
        Batch_ImportHL7LabResults.LabResultImportHL7_DbCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages("Data imported successfully.", 1);
                $(" #mainForm #hfTriggerLocation").val('LabResult');
                ClinicalCDSDetail.showCDSAlert('', 0);
                //  utility.DisplayMessages(response.Message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LabResultUploadHL7: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'batchTabImportHL7LabResults';
        LoadActionPan('Clinical_LabResultHL7_Import', params);
    },
    LabResultImportHL7_DbCall: function () {
        var objData = {};

        objData["commandType"] = "LabResult_ImportHL7";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
}