Bill_ClaimHcfaForm = {
    params: [],
    bIsFirstLoad: true,
    PrintUri: "",

    Load: function (params) {

        if (Bill_ClaimHcfaForm.bIsFirstLoad) {
            Bill_ClaimHcfaForm.bIsFirstLoad = false;
            Bill_ClaimHcfaForm.params = params;
            Bill_ClaimHcfaForm.LoadClaimForm();
        }
    },

    LoadClaimForm: function () {

        if (Bill_ClaimHcfaForm.params["ParentCtrl"] && Bill_ClaimHcfaForm.params["ParentCtrl"].indexOf("EncounterChargeCapture") >= 0) {
            if (Bill_ClaimHcfaForm.params["SubmitStatus"] && Bill_ClaimHcfaForm.params["IsActive"] == true && (Bill_ClaimHcfaForm.params["SubmitStatus"].toLowerCase() == "ready to submit paper" || Bill_ClaimHcfaForm.params["SubmitStatus"].toLowerCase() == "ready to submit electronic")) {
                $("#Bill_ClaimHcfaForm #btnprintnsubmit").removeClass("hidden");
            }

            $("#Bill_ClaimHcfaForm #btnprintandhistory").removeClass("hidden");
            $("#Bill_ClaimHcfaForm #btnprint").addClass("hidden");

        }


        Bill_ClaimHcfaForm.PrintClaim().done(function (response) {

            if (response.status != false) {

                //var PreviewUri = "data:application/pdf;base64," + response.PreviewClaimForm;
                //var PrintUri = "data:application/pdf;base64," + response.PrintClaimForm;

                //ParentControl
                utility.PDFViewer(response.PreviewClaimForm, false, 'Bill_ClaimHcfaForm #PreviewClaimFormIF');
                //Bill_ClaimHcfaForm.PDFViewer(response.PreviewClaimForm, false);

                ////Print ClaimForm
                Bill_ClaimHcfaForm.PrintUri = response.PrintClaimForm;

                //Bill_ClaimHcfaForm.PrintUri = "data:application/pdf;base64," + response.PrintClaimForm;

                if (Bill_ClaimHcfaForm.params.IsSearch != null && Bill_ClaimHcfaForm.params.IsSearch == false) {
                    //do nothing.
                }
                else {
                    //Search Claims
                    Bill_ClaimSubmission.Claim_Search();
                }

                if (response.Message != "")
                    utility.DisplayMessages(response.Message, 1);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }


        });

    },

    PrintClaim: function () {

        var UserBrowser = utility.UserBrowser().toLowerCase();

        var ViewOnly = false;
        if (Bill_ClaimHcfaForm.params["ViewOnly"] != undefined && Bill_ClaimHcfaForm.params["ViewOnly"] != "")
            ViewOnly = Bill_ClaimHcfaForm.params["ViewOnly"];

        var objData = new Object();

        objData["Visits"] = Bill_ClaimHcfaForm.params.Visits.toString();
        objData["isSubmit"] = Bill_ClaimHcfaForm.params.IsSubmit;
        objData["MarkSubmitted"] = Bill_ClaimHcfaForm.params.MarkSubmitted;
        objData["ClearingHouse"] = Bill_ClaimHcfaForm.params.ClearningHouseId;
        objData["ViewOnly"] = ViewOnly;
        objData["UserBrowser"] = UserBrowser;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "PrintPatientClaim");

    },

    Print_Claim_History: function () {
    
        Bill_ClaimHcfaForm.PrintClaimHistory().done(function (response) {
            if (response.status != false) {
                Bill_ClaimHcfaForm.PrintClaimForm();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    PrintClaimHistory: function () {

        var objData = new Object();

        objData["VisitId"] = Bill_ClaimHcfaForm.params.Visits.toString();
        objData["PatientId"] = Bill_ClaimHcfaForm.params.PatientId.toString();
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "PrintClaimHistory");

    },

    SubmitClaim: function (IsSubmit, MarkSubmitted) {

        var objData = new Object();

        objData["Visits"] = Bill_ClaimHcfaForm.params.Visits.toString();
        objData["isSubmit"] = IsSubmit;
        objData["MarkSubmitted"] = MarkSubmitted;
        objData["ClearingHouse"] = Bill_ClaimHcfaForm.params.ClearningHouseId;
        objData["UserBrowser"] = utility.UserBrowser().toLowerCase();

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "UpdatePatientClaim");
    },


    SubmitClaimForm: function () {

        Bill_ClaimHcfaForm.SubmitClaim(true, false).done(function (response) {
            if (response.status != false) {

                if (response.Message != "")
                    utility.DisplayMessages(response.Message, 1);

                Bill_ClaimHcfaForm.PrintClaimForm(response.PrintClaimForm);
                //after submission we'll hit here
                EncounterChargeCapture.params["IsReload"] = true;
                eval('EncounterChargeCapture.Load')(EncounterChargeCapture.params);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },


    PrintClaimForm: function (PrintUri) {

        if (PrintUri)
            utility.PDFViewer(PrintUri, true);
        else
            utility.PDFViewer(Bill_ClaimHcfaForm.PrintUri, true);
    },

    UnLoad: function () {

        if (Bill_ClaimHcfaForm.params != null && Bill_ClaimHcfaForm.params.ParentCtrl != undefined && Bill_ClaimHcfaForm.params.ParentCtrl != null) {
            UnloadActionPan(Bill_ClaimHcfaForm.params.ParentCtrl);
        }
        else
            UnloadActionPan();
    },

};