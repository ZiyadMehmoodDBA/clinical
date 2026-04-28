OrderSet_Medications = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        OrderSet_Medications.params = params;
        if (OrderSet_Medications.params.PanelID != 'pnlOrderSetMedications') {
            OrderSet_Medications.params.PanelID = OrderSet_Medications.params.PanelID + ' #pnlOrderSetMedications';
        } else {
            OrderSet_Medications.params.PanelID = 'pnlOrderSetMedications';
        }

        var self = $('#' + OrderSet_Medications.params.PanelID);

        if (OrderSet_Medications.bIsFirstLoad == true) {
            OrderSet_Medications.bIsFirstLoad = false;
            self.loadDropDowns(true).done(function () {
                OrderSet_Medications.ValidateMedication();
                if (OrderSet_Medications.params.mode == "Edit") {
                    $.when(OrderSet_Medications.LoadMedication(OrderSet_Medications.params.OS_MedicationId)).then(function () {
                        $('#' + OrderSet_Medications.params.PanelID + " #frmOrderSetMedications").data('serialize', $('#' + OrderSet_Medications.params.PanelID + " #frmOrderSetMedications").serialize());
                    });
                }
                else {
                    $($("#frmOrderSetMedications #txtMedication")).autocomplete({
                        autoFocus: true,
                        source: [{ id: "-1", value: "" }],
                        select: function (event, ui) { }
                    });
                    $('#' + OrderSet_Medications.params.PanelID + " #frmOrderSetMedications").data('serialize', $('#' + OrderSet_Medications.params.PanelID + " #frmOrderSetMedications").serialize());
                }
            });
        }

    },
    ValidateMedication: function () {
        $('#' + OrderSet_Medications.params.PanelID + ' #frmOrderSetMedications')
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
            if (utility.ValidateAutoComplete($('#' + OrderSet_Medications.params.PanelID + ' #frmOrderSetMedications #txtMedication'), 'pnlOrderSetMedications #hfMedication', false, false, true)) {
                OrderSet_Medications.Save();
            }
            else {
                $("#" + OrderSet_Medications.params.PanelID + " #frmOrderSetMedications").bootstrapValidator('revalidateField', 'Medication');
            }
            
        });
    },
    LoadMedication: function (OS_MedicationId) {
        var dfd = $.Deferred();
        Clinical_OrderSetDetails.searchMedication(1, 1, OS_MedicationId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.MedicationCount > 0) {
                    var MedicationDetails = response.Medication_JSON[0];
                    var self = $('#' + OrderSet_Medications.params.PanelID + " #frmOrderSetMedications");
                    utility.bindMyJSONByName(true, MedicationDetails, false, self).done(function () {
                        $('#' + OrderSet_Medications.params.PanelID + ' #hfMedication').val(-1);
                        $("#frmOrderSetMedications #txtMedication").val(MedicationDetails.BrandName + " " + MedicationDetails.Strength);
                        $('#' + OrderSet_Medications.params.PanelID + ' #NDCID').val(MedicationDetails.NDCID);
                        $('#' + OrderSet_Medications.params.PanelID + ' #BrandName').val(MedicationDetails.BrandName);
                        $('#' + OrderSet_Medications.params.PanelID + ' #GenericName').val(MedicationDetails.GenericName);
                        $('#' + OrderSet_Medications.params.PanelID + ' #Form').val(MedicationDetails.Form);
                        $('#' + OrderSet_Medications.params.PanelID + ' #Strength').val(MedicationDetails.Strength);
                        utility.SetAutoCompleteSource($("#frmOrderSetMedications #txtMedication"), $('#' + OrderSet_Medications.params.PanelID + ' #hfMedication'));
                        var found = false;
                        $('#' + OrderSet_Medications.params.PanelID + ' #ddlDoseSelect').find('option').each(function (i, item) {
                            if ($(item).text().trim() == $('#' + OrderSet_Medications.params.PanelID + ' #txtDose').val()) {
                                found = true;
                                $(item).attr('selected', true);
                            }
                        });
                        dfd.resolve();
                    });
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd;
    },
    BindMedicationAutoComplete: function (obj) {
        var Med = $('#' + OrderSet_Medications.params.PanelID + ' #txtMedication').val();
        utility.Keyupdelay(function () {
            OrderSet_Medications.GetDrugArray(Med).done(function (response) {
                $("#frmOrderSetMedications #txtMedication").autocomplete({
                    autoFocus: true,
                    source: response,
                    select: function (event, ui) {
                        setTimeout(function () {
                            $('#' + OrderSet_Medications.params.PanelID + ' #hfMedication').val(ui.item.id);
                            $("#frmOrderSetMedications #txtMedication").val(ui.item.value);
                            $('#' + OrderSet_Medications.params.PanelID + ' #NDCID').val(ui.item.NDCID);
                            $('#' + OrderSet_Medications.params.PanelID + ' #BrandName').val(ui.item.BrandName);
                            $('#' + OrderSet_Medications.params.PanelID + ' #GenericName').val(ui.item.GenericName);
                            $('#' + OrderSet_Medications.params.PanelID + ' #Form').val(ui.item.Form);
                            $('#' + OrderSet_Medications.params.PanelID + ' #Strength').val(ui.item.Strength);
                        }, 100);
                    }
                });
                $("#frmOrderSetMedications #txtMedication").autocomplete("search");
            });
        });
        if (Med == "") {
            $('#' + OrderSet_Medications.params.PanelID + ' #hfMedication').val("");
            $("#frmOrderSetMedications #txtMedication").val("");
            $('#' + OrderSet_Medications.params.PanelID + ' #NDCID').val("");
            $('#' + OrderSet_Medications.params.PanelID + ' #BrandName').val("");
            $('#' + OrderSet_Medications.params.PanelID + ' #GenericName').val("");
            $('#' + OrderSet_Medications.params.PanelID + ' #Form').val("");
            $('#' + OrderSet_Medications.params.PanelID + ' #Strength').val("");
        }
    },
    GetDrugArray: function (Med) {
        var AllDrug = [];
        var IsValid = false;
        if (Med != null && Med.length > 1) {
            IsValid = true;
        }
        else {
            IsValid = false;
        }
        var dfd = new $.Deferred();
        if (IsValid) {
            OrderSet_Medications.GetGrugArray_DBCALL(Med).done(function (responseData) {
                if (responseData.status != false) {
                    responseData = JSON.parse(responseData)
                    var DrugsData = JSON.parse(responseData.DrugsData);
                    if (DrugsData.length == 0) {
                        utility.DisplayMessages("No Record Found", 2);
                    }
                    else {
                        var id = -2;
                        $.each(DrugsData, function (i, item) {
                            AllDrug.push({ id: id, value: item.BrandName + " " + item.Strength, NDCID: item.NDCID, BrandName: item.BrandName, GenericName: item.GenericName, Form: item.Form, Strength: item.Strength });
                            id--;
                        });
                    }
                }

                dfd.resolve(AllDrug);
            });
        }
        else {
            utility.DisplayMessages("Please enter minimum  2 characters in the search field", 2);
            dfd.resolve(AllDrug);
        }
        return dfd.promise();

    },
    GetGrugArray_DBCALL: function (Med) {
        var objData = new Object();
        objData["DrugName"] = Med;
        objData["commandType"] = "Get_Drug_Array";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "MEDICATION");
    },
    DoseSelectChange: function (obj) {
        if ($(obj).val() != "") {
            $("#" + OrderSet_Medications.params.PanelID + " #txtDose").val($("#" + OrderSet_Medications.params.PanelID + " #ddlDoseSelect option:selected").text());
            OrderSet_Medications.DurationChange();
        }
        else {
            $("#" + OrderSet_Medications.params.PanelID + " #txtDose").val("1");
        }
    },
    DoseChange: function (obj) {
        OrderSet_Medications.DurationChange();
    },
    DoseTimingChange: function () {
        OrderSet_Medications.DurationChange();
    },
    DurationChange: function () {

        var Dose = $("#" + OrderSet_Medications.params.PanelID + " #txtDose").val();
        var DoseMath;
        if (Dose != "" && $("#" + OrderSet_Medications.params.PanelID + " #ddlDuration option:selected").text() != "- Select -") {
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

            var Duration = $("#" + OrderSet_Medications.params.PanelID + " #ddlDuration option:selected").text().replace(" days", "");
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
            $("#" + OrderSet_Medications.params.PanelID + " #txtQuantity").val(Math.ceil(DoseMath * (OrderSet_Medications.AddDoseTimingValue(parseInt(DurationActual)))));
        }
        else {
            $("#" + OrderSet_Medications.params.PanelID + " #txtQuantity").val("");
        }
    },
    AddDoseTimingValue: function (Days) {
        var DoseTimingText = $("#" + OrderSet_Medications.params.PanelID + " #ddlDoseTiming option:selected").text();
        if (DoseTimingText != "- Select -") {
            var WhatToDo="";
            var WhichValue="";
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
                var WhichValue=2;
            }
            else if (DoseTimingText == "every 72 hours") {
                WhatToDo = "d";
                var WhichValue = 3;
            }
            else if (DoseTimingText == "every eight hours") {
                WhatToDo = "m";
                var WhichValue=3;
            }
            else if (DoseTimingText == "every eight to twelve hours") {
                WhatToDo = "m";
                var WhichValue=3;
            }
            else if (DoseTimingText == "every four hours") {
                WhatToDo = "m";
                var WhichValue=6;
            }
            else if (DoseTimingText == "every four hours while awake") {
                WhatToDo = "m";
                var WhichValue=5;
            }
            else if (DoseTimingText == "every four to six hours") {
                WhatToDo = "m";
                var WhichValue=6;
            }
            else if (DoseTimingText == "every four to six hours while awake") {
                WhatToDo = "m";
                var WhichValue=5;
            }
            else if (DoseTimingText == "every morning") {
                WhatToDo = "nothing";
            }
            else if (DoseTimingText == "every night") {
                WhatToDo = "nothing";
            }
            else if (DoseTimingText == "every other day") {
                WhatToDo = "d";
                var WhichValue=2;
            }
            else if (DoseTimingText == "every six hours") {
                WhatToDo = "m";
                var WhichValue=4;
            }
            else if (DoseTimingText == "every six to eight hours") {
                WhatToDo = "m";
                var WhichValue = 3.5;
            }
            else if (DoseTimingText == "every three days") {
                WhatToDo = "d";
                var WhichValue=3;
            }
            else if (DoseTimingText == "every three hours") {
                WhatToDo = "m";
                var WhichValue=8;
            }
            else if (DoseTimingText == "every three hours while awake") {
                WhatToDo = "m";
                var WhichValue=6;
            }
            else if (DoseTimingText == "every three months") {
                WhatToDo = "d";
                var WhichValue=91;
            }
            else if (DoseTimingText == "every three to four hours") {
                WhatToDo = "m";
                var WhichValue=8;
            }
            else if (DoseTimingText == "every three to four hours while awake") {
                WhatToDo = "m";
                var WhichValue=6;
            }
            else if (DoseTimingText == "every twelve hours") {
                WhatToDo = "m";
                var WhichValue=2;
            }
            else if (DoseTimingText == "every two hours") {
                WhatToDo = "m";
                var WhichValue=12;
            }
            else if (DoseTimingText == "every two hours while awake") {
                WhatToDo = "m";
                var WhichValue=8;
            }
            else if (DoseTimingText == "every two weeks") {
                WhatToDo = "d";
                var WhichValue=14;
            }
            else if (DoseTimingText == "five times a day") {
                WhatToDo = "m";
                var WhichValue=5;
            }
            else if (DoseTimingText == "four times a day") {
                WhatToDo = "m";
                var WhichValue=4;
            }
            else if (DoseTimingText == "once a day") {
                WhatToDo = "nothing";
            }
            else if (DoseTimingText == "once a month") {
                WhatToDo = "d";
                var WhichValue=30;
            }
            else if (DoseTimingText == "once a week") {
                WhatToDo = "d";
                var WhichValue=7;
            }
            else if (DoseTimingText == "once every two weeks") {
                WhatToDo = "d";
                var WhichValue=14;
            }
            else if (DoseTimingText == "single dose") {
                WhatToDo = "nothing";
            }
            else if (DoseTimingText == "three times a day") {
                WhatToDo = "m";
                var WhichValue=3;
            }
            else if (DoseTimingText == "three times a week") {
                WhatToDo = "m";
                var WhichValue=(3/7);
            }
            else if (DoseTimingText == "twice a day") {
                WhatToDo = "m";
                var WhichValue=2;
            }
            else if (DoseTimingText == "twice a week") {
                WhatToDo = "m";
                var WhichValue = (2 / 7);
            }
            else{
                var WhatToDo="";
                var WhichValue="";
            }
            if (WhatToDo != "" && WhatToDo != "nothing") {
                if (WhatToDo == "m") {
                    return Days * WhichValue;
                }
                else if (WhatToDo == "d") {
                    return Math.ceil(Days / WhichValue);
                }
                else {
                    return Days;
                }
            }
            else {
                return Days;
            }
        }
        else {
            return Days;
        }
    },
    Save: function () {
        var self = $("#" + OrderSet_Medications.params.PanelID + " #frmOrderSetMedications");
        var myJSON = self.getMyJSONByName();
        if (OrderSet_Medications.params.mode == "Add") {

            OrderSet_Medications.SaveMedication_DBCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' + OrderSet_Medications.params.PanelID + " #frmOrderSetMedications").data('serialize', $('#' + OrderSet_Medications.params.PanelID + " #frmOrderSetMedications").serialize());
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_OrderSetDetails.MedicationSearch();
                    OrderSet_Medications.UnLoad();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else if (OrderSet_Medications.params.mode == "Edit") {
            OrderSet_Medications.SaveMedication_DBCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' +OrderSet_Medications.params.PanelID + " #frmOrderSetMedications").data('serialize', $('#' +OrderSet_Medications.params.PanelID + " #frmOrderSetMedications").serialize());
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_OrderSetDetails.MedicationSearch();
                    OrderSet_Medications.UnLoad();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SaveMedication_DBCall: function (MedicationData) {

        var objData = JSON.parse(MedicationData);
        objData.OrdersetId = OrderSet_Medications.params.OrderSetId;
        if (OrderSet_Medications.params.mode == "Add") {
            objData["commandType"] = "SAVE_OS_Medication";
        }
        else if (OrderSet_Medications.params.mode == "Edit") {
            objData["commandType"] = "UPDATE_OS_Medication";
            objData["OS_MedicationId"] = OrderSet_Medications.params.OS_MedicationId;
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "MEDICATION");
    },

    UnLoad: function () {
        var form = '#' + OrderSet_Medications.params.PanelID + " #frmOrderSetMedications";
        utility.UnLoadDialog(form, function () {
            if (OrderSet_Medications.params != null && OrderSet_Medications.params.ParentCtrl && OrderSet_Medications.params.ParentCtrlPanelID) {
                UnloadActionPan(OrderSet_Medications.params.ParentCtrl, "OrderSet_Medications", null, OrderSet_Medications.params.ParentCtrlPanelID);
            }
            else if (OrderSet_Medications.params != null && OrderSet_Medications.params.ParentCtrl) {
                UnloadActionPan(OrderSet_Medications.params.ParentCtrl, "OrderSet_Medications");
            }
            else {
                UnloadActionPan(null, "OrderSet_Medications");
            }
        },
        function () {
            if (OrderSet_Medications.params != null && OrderSet_Medications.params.ParentCtrl && OrderSet_Medications.params.ParentCtrlPanelID) {
                UnloadActionPan(OrderSet_Medications.params.ParentCtrl, "OrderSet_Medications", null, OrderSet_Medications.params.ParentCtrlPanelID);
            }
            else if (OrderSet_Medications.params != null && OrderSet_Medications.params.ParentCtrl) {
                UnloadActionPan(OrderSet_Medications.params.ParentCtrl, "OrderSet_Medications");
            }
            else {
                UnloadActionPan(null, "OrderSet_Medications");
            }
        }
        );
    },
}