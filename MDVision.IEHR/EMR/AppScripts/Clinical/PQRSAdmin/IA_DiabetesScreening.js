IA_DiabetesScreening={
    Load: function (params) {
        IA_DiabetesScreening.params = params;
        if (IA_DiabetesScreening.params.PanelID != 'pnlDiabtesScreening') {
            IA_DiabetesScreening.params.PanelID = IA_DiabetesScreening.params.PanelID + ' #pnlDiabtesScreening';
        }
        else {
            IA_DiabetesScreening.params.PanelID = 'pnlDiabtesScreening';
        }
        var self = $('#' + IA_DiabetesScreening.params.PanelID);
        self.loadDropDowns(true).done(function () {
            IA_DiabetesScreening.domReadyFunc();
            IA_DiabetesScreening.ValidateProblemLists();
        });
       
    },
    BindFavProblems: function () {

        var FavoriteListId = $('#' + IA_DiabetesScreening.params.PanelID + ' #ddlFavProblems').val();
        var SerachData = $('#' + IA_DiabetesScreening.params.PanelID + ' #FavSearchBox').val();
        if (FavoriteListId != "") {

            Favorite_Problems.searchFavoriteList_ICD_DBCall(null, FavoriteListId, null, null, SerachData).done(function (response) {

                response = JSON.parse(response);

                if (response.status != false) {

                    if (response.FavoriteListICDCount > 0) {

                        $('#' + IA_DiabetesScreening.params.PanelID + " #frmClinicalDiabetesLists #ulFavCompliantDisease li").remove();
                        var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
                        var li = "";

                        $.each(FavoriteListJSON, function (i, item) {

                            if (typeof item.ICD9Code == 'undefined' || item.ICD9Code == null) { item.ICD9Code = ''; }
                            if (typeof item.ICD10Code == 'undefined' || item.ICD10Code == null) { item.ICD10Code = ''; }
                            if (typeof item.SNOMEDID == 'undefined' || item.SNOMEDID == null) { item.SNOMEDID = ''; }
                            if (typeof item.ICD10CodeDescription == 'undefined' || item.ICD10CodeDescription == null) { item.ICD10CodeDescription = ''; }

                            var diagnosis = item.ICD10Code + " - " + item.ICD10CodeDescription;
                            var ICD9Code = "" + item.ICD9Code + "";
                            var ICD10Code = "" + item.ICD10Code + "";
                            var ICD9CodeDescription = "" + item.ICD9CodeDescription + "";
                            var ICD10CodeDescription = "" + item.ICD10CodeDescription + "";
                            var SNOMEDID = "" + item.SNOMEDID + "";
                            var SNOMEDDescription = "" + item.SNOMEDDescription + "";

                            li += "<li  id=" + item.FavoriteListICDId + " onclick='IA_DiabetesScreening.PopulateFields(this,\"" + diagnosis + "\",\"" + ICD9Code + "\",\"" + ICD10Code + "\",\"" + ICD9CodeDescription + "\",\"" + ICD10CodeDescription + "\",\"" + SNOMEDID + "\",\"" + SNOMEDDescription + "\");' ><a href='#' class='pr-sm'>" + item.ICD10CodeDescription + "</a></li>";
                        });

                        $('#pnlDiabtesScreening #ulFavCompliantDisease').append(li);

                        //Save
                    }
                    else {
                        $('#' + IA_DiabetesScreening.params.PanelID + " #frmClinicalDiabetesLists #ulFavCompliantDisease li").remove();
                    }
                }
                else {
                    $('#' + IA_DiabetesScreening.params.PanelID + " #frmClinicalDiabetesLists #ulFavCompliantDisease li").remove();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        if ($('#pnlDiabtesScreening #ddlFavProblems').val() == '' || $('#pnlDiabtesScreening #ddlFavProblems').val() == '- Select -') {
            $('#pnlDiabtesScreening #ulFavCompliantDisease li').remove();
        }
    },
    BindICD9AutoComplete: function (element) {

        $('#pnlDiabtesScreening #txtProblems').attr("data-popupunload", "false");

        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "IA_DiabetesScreening", null, false);
    },
    domReadyFunc: function () {

        $(document).ready(function () {

            $('#' + IA_DiabetesScreening.params.PanelID + ' .toggleHorSmallLeft section').unbind('click').bind("click", function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));

            });

            //for autocomplete zIndex fix
            $("#txtProblems").on("autocompleteopen", function (event, ui) {
                if ($(this).closest(".modal-dialog").length == 0)
                    $(this).autocomplete('widget').zIndex("1018");
            });
        });

    },
    PopulateFields: function (cntrl, diagnosis, ICD9Code, ICD10Code, ICD9CodeDescription, ICD10CodeDescription, SNOMEDID, SNOMEDDescription) {

       

        var lii = "<li icd9Code=\"" + ICD9Code + "\" icd9Desc=\"" + ICD9CodeDescription + "\" icd10Code=\"" + ICD10Code + "\" icd10Desc=\"" + ICD10CodeDescription + "\" snomedCode=\"" + SNOMEDID + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#' class='pr-sm'>" + ICD10CodeDescription + "</a></li>"
        var IsAlreadyExist = false;
        $('#pnlDiabtesScreening #ulProblemDisease li').each(function () {
            if ($(this).attr('snomedCode') == SNOMEDID) {
                IsAlreadyExist = true;
            }
        });
        if (!IsAlreadyExist) {
            $('#pnlDiabtesScreening #ulProblemDisease').html(lii);
            $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtProblems").val(ICD10Code);
            $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtICD9Code").val(ICD9Code);
            $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtICD9Description").val(ICD9CodeDescription);
            $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtICD10Code").val(ICD10Code); // Selected List Item ID i.e. [ICD10 Code]
            $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtICD10Description").val(ICD10CodeDescription); // Selected List Item text i.e. [ICD10 description]
            $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtSnomedCode").val(SNOMEDID);
            $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtSnomedDescription").val(SNOMEDDescription);
            var formValidation = $('#' + IA_DiabetesScreening.params.PanelID + ' #frmClinicalDiabetesLists').data("bootstrapValidator");
            formValidation.enableFieldValidators('ProblemName', true);
        }
        else {
            utility.DisplayMessages('Problem List already added', 2);
        }
       
    },
    OpenSearchPopup: function () {
        var controlToLoad = "";
        controlToLoad = "Admin_IMOICD";
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "IA_DiabetesScreening";
        params["RefCtrl"] = "txtProblems";

        $('#pnlDiabtesScreening #txtProblems').attr('data-popupunload', 'true');

        params["Parent"] = 'pnlAdminIMOICD';
        HiddenCtrl = 'hfICD1-1,hfICDDescription1-1,hfICD101-1,hfICD10Description1-1,hfSNOMED1-1,hfSNOMEDDescription1-1';
        params["RefHiddenCtrl"] = HiddenCtrl;
        LoadActionPan(controlToLoad, params);
    },
    UnLoad: function () {
        UnloadActionPan(IA_DiabetesScreening.params.ParentCtrl)
    },
    SaveDiabeticScreeningProblem: function () {
        var self = $("#" + IA_DiabetesScreening.params.PanelID);
        var myJSON = self.getMyJSONByName();
        IA_DiabetesScreening.SaveProblemLists(myJSON).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            IA_DiabetesScreening.UnLoad();
        });
    },
    // Validate and save/edit functions
    ValidateProblemLists: function () {
       // $('#' + IA_DiabetesScreening.params.PanelID + ' #frmClinicalDiabetesLists').unbind("success.form.bv");
        $('#' + IA_DiabetesScreening.params.PanelID + ' #frmClinicalDiabetesLists')
           .bootstrapValidator({
               
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   ProblemName: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            IA_DiabetesScreening.SaveDiabeticScreeningProblem();
        });
    },
    SaveProblemLists: function (ProblemListsData) {
       var descicd9= IA_DiabetesScreening.RemoveDashSignFromStr($("#pnlDiabtesScreening #frmClinicalDiabetesLists #ulProblemDisease li").attr("icd9desc"));
       var descicd10 = IA_DiabetesScreening.RemoveDashSignFromStr($("#pnlDiabtesScreening #frmClinicalDiabetesLists #ulProblemDisease li").attr("icd10desc"));
        var objData = JSON.parse(ProblemListsData);
       
        objData.PatientId = IA_DiabetesScreening.params.PatientIds;
        objData["ProblemName"] = descicd10 +descicd9;
        objData["ICD9"] = $("#pnlDiabtesScreening #frmClinicalDiabetesLists #ulProblemDisease li").attr("icd9code");
        objData["ICD10"] = $("#pnlDiabtesScreening #frmClinicalDiabetesLists #ulProblemDisease li").attr("icd10code");
        objData["ICD9_Description"] = $("#pnlDiabtesScreening #frmClinicalDiabetesLists #ulProblemDisease li").attr("icd9desc");
        objData["ICD10_Description"] = $("#pnlDiabtesScreening #frmClinicalDiabetesLists #ulProblemDisease li").attr("icd10desc");
        objData["SNOMEDID"] = $("#pnlDiabtesScreening #frmClinicalDiabetesLists #ulProblemDisease li").attr("snomedcode");
        objData["SNOMED_DESCRIPTION"] = $("#pnlDiabtesScreening #frmClinicalDiabetesLists #ulProblemDisease li").attr("snomeddesc");
        objData["ICD9_Description"] = descicd9;
        objData["ICD10_Description"] = descicd10;
        objData["Comments"] = $("#pnlDiabtesScreening #frmClinicalDiabetesLists #txtComments").val();
        objData["IsNonDiabetic"] = $("#chkDiabetic").is(":checked") ? true : false;
        objData["ProviderId"] = IA_DiabetesScreening.params.ProviderId;
        objData["IsDiabeticScreening"] = true;
        //--------------------------
        objData["commandType"] = "SAVE_PROBLEMLIST";
        
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    RemoveDashSignFromStr: function (str) {
        if (str != null && str.indexOf(' - ') > -1) {
            var strArray = str.split(' - ');
            return strArray[strArray.length - 1].trim();
        } else {
            return str;
        }
    },

}