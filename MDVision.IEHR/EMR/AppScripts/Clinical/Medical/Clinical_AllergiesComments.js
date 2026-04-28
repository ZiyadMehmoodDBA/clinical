//Author: Ahmad Raza
//Date: 03-12-2015
//This file will handle all actions performed for Allergy comments popup
Clinical_AllergiesComments = {
    bIsFirstLoad: true,
    params: [],
    //Start//03/12/2015//Ahmad Raza//This function will load the selected allergy's existing comments in popup textarea
    Load: function (params) {
      
        Clinical_AllergiesComments.params = params;

        var allergyID = Clinical_AllergiesComments.params.AllergyId;
        var patientID = Clinical_AllergiesComments.params.PatientId;

        Clinical_AllergiesComments.allergiesSearch(allergyID, patientID);

    },
    //End//03/12/2015//Ahmad Raza//This function will load the selected allergy's existing comments in popup textarea
  
    //Start//03/12/2015//Ahmad Raza//This function will search selected allergy's data
    allergiesSearch: function (AllergyId, PatientId) {
       
        Clinical_AllergiesComments.searchAllergy(AllergyId, PatientId).done(function (response) {
           
            response = JSON.parse(response);
            if (response.status != false) {
                var allergyJSON = JSON.parse(response.allergiesLoad_JSON);

                var comments = allergyJSON[0].Comments;

                $('#Clinical_AllergiesComments #txtComments').val(comments);

                var comments = $('#Clinical_AllergiesComments #txtComments').val();
                $('#pnlClinicalAllergies #hfGridComments').val(comments);

                $('#frmAllergiesComments').data('serialize', $('#frmAllergiesComments').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    //End//03/12/2015//Ahmad Raza//This function will search selected allergy's data

    //Start//03/12/2015//Ahmad Raza//This function will call database to get selected allergy's data
    searchAllergy: function (AllergyId, PatientId) {
       
        var objData = new Object();
        objData["AllergyId"] = AllergyId;
        objData["PatientId"] = PatientId;
        objData["commandType"] = "SEARCH_ALLERGY";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");

    },
    //End//03/12/2015//Ahmad Raza//This function will call database to get selected allergy's data

    //Start//03/12/2015//Ahmad Raza//This function will save the new allergy comments and will call the unload method of the comments popup
    saveComments: function () {

        var comments = $('#Clinical_AllergiesComments #txtComments').val();

        $('#pnlClinicalAllergies #hfGridComments').val(comments);

        $('#frmAllergiesComments').data('serialize', $('#frmAllergiesComments').serialize());

        Clinical_AllergiesComments.unLoad();
    },
    //End//03/12/2015//Ahmad Raza//This function will save the new allergy comments and will call the unload method of the comments popup

    //Start//03/12/2015//Ahmad Raza//This function will call the database to update the comments of selected allergy
    commentsSave: function () {

        var allergyId = Clinical_AllergiesComments.params.AllergyId;
        var patientId = Clinical_AllergiesComments.params.PatientId;
        var comments = $('#Clinical_AllergiesComments #txtComments').val();

        var objData = new Object();
        objData["AllergyId"] = allergyId;
        objData["PatientId"] = patientId;
        objData["Comments"] = comments;
        objData["commandType"] = "UPDATE_ALLERGYCOMMENTS";

        var data = JSON.stringify(objData);

 
        return MDVisionService.APIService(data, "MEDICAL", "Allergy");

    },
    //End//03/12/2015//Ahmad Raza//This function will call the database to update the comments of selected allergy

    //Start//03/12/2015//Ahmad Raza//This function will Unload the allergy comments popup
    unLoad: function () {

       
        //Start//23/12/2015//Ahmad Raza//Fixed EMR Bug#149
        if (Clinical_AllergiesComments.params.ParentCtrl == 'clinicalTabAllergies') {
            UnloadActionPan(Clinical_AllergiesComments.params["ParentCtrl"], "Clinical_AllergiesComments");
        } else {
            UnloadActionPan(Clinical_AllergiesComments.params.ParentCtrl, 'Clinical_AllergiesComments', null, Clinical_AllergiesComments.params.PanelID);
        }
        //End//23/12/2015//Ahmad Raza//Fixed EMR Bug#149

    },
    //End//03/12/2015//Ahmad Raza//This function will Unload the allergy comments popup

}