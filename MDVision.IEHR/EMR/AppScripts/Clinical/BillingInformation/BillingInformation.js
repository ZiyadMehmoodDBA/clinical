BillingInformation = {
    //Author: Farooq Ahmad
    //Date: 21-07-2016
    //This file will handle all actions performed for Billing Information.
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    FamilyMembers: [],
    ExamDetails: {},
    SelectedSystem: '',
    array: [],
    myArr: [],
    onChangecheckBox: "BillingInformation.onInnercheckBoxChange(this);",
    SuggestionCount: 0,
    parentCtrlGlobel: null,
    mainSelected: null,
    SectionNormalInfo: [],
    selectedcharacteristicsIds: [],
    characteristicsWithData: [],
    selectedsubcharacteristicsIds: [],
    subcharacteristicsWithData: [],
    selectedData: null,
    isNormalTriggred: false,
    isBothUnCheck: false,
    specialityCheckedIds: [],
    providerSelectedIds: [],
    providerCheckedIds: [],
    normalSystemIdsGlobel: [],
    selectedPhyExamTempData: [],
    SpecialtyIds: '',
    ProviderIds: '',
    BillingInfoTime: [],
    TempBillingInfoTime: [],
    VisitInformation: null,
    HideAlert: false,
    AttachtedCPTData: [],
    EMCheckedCheckBoxArray: [],
    CodeSuggestions: new Object(),
    SelectedPatientType: "",
    SuggestedCheckBox: new Object(),
    Status: "",
    AllCheckBoxes: [],
    PreviouslySuggestedCode: "",
    BillingInformation_DBResponse: new Object(),
    EMCodeNewptDesc: "E/M Services New Pt",
    EMCodeESTptDesc: "E/M Services Est Pt",

    Load: function (params) {
        BillingInformation.HideAlert = false;
        BillingInformation.EMCheckedCheckBoxArray = [];
        //BillingInformation.SuggestionCount = 0;
        if (params != null) {

            BillingInformation.params = params;
            if (BillingInformation.params.PanelID != "pnlBillingInformation") {
                BillingInformation.params.PanelID = BillingInformation.params.PanelID + " #pnlBillingInformation";
            }

            if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                BillingInformation.params.PatientId = $('#PatientProfile #hfPatientId').val();

            }
            // else {
            var deffered = $.Deferred();
            $.when(BillingInformation.LoadPatientInfo(BillingInformation.params.PatientId)).then(function () {
                deffered.resolve();
            });
            //}
            if (BillingInformation.params.FromCCM) {
                //$("#pnlBillingInformation #ddlTimeMin").attr("disabled", true);
                //$("#pnlBillingInformation #ddlType").attr("disabled", true);
                // will work after some issue with consultation of QA.
            }

        }


        if (BillingInformation.bIsFirstLoad) {
            var enableValidation = false;
            if (BillingInformation.params.ParentCtrl == "clinicalTabProgressNote") {
                $("#" + BillingInformation.params.PanelID + " #IsNonBilableDiv").removeClass("hidden");
                BillingInformation.SetNonBillable();
            }
            if (BillingInformation.params.ParentCtrl == "schTabCalendar") {
                $('#' + BillingInformation.params.PanelID + " #headingTitle").html("eSuperbill");
            }
            BillingInformation.validateBillingInfo();
            if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                enableValidation = true;
            }
            var formValidation = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation").data("bootstrapValidator");
            formValidation.enableFieldValidators('Provider', enableValidation);
            formValidation.enableFieldValidators('Facility', enableValidation);
            formValidation.enableFieldValidators('DOSFrom', enableValidation);
            formValidation.enableFieldValidators('DOSTo', enableValidation);
            formValidation.enableFieldValidators('ResourceProvider', enableValidation);


            BillingInformation.bIsFirstLoad = false;
            var self = $('#pnlBillingInformation');
            if (globalAppdata["ENMCodesTime"] == "2") {
                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #divEntity").addClass('hidden');
            } else {
                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #divEntity").removeClass('hidden');
            }
            $.when(deffered, self.loadDropDowns(true)).done(function () {

                //Start//Abid Ali//Add/remove hidden class for OOOVisit
                if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                    self.find('#divOutOfOfficeVisit').removeClass('hidden');
                    EMRUtility.ValidateFromToDate('pnlBillingInformation', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "DOS To should be greater than DOS From");
                    EMRUtility.ValidateFromToDate('pnlBillingInformation', 'dpAdmissionDate', 'dpDischargeDate', true, function () { }, function () { }, "Discharge date should be greater than Admission date.");

                    BillingInformation.FillDOS('dpStartDate');
                    BillingInformation.FillDOS('dpToDate');


                }
                else {
                    self.find('#divOutOfOfficeVisit').addClass('hidden');
                }
                //End//Abid Ali//Add/remove hidden class for OOOVisit

                BillingInformation.Get_LookupBillingInfoTime();
                BillingInformation.AddICDAutoComplete();
                BillingInformation.BindFacility();
                BillingInformation.BindProvider();
                BillingInformation.BindResourceProvider();
                BillingInformation.BindRefProvider();

                var PanelChargeGrid = "#" + BillingInformation.params.PanelID + " #pnlVisitCharge_Result";
                var ChargeGridId = "#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge";
                $(ChargeGridId + " tbody tr").remove();

                // Fixed PMS-1701
                // if (BillingInformation.params.EnbtnViewCharges == true) {
                // if(BillingInformation.EditableGrid != null && BillingInformation.EditableGrid != undefined)
                //     BillingInformation.EditableGrid.datatable().fnDestroy();
                //  }

                BillingInformation.EditableGrid = utility.MakeEditableGrid(PanelChargeGrid, ChargeGridId, BillingInformation, "0", false, false, false, false);
                BillingInformation.AddNewChargeRow("Add");
                if (BillingInformation.params.mode == "Add") {
                    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #dpToDate").prop("disabled", false);
                }

            });

        }

        BillingInformation.domReady();
        utility.callbackAfterAllDOMLoaded(function () {
            if (BillingInformation.Status && BillingInformation.Status == "Signed") {
                $("#" + BillingInformation.params.PanelID + " #btnSign").addClass('hidden');
                $("#" + BillingInformation.params.PanelID + " #btnSignBottom").addClass('hidden');
            }
            // Added by Zia fixed
            if (BillingInformation.params.EnbtnViewCharges != null) {
                if (BillingInformation.params.EnbtnViewCharges == true) {
                    $("#" + BillingInformation.params.PanelID + " #btnViewCharges").addClass("hidden");
                    $("#" + BillingInformation.params.PanelID + " #btnViewChargesBottom").addClass("hidden");
                }
            }

            $("#frmBillingInformation").data('serialize', $("#frmBillingInformation").serialize());
        });


        if (BillingInformation.params.ParentCtrl == "clinicalTabProgressNote" || BillingInformation.params.ParentCtrl == "Clinical_NotesView"
            || BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
            Clinical_Notes.checkSignRights(BillingInformation.params.PanelID, 'btnSign', 'btnSignBottom');
        }
        if (BillingInformation.params.ParentCtrl == "schTabCalendar") {
            $("#" + BillingInformation.params.PanelID + " #btnSign").removeClass('hidden');
            $("#" + BillingInformation.params.PanelID + " #btnSignBottom").removeClass('hidden');
        }



        if ((BillingInformation.params.NotesId != "" || BillingInformation.params.NotesId != undefined) && (BillingInformation.params.NoteStatus == "Draft")) {
            if (BillingInformation.params.ParentCtrl == "Clinical_NotesView") {
                $('#pnlBillingInformation #divEvalnMangement,#divICDCPT').addClass("disableAll");
                $('#pnlBillingInformation #btnSign').hide();
                $('#pnlBillingInformation #btnSave').hide();

                $('#pnlBillingInformation #btnSignBottom').hide();
                $('#pnlBillingInformation #btnSaveBottom').hide();
            }
        }
    },
    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to apply bootstrap validations


    validateBillingInfo: function () {
        $('#' + BillingInformation.params.PanelID + " #frmBillingInformation")
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Provider: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       },
                       trigger: 'blur'
                   },
                   Facility: {
                       group: '.col-sm-6',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       },
                       trigger: 'blur'
                   },
                   DOSFrom: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       },
                       trigger: 'blur'
                   },
                   DOSTo: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       },
                       trigger: 'blur'
                   },
                   ResourceProvider: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       },
                       trigger: 'blur'
                   },
               }
           })
    .on('success.form.bv', function (e) {
        e.preventDefault();
        if (e.type == "success") {
            //Start 22-03-2016 Humaira Yousaf for multiple submit buttons
            var $form = $(e.target);
            var $button = $form.data('bootstrapValidator').getSubmitButton();
            switch ($button.attr('id')) {
                case 'btnSave':
                case 'btnSaveBottom':
                    BillingInformation.Save();
                    break;
                case 'btnSign':
                case 'btnSignBottom':
                    BillingInformation.Save(true);
                    break;
                default:
                    BillingInformation.Save();
                    break;
            }
            //End 22-03-2016 Humaira Yousaf for multiple submit buttons
        }
        e.type = "";
    });

    },



    Get_LookupBillingInfoTime: function () {
        var BillingInfoType = [];
        var BillingInfoTypeEMCodes = [];
        BillingInformation.BillingInfoTimeLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                BillingInformation.BillingInfoTime = response.BillingInfoTime = JSON.parse(response.BillingInfoTime);
                for (index = 0; index < BillingInformation.BillingInfoTime.length; index++) {
                    BillingInformation.BillingInfoTime[index].Description = BillingInformation.BillingInfoTime[index].Description.trim();
                }
                $.each(BillingInformation.BillingInfoTime, function (index, item) {
                    var items = $.grep(BillingInfoType, function (e) {
                        return item.TypeDescription == e.TypeDescription;
                    });
                    if (items.length == 0) {
                        BillingInfoTypeEMCodes = $.grep(BillingInformation.BillingInfoTime, function (e) {
                            return item.typeId == e.typeId;
                        });
                        if (BillingInfoTypeEMCodes.length > 0)
                            item.BillingInfoTypeEMCodesArr = BillingInfoTypeEMCodes;
                        else
                            item.BillingInfoTypeEMCodesArr = [];
                        BillingInfoType.push(item);
                    }
                });
                if (globalAppdata["EMCodeTypeIds"] && globalAppdata["EMCodeTypeIds"] != "") {
                    var codeIds = globalAppdata["EMCodeTypeIds"].split(',');
                    if (codeIds.length > 0) {
                        $(BillingInfoType).each(function (index, item) {
                            var id = item.typeId; var found = false;
                            $.each(codeIds, function (i, codeIdsItem) {
                                if (codeIdsItem == id)
                                    found = true;
                            });
                            if (found)
                                BillingInformation.EMCodeGridAppend(item);
                        });
                    }
                    else
                        $(BillingInfoType).each(function (index, item) {
                            BillingInformation.EMCodeGridAppend(item);
                        });
                }
                else
                    $(BillingInfoType).each(function (index, item) {
                        BillingInformation.EMCodeGridAppend(item);
                    });
                $.when(BillingInformation.FillChargePOS('hfFacility', 'hfPOS')).then(function () {
                    BillingInformation.LoadBillingInformation()
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    EMCodeGridAppend: function (item, BillingInfoTime) {
        var $row = $('<tr/>');
        var checkBoxesHtml = "";
        if (item.BillingInfoTypeEMCodesArr && item.BillingInfoTypeEMCodesArr.length > 0) {
            $.each(item.BillingInfoTypeEMCodesArr, function (i, codeItems) {
                checkBoxesHtml += '<div class="pull-left pl-default pr-default"><div class="checkbox-custom" data-toggle="tooltip" title="' + codeItems.ENMCPTDescription + '"><input type="checkbox" id="' + codeItems.BillingInfoTimeId + '"ENMCPTDescription="' + codeItems.ENMCPTDescription + '"Code="' + codeItems.ENMCPT + '"typeid="' + codeItems.typeId + '" value="' + codeItems.BillingInfoTimeId + '" billinginfotype="' + codeItems.TypeDescription + '" onclick="BillingInformation.SelectCheckBox(this,true);" class="input-block"><label class="control-label" for="' + codeItems.BillingInfoTimeId + '">' + codeItems.ENMCPT + '</label></div></div>';
            });
        }
        var ENMIcon = "";
        if (item.TypeDescription.trim() == BillingInformation.EMCodeNewptDesc) {
            ENMIcon = '<span><a onclick="BillingInformation.OpenSuggestEMCodes(1)"  title="E&M Calculator" class="btn btn-xs btn-success pt-none pb-none"><i style="" class="fa fa-calculator "></i></a></span>';
        }
        else if (item.TypeDescription.trim() == BillingInformation.EMCodeESTptDesc) {
            ENMIcon = '<span><a onclick="BillingInformation.OpenSuggestEMCodes(2)" title="E&M Calculator" class="btn btn-xs btn-success pt-none pb-none"><i style="" class="fa fa-calculator "></i></a></span>';
        }

        $row.append('<td>' + item.TypeDescription + ' ' + ENMIcon + '</td><td>' + checkBoxesHtml + '</td>');
        $("#" + BillingInformation.params.PanelID + " #tblEMCodes tbody").last().append($row);
    },

    IsICDExists: function (ICD, value) {
        var breturn = false;
        if (EncounterChargeCapture.ICDArrays) {
            for (var i = 0; i < EncounterChargeCapture.ICDArrays.length; i++) {
                if (EncounterChargeCapture.ICDArrays[i].ICD == ICD && EncounterChargeCapture.ICDArrays[i].value == value) {
                    breturn = true;
                    break;
                }
            }
        }
        return breturn;
    },

    ValidateICDCodeAndSetDesc: function (item) {
        var CurrentCPT = {};
        $(item).parent().parent().find('input').each(function () {
            var hiddenId = $(this).attr("id");
            if (hiddenId.indexOf("hfICDCode9") > -1) {
                CurrentCPT["ICDCode9"] = $(this).val();
            }
            if (hiddenId.indexOf("hfICDCode9") > -1) {
                CurrentCPT["ICDCode9"] = $(this).val();
            }
            if (hiddenId.indexOf("hfICDCode10") > -1) {
                CurrentCPT["ICDCode10"] = $(this).val();
            }
            if (hiddenId.indexOf("hfSNOMEDCode") > -1) {
                CurrentCPT["SNOMEDCode"] = $(this).val();
            }

            if (hiddenId.indexOf("txtDisease") > -1) {
                CurrentCPT["txtDisease"] = hiddenId;
            }
        });

        var Duplicate = false;

        $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").find('.col-sm-3').each(function (i, item) {

            if (CurrentCPT["txtDisease"] != $(item).find('input[type=text]').attr("id")) {
                if (CurrentCPT["ICDCode10"] != '') {
                    if ($(this).find('input[id*="hfICDCode10"]').val() == CurrentCPT["ICDCode10"]) {
                        Duplicate = true;
                    }
                }

            }

        });

        if (Duplicate) {

            setTimeout(function (item) {
                utility.DisplayMessages('ICD already added.', 3);
                $(item).parent().parent().find('input').each(function () {
                    $(this).val('');
                });
            }, 300, item)


        }


    },

    LoadPatientInfo: function (PatientID, caller) {
        var deffered = $.Deferred();

        BillingInformation.FillDemographic(PatientID).done(function (response) {

            if (response.status != false) {
                var NoteInsurancePlanName = '';

                BillingInformation.PatientInfoJSON = JSON.parse(response.DemographicFill_JSON);
                $("#pnlBillingInformation #hfRefProvider").val(BillingInformation.PatientInfoJSON.RefProviderID);
                BillingInformation.params.RefProviderId = $("#pnlBillingInformation #hfRefProvider").val();
                $("#pnlBillingInformation #txtRefProvider").val(BillingInformation.PatientInfoJSON.RefProvider);
                if ((BillingInformation.PatientInfoJSON.InsuranceName != '' && BillingInformation.PatientInfoJSON.InsuranceName.toLowerCase() != 'selfpay') && (BillingInformation.params.NoteStatus != "Signed")) {
                    NoteInsurancePlanName = ' :  ' + BillingInformation.PatientInfoJSON.InsuranceName;
                }
                var patientName = BillingInformation.PatientInfoJSON.FirstName + " " + BillingInformation.PatientInfoJSON.LastName;

                if (BillingInformation.params.VisitId != null) {

                    BillingInformation.FillVisit(BillingInformation.params.PatientId, BillingInformation.params.VisitId).done(function (response) {

                        var visitDetails = JSON.parse(response.VisitFill_JSON);
                        BillingInformation.VisitInformation = visitDetails;

                        BillingInformation.params.AppointmentId = visitDetails.AppointmentId;

                        if (visitDetails.NoteInsurancePlanName && visitDetails.NoteInsurancePlanName != '') {
                            NoteInsurancePlanName = ' :  ' + visitDetails.NoteInsurancePlanName;
                        }

                        if (BillingInformation.params.ParentCtrl == "schTabCalendar" || BillingInformation.params.ParentCtrl == "schTabMultipleView") {

                            BillingInformation.params.AppointmentDate = utility.RemoveTimeFromDate(null, visitDetails.dtpAppointmentDate);
                            BillingInformation.params.VisitDate = utility.RemoveTimeFromDate(null, visitDetails.hfVisitDate);



                            $("#" + BillingInformation.params.PanelID + " #noteDate").text(visitDetails.dtpDOSFrom + ': ' + patientName + NoteInsurancePlanName);
                        }
                        else if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                            BillingInformation.params.AppointmentDate = utility.RemoveTimeFromDate(null, visitDetails.dtpAppointmentDate);
                            BillingInformation.params.VisitDate = utility.RemoveTimeFromDate(null, visitDetails.hfVisitDate);
                            $("#" + BillingInformation.params.PanelID + " #noteDate").text(patientName + NoteInsurancePlanName);

                        }
                        else {
                            $("#" + BillingInformation.params.PanelID + " #noteDate").text(BillingInformation.params.NoteDate + ': ' + patientName + NoteInsurancePlanName);
                        }
                    });
                }
                if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                    var title = '<small id="noteDate" class="sub-heading">' + patientName + NoteInsurancePlanName + '</small>';
                    $("#" + BillingInformation.params.PanelID + " #headingTitle").html("Out of Office Visit | " + title);
                }

            }
            else {
                // utility.DisplayMessages(response.Message, 3);
            }
            deffered.resolve();
        });
        return deffered;
    },


    FillDemographic: function (PatientID) {

        var objData = new Object();
        objData["PatientID"] = PatientID;
        objData["CommandType"] = "fill_patient_demographic";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },

    BillingInfoTimeLoad: function () {
        var objData = new Object();

        objData["commandType"] = "BILLING_INFORMATION_TIME_LOAD";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");

    },


    //Author: Abid Ali
    pushCPTICDCodes: function (CPTs, ICDs) {
        var Object = $('#' + BillingInformation.params.PanelID + ' #tblBillingInformation');

        //Push ICD Codes
        $(ICDs).each(function (index, item) {
            var counter = index + 1;

            var ISExist = false;


            for (var i = 1; i < 13; i++) {
                if (($(Object).find("#hfICDCode9_" + i).val() == item.ICD9 && $(Object).find("#hfICDDescription9_" + i).val() == item.Description)
                    || ($(Object).find("#hfICDCode10_" + i).val() == item.ICD10 && $(Object).find("#hfICDDescription10_" + i).val() == item.Description)) {
                    ISExist = true;
                }


            }

            if (ISExist) {
                utility.DisplayMessages('ICD already added.', 3);
                return true;
            }

            for (var i = 1; i < 13; i++) {
                var arrICD9Codes = $(Object).find("input[type=hidden][id*='hfICDCode9_" + i + "'][value!='']");
                var arrICD10Codes = $(Object).find("input[type=hidden][id*='hfICDCode10_" + i + "'][value!='']");
                if (!(arrICD9Codes.length > 0 || arrICD10Codes.length > 0)) {

                    if ($(Object).find("#hfICDCode9_" + i).length == 0) {
                        BillingInformation.AddMoreDiagnosis();
                    }
                    if (item.ICD9 == '' && item.ICD10 == '') {
                        utility.DisplayMessages('Please select a valid ICD.', 3);
                        return true;
                    }


                    $(Object).find("#hfICDCode9_" + i).val(item.ICD9);
                    $(Object).find("#hfICDDescription9_" + i).val(item.Description);
                    $(Object).find("#hfICDCode10_" + i).val(item.ICD10);
                    $(Object).find("#hfICDDescription10_" + i).val(item.ICD10Description);
                    //$(Object).find("#hfSNOMEDCode_" + counter).val(item.SNOMEDID);
                    $(Object).find("#hfSNOMEDDescription_" + i).val(item.SNOMED_DESCRIPTION);
                    $(Object).find("#txtDisease_" + i).val(item.ICD10 + ' - ' + item.ICD10Description);
                    break;
                }
            }
        });
        //Push CPT Codes
        $(CPTs).each(function (index, item) {
            try {

                var IsDuplicate = false;
                $('input[id^="hfCPTCode"]').each(function () {
                    var thisRow = $(this).closest('tr');
                    if ($(this).val() == item.CPTCode) {
                        IsDuplicate = true;
                    }
                });
                if (IsDuplicate) {
                    utility.DisplayMessages('CPT already added.', 3);
                    return true;
                }


                var LastRow = $("#" + BillingInformation.params.PanelID + " #pnlVisitCharge_Result #dgvBillVisitCharge tbody tr:last");

                var lastRowContainValue = $(LastRow).find("input[id*='hfCPTCode']").val() != null && $(LastRow).find("input[id*='hfCPTCode']").val() != "" && $(LastRow).find("input[id*='hfCPTDescription']").val() != null && $(LastRow).find("input[id*='hfCPTDescription']").val() != "";


                if (item.CPTCode == "") {
                    utility.DisplayMessages('Please select a valid CPT.', 3);
                    return true;
                }
                var counter = $("#" + BillingInformation.params.PanelID + " #pnlVisitCharge_Result #dgvBillVisitCharge tbody tr").length;
                var isRow = false;
                if ($("#" + BillingInformation.params.PanelID + " #pnlVisitCharge_Result #dgvBillVisitCharge tbody tr").length == 1 && $("#" + BillingInformation.params.PanelID + " #pnlVisitCharge_Result #dgvBillVisitCharge tbody tr td").hasClass("dataTables_empty")) {
                    isRow = true;
                    counter = 0;
                }
                if (isRow || lastRowContainValue) {
                    $.when(BillingInformation.AddNewChargeRow(null, 'Add')).then(function () {
                        counter = (counter + 1) * -1;
                        var Object = $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge tr[id='" + counter + "'");
                        if ($(Object).find("#txtCPT" + counter).val() != "") {
                            // this logic is due to tr id -2 is on top and -1 is at bottom so i check for empty cpt textbox row
                            counter = counter * -1;
                            counter = (counter + 1) * -1;
                        }
                        Object = $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge tr[id='" + counter + "'");
                        counter = parseInt(counter) < 0 ? counter * -1 : counter;
                        $(Object).find("#hfCPTCode-" + counter).val(item.CPTCode);
                        $(Object).find("#hfCPTDescription-" + counter).val(item.Description);
                        $(Object).find("#txtCPT-" + counter).val(item.CPTCode + ' - ' + item.Description);
                        $(Object).find("#hfBillingInfoCPTId-" + counter).val(item.BillingInfoCPTId ? item.BillingInfoCPTId : "");
                        utility.SetAutoCompleteSourceforValidate($(Object).find("#txtCPT-" + counter), item.CPTCode, item.Description);
                        $(Object).find("#hfUnitsId-" + counter).val(item.Unit);
                        $(Object).find("#txtUnits-" + counter).val(item.CPTUnit);

                        BillingInformation.EmptyRowsGoDown();
                        BillingInformation.ReorderCPTs();
                    });
                }
                else {
                    var Object = $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge tr:last");
                    var counter = parseInt(Object.attr("id"));
                    counter = parseInt(counter) < 0 ? counter * -1 : counter;
                    $(Object).find("#hfCPTCode-" + counter).val(item.CPTCode);
                    $(Object).find("#hfCPTDescription-" + counter).val(item.Description);
                    $(Object).find("#hfBillingInfoCPTId-" + counter).val(item.BillingInfoCPTId ? item.BillingInfoCPTId : "");
                    $(Object).find("#txtCPT-" + counter).val(item.CPTCode + ' - ' + item.Description);
                    utility.SetAutoCompleteSourceforValidate($(Object).find("#txtCPT-" + counter), item.CPTCode, item.Description);
                    $(Object).find("#hfUnitsId-" + counter).val(item.Unit);
                    $(Object).find("#txtUnits-" + counter).val(item.CPTUnit);
                    $.when(BillingInformation.AddNewChargeRow(null, 'Add')).then(function () {
                        BillingInformation.EmptyRowsGoDown();
                        BillingInformation.ReorderCPTs();
                    });
                }

            } catch (ex) {
                console.log(ex);
            }


        });
        $.when(BillingInformation.AddNewChargeRowWrapper()).then(function () {
            BillingInformation.EnableDisableICDs();
            BillingInformation.Re_CalculateCPTFee();
        });
    },


    AddNewChargeRowWrapper: function () {
        var _def = $.Deferred();
        var found = false;
        $('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').each(function (i, item) {
            if ($(item).find('input[id*="txtCPT"]').val() == "")
                found = true;
        });
        if (!found) {
            $.when(BillingInformation.AddNewChargeRow(null, 'Add')).then(function () {
                _def.resolve();
            });
        }
        else
            _def.resolve();
        return _def;
    },
    AddNewChargeRow: function (RowId, mode, CurrRef, SelectedType, CheckBoxValue) {
        var deffered = $.Deferred();
        //var myRows= BillingInformation.EditableGrid;
        var MasterVisitId = $("#" + BillingInformation.params.PanelID + " #hfMasterVisitId").val();
        // Hide PrimaryFee Column if Visit is Primary
        var ChargeGridId = "#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge";
        if (MasterVisitId == "") {
            $(ChargeGridId + " th#ColumnPrimaryFee").css("display", "none");
        }
        else {
            $(ChargeGridId + " th#ColumnPrimaryFee").css("display", "");
        }
        var CurrentRow = null;
        if (RowId != null && RowId > 0) {
            if (BillingInformation.params.ParentCtrl != null) {
                CurrentRow = BillingInformation.EditableGrid.rowAdd(RowId, "", null, null, null, null, BillingInformation.onChangecheckBox, false);
                deffered.resolve();

            }
            else {
                CurrentRow = BillingInformation.EditableGrid.rowAdd(RowId, BillingInformation.params.VisitId, null, null, null, null, BillingInformation.onChangecheckBox, false);
                deffered.resolve();

            }
        }
        else {
            var TemplateRow = $("#" + BillingInformation.params.PanelID + " #pnlVisitCharge_Result #dgvBillVisitCharge tbody tr[id*=-]").last();
            var minId = 0;
            $("#" + BillingInformation.params.PanelID + " #pnlVisitCharge_Result #dgvBillVisitCharge tbody tr").each(function () {
                var currentId = $(this).attr("id");
                if (minId > parseInt(currentId)) {
                    minId = parseInt(currentId);
                }
            });
            if (minId < 0)
                TemplateRow = $("#" + BillingInformation.params.PanelID + " #pnlVisitCharge_Result #dgvBillVisitCharge tbody tr#" + minId);

            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            //else {
            //    TemplateRowId = -1;
            //}


            if (BillingInformation.params.ParentCtrl != null) {
                CurrentRow = BillingInformation.EditableGrid.rowAdd(TemplateRowId - 1, "", null, null, null, null, BillingInformation.onChangecheckBox, false);
                deffered.resolve();


            }
            else {
                CurrentRow = BillingInformation.EditableGrid.rowAdd(TemplateRowId - 1, BillingInformation.params.VisitId, null, null, null, null, BillingInformation.onChangecheckBox, false);
                deffered.resolve();
            }
        }
        // Hide PrimaryFee Column if Visit is Primary
        if ((SelectedType != null && typeof SelectedType != "undefined") && (CheckBoxValue != null && typeof CheckBoxValue != "undefined")) {
            $(CurrentRow).attr("Type", SelectedType);
            $(CurrentRow).attr("CheckBoxValue", CheckBoxValue);
        }
        if (MasterVisitId == "") {
            $(CurrentRow).find('input[id*="txtPrimaryFEE"]').parent().css("display", "none");
        }
        else {
            $(CurrentRow).find('input[id*="txtPrimaryFEE"]').parent().css("display", "");
        }

        //Start//Abid Ali//For adding DOSFrom DOSTo in CPT Grid
        if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {

            $(ChargeGridId).find('th[id*="thDos"]').removeClass('hidden');
            $(CurrentRow).find('input[id*="dtpDOS"]').closest('td').removeClass('hidden');

            //if (CurrRef != null) {
            //    var dateFromTo = item.DOSFrom + "," + item.DOSTo;

            //    BillingInformation.FillGridDOS($(CurrentRow), dateFromTo);
            //}
            //else {
            if (mode == "Copy") {
                EMRUtility.ValidateFromToDate('pnlBillingInformation', $(CurrentRow).find('input[id*="dtpDOSFrom"]').attr('id'), $(CurrentRow).find('input[id*="dtpDOSTo"]').attr('id'), true, function () { }, function () { }, "DOS To should be greater than DOS From");
                $(CurrentRow).find('input[id*="dtpDOSFrom"]').prop('disabled', false);
                $(CurrentRow).find('input[id*="dtpDOSTo"]').prop('disabled', false);
                $(CurrentRow).attr("mode", "copy");
            }
            else {
                BillingInformation.FillGridDOS($(CurrentRow));
            }
            // }
            // $(CurrentRow).find('input[id*="dtp"]').
            BillingInformation.validateDosFromTo($(CurrentRow).find('input[id*="dtpDOSFrom"]'), $(CurrentRow).find('input[id*="dtpDOSTo"]'));
        }
        else {

            $(ChargeGridId).find('th[id*="thDos"]').addClass('hidden');
            $(CurrentRow).find('input[id*="dtpDOS"]').closest('td').addClass('hidden');
        }
        //End//Abid Ali//For adding DOSFrom DOSTo in CPT Grid

        // Start Copy Previous Row Data to New Row
        if ($(CurrentRow).attr("id") != null && parseInt($(CurrentRow).attr("id")) < 0) {

            var PreviousRow = "";
            //if previous row's child is opened then its a row hence we need to consider the prev row of child which is parent
            if ($(CurrentRow).prev().length > 0 && $(CurrentRow).prev().attr("id").indexOf('Child') > -1) {
                PreviousRow = $("#" + $(CurrentRow).prev().prev().attr("id"));
            } else {
                PreviousRow = $("#" + $(CurrentRow).prev().attr("id"));
            }
        }


        var row = BillingInformation.EditableGrid.datatable.row(CurrentRow);
        row.child(BillingInformation.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        $(CurrentRow).find('input:text[id*="txtTotalFEE"]').attr('disabled', true);

        row.child().loadDropDowns(true).done(function () {
        });

        //internalize date picker

        row.child.hide();
        //$(CurrentRow).find('input[id*="dtpDOSFrom"]').val($('#' + BillingInformation.params.PanelID + " #dtpDOSFrom").val() != "" ? $('#' + BillingInformation.params.PanelID + " #dtpDOSFrom").val() : $('#' + BillingInformation.params.PanelID + " #dtpAppointmentDate").val());
        //$(CurrentRow).find('input[id*="dtpDOSTo"]').val($('#' + BillingInformation.params.PanelID + " #dtpDOSTo").val() != "" ? $('#' + BillingInformation.params.PanelID + " #dtpDOSTo").val() : $('#' + BillingInformation.params.PanelID + " #dtpAppointmentDate").val());
        $(CurrentRow).find('input[id*="txtUnits"]').val(1);

        $(CurrentRow).find('input[id*="txtUnits"]').on('keydown', function (e) {
            -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) &&
            (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode ||
            (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault()
        });
        $(CurrentRow).find('input[id*="txtUnits"]').on("input", function () {
            if (this.value != "") {
                var val = Math.abs(parseInt(this.value, 10) || 1);
                this.value = val > 999 ? 999 : val;
            }
        });
        $(CurrentRow).find('input[id*="txtUnits"]').on("blur", function () {
            var val = Math.abs(parseInt(this.value, 10) || 1);
            this.value = val > 999 ? 999 : val;
        });
        if ($(CurrentRow).prev().length <= 0) {
            if (BillingInformation.IsSelfPay == "False") {
                $(CurrentRow).find('input[id*="txtCOPAY"]').val($('#' + BillingInformation.params.PanelID + " #txtVisitCopayment").val());
            }
            else if (BillingInformation.IsSelfPay == "True") {
                $(CurrentRow).find('input[id*="txtCOPAY"]').val(Number(0).toFixed(globalAppdata.DecimalPlaces));
            }
        }
        $(CurrentRow).find('input[id*="txtFEE"]').val(0);

        $(CurrentRow).find('input[id*="txtFEE"]').on("blur", function () {

            //BillingInformation.ValidateCharges($(CurrentRow).find('input[id*="txtUnit"]').attr("id"), $(CurrentRow).find('input[id*="txtTotalFEE"]').attr("id"), $(CurrentRow).find('input[id*="txtINSCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtPATCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtCOPAY"]').attr("id"), 'fee');
        });
        $(CurrentRow).find('input[id*="txtINSCharges"]').val(0);

        $(CurrentRow).find('input[id*="chkActive"]').wrapAll('<div>')
        $(CurrentRow).find('input[id*="chkActive"]').parent().addClass("pull-left mr-tiny");

        $(CurrentRow).find('input[id*="txtINSCharges"]').on("blur", function () {

            //BillingInformation.ValidateCharges($(CurrentRow).find('input[id*="txtUnit"]').attr("id"), $(CurrentRow).find('input[id*="txtTotalFEE"]').attr("id"), $(CurrentRow).find('input[id*="txtINSCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtPATCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtCOPAY"]').attr("id"), 'insurance');
        });


        $(CurrentRow).find('input[id*="txtPATCharges"]').val(0);
        $(CurrentRow).find('input[id*="txtPATCharges"]').on("blur", function () {

            //BillingInformation.ValidateCharges($(CurrentRow).find('input[id*="txtUnit"]').attr("id"), $(CurrentRow).find('input[id*="txtTotalFEE"]').attr("id"), $(CurrentRow).find('input[id*="txtINSCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtPATCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');
        });
        $(CurrentRow).find('input[id*="txtCOPAY"]').on("blur", function () {

            //BillingInformation.ValidateCharges($(CurrentRow).find('input[id*="txtUnit"]').attr("id"), $(CurrentRow).find('input[id*="txtTotalFEE"]').attr("id"), $(CurrentRow).find('input[id*="txtINSCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtPATCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');
        });



        //if (mode == 'Add')
        //    BillingInformation.FillChargePOS('hfFacility', 'hfPOS', CurrentRow);
        //$(CurrentRow).find('input[id*="txtPOS"]').val($('#hfPOS').val());
        if ($(CurrentRow).find('input[id*="txtUnits"]').val() == "")
            $(CurrentRow).find('input[id*="txtUnits"]').val("1");

        $(CurrentRow).find('input[id*="txtCPT"]').on("blur", function () {
            var CurrentCPT = $(this);
            var duplicateCPT = false;
            if (CurrentCPT.val() != "") {
                duplicateCPT = BillingInformation.CheckDuplicateCPT(CurrentCPT);
            }
            else {
                BillingInformation.FillCurrentCPTFee(currentCPTVal, CurrentRow);
            }
            if (duplicateCPT == true) {

                $(CurrentCPT).val('');
                $(CurrentCPT).focus();
                utility.DisplayMessages('CPT already added.', 3);
                //utility.myConfirm('This code already exists, do you want to add this code again?', function () {
                //    var CurrentCPT = $(CurrentRow).find('input[id*="txtCPT"]');
                //    var newChildRow = BillingInformation.EditableGrid.datatable.row(CurrentRow).child();
                //    var PANDropDown = $(newChildRow).find('datalist[id*="ddlPriorAuthorization"]');
                //    var PANCtrl = $(newChildRow).find('input[id*="txtPriorAuthorization"]');
                //    var PANCtrlHf = $(newChildRow).find('input[id*="hfAuthorizationId"]');
                //    var currentChargeCPT = $(CurrentRow).find('input[id*="hfCurrentChargeCPT"]').val();
                //    var currentCPTVal = $(CurrentCPT).val();
                //    var currentChargeId = $(CurrentRow).attr("id");
                //    utility.ValidateCode($(CurrentCPT), "CPT", null);
                //    $.when(BillingInformation.fillChargeChildRow(currentCPTVal, CurrentRow, newChildRow)).then(function () {
                //        var unitsVal = $(CurrentRow).find('input[id*="txtUnits"]').val();
                //        if (currentChargeCPT != currentCPTVal) {
                //            $(CurrentRow).find('input[id*="hfCurrentChargeCPT"]').val("");
                //            currentChargeCPT = "";
                //        }
                //        if (currentChargeId > 0 && currentChargeCPT == "") {

                //            BillingInformation.FillCurrentCPTFee(currentCPTVal, CurrentRow);
                //        }
                //        else if (currentChargeId < 0) {
                //            BillingInformation.FillCurrentCPTFee(currentCPTVal, CurrentRow);
                //        }
                //        //BillingInformation.LoadPreAuthorizations(CurrentRow, PANDropDown, PANCtrl, PANCtrlHf);
                //    });
                //}, function () {
                //    $(CurrentCPT).val('');
                //    $(CurrentCPT).focus();
                //    return false;
                //}, 'Confirmation duplicate CPT');
            }
            else {
                var newChildRow = BillingInformation.EditableGrid.datatable.row(CurrentRow).child();
                var PANDropDown = $(newChildRow).find('datalist[id*="ddlPriorAuthorization"]');
                var PANCtrl = $(newChildRow).find('input[id*="txtPriorAuthorization"]');
                var PANCtrlHf = $(newChildRow).find('input[id*="hfAuthorizationId"]');
                var currentChargeCPT = $(CurrentRow).find('input[id*="hfCurrentChargeCPT"]').val();
                var currentCPTVal = $(this).val();
                var currentChargeId = $(CurrentRow).attr("id");
                var currentCptBox = $(CurrentRow).find('input[id*="txtCPT"]');

                var hfCPT = $(CurrentRow).find("input[type=hidden][id*='hfCPTCode-']");
                if ($(currentCptBox).val() != "") {
                    utility.ValidateCPTCode($(currentCptBox), $(hfCPT), $(currentCptBox).val());
                }
                $.when(BillingInformation.fillChargeChildRow(currentCPTVal, CurrentRow, newChildRow)).then(function () {
                    var unitsVal = $(CurrentRow).find('input[id*="txtUnits"]').val();
                    if (currentChargeCPT != currentCPTVal) {
                        $(CurrentRow).find('input[id*="hfCurrentChargeCPT"]').val("");
                        currentChargeCPT = "";
                    }
                    if (currentChargeId > 0 && currentChargeCPT == "") {

                        BillingInformation.FillCurrentCPTFee(currentCPTVal, CurrentRow);
                    }
                    else if (currentChargeId < 0) {
                        BillingInformation.FillCurrentCPTFee(currentCPTVal, CurrentRow);
                    }
                    //BillingInformation.LoadPreAuthorizations(CurrentRow, PANDropDown, PANCtrl, PANCtrlHf);
                });
            }
        });
        $(CurrentRow).find('input[id*="txtDxPointer1"]').val('1');
        if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
            var checkDuplicateDOS = function () {
                var thisRow = $(this).closest('tr');
                var CurrentCPT = $(thisRow).find("input[id*=txtCPT]");
                var duplicateCPT = false;
                if (CurrentCPT.val() != "") {
                    duplicateCPT = BillingInformation.CheckDuplicateCPT(CurrentCPT);
                }
                if (duplicateCPT == true) {
                    $(this).val('');
                    utility.DisplayMessages('CPT already added.', 3);
                }
            };
            $(CurrentRow).find('input[id*="dtpDOSFrom"]').on("change", checkDuplicateDOS);
            $(CurrentRow).find('input[id*="dtpDOSTo"]').on("change", checkDuplicateDOS);
        }
        $(CurrentRow).find('input[id*="txtDxPointer"]').on("blur", function () {
            var icdRef = $(this).val();
            var self = null;
            if (BillingInformation.params.PanelID.indexOf("pnlBillingInformation") < 0)
                self = $('#' + BillingInformation.params.PanelID + ' #pnlBillingInformation');
            else
                self = $('#' + BillingInformation.params.PanelID);
            var RefICD = self.find('#hfICDCode10_' + icdRef);
            if ($(RefICD).val() == "") {
                RefICD = self.find('#hfICDCode9_' + icdRef);
            }
            if (RefICD && RefICD.length > 0 && $(this).val() != "" && RefICD.val() != "") {
                if ($(this).prev('input[id*=txtDxPointer]').length > 0 && $(this).prev('input[id*=txtDxPointer]').val() == "") {
                    isValid = false;
                    $(this).val('');
                    utility.DisplayMessages("Please enter previous ICD pointer.", 3);
                }
                else if ($(this).val() !== "") {
                    var selectedPointer = $(this).val();
                    var selectedPointerId = $(this).attr("id");
                    var isPointerExist = false;
                    $.each($(this).closest("tr").find('input[id*="txtDxPointer"]'), function (key, value) {
                        if (selectedPointer == $(this).val() && selectedPointerId != $(this).attr("id")) {
                            isPointerExist = true;
                            utility.DisplayMessages("ICD pointer already exists.", 3);
                        }
                    });

                    if (isPointerExist == true)
                    { $(this).val(''); }

                }
                    //else if ($(this).prev('input[id*=txtDxPointer]').length > 0 && $(this).prev('input[id*=txtDxPointer]').val() == $(this).val()) {
                    //    $(this).val('');
                    //    utility.DisplayMessages("ICD pointer already exists.", 3);
                    //}
                else {
                    if ($(this).val() != "" && icdRef != $(this).val()) {

                    }
                }
            }
            else {
                if ($(this).val() != "") {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid ICD pointer.", 3);
                }
            }
        });
        // We don't need Up/Down for newly added Row as Id is in Minus
        BillingInformation.enableRemoveRow($(CurrentRow));

        //if ($(CurrentRow).find("input[id*=dtpDOSFrom]").attr('id').indexOf('dtpDOSFrom') > -1 || $(CurrentRow).find("input[id*=dtpDOSTo]").attr("id").indexOf('dtpDOSTo') > -1) {
        //    var dtDOSFromId = String($(CurrentRow).find("input[id*=dtpDOSFrom]").attr('id'));
        //    var dtDOSToId = String($(CurrentRow).find("input[id*=dtpDOSTo]").attr("id"));
        //    BillingInformation.ValidateFromToDate(BillingInformation.params["PanelID"] + ' #frmBillingInformation', dtDOSFromId, dtDOSToId, true, function () {

        //        //from date change callback

        //        var DOSTo = $("#" + BillingInformation.params["PanelID"] + ' #frmBillingInformation #dtpDOSTo').datepicker("getDate");
        //        $('#' + dtDOSToId).datepicker('setEndDate', DOSTo);
        //        if ($('#' + dtDOSToId).val() == "") {
        //            $('#' + dtDOSToId).val($('#' + dtDOSFromId).val())
        //        }
        //        BillingInformation.validateDosFromTo('#' + dtDOSFromId, '#' + dtDOSToId);
        //    }, function () {

        //        //to date change callback


        //    });
        //    BillingInformation.validateDosFromTo('#' + dtDOSFromId, '#' + dtDOSToId);
        //}
        if (RowId < 1) {
            BillingInformation.BindSelfPayToBillToPatient();
        }
        if (RowId == null) {
            var ICDPointers = $(CurrentRow).find("input[id*='txtICDPointer1']");
            if (ICDPointers && ICDPointers.length > 0) {
                $(ICDPointers[0]).val('1');
            }
        }
        if (CurrRef != null) {
            var chk = $("#" + BillingInformation.params.PanelID + " #tblEMCodes input[type='checkbox']:checked");
            if (chk.length > 0)
                BillingInformation.SelectCheckBox($(chk));
            BillingInformation.EmptyRowsGoDown();
        }
        return deffered;
    },
    validateDosFromTo: function (objDosFrom, objDosTo) {
        $(objDosFrom).datepicker('setEndDate', new Date());
        $(objDosTo).datepicker('setEndDate', new Date());
    },
    FillDOS: function (ctrlId, dateValue) {

        if (dateValue == null) {
            $('#' + BillingInformation.params.PanelID + ' #' + ctrlId).datepicker("setDate", new Date());
        }
        else {
            $('#' + BillingInformation.params.PanelID + ' #' + ctrlId).datepicker("setDate", dateValue);
            $('#' + BillingInformation.params.PanelID + ' #' + ctrlId).prop('disabled', false);
        }

    },
    FillGridDOS: function ($ctrl, dateValue) {
        if (dateValue == null) {
            $ctrl.find('input[id*="dtpDOS"]').val($.datepicker.formatDate('mm/dd/yy', new Date()));
        }
        else {
            //$ctrl.find('input[id*="dtpDOSFrom"]').val(utility.RemoveTimeFromDate(null,dateValue.split[0]));
            //$ctrl.find('input[id*="dtpDOSTo"]').val(utility.RemoveTimeFromDate(null, dateValue.split[1]));
        }
        EMRUtility.ValidateFromToDate('pnlBillingInformation', $ctrl.find('input[id*="dtpDOSFrom"]').attr('id'), $ctrl.find('input[id*="dtpDOSTo"]').attr('id'), true, function () { }, function () { }, "DOS To should be greater than DOS From");
        $ctrl.find('input[id*="dtpDOSFrom"]').prop('disabled', false);
        $ctrl.find('input[id*="dtpDOSTo"]').prop('disabled', false);
    },

    FillCurrentCPTFee: function (CPTCode, insertedRow) {
        if (CPTCode != null && CPTCode != "") {
            var childRow = BillingInformation.EditableGrid.datatable.row(insertedRow).child();

            var self = null;
            if (BillingInformation.params.PanelID.indexOf("pnlBillingInformation") < 0)
                self = $('#' + BillingInformation.params.PanelID + ' #pnlBillingInformation');
            else
                self = $('#' + BillingInformation.params.PanelID);

            var provider = self.find('#hfProvider').val();
            var facility = self.find('#hfFacility').val();
            var practice = self.find('#hfPractice').val();
            var POSCode = self.find('#txtPOS').val() == "" ? 0 : self.find('#txtPOS').val();


            var patientInsuraceId = 0;
            if ($("#PatientProfile #hfPatientInsuranceId").val())
                patientInsuraceId = $("#PatientProfile #hfPatientInsuranceId").val();

            var Modifier1 = $(insertedRow).find('input[id*="txtModifier1"]').val();
            var Modifier2 = $(insertedRow).find('input[id*="txtModifier2"]').val();
            var Modifier3 = $(insertedRow).find('input[id*="txtModifier3"]').val();
            var Modifier4 = $(insertedRow).find('input[id*="txtModifier4"]').val();
            var ChargeDOS = $(insertedRow).find('input[id*="dtpDOSFrom"]').val();
            if (ChargeDOS == "") {
                ChargeDOS = $.datepicker.formatDate(globalAppdata["DateFormat"].replace('yy', ''), new Date());
            }
            BillingInformation.FillCPTFee(BillingInformation.params.VisitId, CPTCode, provider, facility, practice, patientInsuraceId, POSCode, Modifier1, Modifier2, Modifier3, Modifier4, ChargeDOS).done(function (response) {
                if (response.status != false && response.CPTCount > 0) {
                    var CPTResponse = JSON.parse(response.CPTLoad_JSON);
                    var item = CPTResponse[0];
                    //$(insertedRow).find('input[id*="hfFee"]').val(CPTResponse[0].Fee);

                    var CurrentFee = item.Fee == "" ? 0 : item.Fee;
                    var CurrentExpectedFee = item.ExpectedFee == "" ? 0 : item.ExpectedFee;
                    var fee = parseFloat(CurrentFee);
                    var Expectedfee = parseFloat(CurrentExpectedFee);

                    var units = parseFloat($(insertedRow).find('input:text[id*="txtUnits"]').val());
                    units = !units ? 1 : units;

                    $(insertedRow).find('input[id*="txtFEE"]').val(fee);
                    $(insertedRow).find('input[id*="txtTotalFEE"]').val(fee * units);
                    $(insertedRow).find('input[id*="txtUnits"]').trigger("blur");
                    $(insertedRow).find('input[id*="hfFee"]').val(fee);
                    $(insertedRow).find('input[id*="hfExpectedFee"]').val(Expectedfee);
                    $(insertedRow).find('input[id*="txtTotalFEE"]').val((units * fee).toFixed(2));
                    if (item.Modifier1 && $(insertedRow).find('input[id*="txtModifier1"]').val() == "") {
                        $(insertedRow).find('input[id*="txtModifier1"]').val(item.Modifier1);
                    }
                    if (item.Modifier2 && $(insertedRow).find('input[id*="txtModifier2"]').val() == "") {
                        $(insertedRow).find('input[id*="txtModifier2"]').val(item.Modifier2);
                    }
                    if (item.Modifier3 && $(insertedRow).find('input[id*="txtModifier3"]').val() == "") {
                        $(insertedRow).find('input[id*="txtModifier3"]').val(item.Modifier3);
                    }
                    if (item.Modifier4 && $(insertedRow).find('input[id*="txtModifier4"]').val() == "") {
                        $(insertedRow).find('input[id*="txtModifier4"]').val(item.Modifier4);
                    }
                }
                else {
                    $(insertedRow).find('input[id*="hfFee"]').val(0);
                    $(insertedRow).find('input[id*="txtFEE"]').val(0);
                    $(insertedRow).find('input[id*="txtTotalFEE"]').val(0);
                    $(insertedRow).find('input[id*="hfExpectedFee"]').val(0);
                    $(childRow).find('input[id*="txtExpectedFee"]').val(0);
                    //BillingInformation.ValidateCharges($(insertedRow).find('input[id*="txtUnit"]').attr('id'), $(insertedRow).find('input[id*="txtTotalFEE"]').attr('id'), $(insertedRow).find('input[id*="txtINSCharges"]').attr('id'), $(insertedRow).find('input[id*="txtPATCharges"]').attr('id'), $(insertedRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');
                }
            });
        }
        else {
            $(insertedRow).find('input[id*="hfFee"]').val(0);
            $(insertedRow).find('input[id*="txtFEE"]').val(0);
            $(insertedRow).find('input[id*="txtTotalFEE"]').val(0);
            $(insertedRow).find('input[id*="hfExpectedFee"]').val(0);
            $(childRow).find('input[id*="txtExpectedFee"]').val(0);
            //  $(childRow).find('input[id*="txtUnit"]').val(1);
            //BillingInformation.ValidateCharges($(insertedRow).find('input[id*="txtUnit"]').attr('id'), $(insertedRow).find('input[id*="txtTotalFEE"]').attr('id'), $(insertedRow).find('input[id*="txtINSCharges"]').attr('id'), $(insertedRow).find('input[id*="txtPATCharges"]').attr('id'), $(insertedRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');
        }
    },


    validateDosFromTo: function (objDosFrom, objDosTo) {
        $(objDosFrom).datepicker('setEndDate', new Date());
        $(objDosTo).datepicker('setEndDate', new Date());

    },

    BindSelfPayToBillToPatient: function () {
        var self = null;
        if (BillingInformation.params.PanelID.indexOf("pnlBillingInformation") < 0) {
            self = $('#' + BillingInformation.params.PanelID + " #pnlBillingInformation");
        }
        else {
            self = $('#' + BillingInformation.params.PanelID);
        }
        if (BillingInformation.IsSelfPay == "True") {
            self.find('#chkBillToPatient').prop('checked', true);
            if (self.find('input[id^="txtINSCharges"]')) {
                self.find('input[id^="txtINSCharges"]').prop('disabled', true);
            }
            self.find("#dgvPatientInsurances").find("input[type='checkbox']").prop('checked', false);
            self.find("#dgvPatientInsurances").find("input[type='checkbox']").prop('disabled', true);
            self.find('#chkBillToPatient').prop('disabled', true);
            self.find('input[id^="txtCOPAY"]').prop('disabled', true);
        }
        else {
            self.find("#dgvPatientInsurances").find("input[type='checkbox']").prop('disabled', false);
            if (self.find("#chkBillToPatientForIns").is(':checked')) {
                self.find('#chkBillToPatient').prop('checked', true);
            }
            else {
                self.find('#chkBillToPatient').prop('checked', false);
            }
            self.find('input[id^="txtCOPAY"]').prop('disabled', false);
            self.find('input[id^="txtINSCharges"]').prop('disabled', false);

        }

        //Lock Claim Fields
        BillingInformation.LockClaim();
        //BillingInformation.SelectPrimaryInsurance();
        //BillingInformation.FillReferralNumber();
    },

    LockClaim: function (isLocked) {

        if ((isLocked == null || isLocked == undefined || isLocked == "") && BillingInformation.params["IsLocked"] != "")
            isLocked = BillingInformation.params.IsLocked;

        $("#" + BillingInformation.params.PanelID + " #frmBillingInformation").find("input:text,input:checkbox,input:radio,select,button,textarea").prop('disabled', isLocked);

        $("#" + BillingInformation.params["PanelID"] + " #frmBillingInformation #dgvBillVisitCharge tr").each(function (i, row) {
            if ($(this).attr("id") && $(this).attr("id") != null) {
                if ($(this).attr('chargestatus') && $(this).attr('chargestatus').toLowerCase() == 'submitted') {
                    var childRow = BillingInformation.EditableGrid.datatable.row(this).child();
                    if (this)
                        $(this).find('a:eq(1),a:eq(2),a:eq(4),a:eq(5),input, textarea,  select,button').prop('disabled', true);
                    if (childRow)
                        $(childRow).find('input, textarea,select,button').prop('disabled', true);
                }
                else {
                    var childRow = BillingInformation.EditableGrid.datatable.row(this).child();
                    if (this) {
                        $(this).find('a:eq(1),a:eq(2),a:eq(4),a:eq(5),input, textarea,  select,button').prop('disabled', isLocked);
                    }
                    if (childRow) {
                        $(childRow).find('input, textarea,select,button').prop('disabled', isLocked);
                    }
                    //Begin Changes done by Azeem Raza Tayyab on 5-May-2016 related to Mailed Changes
                    if (isLocked && BillingInformation.IsVNC == null) {
                        //Enable ICDCode and Modifier for locked charge when charge status is not submitted
                        $(this).find('input[id*="txtICD"]').prop('disabled', false);
                        $(this).find('input[id*="txtICD"]').next().find('.btn').prop('disabled', false);
                        $(this).find('input[id*="txtModifier"]').prop('disabled', false);
                        $(this).find('input[id*="txtModifier"]').next().find('.btn').prop('disabled', false);
                        $(this).find('a:eq(1)').prop('disabled', false);
                    }
                    //End Changes done by Azeem Raza Tayyab on 5-May-2016 related to Mailed Changes
                }
            }
        });

        //disabling add new item link
        $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #pnlVisitCharge_Result a").filter(function () {
            if ($(this).text().toLowerCase() === 'add line item') {
                if (isLocked)
                    $(this).addClass("disableAll")
                else
                    $(this).removeClass("disableAll")
            }

        });

        //allow change submit status
        $("#" + BillingInformation.params.PanelID + " #frmBillingInformation").find("#ddlSubmitStatus").prop('disabled', false);
        $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #SaveVisit").prop('disabled', false);

        if (isLocked == false) {
            $("#" + BillingInformation.params.PanelID + " #frmBillingInformation").find(
                "#txtPatientFullName,#txtClaimNumber,#dtpAppointmentDate,#dtpEncounterSignOffDate,#dtpSubmittedDate,#txtSubmittedBy,#chkPaid,#dtpClaimDate"
                ).prop('disabled', true);

            $('#' + BillingInformation.params["PanelID"] + ' input[id*="txtTotalFEE"]').prop('disabled', true);
        }
        //Enable Claim Document close button
        $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #CloseCD").prop('disabled', false);

        //Begin Changes done by Azeem Raza Tayyab on 5-May-2016 related to Mailed Changes
        var self = null;
        if (BillingInformation.params.PanelID.indexOf("pnlBillingInformation") < 0)
            self = $('#' + BillingInformation.params.PanelID + ' #pnlBillingInformation');
        else
            self = $('#' + BillingInformation.params.PanelID);
        if (isLocked && BillingInformation.IsVNC == null) {
            //Enable Referring Provider
            self.find("#frmBillingInformation #txtRefProvider").prop('disabled', false);
            self.find("#frmBillingInformation #lnkRefProvider").prop('disabled', false);
            //Enabale claim CommentsdtpLastSeenDate
            self.find("#frmBillingInformation #txtClaimComments").prop('disabled', false);
            self.find("#frmBillingInformation #dtpLastSeenDate").prop('disabled', false);
        }
        //End Changes done by Azeem Raza Tayyab on 5-May-2016 related to Mailed Changes
        if (isLocked == false) {
            self.find("#txtICD1").prop("disabled", false);
            self.find("#btnICD1").prop("disabled", false);
            BillingInformation.EnableDisableICDs();
        }
        else if (isLocked == true) {
            self.find("input[id^=txtICD]").prop("disabled", true);
            self.find("button[id^=btnICD]").prop("disabled", true);
        }
        self.find("input[id^=txtICD10Description]").prop("disabled", true);
    },

    //START Charge Editable Grid Code
    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div></div>");
        var PriorAuthorization = "<div class='col-xs-2'><label class='control-label'>Prior Auth. Number</label> <input class='form-control' type='text' list='ddlPriorAuthorization" + ParentRowId + "'  maxlength='35' name='PriorAuthorization" + ParentRowId + "' id='txtPriorAuthorization" + ParentRowId + "'/><input type='hidden' id='hfAuthorizationId" + ParentRowId + "'/> <datalist id='ddlPriorAuthorization" + ParentRowId + "' name='PriorAuthorization" + ParentRowId + "><option  value='- SELECT -'></option></datalist></div>";
        var expectedFee = "<div class='col-xs-1'><label class='control-label'>Expected Fee</label><input  class='form-control'type='text' id='txtExpectedFee" + ParentRowId + "' name='txtExpectedFee" + ParentRowId + "' disabled/><input type='hidden' id='hfExpectedFee" + ParentRowId + "'/></div>";
        var txtNDC = "<div class='col-xs-1 size-max90'><label class='control-label'>NDC</label><input maxlength='11' class='form-control' id='txtNDC" + ParentRowId + "' onblur='BillingInformation.SetValidation(this," + ParentRowId + ")' name='txtNDC" + ParentRowId + "' type='text' /></div>";
        var NDCUnit = "<div class='col-xs-1 size-max90'><label class='control-label'>NDC Unit</label><input class='form-control' id='txtNDCUnit" + ParentRowId + "' name='NDCUnit" + ParentRowId + "' type='text' onkeypress='utility.ValidateDecimal(event, 2);' /></div>";
        var NDCUnitPrice = "<div class='col-xs-1  size-min100'><label class='control-label'>NDC Unit Price</label><input class='form-control' id='txtNDCUnitPrice" + ParentRowId + "' name='txtNDCUnitPrice" + ParentRowId + "' type='text' onkeypress='utility.ValidateDecimal(event, 2);' /></div>";
        var ddlNDCMeasurement = "<div class='col-xs-2'><label class='control-label'>NDC Measurement Code</label><select id='ddlNDCMeasurement" + ParentRowId + "' name='ddlNDCMeasurement" + ParentRowId + "' class='form-control' ddlist='GetNDCMeasurementCode'></select></div>";
        var LineNotes = "<div class='col-xs-2'><label class='control-label'>Line Notes</label><textarea class='form-control' spellcheck='true' rows='1' id='txtComments" + ParentRowId + "' maxlength='100' name='txtComments" + ParentRowId + "'></textarea></div>";
        var CLIA = "<div id='divCLIA" + ParentRowId + "' ><div class='col-xs-1 size-max120'><label class='control-label'>CLIA</label><input type='text' class='form-control size100'  id='txtCLIA" + ParentRowId + "' maxlength='10' name='CLIA" + ParentRowId + "''/></div></div>";
        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(PriorAuthorization, expectedFee, txtNDC, NDCUnit, NDCUnitPrice, ddlNDCMeasurement, LineNotes, CLIA, spacer);
        return ChildHTML;

    },

    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
        if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
            CurrentRow.find("td.actions .copy-row").removeClass("hidden");
        }
    },

    ValidateFromToDate: function (FormId, CtrlFromDateId, CtrlToDateId, IsOptional, onFromDateChangeCallback, onToDateChangeCallback) {

        var CtrlForm = "#" + FormId;
        var CtrlFromDate = CtrlForm + " #" + CtrlFromDateId;
        var CtrlToDate = CtrlForm + " #" + CtrlToDateId;
        var CtrFromDateName = $(CtrlToDate).attr("name");
        var CtrToDateName = $(CtrlToDate).attr("name");

        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];

        $(CtrlToDate).attr('disabled', true);
        $(CtrlToDate).attr('maxlength', '10');
        $(CtrlFromDate).attr('maxlength', '10');
        $(CtrlFromDate).datepicker({
            todayHighlight: true,
            format: date_format,
            todayBtn: 'linked',
        }).change(function (e) {
            if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                var fromDate = new Date($(CtrlFromDate).val());
                var toDate = new Date($(CtrlToDate).val());

                if (fromDate <= toDate && fromDate != '') {
                    $(CtrlToDate).val($(CtrlToDate).val()).datepicker('update');
                } else {
                    $(this).val('');
                }
            }
            $(CtrlToDate).attr('disabled', false);
            if (!IsOptional) {
                if ($('#' + BillingInformation.params["PanelID"] + ' #frmBillingInformation').data('bootstrapValidator') != null && typeof $('#' + BillingInformation.params["PanelID"] + ' #frmBillingInformation').data('bootstrapValidator') != 'undefined') {
                    $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);
                }

            }
            $(this).datepicker('hide');
            if (!IsOptional) {
                if ($('#' + BillingInformation.params["PanelID"] + ' #frmBillingInformation').data('bootstrapValidator') != null && typeof $('#' + BillingInformation.params["PanelID"] + ' #frmBillingInformation').data('bootstrapValidator') != 'undefined') {
                    $(CtrlForm).bootstrapValidator('revalidateField', CtrFromDateName);
                }
            }

            var inputDate = $(CtrlFromDate).datepicker('getDate');
            var date_format = 'dd/mm/yyyy';
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }
            if (onFromDateChangeCallback != null && typeof (onFromDateChangeCallback) == 'function') {
                setTimeout(onFromDateChangeCallback, 50);
            }

        }).on('keypress', function (e) {
            utility.preventAlphabetsInDatePicker(e);
        });

        $(CtrlToDate).datepicker({
            todayHighlight: true,
            format: date_format,
        }).change(function (e) {

            $(this).datepicker('hide');
            if (!IsOptional) {
                if ($('#' + BillingInformation.params["PanelID"] + ' #frmBillingInformation').data('bootstrapValidator') != null && typeof $('#' + BillingInformation.params["PanelID"] + ' #frmBillingInformation').data('bootstrapValidator') != 'undefined') {
                    $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);
                }
            }
            if (onToDateChangeCallback != null && typeof (onToDateChangeCallback) == 'function') {
                setTimeout(onToDateChangeCallback, 50);
            }
            var CurrentDatepicker = this;
            setTimeout(function () {
                if ($(CurrentDatepicker).val().length == date_format.length) {
                    if (!utility.isValidDate($(CurrentDatepicker).val())) {
                        $(CurrentDatepicker).val('');
                        utility.DisplayMessages("Please enter valid date", 3);
                    }
                    if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                        var fromDate = new Date($(CtrlFromDate).val());
                        var toDate = new Date($(CtrlToDate).val());
                        if (fromDate > toDate) {
                            $(CurrentDatepicker).val('');
                        }
                    }
                }
            }, 50);
        }).on('keypress', function (e) {
            utility.preventAlphabetsInDatePicker(e);
        });

        $(CtrlFromDate).on('blur', function (e) {
            setTimeout(
               function () {
                   if ($(CtrlFromDate).val() != '') {
                       utility.ValidateDate(CtrlFromDate);
                   }

               }, 100);
        });
        $(CtrlToDate).on('blur', function (e) {
            setTimeout(function () {
                if ($(CtrlToDate).val() != '') {
                    utility.ValidateDate(CtrlToDate);
                }
            }, 100);
        });
    },

    AddICDAutoComplete: function () {

        $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").html('');

        for (var counter = 1; counter < 2 ; counter++) {

            var Html = '<div class="col-sm-3 col-md-3"><label class="control-label">Diagnosis ' + counter + '</label><div class="input-group"><input class="form-control" id="txtDisease_' + counter + '" oninput="BillingInformation.EnableDisableICDs();BillingInformation.bindICD9AutoComplete(this);" onblur="BillingInformation.ValidateICDCodeAndSetDesc(this);" onkeypress="    BillingInformation.disableEnterKey(event)" name="Disease_' + counter + '" type="text" value=""><div class="input-group-btn"><button id="btnDisease_' + counter + '" class="btn btn-primary btn-xs" type="button" onclick="BillingInformation.OpenSearchPopup(\'ICD\', \'txtDisease_' + counter + '\', \'hfICDCode9_' + counter + ',hfICDDescription9_' + counter + ',hfICDCode10_' + counter + ',hfICDDescription10_' + counter + ',hfSNOMEDCode_' + counter + ',hfSNOMEDDescription_' + counter + ',txtDisease_' + counter + '\')"><i class="glyphicon glyphicon-search"></i></button></div></div><input type="hidden" id="hfICDCode9_' + counter + '" name="ICDCode9_' + counter + '" /><input type="hidden" id="hfICDDescription9_' + counter + '" name="ICDDescription9_' + counter + '" /><input type="hidden" id="hfICDCode10_' + counter + '" name="ICDCode10_' + counter + '" /><input type="hidden" id="hfICDDescription10_' + counter + '" name="ICDDescription10_' + counter + '" /><input type="hidden" id="hfSNOMEDCode_' + counter + '" name="hfSNOMEDCode_' + counter + '" /><input type="hidden" id="hfSNOMEDDescription_' + counter + '" name="SNOMEDDescription_' + counter + '" /><input type="hidden" id="hfCustomFormId_' + counter + '" name="CustomFormId_' + counter + '" /></div>';

            $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").append(Html);

        }

        $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").append('<div class="clearfix spacer5"></div><div class="pull-left pl-default"><a class="btn btn-link btn-xs p-none" onclick="BillingInformation.AddMoreDiagnosis();">Add More</a></div>');
        BillingInformation.EnableDisableICDs();
        //BillingInformation.params.NotesId

    },

    fillChargeChildRow: function (CPTCode, insertedRow, childRow) {
        if (CPTCode != null && CPTCode != "") {

            var objDeffered = $.Deferred();

            var j = { "txtShortName": "", "txtCPTCode": String(CPTCode), "txtDiscription": "", "lstTOSId": "", "lstSpeciality": "", "chkDiscontinued": true, "ddlEntity": "" };
            var myJSON = JSON.stringify(j);
            Admin_CPTCode.SearchCPTCode(myJSON, 0, 1, 15).done(function (response) {
                if (response.status != false) {
                    if (response.CPTCount > 0) {
                        var CPTDetails = JSON.parse(response.CPTLoad_JSON)[0];


                        $(insertedRow).find('input[id*="txtUnits"]').val(CPTDetails.BasicUnits);
                        $(insertedRow).find('input[id*="txtTotalFEE"]').val((parseFloat(CPTDetails.BasicUnits) * parseFloat($(insertedRow).find('input[id*="txtFEE"]').val())).toFixed(2));
                        $(insertedRow).find('input[id*="txtExpectedFEE"]').val((parseFloat(CPTDetails.BasicUnits) * parseFloat($(insertedRow).find('input[id*="hfExpectedFEE"]').val())).toFixed(2));

                        var units = parseFloat(CPTDetails.BasicUnits);
                        var fee = parseFloat(CPTDetails.Fee);

                        $(childRow).find('input[id*="txtNDC"]').val(CPTDetails.NDC);
                        $(childRow).find('input[id*="txtNDCUnit"]').val(CPTDetails.NDCUnit);
                        $(childRow).find('input[id*="txtNDCUnitPrice"]').val(CPTDetails.NDCUnitPrice);
                        $(childRow).find('select[id*="ddlNDCMeasurement"]').val(CPTDetails.NDCMeasurementId).attr("selected", "selected");

                        objDeffered.resolve().then(function () {

                            //BillingInformation.ValidateCharges($(insertedRow).find('input[id*="txtUnit"]').attr('id'), $(insertedRow).find('input[id*="txtTotalFEE"]').attr('id'), $(insertedRow).find('input[id*="txtINSCharges"]').attr('id'), $(insertedRow).find('input[id*="txtPATCharges"]').attr('id'), $(insertedRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');
                            return objDeffered;

                        });


                    }
                    else {
                        //// If CPT not found in DB
                        if ($(insertedRow).find('input[id*="txtFEE"]').val() == "" || $(insertedRow).find('input[id*="txtFEE"]').val() == 0) {
                            $(insertedRow).find('input[id*="txtUnits"]').val(1);
                            $(insertedRow).find('input[id*="txtFEE"]').val(0)
                            $(insertedRow).find('input[id*="txtTotalFEE"]').val((parseFloat(1) * parseFloat(0)).toFixed(2));
                        }
                        $(childRow).find('input[id*="txtNDC"]').val("");
                        $(childRow).find('input[id*="txtNDCUnit"]').val("");
                        $(childRow).find('input[id*="txtNDCUnitPrice"]').val("");
                        $(childRow).find('select[id*="ddlNDCMeasurement"]').val("").attr("selected", "selected");

                        objDeffered.resolve().then(function () {
                            //BillingInformation.ValidateCharges($(insertedRow).find('input[id*="txtUnit"]').attr('id'), $(insertedRow).find('input[id*="txtTotalFEE"]').attr('id'), $(insertedRow).find('input[id*="txtINSCharges"]').attr('id'), $(insertedRow).find('input[id*="txtPATCharges"]').attr('id'), $(insertedRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');
                            return objDeffered;
                        });
                    }
                }
                else {
                    objDeffered.resolve();
                    return objDeffered;
                }
            });
        }

    },

    //Author:Farooq Ahmad
    //Date: 21-07-2016
    //Logic implimented to bind ICD code of BillingInformation
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);

        var hiddenFields = $(element).attr('id') + ',';
        $(element).parent().parent().find('input[type="hidden"]').each(function () {
            hiddenFields += $(this).attr('id') + ',';
        });

        if (hiddenFields.length > 0) {
            hiddenFields = hiddenFields.substring(0, hiddenFields.length - 1);
        }

        try {
            if ($(element).val() == "") {
                $("#" + BillingInformation.params.PanelID + " #" + hiddenFields.replace(/,/g, ",#")).val('');
            }
        } catch (ex) {
            console.log(ex);
        }



        BillingInformation.params.RefHiddenCtrl = hiddenFields;

        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "BillingInformation", hiddenFields, false);
    },

    //Author:Farooq Ahmad
    //Date: 21-07-2016
    //Description: To prevent submition of form on Enter Press
    disableEnterKey: function (e) {

        if (e.which == 13) // Enter key = keycode 13
        {
            e.preventDefault();
        }
    },

    ///Author: Farooq Ahmad
    //Date: 21-07-2016
    //Logic to popup search from on procedure field of Billing disease
    OpenSearchPopup: function (searchType, ctrl, hiddenCtrl) {
        var controlToLoad = "";
        if (searchType == "ICD") {
            controlToLoad = "Admin_IMOICD";
        }
        else if (searchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (searchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }

        var params = [];
        params["fromIcon"] = "true";
        params["FromAdmin"] = "0";

        if (BillingInformation.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
        }

        params["ParentCtrl"] = 'BillingInformation';

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (typeof Clinical_SurgicalHx != "undefined" && Clinical_SurgicalHx != null && Clinical_SurgicalHx.params != null && Clinical_SurgicalHx.params.TabID && Clinical_SurgicalHx.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params, BillingInformation.params.PanelID);
        }

    },

    ddlType_Change: function (ddl, EMRTimeId) {
        if (BillingInformation.BillingInfoTime.length == 0) {
            BillingInformation.BillingInfoTimeLoad().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    BillingInformation.BillingInfoTime = response.BillingInfoTime = JSON.parse(response.BillingInfoTime);
                    for (index = 0; index < BillingInformation.BillingInfoTime.length; index++) {
                        BillingInformation.BillingInfoTime[index].Description = BillingInformation.BillingInfoTime[index].Description.trim();
                    }
                    BillingInformation.BindCheckBoxesForTime(ddl, EMRTimeId);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            BillingInformation.BindCheckBoxesForTime(ddl, EMRTimeId);
        }
    },

    BindCheckBoxesForTime: function (ddl, EMRTimeId) {
        var SelectedType = $(ddl).find("option:selected").val();
        BillingInformation.SelectedPatientType = SelectedType;
        //Start Ahson Code

        if (BillingInformation.params.ParentCtrl == "clinicalTabProgressNote" || BillingInformation.params.ParentCtrl == 'schTabCalendar') {
            if (SelectedType == "1" || SelectedType == "2") {
                //$('#txtSuggestEM').show();
                if (!BillingInformation.params.FromCCM) {
                    if (BillingInformation.Status != "Signed") {
                        if (EMRTimeId == undefined) {
                            BillingInformation.GetEMCodeSuggestions().done(
                                function (response) {
                                    response = JSON.parse(response);
                                    if (response.status == true) {
                                        BillingInformation.CodeSuggestions = response;
                                    }
                                    else {
                                        utility.DisplayMessages(response.message, 2);
                                    }
                                });
                        }
                    }
                }
            }
            else {
                //$('#txtSuggestEM').hide();
            }
        }


        $("#" + BillingInformation.params.PanelID + " #ddlTimeMin").html("");

        $("#" + BillingInformation.params.PanelID + " #ddlTimeMin").append('<option value="" refvalue="" refname="">- Select -</option>');
        //$("#" + BillingInformation.params.PanelID + " #divCheckBoxes").html('');


        BillingInformation.TempBillingInfoTime = $.grep(BillingInformation.BillingInfoTime, function (v) {
            return v.typeId == SelectedType;

        });
        for (index = 0; index < BillingInformation.TempBillingInfoTime.length; index++) {
            if (BillingInformation.TempBillingInfoTime[index].Description !== "0") {
                $("#" + BillingInformation.params.PanelID + " #ddlTimeMin").append('<option value="' + BillingInformation.TempBillingInfoTime[index].BillingInfoTimeId + '" refvalue="" refname="">' + BillingInformation.TempBillingInfoTime[index].Description.trim() + '</option>');
                $("#" + BillingInformation.params.PanelID + " #divCheckBoxes").append("<div class='col-sm-2 col-lg-1'><div class='checkbox-custom' data-toggle='tooltip' title='" + BillingInformation.TempBillingInfoTime[index].ENMCPTDescription + "'><input type='checkbox' onclick='BillingInformation.SelectCheckBox(this,true)' id='" + BillingInformation.TempBillingInfoTime[index].BillingInfoTimeId + "' Code='" + BillingInformation.TempBillingInfoTime[index].ENMCPT + "' value='" + BillingInformation.TempBillingInfoTime[index].BillingInfoTimeId + "'/><label class='control-label'>" + BillingInformation.TempBillingInfoTime[index].ENMCPT + "</label></div></div>");
            }
            else {
                $("#" + BillingInformation.params.PanelID + " #divCheckBoxes").append("<div class='col-sm-2 col-lg-1'><div class='checkbox-custom' data-toggle='tooltip' title='" + BillingInformation.TempBillingInfoTime[index].ENMCPTDescription + "'><input type='checkbox' onclick='BillingInformation.SelectCheckBox(this,true)' id='" + BillingInformation.TempBillingInfoTime[index].BillingInfoTimeId + "' Code='" + BillingInformation.TempBillingInfoTime[index].ENMCPT + "' value='" + BillingInformation.TempBillingInfoTime[index].BillingInfoTimeId + "'/><label class='control-label'>" + BillingInformation.TempBillingInfoTime[index].ENMCPT + "</label></div></div>");
            }

            if ((BillingInformation.params.ParentCtrl == "clinicalTabProgressNote" || BillingInformation.params.ParentCtrl == 'schTabCalendar') && BillingInformation.Status != "Signed") {
                if (BillingInformation.CodeSuggestions.SuggestedCode == BillingInformation.TempBillingInfoTime[index].ENMCPT) {
                    BillingInformation.SuggestedCheckBox = $("#" + BillingInformation.params.PanelID + ' #' + BillingInformation.TempBillingInfoTime[index].BillingInfoTimeId);
                }
            }

        }

        if ((BillingInformation.params.ParentCtrl == "clinicalTabProgressNote" || BillingInformation.params.ParentCtrl == 'schTabCalendar') && BillingInformation.Status != "Signed") {
            if (BillingInformation.SuggestedCheckBox != null && BillingInformation.SuggestedCheckBox.length > 0) {
                if (!BillingInformation.params.FromCCM) {

                    if (EMRTimeId == undefined) {
                        //BillingInformation.SuggestedCheckBox.prop('checked', true);
                        //BillingInformation.SelectCheckBox(BillingInformation.SuggestedCheckBox[0]);

                    }

                }
            }
        }


        // Commented By faizan
        //for (var index in BillingInformation.BillingInfoTime) {
        //    var add = false;
        //    if (SelectedType == '1' && BillingInformation.BillingInfoTime[index].Type == 'New')
        //        add = true;
        //    else if (SelectedType == '2' && BillingInformation.BillingInfoTime[index].Type == 'Established')
        //        add = true;
        //    if (add) {
        //        $("#" + BillingInformation.params.PanelID + " #ddlTimeMin").append('<option value="' + BillingInformation.BillingInfoTime[index].Description.trim() + '" refvalue="" refname="">' + BillingInformation.BillingInfoTime[index].Description.trim() + '</option>');
        //        //$("#" + BillingInformation.params.PanelID + " #divCheckBoxes").append("<input type='checkbox' oncheck='BillingInformation.SelectCheckBox(this)' id='" + BillingInformation.BillingInfoTime[index].Description + "' value='" + BillingInformation.BillingInfoTime[index].Description + "'/>" + BillingInformation.BillingInfoTime[index].ENMCPT);
        //        $("#" + BillingInformation.params.PanelID + " #divCheckBoxes").append("<div class='col-sm-2'><div class='checkbox-custom'><input type='checkbox' onclick='BillingInformation.SelectCheckBox(this)' id='" + BillingInformation.BillingInfoTime[index].Description.trim() + "' value='" + BillingInformation.BillingInfoTime[index].Description.trim() + "'/><label class='control-label'>" + BillingInformation.BillingInfoTime[index].ENMCPT + "</label></div></div>");
        //    }
        //}
        //$("#" + BillingInformation.params.PanelID + " #divCheckBoxes").addClass('col-sm-6 pad-a-labelsize');
        $("#" + BillingInformation.params.PanelID + " #ddlTimeMin").val(EMRTimeId)
        $("#" + BillingInformation.params.PanelID + " #divCheckBoxes").find('input[type=checkbox]').prop('checked', false);
        $("#" + BillingInformation.params.PanelID + " #divCheckBoxes").find('input[id="' + EMRTimeId + '"]').prop('checked', true);


        if ($(ddl).val() == "") {
            BillingInformation.ddlTimeMin_Change();
        }
        BillingInformation.CheckPreviousStateofChkBoxs();
        //var fromTo = $(ddl).find("option:selected").attr("range").split("-");


        //if (fromTo.length > 1) {

        //    var from = parseInt(fromTo[0]);
        //    var to = parseInt(fromTo[1]);
        //    $("#" + BillingInformation.params.PanelID + " #divCheckBoxes").html('');
        //    for (var checkBox = from; checkBox <= to ; checkBox++) {
        //        $("#" + BillingInformation.params.PanelID + " #divCheckBoxes").append("<input type='checkbox' oncheck='BillingInformation.SelectCheckBox(this)' id='" + checkBox + "' />" + checkBox);
        //    }
        //}
    },

    //SuggestedCodeSelect: function () {
    //    //ahson
    //    BillingInformation.SelectedPatientType = SelectedType;
    //    if (SelectedType == "1" || SelectedType == "2") {
    //        $('#txtSuggestEM').show();
    //        BillingInformation.GetEMCodeSuggestions().done(
    //        function (response) {
    //            BillingInformation.CodeSuggestions = JSON.parse(response);
    //        });
    //    }
    //    else {
    //        $('#txtSuggestEM').hide();
    //    }
    //    ///end ahson

    //    //ahson
    //    for (var index in BillingInformation.TempBillingInfoTime) {
    //        if (BillingInformation.CodeSuggestions.SuggestedCode == BillingInformation.TempBillingInfoTime[index].ENMCPT) {
    //            checkbox = $('#' + BillingInformation.TempBillingInfoTime[index].Description.trim());
    //        }
    //    }
    //    //end ahson

    //    if (checkbox != null) {
    //        checkbox.prop('checked', true);
    //        BillingInformation.SelectCheckBox(checkbox[0]);
    //    }

    //},

    CheckPreviousStateofChkBoxs: function () {
        $("#" + BillingInformation.params.PanelID + " #tblEMCodes input[type='checkbox']").prop('checked', false);
        $.each(BillingInformation.EMCheckedCheckBoxArray, function (index, item) {
            $.each($("#" + BillingInformation.params.PanelID + " #tblEMCodes input[type='checkbox']"), function (i, chkbox) {
                if ($(chkbox).attr("value") == item.CheckBoxVal)
                    $(chkbox).prop("checked", true);
            });
        });
    },
    ddlTimeMin_Change: function () {
        var time = $("#" + BillingInformation.params.PanelID + " #ddlTimeMin").val();

        $("#" + BillingInformation.params.PanelID + " #divCheckBoxes [type='checkbox']").prop("checked", false);
        if (time != '') {
            $("#" + BillingInformation.params.PanelID + " #divCheckBoxes #" + time).prop("checked", true);
            BillingInformation.SelectCheckBox($("#" + BillingInformation.params.PanelID + " #divCheckBoxes #" + time));
        }
        else {
            var NewPatient = BillingInformation.EMCodeNewptDesc;
            var EstPatient = BillingInformation.EMCodeESTptDesc;
            var SelectedTypeText = $('#' + BillingInformation.params.PanelID + ' #ddlType').find("option:selected").text().trim();

            if (SelectedTypeText == NewPatient || SelectedTypeText == EstPatient) {

                var Found = false;
                var CopyEMCheckedCheckBoxArray = [];
                $.grep(BillingInformation.EMCheckedCheckBoxArray, function (item, index) {
                    if (item.Type == NewPatient || item.Type == EstPatient) {
                        $.when(BillingInformation.DeletChargeRow(item.Type, item.CheckBoxVal)).then(function () {

                        });
                        Found = true;
                    }
                    else {
                        CopyEMCheckedCheckBoxArray.push(item);
                    }
                });
                BillingInformation.EMCheckedCheckBoxArray = CopyEMCheckedCheckBoxArray;
            }
            else {

            }


        }



    },
    DeletChargeRow: function (SelectedType, ChkBoxValue) {
        var dfd = $.Deferred();
        var def = [];
        $.each($('#' + BillingInformation.params.PanelID + ' #pnlVisitCharge_Result #dgvBillVisitCharge tbody tr'), function (i, item) {
            var Type = $(item).attr("Type");
            var CheckBoxValue = $(item).attr("CheckBoxValue");
            if ((typeof Type !== typeof undefined && Type != "") && (typeof CheckBoxValue !== typeof undefined && CheckBoxValue != "")) {
                if (Type == SelectedType && CheckBoxValue == ChkBoxValue) {

                    var CPTCode = $(item).find("input[id*='hfCPTCode']").val();
                    //var BillingInfoCPTId = $(item).attr("BillingInfoCPTId")
                    //if (typeof BillingInfoCPTId !== typeof undefined && BillingInfoCPTId != "") {
                    //    if (parseInt(BillingInfoCPTId) < 0) {

                    BillingInformation.EditableGrid.datatable.row($(item).get(0)).remove().draw();
                    BillingInformation.ReorderCPTs();
                    //}
                    //else {
                    def.push(BillingInformation.DeleteCPT(CPTCode, BillingInformation.params.BillingInfoId).done(function (response) {
                        response = JSON.parse(response);
                    }));

                    //    }
                    //}
                    //else{
                    //    BillingInformation.DeleteCPT(CPTCode, BillingInformation.params.BillingInfoId).done(function (response) {
                    //        response = JSON.parse(response);
                    //    });
                    //}
                }
            }
        });
        $.when.apply($, def).done(function ($n) {
            dfd.resolve();
        });
        return dfd;
    },
    AddInEMCheckedCheckBoxArray: function (chk) {
        var dfd = $.Deferred();
        var NewPatient = BillingInformation.EMCodeNewptDesc;
        var EstPatient = BillingInformation.EMCodeESTptDesc;
        var SelectedTypeText = $(chk).attr('billinginfotype').trim();

        if ($(chk).prop("checked")) {
            if (SelectedTypeText == NewPatient || SelectedTypeText == EstPatient) {

                var Found = false;
                var CopyEMCheckedCheckBoxArray = [];
                $.grep(BillingInformation.EMCheckedCheckBoxArray, function (item, index) {
                    if (item.Type == NewPatient || item.Type == EstPatient) {
                        var obj = {};
                        obj.Type = SelectedTypeText;
                        obj.CheckBoxVal = $(chk).val();
                        CopyEMCheckedCheckBoxArray.push(obj);
                        //delete from charge grid
                        $.when(BillingInformation.DeletChargeRow(item.Type, item.CheckBoxVal)).then(function () {
                            dfd.resolve();
                        });
                        Found = true;
                    }
                    else {
                        CopyEMCheckedCheckBoxArray.push(item);
                    }
                });
                BillingInformation.EMCheckedCheckBoxArray = CopyEMCheckedCheckBoxArray;
                if (!Found) {
                    var obj = {};
                    obj.Type = SelectedTypeText;
                    obj.CheckBoxVal = $(chk).val();
                    BillingInformation.EMCheckedCheckBoxArray.push(obj);
                    dfd.resolve();
                }
            }
            else {

                var Found = false;

                $.grep(BillingInformation.EMCheckedCheckBoxArray, function (item, index) {
                    if (item.Type == SelectedTypeText && item.CheckBoxVal == $(chk).val()) {
                        Found = true;
                        return;
                    }
                });

                if (!Found) {
                    var obj = {};
                    obj.Type = SelectedTypeText;
                    obj.CheckBoxVal = $(chk).val();
                    BillingInformation.EMCheckedCheckBoxArray.push(obj);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }


            }
        }
        else {
            var Found = false;
            var CopyEMCheckedCheckBoxArray = [];
            $.grep(BillingInformation.EMCheckedCheckBoxArray, function (item, index) {
                if (item.Type == SelectedTypeText && item.CheckBoxVal == $(chk).val()) {
                    Found = true;
                }
                else {
                    CopyEMCheckedCheckBoxArray.push(item);
                }
            });
            BillingInformation.EMCheckedCheckBoxArray = CopyEMCheckedCheckBoxArray;
            dfd.resolve();



        }
        return dfd;

        //&& item.CheckBoxVal == $(chk).val()
    },
    CheckCodeIsAlreadyExists: function (chk) {
        var exists = false;
        if ($('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr input[id*="txtCPT"]').length > 0) {
            $.each($('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr'), function (i, item) {
                var cptDescription = $(item).find("td:nth-child(5)").find('input[id*="txtCPT"]').val().trim();
                if (cptDescription.split('-')[0].trim() == $(chk).attr('code').trim())
                    exists = true;
            });
        }
        return exists;
    },
    InChargeGridAnyEmCodeExists: function () {
        var exists = false;
        $.each($('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr'), function (i, item) {
            //$(item).find("input[id*='hfCPTCode']").val()
            var cptDescription = $(item).find("td:nth-child(5)").find('input[id*="txtCPT"]').val().trim();
            var cptCode = cptDescription.split('-')[0].trim();
            $.each(BillingInformation.BillingInfoTime, function (i, item) {
                if (item.ENMCPT.trim() == cptCode) {
                    exists = true;
                }
            });
        });
        return exists;
    },
    SelectCheckBox: function (chk, ShowAlert) {
        if ($('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr input[id*="txtCPT"]').length > 0)
            BillingInformation.SelectCheckBoxDone(chk, ShowAlert);
        else {
            $.when(BillingInformation.AddNewChargeRow(null, 'Add')).then(function () {
                BillingInformation.SelectCheckBoxDone(chk, ShowAlert);
            });
        }
    },
    SelectCheckBoxDone: function (chk, ShowAlert) {
        if (typeof ShowAlert != typeof undefined && ShowAlert != null && ShowAlert && $(chk).prop("checked")) {
            if (BillingInformation.CheckCodeIsAlreadyExists(chk)) {
                utility.DisplayMessages('CPT Code ' + $(chk).attr('code').trim() + ' already Exist', 2);
                $(chk).prop("checked", false);
            }
            else {
                if (!ShowAlert)
                    $(chk).prop("checked", true);
                if (BillingInformation.InChargeGridAnyEmCodeExists()) {
                    var Message = ''
                    var NewPatient = BillingInformation.EMCodeNewptDesc;
                    var EstPatient = BillingInformation.EMCodeESTptDesc;
                    var SelectedTypeText = $(chk).attr('billinginfotype').trim();
                    if (SelectedTypeText == NewPatient || SelectedTypeText == EstPatient) {
                        if (BillingInformation.EMCheckedCheckBoxArray.length > 0) {
                            var Found = false;
                            $.grep(BillingInformation.EMCheckedCheckBoxArray, function (item, index) {
                                if (item.Type == NewPatient || item.Type == EstPatient) {
                                    Found = true;
                                    return;
                                }
                            });
                            if (!Found)
                                Message = '47';
                            else
                                Message = '46';
                        }
                        else
                            Message = '47';
                    }
                    else
                        Message = '47';
                    utility.myConfirm(Message, function () {
                        BillingInformation.OldSelectCheckBox(chk);
                    }, function () {
                        $(chk).prop("checked", false);
                    },
                           '1'
                       );
                }
                else
                    BillingInformation.OldSelectCheckBox(chk);
            }
        }
        else {
            if (!ShowAlert)
                $(chk).prop("checked", true);
            BillingInformation.OldSelectCheckBox(chk);
        }

    },
    OldSelectCheckBox: function (chk) {

        $.when(BillingInformation.AddInEMCheckedCheckBoxArray(chk)).then(function () {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10)
                dd = '0' + dd;
            if (mm < 10)
                mm = '0' + mm;
            today = mm + '/' + dd + '/' + yyyy;

            var counter;
            var SelectedType = $(chk).attr('billinginfotype').trim();
            var Object = $('#' + BillingInformation.params.PanelID + ' #tblBillingInformation');
            if ($('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').length == 1) {
                if ($('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr td').length == 1)
                    counter = 0;
                else
                    counter = $('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').length;
            }
            else
                counter = $('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').length;
            // added by faizan

            var CurrentCpt = $.grep(BillingInformation.BillingInfoTime, function (a) {
                return a.BillingInfoTimeId == $(chk).val();
            })[0];

            // commented by faizan
            //var CurrentCpt = $.grep(BillingInformation.BillingInfoTime, function (a) {
            //    return a.Description == $(chk).val() && ((SelectedType == '1' && a.Type == 'New') || (SelectedType == '2' && a.Type == 'Established'))
            //})[0];

            var isEdit = false;

            var indexCPT = 0;

            var $CPTRow = null;

            $("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr").each(function () {
                var CPTCode = $(this).find("input[id*='hfCPTCode']").val();
                //var filterCPT = $.grep(BillingInformation.BillingInfoTime, function (a) {
                //    return  CPTCode
                //})
                var filterCPT = 0;
                if (CurrentCpt != null && CurrentCpt.ENMCPT == CPTCode) {
                    filterCPT = CPTCode;
                }
                if (filterCPT != 0) {
                    if (filterCPT.length > 0) {
                        indexCPT = $(this).index() + 1;
                        $CPTRow = $(this);
                    }
                }

            });
            var count = counter + 1;
            var AlreadyExist = false;

            if ($(chk).prop("checked")) {
                var selectedValueText = $(chk).attr('billinginfotype').trim();
                if (selectedValueText == BillingInformation.EMCodeNewptDesc || selectedValueText == BillingInformation.EMCodeESTptDesc)
                    $("#" + BillingInformation.params.PanelID + " #tblEMCodes input[type='checkbox']").prop('checked', false);
                $(chk).prop("checked", true);
                if (indexCPT == 0)
                    BillingInformation.AddNewChargeRow(count * -1, 'Add', null, selectedValueText, $(chk).val().trim());
                else {
                    AlreadyExist = true;
                    count = indexCPT;
                }

                if (AlreadyExist) {
                    var CPTCode = $CPTRow.find("input[id*='hfCPTCode']").val();
                    BillingInformation.DeleteCPT(CPTCode, BillingInformation.params.BillingInfoId).done(function (response) {
                        response = JSON.parse(response);
                    });
                    $CPTRow.find("input[id*='hfCPTCode']").val(CurrentCpt.ENMCPT);
                    $CPTRow.find("input[id*='hfCPTDescription']").val(CurrentCpt.ENMCPTDescription);
                    $CPTRow.find("input[id*='hfIsMain']").val(1);
                    $CPTRow.find("input[id*='txtCPT']").val(CurrentCpt.ENMCPT + ' - ' + CurrentCpt.ENMCPTDescription);
                    $CPTRow.find("#hfBillingInfoCPTId").val(CurrentCpt.BillingInfoCPTId ? CurrentCpt.BillingInfoCPTId : "");
                    utility.SetAutoCompleteSourceforValidate($CPTRow.find("input[id*='txtCPT']"), CurrentCpt.ENMCPT, CurrentCpt.ENMCPTDescription);
                    $CPTRow.find("input[id*='hfUnitsId']").val(1);
                    $CPTRow.find("input[id*='txtUnits']").val(1);
                    $CPTRow.find("input[id*='hfStartDate']").val(today);
                    $CPTRow.find("input[id*='hfEndDate']").val(today);
                    $CPTRow.find("input[id*='txtCPT']").focus();
                    $CPTRow.find("input[id*='txtCPT']").blur();
                    if (indexCPT > 1) {
                        //$CPTRow.insertBefore($("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr:first"));
                        $("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr:first").find("input[id*='txtCPT']").focus();
                        $("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr:first").find("input[id*='txtCPT']").blur();
                    }

                } else {
                    var $row = $("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr:nth-child(" + count + ")");
                    $row.find("input[id*='hfCPTDescription']").val(CurrentCpt.ENMCPTDescription);
                    $row.find("input[id*='hfIsMain']").val(1);
                    $row.find("input[id*='txtCPT']").val(CurrentCpt.ENMCPT + ' - ' + CurrentCpt.ENMCPTDescription);
                    $row.find("#hfBillingInfoCPTId").val(CurrentCpt.BillingInfoCPTId ? CurrentCpt.BillingInfoCPTId : "");
                    utility.SetAutoCompleteSourceforValidate($row.find("input[id*='txtCPT']"), CurrentCpt.ENMCPT, CurrentCpt.ENMCPTDescription);
                    $row.find("input[id*='hfUnitsId']").val(1);
                    $row.find("input[id*='txtUnits']").val(1);
                    $row.find("input[id*='hfStartDate']").val(today);
                    $row.find("input[id*='hfEndDate']").val(today);
                    $row.find("input[id*='hfCPTCode']").val(CurrentCpt.ENMCPT);
                    $row.find("input[id*='txtCPT']").focus();
                    $row.find("input[id*='txtCPT']").blur();
                    //$row.insertBefore($("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr:first"));
                    BillingInformation.EmptyRowsGoDown();
                    //$("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr:first").find("input[id*='txtCPT']").focus();
                    //$("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr:first").find("input[id*='txtCPT']").blur();
                }
                BillingInformation.ReorderCPTs();
            }
            else {
                if (indexCPT != 0) {
                    var $row = $("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr:nth-child(" + indexCPT + ")");
                    var CPTCode = $("#" + BillingInformation.params.PanelID + " #tblEMCodes #" + $(chk).attr("id")).parent().text()
                    BillingInformation.DeleteCPT(CPTCode, BillingInformation.params.BillingInfoId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            if ($row.hasClass('adding')) {
                            }
                            var _self = EditableGrid;
                            _self.datatable.row($row.get(0)).remove().draw();

                            utility.DisplayMessages(response.message, 1);
                            BillingInformation.EmptyRowsGoDown();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                BillingInformation.ReorderCPTs();
            }
            $.each(BillingInformation.EMCheckedCheckBoxArray, function (index, itm) {
                $("#" + BillingInformation.params.PanelID + " #tblEMCodes input[type='checkbox'][id='" + itm.CheckBoxVal + "']").prop("checked", true);
            });
        });
    },
    EmptyRowsGoDown: function () {
        $.each($("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr"), function (index, row) {
            var Switch = false;
            if ($(row).find("#txtCPT" + ($(row).attr("id"))).val() == "") {
                if ($("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr").length != (index + 1)) {
                    if ($("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr:nth-child(" + $("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr").length + ")").find("input[id*='txtCPT']").val() != "") {
                        $(row).insertAfter($("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr:nth-child(" + $("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr").length + ")"));
                        Switch = true;
                    }
                }
            }
            if (Switch) {
                index--;
            }
        });
    },

    putBillingInformationCPTid: function (infoResponse) {
        var BillingInfoCptObject = null;
        var Object = $('#' + BillingInformation.params.PanelID + ' #tblBillingInformation');
        if (infoResponse.BillingInfoCPTFill_JSON.length > 0) {
            BillingInfoCptObject = infoResponse.BillingInfoCPTFill_JSON;
            if (BillingInfoCptObject) {
                $(BillingInfoCptObject).each(function (index, item) {
                    $('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').each(function (i, tm) {
                        item.DOSTo = new Date(item.DOSTo);
                        item.DOSFrom = new Date(item.DOSFrom);
                        var dayTo = item.DOSTo.getDate();
                        var monthTo = item.DOSTo.getMonth() + 1;
                        var yearTo = item.DOSTo.getFullYear();
                        if (dayTo < 10)
                            dayTo = "0" + dayTo;
                        if (monthTo < 10)
                            monthTo = "0" + monthTo;
                        var dateTo = monthTo + "/" + dayTo + "/" + yearTo;
                        var dayFrom = item.DOSFrom.getDate();
                        var monthFrom = item.DOSFrom.getMonth() + 1;
                        var yearFrom = item.DOSFrom.getFullYear();
                        if (dayFrom < 10)
                            dayFrom = "0" + dayFrom;
                        if (monthFrom < 10)
                            monthFrom = "0" + monthFrom;
                        var dateFrom = monthFrom + "/" + dayFrom + "/" + yearFrom;
                        item.DOSTo = dateTo;
                        item.DOSFrom = dateFrom;
                        if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                            if (item.CPTCode && $(this).find("input:hidden[id*='hfCPTCode']").val() == item.CPTCode && item.CPTCodeDescription && $(this).find("input:hidden[id*='hfCPTDescription']").val() == utility.decodeHtml(item.CPTCodeDescription) && $(this).find("input[id*='dtpDOSFrom']").val() == item.DOSFrom && $(this).find("input[id*='dtpDOSTo']").val() == item.DOSTo) {
                                rowId = $(this).attr("id");
                                $(Object).find("#hfBillingInfoCPTId" + rowId).val(item.BillingInfoCPTId ? item.BillingInfoCPTId : "");
                                return false;
                            }
                        }
                        else {
                            if (item.CPTCode && $(this).find("input:hidden[id*='hfCPTCode']").val() == item.CPTCode && item.CPTCodeDescription && $(this).find("input:hidden[id*='hfCPTDescription']").val() == utility.decodeHtml(item.CPTCodeDescription)) {
                                rowId = $(this).attr("id");
                                $(Object).find("#hfBillingInfoCPTId" + rowId).val(item.BillingInfoCPTId ? item.BillingInfoCPTId : "");
                                return false;
                            }
                        }

                    });
                });
            }
        }
    },
    //Author: Farooq Ahmad
    //Date: 08/03/2016
    //This function will load the detail of the physical template detail by id
    LoadBillingInformation: function () {
        var selectedEMCodeObject;
        BillingInformation.BillingInformationLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                BillingInformation.BillingInformation_DBResponse = response;
                response.BillingInfoFill_JSON = JSON.parse(response.BillingInfoFill_JSON);
                BillingInformation.Status = response.BillingInfoFill_JSON.Status;
                var BillingInfoFill_JSONENMTypeId = response.BillingInfoFill_JSON.ENMTypeId;
                var self = $('#' + BillingInformation.params.PanelID + " #tblBillingInformation");
                utility.bindMyJSONByName(true, response.BillingInfoFill_JSON, false, self).done(function () {
                    //PRD-234 By:MAhmad
                    if (response.BillingInfoFill_JSON.Facility) {
                        var Ctrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #txtFacility");
                        var hfCtrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #hfFacility");

                        utility.SetKendoAutoCompleteSourceforValidate(Ctrl, response.BillingInfoFill_JSON.Facility, hfCtrl, response.BillingInfoFill_JSON.FacilityId);
                        $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #txtFacility").blur();
                    }

                    if (response.BillingInfoFill_JSON.Provider) {
                        var Ctrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #txtProvider");
                        var hfCtrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #hfProvider");
                        utility.SetKendoAutoCompleteSourceforValidate(Ctrl, response.BillingInfoFill_JSON.Provider, hfCtrl, response.BillingInfoFill_JSON.ProviderId);
                    }



                    if (response.BillingInfoFill_JSON.ResourceProvider) {

                        var Ctrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #txtResourceProvider");
                        var hfCtrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #hfResourceProvider");
                        utility.SetKendoAutoCompleteSourceforValidate(Ctrl, response.BillingInfoFill_JSON.ResourceProvider, hfCtrl, response.BillingInfoFill_JSON.ResourceProviderId);
                    }


                    if (response.BillingInfoFill_JSON.RefProvider) {
                        var Ctrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #txtRefProvider");
                        var hfCtrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #hfRefProvider");
                        utility.SetKendoAutoCompleteSourceforValidate(Ctrl, response.BillingInfoFill_JSON.RefProvider, hfCtrl, response.BillingInfoFill_JSON.RefProviderId);
                    }
                    //PRD-234 By:MAhmad

                    var selectedTime = "";
                    try {
                        selectedTime = $.grep(BillingInformation.BillingInfoTime, function (a) {
                            return a.BillingInfoTimeId == response.BillingInfoFill_JSON.ENMTimeId;
                        });
                    } catch (ex) {
                        console.log(ex);
                    }
                    selectedEMCodeObject = selectedTime;
                    if (selectedTime != "" || selectedTime.length > 0)
                        selectedTime = selectedTime[0].Description;
                    if (response.BillingInfoFill_JSON.Status == "Signed") {
                        if (BillingInformation.params.NotesId != null && BillingInformation.params.NotesId > 0) {
                            Clinical_ProgressNote.FillNotes(null, BillingInformation.params.NotesId, BillingInformation.params.PatientId).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                                    if (Clinical_Notes_detail.IsNonBilable.toString().toLowerCase() == "true") {
                                        $("#" + BillingInformation.params.PanelID + " #btnViewCharges").addClass("hidden");
                                        $("#" + BillingInformation.params.PanelID + " #btnViewChargesBottom").addClass("hidden");
                                    } else {
                                        $("#" + BillingInformation.params.PanelID + " #btnViewCharges").removeClass("hidden");
                                        $("#" + BillingInformation.params.PanelID + " #btnViewChargesBottom").removeClass("hidden");
                                    }
                                } else {
                                    utility.DisplayMessages(response.Message, 3);
                                    BackgroundLoaderShow(false);
                                }
                            });
                        }
                        else {
                            $("#" + BillingInformation.params.PanelID + " #btnViewCharges").removeClass("hidden");
                            $("#" + BillingInformation.params.PanelID + " #btnViewChargesBottom").removeClass("hidden");
                        }

                        $("#" + BillingInformation.params.PanelID + " #divOutOfOfficeVisit,#divEvalnMangement,#divICDCPT,#IsNonBilableDiv").addClass("disableAll");
                        $("#" + BillingInformation.params.PanelID + " #btnViewCharges").attr("onclick", "BillingInformation.LoadVisitDetail('" + BillingInformation.params.VisitId + "','" + BillingInformation.params.PatientId + "',event)");
                        $("#" + BillingInformation.params.PanelID + " #btnSign").addClass("hidden");
                        $("#" + BillingInformation.params.PanelID + " #btnSave").addClass("hidden");

                        $("#" + BillingInformation.params.PanelID + " #btnViewChargesBottom").attr("onclick", "BillingInformation.LoadVisitDetail('" + BillingInformation.params.VisitId + "','" + BillingInformation.params.PatientId + "',event)");
                        $("#" + BillingInformation.params.PanelID + " #btnSignBottom").addClass("hidden");
                        $("#" + BillingInformation.params.PanelID + " #btnSaveBottom").addClass("hidden");

                    }
                    if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {

                        if (BillingInformation.params.BillingInfoId != null && parseInt(BillingInformation.params.BillingInfoId) > 0) {

                            BillingInformation.FillDOS('dpStartDate', utility.RemoveTimeFromDate(null, response.BillingInfoFill_JSON.ENMCPTDOSFrom));
                            BillingInformation.FillDOS('dpToDate', utility.RemoveTimeFromDate(null, response.BillingInfoFill_JSON.ENMCPTDOSTo));
                            BillingInformation.FillDOS('dpAdmissionDate', utility.RemoveTimeFromDate(null, response.BillingInfoFill_JSON.AdmissionDate));
                            BillingInformation.FillDOS('dpDischargeDate', utility.RemoveTimeFromDate(null, response.BillingInfoFill_JSON.DischargeDate));
                        }
                        else {

                            var userType = EMRUtility.checkCurrentUserType(Providers, globalAppdata["AppUserLastName"].trim().toLowerCase(), globalAppdata["AppUserFirstName"].trim().toLowerCase());
                            if (userType != null) {
                                $("#" + BillingInformation.params.PanelID + " #txtProvider").val(userType.value);
                                $("#" + BillingInformation.params.PanelID + " #hfProvider").val(userType.id);
                                $("#" + BillingInformation.params.PanelID + " #txtResourceProvider").val(userType.value);
                                $("#" + BillingInformation.params.PanelID + " #hfResourceProvider").val(userType.id);
                            }

                        }
                    }

                    $Ctrl_p = $('#' + BillingInformation.params.PanelID + ' #txtProvider');
                    $hfCtrl_p = $('#' + BillingInformation.params.PanelID + ' #hfProvider');
                    //Provider
                    utility.SetAutoCompleteSource($Ctrl_p, $hfCtrl_p);

                    $Ctrl_rp = $('#' + BillingInformation.params.PanelID + ' #txtResourceProvider');
                    $hfCtrl_rp = $('#' + BillingInformation.params.PanelID + ' #hfResourceProvider');
                    //Resource Provider
                    utility.SetAutoCompleteSource($Ctrl_rp, $hfCtrl_rp);

                    $Ctrl_f = $('#' + BillingInformation.params.PanelID + ' #txtFacility');
                    $hfCtrl_f = $('#' + BillingInformation.params.PanelID + ' #hfFacility');
                    //Provider
                    utility.SetAutoCompleteSource($Ctrl_f, $hfCtrl_f);

                    //End//Abid Ali//For Out Of Office Visit
                });

                BillingInformation.SavedCPTs = response.BillingInfoCPTFill_JSON = JSON.parse(response.BillingInfoCPTFill_JSON);
                BillingInformation.SavedICDs = response.BillingInfoICDFill_JSON = JSON.parse(response.BillingInfoICDFill_JSON);
                BillingInformation.EMCheckedCheckBoxArray = [];
                $.each(BillingInformation.SavedCPTs, function (idexSavedCPTs, SavedCPTs) {
                    if (SavedCPTs.Type != "" && SavedCPTs.Type != null && (typeof SavedCPTs.Type != typeof undefined) && SavedCPTs.BillingInfoTimeId != "" && SavedCPTs.BillingInfoTimeId != null && (typeof SavedCPTs.Type != typeof undefined)) {
                        var EmObj = {};
                        EmObj.Type = SavedCPTs.Type;
                        EmObj.CheckBoxVal = SavedCPTs.BillingInfoTimeId;
                        BillingInformation.EMCheckedCheckBoxArray.push(EmObj);
                    }
                });
                var copyEMCheckedCheckBoxArray = BillingInformation.EMCheckedCheckBoxArray;
                if (((response.BillingInfoCPTFill_JSON.length == 0 && response.BillingInfoICDFill_JSON.length == 0) || response.BillingInfoFill_JSON.Status == "Draft" || response.BillingInfoFill_JSON.Status == "Signed") && (BillingInformation.params.NotesId != null && parseInt(BillingInformation.params.NotesId) > 0)) {

                    BillingInformation.LoadAttachecdICDsAndCPTs().done(function (response) {
                        response = utility.decodeHtml(response);
                        response = JSON.parse(response);
                        if (response.status != false) {
                            response.ProblemListFill_JSON = JSON.parse(response.ProblemListFill_JSON);
                            response.ProcedureListFill_JSON = JSON.parse(response.ProcedureListFill_JSON);
                            response.ProblemListFill_JSON = $.grep(response.ProblemListFill_JSON, function (a) {
                                return a.IsNoteLinked == "True";
                            });
                            response.ProcedureListFill_JSON = $.grep(response.ProcedureListFill_JSON, function (a) {
                                return a.IsNoteLinked == "1" || (parseInt(a.VaccineHxId) > 0 || parseInt(a.ImmTherInjectionId) > 0);
                            });
                            $.each(response.ProcedureListFill_JSON, function (i, procCpt) {
                                var found = false;
                                $.each(BillingInformation.SavedCPTs, function (index, BillCPT) {
                                    if (procCpt.IsFromSupperBill == "True" && procCpt.CPTId == BillCPT.CPTId) {
                                        procCpt.BillType = BillCPT.Type;
                                        procCpt.BillingInfoTimeId = BillCPT.BillingInfoTimeId;
                                        found = true;
                                        return;
                                    }
                                });
                                if (!found) {
                                    procCpt.BillType = null;
                                    procCpt.BillingInfoTimeId = null;
                                }
                            });

                            var NumberOfProblems = response.ProblemListFill_JSON.length;
                            if (NumberOfProblems > 0) {
                                $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").html('');
                                if (NumberOfProblems > 12)
                                    NumberOfProblems = 12;
                                for (var counter = 1; counter <= NumberOfProblems  ; counter++) {
                                    var Html = '<div class="col-sm-3 col-md-3"><label class="control-label">Diagnosis ' + counter + '</label><div class="input-group"><input class="form-control" id="txtDisease_' + counter + '" oninput="BillingInformation.EnableDisableICDs();BillingInformation.bindICD9AutoComplete(this);" onblur="BillingInformation.ValidateICDCodeAndSetDesc(this);" onkeypress="    BillingInformation.disableEnterKey(event)" name="Disease_' + counter + '" type="text" value=""><div class="input-group-btn"><button id="btnDisease_' + counter + '" class="btn btn-primary btn-xs" type="button" onclick="BillingInformation.OpenSearchPopup(\'ICD\', \'txtDisease_' + counter + '\', \'hfICDCode9_' + counter + ',hfICDDescription9_' + counter + ',hfICDCode10_' + counter + ',hfICDDescription10_' + counter + ',hfSNOMEDCode_' + counter + ',hfSNOMEDDescription_' + counter + ',txtDisease_' + counter + '\')"><i class="glyphicon glyphicon-search"></i></button></div></div><input type="hidden" id="hfICDCode9_' + counter + '" name="ICDCode9_' + counter + '" /><input type="hidden" id="hfICDDescription9_' + counter + '" name="ICDDescription9_' + counter + '" /><input type="hidden" id="hfICDCode10_' + counter + '" name="ICDCode10_' + counter + '" /><input type="hidden" id="hfICDDescription10_' + counter + '" name="ICDDescription10_' + counter + '" /><input type="hidden" id="hfSNOMEDCode_' + counter + '" name="hfSNOMEDCode_' + counter + '" /><input type="hidden" id="hfSNOMEDDescription_' + counter + '" name="SNOMEDDescription_' + counter + '" /><input type="hidden" id="hfCustomFormId_' + counter + '" name="CustomFormId_' + counter + '" /></div>';
                                    $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").append(Html);
                                }
                                if (NumberOfProblems == 12) {
                                    $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").append('<div class="clearfix spacer5"></div><div class="pull-left pl-default"><a class="btn btn-link btn-xs p-none disableAll" onclick="BillingInformation.AddMoreDiagnosis();">Add More</a></div>');
                                }
                                else {
                                    $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").append('<div class="clearfix spacer5"></div><div class="pull-left pl-default"><a class="btn btn-link btn-xs p-none" onclick="BillingInformation.AddMoreDiagnosis();">Add More</a></div>');
                                }

                            }

                            BillingInformation.EnableDisableICDs();

                            var Object = $('#' + BillingInformation.params.PanelID + ' #tblBillingInformation');
                            var counter = 0;
                            $(response.ProblemListFill_JSON).each(function (index, item) {
                                var isExist = false;
                                for (var count = 1; count < 13; count++) {
                                    if ((item.ICD9 == $(Object).find("#hfICDCode9_" + count).val() && item.ICD9 != null && item.ICD9 != '' && $(Object).find("#hfICDDescription9_" + count).val() == item.ICD9_Description) ||
                                     (item.ICD10 == $(Object).find("#hfICDCode10_" + count).val() && item.ICD10 != null && item.ICD10 != '' && $(Object).find("#hfICDDescription10_" + count).val() == item.ICD10_Description)) {
                                        isExist = true;
                                    }
                                }

                                if (!isExist) {
                                    counter = counter + 1;
                                    if (item.Description == null || item.Description == '') {
                                        item.Description = '';
                                        if (item.ICD9 != null && item.ICD9 != '') {
                                            item.Description = item.ICD9 + ' - ';
                                        }
                                        if (item.ICD10 != null && item.ICD10 != '') {
                                            item.Description = item.Description + item.ICD10 + ' - ';
                                        }
                                        if (item.SNOMEDID != null && item.SNOMEDID != '') {
                                            item.Description = item.Description + item.SNOMEDID + ' - ';
                                        }
                                        if (item.ICD10_Description != null && item.ICD10_Description != '') {
                                            item.Description = item.Description + item.ICD10_Description;
                                        }
                                        else if (item.ICD9_Description != null && item.ICD9_Description != '') {
                                            item.Description = item.Description + item.ICD9_Description;
                                        }
                                        else if (item.ProblemName != null && item.ProblemName != '') {
                                            item.Description = item.Description + item.ProblemName;
                                        }
                                    }
                                    $(Object).find("#hfICDCode9_" + counter).val(item.ICD9);
                                    $(Object).find("#hfICDDescription9_" + counter).val(item.ICD9_Description);
                                    $(Object).find("#hfICDCode10_" + counter).val(item.ICD10);
                                    $(Object).find("#hfICDDescription10_" + counter).val(item.ICD10_Description);
                                    $(Object).find("#hfSNOMEDCode_" + counter).val(item.SNOMEDID);
                                    $(Object).find("#hfSNOMEDDescription_" + counter).val(item.SNOMED_DESCRIPTION);
                                    $(Object).find("#txtDisease_" + counter).val(item.Description);
                                    $(Object).find("#hfCustomFormId_" + counter).val(item.CustomFormId);

                                }
                                else
                                    utility.DisplayMessages('ICD already added.', 3);
                            });
                            var counter = 0;
                            BillingInformation.EMCheckedCheckBoxArray = copyEMCheckedCheckBoxArray;
                            $(response.ProcedureListFill_JSON).each(function (index, item) {
                                item.CPT_DESCRIPTION = item.CPT_DESCRIPTION.replace(/&quot;/g, '"').replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&#39;/g, "'").replace(/&amp;/g, "&").replace(/\/\equal/g, '=');
                                var isExist = false;

                                $('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').each(function (i, tm) {
                                    if ($(this).find("input:hidden[id*='hfCPTCode']").val() == item.CPTCode && item.CPTCode != null && item.CPTCode != '' && item.CPT_DESCRIPTION != '' && $(this).find("input:hidden[id*='hfCPTDescription']").val() == item.CPT_DESCRIPTION) {
                                        isExist = true;
                                    }
                                });

                                if (item.CPTCode == null || item.CPTCode == '') {
                                    utility.DisplayMessages('Please select a valid CPT.', 3);
                                    return true;
                                }
                                if (!isExist) {
                                    try {
                                        var SavedItem = $.grep(BillingInformation.SavedCPTs, function (itSavedCPT, itIndex) {
                                            if (item.CPTId == itSavedCPT.CPTId) {
                                                return itSavedCPT;
                                            }
                                        });
                                    }
                                    catch (ex) {
                                        console.log(ex);
                                    }
                                    if (BillingInformation.params.FromCCM && (item.CPTCode != "99201" || item.CPTCode != "99211")) {
                                        counter = counter + 1;
                                        $(Object).find("#hfCPTCode-" + counter).val(item.CPTCode);
                                        //to fill cpt fee
                                        $(Object).find("#hfFee-" + counter).val(item.Fee);
                                        // $(Object).find("#txtFee-" + counter).val(item.Fee);
                                        //
                                        $(Object).find("#hfCPTDescription-" + counter).val(item.CPT_DESCRIPTION);
                                        $(Object).find("#hfCPTSNOMEDCodeId-" + counter).val(item.SNOMEDID);
                                        $(Object).find("#hfCPTSNOMEDDescription-" + counter).val(item.SNOMED_DESCRIPTION);
                                        $(Object).find("#txtModifier1-" + counter).val(item.Modifier);
                                        $(Object).find("#txtCPT-" + counter).val(item.CPTCode + ' - ' + item.CPT_DESCRIPTION);
                                        utility.SetAutoCompleteSourceforValidate($(Object).find("#txtCPT-" + counter), item.CPTCode, item.CPT_DESCRIPTION);
                                        $(Object).find("#-" + counter).attr("IsLabBasedCPT", item.IsLabBasedCPT);
                                        $(Object).find("#-" + counter).attr("Type", item.BillType);
                                        $(Object).find("#-" + counter).attr("CheckBoxValue", item.BillingInfoTimeId);
                                        $(Object).find("#-" + counter).attr("CustomFormId", item.CustomFormId);

                                        if (item.Unit != null && item.Unit != '') {

                                            if (isNaN(parseInt(item.Unit))) {
                                                item.Unit = '1';
                                            } else {
                                                unit = parseInt(item.Unit);
                                                if (unit < 1 || unit > 999) {
                                                    item.Unit = '1';
                                                }
                                            }

                                            try {
                                                BillingInformation.loadCPTPointers(Object, SavedItem, counter, item);

                                            } catch (ex) {
                                                console.log(ex);
                                            }

                                            $(Object).find("#hfUnitsId-" + counter).val(item.Unit);
                                            $(Object).find("#txtUnits-" + counter).val(item.Unit);

                                        }
                                        else {
                                            BillingInformation.loadCPTPointers(Object, SavedItem, counter, item);
                                        }

                                        $(Object).find("#hfStartDate-" + counter).val(item.StartDate);
                                        $(Object).find("#hfEndDate-" + counter).val(item.EndDate);
                                        for (var i = 1; i <= 12; i++) {
                                            try {
                                                if (($(Object).find("#hfICDCode9_" + i).val().trim() == item.ICD9.trim() && $(Object).find("#hfICDDescription9_" + i).val().trim() == item.ICD9_Description.trim())
                                                    || ($(Object).find("#hfICDCode10_" + i).val().trim() == item.ICD10.trim() && $(Object).find("#hfICDDescription10_" + i).val().trim() == item.ICD10_Description.trim())
                                                    || ($(Object).find("#hfSNOMEDCode_" + i).val().trim() == item.SNOMEDID.trim() && $(Object).find("#hfSNOMEDDescription_" + i).val().trim() == item.SNOMED_DESCRIPTION.trim())) {

                                                    $(Object).find("#txtDxPointer1-" + counter).val(i);
                                                    break;
                                                }
                                            }
                                            catch (ex) {
                                                console.log(ex);
                                            }
                                        }
                                        try {
                                            BillingInformation.AddNewChargeRow(null, 'Add', null);
                                        } catch (ex) {
                                            console.log(ex);
                                        }
                                    }
                                    else {
                                        counter = counter + 1;
                                        $(Object).find("#hfCPTCode-" + counter).val(item.CPTCode);
                                        //to fill cpt fee
                                        $(Object).find("#hfFee-" + counter).val(item.Fee);
                                        // $(Object).find("#txtFee-" + counter).val(item.Fee);
                                        //
                                        $(Object).find("#hfCPTDescription-" + counter).val(item.CPT_DESCRIPTION);
                                        $(Object).find("#hfCPTSNOMEDCodeId-" + counter).val(item.SNOMEDID);
                                        $(Object).find("#hfCPTSNOMEDDescription-" + counter).val(item.SNOMED_DESCRIPTION);
                                        $(Object).find("#txtModifier1-" + counter).val(item.Modifier);
                                        $(Object).find("#txtCPT-" + counter).val(item.CPTCode + ' - ' + item.CPT_DESCRIPTION);
                                        utility.SetAutoCompleteSourceforValidate($(Object).find("#txtCPT-" + counter), item.CPTCode, item.CPT_DESCRIPTION);
                                        $(Object).find("#-" + counter).attr("IsLabBasedCPT", item.IsLabBasedCPT);
                                        $(Object).find("#-" + counter).attr("Type", item.BillType);
                                        $(Object).find("#-" + counter).attr("CheckBoxValue", item.BillingInfoTimeId);
                                        $(Object).find("#-" + counter).attr("CustomFormId", item.CustomFormId);

                                        if (item.Unit != null && item.Unit != '') {

                                            if (isNaN(parseInt(item.Unit))) {
                                                item.Unit = '1';
                                            } else {
                                                unit = parseInt(item.Unit);
                                                if (unit < 1 || unit > 999) {
                                                    item.Unit = '1';
                                                }
                                            }

                                            try {
                                                BillingInformation.loadCPTPointers(Object, SavedItem, counter, item);
                                            } catch (ex) {
                                                console.log(ex);
                                            }

                                            $(Object).find("#hfUnitsId-" + counter).val(item.Unit);
                                            $(Object).find("#txtUnits-" + counter).val(item.Unit);
                                        }
                                        else {
                                            BillingInformation.loadCPTPointers(Object, SavedItem, counter, item);
                                        }

                                        $(Object).find("#hfStartDate-" + counter).val(item.StartDate);
                                        $(Object).find("#hfEndDate-" + counter).val(item.EndDate);
                                        for (var i = 1; i <= 12; i++) {
                                            try {
                                                if ((item.ICD9 && $(Object).find("#hfICDCode9_" + i).val().trim() == item.ICD9.trim() && item.ICD9_Description && $(Object).find("#hfICDDescription9_" + i).val().trim() == item.ICD9_Description.trim())
                                                    || (item.ICD10 && $(Object).find("#hfICDCode10_" + i).val().trim() == item.ICD10.trim() && item.ICD10_Description && $(Object).find("#hfICDDescription10_" + i).val().trim() == item.ICD10_Description.trim())
                                                    || (item.SNOMEDID && $(Object).find("#hfSNOMEDCode_" + i).val().trim() == item.SNOMEDID.trim() && item.SNOMED_DESCRIPTION && $(Object).find("#hfSNOMEDDescription_" + i).val().trim() == item.SNOMED_DESCRIPTION.trim())) {

                                                    $(Object).find("#txtDxPointer1-" + counter).val(i);
                                                    break;
                                                }
                                            }
                                            catch (ex) {
                                                console.log(ex);
                                            }
                                        }
                                        try {
                                            BillingInformation.AddNewChargeRow(null, 'Add', null);
                                        } catch (ex) {
                                            console.log(ex);
                                        }
                                    }
                                }
                                else {
                                    if (!BillingInformation.params.FromCCM)
                                        utility.DisplayMessages('CPT already added.', 3);
                                }
                            });
                            try {
                                BillingInformation.putBillingInformationCPTid(BillingInformation.BillingInformation_DBResponse);

                            } catch (ex) {
                                console.log(ex);
                            }
                            // Get EMCode Suggestions
                            BillingInformation.GetEMCodeSuggestions().done(
                                function (resp) {
                                    resp = JSON.parse(resp);
                                    if (resp.status == true) {
                                        BillingInformation.CodeSuggestions = resp;
                                        var CodeExists = false;
                                        $.each(response.ProcedureListFill_JSON, function (i, item) {
                                            if (BillingInformation.CodeSuggestions.SuggestedCode == item.CPTCode) {
                                                CodeExists = true;
                                            }
                                        });
                                        var CPTs = [];
                                        CPTs = $.grep(response.ProcedureListFill_JSON, function (a) {
                                            if ((parseInt(a.CPTCode) >= 99201 && parseInt(a.CPTCode) <= 99205)) {
                                                return a.CPTCode;
                                            }
                                        });
                                        if (CPTs.length > 0) {
                                            CodeExists = true;
                                        }
                                        CPTs = $.grep(response.ProcedureListFill_JSON, function (a) {
                                            if ((parseInt(a.CPTCode) >= 99211 && parseInt(a.CPTCode) <= 99215)) {
                                                return a.CPTCode;
                                            }
                                        });
                                        if (CPTs.length > 0) {
                                            CodeExists = true;
                                        }
                                        if (!CodeExists) {
                                            BillingInformation.SuggestedCheckBox = $("#pnlBillingInformation #tblEMCodes input[Code=" + BillingInformation.CodeSuggestions.SuggestedCode + "]");
                                            if ((BillingInformation.SuggestedCheckBox != null || BillingInformation.SuggestedCheckBox != undefined) && (BillingInformation.params.AppointmentId != undefined) && BillingInformation.SuggestedCheckBox.length > 0) {
                                                if (!BillingInformation.params.FromCCM) {
                                                    //BillingInformation.SuggestedCheckBox.prop('checked', true);
                                                    //BillingInformation.SelectCheckBox(BillingInformation.SuggestedCheckBox[0]);

                                                    setTimeout(function () {
                                                        if (!BillingInformation.SuggestedCheckBox.prop('checked')) {
                                                            //BillingInformation.SuggestedCheckBox.prop('checked', true);
                                                        }
                                                    }, 1200);
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        utility.DisplayMessages(resp.message, 2);
                                    }

                                });

                            BillingInformation.EnableDisableICDs();
                        }
                        //this is because CPT and thier Fee is fetched upon screen launched.
                        if (BillingInformation.params.ParentCtrl != "billTabOutOfOfficeVisits" && BillingInformation.Status && BillingInformation.Status != "Signed") {
                            BillingInformation.Re_CalculateCPTFee();
                        }
                    });
                }
                else {

                    var Object = $('#' + BillingInformation.params.PanelID + ' #tblBillingInformation');
                    var NumberOfProblems = response.BillingInfoICDFill_JSON.length;
                    if (NumberOfProblems > 0) {
                        $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").html('');
                        if (NumberOfProblems > 12)
                            NumberOfProblems = 12;
                        for (var counter = 1; counter <= NumberOfProblems  ; counter++) {
                            var Html = '<div class="col-sm-3 col-md-3"><label class="control-label">Diagnosis ' + counter + '</label><div class="input-group"><input class="form-control" id="txtDisease_' + counter + '" oninput="BillingInformation.EnableDisableICDs();BillingInformation.bindICD9AutoComplete(this);" onblur="BillingInformation.ValidateICDCodeAndSetDesc(this);" onkeypress="    BillingInformation.disableEnterKey(event)" name="Disease_' + counter + '" type="text" value=""><div class="input-group-btn"><button id="btnDisease_' + counter + '" class="btn btn-primary btn-xs" type="button" onclick="BillingInformation.OpenSearchPopup(\'ICD\', \'txtDisease_' + counter + '\', \'hfICDCode9_' + counter + ',hfICDDescription9_' + counter + ',hfICDCode10_' + counter + ',hfICDDescription10_' + counter + ',hfSNOMEDCode_' + counter + ',hfSNOMEDDescription_' + counter + ',txtDisease_' + counter + '\')"><i class="glyphicon glyphicon-search"></i></button></div></div><input type="hidden" id="hfICDCode9_' + counter + '" name="ICDCode9_' + counter + '" /><input type="hidden" id="hfICDDescription9_' + counter + '" name="ICDDescription9_' + counter + '" /><input type="hidden" id="hfICDCode10_' + counter + '" name="ICDCode10_' + counter + '" /><input type="hidden" id="hfICDDescription10_' + counter + '" name="ICDDescription10_' + counter + '" /><input type="hidden" id="hfSNOMEDCode_' + counter + '" name="hfSNOMEDCode_' + counter + '" /><input type="hidden" id="hfSNOMEDDescription_' + counter + '" name="SNOMEDDescription_' + counter + '" /><input type="hidden" id="hfCustomFormId_' + counter + '" name="CustomFormId_' + counter + '" /></div>';
                            $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").append(Html);
                        }
                        if (NumberOfProblems == 12)
                            $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").append('<div class="clearfix spacer5"></div><div class="pull-left pl-default"><a class="btn btn-link btn-xs p-none disableAll" onclick="BillingInformation.AddMoreDiagnosis();">Add More</a></div>');
                        else
                            $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").append('<div class="clearfix spacer5"></div><div class="pull-left pl-default"><a class="btn btn-link btn-xs p-none" onclick="BillingInformation.AddMoreDiagnosis();">Add More</a></div>');
                    }
                    $(response.BillingInfoICDFill_JSON).each(function (index, item) {
                        var counter = index + 1;
                        if (item.ICDType == "10") {
                            $(Object).find("#hfICDCode9_" + counter).val('');
                            $(Object).find("#hfICDDescription9_" + counter).val('');
                            $(Object).find("#hfICDCode10_" + counter).val(item.ICDCode);
                            $(Object).find("#hfICDDescription10_" + counter).val(item.ICDCodeDescription);
                        }
                        else {
                            $(Object).find("#hfICDCode9_" + counter).val(item.ICDCode);
                            $(Object).find("#hfICDDescription9_" + counter).val(item.ICDCodeDescription);
                            $(Object).find("#hfICDCode10_" + counter).val('');
                            $(Object).find("#hfICDDescription10_" + counter).val('');
                        }

                        $(Object).find("#hfSNOMEDCode_" + counter).val(item.SNOMEDID);
                        $(Object).find("#hfSNOMEDDescription_" + counter).val(item.SNOMEDDescription);
                        $(Object).find("#txtDisease_" + counter).val(item.ICDCode + ' - ' + item.ICDCodeDescription);

                    });


                    isFromSecond = false;
                    var cptarr = response.BillingInfoCPTFill_JSON;
                    BillingInformation.EnableDisableICDs();
                    var counter = 1;
                    $(cptarr).each(function (index, item) {
                        counter = index + 1;

                        counter = counter * -1;
                        if (item.CPTDescription == '') {
                            item.CPTDescription = item.CPTCodeDescription;
                        }
                        item.CPTDescription = item.CPTDescription.replace(/&quot;/g, '"').replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&#39;/g, "'").replace(/&amp;/g, "&").replace(/\/\equal/g, '=');
                        $(Object).find("#hfCPTCode" + counter).val(item.CPTCode);
                        $(Object).find("#hfBillingInfoCPTId" + counter).val(item.BillingInfoCPTId ? item.BillingInfoCPTId : "");
                        $(Object).find("#hfCPTDescription" + counter).val(item.CPTDescription);
                        $(Object).find("#txtCPT" + counter).val(item.CPTCode + ' - ' + item.CPTDescription);
                        utility.SetAutoCompleteSourceforValidate($(Object).find("#txtCPT" + counter), item.CPTCode, item.CPTDescription);
                        $(Object).find("#txtModifier1" + counter).val(item.Modifier1);
                        $(Object).find("#txtModifier2" + counter).val(item.Modifier2);
                        $(Object).find("#txtModifier3" + counter).val(item.Modifier3);
                        $(Object).find("#txtModifier4" + counter).val(item.Modifier4);
                        $(Object).find("#txtDxPointer1" + counter).val(item.ICDPointer1);
                        $(Object).find("#txtDxPointer2" + counter).val(item.ICDPointer2);
                        $(Object).find("#txtDxPointer3" + counter).val(item.ICDPointer3);
                        $(Object).find("#txtDxPointer4" + counter).val(item.ICDPointer4);
                        $(Object).find("#hfUnitsId" + counter).val(item.Units);
                        $(Object).find("#txtUnits" + counter).val(item.Units);
                        $(Object).find("#-" + counter).attr("IsLabBasedCPT", item.IsLabBasedCPT);
                        $(Object).find("#" + counter).attr("Type", item.Type);
                        $(Object).find("#" + counter).attr("CheckBoxValue", item.BillingInfoTimeId);

                        if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                            $(Object).find("#dtpDOSFrom" + counter).val(utility.RemoveTimeFromDate(null, item.DOSFrom));
                            $(Object).find("#dtpDOSTo" + counter).val(utility.RemoveTimeFromDate(null, item.DOSTo));
                        }

                        try {
                            BillingInformation.AddNewChargeRow(null, 'Add', null);
                        } catch (ex) {
                            console.log(ex);
                        }
                    });
                    BillingInformation.EnableDisableICDs();
                    BillingInformation.ReorderCPTs();
                }
                BillingInformation.CheckPreviousStateofChkBoxs();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },


    Re_CalculateCPTFee: function () {
        var deffered = $.Deferred();
        var self = null;
        if (BillingInformation.params.PanelID.indexOf("pnlBillingInformation") < 0)
            self = $('#' + BillingInformation.params.PanelID + ' #pnlBillingInformation');
        else
            self = $('#' + BillingInformation.params.PanelID);

        var data_ = [];
        $(self).find("#dgvBillVisitCharge tbody tr").each(function (i, item) {
            var CPT = $(item).find('input[id*="txtCPT"]').val();
            if (CPT != "") {
                var provider = self.find('#hfProvider').val();
                var facility = self.find('#hfFacility').val();
                var practice = self.find('#hfPractice').val();
                if (!practice) {
                    practice = 0;
                }
                var patientInsuraceId = 0;
                if ($("#PatientProfile #hfPatientInsuranceId").val())
                    patientInsuraceId = $("#PatientProfile #hfPatientInsuranceId").val();
                var POSCode = self.find('#txtPOS').val() == "" ? 0 : self.find('#txtPOS').val();
                var Modifier1 = $(item).find('input[id*="txtModifier1"]').val();
                var Modifier2 = $(item).find('input[id*="txtModifier2"]').val();
                var Modifier3 = $(item).find('input[id*="txtModifier3"]').val();
                var Modifier4 = $(item).find('input[id*="txtModifier4"]').val();
                var cptCode = $(item).find("input[id*='hfCPTCode']").val();
                var ChargeDOS = $(item).find('input[id*="dtpDOSFrom"]').val();
                if (ChargeDOS == "") {
                    ChargeDOS = $.datepicker.formatDate(globalAppdata["DateFormat"].replace('yy', ''), new Date());
                }
                var ChargeId = $(item).attr('id');

                data_.push({
                    Provider: provider,
                    Facility: facility,
                    Practice: practice,
                    PatientInsuraceId: patientInsuraceId,
                    POSCode: POSCode,
                    CPT: cptCode,
                    Modifier1: Modifier1,
                    Modifier2: Modifier2,
                    Modifier3: Modifier3,
                    Modifier4: Modifier4,
                    ChargeDOS: ChargeDOS,
                    ChargeId: ChargeId
                });
            }
        });

        var jsonData = JSON.stringify(data_);
        BillingInformation.ReCalculateCPTFee(jsonData, BillingInformation.params.VisitId).done(function (response) {
            if (response.status != false) {

                var AllCPTJsonList = JSON.parse(response.AllCPTJsonList);
                $.each(AllCPTJsonList, function (i, item) {

                    var JItem = JSON.parse(item);
                    var $currentRow = $(self).find("#dgvBillVisitCharge tbody tr[id='" + JItem.ChargeId + "']");

                    if ($currentRow) {
                        $.when(BillingInformation.setCurrentFee(JItem, $currentRow)).then(function () {
                        });
                    }
                });
                deffered.resolve();
            }
            else {
                deffered.resolve();
            }
        });
        return deffered;
    },
    setCurrentFee: function (response, insertedRow) {
        var CPTLoad_detail = JSON.parse(response.CPTLoad_JSON);
        $.each(CPTLoad_detail, function (i, item) {
            var deffered = $.Deferred();
            var CurrentFee = item.Fee == "" ? 0 : item.Fee;
            var CurrentExpectedFee = item.ExpectedFee == "" ? 0 : item.ExpectedFee;
            var fee = parseFloat(CurrentFee);
            var Expectedfee = parseFloat(CurrentExpectedFee);

            var units = parseFloat($(insertedRow).find('input:text[id*="txtUnits"]').val());
            units = !units ? 1 : units;

            $(insertedRow).find('input[id*="txtFEE"]').val(fee);
            $(insertedRow).find('input[id*="txtTotalFEE"]').val(fee * units);
            $(insertedRow).find('input[id*="txtUnits"]').trigger("blur");
            $(insertedRow).find('input[id*="hfFee"]').val(fee);
            $(insertedRow).find('input[id*="hfExpectedFee"]').val(Expectedfee);
            $(insertedRow).find('input[id*="txtExpectedFee"]').val((Expectedfee * units).toFixed(2));
            $(insertedRow).find('input[id*="txtTotalFEE"]').val((units * fee).toFixed(2));
            if (item.Modifier1 && $(insertedRow).find('input[id*="txtModifier1"]').val() == "") {
                $(insertedRow).find('input[id*="txtModifier1"]').val(item.Modifier1);
            }
            if (item.Modifier2 && $(insertedRow).find('input[id*="txtModifier2"]').val() == "") {
                $(insertedRow).find('input[id*="txtModifier2"]').val(item.Modifier2);
            }
            if (item.Modifier3 && $(insertedRow).find('input[id*="txtModifier3"]').val() == "") {
                $(insertedRow).find('input[id*="txtModifier3"]').val(item.Modifier3);
            }
            if (item.Modifier4 && $(insertedRow).find('input[id*="txtModifier4"]').val() == "") {
                $(insertedRow).find('input[id*="txtModifier4"]').val(item.Modifier4);
            }
            deffered.resolve();
            return deffered;
        });
    },
    ReCalculateCPTFee: function (data, VisitId) {

        var Data = "CPTData=" + data + "&VisitId=" + VisitId;
        return MDVisionService.defaultService(Data, "PATIENT_CHARGE_CAPTURE", "FILL_ALL_CPT_FEE");
    },
    loadCPTPointers: function (Object, SavedItem, counter, item) {
        if (item.IsLabBasedCPT == "True") {
            $(Object).find("#txtModifier1-" + counter).val(item.Modifier);
            $(Object).find("#txtModifier2-" + counter).val(item.Modifier2);
            $(Object).find("#txtModifier3-" + counter).val(item.Modifier3);
            $(Object).find("#txtModifier4-" + counter).val(item.Modifier4);
            item.ICDPointer1 ? $(Object).find("#txtDxPointer1-" + counter).val(item.ICDPointer1) : $(Object).find("#txtDxPointer1-" + counter).val("1");
            $(Object).find("#txtDxPointer2-" + counter).val(item.ICDPointer2);
            $(Object).find("#txtDxPointer3-" + counter).val(item.ICDPointer3);
            $(Object).find("#txtDxPointer4-" + counter).val(item.ICDPointer4);
        }
        else if (SavedItem != null && SavedItem.length > 0) {
            $(Object).find("#txtModifier1-" + counter).val(SavedItem[0].Modifier1);
            $(Object).find("#txtModifier2-" + counter).val(SavedItem[0].Modifier2);
            $(Object).find("#txtModifier3-" + counter).val(SavedItem[0].Modifier3);
            $(Object).find("#txtModifier4-" + counter).val(SavedItem[0].Modifier4);
            $(Object).find("#txtDxPointer1-" + counter).val(SavedItem[0].ICDPointer1);
            $(Object).find("#txtDxPointer2-" + counter).val(SavedItem[0].ICDPointer2);
            $(Object).find("#txtDxPointer3-" + counter).val(SavedItem[0].ICDPointer3);
            $(Object).find("#txtDxPointer4-" + counter).val(SavedItem[0].ICDPointer4);
        }
        else {
            $(Object).find("#txtModifier1-" + counter).val(item.Modifier);
            $(Object).find("#txtModifier2-" + counter).val("");
            $(Object).find("#txtModifier3-" + counter).val("");
            $(Object).find("#txtModifier4-" + counter).val("");
            var icd_cods = item.ICDCodes.split(',');
            var pinter1 = icd_cods.length > 0 && icd_cods[0] != '' ? BillingInformation.Getpointers(icd_cods[0]) : '1';
            var pinter2 = icd_cods.length > 1 && icd_cods[1] != '' ? BillingInformation.Getpointers(icd_cods[1]) : '';
            var pinter3 = icd_cods.length > 2 && icd_cods[2] != '' ? BillingInformation.Getpointers(icd_cods[2]) : '';
            var pinter4 = icd_cods.length > 3 && icd_cods[3] != '' ? BillingInformation.Getpointers(icd_cods[3]) : '';

            $(Object).find("#txtDxPointer1-" + counter).val(pinter1);
            $(Object).find("#txtDxPointer2-" + counter).val(pinter2);
            $(Object).find("#txtDxPointer3-" + counter).val(pinter3);
            $(Object).find("#txtDxPointer4-" + counter).val(pinter4);
        }
    },

    Getpointers: function (code) {
        var toReturn = '';
        $("#dvDiagnosis :input[name*='ICDCode10']").each(function () {
            if (code.trim() == $(this).val().trim()) {
                toReturn = $(this).attr('id').split('_')[1];
                return false;
            }
        });
        return toReturn.toString();
    },

    FillCPTFee: function (VisitId, CPTCode, ProviderId, FacilityId, PracticeId, patientInsuraceId, POSCode, Modifier1, Modifier2, Modifier3, Modifier4, ChargeDOS) {
        if (VisitId == null) {
            VisitId = 0;
        }
        if (CPTCode == null) {
            CPTCode = "";
        } else {
            CPTCode = CPTCode.split('-')[0];
        }
        if (FacilityId == null) {
            FacilityId = 0;
        }
        if (PracticeId == null) {
            PracticeId = 0;
        }
        if (patientInsuraceId == null) {
            patientInsuraceId = 0;
        }
        if (POSCode == null) {
            POSCode = "";
        }
        if (Modifier1 == null) {
            Modifier1 = "";
        }
        if (Modifier2 == null) {
            Modifier2 = "";
        }
        if (Modifier3 == null) {
            Modifier3 = "";
        }
        if (Modifier4 == null) {
            Modifier4 = "";
        }
        if (ChargeDOS == null) {
            ChargeDOS = "";
        }
        var data = "VisitId=" + VisitId + "&CPTCode=" + CPTCode + "&ProviderId=" + ProviderId + "&FacilityId=" + FacilityId + "&PracticeId=" + PracticeId
                    + "&patientInsuraceId=" + patientInsuraceId + "&POSCode=" + POSCode + "&Modifier1=" + Modifier1 + "&Modifier2=" + Modifier2
                    + "&Modifier3=" + Modifier3 + "&Modifier4=" + Modifier4 + "&ChargeDOS=" + ChargeDOS;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "FILL_CPT_FEE");
    },

    AddMoreDiagnosis: function () {
        var NumberOfDiv = $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").find("div label").length;

        $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").find(".btn-link, .clearfix").remove();
        if (NumberOfDiv < 12) {
            var counter = NumberOfDiv + 1;
            var Html = '<div class="col-sm-3 col-md-3"><label class="control-label">Diagnosis ' + counter + '</label><div class="input-group"><input class="form-control" id="txtDisease_' + counter + '" oninput="BillingInformation.EnableDisableICDs();BillingInformation.bindICD9AutoComplete(this);" onblur="BillingInformation.ValidateICDCodeAndSetDesc(this);" onkeypress="    BillingInformation.disableEnterKey(event)" name="Disease_' + counter + '" type="text" value=""><div class="input-group-btn"><button id="btnDisease_' + counter + '" class="btn btn-primary btn-xs" type="button" onclick="BillingInformation.OpenSearchPopup(\'ICD\', \'txtDisease_' + counter + '\', \'hfICDCode9_' + counter + ',hfICDDescription9_' + counter + ',hfICDCode10_' + counter + ',hfICDDescription10_' + counter + ',hfSNOMEDCode_' + counter + ',hfSNOMEDDescription_' + counter + ',txtDisease_' + counter + '\')"><i class="glyphicon glyphicon-search"></i></button></div></div><input type="hidden" id="hfICDCode9_' + counter + '" name="ICDCode9_' + counter + '" /><input type="hidden" id="hfICDDescription9_' + counter + '" name="ICDDescription9_' + counter + '" /><input type="hidden" id="hfICDCode10_' + counter + '" name="ICDCode10_' + counter + '" /><input type="hidden" id="hfICDDescription10_' + counter + '" name="ICDDescription10_' + counter + '" /><input type="hidden" id="hfSNOMEDCode_' + counter + '" name="hfSNOMEDCode_' + counter + '" /><input type="hidden" id="hfSNOMEDDescription_' + counter + '" name="SNOMEDDescription_' + counter + '" /><input type="hidden" id="hfCustomFormId_' + counter + '" name="CustomFormId_' + counter + '" /></div>';
            $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").append(Html);
        }
        if (NumberOfDiv >= 11) {
            $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").append('<div class="clearfix spacer5"></div><div class="pull-left pl-default"><a class="btn btn-link btn-xs p-none disableAll" onclick="BillingInformation.AddMoreDiagnosis();">Add More</a></div>');
        }
        else {
            $("#" + BillingInformation.params.PanelID + " #dvDiagnosis").append('<div class="clearfix spacer5"></div><div class="pull-left pl-default"><a class="btn btn-link btn-xs p-none" onclick="BillingInformation.AddMoreDiagnosis();">Add More</a></div>');
        }

        BillingInformation.EnableDisableICDs();
    },




    LoadAttachecdICDsAndCPTs: function () {
        var objData = new Object();
        objData["NotesId"] = BillingInformation.params.NotesId;
        objData["PatientId"] = BillingInformation.params.PatientId;
        objData["commandType"] = "LOADATTACHEDPROCEDURESANDPROBLEMS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");

    },

    LoadAttachecdICDsAndCPTsForSign: function () {
        var objData = new Object();
        objData["BillingInfoId"] = BillingInformation.params.BillingInfoId;
        objData["NotesId"] = BillingInformation.params.NotesId;
        objData["PatientId"] = BillingInformation.params.PatientId;
        objData["commandType"] = "LOADATTACHEDPROCEDURESANDPROBLEMSFORSIGN";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");

    },

    BillingInformationLoad: function () {
        var objData = new Object();
        objData["BillingInfoId"] = BillingInformation.params.BillingInfoId;
        objData["commandType"] = "BILLING_INFORMATION_SELECT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },


    CheckDuplicateCPT: function (CurrentCPT) {
        IsDuplicate = false;
        var CurrentRow = $(CurrentCPT).closest('tr');
        var CurrentDOSFrom = $(CurrentRow).find("input[id*=dtpDOSFrom]");
        var CurrentDOSTo = $(CurrentRow).find("input[id*=dtpDOSTo]");

        $('input[id^="txtCPT"]').each(function () {
            var thisRow = $(this).closest('tr');
            if ($(CurrentRow).attr("id") && $(thisRow).attr("id") && $(CurrentRow).attr("id") != $(thisRow).attr("id")) {
                var ThisDOSFrom = $(thisRow).find("input[id*=dtpDOSFrom]");
                var ThisDOSTo = $(thisRow).find("input[id*=dtpDOSTo]");
                if (ThisDOSFrom.val() == CurrentDOSFrom.val() && ThisDOSTo.val() == CurrentDOSTo.val() && $(this).val() == $(CurrentCPT).val()) {
                    IsDuplicate = true;
                }
            }
        });

        return IsDuplicate;
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will split Ids on '-' from array
    stripIdsFromArray: function (array) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item.split('-')[0];
        });
        return resultantArray;
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will enable disable multiselect ddls provided
    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + BillingInformation.params["PanelID"];
        $.each(ddlCommaSeparatedIds, function (index, Item) {
            if (isHide) {
                $(parrentPanelId + " #" + Item).multiselect('disable');
            }
            else {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            }
        });
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will remove item from the "array and item" provided as input args
    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will load entity based provider
    loadEntityProvider: function (entityId) {

        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + BillingInformation.params.PanelID + ' #ddlPhysicalExamTemplateProvider');
                var $providerHiddenDdl = $('#' + BillingInformation.params.PanelID + ' #ddlHiddenPhysicalExamTemplateProvider');

                //Empty both the providers ddls.
                $providerDdl.empty();
                $providerHiddenDdl.empty();

                //Loop through all providers loaded from the server
                $.each(options, function (i, item) {
                    if (item.Value != "" && typeof item.Value != 'undefined') {

                        // User will see these providers in multiSelect dropdownlist
                        $providerDdl.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                                refname: item.RefName,
                                refvalue: item.RefValue

                            })
                        );
                        // Populate hidden ddl provider
                        //A Hack to load all the providers in hidden dropdownlist
                        $providerHiddenDdl.append(
                             $('<option/>', {
                                 value: item.Value,
                                 html: item.Name,
                                 refname: item.RefName,
                                 refvalue: item.RefValue

                             })
                        );
                    }
                });
                // Assigned server side providers to providerCheckedIds array and made selected
                if (BillingInformation.ProviderIds != '') {
                    var Providers = BillingInformation.ProviderIds.split(",");
                    BillingInformation.providerCheckedIds = Providers;
                    $('#' + BillingInformation.params.PanelID + ' #ddlPhysicalExamTemplateProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect
                $('#' + BillingInformation.params.PanelID + ' #divPhysicalExamTemplateSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.
                BillingInformation.IntializeMultiSelectDropDownProviders();
            });
            //enable multiselect
            BillingInformation.enableDisableDropDownLists('ddlPhysicalExamTemplateProvider', false);
        }
        else {
            //disable multiselect
            BillingInformation.enableDisableDropDownLists('ddlPhysicalExamTemplateProvider', true);
        }
    },


    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will perform tasks on document ready event
    domReady: function () {
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
    },
    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will return refname using (li's input value equals ddl option value)
    getRefValuefromDdl: function (ddlId, liId) {
        var $ddlOptions = $('#' + BillingInformation.params.PanelID + " #" + ddlId).find('option');
        var value = null;
        $ddlOptions.each(function () {

            if ($(this).attr('value') == liId) {
                value = $(this).attr('refname');
                return false;
            }
        });
        return value;
    },


    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will handle toggling of +ve/-ve checkboxes

    toggleCheckBoxes: function (chkObject) {
        if (chkObject != null) {
            BillingInformation.isBothUnCheck = false;
            var isChecked = $(chkObject).prop("checked");
            //Start//25-02-2016//ahmad Raza//Logic to delete subCharacteristic on uncheck
            if ($(chkObject).parent().parent().parent().attr('id') == 'ulExamSubCharacteristics' && isChecked == false) {
                var subCharId = $(chkObject).attr('name');
                var subCharPKId = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #ulExamSubCharacteristics li#" + subCharId).data('SystemSubCharacteristicPk_' + subCharId);
                var physicalExamId = $('#' + BillingInformation.params.PanelID + " #hfPhysicalExamId").val();
                var charId = $(chkObject).parent().parent().attr('parentid');
                if (subCharPKId != '' && subCharPKId != null) {
                    BillingInformation.deleteSubCharacteristicDetail('Characteristics', charId, physicalExamId, subCharPKId, subCharId);
                }
            }
            //End//25-02-2016//ahmad Raza//Logic to delete subCharacteristic on uncheck
            /*

                        //Start//09-02-2016//Ahmad Raza//implimentation of adding color to system and sections
                        var characteristics = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics input[type=checkbox]:checked").length;
                        var subCharacteristics = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamSubCharacteristics input[type=checkbox]:checked").length;
                        if (characteristics > 0 || subCharacteristics > 0) {
                            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystems li.active").addClass('green');
                            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection li.active").addClass('green');
                        }
                        else {
                            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystems li.active").removeClass('green');
                            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection li.active").removeClass('green');
                        }
                        //End//09-02-2016//Ahmad Raza//implimentation of adding color to system and sections
            */
            // var array = [];
            var examId = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystems li.active").attr('id');
            var finalexam = examId + "exam";
            var sectionId = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection li.active").attr('id');
            var finalsectionids = [];
            if (BillingInformation.array.indexOf(sectionId) < 0) {
                finalsectionids.push(sectionId + "section");
            }
            //var finalsection = sectionId + "section";

            var currentId = $(chkObject).attr("id");
            var character = [];
            if (BillingInformation.array.indexOf(currentId) < 0) {
                character.push(currentId + "character");
            }
            // var character = currentId;

            BillingInformation.array.push(finalexam, finalsectionids, character);
            var parentObj = $(chkObject).parent();
            parentObj.find("input[type='checkbox']").each(function (i, item) {
                $(item).prop("checked", false);
            });
            parentObj.find("input[id='" + currentId + "']").prop("checked", isChecked);
            if (isChecked == true) {
                //Start 12-02-2016 Humaira Yousaf
                $(parentObj).find('#btnShowSubCharacteristics' + $(chkObject).attr("name")).removeAttr('disabled');
                //End 12-02-2016 Humaira Yousaf
                var DetailExists = BillingInformation.isDetailsHaveData();;
                var isExist = BillingInformation.isDetailsHaveData();
                if (isExist == true) {
                    //var currentIdPR = $(parentObj).parent().parent().attr("id");
                    //if (currentIdPR != null && currentIdPR != "")
                    //    BillingInformation.setHiddenFieldValues(currentIdPR, currentId, parentObj);
                    var self = $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails')
                    var myJSON = self != null ? self.getMyJSONByName() : "{}";

                    var selectedJSON = JSON.parse(myJSON);

                    //arrSelectedJSON.push(selectedJSON);
                    var selectedLiId = BillingInformation.getNumberPart(selectedJSON)
                    if (selectedLiId != null && selectedLiId != "") {
                        if (BillingInformation.ExamDetails[selectedLiId] != null) {
                            //if (JSON.parse(BillingInformation.ExamDetails[selectedLiId])['SectionId' + selectedLiId] == selectedJSON['SectionId' + selectedLiId] &&
                            //    JSON.parse(BillingInformation.ExamDetails[selectedLiId])['SystemId' + selectedLiId] == selectedJSON['SystemId' + selectedLiId] &&
                            //    (selectedLiId == selectedJSON['CharacteristicId' + selectedLiId] || selectedLiId == selectedJSON['SubCharacteristicId' + selectedLiId]))
                            BillingInformation.ExamDetails[selectedLiId] = BillingInformation.ExamDetails[selectedLiId].replace(BillingInformation.ExamDetails[selectedLiId], myJSON);
                        }
                        else {
                            BillingInformation.ExamDetails[selectedLiId] = myJSON;

                        }
                    }

                    //if ($.inArray(BillingInformation.ExamDetails, selectedLiId) < 0) {
                    //    BillingInformation.ExamDetails[selectedLiId] = myJSON;
                    //}

                    //utility.DisplayMessages($(parentObj).parent().parent().attr("id"), 2);
                }

                ////$(parentObj).parent().addClass("active");
                //BillingInformation.markStatusActive($(parentObj).parent().parent().attr("id"), $(parentObj).parent().attr("id"));
                ////Show Details Section
                //BillingInformation.showHideToggleDetails(true);
            }
            else {
                $(chkObject).closest('li').removeClass("green");
                $(parentObj).parent().removeClass("active");
                //Start 12-02-2016 Humaira Yousaf
                $(parentObj).find('#btnShowSubCharacteristics' + $(chkObject).attr("name")).attr('disabled', 'disabled');
                //End 12-02-2016 Humaira Yousaf
                var selectedLiId = $(parentObj).parent().attr("id");
                // BillingInformation.toggleDetailsDiv(selectedLiId, true);

                //Start Farooq Ahmad 25/02/2016
                if (selectedLiId != null && selectedLiId != "") {

                    //  BillingInformation.deleteFromJsonObject($(parentObj).parent().parent().attr('id'), selectedLiId);

                }

                //End Farooq Ahmad 25/2/2016

            }

            // check selecteall for +ve checkbox if all child are checked
            var isAllPstiveChecked = false;
            var allPstivechk = parentObj.parent().parent().find("input[id*='+ve']").not("input[id='chkSelectAll+ve']");
            if (allPstivechk.filter(":checked").length == allPstivechk.length) {
                parentObj.parent().parent().find("input[id='chkSelectAll+ve']").prop("checked", true);
            }
            else {
                parentObj.parent().parent().find("input[id='chkSelectAll+ve']").prop("checked", false);
            }

            // check selecteall for -ve checkbox if all child are checked
            var allNgtivechk = parentObj.parent().parent().find("input[id*='-ve']").not("input[id='chkSelectAll-ve']");
            if (allNgtivechk.filter(":checked").length == allNgtivechk.length) {
                parentObj.parent().parent().find("input[id='chkSelectAll-ve']").prop("checked", true);
            }
            else {
                parentObj.parent().parent().find("input[id='chkSelectAll-ve']").prop("checked", false);
            }

            var currentIdPR = $(parentObj).parent().parent().attr("id");
            if (currentIdPR != null && currentIdPR != "")
                BillingInformation.setHiddenFieldValues(currentIdPR, currentId, parentObj);

        }
    },

    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will check if any characteristic/subcharacteristic has data in details section
    isDetailsHaveData: function () {
        var DetailExists = false;
        var sectionDetails = "";
        var self = $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails').find('[type=hidden],[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
            if ($(this).prop("disabled") != true && DetailExists == false) {
                var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'checkbox' && this.checked == true) {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'radio' && $(this).attr('id').toLowerCase().indexOf("no") > -1 && this.checked == true) {
                    DetailExists = false;
                }
                if (currentElementTagName.toLowerCase() == 'select' && $(this).val() != null && $(this).val() != "") {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'hidden' && $(this).val() != "" && $(this).val() != "-1") {
                    DetailExists = true;
                }
                //if (currentElementTagName.toLowerCase() == 'ul') {
                //    $(this).find('li.active').removeClass('active');
                //}
            }
        });

        return DetailExists;
    },

    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will check if any characteristic/subcharacteristic has any value selected
    isDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";


        var self = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #PhysicalExam");
        var objCharacteristic = self.find("div#divExamCharacteristics");
        var objSubCharacteristic = self.find("section#CharacteristicsSubCharacteristics");
        if (objSubCharacteristic.hasClass("hidden") == false) {
            DetailExists = BillingInformation.isSystemChecked(objSubCharacteristic.find("ul#ulExamSubCharacteristics"));
            if (DetailExists == false) {
                DetailExists = BillingInformation.isSystemChecked(objCharacteristic.find("ul#ulExamCharacteristics"));
            }
        }
        else if (objCharacteristic.hasClass("hidden") == false) {
            DetailExists = BillingInformation.isSystemChecked(objCharacteristic.find("ul#ulExamCharacteristics"));
        }
        return DetailExists;

    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will return comma Separated Ids of either selected Characteristics/SubCharacteristics from Given JSON on basis of characteristics Type as parameter
    getCommaSeparatedIds: function (arrSelectedJSON, IsSubCharacteristic) {
        var selectedIds = "";
        var isFirstSelected = false;
        //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
        if (IsSubCharacteristic == true) {
            BillingInformation.subcharacteristicsWithData.length = 0;
            BillingInformation.selectedsubcharacteristicsIds.length = 0;
        }
        else if (IsSubCharacteristic == false) {
            BillingInformation.characteristicsWithData.length = 0;
            BillingInformation.selectedcharacteristicsIds.length = 0;
        }
        //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
        if (arrSelectedJSON != null)
            $.each(arrSelectedJSON, function (i, item) {
                var numericPart = 0;
                for (key in item) {
                    var myval = key;
                    if (key.indexOf("SubCharacteristicId") > -1) {
                        numericPart = key.replace(/[^\d]+/, '');
                    }
                }

                //var currentSubCharacteristicId = item["SubCharacteristicId" + numericPart];
                //if (currentSubCharacteristicId != null && currentSubCharacteristicId != "" && parseInt(currentSubCharacteristicId) > 0) {
                //    //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                //    BillingInformation.subcharacteristicsWithData.push(item);
                //    //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                //}
                //else if (currentCharacteristicId != null && currentCharacteristicId != "" && parseInt(currentCharacteristicId) > 0) {
                //    //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                //    BillingInformation.characteristicsWithData.push(item);
                //    //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                //}

                if (IsSubCharacteristic == true) {
                    var currentSubCharacteristicId = item["SubCharacteristicId" + numericPart];
                    var currentCharacteristicId = item["CharacteristicId" + numericPart];
                    if (currentSubCharacteristicId != null && currentSubCharacteristicId != "" && parseInt(currentSubCharacteristicId) > 0) {
                        if (isFirstSelected == false) {
                            selectedIds = currentSubCharacteristicId;
                            isFirstSelected = true;
                        }
                        else if (isFirstSelected == true) {
                            selectedIds += "," + currentSubCharacteristicId;
                        }
                        //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                        var num = BillingInformation.getNumberPart(item);

                        var Index = BillingInformation.selectedsubcharacteristicsIds.indexOf(num);
                        if (Index == -1 && item["CharacteristicId" + num] != num) {
                            BillingInformation.subcharacteristicsWithData.push(item);
                            BillingInformation.selectedsubcharacteristicsIds.push(num);
                        }


                        //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                    }
                }
                else {
                    var currentSubCharacteristicId = item["CharacteristicId" + numericPart];
                    if (item["CharacteristicId" + numericPart] != "") {
                        var currentCharacteristicId = item["CharacteristicId" + numericPart];
                        if (currentCharacteristicId != null && currentCharacteristicId != "" && parseInt(currentCharacteristicId) > 0) {
                            if (isFirstSelected == false) {
                                selectedIds = currentCharacteristicId;
                                isFirstSelected = true;
                            }
                            else if (isFirstSelected == true) {
                                selectedIds += "," + currentCharacteristicId;
                            }
                            var currentSubCharacteristicId = item["SubCharacteristicId" + numericPart];
                            if (currentSubCharacteristicId == "" || currentSubCharacteristicId == "-1") {
                                //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                                BillingInformation.characteristicsWithData.push(item);
                                BillingInformation.selectedcharacteristicsIds.push(currentCharacteristicId);
                                //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                            }

                        }
                    }
                }
            });

        return selectedIds;
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle load of child of either Systems/Section/Characteristics

    loadChildData: function (parentId, parentType) {
        var objDeffered = $.Deferred();
        if (parentType != null && parentType.toLowerCase() == "system") {

        }
        else if (parentType != null && parentType.toLowerCase() == "section") {

        }
        else if (parentType != null && parentType.toLowerCase() == "characteristics") {

        }
        else if (parentType != null && parentType.toLowerCase() == "subcharacteristics") {

        }
        BillingInformation.loadPhysicalExamStatuses(parentId, parentType).done(function () {

            objDeffered.resolve();
        });
        return objDeffered;
    },

    //Author: Muhammad Arshad
    //Date: 01-03-2016
    //This function will mark parent control as checked

    selectParentControls: function (chkObject) {
        if (chkObject != null) {
            var isChecked = $(chkObject).prop("checked");
            //if (isChecked == true) {

            //Start Farooq Ahmad 02-03-2016 Store the Selected Items in Json Object
            if ($(chkObject).closest("ul").attr("id") == "ulPhysicalExamSystems") {
                var obj = {
                    SystemId: $(chkObject).closest("li").attr('id'),
                    IsChecked: isChecked,
                    SystemName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                    Sections: []
                };
                var isUpdated = false;
                for (var counter in BillingInformation.selectedPhyExamTempData) {
                    if (BillingInformation.selectedPhyExamTempData[counter].SystemId == obj.SystemId) {

                        if (!isChecked) {
                            for (var innerIndex in BillingInformation.selectedPhyExamTempData[counter].Sections) {
                                BillingInformation.selectedPhyExamTempData[counter].Sections[innerIndex].IsChecked = isChecked;
                                for (var mostInnerIndex in BillingInformation.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics) {
                                    BillingInformation.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].IsChecked = isChecked;
                                    for (var innercounter in BillingInformation.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics) {
                                        BillingInformation.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[innercounter].IsChecked = isChecked;
                                    }
                                }
                            }
                            $('#' + BillingInformation.params.PanelID + ' #ulPhysicalExamSystemSection').find('input[type=checkbox]').prop("checked", false);
                            $('#' + BillingInformation.params.PanelID + ' #ulExamCharacteristics').find('input[type=checkbox]').prop("checked", false);
                            $('#' + BillingInformation.params.PanelID + ' #ulExamSubCharacteristics').find('input[type=checkbox]').prop("checked", false);

                        }
                        obj.Sections = BillingInformation.selectedPhyExamTempData[counter].Sections;
                        BillingInformation.selectedPhyExamTempData[counter] = obj;
                        isUpdated = true;
                    }
                }

                if (!isUpdated) {
                    if (BillingInformation.selectedPhyExamTempData == null)
                        BillingInformation.selectedPhyExamTempData = [];
                    BillingInformation.selectedPhyExamTempData.push(obj);
                }


            } else if ($(chkObject).closest("ul").attr("id") == "ulPhysicalExamSystemSection") {

                var isParentExist = false;
                for (index = 0; index < BillingInformation.selectedPhyExamTempData.length; index++) {
                    if (BillingInformation.selectedPhyExamTempData[index].SystemId == $(chkObject).closest("li").attr('parentid')) {
                        var obj = {
                            SystemId: $(chkObject).closest("li").attr('parentid'),
                            SectionId: $(chkObject).closest("li").attr('id'),
                            IsChecked: isChecked,
                            SectionName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                            Characteristics: []
                        };
                        var isUpdated = false;
                        for (var counter in BillingInformation.selectedPhyExamTempData[index].Sections) {
                            if (BillingInformation.selectedPhyExamTempData[index].Sections[counter].SectionId == obj.SectionId) {

                                if (!isChecked) {

                                    for (var mostInnerIndex in BillingInformation.selectedPhyExamTempData[index].Sections[counter].Characteristics) {
                                        BillingInformation.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].IsChecked = isChecked;
                                        for (var innercounter in BillingInformation.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].SubCharacteristics) {
                                            BillingInformation.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].SubCharacteristics[innercounter].IsChecked = isChecked;
                                        }
                                    }

                                    $('#' + BillingInformation.params.PanelID + ' #ulExamCharacteristics').find('input[type=checkbox]').prop("checked", false);
                                    $('#' + BillingInformation.params.PanelID + ' #ulExamSubCharacteristics').find('input[type=checkbox]').prop("checked", false);
                                }
                                obj.Characteristics = BillingInformation.selectedPhyExamTempData[index].Sections[counter].Characteristics;
                                BillingInformation.selectedPhyExamTempData[index].Sections[counter] = obj;
                                isUpdated = true;
                            }
                        }
                        if (!isUpdated) {
                            if (BillingInformation.selectedPhyExamTempData[index].Sections == null)
                                BillingInformation.selectedPhyExamTempData[index].Sections = [];
                            BillingInformation.selectedPhyExamTempData[index].Sections.push(obj);
                        }

                        isParentExist = true;
                    }
                }
                if (!isParentExist) {
                    var sectionId = $(chkObject).closest("li").attr('id');
                    var systemId = $(chkObject).closest("li").attr('parentid');
                    var systemName = $('#' + BillingInformation.params.PanelID + ' #ulPhysicalExamSystems #lblName' + systemId).text();//  $(chkObject).closest("li").attr('parentid');
                    var system = {
                        SystemId: systemId,
                        IsChecked: isChecked,
                        SystemName: systemName,
                        Sections: [{
                            SystemId: $(chkObject).closest("li").attr('parentid'),
                            SectionId: sectionId,
                            IsChecked: isChecked,
                            SectionName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                            Characteristics: []
                        }]
                    };
                    if (BillingInformation.selectedPhyExamTempData == null)
                        BillingInformation.selectedPhyExamTempData = [];
                    BillingInformation.selectedPhyExamTempData.push(system);
                }

            } else if ($(chkObject).closest("ul").attr("id") == "ulExamCharacteristics") {

                var isParentExist = false;
                for (index = 0; index < BillingInformation.selectedPhyExamTempData.length; index++) {
                    for (var innerIndex in BillingInformation.selectedPhyExamTempData[index].Sections) {
                        if (BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].SectionId == $(chkObject).closest("li").attr('parentid')) {
                            var obj = {
                                SectionId: $(chkObject).closest("li").attr('parentid'),
                                CharacteristicId: $(chkObject).closest("li").attr('id'),
                                CharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                IsChecked: isChecked,
                                SubCharacteristics: []
                            };
                            var isUpdated = false;
                            for (var counter in BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                                if (BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].CharacteristicId == obj.CharacteristicId) {


                                    if (!isChecked) {
                                        for (var innercounter in BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].SubCharacteristics) {
                                            BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].SubCharacteristics[innercounter].IsChecked = isChecked;
                                        }
                                        $('#' + BillingInformation.params.PanelID + ' #ulExamSubCharacteristics').find('input[type=checkbox]').prop("checked", false);
                                    }

                                    obj.SubCharacteristics = BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].SubCharacteristics;


                                    BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter] = obj;
                                    isUpdated = true;
                                }
                            }
                            if (!isUpdated) {
                                if (BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics == null)
                                    BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics = [];
                                BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics.push(obj);
                            }

                            isParentExist = true;
                        }
                    }
                }
                if (!isParentExist) {
                    var sectionId = $(chkObject).closest("li").attr('parentid');
                    var systemId = $('#' + BillingInformation.params.PanelID + " #ulPhysicalExamSystemSection li#" + sectionId).attr('parentid');
                    var systemName = $('#' + BillingInformation.params.PanelID + ' #ulPhysicalExamSystems #lblName' + systemId).text();
                    var sectionName = $('#' + BillingInformation.params.PanelID + ' #ulPhysicalExamSystemSection #lblName' + sectionId).text();
                    for (index = 0; index < BillingInformation.selectedPhyExamTempData.length; index++) {
                        if (BillingInformation.selectedPhyExamTempData.SystemId == systemId) {
                            var section = {
                                SystemId: systemId,
                                SectionId: sectionId,
                                IsChecked: isChecked,
                                SectionName: sectionName,
                                Characteristics: [{
                                    SectionId: $(chkObject).closest("li").attr('parentid'),
                                    CharacteristicId: $(chkObject).closest("li").attr('id'),
                                    CharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                    IsChecked: isChecked,
                                    SubCharacteristics: []
                                }]
                            };
                            if (BillingInformation.selectedPhyExamTempData[index].Sections == null)
                                BillingInformation.selectedPhyExamTempData[index].Sections = [];
                            BillingInformation.selectedPhyExamTempData[index].Sections.push(section);
                            isParentExist = true;
                        }
                    }
                    if (!isParentExist) {
                        var system = {
                            SystemId: systemId,
                            IsChecked: isChecked,
                            SystemName: systemName,
                            Sections: [{
                                SystemId: systemId,
                                SectionId: sectionId,
                                IsChecked: isChecked,
                                SectionName: sectionName,
                                Characteristics: [{
                                    SectionId: $(chkObject).closest("li").attr('parentid'),
                                    CharacteristicId: $(chkObject).closest("li").attr('id'),
                                    CharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                    IsChecked: isChecked,
                                    SubCharacteristics: []
                                }]
                            }]
                        };
                        if (BillingInformation.selectedPhyExamTempData == null)
                            BillingInformation.selectedPhyExamTempData = [];
                        BillingInformation.selectedPhyExamTempData.push(system);
                    }
                }

            } else if ($(chkObject).closest("ul").attr("id") == "ulExamSubCharacteristics") {
                var isParentExist = false;
                for (index = 0; index < BillingInformation.selectedPhyExamTempData.length; index++) {
                    for (var innerIndex in BillingInformation.selectedPhyExamTempData[index].Sections) {
                        for (var mostInnerIndex in BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                            if (BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId == $(chkObject).closest("li").attr('parentid')) {
                                var obj = {
                                    CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                    SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                    SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                    IsChecked: isChecked
                                };
                                var isUpdated = false;
                                for (var counter in BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics) {
                                    if (BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[counter].SubCharacteristicId == obj.SubCharacteristicId) {
                                        BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[counter] = obj;
                                        isUpdated = true;
                                    }
                                }
                                if (!isUpdated) {
                                    if (BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics == null)
                                        BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics = [];
                                    BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics.push(obj);
                                }

                                isParentExist = true;
                            }
                        }
                    }
                }
                if (!isParentExist) {
                    var characteristicId = $(chkObject).closest("li").attr('parentid');
                    var sectionId = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics li#" + characteristicId).attr('parentid');
                    var systemId = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection li#" + sectionId).attr('parentid');
                    var systemName = $('#' + BillingInformation.params.PanelID + ' #ulPhysicalExamSystems #lblName' + systemId).text();
                    var sectionName = $('#' + BillingInformation.params.PanelID + ' #ulPhysicalExamSystemSection #lblName' + sectionId).text();
                    var charName = $('#' + BillingInformation.params.PanelID + ' #ulExamCharacteristics #lblName' + sectionId).text();
                    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics li#" + characteristicId).addClass('green');
                    for (index = 0; index < BillingInformation.selectedPhyExamTempData.length; index++) {
                        for (var innerIndex in BillingInformation.selectedPhyExamTempData[index].Sections) {
                            if (BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].SectionId == sectionId) {
                                var char = {
                                    SectionId: sectionId,
                                    CharName: charName,
                                    IsChecked: isChecked,
                                    CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                    SubCharacteristics: [{
                                        CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                        SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                        SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                        IsChecked: isChecked
                                    }]
                                };
                                if (BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics == null)
                                    BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics = [];
                                BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics.push(char);
                                isParentExist = true;
                            }
                        }
                    }
                    if (!isParentExist) {
                        for (index = 0; index < BillingInformation.selectedPhyExamTempData.length; index++) {
                            if (BillingInformation.selectedPhyExamTempData.SystemId == systemId) {
                                var section = {
                                    SystemId: systemId,
                                    SectionId: sectionId,
                                    IsChecked: isChecked,
                                    SectionName: sectionName,
                                    Characteristics: [{
                                        SectionId: sectionId,
                                        CharName: charName,
                                        IsChecked: isChecked,
                                        CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                        SubCharacteristics: [{
                                            CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                            SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                            SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                            IsChecked: isChecked
                                        }]
                                    }]
                                };
                                if (BillingInformation.selectedPhyExamTempData[index].Sections == null)
                                    BillingInformation.selectedPhyExamTempData[index].Sections = [];
                                BillingInformation.selectedPhyExamTempData[index].Sections.push(section);
                                isParentExist = true;
                            }
                        }
                        if (!isParentExist) {
                            var system = {
                                SystemId: systemId,
                                IsChecked: isChecked,
                                SystemName: systemName,
                                Sections: [{
                                    SystemId: systemId,
                                    SectionId: sectionId,
                                    IsChecked: isChecked,
                                    SectionName: sectionName,
                                    Characteristics: [{
                                        SectionId: sectionId,
                                        CharName: charName,
                                        IsChecked: isChecked,
                                        CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                        SubCharacteristics: [{
                                            CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                            SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                            SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                            IsChecked: isChecked
                                        }]
                                    }]
                                }]
                            };
                            if (BillingInformation.selectedPhyExamTempData == null)
                                BillingInformation.selectedPhyExamTempData = [];
                            BillingInformation.selectedPhyExamTempData.push(system);
                        }

                    }
                }
            }
            if ($(chkObject).prop('checked'))
                $(chkObject).closest("li").addClass('green');
            //End Farooq Ahmad 02-03-2016 Store the Selected Items in Json Object

            var parentUlContrl = $(chkObject).parent().parent().parent();

            if (parentUlContrl != null && (parentUlContrl.attr("id") == "ulExamCharacteristics" || parentUlContrl.attr("id") == "ulExamSubCharacteristics")) {
                var currentParentId = $(chkObject).parent().parent().attr("parentid");

                if (parentUlContrl.attr("id") == "ulExamSubCharacteristics") {
                    var ParentCrtl = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics");
                    var parentLi = ParentCrtl.find("li#" + currentParentId);
                    parentLi.find("input[id*='chk']").prop("checked", true);
                    var parentSysId = parentLi.attr("parentid");
                    BillingInformation.selectParentSysControls(parentSysId);
                }
                else if (parentUlContrl.attr("id") == "ulExamCharacteristics") {
                    BillingInformation.selectParentSysControls(currentParentId);
                }
            }

            //}
            /*else {
                //Start Farooq Ahmad 02-03-201 Remove the Selected Items if uncheck in Json Object
                var systemIndex = -1, sectionIndex = -1, characteristicIndex = -1, subcharacteristicIndex = -1;

                if ($(chkObject).closest("ul").attr("id") == "ulPhysicalExamSystems") {
                    for (var index in BillingInformation.selectedPhyExamTempData) {
                        if (BillingInformation.selectedPhyExamTempData[index].SystemId == $(chkObject).closest("li").attr('id')) {
                            systemIndex = index;
                            break;
                        }
                    }
                    if (systemIndex > -1)
                        BillingInformation.selectedPhyExamTempData.splice(systemIndex, 1);

                } else if ($(chkObject).closest("ul").attr("id") == "ulPhysicalExamSystemSection") {

                    for (var index in BillingInformation.selectedPhyExamTempData) {
                        systemIndex = index;
                        for (var innerIndex in BillingInformation.selectedPhyExamTempData[index].Sections) {
                            if (BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].SectionId == $(chkObject).closest("li").attr('id')) {
                                sectionIndex = innerIndex;
                                break;
                            }
                        }
                        if (sectionIndex > -1)
                            break;
                    }

                    if (sectionIndex > -1)
                        BillingInformation.selectedPhyExamTempData[systemIndex].Sections.splice(sectionIndex, 1);

                } else if ($(chkObject).closest("ul").attr("id") == "ulExamCharacteristics") {

                    for (var index in BillingInformation.selectedPhyExamTempData) {
                        systemIndex = index;
                        for (var innerIndex in BillingInformation.selectedPhyExamTempData[index].Sections) {
                            sectionIndex = innerIndex;
                            for (var mostInnerIndex in BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                                if (BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId == $(chkObject).closest("li").attr('id')) {
                                    characteristicIndex = mostInnerIndex;
                                    break;
                                }
                            }
                            if (characteristicIndex > -1)
                                break;
                        }
                        if (characteristicIndex > -1)
                            break;
                    }
                    if (characteristicIndex > -1)
                        BillingInformation.selectedPhyExamTempData[systemIndex].Sections[sectionIndex].Characteristics.splice(characteristicIndex, 1);

                } else if ($(chkObject).closest("ul").attr("id") == "ulExamSubCharacteristics") {

                    for (var index in BillingInformation.selectedPhyExamTempData) {
                        systemIndex = index;
                        for (var innerIndex in BillingInformation.selectedPhyExamTempData[index].Sections) {
                            sectionIndex = innerIndex;
                            for (var mostInnerIndex in BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                                characteristicIndex = mostInnerIndex;
                                for (var mostestInnerIndex in BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics) {
                                    if (BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId == $(chkObject).closest("li").attr('id')) {
                                        subcharacteristicIndex = mostestInnerIndex;
                                        break;
                                    }
                                }
                                if (subcharacteristicIndex > -1)
                                    break;
                            }
                            if (subcharacteristicIndex > -1)
                                break;
                        }
                        if (subcharacteristicIndex > -1)
                            break;
                    }
                    if (subcharacteristicIndex > -1)
                        BillingInformation.selectedPhyExamTempData[systemIndex].Sections[sectionIndex].Characteristics[characteristicIndex].SubCharacteristics.splice(subcharacteristicIndex, 1);
                }
                $(chkObject).closest("li").removeClass('green');
                //End Farooq Ahmad 03-03-2016 Remove the Selected Items if uncheck in Json Object
            }*/
        }
        a = 10;
    },

    //Author: Muhammad Arshad
    //Date: 01-03-2016
    //This function will mark parent system/subsystem control as checked

    selectParentSysControls: function (ParentLiId) {
        if (ParentLiId != null) {
            var ParentCrtl = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection");
            var parentLi = ParentCrtl.find("li#" + ParentLiId);
            parentLi.find("input[id*='chk']").prop("checked", true);

            var ParentSystCrtl = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystems");
            var parentSysLi = ParentSystCrtl.find("li#" + parentLi.attr("parentid"));
            parentSysLi.find("input[id*='chk']").prop("checked", true);
        }
    },

    //Author: Muhammad Arshad
    //Date: 01-03-2016
    //This function will handle add new system/subsystem/characteristic/subcharacteristic

    addNewItem: function (itemType) {
        if (itemType != null && itemType != "") {
            var isSubCharacteristic = false;
            var ulControl = "";// $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #PhysicalExam");
            var currentLiClick = "";
            var currentCtrlId = "";
            var currentParentId = "";
            if (itemType.toLowerCase() == "system") {
                currentLiClick = "BillingInformation.showHideChildControls";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #" + currentCtrlId);
                var myval = "131";
                var myval = "133";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "BillingInformation.showHideChildControls";
                currentCtrlId = "ulPhysicalExamSystemSection";
                ulControl = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #" + currentCtrlId);
                var myval = "131";
                var myval = "133";
            }
            else if (itemType.toLowerCase() == "characteristic") {
                currentLiClick = "BillingInformation.showHideChildControls";
                currentCtrlId = "ulExamCharacteristics";
                ulControl = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #" + currentCtrlId);
                var myval = "131";
                var myval = "133";
            }
            else if (itemType.toLowerCase() == "subcharacteristic") {
                currentLiClick = "BillingInformation.showHideChildControls";
                currentCtrlId = "ulExamSubCharacteristics";
                isSubCharacteristic = true;
                ulControl = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #" + currentCtrlId);
                var myval = "131";
                var myval = "133";
            }
            if (ulControl != null && ulControl != "") {
                var currentLiClass = "";

                var arrNewlyAddedLi = ulControl.find("li[id*='-']");
                var currentId = "";
                if (itemType.toLowerCase() != "system") {
                    currentParentId = ulControl.find("li:last").attr("parentid");
                    //Start Farooq Ahmad 16-03-2016 if no li is in ul
                    if (currentParentId == null)
                        currentParentId = ulControl.attr("ParentId");
                    //End Farooq Ahmad 16-03-2016 if no li is in ul
                }


                if (arrNewlyAddedLi != null && arrNewlyAddedLi.length > 0) {
                    currentId = parseInt($(arrNewlyAddedLi[arrNewlyAddedLi.length - 1]).attr("id")) - 1;
                }
                else {
                    currentId = "-1";
                }
                var onClick = "";// currentLiClick + "('" + currentCtrlId + "','" + String(currentId) + "');";
                //Start Farooq Ahmad 16-03-2016 set onclick prop
                onClick = "BillingInformation.showHideChildControls('" + currentCtrlId + "','" + currentId + "');"
                //End Farooq Ahmad 16-03-2016 set onclick prop
                //Start Farooq Ahmad 03/03/2016 changing the on click function name
                liInnerText = '<div><span id="btnOpenDetail' + currentId + '" onclick="BillingInformation.editName(this,\'' + currentId + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-edit"></i></span><input type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="ml-sm pull-left  char" onclick="BillingInformation.selectParentControls(this);BillingInformation.toggleCheckBoxes(this);"><label id="lblName' + currentId + '" class="control-label pull-left ml-xs size65per ellipses hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label><div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs"><textarea spellcheck="true" rows="1" id="txtName' + currentId + '" onkeypress="BillingInformation.saveDetailComments(event,this)" name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="BillingInformation.saveTemplateSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';
                //End Farooq Ahmad 03/03/2016 changing the on click function name
                ulControl.append('<li id="' + currentId + '" ' + currentLiClass + ' parentid="' + currentParentId + '" onclick="' + onClick + '" value="' + currentId + '" refValue="' + currentId + '" subCharacteristicExist="">' + liInnerText + '</li>');
            }
        }
    },

    //Author: Farooq Ahmad
    //Date: 15-03-2016
    //This function will add new System Section Characteristics or Subcharacteristics
    addNewSysSecCharSubchar: function (event, obj) {
        if (event.which == 13) {
            event.preventDefault();
            $(obj).focus();
            $(obj).closest("ul").attr("id");
            //this.currentIdOfText = $(obj).attr("id").replace("txtName", '');
            //var onClickFunction = $(obj).parent().parent().find('.btn-link').attr("onclick");
            //var ID = $(obj).parent().parent().find('.btn-link').attr("id");
            //var ULID = $(obj).parent().parent().find('.btn-link').closest("ul").attr("id");
            //onClickFunction = onClickFunction.replace('this', "$('#" + BillingInformation.params.PanelID + " #" + ULID + " #" + ID + "')");
            //eval(onClickFunction);


        }
    },

    //Author: Muhammad Arshad
    //Date: 01-03-2016
    //This function will handle show/hide of Name Label/Textbox

    editName: function (objButton, detailParentId) {
        if (objButton != null && detailParentId != null) {
            var liObject = $(objButton).parent().parent();
            var SystemDetailDiv = $(objButton).parent().find("div#divNameDetail" + detailParentId);
            var SystemNameLabel = $(objButton).parent().find("#lblName" + detailParentId);
            var txtSystemName = SystemDetailDiv.find("#txtName" + detailParentId);
            if (SystemDetailDiv.hasClass("hidden") == true) {
                SystemDetailDiv.removeClass("hidden");
                txtSystemName.val(SystemNameLabel.text());
                SystemNameLabel.addClass("hidden");
            }
            else {
                SystemDetailDiv.addClass("hidden");
                if (txtSystemName.val() != "") {
                    SystemNameLabel.text(txtSystemName.val());
                }
                SystemNameLabel.removeClass("hidden");
                BillingInformation.selectParentControls($(liObject).find('input:checkbox'));
            }
        }
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle fill of PhysicalExam childs
    loadPhysicalExamStatuses: function (parentId, parentType) {

        var currentLiClass = "";
        var currentLiClick = "";
        var currentCtrlId = "";
        var ParentDiv = "";
        var data = "";

        var selectedData = "";

        if (parentType != null && parentType.toLowerCase() == "mainpesystem") {
            Crtl = '#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems";
            currentLiClick = "BillingInformation.showHideChildControls";
            ParentDiv = "divPhysicalExamSystems";
            methodName = "GetPhysicalExamSystem";
            currentCtrlId = "ulPhysicalExamSystems";

        }
        else if (parentType != null && parentType.toLowerCase() == "system") {
            Crtl = '#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection";
            currentLiClick = "BillingInformation.showHideChildControls";
            ParentDiv = "divPhysicalExamSystemSection";
            methodName = "GetPhysicalExamSectionBySystemId";
            currentCtrlId = "ulPhysicalExamSystemSection";
            //Start 09-03-2016 Farooq Ahmad Sending Template ID as parameter
            data = "ID=" + parentId + "&ID2=-1";
            //End 09-03-2016 Farooq Ahmad Sending Template ID as parameter
            //   BillingInformation.isNormalTriggred = selectedData != null ? selectedData.IsNormal : false;
        }
        else if (parentType != null && parentType.toLowerCase() == "section") {
            Crtl = '#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics";
            currentLiClick = "BillingInformation.showHideChildControls";
            ParentDiv = "divExamCharacteristics";
            methodName = "GetPhysicalExamCharcteristicBySectionId";
            currentCtrlId = "ulExamCharacteristics";
            //Start 09-03-2016 Farooq Ahmad Sending Template ID as parameter
            data = "ID=" + parentId + "&ID2=-1";
            //End 09-03-2016 Farooq Ahmad Sending Template ID as parameter
        }
        else if (parentType != null && parentType.toLowerCase() == "characteristics") {
            Crtl = '#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #ulExamSubCharacteristics";
            currentLiClick = "BillingInformation.showHideChildControls";
            ParentDiv = "divExamSubCharacteristics";

            methodName = "GetPhysicalExamSubCharcteristicByCharacteristicId";
            currentCtrlId = "ulExamSubCharacteristics";
            //Start 09-03-2016 Farooq Ahmad Sending Template ID as parameter
            data = "ID=" + parentId + "&ID2=-1";
            //End 09-03-2016 Farooq Ahmad Sending Template ID as parameter
        }
        else if (parentType != null && parentType.toLowerCase() == "subcharacteristics") {
            //  Crtl = '#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #CharacteristicsDetails";
            currentLiClick = "BillingInformation.showHideChildControls";
            ParentDiv = "amSubCharacteristics";
            methodName = "GetSocialHxCounsellingPeriod";
            currentCtrlId = "";
        }
        else {
            data = "ID=" + parentId;
        }

        return MDVisionService.lookups(methodName, true, data).done(function (result) {
            result = JSON.parse(result[methodName]);
            if ($(Crtl).length > 0)
                l = $(Crtl);
            if (parentType != null && parentType.toLowerCase() == "subcharacteristics") {
                return;
            }

            l.empty();


            var isFirstLi = true;
            var onClick = "";//currentLiClick + "('" + currentCtrlId + "','" + String(item.Value) + "');";
            //item.Value = item.Value == "" ? 0 : item.Value;
            //if (parentType.toLowerCase() == "section" || parentType.toLowerCase() == "characteristics" || parentType.toLowerCase() == "subcharacteristics") {
            //    //Start 11-02-2016 Humaira Yousaf to show select all checkboxes only if there is data
            //    if (result.length > 0) {
            //        //End 11-02-2016 Humaira Yousaf
            //        var liInnerText = "";
            //        liInnerText = '<div><input type="checkbox" id="chkSelectAll+ve" name="SelectAll+ve" class="ml-xlg pull-left" onclick="BillingInformation.selectAllCharacteristics(this,true);"><input type="checkbox" id="chkSelectAll-ve" name="SelectAll-ve" class="ml-sm pull-left" onclick="BillingInformation.selectAllCharacteristics(this,false);"><label class="control-label pull-left pl-xs">Select All</label><div class="clearfix"></div></div>';
            //        l.append('<li id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value=' + item.Value + ' refValue="' + item.RefValue + '">' + liInnerText + ' </li>');
            //    }
            //}
            //else if (parentType.toLowerCase() == "system") {
            //    var normalDetail = '<a id="btnNormalSectionDetails" onclick="BillingInformation.openNormalSectionDetail(this);" class="btnEffect pull-left ml-md hidden"><i class="fa fa-book blue"></i></a><div id="textAreaNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="4" id="txtNormalSectionDetail" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveNormalSystemDetail' + item.Value + '" onclick="BillingInformation.saveExamSystemNormalDetail(this,\'textNormalSectionDetailNormal' + '\',\'' + parentId + '\');" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div></div>';

            //    liInnerText = '<div><input type="checkbox" id="chkNormalSection" name="NormalSection" class="pull-left" onclick="BillingInformation.markSectionAsNormal(this,true);"><label class="control-label pull-left pl-xs">Normal</label>' + normalDetail + '<div class="clearfix"></div></div>';
            //    // Added by Humaira Yousaf on Feb 9, 2016 for section normal details
            //    l.append('<li id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value=' + item.Value + ' refValue="' + item.RefValue + '">' + liInnerText + ' </li>');
            //}

            $.each(result, function (j, item) {
                if (item.Value != "") {
                    if (isFirstLi == true) {
                        //currentLiClass = 'class="active"';
                        isFirstLi = false;
                    }
                    else {
                        currentLiClass = "";
                    }
                    var physicalExamId = $('#' + BillingInformation.params.PanelID + " #hfPhysicalExamId").val();
                    var onClick = currentLiClick + "('" + currentCtrlId + "','" + String(item.Value) + "');";
                    //Start//18-02-2016//Ahmad Raza//Delete system,section,Characteristics,SubCharacteristics detail
                    var deleteClick = "";
                    if (parentType.toLowerCase() == "system") {
                        deleteClick = "BillingInformation.deleteSectionDetail('" + parentType + "'," + parentId + "," + physicalExamId + "," + item.Value + ")";
                    }
                    else if (parentType.toLowerCase() == "section") {
                        deleteClick = "BillingInformation.deleteCharacteristicDetail('" + parentType + "'," + parentId + "," + physicalExamId + "," + item.Value + ")";
                    }
                    else if (parentType.toLowerCase() == "characteristics") {
                        deleteClick = "BillingInformation.deleteSubCharacteristicDetail('" + parentType + "'," + parentId + "," + physicalExamId + "," + item.Value + ")";
                    }
                    //End//18-02-2016//Ahmad Raza//Delete system,section,Characteristics,SubCharacteristics detail
                    //item.Value = item.Value == "" ? 0 : item.Value;
                    var liInnerText = '<a href="#' + ParentDiv + '">' + item.Name + '</a>';
                    var isSubCharacteristic = false;
                    if (parentType.toLowerCase() == "characteristics") {
                        isSubCharacteristic = true;
                    }

                    //Start Farooq Ahmad 3/3/2016 Changing the on click function name
                    if (item.RefName != "") {
                        liInnerText = '<div><span id="btnOpenDetail' + item.Value + '" onclick="BillingInformation.editName(this,\'' + item.Value + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-edit"></i></span><input type="checkbox" id="chk' + item.Value + '+ve" name="' + item.Value + '" class="ml-sm pull-left  char" onclick="BillingInformation.selectParentControls(this);BillingInformation.toggleCheckBoxes(this);"><label id="lblName' + item.Value + '" class="control-label pull-left ml-xs size65per ellipses" data-toggle="tooltip"  title="" data-original-title="' + item.Name + '">' + item.Name + '</label><div id="divNameDetail' + item.Value + '" class="rightInnerAddon pb-xs hidden"><textarea rows="1" spellcheck="true" id="txtName' + item.Value + '" onkeypress="BillingInformation.saveDetailComments(event,this)" name="Name' + item.Value + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.Value + '" onclick="BillingInformation.saveTemplateSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><span id="btnShowSubCharacteristics' + item.Value + '" onclick="" class="pull-right" disabled="disabled"><i class="fa fa-caret-right blue"></i></span><div class="clearfix"></div><div class="clearfix"></div></div>';
                    }
                    else {
                        liInnerText = '<div><span id="btnOpenDetail' + item.Value + '" onclick="BillingInformation.editName(this,\'' + item.Value + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-edit"></i></span><input type="checkbox" id="chk' + item.Value + '+ve" name="' + item.Value + '" class="ml-sm pull-left  char" onclick="BillingInformation.selectParentControls(this);BillingInformation.toggleCheckBoxes(this);"><label id="lblName' + item.Value + '" class="control-label pull-left ml-xs size65per ellipses" data-toggle="tooltip"  title="" data-original-title="' + item.Name + '">' + item.Name + '</label><div id="divNameDetail' + item.Value + '" class="rightInnerAddon pb-xs hidden"><textarea rows="1" spellcheck="true" id="txtName' + item.Value + '" onkeypress="BillingInformation.saveDetailComments(event,this)" name="Name' + item.Value + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.Value + '" onclick="BillingInformation.saveTemplateSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';
                    }
                    //End Farooq Ahmad 3/3/2016 Changing the on click function name
                    l.append('<li id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value="' + item.Value + '" refValue="' + item.RefValue + '" subCharacteristicExist="' + item.RefName + ' ">' + liInnerText + '</li>');
                    // l.append('<li id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value="' + item.Value + '" refValue="' + item.RefValue + '" subCharacteristicExist="' + item.RefName + ' ">' + liInnerText + '<span class="removeIconListHover" onclick="' + deleteClick + '" ><i class="fa fa-times"></i></span></li>');

                }
            });

            //Start//25-02-2016//Ahmad Raza//Setting ToolTip for Characteristics and subCharacteristics .
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            //End//25-02-2016//Ahmad Raza//Setting ToolTip for Characteristics and subCharacteristics .

            // Added by Humaira Yousaf on Feb 9, 2016
            $('.textAreaScroll').slimScroll({
                position: 'right',
                height: '100%',
            });
        });



    },

    //Author: Muhammad Arshad
    //Date: 18-07-2016
    //This function will handle filtering of PhysicalExam Template Characteristics/Sub Characteristics
    filterOptions: function (obj, ulId) {
        if (obj != null && ulId != null) {
            var strSearch = $(obj).val();
            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #" + ulId + " li").each(function () {
                var currentLiText = $(this).text();
                var showCurrentLi = currentLiText.toLowerCase().indexOf(strSearch.toLowerCase()) > -1 ? true : false;
                $(this).toggle(showCurrentLi);
            });

        }
    },

    //Author: Farooq Ahmad
    //Date: 09/03/2016
    //This function will handel if press enter in edit field
    saveDetailComments: function (event, obj) {
        if (event.which == 13) {
            event.preventDefault();
            $(obj).focusout();
            $(obj).blur();
            this.currentIdOfText = $(obj).attr("id").replace("txtName", '');
            var onClickFunction = $(obj).parent().parent().find('.btn-link').attr("onclick");
            var ID = $(obj).parent().parent().find('.btn-link').attr("id");
            var ULID = $(obj).parent().parent().find('.btn-link').closest("ul").attr("id");
            onClickFunction = onClickFunction.replace('this', "$('#" + BillingInformation.params.PanelID + " #" + ULID + " #" + ID + "')");
            eval(onClickFunction);


        }
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will get number part from string
    getNumberPart: function (obj) {
        var innernumericPart = 0;
        $.each(obj, function (i, item) {
            if (i.indexOf("SystemId") > -1) {
                innernumericPart = i.replace(/[^\d]+/, '');
            }
        });
        return innernumericPart;
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will get object of clicked element
    getObjectOfClickedElement: function (parentType, parentId) {
        var objData = null;
        //retrieve data of sections from system li's
        if (parentType != null && parentType.toLowerCase() == "system") {
            var ctrl = '#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems";
            objData = $(ctrl).find('li#' + parentId).data("SystemSectionIds_" + parentId);
        }
            //retrieve data of characteristics from section li's
        else if (parentType != null && parentType.toLowerCase() == "section") {
            var ctrl = '#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection";
            objData = $(ctrl).find('li#' + parentId).data("SystemCharacteristicsIds_" + parentId);
        }
            //retrieve data of subCharacteristics from characteristics li's
        else if (parentType != null && parentType.toLowerCase() == "characteristics") {
            var ctrl = '#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics";
            objData = $(ctrl).find('li#' + parentId).data("SystemSubCharacteristicsIds_" + parentId);
        }
        return objData;
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle enabling/disabling of Exercises controls on Miscellanous Tab
    enableDisableList: function (listId, isDisable) {
        if (listId != null && listId != "") {
            var self = $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation ' + listId + " li").not(":first").each(function () {

                if (isDisable) {
                    if (!$(this).hasClass('disableAll'))
                        $(this).addClass('disableAll');
                }
                else
                    $(this).removeClass('disableAll');
            });

        }
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle setting/calculating width of PhysicalExam

    toggleVerticalWidth: function (obj) {


        var panelChildrens = null;
        var currentPanel = null;
        var applyWidth = null;
        if (obj != null) {
            currentPanel = $(obj.delegateTarget).parent();
            panelChildrens = currentPanel.children("section.panel");
            applyWidth = currentPanel;
            BillingInformation.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
        }
        else {
            $('.toggleVertical').each(function (e) {
                currentPanel = $(this);
                panelChildrens = currentPanel.children().children("section.panel");
                applyWidth = currentPanel.children();
                BillingInformation.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
            });


        }
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle calculating width of PhysicalExam

    toggleVerticalWidthCtrl: function (currentPanel, panelChildrens, applyWidth) {
        var totalWidth = 0;
        var hidden = 0;
        //find total width of panels
        panelChildrens.each(function (e) {
            totalWidth += $(this).outerWidth(true);
        });
        //find total width of hidden panel
        currentPanel.find("section.hidden").each(function (e) {
            hidden += $(this).outerWidth(true);
        });
        //apply width to div
        applyWidth.width(((totalWidth - hidden) + 60) + "px");

    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will set the values in hidden field
    setHiddenFieldValues: function (currentUlId, currentId, parentObj) {
        var systemId = $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #ulPhysicalExamSystems li.active').attr("id");
        var sectionId = $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #ulPhysicalExamSystemSection li.active').attr("id");
        $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails input[id*="hfSystemId"]').val(systemId);
        $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails input[id*="hfSectionId"]').val(sectionId);
        var characteristicId = "";
        var isCharacteristicPostive = false;
        var isSubCharacteristicPostive = false;
        var subCharacteristicId = "";
        if (currentUlId.toLowerCase() == "ulexamcharacteristics") {
            if (currentId != null && currentId.indexOf("+ve") > -1) {
                isCharacteristicPostive = true;
            }
            characteristicId = $(parentObj).parent().attr("id");
            $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails input[id*="hfCharacteristicId"]').val(characteristicId);
            $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails input[id*="hfIsCharacteristicPositive"]').val(isCharacteristicPostive);


        }
        else if (currentUlId.toLowerCase() == "ulexamsubcharacteristics") {
            if (currentId != null && currentId.indexOf("+ve") > -1) {
                isSubCharacteristicPostive = true;
            }
            characteristicId = $(parentObj).parent().attr("parentid");
            subCharacteristicId = $(parentObj).parent().attr("id");

            $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails input[id*="hfCharacteristicId"]').val(characteristicId);

            var chkOfCharacteristics = $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #ulExamCharacteristics li#' + characteristicId + '  input[type=checkbox]:checked').attr("id");
            if (chkOfCharacteristics != null && chkOfCharacteristics.indexOf("+ve") > -1) {
                isCharacteristicPostive = true;
            }
            $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails input[id*="hfSubCharacteristicId"]').val(subCharacteristicId);
            $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails input[id*="hfIsCharacteristicPositive"]').val(isCharacteristicPostive);
            $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails input[id*="hfIsSubCharacteristicPositive"]').val(isSubCharacteristicPostive);
        }
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle show/hide of PhysicalExam child controls
    showHideChildControls: function (parentCtrl, liId, event) {

        BillingInformation.parentCtrlGlobel = parentCtrl;

        var self = $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails')
        var myJSON = self != null ? self.getMyJSONByName() : "{}";

        var selectedJSON = JSON.parse(myJSON);

        //arrSelectedJSON.push(selectedJSON);
        var selectedLiId = BillingInformation.getNumberPart(selectedJSON)
        if (selectedLiId != null && selectedLiId != "") {
            if (BillingInformation.ExamDetails[selectedLiId] != null) {
                BillingInformation.ExamDetails[selectedLiId] = BillingInformation.ExamDetails[selectedLiId].replace(BillingInformation.ExamDetails[selectedLiId], myJSON);
            }
            else {
                BillingInformation.ExamDetails[selectedLiId] = myJSON;

            }
        }

        //var isCharGreen = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics li").hasClass('green');
        //var isSectionGreen = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection li").hasClass('green');
        //if (isSectionGreen == false && isCharGreen == true)
        //{
        //    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics > li").find('input').attr('checked', false);
        //    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics > li").removeClass('green');

        //}
        //Start//09-02-2016//Ahmad Raza//Show/hide sections logic
        if (parentCtrl == "ulPhysicalExamSystems") {

            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#SectionCharacteristics").addClass('hidden');
            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#CharacteristicsSubCharacteristics").addClass('hidden');
        }
        if (parentCtrl == "ulPhysicalExamSystems" && liId != $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystems li.active").attr("id")) {

            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#SectionCharacteristics").addClass('hidden');
            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#CharacteristicsSubCharacteristics").addClass('hidden');
        }
        else if ((parentCtrl == "ulPhysicalExamSystemSection" && liId != $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection li.active").attr("id"))) {
            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#CharacteristicsSubCharacteristics").addClass('hidden');

        }
        //End//09-02-2016//Ahmad Raza//Show/hide sections logic
        if (parentCtrl != null && parentCtrl != "") {

            var childPartialId = "";
            var isSystemSectionCtrl = "";
            var isCharacteristicsCtrl = "";
            var isSubCharacteristicsCtrl = "";
            if (parentCtrl.toLowerCase() == "ulphysicalexamsystems") {
                isSystemSectionCtrl = "1";
                childPartialId = "System";
                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section[id^='SectionCharacteristics']").addClass("hidden");

            }
            else if (parentCtrl.toLowerCase() == "ulphysicalexamsystemsection") {
                isSystemSectionCtrl = "1";
                childPartialId = "Section";
            }
            else if (parentCtrl.toLowerCase() == "ulexamcharacteristics") {
                isCharacteristicsCtrl = "1";
                childPartialId = "Characteristics";
            }
            else if (parentCtrl.toLowerCase() == "ulexamsubcharacteristics") {
                isSubCharacteristicsCtrl = "1";
                childPartialId = "SubCharacteristics";
            }

            if (liId != null && liId != "") {
                $('#' + BillingInformation.parentCtrlGlobel).find("li").each(function (i, item) {
                    if ($(this).attr("id") != null && $(this).attr("id") == liId) {
                        //BillingInformation.loadChildData(liId, childPartialId);

                        var objCurrent = item;
                        $.when(BillingInformation.loadChildData(liId, childPartialId)).then(function () {
                            if (isSystemSectionCtrl == "1") {
                                if ($(objCurrent).hasClass("active") == false) {
                                    $(objCurrent).addClass("active");
                                    BillingInformation.SelectedSystem = objCurrent;
                                }
                                if (childPartialId != "" && childPartialId != "Characteristics") {

                                    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section[id^='" + childPartialId + "']").removeClass("hidden");
                                    if ($('#' + BillingInformation.params.PanelID + " #frmBillingInformation input[id='chkPhysicalExamsNormal']").prop("checked") == true) {

                                        // Added by Humaira Yousaf on Feb 9, 2016 to reset checkboxes
                                        var characteristicDiv = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#SectionCharacteristics");
                                        var subCharacteristicDiv = $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section#CharacteristicsSubCharacteristics");

                                        $(characteristicDiv).find("#ulExamCharacteristics > li").find('input:checkbox').prop('checked', false);
                                        $(subCharacteristicDiv).find("#ulExamSubCharacteristics> li").find('input:checkbox').prop('checked', false);
                                        // End
                                        $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section[id^='" + childPartialId + "'] li:first input[id='chkNormalSection']").trigger("click");
                                    }
                                        // Added by Humaira Yousaf on Feb 4, 2016 to check normal checkbox
                                    else {
                                        var system = null;
                                        if (childPartialId.toLowerCase() == "system") {
                                            var system = BillingInformation.getObjectOfClickedElement(childPartialId, liId);
                                            system = typeof system == 'undefined' ? null : system;
                                            var sectionExists = system != null ? (system.Sections != 'undefined' && system.Sections.length > 0 ? true : false) : false;
                                            var IsNormal = system != null ? system.IsNormal : false;
                                            if (($(objCurrent).hasClass("green") == true && !sectionExists && IsNormal)) {
                                                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection li:first input[id='chkNormalSection']").trigger("click");
                                            }
                                            else
                                                BillingInformation.enableDisableList('#ulPhysicalExamSystemSection', false);
                                        }
                                    }
                                    //End
                                }
                            }
                            else if (isCharacteristicsCtrl == "1") {
                                childPartialId = "Characteristics";
                                var objCheckBox = $(objCurrent).find("input[type='checkbox']");
                                var isChecked = false;
                                $.each(objCheckBox, function (i, item) {
                                    var id = $(item).attr("id");
                                    if (isChecked == false && $(item).prop("checked") == true) {
                                        isChecked = $(item).prop("checked");
                                    }
                                });
                                //changed by Abid ali
                                //if (isChecked == true) {
                                if ($(objCurrent).hasClass("active") == false) {
                                    $(objCurrent).addClass("active");
                                }
                                //Start 12-02-2016 Humaira Yousaf to hide/show sub characteristics
                                if ($.trim($(objCurrent).attr('subcharacteristicExist')) != "") {
                                    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section[id^='" + childPartialId + "']").removeClass("hidden");
                                }
                                else {
                                    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section[id^='" + childPartialId + "']").addClass("hidden");
                                }
                                //End 12-02-2016 Humaira Yousaf
                                // }
                                //else {
                                //    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section[id^='" + childPartialId + "']").addClass("hidden");
                                // }
                                //changed by Abid ali
                            }
                            else if (isSubCharacteristicsCtrl == "1") {
                                childPartialId = "SubCharacteristics";
                                var objCheckBox = $(objCurrent).find("input[type='checkbox']");
                                var isChecked = false;
                                $.each(objCheckBox, function (i, item) {
                                    var id = $(item).attr("id");
                                    if (isChecked == false && $(item).prop("checked") == true) {
                                        isChecked = $(item).prop("checked");
                                    }
                                });
                                if (isChecked == true) {
                                    if ($(objCurrent).hasClass("active") == false) {
                                        $(objCurrent).addClass("active");
                                    }
                                    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section[id^='" + childPartialId + "']").removeClass("hidden");
                                }
                                else {
                                    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation section[id^='" + childPartialId + "']").addClass("hidden");
                                }
                            }

                            BillingInformation.toggleVerticalWidth();

                            //if (BillingInformation.array.indexOf(liId + 'exam') > -1) {

                            //var currentIdPR = $(parentObj).parent().parent().attr("id");
                            //if (currentIdPR != null && currentIdPR != "")
                            param1 = $(objCurrent).parent().attr('id');
                            param2 = $(objCurrent).find('div input[type="checkbox"]:checked').attr('id');
                            param3 = $(objCurrent).find('button').parent()
                            BillingInformation.setHiddenFieldValues(param1, param2, param3);

                            var self = $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #CharacteristicsDetails');

                            var finalStr = self != null ? self.getMyJSONByName() : "";

                            var selectedJSON = JSON.parse(finalStr);
                            //Start//10-02-2016//Ahmad Raza//implimentation of State Management
                            var checkChar = '';
                            var subCheckChar = '';
                            var parentOfSubChar = 0;
                            if (BillingInformation.parentCtrlGlobel == "ulExamSubCharacteristics") {
                                $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #ulExamSubCharacteristics .char').each(function () {
                                    if ($(this).prop("checked")) {
                                        parentOfSubChar = $(this).parent().parent().attr('parentid');
                                        if ($(this).attr('id').indexOf('-') > -1)
                                            subCheckChar += parseInt($(this).attr('id').replace(/[^\d]+/, '')) + 'N,';
                                        else
                                            subCheckChar += parseInt($(this).attr('id').replace(/[^\d]+/, '')) + 'P,';
                                    }
                                });
                                var subCharNum = BillingInformation.getNumberPart(selectedJSON);
                                if (subCheckChar.length > 0)
                                    subCheckChar = subCheckChar.substr(0, subCheckChar.length - 1);
                                selectedJSON['SubCharacteristicId' + parentOfSubChar] = subCheckChar;
                                var arrSelectedJSON = [];
                                arrSelectedJSON.push(selectedJSON);
                            }
                            if (BillingInformation.parentCtrlGlobel == "ulExamCharacteristics") {
                                $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation #ulExamCharacteristics .char').each(function () {
                                    if ($(this).prop("checked")) {
                                        if ($(this).attr('id').indexOf('-') > -1)
                                            checkChar += parseInt($(this).attr('id').replace(/[^\d]+/, '')) + 'N,';
                                        else
                                            checkChar += parseInt($(this).attr('id').replace(/[^\d]+/, '')) + 'P,';
                                    }
                                });
                                var charNum = BillingInformation.getNumberPart(selectedJSON);
                                if (checkChar.length > 0)
                                    checkChar = checkChar.substr(0, checkChar.length - 1);
                                selectedJSON['CharacteristicId' + charNum] = checkChar;
                                var arrSelectedJSON = [];
                                arrSelectedJSON.push(selectedJSON);
                            }
                            //var charNum = BillingInformation.getNumberPart(selectedJSON);
                            //if (checkChar.length > 0)
                            //    checkChar = checkChar.substr(0, checkChar.length - 1);
                            //selectedJSON['CharacteristicId' + charNum] = checkChar;
                            //var arrSelectedJSON = [];
                            //arrSelectedJSON.push(selectedJSON);

                            if (BillingInformation.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics") {
                                //Start Farooq Ahmad 18/02/2016 Store Charachteristics in JSON Array
                                var charNum = BillingInformation.getNumberPart(selectedJSON);
                                if (charNum != '') {
                                    var isAlreadyContain = false;
                                    objectToPop = -1;

                                    var charNumInner = BillingInformation.getNumberPart(selectedJSON);
                                    var val = selectedJSON['CharacteristicId' + charNumInner];
                                    selectedJSON['CharacteristicId' + charNumInner] = val.substr(val.indexOf(charNumInner), charNumInner.length + 1);

                                    for (var counter in BillingInformation.myArr) {
                                        if (BillingInformation.myArr[counter].hasOwnProperty('CharacteristicId' + charNum)) {
                                            BillingInformation.myArr[counter] = selectedJSON;
                                            objectToPop = counter;
                                            isAlreadyContain = true;
                                        }

                                    }
                                    if (isAlreadyContain == false)
                                        BillingInformation.myArr.push(selectedJSON);

                                    if ($('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics li#" + charNum + " input[type='checkbox']:checked").length == 0)
                                        if (parseInt(objectToPop) > -1) {
                                            BillingInformation.myArr.splice(parseInt(objectToPop), 1);
                                        } //BillingInformation.myArr.pop(selectedJSON);
                                } else {
                                    //var Characteristics = selectedJSON.CharacteristicId.split(',');
                                    //if (Characteristics.length > 0) {
                                    //    Characteristics = Characteristics[Characteristics.length - 1];
                                    //    Characteristics = Characteristics.replace('N', '').replace('P', '');
                                    //    Characteristics = parseInt(liId);
                                    //    objectToPop = -1;
                                    //    var isAlreadyContain = false;
                                    //    for (var counter in BillingInformation.myArr) {
                                    //        if (BillingInformation.myArr[counter].hasOwnProperty('CharacteristicId' + Characteristics)) {
                                    //            objectToPop = counter;
                                    //            break;
                                    //        }
                                    //    }
                                    //    if ($('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics li#" + Characteristics + " input[type='checkbox']:checked").length == 0)
                                    //        if (parseInt(objectToPop) > -1) {
                                    //            BillingInformation.myArr.splice(parseInt(objectToPop), 1);
                                    //        }

                                    //}
                                }
                                var arrSelectedJSON = [];
                                arrSelectedJSON = BillingInformation.myArr;
                                //End Farooq Ahmad 18/02/2016 Store Charachteristics in JSON Array
                            }
                            if (BillingInformation.parentCtrlGlobel.toLowerCase() == "ulexamsubcharacteristics") {

                                for (var result in BillingInformation.myArr) {
                                    var finalsub = BillingInformation.myArr[result];
                                    var innernumericPartSub = 0;
                                    var subNumberValue = 0;
                                    $.each(finalsub, function (i, item) {
                                        if (i.indexOf("SystemId") > -1) {
                                            innernumericPartSub = i.replace(/[^\d]+/, '');
                                            subNumberValue = finalsub[i];
                                        }
                                    });
                                    if (innernumericPartSub == parentOfSubChar) {
                                        BillingInformation.myArr[result]["SubCharacteristicId" + innernumericPartSub] = subCheckChar;
                                    }
                                }

                                //BillingInformation.myArr.push(selectedJSON);

                            }

                            if (BillingInformation.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystems" || BillingInformation.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystemsection") {
                                arrSelectedJSON = [];
                                //Start Farooq Ahmad 02/18/2016 load Characteristics
                                // arrSelectedJSON.push(BillingInformation.mainSelected);
                                arrSelectedJSON = BillingInformation.myArr;
                                //End Farooq Ahmad 02/18/2016 load Characteristics

                            }

                            //var finalStr = "";

                            //$.each(BillingInformation.ExamDetails, function (i, item) {
                            //    var mystr = item;
                            //    arrSelectedJSON.push(JSON.parse(item));
                            //    finalStr = finalStr.concat(item);
                            //    finalStr = finalStr.replace("}{", ",");
                            //});
                            var numericPart = 0;
                            var selectedCharacteristicIds = BillingInformation.getCommaSeparatedIds(arrSelectedJSON, false);
                            if (arrSelectedJSON != null)
                                $.each(arrSelectedJSON, function (i, item) {

                                    for (key in item) {
                                        var myval = key;
                                        if (key.indexOf("SystemId") > -1) {
                                            numericPart = key.replace(/[^\d]+/, '');
                                        }
                                    }
                                });
                            if (arrSelectedJSON != null)
                                $.each(arrSelectedJSON, function (index, value) {
                                    numericPart = BillingInformation.getNumberPart(arrSelectedJSON[index]);

                                    if (arrSelectedJSON[index]["SystemId" + numericPart] == $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystems li.active").attr('id'))
                                        var charids = selectedCharacteristicIds.split(',');
                                    for (var out in charids) {
                                        var charnumpart = parseInt(charids[out]);
                                        if ($('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection #" + arrSelectedJSON[index]["SectionId" + numericPart]).attr('id') == $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection li.active").attr('id')) {

                                            if (charids[out].indexOf('N') > -1) {
                                                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics li#" + charnumpart).find("input[id*='-ve']").prop("checked", true);
                                                //$('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics #chk" + charnumpart + "-ve").prop("checked", true);
                                            }
                                            if (charids[out].indexOf('P') > -1) {
                                                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics li#" + charnumpart).find("input[id*='+ve']").prop("checked", true);
                                                //$('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics #chk" + charnumpart + "+ve").prop("checked", true);
                                            }
                                        }
                                        else {
                                            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection #" + arrSelectedJSON[index]["SectionId" + numericPart]).addClass('green');
                                        }

                                    }

                                    var subNumPart = 0;



                                    //Farooq Ahmad
                                    //25/02/2016
                                    //Bind Sub Charateristics if clicked on Charateristics

                                    if (BillingInformation.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics") {
                                        //var selectedSubCharacteristicIds = BillingInformation.getCommaSeparatedIds(arrSelectedJSON, true);

                                        for (var Count in BillingInformation.ExamDetails) {
                                            var CurrentObj = BillingInformation.ExamDetails[Count];
                                            CurrentObj = JSON.parse(CurrentObj);
                                            var CurrentIndex = BillingInformation.getNumberPart(CurrentObj)
                                            if (CurrentObj["CharacteristicId" + CurrentIndex] == liId) {
                                                var SubCharacteristicId = CurrentObj["SubCharacteristicId" + CurrentIndex];
                                                var IsSubCharacteristicPositive = CurrentObj["IsSubCharacteristicPositive" + CurrentIndex];
                                                if (SubCharacteristicId != "") {
                                                    if (IsSubCharacteristicPositive.toLowerCase() == "true") {
                                                        $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamSubCharacteristics li#" + SubCharacteristicId).find("input[id*='+ve']").prop("checked", true);
                                                        $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamSubCharacteristics li#" + SubCharacteristicId).find("input[id*='-ve']").prop("checked", false);
                                                    }
                                                    else {
                                                        $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamSubCharacteristics li#" + SubCharacteristicId).find("input[id*='+ve']").prop("checked", false);
                                                        $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamSubCharacteristics li#" + SubCharacteristicId).find("input[id*='-ve']").prop("checked", true);
                                                    }
                                                }

                                            }
                                        }
                                    }

                                });

                            //End//10-02-2016//Ahmad Raza//implimentation of State Management

                            param1 = $(objCurrent).parent().attr('id');
                            param2 = $(objCurrent).find('div input[type="checkbox"]:checked').attr('id');
                            param3 = $(objCurrent).find('div');
                            BillingInformation.setHiddenFieldValues(param1, param2, param3);


                            //Start Farooq Ahmad 02/03/2016 add green class if present in selectedphyexamtempdata

                            if ($(objCurrent).closest("ul").attr("id") == "ulPhysicalExamSystems") {
                                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection").attr("parentId", liId);
                                for (index = 0; index < BillingInformation.selectedPhyExamTempData.length; index++) {
                                    for (var innerIndex in BillingInformation.selectedPhyExamTempData[index].Sections) {
                                        if ($.parseJSON(BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].IsChecked.toString().toLowerCase()))
                                            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].SectionId).addClass("green");
                                        else
                                            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].SectionId).removeClass("green");
                                        $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].SectionId + " input:checkbox").prop("checked", $.parseJSON(BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].IsChecked.toString().toLowerCase()));
                                        $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].SectionId + " label").text(BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].SectionName);
                                        $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulPhysicalExamSystemSection").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].SectionId + " label").attr("data-original-title", BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].SectionName);
                                    }
                                }

                            } else if ($(objCurrent).closest("ul").attr("id") == "ulPhysicalExamSystemSection") {
                                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics").attr("parentId", liId);
                                for (index = 0; index < BillingInformation.selectedPhyExamTempData.length; index++) {
                                    for (var innerIndex in BillingInformation.selectedPhyExamTempData[index].Sections) {
                                        for (var mostInnerIndex in BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                                            if ($.parseJSON(BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].IsChecked.toString().toLowerCase()))
                                                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId).addClass("green");
                                            else
                                                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId).removeClass("green");
                                            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId + " input:checkbox").prop("checked", $.parseJSON(BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].IsChecked.toString().toLowerCase()));
                                            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId + " label").text(BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharName);
                                            $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamCharacteristics").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId + " label").attr("data-original-title", BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharName);
                                        }
                                    }
                                }


                            } else if ($(objCurrent).closest("ul").attr("id") == "ulExamCharacteristics") {
                                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamSubCharacteristics").attr("parentId", liId);
                                for (index = 0; index < BillingInformation.selectedPhyExamTempData.length; index++) {
                                    for (var innerIndex in BillingInformation.selectedPhyExamTempData[index].Sections) {
                                        for (var mostInnerIndex in BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                                            for (var mostestInnerIndex in BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics) {
                                                if ($.parseJSON(BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].IsChecked.toString().toLowerCase()))
                                                    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamSubCharacteristics").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId).addClass("green");
                                                else
                                                    $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamSubCharacteristics").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId).removeClass("green");
                                                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamSubCharacteristics").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId + " input:checkbox").prop("checked", $.parseJSON(BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].IsChecked.toString().toLowerCase()));
                                                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamSubCharacteristics").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId + " label").text(BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharName);
                                                $('#' + BillingInformation.params.PanelID + " #frmBillingInformation #ulExamSubCharacteristics").find("li#" + BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId + " label").attr("data-original-title", BillingInformation.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharName);
                                            }

                                        }
                                    }
                                }

                            }

                            //End Farooq Ahmad 02/03/2016 add green class if present in selectedphyexamtempdata

                        });


                    }
                    else {
                        if ($(this).hasClass("active") == true) {
                            $(this).removeClass("active");
                        }
                    }
                });
            }
        }

    },


    //Author: Farooq Ahmad
    //Date: 02-03-2016
    //This function will save physical exam template
    physicalExamTemplateSave: function () {
        var isValid = false;
        var self = null;
        self = $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);

        objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = BillingInformation.specialityCheckedIds.join();
        objData.ProviderIds = objData.PhysicalExamTemplateProvider = BillingInformation.providerCheckedIds.join();

        //Start 11-03-2016 Muhammad Arshad Checks if characteristic/sub-characteristic selected
        if ($(BillingInformation.selectedPhyExamTempData).length > 0) {
            $.each(BillingInformation.selectedPhyExamTempData, function (i, item) {
                if ($(item.Sections).length > 0 && $.parseJSON(item.IsChecked.toString().toLowerCase()) == true) {
                    $.each(item.Sections, function (i, item) {
                        if ($(item.Characteristics).length > 0 && $.parseJSON(item.IsChecked.toString().toLowerCase()) == true) {
                            isValid = false;
                            $.each(item.Characteristics, function (counter, item) {
                                if (item.IsChecked != null && $.parseJSON(item.IsChecked.toString().toLowerCase()) == true) {
                                    isValid = true;
                                }
                            });

                        }
                        else {
                            if ($.parseJSON(item.IsChecked.toString().toLowerCase()))
                                isValid = false;
                            else
                                isValid = true;
                        }
                        if (!isValid) {
                            return false;
                        }
                    });
                }
                else {
                    if ($.parseJSON(item.IsChecked.toString().toLowerCase()))
                        isValid = false;
                    else
                        isValid = true;
                }
                if (!isValid) {
                    return false;
                }
            });
        }
        else {
            isValid = false;
        }
        //End 11-03-2016 Muhammad Arshad Checks if characteristic/sub-characteristic selected

        //Start 11-03-2016 Muhammad Arshad, perform checking if required data Exists prior to save Template
        if (isValid == true) {
            myJSON = JSON.stringify(objData);
            BillingInformation.savePhysicalExamTemplate(myJSON).done(function (response) {
                if (response != null && response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        if (response.phyExamTemplateId != "") {
                            BillingInformation.params.PhysicalExamTemplateId = response.phyExamTemplateId;
                            for (var count in BillingInformation.selectedPhyExamTempData) {

                                BillingInformation.selectedPhyExamTempData[count];


                            }
                        }
                        //BillingInformation.loadHospitalizationHx();

                        BillingInformation.params.mode = "Edit";
                        if (BillingInformation.params.ParentCtrl == "clinicalTabProgressNote" && UnloadHospitalizationhx == true) {
                            BillingInformation.getHospitalizationHxInfo(HospitalizationHxType, UnloadHospitalizationhx);
                            EMRUtility.scrollToPNcomponent('clinical_billinginfo');
                        }
                        $('#' + BillingInformation.params.PanelID + " #hfHospitalizationHxId").val(response.HospitalizationHxId);
                        $('#' + BillingInformation.params.PanelID + ' #frmClinicalHospitalizationHx').data('serialize', $('#' + BillingInformation.params.PanelID + ' #frmClinicalHospitalizationHx').serialize());
                        //
                        //Start Farooq Ahmad 16-03-2016 Unload the Physical Exam on Save
                        UnloadActionPan(BillingInformation.params["ParentCtrl"], "BillingInformation");
                        if (PhysicalExamTemplate != null) {
                            PhysicalExamTemplate.loadPhysicalExamTemplate();
                        }
                        //End Farooq Ahmad 16-03-2016 Unload the Physical Exam on Save
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                }

            });
        }
        else {
            utility.DisplayMessages("Please select characteristic/Sub-characteristic", 3);
        }
        //End 11-03-2016 Muhammad Arshad, perform checking if required data Exists prior to save Template

    },

    //Author: Farooq Ahmad
    //Date: 02-03-2016
    //This function will handle save physical exam template
    //It represents service call to API
    savePhysicalExamTemplate: function (PhysicalExamTemplateData, TemplateName) {
        var self = null;
        self = $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if (BillingInformation.params["mode"] == "Edit") {
            objData["TemplateId"] = (BillingInformation.params["PhysicalExamTemplateId"]);
        }
        else {
            objData["TemplateId"] = '-1';
        }

        if (TemplateName != null) {

            objData["PhysicalExamTemplateName"] = TemplateName;

        }

        objData["SpecialtyIds"] = objData["PhysicalExamTemplateSpecialty"];
        objData["ProviderIds"] = objData["PhysicalExamTemplateProvider"];
        objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = BillingInformation.specialityCheckedIds.join();
        objData.ProviderIds = objData.PhysicalExamTemplateProvider = BillingInformation.providerCheckedIds.join();
        objData["commandType"] = "Save_PhyscialExam_Template";
        objData["SysSecCharSubcharData"] = BillingInformation.selectedPhyExamTempData;

        var data = JSON.stringify(objData);

        //var data = "HospitalizationHxignsData=" + HospitalizationHxignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    //Author: Farooq Ahmad
    //Date: 02-03-2016
    //This function will handle save Template System or Section or Characteristics or SubCharacteristics
    saveTemplateSysSecCharSubchar: function (obj) {



        var MainId = '', Type = '';
        var objData = [];
        if ($(obj).closest("ul").attr("id") == "ulPhysicalExamSystems") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "PhysicalExamSystems";
        } else if ($(obj).closest("ul").attr("id") == "ulPhysicalExamSystemSection") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "PhysicalExamSystemSection";
        } else if ($(obj).closest("ul").attr("id") == "ulExamCharacteristics") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "ExamCharacteristics";
        } else if ($(obj).closest("ul").attr("id") == "ulExamSubCharacteristics") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "ExamSubCharacteristics";
        }
        $(obj).parent().parent().parent().find("span[id^='btnOpenDetail']").click();
        var data = JSON.stringify(objData);
        //return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },


    Save: function (isSigned) {

        utility.callbackAfterAllDOMLoaded(function () {
            $("#frmBillingInformation").data('serialize', $("#frmBillingInformation").serialize());
        });

        $.when(BillingInformation.Re_CalculateCPTFee()).then(function () {
            var self = $('#' + BillingInformation.params.PanelID + ' #tblBillingInformation');
            var Obj = self.getMyJSON();
            Obj = JSON.parse(Obj);
            var ICDs = []
            for (var index = 1; index <= 12; index++) {
                var currentICD = {};
                currentICD.ICDCode9 = Obj["hfICDCode9_" + index];
                currentICD.ICDDescription9 = decodeURIComponent(Obj["hfICDDescription9_" + index]);
                currentICD.ICDCode10 = Obj["hfICDCode10_" + index];
                currentICD.ICDDescription10 = decodeURIComponent(Obj["hfICDDescription10_" + index]);
                currentICD.SNOMEDCode = Obj["hfSNOMEDCode_" + index];
                currentICD.SNOMEDDescription = decodeURIComponent(Obj["hfSNOMEDDescription_" + index]);
                currentICD.CustomFormId = Obj["hfCustomFormId_" + index];
                ICDs.push(currentICD);
            }
            if (ICDs.length > 0) {
                if (ICDs[0].ICDCode9 == "" && ICDs[0].ICDDescription9 == "" && ICDs[0].ICDCode10 == "" && ICDs[0].ICDDescription10 == "" && ICDs[0].SNOMEDCode == "" && ICDs[0].SNOMEDDescription == "") {
                    utility.DisplayMessages("Primary ICD is mandatory.", 3);
                    return;
                }
            }

            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
                if (mm < 10)
                    mm = '0' + mm;
            }
            today = mm + '/' + dd + '/' + yyyy;

            var CPTs = []
            var index = 1;
            $('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').each(function (i, item) {
                if ($(item).find("#txtCPT" + $(item).attr("id")).val() != "") {
                    var currentCPT = {};
                    index = $(item).attr('id');
                    var Type = $(item).attr('Type');
                    var CheckBoxValue = $(item).attr('CheckBoxValue');
                    if (Obj["hfCPTDescription" + index] == null && Obj["hfCPTCode" + index] == null)
                        return true;
                    if (Type != null && typeof Type != typeof undefined && Type != "")
                        currentCPT.Type = Type;
                    else
                        currentCPT.Type = "";
                    if (CheckBoxValue != null && typeof CheckBoxValue != typeof undefined && CheckBoxValue != "")
                        currentCPT.BillingInfoTimeId = CheckBoxValue;
                    else
                        currentCPT.BillingInfoTimeId = "";
                    currentCPT.CPTCode = Obj["hfCPTCode" + index];
                    var cptDespList = $(item).find("#txtCPT" + $(item).attr("id")).val().split(' - ');
                    var cptDesp = "";
                    for (var a = 1; a < cptDespList.length; a++) {
                        if (a == (cptDespList.length - 1))
                            cptDesp = cptDesp + cptDespList[a];
                        else
                            cptDesp = cptDesp + cptDespList[a] + ' - ';
                    }
                    currentCPT.CPTDescription = cptDesp.trim();
                    currentCPT.CPTSNOMEDCodeId = Obj["hfCPTSNOMEDCodeId" + index];
                    currentCPT.BillingInfoCPTId = Obj["hfBillingInfoCPTId" + index];
                    currentCPT.CPTSNOMEDDescription = decodeURIComponent(Obj["hfCPTSNOMEDDescription" + index]);
                    currentCPT.Modifier1 = Obj["txtModifier1" + index];
                    currentCPT.Modifier2 = Obj["txtModifier2" + index];
                    currentCPT.Modifier3 = Obj["txtModifier3" + index];
                    currentCPT.Modifier4 = Obj["txtModifier4" + index];
                    currentCPT.DxPointer1 = Obj["txtDxPointer1" + index];
                    currentCPT.DxPointer2 = Obj["txtDxPointer2" + index];
                    currentCPT.DxPointer3 = Obj["txtDxPointer3" + index];
                    currentCPT.DxPointer4 = Obj["txtDxPointer4" + index];
                    if ($(item).attr('islabbasedcpt') == undefined || $(item).attr('islabbasedcpt') == null || $(item).attr('islabbasedcpt') == "")
                        currentCPT.IsLabBasedCPT = "False";
                    else
                        currentCPT.IsLabBasedCPT = $(item).attr('islabbasedcpt');
                    currentCPT.UnitsId = Obj["txtUnits" + index];
                    currentCPT.POS = $('#' + BillingInformation.params.PanelID).find('#txtPOS').val();

                    currentCPT.txtFEE = Obj["hfFee" + index];
                    currentCPT.hfExpectedFee = Obj["hfExpectedFee" + index];
                    currentCPT.StartDate = Obj["hfStartDate" + index];
                    currentCPT.EndDate = Obj["hfEndDate" + index];
                    if (currentCPT.StartDate == null || currentCPT.StartDate == "")
                        currentCPT.StartDate = today;
                    else
                        currentCPT.StartDate = decodeURIComponent(currentCPT.StartDate);
                    if (currentCPT.EndDate == null || currentCPT.EndDate == "")
                        currentCPT.EndDate = today;
                    else
                        currentCPT.EndDate = decodeURIComponent(currentCPT.EndDate);
                    currentCPT.DOSFrom = currentCPT.StartDate;
                    currentCPT.DOSTo = currentCPT.EndDate;
                    if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                        currentCPT.DOSFrom = decodeURIComponent(Obj["dtpDOSFrom" + index]);
                        currentCPT.DOSTo = decodeURIComponent(Obj["dtpDOSTo" + index]);
                    }
                    currentCPT.CustomFormId = $(item).attr('CustomFormId');
                    CPTs.push(currentCPT);
                }
            });
            if (CPTs.length > 0) {
                if (CPTs[0].CPTCode == "" && CPTs[0].CPTDescription == "") {
                    utility.DisplayMessages("Please select atleast one CPT.", 3);
                    return;
                }
            }
            if (CPTs.length == 0) {
                utility.DisplayMessages("Please select atleast one CPT.", 3);
                return;
            }

            var Obj = $('#' + BillingInformation.params.PanelID + ' #tblBillingInformation').getMyJSONByName();
            Obj = JSON.parse(Obj);
            Obj.CPTCode = $('#' + BillingInformation.params.PanelID + ' #tblEMCodes input[type="checkbox"]:checked').parent().text();
            Obj["NotesId"] = BillingInformation.params.NotesId;
            Obj["NoteDate"] = BillingInformation.params.NoteDate;
            Obj["PatientId"] = BillingInformation.params.PatientId;
            Obj["VisitId"] = BillingInformation.params.VisitId;
            Obj["BillingInfoId"] = BillingInformation.params.BillingInfoId;
            Obj["ProviderId"] = BillingInformation.params.ProviderId;
            var chk = $("#" + BillingInformation.params.PanelID + " #tblEMCodes input[type='checkbox']:checked");
            if (chk.length > 0)
                Obj["ENMTypeId"] = $(chk).attr('typeid').trim();
            else
                Obj["ENMTypeId"] = "";
            ObjTime = $.grep(BillingInformation.BillingInfoTime, function (a) {
                return (a.BillingInfoTimeId == Obj.BillingInfoTimeId && ((Obj.ENMTypeId == "1" && a.Type == "New") || (Obj.ENMTypeId == "2" && a.Type == "Established")));
            });
            if (ObjTime != null && ObjTime.length > 0) {
                ObjTime = ObjTime[0];
                Obj["ENMCPTDescription"] = ObjTime.Description;
                Obj["ENMCPTCode"] = ObjTime.ENMCPT;
                Obj["ENMTimeId"] = ObjTime.BillingInfoTimeId;
                Obj["ENMCPTUnit"] = '1';
                Obj["ENMCPTDOSFrom"] = BillingInformation.params.NoteDate;
                Obj["ENMCPTDOSTo"] = BillingInformation.params.NoteDate;
            } else {
                Obj["ENMCPTDescription"] = Obj["ENMTypeId"] ? $(chk).attr('ENMCPTDescription').trim() : "";
                Obj["ENMCPTCode"] = Obj["ENMTypeId"] ? $(chk).attr('Code').trim() : "";
                Obj["ENMTimeId"] = Obj["ENMTypeId"] ? $(chk).val() : "";
                Obj["ENMCPTUnit"] = Obj["ENMTypeId"] ? '1' : "";
                Obj["ENMCPTDOSFrom"] = BillingInformation.params.NoteDate;
                Obj["ENMCPTDOSTo"] = BillingInformation.params.NoteDate;
            }
            Obj.CPTs = CPTs;
            Obj.ICDs = ICDs;
            Obj.NotesId = BillingInformation.params.NotesId;
            Obj["Status"] = "Draft";
            if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                Obj["BillingInfoType"] = "OutOfOfficeVisit";
                Obj["ProviderId"] = self.find('#hfProvider').val();
                Obj["PatientId"] = $('#PatientProfile #hfPatientId').val();
                Obj["ENMCPTDOSFrom"] = Obj["DOSFrom"];
                Obj["ENMCPTDOSTo"] = Obj["DOSTo"];
                Obj["RefProviderId"] = self.find('#hfRefProvider').val();
                Obj["RefProvider"] = self.find('#txtRefProvider').val();
                Obj["AdmissionDate"] = self.find('#dpAdmissionDate').val();
                Obj["DischargeDate"] = self.find('#dpDischargeDate').val();
            }
            BillingInformation.BillingObj = Obj;
            BillingInformation.BillingInfoSave(Obj).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    BillingInformation.SuggestionCount = 0;
                    // to fetch cpt fee and charges in response
                    response.CPTICDResponse = JSON.parse(response.CPTICDResponse);
                    response.CPTICDResponse = JSON.parse(response.CPTICDResponse.CPTSListFill_JSON);
                    if (response.CPTICDResponse != null && response.CPTICDResponse != "") {
                        response.CPTICDResponse = $.grep(response.CPTICDResponse, function (a) {
                            return a.IsNoteLinked == "1";
                        });
                        BillingInformation.AttachtedCPTData = response.CPTICDResponse;
                        BillingInformation.AttachtedCPTData.reverse();
                    }
                    Clinical_ProgressNote.params.IsOutOfOfficeVisit = false;
                    var parsedObject = null;
                    var BillingInfoObject = null;
                    var BillingInfoCptObject = null;
                    var rowId = 0;
                    var Object = $('#' + BillingInformation.params.PanelID + ' #tblBillingInformation');
                    try {
                        if (response.BillingInfoFillJson) {
                            parsedObject = JSON.parse(response.BillingInfoFillJson);
                            if (parsedObject) {
                                BillingInfoObject = JSON.parse(parsedObject.BillingInfoFill_JSON);
                                if (BillingInfoObject) {
                                    BillingInfoCptObject = JSON.parse(parsedObject.BillingInfoCPTFill_JSON);
                                    if (BillingInfoCptObject) {
                                        $(BillingInfoCptObject).each(function (index, item) {
                                            $('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').each(function (i, tm) {
                                                item.DOSTo = new Date(item.DOSTo);
                                                item.DOSFrom = new Date(item.DOSFrom);
                                                var dayTo = item.DOSTo.getDate();
                                                var monthTo = item.DOSTo.getMonth() + 1;
                                                var yearTo = item.DOSTo.getFullYear();
                                                if (dayTo < 10)
                                                    dayTo = "0" + dayTo;
                                                if (monthTo < 10)
                                                    monthTo = "0" + monthTo;
                                                var dateTo = monthTo + "/" + dayTo + "/" + yearTo;
                                                var dayFrom = item.DOSFrom.getDate();
                                                var monthFrom = item.DOSFrom.getMonth() + 1;
                                                var yearFrom = item.DOSFrom.getFullYear();
                                                if (dayFrom < 10)
                                                    dayFrom = "0" + dayFrom;
                                                if (monthFrom < 10)
                                                    monthFrom = "0" + monthFrom;
                                                var dateFrom = monthFrom + "/" + dayFrom + "/" + yearFrom;
                                                item.DOSTo = dateTo;
                                                item.DOSFrom = dateFrom;
                                                if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                                                    if (item.CPTCode && $(this).find("input:hidden[id*='hfCPTCode']").val() == item.CPTCode && item.CPTCodeDescription && $(this).find("input:hidden[id*='hfCPTDescription']").val() == item.CPTCodeDescription && $(this).find("input[id*='dtpDOSFrom']").val() == item.DOSFrom && $(this).find("input[id*='dtpDOSTo']").val() == item.DOSTo) {
                                                        rowId = $(this).attr("id");
                                                        $(Object).find("#hfBillingInfoCPTId" + rowId).val(item.BillingInfoCPTId ? item.BillingInfoCPTId : "");
                                                        return false;
                                                    }
                                                }
                                                else {
                                                    if (item.CPTCode && $(this).find("input:hidden[id*='hfCPTCode']").val() == item.CPTCode && item.CPTCodeDescription && $(this).find("input:hidden[id*='hfCPTDescription']").val() == item.CPTCodeDescription) {
                                                        rowId = $(this).attr("id");
                                                        $(Object).find("#hfBillingInfoCPTId" + rowId).val(item.BillingInfoCPTId ? item.BillingInfoCPTId : "");
                                                        return false;
                                                    }
                                                }

                                            });
                                        });
                                    }
                                }
                            }
                        }
                    } catch (e) {
                        console.log(e);
                    }

                    if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                        response.BillingInfoFillJson = JSON.parse(response.BillingInfoFillJson);
                        response.BillingInfoFillJson.BillingInfoFill_JSON = JSON.parse(response.BillingInfoFillJson.BillingInfoFill_JSON);
                        BillingInformation.params.ProviderId = response.BillingInfoFillJson.BillingInfoFill_JSON.ProviderId;
                        BillingInformation.params.VisitId = response.BillingInfoFillJson.BillingInfoFill_JSON.VisitId;
                        BillingInformation.params.NotesId = 0;
                        BillingInformation.BillingObj['VisitId'] = BillingInformation.params.VisitId;
                        Obj["VisitId"] = BillingInformation.params.VisitId;
                        BillingInformation.BillingObj['BillingInfoId'] = response.BillingInfoFillJson.BillingInfoFill_JSON.BillingInfoId;
                        Obj["BillingInfoId"] = response.BillingInfoFillJson.BillingInfoFill_JSON.BillingInfoId;
                        $.when(BillingInformation.LoadPatientInfo(BillingInformation.params.PatientId)).done(function () {
                            $("#pnlBillingInformation #hfRefProvider").val(Obj["RefProviderId"]);
                            BillingInformation.params.RefProviderId = $("#pnlBillingInformation #hfRefProvider").val();
                            $("#pnlBillingInformation #txtRefProvider").val(Obj["RefProvider"]);
                        });
                        BillingInformation.params.BillingInfoId = response.BillingInfoFillJson.BillingInfoFill_JSON.BillingInfoId;
                        Clinical_ProgressNote.params.IsOutOfOfficeVisit = true;
                    } else if (BillingInformation.params.ParentCtrl == "clinicalTabProgressNote") {
                        $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val(response.BillingInfoId);


                        BillingInformation.ProblemList = JSON.parse(response.ProblemList);
                        BillingInformation.ProblemList = $.grep(BillingInformation.ProblemList, function (a) {
                            return a.IsNoteLinked == "True";
                        });
                        var hideAlertMessage = null;
                        if (Clinical_ProblemLists != null) {
                            try {
                                var ProblemParams = {};
                                ProblemParams.ProblemListSoapCount = BillingInformation.ProblemList.length;
                                ProblemParams.ProblemListSoap_JSON = JSON.stringify(BillingInformation.ProblemList);

                                //$(BillingInformation.ProblemList).each(function (i, item) {
                                //    BillingInformation.AddProblemOnDrFirst_DB_Call(item.ProblemListId);
                                //}); QA will verify if Problems are adding to Dr First.
                                var state = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML section[id*=Cli_Problems_Main]');
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML section[id*=Cli_Problems_Main]').remove();
                                hideAlertMessage = true;
                                Clinical_ProblemLists.createProblemListBodyHTML(ProblemParams, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, false, true, state);
                            } catch (ex) {
                                console.log(ex);
                            }
                        }

                        BillingInformation.Procedure = JSON.parse(response.Procedure);
                        BillingInformation.Procedure = $.grep(BillingInformation.Procedure, function (a) {
                            return a.IsNoteLinked == "1";
                        });

                        if (Clinical_Procedures != null) {
                            try {
                                var ProcedureParams = {};
                                ProcedureParams.ProcedureSoapCount = BillingInformation.Procedure.length;
                                if (ProcedureParams.ProcedureSoapCount > 0) {
                                    var ProviderCpts = JSON.parse(response.ProviderCPTs);
                                    for (index = 0; index < BillingInformation.Procedure.length; index++) {
                                        var cptItem = $.grep(ProviderCpts, function (item, i) {
                                            return item.CPTCode == BillingInformation.Procedure[index].CPTCode && item.CPTDescription == BillingInformation.Procedure[index].CPT_DESCRIPTION && item.ShowCPTCode == "0"
                                        });
                                        if (cptItem.length > 0) {
                                            BillingInformation.Procedure[index].ProcedureCodeName = BillingInformation.Procedure[index].CPT_DESCRIPTION;
                                        }
                                        else {
                                            BillingInformation.Procedure[index].ProcedureCodeName = BillingInformation.Procedure[index].CPTCode + ' - ' + BillingInformation.Procedure[index].CPT_DESCRIPTION;
                                        }

                                        BillingInformation.Procedure[index].Diagnosis = '';
                                    }
                                    ProcedureParams.ProcedureSoap_JSON = JSON.stringify(BillingInformation.Procedure);
                                    var state = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML section[id*=Cli_Procedures_Main]');
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML section[id*=Cli_Procedures_Main]').remove();
                                    Clinical_Procedures.createProceduresBodyHTML(ProcedureParams, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage, true, true, true, state);
                                }

                            } catch (ex) {
                                console.log(ex);
                            }
                        }
                    }

                    if (BillingInformation.params.NotesId != null && parseInt(BillingInformation.params.NotesId) > 0) {
                        BillingInformation.params.BillingInfoId = response.BillingInfoId;
                        BillingInformation.getBillingInfo(response.BillingInfoId);
                    }
                    if (isSigned == true) {
                        if (Clinical_ProgressNote != null) {
                            var customSigMsg = "";
                            if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                                customSigMsg = "Are you sure you want to sign eSuperbill?";
                            }
                            else if (BillingInformation.params.ParentCtrl == "schTabCalendar" || BillingInformation.params.ParentCtrl == "schTabMultipleView") {
                                customSigMsg = "Are you sure you want to sign eSuperbill?";
                                //Start 19-12-2016 Zain ul abdin for EMR-2252
                                BillingInformation.params.BillingInfoId = response.BillingInfoId;
                                //End 19-12-2016 Zain-ul-abdin for EMR-2252
                                Obj["prntCtrl"] = BillingInformation.params.ParentCtrl;
                            }
                            $.when(Clinical_ProgressNote.Sign(BillingInformation, Obj, customSigMsg, false, 'BillingInformation', false, BillingInformation.params.NotesId, true)).then(function () {
                                if (BillingInformation != null) {
                                    if (BillingInformation.params.ParentCtrl == "clinicalTabProgressNote") {
                                        if ($('#' + Clinical_ProgressNote.params.PanelID + ' #hfNoteStatus').val() == 'Signed' && $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val() > 0
                                            && !Clinical_ProgressNote.params.CCMForSigned) {
                                            $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
                                            if (Clinical_ProgressNote.params.IsNonBilable && !Clinical_ProgressNote.params.CCMForSigned)
                                                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
                                            $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + Clinical_ProgressNote.params.VisitId + "','" + Clinical_ProgressNote.params.PatientId + "',event)");
                                        }
                                        else {
                                            $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
                                        }
                                        EMRUtility.scrollToPNcomponent('clinical_billinginfo');
                                    }
                                    else if (BillingInformation.params.ParentCtrl == "schTabCalendar" || BillingInformation.params.ParentCtrl == "schTabMultipleView") {
                                        Scheduling_Calendar.getAllESuperbillsDbCall().done(function (response) {
                                            response = JSON.parse(response);
                                            if (response.status != false) {
                                                Scheduling_Calendar.eSuperbillInfo = [];
                                                response.BillingInfoFill_JSON = JSON.parse(response.BillingInfoFill_JSON);
                                                $.each(response.BillingInfoFill_JSON, function () {
                                                    Scheduling_Calendar.eSuperbillInfo.push($(this));

                                                });
                                                if (BillingInformation.params.ParentCtrl == "schTabCalendar") {
                                                    if (BillingInformation.params.BtnClicked == 'week') {
                                                        $('#pnlScheduleCalendar').find('#btnweek').trigger('click');
                                                    }
                                                    else if (BillingInformation.params.BtnClicked == 'day') {
                                                        $('#pnlScheduleCalendar').find('#btnday').trigger('click');
                                                    }
                                                }
                                                else {
                                                    Scheduling_MuliView.LoadMultipleViewCalendar();
                                                }
                                            } else {
                                                Scheduling_Calendar.eSuperbillInfo = [];
                                            }
                                            BillingInformation.DirectUnLoad();
                                        });
                                    }
                                    Clinical_NotesSearch.SetNotesCount();
                                }
                                Clinical_ProgressNote.params.isCQMExists == 1 || Clinical_ProgressNote.params.isVBPExists == 1 ? "" : BillingInformation.params.ParentCtrl && BillingInformation.params["ParentCtrl"] == "billTabOutOfOfficeVisits" ? function () { OutOfOfficeVisits.loadOOOVisit('0'); UnloadActionPan(BillingInformation.params["ParentCtrl"], "BillingInformation"); }() : UnloadActionPan(BillingInformation.params["ParentCtrl"], "BillingInformation");

                            });
                        }
                        else if (BillingInformation.params["NoteStatus"] == "Signed")
                            BillingInformation.CreateCharge(Obj);
                    }
                    else if (BillingInformation.params.ParentCtrl != "clinicalTabProgressNote") {
                        BillingInformation.HideAlert = true;
                        utility.DisplayMessages(response.message, 1);
                        BillingInformation.DirectUnLoad();
                    }
                    else {
                        BillingInformation.DirectUnLoad();
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 4)
                }
            });
        });
    },
    DirectUnLoad: function () {

        if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
            OutOfOfficeVisits.loadOOOVisit();
            OutOfOfficeVisits.loadProviderFailityddl();
        }
        if (BillingInformation.params != null && BillingInformation.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.scrollToPNcomponent('clinical_billinginfo');
        }
        UnloadActionPan(BillingInformation.params["ParentCtrl"], "BillingInformation");

    },

    SaveVisit: function (VisitData, PatientID) {
        var data = "";

        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "SAVE_VISIT");
    },

    BillingInfoSave: function (objData) {

        objData["commandType"] = "BILLING_INFORMATION_SAVE";
        var hasCustomForm = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit clinical_customform');
        if (hasCustomForm.length > 0) {
            objData["ShowCustomFormData"] = true;
        }
        else {
            objData["ShowCustomFormData"] = false;
        }

        var hasImplantableDevice = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit clinical_implantabledevices');
        if (hasImplantableDevice.length > 0) {
            objData["ShowImplantableDeviceData"] = true;
        }
        else {
            objData["ShowImplantableDeviceData"] = false;
        }
        if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
            objData["ProviderId"] = $("#frmBillingInformation #hfProvider").val();
        }
        else {
            objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },

    BillingInfoSIGN: function (status) {
        var objData = {};
        objData["commandType"] = "SIGNED_BILLINGINFO";
        objData["BillingInfoId"] = BillingInformation.params.BillingInfoId;
        objData["Status"] = status;//"Signed";

        if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
            OutOfOfficeVisits.loadOOOVisit();
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");

    },

    //Author: Farooq Ahmad
    //Date: 02-03-2016
    //This function will validate physical exam template
    validatePhysicalExamTemplate: function () {
        $('#' + BillingInformation.params.PanelID + ' #frmBillingInformation')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   PhysicalExamTemplateName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PhysicalExamTemplateEntity: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            BillingInformation.physicalExamTemplateSave();
        });
    },

    //Author: Farooq Ahmad
    //Date : 07-01-2016
    saveAsPhysicalExam: function () {
        var strMessage = "";
        var params = [];
        params["FromAdmin"] = '0';
        params["TabID"] = "PhysicalExamTemplateSaveAs";
        params["ParentCtrl"] = 'BillingInformation';


        LoadActionPan('PhysicalExamTemplateSaveAs', params, BillingInformation.params.PanelID);

    },

    UnLoadPlan: function () {

        if ($('#frmProviderLicense').serialize() != $('#frmProviderLicense').data('serialize')) {
            utility.myConfirm('2', function () {
                $('#ProviderLicenseDetailGrid').modal('hide');
            }, function () { },
                    '2'
                );
        }
        else {
            $('#ProviderLicenseDetailGrid').modal('hide');
        }


    },



    UnLoad: function () {

        //Abid Ali//Perform Actions on unload if parent control is Scheduling_Calender
        BillingInformation.AttachtedCPTData = [];
        if (BillingInformation.params.ParentCtrl == "schTabCalendar" || BillingInformation.params.ParentCtrl == "schTabMultipleView") {

            //relaod all eSuperbills on save/updating the link.


            Scheduling_Calendar.getAllESuperbillsDbCall().done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {

                    Scheduling_Calendar.eSuperbillInfo = [];

                    response.BillingInfoFill_JSON = JSON.parse(response.BillingInfoFill_JSON);

                    $.each(response.BillingInfoFill_JSON, function () {

                        Scheduling_Calendar.eSuperbillInfo.push($(this));

                    });
                    //  var dfd = new $.Deferred();
                    if (BillingInformation.params.ParentCtrl == "schTabCalendar") {
                        if (BillingInformation.params.BtnClicked == 'week') {
                            $('#pnlScheduleCalendar').find('#btnweek').trigger('click');
                        }
                        else if (BillingInformation.params.BtnClicked == 'day') {
                            $('#pnlScheduleCalendar').find('#btnday').trigger('click');
                        }
                    }
                    else {
                        Scheduling_MuliView.LoadMultipleViewCalendar();
                    }

                } else {
                    Scheduling_Calendar.eSuperbillInfo = [];
                }

                //Unload Tab

                BillingInformation.UnloadForm();

            });

        }
        else {

            BillingInformation.UnloadForm();

            setTimeout(function () {
                if ($('body').find('.modal-backdrop').length == 0) {
                    $(document.body).removeClass('modal-open');
                }
            }, 501);
            if (BillingInformation.params.ParentCtrl == "BillingInformation") {
                var PanelChargeGrid = "#" + BillingInformation.params.PanelID + " #pnlVisitCharge_Result";
                var ChargeGridId = "#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge";
                EditableGrid.initialize(ChargeGridId, BillingInformation, "0", false, false, false, false, undefined, false);
            } else if (BillingInformation.params.IsFromEncounter == true) {
                var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
                var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
                EditableGrid.initialize(ChargeGridId, EncounterChargeCapture, "0", false, false, false, false, undefined, false, true);
            }
        }
        setTimeout(function () {
            if ($('body').find('.modal-backdrop').length == 0) {
                $(document.body).removeClass('modal-open');
            }
        }, 501);

    },

    UnloadForm: function () {
        if (BillingInformation.params["ParentCtrl"] != null) {
            if (BillingInformation.params["ParentCtrl"] == "Clinical_NotesView") {
                UnloadActionPan(BillingInformation.params["ParentCtrl"], "BillingInformation");
                if (BillingInformation.params["ParentCtrl"] == "billTabOutOfOfficeVisits") {
                    OutOfOfficeVisits.loadOOOVisit('0');
                }
            }
            else {
                utility.UnLoadDialog('frmBillingInformation', function () {

                    UnloadActionPan(BillingInformation.params["ParentCtrl"], "BillingInformation");
                    if (BillingInformation.params["ParentCtrl"] == "billTabOutOfOfficeVisits") {
                        OutOfOfficeVisits.loadOOOVisit('0');
                    }
                }, function () {
                    UnloadActionPan(BillingInformation.params.ParentCtrl, 'BillingInformation');
                    if (BillingInformation.params["ParentCtrl"] == "billTabOutOfOfficeVisits") {
                        OutOfOfficeVisits.loadOOOVisit('0');
                    }
                });
            }

        }

        else {
            UnloadActionPan("BillingInformation");
        }


        //utility.UnLoadDialog("frmBillingInformation", function () {
        //    UnloadActionPan(BillingInformation.params["ParentCtrl"], "BillingInformation");

        //}, function () {
        //    UnloadActionPan(BillingInformation.params["ParentCtrl"], "BillingInformation");
        //});
    },
    //-------------------Editable Grid Methods Starts---

    rowSave: function ($row, obj) {

        if (obj.rowValidate($row)) {

            var _self = obj,
            $actions,
            values = [];

            if ($row.hasClass('adding')) {
                $row.removeClass('adding');
            }

            values = $row.find('td').map(function () {

                var $this = $(this);

                if ($this.hasClass('expand')) {
                    return '<a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
                }
                else if ($this.hasClass('actions')) {

                    return _self.datatable.cell(this).data();
                }
                else if ($this.hasClass('ddl')) {
                    return $.trim($this.find('select').val());

                } else {
                    return $.trim($this.find('input').val());
                }
            });

            var id = $row.attr("id");
            var myJSON = $row.getMyJSON();


            if (id && id > 0) {
                //Edit Record
                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        BillingInformation.UpdateProviderLicense(myJSON, id).done(function (response) {
                            if (response.status != false) {

                                utility.DisplayMessages(response.Message, 1);
                                BillingInformation.rowDraw($row, _self, values);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                });
            }
            else {
                //Add Record

                AppPrivileges.GetFormPrivileges("Provider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        BillingInformation.SaveProviderLicense(myJSON, id).done(function (response) {
                            if (response.status != false) {

                                $row.attr("id", response.ProviderLicenseInfoId);
                                $row.attr("onclick", "utility.SelectGridRow($(this))");
                                utility.DisplayMessages(response.Message, 1);
                                BillingInformation.rowDraw($row, _self, values);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
        }
    },

    rowAdd: function () {

        AppPrivileges.GetFormPrivileges("Provider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd(null, null, null, null, null, null, BillingInformation.onChangecheckBox, false);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowCopy: function ($row, obj) {

        $.when(BillingInformation.rowCopyNewChargeRowWrapper()).then(function () {
            var addedRow = $row.parent().find("[mode=copy]");
            addedRow.removeAttr("mode");
            addedRow.find('input[id*="txtCPT"]').val($row.find('input[id*="txtCPT"]').val());
            addedRow.find('input[id*="txtModifier1"]').val($row.find('input[id*="txtModifier1"]').val());
            addedRow.find('input[id*="txtModifier2"]').val($row.find('input[id*="txtModifier2"]').val());
            addedRow.find('input[id*="txtModifier3"]').val($row.find('input[id*="txtModifier3"]').val());
            addedRow.find('input[id*="txtModifier4"]').val($row.find('input[id*="txtModifier4"]').val());
            addedRow.find('input[id*="txtDxPointer1"]').val($row.find('input[id*="txtDxPointer1"]').val());
            addedRow.find('input[id*="txtDxPointer2"]').val($row.find('input[id*="txtDxPointer2"]').val());
            addedRow.find('input[id*="txtDxPointer3"]').val($row.find('input[id*="txtDxPointer3"]').val());
            addedRow.find('input[id*="txtDxPointer4"]').val($row.find('input[id*="txtDxPointer4"]').val());
            addedRow.find('input[id*="txtUnits"]').val($row.find('input[id*="txtUnits"]').val());
            addedRow.find('input[id*="hfCPTCode"]').val($row.find('input[id*="hfCPTCode"]').val());
            addedRow.find('input[id*="dtpDOSFrom"]').val("");
            addedRow.find('input[id*="dtpDOSTo"]').val("");
            addedRow.find('input[id*="hfCPTDescription"]').val($row.find('input[id*="hfCPTDescription"]').val());
            addedRow.find('input[id*="hfFee"]').val($row.find('input[id*="hfFee"]').val());
            addedRow.find('input[id*="hfExpectedFee"]').val($row.find('input[id*="hfExpectedFee"]').val());
            utility.SetAutoCompleteSource(addedRow.find('input[id*="txtCPT"]'), addedRow.find('input[id*="hfCPTCode"]'));
            BillingInformation.AddNewChargeRow(null, 'Add');
        });
    },
    rowCopyNewChargeRowWrapper: function () {
        var _def = $.Deferred();
        var found = false;
        $('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').each(function (i, item) {
            if ($(item).find('input[id*="txtCPT"]').val() == "")
                found = true;
            if (found) {
                EMRUtility.ValidateFromToDate('pnlBillingInformation', $(item).find('input[id*="dtpDOSFrom"]').attr('id'), $(item).find('input[id*="dtpDOSTo"]').attr('id'), true, function () { }, function () { }, "DOS To should be greater than DOS From");
                $(item).find('input[id*="dtpDOSFrom"]').prop('disabled', false);
                $(item).find('input[id*="dtpDOSTo"]').prop('disabled', false);
                $(item).attr("mode", "copy");
                _def.resolve();
                return false;
            }
        });
        if (!found) {
            $.when(BillingInformation.AddNewChargeRow(null, 'copy')).then(function () {
                _def.resolve();
            });
        }
        else
            _def.resolve();
        return _def;
    },
    DeleteCPT: function (CPTCode, BillingInfoId) {
        var objData = {};
        objData["CPTCode"] = CPTCode;
        objData["BillingInfoId"] = BillingInfoId;
        objData["PatientId"] = BillingInformation.params.PatientId;
        objData["NotesId"] = BillingInformation.params.NotesId;
        objData["commandType"] = "Delete_CPTid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");


    },

    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        var Type = $row.attr("Type");
        var CheckBoxValue = $row.attr("CheckBoxValue");
        AppPrivileges.GetFormPrivileges("Provider", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {

                        var CPTCode = $("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tr#" + selectedValue).find("input[id*='hfCPTCode']").val()

                        if ((BillingInformation.params.BillingInfoId != null && BillingInformation.params.BillingInfoId != "" && parseInt(BillingInformation.params.BillingInfoId) > 0) && CPTCode != '' && CPTCode != null) {
                            BillingInformation.DeleteCPT(CPTCode, BillingInformation.params.BillingInfoId).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {

                                    var ObjTime = $.grep(BillingInformation.BillingInfoTime, function (a) {
                                        return (a.ENMCPT == CPTCode);
                                    });

                                    if (ObjTime != null && ObjTime.length > 0)
                                        $("#" + BillingInformation.params.PanelID + " #tblEMCodes [type='checkbox']").prop("checked", false);

                                    if (Type != null && (typeof Type != typeof undefined) && Type != "" && CheckBoxValue != null && (typeof CheckBoxValue != typeof undefined) && CheckBoxValue != "") {

                                        var Found = false;
                                        var CopyEMCheckedCheckBoxArray = [];
                                        $.grep(BillingInformation.EMCheckedCheckBoxArray, function (item, index) {
                                            if (item.Type == Type && item.CheckBoxVal == CheckBoxValue) {
                                                Found = true;
                                            }
                                            else {
                                                CopyEMCheckedCheckBoxArray.push(item);
                                            }
                                        });
                                        BillingInformation.EMCheckedCheckBoxArray = CopyEMCheckedCheckBoxArray;
                                    }
                                    if ($row.hasClass('adding')) {
                                    }
                                    var _self = obj;
                                    _self.datatable.row($row.get(0)).remove().draw();

                                    BillingInformation.ReorderCPTs();
                                    BillingInformation.EmptyRowsGoDown();
                                    utility.DisplayMessages(response.message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else {
                            var ObjTime = $.grep(BillingInformation.BillingInfoTime, function (a) {
                                return (a.ENMCPT == CPTCode);
                            });

                            if (ObjTime != null && ObjTime.length > 0)
                                $("#" + BillingInformation.params.PanelID + " #tblEMCodes [type='checkbox']").prop("checked", false);

                            if (Type != null && (typeof Type != typeof undefined) && Type != "" && CheckBoxValue != null && (typeof CheckBoxValue != typeof undefined) && CheckBoxValue != "") {

                                var Found = false;
                                var CopyEMCheckedCheckBoxArray = [];
                                $.grep(BillingInformation.EMCheckedCheckBoxArray, function (item, index) {
                                    if (item.Type == Type && item.CheckBoxVal == CheckBoxValue) {
                                        Found = true;
                                    }
                                    else {
                                        CopyEMCheckedCheckBoxArray.push(item);
                                    }
                                });
                                BillingInformation.EMCheckedCheckBoxArray = CopyEMCheckedCheckBoxArray;
                            }
                            var _self = obj;
                            _self.datatable.row($row.get(0)).remove().draw();
                            BillingInformation.ReorderCPTs();
                            BillingInformation.EmptyRowsGoDown();
                        }
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    EnableDisableICDs: function () {




        var self = null;
        if (BillingInformation.params.PanelID.indexOf("pnlBillingInformation") < 0)
            self = $('#' + BillingInformation.params.PanelID + ' #pnlBillingInformation');
        else
            self = $('#' + BillingInformation.params.PanelID);
        for (var Id = 2; Id <= 12; Id++) {
            var icd = self.find('#txtDisease_' + Id);
            if (icd && icd.val() == "" && self.find('#txtDisease_' + (Number(Id) - 1)).val() == "") {
                icd.prop("disabled", true);
                self.find('#btnDisease_' + Id).prop("disabled", true);
            }
            else if (icd) {
                icd.prop("disabled", false);
                self.find('#btnDisease_' + Id).prop("disabled", false);
            }
        }
        //$('#frmBillingInformation').data('serialize', $('#frmBillingInformation').serialize());
        //BillingInformation.BindICDNValues(self);
    },

    ReorderCPTs: function () {
        var ENMCPT = $('#' + BillingInformation.params.PanelID + ' #tblEMCodes input:checked').parent().text();

        if (ENMCPT != null && ENMCPT != '') {
            var PrimaryTr = $("#" + BillingInformation.params.PanelID + " #pnlVisitCharge_Result #dgvBillVisitCharge tbody ").find("input:hidden[value='" + ENMCPT + "']").closest('tr');
            if (PrimaryTr != null && PrimaryTr.length > 0) {
                if ($(PrimaryTr).index() > 0) {
                    $(PrimaryTr).insertBefore($("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr:first"));
                }
            }
        }
    },

    rowCancel: function ($row, obj) {


        var _self = obj,
    $actions,
    i,
    data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                _self.rowSetActionsDefault($row);
            }

            _self.datatable.draw();
        }
    },

    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },
    validateLength: function (ev) {
        if ($(ev).val().length != 10) {
            $(ev).val('');
        }

    },
    //-------------------Editable Grid Methods Ends---

    //=== For loading Template======

    SignNotes: function () {

    },

    useSuperbillTemplates: function (appointmentId, patientId) {

        var params = [];
        params["FromAdmin"] = "0";
        params["NoteId"] = BillingInformation.params.NotesId;
        // params["PreviousTab"] = GetSelectedTab();
        params["ParentCtrl"] = 'BillingInformation';
        params["PatientId"] = BillingInformation.params.PatientId;
        params["ProviderId"] = BillingInformation.params.ProviderId;

        if (BillingInformation.params.appId != null && parseInt(BillingInformation.params.appId) > 0) {
            params["appid"] = BillingInformation.params.appId;
        }
        LoadActionPan('Clinical_SuperBillTemplate', params);
    },

    //==============================
    UpdateVisit: function (VisitData, VisitID, PatientID, SelectedInsurancePlan) {
        var data = "VisitData=" + VisitData + "&VisitID=" + VisitID + "&PatientID=" + PatientID + "&SelectedPatientInsurance=" + SelectedInsurancePlan;// + "&SelectedPatientInsurance=" + EncounterChargeCapture.GetSelectedInsurancePlan();
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "UPDATE_VISIT");
    },

    SearchVisits: function (VisitData, PatientID, VisitId, PageNumber, RowsPerPage, VisitStatus) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "VisitData=" + VisitData + "&VisitID=" + VisitId + "&PatientID=" + PatientID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage + "&VisitStatus=" + VisitStatus;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "SEARCH_VISITS");
    },

    CreateCharge: function (obj, isFromNotes) {
        if (BillingInformation.params.NotesId != null && BillingInformation.params.NotesId > 0) {
            Clinical_ProgressNote.FillNotes(null, BillingInformation.params.NotesId, BillingInformation.params.PatientId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    if (Clinical_Notes_detail.IsNonBilable.toString().toLowerCase() == "true") {
                        utility.myConfirm('System will not create claim. Are you sure you want to make the Visit as Non Billable?', function () {
                            BillingInformation.BillingInfoSIGN("Signed").done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    BillingInformation.Status = 'Signed';
                                    if (BillingInformation.params.ParentCtrl == "clinicalTabProgressNote") {
                                        $('#pnlClinicalProgressNote').find('#btnNoteViewCharges').addClass('disabled');
                                    }
                                    $("#" + BillingInformation.params.PanelID + " #divOutOfOfficeVisit,#divEvalnMangement,#divICDCPT").addClass("disableAll");
                                    utility.DisplayMessages("eSuperbill Sign Successfully.", 1);
                                }
                            });
                        }, function () {

                            BillingInformation.RollBackSign();

                        }, 'Confirm Non Billable');
                    } else {
                        BillingInformation.ChargeCreation(obj, isFromNotes, Clinical_Notes_detail.ResourceProviderId);
                    }
                } else {
                    utility.DisplayMessages(response.Message, 3);
                    BackgroundLoaderShow(false);
                }
            });
        }
        else if (BillingInformation.params.AppointmentId != null && BillingInformation.params.AppointmentId != "" && BillingInformation.params.AppointmentId != "0") {
            BillingInformation.FillAppointment(BillingInformation.params.AppointmentId).done(function (response) {

                if (response.status != false) {
                    var appointment_detail = JSON.parse(response.AppointmentFill_JSON);
                    if (appointment_detail.chkNonBilable.toLowerCase() == "true") {
                        utility.myConfirm('System will not create claim. Are you sure you want to make the Visit as Non Billable?', function () {
                            BillingInformation.BillingInfoSIGN("Signed").done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    if (BillingInformation.params.ParentCtrl == "clinicalTabProgressNote") {
                                        $('#pnlClinicalProgressNote').find('#btnNoteViewCharges').addClass('disabled');
                                    }
                                    $("#" + BillingInformation.params.PanelID + " #divOutOfOfficeVisit,#divEvalnMangement,#divICDCPT").addClass("disableAll");
                                    utility.DisplayMessages("eSuperbill Sign Successfully.", 1);
                                }
                            });
                        }, function () {
                            BillingInformation.RollBackSign();
                            $("#" + BillingInformation.params.PanelID + " #tblNoteInformation ").addClass("btn-block");
                            $("#" + BillingInformation.params.PanelID + " #tblNoteInformationBottom ").addClass("btn-block");

                        }, 'Confirm Non Billable');

                    } else {
                        BillingInformation.ChargeCreation(obj, isFromNotes);
                    }
                } else {
                    utility.DisplayMessages(response.Message, 3);
                    BackgroundLoaderShow(false);
                }
            });
        }
        else {
            BillingInformation.ChargeCreation(obj, isFromNotes);
        }
    },

    RollBackSign: function () {
        $('#pnlClinicalProgressNote #ProgressnoteHTML').find('img#img_eSignature_ProgressNotes').parent().remove();
        $('#pnlClinicalProgressNote #ProgressnoteHTML').find('ul#signedByProvider').remove();
        $('#pnlClinicalProgressNote #ProgressnoteHTML').find('li:contains("e-Signed by:")').remove();
        $('#pnlClinicalProgressNote #hfNoteText').val($('#pnlClinicalProgressNote #ProgressnoteHTML').html());
        $('#pnlClinicalProgressNote #hfNoteStatus').val("Draft");
        var self = $("#" + Clinical_Notes.params["PanelID"]);
        var myJSON = self.getMyJSONByName();
        var NotesId = BillingInformation.params.NotesId;
        if (parseInt(NotesId) > 0) {
            Clinical_Notes.NotesUpdate(myJSON, NotesId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    BillingInformation.BillingInfoSIGN("Draft").done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            $('#pnlClinicalProgressNote').find('#btnSend,#btnReview,#btnCreate_eSupperbill,#btnNoteCoSign,#btnNoteAmendment').addClass('disabled');
                            $('#pnlClinicalProgressNote').find('div#InitialOfficeVisit,div#sidebar-wrapper').removeClass('disableAll');
                            $('#pnlClinicalProgressNote').find('#ChkBox_IsNonBilable').parent().removeClass('disableAll');
                            $('#pnlClinicalProgressNote').find('#btnSave,#btnSign,#btnPrint,#btnAssign,#btnBillingInfo,#btnCreateLetter,#btnNotesDelete,#btnSyndromicSurveillance').removeClass('disabled');


                            $("#" + BillingInformation.params.PanelID + " #divOutOfOfficeVisit,#divEvalnMangement,#divICDCPT").addClass("disableAll");
                            $("#" + BillingInformation.params.PanelID + " #tblNoteInformation ").addClass("btn-block");
                            $("#" + BillingInformation.params.PanelID + " #tblNoteInformation ").find("#btnSave,#btnSign").removeClass("disabled");
                            $("#" + BillingInformation.params.PanelID + " #divOutOfOfficeVisit,#divEvalnMangement,#divICDCPT").removeClass("disableAll");
                            $("#" + BillingInformation.params.PanelID + " #tblNoteInformation ").find("#btnSave,#btnSign").removeClass("disabled");

                            $("#" + BillingInformation.params.PanelID + " #tblNoteInformationBottom").addClass("btn-block");
                            $("#" + BillingInformation.params.PanelID + " #tblNoteInformationBottom").find("#btnSaveBottom,#btnSignBottom").removeClass("disabled");
                            $("#" + BillingInformation.params.PanelID + " #tblNoteInformationBottom").find("#btnSaveBottom,#btnSignBottom").removeClass("disabled");

                        }
                    });

                }
            });
        }
    },

    FillAppointment: function (AppointmentID) {
        var data = "AppointmentID=" + AppointmentID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "FILL_APPOINTMENT");
    },
    removeDupRcdsByCPTCode: function (array) {
        var a = [];
        var l = array.length;
        for (var i = 0; i < l; i++) {
            for (var j = i + 1; j < l; j++) {
                if (array[i].CPTCode == array[j].CPTCode)
                    j = ++i;
            }
            a.push(array[i]);
        }
        return a;
    },
    AddtoJsonObjectInCPTObject: function (array) {
        var resultedIndex = -1;
        var l = array.length;
        for (var i = 0; i < l; i++) {
            var CurrentCpt = $.grep(BillingInformation.AttachtedCPTData, function (a) {
                return a.CPTCode == array[i].CPTCode;
            })[0];
            if (CurrentCpt) {
                if (array[i].UnitsId == null || array[i].UnitsId == "" || array[i].UnitsId == 0) {
                    array[i].UnitsId = CurrentCpt.Unit;
                    if (array[i].UnitsId == "") {
                        array[i].UnitsId = "1";
                    }
                }
                if (array[i].txtFEE == null || array[i].txtFEE == "" || array[i].txtFEE == 0) {
                    array[i].txtFEE = CurrentCpt.Fee;
                    if (array[i].txtFEE == "") {
                        array[i].txtFEE = "0.00";
                    }
                }
                if (array[i].Inscharges == null || array[i].Inscharges == "") {
                    array[i].Inscharges = CurrentCpt.Inscharges;
                } else array[i].Inscharges = "0.00";
                if (array[i].PatCharges == null || array[i].PatCharges == "") {
                    array[i].PatCharges = CurrentCpt.PatCharges;
                } else array[i].PatCharges = "0.00";
                if (array[i].Copay == null || array[i].Copay == "") {
                    array[i].Copay = CurrentCpt.PatChargesCopay;
                } else array[i].Copay = "0.00";
                if (array[i].Expectedfee == null || array[i].Expectedfee == "") {
                    array[i].Expectedfee = CurrentCpt.Expectedfee;
                    if (array[i].Expectedfee == "") {
                        array[i].Expectedfee = "0.00";
                    }
                } else array[i].Expectedfee = "0.00";
            }
        }
        return a;
    },

    ChargeCreation: function (obj, isFromNotes, ResourceProvider) {

        try {
            var JSONObject = {
                "dtpHoldTill": "",
                "txtPatientName": "",
                "txtPatientFullName": "",
                "dtpAppointmentDate": "",
                "dtpEncounterSignOffDate": "",
                "dtpDOSFrom": "",
                "dtpDOSTo": "",
                "dtpSubmittedDate": "",
                "txtSubmittedBy": "",
                "txtReferralNumber": "",
                "txtCaseNumber": "",
                "txtBatchNumber": "",
                "txtClaimNumber": "",
                "txtProvider": "",
                "txtFacility": "",
                "txtRefProvider": "",
                "txtResourceProvider": "",
                "txtSupervisingProvider": "",
                "dtpClaimDate": "",
                "txtVisitCopayment": "0",
                "txtPriorAuthNumber": "",
                "txtClaimComments": "",
                "PatientOutstanding_SelectedDataKeys": "",
                "txtPatCharges": "0.00",
                "txtInsCharges": "0.00",
                "ddlBox24BShaded": "1",
                "ddlBox24IJShaded": "1",
                "txtTotalCharges": "0.00",
                "txtPatPayments": "0.00",
                "txtInsPayments": "0.00",
                "txtTotalPayments": "0.00",
                "txtPatAdjust": "0.00",
                "txtInsAdjust": "0.00",
                "txtTotalAdjust": "0.00",
                "txtPatBalances": "0.00",
                "txtInsBalances": "0.00",
                "txtTotalBalances": "0.00",
                "txtAnesthesiologist": "",
                "txtAnesthesiologistStartTime": "",
                "txtAnesthesiologistEndTime": "",
                "txtCRNA": "",
                "txtCRNAStartTime": "",
                "txtCRNAEndTime": "",
                "txtRiskUnits": "",
                "txtAnesthesiaComments": "",
                "txtICD1": "",
                "hfICD1": "",
                "hfICD101": "",
                "hfICDDescription1": "",
                "hfSNOMED1": "",
                "hfSNOMEDDescription1": "",
                "txtICD10Description1": "",
                "txtICD4": "",
                "hfICD4": "",
                "hfICD104": "",
                "hfICDDescription4": "",
                "hfSNOMED4": "",
                "hfSNOMEDDescription4": "",
                "txtICD10Description4": "",
                "txtICD7": "",
                "hfICD7": "",
                "hfICD107": "",
                "hfICDDescription7": "",
                "hfSNOMED7": "",
                "hfSNOMEDDescription7": "",
                "txtICD10Description7": "",
                "txtICD10": "",
                "hfICD10": "",
                "hfICD1010": "",
                "hfICDDescription10": "",
                "hfSNOMED10": "",
                "hfSNOMEDDescription10": "",
                "txtICD10Description10": "",
                "txtICD2": "",
                "hfICD2": "",
                "hfICD102": "",
                "hfICDDescription2": "",
                "hfSNOMED2": "",
                "hfSNOMEDDescription2": "",
                "txtICD10Description2": "",
                "txtICD5": "",
                "hfICD5": "",
                "hfICD105": "",
                "hfICDDescription5": "",
                "hfSNOMED5": "",
                "hfSNOMEDDescription5": "",
                "txtICD10Description5": "",
                "txtICD8": "",
                "hfICD8": "",
                "hfICD108": "",
                "hfICDDescription8": "",
                "hfSNOMED8": "",
                "hfSNOMEDDescription8": "",
                "txtICD10Description8": "",
                "txtICD11": "",
                "hfICD11": "",
                "hfICD1011": "",
                "hfICDDescription11": "",
                "hfSNOMED11": "",
                "hfSNOMEDDescription11": "",
                "txtICD10Description11": "",
                "txtICD3": "",
                "hfICD3": "",
                "hfICD103": "",
                "hfICDDescription3": "",
                "hfSNOMED3": "",
                "hfSNOMEDDescription3": "",
                "txtICD10Description3": "",
                "txtICD6": "",
                "hfICD6": "",
                "hfICD106": "",
                "hfICDDescription6": "",
                "hfSNOMED6": "",
                "hfSNOMEDDescription6": "",
                "txtICD10Description6": "",
                "txtICD9": "",
                "hfICD9": "",
                "hfICD109": "",
                "hfICDDescription9": "",
                "hfSNOMED9": "",
                "hfSNOMEDDescription9": "",
                "txtICD10Description9": "",
                "txtICD12": "",
                "hfICD12": "",
                "hfICD1012": "",
                "hfICDDescription12": "",
                "hfSNOMED12": "",
                "hfSNOMEDDescription12": "",
                "txtICD10Description12": "",
                "VisitCharge_SelectedDataKeys": "",
                "dtpDOSFrom": "",
                "dtpDOSTo": "",
                "txtCPT": "",
                "hfCPT": "",
                "hfCPTDescription": "",
                "txtModifier1": "",
                "txtModifier2": "",
                "txtModifier3": "",
                "txtModifier4": "",
                "txtICDPointer1": "",
                "txtICDPointer2": "",
                "txtICDPointer3": "",
                "txtICDPointer4": "",
                "txtPOS": "",
                "hfPOSId": "0",
                "txtTOS": "",
                "hfTOSId": "0",
                "txtPrimaryFEE": "0.00",
                "hfPrimaryFee": "0",
                "txtFEE": "0.00",
                "hfFee": "0.00",
                "hfExpectedFee": "0.00",
                "hfTotalBalance": "0.00",
                "hfAssignBenefits": "",
                "hfCurrentChargeCPT": "",
                "txtUnits": "1",
                "hfUnits": "1",
                "txtTotalFEE": "0.00",
                "txtINSCharges": "0.00",
                "txtPATCharges": "0.00",
                "txtCOPAY": "0.00",
                "txtPriorAuthorization": "",
                "hfAuthorizationId": "",
                "txtExpectedFee": "0.00",
                "txtNDC": "",
                "txtNDCUnit": "",
                "txtNDCUnitPrice": "0",
                "txtComments": "",
                "txtCLIA": "",
                "dtpUnableToWorkFrom": "",
                "dtpUnableToWorkTo": "",
                "dtpLMPDate": "",
                "txtOrderingProvider": "",
                "dtpCurrentIllnessDate": "",
                "txtICN_DCN": "",
                "dtpAdmissionDate": "",
                "dtpDischargeDate": "",
                "dtpLastSeenDate": "",
                "txtComments": "",
                "dtpInjuryDate": "",
                "txtState": "",
                "txtControlNumber": "",
                "dtpDocumentSentDate": "",
                "hfPatientId": "",
                "hfProvider": "",
                "hfPractice": "",
                "hfRefProvider": "",
                "hfOrderingProvider": "",
                "hfSupervisingProvider": "",
                "hfResourceProvider": "",
                "hfFacility": "",
                "hfInsurancePlan": "",
                "hfCaseId": "",
                "hfPOS": "",
                "hfIsSubmitted": "",
                "hfBatchId": "",
                "hfChargeRowId": "",
                "hfPOSId": "",
                "hfVisitDate": "",
                "hfMasterVisitId": "",
                "hfPatientReferralId": "",
                "hfReferralNumerId": "",
                "hfAuthorizeId": "",
                "hfPVICDIdsLength": "",
                "hfAnesthesiologist": "",
                "hfCRNA": "",
                "hfIsAnesthesiaBilling": "false",
                "chkBillToPatientForIns": false,
                "chkIsCleanclaim": false,
                "radAnesthesiologist": false,
                "radCRNA": true,
                "radPCP": true,
                "radSpecialist": false,
                "chkPaid": false,
                "chkAllCharges": false,
                "chkActive": true,
                "chkEMG": false,
                "chkPrintonHCFAF19": false,
                "chkAssgBenefits": false,
                "chkIsReportNPI": false,
                "RadEmploymentYes": false,
                "RadEmploymentNo": true,
                "RadAutoYes": false,
                "RadAutoNo": true,
                "RadOtherYes": false,
                "RadOtherNo": true,
                "chkBillToPatient": false,
                "ddlSubmitStatus": "1",
                "ddlSubmitStatus_text": "Pending",
                "ddlBillingProvider": "",
                "ddlBillingProvider_text": "- Select -",
                "ddlClaimType": "1",
                "ddlClaimType_text": "Professional Medical",
                "ddlLastVisits": "",
                "ddlLastVisits_text": "- Select -",
                "ddlAnesthesiaType": "",
                "ddlAnesthesiaType_text": "- Select -",
                "ddlASA": "",
                "ddlASA_text": "- Select -",
                "ddlServiceType": "",
                "ddlServiceType_text": "- Select -",
                "ddlNDCMeasurement": "",
                "ddlNDCMeasurement_text": "- Select -",
                "ddlDelayReason": "",
                "ddlDelayReason_text": "- Select -",
                "ddlClaimFrequency": "",
                "ddlClaimFrequency_text": "- Select -",
                "ddlReportTypeCode": "",
                "ddlReportTypeCode_text": "- Select -",
                "ddlTransmissionCode": "",
                "ddlTransmissionCode_text": "- Select -"
            }

            var objdef = $.Deferred();

            if (BillingInformation.VisitInformation != null) {
                JSONObject["txtVisitCopayment"] = BillingInformation.VisitInformation.txtVisitCopayment;
                JSONObject["hfInsurancePlan"] = BillingInformation.VisitInformation.hfInsurancePlan;
                JSONObject["chkAssgBenefits"] = BillingInformation.VisitInformation.chkAssgBenefits;
                JSONObject["chkIsReportNPI"] = BillingInformation.VisitInformation.chkIsReportNPI;
                objdef.resolve(true);
            }
            else {
                BillingInformation.FillVisit(BillingInformation.params.PatientId, BillingInformation.params.VisitId).done(function (response) {

                    var visitDetails = JSON.parse(response.VisitFill_JSON);
                    BillingInformation.VisitInformation = visitDetails;
                    BillingInformation.params.AppointmentId = visitDetails.AppointmentId;
                    JSONObject["hfInsurancePlan"] = visitDetails.ddlPatientInsurance;
                    JSONObject["chkAssgBenefits"] = visitDetails.chkAssgBenefits;
                    JSONObject["chkIsReportNPI"] = visitDetails.chkIsReportNPI;
                    objdef.resolve(true);
                });
            }

            objdef.done(function (res) {

                if (res == true) {

                    if (JSONObject["hfInsurancePlan"] != "")
                        JSONObject["chkBillToPatient"] = false;
                    else
                        JSONObject["chkBillToPatient"] = true;

                    JSONObject["hfPatientId"] = BillingInformation.params.PatientId != null ? BillingInformation.params.PatientId.toString() : null;
                    JSONObject["hfProvider"] = BillingInformation.params.ProviderId != null ? BillingInformation.params.ProviderId.toString() : null;
                    if (globalAppdata['DefaultPracticeId'] != "")
                        JSONObject["hfPractice"] = globalAppdata['DefaultPracticeId'];

                    if (BillingInformation.params["AppointmentDate"] != "No Appointment Selected" && BillingInformation.params["AppointmentDate"] != null && BillingInformation.params["AppointmentDate"] != '') {
                        if (typeof Date(BillingInformation.params["AppointmentDate"]) == "string" && BillingInformation.params["AppointmentDate"].length > 9) {

                            var d = BillingInformation.params["AppointmentDate"].split(' ');
                            BillingInformation.params["AppointmentDate"] = d[0];
                            //BillingInformation.params["AppointmentDate"] = new Date(Date(BillingInformation.params["AppointmentDate"])).toISOString().slice(0, 10).replace(/-/g, "/");

                            //if (/*@cc_on!@*/false || !!document.documentMode)
                            //    BillingInformation.params["AppointmentDate"] = new Date(Date(BillingInformation.params["AppointmentDate"])).toISOString().slice(0, 10).replace(/-/g, "/");
                            //else
                            //    BillingInformation.params["AppointmentDate"] = utility.formatDate(new Date(Date(BillingInformation.params["AppointmentDate"])));
                        }
                        JSONObject["dtpAppointmentDate"] = BillingInformation.params["AppointmentDate"];
                    }
                    else {
                        JSONObject["dtpAppointmentDate"] = '';
                    }
                    if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits")
                        JSONObject["dtpAppointmentDate"] = "";
                    JSONObject["hfOrderingProvider"] = BillingInformation.params.ProviderId != null ? BillingInformation.params.ProviderId.toString() : null;
                    JSONObject["hfSupervisingProvider"] = BillingInformation.params.ProviderId != null ? BillingInformation.params.ProviderId.toString() : null;
                    if (ResourceProvider == null || ResourceProvider == "" || ResourceProvider == undefined) {
                        JSONObject["hfResourceProvider"] = BillingInformation.params.ProviderId != null ? BillingInformation.params.ProviderId.toString() : null;
                    } else {
                        JSONObject["hfResourceProvider"] = ResourceProvider;
                    }

                    if (BillingInformation.params.FacilityId) {
                        JSONObject["hfFacility"] = BillingInformation.params.FacilityId;
                    } else {
                        JSONObject["hfFacility"] = BillingInformation.PatientInfoJSON.FacilityID != null ? BillingInformation.PatientInfoJSON.FacilityID.toString() : null;
                    }


                    //JSONObject["hfInsurancePlan"] = "";
                    JSONObject["hfCaseId"] = "";
                    JSONObject["hfPOS"] = "";
                    JSONObject["hfIsSubmitted"] = "";
                    JSONObject["hfBatchId"] = "";

                    JSONObject["hfPOSId"] = "";
                    JSONObject["hfVisitDate"] = BillingInformation.params.VisitDate != null ? BillingInformation.params.VisitDate.toString() : null;
                    JSONObject["hfMasterVisitId"] = BillingInformation.params.VisitId != null ? BillingInformation.params.VisitId.toString() : null;
                    JSONObject["hfPatientReferralId"] = "";
                    JSONObject["hfReferralNumerId"] = "";
                    JSONObject["hfAuthorizeId"] = "";
                    JSONObject["hfPVICDIdsLength"] = "";
                    JSONObject["hfAnesthesiologist"] = "";
                    JSONObject["hfCRNA"] = "";

                    obj["NotesId"] = BillingInformation.params.NotesId != null ? BillingInformation.params.NotesId.toString() : null;
                    obj["NoteDate"] = BillingInformation.params.NoteDate != null ? BillingInformation.params.NoteDate.toString() : null;
                    obj["PatientId"] = BillingInformation.params.PatientId != null ? BillingInformation.params.PatientId.toString() : null;
                    obj["VisitId"] = BillingInformation.params.VisitId != null ? BillingInformation.params.VisitId.toString() : null;
                    obj["BillingInfoId"] = BillingInformation.params.BillingInfoId != null ? BillingInformation.params.BillingInfoId.toString() : null;

                    obj["ProviderId"] = BillingInformation.params.ProviderId != null ? BillingInformation.params.ProviderId.toString() : null;

                    JSONObject["hfPatientId"] = BillingInformation.params.PatientId != null ? BillingInformation.params.PatientId.toString() : null;
                    JSONObject["hfProvider"] = BillingInformation.params.ProviderId != null ? BillingInformation.params.ProviderId.toString() : null;
                    if (ResourceProvider == null || ResourceProvider == "" || ResourceProvider == undefined) {
                        JSONObject["hfResourceProvider"] = BillingInformation.params.ProviderId != null ? BillingInformation.params.ProviderId.toString() : null;
                    } else {
                        JSONObject["hfResourceProvider"] = ResourceProvider;
                    }

                    if (globalAppdata['DefaultPracticeId'] != "")
                        JSONObject["hfPractice"] = globalAppdata['DefaultPracticeId'];
                    if (BillingInformation.params.RefProviderId) {
                        JSONObject["hfRefProvider"] = BillingInformation.params.RefProviderId;
                    } else {
                        JSONObject["hfRefProvider"] = BillingInformation.PatientInfoJSON.RefProviderID != null ? BillingInformation.PatientInfoJSON.RefProviderID.toString() : null;
                    }
                    var haveValue = 0
                    for (var index = 1; index <= 12 ; index++) {

                        JSONObject["txtICD" + index] = obj["Disease_" + index];
                        JSONObject["hfICD" + index] = obj["ICDCode9_" + index];
                        JSONObject["hfICD10" + index] = obj["ICDCode10_" + index];
                        if ((obj["ICDCode9_" + index] != null && obj["ICDCode9_" + index] != "") || (obj["ICDCode10_" + index] != null && obj["ICDCode10_" + index] != "")) {
                            haveValue = haveValue + 1;
                        }
                        JSONObject["hfICDDescription" + index] = obj["ICDDescription9_" + index];
                        JSONObject["hfSNOMED" + index] = obj["hfSNOMEDCode_" + index];
                        JSONObject["hfSNOMEDDescription" + index] = obj["SNOMEDDescription_" + index];
                        JSONObject["txtICD10Description" + index] = obj["ICDDescription10_" + index];
                    }

                    if (isFromNotes == true) {
                        haveValue = 0
                        for (var index = 1; index <= 12 ; index++) {
                            arrayIndex = index - 1;
                            if (obj.ICDs.length >= index) {
                                JSONObject["txtICD" + index] = obj.ICDs[arrayIndex].ICDCode9 + obj.ICDs[arrayIndex].ICDCode10;
                                JSONObject["hfICD" + index] = obj.ICDs[arrayIndex].ICDCode9;
                                JSONObject["hfICD10" + index] = obj.ICDs[arrayIndex].ICDCode10;
                                if ((obj.ICDs[arrayIndex].ICDCode9 != null && obj.ICDs[arrayIndex].ICDCode9 != "") || (obj.ICDs[arrayIndex].ICDCode10 != null && obj.ICDs[arrayIndex].ICDCode10 != "")) {
                                    haveValue = haveValue + 1;
                                }
                                JSONObject["hfICDDescription" + index] = obj.ICDs[arrayIndex].ICDDescription9;
                                JSONObject["hfSNOMED" + index] = obj.ICDs[arrayIndex].SNOMEDCode;
                                JSONObject["hfSNOMEDDescription" + index] = obj.ICDs[arrayIndex].SNOMEDDescription;
                                JSONObject["txtICD10Description" + index] = obj.ICDs[arrayIndex].ICDDescription10;
                            }
                        }
                    }
                    JSONObject["hfPVICDIdsLength"] = haveValue;
                    JSONObject["hfChargeRowId"] = "";

                    var today = new Date();
                    var dd = today.getDate();
                    var mm = today.getMonth() + 1; //January is 0!
                    var yyyy = today.getFullYear();
                    if (dd < 10) {
                        dd = '0' + dd
                    }
                    if (mm < 10) {
                        mm = '0' + mm
                    }
                    today = mm + '/' + dd + '/' + yyyy;
                    BillingInformation.AttachtedCPTData = BillingInformation.removeDupRcdsByCPTCode(BillingInformation.AttachtedCPTData);
                    BillingInformation.AddtoJsonObjectInCPTObject(obj.CPTs);
                    for (index = 0; index < obj.CPTs.length; index++) {

                        if ((obj.CPTs[index]["CPTCode"] == null || obj.CPTs[index]["CPTCode"] == '') && (obj.CPTs[index]["CPTDescription"] == null || obj.CPTs[index]["CPTDescription"] == '' || obj.CPTs[index]["CPTDescription"] == 'undefined'))
                            continue;
                        var currentNegIndex = (parseInt(index) + 1) * (-1);
                        JSONObject["hfChargeRowId"] = JSONObject["hfChargeRowId"] + currentNegIndex + ",";
                        JSONObject["txtCPT" + currentNegIndex] = obj.CPTs[index]["CPTCode"];
                        JSONObject["hfCPT" + currentNegIndex] = obj.CPTs[index]["CPTCode"];
                        var tempCPTDescription = obj.CPTs[index]["CPTDescription"].replace(/"/g, "'");
                        if (tempCPTDescription.length > 50) {
                            tempCPTDescription = tempCPTDescription.substring(0, 49);
                        }
                        JSONObject["hfCPTDescription" + currentNegIndex] = tempCPTDescription;
                        JSONObject["txtModifier1" + currentNegIndex] = obj.CPTs[index]["Modifier1"];
                        JSONObject["txtModifier2" + currentNegIndex] = obj.CPTs[index]["Modifier2"];
                        JSONObject["txtModifier3" + currentNegIndex] = obj.CPTs[index]["Modifier3"];
                        JSONObject["txtModifier4" + currentNegIndex] = obj.CPTs[index]["Modifier4"];
                        JSONObject["txtICDPointer1" + currentNegIndex] = obj.CPTs[index]["DxPointer1"];
                        JSONObject["txtICDPointer2" + currentNegIndex] = obj.CPTs[index]["DxPointer2"];
                        JSONObject["txtICDPointer3" + currentNegIndex] = obj.CPTs[index]["DxPointer3"];
                        JSONObject["txtICDPointer4" + currentNegIndex] = obj.CPTs[index]["DxPointer4"];
                        JSONObject["hfUnits" + currentNegIndex] = obj.CPTs[index]["UnitsId"];
                        JSONObject["hfTotalBalance" + currentNegIndex] = "0.00";
                        JSONObject["hfAssignBenefits" + currentNegIndex] = "";
                        JSONObject["hfCurrentChargeCPT" + currentNegIndex] = obj.CPTs[index]["CPTCode"];
                        JSONObject["txtUnits" + currentNegIndex] = obj.CPTs[index]["UnitsId"];
                        JSONObject["txtTotalFEE" + currentNegIndex] = obj.CPTs[index]["UnitsId"] * obj.CPTs[index]["txtFEE"];
                        JSONObject["txtFEE" + currentNegIndex] = obj.CPTs[index]["txtFEE"];
                        JSONObject["hfFee" + currentNegIndex] = obj.CPTs[index]["txtFEE"];
                        JSONObject["hfExpectedFee" + currentNegIndex] = obj.CPTs[index]["hfExpectedFee"] ? obj.CPTs[index]["hfExpectedFee"] : "0.00";
                        JSONObject["txtExpectedFee" + currentNegIndex] = (JSONObject["hfExpectedFee" + currentNegIndex] * JSONObject["hfUnits" + currentNegIndex]).toFixed(2);

                        if (index == 0) {
                            JSONObject["txtCOPAY" + currentNegIndex] = obj.CPTs[index]["Copay"];
                        }
                        else {
                            JSONObject["txtCOPAY" + currentNegIndex] = "0.00";
                        }
                        JSONObject["txtPriorAuthorization" + currentNegIndex] = "";
                        JSONObject["hfAuthorizationId" + currentNegIndex] = "";
                        JSONObject["txtNDC" + currentNegIndex] = "";
                        JSONObject["txtNDCUnit" + currentNegIndex] = "";
                        JSONObject["txtNDCUnitPrice" + currentNegIndex] = "0";
                        JSONObject["txtComments" + currentNegIndex] = "";
                        JSONObject["txtCLIA" + currentNegIndex] = "";
                        JSONObject["dtpDOSFrom" + currentNegIndex] = "";
                        JSONObject["txtPOS" + currentNegIndex] = obj.CPTs[index]["POS"];
                        JSONObject["txtTOS" + currentNegIndex] = "";
                        JSONObject["ddlNDCMeasurement" + currentNegIndex] = "";
                        JSONObject["chkActive" + currentNegIndex] = "True";
                        JSONObject["chkEMG" + currentNegIndex] = "";

                        if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                            JSONObject["dtpDOSTo" + currentNegIndex] = obj.CPTs[index]["DOSTo"];
                            JSONObject["dtpDOSFrom" + currentNegIndex] = obj.CPTs[index]["DOSFrom"];
                        }
                        else {
                            if (BillingInformation.params["AppointmentDate"] && BillingInformation.params["AppointmentDate"] != "") {
                                if (typeof Date(BillingInformation.params["AppointmentDate"]) == "string" && BillingInformation.params["AppointmentDate"].length < 10) {
                                    JSONObject["dtpDOSTo" + currentNegIndex] = BillingInformation.params["AppointmentDate"];
                                    JSONObject["dtpDOSFrom" + currentNegIndex] = BillingInformation.params["AppointmentDate"];
                                }
                                else {
                                    JSONObject["dtpDOSTo" + currentNegIndex] = today;
                                    JSONObject["dtpDOSFrom" + currentNegIndex] = today;
                                }
                            } else {
                                JSONObject["dtpDOSTo" + currentNegIndex] = today;
                                JSONObject["dtpDOSFrom" + currentNegIndex] = today;
                            }
                        }

                        if (BillingInformation.VisitInformation != null) {
                            if (BillingInformation.VisitInformation.hfInsurancePlan) {
                                JSONObject["txtINSCharges" + currentNegIndex] = JSONObject["txtTotalFEE" + currentNegIndex] + "";
                                JSONObject["txtPATCharges" + currentNegIndex] = "";
                            }
                            else {
                                JSONObject["txtINSCharges" + currentNegIndex] = "";
                                JSONObject["txtPATCharges" + currentNegIndex] = JSONObject["txtTotalFEE" + currentNegIndex] + "";
                            }
                        }
                    }

                    BillingInformation.VisitUpdate(JSONObject);
                }
            });

        } catch (ex) {
            utility.DisplayMessages(ex.message, 3);
            console.log(ex);
        }

    },

    VisitUpdate: function (JSONObject) {
        //   JSONObject["dtpAppointmentDate"] = BillingInformation.VisitInformation.dtpAppointmentDate;
        JSONObject["dtpClaimDate"] = BillingInformation.VisitInformation.dtpClaimDate;
        JSONObject["txtClaimNumber"] = BillingInformation.VisitInformation.txtClaimNumber;

        //Resource provider, provider and facility set for out of office visit
        if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
            var self = $('#' + BillingInformation.params.PanelID);
            JSONObject["hfFacility"] = self.find('#hfFacility').val();
            JSONObject["hfResourceProvider"] = self.find('#hfResourceProvider').val();
            JSONObject["hfProvider"] = self.find('#hfProvider').val();
            JSONObject["hfPOS"] = self.find('#txtPOS').val();
        }
        var selectedInsurancePlan = JSONObject["hfInsurancePlan"];
        if (BillingInformation.params.ParentCtrl == "Clinical_NotesSearch" || BillingInformation.params.ParentCtrl == "clinicalTabProgressNote") {
            selectedInsurancePlan = "";
            JSONObject.hfFacility = "";
            JSONObject.hfProvider = "";
            JSONObject["txtClaimNumber"] = "";
        }
        var myJSON = encodeURIComponent(JSON.stringify(JSONObject));
        BillingInformation.UpdateVisit(myJSON, BillingInformation.params.VisitId, BillingInformation.params.PatientId, selectedInsurancePlan).done(function (response) {
            if (response.status != false) {


                BillingInformation.BillingInfoSIGN("Signed").done(function (response) {
                    if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                        OutOfOfficeVisits.loadOOOVisit();
                    }
                });
                if (BillingInformation.params.PanelID == null) {
                    BillingInformation.params.PanelID = "pnlBillingInformation";
                }

                $("#" + BillingInformation.params.PanelID + " #btnViewCharges").removeClass("hidden");
                $("#" + BillingInformation.params.PanelID + " #divOutOfOfficeVisit,#divEvalnMangement,#divICDCPT").addClass("disableAll");
                $("#" + BillingInformation.params.PanelID + " #btnViewCharges").attr("onclick", "BillingInformation.LoadVisitDetail('" + BillingInformation.params.VisitId + "','" + BillingInformation.params.PatientId + "',event)");
                $("#" + BillingInformation.params.PanelID + " #btnSign").addClass("hidden");
                $("#" + BillingInformation.params.PanelID + " #btnSave").addClass("hidden");

                $("#" + BillingInformation.params.PanelID + " #btnViewChargesBottom").removeClass("hidden");
                $("#" + BillingInformation.params.PanelID + " #btnViewChargesBottom").attr("onclick", "BillingInformation.LoadVisitDetail('" + BillingInformation.params.VisitId + "','" + BillingInformation.params.PatientId + "',event)");
                $("#" + BillingInformation.params.PanelID + " #btnSignBottom").addClass("hidden");
                $("#" + BillingInformation.params.PanelID + " #btnSaveBottom").addClass("hidden");



                BillingInformation.Status = 'Signed';
                utility.DisplayMessages("Claim created successfully.", 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadVisitDetail: function (VisitId, PatientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if (BillingInformation.params.ParentCtrl == "Clinical_NotesView") {
                //    UnloadActionPan(BillingInformation.params["ParentCtrl"], "BillingInformation");
                //    if (Clinical_NotesView.params != null && Clinical_NotesView.params.ParentCtrl) {
                //        UnloadActionPan(Clinical_NotesView.params.ParentCtrl, "Clinical_NotesView");
                //    }
                //    else {
                //        UnloadActionPan(null, "Clinical_NotesView");
                //    }
                //} else {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'BillingInformation';
                params["Parent"] = BillingInformation.params.ParentCtrl;

                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;

                LoadActionPan('EncounterChargeCapture', params, BillingInformation.params.PanelID);
                //}
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    //get visit detail
    FillVisit: function (PatientID, VisitId) {
        var data = "VisitID=" + VisitId + "&PatientID=" + PatientID;
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "FILL_VISIT");
    },

    ////--------progress note functions-------///////

    AddProblemOnDrFirst_DB_Call: function (ProblemListID) {
        var objData = {};
        objData.ProblemListId = ProblemListID;
        objData["commandType"] = "SAVE_PROBLEMLIST_ON_DRFIRST";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    getBillingInfo: function (billingInfoId) {
        BillingInformation.fillBillingInfo(billingInfoId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    BillingInformation.createBillingInfoBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    fillBillingInfo: function (billingInfoId) {
        var objData = new Object();
        objData["BillingInfoId"] = BillingInformation.params.BillingInfoId;
        objData["commandType"] = "BILLING_INFORMATION_SELECT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },

    //This Function will check, if Medical History Soap is already attached in Progress note, if Medical History is not attached than it will create main divs to attach allergy
    checkBillingInfoExists: function () {

        //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_billinginfo').parent().parent().length > 0) {
        //    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_billinginfo').parent().parent().remove();
        //}
        //else if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_billinginfo').length > 0) {
        //    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_billinginfo').remove();
        //}
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_billinginfo').length == 0) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList').append(' <li class="BillingInfoComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_billinginfo title="eSuperbill"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'BillingInformation\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="eSuperbill">eSuperbill</a> ' +
                                  '</clinical_billinginfo> </header></li>');
            Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                //  $(this).find('.closeBtn').removeClass('hidden');
                // $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                //    $(this).find('.closeBtn').addClass('hidden');
                //$(this).css('background', '#fff');
            });
        }
    },

    //This Function is used to create SOAP html and append it to  Progress note
    createBillingInfoBodyHTML: function (response, NoteHTMLCtrl, UnloadMedicalhx) {
        BillingInformation.checkBillingInfoExists();
        if (response.BillingInfoFill_JSON != null && response.BillingInfoFill_JSON != '') {
            var MedicalHxFill_Obj = JSON.parse(response.BillingInfoFill_JSON);
            var $mainDivMedicalHx = $(document.createElement('div'));

            var MedicalHxId = MedicalHxFill_Obj.BillingInfoId;
            if (MedicalHxId > 0) {
                var $SectionBodyMedicalHx = $(document.createElement('section'));
                $SectionBodyMedicalHx.attr('id', "Cli_BillingInfo_Main" + MedicalHxId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_BillingInfo_" + MedicalHxId);
                var $ListMedicalHx = $(document.createElement('ul'));

                $ListMedicalHx.attr('class', 'list-unstyled')

                //  $SectionBodyMedicalHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_BillingInfo_" + MedicalHxId + '"><i class="fa fa-edit"></i></a>' +
                //       '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_BillingInfo_Main" + MedicalHxId + '"  ><i class="fa fa-times"></i></a></div> ');                

                $ListMedicalHx.append("<li>" + MedicalHxFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListMedicalHx);
                if (Clinical_Notes.params.mode == null)
                    Clinical_Notes.params.mode = "Edit";
                if (Clinical_ProgressNote.params.NotesId == null)
                    Clinical_ProgressNote.params.NotesId = BillingInformation.params.NotesId;
                $SectionBodyMedicalHx.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_Billinginfo').parent().parent().find('#Cli_BillingInfo_Main' + MedicalHxId).length == 0) {
                    $mainDivMedicalHx.append($SectionBodyMedicalHx);
                    BillingInformation.updateBillingInfoHtml($mainDivMedicalHx.html(), MedicalHxId, NoteHTMLCtrl);
                    Clinical_ProgressNote.saveComponentSOAPText('Billing Info', BillingInformation.HideAlert);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_Billinginfo').parent().parent().find('#Cli_BillingInfo_Main' + MedicalHxId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_Billinginfo').parent().parent().find('#Cli_BillingInfo_Main' + MedicalHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_Billinginfo').parent().parent().find('#Cli_BillingInfo_Main' + MedicalHxId).html($SectionBodyMedicalHx.html());
                    $(NoteHTMLCtrl + ' clinical_Billinginfo').parent().parent().find('#Cli_BillingInfo_Main' + MedicalHxId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText('Billing Info', BillingInformation.HideAlert);
                    BillingInformation.updateBillingInfoHtml("", MedicalHxId, NoteHTMLCtrl);

                }

                //if (UnloadMedicalhx == true) {
                //    Clinical_MedicalHx.UnloadMedicalHistory(Clinical_MedicalHx.bNextPrev);
                //}
            }
        }
    },



    // This Function is called by Progress Notes (Fill MedicalHx Func, CopyAllNotesCategories)
    updateBillingInfoHtml: function (MedicalHxHtml, MedicalHxId, NoteHTMLCtrl) {
        $(NoteHTMLCtrl + ' clinical_Billinginfo').parent().parent().addClass('initialVisitBody');
        if (MedicalHxHtml != '') {
            $(NoteHTMLCtrl + ' clinical_Billinginfo').closest('li').find('section').remove();
            $(NoteHTMLCtrl + ' clinical_Billinginfo').parent().parent().append(MedicalHxHtml);

        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
    },

    //Start//Abid Ali//For OOOVisit

    FillChargePOS: function (facilityHiddenCtrl, POSHiddenCtrl, currRow, mode) {
        var deffered = $.Deferred();
        var facilityId = $('#' + BillingInformation.params["PanelID"] + ' #' + facilityHiddenCtrl).val();
        if (facilityId == "" && BillingInformation.params["FacilityId"]) {
            facilityId = BillingInformation.params["FacilityId"];
        }
        if (facilityId > 0) {
            Admin_Facility.SearchFacility(null, facilityId).done(function (response) {
                if (response.status != false) {

                    if (response.FacilityCount > 0) {
                        var FacilityLoadJSONData = JSON.parse(response.FacilityLoad_JSON)[0];

                        if (POSHiddenCtrl) {

                            $('#' + BillingInformation.params["PanelID"] + ' #' + POSHiddenCtrl).val(FacilityLoadJSONData.POSName);

                            $('#' + BillingInformation.params["PanelID"] + ' #txtPOS').val(FacilityLoadJSONData.POSName);
                            deffered.resolve();
                            //if (mode != "edit")
                            //BillingInformation.fillCodeInTable(FacilityLoadJSONData.POSName, "POS", true, currRow);
                        } else {
                            deffered.resolve();
                            //BillingInformation.fillCodeInTable(FacilityLoadJSONData.POSName, "POS", true, currRow);
                        }
                    }
                    else {
                        deffered.resolve();
                    }
                }
                else {
                    deffered.resolve();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            deffered.resolve();
        }
        return deffered;
    },
    GetEMCodeSuggestions: function (type) {

        var objData = new Object();
        objData["PatientID"] = BillingInformation.params.PatientId;
        objData["CommandType"] = "get_EMCodeSuggestion";
        objData["ENMTypeId"] = BillingInformation.SelectedPatientType == "" || null ? type : BillingInformation.SelectedPatientType;
        objData["NotesId"] = BillingInformation.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIServiceSync(data, "BillingInformation", "BillingInformation");
    },
    OpenSuggestEMCodes: function (type) {
        BillingInformation.GetEMCodeSuggestions(type).done(
                          function (response) {
                              response = JSON.parse(response);

                              if (response.status == true) {
                                  BillingInformation.CodeSuggestions = response;
                                  var params = [];
                                  params["NotesId"] = BillingInformation.params.NotesId;
                                  params["ParentCtrl"] = "BillingInformation";
                                  params["Suggestions"] = BillingInformation.CodeSuggestions;
                                  LoadActionPan("ENMCodeSuggest", params);
                              }
                              else {
                                  utility.DisplayMessages(response.message, 2);
                              }
                          });

    },
    OpenFacility: function () {
        var params = [];
        //params["IsOptional"] = false;
        // params["RefForm"] = "frmChargeSearchDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "BillingInformation";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {

        var params = [];
        params["FacilityId"] = $("#" + BillingInformation.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'BillingInformation';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },

    OpenProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];

        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "BillingInformation";

        params["RefForm"] = 'frmBillingInformation';
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;

        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function (HiddenCtrl, TxtBoxCtrl) {

        var params = [];
        params["ProviderId"] = $('#' + BillingInformation.params["PanelID"] + ' #' + HiddenCtrl).val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = TxtBoxCtrl;
        params["ParentCtrl"] = 'BillingInformation';
        LoadActionPan('providerDetail', params);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = $('#' + BillingInformation.params["PanelID"] + ' #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["ParentCtrl"] = "BillingInformation";
        LoadActionPan('Admin_ReferringProvider', params);
    },
    OpenRefProviderDetail: function () {
        var params = [];
        params["ReferringProviderId"] = $('#' + BillingInformation.params.PanelID + ' #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = 'BillingInformation';
        LoadActionPan('referringproviderDetail', params);
    },
    BindRefProvider: function () {
        var Ctrl = $('#pnlBillingInformation #txtRefProvider');
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnlBillingInformation #hfRefProvider");
        var onSelect = function (e) { BillingInformation.params.RefProviderId = e.id; };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },
    BindFacility: function () {
        var Ctrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #txtFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var hfCtrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #hfFacility");
        var onChange = function (e) { BillingInformation.FillChargePOS('hfFacility', 'hfPOS') };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, null, onChange);
    },

    BindProvider: function (Id, linkId, lblId, hfId) {

        if (!Id)
            Id = 'txtProvider';
        if (!linkId)
            linkId = 'lnkProviderEdit';
        if (!lblId)
            lblId = 'lblProvider';
        if (!hfId)
            hfId = 'hfProvider';

        var Ctrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #" + Id);
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + BillingInformation.params.PanelID + " #frmBillingInformation #" + hfId);
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindResourceProvider: function () {

        BillingInformation.BindProvider('txtResourceProvider', 'lnkResourceProviderEdit', 'lblResourceProvider', 'hfResourceProvider');
    },
    //End//Abid Ali//For OOOVisit

    RemoveMultipleRows: function () {
        var checkBoxCount = BillingInformation.IfAnyCheckBOxChecked();
        var messageShown = true;
        if ($('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr").length > 0 && checkBoxCount > 0) {
            var obj = EditableGrid;
            var responceMescheck = true;
            AppPrivileges.GetFormPrivileges("Provider", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                utility.myConfirm('50', function () {

                    $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr").each(function () {
                        var $row = $(this);
                        if (($row.find("input:checkbox")[0].checked)) {

                            var id = $row.attr("id");
                            var Type = $row.attr("Type");
                            var CheckBoxValue = $row.attr("CheckBoxValue");

                            if (strMessage == "") {

                                var selectedValue = id;
                                if (selectedValue == "" || selectedValue == "undefined") {
                                }
                                else {
                                    var CopyEMCheckedCheckBoxArray = [];
                                    var CPTCode = $("#" + BillingInformation.params.PanelID + " #dgvBillVisitCharge tr#" + selectedValue).find("input[id*='hfCPTCode']").val()

                                    if ((BillingInformation.params.BillingInfoId && parseInt(BillingInformation.params.BillingInfoId) > 0) && CPTCode != '' && CPTCode != null) {
                                        BillingInformation.DeleteCPT(CPTCode, BillingInformation.params.BillingInfoId).done(function (response) {
                                            response = JSON.parse(response);
                                            if (response.status != false) {

                                                var ObjTime = $.grep(BillingInformation.BillingInfoTime, function (a) {
                                                    return (a.ENMCPT == CPTCode);
                                                });

                                                if (ObjTime != null && ObjTime.length > 0)
                                                    $("#" + BillingInformation.params.PanelID + " #tblEMCodes [type='checkbox']").prop("checked", false);

                                                if (Type != null && (typeof Type != typeof undefined) && Type != "" && CheckBoxValue != null && (typeof CheckBoxValue != typeof undefined) && CheckBoxValue != "") {

                                                    var Found = false;
                                                    $.grep(BillingInformation.EMCheckedCheckBoxArray, function (item, index) {
                                                        if (item.Type == Type && item.CheckBoxVal == CheckBoxValue) {
                                                            Found = true;
                                                        }
                                                        else {
                                                            CopyEMCheckedCheckBoxArray.push(item);
                                                        }
                                                    });
                                                    BillingInformation.EMCheckedCheckBoxArray = CopyEMCheckedCheckBoxArray;
                                                }
                                                if ($row.hasClass('adding')) {
                                                }
                                                var _self = obj;
                                                _self.datatable.row($row.get(0)).remove().draw();

                                                BillingInformation.ReorderCPTs();
                                                BillingInformation.EmptyRowsGoDown();
                                                if (checkBoxCount <= 2 && messageShown) {
                                                    utility.DisplayMessages(response.message, 1);
                                                    messageShown = false;
                                                } checkBoxCount--;

                                            }
                                            else {
                                                utility.DisplayMessages(response.Message, 3);
                                            }
                                        });
                                    }
                                    else {
                                        var ObjTime = $.grep(BillingInformation.BillingInfoTime, function (a) {
                                            return (a.ENMCPT == CPTCode);
                                        });

                                        if (ObjTime != null && ObjTime.length > 0)
                                            $("#" + BillingInformation.params.PanelID + " #tblEMCodes [type='checkbox']").prop("checked", false);

                                        if (Type != null && (typeof Type != typeof undefined) && Type != "" && CheckBoxValue != null && (typeof CheckBoxValue != typeof undefined) && CheckBoxValue != "") {

                                            var Found = false;
                                            var re = [];
                                            $.grep(BillingInformation.EMCheckedCheckBoxArray, function (item, index) {
                                                if (item.Type == Type && item.CheckBoxVal == CheckBoxValue) {
                                                    Found = true;
                                                }
                                                else {
                                                    CopyEMCheckedCheckBoxArray.push(item);
                                                }
                                            });
                                            BillingInformation.EMCheckedCheckBoxArray = CopyEMCheckedCheckBoxArray;
                                        }
                                        var _self = obj;
                                        _self.datatable.row($row.get(0)).remove().draw();

                                        BillingInformation.ReorderCPTs();
                                        BillingInformation.EmptyRowsGoDown();
                                    }
                                }
                            }
                            else
                                utility.DisplayMessages(strMessage, 2);
                        }// if row checkbox checked
                    }); //rows loop

                    $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge thead tr:first").find("input:checkbox")[0].checked = false;

                }, function () { }, '50'); //utilityConfirm
            }); //AppPrivileges
        } //rows length check
    },

    checkUncheckselectAllCHkBox: function (chkBox) {

        if ($(chkBox).is(':checked')) {

            $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody").find('input[type="checkbox"]').each(function () {
                this.checked = true;
            });
        }
        else {
            $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody").find('input[type="checkbox"]').each(function () {
                this.checked = false;
            });
        }
    },

    DeleteAllbtnClick: function () {
        // $("input[type='checkbox']").val();
        var x = $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge thead input[type='checkbox']").is(':checked');
        BillingInformation.RemoveMultipleRows();

        //if ($(chkBox).is(':checked')) {

        //    $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody").find('input[type="checkbox"]').each(function () {
        //        this.checked = true;
        //    });
        //}
        //else {
        //    $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody").find('input[type="checkbox"]').each(function () {
        //        this.checked = false;
        //    });
        //}
    },

    onInnercheckBoxChange: function (chkb) {
        //$('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge thead").find('input[type="checkbox"]').each(function () {
        //    this.checked = false;
        //});
        $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge thead tr:first").find("input:checkbox")[0].checked = false;
    },

    IfAnyCheckBOxChecked: function () {
        var retval = 0;
        $('#' + BillingInformation.params.PanelID + " #dgvBillVisitCharge tbody tr").each(function () {

            if ($(this).find("input:checkbox")[0].checked) {
                retval++;
            }

        });
        return retval;

    },
    changeIsNonBilableForProgressNote: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            utility.myConfirm('Are you sure you want to make the Visit and eSuperbill as Non Billable?', function () {
                if (BillingInformation.params.ParentCtrl == "clinicalTabProgressNote") {
                    $("#pnlClinicalProgressNote .IsNonBilablep input[type=checkbox]").prop('checked', true);
                }
                BillingInformation.NotesUpdateForNonBillable(BillingInformation.params.NotesId, $(chkBox).is(':checked'));
            }, function () {
                $(chkBox).prop('checked', false);
                if (BillingInformation.params.ParentCtrl == "clinicalTabProgressNote") {
                    $("#pnlClinicalProgressNote .IsNonBilablep input[type=checkbox]").prop('checked', false);
                }
                BillingInformation.NotesUpdateForNonBillable(BillingInformation.params.NotesId, $(chkBox).is(':checked'));
            }, 'Confirm Non Billable');
        }
        else {
            if (BillingInformation.params.ParentCtrl == "clinicalTabProgressNote") {
                $("#pnlClinicalProgressNote .IsNonBilablep input[type=checkbox]").prop('checked', false);
            }
            BillingInformation.NotesUpdateForNonBillable(BillingInformation.params.NotesId, $(chkBox).is(':checked'));
        }
    },
    NotesUpdateForNonBillable: function (NotesId, IsNonBillable) {

        BillingInformation.NotesUpdateForNonBillable_DBCall(NotesId, IsNonBillable).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    NotesUpdateForNonBillable_DBCall: function (NoteId, IsNonBilable) {
        var objData = new Object();
        objData["NotesId"] = NoteId;
        objData["commandType"] = "update_isnonbillable_notes";
        objData["IsNonBilable"] = IsNonBilable;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "ClinicalNotes");
    },
    SetNonBillable: function () {

        BillingInformation.GetNonBillable_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.IsNonBillable.toLowerCase() == "true") {
                    $("#" + BillingInformation.params.PanelID + " #ChkBox_IsNonBilable").prop('checked', true);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    GetNonBillable_DBCall: function () {
        var objData = new Object();
        objData["NotesId"] = BillingInformation.params.NotesId;
        objData["commandType"] = "get_is_nonbillable_info";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "ClinicalNotes");
    },

}