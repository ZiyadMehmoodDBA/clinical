Clinical_ProblemListInActive = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_ProblemListInActive.params = params;
        //serialize Data.
        $('#frmProblemListInActive').data('serialize', $('#frmProblemListInActive').serialize());


        utility.CreateDatePicker('frmProblemListInActive #dpEndDate', function () {
            //on-change callback method 
        });

        //var problemID = Clinical_ProblemListInActive.params.ProblemListId;
        //var patientID = Clinical_ProblemListInActive.params.PatientId;

        //Clinical_ProblemListInActive.ProblemListsSearch(problemID, patientID);

    },

    // Fill comments function

    // End Fill comments function

    ProblemListCancel: function () {

        Clinical_ProblemListInActive.UnLoad();

    },

    ProblemListInActive: function () {

        var endDate = "";
        endDate = $('#Clinical_ProblemListInActive #frmProblemListInActive #dpEndDate').val();

        var comments = "";
        comments = $('#Clinical_ProblemListInActive #frmProblemListInActive #txtComments').val();

        var selected = $('#Clinical_ProblemListInActive #frmProblemListInActive input[type=checkbox]:checked').map(function () {
            return ($(this).attr('id').replace('checkbox', ''));

        }).get(0);
        if (selected == '' || selected == null) {
            utility.DisplayMessages("Please select reason to in active Problem list", 2);
            return false;
        }

        Clinical_ProblemListInActive.InActiveProblemList(selected, comments, endDate).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                utility.DisplayMessages(response.message, 1);
                Clinical_ProblemListInActive.UnLoad();
                if (Clinical_ProblemListInActive.params.ParentCtrl == "Clinical_OrderSetDetails") {
                    OrderSet_ProblemListGrid.ProblemListsSearch();
                } else if (Clinical_ProblemListInActive.params.ParentCtrl == "OrderSet_Problems") {
                    OrderSet_Problems.ProblemListsSearch();
                } else
                    if (Clinical_ProblemListInActive.params.ParentCtrl.indexOf("CCM_Patient_Hub") >= 0) {
                        CCM_Patient_Hub.ProblemListsSearch();
                    } else {
                        Clinical_ProblemLists.ProblemListsSearch();
                    }

                Clinical_ProblemLists.UpdateProblemOnDrFirst("inactive_problemlistFromDrfirst");
            }
            else {
                utility.DisplayMessages(response.message, 1);
            }
        });


    },

    InActiveProblemList: function (selectedChkBox, comments, endDate) {
        var Controller = null;
        var IsActiveRecord = null;

        var ProblemListId = Clinical_ProblemListInActive.params.ProblemListId;
        var patientId = Clinical_ProblemListInActive.params.PatientId;
        var objData = new Object();
        if (Clinical_ProblemListInActive.params.ParentCtrl.indexOf("Clinical_OrderSetDetails") >= 0) {
            IsActiveRecord = $('#' + Clinical_OrderSetDetails.params.PanelID + ' #pnlProblemLists_ResultOS #divSwitch #switchActive').attr('isactive');
            Controller = 'OrderSet';
            objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        } else if (Clinical_ProblemListInActive.params.ParentCtrl.indexOf("OrderSet_Problems") >= 0) {
            IsActiveRecord = $("#" + OrderSet_Problems.params.PanelID + ' #pnlOrderSetProblemLists_Result #divSwitch #switchActive').attr('isactive');
            Controller = 'OrderSet';
            objData["OrderSetId"] = Clinical_OrderSetDetails.params.OrderSetId;
        } else {
            IsActiveRecord = $('#pnlClinicalProblemLists #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
            Controller = 'MEDICAL';
            objData["PatientId"] = patientId;
        }

        var IsCheckedIn = IsActiveRecord;
        if (IsActiveRecord == "1")
            IsActiveRecord = "0";
        else if (IsActiveRecord == "0")
            IsActiveRecord = "1";


        objData["ProblemListId"] = ProblemListId;

        objData["InActiveChkBoxValue"] = selectedChkBox;
        objData["InActiveReason"] = comments;
        objData["EndDate"] = endDate;
        objData["IsActive"] = IsCheckedIn;
        objData["IsActiveRecord"] = IsActiveRecord;
        objData["commandType"] = "INACTIVE_PROBLEMLIST";

        var data = JSON.stringify(objData);

        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, Controller, "ProblemList");

    },

    UnLoad: function () {

        //UnloadActionPan(Clinical_ProblemListInActive.params["ParentCtrl"], "actionPanProblemListInActive");
        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-141 */
        if (Clinical_ProblemListInActive.params.ParentCtrl == 'clinicalTabProblemLists') {
            UnloadActionPan(Clinical_ProblemListInActive.params["ParentCtrl"], "Clinical_ProblemListInActive");
        } else {
            UnloadActionPan(Clinical_ProblemListInActive.params.ParentCtrl, 'Clinical_ProblemListInActive', null, Clinical_ProblemListInActive.params.PanelID);
        }
        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-141 */
    },
}