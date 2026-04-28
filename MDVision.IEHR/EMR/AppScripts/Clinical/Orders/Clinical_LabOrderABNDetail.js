Clinical_LabOrderABNDetail = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    LabOrderProblems: [],
    FavListName: "LabOrderABNDetail",
    checkedProblems: [],
    CPTCodeQA: [],
    ArrayValidation: [],
    selectedTestCode: null,
    selectedTestDescription: null,

    // Author : Ahsan Nasir
    Tests: [],
    TestDesc: [],
    TestABNDesc: [],
    Load: function (params) {
        BackgroundLoaderShow(true);
        //ClinicalLabOrderDetail.params["TabID"] = 'ClinicalLabOrderDetail';
        Clinical_LabOrderABNDetail.params = params;
        Clinical_LabOrderABNDetail.fillLabOrder(Clinical_LabOrderABNDetail.params.LabOrderId).done(function (response) {

            // Showing checkboxes

            var obj = JSON.parse(response);
            if (obj.status != false) {
                var testsLoadJSONData = JSON.parse(obj.LabOrderTest_JSON);
                    var finalTr = '';
                    var counter = 2;
                    $.each(testsLoadJSONData, function (i, item) {
                        finalTr = finalTr + '<tr><td><div class="checkbox-custom checkbox-default"><input type="checkbox" name="' + item.CPTCodeDescription + '" id="' + item.LabOrderTestId + '"><label for="' + item.LabOrderTestId + '"> ' + item.CPTDescription + ' </label></div></td></tr>';
                        Clinical_LabOrderABNDetail.Tests[i] = item.LabOrderTestId;
                        Clinical_LabOrderABNDetail.TestDesc[item.LabOrderTestId] = "" + item.CPTDescription;
                    });
                    finalTr = finalTr + '<tr><td id="#buttonRow"><button type="button" id="#btnPrintABNpdf" style="float:right;" class="btn btn-primary" onclick="Clinical_LabOrderABNDetail.onPrintABN();">Print</button></td></tr>';
                    $("#ulTestList tbody").append(finalTr);
                }
         });
        
    },
    
    UnLoad: function () {

        if (Clinical_LabOrderABNDetail.params != null && Clinical_LabOrderABNDetail.params.ParentCtrl) {
            UnloadActionPan(Clinical_LabOrderABNDetail.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    },
    checkBoxEntry: function () {

    },
    onPrintABN: function () {

        // Get Checked Tests
        var count = 0;
        var IsABN = new Array();
        Clinical_LabOrderABNDetail.TestABNDesc = [];
        $.each(Clinical_LabOrderABNDetail.Tests, function (i, item) {
            

            if ($('#' + item).is(':checked')) {

                IsABN[i] = "true";
                Clinical_LabOrderABNDetail.TestABNDesc[count] = Clinical_LabOrderABNDetail.TestDesc[item];
                count++;

            }
            else {
                IsABN[i] = "false";
                //Clinical_LabOrderABNDetail.TestABNDesc[count] = "";
            }
          

        })

        if (count > 0) {

            $.each(Clinical_LabOrderABNDetail.Tests, function (i, item) {
                Clinical_LabOrderABNDetail.saveABN(item, IsABN[i]);
            })
            //Call PDF viewer
            var params = [];
            params["FromAdmin"] = "0";
            params["UserId"] = globalAppdata['AppUserId'];
            params["PatientId"] = Clinical_LabOrderABNDetail.params.PatientId;
            params["ParentCtrl"] = "Clinical_LabOrderABNDetail";
            params["LabOrderId"] = Clinical_LabOrderABNDetail.params.LabOrderId;
            params["Tests"] = Clinical_LabOrderABNDetail.TestABNDesc;

            LoadActionPan('Clinical_LabOrderABNView', params);
        }
        else {
            $('#btnPrintABNpdf').prop('disabled', true);
            utility.DisplayMessages("Please select at least one test.", 4);
       
        }

        
       
           
    },
    saveABN: function(LabOrderTestId,IsABN) {
        var objData = {};
        objData["commandType"] = "save_ABNTest";
        objData["LabOrderTestId"] = LabOrderTestId;
        objData["IsABN"] = IsABN;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },


    fillLabOrder: function (LabOrderId) {

        var objData = {};
        objData["commandType"] = "fill_LabOrder";
        objData["LabOrderId"] = LabOrderId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    }
}