demographicDetail = {
    params: [],
    bIsFirstLoad: true,
    imagedata: '',
    filetype: '',
    filename: '',
    ScanPrivilige: false,
    OCRPrivilige: false,
    ProviderFName: '',
    ProviderLName: '',
    FacilityAddress: '',
    FacilityCity: '',
    FacilityState: '',
    FacilityZip: '',
    FacilityZipExt: '',
    FacilityPhone: '',
    // scannerjson: '',
    FacilityLabel: '',
    ProviderLabel: '',
    PatientSSN: '',
    multipleEthnicityIds: '',
    checkedEthnicityNodes: [],
    Load: function (params) {

        var ObjDeferred = $.Deferred();
        demographicDetail.params = params;

        //------- Banner Checks (IrFan) ------

        if (demographicDetail.params.PatBanner == true) {

            $("#" + demographicDetail.params["PanelID"] + " #lblHeading").html('Patient');

            if (demographicDetail.params.TabID != "patTabInsurance") {
                $("#" + demographicDetail.params["PanelID"] + " #lnkInsurance").css("display", "inline");
                if (demographicDetail.params.GrandParentCtrl == "appointmentDetail") {
                    $("#" + demographicDetail.params["PanelID"] + " #lnkFacesheet").css("display", "none");
                    $("#" + demographicDetail.params["PanelID"] + " #lnkInsurance").addClass('disableAll');
                    $("#" + demographicDetail.params["PanelID"] + " #lnkDocuments").addClass('disableAll');
                }
                else if (demographicDetail.params.TabID == "schTabCalendar" || demographicDetail.params.TabID == "mstrTabClinical" || demographicDetail.params.TabID.indexOf('clinicalTab') > -1 || demographicDetail.params.FormsTitle != "") {
                    $("#pnldemographicDetail #lnkFacesheet").css("display", "none");
                }
            }
            else if (demographicDetail.params.TabID == "patTabInsurance") {
                $("#" + demographicDetail.params["PanelID"] + " #lnkInsurance").css("display", "none");
                
            }
            if (demographicDetail.params.TabID != "patTabDocuments" && demographicDetail.params.TabID != "Document_Viewer") {
                $("#" + demographicDetail.params["PanelID"] + " #lnkDocuments").css("display", "inline");
            }
            else if (demographicDetail.params.TabID == "patTabDocuments" || demographicDetail.params.TabID == "Document_Viewer") {
                $("#" + demographicDetail.params["PanelID"] + " #lnkDocuments").css("display", "none");
            }
        }
        else {
            $("#" + demographicDetail.params["PanelID"] + " #divPatBannerDemo").css("display", "none");
        }

        //--------------------------------------

        if (demographicDetail.params["PanelID"] != 'pnldemographicDetail')
            demographicDetail.params["PanelID"] = demographicDetail.params["PanelID"] + ' #pnldemographicDetail'
        //else
        //    demographicDetail.params.PanelID = demographicDetail.params["PanelID"];

        if (demographicDetail.params.mode == "Edit")
            $("#" + demographicDetail.params.PanelID + " #div_CCDAAvailable").hide();
        else
            $("#" + demographicDetail.params.PanelID + " #div_CCDAAvailable").show();
        $("#" + demographicDetail.params.PanelID + " #divdemographicDetailPicture #imgPatient").attr("src", "Content/images/default_male_profile.gif");
        if (demographicDetail.bIsFirstLoad) {
            demographicDetail.bIsFirstLoad = false;
            demographicDetail.LoadAllAutocomplete();
            demographicDetail.BindProvider();
            demographicDetail.BindFacility();
            demographicDetail.BindRefProvider();
            demographicDetail.BindPCPProvider();
            demographicDetail.BindGuarantor();
            demographicDetail.BindRefProviderTo();
            demographicDetail.BindRefProviderReferral();
            demographicDetail.BindLanguages();
            demographicDetail.BindCountries();
            Patient_Demographic.BindCity("#" + demographicDetail.params.PanelID + " #frmdemographicDetail");
            demographicDetail.OpenAssignee();

            var self = $('#' + demographicDetail.params["PanelID"]);
            //if (typeof demographicDetail.params["PanelID"] != 'undefined')
            //    self = $('#' + demographicDetail.params["PanelID"] + ' #pnlDemographic');
            //else
            //    self = $('#pnlDemographic');
            //--------- (IrFan)            
            if (self.length == 0) {
                self = $('#pnldemographicDetail');
                demographicDetail.params.PanelID = 'pnldemographicDetail';
            }
            //-------
            $.when(Patient_Demographic.LoadingDropDowns('pnldemographicDetail'), self.loadDropDowns(true)).then(function () {
                //------------------------------------
                utility.InitKendoRaceAutoComplete(demographicDetail.params["PanelID"] + ' #ddlPatientRace', demographicDetail.params["PanelID"] + ' #hfRaceIds');
                demographicDetail.IntializeMultiSelectDropDownEthnicity();

                demographicDetail.multipleEthnicityIds = '';
                demographicDetail.checkedEthnicityNodes = [];
                demographicDetail.ValidateDemographic();

                utility.ValidateDOB('pnldemographicDetail #frmdemographicDetail', 'pnldemographicDetail #frmdemographicDetail #dtpDOB', new Date('1800-01-01'), new Date(), false);
                ObjDeferred.resolve();


            });

        }
        else
            ObjDeferred.resolve();

        $.when(ObjDeferred).then(function () {
            demographicDetail.LoadPatientDemogrphic();
            $('#' +demographicDetail.params.PanelID + ' #frmdemographicDetail').data('serialize', $('#frmdemographicDetail').serialize());
        });



        //demographicDetail.params["mode"] = "Add";
        // alert($.inArray("mode", demographicDetail.params));
        //if (params["mode"] == "" )
        //    params["mode"] = "Add";
        //demographicDetail.params = params;
        //demographicDetail.LoadDemographic();

        if (demographicDetail.params.mode == "Add") {
            $("#" + demographicDetail.params["PanelID"] + " #divdemographicDetailPicture #divdemographicDetailActionButton").css("display", "none");
        }

        else {
            $("#" + demographicDetail.params["PanelID"] + " #divdemographicDetailPicture #divdemographicDetailActionButton").css("display", "inline");
            $("#" + demographicDetail.params["PanelID"] + " #btnScanDemographic").css("display", "none");
        }

        demographicDetail.GetAccountManagerPriviliges();


        //-------------------------- Start 
        if (demographicDetail.params.mode == "Add" && demographicDetail.params.ParentCtrl == "Patient_Search") {
            $("#" + demographicDetail.params["PanelID"] + " #btnDemographic").text("Save");
        } else {
            $("#" + demographicDetail.params["PanelID"] + " #btnDemographic").text("Save & Exit");
        }
        //-------------------------- End
        $('#' + demographicDetail.params.PanelID + " #frmdemographicDetail #divDateOfDeath").addClass('hidden');

        utility.callbackAfterAllDOMLoaded(function () {
            //serialize Data after all controls loaded.
            $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail').data('serialize', $('#frmdemographicDetail').serialize());
            if (demographicDetail.params.mode == "Edit")
            {
                utility.ValidateMU3Demographics("#"+demographicDetail.params.PanelID);
                utility.MU3Demographics("#" + demographicDetail.params.PanelID + " #frmdemographicDetail", demographicDetail.params.patientID);
            }
        });

        if (demographicDetail.params.NotFoundPatientDetails && demographicDetail.params.NotFoundPatientDetails["FirstName"] != null) {
            $('#' + demographicDetail.params.PanelID + ' #txtFirstName').val(demographicDetail.params.NotFoundPatientDetails["FirstName"]);
            $('#' + demographicDetail.params.PanelID + ' #txtLastName').val(demographicDetail.params.NotFoundPatientDetails["LastName"]);
            $('#' + demographicDetail.params.PanelID + ' #dtpDOB').val(demographicDetail.params.NotFoundPatientDetails["DateOfBirth"]);
            $('#' + demographicDetail.params.PanelID + " #dtpDOB").datepicker("setDate", demographicDetail.params.NotFoundPatientDetails["DateOfBirth"]);
            $('#' + demographicDetail.params.PanelID + " #dtpDOB").trigger('change');
            demographicDetail.CalculateAge($('#' + demographicDetail.params.PanelID + ' #dtpDOB')[0]);
        }

        // load patient Documents from Dashboad flow
        if (demographicDetail.params["ParentCtrl"] == 'mstrTabDashBoard' && $('#pnlDashboard #widgetpanel #Documents ').hasClass('active')) {
            $('#' + demographicDetail.params.PanelID + ' #lnkDocuments').click();
        }

        if (demographicDetail.params.FormsTitle) {
            var title = $("#" + demographicDetail.params["PanelID"] + " #lblHeading").html() + ' <span class="maroon">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp(' + demographicDetail.params.FormsTitle + ')</span>';
            $("#" + demographicDetail.params["PanelID"] + " #lblHeading").html(title);
        }
        if (globalAppdata["isDemographics"] && globalAppdata["isDemographics"].toLowerCase() == "false") {
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divSexualOrientation").addClass("hidden");
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divGenderIdentity").addClass("hidden");
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divBirthSex").addClass("hidden");
        } else {
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divSexualOrientation").removeClass("hidden");
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divGenderIdentity").removeClass("hidden");
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divBirthSex").removeClass("hidden");
        }

        if (globalAppdata["isTransPubHealthAgHealthCareSurveys"] && globalAppdata["isTransPubHealthAgHealthCareSurveys"].toLowerCase() == "false") {
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divPatientPreferredPhone").addClass("hidden");
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divPatientCountry").addClass("hidden");
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divPatientPreferredAddress").addClass("hidden");
        }
        else {
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divPatientPreferredPhone").removeClass("hidden");
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divPatientCountry").removeClass("hidden");
            $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #divPatientPreferredAddress").removeClass("hidden");
        }

    },
    populateDODischarge: function (obj) {
        if ($(obj).prop("checked") == true) {
            $(" #dtpDischargeDate").datepicker('setDate', new Date());
        } else {
            $(" #dtpDischargeDate").datepicker('setDate', null);
        }
    },
    enableDisableDropDownLists: function (ddlIds, isHide) {
        if (typeof ddlIds != 'undefined' && ddlIds != null) {
            ddlCommaSeparatedIds = ddlIds.split(',');
            var parrentPanelId = "#" + demographicDetail.params["PanelID"];
            $.each(ddlCommaSeparatedIds, function (index, Item) {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            });
        }
    },
    //End By KR to make Race drop down as multi-select

    IntializeMultiSelectDropDownEthnicity: function () {
        $('#' + demographicDetail.params.PanelID + ' #ddlEthnicity').multiselect('destroy');
        $('#' + demographicDetail.params.PanelID + ' #ddlEthnicity').multiselect({
            //includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            onChange: function (element, checked) {
                var self = $('#pnldemographicDetail');
                var options = $(element).parent().find('option');
                var Selectedoptions = $(element).parent().find('option:selected');
                if (element.length > 0) {
                    var optionText = $(element)[0].outerText;
                    var optionVal = $($(element)[0]).val();
                    if (checked) {
                        if (optionText.trim().toLowerCase() == "declined to specify" || optionText.trim().toLowerCase() == "refused to report") {
                            Selectedoptions.each(function (i, itm) {
                                if ($(itm).val() != optionVal)
                                    $(this).prop('selected', false);
                            });
                            $('#' + demographicDetail.params.PanelID + " #ddlEthnicity").multiselect('refresh');
                            options.each(function (e, item) {
                                if ($(item).val() != optionVal) {
                                    var input = $('#pnldemographicDetail #divPatientEthnicity input[type=checkbox][value="' + $(this).val() + '"]');
                                    input.prop('disabled', true);
                                    input.parent('li').addClass('disabled');
                                }
                            });
                        }
                    }
                    else {
                        if (optionText.trim().toLowerCase() == "declined to specify" || optionText.trim().toLowerCase() == "refused to report") {
                            options.each(function (e, item) {
                                if ($(item).val() != optionVal) {
                                    var input = $('#pnldemographicDetail #divPatientEthnicity input[type=checkbox][value="' + $(this).val() + '"]');
                                    input.prop('disabled', false);
                                    input.parent('li').removeClass('disabled');
                                }
                            });
                        }
                        else {
                            options.each(function () {
                                var input = $('#pnldemographicDetail #divPatientEthnicity input[type=checkbox][value="' + $(this).val() + '"]');
                                input.prop('disabled', false);
                                input.parent('li').addClass('disabled');
                            });
                        }
                    }
                }
                $('#' + demographicDetail.params.PanelID + " #ddlEthnicity").multiselect('updateButtonText');
                var EthnicityIds = self.find('#divPatientEthnicity ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                    return this.value;
                }).get().join(',');
                demographicDetail.multipleEthnicityIds = EthnicityIds;
                if (demographicDetail.multipleEthnicityIds != '')
                    demographicDetail.validateEthnicity(2);
                else
                    demographicDetail.validateEthnicity(1);
            }
        });
        $('#' + demographicDetail.params.PanelID + " #ddlEthnicity").val("");
        $('#pnldemographicDetail').find('#divPatientEthnicity ul.multiselect-container li input[type=checkbox]').each(function () {
            if ($(this).attr('refval') == "hidden") {
                $(this).parent().addClass('text-bold');
            }
        });
    },
    validateRace: function (operationid) {
        $("#pnldemographicDetail #frmdemographicDetail #divPatientRace label").find("i").remove();
        if (operationid == 1) {
            $("#pnldemographicDetail #frmdemographicDetail #divPatientRace .multiselect").css("border-color", "#cc2724");
            $("#pnldemographicDetail #frmdemographicDetail #divPatientRace").find(".control-label").css("color", "#cc2724");
            $("#pnldemographicDetail #frmdemographicDetail #divPatientRace").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnldemographicDetail #frmdemographicDetail #divPatientRace .multiselect").css("border-color", "#3c763d");
            $("#pnldemographicDetail #frmdemographicDetail #divPatientRace").find(".control-label").css("color", "#3c763d");
            $("#pnldemographicDetail #frmdemographicDetail #divPatientRace").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#pnldemographicDetail #frmdemographicDetail #divPatientRace .multiselect").css("border-color", "#ccc");
            $("#pnldemographicDetail #frmdemographicDetail #divPatientRace").find(".control-label").css("color", "#000000");
        }
    },
    validateEthnicity: function (operationid) {
        $("#pnldemographicDetail #frmdemographicDetail #divPatientEthnicity label").find("i").remove();
        if (operationid == 1) {
            $("#pnldemographicDetail #frmdemographicDetail #divPatientEthnicity .multiselect").css("border-color", "#cc2724");
            $("#pnldemographicDetail #frmdemographicDetail #divPatientEthnicity").find(".control-label").css("color", "#cc2724");
            $("#pnldemographicDetail #frmdemographicDetail #divPatientEthnicity").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Ethnicity" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnldemographicDetail #frmdemographicDetail #divPatientEthnicity .multiselect").css("border-color", "#3c763d");
            $("#pnldemographicDetail #frmdemographicDetail #divPatientEthnicity").find(".control-label").css("color", "#3c763d");
            $("#pnldemographicDetail #frmdemographicDetail #divPatientEthnicity").find(".control-label").prepend('<i class="form-control-feedback glyphicon green glyphicon-ok" data-bv-icon-for="Ethnicity" style="display: block;"></i>');
        } else {
            $("#pnldemographicDetail #frmdemographicDetail #divPatientEthnicity .multiselect").css("border-color", "#ccc");
            $("#pnldemographicDetail #frmdemographicDetail #divPatientEthnicity").find(".control-label").css("color", "#000000");
        }
    },
    LoadAllAutocomplete: function () {

        //demographicDetail.BindLanguages();
        utility.CreateDatePicker(demographicDetail.params["PanelID"] + ' #frmdemographicDetail #dtpDOB',
      null, false);
        utility.CreateDatePicker("pnldemographicDetail #frmdemographicDetail #dtpDateOfDeath", function () {
        }, true);

        $('#pnldemographicDetail #frmdemographicDetail #dtpDateOfDeath').datepicker('setEndDate', new Date());
    },
    //LoadPatientDemogrphic: function () {
    //    $('#pnlDemographic #txtProvider').val(globalAppdata['DefaultProviderName']);
    //    $('#pnlDemographic #hfProvider').val(globalAppdata['DefaultProviderId']);
    //    $('#pnlDemographic #txtFacility').val(globalAppdata['DefaultFacilityName']);
    //    $('#pnlDemographic #hfFacility').val(globalAppdata['DefaultFacilityId']);
    //    $('#pnlDemographic #txtPractice').val(globalAppdata['DefaultPracticeName']);
    //    $('#pnlDemographic #hfPractice').val(globalAppdata['DefaultPracticeId']);
    //},

    LoadPatientDemogrphic: function () {

        if (demographicDetail.params.mode == "Add") {
            demographicDetail.ProviderLabel = "Provider";
            $("#" + demographicDetail.params["PanelID"] + " #frmdemographicDetail #lblProvider").val(demographicDetail.ProviderLabel);
            demographicDetail.FacilityLabel = "Facility";
            $("#" + demographicDetail.params["PanelID"] + " #frmdemographicDetail #lblFacility").val(demographicDetail.FacilityLabel);
            $("#" + demographicDetail.params.PanelID + " #dtpDischargeDate").datepicker('setDate', null);
            if (globalAppdata['DefaultProviderName'] != "" && globalAppdata['DefaultProviderName'] != "- Select -")
                $('#' + demographicDetail.params.PanelID + ' #txtProvider').val(globalAppdata['DefaultProviderName']);
            if (globalAppdata['DefaultProviderId'] != "")
                $('#' + demographicDetail.params.PanelID + ' #hfProvider').val(globalAppdata['DefaultProviderId']);
            if (globalAppdata['DefaultProviderId'] != "") {
                $('#' + demographicDetail.params.PanelID + ' #lnkProviderEdit').css("display", "inline");
                $('#' + demographicDetail.params.PanelID + ' #lblProvider').css("display", "none");
            }

            if (globalAppdata['DefaultFacilityName'] != "" && globalAppdata['DefaultFacilityName'] != "- Select -")
                $('#' + demographicDetail.params.PanelID + ' #txtFacility').val(globalAppdata['DefaultFacilityName']);
            if (globalAppdata['DefaultFacilityId'] != "")
                $('#' + demographicDetail.params.PanelID + ' #hfFacility').val(globalAppdata['DefaultFacilityId']);
            if (globalAppdata['DefaultFacilityId'] != "") {
                $('#' + demographicDetail.params.PanelID + ' #lnkFacilityEdit').css("display", "inline");
                $('#' + demographicDetail.params.PanelID + ' #lblFacility').css("display", "none");
            }
            if (globalAppdata['DefaultPracticeName'] != "")
                $('#' + demographicDetail.params.PanelID + ' #txtPractice').val(globalAppdata['DefaultPracticeName']);
            if (globalAppdata['DefaultPracticeId'] != "") {
                $('#' + demographicDetail.params.PanelID + ' #hfPractice').val(globalAppdata['DefaultPracticeId']);
                demographicDetail.ScanOCRPriviliges(globalAppdata['DefaultPracticeId']);
            }

            $("#" + demographicDetail.params.PanelID + " #ddlPrefLanguage").find("option").filter(function (index) {
                return "english" === $(this).text().toLowerCase();
            }).prop("selected", "selected");

            demographicDetail.SetAutoCompleteSourceValues();
        }
        else if (demographicDetail.params.mode == "Edit") {
            demographicDetail.ProviderLabel = "Last Seen Provider";
            $("#" + demographicDetail.params["PanelID"] + " #frmdemographicDetail #lblProvider").text(demographicDetail.ProviderLabel);
            $("#" + demographicDetail.params["PanelID"] + " #frmdemographicDetail #lnkProviderEdit").text(demographicDetail.ProviderLabel);
            demographicDetail.FacilityLabel = "Last Seen Facility";
            $("#" + demographicDetail.params["PanelID"] + " #frmdemographicDetail #lblFacility").text(demographicDetail.FacilityLabel);
            $("#" + demographicDetail.params["PanelID"] + " #frmdemographicDetail #lnkFacilityEdit").text(demographicDetail.FacilityLabel);
            demographicDetail.FillPatientInfo(params).done(function () {
                if ($('#' + demographicDetail.params.PanelID + ' #lnkProviderEdit').css("display") == "none") {
                    $('#' + demographicDetail.params.PanelID + ' #lnkProviderEdit').css("display", "inline");
                    $('#' + demographicDetail.params.PanelID + ' #lblProvider').css("display", "none");
                }
                if ($('#' + demographicDetail.params.PanelID + ' #lnkFacilityEdit').css("display") == "none") {
                    $('#' + demographicDetail.params.PanelID + ' #lnkFacilityEdit').css("display", "inline");
                    $('#' + demographicDetail.params.PanelID + ' #lblFacility').css("display", "none");
                }
                if ($('#' + demographicDetail.params.PanelID + ' #hfRefProvider').val() != "") {
                    $('#' + demographicDetail.params.PanelID + ' #lnkRefProviderEdit').css("display", "inline");
                    $('#' + demographicDetail.params.PanelID + ' #lblRefProvider').css("display", "none");
                }
                if ($('#' + demographicDetail.params.PanelID + ' #hfPCP').val() != "") {
                    $('#' + demographicDetail.params.PanelID + ' #lnkPCPEdit').css("display", "inline");
                    $('#' + demographicDetail.params.PanelID + ' #lblPCP').css("display", "none");
                }
                if ($('#' + demographicDetail.params.PanelID + ' #hfGuarantor').val() != "") {
                    $('#' + demographicDetail.params.PanelID + ' #lnkGuarantorEdit').css("display", "inline");
                    $('#' + demographicDetail.params.PanelID + ' #lblGuarantor').css("display", "none");
                }

                demographicDetail.SetAutoCompleteSourceValues();

                var imageSrc = $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail  #imgPatient').attr('src');
                if (imageSrc == 'Content/images/default_male_profile.gif' || imageSrc == 'Content/images/default_female_profile.gif') {
                    $('#' + Patient_Demographic.params.PanelID + " #btnRemoveUploadedImage").hide();
                }
                else {
                    $('#' + Patient_Demographic.params.PanelID + " #btnRemoveUploadedImage").show();
                }
                //demographicDetail.FillPatientInfo(demographicDetail.params.patientID);
                //demographicDetail.params["DemographicId"] = demographicDetail.params.patientID;
            });
        }

    },

    SetAutoCompleteSourceValues: function () {

        $Ctrl_gr = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtGuarantor");
        $hfCtrl_gr = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfGuarantor");
        //Guarantor
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_gr, $Ctrl_gr.val(), $hfCtrl_gr, $hfCtrl_gr.val());
        $Ctrl_reft = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtProviderReferral");
        $hfCtrl_reft = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfProviderReferral");
        //RefProvider To
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_reft, $Ctrl_reft.val(), $hfCtrl_reft, $hfCtrl_reft.val());
        $Ctrl_ref = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtRefProviderReferral");
        $hfCtrl_ref = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfRefProviderReferral");
        //RefProvider From
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_ref, $Ctrl_ref.val(), $hfCtrl_ref, $hfCtrl_ref.val());
        $Ctrl_ref = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtRefProvider");
        $hfCtrl_ref = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfRefProvider");
        //RefProvider
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_ref, $Ctrl_ref.val(), $hfCtrl_ref, $hfCtrl_ref.val());
        $Ctrl_pcp = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtPCP");
        $hfCtrl_pcp = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfPCP");
        //PCP
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_pcp, $Ctrl_pcp.val(), $hfCtrl_pcp, $hfCtrl_pcp.val());
        $Ctrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtFacility");
        $hfCtrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfFacility");
        //Facility
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl, $Ctrl.val(), $hfCtrl, $hfCtrl.val());
        $Ctrl_p = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtProvider");
        $hfCtrl_p = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfProvider");
        //Provider
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, $Ctrl_p.val(), $hfCtrl_p, $hfCtrl_p.val());
        $Ctrl_PreferedLanguage = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #ddlPrefLanguage");
        $hfCtrl_PreferedLanguage = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfLanguages");
        //Prefered Languages
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_PreferedLanguage, $Ctrl_PreferedLanguage.val(), $hfCtrl_PreferedLanguage, $hfCtrl_PreferedLanguage.val());
        $Ctrl_Country = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #ddlCountry");
        $hfCtrl_Country = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfCountry");
        //Countries
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_Country, $Ctrl_Country.val(), $hfCtrl_Country, $hfCtrl_Country.val());

        $Ctrl_Cities = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtCity");
        $hfCtrl_Cities = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfCity");
        //Cities
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_Cities, $Ctrl_Cities.val(), $hfCtrl_Cities, $hfCtrl_Cities.val());

    },

    RemoveUploadImage: function () {

        utility.myConfirm('Are you sure to delete the picture ?', function () {
            $('#' + demographicDetail.params.PanelID + " #frmdemographicDetail #imgPatient").attr("src", "Content/images/default_male_profile.gif");
            var self = $('#' + demographicDetail.params.PanelID);

            var strRaceIds = $('#' + demographicDetail.params.PanelID + ' #hfRaceIds').val();
            var EthnicityIds = self.find('#divPatientEthnicity ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                return this.value;
            }).get().join(',');


            var myJSON = self.getMyJSONByName();
            var strRaceIds = $('#' + demographicDetail.params.PanelID + ' #hfRaceIds').val();
            var FromPatientDetail = "1";
            Patient_Demographic.IsImageUpdated = true;
            Patient_Demographic.UpdateDemographic(myJSON, demographicDetail.params.patientID, strRaceIds, FromPatientDetail, EthnicityIds, true).done(function (response) {
                Patient_Demographic.IsImageUpdated = '';
                Patient_Demographic.params["isFromPictureMode"] = '';
                demographicDetail.LoadPatientDemogrphic();
                Patient_Demographic.LoadPatientDemogrphic();
                $('#' + demographicDetail.params.PanelID + " #frmdemographicDetail #imgPatient").css('cursor', 'default');
                utility.DisplayMessages("Successfully Deleted", 1);
            });
        }


         , function () {
             //NO CALLBACK
         },
  'Confirm Delete'

  //
);

    },

    FillPatientInfo: function (params) {
        //demographicDetail.params = params;
        var dfd = new $.Deferred();
        //ClosePatient();
        var patientJSON = ""; //store.fetch('selectPatientData', patientID);
        if (patientJSON != "") {
            var demographic_detail = JSON.parse(patientJSON);
            demographicDetail.BindJSON(demographic_detail);

            //if (response.Image_url != "") {
            //    $("#" + demographicDetail.params.PanelID + " #divdemographicDetailPicture #imgPatient").attr("src", response.Image_url);
            //}
            //else {
            if (demographic_detail.Sex == "Female") {
                $("#" + demographicDetail.params.PanelID + " #divdemographicDetailPicture #imgPatient").attr("src", "Content/images/default_female_profile.gif");

            }
            //}


        }
        else {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Demographic", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Patient_Demographic.FillDemographic(demographicDetail.params.patientID).done(function (response) {
                        if (response.status != false) {
                            var demographic_detail = JSON.parse(response.DemographicFill_JSON);
                            if (demographic_detail.Active == 2) {
                                $('#' + demographicDetail.params.PanelID + " #frmdemographicDetail #divDateOfDeath").removeClass('hidden');
                            }
                            else {
                                $('#' + demographicDetail.params.PanelID + " #frmdemographicDetail #divDateOfDeath").addClass('hidden');
                            }

                            demographicDetail.BindJSON(demographic_detail).done(function () {
                                utility.ChangeNameCase($("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #ddlCountry"));   //change country text from UpperCase to Title Case
                                if (demographic_detail.strRaceIds) {
                                    var RaceArr = demographic_detail.strRaceIds.split(',');
                                    utility.callbackAfterAllDOMLoaded(function () {
                                        var values_ = new Array();
                                        var multiselect = $('#' + demographicDetail.params.PanelID + ' #ddlPatientRace').data("kendoMultiSelect");
                                        var multiselect_data = multiselect.dataSource.data();
                                        for (var i = 0; i < demographic_detail.PatientRaces.length; i++) {
                                            values_[i] = demographic_detail.PatientRaces[i].Id;

                                            var filter_data = $.grep(multiselect_data, function (item, j) {
                                                return item.Name == demographic_detail.PatientRaces[i].Name
                                                && item.Value == demographic_detail.PatientRaces[i].Id
                                            });

                                            if (filter_data.length == 0)
                                                multiselect_data.push({ Name: demographic_detail.PatientRaces[i].Name, Value: demographic_detail.PatientRaces[i].Id });
                                        }

                                        multiselect.dataSource.data(multiselect_data);
                                        multiselect.value(values_);
                                        multiselect.refresh();
                                        $('#' + demographicDetail.params["PanelID"] + ' #hfRaceIds').val(values_.join(','));
                                        $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail').data('serialize', $('#frmdemographicDetail').serialize());
                                    });
                                }

                                if (demographic_detail.strEthnicityIds) {
                                    var arrEthnicityIds = demographic_detail.strEthnicityIds.split(',');
                                    utility.callbackAfterAllDOMLoaded(function () {
                                        $.each(arrEthnicityIds, function (i, itm) {
                                            var OptionText = $('#' + demographicDetail.params.PanelID + " #ddlEthnicity option[value='" + itm + "']").text();
                                            if (OptionText.toLowerCase() == "declined to specify" || OptionText.toLowerCase() == "refused to report") {
                                                $('#' + demographicDetail.params.PanelID + " #ddlEthnicity option").each(function (e, item) {
                                                    if ($(item).val() != itm) {
                                                        var input = $('#pnlDemographic #divPatientEthnicity input[type=checkbox][value="' + $(item).val() + '"]');
                                                        input.prop('disabled', true);
                                                        input.parent('li').addClass('disabled');
                                                    }
                                                });
                                            }
                                        });
                                        $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail').data('serialize', $('#frmdemographicDetail').serialize());
                                    });
                                    $('#' + demographicDetail.params.PanelID + " #ddlEthnicity").val(arrEthnicityIds);
                                    $('#' + demographicDetail.params.PanelID + " #ddlEthnicity").multiselect("refresh");
                                    demographicDetail.multipleEthnicityIds = demographic_detail.strEthnicityIds;
                                } else {
                                    $('#' + demographicDetail.params.PanelID + " #ddlEthnicity").find("option:selected").removeAttr("selected");
                                }
                                //  ScanPrivilige = demographic_detail.Scan;
                                //  OCRPrivilige = demographic_detail.OCR;

                                if (demographic_detail.imgPatient != undefined && demographic_detail.imgPatient != "") {
                                    $("#" + demographicDetail.params.PanelID + " #divdemographicDetailPicture #imgPatient").attr("src", demographic_detail.imgPatient);
                                }
                                else {
                                    if (demographic_detail.Sex == "Female") {
                                        $("#" + demographicDetail.params.PanelID + " #divdemographicDetailPicture #imgPatient").attr("src", "Content/images/default_female_profile.gif");

                                    }
                                }
                                //Start 01-11-2016 Humaira Yousaf to load hear from data
                                $("#" + demographicDetail.params.PanelID + " #ddlHearFrom").val(demographic_detail.HearFromId);
                                if (demographic_detail.HearFromId == '1') {
                                    demographicDetail.getPatientReferral();
                                    $("#" + demographicDetail.params.PanelID + " #incomingReferral").removeClass('hidden');
                                }
                                else {
                                    $("#" + demographicDetail.params.PanelID + " #incomingReferral").addClass('hidden');
                                }

                                if (demographic_detail.HearFromId == '10') {
                                    $("#" + demographicDetail.params.PanelID + " #txtOtherText").val(demographic_detail.HearFromOther);
                                    $("#" + demographicDetail.params.PanelID + " #divOtherText").removeClass('hidden');
                                }
                                else {
                                    $("#" + demographicDetail.params.PanelID + " #txtOtherText").val('');
                                    $("#" + demographicDetail.params.PanelID + " #divOtherText").addClass('hidden');
                                }
                                if (demographic_detail.SSN) {
                                    demographicDetail.PatientSSN = demographic_detail.SSN;
                                    $('#pnldemographicDetail #frmdemographicDetail #hfPatientSSN').val(demographic_detail.SSN);
                                    if (globalAppdata.IsFullSSN.toLowerCase() === 'true') {

                                        $('#pnldemographicDetail #frmdemographicDetail #txtSSN').attr("data-mask", "999-99-9999");
                                        $('#pnldemographicDetail #frmdemographicDetail #txtSSN').val(demographic_detail.SSN);
                                        $('#pnldemographicDetail #frmdemographicDetail #txtSSN').attr("disabled", false);
                                    }
                                    else {

                                        $('#pnldemographicDetail #frmdemographicDetail #txtSSN').attr("data-mask", "xxx-xx-9999");
                                        var lastFourDigit = demographic_detail.SSN.slice(-4);
                                        var formatSSN = "XXX-XX-" + lastFourDigit;
                                        $('#pnldemographicDetail #frmdemographicDetail #txtSSN').val(formatSSN);
                                        $('#pnldemographicDetail #frmdemographicDetail #txtSSN').attr("disabled", true);
                                    }
                                }
                                else {
                                    Patient_Demographic.PatientSSN = demographic_detail.SSN;
                                    $('#pnldemographicDetail #frmdemographicDetail #txtSSN').attr("placeholder", "999-99-9999");
                                    $('#pnldemographicDetail #frmdemographicDetail #txtSSN').attr("data-mask", "999-99-9999");
                                    $('#pnldemographicDetail #frmdemographicDetail #txtSSN').val(demographic_detail.SSN);
                                    $('#pnldemographicDetail #frmdemographicDetail #txtSSN').attr("disabled", false);

                                }
                                demographicDetail.ProviderFName = demographic_detail.ProviderFName;
                                demographicDetail.ProviderLName = demographic_detail.ProviderLName;
                                demographicDetail.FacilityAddress = demographic_detail.FacilityAddress;
                                demographicDetail.FacilityCity = demographic_detail.FacilityCity;
                                demographicDetail.FacilityState = demographic_detail.FacilityState;
                                demographicDetail.FacilityZip = demographic_detail.FacilityZip;
                                demographicDetail.FacilityZipExt = demographic_detail.FacilityZipExt;
                                demographicDetail.FacilityPhone = demographic_detail.FacilityPhone;
                                //End 01-11-2016 Humaira Yousaf to load hear from data
                                //  Patient_Demographic.FillPatientBar(response.DemographicFill_JSON, demographicDetail.params.patientID);
                                dfd.resolve('ok');

                            });
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                            return dfd.promise();
                        }
                    });
                }
                else {
                    utility.DisplayMessages(strMessage, 2);
                    return dfd.promise();
                }
            });
        }
        return dfd.promise();
    },



    OpenEmergencyContact: function () {
        var params = [];
        params["EmergencyContactId"] = "-1";
        params["FromAdmin"] = "0";
        params["patientID"] = demographicDetail.params.patientID;
        params["ParentCtrl"] = "demographicDetail";
        LoadActionPan('Patient_EmergencyContact', params);
    },

    OpenUploadImage: function () {
        var params = [];
        //params = demographicDetail.params
        params["patientID"] = demographicDetail.params.patientID;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "demographicDetail";
        setDefaultValuesForScanCanvas(500, 360);
        params["PracticeId"] = $("#frmdemographicDetail #hfPractice").val();
        LoadActionPan('uploadImage', params);
    },

    OpenFamily: function () {
        var params = [];
        params["FamilyId"] = "-1";
        params["FromAdmin"] = "0";
        params["patientID"] = demographicDetail.params.patientID;
        params["ParentCtrl"] = "demographicDetail";
        LoadActionPan('Patient_Family', params);
    },

    OpenPreferences: function () {
        AppPrivileges.GetFormPrivileges("Preferences", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientPreferencesId"] = "-1";
                params["FromAdmin"] = "0";
                params["patientID"] = demographicDetail.params.patientID;
                params["GuarantorID"] = demographicDetail.params.GuarantorID;
                params["ParentCtrl"] = "demographicDetail";
                LoadActionPan('Patient_Preferences', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    OpenAdvancePayment: function (isIndependent) {
        var params = [];
        params["PatientAdvancePaymentSearchId"] = "-1";
        params["FromAdmin"] = "0";
        params["patientID"] = demographicDetail.params.patientID;
        if (isIndependent)
            params["ParentCtrl"] = selectedtab.TabID;
        else
            params["ParentCtrl"] = "demographicDetail";

        params["PatAccountNumber"] = $('#' + demographicDetail.params.PanelID + ' #txtAccountNo').val();
        params["PatFirstName"] = $('#' + demographicDetail.params.PanelID + ' #txtFirstName').val();
        params["PatLastName"] = $('#' + demographicDetail.params.PanelID + ' #txtLastName').val();
        params["PatMidInitial"] = $('#' + demographicDetail.params.PanelID + ' #txtMiddleInitial').val();
        params["IsPatientDetail"] = "1";

        LoadActionPan('Patient_AdvancePayment', params);
    },
    BindJSON: function (demographic_detail) {
        //var demographic_detail = JSON.parse(JSONData);

        var self = $('#' + demographicDetail.params.PanelID);
        return utility.bindMyJSONByName(true, demographic_detail, false, self).done(function () {

            //if (demographic_detail.Active == 'True')
            //    $('#' + demographicDetail.params.PanelID + ' #chkActive').attr("checked", true);
            //else {
            //    $('#' + demographicDetail.params.PanelID + ' #chkActive').attr("checked", false);
            //    $('#' + demographicDetail.params.PanelID + ' #dvresion').show();
            //}
            if (demographic_detail.Active == 0)
                $('#' + demographicDetail.params.PanelID + ' #dvresion').show();
            else {

                $('#' + demographicDetail.params.PanelID + ' #dvresion').hide();
            }
            if (demographic_detail.BadAddress == 'True')
                $('#' + demographicDetail.params.PanelID + ' #chkBadAddress').attr("checked", true);
            else
                $('#' + demographicDetail.params.PanelID + ' #chkBadAddress').attr("checked", false);

            demographicDetail.params.GuarantorID = demographic_detail.GuarantorID;
            demographicDetail.params.PatientFullName = demographic_detail.FullName;
        });


    },

    ValidateDemographic: function () {
        $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  LastName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  FirstName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Sex: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DOB: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  MaritalStatus: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Ethnicity: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  PatientRaceIds: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  PrefLanguage: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Address1: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  City: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Provider: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Facility: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Practice: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Zip: {
                      group: '.col-xs-8',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  hiddenPatientRace: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  State: {
                      group: '.size30per',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  //RefProvider: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  //PCP: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  //Guarantor: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  Email: {
                      group: '.col-sm-3',
                      enabled: false,
                      validators: {
                          //emailAddress: {
                          //    message: 'Email not Valid'
                          //},
                          regexp: {
                              regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                              message: 'Email not Valid'
                          },
                      }
                  },
                  RefProviderReferral: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  Status: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  HomeTel: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
              }
          })
            .on('keyup', 'input#txtEmail', function (e) {
                var formValidation = $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail').data("bootstrapValidator");
                switch ($(this).attr("name")) {
                    case 'Email':
                        var OccurenceCod1Val = $("input#txtEmail").val();
                        if (OccurenceCod1Val != "") {
                            //formValidation.enableFieldValidators('OccurrenceDate1', true);//.revalidateField('OccurrenceDate1');
                            formValidation.enableFieldValidators('Email', true);
                        }
                        else
                            formValidation.enableFieldValidators('Email', false);
                        break;
                    default:
                        break;
                }
            })
                .on('click', '#btnDemographic', function (e) {
                    if (demographicDetail.multipleEthnicityIds == '')
                        demographicDetail.validateEthnicity(1);
                    //$('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail select[multiple]').each(function () {
                    //    if ($(this).val() == null || $(this).val() == "") {
                    //        $(this).closest('div').find('label.control-label').addClass('has-error');
                    //        $(this).closest('div').find('label.control-label').css('color', '#cc2724');
                    //        $(this).closest('div').find('button').css('border-color', '#cc2724');
                    //        $('#multiselectRace').css('display', '');

                    //    } else {
                    //        $(this).closest('div').find('label.control-label').removeClass('has-error');
                    //        $(this).closest('div').find('label.control-label').css('color', '');
                    //        $(this).closest('div').find('button').css('border-color', '');
                    //        $('#multiselectRace').css('display', 'none');

                    //    }
                    //});
                })
           .on('success.form.bv', function (e) {
               e.preventDefault();
               demographicDetail.DemographicSave();
           });

    },

    multiselect_Validator: function (cntrl) {
        if ($(cntrl).val() == null || $(cntrl).val() == "") {
            $(cntrl).closest('div').find('label.control-label').addClass('has-error');
            $(cntrl).closest('div').find('label.control-label').css('color', '#cc2724');
            $(cntrl).closest('div').find('button').css('border-color', '#cc2724');
            $('#multiselectRace').css('display', '');
        } else {
            $(cntrl).closest('div').find('label.control-label').removeClass('has-error');
            $(cntrl).closest('div').find('label.control-label').css('color', '');
            $(cntrl).closest('div').find('button').css('border-color', '');
            $('#multiselectRace').css('display', 'none');
        }
    },
    openDrFirst: function () {
        var params = [];
        params["StartupScreen"] = "patient";
        params["PatientId"] = demographicDetail.params.patientID;
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = "demographicDetail";
        LoadActionPan("DRFirst", params);


    },

    VarifyMUAlert: function () {
        var PatientId = demographicDetail.params.PatientId ? demographicDetail.params.PatientId : demographicDetail.params.patientID;
        var obj_ = utility.MU3Demographics("#" + demographicDetail.params.PanelID + " #frmdemographicDetail", PatientId);
        if (obj_ != null) {
            if (demographicDetail.params.mode == "Add") {
                Patient_Demographic.SaveMUAlert(obj_).done(function (result) {
                    if (result.status != false) {
                        utility.toggelMU3Alerts(true);
                    }
                    else {
                        console.log(result.Message);
                    }
                });
            }
            else {
                Patient_Demographic.UpdateMUAlert(obj_).done(function (result) {
                    if (result.status != false) {
                        var data = JSON.parse(result.MUAlerts_JSON);
                        var IsAnyOtherAlert = data.filter(item=>item.PatientId == PatientId);
                        if (IsAnyOtherAlert.length > 0 && result.MissingDataAlertCount > 0 ) {
                            utility.toggelMU3Alerts(true, result.MissingDataAlertCount);
                        }
                        else {
                            utility.toggelMU3Alerts(false, result.MissingDataAlertCount);
                        }

                    }
                    else {
                        console.log(result.Message);
                    }
                });
            }
        }
    },

    ValidateValues: function () {
        if ($('#' + demographicDetail.params.PanelID + ' #hfPractice').val() == "-1") {
            utility.DisplayMessages("Practice not Valid", 2);
            return false;
        }
        else
            return true;
    },

    DemographicSave: function () {
        var self = $('#' + demographicDetail.params.PanelID);
        if (demographicDetail.PatientSSN) {

            var txtValue = self.find("#txtSSN").val();   // get ssn value
            if (txtValue) {
                if (globalAppdata.IsFullSSN.toLowerCase() === 'false') {
                    self.find("#txtSSN").val(demographicDetail.PatientSSN);
                }
                self.find("#hfPatientSSN").val(Patient_Demographic.PatientSSN);
            }
            else {
                self.find("#txtSSN").val("");
                self.find("#hfPatientSSN").val("");
            }
        }
        var strRaceIds = $('#' + demographicDetail.params.PanelID + ' #hfRaceIds').val();
        var EthnicityIds = self.find('#divPatientEthnicity ul.multiselect-container li input[type=checkbox]:checked').map(function () {
            return this.value;
        }).get().join(',');
        demographicDetail.multipleEthnicityIds = EthnicityIds;

        if (strRaceIds != '') {
            demographicDetail.validateRace(2);

            if (!EthnicityIds) {
                return;
            } else {
                demographicDetail.validateEthnicity(2);
            }

            $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail').bootstrapValidator('revalidateField', 'LastName');

            var strMessage = "";
            var self = $('#' + demographicDetail.params.PanelID);
            var myJSON = self.getMyJSONByName();
            var objMyJSON = JSON.parse(myJSON);

            //-----------------------
            var Imagedata = demographicDetail.imagedata;
            var Filetype = demographicDetail.filetype;
            var Filename = demographicDetail.filename;
            //var Foldername = demographicDetail.scannerjson;
            // var Foldername = "";
            //-----------------------
            objMyJSON.imgBase64 = $('#' + demographicDetail.params.PanelID + ' #imgPatient').attr('src');
            myJSON = JSON.stringify(objMyJSON);
            self.find("#txtSSN").val(txtValue); //hide ssn
            if (demographicDetail.params.mode == "Add") {
                AppPrivileges.GetFormPrivileges("Demographic", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        var result;
                        result = demographicDetail.ValidateValues();
                        if (result != false) {
                            var objData = JSON.parse(myJSON);
                            objData["strEthnicityIds"] = EthnicityIds;
                            myJSON = JSON.stringify(objData);
                            Patient_Demographic.SaveDemographic(myJSON, "", strRaceIds, Imagedata, Filetype, Filename).done(function (response) {
                                if (response.status != false) {
                                    demographicDetail.params.PatientId = response.PatientId;
                                    demographicDetail.VarifyMUAlert();
                                    // $("#txtAccountNo").val(response.AccountNumber);
                                    $("#" + demographicDetail.params["PanelID"] + " #txtAccountNo").val(response.AccountNumber);
                                    utility.DisplayMessages(response.message, 1);
                                    var badAddressCheck = $('#' + demographicDetail.params.PanelID + ' #chkBadAddress').prop('checked');
                                    if (demographicDetail.params.ParentCtrl == "Patient_Search") {
                                        //Patient_Search.PatientSearch(response.PatientId, null, null, 0, badAddressCheck);

                                        if ((demographicDetail.params.IsNotFromParent && demographicDetail.params.IsNotFromParent == true)) {
                                            UnloadActionPan(demographicDetail.params["ParentCtrl"], "demographicDetail");
                                            setTimeout(function () {
                                                Patient_Search.SelectPatient(response.PatientId, null);
                                            }, 500);
                                        }
                                        else if (demographicDetail.params.GrandParentCtrl == "appointmentDetail") {
                                            demographicDetail.params.patientID = response.PatientId;
                                            demographicDetail.params.mode = "Edit";
                                            $("#" + demographicDetail.params["PanelID"] + " #lnkInsurance").removeClass('disableAll');
                                            $("#" + demographicDetail.params["PanelID"] + " #lnkDocuments").removeClass('disableAll');
                                        }
                                        else {
                                            Patient_Search.PatientSearch(response.PatientId, null, null, 0, badAddressCheck);
                                            UnloadActionPan(demographicDetail.params["ParentCtrl"]);
                                        }


                                    } else {
                                        UnloadActionPan(demographicDetail.params["ParentCtrl"]);
                                    }
                                    //else
                                    //    SelectPatient(response.PatientId, "");
                                    demographicDetail.imagedata = "";
                                    demographicDetail.filetype = "";
                                    demographicDetail.filename = "";
                                    $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail').data('serialize', $('#frmdemographicDetail').serialize());
                                    //demographicDetail.scannerjson = "";
                                    //UnloadActionPan(demographicDetail.params["ParentCtrl"]);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
            else if (demographicDetail.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        var result;
                        result = demographicDetail.ValidateValues();
                        if (result != false) {
                            var FromPatientDetail = "1";
                            var objData = JSON.parse(myJSON);
                            objData["strEthnicityIds"] = EthnicityIds;
                            myJSON = JSON.stringify(objData);
                            Patient_Demographic.UpdateDemographic(myJSON, demographicDetail.params.patientID, strRaceIds, FromPatientDetail, demographicDetail.multipleEthnicityIds).done(function (response) {
                                if (response.status != false) {
                                    $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail').data('serialize', $('#frmdemographicDetail').serialize());
                                    if (demographicDetail.params.ParentCtrl == "Patient_Search" && demographicDetail.params.GrandParentCtrl != "appointmentDetail") {                                      
                                        Patient_Search.PatientSearch(demographicDetail.params.patientID);
                                        UnloadActionPan(demographicDetail.params["ParentCtrl"]);
                                    }


                                    demographicDetail.VarifyMUAlert();

                                    //demographicDetail.FillDemographic(demographicDetail.params.patientID)
                                    //.done(function (response) {
                                    //    store.clear('selectPatientData', demographicDetail.params.patientID);
                                    //    store.save('selectPatientData', response.DemographicFill_JSON, demographicDetail.params.patientID);
                                    //    demographicDetail.FillPatientBar(demographicDetail.params.patientID);
                                    //});

                                    //to update count and grid on dashboardpatientchanges
                                    if (demographicDetail.params['PanelID'] == "pnlDashboard #pnldemographicDetail" && $('#pnlDashboard #frmDashboard #PatientChanges').hasClass('active'))
                                        DashBoard.DashBoardPatientChangesSearch(null, null, null);
                                    var dfd = new $.Deferred();
                                    if (demographicDetail.params["IsFill"] != false && demographicDetail.params.GrandParentCtrl != "appointmentDetail") {
                                        if (demographicDetail.params.PatBanner == true) {
                                            var Params = demographicDetail.params;
                                            Params.PanelID = "pnlDemographic";
                                            Patient_Demographic.FillPatientInfo(Params).done(function () {
                                                if (demographicDetail.params.ParentCtrl == "schTabCalendar")
                                                    $('#pnlScheduleCalendar #containerScheduleMode button.active').trigger('onclick');
                                                if (demographicDetail.params.TabID == "schTabCalendar")
                                                    PMSScheduler.InitilizeScheduler();
                                                if (demographicDetail.params.GrandParent == "DashboardAppointment") {
                                                    UnloadActionPan();
                                                    DashBoard.NoteCreation(demographicDetail.params.PatientId, demographicDetail.params.AppointmentId, demographicDetail.params.ProviderId, demographicDetail.params.ProviderName, demographicDetail.params.AppointmentTime, demographicDetail.params.VisitId, demographicDetail.params.VisitDate, demographicDetail.params.Reason, demographicDetail.params.FacilityName, demographicDetail.params.FacilityId, demographicDetail.params.Room, demographicDetail.params.NotesId);
                                                }
                                                if (demographicDetail.params.CallBackFunction != null && typeof (demographicDetail.params.CallBackFunction) == 'function') {
                                                    $.when(demographicDetail.UnLoad()).then(function () {
                                                        var menu = demographicDetail.params.objButtonId.split("_");
                                                        if (menu.length > 0) {
                                                            if (menu[1] != null) {
                                                                if (menu[1] == "History") {
                                                                    menu[1] = "Histroy";
                                                                }
                                                                var menuTab = menu[0] + '' + menu[1];
                                                            }
                                                            else {
                                                                var menuTab = menu[0];
                                                            }

                                                            ClinicalMenuSettings.TopButtons(menuTab, null, demographicDetail.params.objButtonId);
                                                            if (menuTab != "clinicalMenuNotes")
                                                                setTimeout(demographicDetail.params.CallBackFunction, 20);
                                                        }
                                                        dfd.resolve();
                                                    });
                                                }
                                                else if (demographicDetail.params.ComeFrom == "Patient_Search") {
                                                    UnloadActionPan();
                                                    if ($("#hfClinicalMenuChildLiId").val() != "" && $("#hfClinicalMenuChildLiId").val() != "clinicalTabNotes")
                                                        $("#ClinicalUL li#" + $("#hfClinicalMenuChildLiId").val() + " a:first").trigger("click");
                                                    else if ($("#hfClinicalMenuChildLiId").val() == "clinicalTabNotes")
                                                        $("li#clinicalMenuNotes a:first").trigger("click");
                                                    else
                                                        $("#ClinicalUL li#clinicalMenuFaceSheet a:first").trigger("click");
                                                }
                                                else
                                                    dfd.resolve();
                                            });
                                        }
                                        else {
                                            if (demographicDetail.params.ParentCtrl == "schTabCalendar")
                                                $('#pnlScheduleCalendar #containerScheduleMode button.active').trigger('onclick');

                                            dfd.resolve();
                                        }

                                    }
                                    else {
                                        dfd.resolve();
                                    }

                                    dfd.done(function () {
                                        utility.DisplayMessages(response.message, 1);
                                        Patient_Demographic.IsImageUpdated = '';
                                        if (demographicDetail.params.GrandParentCtrl && demographicDetail.params.GrandParentCtrl != "appointmentDetail") {
                                            demographicDetail.UnLoad();
                                        }
                                    });
                                    // Unload Demographic detail clicks on save& exit button. 
                                    demographicDetail.UnLoad();
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }//edit end

        } else {
            if (strRaceIds == '')
                demographicDetail.validateRace(1);
            if (demographicDetail.multipleEthnicityIds == '')
                demographicDetail.validateEthnicity(1);
        }
    },

    setImageData: function () {
        demographicDetail.imagedata = $('#frmdemographicDetail #imagedatabase64').val();
    },

    // -------------- Provider ---------------------

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmdemographicDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "demographicDetail";
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#' + demographicDetail.params.PanelID + ' #hfProvider').val(), "demographicDetail");
        var params = [];
        params["ProviderId"] = $('#' + demographicDetail.params.PanelID + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'demographicDetail';
        LoadActionPan('providerDetail', params);
    },

    HideProviderLink: function () {
        $('#' + demographicDetail.params.PanelID + ' #txtProvider').attr("ProviderId", "-1");
        $('#' + demographicDetail.params.PanelID + ' #hfProvider').val("-1");
        $('#' + demographicDetail.params.PanelID + ' #lnkProviderEdit').css("display", "none");
        $('#' + demographicDetail.params.PanelID + ' #lblProvider').css("display", "inline");
    },
    // -------------- End Provider -----------------

    // -------------- Facility ---------------------

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmdemographicDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "demographicDetail";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#' + demographicDetail.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'demographicDetail';
        LoadActionPan('facilityDetail', params);
    },

    HideFacilityLink: function () {
        $('#' + demographicDetail.params.PanelID + ' #txtFacility').attr("FacilityId", "-1");
        $('#' + demographicDetail.params.PanelID + ' #hfFacility').val("-1");
        $('#' + demographicDetail.params.PanelID + ' #lnkFacilityEdit').css("display", "none");
        $('#' + demographicDetail.params.PanelID + ' #lblFacility').css("display", "inline");
    },
    // -------------- End Facility -----------------

    // -------------- Ref Provider -----------------
    FillRefProviderName: function (RefProviderId, RefProviderName) {
        $('#' + demographicDetail.params.PanelID + ' #txtRefProvider').val(RefProviderName);
        $('#' + demographicDetail.params.PanelID + ' #hfRefProvider').val(RefProviderId);
        UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"]);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["ParentCtrl"] = "demographicDetail";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenRefProviderDetail: function () {
        //Admin_ReferringProvider.ReferringProviderEdit($('#' + demographicDetail.params.PanelID + ' #hfRefProvider').val(), "demographicDetail");
        var params = [];
        params["ReferringProviderId"] = $('#' + demographicDetail.params.PanelID + ' #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = "demographicDetail";

        LoadActionPan('referringproviderDetail', params);
    },

    HideRefProviderLink: function () {
        $('#' + demographicDetail.params.PanelID + ' #hfRefProvider').val("-1");
        $('#' + demographicDetail.params.PanelID + ' #lnkRefProviderEdit').css("display", "none");
        $('#' + demographicDetail.params.PanelID + ' #lblRefProvider').css("display", "inline");
    },
    // -------------- End Ref Provider -------------

    // -------------- PCP --------------------------
    FillPCPName: function (PCPId, PCPName) {
        $('#' + demographicDetail.params.PanelID + ' #txtPCP').val(PCPName);
        $('#' + demographicDetail.params.PanelID + ' #hfPCP').val(PCPId);
        UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"]);
    },

    OpenPCP: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPCP";
        params["Title"] = "Search PCP Provider";
        params["ParentCtrl"] = "demographicDetail";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenPCPDetail: function () {
        //Admin_ReferringProvider.ReferringProviderEdit($('#' + demographicDetail.params.PanelID + ' #hfPCP').val(), "demographicDetail");
        var params = [];
        params["ReferringProviderId"] = $('#' + demographicDetail.params.PanelID + ' #hfPCP').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPCP";
        params["mode"] = "Edit";
        params["Title"] = "PCP";
        params["ParentCtrl"] = "demographicDetail";

        LoadActionPan('referringproviderDetail', params);
    },

    HidePCPLink: function () {
        $('#' + demographicDetail.params.PanelID + ' #hfPCP').val("-1");
        $('#' + demographicDetail.params.PanelID + ' #lnkPCPEdit').css("display", "none");
        $('#' + demographicDetail.params.PanelID + ' #lblPCP').css("display", "inline");
    },
    // -------------- End PCP ----------------------

    //// -------------- Guarantor ------------

    OpenGuarantor: function () {
        var params = [];
        params["GuarantorId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "demographicDetail";
        params["Address1"] = $('#' + demographicDetail.params.PanelID + ' #txtAddress1').val();
        params["Zip"] = $('#' + demographicDetail.params.PanelID + ' #txtZip').val();
        params["City"] = $('#' + demographicDetail.params.PanelID + ' #txtCity').val();
        params["State"] = $('#' + demographicDetail.params.PanelID + ' #txtState').val();
        params["RefCtrl"] = "txtGuarantor";
        LoadActionPan('Patient_Guarantor', params);
    },

    OpenGuarantorDetail: function () {
        //Patient_Guarantor.GuarantorEdit($('#' + demographicDetail.params.PanelID + ' #hfGuarantor').val(), "demographicDetail");
        var params = [];
        params["GuarantorId"] = $('#' + demographicDetail.params.PanelID + ' #hfGuarantor').val();
        params["mode"] = "Edit";
        params["RefCtrl"] = "txtGuarantor";
        params["ParentCtrl"] = 'demographicDetail';
        params["Address1"] = $('#' + demographicDetail.params.PanelID + ' #txtAddress1').val();
        params["Zip"] = $('#' + demographicDetail.params.PanelID + ' #txtZip').val();
        params["City"] = $('#' + demographicDetail.params.PanelID + ' #txtCity').val();
        params["State"] = $('#' + demographicDetail.params.PanelID + ' #txtState').val();
        params["PatientId"] = demographicDetail.params.patientID;
        LoadActionPan('guarantorDetail', params);
    },

    HideGuarantorLink: function () {
        $('#' + demographicDetail.params.PanelID + ' #hfGuarantor').val("-1");
        $('#' + demographicDetail.params.PanelID + ' #lnkGuarantorEdit').css("display", "none");
        $('#' + demographicDetail.params.PanelID + ' #lblGuarantor').css("display", "inline");
    },
    //// -------------- End Guarantor ------------

    CalculateAge: function (ev) {
        //Patient_Demographic.FillAge($('#' + demographicDetail.params.PanelID + ' #dtpDOB').val()).done(function (response) {
        //    if (response.status != false) {
        //        $('#' + demographicDetail.params.PanelID + ' #txtAge').val(response.ActualAge);
        //    }
        //});
        var ControliD = ev;
        setTimeout(function () {
            if ($(ControliD).val().length == 10 && utility.isValidDate($(ControliD).val()) && Date.parse($(ControliD).val())) {
                if (new Date($(ControliD).val()) > new Date()) {
                    $(ControliD).val('');
                }
                else {
                    Patient_Demographic.FillAge($(ControliD).val()).done(function (response) {
                        if (response.status != false) {
                            $('#' + demographicDetail.params.PanelID + ' #txtAge').val(response.ActualAge);
                        } else {
                            $('#' + demographicDetail.params.PanelID + ' #txtAge').val('');
                        }
                    });
                }
            } else {
                $('#' + demographicDetail.params.PanelID + ' #txtAge').val('');
                $(ControliD).val('');

            }
            if ($(ControliD).val() == "") {
                var ControlName = $(ControliD).attr("name");
                if ($('#' + demographicDetail.params["PanelID"] + ' #frmdemographicDetail').data('bootstrapValidator') != null && typeof $('#' + demographicDetail.params["PanelID"] + ' #frmdemographicDetail').data('bootstrapValidator') != 'undefined') {

                    $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail').bootstrapValidator('revalidateField', ControlName);
                }
            }
        }, 300);
        //var now = new Date();
        //var current_year = now.getFullYear();
        //var curr_month = now.getMonth() + 1;

        //var birthDate = new Date($("#dtpDOB").val());
        //var year_diff = current_year - birthDate.getFullYear();
        //var curr_month = curr_month - (birthDate.getMonth() + 1);

        //alert(year_diff + "  " + curr_month);
    },

    UnLoad: function () {

        var dfd = new $.Deferred();
        Patient_Insurance.bIsFirstLoad = true;

        demographicDetail.imagedata = "";
        demographicDetail.filetype = "";
        demographicDetail.filename = "";
        // demographicDetail.scannerjson = "";
        utility.UnLoadDialog('frmdemographicDetail', function () {
            if (demographicDetail.params.PanelID == "pnlBatchDocuments #pnldemographicDetail" || demographicDetail.params.PanelID == "pnlPatientDocument #pnldemographicDetail" || demographicDetail.params.PanelID == "Patient_Search #pnldemographicDetail") {
                Patient_Document.params = [];
                Patient_Document.params = params;
            }
            else if (demographicDetail.params.PanelID != "pnlDocumentViewer #pnldemographicDetail") {
                Patient_Document.params = [];
            }
            if (demographicDetail.params.GrandParentCtrl == "appointmentDetail") {
                ScanPrivilige = false;
                OCRPrivilige = false;
                UnloadActionPan(demographicDetail.params.ParentCtrl);
                appointmentDetail.FillPatientInfoFromSearch(demographicDetail.params.PatientId, null);
            }
            //uploadImage.RemovePicture(demographicDetail.params.PatientId);
            if (demographicDetail.params != null && demographicDetail.params.ParentCtrl && demographicDetail.params.ParentCtrl != "") {
                ScanPrivilige = false;
                OCRPrivilige = false;
                UnloadActionPan(demographicDetail.params.ParentCtrl);
                dfd.resolve();
            }
            else {
                Patient_Demographic.FillDemographic(demographicDetail.params.patientID).done(function (response) {
                    if (response.status != false) {
                        var demographic_detail = JSON.parse(response.DemographicFill_JSON);

                        if (demographic_detail.Sex != "" && demographic_detail.MaritalStatus != "" && demographic_detail.Ethnicity != "" && demographic_detail.Race != "" && demographic_detail.Address1 != "" && demographic_detail.City != "" && demographic_detail.State != "" && demographic_detail.Zip != "" && demographic_detail.HomeTel != "" && demographic_detail.PrefLanguage != "") {
                            UnloadActionPan(null, "demographicDetail");

                            if (demographicDetail.params.GrandParent == "PMSScheduler") {
                                demographicDetail.params.model.PatientSex = demographic_detail.Sex;
                                demographicDetail.params.model.PatientAddress1 = demographic_detail.Address1;
                                demographicDetail.params.model.PatientCity = demographic_detail.City;
                                demographicDetail.params.model.PatientState = demographic_detail.State;
                                demographicDetail.params.model.PatientZip = demographic_detail.Zip;
                                demographicDetail.params.model.PatientEthnicityIds = demographic_detail.Ethnicity;
                                demographicDetail.params.model.PatientRaceIds = demographic_detail.strRaceIds;
                                demographicDetail.params.model.PatientMaritalStatus = demographic_detail.MaritalStatus;
                                demographicDetail.params.model.PatientHomeTel = demographic_detail.HomeTel;
                                PMSScheduler.LoadClinicalNote(demographicDetail.params.model, demographicDetail.params.IsView);
                            }
                        }
                        else {
                            UnloadActionPan(null, "demographicDetail");
                            if (typeof DefaultMenuSelected != "undefined" && DefaultMenuSelected != "MDVisionBilling") {
                                SelectTab('mstrTabPatient', 'false');
                            }
                        }
                    }
                    dfd.resolve();
                });
            }

        }, function () {

            if (demographicDetail.params.PanelID == "pnlBatchDocuments #pnldemographicDetail" || demographicDetail.params.PanelID == "pnlPatientDocument #pnldemographicDetail" || demographicDetail.params.PanelID == "Patient_Search #pnldemographicDetail") {
                Patient_Document.params = [];
                Patient_Document.params = params;
            }
            else if (demographicDetail.params.PanelID && demographicDetail.params.PanelID != "pnlDocumentViewer #pnldemographicDetail") {
                // PMS - 4550
                if (demographicDetail.params.PanelID == "pnlDemographic") {
                    if (Patient_Document.params.PanelID && Patient_Document.params.PanelID != "pnlBatchDocuments")
                    Patient_Document.params.PanelID = "pnlPatientDocument"
                }
                else {
                    Patient_Document.params = [];
                }
               
            }            
            if (demographicDetail.params.GrandParentCtrl == "appointmentDetail") {
                ScanPrivilige = false;
                OCRPrivilige = false;
                UnloadActionPan(demographicDetail.params.ParentCtrl);
                appointmentDetail.FillPatientInfoFromSearch(demographicDetail.params.PatientId, null);
            }

            if (demographicDetail.params != null && demographicDetail.params.ParentCtrl && demographicDetail.params.ParentCtrl != "") {
                ScanPrivilige = false;
                demographicDetail.OCRPrivilige = false;
                UnloadActionPan(demographicDetail.params.ParentCtrl);
                dfd.resolve();
            }
            else {
                Patient_Demographic.FillDemographic(demographicDetail.params.patientID).done(function (response) {
                    if (response.status != false) {
                        var demographic_detail = JSON.parse(response.DemographicFill_JSON);

                        if (demographic_detail.Sex != "" && demographic_detail.MaritalStatus != "" && demographic_detail.Ethnicity != "" && demographic_detail.Race != "" && demographic_detail.Address1 != "" && demographic_detail.City != "" && demographic_detail.State != "" && demographic_detail.Zip != "" && demographic_detail.HomeTel != "" && demographic_detail.PrefLanguage != "") {
                            UnloadActionPan(null, "demographicDetail");
                           if (demographicDetail.params.TabID == "patTabInsurance") {
                                Patient_Insurance.params.PanelID = "pnlPatientInsurance";
                            }
                            if (demographicDetail.params.GrandParent == "PMSScheduler") {
                                demographicDetail.params.model.PatientSex = demographic_detail.Sex;
                                demographicDetail.params.model.PatientAddress1 = demographic_detail.Address1;
                                demographicDetail.params.model.PatientCity = demographic_detail.City;
                                demographicDetail.params.model.PatientState = demographic_detail.State;
                                demographicDetail.params.model.PatientZip = demographic_detail.Zip;
                                demographicDetail.params.model.PatientEthnicityIds = demographic_detail.Ethnicity;
                                demographicDetail.params.model.PatientRaceIds = demographic_detail.strRaceIds;
                                demographicDetail.params.model.PatientMaritalStatus = demographic_detail.MaritalStatus;
                                demographicDetail.params.model.PatientHomeTel = demographic_detail.HomeTel;
                                PMSScheduler.LoadClinicalNote(demographicDetail.params.model, demographicDetail.params.IsView);
                            }
                        }
                        else {
                            UnloadActionPan(null, "demographicDetail");
                            if (typeof DefaultMenuSelected != "undefined" && DefaultMenuSelected != "MDVisionBilling") {
                                SelectTab('mstrTabPatient', 'false');
                            }
                        }
                    }
                    dfd.resolve();
                });
            }
        });
        return dfd;
    },

    changeGender: function (ev) {
        var imageSrc = $('#' + demographicDetail.params.PanelID + ' #imgPatient').attr('src');
        if (imageSrc == 'Content/images/default_male_profile.gif' || imageSrc == 'Content/images/default_female_profile.gif') {
            switch ($(ev.selectedOptions).text()) {
                case "Male":
                    $('#' + demographicDetail.params.PanelID + ' #imgPatient').attr('src', 'Content/images/default_male_profile.gif');
                    break;
                case "Female":
                    $('#' + demographicDetail.params.PanelID + ' #imgPatient').attr('src', 'Content/images/default_female_profile.gif');
                    break
            }
        }
    },
    setImageSource: function (sourceString, RefFill) {

        var PanelId = demographicDetail.params.PanelID;
        if (RefFill == "patTabDemographic")
            PanelId = Patient_Demographic.params.PanelID;
        if (Document_Scan.params["IsFromIfram"] == true) {
            PanelId = Document_Scan.params["PanelID"];
            $(window.parent.document).find('#' + PanelId + '  #imgPatient').attr('src', sourceString);
        } else {
            $('#' + PanelId + '  #imgPatient').attr('src', sourceString);
        }

        // set banner image
        if (RefFill == "patTabDemographic" && Document_Scan.params["IsFromUploadImage"] == true)
            $(window.parent.document).find('#imgPatientProfile').attr('src', sourceString);
        else if (RefFill == "patTabDemographic")
            $('#imgPatientProfile').attr('src', sourceString);

    },

    //-------------------- Functions to open patient controls from patient Banner (IrFan) --------------------

    OpenInsurance: function () {
        var params = [];
        //params["PanelID"] = demographicDetail.params["PanelID"];
        params["patientID"] = demographicDetail.params.patientID;
        params["PatBanner"] = demographicDetail.params.PatBanner;
        params["RefCtrl"] = "frmdemographicDetail";
        params["ParentCtrl"] = 'demographicDetail';
        LoadActionPan('Patient_Insurance', params);

        $('#frmdemographicDetail').addClass('disableAll');
    },

    OpenDocuments: function () {
        var params = [];
        //params["PanelID"] = demographicDetail.params["PanelID"];
        params["PatientId"] = demographicDetail.params.patientID;
        params.PatientFullName = demographicDetail.params.PatientFullName;
        params["AccountNo"] = $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail #txtAccountNo').val();
        params["PatBanner"] = demographicDetail.params.PatBanner;
        params["GridPatientDocument"] = "dgvPatientDocument";
        params["GridRevDocument"] = "dgvPatRevDocument";
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["RefCtrl"] = "frmdemographicDetail";
        params["ParentCtrl"] = 'demographicDetail';
        LoadActionPan('Patient_Document', params);

        //$('#frmdemographicDetail').addClass('disableAll');
    },
    OpenClinicalSummary: function () {
        var params = [];

        params["patientID"] = demographicDetail.params.patientID;
        params["PatBanner"] = demographicDetail.params.PatBanner;
        params["RefCtrl"] = "frmdemographicDetail";
        params["ParentCtrl"] = 'demographicDetail';
        LoadActionPan('Clinical_FaceSheet', params);
    },

    BindGuarantor: function () {
        var Ctrl = $('#' + demographicDetail.params.PanelID + ' #txtGuarantor');
        var hfCtrl = $('#' + demographicDetail.params.PanelID + " #hfGuarantor");
        var func = function () { return utility.GetGuarontorArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    PatientPortal: function () {

        AppPrivileges.GetFormPrivileges("Patient Portal Account", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientId"] = demographicDetail.params.patientID;
                params["ParentCtrl"] = "demographicDetail";
                params["PatientFirstName"] = $('#' + demographicDetail.params.PanelID + ' #txtFirstName').val();
                params["PatientLastName"] = $('#' + demographicDetail.params.PanelID + ' #txtLastName').val();
                params["PatientDOB"] = $('#' + demographicDetail.params.PanelID + ' #dtpDOB').val();
                params["PatientEmail"] = $('#' + demographicDetail.params.PanelID + ' #txtEmail').val();
                params["PatientPortalStatus"] = $('#' + demographicDetail.params.PanelID + ' #hfPatientPortalStatus').val();
                //---------------------
                params["ProviderFName"] = demographicDetail.ProviderFName;
                params["ProviderLName"] = demographicDetail.ProviderLName;
                params["FacilityAddress"] = demographicDetail.FacilityAddress;
                params["FacilityCity"] = demographicDetail.FacilityCity;
                params["FacilityState"] = demographicDetail.FacilityState;
                params["FacilityZip"] = demographicDetail.FacilityZip;
                params["FacilityZipExt"] = demographicDetail.FacilityZipExt;
                params["FacilityPhone"] = demographicDetail.FacilityPhone;
                params["ZipCode"] = $('#' + demographicDetail.params.PanelID + ' #txtZip').val();

                //---------------------
                if ($('#' + demographicDetail.params.PanelID + ' #hfPatientPortalStatus').val() == "0")
                    params["mode"] = "Add";
                else if ($('#' + demographicDetail.params.PanelID + ' #hfPatientPortalStatus').val() == "1")
                    params["mode"] = "Edit";

                LoadActionPan('Patient_AccountManager', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    GetAccountManagerPriviliges: function () {

        AppPrivileges.GetFormPrivileges("Patient Portal Account", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                $('#' + demographicDetail.params.PanelID + ' #btnPatientPortal').removeAttr("style");

            } else {
                $('#' + demographicDetail.params.PanelID + ' #btnPatientPortal').css("display", "none");
            }
        });

    },

    //Start || 16 April, 2016 || ZeeshanAK || Changes for DOC 34 - Break The Glass 
    OpenRestrictUser: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["patientID"] = demographicDetail.params.patientID;
        params["ParentCtrl"] = "demographicDetail";
        LoadActionPan('Restrict_User', params);
    },


    //
    OpenDocumentScan: function () {
        AppPrivileges.GetFormPrivileges("Documents", "SCAN", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var pid = $("#frmdemographicDetail #hfPractice").val();
                practiceDetail.DemographicPractice(pid).done(function (response) {
                    if (response.status != false) {
                        var medication_detail = JSON.parse(response.PracticeFill_JSON);
                        ScanPrivilige = medication_detail.chkScan;
                        OCRPrivilige = medication_detail.chkOCR;
                        if (demographicDetail.params.PanelID == "Patient_Search #pnldemographicDetail"); {
                            demographicDetail.ScanPrivilige = medication_detail.chkScan;
                            demographicDetail.OCRPrivilige = medication_detail.chkOCR;
                        }
                    }
                    else {
                        ScanPrivilige = false;
                        OCRPrivilige = false;
                        if (demographicDetail.params.PanelID == "Patient_Search #pnldemographicDetail"); {
                            demographicDetail.ScanPrivilige = false;
                            demographicDetail.OCRPrivilige = false;
                        }
                    }
                    if (ScanPrivilige == "True") {
                        var param = [];
                        var PanelID = null;
                        if (demographicDetail.params["PanelID"] != "pnlPatientInsurance")
                            PanelID = demographicDetail.params["PanelID"];
                        if (demographicDetail.params.ParentCtrl == "demographicDetail")
                            param["ParentCtrl"] = 'Patient_Insurance';


                        else
                            param["ParentCtrl"] = 'demographicDetail';
                        param["RefCtrl"] = "patientDemographic";
                        //param["RefFill"] = "patientDemographic";
                        param["IsScanFrom"] = "true";
                        param["mode"] = "Scan";
                        param["PracticeId"] = pid;
                        setDefaultValuesForScanCanvas(500, 260);
                        LoadActionPan('Document_Scanner', param, PanelID);
                        //LoadActionPan('Document_Scan', param, PanelID);
                    }
                    else {
                        utility.DisplayMessages("Practice is not selected or doesnot have priviliges to Scan. Please contact your administrator.", 2);
                    }
                });
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    fillDrivingLicenseDataWithAPI: function (frontImage, backImage, QuickPatientRef) {

        var frontImageBlob;
        var backImagerBlob;
        var frmImageToProcess = new FormData();
        frontImageBlob = utility.basr64URLtoBlob(frontImage);
        backImagerBlob = utility.basr64URLtoBlob(backImage);

        frmImageToProcess.append("backImage", frontImageBlob);
        frmImageToProcess.append("frontImage", backImagerBlob);

        //Accesing the web service..
        $.ajax({
            type: "POST",
            url: "https://cssnwebservices.com/CSSNService/CardProcessor/ProcessDLDuplexEX/0/true/-1/true/true/true/0/150/false/false",
            data: frmImageToProcess,
            cache: false,
            contentType: 'application/octet-stream; charset=utf-8;',
            dataType: "json",
            processData: false,
            beforeSend: function (xhr) {
                BackgroundLoaderShow(true);
                xhr.setRequestHeader("Authorization", "LicenseKey " + $.base64.encode("B3E4DBF19BED"));
            },
            success: function (data) {
                try {
                    //Converts data to string before parsing
                    var parsedDLCardData = JSON.stringify(data);
                    parsedDLCardData = $.parseJSON(parsedDLCardData);
                    if (parsedDLCardData.Zip) {
                        demographicDetail.setpatientImage(data, QuickPatientRef);
                    }

                    //Display error description if there are errors.
                    if (parsedDLCardData.ResponseCodeAuthorization < 0) {
                        utility.DisplayMessages("CSSN Error Code: " + parsedDLCardData.ResponseMessageAuthorization, 3);
                    }
                    else if (parsedDLCardData.WebResponseCode < 1) {
                        utility.DisplayMessages("CSSN Error Code: " + parsedDLCardData.WebResponseDescription, 3);
                    }
                    else {

                        if (parsedDLCardData.Zip) {
                            if (QuickPatientRef == "QuickpatientDemographic") {
                                var myBase64 = utility.arrayBufferToBase64(data.FaceImage);
                                Patient_DemographicQuick.fillQuickDemographicWithCardInfo(parsedDLCardData, myBase64);

                            } else {

                                demographicDetail.fillDemographicWithCardInfo(parsedDLCardData, QuickPatientRef);

                            }
                        }
                        else {
                            BackgroundLoaderShow(false);
                            utility.DisplayMessages("Please scan card properly.", 3);
                        }


                    }
                } catch (ex) {

                    console.log(ex);
                    BackgroundLoaderShow(false);
                    utility.DisplayMessages("Please scan card properly.", 3);

                }
            },
            error: function (e) {
                BackgroundLoaderShow(false);
                utility.DisplayMessages("Error: " + e, 3);
            }
        });
    },

    fillDemographicWithCardInfo: function (parsedDLCardData, RefFill) {

        var PanelId = demographicDetail.params.PanelID;
        if (RefFill == "patTabDemographic")
            PanelId = Patient_Demographic.params.PanelID;

        var NameMiddle = "";
        if (parsedDLCardData.NameMiddle && parsedDLCardData.NameMiddle.length > 0) {
            NameMiddle = parsedDLCardData.NameMiddle.substring(0, 1);
        }
        var dob = utility.formatDate(parsedDLCardData.DateOfBirth4);

        var PatientSex = 0;
        var PatientSex_ = "";
        if (parsedDLCardData.Sex == "M") {
            PatientSex = 0;
            PatientSex_ = "Male";
            //  $("#" + PanelId + " #divdemographicDetailPicture #imgPatient").attr("src", "Content/images/default_male_profile.gif");
        }
        else if (parsedDLCardData.Sex == "F") {
            PatientSex = 1;
            PatientSex_ = "Female";
            //  $("#" + PanelId + " #divdemographicDetailPicture #imgPatient").attr("src", "Content/images/default_female_profile.gif");
        }
        else {
            PatientSex = 2;
        }

        var strzipCode = "";
        var strZipExt = "";
        if (parsedDLCardData.Zip.indexOf("-") > -1) {
            var strzip = parsedDLCardData.Zip.split("-");
            strzipCode = strzip[0];
            if (strzip.length > 1) {
                strZipExt = strzip[1];
            }
        }
        else {
            strzipCode = parsedDLCardData.Zip.substring(0, 5);
            strZipExt = parsedDLCardData.Zip.substring(5, parsedDLCardData.Zip.length);
        }

        var patientAge = "";
        try {
            var birthdate = new Date(dob);
            var cur = new Date();
            var diff = cur - birthdate; // This is the difference in milliseconds
            patientAge = ", " + Math.floor(diff / 31557600000); // Div
        } catch (ex) {
            console.log(ex);
            patientAge = "";
        }
        var patientAddress = ((parsedDLCardData.Address != null && parsedDLCardData.Address != "") ? ", " + parsedDLCardData.Address + ', ' : "")
                + ((parsedDLCardData.City != null && parsedDLCardData.City != "") ? parsedDLCardData.City + ', ' : "")
                + ((parsedDLCardData.State != null && parsedDLCardData.State != "") ? parsedDLCardData.State + ' ' : "")
                + ((strzipCode != null && strzipCode != "" && strzipCode != undefined) ? strzipCode + ' ' : "");


        if (Document_Scan.params["IsFromIfram"] == true) {


            PanelId = Document_Scan.params["PanelID"];

            $(window.parent.document).find('#' + PanelId + " #txtLastName").val(parsedDLCardData.NameLast);
            //$('#' + PanelId + " #ddlSuffix").val(parsedDLCardData.NameSuffix);
            $(window.parent.document).find('#' + PanelId + " #txtFirstName").val(parsedDLCardData.NameFirst);


            $(window.parent.document).find('#' + PanelId + " #txtMiddleInitial").val(NameMiddle);
            //$('#' + PanelId + " #dtpDOB").val(utility.formatDate(parsedDLCardData.DateOfBirth4));

            //$('#' + PanelId + " #dtpDOB").val(utility.formatDate(parsedDLCardData.DateOfBirth4));

            $(window.parent.document).find('#' + PanelId + " #dtpDOB").datepicker("setDate", dob);
            $(window.parent.document).find('#' + PanelId + " #dtpDOB").trigger('change');
            //$('#' + PanelId + " #dtpDOB").bootstrapzzValidator('revalidateField', 'DOB');



            $(window.parent.document).find('#' + PanelId + " #ddlSex").val(PatientSex);
            $(window.parent.document).find('#' + PanelId + " #txtAddress1").val(parsedDLCardData.Address);
            $(window.parent.document).find('#' + PanelId + " #txtAddress2").val(parsedDLCardData.Address2);
            $(window.parent.document).find('#' + PanelId + " #txtCity").val(parsedDLCardData.City);
            $(window.parent.document).find('#' + PanelId + " #txtState").val(parsedDLCardData.State);



            $(window.parent.document).find('#' + PanelId + " #txtZip").val(strzipCode);
            $(window.parent.document).find('#' + PanelId + " #txtZipExt").val(strZipExt);


            // update Patient banner
            if (RefFill == "patTabDemographic") {
                $(window.parent.document).find("#banner_PatientName").html(parsedDLCardData.NameLast + ", " + parsedDLCardData.NameFirst + " " + NameMiddle
                    + " " + "<button class='btn btn-success btn-xs mr-xs' id='btnDemographicLabel' hidden='' type='button' onclick='OpenDemographicLabel()'>Demographic Label</button>");
                $(window.parent.document).find("#banner_PatientDOB").html(dob);
                $(window.parent.document).find("#banner_PatientSex").html(PatientSex_);
                $(window.parent.document).find("#banner_PatientAddress").html(", " + parsedDLCardData.Address);
                $(window.parent.document).find("#banner_PatientCityStateZip").html(", " + parsedDLCardData.City + ", " + parsedDLCardData.State + " " + parsedDLCardData.strzipCode);

                var title_ = "DOB: " + dob + patientAge + ", " + PatientSex_ + ", " + $(window.parent.document).find("#banner_PatientInfoAndAddress").attr("MaritalStatus");
                $(window.parent.document).find("#banner_PatientInfoAndAddress").attr('data-original-title', title_ + patientAddress)
            }

            if (Document_Scan.params["IsFromUploadImage"] == true)
                $(window.parent.document).find('#pnlUploadImage #btnClose').click(); 
            else
                $(window.parent.document).find('#pnlDocument_Scanner #btnIframeScannerClose').click();

            
        }
        else {

            $('#' + PanelId + " #txtLastName").val(parsedDLCardData.NameLast);
            //$('#' + PanelId + " #ddlSuffix").val(parsedDLCardData.NameSuffix);
            $('#' + PanelId + " #txtFirstName").val(parsedDLCardData.NameFirst);


            $('#' + PanelId + " #txtMiddleInitial").val(NameMiddle);
            //$('#' + PanelId + " #dtpDOB").val(utility.formatDate(parsedDLCardData.DateOfBirth4));

            //$('#' + PanelId + " #dtpDOB").val(utility.formatDate(parsedDLCardData.DateOfBirth4));

            $('#' + PanelId + " #dtpDOB").datepicker("setDate", dob);
            $('#' + PanelId + " #dtpDOB").trigger('change');
            //$('#' + PanelId + " #dtpDOB").bootstrapzzValidator('revalidateField', 'DOB');



            $('#' + PanelId + " #ddlSex").val(PatientSex);
            $('#' + PanelId + " #txtAddress1").val(parsedDLCardData.Address);
            $('#' + PanelId + " #txtAddress2").val(parsedDLCardData.Address2);
            $('#' + PanelId + " #txtCity").val(parsedDLCardData.City);
            $('#' + PanelId + " #txtState").val(parsedDLCardData.State);



            $('#' + PanelId + " #txtZip").val(strzipCode);
            $('#' + PanelId + " #txtZipExt").val(strZipExt);


            // update Patient banner
            if (RefFill == "patTabDemographic") {
                $("#banner_PatientName").html(parsedDLCardData.NameLast + ", " + parsedDLCardData.NameFirst + " " + NameMiddle
                    + " " + "<button class='btn btn-success btn-xs mr-xs' id='btnDemographicLabel' hidden='' type='button' onclick='OpenDemographicLabel()'>Demographic Label</button>");
                $("#banner_PatientDOB").html(dob);
                $("#banner_PatientSex").html(PatientSex_);
                $("#banner_PatientAddress").html(", " + parsedDLCardData.Address);
                $("#banner_PatientCityStateZip").html(", " + parsedDLCardData.City + ", " + parsedDLCardData.State + " " + parsedDLCardData.strzipCode);

                var title_ = "DOB: " + dob + patientAge + ", " + PatientSex_ + ", " + $("#banner_PatientInfoAndAddress").attr("MaritalStatus");
                $("#banner_PatientInfoAndAddress").attr('data-original-title', title_ + patientAddress)
            }

            // $('#' + PanelId + " #chkBadAddress").prop("checked", !parsedDLCardData.IsAddressVerified);
            if (RefFill == "patTabDemographic" || RefFill == "demographicDetail")
                uploadImage.UnLoad();
            else
                Document_Scan.UnLoadTab();

            BackgroundLoaderShow(false);

        }
    },

    setpatientImage: function (data, RefFill) {

        //var str = String.fromCharCode.apply(null, data.FaceImage);
        // var base64FaceImage = goog.crypt.base64.encodeByteArray(faceImage);

        //var myBase64 = "data:image/jpg;base64," + btoa(str).replace(/.{76}(?=.)/g, '$&\n');
        var myBase64 = utility.arrayBufferToBase64(data.FaceImage);

        demographicDetail.setImageSource(myBase64, RefFill);
        //document.getElementById("faceImage").style.display = "inline";
        //$("#face-image").attr("src", "data:image/jpg;base64," + base64FaceImage);


        //var reader = new window.FileReader();
        //reader.readAsDataURL(blob);
        //reader.onloadend = function () {
        //    base64data = reader.result;
        //    console.log(base64data);
        //}

    },

    ScanOCRPriviliges: function (PracticeId) {

        practiceDetail.DemographicPractice(PracticeId).done(function (response) {
            if (response.status != false) {
                var medication_detail = JSON.parse(response.PracticeFill_JSON);
                ScanPrivilige = medication_detail.chkScan;
                OCRPrivilige = medication_detail.chkOCR;
                if (Patient_Demographic.params.PanelID == "pnlDemographic"); {
                    Patient_Demographic.ScanPrivilige = medication_detail.chkScan;
                    Patient_Demographic.OCRPrivilige = medication_detail.chkOCR;
                }
            }
            else {
                ScanPrivilige = false;
                OCRPrivilige = false;
                if (Patient_Demographic.params.PanelID == "pnlDemographic"); {
                    Patient_Demographic.ScanPrivilige = false;
                    Patient_Demographic.OCRPrivilige = false;
                }
            }
        });
    },

    ///Referrals -- Added by Humaira Yousaf ///

    hideShowTextbox: function () {
        if ($("#pnldemographicDetail #ddlHearFrom").val() == 1) {
            var bootstrapValidator = $('#frmdemographicDetail').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('RefProviderReferral', true);

            var bootstrapValidator = $('#frmdemographicDetail').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('Status', true);

            demographicDetail.getPatientReferral();
            $("#pnldemographicDetail #incomingReferral").removeClass('hidden');
        }
        else {
            $("#pnldemographicDetail #incomingReferral").addClass('hidden');

            var bootstrapValidator = $('#frmdemographicDetail').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('RefProviderReferral', false);

            var bootstrapValidator = $('#frmdemographicDetail').data('bootstrapValidator');
            bootstrapValidator.enableFieldValidators('Status', false);

        }

        if ($("#pnldemographicDetail #ddlHearFrom").val() == 10) {
            $("#pnldemographicDetail #divOtherText").removeClass('hidden');
        }
        else {
            $("#pnldemographicDetail #txtOtherText").val('');
            $("#pnldemographicDetail #divOtherText").addClass('hidden');
        }
        if ($('#' + demographicDetail.params.PanelID + " #hfReferralID").val())
            $('#' + demographicDetail.params.PanelID + " #divIncomingReferralInfo #btnScanResult,#btnViewAttachment").removeClass("disableAll");
        else
            $('#' + demographicDetail.params.PanelID + " #divIncomingReferralInfo #btnScanResult,#btnViewAttachment").addClass("disableAll");
    },
    OpenRefProviderReferral: function () {
        var params = [];
        params["ReferringProviderIdReferral"] = "-1";
        params["FromAdmin"] = "0";
        params["RefForm"] = "frmdemographicDetail";
        params["IsOptional"] = false;
        params["RefCtrl"] = "txtRefProviderReferral";
        params["ParentCtrl"] = "demographicDetail";
        params["RefCtrlHidden"] = "hfRefProviderReferral";
        LoadActionPan('Admin_ReferringProvider', params);
    },
    OpenProviderReferral: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmdemographicDetail";
        params["ProviderIdReferral"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "demographicDetail";
        params["RefCtrl"] = "txtProviderReferral";
        params["RefCtrlHidden"] = "hfProviderReferral";
        LoadActionPan('Admin_Provider', params);
    },
    BindLanguages: function () {
        var Ctrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #ddlPrefLanguage");
        var hfCtrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfLanguages");
        var func = function () { return utility.GetPreferedLanguagesArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindCountries: function () {
        var Ctrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #ddlCountry");
        var hfCtrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfCountry");
        var func = function () { return utility.GetCountriesArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindFacility: function () {
        var Ctrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtFacility");
        var hfCtrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfFacility");
        var func = function () { return utility.GetFacilityDescriptionArray(Ctrl.val()) };
        var onSelect = function (dataItem) {
            $("#pnldemographicDetail #txtPractice").val(dataItem.Practice);
            $("#pnldemographicDetail #hfPractice").val(dataItem.PracticeId);
        }
        var onChange = function (valid) {
            if (Ctrl.val() != "" && $("#pnldemographicDetail #txtPractice").val() == "") {
                if (!valid) {
                    $("#pnldemographicDetail #txtPractice").val("");
                    $("#pnldemographicDetail #hfPractice").val("");
                }
            }
            if ($(Ctrl).val() == "") {
                $("#pnldemographicDetail #txtPractice").val("");
                $("#pnldemographicDetail #hfPractice").val("");
                ScanPrivilige = false;
                OCRPrivilige = false;
            }
        }
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },
    BindProvider: function (isFullName, shortName) {
        var Ctrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtProvider");
        var hfCtrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindPCPProvider: function () {
        var Ctrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtPCP");
        var hfCtrl = $("#pnldemographicDetail #hfPCP");
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindRefProviderTo: function (isFullName, shortName) {
        var Ctrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #txtProviderReferral");
        var hfCtrl = $("#" + demographicDetail.params.PanelID + " #frmdemographicDetail #hfProviderReferral");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindRefProvider: function () {
        var Ctrl = $('#pnldemographicDetail #txtRefProvider');
        var hfCtrl = $("#pnldemographicDetail #hfRefProvider");
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindRefProviderReferral: function () {
        var Ctrl = $('#pnldemographicDetail #txtRefProviderReferral');
        var hfCtrl = $("#pnldemographicDetail #hfRefProviderReferral");
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    OpenAssignee: function () {
        CacheManager.BindCodes('GetUsers', true).done(function (result) {
            var Ctrl = $("#pnldemographicDetail #frmdemographicDetail #txtAssignee");
            var hfCtrl = $("#pnldemographicDetail #frmdemographicDetail  #hfAssignee");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Users, null, hfCtrl);
        });
    },



    getPatientReferral: function () {
        utility.CreateDatePicker(demographicDetail.params.PanelID + " #dtDate", function () {
        }, true);

        $('#' + demographicDetail.params.PanelID + " #tmTime").timepicker({
            defaultTime: new Date()
        });

        if (demographicDetail.params.mode == "Add") {
            $('#' + demographicDetail.params.PanelID + " #menuAttach").remove();
            $('#' + demographicDetail.params.PanelID + " #attachmentDiv").html('<a id="btnViewAttachment" href="#" class="dropdown-toggle btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="">View Attachment</a>');
            $('#' + demographicDetail.params.PanelID + " #btnScanResult,#btnViewAttachment").addClass("disableAll");
        }
        else if (demographicDetail.params.mode == "Edit") {

            CacheManager.BindDropDownsByID('#' + demographicDetail.params.PanelID + " #ddlInsurance", 'GetPatientInsurance', true, demographicDetail.params.patientID).done(function () {
                if ($('#' + demographicDetail.params.PanelID).find("#ddlInsurance option").length > 1) {
                    $($('#' + demographicDetail.params.PanelID).find("#ddlInsurance option")[1]).prop('selected', true);
                }
            });

            $('#' + demographicDetail.params.PanelID + " #divIncomingReferralInfo #attachDiv").append('<ul id="menuAttach" class="dropdown-menu" aria-labelledby="btnScanResult">' +
                     '<li><a href="#" onclick="demographicDetail.documentScan()">Scan</a></li><li><a href="#" onclick="demographicDetail.documentImport()">Upload</a></li></ul>');

            $('#' + demographicDetail.params.PanelID + " #divIncomingReferralInfo #attachmentDiv").html('<a id="btnViewAttachment" href="#" class="dropdown-toggle btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="demographicDetail.loadAttachments()">View Attachment</a><ul id="menuViewAttachment" class="dropdown-menu" aria-labelledby="btnViewAttachment"></ul>');

            demographicDetail.fillPatientReferralInfo();
        }

    },
    fillPatientReferralInfo: function (params) {

        var patientId = demographicDetail.params.patientID;
        var referralId = $('#' + demographicDetail.params.PanelID + " #hfReferralID").val();

        var dfd = new $.Deferred();

        demographicDetail.fillPatientReferralInfo_DbCall(patientId, referralId).done(function (response) {
            if (response.status != false) {

                var self = $("#frmdemographicDetail");
                var referralJSON = JSON.parse(response.ReferralListLoad_JSON);
                utility.bindMyJSONByName(true, referralJSON, false, self).done(function () {
                    if ($('#' + demographicDetail.params.PanelID + " #hfReferralID").val())
                        $('#' + demographicDetail.params.PanelID + " #divIncomingReferralInfo #btnScanResult,#btnViewAttachment").removeClass("disableAll");
                    else
                        $('#' + demographicDetail.params.PanelID + " #divIncomingReferralInfo #btnScanResult,#btnViewAttachment").addClass("disableAll");
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                return dfd.promise();
            }
        });

        return dfd.promise();
    },
    fillPatientReferralInfo_DbCall: function (patientId, referralId) {

        var objData = new Object();
        objData["PatientID"] = patientId;
        objData["ReferralId"] = referralId;
        objData["CommandType"] = "fill_patient_referral";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },

    documentScan: function () {
        AppPrivileges.GetFormPrivileges("Documents", "SCAN", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var param = [];
                param["mode"] = "Scan";
                param["RefCtrl"] = "IncomingReferral";
                param['ReferralId'] = $('#' + demographicDetail.params.PanelID + " #hfReferralID").val();
                param['RefModuleName'] = "Incoming Referral";
                param['TransitionId'] = $('#' + demographicDetail.params.PanelID + " #hfReferralID").val();
                param['patientID'] = demographicDetail.params.patientID;
                param["ParentCtrl"] = 'demographicDetail'; //demographicDetail.params["TabID"];
                LoadActionPan('Document_Scan', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    documentImport: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];

                var AccountNo = $('#' + demographicDetail.params.PanelID + ' #txtAccountNo').val();
                var PatientId = demographicDetail.params.patientID;

                var Firstname = $('#' + demographicDetail.params.PanelID + ' #txtFirstName').val();
                var Lastname = $('#' + demographicDetail.params.PanelID + ' #txtLastName').val() + ",";
                var MiddleInitial = $('#' + demographicDetail.params.PanelID + ' #txtMiddleInitial').val();
                var PatientName = Lastname + " " + Firstname + " " + MiddleInitial;

                params["AccountNo"] = AccountNo;
                params["patientId"] = demographicDetail.params.patientID;
                params["RefCtrl"] = "IncomingReferral";
                params['ReferralId'] = $('#' + demographicDetail.params.PanelID + " #hfReferralID").val();
                params['RefModuleName'] = "Incoming Referral";
                params['TransitionId'] = $('#' + demographicDetail.params.PanelID + " #hfReferralID").val();
                params["FromAdmin"] = "0";
                params["mode"] = "Add";
                params["ParentCtrl"] = "demographicDetail";// demographicDetail.params["TabID"];
                params["PatientName"] = PatientName;
                LoadActionPan('Document_Import', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    loadAttachments: function (controlName, radiologyOrderId, resultId, tableId) {

        demographicDetail.loadAttachments_DbCall().done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var ulAttachment = null;

                if (controlName == null)
                    ulAttachment = $('#' + demographicDetail.params.PanelID + " #menuViewAttachment");
                else {
                    var control = eval(controlName.trim());
                    if (tableId != null) {

                        ulAttachment = $('#' + control.params.PanelID + " #" + tableId.trim() + " tr td").find('div.dropdown').find("#menuViewAttachment" + resultId);
                        if ($('#' + control.params.PanelID + " #" + tableId.trim()).parent() != null) {
                            $('#' + control.params.PanelID + " #" + tableId.trim()).parent().removeClass('Of-a');
                        }
                    }
                }
                $(ulAttachment).empty();
                if (response.AttachmentCount > 0) {
                    var attachments = JSON.parse(response.AttachmentLoad_JSON);

                    $(attachments).each(function (index, item) {
                        if (controlName == null)
                            $(ulAttachment).append('<li><a href="#" id="' + item.PatDocId + '" onclick="demographicDetail.showAttachment(\'' + item.PatDocId + '\',event)">' + item.ModifiedOn + '</a></li>');
                        else {
                            var onClick = controlName.trim() + ".showAttachment";
                            $(ulAttachment).append('<li><a href="#" id="' + item.PatDocId + '" onclick="' + onClick + '(\'' + item.PatDocId + '\',event,this);">' + item.ModifiedOn + '</a></li>');
                        }
                    });

                }
                else {
                    $(ulAttachment).append('<li><a href="#">No Attachment Found</a></li>');
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    loadAttachments_DbCall: function () {

        var objData = {};
        objData["TransitionId"] = $('#' + demographicDetail.params.PanelID + " #hfReferralID").val();
        objData["RefModuleName"] = "Incoming Referral";
        objData["PatientId"] = $("div#PatientProfile #hfPatientId").val();

        objData["commandType"] = "load_attachments";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },

    showAttachment: function (PatDocID, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientID"] = demographicDetail.params.patientID;
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = 'demographicDetail';
                params["isFromShowAttachment"] = true;               
                LoadActionPan('Document_Viewer', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    //end referrals ///
    IsDeathSelected: function (ddlStatus) {

        if ($(ddlStatus).val() == 2) {
            if ($("#pnldemographicDetail #frmdemographicDetail #dtpDateOfDeath").val() == "" || $("pnldemographicDetail #frmdemographicDetail #dtpDateOfDeath").val() == null) {
                $("#pnldemographicDetail #frmdemographicDetail #divDateOfDeath").removeClass('hidden');

            }
        }
        else {
            $("#pnldemographicDetail #frmdemographicDetail #divDateOfDeath").addClass('hidden');
            $("#pnldemographicDetail #frmdemographicDetail #dtpDateOfDeath").val("");
            $("#pnldemographicDetail #frmdemographicDetail #txtCauseOfDeath").val("");

        }
        if ($(ddlStatus).val() == 0) {
            $("#pnldemographicDetail #frmdemographicDetail #dvresion").show();
        }
        else {
            $("#pnldemographicDetail #frmdemographicDetail #dvresion").hide();
        }
    },
    DeathDateChange: function () {
        var bootstrapValidator = $('#pnldemographicDetail #frmdemographicDetail').data('bootstrapValidator');
        if (bootstrapValidator)
        { bootstrapValidator.revalidateField("DateOfDeath"); }
    },
    RevalidatedCityZipCode:function(){
        var formValidation = $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail').data("bootstrapValidator");
        formValidation.enableFieldValidators('State', true);
        formValidation.enableFieldValidators('City', true);
    },
}