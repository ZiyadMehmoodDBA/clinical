FavMedication_Detail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        FavMedication_Detail.params = params;
        if (FavMedication_Detail.params.PanelID != 'pnlFavMedicationDetail')
            FavMedication_Detail.params.PanelID = FavMedication_Detail.params.PanelID + ' #pnlFavMedicationDetail';
        else
            FavMedication_Detail.params.PanelID = 'pnlFavMedicationDetail';
        var self = $('#' + FavMedication_Detail.params.PanelID);

        self.loadDropDowns(true).done(function () {
            FavMedication_Detail.ValidateMedication();
            if (FavMedication_Detail.params.mode == "Edit") {
                $.when(FavMedication_Detail.LoadMedication(FavMedication_Detail.params.FavMedicationId)).then(function () {
                    $('#' + FavMedication_Detail.params.PanelID + " #frmFavMedicationDetail").data('serialize', $('#' + FavMedication_Detail.params.PanelID + " #frmFavMedicationDetail").serialize());
                });
            }
            else {
                //$('#' + FavMedication_Detail.params.PanelID + " #frmFavMedicationDetail").data('serialize', $('#' + FavMedication_Detail.params.PanelID + " #frmFavMedicationDetail").serialize());
                if (FavMedication_Detail.params.MedicationObject) {
                    var medicationObject = FavMedication_Detail.params.MedicationObject
                    $('#' + FavMedication_Detail.params.PanelID + ' #hfMedication').val(medicationObject.id);
                    $('#' + FavMedication_Detail.params.PanelID + ' #txtMedication').val(medicationObject.value);
                    $('#' + FavMedication_Detail.params.PanelID + ' #NDCID').val(medicationObject.NDCID);
                    $('#' + FavMedication_Detail.params.PanelID + ' #BrandName').val(medicationObject.BrandName);
                    $('#' + FavMedication_Detail.params.PanelID + ' #GenericName').val(medicationObject.GenericName);
                    $('#' + FavMedication_Detail.params.PanelID + ' #Form').val(medicationObject.Form);
                    $('#' + FavMedication_Detail.params.PanelID + ' #Strength').val(medicationObject.Strength);
                }
            }
        });
    },
    ValidateMedication: function () {
        $('#' + FavMedication_Detail.params.PanelID + ' #frmFavMedicationDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Medication: {
                       group: '.col-sm-3',
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
            FavMedication_Detail.SaveMedication();
        });
    },
    LoadMedication: function (FavMedicationId) {
        var dfd = $.Deferred();
        FavMedication_Detail.searchMedication(1, 1, FavMedicationId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var MedicationDetails = response.Medication_JSON;
                var self = $('#' + FavMedication_Detail.params.PanelID + " #frmFavMedicationDetail");
                utility.bindMyJSONByName(true, MedicationDetails, false, self).done(function () {
                    $('#' + FavMedication_Detail.params.PanelID + ' #hfMedication').val(MedicationDetails.Id);
                    $('#' + FavMedication_Detail.params.PanelID + ' #txtMedication').val(MedicationDetails.BrandName + " " + MedicationDetails.Strength);
                    $('#' + FavMedication_Detail.params.PanelID + ' #NDCID').val(MedicationDetails.NDCID);
                    $('#' + FavMedication_Detail.params.PanelID + ' #BrandName').val(MedicationDetails.BrandName);
                    $('#' + FavMedication_Detail.params.PanelID + ' #GenericName').val(MedicationDetails.GenericName);
                    $('#' + FavMedication_Detail.params.PanelID + ' #Form').val(MedicationDetails.Form);
                    $('#' + FavMedication_Detail.params.PanelID + ' #Strength').val(MedicationDetails.Strength);
                    utility.SetAutoCompleteSource($("#frmFavMedicationDetail #txtMedication"), $('#' + FavMedication_Detail.params.PanelID + ' #hfMedication'));
                    var found = false;
                    $('#' + FavMedication_Detail.params.PanelID + ' #ddlDoseSelect').find('option').each(function (i, item) {
                        if ($(item).text().trim() == $('#' + FavMedication_Detail.params.PanelID + ' #txtDose').val()) {
                            found = true;
                            $(item).attr('selected', true);
                        }
                    });
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd;
    },
    searchMedication: function (pageNumber, rowsPerPage, FavMedicationId) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        objData["id"] = FavMedicationId;
        objData["FavoriteListId"] = FavMedication_Detail.params.FavoriteListId;
        objData["pageNumber"] = pageNumber;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_Medication";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteListMedicationDetail");
    },
    DoseSelectChange: function (obj) {
        if ($(obj).val() != "") {
            $("#" + FavMedication_Detail.params.PanelID + " #txtDose").val($("#" + FavMedication_Detail.params.PanelID + " #ddlDoseSelect option:selected").text());
            FavMedication_Detail.DurationChange();
        }
        else
            $("#" + FavMedication_Detail.params.PanelID + " #txtDose").val("1");
    },
    DoseChange: function (obj) {
        FavMedication_Detail.DurationChange();
    },
    DoseTimingChange: function () {
        FavMedication_Detail.DurationChange();
    },
    DurationChange: function () {

        var Dose = $("#" + FavMedication_Detail.params.PanelID + " #txtDose").val();
        var DoseMath;
        if (Dose != "" && $("#" + FavMedication_Detail.params.PanelID + " #ddlDuration option:selected").text() != "- Select -") {
            if (Dose == '1 1/2') {
                DoseMath = 1.5;
            }
            else if (Dose == '1 1/4') {
                DoseMath = 1.25;
            }
            else if (Dose == '0.5') {
                DoseMath = 1.25;
            }
            else if (Dose == '1/4') {
                DoseMath = 0.25;
            }
            else if (Dose == '2 1/2') {
                DoseMath = 2.5;
            }
            else if (Dose == '3/4') {
                DoseMath = 0.75;
            }
            else {
                DoseMath = parseInt(Dose);
            }

            var Duration = $("#" + FavMedication_Detail.params.PanelID + " #ddlDuration option:selected").text().replace(" days", "");
            var DurationActual;
            if (Duration == 'five') {
                DurationActual = 5;
            }
            else if (Duration == 'four') {
                DurationActual = 4;
            }
            else if (Duration == 'one day') {
                DurationActual = 1;
            }
            else if (Duration == 'seven') {
                DurationActual = 7;
            }
            else if (Duration == 'six') {
                DurationActual = 6;
            }
            else if (Duration == 'three') {
                DurationActual = 3;
            }
            else if (Duration == 'two') {
                DurationActual = 2;
            }
            else {
                DurationActual = Duration;
            }
            $("#" + FavMedication_Detail.params.PanelID + " #txtQuantity").val(Math.ceil(DoseMath * (FavMedication_Detail.AddDoseTimingValue(parseInt(DurationActual)))));
        }
        else {
            $("#" + FavMedication_Detail.params.PanelID + " #txtQuantity").val("");
        }
    },
    AddDoseTimingValue: function (Days) {
        var DoseTimingText = $("#" + FavMedication_Detail.params.PanelID + " #ddlDoseTiming option:selected").text();
        if (DoseTimingText != "- Select -") {
            var WhatToDo = "";
            var WhichValue = "";
            if (DoseTimingText == "as directed") {
                WhatToDo = "nothing";
            }
            else if (DoseTimingText == "at bedtime") {
                WhatToDo = "nothing";
            }
            else if (DoseTimingText == "every 24 hours") {
                WhatToDo = "nothing";
            }
            else if (DoseTimingText == "every 48 hours") {
                WhatToDo = "d";
                WhichValue = 2;
            }
            else if (DoseTimingText == "every 72 hours") {
                WhatToDo = "d";
                WhichValue = 3;
            }
            else if (DoseTimingText == "every eight hours") {
                WhatToDo = "m";
                WhichValue = 3;
            }
            else if (DoseTimingText == "every eight to twelve hours") {
                WhatToDo = "m";
                WhichValue = 3;
            }
            else if (DoseTimingText == "every four hours") {
                WhatToDo = "m";
                WhichValue = 6;
            }
            else if (DoseTimingText == "every four hours while awake") {
                WhatToDo = "m";
                WhichValue = 5;
            }
            else if (DoseTimingText == "every four to six hours") {
                WhatToDo = "m";
                WhichValue = 6;
            }
            else if (DoseTimingText == "every four to six hours while awake") {
                WhatToDo = "m";
                WhichValue = 5;
            }
            else if (DoseTimingText == "every morning") {
                WhatToDo = "nothing";
            }
            else if (DoseTimingText == "every night") {
                WhatToDo = "nothing";
            }
            else if (DoseTimingText == "every other day") {
                WhatToDo = "d";
                WhichValue = 2;
            }
            else if (DoseTimingText == "every six hours") {
                WhatToDo = "m";
                WhichValue = 4;
            }
            else if (DoseTimingText == "every six to eight hours") {
                WhatToDo = "m";
                WhichValue = 3.5;
            }
            else if (DoseTimingText == "every three days") {
                WhatToDo = "d";
                WhichValue = 3;
            }
            else if (DoseTimingText == "every three hours") {
                WhatToDo = "m";
                WhichValue = 8;
            }
            else if (DoseTimingText == "every three hours while awake") {
                WhatToDo = "m";
                WhichValue = 6;
            }
            else if (DoseTimingText == "every three months") {
                WhatToDo = "d";
                WhichValue = 91;
            }
            else if (DoseTimingText == "every three to four hours") {
                WhatToDo = "m";
                WhichValue = 8;
            }
            else if (DoseTimingText == "every three to four hours while awake") {
                WhatToDo = "m";
                WhichValue = 6;
            }
            else if (DoseTimingText == "every twelve hours") {
                WhatToDo = "m";
                WhichValue = 2;
            }
            else if (DoseTimingText == "every two hours") {
                WhatToDo = "m";
                WhichValue = 12;
            }
            else if (DoseTimingText == "every two hours while awake") {
                WhatToDo = "m";
                WhichValue = 8;
            }
            else if (DoseTimingText == "every two weeks") {
                WhatToDo = "d";
                WhichValue = 14;
            }
            else if (DoseTimingText == "five times a day") {
                WhatToDo = "m";
                WhichValue = 5;
            }
            else if (DoseTimingText == "four times a day") {
                WhatToDo = "m";
                WhichValue = 4;
            }
            else if (DoseTimingText == "once a day") {
                WhatToDo = "nothing";
            }
            else if (DoseTimingText == "once a month") {
                WhatToDo = "d";
                WhichValue = 30;
            }
            else if (DoseTimingText == "once a week") {
                WhatToDo = "d";
                WhichValue = 7;
            }
            else if (DoseTimingText == "once every two weeks") {
                WhatToDo = "d";
                WhichValue = 14;
            }
            else if (DoseTimingText == "single dose") {
                WhatToDo = "nothing";
            }
            else if (DoseTimingText == "three times a day") {
                WhatToDo = "m";
                WhichValue = 3;
            }
            else if (DoseTimingText == "three times a week") {
                WhatToDo = "m";
                WhichValue = (3 / 7);
            }
            else if (DoseTimingText == "twice a day") {
                WhatToDo = "m";
                WhichValue = 2;
            }
            else if (DoseTimingText == "twice a week") {
                WhatToDo = "m";
                WhichValue = (2 / 7);
            }
            else {
                WhatToDo = "";
                WhichValue = "";
            }
            if (WhatToDo != "" && WhatToDo != "nothing") {
                if (WhatToDo == "m")
                    return Days * WhichValue;
                else if (WhatToDo == "d")
                    return Math.ceil(Days / WhichValue);
                else
                    return Days;
            }
            else
                return Days;
        }
        else
            return Days;
    },

    SaveMedication: function () {
        var self = $("#" + FavMedication_Detail.params.PanelID + " #frmFavMedicationDetail");
        var myJSON = self.getMyJSONByName();
        if (FavMedication_Detail.params.mode == "Add") {
            FavMedication_Detail.SaveMedication_DBCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' + FavMedication_Detail.params.PanelID + " #frmFavMedicationDetail").data('serialize', $('#' + FavMedication_Detail.params.PanelID + " #frmFavMedicationDetail").serialize());
                    utility.DisplayMessages(response.Message, 1);
                    Favorite_MedicationDetail.loadFavoriteListMedications(FavMedication_Detail.params.FavoriteListId);
                    Favorite_Medication.favoriteList_MedicationSearch(FavMedication_Detail.params.FavoriteListId);
                    FavMedication_Detail.UnLoad();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else if (FavMedication_Detail.params.mode == "Edit") {
            FavMedication_Detail.SaveMedication_DBCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' + FavMedication_Detail.params.PanelID + " #frmFavMedicationDetail").data('serialize', $('#' + FavMedication_Detail.params.PanelID + " #frmFavMedicationDetail").serialize());
                    utility.DisplayMessages(response.Message, 1);
                    Favorite_MedicationDetail.loadFavoriteListMedications(FavMedication_Detail.params.FavoriteListId);
                    Favorite_Medication.favoriteList_MedicationSearch(FavMedication_Detail.params.FavoriteListId);
                    FavMedication_Detail.UnLoad();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SaveMedication_DBCall: function (MedicationData) {
        var objData = JSON.parse(MedicationData);
        objData.FavoriteListId = FavMedication_Detail.params.FavoriteListId;
        if (FavMedication_Detail.params.mode == "Add")
            objData["commandType"] = "SaveFavMedicationDetail";
        else if (FavMedication_Detail.params.mode == "Edit") {
            objData["commandType"] = "UpdateFavMedicationDetail";
            objData["Id"] = FavMedication_Detail.params.FavMedicationId;
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteListMedicationDetail");
    },
    UnLoad: function () {
        var form = '#' + FavMedication_Detail.params.PanelID + " #frmFavMedicationDetail";
        utility.UnLoadDialog(form, function () {
            if (FavMedication_Detail.params != null && FavMedication_Detail.params.ParentCtrl)
                UnloadActionPan(FavMedication_Detail.params.ParentCtrl, "FavMedication_Detail");
            else
                UnloadActionPan(null, "FavMedication_Detail");
        },
        function () {
            if (FavMedication_Detail.params != null && FavMedication_Detail.params.ParentCtrl)
                UnloadActionPan(FavMedication_Detail.params.ParentCtrl, "FavMedication_Detail");
            else
                UnloadActionPan(null, "FavMedication_Detail");
        });
    },
}