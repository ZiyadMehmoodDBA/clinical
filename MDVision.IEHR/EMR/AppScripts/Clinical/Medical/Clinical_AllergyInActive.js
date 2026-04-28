//Author: Ahmad Raza
//Date: 04-12-2015
//This file will handle all actions performed for Allergy InActive PopUp
Clinical_AllergyInActive = {
    bIsFirstLoad: true,
    params: [],
    //Start//04/12/2015//Ahmad Raza//This function is serializing the InActive form and calling the domReadyFunction  
    Load: function (params) {
       
        Clinical_AllergyInActive.params = params;
        //serialize Data.
        $('#frmAllergyInActive').data('serialize', $('#frmAllergyInActive').serialize());

        Clinical_AllergyInActive.domReadyFunction();
    },
    //End//04/12/2015//Ahmad Raza//This function is serializing the InActive form and calling the domReadyFunction  

    //Start//04/12/2015//Ahmad Raza//This function is getting the checkbox values of InActive form  
    domReadyFunction: function () {

        $("#Clinical_AllergyInActive input:checkbox").on('click', function () {
            // in the handler, 'this' refers to the box clicked on
            var $box = $(this);
            if ($box.is(":checked")) {
                var group = "input:checkbox[name='" + $box.attr("name") + "']";

                $(group).prop("checked", false);
                $box.prop("checked", true);
            } else {
                $box.prop("checked", false);
            }
        });
    },
    //End//04/12/2015//Ahmad Raza//This function is getting the checkbox values of InActive form

    //Start//04/12/2015//Ahmad Raza//This function will close the InActive Allergy popup
    allergyCancel: function () {

        Clinical_AllergyInActive.unLoad();

    },
    //End//04/12/2015//Ahmad Raza//This function will close the InActive Allergy popup

    //Start//04/12/2015//Ahmad Raza//This function will check and show alert if no checkbox is checked and after that will call inActiveAllergy function,unLoad function and Search function
    allergyInActive: function () {
        var comments = "";
        comments = $('#Clinical_AllergyInActive #frmAllergyInActive #txtComments').val();

        var selected = $('#Clinical_AllergyInActive #frmAllergyInActive input[type=checkbox]:checked').map(function () {
            return ($(this).attr('id').replace('checkbox', ''));

        }).get(0);
        if (selected == '' || selected == null) {
            utility.DisplayMessages("Please select reason to in active Allergy", 2);
            return false;
        }

        Clinical_AllergyInActive.inActiveAllergy(selected, comments).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                utility.DisplayMessages(response.message, 1);
                Clinical_AllergyInActive.unLoad();
                Clinical_Allergies.allergiesSearch();
            }
            else {
                utility.DisplayMessages(response.message, 1);
            }
        });


    },
    //End//04/12/2015//Ahmad Raza//This function will check and show alert if no checkbox is checked and after that will call inActiveAllergy function,unLoad function and Search function

    //Start//04/12/2015//Ahmad Raza//This function will call DB to update the Allergy Status as InActive with reasons
    inActiveAllergy: function (selectedChkBox, comments) {

        var isActive = null;
        isActive = $('#pnlClinicalAllergies #pnlAllergies_Result #divSwitch #switchActive').attr('isactive');

        var isActiveRecord = null;
        isActiveRecord = $('#pnlClinicalAllergies #pnlAllergies_Result #divSwitch #switchActive').attr('isactive');
        if (isActiveRecord == "1")
            isActiveRecord = "0";
        else if (isActiveRecord == "0")
            isActiveRecord = "1";

        var AllergyId = Clinical_AllergyInActive.params.AllergyId;
        var patientId = Clinical_AllergyInActive.params.PatientId;

        var objData = new Object();
        objData["AllergyId"] = AllergyId;
        objData["PatientId"] = patientId;
        objData["InActiveChkBoxValue"] = selectedChkBox;
        objData["InActiveReason"] = comments;
        //objData["EndDate"] = endDate;
        objData["IsActive"] = isActive;
        objData["IsActiveRecord"] = isActiveRecord;
        objData["commandType"] = "INACTIVE_ALLERGY";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "Allergy");

    },
    //End//04/12/2015//Ahmad Raza//This function will call DB to update the Allergy Status as InActive with reasons

    //Start//04/12/2015//Ahmad Raza//This function will unLoad the InActiveAllergy popup
    unLoad: function () {

        //UnloadActionPan(Clinical_AllergyInActive.params["ParentCtrl"], "actionPanAllergyInActive");

        //Start//23/12/2015//Ahmad Raza//Fixed EMR Bug#150
        if (Clinical_AllergyInActive.params.ParentCtrl == 'clinicalTabAllergies') {
            UnloadActionPan(Clinical_AllergyInActive.params["ParentCtrl"], "Clinical_AllergyInActive");
        } else {
            UnloadActionPan(Clinical_AllergyInActive.params.ParentCtrl, 'Clinical_AllergyInActive', null, Clinical_AllergyInActive.params.PanelID);
        }
        //Start//23/12/2015//Ahmad Raza//Fixed EMR Bug#150
    },
    //End//04/12/2015//Ahmad Raza//This function will unLoad the InActiveAllergy popup
}