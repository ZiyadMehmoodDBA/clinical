MobileAppRequest = {
    params: [],
    bIsFirstLoad: true,
    IsHospitalizationHxFirstLoad: true,
    isFamilyHXFirstLoad: true,
    IsSurgicalHxFirstLoad: true,
    IsMedicalHxFirstLoad: true,
    IsSocialHxFirstLoad: true,
    IsEmergencyContactsFirstLoad: true,
    IsInsuranceFirstLoad: true,
    IsschedulerFirstLoad: true,
    ApproveMessage: "Record has been approved successfully!",
    DiscardMessage: "Record has been discarded successfully!",
    PatientNotFound: "Please Insert Demographic Record first as no Patient has been created Yet!",
    Load: function (parameters) {

        MobileAppRequest.params = parameters;
        MobileAppRequest.IsHospitalizationHxFirstLoad = true;
        MobileAppRequest.IsMedicalHxFirstLoad = true;
        MobileAppRequest.IsSurgicalHxFirstLoad = true;
        MobileAppRequest.IsSocialHxFirstLoad = true;
        MobileAppRequest.IsEmergencyContactsFirstLoad = true;
        MobileAppRequest.IsInsuranceFirstLoad = true;
        MobileAppRequest.IsEverythingChecked = false;
        MobileAppRequest.isFamilyHXFirstLoad = true;
        MobileAppRequest.IsAppointmentFirstLoad = true;

        var self = $('#pnlMobileAppRequest #frmMobileAppRequest');
        $('#' + MobileAppRequest.params["PanelID"] + ' section[name="MobileApp_Module"]').hide();
        MobileAppRequest.ShowHideTabs();


        AppPrivileges.GetFormPrivileges("Live Requests", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                self.loadDropDowns(true).done(function () {
                    MobileAppRequest.LoadFirstTabOfFirstSection(MobileAppRequest.params.DataSection, MobileAppRequest.params.DataSubSection);

                    MobileAppRequest.LoadAllAutocomplete();

                    MobileAppRequest.ShowHideButtons();

                    $('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ShowHideTabs: function () {

        MobileAppRequest.SectionButtonsCount = 0;
        $('#' + MobileAppRequest.params["PanelID"] + ' Button[name="MobileApp_Button"]').hide();
        var tabs = MobileAppRequest.params.DataSection.split(',');
        $.each(tabs, function (index, Item) {
            if (Item != "PatientAppointments") {
                $('#pnlMobileAppRequest' + ' #btn' + Item).show();
                MobileAppRequest.SectionButtonsCount++;
            }
        })
        MobileAppRequest.SectionButtonsChecked = 0;
    },

    UnLoad: function () {
        if (MobileAppRequest.params != null && MobileAppRequest.params.ParentCtrl != "") {
            UnloadActionPan(MobileAppRequest.params.ParentCtrl, "MobileAppRequest");
            DashBoard.SearchDashBoardDataChangeRequest(null, null, null, null, 'search')
        }
        else
            UnloadActionPan(null, 'MobileAppRequest');
    },

    DemographicUpdate: function () {
        var strMessage = "";
        var self = $('#pnlMobileAppRequest');
        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                MobileAppRequest.UpdateDemographic(MobileAppRequest.params.Demographic_Data).done(function (response) {
                    if (response.status != false) {
                        MobileAppRequest.params.patientID = response.PatientId;
                        utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                        // Hide Demographic Button
                        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                        var Index = subtabs.indexOf('Patients');
                        subtabs.splice(Index, 1);
                        MobileAppRequest.params.DataSubSection = subtabs.join(',')
                        MobileAppRequest.ShowDemographicsButtons();
                        //to update count and grid on dashboardpatientchanges
                        //  DashBoard.DashBoardPatientChangesSearch(null, null, null);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },//edit end

    LoadPatientDemogrphicNative: function () {
        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $('#pnlMobileAppRequest  #frmMobileAppRequest').resetAllControls();
                MobileAppRequest.FillPatientInfo(MobileAppRequest.params).done(function () {
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FillPatientInfo: function (params) {
        var dfd = new $.Deferred();
        MobileAppRequest.FillDemographicNative(MobileAppRequest.params.patientID, MobileAppRequest.params.DimmyPatientId, MobileAppRequest.params.Status).done(function (response) {
            if (response.status != false) {
                if ($('#pnlMobileAppRequest #btnPatients').find('i').length == 0) {
                    $('#pnlMobileAppRequest #btnPatients').append('<i class="fa fa-check pull-left"></i>');
                    MobileAppRequest.TickDemographicsButton();
                }

                $('#pnlMobileAppRequest  #frmMobileAppRequest #divEmergencyContactList').hide();
                $('#pnlMobileAppRequest #frmMobileAppRequest #divPatientPreferences').hide();
                $('#' + MobileAppRequest.params["PanelID"] + ' #divDemographics').show();
                var demographic_detail = JSON.parse(response.DemographicFill_JSON);

                var self = $('#pnlMobileAppRequest #frmMobileAppRequest #divDemographics');
                utility.bindMyJSONByName(true, demographic_detail, false, self).done(function () {
                });

                if (demographic_detail.Gender == "Other") {
                    $('#pnlMobileAppRequest #frmMobileAppRequest #divDemographics #ddlSex').find('option[value=2]').prop("selected", true);
                }

                if (typeof demographic_detail.strRaceIds != 'undefined' && demographic_detail.strRaceIds != '' && demographic_detail.strRaceIds != null) {
                    $('#' + MobileAppRequest.params.PanelID + " #ddlPatientRace").val(demographic_detail.strRaceIds.split(','));
                    $('#' + MobileAppRequest.params.PanelID + " #ddlPatientRace").multiselect("refresh");
                    $('#' + MobileAppRequest.params.PanelID + " #ddlPatientRace").multiselect({
                        includeSelectAllOption: false,
                        enableFiltering: false,
                        enableCaseInsensitiveFiltering: false,
                        maxHeight: 500
                    });
                }
                if (typeof demographic_detail.EthnicityId != 'undefined' && demographic_detail.EthnicityId != '' && demographic_detail.EthnicityId != null) {
                    $('#' + MobileAppRequest.params.PanelID + " #ddlEthnicity").val(demographic_detail.EthnicityId.split(','));
                    $('#' + MobileAppRequest.params.PanelID + " #ddlEthnicity").multiselect("refresh");
                    $('#' + MobileAppRequest.params.PanelID + " #ddlEthnicity").multiselect({
                        includeSelectAllOption: false,
                        enableFiltering: false,
                        enableCaseInsensitiveFiltering: false,
                        maxHeight: 500
                    });
                }

                if (demographic_detail.imgPatient != undefined && demographic_detail.imgPatient != "") {
                    $("#pnlMobileAppRequest #imgPatient").attr("src", demographic_detail.imgPatient);
                    //$("#PatientProfile #imgPatientProfile").attr("src", demographic_detail.imgPatient);
                    $("#pnlMobileAppRequest #imgPatient").css({ "cursor": "pointer" });
                }
                else {
                    if (demographic_detail.Gender == "Female") {
                        $("#pnlMobileAppRequest #divDemographicPicture #imgPatient").attr("src", "Content/images/default_female_profile.gif");
                        $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_female_profile.gif");
                    }
                    else if (demographic_detail.Gender == "Male" || demographic_detail.Gender == "Other") {
                        $("#pnlMobileAppRequest #divDemographicPicture #imgPatient").attr("src", "Content/images/default_male_profile.gif");
                        $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_male_profile.gif");
                    }
                    $("#pnlDemographic #divDemographicPicture #imgPatient").css({ "cursor": "default" });
                }
                MobileAppRequest.HighLightChangedFields(demographic_detail);
                MobileAppRequest.params.Demographic_Data = demographic_detail;
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                return dfd.promise();
            }
        });

        return dfd.promise();
    },
    FillDemographicNative: function (PatientID, DimmyPatientId, Status) {
        var objData = new Object();
        objData["PatientID"] = PatientID;
        objData["DimmyPatientId"] = DimmyPatientId;
        objData["RequestStatus"] = Status;
        objData["CommandType"] = "fill_patient_demographic_native";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographicNative");
    },
    BindJSON: function (demographic_detail) {
        var self = $('#pnlMobileAppRequest #frmMobileAppRequest');
        utility.bindMyJSONByName(true, demographic_detail, false, self).done(function () {
        });
    },

    LoadAllAutocomplete: function () {
        CacheManager.BindCodes('GetLanguages', false).done(function (result) {
            $("#frmMobileAppRequest #ddlPrefLanguage").autocomplete({
                autoFocus: true,
                source: PrefLanguages, // pass an array
                select: function (event, ui) {

                    setTimeout(function () {
                        $("#pnlMobileAppRequest #ddlPrefLanguage").attr("PrefLanguageId", ui.item.id); // add the selected id

                    }, 100);
                }
            });
        });
        $('#pnlMobileAppRequest #frmMobileAppRequest #dtpDOB').datepicker('setEndDate', new Date());
    },
    HighLightChangedFields: function (demographic_detail) {
        var ChangedFields = demographic_detail.lstChangedColumns;
        $.each(ChangedFields, function (index, Item) {
            $("#pnlMobileAppRequest #frmMobileAppRequest #divDemographics #" + Item.columnName.toLowerCase()).removeClass('hidden');
        });
    },

    UpdateDemographic: function (DemographicData) {

        var objData = DemographicData;
        objData["PatientID"] = MobileAppRequest.params.patientID;
        objData["CommandType"] = "update_patient_demographicnative";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographicNative");
    },
    ShowPanel: function (tabMobileApp, currentButton) {
        //$('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();
        // $('#' + MobileAppRequest.params["PanelID"] + ' #div' + tabMobileApp).show();
        if (!MobileAppRequest.params.patientID && tabMobileApp != "Demographics") {
            utility.DisplayMessages(MobileAppRequest.PatientNotFound, 3);
            return false;
        }

        switch (tabMobileApp) {
            case "EmergencyContactList":
                MobileAppRequest.LoadPatientEmergencyContactsNative();
                break;
            case "PatientPreferences":
                MobileAppRequest.FillPatientPreferences();
                break;
            case "SectionInsurance":
                MobileAppRequest.LoadPatientInsurances();
                break;
            case "Demographics":
                MobileAppRequest.LoadPatientDemogrphicNative();
                break;
            case "HospitalizationHx":
                MobileAppRequest.LoadHospitalizationHxDiseases();
                break;
            case "SurgicalHx":
                MobileAppRequest.LoadSurgicalHxDiseases();
                break;
            case "SocialHxWrapper":
                MobileAppRequest.LoadSocialHx();
                break;
            case "MedicalHx":
                MobileAppRequest.LoadMedicalHxDiseases();
                break;
            case "BirthHx":
                MobileAppRequest.ShowBirthHistoryButtons();
                break;
            case "BirthHx_General":
                MobileAppRequest.LoadBirthHxByName('BirthHx_General', currentButton);
                break;
            case "BirthHx_MaternalDelivery":
                MobileAppRequest.LoadBirthHxByName('BirthHx_MaternalDelivery', currentButton);
                break;
            case "BirthHx_NewBorn":
                MobileAppRequest.LoadBirthHxByName('BirthHx_Newborn', currentButton);
                break;
            case "ProgramUpdates":
                CCMProgramUpdate.Load(CCM_Patient_Hub.params, 1);
                $("#tabCCM_ a").removeClass('active');
                $("#tabCCMProgramUpdate").addClass('active');
                break;
            case "FamilyHx":
                MobileAppRequest.LoadFamilyHxDiseases();
                break;
            default:
                //  utility.DisplayMessages("Please select CCM module", 2);
        }
    },
    ShowSection: function (Section) {
        $('#' + MobileAppRequest.params["PanelID"] + ' section[name="MobileApp_Module"]').hide();
        $('#pnlMobileAppRequest' + ' #section' + Section).show();
        //  $('#' + MobileAppRequest.params["PanelID"] + ' #sectionInsurance').show();

        switch (Section) {
            case "Insurance":
                MobileAppRequest.LoadPatientInsurances();
                break;
            case "Demographics":
                MobileAppRequest.ShowDemographicsButtons();
                break;
            case "History":
                MobileAppRequest.ShowHistoryButtons();
                break;
                //case "Appointment":
                //    MobileAppRequest.LoadPatientAppointment();
                //    break;
            default:
        }

    },
    ShowDemographicsButtons: function () {
        $('#pnlMobileAppRequest' + ' #sectionDemographics').show();
        $('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();
        $('#' + MobileAppRequest.params["PanelID"] + ' Button[name="Demographics"]').addClass("hidden").removeClass("btn-pipe");

        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
        var count = 0;
        $.each(subtabs, function (index, Item) {
            // $('#pnlMobileAppRequest' + '#sectionDemographics' + ' #btnPatients').show();
            if (Item == "Patients" || Item == "EmergencyContacts" || Item == "PatientPreferences") {
                $('#pnlMobileAppRequest' + ' #btn' + Item).removeClass("hidden").addClass("btn-pipe");
                count++;
            }
        })
        if (subtabs.indexOf('Patients') != -1) {

            MobileAppRequest.LoadPatientDemogrphicNative();
        }
        else if (subtabs.indexOf('EmergencyContacts') != -1) {


            MobileAppRequest.LoadPatientEmergencyContactsNative();
        }
        else if (subtabs.indexOf('PatientPreferences') != -1) {


            MobileAppRequest.FillPatientPreferences();
        }

        if (count == 0) {
            $('#pnlMobileAppRequest' + ' #sectionDemographics').hide();
            $('#pnlMobileAppRequest' + ' #btnDemographics').hide();

            // Remove Section Demographics

            var Sections = MobileAppRequest.params.DataSection.split(',');
            var SectionIndex = Sections.indexOf('Demographics');
            Sections.splice(SectionIndex, 1);
            MobileAppRequest.params.DataSection = Sections.join(',');

            // Load Next first avaliable tab of first section
            MobileAppRequest.LoadFirstTabOfFirstSection(MobileAppRequest.params.DataSection, MobileAppRequest.params.DataSubSection);


        }

    },
    ShowHistoryButtons: function () {
        $('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();
        $('#pnlMobileAppRequest' + ' #sectionHistory').show();
        $('#' + MobileAppRequest.params["PanelID"] + ' Button[name="History"]').addClass("hidden").removeClass("btn-pipe");

        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
        var count = 0;
        $.each(subtabs, function (index, Item) {

            // $('#pnlMobileAppRequest' + '#sectionDemographics' + ' #btnPatients').show();

            if (Item == "BirthHx" || Item == "HospitalizationHx" || Item == "MedicalHx" || Item == "SurgicalHx" || Item == "FamilyHx" || Item == "SocialHx") {
                $('#pnlMobileAppRequest' + ' #btn' + Item).removeClass("hidden").addClass("btn-pipe");
                count++;
            }


        })
        if (subtabs.indexOf('SocialHx') != -1) {

            MobileAppRequest.LoadSocialHx();
        }
        else if (subtabs.indexOf('MedicalHx') != -1) {


            MobileAppRequest.LoadMedicalHxDiseases();
        }
        else if (subtabs.indexOf('FamilyHx') != -1) {


            MobileAppRequest.LoadFamilyHxDiseases();
        }
        else if (subtabs.indexOf('SurgicalHx') != -1) {


            MobileAppRequest.LoadSurgicalHxDiseases();
        }
        else if (subtabs.indexOf('BirthHx') != -1) {
            MobileAppRequest.ShowBirthHistoryButtons();
        }
        else if (subtabs.indexOf('HospitalizationHx') != -1) {


            MobileAppRequest.LoadHospitalizationHxDiseases();
        }
        if (count == 0) {
            $('#pnlMobileAppRequest' + ' #sectionHistory').hide();
            $('#pnlMobileAppRequest' + ' #btnHistory').hide();

            // Remove Section History

            var Sections = MobileAppRequest.params.DataSection.split(',');
            var SectionIndex = Sections.indexOf('History');
            Sections.splice(SectionIndex, 1);
            MobileAppRequest.params.DataSection = Sections.join(',');

            // Load Next first avaliable tab of first section
            MobileAppRequest.LoadFirstTabOfFirstSection(MobileAppRequest.params.DataSection, MobileAppRequest.params.DataSubSection);

        }



    },
    ShowBirthHistoryButtons: function () {
        $('#' + MobileAppRequest.params["PanelID"] + ' Button[name="BirthHx"]').hide();
        $('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();
        $("#pnlMobileAppRequest #frmMobileAppRequest #divBirthHx").show();
        $('#pnlMobileAppRequest #btnBirthHx .fa-check').remove();
        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
        var count = 0;
        var isFirstBirthHx = true;
        $.each(subtabs, function (index, Item) {
            if (Item == "BirthHx_General" || Item == "BirthHx_MaternalDelivery" || Item == "BirthHx_Newborn") {
                {
                    $('#pnlMobileAppRequest' + ' #btn' + Item).show();
                    if (isFirstBirthHx) {
                        $('#pnlMobileAppRequest' + ' #btn' + Item).click();
                        isFirstBirthHx = false;
                    }
                }
                count++;
            }
        });
        if (count == 0) {
            $("#pnlMobileAppRequest #frmMobileAppRequest #divBirthHx").hide();

            $('#pnlMobileAppRequest' + ' #btnBirthHx').hide();

            var subtabs = MobileAppRequest.params.DataSubSection.split(',');
            var Index = subtabs.indexOf('BirthHx');
            subtabs.splice(Index, 1);
            MobileAppRequest.params.DataSubSection = subtabs.join(',');
            MobileAppRequest.ShowHistoryButtons();

        }
    },
    ShowHideButtons: function () {
        var status = MobileAppRequest.params.Status;
        if (status == "Approved") {
            $("#pnlMobileAppRequest #btnEmergencyContactApprove").hide();
            $("#pnlMobileAppRequest #btnPatPreferencesApprove").hide();
            $("#pnlMobileAppRequest #btnPatInsuranceApprove").hide();
            $("#pnlMobileAppRequest #btnDemographicApprove").hide();

            $("#pnlMobileAppRequest #btnDemographicDiscard").hide();
            $("#pnlMobileAppRequest #btnPatPreferencesDiscard").hide();
            $("#pnlMobileAppRequest #btnPatInsuranceDiscard").hide();
            $("#pnlMobileAppRequest #btnEmergencyContactDiscard").hide();
            // History Buttons 
            $("#pnlMobileAppRequest #btnHospitalizationHxApprove").hide();
            $("#pnlMobileAppRequest #btnMedicalHxApprove").hide();
            $("#pnlMobileAppRequest #btnSurgicalHxApprove").hide();

            $("#pnlMobileAppRequest #btnHospitalizationHxDiscard").hide();
            $("#pnlMobileAppRequest #btnMedicalHxDiscard").hide();
            $("#pnlMobileAppRequest #btnSurgicalHxDiscard").hide();

            $("#pnlMobileAppRequest #btngeneralHxApprove").hide();
            $("#pnlMobileAppRequest #btngeneralHxDiscard").hide();
            $("#pnlMobileAppRequest #btnmaternalHxApprove").hide();
            $("#pnlMobileAppRequest #btnmaternalHxDiscard").hide();
            $("#pnlMobileAppRequest #btnNewBornHxApprove").hide();
            $("#pnlMobileAppRequest #btnNewBornHxDiscard").hide();

            $("#pnlMobileAppRequest #btnFamilyHxApprove").hide();
            $("#pnlMobileAppRequest #btnFamilyHxDiscard").hide();

            $("#pnlMobileAppRequest #divSocialHxWrapper #btnSocialHxApprove").hide();
            $("#pnlMobileAppRequest #divSocialHxWrapper #btnSocialHxDiscard").hide();

            $("#pnlMobileAppRequest #btnAppointmentApprove").hide();
            $("#pnlMobileAppRequest #btnAppointmentDiscard").hide();

        }
        else if (status == "Discard") {
            $("#pnlMobileAppRequest #btnDemographicDiscard").hide();
            $("#pnlMobileAppRequest #btnPatPreferencesDiscard").hide();
            $("#pnlMobileAppRequest #btnPatInsuranceDiscard").hide();
            $("#pnlMobileAppRequest #btnEmergencyContactDiscard").hide();

            $("#pnlMobileAppRequest #btnHospitalizationHxDiscard").hide();
            $("#pnlMobileAppRequest #btnMedicalHxDiscard").hide();
            $("#pnlMobileAppRequest #btnSurgicalHxDiscard").hide();
            $("#pnlMobileAppRequest #btnFamilyHxDiscard").hide();

            $("#pnlMobileAppRequest #btngeneralHxDiscard").hide();
            $("#pnlMobileAppRequest #btnmaternalHxDiscard").hide();
            $("#pnlMobileAppRequest #btnNewBornHxDiscard").hide();

            $("#pnlMobileAppRequest #divSocialHxWrapper #btnSocialHxDiscard").hide();
            $("#pnlMobileAppRequest #btnAppointmentDiscard").hide();

        }
    },

    PatientEmergencyContactsLoadNative: function (PatientID, RequestStatus) {
        var data = "PatientID=" + PatientID + "&RequestStatus=" + RequestStatus + "&pageNumber=" + 1 + "&rowsPerPage=" + 15;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "MobileAppRequest", "LOAD_EMERGENCYCONTACTS_NATIVE");
    },
    EmergencyContactsLoadByIdNative: function (EmergencyContactId, PatientID, RequestStatus) {
        //if (PageNumber == null) {
        //    PageNumber = 1;
        //}
        //if (RowsPerPage == null) {
        //    RowsPerPage = 15;
        //}
        var data = "PatientID=" + PatientID + "&EmergencyContactId=" + EmergencyContactId + "&RequestStatus=" + RequestStatus;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "MobileAppRequest", "FILL_EMERGENCYCONTACT_NATIVE");

        //var objData = new Object();
        //objData["PatientID"] = PatientID;
        //objData["CommandType"] = "load_patient_emergency_contacts_native";
        //var data = JSON.stringify(objData);
        //return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographicNative");
    },
    LoadPatientEmergencyContactsNative: function () {


        //   $('#pnlMobileAppRequest  #frmMobileAppRequest').resetAllControls();
        $('#pnlMobileAppRequest #frmMobileAppRequest #divPatientPreferences').hide();
        $('#pnlMobileAppRequest  #frmMobileAppRequest #divDemographics').hide();
        $('#pnlMobileAppRequest  #frmMobileAppRequest #divEmergencyContact').hide();
        $("#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContactList").show();
        if (MobileAppRequest.IsEmergencyContactsFirstLoad == true) {
            MobileAppRequest.PatientEmergencyContactsLoadNative(MobileAppRequest.params.patientID, MobileAppRequest.params.Status).done(function (response) {
                if (response.status != false) {

                    var EmergencyContactsCount = JSON.parse(response.EmergencyContactsCount);
                    MobileAppRequest.EmergencyContactsList = response.EmergencyContactsLoad_JSON;

                    $("#pnlMobileAppRequest #ECList").html('');
                    var ContactsCount = 0;

                    MobileAppRequest.EmergencyContactsChecked = 0;
                    if (EmergencyContactsCount > 0) {
                        $.each(MobileAppRequest.EmergencyContactsList, function (index, value) {
                            if (value.ContactId != "") {
                                var ButtonId = index + 1;
                                var $Button = $('<Button class="btn btn-default btn-sm tab_space">' + "Emergency Contact " + ButtonId + '</button>');

                                $Button.attr("id", "btnEmergencyContact" + value.ContactId + "");
                                $Button.attr("onclick", "MobileAppRequest.LoadEmergencyContactById(" + value.ContactId + ");");



                                $("#pnlMobileAppRequest #ECList").append($Button);

                                ContactsCount++;
                                // This Check will tick the first Disease button and load the disease 

                                if (ContactsCount == 1) {

                                    MobileAppRequest.FirstContactId = value.ContactId;

                                }
                            }
                        });
                        MobileAppRequest.IsEmergencyContactsFirstLoad = false;
                        MobileAppRequest.EmergencyContactsCount = ContactsCount;


                        MobileAppRequest.LoadEmergencyContactById(MobileAppRequest.FirstContactId);

                    }
                    else {
                        $("#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContactList").hide();
                        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                        var Index = subtabs.indexOf('EmergencyContacts');
                        subtabs.splice(Index, 1);
                        MobileAppRequest.params.DataSubSection = subtabs.join(',');
                        MobileAppRequest.ShowDemographicsButtons();
                    }
                }
                else {

                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            var buttonsCount = $('#pnlMobileAppRequest #ECList').find("button").length;

            if (buttonsCount > 0) {
                $('#pnlMobileAppRequest #ECList').find("button")[0].click();
            }
            else {
                $("#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContactList").hide();
                var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                var Index = subtabs.indexOf('EmergencyContacts');
                subtabs.splice(Index, 1);
                MobileAppRequest.params.DataSubSection = subtabs.join(',');
                MobileAppRequest.ShowDemographicsButtons();
            }

        }

    },
    LoadEmergencyContactById: function (EmergencyContactId) {
        $('#pnlMobileAppRequest  #frmMobileAppRequest').resetAllControls();
        MobileAppRequest.EmergencyContactsLoadByIdNative(EmergencyContactId, MobileAppRequest.params.patientID, MobileAppRequest.params.Status).done(function (response) {
            if (response.status != false) {
                $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact').find('label').removeClass('changed-field');
                $('#pnlMobileAppRequest #frmMobileAppRequest #divPatientPreferences').hide();
                $('#pnlMobileAppRequest  #frmMobileAppRequest #divDemographics').hide();
                $('#pnlMobileAppRequest  #frmMobileAppRequest #divEmergencyContact').show();
                var ContactDetail = JSON.parse(response.EmergencyContactFill_JSON);
                var self = $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact');
                utility.bindMyJSONByName(true, ContactDetail, false, self).done(function () {

                    if (ContactDetail.IsPrimary == "1")
                        $('#pnlMobileAppRequest  #frmMobileAppRequest #divEmergencyContact #txtIsPrimary').val('Yes')
                    else
                        $('#pnlMobileAppRequest  #frmMobileAppRequest #divEmergencyContact #txtIsPrimary').val('No')
                    // High Ligt Changed Columns.
                    if ($('#pnlMobileAppRequest #btnEmergencyContact' + EmergencyContactId).find('i').length == 0) {
                        MobileAppRequest.EmergencyContactsChecked++;
                        $('#pnlMobileAppRequest #btnEmergencyContact' + EmergencyContactId).append('<i class="fa fa-check pull-left"></i>');
                        if (MobileAppRequest.EmergencyContactsCount == MobileAppRequest.EmergencyContactsChecked) {
                            if ($('#pnlMobileAppRequest #btnEmergencyContacts').find('i').length == 0)
                                $('#pnlMobileAppRequest #btnEmergencyContacts').append('<i class="fa fa-check pull-left"></i>');
                            MobileAppRequest.TickDemographicsButton();
                        }
                    }
                });

                MobileAppRequest.params.ContactId = ContactDetail.ContactId;
                MobileAppRequest.params.EmergencyContactData = ContactDetail;
                MobileAppRequest.HighLightChangedFieldsOfEmergencyContact(MobileAppRequest.params.EmergencyContactData)
            }
            else {

                utility.DisplayMessages(response.Message, 3);
            }
        });



    },
    InsertUpdateEmergencyContact: function () {
        var strMessage = "";
        var self = $('#pnlMobileAppRequest');

        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (MobileAppRequest.params.ContactId < 0) {
                    MobileAppRequest.EmergencyContactSave(MobileAppRequest.params.EmergencyContactData).done(function (response) {
                        if (response.status != false) {
                            $("#pnlMobileAppRequest #ECList").find("#btnEmergencyContact" + MobileAppRequest.params.ContactId).remove();
                            utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                            MobileAppRequest.LoadPatientEmergencyContactsNative();
                            //to update count and grid on dashboardpatientchanges
                            //  DashBoard.DashBoardPatientChangesSearch(null, null, null);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    MobileAppRequest.EmergencyContactUpdate(MobileAppRequest.params.EmergencyContactData).done(function (response) {
                        if (response.status != false) {
                            //  MobileAppRequest.IsEmergencyContactsFirstLoad = true;
                            // Remove the contactid and corresponedent Button from HTML and backend
                            $("#pnlMobileAppRequest #ECList").find("#btnEmergencyContact" + MobileAppRequest.params.ContactId).remove();
                            //  MobileAppRequest.EmergencyContactsCount--;
                            utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                            MobileAppRequest.LoadPatientEmergencyContactsNative();
                            //to update count and grid on dashboardpatientchanges
                            //  DashBoard.DashBoardPatientChangesSearch(null, null, null);
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
    },
    EmergencyContactSave: function (EmergencyContactData) {
        var data = "EmergencyContactData=" + JSON.stringify(EmergencyContactData);
        //  var data = "EmergencyContactData=" + EmergencyContactData ;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "MobileAppRequest", "SAVE_EMERGENCYCONTACT");
    },
    EmergencyContactUpdate: function (EmergencyContactData) {
        // var data = JSON.stringify("EmergencyContactData=" + EmergencyContactData);
        var data = "EmergencyContactData=" + JSON.stringify(EmergencyContactData);
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "MobileAppRequest", "UPDATE_EMERGENCYCONTACT");
    },
    FillPatientPreferences: function () {
        $('#pnlMobileAppRequest  #frmMobileAppRequest').resetAllControls();
        MobileAppRequest.PatientPreferencesfill(MobileAppRequest.params.patientID, MobileAppRequest.params.Status).done(function (response) {
            if (response.status != false) {

                if ($('#pnlMobileAppRequest #btnPatientPreferences').find('i').length == 0) {
                    $('#pnlMobileAppRequest #btnPatientPreferences').append('<i class="fa fa-check pull-left"></i>');
                    MobileAppRequest.TickDemographicsButton();
                }

                $('#pnlMobileAppRequest  #frmMobileAppRequest #divEmergencyContactList').hide();
                $('#pnlMobileAppRequest  #frmMobileAppRequest #divDemographics').hide();
                $('#pnlMobileAppRequest #frmMobileAppRequest #divPatientPreferences').show();
                var PatientPreferences_Detail = response.PatientPreferences_JSON;
                // Bind patient Preference JSON...
                var self = $('#pnlMobileAppRequest #frmMobileAppRequest #divPatientPreferences');
                utility.bindMyJSONByName(true, PatientPreferences_Detail, false, self).done(function () {
                    MobileAppRequest.params.PatientPreferencesData = PatientPreferences_Detail;
                    MobileAppRequest.HighLightChangedFieldsOfPatientPreferences(PatientPreferences_Detail);
                    // High Ligt Changed Columns.
                });
            }
        });
    },
    PatientPreferencesfill: function (PatientID, Status) {
        //if (PageNumber == null) {
        //    PageNumber = 1;
        //}
        //if (RowsPerPage == null) {
        //    RowsPerPage = 15;
        //}
        var data = "PatientID=" + PatientID + "&RequestStatus=" + Status;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "MOBILEAPPREQUEST", "FILL_PATIENT_PREFERENCES");

        //var objData = new Object();
        //objData["PatientID"] = PatientID;
        //objData["CommandType"] = "load_patient_emergency_contacts_native";
        //var data = JSON.stringify(objData);
        //return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographicNative");
    },
    UpdatePatientPreferences: function () {
        var strMessage = "";


        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (MobileAppRequest.params.patientID > 0) {
                    MobileAppRequest.PatientPreferencesUpdate(MobileAppRequest.params.PatientPreferencesData).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                            // Hide Demographic Button
                            var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                            var Index = subtabs.indexOf('PatientPreferences');
                            subtabs.splice(Index, 1);
                            MobileAppRequest.params.DataSubSection = subtabs.join(',')
                            MobileAppRequest.ShowDemographicsButtons();

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
    },
    PatientPreferencesUpdate: function (PatientPreferencesData) {
        // var data = JSON.stringify("EmergencyContactData=" + EmergencyContactData);
        var data = "PatientPreferencesData=" + JSON.stringify(PatientPreferencesData);
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "MobileAppRequest", "UPDATE_PATIENT_PREFERENCES");
    },
    HighLightChangedFieldsOfEmergencyContact: function (EmergencyContactData) {
        var ChangedFields = EmergencyContactData.lstChangedColumns;
        $.each(ChangedFields, function (index, Item) {
            $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #' + Item.columnName.toLowerCase()).removeClass('hidden');
            //if (Item.columnName.toLowerCase() == "lastname") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblLastName').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "firstname") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblFirstName').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "mi") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblMI').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "dob") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblDOB').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "emailaddress") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblEmail').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "relationshipid") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblRelation').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "address1") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblAddress1').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "address2") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblAddress2').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "city") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblCity').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "state") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblState').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "zipcode") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblZipCode').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "zipext") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #txtZipExt').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "homephone") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblHomePhone').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "workphone") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblWorkPhone').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "workphext") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblWorkPhExt').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "cellno") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblCellNo').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "faxno") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblFaxNo').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "isprimary") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divEmergencyContact #lblIsPrimary').addClass('changed-field');
            //}
        });
    },
    HighLightChangedFieldsOfPatientPreferences: function (PatientPreferenceData) {
        var ChangedFields = PatientPreferenceData.lstChangedColumns;
        $.each(ChangedFields, function (index, Item) {
            $('#pnlMobileAppRequest #frmMobileAppRequest #divPatientPreferences #' + Item.columnName.toLowerCase()).removeClass('hidden');
            //if (Item.columnName.toLowerCase() == "prefcommunicationid") {
            // $('#pnlMobileAppRequest #frmMobileAppRequest #divPatientPreferences #lblPrefCommunicationId').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "scndprefcommunicationid") {
            // $('#pnlMobileAppRequest #frmMobileAppRequest #divPatientPreferences #lblScndPrefCommunicationId').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "communicationoptout") {
            // $('#pnlMobileAppRequest #frmMobileAppRequest #divPatientPreferences #lblCommunicationOptout').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "patientstatement") {
            // $('#pnlMobileAppRequest #frmMobileAppRequest #divPatientPreferences #lblPatientStatement').addClass('changed-field');
            //}
        });
    },
    HighLightChangedFieldsOfInsurance: function (PatientInsuranceData) {
        var ChangedFields = PatientInsuranceData.lstChangedColumns;
        $.each(ChangedFields, function (index, Item) {
            $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance #' + Item.columnName.toLowerCase()).removeClass('hidden');
            //if (Item.columnName.toLowerCase() == "firstname") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance #lblSubscriberFirstName').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "mi") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance #lblSubscriberMI').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "lastname") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance #lblSubscriberLastName').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "subscriberid") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance #lblSubscriberId').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "groupid") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance #lblGroupId').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "dob") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance #lblSubscriberDOB').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "relationshipid") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance #lblRelationShipId').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "insuranceplanid") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance #lblInsurancePlanName').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "gender") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance #lblGender').addClass('changed-field');
            //}
            //if (Item.columnName.toLowerCase() == "isactive") {
            //    $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance #lblIsActive').addClass('changed-field');
            //}
        });
    },

    LoadPatientAppointment: function () {

        if (MobileAppRequest.IsAppointmentFirstLoad == true) {
            $('#pnlMobileAppRequest  #frmMobileAppRequest').resetAllControls();
            MobileAppRequest.PatientAppointmentLoad(MobileAppRequest.params.patientID, MobileAppRequest.params.Status).done(function (response) {
                if (response.status != false) {
                    if ($('#pnlMobileAppRequest #btnPatientAppointments').find('i').length == 0)
                        $('#pnlMobileAppRequest #btnPatientAppointments').append('<i class="fa fa-check pull-left"></i>');
                    MobileAppRequest.PatientAppointmentChecked = 0;
                    var self = $('#pnlMobileAppRequest #frmMobileAppRequest #sectionAppointment');
                    self.loadDropDowns(true).done(function () {
                        $('#' + MobileAppRequest.params["PanelID"] + ' #divAppointment').show();
                        var PatientAppointmentDetail = JSON.parse(response.PatientAppointmentDetail_JSON)
                        MobileAppRequest.BindJSON(PatientAppointmentDetail);
                        MobileAppRequest.params.PatientAppointmentData = PatientAppointmentDetail;
                        MobileAppRequest.SectionButtonsChecked++;
                        MobileAppRequest.ShowButtonsOfApprovalAllAndDiscardAll();
                    })
                }
                else {

                    utility.DisplayMessages(response.Message, 2);
                }
            });
        }
    },
    PatientAppointmentLoad: function (PatientID, RequestStatus) {
        var data = "PatientID=" + PatientID + "&RequestStatus=" + RequestStatus;
        return MDVisionService.defaultService(data, "MobileAppRequest", "FILL_PATIENT_APPOINTMENT");

    },
    InsertPatientAppointment: function (PatientId, AppointmentId, Approve) {
        var strMessage = "";
        //var self = $('#pnlMobileAppRequest');
        //MobileAppRequest.params.PatientAppointmentData
        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (Approve == false) {
                    if ($('body').find('#modal-from-dom-RejectionReason').length < 1) {
                        MobileAppRequest.RejectionPopupHTML(PatientId, AppointmentId, Approve);
                    } else {
                        $('body').find('#modal-from-dom-RejectionReason').modal("show");
                        $('#popDiv #TxtRejectionReason').val("");
                    }
                } else {
                    MobileAppRequest.PatientAppointmentStatusChange(PatientId, AppointmentId, Approve);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PatientAppointmentStatusChange: function (PatientId, AppointmentId, Approve) {
        if (Approve == false) {
            var RejectionReason = $('#modal-from-dom-RejectionReason #TxtRejectionReason').val();
            MobileAppRequest.PatientInsertAppointment(PatientId, AppointmentId, Approve, RejectionReason).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(MobileAppRequest.DiscardMessage, 1);
                    $('#modal-from-dom-RejectionReason #TxtRejectionReason').val("");
                    MobileAppRequest.cancelConfirmDialog();
                    DashBoard.DashBoardCheckInRequestSearch(null, null, null);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    MobileAppRequest.cancelConfirmDialog();
                }
            });
        } else {
            MobileAppRequest.PatientInsertAppointment(PatientId, AppointmentId, Approve).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                    DashBoard.DashBoardCheckInRequestSearch(null, null, null);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    RejectionPopupHTML: function (PatientId, AppointmentId, Approve) {
        var DivFormGroup = '<div class="form-group">';
        var DivEnd = '</div>'
        var dialogTitle = "Rejection Reason";
        var SaveFunc = "MobileAppRequest.PatientAppointmentStatusChange(" + PatientId + ", " + AppointmentId + ", " + Approve + ");";
        var markUp = '';
        markUp = '<div id="modal-from-dom-RejectionReason" class="modal fade">'
                    + '<div class="modal-dialog modal-dialog-smd modal-top-adjust">'
                        + '<div class="modal-content">'
                            + '<div class="modal-header"><button type="button" onclick="MobileAppRequest.cancelConfirmDialog();"  class="close" "></button>'
                                + '<h4 class="modal-title">' + dialogTitle + '</h4>'
                            + DivEnd
                        + '<div class="modal-body bg-white" id="popDiv">'
                            + '<div class="col-xs-9">'
					            + '<textarea rows="3" maxlength="1000" name="RejectionReason" id="TxtRejectionReason" class="form-control" spellcheck="true" placeholder="Rejection reason goes here....."></textarea>'
				            + DivEnd
                            + DivFormGroup
                                    + '<div class="col-xs-12 pad-a-labelsize-btn">'
                                    + '<button id="btnDocumentScan" class="btn btn-primary btn-sm  rightbtn" type="button" onclick="' + SaveFunc + '">Save</button>'
                                    + DivEnd
                            + DivEnd
                        + DivEnd
                     + DivEnd
                    + DivEnd
                + DivEnd
                + '<div></div>;';
        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {

        }).on('hidden.bs.modal', function () {
            if ($('body').find('.modal-backdrop').length > 0) {
                if ($('body').css('overflow').toLowerCase() != "scroll") {
                    $('body').addClass('modal-open');
                }
                else {
                    $('body').addClass('modal-open');
                }
            }
        });
    },

    cancelConfirmDialog: function () {
        $("#modal-from-dom-RejectionReason").modal('hide');
        if ($("body").find(".modal-backdrop").length > 0) {
            $("body").find(".modal-backdrop").removeClass("modal-backdrop");
        }
    },

    PatientInsertAppointment: function (PatientId, AppointmentId, Approve, RejectionReason) {
        var objData = new JSON.constructor();
        objData["PatientId"] = PatientId;
        objData["AppointmentId"] = AppointmentId;
        objData["Approve"] = Approve;
        objData["RejectionReason"] = RejectionReason;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Scheduler", "PMSSchedulerNative");
    },


    LoadPatientInsurances: function () {

        if (MobileAppRequest.IsInsuranceFirstLoad == true) {
            $('#pnlMobileAppRequest  #frmMobileAppRequest').resetAllControls();
            MobileAppRequest.PatientInsurancesLoad(MobileAppRequest.params.patientID, MobileAppRequest.params.Status).done(function (response) {
                if (response.status != false) {
                    var InsuranceCount = 0;

                    MobileAppRequest.PatientInsuranceChecked = 0;
                    var PatientInsurancesCount = JSON.parse(response.PatientInsurancesCount);
                    var PatientInsuranceList = response.PatientInsuranceList_JSON;
                    $("#pnlMobileAppRequest #PatientInsurancesList").html('');
                    if (PatientInsurancesCount > 0) {
                        $.each(PatientInsuranceList, function (index, value) {
                            if (value.PlanPriority == "1") {
                                ButtonId = "Primary";
                            }
                            else if (value.PlanPriority == "2") {
                                ButtonId = "Secondary";
                            }
                            else if (value.PlanPriority == "3") {
                                ButtonId = "Tertiary";
                            }
                                //else if (value.PlanPriority == "4") {
                                //    ButtonId = "Quaternary";
                                //}
                                //else if (value.PlanPriority == "5") {
                                //    ButtonId = "Quinary";
                                //}
                                //else if (value.PlanPriority == "6") {
                                //    ButtonId = "Senary";
                                //}
                                //else if (value.PlanPriority == "7") {
                                //    ButtonId = "Septenary";
                                //}
                                //else if (value.PlanPriority == "8") {
                                //    ButtonId = "Octonary";
                                //}
                                //else if (value.PlanPriority == "9") {
                                //    ButtonId = "Nonary";
                                //}
                                //else if (value.PlanPriority == "10") {
                                //    ButtonId = "Denary";
                                //}

                            else {
                                ButtonId = "Insurance " + value.PlanPriority;
                            }
                            var $Button = $('<Button class="btn btn-link btn-sm btn-pipe text-bold">' + ButtonId + '</button>');
                            $Button.attr("id", "btnInsurance" + value.ColumnKeyId + "");
                            $Button.attr("onclick", "MobileAppRequest.LoadPatientInsuranceById(" + value.ColumnKeyId + ");");


                            $("#pnlMobileAppRequest #PatientInsurancesList").append($Button);
                            InsuranceCount++;
                            // This Check will tick the first Disease button and load the disease 

                            if (InsuranceCount == 1) {

                                MobileAppRequest.FirstInsuranceId = value.ColumnKeyId;

                            }

                        });
                        MobileAppRequest.IsInsuranceFirstLoad = false;
                        MobileAppRequest.PatientInsuranceCount = InsuranceCount;



                        MobileAppRequest.LoadPatientInsuranceById(MobileAppRequest.FirstInsuranceId);

                    }
                    else {
                        $('#pnlMobileAppRequest' + ' #btnInsurance').hide();

                        // Remove Section Insurance

                        var Sections = MobileAppRequest.params.DataSection.split(',');
                        var SectionIndex = Sections.indexOf('Insurance');
                        Sections.splice(SectionIndex, 1);
                        MobileAppRequest.params.DataSection = Sections.join(',');

                        // Load Next first avaliable tab of first section
                        MobileAppRequest.LoadFirstTabOfFirstSection(MobileAppRequest.params.DataSection, MobileAppRequest.params.DataSubSection);


                    }
                    $("#pnlMobileAppRequest #frmMobileAppRequest #divInsurance").hide();
                }
                else {

                    utility.DisplayMessages(response.Message, 2);
                }
            });
        }
        else {
            var buttonsCount = $('#pnlMobileAppRequest #PatientInsurancesList').find("button").length;

            if (buttonsCount > 0) {
                $('#pnlMobileAppRequest #PatientInsurancesList').find("button")[0].click();
            }
            else {
                $('#pnlMobileAppRequest' + ' #btnInsurance').hide();

                // Remove Section Insurance

                var Sections = MobileAppRequest.params.DataSection.split(',');
                var SectionIndex = Sections.indexOf('Insurance');
                Sections.splice(SectionIndex, 1);
                MobileAppRequest.params.DataSection = Sections.join(',');

                // Load Next first avaliable tab of first section
                MobileAppRequest.LoadFirstTabOfFirstSection(MobileAppRequest.params.DataSection, MobileAppRequest.params.DataSubSection);
            }
        }


    },
    PatientInsurancesLoad: function (PatientID, RequestStatus) {
        var data = "PatientID=" + PatientID + "&RequestStatus=" + RequestStatus;
        return MDVisionService.defaultService(data, "MobileAppRequest", "LOAD_PATIENT_INSURANCE");

    },

    LoadPatientInsuranceById: function (PatientInsuranceId) {

        var self = $('#pnlMobileAppRequest #frmMobileAppRequest #sectionInsurance');
        self.loadDropDowns(true).done(function () {
            $('#pnlMobileAppRequest  #frmMobileAppRequest').resetAllControls();
            MobileAppRequest.PatientInsuranceLoadById(PatientInsuranceId, MobileAppRequest.params.patientID, MobileAppRequest.params.Status).done(function (response) {
                if (response.status != false) {
                    $('#' + MobileAppRequest.params["PanelID"] + ' #divInsurance').show();
                    var PatientInsuranceDetail = JSON.parse(response.PatientInsuranceDetail_JSON)
                    MobileAppRequest.BindJSON(PatientInsuranceDetail);
                    MobileAppRequest.params.InsuranceId = PatientInsuranceDetail.InsuranceId;

                    MobileAppRequest.params.PatientInsuranceData = PatientInsuranceDetail;
                    MobileAppRequest.params.CurrentPlanPriority = PatientInsuranceDetail.PlanPriority;
                    $('#pnlMobileAppRequest #frmMobileAppRequest #divInsurance').find('label').removeClass('changed-field');
                    MobileAppRequest.HighLightChangedFieldsOfInsurance(PatientInsuranceDetail)
                    if ($('#pnlMobileAppRequest #btnInsurance' + PatientInsuranceId).find('i').length == 0) {
                        MobileAppRequest.PatientInsuranceChecked++;
                        $('#pnlMobileAppRequest #btnInsurance' + PatientInsuranceId).append('<i class="fa fa-check pull-left"></i>');
                        if (MobileAppRequest.PatientInsuranceCount == MobileAppRequest.PatientInsuranceChecked) {
                            if ($('#pnlMobileAppRequest #btnInsurance').find('i').length == 0) {
                                $('#pnlMobileAppRequest #btnInsurance').append('<i class="fa fa-check pull-left"></i>');
                                MobileAppRequest.SectionButtonsChecked++;
                            }
                            // uncomment when approve all and discard all will be ready
                            MobileAppRequest.ShowButtonsOfApprovalAllAndDiscardAll();

                        }
                    }
                }
                else {

                    utility.DisplayMessages(response.Message, 2);
                }
            });
        });
    },
    InsertUpdatePatientInsurance: function () {
        var strMessage = "";
        var InsurancePlanPriorityName = "";
        var self = $('#pnlMobileAppRequest');

        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (MobileAppRequest.params.InsuranceId < 0) {
                    // Implement the check here get insurance plan priorities 
                    var MaxPlanPriority = 0;
                    MobileAppRequest.GetMaxInsurancePlanPriority(MobileAppRequest.params.patientID).done(function (response) {
                        if (response.status != false) {

                            MaxPlanPriority = JSON.parse(response.PlanPriority);
                            MaxPlanPriority++;
                        }


                        if (MaxPlanPriority == "1") {
                            InsurancePlanPriorityName = "Primary";
                        }
                        else if (MaxPlanPriority == "2") {
                            InsurancePlanPriorityName = "Secondary";
                        }
                        else if (MaxPlanPriority == "3") {
                            InsurancePlanPriorityName = "Tertiary";
                        }
                        else if (MaxPlanPriority == "4") {
                            InsurancePlanPriorityName = "Quaternary";
                        }
                        else if (MaxPlanPriority == "5") {
                            InsurancePlanPriorityName = "Quinary";
                        }
                        else if (MaxPlanPriority == "6") {
                            InsurancePlanPriorityName = "Senary";
                        }
                        else if (MaxPlanPriority == "7") {
                            InsurancePlanPriorityName = "Septenary";
                        }
                        else if (MaxPlanPriority == "8") {
                            InsurancePlanPriorityName = "Octonary";
                        }
                        else if (MaxPlanPriority == "9") {
                            InsurancePlanPriorityName = "Nonary";
                        }
                        else if (MaxPlanPriority == "10") {
                            InsurancePlanPriorityName = "Denary";
                        }

                        else {
                            InsurancePlanPriorityName = "Insurance at " + MaxPlanPriority + " Priority";
                        }


                        // Yes no Alert

                        MobileAppRequest.params.PatientInsuranceData.IsActive = true;
                        if (MobileAppRequest.params.CurrentPlanPriority != (MaxPlanPriority) && MaxPlanPriority != 0) {
                            //utility.myConfirm(" Do you want to save this Insurance as " + InsurancePlanPriorityName + "",
                            //                              function () {
                            MobileAppRequest.PatientInsuranceSave(MobileAppRequest.params.PatientInsuranceData).done(function (response) {
                                if (response.status != false) {

                                    utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                                    $("#pnlMobileAppRequest #PatientInsurancesList").find("#btnInsurance" + MobileAppRequest.params.InsuranceId).remove();
                                    // MobileAppRequest.PatientInsuranceCount--;
                                    MobileAppRequest.LoadPatientInsurances();

                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                            //},
                            //function () {

                            //}, "ALERT");

                        }
                        else {
                            MobileAppRequest.PatientInsuranceSave(MobileAppRequest.params.PatientInsuranceData).done(function (response) {
                                if (response.status != false) {

                                    utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                                    $("#pnlMobileAppRequest #PatientInsurancesList").find("#btnInsurance" + MobileAppRequest.params.InsuranceId).remove();
                                    // MobileAppRequest.PatientInsuranceCount--;
                                    MobileAppRequest.LoadPatientInsurances();

                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });

                        }

                    });

                }
                else {
                    MobileAppRequest.PatientInsuranceUpdate(MobileAppRequest.params.PatientInsuranceData).done(function (response) {
                        if (response.status != false) {

                            utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                            $("#pnlMobileAppRequest #PatientInsurancesList").find("#btnInsurance" + MobileAppRequest.params.InsuranceId).remove();
                            //  MobileAppRequest.PatientInsuranceCount--;
                            MobileAppRequest.LoadPatientInsurances();

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
    },
    GetMaxInsurancePlanPriority: function (PatientId) {
        var data = "PatientID=" + PatientId;
        return MDVisionService.defaultService(data, "MobileAppRequest", "LOAD_PATIENT_INSURANCE_PRIORITY");

    },
    PatientInsuranceLoadById: function (InsuranceId, PatientID, RequestStatus) {
        //if (PageNumber == null) {
        //    PageNumber = 1;
        //}
        //if (RowsPerPage == null) {
        //    RowsPerPage = 15;
        //}
        var data = "PatientID=" + PatientID + "&InsuranceId=" + InsuranceId + "&RequestStatus=" + RequestStatus;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "MobileAppRequest", "FILL_PATIENT_INSURANCE");
        //var objData = new Object();
        //objData["PatientID"] = PatientID;
        //objData["CommandType"] = "load_patient_emergency_contacts_native";
        //var data = JSON.stringify(objData);
        //return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographicNative");
    },
    PatientInsuranceSave: function (PatientInsuranceData) {
        var data = "PatientInsuranceData=" + encodeURIComponent(JSON.stringify(PatientInsuranceData));
        //  var data = "EmergencyContactData=" + EmergencyContactData ;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "MobileAppRequest", "SAVE_PATIENT_INSURANCE");
    },
    PatientInsuranceUpdate: function (PatientInsuranceData) {
        // var data = JSON.stringify("EmergencyContactData=" + EmergencyContactData);
        var data = "PatientInsuranceData=" + encodeURIComponent(JSON.stringify(PatientInsuranceData));
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "MobileAppRequest", "UPDATE_PATIENT_INSURANCE");
    },
    DiscardRecordInDatabase: function (DBtableName, PatientID, ColumnKeyId, changedColumnsArray) {

        // var data = JSON.stringify("EmergencyContactData=" + EmergencyContactData);
        var data = "";
        if (changedColumnsArray != "")
            data = "PatientID=" + PatientID + "&DBtableName=" + DBtableName + "&ColumnKeyId=" + ColumnKeyId + "&changedColumnsArray=" + JSON.stringify(changedColumnsArray);
        else
            data = "PatientID=" + PatientID + "&DBtableName=" + DBtableName + "&ColumnKeyId=" + ColumnKeyId + "&changedColumnsArray=" + changedColumnsArray;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "MobileAppRequest", "DISCARD_RECORD");
    },
    DiscardRecord: function (DBTableName) {
        var strMessage = "";
        var self = $('#pnlMobileAppRequest');
        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var ColumnKeyId = "-1";
                var ChangedColumns = "";
                if (MobileAppRequest.params.patientID > 0) {
                    if (DBTableName == "Patients") {
                        ColumnKeyId = MobileAppRequest.params.patientID;
                        ChangedColumns = MobileAppRequest.params.Demographic_Data.lstChangedColumns;
                    }
                    else if (DBTableName == "PatientPreferences") {
                        ChangedColumns = MobileAppRequest.params.PatientPreferencesData.lstChangedColumns;
                        ColumnKeyId = MobileAppRequest.params.patientID;
                    }
                    else if (DBTableName == "PatientInsurance") {

                        ChangedColumns = MobileAppRequest.params.PatientInsuranceData.lstChangedColumns;
                        ColumnKeyId = MobileAppRequest.params.InsuranceId;
                    }
                    else if (DBTableName == "EmergencyContacts") {
                        ChangedColumns = MobileAppRequest.params.EmergencyContactData.lstChangedColumns;
                        ColumnKeyId = MobileAppRequest.params.ContactId;
                    }
                    else if (DBTableName == "SurgicalHx_Disease") {
                        ChangedColumns = "";
                        ColumnKeyId = MobileAppRequest.params.SurgicalHxDiseaseData.DiseaseId;
                    }
                    else if (DBTableName == "MedicalHx_Disease") {
                        ChangedColumns = "";
                        ColumnKeyId = MobileAppRequest.params.MedicalHxDiseaseId;
                    }
                    else if (DBTableName == "BirthHx_General") {

                        ChangedColumns = MobileAppRequest.params.BirthHxGeneralData.lstChangedColumns;
                        ColumnKeyId = MobileAppRequest.params.BirthHxGeneralData.GeneralId;
                    }
                    else if (DBTableName == "BirthHx_Newborn") {

                        ChangedColumns = MobileAppRequest.params.NewBornData.lstChangedColumns;
                        ColumnKeyId = MobileAppRequest.params.NewBornData.NewbornId;
                    }
                    else if (DBTableName == "BirthHx_MaternalDelivery") {

                        ChangedColumns = MobileAppRequest.params.MaternalData.lstChangedColumns;
                        ColumnKeyId = MobileAppRequest.params.MaternalData.MaternalDeliveryId;
                    }
                    else if (DBTableName == "HospitalizationHx_Disease") {

                        ChangedColumns = "";
                        ColumnKeyId = MobileAppRequest.params.HospitalizationHxDiseaseId;
                    }
                    else if (DBTableName == "SocialHx") {
                        ChangedColumns = "";
                        ColumnKeyId = null;
                    }
                    else if (DBTableName == "FamilyHx_Mobile") {
                        ChangedColumns = "";
                        ColumnKeyId = MobileAppRequest.params.FamilyHxDiseasesId
                    }
                    MobileAppRequest.DiscardRecordInDatabase(DBTableName, MobileAppRequest.params.patientID, ColumnKeyId, ChangedColumns).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(MobileAppRequest.DiscardMessage, 1);
                            if (DBTableName == "Patients") {
                                var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                                var Index = subtabs.indexOf('Patients');
                                subtabs.splice(Index, 1);
                                MobileAppRequest.params.DataSubSection = subtabs.join(',')
                                MobileAppRequest.ShowDemographicsButtons();
                            }
                            else if (DBTableName == "PatientPreferences") {
                                var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                                var Index = subtabs.indexOf('PatientPreferences');
                                subtabs.splice(Index, 1);
                                MobileAppRequest.params.DataSubSection = subtabs.join(',')
                                MobileAppRequest.ShowDemographicsButtons();
                            }
                            else if (DBTableName == "PatientInsurance") {
                                $("#pnlMobileAppRequest #PatientInsurancesList").find("#btnInsurance" + MobileAppRequest.params.InsuranceId).remove();
                                // MobileAppRequest.PatientInsuranceCount--;
                                MobileAppRequest.LoadPatientInsurances();
                            }
                            else if (DBTableName == "EmergencyContacts") {
                                $("#pnlMobileAppRequest #ECList").find("#btnEmergencyContact" + MobileAppRequest.params.ContactId).remove();
                                //  MobileAppRequest.EmergencyContactsCount--;
                                MobileAppRequest.LoadPatientEmergencyContactsNative();
                            }
                            else if (DBTableName == "HospitalizationHx_Disease") {
                                $("#pnlMobileAppRequest #divHospitalizationDiseaseList").find("#btnHospitalizationHxDiseaseId" + MobileAppRequest.params.HospitalizationHxDiseaseId).remove();
                                MobileAppRequest.LoadHospitalizationHxDiseases();
                            }
                            else if (DBTableName == "MedicalHx_Disease") {
                                $("#pnlMobileAppRequest #divMedicalHxDiseaseList").find("#btnMedicalHxDiseaseId" + MobileAppRequest.params.MedicalHxDiseaseId).remove();
                                MobileAppRequest.LoadMedicalHxDiseases();

                            }
                            else if (DBTableName == "SurgicalHx_Disease") {
                                $("#pnlMobileAppRequest #divSurgicalHxDiseaseList").find("#btnSurgicalHxDiseaseId" + MobileAppRequest.params.SurgicalHxDiseaseData.DiseaseId).remove();
                                MobileAppRequest.LoadSurgicalHxDiseases();
                            }
                            else if (DBTableName == "SocialHx") {
                                var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                                var Index = subtabs.indexOf('SocialHx');
                                subtabs.splice(Index, 1);
                                MobileAppRequest.params.DataSubSection = subtabs.join(',');
                                MobileAppRequest.ShowHistoryButtons();
                            }


                            else if (DBTableName == "FamilyHx_Mobile") {
                                //MobileAppRequest.isFamilyHXFirstLoad = true;
                                // MobileAppRequest.LoadFamilyHxDiseases();
                                //      if (MobileAppRequest.params.CurrentFamilyMemberId != null) {
                                var ButtonDiscard = $("#pnlMobileAppRequest #frmMobileAppRequest #btnFamilyHxDiscard");
                                var RelationName = ButtonDiscard[0].name;
                                var DiscardDiseaseID = $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #hfbtnDiseaseID").val();
                                var FamilyMemberButton = $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #divFamilyHxMembersList button[name=" + RelationName + "]");
                                //MobileAppRequest.LoadFamilyHxMemberDiseases(MobileAppRequest.params.CurrentFamilyMemberId, FamilyMemberButton[0], DiscardDiseaseID);
                                // MobileAppRequest.LoadFamilyHxMemberDiseases(MobileAppRequest.params.CurrentFamilyMemberId);
                                // var DiscardDiseaseID = $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #hfbtnDiseaseID").val();

                                $('#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHxDetail').hide();
                                if (DiscardDiseaseID != null || DiscardDiseaseID != "undefiend") {
                                    var parentDiv = $("#pnlMobileAppRequest #divFamilyHxDiseaseList #" + DiscardDiseaseID).parent()[0];
                                    var FmDiseaseCount = $(parentDiv).find("button").length;
                                    $("#pnlMobileAppRequest #divFamilyHxDiseaseList #" + DiscardDiseaseID).remove();

                                    if (FmDiseaseCount == 1) {
                                        var familyMembersbtn = $(FamilyMemberButton.parent()[0]).find("button");

                                        var familyMembersCount = familyMembersbtn.length;
                                        FamilyMemberButton.remove();
                                        parentDiv.remove();
                                        if (familyMembersCount <= 1) {
                                            $("#divFamilyHxMembersList").html('');
                                            $("#pnlMobileAppRequest #btnFamilyHx").hide();
                                            var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                                            var Index = subtabs.indexOf('FamilyHx');
                                            subtabs.splice(Index, 1);
                                            MobileAppRequest.params.DataSubSection = subtabs.join(',');
                                            MobileAppRequest.ShowHistoryButtons();

                                        }

                                        else {
                                            $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #divFamilyHxMembersList").find("button")[0].click();
                                        }

                                    }
                                    else {
                                        $(parentDiv).find("button")[0].click();
                                    }
                                }
                                //   }
                                //  MobileAppRequest.LoadFamilyHxDiseases();

                            }
                            else if (DBTableName == "BirthHx_General") {
                                var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                                var Index = subtabs.indexOf('BirthHx_General');
                                subtabs.splice(Index, 1);
                                MobileAppRequest.params.DataSubSection = subtabs.join(',');

                                MobileAppRequest.ShowBirthHistoryButtons();
                            }
                            else if (DBTableName == "BirthHx_Newborn") {
                                var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                                var Index = subtabs.indexOf('BirthHx_Newborn');
                                subtabs.splice(Index, 1);
                                MobileAppRequest.params.DataSubSection = subtabs.join(',');

                                MobileAppRequest.ShowBirthHistoryButtons();
                            }
                            else if (DBTableName == "BirthHx_MaternalDelivery") {
                                var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                                var Index = subtabs.indexOf('BirthHx_MaternalDelivery');
                                subtabs.splice(Index, 1);
                                MobileAppRequest.params.DataSubSection = subtabs.join(',');

                                MobileAppRequest.ShowBirthHistoryButtons();
                            }
                            else {

                            }
                            //to update count and grid on dashboardpatientchanges
                            //  DashBoard.DashBoardPatientChangesSearch(null, null, null);

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
    },

    LoadHospitalizationHxDiseases: function () {
        $('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();

        $("#pnlMobileAppRequest #frmMobileAppRequest #divHospitalizationHx").show();
        if (MobileAppRequest.IsHospitalizationHxFirstLoad == true) {
            MobileAppRequest.HospitalizationHxDiseasesLoad(MobileAppRequest.params.patientID, MobileAppRequest.params.Status).done(function (response) {
                if (response.status != false) {
                    $("#pnlMobileAppRequest #frmMobileAppRequest #divHospitalizationHx").show();
                    var HospitalizationHxDiseaseList = JSON.parse(response.HospitalizationHxDiseaseLoad_JSON);
                    if (HospitalizationHxDiseaseList.length > 0) {
                        var DiseaseCount = 0;

                        MobileAppRequest.HospitalizationHxDiseasesChecked = 0;
                        $("#pnlMobileAppRequest #divHospitalizationDiseaseList").html('');

                        $.each(HospitalizationHxDiseaseList, function (index, value) {
                            var ButtonId = index + 1;
                            var $Button = $('<Button class="btn btn-default btn-sm tab_space">' + value.ICD10CodeDescription + '</button>');
                            $Button.attr("id", "btnHospitalizationHxDiseaseId" + value.DiseaseId + "");
                            $Button.attr("onclick", "MobileAppRequest.LoadHospitalizationHxDiseaseById(" + value.DiseaseId + ");");
                            $("#pnlMobileAppRequest #divHospitalizationDiseaseList").append($Button);
                            DiseaseCount++;
                            // This Check will tick the first Disease button and load the disease 

                            if (DiseaseCount == 1) {

                                MobileAppRequest.FirstHospitalizationHxDisease = value.DiseaseId;
                            }

                        });
                        MobileAppRequest.IsHospitalizationHxFirstLoad = false;
                        MobileAppRequest.HospitalizationHxDiseaseCount = DiseaseCount;
                        MobileAppRequest.LoadHospitalizationHxDiseaseById(MobileAppRequest.FirstHospitalizationHxDisease);

                    }
                    else {
                        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                        var Index = subtabs.indexOf('HospitalizationHx');
                        subtabs.splice(Index, 1);
                        MobileAppRequest.params.DataSubSection = subtabs.join(',');
                        MobileAppRequest.ShowHistoryButtons();
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 2);

                }
            });
        }
        else {

            var buttonsCount = $('#pnlMobileAppRequest #divHospitalizationDiseaseList').find("button").length;

            if (buttonsCount > 0) {
                $('#pnlMobileAppRequest #divHospitalizationDiseaseList').find("button")[0].click();
            }
            else {
                var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                var Index = subtabs.indexOf('HospitalizationHx');
                subtabs.splice(Index, 1);
                MobileAppRequest.params.DataSubSection = subtabs.join(',');
                MobileAppRequest.ShowHistoryButtons();
            }

        }

    },

    TickHistoryButton: function () {
        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
        var SubHistoriescount = 0;
        var SubHistoriesCheckedCount = 0;
        $.each(subtabs, function (index, Item) {

            if (Item == "BirthHx" || Item == "HospitalizationHx" || Item == "MedicalHx" || Item == "SurgicalHx" || Item == "FamilyHx" || Item == "SocialHx") {

                if ($('#btn' + Item).find('i').length != 0) {
                    SubHistoriesCheckedCount++;
                }
                SubHistoriescount++;

            }
        })
        if (SubHistoriesCheckedCount == SubHistoriescount) {
            if ($('#pnlMobileAppRequest #btnHistory').find('i').length == 0) {
                $('#pnlMobileAppRequest #btnHistory').append('<i class="fa fa-check pull-left"></i>');
                MobileAppRequest.SectionButtonsChecked++;
            }
            // uncomment when approve all and discard all will be ready
            MobileAppRequest.ShowButtonsOfApprovalAllAndDiscardAll();
        }
    },
    TickDemographicsButton: function () {
        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
        var SubDemographicscount = 0;
        var SubDemographicsCheckedCount = 0;
        $.each(subtabs, function (index, Item) {

            if (Item == "Patients" || Item == "EmergencyContacts" || Item == "PatientPreferences") {

                if ($('#btn' + Item).find('i').length != 0) {
                    SubDemographicsCheckedCount++;
                }
                SubDemographicscount++;

            }
        })

        if (SubDemographicsCheckedCount == SubDemographicscount) {
            if ($('#pnlMobileAppRequest #btnDemographics').find('i').length == 0) {
                $('#pnlMobileAppRequest #btnDemographics').append('<i class="fa fa-check pull-left"></i>');
                MobileAppRequest.SectionButtonsChecked++;
            }
            // uncomment when approve all and discard all will be ready
            MobileAppRequest.ShowButtonsOfApprovalAllAndDiscardAll();
        }
    },

    LoadFirstTabOfFirstSection: function (Sections, Tabs) {

        // ,History,Insurance
        // BirthHx,BirthHx_General,BirthHx_MaternalDelivery,BirthHx_Newborn,EmergencyContacts,FamilyHx,HospitalizationHx,Insurance,PatientPreferences,Patients
        if (Sections.indexOf("Demographics") != -1) {

            MobileAppRequest.ShowDemographicsButtons();

        }
        else if (Sections.indexOf("Insurance") != -1) {

            $('#pnlMobileAppRequest' + ' #sectionInsurance').show();
            MobileAppRequest.LoadPatientInsurances();
        }
        else if (Sections.indexOf("History") != -1) {

            MobileAppRequest.ShowHistoryButtons();
        }
            //else if (Sections.indexOf("Appointment") != -1) {

            //    MobileAppRequest.LoadPatientAppointment();
            //}
        else {
            MobileAppRequest.UnLoad();
        }



    },
    HospitalizationHxDiseasesLoad: function (PatientID, RequestStatus, DiseaseId) {
        if (DiseaseId != undefined)
            var data = "PatientId=" + PatientID + "&RequestStatus=" + RequestStatus + "&DiseaseId=" + DiseaseId;
        else
            var data = "PatientId=" + PatientID + "&RequestStatus=" + RequestStatus;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "MobileAppRequest", "LOAD_HOSPITALIZATIONHX_DISEASES");
    },
    LoadHospitalizationHxDiseaseById: function (HospitalizationHxDiseaseId) {


        MobileAppRequest.HospitalizationHxDiseasesLoad(MobileAppRequest.params.patientID, MobileAppRequest.params.Status, HospitalizationHxDiseaseId).done(function (response) {
            if (response.status != false) {

                $('#pnlMobileAppRequest #frmMobileAppRequest #divHospitalizationHxDetail').find('label').removeClass('changed-field');

                var HospitalizationHxDiseaseDetail = JSON.parse(response.DiseaseFill_JSON)
                MobileAppRequest.params.HospitalizationHxId = HospitalizationHxDiseaseDetail.HospitalizationHxId;
                MobileAppRequest.params.HospitalizationHxDiseaseId = HospitalizationHxDiseaseDetail.DiseaseId;
                MobileAppRequest.params.HospitalizationhxDiseaseData = HospitalizationHxDiseaseDetail;
                MobileAppRequest.params.HospitalizationHxDate = HospitalizationHxDiseaseDetail.CreatedOn;
                var self = $('#pnlMobileAppRequest #frmMobileAppRequest #divHospitalizationHxDetail');
                $('#pnlMobileAppRequest #frmMobileAppRequest #divHospitalizationHxDetail').show();
                utility.bindMyJSONByName(true, MobileAppRequest.params.HospitalizationhxDiseaseData, false, self).done(function () {
                    $('#pnlMobileAppRequest #frmMobileAppRequest #divHospitalizationHxDetail').find('label').addClass('changed-field');
                    if ($('#pnlMobileAppRequest #btnHospitalizationHxDiseaseId' + HospitalizationHxDiseaseId).find('i').length == 0) {
                        MobileAppRequest.HospitalizationHxDiseasesChecked++;
                        $('#pnlMobileAppRequest #btnHospitalizationHxDiseaseId' + HospitalizationHxDiseaseId).append('<i class="fa fa-check pull-left"></i>');
                        if (MobileAppRequest.HospitalizationHxDiseaseCount == MobileAppRequest.HospitalizationHxDiseasesChecked) {
                            if ($('#pnlMobileAppRequest #btnHospitalizationHx').find('i').length == 0)
                                $('#pnlMobileAppRequest #btnHospitalizationHx').append('<i class="fa fa-check pull-left"></i>');
                            MobileAppRequest.TickHistoryButton();


                        }
                    }

                });

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    InsertHospitalizationHxDiseaase: function () {
        var strMessage = "";
        var self = $('#pnlMobileAppRequest');
        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (MobileAppRequest.params.HospitalizationHxDiseaseId < 0) {
                    MobileAppRequest.HospitalizationHxDiseaseSave(MobileAppRequest.params.HospitalizationhxDiseaseData).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                            $("#pnlMobileAppRequest #divHospitalizationDiseaseList").find("#btnHospitalizationHxDiseaseId" + MobileAppRequest.params.HospitalizationHxDiseaseId).remove();
                            MobileAppRequest.LoadHospitalizationHxDiseases();
                            Clinical_HospitalizationHx.bIsFirstLoad = true;
                            $('#pnlMobileAppRequest #frmMobileAppRequest #divHospitalizationHxDetail').hide();


                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }

                else {
                    utility.DisplayMessages("Disease Already Saved", 3);

                }
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    InsertSurgicalHxDiseaase: function () {
        var strMessage = "";
        var self = $('#pnlMobileAppRequest');
        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                MobileAppRequest.SurgicalHxDiseaseSave(MobileAppRequest.params.SurgicalHxDiseaseData).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $("#pnlMobileAppRequest #divSurgicalHxDiseaseList").find("#btnSurgicalHxDiseaseId" + MobileAppRequest.params.SurgicalHxDiseaseData.DiseaseId).remove();
                        Clinical_SurgicalHx.bIsFirstLoad = true;
                        utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                        MobileAppRequest.LoadSurgicalHxDiseases();

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    HospitalizationHxDiseaseSave: function (HospitalizationHxData) {
        var objData = HospitalizationHxData;
        var HospitalizationDiseaseStayText = $('#pnlMobileAppRequest #frmMobileAppRequest #divHospitalizationHxDetail #ddlHospitalizationDiseaseStayId :selected').text();
        objData["HospitalizationDiseaseStayText"] = HospitalizationDiseaseStayText;
        var HospitalizationDiseaseStatusText = $('#pnlMobileAppRequest #frmMobileAppRequest #divHospitalizationHxDetail #ddlHospitalizationStatus :selected').text();
        objData["HospitalizationDiseaseStatusText"] = HospitalizationDiseaseStatusText;

        objData["PatientId"] = MobileAppRequest.params.patientID;
        objData["HospitalizationHxId"] = MobileAppRequest.params.HospitalizationHxId;
        if (MobileAppRequest.params.HospitalizationHxId == "-1")
            objData["commandType"] = "save_hospitalizationhx";
        else
            objData["commandType"] = "update_hospitalizationhx";
        objData["HospitalizationHxType"] = "disease";
        objData["HospitalizationHxUnremarkable"] = "false";
        objData["AddedFromMobileApp"] = "1";
        objData["HospitalizationHxDate"] = MobileAppRequest.params.HospitalizationHxDate;

        var data = JSON.stringify(objData);

        //var data = "HospitalizationHxignsData=" + HospitalizationHxignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HospitalizationHx");
    },

    SurgicalHxDiseaseSave: function (SurgicalHxData) {
        var objData = SurgicalHxData;
        var SurgicalStatusText = $('#pnlMobileAppRequest #frmMobileAppRequest #divSurgicalHx #ddlSurgicalStatus :selected').text();
        objData["SurgicalStatusText"] = SurgicalStatusText;

        objData["PatientId"] = MobileAppRequest.params.patientID;
        objData["SurgicalHxId"] = MobileAppRequest.params.SurgicalHxDiseaseData.SurgicalHxId;
        objData["AddFromMobile"] = "1";
        objData["SurgicalHxUnremarkable"] = "false";
        objData["SurgicalHxType"] = "disease";
        if (MobileAppRequest.params.SurgicalHxDiseaseData.SurgicalHxId == "-1")
            objData["commandType"] = "save_surgicalhx";
        else
            objData["commandType"] = "update_surgicalhx";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "HISTORY", "SurgicalHx");
    },

    LoadSurgicalHxDiseases: function () {

        $('#pnlMobileAppRequest  #frmMobileAppRequest').resetAllControls();
        $('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();
        $("#pnlMobileAppRequest #frmMobileAppRequest #divSurgicalHx").show();
        if (MobileAppRequest.IsSurgicalHxFirstLoad == true) {
            MobileAppRequest.SurgicalHxDiseasesLoad(MobileAppRequest.params.patientID, MobileAppRequest.params.Status).done(function (response) {
                if (response.status != false) {
                    $("#pnlMobileAppRequest #frmMobileAppRequest #divSurgicalHx").show();
                    var SurgicalHxDiseaseList = JSON.parse(response.SurgicalHxDiseaseLoad_JSON);

                    if (SurgicalHxDiseaseList.length > 0) {


                        $("#pnlMobileAppRequest #divSurgicalHxDiseaseList").html('');
                        var DiseaseCount = 0;
                        MobileAppRequest.SurgicalHxDiseasesChecked = 0;
                        $.each(SurgicalHxDiseaseList, function (index, value) {
                            var ButtonId = index + 1;
                            var $Button = $('<Button class="btn btn-default btn-sm tab_space">' + value.CPTCodeDescription + '</button>');
                            $Button.attr("id", "btnSurgicalHxDiseaseId" + value.DiseaseId + "");
                            $Button.attr("onclick", "MobileAppRequest.LoadSurgicalHxDiseaseById(" + value.DiseaseId + ");");
                            $("#pnlMobileAppRequest #divSurgicalHxDiseaseList").append($Button);
                            DiseaseCount++;
                            // This Check will tick the first Disease button and load the disease 

                            if (DiseaseCount == 1) {

                                MobileAppRequest.FirstSurgicalHxDiseaseId = value.DiseaseId;

                            }
                        });


                        MobileAppRequest.IsSurgicalHxFirstLoad = false;
                        MobileAppRequest.SurgicalHxDiseaseCount = DiseaseCount;




                        MobileAppRequest.LoadSurgicalHxDiseaseById(MobileAppRequest.FirstSurgicalHxDiseaseId);



                    }
                    else {
                        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                        var Index = subtabs.indexOf('SurgicalHx');
                        subtabs.splice(Index, 1);
                        MobileAppRequest.params.DataSubSection = subtabs.join(',');
                        MobileAppRequest.ShowHistoryButtons();
                    }
                }
            });
        }
        else {
            var buttonsCount = $('#pnlMobileAppRequest #divSurgicalHxDiseaseList').find("button").length;

            if (buttonsCount > 0) {
                $('#pnlMobileAppRequest #divSurgicalHxDiseaseList').find("button")[0].click();
            }
            else {
                var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                var Index = subtabs.indexOf('SurgicalHx');
                subtabs.splice(Index, 1);
                MobileAppRequest.params.DataSubSection = subtabs.join(',');
                MobileAppRequest.ShowHistoryButtons();
            }
        }
    },
    SurgicalHxDiseasesLoad: function (PatientID, RequestStatus, DiseaseId) {
        if (DiseaseId != undefined)
            var data = "PatientId=" + PatientID + "&RequestStatus=" + RequestStatus + "&DiseaseId=" + DiseaseId;
        else
            var data = "PatientId=" + PatientID + "&RequestStatus=" + RequestStatus;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "MobileAppRequest", "LOAD_SURGICALHX_DISEASES");
    },
    LoadSurgicalHxDiseaseById: function (SurgicalHxDiseaseId) {

        MobileAppRequest.SurgicalHxDiseasesLoad(MobileAppRequest.params.patientID, MobileAppRequest.params.Status, SurgicalHxDiseaseId).done(function (response) {
            if (response.status != false) {

                var SurgicalHxDiseaseDetail = JSON.parse(response.DiseaseFill_JSON)

                MobileAppRequest.params.SurgicalHxDiseaseData = SurgicalHxDiseaseDetail;
                // MobileAppRequest.params.SurgicalHxDiseaseData.SurgicalSurgeryDate = MobileAppRequest.params.SurgicalHxDiseaseData.SurgicalSurgeryDate.substr(0, 10);
                if (MobileAppRequest.params.SurgicalHxDiseaseData.SurgicalSurgeryDate != "") {
                    var cdate = new Date(MobileAppRequest.params.SurgicalHxDiseaseData.SurgicalSurgeryDate)
                    var sDate = ((cdate.getMonth() + 1) < 10 ? "0" + (cdate.getMonth() + 1) : (cdate.getMonth() + 1)) + "/" + (cdate.getDate() < 10 ? "0" + cdate.getDate() : cdate.getDate()) + "/" + cdate.getFullYear();
                    MobileAppRequest.params.SurgicalHxDiseaseData.SurgicalSurgeryDate = sDate;
                }
                var self = $('#pnlMobileAppRequest #frmMobileAppRequest #divSurgicalHxDetail');
                utility.bindMyJSONByName(true, MobileAppRequest.params.SurgicalHxDiseaseData, false, self).done(function () {
                    $('#pnlMobileAppRequest #frmMobileAppRequest #divSurgicalHxDetail').show();
                    $('#pnlMobileAppRequest #frmMobileAppRequest #divSurgicalHxDetail').find('label').addClass('changed-field');
                    if ($('#pnlMobileAppRequest #btnSurgicalHxDiseaseId' + SurgicalHxDiseaseId).find('i').length == 0) {
                        MobileAppRequest.SurgicalHxDiseasesChecked++;
                        $('#pnlMobileAppRequest #btnSurgicalHxDiseaseId' + SurgicalHxDiseaseId).append('<i class="fa fa-check pull-left"></i>');
                        if (MobileAppRequest.SurgicalHxDiseaseCount == MobileAppRequest.SurgicalHxDiseasesChecked) {
                            if ($('#pnlMobileAppRequest #btnSurgicalHx').find('i').length == 0)
                                $('#pnlMobileAppRequest #btnSurgicalHx').append('<i class="fa fa-check pull-left"></i>');
                            MobileAppRequest.TickHistoryButton();

                        }
                    }
                    // High Ligt Changed Columns.
                });

            }
        });
    },

    LoadMedicalHxDiseases: function () {


        $('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();
        $("#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHx").show();
        if (MobileAppRequest.IsMedicalHxFirstLoad == true) {
            MobileAppRequest.MedicalHxDiseasesLoad(MobileAppRequest.params.patientID, MobileAppRequest.params.Status).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    // var EmergencyContactsCount = JSON.parse(response.EmergencyContactsCount);
                    $("#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHx").show();
                    var MedicalHxDiseaseList = JSON.parse(response.MedicalHxDiseaseLoad_JSON);
                    //  $("#pnlMobileAppRequest #ECList").html('');

                    if (MedicalHxDiseaseList.length > 0) {


                        $("#pnlMobileAppRequest #divMedicalHxDiseaseList").html('');
                        var DiseaseCount = 0;

                        MobileAppRequest.MedicalHxDiseasesChecked = 0;
                        //  $("#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHx").show();
                        $.each(MedicalHxDiseaseList, function (index, value) {
                            var ButtonId = index + 1;
                            var $Button = $('<Button class="btn btn-default btn-sm tab_space">' + value.ICD10CodeDescription + '</button>');
                            $Button.attr("id", "btnMedicalHxDiseaseId" + value.DiseaseId + "");
                            $Button.attr("onclick", "MobileAppRequest.LoadMedicalHxDiseaseById(" + value.DiseaseId + ");");
                            // $Button.attr("value", "EmergencyContact " + ButtonId + "");
                            $("#pnlMobileAppRequest #divMedicalHxDiseaseList").append($Button);
                            DiseaseCount++;
                            // This Check will set the first Disease Id

                            if (DiseaseCount == 1) {

                                MobileAppRequest.FirstMedicalHxDiseaseId = value.DiseaseId;

                            }
                        });


                        MobileAppRequest.IsMedicalHxFirstLoad = false;
                        MobileAppRequest.medicalhxDiseaseCount = DiseaseCount;



                        MobileAppRequest.LoadMedicalHxDiseaseById(MobileAppRequest.FirstMedicalHxDiseaseId);

                    }
                    else {
                        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                        var Index = subtabs.indexOf('MedicalHx');
                        subtabs.splice(Index, 1);
                        MobileAppRequest.params.DataSubSection = subtabs.join(',');
                        MobileAppRequest.ShowHistoryButtons();
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }

        else {
            var buttonsCount = $('#pnlMobileAppRequest #divMedicalHxDiseaseList').find("button").length;

            if (buttonsCount > 0) {
                $('#pnlMobileAppRequest #divMedicalHxDiseaseList').find("button")[0].click();
            }
            else {
                var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                var Index = subtabs.indexOf('MedicalHx');
                subtabs.splice(Index, 1);
                MobileAppRequest.params.DataSubSection = subtabs.join(',');
                MobileAppRequest.ShowHistoryButtons();
            }
        }



    },
    MedicalHxDiseasesLoad: function (PatientID, RequestStatus, DiseaseId) {
        var objData = new Object();
        objData["PatientId"] = PatientID;
        objData["MedicalHxType"] = "disease";
        objData["RequestStatus"] = RequestStatus;
        objData["commandType"] = "fill_medicalhx_native";
        objData["DiseaseId"] = DiseaseId != undefined ? DiseaseId : 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "MedicalHx");

    },

    LoadMedicalHxDiseaseById: function (MedicalHxDiseaseId) {
        MobileAppRequest.MedicalHxDiseasesLoad(MobileAppRequest.params.patientID, MobileAppRequest.params.Status, MedicalHxDiseaseId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail').find('label').removeClass('changed-field');
                if ($('#pnlMobileAppRequest #btnMedicalHxDiseaseId' + MedicalHxDiseaseId).find('i').length == 0) {
                    MobileAppRequest.MedicalHxDiseasesChecked++;
                    $('#pnlMobileAppRequest #btnMedicalHxDiseaseId' + MedicalHxDiseaseId).append('<i class="fa fa-check pull-left"></i>');
                    if (MobileAppRequest.medicalhxDiseaseCount == MobileAppRequest.MedicalHxDiseasesChecked) {
                        if ($('#pnlMobileAppRequest #btnMedicalHx').find('i').length == 0)
                            $('#pnlMobileAppRequest #btnMedicalHx').append('<i class="fa fa-check pull-left"></i>');
                        MobileAppRequest.TickHistoryButton();

                    }
                }
                var MedicalHxDiseaseDetail = JSON.parse(response.DiseaseFill_JSON);
                var MedicalHxDetail = JSON.parse(response.MedicalHxFill_JSON);
                var ChangedColuns = "";
                if (response.ChangedColumns != undefined) {
                    ChangedColuns = response.ChangedColumns.split(",");
                }
                //MobileAppRequest.params.MedicalHxDiseaseId = MedicalHxDiseaseDetail.DiseaseId;
                //MobileAppRequest.params.MedicalHxId = MedicalHxDetail.MedicalHxId;
                //MobileAppRequest.params.MedicalHxDate = MedicalHxDiseaseDetail.CreatedOn;
                //$.extend(MedicalHxDetail, MedicalHxDiseaseDetail);
                //if (MedicalHxDetail.IsRCPneumococcal == 'False' || MedicalHxDetail.IsRCPneumococcal == "" || MedicalHxDetail.RCPneumococcalDate == "1/1/1753")
                //    MedicalHxDetail.RCPneumococcalDate = "";
                //if (MedicalHxDetail.IsRCInfluenza == 'False' || MedicalHxDetail.IsRCInfluenza== ""|| MedicalHxDetail.RCInfluenzaDate == "1/1/1753")
                //    MedicalHxDetail.RCInfluenzaDate = "";

                //MobileAppRequest.params.MedicalhxDiseaseData = MedicalHxDetail;
                MobileAppRequest.params.MedicalHxDiseaseId = MedicalHxDiseaseDetail.DiseaseId;
                $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail').show();
                var self = $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail');
                utility.bindMyJSONByName(true, MedicalHxDetail, false, self).done(function () {
                    //var IsRCPneumococcal = MobileAppRequest.params.MedicalhxDiseaseData.IsRCPneumococcal;
                    //var IsRCInfluenza = MobileAppRequest.params.MedicalhxDiseaseData.IsRCInfluenza;
                    //if (IsRCInfluenza == "True") {
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkinfluenzavaccineYes').prop("checked", true);
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkinfluenzavaccineNo').prop("checked", false);
                    //}
                    //else  if (IsRCInfluenza == "False")
                    //{
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkinfluenzavaccineNo').prop("checked", true);
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkinfluenzavaccineYes').prop("checked", false);
                    //}
                    //else if (IsRCInfluenza = "")
                    //{
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkinfluenzavaccineYes').prop("checked", false);
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkinfluenzavaccineNo').prop("checked", false);
                    //}
                    //if (IsRCPneumococcal == "True") {
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkpneumoniavaccineYes').prop("checked", true);
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkpneumoniavaccineNo').prop("checked", false);
                    //}
                    //else if (IsRCPneumococcal == "False") {
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkpneumoniavaccineNo').prop("checked", true);
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkpneumoniavaccineYes').prop("checked", false);
                    //}
                    //else if (IsRCPneumococcal == "")
                    //{
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkpneumoniavaccineYes').prop("checked", false);
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #chkpneumoniavaccineNo').prop("checked", false);
                    //}
                    //$('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail').show();
                    //$.each(ChangedColuns, function (i, item) {
                    //    $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail').find("label[Refname=" + item + "]").addClass('changed-field');
                    //});
                    //$('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail').find('label').addClass('changed-field');
                    // High Ligt Changed Columns. 
                    //self.find("#hfMDPatientId").val(MobileAppRequest.params.patientID);
                });
                // MobileAppRequest.HighLightChangedFieldsOfEmergencyContact(MobileAppRequest.params.EmergencyContactData)
            }
        });
    },

    LoadSocialHx: function () {

        $('#pnlMobileAppRequest  #frmMobileAppRequest').resetAllControls();
        $('#' + MobileAppRequest.params.PanelID + " #ddlDrugType,#ddlSexualSTD").multiselect({
            includeSelectAllOption: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247
        });

        CacheManager.BindDropDownsByEntityID('#pnlMobileAppRequest #frmMobileAppRequest #divSociallHx #SexualHxDiv #ddlSexualComplaints', 'GetSexualHxComplaintsWithoutGender', true, 'Male');
        MobileAppRequest.SocialHxLoad(MobileAppRequest.params.patientID, MobileAppRequest.params.Status).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();
                $("#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper").show();
                var AlcoholHxList = JSON.parse(response.AlcoholHxFill_JSON);
                var CaffeineIntakeHxList = JSON.parse(response.CaffeineIntakeHxFill_JSON);
                var DrugAbuseList = JSON.parse(response.DrugAbuseFill_JSON);
                var ExercisesHxList = JSON.parse(response.ExercisesHxFill_JSON);
                var HousingHxList = JSON.parse(response.HousingHxFill_JSON);
                var OccupationHxList = JSON.parse(response.OccupationHxFill_JSON);
                var SexualHxList = JSON.parse(response.SexualHxFill_JSON);
                var SleepHxList = JSON.parse(response.SleepHxFill_JSON);
                var TobaccoHxList = JSON.parse(response.TobaccoHxFill_JSON);
                var TravelHxList = JSON.parse(response.TravelHxFill_JSON);
                var ChangedColuns = response.ChangedColumns.split(",");
                if (TobaccoHxList.length > 0) {
                    $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #TababaccoHxDiv').removeClass('hidden');
                    utility.bindMyJSON(true, TobaccoHxList[0], false, $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #TababaccoHxDiv')).done(function () {
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #TababaccoHxDiv').find('label').addClass('changed-field');
                    });
                }
                if (AlcoholHxList.length > 0) {
                    utility.bindMyJSON(true, AlcoholHxList[0], false, $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #AlcoholHxDiv')).done(function () {
                    });
                }
                if (DrugAbuseList.length > 0) {
                    utility.bindMyJSON(true, DrugAbuseList[0], false, $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #DrugAbuseHxDiv')).done(function () {
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #DrugAbuseHxDiv #ddlDrugType').val(DrugAbuseList[0].ddlDrugType.split(','));
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #DrugAbuseHxDiv #ddlDrugType').multiselect("refresh");
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #DrugAbuseHxDiv #ddlDrugType').multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                    });
                }
                if (SexualHxList.length > 0) {
                    utility.bindMyJSON(true, SexualHxList[0], false, $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SexualHxDiv')).done(function () {
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SexualHxDiv #ddlSexualComplaints').val(SexualHxList[0].ddlSexualComplaints);
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SexualHxDiv #ddlSexualbUsingProtection').val(SexualHxList[0].ddlSexualbUsingProtection);
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SexualHxDiv #ddlSexualProtectionPeriod').val(SexualHxList[0].ddlSexualProtectionPeriod);
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SexualHxDiv #ddlSexualSTD').val(SexualHxList[0].ddlSexualSTD.split(','));
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SexualHxDiv #ddlSexualSTD').multiselect("refresh");
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SexualHxDiv #ddlSexualSTD').multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SexualHxDiv').find("input[name=SocialHxComments]").val(SexualHxList[0].SocialHxComments);
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SexualHxDiv #txtPregnancyDurations').val(SexualHxList[0].PregnancyDuration);
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SexualHxDiv #dtpSexualLMP').val(SexualHxList[0].SexualLMP);
                    });
                }
                if (OccupationHxList.length > 0) {
                    utility.bindMyJSON(true, OccupationHxList[0], false, $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #OccupationHxDiv')).done(function () {
                    });
                    if (OccupationHxList[0].RadPresentNo && OccupationHxList[0].RadPresentNo.toLowerCase() == "true") {
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #OccupationHxDiv #RadPresentNo').prop("checked", "checked");
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #OccupationHxDiv #RadPresentYes').removeAttr("checked", "checked");
                    }
                    else if (OccupationHxList[0].RadPresentNo && OccupationHxList[0].RadPresentNo.toLowerCase() == "false")
                    {
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #OccupationHxDiv #RadPresentYes').prop("checked", "checked");
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #OccupationHxDiv #RadPresentNo').removeAttr("checked", "checked");
                    }
                }
                if (SleepHxList.length > 0) {
                    utility.bindMyJSON(true, SleepHxList[0], false, $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SleepHxDiv')).done(function () {
                    });
                }
                if (ExercisesHxList.length > 0) {
                    utility.bindMyJSON(true, ExercisesHxList[0], false, $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #ExerciseHxDiv')).done(function () {
                    });
                }
                if (HousingHxList.length > 0) {
                    utility.bindMyJSON(true, HousingHxList[0], false, $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #HousingHxDiv')).done(function () {
                    });
                }
                if (CaffeineIntakeHxList.length > 0) {
                    utility.bindMyJSON(true, CaffeineIntakeHxList[0], false, $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #CaffeineIntakeHxDiv')).done(function () {
                    });
                }
                if (TravelHxList.length > 0) {
                    utility.bindMyJSON(true, TravelHxList[0], false, $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #TravelHxDiv')).done(function () {
                    });
                }

                if ($('#pnlMobileAppRequest #btnSocialHx').find('i').length == 0) {

                    $('#pnlMobileAppRequest #btnSocialHx').append('<i class="fa fa-check pull-left"></i>');

                    MobileAppRequest.TickHistoryButton();

                }
                if (ChangedColuns != "") {
                    $.each(ChangedColuns, function (i, item) {
                        if (item != "") {
                            $('#pnlMobileAppRequest #frmMobileAppRequest #DivMainSocialHx').find("label[name=" + item + "]").addClass('changed-field');
                        }
                    });
                }
                //}

            }
        });

    },

    SocialHxLoad: function (PatientID, RequestStatus) {
        //    var data = "PatientId=" + PatientID + "&RequestStatus=" + RequestStatus;
        //// serach parameter , class name, command name of class 
        //return MDVisionService.defaultService(data, "MobileAppRequest", "LOAD_HOSPITALIZATIONHX_DISEASES");
        var objData = new Object();
        objData["PatientId"] = PatientID;
        objData["Status"] = RequestStatus;
        objData["commandType"] = "fill_socialhx_native";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHx");
    },

    InsertSocialHxRecord: function () {
        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var TabbaccoJSON = $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper').getMyJSONByName();
                var AlcoholJSON = $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #AlcoholHxDiv').getMyJSONByName();
                var DrugAbuseJSON = $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #DrugAbuseHxDiv').getMyJSONByName();
                var SexualJSON = $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SexualHxDiv').getMyJSONByName();
                var OccupationJSON = $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #OccupationHxDiv').getMyJSONByName();
                var SleepJSON = $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #SleepHxDiv').getMyJSONByName();
                var ExerciseJSON = $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #ExerciseHxDiv').getMyJSONByName();
                var HousingJSON = $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #HousingHxDiv').getMyJSONByName();
                var CaffeineJSON = $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #CaffeineIntakeHxDiv').getMyJSONByName();
                var TravelJSON = $('#pnlMobileAppRequest #frmMobileAppRequest #divSocialHxWrapper #TravelHxDiv').getMyJSONByName();
                MobileAppRequest.SaveSocialHxRecords(TabbaccoJSON, AlcoholJSON, DrugAbuseJSON, SexualJSON, OccupationJSON, SleepJSON, ExerciseJSON, HousingJSON, CaffeineJSON, TravelJSON).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                        var Index = subtabs.indexOf('SocialHx');
                        subtabs.splice(Index, 1);
                        MobileAppRequest.params.DataSubSection = subtabs.join(',');
                        MobileAppRequest.ShowHistoryButtons();
                    } else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    SaveSocialHxRecords: function (TabbaccoJSON, AlcoholJSON, DrugAbuseJSON, SexualJSON, OccupationJSON, SleepJSON, ExerciseJSON, HousingJSON, CaffeineJSON, TravelJSON) {
        var objData = {};
        objData["PatientId"] = MobileAppRequest.params.patientID;
        objData["IsSynced"] = MobileAppRequest.params.Status;
        objData["SocialHxUnremarkable"] = "false";
        objData["commandType"] = "save_socialhx_native";
        objData["AddedFromMobileApp"] = "1";
        objData["TabbacooData"] = TabbaccoJSON;
        objData["AlcoholData"] = AlcoholJSON;
        objData["DrugAbuseData"] = DrugAbuseJSON;
        objData["SexualData"] = SexualJSON;
        objData["OccupationData"] = OccupationJSON;
        objData["SleepData"] = SleepJSON;
        objData["ExerciseData"] = ExerciseJSON;
        objData["HousingData"] = HousingJSON;
        objData["CaffeineData"] = CaffeineJSON;
        objData["TravelData"] = TravelJSON;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHx");
    },
    InsertMedicalHxDiseaase: function () {
        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail');
                var data = self.getMyJSONByName();
                MobileAppRequest.MedicalHxDiseaseSave(data).done(function (response) {

                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                        $("#pnlMobileAppRequest #divMedicalHxDiseaseList").find("#btnMedicalHxDiseaseId" + MobileAppRequest.params.MedicalHxDiseaseId).remove();
                        MobileAppRequest.LoadMedicalHxDiseases();
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail').hide();

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    MedicalHxDiseaseSave: function (MedicalHxData) {

        // var MedicalDiseaseStatusText = $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseaseStatus :selected').text();
        //objData["MedicalDisease"] = MedicalDiseaseStatusText;
        var objData = JSON.parse(MedicalHxData);
        objData["PatientId"] = MobileAppRequest.params.patientID;
        objData["MedicalhxId"] = MobileAppRequest.params.MedicalHxId;
        objData["MedicalDiseaseTestResultText"] = $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseaseTestResult').find('option:selected').val() == "" ? null : $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseaseTestResult').find('option:selected').text();
        objData["MedicalDiseaseStatusText"] = $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseaseStatus').find('option:selected').val() == "" ? null : $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseaseStatus').find('option:selected').text();
        objData["MedicalDiseaseDurationPeriodText"] = $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseaseDurationPeriod').find('option:selected').val() == "" ? null : $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseaseDurationPeriod').find('option:selected').text();
        objData["MedicalDiseaseSeverityText"] = $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseaseSeverity').find('option:selected').val() == "" ? null : $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseaseSeverity').find('option:selected').text();
        objData["MedicalDiseasePatternText"] = $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseasePattern').find('option:selected').val() == "" ? null : $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseasePattern').find('option:selected').text();
        objData["MedicalDiseaseAgggravatedByText"] = $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseaseAgggravatedBy').find('option:selected').val() == "" ? null : $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail #ddlMedicalDiseaseAgggravatedBy').find('option:selected').text();
        objData["commandType"] = "save_medicalhx";
        objData["MedicalHxUnremarkable"] = "false";
        objData["MedicalHxType"] = "disease";
        objData["AddedFromMobileApp"] = "1";
        objData[""]
        objData["MedicalHxDate"] = MobileAppRequest.params.MedicalHxDate;
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(objData, "HISTORY", "MedicalHx");
    },

    LoadBirthHxByName: function (BirthHx, currentButton) {
        MobileAppRequest.BirthHxLoad(MobileAppRequest.params.patientID, MobileAppRequest.params.Status, BirthHx).done(function (response) {
            if (response.status != false) {
                //code to place ticks on the button
                $("#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxList button").removeClass("active");
                $(currentButton).addClass("active");
                if ($(currentButton).find('i').length == 0) {
                    $(currentButton).append('<i class="fa fa-check pull-left"></i>');
                }
                if ($("#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxList button[style$='display: inline-block;'] .fa-check").length == $("#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxList button[style$='display: inline-block;']").length) {
                    if ($('#pnlMobileAppRequest #btnBirthHx').find('i').length == 0)
                        $('#pnlMobileAppRequest #btnBirthHx').append('<i class="fa fa-check pull-left"></i>');
                    MobileAppRequest.TickHistoryButton();
                }
                //code to place ticks on the button

                $('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();
                $("#pnlMobileAppRequest #frmMobileAppRequest #divBirthHx").show();
                if (BirthHx == "BirthHx_General") {
                    $('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxGeneral').find('label').removeClass('changed-field');
                    var BirthHxGeneralDetail = JSON.parse(response.GeneralFill_JSON);
                    MobileAppRequest.params.BirthHxGeneralData = BirthHxGeneralDetail;
                    var self = $('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxGeneral');
                    utility.bindMyJSONByName(true, MobileAppRequest.params.BirthHxGeneralData, false, self).done(function () {

                        $('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxGeneral').show();
                        $(MobileAppRequest.params.BirthHxGeneralData.lstChangedColumns).each(function (index) {
                            $('label[for= ' + this.columnName + ']').addClass("changed-field");
                        });
                        //$('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxGeneral').find('label').addClass('changed-field');
                    });

                }
                else if (BirthHx == "BirthHx_Newborn") {
                    $('#pnlMobileAppRequest #frmMobileAppRequest #divMedicalHxDetail').find('label').removeClass('changed-field');
                    var NewBornDetail = JSON.parse(response.NewBornFill_JSON);
                    MobileAppRequest.params.NewBornData = NewBornDetail;
                    var self = $('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxBirthHxNewBornInformation');
                    utility.bindMyJSONByName(true, MobileAppRequest.params.NewBornData, false, self).done(function () {
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxBirthHxNewBornInformation').show();
                        $(MobileAppRequest.params.NewBornData.lstChangedColumns).each(function (index) {
                            $('label[for= ' + this.columnName + ']').addClass("changed-field");
                        });
                        //$('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxBirthHxNewBornInformation').find('label').addClass('changed-field');
                        // High Ligt Changed Columns.
                    });
                }
                else if (BirthHx == "BirthHx_MaternalDelivery") {
                    $('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxMaternalDelivery').find('label').removeClass('changed-field');
                    var MaternalDetail = JSON.parse(response.MaternalFill_JSON);
                    MobileAppRequest.params.MaternalData = MaternalDetail;
                    var self = $('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxMaternalDelivery');
                    utility.bindMyJSONByName(true, MobileAppRequest.params.MaternalData, false, self).done(function () {
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxMaternalDelivery').show();
                        $(MobileAppRequest.params.MaternalData.lstChangedColumns).each(function (index) {
                            $('label[for= ' + this.columnName + ']').addClass("changed-field");
                        });
                        //$('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxMaternalDelivery').find('label').addClass('changed-field');
                    });
                }
            }
        });
    },



    BirthHxLoad: function (PatientID, RequestStatus, BirthHxType) {
        var commandType = "";
        if (BirthHxType == "BirthHx_General")
            commandType = "LOAD_General_HISTORY_NATIVE";
        else if (BirthHxType == "BirthHx_Newborn")
            commandType = "LOAD_NEWBORN_HISTORY_NATIVE";
        else if (BirthHxType == "BirthHx_MaternalDelivery")
            commandType = "LOAD_Maternal_HISTORY_NATIVE";
        var data = "PatientId=" + PatientID + "&RequestStatus=" + RequestStatus;

        return MDVisionService.defaultService(data, "MobileAppRequest", commandType);
    },

    BirthHxSave: function (BirthHxType) {
        var commandType = "";
        var objData;

        if (BirthHxType == "BirthHx_General") {
            objData = MobileAppRequest.params.BirthHxGeneralData;
            objData["IsGeneralUpdate"] = "true";


            if (MobileAppRequest.params.BirthHxGeneralData.BirthHxId != "-1")
                objData["commandType"] = "update_birthhx";
            else
                objData["commandType"] = "save_birthhx";

        }
        else if (BirthHxType == "BirthHx_Newborn") {
            objData = MobileAppRequest.params.NewBornData;
            objData["IsNewbornUpdate"] = "true";

            if (MobileAppRequest.params.NewBornData.bFetalDistress = "1") objData["bFetalDistress"] = "true";
            else objData["bFetalDistress"] = "false";
            objData["bFetalDistress"] = MobileAppRequest.params.NewBornData.bFetalDistress = "1" ? "true" : "false";

            if (MobileAppRequest.params.NewBornData.BirthHxId != "-1")
                objData["commandType"] = "update_birthhx";
            else
                objData["commandType"] = "save_birthhx";

        }
        else if (BirthHxType == "BirthHx_MaternalDelivery") {
            objData = MobileAppRequest.params.MaternalData;
            objData["IsDeliveryUpdate"] = "true";
            if (MobileAppRequest.params.MaternalData.BirthHxId != "-1")
                objData["commandType"] = "update_birthhx";
            else
                objData["commandType"] = "save_birthhx";

        }

        objData["AddFromMobile"] = "1";
        objData["BirthHxUnremarkable"] = "false";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "birthHistory");
    },
    SaveBirthHx: function (BirthHxType) {
        var strMessage = "";

        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                MobileAppRequest.BirthHxSave(BirthHxType).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                        Clinical_BirthHx.bIsFirstLoad = true;
                        var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                        var Index = subtabs.indexOf(BirthHxType);
                        subtabs.splice(Index, 1);
                        MobileAppRequest.params.DataSubSection = subtabs.join(',');
                        MobileAppRequest.ShowBirthHistoryButtons();

                        if (BirthHxType == "BirthHx_General") {
                            $('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxGeneral').hide();
                        }
                        else if (BirthHxType == "BirthHx_Newborn") {
                            $('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxBirthHxNewBornInformation').hide();
                        }
                        else if (BirthHxType == "BirthHx_MaternalDelivery") {
                            $('#pnlMobileAppRequest #frmMobileAppRequest #divBirthHxMaternalDelivery').hide();
                        }

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    TickMainFaimlyHxbtn: function () {
        if ($("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHxMembersList button .fa-check").length == $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHxMembersList button ").length) {
            if ($("#pnlMobileAppRequest #frmMobileAppRequest #btnFamilyHx").find('i').length == 0)
                $("#pnlMobileAppRequest #frmMobileAppRequest #btnFamilyHx").append('<i class="fa fa-check pull-left"></i>');

            MobileAppRequest.TickHistoryButton();
        }

    },

    LoadFamilyHxDiseases: function () {

        MobileAppRequest.FamilyHxMemberLoad(MobileAppRequest.params.patientID, MobileAppRequest.params.Status).done(function (response) {
            if (response.status != false) {
                var FamilyMemberHxList = JSON.parse(response.FamilyMemberHxLoad_JSON);
                if (MobileAppRequest.isFamilyHXFirstLoad == true) {
                    $("#pnlMobileAppRequest #divFamilyHxMembersList").html('');
                    $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHxDiseaseList").hide();
                    if (FamilyMemberHxList.length > 0) {
                        $('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();
                        $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx").show();
                        var familyMemberCount = 0;
                        $.each(FamilyMemberHxList, function (index, value) {
                            var ButtonId = index + 1;
                            var $Button = $('<Button class="btn btn-default btn-sm tab_space"data-DiseaseCount="">' + value.Description + '</button>');
                            $Button.attr("id", "btnFamilyHxMemberId" + ButtonId + "");
                            var familyMemberDescriptin = value.Description
                            $Button.attr("onclick", "MobileAppRequest.LoadFamilyHxMemberDiseases(" + value.FamilyMemberId + " , this );");

                            if ($("#pnlMobileAppRequest #frmMobileAppRequest #btnFamilyHx").find('i').length > 0)
                                $Button.append('<i class="fa fa-check pull-left"></i>');

                            $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #divFamilyHxMembersList").append($Button);
                            var name = value.Description.replace(/\s/g, "");
                            $Button.attr("name", "" + name + "");
                            familyMemberCount++;
                            if (familyMemberCount == 1) {
                                $("#pnlMobileAppRequest #frmMobileAppRequest #btnFamilyHxMemberId" + ButtonId).click();
                            }
                        });

                    }
                    else {
                        var subtabs = MobileAppRequest.params.DataSubSection.split(',');

                        var Index = subtabs.indexOf('FamilyHx');
                        subtabs.splice(Index, 1);
                        MobileAppRequest.params.DataSubSection = subtabs.join(',');
                        MobileAppRequest.ShowHistoryButtons();
                    }
                    MobileAppRequest.isFamilyHXFirstLoad = false;
                }
                else {
                    $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx").show();
                    $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx").find('button')[0].click();
                }
            }

        });
    },



    LoadFamilyHxMemberDiseases: function (FamilyMemberId, familyMemberbtn) {
        $("#pnlMobileAppRequest #divFamilyHxMembersList ").find('button').removeClass('active');
        $(familyMemberbtn).addClass('active');
        MobileAppRequest.FamilyHxDiseasesLoad(MobileAppRequest.params.patientID, FamilyMemberId, MobileAppRequest.params.Status).done(function (response) {
            if (response.status != false) {
                var FamilyMemberDiseasesHxList = JSON.parse(response.FamilyMemberDiseasesHxLoad_JSON);
                var DiseaseCount = 0;
                $("#pnlMobileAppRequest #divFamilyHxDiseaseList div").css("display", "none");
                if (response.FamilyMemberDiseasesCount > 0) {
                    $('#' + MobileAppRequest.params["PanelID"] + ' div[name="MobileApp_Module"]').hide();
                    $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx").show();
                    $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHxDiseaseList").show();

                    if ($("#pnlMobileAppRequest #divFamilyHxDiseaseList ." + familyMemberbtn.name).length > 0) {


                        var ButtonAprrove = $("#pnlMobileAppRequest #frmMobileAppRequest #btnFamilyHxApprove");
                        ButtonAprrove.attr("name", "" + familyMemberbtn.name + "");
                        var ButtonDiscard = $("#pnlMobileAppRequest #frmMobileAppRequest #btnFamilyHxDiscard");
                        ButtonDiscard.attr("name", "" + familyMemberbtn.name + "");

                        $("#pnlMobileAppRequest #divFamilyHxDiseaseList ." + familyMemberbtn.name).css("display", "inline-block");
                        $($("#pnlMobileAppRequest #divFamilyHxDiseaseList ." + familyMemberbtn.name)[0]).find('button')[0].click();


                    }
                    else {
                        var $diseasediv = $('<div ></div>')
                        $diseasediv.attr("class", familyMemberbtn.name);
                        $diseasediv.attr("id", familyMemberbtn.name);
                        $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #divFamilyHxDiseaseList").append($diseasediv);

                        $.each(FamilyMemberDiseasesHxList, function (index, value) {
                            var ButtonId = index + 1;
                            var $Button = $('<Button class="btn btn-link btn-sm btn-pipe text-bold">' + value.ICD10CodeDescription + '</button>');
                            $Button.attr("id", "btnFamilyHxMemberDiseasId" + ButtonId + familyMemberbtn.name);
                            $Button.attr("onclick", "MobileAppRequest.LoadFamilyHxDiseaseById(" + value.FamilyMemberId + " , " + value.DiseaseId + ", this );");

                            $diseasediv.append($Button);
                            var name = familyMemberbtn.name.replace(/\s/g, "");
                            $Button.attr("name", "" + name + "");


                            var ButtonAprrove = $("#pnlMobileAppRequest #frmMobileAppRequest #btnFamilyHxApprove");
                            ButtonAprrove.attr("name", "" + name + "");
                            var ButtonDiscard = $("#pnlMobileAppRequest #frmMobileAppRequest #btnFamilyHxDiscard");
                            ButtonDiscard.attr("name", "" + name + "");
                            DiseaseCount++;

                            if (DiseaseCount == 1) {
                                $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #divFamilyHxDiseaseList  #" + familyMemberbtn.name + ' #btnFamilyHxMemberDiseasId' + ButtonId + familyMemberbtn.name).click();
                            }
                            $(familyMemberbtn).attr('data-DiseaseCount', DiseaseCount);

                        });
                    }


                }
                else {
                    //var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                    //var Index = subtabs.indexOf('FamilyHx');
                    //subtabs.splice(Index, 1);
                    //MobileAppRequest.params.DataSubSection = subtabs.join(',');
                    //MobileAppRequest.ShowHistoryButtons();
                }

            }

            else {
                utility.DisplayMessages(response.Message, 3);

            }



        });

    },



    LoadFamilyHxDiseaseById: function (FamilyMemberId, DiseaseId, currentButton) {
        $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHxDiseaseList ").find('button').removeClass('underline');
        $(currentButton).addClass('underline');
        MobileAppRequest.FamilyHxDiseasesLoad(MobileAppRequest.params.patientID, FamilyMemberId, MobileAppRequest.params.Status, DiseaseId).done(function (response) {
            if (response.status != false) {

                var DiseasebuttonId = currentButton.id;
                $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #hfbtnDiseaseID").val(DiseasebuttonId);

                if ($(currentButton).find('i').length == 0) {
                    $(currentButton).append('<i class="fa fa-check pull-left"></i>');

                }

                var divDiseaseButton = $("#pnlMobileAppRequest #divFamilyHxDiseaseList #" + DiseasebuttonId).parent()[0];
                var checkedDiseasCount = $(divDiseaseButton).find("button").find('i').length;
                var TotallDiseasCount = $(divDiseaseButton).find("button").length;
                if (checkedDiseasCount == TotallDiseasCount) {
                    if ($("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #divFamilyHxMembersList button[name=" + currentButton.name + "]").find('i').length == 0)
                        $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #divFamilyHxMembersList button[name=" + currentButton.name + "]").append('<i class="fa fa-check pull-left"></i>');
                    MobileAppRequest.TickMainFaimlyHxbtn();
                }



                var FamilyMemberDiseasesDetail = JSON.parse(response.FamilyMemberDiseasesHxLoad_JSON);
                $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHxDetail").show();
                var healthstatusId = FamilyMemberDiseasesDetail[0].HealthStatusId;
                $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #ddlHealthStatus').val(healthstatusId);
                $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #dtpYearofBirth').val(FamilyMemberDiseasesDetail[0].YearofBirth);
                $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #txtAgeAtDeath').val(FamilyMemberDiseasesDetail[0].AgeAtDeath);
                $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #txtAgeAtDiagnosis').val(FamilyMemberDiseasesDetail[0].AgeAtDiagnosis);
                $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #txtFamilyMemberComments').val(FamilyMemberDiseasesDetail[0].FamilyMemberComments);
                $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #txtDiagnosis').val(FamilyMemberDiseasesDetail[0].FreeTextICD);
                if (FamilyMemberDiseasesDetail[0].RadRelativeDied.toLowerCase() == "false" || FamilyMemberDiseasesDetail[0].RadRelativeDied.toLowerCase() == "no") {
                    $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #RadYesRelativeDied').prop('checked', false);
                    $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #RadNoRelativeDied').prop('checked', true);
                } else if (FamilyMemberDiseasesDetail[0].RadRelativeDied.toLowerCase() == "true" || FamilyMemberDiseasesDetail[0].RadRelativeDied.toLowerCase() == "yes") {
                    $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #RadYesRelativeDied').prop('checked', true);
                    $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #RadNoRelativeDied').prop('checked', false);
                } else {
                    $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #RadYesRelativeDied').prop('checked', false);
                    $('#pnlMobileAppRequest #frmMobileAppRequest  #divFamilyHxDetail #RadNoRelativeDied').prop('checked', false);
                }
                MobileAppRequest.params.FamilyMemberDiseasesDetail = FamilyMemberDiseasesDetail[0];
                MobileAppRequest.params.FamilyHxDiseasesId = FamilyMemberDiseasesDetail[0].DiseaseId;

            }
        });

    },
    FamilyHxMemberLoad: function (PatientID, RequestStatus) {
        var data = "PatientId=" + PatientID + "&RequestStatus=" + RequestStatus;
        return MDVisionService.defaultService(data, "MobileAppRequest", "Load_FamilyHX_MEMBERS");
    },

    FamilyHxDiseasesLoad: function (PatientID, FamilyMemberId, RequestStatus, DiseaseId) {
        if (DiseaseId != undefined)
            var data = "PatientId=" + PatientID + "&RequestStatus=" + RequestStatus + "&FamilyMemberId=" + FamilyMemberId + "&DiseaseID=" + DiseaseId;
        else
            var data = "PatientId=" + PatientID + "&RequestStatus=" + RequestStatus + "&FamilyMemberId=" + FamilyMemberId;
        return MDVisionService.defaultService(data, "MobileAppRequest", "Load_FamilyHX_DISEASES");
    },

    InsertFamilyHxDiseaase: function () {
        var strMessage = "";
        var self = $('#pnlMobileAppRequest');
        var ButtonAprrove = $("#pnlMobileAppRequest #frmMobileAppRequest #btnFamilyHxApprove");
        var RelationName = ButtonAprrove[0].name;
        var FamilyMemberButton = $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #divFamilyHxMembersList button[name=" + RelationName + "]");

        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                MobileAppRequest.FamilyHxDiseaseSave(MobileAppRequest.params.FamilyMemberDiseasesDetail).done(function (response) {

                    if (response.status != false) {
                        Clinical_FamilyHx.bIsFirstLoad = true;
                        utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                        var ApprovedDiseaseID = $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #hfbtnDiseaseID").val();
                        $('#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHxDetail').hide();
                        if (ApprovedDiseaseID != null || ApprovedDiseaseID != "undefiend") {
                            var parentDiv = $("#pnlMobileAppRequest #divFamilyHxDiseaseList #" + ApprovedDiseaseID).parent()[0];
                            var FmDiseaseCount = $(parentDiv).find("button").length;
                            $("#pnlMobileAppRequest #divFamilyHxDiseaseList #" + ApprovedDiseaseID).remove();

                            if (FmDiseaseCount == 1) {
                                var familyMembersbtn = $(FamilyMemberButton.parent()[0]).find("button");

                                var familyMembersCount = familyMembersbtn.length;
                                FamilyMemberButton.remove();
                                parentDiv.remove();
                                if (familyMembersCount <= 1) {
                                    $("#divFamilyHxMembersList").html('');
                                    $("#pnlMobileAppRequest #btnFamilyHx").hide();

                                    var subtabs = MobileAppRequest.params.DataSubSection.split(',');
                                    var Index = subtabs.indexOf('FamilyHx');
                                    subtabs.splice(Index, 1);
                                    MobileAppRequest.params.DataSubSection = subtabs.join(',');
                                    MobileAppRequest.ShowHistoryButtons();

                                }

                                else {
                                    $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #divFamilyHxMembersList").find("button")[0].click();
                                }

                            }
                            else {
                                $(parentDiv).find("button")[0].click();
                            }
                        }


                        else {
                            //  $("#pnlMobileAppRequest #frmMobileAppRequest #divFamilyHx #divFamilyHxMembersList").find("button")[0].remove();
                        }


                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FamilyHxDiseaseSave: function (FamilyHxData) {
        var objData = FamilyHxData;
        objData["PatientId"] = MobileAppRequest.params.patientID;
        if (objData.FamilyHxId != "-1" && objData.FamilyHxId != undefined)
            objData["commandType"] = "update_familyhx";
        else
            objData["commandType"] = "SAVE_FamilyHx";
        objData["FamilyHxType"] = "disease";
        objData["FamilyHxUnremarkable"] = "false";
        objData["HealthStatus"] = objData.HealthStatusId;
        objData["AddedFromMobileApp"] = "1";
        objData["FamilyHxDate"] = objData.CreatedOn;
        objData["MemberId"] = objData.FamilyMemberId;
        objData["MemberDetailId"] = "-2";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "FamilyHx");

    },

    ShowButtonsOfApprovalAllAndDiscardAll: function () {



        if (MobileAppRequest.IsEverythingChecked == false) {

            if (MobileAppRequest.SectionButtonsCount == MobileAppRequest.SectionButtonsChecked) {
                var status = MobileAppRequest.params.Status;
                if (status == "Pending") {
                    //$('#pnlMobileAppRequest #btnApproveAll').removeClass('hidden');
                    //$('#pnlMobileAppRequest #btnDiscardAll').removeClass('hidden');
                }
                else if (status == "Discard") {
                    //$('#pnlMobileAppRequest #btnApproveAll').removeClass('hidden');
                }


                MobileAppRequest.IsEverythingChecked = true;
                MobileAppRequest.SectionButtonsChecked = 0;
            }
            else {
                //$('#pnlMobileAppRequest #btnApproveAll').addClass('hidden');
                //$('#pnlMobileAppRequest #btnDiscardAll').addClass('hidden');

            }
        }
        else {
            if (status == "Pending") {
                //$('#pnlMobileAppRequest #btnApproveAll').removeClass('hidden');
                //$('#pnlMobileAppRequest #btnDiscardAll').removeClass('hidden');
            }
            else if (status == "Discard") {
                //$('#pnlMobileAppRequest #btnApproveAll').removeClass('hidden');
            }
        }

    },

    ApproveAll: function () {

        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                MobileAppRequest.ApproveAllInDB().done(function (response) {

                    if (response.status != false) {
                        if (response.message == "") {
                            utility.DisplayMessages(MobileAppRequest.ApproveMessage, 1);
                            MobileAppRequest.UnLoad();
                        }
                        else
                            utility.DisplayMessages(response.Message, 3);

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);

                    }

                });

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    ApproveAllInDB: function () {

        //   var dfd = new $.Deferred();        
        var data = "";
        var allSubTabs = MobileAppRequest.params.DataSubSection;
        if (allSubTabs.indexOf("Patients") >= 0) {
            var PaientDemographic = MobileAppRequest.params.Demographic_Data;
            data = "PatientID=" + MobileAppRequest.params.patientID + "&AccountNo=" + PaientDemographic.AccountNo + "&FirstName=" + PaientDemographic.FirstName +
                       "&MI=" + PaientDemographic.MI + "&LastName=" + PaientDemographic.LastName + "&DOB=" + PaientDemographic.DOB
                       + "&Gender=" + PaientDemographic.Gender + "&Address1=" + PaientDemographic.Address1 + "&City=" + PaientDemographic.City +
                       + "&State=" + PaientDemographic.State + "&ZipCode=" + PaientDemographic.ZipCode + "&IsDemographicExist=" + 1;

        }

        else {
            data = "PatientID=" + MobileAppRequest.params.patientID + "&IsDemographicExist=" + 0
        }
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "MobileAppRequest", "APPROVE_ALL_RECORD");

        //  return dfd.promise();
    },
    DiscardAllInDB: function () {

        //   var dfd = new $.Deferred();

        var data = "PatientID=" + MobileAppRequest.params.patientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "MobileAppRequest", "DISCARD_ALL_RECORD");

        //  return dfd.promise();
    },

    DiscardAll: function () {

        AppPrivileges.GetFormPrivileges("Live Requests", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                MobileAppRequest.DiscardAllInDB().done(function (response) {

                    if (response.status != false) {

                        if (response.message == "") {
                            utility.DisplayMessages(MobileAppRequest.DiscardMessage, 1);
                            MobileAppRequest.UnLoad();
                        }
                        else
                            utility.DisplayMessages(response.Message, 3);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    enableDisableCounsellingTopic: function (sender) {

        if (sender == "tobacco") {
            var selectedVal = $('#' + MobileAppRequest.params.PanelID + " #TababaccoHxDiv #ddlTobaccoCounsellingPeriod").val();
            if (selectedVal == "" || selectedVal == "4") {
                $('#' + MobileAppRequest.params.PanelID + " #TababaccoHxDiv #ddlTobaccoCounsellingTopic").addClass("disableAll");
                $('#' + MobileAppRequest.params.PanelID + " #TababaccoHxDiv #ddlTobaccoCounsellingTopic").val($('#' + MobileAppRequest.params.PanelID + " #TababaccoHxDiv #ddlTobaccoCounsellingTopic option:first").val());
            }
            else {
                $('#' + MobileAppRequest.params.PanelID + " #TababaccoHxDiv #ddlTobaccoCounsellingTopic").removeClass("disableAll");
            }
        }
        else if (sender == "alcohol") {
            var selectedVal = $('#' + MobileAppRequest.params.PanelID + " #TababaccoHxDiv #ddlAlcoholCounsellingPeriod").val();
            if (selectedVal == "" || selectedVal == "4") {
                $('#' + MobileAppRequest.params.PanelID + " #TababaccoHxDiv #ddlAlcoholCounsellingTopic").addClass("disableAll");
                $('#' + MobileAppRequest.params.PanelID + " #TababaccoHxDiv #ddlAlcoholCounsellingTopic").val($('#' + MobileAppRequest.params.PanelID + " #TababaccoHxDiv #ddlAlcoholCounsellingTopic option:first").val());
            }
            else {
                $('#' + MobileAppRequest.params.PanelID + " #TababaccoHxDiv #ddlAlcoholCounsellingTopic").removeClass("disableAll");
            }
        }
    },


}