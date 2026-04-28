

// Cache Manager variable 
var CPTCodes = []; // array is created
var InsurancePlans = [];
var Employers = [];
var EligibilityInsurance = [];
var EDISubmitInsurance = [];
var Lawyers = [];
var Providers = [];
var GetBillingProviders = [];
var Facilities = [];
var Specialities = [];
var SpecialitiesReferals = [];
var SpecialitiesDescription = [];
var FacilitiesOutgoingReferral = [];
var RefProviders = [];
var BlockReasons = [];
var Guarantors = [];
var Schools = [];
var Practices = [];
var Resources = [];
var ScheduleGroup = [];
var PatientCase = [];
var Modifiers = [];
var ClearingHouse = [];
var PaymentBatch = [];
var PatientReferral = [];
var PatientType = [];
var PatientVistType = [];
var Batches = [];
var AllSubmitStatus = [];
var SubmitStatus = [];
var SecurityQuestions = [];
var PatientPAN = [];
var Users = [];
var TimeZons = [];
var PrefLanguages = [];
var Labs = [];
var Pharmacy = [];
var TherapeuticInjection = [];
var RefProvidersOutgoing = [];
var AllProviders = [];
var CaseAdjuster = [];
var EntityProviders = [];
var NoteComponents = [];
var NoteSections = [];
var arrAdminInsurances = [];
var GetRegistery = [];
var GetRegistrySubmission = [];
var GetUsersFullName = [];
var Countries = [];
var DocumentPriorities = [];
var AppointmentStatusWorkFlow = [];
var IncomingReferralsStatusReasons = [];
/////
CacheManager = {

    //BindCPTCodes: function (methodName, reload) {
    //    if (CPTCodes.length == 0 || reload == true) {
    //        return CacheManager.GetData(methodName, reload).done(function (result) {

    //            // if (response.CPTCodeCount > 0) {
    //            if (reload == true)
    //                CPTCodes.length = 0;

    //            //var CPTCodeLoadJSONData = JSON.parse(result);
    //            $.each(result, function (i, item) {

    //                CPTCodes.push(item.Name); //add item to an array
    //            });


    //        });
    //    }
    //    else {
    //        var results = store.fetch(methodName);
    //        return results;
    //    }

    //},

    BindCodesWithEntityId: function (methodName, reload, entityID) {
        BackgroundLoaderShow(true);
        var data = "entityID=" + entityID
        return MDVisionService.lookups(methodName, reload, data).done(function (result) {
            if (!$.isArray(result) && result[methodName] != null) {
                result = JSON.parse(result[methodName]);

            } else if ($.type(result) == "string") {
                result = JSON.parse(result);
            }
            if (methodName == 'GetProviderEntityBased') {
                if (EntityProviders.length == 0 || reload == true) {
                    if (reload == true)
                        EntityProviders.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            EntityProviders.push({ id: item.Value, value: item.Name, Refvalue: item.Refvalue, Refname: item.Refname, SpecialityName: item.ExValue }); //add item to an array
                    });
                }
            }
            BackgroundLoaderShow(false);
        });

    },

    BindCodes: function (methodName, reload) {
        BackgroundLoaderShow(true);
        return MDVisionService.lookups(methodName, reload).done(function (result) {
            if (!$.isArray(result) && result[methodName] != null) {
                result = JSON.parse(result[methodName]);

            } else if ($.type(result) == "string") {
                result = JSON.parse(result);
            }
            //if (methodName == 'GetCPTCode') {
            //    if (CPTCodes.length == 0 || reload == true) {
            //        if (reload == true)
            //            CPTCodes.length = 0;

            //        $.each(result, function (i, item) {
            //            if (item.Name != "- SELECT -")
            //                // Autocomplete plugin always expects id, value parameters in Array Collection
            //                CPTCodes.push({ id: item.Value, value: item.Name }); //add item to an array
            //        });
            //    }
            //}
            //else
            if (methodName == 'GetInsurancePlan') {
                if (InsurancePlans.length == 0 || reload == true) {
                    if (reload == true)
                        InsurancePlans.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            InsurancePlans.push({ id: item.Value, value: item.Name, searchPattern: item.RefValue, IPDescription: item.RefName, Isreferral: item.IsReferral }); //add item to an array
                    });
                }
            }
            if (methodName == 'GetInsurance') {
                if (arrAdminInsurances.length == 0 || reload == true) {
                    if (reload == true)
                        arrAdminInsurances.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            arrAdminInsurances.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetEmployer') {
                if (Employers.length == 0 || reload == true) {
                    if (reload == true)
                        Employers.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Employers.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetEDISubmitInsurance') {
                if (EDISubmitInsurance.length == 0 || reload == true) {
                    if (reload == true)
                        EDISubmitInsurance.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            EDISubmitInsurance.push({ id: item.Value, value: item.Name, payorId: item.RefValue }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetEDIEligibilityInsurance') {
                if (EligibilityInsurance.length == 0 || reload == true) {
                    if (reload == true)
                        EligibilityInsurance.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            EligibilityInsurance.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetLawyer') {
                if (Lawyers.length == 0 || reload == true) {
                    if (reload == true)
                        Lawyers.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Lawyers.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetProvider') {
                if (Providers.length == 0 || reload == true) {
                    if (reload == true)
                        Providers.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Providers.push({ id: item.Value, value: item.Name, Refvalue: item.Refvalue, Refname: item.Refname, SpecialityName: item.ExValue }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetRegistery') {
                if (GetRegistery.length == 0 || reload == true) {
                    if (reload == true)
                        GetRegistery.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            GetRegistery.push({ id: item.Value, value: item.Name, Refvalue: item.Refvalue, Refname: item.Refname, SpecialityName: item.ExValue }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetRegistrySubmission') {
                if (GetRegistrySubmission.length == 0 || reload == true) {
                    if (reload == true)
                        GetRegistrySubmission.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            GetRegistrySubmission.push({ id: item.Value, value: item.Name, Refvalue: item.Refvalue, Refname: item.Refname, SpecialityName: item.ExValue }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetProviderEntityBased') {
                if (Providers.length == 0 || reload == true) {
                    if (reload == true)
                        Providers.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Providers.push({ id: item.Value, value: item.Name, Refvalue: item.Refvalue, Refname: item.Refname, SpecialityName: item.ExValue }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetBillingProviders') {
                if (GetBillingProviders.length == 0 || reload == true) {
                    if (reload == true)
                        GetBillingProviders.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            GetBillingProviders.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetFacility') {
                if (Facilities.length == 0 || reload == true) {
                    if (reload == true)
                        Facilities.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Facilities.push({ id: item.Value, value: item.Name, PracticeId: item.RefValue, Practice: item.RefName, FacilityDescription: item.FacilityDescription }); //add item to an array
                    });
                }
            }

            else if (methodName == 'GetSpecialtiesAllEntities') {
                if (Specialities.length == 0 || reload == true) {
                    if (reload == true)
                        Specialities.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Specialities.push({ id: item.Value, value: item.Name, PracticeId: item.RefValue, Practice: item.RefName }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetSpecialtiesAllEntitiesReferals') {
                if (SpecialitiesReferals.length == 0 || reload == true) {
                    if (reload == true)
                        SpecialitiesReferals.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            SpecialitiesReferals.push({ id: item.Value, value: item.Name, PracticeId: item.RefValue, Practice: item.RefName }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetSpecialtyDescription') {
                if (SpecialitiesDescription.length == 0 || reload == true) {
                    if (reload == true)
                        SpecialitiesDescription.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            SpecialitiesDescription.push({ id: item.Value, value: item.Name, PracticeId: item.RefValue, Practice: item.RefName }); //add item to an array
                    });
                }
            }
                //Begin Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277
            else if (methodName == 'GetFacilityOutgoingReferral') {
                if (FacilitiesOutgoingReferral.length == 0 || reload == true) {
                    if (reload == true)
                        FacilitiesOutgoingReferral.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            FacilitiesOutgoingReferral.push({ id: item.Value, value: item.Name, PracticeId: item.RefValue, Practice: item.RefName }); //add item to an array
                    });
                }
            }
                //Begin Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277
            else if (methodName == 'GetRefProviders') {
                if (RefProviders.length == 0 || reload == true) {
                    if (reload == true)
                        RefProviders.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            RefProviders.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetBlockReasons') {
                if (BlockReasons.length == 0 || reload == true) {
                    if (reload == true)
                        BlockReasons.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            BlockReasons.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetGuarantor') {
                if (Guarantors.length == 0 || reload == true) {
                    if (reload == true)
                        Guarantors.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Guarantors.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetSchool') {
                if (Schools.length == 0 || reload == true) {
                    if (reload == true)
                        Schools.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Schools.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetPractice') {
                if (Practices.length == 0 || reload == true) {
                    if (reload == true)
                        Practices.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Practices.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetResources') {
                if (Resources.length == 0 || reload == true) {
                    if (reload == true)
                        Resources.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Resources.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetScheduleGroup') {
                if (ScheduleGroup.length == 0 || reload == true) {
                    if (reload == true)
                        ScheduleGroup.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            ScheduleGroup.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetModifier') {
                if (Modifiers.length == 0 || reload == true) {
                    if (reload == true)
                        Modifiers.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Modifiers.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetClearingHouse') {
                if (ClearingHouse.length == 0 || reload == true) {
                    if (reload == true)
                        ClearingHouse.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            ClearingHouse.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetPatientType') {
                if (PatientType.length == 0 || reload == true) {
                    if (reload == true)
                        PatientType.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            PatientType.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetPatientVisitType') {
                if (PatientVistType.length == 0 || reload == true) {
                    if (reload == true)
                        PatientVistType.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            PatientVistType.push({ id: item.Value, value: item.Name, PatientType: item.RefValue }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetPaymentBatch') {
                if (PaymentBatch.length == 0 || reload == true) {
                    if (reload == true)
                        PaymentBatch.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            PaymentBatch.push({ id: item.Value, value: item.Name, CheckNumber: item.RefValue, CheckDate: item.RefName }); //add item to an array
                    });
                }
            }


            else if (methodName == 'GetPatientReferral') {
                if (PatientReferral.length == 0 || reload == true) {
                    if (reload == true)
                        PatientReferral.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            PatientReferral.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetChargeBatches') {
                if (Batches.length == 0 || reload == true) {
                    if (reload == true)
                        Batches.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Batches.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetSubmitStatus') {
                if (SubmitStatus.length == 0 || reload == true) {
                    if (reload == true)
                        SubmitStatus.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            SubmitStatus.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetAllSubmitStatus') {
                if (AllSubmitStatus.length == 0 || reload == true) {
                    if (reload == true)
                        AllSubmitStatus.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "All")
                            AllSubmitStatus.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetSecurityQuestions') {
                if (SecurityQuestions.length == 0 || reload == true) {
                    if (reload == true)
                        SecurityQuestions.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            SecurityQuestions.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetUsers') {
                if (Users.length == 0 || reload == true) {
                    if (reload == true)
                        Users.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Users.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetUsersFullName') {
                if (GetUsersFullName.length == 0 || reload == true) {
                    if (reload == true)
                        GetUsersFullName.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            GetUsersFullName.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetPatientPAN') {
                if (PatientPAN.length == 0 || reload == true) {
                    if (reload == true)
                        PatientPAN.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            PatientPAN.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetTimeZones') {
                if (TimeZons.length == 0 || reload == true) {
                    if (reload == true)
                        TimeZons.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            TimeZons.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetLabs') {
                if (Labs.length == 0 || reload == true) {
                    if (reload == true)
                        Labs.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Labs.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
                //Start 07-09-2016 Humaira Yousaf to get pharmacy list
            else if (methodName == 'GetPharmacy') {
                if (Pharmacy.length == 0 || reload == true) {
                    if (reload == true)
                        Pharmacy.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Pharmacy.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetTherapeuticInjection') {
                if (TherapeuticInjection.length == 0 || reload == true) {
                    if (reload == true)
                        TherapeuticInjection.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            TherapeuticInjection.push({ id: item.Value, value: item.Name, Refvalue: item.Refvalue, Refname: item.Refname, SpecialityName: item.ExValue }); //add item to an array
                    });
                }
            }
                //End 07-09-2016 Humaira Yousaf to get pharmacy list
                // Start 22-12-2016 Humaira Yousaf to get outgoing referral providers
            else if (methodName == 'GetRefProvidersOutgoingReferral') {
                if (RefProvidersOutgoing.length == 0 || reload == true) {
                    if (reload == true)
                        RefProvidersOutgoing.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            RefProvidersOutgoing.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
                // End 21-12-2016 Humaira Yousaf to get outgoing referral providers
            else if (methodName == 'GetAllProviders') {
                if (AllProviders.length == 0 || reload == true) {
                    if (reload == true)
                        AllProviders.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            AllProviders.push({ id: item.Value, value: item.Name, Refvalue: item.Refvalue, Refname: item.Refname, SpecialityName: item.ExValue }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetNoteComponents') {
                if (NoteComponents.length == 0 || reload == true) {
                    if (reload == true)
                        NoteComponents.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            NoteComponents.push({ id: item.Value, value: item.Name, PracticeId: item.RefValue, Practice: item.RefName, FacilityDescription: item.FacilityDescription }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetNoteSections') {
                if (NoteSections.length == 0 || reload == true) {
                    if (reload == true)
                        NoteSections.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            NoteSections.push({ id: item.Value, value: item.Name, SectionMarkup: item.RefValue }); //add item to an array
                    });
                }
            }
            else if (methodName == 'GetDocumentPriority') {
                if (DocumentPriorities.length == 0 || reload == true) {
                    if (reload == true)
                        DocumentPriorities.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            DocumentPriorities.push({ id: item.Value, value: item.Name }); //add item to an array
                    });
                }
            }
            if (methodName == 'GetLanguages') {
                if (PrefLanguages.length == 0 || reload == true) {
                    if (reload == true)
                        PrefLanguages.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            PrefLanguages.push({ id: item.Value, value: item.Name, searchPattern: item.RefValue }); //add item to an array
                    });
                }
            }
            if (methodName == 'GetCountries') {
                if (Countries.length == 0 || reload == true) {
                    if (reload == true)
                        Countries.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            Countries.push({ id: item.Value, value: item.Name, searchPattern: item.RefValue }); //add item to an array
                    });
                }
            }
            if (methodName == 'GetCaseAdjuster') {
                if (CaseAdjuster.length == 0 || reload == true) {
                    if (reload == true)
                        CaseAdjuster.length = 0;

                    $.each(result, function (i, item) {
                        if (item.Name.toUpperCase() != "- SELECT -")
                            CaseAdjuster.push({ id: item.Value, value: item.Name, searchPattern: item.Value }); //add item to an array
                    });
                }
            }
            BackgroundLoaderShow(false);
        });

    },
    BindDropDownsByEntityID: function (Crtl, methodName, reload, entityID) {
        BackgroundLoaderShow(true);
        var data = "entityID=" + entityID


        return MDVisionService.lookups(methodName, reload, data).done(function (result) {
            if ($(Crtl).length > 0)
                l = $(Crtl);

            l.empty();
            if (!$.isArray(result) && result[methodName] != null) {
                result = JSON.parse(result[methodName]);

            } else if ($.type(result) == "string") {
                result = JSON.parse(result);
            }

            if (methodName.toLowerCase() == "getreminderstemplatetype") {
                $.each(result, function (j, item) {
                    l.append($("<option />").val(item.Value).text(item.Name).attr("RefValue", item.RefValue));
                });
            } else {
                $.each(result, function (j, item) {
                    if (entityID == null)
                        l.append($("<option />").val(item.Value).text(item.Name));
                    else if (entityID == item.RefValue)
                        l.append($("<option />").val(item.Value).text(item.Name));
                    else if (item.Value == "")
                        l.append($("<option />").val(item.Value).text(item.Name));
                    else if (entityID != null && entityID != "" && isNaN(entityID) == true) {
                        l.append($("<option />").val(item.Value).text(item.Name));
                    }
                    else if (methodName.toLowerCase() == "getphoneencounterduration") {
                        if (entityID != null && entityID != "" && isNaN(entityID) == false) {
                            l.append($("<option />").val(item.Value).text(item.Name));
                        }
                    }
                });
            }
            BackgroundLoaderShow(false);
        });

    },

    BindDropDownsByID: function (Crtl, methodName, reload, Id, customAttr, selectFirstOption) {
        BackgroundLoaderShow(true);
        //var data = "entityID=" + entityID + "&ID=" + Id
        var dfd = new $.Deferred();
        var data = "ID=" + Id
        var l = "";
        return MDVisionService.lookups(methodName, reload, data).done(function (result) {
            if ($(Crtl).length > 0)
                l = $(Crtl);

            l.empty();
            if (!$.isArray(result) && result[methodName] != null) {
                result = JSON.parse(result[methodName]);

            } else if ($.type(result) == "string") {
                result = JSON.parse(result);
            }
            $.each(result, function (j, item) {
                if (customAttr != null) {
                    var arrcustomAttr = customAttr.split(",");
                    var option = $("<option />");

                    if (arrcustomAttr[0]) {
                        option.attr(arrcustomAttr[0], item.RefValue);
                    }

                    if (arrcustomAttr[1]) {
                        option.attr(arrcustomAttr[1], item.RefName);
                    }

                    option.val(item.Value).text(item.Name);

                    l.append(option);
                }
                else
                    l.append($("<option />").val(item.Value).text(item.Name));

            });

            if (selectFirstOption != null) {
                if (selectFirstOption == true) {
                    l.find("option:eq(1)").attr("selected", "selected").trigger("change");
                }
            }




            BackgroundLoaderShow(false);
            dfd.resolve();
        });
        return dfd;
    },


    BindDropDownsByTwoIDs: function (Crtl, methodName, reload, Id1, Id2) {

        BackgroundLoaderShow(true);
        //var data = "entityID=" + entityID + "&ID=" + Id

        var data = "ID=" + Id1 + "&ID2=" + Id2
        return MDVisionService.lookups(methodName, reload, data).done(function (result) {
            if ($(Crtl).length > 0)
                l = $(Crtl);

            l.empty();
            if (!$.isArray(result) && result[methodName] != null) {
                result = JSON.parse(result[methodName]);

            } else if ($.type(result) == "string") {
                result = JSON.parse(result);
            }
            $.each(result, function (j, item) {
                if (methodName.toLowerCase() == "getledgeraccount") {

                    if (item.RefValue != null && item.RefValue.toLowerCase() == "true") {
                        l.append($("<option />").val(item.Value).text(item.Name).attr("RefValue", item.RefValue).prop('selected', true));
                    } else {
                        l.append($("<option />").val(item.Value).text(item.Name).attr("RefValue", item.RefValue));
                    }

                }

                else if (methodName.toLowerCase() == "getledgeraccountforpatient") {
                    if (item.RefValue != null && item.RefValue.toLowerCase() == "true") {
                        l.append($("<option />").val(item.Value).text(item.Name).attr("RefValue", item.RefValue).prop('selected', true));
                    } else {
                        l.append($("<option />").val(item.Value).text(item.Name).attr("RefValue", item.RefValue));
                    }
                }
                else {
                    l.append($("<option />").val(item.Value).text(item.Name));
                }
                $(l).trigger('change');
            });


            BackgroundLoaderShow(false);
        });

    },


    BindDropDownsByMultipleIDs: function (Crtl, methodName, reload) {
        BackgroundLoaderShow(true);
        //var data = "entityID=" + entityID + "&ID=" + Id

        //var data = "ID=" + Id1 + "&ID2=" + Id2 + "&ID3=" + Id3

        var data = "ID=" + arguments[3];
        for (var i = 4; i < arguments.length; i++) {
            data += "&ID" + parseInt(i - 2) + "=" + arguments[i];
        }

        return MDVisionService.lookups(methodName, reload, data).done(function (result) {
            if ($(Crtl).length > 0)
                l = $(Crtl);

            l.empty();
            if (!$.isArray(result) && result[methodName] != null) {
                result = JSON.parse(result[methodName]);

            } else if ($.type(result) == "string") {
                result = JSON.parse(result);
            }
            $.each(result, function (j, item) {
                l.append($("<option />").val(item.Value).text(item.Name));

            });

            BackgroundLoaderShow(false);
        });

    },


    BindAutoCompleteText: function (Crtl, methodName, reload, hiddenCrtl, entityID, isDescription) {
        BackgroundLoaderShow(false);
        var AllData = [];
        var text = $(Crtl).val();
        if (text.length >= 2) {

            BackgroundLoaderShow(true);
            var data = "text=" + text + "&entityID=" + entityID
            // serach parameter , class name, command name of class 
            MDVisionService.lookups(methodName, reload, data).done(function (result) {
                // if (result.status != false) {
                AllData = [];
                if (!$.isArray(result) && result[methodName] != null) {
                    result = JSON.parse(result[methodName]);

                } else if ($.type(result) == "string") {
                    result = JSON.parse(result);
                }
                $.each(result, function (j, item) {
                    if (item.Name.toUpperCase() != "- SELECT -")
                        AllData.push({ id: item.Value, value: item.Name, RefValue: item.RefValue });
                });

                BackgroundLoaderShow(false);
                // }

            });
        }
        $(Crtl).autocomplete({
            autoFocus: true,
            //source: AllCode, // pass an array (without a comma)
            source: AllData,

            select: function (event, ui) {
                BackgroundLoaderShow(false);
                // add the selected id
                //$(Crtl).
                setTimeout(function () {
                    $(Crtl).val(ui.item.RefValue);

                    if (typeof hiddenCrtl != "undefined")
                        if (hiddenCrtl != null) {
                            if (isDescription) {
                                setTimeout(function () { $(hiddenCrtl).val(ui.item.value).focus(); }, 100);
                            }
                            else {
                                setTimeout(function () { $(hiddenCrtl).val(ui.item.id).focus(); }, 100);
                            }
                        }



                }, 10);
                //$(Crtl).text(ui.item.RefValue);



            }
        });





    },
    BindMenuByEntityID: function (Crtl, methodName, reload, entityID) {
        BackgroundLoaderShow(true);
        var data = "entityID=" + entityID


        MDVisionService.lookups(methodName, reload, data).done(function (result) {
            if ($(Crtl).length > 0)
                l = $(Crtl);

            l.empty();

            if (!$.isArray(result) && result[methodName] != null) {
                result = JSON.parse(result[methodName]);

            } else if ($.type(result) == "string") {
                result = JSON.parse(result);
            }

            $.each(result, function (j, item) {
                if (entityID == null)
                    l.append($("<option />").val(item.Value).text(item.Name));
                else if (entityID == item.RefValue)
                    l.append($("<option />").val(item.Value).text(item.Name));
                else if (item.Value == "")
                    l.append($("<option />").val(item.Value).text(item.Name));
            });





            BackgroundLoaderShow(false);
        });

    },

    BindListCodes: function (Crtl, methodName, reload, PatientId, Type) {
        if (!Type) {
            Type = "1";
        }
        BackgroundLoaderShow(true);
        //var data = "entityID=" + entityID + "&ID=" + Id

        //var data = "ID=" + Id
        if (PatientId != "" && PatientId != 'undefined' && PatientId != "-1") {
            var data = "StrID=" + PatientId + "&StrID2=" + Type;
        }
        else {
            var data = "StrID=0&StrID2=" + Type;
        }
        return MDVisionService.lookups(methodName, reload, data).done(function (result) {
            if ($(Crtl).length > 0)
                l = $(Crtl);

            l.empty();
            if (!$.isArray(result) && result[methodName] != null) {
                result = JSON.parse(result[methodName]);

            } else if ($.type(result) == "string") {
                result = JSON.parse(result);
            }
            $.each(result, function (j, item) {
                //l.append($("<li />").val(item.Value).text(item.Name == '- SELECT -' ? 'All' : item.Name));
                item.Value = item.Value == "" ? 0 : item.Value;
                l.append('<li id="FolderId_"' + item.Value + ' onclick="ListItemClick(this,' + item.Value + ');" value=' + item.Value + '><a href="#"> ' + (item.Name == '- SELECT -' ? 'All' : item.Name) + ' </a></li>');
            });

            BackgroundLoaderShow(false);
        });

    },

    BindListCodes_Search: function (Crtl, methodName, reload, PatientId, Type) {
        if (!Type) {
            Type = "1";
        }
        BackgroundLoaderShow(true);
        //var data = "entityID=" + entityID + "&ID=" + Id

        //var data = "ID=" + Id
        if (PatientId != "" && PatientId != 'undefined' && PatientId != "-1") {
            var data = "StrID=" + PatientId + "&StrID2=" + Type;
        }
        else {
            var data = "StrID=0&StrID2=" + Type;
        }
        return MDVisionService.lookups(methodName, reload, data).done(function (result) {
            if ($(Crtl).length > 0)
                l = $(Crtl);

            l.empty();
            if (!$.isArray(result) && result[methodName] != null) {
                result = JSON.parse(result[methodName]);

            } else if ($.type(result) == "string") {
                result = JSON.parse(result);
            }
            $.each(result, function (j, item) {
                //l.append($("<li />").val(item.Value).text(item.Name == '- SELECT -' ? 'All' : item.Name));
                item.Value = item.Value == "" ? 0 : item.Value;
                l.append('<li onclick="ListItemClick_Search(this,' + item.Value + ');" value=' + item.Value + '><a href="#"> ' + (item.Name == '- SELECT -' ? 'All' : item.Name) + ' </a></li>');
            });

            BackgroundLoaderShow(false);
        });

    },

    BindPatientData: function (methodName, reload, PatientId, IsActive) {
        BackgroundLoaderShow(true);
        var data = "ID=" + PatientId;
        if (IsActive)
            data += "&IsActive=" + IsActive;

        //var data = "text=" + Text + "&ID=" + PatientId;
        // serach parameter , class name, command name of class 
        return MDVisionService.lookups(methodName, reload, data).done(function (result) {
            // if (result.status != false) {
            PatientCase = [];
            if (!$.isArray(result) && result[methodName] != null) {
                result = JSON.parse(result[methodName]);

            } else if ($.type(result) == "string") {
                result = JSON.parse(result);
            }
            $.each(result, function (j, item) {
                if (item.Name.toUpperCase() != "- SELECT -")
                    PatientCase.push({ id: item.Value, value: item.Name, RefValue: item.RefValue, RefName: item.RefName });
            });

            //response(AllData);
            //return AllData;
            BackgroundLoaderShow(false);
            // }

        });
    },

    BindAppointmentStatusWorkFlow: function (AppointmentStatusId, AppointmentStatus) {

        var dfd = new $.Deferred();
        var optionsList = $.grep(AppointmentStatusWorkFlow, function (item) {
            return item.AppointmentStatusId == AppointmentStatusId;

        });

        if (optionsList.length > 0) {
            dfd.resolve(optionsList[0].List);
        }
        else {

            var objData = new Object();
            objData["CommandType"] = "load_appointment_status";
            objData["AppointmentStatusId"] = AppointmentStatusId;
            objData["AppointmentStatus"] = AppointmentStatus;
            var data = JSON.stringify(objData);

            MDVisionService.PMSAPIService(data, "Scheduler", "PMSScheduler").done(function (response) {
                if (response.status != false) {
                    var optionsList = JSON.parse(response.AppointmentStatusOptions_JSON);
                    AppointmentStatusWorkFlow.push({ AppointmentStatusId: AppointmentStatusId, List: optionsList });
                    dfd.resolve(optionsList);
                }
            });
        }

        return dfd;

    },

    BindIncomingReferralsStatusReason: function (methodName, reload, StatusId) {
        BackgroundLoaderShow(true);
        var data = "StatusId=" + StatusId;

        // serach parameter , class name, command name of class 
        return MDVisionService.lookups(methodName, reload, data).done(function (result) {
            IncomingReferralsStatusReasons = [];
            if (!$.isArray(result) && result[methodName] != null) {
                result = JSON.parse(result[methodName]);

            }
            else if ($.type(result) == "string") {
                result = JSON.parse(result);
            }
            $.each(result, function (j, item) {
                if (item.Name.toUpperCase() != "- SELECT -")
                    IncomingReferralsStatusReasons.push({ id: item.Value, value: item.Name, RefValue: item.RefValue, RefName: item.RefName });
            });

            BackgroundLoaderShow(false);
        });
    },
}
